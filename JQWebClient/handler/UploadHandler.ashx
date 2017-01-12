<%@ WebHandler Language="C#" Class="UploadHandler" %>

using System;
using System.Web;

public class UploadHandler : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    public void ProcessRequest(HttpContext context)
    {
        HttpRequest Request = context.Request;
        HttpResponse Response = context.Response;
        HttpServerUtility Server = context.Server;
        //指定输出头和编码
        Response.ContentType = "text/plain";
        Response.Charset = "utf-8";

        string isAutoNum = Request.Form["isAutoNum"];
        string UpLoadFolder = Request.Form["UpLoadFolder"];
        string filter = Request.Form["filter"];
        string fileSizeLimited = Request.Form["fileSizeLimited"];
        char[] charter = new char[] { '|' };
        string[] filterlist = filter.Split(charter, StringSplitOptions.RemoveEmptyEntries);
        EFClientTools.ClientUtility.ClientInfo.cErrorCode = 1280;
        if (Request.Files.AllKeys.Length == 0)
        {
            if (Request.QueryString["type"] != null && Request.QueryString["type"].ToLower() == "delete")
            {
                EFClientTools.ClientUtility.ClientInfo.cErrorCode = 1281;
                var value = Request.Form["value"];
                var virtualpath = "~/" + (UpLoadFolder != "" ? UpLoadFolder + "/" : "") + value;
                var path = Server.MapPath(virtualpath);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                    Response.Write(ToJson("ok", path));
                }
            }
            else
            {
                EFClientTools.ClientUtility.ClientInfo.cErrorCode = 1282;

                var messageKey = "JQWebClient/nofile";
                var locale = context.Request.UserLanguages.Length > 0 ? context.Request.UserLanguages[0] : "en-us";
                EFBase.MessageProvider provider = new EFBase.MessageProvider(context.Request.PhysicalApplicationPath, locale);

                Response.Write(ToJson("error", string.Format(provider[messageKey])));
            }
        }
        else
        {
            for (int i = 0; i < Request.Files.AllKeys.Length; i++)
            {
                HttpPostedFile f = Request.Files[i];//获取上传的文件
                try
                {
                    EFClientTools.ClientUtility.ClientInfo.cErrorCode = 1283;
                    var filename = System.IO.Path.GetFileName(f.FileName);
                    if (filename.Contains(" "))
                        filename = filename.Replace(" ", "__");
                    var size = f.InputStream.Length;
                    bool sizelimit = true;
                    long limit = 0;
                    if (fileSizeLimited != null && fileSizeLimited != "" && fileSizeLimited != "0")
                    {
                        limit = (long)double.Parse(fileSizeLimited);
                        if (limit * 1024 < size)
                        {
                            sizelimit = false;
                        }
                    }
                    if (sizelimit)
                    {
                        var virtualDirectoryPath = "~/" + (UpLoadFolder != "" ? UpLoadFolder + "/" : "");
                        var directoryPath = Server.MapPath(virtualDirectoryPath);
                        if (!System.IO.Directory.Exists(directoryPath))
                            System.IO.Directory.CreateDirectory(directoryPath);
                        var virtualpath = "~/" + (UpLoadFolder != "" ? UpLoadFolder + "/" : "") + filename;
                        var path = Server.MapPath(virtualpath);
                        var ext = System.IO.Path.GetExtension(f.FileName);
                        if (ext.StartsWith("."))
                            ext = ext.Substring(1);
                        bool extexist = false;
                        if (filter == "")
                            extexist = true;
                        foreach (var filtereach in filterlist)
                        {
                            if (ext.ToLower() == filtereach.ToLower())
                            {
                                extexist = true;
                                break;
                            }
                        }
                        if (!extexist)
                        {
                            EFClientTools.ClientUtility.ClientInfo.cErrorCode = 1284;
                            var messageKey = "JQWebClient/uploadextension";
                            var locale = context.Request.UserLanguages.Length > 0 ? context.Request.UserLanguages[0] : "en-us";
                            EFBase.MessageProvider provider = new EFBase.MessageProvider(context.Request.PhysicalApplicationPath, locale);

                            Response.Write(ToJson("error", string.Format(provider[messageKey])));
                        }
                        else
                        {
                            EFClientTools.ClientUtility.ClientInfo.cErrorCode = 1285;
                            if (isAutoNum.ToLower() == "true")
                            {
                                int count = 1;
                                while (System.IO.File.Exists(path))
                                {
                                    filename = System.IO.Path.GetFileNameWithoutExtension(f.FileName) + count.ToString() + System.IO.Path.GetExtension(f.FileName);
                                    virtualpath = "~/" + (UpLoadFolder != "" ? UpLoadFolder + "/" : "") + filename;
                                    path = Server.MapPath(virtualpath);
                                    count++;
                                }
                            }
                            f.SaveAs(path);//如果要保存到其他地方，注意修改这里
                            //调用父过程更新内容，注意要对des变量进行js转义替换，防止字符串不闭合提示错误

                            Response.Write(ToJson("success", filename, size));
                        }
                    }
                    else
                    {
                        var messageKey = "JQWebClient/uploadsizelimit";
                        var locale = context.Request.UserLanguages.Length > 0 ? context.Request.UserLanguages[0] : "en-us";
                        EFBase.MessageProvider provider = new EFBase.MessageProvider(context.Request.PhysicalApplicationPath, locale);

                        Response.Write(ToJson("error", string.Format(provider[messageKey], fileSizeLimited.ToString())));
                    }
                }
                catch (Exception e)
                {
                    Response.Write(ToJson("error", e.Message.Replace(@"\", @"\\")));//如果保存失败，输出js提示保存失败
                }
            }
        }
        EFClientTools.ClientUtility.ClientInfo.cErrorCode = 0;
    }
    public string ToJson(string s, string message)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append("{\"result\":\"");
        sb.Append(s);
        sb.Append("\",\"message\":\"");
        sb.Append(message);
        sb.Append("\"}");
        return sb.ToString();
    }
    public string ToJson(string s, string message, long size)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append("{\"result\":\"");
        sb.Append(s);
        sb.Append("\",\"size\":\"");
        sb.Append(size.ToString());
        sb.Append("\",\"message\":\"");
        sb.Append(message);
        sb.Append("\"}");
        return sb.ToString();
    }

    //private string Js(string v)
    //{
    //    此函数进行js的转义替换的，防止字符串中输入了'后造成回调输出的js中字符串不闭合
    //    if (v == null) return "";
    //    return v.Replace("'", @"\'");
    //}
    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}