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
using Microsoft.VisualStudio.Designer.Interfaces;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.UI;
//using System.Web.UI.WebControls;
using System.Web.UI.Design;
using System.Web.UI.Design.WebControls;
using System.Globalization;
using System.Resources;
using System.Text.RegularExpressions;
using mshtml;
using InfoRemoteModule;
using AjaxTools;
#if VS90
using WebDevPage = Microsoft.VisualWebDeveloper.Interop.WebDeveloperPage;
using JQClientTools;
#endif


namespace MWizard2015
{
    public partial class fmJQueryToJQMobile : Form
    {
        private TJQueryToJQMobileFormData FClientData;
        private DTE2 FDTE2;
        private AddIn FAddIn;
        private DbConnection InternalConnection = null;
        private TStringList FAlias;
        private static string _serverPath;
        private InfoDataSet FInfoDataSet = null;
        private string[] FProviderNameList;
        public Boolean SDCall = false;

        public fmJQueryToJQMobile()
        {
            InitializeComponent();
            FClientData = new TJQueryToJQMobileFormData(this);
        }

        public fmJQueryToJQMobile(DTE2 aDTE2, AddIn aAddIn)
        {
            InitializeComponent();
            FClientData = new TJQueryToJQMobileFormData(this);
            FDTE2 = aDTE2;
            FAddIn = aAddIn;
        }

        public DbConnection GlobalConnection
        {
            get { return InternalConnection; }
        }

        public String SelectedAlias
        {
            get { return cbEEPAlias.Text; }
        }

        private void PrepareWizardService()
        {
            Show();
            Hide();
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
            tbFormName.Text = "Form1";
            ClearAll();
        }

        private void ClearAll()
        {

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
            btnNext.Enabled = tabControl.SelectedTab != tpFormSetting;
            btnDone.Enabled = tabControl.SelectedTab == tpFormSetting;
            btnCancel.Enabled = true;
        }

        private static string GetServerPath()
        {
            if ((fmJQueryToJQMobile._serverPath == null) || (fmJQueryToJQMobile._serverPath.Length == 0))
            {
                fmJQueryToJQMobile._serverPath = EEPRegistry.Server + "\\";
            }
            return fmJQueryToJQMobile._serverPath;
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

        public void ShowJQueryToJQMobileWebForm()
        {
            //Show();
            Init();
            ShowDialog();
        }

        public void SDGenWebForm(string XML)
        {
            SDCall = true;
            if (XML != "")
            {
                FClientData.Blocks.Clear();
                FClientData.LoadFromXML(XML);
            }
            TJQueryToJQMobileFormGenerator CG = new TJQueryToJQMobileFormGenerator(FClientData, FDTE2, FAddIn);
            CG.GenWebClientModule();
            SDCall = false;
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
                    DisplayPage(tpFormSetting);
                }
            }
            else if (tabControl.SelectedTab.Equals(tpFormSetting))
            {

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
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            FInfoDataSet.Dispose();
            FInfoDataSet = null;
            Hide();
        }

        private void DoGenClient()
        {
            FClientData.FormName = tbFormName.Text;
            TJQueryToJQMobileFormGenerator Generator = new TJQueryToJQMobileFormGenerator(FClientData, FDTE2, FAddIn);
            Generator.GenWebClientModule();
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            if (FClientData.BaseFormName == "")
            {
                MessageBox.Show("Web path cannot be empty!");
                return;
            }
            Hide();
            FDTE2.MainWindow.Activate();
            DoGenClient();
            FInfoDataSet.Dispose();
            FInfoDataSet = null;
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

        private void rbNewSolution_Click(object sender, EventArgs e)
        {
            EnabledOutputControls();
        }

        private void rbAddToExistSln_Click(object sender, EventArgs e)
        {
            EnabledOutputControls();
        }

        private void rbAddToCurrent_Click(object sender, EventArgs e)
        {
            EnabledOutputControls();
        }

        private void btnConnectionString_Click(object sender, EventArgs e)
        {
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

        private void cbDatabaseType_SelectedIndexChanged(object sender, EventArgs e)
        {
            FClientData.DatabaseType = (ClientType)cbDatabaseType.SelectedIndex;
        }

        private void tbFormName_TextChanged(object sender, EventArgs e)
        {
            tbFormTitle.Text = tbFormName.Text;
        }

        private void btnSelectJQPage_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = FClientData.WebSiteName;
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtSelectJQPage.Text = openFileDialog1.FileName;
                FClientData.BaseFormName = txtSelectJQPage.Text;
            }
        }
    }

    public class TJQueryToJQMobileFormData : Object
    {
        private string FPackageName, FBaseFormName, FServerPackageName, FFolderName, FTableName, FRealTableName, FFormName, FProviderName,
            FDatabaseName, FSolutionName, FViewProviderName, FWebSiteName, FFolderMode, FFormTitle;
        private TBlockItems FBlocks;
        private MWizard2015.fmJQueryToJQMobile FOwner;
        private bool FNewSolution = false;
        private string FCodeFolderName;
        private int FColumnCount;
        private ClientType FDatabaseType;
        private String FConnString;
        private String FLanguage = "cs";

        public TJQueryToJQMobileFormData(MWizard2015.fmJQueryToJQMobile Owner)
        {
            FOwner = Owner;
            FBlocks = new TBlockItems(this);
        }

        public ClientType DatabaseType
        {
            get { return FDatabaseType; }
            set { FDatabaseType = value; }
        }

        public fmJQueryToJQMobile Owner
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

        public object LoadFromXML(string XML)
        {
            System.Xml.XmlNode Node = null;
            System.Xml.XmlDocument Doc = new System.Xml.XmlDocument();
            Doc.LoadXml(XML);
            Node = Doc.SelectSingleNode("ClientData");
            SolutionName = Node.Attributes["SolutionName"].Value;
            NewSolution = Node.Attributes["NewSolution"].Value == "1";
            WebSiteName = Node.Attributes["PackageName"].Value;
            BaseFormName = Node.Attributes["BaseFormName"].Value;
            FolderName = Node.Attributes["FolderName"].Value;
            FolderMode = "ExistFolder";
            FormName = Node.Attributes["FormName"].Value;
            TableName = Node.Attributes["TableName"].Value;
            FormTitle = Node.Attributes["FormName"].Value;
            ProviderName = Node.Attributes["ProviderName"].Value;
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
            if (string.Compare(FBaseFormName, "JQMobileMasterDetail1") == 0 ||
                string.Compare(FBaseFormName, "VBJQMobileMasterDetail1") == 0 ||
                string.Compare(FBaseFormName, "JQMobileMasterDetail2") == 0 ||
                string.Compare(FBaseFormName, "VBJQMobileMasterDetail2") == 0)
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

    partial class TJQueryToJQMobileFormGenerator : System.ComponentModel.Component
    {
        private TJQueryToJQMobileFormData FClientData;
        private DTE2 FDTE2;
        private AddIn FAddIn;
#if VS90
        private WebDevPage.DesignerDocument FDesignerDocument;
#endif
        private ProjectItem FPI;
        private ProjectItem FPIFolder;
        private Project FProject = null;
        private DataSet FSYS_REFVAL;
        private Window FDesignWindow;
        private List<WebDataSource> FWebDataSourceList;
        private List<WebRefVal> FWebRefValList;
        private List<AjaxTools.AjaxRefVal> FAjaxRefValList;
        private List<WebRefVal> FWebRefValListPage;
        private List<WebDefault> FWebDefaultList;
        private List<WebValidate> FWebValidateList;
        private List<ExtComboBox> FExtComboBoxList;
        private List<MyWebDropDownList> FMyWebDropDownList;
        private List<WebDateTimePicker> FWebDateTimePickerList;
        private List<AjaxTools.AjaxDateTimePicker> FAjaxDateTimePickerList;
        private List<WebValidateBox> FWebValidateBoxList;
        private List<System.Web.UI.WebControls.CheckBox> FWebCheckBoxList;
        private List<System.Web.UI.WebControls.TextBox> FWebTextBoxList;
        private List<System.Web.UI.WebControls.Label> FLabelList;

        public TJQueryToJQMobileFormGenerator(TJQueryToJQMobileFormData ClientData, DTE2 dte2, AddIn aAddIn)
        {
            FClientData = ClientData;
            FDTE2 = dte2;
            FAddIn = aAddIn;
            FSYS_REFVAL = new DataSet();
            FWebDataSourceList = new List<WebDataSource>();
            FWebRefValList = new List<WebRefVal>();
            FAjaxRefValList = new List<AjaxTools.AjaxRefVal>();
            FWebRefValListPage = new List<WebRefVal>();
            FWebDefaultList = new List<WebDefault>();
            FWebValidateList = new List<WebValidate>();
            FExtComboBoxList = new List<ExtComboBox>();
            FMyWebDropDownList = new List<MyWebDropDownList>();
            FWebDateTimePickerList = new List<WebDateTimePicker>();
            FAjaxDateTimePickerList = new List<AjaxTools.AjaxDateTimePicker>();
            FWebValidateBoxList = new List<WebValidateBox>();
            FWebCheckBoxList = new List<System.Web.UI.WebControls.CheckBox>();
            FWebTextBoxList = new List<System.Web.UI.WebControls.TextBox>();
            FLabelList = new List<System.Web.UI.WebControls.Label>();
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

        private bool GetForm()
        {
            if (FPIFolder != null)
            {
                bool flag = false;
                foreach (ProjectItem aPI in FPIFolder.ProjectItems)
                {
                    if (string.Compare(FClientData.FormName + ".aspx", aPI.Name) == 0)
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
                    if (string.Compare(FClientData.BaseFormName + ".aspx", aPI.Name) == 0)
                    {
                        string Path = aPI.get_FileNames(0);

                        aPI.Name = Guid.NewGuid().ToString();
                        aPI.Delete();
                        File.Delete(Path);
                    }
                }

                FPI = null;
                FPI = FPIFolder.ProjectItems.AddFromFileCopy(FClientData.BaseFormName);
                FPI.Name = Guid.NewGuid().ToString() + ".aspx";
                FPIFolder = null;
            }
            else
            {
                foreach (ProjectItem aPI in FProject.ProjectItems)
                {
                    if (string.Compare(FClientData.FormName + ".aspx", aPI.Name) == 0)
                    {
                        string Path = aPI.get_FileNames(0);
                        Path = System.IO.Path.GetDirectoryName(Path);
                        aPI.Delete();
                        if (System.IO.File.Exists(Path + "\\" + FClientData.FormName + ".aspx.resx"))
                            System.IO.File.Delete(Path + "\\" + FClientData.FormName + ".aspx.resx");
                        if (System.IO.File.Exists(Path + "\\" + FClientData.FormName + ".aspx.vi-VN.resx"))
                            System.IO.File.Delete(Path + "\\" + FClientData.FormName + ".aspx.vi-VN.resx");
                        break;
                    }
                }

                FPI = FProject.ProjectItems.AddFromFileCopy(FClientData.BaseFormName);
                FPI.Name = FClientData.FormName + ".aspx";
            }

            return true;
        }

        private void GetDesignerHost()
        {
#if VS90
            //FDesignWindow = FPI.Open("{00000000-0000-0000-0000-000000000000}");
            //FDesignWindow.Activate();
            FDesignWindow = FPI.Open("{7651A702-06E5-11D1-8EBD-00A0C90F26EA}");
            FDesignWindow.Activate();
            HTMLWindow W = (HTMLWindow)FDesignWindow.Object;

            //W.CurrentTab = vsHTMLTabs.vsHTMLTabsSource;
            //if (W.CurrentTabObject is TextWindow)
            //    FTextWindow = W.CurrentTabObject as TextWindow;
            W.CurrentTab = vsHTMLTabs.vsHTMLTabsDesign;
            if (W.CurrentTabObject is WebDevPage.DesignerDocument)
            {
                FDesignerDocument = W.CurrentTabObject as WebDevPage.DesignerDocument;
            }
#else
            FDesignWindow = FPI.Open("{00000000-0000-0000-0000-000000000000}");
            FDesignWindow.Activate();
            FDesignWindow = FPI.Open("{7651A702-06E5-11D1-8EBD-00A0C90F26EA}");
            FDesignWindow.Activate();
            HTMLWindow W = (HTMLWindow)FDesignWindow.Object;
            object o = W.CurrentTabObject;
            IntPtr pObject;
            Microsoft.VisualStudio.OLE.Interop.IServiceProvider oleSP = (Microsoft.VisualStudio.OLE.Interop.IServiceProvider)o;
            Guid sid = typeof(IVSMDDesigner).GUID;
            Guid iid = typeof(IVSMDDesigner).GUID;
            int hr = oleSP.QueryService(ref sid, ref iid, out pObject);
            System.Runtime.InteropServices.Marshal.ThrowExceptionForHR(hr);
            if (pObject != IntPtr.Zero)
            {
                try
                {
                    Object TempObj = Marshal.GetObjectForIUnknown(pObject);
                    if (TempObj is IDesignerHost)
                    {
                        FDesignerHost = (IDesignerHost)TempObj;
                    }
                    else
                    {
                        Object ObjContainer = TempObj.GetType().InvokeMember("ComponentContainer",
                            System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public |
                            System.Reflection.BindingFlags.GetProperty, null, TempObj, null);
                        if (ObjContainer is IDesignerHost)
                        {
                            FDesignerHost = (IDesignerHost)ObjContainer;
                        }
                    }
                    FPage = (System.Web.UI.Page)FDesignerHost.RootComponent;
                    NotifyRefresh(200);
                    Application.DoEvents();
                    //FPage.Form.ID = FClientData.FormName;
                }
                finally
                {
                    Marshal.Release(pObject);
                }
            }
#endif
        }

        private void WriteWebDataSourceHTML()
        {
            String FileName = FDesignWindow.Document.FullName;
            FDesignWindow.Close(vsSaveChanges.vsSaveChangesYes);
            System.IO.StreamReader SR = new System.IO.StreamReader(FileName, Encoding.Default);
            String Context = SR.ReadToEnd();
            SR.Close();
            //Context = Context.Replace("<title>Untitled Page</title>", "<title>" + FClientData.FormTitle + "</title>");

            Context = Context.Replace("</JQTools:JQDialog>", String.Empty);
            String[] allStringsByEnter = Context.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in allStringsByEnter)
            {
                if (item.Trim().StartsWith("<JQTools:JQDialog"))
                {
                    Context = Context.Replace(item, String.Empty);
                }
            }
            Context = Context.Replace("JQTools", "JQMobileTools");
            for (int i = 0; i <= 200; i++)
            {
                Context = Context.Replace(String.Format("MaxLength=\"{0}\"", i), String.Empty);
                Context = Context.Replace(String.Format("maxlength=\"{0}\"", i), String.Empty);
            }

            System.IO.FileStream Filefs = new System.IO.FileStream(FileName, System.IO.FileMode.Create, System.IO.FileAccess.ReadWrite);
            System.IO.StreamWriter SW = new System.IO.StreamWriter(Filefs, Encoding.UTF8);
            SW.Write(Context);
            SW.Close();
            Filefs.Close();
        }

        private String GetHTMLProperty(String[] allHTMLs, String propertyName)
        {
            String propertyValue = String.Empty;
            foreach (var item in allHTMLs)
            {
                if (item.StartsWith(propertyName + "="))
                {
                    String[] values = item.Split(new char[] { '=', '\"' }, StringSplitOptions.RemoveEmptyEntries);
                    if(values.Length > 2)
                        propertyValue = values[1];
                    break;
                }
            }

            return propertyValue;
        }

        private void MyFunction()
        {
            String FileName = FDesignWindow.Document.FullName;
            System.IO.StreamReader SR = new System.IO.StreamReader(FileName, Encoding.Default);
            String Context = SR.ReadToEnd();
            SR.Close();

            String[] allStringsBySpace = Context.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            List<String> listIds = new List<string>();
            foreach (var item in allStringsBySpace)
            {
                if (item.StartsWith("ID="))
                {
                    String[] temp = item.Split(new char[] { '=', '\"' }, StringSplitOptions.RemoveEmptyEntries);
                    if (temp.Length > 1)
                        listIds.Add(temp[1]);
                }
            }
            foreach (var item in listIds)
            {
                object obj = FDesignerDocument.webControls.item(item, 0);
                WebDevPage.IHTMLElement eObj = (WebDevPage.IHTMLElement)obj;
                if (eObj != null)
                {
                    if (eObj.outerHTML.Trim().StartsWith("<JQTools:JQDataGrid"))
                    {
                        eObj.outerHTML = ReplaceJQDataGrid(eObj.outerHTML, Context);
                    }
                    else if (eObj.outerHTML.Trim().StartsWith("<JQTools:JQDataForm"))
                    {
                        eObj.outerHTML = ReplaceJQDataForm(eObj.outerHTML);
                    }
                }
            }
        }

        private String ReplaceJQDataGrid(String sourceHTML, String allHTML)
        {
            String returnValue = sourceHTML;
            while (returnValue.Contains("Frozen"))
                returnValue = RemoveProperty(returnValue, "Frozen");
            while (returnValue.Contains("ReadOnly"))
                returnValue = RemoveProperty(returnValue, "ReadOnly");
            while (returnValue.Contains("Sortable"))
                returnValue = RemoveProperty(returnValue, "Sortable");

            returnValue = returnValue.Replace("data-options=\"pagination:true,view:commandview\"", "RenderFooter=\"False\" RenderHeader=\"True\"");
            returnValue = returnValue.Replace("Editor=\"text\"", String.Empty);
            returnValue = returnValue.Replace("Editor=\"textarea\"", String.Empty);
            returnValue = returnValue.Replace("Editor=\"checkbox\"", String.Empty);
            returnValue = returnValue.Replace("Editor=\"numberbox\"", String.Empty);
            returnValue = returnValue.Replace("Editor=\"validatebox\"", String.Empty);
            returnValue = returnValue.Replace("Editor=\"datebox\"", String.Empty);
            returnValue = returnValue.Replace("Editor=\"timespinner\"", String.Empty);
            returnValue = returnValue.Replace("Editor=\"infocombobox\"", String.Empty);
            returnValue = returnValue.Replace("Editor=\"infocombogrid\"", String.Empty);
            returnValue = returnValue.Replace("Editor=\"inforefval\"", String.Empty);
            returnValue = returnValue.Replace("Editor=\"password\"", String.Empty);
            returnValue = returnValue.Replace("Editor=\"infofileupload\"", String.Empty);
            returnValue = returnValue.Replace("Editor=\"infoautocomplete\"", String.Empty);
            returnValue = returnValue.Replace("Editor=\"infooptions\"", String.Empty);
            returnValue = returnValue.Replace("Editor=\"qrcode\"", String.Empty);

            String[] allStringsBySpace = returnValue.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in allStringsBySpace)
            {
                if (item.Trim().StartsWith("EditorOptions="))
                {
                    String[] sEditorOptions = item.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    String sRemoteName = String.Empty;
                    String sDisplayMember = String.Empty;
                    String sValueMember = String.Empty;
                    foreach (var item1 in sEditorOptions)
                    {
                        if (item1.StartsWith("remoteName:"))
                        {
                            String[] temp = item1.Split(new char[] { ':', '\'' }, StringSplitOptions.RemoveEmptyEntries);
                            if (temp.Length > 1)
                                sRemoteName = temp[1];
                        }
                        else if (item1.StartsWith("valueField:"))
                        {
                            String[] temp = item1.Split(new char[] { ':', '\'' }, StringSplitOptions.RemoveEmptyEntries);
                            if (temp.Length > 1)
                                sValueMember = temp[1];
                        }
                        else if (item1.StartsWith("textField:"))
                        {
                            String[] temp = item1.Split(new char[] { ':', '\'' }, StringSplitOptions.RemoveEmptyEntries);
                            if (temp.Length > 1)
                                sDisplayMember = temp[1];
                        }
                    }
                    returnValue = returnValue.Replace(item, String.Format("RelationOptions=\"{{RemoteName:'{0}',DisplayMember:'{1}',ValueMember:'{2}'}}\"", sRemoteName, sDisplayMember, sValueMember));
                }
            }

            String sEditDialogID = GetHTMLProperty(allStringsBySpace, "EditDialogID");
            if (!String.IsNullOrEmpty(sEditDialogID))
            {
                String sEditFormID = String.Empty;
                object oEditDialog = FDesignerDocument.webControls.item(sEditDialogID, 0);
                WebDevPage.IHTMLElement eEditDialog = (WebDevPage.IHTMLElement)oEditDialog;
                List<String> listIds = new List<string>();
                if (eEditDialog != null)
                {
                    foreach (var item in eEditDialog.innerHTML.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        if (item.StartsWith("ID="))
                        {
                            String[] temp = item.Split(new char[] { '=', '\"' }, StringSplitOptions.RemoveEmptyEntries);
                            if (temp.Length > 1)
                                listIds.Add(temp[1]);
                        }
                    }
                }
                foreach (var item in listIds)
                {
                    object obj = FDesignerDocument.webControls.item(item, 0);
                    WebDevPage.IHTMLElement eObj = (WebDevPage.IHTMLElement)obj;
                    if (eObj != null && eObj.outerHTML.Trim().StartsWith("<JQTools:JQDataForm"))
                    {
                        sEditFormID = GetHTMLProperty(eObj.outerHTML.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries), "ID");
                    }
                }
                returnValue = returnValue.Replace(String.Format("EditDialogID=\"{0}\"", sEditDialogID), String.Format("EditFormID=\"{0}\"", sEditFormID));
            }

            returnValue = returnValue.Replace("TooItems", "ToolItems");
            returnValue = returnValue.Replace("ItemType=\"easyui-linkbutton\"", String.Empty);
            returnValue = returnValue.Replace("Icon=\"icon-add\"", "Icon=\"plus\" Name=\"grid-insert\"");
            returnValue = returnValue.Replace("Icon=\"icon-search\"", "Icon=\"search\" Name=\"grid-query\"");
            returnValue = RemoveLine(returnValue, "Icon=\"icon-save\"");
            returnValue = RemoveLine(returnValue, "Icon=\"icon-undo\"");
            returnValue = returnValue.Replace("OnClick=\"insertItem\"", String.Empty);
            returnValue = returnValue.Replace("OnClick=\"openQuery\"", String.Empty);

            if (returnValue.IndexOf("<RelationColumns>") != -1)
                returnValue = returnValue.Remove(returnValue.IndexOf("<RelationColumns>"), returnValue.IndexOf("</RelationColumns>") + 18 - returnValue.IndexOf("<RelationColumns>"));
            if (returnValue.IndexOf("QueryColumns") != -1)
            {
                while (returnValue.IndexOf("RelationOptions", returnValue.IndexOf("QueryColumns")) != -1)
                    returnValue = RemoveProperty(returnValue, "RelationOptions", returnValue.IndexOf("QueryColumns"));
                while (returnValue.IndexOf("Width", returnValue.IndexOf("QueryColumns")) != -1)
                    returnValue = RemoveProperty(returnValue, "Width", returnValue.IndexOf("QueryColumns"));
                while (returnValue.IndexOf("NewLine", returnValue.IndexOf("QueryColumns")) != -1)
                    returnValue = RemoveProperty(returnValue, "NewLine", returnValue.IndexOf("QueryColumns"));
                while (returnValue.IndexOf("RemoteMethod", returnValue.IndexOf("QueryColumns")) != -1)
                    returnValue = RemoveProperty(returnValue, "RemoteMethod", returnValue.IndexOf("QueryColumns"));
            }
            return returnValue;
        }

        private String ReplaceJQDataForm(String sourceHTML)
        {
            String returnValue = sourceHTML;
            while (returnValue.Contains("OnBlur"))
                returnValue = RemoveProperty(returnValue, "OnBlur");
            while (returnValue.Contains("ReadOnly"))
                returnValue = RemoveProperty(returnValue, "ReadOnly");
            while (returnValue.Contains("Sortable"))
                returnValue = RemoveProperty(returnValue, "Sortable");
            while (returnValue.Contains("Visible"))
                returnValue = RemoveProperty(returnValue, "Visible");

            returnValue = returnValue.Replace("Editor=\"checkbox\"", "Editor=\"checkboxes\"");
            returnValue = returnValue.Replace("Editor=\"numberbox\"", "Editor=\"text\"");
            returnValue = returnValue.Replace("Editor=\"validatebox\"", "Editor=\"text\"");
            returnValue = returnValue.Replace("Editor=\"datebox\"", "Editor=\"date\"");
            returnValue = returnValue.Replace("Editor=\"timespinner\"", "Editor=\"text\"");
            returnValue = returnValue.Replace("Editor=\"infocombobox\"", "Editor=\"selects\"");
            returnValue = returnValue.Replace("Editor=\"infocombogrid\"", "Editor=\"selects\"");
            returnValue = returnValue.Replace("Editor=\"inforefval\"", "Editor=\"refval\"");
            returnValue = returnValue.Replace("Editor=\"infofileupload\"", "Editor=\"file\"");
            returnValue = returnValue.Replace("Editor=\"infoautocomplete\"", "Editor=\"text\"");
            returnValue = returnValue.Replace("Editor=\"infooptions\"", "Editor=\"text\"");

            String[] allStringsBySpace = returnValue.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in allStringsBySpace)
            {
                if (item.Trim().StartsWith("EditorOptions="))
                {
                    String newEditorOptions = item;
                    newEditorOptions = newEditorOptions.Replace("remoteName", "RemoteName");
                    newEditorOptions = newEditorOptions.Replace("valueField", "ValueMember");
                    newEditorOptions = newEditorOptions.Replace("textField", "DisplayMember");
                    newEditorOptions = newEditorOptions.Replace("panelWidth", "DialogWidth");
                    newEditorOptions = newEditorOptions.Replace("title", "DialogTitle");
                    newEditorOptions = newEditorOptions.Replace("columnMatches", "ColumnMatches");
                    newEditorOptions = newEditorOptions.Replace("columns", "Columns");

                    returnValue = returnValue.Replace(item, newEditorOptions);
                }
            }
            return returnValue;
        }

        private String RemoveLine(String source, String key)
        {
            int startIndex, endIndex = -1;
            startIndex = source.IndexOf(key);
            if (startIndex != -1)
            {
                endIndex = startIndex;
                do
                {

                } while (source[startIndex--] != '<');
                do
                {

                } while (source[endIndex++] != '>');
                source = source.Remove(startIndex, endIndex - startIndex);
            }
            return source;
        }

        private String RemoveProperty(String source, String key, int index = 0)
        {
            int startIndex, endIndex = -1;
            startIndex = source.IndexOf(key, index);
            if (startIndex != -1)
            {
                endIndex = startIndex + key.Length + 2;
                do
                {

                } while (source[endIndex++] != '"');
                source = source.Remove(startIndex, endIndex - startIndex);
            }
            return source;
        }

        public void GenWebClientModule()
        {
            GenFolder();
            if (GetForm())
            {
                GetDesignerHost();
#if VS90
#else
                DesignerTransaction transaction1 = FDesignerHost.CreateTransaction();
#endif
                try
                {
                    MyFunction();

                    WriteWebDataSourceHTML();
                }
                catch (Exception exception2)
                {
                    MessageBox.Show(exception2.Message);
                    return;
                }
                finally
                {
                    FPI.Name = FClientData.FormName + ".aspx";
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
