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

public partial class InnerPages_WebUControlAdd : System.Web.UI.Page
{
    private Srvtools.WebDataSet WUser;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            InitializeComponent();
            wdsUser.DataSource = WUser;

            //2009/09/12 add by eva由於添加css後在新增修改時希望不要看到access的frame的色塊
            string ModeStr = Request.QueryString["OpenEditMode"];
            if (ModeStr != null)
            {
                this.ViewState.Add("ModeStr", ModeStr);
            }
        }

        if (this.ViewState["ModeStr"].ToString() == "Insert" || this.ViewState["ModeStr"].ToString() == "Update")
        {
            this.accessFrame.Visible = false;
        }
        else
        {
            this.accessFrame.Visible = true;
        }
        //End add

        this.Page.LoadComplete += new EventHandler(Page_LoadComplete);
    }

    void Page_LoadComplete(object sender, EventArgs e)
    {
        if (!(wfvUser.CurrentMode == FormViewMode.Insert))
        {
            if (wfvUser.FindControl("USERIDLabel") as Label != null)
                this.Session["oldUserID"] = (wfvUser.FindControl("USERIDLabel") as Label).Text;
        }

        if (wfvUser.CurrentMode == FormViewMode.ReadOnly)
        {
            Session["State"] = "U";
            Session["UserID"] = (wfvUser.FindControl("USERIDLabel") as Label).Text;
            Session["UserName"] = (wfvUser.FindControl("USERNAMELabel") as Label).Text;
        }
        else
        {
            Session["State"] = "X";
        }
    }

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InnerPages_WebUControlAdd));
        this.WUser = new Srvtools.WebDataSet();
        ((System.ComponentModel.ISupportInitialize)(this.WUser)).BeginInit();
        // 
        // WUser
        // 
        this.WUser.Active = true;
        this.WUser.AlwaysClose = false;
        this.WUser.DeleteIncomplete = true;
        this.WUser.Guid = "10684bb2-5e9c-4385-9b1f-e9c71d4c5b4b";
        this.WUser.LastKeyValues = null;
        this.WUser.PacketRecords = 100;
        this.WUser.Position = -1;
        this.WUser.RemoteName = "GLModule.userInfo";
        this.WUser.ServerModify = false;
        ((System.ComponentModel.ISupportInitialize)(this.WUser)).EndInit();

    }

    protected void wtUser_OKClick(object sender, EventArgs e)
    {
        if (wfvUser.CurrentMode == FormViewMode.Edit)
        {
            String oldUserID = this.Session["oldUserID"].ToString();
            String newUserID = (wfvUser.FindControl("USERIDTextBox") as TextBox).Text;
            if (oldUserID != null && oldUserID != "" && oldUserID != newUserID)
            {
                Srvtools.CliUtils.ExecuteSql("GLModule", "userInfo", "Update USERMENUS set USERID='" + newUserID + "' where USERID='" + oldUserID + "'", false, Srvtools.CliUtils.fCurrentProject);
                Srvtools.CliUtils.ExecuteSql("GLModule", "userInfo", "Update USERMENUCONTROL set USERID='" + newUserID + "' where USERID='" + oldUserID + "'", false, Srvtools.CliUtils.fCurrentProject);
            }
        }

        if (wfvUser.CurrentMode != FormViewMode.ReadOnly)
        {
            String newUserID = (wfvUser.FindControl("USERIDTextBox") as TextBox).Text;
            String txtPassword = (wfvUser.FindControl("PWDTextBox") as TextBox).Text;
            if (!string.IsNullOrEmpty(txtPassword))
            {
                char[] p = new char[] { };
                bool s = Srvtools.Encrypt.EncryptPassword(newUserID, txtPassword, 10, ref p, false);
                string pwd = new string(p);
                (wfvUser.FindControl("PWDTextBox") as TextBox).Text = pwd;
            }
        }
    }

    protected void wfvUser_DataBound(object sender, EventArgs e)
    {
        string[] headers = SysMsg.GetSystemMessage(Srvtools.CliUtils.fClientLang, "Srvtools", "UGControl", "Caption_User").Split(';');
        Srvtools.WebFormView aWebFormView = this.wfvUser;
        if (aWebFormView != null && (aWebFormView.FindControl("CaptionUSERID") as Label) != null)
        {
            (aWebFormView.FindControl("CaptionUSERID") as Label).Text = headers[0] + ": ";
            (aWebFormView.FindControl("CaptionUSERNAME") as Label).Text = headers[1] + ": ";
            (aWebFormView.FindControl("CaptionDESCRIPTION") as Label).Text = headers[2] + ": ";
            (aWebFormView.FindControl("CaptionAGENT") as Label).Text = headers[3] + ": ";
            (aWebFormView.FindControl("CaptionCREATEDATE") as Label).Text = headers[4] + ": ";
            (aWebFormView.FindControl("CaptionDESCRIPTION") as Label).Text = headers[5] + ": ";
            (aWebFormView.FindControl("CaptionEMAIL") as Label).Text = headers[6] + ": ";
            (aWebFormView.FindControl("CaptionAUTOLOGIN") as Label).Text = headers[7] + ": ";
            (aWebFormView.FindControl("CaptionMSAD") as Label).Text = headers[8] + ": ";
            if (aWebFormView.FindControl("CaptionPWD") != null)
                (aWebFormView.FindControl("CaptionPWD") as Label).Text = headers[9] + ": ";
        }
    }
}
