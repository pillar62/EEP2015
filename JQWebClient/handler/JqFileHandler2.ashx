<%@ WebHandler Language="C#" Class="JQFileHandler2" %>

using System;
using System.Web;

public class JQFileHandler2 : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        
        var path = context.Request.QueryString["File"];

        path = string.Format("../download/{0}", path);
        System.IO.FileInfo file = new System.IO.FileInfo(HttpContext.Current.Server.MapPath(path));
        context.Response.Clear();
        context.Response.Buffer = false;
        context.Response.ContentType = "application/octet-stream";
        context.Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
        context.Response.AddHeader("Content-Length", file.Length.ToString());
        context.Response.Filter.Close();
        context.Response.WriteFile(file.FullName);
        context.Response.End();
    }

public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}