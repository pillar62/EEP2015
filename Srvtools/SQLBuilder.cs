using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Reflection;
using System.Data.OracleClient;
using System.Data.OleDb;

namespace Srvtools
{
    public class SQLBuilder
    {
        #region Constructor

        public SQLBuilder()
        {
        }

        public SQLBuilder(UpdateComponent updateComponent, DataRow row, DataTable schema)
        {
            _updateComonent = updateComponent;
            _row = row;
            _schema = schema;
        }
        #endregion

        #region Properties

        public UpdateComponent UpdateComponent
        {
            get { return _updateComonent; }
        }

        public DataRow Row
        {
            get { return _row; }
        }

        public DataTable Schema
        {
            get { return _schema; }
        }

        public String QuotePrefix
        {
            get { return _quotePrefix; }
            set { _quotePrefix = value; }
        }

        public String QuoteSuffix
        {
            get { return _quoteSuffix; }
            set { _quoteSuffix = value; }
        }

        public Char Marker
        {
            get { return _marker; }
            set { _marker = value; }
        }

        private String TableName
        {
            get
            {
                if (_tableName.Contains("."))
                {
                    String[] str = _tableName.Split('.');
                    return Quote(str[0]) + "." + Quote(str[1]);
                }
                else
                {
                    if (_schemaName != null && _schemaName.Length > 0)
                        return Quote(_schemaName) + "." + Quote(_tableName);
                    return Quote(_tableName);
                }
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// 得到Delete的SQL语句。
        /// </summary>
        /// <returns></returns>
        public String GetDeleteSQL()
        {
            if (_schema == null)
            {
                String message = SysMsg.GetSystemMessage(((DataModule)_updateComonent.OwnerComp).Language, "Srvtools", "UpdateComponent", "msg_SchemaIsNull");
                throw new ArgumentException(String.Format(message, _updateComonent.Name));
            }

            if (_deleteSQL != null) return _deleteSQL;

            CreateTableName(); CreateSchemaName();

            String originalWherePart = CreateOriginalWherePart();
            if (originalWherePart == null || originalWherePart.Length == 0)
            { return null; }

            String deleteSQL = "delete from " + TableName + " where " + originalWherePart;
            _deleteSQL = deleteSQL;
            return _deleteSQL;
        }

        public String GetDeleteOracle()
        {
            if (_schema == null)
            {
                String message = SysMsg.GetSystemMessage(((DataModule)_updateComonent.OwnerComp).Language, "Srvtools", "UpdateComponent", "msg_SchemaIsNull");
                throw new ArgumentException(String.Format(message, _updateComonent.Name));
            }

            if (_deleteSQL != null) return _deleteSQL;

            CreateTableName(); CreateSchemaName();

            String originalWherePart = CreateOriginalOracleWherePart();
            if (originalWherePart == null || originalWherePart.Length == 0)
            { return null; }

            //String deleteSQL = "delete from " + TableName + " where " + originalWherePart;
            String deleteSQL = "delete from " + _tableName + " where " + originalWherePart;
            _deleteSQL = deleteSQL;
            return _deleteSQL;
        }

        public String GetDeleteSybase(String TN)
        {
            if (_schema == null)
            {
                String message = SysMsg.GetSystemMessage(((DataModule)_updateComonent.OwnerComp).Language, "Srvtools", "UpdateComponent", "msg_SchemaIsNull");
                throw new ArgumentException(String.Format(message, _updateComonent.Name));
            }

            if (_deleteSQL != null) return _deleteSQL;

            CreateTableName(); CreateSchemaName();

            String originalWherePart = CreateOriginalOracleWherePart();
            if (originalWherePart == null || originalWherePart.Length == 0)
            { return null; }

            String deleteSQL = String.Empty;
            if (_tableName != String.Empty)
                deleteSQL = "delete from " + _tableName + " where " + originalWherePart;
            else
                deleteSQL = "delete from " + TN + " where " + originalWherePart;
            _deleteSQL = deleteSQL;
            return _deleteSQL;
        }

        public String GetDeleteMySql()
        {
            if (_schema == null)
            {
                String message = SysMsg.GetSystemMessage(((DataModule)_updateComonent.OwnerComp).Language, "Srvtools", "UpdateComponent", "msg_SchemaIsNull");
                throw new ArgumentException(String.Format(message, _updateComonent.Name));
            }

            if (_deleteSQL != null) return _deleteSQL;

            CreateTableName(); CreateSchemaName();

            String originalWherePart = CreateOriginalMySqlWherePart();
            if (originalWherePart == null || originalWherePart.Length == 0)
            { return null; }

            //String deleteSQL = "delete from " + TableName + " where " + originalWherePart;
            String deleteSQL = "delete from " + _tableName + " where " + originalWherePart;
            _deleteSQL = deleteSQL;
            return _deleteSQL;
        }

        public String GetDeleteOdbc(string TN, OdbcDBType type)
        {
            if (_schema == null)
            {
                String message = SysMsg.GetSystemMessage(((DataModule)_updateComonent.OwnerComp).Language, "Srvtools", "UpdateComponent", "msg_SchemaIsNull");
                throw new ArgumentException(String.Format(message, _updateComonent.Name));
            }

            if (_deleteSQL != null) return _deleteSQL;

            CreateTableName(); CreateSchemaName();

            String originalWherePart = CreateOriginalOdbcWherePart(type);
            if (originalWherePart == null || originalWherePart.Length == 0)
            { return null; }

            //String deleteSQL = "delete from " + TableName + " where " + originalWherePart;
            String deleteSQL = "";
            if (_tableName != "")
                deleteSQL = "delete from " + _tableName + " where " + originalWherePart;
            else
                deleteSQL = "delete from " + TN + " where " + originalWherePart;
            _deleteSQL = deleteSQL;
            return _deleteSQL;
        }

        public String GetDeleteInformix(String TN)
        {
            if (_schema == null)
            {
                String message = SysMsg.GetSystemMessage(((DataModule)_updateComonent.OwnerComp).Language, "Srvtools", "UpdateComponent", "msg_SchemaIsNull");
                throw new ArgumentException(String.Format(message, _updateComonent.Name));
            }

            if (_deleteSQL != null) return _deleteSQL;

            CreateTableName(); CreateSchemaName();

            String originalWherePart = CreateOriginalInformixWherePart();
            if (originalWherePart == null || originalWherePart.Length == 0)
            { return null; }

            //String deleteSQL = "delete from " + TableName + " where " + originalWherePart;
            if (string.IsNullOrEmpty(_tableName))
                _deleteSQL = "delete from " + TN + " where " + originalWherePart;
            else
                _deleteSQL = "delete from " + _tableName + " where " + originalWherePart;

            return _deleteSQL;
        }

        /// <summary>
        /// 得到Insert的SQL语句。
        /// </summary>
        /// <returns></returns>
        public String GetInsertSQL()
        {
            if (_schema == null)
            {
                String message = SysMsg.GetSystemMessage(((DataModule)_updateComonent.OwnerComp).Language, "Srvtools", "UpdateComponent", "msg_SchemaIsNull");
                throw new ArgumentException(String.Format(message, _updateComonent.Name));
            }

            if (_insertSQL != null && _insertSQL.Length > 0) return _insertSQL;

            // Create the table name and schema name.
            CreateTableName(); CreateSchemaName();

            StringBuilder columnsSB = new StringBuilder();
            StringBuilder valuesSB = new StringBuilder();

            Int32 index = 0;
            var insertColumns = new List<string>();
            foreach (DataRow schemaRow in _schema.Rows)
            {
                String columnName = (String)schemaRow["ColumnName"];
                String columnType = schemaRow["DataType"].ToString();
                String columnTypeName = schemaRow["DataTypeName"].ToString();
                String baseColumnName = (String)schemaRow["BaseColumnName"];

                if (IsLeftJoinClumns(schemaRow))
                { index++; continue; }
                else
                { index++; }

                if (IsReadOnly(schemaRow)) continue;
                if (insertColumns.Contains(baseColumnName))
                {
                    continue;
                }

                FieldAttr attr = GetFieldAttrByColumnName(columnName);
                if (attr != null && !attr.UpdateEnable)
                {
                    continue;
                }

                Object columnValue = GetColumnIOrUValue(columnName, columnType);
                if (columnValue == null || columnValue == DBNull.Value)
                { continue; }

                if (columnsSB.Length > 0)
                { columnsSB.Append(", "); valuesSB.Append(", "); }

                columnsSB.Append(Quote(baseColumnName));
                valuesSB.Append(Mark(columnTypeName, columnType, TransformMarkerInColumnValue(columnType, columnValue)));
                insertColumns.Add(baseColumnName);
            }

            if (columnsSB.ToString() != null && columnsSB.ToString().Length > 0)
            {
                String insertSQL = "insert into " + TableName + " (" + columnsSB.ToString() + ") " +
                    " values (" + valuesSB.ToString() + ")";

                _insertSQL = insertSQL;
            }

            return _insertSQL;
        }

        public String GetInsertOracle(IDbCommand command)
        {
            if (_schema == null)
            {
                String message = SysMsg.GetSystemMessage(((DataModule)_updateComonent.OwnerComp).Language, "Srvtools", "UpdateComponent", "msg_SchemaIsNull");
                throw new ArgumentException(String.Format(message, _updateComonent.Name));
            }

            if (_insertSQL != null && _insertSQL.Length > 0) return _insertSQL;

            // Create the table name and schema name.
            CreateTableName(); CreateSchemaName();

            StringBuilder columnsSB = new StringBuilder();
            StringBuilder valuesSB = new StringBuilder();

            Int32 index = 0;
            foreach (DataRow schemaRow in _schema.Rows)
            {
                String columnName = (String)schemaRow["ColumnName"];
                String columnType = schemaRow["DataType"].ToString();
                //String columnTypeName = schemaRow["DataTypeName"].ToString();

                if (IsLeftJoinClumns(schemaRow))
                { index++; continue; }
                else
                { index++; }

                FieldAttr attr = GetFieldAttrByColumnName(columnName);
                if (attr != null && !attr.UpdateEnable)
                {
                    continue;
                }

                //if (IsReadOnly(schemaRow)) continue;

                Object columnValue = GetColumnIOrUValue(columnName, columnType);
                if (columnValue == null || columnValue == DBNull.Value)
                { continue; }

                if (columnsSB.Length > 0)
                { columnsSB.Append(", "); valuesSB.Append(", "); }

                if (string.Compare(columnType, "System.String") == 0)
                {
                    if (string.Compare(schemaRow["IsLong"].ToString(), bool.TrueString, true) == 0)
                    {
                        OracleParameter oParam = new OracleParameter();
                        oParam.ParameterName = ":" + columnName;
                        oParam.OracleType = OracleType.Clob;
                        oParam.Value = columnValue;
                        command.Parameters.Add(oParam);

                        columnsSB.Append(columnName);
                        valuesSB.Append(":" + columnName);

                        continue;
                    }
                    else
                    {
                        OracleParameter oParam = new OracleParameter();
                        oParam.ParameterName = ":" + columnName;
                        oParam.OracleType = OracleType.NVarChar;
                        oParam.Value = columnValue;
                        command.Parameters.Add(oParam);

                        columnsSB.Append(columnName);
                        valuesSB.Append(":" + columnName);

                        continue;
                    }
                }
                else if (string.Compare(columnType, "System.Byte[]") == 0)
                {
                    OracleParameter oParam = new OracleParameter();
                    oParam.ParameterName = ":" + columnName;
                    oParam.OracleType = OracleType.Blob;
                    oParam.Value = columnValue;
                    command.Parameters.Add(oParam);

                    columnsSB.Append(columnName);
                    valuesSB.Append(":" + columnName);

                    continue;
                }

                //columnsSB.Append(Quote(columnName));
                //valuesSB.Append(Mark(columnTypeName, columnType, TransformMarkerInColumnValue(columnType, columnValue)));
                columnsSB.Append(columnName);
                valuesSB.Append(Mark(columnType, TransformMarkerInColumnValue(columnType, columnValue)));
            }

            if (columnsSB.ToString() != null && columnsSB.ToString().Length > 0)
            {
                //String insertSQL = "insert into " + TableName + " (" + columnsSB.ToString() + ") " +
                //    " values (" + valuesSB.ToString() + ")";
                String insertSQL = "insert into " + _tableName + " (" + columnsSB.ToString() + ") " +
                    " values (" + valuesSB.ToString() + ")";

                _insertSQL = insertSQL;
            }

            return _insertSQL;
        }

        public String GetInsertSybase(String TN, IDbCommand command)
        {
            if (_schema == null)
            {
                String message = SysMsg.GetSystemMessage(((DataModule)_updateComonent.OwnerComp).Language, "Srvtools", "UpdateComponent", "msg_SchemaIsNull");
                throw new ArgumentException(String.Format(message, _updateComonent.Name));
            }

            if (_insertSQL != null && _insertSQL.Length > 0) return _insertSQL;

            // Create the table name and schema name.
            CreateTableName(); CreateSchemaName();

            StringBuilder columnsSB = new StringBuilder();
            StringBuilder valuesSB = new StringBuilder();

            Int32 index = 0;
            foreach (DataRow schemaRow in _schema.Rows)
            {
                String columnName = (String)schemaRow["ColumnName"];
                String columnType = schemaRow["DataType"].ToString();
                //String columnTypeName = schemaRow["DataTypeName"].ToString();

                if (IsLeftJoinClumns(schemaRow))
                { index++; continue; }
                else
                { index++; }

                FieldAttr attr = GetFieldAttrByColumnName(columnName);
                if (attr != null && !attr.UpdateEnable)
                {
                    continue;
                }

                //if (IsReadOnly(schemaRow)) continue;

                Object columnValue = GetColumnIOrUValue(columnName, columnType);
                if (columnValue == null || columnValue == DBNull.Value)
                { continue; }

                if (columnsSB.Length > 0)
                { columnsSB.Append(", "); valuesSB.Append(", "); }

                if (string.Compare(columnType, "System.String") == 0)
                {
                    OleDbParameter oParam = new OleDbParameter();
                    oParam.ParameterName = "@" + columnName;
                    oParam.OleDbType = OleDbType.VarChar;
                    oParam.Value = columnValue;
                    command.Parameters.Add(oParam);

                    columnsSB.Append(columnName);
                    valuesSB.Append("?");

                    continue;
                }
                else if (string.Compare(columnType, "System.Byte[]") == 0)
                {
                    OleDbParameter oParam = new OleDbParameter();
                    oParam.ParameterName = "@" + columnName;
                    oParam.OleDbType = OleDbType.Binary;
                    oParam.Value = columnValue;
                    command.Parameters.Add(oParam);

                    columnsSB.Append(columnName);
                    valuesSB.Append("?");

                    continue;
                }

                columnsSB.Append(columnName);
                valuesSB.Append(MarkSybase(columnType, TransformMarkerInColumnValue(columnType, columnValue)));
            }

            if (columnsSB.ToString() != null && columnsSB.ToString().Length > 0)
            {
                String insertSQL = String.Empty;
                if (_tableName != String.Empty)
                    insertSQL = "insert into " + _tableName + " (" + columnsSB.ToString() + ") " +
                                " values (" + valuesSB.ToString() + ")";
                else
                    insertSQL = "insert into " + TN + " (" + columnsSB.ToString() + ") " +
                                " values (" + valuesSB.ToString() + ")";

                _insertSQL = insertSQL;
            }

            return _insertSQL;
        }

        public String GetInsertMySql(IDbCommand command)
        {
            if (_schema == null)
            {
                String message = SysMsg.GetSystemMessage(((DataModule)_updateComonent.OwnerComp).Language, "Srvtools", "UpdateComponent", "msg_SchemaIsNull");
                throw new ArgumentException(String.Format(message, _updateComonent.Name));
            }

            if (_insertSQL != null && _insertSQL.Length > 0) return _insertSQL;

            // Create the table name and schema name.
            CreateTableName(); CreateSchemaName();

            StringBuilder columnsSB = new StringBuilder();
            StringBuilder valuesSB = new StringBuilder();

            Int32 index = 0;
            foreach (DataRow schemaRow in _schema.Rows)
            {
                String columnName = (String)schemaRow["ColumnName"];
                String columnType = schemaRow["DataType"].ToString();
                //String columnTypeName = schemaRow["DataTypeName"].ToString();

                if (IsLeftJoinClumns(schemaRow))
                { index++; continue; }
                else
                { index++; }

                FieldAttr attr = GetFieldAttrByColumnName(columnName);
                if (attr != null && !attr.UpdateEnable)
                {
                    continue;
                }

                //if (IsReadOnly(schemaRow)) continue;

                Object columnValue = GetColumnIOrUValue(columnName, columnType);
                if (columnValue == null || columnValue == DBNull.Value)
                { continue; }

                if (columnsSB.Length > 0)
                { columnsSB.Append(", "); valuesSB.Append(", "); }

#if MySql
                if (string.Compare(columnType, "System.String") == 0)
                {
                    MySql.Data.MySqlClient.MySqlParameter mParam = new MySql.Data.MySqlClient.MySqlParameter();
                    mParam.ParameterName = "@" + columnName;
                    mParam.MySqlDbType = MySql.Data.MySqlClient.MySqlDbType.VarChar;
                    mParam.Value = columnValue;
                    command.Parameters.Add(mParam);

                    columnsSB.Append(columnName);
                    valuesSB.Append("@" + columnName);

                    continue;
                }
                else if (string.Compare(columnType, "System.Byte[]") == 0)
                {
                    MySql.Data.MySqlClient.MySqlParameter mParam = new MySql.Data.MySqlClient.MySqlParameter();
                    mParam.ParameterName = "@" + columnName;
                    mParam.MySqlDbType = MySql.Data.MySqlClient.MySqlDbType.Blob;
                    mParam.Value = columnValue;
                    command.Parameters.Add(mParam);

                    columnsSB.Append(columnName);
                    valuesSB.Append("@" + columnName);

                    continue;
                }
#endif

                columnsSB.Append(columnName);
                valuesSB.Append(MarkMySql(columnType, TransformMarkerInColumnValue(columnType, columnValue)));
            }

            if (columnsSB.ToString() != null && columnsSB.ToString().Length > 0)
            {
                //String insertSQL = "insert into " + TableName + " (" + columnsSB.ToString() + ") " +
                //    " values (" + valuesSB.ToString() + ")";
                String insertSQL = "insert into " + _tableName + " (" + columnsSB.ToString() + ") " +
                    " values (" + valuesSB.ToString() + ")";

                _insertSQL = insertSQL;
            }

            return _insertSQL;
        }

        public String GetInsertOdbc(string TN, OdbcDBType type)
        {
            if (_schema == null)
            {
                String message = SysMsg.GetSystemMessage(((DataModule)_updateComonent.OwnerComp).Language, "Srvtools", "UpdateComponent", "msg_SchemaIsNull");
                throw new ArgumentException(String.Format(message, _updateComonent.Name));
            }

            if (_insertSQL != null && _insertSQL.Length > 0) return _insertSQL;

            // Create the table name and schema name.
            CreateTableName(); CreateSchemaName();

            StringBuilder columnsSB = new StringBuilder();
            StringBuilder valuesSB = new StringBuilder();

            Int32 index = 0;
            foreach (DataRow schemaRow in _schema.Rows)
            {
                String columnName = (String)schemaRow["ColumnName"];
                String columnType = schemaRow["DataType"].ToString();
                //String columnTypeName = schemaRow["DataTypeName"].ToString();

                if (IsLeftJoinClumns(schemaRow))
                { index++; continue; }
                else
                { index++; }

                FieldAttr attr = GetFieldAttrByColumnName(columnName);
                if (attr != null && !attr.UpdateEnable)
                {
                    continue;
                }

                //if (IsReadOnly(schemaRow)) continue;

                Object columnValue = GetColumnIOrUValue(columnName, columnType);
                if (columnValue == null || columnValue == DBNull.Value)
                { continue; }

                if (columnsSB.Length > 0)
                { columnsSB.Append(", "); valuesSB.Append(", "); }

                //columnsSB.Append(Quote(columnName));
                //valuesSB.Append(Mark(columnTypeName, columnType, TransformMarkerInColumnValue(columnType, columnValue)));
                columnsSB.Append(columnName);
                valuesSB.Append(MarkOdbc(columnType, TransformMarkerInColumnValue(columnType, columnValue), type));
            }

            if (columnsSB.ToString() != null && columnsSB.ToString().Length > 0)
            {
                String insertSQL = "";

                if (_tableName == "")
                    insertSQL = "insert into " + TN + " (" + columnsSB.ToString() + ") " +
                    " values (" + valuesSB.ToString() + ")";
                else
                    insertSQL = "insert into " + _tableName + " (" + columnsSB.ToString() + ") " +
                    " values (" + valuesSB.ToString() + ")";

                _insertSQL = insertSQL;
            }

            return _insertSQL;
        }

        public String GetInsertInformix(String TN)
        {
            if (_schema == null)
            {
                String message = SysMsg.GetSystemMessage(((DataModule)_updateComonent.OwnerComp).Language, "Srvtools", "UpdateComponent", "msg_SchemaIsNull");
                throw new ArgumentException(String.Format(message, _updateComonent.Name));
            }

            if (_insertSQL != null && _insertSQL.Length > 0) return _insertSQL;

            // Create the table name and schema name.
            CreateTableName(); CreateSchemaName();

            StringBuilder columnsSB = new StringBuilder();
            StringBuilder valuesSB = new StringBuilder();

            Int32 index = 0;
            foreach (DataRow schemaRow in _schema.Rows)
            {
                String columnName = (String)schemaRow["ColumnName"];
                String columnType = schemaRow["DataType"].ToString();
                //String columnTypeName = schemaRow["DataTypeName"].ToString();

                if (IsLeftJoinClumns(schemaRow))
                { index++; continue; }
                else
                { index++; }

                FieldAttr attr = GetFieldAttrByColumnName(columnName);
                if (attr != null && !attr.UpdateEnable)
                {
                    continue;
                }

                //if (IsReadOnly(schemaRow)) continue;

                Object columnValue = GetColumnIOrUValue(columnName, columnType);
                if (columnValue == null || columnValue == DBNull.Value)
                { continue; }

                if (columnsSB.Length > 0)
                { columnsSB.Append(", "); valuesSB.Append(", "); }

                columnsSB.Append(columnName);
                valuesSB.Append(MarkInformix(columnType, TransformMarkerInColumnValue(columnType, columnValue)));
            }

            if (columnsSB.ToString() != null && columnsSB.ToString().Length > 0)
            {
                if (String.IsNullOrEmpty(_tableName))
                    _insertSQL = "insert into " + TN + " (" + columnsSB.ToString() + ") " +
                        " values (" + valuesSB.ToString() + ")";
                else
                    _insertSQL = "insert into " + _tableName + " (" + columnsSB.ToString() + ") " +
                        " values (" + valuesSB.ToString() + ")";

            }

            return _insertSQL;
        }

        public String GetUpdateSQL()
        {
            if (_schema == null)
            {
                String message = SysMsg.GetSystemMessage(((DataModule)_updateComonent.OwnerComp).Language, "Srvtools", "UpdateComponent", "msg_SchemaIsNull");
                throw new ArgumentException(String.Format(message, _updateComonent.Name));
            }

            if (_updateSQL != null) return _updateSQL;

            CreateTableName(); CreateSchemaName();

            String updatePart = CreateUpdatePart();
            String originalWherePart = CreateOriginalWherePart();

            if (updatePart == null || updatePart.Length == 0)
            { return null; }

            if (originalWherePart == null || originalWherePart.Length == 0)
            { return null; }

            String updateSQL = "update " + TableName + " set " + updatePart + " where " + originalWherePart;
            _updateSQL = updateSQL;
            return _updateSQL;
        }

        public String GetUpdateSybase(String TN, IDbCommand command)
        {
            if (_schema == null)
            {
                String message = SysMsg.GetSystemMessage(((DataModule)_updateComonent.OwnerComp).Language, "Srvtools", "UpdateComponent", "msg_SchemaIsNull");
                throw new ArgumentException(String.Format(message, _updateComonent.Name));
            }

            if (_updateSQL != null) return _updateSQL;

            CreateTableName(); CreateSchemaName();

            String updatePart = CreateSybaseUpdatePart(command);
            String originalWherePart = CreateOriginalSybaseWherePart();

            if (updatePart == null || updatePart.Length == 0)
            { return null; }

            if (originalWherePart == null || originalWherePart.Length == 0)
            { return null; }

            String updateSQL = String.Empty;
            if (_tableName != String.Empty)
                updateSQL = "update " + _tableName + " set " + updatePart + " where " + originalWherePart;
            else
                updateSQL = "update " + TN + " set " + updatePart + " where " + originalWherePart;
            _updateSQL = updateSQL;
            return _updateSQL;
        }

        public String GetUpdateOracle(IDbCommand command)
        {
            if (_schema == null)
            {
                String message = SysMsg.GetSystemMessage(((DataModule)_updateComonent.OwnerComp).Language, "Srvtools", "UpdateComponent", "msg_SchemaIsNull");
                throw new ArgumentException(String.Format(message, _updateComonent.Name));
            }

            if (_updateSQL != null) return _updateSQL;

            CreateTableName(); CreateSchemaName();

            String updatePart = CreateOracleUpdatePart(command);
            String originalWherePart = CreateOriginalOracleWherePart();

            if (updatePart == null || updatePart.Length == 0)
            { return null; }

            if (originalWherePart == null || originalWherePart.Length == 0)
            { return null; }

            //String updateSQL = "update " + TableName + " set " + updatePart + " where " + originalWherePart;
            String updateSQL = "update " + _tableName + " set " + updatePart + " where " + originalWherePart;
            _updateSQL = updateSQL;
            return _updateSQL;
        }

        public String GetUpdateMySql(IDbCommand command)
        {
            if (_schema == null)
            {
                String message = SysMsg.GetSystemMessage(((DataModule)_updateComonent.OwnerComp).Language, "Srvtools", "UpdateComponent", "msg_SchemaIsNull");
                throw new ArgumentException(String.Format(message, _updateComonent.Name));
            }

            if (_updateSQL != null) return _updateSQL;

            CreateTableName(); CreateSchemaName();

            String updatePart = CreateMySqlUpdatePart(command);
            String originalWherePart = CreateOriginalMySqlWherePart();

            if (updatePart == null || updatePart.Length == 0)
            { return null; }

            if (originalWherePart == null || originalWherePart.Length == 0)
            { return null; }

            String updateSQL = "update " + _tableName + " set " + updatePart + " where " + originalWherePart;
            _updateSQL = updateSQL;
            return _updateSQL;
        }

        public String GetUpdateOdbc(string TN, OdbcDBType type)
        {
            if (_schema == null)
            {
                String message = SysMsg.GetSystemMessage(((DataModule)_updateComonent.OwnerComp).Language, "Srvtools", "UpdateComponent", "msg_SchemaIsNull");
                throw new ArgumentException(String.Format(message, _updateComonent.Name));
            }

            if (_updateSQL != null) return _updateSQL;

            CreateTableName(); CreateSchemaName();

            String updatePart = CreateOdbcUpdatePart(type);
            String originalWherePart = CreateOriginalOracleWherePart();

            if (updatePart == null || updatePart.Length == 0)
            { return null; }

            if (originalWherePart == null || originalWherePart.Length == 0)
            { return null; }

            //String updateSQL = "update " + TableName + " set " + updatePart + " where " + originalWherePart;
            String updateSQL = "";
            if (_tableName != "")
                updateSQL = "update " + _tableName + " set " + updatePart + " where " + originalWherePart;
            else
                updateSQL = "update " + TN + " set " + updatePart + " where " + originalWherePart;

            _updateSQL = updateSQL;
            return _updateSQL;
        }

        public String GetUpdateInformix(String TN)
        {
            if (_schema == null)
            {
                String message = SysMsg.GetSystemMessage(((DataModule)_updateComonent.OwnerComp).Language, "Srvtools", "UpdateComponent", "msg_SchemaIsNull");
                throw new ArgumentException(String.Format(message, _updateComonent.Name));
            }

            if (_updateSQL != null) return _updateSQL;

            CreateTableName(); CreateSchemaName();

            String updatePart = CreateInformixUpdatePart();
            String originalWherePart = CreateOriginalInformixWherePart();

            if (updatePart == null || updatePart.Length == 0)
            { return null; }

            if (originalWherePart == null || originalWherePart.Length == 0)
            { return null; }

            if (String.IsNullOrEmpty(_tableName))
                _updateSQL = "update " + TN + " set " + updatePart + " where " + originalWherePart;
            else
                _updateSQL = "update " + _tableName + " set " + updatePart + " where " + originalWherePart;
            return _updateSQL;
        }

        public void RefreshSchema()
        {
            _schema = null;
            _insertSQL = null;
            _deleteSQL = null;
            _updateSQL = null;
            _tableName = null;
            _schemaName = null;
        }

        #endregion

        // --------------------------------------------------------------------

        #region Private Methods

        private void CreateTableName()
        {
            if (_tableName != null && _tableName.Length > 0) return;

            //_tableName = _schema.Rows[0]["BaseTableName"].ToString(); 给表加上owner by Rei
            _schemaName = _schema.Rows[0]["BaseSchemaName"].ToString();
            _tableName = _schema.Rows[0]["BaseTableName"].ToString();
            if (_schemaName != null && _schemaName != "")
                _tableName = _schemaName + "." + _tableName;

        }

        private void CreateSchemaName()
        {
            if (_schemaName != null && _schemaName.Length > 0) return;

            _schemaName = _schema.Rows[0]["BaseSchemaName"].ToString();
        }

        private List<DataRow> GetUpdateColumnsList()
        {
            List<DataRow> columnsList = new List<DataRow>();

            // get updatecomonent's keyfield.
            // String keyField = _updateComonent.KeyField;
            KeyItems keyFields = _updateComonent.SelectCmd.KeyFields;
            //if (keyField != null)
            //{
            //    String[] keyFields = keyField.Split(":".ToCharArray());

            if (keyFields != null)
            {
                foreach (KeyItem tmpKeyField in keyFields)
                {
                    Int32 index = 0;
                    foreach (DataRow schemaRow in _schema.Rows)
                    {
                        if (IsLeftJoinClumns(schemaRow) || IsIncludedInList(schemaRow, columnsList))
                        { index++; continue; }
                        else
                        { index++; }

                        if (string.Compare(((String)schemaRow["ColumnName"]), tmpKeyField.KeyName, true) != 0)//IgnoreCase
                        { continue; }
                        else
                        {
                            if (IsReadOnly(schemaRow)) continue;

                            columnsList.Add(schemaRow);
                            break;
                        }
                    }
                }
            }
            //}

            // if the schemarow not in fieldatttrs then the schemarow can be update.
            Int32 index2 = 0;
            foreach (DataRow schemaRow in _schema.Rows)
            {
                if (IsLeftJoinClumns(schemaRow) || IsIncludedInList(schemaRow, columnsList))
                { index2++; continue; }
                else
                { index2++; }

                bool b = false;  // IsIncludedInUpdate.
                bool c = true;   // IsUpdateEnable.
                foreach (FieldAttr fieldAttr in _updateComonent.FieldAttrs)
                {
                    if (string.Compare(((String)schemaRow["ColumnName"]), fieldAttr.DataField, true) != 0)//IgnoreCase
                    { continue; }
                    else
                    {
                        if (fieldAttr.UpdateEnable == true)
                        {
                            if (IsReadOnly(schemaRow)) continue;

                            b = true;
                            columnsList.Add(schemaRow);
                        }
                        else
                        { c = false; }

                        break;
                    }
                }
                if (b == false && c == true) { columnsList.Add(schemaRow); }
            }

            return columnsList;
        }

        private List<DataRow> GetOracleUpdateColumnsList()
        {
            List<DataRow> columnsList = new List<DataRow>();

            // get updatecomonent's keyfield.
            // String keyField = _updateComonent.KeyField;
            KeyItems keyFields = _updateComonent.SelectCmd.KeyFields;
            //if (keyField != null)
            //{
            //    String[] keyFields = keyField.Split(":".ToCharArray());

            if (keyFields != null)
            {
                foreach (KeyItem tmpKeyField in keyFields)
                {
                    Int32 index = 0;
                    foreach (DataRow schemaRow in _schema.Rows)
                    {
                        if (IsLeftJoinClumns(schemaRow) || IsIncludedInList(schemaRow, columnsList))
                        { index++; continue; }
                        else
                        { index++; }

                        if (string.Compare(((String)schemaRow["ColumnName"]), tmpKeyField.KeyName, true) != 0)//IgnoreCase
                        { continue; }
                        else
                        {
                            //if (IsReadOnly(schemaRow)) continue;

                            columnsList.Add(schemaRow);
                            break;
                        }
                    }
                }
            }
            //}

            // if the schemarow not in fieldatttrs then the schemarow can be update.
            Int32 index2 = 0;
            foreach (DataRow schemaRow in _schema.Rows)
            {
                if (IsLeftJoinClumns(schemaRow) || IsIncludedInList(schemaRow, columnsList))
                { index2++; continue; }
                else
                { index2++; }

                bool b = false;  // IsIncludedInUpdate.
                bool c = true;   // IsUpdateEnable.
                foreach (FieldAttr fieldAttr in _updateComonent.FieldAttrs)
                {
                    if (string.Compare(((String)schemaRow["ColumnName"]), fieldAttr.DataField, true) != 0)//IgnoreCase
                    { continue; }
                    else
                    {
                        if (fieldAttr.UpdateEnable == true)
                        {
                            //if (IsReadOnly(schemaRow)) continue;

                            b = true;
                            columnsList.Add(schemaRow);
                        }
                        else
                        { c = false; }

                        break;
                    }
                }
                if (b == false && c == true) { columnsList.Add(schemaRow); }
            }

            return columnsList;
        }

        private List<DataRow> GetWhereColumnsList()
        {
            List<DataRow> columnsList = new List<DataRow>();

            if (_updateComonent.WhereMode == WhereModeType.All)
            {
                #region All

                Int32 index = 0;
                foreach (DataRow schemaRow in _schema.Rows)
                {
                    if (IsLeftJoinClumns(schemaRow) || IsIncludedInList(schemaRow, columnsList))
                    { index++; continue; }
                    else
                    { index++; }

                    columnsList.Add(schemaRow);
                }

                #endregion
            }
            else
            {
                #region get keyfield

                //String keyField = _updateComonent.KeyField;
                KeyItems keyFields = _updateComonent.SelectCmd.KeyFields;
                if (keyFields == null)
                {
                    if (_updateComonent.WhereMode == WhereModeType.Keyfields)
                    {
                        String message = SysMsg.GetSystemMessage(((DataModule)_updateComonent.OwnerComp).Language, "Srvtools", "UpdateComponent", "msg_KeyFieldsIsNull");
                        throw new ArgumentException(String.Format(message, _updateComonent.Name));
                    }
                }
                else
                {
                    if (_updateComonent.WhereMode == WhereModeType.Keyfields && keyFields.Count == 0)
                    {
                        String message = SysMsg.GetSystemMessage(((DataModule)_updateComonent.OwnerComp).Language, "Srvtools", "UpdateComponent", "msg_KeyFieldsIsNull");
                        throw new ArgumentException(String.Format(message, _updateComonent.Name));
                    }

                    // String[] keyFields = keyField.Split(";".ToCharArray());
                    // KeyItems keyFields = _updateComonent.SelectCmd.KeyFields;
                    if (keyFields != null)
                    {
                        foreach (KeyItem tmpKeyField in keyFields)
                        {
                            Int32 index = 0;
                            Boolean isExist = false;
                            foreach (DataRow schemaRow in _schema.Rows)
                            {
                                if (IsIncludedInList(schemaRow, columnsList))
                                { isExist = true; continue; }

                                if (IsLeftJoinClumns(schemaRow))
                                { index++; continue; }
                                else
                                { index++; }

                                if (string.Compare(tmpKeyField.KeyName, ((String)schemaRow["ColumnName"]), true) != 0)//IgnoreCase
                                { continue; }
                                else
                                {
                                    if (!IsIncludedInWhereClause(schemaRow))
                                    { continue; }
                                    else
                                    { isExist = true; columnsList.Add(schemaRow); break; }
                                }
                            }

                            if (isExist == false)
                            {
                                String message = SysMsg.GetSystemMessage(((DataModule)_updateComonent.OwnerComp).Language, "Srvtools", "UpdateComponent", "msg_KeyFieldNotExistInTable");
                                throw new ArgumentException(String.Format(message, _updateComonent.Name, tmpKeyField));
                            }
                        }
                    }
                }

                #endregion

                #region get fieldattrs

                if (_updateComonent.WhereMode == WhereModeType.FieldAttrs)
                {
                    foreach (FieldAttr fieldAttr in _updateComonent.FieldAttrs)
                    {
                        if (fieldAttr.WhereMode == true)
                        {
                            Int32 index = 0;
                            Boolean isExist = false;
                            foreach (DataRow schemaRow in _schema.Rows)
                            {
                                if (IsIncludedInList(schemaRow, columnsList))
                                { isExist = true; continue; }

                                if (IsLeftJoinClumns(schemaRow))
                                { index++; continue; }
                                else
                                { index++; }

                                if (string.Compare(((String)schemaRow["ColumnName"]), fieldAttr.DataField, true) != 0)//IgnoreCase
                                { continue; }
                                else
                                { isExist = true; columnsList.Add(schemaRow); break; }
                            }

                            if (isExist == false)
                            {
                                String message = SysMsg.GetSystemMessage(((DataModule)_updateComonent.OwnerComp).Language, "Srvtools", "UpdateComponent", "msg_FieldAttrNotExistInTable");
                                throw new ArgumentException(String.Format(message, _updateComonent.Name, fieldAttr));
                            }
                        }
                    }
                }

                #endregion
            }

            // if (columnsList.Count == 0) throw new Exception("no column in where clause");

            return columnsList;
        }

        private String CreateUpdatePart()
        {
            // create updateColumns.
            StringBuilder updatePartSB = new StringBuilder();

            List<DataRow> columnsList = GetUpdateColumnsList();
            var updateColumns = new List<string>();
            foreach (DataRow schemaRow in columnsList)
            {
                String columnName = (String)schemaRow["ColumnName"];
                String columnType = schemaRow["DataType"].ToString();
                String columnTypeName = schemaRow["DataTypeName"].ToString();
                String baseColumnName = (String)schemaRow["BaseColumnName"];

                Object currentValue = _row[columnName, DataRowVersion.Current];
                Object originalValue = _row[columnName, DataRowVersion.Original];

                //由原来的DBNull.Value和Null才带DefaultValue —> 任何时候都带DefaultValue
                //if ((originalValue == null || originalValue == DBNull.Value) && (currentValue == null || currentValue == DBNull.Value))
                //{ 
                currentValue = GetColumnIOrUValue(columnName, columnType);
                if ((currentValue == null || currentValue == DBNull.Value) &&
                (originalValue == null || originalValue == DBNull.Value))
                {
                    continue;
                }
                //}
                //else
                //{ 
                if (currentValue.Equals(originalValue)) continue;
                //}
                if (updateColumns.Contains(baseColumnName))
                {
                    continue;
                }
                //byte[] compare
                if (columnType == "System.Byte[]")
                {
                    if (currentValue != DBNull.Value && originalValue != DBNull.Value)
                    {
                        byte[] btc = (byte[])currentValue;
                        byte[] bto = (byte[])originalValue;
                        if (btc.Length == bto.Length)
                        {
                            bool blequal = true;
                            for (int i = 0; i < btc.Length; i++)
                            {
                                if (!btc[i].Equals(bto[i]))
                                {
                                    blequal = false;
                                    break;
                                }
                            }
                            if (blequal)
                            {
                                continue;
                            }
                        }
                    }
                }


                if (updatePartSB.Length > 0)
                    updatePartSB.Append(", ");

                if (currentValue == null || currentValue == DBNull.Value)
                { updatePartSB.Append(Quote(baseColumnName) + " = null"); }
                else
                { updatePartSB.Append(Quote(baseColumnName) + " = " + Mark(columnTypeName, columnType, TransformMarkerInColumnValue(columnType, currentValue))); }
                updateColumns.Add(baseColumnName);
                continue;
            }

            return updatePartSB.ToString();
        }

        private String CreateOracleUpdatePart(IDbCommand command)
        {
            // create updateColumns.
            StringBuilder updatePartSB = new StringBuilder();

            List<DataRow> columnsList = GetOracleUpdateColumnsList();

            foreach (DataRow schemaRow in columnsList)
            {
                String columnName = (String)schemaRow["ColumnName"];
                String columnType = schemaRow["DataType"].ToString();
                //String columnTypeName = schemaRow["DataTypeName"].ToString();

                Object currentValue = _row[columnName, DataRowVersion.Current];
                Object originalValue = _row[columnName, DataRowVersion.Original];

                //由原来的DBNull.Value和Null才带DefaultValue —> 任何时候都带DefaultValue
                //if ((originalValue == null || originalValue == DBNull.Value) && (currentValue == null || currentValue == DBNull.Value))
                //{ 
                currentValue = GetColumnIOrUValue(columnName, columnType);
                if ((currentValue == null || currentValue == DBNull.Value) &&
                (originalValue == null || originalValue == DBNull.Value))
                {
                    continue;
                }
                //}
                //else
                //{ 
                if (currentValue.Equals(originalValue)) continue;
                //}
                if (updatePartSB.Length > 0)
                    updatePartSB.Append(", ");

                if (currentValue == null || currentValue == DBNull.Value)
                { updatePartSB.Append(columnName + " = null"); }
                else
                {
                    if (columnType == "System.String")
                    {
                        if (string.Compare(schemaRow["IsLong"].ToString(), bool.TrueString, true) == 0)
                        {
                            OracleParameter oParam = new OracleParameter();
                            oParam.ParameterName = ":" + columnName;
                            oParam.OracleType = OracleType.Clob;
                            oParam.Value = currentValue;
                            command.Parameters.Add(oParam);

                            updatePartSB.Append(columnName + "=:" + columnName);
                        }
                        else
                        {

                            OracleParameter oParam = new OracleParameter();
                            oParam.ParameterName = ":" + columnName;
                            oParam.OracleType = OracleType.NVarChar;
                            oParam.Value = currentValue;
                            command.Parameters.Add(oParam);

                            updatePartSB.Append(columnName + "=:" + columnName);
                        }
                    }
                    else if (columnType == "System.Byte[]")
                    {
                        if (currentValue != DBNull.Value && originalValue != DBNull.Value)
                        {
                            byte[] btc = (byte[])currentValue;
                            byte[] bto = (byte[])originalValue;
                            if (btc.Length == bto.Length)
                            {
                                bool blequal = true;
                                for (int i = 0; i < btc.Length; i++)
                                {
                                    if (!btc[i].Equals(bto[i]))
                                    {
                                        blequal = false;
                                        break;
                                    }
                                }
                                if (blequal)
                                {
                                    continue;
                                }
                            }
                        }

                        OracleParameter oParam = new OracleParameter();
                        oParam.ParameterName = ":" + columnName;
                        oParam.OracleType = OracleType.Blob;
                        //oParam.Value = Mark(columnType, TransformMarkerInColumnValue(columnType, currentValue));
                        oParam.Value = currentValue;
                        command.Parameters.Add(oParam);

                        updatePartSB.Append(columnName + "=:" + columnName);
                    }
                    else
                    { updatePartSB.Append(columnName + " = " + Mark(columnType, TransformMarkerInColumnValue(columnType, currentValue))); }
                    //{ updatePartSB.Append(Quote(columnName) + " = " + Mark(columnTypeName, columnType, TransformMarkerInColumnValue(columnType, currentValue))); }
                }
            }

            return updatePartSB.ToString();
        }

        private String CreateSybaseUpdatePart(IDbCommand command)
        {
            // create updateColumns.
            StringBuilder updatePartSB = new StringBuilder();

            List<DataRow> columnsList = GetOracleUpdateColumnsList();

            foreach (DataRow schemaRow in columnsList)
            {
                String columnName = (String)schemaRow["ColumnName"];
                String columnType = schemaRow["DataType"].ToString();
                //String columnTypeName = schemaRow["DataTypeName"].ToString();

                Object currentValue = _row[columnName, DataRowVersion.Current];
                Object originalValue = _row[columnName, DataRowVersion.Original];

                //由原来的DBNull.Value和Null才带DefaultValue —> 任何时候都带DefaultValue
                //if ((originalValue == null || originalValue == DBNull.Value) && (currentValue == null || currentValue == DBNull.Value))
                //{ 
                currentValue = GetColumnIOrUValue(columnName, columnType);
                if ((currentValue == null || currentValue == DBNull.Value) &&
                (originalValue == null || originalValue == DBNull.Value))
                {
                    continue;
                }
                //}
                //else
                //{ 
                if (currentValue.Equals(originalValue)) continue;
                //}
                if (updatePartSB.Length > 0)
                    updatePartSB.Append(", ");

                if (currentValue == null || currentValue == DBNull.Value)
                { updatePartSB.Append(columnName + " = null"); }
                else
                {
                    if (columnType == "System.String")
                    {
                        OleDbParameter oParam = new OleDbParameter();
                        oParam.ParameterName = "@" + columnName;
                        oParam.OleDbType = OleDbType.VarChar;
                        oParam.Value = currentValue;
                        command.Parameters.Add(oParam);

                        updatePartSB.Append(columnName + "=?");
                        //updatePartSB.Append(columnName + "=@" + columnName);
                    }
                    else if (columnType == "System.Byte[]")
                    {
                        if (currentValue != DBNull.Value && originalValue != DBNull.Value)
                        {
                            byte[] btc = (byte[])currentValue;
                            byte[] bto = (byte[])originalValue;
                            if (btc.Length == bto.Length)
                            {
                                bool blequal = true;
                                for (int i = 0; i < btc.Length; i++)
                                {
                                    if (!btc[i].Equals(bto[i]))
                                    {
                                        blequal = false;
                                        break;
                                    }
                                }
                                if (blequal)
                                {
                                    continue;
                                }
                            }
                        }

                        OleDbParameter oParam = new OleDbParameter();
                        oParam.ParameterName = "@" + columnName;
                        oParam.OleDbType = OleDbType.Binary;
                        //oParam.Value = Mark(columnType, TransformMarkerInColumnValue(columnType, currentValue));
                        oParam.Value = currentValue;
                        command.Parameters.Add(oParam);

                        //updatePartSB.Append(columnName + "=@" + columnName);
                        updatePartSB.Append(columnName + "=?");
                    }
                    else
                    { updatePartSB.Append(columnName + " = " + MarkSybase(columnType, TransformMarkerInColumnValue(columnType, currentValue))); }
                    //{ updatePartSB.Append(Quote(columnName) + " = " + Mark(columnTypeName, columnType, TransformMarkerInColumnValue(columnType, currentValue))); }
                }



            }

            return updatePartSB.ToString();
        }

        private String CreateMySqlUpdatePart(IDbCommand command)
        {
            // create updateColumns.
            StringBuilder updatePartSB = new StringBuilder();

            List<DataRow> columnsList = GetOracleUpdateColumnsList();

            foreach (DataRow schemaRow in columnsList)
            {
                String columnName = (String)schemaRow["ColumnName"];
                String columnType = schemaRow["DataType"].ToString();
                //String columnTypeName = schemaRow["DataTypeName"].ToString();

                Object currentValue = _row[columnName, DataRowVersion.Current];
                Object originalValue = _row[columnName, DataRowVersion.Original];

                //由原来的DBNull.Value和Null才带DefaultValue —> 任何时候都带DefaultValue
                //if ((originalValue == null || originalValue == DBNull.Value) && (currentValue == null || currentValue == DBNull.Value))
                //{ 
                currentValue = GetColumnIOrUValue(columnName, columnType);
                if ((currentValue == null || currentValue == DBNull.Value) &&
                (originalValue == null || originalValue == DBNull.Value))
                {
                    continue;
                }
                //}
                //else
                //{ 
                if (currentValue.Equals(originalValue)) continue;
                //}
#if MySql
                if (updatePartSB.Length > 0)
                    updatePartSB.Append(", ");

                if (currentValue == null || currentValue == DBNull.Value)
                { updatePartSB.Append(columnName + " = null"); }
                else
                {
                    if (columnType == "System.String")
                    {
                        MySql.Data.MySqlClient.MySqlParameter mParam = new MySql.Data.MySqlClient.MySqlParameter();
                        mParam.ParameterName = "@" + columnName;
                        mParam.MySqlDbType = MySql.Data.MySqlClient.MySqlDbType.VarChar;
                        mParam.Value = currentValue;
                        command.Parameters.Add(mParam);

                        updatePartSB.Append(columnName + "=@" + columnName);
                    }
                    else if (columnType == "System.Byte[]")
                    {
                        if (currentValue != DBNull.Value && originalValue != DBNull.Value)
                        {
                            byte[] btc = (byte[])currentValue;
                            byte[] bto = (byte[])originalValue;
                            if (btc.Length == bto.Length)
                            {
                                bool blequal = true;
                                for (int i = 0; i < btc.Length; i++)
                                {
                                    if (!btc[i].Equals(bto[i]))
                                    {
                                        blequal = false;
                                        break;
                                    }
                                }
                                if (blequal)
                                {
                                    continue;
                                }
                            }
                        }

                        MySql.Data.MySqlClient.MySqlParameter mParam = new MySql.Data.MySqlClient.MySqlParameter();
                        mParam.ParameterName = "@" + columnName;
                        mParam.MySqlDbType = MySql.Data.MySqlClient.MySqlDbType.Blob;
                        mParam.Value = currentValue;
                        command.Parameters.Add(mParam);

                        updatePartSB.Append(columnName + "=@" + columnName);
                    }
                    else
                    { updatePartSB.Append(columnName + " = " + MarkMySql(columnType, TransformMarkerInColumnValue(columnType, currentValue))); }
                }
#endif
            }

            return updatePartSB.ToString();
        }

        private String CreateOdbcUpdatePart(OdbcDBType type)
        {
            // create updateColumns.
            StringBuilder updatePartSB = new StringBuilder();

            List<DataRow> columnsList = GetOracleUpdateColumnsList();

            foreach (DataRow schemaRow in columnsList)
            {
                String columnName = (String)schemaRow["ColumnName"];
                String columnType = schemaRow["DataType"].ToString();
                //String columnTypeName = schemaRow["DataTypeName"].ToString();

                Object currentValue = _row[columnName, DataRowVersion.Current];
                Object originalValue = _row[columnName, DataRowVersion.Original];

                //由原来的DBNull.Value和Null才带DefaultValue —> 任何时候都带DefaultValue
                //if ((originalValue == null || originalValue == DBNull.Value) && (currentValue == null || currentValue == DBNull.Value))
                //{ 
                currentValue = GetColumnIOrUValue(columnName, columnType);
                if ((currentValue == null || currentValue == DBNull.Value) &&
                (originalValue == null || originalValue == DBNull.Value))
                {
                    continue;
                }
                //}
                //else
                //{ 
                if (currentValue.Equals(originalValue)) continue;
                //}

                if (columnType == "System.Byte[]")
                {
                    if (currentValue != DBNull.Value && originalValue != DBNull.Value)
                    {
                        byte[] btc = (byte[])currentValue;
                        byte[] bto = (byte[])originalValue;
                        if (btc.Length == bto.Length)
                        {
                            bool blequal = true;
                            for (int i = 0; i < btc.Length; i++)
                            {
                                if (!btc[i].Equals(bto[i]))
                                {
                                    blequal = false;
                                    break;
                                }
                            }
                            if (blequal)
                            {
                                continue;
                            }
                        }
                    }
                }

                if (updatePartSB.Length > 0)
                    updatePartSB.Append(", ");

                if (currentValue == null || currentValue == DBNull.Value)
                { updatePartSB.Append(columnName + " = null"); }
                else
                {
                    updatePartSB.Append(columnName + " = " + MarkOdbc(columnType, TransformMarkerInColumnValue(columnType, currentValue), type));
                }
                continue;
            }
            return updatePartSB.ToString();
        }

        private String CreateInformixUpdatePart()
        {
            // create updateColumns.
            StringBuilder updatePartSB = new StringBuilder();

            List<DataRow> columnsList = GetOracleUpdateColumnsList();

            foreach (DataRow schemaRow in columnsList)
            {
                String columnName = (String)schemaRow["ColumnName"];
                String columnType = schemaRow["DataType"].ToString();
                //String columnTypeName = schemaRow["DataTypeName"].ToString();

                Object currentValue = _row[columnName, DataRowVersion.Current];
                Object originalValue = _row[columnName, DataRowVersion.Original];

                //由原来的DBNull.Value和Null才带DefaultValue —> 任何时候都带DefaultValue
                //if ((originalValue == null || originalValue == DBNull.Value) && (currentValue == null || currentValue == DBNull.Value))
                //{ 
                currentValue = GetColumnIOrUValue(columnName, columnType);
                if ((currentValue == null || currentValue == DBNull.Value) &&
                (originalValue == null || originalValue == DBNull.Value))
                {
                    continue;
                }
                //}
                //else
                //{ 
                if (currentValue.Equals(originalValue)) continue;
                //}
                if (columnType == "System.Byte[]")
                {
                    if (currentValue != DBNull.Value && originalValue != DBNull.Value)
                    {
                        byte[] btc = (byte[])currentValue;
                        byte[] bto = (byte[])originalValue;
                        if (btc.Length == bto.Length)
                        {
                            bool blequal = true;
                            for (int i = 0; i < btc.Length; i++)
                            {
                                if (!btc[i].Equals(bto[i]))
                                {
                                    blequal = false;
                                    break;
                                }
                            }
                            if (blequal)
                            {
                                continue;
                            }
                        }
                    }
                }

                if (updatePartSB.Length > 0)
                    updatePartSB.Append(", ");

                if (currentValue == null || currentValue == DBNull.Value)
                { updatePartSB.Append(columnName + " = null"); }
                else
                { updatePartSB.Append(columnName + " = " + MarkInformix(columnType, TransformMarkerInColumnValue(columnType, currentValue))); }

                continue;
            }

            return updatePartSB.ToString();
        }


        /// <summary>
        /// 创建Update或者Delete的SQL语句的Where部分。
        /// </summary>
        /// <returns></returns>
        private String CreateOriginalWherePart()
        {
            StringBuilder originalWherePartSB = new StringBuilder();

            List<DataRow> columnsList = GetWhereColumnsList();
            if (columnsList == null || columnsList.Count == 0)
            { return null; }

            foreach (DataRow schemaRow in columnsList)
            {
                String columnName = (String)schemaRow["ColumnName"];
                String columnType = schemaRow["DataType"].ToString();
                String columnTypeName = schemaRow["DataTypeName"].ToString();

                Object originalValue = _row[columnName, DataRowVersion.Original];
                if (originalValue == null || originalValue == DBNull.Value)
                { originalWherePartSB.Append(Quote(columnName) + " is null and "); }
                else
                {
                    originalWherePartSB.Append(Quote(columnName) + " = "
                        + Mark(columnTypeName, columnType, TransformMarkerInColumnValue(columnType, originalValue)) + " and ");
                }
            }

            originalWherePartSB.Remove(originalWherePartSB.Length - 5, 5);
            return originalWherePartSB.ToString();
        }

        private String CreateOriginalOracleWherePart()
        {
            StringBuilder originalWherePartSB = new StringBuilder();

            List<DataRow> columnsList = GetWhereColumnsList();
            if (columnsList == null || columnsList.Count == 0)
            { return null; }

            foreach (DataRow schemaRow in columnsList)
            {
                String columnName = (String)schemaRow["ColumnName"];
                String columnType = schemaRow["DataType"].ToString();
                //String columnTypeName = schemaRow["DataTypeName"].ToString();

                Object originalValue = _row[columnName, DataRowVersion.Original];
                if (originalValue == null || originalValue == DBNull.Value)
                { originalWherePartSB.Append(columnName + " is null and "); }
                else
                {
                    originalWherePartSB.Append(columnName + " = "
                        + Mark(columnType, TransformMarkerInColumnValue(columnType, originalValue)) + " and ");
                    //originalWherePartSB.Append(Quote(columnName) + " = "
                    //    + Mark(columnTypeName, columnType, TransformMarkerInColumnValue(columnType, originalValue)) + " and ");
                }
            }

            originalWherePartSB.Remove(originalWherePartSB.Length - 5, 5);
            return originalWherePartSB.ToString();
        }

        private String CreateOriginalSybaseWherePart()
        {
            StringBuilder originalWherePartSB = new StringBuilder();

            List<DataRow> columnsList = GetWhereColumnsList();
            if (columnsList == null || columnsList.Count == 0)
            { return null; }

            foreach (DataRow schemaRow in columnsList)
            {
                String columnName = (String)schemaRow["ColumnName"];
                String columnType = schemaRow["DataType"].ToString();
                //String columnTypeName = schemaRow["DataTypeName"].ToString();

                Object originalValue = _row[columnName, DataRowVersion.Original];
                if (originalValue == null || originalValue == DBNull.Value)
                { originalWherePartSB.Append(columnName + " is null and "); }
                else
                {
                    originalWherePartSB.Append(columnName + " = "
                        + MarkSybase(columnType, TransformMarkerInColumnValue(columnType, originalValue)) + " and ");
                    //originalWherePartSB.Append(Quote(columnName) + " = "
                    //    + Mark(columnTypeName, columnType, TransformMarkerInColumnValue(columnType, originalValue)) + " and ");
                }
            }

            originalWherePartSB.Remove(originalWherePartSB.Length - 5, 5);
            return originalWherePartSB.ToString();
        }

        private String CreateOriginalOdbcWherePart(OdbcDBType type)
        {
            StringBuilder originalWherePartSB = new StringBuilder();

            List<DataRow> columnsList = GetWhereColumnsList();
            if (columnsList == null || columnsList.Count == 0)
            { return null; }

            foreach (DataRow schemaRow in columnsList)
            {
                String columnName = (String)schemaRow["ColumnName"];
                String columnType = schemaRow["DataType"].ToString();
                //String columnTypeName = schemaRow["DataTypeName"].ToString();

                Object originalValue = _row[columnName, DataRowVersion.Original];
                if (originalValue == null || originalValue == DBNull.Value)
                { originalWherePartSB.Append(columnName + " is null and "); }
                else
                {
                    originalWherePartSB.Append(columnName + " = "
                        + MarkOdbc(columnType, TransformMarkerInColumnValue(columnType, originalValue), type) + " and ");
                    //originalWherePartSB.Append(Quote(columnName) + " = "
                    //    + Mark(columnTypeName, columnType, TransformMarkerInColumnValue(columnType, originalValue)) + " and ");
                }
            }

            originalWherePartSB.Remove(originalWherePartSB.Length - 5, 5);
            return originalWherePartSB.ToString();
        }

        private String CreateOriginalMySqlWherePart()
        {
            StringBuilder originalWherePartSB = new StringBuilder();

            List<DataRow> columnsList = GetWhereColumnsList();
            if (columnsList == null || columnsList.Count == 0)
            { return null; }

            foreach (DataRow schemaRow in columnsList)
            {
                String columnName = (String)schemaRow["ColumnName"];
                String columnType = schemaRow["DataType"].ToString();
                //String columnTypeName = schemaRow["DataTypeName"].ToString();

                Object originalValue = _row[columnName, DataRowVersion.Original];
                if (originalValue == null || originalValue == DBNull.Value)
                { originalWherePartSB.Append(columnName + " is null and "); }
                else
                {
                    originalWherePartSB.Append(columnName + " = "
                        + MarkMySql(columnType, TransformMarkerInColumnValue(columnType, originalValue)) + " and ");
                }
            }

            originalWherePartSB.Remove(originalWherePartSB.Length - 5, 5);
            return originalWherePartSB.ToString();
        }

        private String CreateOriginalInformixWherePart()
        {
            StringBuilder originalWherePartSB = new StringBuilder();

            List<DataRow> columnsList = GetWhereColumnsList();
            if (columnsList == null || columnsList.Count == 0)
            { return null; }

            foreach (DataRow schemaRow in columnsList)
            {
                String columnName = (String)schemaRow["ColumnName"];
                String columnType = schemaRow["DataType"].ToString();
                //String columnTypeName = schemaRow["DataTypeName"].ToString();

                Object originalValue = _row[columnName, DataRowVersion.Original];
                if (originalValue == null || originalValue == DBNull.Value)
                { originalWherePartSB.Append(columnName + " is null and "); }
                else
                {
                    originalWherePartSB.Append(columnName + " = "
                        + MarkInformix(columnType, TransformMarkerInColumnValue(columnType, originalValue)) + " and ");
                }
            }

            originalWherePartSB.Remove(originalWherePartSB.Length - 5, 5);
            return originalWherePartSB.ToString();
        }

        /// <summary>
        /// 得到WriteBack SQL语句的Where部分。
        /// </summary>
        /// <returns></returns>
        public String GetWriteBackSQLWherePart()
        {
            StringBuilder wBWhereSQLSB = new StringBuilder();

            List<DataRow> whereColumnsList = GetWhereColumnsList();
            foreach (DataRow r in whereColumnsList)
            {
                String columnName = r["ColumnName"].ToString();
                String columnType = r["DataType"].ToString();
                String columnTypeName = r["DataTypeName"].ToString();

                Object columnValue = null;
                if (_row.RowState == DataRowState.Deleted)
                { columnValue = _row[columnName, DataRowVersion.Original]; }
                else
                { columnValue = _row[columnName, DataRowVersion.Current]; }

                if (wBWhereSQLSB.Length > 0)
                { wBWhereSQLSB.Append(" and "); }

                if (columnValue == null || columnValue == DBNull.Value)
                { wBWhereSQLSB.Append(Quote(columnName) + " = null"); }
                else
                { wBWhereSQLSB.Append(Quote(columnName) + " = " + Mark(columnTypeName, columnType, TransformMarkerInColumnValue(columnType, columnValue))); }
            }

            return wBWhereSQLSB.ToString();
        }

        public String GetWriteBackOracleWherePart()
        {
            StringBuilder wBWhereSQLSB = new StringBuilder();

            List<DataRow> whereColumnsList = GetWhereColumnsList();
            foreach (DataRow r in whereColumnsList)
            {
                String columnName = r["ColumnName"].ToString();
                String columnType = r["DataType"].ToString();
                //String columnTypeName = r["DataTypeName"].ToString();

                Object columnValue = null;
                if (_row.RowState == DataRowState.Deleted)
                { columnValue = _row[columnName, DataRowVersion.Original]; }
                else
                { columnValue = _row[columnName, DataRowVersion.Current]; }

                if (wBWhereSQLSB.Length > 0)
                { wBWhereSQLSB.Append(" and "); }

                if (columnValue == null || columnValue == DBNull.Value)
                { wBWhereSQLSB.Append(columnName + " is null"); }
                else
                { wBWhereSQLSB.Append(columnName + " = " + Mark(columnType, TransformMarkerInColumnValue(columnType, columnValue))); }
                //{ wBWhereSQLSB.Append(Quote(columnName) + " = " + Mark(columnTypeName, columnType, TransformMarkerInColumnValue(columnType, columnValue))); }
            }

            return wBWhereSQLSB.ToString();
        }

        public String GetWriteBackOdbcWherePart(OdbcDBType type)
        {
            StringBuilder wBWhereSQLSB = new StringBuilder();

            List<DataRow> whereColumnsList = GetWhereColumnsList();
            foreach (DataRow r in whereColumnsList)
            {
                String columnName = r["ColumnName"].ToString();
                String columnType = r["DataType"].ToString();
                //String columnTypeName = r["DataTypeName"].ToString();

                Object columnValue = null;
                if (_row.RowState == DataRowState.Deleted)
                { columnValue = _row[columnName, DataRowVersion.Original]; }
                else
                { columnValue = _row[columnName, DataRowVersion.Current]; }

                if (wBWhereSQLSB.Length > 0)
                { wBWhereSQLSB.Append(" and "); }

                if (columnValue == null || columnValue == DBNull.Value)
                { wBWhereSQLSB.Append(columnName + "is null"); }
                else
                { wBWhereSQLSB.Append(columnName + " = " + MarkOdbc(columnType, TransformMarkerInColumnValue(columnType, columnValue), type)); }
                //{ wBWhereSQLSB.Append(Quote(columnName) + " = " + Mark(columnTypeName, columnType, TransformMarkerInColumnValue(columnType, columnValue))); }
            }

            return wBWhereSQLSB.ToString();
        }

        public String GetWriteBackSybaseWherePart()
        {
            StringBuilder wBWhereSQLSB = new StringBuilder();

            List<DataRow> whereColumnsList = GetWhereColumnsList();
            foreach (DataRow r in whereColumnsList)
            {
                String columnName = r["ColumnName"].ToString();
                String columnType = r["DataType"].ToString();
                //String columnTypeName = r["DataTypeName"].ToString();

                Object columnValue = null;
                if (_row.RowState == DataRowState.Deleted)
                { columnValue = _row[columnName, DataRowVersion.Original]; }
                else
                { columnValue = _row[columnName, DataRowVersion.Current]; }

                if (wBWhereSQLSB.Length > 0)
                { wBWhereSQLSB.Append(" and "); }

                if (columnValue == null || columnValue == DBNull.Value)
                { wBWhereSQLSB.Append(columnName + "is null"); }
                else
                { wBWhereSQLSB.Append(columnName + " = " + MarkSybase(columnType, TransformMarkerInColumnValue(columnType, columnValue))); }
                //{ wBWhereSQLSB.Append(Quote(columnName) + " = " + Mark(columnTypeName, columnType, TransformMarkerInColumnValue(columnType, columnValue))); }
            }

            return wBWhereSQLSB.ToString();
        }

        public String GetWriteBackMySqlWherePart()
        {
            StringBuilder wBWhereSQLSB = new StringBuilder();

            List<DataRow> whereColumnsList = GetWhereColumnsList();
            foreach (DataRow r in whereColumnsList)
            {
                String columnName = r["ColumnName"].ToString();
                String columnType = r["DataType"].ToString();
                //String columnTypeName = r["DataTypeName"].ToString();

                Object columnValue = null;
                if (_row.RowState == DataRowState.Deleted)
                { columnValue = _row[columnName, DataRowVersion.Original]; }
                else
                { columnValue = _row[columnName, DataRowVersion.Current]; }

                if (wBWhereSQLSB.Length > 0)
                { wBWhereSQLSB.Append(" and "); }

                if (columnValue == null || columnValue == DBNull.Value)
                { wBWhereSQLSB.Append(columnName + "is null"); }
                else
                { wBWhereSQLSB.Append(columnName + " = " + MarkMySql(columnType, TransformMarkerInColumnValue(columnType, columnValue))); }
            }

            return wBWhereSQLSB.ToString();
        }

        public String GetWriteBackInformixWherePart()
        {
            StringBuilder wBWhereSQLSB = new StringBuilder();

            List<DataRow> whereColumnsList = GetWhereColumnsList();
            foreach (DataRow r in whereColumnsList)
            {
                String columnName = r["ColumnName"].ToString();
                String columnType = r["DataType"].ToString();
                //String columnTypeName = r["DataTypeName"].ToString();

                Object columnValue = null;
                if (_row.RowState == DataRowState.Deleted)
                { columnValue = _row[columnName, DataRowVersion.Original]; }
                else
                { columnValue = _row[columnName, DataRowVersion.Current]; }

                if (wBWhereSQLSB.Length > 0)
                { wBWhereSQLSB.Append(" and "); }

                if (columnValue == null || columnValue == DBNull.Value)
                { wBWhereSQLSB.Append(columnName + "is null"); }
                else
                { wBWhereSQLSB.Append(columnName + " = " + MarkInformix(columnType, TransformMarkerInColumnValue(columnType, columnValue))); }
            }

            return wBWhereSQLSB.ToString();
        }


        private String Quote(String table_or_column)
        {
            if (_quotePrefix == null || _quoteSuffix == null)
                return table_or_column;
            return _quotePrefix + table_or_column + _quoteSuffix;
        }

        private DataRow GetSchemaRow(String rowName)
        {
            foreach (DataRow r in _schema.Rows)
            {
                if (string.Compare(r["ColumnName"].ToString(), rowName, true) == 0)//IgnoreCase
                { return r; }
            }
            return null;
        }

        //private static Boolean IsIncludedInInsert(DataRow schemaRow)
        //{
        //    if ((bool)schemaRow["IsRowVersion"])
        //        return false;
        //    if ((bool)schemaRow["IsReadOnly"])
        //        return false;
        //    return true;
        //}

        //private static Boolean IsIncludedInUpdate(DataRow schemaRow)
        //{
        //    if ((bool)schemaRow["IsRowVersion"])
        //        return false;
        //    if ((bool)schemaRow["IsReadOnly"])    // not had one line 
        //        return false;
        //    return true;
        //}

        private static Boolean IsReadOnly(DataRow schemaRow)
        {
            if ((bool)schemaRow["IsRowVersion"])
                return true;
            if ((bool)schemaRow["IsReadOnly"])
                return true;
            return false;
        }

        private static Boolean IsIncludedInWhereClause(DataRow schemaRow)
        {
            bool hasErrors = schemaRow.HasErrors;

            return true;
        }

        private static Boolean IsIncludedInList(DataRow schemaRow, List<DataRow> list)
        {
            foreach (DataRow r in list)
            {
                if (string.Compare(schemaRow["ColumnName"].ToString(), r["ColumnName"].ToString(), true) == 0)//IgnoreCase
                { return true; }
            }
            return false;
        }

        private FieldAttr GetFieldAttrByColumnName(String columnName)
        {
            //List<FieldAttr> fieldAttrsList = _updateComonent.FieldAttrs;
            FieldAttrCollection fieldAttrsList = _updateComonent.FieldAttrs;
            if (fieldAttrsList == null)
            { return null; }
            else
            { if (fieldAttrsList.Count == 0) return null; }

            foreach (FieldAttr f in fieldAttrsList)
            {
                if (string.Compare(f.DataField, columnName, true) == 0)//IgnoreCase
                { return f; }
            }
            return null;
        }

        // 有可能含“null”的字符串。
        // GetColumnInsertOrUpdateValue
        private Object GetColumnIOrUValue(String columnName, String columnType)
        {
            object columnValue = _row[columnName, DataRowVersion.Current];

            FieldAttr fieldAttr = GetFieldAttrByColumnName(columnName);
            if (fieldAttr != null)
            {
                if (fieldAttr.DefaultValue != null && fieldAttr.DefaultValue.Length > 0)
                {
                    if (!((fieldAttr.DefaultMode != DefaultModeType.Update && _row.RowState == DataRowState.Added)
                            || (fieldAttr.DefaultMode != DefaultModeType.Insert && _row.RowState == DataRowState.Modified)))
                    {
                        if ((columnValue == null || columnValue.ToString().Length == 0) && fieldAttr.CheckNull == true)
                        {
                            String message = SysMsg.GetSystemMessage(((DataModule)_updateComonent.OwnerComp).Language, "Srvtools", "UpdateComponent", "msg_FieldAttrValueIsNull");
                            throw new ArgumentException(String.Format(message, _updateComonent.Name, fieldAttr.DataField));
                        }
                    }
                    else
                    {
                        String temp2 = GetFieldDefaultValue(fieldAttr);
                        if (temp2 != null && temp2.Length > 0)
                        { columnValue = temp2; }
                        else
                        {
                            if (fieldAttr.CheckNull == true)
                            {
                                String message = SysMsg.GetSystemMessage(((DataModule)_updateComonent.OwnerComp).Language, "Srvtools", "UpdateComponent", "msg_FieldAttrValueIsNull");
                                throw new ArgumentException(String.Format(message, _updateComonent.Name, fieldAttr.DataField));
                            }
                        }
                    }
                }
                else
                {
                    if ((columnValue == null || columnValue.ToString().Length == 0) && fieldAttr.CheckNull == true)
                    {
                        String message = SysMsg.GetSystemMessage(((DataModule)_updateComonent.OwnerComp).Language, "Srvtools", "UpdateComponent", "msg_FieldAttrValueIsNull");
                        throw new ArgumentException(String.Format(message, _updateComonent.Name, fieldAttr.DataField));
                    }
                }

                if (fieldAttr.TrimLength != 0 && Type.GetType(columnType).Equals(typeof(String)))
                {
                    //設定FieldAttrs的TrimLength為-1時，代表要將空白內容轉成Null來存檔。CMC
                    if (fieldAttr.TrimLength == -1 && string.IsNullOrEmpty(columnValue.ToString().Trim()))
                    {
                        columnValue = DBNull.Value;
                    }
                    else if (fieldAttr.TrimLength != -1)
                    {
                        int lengh = (columnValue.ToString().Length < fieldAttr.TrimLength) ? columnValue.ToString().Length : fieldAttr.TrimLength;
                        columnValue = columnValue.ToString().Substring(0, lengh);
                    }
                }
                if (fieldAttr.CharSetNull && Type.GetType(columnType).Equals(typeof(String)))
                {
                    if (string.IsNullOrEmpty(columnValue.ToString().Trim())) {
                        columnValue = DBNull.Value;
                    }
                }
            }
            return columnValue;
        }

        private String GetFieldDefaultValue(FieldAttr fieldAttr)
        {
            String defaultValue = fieldAttr.DefaultValue;
            Char[] cs = defaultValue.ToCharArray();
            if (cs.Length == 0)
            { return ""; }

            object[] myret = SrvUtils.GetValue(defaultValue, (DataModule)_updateComonent.OwnerComp);
            if (myret != null && (int)myret[0] == 0)
            {
                return (string)myret[1];
            }
            if (cs[0] != '"' && cs[0] != '\'')
            {
                Char[] sep1 = "()".ToCharArray();
                String[] sps1 = defaultValue.Split(sep1);

                if (sps1.Length == 3)
                { return InvokeOwerMethod(sps1[0], null); }

                if (sps1.Length == 1)
                { return sps1[0]; }

                if (sps1.Length != 1 && sps1.Length == 3)
                {
                    String message = SysMsg.GetSystemMessage(((DataModule)_updateComonent.OwnerComp).Language, "Srvtools", "UpdateComponent", "msg_FieldAttrDefaultValueIsBad");
                    throw new ArgumentException(String.Format(message, _updateComonent.Name, fieldAttr.DataField));
                }
            }

            Char[] sep2 = null;
            if (cs[0] == '"')
            { sep2 = "\"".ToCharArray(); }
            if (cs[0] == '\'')
            { sep2 = "'".ToCharArray(); }

            String[] sps2 = defaultValue.Split(sep2);
            if (sps2.Length == 3)
            { return sps2[1]; }
            else
            {
                String message = SysMsg.GetSystemMessage(((DataModule)_updateComonent.OwnerComp).Language, "Srvtools", "UpdateComponent", "msg_FieldAttrDefaultValueIsBad");
                throw new ArgumentException(String.Format(message, _updateComonent.Name, fieldAttr.DataField));
            }
        }

        private String InvokeOwerMethod(String methodName, Object[] parameters)
        {
            MethodInfo methodInfo = _updateComonent.OwnerComp.GetType().GetMethod(methodName);

            Object obj = null;
            if (methodInfo != null)
            { obj = methodInfo.Invoke(_updateComonent.OwnerComp, parameters); }

            if (obj != null)
            { return obj.ToString(); }
            else
            { return ""; }
        }

        private Boolean IsLeftJoinClumns(DataRow schemaRow)
        {
            if (_tableName == "" || _tableName == null || _tableName == String.Empty)
            {
                CreateTableName();
            }

            //String rowTableName = _schema.Rows[index]["BaseTableName"].ToString(); 给表加上owner by Rei
            String rowSchemaName = schemaRow["BaseSchemaName"].ToString();
            String rowTableName = schemaRow["BaseTableName"].ToString();
            if (rowSchemaName != null && rowSchemaName != "")
                rowTableName = rowSchemaName + "." + rowTableName;
            if (string.Compare(_tableName, rowTableName, true) == 0)//IgnoreCase
                return false;
            else
                return true;
        }

        // Transform the marker in columnvalue.   ex:in sql server the "'" is marker
        private Object TransformMarkerInColumnValue(String typeName, Object columnValue)
        {
            if (Type.GetType(typeName).Equals(typeof(Char)) || Type.GetType(typeName).Equals(typeof(String)))
            {
                StringBuilder sb = new StringBuilder();
                if (columnValue.ToString().Length > 0)
                {
                    Char[] cVChars = columnValue.ToString().ToCharArray();

                    foreach (Char cVChar in cVChars)
                    {
                        if (cVChar == _marker)
                        { sb.Append(cVChar.ToString()); }

                        sb.Append(cVChar.ToString());
                    }
                }
                return sb.ToString();
            }
            else
            { return columnValue; }
        }

        private String Mark(String typeName, String type, Object columnValue)
        {
            bool isN = false;
            if (string.Compare(typeName, "NChar", true) == 0 || string.Compare(typeName, "NVarChar", true) == 0
                || string.Compare(typeName, "NText", true) == 0 || string.Compare(typeName, "NClob", true) == 0)//IgnoreCase
            {
                isN = true;
            }
            if (Type.GetType(type).Equals(typeof(Char)) || Type.GetType(type).Equals(typeof(String)) || Type.GetType(type).Equals(typeof(Guid)))
            {
                if (isN)
                    return "N" + _marker.ToString() + columnValue.ToString() + _marker.ToString();
                else
                {

                    if (!string.IsNullOrEmpty(UpdateComponent.SelectCmd.EncodingAfter))
                    {
                        var encodeValue = UpdateComponent.SelectCmd.EncodeString(columnValue.ToString());
                        //var encodeValue = Encoding.GetEncoding(UpdateComponent.SelectCmd.EncodingBefore).GetString(Encoding.GetEncoding(UpdateComponent.SelectCmd.EncodingAfter).GetBytes(columnValue.ToString()));
                        return _marker.ToString() + encodeValue + _marker.ToString();
                    }
                    else
                    {
                        return _marker.ToString() + columnValue.ToString() + _marker.ToString();
                    }
                }
            }
            else if (Type.GetType(type).Equals(typeof(Boolean)))
            {
                Boolean b = (Boolean)columnValue;
                if (b)
                    return "1";
                else
                    return "0";
            }
            else if (Type.GetType(type).Equals(typeof(DateTime)))
            {
                DateTime t = Convert.ToDateTime(columnValue);
                string s = t.Year.ToString() + "-" + t.Month.ToString() + "-" + t.Day.ToString() + " "
                    + t.Hour.ToString() + ":" + t.Minute.ToString() + ":" + t.Second.ToString();
                return _marker.ToString() + s + _marker.ToString();
            }
            else if (Type.GetType(type).Equals(typeof(DateTime)))
            {
                return _marker.ToString() + columnValue.ToString() + _marker.ToString();
            }
            else if (Type.GetType(type).Equals(typeof(Byte[])))
            {
                StringBuilder builder = new StringBuilder("0x");   // 16进制、Oracle里的Binary没有测试。
                foreach (Byte b in (Byte[])columnValue)
                {
                    string tmp = Convert.ToString(b, 16);
                    if (tmp.Length < 2)
                        tmp = "0" + tmp;
                    builder.Append(tmp);
                }
                return builder.ToString();
            }
            else
            {
                return columnValue.ToString();
            }
        }

        private String Mark(String type, Object columnValue)
        {
            if (Type.GetType(type).Equals(typeof(Char)) || Type.GetType(type).Equals(typeof(String)))
            {
                return _marker.ToString() + columnValue.ToString() + _marker.ToString();
            }
            else if (Type.GetType(type).Equals(typeof(Boolean)))
            {
                Boolean b = (Boolean)columnValue;
                if (b)
                    return "1";
                else
                    return "0";
            }
            else if (Type.GetType(type).Equals(typeof(DateTime)))
            {
                String s = "";
                DateTime t = Convert.ToDateTime(columnValue);
                s = t.Year.ToString() + "-" + t.Month.ToString() + "-" + t.Day.ToString() + " "
                    + t.Hour.ToString() + ":" + t.Minute.ToString() + ":" + t.Second.ToString();
                s = "to_date('" + s + "', 'yyyy-mm-dd hh24:mi:ss')";
                //return _marker.ToString() + s + _marker.ToString();
                return s;
            }
            else if (Type.GetType(type).Equals(typeof(Byte[])))
            {
                StringBuilder builder = new StringBuilder("0x");   // 16进制、Oracle里的Binary没有测试。
                foreach (Byte b in (Byte[])columnValue)
                {
                    string tmp = Convert.ToString(b, 16);
                    if (tmp.Length < 2)
                        tmp = "0" + tmp;
                    builder.Append(tmp);
                }
                return builder.ToString();
            }
            else
            {
                return columnValue.ToString();
            }
        }

        private String MarkSybase(String type, Object columnValue)
        {
            if (Type.GetType(type).Equals(typeof(Char)) || Type.GetType(type).Equals(typeof(String)))
            {
                return _marker.ToString() + columnValue.ToString() + _marker.ToString();
            }
            else if (Type.GetType(type).Equals(typeof(Boolean)))
            {
                Boolean b = (Boolean)columnValue;
                if (b)
                    return "1";
                else
                    return "0";
            }
            else if (Type.GetType(type).Equals(typeof(DateTime)))
            {
                String s = "";
                DateTime t = Convert.ToDateTime(columnValue);
                s = t.Year.ToString() + "-" + t.Month.ToString() + "-" + t.Day.ToString() + " "
                    + t.Hour.ToString() + ":" + t.Minute.ToString() + ":" + t.Second.ToString();
                return _marker.ToString() + s + _marker.ToString();
            }
            else if (Type.GetType(type).Equals(typeof(Byte[])))
            {
                StringBuilder builder = new StringBuilder("0x");   // 16进制、Oracle里的Binary没有测试。
                foreach (Byte b in (Byte[])columnValue)
                {
                    string tmp = Convert.ToString(b, 16);
                    if (tmp.Length < 2)
                        tmp = "0" + tmp;
                    builder.Append(tmp);
                }
                return builder.ToString();
            }
            else
            {
                return columnValue.ToString();
            }
        }

        private String MarkMySql(String type, Object columnValue)
        {
            if (Type.GetType(type).Equals(typeof(Char)) || Type.GetType(type).Equals(typeof(String)) || Type.GetType(type).Equals(typeof(TimeSpan)))
            {
                return _marker.ToString() + columnValue.ToString() + _marker.ToString();
            }
            else if (Type.GetType(type).Equals(typeof(Boolean)))
            {
                Boolean b = (Boolean)columnValue;
                if (b)
                    return "1";
                else
                    return "0";
            }
            else if (Type.GetType(type).Equals(typeof(DateTime)))
            {
                String s = "";
                DateTime t = Convert.ToDateTime(columnValue);
                s = "'" + t.Year.ToString() + "-" + t.Month.ToString() + "-" + t.Day.ToString() + " "
                    + t.Hour.ToString() + ":" + t.Minute.ToString() + ":" + t.Second.ToString() + "'";
                return s;
            }
            else if (Type.GetType(type).Equals(typeof(Byte[])))
            {
                StringBuilder builder = new StringBuilder("0x");   // 16进制、Oracle里的Binary没有测试。
                foreach (Byte b in (Byte[])columnValue)
                {
                    string tmp = Convert.ToString(b, 16);
                    if (tmp.Length < 2)
                        tmp = "0" + tmp;
                    builder.Append(tmp);
                }
                return builder.ToString();
            }
            else
            {
                return columnValue.ToString();
            }
        }

        private String MarkOdbc(String type, Object columnValue, OdbcDBType odbcType)
        {
            if (Type.GetType(type).Equals(typeof(Char)) || Type.GetType(type).Equals(typeof(String)))
            {
                return _marker.ToString() + columnValue.ToString() + _marker.ToString();
            }
            else if (Type.GetType(type).Equals(typeof(Boolean)))
            {
                Boolean b = (Boolean)columnValue;
                if (b)
                    return "1";
                else
                    return "0";
            }
            else if (Type.GetType(type).Equals(typeof(DateTime)))
            {
                string s = "";
                if (odbcType == OdbcDBType.InfoMix)
                {
                    DateTime t = Convert.ToDateTime(columnValue);
                    s = t.Year.ToString() + checkDate(t.Month.ToString()) + checkDate(t.Day.ToString()) + checkDate(t.Hour.ToString()) + checkDate(t.Minute.ToString()) + checkDate(t.Second.ToString());
                    s = "to_date('" + s + "', '%Y%m%d%H%M%S')";
                    //return _marker.ToString() + s + _marker.ToString();
                }
                else if (odbcType == OdbcDBType.FoxPro)
                {
                    DateTime t = Convert.ToDateTime(columnValue);
                    s = t.Month.ToString() + "/" + t.Day.ToString() + "/" + t.Year.ToString();
                    s = "{" + s + "}";
                    //return _marker.ToString() + s + _marker.ToString();
                }
                else
                {
                    DateTime t = Convert.ToDateTime(columnValue);
                    s = "'" + t.Month.ToString() + "/" + t.Day.ToString() + "/" + t.Year.ToString() + "'";
                }
                return s;
            }
            else if (Type.GetType(type).Equals(typeof(Byte[])))
            {
                StringBuilder builder = new StringBuilder("0x");   // 16进制、Oracle里的Binary没有测试。
                foreach (Byte b in (Byte[])columnValue)
                {
                    string tmp = Convert.ToString(b, 16);
                    if (tmp.Length < 2)
                        tmp = "0" + tmp;
                    builder.Append(tmp);
                }
                return builder.ToString();
            }
            else
            {
                return columnValue.ToString();
            }
        }

        private String MarkInformix(String type, Object columnValue)
        {
            if (Type.GetType(type).Equals(typeof(Char)) || Type.GetType(type).Equals(typeof(String)))
            {
                return _marker.ToString() + columnValue.ToString() + _marker.ToString();
            }
            else if (Type.GetType(type).Equals(typeof(Boolean)))
            {
                Boolean b = (Boolean)columnValue;
                if (b)
                    return "1";
                else
                    return "0";
            }
            else if (Type.GetType(type).Equals(typeof(DateTime)))
            {
                string s = "";
                DateTime t = Convert.ToDateTime(columnValue);
                s = t.Year.ToString() + checkDate(t.Month.ToString()) + checkDate(t.Day.ToString()) + checkDate(t.Hour.ToString()) + checkDate(t.Minute.ToString()) + checkDate(t.Second.ToString());
                s = "to_date('" + s + "', '%Y%m%d%H%M%S')";
                return s;
            }
            else if (Type.GetType(type).Equals(typeof(Byte[])))
            {
                StringBuilder builder = new StringBuilder("0x");   // 16进制、Oracle里的Binary没有测试。
                foreach (Byte b in (Byte[])columnValue)
                {
                    string tmp = Convert.ToString(b, 16);
                    if (tmp.Length < 2)
                        tmp = "0" + tmp;
                    builder.Append(tmp);
                }
                return builder.ToString();
            }
            else
            {
                return columnValue.ToString();
            }
        }

        public string checkDate(string date)
        {
            if (date.Length < 2)
                date = "0" + date;
            return date;
        }

        // -----------------------------------------------------------------
        // Identity
        public String GetIdenSelectSQLWherePart()
        {
            List<DataRow> columnsList = new List<DataRow>();

            #region get keyfield

            //String keyField = _updateComonent.KeyField;
            ServerModifyColumnCollection SMCs = _updateComonent.ServerModifyColumns;
            if (SMCs == null)
            {
                return null;
            }
            else
            {
                if (SMCs.Count == 0)
                {
                    return null;
                }

                foreach (ServerModifyColumn SM in SMCs)
                {
                    Int32 index = 0;
                    Boolean isExist = false;
                    foreach (DataRow schemaRow in _schema.Rows)
                    {
                        if (IsIncludedInList(schemaRow, columnsList))
                        { isExist = true; continue; }

                        if (IsLeftJoinClumns(schemaRow))
                        { index++; continue; }
                        else
                        { index++; }

                        if (string.Compare(SM.ColumnName, ((String)schemaRow["ColumnName"]), true) != 0)//IgnoreCase
                        { continue; }
                        else
                        {
                            if (!IsIncludedInWhereClause(schemaRow))
                            { continue; }
                            else
                            { isExist = true; columnsList.Add(schemaRow); break; }
                        }
                    }

                    if (isExist == false)
                    {
                        return null;
                    }
                }
            }

            #endregion

            StringBuilder idenSQLWhereSB = new StringBuilder();
            string sql = _updateComonent.SelectCmd.CommandText;
            foreach (DataRow r in columnsList)
            {
                String columnName = r["ColumnName"].ToString();
                String columnType = r["DataType"].ToString();
                String columnTypeName = r["DataTypeName"].ToString();

                Object columnValue = null;
                if (_row.RowState == DataRowState.Deleted)
                { columnValue = _row[columnName, DataRowVersion.Original]; }
                else
                { columnValue = _row[columnName, DataRowVersion.Current]; }

                if (idenSQLWhereSB.Length > 0)
                { idenSQLWhereSB.Append(" and "); }

                columnName = DBUtils.GetTableNameForColumn(sql, columnName, ClientType.ctMsSql);

                if (columnValue == null || columnValue == DBNull.Value)
                { idenSQLWhereSB.Append(columnName + " is null"); }
                else
                { idenSQLWhereSB.Append(columnName + " = " + Mark(columnTypeName, columnType, TransformMarkerInColumnValue(columnType, columnValue))); }
            }

            return idenSQLWhereSB.ToString();
        }

        public String GetIdenSelectSybaseWherePart()
        {
            List<DataRow> columnsList = new List<DataRow>();

            #region get keyfield

            //String keyField = _updateComonent.KeyField;
            ServerModifyColumnCollection SMCs = _updateComonent.ServerModifyColumns;
            if (SMCs == null)
            {
                return null;
            }
            else
            {
                if (SMCs.Count == 0)
                {
                    return null;
                }

                foreach (ServerModifyColumn SM in SMCs)
                {
                    Int32 index = 0;
                    Boolean isExist = false;
                    foreach (DataRow schemaRow in _schema.Rows)
                    {
                        if (IsIncludedInList(schemaRow, columnsList))
                        { isExist = true; continue; }

                        if (IsLeftJoinClumns(schemaRow))
                        { index++; continue; }
                        else
                        { index++; }

                        if (string.Compare(SM.ColumnName, ((String)schemaRow["ColumnName"]), true) != 0)//IgnoreCase
                        { continue; }
                        else
                        {
                            if (!IsIncludedInWhereClause(schemaRow))
                            { continue; }
                            else
                            { isExist = true; columnsList.Add(schemaRow); break; }
                        }
                    }

                    if (isExist == false)
                    {
                        return null;
                    }
                }
            }

            #endregion

            StringBuilder idenSQLWhereSB = new StringBuilder();
            string sql = _updateComonent.SelectCmd.CommandText;
            foreach (DataRow r in columnsList)
            {
                String columnName = r["ColumnName"].ToString();
                String columnType = r["DataType"].ToString();
                //String columnTypeName = r["DataTypeName"].ToString();

                Object columnValue = null;
                if (_row.RowState == DataRowState.Deleted)
                { columnValue = _row[columnName, DataRowVersion.Original]; }
                else
                { columnValue = _row[columnName, DataRowVersion.Current]; }

                if (idenSQLWhereSB.Length > 0)
                { idenSQLWhereSB.Append(" and "); }

                columnName = DBUtils.GetTableNameForColumn(sql, columnName, ClientType.ctMsSql);

                if (columnValue == null || columnValue == DBNull.Value)
                { idenSQLWhereSB.Append(columnName + " is null"); }
                else
                { idenSQLWhereSB.Append(columnName + " = " + MarkSybase(columnType, TransformMarkerInColumnValue(columnType, columnValue))); }
            }

            return idenSQLWhereSB.ToString();
        }

        public String GetIdenSelectOracleWherePart()
        {
            List<DataRow> columnsList = new List<DataRow>();

            #region get keyfield

            //String keyField = _updateComonent.KeyField;
            ServerModifyColumnCollection SMCs = _updateComonent.ServerModifyColumns;
            if (SMCs == null)
            {
                return null;
            }
            else
            {
                if (SMCs.Count == 0)
                {
                    return null;
                }

                foreach (ServerModifyColumn SM in SMCs)
                {
                    Int32 index = 0;
                    Boolean isExist = false;
                    foreach (DataRow schemaRow in _schema.Rows)
                    {
                        if (IsIncludedInList(schemaRow, columnsList))
                        { isExist = true; continue; }

                        if (IsLeftJoinClumns(schemaRow))
                        { index++; continue; }
                        else
                        { index++; }

                        if (string.Compare(SM.ColumnName, ((String)schemaRow["ColumnName"]), true) != 0)//IgnoreCase
                        { continue; }
                        else
                        {
                            if (!IsIncludedInWhereClause(schemaRow))
                            { continue; }
                            else
                            { isExist = true; columnsList.Add(schemaRow); break; }
                        }
                    }

                    if (isExist == false)
                    {
                        return null;
                    }
                }
            }

            #endregion

            StringBuilder idenSQLWhereSB = new StringBuilder();
            foreach (DataRow r in columnsList)
            {
                String columnName = r["ColumnName"].ToString();
                String columnType = r["DataType"].ToString();
                //String columnTypeName = r["DataTypeName"].ToString();

                Object columnValue = null;
                if (_row.RowState == DataRowState.Deleted)
                { columnValue = _row[columnName, DataRowVersion.Original]; }
                else
                { columnValue = _row[columnName, DataRowVersion.Current]; }

                if (idenSQLWhereSB.Length > 0)
                { idenSQLWhereSB.Append(" and "); }

                if (columnValue == null || columnValue == DBNull.Value)
                { idenSQLWhereSB.Append(columnName + " is null"); }
                else
                { idenSQLWhereSB.Append(columnName + " = " + Mark(columnType, TransformMarkerInColumnValue(columnType, columnValue))); }
                //{ idenSQLWhereSB.Append(Quote(columnName) + " = " + Mark(columnTypeName, columnType, TransformMarkerInColumnValue(columnType, columnValue))); }
            }

            return idenSQLWhereSB.ToString();
        }
        // -------------------------------------------------------------------

        #endregion

        #region Vars

        private String _quotePrefix = "[";
        private String _quoteSuffix = "]";
        private Char _marker = '\'';
        private UpdateComponent _updateComonent;
        private DataRow _row;

        private DataTable _schema;
        private String _tableName;
        private String _schemaName;

        private String _insertSQL;
        private String _updateSQL;
        private String _deleteSQL;

        // private String _provider = "SQLOLEDB";

        #endregion
    }
}
