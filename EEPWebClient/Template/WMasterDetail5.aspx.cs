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

public partial class Template_WMasterDetail6 : System.Web.UI.Page
{
    private Srvtools.WebDataSet WMaster;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            InitializeComponent();

            Master.DataSource = WMaster;
            Detail.DataSource = WMaster;
        }
    }

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Template_WMasterDetail6));
        this.WMaster = new Srvtools.WebDataSet();
        ((System.ComponentModel.ISupportInitialize)(this.WMaster)).BeginInit();
        // 
        // WMaster
        // 
        this.WMaster.Active = false;
        this.WMaster.AlwaysClose = false;
        this.WMaster.DeleteIncomplete = true;
        this.WMaster.Guid = null;
        this.WMaster.LastKeyValues = null;
        this.WMaster.Locale = new System.Globalization.CultureInfo("zh-CN");
        this.WMaster.PacketRecords = 100;
        this.WMaster.Position = -1;
        this.WMaster.RefCommandText = null;
        this.WMaster.RefDBAlias = null;
        this.WMaster.RemoteName = null;
        this.WMaster.ServerModify = false;
        ((System.ComponentModel.ISupportInitialize)(this.WMaster)).EndInit();

    }
    protected void ButtonOK_Click(object sender, EventArgs e)
    {
        if (this.wfvMaster.CurrentMode == FormViewMode.Insert)
        {
            this.wfvMaster.InsertItem(false);
            this.wfvMaster.PageIndex = this.wfvMaster.PageCount - 1;
            this.wfvMaster.AllowPaging = false;
            Master.ApplyUpdates();
        }
        else
        {
            if (this.WebNavigator1.CurrentNavState != "Editing")
            {
                this.WebNavigator1.SetNavState("Inserting");
                this.WebNavigator1.SetState(Srvtools.WebNavigator.NavigatorState.Inserting);
            }
            AjaxModalPanel1.Submit();
        }
    }
    protected void buttonClose_Click(object sender, EventArgs e)
    {
        if (wfvDetail.CurrentMode != FormViewMode.ReadOnly)
            wfvDetail.ChangeMode(FormViewMode.ReadOnly);
        this.WebNavigator1.SetNavState("Browsed");
        this.WebNavigator1.SetState(Srvtools.WebNavigator.NavigatorState.Browsing);
        AjaxModalPanel1.Close();
    }
    protected void ButtonOK1_Click(object sender, EventArgs e)
    {
        AjaxModalPanel2.Submit();
    }
    protected void buttonClose1_Click(object sender, EventArgs e)
    {
        AjaxModalPanel2.Close();
    }
    protected void wfvMaster_PageIndexChanged(object sender, EventArgs e)
    {
        Detail.ExecuteSelect(wfvMaster);
        DataBind();
    }
    protected void wfvMaster_AfterInsertLocate(object sender, EventArgs e)
    {
        Detail.ExecuteSelect(wfvMaster);
        DataBind();
    }
    protected void wfvMaster_Canceled(object sender, EventArgs e)
    {
        Detail.ExecuteSelect(wfvMaster);
        DataBind();
    }
    protected void WebNavigator1_Command(object sender, CommandEventArgs e)
    {
        if (e.CommandName == "cmdFirst" || e.CommandName == "cmdPrevious"
                || e.CommandName == "cmdNext" || e.CommandName == "cmdLast")
        {
            Detail.ExecuteSelect(wfvMaster);
            DataBind();
        }
        else if (e.CommandName == "cmdAdd")
        {
            Detail.ExecuteAdd(wfvMaster);
            DataBind();
        }
        else if (e.CommandName == "cmdDelete")
        {
            Detail.ExecuteSelect(wfvMaster);
            DataBind();
        }
        else if (e.CommandName == "cmdApply" && wfvMaster.AllValidateSucess)
        {
            Detail.ExecuteSelect(wfvMaster);
            DataBind();
        }
    }

    protected void wgvDetail_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "AjaxEdit")
        {
            this.WebNavigator1.SetNavState("Editing");
            this.WebNavigator1.SetState(Srvtools.WebNavigator.NavigatorState.Editing);
        }
    }
}
