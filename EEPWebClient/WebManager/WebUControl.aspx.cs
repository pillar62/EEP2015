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

public partial class InnerPages_WebUControl : System.Web.UI.Page
{
    private Srvtools.WebDataSet WUser;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            InitializeComponent();
            wdsUser.DataSource = WUser;

            setLanguage();
        }
    }

    private void setLanguage()
    {
        string[] gridHeaders = SysMsg.GetSystemMessage(CliUtils.fClientLang, "Srvtools", "UGControl", "Caption_User").Split(';');
        if (gridHeaders.Length == 10)
        {
            wgvUser.Columns[1].HeaderText = gridHeaders[0];
            wgvUser.Columns[2].HeaderText = gridHeaders[1];
            wgvUser.Columns[3].HeaderText = gridHeaders[2];
            wgvUser.Columns[4].HeaderText = gridHeaders[3];
            wgvUser.Columns[5].HeaderText = gridHeaders[4];
            wgvUser.Columns[6].HeaderText = gridHeaders[5];
            wgvUser.Columns[7].HeaderText = gridHeaders[6];
            wgvUser.Columns[8].HeaderText = gridHeaders[7];
        }
    }

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InnerPages_WebUControl));
        this.WUser = new Srvtools.WebDataSet();
        ((System.ComponentModel.ISupportInitialize)(this.WUser)).BeginInit();
        // 
        // WUser
        // 
        this.WUser.Active = true;
        this.WUser.AlwaysClose = false;
        this.WUser.DeleteIncomplete = true;
        this.WUser.Guid = "10684bb2-5e9c-4385-9b1f-e9c71d4c5b4b";
        this.WUser.LastKeyValues = null;
        this.WUser.PacketRecords = 100;
        this.WUser.Position = -1;
        this.WUser.RemoteName = "GLModule.userInfo";
        this.WUser.ServerModify = false;
        ((System.ComponentModel.ISupportInitialize)(this.WUser)).EndInit();

    }

    protected void btnAccessMenu_Click(object sender, EventArgs e)
    {
        if (this.wgvUser.SelectedIndex != -1)
        {
            Session.Add("State", "U");
            Session.Add("Index", wgvUser.SelectedIndex);
            Session.Add("UserID", wgvUser.SelectedRow.Cells[1].Text);
            Session.Add("UserName", wgvUser.SelectedRow.Cells[2].Text);
            Response.Redirect("WebAccessMenu.aspx", true);
        }
    }

    protected void lbGroup_Click(object sender, EventArgs e)
    {
        Response.Redirect("WebGControl.aspx", true);
    }

    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        Response.Redirect("../WebMenuUtility.aspx", true);
    }

    private static ArrayList deleteUserID = new ArrayList();
    protected void wgvUser_RowDeleted(object sender, GridViewDeletedEventArgs e)
    {
        if (!deleteUserID.Contains(e.Values["USERID"].ToString()))
        {
            deleteUserID.Add(e.Values["USERID"].ToString());
        }
    }

    protected void wnUser_Command(object sender, CommandEventArgs e)
    {
        if (e.CommandName == "cmdApply")
        {
            for (int i = 0; i < deleteUserID.Count; i++)
            {
                Srvtools.CliUtils.ExecuteSql("GLModule", "userInfo", "delete USERMENUS where USERID='" + deleteUserID[i].ToString() + "'", false, Srvtools.CliUtils.fCurrentProject);
                Srvtools.CliUtils.ExecuteSql("GLModule", "userInfo", "delete USERMENUCONTROL where USERID='" + deleteUserID[i].ToString() + "'", false, Srvtools.CliUtils.fCurrentProject);
            }
            deleteUserID.Clear();
        }
        else if (e.CommandName == "cmdAbort")
        {
            deleteUserID.Clear();
        }
    }

    protected void btnGetADUser_Click(object sender, EventArgs e)
    {
        btnGetADUser.Enabled = false;
        CheckBoxList1.Visible = true;
        btnOK.Visible = true;
        btnCancel.Visible = true;

        btnOK.CommandName = "ADUser";

        this.CheckBoxList1.Items.Clear();

        ArrayList lstUser = new ArrayList();
        object[] myRet = CliUtils.CallMethod("GLModule", "GetADUsers", new object[] { });
        if ((null != myRet) && (0 == (int)myRet[0]))
        {
            lstUser = ((ArrayList)myRet[1]);

            foreach (ADUser user in lstUser)
            {
                ListItem li = new ListItem();
                li.Text = user.Name;
                li.Value = user.ID;
                CheckBoxList1.Items.Add(li);
            }
        }
    }

    protected void btnOK_Click(object sender, EventArgs e)
    {
        ArrayList lstUser = new ArrayList();

        object[] myRet = CliUtils.CallMethod("GLModule", "GetADUsers", new object[] { });
        if ((null != myRet) && (0 == (int)myRet[0]))
        {
            lstUser = ((ArrayList)myRet[1]);
        }

        foreach (ListItem li in this.CheckBoxList1.Items)
        {
            if (li.Selected)
            {
                foreach(ADUser user in lstUser)
                {
                    if (user.ID == li.Value)
                    {
                        DataRow druser = null;
                        DataRow[] drintables = wdsUser.InnerDataSet.Tables[0].Select("USERID='" + user.ID.Replace("'", "''") + "'");
                        if (drintables.Length > 0)
                        {
                            //Response.Write("<script>confirm('" + string.Format("Replace {0} infomation by AD defination?", user.ID) + "')</script>");
                            druser = drintables[0];
                        }
                        else
                        {
                            druser = wdsUser.InnerDataSet.Tables[0].NewRow();
                            druser["USERID"] = user.ID;
                            druser["AUTOLOGIN"] = "S";
                            wdsUser.InnerDataSet.Tables[0].Rows.Add(druser);
                        }
                        druser["USERNAME"] = user.Name;
                        druser["DESCRIPTION"] = user.Description;
                        druser["EMAIL"] = user.Email;
                        druser["MSAD"] = "Y";
                        druser["CREATEDATE"] = DateTime.Today.ToShortDateString();
                        break;
                    }
                }
            }
        }
        wdsUser.ApplyUpdates();

        btnGetADUser.Enabled = true;
        CheckBoxList1.Visible = false;
        btnOK.Visible = false;
        btnCancel.Visible = false;
        wgvUser.DataBind();
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        btnGetADUser.Enabled = true;
        CheckBoxList1.Visible = false;
        btnOK.Visible = false;
        btnCancel.Visible = false;
    }
}
