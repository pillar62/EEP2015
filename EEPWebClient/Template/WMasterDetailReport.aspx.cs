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

public partial class Template_WMasterDetailReport : System.Web.UI.Page
{
    private Srvtools.WebDataSet WDetail;
    private Srvtools.WebDataSet WMaster;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            InitializeComponent();
            Master.DataSource = WMaster;
        }

        this.WebClientQuery1.Show(this.Panel1);
    }

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Template_WMasterDetailReport));
        this.WMaster = new Srvtools.WebDataSet();
        this.WDetail = new Srvtools.WebDataSet();
        ((System.ComponentModel.ISupportInitialize)(this.WMaster)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.WDetail)).BeginInit();
        // 
        // WMaster
        // 
        this.WMaster.Active = false;
        this.WMaster.AlwaysClose = true;
        this.WMaster.DeleteIncomplete = true;
        this.WMaster.Guid = "522577e4-34b7-402c-801d-0c4256675bfc";
        this.WMaster.LastKeyValues = null;
        this.WMaster.Locale = new System.Globalization.CultureInfo("zh-CN");
        this.WMaster.PacketRecords = 100;
        this.WMaster.Position = -1;
        this.WMaster.RemoteName = null;
        this.WMaster.ServerModify = false;
        // 
        // WDetail
        // 
        this.WDetail.Active = false;
        this.WDetail.AlwaysClose = false;
        this.WDetail.DeleteIncomplete = true;
        this.WDetail.Guid = "77df1b03-e43c-45f8-a8ec-80060a1a60e2";
        this.WDetail.LastKeyValues = null;
        this.WDetail.Locale = new System.Globalization.CultureInfo("zh-CN");
        this.WDetail.PacketRecords = 100;
        this.WDetail.Position = -1;
        this.WDetail.RemoteName = null;
        this.WDetail.ServerModify = false;
        ((System.ComponentModel.ISupportInitialize)(this.WMaster)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.WDetail)).EndInit();

    }

    protected void SubreportProcessing(object sender, Microsoft.Reporting.WebForms.SubreportProcessingEventArgs e)
    {
        e.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("NewDataSet_", Master.InnerDataSet.Tables[1]));
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        WebClientQuery1.Execute(Panel1);
        DataBind();
        this.ReportViewer1.LocalReport.Refresh();
    }


    protected void Button2_Click(object sender, EventArgs e)
    {
        WebClientQuery1.Clear(Panel1);
    }
}
