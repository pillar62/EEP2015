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

public partial class InnerPages_frmWebHyperLinkDialog : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string url = this.Request.QueryString["WebHyperLinkURL"];
            string text = this.Request.QueryString["WebHyperLinkText"];
            text = HttpUtility.UrlEncode(text).Replace("'", "\\'");
            string param = this.Request.QueryString["itemparam"];
            param = HttpUtility.UrlEncode(param).Replace("'", "\\'");
            this.Response.Write("<script>var pagepath='" + url + "?WebHyperLinkText=" + text + "&itemparam=" + param + "';</script>");
        }
    }
}
