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
using System.IO;
using System.Xml;
using System.Diagnostics;
using Srvtools;
using System.Runtime.Remoting;
using Microsoft.Win32;

public partial class WebSingleSignOn : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Register())
        {
            return;
        }
        String arg = CliUtils.fLoginUser + ";" + CliUtils.fLoginPassword + ";" + CliUtils.fLoginDB + ";" + CliUtils.fCurrentProject
                        + "!" + this.Page.Request.QueryString["Package"] + ";" + this.Page.Request.QueryString["Form"] + "!W";
        this.Page.Response.Write("<script>window.open(\"WebSingleSignOn:" + arg + "\",\"main\")</script>");
    }

    private bool Register()
    {
        string message = "";
        bool rtn = CliUtils.Register(ref message);
        if (rtn)
        {
            CliUtils.GetSysXml(string.Format("{0}\\sysmsg.xml", EEPRegistry.WebClient));
        }
        else
        {
            this.Page.Response.Write("<script>alert(\"" + message + "\");</script>");
        }

        return rtn;
    }
}