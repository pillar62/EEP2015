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
using System.Collections.Generic;

public partial class WebManager_WebOrganization : System.Web.UI.Page
{
    private Srvtools.WebDataSet wOrgRoles;
    private Srvtools.WebDataSet wOrgKind;
    private WebDataSet wGroup;
    private Srvtools.WebDataSet wOrgLevel;

    protected void Page_Load(object sender, EventArgs e)
    {
        string sqlOrg = "select * from SYS_ORG where ORG_KIND='" + this.cmbOrgKind.SelectedValue + "'";
        tabOrg = CliUtils.ExecuteSql("GLModule", "cmdDDUse", sqlOrg, true, CliUtils.fCurrentProject).Tables[0];
        
        if (!this.Page.IsPostBack)
        {
            InitializeComponent();
            WDSOrgKind.DataSource = wOrgKind;
            WDSOrgRoles.DataSource = wOrgRoles;
            WDSOrgLevel.DataSource = wOrgLevel;
            WDSGroup.DataSource = wGroup;

            initOrgKind();
            initOrgManager();
            initOrgLevel();
            initOrg(true);

            setControlsEnable(false);

            cmbOrgManagerSelectedIndexChanged();
            setLanguage();
        }
    }

    private void setLanguage()
    {
        string[] uiTexts = SysMsg.GetSystemMessage(CliUtils.fClientLang, "EEPManager", "frmSecurityMain", "UIText").Split(',');
        if (uiTexts.Length == 5)
        {
            this.lblOrgNo.Text = uiTexts[0];
            this.lblOrgDesc.Text = uiTexts[1];
            this.lblUpperOrg.Text = uiTexts[2];
            this.lblOrgManager.Text = uiTexts[3];
            this.lblLevelNo.Text = uiTexts[4];
        }

        string[] gridHeaders = SysMsg.GetSystemMessage(CliUtils.fClientLang, "EEPManager", "frmSecurityMain", "GridHeaders").Split(',');
        if (gridHeaders.Length == 6)
        {
            this.dgvOrgRoles.Columns[1].HeaderText = uiTexts[0];
            this.dgvOrgRoles.Columns[2].HeaderText = gridHeaders[1];
            this.dgvOrgRoles.Columns[3].HeaderText = gridHeaders[4];
            this.dgvOrgRoles.Columns[4].HeaderText = gridHeaders[1];
        }
    }

    private void initOrgKind()
    {
        string sqlOrgKind = "select * from SYS_ORGKIND";
        DataTable tabOrgKind = CliUtils.ExecuteSql("GLModule", "cmdDDUse", sqlOrgKind, true, CliUtils.fCurrentProject).Tables[0];
        this.cmbOrgKind.DataSource = tabOrgKind;
        this.cmbOrgKind.DataValueField = "ORG_KIND";
        this.cmbOrgKind.DataTextField = "KIND_DESC";
        this.cmbOrgKind.DataBind();
    }

    private void initOrgManager()
    {
        string sqlOrgManager = "select * from GROUPS where ISROLE='Y'";
        DataTable tabOrgManager = CliUtils.ExecuteSql("GLModule", "cmdDDUse", sqlOrgManager, true, CliUtils.fCurrentProject).Tables[0];
        this.cmbOrgManager.DataSource = tabOrgManager;
        this.cmbOrgManager.DataValueField = "GROUPID";
        this.cmbOrgManager.DataTextField = "GROUPNAME";
        this.cmbOrgManager.DataBind();

        //DataTable tabRoles = CliUtils.ExecuteSql("GLModule", "cmdDDUse", sqlOrgManager, true, CliUtils.fCurrentProject).Tables[0];
        //this.colRoleId.DataSource = tabRoles;
        //this.colRoleId.DataValueField = "GROUPID";
        //this.colRoleId.DataTextField = "GROUPID";
        //this.colRoleId
    }

    private void initOrgLevel()
    {
        string sqlOrgLevel = "select * from SYS_ORGLEVEL";
        DataTable tabOrgLevel = CliUtils.ExecuteSql("GLModule", "cmdDDUse", sqlOrgLevel, true, CliUtils.fCurrentProject).Tables[0];
        this.cmbLevelNo.DataSource = tabOrgLevel;
        this.cmbLevelNo.DataValueField = "LEVEL_NO";
        this.cmbLevelNo.DataTextField = "LEVEL_DESC";
        this.cmbLevelNo.DataBind();
    }

    DataTable tabOrg = null;
    bool orgOpFlag = false;
    private void initOrg(bool clearValues)
    {
        object o = null;
        if (this.cmbUpperOrg.SelectedValue != null)
        {
            o = this.cmbUpperOrg.SelectedValue;
        }
        string sqlOrg = "select * from SYS_ORG where ORG_KIND='" + this.cmbOrgKind.SelectedValue + "'";
        tabOrg = CliUtils.ExecuteSql("GLModule", "cmdDDUse", sqlOrg, true, CliUtils.fCurrentProject).Tables[0];
        this.cmbUpperOrg.DataSource = tabOrg;
        this.cmbUpperOrg.DataValueField = "ORG_NO";
        this.cmbUpperOrg.DataTextField = "ORG_DESC";
        this.cmbUpperOrg.DataBind();
        if (o != null)
        {
            this.cmbUpperOrg.SelectedValue = o.ToString();
        }

        refreshOrgs(clearValues);
    }

    private void refreshOrgs(bool clear)
    {
        List<string> lstOrgNo = new List<string>();
        List<string> lstOrgDesc = new List<string>();
        List<string> lstUpperOrg = new List<string>();
        this.tView.Nodes.Clear();
        foreach (DataRow row in tabOrg.Rows)
        {
            lstOrgNo.Add(row["ORG_NO"].ToString());
            lstOrgDesc.Add(row["ORG_DESC"].ToString());
            lstUpperOrg.Add(row["UPPER_ORG"] == null ? "" : row["UPPER_ORG"].ToString());
        }

        List<string> lstRootOrgNo = new List<string>();
        List<string> lstRootOrgDesc = new List<string>();
        List<string> lstChildOrgNo = new List<string>();
        List<string> lstChildOrgDesc = new List<string>();
        List<string> lstChildUpperOrg = new List<string>();
        for (int ix = 0; ix < lstOrgNo.Count; ix++)
        {
            // 以下判断为解决orgNo = upperOrg
            if (lstOrgNo[ix] != lstUpperOrg[ix])
            {
                if (lstUpperOrg[ix] == string.Empty)
                {
                    lstRootOrgNo.Add(lstOrgNo[ix]);
                    lstRootOrgDesc.Add(lstOrgDesc[ix]);
                }
                else
                {
                    lstChildOrgNo.Add(lstOrgNo[ix]);
                    lstChildOrgDesc.Add(lstOrgDesc[ix]);
                    lstChildUpperOrg.Add(lstUpperOrg[ix]);
                }
            }
        }
        int i = lstRootOrgNo.Count;
        TreeNode[] nodeMain = new TreeNode[i];
        for (int j = 0; j < i; j++)
        {
            nodeMain[j] = new TreeNode();
            tView.Nodes.Add(nodeMain[j]);
            nodeMain[j].Text = lstRootOrgDesc[j];
            nodeMain[j].Value = lstRootOrgNo[j];
        }
        int p = lstChildOrgNo.Count;
        TreeNode[] nodeChildren = new TreeNode[p];
        for (int q = 0; q < p; q++)
        {
            nodeChildren[q] = new TreeNode();
            nodeChildren[q].Text = lstChildOrgDesc[q];
            nodeChildren[q].Value = lstChildOrgNo[q];
        }
        for (int an = 0; an < p; an++)
        {
            for (int x = 0; x < lstRootOrgNo.Count; x++)
            {
                if (lstChildUpperOrg[an] == lstRootOrgNo[x])
                {
                    nodeMain[x].ChildNodes.Add(nodeChildren[an]);
                }
            }
            for (int s = 0; s < p; s++)
            {
                if (lstChildUpperOrg[an].ToString() == lstChildOrgNo[s].ToString())
                {
                    nodeChildren[s].ChildNodes.Add(nodeChildren[an]);
                }
            }
        }
        this.tView.ExpandAll();
        this.tView.DataBind();
        if (clear)
            clearValues();
    }

    private void clearValues()
    {
        this.txtOrgNo.Text = "";
        this.txtOrgDesc.Text = "";
        this.cmbUpperOrg.SelectedIndex = -1;
        this.cmbOrgManager.SelectedIndex = -1;
        this.cmbLevelNo.SelectedIndex = -1;
    }

    private void setControlsEnable(bool isEditing)
    {
        this.txtOrgNo.Enabled = isEditing;
        this.txtOrgDesc.Enabled = isEditing;
        this.cmbUpperOrg.Enabled = isEditing;
        this.cmbOrgManager.Enabled = isEditing;
        this.cmbLevelNo.Enabled = isEditing;
        this.dgvOrgRoles.Enabled = isEditing;

        //this.btnReload.Enabled = !isEditing;
        //this.btnQuery.Enabled = !isEditing;

        this.btnOrgAdd.Enabled = !isEditing;
        this.btnOrgUpdate.Enabled = !isEditing;
        this.btnOrgDelete.Enabled = !isEditing;
        this.btnOK.Enabled = isEditing;
        this.btnCancel.Enabled = isEditing;

        this.cmbOrgKind.Enabled = !isEditing;
        this.tView.Enabled = !isEditing;
    }

    protected void tView_SelectedNodeChanged(object sender, EventArgs e)
    {
        string nodeName = this.tView.SelectedNode.Value;
        DataRow[] orgRows = tabOrg.Select("ORG_NO='" + nodeName + "'");
        if (orgRows.Length > 0)
        {
            DataRow row = orgRows[0];
            this.txtOrgNo.Text = row["ORG_NO"].ToString();
            this.txtOrgDesc.Text = row["ORG_DESC"].ToString();
            setComboSelect(row["UPPER_ORG"], "UPPER_ORG");
            setComboSelect(row["ORG_MAN"], "ORG_MAN");
            setComboSelect(row["LEVEL_NO"], "LEVEL_NO");
        }

        cmbOrgManagerSelectedIndexChanged();

        this.WDSOrgRoles.SetWhere("SYS_ORGROLES.ORG_NO='" + this.txtOrgNo.Text + "'");
        this.WDSOrgRoles.DataBind();
        this.dgvOrgRoles.DataBind();
    }

    private void setComboSelect(object obj, string colName)
    {
        DropDownList cmb = null;
        if (colName == "UPPER_ORG")
            cmb = this.cmbUpperOrg;
        else if (colName == "ORG_MAN")
            cmb = this.cmbOrgManager;
        else if (colName == "LEVEL_NO")
            cmb = this.cmbLevelNo;
        if (cmb != null)
        {
            if (obj == null || obj.ToString() == "")
                cmb.SelectedIndex = -1;
            else
                cmb.SelectedValue = obj.ToString();
        }
    }

    protected void cmbOrgManager_SelectedIndexChanged(object sender, EventArgs e)
    {
        cmbOrgManagerSelectedIndexChanged();
    }

    private void cmbOrgManagerSelectedIndexChanged()
    {
        this.lblUser.Text = "";
        string sqlUser = "SELECT USERS.USERID, USERS.USERNAME FROM USERS,USERGROUPS WHERE USERGROUPS.GROUPID='" + this.cmbOrgManager.SelectedValue + "' AND USERS.USERID=USERGROUPS.USERID";
        DataTable tabUser = CliUtils.ExecuteSql("GLModule", "cmdDDUse", sqlUser, true, CliUtils.fCurrentProject).Tables[0];
        if (tabUser.Rows.Count > 0)
        {
            foreach (DataRow row in tabUser.Rows)
            {
                this.lblUser.Text += row["USERID"].ToString() + "(" + row["USERNAME"].ToString() + "),";
            }
            this.lblUser.Text = this.lblUser.Text.Substring(0, this.lblUser.Text.LastIndexOf(','));
        }
    }

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WebManager_WebOrganization));
        this.wOrgRoles = new Srvtools.WebDataSet();
        this.wOrgLevel = new Srvtools.WebDataSet();
        this.wOrgKind = new Srvtools.WebDataSet();
        this.wGroup = new Srvtools.WebDataSet();
        ((System.ComponentModel.ISupportInitialize)(this.wOrgRoles)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.wOrgLevel)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.wOrgKind)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.wGroup)).BeginInit();
        // 
        // wOrgRoles
        // 
        this.wOrgRoles.Active = true;
        this.wOrgRoles.AlwaysClose = true;
        this.wOrgRoles.DeleteIncomplete = true;
        this.wOrgRoles.Guid = "eab74ca2-6eba-4bc6-bb24-d64cd24303dc";
        this.wOrgRoles.LastKeyValues = null;
        this.wOrgRoles.Locale = new System.Globalization.CultureInfo("zh-CN");
        this.wOrgRoles.PacketRecords = 100;
        this.wOrgRoles.Position = -1;
        this.wOrgRoles.RemoteName = "GLModule.cmdOrgRoles";
        this.wOrgRoles.ServerModify = false;
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
        // 
        // wGroup
        // 
        this.wGroup.Active = true;
        this.wGroup.AlwaysClose = false;
        this.wGroup.DeleteIncomplete = true;
        this.wGroup.Guid = "587f0d0d-e292-49e7-b210-151df6fdd05a";
        this.wGroup.LastKeyValues = null;
        this.wGroup.Locale = new System.Globalization.CultureInfo("zh-CN");
        this.wGroup.PacketRecords = 100;
        this.wGroup.Position = -1;
        this.wGroup.RemoteName = "GLModule.groupInfo";
        this.wGroup.ServerModify = false;
        ((System.ComponentModel.ISupportInitialize)(this.wOrgRoles)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.wOrgLevel)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.wOrgKind)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.wGroup)).EndInit();

    }

    protected void btnOrgAdd_Click(object sender, EventArgs e)
    {
        setControlsEnable(true);
        clearValues();
        if (this.tView.SelectedNode != null)
            this.cmbUpperOrg.SelectedValue = this.tView.SelectedNode.Value;
        else
            this.cmbUpperOrg.SelectedIndex = -1;
        orgOpFlag = true;

        this.WDSOrgRoles.SetWhere("1=0");
        this.WDSOrgRoles.DataBind();
        this.dgvOrgRoles.DataBind();

    }
    protected void btnOrgUpdate_Click(object sender, EventArgs e)
    {
        setControlsEnable(true);
        orgOpFlag = false;
        this.txtOrgNo.Enabled = false;
    }
    protected void btnOrgDelete_Click(object sender, EventArgs e)
    {
        if (this.txtOrgNo.Text == "")
            return;
        List<string> orgNums = ChildrenOrgNums(this.txtOrgNo.Text);
        orgNums.Add(this.txtOrgNo.Text);
        string s = "";
        for (int i = 0; i < orgNums.Count; i++)
        {
            if (i == orgNums.Count - 1)
            {
                s += "'" + orgNums[i] + "'";
            }
            else
            {
                s += "'" + orgNums[i] + "',";
            }
        }
        string sqlDeleteOrg = "delete SYS_ORG where ORG_NO in (" + s + ")";
        CliUtils.ExecuteSql("GLModule", "cmdDDUse", sqlDeleteOrg, true, CliUtils.fCurrentProject);
        initOrg(false);
    }
    private List<string> ChildrenOrgNums(string OrgNo)
    {
        List<string> childOrgs = new List<string>();
        foreach (DataRow row in tabOrg.Rows)
        {
            if (row["UPPER_ORG"].ToString() == OrgNo)
            {
                childOrgs.Add(row["ORG_NO"].ToString());
                childOrgs.AddRange(ChildrenOrgNums(row["ORG_NO"].ToString()));
            }
        }
        return childOrgs;
    }
    protected void btnOK_Click(object sender, EventArgs e)
    {
        if (this.txtOrgNo.Text == "" || this.txtOrgDesc.Text == "" || this.cmbOrgKind.SelectedIndex == -1 || this.cmbOrgManager.SelectedIndex == -1 || this.cmbLevelNo.SelectedIndex == -1)
        {
            string message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "EEPManager", "frmSecurityMain", "DataRequire");
            this.Page.Response.Write("<script language='javascript'>alert(\"" + message + "\")</script>");
            return;
        }
        if (orgOpFlag)//insert
        {
            string sqlOrgNoExist = "select * from SYS_ORG where ORG_NO='" + this.txtOrgNo.Text + "'";
            DataTable tabOrgNoExist = CliUtils.ExecuteSql("GLModule", "cmdDDUse", sqlOrgNoExist, true, CliUtils.fCurrentProject).Tables[0];
            if (tabOrgNoExist.Rows.Count > 0)
            {
                this.Page.Response.Write("<script language='javascript'>alert(\"org_no is already exist!\")</script>");
                return;
            }
            else
            {
                string sqlInsertOrg = "insert into SYS_ORG (ORG_NO, ORG_DESC, ORG_KIND, UPPER_ORG, ORG_MAN, LEVEL_NO) values ('" + this.txtOrgNo.Text + "', '" + this.txtOrgDesc.Text + "', '" + this.cmbOrgKind.SelectedValue + "', '" + (this.cmbUpperOrg.SelectedValue ?? string.Empty) + "', '" + this.cmbOrgManager.SelectedValue + "', '" + this.cmbLevelNo.SelectedValue + "')";

                DataSet dsTemp = CliUtils.ExecuteSql("GLModule", "cmdDDUse", sqlInsertOrg, true, CliUtils.fCurrentProject);
                if (dsTemp == null)
                    return;
            }
        }
        else
        {
            string sqlUpdateOrg = "update SYS_ORG set ORG_DESC='" + this.txtOrgDesc.Text + "', UPPER_ORG='" + (this.cmbUpperOrg.SelectedValue ?? string.Empty) + "', ORG_MAN='" + this.cmbOrgManager.SelectedValue + "', LEVEL_NO='" + this.cmbLevelNo.SelectedValue + "' where ORG_NO='" + this.txtOrgNo.Text + "'";
            DataSet dsTemp = CliUtils.ExecuteSql("GLModule", "cmdDDUse", sqlUpdateOrg, true, CliUtils.fCurrentProject);
            if (dsTemp == null)
                return;
        }

        this.WDSOrgRoles.ApplyUpdates();
        setControlsEnable(false);
        initOrg(false);
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        dgvOrgRoles.DataBind();
        cmbOrgManagerSelectedIndexChanged();
        setControlsEnable(false);
    }

    protected void txtOrgNo_TextChanged(object sender, EventArgs e)
    {
        this.WDSOrgRoles.SetWhere("SYS_ORGROLES.ORG_NO='" + this.txtOrgNo.Text.Replace("'", "''") + "'");
        this.WDSOrgRoles.DataBind();
        this.dgvOrgRoles.DataBind();
    }

    public String GetOrgNo()
    {
        return (this.Page.FindControl("txtOrgNo") as TextBox).Text;
    }

    public String GetOrgKind()
    {
        return (this.Page.FindControl("cmbOrgKind") as DropDownList).SelectedValue;
    }

    protected void btnQuery_Click(object sender, EventArgs e)
    {
        string sqlOrgNo = "SELECT ORG_NO FROM SYS_ORG WHERE ORG_DESC like '%" + this.txtQuery.Text.Replace("'", "''") + "%'";
        DataSet dsTemp = CliUtils.ExecuteSql("GLModule", "cmdDDUse", sqlOrgNo, true, CliUtils.fCurrentProject);

        if (dsTemp != null && dsTemp.Tables[0].Rows.Count > 0)
        {
            string nodeName = dsTemp.Tables[0].Rows[0]["ORG_NO"].ToString();
            DataRow[] orgRows = tabOrg.Select("ORG_NO='" + nodeName + "'");
            if (orgRows.Length > 0)
            {
                DataRow row = orgRows[0];
                this.txtOrgNo.Text = row["ORG_NO"].ToString();
                this.txtOrgDesc.Text = row["ORG_DESC"].ToString();
                setComboSelect(row["UPPER_ORG"], "UPPER_ORG");
                setComboSelect(row["ORG_MAN"], "ORG_MAN");
                setComboSelect(row["LEVEL_NO"], "LEVEL_NO");
            }

            cmbOrgManagerSelectedIndexChanged();

            this.WDSOrgRoles.SetWhere("SYS_ORGROLES.ORG_NO='" + this.txtOrgNo.Text + "'");
            this.WDSOrgRoles.DataBind();
            this.dgvOrgRoles.DataBind();
        }
    }
}
