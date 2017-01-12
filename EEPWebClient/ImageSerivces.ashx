<%@ WebHandler Language="C#" Class="ImageSerivces" %>

using System;
using System.Web;

public class ImageSerivces : IHttpHandler 
{    
    public void ProcessRequest(HttpContext context)
    {
        //設定網頁類型 
        context.Response.ContentType = "image/Jpeg";

        //從 key 抓回 session 中的圖檔 
        System.Drawing.Bitmap BMP;
        string key = context.Request["ImageKey"].ToString();
        //讀取圖檔 
        BMP = context.Application[key] as System.Drawing.Bitmap;
        //釋放記憶體 
        context.Application[key] = null;

        //處理圖形品質 
        System.Drawing.Imaging.ImageCodecInfo[] codecs = System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders();
        System.Drawing.Imaging.ImageCodecInfo ici = null;
        //找出Encoder 
        foreach (System.Drawing.Imaging.ImageCodecInfo codec in codecs)
        {
            if (codec.MimeType == "image/jpeg")
            {
                ici = codec;
            }
        }
        //參數 - 高品質圖檔 
        System.Drawing.Imaging.EncoderParameters ep = new System.Drawing.Imaging.EncoderParameters();
        ep.Param[0] = new System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)100);

        if (ici == null | ep == null)
        {
            //儲存-低品質 
            BMP.Save(context.Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);
        }
        else
        {
            //儲存-高品質 
            BMP.Save(context.Response.OutputStream, ici, ep);
        }
        //釋放 
        BMP.Dispose();
        //結束 
        context.Response.End();
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}