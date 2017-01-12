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

public partial class InnerPages_WebGControl : System.Web.UI.Page
{
    private WebDataSet WUser;
    private Srvtools.WebDataSet WGroup;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            InitializeComponent();
            wdsGroup.DataSource = WGroup;
            //for (int i = 0; i < WUser.RealDataSet.Tables[0].Rows.Count; i++)
            //{
            //    cblUser.Items.Add(WUser.RealDataSet.Tables[0].Rows[i]["UserName"].ToString());
            //    cblUser.Items[i].Text = WUser.RealDataSet.Tables[0].Rows[i]["UserName"].ToString();
            //    cblUser.Items[i].Value = WUser.RealDataSet.Tables[0].Rows[i]["UserID"].ToString();
            //}

            setLanguage();
        }
    }

    private void setLanguage()
    {
        string[] gridHeaders = SysMsg.GetSystemMessage(CliUtils.fClientLang, "Srvtools", "UGControl", "Caption_Group").Split(';');
        if (gridHeaders.Length == 4)
        {
            wgvGroup.Columns[1].HeaderText = gridHeaders[0];
            wgvGroup.Columns[2].HeaderText = gridHeaders[1];
            wgvGroup.Columns[3].HeaderText = gridHeaders[2];
            wgvGroup.Columns[4].HeaderText = gridHeaders[3];
        }
    }

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InnerPages_WebGControl));
        this.WGroup = new Srvtools.WebDataSet();
        this.WUser = new Srvtools.WebDataSet();
        ((System.ComponentModel.ISupportInitialize)(this.WGroup)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.WUser)).BeginInit();
        // 
        // WGroup
        // 
        this.WGroup.Active = true;
        this.WGroup.AlwaysClose = false;
        this.WGroup.DeleteIncomplete = true;
        this.WGroup.Guid = "4c25fe9f-0d55-4823-84f2-9539fbca75d4";
        this.WGroup.LastKeyValues = null;
        this.WGroup.PacketRecords = 100;
        this.WGroup.Position = -1;
        this.WGroup.RemoteName = "GLModule.groupInfo";
        this.WGroup.ServerModify = false;
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
        ((System.ComponentModel.ISupportInitialize)(this.WGroup)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.WUser)).EndInit();

    }

    protected void lbUser_Click(object sender, EventArgs e)
    {
        Response.Redirect("WebUControl.aspx", true);
    }

    protected void wgvGroup_SelectedIndexChanged(object sender, EventArgs e)
    {
        //if (wgvGroup.SelectedIndex > -1)
        //{
        //    for (int i = 0; i < cblUser.Items.Count; i++)
        //        cblUser.Items[i].Selected = false;
        //    object[] param = new object[1];
        //    DataSet dsUsers = new DataSet();
        //    param[0] = wgvGroup.SelectedRow.Cells[1].Text;
        //    object[] myRet = CliUtils.CallMethod("GLModule", "ListUsers", param);
        //    if ((null != myRet) && (0 == (int)myRet[0]))
        //        dsUsers = (DataSet)(myRet[1]);

        //    if (dsUsers.Tables.Count > 0)
        //    {
        //        for (int i = 0; i < dsUsers.Tables[0].Rows.Count; i++)
        //            for (int j = 0; j < cblUser.Items.Count; j++)
        //            {
        //                //string text = cblUser.Items[j].Text.Substring(cblUser.Items[j].Text.IndexOf('('));
        //                //text = text.Substring(0, text.Length - 1);
        //                if (dsUsers.Tables[0].Rows[i]["UserID"].ToString() == cblUser.Items[j].Value)
        //                    cblUser.Items[j].Selected = true;
        //            }
        //    }
        //}
    }

    //protected void btnSave_Click(object sender, EventArgs e)
    //{
    //    if (wgvGroup.SelectedIndex > -1)
    //    {
    //        object[] param = new object[2];
    //        param[0] = wgvGroup.SelectedRow.Cells[1].Text;
    //        for (int i = 0; i < cblUser.Items.Count; i++)
    //            if (cblUser.Items[i].Selected == true)
    //            {
    //                string temp = cblUser.Items[i].Text;
    //                param[1] += temp + ";";
    //            }
    //        CliUtils.CallMethod("GLModule", "SetUserGroups", param);
    //    }
    //}

    protected void btnAccessMenu_Click(object sender, EventArgs e)
    {
        if (this.wgvGroup.SelectedIndex != -1)
        {
            Session.Add("State", "G");
            Session.Add("Index", wgvGroup.SelectedIndex);
            Session.Add("GroupID", wgvGroup.SelectedRow.Cells[1].Text);
            Session.Add("GroupName", wgvGroup.SelectedRow.Cells[2].Text);
            Response.Redirect("WebAccessMenu.aspx", true);
        }
    }

    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        Response.Redirect("../WebMenuUtility.aspx", true);
    }

    private static ArrayList deleteGroupID = new ArrayList();
    protected void wgvGroup_RowDeleted(object sender, GridViewDeletedEventArgs e)
    {
        if (!deleteGroupID.Contains(e.Values["GROUPID"].ToString()))
        {
            deleteGroupID.Add(e.Values["GROUPID"].ToString());
        }
    }

    protected void wnGroup_Command(object sender, CommandEventArgs e)
    {
        if (e.CommandName == "cmdApply")
        {
            for (int i = 0; i < deleteGroupID.Count; i++)
            {
                Srvtools.CliUtils.ExecuteSql("GLModule", "userInfo", "delete GROUPMENUS where GROUPID='" + deleteGroupID[i].ToString() + "'", false, Srvtools.CliUtils.fCurrentProject);
                Srvtools.CliUtils.ExecuteSql("GLModule", "userInfo", "delete GROUPMENUCONTROL where GROUPID='" + deleteGroupID[i].ToString() + "'", false, Srvtools.CliUtils.fCurrentProject);
            }
            deleteGroupID.Clear();
        }
        else if (e.CommandName == "cmdAbort")
        {
            deleteGroupID.Clear();
        }
    }

    protected void btnGetADGroup_Click(object sender, EventArgs e)
    {
        btnGetADGroup.Enabled = false;
        CheckBoxList1.Visible = true;
        btnOK.Visible = true;
        btnCancel.Visible = true;

        btnOK.CommandName = "ADUser";

        this.CheckBoxList1.Items.Clear();

        ArrayList lstGroup = new ArrayList();
        object[] myRet = CliUtils.CallMethod("GLModule", "GetADUserForGroup", new object[] { });
        if ((null != myRet) && (0 == (int)myRet[0]))
        {
            lstGroup = ((ArrayList)myRet[1]);

            foreach (ADGroup group in lstGroup)
            {
                ListItem li = new ListItem();
                li.Text = group.Description;
                li.Value = group.ID;
                CheckBoxList1.Items.Add(li);
            }
        }
    }

    private int GetGroupID()
    {
        int maxid = 0;
        int rowcount = wdsGroup.InnerDataSet.Tables[0].Rows.Count;
        for (int i = 0; i < rowcount; i++)
        {
            string strGroupID = wdsGroup.InnerDataSet.Tables[0].Rows[i]["GROUPID"].ToString();
            if (strGroupID.StartsWith("ad"))
            {
                int id = 0;
                try
                {
                    id = int.Parse(strGroupID.Substring(2));
                }
                catch { }
                maxid = Math.Max(id, maxid);
            }
        }
        return maxid + 1;
    }

    protected void btnOK_Click(object sender, EventArgs e)
    {
        ArrayList lstGroup = new ArrayList();

        object[] myRet = CliUtils.CallMethod("GLModule", "GetADUserForGroup", new object[] { });
        if ((null != myRet) && (0 == (int)myRet[0]))
        {
            lstGroup = ((ArrayList)myRet[1]);
        }

        foreach (ListItem li in this.CheckBoxList1.Items)
        {
            if (li.Selected)
            {
                foreach (ADGroup group in lstGroup)
                {
                    if (group.ID == li.Value)
                    {
                        DataRow drgroup = null;
                        DataRow[] drintables = wdsGroup.InnerDataSet.Tables[0].Select("GROUPNAME='" + group.ID + "'");
                        if (drintables.Length > 0)
                        {
                            //Response.Write("<script>confirm('" + string.Format("Replace {0} infomation by AD defination?", user.ID) + "')</script>");
                            drgroup = drintables[0];
                        }
                        else
                        {
                            drgroup = wdsGroup.InnerDataSet.Tables[0].NewRow();
                            drgroup["GROUPID"] = "ad" + GetGroupID().ToString("000");
                            drgroup["GROUPNAME"] = group.ID;
                            wdsGroup.InnerDataSet.Tables[0].Rows.Add(drgroup);
                        }
                        drgroup["DESCRIPTION"] = group.Description;
                        drgroup["MSAD"] = "Y";

                        foreach (string user in group.Users)
                        {
                            DataRow[] useringroup = wdsGroup.InnerDataSet.Tables[1].Select("GROUPID='" + drgroup["GROUPID"]
                                + "' and USERID='" + user + "'");
                            if (useringroup.Length == 0)
                            {
                                DataRow druser = wdsGroup.InnerDataSet.Tables[1].NewRow();
                                druser["GROUPID"] = drgroup["GROUPID"];
                                druser["USERID"] = user;
                                wdsGroup.InnerDataSet.Tables[1].Rows.Add(druser);
                            }
                        }
                        break;
                    }
                }
            }
        }
        wdsGroup.ApplyUpdates();

        btnGetADGroup.Enabled = true;
        CheckBoxList1.Visible = false;
        btnOK.Visible = false;
        btnCancel.Visible = false;
        wgvGroup.DataBind();
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        btnGetADGroup.Enabled = true;
        CheckBoxList1.Visible = false;
        btnOK.Visible = false;
        btnCancel.Visible = false;
        wgvGroup.DataBind();
    }
}
