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

public partial class Template_WSingle : System.Web.UI.Page
{
    private Srvtools.WebDataSet WMaster;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            InitializeComponent();

            Master.DataSource = WMaster;
            
        }
        WebClientQuery1.Show(Panel1);
    }

    private void InitializeComponent()
    {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Template_WSingle));
            this.WMaster = new Srvtools.WebDataSet();
            ((System.ComponentModel.ISupportInitialize)(this.WMaster)).BeginInit();
            // 
            // WMaster
            // 
            this.WMaster.Active = false;
            this.WMaster.AlwaysClose = true;
            this.WMaster.DataCompressed = false;
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
    protected void WebGridView1_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        WebClientQuery1.Execute(Panel1);
        this.AjaxGridView1.Query(WebClientQuery1, Panel1,UpdatePanel1); 

    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        WebClientQuery1.Clear(Panel1);
    }
}
