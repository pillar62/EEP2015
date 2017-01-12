using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Srvtools;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;

namespace EEPNetClient
{
    public partial class frmFavorMenu : Form
    {
        public frmFavorMenu()
        {
            InitializeComponent();
        }

        private DataSet menuDataSet;
        private List<FavorMenuItem> fmiAll = new List<FavorMenuItem>();
        private List<FavorMenuItem> fmiFavor = new List<FavorMenuItem>();
        ArrayList ListMainID = new ArrayList();
        ArrayList ListMainCaption = new ArrayList();
        ArrayList ListChildrenID = new ArrayList();
        ArrayList ListOwnerParentID = new ArrayList();
        ArrayList ListChildrenCaption = new ArrayList();
        public bool result = false;
        public frmFavorMenu(ArrayList MenuID, ArrayList Caption, ArrayList Parent)
        {
            InitializeComponent();
        }

        private void frmFavorMenu_Load(object sender, EventArgs e)
        {
            fmiAll = new List<FavorMenuItem>();
            this.cbGroup.Items.Add("");

            object[] strParam1 = new object[2];
            strParam1[0] = CliUtils.fCurrentProject;
            strParam1[1] = "F";
            DataSet dsMenus = new DataSet();
            object[] myRet = CliUtils.CallMethod("GLModule", "FetchMenus", strParam1);
            if ((null != myRet) && (0 == (int)myRet[0]))
            {
                menuDataSet = (DataSet)(myRet[1]);
            }
            int menuCount = menuDataSet.Tables[0].Rows.Count;
            string strCaption = SetMenuLanguage();
            for (int i = 0; i < menuCount; i++)
            {
                FavorMenuItem fmiMenuTemp = new FavorMenuItem();
                fmiMenuTemp.MenuID = menuDataSet.Tables[0].Rows[i]["MENUID"].ToString();
                if (strCaption != "")
                {
                    if (menuDataSet.Tables[0].Rows[i][strCaption].ToString() != "")
                        fmiMenuTemp.Caption = menuDataSet.Tables[0].Rows[i][strCaption].ToString();
                    else
                        fmiMenuTemp.Caption = menuDataSet.Tables[0].Rows[i]["CAPTION"].ToString();
                }
                else
                {
                    fmiMenuTemp.Caption = menuDataSet.Tables[0].Rows[i]["CAPTION"].ToString();
                }
                fmiMenuTemp.Parent = menuDataSet.Tables[0].Rows[i]["PARENT"].ToString();
                if (menuDataSet.Tables[0].Rows[i]["PARENT"].ToString() == String.Empty)
                    this.cbGroup.Items.Add(menuDataSet.Tables[0].Rows[i]["CAPTION"].ToString());
                fmiAll.Add(fmiMenuTemp);

            }

            DataSet dsFavorMenus = new DataSet();
            object[] strParam = new object[2];
            strParam[0] = CliUtils.fCurrentProject;
            strParam[1] = "F";
            object[] favorMenus = CliUtils.CallMethod("GLModule", "FetchFavorMenus", strParam);
            if (favorMenus != null && Convert.ToInt16(favorMenus[0]) == 0)
            {
                dsFavorMenus = favorMenus[1] as DataSet;
            }
            if (dsFavorMenus.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsFavorMenus.Tables[0].Rows.Count; i++)
                {
                    FavorMenuItem fmiTemp = new FavorMenuItem();
                    fmiTemp.MenuID = dsFavorMenus.Tables[0].Rows[i]["MENUID"].ToString();
                    fmiTemp.Caption = dsFavorMenus.Tables[0].Rows[i]["CAPTION"].ToString();
                    fmiTemp.Parent = dsFavorMenus.Tables[0].Rows[i]["PARENT"].ToString();
                    fmiFavor.Add(fmiTemp);
                    int index = FindMenuItem(fmiTemp.MenuID, fmiAll);
                    if (index != -1)
                        fmiAll.RemoveAt(index);
                }
            }

            tView.Nodes.Clear();
            initializeTreeView();

            cbGroup_SelectedIndexChanged(sender, e);
        }

        private string SetMenuLanguage()
        {
            string strCaption = "";
            switch (GetClientLanguage())
            {
                case SYS_LANGUAGE.ENG:
                    strCaption = "CAPTION0";
                    break;
                case SYS_LANGUAGE.TRA:
                    strCaption = "CAPTION1";
                    break;
                case SYS_LANGUAGE.SIM:
                    strCaption = "CAPTION2";
                    break;
                case SYS_LANGUAGE.HKG:
                    strCaption = "CAPTION3";
                    break;
                case SYS_LANGUAGE.JPN:
                    strCaption = "CAPTION4";
                    break;
                case SYS_LANGUAGE.LAN1:
                    strCaption = "CAPTION5";
                    break;
                case SYS_LANGUAGE.LAN2:
                    strCaption = "CAPTION6";
                    break;
                case SYS_LANGUAGE.LAN3:
                    strCaption = "CAPTION7";
                    break;
            }
            return strCaption;
        }

        [DllImport("KERNEL32.DLL", EntryPoint = "GetThreadLocale", SetLastError = true,
            CharSet = CharSet.Unicode, ExactSpelling = true,
            CallingConvention = CallingConvention.StdCall)]
        public static extern uint GetThreadLocale();
        static private SYS_LANGUAGE GetClientLanguage()
        {
            uint dwlang = GetThreadLocale();
            ushort wlang = (ushort)dwlang;
            ushort wprilangid = (ushort)(wlang & 0x3FF);
            ushort wsublangid = (ushort)(wlang >> 10);

            if (0x09 == wprilangid)
                return SYS_LANGUAGE.ENG;
            else if (0x04 == wprilangid)
            {
                if (0x01 == wsublangid)
                    return SYS_LANGUAGE.TRA;
                else if (0x02 == wsublangid)
                    return SYS_LANGUAGE.SIM;
                else if (0x03 == wsublangid)
                    return SYS_LANGUAGE.HKG;
                else
                    return SYS_LANGUAGE.TRA;
            }
            else if (0x11 == wprilangid)
                return SYS_LANGUAGE.JPN;
            else
                return SYS_LANGUAGE.ENG;
        }

        private void initializeTreeView()
        {
            ListMainID = new ArrayList();
            ListMainCaption = new ArrayList();
            ListChildrenID = new ArrayList();
            ListOwnerParentID = new ArrayList();
            ListChildrenCaption = new ArrayList();

            for (int i = 0; i < fmiAll.Count; i++)
            {
                if (fmiAll[i].Parent.Length == 0)
                {
                    ListMainID.Add(fmiAll[i].MenuID);
                    ListMainCaption.Add(fmiAll[i].Caption);
                }
                else
                {
                    ListChildrenID.Add(fmiAll[i].MenuID);
                    ListOwnerParentID.Add(fmiAll[i].Parent);
                    ListChildrenCaption.Add(fmiAll[i].Caption);
                }
            }

            for (int i = 0; i < ListMainID.Count; i++)
            {
                TreeNode nodeMain = new TreeNode();
                tView.Nodes.Add(nodeMain);
                nodeMain.Text = ListMainCaption[i].ToString();
                nodeMain.SelectedImageKey = ListMainID[i].ToString();
                nodeMain.ImageKey = ListMainID[i].ToString();
                nodeMain.Tag = ListMainID[i].ToString();
            }

            List<TreeNode> emptynodes = new List<TreeNode>();
            for (int i = 0; i < tView.Nodes.Count; i++)
            {
                InitializeNode(tView.Nodes[i]);
                if (TreeViewLevel != 1)
                {
                    if (IsEmptyFolderNode(tView.Nodes[i]))
                    {
                        emptynodes.Add(tView.Nodes[i]);
                    }
                }
            }
            foreach (TreeNode node in emptynodes)
            {
                tView.Nodes.Remove(node);
            }
            tView.ExpandAll();
        }

        public const int TreeViewLevel = 2;
        private void InitializeNode(TreeNode node)
        {
            //if (TreeViewLevel == -1 || node.Level < TreeViewLevel - 1)
            //{
                for (int i = 0; i < ListChildrenID.Count; i++)
                {
                    if (node.Tag.ToString() == ListOwnerParentID[i].ToString())
                    {
                        TreeNode nodeChild = new TreeNode();
                        nodeChild.Text = ListChildrenCaption[i].ToString();
                        nodeChild.SelectedImageKey = ListChildrenID[i].ToString();
                        nodeChild.ImageKey = ListChildrenID[i].ToString();
                        nodeChild.Tag = ListChildrenID[i].ToString();
                        nodeChild.Name = ListOwnerParentID[i].ToString();
                        node.Nodes.Add(nodeChild);
                    }
                }

                for (int i = 0; i < node.Nodes.Count; i++)
                {
                    InitializeNode(node.Nodes[i]);
                }
            //}
        }

        private bool IsEmptyFolderNode(TreeNode node)
        {
            if (node != null && node.Nodes.Count == 0)
            {
                if (node.Tag != null)
                {
                    DataRow[] dr = menuDataSet.Tables[0].Select(string.Format("MENUID='{0}'", node.Tag));
                    if (dr.Length > 0)
                    {
                        return (dr[0]["PACKAGE"] == DBNull.Value || dr[0]["PACKAGE"].ToString().Length == 0);
                    }
                }
            }
            return false;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAddAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < tView.Nodes.Count; i++)
            {
                FavorMenuItem fmi = new FavorMenuItem();
                fmi.MenuID = tView.Nodes[i].Tag.ToString();
                fmi.Caption = tView.Nodes[i].Text;
                fmi.Parent = tView.Nodes[i].Name;
                fmiFavor.Add(fmi);
                int index = FindMenuItem(fmi.MenuID, fmiAll);
                if (index != -1)
                    fmiAll.RemoveAt(index);
                SearchNodes(tView.Nodes[i]);
            }
            tView.Nodes.Clear();
            initializeTreeView();

            lbFavor.Items.Clear();
            initializeListBox();
        }

        private void btnRemoveAll_Click(object sender, EventArgs e)
        {
            fmiAll.AddRange(fmiFavor);
            fmiFavor.Clear();
            tView.Nodes.Clear();
            initializeTreeView();

            lbFavor.Items.Clear();
            initializeListBox();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (tView.SelectedNode != null)
            {
                FavorMenuItem fmi = new FavorMenuItem();
                fmi.MenuID = tView.SelectedNode.Tag.ToString();
                fmi.Caption = tView.SelectedNode.Text;
                fmi.Parent = tView.SelectedNode.Name;
                int index = FindMenuItem(fmi.MenuID, fmiAll);
                if (index != -1)
                    fmiAll.RemoveAt(index);
                fmiFavor.Add(fmi);
                //SearchNodes(tView.SelectedNode);

                tView.Nodes.Clear();
                initializeTreeView();

                lbFavor.Items.Clear();
                initializeListBox();
            }
        }

        private int FindMenuItem(String menuID, List<FavorMenuItem> fmis)
        {
            for (int i = 0; i < fmis.Count; i++)
            {
                if (fmis[i].MenuID == menuID)
                    return i;
            }
            return -1;
        }

        private void SearchNodes(TreeNode tn)
        {
            for (int i = 0; i < tn.Nodes.Count; i++)
            {
                FavorMenuItem fmiTemp = new FavorMenuItem();
                fmiTemp.MenuID = tn.Nodes[i].Tag.ToString();
                fmiTemp.Caption = tn.Nodes[i].Text;
                fmiTemp.Parent = tn.Nodes[i].Name;
                int index = FindMenuItem(fmiTemp.MenuID, fmiAll);
                if (index != -1)
                    fmiAll.RemoveAt(index);
                fmiFavor.Add(fmiTemp);
                SearchNodes(tn.Nodes[i]);
            }
        }

        private void initializeListBox()
        {
            for (int i = 0; i < fmiFavor.Count; i++)
            {
                lbFavor.Items.Add(fmiFavor[i].Caption + "(" + fmiFavor[i].MenuID + ")");
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (this.lbFavor.SelectedItems.Count > 0)
            {
                for (int i = 0; i < this.lbFavor.SelectedItems.Count; i++)
                {
                    String selectItem = this.lbFavor.SelectedItems[i].ToString();
                    String[] temp = selectItem.Split(new String[] {"(", ")"}, StringSplitOptions.RemoveEmptyEntries);
                    String selectID = temp[1];
                    FavorMenuItem fmi = new FavorMenuItem();
                    for (int j = 0; j < fmiFavor.Count; j++)
                    {
                        if (selectID == fmiFavor[j].MenuID)
                        {
                            fmi = fmiFavor[j];
                            fmiFavor.RemoveAt(j);
                            break;
                        }
                    }
                    fmiAll.Add(fmi);
                }

                tView.Nodes.Clear();
                initializeTreeView();

                lbFavor.Items.Clear();
                initializeListBox();
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            ArrayList menuID = new ArrayList();
            ArrayList caption = new ArrayList();
            for (int i = 0; i < this.fmiFavor.Count; i++)
            {
                caption.Add(fmiFavor[i].Caption);
                menuID.Add(fmiFavor[i].MenuID);
            }
            object[] param = new object[5];
            param[0] = CliUtils.fLoginUser;
            param[1] = CliUtils.fCurrentProject;
            param[2] = menuID;
            param[3] = caption;
            param[4] = cbGroup.Text;

            CliUtils.CallMethod("GLModule", "GetFavorMenuID", param);
            result = true;
            this.Close();
        }

        private void cbGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.lbFavor.Items.Clear();

            DataSet dsFavorMenus = new DataSet();
            object[] strParam = new object[2];
            strParam[0] = CliUtils.fCurrentProject;
            strParam[1] = "F";
            object[] favorMenus = CliUtils.CallMethod("GLModule", "FetchFavorMenus", strParam);
            if (favorMenus != null && Convert.ToInt16(favorMenus[0]) == 0)
            {
                dsFavorMenus = favorMenus[1] as DataSet;
            }
            if (dsFavorMenus.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsFavorMenus.Tables[0].Rows.Count; i++)
                    if (dsFavorMenus.Tables[0].Rows[i]["GROUPNAME"].ToString() == cbGroup.Text)
                        this.lbFavor.Items.Add(dsFavorMenus.Tables[0].Rows[i]["CAPTION"].ToString() + "(" + dsFavorMenus.Tables[0].Rows[i]["MENUID"].ToString() + ")");
            }
        }
    }

    public class FavorMenuItem
    {
        public String MenuID = String.Empty;
        public String Caption = String.Empty;
        public String Parent = String.Empty;

        public FavorMenuItem()
        {

        }
    }
}