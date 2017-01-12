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

public partial class WebAutoLogin : System.Web.UI.Page
{
    private SYS_LANGUAGE language;

    protected void Button1_Click(object sender, EventArgs e)
    {
        int count = getInt(txtUser.Text);
        string strPath = Request.Path;
        string str = strPath.Substring(0, strPath.LastIndexOf('/') + 1);
        strPath = Request.MapPath(strPath);
        if (txtChooseXML.Text == null || txtChooseXML.Text == "")
            strPath = strPath.Substring(0, strPath.LastIndexOf('\\') + 1) + "WebAutoLogin.xml";
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
                if (xNode.Name.ToUpper().Equals("LOGININFORMATION"))
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
            int indexl = 0, indexp = 0;

            foreach (XmlNode xNode in el.ChildNodes)
            {
                if (xNode.Name.ToUpper().Equals("LOGININFORMATION"))
                {
                    foreach (XmlNode xNodel in xNode)
                    {
                        if (indexl >= maxl)
                            break;
                        if (xNodel.Name.ToUpper().Equals("USERID"))
                        {
                            userID[indexl] = xNodel.InnerText.Trim();
                        }
                        else if (xNodel.Name.ToUpper().Equals("PASSWORD"))
                        {
                            password[indexl] = xNodel.InnerText.Trim();
                        }
                        else if (xNodel.Name.ToUpper().Equals("DATABASE"))
                        {
                            db[indexl] = xNodel.InnerText.Trim();
                        }
                        else if (xNodel.Name.ToUpper().Equals("SOLUTION"))
                        {
                            solution[indexl] = xNodel.InnerText.Trim();
                        }
                        else if (xNodel.Name.ToUpper().Equals("SERVERIPADDRESS"))
                        {
                            serverIPaddress[indexl] = xNodel.InnerText.Trim();
                        }
                        userMessage[indexl] = userID[indexl] + ";" + password[indexl] + ";" + db[indexl] + ";" + solution[indexl];
                    }
                    indexl++;
                }
            }
        }

        StringBuilder Path = new StringBuilder(Request.Url.ToString());
        int x = 0;
        for (int i = 0; i < Path.Length; i++)
        {
            if (Path[i] == '/')
                x++;
            if (x == 4)
                Path.Remove(i, Path.Length - i);
        }
        for (int i = 0; i < maxl; i++)
        {
            Process.Start("IExplore.exe", Path + "/WebAutoLoginStep.aspx?usermessage=" + userMessage[i]);
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
        language = Srvtools.CliSysMegLag.GetClientLanguage();
        string message = SysMsg.GetSystemMessage(language, "Srvtools", "EEPNetAutoRun", "LabelName", true);
        string[] user = message.Split(';');
        this.lblUsers.Text = user[0];
        this.labelChooseXML.Text = user[4];

        string strPath = Request.Path;
        strPath = Request.MapPath(strPath);
        strPath = strPath.Substring(0, strPath.LastIndexOf('\\') + 1);
        if (System.Runtime.Remoting.Channels.ChannelServices.RegisteredChannels.Length == 0)
            RemotingConfiguration.Configure(strPath + "WebAutoLogin.exe.config", true);
    }
}