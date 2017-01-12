using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using System.Data.SqlClient;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.OracleClient;
using System.Reflection;
#if MySql
using MySql.Data.MySqlClient;
#endif

namespace Srvtools
{
    public class LogSQLBuilder
    {

        public LogSQLBuilder(LogInfo logInfo, DataRow srcRow, DataTable srcSchema, KeyItems srcKeysList)
        {
            _srcRow = srcRow;
            _srcSchema = srcSchema;
            _logInfo = logInfo;
            _keysList = srcKeysList;

            _columnsBuilder = new StringBuilder();
            _valuesBuilder = new StringBuilder();
        }

        private IDbConnection _globleConn = null;
        private String LogTableName
        {
            get
            {
                if (_logTableName == null)
                {
                    if (LogSchemaName != null)
                    {
                        _logTableName = LogSchemaName + "." + Quote(_logInfo.LogTableName);
                    }
                    else
                    {
                        _logTableName = Quote(_logInfo.LogTableName);
                    }
                }

                return _logTableName;
            }
        }

        private String LogSchemaName
        {
            get
            {
                if (_logTableName != null)
                {
                    _logSchemaName = Quote(_logTableSchema.Rows[0]["BaseSchemaName"].ToString());
                }

                return _logSchemaName;
            }
        }

        public String GetLogSQL(IDbConnection connection)
        {
            return GetLogSQL(connection, null);
        }

        public String GetLogSQL(IDbConnection connection, IDbTransaction dbTrans)
        {
            _globleConn = connection;
            String _logSQL = "";

            if (_logInfo.NeedLog == false) return _logSQL;
            GenerateLogTableSchema(_logInfo.LogTableName, connection, dbTrans);

            if (connection is OracleConnection)
                AppendLogColumnsListOracle();
            else
                AppendLogColumnsList();

            // ModifierField、ModifyDateField、MarkField

            // Is Include.
            //if (_logInfo.LogDateType != null)
            //{
            //    if (!IsIncludedInSchema(_logInfo.LogDateType, _logTableSchema))
            //    {
            //        Object[] clientInfos = ((DataModule)_logInfo.OwnerComp).ClientInfo;
            //        String message = SysMsg.GetSystemMessage(((DataModule)_logInfo.OwnerComp).Language, "Srvtools", "LogInfo", "msg_LogDateTypeIsBad");
            //        throw new ArgumentException(String.Format(message, _logInfo.Name, _logInfo.LogDateType));
            //    }
            //}

            if (!IsIncludedInSchema(_logInfo.ModifierField, _logTableSchema))
            {
                String message = SysMsg.GetSystemMessage(((DataModule)_logInfo.OwnerComp).Language, "Srvtools", "LogInfo", "msg_ModifierFieldNotExistInTable");
                throw new ArgumentException(String.Format(message, _logInfo.Name, _logInfo.LogIDField));
            }

            if (!IsIncludedInSchema(_logInfo.MarkField, _logTableSchema))
            {
                String message = SysMsg.GetSystemMessage(((DataModule)_logInfo.OwnerComp).Language, "Srvtools", "LogInfo", "msg_MarkFieldIsNull");
                throw new ArgumentException(String.Format(message, _logInfo.Name, _logInfo.LogIDField));
            }

            if (!IsIncludedInSchema(_logInfo.ModifyDateField, _logTableSchema))
            {
                String message = SysMsg.GetSystemMessage(((DataModule)_logInfo.OwnerComp).Language, "Srvtools", "LogInfo", "msg_ModifyDateFieldIsNull");
                throw new ArgumentException(String.Format(message, _logInfo.Name, _logInfo.LogIDField));
            }



            if (connection is OracleConnection)
            {
                AppendModifyDateColumnOracle();
                AppendMarkColumnOracle();
                AppendModifierColumnOracle();
            }
            else
            {
                // IsAutoIncrement.
                if (!IsAutoIncrement(_logInfo.LogIDField, _logTableSchema))
                {
                    String message = SysMsg.GetSystemMessage(((DataModule)_logInfo.OwnerComp).Language, "Srvtools", "LogInfo", "msg_LogIDFieldIsNotAutoIncrement");
                    throw new ArgumentException(String.Format(message, _logInfo.Name, _logInfo.LogIDField));
                }
                AppendModifyDateColumn();
                AppendMarkColumn();
                AppendModifierColumn();
            }

            return "insert into " + LogTableName + "(" + _columnsBuilder.ToString() + ") values(" + _valuesBuilder.ToString() + ")";
        }

        // 类型也必须一样。
        private List<DataRow> AppendLogColumnsList()
        {
            //String[] logColumns = _logInfo.SrcFieldNames;
            SrcFieldNameCollection logColumns = _logInfo.SrcFieldNames;
            Int32 columnCount = 0;
            if (logColumns != null)
            //{ columnCount = logColumns.Length; }
            { columnCount = logColumns.Count; }

            List<DataRow> logColumnsList = new List<DataRow>();
            Boolean isNotColumns = logColumns.Count == 0;
            //foreach (DataRow c in logColumnsList)
            //{
            //    String name = c["ColumnName"].ToString();
            //    if (name != null && name.Length != 0)
            //    {
            //        isNotColumns = false;
            //    }
            //}

            if (isNotColumns == true)
            {
                foreach (DataRow r in _srcSchema.Rows)
                {
                    String columnName = r["ColumnName"].ToString();
                    if (!IsIncludedInSchema(columnName, _logTableSchema))
                    {
                        String message = SysMsg.GetSystemMessage(((DataModule)_logInfo.OwnerComp).Language, "Srvtools", "LogInfo", "msg_ColumnNotExistInTable");
                        throw new ArgumentException(String.Format(message, columnName, _logInfo.LogTableName));
                    }

                    if (_srcRow.RowState == DataRowState.Modified && _logInfo.OnlyDistinct == true)
                    {
                        String oV = _srcRow[columnName, DataRowVersion.Original].ToString();
                        String nV = _srcRow[columnName, DataRowVersion.Current].ToString();

                        if (IsKey(r) == false && oV == nV)
                        { continue; }
                    }
                    logColumnsList.Add(r);
                }
            }
            else
            {
                for (Int32 i = 0; i < columnCount; i++)
                {
                    //String columnName = logColumns[i];
                    String columnName = ((SrcFieldNameColumn)logColumns[i]).FieldName;
                    DataRow f1 = GetFieldInSchema(columnName, _logTableSchema);
                    DataRow f2 = GetFieldInSchema(columnName, _srcSchema);

                    if (f1 == null)
                    {
                        String message = SysMsg.GetSystemMessage(((DataModule)_logInfo.OwnerComp).Language, "Srvtools", "LogInfo", "msg_ColumnNotExistInTable");
                        throw new ArgumentException(String.Format(message, columnName, _logInfo.LogTableName));
                    }

                    if (f2 == null)
                    {
                        String message = SysMsg.GetSystemMessage(((DataModule)_logInfo.OwnerComp).Language, "Srvtools", "LogInfo", "msg_ColumnNotExistInTable");
                        throw new ArgumentException(String.Format(message, columnName, _srcSchema.Rows[0]["BaseTableName"].ToString()));
                    }

                    if (_srcRow.RowState == DataRowState.Modified && _logInfo.OnlyDistinct == true)
                    {
                        String oV = _srcRow[columnName, DataRowVersion.Original].ToString();
                        String nV = _srcRow[columnName, DataRowVersion.Current].ToString();

                        if (oV == nV)
                        {
                            continue;
                        }
                    }
                    logColumnsList.Add(f1);
                }
            }

            foreach (DataRow c in logColumnsList)
            {
                String columnName = c["ColumnName"].ToString();
                String columnType = c["DataType"].ToString();
                String columnTypeName = c["DataTypeName"].ToString();
                Object columnValue = "";

                if (_srcRow.RowState == DataRowState.Added)
                {
                    columnValue = _srcRow[columnName, DataRowVersion.Current];
                }

                if (_srcRow.RowState == DataRowState.Modified)
                {
                    columnValue = _srcRow[columnName, DataRowVersion.Original];
                }

                if (_srcRow.RowState == DataRowState.Deleted)
                {
                    if (_srcRow[columnName, DataRowVersion.Original] != null || _srcRow[columnName, DataRowVersion.Original] != DBNull.Value)
                    {
                        columnValue = _srcRow[columnName, DataRowVersion.Original];
                    }
                    else
                    {
                        columnValue = _srcRow[columnName, DataRowVersion.Current];
                    }
                }

                if (_columnsBuilder.Length > 0)
                {
                    _columnsBuilder.Append(", "); _valuesBuilder.Append(", ");
                }

                _columnsBuilder.Append(Quote(columnName));

                if (columnValue == null || columnValue == DBNull.Value)
                {
                    _valuesBuilder.Append("null");
                }
                else
                {
                    _valuesBuilder.Append(Mark(columnTypeName, columnType, TransformMarkerInColumnValue(columnType, columnValue)));
                }
            }

            return logColumnsList;
        }

        private List<DataRow> AppendLogColumnsListOracle()
        {
            //String[] logColumns = _logInfo.SrcFieldNames;
            SrcFieldNameCollection logColumns = _logInfo.SrcFieldNames;
            Int32 columnCount = 0;
            if (logColumns != null)
            //{ columnCount = logColumns.Length; }
            { columnCount = logColumns.Count; }

            List<DataRow> logColumnsList = new List<DataRow>();
            Boolean isNotColumns = logColumns.Count == 0;
            //foreach (DataRow c in logColumnsList)
            //{
            //    String name = c["ColumnName"].ToString();
            //    if (name != null && name.Length != 0)
            //    {
            //        isNotColumns = false;
            //    }
            //}

            if (isNotColumns == true)
            {
                foreach (DataRow r in _srcSchema.Rows)
                {
                    String columnName = r["ColumnName"].ToString();
                    if (!IsIncludedInSchema(columnName, _logTableSchema))
                    {
                        String message = SysMsg.GetSystemMessage(((DataModule)_logInfo.OwnerComp).Language, "Srvtools", "LogInfo", "msg_ColumnNotExistInTable");
                        throw new ArgumentException(String.Format(message, columnName, _logInfo.LogTableName));
                    }

                    if (_srcRow.RowState == DataRowState.Modified && _logInfo.OnlyDistinct == true)
                    {
                        String oV = _srcRow[columnName, DataRowVersion.Original].ToString();
                        String nV = _srcRow[columnName, DataRowVersion.Current].ToString();

                        if (IsKey(r) == false && oV == nV)
                        { continue; }
                    }
                    logColumnsList.Add(r);
                }
            }
            else
            {
                for (Int32 i = 0; i < columnCount; i++)
                {
                    //String columnName = logColumns[i];
                    String columnName = ((SrcFieldNameColumn)logColumns[i]).FieldName;
                    DataRow f1 = GetFieldInSchema(columnName, _logTableSchema);
                    DataRow f2 = GetFieldInSchema(columnName, _srcSchema);

                    if (f1 == null)
                    {
                        String message = SysMsg.GetSystemMessage(((DataModule)_logInfo.OwnerComp).Language, "Srvtools", "LogInfo", "msg_ColumnNotExistInTable");
                        throw new ArgumentException(String.Format(message, columnName, _logInfo.LogTableName));
                    }

                    if (f2 == null)
                    {
                        String message = SysMsg.GetSystemMessage(((DataModule)_logInfo.OwnerComp).Language, "Srvtools", "LogInfo", "msg_ColumnNotExistInTable");
                        throw new ArgumentException(String.Format(message, columnName, _srcSchema.Rows[0]["BaseTableName"].ToString()));
                    }

                    if (_srcRow.RowState == DataRowState.Modified && _logInfo.OnlyDistinct == true)
                    {
                        String oV = _srcRow[columnName, DataRowVersion.Original].ToString();
                        String nV = _srcRow[columnName, DataRowVersion.Current].ToString();

                        if (oV == nV)
                        {
                            continue;
                        }
                    }
                    logColumnsList.Add(f1);
                }
            }

            foreach (DataRow c in logColumnsList)
            {
                String columnName = c["ColumnName"].ToString();
                String columnType = c["DataType"].ToString();
                //String columnTypeName = c["DataTypeName"].ToString();
                Object columnValue = "";

                if (_srcRow.RowState == DataRowState.Added)
                {
                    columnValue = _srcRow[columnName, DataRowVersion.Current];
                }

                if (_srcRow.RowState == DataRowState.Modified)
                {
                    columnValue = _srcRow[columnName, DataRowVersion.Original];
                }

                if (_srcRow.RowState == DataRowState.Deleted)
                {
                    if (_srcRow[columnName, DataRowVersion.Original] != null || _srcRow[columnName, DataRowVersion.Original] != DBNull.Value)
                    {
                        columnValue = _srcRow[columnName, DataRowVersion.Original];
                    }
                    else
                    {
                        columnValue = _srcRow[columnName, DataRowVersion.Current];
                    }
                }

                if (_columnsBuilder.Length > 0)
                {
                    _columnsBuilder.Append(", "); _valuesBuilder.Append(", ");
                }

                _columnsBuilder.Append(Quote(columnName));

                if (columnValue == null || columnValue == DBNull.Value)
                {
                    _valuesBuilder.Append("null");
                }
                else
                {
                    _valuesBuilder.Append(MarkOracle(columnType, TransformMarkerInColumnValue(columnType, columnValue)));
                }
            }

            return logColumnsList;
        }

        private void AppendModifyDateColumn()
        {
            String columnValue = "";
            String columnType = "System.DateTime";
            String columnTypeName = "datetime";

            if (_logInfo.LogDateType != null && _logInfo.LogDateType != "" && _logInfo.LogDateType != String.Empty)
            {
                columnValue = DateTime.Now.ToString(_logInfo.LogDateType);
            }
            else
            {
                columnValue = DateTime.Now.ToString();
            }

            if (_columnsBuilder.Length > 0)
            {
                _columnsBuilder.Append(", "); _valuesBuilder.Append(", ");
            }

            _columnsBuilder.Append(Quote(_logInfo.ModifyDateField));
            _valuesBuilder.Append(Mark(columnTypeName, columnType, TransformMarkerInColumnValue(columnType, columnValue)));
        }

        private void AppendModifyDateColumnOracle()
        {
            String columnValue = "";
            String columnType = "System.DateTime";

            if (_logInfo.LogDateType != null && _logInfo.LogDateType != "" && _logInfo.LogDateType != String.Empty)
            {
                columnValue = DateTime.Now.ToString(_logInfo.LogDateType);
            }
            else
            {
                columnValue = DateTime.Now.ToString();
            }

            if (_columnsBuilder.Length > 0)
            {
                _columnsBuilder.Append(", "); _valuesBuilder.Append(", ");
            }

            _columnsBuilder.Append(Quote(_logInfo.ModifyDateField));
            _valuesBuilder.Append(MarkOracle(columnType, TransformMarkerInColumnValue(columnType, columnValue)));
        }

        private void AppendMarkColumn()
        {
            String columnValue = "";
            String columnType = "System.String";
            String columnTypeName = "char";

            if (_srcRow.RowState == DataRowState.Added) columnValue = "I";
            if (_srcRow.RowState == DataRowState.Modified) columnValue = "M";
            if (_srcRow.RowState == DataRowState.Deleted) columnValue = "D";

            if (_columnsBuilder.Length > 0)
            {
                _columnsBuilder.Append(", "); _valuesBuilder.Append(", ");
            }

            _columnsBuilder.Append(Quote(_logInfo.MarkField));
            _valuesBuilder.Append(Mark(columnTypeName, columnType, TransformMarkerInColumnValue(columnType, columnValue)));
        }

        private void AppendMarkColumnOracle()
        {
            String columnValue = "";
            String columnType = "System.String";

            if (_srcRow.RowState == DataRowState.Added) columnValue = "I";
            if (_srcRow.RowState == DataRowState.Modified) columnValue = "M";
            if (_srcRow.RowState == DataRowState.Deleted) columnValue = "D";

            if (_columnsBuilder.Length > 0)
            {
                _columnsBuilder.Append(", "); _valuesBuilder.Append(", ");
            }

            _columnsBuilder.Append(Quote(_logInfo.MarkField));
            _valuesBuilder.Append(MarkOracle(columnType, TransformMarkerInColumnValue(columnType, columnValue)));
        }

        private void AppendModifierColumn()
        {
            String columnType = "System.String";
            String columnValue = null;
            String columnTypeName = "varchar";

            if (_columnsBuilder.Length > 0)
            {
                _columnsBuilder.Append(", "); _valuesBuilder.Append(", ");
            }
            string user = (string)((DataModule)_logInfo.OwnerComp).GetClientInfo(ClientInfoType.LoginUser);
            if (!string.IsNullOrEmpty(user))
            {
                columnValue = user;
            }

            // if (_logInfo.Modifier != "" && _logInfo.Modifier != null && _logInfo.Modifier != String.Empty)
            //if (_logInfo.Modifier != 0)
            //{
            _columnsBuilder.Append(Quote(_logInfo.ModifierField));
            _valuesBuilder.Append(Mark(columnTypeName, columnType, TransformMarkerInColumnValue(columnType, columnValue)));
            //}
        }

        private void AppendModifierColumnOracle()
        {
            String columnType = "System.String";
            String columnValue = null;

            if (_columnsBuilder.Length > 0)
            {
                _columnsBuilder.Append(", "); _valuesBuilder.Append(", ");
            }
            string user = (string)((DataModule)_logInfo.OwnerComp).GetClientInfo(ClientInfoType.LoginUser);
            if (!string.IsNullOrEmpty(user))
            {
                columnValue = user;
            }

            // if (_logInfo.Modifier != "" && _logInfo.Modifier != null && _logInfo.Modifier != String.Empty)
            //if (_logInfo.Modifier != 0)
            //{
            _columnsBuilder.Append(Quote(_logInfo.ModifierField));
            _valuesBuilder.Append(MarkOracle(columnType, TransformMarkerInColumnValue(columnType, columnValue)));
            //}
        }

        private DataTable GenerateLogTableSchema(String tableName, IDbConnection connection, IDbTransaction dbTrans)
        {
            if (_logTableSchema != null) return _logTableSchema;

            String sQL = "select * from " + Quote(tableName) + " where 1 <> 1";

            IDbCommand command = LocateCommand(sQL, connection);
            if (dbTrans != null) command.Transaction = dbTrans;

            IDataReader reader = command.ExecuteReader(CommandBehavior.SchemaOnly | CommandBehavior.KeyInfo);

            DataTable dataTable = reader.GetSchemaTable();

            reader.Close();

            if (dataTable != null)
            {
                _logTableSchema = dataTable;
                return dataTable;
            }
            else
            {
                return null;
            }
        }

        // 有可能有两种Key，一种是DB的Key，一种是我们UpdateComponent的KeyField。
        private Boolean IsKey(DataRow field)
        {
            if (_keysList != null && _keysList.Count != 0)
            {
                foreach (KeyItem key in _keysList)
                {
                    if (string.Compare(field["ColumnName"].ToString(), key.KeyName, true) == 0)//IgnoreCase
                    { return true; }
                }
            }

            if ((Boolean)field["IsKey"] == true)
            { return true; }
            else
            { return false; }
        }

        private static Boolean IsAutoIncrement(String fieldName, DataTable schema)
        {
            foreach (DataRow schemaRow in schema.Rows)
            {
                String columnName = (String)schemaRow["ColumnName"];

                if (string.Compare(columnName, fieldName, true) == 0)//IgnoreCase
                {
                    if ((bool)schemaRow["IsAutoIncrement"])
                        return true;
                }
            }
            return false;
        }

        private static Boolean IsIncludedInSchema(String field, DataTable schema)
        {
            foreach (DataRow schemaRow in schema.Rows)
            {
                String columnName = (String)schemaRow["ColumnName"];

                if (string.Compare(columnName, field, true) == 0)//IgnoreCase
                {
                    return true;
                }
            }
            return false;
        }

        private static DataRow GetFieldInSchema(String field, DataTable schema)
        {
            foreach (DataRow schemaRow in schema.Rows)
            {
                String columnName = (String)schemaRow["ColumnName"];

                if (string.Compare(columnName, field, true) == 0)//IgnoreCase
                {
                    return schemaRow;
                }
            }
            return null;
        }


        private IDbCommand LocateCommand(string sQL, IDbConnection connection)
        {
            if (connection is SqlConnection)
                return new SqlCommand(sQL, (SqlConnection)connection);
            else if (connection is OdbcConnection)
                return new OdbcCommand(sQL, (OdbcConnection)connection);
            else if (connection is OracleConnection)
                return new OracleCommand(sQL, (OracleConnection)connection);
            else if (connection is OleDbConnection)
                return new OleDbCommand(sQL, (OleDbConnection)connection);
#if MySql
            else if (connection.GetType().Name == "MySqlConnection")
            {
                return new MySqlCommand(sQL, (MySqlConnection)connection);
                //Assembly assembly = Assembly.Load("MySql.Data");
                //IDbCommand cmd = assembly.CreateInstance("MySql.Data.MySqlClient.MySqlCommand") as IDbCommand;
                //cmd.CommandText = sQL;
                //cmd.Connection = connection;
                //return cmd;
            }
#endif
#if Informix
            else if (connection.GetType().Name == "IfxConnection")
            {
                return new IBM.Data.Informix.IfxCommand(sQL, (IBM.Data.Informix.IfxConnection)connection);
            }
#endif
#if Sybase
            else if (connection.GetType().Name == "AseConnection")
            {
                return new Sybase.Data.AseClient.AseCommand(sQL, (Sybase.Data.AseClient.AseConnection)connection);
            }
#endif
            else return null;
        }

        private String Quote(String table_or_column)
        {
            if (_globleConn == null) return _quotePrefix + table_or_column + _quoteSuffix;
            IDbConnection conn = _globleConn;
            if (conn is SqlConnection)
            {
                if (_quotePrefix == null || _quoteSuffix == null)
                    return table_or_column;
                return _quotePrefix + table_or_column + _quoteSuffix;
            }
            else if (conn is OracleConnection)
            {
                return table_or_column;
            }
            else if (conn is OdbcConnection)
            {
                return table_or_column;
            }
            else if (conn is OleDbConnection)
            {
                return table_or_column;
            }
            else if (conn.GetType().Name == "MySqlConnection")
            {
                return table_or_column;
            }
            else if (conn.GetType().Name == "IfxConnection")
            {
                return table_or_column;
            }
            return _quotePrefix + table_or_column + _quoteSuffix;
        }

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

        // 现在Array类型的只考虑了Byte[]（DB里对应的类型为Binary）。
        private String Mark(String typeName, String type, Object columnValue)
        {
            bool isN = false;
            if (string.Compare(typeName, "NChar", true) == 0 || string.Compare(typeName, "NVarChar", true) == 0
                   || string.Compare(typeName, "NText", true) == 0 || string.Compare(typeName, "NClob", true) == 0)//IgnoreCase
            {
                isN = true;
            }

            if (Type.GetType(type).Equals(typeof(Char)) || Type.GetType(type).Equals(typeof(String)))
            {
                if (isN)
                    return "N" + _marker.ToString() + columnValue.ToString() + _marker.ToString();
                else
                    return _marker.ToString() + columnValue.ToString() + _marker.ToString();
            }
            else if (Type.GetType(type).Equals(typeof(DateTime)))
            {
                DateTime t = Convert.ToDateTime(columnValue);
                string s = t.Year.ToString() + "-" + t.Month.ToString() + "-" + t.Day.ToString() + " "
                    + t.Hour.ToString() + ":" + t.Minute.ToString() + ":" + t.Second.ToString();
                return _marker.ToString() + s + _marker.ToString();
            }
            else if (Type.GetType(type).Equals(typeof(Boolean)))
            {
                Boolean b = (Boolean)columnValue;
                if (b)
                    return "1";
                else
                    return "0";
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

        private String MarkOracle(String type, Object columnValue)
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

        private String _quotePrefix = "[";
        private String _quoteSuffix = "]";
        private Char _marker = '\'';

        private StringBuilder _columnsBuilder;
        private StringBuilder _valuesBuilder;

        private LogInfo _logInfo;
        private DataTable _logTableSchema = null;
        private String _logTableName = null;
        private String _logSchemaName = null;

        private DataRow _srcRow;
        private DataTable _srcSchema;
        private KeyItems _keysList;
    }
}
