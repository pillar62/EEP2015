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
    public partial class frmAccessMenus : Form
    {
        InfoDataSet idsGroup;
        string strSelectedGroup;
        string strSelectedGroupName;
        public frmAccessMenus(InfoDataSet dsGroup, string selectedGroup, string selectedGroupName)
        {
            InitializeComponent();
            idsGroup = dsGroup;
            strSelectedGroup = selectedGroup;
            strSelectedGroupName = selectedGroupName;
            lblGroupID.Text = strSelectedGroup;
            lblGroupName.Text = strSelectedGroupName;
        }

        private void frmAccessMenus_Load(object sender, EventArgs e)
        {
            this.idsMenu.Active = true;
            this.idsGroupMenu.WhereStr = string.Format("GROUPID = '{0}'", strSelectedGroup);
            this.idsGroupMenu.Active = true;
            this.idsSolution.Active = true;
            InitialCBGroup();
            ItemToGet();
            tvmenu.ExpandAll();
        }

        private void InitialCBGroup()
        {
            int count = idsGroup.RealDataSet.Tables[0].Rows.Count;
            for(int i = 0; i < count; i ++)
            {
                if (idsGroup.RealDataSet.Tables[0].Rows[i]["GROUPID"].ToString() != strSelectedGroup)
                {
                    cbGroup.Items.Add(idsGroup.RealDataSet.Tables[0].Rows[i]["GROUPID"].ToString());
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
            int count = idsGroupMenu.RealDataSet.Tables[0].Rows.Count;
            for (int i = 0; i < count; i++)
            {
                if (idsGroupMenu.RealDataSet.Tables[0].Rows[i]["GROUPID"].ToString() == strSelectedGroup
                    && idsGroupMenu.RealDataSet.Tables[0].Rows[i]["MENUID"].ToString() == menuid)
                {
                    nodecheck = true;
                    break;
                }
            }
            return nodecheck;
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

            foreach( TreeNode tn in GetAllNode(this.tvmenu))
            {
                if (tn.Nodes.Count == 0)
                {
                    tn.ContextMenuStrip = this.cmsGroup;
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cbSolution_SelectedIndexChanged(object sender, EventArgs e)
        {
            tvmenu.Nodes.Clear();
            this.idsMenu.Active = false;
            this.idsMenu.Active = true;
            ItemToGet();
            tvmenu.ExpandAll();
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            if (cbGroup.Text == string.Empty)
            {
                MessageBox.Show("Select a group to copy menus from.");
                return;
            }
            this.idsGroupMenu.SetWhere(string.Format("GROUPID = '{0}' or GROUPID = '{1}'", strSelectedGroup, cbGroup.Text));
            foreach (TreeNode tn in GetAllNode(tvmenu))
            {
                int count = idsGroupMenu.RealDataSet.Tables[0].Rows.Count;
                for (int i = 0; i < count; i++)
                {
                    if (idsGroupMenu.RealDataSet.Tables[0].Rows[i]["GROUPID"].ToString() == cbGroup.Text
                        && idsGroupMenu.RealDataSet.Tables[0].Rows[i]["MENUID"].ToString() == tn.Name)
                    {
                        tn.Checked = true;
                        break;
                    }
                }
            }           
        }

        private List<Hashtable> GetNodeChecked()
        {
            List<Hashtable> lstnodechecked = new List<Hashtable>();
            foreach( TreeNode tn in GetAllNode(tvmenu))
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
                    foreach( TreeNode tnd in GetAllNode(tn) )
                    {
                        lstnode.Add(tnd);
                    }
                }
            }
            else if( node is TreeNode)
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

        private void btnApply_Click(object sender, EventArgs e)
        {
            
            List<Hashtable> lstnodechecked = GetNodeChecked();
            foreach (Hashtable ht in lstnodechecked)
            {
                if (((bool)ht["checked"]))
                {
                    bool blfind = false;
                   
                    foreach (DataRow dr in idsGroupMenu.RealDataSet.Tables[0].Rows)
                    {
                        if (dr.RowState != DataRowState.Deleted)
                        {
                            if (dr["GROUPID"].ToString() == strSelectedGroup && dr["MENUID"].ToString() == ht["menuid"].ToString())
                            {
                                blfind = true;
                                break;
                            }

                        }
                    }
                    if (!blfind)
                    {
                        DataRow dr = idsGroupMenu.RealDataSet.Tables[0].NewRow();
                        dr["GROUPID"] = strSelectedGroup;
                        dr["MENUID"] = ht["menuid"].ToString();
                        idsGroupMenu.RealDataSet.Tables[0].Rows.Add(dr);
                    }
                }
                else
                {
                    foreach (DataRow dr in idsGroupMenu.RealDataSet.Tables[0].Rows)
                    {
                        if (dr.RowState != DataRowState.Deleted)
                        {
                            if (dr["GROUPID"].ToString() == strSelectedGroup && dr["MENUID"].ToString() == ht["menuid"].ToString())
                            {
                                dr.Delete();
                            }
                        }
                    }
                }        
            }
            idsGroupMenu.ApplyUpdates();
            this.Close();
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
            //MessageBox.Show(this.tvmenu.SelectedNode.Text.ToString());
            string strmenu = tvmenu.SelectedNode.Name;
            string strmenuname = tvmenu.SelectedNode.Text;
            frmAccessControlForGroup fac = new frmAccessControlForGroup(strmenu,strmenuname, strSelectedGroup, strSelectedGroupName);
            fac.ShowDialog();
        }

        private void btnEqual_Click(object sender, EventArgs e)
        {
            if (cbGroup.Text == string.Empty)
            {
                MessageBox.Show("Select a group to copy menus from.");
                return;
            }
            this.idsGroupMenu.SetWhere(string.Format("GROUPID = '{0}' or GROUPID = '{1}'", strSelectedGroup, cbGroup.Text));
            foreach (TreeNode tn in GetAllNode(tvmenu))
            {
                bool nodecheck = false;
                int count = idsGroupMenu.RealDataSet.Tables[0].Rows.Count;
                for (int i = 0; i < count; i++)
                {
                    if (idsGroupMenu.RealDataSet.Tables[0].Rows[i]["GROUPID"].ToString() == cbGroup.Text
                        && idsGroupMenu.RealDataSet.Tables[0].Rows[i]["MENUID"].ToString() == tn.Name)
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