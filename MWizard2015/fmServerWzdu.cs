using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using EnvDTE;
using EnvDTE80;
using System.Collections;
using System.Data.SqlClient;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.OracleClient;
using System.ComponentModel.Design;
using System.IO;
using Srvtools;
using System.Xml;
using EEPLibrary;
using System.Runtime.InteropServices;
using System.Data.Common;
using Microsoft.Win32;
using System.Reflection;

namespace MWizard2015
{

    public partial class fmServerWzd : Form
    {
        private TServerData FServerData;
        private DTE2 FDTE2 = null;
        private AddIn FAddIn = null;
        private DbConnection InternalConnection = null;
        private TStringList FAlias;
        private static string _serverPath;
        private EEPWizard FEEPWizard;
        private ListViewColumnSorter lvwColumnSorter;

        public fmServerWzd()
        {
            InitializeComponent();
            FServerData = new TServerData(this);
            //PrepareWizardService();
            lvwColumnSorter = new ListViewColumnSorter();
            this.lvSelectedFields.ListViewItemSorter = lvwColumnSorter;

        }

        public fmServerWzd(DTE2 aDTE2, AddIn aAddIn, EEPWizard aEEPWizard)
        {
            InitializeComponent();
            FServerData = new TServerData(this);
            FDTE2 = aDTE2;
            FAddIn = aAddIn;
            FEEPWizard = aEEPWizard;
            //PrepareWizardService();
            lvwColumnSorter = new ListViewColumnSorter();
            this.lvSelectedFields.ListViewItemSorter = lvwColumnSorter;
        }

        private void PrepareWizardService()
        {
            Show();
            Hide();
        }

        public TStringList AliasList
        {
            get { return FAlias; }
        }

        private void ClearValues()
        {
            tbConnectionString.Text = "";
            tbCurrentSolution.Text = "";
            tbNewLocation.Text = "";
            tbNewSolutionName.Text = "";
            tbPackageName.Text = "ServerPackage";
            tbSolutionName.Text = "";
            tbOutputPath.Text = "";
            tbAssemblyOutputPath.Text = "";
            tvTables.Nodes.Clear();
            lvSelectedFields.Items.Clear();
            FServerData.Datasets.Clear();
        }

        public void ShowServerWizard()
        {
            //Show();
            Init();
            ShowDialog();
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == 10001)
            {
                SDGenServerModule("");
            }
        }

        public void SDGenServerModule(string XML)
        {
            if (XML != "")
            {
                FServerData.Datasets.Clear();
                FServerData.LoadFromXML(XML);
            }
            TServerGenerator SG = new TServerGenerator(FServerData, FDTE2, FAddIn);
            SG.GenServerModule();
        }

        private void Init()
        {
            ClearValues();
            LoadDBString();
            tabControl.SelectedTab = tpConnection;
            btnNewSubDataset.Enabled = false;
            btnDeleteDataset.Enabled = false;
            btnNewField.Enabled = false;
            btnDeleteField.Enabled = false;
            if (((FDTE2 != null) && (FDTE2.Solution.FileName != "")) && File.Exists(FDTE2.Solution.FileName))
            {
                rbAddToCurrent.Enabled = true;
                rbAddToCurrent.Checked = true;
                tbCurrentSolution.Text = FDTE2.Solution.FileName;
                EnabledOutputControls();
            }

            try
            {
                cbEEPAlias.Text = EEPRegistry.WizardConnectionString;
                cbDatabaseType.Text = EEPRegistry.DataBaseType;
            }
            catch
            {

            }
            DisplayPage(tpConnection);
        }

        public TServerData ServerData
        {
            get
            {
                return FServerData;
            }
        }

        public void CheckCurrentSolution()
        {
        }

        public DbConnection GlobalConnection
        {
            get
            {
                return InternalConnection;
            }
        }

        private void DisplayPage(TabPage aPage)
        {
            while (tabControl.TabPages.Count > 0)
                tabControl.TabPages.Remove(tabControl.TabPages[0]);
            tabControl.TabPages.Add(aPage);
            tabControl.SelectedTab = aPage;
            EnableButton();
        }

        private void EnableButton()
        {
            btnPrevious.Enabled = tabControl.SelectedTab != tpConnection;
            btnNext.Enabled = tabControl.SelectedTab != tpTables;
            btnDone.Enabled = tabControl.SelectedTab == tpTables;
            btnCancel.Enabled = true;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab.Equals(tpConnection))
            {
                WzdUtils.SetRegistryValueByKey("WizardConnectionString", cbEEPAlias.Text);
                WzdUtils.SetRegistryValueByKey("DatabaseType", cbDatabaseType.Text);
                FServerData.ConnectionString = tbConnectionString.Text;
                FServerData.DatabaseType = (ClientType)cbDatabaseType.SelectedIndex;
                FServerData.EEPAlias = cbEEPAlias.Text;
                if (FDTE2.Solution.FullName == null || FDTE2.Solution.FullName == "")
                {
                    rbAddToExistSln.Checked = true;
                    rbAddToExistSln_Click(rbAddToExistSln, null);
                }
                else
                    rbAddToCurrent.Checked = true;
                DisplayPage(tpOutputSetting);
                if (cbChooseLanguage.Text == "" || cbChooseLanguage.Text == "C#")
                    FServerData.Language = "cs";
                else if (cbChooseLanguage.Text == "VB")
                    FServerData.Language = "vb";
            }
            else if (tabControl.SelectedTab.Equals(tpOutputSetting))
            {
                if (rbAddToCurrent.Checked)
                {
                    FServerData.SolutionName = tbCurrentSolution.Text;
                }
                if (rbNewSolution.Checked)
                {
                    if (tbNewSolutionName.Text == "")
                    {
                        MessageBox.Show("Please input Solution Name !!");
                        if (tbNewSolutionName.CanFocus)
                        {
                            tbNewSolutionName.Focus();
                        }
                        return;
                    }
                    if (tbNewLocation.Text == "")
                    {
                        MessageBox.Show("Please input Location !!");
                        if (tbNewLocation.CanFocus)
                        {
                            tbNewLocation.Focus();
                        }
                        return;
                    }
                    FServerData.SolutionName = tbNewSolutionName.Text;
                    FServerData.OutputPath = tbNewLocation.Text;
                    FServerData.CodeOutputPath = tbOutputPath.Text;
                }
                if (rbAddToExistSln.Checked)
                {
                    if (tbSolutionName.Text == "")
                    {
                        MessageBox.Show("Please input Location !!");
                        if (tbSolutionName.CanFocus)
                        {
                            tbSolutionName.Focus();
                        }
                        return;
                    }
                    FServerData.SolutionName = tbSolutionName.Text;
                    FServerData.CodeOutputPath = tbOutputPath.Text;
                }
                if (tbPackageName.Text == "")
                {
                    MessageBox.Show("Please input Package Name !!");
                    if (tbPackageName.CanFocus)
                    {
                        tbPackageName.Focus();
                    }
                }
                else
                {
                    FServerData.PackageName = tbPackageName.Text;
                    FServerData.NewSolution = rbNewSolution.Checked;
                    FServerData.CodeOutputPath = tbOutputPath.Text;
                    DisplayPage(tpTables);
                }
                FServerData.AssemblyOutputPath = tbAssemblyOutputPath.Text;
            }
            BringToFront();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab.Equals(tpOutputSetting))
            {
                DisplayPage(tpConnection);
            }
            else if (tabControl.SelectedTab.Equals(tpTables))
            {
                DisplayPage(tpOutputSetting);
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            TreeNode Node = tvTables.SelectedNode;
            string TableName = "";
            MWizard2015.fmSelTableField F;

            if (Node != null)
                TableName = Node.Text;
            F = new fmSelTableField();
            if (F.ShowSelTableFieldForm(FServerData, FServerData.DatabaseName, ref TableName, InternalConnection))
            {
                Node = tvTables.Nodes.Add(TableName);
                Node.Name = TableName;
                AddDatasetNode(Node);
                tvTables.SelectedNode = Node;

                String sQL = "select * from " + WzdUtils.Quote(TableName, GlobalConnection) + " where 1=0";
                IDbCommand cmd = GlobalConnection.CreateCommand();
                cmd.CommandText = sQL;
                if (GlobalConnection.State == ConnectionState.Closed)
                { GlobalConnection.Open(); }

                DataSet schemaTable = new DataSet();
                IDbDataAdapter ida = WzdUtils.AllocateDataAdapter(FServerData.DatabaseType);
                ida.SelectCommand = cmd;
                ida.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                ida.Fill(schemaTable);

                lvSelectedFields.Items.Clear();
                TDatasetItem tdsItem = null;
                foreach (TDatasetItem item in FServerData.Datasets)
                {
                    if (item.TableName == Node.Text)
                    {
                        tdsItem = item;
                        break;
                    }
                }

                foreach (DataColumn DR in schemaTable.Tables[0].Columns)
                {
                    lvSelectedFields.Items.Add(DR.ColumnName);
                    TFieldAttrItem Item = new TFieldAttrItem();
                    tdsItem.FieldAttrItems.Add(Item);
                    Item.DataField = lvSelectedFields.Items[lvSelectedFields.Items.Count - 1].Text;
                    if (lvSelectedFields.Items[lvSelectedFields.Items.Count - 1].SubItems.Count > 1)
                    {
                        if (tdsItem.FieldCaptionList.Count > 0)
                        {
                            Item.Description = tdsItem.FieldCaptionList.Values(Item.DataField);
                        }
                        else
                        {
                            Item.Description = lvSelectedFields.Items[lvSelectedFields.Items.Count - 1].SubItems[1].Text;
                        }
                    }
                    else
                    {
                        if (tdsItem.FieldCaptionList.Count > 0)
                        {
                            Item.Description = tdsItem.FieldCaptionList.Values(Item.DataField);
                            //lvSelectedFields.Items[lvSelectedFields.Items.Count - 1].SubItems.Add(tdsItem.FieldCaptionList.Values(Item.DataField));
                        }
                    }
                    if (!String.IsNullOrEmpty(Item.Description))
                        lvSelectedFields.Items[lvSelectedFields.Items.Count - 1].SubItems.Add(Item.Description);
                    lvSelectedFields.Items[lvSelectedFields.Items.Count - 1].Tag = Item;
                }

                foreach (DataColumn DR in schemaTable.Tables[0].PrimaryKey)
                {
                    for (int I = 0; I < lvSelectedFields.Items.Count; I++)
                    {
                        if (lvSelectedFields.Items[I].Text == DR.ColumnName)
                        {
                            TFieldAttrItem fi = (TFieldAttrItem)lvSelectedFields.Items[I].Tag;
                            fi.IsKey = true;
                        }
                    }
                }

                Node.Tag = tdsItem;
                if (lvSelectedFields.Items.Count > 0)
                {
                    lvSelectedFields.Items[0].Selected = true;
                }

                btnDeleteField.Enabled = lvSelectedFields.Items.Count > 0;
            }
        }

        private void AddDatasetNode(TreeNode Node)
        {
            TDatasetItem Item = new TDatasetItem();
            Item.DatabaseType = FServerData.DatabaseType;
            FServerData.Datasets.Add(Item);
            Item.DatabaseName = FServerData.DatabaseName;
            Item.TableName = Node.Text;
            Node.Tag = Item;
            if (Node.Parent != null)
            {
                TDatasetItem ParentItem = (TDatasetItem)Node.Parent.Tag;
                ParentItem.ChildItem = Item;
                Item.ParentItem = ParentItem;
            }
        }

        private void GetFieldList(string DatabaseName, string TableName, TStringList FieldNameList)
        {
            String Owner = "";
            if (TableName.IndexOf('.') > -1)
            {
                Owner = WzdUtils.GetToken(ref TableName, new char[] { '.' });
            }

            string[] S = new string[4];
            S[1] = Owner;
            S[2] = TableName;
            String SortName = "ORDINAL_POSITION";
            if (FServerData.DatabaseType == ClientType.ctOracle)
            {
                String UserID = WzdUtils.GetFieldParam(FServerData.ConnectionString.ToLower(), "user id");
                S = new String[] { UserID.ToUpper(), TableName };
                SortName = "ID";
            }
            DataTable D = FServerData.Owner.GlobalConnection.GetSchema("Columns", S);
            DataRow[] DRs = D.Select("", SortName + " ASC");

            foreach (DataRow DR in DRs)
                FieldNameList.Add(DR["COLUMN_NAME"].ToString());
        }

        //private void GetCaptionFromCOLDEF(string DatabaseName, string TableName, TStringList FieldCaptionList)
        //{
        //    InfoCommand aInfoCommand = new InfoCommand(FServerData.DatabaseType);
        //    aInfoCommand.Connection = InternalConnection;
        //    TableName = WzdUtils.RemoveQuote(TableName, FServerData.DatabaseType);
        //    aInfoCommand.CommandText = "Select FIELD_NAME,CAPTION from COLDEF where TABLE_NAME = '" + TableName + "'";
        //    IDbDataAdapter DA = DBUtils.CreateDbDataAdapter(aInfoCommand);
        //    DataSet D = new DataSet();
        //    WzdUtils.FillDataAdapter(FServerData.DatabaseType, DA, D, "COLDEF");
        //    FieldCaptionList.Clear();
        //    int I;
        //    DataRow DR;
        //    for (I = 0; I < D.Tables[0].Rows.Count; I++)
        //    {
        //        DR = D.Tables[0].Rows[I];
        //        if (DR["FIELD_NAME"].ToString() != "")
        //            FieldCaptionList.Add(DR["FIELD_NAME"] + "=" + DR["CAPTION"]);
        //    }
        //}

        public delegate void GetFieldNamesFunc(string DatabaseName, string TableName, String DataSetName, ListView SrcListView, ListView DestListView);

        public void GetFieldNames(string DatabaseName, string TableName, String DataSetName, ListView SrcListView, ListView DestListView)
        {
            GetFieldNamesEx(null, DatabaseName, TableName, DataSetName, SrcListView, DestListView);
        }

        public delegate void GetFieldNamesExFunc(TDatasetItem aDatasetItem, string DatabaseName, string TableName, String DataSetName, ListView SrcListView, ListView DestListView);

        private void GetFieldNamesEx(TDatasetItem aDatasetItem, string DatabaseName, string TableName, String DataSetName, ListView SrcListView, ListView DestListView)
        {
            int I, J = 0;
            bool Found = false;
            ListViewItem lvi = null;
            //TFieldAttrItem tai;
            TStringList aPhysFieldNameList = new TStringList();
            TStringList aFieldCaptionList = new TStringList();

            if (aDatasetItem != null)
            {
                aPhysFieldNameList = aDatasetItem.FieldList;
                aFieldCaptionList = aDatasetItem.FieldCaptionList;
            }
            else
            {
                GetFieldList(DatabaseName, TableName, aPhysFieldNameList);
            }

            /*
            for (I = 0; I < DestListView.Items.Count - 1; I++)
            { 
                lvi = DestListView.Items[I];
                if (aPhysFieldNameList.IndexOf(lvi.Text) < 0)
                {
                    tai = (TFieldAttrItem)lvi.Tag;
                    }
            }
            */

            SrcListView.Items.Clear();
            for (I = 0; I < aPhysFieldNameList.Count; I++)
            {
                Found = false;
                for (J = 0; J < DestListView.Items.Count; J++)
                {
                    lvi = DestListView.Items[J];
                    if (string.Compare(aPhysFieldNameList[I].ToString(), lvi.Text, false) == 0)
                    {
                        Found = true;
                        break;
                    }
                }
                if (Found == false)
                {
                    lvi = SrcListView.Items.Add(aPhysFieldNameList[I].ToString());
                    lvi.SubItems.Add(aFieldCaptionList.Values(lvi.Text));
                }

            }

            if (SrcListView.Items.Count > 0)
                SrcListView.Items[0].Selected = true;
        }

        private void btnAddField_Click(object sender, EventArgs e)
        {
            TreeNode Node = tvTables.SelectedNode;
            if (Node != null)
            {
                TDatasetItem DatasetItem = (TDatasetItem)Node.Tag;
                MWizard2015.fmSelTableField F = new fmSelTableField();
                if (F.ShowSelTableFieldForm(DatasetItem, GetFieldNamesEx, lvSelectedFields, InternalConnection, FServerData.DatabaseType))
                {
                    btnDeleteField.Enabled = lvSelectedFields.Items.Count > 0;


                    String sQL = "select * from " + WzdUtils.Quote(DatasetItem.TableName, GlobalConnection) + " where 1=0";
                    IDbCommand cmd = GlobalConnection.CreateCommand();
                    cmd.CommandText = sQL;
                    if (GlobalConnection.State == ConnectionState.Closed)
                    { GlobalConnection.Open(); }

                    DataSet schemaTable = new DataSet();
                    IDbDataAdapter ida = WzdUtils.AllocateDataAdapter(DatasetItem.DatabaseType);
                    ida.SelectCommand = cmd;
                    ida.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                    ida.Fill(schemaTable);

                    foreach (DataColumn DR in schemaTable.Tables[0].PrimaryKey)
                    {
                        for (int I = 0; I < lvSelectedFields.Items.Count; I++)
                        {
                            if (lvSelectedFields.Items[I].Text == DR.ColumnName)
                            {
                                TFieldAttrItem fi = (TFieldAttrItem)lvSelectedFields.Items[I].Tag;
                                fi.IsKey = true;
                                break;
                            }
                        }
                    }
                }
            }
        }

        private void btnAddNext_Click(object sender, EventArgs e)
        {
            TreeNode Node = tvTables.SelectedNode;
            if (Node != null)
            {
                fmSelTableField F = new fmSelTableField();
                string TableName = "";
                if (Node != null)
                    TableName = Node.Text;
                if (F.ShowSelTableFieldForm(FServerData, FServerData.DatabaseName, ref TableName, InternalConnection))
                {
                    TreeNode node2 = new TreeNode();
                    node2.Text = TableName;
                    node2.Name = TableName;
                    Node.Nodes.Add(node2);
                    AddDatasetNode(node2);
                    tvTables.SelectedNode = node2;

                    String sQL = "select * from " + WzdUtils.Quote(TableName, GlobalConnection) + " where 1=0";
                    IDbCommand cmd = GlobalConnection.CreateCommand();
                    cmd.CommandText = sQL;
                    if (GlobalConnection.State == ConnectionState.Closed)
                    { GlobalConnection.Open(); }

                    DataSet schemaTable = new DataSet();
                    IDbDataAdapter ida = WzdUtils.AllocateDataAdapter(FServerData.DatabaseType);
                    ida.SelectCommand = cmd;
                    ida.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                    ida.Fill(schemaTable);

                    lvSelectedFields.Items.Clear();
                    TDatasetItem tdsItem = null;
                    foreach (TDatasetItem item in FServerData.Datasets)
                    {
                        if (item.TableName == node2.Text)
                        {
                            tdsItem = item;
                            break;
                        }
                    }

                    foreach (DataColumn DR in schemaTable.Tables[0].Columns)
                    {
                        lvSelectedFields.Items.Add(DR.ColumnName);
                        TFieldAttrItem Item = new TFieldAttrItem();
                        tdsItem.FieldAttrItems.Add(Item);
                        Item.DataField = lvSelectedFields.Items[lvSelectedFields.Items.Count - 1].Text;
                        //if (lvSelectedFields.Items[lvSelectedFields.Items.Count - 1].SubItems.Count > 1)
                        //    Item.Description = lvSelectedFields.Items[lvSelectedFields.Items.Count - 1].SubItems[1].Text;

                        if (lvSelectedFields.Items[lvSelectedFields.Items.Count - 1].SubItems.Count > 1)
                        {
                            if (tdsItem.FieldCaptionList.Count > 0)
                            {
                                Item.Description = tdsItem.FieldCaptionList.Values(Item.DataField);
                            }
                            else
                            {
                                Item.Description = lvSelectedFields.Items[lvSelectedFields.Items.Count - 1].SubItems[1].Text;
                            }
                        }
                        else
                        {
                            if (tdsItem.FieldCaptionList.Count > 0)
                            {
                                Item.Description = tdsItem.FieldCaptionList.Values(Item.DataField);
                                //lvSelectedFields.Items[lvSelectedFields.Items.Count - 1].SubItems.Add(tdsItem.FieldCaptionList.Values(Item.DataField));
                            }
                        }
                        if (!String.IsNullOrEmpty(Item.Description))
                            lvSelectedFields.Items[lvSelectedFields.Items.Count - 1].SubItems.Add(Item.Description);
                        lvSelectedFields.Items[lvSelectedFields.Items.Count - 1].Tag = Item;
                    }

                    foreach (DataColumn DR in schemaTable.Tables[0].PrimaryKey)
                    {
                        for (int I = 0; I < lvSelectedFields.Items.Count; I++)
                        {
                            if (lvSelectedFields.Items[I].Text == DR.ColumnName)
                            {
                                TFieldAttrItem fi = (TFieldAttrItem)lvSelectedFields.Items[I].Tag;
                                fi.IsKey = true;
                            }
                        }
                    }

                    node2.Tag = tdsItem;
                    if (lvSelectedFields.Items.Count > 0)
                    {
                        lvSelectedFields.Items[0].Selected = true;
                    }
                }
            }

        }

        private void UpdatelvSelectedFields(TDatasetItem DatasetItem)
        {
            cbIsKey.Checked = false;
            cbCheckNull.Checked = false;
            //cbIsRelationKey.Checked = false;
            lvSelectedFields.Items.Clear();
            if (DatasetItem != null)
            {
                lvSelectedFields.BeginUpdate();
                for (int num1 = 0; num1 < DatasetItem.FieldAttrItems.Count; num1++)
                {
                    TFieldAttrItem item1 = DatasetItem.FieldAttrItems[num1] as TFieldAttrItem;
                    ListViewItem item2 = lvSelectedFields.Items.Add(item1.DataField);
                    item2.SubItems.Add(item1.Description);
                    item2.Tag = item1;
                }
                lvSelectedFields.EndUpdate();
                btnDeleteField.Enabled = lvSelectedFields.Items.Count > 0;
            }

        }

        private void tvTables_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode Node = tvTables.SelectedNode;
            btnNewSubDataset.Enabled = Node != null;
            btnDeleteDataset.Enabled = btnNewSubDataset.Enabled;
            btnNewField.Enabled = btnNewSubDataset.Enabled;
            TDatasetItem aItem = (TDatasetItem)Node.Tag;
            UpdatelvSelectedFields(aItem);
            btnRelation.Enabled = aItem.ParentItem != null;
        }

        private void lvSelectedFields_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            cbIsKey.Enabled = lvSelectedFields.SelectedItems.Count > 0;
            cbCheckNull.Enabled = lvSelectedFields.SelectedItems.Count > 0;
            //cbIsRelationKey.Enabled = lvSelectedFields.SelectedItems.Count > 0;
            if (lvSelectedFields.SelectedItems.Count == 1)
            {
                ListViewItem Item = lvSelectedFields.SelectedItems[0];
                TFieldAttrItem FieldItem = (TFieldAttrItem)Item.Tag;
                cbIsKey.Checked = FieldItem.IsKey;
                cbCheckNull.Checked = FieldItem.CheckNull;
                //cbIsRelationKey.Checked = FieldItem.IsRelationKey;
            }
            else
            {
                cbIsKey.Checked = false;
                cbCheckNull.Checked = false;
                //cbIsRelationKey.Checked = false;
            }
        }

        private void SetFieldAttrOption(string OptionName)
        {
            int I;
            ListViewItem Item;
            TFieldAttrItem FieldItem;
            for (I = 0; I < lvSelectedFields.Items.Count; I++)
            {
                if (lvSelectedFields.Items[I].Selected)
                {
                    Item = lvSelectedFields.Items[I];
                    FieldItem = (TFieldAttrItem)Item.Tag;
                    if (string.Compare(OptionName, "IsKey") == 0)
                        FieldItem.IsKey = cbIsKey.Checked;
                    if (string.Compare(OptionName, "CheckNull") == 0)
                        FieldItem.CheckNull = cbCheckNull.Checked;
                    //if (string.Compare(OptionName, "IsRelationKey") == 0)
                    //	   FieldItem.IsRelationKey = cbIsRelationKey.Checked;
                }
            }
        }

        private void cbIsKey_Click(object sender, EventArgs e)
        {
            SetFieldAttrOption("IsKey");
        }

        private void cbCheckNull_Click(object sender, EventArgs e)
        {
            SetFieldAttrOption("CheckNull");
        }

        private void cbIsRelationKey_Click(object sender, EventArgs e)
        {
            SetFieldAttrOption("IsRelationKey");
        }

        private void btnDeleteField_Click(object sender, EventArgs e)
        {
            int I;
            ListViewItem Item;
            TFieldAttrItem FieldItem;
            Boolean HaveDelete = false;
            for (I = lvSelectedFields.Items.Count - 1; I >= 0; I--)
            {
                if (lvSelectedFields.Items[I].Selected)
                {
                    Item = lvSelectedFields.Items[I];
                    FieldItem = (TFieldAttrItem)Item.Tag;
                    FieldItem.Collection.Remove(FieldItem);
                    lvSelectedFields.Items.Remove(Item);
                    HaveDelete = true;
                }
            }

            if (HaveDelete)
            {
                TreeNode Node = tvTables.SelectedNode;
                ((TDatasetItem)Node.Tag).AddAll = false;
            }
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            //Solution sln = FDTE2.Solution;

            //string vsWizardAddItem = "{0F90E1D1-4999-11D1-B6D1-00A0C90F2744}";//WizardType Guid
            //bool silent = false;

            //DTE dte = System.Runtime.InteropServices.Marshal.GetActiveObject("VisualStudio.DTE.10.0") as DTE;
            //object[] obj = dte.ActiveSolutionProjects as object[];
            //Project p1 = null;
            //if (obj.Length > 0)
            //    p1 = obj[0] as Project;

            //int commonIndex = FDTE2.Application.FileName.IndexOf(@"\Common7");
            //string vsInstallPath = FDTE2.Application.FileName.Substring(0, commonIndex);

            ////Project p = sln.Projects.Item(19);

            //string itemName = p1.Name + ".edmx";
            //string localDir = System.IO.Path.GetDirectoryName(p1.FullName);

            ////object[] prams = {vsWizardAddItem,"Project10","C:\\MyProjects",
            ////             vsInstallPath, false,"Solution10", silent};

            ////Solution2 soln = (Solution2)FDTE2.Solution;
            ////string  templatePath = soln.GetProjectTemplate("ConsoleApplication.zip", "CSharp");

            //object[] prams = {vsWizardAddItem,p1.Name, p1.ProjectItems,
            //                 "C:\\MyProjects", itemName, vsInstallPath, silent};
            //Solution2 soln = (Solution2)FDTE2.Solution;
            //string templatePath = soln.GetProjectItemTemplate("AdoNetEntityDataModelCSharp.zip", "CSharp");
            //EnvDTE.wizardResult res = FDTE2.LaunchWizard(templatePath, ref prams);
            Hide();
            FDTE2.MainWindow.Activate();
            TServerGenerator SG = new TServerGenerator(FServerData, FDTE2, FAddIn);
            SG.GenServerModule();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                tbSolutionName.Text = openFileDialog.FileName;
            }
        }

        private void EnabledOutputControls()
        {
            tbNewSolutionName.Enabled = rbNewSolution.Checked;
            tbNewLocation.Enabled = rbNewSolution.Checked;
            btnNewLocation.Enabled = rbNewSolution.Checked;
            tbSolutionName.Enabled = rbAddToExistSln.Checked;
            btnSolutionName.Enabled = rbAddToExistSln.Checked;
        }

        private void rbNewSolution_Click(object sender, EventArgs e)
        {
            EnabledOutputControls();
            SetCodeOutputPath();
        }

        private void rbAddToExistSln_Click(object sender, EventArgs e)
        {
            EnabledOutputControls();
            SetCodeOutputPath();
        }

        private void btnNewSolution_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                tbNewLocation.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void tbNewSolutionName_TextChanged(object sender, EventArgs e)
        {
            tbAssemblyOutputPath.Text = String.Format(@"..\..\EEPNetServer\{0}", tbNewSolutionName.Text);
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            TServerGenerator S = new TServerGenerator(FServerData, FDTE2, FAddIn);
            // /*
            FServerData.SolutionName = tbSolutionName.Text;
            FServerData.OutputPath = "";
            FServerData.NewSolution = false;
            FServerData.PackageName = "ExistSolution";
            // */
            /*
        FServerData.SolutionName = tbNewSolutionName.Text;
        FServerData.OutputPath = tbNewLocation.Text;
        FServerData.NewSolution = true;
        FServerData.PackageName = "NewSolution";
            */
            S.GenServerModule();
            Hide();
        }

        private void rbAddToCurrent_Click(object sender, EventArgs e)
        {
            EnabledOutputControls();
            SetCodeOutputPath();
        }


        private void button3_Click(object sender, EventArgs e)
        {
        }

        private void btnConnectionString_Click(object sender, EventArgs e)
        {
            ADODB.Connection Conn = null;
            String ConnectionString = tbConnectionString.Text;
            MSDASC.DataLinks dataLinks = new MSDASC.DataLinksClass();
            if (ConnectionString == string.Empty)
            {
                Conn = (ADODB.Connection)dataLinks.PromptNew();
            }
            else
            {
                Conn = new ADODB.Connection();
                Conn.ConnectionString = tbConnectionString.Text;
                object TempConn = Conn;
                if (dataLinks.PromptEdit(ref TempConn))
                    ConnectionString = Conn.ConnectionString;
            }
            tbConnectionString.Text = Conn.ConnectionString;
        }

        private static string GetServerPath()
        {
            if ((fmServerWzd._serverPath == null) || (fmServerWzd._serverPath.Length == 0))
            {
                fmServerWzd._serverPath = EEPRegistry.Server + "\\";
            }
            return fmServerWzd._serverPath;
        }

        private void LoadDBString()
        {
            cbEEPAlias.Items.Clear();
            FAlias = new TStringList();
            List<string> list1 = new List<string>();
            string text3 = SystemFile.DBFile;
            XmlDocument document1 = new XmlDocument();
            document1.Load(text3);
            foreach (XmlNode node1 in document1.FirstChild.FirstChild.ChildNodes)
            {
                list1.Add((string)node1.Name);
                cbEEPAlias.Items.Add(node1.Name);
                FServerData.DatabaseType = (ClientType)int.Parse(node1.Attributes["Type"].Value);
                string text1 = node1.Attributes["String"].Value.Trim();
                string text2 = WzdUtils.GetPwdString(node1.Attributes["Password"].Value.Trim());
                if ((text1.Length > 0) && (text2.Length > 0) && text2 != String.Empty)
                {
                    if (text1[text1.Length - 1] != ';')
                    {
                        text1 = text1 + ";Password=" + text2;
                    }
                    else
                    {
                        text1 = text1 + "Password=" + text2;
                    }
                }
                FAlias.AddObject(node1.Name, text1);
            }

        }

        private void fmServerWzd_Load(object sender, EventArgs e)
        {
            //???LoadDBString();
        }

        public String FindDBType(String aliasName)
        {
            String xmlName = SystemFile.DBFile;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlName);

            XmlNode node = xmlDoc.FirstChild.FirstChild.SelectSingleNode(aliasName);

            string DbType = node.Attributes["Type"].Value.Trim();
            return DbType;
        }

        private void cbEEPAlias_SelectedIndexChanged(object sender, EventArgs e)
        {
            string type = FindDBType(cbEEPAlias.Text);
            int num1 = FAlias.IndexOf(cbEEPAlias.Text);
            FServerData.EEPAlias = cbEEPAlias.Text;
            tbConnectionString.Text = (string)FAlias.Objects(num1);
            switch (type)
            {
                case "1":
                    FServerData.DatabaseType = ClientType.ctMsSql; break;
                case "2":
                    FServerData.DatabaseType = ClientType.ctOleDB; break;
                case "3":
                    FServerData.DatabaseType = ClientType.ctOracle; break;
                case "4":
                    FServerData.DatabaseType = ClientType.ctODBC; break;
                case "5":
                    FServerData.DatabaseType = ClientType.ctMySql; break;
                case "6":
                    FServerData.DatabaseType = ClientType.ctInformix; break;
                case "7":
                    FServerData.DatabaseType = ClientType.ctSybase; break;

            }
            cbDatabaseType.SelectedIndex = (int)FServerData.DatabaseType;

            if (InternalConnection == null)
            {
                InternalConnection = WzdUtils.AllocateConnection(this.cbEEPAlias.Text, FServerData.DatabaseType, false);
            }
            else
            {
                if (InternalConnection.State == ConnectionState.Open)
                    InternalConnection.Close();
                InternalConnection = WzdUtils.AllocateConnection(this.cbEEPAlias.Text, FServerData.DatabaseType, false);
                //InternalConnection.ConnectionString = tbConnectionString.Text;
            }

            if (InternalConnection.ConnectionString.Trim() != "")
            {
                try
                {
                    if (InternalConnection.State != ConnectionState.Open)
                        InternalConnection.Open();
                }
                catch (Exception E)
                {
                    MessageBox.Show(string.Format("Database ConnnectionString information error, please reset ConnectionString.\nThe error message:\n{0}", E.Message));
                }
            }
            FServerData.ResetDatabaseConnection();
        }

        private void SetCodeOutputPath()
        {
            // ..\..\EEPNetServer\Solution1
            if (rbAddToCurrent.Checked)
            {
                string S = tbCurrentSolution.Text;
                if (S != "")
                {
                    S = System.IO.Path.GetDirectoryName(S);
                    String SolutionName = Path.GetFileNameWithoutExtension(tbCurrentSolution.Text);
                    tbOutputPath.Text = S + @"\" + SolutionName;
                    tbAssemblyOutputPath.Text = String.Format(@"..\..\EEPNetServer\{0}", SolutionName);
                }
            }
            if (rbNewSolution.Checked)
            {
                tbOutputPath.Text = tbNewLocation.Text;
            }
            if (rbAddToExistSln.Checked)
            {
                string S = tbSolutionName.Text;
                if (S != "")
                {
                    S = System.IO.Path.GetDirectoryName(S);
                    String SolutionName = Path.GetFileNameWithoutExtension(tbSolutionName.Text);
                    tbOutputPath.Text = S + @"\" + SolutionName;
                    tbAssemblyOutputPath.Text = String.Format(@"..\..\EEPNetServer\{0}", SolutionName);
                }
            }
        }

        private void tbCurrentSolution_TextChanged(object sender, EventArgs e)
        {
            SetCodeOutputPath();
        }

        private void btnOutputPath_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                tbOutputPath.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            //Object[] Params = new Object[] { "Solution1", "S002.GROUPS_1" };
            //FEEPWizard.GetTableRelationByProvider(Params);

            Type aInterface = FEEPWizard.GetType().GetInterface("IEEPWizard");
            if (aInterface != null)
            {
                Object Params = new Object[] { 
                   @"C:\Program Files\InfoLight\EEP2006\EEPNetClient\Solution1\C001.dll", 
                    "Form1", 
                    "001", 
                    "", 
                    "ERPS", 
                    "", 
                    "Solution1",
                    @"C:\Program Files\InfoLight\EEP2006\Solution1.sln"};
                FEEPWizard.GetFormImage(Params);
                //IEEPWizard A = (IEEPWizard)aInterface;
                //A.CallMethod("GetFormImage", null);
            }
        }

        private void btnAssemblyOutputPath_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                tbAssemblyOutputPath.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void button2_Click_2(object sender, EventArgs e)
        {
            //Project.SetasStartUpProject

            //MessageBox.Show("1");
            FDTE2.Solution.Open(@"C:\Program Files\InfoLight\EEP2006\Solution1.sln");
            //MessageBox.Show("2");

            FDTE2.Windows.Item(Constants.vsWindowKindSolutionExplorer).Activate();
            //MessageBox.Show("3");
            UIHierarchy A = (UIHierarchy)FDTE2.ActiveWindow.Object;

            foreach (UIHierarchyItem aItem in A.UIHierarchyItems)
            {
                foreach (UIHierarchyItem B in aItem.UIHierarchyItems)
                {
                    if (B.Name.CompareTo("C:\\...\\EEPWebClient\\") == 0)
                    {
                        foreach (UIHierarchyItem C in B.UIHierarchyItems)
                        {
                            if (C.Name.CompareTo("InfoLogin.aspx") == 0)
                            {
                                C.Select(vsUISelectionType.vsUISelectionTypeSelect);
                                try
                                {
                                    FDTE2.MainWindow.Activate();
                                    FDTE2.ActiveWindow.Activate();
                                    MessageBox.Show("1");
                                    C.DTE.ExecuteCommand("File.ViewinBrowser", String.Empty);
                                    MessageBox.Show("2");
                                    //MessageBox.Show("OK");
                                }
                                catch (Exception E)
                                {
                                    MessageBox.Show(E.Message);
                                }
                            }
                        }
                    }
                }
            }

            //return;
            //MessageBox.Show("4");
            try
            {
                String S = "Solution1\\C:\\...\\EEPWebClient\\";
                A.GetItem(S);
                MessageBox.Show("OK");
            }
            catch (Exception E)
            {
                MessageBox.Show(E.Message);
            }
            //MessageBox.Show("4.5");
            A.GetItem(@"EEPWebClient").Select(vsUISelectionType.vsUISelectionTypeSelect);
            //MessageBox.Show("5");
            FDTE2.ExecuteCommand("Project.SetasStartUpProject", null);
            //MessageBox.Show("6");
            A.GetItem(@"Solution1\C:\...\EEPWebClient\").UIHierarchyItems.Expanded = true;
            //MessageBox.Show("7");
            A.GetItem(@"Solution1\C:\...\EEPWebClient\\InfoLogin.aspx").Select(vsUISelectionType.vsUISelectionTypeSelect);
            //MessageBox.Show("8");
            FDTE2.ExecuteCommand("Project.SetAsStartPage", null);
            //MessageBox.Show("9");

            A.GetItem(@"Solution1\C:\...\EEPWebClient\\InfoLogin.aspx").Select(vsUISelectionType.vsUISelectionTypeSelect);
            //MessageBox.Show("10");
            FDTE2.ExecuteCommand("File.ViewinBrowser", null);
            //MessageBox.Show("11");

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            /*
            //OpenPage
            Type bInterface = FEEPWizard.GetType().GetInterface("IEEPWizard");
            if (bInterface != null)
            {
                Object Params = new Object[] { 
                    @"C:\Program Files\InfoLight\EEP2006\Solution1.sln", 
                    @"C:\...\EEPWebClient\",
                    "AP",
                    "Form1.aspx" };
                FEEPWizard.OpenPage(Params);
            }
             */

            /*
            //GetTableRelation
            Type bInterface = FEEPWizard.GetType().GetInterface("IEEPWizard");
            if (bInterface != null)
            {
                Object Params = new Object[] { "Solution1", "S002.GROUPS" };
                FEEPWizard.GetTableRelationByProvider(Params);
                //IEEPWizard A = (IEEPWizard)aInterface;
                //A.CallMethod("GetFormImage", null);
            }
             */

            //GenClientModule
            Type bInterface = FEEPWizard.GetType().GetInterface("IEEPWizard");
            if (bInterface != null)
            {
                System.IO.FileStream file = new System.IO.FileStream("d:\\a.xml", System.IO.FileMode.Open);
                try
                {
                    XmlDocument X = new XmlDocument();
                    X.Load(file);
                    String S = X.OuterXml;
                    Object Params = new Object[] { S }; //{ "<?xml version=\"1.0\" encoding=\"UTF-8\"?><ClientData PackageName=\"c9\" BaseFormName=\"CMasterDetail\" ServerPackageName=\"S002.GROUPS\" OutputPath=\"C:\\Program Files\\InfoLight\\EEP2006\\\" CodeOutputPath=\"C:\\Program Files\\InfoLight\\EEP2006\\Solution1\" TableName=\"GROUPS\" FormName=\"fmc9\" ProviderName=\"S002.GROUPS\" DatabaseName=\"ERPS\" SolutionName=\"C:\\Program Files\\InfoLight\\EEP2006\\Solution1.sln\" NewSolution=\"0\" ColumnCount=\"1\" ViewProviderName=\"S002.View_Provider\"><Blocks><Block0 ProviderName=\"GROUPS\" TableName=\"GROUPS\" Name=\"View\" ParentItemName=\"\"><BlockFieldItems><BlockFieldItem0 DataField=\"GROUPID\" Description=\"\"/><BlockFieldItem1 DataField=\"GROUPNAME\" Description=\"\"/><BlockFieldItem2 DataField=\"DESCRIPTION\" Description=\"\"/></BlockFieldItems></Block0><Block1 ProviderName=\"GROUPS\" TableName=\"GROUPS\" Name=\"Master\" ParentItemName=\"\"><BlockFieldItems><BlockFieldItem0 DataField=\"GROUPID\" Description=\"\"/><BlockFieldItem1 DataField=\"GROUPNAME\" Description=\"\"/><BlockFieldItem2 DataField=\"DESCRIPTION\" Description=\"\"/></BlockFieldItems></Block1><Block2 ProviderName=\"GROUPMENUS\" TableName=\"GROUPMENUS\" Name=\"GROUPMENUS\" ParentItemName=\"Master\"><BlockFieldItems><BlockFieldItem0 DataField=\"GROUPID\" Description=\"\"/><BlockFieldItem1 DataField=\"MENUID\" Description=\"\"/></BlockFieldItems></Block2></Blocks></ClientData>" };
                    FEEPWizard.GenClientModule(Params);
                }
                finally
                {
                    file.Dispose();
                }
            }

            //GenServerModule
            //Type bInterface = FEEPWizard.GetType().GetInterface("IEEPWizard");
            //if (bInterface != null)
            //{
            //    //System.Xml.Serialization.XmlSerializer xs;
            //    //xs = new System.Xml.Serialization.XmlSerializer(typeof(ClassTestData));
            //    System.IO.FileStream file = new System.IO.FileStream("d:\\a.xml", System.IO.FileMode.Open);
            //    try
            //    {
            //        XmlDocument X = new XmlDocument();
            //        X.Load(file);
            //        String S = X.OuterXml;
            //        Object Params = new Object[] { S }; //{ "<?xml version=\"1.0\" encoding=\"UTF-8\"?><ClientData PackageName=\"c9\" BaseFormName=\"CMasterDetail\" ServerPackageName=\"S002.GROUPS\" OutputPath=\"C:\\Program Files\\InfoLight\\EEP2006\\\" CodeOutputPath=\"C:\\Program Files\\InfoLight\\EEP2006\\Solution1\" TableName=\"GROUPS\" FormName=\"fmc9\" ProviderName=\"S002.GROUPS\" DatabaseName=\"ERPS\" SolutionName=\"C:\\Program Files\\InfoLight\\EEP2006\\Solution1.sln\" NewSolution=\"0\" ColumnCount=\"1\" ViewProviderName=\"S002.View_Provider\"><Blocks><Block0 ProviderName=\"GROUPS\" TableName=\"GROUPS\" Name=\"View\" ParentItemName=\"\"><BlockFieldItems><BlockFieldItem0 DataField=\"GROUPID\" Description=\"\"/><BlockFieldItem1 DataField=\"GROUPNAME\" Description=\"\"/><BlockFieldItem2 DataField=\"DESCRIPTION\" Description=\"\"/></BlockFieldItems></Block0><Block1 ProviderName=\"GROUPS\" TableName=\"GROUPS\" Name=\"Master\" ParentItemName=\"\"><BlockFieldItems><BlockFieldItem0 DataField=\"GROUPID\" Description=\"\"/><BlockFieldItem1 DataField=\"GROUPNAME\" Description=\"\"/><BlockFieldItem2 DataField=\"DESCRIPTION\" Description=\"\"/></BlockFieldItems></Block1><Block2 ProviderName=\"GROUPMENUS\" TableName=\"GROUPMENUS\" Name=\"GROUPMENUS\" ParentItemName=\"Master\"><BlockFieldItems><BlockFieldItem0 DataField=\"GROUPID\" Description=\"\"/><BlockFieldItem1 DataField=\"MENUID\" Description=\"\"/></BlockFieldItems></Block2></Blocks></ClientData>" };
            //        FEEPWizard.GenServerModule(Params);
            //    }
            //    finally
            //    {
            //        file.Dispose();
            //    }
            //}

            /*
            //GetPageInfo
            Type aInterface = FEEPWizard.GetType().GetInterface("IEEPWizard");
            if (aInterface != null)
            {
                Object Params = new Object[] { 
                   @"C:\Program Files\InfoLight\EEP2006\Solution1.sln",
                   @"C:\...\EEPWebClient\", 
                   @"C:\Program Files\InfoLight\EEP2006\EEPWebClient\",
                    new Object[] { "Ap" },
                    "Form1.aspx",
                    "001",
                    "",
                    "ERPS",
                    "Solution1",
                    "0"
                };
                FEEPWizard.GetPageInfo(Params);
                //IEEPWizard A = (IEEPWizard)aInterface;
                //A.CallMethod("GetFormImage", null);
            }
             */


            /*
            String SolutionFileName = RealParams[0].ToString();
            String WebSiteName = RealParams[1].ToString();
            String WebSitePath = RealParams[2].ToString();
            Object[] FolderOffset = (Object[])RealParams[3];
            String PageName = RealParams[4].ToString();
            String UserID = RealParams[5].ToString();
            String Password = RealParams[6].ToString();
            String DBName = RealParams[7].ToString();
            String Solutionname = RealParams[8].ToString();
            String PrintWaitingTime = RealParams[9].ToString();
            */

            /*
            //GenServerModule
            Type bInterface = FEEPWizard.GetType().GetInterface("IEEPWizard");
            if (bInterface != null)
            {
                Object Params = new Object[] { "<?xml version=\"1.0\" encoding=\"UTF-8\"?><ServerData DatabaseName=\"ERPS\" PackageName=\"s1\" SolutionName=\"C:\\Program Files\\InfoLight\\EEP2006\\Solution1.sln\" OutputPath=\"C:\\Program Files\\InfoLight\\EEP2006\\\" CodeOutputPath=\"C:\\Program Files\\InfoLight\\EEP2006\\\" NewSolution=\"0\"><Datasets><Dataset0 DatabaseName=\"ERPS\" TableName=\"Customers\" Name=\"Customers\" RelFields=\"\" KeyFields=\"CustomerID\" ParentItem=\"\"><FieldAttrItems><FieldAttrItem0 DataField=\"CustomerID\" Description=\"め絪腹\" IsKey=\"1\" CheckNull=\"0\" IsRelationKey=\"0\" ParentRelationField=\"\"/><FieldAttrItem1 DataField=\"CompanyName\" Description=\"CompanyName\" IsKey=\"0\" CheckNull=\"0\" IsRelationKey=\"0\" ParentRelationField=\"\"/><FieldAttrItem2 DataField=\"ContactName\" Description=\"ContactName\" IsKey=\"0\" CheckNull=\"0\" IsRelationKey=\"0\" ParentRelationField=\"\"/></FieldAttrItems></Dataset0></Datasets></ServerData>" };
                FEEPWizard.GenServerModule(Params);
                //IEEPWizard A = (IEEPWizard)aInterface;
                //A.CallMethod("GetFormImage", null);
            }
             */

            //GenClientModule
            bInterface = FEEPWizard.GetType().GetInterface("IEEPWizard");
            if (bInterface != null)
            {
                System.IO.FileStream file = new System.IO.FileStream("d:\\a.xml", System.IO.FileMode.Open);
                try
                {
                    XmlDocument X = new XmlDocument();
                    X.Load(file);
                    String S = X.OuterXml;
                    Object Params = new Object[] { S }; //{ "<?xml version=\"1.0\" encoding=\"UTF-8\"?><ClientData PackageName=\"c9\" BaseFormName=\"CMasterDetail\" ServerPackageName=\"S002.GROUPS\" OutputPath=\"C:\\Program Files\\InfoLight\\EEP2006\\\" CodeOutputPath=\"C:\\Program Files\\InfoLight\\EEP2006\\Solution1\" TableName=\"GROUPS\" FormName=\"fmc9\" ProviderName=\"S002.GROUPS\" DatabaseName=\"ERPS\" SolutionName=\"C:\\Program Files\\InfoLight\\EEP2006\\Solution1.sln\" NewSolution=\"0\" ColumnCount=\"1\" ViewProviderName=\"S002.View_Provider\"><Blocks><Block0 ProviderName=\"GROUPS\" TableName=\"GROUPS\" Name=\"View\" ParentItemName=\"\"><BlockFieldItems><BlockFieldItem0 DataField=\"GROUPID\" Description=\"\"/><BlockFieldItem1 DataField=\"GROUPNAME\" Description=\"\"/><BlockFieldItem2 DataField=\"DESCRIPTION\" Description=\"\"/></BlockFieldItems></Block0><Block1 ProviderName=\"GROUPS\" TableName=\"GROUPS\" Name=\"Master\" ParentItemName=\"\"><BlockFieldItems><BlockFieldItem0 DataField=\"GROUPID\" Description=\"\"/><BlockFieldItem1 DataField=\"GROUPNAME\" Description=\"\"/><BlockFieldItem2 DataField=\"DESCRIPTION\" Description=\"\"/></BlockFieldItems></Block1><Block2 ProviderName=\"GROUPMENUS\" TableName=\"GROUPMENUS\" Name=\"GROUPMENUS\" ParentItemName=\"Master\"><BlockFieldItems><BlockFieldItem0 DataField=\"GROUPID\" Description=\"\"/><BlockFieldItem1 DataField=\"MENUID\" Description=\"\"/></BlockFieldItems></Block2></Blocks></ClientData>" };
                    FEEPWizard.GenWebForm(Params);
                }
                finally
                {
                    file.Dispose();
                }
            }
        }

        private void cbDatabaseType_SelectedIndexChanged(object sender, EventArgs e)
        {
            FServerData.DatabaseType = (ClientType)cbDatabaseType.SelectedIndex;
        }

        private void RemoveChildItem(TreeNode aNode)
        {
            while (aNode.Nodes.Count > 0)
            {
                RemoveChildItem(aNode.Nodes[0]);
            }

            if (aNode.Tag != null)
            {
                TDatasetItem bItem = (TDatasetItem)aNode.Tag;
                FServerData.Datasets.Remove(bItem);
            }
            aNode.Nodes.Remove(aNode);
        }

        private void btnDeleteDataset_Click(object sender, EventArgs e)
        {
            if (tvTables.SelectedNode != null)
            {
                RemoveChildItem(tvTables.SelectedNode);
                //if (tvTables.SelectedNode.Tag != null)
                //{
                //    TDatasetItem aItem = (TDatasetItem)tvTables.SelectedNode.Tag;
                //    aItem.ChildItem.Clear();
                //    FServerData.Datasets.Remove(aItem);
                //}
                //tvTables.Nodes.Remove(tvTables.SelectedNode);
            }
        }

        private void btnRelation_Click(object sender, EventArgs e)
        {
            TreeNode Node = tvTables.SelectedNode;
            if (Node == null)
                return;
            TDatasetItem DetailItem = (TDatasetItem)Node.Tag;
            fmSelKeyField aForm = new fmSelKeyField();
            aForm.ShowSelKeyField(DetailItem.ParentItem, DetailItem);
        }

        private void lvSelectedFields_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // 检查点击的列是不是现在的排序列.
            if (e.Column == lvwColumnSorter.SortColumn)
            {
                // 重新设置此列的排序方法.
                if (lvwColumnSorter.OrderOfSort == System.Windows.Forms.SortOrder.Ascending)
                {
                    lvwColumnSorter.OrderOfSort = System.Windows.Forms.SortOrder.Descending;
                }
                else
                {
                    lvwColumnSorter.OrderOfSort = System.Windows.Forms.SortOrder.Ascending;
                }
            }
            else
            {
                // 设置排序列，默认为正向排序
                lvwColumnSorter.SortColumn = e.Column;
                lvwColumnSorter.OrderOfSort = System.Windows.Forms.SortOrder.Ascending;
            }

            // 用新的排序方法对ListView排序
            (sender as ListView).Sort();
        }
    }

    public class ListViewColumnSorter : IComparer
    {
        /// <summary>
        /// 指定按照哪个列排序
        /// </summary>
        private int ColumnToSort;

        /// <summary>
        /// 指定排序的方式
        /// </summary>
        public System.Windows.Forms.SortOrder OrderOfSort;

        /// <summary>
        /// 声明CaseInsensitiveComparer类对象，
        /// 参见ms-help://MS.VSCC.2003/MS.MSDNQTR.2003FEB.2052/cpref/html/frlrfSystemCollectionsCaseInsensitiveComparerClassTopic.htm
        /// </summary>
        private CaseInsensitiveComparer ObjectCompare;

        public int SortColumn
        {
            set
            {
                ColumnToSort = value;
            }
            get
            {
                return ColumnToSort;
            }
        }

        public ListViewColumnSorter()
        {
            // 默认按第一列排序
            ColumnToSort = 0;

            // 排序方式为不排序
            OrderOfSort = System.Windows.Forms.SortOrder.None;

            // 初始化CaseInsensitiveComparer类对象
            ObjectCompare = new CaseInsensitiveComparer();
        }

        public int Compare(object x, object y)
        {
            int compareResult;
            ListViewItem listviewX, listviewY;

            // 将比较对象转换为ListViewItem对象
            listviewX = (ListViewItem)x;
            listviewY = (ListViewItem)y;

            // 比较
            compareResult = ObjectCompare.Compare(listviewX.SubItems[ColumnToSort].Text, listviewY.SubItems[ColumnToSort].Text);

            // 根据上面的比较结果返回正确的比较结果
            if (OrderOfSort == System.Windows.Forms.SortOrder.Ascending)
            {
                // 因为是正序排序，所以直接返回结果
                return compareResult;
            }
            else if (OrderOfSort == System.Windows.Forms.SortOrder.Descending)
            {
                // 如果是反序排序，所以要取负值再返回
                return (-compareResult);
            }
            else
            {
                // 如果相等返回0
                return 0;
            }
        }
    }

    public class TFieldAttrItem : TCollectionItem
    {
        string FDataField, FDescription, FParentRelationField;
        bool FIsKey, FCheckNull, FIsRelationKey;

        public string DataField
        {
            get
            {
                return FDataField;
            }
            set
            {
                FDataField = value;
            }
        }

        public string Description
        {
            get
            {
                return FDescription;
            }
            set
            {
                FDescription = value;
            }
        }

        public bool CheckNull
        {
            get
            {
                return FCheckNull;
            }
            set
            {
                FCheckNull = value;
            }
        }

        public bool IsKey
        {
            get
            {
                return FIsKey;
            }
            set
            {
                FIsKey = value;
            }
        }

        public bool IsRelationKey
        {
            get
            {
                return FIsRelationKey;
            }
            set
            {
                FIsRelationKey = value;
            }
        }

        public string ParentRelationField
        {
            get
            {
                return FParentRelationField;
            }
            set
            {
                FParentRelationField = value;
            }
        }
    }

    public class TFieldAttrItems : TCollection
    {
        public TFieldAttrItems(object Owner)
        {
            base.Owner = Owner;
        }

        public TFieldAttrItem FindItem(string FieldName)
        {
            foreach (TFieldAttrItem aItem in InnerList)
            {
                if (string.Compare(aItem.DataField, FieldName) == 0)
                    return aItem;
            }
            return null;
        }

        public string KeyFields
        {
            get
            {
                string Result = "";
                TFieldAttrItem FAI;
                int I;
                for (I = 0; I < Count; I++)
                {
                    FAI = (TFieldAttrItem)this[I];
                    if (FAI.IsKey)
                        Result = Result + FAI.DataField + ";";
                }
                if (Result != "")
                    Result = Result.Remove(Result.Length - 1);
                return Result;
            }
        }
    }

    public class TDatasetItem : TCollectionItem
    {
        private string FName, FTableName, FDatabaseName;
        private TFieldAttrItems FFieldAttrItems;
        private TDatasetItem FParentItem, FChildItem;
        private TStringList FFieldList = null;
        private TStringList FFieldCaptionList = null;
        private InfoDataSource FInfoDataSource;
        private InfoCommand FInfoCommand;
        private bool FAddAll;
        private ClientType FDatabaseType;

        public TDatasetItem()
        {
            FFieldAttrItems = new TFieldAttrItems(this);
        }

        public string Name
        {
            get
            {
                return FName;
            }
            set
            {
                FName = value;
            }
        }

        public string KeyFields
        {
            get
            {
                string Result = "";
                int I;
                TFieldAttrItem FAI;
                for (I = 0; I < FieldAttrItems.Count; I++)
                {
                    FAI = FieldAttrItems[I] as TFieldAttrItem;
                    if (FAI.IsKey)
                        Result = Result + FAI.DataField + ";";
                }
                if (Result != "")
                    Result = Result.Remove(Result.Length - 1);
                return Result;
            }
        }

        public ClientType DatabaseType
        {
            get { return FDatabaseType; }
            set { FDatabaseType = value; }
        }

        public string TableName
        {
            get
            {
                return FTableName;
            }
            set
            {
                FTableName = value;
            }
        }

        public InfoDataSource DataSource
        {
            get
            {
                return FInfoDataSource;
            }
            set
            {
                FInfoDataSource = value;
            }
        }

        public InfoCommand Command
        {
            get
            {
                return FInfoCommand;
            }
            set
            {
                FInfoCommand = value;
            }
        }

        public string DatabaseName
        {
            get
            {
                return FDatabaseName;
            }
            set
            {
                FDatabaseName = value;
            }
        }

        public bool AddAll
        {
            get { return FAddAll; }
            set { FAddAll = value; }
        }

        public TFieldAttrItems FieldAttrItems
        {
            get
            {
                return FFieldAttrItems;
            }
            set
            {
                FFieldAttrItems = value;
            }
        }

        public TDatasetItem ParentItem
        {
            get
            {
                return FParentItem;
            }
            set
            {
                FParentItem = value;
            }
        }

        public TDatasetItem ChildItem
        {
            get
            {
                return FChildItem;
            }
            set
            {
                FChildItem = value;
            }
        }

        public TStringList FieldList
        {
            get
            {
                if (FFieldList == null)
                {
                    FFieldList = new TStringList();
                }
                if (FFieldList.Count == 0)
                {
                    String OldTableName = TableName;
                    String Owner = String.Empty;
                    String SS = TableName;
                    if (TableName.IndexOf('.') > -1)
                    {
                        Owner = WzdUtils.GetToken(ref SS, new char[] { '.' });
                        TableName = SS;
                    }

                    string[] S = new string[4];
                    //S[1] = Owner;
                    S[2] = TableName;

                    String SortName = "ORDINAL_POSITION";
                    TDatasetCollection dc = (TDatasetCollection)Collection;
                    TServerData sd = (TServerData)dc.Owner;
                    if (sd.DatabaseType != ClientType.ctInformix)
                    {
                        if (sd.DatabaseType == ClientType.ctOracle)
                        {
                            String UserID = WzdUtils.GetFieldParam(sd.ConnectionString.ToLower(), "user id") == "" ? WzdUtils.GetFieldParam(sd.ConnectionString.ToLower(), "uid") : WzdUtils.GetFieldParam(sd.ConnectionString.ToLower(), "user id");
                            if (Owner != null && Owner != "") UserID = Owner;
                            S = new String[] { UserID, TableName };
                            SortName = "ID";
                        }
                        DataTable D = null;
                        D = sd.Owner.GlobalConnection.GetSchema("Columns", S);
                        if (D.Rows.Count == 0 && sd.DatabaseType == ClientType.ctOracle)
                        {
                            S = new String[] { Owner, TableName };
                            D = sd.Owner.GlobalConnection.GetSchema("Columns", S);
                        }
                        DataRow[] DRs = D.Select("", SortName + " ASC");

                        foreach (DataRow DR in DRs)
                            FFieldList.Add(DR["COLUMN_NAME"].ToString());

                        TableName = OldTableName;
                    }
                    else
                    {
                        String sQL = "select * from " + TableName + " where 1=0";
                        IDbCommand cmd = sd.Owner.GlobalConnection.CreateCommand();
                        cmd.CommandText = sQL;
                        if (sd.Owner.GlobalConnection.State == ConnectionState.Closed)
                        { sd.Owner.GlobalConnection.Open(); }

                        IDataReader reader = cmd.ExecuteReader(CommandBehavior.SchemaOnly);
                        DataTable schemaTable = reader.GetSchemaTable();

                        //DataSet schemaTable = new DataSet();
                        //IDbDataAdapter ida = WzdUtils.AllocateDataAdapter(sd.DatabaseType);
                        //ida.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                        //ida.Fill(schemaTable);

                        foreach (DataRow DR in schemaTable.Rows)
                            FFieldList.Add(DR["COLUMNNAME"].ToString());
                    }
                }
                return FFieldList;
            }
        }

        public TStringList FieldCaptionList
        {
            get
            {
                if (FFieldCaptionList == null)
                {
                    FFieldCaptionList = new TStringList();
                }
                if (FFieldCaptionList.Count == 0)
                {
                    TDatasetCollection dc = (TDatasetCollection)Collection;
                    TServerData sd = (TServerData)dc.Owner;
                    InfoCommand aInfoCommand = new InfoCommand(DatabaseType);
                    aInfoCommand.Connection = sd.Owner.GlobalConnection;
                    TableName = WzdUtils.RemoveQuote(TableName, DatabaseType);
                    String Owner = String.Empty, SS = TableName;
                    String oldTableName = TableName;
                    if (TableName.IndexOf('.') > -1)
                    {
                        Owner = WzdUtils.GetToken(ref SS, new char[] { '.' });
                        TableName = SS;
                    }
                    aInfoCommand.CommandText = "Select FIELD_NAME,CAPTION from COLDEF where TABLE_NAME='" + TableName + "' OR TABLE_NAME='" + Owner + "." + TableName + "'";
                    IDbDataAdapter DA = DBUtils.CreateDbDataAdapter(aInfoCommand);
                    DataSet D = new DataSet();
                    WzdUtils.FillDataAdapter(DatabaseType, DA, D, "COLDEF");
                    TableName = oldTableName;

                    FFieldCaptionList.Clear();
                    int I;
                    DataRow DR;
                    for (I = 0; I < D.Tables[0].Rows.Count; I++)
                    {
                        DR = D.Tables[0].Rows[I];
                        if (DR["FIELD_NAME"].ToString() != "")
                            FFieldCaptionList.Add(DR["FIELD_NAME"] + "=" + DR["CAPTION"]);
                    }
                }
                return FFieldCaptionList;
            }
        }
    }

    public class TDatasetCollection : TCollection
    {
        public TDatasetCollection(object Owner)
        {
            base.Owner = Owner;
        }

        public TDatasetItem FindItem(string AName)
        {
            foreach (TDatasetItem aItem in InnerList)
            {
                if (String.Compare(aItem.Name, AName) == 0)
                    return aItem;
            }
            return null;
        }
    }

    public class TServerData : Object
    {
        private string FDatabaseName, FPackageName, FOutputPath, FSolutionName, FAssemblyOutputPath, FEEPAlias;
        private TDatasetCollection FDatasetCollection;
        private TStringList FTableNameList = null;
        private TStringList FTableNameCaptionList = null;
        private MWizard2015.fmServerWzd FOwner;
        private bool FNewSolution = false;
        private string FConnectionString;
        private string FCodeOutputPath;
        private ClientType FDatabaseType;
        private String FLanguage = "cs";

        public TServerData(MWizard2015.fmServerWzd Owner)
        {
            FDatasetCollection = new TDatasetCollection(this);
            FOwner = Owner;
        }

        //public TServerData(fmWCFServerWzd Owner)
        //{
        //    // TODO: Complete member initialization
        //    FDatasetCollection = new TDatasetCollection(this);
        //}

        public void ResetDatabaseConnection()
        {
            TableNameList.Clear();
            TableNameCaptionList.Clear();
        }

        public String EEPAlias
        {
            get { return FEEPAlias; }
            set { FEEPAlias = value; }
        }

        public String Language
        {
            get { return FLanguage; }
            set { FLanguage = value; }
        }

        private Boolean IsKeyField(String KeyFields, String aField)
        {
            String Temp = KeyFields;
            String S = WzdUtils.GetToken(ref Temp, new char[] { ';' });
            while (S != "")
            {
                if (String.Compare(S, aField) == 0)
                    return true;
                S = WzdUtils.GetToken(ref Temp, new char[] { ';' });
            }
            return false;
        }

        private void LoadFieldAttrs(XmlNode Node, TFieldAttrItems AttrItems, String KeyFields)
        {
            TFieldAttrItem FAI;
            int I;
            XmlNode AttrNode;
            for (I = 0; I < Node.ChildNodes.Count; I++)
            {
                AttrNode = Node.ChildNodes[I];
                FAI = new TFieldAttrItem();
                FAI.DataField = AttrNode.Attributes["DataField"].Value;
                FAI.Description = AttrNode.Attributes["Description"].Value;
                FAI.IsKey = AttrNode.Attributes["IsKey"].Value == "1";
                FAI.CheckNull = AttrNode.Attributes["CheckNull"].Value == "1";
                FAI.IsRelationKey = AttrNode.Attributes["IsRelationKey"].Value == "1"; //IsKeyField(KeyFields, FAI.DataField);
                FAI.ParentRelationField = AttrNode.Attributes["ParentRelationField"].Value;
                AttrItems.Add(FAI);
            }
        }

        private void LoadDatasets(XmlNode Node)
        {
            int I;
            TDatasetItem DI;
            XmlNode DatasetNode, FieldAttrsNode;
            for (I = 0; I < Node.ChildNodes.Count; I++)
            {
                DatasetNode = Node.ChildNodes[I];
                DI = new TDatasetItem();
                DI.Name = DatasetNode.Attributes["Name"].Value;
                DI.DatabaseType = FDatabaseType;
                DI.DatabaseName = DatasetNode.Attributes["DatabaseName"].Value;
                DI.TableName = DatasetNode.Attributes["TableName"].Value;
                FieldAttrsNode = WzdUtils.FindNode(null, DatasetNode, "FieldAttrItems");
                LoadFieldAttrs(FieldAttrsNode, DI.FieldAttrItems, DatasetNode.Attributes["RelFields"].Value);
                Datasets.Add(DI);
            }

            for (I = 0; I < Node.ChildNodes.Count; I++)
            {
                DatasetNode = Node.ChildNodes[I];
                if (DatasetNode.Attributes["ParentItem"].Value != "")
                {
                    TDatasetItem ChildItem = Datasets.FindItem(DatasetNode.Attributes["Name"].Value);
                    TDatasetItem ParentItem = Datasets.FindItem(DatasetNode.Attributes["ParentItem"].Value);
                    ChildItem.ParentItem = ParentItem;
                }
            }
        }

        public ClientType DatabaseType
        {
            get { return FDatabaseType; }
            set { FDatabaseType = value; }
        }

        private String GetConnectionString(String DatabaseName)
        {
            String ServerPath = EEPRegistry.Server + "\\";
            string text3 = SystemFile.DBFile;
            XmlDocument document1 = new XmlDocument();
            document1.Load(text3);
            foreach (XmlNode node1 in document1.FirstChild.FirstChild.ChildNodes)
            {
                String aName = node1.Name;
                if (String.Compare(aName, DatabaseName) == 0)
                {
                    DatabaseType = (ClientType)int.Parse(node1.Attributes["Type"].Value);
                    string text1 = node1.Attributes["String"].Value.Trim();
                    string text2 = WzdUtils.GetPwdString(node1.Attributes["Password"].Value.Trim());
                    if ((text1.Length > 0) && (text2.Length > 0) && text2 != String.Empty)
                    {
                        if (text1[text1.Length - 1] != ';')
                        {
                            text1 = text1 + ";Password=" + text2;
                        }
                        else
                        {
                            text1 = text1 + "Password=" + text2;
                        }
                    }
                    return text1;
                }
            }
            return "";
        }

        public object LoadFromXML(string XML)
        {
            System.Xml.XmlNode Node = null;
            System.Xml.XmlDocument Doc = new System.Xml.XmlDocument();
            Doc.LoadXml(XML);
            Node = Doc.SelectSingleNode("ServerData");
            EEPAlias = Node.Attributes["DatabaseName"].Value;
            ConnectionString = GetConnectionString(EEPAlias);
            SolutionName = Node.Attributes["SolutionName"].Value;
            OutputPath = Node.Attributes["OutputPath"].Value;
            CodeOutputPath = Node.Attributes["CodeOutputPath"].Value;
            NewSolution = Node.Attributes["NewSolution"].Value == "1";
            PackageName = Node.Attributes["PackageName"].Value;
            if (Node.Attributes["Language"].Value.ToString().CompareTo("C#") == 0)
                this.Language = "cs";
            else
                this.Language = "vb";
            Node = WzdUtils.FindNode(Doc, Node, "Datasets");
            LoadDatasets(Node);
            return null;
        }

        public MWizard2015.fmServerWzd Owner
        {
            get
            {
                return FOwner;
            }
        }

        public bool NewSolution
        {
            get
            {
                return FNewSolution;
            }
            set
            {
                FNewSolution = value;
            }
        }

        public string SolutionName
        {
            get
            {
                return FSolutionName;
            }
            set
            {
                FSolutionName = value;
            }
        }

        public string ConnectionString
        {
            get
            {
                return FConnectionString;
            }
            set
            {
                FConnectionString = value;
            }
        }

        public string DatabaseName
        {
            get
            {
                return FDatabaseName;
            }
            set
            {
                FDatabaseName = value;
            }
        }

        public string PackageName
        {
            get
            {
                return FPackageName;
            }
            set
            {
                FPackageName = value;
            }
        }

        public string OutputPath
        {
            get
            {
                return FOutputPath;
            }
            set
            {
                FOutputPath = value;
            }
        }

        public String AssemblyOutputPath
        {
            get { return FAssemblyOutputPath; }
            set { FAssemblyOutputPath = value; }
        }

        public string CodeOutputPath
        {
            get
            {
                return FCodeOutputPath;
            }
            set
            {
                string S = value;
                if (S != "")
                    if (S[S.Length - 1] == '\\')
                        S = S.Substring(0, S.Length - 1);
                FCodeOutputPath = S;
            }
        }

        public TDatasetCollection Datasets
        {
            get
            {
                return FDatasetCollection;
            }
            set
            {
                FDatasetCollection = value;
            }
        }

        public TStringList TableNameList
        {
            get
            {
                if (FTableNameList == null)
                {
                    FTableNameList = new TStringList();
                }
                if (FTableNameList.Count == 0)
                {
                    if (FOwner != null)
                    {
                        if (FOwner.ServerData.DatabaseType != ClientType.ctInformix)
                        {
                            String[] Params = null;
                            if (FOwner.GlobalConnection.ConnectionString != null && FOwner.ServerData.DatabaseType == ClientType.ctOracle)
                            {
                                String UserID = WzdUtils.GetFieldParam(FOwner.GlobalConnection.ConnectionString.ToLower(), "user id");
                                Params = new String[] { UserID.ToUpper() };
                            }
                            DataTable T = FOwner.GlobalConnection.GetSchema("Tables", Params);
                            DataRow[] dr = T.Select("", "TABLE_NAME ASC");
                            bool flag = false;
                            foreach (DataColumn DC in T.Columns)
                            {
                                if (DC.Caption.ToLower() == "owner")
                                {
                                    flag = true;
                                    break;
                                }
                            }
                            foreach (DataRow DR in dr)
                            {
                                String S = "";
                                if (flag && !String.IsNullOrEmpty(DR["OWNER"].ToString()))
                                    S = DR["OWNER"].ToString() + '.';
                                if (FOwner.ServerData.DatabaseType == ClientType.ctMsSql)
                                    S = DR["TABLE_SCHEMA"].ToString() + '.';
                                FTableNameList.Add(S + DR["TABLE_NAME"].ToString());
                            }

                            T = FOwner.GlobalConnection.GetSchema("Views", Params);
                            if (T.Rows.Count > 0)
                            {
                                if (FOwner.ServerData.DatabaseType != ClientType.ctOracle)
                                    dr = T.Select("", "TABLE_NAME ASC");
                                else
                                    dr = T.Select("", "VIEW_NAME ASC");
                                flag = false;
                                foreach (DataColumn DC in T.Columns)
                                {
                                    if (DC.Caption.ToLower() == "owner")
                                    {
                                        flag = true;
                                        break;
                                    }
                                }
                                foreach (DataRow DR in dr)
                                {
                                    String S = "";
                                    if (flag && !String.IsNullOrEmpty(DR["OWNER"].ToString()))
                                        S = DR["OWNER"].ToString() + '.';
                                    if (FOwner.ServerData.DatabaseType == ClientType.ctMsSql)
                                        S = DR["TABLE_SCHEMA"].ToString() + '.';
                                    if (FOwner.ServerData.DatabaseType != ClientType.ctOracle)
                                        FTableNameList.Add(DR["TABLE_NAME"].ToString());
                                    else
                                        FTableNameList.Add(S + DR["VIEW_NAME"].ToString());

                                }
                            }
                        }
                        else
                        {
                            List<String> allTables = WzdUtils.GetAllTablesList(FOwner.GlobalConnection, ClientType.ctInformix);
                            allTables.Sort();
                            foreach (String str in allTables)
                                FTableNameList.Add(str);
                        }
                    }
                }

                return FTableNameList;
            }
        }

        public TStringList TableNameCaptionList
        {
            get
            {
                if (FTableNameCaptionList == null)
                {
                    FTableNameCaptionList = new TStringList();
                }
                if (FTableNameCaptionList.Count == 0)
                {
                    int I;
                    DataRow DR;
                    InfoCommand aInfoCommand = new InfoCommand(DatabaseType);
                    aInfoCommand.Connection = FOwner.GlobalConnection;

                    aInfoCommand.CommandText = "Select TABLE_NAME, CAPTION from COLDEF where FIELD_NAME = '*' or FIELD_NAME is null order by TABLE_NAME";
                    IDbDataAdapter DA = DBUtils.CreateDbDataAdapter(aInfoCommand);
                    DataSet D = new DataSet();
                    WzdUtils.FillDataAdapter(DatabaseType, DA, D, "COLDEF");


                    FTableNameCaptionList.Clear();
                    for (I = 0; I < D.Tables[0].Rows.Count; I++)
                    {
                        DR = D.Tables[0].Rows[I];
                        if (DR["TABLE_NAME"].ToString().Trim() != "" && DR["CAPTION"].ToString().Trim() != "")
                            FTableNameCaptionList.Add(DR["TABLE_NAME"].ToString().Trim() + "=" + DR["CAPTION"].ToString().Trim());
                    }
                }
                return FTableNameCaptionList;
            }
        }
    }

    public class TStringList : ArrayList
    {
        private ArrayList FObjects = null;

        public TStringList()
        {
            FObjects = new ArrayList();
        }

        public string Values(string ParamName)
        {
            string Result = "", TempName, S;
            int I, J;
            for (I = 0; I < Count; I++) //???
            {
                S = this[I].ToString();
                for (J = 0; J < S.Length; J++)
                {
                    if (S[J].ToString() == "=")
                    {
                        TempName = S.Remove(J);
                        if (string.Compare(ParamName, TempName) == 0)
                        {
                            if (J < S.Length - 1)
                            {
                                Result = S.Remove(0, J + 1);
                            }
                            break;
                        }
                    }
                }
            }
            return Result;
        }

        public object Objects(int Index)
        {
            if (Index > Count || Index < 0)
                throw new Exception("Index out of bounds");
            return FObjects[Index];
        }

        public int AddObject(string Value, object Obj)
        {
            int Result;
            Result = Add(Value);
            FObjects.Add(Obj);
            return Result;
        }
    }

    public class TServerGenerator : Object
    {
        private TServerData FServerData;
        private DTE2 FDTE2;
        private Component FDataModule;
        private System.ComponentModel.Design.IDesignerHost FDesignerHost;
        private AddIn FAddIn;
        private string FTemplateName;
        private InfoConnection FConnection;
        private Project GlobalProject;
        private ProjectItem GlobalPI;
        private Window GlobalWindow;

        public TServerGenerator(TServerData ServerData, DTE2 aDTE, AddIn aAddIn)
        {
            FServerData = ServerData;
            FDTE2 = aDTE;
            FAddIn = aAddIn;
            switch (FServerData.Language)
            {
                case "cs":
                    FTemplateName = WzdUtils.GetAddinsPath() + "\\Templates\\ServerPackage\\ServerPackage.csproj";
                    break;
                case "vb":
                    FTemplateName = WzdUtils.GetAddinsPath() + "\\Templates\\VBServerPackage\\VBServerPackage.vbproj";
                    break;
                default:
                    FTemplateName = WzdUtils.GetAddinsPath() + "\\Templates\\ServerPackage\\ServerPackage.csproj";
                    break;
            }
        }

        public void GenServerModule()
        {
            if (GenSolution())
            {
                GetDataModule();
                DesignerTransaction T = FDesignerHost.CreateTransaction();
                try
                {
                    try
                    {
                        GenConnection();
                        GenDatasets();
                        GenViewDataSet();

                    }
                    catch (Exception E)
                    {
                        MessageBox.Show(E.Message);
                    }
                }
                finally
                {
                    T.Commit();
                }
                //GlobalPI.Save(GlobalPI.get_FileNames(0));
                GlobalWindow.Close(vsSaveChanges.vsSaveChangesYes);
                Window W = GlobalPI.Open("{00000000-0000-0000-0000-000000000000}");
                W.Activate();
                GlobalProject.Save(GlobalProject.FullName);
                FDTE2.Solution.SolutionBuild.BuildProject(FDTE2.Solution.SolutionBuild.ActiveConfiguration.Name,
                    GlobalProject.FullName, true);
            }
        }

        private bool GenSolution()
        {
            Solution sln = /*(Solution2)*/FDTE2.Solution;

            if (FServerData.NewSolution)
            {
                if (System.IO.Directory.Exists(FServerData.OutputPath))
                {
                    if (FServerData.OutputPath == "\\")
                        throw new Exception("Unknown Output Path: " + "\\");
                    System.IO.Directory.Delete(FServerData.OutputPath, true);
                }
                sln.Create(FServerData.OutputPath, FServerData.SolutionName);
                ProjectLoader.AddDefaultProject(FDTE2);
                Project P = sln.AddFromTemplate(FTemplateName,
                    FServerData.CodeOutputPath + "\\" + FServerData.PackageName, FServerData.PackageName, false);
                P.Name = FServerData.PackageName;
                string FileName = FServerData.OutputPath + "\\" + FServerData.SolutionName + ".sln";
                sln.SaveAs(FileName);
                //sln.Open(FileName);
                sln.SolutionBuild.StartupProjects = P;
                sln.SolutionBuild.BuildProject(sln.SolutionBuild.ActiveConfiguration.Name, P.FullName, true);
                GlobalProject = P;
                ////sln.SolutionBuild.Clean(true);
            }
            else
            {
                string FilePath = FServerData.CodeOutputPath + "\\" + FServerData.PackageName;
                //string FilePath = Path.GetDirectoryName(FServerData.SolutionName) + "\\" + FServerData.PackageName;
                if (System.IO.Directory.Exists(FilePath))
                {
                    if (FilePath == "\\")
                        throw new Exception("Unknown Output Path: " + "\\");

                    DialogResult dr = MessageBox.Show("There is another File which name is " + FServerData.PackageName + " existed! Do you want to delete it first", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dr == DialogResult.Yes)
                    {
                        System.IO.Directory.Delete(FilePath, true);
                    }
                    else
                    {
                        return false;
                    }

                }
                Project P = sln.AddFromTemplate(FTemplateName, FilePath, FServerData.PackageName, true);
                P.Name = FServerData.PackageName;
                string FileName = FilePath + "\\" + FServerData.PackageName + "." + FServerData.Language + "proj";
                P.Save(FileName);
                sln.Open(FServerData.SolutionName);
                int I;
                P = null;
                for (I = 1; I <= sln.Projects.Count; I++)
                {
                    P = sln.Projects.Item(I);
                    if (string.Compare(P.Name, FServerData.PackageName, true) == 0) //大小写重命问题
                        break;
                    else
                        P = null;
                }
                if (P != null)
                    sln.Remove(P);
                P = sln.AddFromFile(FilePath + "\\" + FServerData.PackageName + "." + FServerData.Language + "proj", false);
                P.Properties.Item("RootNamespace").Value = FServerData.PackageName;
                P.Properties.Item("AssemblyName").Value = FServerData.PackageName;
                sln.SaveAs(FServerData.SolutionName);
                sln.SolutionBuild.StartupProjects = P;
                sln.SolutionBuild.BuildProject(sln.SolutionBuild.ActiveConfiguration.Name, P.FullName, true);
                GlobalProject = P;
                //sln.SolutionBuild.Clean(true);

                //try
                //{
                //    EnvDTE.DTE dte = System.Runtime.InteropServices.Marshal.GetActiveObject("VisualStudio.DTE.10.0") as DTE;
                //    EnvDTE80.DTE2 dte2 = dte as EnvDTE80.DTE2;// Activator.CreateInstance(Type.GetTypeFromProgID("VisualStudio.DTE.10.0")) as EnvDTE80.DTE2;

                //    string vsWizardAddItem = "{0F90E1D1-4999-11D1-B6D1-00A0C90F2744}";//WizardType Guid
                //    bool silent = false;

                //    int commonIndex = dte2.Application.FileName.IndexOf(@"\Common7");
                //    string vsInstallPath = dte2.Application.FileName.Substring(0, commonIndex);
                //    //object[] obj = dte.ActiveSolutionProjects as object[];
                //    Project project = P;
                //    //if (obj.Length > 0)
                //    //    project = obj[0] as Project;
                //    //Project project = (Project)(((object[])dte.ActiveSolutionProjects)[0]);

                //    string itemName = project.Name + ".edmx";
                //    string localDir = System.IO.Path.GetDirectoryName(project.FullName);

                //    object[] prams = {vsWizardAddItem,project.Name,project.ProjectItems,
                //             localDir, itemName,vsInstallPath, silent};

                //    Solution2 soln = (Solution2)dte2.Solution;
                //    string templatePath = soln.GetProjectItemTemplate("AdoNetEntityDataModelCSharp.zip", "CSharp");
                //    dte.LaunchWizard(templatePath, ref prams);
                //}
                //catch (Exception ex)
                //{
                //    throw ex;
                //}
            }
            if (FServerData.AssemblyOutputPath != null && FServerData.AssemblyOutputPath != "")
                GlobalProject.ConfigurationManager.ActiveConfiguration.Properties.Item("OutputPath").Value = FServerData.AssemblyOutputPath;

            return true;
        }

        private void RenameNameSpace(string FileName)
        {
            if (!File.Exists(FileName))
                return;
            System.IO.StreamReader SR = new System.IO.StreamReader(FileName);
            string Context = SR.ReadToEnd();
            SR.Close();
            Context = Context.Replace("TAG_NAMESPACE", FServerData.PackageName);
            System.IO.FileStream Filefs = new System.IO.FileStream(FileName, System.IO.FileMode.Create, System.IO.FileAccess.ReadWrite);
            System.IO.StreamWriter SW = new System.IO.StreamWriter(Filefs);
            SW.Write(Context);
            SW.Close();
            Filefs.Close();
        }

        private void GetDataModule()
        {
            Solution Sln = FDTE2.Solution;
            Project P = null;
            int I;
            for (I = 1; I <= Sln.Projects.Count; I++)
            {
                P = Sln.Projects.Item(I);
                if (string.Compare(FServerData.PackageName, P.Name) == 0)
                    break;
                P = null;
            }
            if (P == null)
                throw new Exception("Can not find project " + FServerData.PackageName + " in solution");
            ProjectItem PI;
            for (I = 1; I <= P.ProjectItems.Count; I++)
            {
                PI = P.ProjectItems.Item(I);
                if (string.Compare(PI.Name, "Component." + FServerData.Language) == 0)
                {
                    string Path = PI.get_FileNames(0);
                    Path = System.IO.Path.GetDirectoryName(Path);
                    RenameNameSpace(Path + "\\Component." + FServerData.Language);
                    Application.DoEvents();
                    FDTE2.MainWindow.Activate();
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(3000);
                    Application.DoEvents();
                    Window W = PI.Open("{00000000-0000-0000-0000-000000000000}");
                    W.Activate();
                    GlobalWindow = W;
                    GlobalPI = PI;
                    //return;//???
                    FDesignerHost = (IDesignerHost)W.Object;
                    FDataModule = (Component)FDesignerHost.RootComponent;
                    break;
                }
            }
        }

        private string CreateUniqueComponentName(Component aOwner, string Name)
        {
            int I, J = 1;
            Component C;
            Type T;
            System.Reflection.PropertyInfo P;
            string Result = Name;
            for (I = 0; I < aOwner.Container.Components.Count - 1; I++)
            {
                C = (Component)aOwner.Container.Components[I];
                T = C.GetType();
                P = T.GetProperty("Name");
                if (P != null)
                {
                    while (string.Compare((string)P.GetValue(C, null), Name) == 0)
                    {
                        Result = Name + J.ToString();
                    }
                }
            }
            return Result;
        }

        private Component CreateDataset(TDatasetItem DatasetItem, int Index)
        {
            string TempName = WzdUtils.RemoveSpecialCharacters(DatasetItem.TableName);
            string ComponentName = CreateUniqueComponentName(FDataModule, TempName);
            if (ComponentName.Contains("."))
                ComponentName = ComponentName.Remove(0, ComponentName.IndexOf('.') + 1);
            InfoCommand IC = FDesignerHost.CreateComponent(typeof(InfoCommand), ComponentName) as InfoCommand;
            IC.InfoConnection = FConnection;
            //IComponentChangeService componentChangeService = FDesignerHost.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
            //componentChangeService.OnComponentChanging(IC, TypeDescriptor.GetProperties(IC)["InfoConnection"]);
            //componentChangeService.OnComponentChanged(IC, TypeDescriptor.GetProperties(IC)["InfoConnection"], null, IC.InfoConnection);

            String sQL = "select * from " + WzdUtils.Quote(DatasetItem.TableName, FConnection.InternalDbConnection) + " where 1=0";
            IDbCommand cmd = FConnection.CreateCommand();
            cmd.CommandText = sQL;
            if (FConnection.State == ConnectionState.Closed)
            { FConnection.Open(); }
            DataSet schemaTable = new DataSet();
            IDbDataAdapter ida = WzdUtils.AllocateDataAdapter(DatasetItem.DatabaseType);
            ida.SelectCommand = cmd;
            ida.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            ida.Fill(schemaTable);

            if (schemaTable.Tables[0].Columns.Count == DatasetItem.FieldAttrItems.Count || DatasetItem.FieldAttrItems.Count == 0)
            {
                DatasetItem.AddAll = true;
            }

            String sTableName = WzdUtils.Quote(DatasetItem.TableName, IC.InfoConnection.InternalDbConnection);
            if (DatasetItem.AddAll)
            {
                IC.CommandText = String.Format("SELECT {0}.* FROM {0}", sTableName);
            }
            else
            {
                String SelectedFields = "";
                foreach (TFieldAttrItem FA in DatasetItem.FieldAttrItems)
                {
                    SelectedFields = SelectedFields + String.Format("{0}." + WzdUtils.Quote("{1}", IC.InfoConnection.InternalDbConnection) + ",", sTableName, FA.DataField);
                }
                if (SelectedFields.Length > 0)
                    SelectedFields = SelectedFields.Substring(0, SelectedFields.Length - 1);
                IC.CommandText = String.Format("SELECT {0} FROM {1}", SelectedFields, sTableName);
            }
            DatasetItem.Command = IC;
            int I;
            TFieldAttrItem FAI;
            IC.KeyFields.Clear();
            Srvtools.KeyItem KI;
            for (I = 0; I < DatasetItem.FieldAttrItems.Count; I++)
            {
                FAI = (TFieldAttrItem)DatasetItem.FieldAttrItems[I];
                if (FAI.IsKey)
                {
                    KI = new Srvtools.KeyItem();
                    KI.KeyName = FAI.DataField;
                    IC.KeyFields.Add(KI);
                }
            }
            return IC;
        }

        private void CreateUpdateComponent(Srvtools.InfoCommand IC, TDatasetItem DatasetItem, int Index)
        {
            string TempName = WzdUtils.RemoveSpecialCharacters(DatasetItem.TableName);
            string ComponentName = CreateUniqueComponentName(FDataModule, "uc" + TempName);
            if (ComponentName.Contains("."))
            {
                ComponentName = ComponentName.Remove(0, ComponentName.IndexOf('.') + 1);
                ComponentName = "uc" + ComponentName;
            }
            UpdateComponent UC = FDesignerHost.CreateComponent(typeof(UpdateComponent), ComponentName) as UpdateComponent;
            UC.SelectCmd = IC;
            UC.Site.Name = "AA";
            int I;

            TFieldAttrItem FAI;
            FieldAttr FA;
            for (I = 0; I < DatasetItem.FieldAttrItems.Count; I++)
            {
                FAI = (TFieldAttrItem)DatasetItem.FieldAttrItems[I];
                FA = new FieldAttr(FAI.DataField);
                FA.CheckNull = FAI.CheckNull;
                UC.FieldAttrs.Add(FA);
            }
            UC.Site.Name = ComponentName;
        }

        private void SetColumns(ColumnItems aColumnItems, TFieldAttrItems FieldAttrItems, Boolean ParentField)
        {
            aColumnItems.Clear();
            for (int num1 = 0; num1 < FieldAttrItems.Count; num1++)
            {
                TFieldAttrItem item2 = FieldAttrItems[num1] as TFieldAttrItem;
                if (item2.ParentRelationField != null && item2.ParentRelationField != "")
                {
                    ColumnItem item1 = new ColumnItem();
                    if (ParentField)
                    {
                        item1.FieldName = item2.ParentRelationField;
                        item1.Name = item2.ParentRelationField;
                    }
                    else
                    {
                        item1.FieldName = item2.DataField;
                        item1.Name = item2.DataField;
                    }
                    aColumnItems.Add(item1);
                }
            }
        }

        /*
        private void SetColumns(ColumnItems aColumnItems, TFieldAttrItems FieldAttrItems, Boolean ParentField)
        {
            aColumnItems.Clear();
            for (int num1 = 0; num1 < FieldAttrItems.Count; num1++)
            {
                TFieldAttrItem item2 = FieldAttrItems[num1] as TFieldAttrItem;
                if (item2.IsRelationKey)
                {
                    ColumnItem item1 = new ColumnItem();
                    item1.FieldName = item2.DataField;
                    item1.Name = item2.DataField;
                    aColumnItems.Add(item1);
                }
            }
        }

         */

        private void FixupMasterDetail()
        {
            for (int num1 = 0; num1 < FServerData.Datasets.Count; num1++)
            {
                TDatasetItem item1 = (TDatasetItem)FServerData.Datasets[num1];
                if (item1.DataSource == null)
                {
                    InfoDataSource source1;
                    string text1;
                    if (item1.ParentItem == null)
                    {
                        TDatasetItem item2 = item1.ChildItem;
                    }
                    if ((item1.ParentItem != null) && (item1.ChildItem == null))
                    {
                        if (item1.ParentItem.DataSource != null && item1.ParentItem.DataSource.Detail == null)
                        {
                            item1.DataSource = item1.ParentItem.DataSource;
                            item1.DataSource.Detail = item1.Command;
                            SetColumns(item1.DataSource.DetailColumns, item1.FieldAttrItems, false);
                        }
                        else
                        {
                            String strMaster = item1.ParentItem.TableName;
                            if (strMaster.Contains("."))
                                strMaster = strMaster.Remove(0, strMaster.IndexOf(".") + 1);
                            String strDetail = item1.TableName;
                            if (strDetail.Contains("."))
                                strDetail = strDetail.Remove(0, strDetail.IndexOf(".") + 1);
                            text1 = "id" + WzdUtils.RemoveSpace(strMaster) + "_" + WzdUtils.RemoveSpace(strDetail);
                            source1 = FDesignerHost.CreateComponent(typeof(InfoDataSource), text1) as InfoDataSource;
                            item1.DataSource = source1;
                            item1.ParentItem.DataSource = source1;
                            source1.Master = item1.ParentItem.Command;
                            SetColumns(source1.MasterColumns, item1.FieldAttrItems, true);
                            source1.Detail = item1.Command;
                            SetColumns(source1.DetailColumns, item1.FieldAttrItems, false);
                        }
                    }
                    if ((item1.ParentItem != null) && (item1.ChildItem != null))
                    {
                        if (item1.ParentItem.DataSource != null && item1.ParentItem.DataSource.Detail == null)
                        {
                            item1.DataSource = item1.ParentItem.DataSource;
                            item1.DataSource.Detail = item1.Command;
                            SetColumns(item1.DataSource.DetailColumns, item1.FieldAttrItems, false);
                        }
                        else
                        {
                            String strMaster = item1.ParentItem.TableName;
                            if (strMaster.Contains("."))
                                strMaster = strMaster.Remove(0, strMaster.IndexOf(".") + 1);
                            String strDetail = item1.TableName;
                            if (strDetail.Contains("."))
                                strDetail = strDetail.Remove(0, strDetail.IndexOf(".") + 1);
                            text1 = "id" + WzdUtils.RemoveSpace(strMaster) + "_" + WzdUtils.RemoveSpace(strDetail);
                            source1 = FDesignerHost.CreateComponent(typeof(InfoDataSource), text1) as InfoDataSource;
                            item1.DataSource = source1;
                            item1.ParentItem.DataSource = source1;
                            source1.Master = item1.ParentItem.Command;
                            SetColumns(source1.MasterColumns, item1.FieldAttrItems, true);
                            source1.Detail = item1.Command;
                            SetColumns(source1.DetailColumns, item1.FieldAttrItems, false);
                        }
                        //if (item1.ChildItem.DataSource != null && item1.ChildItem.DataSource.Master == null)
                        //{
                        //    item1.DataSource = item1.ChildItem.DataSource;
                        //    item1.DataSource.Master = item1.Command;
                        //    SetColumns(item1.DataSource.MasterColumns, item1.FieldAttrItems, true);
                        //}
                        //else
                        //{
                        //    text1 = "id" + WzdUtils.RemoveSpace(item1.TableName) + "_" + WzdUtils.RemoveSpace(item1.ChildItem.TableName);
                        //    source1 = FDesignerHost.CreateComponent(typeof(InfoDataSource), text1) as InfoDataSource;
                        //    item1.DataSource = source1;
                        //    item1.ChildItem.DataSource = source1;
                        //    source1.Master = item1.Command;
                        //    SetColumns(source1.MasterColumns, item1.FieldAttrItems, true);
                        //    source1.Detail = item1.ChildItem.Command;
                        //    SetColumns(source1.DetailColumns, item1.FieldAttrItems, false);
                        //}
                    }
                }
            }
        }

        private void GenConnection()
        {
            FConnection = FDesignerHost.CreateComponent(typeof(InfoConnection), "InfoConnection1") as InfoConnection;
            //FConnection.ConnectionString = FServerData.ConnectionString;
            FConnection.EEPAlias = FServerData.EEPAlias;
            switch (FServerData.DatabaseType)
            {
                case ClientType.ctMsSql:
                    FConnection.ConnectionType = ConnectionType.SqlClient;
                    break;
                case ClientType.ctODBC:
                    FConnection.ConnectionType = ConnectionType.Odbc;
                    break;
                case ClientType.ctOleDB:
                    FConnection.ConnectionType = ConnectionType.OleDb;
                    break;
                case ClientType.ctOracle:
                    FConnection.ConnectionType = ConnectionType.OracleClient;
                    break;
                case ClientType.ctMySql:
                    FConnection.ConnectionType = ConnectionType.MySqlClient;
                    break;
                case ClientType.ctInformix:
                    FConnection.ConnectionType = ConnectionType.IfxClient;
                    break;
                case ClientType.ctSybase:
                    FConnection.ConnectionType = ConnectionType.Sybase;
                    break;
            }
            if (FConnection.ConnectionString == "" || FConnection.ConnectionString == null)
            {
                try
                {
                    FConnection.ConnectionString = EEPRegistry.WizardConnectionString;
                }
                catch { }
                switch (FServerData.DatabaseType)
                {
                    case ClientType.ctMsSql:
                        FConnection.ConnectionType = ConnectionType.SqlClient;
                        break;
                    case ClientType.ctODBC:
                        FConnection.ConnectionType = ConnectionType.Odbc;
                        break;
                    case ClientType.ctOleDB:
                        FConnection.ConnectionType = ConnectionType.OleDb;
                        break;
                    case ClientType.ctOracle:
                        FConnection.ConnectionType = ConnectionType.OracleClient;
                        break;
                    case ClientType.ctMySql:
                        FConnection.ConnectionType = ConnectionType.MySqlClient;
                        break;
                    case ClientType.ctInformix:
                        FConnection.ConnectionType = ConnectionType.IfxClient;
                        break;
                    case ClientType.ctSybase:
                        FConnection.ConnectionType = ConnectionType.Sybase;
                        break;
                }
            }
        }

        private void GenDatasets()
        {
            int I;
            TDatasetItem DatasetItem;
            InfoCommand IC;
            for (I = 0; I < FServerData.Datasets.Count; I++)
            {
                DatasetItem = (TDatasetItem)FServerData.Datasets[I];
                IC = CreateDataset(DatasetItem, I + 1) as Srvtools.InfoCommand;
                CreateUpdateComponent(IC, DatasetItem, I + 1);
            }
            FixupMasterDetail();
        }

        private void GenViewDataSet()
        {
            foreach (TDatasetItem DatasetItem in FServerData.Datasets)
            {
                if (DatasetItem.ParentItem == null)
                {
                    string ComponentName = CreateUniqueComponentName(FDataModule, "View_" + WzdUtils.RemoveSpecialCharacters(DatasetItem.TableName));
                    if (ComponentName.Contains("."))
                    {
                        ComponentName = ComponentName.Remove(0, ComponentName.IndexOf('.') + 1);
                        ComponentName = "View_" + ComponentName;
                    }
                    if (ComponentName.Contains(" "))
                    {
                        String[] temp = ComponentName.Split(' ');
                        ComponentName = "";
                        foreach (String str in temp)
                            ComponentName += str.Trim();
                    }
                    InfoCommand IC = FDesignerHost.CreateComponent(typeof(InfoCommand), ComponentName) as InfoCommand;
                    //IComponentChangeService componentChangeService = FDesignerHost.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
                    //componentChangeService.OnComponentChanging(IC, TypeDescriptor.GetProperties(IC)["InfoConnection"]);
                    //componentChangeService.OnComponentChanged(IC, TypeDescriptor.GetProperties(IC)["InfoConnection"], null, IC.InfoConnection);
                    IC.InfoConnection = FConnection;
                    IC.CommandText = "SELECT * FROM " + WzdUtils.Quote(DatasetItem.TableName, IC.InfoConnection.InternalDbConnection);
                    DatasetItem.Command = IC;
                    int I;
                    TFieldAttrItem FAI;
                    IC.KeyFields.Clear();
                    Srvtools.KeyItem KI;
                    for (I = 0; I < DatasetItem.FieldAttrItems.Count; I++)
                    {
                        FAI = (TFieldAttrItem)DatasetItem.FieldAttrItems[I];
                        if (FAI.IsKey)
                        {
                            KI = new Srvtools.KeyItem();
                            KI.KeyName = FAI.DataField;
                            IC.KeyFields.Add(KI);
                        }
                    }
                }
            }
        }
    }
}