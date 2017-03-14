using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using Srvtools;

public partial class Template_JQueryQuery1 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public override void ProcessRequest(HttpContext context)
    {
        if (context.Request.Form["mode"] == "getFile")   //步驟1中定義的值
        {
            ButtonDownloadFile("C:\\EEP2015\\JQWebClient\\download\\334.20170310");
        }
        else if (!JqHttpHandler.ProcessRequest(context))
        {
            base.ProcessRequest(context);
        }
    }

    protected void ButtonDownloadFile(string xFile)
    {

        /*
        string srv_file = "C:\\EEP2015\\JQWebClient\\download\\334.20170310";
        string cli_file = "C:\\334.20170310";
        if (File.Exists(srv_file))
            CliUtils.DownLoad(srv_file, cli_file);
        */

        /*
        string filename = "C:\\EEP2015\\JQWebClient\\download\\334.20170310";
        string saveFileName = "c:\\MyFolder\\334.20170310";
        int intStart = filename.LastIndexOf("\\") + 1;
        saveFileName = filename.Substring(intStart, filename.Length - intStart);


        System.IO.FileInfo fi = new System.IO.FileInfo(filename);
        string fileextname = fi.Extension;
        string DEFAULT_CONTENT_TYPE = "application/unknow";
        Microsoft.Win32.RegistryKey regkey, fileextkey;
        string filecontenttype;
        try
        {
            regkey = Microsoft.Win32.Registry.ClassesRoot;
            fileextkey = regkey.OpenSubKey(fileextname);
            filecontenttype = fileextkey.GetValue("Content Type", DEFAULT_CONTENT_TYPE).ToString();
        }
        catch
        {
            filecontenttype = DEFAULT_CONTENT_TYPE;
        }
        Response.Clear();
        Response.Charset = "utf-8";
        Response.Buffer = true;
        this.EnableViewState = false;
        Response.ContentEncoding = System.Text.Encoding.UTF8;
        Response.AppendHeader("Content-Disposition", "attachment;filename=" + saveFileName);
        Response.ContentType = filecontenttype;
        Response.WriteFile(filename);
        Response.Flush();
        Response.Close();
        Response.End();
        */

        /*
        //用戶端的物件
        string fileUrlPath = "C:\\EEP2015\\JQWebClient\\download\\334.20170310";
        //string cli_file = "C:\\334.20170310";
        System.Net.WebClient wc = new System.Net.WebClient();
        byte[] file = null;

        try
        {
            //用戶端下載檔案到byte陣列
            file = wc.DownloadData(fileUrlPath);
        }
        catch (Exception ex)
        {
            HttpContext.Current.Response.Write("ASP.net禁止下載此敏感檔案(通常為：.cs、.vb、微軟資料庫mdb、mdf和config組態檔等)。<br/>檔案路徑：" + fileUrlPath + "<br/>錯誤訊息：" + ex.ToString());
            return;
        }

        HttpContext.Current.Response.Clear();
        string fileName = System.IO.Path.GetFileName(fileUrlPath);
        //跳出視窗，讓用戶端選擇要儲存的地方                         //使用Server.UrlEncode()編碼中文字才不會下載時，檔名為亂碼
        HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + HttpContext.Current.Server.UrlEncode(fileName));
        //設定MIME類型為二進位檔案
        HttpContext.Current.Response.ContentType = "application/octet-stream";

        try
        {
            //檔案有各式各樣，所以用BinaryWrite
            HttpContext.Current.Response.BinaryWrite(file);

        }
        catch (Exception ex)
        {
            HttpContext.Current.Response.Write("檔案輸出有誤，您可以在瀏覽器的URL網址貼上以下路徑嘗試看看。<br/>檔案路徑：" + fileUrlPath + "<br/>錯誤訊息：" + ex.ToString());
            return;
        }

        //這是專門寫文字的
        //HttpContext.Current.Response.Write();
        HttpContext.Current.Response.End();
        */


        var out_file = xFile;
        if (File.Exists(xFile))
        {
            try
            {
                FileInfo xpath_file = new FileInfo(xFile);  //要 using System.IO;
                                                            // 將傳入的檔名以 FileInfo 來進行解析（只以字串無法做）
                System.Web.HttpContext.Current.Response.Clear(); //清除buffer
                System.Web.HttpContext.Current.Response.ClearHeaders(); //清除buffer 表頭
                System.Web.HttpContext.Current.Response.Buffer = false;
                System.Web.HttpContext.Current.Response.ContentType = "text/plain";
                // 檔案類型還有下列幾種"application/pdf"、"application/vnd.ms-excel"、"text/xml"、"text/HTML"、"image/JPEG"、"image/GIF"
                System.Web.HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode(out_file, System.Text.Encoding.UTF8));
                // 考慮 utf-8 檔名問題，以 out_file 設定另存的檔名
                System.Web.HttpContext.Current.Response.AppendHeader("Content-Length", xpath_file.Length.ToString()); //表頭加入檔案大小
                System.Web.HttpContext.Current.Response.WriteFile(xpath_file.FullName);

                // 將檔案輸出
                System.Web.HttpContext.Current.Response.Flush();
                // 強制 Flush buffer 內容
                System.Web.HttpContext.Current.Response.End();
                //return true;

            }
            catch (Exception)
            { 
                //return false; 
            }
        }
        else
        {
                //return false;
        }
    }
}