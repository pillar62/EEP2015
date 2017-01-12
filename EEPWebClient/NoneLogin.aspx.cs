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
using System.Runtime.Remoting;
using Microsoft.Win32;
using System.Xml;
using System.IO;
using System.Threading;

public partial class NoneLogin : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            string[] langs = Request.UserLanguages;
            try
            {
                if (string.Compare(langs[0], "zh-cn", true) == 0)//IgnoreCase
                {
                    CliUtils.fClientLang = SYS_LANGUAGE.SIM;
                }
                else if (string.Compare(langs[0], "zh-tw", true) == 0)//IgnoreCase
                {
                    CliUtils.fClientLang = SYS_LANGUAGE.TRA;
                }
                else
                {
                    CliUtils.fClientLang = SYS_LANGUAGE.ENG;
                }
            }
            catch
            {
                CliUtils.fClientLang = SYS_LANGUAGE.ENG;
            }

            string strPath = Request.Path;
            strPath = Request.MapPath(strPath);
            strPath = strPath.Substring(0, strPath.LastIndexOf('\\') + 1);
            //if (System.Runtime.Remoting.Channels.ChannelServices.RegisteredChannels.Length == 0)
            //{
            //    RemotingConfiguration.Configure(strPath + "EEPWebClient.exe.config", true);
            //}
            CliUtils.LoadLoginServiceConfig(strPath + "EEPWebClient.exe.config");

            if (!Register())
            {
                return;
            }

            CliUtils.fClientSystem = "Web";
            CliUtils.fLoginUser = "001";
            CliUtils.fLoginPassword = "";
            CliUtils.fLoginDB = "ERPS";
            CliUtils.fCurrentProject = "Solution1";

            string sParam = CliUtils.fLoginUser + ':' + CliUtils.fLoginPassword + ':' + CliUtils.fLoginDB + ':' + "0";
            object[] myRet = CliUtils.CallMethod("GLModule", "CheckUser", new object[] { (object)sParam });
            LoginResult result = (LoginResult)myRet[1];
            if (result == LoginResult.PasswordError)
            {
                this.FailureText.Text = "User or Password is error.";
            }
            else if (result == LoginResult.UserNotFound)
            {
                this.FailureText.Text = "User Not Found.";
            }
            else if (result == LoginResult.Disabled)
            {
                this.FailureText.Text = "User has been disabled.";
            }
            else//sucessed to login....
            {
                CliUtils.fUserName = myRet[2].ToString();
                myRet = CliUtils.CallMethod("GLModule", "GetUserGroup", new object[] { CliUtils.fLoginUser });
                if (myRet != null && (int)myRet[0] == 0)
                {
                    CliUtils.fGroupID = myRet[1].ToString();
                }
                Response.Redirect("webClientMain.aspx", true);
            }
        }
    }
    private bool Register()
    {
        string message = "";
        bool rtn = CliUtils.Register(ref message);
        if (rtn)
        {
            CliUtils.GetSysXml(string.Format("{0}\\sysmsg.xml", EEPRegistry.WebClient));
            CliUtils.GetPasswordPolicy();
        }
        else
        {
            this.FailureText.Text = message;
        }
        return rtn;
    }
}
