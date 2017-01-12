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

public partial class WebAccessGroup : System.Web.UI.Page
{
    private Srvtools.WebDataSet WGroup;
    private Srvtools.WebDataSet WUser;
    static string DataBase = "";
    static DataSet ds = new DataSet();
    static Srvtools.WebDataSet user;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            InitializeComponent();
            wdsUser.DataSource = WUser;
            wdsGroup.DataSource = WGroup;
            this.labelMenuID.Text = Session["MenuID"].ToString();
            DataBase = Session["DataBase"].ToString();
            user = WUser;
        }
    }

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WebAccessGroup));
        this.WUser = new Srvtools.WebDataSet();
        this.WGroup = new Srvtools.WebDataSet();
        ((System.ComponentModel.ISupportInitialize)(this.WUser)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.WGroup)).BeginInit();
        // 
        // WUser
        // 
        this.WUser.Active = true;
        this.WUser.AlwaysClose = false;
        this.WUser.Guid = "ef1c5b54-852e-40fc-84d5-c6e1d0497c57";
        this.WUser.LastKeyValues = null;
        this.WUser.PacketRecords = 100;
        this.WUser.Position = -1;
        this.WUser.RemoteName = "GLModule.userInfo";
        this.WUser.ServerModify = false;
        this.WUser.WhereStr = "";
        // 
        // WGroup
        // 
        this.WGroup.Active = true;
        this.WGroup.AlwaysClose = false;
        this.WGroup.Guid = "99cdf311-ab3f-43af-b0b7-443241a6dccf";
        this.WGroup.LastKeyValues = null;
        this.WGroup.PacketRecords = 100;
        this.WGroup.Position = -1;
        this.WGroup.RemoteName = "GLModule.sqlMGroups";
        this.WGroup.ServerModify = false;
        this.WGroup.WhereStr = "";
        ((System.ComponentModel.ISupportInitialize)(this.WUser)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.WGroup)).EndInit();

    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Session.Add("MenuID", this.labelMenuID.Text);
        Session.Add("DataBase", DataBase);
        //Response.Redirect("WebMenuUtilityMain.aspx", true); 
        this.Page.Response.Write("<script language=javascript>window.close();</script>");
    }

    protected void cbUser_DataBound(object sender, EventArgs e)
    {
        CliUtils.fLoginDB = DataBase;
        if (Session["State"].ToString() == "User")
        {
            cbGroup.Visible = false;
            object[] myRet = CliUtils.CallMethod("GLModule", "LoadUsers", new object[] { this.labelMenuID.Text });
            if ((null != myRet) && (0 == (int)myRet[0]))
                ds = (DataSet)(myRet[1]);
            for (int i = 0; i < cbUser.Items.Count; i++)
                for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                {
                    if (cbUser.Items[i].Value == ds.Tables[0].Rows[j]["USERID"].ToString())
                        cbUser.Items[i].Selected = true;
                }
        }
        else
        {
            cbUser.Visible = false;
            object[] myRet = CliUtils.CallMethod("GLModule", "LoadGroups", new object[] { this.labelMenuID.Text });
            if ((null != myRet) && (0 == (int)myRet[0]))
                ds = (DataSet)(myRet[1]);
            for (int i = 0; i < cbGroup.Items.Count; i++)
                for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                {
                    if (cbGroup.Items[i].Value == ds.Tables[0].Rows[j]["GROUPID"].ToString())
                        cbGroup.Items[i].Selected = true;
                }
        }
    }

    protected void btnOK_Click(object sender, EventArgs e)
    {
        if (cbGroup.Visible != false)
        {
            ds.Tables[0].Clear();
            object[] lst = new object[3];
            lst[0] = this.labelMenuID.Text;
            ArrayList strGroupID = new ArrayList();
            ArrayList strMenuID = new ArrayList();
            for (int i = 0; i < cbGroup.Items.Count; i++)
            {
                object[] GetParam = new object[1];
                if (cbGroup.Items[i].Selected == true)
                {

                    strGroupID.Add(cbGroup.Items[i].Value);
                    strMenuID.Add(this.labelMenuID.Text);
                }
            }
            lst[1] = strGroupID;
            lst[2] = strMenuID;

            CliUtils.CallMethod("GLModule", "setGroups", lst);
        }
        else
        {
            ArrayList lstUserID = new ArrayList();
            for (int i = 0; i < cbUser.Items.Count; i++)
                if (cbUser.Items[i].Selected == true)
                {
                    string userID = cbUser.Items[i].Value;
                    lstUserID.Add(userID);
                }

            CliUtils.CallMethod("GLModule", "SetUsers", new object[] { this.labelMenuID.Text, lstUserID });
        }

        Session.Add("MenuID", this.labelMenuID.Text);
        Session.Add("DataBase", DataBase);
        //Response.Redirect("WebMenuUtilityMain.aspx", true);
        this.Page.Response.Write("<script language=javascript>window.close();</script>");

    }

    public string GetUserID(string username)
    {
        string strUserID = "";
        for (int i = 0; i < user.RealDataSet.Tables[0].Rows.Count; i++)
        {
            if (user.RealDataSet.Tables[0].Rows[i]["USERNAME"].ToString() == username)
            {
                strUserID = user.RealDataSet.Tables[0].Rows[i]["USERID"].ToString();
                break;
            }
        }
        return strUserID;
    }
}
