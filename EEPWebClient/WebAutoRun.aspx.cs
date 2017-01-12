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
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Text;
using System.Runtime.Remoting;
using Srvtools;

public partial class WebAutoRun : System.Web.UI.Page
{
    private SYS_LANGUAGE language;
    
    protected void Button1_Click(object sender, EventArgs e)
    {
        int count = getInt(txtUser.Text);
        int Modules = getInt(txtPackage.Text);
        if (Modules == 0) return;
        int interval = getInt(txtInterval.Text);
        string LogFile = txtLog.Text;
        string strPath = Request.Path;
        string str = strPath.Substring(0, strPath.LastIndexOf('/') + 1);
        strPath = Request.MapPath(strPath);
        if (txtChooseXML.Text == null || txtChooseXML.Text == "")
            strPath = strPath.Substring(0, strPath.LastIndexOf('\\') + 1) + "WebAutoRun.xml";
        else
            strPath = txtChooseXML.Text.ToString();
        int maxl = 0, maxp = 0;
        int countl = 0, coutp = 0;
        if (File.Exists(strPath))
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(strPath);
            XmlNode el = xml.DocumentElement;
            foreach (XmlNode xNode in el.ChildNodes)
            {
                if (string.Compare(xNode.Name, "LOGININFORMATION", true) == 0)//IgnoreCase
                {
                    countl++;
                }
            }
        }

        maxl = (count < countl) ? count : countl;
        string[] userID = new string[maxl];
        string[] password = new string[maxl];
        string[] db = new string[maxl];
        string[] solution = new string[maxl];
        string[] serverIPaddress = new string[maxl];
        string[] userMessage = new string[maxl];
        if (File.Exists(strPath))
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(strPath);
            XmlNode el = xml.DocumentElement;
            foreach (XmlNode xNode in el.ChildNodes)
            {
                if (string.Compare(xNode.Name, "PACKAGEINFORMATION", true) == 0)//IgnoreCase
                {
                    coutp++;
                }
            }
        }
        maxp = (Modules < coutp) ? Modules : coutp;
        string[] packageName = new string[maxp];
        string[] formName = new string[maxp];
        string[] times = new string[maxp];
        string[] packageMessage = new string[maxp];
        if (File.Exists(strPath))
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(strPath);
            XmlNode el = xml.DocumentElement;
            int indexl = 0, indexp = 0;

            foreach (XmlNode xNode in el.ChildNodes)
            {
                if (string.Compare(xNode.Name, "LOGININFORMATION", true) == 0)//IgnoreCase
                {
                    foreach (XmlNode xNodel in xNode)
                    {
                        if (indexl >= maxl)
                            break;
                        if (string.Compare(xNodel.Name, "USERID", true) == 0)//IgnoreCase
                        {
                            userID[indexl] = xNodel.InnerText.Trim();
                        }
                        else if (string.Compare(xNodel.Name, "PASSWORD", true) == 0)//IgnoreCase
                        {
                            password[indexl] = xNodel.InnerText.Trim();
                        }
                        else if (string.Compare(xNodel.Name, "DATABASE", true) == 0)//IgnoreCase
                        {
                            db[indexl] = xNodel.InnerText.Trim();
                        }
                        else if (string.Compare(xNodel.Name, "SOLUTION", true) == 0)//IgnoreCase
                        {
                            solution[indexl] = xNodel.InnerText.Trim();
                        }
                        else if (string.Compare(xNodel.Name, "SERVERIPADDRESS", true) == 0)//IgnoreCase
                        {
                            serverIPaddress[indexl] = xNodel.InnerText.Trim();
                        }
                        userMessage[indexl] = userID[indexl] + ";" + password[indexl] + ";" + db[indexl] + ";" + solution[indexl];
                    }
                    indexl++;
                }
                if (string.Compare(xNode.Name, "PACKAGEINFORMATION", true) == 0)//IgnoreCase
                {
                    foreach (XmlNode xNodel in xNode)
                    {
                        if (indexp >= maxp)
                            break;
                        if (string.Compare(xNodel.Name, "PACKAGENAME", true) == 0)//IgnoreCase
                        {
                            packageName[indexp] = xNodel.InnerText.Trim();
                        }
                        else if (string.Compare(xNodel.Name, "FORMNAME", true) == 0)//IgnoreCase
                        {
                            formName[indexp] = xNodel.InnerText.Trim();
                        }
                        else if (string.Compare(xNodel.Name, "TIMES", true) == 0)//IgnoreCase
                        {
                            times[indexp] = xNodel.InnerText.Trim();
                        }
                        packageMessage[indexp] = packageName[indexp] + ";" + formName[indexp] + ";" + times[indexp];
                    }
                    indexp++;
                }
            }
        }
        string package = null;
        for (int i = 0; i < packageMessage.Length; i++)
        {
            if (i < packageMessage.Length - 1)
                package += packageMessage[i] + ",";
            else
                package += packageMessage[i];
        }

        StringBuilder Path = new StringBuilder(Request.Url.ToString());
        String p = "";
        int x = 0;
        for (int i = 0; i < Path.Length; i++)
        {
            if (Path[i] == '/')
                x++;
            if (x == 4)
                Path.Remove(i, Path.Length - i);
        }
        p = Path.ToString().Substring(Path.ToString().LastIndexOf('/'));
        for (int i = 0; i < maxl; i++)
        {
            Page.Response.Write("<script>alert(\"" + Path + "\")</script>");
            Page.Response.Write("<script>window.open(\"" + Path + "/WebAutoRunStep.aspx?usermessage=" + userMessage[i] +
                                           "&packagemessage=" + package + "&Interval=" + interval + "&Log=" + LogFile + "&Path=" + Path + "\");</script>");
            //Process.Start("IExplore.exe", Path + "/WebAutoRunStep.aspx?usermessage=" + userMessage[i] +
            //                               "&packagemessage=" + package + "&Interval=" + interval + "&Log=" + LogFile + "&Path=" + Path);
        }
    }
    
    public int getInt(string str)
    {
        int x = 0;
        for (int i = 0; i < str.Length; i++)
        {
            int temp = 0;
            switch (str[i])
            {
                case '0': temp = 0; break;
                case '1': temp = 1; break;
                case '2': temp = 2; break;
                case '3': temp = 3; break;
                case '4': temp = 4; break;
                case '5': temp = 5; break;
                case '6': temp = 6; break;
                case '7': temp = 7; break;
                case '8': temp = 8; break;
                case '9': temp = 9; break;
            }
            x = x + temp * (int)Math.Pow(10, (str.Length - i - 1));
        }
        return x;
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        string[] langs = Request.UserLanguages;
        if (langs.Length > 0)
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
        else
        {
            CliUtils.fClientLang = SYS_LANGUAGE.ENG;
        }
        string message = SysMsg.GetSystemMessage(Srvtools.CliUtils.fClientLang, "Srvtools", "EEPNetAutoRun", "LabelName", true);
        string[] user = message.Split(';');
        this.lblUsers.Text = user[0];
        this.lblModules.Text = user[1];
        this.lblInterval.Text = user[2];
        this.lblLog.Text = user[3];
        this.labelChooseXML.Text = user[4];

        string strPath = Request.Path;
        strPath = Request.MapPath(strPath);
        strPath = strPath.Substring(0, strPath.LastIndexOf('\\') + 1);
        if (System.Runtime.Remoting.Channels.ChannelServices.RegisteredChannels.Length == 0)
            RemotingConfiguration.Configure(strPath + "WebAutoRun.exe.config", true);
    }
}