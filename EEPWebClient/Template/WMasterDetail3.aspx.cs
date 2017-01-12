using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Srvtools;

public partial class Template_WMasterDetail3 : System.Web.UI.Page
{
    private Srvtools.WebDataSet WView;
    private Srvtools.WebDataSet WMaster;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            InitializeComponent();
            Master.DataSource = WMaster;
            Detail.DataSource = WMaster;
            View.DataSource = WView;
        }
    }

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Template_WMasterDetail3));
        this.WMaster = new Srvtools.WebDataSet();
        this.WView = new Srvtools.WebDataSet();
        ((System.ComponentModel.ISupportInitialize)(this.WMaster)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.WView)).BeginInit();
        // 
        // WMaster
        // 
        this.WMaster.Active = false;
        this.WMaster.AlwaysClose = true;
        this.WMaster.Guid = null;
        this.WMaster.LastKeyValues = null;
        this.WMaster.PacketRecords = 100;
        this.WMaster.Position = -1;
        this.WMaster.RemoteName = null;
        this.WMaster.ServerModify = false;
        // 
        // WView
        // 
        this.WView.Active = false;
        this.WView.AlwaysClose = false;
        this.WView.Guid = "2c363173-ea0b-47df-8082-340176b68302";
        this.WView.LastKeyValues = null;
        this.WView.PacketRecords = 100;
        this.WView.Position = -1;
        this.WView.RemoteName = null;
        this.WView.ServerModify = false;
        ((System.ComponentModel.ISupportInitialize)(this.WMaster)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.WView)).EndInit();

    }
    protected void WebNavigator1_Command(object sender, CommandEventArgs e)
    {
        if (e.CommandName == "cmdFirst" || e.CommandName == "cmdPrevious"
            || e.CommandName == "cmdNext" || e.CommandName == "cmdLast")
        {
            Master.ExecuteSync(WgView);
            DataBind();
            Detail.ExecuteSelect(wfvMaster);
            DataBind();
        }
        else if (e.CommandName == "cmdAdd")
        {
            Detail.ExecuteAdd(wfvMaster);
            DataBind();
        }
        else if ((e.CommandName == "cmdDelete" || e.CommandName == "cmdApply" || e.CommandName == "cmdOK")
            && (sender as WebNavigator).State == WebNavigator.NavigatorState.ApplySucess)
        {
            View.Reload();
            DataBind();
            if (e.CommandName == "cmdDelete")
            {
                Master.ExecuteSync(WgView);
                DataBind();
                Detail.ExecuteSelect(wfvMaster);
                DataBind();
            }
        }
    }
    protected void wfvMaster_AfterInsertLocate(object sender, EventArgs e)
    {
        Detail.ExecuteSelect(wfvMaster);
        DataBind();
    }
    protected void WgView_SelectedIndexChanged(object sender, EventArgs e)
    {
        Master.ExecuteSync(WgView);
        DataBind();
        Detail.ExecuteSelect(wfvMaster);
        DataBind();
    }
    protected void wfvMaster_Canceled(object sender, EventArgs e)
    {
        Detail.ExecuteSelect(wfvMaster);
        DataBind();
    }
}
