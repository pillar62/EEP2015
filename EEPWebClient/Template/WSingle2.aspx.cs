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

public partial class WSingle_WSingle : System.Web.UI.Page
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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WSingle_WSingle));
        this.WMaster = new Srvtools.WebDataSet();
        ((System.ComponentModel.ISupportInitialize)(this.WMaster)).BeginInit();
        // 
        // WMaster
        // 
        this.WMaster.Active = true;
        this.WMaster.AlwaysClose = false;
        this.WMaster.DeleteIncomplete = true;
        this.WMaster.Guid = null;
        this.WMaster.LastKeyValues = null;
        this.WMaster.PacketRecords = 100;
        this.WMaster.Position = -1;
        this.WMaster.RemoteName = null;
        this.WMaster.ServerModify = false;
        ((System.ComponentModel.ISupportInitialize)(this.WMaster)).EndInit();

    }
    protected void WebGridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Select")
        {
            this.WebFormView1.ExecuteSync(e);
        }
    }
    protected void WebNavigator1_Command(object sender, CommandEventArgs e)
    {
        if (e.CommandName == "cmdAdd" || e.CommandName == "cmdUpdate")
        {
            MultiView1.ActiveViewIndex = 1;
            WebMultiViewCaptions1.ActiveIndex = 1;
        }
        WebGridView1.DataBind();
        if (((WebGridView1.SelectedIndex == -1) || (WebGridView1.Rows.Count <= WebGridView1.SelectedIndex)) && (!Master.IsEmpty))
        {
            WebGridView1.SelectedIndex = 0;
        }
        this.WebFormView1.ExecuteSync(new GridViewCommandEventArgs(WebGridView1.SelectedRow, new CommandEventArgs("Select", null)));
    }
}
