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

public partial class Timeout : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string virPath = Request.ApplicationPath;
        string[] langs = Request.UserLanguages;
        try
        {
            if (string.Compare(langs[0], "zh-cn", true) == 0)//IgnoreCase
            {
                CliUtils.fClientLang = SYS_LANGUAGE.SIM;
                inhalt.Attributes.CssStyle.Value = "background-image:url('" + virPath + "/Image/main/timeout_bg_cn.png')";
            }
            else if (string.Compare(langs[0], "zh-tw", true) == 0)//IgnoreCase
            {
                CliUtils.fClientLang = SYS_LANGUAGE.TRA;
                inhalt.Attributes.CssStyle.Value = "background-image:url('" + virPath + "/Image/main/timeout_bg_tw.png')";
            }
            else
            {
                CliUtils.fClientLang = SYS_LANGUAGE.ENG;
            }
        }
        catch
        {
            CliUtils.fClientLang = SYS_LANGUAGE.ENG;
            inhalt.Attributes.CssStyle.Value = "background-image:url('" + virPath + "/Image/main/timeout_bg_tw.png')";
        }

        String mu = "";
        if (Page.Request.QueryString["MU"] != null && Page.Request.QueryString["MU"] == "true")
            mu = "?IsMU=true";
        if (mu == "?IsMU=true")
            Relogin.NavigateUrl = virPath + "/Infologin.aspx" + mu;
        else
        {
            Relogin.NavigateUrl = virPath + "/Infologin.aspx";
        }
    }
}
