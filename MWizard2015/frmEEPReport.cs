using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using EnvDTE80;
using EnvDTE;
using System.ComponentModel.Design;
using System.Xml;
using System.Collections;
using Srvtools;
using System.IO;
using System.Runtime.InteropServices;
//using Microsoft.Reporting.WinForms;
#if VS90
using WebDevPage = Microsoft.VisualWebDeveloper.Interop.WebDeveloperPage;
using System.Reflection;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
#endif


namespace MWizard2015
{
    public partial class frmEEPReport : Form
    {
        #region WebFormDefine
        private InfoDataSet FInfoDataSet = null;
        private string[] FProviderNameList;        
        private System.Web.UI.Page FPage;        
        private ProjectItem webformDir;
        private InfoDataSet WizardDataSet;
        private InfoDataSet WizardDetailDataSet;
        const string INFOLIGHTMARK = "Infolight";
        #endregion       

        #region WinFormDefine
        private string FTemplatePath;
        private Project GlobalProject;
        private System.Windows.Forms.Form FRootForm;
        private ProjectItem GlobalPI;
        private Window GlobalWindow;
        #endregion
        private bool GenSuccess = true;
        private System.ComponentModel.Design.IDesignerHost FDesignerHost;
        private Window FDesignWindow;
        private WebDevPage.DesignerDocument FDesignerDocument;

        public frmEEPReport(DTE2 dte2, AddIn addIn, bool isWebReport)
        {
            InitializeComponent();
            _dte2 = dte2;
            _addIn = addIn;
            _isWebReport = isWebReport;            
        }        

        private DTE2 _dte2;
        private AddIn _addIn;
        private bool _isWebReport;
        private DataSet ds = null;

        private void frmEEPReport_Load(object sender, EventArgs e)
        {
            setWizardStep(0);
        }

        private void CreateData()
        {
            this.tvDataSelect.Nodes.Clear();
            this.tvGroupSelect.Nodes.Clear();
            GenDataSet();
            if (ds.Tables.Count > 0)
            {
                this.panel2.Visible = true;
                foreach (DataTable tab in ds.Tables)
                {
                    System.Windows.Forms.TreeNode nTab = this.tvDataSelect.Nodes.Add(tab.TableName, tab.TableName);
                    foreach (DataColumn col in tab.Columns)
                    {
                        nTab.Nodes.Add(col.ColumnName, col.ColumnName);
                    }
                }
            }
        }

        private void GenDataSet()
        {
            ds = new DataSet();
            string dataDir = "";
            if (_isWebReport)
            {
                dataDir = ReportCreator.GetWebClientPath() + this.cmbRootName.Text + "\\";
                ds.ReadXmlSchema(dataDir + this.cmbXSDName.Text);
            }
            else
            {
                //dataDir = ReportCreator.GetWinClientPath(this.cmbProName.Text);
                dataDir = this.tbOutputPath.Text + "\\" + this.tbPackageName.Text + "\\";
                ds.ReadXmlSchema(dataDir + this.cmbWinXSDName.Text);
            }
        }

        private void chkIsMasterDetails_CheckedChanged(object sender, EventArgs e)
        {
            this.txtDetailsRptFileName.Enabled = this.chkIsMasterDetails.Checked;
            if (!this.chkIsMasterDetails.Checked)
            {
                this.txtDetailsRptFileName.Text = string.Empty;
                foreach(System.Windows.Forms.TreeNode node in this.tvDataSelect.Nodes)
                {
                    if (node.Index > 0)
                    {
                        if (node.Checked)
                        {
                            node.Checked = false;
                            continue;
                        }
                        foreach (System.Windows.Forms.TreeNode subNode in node.Nodes)
                        {
                            if (subNode.Checked)
                                subNode.Checked = false;
                        }
                    }
                }
            }
        }

        private void cmbMasterReportStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cmbMasterReportStyle.SelectedIndex == 0)
            {
                this.numLayoutColumn.Enabled = true;
            }
            else
            {
                this.numLayoutColumn.Enabled = false;
            }
        }

        private void tvDataSelect_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Level == 0)
            {
                System.Windows.Forms.TreeNode[] nodes = this.tvGroupSelect.Nodes.Find(e.Node.Text, false);
                if (nodes.Length > 0)
                {
                    System.Windows.Forms.TreeNode node = nodes[0];
                    this.tvGroupSelect.Nodes.Remove(node);
                }
                if (e.Node.Checked)
                {
                    this.tvGroupSelect.Nodes.Add(e.Node.Text, e.Node.Text);
                }
                else
                {
                    this.tvGroupSelect.Nodes.RemoveByKey(e.Node.Text);
                }
                foreach (System.Windows.Forms.TreeNode node in e.Node.Nodes)
                {
                    node.Checked = e.Node.Checked;
                }
            }
            else if(e.Node.Level == 1)
            {
                System.Windows.Forms.TreeNode[] nodes = this.tvGroupSelect.Nodes.Find(e.Node.Parent.Text, false);
                if (nodes.Length > 0)
                {
                    System.Windows.Forms.TreeNode node = nodes[0];
                    if (e.Node.Checked)
                    {
                        node.Nodes.Add(e.Node.Text, e.Node.Text);
                    }
                    else
                    {
                        node.Nodes.RemoveByKey(e.Node.Text);
                        bool hasBrotherChecked = false;
                        foreach (System.Windows.Forms.TreeNode anode in e.Node.Parent.Nodes)
                        {
                            if (anode.Checked)
                            {
                                hasBrotherChecked = true;
                                break;
                            }
                        }
                        if (!hasBrotherChecked)
                            this.tvGroupSelect.Nodes.RemoveByKey(e.Node.Parent.Text);
                    }
                }
                else
                {
                    if (e.Node.Checked)
                    {
                        System.Windows.Forms.TreeNode node = this.tvGroupSelect.Nodes.Add(e.Node.Parent.Text, e.Node.Parent.Text);
                        node.Nodes.Add(e.Node.Text, e.Node.Text);
                    }
                }
            }
        }

        private void tvDataSelect_BeforeCheck(object sender, TreeViewCancelEventArgs e)
        {
            if (!e.Node.Checked)
            {
                if ((e.Node.Index > 0 && e.Node.Level == 0) || (e.Node.Level > 0 && e.Node.Parent.Index > 0))
                {
                    if (!this.chkIsMasterDetails.Checked)
                    {
                        MessageBox.Show("Before you selct two or more tables as datasource, you must confirm that the 'master-details' checkbox has been already checked!");
                        e.Cancel = true;
                    }
                }
            }
        }

        private void tvGroupSelect_BeforeCheck(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Level == 0)
            {
                e.Cancel = true;
            }
            else if (e.Node.Level == 1)
            {
                if (!e.Node.Checked && cmbMasterReportStyle.SelectedIndex == 1)
                {
                    foreach (System.Windows.Forms.TreeNode node in e.Node.Parent.Nodes)
                    {
                        if (node.Checked)
                        {
                            e.Cancel = true;
                        }
                    }
                }
            }
        }

        private Project WebClientProject()
        {
            Solution2 sln = (Solution2)_dte2.Solution;
            if (sln != null)
            {
                return ReportCreator.FindProject(sln, ReportCreator.GetWebClientPath());
            }
            return null;
        }

        private Project WinClientProject(string name)
        {
            Solution2 sln = (Solution2)_dte2.Solution;
            if (sln != null)
            {
                return ReportCreator.FindProject(sln, name);
            }
            return null;
        }


        private void btnDone_Click(object sender, EventArgs e)
        {
            if (currentStep == 2)
            {                
                //產生Client Project
                if (checkStepCompleted(currentStep))
                {
                    if (this._isWebReport)
                    {
                        CreateWebClient();
                    }
                    else
                    {
                        CreateWinClient();
                    }
                    if (GenSuccess)
                    {
                        setWizardStep(currentStep + 1);
                    }
                }
            }
            else
            {
                ReportParameter rptParams = GetRptParams();
                if (this._isWebReport)
                {
                    Project proj = WebClientProject();
                    if (proj != null)
                    {
                        ProjectItem reportDir = ReportCreator.FindProjectItem(proj, rptParams.RptRootName);
                        if (reportDir == null)
                        {
                            //为Report创建文件夹
                            reportDir = ReportCreator.CreateProjectItem(proj, rptParams.RptRootName, 0);
                        }
                        ProjectItem piDataSet = ReportCreator.FindProjectItem(reportDir, rptParams.RptXSDFile);
                        if (piDataSet != null)
                        {
                            List<string> prtFileNames = new List<string>(new string[] { this.txtRptFileName.Text });
                            List<string> RealTableName = new List<string>(new string[] { this.tbTableNameF.Text });
                            if (this.chkIsMasterDetails.Checked)
                            {
                                prtFileNames.Add(this.txtDetailsRptFileName.Text);
                                RealTableName.Add(this.tbRealChildTableName.Text.Split(',')[0]);
                            }
                            ReportCreator.GenWebReportFiles(reportDir, piDataSet, rptParams, prtFileNames, RealTableName);
                            WebClientCreator.GenReportViewProperty(FDesignWindow, reportDir, chkIsMasterDetails.Checked, cmbRootName.SelectedItem.ToString(), txtRptFileName.Text);
                        }
                    }
                }
                else
                {
                    Project proj = WinClientProject(this.cmbProName.Text);
                    ProjectItem piDataSet = ReportCreator.FindProjectItem(proj, rptParams.RptXSDFile);
                    if (piDataSet != null)
                    {
                        String lan = GetLanguage();
                        List<string> prtFileNames = new List<string>(new string[] { this.txtRptFileName.Text });
                        List<string> RealTableName = new List<string>(new string[] { this.tbTableNameF.Text });
                        if (this.chkIsMasterDetails.Checked)
                        {
                            prtFileNames.Add(this.txtDetailsRptFileName.Text);
                            RealTableName.Add(this.tbRealChildTableName.Text.Split(',')[0]);
                        }
                        ReportCreator.GenWinReportFiles(proj, piDataSet, rptParams, prtFileNames, RealTableName);
                        WinClientCreator.GetReportViewProperty(FRootForm, proj, chkIsMasterDetails.Checked, tbTableName.Text, tbPackageName.Text, tbFormName.Text, txtRptFileName.Text, tbChildTableName.Text, lan);
                        GlobalProject.Save(GlobalProject.FullName);
                        Solution sln = _dte2.Solution;
                        sln.Remove(GlobalProject);
                        string FilePath = tbOutputPath.Text + "\\" + tbPackageName.Text;
                        Project P = sln.AddFromFile(FilePath + "\\" + tbPackageName.Text + lan + "proj", false);
                        if (lan != ".vb")
                            P.Properties.Item("RootNamespace").Value = tbPackageName.Text;
                        sln.SaveAs(sln.FileName);
                        sln.SolutionBuild.StartupProjects = P;
                        sln.SolutionBuild.BuildProject(sln.SolutionBuild.ActiveConfiguration.Name, P.FullName, true);
                        GlobalProject = P;
                        _dte2.Solution.SolutionBuild.BuildProject(_dte2.Solution.SolutionBuild.ActiveConfiguration.Name,
                            GlobalProject.FullName, true);
                        foreach (ProjectItem PI in GlobalProject.ProjectItems)
                        {
                            if (PI.Name == tbFormName.Text + lan)
                            {
                                Window W = PI.Open("{00000000-0000-0000-0000-000000000000}");
                                W.Activate();
                                FDesignerHost = (IDesignerHost)W.Object;
                            }
                        }
                    }
                }
                this.Close();
            }
        }

        private String GetLanguage()
        {
            if (this.cbChooseLanguage.Text == String.Empty || this.cbChooseLanguage.Text == "C#")
                return ".cs";
            else if (this.cbChooseLanguage.Text == "VB")
                return ".vb";
            return String.Empty;
        }

        private ReportParameter GetRptParams()
        {
            ReportParameter rptParams = new ReportParameter();
            string dsName = "NewDataSet";
            if (_isWebReport)
            {
                rptParams.RptRootName = this.cmbRootName.Text;
                rptParams.RptXSDFile = this.cmbXSDName.Text;
                if (this.cmbXSDName.Text.IndexOf(".xsd") != -1)
                    dsName = this.cmbXSDName.Text.Substring(0, this.cmbXSDName.Text.IndexOf(".xsd"));
            }
            else
            {
                rptParams.RptProjName = this.cmbProName.Text;
                rptParams.RptXSDFile = this.cmbWinXSDName.Text;
                if (this.cmbWinXSDName.Text.IndexOf(".xsd") != -1)
                    dsName = this.cmbWinXSDName.Text.Substring(0, this.cmbWinXSDName.Text.IndexOf(".xsd"));
            }
            rptParams.RptFileNames = new string[] { this.txtRptFileName.Text, this.txtDetailsRptFileName.Text };
            rptParams.IsMasterDetails = this.chkIsMasterDetails.Checked;
            rptParams.RptStyle = (EEPReportStyle)this.cmbMasterReportStyle.SelectedIndex;
            rptParams.LayoutColumnNum = (int)this.numLayoutColumn.Value;
            rptParams.SelectAlias = this.cmbSelectAlias.Text;
            rptParams.ClientType = (ClientType)this.cmbDataBaseType.SelectedIndex;
            rptParams.RptSet = new ReportSet(dsName);
            rptParams.HorGaps = this.tbHorGaps.Text;
            rptParams.VertGaps = this.tbVertGaps.Text;

            foreach (System.Windows.Forms.TreeNode tabNode in this.tvGroupSelect.Nodes)
            {
                ReportTable tab = null;
                if (rptParams.IsMasterDetails)
                {
                    if (ds.Relations.Count > 0 && tabNode.Text != ds.Relations[0].ParentTable.TableName)
                        tab = new ReportTable(tabNode.Text);
                    else
                        tab = new ReportTable(tabNode.Text, this.txtRptCaption.Text);
                }
                else
                {
                    tab = new ReportTable(tabNode.Text, this.txtRptCaption.Text);
                }

                foreach (System.Windows.Forms.TreeNode colNode in tabNode.Nodes)
                {
                    tab.ReportColumns.Add(new ReportColumn(colNode.Text, colNode.Checked));
                }
                rptParams.RptSet.ReportTables.Add(tab);
            }

            return rptParams;
        }
        
        private void CreateWebClient()
        {
            WebClientParam wecParam = GetWebClientParams();
            ClientParam cParam = GetClientParams();            
            Project proj = WebClientProject();
            if (proj != null)
            {
                webformDir = ReportCreator.FindProjectItem(proj, wecParam.FolderName);
                if (webformDir == null)
                {
                    //为WebForm创建文件夹
                    webformDir = ReportCreator.CreateProjectItem(proj, wecParam.FolderName, 0);
                }
                try
                {
                    if (GetWebForm(cParam.FormName, cParam.IsMasterDetails))
                    {
                        GetWebDesignerHost(webformDir);
                        try
                        {
                            GenWebDataSet(webformDir, cParam, cParam.IsMasterDetails);
                            GenWebQuery(cParam);
                            WebClientCreator.WebCreateXSD(FDesignerHost, cParam, wecParam, proj);
                            WriteWebDataSourceHTML(ref FDesignWindow, cParam, webformDir);                            
                            GenSuccess = true;
                        }
                        catch (Exception exception2)
                        {
                            MessageBox.Show(exception2.Message);
                            GenSuccess = false;
                            this.Close();
                            return;
                        }
                        finally
                        {
                        }

                        proj.Save(proj.FullName);
                    }
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                    GenSuccess = false;
                    this.Close();
                    return;
                }
            }
        }

        private void WriteWebDataSourceHTML(ref Window FDesignWindow, ClientParam cParam, ProjectItem projItem)
        {
            String FileName = FDesignWindow.Document.FullName;
            FDesignWindow.Close(vsSaveChanges.vsSaveChangesYes);

            String UpdateHTML = "";

            UpdateHTML = String.Format("<rsweb:reportviewer id=\"ReportViewer1\" runat=\"server\" width=\"100%\" Font-Names=\"Verdana\""
                + " Font-Size=\"8pt\" Height=\"400px\">"
                + "<LocalReport ReportPath=\"\">"
                + "<DataSources>"
                + "<rsweb:ReportDataSource DataSourceId=\"Master\" Name={0} />"
                + "</DataSources>"
                + "</LocalReport>"
                + "</rsweb:reportviewer>",
                "\"NewDataSet_" + cParam.ProviderName.Substring(cParam.ProviderName.IndexOf('.') + 1) + "\"");

            //Start Update Process
            System.IO.StreamReader SR = new System.IO.StreamReader(FileName, Encoding.Default);
            String Context = SR.ReadToEnd();
            SR.Close();

            //Update HTML
            int x, y;
            string temp = "";
            x = Context.IndexOf("<rsweb");
            y = Context.IndexOf("</rsweb");
            temp = Context.Substring(x, (y - x) + 21);
            Context = Context.Replace(temp, UpdateHTML);

            //Page Title
            Context = Context.Replace("<title>Untitled Page</title>", "<title>" + cParam.FormTitle + "</title>");

            System.IO.FileStream Filefs = new System.IO.FileStream(FileName, System.IO.FileMode.Create, System.IO.FileAccess.ReadWrite);
            System.IO.StreamWriter SW = new System.IO.StreamWriter(Filefs, Encoding.UTF8);
            SW.Write(Context);
            SW.Close();
            Filefs.Close();

            FDesignWindow = projItem.Open("{7651A702-06E5-11D1-8EBD-00A0C90F26EA}");
            FDesignWindow.Activate();
        }        

        private void CreateWinClient()
        {
            WinClientParam winParam = GetWinClientParams();
            ClientParam cParam = GetClientParams();
            if (GenWinSolution(winParam, cParam))
            {
                GenWinForm(winParam, cParam);

                DesignerTransaction transaction1 = FDesignerHost.CreateTransaction();
                try
                {
                    WinClientCreator.GenDataSet(GlobalProject, cParam, winParam, FDesignerHost);
                    WinClientCreator.GetWinQuery(FRootForm, cParam, SetFields(cParam));
                    GlobalProject.Save(GlobalProject.FullName);
                    GenSuccess = true;
                }
                catch (Exception exception2)
                {
                    MessageBox.Show(exception2.Message);
                    GenSuccess = false;
                    this.Close();
                    return;
                }
                finally
                {
                    transaction1.Commit();
                }
            }
        }

        private WebClientParam GetWebClientParams()
        {
            WebClientParam wecParam = new WebClientParam();
            wecParam.WebSiteName = (string)cbWebSite.SelectedItem;
            wecParam.AddNewFolder = rbAddToNewFolder.Checked;
            if (!wecParam.AddNewFolder)
            {
                wecParam.FolderName = (string)cbAddToExistFolder.SelectedItem;
            }
            else
            {
                wecParam.FolderName = tbAddToNewFolder.Text;
            }
            return wecParam;
        }

        private ClientParam GetClientParams()
        {
            ClientParam cParam = new ClientParam();
            cParam.FolderName = string.IsNullOrEmpty(tbAddToNewFolder.Text) ? cbAddToExistFolder.Text : tbAddToNewFolder.Text;
            cParam.FormName = tbFormName.Text;
            cParam.FormTitle = tbFormTitle.Text;
            cParam.IsMasterDetails = cbIsMasterDetails.Checked;
            cParam.ProviderName = tbProviderName.Text;
            cParam.TableName = tbTableName.Text;
            cParam.RealTableName = tbTableNameF.Text;
            cParam.ChildTableName = tbChildTableName.Text;
            cParam.SelectAlias = this.cmbSelectAlias.Text;
            cParam.ClientType = (ClientType)this.cmbDataBaseType.SelectedIndex;
            cParam.RptFileName = txtRptFileName.Text;
            if (cParam.IsMasterDetails)
            {
                if (tbChildTableName.Text != "")
                {
                    string[] DetailTableName = tbChildTableName.Text.Split(',');
                    cParam.DetailProviderName = cParam.ProviderName.Substring(0, cParam.ProviderName.IndexOf('.')) + "." + DetailTableName[0];
                }
            }
            return cParam;
        }

        private WinClientParam GetWinClientParams()
        {
            WinClientParam winParam = new WinClientParam();
            winParam.PackageName = tbPackageName.Text;
            winParam.OutputPath = tbOutputPath.Text;
            winParam.AssemblyOutputPath = tbAssemblyOutputPath.Text;
            return winParam;
        }

        public bool GetWebForm(string FormName, bool isMasterDetails)
        {
            string TemplatePath = GetTemplatePath();
            if (TemplatePath == "")
            {
                MessageBox.Show("Cannot find WebTemplate path: {0}", TemplatePath);
                return false;
            }
            if (webformDir != null)
            {
                string BaseFormName = GetBaseWebForm(isMasterDetails);
                foreach (ProjectItem PI in webformDir.ProjectItems)
                {
                    if ((FormName + ".aspx" == PI.Name) ||
                        (FormName + ".aspx.resx" == PI.Name) ||
                        (FormName + ".aspx.vi-VN.resx" == PI.Name))
                    {
                        DialogResult dr = MessageBox.Show("There is another File which name is " + FormName + " existed! Do you want to delete it first", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (dr == DialogResult.Yes)
                        {
                            string Path = PI.get_FileNames(0);

                            PI.Name = Guid.NewGuid().ToString();

                            PI.Open("{7651A702-06E5-11D1-8EBD-00A0C90F26EA}");

                            PI.Delete();

                            File.Delete(Path);
                        }
                        else
                        {
                            return false;
                        }
                    }
                }

                //Copy Template
                ProjectItem TempPI = webformDir;
                webformDir = webformDir.ProjectItems.AddFromFileCopy(TemplatePath + "\\" + BaseFormName + ".aspx");
                webformDir.Name = FormName + ".aspx";
                ProjectItem P1 = TempPI.ProjectItems.AddFromFileCopy(TemplatePath + "\\" + BaseFormName + ".aspx.resx");
                P1.Name = FormName + ".aspx.resx";
                ProjectItem P2 = TempPI.ProjectItems.AddFromFileCopy(TemplatePath + "\\" + BaseFormName + ".aspx.vi-VN.resx");
                P2.Name = FormName + ".aspx.vi-VN.resx";
                //FResxFileName = P2.Name;              
            }
            return true;
        }

        private string GetBaseWebForm(bool isMasterDetails)
        {
            String lan = GetLanguage();
            if (isMasterDetails)
            {
                if (lan == String.Empty || lan == ".cs")
                    return "WMasterDetailReport";
                else if (lan == ".vb")
                    return "VBWMasterDetailReport";
            }
            else
            {
                if (lan == String.Empty || lan == ".cs")
                    return "WebReport";
                else if(lan == ".vb")
                    return "VBWebReport";
            }

            return String.Empty;
        }

        private string GetTemplatePath()
        {
            string TemplatePath = "";
            TemplatePath = EEPRegistry.WebClient + "\\Template";
            return TemplatePath;
        }

        private void GetWebDesignerHost(ProjectItem projItem)
        {
            //FDesignWindow = projItem.Open("{7651A702-06E5-11D1-8EBD-00A0C90F26EA}");
            FDesignWindow = projItem.Open(Constants.vsViewKindDesigner);
            FDesignWindow.Activate();

            HTMLWindow W = (HTMLWindow)FDesignWindow.Object;

            W.CurrentTab = vsHTMLTabs.vsHTMLTabsDesign;
            if (W.CurrentTabObject is WebDevPage.DesignerDocument)
            {
                FDesignerDocument = W.CurrentTabObject as WebDevPage.DesignerDocument; 
            }
        }

        public void GenWebDataSet(ProjectItem projItem, ClientParam cParam, bool isMasterDetails)
        {
            //NotifyRefresh(1000);
            NotifyRefresh(1000);
            //WebDataSet
            string Path = projItem.get_FileNames(0);
            String lan = GetLanguage();
            string FileName = System.IO.Path.GetFileNameWithoutExtension(Path) + ".aspx" + lan;
            Path = System.IO.Path.GetDirectoryName(Path);
            FileName = Path + "\\" + FileName;
            if (!File.Exists(FileName))
                return;
            System.IO.StreamReader SR = new System.IO.StreamReader(FileName);
            string Context = SR.ReadToEnd();
            SR.Close();

            if (lan == String.Empty || lan == ".cs")
            {
                Context = Context.Replace("this.WMaster.RemoteName = null;", "this.WMaster.RemoteName = \"" + cParam.ProviderName + "\";");
                Context = Context.Replace("this.WMaster.Active = false;", "this.WMaster.Active = true;");

                if (isMasterDetails)
                {
                    Context = Context.Replace("this.WDetail.RemoteName = null;", "this.WDetail.RemoteName = \"" + cParam.DetailProviderName + "\";");
                    Context = Context.Replace("this.WDetail.Active = false;", "this.WDetail.Active = true;");
                    Context = Context.Replace("\"NewDataSet_\"", "\"NewDataSet_" + cParam.DetailProviderName.Substring(cParam.DetailProviderName.IndexOf('.') + 1, cParam.DetailProviderName.Length - cParam.DetailProviderName.IndexOf('.') - 1) + "\"");
                }
            }
            else if (lan == ".vb")
            {
                Context = Context.Replace("Me.WMaster.RemoteName = Nothing", "Me.WMaster.RemoteName = \"" + cParam.ProviderName + "\"");
                Context = Context.Replace("Me.WMaster.Active = False", "Me.WMaster.Active = True");

                if (isMasterDetails)
                {
                    Context = Context.Replace("Me.WDetail.RemoteName = Nothing", "Me.WDetail.RemoteName = \"" + cParam.DetailProviderName + "\"");
                    Context = Context.Replace("Me.WDetail.Active = False", "Me.WDetail.Active = True");
                    Context = Context.Replace("\"NewDataSet_\"", "\"NewDataSet_" + cParam.DetailProviderName.Substring(cParam.DetailProviderName.IndexOf('.') + 1, cParam.DetailProviderName.Length - cParam.DetailProviderName.IndexOf('.') - 1) + "\"");
                }
            }

            System.IO.FileStream Filefs = new System.IO.FileStream(FileName, System.IO.FileMode.Create, System.IO.FileAccess.ReadWrite);
            System.IO.StreamWriter SW = new System.IO.StreamWriter(Filefs, Encoding.UTF8);
            SW.Write(Context);
            SW.Close();
            Filefs.Close();
            //WebDataSource Master
            NotifyRefresh(1000);             

            //GetSchema
            if (WizardDataSet != null)
                WizardDataSet.Dispose();
            WizardDataSet = new InfoDataSet(true);
            WizardDataSet.RemoteName = cParam.ProviderName;
            WizardDataSet.Active = true;
            WizardDataSet.ServerModify = false;
            WizardDataSet.PacketRecords = 100;
            WizardDataSet.AlwaysClose = true;

            if (isMasterDetails)
            {
                if (WizardDetailDataSet != null)
                    WizardDetailDataSet.Dispose();
                WizardDetailDataSet = new InfoDataSet(true);
                WizardDetailDataSet.RemoteName = cParam.DetailProviderName;
                WizardDetailDataSet.Active = true;
                WizardDetailDataSet.ServerModify = false;
                WizardDetailDataSet.PacketRecords = 100;
                WizardDetailDataSet.AlwaysClose = false;
            }

            object oMaster = FDesignerDocument.webControls.item("Master", 0);
            if (oMaster != null && oMaster is WebDevPage.IHTMLElement)
            {
                ((WebDevPage.IHTMLElement)oMaster).setAttribute("DataMember", cParam.TableName, 0);
            }

            if (isMasterDetails)
            {
                object oDetail = FDesignerDocument.webControls.item("Detail", 0);
                if (oDetail != null && oDetail is WebDevPage.IHTMLElement)
                {
                    ((WebDevPage.IHTMLElement)oDetail).setAttribute("DataMember", (cParam.ChildTableName.Split(','))[0], 0);
                }
            }         

            WebClientCreator.SaveWebDataSet(WizardDataSet, WizardDetailDataSet, projItem);
        }

        private void NotifyRefresh(uint SleepTime)
        {
            return;
        }

        public void GenWebQuery(ClientParam cParam)
        {
            List<TBlockFieldItem> listfield = SetFields(cParam);
            foreach (TBlockFieldItem aFieldItem in listfield)
            {
                CreateWebQueryField(aFieldItem, "", true);
            }
        }

        private List<TBlockFieldItem> SetFields(ClientParam cParam)
        {
            DataTable dtDD = ReportCreator.GetDDTable(cParam.ClientType, cParam.SelectAlias, cParam.RealTableName);
            DataTable dtTableSchema = FInfoDataSet.RealDataSet.Tables[0];
            List<TBlockFieldItem> list = new List<TBlockFieldItem>();
            for (int I = 0; I < dtTableSchema.Columns.Count; I++)
            {
                DataRow[] DRs = dtDD.Select("FIELD_NAME='" + dtTableSchema.Columns[I].ColumnName + "'");
                TBlockFieldItem aBlockFieldItem = new TBlockFieldItem();
                aBlockFieldItem.DataField = dtTableSchema.Columns[I].ColumnName;
                aBlockFieldItem.DataType = dtTableSchema.Columns[I].DataType;
                if (DRs.Length == 1)
                {
                    DataRow DR = DRs[0];

                    aBlockFieldItem.Description = DR["CAPTION"].ToString();
                    aBlockFieldItem.CheckNull = DR["CHECK_NULL"].ToString().ToUpper();
                    aBlockFieldItem.DefaultValue = DR["DEFAULT_VALUE"].ToString();
                    aBlockFieldItem.ControlType = DR["NEEDBOX"].ToString();
                    aBlockFieldItem.EditMask = DR["EDITMASK"].ToString();
                    if (aBlockFieldItem.DataType == typeof(DateTime))
                    {
                        if (aBlockFieldItem.ControlType == null || aBlockFieldItem.ControlType == "")
                            aBlockFieldItem.ControlType = "DateTimeBox";
                    }
                    aBlockFieldItem.QueryMode = DR["QUERYMODE"].ToString();
                    list.Add(aBlockFieldItem);
                }
            }
            return list;
        }
        
        private void CreateWebQueryField(TBlockFieldItem aFieldItem, string Range, bool NewLine)
        {
            WebQueryColumnsCollection QueryColumns = new WebQueryColumnsCollection(null, typeof(QueryColumns));
            if (string.Compare(aFieldItem.QueryMode, "normal", true) == 0
                  || string.Compare(aFieldItem.QueryMode, "range", true) == 0)
            {
                if (QueryColumns != null)
                {
                    WebQueryColumns column = new WebQueryColumns();
                    column.Column = aFieldItem.DataField;
                    column.Caption = string.IsNullOrEmpty(aFieldItem.Description) ? aFieldItem.DataField : aFieldItem.Description;
                    if (string.Compare(aFieldItem.ControlType, "textbox", true) == 0)
                    {
                        column.ColumnType = "ClientQueryTextBoxColumn";
                    }
                    else if (string.Compare(aFieldItem.ControlType, "combobox", true) == 0)
                    {
                        column.ColumnType = "ClientQueryTextBoxColumn";
                    }
                    else if (string.Compare(aFieldItem.ControlType, "refvalbox", true) == 0)
                    {
                        column.ColumnType = "ClientQueryTextBoxColumn";
                    }
                    else if (string.Compare(aFieldItem.ControlType, "datetimebox", true) == 0)
                    {
                        column.ColumnType = "ClientQueryCalendarColumn";
                    }
                    if (string.Compare(aFieldItem.QueryMode, "normal", true) == 0)
                    {
                        column.Operator = (aFieldItem.DataType == typeof(string)) ? "%" : "=";
                    }
                    else
                    {
                        WebQueryColumns columnrev = new WebQueryColumns();
                        columnrev.Column = column.Column;
                        columnrev.Caption = column.Caption;
                        columnrev.ColumnType = column.ColumnType;
                        columnrev.Operator = ">=";
                        QueryColumns.Add(columnrev);
                        column.Operator = "<=";
                    }
                    QueryColumns.Add(column);
                }
            }

            WebDevPage.IHTMLElement ClientQuery = (WebDevPage.IHTMLElement)FDesignerDocument.webControls.item("WebClientQuery1", 0);
            if (ClientQuery != null)
            {
                SetCollectionValue(ClientQuery, typeof(WebClientQuery).GetProperty("Columns"), QueryColumns);
            }
        }

        private void SetCollectionValue(WebDevPage.IHTMLElement controlElement, PropertyInfo prop, InfoOwnerCollection collection)
        {
            if (controlElement != null)
            {
                string collectionxml = GetCollectionXml(prop, collection);
                if (collectionxml.Length > 0)
                {
                    string html = controlElement.innerHTML;
                    int index = IndexOfBeginTag(html, prop.Name);
                    int length;
                    if (index <= 0)                   
                    {
                        index = 0;
                    }
                    controlElement.innerHTML = html.Insert(index, collectionxml);
                }
            }
        }

        private string GetCollectionXml(PropertyInfo prop, InfoOwnerCollection collection)
        {
            StringBuilder builder = new StringBuilder();
            if (prop != null && collection != null && prop.PropertyType == collection.GetType())
            {
                if (collection.Count > 0)
                {
                    builder.AppendLine(string.Format("\t<{0}>", prop.Name));
                    for (int i = 0; i < collection.Count; i++)
                    {
                        builder.Append(string.Format("\t\t<{0}:{1} ", INFOLIGHTMARK, collection.ItemType.Name));
                        PropertyInfo[] infos = collection.ItemType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
                        for (int j = 0; j < infos.Length; j++)
                        {
                            if (!IsVisibilityHidden(infos[j]))
                            {
                                if (infos[j].PropertyType == typeof(string) || infos[j].PropertyType == typeof(int) || infos[j].PropertyType == typeof(bool)
                                    || infos[j].PropertyType.BaseType == typeof(Enum))
                                {
                                    if (!infos[j].Name.Equals("Name"))
                                    {
                                        object value = infos[j].GetValue(collection[i], null);
                                        object defaultvalue = GetDefaultValue(infos[j]);
                                        if (infos[j].CanWrite && value != defaultvalue)
                                        {
                                            builder.Append(string.Format("{0}=\"{1}\" ", infos[j].Name, value));
                                        }
                                    }
                                }
                            }
                        }
                        builder.AppendLine("/>");
                    }
                    builder.AppendLine(string.Format("\t</{0}>", prop.Name));
                }
            }
            return builder.ToString();
        }

        private bool IsVisibilityHidden(PropertyInfo info)
        {
            object[] attributes = info.GetCustomAttributes(typeof(System.ComponentModel.DesignerSerializationVisibilityAttribute), true);
            if (attributes != null && attributes.Length > 0)
            {
                if (((System.ComponentModel.DesignerSerializationVisibilityAttribute)attributes[0]).Visibility
                    == System.ComponentModel.DesignerSerializationVisibility.Hidden)
                {
                    return true;
                }
            }
            return false;
        }

        private object GetDefaultValue(PropertyInfo info)
        {
            object[] attributes = info.GetCustomAttributes(typeof(System.ComponentModel.DefaultValueAttribute), true);
            if (attributes != null && attributes.Length > 0)
            {
                return ((System.ComponentModel.DefaultValueAttribute)attributes[0]).Value;
            }
            return null;
        }

        private int IndexOfBeginTag(string html, string tag)
        {
            Match mc = Regex.Match(html, string.Format(@"<{0}\s*>", tag));
            if (mc.Success)
            {
                return mc.Index;
            }
            else
            {
                return -1;
            }
        }

        private bool GenWinSolution(WinClientParam winParam, ClientParam cParam)
        {
            Solution sln = _dte2.Solution;
            string BaseFormName = GetBaseWinForm(cbIsMasterDetails.Checked);
            FTemplatePath = WzdUtils.GetAddinsPath() + "\\Templates\\";
            string CurrentSln = _dte2.Solution.FileName;
            String lan = GetLanguage();
            string BaseFormProj = BaseFormName + "\\" + BaseFormName + lan + "proj";
            string FilePath = winParam.OutputPath + "\\" + winParam.PackageName;
            if (System.IO.Directory.Exists(FilePath))
            {
                if (FilePath == "\\")
                    throw new Exception("Unknown Output Path: " + "\\");

                DialogResult dr = MessageBox.Show("There is another File which name is " + winParam.PackageName + " existed! Do you want to delete it first", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

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
                    FilePath, winParam.PackageName, true);
            P.Name = winParam.PackageName;
            string FileName = FilePath + "\\" + winParam.PackageName + lan + "proj";
            P.Save(FileName);
            sln.Open(CurrentSln);
            int I;
            P = null;
            for (I = 1; I <= sln.Projects.Count; I++)
            {
                P = sln.Projects.Item(I);
                if (string.Compare(P.Name, winParam.PackageName) == 0)
                    break;
                else
                    P = null;
            }
            if (P != null)
                sln.Remove(P);
            P = sln.AddFromFile(FilePath + "\\" + winParam.PackageName + lan + "proj", false);
            P.Properties.Item("RootNamespace").Value = winParam.PackageName;
            P.Properties.Item("AssemblyName").Value = winParam.PackageName;
            sln.SaveAs(sln.FileName);
            sln.SolutionBuild.StartupProjects = P;
            sln.SolutionBuild.BuildProject(sln.SolutionBuild.ActiveConfiguration.Name, P.FullName, true);
            GlobalProject = P;

            if (winParam.AssemblyOutputPath != null && winParam.AssemblyOutputPath != "")
                GlobalProject.ConfigurationManager.ActiveConfiguration.Properties.Item("OutputPath").Value = winParam.AssemblyOutputPath;

            return true;
        }

        private string GetBaseWinForm(bool isMasterDetails)
        {
            string BaseFormName = "";
            if (isMasterDetails)
            {
                if (this.cbChooseLanguage.Text == String.Empty || this.cbChooseLanguage.Text == "C#")
                    BaseFormName = "CMasterDetailReport";
                else if (this.cbChooseLanguage.Text == "VB")
                    BaseFormName = "VBCMasterDetailReport";
            }
            else
            {
                if (this.cbChooseLanguage.Text == String.Empty || this.cbChooseLanguage.Text == "C#")
                    BaseFormName = "CReport";
                else if(this.cbChooseLanguage.Text == "VB")
                    BaseFormName = "VBCReport";
            }
            return BaseFormName;
        }

        private void GenWinForm(WinClientParam winParam, ClientParam cParam)
        {
            Solution Sln = _dte2.Solution;
            Project P = null;
            int I;
            for (I = 1; I <= Sln.Projects.Count; I++)
            {
                P = Sln.Projects.Item(I);
                if (string.Compare(winParam.PackageName, P.Name) == 0)
                    break;
                P = null;
            }
            if (P == null)
                throw new Exception("Can not find project " + winParam.PackageName + " in solution");
            ProjectItem PI;
            String lan = GetLanguage();
            for (I = P.ProjectItems.Count; I >= 1; I--)
            {
                PI = P.ProjectItems.Item(I);
                if (string.Compare(PI.Name, "Form1" + lan) == 0)
                {
                    string Path = PI.get_FileNames(0);
                    Path = System.IO.Path.GetDirectoryName(Path);
                    RenameNameSpace(Path + "\\Form1" + lan, winParam.PackageName, cParam.FormName);
                    RenameNameSpace(Path + "\\Form1.Designer" + lan, winParam.PackageName, cParam.FormName);
                    Application.DoEvents();
                    _dte2.MainWindow.Activate();
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(3000);
                    Application.DoEvents();
                    Window W = PI.Open("{00000000-0000-0000-0000-000000000000}");
                    W.Activate();
                    GlobalPI = PI;
                    GlobalWindow = W;
                    if (string.Compare(cParam.FormName, "Form1") != 0)
                    {
                        PI.Name = cParam.FormName + lan;
                        W.Close(vsSaveChanges.vsSaveChangesYes);
                        W = PI.Open("{00000000-0000-0000-0000-000000000000}");
                        W.Activate();
                    }
                    FDesignerHost = (IDesignerHost)W.Object;
                    FRootForm = (System.Windows.Forms.Form)FDesignerHost.RootComponent;
                    FRootForm.Name = cParam.FormName;
                    FRootForm.Text = cParam.FormTitle;
                    IComponentChangeService FComponentChangeService = (IComponentChangeService)FDesignerHost.RootComponent.Site.GetService(typeof(IComponentChangeService));
                }
                if (string.Compare(PI.Name, "Program" + lan) == 0)
                {
                    RenameNameSpace(PI.get_FileNames(0), winParam.PackageName, cParam.FormName);
                    Application.DoEvents();
                    _dte2.MainWindow.Activate();
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(3000);
                    Application.DoEvents();

                }
            }
        }

        private void RenameNameSpace(string FileName, string PackageName, string FormName)
        {
            if (!File.Exists(FileName))
                return;
            System.IO.StreamReader SR = new System.IO.StreamReader(FileName);
            string Context = SR.ReadToEnd();
            SR.Close();
            Context = Context.Replace("TAG_NAMESPACE", PackageName);
            Context = Context.Replace("TAG_FORMNAME", FormName);
            System.IO.FileStream Filefs = new System.IO.FileStream(FileName, System.IO.FileMode.Create, System.IO.FileAccess.ReadWrite);
            System.IO.StreamWriter SW = new System.IO.StreamWriter(Filefs, Encoding.UTF8);
            SW.Write(Context);
            SW.Close();
            Filefs.Close();
        }        

        private void btnNext_Click(object sender, EventArgs e)
        {
            Previousflag = false;
            setWizardStep(currentStep + 1);
            if(currentStep == 4)
                CreateData();
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            Previousflag = true;
            setWizardStep(currentStep - 1);            
        }

        private bool Previousflag = false;
        private int currentStep = 0;
        private void setWizardStep(int step)
        {
            if (step > 4 || step < 0)
                return;
            currentStep = step;
            this.tbcContainer.TabPages.Clear();
            switch (step)
            { 
                case 0:
                    this.tbcContainer.TabPages.Add(this.tp1);
                    this.btnPrevious.Enabled = false;
                    this.btnNext.Enabled = true;
                    this.btnDone.Enabled = false;
                    //init
                    string sevPath = ReportCreator.GetServerPath();
                    XmlDocument doc = new XmlDocument();
                    doc.Load(SystemFile.DBFile);
                    XmlNode nDataBase = doc.SelectSingleNode("InfolightDB/DataBase");
                    ArrayList lstDB = new ArrayList();
                    foreach (XmlNode node in nDataBase.ChildNodes)
                    {
                        lstDB.Add(node.Name);
                    }
                    this.cmbSelectAlias.DataSource = lstDB;
                    this.cmbSelectAlias.SelectedIndex = 0;
                    this.cmbDataBaseType.SelectedIndex = 1;
                    this.txtDetailsRptFileName.Enabled = false;
                    this.cmbMasterReportStyle.SelectedIndex = 0;
                    break;
                case 1:
                    this.tbcContainer.TabPages.Add(this.tp4);
                    this.btnPrevious.Enabled = false;
                    this.btnNext.Enabled = true;
                    this.btnDone.Enabled = false;
                    if (this._isWebReport)
                    {
                        this.panel4.Visible = false;
                        if (!Previousflag)
                        {
                            cbWebSite.Items.Clear();
                            foreach (Project P in _dte2.Solution.Projects)
                            {
                                if (string.Compare(P.Kind, "{E24C65DC-7377-472b-9ABA-BC803B73C61A}") == 0)
                                {
                                    cbWebSite.Items.Add(P.Name);
                                }
                            }
                        }
                    }
                    else
                    {
                        this.panel4.Visible = true;
                        if (!Previousflag)
                        {
                            string S = _dte2.Solution.FileName;
                            S = System.IO.Path.GetDirectoryName(S);
                            String SolutionName = Path.GetFileNameWithoutExtension(_dte2.Solution.FileName);
                            tbOutputPath.Text = S + @"\" + SolutionName;
                            tbAssemblyOutputPath.Text = String.Format(@"..\..\EEPNetClient\{0}", SolutionName);
                            tbPackageName.Text = "ClientPackage";
                        }
                    }
                    if (!Previousflag)
                    {
                        tbFormName.Text = "Form1";
                        tbFormTitle.Text = "Form1";
                    }
                    break;
                case 2:
                    if (checkStepCompleted(step - 1))
                    {
                        this.tbcContainer.TabPages.Add(this.tp5);
                        this.btnPrevious.Enabled = true;
                        this.btnNext.Enabled = false;
                        this.btnDone.Enabled = true;
                        FInfoDataSet = new InfoDataSet();
                        FInfoDataSet.SetWizardDesignMode(true);
                        break;
                    }
                    else
                    {
                        this.tbcContainer.TabPages.Add(this.tp4);
                        this.btnPrevious.Enabled = false;
                        this.btnNext.Enabled = true;
                        this.btnDone.Enabled = false;
                        currentStep--;
                        step--;
                        return;
                    }                    
                case 3:
                    this.tbcContainer.TabPages.Add(this.tp2);
                    if (this._isWebReport)
                    {
                        this.panel3.Visible = false;
                        Project proj = WebClientProject();
                        foreach (ProjectItem item in proj.ProjectItems)
                        {
                            if (item.Kind == "{6BB5F8EF-4483-11D3-8BCF-00C04F8EC28C}")
                            {
                                this.cmbRootName.Items.Add(item.Name);
                            }
                        }
                        this.cmbRootName.SelectedItem = this.cbAddToExistFolder.SelectedItem;
                    }
                    else
                    {
                        this.panel3.Visible = true;
                        this.cmbProName.Items.AddRange(ReportCreator.FindAllClientProject(_dte2.Solution as Solution2).ToArray());
                        this.cmbProName.SelectedItem = tbPackageName.Text;
                    }
                    chkIsMasterDetails.Checked = cbIsMasterDetails.Checked;
                    txtRptFileName.Text = this.tbFormName.Text;
                    if (cbIsMasterDetails.Checked)
                    {
                        txtDetailsRptFileName.Text = this.tbFormName.Text + "-dt";
                    }
                    txtRptCaption.Text = tbFormTitle.Text;
                    tbHorGaps.Text = "2.54";
                    tbVertGaps.Text = "0.64";
                    this.btnPrevious.Enabled = false;
                    this.btnNext.Enabled = true;
                    this.btnDone.Enabled = false;
                    break;
                case 4:
                    this.tbcContainer.TabPages.Add(this.tp3);
                    this.btnPrevious.Enabled = true;
                    this.btnNext.Enabled = false;
                    this.btnDone.Enabled = true;
                    break;
            }
            tbcContainer.SelectedIndex = step;
        }

        private bool checkStepCompleted(int step)
        {
            if (step > 2 || step < 0)
                return false;
            switch (step)
            { 
                case 0:
                    if (this.cmbDataBaseType.SelectedIndex == -1 || this.cmbSelectAlias.SelectedIndex == -1)
                        return false;
                    break;
                case 1:
                    if (this._isWebReport && cbWebSite.Text == "")
                    {
                        cbWebSite.Focus();
                        MessageBox.Show("Please select a WebSite");
                        return false;
                    }
                    else if (this._isWebReport && rbAddToExistFolder.Checked && (cbAddToExistFolder.Text == ""))
                    {
                        cbAddToExistFolder.Focus();
                        MessageBox.Show("Please select a exist folder");
                        return false;
                    }
                    else if (this._isWebReport && rbAddToNewFolder.Checked && (tbAddToNewFolder.Text == ""))
                    {
                        tbAddToNewFolder.Focus();
                        MessageBox.Show("Please input new folder");
                        return false;
                    }
                    if (!this._isWebReport && tbPackageName.Text == "")
                    {
                        MessageBox.Show("Please input Package Name !!");
                        if (tbPackageName.CanFocus)
                        {
                            tbPackageName.Focus();
                        }
                        return false;
                    }
                    if (tbFormName.Text == "")
                    {
                        tbFormName.Focus();
                        MessageBox.Show("Please input Form Name");
                        return false;
                    }
                    break;
                case 2:
                    if (tbProviderName.Text == "")
                    {
                        MessageBox.Show("Please input Provider Name !!");
                        if (tbProviderName.CanFocus)
                        {
                            tbProviderName.Focus();
                            return false;
                        }
                    }
                    else if (tbTableNameF.Text == "")
                    {
                        MessageBox.Show("Please input Table Name !!");
                        if (tbTableNameF.CanFocus)
                        {
                            tbTableNameF.Focus();
                            return false;
                        }
                    }
                    break;
                case 3:

                    break;
                case 4:

                    break;
            }
            return true;
        }

        private void cmbRootName_SelectedIndexChanged(object sender, EventArgs e)
        {
            //this.cmbXSDName.Enabled = true;
            Project proj = WebClientProject();
            ProjectItem floder = null;
            this.cmbXSDName.Items.Clear();
            foreach (ProjectItem folderItem in proj.ProjectItems)
            {
                if (folderItem.Name == this.cmbRootName.Text)
                {
                    foreach (ProjectItem item in folderItem.ProjectItems)
                    {
                        if (item.Name.ToLower().EndsWith(".xsd"))
                            this.cmbXSDName.Items.Add(item.Name);
                    }
                    floder = folderItem;
                    break;
                }
            }

            this.cmbXSDName.Enabled = (this.cmbXSDName.Items.Count > 0);
            if (floder != null && !this.cmbXSDName.Enabled)
            {
                MessageBox.Show(string.Format("{0} doesn't contain any xsd files", floder.Name));
            }
            else
            {
                cmbXSDName.SelectedIndex = cmbXSDName.FindString(this.tbFormName.Text + ".xsd");
            }
        }

        private void cmbProName_SelectedIndexChanged(object sender, EventArgs e)
        {
            Project proj = WinClientProject(this.cmbProName.Text);
            this.cmbWinXSDName.Items.Clear();
            foreach (ProjectItem item in proj.ProjectItems)
            {
                if (item.Name.ToLower().EndsWith(".xsd"))
                    this.cmbWinXSDName.Items.Add(item.Name);
            }

            this.cmbWinXSDName.Enabled = (this.cmbWinXSDName.Items.Count > 0);
            if (!this.cmbWinXSDName.Enabled)
            {
                MessageBox.Show(string.Format("{0} doesn't contain any xsd files", proj.Name));
            }
            else
            {
                cmbWinXSDName.SelectedIndex = 0;
            }
        }

        private void cmbWinXSDName_SelectedIndexChanged(object sender, EventArgs e)
        {
            //GenDataSet();
            //if (ds.Relations.Count > 0)
            //{
            //    this.txtRptCaption.Text = ds.Relations[0].ParentTable.TableName;
            //}
            //else
            //{
            //    this.txtRptCaption.Text = ds.Tables[0].TableName;
            //}
        }

        private void cmbXSDName_SelectedIndexChanged(object sender, EventArgs e)
        {
            //GenDataSet();
            //if (ds.Relations.Count > 0)
            //{
            //    this.txtRptCaption.Text = ds.Relations[0].ParentTable.TableName;
            //}
            //else
            //{
            //    this.txtRptCaption.Text = ds.Tables[0].TableName;
            //}
        }

        private void btnOutputPath_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                tbOutputPath.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void btnAssemblyOutputPath_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                tbAssemblyOutputPath.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void tbFormName_TextChanged(object sender, EventArgs e)
        {
            tbFormTitle.Text = tbFormName.Text;
        }

        private void cbWebSite_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbAddToExistFolder.Items.Clear();
            foreach (Project P in _dte2.Solution.Projects)
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

        private void EnableFolderControl()
        {
            cbAddToExistFolder.Enabled = rbAddToExistFolder.Checked;
            tbAddToNewFolder.Enabled = rbAddToNewFolder.Checked;
        }

        private void rbAddToExistFolder_CheckedChanged(object sender, EventArgs e)
        {
            EnableFolderControl();
        }

        private void rbAddToNewFolder_CheckedChanged(object sender, EventArgs e)
        {
            EnableFolderControl();
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
            String SolutionName = System.IO.Path.GetFileNameWithoutExtension(_dte2.Solution.FullName);
            tbTableNameF.Text = CliUtils.GetTableName(ModuleName, DataSetName, SolutionName, "", true);
            string DllName = tbProviderName.Text;
            int Index = DllName.IndexOf('.');
            DllName = DllName.Substring(0, Index + 1);
            ShowTableRelations(ModuleName, SolutionName);
        }

        private void ShowTableRelations(String ModuleName, String SolutionName)
        {
            InfoBindingSource IBS = new InfoBindingSource();
            DataRelation R = null;
            try
            {
                IBS.DataSource = FInfoDataSet;
                IBS.DataMember = FInfoDataSet.RealDataSet.Tables[0].TableName;
                ShowTable(ModuleName, SolutionName, IBS, R);
            }
            finally
            {
                IBS.Dispose();
            }
        }

        private void ShowTable(String ModuleName, String SolutionName, InfoBindingSource aBindingSource, DataRelation Relation)
        {
            DataRelation R1;
            if (aBindingSource.DataSource.GetType().Equals(typeof(InfoDataSet)))
            {
                InfoDataSet set1 = (InfoDataSet)aBindingSource.DataSource;
                for (int I = 0; I < set1.RealDataSet.Tables[0].ChildRelations.Count; I++)
                {
                    R1 = set1.RealDataSet.Tables[0].ChildRelations[I];                    
                    tbChildTableName.Text += R1.ChildTable.TableName + ",";
                    tbRealChildTableName.Text += CliUtils.GetTableName(ModuleName, R1.ChildTable.TableName, SolutionName, "", true) + ",";          
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
                            tbChildTableName.Text += R1.ChildTable.TableName + ",";
                            tbRealChildTableName.Text += CliUtils.GetTableName(ModuleName, R1.ChildTable.TableName, SolutionName, "", true) + ",";   

                        }
                    }
                }
            }
            if (tbChildTableName.Text != "")
            {
                tbChildTableName.Text = tbChildTableName.Text.Substring(0, tbChildTableName.Text.Length - 1);
                tbRealChildTableName.Text = tbRealChildTableName.Text.Substring(0, tbRealChildTableName.Text.Length - 1);
            }
        }        
    }
}