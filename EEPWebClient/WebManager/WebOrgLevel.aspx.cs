using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Srvtools;

public partial class WebManager_WebOrgLevel : System.Web.UI.Page
{
    private Srvtools.WebDataSet wOrgKind;
    private Srvtools.WebDataSet wOrgLevel;

    protected void Page_Load(object sender, EventArgs e)
    {
        if(!this.Page.IsPostBack)
        {
            InitializeComponent();

            WDSOrgKind.DataSource = wOrgKind;
            WDSOrgLevel.DataSource = wOrgLevel;

            setLanguage();
        }
    }

    private void setLanguage()
    {
        string[] gridHeaders = SysMsg.GetSystemMessage(CliUtils.fClientLang, "EEPManager", "frmSecurityMain", "GridHeaders").Split(',');
        if (gridHeaders.Length == 6)
        {
            WebGridView1.Columns[1].HeaderText = gridHeaders[2];
            WebGridView1.Columns[2].HeaderText = gridHeaders[3];

            WebGridView2.Columns[1].HeaderText = gridHeaders[4];
            WebGridView2.Columns[2].HeaderText = gridHeaders[5];
        }
    }

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WebManager_WebOrgLevel));
        this.wOrgLevel = new Srvtools.WebDataSet();
        this.wOrgKind = new Srvtools.WebDataSet();
        ((System.ComponentModel.ISupportInitialize)(this.wOrgLevel)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.wOrgKind)).BeginInit();
        // 
        // wOrgLevel
        // 
        this.wOrgLevel.Active = true;
        this.wOrgLevel.AlwaysClose = false;
        this.wOrgLevel.DeleteIncomplete = true;
        this.wOrgLevel.Guid = "018297d2-4e4c-4a5f-adfd-6d59c3ca41d2";
        this.wOrgLevel.LastKeyValues = null;
        this.wOrgLevel.Locale = new System.Globalization.CultureInfo("zh-CN");
        this.wOrgLevel.PacketRecords = 100;
        this.wOrgLevel.Position = -1;
        this.wOrgLevel.RemoteName = "GLModule.cmdOrgLevel";
        this.wOrgLevel.ServerModify = false;
        // 
        // wOrgKind
        // 
        this.wOrgKind.Active = true;
        this.wOrgKind.AlwaysClose = false;
        this.wOrgKind.DeleteIncomplete = true;
        this.wOrgKind.Guid = "669b01fa-ee5b-4b6a-b267-fed3ea68cbe4";
        this.wOrgKind.LastKeyValues = null;
        this.wOrgKind.Locale = new System.Globalization.CultureInfo("zh-CN");
        this.wOrgKind.PacketRecords = 100;
        this.wOrgKind.Position = -1;
        this.wOrgKind.RemoteName = "GLModule.cmdOrgKind";
        this.wOrgKind.ServerModify = false;
        ((System.ComponentModel.ISupportInitialize)(this.wOrgLevel)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.wOrgKind)).EndInit();

    }
}
