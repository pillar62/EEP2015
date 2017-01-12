using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Reflection;

namespace AjaxTools
{
    public static class JsonSerializer
    {
        public static string TableToJavascriptArray(DataTable dt, string[] fields)
        {
            StringBuilder JsonString = new StringBuilder();
            if (dt != null && dt.Rows.Count > 0)
            {
                JsonString.Append("[");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    JsonString.Append("[");
                    for (int j = 0; j < fields.Length; j++)
                    {
                        object fieldValue = dt.Rows[i][fields[j]];
                        if (GloFix.IsNumeric(fieldValue.GetType()))
                        {
                            JsonString.Append(fieldValue.ToString() + ((j < fields.Length - 1) ? "," : ""));
                        }
                        else
                        {
                            JsonString.Append("'" + GloFix.FormatESC(fieldValue.ToString()) + ((j < fields.Length - 1) ? "'," : "'"));
                        }
                    }
                    if (i < dt.Rows.Count - 1)
                    {
                        JsonString.Append("],");
                    }
                    else
                    {
                        JsonString.Append("]");
                    }
                }
                JsonString.Append("]");
                return JsonString.ToString();
            }
            return "";
        }

        public static string TableToJsonArray(DataTable dt)
        {
            List<string> fields = new List<string>();
            foreach (DataColumn column in dt.Columns)
            {
                fields.Add(column.ColumnName);
            }
            return TableToJsonArray(dt, fields.ToArray());
        }

        public static string TableToJsonArray(DataTable dt, string[] fields)
        {
            List<DataRow> rows = new List<DataRow>();
            foreach(DataRow row in dt.Rows)
            {
                if (row.RowState != DataRowState.Deleted)
                {
                    rows.Add(row);
                }
            }
            return RowsToJsonArray(rows.ToArray(), fields);
        }

        public static string RowsToJsonArray(DataRow[] rows, string[] fields)
        {
            StringBuilder JsonString = new StringBuilder();
            JsonString.Append("[");
            if (rows != null && rows.Length > 0)
            {
                for (int i = 0; i < rows.Length; i++)
                {
                    JsonString.Append(RowToJson(rows[i], fields));
                    if (i < rows.Length - 1)
                    {
                        JsonString.Append(",");
                    }
                }
            }
            JsonString.Append("]");
            return JsonString.ToString();
        }

        public static string RowToJson(DataRow row, string[] fields)
        {
            StringBuilder JsonString = new StringBuilder();
            JsonString.Append("{");
            for (int j = 0; j < fields.Length; j++)
            {
                Type fieldDataType = row[fields[j]].GetType();
                if (GloFix.IsNumeric(fieldDataType))
                {
                    JsonString.Append(string.Format("{0}:{1}", fields[j], row[fields[j]].ToString()));
                }
                else if (fieldDataType == typeof(Boolean))
                {
                    JsonString.Append(string.Format("{0}:{1}", fields[j], row[fields[j]].ToString().ToLower()));
                }
                else if (fieldDataType == typeof(DateTime))
                {
                    object date = row[fields[j]];
                    if (date != null)
                    {
                        // 數據日期格式存在bug,似乎數據源只支持yyyy/mm/dd格式的日期(寫在JsonSerializer中RowsToJsonArray方法)
                        string dt = ((DateTime)row[fields[j]]).ToString("yyyy/MM/dd HH:mm:ss", System.Globalization.CultureInfo.CreateSpecificCulture("en-us"));
                        //JsonString.Append(string.Format("{0}:'{1}'", fields[j], dt));
                        JsonString.Append(string.Format("{0}:new Date('{1}')", fields[j], dt));
                    }
                    else
                    {
                        JsonString.Append(string.Format("{0}:''", fields[j]));
                    }
                }
                else
                {
                    JsonString.Append(string.Format("{0}:'{1}'", fields[j], GloFix.FormatESC(row[fields[j]].ToString())));
                }
                JsonString.Append((j < fields.Length - 1) ? "," : "");
            }
            JsonString.Append("}");
            return JsonString.ToString();
        }
    }
}
