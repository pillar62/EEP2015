using EFClientTools.EFServerReference;
using Srvtools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace JQMobileTools
{
    public partial class JQMetroEditForm : Form
    {
        internal enum SizeType
        {
            //s:square m:wide l:squarepeek h:widepeek 
            /// <summary>
            /// square
            /// </summary>
            s = 0,
            /// <summary>
            /// wide
            /// </summary>
            m = 1,
            /// <summary>
            /// squarepeek
            /// </summary>
            l = 2,
            /// <summary>
            /// widepeek
            /// </summary>
            h = 3,
            /// <summary>
            /// app
            /// </summary>
            a = 4
        }
        internal enum StyleType
        {
            //s:square m:wide l:squarepeek h:widepeek 
            /// <summary>
            /// None
            /// </summary>
            N = 0,
            /// <summary>
            /// DataGrid
            /// </summary>
            G = 1,
            /// <summary>
            /// DataForm
            /// </summary>
            F = 2,
            /// <summary>
            /// Chart
            /// </summary>
            C = 3,
            /// <summary>
            /// Dashboard
            /// </summary>
            D = 4,
            /// <summary>
            /// Image
            /// </summary>
            I = 5,
            /// <summary>
            /// Text
            /// </summary>
            T = 6
        }

        internal enum BackGroundColor { orange, purple, greenDark, blue, red, green, blueDark, yellow, pink, darken, gray, grayLight }

        public enum OpType { add, modify, delete }

        private JQMetro metro = null;
        private ArrayList MenuIDList = new ArrayList();
        private ArrayList CaptionList = new ArrayList();
        private ArrayList ParentList = new ArrayList();
        private List<MENUTABLE> MenuList = new List<MENUTABLE>();
        private TreeNode curNode = new TreeNode();
        private string initialDir = "";
        private OpType optype;
        private string iconpath = null;

        public JQMetroEditForm(JQMetro jqm, IDesignerHost host)
        {
            metro = jqm;
            InitializeComponent();

            DataTable tableSize = new DataTable();
            tableSize.Columns.Add("Value");
            tableSize.Columns.Add("Name");
            tableSize.Rows.Add(new object[] { SizeType.s, "square" });
            tableSize.Rows.Add(new object[] { SizeType.m, "wide" });
            tableSize.Rows.Add(new object[] { SizeType.l, "squarepeek" });
            tableSize.Rows.Add(new object[] { SizeType.h, "widepeek" });
            tableSize.Rows.Add(new object[] { SizeType.a, "app" });
            this.cmbSize.DataSource = tableSize;
            this.cmbSize.DisplayMember = "Name";
            this.cmbSize.ValueMember = "Value";

            DataTable tableBDColor = new DataTable();
            tableBDColor.Columns.Add("Value");
            tableBDColor.Columns.Add("Name");
            tableBDColor.Rows.Add(new object[] { "orange", "orange" });
            tableBDColor.Rows.Add(new object[] { "purple", "purple" });
            tableBDColor.Rows.Add(new object[] { "greenDark", "greenDark" });
            tableBDColor.Rows.Add(new object[] { "blue", "blue" });
            tableBDColor.Rows.Add(new object[] { "red", "red" });
            tableBDColor.Rows.Add(new object[] { "green", "green" });
            tableBDColor.Rows.Add(new object[] { "blueDark", "blueDark" });
            tableBDColor.Rows.Add(new object[] { "yellow", "yellow" });
            tableBDColor.Rows.Add(new object[] { "pink", "pink" });
            tableBDColor.Rows.Add(new object[] { "darken", "darken" });
            tableBDColor.Rows.Add(new object[] { "gray", "gray" });
            tableBDColor.Rows.Add(new object[] { "grayLight", "grayLight" });
            this.cmbBGColor.DataSource = tableBDColor;
            this.cmbBGColor.DisplayMember = "Name";
            this.cmbBGColor.ValueMember = "Value";

            DataTable tableStyle = new DataTable();
            tableStyle.Columns.Add("Value");
            tableStyle.Columns.Add("Name");
            tableStyle.Rows.Add(new object[] { StyleType.N, "None" });
            tableStyle.Rows.Add(new object[] { StyleType.G, "DataGrid" });
            tableStyle.Rows.Add(new object[] { StyleType.F, "DataForm" });
            tableStyle.Rows.Add(new object[] { StyleType.C, "Chart" });
            tableStyle.Rows.Add(new object[] { StyleType.D, "Dashboard" });
            tableStyle.Rows.Add(new object[] { StyleType.I, "Image" });
            tableStyle.Rows.Add(new object[] { StyleType.T, "Text" });
            this.cbStyle.DataSource = tableStyle;
            this.cbStyle.DisplayMember = "Name";
            this.cbStyle.ValueMember = "Value";

        }

        private void JQMetroEditForm_Load(object sender, EventArgs e)
        {
            try
            {
                this.infodsSolutions.Active = true;
                this.infodsMenus.Active = true;
                //btnApplyTree.Enabled = false;
                EnableControl(false);
                txtItemType.Enabled = false;
                ItemToGet();
                //tView.ExpandAll();
                // 初始化image目录（web）
                initialDir = string.Format("{0}\\Image\\MenuTree", EEPRegistry.WebClient.TrimEnd('\\'));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void infoCmbSolution_SelectedIndexChanged(object sender, EventArgs e)
        {
            tView.Nodes.Clear();
            EnableControl(false);
            ClearAllText();
            this.infodsMenus.Active = false;
            this.infodsMenus.Active = true;
            ItemToGet();
            //tView.ExpandAll();
        }

        private void ItemToGet()
        {
            MenuIDList.Clear();
            CaptionList.Clear();
            ParentList.Clear();
            MenuList.Clear();

            if (String.IsNullOrEmpty(metro.DBAlias))
            {
                System.Windows.Forms.MessageBox.Show("Please set DBAlias first.");
                return;
            }
            var client = EFClientTools.DesignClientUtility.Client;
            var userid = EFClientTools.DesignClientUtility.ClientInfo.UserID;
            var solutionid = EFClientTools.DesignClientUtility.ClientInfo.Solution;
            try
            {
                EFClientTools.DesignClientUtility.ClientInfo.Database = metro.DBAlias;
                EFClientTools.DesignClientUtility.ClientInfo.UserID = metro.UserID;
                EFClientTools.DesignClientUtility.ClientInfo.UseDataSet = true;
                var currentGroup = EFClientTools.DesignClientUtility.ClientInfo.CurrentGroup;
                EFClientTools.DesignClientUtility.ClientInfo.CurrentGroup = "forManager";
                var allMenus = client.FetchMenus(EFClientTools.DesignClientUtility.ClientInfo).OfType<EFClientTools.EFServerReference.MENUTABLE>().Where(m => String.IsNullOrEmpty(m.PARENT));
                EFClientTools.DesignClientUtility.ClientInfo.CurrentGroup = currentGroup; 
                List<MENUTABLE> menus = null;
                if (!String.IsNullOrEmpty(metro.RootValue))
                {
                    if (allMenus.Where(m => m.MENUID == metro.RootValue).FirstOrDefault() != null)
                        menus = allMenus.Where(m => m.MENUID == metro.RootValue).FirstOrDefault().MENUTABLE1;
                    else
                        menus = new List<MENUTABLE>();
                }
                else
                {
                    menus = allMenus.ToList();
                }
                EFClientTools.DesignClientUtility.ClientInfo.UserID = "";
                initializeMenus(menus, MenuIDList, CaptionList, ParentList, MenuList);

                initializeTreeView(MenuIDList, CaptionList, ParentList);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    MessageBox.Show(ex.InnerException.Message);
                else
                    MessageBox.Show(ex.Message);
            }
            finally
            {
                EFClientTools.DesignClientUtility.ClientInfo.UserID = userid;
                EFClientTools.DesignClientUtility.ClientInfo.Solution = solutionid;
            }
        }

        private void initializeMenus(List<MENUTABLE> menus, ArrayList menuIDList, ArrayList menuCaptionList, ArrayList menuParentIDList, List<MENUTABLE> MenuList)
        {
            foreach (var item in menus)
            {
                MenuIDList.Add(item.MENUID);
                CaptionList.Add(item.CAPTION);
                ParentList.Add(item.PARENT);
                MenuList.Add(item);
                if (item.MENUTABLE1 != null)
                    initializeMenus(item.MENUTABLE1, MenuIDList, CaptionList, ParentList, MenuList);
            }
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
                if (menuParentIDList[i] == null || MenuIDList[i].ToString() != menuParentIDList[i].ToString())
                {
                    if (!String.IsNullOrEmpty(metro.RootValue))
                    {
                        if (menuParentIDList[i] != null && menuParentIDList[i].ToString() == metro.RootValue)
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
                    else
                    {
                        if (menuParentIDList[i] == null || menuParentIDList[i].ToString().Trim() == "")
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
            }
            TreeNode[] nodeMain = new TreeNode[ListMainID.Count];
            for (int i = 0; i < ListMainID.Count; i++)
            {
                nodeMain[i] = new TreeNode();
                tView.Nodes.Add(nodeMain[i]);
                nodeMain[i].Text = ListMainCaption[i].ToString();
                nodeMain[i].Name = ListMainID[i].ToString();
                nodeMain[i].Tag = MenuList.Where(m => m.MENUID == ListMainID[i].ToString()).FirstOrDefault();
            }
            TreeNode[] nodeChildren = new TreeNode[ListChildrenID.Count];
            for (int i = 0; i < ListChildrenID.Count; i++)
            {
                nodeChildren[i] = new TreeNode();
                nodeChildren[i].Text = ListChildrenCaption[i].ToString();
                nodeChildren[i].Name = ListChildrenID[i].ToString();
                nodeChildren[i].Tag = MenuList.Where(m => m.MENUID == ListChildrenID[i].ToString()).FirstOrDefault();
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

            MENUTABLE selectedMENU = (MENUTABLE)curNode.Tag;
            txtMenuID.Text = selectedMENU.MENUID;
            switch (this.cbLanguageID.Text)
            {
                case "English": txtCaption.Text = selectedMENU.CAPTION0; break;
                case "Traditional Chinese": txtCaption.Text = selectedMENU.CAPTION1; break;
                case "Simplified Chinese": txtCaption.Text = selectedMENU.CAPTION2; break;
                case "HongKong": txtCaption.Text = selectedMENU.CAPTION3; break;
                case "Japanese": txtCaption.Text = selectedMENU.CAPTION4; break;
                case "Korean": txtCaption.Text = selectedMENU.CAPTION5; break;
                case "User-defined1": txtCaption.Text = selectedMENU.CAPTION6; break;
                case "User-defined2": txtCaption.Text = selectedMENU.CAPTION7; break;
                case "Default": txtCaption.Text = selectedMENU.CAPTION; break;
            }
            txtParent.Text = selectedMENU.PARENT;
            if (!String.IsNullOrEmpty(selectedMENU.ISSHOWMODAL))
                cmbSize.SelectedIndex = (int)Enum.Parse(typeof(SizeType), selectedMENU.ISSHOWMODAL);
            else
                cmbSize.SelectedIndex = 0;
            if (!String.IsNullOrEmpty(selectedMENU.VERSIONNO))
                cmbBGColor.SelectedValue = selectedMENU.VERSIONNO;
            else
                cmbBGColor.SelectedIndex = -1;

            txtImageUrl.Text = selectedMENU.IMAGEURL;
            txtPackage.Text = selectedMENU.PACKAGE;
            txtItemParam.Text = selectedMENU.ITEMPARAM;
            txtForm.Text = selectedMENU.FORM;
            txtItemType.Text = selectedMENU.ITEMTYPE;
            txtSEQ_NO.Text = selectedMENU.SEQ_NO;
            if (txtImageUrl.Text != null && txtImageUrl.Text.Trim() != "")
                preViewImage.Image = Image.FromFile(initialDir + "\\" + txtImageUrl.Text);
            else
                preViewImage.Image = null;

            if (!String.IsNullOrEmpty(selectedMENU.ISSERVER))
                cbStyle.SelectedIndex = (int)Enum.Parse(typeof(StyleType), selectedMENU.ISSERVER);
            else
                cbStyle.SelectedIndex = 0;
            txtStyleObject.Text = selectedMENU.OWNER;

            EnableControl(false);
        }

        private void EnableControl(bool bEnable)
        {
            txtMenuID.Enabled = bEnable;
            txtCaption.Enabled = bEnable;
            txtParent.Enabled = bEnable;
            cmbSize.Enabled = bEnable;
            cmbBGColor.Enabled = bEnable;
            txtImageUrl.Enabled = bEnable;////
            btnImageUrl.Enabled = bEnable;
            txtPackage.Enabled = bEnable;
            btnSelPackage.Enabled = bEnable;
            txtItemParam.Enabled = bEnable && cmbSize.SelectedIndex != 2 && cmbSize.SelectedIndex != 3;
            txtForm.Enabled = bEnable;
            txtSEQ_NO.Enabled = bEnable;
            cbStyle.Enabled = bEnable;
            txtStyleObject.Enabled = bEnable;////
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
            cmbSize.SelectedIndex = -1;
            cmbBGColor.SelectedIndex = -1;
            txtImageUrl.Text = string.Empty;
            txtPackage.Text = string.Empty;
            txtItemParam.Text = string.Empty;
            txtForm.Text = string.Empty;
            txtItemType.Text = string.Empty;
            txtSEQ_NO.Text = string.Empty;
            cbStyle.SelectedIndex = -1;
            txtStyleObject.Text = string.Empty;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            doAdd();
        }

        private void doAdd()
        {
            string strTempParent = txtMenuID.Text;
            int moduletypeparent = this.cmbSize.SelectedIndex;
            ClearAllText();
            this.cmbSize.SelectedIndex = moduletypeparent;//wait test
            try
            {
                var client = EFClientTools.DesignClientUtility.Client;
                EFClientTools.DesignClientUtility.ClientInfo.Database = metro.DBAlias;
                EFClientTools.DesignClientUtility.ClientInfo.UseDataSet = true;
                txtMenuID.Text = client.AutoSeqMenuID(EFClientTools.DesignClientUtility.ClientInfo, null).ToString();
            }
            catch
            {
                txtMenuID.Text = "";
            }
            txtParent.Text = strTempParent;
            if (String.IsNullOrEmpty(txtParent.Text))
                txtParent.Text = metro.RootValue;
            txtItemType.Text = this.infoCmbSolution.SelectedValue.ToString();
            txtForm.Text = "Form1";
            curNode = tView.SelectedNode;
            EnableControl(true);
            //btnReloadTree.Enabled = false;
            infoCmbSolution.Enabled = false;
            tView.Enabled = false;
            this.cbLanguageID.SelectedIndexChanged -= this.cbLanguageID_SelectedIndexChanged;
            this.cbLanguageID.Text = "Default";
            this.cbLanguageID.SelectedIndexChanged += this.cbLanguageID_SelectedIndexChanged;
            this.cbLanguageID.Enabled = false;
            optype = OpType.add;
            iconpath = null;
        }

        private void btnDelete_Click(object sender, EventArgs e)
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
                //this.btnReloadTree.Enabled = false;
                this.btnAdd.Enabled = false;
                this.btnModify.Enabled = false;
                this.btnDelete.Enabled = false;
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

        private void btnModify_Click(object sender, EventArgs e)
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
            //btnReloadTree.Enabled = false;
            infoCmbSolution.Enabled = false;
            tView.Enabled = false;
            optype = OpType.modify;
            iconpath = null;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            //new add by ccm
            if (txtCaption.Text == string.Empty)
            {
                MessageBox.Show("Caption can't be null");
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
            //btnReloadTree.Enabled = true;
            tView.Enabled = true;
            infoCmbSolution.Enabled = true;
            this.cbLanguageID.Enabled = true;

            //object[] strAddParam = new object[13];
            MENUTABLE aMENUTABLE = new MENUTABLE();
            if (optype == OpType.modify)
            {
                aMENUTABLE = (MENUTABLE)curNode.Tag;
            }
            //new add by ccm

            aMENUTABLE.MENUID = (txtMenuID.Text == string.Empty) ? null : txtMenuID.Text;
            //aMENUTABLE.CAPTION = txtCaption.Text;
            switch (this.cbLanguageID.Text)
            {
                case "English": aMENUTABLE.CAPTION0 = txtCaption.Text; break;
                case "Traditional Chinese": aMENUTABLE.CAPTION1 = txtCaption.Text; break;
                case "Simplified Chinese": aMENUTABLE.CAPTION2 = txtCaption.Text; break;
                case "HongKong": aMENUTABLE.CAPTION3 = txtCaption.Text; break;
                case "Japanese": aMENUTABLE.CAPTION4 = txtCaption.Text; break;
                case "Korean": aMENUTABLE.CAPTION5 = txtCaption.Text; break;
                case "User-defined1": aMENUTABLE.CAPTION6 = txtCaption.Text; break;
                case "User-defined2": aMENUTABLE.CAPTION7 = txtCaption.Text; break;
                case "Default": aMENUTABLE.CAPTION = txtCaption.Text; break;
            }
            aMENUTABLE.PARENT = (txtParent.Text == string.Empty) ? null : txtParent.Text;
            if (cmbSize.SelectedValue == null)
                aMENUTABLE.ISSHOWMODAL = "s";
            else
                aMENUTABLE.ISSHOWMODAL = cmbSize.SelectedValue.ToString();
            if (cmbBGColor.SelectedValue == null)
                aMENUTABLE.VERSIONNO = "blue";
            else
                aMENUTABLE.VERSIONNO = cmbBGColor.SelectedValue.ToString();
            aMENUTABLE.MODULETYPE = "M";
            aMENUTABLE.PACKAGE = (txtPackage.Text == string.Empty) ? null : txtPackage.Text;
            aMENUTABLE.ITEMPARAM = (txtItemParam.Text == string.Empty) ? null : txtItemParam.Text;
            aMENUTABLE.FORM = (txtForm.Text == string.Empty) ? null : txtForm.Text;
            aMENUTABLE.ITEMTYPE = (txtItemType.Text == string.Empty) ? null : txtItemType.Text;
            if (txtSEQ_NO.Text != string.Empty && txtSEQ_NO.Text.Length == 1 && txtSEQ_NO.Text[0] < 58 && txtSEQ_NO.Text[0] > 48)
                txtSEQ_NO.Text = '0' + txtSEQ_NO.Text;
            aMENUTABLE.SEQ_NO = (txtSEQ_NO.Text == string.Empty) ? null : txtSEQ_NO.Text;
            aMENUTABLE.IMAGEURL = (txtImageUrl.Text == string.Empty) ? null : txtImageUrl.Text;
            if (cbStyle.SelectedValue == null)
                aMENUTABLE.ISSERVER = "N";
            else
                aMENUTABLE.ISSERVER = cbStyle.SelectedValue.ToString();
            aMENUTABLE.OWNER = (txtStyleObject.Text == string.Empty) ? null : txtStyleObject.Text;
            List<object> menus = new List<object>();
            menus.Add(aMENUTABLE);
            if (optype == OpType.add || optype == OpType.modify)
            {
                EFClientTools.DesignClientUtility.SaveDataToTable(menus, "MENUTABLE");
            }
            else
            {
                EFClientTools.DesignClientUtility.DeleteDataFromTable(aMENUTABLE, "MENUTABLE");
            }

            if (optype == OpType.add)
            {
                List<object> groupmenus = new List<object>();
                groupmenus.Add(aMENUTABLE.MENUID);
                groupmenus.Add("00");
                EFClientTools.DesignClientUtility.SaveDataToTable(groupmenus, "GROUPMENUS");

                int i = this.infodsMenus.RealDataSet.Tables[0].Rows.Count;
                for (int j = 0; j < i; j++)
                {
                    if (this.infodsMenus.RealDataSet.Tables[0].Rows[j]["menuid"].ToString() == aMENUTABLE.MENUID)
                    {
                        MessageBox.Show("menuid already existed !");
                        curNode = tView.SelectedNode;
                        EnableControl(true);
                        //txtCaption.Enabled = false;
                        //btnReloadTree.Enabled = false;
                        infoCmbSolution.Enabled = false;
                        tView.Enabled = false;
                        optype = OpType.add;
                        iconpath = null;
                        this.txtParent.Text = aMENUTABLE.PARENT;
                        return;
                    }
                }
                //TreeNode addNode = new TreeNode(strAddParam[1].ToString());
                //if (curNode != null)
                //    curNode.Nodes.Add(addNode);
                //else
                //    this.tView.Nodes.Add(addNode);
            }
            //.CallMethod("GLModule", "OPMenu", strAddParam);
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
                    node[0].Tag = aMENUTABLE;
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
                                //tView.Nodes.Remove(node[0]);
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
            //btnReloadTree.Enabled = true;
            tView.Enabled = true;
            this.infoCmbSolution.Enabled = true;
            this.cbLanguageID.Enabled = true;

            //this.ClearAllText();
            //new add 为了解删除node后直接刷新的问题 == refresh
            //tView.Nodes.Clear();
            //this.infodsMenus.Active = false;
            //this.infodsMenus.Active = true;
            //ItemToGet();
            //tView.ExpandAll();
            //////////////////
        }

        private void btnSelPackage_Click(object sender, EventArgs e)
        {
            this.openFileDialog1.FilterIndex = 2;
            if (!string.IsNullOrWhiteSpace(EEPRegistry.Server))
            {
                this.openFileDialog1.InitialDirectory = string.Format("{0}\\JQWebClient", EEPRegistry.Server.Substring(0, EEPRegistry.Server.LastIndexOf("\\")));
            }
            this.openFileDialog1.Filter = "dll files (*.dll)|*.dll|aspx files (*.aspx)|*.aspx|asmx files (*.asmx)|*.asmx|xoml files (*.xoml)|*.xoml|xaml files (*.xaml)|*.xaml|All files (*.*)|*.*";
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string strFilePath = "";
                string FileName = "";
                string ExtName = "";
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
            }
        }

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

        private void cbLanguageID_SelectedIndexChanged(object sender, EventArgs e)
        {
            MENUTABLE selectedMENU = (MENUTABLE)curNode.Tag;
            switch (cbLanguageID.Text.ToString())
            {
                case "English": this.txtCaption.Text = selectedMENU.CAPTION0; break;
                case "Traditional Chinese": this.txtCaption.Text = selectedMENU.CAPTION1; break;
                case "Simplified Chinese": this.txtCaption.Text = selectedMENU.CAPTION2; break;
                case "HongKong": this.txtCaption.Text = selectedMENU.CAPTION3; break;
                case "Japanese": this.txtCaption.Text = selectedMENU.CAPTION4; break;
                case "Korean": this.txtCaption.Text = selectedMENU.CAPTION5; break;
                case "User-defined1": this.txtCaption.Text = selectedMENU.CAPTION6; break;
                case "User-defined2": this.txtCaption.Text = selectedMENU.CAPTION7; break;
                case "Default": this.txtCaption.Text = selectedMENU.CAPTION; break;
            }
        }

        private void btnStyleObject_Click(object sender, EventArgs e)
        {
            string style = cbStyle.SelectedValue.ToString();
            List<string> list = new List<string>();
            var controls = metro.Page.Controls;

            if (style == "N") { return; }
            else if (style == "G")
            {
                foreach (System.Web.UI.WebControls.WebControl control in controls.OfType<System.Web.UI.WebControls.WebControl>())
                {
                    if (control.GetType().Name == "JQDataGrid")
                        list.Add(control.ID);
                    else
                    {
                        GetChildrenControls(control, list, new List<string>() { "JQDataGrid" });
                    }
                }
            }
            else if (style == "F")
            {
                foreach (System.Web.UI.WebControls.WebControl control in controls.OfType<System.Web.UI.WebControls.WebControl>())
                {
                    if (control.GetType().Name == "JQDataForm")
                        list.Add(control.ID);
                    else
                    {
                        GetChildrenControls(control, list, new List<string>() { "JQDataForm" });
                    }
                }
            }
            else if (style == "I" || style == "T")
            {
                foreach (System.Web.UI.WebControls.WebControl control in controls.OfType<System.Web.UI.WebControls.WebControl>())
                {
                    if (control.GetType().Name == "JQRotator")
                        list.Add(control.ID);
                    else
                    {
                        GetChildrenControls(control, list, new List<string>() { "JQRotator" });
                    }
                }

            }
            else if (style == "D")
            {
                foreach (System.Web.UI.WebControls.WebControl control in controls.OfType<System.Web.UI.WebControls.WebControl>())
                {
                    if (control.GetType().Name == "JQDashBoard")
                        list.Add(control.ID);
                    else
                    {
                        GetChildrenControls(control, list, new List<string>() { "JQDashBoard" });
                    }
                }

            }
            else if (style == "C")
            {
                foreach (System.Web.UI.WebControls.WebControl control in controls.OfType<System.Web.UI.WebControls.WebControl>())
                {
                    if (control.GetType().Name == "JQBarChart" || control.GetType().Name == "JQLineChart" || control.GetType().Name == "JQPieChart")
                        list.Add(control.ID);
                    else {
                        GetChildrenControls(control, list, new List<string>(){"JQBarChart","JQLineChart","JQPieChart"});
                    }
                }
            }
            JQMetroEditStyleObjectForm editform = new JQMetroEditStyleObjectForm(list, txtStyleObject.Text);
            if (editform.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtStyleObject.Text = editform.SelectedValue;
            }

        }
        private void GetChildrenControls(System.Web.UI.WebControls.WebControl control, List<string> list, List<string> NameList)
        {
            foreach (var item in control.Controls.OfType<System.Web.UI.WebControls.WebControl>())
            {
                if (NameList.Contains(item.GetType().Name))
                {
                    list.Add(item.ID);
                }
                if (item.Controls.Count > 0)
                {
                    GetChildrenControls(item, list, NameList);
                }
            }
        }
    }
}
