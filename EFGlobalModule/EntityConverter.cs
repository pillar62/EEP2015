using System;
using System.Net;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Data.Objects.DataClasses;
using System.Data;

namespace EFGlobalModule
{
    public class EntityConverter
    {
        public string AssemblyName { get; set; }

        public IEnumerable<EntityObject> FromDataSet(DataSet dataset) {
            var entityObjects = new List<EntityObject>();
            if (dataset.Tables.Count > 0) {
                var table = dataset.Tables[0];
                var tableName = table.TableName;
                var entityType = GetEntityType(tableName);

                foreach (DataRow row in table.Rows) {
                    var entityobject = CreateObject(row, entityType, dataset);
                    entityObjects.Add(entityobject);
                }
            }
            return entityObjects;
        }

        private EntityObject CreateObject(DataRow row, Type entityType, DataSet dataset) {
            var entityObject = (EntityObject)entityType.GetConstructor(new Type[] { }).Invoke(null);
            foreach (DataColumn column in row.Table.Columns) {
                var columnName = column.ColumnName.ToUpper(); // Informix在创建表的时候会自动把Column的名字改成小写，这里就取不到属性了。
                var property = entityType.GetProperty(columnName);
                if (property != null && row[columnName] != DBNull.Value){
                    entityObject.SetValue(columnName, row[columnName]);
                }
            }
            var entityKey = new EntityKey();
            entityKey.EntityContainerName = AssemblyName;
            entityKey.EntitySetName = row.Table.TableName;
            entityKey.EntityKeyValues = new EntityKeyMember[row.Table.PrimaryKey.Length];
            for (int i = 0; i < row.Table.PrimaryKey.Length; i++) {
                var primaryKey = row.Table.PrimaryKey[i];
                entityKey.EntityKeyValues[i] = new EntityKeyMember() { Key = primaryKey.ColumnName, Value = row[primaryKey.ColumnName] };
            }
            entityObject.EntityKey = entityKey;

            foreach (DataRelation relation in dataset.Relations) {
                if (relation.ParentTable.Equals(row.Table)) {
                    var navigationProperty = entityType.GetProperty(relation.ChildTable.TableName);
                    if (navigationProperty != null) {
                        var list = (IList)navigationProperty.PropertyType.GetConstructor(new Type[] { }).Invoke(null);
                        var childEntityType = GetEntityType(relation.ChildTable.TableName);
                        var childRows = row.GetChildRows(relation);
                        foreach (var childRow in childRows) {
                            var childObject = CreateObject(childRow, childEntityType, dataset);
                            var childNavigationProperty = childEntityType.GetProperty(relation.ParentTable.TableName);
                            if (childNavigationProperty != null)
                            {
                                childNavigationProperty.SetValue(childObject, entityObject, null);
                            }
                            list.Add(childObject);
                        }
                        navigationProperty.SetValue(entityObject, list, null);
                    }
                }
            }
            return entityObject;
        }

        const string PROJECT_NAME = "EFGlobalModule";

        private Type GetEntityType(string typeName) {
            var assembly = this.GetType().Assembly;
            if (string.IsNullOrEmpty(AssemblyName)) {
                return assembly.GetType(string.Format("{0}.{1}", PROJECT_NAME, typeName), true);
            }
            else {
                return assembly.GetType(string.Format("{0}.{1}.{2}", PROJECT_NAME, AssemblyName, typeName), true);
            }
        }

        public DataSet ToDataSet(IEnumerable<EntityObject> entityObjects, Dictionary<EntityKey, EntityState> states, DataSet dataset) {
            foreach (var entityObject in entityObjects) {
                CreateRow(entityObject, null, states, dataset);    
            }
            return dataset;
        }   

        private void CreateRow(EntityObject entityObject, DataRow parentRow, Dictionary<EntityKey, EntityState> states, DataSet dataset) {

            var tableName = entityObject.GetType().Name;
            var dataTable = dataset.Tables[tableName];
            var state = GetState(entityObject, states,  parentRow);
            DataRow dataRow = null;

            if (state == EntityState.Added) {
                dataRow = dataTable.NewRow();
                ApplyValue(dataRow, entityObject);
                dataTable.Rows.Add(dataRow);
                ApplyRelationValue(dataRow, parentRow, dataset);
                if (parentRow != null && parentRow.RowState == DataRowState.Unchanged)
                {
                    parentRow.SetModified();
                }
            }
            else {
                dataRow = dataTable.Rows.Find(entityObject.EntityKey.EntityKeyValues.Select(c => c.Value).ToArray());
                if (dataRow != null)
                {
                    if (state == EntityState.Deleted)
                    {
                        dataRow.Delete();
                    }
                    else
                    {
                        ApplyValue(dataRow, entityObject);
                    }
                }
            }

            var navigationProperties = entityObject.GetType().GetProperties().Where(c => typeof(IList).IsAssignableFrom(c.PropertyType));
            foreach (var navigationProperty in navigationProperties) {
                var navigationValue = (IList)navigationProperty.GetValue(entityObject, null);
                foreach (EntityObject childObject in navigationValue) {
                    CreateRow(childObject, dataRow, states, dataset);
                }
            }
        }

        private void ApplyValue(DataRow dataRow, EntityObject entityObject) {
            var properties = entityObject.GetType().GetProperties()
                .Where(c => (!typeof(IList).IsAssignableFrom(c.PropertyType) && !typeof(EntityObject).IsAssignableFrom(c.PropertyType)) && !typeof(EntityKey).IsAssignableFrom(c.PropertyType));
            foreach (var property in properties) {
                if (dataRow.Table.Columns[property.Name] != null) {
                    dataRow[property.Name] = entityObject.GetValue(property.Name);
                }
            }
        }

        private void ApplyRelationValue(DataRow row, DataRow parentRow, DataSet dataset) {
            if (parentRow == null) {
                return;
            }
            var relation = dataset.Relations.OfType<DataRelation>().FirstOrDefault(c => c.ParentTable.Equals(parentRow.Table) && c.ChildTable.Equals(row.Table));
            if (relation != null) 
            {
                for (int i = 0; i < relation.ParentColumns.Length; i++) {
                    row[relation.ChildColumns[i].ColumnName] = parentRow[relation.ParentColumns[i].ColumnName];
                }
            }
        }

        private EntityState GetState(EntityObject entityObject, Dictionary<EntityKey, EntityState> states, DataRow parentRow) {
            if (parentRow != null && parentRow.RowState == DataRowState.Deleted)
            {
                return EntityState.Deleted;
            }
            else if (entityObject.EntityKey == null)
            {
                return EntityState.Added;
            }
            else if (states.ContainsKey(entityObject.EntityKey))
            {
                return states[entityObject.EntityKey];
            }
            else
            {
                return EntityState.Unchanged;
            }
        }
    }
}
