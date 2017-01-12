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

public partial class Timeout : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["p"] == "true")
        {
            var count = Request.FilePath.Split('/').Length - Request.CurrentExecutionFilePath.Split('/').Length;
            string tPath = string.Empty;
            for (int i = 0; i < count; i++)
            {
                tPath += "../";
            }

            tPath += "Timeout.aspx";
            this.ClientScript.RegisterStartupScript(typeof(string), "", "window.top.location.href='"+ tPath  + "'", true);
        }

        string virPath = Request.ApplicationPath;
        string[] langs = Request.UserLanguages;
        try
        {
            if (string.Compare(langs[0], "zh-cn", true) == 0)//IgnoreCase
            {
                inhalt.Attributes.CssStyle.Value = "background-image:url('" + virPath + "/Image/main/timeout_bg_cn.png')";
            }
            else if (string.Compare(langs[0], "zh-tw", true) == 0)//IgnoreCase
            {
                inhalt.Attributes.CssStyle.Value = "background-image:url('" + virPath + "/Image/main/timeout_bg_tw.png')";
            }
            else
            {
            }
        }
        catch
        {
            inhalt.Attributes.CssStyle.Value = "background-image:url('" + virPath + "/Image/main/timeout_bg_tw.png')";
        }
        Relogin.NavigateUrl = "LogOn.aspx";
    }
}
