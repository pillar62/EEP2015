using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using EnvDTE;
using EnvDTE80;
using System.IO;
using Srvtools;
using System.Data.Common;
using System.Reflection;
using System.Xml;
using System.Runtime.InteropServices;
//using System.Web.UI.WebControls;
using AjaxTools;
#if VS90
using WebDevPage = Microsoft.VisualWebDeveloper.Interop.WebDeveloperPage;
#endif


namespace MWizard2015
{
    public partial class fmRDLCWizard : Form
    {
        private TRDLCWizardData FClientData;
        private DTE2 FDTE2;
        private AddIn FAddIn;
        private DbConnection InternalConnection = null;
        private TStringList FAlias;
        private InfoDataSet FInfoDataSet = null;
        private string[] FProviderNameList;
        public Boolean SDCall = false;
        private ListViewColumnSorter lvwColumnSorterViewSrc;
        private ListViewColumnSorter lvwColumnSorterViewDes;
        private ListViewColumnSorter lvwColumnSorterMasterSrc;
        private ListViewColumnSorter lvwColumnSorterMasterDes;
        private ListViewColumnSorter lvwColumnSorterDetail;

        public fmRDLCWizard(DTE2 aDTE2, AddIn aAddIn)
        {
            InitializeComponent();
            FClientData = new TRDLCWizardData(this);
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

        public String SelectedAlias
        {
            get { return cbEEPAlias.Text; }
        }

        private void ClearValues()
        {
            SDCall = false;
            cbWebSite.Items.Clear();
            cbWebSite.Text = "";
            tbCurrentSolution.Text = FDTE2.Solution.FileName;
            if (tbCurrentSolution.Text != "")
            {
                rbCurrentSolution.Enabled = true;
                rbCurrentSolution.Checked = true;
                rbAddToExistSolution.Checked = false;
                tbSolutionName.Text = "";
                GetWebSite();
            }
            else
            {
                rbCurrentSolution.Enabled = false;
                rbAddToExistSolution.Checked = true;
            }
            tbSolutionName.Text = "";
            cbAddToExistFolder.Items.Clear();
            cbAddToExistFolder.Text = "";
            tbAddToNewFolder.Text = "";
            rbAddToRootFolder_CheckedChanged(rbAddToRootFolder, null);
            tbTableName.Text = "";
            tbTableNameF.Text = "";
            tbProviderName.Text = "";
            tbFormName.Text = "Form1";
            tbDetailTableName.Text = "";
            cbViewProviderName.Items.Clear();
            cbViewProviderName.Text = "";
            cbWebForm.Text = "SingleLabel";
            ClearAll();
        }

        private void ClearAll()
        {
            lvViewSrcField.Items.Clear();
            lvViewDesField.Items.Clear();
            tbCaption.Text = "";
            tbWidth.Text = "";
            cbIsGroup.Checked = false;
            lvMasterDesField.Items.Clear();
            lvMasterSrcField.Items.Clear();
            FClientData.Blocks.Clear();
            tvRelation.Nodes.Clear();
            tbCaption_D.Text = "";
            tbWidth_D.Text = "";
            cbIsGroup_D.Checked = false;
            lvSelectedFields.Items.Clear();
        }

        private void Init()
        {
            ClearValues();
            LoadDBString();
            FInfoDataSet = new InfoDataSet();
            if (((FDTE2 != null) && (FDTE2.Solution.FileName != "")) && File.Exists(FDTE2.Solution.FileName))
            {
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


        private void LoadDBString()
        {
            try
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
            catch (Exception ex)
            {
                MessageBox.Show("Please setup <DB Manager> of EEPNetServer at first !");
            }
        }

        public void ShowRDLCWizard()
        {
            //Show();
            Init();
            ShowDialog();
        }

        private void SetFieldNames(String TableName, ListView LV)
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
                    //aBlockFieldItem.EditMask = DR["EDITMASK"].ToString();
                    if (aBlockFieldItem.DataType == typeof(DateTime))
                    {
                        if (aBlockFieldItem.ControlType == null || aBlockFieldItem.ControlType == "")
                            aBlockFieldItem.ControlType = "DateTimeBox";
                    }
                    else if (aBlockFieldItem.DataType == typeof(Int16) || aBlockFieldItem.DataType == typeof(Int32)
                            || aBlockFieldItem.DataType == typeof(Int64) || aBlockFieldItem.DataType == typeof(float)
                            || aBlockFieldItem.DataType == typeof(double) || aBlockFieldItem.DataType == typeof(decimal))
                    {
                        if (aBlockFieldItem.ControlType == null || aBlockFieldItem.ControlType == "")
                            aBlockFieldItem.ControlType = "NumberBox";
                    }
                    aBlockFieldItem.QueryMode = DR["QUERYMODE"].ToString();
                    if (DR["FIELD_LENGTH"] != null && DR["FIELD_LENGTH"].ToString() != "")
                        aBlockFieldItem.Length = Convert.ToInt32(DR["FIELD_LENGTH"]);
                    if (DR["IS_KEY"] != null && DR["IS_KEY"].ToString() == "Y")
                        aBlockFieldItem.IsKey = true;
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
                    TBlockFieldItem aFieldItem = new TBlockFieldItem();
                    if (DRS.Length > 0)
                    {
                        aItem.SubItems.Add(DRS[0]["CAPTION"].ToString());
                        aFieldItem.Description = DRS[0]["CAPTION"].ToString();
                        aFieldItem.QueryMode = DRS[0]["QUERYMODE"].ToString();
                        if (DRS[0]["IS_KEY"] != null && DRS[0]["IS_KEY"].ToString() == "Y")
                            aFieldItem.IsKey = true;
                    }
                    else
                    {
                        aItem.SubItems.Add("");
                        aFieldItem.Description = "";
                    }
                    aListView.Items.Add(aItem);
                    aFieldItem.DataField = Column.ColumnName;
                    aFieldItem.DataType = Column.DataType;
                    if (DRS.Length > 0 && DRS[0]["CAPTION"] != null)
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

                if (cbChooseLanguage.Text == "" || cbChooseLanguage.Text == "C#")
                    FClientData.Language = "cs";
                else if (cbChooseLanguage.Text == "VB")
                    FClientData.Language = "vb";

                if (cbWebSite.Items.Count == 1)
                {
                    cbWebSite.SelectedIndex = 0;
                    cbWebSite_SelectedIndexChanged(new object(), new EventArgs());
                }

                DisplayPage(tpOutputSetting);
            }
            else if (tabControl.SelectedTab.Equals(tpOutputSetting))
            {
                FClientData.FolderName = "";
                if (rbAddToExistSolution.Checked && tbSolutionName.Text == "")
                {
                    tbSolutionName.Focus();
                    MessageBox.Show("Please input SolutionName");
                }
                else if (cbWebSite.Text == "")
                {
                    cbWebSite.Focus();
                    MessageBox.Show("Please select a WebSite");
                }
                else if (rbAddToExistFolder.Checked && (cbAddToExistFolder.Text == ""))
                {
                    cbAddToExistFolder.Focus();
                    MessageBox.Show("Please select a exist folder");
                }
                else if (rbCurrentSolution.Checked && (tbCurrentSolution.Text == ""))
                {
                    MessageBox.Show("The IDE's Solution is empty");
                }
                else if (rbAddToNewFolder.Checked && (tbAddToNewFolder.Text == ""))
                {
                    tbAddToNewFolder.Focus();
                    MessageBox.Show("Please input new folder");
                }
                else
                {
                    if (rbAddToExistFolder.Checked)
                    {
                        FClientData.FolderName = cbAddToExistFolder.Text;
                        FClientData.FolderMode = "ExistFolder";
                    }
                    else if (rbAddToNewFolder.Checked)
                    {
                        if (cbAddToExistFolder.Items.Contains(tbAddToNewFolder.Text))
                        {
                            MessageBox.Show("The folder name you typed has already existed.");
                            return;
                        }
                        FClientData.FolderName = tbAddToNewFolder.Text;
                        FClientData.FolderMode = "NewFolder";
                    }
                    if (rbCurrentSolution.Checked)
                    {
                        FClientData.SolutionName = tbCurrentSolution.Text;
                    }
                    if (rbAddToExistSolution.Checked)
                    {
                        FClientData.SolutionName = tbSolutionName.Text;
                    }
                    FClientData.WebSiteName = cbWebSite.Text;
                    FClientData.WebSiteFullName = cbWebSite.Tag != null ? cbWebSite.Tag.ToString() : cbWebSite.Text;
                    tbProviderName.Text = "";
                    DisplayPage(tpFormSetting);
                }

                if (this.cbChooseLanguage.SelectedItem != null)
                {
                    switch (this.cbChooseLanguage.SelectedItem.ToString())
                    {
                        case "C#":
                            this.cbWebForm.Items.Clear();
                            this.cbWebForm.Items.Add("SingleLabel");
                            this.cbWebForm.Items.Add("SingleTable");
                            this.cbWebForm.Items.Add("MasterDetail");
                            this.cbWebForm.SelectedIndex = 0;
                            break;
                    }
                }
                else
                {
                    this.cbWebForm.Items.Clear();
                    this.cbWebForm.Items.Add("SingleLabel");
                    this.cbWebForm.Items.Add("SingleTable");
                    this.cbWebForm.Items.Add("MasterDetail");
                    this.cbWebForm.SelectedIndex = 0;
                }
            }
            else if (tabControl.SelectedTab.Equals(tpFormSetting))
            {
                if (cbWebForm.Text == "")
                {
                    MessageBox.Show("Please select EEP Web Templates Form !!");
                    if (cbWebForm.CanFocus)
                    {
                        cbWebForm.Focus();
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
                    FClientData.FormTitle = tbFormTitle.Text;
                    FClientData.BaseFormName = cbWebForm.Text;
                    cbViewProviderName.Visible = (FClientData.BaseFormName.CompareTo("WMasterDetail3") == 0 || FClientData.BaseFormName.CompareTo("VBWebCMasterDetail_VFG") == 0 || FClientData.BaseFormName.CompareTo("WMasterDetail8") == 0 || FClientData.BaseFormName.CompareTo("VBWebCMasterDetail8") == 0);
                    label18.Visible = cbViewProviderName.Visible;
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
                    FClientData.RealTableName = tbTableNameF.Text;
                    FClientData.BaseFormName = cbWebForm.Text;
                    if (lvMasterSrcField.Items.Count == 0 && lvMasterDesField.Items.Count == 0)
                        SetFieldNames(FClientData.RealTableName, lvMasterSrcField);
                    if (FClientData.BaseFormName == "WMasterDetail3" || FClientData.BaseFormName == "VBWebCMasterDetail_VFG" || FClientData.BaseFormName == "WMasterDetail8"
                        || FClientData.BaseFormName == "WSingle2" || FClientData.BaseFormName == "WSingle3" || FClientData.BaseFormName == "WSingle4"
                        || FClientData.BaseFormName == "WSingle5" || FClientData.BaseFormName == "VBWebCMasterDetail8" || FClientData.BaseFormName == "VBWebSingle5")
                    {
                        if (lvViewSrcField.Items.Count == 0 && lvViewDesField.Items.Count == 0)
                        {
                            if (cbViewProviderName.Visible)
                                SetFieldNamesByProvider(FClientData.RealTableName, FClientData.ViewProviderName, lvViewSrcField);
                            else
                                SetFieldNames(FClientData.RealTableName, lvViewSrcField);
                        }
                        DisplayPage(tpViewFields);
                    }
                    else
                    {
                        DisplayPage(tpMasterFields);
                    }
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
                    if (FClientData.BaseFormName == "WMasterDetail3" || FClientData.BaseFormName == "VBWebCMasterDetail_VFG"
                        || FClientData.BaseFormName == "WMasterDetail8" || FClientData.BaseFormName == "VBWebCMasterDetail8")
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ClearAllListViewSort();

            FInfoDataSet.Dispose();
            FInfoDataSet = null;
            Hide();
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
                        System.Windows.Forms.Button B = new System.Windows.Forms.Button();
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
                                ((System.Windows.Forms.Button)LVSI.Tag).Dispose();
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
                        RearrangeRefValButton((System.Windows.Forms.Button)LVSI.Tag, LVSI.Bounds);
                }
            }
        }

        public delegate void RearrangeRefValButtonFunc(System.Windows.Forms.Button B, Rectangle Bounds);

        private void RearrangeRefValButton(System.Windows.Forms.Button B, Rectangle Bounds)
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

        private void AddDetailBlockItem(string MasterItemName, System.Windows.Forms.TreeNodeCollection NodeCollection, ListView LV)
        {
            for (int I = 0; I < NodeCollection.Count; I++)
            {
                TBlockItem BlockItem = new TBlockItem();
                BlockItem.Name = NodeCollection[I].Text;
                BlockItem.TableName = ((TDetailItem)NodeCollection[I].Tag).TableName;
                BlockItem.Relation = ((TDetailItem)NodeCollection[I].Tag).Relation;
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
            FClientData.FormName = tbFormName.Text;
            FClientData.TableName = tbTableName.Text;
            FClientData.RealTableName = tbTableNameF.Text;
            FClientData.ViewProviderName = cbViewProviderName.Text;
            TRDLCWizardGenerator Generator = new TRDLCWizardGenerator(FClientData, FDTE2, FAddIn);
            Generator.GenWebClientModule();
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
            ClearAllListViewSort();
            SetValue();
            SetValue_D();

            if (FClientData.IsMasterDetailBaseForm())
            {
                if (FClientData.BaseFormName == "DEMasterDetail2" || FClientData.BaseFormName == "DEMasterDetail2_VB")
                    AddBlockItem("View", FClientData.ProviderName, FClientData.TableName, lvViewDesField);
                AddBlockItem("Master", FClientData.ProviderName, FClientData.TableName, lvMasterDesField);
                AddDetailBlockItem("Master", tvRelation.Nodes, lvSelectedFields);
            }
            else
            {
                if (FClientData.BaseFormName == "DESingle2" || FClientData.BaseFormName == "DESingle2_VB")
                    AddBlockItem("View", FClientData.ProviderName, FClientData.TableName, lvViewDesField);
                AddBlockItem("Main", FClientData.ProviderName, FClientData.TableName, lvMasterDesField);
            }
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
            String OWNER = String.Empty, SS = TableName;
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
                if (aItem.Tag != null)
                {
                    BlockFieldItem.DataField = ((TBlockFieldItem)aItem.Tag).DataField;
                    BlockFieldItem.CheckNull = ((TBlockFieldItem)aItem.Tag).CheckNull;
                    BlockFieldItem.DefaultValue = ((TBlockFieldItem)aItem.Tag).DefaultValue;
                    BlockFieldItem.Description = ((TBlockFieldItem)aItem.Tag).Description;
                    BlockFieldItem.RefValNo = ((TBlockFieldItem)aItem.Tag).RefValNo;
                    BlockFieldItem.ControlType = ((TBlockFieldItem)aItem.Tag).ControlType;
                    BlockFieldItem.ComboEntityName = ((TBlockFieldItem)aItem.Tag).ComboEntityName;
                    BlockFieldItem.ComboRemoteName = ((TBlockFieldItem)aItem.Tag).ComboRemoteName;
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

        private void GetWebSite()
        {
            cbWebSite.Items.Clear();
            foreach (Project P in FDTE2.Solution.Projects)
            {
                if (string.Compare(P.Kind, "{E24C65DC-7377-472b-9ABA-BC803B73C61A}") == 0)
                {
                    cbWebSite.Items.Add(P.Name);
                }
            }

            if (cbWebSite.Items.Count == 1)
            {
                cbWebSite.SelectedIndex = 0;
                cbWebSite_SelectedIndexChanged(new object(), new EventArgs());
            }
        }

        private void btnSolutionName_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                tbSolutionName.Text = openFileDialog.FileName;
                if (string.Compare(tbSolutionName.Text, FDTE2.Solution.FullName) != 0)
                {
                    FDTE2.Solution.Open(tbSolutionName.Text);
                }
                GetWebSite();
            }
        }

        private void EnabledOutputControls()
        {
        }
        private void ShowChildRelation(DataRelationCollection Relations, System.Windows.Forms.TreeNode Node)
        {
            System.Windows.Forms.TreeNode ChildNode;
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
                ChildNode = new System.Windows.Forms.TreeNode();
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
            System.Windows.Forms.TreeNode Node;
            InfoBindingSource IBS;
            if (aBindingSource.DataSource.GetType().Equals(typeof(InfoDataSet)))
            {
                InfoDataSet set1 = (InfoDataSet)aBindingSource.DataSource;
                for (int I = 0; I < set1.RealDataSet.Tables[0].ChildRelations.Count; I++)
                {
                    R1 = set1.RealDataSet.Tables[0].ChildRelations[I];
                    Node = new System.Windows.Forms.TreeNode();
                    Node.Text = R1.ChildTable.TableName;
                    Node.Name = R1.ChildTable.TableName;
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
                            Node = new System.Windows.Forms.TreeNode();
                            Node.Text = R1.ChildTable.TableName;
                            Node.Name = R1.ChildTable.TableName;
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
        }
        private void SetNodeData(DataRelation Relation, InfoBindingSource BindingSource, System.Windows.Forms.TreeNode Node)
        {
            TDetailItem DetailItem = new TDetailItem();
            DetailItem.BindingSource = BindingSource;
            DetailItem.Relation = Relation;
            DetailItem.TableName = Relation.ChildTable.TableName;
            String ModuleName = tbProviderName.Text;
            ModuleName = ModuleName.Substring(0, ModuleName.IndexOf('.'));
            String SolutionName = System.IO.Path.GetFileNameWithoutExtension(FClientData.SolutionName);
            DetailItem.RealTableName = CliUtils.GetTableName(ModuleName, DetailItem.TableName, SolutionName, "", true);
            Node.Tag = DetailItem;
            tvRelation.SelectedNode = Node;
        }

        private void UpdatelvSelectedFields(TDetailItem DetailItem)
        {
            lvSelectedFields.BeginUpdate();
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
                }
            }
            finally
            {
                lvSelectedFields.EndUpdate();
            }
        }

        private void btnNewField_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.TreeNode Node = tvRelation.SelectedNode;
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
            System.Windows.Forms.TreeNode Node = tvRelation.SelectedNode;
            if (Node == null)
                return;
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
                        FieldItem.ControlType = DRs[0]["NEEDBOX"].ToString();
                        FieldItem.EditMask = DRs[0]["EDITMASK"].ToString();
                        if (DRs[0]["FIELD_LENGTH"] != null && DRs[0]["FIELD_LENGTH"].ToString() != String.Empty)
                            FieldItem.Length = int.Parse(DRs[0]["FIELD_LENGTH"].ToString());
                        if (DRs[0]["IS_KEY"] != null && DRs[0]["IS_KEY"].ToString() == "Y")
                            FieldItem.IsKey = true;
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
                InternalConnection = WzdUtils.AllocateConnection(FClientData.DatabaseName, FClientData.DatabaseType, false);
                this.FClientData.ConnString = InternalConnection.ConnectionString;
            }
            else
            {
                if (InternalConnection.State == ConnectionState.Open)
                    InternalConnection.Close();
                InternalConnection = WzdUtils.AllocateConnection(FClientData.DatabaseName, FClientData.DatabaseType, false);
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
        }

        private void btnDeleteField_Click(object sender, EventArgs e)
        {
            try
            {
                System.Windows.Forms.TreeNode Node = tvRelation.SelectedNode;
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
                        ListViewItem.ListViewSubItem LVSI = item2.SubItems[2];
                        if (LVSI.Tag != null)
                            ((System.Windows.Forms.Button)LVSI.Tag).Dispose();
                        DetailItem.BlockFieldItems.Remove(item3);
                        lvSelectedFields.Items.Remove(item2);
                    }
                }

                //foreach (ListViewItem LVI in lvSelectedFields.Items)
                //{
                //    ListViewItem.ListViewSubItem LVSI = LVI.SubItems[2];
                //    if (LVSI.Tag != null)
                //        RearrangeRefValButton((System.Windows.Forms.Button)LVSI.Tag, LVSI.Bounds);
                //}
            }
            catch (Exception exception1)
            {
                MessageBox.Show(exception1.Message);
            }
        }

        private void tvRelation_AfterSelect(object sender, TreeViewEventArgs e)
        {
            System.Windows.Forms.TreeNode Node = tvRelation.SelectedNode;
            //btnNewSubDataset.Enabled = Node != null;
            //btnDeleteDataset.Enabled = btnNewSubDataset.Enabled;
            //btnNewField.Enabled = btnNewSubDataset.Enabled;
            UpdatelvSelectedFields((TDetailItem)Node.Tag);
        }

        private void EnableFolderControl()
        {
            cbAddToExistFolder.Enabled = rbAddToExistFolder.Checked;
            tbAddToNewFolder.Enabled = rbAddToNewFolder.Checked;
        }

        private void rbAddToRootFolder_CheckedChanged(object sender, EventArgs e)
        {
            EnableFolderControl();
        }

        private void rbAddToExistFolder_CheckedChanged(object sender, EventArgs e)
        {
            EnableFolderControl();
        }

        private void rbAddToNewFolder_CheckedChanged(object sender, EventArgs e)
        {
            EnableFolderControl();
        }

        private void cbWebSite_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbAddToExistFolder.Items.Clear();
            foreach (Project P in FDTE2.Solution.Projects)
            {
                if (string.Compare(P.Name, cbWebSite.Text) == 0)
                {
                    cbWebSite.Tag = P.FullName;
                    foreach (ProjectItem PI in P.ProjectItems)
                    {
                        if (string.Compare(PI.Kind, "{6BB5F8EF-4483-11D3-8BCF-00C04F8EC28C}") == 0)
                        {
                            cbAddToExistFolder.Items.Add(PI.Name);
                        }
                    }
                }
            }
            if (cbAddToExistFolder.Items.Count > 0)
            {
                rbAddToExistFolder.Checked = true;
                rbAddToExistFolder_CheckedChanged(rbAddToExistFolder, null);
            }
            else
            {
                rbAddToNewFolder.Checked = true;
                rbAddToNewFolder_CheckedChanged(rbAddToNewFolder, null);
            }
        }

        private void btnProviderName_Click(object sender, EventArgs e)
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
            FInfoDataSet.ClearWhere();
            FInfoDataSet.SetWhere("1=0");
            FInfoDataSet.Active = true;
            tbTableName.Text = FInfoDataSet.RealDataSet.Tables[0].TableName;
            String DataSetName = FInfoDataSet.RealDataSet.Tables[0].TableName;
            String ModuleName = FInfoDataSet.RemoteName.Substring(0, FInfoDataSet.RemoteName.IndexOf('.'));
            String SolutionName = System.IO.Path.GetFileNameWithoutExtension(FDTE2.Solution.FullName);
            tbTableNameF.Text = CliUtils.GetTableName(ModuleName, DataSetName, SolutionName, "", true);
            tbTableNameF.Text = WzdUtils.RemoveQuote(tbTableNameF.Text, FClientData.DatabaseType);
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

        private void SolutionCheckedChange()
        {
            if (rbCurrentSolution.Checked)
            {
                tbSolutionName.Enabled = false;
                btnSolutionName.Enabled = false;
            }
            if (rbAddToExistSolution.Checked)
            {
                tbSolutionName.Enabled = true;
                btnSolutionName.Enabled = true;
            }
        }

        private void rbCurrentSolution_CheckedChanged(object sender, EventArgs e)
        {
            SolutionCheckedChange();
        }

        private void rbAddToExistSolution_CheckedChanged(object sender, EventArgs e)
        {
            SolutionCheckedChange();
        }

        private void btnViewAdd_Click(object sender, EventArgs e)
        {
            SelectFields(lvViewSrcField, lvViewDesField, false);
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

            fmFieldSetting aForm = new fmFieldSetting(InternalConnection, FClientData.DatabaseType, aViewItem.ListView, TWizardType.wtWebPage, FClientData.DatabaseName);
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
                    //BlockFieldItem.ComboEntityName = Params[5];
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

        private void lvMasterDesField_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
        {
            //ListView LV = (ListView)sender;
            //foreach (ListViewItem LVI in LV.Items)
            //{
            //    ListViewItem.ListViewSubItem LVSI = LVI.SubItems[2];
            //    if (LVSI.Tag != null)
            //    {
            //        System.Windows.Forms.Button B = (System.Windows.Forms.Button)LVSI.Tag;
            //        RearrangeRefValButton(B, LVSI.Bounds);
            //    }
            //}
        }

        private void cbDatabaseType_SelectedIndexChanged(object sender, EventArgs e)
        {
            FClientData.DatabaseType = (ClientType)cbDatabaseType.SelectedIndex;
        }

        private void tbFormName_TextChanged(object sender, EventArgs e)
        {
            tbFormTitle.Text = tbFormName.Text;
        }

        private void cbViewProviderName_SelectedIndexChanged(object sender, EventArgs e)
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

        private void btnViewUp_Click(object sender, EventArgs e)
        {
            WzdUtils.SelectedListViewItemUp(lvViewDesField);
        }

        private void btnViewDown_Click(object sender, EventArgs e)
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

        private void cbWebForm_SelectedIndexChanged(object sender, EventArgs e)
        {
            String templateName = (sender as ComboBox).Text;
            switch (templateName)
            {
                case "SingleLabel":
                    this.label16.Text = templateName + ": ";
                    break;
                case "SingleTable":
                    this.label16.Text = templateName + ": ";
                    break;
                case "MasterDetail":
                    this.label16.Text = templateName + ": ";
                    break;
            }
        }

        private TBlockFieldItem FSelectedBlockFieldItem;
        private ListViewItem FSelectedListViewItem;
        private Boolean FDisplayValue = false;
        private void lvMasterDesField_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvMasterDesField.SelectedItems.Count == 1)
            {
                if (!FDisplayValue)
                    SetValue();

                ListViewItem aViewItem = lvMasterDesField.SelectedItems[0];
                FSelectedListViewItem = aViewItem;
                FSelectedBlockFieldItem = (TBlockFieldItem)aViewItem.Tag;
                if (FClientData.BaseFormName == "SingleLabel")
                {
                    if (String.IsNullOrEmpty(FSelectedBlockFieldItem.EditMask))
                        FSelectedBlockFieldItem.EditMask = "Left";
                }
                else if (FClientData.BaseFormName == "SingleTable")
                {
                    if (String.IsNullOrEmpty(FSelectedBlockFieldItem.EditMask))
                    {
                        if (FSelectedBlockFieldItem.DataType == typeof(int)
                            || FSelectedBlockFieldItem.DataType == typeof(float)
                            || FSelectedBlockFieldItem.DataType == typeof(double)
                            || FSelectedBlockFieldItem.DataType == typeof(decimal))
                            FSelectedBlockFieldItem.EditMask = "Right";
                        else
                            FSelectedBlockFieldItem.EditMask = "Left";
                    }
                }
                else if (FClientData.BaseFormName == "MasterDetail")
                {
                    if (String.IsNullOrEmpty(FSelectedBlockFieldItem.EditMask))
                        FSelectedBlockFieldItem.EditMask = "Left";
                }
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
            int width = 0;
            int.TryParse(tbWidth.Text, out width);
            FSelectedBlockFieldItem.Length = width;
            FSelectedBlockFieldItem.CheckNull = cbIsGroup.Checked.ToString();
            FSelectedBlockFieldItem.EditMask = cbTextAlign.Text;
        }

        private void DisplayValue()
        {
            if (FSelectedBlockFieldItem == null)
                return;
            tbCaption.Text = FSelectedBlockFieldItem.Description;
            tbWidth.Text = FSelectedBlockFieldItem.Length.ToString();
            if (!String.IsNullOrEmpty(FSelectedBlockFieldItem.CheckNull) && FSelectedBlockFieldItem.CheckNull.ToLower() == "true")
                cbIsGroup.Checked = true;
            else
                cbIsGroup.Checked = false;
            cbTextAlign.Text = FSelectedBlockFieldItem.EditMask;
        }

        private TBlockFieldItem FSelectedBlockFieldItem_D;
        private ListViewItem FSelectedListViewItem_D;
        private Boolean FDisplayValue_D = false;
        private void lvSelectedFields_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvSelectedFields.SelectedItems.Count == 1)
            {
                if (!FDisplayValue_D)
                    SetValue_D();

                ListViewItem aViewItem = lvSelectedFields.SelectedItems[0];
                FSelectedListViewItem_D = aViewItem;
                FSelectedBlockFieldItem_D = (TBlockFieldItem)aViewItem.Tag;
                if (FClientData.BaseFormName == "SingleLabel")
                {
                    if (String.IsNullOrEmpty(FSelectedBlockFieldItem_D.EditMask))
                        FSelectedBlockFieldItem_D.EditMask = "Left";
                }
                else if (FClientData.BaseFormName == "SingleTable")
                {
                    if (String.IsNullOrEmpty(FSelectedBlockFieldItem_D.EditMask))
                    {
                        if (FSelectedBlockFieldItem_D.DataType == typeof(int)
                            || FSelectedBlockFieldItem_D.DataType == typeof(float)
                            || FSelectedBlockFieldItem_D.DataType == typeof(double)
                            || FSelectedBlockFieldItem_D.DataType == typeof(decimal))
                            FSelectedBlockFieldItem_D.EditMask = "Right";
                        else
                            FSelectedBlockFieldItem_D.EditMask = "Left";
                    }
                }
                else if (FClientData.BaseFormName == "MasterDetail")
                {
                    if (String.IsNullOrEmpty(FSelectedBlockFieldItem_D.EditMask))
                    {
                        if (FSelectedBlockFieldItem_D.DataType == typeof(int)
                            || FSelectedBlockFieldItem_D.DataType == typeof(float)
                            || FSelectedBlockFieldItem_D.DataType == typeof(double)
                            || FSelectedBlockFieldItem_D.DataType == typeof(decimal))
                            FSelectedBlockFieldItem_D.EditMask = "Right";
                        else
                            FSelectedBlockFieldItem_D.EditMask = "Left";
                    }
                }
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
            tbWidth_D.Text = FSelectedBlockFieldItem_D.Length.ToString();
            if (!String.IsNullOrEmpty(FSelectedBlockFieldItem_D.CheckNull) && FSelectedBlockFieldItem_D.CheckNull.ToLower() == "true")
                cbIsGroup_D.Checked = true;
            else
                cbIsGroup_D.Checked = false;
            cbTextAlign_D.Text = FSelectedBlockFieldItem_D.EditMask;
        }

        private void SetValue_D()
        {
            if (FSelectedBlockFieldItem_D == null)
                return;
            FSelectedBlockFieldItem_D.Description = tbCaption_D.Text;
            int width = 0;
            int.TryParse(tbWidth_D.Text, out width);
            FSelectedBlockFieldItem_D.Length = width;
            FSelectedBlockFieldItem_D.CheckNull = cbIsGroup_D.Checked.ToString();
            FSelectedBlockFieldItem_D.EditMask = cbTextAlign_D.Text;
        }
    }

    public class TRDLCWizardData : Object
    {
        private string FPackageName, FBaseFormName, FServerPackageName, FFolderName, FTableName, FRealTableName, FFormName, FProviderName,
            FDatabaseName, FSolutionName, FViewProviderName, FWebSiteName,FWebSiteFullName, FFolderMode, FFormTitle;
        private TBlockItems FBlocks;
        private MWizard2015.fmRDLCWizard FOwner;
        private bool FNewSolution = false;
        private string FCodeFolderName;
        private int FColumnCount;
        private ClientType FDatabaseType;
        private String FConnString;
        private String FLanguage = "cs";

        public TRDLCWizardData(MWizard2015.fmRDLCWizard Owner)
        {
            FOwner = Owner;
            FBlocks = new TBlockItems(this);
        }

        public ClientType DatabaseType
        {
            get { return FDatabaseType; }
            set { FDatabaseType = value; }
        }

        public fmRDLCWizard Owner
        {
            get { return FOwner; }
            set { FOwner = value; }
        }

        public String FormTitle
        {
            get { return FFormTitle; }
            set { FFormTitle = value; }
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
                BlockFieldItemsNode = WzdUtils.FindNode(null, BlockNode, "BlockFieldItems");
                LoadBlockFieldItems(BlockFieldItemsNode, BI.BlockFieldItems);
                Blocks.Add(BI);
            }
        }

        public bool IsMasterDetailBaseForm()
        {
            bool Result;
            Result = false;
            if (string.Compare(FBaseFormName, "MasterDetail") == 0)
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

        public string WebSiteName
        {
            get
            {
                return FWebSiteName;
            }
            set
            {
                FWebSiteName = value;
            }
        }

        public string WebSiteFullName
        {
            get
            {
                return FWebSiteFullName;
            }
            set
            {
                FWebSiteFullName = value;
            }
        }

        public string FolderMode
        {
            get
            {
                return FFolderMode;
            }
            set
            {
                FFolderMode = value;
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

        public string FolderName
        {
            get
            {
                return FFolderName;
            }
            set
            {
                FFolderName = value;
            }

        }

        public string CodeFolderName
        {
            get
            {
                return FCodeFolderName;
            }
            set
            {
                string S = value;
                if (S != "")
                    if (S[S.Length - 1] == '\\')
                        S = S.Substring(0, S.Length - 1);
                FCodeFolderName = S;
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

        public string RealTableName
        {
            get
            {
                return FRealTableName;
            }
            set
            {
                FRealTableName = value;
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

    partial class TRDLCWizardGenerator : System.ComponentModel.Component
    {
        private TRDLCWizardData FClientData;
        private DTE2 FDTE2;
        private AddIn FAddIn;
        private System.Windows.Forms.Form FRootForm = null;
        private InfoDataSet FDataSet = null;
        private ProjectItem FPI;
        private ProjectItem FPIFolder;
        private Project FProject = null;
        private InfoDataGridView FViewGrid = null;
        private InfoDataSet FWizardDataSet = null;
        private DataSet FSYS_REFVAL;
        private Window FDesignWindow;

        public TRDLCWizardGenerator(TRDLCWizardData ClientData, DTE2 dte2, AddIn aAddIn)
        {
            FClientData = ClientData;
            FDTE2 = dte2;
            FAddIn = aAddIn;
            FSYS_REFVAL = new DataSet();
            //???FTemplatePath = WzdUtils.GetAddinsPath() + "\\Templates\\";
        }

        private void GenFolder()
        {
            Solution2 sln = (Solution2)FDTE2.Solution;

            if (string.Compare(sln.FullName, FClientData.SolutionName) != 0)
            {
                sln.Open(FClientData.SolutionName);
            }
            foreach (Project P in sln.Projects)
            {
                if (String.Compare(P.Kind, "{E24C65DC-7377-472b-9ABA-BC803B73C61A}") == 0)
                {
                    String VSName = P.Name;
                    if (FClientData.Owner.SDCall)
                    {
                        VSName = WzdUtils.FixupToVSWebSiteName(VSName);
                    }
                    if (string.Compare(VSName, FClientData.WebSiteName) == 0)
                    {
                        FProject = P;
                        break;
                    }
                }
            }
            switch (FClientData.FolderMode)
            {
                case "ExistFolder":
                    foreach (ProjectItem PI in FProject.ProjectItems)
                    {
                        if (string.Compare(PI.Name, FClientData.FolderName) == 0)
                        {
                            FPIFolder = PI;
                            break;
                        }
                    }
                    break;
                case "NewFolder":
                    FPIFolder = FProject.ProjectItems.AddFolder(FClientData.FolderName, "{6BB5F8EF-4483-11D3-8BCF-00C04F8EC28C}");
                    break;
                default:
                    break;
            }
        }

        private const string ms = "http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition";
        private const string rd = "http://schemas.microsoft.com/SQLServer/reporting/reportdesigner";
        private void GenSingleLabelRdlc(string rdlcFileName, TBlockItem BlockItem, bool isMaster, String detailsRptFileName = "", DataColumn[] relationColumns = null)
        {
            String DataSetName = BlockItem.TableName;
            String ModuleName = FClientData.ProviderName.Substring(0, FClientData.ProviderName.IndexOf('.'));
            String SolutionName = System.IO.Path.GetFileNameWithoutExtension(FDTE2.Solution.FullName);
            String realTableName = CliUtils.GetTableName(ModuleName, DataSetName, SolutionName, "", true);
            realTableName = WzdUtils.RemoveQuote(realTableName, FClientData.DatabaseType);

            DataTable dtDataDic = GetDDTable(FClientData.DatabaseType, FClientData.Owner.SelectedAlias, realTableName);
            XmlDocument doc = new XmlDocument();
            doc.Load(rdlcFileName);

            XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
            nsmgr.AddNamespace("ms", ms);
            nsmgr.AddNamespace("rd", rd);
            GenDataSources(doc, nsmgr);
            GenDataSets(doc, nsmgr, BlockItem);
            ReportTable rt = new ReportTable(BlockItem.TableName);
            foreach (TBlockFieldItem item in BlockItem.BlockFieldItems)
            {
                bool isGroup = false;
                if (!String.IsNullOrEmpty(item.CheckNull) && item.CheckNull.ToLower() == "true")
                    isGroup = true;
                ReportColumn rc = new ReportColumn(item.DataField, isGroup);
                rc.TextAlign = item.EditMask;
                rt.ReportColumns.Add(rc);
            }
            //GenList(doc, nsmgr, rt, FClientData.TableName);

            XmlNode nBodyRptItems = doc.SelectSingleNode("ms:Report/ms:Body/ms:ReportItems", nsmgr);
            XmlNode nRect = nBodyRptItems.SelectSingleNode("ms:Rectangle[@Name='rectContainer']", nsmgr);
            nBodyRptItems.RemoveChild(nRect);
            //List
            XmlNode nTablix = doc.CreateElement("Tablix", nBodyRptItems.NamespaceURI);
            XmlAttribute aListName = doc.CreateAttribute("Name");
            aListName.Value = "lstContainer";
            nTablix.Attributes.Append(aListName);
            XmlNode nTablixBody = doc.CreateElement("TablixBody", nBodyRptItems.NamespaceURI);
            nTablix.AppendChild(nTablixBody);
            XmlNode nTablixColumns = doc.CreateElement("TablixColumns", nBodyRptItems.NamespaceURI);
            nTablixBody.AppendChild(nTablixColumns);
            XmlNode nTablixColumn = doc.CreateElement("TablixColumn", nBodyRptItems.NamespaceURI);
            nTablixColumns.AppendChild(nTablixColumn);
            XmlNode nWidth = doc.CreateElement("Width", nBodyRptItems.NamespaceURI);
            nWidth.InnerText = "21cm";
            nTablixColumn.AppendChild(nWidth);
            XmlNode nTablixRows = doc.CreateElement("TablixRows", nBodyRptItems.NamespaceURI);
            nTablixBody.AppendChild(nTablixRows);
            XmlNode nTablixRow = doc.CreateElement("TablixRow", nBodyRptItems.NamespaceURI);
            nTablixRows.AppendChild(nTablixRow);
            XmlNode nHeight = doc.CreateElement("Height", nBodyRptItems.NamespaceURI);
            nHeight.InnerText = "4.9cm";
            nTablixRow.AppendChild(nHeight);
            XmlNode nTablixCells = doc.CreateElement("TablixCells", nBodyRptItems.NamespaceURI);
            nTablixRow.AppendChild(nTablixCells);
            XmlNode nTablixCell = doc.CreateElement("TablixCell", nBodyRptItems.NamespaceURI);
            nTablixCells.AppendChild(nTablixCell);
            XmlNode nCellContents = doc.CreateElement("CellContents", nBodyRptItems.NamespaceURI);
            nTablixCell.AppendChild(nCellContents);
            XmlNode nRectangle = doc.CreateElement("Rectangle", nBodyRptItems.NamespaceURI);
            XmlAttribute aRectangleName = doc.CreateAttribute("Name");
            aRectangleName.Value = "lstContainer_Contents";
            nRectangle.Attributes.Append(aRectangleName);
            nCellContents.AppendChild(nRectangle);
            XmlNode nReportItems = doc.CreateElement("ReportItems", nBodyRptItems.NamespaceURI);
            nRectangle.AppendChild(nReportItems);
            XmlNode nRectangle2 = doc.CreateElement("Rectangle", nBodyRptItems.NamespaceURI);
            XmlAttribute aRectangle2Name = doc.CreateAttribute("Name");
            aRectangle2Name.Value = "rectContainer";
            nRectangle2.Attributes.Append(aRectangle2Name);
            nReportItems.AppendChild(nRectangle2);

            //生成Group设置
            if (HasGroupCondition(rt))
            {
                //Grouping
                XmlNode nGrouping = doc.CreateElement("Grouping", nTablix.NamespaceURI);
                XmlAttribute aGroupingName = doc.CreateAttribute("Name");
                aGroupingName.Value = "lstContainer_Details_" + FClientData.TableName;
                nGrouping.Attributes.Append(aGroupingName);
                //Grouping.PageBreakAtEnd
                XmlNode nPageBreakAtEnd = doc.CreateElement("PageBreakAtEnd", nTablix.NamespaceURI);
                nPageBreakAtEnd.InnerText = "true";
                nGrouping.AppendChild(nPageBreakAtEnd);
                //Grouping.GroupExpressions
                XmlNode nGroupExpressions = doc.CreateElement("GroupExpressions", nTablix.NamespaceURI);
                List<ReportColumn> lstGroupConditionFields = GetGroupConditionRptColumns(rt);
                foreach (ReportColumn cdtField in lstGroupConditionFields)
                {
                    //Grouping.GroupExpressions.GroupExpression
                    XmlNode nGroupExpression = doc.CreateElement("GroupExpression", nTablix.NamespaceURI);

                    nGroupExpression.InnerText = "=Fields!" + cdtField.ColumnName + ".Value";
                    nGroupExpressions.AppendChild(nGroupExpression);
                }
                nGrouping.AppendChild(nGroupExpressions);
                nTablix.AppendChild(nGrouping);
            }
            //List.Style
            XmlNode nListStyle = doc.CreateElement("Style", nBodyRptItems.NamespaceURI);
            //List.Style.FontFamily
            XmlNode nFont = doc.CreateElement("FontFamily", nBodyRptItems.NamespaceURI);
            nFont.InnerText = "PMingLiU";
            nListStyle.AppendChild(nFont);
            nTablix.AppendChild(nListStyle);
            nBodyRptItems.AppendChild(nTablix);

            XmlNode startNode = nRectangle2;//;GetStartXPathNode(doc, nsmgr, true, isMaster);
            //给Title赋值
            XmlNode nTitle = GetTitleNode(doc, nsmgr, true);
            if (nTitle != null)
            {
                XmlNode nTitleValue = nTitle.SelectSingleNode("ms:Paragraphs/ms:Paragraph/ms:TextRuns/ms:TextRun/ms:Value", nsmgr);
                if (nTitleValue != null)
                {
                    nTitleValue.InnerText = FClientData.FormTitle;
                }
            }

            #region GenDataRegion
            // Top Init
            float curLeft = 0.5f;
            float curTop = 0.5f;
            int layoutIndex = 0;
            XmlDocument docrealContext = startNode.OwnerDocument;
            XmlNode temNode = startNode;
            //if (!isMaster)
            //{
            temNode = docrealContext.CreateElement("ReportItems", temNode.NamespaceURI);
            //}
            int iLayoutColumnNum = 2;// rptParams.LayoutColumnNum
            String sHorGaps = "2.54";//rptParams.HorGaps
            String sVertGaps = "0.64";//rptParams.VertGaps
            for (int i = 0; i < rt.ReportColumns.Count; i++)
            {
                string colName = rt.ReportColumns[i].ColumnName;
                string EditMask = GetFieldEditMask(dtDataDic, colName);
                string textAlign = rt.ReportColumns[i].TextAlign;
                if (layoutIndex >= iLayoutColumnNum)
                {
                    layoutIndex = 0;
                    curLeft = 0.5f;
                    curTop += 0.94f;

                    //GenLabel
                    GenLabel(temNode, curLeft, curTop, "lbl" + colName, GetFieldCaption(dtDataDic, colName) + ":", sHorGaps, sVertGaps, false, "");
                    //GenrTextBox
                    curLeft += (float)Convert.ToDouble(sHorGaps);
                    GenLabel(temNode, curLeft, curTop, "txt" + colName, EditMask, sHorGaps, sVertGaps, false, textAlign);
                    layoutIndex++;
                    curLeft += (float)Convert.ToDouble(sHorGaps) + 0.5f;
                }
                else
                {
                    //GenLabel
                    GenLabel(temNode, curLeft, curTop, "lbl" + colName, GetFieldCaption(dtDataDic, colName) + ":", sHorGaps, sVertGaps, false, "");
                    //GenrTextBox
                    curLeft += (float)Convert.ToDouble(sHorGaps);
                    GenLabel(temNode, curLeft, curTop, "txt" + colName, EditMask, sHorGaps, sVertGaps, false, textAlign);
                    layoutIndex++;
                    curLeft += (float)Convert.ToDouble(sHorGaps) + 0.5f;
                }
            }
            //if (!isMaster)
            //{
            startNode.AppendChild(temNode);
            //}
            XmlNode nStyleRectangle = doc.CreateElement("Style", nBodyRptItems.NamespaceURI);
            startNode.AppendChild(nStyleRectangle);
            XmlNode nBorderStyleRectangle = doc.CreateElement("Border", nBodyRptItems.NamespaceURI);
            nStyleRectangle.AppendChild(nBorderStyleRectangle);
            XmlNode nStyleBorderStyleRectangle = doc.CreateElement("Style", nBodyRptItems.NamespaceURI);
            nStyleBorderStyleRectangle.InnerText = "Solid";
            nBorderStyleRectangle.AppendChild(nStyleBorderStyleRectangle);

            if (isMaster)
            {
                //nBodyRptItems
                XmlNode nSubreport = doc.CreateElement("Subreport", nBodyRptItems.NamespaceURI);
                XmlAttribute aSubreportName = doc.CreateAttribute("Name");
                aSubreportName.Value = "subreport1";
                nSubreport.Attributes.Append(aSubreportName);
                temNode.AppendChild(nSubreport);
                XmlNode nReportName = doc.CreateElement("ReportName", nBodyRptItems.NamespaceURI);
                nReportName.InnerText = detailsRptFileName;
                nSubreport.AppendChild(nReportName);
                XmlNode nKeepTogether1 = doc.CreateElement("KeepTogether", nBodyRptItems.NamespaceURI);
                nKeepTogether1.InnerText = "true";
                nSubreport.AppendChild(nKeepTogether1);
                XmlNode nTopSubreport = doc.CreateElement("Top", nBodyRptItems.NamespaceURI);
                nTopSubreport.InnerText = "9.8cm";
                nSubreport.AppendChild(nTopSubreport);
                XmlNode nHeightSubreport = doc.CreateElement("Height", nBodyRptItems.NamespaceURI);
                nHeightSubreport.InnerText = "9.6cm";
                nSubreport.AppendChild(nHeightSubreport);
                XmlNode nWidthSubreport = doc.CreateElement("Width", nBodyRptItems.NamespaceURI);
                nWidthSubreport.InnerText = "21cm";
                nSubreport.AppendChild(nWidthSubreport);

                //Parameters
                XmlNode nParameters = doc.CreateElement("Parameters", nBodyRptItems.NamespaceURI);
                foreach (DataColumn col in relationColumns)
                {
                    //Parameters.Parameter
                    XmlNode nParameter = doc.CreateElement("Parameter", nBodyRptItems.NamespaceURI);
                    XmlAttribute aParamName = doc.CreateAttribute("Name");
                    aParamName.Value = "p" + col.ColumnName;
                    nParameter.Attributes.Append(aParamName);
                    //Parameters.Parameter.Value
                    string EditMask = GetFieldEditMask(dtDataDic, col.ColumnName);
                    XmlNode nParamValue = doc.CreateElement("Value", nBodyRptItems.NamespaceURI);
                    nParamValue.InnerText = EditMask;
                    nParameter.AppendChild(nParamValue);
                    nParameters.AppendChild(nParameter);
                }
                nSubreport.AppendChild(nParameters);
            }

            GenReportBodyHeight(doc, curTop, nsmgr, isMaster);

            #endregion

            XmlNode nTablixColumnHierarchy = doc.CreateElement("TablixColumnHierarchy", nBodyRptItems.NamespaceURI);
            nTablix.AppendChild(nTablixColumnHierarchy);
            XmlNode nTablixMembers = doc.CreateElement("TablixMembers", nBodyRptItems.NamespaceURI);
            nTablixColumnHierarchy.AppendChild(nTablixMembers);
            XmlNode nTablixMember = doc.CreateElement("TablixMember", nBodyRptItems.NamespaceURI);
            nTablixMembers.AppendChild(nTablixMember);

            XmlNode nTablixRowHierarchy = doc.CreateElement("TablixRowHierarchy", nBodyRptItems.NamespaceURI);
            nTablix.AppendChild(nTablixRowHierarchy);
            XmlNode nTablixMembers2 = doc.CreateElement("TablixMembers", nBodyRptItems.NamespaceURI);
            nTablixRowHierarchy.AppendChild(nTablixMembers2);
            XmlNode nTablixMember2 = doc.CreateElement("TablixMember", nBodyRptItems.NamespaceURI);
            nTablixMembers2.AppendChild(nTablixMember2);
            XmlNode nGroup = doc.CreateElement("Group", nBodyRptItems.NamespaceURI);
            XmlAttribute aGroupName = doc.CreateAttribute("Name");
            aGroupName.Value = "lstContainer_Details_Group";
            nGroup.Attributes.Append(aGroupName);
            nTablixMember2.AppendChild(nGroup);
            XmlNode nDataElementName = doc.CreateElement("DataElementName", nBodyRptItems.NamespaceURI);
            nDataElementName.InnerText = "Item";
            nGroup.AppendChild(nDataElementName);
            XmlNode nDataElementName2 = doc.CreateElement("DataElementName", nBodyRptItems.NamespaceURI);
            nDataElementName2.InnerText = "Item_Collection";
            nTablixMember2.AppendChild(nDataElementName2);
            XmlNode nDataElementOutput = doc.CreateElement("DataElementOutput", nBodyRptItems.NamespaceURI);
            nDataElementOutput.InnerText = "Output";
            nTablixMember2.AppendChild(nDataElementOutput);
            XmlNode nKeepTogether = doc.CreateElement("KeepTogether", nBodyRptItems.NamespaceURI);
            nKeepTogether.InnerText = "true";
            nTablixMember2.AppendChild(nKeepTogether);

            XmlNode nDataSetName = doc.CreateElement("DataSetName", nBodyRptItems.NamespaceURI);
            nDataSetName.InnerText = BlockItem.TableName;// "NewDataSet";
            if (rdlcFileName.EndsWith("-dt.rdlc"))
            {
                nDataSetName.InnerText = BlockItem.Relation.ParentTable.TableName;// "NewDataSet";
            }
            nTablix.AppendChild(nDataSetName);

            doc.Save(rdlcFileName);
        }

        private void GenSingleTableRdlc(string rdlcFileName, TBlockItem BlockItem, bool isDetails)
        {
            String DataSetName = BlockItem.TableName;
            String ModuleName = FClientData.ProviderName.Substring(0, FClientData.ProviderName.IndexOf('.'));
            String SolutionName = System.IO.Path.GetFileNameWithoutExtension(FDTE2.Solution.FullName);
            String realTableName = CliUtils.GetTableName(ModuleName, DataSetName, SolutionName, "", true);
            realTableName = WzdUtils.RemoveQuote(realTableName, FClientData.DatabaseType);

            DataTable dtDataDic = GetDDTable(FClientData.DatabaseType, FClientData.Owner.SelectedAlias, realTableName);
            XmlDocument doc = new XmlDocument();
            doc.Load(rdlcFileName);

            XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
            nsmgr.AddNamespace("ms", ms);
            nsmgr.AddNamespace("rd", rd);
            GenDataSources(doc, nsmgr);
            GenDataSets(doc, nsmgr, BlockItem);
            ReportTable rt = new ReportTable(BlockItem.TableName);
            foreach (TBlockFieldItem item in BlockItem.BlockFieldItems)
            {
                bool isGroup = false;
                if (!String.IsNullOrEmpty(item.CheckNull) && item.CheckNull.ToLower() == "true")
                    isGroup = true;
                ReportColumn rc = new ReportColumn(item.DataField, isGroup);
                rc.Width = item.Length * 0.2;
                if (rc.Width == 0)
                    rc.Width = 2;
                else if (rc.Width < 1)
                    rc.Width = 1;
                else if (rc.Width > 8)
                    rc.Width = 8;
                double titleWidth = (item.Description == null ? item.DataField.Length : item.Description.Length) * 0.4;
                if (rc.Width < titleWidth)
                    rc.Width = titleWidth;
                rc.TextAlign = item.EditMask;
                rt.ReportColumns.Add(rc);
            }

            XmlNode nBodyRptItems = doc.SelectSingleNode("ms:Report/ms:Body/ms:ReportItems", nsmgr);
            XmlNode nRect = nBodyRptItems.SelectSingleNode("ms:Rectangle[@Name='rectContainer']", nsmgr);
            nBodyRptItems.RemoveChild(nRect);
            //List
            XmlNode nRectangle1 = doc.CreateElement("Rectangle", nBodyRptItems.NamespaceURI);
            XmlAttribute aRectangleName1 = doc.CreateAttribute("Name");
            aRectangleName1.Value = "rectContainer";
            nRectangle1.Attributes.Append(aRectangleName1);
            XmlNode nReportItems1 = doc.CreateElement("ReportItems", nBodyRptItems.NamespaceURI);
            nRectangle1.AppendChild(nReportItems1);
            XmlNode nDataElementOutput1 = doc.CreateElement("DataElementOutput", nBodyRptItems.NamespaceURI);
            nDataElementOutput1.InnerText = "ContentsOnly";
            nRectangle1.AppendChild(nDataElementOutput1);
            XmlNode nListStyle1 = doc.CreateElement("Style", nBodyRptItems.NamespaceURI);
            //List.Style.FontFamily
            XmlNode nFont1 = doc.CreateElement("FontFamily", nBodyRptItems.NamespaceURI);
            nFont1.InnerText = "Verdana";
            nListStyle1.AppendChild(nFont1);
            nRectangle1.AppendChild(nListStyle1);
            XmlNode nTop1 = doc.CreateElement("Top", nBodyRptItems.NamespaceURI);
            nTop1.InnerText = "0cm";
            nRectangle1.AppendChild(nTop1);
            XmlNode nHeight3 = doc.CreateElement("Height", nBodyRptItems.NamespaceURI);
            nHeight3.InnerText = "5.5cm";
            nRectangle1.AppendChild(nHeight3);
            XmlNode nWidth3 = doc.CreateElement("Width", nBodyRptItems.NamespaceURI);
            nWidth3.InnerText = "21cm";
            nRectangle1.AppendChild(nWidth3);

            String sHorGaps = "2.54";//rptParams.HorGaps
            String sVertGaps = "0.64";//rptParams.VertGaps
            float curLeft = 0.5f;
            float curTop = 0.5f;
            if (isDetails)
            {
                //XmlNode nLine = doc.CreateElement("Line", nBodyRptItems.NamespaceURI);
                //XmlAttribute aLineName = doc.CreateAttribute("Name");
                //aLineName.Value = "line1";
                //nLine.Attributes.Append(aLineName);
                //nReportItems1.AppendChild(nLine);
                //XmlNode nTopLine = doc.CreateElement("Top", nBodyRptItems.NamespaceURI);
                //nTopLine.InnerText = "1.5cm";
                //nLine.AppendChild(nTopLine);
                //XmlNode nHeightLine = doc.CreateElement("Height", nBodyRptItems.NamespaceURI);
                //nHeightLine.InnerText = "0cm";
                //nLine.AppendChild(nHeightLine);
                //XmlNode nWidthLine = doc.CreateElement("Width", nBodyRptItems.NamespaceURI);
                //nWidthLine.InnerText = "21cm";
                //nLine.AppendChild(nWidthLine);
                //XmlNode nStyleLine = doc.CreateElement("Style", nBodyRptItems.NamespaceURI);
                //nLine.AppendChild(nStyleLine);
                //XmlNode nBorderStyleLine = doc.CreateElement("Border", nBodyRptItems.NamespaceURI);
                //nStyleLine.AppendChild(nBorderStyleLine);
                //XmlNode nStyleBorderStyleLine = doc.CreateElement("Style", nBodyRptItems.NamespaceURI);
                //nStyleBorderStyleLine.InnerText = "Solid";
                //nBorderStyleLine.AppendChild(nStyleBorderStyleLine);
                //XmlNode nFontFamilyStyleLine = doc.CreateElement("FontFamily", nBodyRptItems.NamespaceURI);
                //nFontFamilyStyleLine.InnerText = "Verdana";
                //nStyleLine.AppendChild(nFontFamilyStyleLine);

                //int layoutIndex = 0;
                //foreach (var item in BlockItem.Relation.ParentColumns)
                //{
                //    string colName = item.ColumnName;
                //    string EditMask = GetFieldEditMask2(dtDataDic, colName);

                //    GenLabel(nReportItems1, curLeft, curTop, "cap" + colName, GetFieldCaption(dtDataDic, colName), sHorGaps, sVertGaps, false);
                //    curLeft += (float)Convert.ToDouble(sHorGaps);
                //    GenLabel(nReportItems1, curLeft, curTop, "txt" + colName, EditMask, sHorGaps, sVertGaps, false);
                //    layoutIndex++;
                //    curLeft += (float)Convert.ToDouble(sHorGaps) + 0.5f;
                //}
            }

            XmlNode nTablix = doc.CreateElement("Tablix", nBodyRptItems.NamespaceURI);
            XmlAttribute aListName = doc.CreateAttribute("Name");
            aListName.Value = "lstContainer";
            nTablix.Attributes.Append(aListName);
            nReportItems1.AppendChild(nTablix);
            XmlNode nTablixBody = doc.CreateElement("TablixBody", nBodyRptItems.NamespaceURI);
            nTablix.AppendChild(nTablixBody);
            XmlNode nTablixColumns = doc.CreateElement("TablixColumns", nBodyRptItems.NamespaceURI);
            nTablixBody.AppendChild(nTablixColumns);
            for (int i = 0; i < rt.ReportColumns.Count; i++)
            {
                XmlNode nTablixColumn = doc.CreateElement("TablixColumn", nBodyRptItems.NamespaceURI);
                nTablixColumns.AppendChild(nTablixColumn);
                XmlNode nWidth = doc.CreateElement("Width", nBodyRptItems.NamespaceURI);
                nWidth.InnerText = rt.ReportColumns[i].Width.ToString() + "cm";
                nTablixColumn.AppendChild(nWidth);
            }
            XmlNode nTablixRows = doc.CreateElement("TablixRows", nBodyRptItems.NamespaceURI);
            nTablixBody.AppendChild(nTablixRows);

            for (int j = 0; j < 2; j++)
            {
                XmlNode nTablixRow = doc.CreateElement("TablixRow", nBodyRptItems.NamespaceURI);
                nTablixRows.AppendChild(nTablixRow);
                XmlNode nHeight = doc.CreateElement("Height", nBodyRptItems.NamespaceURI);
                nHeight.InnerText = "0.64cm";
                nTablixRow.AppendChild(nHeight);
                XmlNode nTablixCells = doc.CreateElement("TablixCells", nBodyRptItems.NamespaceURI);
                nTablixRow.AppendChild(nTablixCells);

                //生成Group设置
                //if (HasGroupCondition(rt))
                //{
                //    //Grouping
                //    XmlNode nGrouping = doc.CreateElement("Grouping", nTablix.NamespaceURI);
                //    XmlAttribute aGroupingName = doc.CreateAttribute("Name");
                //    aGroupingName.Value = "lstContainer_Details_" + FClientData.TableName;
                //    nGrouping.Attributes.Append(aGroupingName);
                //    //Grouping.PageBreakAtEnd
                //    XmlNode nPageBreakAtEnd = doc.CreateElement("PageBreakAtEnd", nTablix.NamespaceURI);
                //    nPageBreakAtEnd.InnerText = "true";
                //    nGrouping.AppendChild(nPageBreakAtEnd);
                //    //Grouping.GroupExpressions
                //    XmlNode nGroupExpressions = doc.CreateElement("GroupExpressions", nTablix.NamespaceURI);
                //    List<ReportColumn> lstGroupConditionFields = GetGroupConditionRptColumns(rt);
                //    foreach (ReportColumn cdtField in lstGroupConditionFields)
                //    {
                //        //Grouping.GroupExpressions.GroupExpression
                //        XmlNode nGroupExpression = doc.CreateElement("GroupExpression", nTablix.NamespaceURI);

                //        nGroupExpression.InnerText = "=Fields!" + cdtField.ColumnName + ".Value";
                //        nGroupExpressions.AppendChild(nGroupExpression);
                //    }
                //    nGrouping.AppendChild(nGroupExpressions);
                //    nTablix.AppendChild(nGrouping);
                //}

                XmlNode startNode = nTablixCells;//;GetStartXPathNode(doc, nsmgr, true, isMaster);
                //给Title赋值
                XmlNode nTitle = GetTitleNode(doc, nsmgr, true);
                if (nTitle != null)
                {
                    XmlNode nTitleValue = nTitle.SelectSingleNode("ms:Paragraphs/ms:Paragraph/ms:TextRuns/ms:TextRun/ms:Value", nsmgr);
                    if (nTitleValue != null)
                    {
                        nTitleValue.InnerText = FClientData.FormTitle;
                    }
                }

                // Top Init
                XmlDocument docrealContext = startNode.OwnerDocument;
                //XmlNode temNode = startNode;

                for (int i = 0; i < rt.ReportColumns.Count; i++)
                {
                    string colName = rt.ReportColumns[i].ColumnName;
                    string EditMask = GetFieldEditMask(dtDataDic, colName);
                    string textAlign = rt.ReportColumns[i].TextAlign;

                    curLeft = 0.5f;
                    curTop += 0.94f;

                    //GenLabel
                    if (j == 0)
                        GenTableLabel(startNode, curLeft, curTop, "hd" + colName, GetFieldCaption(dtDataDic, colName), sHorGaps, sVertGaps, false, textAlign);
                    else if (j == 1)
                        GenTableLabel(startNode, curLeft, curTop, "cnt" + colName, EditMask, sHorGaps, sVertGaps, false, textAlign);
                }
                //if (!isMaster)
                //{
                //    startNode.AppendChild(temNode);
                //}
            }
            //List.Style
            XmlNode nListStyle = doc.CreateElement("Style", nBodyRptItems.NamespaceURI);
            //List.Style.FontFamily
            XmlNode nFont = doc.CreateElement("FontFamily", nBodyRptItems.NamespaceURI);
            nFont.InnerText = "PMingLiU";
            nListStyle.AppendChild(nFont);
            nTablix.AppendChild(nListStyle);
            XmlNode nDataSetName = doc.CreateElement("DataSetName", nBodyRptItems.NamespaceURI);
            nDataSetName.InnerText = BlockItem.TableName;// "NewDataSet";
            if (rdlcFileName.EndsWith("-dt.rdlc"))
            {
                nDataSetName.InnerText = BlockItem.Relation.ParentTable.TableName;// "NewDataSet";
            }
            nTablix.AppendChild(nDataSetName);
            XmlNode nTop = doc.CreateElement("Top", nBodyRptItems.NamespaceURI);
            nTop.InnerText = "0.1cm";
            nTablix.AppendChild(nTop);
            XmlNode nLeft = doc.CreateElement("Left", nBodyRptItems.NamespaceURI);
            nLeft.InnerText = "0.5cm";
            nTablix.AppendChild(nLeft);
            XmlNode nHeight2 = doc.CreateElement("Height", nBodyRptItems.NamespaceURI);
            nHeight2.InnerText = "1.28cm";
            nTablix.AppendChild(nHeight2);
            XmlNode nWidth2 = doc.CreateElement("Width", nBodyRptItems.NamespaceURI);
            nWidth2.InnerText = "10.16cm";
            nTablix.AppendChild(nWidth2);
            nBodyRptItems.AppendChild(nRectangle1);
            //GenReportBodyHeight(doc, curTop, nsmgr, isMaster);

            XmlNode nTablixColumnHierarchy = doc.CreateElement("TablixColumnHierarchy", nBodyRptItems.NamespaceURI);
            nTablix.AppendChild(nTablixColumnHierarchy);
            XmlNode nTablixMembers = doc.CreateElement("TablixMembers", nBodyRptItems.NamespaceURI);
            nTablixColumnHierarchy.AppendChild(nTablixMembers);
            for (int i = 0; i < rt.ReportColumns.Count; i++)
            {
                XmlNode nTablixMember = doc.CreateElement("TablixMember", nBodyRptItems.NamespaceURI);
                nTablixMembers.AppendChild(nTablixMember);
            }


            if (HasGroupCondition(rt))
            {
                CreateGroup(nTablix, doc, rt, dtDataDic, nBodyRptItems);
            }
            else
            {
                XmlNode nTablixRowHierarchy = doc.CreateElement("TablixRowHierarchy", nBodyRptItems.NamespaceURI);
                nTablix.AppendChild(nTablixRowHierarchy);
                XmlNode nTablixMembers2 = doc.CreateElement("TablixMembers", nBodyRptItems.NamespaceURI);
                nTablixRowHierarchy.AppendChild(nTablixMembers2);
                XmlNode nTablixMember3 = doc.CreateElement("TablixMember", nBodyRptItems.NamespaceURI);
                nTablixMembers2.AppendChild(nTablixMember3);
                XmlNode nKeepWithGroup = doc.CreateElement("KeepWithGroup", nBodyRptItems.NamespaceURI);
                nKeepWithGroup.InnerText = "After";
                nTablixMember3.AppendChild(nKeepWithGroup);
                XmlNode nRepeatOnNewPage = doc.CreateElement("RepeatOnNewPage", nBodyRptItems.NamespaceURI);
                nRepeatOnNewPage.InnerText = "true";
                nTablixMember3.AppendChild(nRepeatOnNewPage);
                XmlNode nKeepTogether = doc.CreateElement("KeepTogether", nBodyRptItems.NamespaceURI);
                nKeepTogether.InnerText = "true";
                nTablixMember3.AppendChild(nKeepTogether);
                XmlNode nTablixMember2 = doc.CreateElement("TablixMember", nBodyRptItems.NamespaceURI);
                nTablixMembers2.AppendChild(nTablixMember2);
                XmlNode nGroup = doc.CreateElement("Group", nBodyRptItems.NamespaceURI);
                XmlAttribute aGroupName = doc.CreateAttribute("Name");
                aGroupName.Value = "lstContainer_Details_Group";
                nGroup.Attributes.Append(aGroupName);
                nTablixMember2.AppendChild(nGroup);
                XmlNode nTablixMembers3 = doc.CreateElement("TablixMembers", nBodyRptItems.NamespaceURI);
                nTablixMember2.AppendChild(nTablixMembers3);
                XmlNode nTablixMember4 = doc.CreateElement("TablixMember", nBodyRptItems.NamespaceURI);
                nTablixMembers3.AppendChild(nTablixMember4);
                XmlNode nDataElementName = doc.CreateElement("DataElementName", nBodyRptItems.NamespaceURI);
                nDataElementName.InnerText = "Item";
                nGroup.AppendChild(nDataElementName);
                XmlNode nDataElementName2 = doc.CreateElement("DataElementName", nBodyRptItems.NamespaceURI);
                nDataElementName2.InnerText = "Item_Collection";
                nTablixMember2.AppendChild(nDataElementName2);
                XmlNode nDataElementOutput = doc.CreateElement("DataElementOutput", nBodyRptItems.NamespaceURI);
                nDataElementOutput.InnerText = "Output";
                nTablixMember2.AppendChild(nDataElementOutput);
                XmlNode nKeepTogether2 = doc.CreateElement("KeepTogether", nBodyRptItems.NamespaceURI);
                nKeepTogether2.InnerText = "true";
                nTablixMember2.AppendChild(nKeepTogether2);
            }

            if (isDetails)
            {
                XmlNode nFilters = doc.CreateElement("Filters", nBodyRptItems.NamespaceURI);
                nTablix.AppendChild(nFilters);
                foreach (var item in BlockItem.Relation.ParentColumns)
                {
                    XmlNode nFilter = doc.CreateElement("Filter", nBodyRptItems.NamespaceURI);
                    nFilters.AppendChild(nFilter);
                    XmlNode nFilterExpression = doc.CreateElement("FilterExpression", nBodyRptItems.NamespaceURI);
                    nFilterExpression.InnerText = "=Fields!" + item.ColumnName + ".Value";
                    nFilter.AppendChild(nFilterExpression);
                    XmlNode nOperator = doc.CreateElement("Operator", nBodyRptItems.NamespaceURI);
                    nOperator.InnerText = "Equal";
                    nFilter.AppendChild(nOperator);
                    XmlNode nFilterValues = doc.CreateElement("FilterValues", nBodyRptItems.NamespaceURI);
                    nFilter.AppendChild(nFilterValues);
                    XmlNode nFilterValue = doc.CreateElement("FilterValue", nBodyRptItems.NamespaceURI);
                    nFilterValue.InnerText = "=Parameters!p" + item.ColumnName + ".Value";
                    nFilterValues.AppendChild(nFilterValue);
                }
                GenReportParameters(doc, nsmgr, BlockItem);
            }

            doc.Save(rdlcFileName);
        }

        private void CreateGroup(XmlNode nTablix, XmlDocument doc, ReportTable rt, DataTable dtDataDic, XmlNode nBodyRptItems)
        {
            XmlNode nTablixRowHierarchy = doc.CreateElement("TablixRowHierarchy", nBodyRptItems.NamespaceURI);
            nTablix.AppendChild(nTablixRowHierarchy);
            XmlNode nTablixMembers2 = doc.CreateElement("TablixMembers", nBodyRptItems.NamespaceURI);
            nTablixRowHierarchy.AppendChild(nTablixMembers2);
            List<ReportColumn> lstGroupConditionFields = GetGroupConditionRptColumns(rt);

            int index = 0;
            String str = String.Empty;
            XmlNode nTablixMember3 = doc.CreateElement("TablixMember", nBodyRptItems.NamespaceURI);
            str += "<TablixHeader>";
            str += "<Size>2.5cm</Size>";
            str += "<CellContents>";
            str += "<Textbox Name=\"Textbox_Group_" + lstGroupConditionFields[index].ColumnName + "\">";
            str += "<CanGrow>true</CanGrow><KeepTogether>true</KeepTogether>";
            str += "<Paragraphs><Paragraph><TextRuns><TextRun><Value>";
            str += GetFieldCaption(dtDataDic, lstGroupConditionFields[index].ColumnName);
            str += "</Value><Style><FontFamily>PMingLiU</FontFamily></Style></TextRun></TextRuns><Style /></Paragraph></Paragraphs>";
            str += "<Style><Border><Style>Solid</Style></Border><BackgroundColor>SteelBlue</BackgroundColor>";
            str += "<PaddingLeft>2pt</PaddingLeft><PaddingRight>2pt</PaddingRight><PaddingTop>2pt</PaddingTop><PaddingBottom>2pt</PaddingBottom></Style>";
            //str += "<rd:DefaultName>\"Textbox_Group_" + cdtField.ColumnName + "\"</rd:DefaultName>";
            str += "</Textbox></CellContents></TablixHeader>";
            str += "<TablixMembers><TablixMember>";
            if (index + 1 < lstGroupConditionFields.Count)
                str += CreateGroup1(dtDataDic, index, lstGroupConditionFields);
            else
                str += "<TablixMembers><TablixMember><KeepTogether>true</KeepTogether></TablixMember></TablixMembers>";
            str += "</TablixMember></TablixMembers>";
            str += "<KeepWithGroup>After</KeepWithGroup><RepeatOnNewPage>true</RepeatOnNewPage>";
            nTablixMember3.InnerXml = str;
            nTablixMembers2.AppendChild(nTablixMember3);

            str = String.Empty;
            XmlNode nTablixMember4 = doc.CreateElement("TablixMember", nBodyRptItems.NamespaceURI);
            str += "<Group Name=\"" + lstGroupConditionFields[index].ColumnName + "\">";
            str += "<GroupExpressions><GroupExpression>=Fields!" + lstGroupConditionFields[index].ColumnName + ".Value</GroupExpression></GroupExpressions>";
            str += "</Group>";
            str += "<SortExpressions><SortExpression><Value>=Fields!" + lstGroupConditionFields[index].ColumnName + ".Value</Value>";
            str += "</SortExpression></SortExpressions>";
            str += "<TablixHeader><Size>2.5cm</Size><CellContents>";
            str += "<Textbox Name=\"" + lstGroupConditionFields[index].ColumnName + "\">";
            str += "<CanGrow>true</CanGrow><KeepTogether>true</KeepTogether>";
            str += "<Paragraphs><Paragraph><TextRuns><TextRun><Value>";
            str += "=Fields!" + lstGroupConditionFields[index].ColumnName + ".Value";
            str += "</Value><Style><FontFamily>PMingLiU</FontFamily></Style></TextRun></TextRuns><Style /></Paragraph></Paragraphs>";
            str += "<Style><Border><Style>Solid</Style></Border><BackgroundColor>SteelBlue</BackgroundColor>";
            str += "<PaddingLeft>2pt</PaddingLeft><PaddingRight>2pt</PaddingRight><PaddingTop>2pt</PaddingTop><PaddingBottom>2pt</PaddingBottom></Style>";
            //str += "<rd:DefaultName>\"" + cdtField.ColumnName + "\"</rd:DefaultName>";
            str += "</Textbox></CellContents></TablixHeader>";
            str += "<TablixMembers><TablixMember>";
            if (index + 1 < lstGroupConditionFields.Count)
                str += CreateGroup2(dtDataDic, index, lstGroupConditionFields);
            else
            {
                str += "<Group Name=\"table1_Details_Group\"><DataElementName>Detail</DataElementName></Group>";
                str += "<TablixMembers><TablixMember /></TablixMembers><DataElementName>Detail_Collection</DataElementName>";
                str += "<DataElementOutput>Output</DataElementOutput><KeepTogether>true</KeepTogether>";
                //str += "<TablixMember><KeepWithGroup>Before</KeepWithGroup></TablixMember>";
            }
            str += "</TablixMember></TablixMembers>";
            nTablixMember4.InnerXml = str;
            nTablixMembers2.AppendChild(nTablixMember4);
            nTablixMembers2.InnerXml = nTablixMembers2.InnerXml.Replace(" xmlns=\"\"", String.Empty);
        }

        private string CreateGroup1(DataTable dtDataDic, int index, List<ReportColumn> lstGroupConditionFields)
        {
            index++;
            String str = String.Empty;
            str += "<TablixHeader>";
            str += "<Size>2.5cm</Size>";
            str += "<CellContents>";
            str += "<Textbox Name=\"Textbox_Group_" + lstGroupConditionFields[index].ColumnName + "\">";
            str += "<CanGrow>true</CanGrow><KeepTogether>true</KeepTogether>";
            str += "<Paragraphs><Paragraph><TextRuns><TextRun><Value>";
            str += GetFieldCaption(dtDataDic, lstGroupConditionFields[index].ColumnName);
            str += "</Value><Style><FontFamily>PMingLiU</FontFamily></Style></TextRun></TextRuns><Style /></Paragraph></Paragraphs>";
            str += "<Style><Border><Style>Solid</Style></Border><BackgroundColor>SteelBlue</BackgroundColor>";
            str += "<PaddingLeft>2pt</PaddingLeft><PaddingRight>2pt</PaddingRight><PaddingTop>2pt</PaddingTop><PaddingBottom>2pt</PaddingBottom></Style>";
            //str += "<rd:DefaultName>\"Textbox_Group_" + cdtField.ColumnName + "\"</rd:DefaultName>";
            str += "</Textbox></CellContents></TablixHeader>";
            if (index + 1 < lstGroupConditionFields.Count)
                str += CreateGroup1(dtDataDic, index, lstGroupConditionFields);
            else
                str += "<TablixMembers><TablixMember><KeepTogether>true</KeepTogether></TablixMember></TablixMembers>";
            return str;
        }

        private string CreateGroup2(DataTable dtDataDic, int index, List<ReportColumn> lstGroupConditionFields)
        {
            index++;
            String str = String.Empty;
            str += "<Group Name=\"" + lstGroupConditionFields[index].ColumnName + "\">";
            str += "<GroupExpressions><GroupExpression>=Fields!" + lstGroupConditionFields[index].ColumnName + ".Value</GroupExpression></GroupExpressions>";
            str += "</Group>";
            str += "<SortExpressions><SortExpression><Value>=Fields!" + lstGroupConditionFields[index].ColumnName + ".Value</Value>";
            str += "</SortExpression></SortExpressions>";
            str += "<TablixHeader><Size>2.5cm</Size><CellContents>";
            str += "<Textbox Name=\"" + lstGroupConditionFields[index].ColumnName + "\">";
            str += "<CanGrow>true</CanGrow><KeepTogether>true</KeepTogether>";
            str += "<Paragraphs><Paragraph><TextRuns><TextRun><Value>";
            str += "=Fields!" + lstGroupConditionFields[index].ColumnName + ".Value";
            str += "</Value><Style><FontFamily>PMingLiU</FontFamily></Style></TextRun></TextRuns><Style /></Paragraph></Paragraphs>";
            str += "<Style><Border><Style>Solid</Style></Border><BackgroundColor>SteelBlue</BackgroundColor>";
            str += "<PaddingLeft>2pt</PaddingLeft><PaddingRight>2pt</PaddingRight><PaddingTop>2pt</PaddingTop><PaddingBottom>2pt</PaddingBottom></Style>";
            //str += "<rd:DefaultName>\"" + cdtField.ColumnName + "\"</rd:DefaultName>";
            str += "</Textbox></CellContents></TablixHeader>";
            str += "<TablixMembers><TablixMember>";
            if (index + 1 < lstGroupConditionFields.Count)
                str += CreateGroup2(dtDataDic, index, lstGroupConditionFields);
            else
            {
                str += "<Group Name=\"table1_Details_Group\"><DataElementName>Detail</DataElementName></Group>";
                str += "<TablixMembers><TablixMember /></TablixMembers><DataElementName>Detail_Collection</DataElementName>";
                str += "<DataElementOutput>Output</DataElementOutput><KeepTogether>true</KeepTogether>";
                //str += "<TablixMember><KeepWithGroup>Before</KeepWithGroup>";
            }
            str += "</TablixMember></TablixMembers>";
            return str;
        }

        private bool GetForm()
        {
            String TemplatePath = String.Empty;
            //TemplatePath = FClientData.WebSiteName + "Template";//EEPRegistry.WebClient + "\\Template";
            if (FClientData.WebSiteFullName.Contains("localhost"))
            {
                String[] webSiteNames = FClientData.WebSiteName.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                String[] webClients = EEPRegistry.WebClient.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
                String newPath = String.Empty;
                for (int i = 0; i < webClients.Length - 1; i++)
                {
                    newPath += webClients[i] + "\\";
                }
                newPath = System.IO.Path.Combine(newPath, webSiteNames[webSiteNames.Length - 1]);
                TemplatePath = System.IO.Path.Combine(newPath, "Template");
                FClientData.WebSiteFullName = newPath;
            }
            else
            {
                TemplatePath = System.IO.Path.Combine(FClientData.WebSiteFullName, "Template");
            }
            if (TemplatePath == "")
            {
                MessageBox.Show("Cannot find WebTemplate path: {0}", TemplatePath);
                return false;
            }
            if (FPIFolder != null)
            {
                bool flag = false;
                foreach (ProjectItem aPI in FPIFolder.ProjectItems)
                {
                    if (string.Compare(FClientData.FormName + ".rdlc", aPI.Name) == 0)
                    {
                        DialogResult dr = DialogResult.No;
                        if (!flag)
                            dr = MessageBox.Show("There is another File which name is " + FClientData.PackageName + " existed! Do you want to delete it first", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        else
                            dr = DialogResult.Yes;
                        if (dr == DialogResult.Yes)
                        {
                            flag = true;
                            string Path = aPI.get_FileNames(0);
                            aPI.Name = Guid.NewGuid().ToString();
                            aPI.Delete();
                            File.Delete(Path);
                        }
                        else
                        {
                            return false;
                        }
                        continue;
                    }
                    if (string.Compare(FClientData.BaseFormName + ".rdlc", aPI.Name) == 0)
                    {
                        string Path = aPI.get_FileNames(0);
                        aPI.Name = Guid.NewGuid().ToString();
                        aPI.Delete();
                        File.Delete(Path);
                    }

                    if (FClientData.IsMasterDetailBaseForm())
                    {
                        if (string.Compare(FClientData.FormName + "-dt.rdlc", aPI.Name) == 0)
                        {
                            DialogResult dr = DialogResult.No;
                            if (!flag)
                                dr = MessageBox.Show("There is another File which name is " + FClientData.PackageName + " existed! Do you want to delete it first", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                            else
                                dr = DialogResult.Yes;
                            if (dr == DialogResult.Yes)
                            {
                                flag = true;
                                string Path = aPI.get_FileNames(0);
                                aPI.Name = Guid.NewGuid().ToString();
                                aPI.Delete();
                                File.Delete(Path);
                            }
                            else
                            {
                                return false;
                            }
                            continue;
                        }
                        if (string.Compare(FClientData.BaseFormName + "-dt.rdlc", aPI.Name) == 0)
                        {
                            string Path = aPI.get_FileNames(0);
                            aPI.Name = Guid.NewGuid().ToString();
                            aPI.Delete();
                            File.Delete(Path);
                        }
                    }
                }

                FPI = null;
                FPI = FPIFolder.ProjectItems.AddFromFileCopy(TemplatePath + "\\" + FClientData.BaseFormName + ".rdlc");
                FPI.Name = FClientData.FormName + ".rdlc";// Guid.NewGuid().ToString() + ".rdlc";
            }
            else
            {
                foreach (ProjectItem aPI in FProject.ProjectItems)
                {
                    if (string.Compare(FClientData.FormName + ".rdlc", aPI.Name) == 0)
                    {
                        string Path = aPI.get_FileNames(0);
                        Path = System.IO.Path.GetDirectoryName(Path);
                        aPI.Delete();
                        break;
                    }
                }

                FPI = FProject.ProjectItems.AddFromFileCopy(TemplatePath + "\\" + FClientData.BaseFormName + ".rdlc");
                FPI.Name = FClientData.FormName + ".rdlc";
            }

            return true;
        }

        public static DataTable GetDDTable(ClientType databaseType, string dbName, string tableName)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(SystemFile.DBFile);
            XmlNode nDataBase = doc.SelectSingleNode("InfolightDB/DataBase");
            foreach (XmlNode node in nDataBase.ChildNodes)
            {
                if (node.Name == dbName)
                {
                    string conString = node.Attributes["String"].Value;
                    string password = WzdUtils.GetPwdString(node.Attributes["Password"].Value);
                    if ((conString.Length > 0) && (password.Length > 0) && password != String.Empty)
                    {
                        if (conString[conString.Length - 1] != ';')
                        {
                            conString = conString + ";Password=" + password;
                        }
                        else
                        {
                            conString = conString + "Password=" + password;
                        }
                    }
                    //Connection
                    DbConnection con = WzdUtils.AllocateConnection(dbName, databaseType, false);
                    //Command
                    InfoCommand cmd = new InfoCommand(databaseType);
                    cmd.Connection = con;
                    tableName = WzdUtils.RemoveQuote(tableName, databaseType);
                    String OWNER = String.Empty, SS = tableName;
                    if (SS.Contains("."))
                    {
                        OWNER = WzdUtils.GetToken(ref SS, new char[] { '.' });
                        tableName = SS;
                    }
                    cmd.CommandText = "Select * from COLDEF where TABLE_NAME = '" + tableName + "' OR TABLE_NAME='" + OWNER + "." + tableName + "'";
                    //Adapter
                    IDbDataAdapter adapter = WzdUtils.AllocateDataAdapter(databaseType);
                    adapter.SelectCommand = cmd.GetInternalCommand();
                    DataTable tab = new DataTable();
                    WzdUtils.FillDataAdapter(databaseType, adapter, tab);
                    return tab;
                }
            }

            return null;
        }

        public void GenDataSources(XmlDocument doc, XmlNamespaceManager nsmgr)
        {
            XmlNode nReport = doc.SelectSingleNode("ms:Report", nsmgr);
            //DataSources
            XmlNode nDataSources = doc.CreateElement("DataSources", nReport.NamespaceURI);
            //DataSources.DataSource
            XmlNode nDataSource = doc.CreateElement("DataSource", nReport.NamespaceURI);
            XmlAttribute aName = doc.CreateAttribute("Name");
            aName.Value = "DummyDataSource";
            nDataSource.Attributes.Append(aName);
            //DataSources.DataSource.ConnectionProperties
            XmlNode nConnectionProperties = doc.CreateElement("ConnectionProperties", nReport.NamespaceURI);
            //DataSources.DataSource.ConnectionProperties.ConnectString
            XmlNode nConnectString = doc.CreateElement("ConnectString", nReport.NamespaceURI);
            nConnectString.InnerText = FClientData.ConnString;// "Data Source=" + Environment.UserName + ";Initial Catalog=aspnetdb;Integrated Security=True;";
            nConnectionProperties.AppendChild(nConnectString);
            //DataSources.DataSource.ConnectionProperties.DataProvider
            XmlNode nDataProvider = doc.CreateElement("DataProvider", nReport.NamespaceURI);
            nDataProvider.InnerText = "SQL";
            nConnectionProperties.AppendChild(nDataProvider);
            nDataSource.AppendChild(nConnectionProperties);
            //DataSources.DataSource.DataSourceID
            XmlNode nDataSourceID = doc.CreateElement("rd", "DataSourceID", rd);
            nDataSourceID.InnerText = Guid.NewGuid().ToString();
            nDataSource.AppendChild(nDataSourceID);
            nDataSources.AppendChild(nDataSource);
            nReport.AppendChild(nDataSources);
        }

        public void GenDataSets(XmlDocument doc, XmlNamespaceManager nsmgr, TBlockItem BlockItem)
        {
            XmlNode nReport = doc.SelectSingleNode("ms:Report", nsmgr);
            //DataSets
            XmlNode nDataSets = doc.CreateElement("DataSets", nReport.NamespaceURI);

            //DataSet
            XmlNode nDataSet = doc.CreateElement("DataSet", nReport.NamespaceURI);
            XmlAttribute aDataSetName = doc.CreateAttribute("Name");
            aDataSetName.InnerText = BlockItem.TableName;// "NewDataSet";// +BlockItem.TableName;
            if (doc.BaseURI.EndsWith("-dt.rdlc"))
            {
                aDataSetName.InnerText = BlockItem.Relation.ParentTable.TableName;// "NewDataSet";
            }
            nDataSet.Attributes.Append(aDataSetName);
            //DataSet.DataSetInfo
            XmlNode nDataSetInfo = doc.CreateElement("rd", "DataSetInfo", rd);
            //DataSet.DataSetInfo.DataSetName
            XmlNode nDataSetName = doc.CreateElement("DataSetName");
            nDataSetName.InnerText = FClientData.FormName;// BlockItem.TableName;// "NewDataSet";
            //if (doc.BaseURI.EndsWith("-dt.rdlc"))
            //{
            //    nDataSetName.InnerText = BlockItem.Relation.ParentTable.TableName;// "NewDataSet";
            //}
            nDataSetInfo.AppendChild(nDataSetName);
            //DataSet.DataSetInfo.SchemaPath
            //XmlNode nSchemaPath = doc.CreateElement("rd", "SchemaPath", rd);
            //nSchemaPath.InnerText = @"C:\Program Files\Infolight\EEP2012\JQWebClient\Test0702\Form11.xsd";
            //nDataSetInfo.AppendChild(nSchemaPath);
            //DataSet.DataSetInfo.TableName
            XmlNode nTableName = doc.CreateElement("rd", "TableName", rd);
            nTableName.InnerText = BlockItem.TableName;
            nDataSetInfo.AppendChild(nTableName);
            nDataSet.AppendChild(nDataSetInfo);
            //DataSet.Query
            XmlNode nQuery = doc.CreateElement("Query", nReport.NamespaceURI);
            //DataSet.Query.UseGenericDesigner
            XmlNode nUseGenericDesigner = doc.CreateElement("rd", "UseGenericDesigner", rd);
            nUseGenericDesigner.InnerText = "true";
            nQuery.AppendChild(nUseGenericDesigner);
            //DataSet.Query.CommandText
            XmlNode nCommandText = doc.CreateElement("CommandText", nReport.NamespaceURI);
            nQuery.AppendChild(nCommandText);
            //DataSet.Query.DataSourceName
            XmlNode nDataSourceName = doc.CreateElement("DataSourceName", nReport.NamespaceURI);
            nDataSourceName.InnerText = "DummyDataSource";
            nQuery.AppendChild(nDataSourceName);
            nDataSet.AppendChild(nQuery);
            //DataSet.Fields
            XmlNode nFields = doc.CreateElement("Fields", nReport.NamespaceURI);
            foreach (TBlockFieldItem colDesigner in BlockItem.BlockFieldItems)
            {
                //DataSet.Fields.Field
                XmlNode nField = doc.CreateElement("Field", nReport.NamespaceURI);
                XmlAttribute aFieldName = doc.CreateAttribute("Name");
                aFieldName.InnerText = colDesigner.DataField;
                nField.Attributes.Append(aFieldName);
                //DataSet.Fields.Field.TypeName
                XmlNode nTypeName = doc.CreateElement("rd", "TypeName", rd);
                nTypeName.InnerText = colDesigner.DataType.ToString();
                nField.AppendChild(nTypeName);
                //DataSet.Fields.Field.DataField
                XmlNode nDataField = doc.CreateElement("DataField", nReport.NamespaceURI);
                nDataField.InnerText = colDesigner.DataField;
                nField.AppendChild(nDataField);
                nFields.AppendChild(nField);
            }
            nDataSet.AppendChild(nFields);
            nDataSets.AppendChild(nDataSet);
            nReport.AppendChild(nDataSets);
        }

        public void GenReportParameters(XmlDocument doc, XmlNamespaceManager nsmgr, TBlockItem BlockItem)
        {
            XmlNode nReport = doc.SelectSingleNode("ms:Report", nsmgr);
            //DataSets
            XmlNode nReportParameters = doc.CreateElement("ReportParameters", nReport.NamespaceURI);
            nReport.AppendChild(nReportParameters);
            foreach (var item in BlockItem.Relation.ParentColumns)
            {
                XmlNode nReportParameter = doc.CreateElement("ReportParameter", nReport.NamespaceURI);
                XmlAttribute aReportParameterName = doc.CreateAttribute("Name");
                aReportParameterName.InnerText = "p" + item.ColumnName;
                nReportParameter.Attributes.Append(aReportParameterName);
                nReportParameters.AppendChild(nReportParameter);
                XmlNode nDataType = doc.CreateElement("DataType", nReport.NamespaceURI);
                if (item.DataType == typeof(String))
                    nDataType.InnerText = "String";
                else if (item.DataType == typeof(bool))
                    nDataType.InnerText = "Boolean";
                else if (item.DataType == typeof(DateTime))
                    nDataType.InnerText = "DateTime";
                else if (item.DataType == typeof(int))
                    nDataType.InnerText = "Integer";
                else if (item.DataType == typeof(float) || item.DataType == typeof(double) || item.DataType == typeof(decimal))
                    nDataType.InnerText = "Float";
                nReportParameter.AppendChild(nDataType);
                XmlNode nAllowBlank = doc.CreateElement("AllowBlank", nReport.NamespaceURI);
                nAllowBlank.InnerText = "true";
                nReportParameter.AppendChild(nAllowBlank);
                XmlNode nPrompt = doc.CreateElement("Prompt", nReport.NamespaceURI);
                nPrompt.InnerText = item.ColumnName;
                nReportParameter.AppendChild(nPrompt);
            }
        }

        private static bool HasGroupCondition(ReportTable rptTable)
        {
            foreach (ReportColumn rptCol in rptTable.ReportColumns)
            {
                if (rptCol.IsGroupCondition)
                    return true;
            }
            return false;
        }

        private static XmlNode GetTitleNode(XmlDocument doc, XmlNamespaceManager nsmgr, bool isLabelStyle)
        {
            string path = "ms:Report/ms:Page/ms:PageHeader/ms:ReportItems";

            XmlNode startNode = doc.SelectSingleNode(path, nsmgr);
            if (startNode != null)
                return startNode.SelectSingleNode("ms:Textbox[@Name='txtTitle']", nsmgr);
            else
                return null;
        }

        public static string GetFieldEditMask(DataTable tabDataDic, string fieldName)
        {
            if (tabDataDic != null && tabDataDic.Rows.Count > 0)
            {
                foreach (DataRow row in tabDataDic.Rows)
                {
                    if (row["FIELD_NAME"] != null && row["FIELD_NAME"].ToString().ToLower() == fieldName.ToLower())
                    {
                        if (row["EDITMASK"] != null && !string.IsNullOrEmpty(row["EDITMASK"].ToString()))
                        {
                            return "=Format(Fields!" + fieldName + ".Value,\"" + row["EDITMASK"].ToString() + "\")";
                        }
                        else
                        {
                            if (row["FIELD_TYPE"] != null)
                            {
                                switch (row["FIELD_TYPE"].ToString().ToLower())
                                {
                                    case "datetime":
                                        return "=Format(Fields!" + fieldName + ".Value,\"d\")";
                                    case "money":
                                        return "=Format(Fields!" + fieldName + ".Value,\"C2\")";
                                }
                            }
                        }
                    }
                }
            }
            return "=Fields!" + fieldName + ".Value";
        }

        public static string GetFieldCaption(DataTable tabDataDic, string fieldName)
        {
            if (tabDataDic != null && tabDataDic.Rows.Count > 0)
            {
                foreach (DataRow row in tabDataDic.Rows)
                {
                    if (row["FIELD_NAME"] != null && row["CAPTION"] != null
                        && row["FIELD_NAME"].ToString().ToLower() == fieldName.ToLower()
                        && !string.IsNullOrEmpty(row["CAPTION"].ToString()))
                    {
                        return row["CAPTION"].ToString();
                    }
                }
            }
            return fieldName;
        }

        public static void GenLabel(XmlNode parentNode, float left, float top, string name, string caption, string HorGaps, string VertGaps, bool isTableCell, String textAlign)
        {
            XmlDocument doc = parentNode.OwnerDocument;

            XmlNode nLabel = doc.CreateElement("Textbox", parentNode.NamespaceURI);
            XmlAttribute aName = doc.CreateAttribute("Name");
            aName.Value = name;
            nLabel.Attributes.Append(aName);
            string temp = name.Substring(0, 2);

            if (!isTableCell)
            {
                // Left
                XmlNode nLeft = doc.CreateElement("Left", parentNode.NamespaceURI);
                nLeft.InnerText = left.ToString() + "cm";
                nLabel.AppendChild(nLeft);
                // Top
                XmlNode nTop = doc.CreateElement("Top", parentNode.NamespaceURI);
                nTop.InnerText = top.ToString() + "cm";
                nLabel.AppendChild(nTop);
                // Width
                XmlNode nWidth = doc.CreateElement("Width", parentNode.NamespaceURI);
                nWidth.InnerText = HorGaps + "cm";
                nLabel.AppendChild(nWidth);
                // Height
                XmlNode nHeight = doc.CreateElement("Height", parentNode.NamespaceURI);
                nHeight.InnerText = VertGaps + "cm";
                nLabel.AppendChild(nHeight);
            }
            // DefaultName
            XmlNode nDefaultName = doc.CreateElement("rd", "DefaultName", rd);
            nDefaultName.InnerText = name;
            nLabel.AppendChild(nDefaultName);
            // ZIndex
            XmlNode nZIndex = doc.CreateElement("ZIndex", parentNode.NamespaceURI);
            nZIndex.InnerText = "2";
            nLabel.AppendChild(nZIndex);
            // Style
            XmlNode nStyle = doc.CreateElement("Style", parentNode.NamespaceURI);
            // Style.PaddingLeft
            XmlNode nPaddingLeft = doc.CreateElement("PaddingLeft", parentNode.NamespaceURI);
            nPaddingLeft.InnerText = "2pt";
            nStyle.AppendChild(nPaddingLeft);
            // Style.PaddingTop
            XmlNode nPaddingTop = doc.CreateElement("PaddingTop", parentNode.NamespaceURI);
            nPaddingTop.InnerText = "2pt";
            nStyle.AppendChild(nPaddingTop);
            // Style.PaddingRight
            XmlNode nPaddingRight = doc.CreateElement("PaddingRight", parentNode.NamespaceURI);
            nPaddingRight.InnerText = "2pt";
            nStyle.AppendChild(nPaddingRight);
            // Style.PaddingBottom
            XmlNode nPaddingBottom = doc.CreateElement("PaddingBottom", parentNode.NamespaceURI);
            nPaddingBottom.InnerText = "2pt";
            nStyle.AppendChild(nPaddingBottom);
            nLabel.AppendChild(nStyle);

            // CanGrow
            XmlNode nCanGrow = doc.CreateElement("CanGrow", parentNode.NamespaceURI);
            nCanGrow.InnerText = "true";
            nLabel.AppendChild(nCanGrow);

            // Value
            XmlNode nKeepTogether = doc.CreateElement("KeepTogether", parentNode.NamespaceURI);
            nKeepTogether.InnerText = "true";
            nLabel.AppendChild(nKeepTogether);

            XmlNode nParagraphs = doc.CreateElement("Paragraphs", parentNode.NamespaceURI);
            nLabel.AppendChild(nParagraphs);
            XmlNode nParagraph = doc.CreateElement("Paragraph", parentNode.NamespaceURI);
            nParagraphs.AppendChild(nParagraph);
            XmlNode nTextRuns = doc.CreateElement("TextRuns", parentNode.NamespaceURI);
            nParagraph.AppendChild(nTextRuns);
            XmlNode nTextRun = doc.CreateElement("TextRun", parentNode.NamespaceURI);
            nTextRuns.AppendChild(nTextRun);
            XmlNode nParagraphStyle = doc.CreateElement("Style", parentNode.NamespaceURI);
            nParagraph.AppendChild(nParagraphStyle);
            XmlNode nParagraphStyleTextAlign = doc.CreateElement("TextAlign", parentNode.NamespaceURI);
            if (String.IsNullOrEmpty(textAlign))
                textAlign = "Default";
            nParagraphStyleTextAlign.InnerText = textAlign;
            nParagraphStyle.AppendChild(nParagraphStyleTextAlign);
            // Value
            XmlNode nValue = doc.CreateElement("Value", parentNode.NamespaceURI);
            nValue.InnerText = caption;
            nTextRun.AppendChild(nValue);
            // Sytle.FontFamily
            XmlNode nStyle2 = doc.CreateElement("Style", parentNode.NamespaceURI);
            XmlNode nFontFamily = doc.CreateElement("FontFamily", parentNode.NamespaceURI);
            nFontFamily.InnerText = "PMingLiU";
            nStyle2.AppendChild(nFontFamily);
            nTextRun.AppendChild(nStyle2);

            //            <Style>
            //  <PaddingLeft>2pt</PaddingLeft>
            //  <PaddingRight>2pt</PaddingRight>
            //  <PaddingTop>2pt</PaddingTop>
            //  <PaddingBottom>2pt</PaddingBottom>
            //</Style>

            parentNode.AppendChild(nLabel);
        }

        public static void GenTableLabel(XmlNode parentNode, float left, float top, string name, string caption, string HorGaps, string VertGaps, bool isTableCell, string textAlign)
        {
            XmlDocument doc = parentNode.OwnerDocument;

            XmlNode nTablixCell = doc.CreateElement("TablixCell", parentNode.NamespaceURI);
            parentNode.AppendChild(nTablixCell);
            XmlNode nCellContents = doc.CreateElement("CellContents", parentNode.NamespaceURI);
            nTablixCell.AppendChild(nCellContents);

            XmlNode nLabel = doc.CreateElement("Textbox", parentNode.NamespaceURI);
            XmlAttribute aName = doc.CreateAttribute("Name");
            aName.Value = name;
            nLabel.Attributes.Append(aName);
            string temp = name.Substring(0, 2);

            // DefaultName
            XmlNode nDefaultName = doc.CreateElement("rd", "DefaultName", rd);
            nDefaultName.InnerText = name;
            nLabel.AppendChild(nDefaultName);
            // ZIndex
            XmlNode nZIndex = doc.CreateElement("ZIndex", parentNode.NamespaceURI);
            nZIndex.InnerText = "2";
            nLabel.AppendChild(nZIndex);
            // Style
            XmlNode nStyle = doc.CreateElement("Style", parentNode.NamespaceURI);
            // Style.PaddingLeft
            XmlNode nPaddingLeft = doc.CreateElement("PaddingLeft", parentNode.NamespaceURI);
            nPaddingLeft.InnerText = "2pt";
            nStyle.AppendChild(nPaddingLeft);
            // Style.PaddingTop
            XmlNode nPaddingTop = doc.CreateElement("PaddingTop", parentNode.NamespaceURI);
            nPaddingTop.InnerText = "2pt";
            nStyle.AppendChild(nPaddingTop);
            // Style.PaddingRight
            XmlNode nPaddingRight = doc.CreateElement("PaddingRight", parentNode.NamespaceURI);
            nPaddingRight.InnerText = "2pt";
            nStyle.AppendChild(nPaddingRight);
            // Style.PaddingBottom
            XmlNode nPaddingBottom = doc.CreateElement("PaddingBottom", parentNode.NamespaceURI);
            nPaddingBottom.InnerText = "2pt";
            nStyle.AppendChild(nPaddingBottom);

            //Style.BorderStyle
            XmlNode nBorderStyle = doc.CreateElement("Border", parentNode.NamespaceURI);
            XmlNode nBorderDefault = doc.CreateElement("Style", parentNode.NamespaceURI);
            nBorderDefault.InnerText = "Solid";
            nBorderStyle.AppendChild(nBorderDefault);
            nStyle.AppendChild(nBorderStyle);

            if (temp == "hd")
            {
                XmlNode nBackgroundColor = doc.CreateElement("BackgroundColor", parentNode.NamespaceURI);
                nBackgroundColor.InnerText = "SteelBlue";
                nStyle.AppendChild(nBackgroundColor);
            }
            nLabel.AppendChild(nStyle);

            // CanGrow
            XmlNode nCanGrow = doc.CreateElement("CanGrow", parentNode.NamespaceURI);
            nCanGrow.InnerText = "true";
            nLabel.AppendChild(nCanGrow);

            // Value
            XmlNode nKeepTogether = doc.CreateElement("KeepTogether", parentNode.NamespaceURI);
            nKeepTogether.InnerText = "true";
            nLabel.AppendChild(nKeepTogether);

            XmlNode nParagraphs = doc.CreateElement("Paragraphs", parentNode.NamespaceURI);
            nLabel.AppendChild(nParagraphs);
            XmlNode nParagraph = doc.CreateElement("Paragraph", parentNode.NamespaceURI);
            nParagraphs.AppendChild(nParagraph);
            XmlNode nTextRuns = doc.CreateElement("TextRuns", parentNode.NamespaceURI);
            nParagraph.AppendChild(nTextRuns);
            XmlNode nTextRun = doc.CreateElement("TextRun", parentNode.NamespaceURI);
            nTextRuns.AppendChild(nTextRun);
            XmlNode nParagraphStyle = doc.CreateElement("Style", parentNode.NamespaceURI);
            nParagraph.AppendChild(nParagraphStyle);
            XmlNode nParagraphStyleTextAlign = doc.CreateElement("TextAlign", parentNode.NamespaceURI);
            if (String.IsNullOrEmpty(textAlign))
                textAlign = "Default";
            nParagraphStyleTextAlign.InnerText = textAlign;
            nParagraphStyle.AppendChild(nParagraphStyleTextAlign);
            // Value
            XmlNode nValue = doc.CreateElement("Value", parentNode.NamespaceURI);
            nValue.InnerText = caption;
            nTextRun.AppendChild(nValue);
            // Sytle.FontFamily
            XmlNode nStyle2 = doc.CreateElement("Style", parentNode.NamespaceURI);
            XmlNode nFontFamily = doc.CreateElement("FontFamily", parentNode.NamespaceURI);
            nFontFamily.InnerText = "PMingLiU";
            nStyle2.AppendChild(nFontFamily);
            nTextRun.AppendChild(nStyle2);

            nCellContents.AppendChild(nLabel);
        }

        private static void GenReportBodyHeight(XmlDocument doc, float curTop, XmlNamespaceManager nsmgr, bool isMaster)
        {
            string path = "ms:Report/ms:Body/ms:Height";
            XmlNode HeightNode = doc.SelectSingleNode(path, nsmgr);
            //if (isMaster)
            //{
            //    HeightNode.InnerText = Convert.ToString(curTop + 4) + "cm";
            //    path = "ms:Report/ms:Body/ms:ReportItems/ms:List[@Name='lstContainer']/ms:ReportItems/ms:Rectangle[@Name='rectContainer']/ms:ReportItems/ms:Subreport[@Name='subreport1']/ms:Top";
            //    XmlNode SubTopNode = doc.SelectSingleNode(path, nsmgr);
            //    SubTopNode.InnerText = Convert.ToString(curTop + 1) + "cm";
            //}
            //else
            {
                HeightNode.InnerText = Convert.ToString(curTop + 0.5) + "cm";
            }
        }

        private static List<ReportColumn> GetGroupConditionRptColumns(ReportTable rptTable)
        {
            List<ReportColumn> lstGCols = new List<ReportColumn>();
            foreach (ReportColumn rptCol in rptTable.ReportColumns)
            {
                if (rptCol.IsGroupCondition)
                    lstGCols.Add(rptCol);
            }
            return lstGCols;
        }

        private void SetBlockFieldControls(TBlockItem BlockItem)
        {
            //String FileName = Path.Combine(FClientData.WebSiteName, FClientData.FolderName, FClientData.FormName + ".rdlc");
            String FileName = "";
            if (FClientData.WebSiteFullName.Contains("localhost"))
                FileName = System.IO.Path.Combine(EEPRegistry.WebClient, FClientData.FolderName, FClientData.FormName + ".rdlc");
            else
                FileName = System.IO.Path.Combine(FClientData.WebSiteFullName, FClientData.FolderName, FClientData.FormName + ".rdlc");
            if (BlockItem.Name == "Main" || BlockItem.Name == "Master")
            {
                if (FClientData.BaseFormName == "SingleLabel")
                {
                    GenSingleLabelRdlc(FileName, BlockItem, false);
                }
                else if (FClientData.BaseFormName == "SingleTable")
                {
                    GenSingleTableRdlc(FileName, BlockItem, false);
                }
                else if (FClientData.BaseFormName == "MasterDetail")
                {
                    TBlockItem BlockItemMaster = BlockItem;
                    BlockItemMaster.wDataSource = new WebDataSource();
                    TBlockItem BlockItemDetail = null;
                    foreach (TBlockItem B in FClientData.Blocks)
                    {
                        if (B.wDataSource == null)
                        {
                            BlockItemDetail = B;
                            break;
                        }
                    }
                    GenSingleLabelRdlc(FileName, BlockItemMaster, true, FClientData.FormName + "-dt", BlockItemDetail.Relation.ParentColumns);
                    GenDetailBlock(BlockItemDetail);
                }
            }
        }

        private void GenDetailBlock(TBlockItem BlockItem)
        {
            String TemplatePath = FClientData.WebSiteName + "Template";
            if (FClientData.WebSiteFullName.Contains("localhost"))
                TemplatePath = System.IO.Path.Combine(EEPRegistry.WebClient, "Template");
            else
                TemplatePath = System.IO.Path.Combine(FClientData.WebSiteFullName, "Template");
            ProjectItem piDetail = FPIFolder.ProjectItems.AddFromFileCopy(TemplatePath + "\\TableDetails.rdlc");
            piDetail.Name = FClientData.FormName + "-dt.rdlc";
            String detailFileName = String.Empty;// System.IO.Path.Combine(FClientData.WebSiteName, FClientData.FolderName, FClientData.FormName + "-dt.rdlc");
            if (FClientData.WebSiteFullName.Contains("localhost"))
            {
                detailFileName = System.IO.Path.Combine(EEPRegistry.WebClient, FClientData.FolderName, FClientData.FormName + "-dt.rdlc");
            }
            else
            {
                detailFileName = System.IO.Path.Combine(FClientData.WebSiteFullName, FClientData.FolderName, FClientData.FormName + "-dt.rdlc");
            }

            GenSingleTableRdlc(detailFileName, BlockItem, true);
        }

        private void GenDataSet()
        {
            WebDataSet aWebDataSet = new WebDataSet();

            if (aWebDataSet != null)
            {
                aWebDataSet.SetWizardDesignMode(true);
                aWebDataSet.RemoteName = FClientData.ProviderName;
                aWebDataSet.PacketRecords = 100;
                aWebDataSet.Active = true;

                String s;
                if (FClientData.WebSiteFullName.Contains("localhost"))
                    s = EEPRegistry.WebClient;
                else
                    s = FClientData.WebSiteFullName;

                string filePath = Path.Combine(s, FClientData.FolderName);
                bool CreateFileSucess = true;
                string fileName = "";
                try
                {
                    fileName = Path.Combine(filePath, FClientData.FormTitle + ".xsd");
                    aWebDataSet.RealDataSet.DataSetName = FClientData.FormTitle;
                    aWebDataSet.RealDataSet.WriteXmlSchema(fileName);
                }
                catch
                {
                    CreateFileSucess = false;
                    MessageBox.Show("Failed to create xsd file!");
                }
                finally
                {
                    if (CreateFileSucess && File.Exists(fileName))
                    {
                        FPIFolder.ProjectItems.AddFromFile(fileName);
                    }
                    if (aWebDataSet != null)
                    {
                        aWebDataSet.Dispose();
                    }
                }
            }

        }

        public void GenWebClientModule()
        {
            GenFolder();
            if (GetForm())
            {
                GenDataSet();
                //GetDesignerHost();
                try
                {
                    TBlockItem BlockItem;
                    if (FClientData.IsMasterDetailBaseForm())
                    {
                        BlockItem = FClientData.Blocks.FindItem("Master");
                        SetBlockFieldControls(BlockItem);
                    }
                    else
                    {
                        BlockItem = FClientData.Blocks.FindItem("Main");
                        SetBlockFieldControls(BlockItem);
                    }
                    //WriteWebDataSourceHTML();
                }
                catch (Exception exception2)
                {
                    MessageBox.Show(exception2.Message);
                    return;
                }
                finally
                {
                    FPI.Name = FClientData.FormName + ".rdlc";
                    FDesignWindow = FPI.Open("{7651A702-06E5-11D1-8EBD-00A0C90F26EA}");
                    FDesignWindow.Activate();
                }
                FProject.Save(FProject.FullName);
            }
        }
    }
}

//常数 值 说明 
//vsViewKindAny {FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF} 任何视图。确定是否在任何上下文中打开项。仅适用于 IsOpen 属性。 
//vsViewKindCode {7651A701-06E5-11D1-8EBD-00A0C90F26EA} “代码”视图。 
//vsViewKindDebugging {7651A700-06E5-11D1-8EBD-00A0C90F26EA} “调试器”视图。 
//vsViewKindDesigner {7651A702-06E5-11D1-8EBD-00A0C90F26EA} “设计器”视图。 
//vsViewKindPrimary {00000000-0000-0000-0000-000000000000} “主”视图。即，项的默认视图。 
//vsViewKindTextView {7651A703-06E5-11D1-8EBD-00A0C90F26EA} “正文”视图。 
