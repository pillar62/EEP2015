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

public partial class InnerPages_WebGControlAdd : System.Web.UI.Page
{
    private Srvtools.WebDataSet WUser;
    private Srvtools.WebDataSet WGroup;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            InitializeComponent();
            wdsGroup.DataSource = WGroup;
            btnSave.Visible = false;

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

        this.LoadComplete += new EventHandler(InnerPages_WebGControlAdd_LoadComplete);
    }

    void InnerPages_WebGControlAdd_LoadComplete(object sender, EventArgs e)
    {
        if (wfvGroup.CurrentMode == FormViewMode.ReadOnly && WUser != null)
        {
            btnSave.Visible = true;

            for (int i = 0; i < WUser.RealDataSet.Tables[0].Rows.Count; i++)
            {
                cblUser.Items.Add(WUser.RealDataSet.Tables[0].Rows[i]["UserName"].ToString());
                cblUser.Items[i].Text = WUser.RealDataSet.Tables[0].Rows[i]["UserName"].ToString();
                cblUser.Items[i].Value = WUser.RealDataSet.Tables[0].Rows[i]["UserID"].ToString();
            }

            for (int i = 0; i < cblUser.Items.Count; i++)
                cblUser.Items[i].Selected = false;
            object[] param = new object[1];
            DataSet dsUsers = new DataSet();
            param[0] = (wfvGroup.FindControl("GROUPIDLabel") as Label).Text;
            object[] myRet = Srvtools.CliUtils.CallMethod("GLModule", "ListUsers", param);
            if ((null != myRet) && (0 == (int)myRet[0]))
                dsUsers = (DataSet)(myRet[1]);

            if (dsUsers.Tables.Count > 0)
            {
                for (int i = 0; i < dsUsers.Tables[0].Rows.Count; i++)
                    for (int j = 0; j < cblUser.Items.Count; j++)
                    {
                        if (dsUsers.Tables[0].Rows[i]["UserID"].ToString() == cblUser.Items[j].Value)
                            cblUser.Items[j].Selected = true;
                    }
            }
        }
        if (wfvGroup.CurrentMode == FormViewMode.Edit)
        {
            if (wfvGroup.FindControl("GROUPIDLabel") as Label != null)
                this.Session["oldGroupID"] = (wfvGroup.FindControl("GROUPIDLabel") as Label).Text;
        }
        if (wfvGroup.CurrentMode == FormViewMode.ReadOnly)
        {
            Session["State"] = "G";
            Session["GroupID"] = (wfvGroup.FindControl("GROUPIDLabel") as Label).Text;
            Session["GroupName"] = (wfvGroup.FindControl("GROUPNAMELabel") as Label).Text;
        }
        else
        {
            Session["State"] = "X";
        }
    }

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InnerPages_WebGControlAdd));
        this.WGroup = new Srvtools.WebDataSet();
        this.WUser = new Srvtools.WebDataSet();
        ((System.ComponentModel.ISupportInitialize)(this.WGroup)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.WUser)).BeginInit();
        // 
        // WGroup
        // 
        this.WGroup.Active = true;
        this.WGroup.AlwaysClose = false;
        this.WGroup.DeleteIncomplete = true;
        this.WGroup.Guid = "69ebc916-8f45-4265-972f-2959809e2a02";
        this.WGroup.LastKeyValues = null;
        this.WGroup.PacketRecords = 100;
        this.WGroup.Position = -1;
        this.WGroup.RemoteName = "GLModule.groupInfo";
        this.WGroup.ServerModify = false;
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
        ((System.ComponentModel.ISupportInitialize)(this.WGroup)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.WUser)).EndInit();

    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        object[] param = new object[2];
        param[0] = (wfvGroup.FindControl("GROUPIDLabel") as Label).Text;
        for (int i = 0; i < cblUser.Items.Count; i++)
            if (cblUser.Items[i].Selected == true)
            {
                string temp = cblUser.Items[i].Value;
                param[1] += temp + ";";
            }
        Srvtools.CliUtils.CallMethod("GLModule", "SetUserGroups", param);
        this.Page.Response.Write("<script language=javascript>window.close();</script>");
    }

    protected void wtGroup_OKClick(object sender, EventArgs e)
    {
        if (wfvGroup.CurrentMode == FormViewMode.Edit)
        {
            String oldGroupID = this.Session["oldGroupID"].ToString();
            String newGroupID = (wfvGroup.FindControl("GROUPIDTextBox") as TextBox).Text;
            if (oldGroupID != null && oldGroupID != "" && oldGroupID != newGroupID)
            {
                Srvtools.CliUtils.ExecuteSql("GLModule", "groupInfo", "Update GROUPMENUS set GROUPID='" + newGroupID + "' where GROUPID='" + oldGroupID + "'", false, Srvtools.CliUtils.fCurrentProject);
                Srvtools.CliUtils.ExecuteSql("GLModule", "groupInfo", "Update GROUPMENUCONTROL set GROUPID='" + newGroupID + "' where GROUPID='" + oldGroupID + "'", false, Srvtools.CliUtils.fCurrentProject);
            }
        }
    }

    protected void wfvGroup_DataBound(object sender, EventArgs e)
    {
        string[] headers = SysMsg.GetSystemMessage(Srvtools.CliUtils.fClientLang, "Srvtools", "UGControl", "Caption_Group").Split(';');
        Srvtools.WebFormView aWebFormView = this.wfvGroup;
        if (aWebFormView != null && (aWebFormView.FindControl("CaptionGROUPID") as Label) != null)
        {
            (aWebFormView.FindControl("CaptionGROUPID") as Label).Text = headers[0] + ": ";
            (aWebFormView.FindControl("CaptionGROUPNAME") as Label).Text = headers[1] + ": ";
            (aWebFormView.FindControl("CaptionDESCRIPTION") as Label).Text = headers[2] + ": ";
            (aWebFormView.FindControl("CaptionMSAD") as Label).Text = headers[3] + ": ";
        }
    }
}
