#define winxp

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
using System.Xml;
using System.IO;
using Srvtools;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Reflection;
using System.Net;
using System.Diagnostics;
using System.Runtime.Remoting;
using System.Text;
using System.Threading;
using System.Text.RegularExpressions;

public partial class WebAutoLoginStep : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //#if winxp
        string strPath = Request.Path;
        strPath = Request.MapPath(strPath);
        strPath = strPath.Substring(0, strPath.LastIndexOf('\\') + 1);
        CliUtils.LoadLoginServiceConfig(strPath + "WebAutoLogin.exe.config");
        CliUtils.fClientSystem = "Web";
        CliUtils.fClientLang = CliSysMegLag.GetClientLanguage();
        //#endif
        if (!Register())
        {
            return;
        }

        if (this.Page.Request.QueryString["usermessage"] != "" && this.Page.Request.QueryString["usermessage"] != null)
        {
            string[] user = this.Page.Request.QueryString["usermessage"].Split(';');
            CliUtils.fLoginUser = user[0];
            CliUtils.fLoginPassword = user[1];
            CliUtils.fLoginDB = user[2];
            CliUtils.fCurrentProject = user[3];

            CliUtils.fComputerIp = Request.UserHostAddress;
            CliUtils.fComputerName = Request.UserHostName;

            string Param = user[0] + ':' + user[1] + ':' + user[2] + ":0";

            //string Param = CliUtils.fLoginUser + ':' + CliUtils.fLoginPassword + ':' + CliUtils.fLoginDB + ':' + CliUtils.fCurrentProject;
            object[] Ret = CliUtils.CallMethod("GLModule", "CheckUser", new object[] { (object)Param });
            System.Threading.Thread.Sleep(5000);
            Session.Abandon();
            Page.Response.Write("<script>window.opener=null;window.open('','_top');window.close();</script>");
        }
    }

    private bool Register()
    {
        string message = "";
        bool rtn = CliUtils.Register(ref message);
        if (rtn)
        {
            RegistryKey rk = Registry.LocalMachine.OpenSubKey("SOFTWARE\\infolight\\eep.net");
            string s = (string)rk.GetValue("WebClient Path");
            rk.Close();
            if (s[s.Length - 1] != '\\')
                s = s + "\\";

            string path = s + @"sysmsg.xml";
            CliUtils.GetSysXml(path);
        }

        return rtn;
    }
}
