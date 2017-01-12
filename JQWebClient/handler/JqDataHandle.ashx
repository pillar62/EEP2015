<%@ WebHandler Language="C#" Class="JqDataHandle" %>

using System;
using System.Web;
using Newtonsoft;
using Newtonsoft.Json;
using System.Data;
using EFClientTools.EFServerReference;
using System.Collections.Generic;
using System.Web.SessionState;

public class JqDataHandle : IHttpHandler, IRequiresSessionState
{
    public void ProcessRequest(HttpContext context)
    {
        context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
        var mode = context.Request.Form["mode"];
        if (mode == "logout" || context.Request.QueryString["logout"] == "true")
        {
            Logout(context);
        }
        else if (mode == "feedback")
        {
            FeedBack(context);
        }
        else if (mode == "method")
        {
            CallMethod(context);
        }
        else if (mode == "update")
        {
            UpdateData(context);
        }
        else if (mode == "import")
        {
            ImportData(context);
        }
        else if (mode == "export")
        {
            ExportData(context);
        }
        else if (mode == "duplicate")
        {
            DuplicateCheckData(context);
        }
        else if (mode == "seq")
        {
            AutoSeq(context);
        }
        else if (mode == "recordlock")
        {
            RecordLock(context);
        }
        else if (mode == "removerecordlock")
        {
            RemoveRecordLock(context);
        }
        else if (mode == "CacheSmartDatetime")
        {
            CacheSmartDatetime(context);
        }
        else
        {
            SelectData(context);
        }
    }

    private object GetJObjectValue(Newtonsoft.Json.Linq.JObject obj, string propertyName)
    {
        if (obj[propertyName] != null && obj[propertyName] is Newtonsoft.Json.Linq.JValue)
        {
            return (obj[propertyName] as Newtonsoft.Json.Linq.JValue).Value;
        }
        return null;
    }

    private void Logout(HttpContext context)
    {
        try
        {
            EFClientTools.ClientUtility.Client.LogOff(EFClientTools.ClientUtility.ClientInfo);
        }
        finally
        {
            context.Session.Clear();
        }

    }

    private void FeedBack(HttpContext context)
    {
        EFClientTools.ClientUtility.ClientInfo.cErrorCode = 1220;

        var jdo = new JqDataObject("GLModule");
        var clientInfo = EFClientTools.ClientUtility.ClientInfo;
        var error = (Newtonsoft.Json.Linq.JObject)JsonConvert.DeserializeObject(context.Request.Form["error"]);
        List<Object> parameters = new List<object>();
        var source = context.Request.Form["source"];
        if (source == null || source == "") source = "JQWebClient";
        parameters.Add(clientInfo.UserID);
        parameters.Add(source);
        parameters.Add((string)GetJObjectValue(error, "message"));
        parameters.Add(context.Request.Form["stack"].Length > 4000 ? context.Request.Form["stack"].Substring(0, 4000) : context.Request.Form["stack"]);
        parameters.Add((string)GetJObjectValue(error, "description"));
        parameters.Add(DateTime.Now);
        parameters.Add(new byte[] { });
        parameters.Add("E");
        EFClientTools.ClientUtility.ClientInfo.cErrorCode = 0;
        jdo.CallMethod("LogError", parameters);

    }

    private void CallMethod(HttpContext context)
    {
        EFClientTools.ClientUtility.ClientInfo.cErrorCode = 1130;

        var jdo = new JqDataObject(GetParameter(context, Data));
        var methodName = context.Request.Form["method"];

        var parameters = new List<object>();
        if (context.Request.Form["parameters"] != null)
        {
            parameters.Add(context.Request["parameters"]);
        }
        var value = jdo.CallMethod(methodName, parameters);
        context.Response.ContentType = "text/plain";
        context.Response.Write(value.ToString());
        EFClientTools.ClientUtility.ClientInfo.cErrorCode = 0;
    }

    private void UpdateData(HttpContext context)
    {
        EFClientTools.ClientUtility.ClientInfo.cErrorCode = 1120;//get properties
        //get master key
        var data = context.Request.Form["data"];

        var array = (Newtonsoft.Json.Linq.JArray)JsonConvert.DeserializeObject(data);

        var tableNames = new List<string>();
        var tableObjects = new Dictionary<string, Newtonsoft.Json.Linq.JObject>();
        foreach (Newtonsoft.Json.Linq.JObject tableObject in array)
        {
            var tableName = (string)GetJObjectValue(tableObject, "tableName");
            tableNames.Add(tableName);
            tableObjects.Add(tableName, tableObject);
        }

        var jdo = new JqDataObject(GetParameter(context, Data));
        var masterTableName = GetParameter(context, "TableName");
        jdo.TableName = masterTableName;
        //if (jdo.IsMaster)
        //{
        var dataSet = jdo.GetDataSet(null);// 先取出结构
        var masterTable = dataSet.Tables[masterTableName];

        var masterTableObject = tableObjects[masterTableName];
        EFClientTools.ClientUtility.ClientInfo.cErrorCode = 1121;//get updated values 
        //把更改和删除的数据都取出来
        var rowObjs = masterTableObject["updated"] as Newtonsoft.Json.Linq.JArray;
        foreach (Newtonsoft.Json.Linq.JObject rowObj in rowObjs)
        {
            var keyValues = new Dictionary<string, object>();
            foreach (var key in masterTable.PrimaryKey)
            {
                if (EFClientTools.ClientUtility.ClientInfo.DatabaseType == "Oracle" && key.DataType == typeof(DateTime))
                {
                    DateTime t = Convert.ToDateTime(GetJObjectValue(rowObj, key.ColumnName));
                    keyValues.Add(key.ColumnName, t);
                }
                else
                {
                    keyValues.Add(key.ColumnName, GetJObjectValue(rowObj, key.ColumnName));
                }
            }
            dataSet.Merge(jdo.GetDataSet(keyValues));
        }

        rowObjs = masterTableObject["deleted"] as Newtonsoft.Json.Linq.JArray;
        foreach (Newtonsoft.Json.Linq.JObject rowObj in rowObjs)
        {
            var keyValues = new Dictionary<string, object>();
            foreach (var key in masterTable.PrimaryKey)
            {
                if (EFClientTools.ClientUtility.ClientInfo.DatabaseType == "Oracle" && key.DataType == typeof(DateTime))
                {
                    DateTime t = Convert.ToDateTime(GetJObjectValue(rowObj, key.ColumnName));
                    keyValues.Add(key.ColumnName, t);
                }
                else
                {
                    keyValues.Add(key.ColumnName, GetJObjectValue(rowObj, key.ColumnName));
                }
            }
            dataSet.Merge(jdo.GetDataSet(keyValues));
        }
        EFClientTools.ClientUtility.ClientInfo.cErrorCode = 1122;//push updates

        foreach (var tableName in tableNames)
        {
            var table = dataSet.Tables[tableName];
            var tableObject = tableObjects[tableName];
            //删除
            DeleteTable(table, tableObject["deleted"] as Newtonsoft.Json.Linq.JArray);
            //新增
            InsertTable(table, tableObject["inserted"] as Newtonsoft.Json.Linq.JArray);
            //更改       
            UpdateTable(table, tableObject["updated"] as Newtonsoft.Json.Linq.JArray);
        }
        EFClientTools.ClientUtility.ClientInfo.cErrorCode = 1123;//run applyupdates

        var returnDataSet = jdo.ApplyUpdates(dataSet);
        if ((masterTableObject["inserted"] as Newtonsoft.Json.Linq.JArray).Count > 0)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write(DataTableToJson(returnDataSet.Tables[masterTableName]));
        }
        //}
        //else
        //{
        //    throw new InvalidOperationException("Only master can apply datas.");
        //}
        EFClientTools.ClientUtility.ClientInfo.cErrorCode = 0;
    }

    private void InsertTable(DataTable table, Newtonsoft.Json.Linq.JArray rowObjs)
    {
        foreach (Newtonsoft.Json.Linq.JObject rowObj in rowObjs)
        {
            var row = table.NewRow();
            foreach (var column in rowObj)
            {
                if (column.Key.StartsWith(CopyKeyFieldPrefix) || table.Columns[column.Key] == null)
                {
                    continue;
                }

                var value = GetJObjectValue(rowObj, column.Key);
                if (value == null)
                {
                    row[column.Key] = DBNull.Value;
                }
                else if (value.ToString().Length == 0 && table.Columns[column.Key].DataType != typeof(string))
                {
                    row[column.Key] = DBNull.Value;
                }
                else if (table.Columns[column.Key].DataType == typeof(bool))
                {
                    if (value.ToString() == "1" || value.ToString().ToLower() == "true")
                    {
                        row[column.Key] = true;
                    }
                    else
                    {
                        row[column.Key] = false;
                    }
                }
                else
                {
                    try
                    {
                        row[column.Key] = GetJObjectValue(rowObj, column.Key);
                    }
                    catch (Exception e)
                    {
                        throw new Exception(e.Message);
                    }
                }
            }
            table.Rows.Add(row);
        }
    }

    private void UpdateTable(DataTable table, Newtonsoft.Json.Linq.JArray rowObjs)
    {
        foreach (Newtonsoft.Json.Linq.JObject rowObj in rowObjs)
        {
            var keyValues = new List<object>();
            foreach (var key in table.PrimaryKey)
            {
                var value = GetJObjectValue(rowObj, CopyKeyFieldPrefix + key.ColumnName);
                if (value == null)
                {
                    value = GetJObjectValue(rowObj, key.ColumnName);
                }
                keyValues.Add(value);
            }
            var row = table.Rows.Find(keyValues.ToArray());
            foreach (var column in rowObj)
            {
                if (column.Key.StartsWith(CopyKeyFieldPrefix) || table.Columns[column.Key] == null)
                {
                    continue;
                }
                //byte[]类型暂时不进行处理
                if (table.Columns[column.Key].DataType == typeof(byte[]))
                {
                    continue;
                }
                var value = GetJObjectValue(rowObj, column.Key);
                if (value == null)
                {
                    row[column.Key] = DBNull.Value;
                }
                else if (value.ToString().Length == 0 && table.Columns[column.Key].DataType != typeof(string))
                {
                    row[column.Key] = DBNull.Value;
                }
                else if (table.Columns[column.Key].DataType == typeof(bool))
                {
                    if (value.ToString() == "1" || value.ToString().ToLower() == "true")
                    {
                        row[column.Key] = true;
                    }
                    else
                    {
                        row[column.Key] = false;
                    }
                }
                else
                {
                    try
                    {
                        row[column.Key] = GetJObjectValue(rowObj, column.Key);
                    }
                    catch (Exception e)
                    {
                        throw new Exception(e.Message);
                    }
                }
            }
        }
    }

    private void DeleteTable(DataTable table, Newtonsoft.Json.Linq.JArray rowObjs)
    {
        foreach (Newtonsoft.Json.Linq.JObject rowObj in rowObjs)
        {
            var keyValues = new List<object>();
            foreach (var key in table.PrimaryKey)
            {
                var value = GetJObjectValue(rowObj, CopyKeyFieldPrefix + key.ColumnName);
                if (value == null)
                {
                    value = GetJObjectValue(rowObj, key.ColumnName);
                }
                keyValues.Add(value);
            }
            var row = table.Rows.Find(keyValues.ToArray());
            row.Delete();
        }
    }

    public void RecordLock(HttpContext context)
    {
        EFClientTools.ClientUtility.ClientInfo.cErrorCode = 1160;
        var rows = (Newtonsoft.Json.Linq.JArray)JsonConvert.DeserializeObject(context.Request.Form["rows"]);
        var locktype = (EFClientTools.EFServerReference.LockType)Enum.Parse(typeof(EFClientTools.EFServerReference.LockType), context.Request.Form["locktype"], true);

        var keys = (Newtonsoft.Json.Linq.JArray)JsonConvert.DeserializeObject(context.Request.Form["keys"]);
        var jdo = new JqDataObject(GetParameter(context, Data));
        jdo.TableName = GetParameter(context, "TableName");
        jdo.RemoteName = context.Request.Form["remoteName"];

        var returnType = LockType.Idle;
        var user = string.Empty;
        var lockRow = new Dictionary<string, object>();
        foreach (Newtonsoft.Json.Linq.JObject row in rows)
        {
            lockRow = new Dictionary<string, object>();
            foreach (Newtonsoft.Json.Linq.JValue key in keys)
            {
                lockRow.Add((string)key.Value, GetJObjectValue(row, (string)key.Value));
            }
            var status = jdo.DoRecordLock(lockRow, locktype);
            returnType = status.LockType;
            if (returnType != LockType.Idle)
            {
                user = status.UserID;
            }
        }
        var reloadRows = "[]";
        if (returnType == LockType.Idle && context.Request.Form["lockmode"] == "reload")
        {
            var dataSet = jdo.GetDataSet(lockRow);
            reloadRows = JsonConvert.SerializeObject(dataSet.Tables[jdo.TableName], Formatting.Indented);
        }

        context.Response.ContentType = "text/plain";
        context.Response.Write(string.Format("{{\"result\":\"{0}\",\"user\":\"{1}\",\"rows\":{2}}}", returnType.ToString().ToLower(), user, reloadRows));
        EFClientTools.ClientUtility.ClientInfo.cErrorCode = 0;
    }

    public void RemoveRecordLock(HttpContext context)
    {
        var userID = context.Request.Form["userID"];
        if (string.IsNullOrEmpty(userID))
        {
            userID = EFClientTools.ClientUtility.ClientInfo.UserID;
        }
        EFClientTools.ClientUtility.Client.RemoveRecordLock(EFClientTools.ClientUtility.ClientInfo, userID);
    }

    public const string Data = "RemoteName";

    private void SelectData(HttpContext context)
    {
        EFClientTools.ClientUtility.ClientInfo.cErrorCode = 1110;//context properties
        //from file
        //from database
        var jdo = new JqDataObject(GetParameter(context, Data));
        jdo.TableName = GetParameter(context, "TableName");
        jdo.RemoteName = context.Request.Form["remoteName"];
        var pageSize = 10;
        if (context.Request.Form["rows"] != null)
        {
            int.TryParse(context.Request.Form["rows"].ToString(), out pageSize);
        }
        else if (context.Request.QueryString["rows"] != null)
        {
            int.TryParse(context.Request.QueryString["rows"].ToString(), out pageSize);
        }
        else
        {
            pageSize = -1;
        }
        var pageIndex = 1;
        if (context.Request.Form["page"] != null)
        {
            int.TryParse(context.Request.Form["page"].ToString(), out pageIndex);
        }
        var queryWord = context.Request.Form["queryWord"];

        var whereString = GetParameter(context, "whereString");
        //add for autocomplete ,combo's reload can't use param
        //if (context.Request.QueryString["whereString"] != null)
        //{
        //    whereString = context.Request.QueryString["whereString"].ToString();
        //}
        Dictionary<string, object> parentRow = null;
        var parentTableName = "";
        if (!string.IsNullOrEmpty(queryWord))
        {
            var queryObj = (Newtonsoft.Json.Linq.JObject)JsonConvert.DeserializeObject(queryWord);
            whereString = (string)GetJObjectValue(queryObj, "whereString");
            //add for codorva encode space and others
            if (whereString != null)
            {
                whereString = whereString.Replace("markspace", " ");
            }
            parentTableName = (string)GetJObjectValue(queryObj, "parentTableName");
            var remoteName = (string)GetJObjectValue(queryObj, "remoteName");

            if (queryObj["parentRow"] != null)
            {
                var masterJdo = new JqDataObject(GetParameter(context, Data));
                masterJdo.TableName = parentTableName;
                var primaryKeys = masterJdo.GetPrimaryKeys(); //master detail remotename 不一致时？？


                parentRow = new Dictionary<string, object>();
                var parentRowObj = (Newtonsoft.Json.Linq.JObject)queryObj["parentRow"];
                foreach (var item in parentRowObj)
                {
                    if (primaryKeys.Contains(item.Key))
                    {
                        parentRow.Add(item.Key, GetJObjectValue(parentRowObj, item.Key));
                    }
                }
            }
        }
        EFClientTools.ClientUtility.ClientInfo.cErrorCode = 1111;//get Data

        var sortfield = context.Request.Form["sort"];
        var sortorder = context.Request.Form["order"];
        DataTable dataTable = jdo.GetDataTable(whereString, sortfield, sortorder, parentTableName, parentRow, (pageIndex - 1) * pageSize, pageSize);

        int rowCount = 0;
        var includeRows = GetParameter(context, "IncludeRows");
        if (!string.IsNullOrEmpty(includeRows) && includeRows.Equals(bool.TrueString, StringComparison.OrdinalIgnoreCase))
        {
            if (jdo.IsMaster)
            {
                rowCount = jdo.GetDataCount(whereString);
            }
            else
            {
                CopyKeyFields(dataTable);
                rowCount = dataTable.Rows.Count;

                if (pageSize != -1)
                {
                    var startIndex = (pageIndex - 1) * pageSize;
                    var count = pageSize;
                    var packetTable = dataTable.Clone();
                    for (int i = startIndex; i < startIndex + count && i < dataTable.Rows.Count; i++)
                    {
                        packetTable.ImportRow(dataTable.Rows[i]);
                    }
                    dataTable = packetTable;
                }

            }
        }
        EFClientTools.ClientUtility.ClientInfo.cErrorCode = 1112;//total
        //total
        var total = context.Request.Form["totalColumn"];

        var totaljs = "";
        if (!string.IsNullOrEmpty(total))
        {
            var totalObj = (Newtonsoft.Json.Linq.JObject)JsonConvert.DeserializeObject(total);
            var totals = new Dictionary<string, string>();
            foreach (var item in totalObj)
            {
                totals.Add(item.Key, GetJObjectValue(totalObj, item.Key).ToString());
            }
            var totalTable = jdo.GetTotalTable(whereString, parentTableName, parentRow, totals);
            if (totalTable.Rows.Count > 0)
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();

                sb.Append(",\"footer\":");

                Newtonsoft.Json.Linq.JArray jArray = new Newtonsoft.Json.Linq.JArray();
                Newtonsoft.Json.Linq.JObject jObject = new Newtonsoft.Json.Linq.JObject();
                foreach (DataColumn totalColumn in totalTable.Columns)
                {
                    if (totalTable.Rows.Count > 1)
                    {
                        long grouptotal = 0;
                        try
                        {
                            foreach (DataRow dr in totalTable.Rows)
                            {
                                if (totals[totalColumn.ColumnName] == "sum")
                                    grouptotal = grouptotal + long.Parse(totalTable.Rows[0][totalColumn].ToString());
                                else if (totals[totalColumn.ColumnName] == "max")
                                {
                                    if (grouptotal > long.Parse(totalTable.Rows[0][totalColumn].ToString()))
                                    {
                                        grouptotal = long.Parse(totalTable.Rows[0][totalColumn].ToString());
                                    }
                                }
                                else if (totals[totalColumn.ColumnName] == "min")
                                {
                                    if (grouptotal < long.Parse(totalTable.Rows[0][totalColumn].ToString()))
                                    {
                                        grouptotal = long.Parse(totalTable.Rows[0][totalColumn].ToString());
                                    }
                                }
                            }
                            jObject[totalColumn.ColumnName] = new Newtonsoft.Json.Linq.JValue(grouptotal);
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    else
                        jObject[totalColumn.ColumnName] = new Newtonsoft.Json.Linq.JValue(totalTable.Rows[0][totalColumn]);
                }
                var totalCaption = context.Request.Form["totalCaption"];
                if (!string.IsNullOrEmpty(totalCaption))
                {
                    jObject[dataTable.Columns[0].ColumnName] = new Newtonsoft.Json.Linq.JValue(totalCaption);
                }
                jArray.Add(jObject);
                sb.Append(jArray.ToString());

                totaljs = sb.ToString();
            }
        }
        EFClientTools.ClientUtility.ClientInfo.cErrorCode = 1113;//to json and return to client

        context.Response.ContentType = "text/plain";
        var tableName = jdo.GetTableName();

        if (!string.IsNullOrEmpty(includeRows) && includeRows.Equals(bool.TrueString, StringComparison.OrdinalIgnoreCase))
        {
            context.Response.Write(DataTableToJson(dataTable, rowCount, totaljs, tableName));
        }
        else
        {
            context.Response.Write(DataTableToJson(dataTable));
        }
        EFClientTools.ClientUtility.ClientInfo.cErrorCode = 0;
    }

    private void CacheSmartDatetime(HttpContext context)
    {
        EFClientTools.ClientUtility.ClientInfo.cErrorCode = 1110;//context properties
        var jdo = new JqDataObject(GetParameter(context, Data));
        jdo.TableName = GetParameter(context, "TableName");
        jdo.RemoteName = context.Request.Form["remoteName"];
        var whereString = "";
        //if (context.Request.QueryString["whereString"] != null)
        //{
        //    whereString = context.Request.QueryString["whereString"].ToString();
        //}
        Dictionary<string, object> parentRow = null;
        var parentTableName = "";

        EFClientTools.ClientUtility.ClientInfo.cErrorCode = 1112;//total
        //total
        var maxColumn = context.Request.Form["maxColumn"];

        var totaljs = "";
        if (!string.IsNullOrEmpty(maxColumn))
        {
            var totals = new Dictionary<string, string>();
            totals.Add(maxColumn, "max");
            var totalTable = jdo.GetTotalTable(whereString, parentTableName, parentRow, totals);
            if (totalTable.Rows.Count > 0 && totalTable.Rows[0][0].ToString() != "")
            {
                var datetimes = totalTable.Rows[0][maxColumn].ToString();
                try
                {
                    var datetime = DateTime.Parse(datetimes);
                    totaljs = datetime.ToString("yyyy-MM-dd HH:mm:ss");
                }
                catch { }
            }
        }

        context.Response.ContentType = "text/plain";
        context.Response.Write(totaljs);
        EFClientTools.ClientUtility.ClientInfo.cErrorCode = 0;
    }

    const string CopyKeyFieldPrefix = "CopyOf_";
    private void CopyKeyFields(DataTable dataTable)
    {
        foreach (var key in dataTable.PrimaryKey)
        {
            var dataColumn = new DataColumn() { ColumnName = CopyKeyFieldPrefix + key.ColumnName, DataType = key.DataType };
            dataTable.Columns.Add(dataColumn);
        }
        for (int i = 0; i < dataTable.Rows.Count; i++)
        {
            foreach (var key in dataTable.PrimaryKey)
            {
                dataTable.Rows[i][CopyKeyFieldPrefix + key.ColumnName] = dataTable.Rows[i][key.ColumnName];
            }
        }
    }

    private void AutoSeq(HttpContext context)
    {
        EFClientTools.ClientUtility.ClientInfo.cErrorCode = 1140;

        var jdo = new JqDataObject(GetParameter(context, Data));
        jdo.TableName = GetParameter(context, "TableName");
        var queryWord = context.Request.Form["queryWord"];
        var whereString = "";
        Dictionary<string, object> parentRow = null;
        var parentTableName = "";
        if (!string.IsNullOrEmpty(queryWord))
        {
            var queryObj = (Newtonsoft.Json.Linq.JObject)JsonConvert.DeserializeObject(queryWord);
            whereString = (string)GetJObjectValue(queryObj, "whereString");
            parentTableName = (string)GetJObjectValue(queryObj, "parentTableName");

            if (queryObj["parentRow"] != null)
            {
                var masterJdo = new JqDataObject(GetParameter(context, Data));
                masterJdo.TableName = parentTableName;
                var primaryKeys = masterJdo.GetPrimaryKeys();

                parentRow = new Dictionary<string, object>();
                var parentRowObj = (Newtonsoft.Json.Linq.JObject)queryObj["parentRow"];
                foreach (var item in parentRowObj)
                {
                    if (primaryKeys.Contains(item.Key))
                    {
                        parentRow.Add(item.Key, GetJObjectValue(parentRowObj, item.Key));
                    }
                }
            }
        }
        var field = context.Request.Form["field"];
        var numDig = context.Request.Form["numDig"];
        var startValue = context.Request.Form["startValue"];
        var step = context.Request.Form["step"];

        EFClientTools.ClientUtility.ClientInfo.cErrorCode = 1141;

        var totals = new Dictionary<string, string>();
        totals.Add(field, "max");
        var totalTable = jdo.GetTotalTable(whereString, parentTableName, parentRow, totals);
        if (totalTable.Rows.Count > 0)
        {
            var maxcount = totalTable.Rows[0][field].ToString();
            var newvalue = 0;
            if (maxcount == "")
            {
                newvalue = Int32.Parse(startValue);
            }
            else
            {
                newvalue = Int32.Parse(maxcount) + Int32.Parse(step);
            }
            if (totalTable.Columns[field].DataType == typeof(int) || totalTable.Columns[field].DataType == typeof(decimal))
            {
                context.Response.ContentType = "text/plain";
                context.Response.Write((newvalue).ToString());

            }
            else if (totalTable.Columns[field].DataType == typeof(string))
            {
                System.Text.StringBuilder format = new System.Text.StringBuilder();
                format.Append("{0:");
                format.Append('0', Int32.Parse(numDig));
                format.Append("}");
                string returnvalue = string.Format(format.ToString(), newvalue);
                context.Response.ContentType = "text/plain";
                context.Response.Write(returnvalue);
            }
        }
        EFClientTools.ClientUtility.ClientInfo.cErrorCode = 0;
    }

    private void DuplicateCheckData(HttpContext context)
    {
        EFClientTools.ClientUtility.ClientInfo.cErrorCode = 1150;

        var data = context.Request.Form["data"];

        var array = (Newtonsoft.Json.Linq.JArray)JsonConvert.DeserializeObject(data);

        var tableNames = new List<string>();
        var tableObjects = new Dictionary<string, Newtonsoft.Json.Linq.JObject>();
        foreach (Newtonsoft.Json.Linq.JObject tableObject in array)
        {
            var tableName = (string)GetJObjectValue(tableObject, "tableName");
            tableNames.Add(tableName);
            tableObjects.Add(tableName, tableObject);
        }

        var jdo = new JqDataObject(GetParameter(context, Data));
        var TableName = GetParameter(context, "TableName");
        jdo.TableName = TableName;

        var dataSet = jdo.GetDataSet(null);// 先取出结构
        var Table = dataSet.Tables[TableName];

        var TableObject = tableObjects[TableName];

        var rowObjs = TableObject["inserted"] as Newtonsoft.Json.Linq.JArray;
        bool returnvalue = true;
        EFClientTools.ClientUtility.ClientInfo.cErrorCode = 1151;

        foreach (Newtonsoft.Json.Linq.JObject rowObj in rowObjs)
        {
            var keyValues = new Dictionary<string, object>();
            foreach (var key in Table.PrimaryKey)
            {
                if (EFClientTools.ClientUtility.ClientInfo.DatabaseType == "Oracle" && key.DataType == typeof(DateTime))
                {
                    DateTime t = Convert.ToDateTime(GetJObjectValue(rowObj, key.ColumnName));
                    keyValues.Add(key.ColumnName, t);
                }
                else
                {
                    keyValues.Add(key.ColumnName, GetJObjectValue(rowObj, key.ColumnName));
                }
            }
            //add 空资料设定duplicatecheck 又没有设定validate时候这边getDataset会进来取道所有资料会返回false不通过，提示信息错误，现在暂时处理成通过duplicate
            bool emptyvalue = true;
            foreach (var pair in keyValues)
            {
                if (pair.Value != "")
                {
                    emptyvalue = false;
                    break;
                }
            }
            if (emptyvalue) { break; }
            //end add by lu 2014.10.07

            if (jdo.GetDataSet(keyValues).Tables[0].Rows.Count > 0)
            {
                returnvalue = false;
                break;
            }
        }

        //jdo.ApplyUpdates(dataSet);
        context.Response.ContentType = "text/plain";
        context.Response.Write(returnvalue.ToString().ToLower());
        EFClientTools.ClientUtility.ClientInfo.cErrorCode = 0;
    }

    public void ImportData(HttpContext context)
    {
        EFClientTools.ClientUtility.ClientInfo.cErrorCode = 1170;//context properties

        var jdo = new JqDataObject(GetParameter(context, Data));
        jdo.TableName = GetParameter(context, "TableName");

        var dataSet = jdo.GetDataSet(null);

        var dataTable = dataSet.Tables[jdo.TableName];

        var beginRow = 0;
        if (context.Request.Form["beginrow"] != null)
        {
            int.TryParse(context.Request.Form["beginrow"].ToString(), out beginRow);
        }
        var beginCell = 0;
        if (context.Request.Form["begincell"] != null)
        {
            int.TryParse(context.Request.Form["begincell"].ToString(), out beginCell);
        }

        if (context.Request.Files.Count > 0)
        {
            EFClientTools.ClientUtility.ClientInfo.cErrorCode = 1171;

            var stream = context.Request.Files[0].InputStream;
            try
            {
                JQUtility.ImportFromExcel(dataTable, stream, beginRow, beginCell);
            }
            catch (System.Xml.XmlException e)
            {
                context.Response.ContentType = "text/plain";
                context.Response.Write("'File is not stored as xml file'");
                return;
            }
            catch (Exception e)
            {
                context.Response.ContentType = "text/plain";
                context.Response.Write("'" + e.Message + "'");
                return;
            }
        }
        var tableName = jdo.GetTableName();
        var rowCount = dataTable.Rows.Count;
        context.Response.ContentType = "text/plain";
        context.Response.Write(DataTableToJson(dataTable, rowCount, string.Empty, tableName));
        EFClientTools.ClientUtility.ClientInfo.cErrorCode = 0;
    }


    public void ExportData(HttpContext context)
    {
        EFClientTools.ClientUtility.ClientInfo.cErrorCode = 1180;//context properties

        var jdo = new JqDataObject(GetParameter(context, Data));
        jdo.TableName = GetParameter(context, "TableName");
        var queryWord = context.Request.Form["queryWord"];
        var whereString = "";
        Dictionary<string, object> parentRow = null;
        var parentTableName = "";
        if (!string.IsNullOrEmpty(queryWord))
        {
            var queryObj = (Newtonsoft.Json.Linq.JObject)JsonConvert.DeserializeObject(queryWord);
            whereString = (string)GetJObjectValue(queryObj, "whereString");
            parentTableName = (string)GetJObjectValue(queryObj, "parentTableName");

            if (queryObj["parentRow"] != null)
            {
                var masterJdo = new JqDataObject(GetParameter(context, Data));
                masterJdo.TableName = parentTableName;
                var primaryKeys = masterJdo.GetPrimaryKeys();

                parentRow = new Dictionary<string, object>();
                var parentRowObj = (Newtonsoft.Json.Linq.JObject)queryObj["parentRow"];
                foreach (var item in parentRowObj)
                {
                    if (primaryKeys.Contains(item.Key))
                    {
                        parentRow.Add(item.Key, GetJObjectValue(parentRowObj, item.Key));
                    }
                }
            }
        }
        var sortfield = context.Request.Form["sort"];
        var sortorder = context.Request.Form["order"];
        DataTable dataTable = jdo.GetDataTable(whereString, sortfield, sortorder, parentTableName, parentRow, 0, -1);
        EFClientTools.ClientUtility.ClientInfo.cErrorCode = 1181;

        DataTable exportTable = dataTable.Copy();
        exportTable.PrimaryKey = new DataColumn[] { };
        var columns = context.Request.Form["columns"];
        var array = (Newtonsoft.Json.Linq.JArray)JsonConvert.DeserializeObject(columns);
        List<string> exportColumns = new List<string>();
        List<string> totals = new List<string>();
        foreach (Newtonsoft.Json.Linq.JObject tableObject in array)
        {
            var field = (string)GetJObjectValue(tableObject, "field");
            if (!string.IsNullOrEmpty(field))
            {
                var title = (string)GetJObjectValue(tableObject, "title");
                var total = (string)GetJObjectValue(tableObject, "total");
                var optionObject = (Newtonsoft.Json.Linq.JObject)tableObject["options"];
                if (optionObject != null)
                {
                    var remoteName = (string)GetJObjectValue(optionObject, "remoteName");
                    var tableName = (string)GetJObjectValue(optionObject, "tableName");
                    var valueField = (string)GetJObjectValue(optionObject, "valueField");
                    var textField = (string)GetJObjectValue(optionObject, "textField");
                    if (!string.IsNullOrEmpty(remoteName))
                    {
                        string textFieldName = string.Format("TempText_{0}_{1}", valueField, Guid.NewGuid().ToString("N"));

                        exportTable.Columns.Add(new DataColumn(textFieldName, typeof(string)) { Caption = title });

                        for (int i = 0; i < exportTable.Rows.Count; i++)
                        {
                            var textJdo = new JqDataObject(remoteName) { TableName = tableName };
                            var textTable = textJdo.GetDataTable(string.Format("{0} = '{1}'", valueField, exportTable.Rows[i][field].ToString().Replace("'", "''")), string.Empty, string.Empty, 0, 1);
                            if (textTable.Rows.Count > 0)
                            {
                                exportTable.Rows[i][textFieldName] = textTable.Rows[0][textField].ToString();
                            }
                            else
                            {
                                exportTable.Rows[i][textFieldName] = DBNull.Value;
                            }
                        }
                        exportColumns.Add(textFieldName);

                    }
                    else if (optionObject["items"] != null)
                    {
                        var items = (Newtonsoft.Json.Linq.JArray)optionObject["items"];

                        string textFieldName = string.Format("TempText_{0}_{1}", valueField, Guid.NewGuid().ToString("N"));
                        exportTable.Columns.Add(new DataColumn(textFieldName, typeof(string)) { Caption = title });
                        for (int i = 0; i < exportTable.Rows.Count; i++)
                        {
                            exportTable.Rows[i][textFieldName] = DBNull.Value;

                            foreach (Newtonsoft.Json.Linq.JObject item in items)
                            {
                                if (item["value"].ToString() == exportTable.Rows[i][field].ToString())
                                {
                                    exportTable.Rows[i][textFieldName] = item["text"].ToString();
                                }
                            }
                        }
                        exportColumns.Add(textFieldName);
                    }
                }
                else
                {
                    if (exportTable.Columns.Contains(field))
                    {
                        exportTable.Columns[field].Caption = title;
                        exportColumns.Add(field);
                    }
                }
                totals.Add(total);
            }
        }
        EFClientTools.ClientUtility.ClientInfo.cErrorCode = 1182;

        var fileName = string.Format("{0}.xls", DateTime.Now.ToString("yyyyMMddHHmmss"));
        var path = string.Format("../Files/{0}", fileName);
        var gridTitle = string.Empty;
        if (context.Request.Form["title"] != null)
        {
            gridTitle = context.Request.Form["title"];
        }

        JQUtility.ExportToExcel(exportTable, HttpContext.Current.Server.MapPath(path), gridTitle, exportColumns, totals);

        context.Response.ContentType = "text/plain";
        context.Response.Write(fileName);
        EFClientTools.ClientUtility.ClientInfo.cErrorCode = 0;
    }

    public string DataTableToJson(DataTable dt, int rowscount, string totaljs, string tableName)
    {
        EFClientTools.ClientUtility.ClientInfo.cErrorCode = 1101;//Internal method

        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        string js = JsonConvert.SerializeObject(dt, Formatting.Indented);//Indented縮排
        sb.Append("{\"total\":");
        sb.Append(rowscount.ToString());
        sb.Append(",\"tableName\":");
        sb.Append("\"" + tableName + "\"");
        sb.Append(",\"keys\":");
        string sPrimaryKey = "";
        foreach (var PrimaryKey in dt.PrimaryKey)
        {
            if (sPrimaryKey != "")
            {
                sPrimaryKey += ",";
            }
            sPrimaryKey += PrimaryKey.ColumnName;
        }
        sb.Append("\"" + sPrimaryKey + "\"");
        sb.Append(",\"rows\":");
        sb.Append(js);
        if (totaljs != "")
        {
            sb.Append(totaljs);
        }
        sb.Append("}");
        return sb.ToString();
    }

    public string DataTableToJson(DataTable dt)
    {
        EFClientTools.ClientUtility.ClientInfo.cErrorCode = 1102;//Internal method
        return JsonConvert.SerializeObject(dt, Formatting.Indented);//Indented縮排
    }


    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

    private string GetParameter(HttpContext context, string name) {
        var value = context.Request.Form[name];
        if (!string.IsNullOrEmpty(value))
        {
            return value;
        }
        else
        {
            return context.Request.QueryString[name];
        }
    }
}
