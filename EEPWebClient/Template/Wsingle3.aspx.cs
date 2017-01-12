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

public partial class Template_Wsingle3 : System.Web.UI.Page
{
    private Srvtools.WebDataSet WMaster;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            InitializeComponent();

            Master.DataSource = WMaster;
        }
    }
    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Template_Wsingle3));
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
        this.WMaster.RemoteName = null;
        this.WMaster.ServerModify = false;
        ((System.ComponentModel.ISupportInitialize)(this.WMaster)).EndInit();

    }
    protected void ButtonOK_Click(object sender, EventArgs e)
    {
        AjaxModalPanel1.Submit();
    }
    protected void buttonClose_Click(object sender, EventArgs e)
    {
        AjaxModalPanel1.Close();
    }
}
