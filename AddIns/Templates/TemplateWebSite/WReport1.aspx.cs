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
public partial class WReport1 : System.Web.UI.Page
{
    private Srvtools.WebDataSet WData;

    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            InitializeComponent();
            WebDataSource1.DataSource = WData;
        }
        CrystalReportViewer1.DisplayPage = true;
    }

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WReport1));
        this.WData = new Srvtools.WebDataSet();
        ((System.ComponentModel.ISupportInitialize)(this.WData)).BeginInit();
        // 
        // WData
        // 
        this.WData.Active = false;
        this.WData.AlwaysClose = false;
        this.WData.Guid = null;
        this.WData.LastKeyValues = null;
        this.WData.PacketRecords = -1;
        this.WData.Position = -1;
        this.WData.RemoteName = "";
        this.WData.ServerModify = false;
        ((System.ComponentModel.ISupportInitialize)(this.WData)).EndInit();

    }

    
}
