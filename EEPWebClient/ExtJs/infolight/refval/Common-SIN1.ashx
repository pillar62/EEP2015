<%@ WebHandler Language="C#" Class="Common_SIN1" %>

using System;
using System.Web;
using Srvtools;
using AjaxTools;
using System.Data;
using System.Text;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Collections;

public class Common_SIN1 : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    public void ProcessRequest(HttpContext context)
    {
        string returnMsg = "";
        string doType = context.Request["doType"];

        string dsSessionName = context.Request["dsSessionName"];

        string tableIndex = context.Request["tableindex"];
        int thetabIndex = 0;
        try { thetabIndex = Convert.ToInt32(tableIndex); }
        catch { }

        switch (doType)
        {
            case "getdetailColumns":
                returnMsg = GetDataColumn(context, dsSessionName, thetabIndex);
                break;
            case "getdetailData":
                returnMsg = GetData(context, dsSessionName, thetabIndex);
                break;
            case "del_save":
                returnMsg = doneDetialSave(context, dsSessionName, thetabIndex);
                break;
            case "del_delete":
                returnMsg = doneDetailDelete(context, dsSessionName, thetabIndex);
                break;
            case "combo_data":
                returnMsg = getComboData(context);
                break;
            case "callmethod":
                returnMsg = getCallMethod(context);
                break;
            case "getArrayData":
                returnMsg = getArrayData(context);
                break;
            case "getRefvalData":
                returnMsg = getRefvalData(context);
                break;
            case "getRefvalColumns":
                returnMsg = getRefDataColumn(context);
                break;
            case "getRefvalRow":
                returnMsg = getRefRow(context);
                break;
            default:
                break;
        }
        context.Response.Write(returnMsg);
    }

    protected string GetDataColumn(HttpContext context, string dsSessionName, int thetabIndex)
    {
        //只要ds不为null，则不管该表是否有数据，都有数据列生成
        string columnJson = "";
        string fields = context.Request["fields"];

        DataTable dt = ((DataSet)context.Session[dsSessionName]).Tables[thetabIndex];
        DataColumn[] columnAry = dt.PrimaryKey;

        if (dt != null)
        {
            //string mKeyvalue = "";
            List<Hashtable> htList = new List<Hashtable>();
            //$edit091128,修改按前段设定的field顺序显示
            if (fields == null || fields.Trim() == "")
            {
                foreach (DataColumn col in dt.Columns)
                {
                    Hashtable ht = new Hashtable();
                    ht.Add(col.ColumnName, col.DataType.FullName);
                    htList.Add(ht);
                }
            }
            else
            {
                string[] fieldAry = fields.Split(',');
                for (int i = 0; i < fieldAry.Length; i++)
                {
                    if (dt.Columns.Contains(fieldAry[i]))
                    {
                        Hashtable ht = new Hashtable();
                        ht.Add(fieldAry[i], ((DataColumn)dt.Columns[fieldAry[i]]).DataType.FullName);
                        htList.Add(ht);
                    }
                }
            }

            try
            {
                columnJson = JsonHelper.ToJSON(htList);//获取Json字符串
            }
            catch (Exception ex)
            {
                columnJson = ex.Message;
            }
        }

        return columnJson;
    }

    protected string GetData(HttpContext context, string dsSessionName, int thetabIndex)
    {
        string module = context.Request["module"];
        string cmd = context.Request["command"];
        string regetFlag = context.Request["regetDataFlag"];

        bool allowPage = (!string.IsNullOrEmpty(context.Request["limit"]) && !string.IsNullOrEmpty(context.Request["start"]));

        // 从Session中获取CacheDataSet
        DataSet cacheData = context.Session[dsSessionName] as DataSet;
        DataTable cacheTable = cacheData.Tables[thetabIndex];
        string where = context.Request.Form["where"];

        //刪除空資料
        DataColumn[] keycols = cacheTable.PrimaryKey;
        if (cacheTable.Rows.Count == 1)
        {
            bool flag = true;
            for (int i = 0; i < keycols.Length; i++)
            {
                object keyvalue = (object)cacheTable.Rows[0][keycols[i].ColumnName.ToString()];
                if (genieJsonSerializer.IsNumeric(keycols[i].DataType.FullName.ToLower()))
                {
                    if (Convert.ToInt32(keyvalue) == 0 && keycols.Length == 1)
                    {
                        flag = false;
                        break;
                    }
                }
                else if (keyvalue == null || keyvalue.ToString() == "" || keyvalue == DBNull.Value)
                {
                    flag = false;
                    break;
                }
            }
            if (!flag)
                cacheTable.Rows.RemoveAt(0);
        }

        string fields = context.Request["fields"];
        string specialfields = context.Request["specialfields"];
        string checkfields = context.Request["checkfields"];

        string setsort = "";
        if (context.Request["setsort"] != null)
            setsort = context.Request["setsort"].ToString();

        //$edit100407 by navy :修改，取得前端傳過來的 Sort 
        //$edit091128.去掉Order by 語句
        string getwhere = "";
        if (where != null && where != "")
        {
            getwhere = where.ToUpper();
            if (getwhere.IndexOf("ORDER") > -1)
            {
                setsort = where.ToUpper().Substring(getwhere.IndexOf("ORDER") + 9);
                getwhere = getwhere.Substring(0, getwhere.IndexOf("ORDER"));
            }
        }

        //$edit20101209 by navy For如果沒有設定sort的話，取原來的Sort
        if (setsort == "")
        {
            string oldsql = CliUtils.GetSqlCommandText(module, cmd, CliUtils.fCurrentProject);
            if (oldsql.ToUpper().IndexOf("ORDER BY") > -1)
                setsort = oldsql.ToUpper().Substring(oldsql.ToUpper().IndexOf("ORDER BY") + 8);
        }

        string json = "";

        if (allowPage)
        {
            int pagesize = int.Parse(context.Request["limit"]);
            int start = int.Parse(context.Request["start"]);
            int packetCount = start / pagesize + 1;

            int totalRecordCount = returnTotal(module, cmd, where);

            int requiredRecordCount = pagesize * packetCount;

            if (start == 0 || (requiredRecordCount < totalRecordCount && requiredRecordCount > cacheTable.Rows.Count)
                || (requiredRecordCount >= totalRecordCount && totalRecordCount > cacheTable.Rows.Count))
            {
                // 创建InfoDataSet
                InfoDataSet ds = new InfoDataSet();
                ds.AlwaysClose = false;
                ds.RemoteName = string.Format("{0}.{1}", module, cmd);
                ds.ServerModify = true;
                ds.PacketRecords = pagesize * packetCount;
                ds.WhereStr = getwhere;
                //ds.OrderStr = setsort;
                ds.Active = true;
                if (cacheData != null)
                {
                    cacheData.Clear();
                    cacheData = ds.RealDataSet.Copy();
                    context.Session[dsSessionName] = cacheData;
                }
            }

            json = string.Format("{{totalProperty:{0},root:{1}}}",
                        totalRecordCount,
                        JsonHelper.dataTabletoJson(GetPageTable(cacheData.Tables[0], start, pagesize), fields, specialfields, checkfields, setsort));
        }
        else
        {
            if (Convert.ToBoolean(context.Request["isDetails"]))
            {
                if (!string.IsNullOrEmpty(where))
                {
                    DataRow[] rows = cacheTable.Select(where, setsort);

                    json = string.Format("{{totalProperty:{0},root:{1}}}",
                        rows.Length,
                        JsonHelper.dataRowtoJson(cacheTable.Columns, rows, fields, specialfields, checkfields));
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(where))
                {//$edit091223--alex:修改CRMM71中查詢更改where為where.Remove(where.IndexOf("(") + 1, where.IndexOf(".")) 
                    //where = (where.IndexOf(".") == -1) ? where : where.Remove(where.IndexOf("(") + 1, where.IndexOf("."));
                    //由於AnyQuery返回的條件是帶Table名的，而DataTable.Select()不支持帶Table名的條件，這裡先去掉Table名
                    if (regetFlag == "Y")
                    {
                        //$edit100115:因為如果不分頁的話，現在要將PacketRecord設-1，默認抓的資料太多了，所以修改成不分頁的時候也來重新抓一下.主要用於INV相關轉單程式，SQL比較單純的情況，如果SQL複雜，有別名，對where的處理要求高。
                        //string getwhere = "";
                        //if (where != "")
                        //{
                        //    getwhere = where.ToLower();
                        //    if (getwhere.IndexOf("order") > -1)
                        //        getwhere = getwhere.Substring(0, getwhere.IndexOf("order"));
                        //}

                        // 创建InfoDataSet
                        InfoDataSet ds = new InfoDataSet();
                        ds.AlwaysClose = false;
                        ds.RemoteName = string.Format("{0}.{1}", module, cmd);
                        ds.ServerModify = true;
                        ds.PacketRecords = -1;
                        ds.WhereStr = getwhere;
                        ds.OrderStr = setsort;
                        ds.Active = true;
                        if (cacheData != null)
                        {
                            cacheData.Clear();
                            cacheData = ds.RealDataSet.Copy();
                            context.Session[dsSessionName] = cacheData;
                        }

                        json = string.Format("{{totalProperty:{0},root:{1}}}",
                                cacheData.Tables[0].Rows.Count,
                                JsonHelper.dataTabletoJson(cacheData.Tables[0], fields, specialfields, checkfields, setsort));
                    }
                    else
                    {
                        where = where.Replace(cacheTable.TableName + ".", "");
                        DataRow[] rows = cacheTable.Select(where, setsort);

                        json = string.Format("{{totalProperty:{0},root:{1}}}",
                        rows.Length,
                        JsonHelper.dataRowtoJson(cacheTable.Columns, rows, fields, specialfields, checkfields));
                    }
                }
                else
                {
                    json = string.Format("{{totalProperty:{0},root:{1}}}",
                            cacheTable.Rows.Count,
                            JsonHelper.dataTabletoJson(cacheTable, fields, specialfields, checkfields, setsort));
                }
            }
        }

        return json;
    }

    //用于ExtAnyQuery取得的数组资料
    protected string getArrayData(HttpContext context)
    {
        string retAry = "";

        string module = context.Request["module"];
        string cmd = context.Request["command"];
        string textfield = context.Request["textfield"];
        string valuefield = context.Request["valuefield"];

        // 创建InfoDataSet
        InfoDataSet ds = new InfoDataSet();
        ds.AlwaysClose = false;
        ds.RemoteName = string.Format("{0}.{1}", module, cmd);
        ds.ServerModify = false;
        ds.PacketRecords = -1;
        ds.WhereStr = "";
        ds.Active = true;

        DataSet data = ds.RealDataSet;
        for (int i = 0; i < data.Tables[0].Rows.Count; i++)
        {
            if (retAry != "")
                retAry += ",";
            DataRow dr = data.Tables[0].Rows[i];
            retAry += "['" + dr[valuefield].ToString() + "','" + dr[textfield].ToString() + "']";
        }

        retAry = "[" + retAry + "]";
        return retAry;
    }

    protected string doneDetailDelete(HttpContext context, string dsSessionName, int thetabIndex)
    {
        string keyStr = context.Request.Form["keyParam"];

        DataSet sds = (DataSet)context.Session[dsSessionName];

        string retMsg = "";
        try
        {
            string[] keyAry = keyStr.Split(',');
            for (int i = 0; i < keyAry.Length; i++)
            {
                object[] keyobj = keyAry[i].Split('|');
                sds.Tables[thetabIndex].Rows.Find(keyobj).Delete();
            }
            retMsg = "{info:'Delete Success!'}";
        }
        catch (Exception ex)
        {
            string errmsg = ex.Message.Replace("\n", "<br>").Replace("\r", "").Replace("\\n", "<br>");
            retMsg = "{info:'Fail!-" + errmsg + "'}";
        }

        return retMsg;
    }

    protected string doneDetialSave(HttpContext context, string dsSessionName, int thetabIndex)
    {
        string keyStr = context.Request.Form["keyParam"];
        string getparam = context.Request.Form["data"];
        string checkfields = context.Request["checkfields"];
        string specialfields = context.Request["specialfields"];
        DataSet sds = (DataSet)context.Session[dsSessionName];
        string retMsg = "";

        try
        {
            //取得Client端异动的数据
            GeneralSearchResult gsr = GeneralSearchResult.GetTransformData(getparam);
            DataTable dt = gsr.RetrunData;

            DataTable sdt = sds.Tables[thetabIndex];

            //处理异动数据，更新到DB

            int len = dt.Rows.Count;
            for (int i = 0; i < len; i++)
            {
                if (dt.Rows[i]["genieEditType"].ToString() == "add")  // Add
                {
                    DataRow dr = sdt.NewRow();
                    for (int k = 0; k < dt.Columns.Count; k++)
                    {
                        string curColumnName = dt.Columns[k].ColumnName;
                        //$edit20100524 by navy For DB类型为varchar(8)的日期栏位处理
                        if (specialfields != null && JsonHelper.checkArray(specialfields.Split(','), curColumnName))
                        {
                            if (dt.Rows[i][curColumnName].ToString() != "")
                            {
                                string getdatastring = dt.Rows[i][curColumnName].ToString();
                                try
                                {
                                    DateTime condt = Convert.ToDateTime(getdatastring);
                                    dr[curColumnName] = condt.ToString("yyyyMMdd");
                                }
                                catch { }
                            }

                        } //-end $edit20100524 by navy
                        //$edit090813,增加特殊bool類型
                        else if (checkfields != null && JsonHelper.checkArray(checkfields.Split(','), curColumnName))
                        {
                            //$edit090825,由于前端不勾选时传回的是空字符串
                            try
                            {
                                if (dt.Rows[i][curColumnName].ToString() == "Y")
                                    dr[curColumnName] = "Y";
                                else if (dt.Rows[i][curColumnName].ToString() == "" || dt.Rows[i][curColumnName].ToString() == "N" || Convert.ToBoolean(dt.Rows[i][curColumnName]) == false)
                                    dr[curColumnName] = "N";
                                else
                                    dr[curColumnName] = "Y";
                            }
                            catch
                            {
                                dr[curColumnName] = "N";
                            }
                        }
                        else if (curColumnName != "genieEditType" && curColumnName != "gotoback")
                        {
                            //$edit090825
                            //string curDataType = dt.Columns[k].DataType.FullName.ToLower();
                            string curDataType = sdt.Columns[curColumnName].DataType.FullName.ToLower();
                            if (curDataType == "system.datetime")
                            {
                                if (dt.Rows[i][curColumnName] != null && dt.Rows[i][curColumnName].ToString() != "")
                                    dr[curColumnName] = Convert.ToDateTime(dt.Rows[i][curColumnName]);
                                else
                                    dr[curColumnName] = DBNull.Value;
                            }
                            else if (genieJsonSerializer.IsNumeric(curDataType) || curDataType == "system.boolean")
                            {
                                dr[curColumnName] = dt.Rows[i][curColumnName].ToString() != "" ? dt.Rows[i][curColumnName] : DBNull.Value;
                            }
                            else
                                dr[curColumnName] = dt.Rows[i][curColumnName];
                        }
                    }
                    sdt.Rows.Add(dr);
                }
                else        //Edit
                {
                    DataRow dr = null;
                    string[] KeyAry = keyStr.Split('|');
                    object[] objKey = new object[KeyAry.Length];
                    for (int j = 0; j < KeyAry.Length; j++)
                    {
                        objKey[j] = dt.Rows[i][KeyAry[j]].ToString();
                    }
                    dr = sdt.Rows.Find(objKey);

                    for (int k = 0; k < dt.Columns.Count; k++)
                    {
                        string curColumnName = dt.Columns[k].ColumnName;
                        //$edit20100524 by navy For DB类型为varchar(8)的日期栏位处理
                        if (specialfields != null && JsonHelper.checkArray(specialfields.Split(','), curColumnName))
                        {
                            if (dt.Rows[i][curColumnName].ToString() != "")
                            {
                                string getdatastring = dt.Rows[i][curColumnName].ToString();
                                try
                                {
                                    DateTime condt = Convert.ToDateTime(getdatastring);
                                    dr[curColumnName] = condt.ToString("yyyyMMdd");
                                }
                                catch { }
                            }

                        } //-end $edit20100524 by navy
                        //$edit090813,增加特殊bool類型
                        else if (checkfields != null && JsonHelper.checkArray(checkfields.Split(','), curColumnName))
                        {
                            //$edit090825,由于前端不勾选时传回的是空字符串
                            try
                            {
                                if (dt.Rows[i][curColumnName].ToString() == "Y")
                                    dr[curColumnName] = "Y";
                                else if (dt.Rows[i][curColumnName].ToString() == "" || dt.Rows[i][curColumnName].ToString() == "N" || Convert.ToBoolean(dt.Rows[i][curColumnName]) == false)
                                    dr[curColumnName] = "N";
                                else
                                    dr[curColumnName] = "Y";
                            }
                            catch
                            {
                                dr[curColumnName] = "N";
                            }
                        }
                        else if (curColumnName != "genieEditType" && curColumnName != "gotoback")
                        {
                            //$edit090825
                            //string curDataType = dt.Columns[k].DataType.FullName.ToLower();
                            string curDataType = sdt.Columns[curColumnName].DataType.FullName.ToLower();
                            if (curDataType == "system.datetime")
                            {
                                if (dt.Rows[i][curColumnName] != null && dt.Rows[i][curColumnName].ToString() != "")
                                    dr[curColumnName] = Convert.ToDateTime(dt.Rows[i][curColumnName]);
                                else
                                    dr[curColumnName] = DBNull.Value;
                            }
                            else if (genieJsonSerializer.IsNumeric(curDataType) || curDataType == "system.boolean")
                            {
                                dr[curColumnName] = dt.Rows[i][curColumnName].ToString() != "" ? dt.Rows[i][curColumnName] : DBNull.Value;
                            }
                            else
                                dr[curColumnName] = dt.Rows[i][curColumnName];
                        }
                    }
                }
            }

            retMsg = "{info:'Save OK!'}";
        }
        catch (Exception ex)
        {
            string errmsg = ex.Message.Replace("\n", "<br>").Replace("\r", "").Replace("\\n", "<br>");
            retMsg = "{info:'Fail!-" + errmsg + "'}";
        }

        return retMsg;
    }

    protected string getComboData(HttpContext context)
    {
        string module = context.Request["module"];
        string cmd = context.Request["command"];
        string dbTab = CliUtils.GetTableName(module, cmd, CliUtils.fCurrentProject);
        ArrayList tabKeys = CliUtils.GetKeyFields(module, cmd, CliUtils.fCurrentProject);
        string fields = context.Request["fields"];

        InfoDataSet ds = new InfoDataSet();
        ds.AlwaysClose = false;
        ds.RemoteName = string.Format("{0}.{1}", module, cmd);

        string json = "";
        int pagesize = int.Parse(context.Request["limit"]);

        if (pagesize > 0)
        {
            int start = int.Parse(context.Request["start"]);
            int packetCount = start / pagesize + 1;

            ds.PacketRecords = pagesize * packetCount;
            ds.Active = true;

            //處理Combo的search:或自動發送查詢關鍵字過來
            string setstr = "";
            if (context.Request["query"] != null && context.Request["query"].ToString() != "")
            {
                setstr = tabKeys[0].ToString() + " like '" + context.Request["query"].ToString() + "%'";
                ds.SetWhere(setstr);
            }

            //處理Combo的Query
            if (context.Request["querywhere"] != null && context.Request["querywhere"].ToString() != "")
            {
                try
                {
                    setstr = context.Request["querywhere"].ToString();
                    ds.SetWhere(setstr);
                }
                catch { }
            }

            DataTable table = ds.RealDataSet.Tables[cmd];

            json = string.Format("{{totalProperty:{0},root:{1}}}",
                    returnTotal(module, cmd, setstr),
                    genieJsonSerializer.TableToJsonArray(GetPageTable(table, start, pagesize), fields.Split('|')));
        }
        else
        {
            ds.PacketRecords = -1;
            ds.Active = true;

            //處理Combo的search:或自動發送查詢關鍵字過來
            string setstr = "";
            if (context.Request["query"] != null && context.Request["query"].ToString() != "")
            {
                setstr = tabKeys[0].ToString() + " like '" + context.Request["query"].ToString() + "%'";
                ds.SetWhere(setstr);
            }

            //處理Combo的Query
            if (context.Request["querywhere"] != null && context.Request["querywhere"].ToString() != "")
            {
                try
                {
                    setstr = context.Request["querywhere"].ToString();
                    ds.SetWhere(setstr);
                }
                catch { }
            }

            DataTable table = ds.RealDataSet.Tables[cmd];

            json = string.Format("{{totalProperty:{0},root:{1}}}",
                    table.Rows.Count,
                    genieJsonSerializer.TableToJsonArray(table, fields.Split('|')));
        }

        return json;
    }

    //$edit20100719 by navy For 添加GexRefval取得欄位的方法，前衛前端不會貼WebDataSource,所以這裡方法重寫
    protected string getRefDataColumn(HttpContext context)
    {
        //只要ds不为null，则不管该表是否有数据，都有数据列生成
        string columnJson = "";
        string fields = context.Request["fields"];
        string module = context.Request["module"];
        string cmd = context.Request["command"];

        string dynamic = context.Request["refdynamic"];
        string cmdsql = context.Request["refcmdsql"];

        //string dbTab = CliUtils.GetTableName(module, cmd, CliUtils.fCurrentProject);
        //ArrayList tabKeys = CliUtils.GetKeyFields(module, cmd, CliUtils.fCurrentProject);
        DataTable dt = new DataTable();
        try
        {
            if (dynamic == "Y")
            {
                InfoDataSet ds = new InfoDataSet();
                ds.AlwaysClose = true;
                ds.WhereStr = "1=0";
                ds.RemoteName = "GLModule.cmdRefValUse";
                ds.CommandText = cmdsql;
                ds.Active = true;
                dt = ds.RealDataSet.Tables[0];
            }
            else
            {
                InfoDataSet ds = new InfoDataSet();
                ds.AlwaysClose = true;
                ds.RemoteName = string.Format("{0}.{1}", module, cmd);
                ds.PacketRecords = 1;
                ds.WhereStr = "1=0";
                ds.Active = true;
                dt = ds.RealDataSet.Tables[cmd];
            }
        }
        catch (Exception ex)
        {
            return ex.Message;
        }

        DataColumn[] columnAry = dt.PrimaryKey;

        if (dt != null)
        {
            //string mKeyvalue = "";
            List<Hashtable> htList = new List<Hashtable>();
            //$修改按前段设定的field顺序显示
            if (fields == null || fields.Trim() == "")
            {
                foreach (DataColumn col in dt.Columns)
                {
                    Hashtable ht = new Hashtable();
                    ht.Add(col.ColumnName, col.DataType.FullName);
                    htList.Add(ht);
                }
            }
            else
            {
                string[] fieldAry = fields.Split(',');
                for (int i = 0; i < fieldAry.Length; i++)
                {
                    string curFieldName = fieldAry[i];
                    if (curFieldName.IndexOf('.') > -1)
                        curFieldName = curFieldName.Substring(curFieldName.IndexOf('.') + 1);

                    if (dt.Columns.Contains(curFieldName))
                    {
                        Hashtable ht = new Hashtable();
                        ht.Add(curFieldName, ((DataColumn)dt.Columns[curFieldName]).DataType.FullName);
                        htList.Add(ht);
                    }
                }
            }

            try
            {
                columnJson = JsonHelper.ToJSON(htList);//获取Json字符串
            }
            catch (Exception ex)
            {
                columnJson = ex.Message;
            }
        }

        return columnJson;
    }

    //$edit20100719 by navy For 添加GexRefVal的取數據方法，類似于Combo，我們默認規劃RefVal一定是分頁的，所以暫時不考慮不分頁的情況
    protected string getRefvalData(HttpContext context)
    {
        string module = context.Request["module"];
        string cmd = context.Request["command"];
        string fields = context.Request["fields"];
        string specialfields = context.Request["specialfields"];
        string checkfields = context.Request["checkfields"];
        string setsort = context.Request["setsort"];
        string wherestr = context.Request["where"];
        string noquerycolumns = context.Request["refnoquerycolumns"];
        if (wherestr == null)
            wherestr = "";

        //增加GexRefVal动态SQL语句取数据，
        string dynamic = context.Request["refdynamic"];
        string cmdsql = context.Request["refcmdsql"];

        //string dbTab = CliUtils.GetTableName(module, cmd, CliUtils.fCurrentProject);
        //ArrayList tabKeys = CliUtils.GetKeyFields(module, cmd, CliUtils.fCurrentProject);
        string json = "";
        int pagesize = 10;
        if (context.Request["limit"] != null)
            pagesize = int.Parse(context.Request["limit"]);

        InfoDataSet ds = new InfoDataSet();
        ds.AlwaysClose = false;
        ds.WhereStr = wherestr;

        if (pagesize > 0)
        {
            int start = context.Request["start"] != null ? int.Parse(context.Request["start"]) : 0;
            int packetCount = start / pagesize + 1;
            try
            {
                if (dynamic == "Y")
                {
                    ds.PacketRecords = -1;
                    ds.RemoteName = "GLModule.cmdRefValUse";
                    ds.CommandText = cmdsql;
                    ds.Active = true;
                }
                else
                {
                    ds.PacketRecords = pagesize * packetCount;
                    ds.RemoteName = string.Format("{0}.{1}", module, cmd);
                    ds.Active = true;
                }
                string filterConditions = context.Request["filterConditions"];
                if (!string.IsNullOrEmpty(filterConditions))
                {
                    DataTable dt = ds.RealDataSet.Tables[0];
                    string tableName = "";
                    if (dynamic == "Y")
                        tableName = CliUtils.GetTableName(cmdsql);
                    else
                        tableName = CliUtils.GetTableName(module, cmd, CliUtils.fCurrentProject);

                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    object[] filters = serializer.DeserializeObject(filterConditions) as object[];
                    foreach (Dictionary<string, object> filter in filters)
                    {
                        string defval = "";
                        Type dataType = dt.Columns[filter["field"].ToString()].DataType;
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

                        if (string.IsNullOrEmpty(wherestr))
                        {
                            if (CliUtils.GetDataBaseType() == ClientType.ctMsSql)
                            {
                                wherestr = string.Format("[{0}].[{1}]{2}{3}{4}{3}", tableName, filter["field"].ToString(), op, quote, defval);
                            }
                            else
                            {
                                wherestr = string.Format("{0}.{1}{2}{3}{4}{3}", tableName, filter["field"].ToString(), op, quote, defval);
                            }
                        }
                        else
                        {
                            if (CliUtils.GetDataBaseType() == ClientType.ctMsSql)
                            {
                                wherestr += string.Format(" {0} [{1}].[{2}]{3}{4}{5}{4}", filter["condition"].ToString(), tableName, filter["field"].ToString(), op, quote, defval);
                            }
                            else
                            {
                                wherestr += string.Format(" {0} {1}.[{2}]{3}{4}{5}{4}", filter["condition"].ToString(), tableName, filter["field"].ToString(), op, quote, defval);
                            }
                        }
                    }
                    if (wherestr != "")
                        ds.SetWhere(wherestr);
                }
                //處理關鍵字search:這裡要對顯示出來的所有欄位做查詢
                string setstr = "";
                if (context.Request["query"] != null && context.Request["query"].ToString() != "")
                {
                    DataTable dt = ds.RealDataSet.Tables[0];
                    string tableName = "";
                    if (dynamic == "Y")
                        tableName = CliUtils.GetTableName(cmdsql);
                    else
                        tableName = CliUtils.GetTableName(module, cmd, CliUtils.fCurrentProject);

                    string keystr = context.Request["query"].ToString();
                    if (fields != "")
                    {
                        //$edit20101220 by navy for 有些欄位，是取function的，不能作為查詢
                        string[] noqueryColumnAry = noquerycolumns.Split(',');

                        string[] fieldAry = fields.Split(',');
                        for (int i = 0; i < fieldAry.Length; i++)
                        {
                            string setCol = fieldAry[i];

                            if (!JsonHelper.checkArray(noqueryColumnAry, setCol))
                            {
                                if (setCol.IndexOf(".") == -1)
                                    setCol = tableName + "." + setCol;
                                setstr += string.Format(setCol + " like '%{0}%' or ", keystr);
                                //if (i != fieldAry.Length - 1)
                                //    setstr += " or ";
                            }
                        }
                        if (setstr != "")
                            setstr = setstr.Substring(0, setstr.Length - 3);
                    }
                    else
                    {
                        //顯示全部欄位的話，就要抓出所有欄位來下條件，不過這種情況很少，一般對RefVal會限制顯示的欄位的
                        int x = 0;
                        foreach (DataColumn col in dt.Columns)
                        {
                            x++;
                            setstr += string.Format(tableName + "." + col.ColumnName + " like '%{0}%'", keystr);
                            if (x != dt.Columns.Count)
                                setstr += " or ";
                        }
                    }
                    if (setstr != "")
                    {
                        setstr = "( " + setstr + " )";
                        ds.SetWhere(setstr);

                        if (wherestr == "")
                            wherestr = setstr;
                        else
                            wherestr += " AND " + setstr;
                    }
                }
            }
            catch (Exception ex)
            {

            }

            DataTable table = ds.RealDataSet.Tables[0];
            int dstotal = table.Rows.Count;
            if (dynamic != "Y")
            {
                dstotal = returnTotal2(module, cmd, wherestr, cmdsql) != 0 ? returnTotal2(module, cmd, wherestr, cmdsql) : table.Rows.Count;
            }

            json = string.Format("{{total:{0},root:{1}}}", dstotal,
                    JsonHelper.dataTabletoJson(GetPageTable(table, start, pagesize), fields, specialfields, checkfields, setsort));
            //json = string.Format("{{total:{0},data:{1}}}", dstotal,
            //        JsonHelper.dataTabletoJson(GetPageTable(table, start, pagesize), fields, specialfields, checkfields, setsort));
        }
        else { }

        return json;
    }

    //$edit20100728 by navy For 手動輸入ID或前面columnMathch待會時，自動抓出Name
    protected string getRefRow(HttpContext context)
    {
        string module = context.Request["module"];
        string cmd = context.Request["command"];
        string fields = context.Request["fields"];
        string refvalue = context.Request["refvalue"];
        string refvaluecolumn = context.Request["refvaluecolumn"];
        string reftextcolumn = context.Request["reftextcolumn"];

        //增加GexRefVal动态SQL语句取数据，
        string dynamic = context.Request["refdynamic"];
        string cmdsql = context.Request["refcmdsql"];

        string tableName = "";
        if (dynamic == "Y")
            tableName = CliUtils.GetTableName(cmdsql);
        else
            tableName = CliUtils.GetTableName(module, cmd, CliUtils.fCurrentProject);

        if (refvaluecolumn.IndexOf('.') > -1)
            refvaluecolumn = tableName + refvaluecolumn.Substring(refvaluecolumn.IndexOf('.'));
        else
            refvaluecolumn = tableName + "." + refvaluecolumn;

        string returnRow = "";
        string wherestr = refvaluecolumn + " =N'" + refvalue + "'";

        try
        {
            InfoDataSet ds = new InfoDataSet();
            ds.AlwaysClose = false;
            ds.PacketRecords = -1;
            ds.WhereStr = wherestr;
            if (dynamic == "Y")
            {
                ds.RemoteName = "GLModule.cmdRefValUse";
                ds.CommandText = cmdsql;
            }
            else
            {
                ds.RemoteName = string.Format("{0}.{1}", module, cmd);
            }
            ds.Active = true;

            DataTable table = ds.RealDataSet.Tables[0];
            if (table.Rows.Count >= 0 && table.Columns.Contains(reftextcolumn))
            {
                returnRow = string.Format("{{flag:'Y',count:{0},rowdata:{1}}}", table.Rows.Count, JsonHelper.dataTabletoJson(table));
            }
        }
        catch (Exception ex)
        {
            returnRow = string.Format("{{flag:'N',count:'0',rowdata:{0}}}", ex.Message);
        }
        return returnRow;
    }

    DataTable GetPageTable(DataTable srcTable, int startIndex, int pagesize)
    {
        int rowCount = srcTable.Rows.Count;
        DataTable tab = srcTable.Clone();
        int maxIndex = Math.Min(startIndex + pagesize, rowCount);
        for (int i = startIndex; i < maxIndex; i++)
        {
            tab.ImportRow(srcTable.Rows[i]);
        }
        return tab;
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

    protected string getCallMethod(HttpContext context)
    {
        string returnMsg = "";
        string returnValue = "";
        string returnObj = "";

        string moduleName = context.Request["module"];
        string methodName = context.Request["method"];
        string returnType = context.Request["retype"];
        object[] objParam = context.Request["param"].Split('|');

        try
        {
            object[] objRet = CliUtils.CallMethod(moduleName, methodName, objParam);
            if (objRet != null && (int)objRet[0] == 0)
            {
                //returnType是定義servermethod的返回類型，目前暫定：0:無返回值； 1:返回單一值； 2:返回一組值，
                switch (returnType)
                {
                    case "0":
                        break;
                    case "1":
                        returnValue = Convert.ToString(objRet[1]);
                        break;
                    case "2"://$Edit 20100713 by alex：實例為INVM16
                        returnValue = Convert.ToString(objRet[1]);
                        if (objRet.Length > 2)
                            returnObj = (string)objRet[2];
                        break;
                    default:
                        break;
                }
            }
            returnMsg = "yes";
        }
        catch (Exception ex)
        {
            returnMsg = "no";
            returnValue = ex.Message;
        }

        string Msg = "({info:'" + returnMsg + "',value:'" + returnValue + "',msg:'" + returnObj + "'})";
        return Msg;
    }

    protected int returnTotal(string module, string cmd, string where)
    {
        int totalRecordCount = 100;
        string tableName = CliUtils.GetTableName(module, cmd, CliUtils.fCurrentProject);
        string sql = "select count(*) from " + tableName + " where 1=1 ";
        //$edit091013:防止order在where中的情况
        if (where != null)
        {
            where = where.ToLower();
            if (where.IndexOf("order") > -1)
                where = where.Substring(0, where.IndexOf("order"));
        }
        //$edit091013--end
        if (where != null && where != "")
            sql += " and " + where;
        //條用EEP提供的取資料筆數的方法，但是對於複雜的SQL還是沒辦法去除，如有Union
        try
        {
            totalRecordCount = CliUtils.GetRecordsCount(module, cmd, where, CliUtils.fCurrentProject);
        }
        catch
        {
            DataSet totalDS = CliUtils.ExecuteSql(module, cmd, sql, true, CliUtils.fCurrentProject);
            if (totalDS != null)
                totalRecordCount = Convert.ToInt32(totalDS.Tables[0].Rows[0][0]);
        }
        return totalRecordCount;
    }

    //$edit20100814 by navy For 上面取筆數還是有問題
    protected int returnTotal2(string module, string cmd, string where)
    {
        return returnTotal2(module, cmd, where, "");
    }

    protected int returnTotal2(string module, string cmd, string where, string sql)
    {
        //注意，暫時不支持 後端很複雜的語句，如有子查詢或Union等，如果是那種的，請將packetreord設-1
        int totalRecordCount = 0;
        if (sql == "")
            sql = CliUtils.GetSqlCommandText(module, cmd, CliUtils.fCurrentProject);

        if (where != null && where != "")
        {
            sql = sql.ToUpper();
            if (sql.IndexOf("WHERE") > -1)
                sql = sql.Replace("WHERE", "WHERE " + where + " AND ");
            else
            {
                if (sql.IndexOf("ORDER BY") > -1)
                    sql = sql.Replace("ORDER BY", " WHERE " + where + " ORDER BY");
                else
                    sql = sql + " WHERE " + where;
            }
        }

        try
        {
            try
            {
                string sql2 = "SELECT COUNT(*) " + sql.Substring(sql.IndexOf("FROM"));
                DataSet totalDS = CliUtils.ExecuteSql(module, cmd, sql2, true, CliUtils.fCurrentProject);
                if (totalDS != null && totalDS.Tables[0].Rows.Count == 1)
                    totalRecordCount = Convert.ToInt32(totalDS.Tables[0].Rows[0][0]);
            }
            catch
            {
                DataSet totalDS = CliUtils.ExecuteSql(module, cmd, sql, true, CliUtils.fCurrentProject);
                if (totalDS != null)
                    totalRecordCount = totalDS.Tables[0].Rows.Count;
            }
        }
        catch
        {
            try
            {
                totalRecordCount = CliUtils.GetRecordsCount(module, cmd, where, CliUtils.fCurrentProject);
            }
            catch
            {
                totalRecordCount = 0;
            }
        }

        return totalRecordCount;
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}