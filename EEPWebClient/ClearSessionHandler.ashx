<%@ WebHandler Language="C#" Class="ClearSessionHandler" %>

using System;
using System.Web;
using System.Web.SessionState;

public class ClearSessionHandler : IHttpHandler, IRequiresSessionState
{

    public void ProcessRequest(HttpContext context)
    {
        context.Session.Abandon();
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}