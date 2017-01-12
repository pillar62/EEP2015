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

public partial class Template_WMasterDetail1 : System.Web.UI.Page
{
    private Srvtools.WebDataSet WMaster;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            InitializeComponent();
            Master.DataSource = WMaster;
            Detail.DataSource = WMaster;
            DetailD.DataSource = WMaster;
        }
    }

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Template_WMasterDetail1));
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
        this.WMaster.RemoteName = "";
        this.WMaster.ServerModify = false;
        ((System.ComponentModel.ISupportInitialize)(this.WMaster)).EndInit();

    }
    protected void WebNavigator1_Command(object sender, CommandEventArgs e)
    {
        if (e.CommandName == "cmdFirst" || e.CommandName == "cmdPrevious" 
            || e.CommandName == "cmdNext" || e.CommandName == "cmdLast")
        {
            Detail.ExecuteSelect(wfvMaster); 
            DataBind();
            if (!Detail.IsEmpty && wgvDetail.SelectedIndex == -1)
                wgvDetail.SelectedIndex = 0;
            DetailD.ExecuteSelect(wgvDetail);
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
            if (!Detail.IsEmpty && wgvDetail.SelectedIndex == -1)
                wgvDetail.SelectedIndex = 0;
            DetailD.ExecuteSelect(wgvDetail);
            DataBind();
        }
        else if (e.CommandName == "cmdApply" && wfvMaster.AllValidateSucess)
        {
            Detail.ExecuteSelect(wfvMaster);
            DataBind();
            if (!Detail.IsEmpty && wgvDetail.SelectedIndex == -1)
                wgvDetail.SelectedIndex = 0;
            DetailD.ExecuteSelect(wgvDetail);
            DataBind();
        }
 
    }
    protected void wfvMaster_PageIndexChanged(object sender, EventArgs e)
    {
        Detail.ExecuteSelect(wfvMaster);
        DataBind();
        if (!Detail.IsEmpty && wgvDetail.SelectedIndex == -1)
            wgvDetail.SelectedIndex = 0;
        DetailD.ExecuteSelect(wgvDetail);
        DataBind();
    }
    protected void wfvMaster_AfterInsertLocate(object sender, EventArgs e)
    {
        Detail.ExecuteSelect(wfvMaster);
        DataBind();
        if (!Detail.IsEmpty && wgvDetail.SelectedIndex == -1)
            wgvDetail.SelectedIndex = 0;
        DetailD.ExecuteSelect(wgvDetail);
        DataBind();
    }
    protected void wfvMaster_Canceled(object sender, EventArgs e)
    {
        Detail.ExecuteSelect(wfvMaster);
        DataBind();
        if (!Detail.IsEmpty && wgvDetail.SelectedIndex == -1)
            wgvDetail.SelectedIndex = 0;
        DetailD.ExecuteSelect(wgvDetail);
        DataBind();
    }
    protected void wgvDetail_PageIndexChanged(object sender, EventArgs e)
    {
        if (!Detail.IsEmpty && wgvDetail.SelectedIndex == -1)
            wgvDetail.SelectedIndex = 0;
        DetailD.ExecuteSelect(wgvDetail);
        DataBind();

    }
    protected void wgvDetail_SelectedIndexChanged(object sender, EventArgs e)
    {
        DetailD.ExecuteSelect(wgvDetail);
        DataBind();

    }
    protected void wgvDetail_AfterInsert(object sender, EventArgs e)
    {
        if (wgvDetail.SelectedIndex == -1)
            wgvDetail.SelectedIndex = 0;
        DetailD.ExecuteSelect(wgvDetail);
        DataBind();

    }
}
