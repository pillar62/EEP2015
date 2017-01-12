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
using Srvtools;

public partial class WebMenuUtilityMain : System.Web.UI.Page
{
    private Srvtools.WebDataSet WMaster;

    static string DataBase = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            InitializeComponent();
            if (CliUtils.fLoginDB != "")
                DataBase = CliUtils.fLoginDB;
            else
                DataBase = Session["DataBase"].ToString();
            if (this.Page.Request.QueryString["MenuID"] != null)
                this.tbMenuID.Text = this.Page.Request.QueryString["MenuID"].ToString();
            else if (Session["MenuID"] != null)
                this.tbMenuID.Text = Session["MenuID"].ToString();
            int x = -1;
            for (int i = 0; i < WMaster.RealDataSet.Tables[0].Rows.Count; i++)
                if (WMaster.RealDataSet.Tables[0].Rows[i]["MenuID"].ToString() == tbMenuID.Text)
                {
                    x = i;
                    break;
                }
            if (x != -1)
            {
                this.tbCaption.Text = WMaster.RealDataSet.Tables[0].Rows[x]["Caption"].ToString();
                this.tbParentID.Text = WMaster.RealDataSet.Tables[0].Rows[x]["PARENT"].ToString();
                //this.ddlModuleType.DataSource = WMaster.RealDataSet.Tables[0];
                //this.ddlModuleType.DataTextField = "MODULETYPE";
                //this.ddlModuleType.DataValueField = "MODULETYPE";
                //this.ddlModuleType.DataBind();
                this.ddlModuleType.SelectedValue = WMaster.RealDataSet.Tables[0].Rows[x]["MODULETYPE"].ToString();
                this.tbPackage.Text = WMaster.RealDataSet.Tables[0].Rows[x]["PACKAGE"].ToString();
                this.tbItem.Text = WMaster.RealDataSet.Tables[0].Rows[x]["ITEMPARAM"].ToString();
                this.tbFormName.Text = WMaster.RealDataSet.Tables[0].Rows[x]["FORM"].ToString();
                this.tbSolution.Text = WMaster.RealDataSet.Tables[0].Rows[x]["ITEMTYPE"].ToString();
                this.tbSequence.Text = WMaster.RealDataSet.Tables[0].Rows[x]["SEQ_NO"].ToString();
            }

            setLanguage();
        }
    }

    private void setLanguage()
    {
        string[] strCaptions = SysMsg.GetSystemMessage(CliUtils.fClientLang, "EEPManager", "frmSecurityMain", "MenuCaption").Split(',');
        Label1.Text = strCaptions[0];
        Label2.Text = strCaptions[1];
        Label3.Text = strCaptions[2];
        Label4.Text = strCaptions[3];
        Label5.Text = strCaptions[5];
        Label6.Text = strCaptions[6];
        Label7.Text = strCaptions[7];
        Label8.Text = strCaptions[8];
        Label9.Text = strCaptions[9];
    }

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WebMenuUtilityMain));
        this.WMaster = new Srvtools.WebDataSet();
        ((System.ComponentModel.ISupportInitialize)(this.WMaster)).BeginInit();
        // 
        // WMaster
        // 
        this.WMaster.Active = true;
        this.WMaster.AlwaysClose = false;
        this.WMaster.Guid = "70578348-2e2e-43bb-bd27-37b8d9ab9d73";
        this.WMaster.LastKeyValues = null;
        this.WMaster.PacketRecords = 100;
        this.WMaster.Position = -1;
        this.WMaster.RemoteName = "GLModule.sqlMenus";
        this.WMaster.ServerModify = false;
        this.WMaster.WhereStr = "";
        ((System.ComponentModel.ISupportInitialize)(this.WMaster)).EndInit();

    }

    protected void btnAccessGroup_Click(object sender, EventArgs e)
    {
        Session.Add("State", "Group");
        Session.Add("MenuID", this.tbMenuID.Text);
        Session.Add("DataBase", DataBase);
        //Response.Redirect("WebAccess.aspx", true);
        this.Page.ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('WebAccess.aspx','','width=300,height=480,toolbar=no,scrollbars,resizable');", true);

    }

    protected void btnAccessUser_Click(object sender, EventArgs e)
    {
        Session.Add("State", "User");
        Session.Add("MenuID", this.tbMenuID.Text);
        Session.Add("DataBase", DataBase);
        //Response.Redirect("WebAccess.aspx", true);
        this.Page.ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('WebAccess.aspx','','width=300,height=480,toolbar=no,scrollbars,resizable');", true);
    }

    protected void btnModify_Click(object sender, EventArgs e)
    {
        ItemEnable(true);

        this.Session["ActiveMode"] = "Modify";
        this.btnOK.Enabled = true;
        this.btnCancel.Enabled = true;
        this.btnModify.Enabled = false;
        this.btnAdd.Enabled = false;
        this.btnDelete.Enabled = false;
    }

    private void ItemEnable(bool status)
    {
        this.tbCaption.Enabled = status;
        this.tbParentID.Enabled = status;
        this.ddlModuleType.Enabled = status;
        this.tbPackage.Enabled = status;
        this.tbItem.Enabled = status;
        this.tbFormName.Enabled = status;
        this.tbSolution.Enabled = status;
        this.tbSequence.Enabled = status;
    }

    private void ItemClear()
    {
        this.tbMenuID.Text = "";
        this.tbCaption.Text = "";
        this.tbPackage.Text = "";
        this.tbItem.Text = "";
        this.tbFormName.Text = "";
        this.tbSequence.Text = "";
    }

    protected void btnOK_Click(object sender, EventArgs e)
    {
        String sql = "";

        switch (this.Session["ActiveMode"].ToString())
        {
            case "Modify":
                sql = "update MENUTABLE SET CAPTION='" + this.tbCaption.Text + "',PARENT='" + this.tbParentID.Text + "',MODULETYPE='" + this.ddlModuleType.SelectedValue + "'"
                         + ",PACKAGE='" + this.tbPackage.Text + "',ITEMPARAM='" + this.tbItem.Text + "',FORM='" + this.tbFormName.Text + "',ITEMTYPE='" + this.tbSolution.Text + "'"
                         + ",SEQ_NO='" + this.tbSequence.Text + "' WHERE MENUID='" + this.tbMenuID.Text + "'";
                break;
            case "Add":
                DataSet ds = Srvtools.CliUtils.ExecuteSql("GLModule", "sqlMenus", "select count(*) from MENUTABLE where MENUID='" + this.tbMenuID.Text + "'", true, Srvtools.CliUtils.fCurrentProject);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if( Convert.ToInt16(ds.Tables[0].Rows[0][0]) > 0)
                    {
                        this.Page.Response.Write("<script>alert(\"MenuID you entered is exsited.\");</script>");
                        return;
                    }
                }
                sql = string.Format("insert into MENUTABLE (MENUID,CAPTION,PARENT,MODULETYPE,PACKAGE,ITEMPARAM,FORM,ITEMTYPE,SEQ_NO) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')",
                        new object[] { tbMenuID.Text, tbCaption.Text, tbParentID.Text, ddlModuleType.SelectedValue, tbPackage.Text, tbItem.Text, tbFormName.Text, CliUtils.fCurrentProject, tbSequence.Text });
                break;
            case "Delete":
                sql = "delete MENUTABLE where MENUID='" + this.tbMenuID.Text + "'";
                break;
        }

        Srvtools.CliUtils.ExecuteSql("GLModule", "sqlMenus", sql, false, Srvtools.CliUtils.fCurrentProject);

        this.btnOK.Enabled = false;
        this.btnCancel.Enabled = false;
        this.btnModify.Enabled = true;
        this.btnAdd.Enabled = true;
        this.btnDelete.Enabled = true;
        this.tbMenuID.Enabled = false;

        ItemEnable(false);

        this.Page.Response.Write("<script language=javascript>window.parent.location.reload();</script>");
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        this.Session["ActiveMode"] = "Add";
        this.tbParentID.Text = this.tbMenuID.Text;
        this.tbMenuID.Enabled = true;
        ItemClear();
        ItemEnable(true);
        try
        {
            object[] myRet = CliUtils.CallMethod("GLModule", "AutoSeqMenuID", null);
            if ((null != myRet) && (0 == (int)myRet[0]))
                this.tbMenuID.Text = myRet[0].ToString();
        }
        catch
        {
            this.tbMenuID.Text = "";
        }
        this.btnOK.Enabled = true;
        this.btnCancel.Enabled = true;
        this.btnModify.Enabled = false;
        this.btnAdd.Enabled = false;
        this.btnDelete.Enabled = false;
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        this.tbMenuID.Enabled = false;
        ItemEnable(false);

        this.btnModify.Enabled = true;
        this.btnAdd.Enabled = true;
        this.btnDelete.Enabled = true;
        this.btnOK.Enabled = false;
        this.btnCancel.Enabled = false;
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        this.Session["ActiveMode"] = "Delete";
        
        this.btnDelete.Enabled = false;
        this.btnAdd.Enabled = false;
        this.btnModify.Enabled = false;

        this.btnOK.Enabled = true;
        this.btnCancel.Enabled = true;
    }
}
