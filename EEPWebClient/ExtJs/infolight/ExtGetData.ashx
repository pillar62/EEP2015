`<%@ WebHandler Language="C#" Class="ExtGetData" %>

using System;
using System.Web;
using Srvtools;
using AjaxTools;
using System.Data;
using System.Text;
using System.Collections.Generic;
using System.Web.Script.Serialization;

public class ExtGetData : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    const string SQL_INSERT = "INSERT INTO [{0}] ({1}) VALUES ({2})";
    const string SQL_UPDATE = "UPDATE [{0}] SET {1} WHERE {2}";
    const string SQL_DELETE = "DELETE [{0}] WHERE {1}";

    string GetWhere(HttpContext context, DataTable table, string dbTabName)
    {
        string where = context.Request["where"];
        if (table != null)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string masterKeys = context.Request["masterkeys"];
            if (!string.IsNullOrEmpty(masterKeys))
            {
                Dictionary<string, object> masterRecordKeys = serializer.DeserializeObject(masterKeys) as Dictionary<string, object>;
                if (masterRecordKeys != null)
                {
                    foreach (DataRelation relation in table.DataSet.Relations)
                    {
                        if (relation.ChildTable.TableName == table.TableName)
                        {
                            foreach (KeyValuePair<string, object> pair in masterRecordKeys)
                            {
                                string val = "";
                                Type dataType = relation.ParentTable.Columns[pair.Key].DataType;
                                if (GloFix.IsNumeric(dataType))
                                {
                                    val = pair.Value.ToString();
                                }
                                else if (dataType == typeof(Boolean))
                                {
                                    val = pair.Value.ToString().ToLower();
                                }
                                else
                                {
                                    val = string.Format("'{0}'", pair.Value.ToString());
                                }
                                if (string.IsNullOrEmpty(where))
                                {
                                    if (CliUtils.GetDataBaseType() == ClientType.ctMsSql)
                                    {
                                        where = string.Format("[{0}]={1}",
                                            pair.Key,
                                            val);
                                    }
                                    else
                                    {
                                        where = string.Format("{0}={1}",
                                            pair.Key,
                                            val);
                                    }
                                }
                                else
                                {
                                    if (CliUtils.GetDataBaseType() == ClientType.ctMsSql)
                                    {
                                        where += string.Format(" AND [{0}]={1}",
                                            pair.Key,
                                            val);
                                    }
                                    else
                                    {
                                        where += string.Format(" AND {0}={1}",
                                                pair.Key,
                                                val);
                                    }
                                }
                            }
                            break;
                        }
                    }
                }
            }
            string filterConditions = context.Request["filterConditions"];
            if (!string.IsNullOrEmpty(filterConditions))
            {
                object[] filters = serializer.DeserializeObject(filterConditions) as object[];
                foreach (Dictionary<string, object> filter in filters)
                {
                    string defval = "";
                    Type dataType = table.Columns[filter["field"].ToString()].DataType;
                    string op = filter["operator"].ToString(), quote = "";
                    if (op == "!=")
                    {
                        op = "<>";
                    }
                    if (GloFix.IsNumeric(dataType) || dataType == typeof(Boolean))
                    {
                        if (op == "%" || op == "%%")
                        {
                            op = "=";
                        }
                        defval = filter["defVal"].ToString();
                        if (defval == "true")
                        {
                            defval = "1";
                        }
                        if (defval == "false")
                        {
                            defval = "0";
                        }
                    }
                    else
                    {
                        quote = "'";
                        if (op == "%")
                        {
                            op = " like ";
                            defval = string.Format("{0}%", filter["defVal"].ToString());
                        }
                        else if (op == "%%")
                        {
                            op = " like ";
                            defval = string.Format("%{0}%", filter["defVal"].ToString());
                        }
                        else
                        {
                            defval = string.Format("{0}", filter["defVal"].ToString());
                        }
                    }

                    if (string.IsNullOrEmpty(where))
                    {
                        if (CliUtils.GetDataBaseType() == ClientType.ctMsSql)
                        {
                            where = string.Format("[{0}].[{1}]{2}{3}{4}{3}", dbTabName, filter["field"].ToString(), op, quote, defval);
                        }
                        else
                        {
                            where = string.Format("{0}.{1}{2}{3}{4}{3}", dbTabName, filter["field"].ToString(), op, quote, defval);
                        }
                    }
                    else
                    {
                        if (CliUtils.GetDataBaseType() == ClientType.ctMsSql)
                        {
                            where += string.Format(" {0} [{1}].[{2}]{3}{4}{5}{4}", filter["condition"].ToString(), dbTabName, filter["field"].ToString(), op, quote, defval);
                        }
                        else
                        {
                            where += string.Format(" {0} {1}.[{2}]{3}{4}{5}{4}", filter["condition"].ToString(), dbTabName, filter["field"].ToString(), op, quote, defval);
                        }
                    }
                }
            }
        }
        return where;
    }
    
    public void ProcessRequest(HttpContext context)
    {
        try
        {
            if (string.IsNullOrEmpty(CliUtils.fLoginUser))
            {
                throw new Exception("75FF57F7-7AC0-43c8-9454-C92B4A2723BB");
            }
            string module = context.Request["module"];
            string cmd = context.Request["command"];
            string oper = context.Request["oper"];
            string cachename = context.Request["cacheDataSet"];
            bool sevmod = Convert.ToBoolean(context.Request["sevmod"]);
            bool alwaysClose = Convert.ToBoolean(context.Request["alwaysClose"]);
            //
            //bool allowPage = (!string.IsNullOrEmpty(context.Request["limit"]) && !string.IsNullOrEmpty(context.Request["start"]));
            bool allowPage = Convert.ToBoolean(context.Request["allowPage"]);
            //if (alwaysClose)
            //{
            //    return; 
            //}
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
                string dbTabName = CliUtils.GetTableName(module, cmd, CliUtils.fCurrentProject);
                string where = this.GetWhere(context, cacheTable, dbTabName);
                if (oper == "select")
                {
                    string fields = context.Request["fields"];
                    string json = "";
                    DataRow[] rows = null;
                    if (allowPage)
                    {
                        int pagesize = int.Parse(context.Request["limit"]);
                        int start = int.Parse(context.Request["start"]);
                        int packetCount = start / pagesize + 1;

                        int totalRecordCount = CliUtils.GetRecordsCount(module, cmd, where, CliUtils.fCurrentProject);
                        // 创建InfoDataSet
                        InfoDataSet ds = new InfoDataSet();
                        ds.RemoteName = string.Format("{0}.{1}", module, cmd);
                        ds.PacketRecords = pagesize * packetCount;
                        ds.WhereStr = where;
                        ds.AlwaysClose = false;
                        ds.Active = true;
                        context.Session[cachename + "Eof"] = ds.Eof;
                        context.Session[cachename] = cacheData = ds.RealDataSet;
                        cacheTable = cacheData.Tables[cmd];

                        json = string.Format("{{total:{0},data:{1}}}",
                            totalRecordCount,
                            JsonSerializer.TableToJsonArray(GloFix.GetPageTable(cacheTable, start, pagesize), fields.Split(',')));
                    }
                    else
                    {
                        if (Convert.ToBoolean(context.Request["isDetails"]))
                        {
                            if (string.IsNullOrEmpty(where))
                            {
                                return;
                            }
                            string masterCmd = context.Request["masterCommand"];
                            foreach (DataRelation relation in cacheData.Relations)
                            {
                                if (relation.ParentTable.TableName == masterCmd && relation.ChildTable.TableName == cmd)
                                {
                                    DataRow[] masterRows = cacheData.Tables[masterCmd].Select(where);
                                    if (masterRows.Length == 1)
                                    {
                                        rows = masterRows[0].GetChildRows(relation);
                                    }
                                    break;
                                }
                            }
                        }
                        else
                        {
                            rows = cacheTable.Select(where);
                        }
                        if (rows == null || rows.Length == 0)
                        {
                            json = "{total:0,data:[]}";
                        }
                        else
                        {
                            json = string.Format("{{total:{0},data:{1}}}",
                                rows.Length,
                                JsonSerializer.RowsToJsonArray(rows, fields.Split(',')));
                        }
                    }
                    context.Response.Write(json);
                }
                else if (oper == "save")
                {
                    bool delaySave = false;
                    if (!string.IsNullOrEmpty(context.Request["delaySave"]))
                    {
                        delaySave = Convert.ToBoolean(context.Request["delaySave"]);
                    }
                    string sEditTypes = context.Request["editTypes"];
                    string sChanges = context.Request["changes"];
                    string masterCmd = context.Request["masterCommand"];
                    if (!string.IsNullOrEmpty(sEditTypes))
                    {
                        // 创建InfoDataSet
                        InfoDataSet ds = new InfoDataSet();
                        ds.AlwaysClose = false;
                        ds.RemoteName = string.Format("{0}.{1}", module, string.IsNullOrEmpty(masterCmd) ? cmd : masterCmd);
                        ds.ServerModify = sevmod;
                        ds.RealDataSet = cacheData;
                        DataTable tab = ds.RealDataSet.Tables[cmd];
                        JavaScriptSerializer serializer = new JavaScriptSerializer();
                        object[] editTypes = serializer.DeserializeObject(sEditTypes) as object[];
                        object[] records = serializer.DeserializeObject(sChanges) as object[];
                        if (editTypes.Length > 0)
                        {
                            int j = 0;
                            for (int i = 0; i < editTypes.Length; i++)
                            {
                                Dictionary<string, object> dicKey = editTypes[i] as Dictionary<string, object>;
                                if (dicKey["editType"].ToString() == "delete")
                                {
                                    string single = this.GenWhereString(tab, dicKey);
                                    DataRow[] rows = tab.Select(single);
                                    if (rows.Length == 1)
                                    {
                                        rows[0].Delete();
                                    }
                                    j++;
                                }
                                else
                                {
                                    Dictionary<string, object> dicRecord = records[i - j] as Dictionary<string, object>;
                                    if (dicKey["editType"].ToString() == "insert")
                                    {
                                        DataRow row = tab.NewRow();
                                        foreach (KeyValuePair<string, object> record in dicRecord)
                                        {
                                            row[record.Key] = record.Value;
                                        }
                                        tab.Rows.Add(row);
                                    }
                                    else if (dicKey["editType"].ToString() == "edit")
                                    {
                                        string single = this.GenWhereString(tab, dicKey);
                                        DataRow[] rows = tab.Select(single);
                                        if (rows.Length == 1)
                                        {
                                            foreach (KeyValuePair<string, object> record in dicRecord)
                                            {
                                                rows[0][record.Key] = record.Value;
                                            }
                                        }
                                    }
                                }
                            }
                            if (ds.RealDataSet.HasChanges() && !delaySave)
                            {
                                ds.ApplyUpdates();
                            }
                        }
                    }
                }
            }
        }
        catch (Exception exception)
        {
            string errorMsg = exception.Message;
            if (errorMsg == "75FF57F7-7AC0-43c8-9454-C92B4A2723BB")
            {
                errorMsg = "session time out...\\r\\nplease relogion";
            }
            context.Response.Write(string.Format("{{success:false,message:'{0}',stack:'{1}'}}",
                errorMsg.Replace("\r", "\\r").Replace("\n", "\\n").Replace(@"\", @"\\").Replace(@"'", " ").Replace("\"", " "),
                exception.StackTrace.Replace("\r", "\\r").Replace("\n", "\\n").Replace(@"\", @"\\").Replace(@"'", " ").Replace("\"", " ")));
        }
    }

    string GenWhereString(DataTable table, Dictionary<string, object> dicKey)
    {
        StringBuilder whereBuilder = new StringBuilder();
        foreach (KeyValuePair<string, object> wherePair in dicKey)
        {
            if (wherePair.Key != "editType")
            {
                Type colType = table.Columns[wherePair.Key].DataType;
                if (GloFix.IsNumeric(colType))
                {
                    whereBuilder.AppendFormat("{0}={1} AND ", wherePair.Key, wherePair.Value);
                }
                else if (colType == typeof(Boolean))
                {
                    whereBuilder.AppendFormat("{0}={1} AND ", wherePair.Key, wherePair.Value.ToString().ToLower());
                }
                else
                {
                    whereBuilder.AppendFormat("{0}='{1}' AND ", wherePair.Key, wherePair.Value);
                }
            }
        }
        if (whereBuilder.ToString().EndsWith(" AND "))
        {
            whereBuilder.Remove(whereBuilder.Length - 5, 5);
        }
        return whereBuilder.ToString();
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}