<%@ WebHandler Language="C#" Class="JQFileHandler" %>

using System;
using System.Web;

public class JQFileHandler : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    public void ProcessRequest(HttpContext context)
    {
        EFClientTools.ClientUtility.ClientInfo.cErrorCode = 1270;
        var fileName = context.Request.QueryString["File"];
        //if (string.IsNullOrEmpty(fileName))
        //{
        //    var id = context.Request.QueryString["ID"];
        //    var chars = id.ToCharArray();
        //    Array.Reverse(chars);

        //    var str = "";
        //    var encodeFileName = string.Empty;
        //    for (int i = 0; i < chars.Length; i++)
        //    {
        //        str += chars[i];
        //        if (i % 2 == 1)
        //        {
        //            var code = int.Parse(str, System.Globalization.NumberStyles.HexNumber);
        //            encodeFileName += (char)code;
        //            str = "";
        //        }
        //    }
        //    fileName = HttpUtility.UrlDecode(encodeFileName);
            
        //}
        var path = string.Format("../Files/{0}", fileName);
        
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

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}