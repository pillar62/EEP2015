using EFClientTools;
using EFClientTools.Beans;
using EFClientTools.EFServerReference;
using EFClientTools.Web;
using EnvDTE;
using EnvDTE80;
using Microsoft.Win32;
using Srvtools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.OracleClient;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace EEPSDModule2015
{
    public partial class fmMain : Form
    {
        private DTE2 FDTE2;
        private AddIn FAddIn;
        private InfoDataSet FInfoDataSet = null;
        private TStringList FAlias;
        private TServerData FServerData;
        private ListViewColumnSorter lvwColumnSorter;
        private ListViewColumnSorter lvwServiceSorter;
        private String FWebSite = "";
        private String FUserID = "";
        private String FPassword = "";
        private SYS_LANGUAGE language = SYS_LANGUAGE.ENG;
        private String FSolution = "";
        private String FOutputFolder = "";
        //client pic,server pic,purpose,suggestion,field,grid,default,infocommand,updatecomponent,client code,server code
        private bool[] documentsetting = new bool[11];
        private DbConnection InternalConnection = null;

        public fmMain()
        {
            InitializeComponent();
            FServerData = new TServerData(this);
            lvwColumnSorter = new ListViewColumnSorter();
            this.lvTables.ListViewItemSorter = lvwColumnSorter;
            this.lvService.ListViewItemSorter = lvwServiceSorter;
        }
        public fmMain(DTE2 aDTE2, AddIn aAddIn)
        {
            InitializeComponent();
            FDTE2 = aDTE2;
            FAddIn = aAddIn;
            FServerData = new TServerData(this);
            lvwColumnSorter = new ListViewColumnSorter();
            this.lvTables.ListViewItemSorter = lvwColumnSorter;
            this.lvService.ListViewItemSorter = lvwServiceSorter;
            tbRoot.Text = FSolution = FDTE2.Solution.FileName;
            tbUserID.Text = "001";
            cbLanguage.SelectedIndex = 2;
            tbFolder.Text = "";
            GetConfig();
            //PrepareWizardService();
        }

        private void GetConfig()
        {
            XmlDocument xml = new XmlDocument();
            string path = WzdUtils.GetAddinsPath();
            string file = path + "\\EEPSDModuleSetting.xml";

            try
            {
                if (File.Exists(file))
                {
                    xml.Load(file);
                    XmlNode sNode = xml.DocumentElement.FirstChild;
                    while (sNode != null)
                    {
                        while (sNode != null)
                        {
                            if (String.Compare(sNode.Name, "SelectType", true) == 0)
                            {
                                #region SelectType
                                string type = sNode.Attributes["type"].InnerText;
                                if (type == "1")
                                {
                                    rbService.Checked = true;
                                }
                                else if (type == "2")
                                {
                                    rbPage.Checked = true;
                                }
                                else if (type == "3")
                                {
                                    rbExportImport.Checked = true;
                                }
                                else if (type == "4")
                                {
                                    rbDDExportImport.Checked = true;
                                }
                                else
                                {
                                    rbTable.Checked = true;
                                }
                                #endregion
                            }
                            else if (string.Compare(sNode.Name, "PringOptions", true) == 0)
                            {
                                #region PringOptions
                                cbDB.Text = sNode.Attributes["Alias"].InnerText;
                                tbUserID.Text = sNode.Attributes["UserID"].InnerText;
                                tbPassword.Text = sNode.Attributes["Password"].InnerText;
                                tbRoot.Text = sNode.Attributes["Solution"].InnerText;
                                cbLanguage.Text = sNode.Attributes["language"].InnerText;
                                string webtype = sNode.Attributes["webType"].InnerText;
                                if (webtype == "Q")
                                {
                                    rbJquery.Checked = true;
                                }
                                else
                                {
                                    rbnet.Checked = true;
                                }
                                if (sNode.Attributes["waitTime"] != null)
                                {
                                    tbPWT.Text = sNode.Attributes["waitTime"].InnerText;
                                }
                                if (sNode.Attributes["wordFolder"] != null)
                                {
                                    tbFolder.Text = sNode.Attributes["wordFolder"].InnerText;
                                }
                                #endregion
                            }
                            else if (string.Compare(sNode.Name, "PrintSettings", true) == 0)
                            {
                                #region PrintSettings
                                string sdocumentsetting = sNode.Attributes["Value"].InnerText;
                                string[] slist = sdocumentsetting.Split(',');
                                if (slist.Length == 11)
                                {
                                    try
                                    {
                                        checkBox1.Checked = bool.Parse(slist[0]);
                                        checkBox2.Checked = bool.Parse(slist[1]);
                                        checkBox3.Checked = bool.Parse(slist[2]);
                                        checkBox4.Checked = bool.Parse(slist[3]);
                                        checkBox5.Checked = bool.Parse(slist[4]);
                                        checkBox6.Checked = bool.Parse(slist[5]);
                                        checkBox7.Checked = bool.Parse(slist[6]);
                                        checkBox8.Checked = bool.Parse(slist[7]);
                                        checkBox9.Checked = bool.Parse(slist[8]);
                                        checkBox10.Checked = bool.Parse(slist[9]);
                                        checkBox11.Checked = bool.Parse(slist[10]);

                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show(ex.Message, "GetConfig");
                                    }
                                }
                                #endregion
                            }
                            sNode = sNode.NextSibling;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void SetConfig()
        {
            XmlDocument xml = new XmlDocument();
            string path = EEPRegistry.Server.Substring(0, EEPRegistry.Server.LastIndexOf("\\"));
            string file = path +"\\Addins\\EEPSDModuleSetting.xml";
            if (File.Exists(file ))
            {
                xml.Load(file);
            }
            XmlNode nodeNormalOptions = xml.SelectSingleNode("Options");
            if (nodeNormalOptions == null)
            {
                nodeNormalOptions = xml.CreateElement("Options");
                xml.AppendChild(nodeNormalOptions);
            }
            nodeNormalOptions.RemoveAll();

            #region select type
            XmlNode nodeSelectType = xml.CreateElement("SelectType");
            XmlAttribute att = xml.CreateAttribute("type");
            string type = "0";
            if (rbService.Checked) type = "1";
            else if (rbPage.Checked) type = "2";
            else if (rbExportImport.Checked) type = "3";
            else if (rbDDExportImport.Checked) type = "4";
            att.Value = type;
            nodeSelectType.Attributes.Append(att);
            nodeNormalOptions.AppendChild(nodeSelectType);
            #endregion

            #region print options
            XmlNode nodePringOptions = xml.CreateElement("PringOptions");
            XmlAttribute att2 = xml.CreateAttribute("Alias");
            att2.Value = cbDB.Text;
            nodePringOptions.Attributes.Append(att2);

            XmlAttribute att3 = xml.CreateAttribute("UserID");
            att3.Value = tbUserID.Text;
            nodePringOptions.Attributes.Append(att3);

            XmlAttribute att4 = xml.CreateAttribute("Password");
            att4.Value = tbPassword.Text;
            nodePringOptions.Attributes.Append(att4);

            XmlAttribute att5 = xml.CreateAttribute("Solution");
            att5.Value = tbRoot.Text;
            nodePringOptions.Attributes.Append(att5);

            XmlAttribute att6 = xml.CreateAttribute("language");
            att6.Value = cbLanguage.Text;
            nodePringOptions.Attributes.Append(att6);

            XmlAttribute att7 = xml.CreateAttribute("webType");
            string webtype = "A";
            if (rbnet.Checked) webtype = "A";
            else if (rbJquery.Checked) webtype = "Q";
            att7.Value = webtype;
            nodePringOptions.Attributes.Append(att7);

            XmlAttribute att8 = xml.CreateAttribute("waitTime");
            att8.Value = tbPWT.Text;
            nodePringOptions.Attributes.Append(att8);

            XmlAttribute att9 = xml.CreateAttribute("wordFolder");
            att9.Value = tbFolder.Text;
            nodePringOptions.Attributes.Append(att9);

            nodeNormalOptions.AppendChild(nodePringOptions);
            #endregion

            XmlNode nodePrintOptions = xml.SelectSingleNode("PrintSettings");
            if (nodePrintOptions == null)
            {
                nodePrintOptions = xml.CreateElement("PrintSettings");

                XmlAttribute att10 = xml.CreateAttribute("Value");

                documentsetting = new bool[11] { checkBox1.Checked, checkBox2.Checked, checkBox3.Checked, checkBox4.Checked, checkBox5.Checked, checkBox6.Checked, checkBox7.Checked, checkBox8.Checked, checkBox9.Checked, checkBox10.Checked, checkBox11.Checked, };
                List<string> documentsettingstring = new List<string>();
                foreach (bool s in documentsetting)
                {
                    documentsettingstring.Add(s.ToString().ToLower());
                }
                string sdocumentsetting = String.Join(",", documentsettingstring);
                att10.Value = sdocumentsetting;
                nodePrintOptions.Attributes.Append(att10);

                nodeNormalOptions.AppendChild(nodePrintOptions);
            }

            //nodeSystemDB.InnerText = cbxSysDB.Text.Trim();
            xml.Save(file);
        }

        public TServerData ServerData
        {
            get
            {
                return FServerData;
            }
        }

        public DbConnection GlobalConnection
        {
            get { return InternalConnection; }
        }

        public String SelectedAlias
        {
            get { return cbDB.Text; }
        }
        private void PrepareWizardService()
        {
            Show();
            Hide();
        }

        public void ShowWebClientWizard()
        {
            //Show();
            Init();
            ShowDialog();
        }
        private void Init()
        {
            ClearValues();
            LoadDBString();

            FInfoDataSet = new InfoDataSet();
            FInfoDataSet.SetWizardDesignMode(true);
            try
            {
                cbDB.Text = EEPRegistry.WizardConnectionString;
                GetWebSite();
                //GetTableNames(lvTables);
                GetAllServer();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            DisplayPage(tpMain,true);
        }

        private void ClearValues()
        {
            lvTables.Items.Clear();
            lvService.Items.Clear();
            FServerData.Datasets.Clear();
        }
        private void LoadDBString()
        {
            try
            {
                cbDB.Items.Clear();
                FAlias = new TStringList();
                List<string> list1 = new List<string>();
                string text3 = SystemFile.DBFile;
                XmlDocument document1 = new XmlDocument();
                document1.Load(text3);
                foreach (XmlNode node1 in document1.FirstChild.FirstChild.ChildNodes)
                {
                    list1.Add((string)node1.Name);
                    cbDB.Items.Add(node1.Name);
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
            catch (Exception ex)
            {
                MessageBox.Show("Please setup <DB Manager> of EEPNetServer at first !");
            }
        }

        private void DisplayPage(TabPage aPage)
        {
            tabControl1.TabPages.Clear();
            tabControl1.TabPages.Add(aPage);
            tabControl1.SelectedTab = aPage;
            EnableButton();
        }
        private void DisplayPage(TabPage aPage,bool first)
        {
            tabControl1.TabPages.Clear();
            tabControl1.TabPages.Add(aPage);
            if (first == true)
                tabControl1.TabPages.Add(tpPrintOptions);
            tabControl1.SelectedTab = aPage;
            EnableButton();
        }

        private void EnableButton()
        {
            btPrevious.Enabled = tabControl1.SelectedTab != tpMain && tabControl1.SelectedTab != tpPrintOptions;
            btNext.Enabled = (tabControl1.SelectedTab != tpTable && tabControl1.SelectedTab != tpPage && tabControl1.SelectedTab != tpService && tabControl1.SelectedTab != tpImportExport && tabControl1.SelectedTab != tpDDImportExport);
            btDone.Enabled = (tabControl1.SelectedTab == tpTable || tabControl1.SelectedTab == tpPage || tabControl1.SelectedTab == tpService || tabControl1.SelectedTab == tpImportExport || tabControl1.SelectedTab == tpDDImportExport);
            btCancel.Enabled = true;
        }

        private void btNext_Click(object sender, EventArgs e)
        {
            SetConfig();
            if (tabControl1.SelectedTab.Equals(tpMain) || tabControl1.SelectedTab.Equals(tpPrintOptions))
            {
                WzdUtils.SetRegistryValueByKey("WizardConnectionString", cbDB.Text);
                WzdUtils.SetRegistryValueByKey("DatabaseType", cbDatabaseType.Text);
                FServerData.ConnectionString = tbConnectionString.Text;
                FServerData.DatabaseType = (ClientType)cbDatabaseType.SelectedIndex;
                FServerData.EEPAlias = cbDB.Text;
                FUserID = tbUserID.Text;
                FPassword = tbPassword.Text;
                FSolution = tbRoot.Text;
                FOutputFolder = tbFolder.Text;
                switch (cbLanguage.SelectedIndex)
                {
                    case 0: language = SYS_LANGUAGE.ENG; break;
                    case 1: language = SYS_LANGUAGE.SIM; break;
                    case 2: language = SYS_LANGUAGE.TRA; break;
                    default: language = SYS_LANGUAGE.ENG; break;
                }

                if (string.IsNullOrEmpty(FOutputFolder) || !Directory.Exists(FOutputFolder))
                {
                    string wmsg = PublicTest.GetSystemMessage(language, "SDModule", "Document", "outputFolder");
                    MessageBox.Show(wmsg);
                    return;
                }
                if (string.IsNullOrEmpty(cbDB.Text))
                {
                    string wmsg = PublicTest.GetSystemMessage(language, "SDModule", "Document", "eepAlias");
                    MessageBox.Show(wmsg);
                    return;
                }


                if (rbTable.Checked)
                {
                    DisplayPage(tpTable);
                }
                else if (rbService.Checked)
                {
                    DisplayPage(tpService);
                }
                else if (rbPage.Checked)
                {
                    DisplayPage(tpPage);
                }
                else if (rbExportImport.Checked)
                {
                    DisplayPage(tpImportExport);
                }
                else if (rbDDExportImport.Checked)
                {
                    DisplayPage(tpDDImportExport);
                }
            }
            //else if (tabControl1.SelectedTab.Equals(tpPage))
            //{
            //    DisplayPage(tpPrintOptions);
            //}
        }

        private void btPrevious_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab.Equals(tpPrintOptions))
            {
                DisplayPage(tpPage);
            }
            else
                DisplayPage(tpMain,true);
        }
        private void btCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btDone_Click(object sender, EventArgs e)
        {
            this.Hide();
            #region table
            if (tabControl1.SelectedTab.Equals(tpTable))
            {
                if (lvTables.CheckedItems.Count > 0)
                {
                    foreach (ListViewItem lvi in lvTables.CheckedItems)
                    {
                        string tableName = lvi.Text;
                        tableName = WzdUtils.Quote(tableName, GlobalConnection);
                        TFieldAttrItems tdsItems = new TFieldAttrItems(lvi);
                        lvi.Tag = tdsItems;

                        String datatypestring = "";
                        switch (FServerData.DatabaseType)
                        {
                            case ClientType.ctMsSql:
                                datatypestring = "Select a.name as 'columnname', b.name as 'typename', a.length as 'columnlength',a.prec as 'prec',a.scale as 'columnscale',a.isnullable as 'dbnull' FROM syscolumns a left join systypes b on a.xtype=b.xusertype where a.id=object_id('" + tableName + "')";
                                break;
                            case ClientType.ctOracle:
                                datatypestring = "select column_name,data_type ,data_length,data_precision,data_scale ,nullable from user_tab_columns where table_name  = '" + tableName+ "'";
                                break;
                            case ClientType.ctODBC:
                            case ClientType.ctOleDB:
                            case ClientType.ctInformix:
                            case ClientType.ctMySql:
                                datatypestring = "select colname, coltype, collength FROM systables a, syscolumns b WHERE a.tabid = b.tabid AND tabname = '" +tableName + "';";
                                break;
                            case ClientType.ctSybase:
                                break;
                        }
                        IDbCommand cmd = GlobalConnection.CreateCommand();
                        cmd.CommandText = datatypestring;
                        if (GlobalConnection.State == ConnectionState.Closed)
                        { GlobalConnection.Open(); }
                        DataSet schemaTable = new DataSet();
                        IDbDataAdapter ida = WzdUtils.AllocateDataAdapter(FServerData.DatabaseType);
                        ida.SelectCommand = cmd;
                        ida.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                        ida.Fill(schemaTable);
                        switch (FServerData.DatabaseType)
                        {
                            case ClientType.ctMsSql:
                                foreach (DataRow dr in schemaTable.Tables[0].Rows)
                                {
                                    TFieldSchemaItem Item = new TFieldSchemaItem();
                                    Item.DataField = dr["columnname"].ToString();
                                    Item.DataType = dr["typename"].ToString();
                                    string typename = ((string)dr["typename"]).ToLower();
                                    int length = Int16.Parse(dr["columnlength"].ToString());
                                    if (typename == "nvarchar" || typename == "nchar" || typename == "decimal" || typename == "numeric") length = Int16.Parse(dr["prec"].ToString());
                                    Item.Length = length;
                                    if (typename == "decimal" || typename == "numeric")
                                    { Item.Scale = Int16.Parse(dr["columnscale"].ToString()); }
                                    Item.Caption = dr["columnname"].ToString();
                                    if (dr["dbnull"].ToString() == "0")
                                    { Item.AllowDBNull = false; }
                                    else Item.AllowDBNull = true;

                                    tdsItems.Add(Item);
                                }
                                break;
                            case ClientType.ctOracle:
                                foreach (DataRow dr in schemaTable.Tables[0].Rows)
                                {
                                    TFieldSchemaItem Item = new TFieldSchemaItem();
                                    Item.DataField = dr["column_name"].ToString();

                                    string typename = ((string)dr["data_type"]).ToLower();
                                    Item.DataType = typename;
                                    if (typename.StartsWith("interval year", StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        Item.Length = Int32.Parse(dr["data_length"].ToString());
                                    }
                                    else if (typename.StartsWith("interval day", StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        Item.Length = Int32.Parse(dr["data_length"].ToString());
                                        Item.Scale = Int32.Parse(dr["data_scale"].ToString());
                                    }
                                    if (typename == "nvarchar2" || typename == "nchar" || typename == "nclob")
                                    {
                                        Item.Length = Int32.Parse(dr["data_length"].ToString()) / 2;
                                    }
                                    else if (typename == "float")
                                    {
                                        Item.Length = Int32.Parse(dr["data_precision"].ToString());
                                    }
                                    else if (typename == "number")
                                    {
                                        if (dr["data_precision"] != DBNull.Value)
                                        {
                                            Item.Length = Int32.Parse(dr["data_precision"].ToString());
                                            Item.Scale = (dr["data_scale"] == DBNull.Value) ? 0 : Int32.Parse(dr["data_scale"].ToString());
                                        }
                                    }
                                    else if (typename == "varchar2" || typename == "char" || typename == "raw")
                                    {
                                        Item.Length = Int32.Parse(dr["data_length"].ToString());
                                    }

                                    if (dr["nullable"].ToString() == "N")
                                    { Item.AllowDBNull = false; }
                                    else Item.AllowDBNull = true;

                                    tdsItems.Add(Item);
                                }
                                break;
                            case ClientType.ctODBC:
                            case ClientType.ctOleDB:
                            case ClientType.ctInformix:
                            case ClientType.ctMySql:
                                foreach (DataRow dr in schemaTable.Tables[0].Rows)
                                {
                                    TFieldSchemaItem Item = new TFieldSchemaItem();
                                    string columnname = dr["colname"].ToString();
                                    Item.DataField = dr["colname"].ToString();
                                    int typeid = Int32.Parse(dr["coltype"].ToString());
                                    int collength = Int32.Parse(dr["collength"].ToString());
                                    bool nonull = typeid / 256 == 1 ? true : false;
                                    typeid = typeid % 256;
                                    string typestring = GetDataType(typeid);
                                    Item.DataType = typestring;
                                    if (typeid == 41 || typeid == 40 || typeid == 44)
                                    {
                                        IDbCommand cmdinfomix = GlobalConnection.CreateCommand();
                                        cmdinfomix.CommandText =  "select b.colname,b.coltype,c.name FROM systables a,syscolumns b,sysxtdtypes c WHERE a.tabid = b.tabid AND a.tabname = '" + tableName + "' AND b.colname = '" + columnname + "' AND b.extended_id =c.extended_id;";
                                        if (GlobalConnection.State == ConnectionState.Closed)
                                        { GlobalConnection.Open(); }
                                        DataSet cmdinfomixTable = new DataSet();
                                        IDbDataAdapter idacmdinfomix = WzdUtils.AllocateDataAdapter(FServerData.DatabaseType);
                                        idacmdinfomix.SelectCommand = cmdinfomix;
                                        idacmdinfomix.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                                        idacmdinfomix.Fill(cmdinfomixTable);
                                        typestring = cmdinfomixTable.Tables[0].Rows[0]["name"].ToString();
                                    }
                                    int length = 0; int scale = 0;
                                    switch (typeid)
                                    {
                                        case (0):
                                        case (15): Item.Length = collength; break;
                                        case (5):
                                        case (8): GetTypeLength(collength, 0, ref length, ref scale); Item.Length = length; Item.Scale = scale; break;
                                        case (13):
                                        case (16): GetTypeLength(collength, 1, ref length, ref scale); Item.Length = length; Item.Scale = scale; break;
                                        case (10):
                                        case (14): GetDateTypeLength(collength, ref length); Item.Length = length; Item.Scale = collength; break;
                                    }
                                    if (nonull)
                                    { Item.AllowDBNull = false; }
                                    else Item.AllowDBNull = true;

                                    tdsItems.Add(Item);
                                }
                                break;
                            case ClientType.ctSybase:
                                break;
                        }
                        
                        //key
                        String sQLkey = "select * from " + tableName + " where 1=0";
                        IDbCommand cmdkey = GlobalConnection.CreateCommand();
                        cmdkey.CommandText = sQLkey;
                        if (GlobalConnection.State == ConnectionState.Closed)
                        { GlobalConnection.Open(); }
                        DataSet schemaTable2 = new DataSet();
                        IDbDataAdapter ida2 = WzdUtils.AllocateDataAdapter(FServerData.DatabaseType);
                        ida2.SelectCommand = cmdkey;
                        ida2.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                        ida2.Fill(schemaTable2); 
                        foreach (DataColumn DR in schemaTable2.Tables[0].PrimaryKey)
                        {
                            for (int I = 0; I < tdsItems.Count; I++)
                            {
                                if ((tdsItems[I] as TFieldSchemaItem).DataField == DR.ColumnName)
                                {
                                    (tdsItems[I] as TFieldSchemaItem).IsKey = true;
                                }
                            }
                        }
                    }
                    MemoryStream ms = GetTableSchemaStream();
                    if (ms.Length > 0)
                    {
                        byte[] b = ms.ToArray();
                        //object[] o = new object[] { "lu", "T", b };
                        //object[] oret = Srvtools.CliUtils.CallMethod("SDModuleUserServer", "RemotePrint",o );

                        object[] o = new object[] { b,FOutputFolder };
                        object oret = PublicTest.PrintTableSchema(o);

                        if ((oret as object[])[0].ToString() == "0")
                        {
                            //string filepath = EEPRegistry.Server + "\\SDModuleFile\\" + (oret as object[])[2].ToString() + ".doc";//object 1 is byte
                            string msg = PublicTest.GetSystemMessage(language, "SDModule", "Document", "outputSuccess");
                            MessageBox.Show(msg);

                            //PublicTest.openWord(filepath);
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show((oret as object[])[1].ToString());
                        }
                    }
                }
                else
                {
                    MessageBox.Show("no table ");
                }
            }
            #endregion

            #region Page
            else if (tabControl1.SelectedTab.Equals(tpPage) && rbPage.Checked && treeView1.Nodes.Count > 0)
            {
                documentsetting = new bool[11] { checkBox1.Checked, checkBox2.Checked, checkBox3.Checked, false, checkBox5.Checked, checkBox6.Checked, checkBox7.Checked, checkBox8.Checked, checkBox9.Checked, checkBox10.Checked, checkBox11.Checked, };
                
                //Hashtable hs = new Hashtable();
                Dictionary<string, object> d = new Dictionary<string, object>();
                string type = rbnet.Checked ? "A" : "";
                if (type == "") type = rbJquery.Checked ? "Q" : "";
                foreach (TreeNode node in treeView1.Nodes)
                {
                    if (node.Checked)
                    {
                        if (node.Tag != null)
                        {
                            string[] astring = node.Tag as string[];
                            if (astring.Length > 2)
                                d.Add(astring[2], astring);
                        }
                    }
                    if (node.Nodes.Count > 0)
                    {
                        getChecked(d, node);
                    }
                }

                int pwt = 5;
                Int32.TryParse(tbPWT.Text, out pwt);

                object[] options = new object[] { type, FSolution, FWebSite, FServerData.EEPAlias, FUserID, FPassword, language, documentsetting, pwt, FOutputFolder };
                object o = PublicTest.UIPrint(FDTE2, d,options);
                if ((o as object[])[0].ToString() == "0")
                {
                    //string filepath = EEPRegistry.Server + "\\SDModuleFile\\" + (oret as object[])[2].ToString() + ".doc";//object 1 is byte
                    string msg = PublicTest.GetSystemMessage(language, "SDModule", "Document", "outputSuccess");
                    MessageBox.Show(msg);

                    //PublicTest.openWord(filepath);
                    this.Close();
                }
                else
                {
                    MessageBox.Show((o as object[])[1].ToString());
                }
            }
            #endregion
            
            #region service
            else if (tabControl1.SelectedTab.Equals(tpService))
            {
                if (lvService.CheckedItems.Count > 0)
                {
                    Dictionary<string, object> hs = new Dictionary<string, object>();

                    foreach (ListViewItem lvi in lvService.CheckedItems)
                    {
                        string s = lvi.Text.ToString();
                        hs.Add(s + ".dll", s);
                    }
                    bool[] documentsettings = new bool[] { false,true,false,false,false,false,false,true,true,false,true};
                    object[] options = new object[] { "S", FSolution, "", FServerData.EEPAlias, FUserID, FPassword, language, documentsettings ,5,FOutputFolder};
                    object o = PublicTest.UIPrint(FDTE2, hs, options);
                    if ((o as object[])[0].ToString() == "0")
                    {
                        //string filepath = EEPRegistry.Server + "\\SDModuleFile\\" + (oret as object[])[2].ToString() + ".doc";//object 1 is byte
                        string msg = PublicTest.GetSystemMessage(language, "SDModule", "Document", "outputSuccess");
                        MessageBox.Show(msg);

                        //PublicTest.openWord(filepath);
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show((o as object[])[1].ToString());
                    }
                }
                else
                {
                    MessageBox.Show("no service select");
                }
            }

            #endregion

            #region exportimport
            else if (tabControl1.SelectedTab.Equals(tpImportExport))
            {
                if (radioButton2.Checked)//export
                {
                    if (lvTable2.CheckedItems.Count > 0)
                    {
                        #region set listviewitem 's Tag 
                        foreach (ListViewItem lvi in lvTable2.CheckedItems)
                        {
                            string tableName = lvi.Text;
                            tableName = WzdUtils.Quote(tableName, GlobalConnection);
                            TFieldAttrItems tdsItems = new TFieldAttrItems(lvi);
                            lvi.Tag = tdsItems;

                            String datatypestring = "";
                            switch (FServerData.DatabaseType)
                            {
                                case ClientType.ctMsSql:
                                    datatypestring = "Select a.name as 'columnname', b.name as 'typename', a.length as 'columnlength',a.prec as 'prec',a.scale as 'columnscale',a.isnullable as 'dbnull' FROM syscolumns a left join systypes b on a.xtype=b.xusertype where a.id=object_id('" + tableName + "')";
                                    break;
                                case ClientType.ctOracle:
                                    datatypestring = "select column_name,data_type ,data_length,data_precision,data_scale ,nullable from user_tab_columns where table_name  = '" + tableName + "'";
                                    break;
                                case ClientType.ctODBC:
                                case ClientType.ctOleDB:
                                case ClientType.ctInformix:
                                case ClientType.ctMySql:
                                    datatypestring = "select colname, coltype, collength FROM systables a, syscolumns b WHERE a.tabid = b.tabid AND tabname = '" + tableName + "';";
                                    break;
                                case ClientType.ctSybase:
                                    break;
                            }
                            IDbCommand cmd = GlobalConnection.CreateCommand();
                            cmd.CommandText = datatypestring;
                            if (GlobalConnection.State == ConnectionState.Closed)
                            { GlobalConnection.Open(); }
                            DataSet schemaTable = new DataSet();
                            IDbDataAdapter ida = WzdUtils.AllocateDataAdapter(FServerData.DatabaseType);
                            ida.SelectCommand = cmd;
                            ida.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                            ida.Fill(schemaTable);
                            switch (FServerData.DatabaseType)
                            {
                                case ClientType.ctMsSql:
                                    foreach (DataRow dr in schemaTable.Tables[0].Rows)
                                    {
                                        TFieldSchemaItem Item = new TFieldSchemaItem();
                                        Item.DataField = dr["columnname"].ToString();
                                        Item.DataType = dr["typename"].ToString();
                                        string typename = ((string)dr["typename"]).ToLower();
                                        int length = Int16.Parse(dr["columnlength"].ToString());
                                        if (typename == "nvarchar" || typename == "nchar" || typename == "decimal" || typename == "numeric") length = Int16.Parse(dr["prec"].ToString());
                                        Item.Length = length;
                                        if (typename == "decimal" || typename == "numeric")
                                        { Item.Scale = Int16.Parse(dr["columnscale"].ToString()); }
                                        Item.Caption = dr["columnname"].ToString();
                                        if (dr["dbnull"].ToString() == "0")
                                        { Item.AllowDBNull = false; }
                                        else Item.AllowDBNull = true;

                                        tdsItems.Add(Item);
                                    }
                                    break;
                                case ClientType.ctOracle:
                                    foreach (DataRow dr in schemaTable.Tables[0].Rows)
                                    {
                                        TFieldSchemaItem Item = new TFieldSchemaItem();
                                        Item.DataField = dr["column_name"].ToString();

                                        string typename = ((string)dr["data_type"]).ToLower();
                                        Item.DataType = typename;
                                        if (typename.StartsWith("interval year", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            Item.Length = Int32.Parse(dr["data_length"].ToString());
                                        }
                                        else if (typename.StartsWith("interval day", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            Item.Length = Int32.Parse(dr["data_length"].ToString());
                                            Item.Scale = Int32.Parse(dr["data_scale"].ToString());
                                        }
                                        if (typename == "nvarchar2" || typename == "nchar" || typename == "nclob")
                                        {
                                            Item.Length = Int32.Parse(dr["data_length"].ToString()) / 2;
                                        }
                                        else if (typename == "float")
                                        {
                                            Item.Length = Int32.Parse(dr["data_precision"].ToString());
                                        }
                                        else if (typename == "number")
                                        {
                                            if (dr["data_precision"] != DBNull.Value)
                                            {
                                                Item.Length = Int32.Parse(dr["data_precision"].ToString());
                                                Item.Scale = (dr["data_scale"] == DBNull.Value) ? 0 : Int32.Parse(dr["data_scale"].ToString());
                                            }
                                        }
                                        else if (typename == "varchar2" || typename == "char" || typename == "raw")
                                        {
                                            Item.Length = Int32.Parse(dr["data_length"].ToString());
                                        }

                                        if (dr["nullable"].ToString() == "N")
                                        { Item.AllowDBNull = false; }
                                        else Item.AllowDBNull = true;

                                        tdsItems.Add(Item);
                                    }
                                    break;
                                case ClientType.ctODBC:
                                case ClientType.ctOleDB:
                                case ClientType.ctInformix:
                                case ClientType.ctMySql:
                                    foreach (DataRow dr in schemaTable.Tables[0].Rows)
                                    {
                                        TFieldSchemaItem Item = new TFieldSchemaItem();
                                        string columnname = dr["colname"].ToString();
                                        Item.DataField = dr["colname"].ToString();
                                        int typeid = Int32.Parse(dr["coltype"].ToString());
                                        int collength = Int32.Parse(dr["collength"].ToString());
                                        bool nonull = typeid / 256 == 1 ? true : false;
                                        typeid = typeid % 256;
                                        string typestring = GetDataType(typeid);
                                        Item.DataType = typestring;
                                        if (typeid == 41 || typeid == 40 || typeid == 44)
                                        {
                                            IDbCommand cmdinfomix = GlobalConnection.CreateCommand();
                                            cmdinfomix.CommandText = "select b.colname,b.coltype,c.name FROM systables a,syscolumns b,sysxtdtypes c WHERE a.tabid = b.tabid AND a.tabname = '" + tableName + "' AND b.colname = '" + columnname + "' AND b.extended_id =c.extended_id;";
                                            if (GlobalConnection.State == ConnectionState.Closed)
                                            { GlobalConnection.Open(); }
                                            DataSet cmdinfomixTable = new DataSet();
                                            IDbDataAdapter idacmdinfomix = WzdUtils.AllocateDataAdapter(FServerData.DatabaseType);
                                            idacmdinfomix.SelectCommand = cmdinfomix;
                                            idacmdinfomix.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                                            idacmdinfomix.Fill(cmdinfomixTable);
                                            typestring = cmdinfomixTable.Tables[0].Rows[0]["name"].ToString();
                                        }
                                        int length = 0; int scale = 0;
                                        switch (typeid)
                                        {
                                            case (0):
                                            case (15): Item.Length = collength; break;
                                            case (5):
                                            case (8): GetTypeLength(collength, 0, ref length, ref scale); Item.Length = length; Item.Scale = scale; break;
                                            case (13):
                                            case (16): GetTypeLength(collength, 1, ref length, ref scale); Item.Length = length; Item.Scale = scale; break;
                                            case (10):
                                            case (14): GetDateTypeLength(collength, ref length); Item.Length = length; Item.Scale = collength; break;
                                        }
                                        if (nonull)
                                        { Item.AllowDBNull = false; }
                                        else Item.AllowDBNull = true;

                                        tdsItems.Add(Item);
                                    }
                                    break;
                                case ClientType.ctSybase:
                                    break;
                            }

                            //key
                            String sQLkey = "select * from " + tableName + " where 1=0";
                            IDbCommand cmdkey = GlobalConnection.CreateCommand();
                            cmdkey.CommandText = sQLkey;
                            if (GlobalConnection.State == ConnectionState.Closed)
                            { GlobalConnection.Open(); }
                            DataSet schemaTable2 = new DataSet();
                            IDbDataAdapter ida2 = WzdUtils.AllocateDataAdapter(FServerData.DatabaseType);
                            ida2.SelectCommand = cmdkey;
                            ida2.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                            ida2.Fill(schemaTable2);
                            foreach (DataColumn DR in schemaTable2.Tables[0].PrimaryKey)
                            {
                                for (int I = 0; I < tdsItems.Count; I++)
                                {
                                    if ((tdsItems[I] as TFieldSchemaItem).DataField == DR.ColumnName)
                                    {
                                        (tdsItems[I] as TFieldSchemaItem).IsKey = true;
                                    }
                                }
                            }
                        }
                        #endregion

                        var items = new Newtonsoft.Json.Linq.JArray();
                        var includedata = cbIncloudData.Checked;

                        foreach (ListViewItem lvi in lvTable2.CheckedItems)
                        {
                            string tableName = lvi.Text;
                            tableName = WzdUtils.Quote(tableName, GlobalConnection);
                            TFieldAttrItems tdsItems = lvi.Tag as TFieldAttrItems;
                            var tables = new Newtonsoft.Json.Linq.JArray();

                            for (int i = 0; i < tdsItems.Count; i++)
                            {
                                TFieldSchemaItem row = tdsItems[i] as TFieldSchemaItem;
                                var node = new Newtonsoft.Json.Linq.JObject();
                                node["ColumnName"] = new Newtonsoft.Json.Linq.JValue(row.DataField.ToString());
                                node["DataTypeName"] = new Newtonsoft.Json.Linq.JValue(row.DataType.ToString());
                                node["AllowDBNull"] = new Newtonsoft.Json.Linq.JValue(row.AllowDBNull.ToString());
                                node["IsKey"] = new Newtonsoft.Json.Linq.JValue(row.IsKey.ToString());
                                if (row.DataType.ToString().EndsWith("char"))
                                {
                                    var columnSize = (int)row.Length == int.MaxValue ? "max" : row.Length.ToString();
                                    node["ColumnSize"] = columnSize;
                                }

                                tables.Add(node);
                            }
                            var table = new Newtonsoft.Json.Linq.JObject();
                            table["tableName"] = tableName;
                            table["tableSchema"] = tables;
                            if (includedata.ToString().ToLower() == "true")
                            {
                                InfoCommand aInfoCommand = new InfoCommand(FServerData.DatabaseType);
                                aInfoCommand.Connection = InternalConnection;

                                aInfoCommand.CommandText = string.Format("SELECT * FROM {0}", tableName.ToString());

                                IDbDataAdapter DA = DBUtils.CreateDbDataAdapter(aInfoCommand);
                                DataSet D = new DataSet();
                                WzdUtils.FillDataAdapter(FServerData.DatabaseType, DA, D, "schema");
                                System.Collections.Generic.List<string> oldcolumnlist = new System.Collections.Generic.List<string>();
                                foreach (System.Data.DataColumn dc in D.Tables[0].Columns)
                                {
                                    if (!oldcolumnlist.Contains(dc.ColumnName.ToString()))
                                        oldcolumnlist.Add(dc.ColumnName.ToString());
                                }
                                var datas = new Newtonsoft.Json.Linq.JArray();
                                StringBuilder returnSB = new StringBuilder();
                                foreach (System.Data.DataRow dr in D.Tables[0].Rows)
                                {
                                    StringBuilder fieldvalue = new StringBuilder();
                                    #region create value string 
                                    for (int i = 0; i < D.Tables[0].Columns.Count; i++)
                                    {
                                        string columnname = D.Tables[0].Columns[i].ColumnName;

                                        switch (FServerData.DatabaseType)
                                        {
                                            case ClientType.ctMsSql:
                                                string columntypename = (dr.ItemArray as object[])[1].ToString();
                                                if (dr[i] == null || dr[i] == DBNull.Value)
                                                    fieldvalue.Append("null,");
                                                else
                                                    fieldvalue.Append(MarkSql(columntypename, dr[i].GetType().ToString(), dr[i]) + ",");
                                                break;
                                            case ClientType.ctOracle:
                                                if (dr[i] == null || dr[i] == DBNull.Value)
                                                    fieldvalue.Append("null,");
                                                else
                                                    fieldvalue.Append(MarkOracle(dr[i].GetType().ToString(), dr[i]) + ",");
                                                break;
                                            case ClientType.ctODBC:
                                            case ClientType.ctOleDB:
                                            case ClientType.ctInformix:
                                                if (dr[i] == null || dr[i] == DBNull.Value)
                                                    fieldvalue.Append("null,");
                                                else
                                                    fieldvalue.Append(MarkOracle(dr[i].GetType().ToString(), dr[i]) + ",");
                                                break;
                                            case ClientType.ctMySql:
                                                if (dr[i] == null || dr[i] == DBNull.Value)
                                                    fieldvalue.Append("null,");
                                                else
                                                    fieldvalue.Append(MarkMySql(dr[i].GetType().ToString(), dr[i]) + ",");

                                                break;
                                            case ClientType.ctSybase:
                                                if (dr[i] == null || dr[i] == DBNull.Value)
                                                    fieldvalue.Append("null,");
                                                else
                                                    fieldvalue.Append(MarkMySql(dr[i].GetType().ToString(), dr[i]) + ",");

                                                break;
                                        }
                                    }
                                    #endregion
                                    var insertstring = new Newtonsoft.Json.Linq.JObject();
                                    insertstring["insertString"] = "INSERT INTO " + tableName + " (" + string.Join(",", oldcolumnlist) + ") Values (" + fieldvalue.ToString().Substring(0, fieldvalue.Length - 1) + ")";
                                    datas.Add(insertstring);
                                }
                                table["insertStrings"] = datas;
                            }
                            items.Add(table);
                        }
                        var json = Newtonsoft.Json.JsonConvert.SerializeObject(items);
                        saveFileDialog1.InitialDirectory = FOutputFolder;
                        saveFileDialog1.FileName = "Database_Export.SQLX";
                        if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                        {
                            FileStream fs = new FileStream(saveFileDialog1.FileName, FileMode.Create);
                            StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
                            sw.Write(json);
                            sw.Close();
                            fs.Close();
                        }

                    }
                }
                else if (radioButton1.Checked)//import
                {
                    string filename = tbImport.Text;
                    FileStream fs = new FileStream(filename, FileMode.Open);
                    StreamReader sr = new StreamReader(fs, true);
                    string filestream = sr.ReadToEnd();
                    var items = (Newtonsoft.Json.Linq.JArray)Newtonsoft.Json.JsonConvert.DeserializeObject(filestream);
                    foreach (Newtonsoft.Json.Linq.JObject item in items)
                    {
                        var tableName = item["tableName"];
                        var tableSchema = (Newtonsoft.Json.Linq.JArray)item["tableSchema"];
                        var insertStrings = item["insertStrings"];
                        string INSERT_TABLE_SQL = "CREATE TABLE {0} ({1} {2})";
                        var columns = new System.Collections.Generic.List<string>();
                        var keycolumns = new System.Collections.Generic.List<string>();
                        System.Collections.Generic.List<string> columnlist = new System.Collections.Generic.List<string>();
                        foreach (Newtonsoft.Json.Linq.JObject column in tableSchema)
                        {
                            var nullValue = column["AllowDBNull"].ToString().ToLower() == bool.TrueString.ToLower() && column["IsKey"].ToString().ToLower() == bool.FalseString.ToLower() ? "NULL" : "NOT NULL";
                            //var primarykeyValue = column["IsKey"].ToString().ToLower() == bool.TrueString.ToLower() ? "PRIMARY KEY" : string.Empty;
                            var columnSize = column["ColumnSize"];
                            var columnName = column["ColumnName"];
                            var columnTypeName = column["DataTypeName"];
                            if (columnSize != null)
                            {
                                columnTypeName = columnTypeName + "(" + columnSize.ToString() + ")";
                            }
                            if (column["IsKey"].ToString().ToLower() == bool.TrueString.ToLower())
                            {
                                keycolumns.Add(columnName.ToString());
                            }
                            columns.Add(string.Format("{0} {1} {2} ", columnName, columnTypeName, nullValue));

                            columnlist.Add(columnName.ToString());
                        }

                        System.Collections.Generic.List<object> paramters = new System.Collections.Generic.List<object>() { tableName.ToString() };
                        string strSqlGetMenu = "select count(*) from sysobjects where id=object_id('" + tableName + "')";
                        InfoCommand aInfoCommand = new InfoCommand(FServerData.DatabaseType);
                        aInfoCommand.Connection = InternalConnection;
                        aInfoCommand.CommandText = strSqlGetMenu;
                        IDbDataAdapter DA = DBUtils.CreateDbDataAdapter(aInfoCommand);
                        DataSet D = new DataSet();
                        WzdUtils.FillDataAdapter(FServerData.DatabaseType, DA, D, "sysobjects");
                        string tablecount = D.Tables[0].Rows[0][0].ToString();
                        if (Convert.ToInt16(tablecount) == 0)
                        {
                            aInfoCommand.CommandText = string.Format(INSERT_TABLE_SQL, tableName, string.Join(",", columns), (keycolumns.Count > 0 ? ",PRIMARY KEY (" + string.Join(",", keycolumns) + ")" : string.Empty));
                            DA = DBUtils.CreateDbDataAdapter(aInfoCommand);
                            D = new DataSet();
                            WzdUtils.FillDataAdapter(FServerData.DatabaseType, DA, D, "Insert");

                            var insertStringList = new System.Collections.Generic.List<string>();
                            if (insertStrings != null)
                            {
                                foreach (Newtonsoft.Json.Linq.JObject insertString in insertStrings)
                                {
                                    insertStringList.Add(insertString["insertString"].ToString());
                                }
                                aInfoCommand.CommandText = string.Join(";", insertStringList);
                                DA = DBUtils.CreateDbDataAdapter(aInfoCommand);
                                D = new DataSet();
                                WzdUtils.FillDataAdapter(FServerData.DatabaseType, DA, D, "insertString");
                            }
                            MessageBox.Show("success");
                        }
                        else
                        {
                            if (insertStrings == null)
                            {
                                System.Collections.Generic.List<object> paramters2 = new System.Collections.Generic.List<object>() { };

                                aInfoCommand.CommandText = string.Format("SELECT * FROM {0} WHERE 1=0", tableName.ToString());

                                DA = DBUtils.CreateDbDataAdapter(aInfoCommand);
                                D = new DataSet();
                                WzdUtils.FillDataAdapter(FServerData.DatabaseType, DA, D, "schema");

                                //var dataSet = DataSetHelper.ExecuteSql(string.Format("SELECT * FROM {0}", tableName.ToString()), true, EFClientTools.EFServerReference.SDTableType.UserTable);
                                System.Collections.Generic.List<string> oldcolumnlist = new System.Collections.Generic.List<string>();
                                foreach (System.Data.DataColumn dc in D.Tables[0].Columns)
                                {
                                    if (columnlist.Contains(dc.ColumnName.ToString()))
                                        oldcolumnlist.Add(dc.ColumnName.ToString());
                                }
                                string temptablename = "SDTemp" + tableName;
                                string s = "insert into " + tableName + " (" + string.Join(",", oldcolumnlist) + ") select " + string.Join(",", oldcolumnlist) + " from " + temptablename;
                                int num1 = FAlias.IndexOf(cbDB.Text);
                                var dbConnection = WzdUtils.AllocateConnection(this.cbDB.Text, (ClientType)num1, false);
                                if (dbConnection.State == ConnectionState.Closed)
                                {
                                    dbConnection.Open();
                                }
                                DbCommand dcCommand = dbConnection.CreateCommand();
                                DbTransaction dbTran = dbConnection.BeginTransaction();
                                dcCommand.Transaction = dbTran;
                                try
                                {
                                    dcCommand.CommandText = "select * into " + temptablename + " from " + tableName + "";//建表和复制数据一起完成
                                    dcCommand.ExecuteNonQuery();
                                    dcCommand.CommandText = "Drop Table " + tableName;//删除原表
                                    dcCommand.ExecuteNonQuery();
                                    dcCommand.CommandText = string.Format(INSERT_TABLE_SQL, tableName, string.Join(",", columns), (keycolumns.Count > 0 ? ",PRIMARY KEY (" + string.Join(",", keycolumns) + ")" : string.Empty));//建新表
                                    dcCommand.ExecuteNonQuery();
                                    dcCommand.CommandText = "select name from syscolumns where  id=object_id(N'" + tableName + "') and COLUMNPROPERTY(id,name,'IsIdentity')=1";
                                    object identity = dcCommand.ExecuteScalar();
                                    dcCommand.CommandText = s;//复制数据到新表
                                    if (identity != null)
                                    {
                                        dcCommand.CommandText = "set identity_insert " + tableName + " on;" + dcCommand.CommandText + ";set identity_insert " + tableName + " off;";
                                    }
                                    dcCommand.ExecuteNonQuery();
                                    dcCommand.CommandText = "Drop Table " + temptablename;//删除临时表
                                    dcCommand.ExecuteNonQuery();
                                    dbTran.Commit();
                                    MessageBox.Show("success");
                                }
                                catch (Exception ex)
                                {
                                    dbTran.Rollback();
                                    MessageBox.Show(ex.Message);
                                }
                                finally
                                {
                                    dbTran.Dispose();
                                    dbConnection.Close();
                                }
                                //string temptablename = "SDTemp" + tableName;
                                //string s = "insert into " + tableName + " (" + string.Join(",", oldcolumnlist) + ") select " + string.Join(",", oldcolumnlist) + " from " + temptablename;

                                //paramters2.Add(tableName.ToString());
                                //paramters2.Add(temptablename);
                                //paramters2.Add(string.Format(INSERT_TABLE_SQL, tableName, string.Join(",", columns), (keycolumns.Count > 0 ? ",PRIMARY KEY (" + string.Join(",", keycolumns) + ")" : string.Empty)));
                                //paramters2.Add(s);
                                //var retobj = EFClientTools.ClientUtility.Client.CallServerMethod(clientInfo, "SDModuleUserServer", "CreateExistTable", paramters2).ToString();
                            }
                            else
                            {
                                MessageBox.Show("Table:" + tableName + " are exist");
                            }
                        }
                    }
                }
            }
            #endregion

            #region DDExportImport
            else if (tabControl1.SelectedTab.Equals(tpDDImportExport))
            {
                if (rbddExport.Checked)//export
                {
                    if (lvDDExport.CheckedItems.Count > 0)
                    {
                        DataSet ds = new DataSet();
                        foreach (ListViewItem lvi in lvDDExport.CheckedItems)
                        {
                            string tableName = lvi.Text;
                            DataSet ds2 = GetDDByTableName(tableName);
                            ds.Merge(ds2);
                        }
                        saveFileDialog2.InitialDirectory = FOutputFolder;
                        saveFileDialog2.FileName = "dictionary.xls";

                        if (saveFileDialog2.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            var path = saveFileDialog2.FileName;
                            List<string> s = new List<string>();
                            PublicTest.ExportToExcel(ds.Tables[0], path, "", s);
                        }
                    }
                }
                else if (rbddImport.Checked)//import
                {
                    if (tbDDImport.Text != string.Empty)
                    {
                        FileStream fs = new FileStream(tbDDImport.Text, FileMode.Open);
                        InfoCommand aInfoCommand = new InfoCommand(FServerData.DatabaseType);
                        aInfoCommand.Connection = InternalConnection;

                        aInfoCommand.CommandText = string.Format("SELECT * FROM COLDEF WHERE 1=0");

                        IDbDataAdapter DA = DBUtils.CreateDbDataAdapter(aInfoCommand);
                        DataSet D = new DataSet();
                        WzdUtils.FillDataAdapter(FServerData.DatabaseType, DA, D, "Table");
                        DataTable dt = D.Tables[0];

                        PublicTest.ImportFromExcel(dt, fs, 1, 0);
                        fs.Close();
                        bool OK = true;
                        List<string> tableNameList = new List<string>();
                        foreach (DataRow dr in dt.Rows)
                        {
                            if (!tableNameList.Contains(dr["TABLE_NAME"].ToString()))
                            {
                                tableNameList.Add(dr["TABLE_NAME"].ToString());
                            }
                        }
                        foreach (string tableName in tableNameList)
                        {
                            int num1 = FAlias.IndexOf(cbDB.Text);
                            var dbConnection = WzdUtils.AllocateConnection(this.cbDB.Text, (ClientType)num1, false);
                            if (dbConnection.State == ConnectionState.Closed)
                            {
                                dbConnection.Open();
                            }
                            DbCommand dcCommand = dbConnection.CreateCommand();
                            DbTransaction dbTran = dbConnection.BeginTransaction();
                            dcCommand.Transaction = dbTran;
                            try
                            {
                                dcCommand.CommandText = "DELETE COLDEF WHERE TABLE_NAME='" + tableName + "'";
                                dcCommand.ExecuteNonQuery();
                                dbTran.Commit();
                            }
                            catch (Exception ex)
                            {
                                OK = false;
                                dbTran.Rollback();
                                MessageBox.Show(ex.Message);
                            }
                            finally
                            {
                                dbTran.Dispose();
                                dbConnection.Close();
                            }
                        }
                        foreach (DataRow dr in D.Tables[0].Rows)
                        {
                            string InsertString = "";
                            foreach (DataColumn dc in D.Tables[0].Columns)
                            {
                                if (InsertString != "") InsertString += ",";
                                if (dc.DataType == typeof(Int32) || dc.DataType == typeof(Int64) || dc.DataType == typeof(Double))
                                    InsertString += dr[dc].ToString();
                                else
                                    InsertString += "'" + dr[dc].ToString() + "'";
                            }
                            int num1 = FAlias.IndexOf(cbDB.Text);
                            var dbConnection = WzdUtils.AllocateConnection(this.cbDB.Text, (ClientType)num1, false);
                            if (dbConnection.State == ConnectionState.Closed)
                            {
                                dbConnection.Open();
                            }
                            DbCommand dcCommand = dbConnection.CreateCommand();
                            DbTransaction dbTran = dbConnection.BeginTransaction();
                            dcCommand.Transaction = dbTran;
                            try
                            {
                                dcCommand.CommandText = "INSERT INTO COLDEF VALUES (" + InsertString + ")";
                                dcCommand.ExecuteNonQuery();
                                dbTran.Commit();
                            }
                            catch (Exception ex)
                            {
                                OK = false;
                                dbTran.Rollback();
                                MessageBox.Show(ex.Message);
                            }
                            finally
                            {
                                dbTran.Dispose();
                                dbConnection.Close();
                            }
                        }
                        if (OK) MessageBox.Show("success");
                        //int num2 = FAlias.I
                        
                    }
                }
            }
            #endregion
            //this.Show();
        }
        private void getChecked(Dictionary<string, object> d, TreeNode parentnode)
        {
            foreach (TreeNode node in parentnode.Nodes)
            {
                if (node.Checked)
                {
                    if (node.Tag != null)
                    {
                        string[] astring = node.Tag as string[];
                        if (astring.Length > 2)
                            d.Add(astring[1] +"\\"+ astring[2], astring);
                    }
                }
                if (node.Nodes.Count > 0)
                {
                    getChecked(d, node);
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

        private void cbDB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbDB.Text != "")
            {
                string type = FindDBType(cbDB.Text);
                int num1 = FAlias.IndexOf(cbDB.Text);

                FServerData.EEPAlias = cbDB.Text;
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
                    //case "7":
                    //    FServerData.DatabaseType = ClientType.ctSybase; break;
                }
                cbDatabaseType.SelectedIndex = (int)FServerData.DatabaseType;
                if (InternalConnection == null)
                {
                    InternalConnection = WzdUtils.AllocateConnection(this.cbDB.Text, (ClientType)num1, false);
                }
                else
                {
                    if (InternalConnection.State == ConnectionState.Open)
                        InternalConnection.Close();
                    InternalConnection = WzdUtils.AllocateConnection(this.cbDB.Text, (ClientType)num1, false);
                    //InternalConnection.ConnectionString = tbConnectionString.Text;
                }
                if (InternalConnection.ConnectionString.Trim() != "")
                {
                    try
                    {
                        InternalConnection.Open();
                    }
                    catch (Exception E)
                    {
                        MessageBox.Show(string.Format("Database ConnnectionString information error, please reset ConnectionString.\nThe error message:\n{0}", E.Message));
                    }
                }
                FServerData.ResetDatabaseConnection();
                GetTableNames(lvTables);
                GetTableNames(lvTable2);
                GetDDTableNames(lvDDExport);
            }
        }

        private void rbService_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked == true)
            {
                //tbRoot.Enabled = true;
                //btRootSelect.Enabled = true;
            }

        }

        private void rbTable_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked == true)
            {
                //tbRoot.Enabled = false;
                //btRootSelect.Enabled = false;
            }
        }

        private void rbPage_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked == true)
            {
                //tbRoot.Enabled = true;
                //btRootSelect.Enabled = true;
            }

        }

        private void btFolder_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                tbFolder.Text = folderBrowserDialog1.SelectedPath ;
            }

        }

        //Server Part
        private void GetAllServer()
        {
            string s = EditionDifference.ActiveSolutionName();

            EEPRemoteModule remoteObject = new EEPRemoteModule();
            object[] myRet = remoteObject.GetSqlCommandList(new object[] { (object)"", (object)"", (object)"", (object)"", (object)"", (object)"", (object)s });
            if ((null != myRet))
            {
                if (0 == (int)(myRet[0]))
                {
                    string[] sList = (string[])(myRet[1]);

                    if (sList.Length > 0)
                    {
                        List<string> servicelist = new List<string>();
                        lvService.BeginUpdate();
                        for (int i = 0; i < sList.Length; i++)
                        {
                            string sCommand = sList[i].Substring(0, sList[i].IndexOf('.'));
                            if (!servicelist.Contains(sCommand))
                            {
                                servicelist.Add(sCommand);
                                lvService.Items.Add(sCommand);
                            }
                        }
                        lvService.EndUpdate();
                    }

                    lvService.Sort();
                }
                else
                {
                    MessageBox.Show(myRet[1].ToString());
                }
            }
        }
        //Server part ends

        //Table Part
        private MemoryStream GetTableSchemaStream()
        {
            MemoryStream ms = new MemoryStream();
            using (XmlWriter writer = XmlWriter.Create(ms))
            {
                try
                {
                    string printNodeXml = "PrintNodeXML";
                    string xElement = "xElement";
                    string xElementName = "xElementName";
                    string nodetype = "nodetype";

                    string properties = "properties";
                    string children = "children";

                    string Caption = "Caption";
                    string isNullable = "isNullable";
                    string DataType = "DataType";
                    string Length = "Length";
                    string Scale = "Scale";
                    string IsKey = "IsKey";

                    XmlDocument xdoc2 = new XmlDocument();
                    XmlDeclaration xdel2 = xdoc2.CreateXmlDeclaration("1.0", "utf-8", "yes");
                    xdoc2.AppendChild(xdel2);

                    //root
                    XmlNode xn2 = xdoc2.CreateElement(printNodeXml);
                    xdoc2.AppendChild(xn2);

                    //get coldef
                    string tablenamelist = string.Empty;
                    foreach (ListViewItem item in lvTables.CheckedItems)
                    {
                        if (tablenamelist != string.Empty)
                        {
                            tablenamelist += ",";
                        }
                        tablenamelist += "'" + item.Text + "'";
                    }

                    //TableNode                 
                    foreach (ListViewItem item in lvTables.CheckedItems)
                    {
                        XmlNode tablenode = xdoc2.CreateElement(xElement);
                        #region
                        XmlAttribute att = xdoc2.CreateAttribute(nodetype);
                        att.Value = "TableNode";
                        tablenode.Attributes.Append(att);

                        att = xdoc2.CreateAttribute(xElementName);
                        att.Value = item.Text;
                        tablenode.Attributes.Append(att);

                        xn2.AppendChild(tablenode);
                        #endregion

                        //table node properties
                        XmlNode propertiesNode = xdoc2.CreateElement(properties);
                        #region
                        att = xdoc2.CreateAttribute("Caption");
                        att.Value = item.Text;
                        propertiesNode.Attributes.Append(att);

                        att = xdoc2.CreateAttribute("Name");
                        att.Value = item.Text;
                        propertiesNode.Attributes.Append(att);
                        tablenode.AppendChild(propertiesNode);
                        #endregion

                        //children
                        XmlNode childrennode = xdoc2.CreateElement(children);
                        tablenode.AppendChild(childrennode);

                        for (int num1 = 0; num1 < (item.Tag as TFieldAttrItems).Count; num1++)
                        {
                            TFieldSchemaItem fieldItem = (item.Tag as TFieldAttrItems)[num1] as TFieldSchemaItem;
                            //field node
                            XmlNode fieldnode = xdoc2.CreateElement(xElement);
                            #region
                            att = xdoc2.CreateAttribute(xElementName);
                            att.Value = fieldItem.DataField;
                            fieldnode.Attributes.Append(att);

                            att = xdoc2.CreateAttribute(nodetype);
                            att.Value = "FieldNode";
                            fieldnode.Attributes.Append(att);

                            childrennode.AppendChild(fieldnode);
                            #endregion

                            //field properties
                            propertiesNode = xdoc2.CreateElement(properties);
                            #region
                            att = xdoc2.CreateAttribute(Caption);
                            att.Value = fieldItem.Caption;
                            propertiesNode.Attributes.Append(att);

                            att = xdoc2.CreateAttribute("Name");
                            att.Value = fieldItem.DataField;
                            propertiesNode.Attributes.Append(att);

                            att = xdoc2.CreateAttribute(isNullable);
                            att.Value = fieldItem.AllowDBNull.ToString();
                            propertiesNode.Attributes.Append(att);

                            att = xdoc2.CreateAttribute(DataType);
                            att.Value = fieldItem.DataType;
                            propertiesNode.Attributes.Append(att);

                            att = xdoc2.CreateAttribute(Length);
                            att.Value = fieldItem.Length.ToString();
                            propertiesNode.Attributes.Append(att);

                            att = xdoc2.CreateAttribute(Scale);
                            att.Value = fieldItem.Scale.ToString();
                            propertiesNode.Attributes.Append(att);

                            att = xdoc2.CreateAttribute(IsKey);
                            att.Value = fieldItem.IsKey.ToString();
                            propertiesNode.Attributes.Append(att);

                            fieldnode.AppendChild(propertiesNode);
                            #endregion

                        }
                    }
                    xdoc2.WriteTo(writer);

                }
                catch (Exception ex) { MessageBox.Show(ex.Message, "Wrong1", MessageBoxButtons.OK); }
                finally
                {
                    writer.Close();
                }
            }
            return ms;
        }

        private void GetTableNames(ListView LV)
        {
            int I;
            ListViewItem lvi;
            TStringList aTableCaptionList = new TStringList();
            TStringList aTableList = new TStringList();
            String[] Params = null;
            String ViewFieldName = "TABLE_NAME";
            if (FServerData.DatabaseType == ClientType.ctOracle)
            {
                String UserID = WzdUtils.GetFieldParam(FServerData.ConnectionString, "USER ID");
                Params = new String[] { UserID };
                ViewFieldName = "VIEW_NAME";
            }

            if (FServerData != null)
            {
                aTableCaptionList = FServerData.TableNameCaptionList;
                aTableList = FServerData.TableNameList;
            }
            else
            {
                GetTableCaptionFromCOLDEF(aTableCaptionList);
                DataTable D = InternalConnection.GetSchema("Tables", Params);
                D.Select("", "TABLE_NAME DESC");
                for (I = 0; I < D.Rows.Count; I++)
                    aTableList.Add(D.Rows[I]["TABLE_NAME"].ToString());

                DataTable T = InternalConnection.GetSchema("Views", Params);
                DataRow[] dr = T.Select("", "VIEW_NAME ASC");
                foreach (DataRow DR in dr)
                {
                    aTableList.Add(DR["VIEW_NAME"].ToString());
                }
            }

            if (InternalConnection.GetType().FullName != "IBM.Data.Informix.IfxConnection")
            {
                DataTable D1 = InternalConnection.GetSchema("Views", Params);
                D1.Select("", ViewFieldName + " DESC");
                foreach (DataRow DR in D1.Rows)
                {
                    if (aTableList.IndexOf(DR[ViewFieldName].ToString()) < 0)
                        aTableList.Add(DR[ViewFieldName].ToString());
                }
                if (InternalConnection.GetType().Name == "OracleConnection")
                {
                    DataTable T = InternalConnection.GetSchema("Synonyms", Params);
                    T.Select("", "SYNONYM_NAME DESC");
                    foreach (DataRow DR in T.Rows)
                    {
                        String S = "";
                        if (!String.IsNullOrEmpty(DR["OWNER"].ToString()))
                            S = DR["OWNER"].ToString() + '.';
                        aTableList.Add(S + DR["SYNONYM_NAME"].ToString());
                    }
                }
            }

            LV.Items.Clear();

            if (aTableList.Count > 0)
            {
                LV.BeginUpdate();
                for (I = 0; I < aTableList.Count; I++)
                {
                    lvi = new ListViewItem();
                    lvi.Text = aTableList[I].ToString();
                    LV.Items.Add(lvi);
                    lvi.SubItems.Add(aTableList.Values(lvi.Text));
                }
                LV.EndUpdate();
            }

            LV.Sort();
        }

        private void GetTableCaptionFromCOLDEF(TStringList aTableNameCaptionList)
        {
            int I;
            DataRow DR;
            DataSet D = GetDDTABLENAMEAll();
            aTableNameCaptionList.Clear();
            for (I = 0; I < D.Tables[0].Rows.Count; I++)
            {
                DR = D.Tables[0].Rows[I];
                if (DR["TABLE_NAME"].ToString().Trim() != "" && DR["CAPTION"].ToString().Trim() != "")
                    aTableNameCaptionList.Add(DR["TABLE_NAME"].ToString().Trim() + "=" + DR["CAPTION"].ToString().Trim());
            }
        }

        private void GetDDTableNames(ListView LV)
        {
            int I;
            ListViewItem lvi;
            TStringList aTableCaptionList = new TStringList();
            TStringList aTableList = new TStringList();
            String[] Params = null;
            if (FServerData.DatabaseType == ClientType.ctOracle)
            {
                String UserID = WzdUtils.GetFieldParam(FServerData.ConnectionString, "USER ID");
                Params = new String[] { UserID };
            }
            DataTable D = GetDDTableNameOnly().Tables[0];
                for (I = 0; I < D.Rows.Count; I++)
                    aTableList.Add(D.Rows[I]["TABLE_NAME"].ToString());

            LV.Items.Clear();

            if (aTableList.Count > 0)
            {
                LV.BeginUpdate();
                for (I = 0; I < aTableList.Count; I++)
                {
                    lvi = new ListViewItem();
                    lvi.Text = aTableList[I].ToString();
                    LV.Items.Add(lvi);
                }
                LV.EndUpdate();
            }

            LV.Sort();
        }

        private DataSet GetDDTABLENAMEAll()
        {
            InfoCommand aInfoCommand = new InfoCommand(FServerData.DatabaseType);
            aInfoCommand.Connection = InternalConnection;
            aInfoCommand.CommandText = "Select TABLE_NAME, CAPTION from COLDEF";
            IDbDataAdapter DA = DBUtils.CreateDbDataAdapter(aInfoCommand);
            DataSet D = new DataSet();
            WzdUtils.FillDataAdapter(FServerData.DatabaseType, DA, D, "COLDEF");
            return D;
        }
        private DataSet GetDDTableNameOnly()
        {             
            InfoCommand aInfoCommand = new InfoCommand(FServerData.DatabaseType);
            aInfoCommand.Connection = InternalConnection;
            aInfoCommand.CommandText = "Select distinct TABLE_NAME from COLDEF";
            IDbDataAdapter DA = DBUtils.CreateDbDataAdapter(aInfoCommand);
            DataSet D = new DataSet();
            WzdUtils.FillDataAdapter(FServerData.DatabaseType, DA, D, "COLDEF");
            return D;
        }
        private DataSet GetDDByTableName(string tablename)
        {
            InfoCommand aInfoCommand = new InfoCommand(FServerData.DatabaseType);
            aInfoCommand.Connection = InternalConnection;
            aInfoCommand.CommandText = "Select * from COLDEF where TABLE_NAME ='" + tablename + "'";
            IDbDataAdapter DA = DBUtils.CreateDbDataAdapter(aInfoCommand);
            DataSet D = new DataSet();
            WzdUtils.FillDataAdapter(FServerData.DatabaseType, DA, D, "COLDEF");
            return D;
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
        //Table Part End

        //Page Part
        private void btRootSelect_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                tbRoot.Text = openFileDialog.FileName;
                if (string.Compare(tbRoot.Text, FDTE2.Solution.FullName) != 0)
                {
                    FDTE2.Solution.Open(tbRoot.Text);
                }
                GetWebSite();
            }

        }

        private void GetWebSite()
        {
            cbWebSite.Items.Clear();
            foreach (Project P in FDTE2.Solution.Projects)
            {
                if (string.Compare(P.Kind, "{E24C65DC-7377-472b-9ABA-BC803B73C61A}") == 0)
                {
                    cbWebSite.Items.Add(P.FullName);
                }
            }

            if (cbWebSite.Items.Count == 1)
            {
                cbWebSite.SelectedIndex = 0;
                //cbWebSite_SelectedIndexChanged(new object(), new EventArgs());
            }
        }

        private void cbWebSite_SelectedIndexChanged(object sender, EventArgs e)
        {
            FWebSite= (sender as ComboBox).SelectedItem.ToString();
            DirectoryInfo dirInfo = new DirectoryInfo(FWebSite);
            treeView1.Nodes.Clear();
            treeView1.Nodes.Add(FWebSite);
            getALLWebItems(dirInfo, treeView1.Nodes[0]);
            treeView1.Nodes[0].Expand();
        }

        private void getALLWebItems(DirectoryInfo directoryinfo, TreeNode node)
        {
            FileSystemInfo[] fSIs = directoryinfo.GetFileSystemInfos();
            FileSystemInfo fSI;

            try
            {
                for (int i = 0; i < fSIs.Length; i++)
                {
                    fSI = fSIs[i];
                    if (fSI.GetType() == typeof(FileInfo))
                    {
                        FileInfo fInfo = (FileInfo)fSI;
                        if (fInfo.Extension == ".aspx")
                        {
                            TreeNode trn2 = new TreeNode(fInfo.Name);
                            string sc = fInfo.DirectoryName.Substring(FWebSite.Length-1);
                            trn2.Tag = new string[] { "aspx", sc, fInfo.Name };
                            node.Nodes.Add(trn2);
                            //暂时全部不加入
                            //DirectoryInfo fdir = fInfo.Directory;
                            //FileSystemInfo[] ffSIs = fdir.GetFileSystemInfos();
                            //foreach (FileSystemInfo ffSI in ffSIs)
                            //{
                            //    if (ffSI.Name == fInfo.Name.Substring(0, fInfo.Name.Length - 5) + ".aspx.cs")
                            //    {
                            //        trn2.Nodes.Add(ffSI.Name);
                            //    }
                            //    else if (ffSI.Name == fInfo.Name.Substring(0, fInfo.Name.Length - 5) + ".exe.config")
                            //    {
                            //        trn2.Nodes.Add(ffSI.Name);
                            //    }
                            //    else if (ffSI.Name == fInfo.Name.Substring(0, fInfo.Name.Length - 5) + ".aspx.resx")
                            //    {
                            //        trn2.Nodes.Add(ffSI.Name);
                            //    }
                            //    else if (ffSI.Name == fInfo.Name.Substring(0, fInfo.Name.Length - 5) + ".aspx.vb")
                            //    {
                            //        trn2.Nodes.Add(ffSI.Name);
                            //    }
                            //    else if (ffSI.Name == fInfo.Name.Substring(0, fInfo.Name.Length - 5) + ".aspx.vi_VN.resx")
                            //    {
                            //        trn2.Nodes.Add(ffSI.Name);
                            //    }
                            //    else if (ffSI.Name == fInfo.Name.Substring(0, fInfo.Name.Length - 5) + ".xml")
                            //    {
                            //        trn2.Nodes.Add(ffSI.Name);
                            //    }

                            //    //要在aspx的文件节点下显示什么类型的文件就在这边加
                            //}

                        }
                    }
                    else if (fSI.GetType() == typeof(DirectoryInfo))
                    {
                        DirectoryInfo dir = (DirectoryInfo)fSI;
                        TreeNode no = new TreeNode(dir.Name);
                        node.Nodes.Add(no);
                        no.Tag = new string[] { "Folder" };
                        getALLWebItems(dir, no);
                    }
                    else
                        MessageBox.Show("Ooops");
                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        private void chTableAll_CheckedChanged(object sender, EventArgs e)
        {
            bool aiphi = false;
            if (chTableAll.CheckState == CheckState.Checked)
            { aiphi = true; }

            for (int i = 0; i < lvTables.Items.Count; i++)
            {
                lvTables.Items[i].Checked = aiphi;
            }
        }

        private void cbServerAll_CheckedChanged(object sender, EventArgs e)
        {
            bool aiphi = false;
            if (cbServerAll.CheckState == CheckState.Checked)
            { aiphi = true; }

            for (int i = 0; i < lvService.Items.Count; i++)
            {
                lvService.Items[i].Checked = aiphi;
            }
        }

 
        //Page Part End

        #region For informix datatype and datalength
        public static string GetDataType(int p)
        {
            switch (p)
            {
                case (0): return "char";
                case (1): return "smallint";
                case (2): return "integer";
                case (3): return "float";
                case (4): return "smallfloat";
                case (5): return "decimal";
                case (6): return "serial";
                case (7): return "date";
                case (8): return "money";
                case (10): return "datetime";
                case (11): return "byte";
                case (12): return "text";
                case (13): return "varchar";
                case (14): return "interval";
                case (15): return "nchar";
                case (16): return "nvarchar";
                case (17): return "int8";
                default: return "char";
            }
        }

        public static int int_start(int i)
        {
            switch (i)
            {
                case (1): return 1;
                case (3): return 5;
                case (5): return 7;
                case (7): return 9;
                case (9): return 11;
                case (11): return 13;
                case (12): return 15;
                case (13): return 16;
                case (14): return 17;
                case (15): return 18;
                case (16): return 19;
                default: return 1;
            }
        }

        public static int int_end(int i)
        {
            switch (i)
            {
                case (1): return 5;
                case (3): return 7;
                case (5): return 9;
                case (7): return 11;
                case (9): return 13;
                case (11): return 15;
                case (12): return 16;
                case (13): return 17;
                case (14): return 18;
                case (15): return 19;
                case (16): return 20;
                default: return 5;
            }
        }

        public static string GetDateType(int i)
        {
            switch (i)
            {
                case (1): return "year";
                case (3): return "month";
                case (5): return "day";
                case (7): return "hour";
                case (9): return "minute";
                case (11): return "second";
                case (12): return "praction(1)";
                case (13): return "praction(2)";
                case (14): return "praction(3)";
                case (15): return "praction(4)";
                case (16): return "praction(5)";
                default: return "year";
            }
        }

        public static string GetDateTypeLength(int collength, ref int length)
        {
            string strg = "";
            int i = collength % 16 + 1;
            int j = ((collength % 256) / 16) + 1;
            int k = collength / 256;
            int ln = int_end(i) - int_start(j);
            ln = k - ln;
            if (ln == 0 || j > 11) { strg = GetDateType(j) + " to " + GetDateType(i); }
            else
            {
                k = int_end(j) - int_start(j);
                k = k + ln;
                strg = GetDateType(j) + "(" + k + ") to " + GetDateType(i);
                length = k;
            }
            return strg;
        }

        public static string GetDateTypeLength(int collength)
        {
            string strg = "";
            int i = collength % 16 + 1;
            int j = ((collength % 256) / 16) + 1;
            int k = collength / 256;
            int ln = int_end(i) - int_start(j);
            ln = k - ln;
            if (ln == 0 || j > 11) { strg = GetDateType(j) + " to " + GetDateType(i); }
            else
            {
                k = int_end(j) - int_start(j);
                k = k + ln;
                strg = GetDateType(j) + "(" + k + ") to " + GetDateType(i);
            }
            return strg;
        }

        public static string GetTypeLength(int p, int a, ref int length, ref int scale)
        {
            string strg = "";
            int i = p / 256;
            int j = p % 256;
            if (a == 0)
            {
                if (j > i) { strg = i.ToString(); length = i; }
                else { strg = i.ToString() + "," + j.ToString(); length = i; scale = j; }
            }
            else
            {
                if (i == 0) { strg = j.ToString(); length = j; }
                else { strg = j.ToString() + "," + i.ToString(); length = j; scale = i; }
            }
            return strg;
        }

        public static string GetTypeLength(int p, int a)
        {
            string strg = "";
            int i = p / 256;
            int j = p % 256;
            if (a == 0)
            {
                if (j > i) { strg = i.ToString(); }
                else { strg = i.ToString() + "," + j.ToString(); }
            }
            else
            {
                if (i == 0) { strg = j.ToString(); }
                else { strg = j.ToString() + "," + i.ToString(); }
            }
            return strg;
        }
        #endregion

        private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
            bool aiphi = e.Node.Checked;
            if (e.Node.Nodes.Count > 0)
            {
                foreach (TreeNode node in e.Node.Nodes)
                {
                    node.Checked = aiphi;
                }
            }
        }

        private void btSaveSetting_Click(object sender, EventArgs e)
        {
            SetConfig();
        }

        private void btOpenFielDialog_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                tbImport.Text = openFileDialog1.FileName;
            }
        }

        private void tpImportExport_Click(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
            {
                gbImport.Enabled = true;
            }
            else
            {
                gbImport.Enabled = false;
            }

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
            {
                gbExport.Enabled = true;
            }
            else
            {
                gbExport.Enabled = false;
            }
        }

        private void rbddImport_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
            {
                gbDDImport.Enabled = true;
            }
            else
            {
                gbDDExport.Enabled = false;
            }

        }

        private void rbddExport_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
            {
                gbDDExport.Enabled = true;
            }
            else
            {
                gbDDImport.Enabled = false;
            }

        }
        private void cbSelectAll1_CheckedChanged(object sender, EventArgs e)
        {
            bool aiphi = false;
            if (cbSelectAll1.CheckState == CheckState.Checked)
            { aiphi = true; }

            for (int i = 0; i < lvTable2.Items.Count; i++)
            {
                lvTable2.Items[i].Checked = aiphi;
            }
        }

        private void cbDDSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            bool aiphi = false;
            if (cbDDSelectAll.CheckState == CheckState.Checked)
            { aiphi = true; }

            for (int i = 0; i < lvDDExport.Items.Count; i++)
            {
                lvDDExport.Items[i].Checked = aiphi;
            }
        }

        private void btDDImport_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                tbDDImport.Text = openFileDialog1.FileName;
            }
        }

        #region get database's type and value insert string
        private Char _marker = '\'';
        private String MarkSql(String typeName, String type, Object columnValue)
        {
            bool isN = false;
            if (string.Compare(typeName, "NChar", true) == 0 || string.Compare(typeName, "NVarChar", true) == 0
                || string.Compare(typeName, "NText", true) == 0)
            {
                isN = true;
            }
            if (Type.GetType(type).Equals(typeof(Char)) || Type.GetType(type).Equals(typeof(String)) || Type.GetType(type).Equals(typeof(Guid)))
            {
                string newcolumnValue = columnValue.ToString().Replace("'", "''");
                if (isN)
                    return "N" + _marker.ToString() + newcolumnValue + _marker.ToString();
                else
                    return _marker.ToString() + newcolumnValue + _marker.ToString();
            }
            else if (Type.GetType(type).Equals(typeof(Boolean)))
            {
                Boolean b = (Boolean)columnValue;
                if (b)
                    return "1";
                else
                    return "0";
            }
            else if (Type.GetType(type).Equals(typeof(DateTime)))
            {
                DateTime t = Convert.ToDateTime(columnValue);
                string s = t.Year.ToString() + "-" + t.Month.ToString() + "-" + t.Day.ToString() + " "
                    + t.Hour.ToString() + ":" + t.Minute.ToString() + ":" + t.Second.ToString();
                return _marker.ToString() + s + _marker.ToString();
            }
            else if (Type.GetType(type).Equals(typeof(Byte[])))
            {
                StringBuilder builder = new StringBuilder("0x");
                foreach (Byte b in (Byte[])columnValue)
                {
                    string tmp = Convert.ToString(b, 16);
                    if (tmp.Length < 2)
                        tmp = "0" + tmp;
                    builder.Append(tmp);
                }
                return builder.ToString();
            }
            else
            {
                return columnValue.ToString();
            }
        }

        private String MarkOracle(String type, Object columnValue)
        {
            if (Type.GetType(type).Equals(typeof(Char)) || Type.GetType(type).Equals(typeof(String)))
            {
                string newcolumnValue = columnValue.ToString().Replace("'", "''");

                return _marker.ToString() + newcolumnValue.ToString() + _marker.ToString();
            }
            else if (Type.GetType(type).Equals(typeof(Boolean)))
            {
                Boolean b = (Boolean)columnValue;
                if (b)
                    return "1";
                else
                    return "0";
            }
            else if (Type.GetType(type).Equals(typeof(DateTime)))
            {
                String s = "";
                DateTime t = Convert.ToDateTime(columnValue);
                s = t.Year.ToString() + "-" + t.Month.ToString() + "-" + t.Day.ToString() + " "
                    + t.Hour.ToString() + ":" + t.Minute.ToString() + ":" + t.Second.ToString();
                s = "to_date('" + s + "', 'yyyy-mm-dd hh24:mi:ss')";
                //return _marker.ToString() + s + _marker.ToString();
                return s;
            }
            else if (Type.GetType(type).Equals(typeof(Byte[])))
            {
                StringBuilder builder = new StringBuilder("0x");
                foreach (Byte b in (Byte[])columnValue)
                {
                    string tmp = Convert.ToString(b, 16);
                    if (tmp.Length < 2)
                        tmp = "0" + tmp;
                    builder.Append(tmp);
                }
                return builder.ToString();
            }
            else
            {
                return columnValue.ToString();
            }
        }

        private String MarkMySql(String type, Object columnValue)
        {
            if (Type.GetType(type).Equals(typeof(Char)) || Type.GetType(type).Equals(typeof(String)) || Type.GetType(type).Equals(typeof(TimeSpan)))
            {
                string newcolumnValue = columnValue.ToString().Replace("'", "''");

                return _marker.ToString() + newcolumnValue.ToString() + _marker.ToString();
            }
            else if (Type.GetType(type).Equals(typeof(Boolean)))
            {
                Boolean b = (Boolean)columnValue;
                if (b)
                    return "1";
                else
                    return "0";
            }
            else if (Type.GetType(type).Equals(typeof(DateTime)))
            {
                String s = "";
                DateTime t = Convert.ToDateTime(columnValue);
                s = "'" + t.Year.ToString() + "-" + t.Month.ToString() + "-" + t.Day.ToString() + " "
                    + t.Hour.ToString() + ":" + t.Minute.ToString() + ":" + t.Second.ToString() + "'";
                return s;
            }
            else if (Type.GetType(type).Equals(typeof(Byte[])))
            {
                StringBuilder builder = new StringBuilder("0x");
                foreach (Byte b in (Byte[])columnValue)
                {
                    string tmp = Convert.ToString(b, 16);
                    if (tmp.Length < 2)
                        tmp = "0" + tmp;
                    builder.Append(tmp);
                }
                return builder.ToString();
            }
            else
            {
                return columnValue.ToString();
            }
        }

        private String MarkOdbc(String type, Object columnValue)
        {
            if (Type.GetType(type).Equals(typeof(Char)) || Type.GetType(type).Equals(typeof(String)))
            {
                return _marker.ToString() + columnValue.ToString() + _marker.ToString();
            }
            else if (Type.GetType(type).Equals(typeof(Boolean)))
            {
                Boolean b = (Boolean)columnValue;
                if (b)
                    return "1";
                else
                    return "0";
            }
            else if (Type.GetType(type).Equals(typeof(DateTime)))
            {
                string s = "";

                DateTime t = Convert.ToDateTime(columnValue);
                s = t.Year.ToString() + checkDate(t.Month.ToString()) + checkDate(t.Day.ToString()) + checkDate(t.Hour.ToString()) + checkDate(t.Minute.ToString()) + checkDate(t.Second.ToString());
                s = "to_date('" + s + "', '%Y%m%d%H%M%S')";
                //return _marker.ToString() + s + _marker.ToString();

                return s;
            }
            else if (Type.GetType(type).Equals(typeof(Byte[])))
            {
                StringBuilder builder = new StringBuilder("0x");
                foreach (Byte b in (Byte[])columnValue)
                {
                    string tmp = Convert.ToString(b, 16);
                    if (tmp.Length < 2)
                        tmp = "0" + tmp;
                    builder.Append(tmp);
                }
                return builder.ToString();
            }
            else
            {
                return columnValue.ToString();
            }
        }

        public string checkDate(string date)
        {
            if (date.Length < 2)
                date = "0" + date;
            return date;
        }

        #endregion

    }

    public class WzdUtils
    {
#if VS90
        const string REGISTRYNAME = "infolight\\eep.net"; //2010中没有infolight\\eep.net2008，统一使用infolight\\eep.net
#else
        const string REGISTRYNAME = "infolight\\eep.net";
#endif

        public static void SetListViewSelect(ListView lv, bool isSelected, int selected)
        {
            for (int i = 0; i < lv.Items.Count; i++)
                if (i == selected)
                    lv.Items[i].Selected = isSelected;
                else
                    lv.Items[i].Selected = !isSelected;
        }

        static WzdUtils()
        {
            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
        }


        internal static string GetRegistryValueByKey(String keyName)
        {
            String registryValue = String.Empty;
            RegistryKey key;
            using (key = Registry.LocalMachine.OpenSubKey("SOFTWARE\\" + REGISTRYNAME, RegistryKeyPermissionCheck.ReadWriteSubTree))
            {
                if (key != null)
                {
                    registryValue = key.GetValue(keyName).ToString();
                }
            }
            return registryValue;
        }

        internal static void SetRegistryValueByKey(String keyName, String registryValue)
        {
            RegistryKey key;
            using (key = Registry.LocalMachine.OpenSubKey("SOFTWARE\\" + REGISTRYNAME, RegistryKeyPermissionCheck.ReadWriteSubTree))
            {
                if (key == null)
                    key = Registry.LocalMachine.CreateSubKey("SOFTWARE\\" + REGISTRYNAME);

                key.SetValue(keyName, registryValue);
            }
        }

        static private AddIn fAddIn;

        /// <summary>
        /// 废弃
        /// </summary>
        public static AddIn FAddIn
        {
            get { return WzdUtils.fAddIn; }
            set { WzdUtils.fAddIn = value; }
        }

        public static void Application_ThreadException(object sender, ThreadExceptionEventArgs args)
        {
            Exception e = (Exception)args.Exception;

            if (e is TargetInvocationException)
            {
                while (e.InnerException != null)
                {
                    e = e.InnerException;
                }
            }
            string serverstatck = e.InnerException == null ? string.Empty : e.InnerException.StackTrace;
            ErrorDialog fError = new ErrorDialog(e.Message, e.StackTrace, serverstatck);
            fError.ShowDialog();
            fError.Dispose();
        }

        static public string GetAddinsPath()
        {
            RegistryKey rk = Registry.LocalMachine.OpenSubKey("SOFTWARE\\" + REGISTRYNAME);
            if (rk != null)
            {
                string value = (string)rk.GetValue("Addins Path");
                rk.Close();
                if (value != null)
                {
                    value = value.TrimEnd('\\');
                    return value;
                }
            }
            return "";
        }
        static private XmlDocument GetServerPathXML(bool isOpenForm)
        {
            String strAddinPath = GetAddinsPath() + "\\ServerPath.xml";
            if (!File.Exists(strAddinPath))
            {
                if (isOpenForm)
                {
                    //fmSetServerPath fmssp = new fmSetServerPath(addIn);
                    //fmssp.ShowDialog();
                }
                else
                {
                    return null;
                }
            }
            XmlDocument x = new XmlDocument();
            x.Load(strAddinPath);

            return x;
        }

        static private XmlDocument GetServerPathXML(AddIn addIn, bool isOpenForm)
        {
            String strAddinPath = Path.GetDirectoryName(addIn.Object.GetType().Assembly.Location) + "\\ServerPath.xml";
            if (!File.Exists(strAddinPath))
            {
                if (isOpenForm)
                {
                    //fmSetServerPath fmssp = new fmSetServerPath(addIn);
                    //fmssp.ShowDialog();
                }
                else
                {
                    return null;
                }
            }
            XmlDocument x = new XmlDocument();
            x.Load(strAddinPath);

            return x;
        }

        static public String GetServerPath(AddIn addIn, bool isOpenForm)
        {
            String strReturn = String.Empty;
            if (addIn != null)
            {
                XmlDocument x = GetServerPathXML(addIn, isOpenForm);
                if (x != null && x.FirstChild.ChildNodes.Count > 0)
                    strReturn = x.FirstChild.ChildNodes[0].Attributes["Value"].Value;
            }
            else
            {
                XmlDocument x = GetServerPathXML(isOpenForm);
                if (x != null && x.FirstChild.ChildNodes.Count > 0)
                    strReturn = x.FirstChild.ChildNodes[0].Attributes["Value"].Value;

            }
            return strReturn;
        }

        static public String GetWebClientPath(AddIn addIn, bool isOpenForm)
        {
            String strReturn = String.Empty;
            XmlDocument x = GetServerPathXML(addIn, isOpenForm);
            if (x != null && x.FirstChild.ChildNodes.Count > 1)
                strReturn = x.FirstChild.ChildNodes[1].Attributes["Value"].Value;

            return strReturn;
        }

        static public String GetEEPAlias(AddIn addIn, bool isOpenForm)
        {
            String strReturn = String.Empty;
            XmlDocument x = GetServerPathXML(addIn, isOpenForm);
            if (x != null && x.FirstChild.ChildNodes.Count > 2)
                strReturn = x.FirstChild.ChildNodes[2].Attributes["Value"].Value;

            return strReturn;
        }

        static public List<String> GetCommandNamesByDataModuleName(String dataModuleName)
        {
            return DesignClientUtility.GetCommandNames(dataModuleName);
        }

        static public Dictionary<string, object> GetFieldsByEntityName(string assembly, string commandName, string entityTypeName)
        {
            return EFAssembly.EFClientToolsAssemblyAdapt.DesignClientUtility.GetEntityPropertiesTypes(assembly, commandName, entityTypeName);
        }

        //static public List<EFClientTools.EFServerReference.EntityObject> GetColumnDefination(string assembly, string commandName, string entityTypeName)
        //{
        //    string serverEntityClassName = EFAssembly.EFClientToolsAssemblyAdapt.EntityProvider.GetServerEntityClassName(assembly, entityTypeName);

        //    return EFAssembly.EFClientToolsAssemblyAdapt.DesignClientUtility.GetColumnDefination(assembly, commandName, serverEntityClassName);
        //}

        [Obsolete]
        static public Hashtable GetFieldsByEntityName(string entityTypeName)
        {
            return EFAssembly.EFClientToolsAssemblyAdapt.EntityProvider.GetEntityPropertiesTypes(entityTypeName);
        }

        static public List<string> GetDetailEntityNames(string strMasterEntityName)
        {
            return EFAssembly.EFClientToolsAssemblyAdapt.EntityProvider.GetDetailEntityClassNames(strMasterEntityName);
        }

        static public Dictionary<String, String> GetDetailEntityClassNameAndEntitySetName(string strMasterEntityName)
        {
            return EFAssembly.EFClientToolsAssemblyAdapt.EntityProvider.GetDetailEntityClassNameAndEntitySetName(strMasterEntityName);
        }

        static public List<object> GetAllDataByTableName(String tableName)
        {
            List<object> lists = new List<object>();
            foreach (var item in DesignClientUtility.GetAllDataByTableName(tableName))
            {
                lists.Add(item);
            }
            return lists;
        }

        static public void SaveDataToTable(List<object> lRefvals, String tableName)
        {
            DesignClientUtility.SaveDataToTable(lRefvals, tableName);
        }

        static public List<COLDEFInfo> GetColumnDefination(string assemblyName, string commandName, string entityTypeName, String loginDataBase)
        {
            //return EFAssembly.EFClientToolsAssemblyAdapt.DesignClientUtility.GetColumnDefination(assemblyName, commandName, entityTypeName);
            //string serverEntityClassName = EFAssembly.EFClientToolsAssemblyAdapt.EntityProvider.GetServerEntityClassName(assemblyName, entityTypeName);
            return DesignClientUtility.GetColumnDefination(assemblyName, commandName, entityTypeName, false, loginDataBase);
        }

        static public List<string> GetEntityPrimaryKeys(string assemblyName, string commandName, string entityTypeName)
        {
            return EFAssembly.EFClientToolsAssemblyAdapt.DesignClientUtility.GetEntityPrimaryKeys(assemblyName, commandName, entityTypeName);
            //return DesignClientUtility.GetEntityPrimaryKeys(assemblyName, commandName, entityTypeName);
        }

        static public String GetServerEntityClassName(String assembly, String entityTypeName)
        {
            string serverEntityClassName = EFAssembly.EFClientToolsAssemblyAdapt.EntityProvider.GetServerEntityClassName(assembly, entityTypeName);
            return serverEntityClassName;
        }

        static public List<String> GetEntityNavigationFields(String masterClassName)
        {
            List<String> entityNavigationFields = EFAssembly.EFClientToolsAssemblyAdapt.EntityProvider.GetEntityNavigationFields(masterClassName);
            return entityNavigationFields;
        }

        //[Obsolete]
        //static public List<string> GetDetailEntityNames(string assemblyName, string commandName, string masterClassName)
        //{
        //    return EFClientTools.DesignClientUtility.GetDetailEntityClassNames(assemblyName,commandName, masterClassName);
        //}

        static public IDbDataAdapter AllocateDataAdapter(ClientType ct)
        {
            IDbDataAdapter ob = null;
            try
            {
                if (ct == ClientType.ctMsSql)
                    ob = new SqlDataAdapter();
                else if (ct == ClientType.ctOleDB)
                    ob = new OleDbDataAdapter();
                else if (ct == ClientType.ctOracle)
                    ob = new OracleDataAdapter();
                else if (ct == ClientType.ctODBC)
                    ob = new OdbcDataAdapter();
                else if (ct == ClientType.ctMySql)
                {
                    String s = EEPRegistry.Server + "\\MySql.Data.dll";
                    Assembly assembly = Assembly.LoadFrom(s);
                    ob = assembly.CreateInstance("MySql.Data.MySqlClient.MySqlDataAdapter") as IDbDataAdapter;
                }
                else if (ct == ClientType.ctInformix)
                {
                    String s = EEPRegistry.Server + "\\IBM.Data.Informix.dll";
                    Assembly assembly = Assembly.LoadFrom(s);
                    ob = assembly.CreateInstance("IBM.Data.Informix.IfxDataAdapter") as IDbDataAdapter;
                }
                //else if (ct == ClientType.ctSybase)
                //{
                //    String s = EEPRegistry.Server + "\\Sybase.Data.AseClient.dll";
                //    Assembly assembly = Assembly.LoadFrom(s);
                //    ob = assembly.CreateInstance("Sybase.Data.AseClient.AseDataAdapter") as IDbDataAdapter;
                //}
            }
            catch (Exception e)
            {
                throw new Exception("Allocate DataAdapter Error: " + e.Message);
            }
            return ob;
        }

        public static void FillDataAdapter(ClientType ct, IDbDataAdapter da, DataTable table)
        {
            try
            {
                if (ct == ClientType.ctMsSql)
                    ((SqlDataAdapter)da).Fill(table);
                else if (ct == ClientType.ctOleDB)
                    ((OleDbDataAdapter)da).Fill(table);
                else if (ct == ClientType.ctOracle)
                    ((OracleDataAdapter)da).Fill(table);
                else if (ct == ClientType.ctODBC)
                    ((OdbcDataAdapter)da).Fill(table);
                else if (ct == ClientType.ctMySql)
                {
                    String s = EEPRegistry.Server + "\\MySql.Data.dll";
                    Assembly assembly = Assembly.LoadFrom(s);
                    Type t = assembly.GetType("MySql.Data.MySqlClient.MySqlDataAdapter");
                    MethodInfo temp = t.GetMethod("Fill", new Type[] { typeof(DataTable) });
                    temp.Invoke(da, new object[] { table });
                }
                else if (ct == ClientType.ctInformix)
                {
                    String s = EEPRegistry.Server + "\\IBM.Data.Informix.dll";
                    Assembly assembly = Assembly.LoadFrom(s);
                    Type t = assembly.GetType("IBM.Data.Informix.IfxDataAdapter");
                    MethodInfo temp = t.GetMethod("Fill", new Type[] { typeof(DataTable) });
                    temp.Invoke(da, new object[] { table });
                }
                //else if (ct == ClientType.ctSybase)
                //{
                //    String s = EEPRegistry.Server + "\\Sybase.Data.AseClient.dll";
                //    Assembly assembly = Assembly.LoadFrom(s);
                //    Type t = assembly.GetType("Sybase.Data.AseClient.AseDataAdapter");
                //    MethodInfo[] temp = t.GetMethods();
                //    temp[26].Invoke(da, new object[] { table });
                //}
            }
            catch (Exception e)
            {
                throw new Exception("Fill Dataset Error: " + e.Message);
            }
        }

        static public void FillDataAdapter(ClientType ct, IDbDataAdapter da, DataSet custDS, string sTable)
        {
            try
            {
                if (ct == ClientType.ctMsSql)
                    ((SqlDataAdapter)da).Fill(custDS, sTable);
                else if (ct == ClientType.ctOleDB)
                {
                    DataTable dt = new DataTable();
                    ((OleDbDataAdapter)da).Fill(dt);
                    dt.TableName = sTable;
                    custDS.Tables.Add(dt);
                    //((OleDbDataAdapter)da).Fill(custDS, sTable);
                }
                else if (ct == ClientType.ctOracle)
                    ((OracleDataAdapter)da).Fill(custDS, sTable);
                else if (ct == ClientType.ctODBC)
                    ((OdbcDataAdapter)da).Fill(custDS, sTable);
                else if (ct == ClientType.ctMySql)
                {
                    String s = EEPRegistry.Server + "\\MySql.Data.dll";
                    Assembly assembly = Assembly.LoadFrom(s);
                    Type t = assembly.GetType("MySql.Data.MySqlClient.MySqlDataAdapter");
                    MethodInfo temp = t.GetMethod("Fill", new Type[] { typeof(DataSet), typeof(String) });
                    temp.Invoke(da, new object[] { custDS, sTable });
                    //((MySqlDataAdapter)da).Fill(custDS, sTable);
                }
                else if (ct == ClientType.ctInformix)
                {
                    String s = EEPRegistry.Server + "\\IBM.Data.Informix.dll";
                    Assembly assembly = Assembly.LoadFrom(s);
                    Type t = assembly.GetType("IBM.Data.Informix.IfxDataAdapter");
                    MethodInfo temp = t.GetMethod("Fill", new Type[] { typeof(DataSet), typeof(String) });
                    temp.Invoke(da, new object[] { custDS, sTable });
                }
                //else if (ct == ClientType.ctSybase)
                //{
                //    String s = EEPRegistry.Server + "\\Sybase.Data.AseClient.dll";
                //    Assembly assembly = Assembly.LoadFrom(s);
                //    Type t = assembly.GetType("Sybase.Data.AseClient.AseDataAdapter");
                //    MethodInfo[] temp = t.GetMethods();
                //    temp[26].Invoke(da, new object[] { custDS, sTable });
                //}
            }
            catch (Exception e)
            {
                throw new Exception("Fill Dataset Error: " + e.Message);
            }
        }

        static public DbConnection AllocateConnection(String DBAlias, ClientType aType, bool bGetSysDB)
        {
            DBAlias = bGetSysDB ? GetSplitSysDB(DBAlias) : DBAlias;
            Srvtools.DbConnectionSet.DbConnection dbc = DbConnectionSet.GetDbConn(DBAlias);

            //String ConnectionString = GetConntionString(DBAlias);
            //DbConnection Result = null;
            //try
            //{
            //    if (aType == ClientType.ctMsSql)
            //        Result = new SqlConnection(ConnectionString);
            //    else if (aType == ClientType.ctOleDB)
            //        Result = new OleDbConnection(ConnectionString);
            //    else if (aType == ClientType.ctOracle)
            //        Result = new OracleConnection(ConnectionString);
            //    else if (aType == ClientType.ctODBC)
            //        Result = new OdbcConnection(ConnectionString);
            //    else if (aType == ClientType.ctMySql)
            //    {
            //        String s = EEPRegistry.Server + "\\MySql.Data.dll";
            //        Assembly assembly = Assembly.LoadFrom(s);
            //        Result = assembly.CreateInstance("MySql.Data.MySqlClient.MySqlConnection") as DbConnection;
            //        Result.ConnectionString = ConnectionString;
            //    }
            //    else if (aType == ClientType.ctInformix)
            //    {
            //        String s = EEPRegistry.Server + "\\IBM.Data.Informix.dll";
            //        Assembly assembly = Assembly.LoadFrom(s);
            //        Result = assembly.CreateInstance("IBM.Data.Informix.IfxConnection") as DbConnection;
            //        Result.ConnectionString = ConnectionString;
            //    }

            //}
            //catch (Exception e)
            //{
            //    MessageBox.Show(e.Message);
            //    throw new Exception("Allocate Connection Error: " + e.Message);
            //}
            return dbc.CreateConnection() as DbConnection;
        }

        static private string GetSplitSysDB(String sDB)
        {
            String s = SystemFile.DBFile;

            if (File.Exists(s))
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(s);
                XmlNode node = xml.SelectSingleNode(string.Format("InfolightDB/DataBase/{0}", sDB));
                if (node != null)
                {
                    if (node.Attributes["Master"] != null && node.Attributes["Master"].Value.Trim() == "1")
                    {
                        XmlNode nodesys = xml.SelectSingleNode("InfolightDB/SystemDB");
                        if (nodesys != null)
                        {
                            string sysdb = nodesys.InnerText.Trim();
                            XmlNode nodecheck = xml.SelectSingleNode(string.Format("InfolightDB/DataBase/{0}", sysdb));
                            if (nodecheck != null)
                            {
                                return sysdb;
                            }
                            else
                            {
                                throw new Exception("SystemDB does not exsit in db list");
                            }
                        }
                        else
                        {
                            throw new Exception("SystemDB is Empty");
                        }
                    }
                    else
                    {
                        return sDB;
                    }
                }
                else
                {
                    throw new Exception(string.Format("EEPAlias:{0} does not exsit", sDB));
                }
            }
            else
            {
                throw new Exception(string.Format("{0} does not exsit", s));
            }
        }

        public static String GetConntionString(String dataBaseName)
        {
            String connString = String.Empty;
            if (File.Exists(SystemFile.DBFile))
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(SystemFile.DBFile);
                XmlNode node = xml.SelectSingleNode(string.Format("InfolightDB/DataBase/{0}", dataBaseName));
                if (node != null)
                {
                    connString = node.Attributes["String"].InnerText;
                    if (connString.Length > 0)
                    {
                        connString = connString.TrimEnd(';');
                        if (GetPwdString(node.Attributes["Password"].InnerText) != String.Empty)
                            connString = connString + ";Password=" + GetPwdString(node.Attributes["Password"].InnerText);
                    }
                }
            }
            return connString;
        }

        private static String _quotePrefix = "[";
        private static String _quoteSuffix = "]";
        public static String Quote(String table_or_column, IDbConnection conn)
        {
            if (conn is SqlConnection)
            {
                if (_quotePrefix == null || _quoteSuffix == null)
                    return table_or_column;
                return _quotePrefix + table_or_column + _quoteSuffix;
            }
            else if (conn is OracleConnection)
            {
                return table_or_column;
            }
            else if (conn is OdbcConnection)
            {
                return table_or_column;
            }
            else if (conn is OleDbConnection)
            {
                return table_or_column;
            }
            else if (conn.GetType().Name == "MySqlConnection")
            {
                return table_or_column;
            }
            else if (conn.GetType().Name == "IfxConnection")
            {
                return table_or_column;
            }
            return _quotePrefix + table_or_column + _quoteSuffix;
        }

        public static string GetPwdString(string s)
        {
            string sRet = "";

            for (int i = 0; i < s.Length; i++)
            {
                sRet = sRet + (char)(((int)(s[s.Length - 1 - i])) ^ s.Length);
            }
            return sRet;
        }

        /*
        public static string GetPwdString(string s)
        {
            string text1 = "";
            for (int num1 = 0; num1 < s.Length; num1++)
            {
                text1 = text1 + (s[(s.Length - 1) - num1] ^ s.Length);
            }
            return text1;
        }
         */

        public static XmlNode FindNode(XmlDocument Doc, XmlNode ParentNode, string NodeName)
        {
            XmlNode Result = null;
            int I;
            if (ParentNode == null)
            {
                for (I = 0; I < Doc.ChildNodes.Count; I++)
                {
                    if (string.Compare(Doc.ChildNodes[I].Name, NodeName) == 0)
                    {
                        Result = Doc.ChildNodes[I];
                        break;
                    }
                }
            }
            else
            {
                for (I = 0; I < ParentNode.ChildNodes.Count; I++)
                {
                    if (string.Compare(ParentNode.ChildNodes[I].Name, NodeName) == 0)
                    {
                        Result = ParentNode.ChildNodes[I];
                        break;
                    }
                }
            }
            return Result;
        }

        public static string RemoveSpace(string SrcString)
        {
            string Result = "";
            for (int I = 0; I < SrcString.Length; I++)
            {
                if (SrcString[I].ToString() != " ")
                    Result = Result + SrcString[I];
            }
            return Result;
        }

        public static String RemoveQuote(String value, ClientType ct)
        {
            String rtn = value;

            return rtn;
        }

        public static String GetToken(ref String AString, char[] Fmt)
        {
            String Result = "";
            while (AString.Length != 0 && AString[0] == ' ')
            {
                AString = AString.Remove(0, 1);
            }

            if (AString.Length == 0)
                return Result;

            Boolean Found = false;
            int I = 0;
            while (I < AString.Length)
            {
                Found = false;
                if ((byte)AString[I] <= 128)
                {
                    foreach (char C in Fmt)
                    {
                        if (AString[I] == C)
                        {
                            Found = true;
                            break;
                        }
                    }
                    if (!Found)
                        I++;
                }
                else
                {
                    I = I + 2;
                }
                if (Found)
                    break;
            }

            if (Found)
            {
                Result = AString.Substring(0, I);
                AString = AString.Remove(0, I + 1);
            }
            else
            {
                Result = AString;
                AString = "";
            }

            return Result;
        }

        public static String GetFieldParam(String Source, String PropName)
        {
            String Result = "";
            String TempParam, S1, S2;
            Char AChar;
            if (Source == "" || Source == null)
                return "";
            AChar = Source[0];
            if (AChar == ';')
            {
                TempParam = Source.Substring(1, Source.Length - 1);
            }
            else
            {
                TempParam = Source;
            }

            while (TempParam != "")
            {
                S1 = GetToken(ref TempParam, new Char[] { ';' });
                if (S1 == "")
                    break;
                S2 = GetToken(ref S1, new Char[] { '=' });
                S2 = S2.Trim();
                if (string.Compare(S2, PropName, true) != 0)
                    continue;
                Result = S1;
                break;
            }
            return Result;
        }

        public static String FixupToVSWebSiteName(String PackageName)
        {
            int I = 0;
            String S, S1, LastFolder, Result = "";
            S1 = PackageName;
            S = WzdUtils.GetToken(ref PackageName, new char[] { '\\' });
            LastFolder = PackageName;
            while (S != "")
            {
                I++;
                LastFolder = S;
                S = WzdUtils.GetToken(ref PackageName, new char[] { '\\' });
            }
            if (I > 3)
            {
                S = System.IO.Directory.GetDirectoryRoot(S1);
                Result = S + @"...\" + LastFolder + @"\";
            }
            return Result;
        }

        public static List<String> GetAllTablesList(DbConnection dbConn, ClientType dbType)
        {
            List<String> tablesList = new List<string>();
            String sQL = "";
            if (dbType == ClientType.ctInformix)
                sQL = "select * from SYSTABLES where (TABTYPE = 'T' or TABTYPE = 'V') and TABID >= 100 order by TABNAME";

            IDbCommand cmd = dbConn.CreateCommand();
            cmd.CommandText = sQL;
            if (dbConn.State == ConnectionState.Closed)
            { dbConn.Open(); }

            IDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (reader.Read())
            {
                tablesList.Add(reader[0].ToString());
            }
            reader.Close();
            return tablesList;
        }

        public static void SelectedListViewItemUp(ListView aListView)
        {
            if (aListView.SelectedItems.Count > 0)
            {
                aListView.BeginUpdate();
                if (aListView.SelectedItems[0].Index > 0)
                {
                    foreach (ListViewItem lvi in aListView.SelectedItems)
                    {
                        ListViewItem lviSelectedItem = lvi;
                        int indexSelectedItem = lvi.Index;
                        aListView.Items.RemoveAt(indexSelectedItem);
                        aListView.Items.Insert(indexSelectedItem - 1, lviSelectedItem);
                    }
                }
                aListView.EndUpdate();

                aListView.Focus();
                aListView.SelectedItems[0].Focused = true;
                aListView.SelectedItems[0].EnsureVisible();
            }
        }

        public static void SelectedListViewItemDown(ListView aListView)
        {
            if (aListView.SelectedItems.Count > 0)
            {
                aListView.BeginUpdate();
                int indexMaxSelectedItem = aListView.SelectedItems[aListView.SelectedItems.Count - 1].Index;

                if (indexMaxSelectedItem < aListView.Items.Count - 1)
                {
                    for (int i = aListView.SelectedItems.Count - 1; i >= 0; i--)
                    {
                        ListViewItem lviSelectedItem = aListView.SelectedItems[i];
                        int indexSelectedItem = lviSelectedItem.Index;
                        aListView.Items.RemoveAt(indexSelectedItem);
                        aListView.Items.Insert(indexSelectedItem + 1, lviSelectedItem);
                    }
                }
                aListView.EndUpdate();

                aListView.Focus();
                aListView.SelectedItems[aListView.SelectedItems.Count - 1].Focused = true;
                aListView.SelectedItems[aListView.SelectedItems.Count - 1].EnsureVisible();
            }
        }
    }
    public class EFAssembly : IDisposable
    {
        private static byte[] buffer = null;

        public static Assembly LoadAssembly(string assemblyPath)
        {
            Assembly assembly = null;


            try
            {
                //if (WzdUtils.FAddIn == null)
                //{
                //    ArgumentNullException exception = new ArgumentNullException("WzdUtils.FAddIn");

                //    WzdUtils.Application_ThreadException(null, new ThreadExceptionEventArgs(exception));
                //}
                //else
                //{
                    string path = WzdUtils.GetServerPath(WzdUtils.FAddIn, true);
                    path = path.Remove(path.LastIndexOf('\\')) + assemblyPath;
                    String fullDllName = path;
                    buffer = System.IO.File.ReadAllBytes(fullDllName);
                    assembly = Assembly.Load(buffer);
                //}
            }
            catch (Exception ex)
            {
                WzdUtils.Application_ThreadException(null, new ThreadExceptionEventArgs(ex));
            }

            return assembly;
        }

        //private static AddIn vAddIn;

        //public static AddIn VAddIn
        //{
        //    get { return EFAssembly.vAddIn; }
        //    set { EFAssembly.vAddIn = value; }
        //}

        public void Dispose()
        {
            buffer = null;
        }

        public class EFClientToolsAssemblyAdapt
        {
            private const string EFCLIENTTOOLSPATH = "\\EFClientTools\\bin\\Debug\\";
            private const string EFCLIENTTOOLSDLL = "EFClientTools.dll";
            private const string REMOTENAMEEDITORDIALOGTYPE = "EFClientTools.Editor.RemoteNameEditorDialog";
            private const string CLIENTENTITYPROVIDER = "EFClientTools.Common.EntityProvider";
            private const string DESIGNCLIENTUTILITY = "EFClientTools.DesignClientUtility";

            private static Assembly _EFClientToolsAssembly;
            public static Assembly EFClientToolsAssembly
            {
                get
                {
                    _EFClientToolsAssembly = LoadEFClientTools();
                    return _EFClientToolsAssembly;
                }
                set { _EFClientToolsAssembly = value; }
            }

            public static Assembly LoadEFClientTools()
            {
                return LoadAssembly(EFCLIENTTOOLSPATH + EFCLIENTTOOLSDLL);
            }

            public class RemoteNameEditorDialog
            {
                public RemoteNameEditorDialog(string remoteName)
                {
                    remoteNameEditorDialogType = EFClientToolsAssembly.GetType(REMOTENAMEEDITORDIALOGTYPE);
                    remoteNameEditorDialogObject = Activator.CreateInstance(RemoteNameEditorDialogType, new object[] { remoteName });
                }

                public RemoteNameEditorDialog()
                    : this(string.Empty)
                {

                }

                private Type remoteNameEditorDialogType;
                public Type RemoteNameEditorDialogType
                {
                    get { return remoteNameEditorDialogType; }
                }

                private object remoteNameEditorDialogObject;
                private object RemoteNameEditorDialogObject
                {
                    get { return remoteNameEditorDialogObject; }
                }

                public Form RemoteNameEditorDialogForm
                {
                    get { return (Form)RemoteNameEditorDialogObject; }
                }

                private object GetValue(string propertyName)
                {
                    return RemoteNameEditorDialogObject.GetType().GetProperty(propertyName).GetValue(RemoteNameEditorDialogObject, null);
                }

                public string ReturnValue
                {
                    get { return GetValue("ReturnValue").ToString(); }
                }

                public string SelectedCommandName
                {
                    get { return GetValue("SelectedCommandName").ToString(); }
                }

                public string ReturnClassName
                {
                    get { return GetValue("ReturnClassName").ToString(); }
                }

                public string EntitySetName
                {
                    get { return GetValue("EntitySetName").ToString(); }
                }
            }

            public static class EntityProvider
            {
                static EntityProvider()
                {
                    entityProviderObject = Activator.CreateInstance(EFClientToolsAssembly.GetType(CLIENTENTITYPROVIDER));
                }

                private static object entityProviderObject;
                public static object EntityProviderObject
                {
                    get { return entityProviderObject; }
                }

                private static object InvokeMethod(string methodName, object[] parameters)
                {
                    return EntityProviderObject.GetType().GetMethod(methodName).Invoke(null, parameters);
                }

                [Obsolete]
                public static Hashtable GetEntityPropertiesTypes(string entityClassName)
                {
                    return InvokeMethod("GetEntityPropertiesTypes", new object[] { entityClassName }) as Hashtable;
                }

                public static List<string> GetDetailEntityClassNames(string masterClassName)
                {
                    return InvokeMethod("GetDetailEntityClassNames", new object[] { masterClassName }) as List<string>;
                }

                public static Dictionary<String, String> GetDetailEntityClassNameAndEntitySetName(string masterClassName)
                {
                    return InvokeMethod("GetDetailEntityClassNameAndEntitySetName", new object[] { masterClassName }) as Dictionary<String, String>;
                }

                public static List<string> GetEntityNavigationFields(string masterClassName)
                {
                    return InvokeMethod("GetEntityNavigationFields", new object[] { masterClassName }) as List<string>;
                }

                public static List<string> GetClientEntityProperties(EFDataSource eds, Assembly assembly)
                {
                    return InvokeMethod("GetClientEntityProperties", new object[] { eds, assembly }) as List<string>;
                }

                public static string GetServerEntityClassName(string assemblyName, string clientEntityClassName)
                {
                    return InvokeMethod("GetServerEntityClassName", new object[] { assemblyName, clientEntityClassName }) as string;
                }
            }

            public static class DesignClientUtility
            {
                static DesignClientUtility()
                {
                    designClientUtilityObject = Activator.CreateInstance(EFClientToolsAssembly.GetType(DESIGNCLIENTUTILITY));
                }

                private static object designClientUtilityObject;
                public static object DesignClientUtilityObject
                {
                    get { return designClientUtilityObject; }
                }

                private static object InvokeMethod(string methodName, object[] parameters)
                {
                    return DesignClientUtilityObject.GetType().GetMethod(methodName).Invoke(null, parameters);
                }

                public static List<string> GetCommandNames(string assemblyName)
                {
                    return InvokeMethod("GetCommandNames", new object[] { assemblyName }) as List<string>;
                }

                public static List<string> GetModuleNames()
                {
                    return InvokeMethod("GetModuleNames", null) as List<string>;
                }

                [Obsolete]
                public static List<COLDEFInfo> GetColumnDefination(string assemblyName, string commandName, string entityTypeName)
                {
                    return InvokeMethod("GetColumnDefination", new object[] { assemblyName, commandName, entityTypeName }) as List<COLDEFInfo>;
                }

                public static Dictionary<string, object> GetEntityPropertiesTypes(string assemblyName, string commandName, string entityTypeName)
                {
                    return InvokeMethod("GetEntityPropertiesTypes", new object[] { assemblyName, commandName, entityTypeName }) as Dictionary<string, object>;
                }

                public static Dictionary<string, string> GetEntityPropertieMappings(string assemblyName, string commandName, string entityTypeName)
                {
                    return InvokeMethod("GetEntityPropertieMappings", new object[] { assemblyName, commandName, entityTypeName }) as Dictionary<string, string>;
                }

                public static List<string> GetEntityPrimaryKeys(string assemblyName, string commandName, string clientEntityClassName)
                {
                    return InvokeMethod("GetEntityPrimaryKeys", new object[] { assemblyName, commandName, clientEntityClassName }) as List<string>;
                }

                public static List<EntityObject> GetAllDataByTableName(String tableName)
                {
                    return InvokeMethod("GetAllDataByTableName", new object[] { tableName }) as List<EntityObject>;
                }

                public static List<EntityObject> GetAllDataByTableName(String dbAlias, String tableName)
                {
                    return InvokeMethod("GetAllDataByTableName", new object[] { dbAlias, tableName }) as List<EntityObject>;
                }
            }
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
    }
    public class TBlockItem : TCollectionItem //System.Collections.CollectionBase, IList, ICollection
    {
        private String FName, FTableName, FProviderName, FRelationName, FParentItemName = "";
        private TBlockFieldItems FBlockFieldItems;
        private InfoBindingSource FBindingSource;
        private InfoBindingSource FViewBindingSource;
        private Srvtools.WebDataSource FWebDataSource;
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

        public Srvtools.WebDataSource wDataSource
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
        private fmMain FOwner;
        private bool FNewSolution = false;
        private string FCodeOutputPath;
        private int FColumnCount;
        private ClientType FDatabaseType;
        private String FConnString;
        private String FLanguage = "cs";
        private String FLabelAliement;

        public TClientData(fmMain Owner)
        {
            FOwner = Owner;
            FBlocks = new TBlockItems(this);
        }

        public fmMain Owner
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
        private fmMain FOwner;
        private bool FNewSolution = false;
        private string FConnectionString;
        private string FCodeOutputPath;
        private ClientType FDatabaseType;
        private String FLanguage = "cs";

        public TServerData(fmMain Owner)
        {
            FDatasetCollection = new TDatasetCollection(this);
            FOwner = Owner;
        }

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

        public fmMain Owner
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

                    aInfoCommand.CommandText = "Select TABLE_NAME, CAPTION from COLDEF where FIELD_NAME = '' or FIELD_NAME is null order by TABLE_NAME";
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
    public class TFieldSchemaItem : TFieldAttrItem
    {
        string FDataType, FCaption;
        bool FAllowDBNull;
        int FLength, FScale;

        public bool AllowDBNull
        {
            get
            {
                return FAllowDBNull;
            }
            set
            {
                FAllowDBNull = value;
            }
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
        public int Scale
        {
            get
            {
                return FScale;
            }
            set
            {
                FScale = value;
            }
        }

        public string DataType
        {
            get
            {
                return FDataType;
            }
            set
            {
                FDataType = value;
            }
        }
        public string Caption
        {
            get
            {
                return FCaption;
            }
            set
            {
                FCaption = value;
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
                    if (TableName.IndexOf('.') > -1)
                    {
                        Owner = WzdUtils.GetToken(ref SS, new char[] { '.' });
                        TableName = SS;
                    }
                    aInfoCommand.CommandText = "Select FIELD_NAME,CAPTION from COLDEF where TABLE_NAME='" + TableName + "' OR TABLE_NAME='" + Owner + "." + TableName + "'";
                    IDbDataAdapter DA = DBUtils.CreateDbDataAdapter(aInfoCommand);
                    DataSet D = new DataSet();
                    WzdUtils.FillDataAdapter(DatabaseType, DA, D, "COLDEF");

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
}
