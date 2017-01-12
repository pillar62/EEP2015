using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data;

namespace Srvtools
{
    public class SetFieldValueSynchronizer
    {
        private String _tableName;
        private DataRow _row;
        private DataTable _schema;
        private Hashtable _fieldValuesList;
        private DataTable _dataTable;

        private String _quotePrefix = "[";
        private String _quoteSuffix = "]";
        private Char _marker = '\'';

        public SetFieldValueSynchronizer(DataTable dataTable, DataRow row, DataTable schema, Hashtable FieldValuesList)
        {
            _row = row;
            _schema = schema;
            _fieldValuesList = FieldValuesList;
            _dataTable = dataTable;
        }

        public void Sync()
        {
            if (_fieldValuesList == null || _fieldValuesList.Count == 0)
            { return; }

            StringBuilder sb = new StringBuilder();

            Int32 index = 0;
            foreach (DataRow r in _schema.Rows)
            {
                if (IsLeftJoinClumns(index))
                { index++; continue; }
                else
                {
                    index++;
                    String columnName = r["ColumnName"].ToString();
                    String columnType = r["DataType"].ToString();
                    if(object.Equals(r["DataType"], typeof(byte[])))
                    {
                        continue;
                    }
                    if (object.Equals(r["DataType"], typeof(TimeSpan)))
                    {

                    }
                    else
                    {
                        if (sb.Length > 0)
                        {
                            sb.Append(" and ");
                        }

                        sb.Append(Quote(columnName));

                        Object columnValue = _row[r["ColumnName"].ToString()];
                        if (columnValue == null || columnValue == DBNull.Value)
                        {
                            sb.Append(" is null");
                        }
                        else
                        {
                            sb.Append("=" + Mark(columnType, TransformMarkerInColumnValue(columnType, columnValue)));
                        }
                    }
                }

                foreach (object a in _fieldValuesList.Keys)
                {
                    if (string.Compare(a.ToString(), r["ColumnName"].ToString(), true) == 0)//IgnoreCase
                    {
                        if ((Boolean)r["IsAutoIncrement"] == true || (Boolean)r["IsReadOnly"] == true)
                        { continue; }
                        else
                        {
                            _row[r["ColumnName"].ToString()] = _fieldValuesList[a];
                        }
                    }
                    else
                    { continue; }
                }
            }

            String filterPart = sb.ToString();
            DataRow[] realRows = _dataTable.Select(filterPart);
            if (realRows.Length == 1)
            {
                SyncRealRow(realRows[0], _row);
            }
            else
            {
                // error.
                throw new Exception("InfoCommand must has key.");
            }
        }

        public void SyncOracle()
        {
            if (_fieldValuesList == null || _fieldValuesList.Count == 0)
            { return; }

            StringBuilder sb = new StringBuilder();

            Int32 index = 0;
            foreach (DataRow r in _schema.Rows)
            {
                if (IsLeftJoinClumns(index))
                { index++; continue; }
                else
                {
                    index++;
                    String columnName = r["ColumnName"].ToString();
                    String columnType = r["DataType"].ToString();
                    if (object.Equals(r["DataType"], typeof(byte[])))
                    {
                        continue;
                    }
                    if (sb.Length > 0)
                    {
                        sb.Append(" and ");
                    }

                    sb.Append(Quote(columnName));

                    Object columnValue = _row[r["ColumnName"].ToString()];
                    if (columnValue == null || columnValue == DBNull.Value)
                    {
                        sb.Append(" is null");
                    }
                    else
                    {
                        sb.Append("=" + Mark(columnType, TransformMarkerInColumnValue(columnType, columnValue)));
                    }
                }

                foreach (object a in _fieldValuesList.Keys)
                {
                    if (string.Compare(a.ToString(), r["ColumnName"].ToString(), true) == 0)//IgnoreCase
                    {
                        //if ((Boolean)r["IsAutoIncrement"] == true || (Boolean)r["IsReadOnly"] == true)
                        //{ continue; }
                        //else
                        //{
                            _row[r["ColumnName"].ToString()] = _fieldValuesList[a];
                        //}
                    }
                    else
                    { continue; }
                }
            }

            String filterPart = sb.ToString();
            DataRow[] realRows = _dataTable.Select(filterPart);
            if (realRows.Length == 1)
            {
                SyncRealRow(realRows[0], _row);
            }
            else
            {
                // error.
                throw new Exception("InfoCommand must has key.");
            }
        }

        private Boolean IsLeftJoinClumns(Int32 index)
        {
            if (_tableName == "" || _tableName == null || _tableName == String.Empty)
            {
                CreateTableName();
            }

            String rowTableName = _schema.Rows[index]["BaseTableName"].ToString();
            if (string.Compare(_tableName, rowTableName, true) == 0)//IgnoreCase
                return false;
            else
                return true;
        }

        private void CreateTableName()
        {
            if (_tableName != null && _tableName.Length > 0) return;

            _tableName = _schema.Rows[0]["BaseTableName"].ToString();
        }

        private void SyncRealRow(DataRow realRow, DataRow row)
        {
            Int32 index = 0;
            foreach (DataRow r in _schema.Rows)
            {
                if (IsLeftJoinClumns(index))
                { index++; continue; }
                else
                {
                    index++;
                }

                realRow[r["ColumnName"].ToString()] = row[r["ColumnName"].ToString()];
            }
        }

        private String Quote(String table_or_column)
        {
            if (_quotePrefix == null || _quoteSuffix == null)
                return table_or_column;
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

        private String Mark(String type, Object columnValue)
        {
            //if (Type.GetType(type).Equals(typeof(Char)) || Type.GetType(type).Equals(typeof(String)))
            if (Type.GetType(type).Equals(typeof(Char)) || Type.GetType(type).Equals(typeof(String)) || Type.GetType(type).Equals(typeof(Guid)) || Type.GetType(type).Equals(typeof(TimeSpan)))
            {
                return _marker.ToString() + columnValue.ToString() + _marker.ToString();
            }
            else if (Type.GetType(type).Equals(typeof(DateTime)))
            {
                DateTime t = Convert.ToDateTime(columnValue);
                string s = t.ToString("yyyy-MM-dd ") + t.TimeOfDay.ToString();
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
    }
}