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

public partial class InnerPages_WebAccessMenu : System.Web.UI.Page
{
    private Srvtools.WebDataSet WUser;
    private Srvtools.WebDataSet WMenu;
    private Srvtools.WebDataSet WUserMenus;
    private WebDataSet WGroupMenus;
    private WebDataSet WGroup;
    private Srvtools.WebDataSet WSolution;
    private static WebDataSet GroupMenus;
    private static WebDataSet UserMenus;
    private static WebDataSet Menu;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            InitializeComponent();
            wdsSolution.DataSource = WSolution;
            GroupMenus = WGroupMenus;
            UserMenus = WUserMenus;
            Menu = WMenu;
            this.ddlSolution.DataBind();

            if (Session["State"] != null)
            {
                if (Session["State"].ToString() == "X")
                {
                    Label1.Visible = false;
                    Label2.Visible = false;
                    ddlCopy.Visible = false;
                    ddlSolution.Visible = false;
                    btnApply.Visible = false;
                    btnEqual.Visible = false;
                    btnCopy.Visible = false;
                    btnSelectAll.Visible = false;
                    btnCancelAll.Visible = false;
                }
                else if (Session["State"].ToString() == "U")
                {
                    String lID = Session["UserID"].ToString();

                    for (int i = 0; i < WUser.RealDataSet.Tables[0].Rows.Count; i++)
                        if (WUser.RealDataSet.Tables[0].Rows[i]["USERID"].ToString() != lID)
                            ddlCopy.Items.Add(WUser.RealDataSet.Tables[0].Rows[i]["USERID"].ToString());


                    Menu.SetWhere("ITEMTYPE='" + this.ddlSolution.SelectedValue + "'");
                    cblMenu.DataSource = Menu;
                    cblMenu.DataValueField = "MENUID";
                    cblMenu.DataTextField = "CAPTION";
                    cblMenu.DataBind();

                    //WUserMenus.SetWhere("USERID='" + lID + "'");
                    for (int i = 0; i < WUserMenus.RealDataSet.Tables[0].Rows.Count; i++)
                    {
                        for (int j = 0; j < cblMenu.Items.Count; j++)
                        {
                            string text = cblMenu.Items[j].Value;

                            if (WUserMenus.RealDataSet.Tables[0].Rows[i]["USERID"].ToString() == lID && WUserMenus.RealDataSet.Tables[0].Rows[i]["MENUID"].ToString() == text)
                            {
                                cblMenu.Items[j].Selected = true;
                                break;
                            }
                        }
                    }
                }
                else if (Session["State"].ToString() == "G")
                {
                    String lID = Session["GroupID"].ToString();

                    for (int i = 0; i < WGroup.RealDataSet.Tables[0].Rows.Count; i++)
                        if (WGroup.RealDataSet.Tables[0].Rows[i]["GROUPID"].ToString() != lID)
                            ddlCopy.Items.Add(WGroup.RealDataSet.Tables[0].Rows[i]["GROUPID"].ToString());

                    Menu.SetWhere("ITEMTYPE='" + this.ddlSolution.SelectedValue + "'");
                    cblMenu.DataSource = Menu;
                    cblMenu.DataValueField = "MENUID";
                    cblMenu.DataTextField = "CAPTION";
                    cblMenu.DataBind();

                    //WGroupMenus.SetWhere("GROUPID='" + lID + "'");
                    for (int i = 0; i < WGroupMenus.RealDataSet.Tables[0].Rows.Count; i++)
                    {
                        for (int j = 0; j < cblMenu.Items.Count; j++)
                        {
                            string text = cblMenu.Items[j].Value;
                            if (WGroupMenus.RealDataSet.Tables[0].Rows[i]["GROUPID"].ToString() == lID && WGroupMenus.RealDataSet.Tables[0].Rows[i]["MENUID"].ToString() == text)
                            {
                                cblMenu.Items[j].Selected = true;
                                break;
                            }
                        }
                    }
                }
            }
        }
    }

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InnerPages_WebAccessMenu));
        this.WSolution = new Srvtools.WebDataSet();
        this.WUser = new Srvtools.WebDataSet();
        this.WMenu = new Srvtools.WebDataSet();
        this.WUserMenus = new Srvtools.WebDataSet();
        this.WGroupMenus = new Srvtools.WebDataSet();
        this.WGroup = new Srvtools.WebDataSet();
        ((System.ComponentModel.ISupportInitialize)(this.WSolution)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.WUser)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.WMenu)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.WUserMenus)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.WGroupMenus)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.WGroup)).BeginInit();
        // 
        // WSolution
        // 
        this.WSolution.Active = true;
        this.WSolution.AlwaysClose = false;
        this.WSolution.Guid = "2611c6b5-e43b-4924-8431-af26e5485624";
        this.WSolution.LastKeyValues = null;
        this.WSolution.PacketRecords = -1;
        this.WSolution.Position = -1;
        this.WSolution.RemoteName = "GLModule.solutionInfo";
        this.WSolution.ServerModify = false;
        this.WSolution.WhereStr = "";
        // 
        // WUser
        // 
        this.WUser.Active = true;
        this.WUser.AlwaysClose = false;
        this.WUser.Guid = "ebeea10f-7b89-48db-990f-886a772e7eb1";
        this.WUser.LastKeyValues = null;
        this.WUser.PacketRecords = -1;
        this.WUser.Position = -1;
        this.WUser.RemoteName = "GLModule.userInfo";
        this.WUser.ServerModify = false;
        this.WUser.WhereStr = "";
        // 
        // WMenu
        // 
        this.WMenu.Active = true;
        this.WMenu.AlwaysClose = false;
        this.WMenu.Guid = "b0c3a6ca-10f9-49ef-90cf-3e4aec5510ab";
        this.WMenu.LastKeyValues = null;
        this.WMenu.PacketRecords = -1;
        this.WMenu.Position = -1;
        this.WMenu.RemoteName = "GLModule.sqlMenus";
        this.WMenu.ServerModify = false;
        this.WMenu.WhereStr = "";
        // 
        // WUserMenus
        // 
        this.WUserMenus.Active = true;
        this.WUserMenus.AlwaysClose = false;
        this.WUserMenus.Guid = "ec1e19a8-8dce-4111-9fa9-a5a822f883cb";
        this.WUserMenus.LastKeyValues = null;
        this.WUserMenus.PacketRecords = -1;
        this.WUserMenus.Position = -1;
        this.WUserMenus.RemoteName = "GLModule.userMenus";
        this.WUserMenus.ServerModify = false;
        this.WUserMenus.WhereStr = "";
        // 
        // WGroupMenus
        // 
        this.WGroupMenus.Active = true;
        this.WGroupMenus.AlwaysClose = false;
        this.WGroupMenus.Guid = "5a4b8cd9-fcb5-4c02-88d5-02b1386b006d";
        this.WGroupMenus.LastKeyValues = null;
        this.WGroupMenus.PacketRecords = -1;
        this.WGroupMenus.Position = -1;
        this.WGroupMenus.RemoteName = "GLModule.sqlMGroupMenus";
        this.WGroupMenus.ServerModify = false;
        this.WGroupMenus.WhereStr = "";
        // 
        // WGroup
        // 
        this.WGroup.Active = true;
        this.WGroup.AlwaysClose = false;
        this.WGroup.Guid = "abbb2fe8-b472-45ad-a6d1-3b098bbdfb53";
        this.WGroup.LastKeyValues = null;
        this.WGroup.PacketRecords = -1;
        this.WGroup.Position = -1;
        this.WGroup.RemoteName = "GLModule.groupInfo";
        this.WGroup.ServerModify = false;
        this.WGroup.WhereStr = "";
        ((System.ComponentModel.ISupportInitialize)(this.WSolution)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.WUser)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.WMenu)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.WUserMenus)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.WGroupMenus)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.WGroup)).EndInit();

    }

    protected void ddlSolution_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Session["State"] != null)
        {
            cblMenu.Items.Clear();

            if (Session["State"].ToString() == "X")
            {
                Label1.Visible = false;
                Label2.Visible = false;
                ddlCopy.Visible = false;
                ddlSolution.Visible = false;
                btnApply.Visible = false;
                btnEqual.Visible = false;
                btnCopy.Visible = false;
                btnSelectAll.Visible = false;
                btnCancelAll.Visible = false;
            }
            else if (Session["State"].ToString() == "U")
            {
                String lID = Session["UserID"].ToString();

                for (int i = 0; i < Menu.RealDataSet.Tables[0].Rows.Count; i++)
                {
                    if (Menu.RealDataSet.Tables[0].Rows[i]["ITEMTYPE"].ToString() == this.ddlSolution.SelectedValue)
                        cblMenu.Items.Add(Menu.RealDataSet.Tables[0].Rows[i]["CAPTION"].ToString()
                                            + "(" + Menu.RealDataSet.Tables[0].Rows[i]["MENUID"].ToString() + ")");
                }

                for (int i = 0; i < UserMenus.RealDataSet.Tables[0].Rows.Count; i++)
                    if (lID == UserMenus.RealDataSet.Tables[0].Rows[i]["USERID"].ToString().Trim())
                        for (int j = 0; j < cblMenu.Items.Count; j++)
                        {
                            string text = cblMenu.Items[j].Text.Substring(cblMenu.Items[j].Text.IndexOf('(') + 1);
                            text = text.Substring(0, text.Length - 1);
                            if (UserMenus.RealDataSet.Tables[0].Rows[i]["MENUID"].ToString() == text)
                            {
                                cblMenu.Items[j].Selected = true;
                                break;
                            }
                        }
            }
            else if (Session["State"].ToString() == "G")
            {
                String lID = Session["GroupID"].ToString();

                for (int i = 0; i < Menu.RealDataSet.Tables[0].Rows.Count; i++)
                {
                    if (Menu.RealDataSet.Tables[0].Rows[i]["ITEMTYPE"].ToString() == this.ddlSolution.SelectedValue)
                        cblMenu.Items.Add(Menu.RealDataSet.Tables[0].Rows[i]["CAPTION"].ToString()
                                        + "(" + Menu.RealDataSet.Tables[0].Rows[i]["MENUID"].ToString() + ")");
                }

                for (int i = 0; i < GroupMenus.RealDataSet.Tables[0].Rows.Count; i++)
                    if (lID == GroupMenus.RealDataSet.Tables[0].Rows[i]["GROUPID"].ToString().Trim())
                        for (int j = 0; j < cblMenu.Items.Count; j++)
                        {
                            string text = cblMenu.Items[j].Text.Substring(cblMenu.Items[j].Text.IndexOf('(') + 1);
                            text = text.Substring(0, text.Length - 1);
                            if (GroupMenus.RealDataSet.Tables[0].Rows[i]["MENUID"].ToString() == text)
                            {
                                cblMenu.Items[j].Selected = true;
                                break;
                            }
                        }
            }
        }
    }
    protected void btnApply_Click(object sender, EventArgs e)
    {
        if (Session["State"] != null && Session["State"].ToString() == "U")
        {
            object[] param = new object[3];
            param[0] = Session["UserID"].ToString();
            for (int i = 0; i < cblMenu.Items.Count; i++)
                if (cblMenu.Items[i].Selected == true)
                {
                    param[1] += cblMenu.Items[i].Value + ";";
                    //string temp = cblMenu.Items[i].Text.Substring(cblMenu.Items[i].Text.IndexOf('('));
                    //temp = temp.Substring(1, temp.Length - 2);
                    //param[1] += temp + ";";
                }
            param[2] = this.ddlSolution.SelectedValue;
            CliUtils.CallMethod("GLModule", "SetUserMenus", param);
        }
        else if (Session["State"] != null && Session["State"].ToString() == "G")
        {
            object[] param = new object[3];
            param[0] = Session["GroupID"].ToString();
            for (int i = 0; i < cblMenu.Items.Count; i++)
                if (cblMenu.Items[i].Selected == true)
                {
                    param[1] += cblMenu.Items[i].Value + ";";
                    //string temp = cblMenu.Items[i].Text.Substring(cblMenu.Items[i].Text.IndexOf('('));
                    //temp = temp.Substring(1, temp.Length - 2);
                    //param[1] += temp + ";";
                }
            param[2] = this.ddlSolution.SelectedValue;
            CliUtils.CallMethod("GLModule", "SetGroupMenus", param);
        }
        this.Page.Response.Write("<script language=javascript>parent.window.close();</script>");
    }

    protected void btnEqual_Click(object sender, EventArgs e)
    {
        if (Session["State"] != null && Session["State"].ToString() == "U")
        {
            if (ddlCopy.Text != string.Empty)
                for (int j = 0; j < cblMenu.Items.Count; j++)
                {
                    bool flag = false;
                    int count = UserMenus.RealDataSet.Tables[0].Rows.Count;
                    string temp = cblMenu.Items[j].Value;
                    //string temp = cblMenu.Items[j].Text.Substring(cblMenu.Items[j].Text.IndexOf('('));
                    //temp = temp.Substring(1, temp.Length - 2);
                    for (int i = 0; i < count; i++)
                        if (UserMenus.RealDataSet.Tables[0].Rows[i]["USERID"].ToString() == ddlCopy.Text
                            && UserMenus.RealDataSet.Tables[0].Rows[i]["MENUID"].ToString() == temp)
                        {
                            flag = true;
                            break;
                        }
                    if (flag == true)
                        cblMenu.Items[j].Selected = true;
                    else
                        cblMenu.Items[j].Selected = false;
                }
        }
        else if (Session["State"] != null && Session["State"].ToString() == "G")
        {
            if (ddlCopy.Text != string.Empty)
                for (int j = 0; j < cblMenu.Items.Count; j++)
                {
                    bool flag = false;
                    int count = GroupMenus.RealDataSet.Tables[0].Rows.Count;
                    string temp = cblMenu.Items[j].Value;
                    //string temp = cblMenu.Items[j].Text.Substring(cblMenu.Items[j].Text.IndexOf('('));
                    //temp = temp.Substring(1, temp.Length - 2);
                    for (int i = 0; i < count; i++)
                        if (GroupMenus.RealDataSet.Tables[0].Rows[i]["GROUPID"].ToString() == ddlCopy.Text
                            && GroupMenus.RealDataSet.Tables[0].Rows[i]["MENUID"].ToString() == temp)
                        {
                            flag = true;
                            break;
                        }
                    if (flag == true)
                        cblMenu.Items[j].Selected = true;
                    else
                        cblMenu.Items[j].Selected = false;
                }
        }
    }

    protected void btnCopy_Click(object sender, EventArgs e)
    {
        if (Session["State"] != null && Session["State"].ToString() == "U")
        {
            if (ddlCopy.Text != string.Empty)
                for (int j = 0; j < cblMenu.Items.Count; j++)
                {
                    int count = UserMenus.RealDataSet.Tables[0].Rows.Count;
                    string temp = cblMenu.Items[j].Value;
                    //string temp = cblMenu.Items[j].Text.Substring(cblMenu.Items[j].Text.IndexOf('('));
                    //temp = temp.Substring(1, temp.Length - 2);
                    for (int i = 0; i < count; i++)
                        if (UserMenus.RealDataSet.Tables[0].Rows[i]["USERID"].ToString() == ddlCopy.Text
                            && UserMenus.RealDataSet.Tables[0].Rows[i]["MENUID"].ToString() == temp)
                        {
                            cblMenu.Items[j].Selected = true;
                            break;
                        }
                }
        }
        else if (Session["State"] != null && Session["State"].ToString() == "G")
        {
            if (ddlCopy.Text != string.Empty)
                for (int j = 0; j < cblMenu.Items.Count; j++)
                {
                    int count = GroupMenus.RealDataSet.Tables[0].Rows.Count;
                    string temp = cblMenu.Items[j].Value;
                    //string temp = cblMenu.Items[j].Text.Substring(cblMenu.Items[j].Text.IndexOf('('));
                    //temp = temp.Substring(1, temp.Length - 2);
                    for (int i = 0; i < count; i++)
                        if (GroupMenus.RealDataSet.Tables[0].Rows[i]["GROUPID"].ToString() == ddlCopy.Text
                            && GroupMenus.RealDataSet.Tables[0].Rows[i]["MENUID"].ToString() == temp)
                        {
                            cblMenu.Items[j].Selected = true;
                            break;
                        }
                }
        }
    }

    protected void btnSelectAll_Click(object sender, EventArgs e)
    {
        for (int j = 0; j < cblMenu.Items.Count; j++)
        {
            cblMenu.Items[j].Selected = true;
        }
    }
    protected void btnCancelAll_Click(object sender, EventArgs e)
    {
        for (int j = 0; j < cblMenu.Items.Count; j++)
        {
            cblMenu.Items[j].Selected = false;
        }
    }
}
