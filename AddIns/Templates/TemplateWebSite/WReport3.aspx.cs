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
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Srvtools;

public partial class WReport3 : System.Web.UI.Page
{
    private Srvtools.WebDataSet WData;


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            InitializeComponent();
            WebDataSource1.DataSource = WData;
            CrystalReportViewer1.DisplayPage = false;
        }
        else
        {
            CrystalReportViewer1.DisplayPage = true;
        }

        WebClientQuery1.Show(Panel1);
    }

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WReport3));
        this.WData = new Srvtools.WebDataSet();
        ((System.ComponentModel.ISupportInitialize)(this.WData)).BeginInit();
        // 
        // WData
        // 
        this.WData.Active = false;
        this.WData.AlwaysClose = true;
        this.WData.Eof = false;
        this.WData.LastKeyValues = null;
        this.WData.PacketRecords = -1;
        this.WData.Position = -1;
        this.WData.RemoteName = "";
        this.WData.ServerModify = false;
        ((System.ComponentModel.ISupportInitialize)(this.WData)).EndInit();

    }
    
    protected void Button1_Click1(object sender, EventArgs e)
    {
        WebClientQuery1.Execute(Panel1,true);
        CrystalReportViewer1.RefreshReport();
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        WebClientQuery1.Clear(Panel1);
        CrystalReportViewer1.DisplayPage = false;
    }
    
}
