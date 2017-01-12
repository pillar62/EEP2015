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
using System.IO;

public partial class WebManager_WebAgentaspx : System.Web.UI.Page
{
    private WebDataSet wdsUsers;
    private Srvtools.WebDataSet wdsAgent;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            InitializeComponent();

            WDSAgent.DataSource = wdsAgent;
            WDSUsers.DataSource = wdsUsers;
        }

        SYS_LANGUAGE language = CliUtils.fClientLang;
        try
        {
            string[] captions = SysMsg.GetSystemMessage(language, "Srvtools", "frmAgent", "UITexts").Split(',');
            for (int i = 0; i < this.WebGridView1.Columns.Count - 2; i++)
                this.WebGridView1.Columns[i + 2].HeaderText = captions[i];

            this.WebGridView1.Columns[1].HeaderText = captions[8];
            this.lblRoleId.Text = captions[8];
            this.lblRoleName.Text = captions[9];
        }
        catch { }

        this.tbRoleID.Text = this.Request.QueryString["GROUPID"];
        this.tbRoleName.Text = this.Request.QueryString["GROUPNAME"];

        this.WDSAgent.SetWhere("role_id='" + this.Request.QueryString["GROUPID"] + "'");
        this.WDSAgent.DataBind();
    }

    public string DefRoleId()
    {
        return this.Request.QueryString["GROUPID"];
    }

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WebManager_WebAgentaspx));
        this.wdsAgent = new Srvtools.WebDataSet();
        this.wdsUsers = new Srvtools.WebDataSet();
        ((System.ComponentModel.ISupportInitialize)(this.wdsAgent)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.wdsUsers)).BeginInit();
        // 
        // wdsAgent
        // 
        this.wdsAgent.Active = true;
        this.wdsAgent.AlwaysClose = false;
        this.wdsAgent.DeleteIncomplete = true;
        this.wdsAgent.Guid = "1bbca14e-4069-4633-80b8-23ba35eda312";
        this.wdsAgent.LastKeyValues = null;
        this.wdsAgent.Locale = new System.Globalization.CultureInfo("zh-CN");
        this.wdsAgent.PacketRecords = 100;
        this.wdsAgent.Position = -1;
        this.wdsAgent.RemoteName = "GLModule.cmdRoleAgent";
        this.wdsAgent.ServerModify = false;
        // 
        // wdsUsers
        // 
        this.wdsUsers.Active = true;
        this.wdsUsers.AlwaysClose = false;
        this.wdsUsers.DeleteIncomplete = true;
        this.wdsUsers.Guid = "16355a42-a12f-452f-a30d-77daa26a7d68";
        this.wdsUsers.LastKeyValues = null;
        this.wdsUsers.Locale = new System.Globalization.CultureInfo("zh-CN");
        this.wdsUsers.PacketRecords = 100;
        this.wdsUsers.Position = -1;
        this.wdsUsers.RemoteName = "GLModule.userInfo";
        this.wdsUsers.ServerModify = false;
        ((System.ComponentModel.ISupportInitialize)(this.wdsAgent)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.wdsUsers)).EndInit();

    }

    protected void WebGridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //if (e.Row.RowIndex != -1)
        //{
        if (this.WebGridView1.FooterRow != null)
        {
            string flowDefPath = string.Format("{0}\\WorkFlow\\", EEPRegistry.Server);
            DirectoryInfo dir = new DirectoryInfo(flowDefPath);
            if (dir.Exists)
            {
                object[] obj = CliUtils.CallFLMethod("GetFLDescriptions", new object[] { flowDefPath });
                if (Convert.ToInt16(obj[0]) == 0)
                {
                    ArrayList flDescs = (ArrayList)obj[1];
                    flDescs.Insert(0, " ");
                    if ((this.WebGridView1.FooterRow.FindControl("colFlowDesc") as DropDownList) != null)
                    {
                        (this.WebGridView1.FooterRow.FindControl("colFlowDesc") as DropDownList).Items.Clear();
                        for (int i = 0; i < flDescs.Count; i++)
                            (this.WebGridView1.FooterRow.FindControl("colFlowDesc") as DropDownList).Items.Add(flDescs[i].ToString());
                    }
                }
            }
        }

        if (this.WebGridView1.SelectedIndex != -1)
        {
            string flowDefPath = string.Format("{0}\\WorkFlow\\", EEPRegistry.Server);
            DirectoryInfo dir = new DirectoryInfo(flowDefPath);
            if (dir.Exists)
            {
                object[] obj = CliUtils.CallFLMethod("GetFLDescriptions", new object[] { flowDefPath });
                if (Convert.ToInt16(obj[0]) == 0)
                {
                    ArrayList flDescs = (ArrayList)obj[1];
                    flDescs.Insert(0, " ");
                    if ((e.Row.FindControl("colFlowDesc") as DropDownList) != null)
                    {
                        (e.Row.FindControl("colFlowDesc") as DropDownList).Items.Clear();
                        for (int i = 0; i < flDescs.Count; i++)
                        {
                            (e.Row.FindControl("colFlowDesc") as DropDownList).Items.Add(flDescs[i].ToString());
                            (e.Row.FindControl("colFlowDesc") as DropDownList).SelectedValue = (e.Row.FindControl("HiddenField1") as HiddenField).Value;
                        }
                    }
                    //(e.Row.FindControl("colFlowDesc") as DropDownList).DataBind();
                }
            }

        }
        //}
    }
    protected void colFlowDesc_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (this.WebGridView1.SelectedRow != null)
            if ((this.WebGridView1.SelectedRow.FindControl("HiddenField1") as HiddenField) != null)
                (this.WebGridView1.SelectedRow.FindControl("HiddenField1") as HiddenField).Value = (sender as DropDownList).SelectedValue;
        if (this.WebGridView1.FooterRow != null)
            if ((this.WebGridView1.FooterRow.FindControl("HiddenField1") as HiddenField) != null)
                (this.WebGridView1.FooterRow.FindControl("HiddenField1") as HiddenField).Value = (sender as DropDownList).SelectedValue;
    }
}
