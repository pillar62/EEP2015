using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace Srvtools
{
    public partial class frmAccessMenusForUser : Form
    {
        InfoDataSet idsUser;
        string strSelectedUser;
        string strSelectedUserName;
        public frmAccessMenusForUser(InfoDataSet dsUser, string selectedUser, string selectedUserName)
        {
            InitializeComponent();
            idsUser = dsUser;
            strSelectedUser = selectedUser;
            strSelectedUserName = selectedUserName;
            this.lblUserID.Text = strSelectedUser;
            this.lblUserName.Text = strSelectedUserName;
        }

        private void frmAccessMenusForUser_Load(object sender, EventArgs e)
        {
            this.idsMenu.Active = true;
            this.idsUserMenu.WhereStr = string.Format("USERID = '{0}'", strSelectedUser);
            this.idsUserMenu.Active = true;
            this.idsSolution.Active = true;
            InitialCBUser();
            ItemToGet();
            tvmenu.ExpandAll();
        }


        private void InitialCBUser()
        {
            int count = idsUser.RealDataSet.Tables[0].Rows.Count;
            for (int i = 0; i < count; i++)
            {
                if (idsUser.RealDataSet.Tables[0].Rows[i]["USERID"].ToString() != strSelectedUser)
                {
                    cbUser.Items.Add(idsUser.RealDataSet.Tables[0].Rows[i]["USERID"].ToString());
                }
            }
        }

        private void ItemToGet()
        {
            ArrayList drColFilter = new ArrayList();
            ArrayList MenuIDList = new ArrayList();
            ArrayList CaptionList = new ArrayList();
            ArrayList ParentList = new ArrayList();

            DataRowCollection drColAll = idsMenu.RealDataSet.Tables[0].Rows;
            int rowCount = drColAll.Count;
            for (int m = 0; m < rowCount; m++)
            {
                if (drColAll[m]["itemtype"].ToString() == this.cbSolution.SelectedValue.ToString())
                {
                    drColFilter.Add(drColAll[m]);
                }
            }
            int menuCount = drColFilter.Count;
            for (int i = 0; i < menuCount; i++)
            {
                MenuIDList.Add(((DataRow)drColFilter[i])["menuid"].ToString());
                CaptionList.Add(((DataRow)drColFilter[i])["caption"].ToString());
                ParentList.Add(((DataRow)drColFilter[i])["parent"].ToString());
            }

            initializeTreeView(MenuIDList, CaptionList, ParentList);
        }

        private bool MenuChecked(string menuid)
        {
            bool nodecheck = false;
            int count = idsUserMenu.RealDataSet.Tables[0].Rows.Count;
            for (int i = 0; i < count; i++)
            {
                if (idsUserMenu.RealDataSet.Tables[0].Rows[i]["USERID"].ToString() == strSelectedUser
                    && idsUserMenu.RealDataSet.Tables[0].Rows[i]["MENUID"].ToString() == menuid)
                {
                    nodecheck = true;
                    break;
                }
            }
            return nodecheck;
        }

        private List<Hashtable> GetNodeChecked()
        {
            List<Hashtable> lstnodechecked = new List<Hashtable>();
            foreach (TreeNode tn in GetAllNode(tvmenu))
            {
                Hashtable ht = new Hashtable();
                ht.Add("menuid", tn.Name);
                ht.Add("checked", tn.Checked);
                lstnodechecked.Add(ht);
            }
            return lstnodechecked;
        }

        private List<TreeNode> GetAllNode(object node)
        {
            List<TreeNode> lstnode = new List<TreeNode>();
            if (node is TreeView)
            {
                foreach (TreeNode tn in ((TreeView)node).Nodes)
                {
                    foreach (TreeNode tnd in GetAllNode(tn))
                    {
                        lstnode.Add(tnd);
                    }
                }
            }
            else if (node is TreeNode)
            {
                foreach (TreeNode tn in ((TreeNode)node).Nodes)
                {
                    foreach (TreeNode tnd in GetAllNode(tn))
                    {
                        lstnode.Add(tnd);
                    }
                }
                lstnode.Add((TreeNode)node);

            }
            return lstnode;
        }

        private void initializeTreeView(ArrayList menuIDList, ArrayList menuCaptionList, ArrayList menuParentIDList)
        {
            ArrayList ListMainID = new ArrayList();
            ArrayList ListMainCaption = new ArrayList();
            ArrayList ListChildrenID = new ArrayList();
            ArrayList ListOwnerParentID = new ArrayList();
            ArrayList ListChildrenCaption = new ArrayList();
            int MenuCount = menuIDList.Count;
            for (int ix = 0; ix < MenuCount; ix++)
            {
                // 以下判断为解决menuid = parentid ...
                if (menuIDList[ix].ToString() != menuParentIDList[ix].ToString())
                {
                    if (menuParentIDList[ix].ToString() == string.Empty)
                    {
                        ListMainID.Add(menuIDList[ix].ToString());
                        ListMainCaption.Add(menuCaptionList[ix].ToString());
                    }
                    else
                    {
                        ListChildrenID.Add(menuIDList[ix].ToString());
                        ListOwnerParentID.Add(menuParentIDList[ix].ToString());
                        ListChildrenCaption.Add(menuCaptionList[ix].ToString());
                    }
                }
            }
            int i = ListMainID.Count;
            TreeNode[] nodeMain = new TreeNode[i];
            for (int j = 0; j < i; j++)
            {
                nodeMain[j] = new TreeNode();
                tvmenu.Nodes.Add(nodeMain[j]);
                nodeMain[j].Checked = MenuChecked(ListMainID[j].ToString());
                nodeMain[j].Text = ListMainCaption[j].ToString();
                nodeMain[j].Name = ListMainID[j].ToString();
            }
            int p = ListChildrenID.Count;
            TreeNode[] nodeChildren = new TreeNode[p];
            for (int q = 0; q < p; q++)
            {
                nodeChildren[q] = new TreeNode();
                nodeChildren[q].Checked = MenuChecked(ListChildrenID[q].ToString());
                nodeChildren[q].Text = ListChildrenCaption[q].ToString();
                nodeChildren[q].Name = ListChildrenID[q].ToString();
            }
            for (int an = 0; an < p; an++)
            {
                for (int x = 0; x < ListMainID.Count; x++)
                {
                    if (ListOwnerParentID[an].ToString() == ListMainID[x].ToString())
                    {
                        nodeMain[x].Nodes.Add(nodeChildren[an]);
                    }
                }
                for (int s = 0; s < p; s++)
                {
                    if (ListOwnerParentID[an].ToString() == ListChildrenID[s].ToString())
                    {
                        nodeChildren[s].Nodes.Add(nodeChildren[an]);
                    }
                }
            }
            foreach (TreeNode tn in GetAllNode(this.tvmenu))
            {
                if (tn.Nodes.Count == 0)
                {
                    tn.ContextMenuStrip = this.cmsUser;
                }
            }
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            List<Hashtable> lstnodechecked = GetNodeChecked();
            foreach (Hashtable ht in lstnodechecked)
            {
                if (((bool)ht["checked"]))
                {
                    bool blfind = false;

                    foreach (DataRow dr in idsUserMenu.RealDataSet.Tables[0].Rows)
                    {
                        if (dr.RowState != DataRowState.Deleted)
                        {
                            if (dr["USERID"].ToString() == strSelectedUser && dr["MENUID"].ToString() == ht["menuid"].ToString())
                            {
                                blfind = true;
                                break;
                            }

                        }
                    }
                    if (!blfind)
                    {
                        DataRow dr = idsUserMenu.RealDataSet.Tables[0].NewRow();
                        dr["USERID"] = strSelectedUser;
                        dr["MENUID"] = ht["menuid"].ToString();
                        idsUserMenu.RealDataSet.Tables[0].Rows.Add(dr);
                    }
                }
                else
                {
                    foreach (DataRow dr in idsUserMenu.RealDataSet.Tables[0].Rows)
                    {
                        if (dr.RowState != DataRowState.Deleted)
                        {
                            if (dr["USERID"].ToString() == strSelectedUser && dr["MENUID"].ToString() == ht["menuid"].ToString())
                            {
                                dr.Delete();
                            }
                        }
                    }
                }
            }
            idsUserMenu.ApplyUpdates();
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            if (cbUser.Text == "")
            {
                MessageBox.Show("select a user to copy menus from.");
                return;
            }
            this.idsUserMenu.SetWhere(string.Format("USERID = '{0}' or USERID = '{1}'", strSelectedUser, cbUser.Text));
            foreach (TreeNode tn in GetAllNode(tvmenu))
            {
                int count = idsUserMenu.RealDataSet.Tables[0].Rows.Count;
                for (int i = 0; i < count; i++)
                {
                    if (idsUserMenu.RealDataSet.Tables[0].Rows[i]["USERID"].ToString() == cbUser.Text
                        && idsUserMenu.RealDataSet.Tables[0].Rows[i]["MENUID"].ToString() == tn.Name)
                    {
                        tn.Checked = true;
                        break;
                    }
                }
            }
        }

        private void cbSolution_SelectedIndexChanged(object sender, EventArgs e)
        {
            tvmenu.Nodes.Clear();
            this.idsMenu.Active = false;
            this.idsMenu.Active = true;
            ItemToGet();
            tvmenu.ExpandAll();
        }

        private void tvmenu_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                this.tvmenu.SelectedNode = e.Node;
            }
        }

        private void accessControlsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string strmenu = tvmenu.SelectedNode.Name;
            string strmenuname = tvmenu.SelectedNode.Text;
            frmAccessControlForUser fac = new frmAccessControlForUser(strmenu,strmenuname, strSelectedUser, strSelectedUserName);
            fac.ShowDialog();
        }

        private void btnEqual_Click(object sender, EventArgs e)
        {
            if (cbUser.Text == "")
            {
                MessageBox.Show("select a user to copy menus from.");
                return;
            }
            this.idsUserMenu.SetWhere(string.Format("USERID = '{0}' or USERID = '{1}'", strSelectedUser, cbUser.Text));
            foreach (TreeNode tn in GetAllNode(tvmenu))
            {
                bool nodecheck = false;
                int count = idsUserMenu.RealDataSet.Tables[0].Rows.Count;
                for (int i = 0; i < count; i++)
                {
                    if (idsUserMenu.RealDataSet.Tables[0].Rows[i]["USERID"].ToString() == cbUser.Text
                        && idsUserMenu.RealDataSet.Tables[0].Rows[i]["MENUID"].ToString() == tn.Name)
                    {
                        nodecheck = true;
                        break;
                    }
                }
                tn.Checked = nodecheck;
            }
        }

        private void btnSelAll_Click(object sender, EventArgs e)
        {
            if (tvmenu.SelectedNode == null)
            {
                MessageBox.Show("Please select a node first!");
            }
            else
            {
                tvmenu.SelectedNode.Checked = true;
                SelectAll(tvmenu.SelectedNode.Nodes);
            }
        }

        private void btnCanAll_Click(object sender, EventArgs e)
        {
            if (tvmenu.SelectedNode == null)
            {
                MessageBox.Show("Please select a node first!");
            }
            else
            {
                tvmenu.SelectedNode.Checked = false;
                CanelAll(tvmenu.SelectedNode.Nodes);
            }
        }

        private void SelectAll(TreeNodeCollection tnc)
        {
            foreach (TreeNode tn in tnc)
            {
                tn.Checked = true;
                if (tn.Nodes.Count < 0)
                    SelectAll(tn.Nodes);
            }
        }

        private void CanelAll(TreeNodeCollection tnc)
        {
            foreach (TreeNode tn in tnc)
            {
                tn.Checked = false;
                if (tn.Nodes.Count < 0)
                    CanelAll(tn.Nodes);
            }
        }
    }
}