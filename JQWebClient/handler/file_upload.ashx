<%@ WebHandler Language="C#" Class="file_upload" %>

using System;
using System.Web;
using System.IO;
using System.Drawing;

public class file_upload : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
        if (context.Request.Files.Count > 0)
        {
            try
            {
                if (context.Request.Form["type"] == "flow")
                {
                    if (context.Request.Files.Count > 0)
                    {
                        string path = context.Server.MapPath("../WorkflowFiles");

                        var fileName = System.DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".jpg";
                        context.Request.Files[0].SaveAs(System.IO.Path.Combine(path, fileName));
                        context.Response.Write(fileName);
                    }
                }
                else
                {
                    for (int i = 0; i < context.Request.Files.Count; i++)
                    {
                        HttpPostedFile file = context.Request.Files[i];
                        int limited = int.MaxValue;
                        if (!String.IsNullOrEmpty(context.Request.QueryString["limited"]))
                            limited = Convert.ToInt32(context.Request.QueryString["limited"]);
                        if (file.ContentLength > limited * 1000)
                        {
                            context.Response.Write("{'ret':'1', 'message':'File size is over limited.'}");
                        }
                        else
                        {
                            String folder = context.Request.QueryString["folder"];
                            string path = context.Server.MapPath("../" + folder);
                            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                            var fullName = System.DateTime.Now.ToString("yyyyMMddHHmmss--") + Path.GetFileName(file.FileName) + ".jpg";
                            file.SaveAs(Path.Combine(path, fullName));
                            //string ok = "{\"ret\":\"Done!\"}";
                            //Bitmap bm = new Bitmap(fullName);
                            //Size newSize = new Size(200, 200);
                            //Bitmap newBm = GetImageThumb(bm, newSize);
                            //newBm.Save(fullName);
                            context.Response.Write("{'ret':'0', 'message':'" + fullName + "'}");
                        }
                    }


                    //for (int i = 0; i < context.Request.Files.Count; i++)
                    //{
                    //    HttpPostedFile file = context.Request.Files[i];
                    //    string path = context.Server.MapPath("../upload_files");
                    //    file.SaveAs(path + "\\" + System.DateTime.Now.ToString("yyyyMMddHHmmss--") + Path.GetFileName(file.FileName));
                    //}
                    //string ok = "{\"ret\":\"Done!\"}";
                    //context.Response.Write(ok);
                }
            }
            catch (Exception e)
            {
                string noGood = "{\"ret\":\"" + e.Message + "\"}";
                context.Response.Write(noGood);
            }

        }
        else
        {
            string noGood = "{\"ret\":\"Fail!\"}";
            context.Response.Write(noGood);
        }
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

    public Bitmap GetImageThumb(Bitmap mg, Size newSize)
    {
        double ratio = 0d;
        double myThumbWidth = 0d;
        double myThumbHeight = 0d;
        int x = 0;
        int y = 0;

        Bitmap bp;

        if ((mg.Width / Convert.ToDouble(newSize.Width)) > (mg.Height /
        Convert.ToDouble(newSize.Height)))
            ratio = Convert.ToDouble(mg.Width) / Convert.ToDouble(newSize.Width);
        else
            ratio = Convert.ToDouble(mg.Height) / Convert.ToDouble(newSize.Height);
        myThumbHeight = Math.Ceiling(mg.Height / ratio);
        myThumbWidth = Math.Ceiling(mg.Width / ratio);

        Size thumbSize = new Size((int)newSize.Width, (int)newSize.Height);
        bp = new Bitmap(newSize.Width, newSize.Height);
        x = (newSize.Width - thumbSize.Width) / 2;
        y = (newSize.Height - thumbSize.Height);
        System.Drawing.Graphics g = Graphics.FromImage(bp);
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
        g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
        Rectangle rect = new Rectangle(x, y, thumbSize.Width, thumbSize.Height);
        g.DrawImage(mg, rect, 0, 0, mg.Width, mg.Height, GraphicsUnit.Pixel);

        return bp;
    }
}