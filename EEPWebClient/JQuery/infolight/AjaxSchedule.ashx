<%@ WebHandler Language="C#" Class="AjaxSchedule" %>

using System;
using System.Web;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Srvtools;

public class AjaxSchedule : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    public void ProcessRequest(HttpContext context)
    {
        string start = context.Request["start"];
        string end = context.Request["end"];
        string json = "";
        Nullable<DateTime> startTime = new Nullable<DateTime>();
        Nullable<DateTime> endTime = new Nullable<DateTime>();
        if (!string.IsNullOrEmpty(start) && !string.IsNullOrEmpty(end))
        {
            startTime = DateTime.Parse(start);
            endTime = DateTime.Parse(end);
        }
        string cachename = context.Request["cacheDataSet"];
        string command = context.Request["command"];
        string idField = context.Request["idField"];
        string titleField = context.Request["titleField"];
        string startField = context.Request["startField"];
        string endField = context.Request["endField"];
        string descriptionField = context.Request["descriptionField"];
        string allDayField = context.Request["allDayField"];

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
                cacheTable = cacheData.Tables[command];
            }
            else
            {
                return;
            }
            string oper = context.Request["oper"];
            if (string.IsNullOrEmpty(oper))
            {
                DataRow[] rows = null;
                if (startTime != null && endTime != null)
                {
                    rows = cacheTable.Select(string.Format("{0}>='{1}' AND {2}<'{3}'", startField, startTime.ToString(), endField, endTime.ToString()));
                }
                else
                {
                    rows = cacheTable.Select();
                }
                if (rows.Length > 0)
                {
                    json = this.GenReturnJson(rows, idField, titleField, startField, endField, descriptionField, allDayField);
                }
            }
            else
            {
                string module = context.Request["module"];
                string sqlTableName = CliUtils.GetTableName(module, command, CliUtils.fCurrentProject);
                if (!string.IsNullOrEmpty(sqlTableName))
                {
                    string values = context.Request["recordValues"];
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    Dictionary<string, object> recordValues = serializer.DeserializeObject(values) as Dictionary<string, object>;
                    InfoDataSet ds = new InfoDataSet();
                    ds.AlwaysClose = false;
                    ds.RemoteName = string.Format("{0}.{1}", module, command);
                    ds.ServerModify = true;
                    ds.PacketRecords = -1;
                    ds.Active = true;
                    DataTable tab = ds.RealDataSet.Tables[command];
                    if (tab != null)
                    {
                        if (oper == "insert")
                        {
                            string sfillter = string.Format("Max({0})",idField);
                            string maxs = tab.Compute(sfillter, "").ToString();
                            int maxi = 0;
                            if(Int32.TryParse(maxs, out maxi))
                                maxi ++;
                            
                            DataRow newRow = tab.NewRow();
                            //newRow[idField] = 0;
                            newRow[idField] = maxi;
                            newRow[titleField] = recordValues[titleField];
                            if (!string.IsNullOrEmpty(descriptionField))
                            {
                                newRow[descriptionField] = recordValues[descriptionField];
                            }
                            if (!string.IsNullOrEmpty(allDayField))
                            {
                                newRow[allDayField] = recordValues[allDayField];
                            }
                            newRow[startField] = recordValues[startField];
                            newRow[endField] = recordValues[endField];
                            tab.Rows.Add(newRow);

                            ds.ApplyUpdates();
                            cacheTable.Merge(tab);

                            json = this.GenRowJson(newRow, idField, titleField, startField, endField, descriptionField, allDayField);
                        }
                        else if (oper == "update")
                        {
                            DataRow updateRow = tab.Rows.Find(recordValues[idField]);
                            updateRow[titleField] = recordValues[titleField];
                            if (!string.IsNullOrEmpty(descriptionField))
                            {
                                updateRow[descriptionField] = recordValues[descriptionField];
                            }
                            if (!string.IsNullOrEmpty(allDayField))
                            {
                                updateRow[allDayField] = recordValues[allDayField];
                            }
                            updateRow[startField] = recordValues[startField];
                            updateRow[endField] = recordValues[endField];
                            ds.ApplyUpdates();
                            cacheTable.Merge(tab);
                            json = this.GenRowJson(updateRow, idField, titleField, startField, endField, descriptionField, allDayField);

                        }
                        else if (oper == "delete")
                        {
                            DataRow delRow = cacheTable.Rows.Find(recordValues[idField]);
                            delRow.Delete();
                            tab.Merge(cacheTable.GetChanges(DataRowState.Deleted));
                            ds.ApplyUpdates();
                            cacheTable.AcceptChanges();
                        }
                        else if (oper == "drag")
                        {
                            DataRow updateRow = tab.Rows.Find(recordValues[idField]);
                            updateRow[startField] = recordValues[startField];
                            updateRow[endField] = recordValues[endField];
                            ds.ApplyUpdates();
                            cacheTable.Merge(tab);
                            //json = this.GenRowJson(updateRow, idField, titleField, startField, endField, descriptionField, allDayField);
                        }
                    }
                }
            }
        }
        
        context.Response.Write(json);
    }

    public string GenRowJson(DataRow row, string idField, string titleField, string startField, string endField, string descField, string allDayField)
    {
        StringBuilder builder = new StringBuilder();
        builder.Append("{");
        builder.AppendFormat("id:{0},title:'{1}',start:new Date({2}),end:new Date({3}),",
            row[idField],
            row[titleField],
            MilliTimeStamp((DateTime)row[startField]),
            MilliTimeStamp((DateTime)row[endField]));
        if (!string.IsNullOrEmpty(descField))
        {
            builder.AppendFormat("description:'{0}',", row[descField]);
        }
        if (!string.IsNullOrEmpty(allDayField))
        {
            string allDay = (row[allDayField] == null || string.IsNullOrEmpty(row[allDayField].ToString())) ? "false" : row[allDayField].ToString().ToLower();
            builder.AppendFormat("allDay:{0}", allDay);
        }
        else
        {
            builder.Append("allDay:false");
        }
        if (builder.ToString().EndsWith(","))
        {
            builder.Remove(builder.Length - 1, 1);
        }
        builder.Append("}");
        return builder.ToString();
    }

    public string GenReturnJson(DataRow[] rows, string idField, string titleField, string startField, string endField, string descField, string allDayField)
    {
        StringBuilder builder = new StringBuilder();
        builder.Append("[");
        foreach (DataRow row in rows)
        {
            builder.Append(this.GenRowJson(row, idField, titleField, startField, endField, descField, allDayField));
            builder.Append(",");
        }
        if (builder.ToString().EndsWith(","))
        {
            builder.Remove(builder.Length - 1, 1);
        }
        builder.Append("]");
        return builder.ToString();
    }

    public double MilliTimeStamp(DateTime date)
    {
        DateTime baseDate = new DateTime(1970, 1, 1);
        TimeSpan ts = new TimeSpan(date.ToUniversalTime().Ticks - baseDate.Ticks);

        return ts.TotalMilliseconds;
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}