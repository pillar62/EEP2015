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

public partial class Template_WMasterDetail4 : System.Web.UI.Page
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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Template_WMasterDetail4));
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
    protected void WebNavigator1_Command(object sender, CommandEventArgs e)
    {
        if (e.CommandName == "cmdFirst" || e.CommandName == "cmdPrevious" 
            || e.CommandName == "cmdNext" || e.CommandName == "cmdLast")
        {
            Detail.ExecuteSelect(wfvMaster); 
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
        }
        else if (e.CommandName == "cmdApply" && wfvMaster.AllValidateSucess)
        {
            Detail.ExecuteSelect(wfvMaster);
            DataBind();
        }
 
    }
    protected void wfvMaster_PageIndexChanged(object sender, EventArgs e)
    {
        Detail.ExecuteSelect(wfvMaster);
        DataBind();
    }
    protected void wfvMaster_AfterInsertLocate(object sender, EventArgs e)
    {
        Detail.ExecuteSelect(wfvMaster);
        DataBind();
    }
    protected void wfvMaster_Canceled(object sender, EventArgs e)
    {
        Detail.ExecuteSelect(wfvMaster);
        DataBind();
    }
    protected void wgvDetail_RowEditing(object sender, GridViewEditEventArgs e)
    {
        wfvDetail.ExecuteSync(new GridViewCommandEventArgs(wgvDetail.Rows[e.NewEditIndex], new CommandEventArgs("Edit", null)));
        WebMultiViewCaptions1.ActiveIndex = 1;
        wfvDetail.ChangeMode(FormViewMode.Edit);
        e.Cancel = true;
    }
    public void SaveDetail(bool ifSave)
    {
        //請將您Detail的FormView的放置的DropDownList,HiddenField的名字放入到wdls列表中，EditTemplate內的就可以了
        if (wfvDetail.CurrentMode == FormViewMode.Edit)
        {
            string[] wdls = new string[] { "DropDownList1", "DropDownList2" };
            foreach (string id in wdls)
            {
                DropDownList list = wfvDetail.FindControl(id) as DropDownList;
                if (list != null)
                {
                    list.EnableViewState = false;
                }
            }
        }
        if (!ifSave)
        {
            wfvDetail.ChangeMode(FormViewMode.ReadOnly);
            if (Master.InnerDataSet.GetChanges() == null)
                WebNavigator1.SetNavState("Browsed");
        }
        else
        {
            if (wfvDetail.CurrentMode == FormViewMode.Edit)
            {
                wfvDetail.UpdateItem(false);
            }
            else if (wfvDetail.CurrentMode == FormViewMode.Insert)
            {
                wfvDetail.InsertItem(true);
            }
        }
        WebMultiViewCaptions1.ActiveIndex = 0;
    }

    protected void WebNavigator1_BeforeCommand(object sender, BeforeCommandArgs e)
    {
        if (e.CommandName == "cmdApply" || e.CommandName == "cmdOK")
        {
            SaveDetail(true);
        }
    }
    protected void ImageButton1_Click1(object sender, ImageClickEventArgs e)
    {
        string stat = WebNavigator1.CurrentNavState;
        if (stat != "Editing" && stat != "Inserting")
        {
            WebNavigator1.SetState(WebNavigator.NavigatorState.Editing);
            WebNavigator1.SetNavState("Editing");
        }
        wfvDetail.ChangeMode(FormViewMode.Insert);
        WebMultiViewCaptions1.ActiveIndex = 1;
    }
    protected void ImageButton10_Click(object sender, ImageClickEventArgs e)
    {
        SaveDetail(true);
        wgvDetail.DataBind();
    }
    protected void ImageButton11_Click(object sender, ImageClickEventArgs e)
    {
        SaveDetail(false);
    }
}
