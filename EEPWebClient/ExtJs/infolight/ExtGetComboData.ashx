<%@ WebHandler Language="C#" Class="ExtGetComboData" %>

using System;
using System.Web;
using Srvtools;
using AjaxTools;
using System.Data;
using System.Text;
using System.Collections.Generic;
using System.Web.Script.Serialization;

public class ExtGetComboData : IHttpHandler, System.Web.SessionState.IRequiresSessionState
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
            object cache = context.Session[cachename];
            if (cache != null)
            {
                string cmd = context.Request["command"];
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
                string module = context.Request["module"];
                string alias = context.Request["alias"];
                string cmdsql = context.Request["sql"];

                string fields = context.Request["fields"];
                string where = context.Request["where"];
                string whereMethods = context.Request["whereMethods"];
                string query = context.Request["query"]; //combobox内有输入值时会自动传入
                string keyFilterField = context.Request["keyFilterField"]; //combobox

                bool allowPage = (!string.IsNullOrEmpty(context.Request["limit"]) && !string.IsNullOrEmpty(context.Request["start"]));
                bool usecmd = !string.IsNullOrEmpty(alias) && !string.IsNullOrEmpty(cmdsql);

                string json = "";
                if (allowPage)
                {
                    int pagesize = int.Parse(context.Request["limit"]);
                    int start = int.Parse(context.Request["start"]);
                    int packetCount = start / pagesize + 1;
                    int totalRecordCount = -1;
                    if (usecmd)
                    {
                        string countsql = string.Format("select count(*) from {0}", CliUtils.GetTableName(cmdsql));

                        if (!string.IsNullOrEmpty(where))
                        {
                            countsql = CliUtils.InsertWhere(countsql, where);
                            cmdsql = CliUtils.InsertWhere(cmdsql, where);
                        }
                        if (!string.IsNullOrEmpty(whereMethods))
                        {
                            string methodsWhereString = GetSrvWhereMethods(whereMethods, cacheTable);
                            countsql = CliUtils.InsertWhere(countsql, methodsWhereString);
                            cmdsql = CliUtils.InsertWhere(cmdsql, methodsWhereString);
                        }
                        if (!string.IsNullOrEmpty(query) && !string.IsNullOrEmpty(keyFilterField))
                        {
                            if (GloFix.IsNumeric(cacheTable.Columns[keyFilterField].DataType))
                            {
                                countsql = CliUtils.InsertWhere(countsql, string.Format("{0}={1}", keyFilterField, query));
                                cmdsql = CliUtils.InsertWhere(cmdsql, string.Format("{0}={1}", keyFilterField, query));
                            }
                            else
                            {
                                countsql = CliUtils.InsertWhere(countsql, string.Format("{0} like '{1}%'", keyFilterField, query));
                                cmdsql = CliUtils.InsertWhere(cmdsql, string.Format("{0} like '{1}%'", keyFilterField, query));
                            }
                        }
                        DataSet countDs = CliUtils.ExecuteSql("GLModule", "cmdRefValUse", countsql, true, CliUtils.fCurrentProject);
                        if (countDs.Tables[0].Rows.Count > 0)
                        {
                            totalRecordCount = Convert.ToInt32(countDs.Tables[0].Rows[0][0]);
                        }

                        WebDataSource wds = new WebDataSource();
                        wds.SelectAlias = alias;
                        wds.SelectCommand = cmdsql;
                        wds.CommandPacketRecords = pagesize * packetCount;
                        // select command 出来的table没有主键,因此这里无法进行merge
                        cacheTable = wds.CommandTable;

                        json = string.Format("{{total:{0},data:{1}}}",
                            totalRecordCount,
                            JsonSerializer.TableToJsonArray(GloFix.GetPageTable(cacheTable, start, pagesize), fields.Split(',')));
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(whereMethods))
                        {
                            if (string.IsNullOrEmpty(where))
                            {
                                where = GetSrvWhereMethods(whereMethods, cacheTable);
                            }
                            else
                            {
                                where += string.Format(" AND {0}", GetSrvWhereMethods(whereMethods, cacheTable));
                            }
                        }
                        if (!string.IsNullOrEmpty(query) && !string.IsNullOrEmpty(keyFilterField))
                        {
                            if (!string.IsNullOrEmpty(where))
                            {
                                where += " AND ";
                            }
                            if (cacheTable.Columns[keyFilterField].DataType == typeof(string))
                            {
                                where += string.Format("[{0}] like '{1}%'", keyFilterField, query);
                            }
                            else
                            {
                                where += string.Format("[{0}]={1}", keyFilterField, query);
                            }
                        }
                        InfoDataSet ds = new InfoDataSet();
                        ds.AlwaysClose = false;
                        ds.RemoteName = string.Format("{0}.{1}", module, cmd);
                        ds.PacketRecords = pagesize * packetCount;
                        ds.WhereStr = where;
                        ds.Active = true;
                        context.Session[cachename] = cacheData = ds.RealDataSet;
                        cacheTable = cacheData.Tables[cmd];

                        totalRecordCount = CliUtils.GetRecordsCount(module, cmd, where, CliUtils.fCurrentProject);
                        json = string.Format("{{total:{0},data:{1}}}",
                            totalRecordCount,
                            JsonSerializer.TableToJsonArray(GloFix.GetPageTable(cacheTable, start, pagesize), fields.Split(',')));
                    }
                }
                else
                {
                    DataRow[] rows = null;
                    if (!string.IsNullOrEmpty(whereMethods))
                    {
                        if (string.IsNullOrEmpty(where))
                        {
                            where = GetSrvWhereMethods(whereMethods, cacheTable);
                        }
                        else
                        {
                            where += string.Format(" AND {0}", GetSrvWhereMethods(whereMethods, cacheTable));
                        }
                    }
                    if (!string.IsNullOrEmpty(query) && !string.IsNullOrEmpty(keyFilterField))
                    {
                        if (!string.IsNullOrEmpty(where))
                        {
                            where += " AND ";
                        }
                        if (GloFix.IsNumeric(cacheTable.Columns[keyFilterField].DataType))
                        {
                            where += string.Format("[{0}]={1}", keyFilterField, query);
                        }
                        else
                        {
                            where += string.Format("[{0}] like '{1}%'", keyFilterField, query);
                        }
                    }
                    if (where != null)
                        rows = cacheTable.Select(where);
                    else
                    {
                        rows = new DataRow[cacheTable.Rows.Count];
                        cacheTable.Rows.CopyTo(rows, 0);
                    }
                    json = string.Format("{{total:{0},data:{1}}}",
                        rows.Length,
                        JsonSerializer.RowsToJsonArray(rows, fields.Split(',')));
                }
                context.Response.Write(json);
            }
        }
        catch (Exception exception)
        {
            string errorMsg = exception.Message;
            context.Response.Write(string.Format("{{success:false,message:\"{0}\",stack:\"{1}\"}}",
                errorMsg.Replace("\r", "\\r").Replace("\n", "\\n").Replace(@"\", @"\\"),
                exception.StackTrace.Replace("\r", "\\r").Replace("\n", "\\n").Replace(@"\", @"\\")));
        }
    }

    string GetSrvWhereMethods(string whereMethods, DataTable tab)
    {
        StringBuilder builder = new StringBuilder();
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        object[] methods = serializer.DeserializeObject(whereMethods) as object[];
        foreach (Dictionary<string, object> dicMethod in methods)
        {
            string method = dicMethod["method"] as string;
            if (!string.IsNullOrEmpty(method) && method.IndexOf('.') != -1)
            {
                object[] whereResult = CliUtils.CallMethod(method.Split('.')[0], method.Split('.')[1], new object[] { });
                if (whereResult.Length == 2 && (int)whereResult[0] == 0)
                {
                    string wherePart = this.GetWhereString(tab, dicMethod["field"].ToString(), dicMethod["condition"].ToString(), whereResult[1].ToString());
                    if (builder.Length > 0)
                    {
                        builder.AppendFormat(" AND {0}", wherePart);
                    }
                    else
                    {
                        builder.Append(wherePart);
                    }
                }
            }
        }
        return builder.ToString();
    }

    string GetWhereString(DataTable tab, string field, string condition, string value)
    {
        Type type = tab.Columns[field].DataType;
        string quote = "";
        if (GloFix.IsNumeric(type) || type == typeof(Boolean))
        {
            if (condition == "%" || condition == "%%")
            {
                condition = "=";
            }
        }
        else
        {
            quote = "'";
            if (condition == "%")
            {
                condition = "like";
                value = string.Format("{0}%", value);
            }
            else if (condition == "%%")
            {
                condition = "like";
                value = string.Format("%{0}%", value);
            }
        }
        if (condition == "!=")
        {
            condition = "<>";
        }
        return string.Format("[{0}] {1} {2}{3}{2}", field, condition, quote, value);
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}