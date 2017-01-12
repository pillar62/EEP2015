using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;  
using System.Configuration;
using System.Text;

public partial class InnerPages_EEPSingleSignOn : System.Web.UI.Page
{
    public const string URL = "http://127.0.0.1:2984/EEPWebClient";

    protected void Page_Load(object sender, EventArgs e)
    {
        var clientInfo = EFClientTools.ClientUtility.ClientInfo;
        if (Request.QueryString["Type"] != null && Request.QueryString["Type"] == "win")
        {
            var redirectUrl = string.Format("WebSingleSignOn:{0};{1};{2};{3}!{4};{5}!W", clientInfo.UserID, clientInfo.Password, clientInfo.Database, clientInfo.Solution
                , this.Request.QueryString["Package"], this.Request.QueryString["Form"]);
            Page.ClientScript.RegisterStartupScript(typeof(string), "", "window.open('" + redirectUrl + "')", true);
        }
        else
        {
            var redirectUrl = string.Empty;
            var serviceUrl = ConfigurationManager.AppSettings["EEPWebClient"];
            if (Request.QueryString["Type"] != null && Request.QueryString["Type"] == "flowtodo")
            {
                
                var itemValues = new List<string>();
                itemValues.Add(Request.QueryString["LISTID"]);//LISTID 0
                itemValues.Add(HttpUtility.UrlEncode(Request.QueryString["FLOWPATH"]));//FLOWPATH 1
                itemValues.Add(HttpUtility.UrlEncode(Request.QueryString["FORM_PRESENTATION"]));//WHERESTRING 2
                itemValues.Add(Request.QueryString["NAVIGATOR_MODE"]);//NAVMODE 3
                itemValues.Add(Request.QueryString["FLNAVIGATOR_MODE"]);//FLNAVMODE 4
                itemValues.Add(Request.QueryString["FLOWIMPORTANT"]);//ISIMPORTANT 5
                itemValues.Add(Request.QueryString["FLOWURGENT"]);//ISURGENT 6
                itemValues.Add(Request.QueryString["STATUS"]);//STATUS 7
                itemValues.Add(Request.QueryString["PLUSAPPROVE"]);//PLUSAPPROVE 8
                itemValues.Add(Request.QueryString["MULTISTEPRETURN"]);//MULTISTEPRETURN 9
                itemValues.Add(HttpUtility.UrlEncode(Request.QueryString["ATTACHMENTS"]));//ATTACHMENTS 10
                itemValues.Add(HttpUtility.UrlEncode(Request.QueryString["SENDTO_ID"]));//SENDTOID 11
                itemValues.Add("");//PARAMETERS 12
                itemValues.Add(Request.QueryString["VDSNAME"]);//VDSNAME 13

                var param = string.Format("LISTID={0}&FLOWPATH={1}&WHERESTRING={2}&NAVMODE={3}&FLNAVMODE={4}&ISIMPORTANT={5}&ISURGENT={6}"
                    + "&STATUS={7}&PLUSAPPROVE={8}&MULTISTEPRETURN={9}&ATTACHMENTS={10}&SENDTOID={11}{12}&VDSNAME={13}", itemValues.ToArray());

                redirectUrl = string.Format("{0}/{1}.aspx?{2}", this.Request.QueryString["Package"], this.Request.QueryString["Form"], param);
                //Page.ClientScript.RegisterStartupScript(typeof(string), string.Empty
                //    , string.Format("singleSignOn('{0}','{1}','{2}','{3}','{4}','{5}');", serviceUrl, redirectUrl, clientInfo.UserID, clientInfo.Password, clientInfo.Database, clientInfo.Solution)
                //    , true);
            }
            else if (Request.QueryString["Type"] != null && Request.QueryString["Type"] == "flowtohis")
            {
                var itemValues = new List<string>();
                itemValues.Add(Request.QueryString["LISTID"]);//LISTID 0
                itemValues.Add(HttpUtility.UrlEncode(Request.QueryString["FLOWPATH"]));//FLOWPATH 1
                itemValues.Add(HttpUtility.UrlEncode(Request.QueryString["FORM_PRESENTATION"]));//WHERESTRING 2
                itemValues.Add("0");//NAVMODE 3
                itemValues.Add("6");//FLNAVMODE 4
                itemValues.Add(HttpUtility.UrlEncode(Request.QueryString["ATTACHMENTS"]));//ATTACHMENTS 5
                itemValues.Add("");//PARAMETERS 6
                itemValues.Add(Request.QueryString["VDSNAME"]);//VDSNAME 7

                var param = string.Format("LISTID={0}&FLOWPATH={1}&WHERESTRING={2}&NAVMODE={3}&FLNAVMODE={4}&ATTACHMENTS={5}{6}&VDSNAME={7}", itemValues.ToArray());

                redirectUrl = string.Format("{0}/{1}.aspx?{2}", this.Request.QueryString["Package"], this.Request.QueryString["Form"], param);
                //Page.ClientScript.RegisterStartupScript(typeof(string), string.Empty
                //    , string.Format("singleSignOn('{0}','{1}','{2}','{3}','{4}','{5}');", serviceUrl, redirectUrl, clientInfo.UserID, clientInfo.Password, clientInfo.Database, clientInfo.Solution)
                //    , true);
            }
            else
            {
                redirectUrl = string.Format("{0}/{1}.aspx?ItemParam={2}", this.Request.QueryString["Package"], this.Request.QueryString["Form"], this.Request.QueryString["ItemParam"]);
                //Page.ClientScript.RegisterStartupScript(typeof(string), string.Empty
                //    , string.Format("singleSignOn('{0}','{1}','{2}','{3}','{4}','{5}');", serviceUrl, redirectUrl, clientInfo.UserID, clientInfo.Password, clientInfo.Database, clientInfo.Solution)
                //    , true);
            }
            //A3-Cross-Site Scripting (XSS)
            Page.ClientScript.RegisterStartupScript(typeof(string), string.Empty
                   , string.Format("singleSignOn('{0}','{1}','{2}','{3}','{4}','{5}');", MarkValue(serviceUrl), MarkValue(redirectUrl), MarkValue(clientInfo.UserID), MarkValue(clientInfo.Password), MarkValue(clientInfo.Database), MarkValue(clientInfo.Solution))
                   , true);
        }
    }

    private string MarkValue(string value)
    {
        if (!string.IsNullOrEmpty(value)) {
            value = value.Replace("'", "''");
        }
        return value;
    }
}