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

public partial class WebAutoRunStep : System.Web.UI.Page
{
    private SYS_LANGUAGE language;

    protected void Page_Load(object sender, EventArgs e)
    {
        string message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "Srvtools", "EEPNetRunStep", "LabelName", true);
        string[] users = message.Split(';');
        this.labelUserID.Text = users[0];
        this.labelPassword.Text = users[1];
        this.labelDB.Text = users[2];
        this.labelSolution.Text = users[3];
        this.labelPackageName.Text = users[4];
        this.labelFormName.Text = users[5];
        this.labelTimes.Text = users[6];

        ////this.Page.Response.Write("<script>alert(\"111\")</script>");

        //#if winxp
        string strPath = Request.Path;
        strPath = Request.MapPath(strPath);
        strPath = strPath.Substring(0, strPath.LastIndexOf('\\') + 1);
        if (System.Runtime.Remoting.Channels.ChannelServices.RegisteredChannels.Length == 0)
            RemotingConfiguration.Configure(strPath + "WebAutoRun.exe.config", true);
        CliUtils.LoadLoginServiceConfig(strPath + "WebAutoRun.exe.config");
        CliUtils.fClientSystem = "Web";
       
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

            string[] PackageMessage = this.Page.Request.QueryString["packagemessage"].Split(',');
            string[,] packages = new string[PackageMessage.Length, 4];
            for (int i = 0; i < PackageMessage.Length; i++)
            {
                string[] temp = PackageMessage[i].Split(';');
                for (int j = 0; j < 4; j++)
                    packages[i, j] = temp[j];
            }
            for (int i = 0; i < PackageMessage.Length; i++)
            {
                string PackageName = packages[i, 0];
                string FormName = packages[i, 1];
                string Times = packages[i, 2];
                string FlowFileName = packages[i, 3];
                string Interval = this.Page.Request.QueryString["Interval"];
                string sfile = PackageName + "\\" + FormName + ".aspx";

                this.Session.Timeout = Convert.ToInt16(Times) * 100;

                Session.Add("active", "true");
                Session.Add("packagename", PackageName + "/" + FormName + ".aspx");
                Session.Add("times", Times);
                Session.Add("flowfilename", FlowFileName);
                Session.Add("Interval", Interval);
                Session.Add("Log", this.Page.Request.QueryString["Log"]);
                Session.Add("userid", user[0]);

                Response.Redirect(PackageName + "/" + FormName + ".aspx", true);

                //Process.Start("IExplore.exe", this.Page.Request.QueryString["Path"] + "\\WebAutoRunTran.aspx?usermessage=" + this.Page.Request.QueryString["usermessage"] + "&packagename=" + PackageName + "/" + FormName + ".aspx"
                //                    + "&times=" + Times + "&Interval=" + Interval + "&Log=" + this.Page.Request.QueryString["Log"]);

                //Process.Start("IExplore.exe", this.Page.Request.QueryString["Path"] + "\\" + sfile + "?active=true&userid=" + user[0] + "&packagename=" + PackageName + "." + FormName
                //                                    + "&times=" + Times + "&Interval=" + Interval + "&Log=" + this.Page.Request.QueryString["Log"]);
            }
            Page.Response.Write("<script>window.opener=null;window.open('','_top');window.close();</script>");
        }
    }

    private bool Register()
    {
        string message = "";
        bool rtn = CliUtils.Register(ref message);
        if (rtn)
        {
            CliUtils.GetSysXml(string.Format("{0}\\sysmsg.xml", EEPRegistry.WebClient));
        }

        return rtn;
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        //CliUtils.fLoginUser = txtUserID.Text;
        //CliUtils.fLoginPassword = txtPassword.Text;
        //CliUtils.fLoginDB = txtDB.Text;
        //CliUtils.fCurrentProject = txtSolution.Text;
        //CliUtils.fSiteCode = "Web";
        //string sParam = CliUtils.fLoginUser + ':' + CliUtils.fLoginPassword + ':' + CliUtils.fLoginDB/* + ':' + CliUtils.fCurrentProject*/;
        //object[] myRet = CliUtils.CallMethod("GLModule", "CheckUser", new object[] { (object)sParam });

        string PackageName = txtPackageName.Text;
        string FormName = txtFormName.Text;
        string Times = txtTimes.Text;
        string sfile = PackageName + "\\" + FormName + ".aspx";

        StringBuilder Path = new StringBuilder(Request.Url.ToString());
        int x = 0;
        for (int i = 0; i < Path.Length; i++)
        {
            if (Path[i] == '/')
                x++;
            if (x == 4)
                Path.Remove(i, Path.Length - i);
        }
        Process.Start("IExplore.exe", Path + "/" + sfile + "?active=true&userid=" + txtUserID.Text + "&packagename=" + txtPackageName.Text + "." + txtFormName.Text
                                            + "&times=" + txtTimes.Text + "&Interval=100");
    }
}
