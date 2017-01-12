<%@ WebHandler Language="C#" Class="ExtGetSysMessage" %>

using System;
using System.Web;
using Srvtools;

public class ExtGetSysMessage: IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    public void ProcessRequest(HttpContext context)
    {
        try
        {
            if (string.IsNullOrEmpty(CliUtils.fLoginUser))
            {
                throw new Exception("75FF57F7-7AC0-43c8-9454-C92B4A2723BB");
            }
            string type = context.Request["type"];
            if (type == "grid")
            {
                context.Response.Write(string.Format("{{success:true,msgSureDelete:'{0}',msgNonSelToDelete:'{1}',msgNonSelToEdit:'{2}',msgPersonalSaved:'{3}',msgPersonalLoaded:'{4}'}}",
                    SysMsg.GetSystemMessage(CliUtils.fClientLang, "AjaxTools", "ExtGrid", "SureDelete", true),
                    SysMsg.GetSystemMessage(CliUtils.fClientLang, "AjaxTools", "ExtGrid", "NonSelectToDelete", true),
                    SysMsg.GetSystemMessage(CliUtils.fClientLang, "AjaxTools", "ExtGrid", "NonSelectToEdit", true),
                    SysMsg.GetSystemMessage(CliUtils.fClientLang, "AjaxTools", "ExtGrid", "PersonalSaved", true),
                    SysMsg.GetSystemMessage(CliUtils.fClientLang, "AjaxTools", "ExtGrid", "PersonalLoaded", true)));
            }
            else if (type == "form")
            {
                context.Response.Write(string.Format("{{success:true,msgValidFail:'{0}',msgSaveDetails:'{1}'}}",
                    SysMsg.GetSystemMessage(CliUtils.fClientLang, "Srvtools", "DefaultValidate", "msg_DefaultValidateCheckMethod", true),
                    SysMsg.GetSystemMessage(CliUtils.fClientLang, "AjaxTools", "AjaxLayout", "SaveDetails", true)));
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

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}