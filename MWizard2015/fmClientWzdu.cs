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
using System.ComponentModel.Design;
using System.IO;
using Srvtools;
using Microsoft.Win32;
using System.Data.Common;
using System.Reflection;
using System.Xml;
using System.Windows.Forms.Design;
using System.Threading;

namespace MWizard2015
{
    public partial class fmClientWzard : Form
    {
        private TClientData FClientData;
        private DTE2 FDTE2;
        private AddIn FAddIn;
        private DbConnection InternalConnection = null;
        private TStringList FAlias;
        private static string _serverPath;
        private InfoDataSet FInfoDataSet = null;
        private string[] FProviderNameList;
        private ListViewColumnSorter lvwColumnSorterViewSrc;
        private ListViewColumnSorter lvwColumnSorterViewDes;
        private ListViewColumnSorter lvwColumnSorterMasterSrc;
        private ListViewColumnSorter lvwColumnSorterMasterDes;
        private ListViewColumnSorter lvwColumnSorterDetail;

        public fmClientWzard()
        {
            InitializeComponent();
            FClientData = new TClientData(this);
            //PrepareWizardService();

            lvwColumnSorterViewSrc = new ListViewColumnSorter();
            lvwColumnSorterViewDes = new ListViewColumnSorter();
            lvwColumnSorterMasterSrc = new ListViewColumnSorter();
            lvwColumnSorterMasterDes = new ListViewColumnSorter();
            lvwColumnSorterDetail = new ListViewColumnSorter();
            this.lvViewSrcField.ListViewItemSorter = lvwColumnSorterViewSrc;
            this.lvViewDesField.ListViewItemSorter = lvwColumnSorterViewDes;
            this.lvMasterSrcField.ListViewItemSorter = lvwColumnSorterMasterSrc;
            this.lvMasterDesField.ListViewItemSorter = lvwColumnSorterMasterDes;
            this.lvSelectedFields.ListViewItemSorter = lvwColumnSorterDetail;
        }

        public fmClientWzard(DTE2 aDTE2, AddIn aAddIn)
        {
            InitializeComponent();
            FClientData = new TClientData(this);
            FDTE2 = aDTE2;
            FAddIn = aAddIn;
            //PrepareWizardService();

            lvwColumnSorterViewSrc = new ListViewColumnSorter();
            lvwColumnSorterViewDes = new ListViewColumnSorter();
            lvwColumnSorterMasterSrc = new ListViewColumnSorter();
            lvwColumnSorterMasterDes = new ListViewColumnSorter();
            lvwColumnSorterDetail = new ListViewColumnSorter();
            this.lvViewSrcField.ListViewItemSorter = lvwColumnSorterViewSrc;
            this.lvViewDesField.ListViewItemSorter = lvwColumnSorterViewDes;
            this.lvMasterSrcField.ListViewItemSorter = lvwColumnSorterMasterSrc;
            this.lvMasterDesField.ListViewItemSorter = lvwColumnSorterMasterDes;
            this.lvSelectedFields.ListViewItemSorter = lvwColumnSorterDetail;
        }

        private void PrepareWizardService()
        {
            Show();
            Hide();
        }

        private void ClearValues()
        {
            tbCurrentSolution.Text = "";
            tbNewLocation.Text = "";
            tbNewSolutionName.Text = "";
            tbPackageName.Text = "ClientPackage";
            tbSolutionName.Text = "";
            tbTableName.Text = "";
            cbBaseForm.Text = "CSingle";
            tbProviderName.Text = "";
            tbFormName.Text = "Form1";
            tbDetailTableName.Text = "";
            cbViewProviderName.Items.Clear();
            cbViewProviderName.Text = "";
            tbAssemblyOutputPath.Text = "";
            cbColumnCount.SelectedIndex = 0;
            cbLabelAliement.SelectedIndex = 0;
            ClearAll();
        }

        private void ClearAll()
        {
            tbCaption.Text = "";
            cbCheckNull.Text = "";
            tbDefaultValue.Text = "";
            cbQueryMode.Text = "";
            tbEditMask.Text = "";
            cbControlType.Text = "";
            cbComboTableName.Text = "";
            cbComboTableName.Items.Clear();
            cbDataTextField.Text = "";
            cbDataValueField.Text = "";
            cbRefValNo.Text = "";
            cbRefValNo.Items.Clear();
            //ClearRefValButton(lvMasterDesField);
            lvMasterDesField.Items.Clear();
            lvMasterSrcField.Items.Clear();
            lvViewSrcField.Items.Clear();
            lvViewDesField.Items.Clear();
            FClientData.Blocks.Clear();
            tvRelation.Nodes.Clear();
            tbCaption_D.Text = "";
            cbCheckNull_D.Text = "";
            tbDefaultValue_D.Text = "";
            cbQueryMode_D.Text = "";
            tbEditMask_D.Text = "";
            cbControlType_D.Text = "";
            cbComboTableName_D.Text = "";
            cbComboTableName_D.Items.Clear();
            cbComboDisplayField_D.Text = "";
            cbComboValueField_D.Text = "";
            cbRefValNo_D.Text = "";
            cbRefValNo_D.Items.Clear();
            //ClearRefValButton(lvSelectedFields);
            lvSelectedFields.Items.Clear();
            rbAddToExistSln_Click(rbAddToExistSln, null);
            rbEEPBaseForm.Checked = false;
            rbEEPBaseForm.Checked = true;
        }

        private void Init()
        {
            ClearValues();
            LoadDBString();
            FInfoDataSet = new InfoDataSet();
            if (((FDTE2 != null) && (FDTE2.Solution.FileName != "")) && File.Exists(FDTE2.Solution.FileName))
            {
                rbAddToCurrent.Enabled = true;
                rbAddToCurrent.Checked = true;
                tbCurrentSolution.Text = FDTE2.Solution.FileName;
                EnabledOutputControls();
            }
            FInfoDataSet.SetWizardDesignMode(true);
            try
            {
                cbEEPAlias.Text = EEPRegistry.WizardConnectionString;
                cbDatabaseType.Text = EEPRegistry.DataBaseType;
            }
            catch { }
            DisplayPage(tpConnection);
        }

        public DbConnection GlobalConnection
        {
            get { return InternalConnection; }
        }

        public String SelectedAlias
        {
            get { return cbEEPAlias.Text; }
        }

        public void SDGenClientModule(string XML)
        {
            if (XML != "")
            {
                FClientData.Blocks.Clear();
                FClientData.LoadFromXML(XML);
            }
            TClientGenerator CG = new TClientGenerator(FClientData, FDTE2, FAddIn);
            CG.GenClientModule();
        }

        private void DisplayPage(TabPage aPage)
        {
            tabControl.TabPages.Clear();
            tabControl.TabPages.Add(aPage);
            tabControl.SelectedTab = aPage;
            EnableButton();
        }

        private void EnableButton()
        {
            btnPrevious.Enabled = tabControl.SelectedTab != tpConnection;
            if (FClientData.IsMasterDetailBaseForm())
            {
                btnNext.Enabled = tabControl.SelectedTab != tpDetailFields;
                btnDone.Enabled = tabControl.SelectedTab == tpDetailFields;
            }
            else
            {
                btnNext.Enabled = tabControl.SelectedTab != tpMasterFields;
                btnDone.Enabled = tabControl.SelectedTab == tpMasterFields;
            }
            btnCancel.Enabled = true;
        }

        private static string GetServerPath()
        {
            if ((fmClientWzard._serverPath == null) || (fmClientWzard._serverPath.Length == 0))
            {
                fmClientWzard._serverPath = EEPRegistry.Server + "\\";
            }
            return fmClientWzard._serverPath;
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

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'flow7_testDataSet.COLDEF' table. You can move, or remove it, as needed.
            //LoadDBString();
        }

        public void ShowClientWizard()
        {
            //Show();
            Init();
            ShowDialog();
        }

        private void SetFieldNames(string TableName, ListView LV)
        {
            int I;
            DataRow[] DRs;
            DataRow DR;
            InfoCommand aInfoCommand = new InfoCommand(FClientData.DatabaseType);
            aInfoCommand.Connection = InternalConnection;
            String OWNER = String.Empty, SS = TableName;
            if (SS.Contains("."))
            {
                OWNER = WzdUtils.GetToken(ref SS, new char[] { '.' });
                TableName = SS;
            }
            aInfoCommand.CommandText = "Select * from COLDEF where TABLE_NAME='" + TableName + "' OR TABLE_NAME='" + OWNER + "." + TableName + "'";
            IDbDataAdapter DA = DBUtils.CreateDbDataAdapter(aInfoCommand);
            DataSet dsColdef = new DataSet();
            WzdUtils.FillDataAdapter(FClientData.DatabaseType, DA, dsColdef, TableName);

            DataTable dtTableSchema = FInfoDataSet.RealDataSet.Tables[0];
            for (I = 0; I < dtTableSchema.Columns.Count; I++)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Text = dtTableSchema.Columns[I].ColumnName;
                DRs = dsColdef.Tables[0].Select("FIELD_NAME='" + lvi.Text + "'");
                TBlockFieldItem aBlockFieldItem = new TBlockFieldItem();
                aBlockFieldItem.DataField = lvi.Text;
                aBlockFieldItem.DataType = dtTableSchema.Columns[I].DataType;
                lvi.Tag = aBlockFieldItem;
                if (DRs.Length > 0)
                {
                    DR = DRs[0];
                    lvi.SubItems.Add(DR["CAPTION"].ToString());

                    aBlockFieldItem.Description = DR["CAPTION"].ToString();
                    aBlockFieldItem.CheckNull = DR["CHECK_NULL"].ToString().ToUpper();
                    aBlockFieldItem.DefaultValue = DR["DEFAULT_VALUE"].ToString();
                    aBlockFieldItem.ControlType = DR["NEEDBOX"].ToString();
                    aBlockFieldItem.EditMask = DR["EDITMASK"].ToString();
                    if (DR["FIELD_LENGTH"] != null && DR["FIELD_LENGTH"].ToString() != "")
                        aBlockFieldItem.Length = Convert.ToInt32(DR["FIELD_LENGTH"].ToString());
                    if (aBlockFieldItem.DataType == typeof(DateTime))
                    {
                        if (aBlockFieldItem.ControlType == null || aBlockFieldItem.ControlType == "")
                            aBlockFieldItem.ControlType = "DateTimeBox";
                    }
                    aBlockFieldItem.QueryMode = DR["QUERYMODE"].ToString();
                }
                LV.Items.Add(lvi);
            }
            /*
    		string[] S = new string[4];
			S[2] = TableName;
            DataTable dtTableSchema = InternalConnection.GetSchema("Columns", S);
            DataRow[] DRs1 = dtTableSchema.Select("", "ORDINAL_POSITION ASC");
            for (I = 0; I < DRs1.Length; I++)
			{
				ListViewItem lvi = new ListViewItem();
				lvi.Text = DRs1[I][3].ToString();
				DRs = dsColdef.Tables[0].Select("FIELD_NAME='" + lvi.Text + "'");
				if (DRs.Length == 1)
				{
					DR = DRs[0];
					lvi.SubItems.Add(DR["CAPTION"].ToString());
				}
				LV.Items.Add(lvi);
			}
             */
        }

        private void SetFieldNamesByProvider(String TableName, String ProviderName, ListView aListView)
        {
            if (ProviderName == null || ProviderName.Trim() == "")
                return;

            InfoCommand aInfoCommand = new InfoCommand(FClientData.DatabaseType);
            aInfoCommand.Connection = InternalConnection;
            string OWNER = string.Empty, SS = TableName;
            if (SS.Contains("."))
            {
                OWNER = WzdUtils.GetToken(ref SS, new char[] { '.' });
                TableName = SS;
            }
            aInfoCommand.CommandText = "Select * from COLDEF where TABLE_NAME='" + TableName + "' OR TABLE_NAME='" + OWNER + "." + TableName + "'";
            IDbDataAdapter DA = DBUtils.CreateDbDataAdapter(aInfoCommand);
            DataSet dsColdef = new DataSet();
            WzdUtils.FillDataAdapter(FClientData.DatabaseType, DA, dsColdef, TableName);

            aListView.Items.Clear();
            InfoDataSet aDataSet = new InfoDataSet();
            try
            {
                aDataSet.SetWizardDesignMode(true);
                aDataSet.RemoteName = ProviderName;
                aDataSet.AlwaysClose = true;
                aDataSet.Active = true;
                DataTable Table = aDataSet.RealDataSet.Tables[0];
                foreach (DataColumn Column in Table.Columns)
                {
                    ListViewItem aItem = new ListViewItem(Column.ColumnName);
                    DataRow[] DRS = dsColdef.Tables[0].Select("FIELD_NAME='" + Column.ColumnName + "'");
                    if (DRS.Length > 0)
                        aItem.SubItems.Add(DRS[0]["CAPTION"].ToString());
                    else
                        aItem.SubItems.Add("");
                    aListView.Items.Add(aItem);
                    TBlockFieldItem aFieldItem = new TBlockFieldItem();
                    aFieldItem.DataField = Column.ColumnName;
                    aFieldItem.DataType = Column.DataType;
                    aItem.Tag = aFieldItem;
                }
            }
            finally
            {
                aDataSet.Dispose();
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab.Equals(tpConnection))
            {
                WzdUtils.SetRegistryValueByKey("WizardConnectionString", cbEEPAlias.Text);
                WzdUtils.SetRegistryValueByKey("DatabaseType", cbDatabaseType.Text);

                if (FDTE2.Solution.FullName == null || FDTE2.Solution.FullName == "")
                {
                    rbAddToExistSln.Checked = true;
                    rbAddToExistSln_Click(rbAddToExistSln, null);
                }
                else
                    rbAddToCurrent.Checked = true;
                string type = FindDBType(cbEEPAlias.Text);
                switch (type)
                {
                    case "1":
                        FClientData.DatabaseType = ClientType.ctMsSql; break;
                    case "2":
                        FClientData.DatabaseType = ClientType.ctOleDB; break;
                    case "3":
                        FClientData.DatabaseType = ClientType.ctOracle; break;
                    case "4":
                        FClientData.DatabaseType = ClientType.ctODBC; break;
                    case "5":
                        FClientData.DatabaseType = ClientType.ctMySql; break;
                    case "6":
                        FClientData.DatabaseType = ClientType.ctInformix; break;
                    case "7":
                        FClientData.DatabaseType = ClientType.ctSybase; break;
                }
                DisplayPage(tpOutputSetting);
                if (cbChooseLanguage.Text == "" || cbChooseLanguage.Text == "C#")
                    FClientData.Language = "cs";
                else if (cbChooseLanguage.Text == "VB")
                    FClientData.Language = "vb";

            }
            else if (tabControl.SelectedTab.Equals(tpOutputSetting))
            {
                if (rbAddToCurrent.Checked)
                {
                    FClientData.SolutionName = tbCurrentSolution.Text;
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
                    FClientData.SolutionName = tbNewSolutionName.Text;
                    FClientData.OutputPath = tbNewLocation.Text;
                    FClientData.CodeOutputPath = tbOutputPath.Text;
                    FDTE2.Solution.Create(FClientData.OutputPath, FClientData.SolutionName);
                    FDTE2.Solution.SaveAs(FClientData.OutputPath + "\\" + FClientData.SolutionName + ".sln");
                    FDTE2.Solution.Open(FClientData.OutputPath + "\\" + FClientData.SolutionName + ".sln");
                }
                if (rbAddToExistSln.Checked)
                {
                    if (tbSolutionName.Text == "")
                    {
                        MessageBox.Show("Please input Location !!");
                        if (tbNewLocation.CanFocus)
                        {
                            tbNewLocation.Focus();
                        }
                        return;
                    }
                    FClientData.SolutionName = tbSolutionName.Text;
                    FClientData.CodeOutputPath = tbOutputPath.Text;
                    if (string.Compare(tbSolutionName.Text, FDTE2.Solution.FullName) != 0)
                        FDTE2.Solution.Open(FClientData.SolutionName);
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
                    FClientData.PackageName = tbPackageName.Text;
                    tbClientPackage.Text = tbPackageName.Text;
                    FClientData.NewSolution = rbNewSolution.Checked;
                    FClientData.CodeOutputPath = tbOutputPath.Text;
                    DisplayPage(tpFormSetting);
                    tbProviderName.Text = "";
                }
                FClientData.AssemblyOutputPath = tbAssemblyOutputPath.Text;
                if (this.cbChooseLanguage.SelectedItem != null)
                {
                    switch (this.cbChooseLanguage.SelectedItem.ToString())
                    {
                        case "C#":
                            this.cbBaseForm.Items.Clear();
                            this.cbBaseForm.Items.Add("CSingle");
                            this.cbBaseForm.Items.Add("CMasterDetail");
                            this.cbBaseForm.Items.Add("CQuery");
                            this.cbBaseForm.SelectedIndex = 0;
                            break;
                        case "VB":
                            this.cbBaseForm.Items.Clear();
                            this.cbBaseForm.Items.Add("VBCSingle");
                            this.cbBaseForm.Items.Add("VBCMasterDetail");
                            this.cbBaseForm.Items.Add("VBCQuery");
                            this.cbBaseForm.SelectedIndex = 0;
                            break;
                    }
                }
                else
                {
                    this.cbBaseForm.Items.Clear();
                    this.cbBaseForm.Items.Add("CSingle");
                    this.cbBaseForm.Items.Add("CMasterDetail");
                    this.cbBaseForm.Items.Add("CQuery");
                    this.cbBaseForm.SelectedIndex = 0;
                }
            }
            else if (tabControl.SelectedTab.Equals(tpFormSetting))
            {
                if (rbEEPBaseForm.Checked && (cbBaseForm.Text == ""))
                {
                    MessageBox.Show("Please select EEP Windows Templates Form !!");
                    if (cbBaseForm.CanFocus)
                    {
                        cbBaseForm.Focus();
                    }
                }
                else if (tbFormName.Text == "")
                {
                    MessageBox.Show("Please input Form Name !!");
                    if (tbFormName.CanFocus)
                    {
                        tbFormName.Focus();
                    }
                }
                else
                {
                    FClientData.FormName = tbFormName.Text;
                    FClientData.FormText = tbFormText.Text;
                    if (rbWindowsForm.Checked)
                    {
                        FClientData.BaseFormName = "System.Windows.Form";
                    }
                    else
                    {
                        FClientData.BaseFormName = cbBaseForm.Text;
                        cbViewProviderName.Visible = string.Compare(cbBaseForm.Text, "CMasterDetail") == 0 || string.Compare(cbBaseForm.Text, "VBCMasterDetail") == 0;
                        label18.Visible = string.Compare(cbBaseForm.Text, "CMasterDetail") == 0 || string.Compare(cbBaseForm.Text, "VBCMasterDetail") == 0;
                    }
                    DisplayPage(tpDataSource);
                }
            }
            else if (tabControl.SelectedTab.Equals(tpDataSource))
            {
                if (tbProviderName.Text == "")
                {
                    MessageBox.Show("Please input Provider Name !!");
                    if (tbProviderName.CanFocus)
                    {
                        tbProviderName.Focus();
                    }
                }
                else if (tbTableName.Text == "")
                {
                    MessageBox.Show("Please input Table Name !!");
                    if (tbTableName.CanFocus)
                    {
                        tbTableName.Focus();
                    }
                }
                else if (cbViewProviderName.Visible && cbViewProviderName.Text == "")
                {
                    MessageBox.Show("Please input View Provider Name !!");
                    if (cbViewProviderName.CanFocus)
                    {
                        cbViewProviderName.Focus();
                    }
                }
                else
                {
                    FClientData.ProviderName = tbProviderName.Text;
                    FClientData.TableName = tbTableName.Text;
                    if (lvViewSrcField.Items.Count == 0 && lvViewDesField.Items.Count == 0)
                    {
                        if (cbViewProviderName.Visible)
                            SetFieldNamesByProvider(FClientData.TableName, FClientData.ViewProviderName, lvViewSrcField);
                        else
                            SetFieldNames(FClientData.TableName, lvViewSrcField);
                    }
                    if (lvMasterSrcField.Items.Count == 0 && lvMasterDesField.Items.Count == 0)
                        SetFieldNames(FClientData.TableName, lvMasterSrcField);
                    if (FClientData.BaseFormName == "CSingle" || FClientData.BaseFormName == "CMasterDetail" ||
                        FClientData.BaseFormName == "VBCSingle" || FClientData.BaseFormName == "VBCMasterDetail")
                        DisplayPage(tpViewFields);
                    else
                        DisplayPage(tpMasterFields);
                }
            }
            else if (tabControl.SelectedTab.Equals(tpViewFields))
            {
                DisplayPage(tpMasterFields);
            }
            else if (tabControl.SelectedTab.Equals(tpMasterFields) && FClientData.IsMasterDetailBaseForm())
            {
                DisplayPage(tpDetailFields);
            }
            BringToFront();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab.Equals(tpOutputSetting))
            {
                DisplayPage(tpConnection);
            }
            else
            {
                if (tabControl.SelectedTab.Equals(tpFormSetting))
                {
                    DisplayPage(tpOutputSetting);
                }
                if (tabControl.SelectedTab.Equals(tpDataSource))
                {
                    DisplayPage(tpFormSetting);
                }
                if (tabControl.SelectedTab.Equals(tpViewFields))
                {
                    DisplayPage(tpDataSource);
                }
                if (tabControl.SelectedTab.Equals(tpMasterFields))
                {
                    if (FClientData.BaseFormName == "CSingle" || FClientData.BaseFormName == "CMasterDetail"
                        || FClientData.BaseFormName == "VBCSingle" || FClientData.BaseFormName == "VBCMasterDetail")
                        DisplayPage(tpViewFields);
                    else
                        DisplayPage(tpDataSource);
                }
                if (tabControl.SelectedTab.Equals(tpDetailFields))
                {
                    DisplayPage(tpMasterFields);
                }
            }

        }

        private void rbWindowsForm_CheckedChanged(object sender, EventArgs e)
        {
            if (rbWindowsForm.Checked)
            {
                cbBaseForm.Enabled = false;
            }
        }

        private void rbEEPBaseForm_CheckedChanged(object sender, EventArgs e)
        {
            if (rbEEPBaseForm.Checked)
            {
                cbBaseForm.Enabled = true;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ClearAllListViewSort();

            FInfoDataSet.Dispose();
            FInfoDataSet = null;
            Hide();
        }

        private void btnViewAdd_Click(object sender, EventArgs e)
        {
            SelectFields(lvViewSrcField, lvViewDesField, false);
        }

        private void ClearRefValButton(ListView LV)
        {
            //foreach (ListViewItem LVI in LV.Items)
            //{
            //    if (LVI.SubItems.Count > 1)
            //    {
            //        ListViewItem.ListViewSubItem LVSI = LVI.SubItems[2];
            //        if (LVSI != null)
            //        {
            //            if (LVSI.Tag != null)
            //            {
            //                ((Button)LVSI.Tag).Dispose();
            //            }
            //        }
            //    }
            //}
        }

        private void SelectFields(ListView lvSrc, ListView lvDes, Boolean All)
        {
            int I;
            ListViewItem Item;
            ListViewItem.ListViewSubItem LVSI;

            for (I = 0; I < lvSrc.Items.Count; I++)
            {
                if (lvSrc.Items[I].Selected || All)
                {
                    Item = new ListViewItem(lvSrc.Items[I].Text);
                    Item.Tag = lvSrc.Items[I].Tag;
                    if (lvSrc.Items[I].SubItems.Count > 1)
                        Item.SubItems.Add(lvSrc.Items[I].SubItems[1]);
                    else
                        Item.SubItems.Add("");
                    lvDes.Items.Add(Item);
                    if (lvDes.Columns.Count == 3)
                    {
                        LVSI = Item.SubItems.Add("");
                        Button B = new Button();
                        B.Parent = lvDes;
                        RearrangeRefValButton(B, LVSI.Bounds);
                        B.BackColor = Color.Silver;
                        B.BringToFront();
                        LVSI.Tag = B;
                        B.Tag = Item;
                        B.Click += new EventHandler(btnRefVal_Click);
                        B.Text = "...";
                    }
                }
            }

            for (I = lvSrc.Items.Count - 1; I >= 0; I--)
            {
                if (lvSrc.Items[I].Selected || All)
                {
                    if (lvSrc.Columns.Count == 3)
                    {
                        LVSI = lvSrc.Items[I].SubItems[2];
                        if (LVSI != null)
                        {
                            if (LVSI.Tag != null)
                            {
                                ((Button)LVSI.Tag).Dispose();
                            }
                        }
                    }
                    lvSrc.Items[I].Remove();
                }
            }

            if (lvSrc.Columns.Count == 3)
            {
                foreach (ListViewItem LVI in lvSrc.Items)
                {
                    LVSI = LVI.SubItems[2];
                    if (LVSI.Tag != null)
                        RearrangeRefValButton((Button)LVSI.Tag, LVSI.Bounds);
                }
            }
        }

        private void DisplayValue(ListView aListView)
        {
            foreach (ListViewItem aViewItem in aListView.Items)
            {
                TBlockFieldItem BlockFieldItem = (TBlockFieldItem)aViewItem.Tag;
                aViewItem.SubItems[1].Text = BlockFieldItem.Description;
            }
        }

        private void btnRefVal_Click(object sender, EventArgs e)
        {
            Object aObject = ((System.Windows.Forms.Button)sender).Tag;
            ListViewItem.ListViewSubItem LVSI = null;
            ListViewItem aViewItem = null;
            TBlockFieldItem BlockFieldItem = null;

            if (aObject.GetType().Equals(typeof(ListViewItem.ListViewSubItem)))
            {
                MessageBox.Show("This is wrong way");
                throw new Exception("");
                //LVSI = (ListViewItem.ListViewSubItem)((System.Windows.Forms.Button)sender).Tag;
                //BlockFieldItem = (TBlockFieldItem)LVSI.Tag;
            }
            if (aObject.GetType().Equals(typeof(ListViewItem)))
            {
                aViewItem = (ListViewItem)((System.Windows.Forms.Button)sender).Tag;
                BlockFieldItem = (TBlockFieldItem)aViewItem.Tag;
                LVSI = ((ListViewItem)aObject).SubItems[2];
            }

            fmFieldSetting aForm = new fmFieldSetting(InternalConnection, FClientData.DatabaseType, aViewItem.ListView, TWizardType.wtWinForm, FClientData.DatabaseName);
            try
            {
                String[] Params = new String[] { BlockFieldItem.Description, BlockFieldItem.CheckNull,
                    BlockFieldItem.DefaultValue, BlockFieldItem.ControlType, BlockFieldItem.RefValNo,
                    BlockFieldItem.ComboEntityName, BlockFieldItem.ComboTextField, BlockFieldItem.ComboValueField, BlockFieldItem.EditMask};
                if (aForm.ShowRefValForm(Params))
                {
                    //BlockFieldItem.Description = Params[0];
                    //BlockFieldItem.CheckNull = Params[1].ToUpper();
                    //BlockFieldItem.DefaultValue = Params[2];
                    //BlockFieldItem.ControlType = Params[3];
                    //BlockFieldItem.RefValNo = Params[4];
                    //BlockFieldItem.ComboTableName = Params[5];
                    //BlockFieldItem.ComboTextField = Params[6];
                    //BlockFieldItem.ComboValueField = Params[7];
                }
                //LVSI.Text = Params[4];
                //aViewItem.SubItems[1].Text = BlockFieldItem.Description;
                DisplayValue(aViewItem.ListView);
            }
            finally
            {
                aForm.Dispose();
            }
        }

        private void btnViewRemove_Click(object sender, EventArgs e)
        {
            SelectFields(lvViewDesField, lvViewSrcField, false);
        }

        private void btnViewAddAll_Click(object sender, EventArgs e)
        {
            SelectFields(lvViewSrcField, lvViewDesField, true);
        }

        private void btnViewRemoveAll_Click(object sender, EventArgs e)
        {
            SelectFields(lvViewDesField, lvViewSrcField, true);
        }

        private void btnMasterAll_Click(object sender, EventArgs e)
        {
            SelectFields(lvMasterSrcField, lvMasterDesField, false);
        }

        private void btnMasterRemove_Click(object sender, EventArgs e)
        {
            SelectFields(lvMasterDesField, lvMasterSrcField, false);
        }

        private void btnMasterAddAll_Click(object sender, EventArgs e)
        {
            SelectFields(lvMasterSrcField, lvMasterDesField, true);
        }

        private void btnMasterRemoveAll_Click(object sender, EventArgs e)
        {
            SelectFields(lvMasterDesField, lvMasterSrcField, true);
        }

        //private void AddDetailBlockItem(string MasterItemName, TreeNodeCollection NodeCollection)
        //{
        //    for (int I = 0; I < NodeCollection.Count; I++)
        //    {
        //        TDetailItem DetailItem = (TDetailItem)NodeCollection[I].Tag;
        //        TBlockItem BlockItem = new TBlockItem();
        //        BlockItem.Name = NodeCollection[I].Text;
        //        BlockItem.RelationName = DetailItem.Relation.RelationName;
        //        BlockItem.TableName = DetailItem.TableName;
        //        if (NodeCollection[I].Parent != null)
        //        {
        //            BlockItem.ParentItemName = NodeCollection[I].Parent.Text;
        //        }
        //        else
        //        {
        //            BlockItem.ParentItemName = MasterItemName;
        //        }
        //        FClientData.Blocks.Add(BlockItem);
        //        BlockItem.BlockFieldItems = DetailItem.BlockFieldItems;
        //        AddDetailBlockItem(MasterItemName, NodeCollection[I].Nodes);
        //    }
        //}

        private void AddDetailBlockItem(string MasterItemName, System.Windows.Forms.TreeNodeCollection NodeCollection, ListView LV)
        {
            for (int I = 0; I < NodeCollection.Count; I++)
            {
                TBlockItem BlockItem = new TBlockItem();
                BlockItem.Name = NodeCollection[I].Text;
                BlockItem.TableName = ((TDetailItem)NodeCollection[I].Tag).TableName;
                BlockItem.RelationName = ((TDetailItem)NodeCollection[I].Tag).Relation.RelationName;
                for (int J = 0; J < LV.Items.Count; J++)
                {
                    ListViewItem aItem = LV.Items[J];
                    TBlockFieldItem BlockFieldItem = new TBlockFieldItem();
                    if (aItem.Tag != null)
                    {
                        BlockFieldItem.DataField = ((TBlockFieldItem)aItem.Tag).DataField;
                        BlockFieldItem.CheckNull = ((TBlockFieldItem)aItem.Tag).CheckNull;
                        BlockFieldItem.DefaultValue = ((TBlockFieldItem)aItem.Tag).DefaultValue;
                        BlockFieldItem.Description = ((TBlockFieldItem)aItem.Tag).Description;
                        BlockFieldItem.RefValNo = ((TBlockFieldItem)aItem.Tag).RefValNo;
                        BlockFieldItem.ControlType = ((TBlockFieldItem)aItem.Tag).ControlType;
                        BlockFieldItem.ComboRemoteName = ((TBlockFieldItem)aItem.Tag).ComboRemoteName;
                        BlockFieldItem.ComboEntityName = ((TBlockFieldItem)aItem.Tag).ComboEntityName;
                        BlockFieldItem.ComboTextField = ((TBlockFieldItem)aItem.Tag).ComboTextField;
                        BlockFieldItem.ComboValueField = ((TBlockFieldItem)aItem.Tag).ComboValueField;
                        BlockFieldItem.ComboTextFieldCaption = ((TBlockFieldItem)aItem.Tag).ComboTextFieldCaption;
                        BlockFieldItem.ComboValueFieldCaption = ((TBlockFieldItem)aItem.Tag).ComboValueFieldCaption;
                        BlockFieldItem.DataType = ((TBlockFieldItem)aItem.Tag).DataType;
                        BlockFieldItem.QueryMode = ((TBlockFieldItem)aItem.Tag).QueryMode;
                        BlockFieldItem.EditMask = ((TBlockFieldItem)aItem.Tag).EditMask;
                        BlockFieldItem.Length = ((TBlockFieldItem)aItem.Tag).Length;
                        BlockFieldItem.ComboOtherFields = ((TBlockFieldItem)aItem.Tag).ComboOtherFields;
                        BlockFieldItem.IsKey = ((TBlockFieldItem)aItem.Tag).IsKey;
                    }
                    else
                    {
                        BlockFieldItem.DataField = aItem.Text;
                    }
                    BlockItem.BlockFieldItems.Add(BlockFieldItem);

                }
                if (NodeCollection[I].Parent != null)
                {
                    BlockItem.ParentItemName = NodeCollection[I].Parent.Text;
                }
                else
                {
                    BlockItem.ParentItemName = MasterItemName;
                }
                FClientData.Blocks.Add(BlockItem);
                AddDetailBlockItem(MasterItemName, NodeCollection[I].Nodes, LV);
            }
        }

        private void DoGenClient()
        {
            FClientData.OutputPath = tbNewLocation.Text;
            FClientData.CodeOutputPath = tbOutputPath.Text;
            FClientData.PackageName = tbPackageName.Text;
            FClientData.FormName = tbFormName.Text;
            FClientData.TableName = tbTableName.Text;
            string OWNER = string.Empty, SS = FClientData.TableName;
            if (SS.Contains("."))
            {
                OWNER = WzdUtils.GetToken(ref SS, new char[] { '.' });
                FClientData.TableName = SS;
            }
            FClientData.ColumnCount = cbColumnCount.SelectedIndex + 1;
            FClientData.LabelAliement = cbLabelAliement.Text;
            FClientData.ViewProviderName = cbViewProviderName.Text;
            TClientGenerator Generator = new TClientGenerator(FClientData, FDTE2, FAddIn);
            if (rbEEPBaseForm.Checked)
            {
                Generator.GenClientModule();
            }
        }

        private bool bAbort = false;
        private void ShowProgressBar()
        {
            bAbort = false;
            ProgressForm aForm = new ProgressForm();
            aForm.Show();
            while (!bAbort)
            {
                aForm.progressBar1.Value += 3;
                if (aForm.progressBar1.Value >= 100)
                    aForm.progressBar1.Value = 1;
                System.Threading.Thread.Sleep(100);
            }
        }

        private void ClearAllListViewSort()
        {
            (this.lvViewSrcField.ListViewItemSorter as ListViewColumnSorter).OrderOfSort = System.Windows.Forms.SortOrder.None;
            (this.lvViewDesField.ListViewItemSorter as ListViewColumnSorter).OrderOfSort = System.Windows.Forms.SortOrder.None;
            (this.lvMasterSrcField.ListViewItemSorter as ListViewColumnSorter).OrderOfSort = System.Windows.Forms.SortOrder.None;
            (this.lvMasterDesField.ListViewItemSorter as ListViewColumnSorter).OrderOfSort = System.Windows.Forms.SortOrder.None;
            (this.lvSelectedFields.ListViewItemSorter as ListViewColumnSorter).OrderOfSort = System.Windows.Forms.SortOrder.None;

            btnViewUp.Enabled = true;
            btnViewDown.Enabled = true;
            btnMasterUp.Enabled = true;
            btnMasterDown.Enabled = true;
            btnDetailUp.Enabled = true;
            btnDetailDown.Enabled = true;
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            SetValue();
            SetValue_D();

            ClearAllListViewSort();

            FClientData.DatabaseType = FClientData.DatabaseType;
            if (FClientData.BaseFormName == "CSingle" || FClientData.BaseFormName == "CMasterDetail" ||
                FClientData.BaseFormName == "VBCSingle" || FClientData.BaseFormName == "VBCMasterDetail")
                AddBlockItem("View", FClientData.ProviderName, FClientData.TableName, lvViewDesField);
            if (FClientData.IsMasterDetailBaseForm())
            {
                AddBlockItem("Master", FClientData.ProviderName, FClientData.TableName, lvMasterDesField);
                AddDetailBlockItem("Master", tvRelation.Nodes, lvSelectedFields);
            }
            else
            {
                AddBlockItem("Main", FClientData.ProviderName, FClientData.TableName, lvMasterDesField);
            }
            /*
            DialogResult = DialogResult.OK;
            DoGenClient();
            FInfoDataSet.Dispose();
            FInfoDataSet = null;
            Hide();
             */

            Hide();
            FDTE2.MainWindow.Activate();
            DoGenClient();
            FInfoDataSet.Dispose();
            FInfoDataSet = null;
        }

        private void AddBlockItem(string BlockName, string ProviderName, string TableName, ListView LV)
        {
            int I;
            TBlockItem BlockItem = new TBlockItem();
            TBlockFieldItem BlockFieldItem;

            InfoCommand aInfoCommand = new InfoCommand(FClientData.DatabaseType);
            aInfoCommand.Connection = InternalConnection;
            string OWNER = string.Empty, SS = TableName;
            if (SS.Contains("."))
            {
                OWNER = WzdUtils.GetToken(ref SS, new char[] { '.' });
                TableName = SS;
            }
            aInfoCommand.CommandText = "Select * from COLDEF where TABLE_NAME='" + TableName + "' OR TABLE_NAME='" + OWNER + "." + TableName + "'";
            IDbDataAdapter DA = DBUtils.CreateDbDataAdapter(aInfoCommand);
            DataSet DS = new DataSet();
            WzdUtils.FillDataAdapter(FClientData.DatabaseType, DA, DS, TableName);
            BlockItem.Name = BlockName;
            BlockItem.ProviderName = ProviderName;
            BlockItem.TableName = TableName;
            for (I = 0; I < LV.Items.Count; I++)
            {
                ListViewItem aItem = LV.Items[I];
                BlockFieldItem = new TBlockFieldItem();
                BlockFieldItem.DataField = ((TBlockFieldItem)aItem.Tag).DataField;
                BlockFieldItem.CheckNull = ((TBlockFieldItem)aItem.Tag).CheckNull;
                BlockFieldItem.DefaultValue = ((TBlockFieldItem)aItem.Tag).DefaultValue;
                BlockFieldItem.Description = ((TBlockFieldItem)aItem.Tag).Description;
                BlockFieldItem.RefValNo = ((TBlockFieldItem)aItem.Tag).RefValNo;
                BlockFieldItem.ControlType = ((TBlockFieldItem)aItem.Tag).ControlType;
                BlockFieldItem.ComboEntityName = ((TBlockFieldItem)aItem.Tag).ComboEntityName;
                BlockFieldItem.ComboTextField = ((TBlockFieldItem)aItem.Tag).ComboTextField;
                BlockFieldItem.ComboValueField = ((TBlockFieldItem)aItem.Tag).ComboValueField;
                BlockFieldItem.QueryMode = ((TBlockFieldItem)aItem.Tag).QueryMode;
                BlockFieldItem.DataType = ((TBlockFieldItem)aItem.Tag).DataType;
                BlockFieldItem.EditMask = ((TBlockFieldItem)aItem.Tag).EditMask;
                BlockFieldItem.Length = ((TBlockFieldItem)aItem.Tag).Length;
                /*
                BlockFieldItem.DataField = LV.Items[I].Text;
				DRs = DS.Tables[0].Select("FIELD_NAME='" + BlockFieldItem.DataField + "'");
                if (DRs.Length == 1)
				{ 
					DR = DRs[0];
                    if (!DR.IsNull("FIELD_LENGTH"))
                       BlockFieldItem.Length = int.Parse(DR["FIELD_LENGTH"].ToString());
                    if (DR["IS_KEY"].ToString() == "Y")
					{
                        BlockFieldItem.IsKey = true;
					}
					else
					{
						BlockFieldItem.IsKey = false;
					}
                    BlockFieldItem.Description = DR["CAPTION"].ToString();
                    if (LV.Items[I].SubItems.Count == 3)
                    {
                        BlockFieldItem.RefValNo = aItem.SubItems[2].Text;
                    }
                    if (BlockFieldItem.Description == "")
					{
						BlockFieldItem.Description = BlockFieldItem.DataField;
					}

                    BlockFieldItem.CheckNull = DR["CHECK_NULL"].ToString().ToUpper();
                    BlockFieldItem.DefaultValue = DR["DEFAULT_VALUE"].ToString();
				}
                 */
                BlockItem.BlockFieldItems.Add(BlockFieldItem);
            }
            FClientData.Blocks.Add(BlockItem);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FClientData.OutputPath = tbNewLocation.Text;
            FClientData.PackageName = tbPackageName.Text;
            FClientData.FormName = tbFormName.Text;
            FClientData.TableName = tbTableName.Text;
            TClientGenerator G = new TClientGenerator(FClientData, FDTE2, FAddIn);
            G.GenClientModule();
            Close();
        }

        private void btnNewLocation_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                tbNewLocation.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void btnSolutionName_Click(object sender, EventArgs e)
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

        private void SetCodeOutputPath()
        {
            if (rbAddToCurrent.Checked)
            {
                string S = tbCurrentSolution.Text;
                if (S != "")
                {
                    S = System.IO.Path.GetDirectoryName(S);
                    String SolutionName = Path.GetFileNameWithoutExtension(tbCurrentSolution.Text);
                    tbOutputPath.Text = S + @"\" + SolutionName;
                    tbAssemblyOutputPath.Text = String.Format(@"..\..\EEPNetClient\{0}", SolutionName);
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
                    tbAssemblyOutputPath.Text = String.Format(@"..\..\EEPNetClient\{0}", SolutionName);
                }
            }
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

        private void rbAddToCurrent_Click(object sender, EventArgs e)
        {
            EnabledOutputControls();
            SetCodeOutputPath();
        }

        private void ShowChildRelation(DataRelationCollection Relations, TreeNode Node)
        {
            TreeNode ChildNode;
            foreach (DataRelation R in Relations)
            {
                InfoBindingSource IBS = new InfoBindingSource();

                //if ((Node == null) || (Node.Level == 0))
                //{
                IBS.DataSource = FInfoDataSet;
                IBS.DataMember = FInfoDataSet.RealDataSet.Tables[0].TableName;
                //}
                //else
                //{
                //    TDetailItem item1 = (TDetailItem)Node.Parent.Tag;
                //    IBS.DataSource = item1.BindingSource;
                //    IBS.DataMember = item1.Relation.RelationName;
                //}
                ChildNode = new TreeNode();

                String DataSetName = R.ChildTable.TableName;
                String ModuleName = FInfoDataSet.RemoteName.Substring(0, FInfoDataSet.RemoteName.IndexOf('.'));
                String SolutionName = System.IO.Path.GetFileNameWithoutExtension(FDTE2.Solution.FullName);
                String TableName = CliUtils.GetTableName(ModuleName, DataSetName, SolutionName);
                TableName = WzdUtils.GetToken(ref TableName, new char[] { '.' });

                ChildNode.Text = R.ChildTable.TableName;
                ChildNode.Name = R.ChildTable.TableName;
                Node.Nodes.Add(ChildNode);
                SetNodeData(R, IBS, ChildNode);
                ShowChildRelation(R.ChildTable.ChildRelations, ChildNode);
            }
        }

        private void ShowTable(InfoBindingSource aBindingSource, DataRelation Relation)
        {
            DataRelation R1;
            TreeNode Node;
            InfoBindingSource IBS;
            if (aBindingSource.DataSource.GetType().Equals(typeof(InfoDataSet)))
            {
                InfoDataSet set1 = (InfoDataSet)aBindingSource.DataSource;
                for (int I = 0; I < set1.RealDataSet.Tables[0].ChildRelations.Count; I++)
                {
                    R1 = set1.RealDataSet.Tables[0].ChildRelations[I];
                    Node = new TreeNode();

                    String DataSetName = R1.ChildTable.TableName;
                    String ModuleName = FInfoDataSet.RemoteName.Substring(0, FInfoDataSet.RemoteName.IndexOf('.'));
                    String SolutionName = System.IO.Path.GetFileNameWithoutExtension(FDTE2.Solution.FullName);
                    String TableName = CliUtils.GetTableName(ModuleName, DataSetName, SolutionName);
                    TableName = WzdUtils.GetToken(ref TableName, new char[] { '.' });

                    Node.Text = R1.ChildTable.TableName;
                    Node.Name = R1.ChildTable.TableName;
                    //Node.Text = TableName;
                    //Node.Name = TableName;
                    tvRelation.Nodes.Add(Node);
                    IBS = new InfoBindingSource();
                    IBS.DataSource = aBindingSource;
                    IBS.DataMember = R1.RelationName;
                    SetNodeData(R1, IBS, Node);
                    ShowChildRelation(R1.ChildTable.ChildRelations, Node);
                }
            }
            if (aBindingSource.DataSource.GetType().Equals(typeof(InfoBindingSource)))
            {
                while (!aBindingSource.DataSource.GetType().Equals(typeof(InfoDataSet)))
                {
                    aBindingSource = (InfoBindingSource)aBindingSource.DataSource;
                }
                InfoDataSet set2 = (InfoDataSet)aBindingSource.DataSource;
                for (int num2 = 0; num2 < set2.RealDataSet.Tables.Count; num2++)
                {
                    if (set2.RealDataSet.Tables[num2].TableName.Equals(Relation.ChildTable.TableName))
                    {
                        for (int num3 = 0; num3 < set2.RealDataSet.Tables[num2].ChildRelations.Count; num3++)
                        {
                            R1 = set2.RealDataSet.Tables[num2].ChildRelations[num3];
                            Node = new TreeNode();

                            String DataSetName = R1.ChildTable.TableName;
                            String ModuleName = FInfoDataSet.RemoteName.Substring(0, FInfoDataSet.RemoteName.IndexOf('.'));
                            String SolutionName = System.IO.Path.GetFileNameWithoutExtension(FDTE2.Solution.FullName);
                            String TableName = CliUtils.GetTableName(ModuleName, DataSetName, SolutionName);
                            TableName = WzdUtils.GetToken(ref TableName, new char[] { '.' });

                            Node.Text = TableName;
                            Node.Name = TableName;
                            tvRelation.Nodes.Add(Node);
                            IBS = new InfoBindingSource();
                            IBS.DataSource = aBindingSource;
                            IBS.DataMember = R1.RelationName;
                            SetNodeData(R1, IBS, Node);
                            ShowChildRelation(R1.ChildTable.ChildRelations, Node);
                        }
                    }
                }
            }
        }

        private void ShowTableRelations()
        {
            tvRelation.Nodes.Clear();
            InfoBindingSource IBS = new InfoBindingSource();
            DataRelation R = null;
            try
            {
                IBS.DataSource = FInfoDataSet;
                IBS.DataMember = FInfoDataSet.RealDataSet.Tables[0].TableName;
                ShowTable(IBS, R);
            }
            finally
            {
                IBS.Dispose();
            }
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

        private void button3_Click(object sender, EventArgs e)
        {
            string s = "C:\\Program Files\\Infolight\\EEP2006\\EEPNetServer\\WindowsApplication1\\SSingle.dll";
            string sDll = "SSingle";
            FileStream fs = null;
            fs = new FileStream(s, FileMode.Open, FileAccess.Read, FileShare.Read);
            byte[] b = new byte[fs.Length];
            fs.Read(b, 0, (int)fs.Length);

            // Add By Chenjian, Can not delete dll file if FileStream is not closed
            fs.Close();
            // End Add

            Assembly a = Assembly.Load(b);
            try
            {
                Type myType = a.GetType(sDll + ".Component", true, true);
                if (myType != null)
                    MessageBox.Show("get");
            }
            catch (Exception E)
            {
                MessageBox.Show(E.Message);
            }
        }

        private void SetNodeData(DataRelation Relation, InfoBindingSource BindingSource, TreeNode Node)
        {
            TDetailItem DetailItem = new TDetailItem();
            DetailItem.BindingSource = BindingSource;
            DetailItem.Relation = Relation;
            DetailItem.TableName = Relation.ChildTable.TableName;
            String ModuleName = tbProviderName.Text;
            ModuleName = ModuleName.Substring(0, ModuleName.IndexOf('.'));
            String SolutionName = System.IO.Path.GetFileNameWithoutExtension(FClientData.SolutionName);
            String S = CliUtils.GetTableName(ModuleName, DetailItem.TableName, SolutionName);
            //if (S.Contains("."))
            //    WzdUtils.GetToken(ref S, new char[] { '.' });
            DetailItem.RealTableName = WzdUtils.RemoveQuote(S, FClientData.DatabaseType);
            Node.Tag = DetailItem;
            tvRelation.SelectedNode = Node;
        }

        private void btnNewDataset_Click(object sender, EventArgs e)
        {
            InfoBindingSource IBS = new InfoBindingSource();
            TreeNode node1 = tvRelation.SelectedNode;
            if ((node1 == null) || (node1.Level == 0))
            {
                if (FInfoDataSet.RemoteName == "")
                {
                    FInfoDataSet.RemoteName = tbProviderName.Text;
                }
                IBS.DataSource = FInfoDataSet;
                IBS.DataMember = FInfoDataSet.RealDataSet.Tables[0].TableName;
            }
            else
            {
                TDetailItem item1 = (TDetailItem)node1.Parent.Tag;
                IBS.DataSource = item1.BindingSource;
                IBS.DataMember = item1.Relation.RelationName;
            }
            fmSelDetail detail1 = new fmSelDetail();
            DataRelation R = null;
            if (detail1.ShowSelDetail(IBS, ref R))
            {
                TreeNode Node = tvRelation.Nodes.Add(R.ChildTable.TableName);
                SetNodeData(R, IBS, Node);
                UpdatelvSelectedFields((TDetailItem)Node.Tag);
            }
        }

        private void UpdatelvSelectedFields(TDetailItem DetailItem)
        {
            lvSelectedFields.BeginUpdate();
            for (int i = 0; i < lvSelectedFields.Items.Count; i++)
            {
                ListViewItem.ListViewSubItem LVSI = lvSelectedFields.Items[i].SubItems[2];
                if (LVSI.Tag != null)
                    ((Button)LVSI.Tag).Dispose();
            }
            lvSelectedFields.Items.Clear();
            try
            {
                tbDetailTableName.Text = DetailItem.RealTableName;
                int I;
                TBlockFieldItem BlockFieldItem;
                ListViewItem ViewItem;
                for (I = 0; I < DetailItem.BlockFieldItems.Count; I++)
                {
                    BlockFieldItem = (TBlockFieldItem)DetailItem.BlockFieldItems[I];
                    ViewItem = lvSelectedFields.Items.Add(BlockFieldItem.DataField);
                    ViewItem.SubItems.Add(BlockFieldItem.Description);
                    ViewItem.Tag = BlockFieldItem;

                    if (lvSelectedFields.Columns.Count == 3 && ViewItem.SubItems.Count < 3)
                    {
                        ListViewItem.ListViewSubItem LVSI = ViewItem.SubItems.Add("");
                        Button B = new Button();
                        B.Parent = lvSelectedFields;
                        RearrangeRefValButton(B, LVSI.Bounds);
                        B.BackColor = Color.Silver;
                        B.BringToFront();
                        LVSI.Tag = B;
                        B.Tag = ViewItem;
                        ViewItem.Tag = BlockFieldItem;
                        B.Click += new EventHandler(btnRefVal_Click);
                        B.Text = "...";
                    }
                }
            }
            finally
            {
                lvSelectedFields.EndUpdate();
            }
        }

        private void btnNewField_Click(object sender, EventArgs e)
        {
            TreeNode Node = tvRelation.SelectedNode;
            if (Node != null)
            {
                TDetailItem DetailItem = (TDetailItem)Node.Tag;
                MWizard2015.fmSelTableField F = new fmSelTableField();
                if (F.ShowSelTableFieldForm(DetailItem, GetFieldNames, lvSelectedFields, InternalConnection, RearrangeRefValButton, btnRefVal_Click, FClientData.DatabaseType))
                {
                    btnDeleteField.Enabled = lvSelectedFields.Items.Count > 0;
                }
            }
        }

        public delegate void GetFieldNamesFunc(string DatabaseName, string TableName, String DataSetName, ListView SrcListView, ListView DestListView);

        public void GetFieldNames(string DatabaseName, string TableName, String DataSetName, ListView SrcListView, ListView DestListView)
        {
            TreeNode Node = tvRelation.SelectedNode;
            if (Node == null)
                return;
            InfoCommand aInfoCommand = new InfoCommand(FClientData.DatabaseType);
            aInfoCommand.Connection = InternalConnection;
            string OWNER = string.Empty, SS = TableName;
            if (SS.Contains("."))
            {
                OWNER = WzdUtils.GetToken(ref SS, new char[] { '.' });
                TableName = SS;
            }
            aInfoCommand.CommandText = "Select * from COLDEF where TABLE_NAME='" + TableName + "' OR TABLE_NAME='" + OWNER + "." + TableName + "'";
            IDbDataAdapter DA = DBUtils.CreateDbDataAdapter(aInfoCommand);
            DataSet dsColdef = new DataSet();
            WzdUtils.FillDataAdapter(FClientData.DatabaseType, DA, dsColdef, "COLDEF");

            int Index = FInfoDataSet.RealDataSet.Tables.IndexOf(WzdUtils.RemoveSpace(DataSetName));
            DataTable Table = FInfoDataSet.RealDataSet.Tables[Index];

            int I;
            ListViewItem ViewItem;
            for (I = 0; I < DestListView.Items.Count; I++)
            {
                ViewItem = DestListView.Items[I];
                if (Table.Columns.IndexOf(ViewItem.Text) < 0)
                {
                    if (ViewItem.Tag != null)
                        if (ViewItem.Tag.GetType().Equals(typeof(TBlockFieldItem)))
                        {
                            TBlockFieldItem B = (TBlockFieldItem)ViewItem.Tag;
                            B.Collection.Remove(B);
                        }
                }
            }

            SrcListView.Items.Clear();
            bool Found;
            int J;
            DataRow[] DRs = null;
            for (I = 0; I < Table.Columns.Count; I++)
            {
                Found = false;
                for (J = 0; J < DestListView.Items.Count; J++)
                {
                    ViewItem = DestListView.Items[J];
                    if (string.Compare(Table.Columns[I].ColumnName, ViewItem.Text, false) == 0)
                    {
                        Found = true;
                        break;
                    }
                }
                if (Found == false)
                {
                    TBlockFieldItem FieldItem = new TBlockFieldItem();
                    FieldItem.DataField = Table.Columns[I].ColumnName;
                    FieldItem.DataType = Table.Columns[I].DataType;
                    ViewItem = SrcListView.Items.Add(Table.Columns[I].ColumnName);
                    ViewItem.Tag = FieldItem;
                    DRs = dsColdef.Tables[0].Select("TABLE_NAME = '" + WzdUtils.RemoveQuote(TableName, FClientData.DatabaseType) + "' and FIELD_NAME = '" + Table.Columns[I].ColumnName + "'");
                    if (DRs.Length == 1)
                    {
                        FieldItem.Description = DRs[0]["CAPTION"].ToString();
                        FieldItem.CheckNull = DRs[0]["CHECK_NULL"].ToString();
                        FieldItem.DefaultValue = DRs[0]["DEFAULT_VALUE"].ToString();
                        FieldItem.IsKey = DRs[0]["IS_KEY"].ToString().ToUpper() == "Y";
                        FieldItem.ControlType = DRs[0]["NEEDBOX"].ToString().ToUpper();
                        FieldItem.EditMask = DRs[0]["EDITMASK"].ToString().ToUpper();
                        if (DRs[0]["FIELD_LENGTH"] != null && DRs[0]["FIELD_LENGTH"].ToString() != "")
                            FieldItem.Length = int.Parse(DRs[0]["FIELD_LENGTH"].ToString());
                    }
                    ViewItem.SubItems.Add(FieldItem.Description);
                }
            }
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
            tbConnectionString.Text = (string)FAlias.Objects(num1);
            FClientData.DatabaseName = this.cbEEPAlias.Text;
            switch (type)
            {
                case "1":
                    FClientData.DatabaseType = ClientType.ctMsSql; break;
                case "2":
                    FClientData.DatabaseType = ClientType.ctOleDB; break;
                case "3":
                    FClientData.DatabaseType = ClientType.ctOracle; break;
                case "4":
                    FClientData.DatabaseType = ClientType.ctODBC; break;
                case "5":
                    FClientData.DatabaseType = ClientType.ctMySql; break;
                case "6":
                    FClientData.DatabaseType = ClientType.ctInformix; break;
                case "7":
                    FClientData.DatabaseType = ClientType.ctSybase; break;
            }
            cbDatabaseType.SelectedIndex = (int)FClientData.DatabaseType;

            if (InternalConnection == null)
            {
                InternalConnection = WzdUtils.AllocateConnection(this.cbEEPAlias.Text, FClientData.DatabaseType, false);
                FClientData.ConnString = InternalConnection.ConnectionString;
            }
            else
            {
                if (InternalConnection.State == ConnectionState.Open)
                    InternalConnection.Close();
                InternalConnection = WzdUtils.AllocateConnection(this.cbEEPAlias.Text, FClientData.DatabaseType, false);
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
            FClientData.ResetDatabaseConnection();
        }

        private void btnNewSubDataset_Click(object sender, EventArgs e)
        {
            InfoBindingSource IBS = new InfoBindingSource();
            TreeNode Node = tvRelation.SelectedNode;
            if (Node != null)
            {
                TDetailItem DetailItem = (TDetailItem)Node.Tag;
                IBS.DataSource = DetailItem.BindingSource;
                fmSelDetail detail1 = new fmSelDetail();
                DataRelation R = DetailItem.Relation;
                if (detail1.ShowSelDetail(IBS, ref R))
                {
                    TreeNode Node1 = Node.Nodes.Add(R.ChildTable.TableName);
                    SetNodeData(R, IBS, Node1);
                    UpdatelvSelectedFields((TDetailItem)Node1.Tag);
                }
            }
        }

        private void btnDeleteField_Click(object sender, EventArgs e)
        {
            try
            {
                TreeNode Node = tvRelation.SelectedNode;
                TDetailItem DetailItem = null;
                if (Node != null)
                {
                    DetailItem = (TDetailItem)Node.Tag;
                }
                for (int I = lvSelectedFields.Items.Count - 1; I >= 0; I--)
                {
                    if (lvSelectedFields.Items[I].Selected)
                    {
                        ListViewItem item2 = lvSelectedFields.Items[I];
                        TBlockFieldItem item3 = (TBlockFieldItem)item2.Tag;
                        if (item2.SubItems.Count > 2)
                        {
                            ListViewItem.ListViewSubItem LVSI = item2.SubItems[2];
                            if (LVSI.Tag != null)
                                ((Button)LVSI.Tag).Dispose();
                        }
                        DetailItem.BlockFieldItems.Remove(item3);
                        lvSelectedFields.Items.Remove(item2);
                    }
                }

                //foreach (ListViewItem LVI in lvSelectedFields.Items)
                //{
                //    if (LVI.SubItems.Count > 2)
                //    {
                //        ListViewItem.ListViewSubItem LVSI = LVI.SubItems[2];
                //        if (LVSI.Tag != null)
                //            RearrangeRefValButton((Button)LVSI.Tag, LVSI.Bounds);
                //    }
                //}
            }
            catch (Exception exception1)
            {
                MessageBox.Show(exception1.Message);
            }
        }

        //ArrayList al;
        private void tvRelation_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode Node = tvRelation.SelectedNode;
            //btnNewSubDataset.Enabled = Node != null;
            //btnDeleteDataset.Enabled = btnNewSubDataset.Enabled;
            //btnNewField.Enabled = btnNewSubDataset.Enabled;
            //ArrayList temp = new ArrayList();
            //if (this.lvSelectedFields.Items.Count > 0)
            //{
            //    ListView lv = new ListView();
            //    for (int i = 0; i < lvSelectedFields.Items.Count; i++)
            //        lv.Items.Add((lvSelectedFields.Items[i].Clone() as ListViewItem));
            //    temp.Add(lv);

            //    for (int i = lvSelectedFields.Items.Count - 1; i >= 0; i--)
            //    {
            //        ListViewItem item2 = lvSelectedFields.Items[i];
            //        TBlockFieldItem item3 = (TBlockFieldItem)item2.Tag;
            //        if (item2.SubItems.Count > 2)
            //        {
            //            ListViewItem.ListViewSubItem LVSI = item2.SubItems[2];
            //            if (LVSI.Tag != null)
            //                ((Button)LVSI.Tag).Dispose();
            //        }
            //        ((TDetailItem)Node.Tag).BlockFieldItems.Remove(item3);
            //        lvSelectedFields.Items.Remove(item2);
            //    }
            //}
            UpdatelvSelectedFields((TDetailItem)Node.Tag);
            //if (al != null && al.Count > 0)
            //{
            //    lvSelectedFields.Items.Clear();
            //    for (int i = 0; i < (al[0] as ListView).Items.Count; i++)
            //    {
            //        lvSelectedFields.Items.Add((al[0] as ListView).Items[i].Clone() as ListViewItem);
            //        AddDestBlockFieldItem((al[0] as ListView), (al[0] as ListView).Items[i], (TBlockFieldItem)(al[0] as ListView).Items[i].Tag, (TDetailItem)Node.Tag);
            //    }
            //}
            //al = new ArrayList();
            //al = temp;
        }

        //private void AddDestBlockFieldItem(ListView lv, ListViewItem ViewItem, TBlockFieldItem SourceItem, TDetailItem DetailItem)
        //{
        //    TBlockFieldItem BlockFieldItem = new TBlockFieldItem();
        //    BlockFieldItem.DataField = SourceItem.DataField;
        //    BlockFieldItem.Description = SourceItem.Description;
        //    BlockFieldItem.DataType = SourceItem.DataType;
        //    BlockFieldItem.IsKey = SourceItem.IsKey;
        //    BlockFieldItem.CheckNull = SourceItem.CheckNull;
        //    BlockFieldItem.DefaultValue = SourceItem.DefaultValue;
        //    BlockFieldItem.Length = SourceItem.Length;
        //    BlockFieldItem.QueryMode = SourceItem.QueryMode;

        //    if (lv.Items[0].SubItems.Count == 3)
        //    {
        //        ListViewItem.ListViewSubItem LVSI = ViewItem.SubItems.Add("");
        //        Button B = new Button();
        //        B.Parent = lv;
        //        if ((MWizard.DsgnMenuForm.RearrangeRefValButtonFunc)RearrangeRefValButton != null)
        //            RearrangeRefValButton(B, LVSI.Bounds);
        //        B.BackColor = Color.Silver;
        //        B.BringToFront();
        //        LVSI.Tag = B;
        //        B.Tag = ViewItem;
        //        ViewItem.Tag = BlockFieldItem;
        //        B.Click += new EventHandler(btnRefVal_Click);
        //        B.Text = "...";
        //    }

        //    DetailItem.BlockFieldItems.Add(BlockFieldItem);
        //}

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

        private void button1_Click_1(object sender, EventArgs e)
        {
            string[] fSelectedList = new string[10];
            string strSelected = "";
            IGetValues aItem = (IGetValues)FInfoDataSet;
            PERemoteName form = new PERemoteName(aItem.GetValues("RemoteName"), strSelected);
            if (form.ShowDialog() == DialogResult.OK)
            {
                strSelected = form.RemoteName;
            }

            /*
            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)GetService(typeof(IWindowsFormsEditorService));
            IGetValues aItem = (IGetValues)FInfoDataSet;
            if (edSvc != null)
            {
                object value = "";
                RemoteNameSelector mySelector = new RemoteNameSelector(edSvc, aItem.GetValues("RemoteName"));
                string strValue = (string)value;
                if (mySelector.Execute(ref strValue)) value = strValue;
            }
             */
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            string[] fSelectedList = new string[10];
            string strSelected = "";
            IGetValues aItem = (IGetValues)FInfoDataSet;
            FProviderNameList = aItem.GetValues("RemoteName");
            PERemoteName form = new PERemoteName(FProviderNameList, strSelected);
            if (form.ShowDialog() == DialogResult.OK)
            {
                tbProviderName.Text = form.RemoteName;
            }
        }

        private String GetTableNameByCommandName(String PackageName, String CommandName)
        {
            if ((fmClientWzard._serverPath == null) || (fmClientWzard._serverPath.Length == 0))
            {
                fmClientWzard._serverPath = EEPRegistry.Server + "\\";
            }
            return "";
        }

        private int GetProviderIndex()
        {
            String FindName = "";
            int Result = -1;
            switch (FClientData.Language)
            {
                case "cs":
                    FindName = "View_";
                    break;
                case "vb":
                    FindName = "_View_";
                    break;
            }

            for (int I = 0; I < cbViewProviderName.Items.Count; I++)
            {
                if (cbViewProviderName.Items[I].ToString().IndexOf(FindName) > -1)
                {
                    Result = I;
                    break;
                }
            }
            return Result;
        }

        private void tbProviderName_TextChanged(object sender, EventArgs e)
        {
            string ProviderName = tbProviderName.Text;
            if (ProviderName.Trim() == "")
                return;

            ClearAll();

            if (FInfoDataSet != null && FInfoDataSet.Active)
            {
                FInfoDataSet.Active = false;
                FInfoDataSet.Dispose();
                FInfoDataSet = null;
                FInfoDataSet = new InfoDataSet();
                FInfoDataSet.SetWizardDesignMode(true);
            }
            FInfoDataSet.RemoteName = ProviderName;
            //FInfoDataSet.ClearWhere();
            FInfoDataSet.AlwaysClose = true;
            FInfoDataSet.SetWhere("1=0");
            FInfoDataSet.Active = true;

            String DataSetName = FInfoDataSet.RealDataSet.Tables[0].TableName;
            String ModuleName = FInfoDataSet.RemoteName.Substring(0, FInfoDataSet.RemoteName.IndexOf('.'));
            String SolutionName = System.IO.Path.GetFileNameWithoutExtension(FDTE2.Solution.FullName);
            String S = CliUtils.GetTableName(ModuleName, DataSetName, SolutionName);
            //if (S.Contains("."))
            //    WzdUtils.GetToken(ref S, new char[] { '.' });
            tbTableName.Text = WzdUtils.RemoveQuote(S, FClientData.DatabaseType);

            cbViewProviderName.Items.Clear();
            string DllName = tbProviderName.Text;
            int Index = DllName.IndexOf('.');
            DllName = DllName.Substring(0, Index + 1);
            for (int I = 0; I < FProviderNameList.Length; I++)
            {
                if (FProviderNameList[I].ToString().IndexOf(DllName) > -1)
                {
                    cbViewProviderName.Items.Add(FProviderNameList[I]);
                }
            }

            cbViewProviderName.SelectedIndex = GetProviderIndex();
            FClientData.ViewProviderName = cbViewProviderName.Text;
            ShowTableRelations();
        }

        private void lvMasterDesField_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            e.DrawDefault = true;
        }

        private void lvMasterDesField_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            e.DrawBackground();
            e.DrawText();
            e.DrawFocusRectangle();
            ListViewItem.ListViewSubItem LVSI = e.Item.SubItems[3];

            if (LVSI != null)
            {
                Rectangle cellBounds = LVSI.Bounds;
                Rectangle rect = cellBounds;
                Point buttonLocation = new Point();
                buttonLocation.Y = cellBounds.Y + 1;
                buttonLocation.X = cellBounds.X + 1;
                Rectangle ButtonBounds = new Rectangle();

                ButtonBounds.X = cellBounds.X + 1;
                ButtonBounds.Y = cellBounds.Y + 1;
                ButtonBounds.Width = cellBounds.Width - 2;
                ButtonBounds.Height = cellBounds.Height - 3;

                Pen pen1 = new Pen(Brushes.White, 1);
                Pen pen2 = new Pen(Brushes.Black, 1);
                Pen pen3 = new Pen(Brushes.DimGray, 1);

                SolidBrush myBrush = new SolidBrush(SystemColors.Control);

                e.Graphics.FillRectangle(myBrush, ButtonBounds.X, ButtonBounds.Y, ButtonBounds.Width, ButtonBounds.Height);
                e.Graphics.DrawLine(pen2, ButtonBounds.X, ButtonBounds.Y + ButtonBounds.Height, ButtonBounds.X + ButtonBounds.Width, ButtonBounds.Y + ButtonBounds.Height);
                e.Graphics.DrawLine(pen2, ButtonBounds.X + ButtonBounds.Width, ButtonBounds.Y, ButtonBounds.X + ButtonBounds.Width, ButtonBounds.Y + ButtonBounds.Height);

                StringFormat sf1 = new StringFormat();
                sf1.Alignment = StringAlignment.Center;
                sf1.LineAlignment = StringAlignment.Center;
                e.Graphics.DrawString("...", new Font("SimSun", 5), Brushes.Black, ButtonBounds.X + 10, ButtonBounds.Y + 5, sf1);
            }
        }

        public delegate void RearrangeRefValButtonFunc(Button B, Rectangle Bounds);

        private void RearrangeRefValButton(Button B, Rectangle Bounds)
        {
            Rectangle NewBounds = new Rectangle();
            if (Bounds.Width > 20)
            {
                NewBounds.X = Bounds.X + Bounds.Width - 20;
                NewBounds.Width = 20;
            }
            else
            {
                NewBounds.X = Bounds.X;
                NewBounds.Width = Bounds.Width;
            }
            NewBounds.Y = Bounds.Y - 1;
            NewBounds.Height = Bounds.Height - 2;
            B.Bounds = NewBounds;
        }

        private void lvMasterDesField_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
        {
            //ListView LV = (ListView)sender;
            //foreach (ListViewItem LVI in LV.Items)
            //{
            //    ListViewItem.ListViewSubItem LVSI = LVI.SubItems[2];
            //    if (LVSI.Tag != null)
            //    {
            //        Button B = (Button)LVSI.Tag;
            //        RearrangeRefValButton(B, LVSI.Bounds);
            //    }
            //}
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void cbViewProviderName_SelectedIndexChanged(object sender, EventArgs e)
        {
            FClientData.ViewProviderName = cbViewProviderName.Text;
        }

        private void label18_Click(object sender, EventArgs e)
        {

        }

        private void tbTableName_TextChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void tpDataSource_Click(object sender, EventArgs e)
        {

        }

        private void btnAssemblyOutputPath_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                tbAssemblyOutputPath.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void tbNewSolutionName_TextChanged(object sender, EventArgs e)
        {
            tbAssemblyOutputPath.Text = String.Format(@"..\..\EEPNetClient\{0}", tbNewSolutionName.Text);
        }

        private void tbFormName_TextChanged(object sender, EventArgs e)
        {
            tbFormText.Text = tbFormName.Text;
        }

        private void cbDatabaseType_TextChanged(object sender, EventArgs e)
        {
            FClientData.ViewProviderName = cbViewProviderName.Text;
        }

        private void lvViewSrcField_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            ListViewColumnSorter lvwColumnSorter = (sender as ListView).ListViewItemSorter as ListViewColumnSorter;

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

            switch ((sender as ListView).Name)
            {
                case "lvViewDesField":
                    btnViewUp.Enabled = false;
                    btnViewDown.Enabled = false;
                    break;
                case "lvMasterDesField":
                    btnMasterUp.Enabled = false;
                    btnMasterDown.Enabled = false;
                    break;
                case "lvSelectedFields":
                    btnDetailUp.Enabled = false;
                    btnDetailDown.Enabled = false;
                    break;
            }
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            WzdUtils.SelectedListViewItemUp(lvViewDesField);
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            WzdUtils.SelectedListViewItemDown(lvViewDesField);
        }

        private void btnMasterUp_Click(object sender, EventArgs e)
        {
            WzdUtils.SelectedListViewItemUp(lvMasterDesField);
        }

        private void btnMasterDown_Click(object sender, EventArgs e)
        {
            WzdUtils.SelectedListViewItemDown(lvMasterDesField);
        }

        private void btnDetailUp_Click(object sender, EventArgs e)
        {
            WzdUtils.SelectedListViewItemUp(lvSelectedFields);
        }

        private void btnDetailDown_Click(object sender, EventArgs e)
        {
            WzdUtils.SelectedListViewItemDown(lvSelectedFields);
        }

        private ListView FListView;
        private TBlockFieldItem FSelectedBlockFieldItem;
        private ListViewItem FSelectedListViewItem;
        private Boolean FDisplayValue = false;
        private TWizardType FWizardType;
        private ListViewColumnSorter lvwColumnSorter;
        private void lvMasterDesField_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvMasterDesField.SelectedItems.Count == 1)
            {
                if (!FDisplayValue)
                    SetValue();

                ListViewItem aViewItem = lvMasterDesField.SelectedItems[0];
                FSelectedListViewItem = aViewItem;
                FSelectedBlockFieldItem = (TBlockFieldItem)aViewItem.Tag;
                FDisplayValue = true;
                DisplayValue();
                FDisplayValue = false;
            }
        }

        private void SetValue()
        {
            if (FSelectedBlockFieldItem == null)
                return;
            FSelectedBlockFieldItem.Description = tbCaption.Text;
            FSelectedBlockFieldItem.CheckNull = cbCheckNull.Text;
            FSelectedBlockFieldItem.DefaultValue = tbDefaultValue.Text;
            FSelectedBlockFieldItem.QueryMode = cbQueryMode.Text;
            FSelectedBlockFieldItem.EditMask = tbEditMask.Text;
            FSelectedBlockFieldItem.ControlType = cbControlType.Text;
            FSelectedBlockFieldItem.ComboRemoteName = cbComboTableName.Text;
            FSelectedBlockFieldItem.ComboTextField = cbDataTextField.Text;
            FSelectedBlockFieldItem.ComboValueField = cbDataValueField.Text;
            FSelectedBlockFieldItem.RefValNo = cbRefValNo.Text;
        }

        private void DisplayValue()
        {
            if (FSelectedBlockFieldItem == null)
                return;
            tbCaption.Text = FSelectedBlockFieldItem.Description;
            cbCheckNull.Text = FSelectedBlockFieldItem.CheckNull;
            tbDefaultValue.Text = FSelectedBlockFieldItem.DefaultValue;
            if (FSelectedBlockFieldItem.QueryMode == null || FSelectedBlockFieldItem.QueryMode == "")
                cbQueryMode.Text = "None";
            else
                cbQueryMode.Text = FSelectedBlockFieldItem.QueryMode;
            tbEditMask.Text = FSelectedBlockFieldItem.EditMask;
            if (FSelectedBlockFieldItem.ControlType == "" || FSelectedBlockFieldItem.ControlType == null)
                cbControlType.Text = "TextBox";
            else
                cbControlType.Text = FSelectedBlockFieldItem.ControlType;
            cbComboTableName.Text = FSelectedBlockFieldItem.ComboRemoteName;
            cbDataTextField.Text = FSelectedBlockFieldItem.ComboTextField;
            cbDataValueField.Text = FSelectedBlockFieldItem.ComboValueField;
            if (FSelectedBlockFieldItem.RefValNo == "" || FSelectedBlockFieldItem.RefValNo == null)
                cbRefValNo.SelectedIndex = -1;
            else
                cbRefValNo.Text = FSelectedBlockFieldItem.RefValNo;
        }

        private void cbControlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((sender as ComboBox).SelectedItem.ToString() == "ComboBox")
            {
                cbComboTableName.Enabled = true;
                cbDataValueField.Enabled = true;
                cbDataTextField.Enabled = true;
                cbRefValNo.Enabled = false;
                btnRefValNo.Enabled = false;

                if (cbComboTableName.Items.Count == 0)
                {
                    cbComboTableName.Items.Add("");

                    if (FClientData.DatabaseType != ClientType.ctInformix)
                    {
                        String[] Params = null;
                        String ViewFieldName = "TABLE_NAME";
                        if (FClientData.DatabaseType == ClientType.ctOracle)
                        {
                            String UserID = WzdUtils.GetFieldParam(InternalConnection.ConnectionString.ToLower(), "user id");
                            Params = new String[] { UserID.ToUpper() };
                            ViewFieldName = "VIEW_NAME";
                        }
                        DataTable T = InternalConnection.GetSchema("Tables", Params);
                        SortedList<String, String> sTable = new SortedList<String, String>();
                        foreach (DataRow DR in T.Rows)
                        {
                            sTable.Add(DR["TABLE_NAME"].ToString(), DR["TABLE_NAME"].ToString());
                        }

                        DataTable D1 = InternalConnection.GetSchema("Views", Params);
                        foreach (DataRow DR in D1.Rows)
                        {
                            if (!sTable.ContainsKey(DR[ViewFieldName].ToString()))
                                sTable.Add(DR[ViewFieldName].ToString(), DR[ViewFieldName].ToString());
                        }

                        foreach (var item in sTable)
                            cbComboTableName.Items.Add(item.Key);
                    }
                    else
                    {
                        List<String> allTables = WzdUtils.GetAllTablesList(InternalConnection, ClientType.ctInformix);
                        allTables.Sort();
                        foreach (String str in allTables)
                            cbComboTableName.Items.Add(str);
                    }
                }

                if (cbRefValNo.Items.Count == 0)
                {
                    InfoCommand FInfoCommand = new InfoCommand(FClientData.DatabaseType);
                    FInfoCommand.Connection = WzdUtils.AllocateConnection(FClientData.DatabaseName, FClientData.DatabaseType, true);
                    FInfoCommand.CommandText = "Select REFVAL_NO from SYS_REFVAL";
                    IDbDataAdapter DA = DBUtils.CreateDbDataAdapter(FInfoCommand);
                    DataSet aDataSet = new DataSet();
                    WzdUtils.FillDataAdapter(FClientData.DatabaseType, DA, aDataSet, "SYS_REFVAL");
                    DataTable aDataTable = aDataSet.Tables[0];
                    foreach (DataRow DR in aDataTable.Rows)
                    {
                        cbRefValNo.Items.Add(DR["REFVAL_NO"].ToString());
                    }
                }
            }
            else if ((sender as ComboBox).SelectedItem.ToString() == "RefValBox")
            {
                cbComboTableName.Enabled = false;
                cbDataValueField.Enabled = false;
                cbDataTextField.Enabled = false;
                cbRefValNo.Enabled = true;
                btnRefValNo.Enabled = true;
                if (cbRefValNo.Items.Count == 0)
                {
                    InfoCommand FInfoCommand = new InfoCommand(FClientData.DatabaseType);
                    FInfoCommand.Connection = WzdUtils.AllocateConnection(FClientData.DatabaseName, FClientData.DatabaseType, true);
                    FInfoCommand.CommandText = "Select REFVAL_NO from SYS_REFVAL";
                    IDbDataAdapter DA = DBUtils.CreateDbDataAdapter(FInfoCommand);
                    DataSet aDataSet = new DataSet();
                    WzdUtils.FillDataAdapter(FClientData.DatabaseType, DA, aDataSet, "SYS_REFVAL");
                    DataTable aDataTable = aDataSet.Tables[0];
                    foreach (DataRow DR in aDataTable.Rows)
                    {
                        cbRefValNo.Items.Add(DR["REFVAL_NO"].ToString());
                    }
                }
            }
            else
            {
                cbComboTableName.Enabled = false;
                cbDataValueField.Enabled = false;
                cbDataTextField.Enabled = false;
                cbRefValNo.Enabled = false;
                btnRefValNo.Enabled = false;
                btnRefValNo.Text = String.Empty;
            }
        }

        private void cbComboTableName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbComboTableName.Text == "")
                return;
            cbDataTextField.Items.Clear();
            cbDataValueField.Items.Clear();
            InfoCommand aInfoCommand = new InfoCommand(FClientData.DatabaseType);
            aInfoCommand.Connection = InternalConnection;
            aInfoCommand.CommandText = String.Format("Select * from {0} where 1=0", cbComboTableName.Text);
            IDbDataAdapter DA = DBUtils.CreateDbDataAdapter(aInfoCommand);
            DataSet aDataSet = new DataSet();
            try
            {
                WzdUtils.FillDataAdapter(FClientData.DatabaseType, DA, aDataSet, cbComboTableName.Text);

                foreach (DataColumn DC in aDataSet.Tables[0].Columns)
                {
                    cbDataTextField.Items.Add(DC.ColumnName);
                    cbDataValueField.Items.Add(DC.ColumnName);
                }
                cbDataTextField.Items.Add("");
                cbDataValueField.Items.Add("");
            }
            catch
            {
                MessageBox.Show(cbComboTableName.Text + " is a illegal table.");
                cbComboTableName.Text = String.Empty;
            }
        }

        private void btnRefValNo_Click(object sender, EventArgs e)
        {
            fmRefVal aForm = new fmRefVal(InternalConnection, FClientData.DatabaseType, FClientData.DatabaseName);
            String RefValNo = aForm.ShowRefValForm();
            cbRefValNo.Text = RefValNo;
        }

        private ListView FListView_D;
        private TBlockFieldItem FSelectedBlockFieldItem_D;
        private ListViewItem FSelectedListViewItem_D;
        private Boolean FDisplayValue_D = false;
        private TWizardType FWizardType_D;
        private void lvSelectedFields_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvSelectedFields.SelectedItems.Count == 1)
            {
                if (!FDisplayValue_D)
                    SetValue_D();

                ListViewItem aViewItem = lvSelectedFields.SelectedItems[0];
                FSelectedListViewItem_D = aViewItem;
                FSelectedBlockFieldItem_D = (TBlockFieldItem)aViewItem.Tag;
                FDisplayValue_D = true;
                DisplayValue_D();
                FDisplayValue_D = false;
            }
        }

        private void DisplayValue_D()
        {
            if (FSelectedBlockFieldItem_D == null)
                return;
            tbCaption_D.Text = FSelectedBlockFieldItem_D.Description;
            cbCheckNull_D.Text = FSelectedBlockFieldItem_D.CheckNull;
            tbDefaultValue_D.Text = FSelectedBlockFieldItem_D.DefaultValue;
            if (FSelectedBlockFieldItem_D.QueryMode == null || FSelectedBlockFieldItem_D.QueryMode == "")
                cbQueryMode_D.Text = "None";
            else
                cbQueryMode_D.Text = FSelectedBlockFieldItem_D.QueryMode;
            tbEditMask_D.Text = FSelectedBlockFieldItem_D.EditMask;
            if (FSelectedBlockFieldItem_D.ControlType == "" || FSelectedBlockFieldItem_D.ControlType == null)
                cbControlType_D.Text = "TextBox";
            else
                cbControlType_D.Text = FSelectedBlockFieldItem_D.ControlType;
            cbComboTableName_D.Text = FSelectedBlockFieldItem_D.ComboRemoteName;
            cbComboDisplayField_D.Text = FSelectedBlockFieldItem_D.ComboTextField;
            cbComboValueField_D.Text = FSelectedBlockFieldItem_D.ComboValueField;
            if (FSelectedBlockFieldItem_D.RefValNo == "" || FSelectedBlockFieldItem_D.RefValNo == null)
                cbRefValNo_D.SelectedIndex = -1;
            else
                cbRefValNo_D.Text = FSelectedBlockFieldItem_D.RefValNo;
        }

        private void SetValue_D()
        {
            if (FSelectedBlockFieldItem_D == null)
                return;
            FSelectedBlockFieldItem_D.Description = tbCaption_D.Text;
            FSelectedBlockFieldItem_D.CheckNull = cbCheckNull_D.Text;
            FSelectedBlockFieldItem_D.DefaultValue = tbDefaultValue_D.Text;
            FSelectedBlockFieldItem_D.ControlType = cbControlType_D.Text;
            FSelectedBlockFieldItem_D.ComboRemoteName = cbComboTableName_D.Text;
            FSelectedBlockFieldItem_D.ComboTextField = cbComboDisplayField_D.Text;
            FSelectedBlockFieldItem_D.ComboValueField = cbComboValueField_D.Text;
            FSelectedBlockFieldItem_D.RefValNo = cbRefValNo_D.Text;
        }

        private void cbControlType_D_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((sender as ComboBox).SelectedItem.ToString() == "ComboBox")
            {
                cbComboTableName_D.Enabled = true;
                cbComboValueField_D.Enabled = true;
                cbComboDisplayField_D.Enabled = true;
                cbRefValNo_D.Enabled = false;
                btnRefValNo_D.Enabled = false;

                if (cbComboTableName_D.Items.Count == 0)
                {
                    cbComboTableName_D.Items.Add("");

                    if (FClientData.DatabaseType != ClientType.ctInformix)
                    {
                        String[] Params = null;
                        String ViewFieldName = "TABLE_NAME";
                        if (FClientData.DatabaseType == ClientType.ctOracle)
                        {
                            String UserID = WzdUtils.GetFieldParam(InternalConnection.ConnectionString.ToLower(), "user id");
                            Params = new String[] { UserID.ToUpper() };
                            ViewFieldName = "VIEW_NAME";
                        }
                        DataTable T = InternalConnection.GetSchema("Tables", Params);
                        SortedList<String, String> sTable = new SortedList<String, String>();
                        foreach (DataRow DR in T.Rows)
                        {
                            sTable.Add(DR["TABLE_NAME"].ToString(), DR["TABLE_NAME"].ToString());
                        }

                        DataTable D1 = InternalConnection.GetSchema("Views", Params);
                        foreach (DataRow DR in D1.Rows)
                        {
                            if (!sTable.ContainsKey(DR[ViewFieldName].ToString()))
                                sTable.Add(DR[ViewFieldName].ToString(), DR[ViewFieldName].ToString());
                        }

                        foreach (var item in sTable)
                            cbComboTableName_D.Items.Add(item.Key);
                    }
                    else
                    {
                        List<String> allTables = WzdUtils.GetAllTablesList(InternalConnection, ClientType.ctInformix);
                        allTables.Sort();
                        foreach (String str in allTables)
                            cbComboTableName_D.Items.Add(str);
                    }
                }

                if (cbRefValNo_D.Items.Count == 0)
                {
                    InfoCommand FInfoCommand = new InfoCommand(FClientData.DatabaseType);
                    FInfoCommand.Connection = WzdUtils.AllocateConnection(FClientData.DatabaseName, FClientData.DatabaseType, true);
                    FInfoCommand.CommandText = "Select REFVAL_NO from SYS_REFVAL";
                    IDbDataAdapter DA = DBUtils.CreateDbDataAdapter(FInfoCommand);
                    DataSet aDataSet = new DataSet();
                    WzdUtils.FillDataAdapter(FClientData.DatabaseType, DA, aDataSet, "SYS_REFVAL");
                    DataTable aDataTable = aDataSet.Tables[0];
                    foreach (DataRow DR in aDataTable.Rows)
                    {
                        cbRefValNo_D.Items.Add(DR["REFVAL_NO"].ToString());
                    }
                }
            }
            else if ((sender as ComboBox).SelectedItem.ToString() == "RefValBox")
            {
                cbComboTableName_D.Enabled = false;
                cbComboValueField_D.Enabled = false;
                cbComboDisplayField_D.Enabled = false;
                cbRefValNo_D.Enabled = true;
                btnRefValNo_D.Enabled = true;
                if (cbRefValNo_D.Items.Count == 0)
                {
                    InfoCommand FInfoCommand = new InfoCommand(FClientData.DatabaseType);
                    FInfoCommand.Connection = WzdUtils.AllocateConnection(FClientData.DatabaseName, FClientData.DatabaseType, true);
                    FInfoCommand.CommandText = "Select REFVAL_NO from SYS_REFVAL";
                    IDbDataAdapter DA = DBUtils.CreateDbDataAdapter(FInfoCommand);
                    DataSet aDataSet = new DataSet();
                    WzdUtils.FillDataAdapter(FClientData.DatabaseType, DA, aDataSet, "SYS_REFVAL");
                    DataTable aDataTable = aDataSet.Tables[0];
                    foreach (DataRow DR in aDataTable.Rows)
                    {
                        cbRefValNo_D.Items.Add(DR["REFVAL_NO"].ToString());
                    }
                }
            }
            else
            {
                cbComboTableName_D.Enabled = false;
                cbComboValueField_D.Enabled = false;
                cbComboDisplayField_D.Enabled = false;
                cbRefValNo_D.Enabled = false;
                btnRefValNo_D.Enabled = false;
                btnRefValNo_D.Text = String.Empty;
            }
        }

        private void cbComboTableName_D_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbComboTableName_D.Text == "")
                return;
            cbComboDisplayField_D.Items.Clear();
            cbComboValueField_D.Items.Clear();
            InfoCommand aInfoCommand = new InfoCommand(FClientData.DatabaseType);
            aInfoCommand.Connection = InternalConnection;
            aInfoCommand.CommandText = String.Format("Select * from {0} where 1=0", cbComboTableName_D.Text);
            IDbDataAdapter DA = DBUtils.CreateDbDataAdapter(aInfoCommand);
            DataSet aDataSet = new DataSet();
            try
            {
                WzdUtils.FillDataAdapter(FClientData.DatabaseType, DA, aDataSet, cbComboTableName_D.Text);

                foreach (DataColumn DC in aDataSet.Tables[0].Columns)
                {
                    cbComboDisplayField_D.Items.Add(DC.ColumnName);
                    cbComboValueField_D.Items.Add(DC.ColumnName);
                }
                cbComboDisplayField_D.Items.Add("");
                cbComboValueField_D.Items.Add("");
            }
            catch
            {
                MessageBox.Show(cbComboTableName_D.Text + " is a illegal table.");
                cbComboTableName_D.Text = String.Empty;
            }
        }

        private void btnRefValNo_D_Click(object sender, EventArgs e)
        {
            fmRefVal aForm = new fmRefVal(InternalConnection, FClientData.DatabaseType, FClientData.DatabaseName);
            String RefValNo = aForm.ShowRefValForm();
            cbRefValNo_D.Text = RefValNo;
        }
    }

    public class TRefField : Object
    {
        public TRefField()
        {
            FLookupColumns = new RefColumnsCollection(this, typeof(RefColumns));
        }

        private String FSelectCommand;
        public String SelectCommand
        {
            get { return FSelectCommand; }
            set { FSelectCommand = value; }
        }

        private String FValueMember;
        public String ValueMember
        {
            get { return FValueMember; }
            set { FValueMember = value; }
        }

        private String FDisplayMember;
        public String DisplayMember
        {
            get { return FDisplayMember; }
            set { FDisplayMember = value; }
        }

        private RefColumnsCollection FLookupColumns;
        public RefColumnsCollection LookupColumns
        {
            get { return FLookupColumns; }
            set { FLookupColumns = value; }
        }
    }

    public class TBlockFieldItem : TCollectionItem
    {
        private string FDataField, FDescription;
        private int FLength;
        private bool FIsKey;
        private String FRefValNo;
        private Type FDataType;
        private String FCheckNull;
        private String FDefaultValue;
        private String FControlType;
        private bool FHideGridColumn;
        private String FComboRemoteName;
        private String FComboEntityName;
        private String FComboEntitySetName;
        private String FComboTextField;
        private String FComboValueField;
        private String FComboTextFieldCaption;
        private String FComboValueFieldCaption;
        private List<OtherField> FComboOtherFields;
        private String FQueryMode;
        private TRefField FRefField;
        private String FEditMask;
        private Boolean FIsInfoCommand = false;

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

        public String RefValNo
        {
            get { return FRefValNo; }
            set { FRefValNo = value; }
        }

        public int Length
        {
            get
            {
                return FLength;
            }
            set
            {
                FLength = value;
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

        public Type DataType
        {
            get { return FDataType; }
            set { FDataType = value; }
        }

        public String CheckNull
        {
            get { return FCheckNull; }
            set { FCheckNull = value; }
        }

        public string DefaultValue
        {
            get { return FDefaultValue; }
            set { FDefaultValue = value; }
        }

        public String ControlType
        {
            get { return FControlType; }
            set { FControlType = value; }
        }

        public bool HideGridColumn
        {
            get { return FHideGridColumn; }
            set { FHideGridColumn = value; }
        }

        public String ComboRemoteName
        {
            get { return FComboRemoteName; }
            set { FComboRemoteName = value; }
        }

        public String ComboEntityName
        {
            get { return FComboEntityName; }
            set { FComboEntityName = value; }
        }

        public String ComboEntitySetName
        {
            get { return FComboEntitySetName; }
            set { FComboEntitySetName = value; }
        }

        public String ComboTextField
        {
            get { return FComboTextField; }
            set { FComboTextField = value; }
        }

        public String ComboValueField
        {
            get { return FComboValueField; }
            set { FComboValueField = value; }
        }

        public String ComboTextFieldCaption
        {
            get { return FComboTextFieldCaption; }
            set { FComboTextFieldCaption = value; }
        }

        public String ComboValueFieldCaption
        {
            get { return FComboValueFieldCaption; }
            set { FComboValueFieldCaption = value; }
        }

        public String QueryMode
        {
            get { return FQueryMode; }
            set { FQueryMode = value; }
        }

        public void CopyTo(Array array, int index)
        {
            base.InnerList.CopyTo(array, index);
        }

        public TRefField RefField
        {
            get { return FRefField; }
            set { FRefField = value; }
        }

        public String EditMask
        {
            get { return FEditMask; }
            set { FEditMask = value; }
        }

        public Boolean IsInfoCommand
        {
            get { return FIsInfoCommand; }
            set { FIsInfoCommand = value; }
        }

        public List<OtherField> ComboOtherFields
        {
            get { return FComboOtherFields; }
            set { FComboOtherFields = value; }
        }
    }

    public class OtherField
    {
        private String _FieldName;
        private String _FieldCaption;

        public String FieldName
        {
            set { _FieldName = value; }
            get { return _FieldName; }
        }

        public String FieldCaption
        {
            set { _FieldCaption = value; }
            get { return _FieldCaption; }
        }
    }

    public class TBlockFieldItems : TCollection
    {
        public TBlockFieldItems(object Owner)
        {
            base.Owner = Owner;
        }

        public TBlockFieldItem FindItem(string DataField)
        {
            TBlockFieldItem item1 = null;
            for (int num1 = 0; num1 < Count; num1++)
            {
                item1 = base[num1] as TBlockFieldItem;
                if (string.Compare(item1.DataField, DataField) == 0)
                {
                    break;
                }
                item1 = null;
            }
            return item1;

        }

        /*
        public Int32 Add(object value)
        {
            int index = base.InnerList.Add(value);
            return index;
        }
        */
    }

    public class TBlockItem : TCollectionItem //System.Collections.CollectionBase, IList, ICollection
    {
        private String FName, FTableName, FProviderName, FRelationName, FParentItemName = "";
        private TBlockFieldItems FBlockFieldItems;
        private InfoBindingSource FBindingSource;
        private InfoBindingSource FViewBindingSource;
        private WebDataSource FWebDataSource;
        private String FContainerName;
        private Control FContainerControl;
        private TLayoutType FLayoutType;
        private object FWebContainerControl;
        private System.Web.UI.Page FPageContainerControl;
        private DataRelation FRelation;

        private void SetBlockFieldItems(TBlockFieldItems aBlockFieldItems)
        {
        }

        private void SetName(string AName)
        {
        }

        public TBlockItem()
        {
            FBlockFieldItems = new TBlockFieldItems(this);
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

        public TBlockFieldItems BlockFieldItems
        {
            get
            {
                return FBlockFieldItems;
            }
            set
            {
                FBlockFieldItems = value;
            }
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

        public string ProviderName
        {
            get
            {
                return FProviderName;
            }
            set
            {
                FProviderName = value;
            }
        }

        public DataRelation Relation
        {
            get
            {
                return FRelation;
            }
            set
            {
                FRelation = value;
            }
        }

        public string RelationName
        {
            get
            {
                return FRelationName;
            }
            set
            {
                FRelationName = value;
            }
        }

        public string ParentItemName
        {
            get
            {
                return FParentItemName;
            }
            set
            {
                FParentItemName = value;
            }
        }

        public InfoBindingSource BindingSource
        {
            get
            {
                return FBindingSource;
            }
            set
            {
                FBindingSource = value;
            }
        }

        public InfoBindingSource ViewBindingSource
        {
            get
            {
                return FViewBindingSource;
            }
            set
            {
                FViewBindingSource = value;
            }
        }

        public WebDataSource wDataSource
        {
            get
            {
                return FWebDataSource;
            }
            set
            {
                FWebDataSource = value;
            }
        }

        public String ContainerName
        {
            get { return FContainerName; }
            set { FContainerName = value; }
        }

        public Control ContainerControl
        {
            get { return FContainerControl; }
            set
            {
                FContainerControl = value;
                ContainerName = "";
                if (FContainerControl != null)
                    ContainerName = FContainerControl.Name;
            }
        }

        public Object WebContainerControl
        {
            get { return FWebContainerControl; }
            set
            {
                FWebContainerControl = value;
                ContainerName = "";
                if (FWebContainerControl != null)
                {
                    PropertyInfo aInfo = FWebContainerControl.GetType().GetProperty("ID");
                    if (aInfo != null)
                    {
                        ContainerName = aInfo.GetValue(FWebContainerControl, null) as String;
                    }
                }
            }
        }

        public System.Web.UI.Page PageContainerControl
        {
            get { return FPageContainerControl; }
            set
            {
                FPageContainerControl = value;
                ContainerName = "";
                if (FPageContainerControl != null)
                    ContainerName = FPageContainerControl.ID;
            }
        }

        public TLayoutType LayoutKind
        {
            get { return FLayoutType; }
            set { FLayoutType = value; }
        }

        public Int32 Add(object value)
        {
            int index = base.InnerList.Add(value);
            return index;
        }
    }

    public class TBlockItems : TCollection//System.Collections.CollectionBase, ICollection, IList
    {
        public TBlockItems(object Owner)
        {
            base.Owner = Owner;
        }

        private string FindUniqueName(string Name)
        {
            int I = 0;
            string Result = Name;
            while (FindItem(Result) != null)
            {
                Result = Name + I.ToString();
                I++;
            }
            return Result;
        }

        private void ChangeBlockItemName(int Index, string NewItemName)
        {
            TBlockItem BlockItem;
            string OldItemName;

            BlockItem = FindItem(NewItemName);
            if (BlockItem != null)
            {
                throw new Exception("名稱已經存在：" + NewItemName);
            }
            BlockItem = (TBlockItem)InnerList[Index];
            OldItemName = BlockItem.Name;
        }

        public TBlockItem FindItem(string Name)
        {
            int I;
            TBlockItem Result = null;
            for (I = 0; I < Count; I++)
            {
                Result = (TBlockItem)InnerList[I];
                if (string.Compare(Name, Result.Name, true) == 0)
                {
                    break;
                }
                else
                {
                    Result = null;
                }
            }
            return Result;
        }

        /*
		public Int32 Add(object value)
		{
			int index = base.InnerList.Add(value);
			return index;
		}
		*/
    }

    public class TClientData : Object
    {
        private string FPackageName, FBaseFormName, FServerPackageName, FOutputPath, FTableName, FFormName, FProviderName,
            FDatabaseName, FSolutionName, FViewProviderName, FAssemblyOutputPath, FFormText;
        private TBlockItems FBlocks;
        private MWizard2015.fmClientWzard FOwner;
        private bool FNewSolution = false;
        private string FCodeOutputPath;
        private int FColumnCount;
        private ClientType FDatabaseType;
        private String FConnString;
        private String FLanguage = "cs";
        private String FLabelAliement;

        public TClientData(MWizard2015.fmClientWzard Owner)
        {
            FOwner = Owner;
            FBlocks = new TBlockItems(this);
        }

        public MWizard2015.fmClientWzard Owner
        {
            get { return FOwner; }
        }

        public ClientType DatabaseType
        {
            get { return FDatabaseType; }
            set { FDatabaseType = value; }
        }

        public String ConnString
        {
            get { return FConnString; }
            set { FConnString = value; }
        }

        public String Language
        {
            get { return FLanguage; }
            set { FLanguage = value; }
        }

        private void LoadBlockFieldItems(XmlNode Node, TBlockFieldItems BlockFieldItems)
        {
            TBlockFieldItem BFI;
            int I;
            XmlNode BlockFieldItemNode;
            for (I = 0; I < Node.ChildNodes.Count; I++)
            {
                BlockFieldItemNode = Node.ChildNodes[I];
                BFI = new TBlockFieldItem();
                BFI.DataField = BlockFieldItemNode.Attributes["DataField"].Value;
                BFI.Description = BlockFieldItemNode.Attributes["Description"].Value;
                BFI.Length = int.Parse(BlockFieldItemNode.Attributes["Length"].Value.ToString());
                BFI.EditMask = BlockFieldItemNode.Attributes["EditMaskType"].Value;
                foreach (XmlNode RefNode in BlockFieldItemNode.ChildNodes)
                {
                    BFI.RefField = new TRefField();
                    BFI.RefField.SelectCommand = RefNode.Attributes["SelectCommand"].Value;
                    BFI.RefField.ValueMember = RefNode.Attributes["ValueMember"].Value;
                    BFI.RefField.DisplayMember = RefNode.Attributes["DisplayMember"].Value;
                    foreach (XmlNode ColumnNode in RefNode.ChildNodes)
                    {
                        RefColumns aColumn = new RefColumns();
                        aColumn.Column = ColumnNode.Attributes["Column"].Value;
                        aColumn.HeaderText = ColumnNode.Attributes["HeaderText"].Value;
                        aColumn.Width = int.Parse(ColumnNode.Attributes["Width"].Value);
                        BFI.RefField.LookupColumns.Add(aColumn);
                    }
                }
                //IPC保留缺口
                //BlockFieldItem.CheckNull = DR["CHECK_NULL"].ToString();
                //BlockFieldItem.DefaultValue = DR["DEFAULT_VALUE"].ToString();
                BlockFieldItems.Add(BFI);
            }
        }

        private void LoadBlocks(XmlNode Node)
        {
            int I;
            TBlockItem BI;
            XmlNode BlockNode, BlockFieldItemsNode;
            for (I = 0; I < Node.ChildNodes.Count; I++)
            {
                BlockNode = Node.ChildNodes[I];
                BI = new TBlockItem();
                BI.Name = BlockNode.Attributes["Name"].Value;
                BI.ProviderName = BlockNode.Attributes["ProviderName"].Value;
                BI.TableName = BlockNode.Attributes["TableName"].Value;
                BI.ParentItemName = BlockNode.Attributes["ParentItemName"].Value;
                BlockFieldItemsNode = WzdUtils.FindNode(null, BlockNode, "BlockFieldItems");
                LoadBlockFieldItems(BlockFieldItemsNode, BI.BlockFieldItems);
                Blocks.Add(BI);
            }
        }

        public void ResetDatabaseConnection()
        {
        }

        public object LoadFromXML(string XML)
        {
            System.Xml.XmlNode Node = null;
            System.Xml.XmlDocument Doc = new System.Xml.XmlDocument();
            Doc.LoadXml(XML);
            Node = Doc.SelectSingleNode("ClientData");
            SolutionName = Node.Attributes["SolutionName"].Value;
            OutputPath = Node.Attributes["OutputPath"].Value;
            CodeOutputPath = Node.Attributes["CodeOutputPath"].Value;
            NewSolution = Node.Attributes["NewSolution"].Value == "1";
            PackageName = Node.Attributes["PackageName"].Value;
            BaseFormName = Node.Attributes["BaseFormName"].Value;
            OutputPath = Node.Attributes["OutputPath"].Value;
            FormName = Node.Attributes["FormName"].Value;
            TableName = Node.Attributes["TableName"].Value;
            ProviderName = Node.Attributes["ProviderName"].Value;
            ColumnCount = Convert.ToInt16(Node.Attributes["ColumnCount"].Value);
            ViewProviderName = Node.Attributes["ViewProviderName"].Value;
            if (Node.Attributes["Language"].Value.ToString().CompareTo("C#") == 0)
                this.Language = "cs";
            else
                this.Language = "vb";
            Node = WzdUtils.FindNode(Doc, Node, "Blocks");
            LoadBlocks(Node);
            return null;
        }

        public bool IsMasterDetailBaseForm()
        {
            bool Result;
            Result = false;
            if (string.Compare(FBaseFormName, "CMasterDetail") == 0 ||
                string.Compare(FBaseFormName, "WorkFlowBase2") == 0 ||
                string.Compare(FBaseFormName, "VBCMasterDetail") == 0)
            {
                Result = true;
            }
            return Result;
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

        public string ViewProviderName
        {
            get
            {
                return FViewProviderName;
            }
            set
            {
                FViewProviderName = value;
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

        public String AssemblyOutputPath
        {
            get { return FAssemblyOutputPath; }
            set { FAssemblyOutputPath = value; }
        }

        public String FormText
        {
            get { return FFormText; }
            set { FFormText = value; }
        }

        public string BaseFormName
        {
            get
            {
                return FBaseFormName;
            }
            set
            {
                FBaseFormName = value;
            }
        }

        public string ServerPackageName
        {
            get
            {
                return FServerPackageName;
            }
            set
            {
                FServerPackageName = value;
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

        public int ColumnCount
        {
            get
            {
                return FColumnCount;
            }
            set
            {
                FColumnCount = value;
            }
        }

        public String LabelAliement
        {
            get
            {
                return FLabelAliement;
            }
            set
            {
                FLabelAliement = value;
            }
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

        public string FormName
        {
            get
            {
                return FFormName;
            }
            set
            {
                FFormName = value;
            }
        }

        public string ProviderName
        {
            get
            {
                return FProviderName;
            }
            set
            {
                FProviderName = value;
            }
        }

        public TBlockItems Blocks
        {
            get
            {
                return FBlocks;
            }
            set
            {
                FBlocks = value;
            }
        }
    }

    public enum TLayoutType { ltNone, ltTextBox, ltDataGridView };

    public class TDetailItem : System.ComponentModel.Component
    {
        private DataRelation FDataRelation;
        private InfoBindingSource FBindingSource;
        private TBlockFieldItems FBlockFieldItems;
        private string FTableName;
        private String FRealTableName;

        public TDetailItem()
        {
            FBlockFieldItems = new TBlockFieldItems(this);
        }

        public DataRelation Relation
        {
            get
            {
                return FDataRelation;
            }
            set
            {
                FDataRelation = value;
            }
        }

        public InfoBindingSource BindingSource
        {
            get
            {
                return FBindingSource;
            }
            set
            {
                FBindingSource = value;
            }
        }

        public TBlockFieldItems BlockFieldItems
        {
            get
            {
                return FBlockFieldItems;
            }
            set
            {
                FBlockFieldItems = value;
            }
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

        public String RealTableName
        {
            get { return FRealTableName; }
            set { FRealTableName = value; }
        }
    }

    public class TCollectionItem : System.Collections.CollectionBase, ICollection
    {
        private TCollection FCollection;

        public TCollection Collection
        {
            get
            {
                return FCollection;
            }
            set
            {
                FCollection = value;
            }
        }

        #region ICollection Members
        int ICollection.Count
        {
            get
            {
                return base.InnerList.Count;
            }
        }

        Boolean IsSynchronized
        {
            get
            {
                return base.InnerList.IsSynchronized;
            }
        }

        Object SyncRoot
        {
            get
            {
                return base.InnerList.SyncRoot;
            }
        }

        void CopyTo(Array array, int index)
        {
            base.InnerList.CopyTo(array, index);
        }
        #endregion
    }

    public class TCollection : System.Collections.CollectionBase, IList, ICollection, IEnumerable
    {
        object FOwner;

        public TCollection()
        {
            FOwner = null;
        }

        public TCollection(object Owner)
        {
            FOwner = Owner;
        }

        public object Owner
        {
            get
            {
                return FOwner;
            }
            set
            {
                FOwner = value;
            }
        }

        #region IList Members
        Boolean IsFixedSize
        {
            get
            {
                return false;
            }
        }
        Boolean IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public object this[int index]
        {
            get
            {
                return base.InnerList[index];
            }
            set
            {
                base.InnerList[index] = value;
            }
        }

        public int Add(object value)
        {
            TCollectionItem Item = (TCollectionItem)value;
            Item.Collection = this;
            int index = base.InnerList.Add(value);
            return index;
        }

        void System.Collections.IList.Clear()
        {
            base.InnerList.Clear();
        }

        bool System.Collections.IList.Contains(object value)
        {
            return base.InnerList.Contains(value);
        }

        int System.Collections.IList.IndexOf(object value)
        {
            return base.InnerList.IndexOf(value);
        }

        void Insert(int index, object value)
        {
            base.InnerList.Insert(index, value);
        }

        public void Remove(object value)
        {
            base.InnerList.Remove(value);
        }

        void System.Collections.IList.RemoveAt(int index)
        {
            base.InnerList.RemoveAt(index);
        }
        #endregion

        #region ICollection Members
        int ICollection.Count
        {
            get
            {
                return base.InnerList.Count;
            }
        }

        Boolean IsSynchronized
        {
            get
            {
                return base.InnerList.IsSynchronized;
            }
        }

        Object SyncRoot
        {
            get
            {
                return base.InnerList.SyncRoot;
            }
        }

        void CopyTo(Array array, int index)
        {
            base.InnerList.CopyTo(array, index);
        }
        #endregion

        #region IEnumerable Members
        System.Collections.IEnumerator IEnumerable.GetEnumerator()
        {
            return base.GetEnumerator();
        }
        #endregion
    }

    public class TClientGenerator : System.ComponentModel.Component
    {
        private TClientData FClientData;
        private DTE2 FDTE2;
        private System.Windows.Forms.Form FRootForm;
        private System.ComponentModel.Design.IDesignerHost FDesignerHost;
        private AddIn FAddIn;
        private string FTemplatePath;
        private InfoDataSet FDataSet = null;
        private ProjectItem GlobalPI;
        private Project GlobalProject;
        private InfoDataGridView FViewGrid;
        private Window GlobalWindow;

        public TClientGenerator(TClientData ClientData, DTE2 dte2, AddIn aAddIn)
        {
            FClientData = ClientData;
            FDTE2 = dte2;
            FAddIn = aAddIn;
            FTemplatePath = WzdUtils.GetAddinsPath() + "\\Templates\\";
        }

        private bool GenSolution()
        {
            Solution sln = FDTE2.Solution;
            string BaseFormProj = FClientData.BaseFormName + "\\" + FClientData.BaseFormName + "." + FClientData.Language + "proj";
            if (FClientData.NewSolution)
            {
                if (System.IO.Directory.Exists(FClientData.OutputPath))
                {
                    if (FClientData.OutputPath == "\\")
                        throw new Exception("Unknown Output Path: " + "\\");
                    System.IO.Directory.Delete(FClientData.OutputPath, true);
                }
                sln.Create(FClientData.OutputPath, FClientData.SolutionName);
                ProjectLoader.AddDefaultProject(FDTE2);
                Project P = sln.AddFromTemplate(FTemplatePath + BaseFormProj,
                    FClientData.CodeOutputPath + "\\" + FClientData.PackageName, FClientData.PackageName, false);
                //FClientData.OutputPath + "\\" + FClientData.PackageName, FClientData.PackageName, true);
                P.Name = FClientData.PackageName;
                string FileName = FClientData.OutputPath + "\\" + FClientData.SolutionName + ".sln";
                sln.SaveAs(FileName);
                //sln.Open(FileName);
                sln.SolutionBuild.StartupProjects = P;
                sln.SolutionBuild.BuildProject(sln.SolutionBuild.ActiveConfiguration.Name, P.FullName, true);
                GlobalProject = P;
            }
            else
            {
                string FilePath = FClientData.CodeOutputPath + "\\" + FClientData.PackageName;
                //string FilePath = Path.GetDirectoryName(FClientData.SolutionName) + "\\" + FClientData.PackageName;
                if (System.IO.Directory.Exists(FilePath))
                {
                    if (FilePath == "\\")
                        throw new Exception("Unknown Output Path: " + "\\");

                    DialogResult dr = MessageBox.Show("There is another File which name is " + FClientData.PackageName + " existed! Do you want to delete it first", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (dr == DialogResult.Yes)
                    {
                        try
                        {
                            System.IO.Directory.Delete(FilePath, true);
                        }
                        catch
                        {
                            System.IO.Directory.Delete(FilePath, true);
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                Project P = sln.AddFromTemplate(FTemplatePath + BaseFormProj,
                    FilePath, FClientData.PackageName, true);
                P.Name = FClientData.PackageName;
                string FileName = FilePath + "\\" + FClientData.PackageName + "." + FClientData.Language + "proj";
                P.Save(FileName);
                sln.Open(FClientData.SolutionName);
                int I;
                P = null;
                for (I = 1; I <= sln.Projects.Count; I++)
                {
                    P = sln.Projects.Item(I);
                    if (string.Compare(P.Name, FClientData.PackageName) == 0)
                        break;
                    else
                        P = null;
                }
                if (P != null)
                    sln.Remove(P);
                P = sln.AddFromFile(FileName, false);
                P.Properties.Item("RootNamespace").Value = FClientData.PackageName;
                P.Properties.Item("AssemblyName").Value = FClientData.PackageName;
                sln.SaveAs(sln.FileName);
                sln.SolutionBuild.StartupProjects = P;
                sln.SolutionBuild.BuildProject(sln.SolutionBuild.ActiveConfiguration.Name, P.FullName, true);
                GlobalProject = P;
            }
            if (FClientData.AssemblyOutputPath != null && FClientData.AssemblyOutputPath != "")
                GlobalProject.ConfigurationManager.ActiveConfiguration.Properties.Item("OutputPath").Value = FClientData.AssemblyOutputPath;

            return true;
        }

        private void RenameNameSpace(string FileName)
        {
            if (!File.Exists(FileName))
                return;
            System.IO.StreamReader SR = new System.IO.StreamReader(FileName);
            string Context = SR.ReadToEnd();
            SR.Close();
            Context = Context.Replace("TAG_NAMESPACE", FClientData.PackageName);
            Context = Context.Replace("TAG_FORMNAME", FClientData.FormName);
            System.IO.FileStream Filefs = new System.IO.FileStream(FileName, System.IO.FileMode.Create, System.IO.FileAccess.ReadWrite);
            System.IO.StreamWriter SW = new System.IO.StreamWriter(Filefs, Encoding.UTF8);
            SW.Write(Context);
            SW.Close();
            Filefs.Close();
        }

        /*
                private void GetForm()
                {
                    Solution Sln = FDTE2.Solution;
                    Project P = null;
                    int I;
                    for (I = 1; I <= Sln.Projects.Count; I++)
                    {
                        P = Sln.Projects.Item(I);
                        if (string.Compare(FClientData.PackageName, P.Name) == 0)
                            break;
                        P = null;
                    }
                    if (P == null)
                        throw new Exception("Can not find project " + FClientData.PackageName + " in solution");
                    GlobalProject = P;
                    ProjectItem PI;
                    for (I = 1; I <= P.ProjectItems.Count; I++)
                    {
                        PI = P.ProjectItems.Item(I);
                        if (string.Compare(PI.Name, "Form1.cs") == 0)
                        {
                            string Path = PI.get_FileNames(0);
                            Path = System.IO.Path.GetDirectoryName(Path);
                            RenameNameSpace(Path + "\\Form1.cs");
                            RenameNameSpace(Path + "\\Form1.Designer.cs");
                            Window W = PI.Open("{00000000-0000-0000-0000-000000000000}");
                            W.Activate();
                            GlobalPI = PI;
                            GlobalWindow = W;
                            if (string.Compare(FClientData.FormName, "Form1") != 0)
                            {
                                string NewName = FClientData.FormName + ".cs"; //???
                                string FileName = Path + "\\" + NewName;
                                PI.SaveAs(FileName);
                                W.Close(vsSaveChanges.vsSaveChangesNo);
                                PI.Remove();
                                P.ProjectItems.AddFromFile(FileName);
                                System.IO.File.Delete(Path + "\\Form1.cs");
                                System.IO.File.Delete(Path + "\\Form1.Designer.cs");
                                System.IO.File.Delete(Path + "\\Form1.resx"); //???
                                foreach (ProjectItem PI2 in P.ProjectItems)
                                {
                                    if (string.Compare(PI2.Name, NewName) == 0)
                                    {
                                        W = PI2.Open("{00000000-0000-0000-0000-000000000000}");
                                        W.Activate();
                                        GlobalWindow = W;
                                        GlobalPI = PI2;
                                    }
                                }
                            }
                            FDesignerHost = (IDesignerHost)W.Object;
                            FRootForm = (System.Windows.Forms.Form)FDesignerHost.RootComponent;
                            FRootForm.Name = FClientData.FormName;
                            FRootForm.Text = FClientData.FormName;
                            IComponentChangeService FComponentChangeService = (IComponentChangeService)FDesignerHost.RootComponent.Site.GetService(typeof(IComponentChangeService));
                        }
                        if (string.Compare(PI.Name, "Program.cs") == 0)
                        {
                            RenameNameSpace(PI.get_FileNames(0));
                        }
                    }
                }
        */

        private void GetForm()
        {
            Solution Sln = FDTE2.Solution;
            Project P = null;
            int I;
            for (I = 1; I <= Sln.Projects.Count; I++)
            {
                P = Sln.Projects.Item(I);
                if (string.Compare(FClientData.PackageName, P.Name) == 0)
                    break;
                P = null;
            }
            if (P == null)
                throw new Exception("Can not find project " + FClientData.PackageName + " in solution");
            ProjectItem PI;
            for (I = P.ProjectItems.Count; I >= 1; I--)
            {
                PI = P.ProjectItems.Item(I);
                if (string.Compare(PI.Name, "Form1." + FClientData.Language) == 0)
                {
                    string Path = PI.get_FileNames(0);
                    Path = System.IO.Path.GetDirectoryName(Path);
                    RenameNameSpace(Path + "\\Form1." + FClientData.Language);
                    RenameNameSpace(Path + "\\Form1.Designer." + FClientData.Language);
                    Application.DoEvents();
                    FDTE2.MainWindow.Activate();
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(3000);
                    Application.DoEvents();
                    Window W = PI.Open("{00000000-0000-0000-0000-000000000000}");
                    W.Activate();
                    GlobalPI = PI;
                    GlobalWindow = W;
                    if (string.Compare(FClientData.FormName, "Form1") != 0)
                    {
                        PI.Name = FClientData.FormName + "." + FClientData.Language;
                        W.Close(vsSaveChanges.vsSaveChangesYes);
                        W = PI.Open("{00000000-0000-0000-0000-000000000000}");
                        W.Activate();
                    }
                    FDesignerHost = (IDesignerHost)W.Object;
                    FRootForm = (System.Windows.Forms.Form)FDesignerHost.RootComponent;
                    FRootForm.Name = FClientData.FormName;
                    FRootForm.Text = FClientData.FormText;
                    IComponentChangeService FComponentChangeService = (IComponentChangeService)FDesignerHost.RootComponent.Site.GetService(typeof(IComponentChangeService));
                }
                if (string.Compare(PI.Name, "Program." + FClientData.Language) == 0)
                {
                    RenameNameSpace(PI.get_FileNames(0));
                }
            }
        }

        private void GenViewBlockControl(TBlockItem BlockItem)
        {
            if (FClientData.IsMasterDetailBaseForm())
            {
                InfoDataSet ViewDataSet = FDesignerHost.CreateComponent(typeof(InfoDataSet), "idView") as InfoDataSet;
                ViewDataSet.RemoteName = FClientData.ViewProviderName;
                if (ViewDataSet.RemoteName.Trim() == "")
                {
                    ViewDataSet.RemoteName = FClientData.ProviderName;
                }
                if (ViewDataSet.RemoteName.IndexOf('.') < 0)
                    ViewDataSet.RemoteName = ViewDataSet.RemoteName + "." + FClientData.TableName;
                ViewDataSet.Active = true;
                InfoBindingSource ViewBindingSource = FDesignerHost.CreateComponent(typeof(InfoBindingSource),
                    "ibsView") as InfoBindingSource;
                ViewBindingSource.DataSource = ViewDataSet;
                ViewBindingSource.DataMember = ViewDataSet.RealDataSet.Tables[0].TableName;
                BlockItem.BindingSource = ViewBindingSource;
                InfoNavigator navigator1 = FRootForm.Controls["infoNavigator1"] as InfoNavigator;
                if (navigator1 != null)
                    navigator1.ViewBindingSource = ViewBindingSource;

                InfoRelation Relation = new InfoRelation();
                Relation.RelationDataSet = FDataSet;
                InfoKeyField KeyField;
                foreach (DataColumn KeyFieldName in FDataSet.RealDataSet.Tables[0].PrimaryKey)
                {
                    KeyField = new InfoKeyField();
                    KeyField.FieldName = KeyFieldName.ColumnName;
                    Relation.SourceKeyFields.Add(KeyField);
                }
                foreach (DataColumn KeyFieldName in FDataSet.RealDataSet.Tables[0].PrimaryKey)
                {
                    KeyField = new InfoKeyField();
                    KeyField.FieldName = KeyFieldName.ColumnName;
                    Relation.TargetKeyFields.Add(KeyField);
                }
                Relation.Active = true;
                ViewBindingSource.Relations.Add(Relation);
            }

            TBlockFieldItem FieldItem;
            System.Windows.Forms.SplitContainer scMaster = FRootForm.Controls["scMaster"] as System.Windows.Forms.SplitContainer;
            FViewGrid = FDesignerHost.CreateComponent(typeof(InfoDataGridView), "grdView") as InfoDataGridView;
            FViewGrid.Parent = scMaster.Panel1;
            FViewGrid.Dock = DockStyle.Fill;
            FViewGrid.TabIndex = 0;
            FViewGrid.DataSource = BlockItem.BindingSource;

            if (FClientData.IsMasterDetailBaseForm())
            {
                FViewGrid.Columns.Clear();
                DataGridViewTextBoxColumn Column;
                int I;
                for (I = 0; I < BlockItem.BlockFieldItems.Count; I++)
                {
                    FieldItem = BlockItem.BlockFieldItems[I] as TBlockFieldItem;
                    Column = FDesignerHost.CreateComponent(typeof(DataGridViewTextBoxColumn), "dgc" + WzdUtils.RemoveSpecialCharacters(BlockItem.TableName) + FieldItem.DataField) as DataGridViewTextBoxColumn;
                    Column.DataPropertyName = FieldItem.DataField;
                    Column.DefaultCellStyle.Format = FieldItem.EditMask;
                    Column.HeaderText = FieldItem.Description;
                    Column.MaxInputLength = FieldItem.Length;
                    if (Column.HeaderText.Trim() == "")
                        Column.HeaderText = FieldItem.DataField;
                    if ((BlockItem.BlockFieldItems[I] as TBlockFieldItem).DataType == typeof(int) || (BlockItem.BlockFieldItems[I] as TBlockFieldItem).DataType == typeof(float)
                        || (BlockItem.BlockFieldItems[I] as TBlockFieldItem).DataType == typeof(double))
                        Column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    FViewGrid.Columns.Add(Column);
                }

                /*
                DataGridViewColumn Column;
                int I, Index;
                for (I = 0; I < BlockItem.BlockFieldItems.Count; I++)
                {
                    FieldItem = BlockItem.BlockFieldItems[I] as TBlockFieldItem;
                    if (FieldItem.Description == "")
                        Index = FViewGrid.Columns.Add(FieldItem.DataField, FieldItem.DataField);
                    else
                        Index = FViewGrid.Columns.Add(FieldItem.DataField, FieldItem.Description);
                    Column = FViewGrid.Columns[Index];
                    Column.DataPropertyName = FieldItem.DataField;
                    Column.Name = "dgc" + FieldItem.DataField;
                    Column.HeaderText = FieldItem.Description;
                    if (Column.HeaderText.Trim() == "")
                        Column.HeaderText = FieldItem.DataField;
                }
                 */
            }
        }

        private void AdjectLabelEditPos(TStringList EditList, TStringList LabelList)
        {
            int MaxLabelWidth = 0;
            Label label = null;
            Control textbox = null;
            for (int I = 0; I < LabelList.Count; I++)
            {
                label = (Label)LabelList[I];
                if (label.Width > MaxLabelWidth)
                    MaxLabelWidth = label.Width;
            }
            if (MaxLabelWidth >= 105)
            {
                int EditOffSet = MaxLabelWidth - 105 + 5;

                for (int I = 0; I < EditList.Count; I++)
                {
                    textbox = (Control)EditList[I];
                    textbox.Left = 110 + EditOffSet;
                }

                for (int I = 0; I < LabelList.Count; I++)
                {
                    label = (Label)LabelList[I];
                    label.Left = 110 - label.Width - 5 + EditOffSet;
                }
            }
            if (EditList.Count == 0)
                return;
            int ColumnIndex = 0;
            int ColumnControlCount = (EditList.Count + (FClientData.ColumnCount - 1)) / FClientData.ColumnCount;
            int ColumnWidth = ((Control)EditList[0]).Left + ((Control)EditList[0]).Width;
            int TopOffset = 10;
            int ColumnControlIndex = 0;

            int maxHeight = 0;
            for (int I = 0; I < EditList.Count; I++)
            {
                textbox = (Control)EditList[I];
                if (textbox.Height > maxHeight)
                    maxHeight = textbox.Height;
            }

            for (int I = 0; I < EditList.Count; I++)
            {
                if (I % ColumnControlCount == 0)
                {
                    if (I + 1 >= ColumnControlCount)
                    {
                        ColumnControlIndex = 0;
                        ColumnIndex++;
                    }
                }
                label = (Label)LabelList[I];
                textbox = (Control)EditList[I];
                textbox.Left = textbox.Left + ColumnWidth * ColumnIndex;
                textbox.Top = TopOffset + (maxHeight + 5) * ColumnControlIndex;
                label.Left = label.Left + ColumnWidth * ColumnIndex;
                label.Top = textbox.Top + (maxHeight - label.Height) / 2;
                ColumnControlIndex++;
            }
        }

        private InfoRefVal GenRefVal(TBlockFieldItem FieldItem, string TableName)
        {
            String Name = "rv" + TableName + FieldItem.DataField;
            InfoRefVal Result = FDesignerHost.CreateComponent(typeof(InfoRefVal), Name) as InfoRefVal;
            InfoCommand aInfoCommand = new InfoCommand(FClientData.DatabaseType);
            aInfoCommand.Connection = WzdUtils.AllocateConnection(FClientData.DatabaseName, FClientData.DatabaseType, true);
            //aInfoCommand.Connection = FClientData.Owner.GlobalConnection;
            IDbDataAdapter DA = DBUtils.CreateDbDataAdapter(aInfoCommand);
            DataSet aDataSet = new DataSet();
            //SYS_REFVAL
            aInfoCommand.CommandText = String.Format("Select * from SYS_REFVAL where REFVAL_NO = '{0}'", FieldItem.RefValNo);
            WzdUtils.FillDataAdapter(FClientData.DatabaseType, DA, aDataSet, FieldItem.RefValNo);
            if (aDataSet.Tables[0].Rows.Count != 1)
                throw new Exception(String.Format("Unknown REFVAL_NO in SYS_REFVAL: {0}", FieldItem.RefValNo));
            Result.Caption = aDataSet.Tables[0].Rows[0]["CAPTION"].ToString();
            Result.DisplayMember = aDataSet.Tables[0].Rows[0]["DISPLAY_MEMBER"].ToString();
            Result.ValueMember = aDataSet.Tables[0].Rows[0]["VALUE_MEMBER"].ToString();
            Result.SelectAlias = aDataSet.Tables[0].Rows[0]["SELECT_ALIAS"].ToString();
            Result.SelectCommand = aDataSet.Tables[0].Rows[0]["SELECT_COMMAND"].ToString();
            //SYS_REFVSL_D1 --> Columns
            aDataSet.Clear();
            aInfoCommand.CommandText = String.Format("Select * from SYS_REFVAL_D1 where REFVAL_NO = '{0}'", FieldItem.RefValNo);
            WzdUtils.FillDataAdapter(FClientData.DatabaseType, DA, aDataSet, FieldItem.RefValNo);
            foreach (DataRow DR in aDataSet.Tables[0].Rows)
            {
                RefColumns RC = new RefColumns();
                RC.Column = DR["FIELD_NAME"].ToString();
                RC.HeaderText = DR["HEADER_TEXT"].ToString();
                Result.Columns.Add(RC);
            }
            /*
            //SYS_REFVAL_D2 --> WhereItem
            aDataSet.Clear();
            aInfoCommand.CommandText = String.Format("Select * from SYS_REFVAL_D2 where REFVAL_NO = '{0}'", FieldItem.RefValNo);
            WzdUtils.FillDataAdapter(ClientType.ctMsSql, DA, aDataSet, FieldItem.RefValNo);
            foreach (DataRow DR in aDataSet.Tables[0].Rows)
            {
                WhereItem WI = new WhereItem();
                WI.FieldName = DR["FIELD_NAME"].ToString();
                String S = DR["CONDITION"].ToString();
                if (S != null && S != "")
                {
                    WI.Condition = (WhereItem.condition)int.Parse(S);
                }
                WI.Value = DR["VALUE"].ToString();
                Result.whereItem.Add(WI);
            }
            //SYS_REFVAL_D3 --> ColumnMatch
            aDataSet.Clear();
            aInfoCommand.CommandText = String.Format("Select * from SYS_REFVAL_D3 where REFVAL_NO = '{0}'", FieldItem.RefValNo);
            WzdUtils.FillDataAdapter(ClientType.ctMsSql, DA, aDataSet, FieldItem.RefValNo);
            foreach (DataRow DR in aDataSet.Tables[0].Rows)
            {
                ColumnMatch CM = new ColumnMatch();
                CM.SrcField = DR["FIELD_NAME"].ToString();
                CM.DestField = DR["DES_FIELD_NAME"].ToString();
                CM.SrcGetValue = DR["SRC_GET_VALUE"].ToString();
                Result.columnMatch.Add(CM);
            }
            */
            return Result;
        }

        private void CreateQueryField(TBlockFieldItem aFieldItem, String Range, InfoRefVal aRefVal, InfoBindingSource ibs)
        {
            if (aFieldItem.QueryMode == null)
                return;
            InfoNavigator navigator1 = FRootForm.Controls["infoNavigator1"] as InfoNavigator;
            if (navigator1 != null)
            {
                if (aFieldItem.QueryMode.ToUpper() == "NORMAL" || aFieldItem.QueryMode.ToUpper() == "RANGE")
                {
                    QueryField qField = new QueryField();
                    qField.FieldName = aFieldItem.DataField;
                    qField.Caption = aFieldItem.Description;
                    if (qField.Caption == "")
                        qField.Caption = aFieldItem.DataField;
                    if (aFieldItem.QueryMode.ToUpper() == "NORMAL")
                    {
                        if (aFieldItem.DataType == typeof(DateTime))
                            qField.Condition = "=";
                        if (aFieldItem.DataType == typeof(int) || aFieldItem.DataType == typeof(float) ||
                            aFieldItem.DataType == typeof(double) || aFieldItem.DataType == typeof(Int16))
                            qField.Condition = "=";
                        if (aFieldItem.DataType == typeof(String))
                            qField.Condition = "%";
                    }
                    if (aFieldItem.QueryMode.ToUpper() == "RANGE")
                    {
                        qField.Condition = "And";
                        if (Range == "")
                        {
                            qField.Condition = "<=";
                            CreateQueryField(aFieldItem, ">=", aRefVal, null);
                        }
                        else
                        {
                            qField.Condition = Range;
                        }
                    }
                    switch (aFieldItem.ControlType.ToUpper())
                    {
                        case "TEXTBOX":
                            qField.Mode = "TextBox";
                            break;
                        case "COMBOBOX":
                            qField.Mode = "ComboBox";
                            qField.RefVal = aRefVal;
                            break;
                        case "REFVALBOX":
                            qField.Mode = "RefVal";
                            qField.RefVal = aRefVal;
                            break;
                        case "DATETIMEBOX":
                            qField.Mode = "Calendar";
                            break;
                    }
                    navigator1.QueryFields.Add(qField);
                }
            }

            ClientQuery aClientQuery = FRootForm.Container.Components["clientQuery1"] as ClientQuery;
            if (aClientQuery != null)
            {
                if (ibs != null && aClientQuery != null)
                    aClientQuery.BindingSource = ibs;
                if (aFieldItem.QueryMode.ToUpper() == "NORMAL" || aFieldItem.QueryMode.ToUpper() == "RANGE")
                {
                    QueryColumns qColumn = new QueryColumns();
                    qColumn.Column = aFieldItem.DataField;
                    qColumn.Caption = aFieldItem.Description;
                    if (qColumn.Caption == "")
                        qColumn.Caption = aFieldItem.DataField;
                    if (aFieldItem.QueryMode.ToUpper() == "NORMAL")
                    {
                        if (aFieldItem.DataType == typeof(DateTime))
                            qColumn.Operator = "=";
                        if (aFieldItem.DataType == typeof(int) || aFieldItem.DataType == typeof(float) ||
                            aFieldItem.DataType == typeof(double) || aFieldItem.DataType == typeof(Int16))
                            qColumn.Operator = "=";
                        if (aFieldItem.DataType == typeof(String))
                            qColumn.Operator = "%";
                    }
                    if (aFieldItem.QueryMode.ToUpper() == "RANGE")
                    {
                        qColumn.Condition = "And";
                        if (Range == "")
                        {
                            qColumn.Operator = "<=";
                            CreateQueryField(aFieldItem, ">=", aRefVal, null);
                        }
                        else
                        {
                            qColumn.Operator = Range;
                            //qColumn.Condition = Range;
                        }
                    }
                    switch (aFieldItem.ControlType.ToUpper())
                    {
                        case "TEXTBOX":
                            qColumn.ColumnType = "ClientQueryTextBoxColumn";
                            break;
                        case "COMBOBOX":
                            qColumn.ColumnType = "ClientQueryComboBoxColumn";
                            qColumn.InfoRefVal = aRefVal;
                            break;
                        case "REFVALBOX":
                            qColumn.ColumnType = "ClientQueryRefValColumn";
                            qColumn.InfoRefVal = aRefVal;
                            break;
                        case "DATETIMEBOX":
                            qColumn.ColumnType = "ClientQueryCalendarColumn";
                            break;
                        case "CHECKBOX":
                            qColumn.ColumnType = "ClientQueryCheckBoxColumn";
                            break;
                    }
                    aClientQuery.Columns.Add(qColumn);
                }
            }
        }

        private void GenMainBlockControl(TBlockItem BlockItem)
        {
            InfoBindingSource MainBindingSource = FDesignerHost.CreateComponent(typeof(InfoBindingSource), "ibs" + WzdUtils.RemoveSpecialCharacters(BlockItem.TableName)) as InfoBindingSource;
            MainBindingSource.DataSource = FDataSet;
            MainBindingSource.DataMember = FDataSet.RealDataSet.Tables[0].TableName;
            MainBindingSource.AutoApply = true;
            BlockItem.BindingSource = MainBindingSource;
            InfoNavigator navigator1 = FRootForm.Controls["infoNavigator1"] as InfoNavigator;
            if (navigator1 != null)
            {
                navigator1.BindingSource = MainBindingSource;
                if (navigator1.ViewBindingSource == null && FClientData.BaseFormName != "CSingle")
                    navigator1.ViewBindingSource = MainBindingSource;
            }

            Control ParentControl;
            System.Windows.Forms.SplitContainer scMaster;
            scMaster = FRootForm.Controls["scMaster"] as System.Windows.Forms.SplitContainer;
            if (FClientData.IsMasterDetailBaseForm())
            {
                SplitContainer NewSpl = FDesignerHost.CreateComponent(typeof(SplitContainer), "scDetail") as SplitContainer;
                NewSpl.Panel1.AutoScroll = true;
                NewSpl.Parent = scMaster.Panel2;
                NewSpl.Dock = DockStyle.Fill;
                NewSpl.Orientation = Orientation.Horizontal;
                ParentControl = NewSpl.Panel1;
                scMaster.Panel2.Controls.Add(NewSpl);
            }
            else
            {
                //Panel ParentPanel = new Panel();
                //ParentPanel.Parent = scMaster.Panel2;
                //ParentPanel.Dock = DockStyle.Fill;
                //scMaster.Panel2.Controls.Add(ParentPanel);
                ParentControl = scMaster.Panel2;
            }
            TStringList aLabelList = new TStringList();
            TStringList aEditList = new TStringList();
            TBlockFieldItem aFieldItem;
            System.Windows.Forms.Label l = null;
            int TopOffset = 10;
            int LeftOffst = 100;
            InfoTextBox aInfoTextBox = null;
            InfoRefvalBox aInfoRefValBox = null;
            InfoDateTimePicker aInfoDateTimePicker = null;
            InfoRefVal aRefVal = null;
            InfoComboBox aComboBox = null;
            CheckBox aCheckBox = null;

            for (int I = 0; I < BlockItem.BlockFieldItems.Count; I++)
            {
                aFieldItem = BlockItem.BlockFieldItems[I] as TBlockFieldItem;
                aInfoTextBox = null;
                aInfoRefValBox = null;
                aInfoDateTimePicker = null;
                aRefVal = null;
                aComboBox = null;
                aCheckBox = null;

                if ((aFieldItem.RefValNo != null && aFieldItem.RefValNo != "") || aFieldItem.RefField != null)
                {
                    aRefVal = GenRefVal(aFieldItem, WzdUtils.RemoveSpecialCharacters(BlockItem.TableName));
                    aInfoRefValBox = FDesignerHost.CreateComponent(typeof(InfoRefvalBox)) as InfoRefvalBox;
                    aInfoRefValBox.Parent = ParentControl;
                    TopOffset += aInfoRefValBox.Height + 5;
                    aInfoRefValBox.Top = TopOffset;
                    aInfoRefValBox.Left = LeftOffst;
                    aInfoRefValBox.Width = 150;
                    //aInfoRefValBox.Text = aFieldItem.DataField;
                    //aInfoRefValBox.Name = "tb" + aFieldItem.DataField;
                    aInfoRefValBox.Name = aFieldItem.DataField + "InfoRefValBox";
                    aInfoRefValBox.Site.Name = aFieldItem.DataField + "InfoRefValBox";
                    aInfoRefValBox.DataBindings.Add(new Binding("Text", BlockItem.BindingSource, aFieldItem.DataField, true));
                    aInfoRefValBox.DataBindings[0].FormatString = aFieldItem.EditMask;
                    aInfoRefValBox.RefVal = aRefVal;
                    aInfoRefValBox.TextBoxBindingSource = BlockItem.BindingSource;
                    aInfoRefValBox.TextBoxBindingMember = aFieldItem.DataField;
                    aInfoRefValBox.MaxLength = aFieldItem.Length;
                    aEditList.Add(aInfoRefValBox);
                }
                else if (aFieldItem.ControlType == "ComboBox")
                {
                    string type = FindSystemDBType("SystemDB");

                    aComboBox = FDesignerHost.CreateComponent(typeof(InfoComboBox)) as InfoComboBox;
                    //aComboBox.Name = "icb" + aFieldItem.DataField;
                    aComboBox.Name = aFieldItem.DataField + "ComboBox";
                    aComboBox.Site.Name = aFieldItem.DataField + "ComboBox";
                    aComboBox.Parent = ParentControl;
                    TopOffset += aComboBox.Height + 5;
                    aComboBox.Top = TopOffset;
                    aComboBox.Left = LeftOffst;
                    aComboBox.Width = 150;
                    aComboBox.SelectAlias = FClientData.Owner.SelectedAlias;
                    if (aFieldItem.ComboEntityName != null && aFieldItem.ComboTextField != null && aFieldItem.ComboValueField != null)
                    {
                        if (type == "1")
                            aComboBox.SelectCommand = String.Format("Select [{0}].[{1}], [{0}].[{2}] from [{0}]", aFieldItem.ComboEntityName, aFieldItem.ComboTextField, aFieldItem.ComboValueField);
                        else if (type == "2")
                            aComboBox.SelectCommand = String.Format("Select [{0}].[{1}], [{0}].[{2}] from [{0}]", aFieldItem.ComboEntityName, aFieldItem.ComboTextField, aFieldItem.ComboValueField);
                        else if (type == "3")
                            aComboBox.SelectCommand = String.Format("Select {0}.{1}, {0}.{2} from {0}", aFieldItem.ComboEntityName, aFieldItem.ComboTextField, aFieldItem.ComboValueField);
                        else if (type == "4")
                            aComboBox.SelectCommand = String.Format("Select {0}.{1}, {0}.{2} from {0}", aFieldItem.ComboEntityName, aFieldItem.ComboTextField, aFieldItem.ComboValueField);
                        else if (type == "5")
                            aComboBox.SelectCommand = String.Format("Select {0}.{1}, {0}.{2} from {0}", aFieldItem.ComboEntityName, aFieldItem.ComboTextField, aFieldItem.ComboValueField);
                    }
                    aComboBox.DisplayMember = aFieldItem.ComboTextField;
                    aComboBox.ValueMember = aFieldItem.ComboValueField;
                    aComboBox.DataBindings.Add(new Binding("SelectedValue", BlockItem.BindingSource, aFieldItem.DataField, true));
                    //if (dsColdef.Tables[0].Rows.Count > 0)
                    //    for (int j = 0; j < dsColdef.Tables[0].Rows.Count; j++)
                    //        if (dsColdef.Tables[0].Rows[j]["FIELD_NAME"].ToString() == aFieldItem.DataField && dsColdef.Tables[0].Rows[j]["EDITMASK"] != null)
                    //            aComboBox.DataBindings[0].FormatString = dsColdef.Tables[0].Rows[j]["EDITMASK"].ToString();
                    aComboBox.DataBindings[0].FormatString = aFieldItem.EditMask;
                    aEditList.Add(aComboBox);
                }
                else if (aFieldItem.ControlType == "CheckBox")
                {
                    aCheckBox = FDesignerHost.CreateComponent(typeof(CheckBox)) as CheckBox;
                    aCheckBox.Name = aFieldItem.DataField + "CheckBox";
                    aCheckBox.Site.Name = aFieldItem.DataField + "CheckBox";
                    aCheckBox.Parent = ParentControl;
                    aCheckBox.Height = 22;
                    TopOffset += aCheckBox.Height + 5;
                    aCheckBox.Top = TopOffset;
                    aCheckBox.Left = LeftOffst;
                    aCheckBox.Width = 150;
                    aCheckBox.DataBindings.Add(new Binding("Checked", BlockItem.BindingSource, aFieldItem.DataField, true));
                    aEditList.Add(aCheckBox);
                }
                else
                {
                    if (FDataSet.RealDataSet.Tables[0].Columns[aFieldItem.DataField].DataType == typeof(DateTime) || (aFieldItem.ControlType != null && aFieldItem.ControlType.ToUpper() == "DATETIMEBOX"))
                    {
                        aInfoDateTimePicker = FDesignerHost.CreateComponent(typeof(InfoDateTimePicker)) as InfoDateTimePicker;
                        aInfoDateTimePicker.Parent = ParentControl;
                        TopOffset += aInfoDateTimePicker.Height + 5;
                        aInfoDateTimePicker.Top = TopOffset;
                        aInfoDateTimePicker.Left = LeftOffst;
                        aInfoDateTimePicker.Width = 150;
                        //aInfoDateTimePicker.Name = "dtp" + aFieldItem.DataField;
                        aInfoDateTimePicker.Site.Name = aFieldItem.DataField + "InfoDateTimePicker";
                        aInfoDateTimePicker.DataBindings.Add(new Binding("Text", BlockItem.BindingSource, aFieldItem.DataField, true));
                        //if (dsColdef.Tables[0].Rows.Count > 0)
                        //    for (int j = 0; j < dsColdef.Tables[0].Rows.Count; j++)
                        //        if (dsColdef.Tables[0].Rows[j]["FIELD_NAME"].ToString() == aFieldItem.DataField && dsColdef.Tables[0].Rows[j]["EDITMASK"] != null)
                        //            aInfoDateTimePicker.DataBindings[0].FormatString = dsColdef.Tables[0].Rows[j]["EDITMASK"].ToString();
                        aInfoDateTimePicker.DataBindings[0].FormatString = aFieldItem.EditMask;
                        aEditList.Add(aInfoDateTimePicker);
                    }
                    else
                    {
                        aInfoTextBox = FDesignerHost.CreateComponent(typeof(InfoTextBox)) as InfoTextBox;
                        aInfoTextBox.Parent = ParentControl;
                        TopOffset += aInfoTextBox.Height + 5;
                        aInfoTextBox.Top = TopOffset;
                        aInfoTextBox.Left = LeftOffst;
                        aInfoTextBox.Width = 150;
                        //aInfoTextBox.Text = aFieldItem.DataField;
                        //aInfoTextBox.Name = "tb" + aFieldItem.DataField;
                        aInfoTextBox.Site.Name = aFieldItem.DataField + "InfoTextBox";
                        aInfoTextBox.DataBindings.Add(new Binding("Text", BlockItem.BindingSource, aFieldItem.DataField, true));
                        //if (dsColdef.Tables[0].Rows.Count > 0)
                        //    for (int j = 0; j < dsColdef.Tables[0].Rows.Count; j++)
                        //        if (dsColdef.Tables[0].Rows[j]["FIELD_NAME"].ToString() == aFieldItem.DataField && dsColdef.Tables[0].Rows[j]["EDITMASK"] != null)
                        //            aInfoTextBox.DataBindings[0].FormatString = dsColdef.Tables[0].Rows[j]["EDITMASK"].ToString();
                        aInfoTextBox.DataBindings[0].FormatString = aFieldItem.EditMask;
                        aInfoTextBox.MaxLength = aFieldItem.Length;
                        aEditList.Add(aInfoTextBox);
                    }
                }

                CreateQueryField(aFieldItem, "", aRefVal, MainBindingSource);

                l = FDesignerHost.CreateComponent(typeof(System.Windows.Forms.Label)) as Label;
                l.Parent = ParentControl;
                l.AutoSize = true;
                l.Text = aFieldItem.Description;
                if (l.Text == "")
                    l.Text = aFieldItem.DataField;
                if (FClientData.LabelAliement != null)
                {
                    if (FClientData.LabelAliement.ToLower() == "right")
                        l.Left = LeftOffst - l.Width - 5;
                }
                else
                    l.Left = 15;
                if (aInfoTextBox != null)
                {
                    l.Top = aInfoTextBox.Top + (aInfoTextBox.Height - l.Height) / 2;
                }
                if (aInfoDateTimePicker != null)
                {
                    l.Top = aInfoDateTimePicker.Top + (aInfoDateTimePicker.Height - l.Height) / 2;
                }
                aLabelList.Add(l);
            }
            AdjectLabelEditPos(aEditList, aLabelList);
        }

        private void GenMainBlockControl2(TBlockItem BlockItem)
        {
            IComponent aDataSet = FRootForm.Container.Components["Query"];
            if (aDataSet != null)
                FDesignerHost.DestroyComponent(aDataSet);
            IComponent aSource = FRootForm.Container.Components["ibsQuery"];
            if (aSource != null)
                FDesignerHost.DestroyComponent(aSource);

            InfoBindingSource MainBindingSource = FDesignerHost.CreateComponent(typeof(InfoBindingSource), "ibs" + WzdUtils.RemoveSpecialCharacters(BlockItem.TableName)) as InfoBindingSource;
            MainBindingSource.DataSource = FDataSet;
            MainBindingSource.DataMember = FDataSet.RealDataSet.Tables[0].TableName;
            BlockItem.BindingSource = MainBindingSource;
            SplitContainer SplitContainer1 = FRootForm.Controls["splitContainer1"] as SplitContainer;
            InfoDataGridView Grid = SplitContainer1.Panel2.Controls["InfoDataGridView1"] as InfoDataGridView;
            Grid.DataSource = BlockItem.BindingSource;
            Grid.Columns.Clear();
            TBlockFieldItem aFieldItem;
            DataGridViewTextBoxColumn Column;
            InfoDataGridViewComboBoxColumn ComboBoxColumn;
            InfoDataGridViewRefValColumn RefValColumn;
            InfoRefVal aInfoRefVal = null;
            int I;
            for (I = 0; I < BlockItem.BlockFieldItems.Count; I++)
            {
                aFieldItem = BlockItem.BlockFieldItems[I] as TBlockFieldItem;
                if ((aFieldItem.RefValNo != null && aFieldItem.RefValNo != "") || aFieldItem.RefField != null)
                {
                    aInfoRefVal = GenRefVal(aFieldItem, WzdUtils.RemoveSpecialCharacters(BlockItem.TableName));
                    RefValColumn = FDesignerHost.CreateComponent(typeof(InfoDataGridViewRefValColumn), "dgc" + WzdUtils.RemoveSpecialCharacters(BlockItem.TableName) + aFieldItem.DataField) as InfoDataGridViewRefValColumn;
                    RefValColumn.DataPropertyName = aFieldItem.DataField;
                    RefValColumn.HeaderText = aFieldItem.Description;
                    RefValColumn.MaxInputLength = aFieldItem.Length;
                    if (RefValColumn.HeaderText.Trim() == "")
                        RefValColumn.HeaderText = aFieldItem.DataField;
                    RefValColumn.RefValue = aInfoRefVal;
                    Grid.Columns.Add(RefValColumn);
                }
                else if (aFieldItem.ControlType == "ComboBox")
                {
                    string type = FindSystemDBType("SystemDB");

                    InfoRefVal bInfoRefVal = FDesignerHost.CreateComponent(typeof(InfoRefVal), "irv" + WzdUtils.RemoveSpecialCharacters(BlockItem.TableName) + aFieldItem.DataField) as InfoRefVal;
                    bInfoRefVal.SelectAlias = FClientData.Owner.SelectedAlias;
                    try
                    {
                        if (type == "1")
                            bInfoRefVal.SelectCommand = String.Format("Select [{0}].[{1}], [{0}].[{2}] from [{0}]", aFieldItem.ComboEntityName, aFieldItem.ComboTextField, aFieldItem.ComboValueField);
                        else if (type == "2")
                            bInfoRefVal.SelectCommand = String.Format("Select [{0}].[{1}], [{0}].[{2}] from [{0}]", aFieldItem.ComboEntityName, aFieldItem.ComboTextField, aFieldItem.ComboValueField);
                        else if (type == "3")
                            bInfoRefVal.SelectCommand = String.Format("Select {0}.{1}, {0}.{2} from {0}", aFieldItem.ComboEntityName, aFieldItem.ComboTextField, aFieldItem.ComboValueField);
                        else if (type == "4")
                            bInfoRefVal.SelectCommand = String.Format("Select {0}.{1}, {0}.{2} from {0}", aFieldItem.ComboEntityName, aFieldItem.ComboTextField, aFieldItem.ComboValueField);
                        else if (type == "5")
                            bInfoRefVal.SelectCommand = String.Format("Select {0}.{1}, {0}.{2} from {0}", aFieldItem.ComboEntityName, aFieldItem.ComboTextField, aFieldItem.ComboValueField);
                    }
                    catch
                    {
                    }
                    bInfoRefVal.ValueMember = aFieldItem.ComboValueField;
                    bInfoRefVal.DisplayMember = aFieldItem.ComboTextField;
                    ComboBoxColumn = FDesignerHost.CreateComponent(typeof(InfoDataGridViewComboBoxColumn), "dgcc" + WzdUtils.RemoveSpecialCharacters(BlockItem.TableName) + aFieldItem.DataField) as InfoDataGridViewComboBoxColumn;
                    ComboBoxColumn.RefValue = bInfoRefVal;
                    ComboBoxColumn.DataPropertyName = aFieldItem.DataField;
                    ComboBoxColumn.HeaderText = aFieldItem.Description;
                    if (ComboBoxColumn.HeaderText.Trim() == "")
                        ComboBoxColumn.HeaderText = aFieldItem.DataField;
                    ComboBoxColumn.DisplayMember = aFieldItem.ComboTextField;
                    ComboBoxColumn.ValueMember = aFieldItem.ComboValueField;
                    Grid.Columns.Add(ComboBoxColumn);
                }
                else
                {
                    Column = FDesignerHost.CreateComponent(typeof(DataGridViewTextBoxColumn), "dgc" + WzdUtils.RemoveSpecialCharacters(BlockItem.TableName) + aFieldItem.DataField) as DataGridViewTextBoxColumn;
                    Column.DataPropertyName = aFieldItem.DataField;
                    Column.HeaderText = aFieldItem.Description;
                    Column.MaxInputLength = aFieldItem.Length;
                    if (Column.HeaderText.Trim() == "")
                        Column.HeaderText = aFieldItem.DataField;
                    Grid.Columns.Add(Column);
                }
                CreateQueryField(aFieldItem, "", aInfoRefVal, MainBindingSource);
                aInfoRefVal = null;
            }
        }

        public String FindSystemDBType(String aliasName)
        {
            String xmlName = SystemFile.DBFile;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlName);

            XmlNode node = xmlDoc.FirstChild.SelectSingleNode(aliasName);

            string DbString = node.FirstChild.Value;
            string systemDBType = FindDBType(DbString);

            return systemDBType;
        }

        private static string GetServerPath()
        {
            String _serverPath = "";
            if (_serverPath.Length == 0)
            {
                _serverPath = EEPRegistry.Server + "\\";
            }
            return _serverPath;
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

        private void SetBlockFieldControls(TBlockItem BlockItem)
        {
            if (BlockItem.Name == "View")
            {
                GenViewBlockControl(BlockItem);
            }
            if (BlockItem.Name == "Main" || BlockItem.Name == "Master")
            {
                if (FClientData.BaseFormName == "CSingle" || FClientData.BaseFormName == "CMasterDetail"
                    || FClientData.BaseFormName == "VBCSingle" || FClientData.BaseFormName == "VBCMasterDetail")
                    GenMainBlockControl(BlockItem);
                if (FClientData.BaseFormName == "CQuery" || FClientData.BaseFormName == "VBCQuery")
                    GenMainBlockControl2(BlockItem);
            }
        }

        private void SetBlockFieldDataComponent(TBlockItem BlockItem, string DataSetName)
        {
            InfoBindingSource aBindingSource = FDesignerHost.CreateComponent(typeof(InfoBindingSource), "ibs" + WzdUtils.RemoveSpecialCharacters(BlockItem.TableName)) as InfoBindingSource;
            if (BlockItem.ParentItemName == "")
            {
                aBindingSource.DataSource = FDataSet;
                aBindingSource.DataMember = BlockItem.TableName;
            }
            else
            {
                TBlockItem ParentItem = ((TBlockItems)BlockItem.Collection).FindItem(BlockItem.ParentItemName);
                aBindingSource.DataSource = ParentItem.BindingSource;
                aBindingSource.DataMember = BlockItem.RelationName;
            }
            BlockItem.BindingSource = aBindingSource;
        }

        private void GenBlock(TBlockItem BlockItem, string DataSetName, bool GenField)
        {
            SetBlockFieldControls(BlockItem);
        }

        private void GenDataSet()
        {
            FDataSet = FDesignerHost.CreateComponent(typeof(InfoDataSet), "id" + WzdUtils.RemoveSpecialCharacters(FClientData.TableName)) as InfoDataSet;
            FDataSet.RemoteName = FClientData.ProviderName;
            FDataSet.Active = true;
            //CliUtils.fCurrentProject = System.IO.Path.GetFileName(FClientData.SolutionName);
            //FKeyFields = FDataSet.GetKeyFields();
        }

        private void GenDetailBindingSource()
        {
            int I;
            TBlockItem BlockItem;
            InfoBindingSource aBindingSource;
            for (I = 0; I < FClientData.Blocks.Count; I++)
            {
                BlockItem = FClientData.Blocks[I] as TBlockItem;
                if (BlockItem.BindingSource == null)
                {
                    aBindingSource = FDesignerHost.CreateComponent(typeof(InfoBindingSource), "ibs" + WzdUtils.RemoveSpecialCharacters(BlockItem.TableName)) as InfoBindingSource;
                    BlockItem.BindingSource = aBindingSource;
                }
            }
        }

        private void FixupMasterDetail(ArrayList DetailList)
        {
            for (int num1 = 0; num1 < FClientData.Blocks.Count; num1++)
            {
                TBlockItem item1 = FClientData.Blocks[num1] as TBlockItem;
                if (item1.BindingSource.DataMember == "")
                {
                    if (item1.ParentItemName == "")
                    {
                        item1.BindingSource.DataMember = item1.TableName;
                        item1.BindingSource.DataSource = item1.BindingSource;
                    }
                    else
                    {
                        TBlockItem item2 = FClientData.Blocks.FindItem(item1.ParentItemName);
                        item1.BindingSource.DataSource = item2.BindingSource;
                        item1.BindingSource.DataMember = item1.RelationName;
                        DetailList.Add(item1);
                    }
                }
            }
        }

        private void GenDetailGrid(TBlockItem BlockItem, Control ParentControl)
        {
            InfoDataGridView Grid = FDesignerHost.CreateComponent(typeof(InfoDataGridView), "grd" + WzdUtils.RemoveSpecialCharacters(BlockItem.TableName)) as InfoDataGridView;
            Grid.Parent = ParentControl;
            Grid.Dock = DockStyle.Fill;
            Grid.DataSource = BlockItem.BindingSource;
            Grid.Columns.Clear();
            TBlockFieldItem aFieldItem;
            DataGridViewTextBoxColumn Column;
            InfoDataGridViewComboBoxColumn ComboBoxColumn;
            InfoDataGridViewRefValColumn RefValColumn;
            int I;
            for (I = 0; I < BlockItem.BlockFieldItems.Count; I++)
            {
                aFieldItem = BlockItem.BlockFieldItems[I] as TBlockFieldItem;
                if ((aFieldItem.RefValNo != null && aFieldItem.RefValNo != "") || aFieldItem.RefField != null)
                {
                    InfoRefVal aInfoRefVal = GenRefVal(aFieldItem, WzdUtils.RemoveSpecialCharacters(BlockItem.TableName));
                    RefValColumn = FDesignerHost.CreateComponent(typeof(InfoDataGridViewRefValColumn), "dgc" + WzdUtils.RemoveSpecialCharacters(BlockItem.TableName) + aFieldItem.DataField) as InfoDataGridViewRefValColumn;
                    RefValColumn.DataPropertyName = aFieldItem.DataField;
                    RefValColumn.DefaultCellStyle.Format = aFieldItem.EditMask;
                    RefValColumn.HeaderText = aFieldItem.Description;
                    RefValColumn.MaxInputLength = aFieldItem.Length;
                    if (RefValColumn.HeaderText.Trim() == "")
                        RefValColumn.HeaderText = aFieldItem.DataField;
                    RefValColumn.RefValue = aInfoRefVal;
                    if ((BlockItem.BlockFieldItems[I] as TBlockFieldItem).DataType == typeof(int) || (BlockItem.BlockFieldItems[I] as TBlockFieldItem).DataType == typeof(float)
                        || (BlockItem.BlockFieldItems[I] as TBlockFieldItem).DataType == typeof(double))
                        RefValColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    Grid.Columns.Add(RefValColumn);
                }
                else if (aFieldItem.ControlType == "ComboBox")
                {
                    string type = FindSystemDBType("SystemDB");

                    InfoRefVal bInfoRefVal = FDesignerHost.CreateComponent(typeof(InfoRefVal), "irv" + WzdUtils.RemoveSpecialCharacters(BlockItem.TableName) + aFieldItem.DataField) as InfoRefVal;
                    bInfoRefVal.SelectAlias = FClientData.Owner.SelectedAlias;
                    if (type == "1")
                        bInfoRefVal.SelectCommand = String.Format("Select [{0}].[{1}], [{0}].[{2}] from [{0}]", aFieldItem.ComboEntityName, aFieldItem.ComboTextField, aFieldItem.ComboValueField);
                    else if (type == "2")
                        bInfoRefVal.SelectCommand = String.Format("Select [{0}].[{1}], [{0}].[{2}] from [{0}]", aFieldItem.ComboEntityName, aFieldItem.ComboTextField, aFieldItem.ComboValueField);
                    else if (type == "3")
                        bInfoRefVal.SelectCommand = String.Format("Select {0}.{1}, {0}.{2} from {0}", aFieldItem.ComboEntityName, aFieldItem.ComboTextField, aFieldItem.ComboValueField);
                    else if (type == "4")
                        bInfoRefVal.SelectCommand = String.Format("Select {0}.{1}, {0}.{2} from {0}", aFieldItem.ComboEntityName, aFieldItem.ComboTextField, aFieldItem.ComboValueField);
                    else if (type == "5")
                        bInfoRefVal.SelectCommand = String.Format("Select {0}.{1}, {0}.{2} from {0}", aFieldItem.ComboEntityName, aFieldItem.ComboTextField, aFieldItem.ComboValueField);
                    bInfoRefVal.ValueMember = aFieldItem.ComboValueField;
                    bInfoRefVal.DisplayMember = aFieldItem.ComboTextField;
                    ComboBoxColumn = FDesignerHost.CreateComponent(typeof(InfoDataGridViewComboBoxColumn), "dgcc" + WzdUtils.RemoveSpecialCharacters(BlockItem.TableName) + aFieldItem.DataField) as InfoDataGridViewComboBoxColumn;
                    ComboBoxColumn.RefValue = bInfoRefVal;
                    ComboBoxColumn.DataPropertyName = aFieldItem.DataField;
                    ComboBoxColumn.DefaultCellStyle.Format = aFieldItem.EditMask;
                    ComboBoxColumn.HeaderText = aFieldItem.Description;
                    if (ComboBoxColumn.HeaderText.Trim() == "")
                        ComboBoxColumn.HeaderText = aFieldItem.DataField;
                    ComboBoxColumn.DisplayMember = aFieldItem.ComboTextField;
                    ComboBoxColumn.ValueMember = aFieldItem.ComboValueField;
                    Grid.Columns.Add(ComboBoxColumn);
                }
                else
                {
                    Column = FDesignerHost.CreateComponent(typeof(DataGridViewTextBoxColumn), "dgc" + WzdUtils.RemoveSpecialCharacters(BlockItem.TableName) + aFieldItem.DataField) as DataGridViewTextBoxColumn;
                    Column.DataPropertyName = aFieldItem.DataField;
                    Column.DefaultCellStyle.Format = aFieldItem.EditMask;
                    Column.HeaderText = aFieldItem.Description;
                    Column.MaxInputLength = aFieldItem.Length;
                    if (Column.HeaderText.Trim() == "")
                        Column.HeaderText = aFieldItem.DataField;
                    if ((BlockItem.BlockFieldItems[I] as TBlockFieldItem).DataType == typeof(int) || (BlockItem.BlockFieldItems[I] as TBlockFieldItem).DataType == typeof(float)
                        || (BlockItem.BlockFieldItems[I] as TBlockFieldItem).DataType == typeof(double))
                        Column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    Grid.Columns.Add(Column);
                }
            }

            /*
            DataGridViewColumn Column;
            TBlockFieldItem FieldItem;
            int I, Index;
            for (I = 0; I < BlockItem.BlockFieldItems.Count; I++)
            {
                FieldItem = BlockItem.BlockFieldItems[I] as TBlockFieldItem;
                if (FieldItem.Description == "")
                    Index = Grid.Columns.Add(FieldItem.DataField, FieldItem.DataField);
                else
                    Index = Grid.Columns.Add(FieldItem.DataField, FieldItem.Description);
                Column = Grid.Columns[Index];
                Column.DataPropertyName = FieldItem.DataField;
                Column.Name = "dgc" + FieldItem.DataField;
                Column.HeaderText = FieldItem.Description;
                if (Column.HeaderText.Trim() == "")
                    Column.HeaderText = FieldItem.DataField;
            }
             */
            /*
            for (int num1 = 0; num1 < Grid.Columns.Count; num1++)
            {
                DataGridViewColumn column1 = Grid.Columns[num1];
                TBlockFieldItem item1 = BlockItem.BlockFieldItems.FindItem(column1.DataPropertyName);
                if (item1 == null)
                {
                    Grid.Columns.Remove(column1);
                }
            }
             */
        }

        private void GenDetailBlockControl(ArrayList DetailList)
        {
            SplitContainer scMaster = FRootForm.Controls["scMaster"] as SplitContainer;
            SplitContainer scDetail = scMaster.Panel2.Controls["scDetail"] as SplitContainer;
            if (DetailList.Count == 1)
            {
                GenDetailGrid(DetailList[0] as TBlockItem, scDetail.Panel2);
            }
            else
            {
                TabControl control1 = FDesignerHost.CreateComponent(typeof(TabControl)) as TabControl;
                control1.Parent = scDetail.Panel2;
                control1.Dock = DockStyle.Fill;
                for (int num1 = 0; num1 < DetailList.Count; num1++)
                {
                    TabPage page1 = FDesignerHost.CreateComponent(typeof(TabPage)) as TabPage;
                    page1.Text = ((TBlockItem)DetailList[num1]).TableName;
                    control1.TabPages.Add(page1);
                    GenDetailGrid((TBlockItem)DetailList[num1], page1);
                }
            }
        }

        private void GenDetailBlock()
        {
            ArrayList DetailList = new ArrayList();
            GenDetailBindingSource();
            FixupMasterDetail(DetailList);
            GenDetailBlockControl(DetailList);
        }

        private void RenameForm()
        {
            string Path = GlobalPI.get_FileNames(0);
            Path = System.IO.Path.GetDirectoryName(Path);
            string NewName = FClientData.FormName + ".cs";
            string FileName = Path + @"\" + NewName;
            GlobalPI.SaveAs(FileName);
            System.IO.File.Delete(Path + @"\Form1.cs");
            System.IO.File.Delete(Path + @"\Form1.Designer.cs");
            System.IO.File.Delete(Path + @"\Form1.resx");
        }

        private void UpdateDataSource(TBlockItem MainBlockItem, TBlockItem ViewBlockItem)
        {
            InfoNavigator navigator1 = FRootForm.Controls["infoNavigator1"] as InfoNavigator;
            //if (navigator1 != null)
            //    navigator1.ViewBindingSource = MainBlockItem.BindingSource;
            if (FViewGrid != null)
            {
                FViewGrid.DataSource = MainBlockItem.BindingSource;
                TBlockFieldItem FieldItem;
                FViewGrid.Columns.Clear();
                DataGridViewTextBoxColumn Column;
                int I;
                for (I = 0; I < ViewBlockItem.BlockFieldItems.Count; I++)
                {
                    FieldItem = ViewBlockItem.BlockFieldItems[I] as TBlockFieldItem;
                    Column = FDesignerHost.CreateComponent(typeof(DataGridViewTextBoxColumn), "dgc" + WzdUtils.RemoveSpecialCharacters(MainBlockItem.TableName) + FieldItem.DataField) as DataGridViewTextBoxColumn;
                    Column.DataPropertyName = FieldItem.DataField;
                    Column.HeaderText = FieldItem.Description;
                    Column.MaxInputLength = FieldItem.Length;
                    if (Column.HeaderText.Trim() == "")
                        Column.HeaderText = FieldItem.DataField;
                    if (FieldItem.DataType == typeof(int) || FieldItem.DataType == typeof(float) || FieldItem.DataType == typeof(double))
                        Column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    FViewGrid.Columns.Add(Column);
                }
            }

            /*
            int I, Index;
            for (I = 0; I < ViewBlockItem.BlockFieldItems.Count; I++)
            {
                FieldItem = ViewBlockItem.BlockFieldItems[I] as TBlockFieldItem;
                if (FieldItem.Description == "")
                    Index = FViewGrid.Columns.Add(FieldItem.DataField, FieldItem.DataField);
                else
                    Index = FViewGrid.Columns.Add(FieldItem.DataField, FieldItem.Description);
                Column = FViewGrid.Columns[Index];
                Column.DataPropertyName = FieldItem.DataField;
                Column.Name = "dgc" + FieldItem.DataField;
                Column.HeaderText = FieldItem.Description;
                if (Column.HeaderText.Trim() == "")
                    Column.HeaderText = FieldItem.DataField;
            }
            IComponentChangeService FComponentChangeService = (IComponentChangeService)FDesignerHost.RootComponent.Site.GetService(typeof(IComponentChangeService));
            FComponentChangeService.OnComponentChanged(FViewGrid, null, "", "M");
            */
        }

        public void GenDefaultValidate()
        {
            foreach (TBlockItem BlockItem in FClientData.Blocks)
            {
                if (String.Compare(BlockItem.Name, "View") == 0)
                    continue;

                DefaultValidate aValidate = null;
                int count = 1;
                foreach (TBlockFieldItem aFieldItem in BlockItem.BlockFieldItems)
                {
                    if (aFieldItem.CheckNull != null && aFieldItem.CheckNull.ToUpper() == "Y" || (aFieldItem.DefaultValue != "" && aFieldItem.DefaultValue != null))
                    {
                        if (aValidate == null)
                        {
                            aValidate = FDesignerHost.CreateComponent(typeof(DefaultValidate), "dv" + WzdUtils.RemoveSpecialCharacters(BlockItem.TableName)) as DefaultValidate;
                            aValidate.BindingSource = BlockItem.BindingSource;
                        }
                        FieldItem FI = new FieldItem();
                        FI.FieldName = aFieldItem.DataField;
                        FI.CheckNull = aFieldItem.CheckNull.ToUpper() == "Y";
                        FI.DefaultValue = aFieldItem.DefaultValue;
                        if (!aValidate.BindingSource.Site.Name.EndsWith("Details"))
                            FI.ValidateLabelLink = "label" + count;
                        aValidate.FieldItems.Add(FI);
                    }
                    count++;
                }
            }
        }

        public void GenClientModule()
        {
            if (GenSolution())
            {
                GetForm();
                DesignerTransaction transaction1 = FDesignerHost.CreateTransaction();
                try
                {
                    TBlockItem BlockItem;
                    GenDataSet();
                    if (FClientData.IsMasterDetailBaseForm())
                    {
                        BlockItem = FClientData.Blocks.FindItem("View");
                        if (BlockItem != null)
                        {
                            GenBlock(BlockItem, "View", false);
                        }
                        BlockItem = FClientData.Blocks.FindItem("Master");
                        if (BlockItem != null)
                        {
                            GenBlock(BlockItem, "Master", false);
                        }
                        GenDetailBlock();
                    }
                    else
                    {
                        BlockItem = FClientData.Blocks.FindItem("View");
                        if (BlockItem != null)
                        {
                            GenBlock(BlockItem, "Main", false);
                        }
                        TBlockItem MainBlockItem = FClientData.Blocks.FindItem("Main");
                        if (MainBlockItem != null)
                        {
                            GenBlock(MainBlockItem, "Main", false);
                            UpdateDataSource(MainBlockItem, BlockItem);
                        }
                    }
                    GenDefaultValidate();
                    GlobalProject.Save(GlobalProject.FullName);
                    FDTE2.Solution.SolutionBuild.BuildProject(FDTE2.Solution.SolutionBuild.ActiveConfiguration.Name,
                        GlobalProject.FullName, true);
                }
                catch (Exception exception2)
                {
                    MessageBox.Show(exception2.Message);
                    return;
                }
                finally
                {
                    transaction1.Commit();
                }
                //RenameForm();
                //???GlobalPI.Save(GlobalPI.get_FileNames(0));
                //GlobalWindow.Close(vsSaveChanges.vsSaveChangesYes);
            }
        }
    }
}