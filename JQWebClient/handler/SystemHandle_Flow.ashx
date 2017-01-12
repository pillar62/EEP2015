<%@ WebHandler Language="C#" Class="SystemHandle_Flow" %>

using System;
using System.Web;
using Newtonsoft;
using Newtonsoft.Json;
using System.Data;
using EFClientTools.EFServerReference;
using System.Collections.Generic;
using System.Web.SessionState;
using System.Linq;
using System.Collections;

public class SystemHandle_Flow : IHttpHandler, IRequiresSessionState
{
    Dictionary<string, object> returnDic = new Dictionary<string, object>();
    Dictionary<string, object> paraDic = new Dictionary<string, object>();
    Dictionary<string, object> masterKeysDic = new Dictionary<string, object>();
    Dictionary<string, object> keysDic = new Dictionary<string, object>();
    HttpContext hContext;
    public void ProcessRequest(HttpContext context)
    {
        try
        {
            hContext = context;
            //object a = null;
            //a.ToString();
            String messageKey = "";
            EFBase.MessageProvider provider = new EFBase.MessageProvider(hContext.Request.PhysicalApplicationPath, EFClientTools.ClientUtility.ClientInfo.Locale);

            context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            String user = EFClientTools.ClientUtility.ClientInfo.UserID;
            EFClientTools.ClientUtility.ClientInfo.UseDataSet = true;
            String filter = hContext.Request.Form["Filter"];
            filter = hContext.Server.UrlDecode(filter);
            String queryParam = hContext.Request.Form["QueryParam"];
            var pageSize = -1;
            if (context.Request.Form["rows"] != null)
            {
                int.TryParse(context.Request.Form["rows"].ToString(), out pageSize);
            }
            var pageIndex = 1;
            if (context.Request.Form["page"] != null)
            {
                int.TryParse(context.Request.Form["page"].ToString(), out pageIndex);
            }

            if (hContext.Request.QueryString["Type"] == "download")
            {
                var fileName = context.Request.QueryString["fileName"];
                var path = string.Format("../WorkflowFiles/{0}", fileName);

                System.IO.FileInfo file = new System.IO.FileInfo(HttpContext.Current.Server.MapPath(path));
                context.Response.Clear();
                context.Response.Buffer = false;
                context.Response.ContentType = "application/octet-stream";
                var filename = HttpUtility.UrlEncode(System.Text.Encoding.UTF8.GetBytes(file.Name));
                context.Response.AddHeader("Content-Disposition", "attachment; filename=" + filename);
                context.Response.AddHeader("Content-Length", file.Length.ToString());
                context.Response.Filter.Close();
                context.Response.WriteFile(file.FullName);
                context.Response.End();
                EFClientTools.ClientUtility.ClientInfo.cErrorCode = 0;
            }


            if (hContext.Request.Form["Type"] == "Encrypt")
            {
                var param = hContext.Request.Form["param"];
                var key = Guid.NewGuid().ToString("N");

                var encryptParam = JQClientTools.JQScriptManager.EncryptParameters(param, key);
                var a = JQClientTools.JQScriptManager.DecryptParameters(encryptParam, key);
                hContext.Response.Write(string.Format("param={0}&key={1}", HttpUtility.UrlEncode(encryptParam), key));
                return;
            }
            else if (hContext.Request.Form["Type"] == "ToDoList" || hContext.Request.Form["Type"] == "ToDoHis"
                || hContext.Request.Form["Type"] == "FlowRunOver")
            {
                DataSet ds = null;
                if (hContext.Request.Form["Type"] == "ToDoList")
                {
                    //增加一个参数，用来取得某一条代办
                    FlowDataParameter param = new FlowDataParameter();
                    if (hContext.Request.Form["listID"] != null)
                    {
                        param = new FlowDataParameter();
                        param.ListID = new Guid(hContext.Request.Form["listID"]);
                    }
                    if (!String.IsNullOrEmpty(filter) || !String.IsNullOrEmpty(queryParam))
                    {
                        String realFilter = "";
                        if (!String.IsNullOrEmpty(filter))
                        {
                            realFilter += filter + " and ";
                        }
                        if (!String.IsNullOrEmpty(queryParam))
                        {
                            realFilter += String.Format("(D_STEP_ID like '%{0}%' or USERNAME like '%{0}%' or SENDTO_NAME like '%{0}%' or FORM_PRESENT_CT like '%{0}%' or REMARK like '%{0}%') and ", queryParam);
                        }
                        realFilter = realFilter.Remove(realFilter.LastIndexOf("and"));
                        param.Description = realFilter;
                    }
                    param.StartIndex = (pageIndex - 1) * pageSize;
                    param.Count = pageSize;
                    param.OrderBy = "";

                    if (!String.IsNullOrEmpty(hContext.Request.Form["sort"]))
                    {
                        if (hContext.Request.Form["sort"] == "UPDATE_WHOLE_TIME")
                            param.OrderBy = String.Format("UPDATE_DATE {0}, UPDATE_TIME {0}", hContext.Request.Form["order"]);
                        else
                            param.OrderBy = String.Format("{0} {1}", hContext.Request.Form["sort"], hContext.Request.Form["order"]);
                    }

                    var ds1 = EFClientTools.ClientUtility.Client.GetFlowDataDS(EFClientTools.ClientUtility.ClientInfo, FlowDataType.Do, param);
                    ds = Deserialize<DataSet>(ds1);
                    //ESqlMode mode = (ESqlMode)Enum.Parse(typeof(ESqlMode), hContext.Request.Form["Type"]);
                    //ds = EFClientTools.ClientUtility.ExecuteSQL(EFClientTools.ClientUtility.ClientInfo.Database, GetFlowSql(user, mode));
                }
                else if (hContext.Request.Form["Type"] == "ToDoHis")
                {
                    FlowDataParameter param = new FlowDataParameter();
                    if (!String.IsNullOrEmpty(filter) || !String.IsNullOrEmpty(queryParam))
                    {
                        String realFilter = "";
                        if (!String.IsNullOrEmpty(filter))
                        {
                            realFilter += filter + " and ";
                        }
                        if (!String.IsNullOrEmpty(queryParam))
                        {
                            realFilter += String.Format("(D_STEP_ID like '%{0}%' or USERNAME like '%{0}%' or SENDTO_NAME like '%{0}%' or FORM_PRESENT_CT like '%{0}%' or REMARK like '%{0}%') and ", queryParam);
                        }
                        realFilter = realFilter.Remove(realFilter.LastIndexOf("and"));
                        param.Description = realFilter;
                    }
                    param.StartIndex = (pageIndex - 1) * pageSize;
                    param.Count = pageSize;
                    if (!String.IsNullOrEmpty(hContext.Request.Form["sort"]))
                    {
                        if (hContext.Request.Form["sort"] == "UPDATE_WHOLE_TIME")
                            param.OrderBy = String.Format("UPDATE_DATE {0}, UPDATE_TIME {0}", hContext.Request.Form["order"]);
                        else
                            param.OrderBy = String.Format("{0} {1}", hContext.Request.Form["sort"], hContext.Request.Form["order"]);
                    }
                    var ds1 = EFClientTools.ClientUtility.Client.GetFlowDataDS(EFClientTools.ClientUtility.ClientInfo, FlowDataType.History, param);
                    ds = Deserialize<DataSet>(ds1);
                    //ESqlMode mode = (ESqlMode)Enum.Parse(typeof(ESqlMode), hContext.Request.Form["Type"]);
                    //ds = EFClientTools.ClientUtility.ExecuteSQL(EFClientTools.ClientUtility.ClientInfo.Database, GetFlowSql(user, mode));
                }
                else if (hContext.Request.Form["Type"] == "FlowRunOver")
                {
                    //?????????????
                    String realFilter = "";
                    if (!String.IsNullOrEmpty(filter) || !String.IsNullOrEmpty(queryParam))
                    {
                        if (!String.IsNullOrEmpty(filter))
                        {
                            realFilter += filter + " and ";
                        }
                        if (!String.IsNullOrEmpty(queryParam))
                        {
                            realFilter += String.Format("(D_STEP_ID like '%{0}%' or USERNAME like '%{0}%' or SENDTO_NAME like '%{0}%' or FORM_PRESENT_CT like '%{0}%' or REMARK like '%{0}%') and ", queryParam);
                        }
                        realFilter = realFilter.Remove(realFilter.LastIndexOf("and"));
                    }
                    FlowDataParameter param = new FlowDataParameter();
                    param.Description = realFilter;
                    param.StartIndex = (pageIndex - 1) * pageSize;
                    param.Count = pageSize;
                    if (!String.IsNullOrEmpty(hContext.Request.Form["sort"]))
                    {
                        if (hContext.Request.Form["sort"] == "UPDATE_WHOLE_TIME")
                            param.OrderBy = String.Format("UPDATE_DATE {0}, UPDATE_TIME {0}", hContext.Request.Form["order"]);
                        else
                            param.OrderBy = String.Format("{0} {1}", hContext.Request.Form["sort"], hContext.Request.Form["order"]);
                    }
                    var ds1 = EFClientTools.ClientUtility.Client.GetFlowDataDS(EFClientTools.ClientUtility.ClientInfo, FlowDataType.End, param);
                    ds = Deserialize<DataSet>(ds1);
                    //ESqlMode mode = (ESqlMode)Enum.Parse(typeof(ESqlMode), hContext.Request.Form["Type"]);
                    //ds = EFClientTools.ClientUtility.ExecuteSQL(EFClientTools.ClientUtility.ClientInfo.Database, GetFlowSql(user, mode));
                }

                if (ds.Tables.Count > 0)
                {
                    DataView view = new DataView(ds.Tables[0]);
                    //if (String.IsNullOrEmpty(hContext.Request.Form["sort"]))
                    //{
                    //    view.Sort = "UPDATE_DATE DESC, UPDATE_TIME DESC";
                    //}
                    //else if (hContext.Request.Form["sort"] != "UPDATE_WHOLE_TIME")
                    //{
                    //    view.Sort = String.Format("{0} {1}", hContext.Request.Form["sort"], hContext.Request.Form["order"]);
                    //}
                    if (hContext.Request.Form["Type"] != "FlowRunOver")
                    {
                        messageKey = "FLDesigner/FLDesigner/Item3";
                        string[] Item3 = provider[messageKey].Split(',');
                        foreach (DataRowView item in view)
                        {
                            foreach (var item1 in Item3)
                            {
                                if (item["STATUS"] != null && item1.StartsWith(item["STATUS"].ToString() + ":"))
                                {
                                    item["STATUS"] = item1.Replace(item["STATUS"].ToString() + ":", string.Empty);
                                    break;
                                }
                            }
                        }

                        ds.Tables[0].Columns.Add(new DataColumn("IsDelay"));
                        ds.Tables[0].Columns.Add(new DataColumn("UPDATE_WHOLE_TIME"));
                        foreach (DataRowView item in view)
                        {
                            string TIME_UNIT = item["TIME_UNIT"].ToString();
                            string FLOWURGENT = item["FLOWURGENT"].ToString();
                            string UPDATE_DATE = item["UPDATE_DATE"].ToString();
                            string UPDATE_TIME = item["UPDATE_TIME"].ToString();
                            string UPDATE_WHOLE_TIME = UPDATE_DATE + " " + UPDATE_TIME;
                            item["UPDATE_WHOLE_TIME"] = UPDATE_WHOLE_TIME;
                            string URGENT_TIME = item["URGENT_TIME"].ToString();
                            string EXP_TIME = item["EXP_TIME"].ToString();
                            if (IsOverTime(TIME_UNIT, FLOWURGENT, UPDATE_DATE, UPDATE_TIME, URGENT_TIME, EXP_TIME))
                            {
                                item["IsDelay"] = "1";
                            }
                            else
                            {
                                item["IsDelay"] = "0";
                            }
                        }
                    }
                    else
                    {
                        messageKey = "FLDesigner/FLDesigner/Item3";
                        string[] Item3 = provider[messageKey].Split(',');
                        ds.Tables[0].Columns.Add(new DataColumn("UPDATE_WHOLE_TIME"));
                        foreach (DataRowView item in view)
                        {
                            string UPDATE_DATE = item["UPDATE_DATE"].ToString();
                            string UPDATE_TIME = item["UPDATE_TIME"].ToString();
                            string UPDATE_WHOLE_TIME = UPDATE_DATE + " " + UPDATE_TIME;
                            item["UPDATE_WHOLE_TIME"] = UPDATE_WHOLE_TIME;
                            foreach (var item1 in Item3)
                            {
                                if (item["STATUS"] != null && item1.StartsWith(item["STATUS"].ToString() + ":"))
                                {
                                    item["STATUS"] = item1.Replace(item["STATUS"].ToString() + ":", string.Empty);
                                    break;
                                }
                            }
                        }
                    }
                    if (hContext.Request.Form["Type"] != "FlowRunOver")
                    {
                        //if (!String.IsNullOrEmpty(filter) || !String.IsNullOrEmpty(queryParam))
                        //{
                        //    String realFilter = "";
                        //    if (!String.IsNullOrEmpty(filter))
                        //    {
                        //        realFilter += filter + " and ";
                        //    }
                        //    if (!String.IsNullOrEmpty(queryParam))
                        //    {
                        //        realFilter += String.Format("(D_STEP_ID like '%{0}%' or USERNAME like '%{0}%' or SENDTO_NAME like '%{0}%' or FORM_PRESENT_CT like '%{0}%' or REMARK like '%{0}%') and ", queryParam);
                        //    }
                        //    realFilter = realFilter.Remove(realFilter.LastIndexOf("and"));
                        //    DataRow[] drs = view.ToTable().Select(realFilter);
                        //    if (drs.Count() > 0)
                        //    {
                        //        DataTable dt = new DataTable();
                        //        foreach (DataColumn column in drs[0].Table.Columns)
                        //        {
                        //            DataColumn newColumn = new DataColumn(column.ColumnName, column.DataType);
                        //            dt.Columns.Add(newColumn);
                        //        }
                        //        foreach (DataRow row in drs)
                        //        {
                        //            DataRow newRow = dt.NewRow();
                        //            foreach (DataColumn column in dt.Columns)
                        //            {
                        //                newRow[column.ColumnName] = row[column.ColumnName];
                        //            }
                        //            dt.Rows.Add(newRow);
                        //        }
                        //        context.Response.Write(DataTableToJson(dt, true));
                        //    }
                        //    else
                        //        context.Response.Write("[]");
                        //}
                        //else
                        //{
                        context.Response.Write(DataTableToJson2(view.ToTable(), ds.Tables[0].ExtendedProperties["Count"]));
                        //}
                    }
                    else
                    {
                        context.Response.Write(DataTableToJson2(view.ToTable(), ds.Tables[0].ExtendedProperties["Count"]));
                    }
                }
            }
            else if (hContext.Request.Form["Type"] == "Notify")
            {
                FlowDataParameter param = new FlowDataParameter();
                if (hContext.Request.Form["listID"] != null)
                {
                    param.Description = string.Format("LISTID='{0}'", hContext.Request.Form["listID"]);
                }
                param.StartIndex = (pageIndex - 1) * pageSize;
                param.Count = pageSize;
                if (!String.IsNullOrEmpty(hContext.Request.Form["sort"]))
                {
                    if (hContext.Request.Form["sort"] == "UPDATE_WHOLE_TIME")
                        param.OrderBy = String.Format("UPDATE_DATE {0}, UPDATE_TIME {0}", hContext.Request.Form["order"]);
                    else
                        param.OrderBy = String.Format("{0} {1}", hContext.Request.Form["sort"], hContext.Request.Form["order"]);
                }
                var ds1 = EFClientTools.ClientUtility.Client.GetFlowDataDS(EFClientTools.ClientUtility.ClientInfo, FlowDataType.Notify, param);
                DataSet ds = Deserialize<DataSet>(ds1);
                //DataSet ds = EFClientTools.ClientUtility.ExecuteSQL(EFClientTools.ClientUtility.ClientInfo.Database, GetFlowSql(user, ESqlMode.Notify));
                DataView view = new DataView(ds.Tables[0]);
                //view.Sort = "UPDATE_DATE DESC, UPDATE_TIME DESC";
                //if (hContext.Request.Form["listID"] != null)
                //{
                //    view.RowFilter = string.Format("LISTID='{0}'", hContext.Request.Form["listID"]);
                //}

                if (view.Count > 0)
                {
                    messageKey = "FLDesigner/FLDesigner/Item3";
                    string[] Item3 = provider[messageKey].Split(',');
                    ds.Tables[0].Columns.Add(new DataColumn("UPDATE_WHOLE_TIME"));
                    foreach (DataRowView item in view)
                    {
                        item["UPDATE_WHOLE_TIME"] = item["UPDATE_DATE"] + " " + item["UPDATE_TIME"];
                        foreach (var item1 in Item3)
                        {
                            if (item["STATUS"] != null && item1.StartsWith(item["STATUS"].ToString() + ":"))
                            {
                                item["STATUS"] = item1.Replace(item["STATUS"].ToString() + ":", string.Empty);
                                break;
                            }
                        }
                    }

                }
                else
                {

                }
                context.Response.Write(DataTableToJson2(view.ToTable(), ds.Tables[0].ExtendedProperties["Count"]));
            }
            else if (hContext.Request.Form["Type"] == "Delay")
            {
                String delayDatas = String.Empty;
                if (String.IsNullOrEmpty(hContext.Request.Form["Level"]))
                {
                    FlowDataParameter param = new FlowDataParameter();
                    if (hContext.Request.Form["listID"] != null)
                    {
                        param.Description = string.Format("LISTID='{0}'", hContext.Request.Form["listID"]);
                    }
                    param.StartIndex = (pageIndex - 1) * pageSize;
                    param.Count = pageSize;
                    if (!String.IsNullOrEmpty(hContext.Request.Form["sort"]))
                    {
                        if (hContext.Request.Form["sort"] == "UPDATE_WHOLE_TIME")
                            param.OrderBy = String.Format("UPDATE_DATE {0}, UPDATE_TIME {0}", hContext.Request.Form["order"]);
                        else
                            param.OrderBy = String.Format("{0} {1}", hContext.Request.Form["sort"], hContext.Request.Form["order"]);
                    }
                    var ds1 = EFClientTools.ClientUtility.Client.GetFlowDataDS(EFClientTools.ClientUtility.ClientInfo, FlowDataType.Do, param);
                    DataSet ds = Deserialize<DataSet>(ds1);
                    //ESqlMode mode = ESqlMode.ToDoList;
                    //DataSet ds = EFClientTools.ClientUtility.ExecuteSQL(EFClientTools.ClientUtility.ClientInfo.Database, GetFlowSql(user, mode));
                    DataTable dt = new DataTable();
                    if (ds.Tables.Count > 0)
                    {
                        messageKey = "FLDesigner/FLDesigner/Item3";
                        string[] Item3 = provider[messageKey].Split(',');
                        foreach (DataRow item in ds.Tables[0].Rows)
                        {
                            foreach (var item1 in Item3)
                            {
                                if (item["STATUS"] != null && item1.StartsWith(item["STATUS"].ToString() + ":"))
                                {
                                    item["STATUS"] = item1.Replace(item["STATUS"].ToString() + ":", string.Empty);
                                    break;
                                }
                            }
                        }

                        foreach (DataColumn column in ds.Tables[0].Columns)
                        {
                            DataColumn newColumn = new DataColumn(column.ColumnName, column.DataType);
                            dt.Columns.Add(newColumn);
                        }
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            DataRow newRow = dt.NewRow();
                            foreach (DataColumn column in dt.Columns)
                            {
                                newRow[column.ColumnName] = row[column.ColumnName];
                            }
                            string TIME_UNIT = row["TIME_UNIT"].ToString();
                            string FLOWURGENT = row["FLOWURGENT"].ToString();
                            string UPDATE_WHOLE_TIME = row["UPDATE_DATE"].ToString() + " " + row["UPDATE_TIME"].ToString();
                            string UPDATE_DATE = row["UPDATE_DATE"].ToString();
                            string UPDATE_TIME = row["UPDATE_TIME"].ToString();
                            //string UPDATE_DATE = UPDATE_WHOLE_TIME.Substring(0, UPDATE_WHOLE_TIME.IndexOf(' '));
                            //string UPDATE_TIME = UPDATE_WHOLE_TIME.Substring(UPDATE_WHOLE_TIME.IndexOf(' ') + 1);
                            string URGENT_TIME = row["URGENT_TIME"].ToString();
                            string EXP_TIME = row["EXP_TIME"].ToString();
                            if (IsOverTime(TIME_UNIT, FLOWURGENT, UPDATE_DATE, UPDATE_TIME, URGENT_TIME, EXP_TIME))
                                dt.Rows.Add(newRow);
                        }
                        //delayDatas += DataTableToJson(dt);
                    }
                    ds1 = EFClientTools.ClientUtility.Client.GetFlowDataDS(EFClientTools.ClientUtility.ClientInfo, FlowDataType.History, null);
                    ds = Deserialize<DataSet>(ds1);
                    //mode = ESqlMode.ToDoHis;
                    //ds = EFClientTools.ClientUtility.ExecuteSQL(EFClientTools.ClientUtility.ClientInfo.Database, GetFlowSql(user, mode));
                    if (ds.Tables.Count > 0)
                    {
                        messageKey = "FLDesigner/FLDesigner/Item3";
                        string[] Item3 = provider[messageKey].Split(',');
                        foreach (DataRow item in ds.Tables[0].Rows)
                        {
                            foreach (var item1 in Item3)
                            {
                                if (item["STATUS"] != null && item1.StartsWith(item["STATUS"].ToString() + ":"))
                                {
                                    item["STATUS"] = item1.Replace(item["STATUS"].ToString() + ":", string.Empty);
                                    break;
                                }
                            }
                        }

                        //foreach (DataColumn column in ds.Tables[0].Columns)
                        //{
                        //    DataColumn newColumn = new DataColumn(column.ColumnName, column.DataType);
                        //    dt.Columns.Add(newColumn);
                        //}
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            DataRow newRow = dt.NewRow();
                            foreach (DataColumn column in dt.Columns)
                            {
                                newRow[column.ColumnName] = row[column.ColumnName];
                            }
                            string TIME_UNIT = row["TIME_UNIT"].ToString();
                            string FLOWURGENT = row["FLOWURGENT"].ToString();
                            string UPDATE_WHOLE_TIME = row["UPDATE_DATE"].ToString() + " " + row["UPDATE_TIME"].ToString();
                            string UPDATE_DATE = row["UPDATE_DATE"].ToString();
                            string UPDATE_TIME = row["UPDATE_TIME"].ToString();
                            //string UPDATE_WHOLE_TIME = row["UPDATE_WHOLE_TIME"].ToString();
                            //string UPDATE_DATE = UPDATE_WHOLE_TIME.Substring(0, UPDATE_WHOLE_TIME.IndexOf(' '));
                            //string UPDATE_TIME = UPDATE_WHOLE_TIME.Substring(UPDATE_WHOLE_TIME.IndexOf(' ') + 1);
                            string URGENT_TIME = row["URGENT_TIME"].ToString();
                            string EXP_TIME = row["EXP_TIME"].ToString();
                            if (IsOverTime(TIME_UNIT, FLOWURGENT, UPDATE_DATE, UPDATE_TIME, URGENT_TIME, EXP_TIME))
                                dt.Rows.Add(newRow);
                        }
                        delayDatas = DataTableToJson2(dt, dt.Rows.Count);
                    }
                }
                else
                {
                    int delayLevel = int.Parse(hContext.Request.Form["Level"]);
                    bool alldata = false;
                    if (!string.IsNullOrEmpty(hContext.Request.Form["SpecialUse"]))
                    {
                        delayLevel = 2147483646;
                        alldata = true;
                    }
                    FlowDataParameter param = new FlowDataParameter();
                    if (hContext.Request.Form["listID"] != null)
                    {
                        param.ListID = new Guid(hContext.Request.Form["listID"]);
                    }
                    //其他几个都是把filter存在description里面，但是delay要存level，所以多开了一个字段放特殊功用
                    if (!String.IsNullOrEmpty(filter))
                    {
                        param.SpecialUse = filter;
                    }
                    param.StartIndex = (pageIndex - 1) * pageSize;
                    param.Count = pageSize;
                    if (!String.IsNullOrEmpty(hContext.Request.Form["sort"]))
                    {
                        if (hContext.Request.Form["sort"] == "UPDATE_WHOLE_TIME")
                            param.OrderBy = String.Format("UPDATE_DATE {0}, UPDATE_TIME {0}", hContext.Request.Form["order"]);
                        else
                            param.OrderBy = String.Format("{0} {1}", hContext.Request.Form["sort"], hContext.Request.Form["order"]);
                    }
                    DataTable tab = FLOvertimeList(user, delayLevel, alldata, hContext, param);
                    if (tab.Rows.Count > 0)
                    {
                        messageKey = "FLDesigner/FLDesigner/Item3";
                        string[] Item3 = provider[messageKey].Split(',');
                        foreach (DataRow item in tab.Rows)
                        {
                            foreach (var item1 in Item3)
                            {
                                if (item["STATUS"] != null && item1.StartsWith(item["STATUS"].ToString() + ":"))
                                {
                                    item["STATUS"] = item1.Replace(item["STATUS"].ToString() + ":", string.Empty);
                                    break;
                                }
                            }
                        }
                    }
                    delayDatas = DataTableToJson2(tab, tab.Rows.Count);
                }
                context.Response.Write(delayDatas);
                ////DataSet ds = EFClientTools.ClientUtility.ExecuteSQL(EFClientTools.ClientUtility.ClientInfo.Database, GetFlowSql(user, ESqlMode.Delay));
                ////if (ds.Tables.Count > 0)
                ////    context.Response.Write(DataTableToJson(ds.Tables[0]));
            }
            else if (hContext.Request.Form["Type"] == "DelayAllData")
            {
                String delayDatas = String.Empty;

                int delayLevel = int.Parse(hContext.Request.Form["Level"]);
                FlowDataParameter param = new FlowDataParameter();
                if (hContext.Request.Form["listID"] != null)
                {
                    param.Description = string.Format("LISTID='{0}'", hContext.Request.Form["listID"]);
                }
                //其他几个都是把filter存在description里面，但是delay要存level，所以多开了一个字段放特殊功用
                if (!String.IsNullOrEmpty(filter))
                {
                    param.SpecialUse = filter;
                }

                param.StartIndex = (pageIndex - 1) * pageSize;
                param.Count = pageSize;
                if (!String.IsNullOrEmpty(hContext.Request.Form["sort"]))
                {
                    if (hContext.Request.Form["sort"] == "UPDATE_WHOLE_TIME")
                        param.OrderBy = String.Format("UPDATE_DATE {0}, UPDATE_TIME {0}", hContext.Request.Form["order"]);
                    else
                        param.OrderBy = String.Format("{0} {1}", hContext.Request.Form["sort"], hContext.Request.Form["order"]);
                }
                DataTable tab = FLOvertimeList(user, delayLevel, true, hContext, param);
                if (tab.Rows.Count > 0)
                {
                    messageKey = "FLDesigner/FLDesigner/Item3";
                    string[] Item3 = provider[messageKey].Split(',');
                    foreach (DataRow item in tab.Rows)
                    {
                        foreach (var item1 in Item3)
                        {
                            if (item["STATUS"] != null && item1.StartsWith(item["STATUS"].ToString() + ":"))
                            {
                                item["STATUS"] = item1.Replace(item["STATUS"].ToString() + ":", string.Empty);
                                break;
                            }
                        }
                    }
                }
                delayDatas = DataTableToJson(tab, true);

                context.Response.Write(delayDatas);
                ////DataSet ds = EFClientTools.ClientUtility.ExecuteSQL(EFClientTools.ClientUtility.ClientInfo.Database, GetFlowSql(user, ESqlMode.Delay));
                ////if (ds.Tables.Count > 0)
                ////    context.Response.Write(DataTableToJson(ds.Tables[0]));
            }
            else if (hContext.Request.Form["Type"] == "Count")
            {
                Newtonsoft.Json.Linq.JObject obj = new Newtonsoft.Json.Linq.JObject();
                var types = new FlowDataType[] { FlowDataType.Do, FlowDataType.History, FlowDataType.Notify };
                foreach (var type in types)
                {
                    var xml = EFClientTools.ClientUtility.Client.GetFlowDataDS(EFClientTools.ClientUtility.ClientInfo, type, null);
                    var dataSet = Deserialize<DataSet>(xml);
                    obj[type.ToString()] = dataSet.Tables[0].Rows.Count;

                }
                obj["Delay"] = FLOvertimeList(user, 0, false, hContext, null).Rows.Count;
                context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(obj));
            }
            else if (hContext.Request.Form["Type"] == "ddlRoles")
            {
                String curTime = FLTools.GloFix.DateTimeString(DateTime.Now);
                String curUser = EFClientTools.ClientUtility.ClientInfo.UserID;
                String flowDesc = String.Empty;

                //String sql = "SELECT FLOW_DESC FROM SYS_TODOLIST WHERE LISTID='" + hContext.Request.Form["LISTID"] + "'";
                //DataSet ds = EFClientTools.ClientUtility.ExecuteSQL(EFClientTools.ClientUtility.ClientInfo.Database, sql);
                //if (ds.Tables[0].Rows.Count > 0)
                //    flowDesc = ds.Tables[0].Rows[0][0].ToString();
                //sql = "SELECT GROUPID,GROUPNAME FROM GROUPS WHERE GROUPID IN (SELECT GROUPID FROM USERGROUPS WHERE USERID='" + curUser + "')  AND ISROLE='Y' UNION SELECT ROLE_ID AS GROUPID,GROUPS.GROUPNAME  FROM SYS_ROLES_AGENT LEFT JOIN GROUPS ON SYS_ROLES_AGENT.ROLE_ID=GROUPS.GROUPID WHERE (SYS_ROLES_AGENT.FLOW_DESC='' OR SYS_ROLES_AGENT.FLOW_DESC='" + flowDesc + "') AND AGENT='" + curUser + "' AND START_DATE+START_TIME<='" + curTime + "' AND END_DATE+END_TIME>='" + curTime + "'";
                //ds = EFClientTools.ClientUtility.ExecuteSQL(EFClientTools.ClientUtility.ClientInfo.Database, sql);
                FlowDataParameter fdp = new FlowDataParameter();
                if (!String.IsNullOrEmpty(hContext.Request.Form["LISTID"]))
                    fdp.ListID = new Guid(hContext.Request.Form["LISTID"]);
                var ds1 = EFClientTools.ClientUtility.Client.GetFlowDataDS(EFClientTools.ClientUtility.ClientInfo, FlowDataType.Group, fdp);
                DataSet ds = Deserialize<DataSet>(ds1);
                if (ds.Tables.Count > 0)
                    context.Response.Write(DataTableToJson(ds.Tables[0], true));
            }
            else if (hContext.Request.Form["Type"] == "ddlReturnStep")
            {
                messageKey = "FLClientControls/SubmitConfirm/UIText";
                string[] UITexts = provider[messageKey].Split(',');
                FlowParameter fParams = new FlowParameter();
                String flowpath = hContext.Request.Form["FLOWPATH"];//FLOWPATH
                String[] fLActivities = null;
                if (!String.IsNullOrEmpty(flowpath))
                    fLActivities = flowpath.ToString().Split(';');
                if (fLActivities != null && fLActivities.Length > 1)
                {
                    fParams.CurrentActivity = fLActivities[0];
                    fParams.NextActivity = fLActivities[1];
                }

                fParams.InstanceID = new Guid(hContext.Request.Form["LISTID"]);
                fParams.Operation = FlowOperation.GetFLPathList;
                FlowResult result = EFClientTools.ClientUtility.Client.CallFlowMethod(EFClientTools.ClientUtility.ClientInfo, fParams);
                DataTable dtReturnStep = new DataTable();
                dtReturnStep.Columns.Add("RERURNSTEPID");
                dtReturnStep.Columns.Add("RERURNSTEPNAME");
                DataRow dr0 = dtReturnStep.NewRow();
                dr0["RERURNSTEPID"] = 0;
                dr0["RERURNSTEPNAME"] = UITexts[11];
                dtReturnStep.Rows.Add(dr0);
                if (result.FLPathList != null)
                {
                    for (int i = 0; i < result.FLPathList.Count; i++)
                    {
                        DataRow dr = dtReturnStep.NewRow();
                        dr["RERURNSTEPID"] = i + 1;
                        dr["RERURNSTEPNAME"] = result.FLPathList[i];
                        dtReturnStep.Rows.Add(dr);
                    }
                }
                context.Response.Write(DataTableToJson(dtReturnStep, true));
            }
            else if (hContext.Request.Form["Type"] == "gdvHis")
            {
                FlowDataParameter fdp = new FlowDataParameter();
                if (!String.IsNullOrEmpty(hContext.Request.Form["LISTID"]))
                {
                    fdp.ListID = new Guid(hContext.Request.Form["LISTID"]);
                    var ds1 = EFClientTools.ClientUtility.Client.GetFlowDataDS(EFClientTools.ClientUtility.ClientInfo, FlowDataType.History, fdp);
                    DataSet ds = Deserialize<DataSet>(ds1);
                    //String sql = "SELECT S_STEP_ID,USER_ID,USERNAME,STATUS,UPDATE_DATE,UPDATE_TIME,REMARK FROM SYS_TODOHIS Where (LISTID = '" + hContext.Request.Form["LISTID"] + "') ORDER BY UPDATE_DATE,UPDATE_TIME";
                    //String sql = "SELECT SYS_TODOHIS.FLOW_DESC,SYS_TODOHIS.S_ROLE_ID,SYS_TODOHIS.S_STEP_ID,SYS_TODOHIS.USER_ID,SYS_TODOHIS.USERNAME,SYS_TODOHIS.STATUS,SYS_TODOHIS.UPDATE_DATE,SYS_TODOHIS.UPDATE_TIME,SYS_TODOHIS.REMARK,SYS_TODOHIS.ATTACHMENTS,SYS_TODOHIS.FORM_PRESENT_CT,GROUPS.GROUPNAME FROM SYS_TODOHIS LEFT JOIN GROUPS ON SYS_TODOHIS.S_ROLE_ID = GROUPS.GROUPID WHERE (SYS_TODOHIS.LISTID = '" + hContext.Request.Form["LISTID"] + "') ORDER BY SYS_TODOHIS.UPDATE_DATE,SYS_TODOHIS.UPDATE_TIME";
                    //DataSet ds = EFClientTools.ClientUtility.ExecuteSQL(EFClientTools.ClientUtility.ClientInfo.Database, sql);
                    if (ds.Tables.Count > 0)
                    {
                        messageKey = "FLDesigner/FLDesigner/Item3";
                        string[] Item3 = provider[messageKey].Split(',');
                        ds.Tables[0].Columns.Add(new DataColumn("UPDATE_WHOLE_TIME"));
                        foreach (DataRow item in ds.Tables[0].Rows)
                        {
                            item["UPDATE_WHOLE_TIME"] = item["UPDATE_DATE"] + " " + item["UPDATE_TIME"];
                            foreach (var item1 in Item3)
                            {
                                if (item["STATUS"] != null && item1.StartsWith(item["STATUS"].ToString() + ":"))
                                {
                                    item["STATUS"] = item1.Replace(item["STATUS"].ToString() + ":", string.Empty);
                                    break;
                                }
                            }
                        }
                        DataView dv = ds.Tables[0].DefaultView;
                        dv.Sort = "UPDATE_WHOLE_TIME ASC";
                        DataTable dtCopy = dv.ToTable();
                        //ds.Tables[0].Select("1=1", "UPDATE_WHOLE_TIME DESC");
                        context.Response.Write(DataTableToJson(dtCopy, true));
                    }
                }
                else
                {
                    context.Response.Write("");
                }
            }
            else if (hContext.Request.Form["Type"] == "lstUsersFrom")
            {
                //String userid = hContext.Request.Form["USERID"];
                //String username = hContext.Request.Form["USERNAME"];
                //String where = " WHERE ";
                //if (!String.IsNullOrEmpty(userid))
                //    where += "USERID like'%" + userid + "%' AND ";
                //if (!String.IsNullOrEmpty(username))
                //    where += "USERNAME like'%" + username + "%' AND ";
                //if (where == " WHERE ")
                //    where = "";
                //else
                //    where = where.Remove(where.LastIndexOf("AND"));
                //String sql = "select USERID, USERNAME from USERS" + where;
                //if (hContext.Request.Form["SecUsers"] != null && hContext.Request.Form["SecUsers"] != "")
                //{
                //    string[] secUsers = hContext.Request.Form["SecUsers"].Split(';');
                //    if (secUsers.Length > 0)
                //    {
                //        sql += " where USERID in (";
                //        for (int i = 0; i < secUsers.Length; i++)
                //        {
                //            if (i == secUsers.Length - 1)
                //                sql += "'" + secUsers[i] + "'";
                //            else
                //                sql += "'" + secUsers[i] + "',";
                //        }
                //        sql += ")";
                //    }
                //}
                //DataSet ds = EFClientTools.ClientUtility.ExecuteSQL(EFClientTools.ClientUtility.ClientInfo.Database, sql);
                String userid = hContext.Server.UrlDecode(hContext.Request.Form["USERID"]);
                String username = hContext.Server.UrlDecode(hContext.Request.Form["USERNAME"]);
                String where = " AND ";
                if (!String.IsNullOrEmpty(userid))
                    where += "USERS.USERID like'%" + userid + "%' AND ";
                if (!String.IsNullOrEmpty(username))
                    where += "USERS.USERNAME like'%" + username + "%' AND ";
                where = where.Remove(where.LastIndexOf("AND"));
                FlowDataParameter fDataParams = new FlowDataParameter();
                fDataParams.Description = where;
                var ds1 = EFClientTools.ClientUtility.Client.GetFlowDataDS(EFClientTools.ClientUtility.ClientInfo, FlowDataType.AllUsers, fDataParams);
                DataSet ds = Deserialize<DataSet>(ds1);
                if (ds.Tables.Count > 0)
                    context.Response.Write(DataTableToJson(ds.Tables[0], true));
            }
            else if (hContext.Request.Form["Type"] == "lstRolesFrom")
            {
                //String groupid = hContext.Request.Form["GROUPID"];
                //String groupname = hContext.Request.Form["GROUPNAME"];
                //String where = " AND ";
                //if (!String.IsNullOrEmpty(groupid))
                //    where += "GROUPID like'%" + groupid + "%' AND ";
                //if (!String.IsNullOrEmpty(groupname))
                //    where += "GROUPNAME like'%" + groupname + "%' AND ";
                //where = where.Remove(where.LastIndexOf("AND"));
                //String sql = "select GROUPID, GROUPNAME, ISROLE from GROUPS where ISROLE='Y'" + where;
                //if (hContext.Request.Form["SecGroups"] != null && hContext.Request.Form["SecGroups"] != "")
                //{
                //    string[] sqlRoles = hContext.Request.Form["SecGroups"].Split(';');
                //    if (sqlRoles.Length > 0)
                //    {
                //        sql += " where GROUPID in (";
                //        for (int i = 0; i < sqlRoles.Length; i++)
                //        {
                //            if (i == sqlRoles.Length - 1)
                //                sql += "'" + sqlRoles[i] + "'";
                //            else
                //                sql += "'" + sqlRoles[i] + "',";
                //        }
                //        sql += ")";
                //    }
                //}
                //DataSet ds = EFClientTools.ClientUtility.ExecuteSQL(EFClientTools.ClientUtility.ClientInfo.Database, sql);
                //FlowDataParameter parameter = new FlowDataParameter();
                //parameter.GroupID = hContext.Request.Form["GROUPID"];
                //parameter.GroupName = hContext.Request.Form["GROUPNAME"];
                String groupid = hContext.Server.UrlDecode(hContext.Request.Form["GROUPID"]);
                String groupname = hContext.Server.UrlDecode(hContext.Request.Form["GROUPNAME"]);
                String where = " AND ";
                if (!String.IsNullOrEmpty(groupid))
                    where += "GROUPID like'%" + groupid + "%' AND ";
                if (!String.IsNullOrEmpty(groupname))
                    where += "GROUPNAME like'%" + groupname + "%' AND ";
                where = where.Remove(where.LastIndexOf("AND"));
                FlowDataParameter fDataParams = new FlowDataParameter();
                fDataParams.Description = where;
                var ds1 = EFClientTools.ClientUtility.Client.GetFlowDataDS(EFClientTools.ClientUtility.ClientInfo, FlowDataType.AllGroups, fDataParams);
                DataSet ds = Deserialize<DataSet>(ds1);
                if (ds.Tables.Count > 0)
                {
                    context.Response.Write(DataTableToJson(ds.Tables[0], true));
                }
            }
            else if (hContext.Request.Form["Type"] == "Workflow")
            {
                FlowResult result = null;
                //CliUtils.fFlowSelectedRole = this.ddlRoles.SelectedValue;
                FlowParameter fParams = new FlowParameter();
                String flowpath = hContext.Request.Form["FLOWPATH"];//FLOWPATH
                String[] fLActivities = null;
                if (!String.IsNullOrEmpty(flowpath))
                    fLActivities = flowpath.ToString().Split(';');
                if (fLActivities != null && fLActivities.Length > 1)
                {
                    fParams.CurrentActivity = fLActivities[0];
                    fParams.NextActivity = fLActivities[1];
                }
                fParams.Important = false;
                if (hContext.Request.Form["important"] == "checked")
                {
                    fParams.Important = true;
                }
                fParams.Urgent = false;
                if (hContext.Request.Form["urgent"] == "checked")
                {
                    fParams.Urgent = true;
                }
                fParams.Remark = hContext.Request.Form["suggest"];
                fParams.RoleID = hContext.Request.Form["roles"];
                fParams.Provider = hContext.Request.Form["PROVIDER_NAME"];//PROVIDER_NAME
                fParams.MailAddress = GetMailAddress();
                //FolderName={0}&FormName={1}&LISTID={2}&FLOWPATH={3}&WHERESTRING={4}&NAVMODE={5}&FLNAVMODE={6}&Users={7}&PLUSAPPROVE={8}&STATUS={9}&SENDTOID={10}&MULTISTEPRETURN={11}&ATTACHMENTS={12}
                fParams.Attachments = hContext.Request.Form["ATTACHMENTS"];//ATTACHMENTS
                fParams.Keys = hContext.Request.Form["FORM_KEYS"];//FORM_KEYS
                fParams.KeyValues = hContext.Request.Form["FORM_PRESENTATION"];//FORM_PRESENTATION
                if (fParams.KeyValues != null && fParams.KeyValues.Contains("'"))
                    fParams.KeyValues = fParams.KeyValues.Replace("'", "''");
                if (!String.IsNullOrEmpty(hContext.Request.Form["LISTID"]) && !hContext.Request.Form["LISTID"].Contains("!"))
                    fParams.InstanceID = new Guid(hContext.Request.Form["LISTID"]);
                String status = hContext.Request.Form["STATUS"];//STATUS
                fParams.SendToIDs = hContext.Request.Form["SENDTO_ID"];//STATUS

                //String Param = hContext.Request.Form["UserParam"];
                //System.Text.StringBuilder userParameter = new System.Text.StringBuilder();
                //if (!String.IsNullOrEmpty(Param))
                //{
                //    String[] listParam = Param.Split(';');
                //    foreach (String par in listParam)
                //    {
                //        if (!String.IsNullOrEmpty(par))
                //        {
                //            String[] keyvalues = par.Split('^');
                //            if (keyvalues.Length == 2)
                //            {
                //                userParameter.Append("&");
                //                userParameter.Append(keyvalues[0]);
                //                userParameter.Append("=");
                //                userParameter.Append(keyvalues[1]);
                //            }
                //        }
                //    }
                //}
                if (hContext.Request.Form["Active"] == "Submit")
                {
                    //string org = "";
                    //if (this.ViewState["ORGCONTROL"] != null && this.ViewState["ORGCONTROL"].ToString() == "True")
                    //{
                    //    org = this.ddlOrg.SelectedValue;
                    //}
                    fParams.Operation = FlowOperation.Submit;
                    var path = EFClientTools.ClientUtility.Client.CallMethod(EFClientTools.ClientUtility.ClientInfo, "GLModule", "GetServerPath", new List<object>());
                    fParams.XomlName = System.IO.Path.Combine(path.ToString(), "Workflow", hContext.Server.UrlDecode(hContext.Request.Form["FLOWFILENAME"]) + ".xoml");//STATUS
                    result = EFClientTools.ClientUtility.Client.CallFlowMethod(EFClientTools.ClientUtility.ClientInfo, fParams);
                    if (result != null)
                    {
                        if (result.InstanceID.ToString() == "512F4277-0D41-441c-BF16-D96B04580C2E")
                        {
                            messageKey = "FLClientControls/FLNavigator/HasRejected";
                            WriteMessage(context.Response, provider[messageKey], "");
                        }
                        else if (result.Status == FlowStatus.End)
                        {
                            messageKey = "FLClientControls/FLNavigator/RunOver";
                            WriteMessage(context.Response, provider[messageKey], "");
                        }
                        else if (result.Status == FlowStatus.Exception)
                        {
                            WriteMessage(context.Response, result.Message, "");
                        }
                        else
                        {
                            if (!String.IsNullOrEmpty(result.Message))
                            {
                                WriteMessage(context.Response, result.Message, "");
                            }
                            else
                            {
                                List<object> param = new List<object>();
                                param.Add(result.SendToIDs);
                                param.Add(true);
                                EFClientTools.ClientUtility.ClientInfo.UseDataSet = false;
                                var message = EFClientTools.ClientUtility.Client.CallMethod(EFClientTools.ClientUtility.ClientInfo, "EFGlobalModule", "GloFix_ShowMessage", param);
                                EFClientTools.ClientUtility.ClientInfo.UseDataSet = true;
                                messageKey = "FLClientControls/FLWizard/Send";
                                WriteMessage(context.Response, provider[messageKey] + message, "");
                            }
                        }
                    }
                }
                else if (hContext.Request.Form["Active"] == "Approve")
                {
                    DoApprove(status, provider, fParams);
                }
                else if (hContext.Request.Form["Active"] == "ApproveAll")
                {
                    String[] LISTIDs = hContext.Request.Form["LISTID"].Split('!');//FLOWPATH
                    String[] FLOWPATHs = hContext.Request.Form["FLOWPATH"].Split('!');//FLOWPATH
                    String[] PROVIDER_NAMEs = hContext.Request.Form["PROVIDER_NAME"].Split('!');//FLOWPATH
                    String[] FORM_KEYSs = hContext.Request.Form["FORM_KEYS"].Split('!');//FLOWPATH
                    String[] FORM_PRESENTATIONs = hContext.Request.Form["FORM_PRESENTATION"].Split('!');//FLOWPATH
                    String[] STATUSs = hContext.Request.Form["STATUS"].Split('!');//FLOWPATH
                    String[] SENDTO_IDs = hContext.Request.Form["SENDTO_ID"].Split('!');//FLOWPATH
                    String[] ROLEs = hContext.Request.Form["ROLE"].Split('!');//FLOWPATH
                    String[] ATTACHMENTs = hContext.Request.Form["ATTACHMENT"].Split('!');//FLOWPATH
                    for (int i = 0; i < LISTIDs.Length; i++)
                    {
                        if (!String.IsNullOrEmpty(LISTIDs[i]))
                        {
                            fLActivities = null;
                            if (!String.IsNullOrEmpty(flowpath))
                                fLActivities = FLOWPATHs[i].Split(';');
                            fParams.CurrentActivity = fLActivities[0];
                            fParams.NextActivity = fLActivities[1];
                            fParams.Remark = hContext.Request.Form["suggest"];
                            //fParams.RoleID = hContext.Request.Form["roles"];
                            fParams.RoleID = ROLEs[i];
                            fParams.Provider = PROVIDER_NAMEs[i];//PROVIDER_NAME
                            //mailAddress = GetMailAddress();
                            fParams.Attachments = ATTACHMENTs[i];//ATTACHMENTS
                            fParams.Keys = FORM_KEYSs[i];//FORM_KEYS
                            fParams.KeyValues = FORM_PRESENTATIONs[i];//FORM_PRESENTATION
                            if (fParams.KeyValues.Contains("'"))
                                fParams.KeyValues = fParams.KeyValues.Replace("'", "''");
                            fParams.InstanceID = new Guid(LISTIDs[i]);
                            status = STATUSs[i];//STATUS
                            fParams.SendToIDs = SENDTO_IDs[i];//STATUS

                            DoApprove(status, provider, fParams);
                        }
                    }
                }
                else if (hContext.Request.Form["Active"] == "Return")
                {
                    DoReturn(status, provider, fParams);
                }
                else if (hContext.Request.Form["Active"] == "Retake")
                {
                    var ds1 = EFClientTools.ClientUtility.Client.GetFlowDataDS(EFClientTools.ClientUtility.ClientInfo, FlowDataType.History, null);
                    DataSet ds = Deserialize<DataSet>(ds1);
                    fParams.NextActivity = hContext.Request.Form["D_STEP_ID"];
                    //string strSql = "SELECT * FROM SYS_TODOHIS WHERE LISTID='" + fParams.InstanceID + "' AND D_STEP_ID='" + fParams.NextActivity + "' ORDER BY UPDATE_TIME DESC";
                    //DataSet ds = EFClientTools.ClientUtility.ExecuteSQL(EFClientTools.ClientUtility.ClientInfo.Database, strSql);
                    if (ds.Tables.Count > 0)
                    {
                        DataRow[] dr = ds.Tables[0].Select("LISTID = '" + fParams.InstanceID + "' AND D_STEP_ID='" + fParams.NextActivity + "'");
                        DataRow row1 = dr[0];
                        fParams.CurrentActivity = row1["S_STEP_ID"].ToString();
                        fParams.Keys = row1["FORM_KEYS"].ToString();
                        fParams.KeyValues = row1["FORM_PRESENTATION"].ToString();
                        fParams.KeyValues = fParams.KeyValues.Replace("'", "''");
                        if (row1["FLOWIMPORTANT"] == null || row1["FLOWIMPORTANT"].ToString() == "0")
                            fParams.Important = false;
                        else
                            fParams.Important = true;
                        if (row1["FLOWURGENT"] == null || row1["FLOWURGENT"].ToString() == "0")
                            fParams.Urgent = false;
                        else
                            fParams.Urgent = true;

                        fParams.Operation = FlowOperation.Retake;
                        result = EFClientTools.ClientUtility.Client.CallFlowMethod(EFClientTools.ClientUtility.ClientInfo, fParams);

                        if (result != null)
                        {
                            if (result.InstanceID.ToString() == "B4DAF3A4-AAE8-4b51-A391-B52E46305E9F")
                            {
                                messageKey = "FLClientControls/FLWebNavigator/ReturnToEnd";
                                WriteMessage(context.Response, provider[messageKey], "");
                            }
                            else if (result.InstanceID.ToString() == "C912D847-1825-458a-8CB5-E680FACA42AF")
                            {
                                List<object> param = new List<object>();
                                param.Add(result.SendToIDs);
                                param.Add(true);
                                EFClientTools.ClientUtility.ClientInfo.UseDataSet = false;
                                var message = EFClientTools.ClientUtility.Client.CallMethod(EFClientTools.ClientUtility.ClientInfo, "EFGlobalModule", "GloFix_ShowMessage3", param);
                                EFClientTools.ClientUtility.ClientInfo.UseDataSet = true;
                                messageKey = "FLClientControls/FLWebNavigator/ReturnToEnd";
                                WriteMessage(context.Response, message.ToString(), "");
                            }
                            else
                            {
                                if (result.Status == FlowStatus.Exception)
                                {
                                    WriteMessage(context.Response, result.Message, "");
                                }
                                else
                                {
                                    messageKey = "FLClientControls/FLWizard/RetakeSucess";
                                    WriteMessage(context.Response, provider[messageKey], "");
                                }
                            }
                        }
                        else
                        {
                            //if (Convert.ToInt16(objParams[0]) == 2)
                            //    this.result.Text = objParams[1].ToString();
                        }
                    }
                }
                else if (hContext.Request.Form["Active"] == "ReturnAll")
                {
                    String[] LISTIDs = hContext.Request.Form["LISTID"].Split('!');//FLOWPATH
                    String[] FLOWPATHs = hContext.Request.Form["FLOWPATH"].Split('!');//FLOWPATH
                    String[] PROVIDER_NAMEs = hContext.Request.Form["PROVIDER_NAME"].Split('!');//FLOWPATH
                    String[] FORM_KEYSs = hContext.Request.Form["FORM_KEYS"].Split('!');//FLOWPATH
                    String[] FORM_PRESENTATIONs = hContext.Request.Form["FORM_PRESENTATION"].Split('!');//FLOWPATH
                    String[] STATUSs = hContext.Request.Form["STATUS"].Split('!');//FLOWPATH
                    String[] SENDTO_IDs = hContext.Request.Form["SENDTO_ID"].Split('!');//FLOWPATH
                    for (int i = 0; i < LISTIDs.Length; i++)
                    {
                        if (!String.IsNullOrEmpty(LISTIDs[i]))
                        {
                            fLActivities = null;
                            if (!String.IsNullOrEmpty(flowpath))
                                fLActivities = FLOWPATHs[i].Split(';');
                            fParams.CurrentActivity = fLActivities[0];
                            fParams.NextActivity = fLActivities[1];
                            fParams.Remark = hContext.Request.Form["suggest"];
                            fParams.RoleID = hContext.Request.Form["roles"];
                            fParams.Provider = PROVIDER_NAMEs[i];//PROVIDER_NAME
                            //mailAddress = GetMailAddress();
                            fParams.Attachments = hContext.Request.Form["ATTACHMENTS"];//ATTACHMENTS
                            fParams.Keys = FORM_KEYSs[i];//FORM_KEYS
                            fParams.KeyValues = FORM_PRESENTATIONs[i];//FORM_PRESENTATION
                            if (fParams.KeyValues.Contains("'"))
                                fParams.KeyValues = fParams.KeyValues.Replace("'", "''");
                            fParams.InstanceID = new Guid(LISTIDs[i]);
                            status = STATUSs[i];//STATUS
                            fParams.SendToIDs = SENDTO_IDs[i];//STATUS

                            DoReturn(status, provider, fParams);
                        }
                    }
                }
                if (hContext.Request.Form["Active"] == "Plus")
                {
                    string users = hContext.Request.Form["USERS"];
                    string users1 = hContext.Request.Form["USERS1"];
                    string roles = hContext.Request.Form["ROLES"];
                    if (string.IsNullOrEmpty(users) && string.IsNullOrEmpty(roles))
                        return;
                    fParams.Operation = FlowOperation.PlusApprove;
                    fParams.SendToIDs = users + roles;
                    //fParams.RoleID = roles;
                    fParams.CurrentActivity = fParams.NextActivity;
                    result = EFClientTools.ClientUtility.Client.CallFlowMethod(EFClientTools.ClientUtility.ClientInfo, fParams);
                    if (result != null)
                    {
                        if (result.Status == FlowStatus.Exception)
                        {
                            WriteMessage(context.Response, result.Message, "");
                        }
                        else
                        {
                            string sendToIds = users1 + roles;
                            sendToIds = sendToIds.Substring(0, sendToIds.LastIndexOf(';'));
                            List<object> param = new List<object>();
                            param.Add(sendToIds);
                            param.Add(false);
                            EFClientTools.ClientUtility.ClientInfo.UseDataSet = false;
                            var message = EFClientTools.ClientUtility.Client.CallMethod(EFClientTools.ClientUtility.ClientInfo, "EFGlobalModule", "GloFix_ShowNotifyMessage", param);
                            EFClientTools.ClientUtility.ClientInfo.UseDataSet = true;
                           WriteMessage( context.Response, message.ToString(), "");

                            //List<object> param = new List<object>();
                            //param.Add(fParams.RoleID);
                            //param.Add(false);
                            //EFClientTools.ClientUtility.ClientInfo.UseDataSet = false;
                            //var message = EFClientTools.ClientUtility.Client.CallMethod(EFClientTools.ClientUtility.ClientInfo, "EFGlobalModule", "GloFix_ShowPlusMessage", param);
                            //EFClientTools.ClientUtility.ClientInfo.UseDataSet = true;
                            //context.Response.Write(message);
                        }
                    }
                    //if (Convert.ToInt16(objParams[0]) == 0)
                    //{
                    //    string sendToIds = roles.Substring(0, roles.LastIndexOf(';'));
                    //    this.result.Text = FLTools.GloFix.ShowPlusMessage(sendToIds);
                    //    string ref_script = "var lnk_element=window.opener.document.getElementById('lnkRefresh');if(lnk_element){window.opener.__doPostBack('lnkRefresh','');}else{lnk_element=window.opener.parent.document.getElementById('lnkRefresh');if(lnk_element){window.opener.parent.__doPostBack('lnkRefresh','');}}";
                    //    this.Page.ClientScript.RegisterStartupScript(this.GetType(), "", ref_script + "window.opener.location.reload('" + this.ViewState["PagePath"].ToString() + "?&NAVMODE=2&FLNAVMODE=8');", true);
                    //}
                    //else
                    //{
                    //    if (Convert.ToInt16(objParams[0]) == 2)
                    //        this.result.Text = SysMsg.GetSystemMessage(CliUtils.fClientLang, "FLTools", "GloFix", "FailToPlus", true);
                    //}
                }
                else if (hContext.Request.Form["Active"] == "Notify")
                {
                    string users = hContext.Request.Form["USERS"], roles = hContext.Request.Form["ROLES"];
                    if (string.IsNullOrEmpty(users) && string.IsNullOrEmpty(roles))
                        return;
                    fParams.Operation = FlowOperation.Notify;
                    fParams.SendToIDs = users + roles;
                    result = EFClientTools.ClientUtility.Client.CallFlowMethod(EFClientTools.ClientUtility.ClientInfo, fParams);

                    //object[] objParams = CliUtils.CallFLMethod("Notify", new object[] { new Guid(listId), new object[] { flowPath, flowPath, 0, 0, this.txtMessage.Text, "", provider, 0, users + roles, "" }, new object[] { keys, values } });
                    if (result.Status == FlowStatus.Exception)
                    {
                        WriteMessage(context.Response, result.Message, "");
                    }
                    else
                    {
                        string sendToIds = users + roles;
                        sendToIds = sendToIds.Substring(0, sendToIds.LastIndexOf(';'));
                        List<object> param = new List<object>();
                        param.Add(sendToIds);
                        param.Add(false);
                        EFClientTools.ClientUtility.ClientInfo.UseDataSet = false;
                        var message = EFClientTools.ClientUtility.Client.CallMethod(EFClientTools.ClientUtility.ClientInfo, "EFGlobalModule", "GloFix_ShowNotifyMessage", param);
                        EFClientTools.ClientUtility.ClientInfo.UseDataSet = true;
                        WriteMessage(context.Response, message.ToString(), "");
                    }
                }
                else if (hContext.Request.Form["Active"] == "Hasten")
                {
                    fParams.RoleID = fParams.SendToIDs;
                    fParams.Operation = FlowOperation.Notify;
                    result = EFClientTools.ClientUtility.Client.CallFlowMethod(EFClientTools.ClientUtility.ClientInfo, fParams);

                    //object[] objParams = CliUtils.CallFLMethod("Notify", new object[] { new Guid(listId), new object[] { flowPath, flowPath, 0, 0, this.txtMessage.Text, "", provider, 0, users + roles, "" }, new object[] { keys, values } });
                    if (result.Status == FlowStatus.Exception)
                    {
                        WriteMessage(context.Response, result.Message, "");
                    }
                    else
                    {
                        List<object> param = new List<object>();
                        param.Add(fParams.SendToIDs);
                        param.Add(false);
                        EFClientTools.ClientUtility.ClientInfo.UseDataSet = false;
                        var message = EFClientTools.ClientUtility.Client.CallMethod(EFClientTools.ClientUtility.ClientInfo, "EFGlobalModule", "GloFix_ShowNotifyMessage", param);
                        EFClientTools.ClientUtility.ClientInfo.UseDataSet = true;
                        WriteMessage(context.Response, message.ToString(), "");
                    }
                }
                else if (hContext.Request.Form["Active"] == "FlowDelete")
                {
                    fParams.Operation = FlowOperation.DeleteNotify;
                    result = EFClientTools.ClientUtility.Client.CallFlowMethod(EFClientTools.ClientUtility.ClientInfo, fParams);
                    if (result.Status == FlowStatus.Exception)
                    {
                        WriteMessage(context.Response, result.Message, "");
                    }
                    else
                    {
                        messageKey = "FLClientControls/FLNavigator/FlowReject";
                        WriteMessage(context.Response, provider[messageKey], "");
                    }
                }
                else if (hContext.Request.Form["Active"] == "FlowDeleteAll")
                {
                    String[] LISTIDs = hContext.Request.Form["LISTID"].Split('!');//FLOWPATH
                    String[] FLOWPATHs = hContext.Request.Form["FLOWPATH"].Split('!');//FLOWPATH
                    String[] PROVIDER_NAMEs = hContext.Request.Form["PROVIDER_NAME"].Split('!');//FLOWPATH
                    String[] FORM_KEYSs = hContext.Request.Form["FORM_KEYS"].Split('!');//FLOWPATH
                    String[] FORM_PRESENTATIONs = hContext.Request.Form["FORM_PRESENTATION"].Split('!');//FLOWPATH
                    String[] STATUSs = hContext.Request.Form["STATUS"].Split('!');//FLOWPATH
                    String[] SENDTO_IDs = hContext.Request.Form["SENDTO_ID"].Split('!');//FLOWPATH
                    for (int i = 0; i < LISTIDs.Length; i++)
                    {
                        if (!String.IsNullOrEmpty(LISTIDs[i]))
                        {
                            fLActivities = null;
                            if (!String.IsNullOrEmpty(FLOWPATHs[i]))
                                fLActivities = FLOWPATHs[i].Split(';');
                            fParams.CurrentActivity = fLActivities[0];
                            fParams.NextActivity = fLActivities[1];
                            fParams.Remark = hContext.Request.Form["suggest"];
                            fParams.RoleID = hContext.Request.Form["roles"];
                            fParams.Provider = PROVIDER_NAMEs[i];//PROVIDER_NAME
                            //mailAddress = GetMailAddress();
                            fParams.Attachments = hContext.Request.Form["ATTACHMENTS"];//ATTACHMENTS
                            fParams.Keys = FORM_KEYSs[i];//FORM_KEYS
                            fParams.KeyValues = FORM_PRESENTATIONs[i];//FORM_PRESENTATION
                            if (fParams.KeyValues.Contains("'"))
                                fParams.KeyValues = fParams.KeyValues.Replace("'", "''");
                            fParams.InstanceID = new Guid(LISTIDs[i]);
                            status = STATUSs[i];//STATUS
                            fParams.SendToIDs = SENDTO_IDs[i];//STATUS

                            fParams.Operation = FlowOperation.DeleteNotify;
                            result = EFClientTools.ClientUtility.Client.CallFlowMethod(EFClientTools.ClientUtility.ClientInfo, fParams);
                        }
                    }
                    messageKey = "FLClientControls/FLNavigator/FlowReject";
                    WriteMessage(context.Response, provider[messageKey], "");
                }
                else if (hContext.Request.Form["Active"] == "Reject")
                {
                    fParams.Operation = FlowOperation.Reject;
                    if (hContext.Request.Form["NotifyAllRoles"] == "true")
                    {
                        fParams.NotifyAllRoles = true;
                    }
                    result = EFClientTools.ClientUtility.Client.CallFlowMethod(EFClientTools.ClientUtility.ClientInfo, fParams);
                    if (result.Status == FlowStatus.Exception)
                    {
                        WriteMessage(context.Response, result.Message, "");
                    }
                    else
                    {
                        messageKey = "FLClientControls/FLNavigator/HasRejected";
                        WriteMessage(context.Response, provider[messageKey], "");
                    }
                }
                else if (hContext.Request.Form["Active"] == "Pause")
                {
                    fParams.Operation = FlowOperation.Pause;
                    var path = EFClientTools.ClientUtility.Client.CallMethod(EFClientTools.ClientUtility.ClientInfo, "GLModule", "GetServerPath", new List<object>());
                    fParams.XomlName = System.IO.Path.Combine(path.ToString(), "Workflow", hContext.Server.UrlDecode(hContext.Request.Form["FLOWFILENAME"]) + ".xoml");//STATUS
                    //fParams.XomlName = 
                    //fParams.XomlName = hContext.Server.UrlDecode(hContext.Request.Form["FLOWFILENAME"]) + ".xoml";//STATUS
                    result = EFClientTools.ClientUtility.Client.CallFlowMethod(EFClientTools.ClientUtility.ClientInfo, fParams);
                    if (result.Status == FlowStatus.Exception)
                    {
                        WriteMessage(context.Response, result.Message, "");
                    }
                    else
                    {
                        messageKey = "FLClientControls/FLNavigator/PauseSucceed";
                        //WriteMessage(context.Response, provider[messageKey], "");
                        WriteMessage(context.Response, result.InstanceID.ToString(), "");
                    }
                    //Hashtable keyValues = GetKeyValues(false);
                    //string sKeys = "", sValues = "", svs = "";
                    //IEnumerator enumer = keyValues.GetEnumerator();
                    //while (enumer.MoveNext())
                    //{
                    //    sKeys += ((DictionaryEntry)enumer.Current).Key.ToString() + ";";
                    //    sValues += ((DictionaryEntry)enumer.Current).Key.ToString() + "=" + ((DictionaryEntry)enumer.Current).Value.ToString() + ";";
                    //    svs += ((DictionaryEntry)enumer.Current).Value.ToString();
                    //}
                    //if (sKeys != "" && sValues != "")
                    //{
                    //    sKeys = sKeys.Substring(0, sKeys.LastIndexOf(';'));
                    //    sValues = sValues.Substring(0, sValues.LastIndexOf(';'));
                    //}

                    ////$edit20100604 by navy For support ajax,彈出訊息的方式支援ajax
                    ////ClientScriptManager csm = this.Page.ClientScript;
                    //string message = "";
                    //if ((string.IsNullOrEmpty(sKeys) && string.IsNullOrEmpty(sValues)) || this.CurrentNavState != "PreInsert")
                    //{
                    //    message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "FLTools", "GloFix", "SelectData", true);
                    //    //$edit20100604 by navy
                    //    //csm.RegisterStartupScript(this.GetType(), "seldata", "alert('" + message + "');", true);
                    //    ScriptHelper.RegisterStartupScript(this, "alert('" + message + "');");
                    //    return;
                    //}
                    ////if (!string.IsNullOrEmpty(sValues) && sValues.IndexOf("''") == -1)
                    ////{
                    ////    string provider = this.GetPorvider();
                    ////    string module = provider.Split('.')[0];
                    ////    string command = provider.Split('.')[1];

                    ////    string sql = CliUtils.GetSqlCommandText(module, command, CliUtils.fCurrentProject).ToUpper();
                    ////    string s = (sql.IndexOf(" WHERE ") == -1) ? " WHERE " + sValues : " AND " + sValues;
                    ////    if (sql.LastIndexOf("ORDER BY") != -1)
                    ////    {
                    ////        sql = sql.Insert(sql.LastIndexOf("ORDER BY"), s);
                    ////    }
                    ////    else
                    ////    {
                    ////        sql += s;
                    ////    }

                    ////    DataTable t = CliUtils.ExecuteSql(module, command, sql, true, CliUtils.fCurrentProject).Tables[0];
                    ////    if (t.Rows.Count == 0)
                    ////        return;
                    ////}
                    //object[] objParams = CliUtils.CallFLMethod("Pause", new object[] { null, new object[] { this.FLOWFILENAME + ".xoml", "", 0, 0, "", "", this.GetPorvider(), 0, "", "" }, new object[] { sKeys, sValues } });
                    //if (Convert.ToInt16(objParams[0]) == 0)
                    //{
                    //    message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "FLClientControls", "FLNavigator", "PauseSucceed", true);
                    //}
                    //else
                    //{
                    //    message = objParams[1].ToString();
                    //}
                    ////$edit20100604 by navy
                    ////if (message != "" && !csm.IsClientScriptBlockRegistered("Pause"))
                    ////    csm.RegisterClientScriptBlock(this.GetType(), "Pause", "alert('" + message + "');", true);
                    //if (message != "")
                    //    ScriptHelper.RegisterStartupScript(this, "alert('" + message + "');");
                    ////catch (FLException e)
                    ////{
                    ////    if (e.Type == 2)
                    ////    {
                    ////        csm.RegisterStartupScript(this.GetType(), "hasPaused", "alert('" + e.Message + "');", true);
                    ////    }
                    ////}        
                }
                else if (hContext.Request.Form["Active"] == "Preview")
                {
                    fParams.Operation = FlowOperation.Preview;
                    if (!String.IsNullOrEmpty(hContext.Request.Form["FLOWFILENAME"]))
                    {
                        var path = EFClientTools.ClientUtility.Client.CallMethod(EFClientTools.ClientUtility.ClientInfo, "GLModule", "GetServerPath", new List<object>());
                        fParams.XomlName = System.IO.Path.Combine(path.ToString(), "Workflow", hContext.Server.UrlDecode(hContext.Request.Form["FLOWFILENAME"]) + ".xoml");//STATUS
                    }
                    result = EFClientTools.ClientUtility.Client.CallFlowMethod(EFClientTools.ClientUtility.ClientInfo, fParams);
                    if (result.Status == FlowStatus.Exception)
                    {
                        WriteMessage(context.Response, result.Message, "");
                    }
                    else
                    {
                        string directory = hContext.Request.MapPath(hContext.Request.ApplicationPath);
                        //string directory = System.IO.Path.GetDirectoryName(path);
                        string fileName = DateTime.Now.ToString("yyMMddHHmmss") + ".jpg";
                        string path = directory + "\\WorkflowFiles\\PreView\\" + fileName;
                        directory = System.IO.Path.GetDirectoryName(path);
                        if (!System.IO.Directory.Exists(directory))
                        {
                            System.IO.Directory.CreateDirectory(directory);
                        }
                        System.IO.File.WriteAllBytes(path, (byte[])result.FLOther);
                        System.IO.FileInfo file = new System.IO.FileInfo(path);
                        WriteMessage(context.Response, fileName, "");
                    }
                }
                else if (hContext.Request.Form["Active"] == "FlowTransfer")
                {
                    fParams.Operation = FlowOperation.ChangeSendTo;
                    fParams.InstanceID = new Guid(hContext.Request.Form["LISTID"]);
                    fParams.CurrentActivity = hContext.Request.Form["FLOWPATH"];
                    fParams.SendToIDs = hContext.Request.Form["SENDTO_ID"];
                    fParams.Remark = hContext.Request.Form["SENDTO_NAME"];
                    EFClientTools.ClientUtility.Client.CallFlowMethod(EFClientTools.ClientUtility.ClientInfo, fParams);
                }
                else if (hContext.Request.Form["Active"] == "FlowUserGroups")
                {
                    FlowDataParameter fDataParams = new FlowDataParameter();
                    fDataParams.Description = " AND GROUPS.GROUPID='" + hContext.Request.Form["GroupID"] + "'";
                    var ds1 = EFClientTools.ClientUtility.Client.GetFlowDataDS(EFClientTools.ClientUtility.ClientInfo, FlowDataType.AllUsers, fDataParams);
                    DataSet ds = Deserialize<DataSet>(ds1);
                    if (ds.Tables.Count > 0)
                    {
                        context.Response.Write(DataTableToJson(ds.Tables[0], true));
                    }
                }
                else if (hContext.Request.Form["Active"] == "FlowGetAllRoleOrUser")
                {
                    if (hContext.Request.Form["SendToKind"] != null && hContext.Request.Form["SendToKind"].ToString() == "2")
                    {
                        FlowDataParameter fDataParams = new FlowDataParameter();
                        fDataParams.Description = "";
                        var ds1 = EFClientTools.ClientUtility.Client.GetFlowDataDS(EFClientTools.ClientUtility.ClientInfo, FlowDataType.AllUsers, fDataParams);
                        DataSet ds = Deserialize<DataSet>(ds1);
                        if (ds.Tables.Count > 0)
                        {
                            context.Response.Write(DataTableToJson(ds.Tables[0], true));
                        }
                    }
                    else
                    {
                        FlowDataParameter fDataParams = new FlowDataParameter();
                        fDataParams.Description = "";
                        var ds1 = EFClientTools.ClientUtility.Client.GetFlowDataDS(EFClientTools.ClientUtility.ClientInfo, FlowDataType.AllGroups, fDataParams);
                        DataSet ds = Deserialize<DataSet>(ds1);
                        if (ds.Tables.Count > 0)
                        {
                            context.Response.Write(DataTableToJson(ds.Tables[0], true));
                        }
                    }
                }
            }
            else if (hContext.Request.QueryString["Type"] == "getMaxQueryStringLength")
            {
                System.Web.Configuration.HttpRuntimeSection hrs = new System.Web.Configuration.HttpRuntimeSection();
                WriteMessage(context.Response, hrs.MaxQueryStringLength.ToString(), "");
                
            }
        }
        catch (Exception e)
        {

            var ex = e;
            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
            }
            throw ex;

            //context.Response.Write(ex.Message);
        }
    }

    private void DoApprove(String status, EFBase.MessageProvider provider, FlowParameter fParams)
    {
        FlowResult result = null;
        String messageKey = String.Empty;
        if (status == "A" || status == "AA" || status == "加签" || status == "加簽" || status == "Plus")
        {
            fParams.CurrentActivity = fParams.NextActivity;
            fParams.Operation = FlowOperation.PlusReturn;
            result = EFClientTools.ClientUtility.Client.CallFlowMethod(EFClientTools.ClientUtility.ClientInfo, fParams);
        }
        else
        {
            fParams.Operation = FlowOperation.Approve;
            result = EFClientTools.ClientUtility.Client.CallFlowMethod(EFClientTools.ClientUtility.ClientInfo, fParams);
        }
        if (result != null)
        {
            if (result.Status == FlowStatus.End)
            {
                messageKey = "FLClientControls/FLNavigator/RunOver";
                WriteMessage(hContext.Response, provider[messageKey], "<br/>");
            }
            else if (result.Status == FlowStatus.Rejected)
            {
                messageKey = "FLClientControls/FLNavigator/HasRejected";
                WriteMessage(hContext.Response, provider[messageKey], "<br/>");
            }
            else if (result.Status == FlowStatus.Exception)
            {
                WriteMessage(hContext.Response, result.Message, "<br/>");
            }
            else
            {
                if (!String.IsNullOrEmpty(result.Message))
                {
                    WriteMessage(hContext.Response, result.Message, "<br/>");
                }
                else if (string.IsNullOrEmpty(result.SendToIDs))
                {
                    //string wait = SysMsg.GetSystemMessage(CliUtils.fClientLang, "FLTools", "GloFix", "WaitMessage", true);
                    FlowDataParameter fdp = new FlowDataParameter();
                    fdp.ListID = fParams.InstanceID;
                    var ds1 = EFClientTools.ClientUtility.Client.GetFlowDataDS(EFClientTools.ClientUtility.ClientInfo, FlowDataType.Do, fdp);
                    DataSet ds = Deserialize<DataSet>(ds1);
                    //string sql = "select SENDTO_KIND, SENDTO_ID from SYS_TODOLIST where LISTID='" + fParams.InstanceID.ToString() + "' and STATUS <> 'F'";
                    //DataSet ds = EFClientTools.ClientUtility.ExecuteSQL(EFClientTools.ClientUtility.ClientInfo.Database, sql);
                    DataTable dtOthers = null;
                    if (ds.Tables.Count > 0)
                        dtOthers = ds.Tables[0];

                    string sendToIds = "";
                    foreach (DataRow row in dtOthers.Rows)
                    {
                        string sendtokind = row["SENDTO_KIND"].ToString();
                        string sendtoid = row["SENDTO_ID"].ToString();
                        if (sendtokind == "1")
                        {
                            sendToIds += sendtoid + ";";
                        }
                        else if (sendtokind == "2")
                        {
                            sendToIds += sendtoid + ":UserId;";
                        }
                    }
                    if (sendToIds != "")
                    {
                        List<object> param = new List<object>();
                        param.Add(sendToIds);
                        param.Add(true);
                        EFClientTools.ClientUtility.ClientInfo.UseDataSet = false;
                        var message = EFClientTools.ClientUtility.Client.CallMethod(EFClientTools.ClientUtility.ClientInfo, "EFGlobalModule", "GloFix_ShowParallelMessage", param);
                        EFClientTools.ClientUtility.ClientInfo.UseDataSet = true;
                        WriteMessage(hContext.Response, message.ToString(), "<br/>");
                    }
                }
                else
                {
                    List<object> param = new List<object>();
                    param.Add(result.SendToIDs);
                    param.Add(true);
                    EFClientTools.ClientUtility.ClientInfo.UseDataSet = false;
                    var message = EFClientTools.ClientUtility.Client.CallMethod(EFClientTools.ClientUtility.ClientInfo, "EFGlobalModule", "GloFix_ShowMessage", param);
                    EFClientTools.ClientUtility.ClientInfo.UseDataSet = true;
                    messageKey = "FLClientControls/FLWizard/Send";
                    WriteMessage(hContext.Response, provider[messageKey] + message, "<br/>");
                }
            }
        }
        else
        {
            //if (Convert.ToInt16(objParams[0]) == 2)
            //    this.result.Text = objParams[1].ToString();
        }
    }

    public void DoReturn(String status, EFBase.MessageProvider provider, FlowParameter fParams)
    {
        FlowResult result = null;
        String multistepreturn = hContext.Request.Form["MULTISTEPRETURN"];//MULTISTEPRETURN
        String returnstep = hContext.Request.Form["returnstep"];
        String messageKey = String.Empty;
        if (status == "A" || status == "AA" || status == "加签" || status == "加簽" || status == "Plus")
        {
            fParams.Operation = FlowOperation.PlusReturnToSender;
            result = EFClientTools.ClientUtility.Client.CallFlowMethod(EFClientTools.ClientUtility.ClientInfo, fParams);
            //objParams = CliUtils.CallFLMethod("PlusReturn2", new object[] { new Guid(this.ViewState["ListId"].ToString()), new object[] { fLActivities[0], fLActivities[1], isImport, isUrgent, suggest, role, provider, mailAddress, "", attachments }, new object[] { keys, values } });
        }
        //else if (multistepreturn == "0")
        //{
        //    fParams.Operation = FlowOperation.Return;
        //    result = EFClientTools.ClientUtility.Client.CallFlowMethod(EFClientTools.ClientUtility.ClientInfo, fParams);
        //}
        //else if (multistepreturn == "1")
        //{
        else if (returnstep == "0")
        {
            fParams.Operation = FlowOperation.Return;
            result = EFClientTools.ClientUtility.Client.CallFlowMethod(EFClientTools.ClientUtility.ClientInfo, fParams);
        }
        else
        {
            fParams.CurrentActivity = hContext.Request.Form["returnsteptext"];
            fParams.Operation = FlowOperation.ReturnToStep;
            result = EFClientTools.ClientUtility.Client.CallFlowMethod(EFClientTools.ClientUtility.ClientInfo, fParams);
        }
        //}
        //else
        //    return;

        if (result != null)
        {
            if (result.InstanceID.ToString() == "B4DAF3A4-AAE8-4b51-A391-B52E46305E9F")
            {
                messageKey = "FLClientControls/FLWebNavigator/ReturnToEnd";
                WriteMessage(hContext.Response, provider[messageKey], "<br/>");
            }
            else if (result.InstanceID.ToString() == "C912D847-1825-458a-8CB5-E680FACA42AF")
            {
                List<object> param = new List<object>();
                param.Add(result.SendToIDs.Split(':')[1]);
                param.Add(true);
                EFClientTools.ClientUtility.ClientInfo.UseDataSet = false;
                var message = EFClientTools.ClientUtility.Client.CallMethod(EFClientTools.ClientUtility.ClientInfo, "EFGlobalModule", "GloFix_ShowMessage3", param);
                EFClientTools.ClientUtility.ClientInfo.UseDataSet = true;
                messageKey = "FLClientControls/FLWebNavigator/ReturnToEnd";
                WriteMessage(hContext.Response, message.ToString(), "<br/>");
            }
            else
            {
                if (result.Status == FlowStatus.Exception)
                {
                    WriteMessage(hContext.Response, result.Message, "<br/>");
                }
                else if (result.SendToIDs.ToString() == "")
                {
                    //messageKey = "FLTools/GloFix/WaitMessage";
                    //string wait = provider[messageKey];
                    FlowDataParameter fdp = new FlowDataParameter();
                    fdp.ListID = fParams.InstanceID;
                    var ds1 = EFClientTools.ClientUtility.Client.GetFlowDataDS(EFClientTools.ClientUtility.ClientInfo, FlowDataType.Do, fdp);
                    DataSet ds = Deserialize<DataSet>(ds1);
                    //string sql = "select SENDTO_KIND, SENDTO_ID from SYS_TODOLIST where LISTID='" + fParams.InstanceID.ToString() + "' and STATUS <> 'F'";
                    //DataSet ds = EFClientTools.ClientUtility.ExecuteSQL(EFClientTools.ClientUtility.ClientInfo.Database, sql);
                    DataTable dtOthers = null;
                    if (ds.Tables.Count > 0)
                        dtOthers = ds.Tables[0];
                    string sendToIds = "";
                    foreach (DataRow row in dtOthers.Rows)
                    {
                        string sendtokind = row["SENDTO_KIND"].ToString();
                        string sendtoid = row["SENDTO_ID"].ToString();
                        if (sendtokind == "1")
                        {
                            sendToIds += sendtoid + ";";
                        }
                        else if (sendtokind == "2")
                        {
                            sendToIds += sendtoid + ":UserId;";
                        }
                    }
                    if (sendToIds != "")
                    {
                        List<object> param = new List<object>();
                        param.Add(sendToIds);
                        //param.Add(result.SendToIDs);
                        param.Add(false);
                        EFClientTools.ClientUtility.ClientInfo.UseDataSet = false;
                        var message = EFClientTools.ClientUtility.Client.CallMethod(EFClientTools.ClientUtility.ClientInfo, "EFGlobalModule", "GloFix_ShowParallelMessage", param);
                        EFClientTools.ClientUtility.ClientInfo.UseDataSet = true;
                        WriteMessage(hContext.Response, message.ToString(), "<br/>");
                    }
                }
                else
                {
                    List<object> param = new List<object>();
                    param.Add(result.SendToIDs);
                    param.Add(false);
                    EFClientTools.ClientUtility.ClientInfo.UseDataSet = false;
                    var message = EFClientTools.ClientUtility.Client.CallMethod(EFClientTools.ClientUtility.ClientInfo, "EFGlobalModule", "GloFix_ShowMessage", param);
                    EFClientTools.ClientUtility.ClientInfo.UseDataSet = true;
                    messageKey = "FLClientControls/FLWizard/Send";
                    WriteMessage(hContext.Response, provider[messageKey] + message, "<br/>");
                }
            }
        }
        else
        {
            //if (Convert.ToInt16(objParams[0]) == 2)
            //    this.result.Text = objParams[1].ToString();
        }
    }


    //A3-Cross-Site Scripting (XSS)
    private void WriteMessage(HttpResponse response, string message, string html)
    {
        response.Write(HttpUtility.HtmlEncode(message) + html);
    }

    public string DataTableToJson(DataTable dt)
    {
        return DataTableToJson(dt, false);
    }

    public string DataTableToJson(DataTable dt, bool toUpper)
    {
        if (toUpper == true)
        {
            DataTable newTable = new DataTable();
            foreach (DataColumn column in dt.Columns)
            {
                newTable.Columns.Add(column.ColumnName.ToUpper(), column.DataType, column.Expression);
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = newTable.NewRow();
                foreach (DataColumn column in dt.Columns)
                {
                    dr[column.ColumnName.ToUpper()] = dt.Rows[i][column.ColumnName];
                }
                newTable.Rows.Add(dr);
            }
            return JsonConvert.SerializeObject(newTable, Formatting.Indented);//Indented縮排
        }
        else
        {
            return JsonConvert.SerializeObject(dt, Formatting.Indented);//Indented縮排
        }
    }

    public string DataTableToJson2(DataTable dt, object count)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        string js = JsonConvert.SerializeObject(dt, Formatting.Indented);//Indented縮排
        sb.Append("{\"total\":");
        sb.Append(count.ToString());
        //sb.Append("50".ToString());
        sb.Append(",\"rows\":");
        sb.Append(js);
        sb.Append("}");
        return sb.ToString();
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

    private string GetMailAddress()
    {
        string url = hContext.Request.Url.AbsoluteUri;
        string paramters = "?FolderName={0}&FormName={1}&LISTID={2}&FLOWPATH={3}&WHERESTRING={4}&NAVMODE={5}&FLNAVMODE={6}&Users={7}&PLUSAPPROVE={8}&STATUS={9}&SENDTOID={10}&MULTISTEPRETURN={11}&ATTACHMENTS={12}";

        //paramters = HttpUtility.UrlEncode(paramters);
        url = url.Substring(0, url.IndexOf("handler/")) + "MainPage_Flow.aspx" + paramters;
        return url;
    }

    bool IgnoreWeekends = true;
    private bool IsOverTime(string TIME_UNIT, string FLOWURGENT, string UPDATE_DATE, string UPDATE_TIME, string URGENT_TIME, string EXP_TIME)
    {
        if (TIME_UNIT == "Day" && FLOWURGENT == "1")
        {
            if (Convert.ToDecimal(URGENT_TIME) == Decimal.Zero) return false;
            TimeSpan span = WorkTimeSpan(DateTime.Now.Date, Convert.ToDateTime(UPDATE_DATE), IgnoreWeekends, null);
            int overtimes = span.Days - Convert.ToInt32(Convert.ToDecimal(URGENT_TIME));
            if (overtimes >= 0)
            {
                return true;
            }
        }
        else if (TIME_UNIT == "Day" && FLOWURGENT == "0")
        {
            if (Convert.ToDecimal(EXP_TIME) == Decimal.Zero) return false;
            TimeSpan span = WorkTimeSpan(DateTime.Now.Date, Convert.ToDateTime(UPDATE_DATE), IgnoreWeekends, null);
            int overtimes = span.Days - Convert.ToInt32(Convert.ToDecimal(EXP_TIME));
            if (overtimes >= 0)
            {
                return true;
            }
        }
        else if (TIME_UNIT == "Hour" && FLOWURGENT == "1")
        {
            if (Convert.ToDecimal(URGENT_TIME) == Decimal.Zero) return false;
            TimeSpan spanDay = WorkTimeSpan(DateTime.Now.Date, Convert.ToDateTime(UPDATE_DATE), IgnoreWeekends, null);
            int spanHour = DateTime.Now.Hour - Convert.ToDateTime(UPDATE_TIME).Hour;
            if (DateTime.Now.Minute < Convert.ToDateTime(UPDATE_TIME).Minute)
            {
                spanHour -= 1;
            }
            int overtimes = spanDay.Days * 8 + spanHour - Convert.ToInt32(Convert.ToDecimal(URGENT_TIME));
            if (overtimes >= 0)
            {
                return true;
            }
        }
        else if (TIME_UNIT == "Hour" && FLOWURGENT == "0")
        {
            if (Convert.ToDecimal(EXP_TIME) == Decimal.Zero) return false;
            TimeSpan spanDay = WorkTimeSpan(DateTime.Now.Date, Convert.ToDateTime(UPDATE_DATE), IgnoreWeekends, null);
            int spanHour = DateTime.Now.Hour - Convert.ToDateTime(UPDATE_TIME).Hour;
            if (DateTime.Now.Minute < Convert.ToDateTime(UPDATE_TIME).Minute)
            {
                spanHour -= 1;
            }
            int overtimes = spanDay.Days * 8 + spanHour - Convert.ToInt32(Convert.ToDecimal(EXP_TIME));
            if (overtimes >= 0)
            {
                return true;
            }
        }
        return false;
    }

    private TimeSpan WorkTimeSpan(DateTime nowTime, DateTime updateTime, bool weekendSensible, List<string> extDates)
    {
        TimeSpan span = new TimeSpan();
        if (weekendSensible)
        {
            if (nowTime.DayOfWeek == DayOfWeek.Saturday)
            {
                nowTime = nowTime.Date.AddSeconds(-1);
            }
            else if (nowTime.DayOfWeek == DayOfWeek.Sunday)
            {
                nowTime = nowTime.Date.AddDays(-1).AddSeconds(-1);
            }

            if (updateTime.DayOfWeek == DayOfWeek.Saturday)
            {
                updateTime = updateTime.Date.AddDays(2);
            }
            else if (updateTime.DayOfWeek == DayOfWeek.Sunday)
            {
                updateTime = updateTime.Date.AddDays(1);
            }
        }
        span = nowTime - updateTime;
        if (weekendSensible)
        {
            int weekends = span.Days / 7;
            int i = nowTime.DayOfWeek - updateTime.DayOfWeek;
            if (i < 0)
                weekends++;
            span = span.Subtract(new TimeSpan(2 * weekends, 0, 0, 0));
        }
        int extDays = 0;
        if (extDates == null) return span;
        foreach (string extDate in extDates)
        {
            if (Convert.ToDateTime(extDate).CompareTo(nowTime) < 0
                && Convert.ToDateTime(extDate).CompareTo(updateTime) > 0)
            {
                if (weekendSensible)
                {
                    if (Convert.ToDateTime(extDate).DayOfWeek != DayOfWeek.Saturday
                        && Convert.ToDateTime(extDate).DayOfWeek != DayOfWeek.Sunday)
                    {
                        extDays++;
                    }
                }
                else
                {
                    extDays++;
                }
            }
        }
        span = span.Subtract(new TimeSpan(extDays, 0, 0, 0));
        return span;
    }

    public DataTable FLOvertimeList(string user, int level, bool isAllData, HttpContext context, FlowDataParameter fParam)
    {
        try
        {
            bool delay = true;
            FlowDataParameter flp = fParam;
            if (flp == null)
            {
                flp = new FlowDataParameter();
            }
            flp.Description = level.ToString();
            var ds1 = EFClientTools.ClientUtility.Client.GetFlowDataDS(EFClientTools.ClientUtility.ClientInfo, FlowDataType.Overtime, flp);
            DataSet ds = Deserialize<DataSet>(ds1);
            DataTable allList = ds.Tables[0];

            if (delay)
            {
                DataColumn colSendToDetail = new DataColumn("SENDTO_DETAIL", typeof(string), "SENDTO_ID+'('+USERNAME+')'");
                DataColumn colUpdateWholeTime = new DataColumn("UPDATE_WHOLE_TIME", typeof(string), "UPDATE_DATE + ' ' + UPDATE_TIME");
                DataColumn colOverTime = new DataColumn("OVERTIME", typeof(string));
                allList.Columns.AddRange(new DataColumn[] { colSendToDetail, colUpdateWholeTime, colOverTime });
            }
            if (isAllData)
                return allList;

            List<DataRow> overTimeRows = new List<DataRow>();
            #region find over time
            foreach (DataRow row in allList.Rows)
            {
                string TIME_UNIT = row["TIME_UNIT"].ToString();
                string FLOWURGENT = row["FLOWURGENT"].ToString();
                string UPDATE_DATE = row["UPDATE_DATE"].ToString();
                string UPDATE_TIME = row["UPDATE_TIME"].ToString();
                string URGENT_TIME = row["URGENT_TIME"].ToString();
                string EXP_TIME = row["EXP_TIME"].ToString();

                //IsOverTime(TIME_UNIT, FLOWURGENT, UPDATE_DATE, UPDATE_TIME, URGENT_TIME, EXP_TIME);
                if (TIME_UNIT == "Day" && FLOWURGENT == "1")
                {
                    if (Convert.ToDecimal(URGENT_TIME) == Decimal.Zero) continue;
                    TimeSpan span = this.WorkTimeSpan(DateTime.Now.Date, Convert.ToDateTime(UPDATE_DATE), IgnoreWeekends, null);

                    int overtimes = span.Days - Convert.ToInt32(Convert.ToDecimal(URGENT_TIME));
                    if (delay) row["OVERTIME"] = overtimes.ToString() + "Days";
                    if (overtimes >= 0)
                    {
                        overTimeRows.Add(row);
                    }
                }
                else if (TIME_UNIT == "Day" && FLOWURGENT == "0")
                {
                    if (Convert.ToDecimal(EXP_TIME) == Decimal.Zero) continue;
                    TimeSpan span = this.WorkTimeSpan(DateTime.Now.Date, Convert.ToDateTime(UPDATE_DATE), IgnoreWeekends, null);
                    int overtimes = span.Days - Convert.ToInt32(Convert.ToDecimal(EXP_TIME));
                    if (delay) row["OVERTIME"] = overtimes.ToString() + "Days";
                    if (overtimes >= 0)
                    {
                        overTimeRows.Add(row);
                    }
                }
                else if (TIME_UNIT == "Hour" && FLOWURGENT == "1")
                {
                    if (Convert.ToDecimal(URGENT_TIME) == Decimal.Zero) continue;
                    TimeSpan spanDay = this.WorkTimeSpan(DateTime.Now.Date, Convert.ToDateTime(UPDATE_DATE), IgnoreWeekends, null);
                    int spanHour = DateTime.Now.Hour - Convert.ToDateTime(UPDATE_TIME).Hour;
                    int overtimes = spanDay.Days * 8 + spanHour - Convert.ToInt32(Convert.ToDecimal(URGENT_TIME));
                    if (delay) row["OVERTIME"] = overtimes.ToString() + "Hours";
                    if (overtimes >= 0)
                    {
                        overTimeRows.Add(row);
                    }
                }
                else if (TIME_UNIT == "Hour" && FLOWURGENT == "0")
                {
                    if (Convert.ToDecimal(EXP_TIME) == Decimal.Zero) continue;
                    TimeSpan spanDay = this.WorkTimeSpan(DateTime.Now.Date, Convert.ToDateTime(UPDATE_DATE), IgnoreWeekends, null);
                    int spanHour = DateTime.Now.Hour - Convert.ToDateTime(UPDATE_TIME).Hour;
                    int overtimes = spanDay.Days * 8 + spanHour - Convert.ToInt32(Convert.ToDecimal(EXP_TIME));
                    if (delay) row["OVERTIME"] = overtimes.ToString() + "Hours";
                    if (overtimes >= 0)
                    {
                        overTimeRows.Add(row);
                    }
                }
            }
            #endregion

            DataTable overtimeList = null;
            if (delay)
            {
                overtimeList = allList.Clone();
                foreach (DataRow row in overTimeRows)
                {
                    overtimeList.ImportRow(row);
                }
            }
            else
            {
                overtimeList = new DataTable();
                DataColumn colFlowDesc = new DataColumn("FLOW_DESC", typeof(string));
                DataColumn colDelayCount = new DataColumn("DELAY_COUNT", typeof(int));
                overtimeList.Columns.AddRange(new DataColumn[] { colFlowDesc, colDelayCount });

                string desc = "";
                foreach (DataRow row in overTimeRows)
                {
                    if (desc != row["FLOW_DESC"].ToString())
                    {
                        desc = row["FLOW_DESC"].ToString();
                        int count = overTimeRows.FindAll(delegate(DataRow irow) { return irow["FLOW_DESC"].ToString() == desc; }).Count;
                        DataRow newRow = overtimeList.NewRow();
                        newRow["FLOW_DESC"] = desc;
                        newRow["DELAY_COUNT"] = count;
                        overtimeList.Rows.Add(newRow);
                    }
                }
            }
            return overtimeList;
        }
        finally
        {

        }
    }

    private static T Deserialize<T>(string xml)
    {
        if (string.IsNullOrEmpty(xml))
        {
            return default(T);
        }

        System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
        System.Xml.XmlReaderSettings settings = new System.Xml.XmlReaderSettings();
        settings.CheckCharacters = false;
        // No settings need modifying here      
        using (System.IO.StringReader textReader = new System.IO.StringReader(xml))
        {
            using (System.Xml.XmlReader xmlReader = System.Xml.XmlReader.Create(textReader, settings))
            {
                return (T)serializer.Deserialize(xmlReader);
            }
        }
    }
}

public enum ESqlMode
{
    ToDoList = 1,
    ToDoHis = 2,
    Notify = 3,
    Delay = 4,
    ToDoListStatist = 5,
    ToDoHisStatist = 6,
    NotifyStatist = 7,
    DelayStatist = 8,
    AllStatist = 9,
    FlowRunOver = 10
}