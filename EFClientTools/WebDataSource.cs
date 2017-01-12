using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.Design;
using System.ComponentModel.Design;
using EFClientTools;
using System.Data;
using System.IO;
using System.Collections;
using System.Xml.Serialization;
using System.Xml;

namespace EFClientTools
{
    [Designer(typeof(DataSourceDesigner), typeof(IDesigner))]
    public class WebDataSource : DataSourceControl, IListSource
    {
        public WebDataSource()
        {
            PacketRecords = 100;
            AutoApply = true;
            WhereParams = new List<EFServerReference.WhereParameter>();
        }

        protected override ICollection GetViewNames()
        {
            var dataSet = InnerDataSet;
            if (dataSet != null)
            {
                return dataSet.Tables.OfType<DataTable>().Select(c => c.TableName).ToList();
            }
            return new List<string>();
        }

        protected override DataSourceView GetView(string viewName)
        {
            if (string.IsNullOrEmpty(viewName))
            {
                viewName = DataMember;
            }
            return new WebDataSourceView(this, viewName);
        }

        public WebDataSourceView GainView(string viewName)
        {
            if (string.IsNullOrEmpty(viewName))
            {
                viewName = DataMember;
            }
            return new WebDataSourceView(this, viewName);
        }

        public string RemoteName { get; set; }
        public string DBAlias { get; set; }
        public string SelectCommand { get; set; }

        private string dataMember = string.Empty;
        public string DataMember
        {
            get
            {
                return dataMember;
            }
            set
            {
                dataMember = value;
            }
        }

        public string MasterDataSourceID { get; set; }

        public bool AutoApply { get; set; }

        [Browsable(false)]
        public DataSet InnerDataSet
        {
            get
            {
                if (IsMaster)
                {
                    var dataset = (DataSet)ViewState["InnerDataSet"];
                    if (dataset == null)
                    {
                        //初始化
                        dataset = GetDataSet();
                        InnerDataSet = dataset;
                        return dataset;
                    }
                    return dataset;
                }
                else
                {
                    return MasterWebDataSource.InnerDataSet;
                }
            }
            set
            {
                if (IsMaster)
                {
                    ViewState["InnerDataSet"] = value;
                }
            }
        }

        private DataSet GetDataSet()
        {
            var packetInfo = new EFClientTools.EFServerReference.PacketInfo()
            {
                StartIndex = LastIndex,
                Count = PacketRecords,
                OrderParameters = new List<EFServerReference.OrderParameter>(),
                WhereParameters = WhereParams,
                WhereString = Where
            };

            var clientInfo = EFClientTools.ClientUtility.ClientInfo;
            //clientInfo.IsSDModule = true;
            clientInfo.UseDataSet = true;
            if (string.IsNullOrEmpty(clientInfo.Solution))
            {
                clientInfo.Solution = PreviewSolution;
                clientInfo.Database = PreviewDatabase;
            }

            var dataSet = new DataSet();
            string xml = "";
            if (String.IsNullOrEmpty(SelectCommand))
            {
                var assemblyName = RemoteName.Split('.')[0];
                var commandName = RemoteName.Split('.')[1];
                xml = (string)EFClientTools.ClientUtility.Client.GetDataSet(clientInfo, assemblyName, commandName, packetInfo);
            }
            else
            {
                xml = (string)EFClientTools.ClientUtility.Client.ExecuteSQL(clientInfo, DBAlias, SelectCommand, packetInfo);
            }
            StringReader reader = new StringReader(xml);
            dataSet.ReadXml(reader, XmlReadMode.Auto);
            var count = dataSet.Tables[0].Rows.Count;
            LastIndex += count;
            return dataSet;
        }

        [Browsable(false)]
        public bool IsMaster
        {
            get
            {
                return string.IsNullOrEmpty(MasterDataSourceID);
            }
        }

        /// <summary>
        /// 取得当前WebDataSource的Parent的WebDataSource(父级)
        /// </summary>
        [Browsable(false)]
        public WebDataSource ParentWebDataSource
        {
            get
            {
                if (IsMaster)
                {
                    return null;
                }
                else if (this.MasterDataSourceID == this.ID)
                {
                    //MasterDataSource不能是自己本身,会造成死循环
                    throw new Exception("MasterDataSource can not be self.");
                }
                Control parent = this.Parent.FindControl(this.MasterDataSourceID);
                if (parent != null && parent is WebDataSource)
                {
                    return parent as WebDataSource;
                }
                else
                {
                    //在Page上找不到MasterDataSrouce
                    throw new Exception(string.Format("can not find datasource:'{0}'", MasterDataSourceID));
                }
            }
        }

        /// <summary>
        /// 取得当前WebDataSource的主Master的WebDataSource(最顶级)
        /// </summary>
        [Browsable(false)]
        public WebDataSource MasterWebDataSource
        {
            get
            {
                WebDataSource master = GetMasterDataSource(this);
                return master;
            }
        }

        private WebDataSource GetMasterDataSource(WebDataSource owner)
        {
            if (owner.IsMaster)//当前就是Master
            {
                return owner;
            }
            else
            {
                if (owner.MasterDataSourceID == owner.ID)
                {
                    //MasterDataSource不能是自己本身,会造成死循环
                    throw new Exception("MasterDataSource can not be self.");
                }
                Control parent = owner.Parent.FindControl(owner.MasterDataSourceID);
                if (parent != null && parent is WebDataSource)
                {
                    return GetMasterDataSource(parent as WebDataSource);//递归直到最顶层
                }
                else
                {
                    throw new Exception(string.Format("can not find datasource:'{0}'", MasterDataSourceID));
                }
            }
        }

        [Obsolete()]
        public object CurrentRowKeyFieldValue { get; set; }

        public int PacketRecords { get; set; }

        [Browsable(false)]
        public int LastIndex
        {
            get
            {
                return ViewState["LastIndex"] == null ? 0 : (int)ViewState["LastIndex"];
            }
            set
            {
                ViewState["LastIndex"] = value;
            }
        }

        [Browsable(false)]
        public ArrayList Keys
        {
            get
            {
                return ViewState["Keys"] == null ? new ArrayList() : (ArrayList)ViewState["Keys"];
            }
            set
            {
                ViewState["Keys"] = value;
            }
        }

        /// <summary>
        /// 当前选中的值
        /// </summary>
        [Browsable(false)]
        public ArrayList CurrentValues
        {
            get
            {
                return ViewState["CurrentValues"] == null ? new ArrayList() : (ArrayList)ViewState["CurrentValues"];
            }
        }

        public void SetCurrentRow(object[] values)
        {
            if (values != null)
            {
                ArrayList list = new ArrayList();
                list.AddRange(values);
                ViewState["CurrentValues"] = list;
            }
        }


        /// <summary>
        /// 取得当前WebDataSource的视图
        /// </summary>
        [Browsable(false)]
        public DataView View
        {
            get
            {
                DataView view = (this.GetView(this.DataMember) as WebDataSourceView).View;
                return view;
            }
        }

        [Browsable(false)]
        public DataRow CurrentRow
        {
            get
            {
                if (CurrentValues.Count > 0)
                {
                    DataView view = View;
                    return view.Table.Rows.Find(CurrentValues.ToArray());
                }
                else
                {
                    return null;
                }
            }
        }

        private Hashtable relationValues;
        /// <summary>
        /// 取得Relation关联的栏位值
        /// </summary>
        [Browsable(false)]
        public Hashtable RelationValues
        {
            get
            {

                if (relationValues == null)
                {
                    relationValues = new Hashtable();
                    if (!IsMaster)//不是master
                    {
                        WebDataSource master = MasterWebDataSource;
                        DataTable table = null;
                        if (string.IsNullOrEmpty(DataMember))
                        {
                            table = master.InnerDataSet.Tables[0];
                        }
                        else
                        {
                            table = master.InnerDataSet.Tables[DataMember];
                        }
                        WebDataSource parent = ParentWebDataSource;

                        //直接用CurrentRow属性
                        if (table == null)
                        {
                            throw new Exception(string.Format("Table:'{0}' not found.", this.DataMember));
                        }
                        if (table.ParentRelations.Count == 0)
                        {
                            throw new Exception("Relation not match.");
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(parent.DataMember) && table.ParentRelations[0].ParentTable.TableName != master.InnerDataSet.Tables[0].TableName)
                            {
                                throw new Exception("Relation not match.");
                            }
                            else if (!string.IsNullOrEmpty(parent.DataMember) && table.ParentRelations[0].ParentTable.TableName != parent.DataMember)
                            {
                                throw new Exception("Relation not match.");
                            }
                            DataRow parentRow = parent.CurrentRow;
                            if (parentRow != null)
                            {
                                DataRelation parentRelation = table.ParentRelations[0];
                                for (int i = 0; i < parentRelation.ParentColumns.Length; i++)
                                {
                                    relationValues.Add(parentRelation.ChildColumns[i].ColumnName, parentRow[parentRelation.ParentColumns[i].ColumnName]);
                                }
                            }
                        }
                    }
                }
                return relationValues;
            }
        }

        [Browsable(false)]
        public string Where { get; set; }

        #region SDModule preview
        [Browsable(false)]
        public string PreviewSolution { get; set; }
        [Browsable(false)]
        public string PreviewDatabase { get; set; }
        #endregion

        public void SetWhere(string where)
        {
            Where = where;
            LastIndex = 0;
            InnerDataSet = null;
        }

        [Browsable(false)]
        public List<EFServerReference.WhereParameter> WhereParams { get; set; }
        //public void SetWhere(List<EFServerReference.WhereParameter> where)
        //{
        //    WhereParams = where;
        //    LastIndex = 0;
        //    InnerDataSet = null;
        //}


        public void SetWhere(List<string> columns, List<string> conditions, List<object> values)
        {
            WhereParams.Clear();
            for (int i = 0; i < columns.Count; i++)
            {
                EFServerReference.WhereParameter wp = new EFServerReference.WhereParameter();
                wp.Field = columns[i];
                wp.Condition = EFServerReference.WhereCondition.BeginWith;
                wp.Value = values[i];
                WhereParams.Add(wp);
            }
            LastIndex = 0;
            InnerDataSet = GetDataSet();
        }

        public bool GetNextPacket()
        {
            if (IsMaster)
            {
                var dataset = GetDataSet();
                if (dataset.Tables[0].Rows.Count > 0)
                {
                    var innerDataSet = InnerDataSet;
                    innerDataSet.Merge(dataset);
                    InnerDataSet = innerDataSet;
                    return true;
                }
            }
            return false;
        }

        public bool ApplyUpdates()
        {
            var assemblyName = RemoteName.Split('.')[0];
            var commandName = RemoteName.Split('.')[1];
            var clientInfo = EFClientTools.ClientUtility.ClientInfo;
            clientInfo.IsSDModule = true;
            clientInfo.UseDataSet = true;
            if (string.IsNullOrEmpty(clientInfo.Solution))
            {
                clientInfo.Solution = PreviewSolution;
                clientInfo.Database = PreviewDatabase;
            }

            var dataSet = InnerDataSet;

            //? getchange?
            //dataSet = dataSet.GetChanges();

            var xml = string.Empty;
            XmlSerializer serializer = new XmlSerializer(typeof(DataSet));
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = new UnicodeEncoding(false, false);
            settings.Indent = false;
            settings.OmitXmlDeclaration = false;
            using (StringWriter textWriter = new StringWriter())
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(textWriter, settings))
                {
                    serializer.Serialize(xmlWriter, dataSet);
                }
                xml = textWriter.ToString();
            }

            EFClientTools.ClientUtility.Client.UpdateDataSet(clientInfo, assemblyName, commandName, xml);

            return true;

        }

        public int GetRecordsCount()
        {
            if (String.IsNullOrEmpty(RemoteName))
                return 0;

            int i = 0;
            int iPos = RemoteName.IndexOf('.');
            if (iPos > 0)
            {
                var packetInfo = new EFClientTools.EFServerReference.PacketInfo()
                {
                    StartIndex = LastIndex,
                    Count = PacketRecords,
                    OrderParameters = new List<EFServerReference.OrderParameter>(),
                    WhereParameters = WhereParams,
                    WhereString = Where
                };
                string assemblyName = RemoteName.Substring(0, iPos);
                string commandName = RemoteName.Substring(iPos + 1);
                var clientInfo = EFClientTools.ClientUtility.ClientInfo;
                clientInfo.IsSDModule = true;
                clientInfo.UseDataSet = true;
                if (string.IsNullOrEmpty(clientInfo.Solution))
                {
                    clientInfo.Solution = PreviewSolution;
                    clientInfo.Database = PreviewDatabase;
                }
                i = EFClientTools.ClientUtility.Client.GetDataCount(clientInfo, assemblyName, commandName, packetInfo);
                //i = CliUtils.GetRecordsCount(sModule, sDataSet, strWhere, CliUtils.fCurrentProject);
            }

            return i;
            //return InnerDataSet.Tables[0].Rows.Count;
        }
    }

    public class WebDataSourceView : DataSourceView
    {
        private WebDataSource _owner;
        private string _viewName;

        public WebDataSourceView(WebDataSource owner, string viewName)
            : base(owner, viewName)
        {
            _owner = owner;
            _viewName = viewName;
        }

        internal DataView View
        {
            get
            {
                WebDataSource dataSource = (WebDataSource)_owner;
                if (dataSource == null)
                {
                    return null;
                }

                else if (dataSource.IsMaster)
                {
                    DataTable table = null;
                    if (string.IsNullOrEmpty(_viewName))
                    {
                        table = dataSource.InnerDataSet.Tables[0];
                    }
                    else
                    {
                        table = dataSource.InnerDataSet.Tables[_viewName];
                    }
                    if (table == null)
                    {
                        throw new Exception(string.Format("Table:'{0}' not found.", _viewName));
                    }
                    return table.DefaultView;
                }
                else
                {
                    WebDataSource master = dataSource.MasterWebDataSource;

                    WebDataSource parent = dataSource.ParentWebDataSource;
                    DataTable table = master.InnerDataSet.Tables[_viewName];
                    if (table == null)
                    {
                        throw new Exception(string.Format("Table:'{0}' not found.", _viewName));
                    }
                    if (table.ParentRelations.Count == 0)
                    {
                        throw new Exception("Relation not match.");
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(parent.DataMember) && table.ParentRelations[0].ParentTable.TableName != master.InnerDataSet.Tables[0].TableName)
                        {
                            throw new Exception("Relation not match.");
                        }
                        else if (!string.IsNullOrEmpty(parent.DataMember) && table.ParentRelations[0].ParentTable.TableName != parent.DataMember)
                        {
                            throw new Exception("Relation not match.");
                        }

                        DataRow parentRow = parent.CurrentRow;
                        DataRelation parentRelation = table.ParentRelations[0];
                        if (parentRow != null && parentRow.Table != table.ParentRelations[0].ParentTable)//找到DataTable中实际的那行
                        {
                            if (parentRow.Table.PrimaryKey.Length > 0)
                            {
                                object[] value = new object[parentRow.Table.PrimaryKey.Length];
                                for (int i = 0; i < parentRow.Table.PrimaryKey.Length; i++)
                                {
                                    value[i] = parentRow[parentRow.Table.PrimaryKey[i].ColumnName];
                                }
                                parentRow = table.ParentRelations[0].ParentTable.Rows.Find(value);
                            }
                            else
                            {
                                throw new Exception(string.Format("DataSource '{0}' not set primary keys.", dataSource.MasterDataSourceID));
                            }
                        }

                        table = table.Clone();
                        if (parentRow != null)//如果parent有选中某一行
                        {
                            DataRow[] childRows = parentRow.GetChildRows(parentRelation);
                            for (int i = 0; i < childRows.Length; i++)
                            {
                                table.Rows.Add(childRows[i].ItemArray);
                            }
                        }
                        return table.DefaultView;
                    }
                }
            }
        }

        public override bool CanSort
        {
            get
            {
                return true;
            }
        }

        protected override IEnumerable ExecuteSelect(DataSourceSelectArguments selectArgs)
        {
            if (CanSort)
            {
                selectArgs.AddSupportedCapabilities(DataSourceCapabilities.Sort);
            }
            DataView view = View;
            if (view != null && CanSort && !string.IsNullOrEmpty(selectArgs.SortExpression))
            {
                view.Sort = selectArgs.SortExpression;
            }
            return view;
        }

        public override bool CanDelete
        {
            get
            {
                return true;
            }
        }

        protected override int ExecuteDelete(IDictionary keys, IDictionary values)
        {
            WebDataSource dataSource = (WebDataSource)_owner;
            if (dataSource == null)
            {
                return 0;
            }
            WebDataSource master = dataSource.MasterWebDataSource;

            DataTable table = null;
            if (string.IsNullOrEmpty(_viewName))
            {
                table = master.InnerDataSet.Tables[0];
            }
            else
            {
                table = master.InnerDataSet.Tables[_viewName];
            }
            if (table == null)
            {
                throw new Exception(string.Format("Table:'{0}' not found.", _viewName));
            }

            DataRow rowDelete = null;

            if (table.PrimaryKey.Length > 0)
            {
                object[] keysDelete = new object[table.PrimaryKey.Length];
                for (int i = 0; i < table.PrimaryKey.Length; i++)
                {
                    string columnName = table.PrimaryKey[i].ColumnName;
                    if (values.Contains(columnName))
                    {
                        keysDelete[i] = values[columnName];
                    }
                    else if (dataSource.RelationValues != null && dataSource.RelationValues.Contains(columnName))
                    {
                        keysDelete[i] = dataSource.RelationValues[columnName];
                    }
                    else
                    {
                        throw new Exception(string.Format("Value of column '{0}' not found.", columnName));
                    }
                }
                rowDelete = table.Rows.Find(keysDelete);
            }
            if (rowDelete != null)
            {
                rowDelete.Delete();
            }
            if (master.AutoApply)
            {
                master.ApplyUpdates();
            }
            return 1;

        }

        public override bool CanInsert
        {
            get
            {
                return true;
            }
        }

        protected override int ExecuteInsert(IDictionary values)
        {
            WebDataSource dataSource = (WebDataSource)_owner;
            if (dataSource == null)
            {
                return 0;
            }
            WebDataSource master = dataSource.MasterWebDataSource;

            DataTable table = null;
            if (string.IsNullOrEmpty(_viewName))
            {
                table = master.InnerDataSet.Tables[0];
            }
            else
            {
                table = master.InnerDataSet.Tables[_viewName];
            }
            if (table == null)
            {
                throw new Exception(string.Format("Table:'{0}' not found.", _viewName));
            }

            DataRow rowInsert = table.NewRow();
            for (int i = 0; i < table.Columns.Count; i++)
            {
                if (values.Contains(table.Columns[i].ColumnName))
                {
                    if (values[table.Columns[i].ColumnName] != null && values[table.Columns[i].ColumnName].ToString().Length > 0)
                    {
                        rowInsert[i] = values[table.Columns[i].ColumnName];
                    }
                }
                else if (dataSource.RelationValues != null && dataSource.RelationValues.Contains(table.Columns[i].ColumnName))
                {
                    rowInsert[i] = dataSource.RelationValues[table.Columns[i].ColumnName];
                }
            }

            table.Rows.Add(rowInsert);
            if (master.AutoApply)
            {
                master.ApplyUpdates();
            }
            return 1;
        }


        public override bool CanUpdate
        {
            get
            {
                return true;
            }
        }

        protected override int ExecuteUpdate(IDictionary keys, IDictionary values, IDictionary oldValues)
        {
            WebDataSource dataSource = (WebDataSource)_owner;
            if (dataSource == null)
            {
                return 0;
            }
            WebDataSource master = dataSource.MasterWebDataSource;

            DataTable table = null;
            if (string.IsNullOrEmpty(_viewName))
            {
                table = master.InnerDataSet.Tables[0];
            }
            else
            {
                table = master.InnerDataSet.Tables[_viewName];
            }
            if (table == null)
            {
                throw new Exception(string.Format("Table:'{0}' not found.", _viewName));
            }

            DataRow rowUpdate = null;

            if (table.PrimaryKey.Length > 0)
            {
                object[] keysUpdate = new object[table.PrimaryKey.Length];
                for (int i = 0; i < table.PrimaryKey.Length; i++)
                {
                    string columnName = table.PrimaryKey[i].ColumnName;
                    if (oldValues.Contains(columnName))
                    {
                        keysUpdate[i] = oldValues[columnName];
                    }
                    else if (dataSource.RelationValues != null && dataSource.RelationValues.Contains(columnName))
                    {
                        keysUpdate[i] = dataSource.RelationValues[columnName];
                    }
                    else
                    {
                        throw new Exception(string.Format("Value of column '{0}' not found.", columnName));
                    }
                }
                rowUpdate = table.Rows.Find(keysUpdate);
            }
            if (rowUpdate == null)
            {
                return 0;
            }
            for (int i = 0; i < table.Columns.Count; i++)
            {
                if (values.Contains(table.Columns[i].ColumnName))
                {
                    if (values[table.Columns[i].ColumnName] != null && values[table.Columns[i].ColumnName].ToString().Length > 0)
                    {
                        rowUpdate[i] = values[table.Columns[i].ColumnName];
                    }
                    else
                    {
                        rowUpdate[i] = DBNull.Value;
                    }
                }
            }

            if (master.AutoApply)
            {
                master.ApplyUpdates();
            }
            return 1;
        }

    }


    public enum CacheMode
    {
        ViewState,
        Session
    }
}
