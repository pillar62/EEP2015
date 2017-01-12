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
using System.Windows.Forms;
using Microsoft.Win32;
using System.IO;
using System.Xml;
using Srvtools;

public partial class WebUPWDControl : System.Web.UI.Page
{
    bool isChanged = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        string[] langs = Request.UserLanguages;

        try
        {
            if (string.Compare(langs[0], "zh-cn", true) == 0)//IgnoreCase
            {
                CliUtils.fClientLang = SYS_LANGUAGE.SIM;
                td11.Attributes.CssStyle.Value = "background-image:url('Image/main/changepwd_bg_cn.png');";
            }
            else if (string.Compare(langs[0], "zh-tw", true) == 0)//IgnoreCase
            {
                CliUtils.fClientLang = SYS_LANGUAGE.TRA;
                td11.Attributes.CssStyle.Value = "background-image:url('Image/main/changepwd_bg_tw.png');";
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

        string message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "Srvtools", "UPWDControl", "LabelName", true);
        string[] user = message.Split(';');
        labelUserID.Text = user[0];
        labelUserName.Text = user[1];
        labelOldPassword.Text = user[2];
        labelNewPassword.Text = user[3];
        labelConfirmPassword.Text = user[4];
        if (CliUtils.fLoginUser != null && CliUtils.fLoginUser.Length != 0)
        {
            string userName = null;
            txtUserID.Text = CliUtils.fLoginUser;
            txtUserName.Text = CliUtils.fUserName;
        }

        if (!IsPostBack)
        {
            if (this.Page.Request.QueryString["Value"] != null)
            {
                if (this.Page.Request.QueryString["Value"].ToString() == "New")
                {
                    String passMessage = SysMsg.GetSystemMessage(CliUtils.fClientLang, "EEPNetClient", "FrmClientMain", "NewPassword", true);
                    this.labelMessage.Text = passMessage;
                }
                else
                {
                    if (Convert.ToInt32(this.Page.Request.QueryString["Value"]) > CliUtils.fPassWordExpiry)
                    {
                        string passMessage = SysMsg.GetSystemMessage(CliUtils.fClientLang, "EEPNetClient", "FrmClientMain", "PasswordAnnulment", true);
                        this.labelMessage.Text = passMessage;

                        isChanged = false;
                    }
                }
            }
            else
            {
                btnLogin.Visible = false;
            }
        }
    }
    protected void btnOK_Click(object sender, EventArgs e)
    {
        if (txtNewPassword.Text != txtConfirmPassword.Text)
        {
            string message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "Srvtools", "UPWDControl", "NewPasswordErrorMessage", true);
            txtConfirmPassword.Text = "";
            txtNewPassword.Text = "";
            this.Page.Response.Write("<script>alert(\"" + message + "\");</script>");
            return;
        }

        if (txtNewPassword.Text.Length >= CliUtils.fPassWordMinSize && txtNewPassword.Text.Length <= CliUtils.fPassWordMaxSize)
        {
            if (CliUtils.fPassWordCharNum)
            {
                int x = 0, y = 0;
                for (int i = 0; i < txtNewPassword.Text.Length; i++)
                {
                    if (!char.IsLetterOrDigit(txtNewPassword.Text, i))
                    {
                        string message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "Srvtools", "UGControl", "PasswordCharCheck", true);
                        labelMessage.Text = message;
                        return;
                    }
                    else if (char.IsLetter(txtNewPassword.Text, i))
                    {
                        x++;
                    }
                    else if (char.IsNumber(txtNewPassword.Text, i))
                    {
                        y++;
                    }
                }
                if (x == 0 || y == 0)
                {
                    string message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "Srvtools", "UGControl", "PasswordCharNum", true);
                    labelMessage.Text = message;
                    return;
                }
            }

            string sParam = txtUserID.Text + ':' + txtOldPassword.Text + ':' + txtNewPassword.Text + ":" + CliUtils.fLoginDB;
            object[] myRet = CliUtils.CallMethod("GLModule", "ChangePassword", new object[] { (object)sParam });
            if (myRet[1].ToString() == "E")
            {
                string message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "Srvtools", "UPWDControl", "OldPasswordErrorMessage", true);
                labelMessage.Text = message;
            }
            else if (myRet[1].ToString() == "O")
            {
                string message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "Srvtools", "UPWDControl", "ChangeSucceed", true);
                labelMessage.Text = message;
                isChanged = true;
            }
        }
        else
        {
            String message = String.Format(SysMsg.GetSystemMessage(CliUtils.fClientLang, "Srvtools", "UGControl", "PasswordLength", true), CliUtils.fPassWordMinSize, CliUtils.fPassWordMaxSize);
            labelMessage.Text = message;
        }
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Response.Redirect("InfoLogin.aspx", true);
    }

    private bool IsFlowPage()
    {
        bool flow = false;
        string config_IsFlow = ConfigurationManager.AppSettings["IsFlow"];
        if (!string.IsNullOrEmpty(config_IsFlow) && string.Compare(config_IsFlow, "true", true) == 0)
        {
            return true;
        }
        string config_FlowSolutions = ConfigurationManager.AppSettings["FlowSolutions"];
        if (!string.IsNullOrEmpty(config_FlowSolutions))
        {
            string[] solutions = config_FlowSolutions.Split(',');
            foreach (string sol in solutions)
            {
                if (string.Compare(sol, CliUtils.fCurrentProject, true) == 0)
                {
                    flow = true;
                    break;
                }
            }
        }
        return flow;
    }

    protected void btnLogin_Click(object sender, EventArgs e)
    {
        if (this.Page.Request.QueryString["Value"] != null)
        {
            object obj = this.ViewState["IsMasterPage"];
            if (IsFlowPage())
            {
                if (this.Page.Request.QueryString["Value"].ToString() == "New")
                {
                    Response.Redirect("webClientMainFlow.aspx", true);
                }
                else if (Convert.ToInt32(this.Page.Request.QueryString["Value"]) > CliUtils.fPassWordExpiry)
                {
                    if (isChanged)
                        Response.Redirect("webClientMainFlow.aspx", true);
                    else
                    {
                        string passMessage = SysMsg.GetSystemMessage(CliUtils.fClientLang, "EEPNetClient", "FrmClientMain", "PasswordAnnulment", true);
                        this.labelMessage.Text = passMessage;
                    }
                } 
                CliUtils.fClientMainFlow = true;
            }
            else
            {
                if (obj == null || obj.ToString() == "false")
                {
                    if (this.Page.Request.QueryString["Value"].ToString() == "New")
                    {
                        Response.Redirect("webClientMain.aspx", true);
                    }
                    else if (Convert.ToInt32(this.Page.Request.QueryString["Value"]) > CliUtils.fPassWordExpiry)
                    {
                        if (isChanged)
                            Response.Redirect("webClientMain.aspx", true);
                        else
                        {
                            string passMessage = SysMsg.GetSystemMessage(CliUtils.fClientLang, "EEPNetClient", "FrmClientMain", "PasswordAnnulment", true);
                            this.labelMessage.Text = passMessage;
                        }
                    } 
                    CliUtils.fClientMainFlow = false;
                }
                else
                {
                    string flow = ConfigurationManager.AppSettings["IsFlow"];
                    if (string.IsNullOrEmpty(flow))
                    {
                        if (this.Page.Request.QueryString["Value"].ToString() == "New")
                        {
                            Response.Redirect("DefaultPage2.aspx", true);
                        }
                        else if (Convert.ToInt32(this.Page.Request.QueryString["Value"]) > CliUtils.fPassWordExpiry)
                        {
                            if (isChanged)
                                Response.Redirect("DefaultPage2.aspx", true);
                            else
                            {
                                string passMessage = SysMsg.GetSystemMessage(CliUtils.fClientLang, "EEPNetClient", "FrmClientMain", "PasswordAnnulment", true);
                                this.labelMessage.Text = passMessage;
                            }
                        } 
                        CliUtils.fClientMainFlow = false;
                    }
                    else
                    {
                        if (flow == "true")
                        {
                            if (this.Page.Request.QueryString["Value"].ToString() == "New")
                            {
                                Response.Redirect("DefaultPage3.aspx", true);
                            }
                            else if (Convert.ToInt32(this.Page.Request.QueryString["Value"]) > CliUtils.fPassWordExpiry)
                            {
                                if (isChanged)
                                    Response.Redirect("DefaultPage3.aspx", true);
                                else
                                {
                                    string passMessage = SysMsg.GetSystemMessage(CliUtils.fClientLang, "EEPNetClient", "FrmClientMain", "PasswordAnnulment", true);
                                    this.labelMessage.Text = passMessage;
                                }
                            } 
                            CliUtils.fClientMainFlow = true;
                        }
                        else
                        {
                            if (this.Page.Request.QueryString["Value"].ToString() == "New")
                            {
                                Response.Redirect("DefaultPage2.aspx", true);
                            }
                            else if (Convert.ToInt32(this.Page.Request.QueryString["Value"]) > CliUtils.fPassWordExpiry)
                            {
                                if (isChanged)
                                    Response.Redirect("DefaultPage2.aspx", true);
                                else
                                {
                                    string passMessage = SysMsg.GetSystemMessage(CliUtils.fClientLang, "EEPNetClient", "FrmClientMain", "PasswordAnnulment", true);
                                    this.labelMessage.Text = passMessage;
                                }
                            } 
                            CliUtils.fClientMainFlow = false;
                        }
                    }
                }
            }
            
        }
    }
}
