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
using System.Runtime.Remoting;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

public partial class InfoLogin : System.Web.UI.Page
{

    //public string Solution
    //{
    //    get
    //    {
    //        return (ViewState["Solution"] == null ? "" : ViewState["Solution"].ToString());
    //    }
    //    set
    //    {
    //        ViewState["Solution"] = value;
    //    }


    //}

    protected void Page_Load(object sender, EventArgs e)
    {
        CliUtils.fClientSystem = "Web";
        CliUtils.fComputerIp = Request.UserHostAddress;
        CliUtils.fComputerName = Request.UserHostName;
        CliUtils.fSolutionSecurity = false;
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
        setButtonAndImage();
        if (!IsPostBack)
        {
            this.ViewState.Add("IsMasterPage", this.Request.QueryString["IsMasterPage"]);
            this.ViewState.Add("IsFlowPage", this.Request.QueryString["IsFlowPage"]);
            string strPath = Request.Path;
            strPath = Request.MapPath(strPath);
            strPath = strPath.Substring(0, strPath.LastIndexOf('\\') + 1);
            //#if winxp
            //            if (System.Runtime.Remoting.Channels.ChannelServices.RegisteredChannels.Length == 0)
            //            {
            //                RemotingConfiguration.Configure(strPath + "EEPWebClient.exe.config", true);
            //            }
            //#endif
            CliUtils.LoadLoginServiceConfig(strPath + "EEPWebClient.exe.config");
            if (!Register())
            {
                return;
            }
            txtUserName.Text = "001";
            loadSysDataBases();
            loadSysSolutions();
        }
        string[] caption = SysMsg.GetSystemMessage(CliUtils.fClientLang, "EEPWebNetClient", "WinSysMsg", "txt_Login", true).Split(';');
        lbUser.Text = caption[0];
        lbPassword.Text = caption[1];
        lbDataBase.Text = caption[2];
        lbSolution.Text = caption[3];
        chkRememberPwd.Text = caption[4];
        lbCheckCode.Text = caption[5];
        this.OKButton.Focus();

        String VNum = hfNValue.Value;
        String ONum = hfOValue.Value;

        ONum = VNum;
        VNum = MakeValidateCode();
        Bitmap validateimage;
        Graphics g;
        validateimage = new Bitmap(70, 20, PixelFormat.Format24bppRgb);
        g = Graphics.FromImage(validateimage);
        g.DrawString(VNum, new Font("Verdana", 15), new SolidBrush(Color.White), new PointF(8, -2));
        g.FillRectangle(new LinearGradientBrush(new Point(0, 0), new Point(110, 20), Color.FromArgb(0, 0, 0, 0), Color.FromArgb(255, 255, 60, 40)), 0, 0, 120, 30);
        g.Save();
        MemoryStream ms = new MemoryStream();
        validateimage.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
        String imagePath = this.Page.Request.ApplicationPath;
        imagePath = imagePath.Remove(imagePath.LastIndexOf('/'));
        String mapPath = this.Page.MapPath(imagePath);
        if (!Directory.Exists(mapPath + "\\InfolightTemp"))
        {
            Directory.CreateDirectory(mapPath + "\\InfolightTemp");
        }
        else
        {
            String[] files = Directory.GetFiles(mapPath + "\\InfolightTemp");
            for (int i = 0; i < files.Length; i++)
            {
                File.Delete(files[i]);
            }
        }
        mapPath += "\\InfolightTemp\\" + VNum + ".gif";
        imagePath = "~/InfolightTemp/" + VNum + ".gif";

        FileStream f = new FileStream(mapPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
        ms.WriteTo(f);
        f.Close();
        f.Dispose();
        ms.Close();
        this.Image1.ImageUrl = imagePath;

        hfNValue.Value = VNum;
        hfOValue.Value = ONum;
    }

    private void setButtonAndImage()
    {
        if (CliUtils.fClientLang == SYS_LANGUAGE.SIM)
        {
            (Page.FindControl("login_bot_bg") as System.Web.UI.WebControls.Image).ImageUrl = "~/Image/login/login_bot_bg_cn.jpg";
            (Page.FindControl("login_top") as System.Web.UI.WebControls.Image).ImageUrl = "~/Image/login/login_top_cn.jpg";
            (Page.FindControl("OKButton") as ImageButton).ImageUrl = "~/Image/Login/Login_cn.png";
            (Page.FindControl("OKButton") as ImageButton).Attributes["onmouseover"] = "this.src='Image/Login/Login_over_cn.png'";
            (Page.FindControl("OKButton") as ImageButton).Attributes["onmouseout"] = "this.src='Image/Login/Login_cn.png'";
            (Page.FindControl("CancelButton") as ImageButton).ImageUrl = "~/Image/Login/cancel_cn.png";
            (Page.FindControl("CancelButton") as ImageButton).Attributes["onmouseover"] = "this.src='Image/Login/cancel_over_cn.png'";
            (Page.FindControl("CancelButton") as ImageButton).Attributes["onmouseout"] = "this.src='Image/Login/cancel_cn.png'";
        }
        else if (CliUtils.fClientLang == SYS_LANGUAGE.TRA)
        {
            (Page.FindControl("login_bot_bg") as System.Web.UI.WebControls.Image).ImageUrl = "~/Image/login/login_bot_bg_tw.jpg";
            (Page.FindControl("OKButton") as ImageButton).ImageUrl = "~/Image/Login/Login_tw.png";
            (Page.FindControl("OKButton") as ImageButton).Attributes["onmouseover"] = "this.src='Image/Login/Login_over_tw.png'";
            (Page.FindControl("OKButton") as ImageButton).Attributes["onmouseout"] = "this.src='Image/Login/Login_tw.png'";
            (Page.FindControl("CancelButton") as ImageButton).ImageUrl = "~/Image/Login/cancel_tw.png";
            (Page.FindControl("CancelButton") as ImageButton).Attributes["onmouseover"] = "this.src='Image/Login/cancel_over_tw.png'";
            (Page.FindControl("CancelButton") as ImageButton).Attributes["onmouseout"] = "this.src='Image/Login/cancel_tw.png'";

        }
        else
        {
            (Page.FindControl("login_bot_bg") as System.Web.UI.WebControls.Image).ImageUrl = "~/Image/login/login_bot_bg_en.jpg";
            (Page.FindControl("OKButton") as ImageButton).ImageUrl = "~/Image/Login/Login_en.png";
            (Page.FindControl("OKButton") as ImageButton).Attributes["onmouseover"] = "this.src='Image/Login/Login_over_en.png'";
            (Page.FindControl("OKButton") as ImageButton).Attributes["onmouseout"] = "this.src='Image/Login/Login_en.png'";
            (Page.FindControl("CancelButton") as ImageButton).ImageUrl = "~/Image/Login/cancel_en.png";
            (Page.FindControl("CancelButton") as ImageButton).Attributes["onmouseover"] = "this.src='Image/Login/cancel_over_en.png'";
            (Page.FindControl("CancelButton") as ImageButton).Attributes["onmouseout"] = "this.src='Image/Login/cancel_en.png'";
        }
    }


    private string MakeValidateCode()
    {
        char[] s = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        string num = "";
        Random r = new Random();
        for (int i = 0; i < 4; i++)
            num += s[r.Next(0, s.Length)].ToString();
        return num;
    }

    private bool Register()
    {
        string message = "";
        bool rtn = CliUtils.Register(ref message);
        if (rtn)
        {
            string path = string.Format("{0}\\{1}", EEPRegistry.WebClient.TrimEnd('\\'), "sysmsg.xml");
            CliUtils.GetSysXml(path);
        }
        else
        {
            this.FailureText.Text = message;
        }

        return rtn;
    }

    private void loadSysDataBases()
    {
        object[] myRet1 = CliUtils.CallMethod("GLModule", "GetDB", null);
        if (myRet1[1] != null && myRet1[1] is ArrayList)
        {
            ArrayList dbList = (ArrayList)myRet1[1];
            foreach (string db in dbList)
            {
                this.ddlDataBase.Items.Add(db);
            }
        }
    }

    private void loadSysSolutions()
    {
        object[] objParam = new object[1];
        objParam[0] = this.ddlDataBase.SelectedValue;
        DataSet dsSolution = new DataSet();
        object[] myRet1 = CliUtils.CallMethod("GLModule", "GetSolution", objParam);
        if ((null != myRet1) && (0 == (int)myRet1[0]))
            dsSolution = ((DataSet)myRet1[1]);
        this.ddlSolution.DataSource = dsSolution;
        this.ddlSolution.DataMember = dsSolution.Tables[0].TableName;
        this.ddlSolution.DataTextField = "itemname";
        this.ddlSolution.DataValueField = "itemtype";
        this.ddlSolution.DataBind();
    }

    protected void OKButton_Click(object sender, ImageClickEventArgs e)
    {
        okClick();
    }

    protected void OKButton_Click(object sender, EventArgs e)
    {
        okClick();
    }

    private void okClick()
    {
        if (!Register())
        {
            return;
        }
        Session.Timeout = 30;

        if (tbCheckCode.Text != hfOValue.Value)
        {
            FailureText.Text = SysMsg.GetSystemMessage(CliUtils.fClientLang, "EEPWebNetClient", "WinSysMsg", "msg_CheckCode", true);
            return;
        }

        CliUtils.fLoginUser = txtUserName.Text;
        CliUtils.fLoginPassword = txtPassword.Text;
        CliUtils.fLoginDB = ddlDataBase.SelectedValue;
        CliUtils.fCurrentProject = ddlSolution.SelectedValue;
        CliUtils.fClientSystem = "Web";
        CliUtils.fComputerIp = Request.UserHostAddress;
        CliUtils.fComputerName = Request.UserHostName;

        if (CliUtils.fLoginUser.Contains("'"))
        {
            this.FailureText.Text = SysMsg.GetSystemMessage(CliUtils.fClientLang, "EEPWebNetClient", "WinSysMsg", "msg_UserNotFound", true);
            return;
        }


        if (this.Request.QueryString["IsMU"] != null && this.Request.QueryString["IsMU"].ToString() == "true")
        {
            object[] myRetm = CliUtils.CallMethod("GLModule", "CheckManagerRight", new object[] { CliUtils.fLoginDB, CliUtils.fLoginUser });
            if (myRetm[1].ToString() != "0")
            {
                if (myRetm[1].ToString() == "1")
                {
                    this.FailureText.Text = "No right to use Manager.";
                    return;
                }
                else
                {
                    this.FailureText.Text = "User Not Found.";
                    return;
                }
            }
        }

        string sParam = CliUtils.fLoginUser + ':' + CliUtils.fLoginPassword + ':' + CliUtils.fLoginDB + ':' + "0";/* + ':' + CliUtils.fCurrentProject*/ ;
        object[] myRet = CliUtils.CallMethod("GLModule", "CheckUser", new object[] { (object)sParam });
        LoginResult result = (LoginResult)myRet[1];
        if (result == LoginResult.PasswordError)
        {
            this.FailureText.Text = SysMsg.GetSystemMessage(CliUtils.fClientLang, "EEPWebNetClient", "WinSysMsg", "msg_UserOrPasswordError", true);
        }
        else if (result == LoginResult.UserNotFound)
        {
            this.FailureText.Text = SysMsg.GetSystemMessage(CliUtils.fClientLang, "EEPWebNetClient", "WinSysMsg", "msg_UserNotFound", true);
        }
        else if (result == LoginResult.Disabled)
        { 
             this.FailureText.Text = SysMsg.GetSystemMessage(CliUtils.fClientLang, "EEPWebNetClient", "WinSysMsg", "msg_UserDisabled", true);
        }
        else if (result == LoginResult.RequestReLogin)
        {
            String message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "EEPWebNetClient", "WinSysMsg", "msg_UserReLogined", true);
            this.FailureText.Text = String.Format(message, CliUtils.fLoginUser);
            ReLoginButton.Visible = true;
        }
        else if (result == LoginResult.UserLogined)
        {
            String message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "EEPWebNetClient", "WinSysMsg", "msg_UserIsLogined", true);
            this.FailureText.Text = String.Format(message, CliUtils.fLoginUser);
        }
        else//sucessed to login...
        {
            CliUtils.fUserName = myRet[2].ToString();
            CliUtils.fLoginUser = myRet[3].ToString();
            myRet = CliUtils.CallMethod("GLModule", "GetUserGroup", new object[] { CliUtils.fLoginUser });
            if (myRet != null && (int)myRet[0] == 0)
            {
                CliUtils.fGroupID = myRet[1].ToString();
            }

            if (this.Request.QueryString["IsMU"] == null || this.Request.QueryString["IsMU"].ToString() != "true")
            {
                if (CliUtils.fPassWordNotify != 0)
                    CheckPassword(CliUtils.fLoginUser, CliUtils.fLoginDB, CliUtils.fCurrentProject);
            }

            if (this.Request.QueryString["IsMU"] != null && this.Request.QueryString["IsMU"].ToString() == "true")
                Response.Redirect("WebMenuUtility.aspx", true);
            else
            {
                DataSet dsSolution = new DataSet();
                if (CliUtils.fSolutionSecurity)
                {
                    object[] myRet1 = CliUtils.CallMethod("GLModule", "GetSolutionSecurity", new object[] { CliUtils.fLoginUser, CliUtils.fGroupID });
                    if ((null != myRet1) && (0 == (int)myRet1[0]))
                        dsSolution = ((DataSet)myRet1[1]);
                    bool flag = false;
                    for (int i = 0; i < dsSolution.Tables[0].Rows.Count; i++)
                    {
                        if (dsSolution.Tables[0].Rows[i]["itemtype"].ToString() == CliUtils.fCurrentProject)
                        {
                            flag = true;
                            break;
                        }
                    }
                    if (!flag)
                    {
                        string message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "EEPWebNetClient", "WinSysMsg", "msg_SolutionSecurity", true);
                        this.FailureText.Text = String.Format(message, CliUtils.fCurrentProject);
                        return;
                    }
                }
                else
                {
                    object[] myRet1 = CliUtils.CallMethod("GLModule", "GetSolution", null);
                    if ((null != myRet1) && (0 == (int)myRet1[0]))
                        dsSolution = ((DataSet)myRet1[1]);
                }
                object obj = this.ViewState["IsMasterPage"];
                object objFlow = this.ViewState["IsFlowPage"];
                if (objFlow != null && objFlow.ToString() == "true")
                {
                    myRet = CliUtils.CallMethod("GLModule", "GetUserRole", new object[] { });
                    if (myRet != null && (int)myRet[0] == 0)
                    {
                        CliUtils.Roles = myRet[1].ToString();
                        CliUtils.OrgRoles = myRet[2].ToString();
                        CliUtils.OrgShares = myRet[3].ToString();
                    }
                    Response.Redirect("webClientMainFlow.aspx", true);
                    CliUtils.fClientMainFlow = true;
                }
                else
                {
                    if (obj == null || obj.ToString() == "false")
                    {
                        Response.Redirect("webClientMain.aspx", true);
                        CliUtils.fClientMainFlow = false;
                    }
                    else
                    {
                        string flow = ConfigurationManager.AppSettings["IsFlow"];
                        if (string.IsNullOrEmpty(flow))
                        {
                            Response.Redirect("DefaultPage2.aspx", true);
                            CliUtils.fClientMainFlow = false;
                        }
                        else
                        {
                            if (flow == "true")
                            {
                                Response.Redirect("DefaultPage3.aspx", true);
                                CliUtils.fClientMainFlow = true;
                            }
                            else
                            {
                                Response.Redirect("DefaultPage2.aspx", true);
                                CliUtils.fClientMainFlow = false;
                            }
                        }
                    }
                }
            }
        }
    }

    protected void ReLoginButton_Click(object sender, ImageClickEventArgs e)
    {
        reloginClick();
    }

    protected void ReLoginButton_Click(object sender, EventArgs e)
    {
        reloginClick();
    }

    private void reloginClick()
    {
        Session.Timeout = 30;

        string sParam = CliUtils.fLoginUser + ':' + CliUtils.fLoginPassword + ':' + CliUtils.fLoginDB + ':' + "1";
        object[] myRet = CliUtils.CallMethod("GLModule", "CheckUser", new object[] { (object)sParam });
        LoginResult result = (LoginResult)myRet[1];
        if (result == LoginResult.PasswordError)
        {
            this.FailureText.Text = SysMsg.GetSystemMessage(CliUtils.fClientLang, "EEPWebNetClient", "WinSysMsg", "msg_UserOrPasswordError", true);
        }
        else if (result == LoginResult.UserNotFound)
        {
            this.FailureText.Text = SysMsg.GetSystemMessage(CliUtils.fClientLang, "EEPWebNetClient", "WinSysMsg", "msg_UserNotFound", true);
        }
        else if (result == LoginResult.Disabled)
        {
            this.FailureText.Text = SysMsg.GetSystemMessage(CliUtils.fClientLang, "EEPWebNetClient", "WinSysMsg", "msg_UserDisabled", true);
        }
        else//sucessed to login...
        {
            CliUtils.fUserName = myRet[2].ToString();
            CliUtils.fLoginUser = myRet[3].ToString();
            myRet = CliUtils.CallMethod("GLModule", "GetUserGroup", new object[] { CliUtils.fLoginUser });
            if (myRet != null && (int)myRet[0] == 0)
            {
                CliUtils.fGroupID = myRet[1].ToString();
            }
            if (Session["State"] != null && Session["State"].ToString() == "MU")
                Response.Redirect("WebMenuUtility.aspx", true);
            else
            {
                object obj = this.ViewState["IsMasterPage"];
                if (obj == null || obj.ToString() == "true")
                    Response.Redirect("webClientMain.aspx", true);
                else
                    Response.Redirect("DefaultPage2.aspx", true);
            }
        }
    }

    private void CheckPassword(String userid, String database, String solution)
    {
        DateTime date = new DateTime();
        DateTime today = DateTime.Today;
        String value = "";
        object[] param = new object[3];
        param[0] = userid;

        object[] myRet = CliUtils.CallMethod("GLModule", "GetPasswordLastDate", param);
        if (myRet != null && myRet[0].ToString() == "0")
        {
            if (myRet[1] == DBNull.Value || myRet[1].ToString() == "")
                value = "new";
            else
            {
                date = DateTime.ParseExact(myRet[1].ToString(), "yyyyMMdd", null);
                TimeSpan ts = today - date;
                value = ts.TotalDays.ToString();
            }
        }

        if (value == "new")
        {
            this.Page.Response.Redirect("WebUPWDControl.aspx?", true); //Value=" + value, true);
        }
        else
        {
            if (Convert.ToInt32(value) > CliUtils.fPassWordExpiry)
            {
                this.Page.Response.Redirect("WebUPWDControl.aspx?Value=" + value, true);
            }
            else if ((CliUtils.fPassWordExpiry - Convert.ToInt32(value)) <= CliUtils.fPassWordNotify)
            {
                string message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "EEPNetClient", "FrmClientMain", "PasswordNotify", true);
                MessageBox.Show(String.Format(message, CliUtils.fPassWordExpiry - Convert.ToInt32(value)));
            }
        }
    }
}