using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Runtime.Remoting;
using Srvtools;

namespace EEPManager
{
    public partial class frmSecurityMain : Form
    {
        public frmSecurityMain()
        {
            InitializeComponent();
        }

#if UseFL
        DataTable tabOrg = null;
        bool orgOpFlag = false;

        private void initOrgKind()
        {
            string sqlOrgKind = "select * from SYS_ORGKIND";
            DataTable tabOrgKind = CliUtils.ExecuteSql("GLModule", "cmdDDUse", sqlOrgKind, true, CliUtils.fCurrentProject).Tables[0];
            this.cmbOrgKind.DataSource = tabOrgKind;
            this.cmbOrgKind.ValueMember = "ORG_KIND";
            this.cmbOrgKind.DisplayMember = "KIND_DESC";
        }

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
            this.cmbUpperOrg.ValueMember = "ORG_NO";
            this.cmbUpperOrg.DisplayMember = "ORG_DESC";
            if (o != null)
            {
                this.cmbUpperOrg.SelectedValue = o;
            }
            refreshOrgs(clearValues);
        }

        private void initOrgManager()
        {
            string sqlOrgManager = "select * from GROUPS where ISROLE='Y'";
            DataTable tabOrgManager = CliUtils.ExecuteSql("GLModule", "cmdDDUse", sqlOrgManager, true, CliUtils.fCurrentProject).Tables[0];
            this.cmbOrgManager.DataSource = tabOrgManager;
            this.cmbOrgManager.ValueMember = "GROUPID";
            this.cmbOrgManager.DisplayMember = "GROUPNAME";

            DataTable tabRoles = CliUtils.ExecuteSql("GLModule", "cmdDDUse", sqlOrgManager, true, CliUtils.fCurrentProject).Tables[0];
            this.colRoleId.DataSource = tabRoles;
            this.colRoleId.ValueMember = "GROUPID";
            this.colRoleId.DisplayMember = "GROUPID";
        }

        private void initOrgLevel()
        {
            string sqlOrgLevel = "SELECT * FROM SYS_ORGLEVEL ORDER BY LEVEL_NO";
            DataTable tabOrgLevel = CliUtils.ExecuteSql("GLModule", "cmdDDUse", sqlOrgLevel, true, CliUtils.fCurrentProject).Tables[0];
            this.cmbLevelNo.DataSource = tabOrgLevel;
            this.cmbLevelNo.ValueMember = "LEVEL_NO";
            this.cmbLevelNo.DisplayMember = "LEVEL_DESC";
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
                nodeMain[j].Name = lstRootOrgNo[j];
                nodeMain[j].Tag = lstRootOrgNo[j];
            }
            int p = lstChildOrgNo.Count;
            TreeNode[] nodeChildren = new TreeNode[p];
            for (int q = 0; q < p; q++)
            {
                nodeChildren[q] = new TreeNode();
                nodeChildren[q].Text = lstChildOrgDesc[q];
                nodeChildren[q].Name = lstChildOrgNo[q];
                nodeChildren[q].Tag = lstChildOrgNo[q];
            }
            for (int an = 0; an < p; an++)
            {
                for (int x = 0; x < lstRootOrgNo.Count; x++)
                {
                    if (lstChildUpperOrg[an] == lstRootOrgNo[x])
                    {
                        nodeMain[x].Nodes.Add(nodeChildren[an]);
                    }
                }
                for (int s = 0; s < p; s++)
                {
                    if (lstChildUpperOrg[an].ToString() == lstChildOrgNo[s].ToString())
                    {
                        nodeChildren[s].Nodes.Add(nodeChildren[an]);
                    }
                }
            }
            this.tView.ExpandAll();
            if (clear)
                clearValues();
        }


        private void setLanguage()
        {
            string[] uiTexts = SysMsg.GetSystemMessage(CliUtils.fClientLang, "EEPManager", "frmSecurityMain", "UIText").Split(',');
            if (uiTexts.Length >= 5)
            {
                this.lblOrgNo.Text = uiTexts[0];
                this.lblOrgDesc.Text = uiTexts[1];
                this.lblUpperOrg.Text = uiTexts[2];
                this.lblOrgManager.Text = uiTexts[3];
                this.lblLevelNo.Text = uiTexts[4];
            }
            string[] gridHeaders = SysMsg.GetSystemMessage(CliUtils.fClientLang, "EEPManager", "frmSecurityMain", "GridHeaders").Split(',');
            if (gridHeaders.Length >= 6)
            {
                this.colRoleId.HeaderText = gridHeaders[0];
                this.colGroupName.HeaderText = gridHeaders[1];
                this.colLevelNo.HeaderText = gridHeaders[2];
                this.colLevelDesc.HeaderText = gridHeaders[3];
                this.colOrgKind.HeaderText = gridHeaders[4];
                this.colKindDesc.HeaderText = gridHeaders[5];
            }
        }

        private void cmbOrgKind_SelectedIndexChanged(object sender, EventArgs e)
        {
            initOrg(true);
        }

        private void tView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            string nodeName = e.Node.Name;
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
        }

        private void setComboSelect(object obj, string colName)
        {
            ComboBox cmb = null;
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

        private void txtOrgNo_TextChanged(object sender, EventArgs e)
        {
            //string sqlOrgRoles = "SELECT SYS_ORGROLES.ROLE_ID,GROUPS.GROUPNAME FROM SYS_ORGROLES,GROUPS WHERE SYS_ORGROLES.ORG_NO='" + this.txtOrgNo.Text + "' AND SYS_ORGROLES.ROLE_ID=GROUPS.GROUPID";
            //DataTable tabOrgRoles = CliUtils.ExecuteSql("GLModule", "cmdDDUse", sqlOrgRoles, true, CliUtils.fCurrentProject).Tables[0];
            //bsOrgRoles.DataSource = tabOrgRoles;
            //this.dgvOrgRoles.DataSource = bsOrgRoles;
            if (!this.dsOrgRoles.Active)
                this.dsOrgRoles.Active = true;
            this.dsOrgRoles.SetWhere("SYS_ORGROLES.ORG_NO='" + this.txtOrgNo.Text + "'");
        }

        private void setControlsEnable(bool isEditing)
        {
            this.txtOrgNo.Enabled = isEditing;
            this.txtOrgDesc.Enabled = isEditing;
            this.cmbUpperOrg.Enabled = isEditing;
            this.cmbOrgManager.Enabled = isEditing;
            this.cmbLevelNo.Enabled = isEditing;
            this.btnRoleAdd.Enabled = isEditing;
            this.btnRoleDelete.Enabled = isEditing;
            this.dgvOrgRoles.ReadOnly = !isEditing;

            this.btnReload.Enabled = !isEditing;
            this.btnQuery.Enabled = !isEditing;

            this.btnOrgAdd.Enabled = !isEditing;
            this.btnOrgUpdate.Enabled = !isEditing;
            this.btnOrgDelete.Enabled = !isEditing;
            this.btnOK.Enabled = isEditing;
            this.btnCancel.Enabled = isEditing;

            this.cmbOrgKind.Enabled = !isEditing;
            this.tView.Enabled = !isEditing;
        }

        private void clearValues()
        {
            this.txtOrgNo.Text = "";
            this.txtOrgDesc.Text = "";
            this.cmbUpperOrg.SelectedIndex = -1;
            this.cmbOrgManager.SelectedIndex = -1;
            this.cmbLevelNo.SelectedIndex = -1;
        }

        #region Click
        private void btnReload_Click(object sender, EventArgs e)
        {
            initOrg(true);
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            this.tView.Focus();
            AllNodes.Clear();
            getAllNodes(tView.Nodes);
            this.tView.SelectedNode = LocateNode(this.txtQuery.Text, AllNodes);
        }

        private List<TreeNode> AllNodes = new List<TreeNode>();
        private void getAllNodes(TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            { 
                AllNodes.Add(node);
                if (node.Nodes.Count > 0)
                {
                    getAllNodes(node.Nodes);
                }
            }
        }

        private TreeNode LocateNode(string nodeText, List<TreeNode> nodes)
        {
            foreach (TreeNode node in nodes)
            {
                if (node.Text == nodeText)
                    return node;
            }
            return null;
        }

        private void btnRoleAdd_Click(object sender, EventArgs e)
        {
            this.bsOrgRoles.AddNew();
        }

        private void btnRoleDelete_Click(object sender, EventArgs e)
        {
            if (this.bsOrgRoles.Current != null && this.bsOrgRoles.Current is DataRowView)
            {
                //bool lastPos = false;
                //if (this.bsOrgRoles.Position == this.bsOrgRoles.List.Count - 1 && this.bsOrgRoles.List.Count != 1)
                //{
                //    lastPos = true;
                //}
                this.bsOrgRoles.RemoveCurrent();
                //if (this.bsOrgRoles.List.Count > 0)
                //{
                //    if (!lastPos)
                //    {
                //        this.bsOrgRoles.Position += 1;
                //        this.bsOrgRoles.Position -= 1;
                //    }
                //    else
                //    {
                //        this.bsOrgRoles.Position -= 1;
                //        this.bsOrgRoles.Position += 1;
                //    }
                //}
            }
        }

        private void btnOrgAdd_Click(object sender, EventArgs e)
        {
            setControlsEnable(true);
            clearValues();
            if (this.tView.SelectedNode != null)
                this.cmbUpperOrg.SelectedValue = this.tView.SelectedNode.Name;
            else
                this.cmbUpperOrg.SelectedIndex = -1;
            orgOpFlag = true;
        }

        private void btnOrgUpdate_Click(object sender, EventArgs e)
        {
            setControlsEnable(true);
            orgOpFlag = false;
            this.txtOrgNo.Enabled = false;
        }

        private void btnOrgDelete_Click(object sender, EventArgs e)
        {
            if (this.txtOrgNo.Text == "")
                return;
            if (MessageBox.Show("are you sure to delete?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
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

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (this.txtOrgNo.Text == "" || this.txtOrgDesc.Text == "" || this.cmbOrgKind.SelectedIndex == -1 || this.cmbOrgManager.SelectedIndex == -1 || this.cmbLevelNo.SelectedIndex == -1)
            {
                string message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "EEPManager", "frmSecurityMain", "DataRequire");
                MessageBox.Show(message);
                return;
            }
            if (orgOpFlag)//insert
            {
                string sqlOrgNoExist = "select * from SYS_ORG where ORG_NO='" + this.txtOrgNo.Text + "'";
                DataTable tabOrgNoExist = CliUtils.ExecuteSql("GLModule", "cmdDDUse", sqlOrgNoExist, true, CliUtils.fCurrentProject).Tables[0];
                if (tabOrgNoExist.Rows.Count > 0)
                {
                    MessageBox.Show("org_no is already exist!");
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

            this.dsOrgRoles.ApplyUpdates();
            setControlsEnable(false);
            initOrg(false);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.dsOrgRoles.RealDataSet.RejectChanges();
            setControlsEnable(false);
        }
        #endregion

        private void cmbOrgManager_SelectedIndexChanged(object sender, EventArgs e)
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

        private void dgvOrgRoles_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0 && e.RowIndex != -1)
            {
                string roleid = this.dgvOrgRoles[0, e.RowIndex].Value.ToString();
                object obj = this.dgvOrgRoles.Rows[e.RowIndex].DataBoundItem;
                if (obj != null && obj is DataRowView)
                {
                    DataRowView rowView = (DataRowView)obj;
                    rowView["ORG_NO"] = this.txtOrgNo.Text;
                    rowView["ORG_KIND"] = this.cmbOrgKind.SelectedValue;
                    if (roleid != "")
                    {
                        string sqlRoles = "SELECT GROUPNAME FROM GROUPS WHERE GROUPID='" + roleid + "'";
                        rowView["GROUPNAME"] = CliUtils.ExecuteSql("GLModule", "cmdDDUse", sqlRoles, true, CliUtils.fCurrentProject).Tables[0].Rows[0][0];
                    }
                }
            }
        }
#endif
        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
#if UseFL
            if (tabControl.SelectedIndex == 2)
            {
                initOrgKind();
                initOrgManager();
                initOrgLevel();
                initOrg(true);

                setControlsEnable(false);
                setLanguage();
            }
#endif
        }

        private void frmSecurityMain_Load(object sender, EventArgs e)
        {
#if UseFL       
            SYS_LANGUAGE language = CliUtils.fClientLang;
            String message = String.Format(SysMsg.GetSystemMessage(language, "Srvtools", "UGControl", "PasswordLength"), CliUtils.fPassWordMinSize, CliUtils.fPassWordMaxSize);
            ugControl1.labPassword.Text = message;
            this.ugControl1.SetLoadState();
        

#else
            this.ugControl1.SetLoadState();
#endif
        }
    }
}