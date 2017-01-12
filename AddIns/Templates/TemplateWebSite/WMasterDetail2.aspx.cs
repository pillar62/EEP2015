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
using System.Collections.Generic;
using Srvtools;

public partial class Template_WMasterDetail2 : System.Web.UI.Page
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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Template_WMasterDetail2));
        this.WMaster = new Srvtools.WebDataSet();
        ((System.ComponentModel.ISupportInitialize)(this.WMaster)).BeginInit();
        // 
        // WMaster
        // 
        this.WMaster.Active = false;
        this.WMaster.AlwaysClose = false;
        this.WMaster.PacketRecords = 100;
        this.WMaster.Position = -1;
        this.WMaster.RemoteName = null;
        this.WMaster.ServerModify = false;
        ((System.ComponentModel.ISupportInitialize)(this.WMaster)).EndInit();

    }

    protected void wdvMaster_PageIndexChanged(object sender, EventArgs e)
    {
        Detail.ExecuteSelect(wdvMaster);
        DataBind();
    }
    protected void wdvMaster_Adding(object sender, EventArgs e)
    {
        Detail.ExecuteAdd(wgvDetail);
        DataBind();
    }
    protected void wdvMaster_AfterInsertLocate(object sender, EventArgs e)
    {
        Detail.ExecuteSelect(wdvMaster);
        DataBind();
    }
    protected void wdvMaster_ItemDeleted(object sender, DetailsViewDeletedEventArgs e)
    {
        Detail.ExecuteSelect(wdvMaster);
        DataBind();
    }
    protected void WebNavigator1_Command(object sender, CommandEventArgs e)
    {
        if (e.CommandName == "cmdFirst" || e.CommandName == "cmdPrevious" 
            || e.CommandName == "cmdNext" || e.CommandName == "cmdLast") 
        { 
            Detail.ExecuteSelect(wdvMaster);
            DataBind(); 
        }
        else if (e.CommandName == "cmdAdd")
        {
            Detail.ExecuteAdd(wdvMaster);
            DataBind();
        }
        else if (e.CommandName == "cmdApply" && wdvMaster.AllValidateSucess)
        {
            Detail.ExecuteSelect(wdvMaster);
            DataBind();
        }
    }
    protected void wdvMaster_Canceled(object sender, EventArgs e)
    {
        Detail.ExecuteSelect(wdvMaster);
        DataBind();
    }
}
