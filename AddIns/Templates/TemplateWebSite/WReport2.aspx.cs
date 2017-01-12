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

public partial class WReport2 : System.Web.UI.Page
{
    private Srvtools.WebDataSet WData;


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            InitializeComponent();
            WebDataSource1.DataSource = WData;
            WebDateTimePicker1.Text = CliUtils.GetValue("_FIRSTDAYTY")[1].ToString();
            WebDateTimePicker2.Text = CliUtils.GetValue("_LASTDAY")[1].ToString();
            CrystalReportViewer1.DisplayPage = false;  // first time disible report viewer
        }
        else
        {
            CrystalReportViewer1.DisplayPage = true;
        }
    }

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WReport2));
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
    protected void Button1_Click(object sender, EventArgs e)
    {
        string swhere = "1=0";  // set condiction here
        WebDataSource1.SetWhere(swhere);
        CrystalReportViewer1.RefreshReport(); // reload page

    }
}
