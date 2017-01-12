<%@ WebHandler Language="C#" Class="ExtGetRecord" %>

using System;
using System.Web;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using System.Data;
using System.Collections;
using AjaxTools;
using Srvtools;

public class ExtGetRecord : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    
    public void ProcessRequest(HttpContext context)
    {
        try
        {
            if (string.IsNullOrEmpty(CliUtils.fLoginUser))
            {
                throw new Exception("75FF57F7-7AC0-43c8-9454-C92B4A2723BB"); 
            }
            string cachename = context.Request["cacheDataSet"];
            string module = context.Request["module"];
            string cmd = context.Request["command"];
            string fields = context.Request["fields"];
            string oper = context.Request["oper"];

            object cache = context.Session[cachename];
            if (cache != null)
            {
                DataSet cacheData = null;
                DataTable cacheTable = null;
                if (cache is DataTable)
                {
                    cacheTable = cache as DataTable;
                }
                else if (cache is DataSet)
                {
                    cacheData = cache as DataSet;
                    cacheTable = cacheData.Tables[cmd];
                }
                else
                {
                    return;
                }
                string json = "";
                DataRow row = null;
                if (string.IsNullOrEmpty(oper))
                {
                    string foreignCacheName = context.Request["foreignCacheDataSet"];
                    string foreignCmd = context.Request["foreignCommand"];
                    string foreignKey = context.Request["foreignKey"];
                    string foreignCommandKeyValues = context.Request["foreignCommandKeyValues"];
                    string keyvalues = context.Request["keyvalues"];
                    if (!string.IsNullOrEmpty(foreignCacheName)
                        && !string.IsNullOrEmpty(foreignKey)
                        && !string.IsNullOrEmpty(foreignCmd)
                        && !string.IsNullOrEmpty(foreignCommandKeyValues))
                    {
                        object foreignCache = context.Session[foreignCacheName];
                        if (foreignCache != null)
                        {
                            DataTable foreignTable = (foreignCache as DataSet).Tables[foreignCmd];
                            DataRow foreignRow = this.FindRow(foreignTable, foreignCommandKeyValues);
                            if (foreignRow != null)
                            {
                                object keyvalue = foreignRow[foreignKey];
                                string key = context.Request["key"];
                                if (!string.IsNullOrEmpty(key) && keyvalue != null)
                                {
                                    string formatter = GloFix.IsNumeric(cacheTable.Columns[key].DataType) ? "{{{0}:{1}}}" : "{{{0}:'{1}'}}";
                                    keyvalues = string.Format(formatter, key, keyvalue.ToString());
                                }
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(keyvalues) && keyvalues != "{}")
                    {
                        row = this.FindRow(cacheTable, keyvalues);
                        if (row == null)
                        {
                            string alias = context.Request["alias"];
                            string sql = context.Request["sql"];
                            if (!string.IsNullOrEmpty(alias) && !string.IsNullOrEmpty(sql))
                            {
                                module = "GLModule";
                                cmd = "cmdRefValUse";
                            }
                            else
                            {
                                sql = CliUtils.GetSqlCommandText(module, cmd, CliUtils.fCurrentProject);
                            }
                            JavaScriptSerializer serializer = new JavaScriptSerializer();
                            Dictionary<string, object> dickeyValues = serializer.DeserializeObject(keyvalues) as Dictionary<string, object>;
                            foreach (KeyValuePair<string, object> pair in dickeyValues)
                            {
                                string formatter = GloFix.IsNumeric(cacheTable.Columns[pair.Key].DataType) ? "{0}={1}" : "{0}='{1}'";
                                sql = CliUtils.InsertWhere(sql, string.Format(formatter, pair.Key, pair.Value.ToString()));
                            }
                            DataTable excTable = CliUtils.ExecuteSql(module, cmd, sql, true, CliUtils.fCurrentProject).Tables[0];
                            if (excTable.Rows.Count == 1)
                            {
                                cacheTable.Merge(excTable);
                                row = excTable.Rows[0];
                            }
                        }
                    }
                }
                else
                {
                    InfoDataSet ds = new InfoDataSet();
                    ds.AlwaysClose = false;
                    ds.RemoteName = string.Format("{0}.{1}", module, cmd);
                    ds.ServerModify = true;
                    ds.RealDataSet = cacheData;

                    bool autoApply = Convert.ToBoolean(context.Request["autoApply"]);
                    if (oper == "add")
                    {
                        row = cacheTable.NewRow();
                        string[] lstFields = fields.Split(',');
                        foreach (string field in lstFields)
                        {
                            if (cacheTable.Columns[field].DataType == typeof(bool))
                            {
                                string boolvalue = context.Request[field];
                                if (string.IsNullOrEmpty(boolvalue))
                                {
                                    row[field] = false;
                                }
                                else if (boolvalue == "on")
                                {
                                    row[field] = true;
                                }
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(context.Request[field]))
                                {
                                    row[field] = DBNull.Value;
                                }
                                else
                                {
                                    row[field] = context.Request[field];
                                }
                            }
                        }
                        cacheTable.Rows.Add(row);
                        if (autoApply)
                        {
                            ds.ApplyUpdates();
                        }
                    }
                    else if (oper == "save")
                    {
                        ds.ApplyUpdates();
                    }
                    else if (oper == "abort")
                    {
                        cacheData.RejectChanges();
                    }
                    else
                    {
                        string keyfields = context.Request["keyfields"];
                        List<object> values = new List<object>();
                        foreach (string keyfield in keyfields.Split(','))
                        {
                            values.Add(context.Request[keyfield]);
                        }
                        row = cacheTable.Rows.Find(values.ToArray());
                        if (row != null)
                        {
                            if (oper == "update")
                            {
                                foreach (DataColumn column in cacheTable.Columns)
                                {
                                    if (column.DataType == typeof(bool))
                                    {
                                        string boolvalue = context.Request[column.ColumnName];
                                        if (string.IsNullOrEmpty(boolvalue))
                                        {
                                            row[column.ColumnName] = false;
                                        }
                                        else if (boolvalue == "on")
                                        {
                                            row[column.ColumnName] = true;
                                        }
                                    }
                                    else
                                    {
                                        if (string.IsNullOrEmpty(context.Request[column.ColumnName]))
                                        {
                                            if (row.RowState != DataRowState.Added)
                                            {
                                                if (row[column.ColumnName, DataRowVersion.Original] != null && row[column.ColumnName, DataRowVersion.Original].ToString() != "")
                                                {
                                                    row[column.ColumnName] = DBNull.Value;
                                                }
                                            }
                                            else
                                            {
                                                if (row[column.ColumnName] != null && row[column.ColumnName].ToString() != "")
                                                {
                                                    row[column.ColumnName] = DBNull.Value;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            row[column.ColumnName] = context.Request[column.ColumnName];
                                        }
                                    }
                                }
                            }
                            else if (oper == "delete")
                            {
                                json = string.Format("{{success: true, data:{0}}}", JsonSerializer.RowToJson(row, keyfields.Split(',')));
                                row.Delete();
                            }
                            if (autoApply)
                            {
                                ds.ApplyUpdates();
                            }
                        }
                        else
                        {
                            json = string.Format("{{success:false,warning:'{0}'}}", SysMsg.GetSystemMessage(CliUtils.fClientLang, "AjaxTools", "AjaxFormView", "pkChange", true));
                            
                        }
                    }
                }
                if (row != null && row.RowState != DataRowState.Deleted && row.RowState != DataRowState.Detached)
                {
                    json = string.Format("{{success:true,data:{0}}}", JsonSerializer.RowToJson(row, fields.Split(',')));
                }
                context.Response.Write(json);
            }
        }
        catch (Exception exception)
        {
            string errorMsg = exception.Message;
            context.Response.Write(string.Format("{{success:false,message:'{0}',stack:'{1}'}}",
                errorMsg.Replace("\r", "\\r").Replace("\n", "\\n").Replace(@"\", @"\\").Replace(@"'", " ").Replace("\"", " "),
                exception.StackTrace.Replace("\r", "\\r").Replace("\n", "\\n").Replace(@"\", @"\\").Replace(@"'", " ").Replace("\"", " ")));
        }
    }

    DataRow FindRow(DataTable table, string keyvalues)
    {
        DataRow row = null;
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        Dictionary<string, object> dickeyValues = serializer.DeserializeObject(keyvalues) as Dictionary<string, object>;
        List<object> values = new List<object>();
        string keyValueString = "";
        foreach (KeyValuePair<string, object> pair in dickeyValues)
        {
            values.Add(pair.Value);
            string formatter = GloFix.IsNumeric(table.Columns[pair.Key].DataType) ? "{0}={1} AND " : "{0}='{1}' AND ";
            keyValueString += string.Format(formatter, pair.Key, pair.Value.ToString());
        }
        if (keyValueString.EndsWith(" AND "))
        {
            keyValueString = keyValueString.Substring(0, keyValueString.Length - 5);
        }
        if (table.PrimaryKey.Length != 0)
        {
            row = table.Rows.Find(values.ToArray());
        }
        else
        {
            DataRow[] rows = table.Select(keyValueString);
            if (rows.Length > 0)
            {
                row = rows[0];
            }
        }
        return row;
    }
    
    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}