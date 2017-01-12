using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Srvtools;

public partial class SingleSignOn : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //string url = Request.Params["RedirectUrl"];                               //joy 2009/12/30 mark
        //這個寫法在 RedirectUrl 裡面有兩個以上參數時會有問題,第一個參數以後的資料都會截掉,因為Request會誤認為&之後是其他參數
        //所以這裡改抓整個URL,再自己去抓 RedirectUrl 後面的完整 url
        string strFullUrl = Request.Url.ToString();                                 //joy 2009/12/30 modify
        string url = strFullUrl.Substring(strFullUrl.IndexOf("RedirectUrl") + 12);  //joy 2009/12/30 modify
        string key = Request.Params["PublicKey"];

        string[] ss = key.Split(":".ToCharArray());//matida mark
        //string[] ss = new string[] { key, key };     //matida add
        string k = ss[0];

        if (PublicKey.CheckPublicKey2(k) && url.Length != 0)//matida modify : CheckPublicKey() -> CheckPublicKey2()   
        {
            CliUtils.LoadLoginServiceConfig(string.Format("{0}\\EEPWebClient.exe.config", EEPRegistry.WebClient));
            string message = "";
            if (CliUtils.Register(ref message) == false)
            {
                throw new Exception(message);
            }
            string[] ss1 = System.Text.RegularExpressions.Regex.Split(ss[0], PublicKey.SPLIT_STRING);
            //string[] ss1 = ss[0].Split("-".ToCharArray());
            CliUtils.fClientSystem = "Web";
            CliUtils.fLoginUser = ss1[0];
            CliUtils.fLoginDB = ss1[1];
            CliUtils.fCurrentProject = ss1[2];
            CliUtils.fComputerIp = Request.UserHostAddress;
            CliUtils.fComputerName = Request.UserHostName;
            string[] langs = Request.UserLanguages;
            if (string.Compare(langs[0], "zh-cn", true) == 0)//IgnoreCase
            {
                CliUtils.fClientLang = SYS_LANGUAGE.SIM;
            }
            else if (string.Compare(langs[0].ToLower(), "zh-tw", true) == 0)//IgnoreCase
            {
                CliUtils.fClientLang = SYS_LANGUAGE.TRA;
            }
            else
            {
                CliUtils.fClientLang = SYS_LANGUAGE.ENG;
            }

            string[] ss2 = ss[1].Split("-".ToCharArray());
            CliUtils.fUserName = HttpUtility.UrlDecode(ss2[0]);
            if (ss2.Length == 2)
            {
                CliUtils.fGroupID = ss2[1];
            }

            Response.Redirect(url);
        }
        else
        {
            throw new Exception("PublishKey is error or url is null.");

        }
    }
}
