using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Drawing.Imaging;
using System.Data.SqlClient;
using System.IO;

using Microsoft.Win32;
using Srvtools.Report;

namespace Srvtools
{
    [ToolboxBitmap(typeof(MGControl), "Resources.MGControl.png")]
    public partial class MGControl : UserControl
    {
        internal enum ModuleType
        {
            /// <summary>
            /// WinForm
            /// </summary>
            F = 0,
            /// <summary>
            /// WebForm
            /// </summary>
            W = 1,
            /// <summary>
            /// Flow
            /// </summary>
            O = 2,
            /// <summary>
            /// WebClientForm
            /// </summary>
            C = 3,
            ///<summary>
            ///JQueryWebForm
            ///</summary>
            J = 4,
            ///<summary>
            ///JQueryMobileForm
            ///</summary>
            M = 5
        }

        SYS_LANGUAGE language;
        public MGControl()
        {
            InitializeComponent();
            this.btnSelPackage.Paint += new PaintEventHandler(btnSelPackage_Paint);
            //language = CliSysMegLag.GetClientLanguage();
            language = CliUtils.fClientLang;

            DataTable table = new DataTable();
            table.Columns.Add("Value");
            table.Columns.Add("Name");
            table.Rows.Add(new object[] { ModuleType.F, "WinForm" });
            table.Rows.Add(new object[] { ModuleType.W, "WebForm" });
            table.Rows.Add(new object[] { ModuleType.O, "Flow" });
            table.Rows.Add(new object[] { ModuleType.C, "WebClientForm" });
            table.Rows.Add(new object[] { ModuleType.J, "JQueryWebForm" });
            table.Rows.Add(new object[] { ModuleType.M, "JQueryMobileForm" });
            this.cmbModuleType.DataSource = table;
            this.cmbModuleType.DisplayMember = "Name";
            this.cmbModuleType.ValueMember = "Value";
            this.btnSelectReport.Visible = false;
            this.btnSelectReport.Enabled = false;
            this.btnSelectReport.Paint += new System.Windows.Forms.PaintEventHandler(this.btnSelImage_Paint);
        }

        void btnSelPackage_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;
            g.DrawString("...", new Font("SimSun", 7), System.Drawing.Brushes.Black, new Point(11, 8), sf);
        }

        private void btnSelImage_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;
            g.DrawString("...", new Font("SimSun", 7), System.Drawing.Brushes.Black, new Point(11, 8), sf);
        }

        private void btnImageUrl_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;
            g.DrawString("...", new Font("SimSun", 7), System.Drawing.Brushes.Black, new Point(11, 8), sf);
        }

        private ArrayList MenuIDList = new ArrayList();
        private ArrayList CaptionList = new ArrayList();
        private ArrayList ParentList = new ArrayList();
        private TreeNode curNode = new TreeNode();
        private Point Position = new Point(0, 0);

        private void MGControl_Load(object sender, EventArgs e)
        {
            this.infodsSolutions.Active = true;
            //this.infodsGroups.Active = true;
            //this.infodsMenus.SetWhere("PARENT ='' or PARENT is null or PARENT in (select MENUID from MENUTABLE where PARENT ='' or PARENT is null)");
            this.infodsMenus.Active = true;
            this.infodsSolutions.Active = true;
            this.btnApplyTree.Enabled = false;
            EnableControl(false);
            txtItemType.Enabled = false;
            ItemToGet();
            //tView.ExpandAll();
            this.panel1.Visible = false;
            // 初始化image目录（web）
            initialDir = string.Format("{0}\\Image\\MenuTree", EEPRegistry.WebClient.TrimEnd('\\'));
        }

        private void EnableControl(bool bEnable)
        {
            txtMenuID.Enabled = bEnable;
            txtCaption.Enabled = bEnable;
            txtParent.Enabled = bEnable;
            cmbModuleType.Enabled = bEnable;
            txtImageUrl.Enabled = bEnable;////
            btnImageUrl.Enabled = bEnable;
            txtPackage.Enabled = bEnable;
            btnSelPackage.Enabled = bEnable;
            btnSelImage.Enabled = bEnable;
            txtItemParam.Enabled = bEnable && cmbModuleType.SelectedIndex != 2 && cmbModuleType.SelectedIndex != 3;
            btnSelectReport.Enabled = bEnable;
            txtForm.Enabled = bEnable;
            txtSEQ_NO.Enabled = bEnable;
            btnOK.Enabled = bEnable;
            btnCancel.Enabled = bEnable;
            btnAdd.Enabled = !bEnable;
            btnModify.Enabled = !bEnable;
            btnDelete.Enabled = !bEnable;
        }

        private void ClearAllText()
        {
            txtMenuID.Text = string.Empty;
            txtCaption.Text = string.Empty;
            txtParent.Text = string.Empty;
            cmbModuleType.SelectedIndex = -1;
            txtImageUrl.Text = string.Empty;
            txtPackage.Text = string.Empty;
            txtItemParam.Text = string.Empty;
            txtForm.Text = string.Empty;
            pbImage.Image = null;
            txtItemType.Text = string.Empty;
            txtSEQ_NO.Text = string.Empty;
        }

        private void ItemToGet()
        {
            MenuIDList.Clear();
            CaptionList.Clear();
            ParentList.Clear();
            DataRowCollection drColAll = infodsMenus.RealDataSet.Tables[0].Rows;
            ArrayList drColFilter = new ArrayList();
            int rowCount = drColAll.Count;
            for (int m = 0; m < rowCount; m++)
            {
                if (this.infoCmbSolution.SelectedValue != null && drColAll[m]["itemtype"].ToString() == this.infoCmbSolution.SelectedValue.ToString())
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

        private void initializeTreeView(ArrayList menuIDList, ArrayList menuCaptionList, ArrayList menuParentIDList)
        {
            ArrayList ListMainID = new ArrayList();
            ArrayList ListMainCaption = new ArrayList();
            ArrayList ListChildrenID = new ArrayList();
            ArrayList ListOwnerParentID = new ArrayList();
            ArrayList ListChildrenCaption = new ArrayList();
            for (int i = 0; i < menuIDList.Count; i++)
            {
                // 以下判断为解决menuid = parentid ...
                if (MenuIDList[i].ToString() != menuParentIDList[i].ToString())
                {
                    if (menuParentIDList[i].ToString() == string.Empty || menuParentIDList[i].ToString() == " ")
                    {
                        ListMainID.Add(menuIDList[i].ToString());
                        ListMainCaption.Add(menuCaptionList[i].ToString());
                    }
                    else
                    {
                        ListChildrenID.Add(menuIDList[i].ToString());
                        ListOwnerParentID.Add(menuParentIDList[i].ToString());
                        ListChildrenCaption.Add(menuCaptionList[i].ToString());
                    }
                }
            }
            TreeNode[] nodeMain = new TreeNode[ListMainID.Count];
            for (int i = 0; i < ListMainID.Count; i++)
            {
                nodeMain[i] = new TreeNode();
                tView.Nodes.Add(nodeMain[i]);
                nodeMain[i].Text = ListMainCaption[i].ToString();
                nodeMain[i].Name = ListMainID[i].ToString();
                nodeMain[i].Tag = ListMainID[i].ToString();
            }
            TreeNode[] nodeChildren = new TreeNode[ListChildrenID.Count];
            for (int i = 0; i < ListChildrenID.Count; i++)
            {
                nodeChildren[i] = new TreeNode();
                nodeChildren[i].Text = ListChildrenCaption[i].ToString();
                nodeChildren[i].Name = ListChildrenID[i].ToString();
                nodeChildren[i].Tag = ListChildrenID[i].ToString();
            }
            for (int i = 0; i < ListChildrenID.Count; i++)
            {
                for (int j = 0; j < ListMainID.Count; j++)
                {
                    if (ListOwnerParentID[i].ToString() == ListMainID[j].ToString())
                    {
                        nodeMain[j].Nodes.Add(nodeChildren[i]);
                    }
                }
                for (int j = 0; j < ListChildrenID.Count; j++)
                {
                    if (ListOwnerParentID[i].ToString() == ListChildrenID[j].ToString())
                    {
                        nodeChildren[j].Nodes.Add(nodeChildren[i]);
                    }
                }
            }
            for (int i = 0; i < tView.Nodes.Count; i++)
            {
                tView.Nodes[i].Expand();
            }
        }

        private void tView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            curNode = tView.SelectedNode;
            object[] strCaption = new object[3];
            strCaption[0] = tView.SelectedNode.Name;
            strCaption[1] = this.infoCmbSolution.SelectedValue.ToString();

            switch (this.cbLanguageID.Text)
            {
                case "English": strCaption[2] = "0"; break;
                case "Traditional Chinese": strCaption[2] = "1"; break;
                case "Simplified Chinese": strCaption[2] = "2"; break;
                case "HongKong": strCaption[2] = "3"; break;
                case "Japanese": strCaption[2] = "4"; break;
                case "Korean": strCaption[2] = "5"; break;
                case "User-defined1": strCaption[2] = "6"; break;
                case "User-defined2": strCaption[2] = "7"; break;
                case "Default": strCaption[2] = ""; break;
            }

            ArrayList lstParam = null;
            object[] myRet = CliUtils.CallMethod("GLModule", "GetParam", strCaption);
            if ((null != myRet) && (0 == (int)myRet[0]))
                lstParam = ((ArrayList)myRet[1]);

            txtMenuID.Text = lstParam[0].ToString();
            txtCaption.Text = lstParam[1].ToString();
            txtParent.Text = lstParam[2].ToString();
            cmbModuleType.SelectedIndex = (int)Enum.Parse(typeof(ModuleType), lstParam[3].ToString());
            txtImageUrl.Text = lstParam[4].ToString();
            txtPackage.Text = lstParam[5].ToString();
            txtItemParam.Text = lstParam[6].ToString();
            txtForm.Text = lstParam[7].ToString();
            txtItemType.Text = lstParam[8].ToString();
            txtSEQ_NO.Text = lstParam[9].ToString();
            if (txtImageUrl.Text != null && txtImageUrl.Text.Trim() != "")
                preViewImage.Image = Image.FromFile(initialDir + "\\" + txtImageUrl.Text);
            else
                preViewImage.Image = null;
            if ((null != myRet) && (((byte[])myRet[2]).Length > 1))
            {
                byte[] blob = (byte[])myRet[2];

                MemoryStream stmblob = new MemoryStream(blob);

                try
                {
                    pbImage.Image = Image.FromStream(stmblob);
                }
                catch
                {
                    pbImage.Image = null;
                }
            }
            else
            {
                pbImage.Image = null;
            }
            this.EnableControl(false);
        }

        //private void btnAddToGroups_Click(object sender, EventArgs e)
        //{
        //    string menuID = txtMenuID.Text;
        //    string menuName = txtCaption.Text;
        //    Form frmSelGroups = new frmSelGroups(menuID,menuName);
        //    frmSelGroups.ShowDialog();
        //}

        private void ItemAdd_Click(object sender, EventArgs e)
        {
            doAdd();
        }

        private void doAdd()
        {
            string strTempParent = txtMenuID.Text;
            int moduletypeparent = this.cmbModuleType.SelectedIndex;
            ClearAllText();
            this.cmbModuleType.SelectedIndex = moduletypeparent;//wait test
            try
            {
                object[] myRet = CliUtils.CallMethod("GLModule", "AutoSeqMenuID", null);
                if ((null != myRet) && (0 == (int)myRet[0]))
                    txtMenuID.Text = ((int)myRet[1]).ToString();
            }
            catch
            {
                txtMenuID.Text = "";
            }
            txtParent.Text = strTempParent;
            txtItemType.Text = this.infoCmbSolution.SelectedValue.ToString();
            txtForm.Text = "Form1";
            curNode = tView.SelectedNode;
            EnableControl(true);
            btnReloadTree.Enabled = false;
            infoCmbSolution.Enabled = false;
            btnGroups.Enabled = false;
            btnUsers.Enabled = false;
            tView.Enabled = false;
            this.cbLanguageID.SelectedIndexChanged -= this.cbLanguageID_SelectedIndexChanged;
            this.cbLanguageID.Text = "Default";
            this.cbLanguageID.SelectedIndexChanged += this.cbLanguageID_SelectedIndexChanged;
            this.cbLanguageID.Enabled = false;
            optype = OpType.add;
            iconpath = null;
        }

        private void ItemModify_Click(object sender, EventArgs e)
        {
            if (txtMenuID.Text != null && txtMenuID.Text != "")
                doModify();
        }

        private void doModify()
        {
            curNode = tView.SelectedNode;
            EnableControl(true);
            //txtCaption.Enabled = false;
            txtMenuID.Enabled = false;
            btnReloadTree.Enabled = false;
            btnGroups.Enabled = false;
            btnUsers.Enabled = false;
            infoCmbSolution.Enabled = false;
            tView.Enabled = false;
            optype = OpType.modify;
            iconpath = null;
        }

        private void ItemDelete_Click(object sender, EventArgs e)
        {
            if (txtMenuID.Text != null && txtMenuID.Text != "")
                doDelete();
        }

        private void doDelete()
        {
            //if (MessageBox.Show("Are you sure to delete the current menu??", "confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            //{
            if (tView.SelectedNode.Nodes.Count == 0)
            {
                EnableControl(false);
                this.btnOK.Enabled = true;
                this.btnCancel.Enabled = true;
                this.btnReloadTree.Enabled = false;
                this.btnAdd.Enabled = false;
                this.btnModify.Enabled = false;
                this.btnDelete.Enabled = false;
                btnGroups.Enabled = false;
                btnUsers.Enabled = false;
                optype = OpType.delete;
                this.tView.Enabled = false;
                this.infoCmbSolution.Enabled = false;
            }
            else
            {
                MessageBox.Show("ParentNode can not be deleted when childnodes exist");
            }

            //}
        }

        public enum OpType { add, modify, delete }

        private OpType optype;
        private string iconpath = null;
        private void btnOK_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    Convert.ToInt32(txtMenuID.Text);
            //}
            //catch
            //{
            //    MessageBox.Show("MenuID has to be NUMBER!");
            //    return;
            //}

            //new add by ccm
            if (cmbModuleType.SelectedIndex == -1 || txtCaption.Text == string.Empty)
            {
                if (cmbModuleType.SelectedIndex == -1 && txtCaption.Text == string.Empty)
                {
                    MessageBox.Show("ModuleType and Caption can't be null");
                }
                else if (cmbModuleType.SelectedIndex == -1)
                {
                    MessageBox.Show("ModuleType can't be null");
                }
                else
                {
                    MessageBox.Show("Caption can't be null");
                }
                return;
            }
            //add by ccm to solve the problem of set the childnode as parent
            for (int i = 0; i < MenuIDList.Count; i++)
            {
                if (MenuIDList[i].ToString() == txtParent.Text.Trim())
                {
                    if (ParentList[i].ToString() == txtMenuID.Text.Trim())
                    {
                        MessageBox.Show("Can not set the childnode as parent");
                        return;
                    }
                }
            }

            EnableControl(false);
            btnReloadTree.Enabled = true;
            tView.Enabled = true;
            infoCmbSolution.Enabled = true;
            this.cbLanguageID.Enabled = true;
            btnGroups.Enabled = true;
            btnUsers.Enabled = true;

            object[] strAddParam = new object[13];
            //new add by ccm

            if (optype != OpType.delete && iconpath != null && File.Exists(iconpath))
            {

                FileStream fs = new FileStream(openFileDialog2.FileName, FileMode.Open, FileAccess.Read);
                Byte[] blob = new Byte[fs.Length];
                fs.Read(blob, 0, blob.Length);

                strAddParam[10] = blob;
            }
            strAddParam[0] = (txtMenuID.Text == string.Empty) ? null : txtMenuID.Text;
            strAddParam[1] = txtCaption.Text;
            //strAddParam[1] = "";
            strAddParam[2] = (txtParent.Text == string.Empty) ? null : txtParent.Text;
            strAddParam[3] = cmbModuleType.SelectedValue.ToString();
            strAddParam[4] = (txtPackage.Text == string.Empty) ? null : txtPackage.Text;
            strAddParam[5] = (txtItemParam.Text == string.Empty) ? null : txtItemParam.Text;
            strAddParam[6] = (txtForm.Text == string.Empty) ? null : txtForm.Text;
            strAddParam[7] = (txtItemType.Text == string.Empty) ? null : txtItemType.Text;
            if (txtSEQ_NO.Text != string.Empty && txtSEQ_NO.Text.Length == 1 && txtSEQ_NO.Text[0] < 58 && txtSEQ_NO.Text[0] > 48)
                txtSEQ_NO.Text = '0' + txtSEQ_NO.Text;
            strAddParam[8] = (txtSEQ_NO.Text == string.Empty) ? null : txtSEQ_NO.Text;
            strAddParam[9] = optype;
            strAddParam[11] = (txtImageUrl.Text == string.Empty) ? null : txtImageUrl.Text;


            switch (this.cbLanguageID.Text)
            {
                case "English": strAddParam[12] = "0"; break;
                case "Traditional Chinese": strAddParam[12] = "1"; break;
                case "Simplified Chinese": strAddParam[12] = "2"; break;
                case "HongKong": strAddParam[12] = "3"; break;
                case "Japanese": strAddParam[12] = "4"; break;
                case "Korean": strAddParam[12] = "5"; break;
                case "User-defined1": strAddParam[12] = "6"; break;
                case "User-defined2": strAddParam[12] = "7"; break;
                case "Default": strAddParam[12] = ""; break;
            }

            if (optype == OpType.add)
            {
                int i = this.infodsMenus.RealDataSet.Tables[0].Rows.Count;
                for (int j = 0; j < i; j++)
                {
                    if (this.infodsMenus.RealDataSet.Tables[0].Rows[j]["menuid"].ToString() == strAddParam[0].ToString())
                    {
                        MessageBox.Show("menuid already existed !");
                        curNode = tView.SelectedNode;
                        EnableControl(true);
                        //txtCaption.Enabled = false;
                        btnReloadTree.Enabled = false;
                        btnGroups.Enabled = false;
                        btnUsers.Enabled = false;
                        infoCmbSolution.Enabled = false;
                        tView.Enabled = false;
                        optype = OpType.add;
                        iconpath = null;
                        this.txtParent.Text = strAddParam[2].ToString();
                        return;
                    }
                }
                //TreeNode addNode = new TreeNode(strAddParam[1].ToString());
                //if (curNode != null)
                //    curNode.Nodes.Add(addNode);
                //else
                //    this.tView.Nodes.Add(addNode);
            }
            CliUtils.CallMethod("GLModule", "OPMenu", strAddParam);
            //this.ClearAllText();

            //new add 为了解修改parent后直接刷新的问题 == refresh
            //tView.Nodes.Clear();
            //this.infodsMenus.Active = false;
            //this.infodsMenus.Active = true;
            //ItemToGet();
            //tView.ExpandAll();
            if (optype == OpType.add)
            {
                if (txtParent.Text.Length > 0)
                {
                    TreeNode[] nodeparent = tView.Nodes.Find(txtParent.Text, true);
                    if (nodeparent.Length > 0)
                    {
                        nodeparent[0].Nodes.Add(txtMenuID.Text, txtCaption.Text);
                        nodeparent[0].Expand();
                    }
                }
                else
                {
                    tView.Nodes.Add(txtMenuID.Text, txtCaption.Text);
                }
            }
            else if (optype == OpType.modify)
            {
                TreeNode[] node = tView.Nodes.Find(txtMenuID.Text, true);
                if (node.Length > 0)
                {
                    if (string.Compare(cbLanguageID.Text, "Default") == 0)
                    {
                        node[0].Text = txtCaption.Text;
                    }
                    string parentname = (node[0].Parent == null) ? string.Empty : node[0].Parent.Name;
                    if (string.Compare(parentname, txtParent.Text) != 0)
                    {
                        if (txtParent.Text.Length == 0)
                        {
                            node[0].Parent.Nodes.Remove(node[0]);
                            tView.Nodes.Add(node[0]);
                        }
                        else
                        {
                            TreeNode[] nodeparent = tView.Nodes.Find(txtParent.Text, true);
                            if (parentname.Length == 0)
                            {
                                tView.Nodes.Remove(node[0]);
                            }
                            else
                            {
                                node[0].Parent.Nodes.Remove(node[0]);
                            }
                            if (nodeparent.Length > 0)
                            {
                                nodeparent[0].Nodes.Add(node[0]);
                            }
                        }
                    }
                }
            }
            else
            {
                TreeNode[] node = tView.Nodes.Find(txtMenuID.Text, true);
                if (node.Length > 0)
                {
                    if (node[0].Parent != null)
                    {
                        node[0].Parent.Nodes.Remove(node[0]);
                    }
                    else
                    {
                        tView.Nodes.Remove(node[0]);
                    }
                }
            }
            ////////////////
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            EnableControl(false);
            btnReloadTree.Enabled = true;
            tView.Enabled = true;
            this.infoCmbSolution.Enabled = true;
            this.cbLanguageID.Enabled = true;
            btnGroups.Enabled = true;
            btnUsers.Enabled = true;
            //this.ClearAllText();
            //new add 为了解删除node后直接刷新的问题 == refresh
            //tView.Nodes.Clear();
            //this.infodsMenus.Active = false;
            //this.infodsMenus.Active = true;
            //ItemToGet();
            //tView.ExpandAll();
            //////////////////
        }

        private void tView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            DoDragDrop(e.Item, DragDropEffects.Move);
        }

        private void tView_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(TreeNode)))
                e.Effect = DragDropEffects.Move;
            else
                e.Effect = DragDropEffects.None;
        }

        private void tView_DragDrop(object sender, DragEventArgs e)
        {
            TreeNode myNode = null;
            if (e.Data.GetDataPresent(typeof(TreeNode)))
            {
                myNode = (TreeNode)(e.Data.GetData(typeof(TreeNode)));
            }
            else
            {
                MessageBox.Show("error");
            }
            Position.X = e.X;
            Position.Y = e.Y;
            Position = tView.PointToClient(Position);
            TreeNode DropNode = this.tView.GetNodeAt(Position);
            if (DropNode != null && DropNode.Parent != myNode && DropNode != myNode)
            {
                TreeNode DragNode = myNode;
                myNode.Remove();
                DropNode.Nodes.Add(DragNode);
            }
            if (DropNode == null)
            {
                TreeNode DragNode = myNode;
                myNode.Remove();
                tView.Nodes.Add(DragNode);
            }
            this.btnApplyTree.Enabled = true;
        }

        private void ItemGroups_Click(object sender, EventArgs e)
        {
            string menuID = txtMenuID.Text;
            string menuName = txtCaption.Text;
            Form frmSelGroups = new frmSelGroups(menuID, menuName);
            frmSelGroups.ShowDialog();
        }

        private void btnReloadTree_Click(object sender, EventArgs e)
        {
            tView.Nodes.Clear();
            this.infodsMenus.Active = false;
            this.infodsMenus.Active = true;
            ClearAllText();
            ItemToGet();
            //tView.ExpandAll();
        }

        private void _GetAllNodes(TreeNode aNode, ref ArrayList myList)
        {
            for (int i = 0; i < aNode.Nodes.Count; i++)
            {
                myList.Add(aNode.Nodes[i]);
                if (aNode.Nodes[i].Nodes.Count > 0)
                    _GetAllNodes(aNode.Nodes[i], ref myList);
            }
        }

        private ArrayList myList = new ArrayList();
        private ArrayList GetAllNodes(TreeView tree)
        {
            for (int i = 0; i < tree.Nodes.Count; i++)
            {
                myList.Add(tree.Nodes[i]);
                _GetAllNodes(tree.Nodes[i], ref myList);
            }

            return myList;
        }

        private void btnApplyTree_Click(object sender, EventArgs e)
        {
            ArrayList allNode = GetAllNodes(tView);

            ArrayList MenuIDList = new ArrayList();
            ArrayList ParentList = new ArrayList();

            //get each node's id and parent
            for (int i = 0; i < allNode.Count; i++)
            {
                //制作menuID的集合
                tView.SelectedNode = (TreeNode)allNode[i];
                string strID = txtMenuID.Text;
                MenuIDList.Add(strID);

                //制作parent的集合
                if (((TreeNode)allNode[i]).Parent == null)
                {
                    ParentList.Add(null);
                }
                else
                {
                    tView.SelectedNode = ((TreeNode)allNode[i]).Parent;
                    string strTempID = txtMenuID.Text;
                    ParentList.Add(strTempID);
                }
            }
            //update
            for (int j = 0; j < MenuIDList.Count; j++)
            {
                object[] param = new object[2];
                param[0] = MenuIDList[j];
                param[1] = ParentList[j];
                CliUtils.CallMethod("GLModule", "UpdateNodes", param);
            }
            this.btnApplyTree.Enabled = false;
        }

        private void infoCmbSolution_SelectedIndexChanged(object sender, EventArgs e)
        {
            tView.Nodes.Clear();
            EnableControl(false);
            this.ClearAllText();
            this.infodsMenus.Active = false;
            this.infodsMenus.Active = true;
            ItemToGet();
            //tView.ExpandAll();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            doAdd();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (txtMenuID.Text != null && txtMenuID.Text != "")
                doDelete();
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            if (txtMenuID.Text != null && txtMenuID.Text != "")
                doModify();
        }

        private void btnGroups_Click(object sender, EventArgs e)
        {
            string menuID = txtMenuID.Text;
            string menuName = txtCaption.Text;
            Form frmSelGroups = new frmSelGroups(menuID, menuName);
            frmSelGroups.ShowDialog();
        }

        private void btnSelPackage_Click(object sender, EventArgs e)
        {
            if (this.cmbModuleType.SelectedIndex == -1)
            {
                MessageBox.Show("Please choose ModuleType first");
                return;
            }
            switch (this.cmbModuleType.SelectedValue.ToString())
            {
                case "F":
                    this.openFileDialog1.FilterIndex = 1;
                    this.openFileDialog1.InitialDirectory = EEPRegistry.Client;
                    break;
                case "W":
                    this.openFileDialog1.FilterIndex = 2;
                    this.openFileDialog1.InitialDirectory = EEPRegistry.WebClient;
                    break;
                case "O":
                    this.openFileDialog1.FilterIndex = 4;
                    this.openFileDialog1.InitialDirectory = string.Format("{0}\\WorkFlow", EEPRegistry.Server);
                    break;
                case "C":
                    this.openFileDialog1.FilterIndex = 1;
                    this.openFileDialog1.InitialDirectory = EEPRegistry.Client;
                    break;
                case "L":
                    this.openFileDialog1.FilterIndex = 5;
                    if (!string.IsNullOrWhiteSpace(EEPRegistry.Server))
                    {
                        this.openFileDialog1.InitialDirectory = string.Format("{0}\\SLClient", EEPRegistry.Server.Substring(0, EEPRegistry.Server.LastIndexOf("\\")));
                    }
                    break;
                case "B":
                    this.openFileDialog1.FilterIndex = 2;
                    if (!string.IsNullOrWhiteSpace(EEPRegistry.Server))
                    {
                        this.openFileDialog1.InitialDirectory = string.Format("{0}\\EFWebClient", EEPRegistry.Server.Substring(0, EEPRegistry.Server.LastIndexOf("\\")));
                    }
                    break;
                case "J":
                    this.openFileDialog1.FilterIndex = 2;
                    if (!string.IsNullOrWhiteSpace(EEPRegistry.Server))
                    {
                        this.openFileDialog1.InitialDirectory = string.Format("{0}\\JQWebClient", EEPRegistry.Server.Substring(0, EEPRegistry.Server.LastIndexOf("\\")));
                    }
                    break;
                case "M":
                    this.openFileDialog1.FilterIndex = 2;
                    if (!string.IsNullOrWhiteSpace(EEPRegistry.Server))
                    {
                        this.openFileDialog1.InitialDirectory = string.Format("{0}\\JQWebClient", EEPRegistry.Server.Substring(0, EEPRegistry.Server.LastIndexOf("\\")));
                    }
                    break;
                default:
                    this.openFileDialog1.FilterIndex = 3;
                    break;
            }
            this.openFileDialog1.Filter = "dll files (*.dll)|*.dll|aspx files (*.aspx)|*.aspx|asmx files (*.asmx)|*.asmx|xoml files (*.xoml)|*.xoml|xaml files (*.xaml)|*.xaml|All files (*.*)|*.*";
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string strFilePath = "";
                string FileName = "";
                string ExtName = "";
                switch (this.cmbModuleType.SelectedValue.ToString())
                {
                    case "F":
                    case "C":
                        {
                            strFilePath = this.openFileDialog1.FileName;
                            ExtName = Path.GetExtension(strFilePath);
                            if (string.Compare(ExtName, ".dll", true) == 0)
                            {
                                this.txtPackage.Text = Path.GetFileNameWithoutExtension(strFilePath);
                            }
                            break;
                        }
                    case "W":
                    case "B":
                    case "J":
                    case "M":
                        {
                            strFilePath = this.openFileDialog1.FileName;
                            FileName = strFilePath.Substring(strFilePath.LastIndexOf('\\') + 1);
                            strFilePath = strFilePath.Substring(0, strFilePath.LastIndexOf('\\'));
                            string strPackageName = strFilePath.Substring(strFilePath.LastIndexOf('\\') + 1);// packageName
                            string strFile = FileName.Substring(0, FileName.IndexOf('.'));
                            ExtName = FileName.Substring(FileName.IndexOf('.') + 1);
                            if (string.Compare(ExtName, "aspx", true) == 0 || string.Compare(ExtName, "asmx", 0) == 0)
                            {
                                this.txtPackage.Text = strPackageName;
                                this.txtForm.Text = strFile;
                            }
                            break;
                        }
                    case "O":
                        {
                            string strPackageName = "";
                            strFilePath = this.openFileDialog1.FileName;
                            FileName = strFilePath.Substring(strFilePath.LastIndexOf('\\') + 1);
                            strFilePath = strFilePath.Substring(0, strFilePath.LastIndexOf('\\'));
                            if (!strFilePath.EndsWith("workflow", StringComparison.OrdinalIgnoreCase))//IgnoreCase
                            {
                                strPackageName = strFilePath.Substring(strFilePath.LastIndexOf('\\') + 1);
                            }
                            string strFile = FileName.Substring(0, FileName.IndexOf('.'));
                            ExtName = FileName.Substring(FileName.IndexOf('.') + 1);
                            if (string.Compare(ExtName, "xoml", true) == 0)
                            {
                                this.txtPackage.Text = strPackageName;
                                this.txtForm.Text = strFile;
                            }
                            break;
                        }
                    case "L":
                        {
                            strFilePath = this.openFileDialog1.FileName;

                            ExtName = Path.GetExtension(strFilePath);
                            if (string.Compare(ExtName, ".xaml", true) == 0)
                            {
                                string[] splits = strFilePath.Split('\\');
                                string package = string.Empty;
                                if (splits.Length > 0)
                                {
                                    if (splits.Length > 3)
                                    {
                                        package = string.Format("{0}.{1}", splits[splits.Length - 3], splits[splits.Length - 2]);
                                    }
                                    else
                                    {
                                        package = splits[splits.Length - 2];
                                    }
                                }
                                this.txtPackage.Text = package;
                                this.txtForm.Text = Path.GetFileNameWithoutExtension(strFilePath);
                            }
                            break;
                        }
                    default:
                        return;
                }
            }
        }

        private void btnSelImage_Click(object sender, EventArgs e)
        {
            this.openFileDialog2.InitialDirectory = EEPRegistry.Client;
            this.openFileDialog2.Filter = "Icon files(*.ico)|*.ico|All files(*.*)|*.*";

            if (this.openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                iconpath = openFileDialog2.FileName;
                pbImage.Image = Image.FromFile(openFileDialog2.FileName);
            }
        }

        private string initialDir = "";
        private void btnImageUrl_Click(object sender, EventArgs e)
        {
            this.openFileDialog3.InitialDirectory = initialDir;
            this.openFileDialog3.Filter = "Image files(*.bmp, *.jpg, *.gif, *.png, *.ico)|*.bmp; *.jpg; *.gif; *.png; *.ico|All files(*.*)|*.*";
            if (this.openFileDialog3.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog3.FileName;
                string fileName = filePath.Substring(filePath.LastIndexOf("\\") + 1);
                // 如果选择路径不是初始路径
                if (string.Compare(filePath, initialDir + "\\" + fileName, true) != 0)//IgnoreCase
                {
                    // 初始路径中存在被选中的文件
                    if (File.Exists(initialDir + "\\" + fileName))
                    {
                        //if (MessageBox.Show("'" + fileName + "' is already exist in the path of '" + initialDir + "', would you like to replace it?",
                        //    "", MessageBoxButtons.OKCancel) == DialogResult.OK)
                        //{
                        //    File.Copy(filePath, initialDir + "\\" + fileName, true);
                        //}
                        MessageBox.Show("'" + fileName + "' has already existed in the path of '" + initialDir + "', which is not allowed");
                    }
                    else
                    {
                        File.Copy(filePath, initialDir + "\\" + fileName, true);
                    }
                }
                this.txtImageUrl.Text = fileName;
                this.preViewImage.Image = Image.FromFile(initialDir + "\\" + fileName);
            }
        }

        private void cmbModuleType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbModuleType.SelectedIndex == -1)
            {
                return;
            }
            this.txtItemParam.Enabled = cmbModuleType.SelectedIndex != 2 && cmbModuleType.SelectedIndex != 3;

            if (cmbModuleType.SelectedValue.ToString() != "O" || optype == OpType.add)
            {
                if (cmbModuleType.SelectedValue.ToString() != "W" && cmbModuleType.SelectedValue.ToString() != "L"
                    && cmbModuleType.SelectedValue.ToString() != "B" && cmbModuleType.SelectedValue.ToString() != "J"
                    && cmbModuleType.SelectedValue.ToString() != "M")
                    this.panel1.Visible = false;
                else
                    this.panel1.Visible = true;
            }
            else
            {
                object[] strCaption = new object[3];
                if (!string.IsNullOrEmpty(this.txtParent.Text))
                    strCaption[0] = this.txtParent.Text;
                strCaption[1] = this.infoCmbSolution.SelectedValue.ToString();
                switch (this.cbLanguageID.Text)
                {
                    case "English": strCaption[2] = "0"; break;
                    case "Traditional Chinese": strCaption[2] = "1"; break;
                    case "Simplified Chinese": strCaption[2] = "2"; break;
                    case "HongKong": strCaption[2] = "3"; break;
                    case "Japanese": strCaption[2] = "4"; break;
                    case "Korean": strCaption[2] = "5"; break;
                    case "User-defined1": strCaption[2] = "6"; break;
                    case "User-defined2": strCaption[2] = "7"; break;
                    case "Default": strCaption[2] = ""; break;
                }
                ArrayList lstParam = null;
                object[] myRet = CliUtils.CallMethod("GLModule", "GetParam", strCaption);
                if ((null != myRet) && (0 == (int)myRet[0]))
                    lstParam = ((ArrayList)myRet[1]);

                string parentModuleType = lstParam[3].ToString();
                if (parentModuleType != "W" && parentModuleType != "L"
                    && parentModuleType != "J" && parentModuleType != "M")
                    this.panel1.Visible = false;
                else
                    this.panel1.Visible = true;
            }
        }

        private void cbLanguageID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (txtMenuID.Text == null || txtMenuID.Text == "")
            {
                string message = SysMsg.GetSystemMessage(language, "Srvtools", "InfoSecurity", "IsChooseMenu");
                MessageBox.Show(message);
            }
            else
            {
                string[] param = new string[1];
                param[0] = txtMenuID.Text;
                object[] myRet = CliUtils.CallMethod("GLModule", "GetLanguage", param);
                DataSet ds = new DataSet();
                if (myRet[1] != null || myRet[1] != DBNull.Value)
                    ds = (DataSet)myRet[1];
                if (ds.Tables[0].Rows.Count > 0)
                {
                    switch (cbLanguageID.Text.ToString())
                    {
                        case "English": this.txtCaption.Text = ds.Tables[0].Rows[0]["Caption0"].ToString(); break;
                        case "Traditional Chinese": this.txtCaption.Text = ds.Tables[0].Rows[0]["Caption1"].ToString(); break;
                        case "Simplified Chinese": this.txtCaption.Text = ds.Tables[0].Rows[0]["Caption2"].ToString(); break;
                        case "HongKong": this.txtCaption.Text = ds.Tables[0].Rows[0]["Caption3"].ToString(); break;
                        case "Japanese": this.txtCaption.Text = ds.Tables[0].Rows[0]["Caption4"].ToString(); break;
                        case "Korean": this.txtCaption.Text = ds.Tables[0].Rows[0]["Caption5"].ToString(); break;
                        case "User-defined1": this.txtCaption.Text = ds.Tables[0].Rows[0]["Caption6"].ToString(); break;
                        case "User-defined2": this.txtCaption.Text = ds.Tables[0].Rows[0]["Caption7"].ToString(); break;
                        case "Default": this.txtCaption.Text = ds.Tables[0].Rows[0]["Caption"].ToString(); break;
                    }
                }
            }
            tView.SelectedNode = curNode;
        }

        private void btnUsers_Click(object sender, EventArgs e)
        {
            string menuID = txtMenuID.Text;
            string menuName = txtCaption.Text;
            frmSelUsers fsu = new frmSelUsers(menuID, menuName);
            fsu.ShowDialog();
            tView.SelectedNode = curNode;
        }

        private void accessUsersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string menuID = txtMenuID.Text;
            string menuName = txtCaption.Text;
            frmSelUsers fsu = new frmSelUsers(menuID, menuName);
            fsu.ShowDialog();
            tView.SelectedNode = curNode;
        }

        private void tView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            ////get child node
            //if (e.Node.Tag != null)
            //{
            //    string filter = string.Format("PARENT = '{0}' and ITEMTYPE = '{1}'", e.Node.Tag.ToString(), infoCmbSolution.SelectedValue.ToString());
            //    DataSet ds = CliUtils.GetSqlCommand("GLModule", "sqlMenus", infodsMenus, filter, CliUtils.fCurrentProject, null);
            //    //add node
            //    foreach (DataRow dr in ds.Tables[0].Rows)
            //    {
            //        TreeNode node = new TreeNode();
            //        node.Name = dr["MENUID"].ToString();
            //        node.Text = dr["CAPTION"].ToString();
            //        node.Tag = dr["MENUID"].ToString();
            //        if (e.Node.Nodes[node.Name] == null)
            //        {
            //            e.Node.Nodes.Add(node);
            //        }
            //    }
            //}
            //e.Node.Expand();
        }

        private void btnSelectReport_Click(object sender, EventArgs e)
        {
            frmSelectReport reportWindow = new frmSelectReport();
            if (reportWindow.ShowDialog() == DialogResult.OK)
            {
                this.txtItemParam.Text = reportWindow.ReturnValue;
            }
        }

        private void txtPackage_TextChanged(object sender, EventArgs e)
        {
            string text = ((TextBox)sender).Text;

            if (text == "SLEasilyReportManager.CommonForm")
            {
                this.btnSelectReport.Visible = true;
            }
            else
            {
                this.btnSelectReport.Visible = false;
            }
        }

        private void accessPageControlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string menuID = txtMenuID.Text;
            frmMenuTableControl m = new frmMenuTableControl(menuID);
            m.ShowDialog();
        }

    }
}
