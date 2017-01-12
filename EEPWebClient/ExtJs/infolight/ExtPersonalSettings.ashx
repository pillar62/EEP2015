<%@ WebHandler Language="C#" Class="ExtPersonalSettings" %>

using System;
using System.Web;
using Srvtools;
using System.Data;
using AjaxTools;

public class ExtPersonalSettings : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    public void ProcessRequest(HttpContext context)
    {
        try 
        {
            if (string.IsNullOrEmpty(CliUtils.fLoginUser))
            {
                throw new Exception("75FF57F7-7AC0-43c8-9454-C92B4A2723BB");
            }
            string oper = context.Request["oper"];
            string formName = context.Request["formName"];
            string compName = context.Request["compName"];
            string userId = context.Request["userId"];
            string json = "";
            if (oper == "load")
            {
                object[] result = CliUtils.CallMethod("GLModule", "LoadPersonalSettings", new object[] { formName, compName, userId });
                if ((int)result[0] == 0)
                {
                    DataTable dt = result[1] as DataTable;
                    if (dt.Rows.Count != 1)
                    {
                        json = "{success:false,message:'No personal settings has been found!'}";
                    }
                    else
                    {
                        json = string.Format("{{success:true,content:{0}}}", dt.Rows[0]["PROPCONTENT"].ToString());
                    }
                }
                else
                {
                    json = string.Format("{{success:false,message:\"{0}\"}}", result[1].ToString());
                }
            }
            else if (oper == "save")
            {
                string remark = context.Request["remark"];
                string propContent = context.Request["propContent"];

                object[] result = CliUtils.CallMethod("GLModule", "SavePersonalSettings", new object[] { formName, compName, userId, remark, propContent });
                if ((int)result[0] == 0)
                {
                    json = "{success:true}";
                }
                else
                {
                    json = string.Format("{{success:false,message:\"{0}\"}}", result[1].ToString());
                }
            }
            context.Response.Write(json);
        }
        catch (Exception exception)
        {
            string errorMsg = exception.Message;
            context.Response.Write(string.Format("{{success:false,message:\"{0}\",stack:\"{1}\"}}",
                errorMsg.Replace("\r", "\\r").Replace("\n", "\\n").Replace(@"\", @"\\"),
                exception.StackTrace.Replace("\r", "\\r").Replace("\n", "\\n").Replace(@"\", @"\\")));
        }
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}