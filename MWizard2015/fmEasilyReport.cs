using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using EnvDTE80;
using EnvDTE;
using System.ComponentModel.Design;
using System.Xml;
using Microsoft.Win32;
using System.Collections;
using Srvtools;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Designer.Interfaces;
//using Microsoft.Reporting.WinForms;
using System.Data.Common;
using System.Web.UI.Design.WebControls;
#if VS90
using WebDevPage = Microsoft.VisualWebDeveloper.Interop.WebDeveloperPage;
using System.Reflection;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
#endif

namespace MWizard2015
{
    public partial class fmEasilyReport : Form
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

#if VS90
        private WebDevPage.DesignerDocument FDesignerDocument;
#endif

        public fmEasilyReport()
        {
            InitializeComponent();
        }

        public fmEasilyReport(DTE2 dte2, AddIn addIn, bool isWebReport)
        {
            InitializeComponent();
            _dte2 = dte2;
            _addIn = addIn;
            _isWebReport = isWebReport;
        }

        private DTE2 _dte2;
        private AddIn _addIn;
        private bool _isWebReport;
        private InfoDataSet ds = null;

        private void frmEasilyReport_Load(object sender, EventArgs e)
        {
            setWizardStep(0);
        }

        private void CreateData()
        {
            this.tvDataSelect.Nodes.Clear();
            this.tvGroupSelect.Nodes.Clear();
            GenDataSet();
            if (ds.RealDataSet.Tables.Count > 0)
            {
                this.panel2.Visible = true;
                foreach (DataTable tab in ds.RealDataSet.Tables)
                {
                    System.Windows.Forms.TreeNode nTab = this.tvDataSelect.Nodes.Add(tab.TableName, tab.TableName);
                    foreach (DataColumn col in tab.Columns)
                    {
                        nTab.Nodes.Add(col.ColumnName, col.ColumnName);
                    }
                }
            }
        }

        private String FindDBType(String aliasName)
        {
            String xmlName = SystemFile.DBFile;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlName);

            XmlNode node = xmlDoc.FirstChild.FirstChild.SelectSingleNode(aliasName);

            string DbType = node.Attributes["Type"].Value.Trim();
            return DbType;
        }

        private void GenDataSet()
        {
            ds = this.FInfoDataSet;
        }

        private void chkIsMasterDetails_CheckedChanged(object sender, EventArgs e)
        {
            foreach (System.Windows.Forms.TreeNode node in this.tvDataSelect.Nodes)
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

        private void cmbMasterReportStyle_SelectedIndexChanged(object sender, EventArgs e)
        {

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
            else if (e.Node.Level == 1)
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
                foreach (System.Windows.Forms.TreeNode node in e.Node.Parent.Nodes)
                {
                    if (node.Checked)
                    {
                        e.Cancel = true;
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
            //產生Client Project
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


            //ReportParameter rptParams = GetRptParams();
            //if (this._isWebReport)
            //{
            //    Project proj = WebClientProject();
            //    if (proj != null)
            //    {
            //        ProjectItem reportDir = ReportCreator.FindProjectItem(proj, rptParams.RptRootName);
            //        if (reportDir == null)
            //        {
            //            //为Report创建文件夹
            //            reportDir = ReportCreator.CreateProjectItem(proj, rptParams.RptRootName, 0);
            //        }
            //        ProjectItem piDataSet = ReportCreator.FindProjectItem(reportDir, rptParams.RptXSDFile);
            //        if (piDataSet != null)
            //        {
            //            List<string> prtFileNames = new List<string>(new string[] { this.txtRptFileName.Text });
            //            List<string> RealTableName = new List<string>(new string[] { this.tbTableNameF.Text });
            //            if (this.chkIsMasterDetails.Checked)
            //            {
            //                prtFileNames.Add(this.txtDetailsRptFileName.Text);
            //                RealTableName.Add(this.tbRealChildTableName.Text.Split(',')[0]);
            //            }
            //            ReportCreator.GenWebReportFiles(reportDir, piDataSet, rptParams, prtFileNames, RealTableName);
            //            WebClientCreator.GenReportViewProperty(FDesignWindow, reportDir, chkIsMasterDetails.Checked, cmbRootName.SelectedItem.ToString(), txtRptFileName.Text);
            //        }
            //    }
            //}
            //else
            //{
            //    Project proj = WinClientProject(this.cmbProName.Text);
            //    ProjectItem piDataSet = ReportCreator.FindProjectItem(proj, rptParams.RptXSDFile);
            //    if (piDataSet != null)
            //    {
            //        String lan = GetLanguage();
            //        List<string> prtFileNames = new List<string>(new string[] { this.txtRptFileName.Text });
            //        List<string> RealTableName = new List<string>(new string[] { this.tbTableNameF.Text });
            //        if (this.chkIsMasterDetails.Checked)
            //        {
            //            prtFileNames.Add(this.txtDetailsRptFileName.Text);
            //            RealTableName.Add(this.tbRealChildTableName.Text.Split(',')[0]);
            //        }
            //        ReportCreator.GenWinReportFiles(proj, piDataSet, rptParams, prtFileNames, RealTableName);
            //        WinClientCreator.GetReportViewProperty(FRootForm, proj, chkIsMasterDetails.Checked, tbTableName.Text, tbPackageName.Text, tbFormName.Text, txtRptFileName.Text, tbChildTableName.Text, lan);
            //        GlobalProject.Save(GlobalProject.FullName);
            //        Solution sln = _dte2.Solution;
            //        sln.Remove(GlobalProject);
            //        string FilePath = tbOutputPath.Text + "\\" + tbPackageName.Text;
            //        Project P = sln.AddFromFile(FilePath + "\\" + tbPackageName.Text + lan + "proj", false);
            //        if (lan != ".vb")
            //            P.Properties.Item("RootNamespace").Value = tbPackageName.Text;
            //        sln.SaveAs(sln.FileName);
            //        sln.SolutionBuild.StartupProjects = P;
            //        sln.SolutionBuild.BuildProject(sln.SolutionBuild.ActiveConfiguration.Name, P.FullName, true);
            //        GlobalProject = P;
            //        _dte2.Solution.SolutionBuild.BuildProject(_dte2.Solution.SolutionBuild.ActiveConfiguration.Name,
            //            GlobalProject.FullName, true);
            //        foreach (ProjectItem PI in GlobalProject.ProjectItems)
            //        {
            //            if (PI.Name == tbFormName.Text + lan)
            //            {
            //                Window W = PI.Open("{00000000-0000-0000-0000-000000000000}");
            //                W.Activate();
            //                FDesignerHost = (IDesignerHost)W.Object;
            //            }
            //        }
            //    }
            //}
            this.Close();
        }

        private String GetLanguage()
        {
            if (this.cbChooseLanguage.Text == String.Empty || this.cbChooseLanguage.Text == "C#")
                return ".cs";
            else if (this.cbChooseLanguage.Text == "VB")
                return ".vb";
            return String.Empty;
        }

        DataTable SelectedMasterFields;
        private void SetSelectedMasterFields(ClientParam cParam)
        {
            SelectedMasterFields = new DataTable();
            SelectedMasterFields.Columns.Add(new DataColumn("ColumnName", typeof(String)));
            SelectedMasterFields.Columns.Add(new DataColumn("DataType", typeof(Type)));
            SelectedMasterFields.Columns.Add(new DataColumn("Caption", typeof(String)));
            SelectedMasterFields.Columns.Add(new DataColumn("Width", typeof(int)));
            SelectedMasterFields.Columns.Add(new DataColumn("EditMask", typeof(String)));

            List<TBlockFieldItem> listfield = SetFields(cParam,cParam.TableName, cParam.RealTableName);
            foreach (TBlockFieldItem tb in listfield)
            {
                foreach (System.Windows.Forms.TreeNode tn in this.tvDataSelect.Nodes[0].Nodes)
                {
                    if(tn.Text == tb.DataField)
                    {
                        if (tn.Checked)
                        {
                            DataRow dr = SelectedMasterFields.NewRow();
                            dr["ColumnName"] = tb.DataField;
                            dr["DataType"] = tb.DataType;
                            dr["Caption"] = tb.Description;
                            dr["Width"] = tb.Length;
                            dr["EditMask"] = tb.EditMask;
                            SelectedMasterFields.Rows.Add(dr);
                        }
                        break;
                    }
                }
            }
        }

        DataTable SelectedDetailFields;
        private void SetSelectedDetailFields(ClientParam cParam)
        {
            SelectedDetailFields = new DataTable();
            SelectedDetailFields.Columns.Add(new DataColumn("ColumnName", typeof(String)));
            SelectedDetailFields.Columns.Add(new DataColumn("DataType", typeof(Type)));
            SelectedDetailFields.Columns.Add(new DataColumn("Caption", typeof(String)));
            SelectedDetailFields.Columns.Add(new DataColumn("Width", typeof(int)));
            SelectedDetailFields.Columns.Add(new DataColumn("EditMask", typeof(String)));

            List<TBlockFieldItem> listfield = SetFields(cParam, cParam.ChildTableName, cParam.ChildRealTableName);
            foreach (TBlockFieldItem tb in listfield)
            {
                foreach (System.Windows.Forms.TreeNode tn in this.tvDataSelect.Nodes[1].Nodes)
                {
                    if (tn.Text == tb.DataField)
                    {
                        if (tn.Checked)
                        {
                            DataRow dr = SelectedDetailFields.NewRow();
                            dr["ColumnName"] = tb.DataField;
                            dr["DataType"] = tb.DataType;
                            dr["Caption"] = tb.Description;
                            dr["Width"] = tb.Length;
                            dr["EditMask"] = tb.EditMask;
                            SelectedDetailFields.Rows.Add(dr);
                        }
                        break;
                    }
                }
            }
        }

        DataTable SelectedMasterGroups;
        private void SetSelectedMasterGroups(ClientParam cParam)
        {
            SelectedMasterGroups = new DataTable();
            SelectedMasterGroups.Columns.Add(new DataColumn("ColumnName", typeof(String)));
            SelectedMasterGroups.Columns.Add(new DataColumn("DataType", typeof(Type)));
            SelectedMasterGroups.Columns.Add(new DataColumn("Caption", typeof(String)));
            SelectedMasterGroups.Columns.Add(new DataColumn("Width", typeof(int)));
            SelectedMasterGroups.Columns.Add(new DataColumn("EditMask", typeof(String)));

            List<TBlockFieldItem> listfield = SetFields(cParam,cParam.TableName, cParam.RealTableName);
            foreach (TBlockFieldItem tb in listfield)
            {
                foreach (System.Windows.Forms.TreeNode tn in this.tvGroupSelect.Nodes[0].Nodes)
                {
                    if (tn.Text == tb.DataField)
                    {
                        if (tn.Checked)
                        {
                            DataRow dr = SelectedMasterGroups.NewRow();
                            dr["ColumnName"] = tb.DataField;
                            dr["DataType"] = tb.DataType;
                            dr["Caption"] = tb.Description;
                            dr["Width"] = tb.Length;
                            dr["EditMask"] = tb.EditMask;
                            SelectedMasterGroups.Rows.Add(dr);
                        }
                        break;
                    }
                }
            }
        }

        DataTable SelectedDetailGroups;
        private void SetSelectedDetailGroups(ClientParam cParam)
        {
            SelectedDetailGroups = new DataTable();
            SelectedDetailGroups.Columns.Add(new DataColumn("ColumnName", typeof(String)));
            SelectedDetailGroups.Columns.Add(new DataColumn("DataType", typeof(Type)));
            SelectedDetailGroups.Columns.Add(new DataColumn("Caption", typeof(String)));
            SelectedDetailGroups.Columns.Add(new DataColumn("Width", typeof(int)));
            SelectedDetailGroups.Columns.Add(new DataColumn("EditMask", typeof(String)));

            List<TBlockFieldItem> listfield = SetFields(cParam, cParam.ChildTableName, cParam.ChildRealTableName);
            foreach (TBlockFieldItem tb in listfield)
            {
                foreach (System.Windows.Forms.TreeNode tn in this.tvGroupSelect.Nodes[1].Nodes)
                {
                    if (tn.Text == tb.DataField)
                    {
                        if (tn.Checked)
                        {
                            DataRow dr = SelectedDetailGroups.NewRow();
                            dr["ColumnName"] = tb.DataField;
                            dr["DataType"] = tb.DataType;
                            dr["Caption"] = tb.Description;
                            dr["Width"] = tb.Length;
                            dr["EditMask"] = tb.EditMask;
                            SelectedDetailGroups.Rows.Add(dr);
                        }
                        break;
                    }
                }
            }
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
#if VS90
#else
                        DesignerTransaction transaction1 = FDesignerHost.CreateTransaction();
#endif
                        try
                        {
                            GenWebDataSet(webformDir, cParam, cParam.IsMasterDetails);
                            GenWebQuery(cParam);
                            GenBlock(cParam);
                            GenWebEasilyReport(cParam);
                            WriteWebFormTitle(FDesignWindow, cParam, webformDir);                            
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
#if VS90
#else
                            transaction1.Commit();
#endif
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

        public void WriteWebFormTitle(Window FDesignWindow, ClientParam cParam, ProjectItem projItem)
        {
            String FileName = FDesignWindow.Document.FullName;
            FDesignWindow.Close(vsSaveChanges.vsSaveChangesYes);

            //Start Update Process
            System.IO.StreamReader SR = new System.IO.StreamReader(FileName, Encoding.Default);
            String Context = SR.ReadToEnd();
            SR.Close();

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

        private void GenBlock(ClientParam cp)
        {
            if (this.cbTemplate.Text == "WebEasilyReportSingle")
            {
                SetSelectedMasterFields(cp);
                SetSelectedMasterGroups(cp);
                GenMasterGridView(cp);
            }
            else if (this.cbTemplate.Text == "WebEasilyReportMasterDetail1")
            {
                SetSelectedMasterFields(cp);
                SetSelectedMasterGroups(cp);
                SetSelectedDetailFields(cp);
                SetSelectedDetailGroups(cp);
                GenMasterFormView(cp);
                GenDetailGridView(cp);
            }
        }

        private void GenMasterGridView(ClientParam cParam)
        {
#if VS90
            object oMaster = FDesignerDocument.webControls.item("Master", 0);

            WebDevPage.IHTMLElement eMaster = null;
            WebDevPage.IHTMLElement eWebGridView1 = null;

            if (oMaster == null || !(oMaster is WebDevPage.IHTMLElement))
                return;
            eMaster = (WebDevPage.IHTMLElement)oMaster;

            object oWebGridView1 = FDesignerDocument.webControls.item("wgvMaster", 0);
            if (oWebGridView1 == null)
                oWebGridView1 = FDesignerDocument.webControls.item("WebGridView1", 0);
            eWebGridView1 = (WebDevPage.IHTMLElement)oWebGridView1;
            //eWebGridView1.setAttribute("DataMember", FClientData.TableName, 0);

            //这里本来想再往下找Columns节点的,可是找不到,只能先这样写了
            StringBuilder sb = new StringBuilder(eWebGridView1.innerHTML);
            int idx = eWebGridView1.innerHTML.IndexOf("</Columns>");
            if (idx == -1)
            {
                idx = sb.ToString().IndexOf("<SelectedRowStyle");
                sb.Insert(idx, "<Columns>\r\n            </Columns>");
            }
            List<string> KeyFields = new List<string>();
            foreach (DataRow dr in SelectedMasterFields.Rows)
            {
                idx = sb.ToString().IndexOf("</Columns>");

                sb.Insert(idx, "\r            <asp:BoundField DataField=\"" + dr["ColumnName"].ToString() + "\" HeaderText=\"" + (string.IsNullOrEmpty(dr["Caption"].ToString()) ? dr["ColumnName"].ToString() : dr["Caption"].ToString()) + "\" SortExpression=\"" + dr["ColumnName"].ToString() + "\" />\r\n            ");

            }
            eWebGridView1.innerHTML = sb.ToString();
#else
            WebGridView WebGridView1 = (WebGridView)FPage.FindControl("WebGridView1");
            WebGridView1.Columns.Clear();
            List<string> KeyFields = new List<string>();
            foreach (DataRow dr in SelectedMasterFields.Rows)
            {
                System.Web.UI.WebControls.BoundField aBoundField = new System.Web.UI.WebControls.BoundField();
                aBoundField.DataField = dr["ColumnName"].ToString();
                aBoundField.HeaderText = dr["Caption"].ToString();
                aBoundField.SortExpression = dr["ColumnName"].ToString();
                if (aBoundField.HeaderText == "")
                    aBoundField.HeaderText = dr["ColumnName"].ToString();
                WebGridView1.Columns.Add(aBoundField);
            }

            IComponentChangeService FComponentChangeService = (IComponentChangeService)FDesignerHost.RootComponent.Site.GetService(typeof(IComponentChangeService));
            NotifyRefresh(200);
            FComponentChangeService.OnComponentChanged(WebGridView1, null, "", "M");
#endif
        }

        private void GenMasterFormView(ClientParam cp)
        {
#if VS90
            WebDevPage.IHTMLElement FormView = (WebDevPage.IHTMLElement)FDesignerDocument.webControls.item("wfvMaster", 0);
            if (FormView != null)
            {
                RefreshFormView(FormView, SelectedMasterFields.Rows);
            }
#else
            WebDataSource Master = (WebDataSource)FPage.FindControl("Master");
            WebFormView wfvMaster = (WebFormView)FPage.FindControl("wfvMaster");
            //Generate RESX
            GenResx(Master);
            FormViewDesigner aDesigner = FDesignerHost.GetDesigner(wfvMaster) as FormViewDesigner;

            //FormView
            foreach (System.Web.UI.Design.TemplateGroup tempGroup in aDesigner.TemplateGroups)
            {
                foreach (System.Web.UI.Design.TemplateDefinition tempDefin in tempGroup.Templates)
                {
                    if (tempDefin.Name == "EditItemTemplate" || tempDefin.Name == "InsertItemTemplate" || tempDefin.Name == "ItemTemplate")
                    {
                        StringBuilder builder = new StringBuilder();
                        string content = tempDefin.Content;
                        if (content == null || content.Length == 0)
                            continue;

                        string[] ctrlTexts = content.Split("\r\n".ToCharArray());
                        //Control[] ctrls = ControlParser.ParseControls(host, content);
                        int i = 0;
                        int j = 0;
                        int m = wfvMaster.LayOutColNum * 2;

                        List<string> lists = new List<string>();
                        String ExtraName = "";

                        foreach (DataRow dr in SelectedMasterFields.Rows)
                        {
                            lists.Add(dr["ColumnName"].ToString());

                            String FormatStyle = FormatEditMask(dr["EditMask"].ToString());

                            if (tempDefin.Name == "ItemTemplate")
                            {
                                String S3 = String.Format("<asp:Label ID=\"{0}\" runat=\"server\" Text='<%# Bind(\"{1}\"{2}) %>'></asp:Label>", "l" + dr["ColumnName"].ToString(), dr["ColumnName"].ToString(), FormatStyle);
                                lists.Add(S3);
                            }
                            else
                            {
                                if (tempDefin.Name == "InsertItemTemplate")
                                {
                                    FormViewField aViewField = new FormViewField();
                                    aViewField.ControlID = "tb" + dr["ColumnName"].ToString();
                                    aViewField.FieldName = dr["Caption"].ToString();
                                    wfvMaster.Fields.Add(aViewField);
                                }
                                String S4 = String.Format("<asp:TextBox ID=\"{0}\" runat=\"server\" Text='<%# Bind(\"{1}\"{2}) %>' MaxLength=\"{3}\"></asp:TextBox>", "tb" + dr["ColumnName"].ToString(), dr["ColumnName"].ToString(), FormatStyle, dr["Width"].ToString());
                                lists.Add(S4);
                            }
                        }

                        j = j * 2;

                        if (m > 0)
                        {
                            builder.Append("<table>");
                        }

                        foreach (string ctrlText in lists.ToArray())
                        {
                            if (ctrlText == null || ctrlText.Length == 0)
                                continue;

                            if (m > 0)
                            {
                                if (i % m == 0)
                                {
                                    builder.Append("<tr>");
                                }

                                builder.Append("<td>");
                            }
                            // add dd
                            string ddText = "";
                            if (tempDefin.Name != "ItemTemplate")
                            {
                                //ddText = GetValidateText(ctrlText);
                                //ddText = GetDDText(ddText, BlockItem);
                                ddText = GetDDText(ctrlText, SelectedMasterFields, tempDefin.Name);
                            }
                            else
                            {
                                ddText = GetDDText(ctrlText, SelectedMasterFields, tempDefin.Name);
                            }
                            builder.Append(ddText);
                            builder.Append("\r\n");

                            if (m > 0)
                            {
                                builder.Append("</td>");

                                if (i % m == m - 1)
                                {
                                    builder.Append("</tr>");
                                }
                            }
                            i++;
                        }

                        if (m > 0)
                        {
                            if (i % m != 0)
                            {
                                int n = m - (i % m);
                                int q = 0;
                                while (q < n)
                                {
                                    builder.Append("<td></td>");
                                    q++;
                                }
                                builder.Append("</tr>");
                            }
                            builder.Append("</table>");
                        }

                        tempDefin.Content = builder.ToString();
                    }
                }
            }

            IComponentChangeService FComponentChangeService = (IComponentChangeService)FDesignerHost.RootComponent.Site.GetService(typeof(IComponentChangeService));
            NotifyRefresh(200);
            FComponentChangeService.OnComponentChanged(wfvMaster, null, "", "M");
            FComponentChangeService.OnComponentChanged(Master, null, "", "M");
#endif
        }

#if VS90
        private void RefreshFormView(WebDevPage.IHTMLElement formViewElement, DataRowCollection drc)
        {
            if (formViewElement != null)
            {
                StringBuilder builderItemTemplate = new StringBuilder("<ItemTemplate>\r\n\t<table class=\"container_table\">");
                FormViewFieldsCollection fields = new FormViewFieldsCollection(null, typeof(FormViewField));
                int layoutcolnum = int.Parse(formViewElement.getAttribute("LayOutColNum", 0).ToString());
                for (int i = 0; i < SelectedMasterFields.Rows.Count; i++)
                {
                    string controlid = string.Empty;
                    if (i % layoutcolnum == 0 || layoutcolnum == 1)
                    {
                        builderItemTemplate.AppendLine("\t\t<tr>");
                    }

                    builderItemTemplate.AppendLine("\t\t\t<td>");
                    builderItemTemplate.AppendLine(string.Format("\t\t\t\t{0}", GetCaptionLabelXml(SelectedMasterFields.Rows[i], false)));
                    builderItemTemplate.AppendLine("\t\t\t</td>");

                    builderItemTemplate.AppendLine("\t\t\t<td>");
                    builderItemTemplate.AppendLine(string.Format("\t\t\t\t{0}", GetLabelXml(SelectedMasterFields.Rows[i])));
                    builderItemTemplate.AppendLine("\t\t\t</td>");
                    if (i % layoutcolnum == layoutcolnum - 1 || layoutcolnum == 1 || i == SelectedMasterFields.Rows.Count - 1)
                    {
                        builderItemTemplate.AppendLine("\t\t</tr>");
                    }

                    FormViewField field = new FormViewField();
                    field.FieldName = SelectedMasterFields.Rows[i]["ColumnName"].ToString();
                    field.ControlID = controlid;
                    fields.Add(field);
                }
                builderItemTemplate.AppendLine("\t</table>\r\n</ItemTemplate>");

        SetCollectionValue(formViewElement, typeof(WebFormView).GetProperty("Fields"), fields);
                SetTemplateValue(formViewElement, builderItemTemplate.ToString(), "ItemTemplate");
            }
        }

        private void SetTemplateValue(WebDevPage.IHTMLElement viewElement, string templatexml, string templatename)
        {
            if (viewElement != null)
            {
                if (templatexml.Length > 0)
                {
                    string html = viewElement.innerHTML;
                    int index = IndexOfBeginTag(html, templatename);
                    int length;
                    if (index > 0)
                    {
                        int indexend = IndexOfEndTag(html, templatename, out length);
                        html = html.Remove(indexend - index - length);
                    }
                    else
                    {
                        index = 0;
                    }
                    viewElement.innerHTML = html.Insert(index, templatexml);
                }
            }
        }

        private string GetLabelXml(DataRow dr)
        {
            String strLabel = String.Empty;

            //用上面的方法实现
            System.Web.UI.WebControls.Label label = new System.Web.UI.WebControls.Label();
            label.ID = string.Format("{0}Label", dr["ColumnName"].ToString());
            strLabel = GetControlXml(label, label.GetType().GetProperty("Text"), dr["ColumnName"].ToString(), dr["EditMask"].ToString());
            return strLabel;
        }

        private string GetCaptionLabelXml(DataRow dr, bool b)
        {
            System.Web.UI.WebControls.Label label = new System.Web.UI.WebControls.Label();
            label.Text = string.IsNullOrEmpty(dr["Caption"].ToString()) ? dr["ColumnName"].ToString() : dr["Caption"].ToString();
            if (b)
            {
                label.ID = string.Format("Caption{0}", dr["ColumnName"].ToString());
            }
            else
            {
                label.ID = string.Format("Caption{0}Label", dr["ColumnName"].ToString());
            }
            return GetControlXml(label);
        }

        private string GetControlXml(WebControl control)
        {
            return GetControlXml(control, null, string.Empty, string.Empty);
        }

        private string GetControlXml(WebControl control, PropertyInfo prop, string field, string format)
        {
            StringBuilder builder = new StringBuilder();
            if (control != null)
            {
                StringBuilder builderInnerHtml = new StringBuilder();
                Type controltype = control.GetType();
                if (controltype.Namespace == "Srvtools")
                {
                    builder.Append(string.Format("<{0}:{1} ID=\"{2}\" runat=\"server\" ", INFOLIGHTMARK, controltype.Name, control.ID));
                }
                else
                {
                    builder.Append(string.Format("<{0}:{1} ID=\"{2}\" runat=\"server\" ", "Asp", controltype.Name, control.ID));
                }
                if (prop != null)
                {
                    builder.Append(string.Format("{0}='<%# Bind(\"{1}\"", prop.Name, field));
                    if (!string.IsNullOrEmpty(format))
                    {
                        builder.Append(string.Format("{0}", FormatEditMask(format)));
                    }
                    builder.Append(") %>' ");
                }

                PropertyInfo[] infos = controltype.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
                for (int i = 0; i < infos.Length; i++)
                {
                    if (prop == null || (prop != null && prop.Name != infos[i].Name))
                    {
                        if (!IsVisibilityHidden(infos[i]))
                        {
                            if (infos[i].PropertyType == typeof(string) || infos[i].PropertyType == typeof(int) || infos[i].PropertyType == typeof(bool)
                                || infos[i].PropertyType.BaseType == typeof(Enum))
                            {
                                object value = infos[i].GetValue(control, null);
                                object defaultvalue = GetDefaultValue(infos[i]);
                                if (infos[i].CanWrite && value != defaultvalue)
                                {
                                    builder.Append(string.Format("{0}=\"{1}\" ", infos[i].Name, value));
                                }
                            }
                            else if (infos[i].PropertyType.BaseType == typeof(InfoOwnerCollection))
                            {
                                string collectionxml = GetCollectionXml(infos[i], (InfoOwnerCollection)infos[i].GetValue(control, null));
                                if (collectionxml.Length > 0)
                                {
                                    builderInnerHtml.AppendLine(collectionxml);
                                }
                            }
                        }
                    }
                }
                builder.AppendLine(">");
                builder.Append(builderInnerHtml.ToString());
                if (controltype.Namespace == "Srvtools")
                {
                    builder.AppendLine(string.Format("</{0}:{1}>", INFOLIGHTMARK, controltype.Name));
                }
                else
                {
                    builder.AppendLine(string.Format("</{0}:{1}>", "Asp", controltype.Name));
                }
            }
            return builder.ToString();
        }
#endif

        private string GetDDText(string ControlText, DataTable BlockItem, String TemplateName)
        {
            int Index1 = ControlText.IndexOf("Bind(\"");
            if (Index1 < 0)
            {
                foreach (DataRow dr in BlockItem.Rows)
                {
                    if (String.Compare(ControlText, dr["ColumnName"].ToString()) == 0)
                    {
                        String Description = dr["Caption"].ToString();
                        if (Description == null || Description == "")
                            Description = dr["ColumnName"].ToString();
                        String ExtraName = "";
                        if (TemplateName == "ItemTemplate")
                            ExtraName = "I";
                        return String.Format("<asp:Label ID=\"Caption{0}\" runat=\"server\" Text=\"{1}:\"></asp:Label>",
                            ExtraName + dr["ColumnName"].ToString(), Description);
                    }
                }
                return ControlText + ":";
            }
            else
            {
                return ControlText;
            }
        }

        private void GenDetailGridView(ClientParam cParam)
        {
#if VS90
            object oDetail = FDesignerDocument.webControls.item("Detail", 0);

            WebDevPage.IHTMLElement eDetail = null;
            WebDevPage.IHTMLElement eWebGridView1 = null;

            if (oDetail == null || !(oDetail is WebDevPage.IHTMLElement))
                return;
            eDetail = (WebDevPage.IHTMLElement)oDetail;
            eDetail.setAttribute("DataMember", cParam.ChildTableName, 0);

            object oWebGridView1 = FDesignerDocument.webControls.item("wgvDetail", 0);
            eWebGridView1 = (WebDevPage.IHTMLElement)oWebGridView1;
            eWebGridView1.setAttribute("DataSourceID", "Detail", 0);
            //eWebGridView1.setAttribute("DataMember", BlockItem.TableName, 0);

            //这里本来想再往下找Columns节点的,可是找不到,只能先这样写了
            StringBuilder sb = new StringBuilder(eWebGridView1.innerHTML);
            int idx = eWebGridView1.innerHTML.IndexOf("</Columns>");
            List<string> KeyFields = new List<string>();
            AddNewRowControlCollection controls = new AddNewRowControlCollection(null, typeof(AddNewRowControlItem));
            foreach (DataRow dr in SelectedDetailFields.Rows)
            {
                idx = sb.ToString().IndexOf("</Columns>");
                sb.Insert(idx, "\r            <asp:BoundField DataField=\"" + dr["ColumnName"].ToString() + "\" HeaderText=\"" + (string.IsNullOrEmpty(dr["Caption"].ToString()) ? dr["ColumnName"].ToString() : dr["Caption"].ToString()) + "\" SortExpression=\"" + dr["ColumnName"].ToString() + "\" />\r\n            ");
            }
            eWebGridView1.innerHTML = sb.ToString();
#else
            WebDataSource Detail = (WebDataSource)FPage.FindControl("Detail");
            Detail.DataMember = cParam.ChildTableName;
            IComponentChangeService FComponentChangeService = (IComponentChangeService)FDesignerHost.RootComponent.Site.GetService(typeof(IComponentChangeService));
            FComponentChangeService.OnComponentChanged(Detail, null, "", "M");
            WebGridView wgvDetail = (WebGridView)FPage.FindControl("wgvDetail");
            wgvDetail.Columns.Clear();

            //GridView
            if (wgvDetail != null)
            {
                foreach (DataRow dr in SelectedDetailFields.Rows)
                {
                    System.Web.UI.WebControls.BoundField aBoundField = new System.Web.UI.WebControls.BoundField();
                    aBoundField.DataField = dr["ColumnName"].ToString();
                    aBoundField.HeaderText = dr["Caption"].ToString();
                    aBoundField.SortExpression = dr["ColumnName"].ToString();
                    if (aBoundField.HeaderText == "")
                        aBoundField.HeaderText = dr["ColumnName"].ToString();
                    wgvDetail.Columns.Add(aBoundField);
                }
            }

            FComponentChangeService = (IComponentChangeService)FDesignerHost.RootComponent.Site.GetService(typeof(IComponentChangeService));
            FComponentChangeService.OnComponentChanged(wgvDetail, null, "", "M");
#endif
        }

        private void we()
        {

        }

        private String FormatEditMask(String editMask)
        {
            if (editMask != null && editMask != String.Empty)
                editMask = ",\"{0:" + editMask + "}\"";
            return editMask;
        }

        private void GenResx(WebDataSource aWebDataSource)
        {
            WebDataSet aWebDataSet = FDesignerHost.CreateComponent(typeof(WebDataSet), "WMaster") as WebDataSet;
            try
            {
                aWebDataSet.SetWizardDesignMode(true);
                aWebDataSet.RemoteName = this.tbProviderName.Text;
                aWebDataSet.PacketRecords = 100;
                aWebDataSet.Active = true;

                MyDesingerLoader ML = new MyDesingerLoader(cbAddToExistFolder.Text + @"\" + FResxFileName);
                FDesignerHost.AddService(typeof(IResourceService), ML);
                WebDataSetDesigner aDesigner = (WebDataSetDesigner)FDesignerHost.GetDesigner(aWebDataSet);

                aDesigner.Initialize(aWebDataSet);
                if (aDesigner != null)
                {
                    aDesigner.OnSave(FDesignWindow.Document, null);
                }

                WebDataSourceDesigner aWebDataSourceDesigner = FDesignerHost.GetDesigner(aWebDataSource) as WebDataSourceDesigner;
                if (aWebDataSourceDesigner != null)
                    aWebDataSourceDesigner.RefreshSchema(true);
            }
            finally
            {
                aWebDataSet.Dispose();
            }
        }

        private void GenWebEasilyReport(ClientParam cParam)
        {
#if VS90
            //WebDataSource Master = (WebDataSource)FPage.FindControl("Master");
            if (this.cbTemplate.Text == "WebEasilyReportSingle")
            {
                Infolight.EasilyReportTools.DataSourceItemCollection dataSourceItemCollection = new Infolight.EasilyReportTools.DataSourceItemCollection();
                Infolight.EasilyReportTools.DataSourceItem dataSourceItem = new Infolight.EasilyReportTools.DataSourceItem();

                DataTable dt = WebDataSet.CreateWebDataSet("WMaster").RealDataSet.Tables[cParam.TableName].Copy();

                foreach (DataRow dr in SelectedMasterFields.Rows)
                {
                    Infolight.EasilyReportTools.FieldItem fieldItem = new Infolight.EasilyReportTools.FieldItem();
                    fieldItem.ColumnName = dr["ColumnName"].ToString();
                    fieldItem.Caption = dr["Caption"].ToString();
                    if (fieldItem.Caption == String.Empty)
                        fieldItem.Caption = dr["ColumnName"].ToString();
                    fieldItem.Width = Convert.ToInt32(dr["Width"]);
                    if (CheckNum(dt, fieldItem.ColumnName))
                    {
                        fieldItem.CaptionAlignment = HorizontalAlignment.Right;
                        fieldItem.ColumnAlignment = HorizontalAlignment.Right;
                    }
                    dataSourceItem.Fields.Add(fieldItem);
                }

                dataSourceItemCollection.Add(dataSourceItem);

                foreach (DataRow dr in SelectedMasterGroups.Rows)
                {
                    foreach (Infolight.EasilyReportTools.FieldItem item in dataSourceItem.Fields)
                    {
                        if (item.ColumnName == dr["ColumnName"].ToString())
                        {
                            item.Group = Infolight.EasilyReportTools.FieldItem.GroupType.Normal;
                            item.Order = Infolight.EasilyReportTools.FieldItem.OrderType.Ascend;
                        }
                    }
                }

                WebDevPage.IHTMLElement WebEasilyReport1 = (WebDevPage.IHTMLElement)FDesignerDocument.webControls.item("WebEasilyReport1", 0);
                if (WebEasilyReport1 != null)
                {
                    WebEasilyReport1.setAttribute("ReportID", cParam.FormName, 0);
                    WebEasilyReport1.setAttribute("ReportName", cParam.FormTitle, 0);

                    SetCollectionValue(WebEasilyReport1, typeof(Infolight.EasilyReportTools.WebEasilyReport).GetProperty("FieldItems"), dataSourceItemCollection);
                }
            }
            else if (this.cbTemplate.Text == "WebEasilyReportMasterDetail1")
            {
                int i = 0;
                Infolight.EasilyReportTools.ReportItemCollection aReportItemCollection = new Infolight.EasilyReportTools.ReportItemCollection();
                foreach (DataRow dr in SelectedMasterFields.Rows)
                {
                    i++;
                    Infolight.EasilyReportTools.ReportDataSourceItem reportDataSourceItem = new Infolight.EasilyReportTools.ReportDataSourceItem();
                    if ((i - 1) % 2 == 0)
                    {
                        reportDataSourceItem.NewLine = true;
                        reportDataSourceItem.Cells = 2;
                    }
                    else
                    {
                        reportDataSourceItem.Cells = 0;
                    }
                    reportDataSourceItem.ColumnName = dr["ColumnName"].ToString();
                    reportDataSourceItem.Format = "{0}";
                    if (dr["Caption"].ToString() == String.Empty)
                        reportDataSourceItem.Format = dr["ColumnName"].ToString() + ": " + reportDataSourceItem.Format;
                    else
                        reportDataSourceItem.Format = dr["Caption"].ToString() + ": " + reportDataSourceItem.Format;
                    aReportItemCollection.Add(reportDataSourceItem);
                }

                Infolight.EasilyReportTools.DataSourceItemCollection dataSourceItemCollection = new Infolight.EasilyReportTools.DataSourceItemCollection();
                Infolight.EasilyReportTools.DataSourceItem dataSourceItem = new Infolight.EasilyReportTools.DataSourceItem();

                DataTable dt = WebDataSet.CreateWebDataSet("WMaster").RealDataSet.Tables[cParam.ChildTableName].Copy();

                foreach (DataRow dr in SelectedDetailFields.Rows)
                {
                    Infolight.EasilyReportTools.FieldItem fieldItem = new Infolight.EasilyReportTools.FieldItem();
                    fieldItem.ColumnName = dr["ColumnName"].ToString();
                    fieldItem.Caption = dr["Caption"].ToString();
                    if (fieldItem.Caption == String.Empty)
                        fieldItem.Caption = dr["ColumnName"].ToString();
                    fieldItem.Width = Convert.ToInt32(dr["Width"]);
                    if (CheckNum(dt, fieldItem.ColumnName))
                    {
                        fieldItem.CaptionAlignment = HorizontalAlignment.Right;
                        fieldItem.ColumnAlignment = HorizontalAlignment.Right;
                    }
                    dataSourceItem.Fields.Add(fieldItem);
                }
                dataSourceItemCollection.Add(dataSourceItem);
                
                foreach (DataRow dr in SelectedDetailGroups.Rows)
                {
                    foreach (Infolight.EasilyReportTools.FieldItem item in dataSourceItem.Fields)
                    {
                        if (item.ColumnName == dr["ColumnName"].ToString())
                        {
                            item.Group = Infolight.EasilyReportTools.FieldItem.GroupType.Normal;
                            item.Order = Infolight.EasilyReportTools.FieldItem.OrderType.Ascend;
                        }
                    }
                }

                WebDevPage.IHTMLElement WebEasilyReport1 = (WebDevPage.IHTMLElement)FDesignerDocument.webControls.item("WebEasilyReport1", 0);
                if (WebEasilyReport1 != null)
                {
                    WebEasilyReport1.setAttribute("ReportID", cParam.FormName, 0);
                    WebEasilyReport1.setAttribute("ReportName", cParam.FormTitle, 0);

                    SetCollectionValue(WebEasilyReport1, typeof(Infolight.EasilyReportTools.WebEasilyReport).GetProperty("HeaderItems"), aReportItemCollection);
                    SetCollectionValue(WebEasilyReport1, typeof(Infolight.EasilyReportTools.WebEasilyReport).GetProperty("FieldItems"), dataSourceItemCollection);
                }
            }
#else
            Object WebEasilyReport1 = FPage.FindControl("WebEasilyReport1");
            WebEasilyReport1.GetType().GetProperty("ReportID").SetValue(WebEasilyReport1, cParam.FormName, null);
            WebEasilyReport1.GetType().GetProperty("ReportName").SetValue(WebEasilyReport1, cParam.FormTitle, null);

            WebDataSource Master = (WebDataSource)FPage.FindControl("Master");

            if (this.cbTemplate.Text == "WebEasilyReportSingle")
            {
                DataTable dt = WebDataSet.CreateWebDataSet("WMaster").RealDataSet.Tables[cParam.TableName].Copy();

                Type datasourceItemType = WebEasilyReport1.GetType().GetProperty("FieldItems").PropertyType.GetProperty("Item").PropertyType;//找出DataSourceItem的类型
                object dataSourceItem = Activator.CreateInstance(datasourceItemType);//创建DataSourceItem类型的实例

                IList iDataSourceItemFields = null;
                foreach (DataRow dr in SelectedMasterFields.Rows)
                {
                    Type fieldsType = WebEasilyReport1.GetType().Assembly.GetType("Infolight.EasilyReportTools.FieldItem");//找出FieldItem的类型
                    object fieldsItem = Activator.CreateInstance(fieldsType);//创建FieldItem类型的实例

                    fieldsItem.GetType().GetProperty("ColumnName").SetValue(fieldsItem, dr["ColumnName"].ToString(), null);
                    fieldsItem.GetType().GetProperty("Caption").SetValue(fieldsItem, dr["Caption"].ToString(), null);
                    if (dr["Caption"].ToString() == String.Empty)
                        fieldsItem.GetType().GetProperty("Caption").SetValue(fieldsItem, dr["ColumnName"].ToString(), null);
                    fieldsItem.GetType().GetProperty("Width").SetValue(fieldsItem, Convert.ToInt32(dr["Width"]), null);

                    if (CheckNum(dt, dr["ColumnName"].ToString()))
                    {
                        fieldsItem.GetType().GetProperty("CaptionAlignment").SetValue(fieldsItem, HorizontalAlignment.Right, null);
                        fieldsItem.GetType().GetProperty("ColumnAlignment").SetValue(fieldsItem, HorizontalAlignment.Right, null);
                    }

                    iDataSourceItemFields = dataSourceItem.GetType().GetProperty("Fields").GetValue(dataSourceItem, null) as IList;//得到一个FieldItemCollection
                    iDataSourceItemFields.Add(fieldsItem);
                }

                foreach (DataRow dr in SelectedMasterGroups.Rows)
                {
                    for (int i = 0; i < iDataSourceItemFields.Count; i++)
                    {
                        String iFieldsColumnName = iDataSourceItemFields[i].GetType().GetProperty("ColumnName").GetValue(iDataSourceItemFields[i], null).ToString();
                        if (iFieldsColumnName == dr["ColumnName"].ToString())
                        {
                            //item.Order = Infolight.EasilyReportTools.FieldItem.OrderType.Ascend;
                            iDataSourceItemFields[i].GetType().GetProperty("Order").SetValue(iDataSourceItemFields[i], iDataSourceItemFields[i].GetType().GetProperty("Order").PropertyType.GetField("Ascend").GetValue(iDataSourceItemFields[i]), null);
                            //item.Group = Infolight.EasilyReportTools.FieldItem.GroupType.Normal;
                            iDataSourceItemFields[i].GetType().GetProperty("Group").SetValue(iDataSourceItemFields[i], iDataSourceItemFields[i].GetType().GetProperty("Group").PropertyType.GetField("Normal").GetValue(iDataSourceItemFields[i]), null);
                        }
                    }
                }

                IList iFieldItems = WebEasilyReport1.GetType().GetProperty("FieldItems").GetValue(WebEasilyReport1, null) as IList;
                iFieldItems.Add(dataSourceItem);
            }
            else if (this.cbTemplate.Text == "WebEasilyReportMasterDetail1")
            {
                int i = 0;
                foreach (DataRow dr in SelectedMasterFields.Rows)
                {
                    i++;

                    Type reportDataSourceItemType = WebEasilyReport1.GetType().Assembly.GetType("Infolight.EasilyReportTools.ReportDataSourceItem");
                    object aReportDataSourceItem = Activator.CreateInstance(reportDataSourceItemType);
                    if ((i - 1) % 2 == 0)
                    {
                        aReportDataSourceItem.GetType().GetProperty("NewLine").SetValue(aReportDataSourceItem, true, null);
                        aReportDataSourceItem.GetType().GetProperty("Cells").SetValue(aReportDataSourceItem, 2, null);
                    }
                    else
                    {
                        aReportDataSourceItem.GetType().GetProperty("Cells").SetValue(aReportDataSourceItem, 0, null);
                    }
                    aReportDataSourceItem.GetType().GetProperty("ColumnName").SetValue(aReportDataSourceItem, dr["ColumnName"].ToString(), null);
                    if (dr["Caption"].ToString() == String.Empty)
                        aReportDataSourceItem.GetType().GetProperty("Format").SetValue(aReportDataSourceItem, dr["ColumnName"].ToString() + ": {0}", null);
                    else
                        aReportDataSourceItem.GetType().GetProperty("Format").SetValue(aReportDataSourceItem, dr["Caption"].ToString() + ": {0}", null);

                    IList iHeaderItems = WebEasilyReport1.GetType().GetProperty("HeaderItems").GetValue(WebEasilyReport1, null) as IList;
                    iHeaderItems.Add(aReportDataSourceItem);
                }

                DataTable dt = WebDataSet.CreateWebDataSet("WMaster").RealDataSet.Tables[cParam.ChildTableName].Copy();

                Type datasourceItemType = WebEasilyReport1.GetType().GetProperty("FieldItems").PropertyType.GetProperty("Item").PropertyType;//找出DataSourceItem的类型
                object dataSourceItem = Activator.CreateInstance(datasourceItemType);//创建DataSourceItem类型的实例

                IList iDataSourceItemFields = null;
                foreach (DataRow dr in SelectedDetailFields.Rows)
                {
                    Type fieldsType = WebEasilyReport1.GetType().Assembly.GetType("Infolight.EasilyReportTools.FieldItem");//找出FieldItem的类型
                    object fieldsItem = Activator.CreateInstance(fieldsType);//创建FieldItem类型的实例
                    fieldsItem.GetType().GetProperty("ColumnName").SetValue(fieldsItem, dr["ColumnName"].ToString(), null);
                    fieldsItem.GetType().GetProperty("Caption").SetValue(fieldsItem, dr["Caption"].ToString(), null);
                    if (dr["Caption"].ToString() == String.Empty)
                        fieldsItem.GetType().GetProperty("Caption").SetValue(fieldsItem, dr["ColumnName"].ToString(), null);
                    fieldsItem.GetType().GetProperty("Width").SetValue(fieldsItem, Convert.ToInt32(dr["Width"]), null);

                    if (CheckNum(dt, dr["ColumnName"].ToString()))
                    {
                        fieldsItem.GetType().GetProperty("CaptionAlignment").SetValue(fieldsItem, HorizontalAlignment.Right, null);
                        fieldsItem.GetType().GetProperty("ColumnAlignment").SetValue(fieldsItem, HorizontalAlignment.Right, null);
                    }

                    iDataSourceItemFields = dataSourceItem.GetType().GetProperty("Fields").GetValue(dataSourceItem, null) as IList;//得到一个FieldItemCollection
                    iDataSourceItemFields.Add(fieldsItem);
                }

                foreach (DataRow dr in SelectedDetailGroups.Rows)
                {
                    for (int j = 0; j < iDataSourceItemFields.Count; j++)
                    {
                        String iFieldsColumnName = iDataSourceItemFields[j].GetType().GetProperty("ColumnName").GetValue(iDataSourceItemFields[j], null).ToString();
                        if (iFieldsColumnName == dr["ColumnName"].ToString())
                        {
                            //item.Order = Infolight.EasilyReportTools.FieldItem.OrderType.Ascend;
                            iDataSourceItemFields[j].GetType().GetProperty("Order").SetValue(iDataSourceItemFields[j], iDataSourceItemFields[j].GetType().GetProperty("Order").PropertyType.GetField("Ascend").GetValue(iDataSourceItemFields[j]), null);
                            //item.Group = Infolight.EasilyReportTools.FieldItem.GroupType.Normal;
                            iDataSourceItemFields[j].GetType().GetProperty("Group").SetValue(iDataSourceItemFields[j], iDataSourceItemFields[j].GetType().GetProperty("Group").PropertyType.GetField("Normal").GetValue(iDataSourceItemFields[j]), null);
                        }
                    }
                }

                IList iFieldItems = WebEasilyReport1.GetType().GetProperty("FieldItems").GetValue(WebEasilyReport1, null) as IList;
                iFieldItems.Add(dataSourceItem);
            }

            IComponentChangeService FComponentChangeService = (IComponentChangeService)FDesignerHost.RootComponent.Site.GetService(typeof(IComponentChangeService));
            NotifyRefresh(200);
            FComponentChangeService.OnComponentChanged(WebEasilyReport1, null, "", "M");
#endif
        }

        private bool CheckNum(DataTable dt, string colName)
        {
            bool resValue = false;
            switch (dt.Columns[colName].DataType.Name)
            {
                case "String":
                case "DateTime":
                case "Boolean":
                    resValue = false;
                    break;

                case "Int16":
                case "Int32":
                case "Int64":
                case "Single":
                case "Double":
                case "Decimal":
                case "UInt16":
                case "UInt32":
                case "UInt64":
                    resValue = true;
                    break;
            }

            return resValue;
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
                    GenMainBlockControl(cParam);
                    WinClientCreator.GetWinQuery(FRootForm, cParam, SetFields(cParam, cParam.TableName, cParam.RealTableName));
                    GenEasilyReport(cParam);
                    GlobalProject.Save(GlobalProject.FullName);
                    GenSuccess = true;
                }
                catch (Exception exception2)
                {
                    MessageBox.Show(exception2.Message);
                    MessageBox.Show(exception2.StackTrace);
                    
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

        private void GenMainBlockControl(ClientParam cParam)
        {
            if (this.cbTemplate.Text == "CEasilyReportSingle1")
            {
                SetSelectedMasterFields(cParam);
                SetSelectedMasterGroups(cParam);
                GenMasterSingle1(cParam);
            }
            else if (this.cbTemplate.Text == "CEasilyReportMasterDetail1")
            {
                SetSelectedMasterFields(cParam);
                SetSelectedMasterGroups(cParam);
                SetSelectedDetailFields(cParam);
                SetSelectedDetailGroups(cParam);
                GenMasterDetail1(cParam);
            }
        }

        private void GenMasterSingle1(ClientParam cParam)
        {
            IComponent aDataSet = FRootForm.Container.Components["Query"];
            if (aDataSet != null)
                FDesignerHost.DestroyComponent(aDataSet);
            IComponent aSource = FRootForm.Container.Components["ibsQuery"];
            if (aSource != null)
                FDesignerHost.DestroyComponent(aSource);

            InfoDataSet FDataSet = FDesignerHost.CreateComponent(typeof(InfoDataSet), "id" + cParam.TableName) as InfoDataSet;
            FDataSet.RemoteName = cParam.ProviderName;
            FDataSet.Active = true;
            FDataSet.AlwaysClose = true;
            InfoBindingSource MainBindingSource = FDesignerHost.CreateComponent(typeof(InfoBindingSource), "ibs" + cParam.TableName) as InfoBindingSource;
            MainBindingSource.DataSource = FDataSet;
            MainBindingSource.DataMember = FDataSet.RealDataSet.Tables[0].TableName;
            SplitContainer SplitContainer1 = FRootForm.Controls["splitContainer1"] as SplitContainer;
            InfoDataGridView Grid = SplitContainer1.Panel2.Controls["InfoDataGridView1"] as InfoDataGridView;
            Grid.DataSource = MainBindingSource;
            Grid.Columns.Clear();
            DataGridViewTextBoxColumn Column;

            for (int i = 0; i < SelectedMasterFields.Rows.Count; i++)
            {
                Column = FDesignerHost.CreateComponent(typeof(DataGridViewTextBoxColumn), "dgc" + cParam.TableName + SelectedMasterFields.Rows[i]["ColumnName"].ToString()) as DataGridViewTextBoxColumn;
                Column.DataPropertyName = SelectedMasterFields.Rows[i]["ColumnName"].ToString();
                Column.HeaderText = SelectedMasterFields.Rows[i]["Caption"].ToString();
                Column.MaxInputLength = Convert.ToInt32(SelectedMasterFields.Rows[i]["Width"]);
                if (Column.HeaderText.Trim() == "")
                    Column.HeaderText = SelectedMasterFields.Rows[i]["ColumnName"].ToString();
                Grid.Columns.Add(Column);
            }
        }

        private void GenMasterDetail1(ClientParam cParam)
        {
            IComponent aDataSet = FRootForm.Container.Components["Query"];
            if (aDataSet != null)
                FDesignerHost.DestroyComponent(aDataSet);
            IComponent aSource = FRootForm.Container.Components["ibsQuery"];
            if (aSource != null)
                FDesignerHost.DestroyComponent(aSource);

            InfoDataSet FDataSet = FDesignerHost.CreateComponent(typeof(InfoDataSet), "id" + cParam.TableName) as InfoDataSet;
            FDataSet.RemoteName = cParam.ProviderName;
            FDataSet.Active = true;
            FDataSet.AlwaysClose = true;
            InfoBindingSource MainBindingSource = FDesignerHost.CreateComponent(typeof(InfoBindingSource), "ibs" + cParam.TableName) as InfoBindingSource;
            MainBindingSource.DataSource = FDataSet;
            MainBindingSource.DataMember = FDataSet.RealDataSet.Tables[0].TableName;
            InfoBindingSource DetailBindingSource = FDesignerHost.CreateComponent(typeof(InfoBindingSource), "ibs" + cParam.ChildTableName) as InfoBindingSource;
            DetailBindingSource.DataSource = MainBindingSource;
            DetailBindingSource.DataMember = FDataSet.RealDataSet.Relations[0].RelationName;

            SplitContainer SplitContainer1 = FRootForm.Controls["scMaster"] as SplitContainer;
            InfoDataGridView MasterGrid = SplitContainer1.Panel1.Controls["InfoDataGridView1"] as InfoDataGridView;
            MasterGrid.DataSource = MainBindingSource;
            MasterGrid.Columns.Clear();
            DataGridViewTextBoxColumn Column;

            for (int i = 0; i < SelectedMasterFields.Rows.Count; i++)
            {
                Column = FDesignerHost.CreateComponent(typeof(DataGridViewTextBoxColumn), "dgc" + cParam.TableName + SelectedMasterFields.Rows[i]["ColumnName"].ToString()) as DataGridViewTextBoxColumn;
                Column.DataPropertyName = SelectedMasterFields.Rows[i]["ColumnName"].ToString();
                Column.HeaderText = SelectedMasterFields.Rows[i]["Caption"].ToString();
                Column.MaxInputLength = Convert.ToInt32(SelectedMasterFields.Rows[i]["Width"]);
                if (Column.HeaderText.Trim() == "")
                    Column.HeaderText = SelectedMasterFields.Rows[i]["ColumnName"].ToString();
                MasterGrid.Columns.Add(Column);
            }

            InfoDataGridView DetailGrid = SplitContainer1.Panel2.Controls["InfoDataGridView2"] as InfoDataGridView;
            DetailGrid.DataSource = DetailBindingSource;
            for (int i = 0; i < SelectedDetailFields.Rows.Count; i++)
            {
                Column = FDesignerHost.CreateComponent(typeof(DataGridViewTextBoxColumn), "dgc" + cParam.ChildTableName + SelectedDetailFields.Rows[i]["ColumnName"].ToString()) as DataGridViewTextBoxColumn;
                Column.DataPropertyName = SelectedDetailFields.Rows[i]["ColumnName"].ToString();
                Column.HeaderText = SelectedDetailFields.Rows[i]["Caption"].ToString();
                Column.MaxInputLength = Convert.ToInt32(SelectedDetailFields.Rows[i]["Width"]);
                if (Column.HeaderText.Trim() == "")
                    Column.HeaderText = SelectedDetailFields.Rows[i]["ColumnName"].ToString();
                DetailGrid.Columns.Add(Column);
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
            cParam.FormName = tbFormName.Text;
            cParam.FormTitle = tbFormTitle.Text;
            cParam.IsMasterDetails = cbIsMasterDetails.Checked;
            cParam.ProviderName = tbProviderName.Text;
            cParam.TableName = tbTableName.Text;
            cParam.RealTableName = tbTableNameF.Text;
            cParam.ChildTableName = tbChildTableName.Text;

            if (cParam.ChildTableName != null && cParam.ChildTableName != String.Empty)
            {
                String ModuleName = FInfoDataSet.RemoteName.Substring(0, FInfoDataSet.RemoteName.IndexOf('.'));
                String SolutionName = System.IO.Path.GetFileNameWithoutExtension(_dte2.Solution.FullName);
                cParam.ChildRealTableName = CliUtils.GetTableName(ModuleName, cParam.ChildTableName, SolutionName);
            }

            cParam.SelectAlias = this.cmbSelectAlias.Text;
            cParam.ClientType = (ClientType)this.cmbDataBaseType.SelectedIndex;
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

        private void GenEasilyReport(ClientParam cParam)
        {
            Object aEasilyReport = FRootForm.Container.Components["easilyreport1"];
            if (this.cbTemplate.Text == "CEasilyReportSingle1")
            {
                Type bindingSourceItemType = aEasilyReport.GetType().GetProperty("DataSource").PropertyType.GetProperty("Item").PropertyType;
                object aBindingSourceItem = Activator.CreateInstance(bindingSourceItemType);
                aBindingSourceItem.GetType().GetProperty("BindingSource").SetValue(aBindingSourceItem, FRootForm.Container.Components["ibs" + cParam.TableName], null);

                IList iDataSource = aEasilyReport.GetType().GetProperty("DataSource").GetValue(aEasilyReport, null) as IList;
                iDataSource.Add(aBindingSourceItem);

                aEasilyReport.GetType().GetProperty("ReportID").SetValue(aEasilyReport, cParam.FormName, null);
                aEasilyReport.GetType().GetProperty("ReportName").SetValue(aEasilyReport, cParam.FormTitle, null);

                DataTable dt = ((DataView)(FRootForm.Container.Components["ibs" + cParam.TableName] as InfoBindingSource).List).Table;

                Type datasourceItemType = aEasilyReport.GetType().GetProperty("FieldItems").PropertyType.GetProperty("Item").PropertyType;//找出DataSourceItem的类型
                object dataSourceItem = Activator.CreateInstance(datasourceItemType);//创建DataSourceItem类型的实例

                IList iDataSourceItemFields = null;
                foreach (DataRow dr in SelectedMasterFields.Rows)
                {
                    Type fieldsType = aEasilyReport.GetType().Assembly.GetType("Infolight.EasilyReportTools.FieldItem");//找出FieldItem的类型
                    object fieldsItem = Activator.CreateInstance(fieldsType);//创建FieldItem类型的实例

                    fieldsItem.GetType().GetProperty("ColumnName").SetValue(fieldsItem, dr["ColumnName"].ToString(), null);
                    fieldsItem.GetType().GetProperty("Caption").SetValue(fieldsItem, dr["Caption"].ToString(), null);
                    if (dr["Caption"].ToString() == String.Empty)
                        fieldsItem.GetType().GetProperty("Caption").SetValue(fieldsItem, dr["ColumnName"].ToString(), null);
                    fieldsItem.GetType().GetProperty("Width").SetValue(fieldsItem, Convert.ToInt32(dr["Width"]), null);

                    if (CheckNum(dt, dr["ColumnName"].ToString()))
                    {
                        fieldsItem.GetType().GetProperty("CaptionAlignment").SetValue(fieldsItem, HorizontalAlignment.Right, null);
                        fieldsItem.GetType().GetProperty("ColumnAlignment").SetValue(fieldsItem, HorizontalAlignment.Right, null);
                    }

                    iDataSourceItemFields = dataSourceItem.GetType().GetProperty("Fields").GetValue(dataSourceItem, null) as IList;//得到一个FieldItemCollection
                    iDataSourceItemFields.Add(fieldsItem);
                }

                foreach (DataRow dr in SelectedMasterGroups.Rows)
                {
                    for (int i = 0; i < iDataSourceItemFields.Count; i++)
                    {
                        String iFieldsColumnName = iDataSourceItemFields[i].GetType().GetProperty("ColumnName").GetValue(iDataSourceItemFields[i], null).ToString();
                        if (iFieldsColumnName == dr["ColumnName"].ToString())
                        {
                            //item.Order = Infolight.EasilyReportTools.FieldItem.OrderType.Ascend;
                            iDataSourceItemFields[i].GetType().GetProperty("Order").SetValue(iDataSourceItemFields[i], iDataSourceItemFields[i].GetType().GetProperty("Order").PropertyType.GetField("Ascend").GetValue(iDataSourceItemFields[i]), null);
                            //item.Group = Infolight.EasilyReportTools.FieldItem.GroupType.Normal;
                            iDataSourceItemFields[i].GetType().GetProperty("Group").SetValue(iDataSourceItemFields[i], iDataSourceItemFields[i].GetType().GetProperty("Group").PropertyType.GetField("Normal").GetValue(iDataSourceItemFields[i]), null);
                        }
                    }
                }

                IList iFieldItems = aEasilyReport.GetType().GetProperty("FieldItems").GetValue(aEasilyReport, null) as IList;
                iFieldItems.Add(dataSourceItem);
            }
            else if (this.cbTemplate.Text == "CEasilyReportMasterDetail1")
            {
                aEasilyReport.GetType().GetProperty("HeaderBindingSource").SetValue(aEasilyReport, FRootForm.Container.Components["ibs" + cParam.TableName], null);

                Type bindingSourceItemType = aEasilyReport.GetType().GetProperty("DataSource").PropertyType.GetProperty("Item").PropertyType;
                object aBindingSourceItem = Activator.CreateInstance(bindingSourceItemType);
                aBindingSourceItem.GetType().GetProperty("BindingSource").SetValue(aBindingSourceItem, FRootForm.Container.Components["ibs" + cParam.ChildTableName], null);

                IList iDataSource = aEasilyReport.GetType().GetProperty("DataSource").GetValue(aEasilyReport, null) as IList;
                iDataSource.Add(aBindingSourceItem);

                aEasilyReport.GetType().GetProperty("ReportID").SetValue(aEasilyReport, cParam.FormName, null);
                aEasilyReport.GetType().GetProperty("ReportName").SetValue(aEasilyReport, cParam.FormTitle, null);

                DataTable dt = ((InfoDataSet)(FRootForm.Container.Components["ibs" + cParam.TableName] as InfoBindingSource).DataSource).RealDataSet.Tables[cParam.ChildTableName];

                int i = 0;
                foreach (DataRow dr in SelectedMasterFields.Rows)
                {
                    i++;

                    Type reportDataSourceItemType = aEasilyReport.GetType().Assembly.GetType("Infolight.EasilyReportTools.ReportDataSourceItem");
                    object aReportDataSourceItem = Activator.CreateInstance(reportDataSourceItemType);
                    if ((i - 1) % 2 == 0)
                    {
                        aReportDataSourceItem.GetType().GetProperty("NewLine").SetValue(aReportDataSourceItem, true, null);
                        aReportDataSourceItem.GetType().GetProperty("Cells").SetValue(aReportDataSourceItem, 2, null);
                    }
                    else
                    {
                        aReportDataSourceItem.GetType().GetProperty("Cells").SetValue(aReportDataSourceItem, 0, null);
                    }
                    aReportDataSourceItem.GetType().GetProperty("ColumnName").SetValue(aReportDataSourceItem, dr["ColumnName"].ToString(), null);
                    if (dr["Caption"].ToString() == String.Empty)
                        aReportDataSourceItem.GetType().GetProperty("Format").SetValue(aReportDataSourceItem, dr["ColumnName"].ToString() + ": {0}", null);
                    else
                        aReportDataSourceItem.GetType().GetProperty("Format").SetValue(aReportDataSourceItem, dr["Caption"].ToString() + ": {0}", null);

                    IList iHeaderItems = aEasilyReport.GetType().GetProperty("HeaderItems").GetValue(aEasilyReport, null) as IList;
                    iHeaderItems.Add(aReportDataSourceItem);
                }

                Type datasourceItemType = aEasilyReport.GetType().GetProperty("FieldItems").PropertyType.GetProperty("Item").PropertyType;//找出DataSourceItem的类型
                object dataSourceItem = Activator.CreateInstance(datasourceItemType);//创建DataSourceItem类型的实例

                IList iDataSourceItemFields = null;
                foreach (DataRow dr in SelectedDetailFields.Rows)
                {
                    Type fieldsType = aEasilyReport.GetType().Assembly.GetType("Infolight.EasilyReportTools.FieldItem");//找出FieldItem的类型
                    object fieldsItem = Activator.CreateInstance(fieldsType);//创建FieldItem类型的实例
                    fieldsItem.GetType().GetProperty("ColumnName").SetValue(fieldsItem, dr["ColumnName"].ToString(), null);
                    fieldsItem.GetType().GetProperty("Caption").SetValue(fieldsItem, dr["Caption"].ToString(), null);
                    if (dr["Caption"].ToString() == String.Empty)
                        fieldsItem.GetType().GetProperty("Caption").SetValue(fieldsItem, dr["ColumnName"].ToString(), null);
                    fieldsItem.GetType().GetProperty("Width").SetValue(fieldsItem, Convert.ToInt32(dr["Width"]), null);

                    if (CheckNum(dt, dr["ColumnName"].ToString()))
                    {
                        fieldsItem.GetType().GetProperty("CaptionAlignment").SetValue(fieldsItem, HorizontalAlignment.Right, null);
                        fieldsItem.GetType().GetProperty("ColumnAlignment").SetValue(fieldsItem, HorizontalAlignment.Right, null);
                    }

                    iDataSourceItemFields = dataSourceItem.GetType().GetProperty("Fields").GetValue(dataSourceItem, null) as IList;//得到一个FieldItemCollection
                    iDataSourceItemFields.Add(fieldsItem);
                }

                foreach (DataRow dr in SelectedDetailGroups.Rows)
                {
                    for (int j = 0; j < iDataSourceItemFields.Count; j++)
                    {
                        String iFieldsColumnName = iDataSourceItemFields[j].GetType().GetProperty("ColumnName").GetValue(iDataSourceItemFields[j], null).ToString();
                        if (iFieldsColumnName == dr["ColumnName"].ToString())
                        {
                            //item.Order = Infolight.EasilyReportTools.FieldItem.OrderType.Ascend;
                            iDataSourceItemFields[j].GetType().GetProperty("Order").SetValue(iDataSourceItemFields[j], iDataSourceItemFields[j].GetType().GetProperty("Order").PropertyType.GetField("Ascend").GetValue(iDataSourceItemFields[j]), null);
                            //item.Group = Infolight.EasilyReportTools.FieldItem.GroupType.Normal;
                            iDataSourceItemFields[j].GetType().GetProperty("Group").SetValue(iDataSourceItemFields[j], iDataSourceItemFields[j].GetType().GetProperty("Group").PropertyType.GetField("Normal").GetValue(iDataSourceItemFields[j]), null);
                        }
                    }
                }

                IList iFieldItems = aEasilyReport.GetType().GetProperty("FieldItems").GetValue(aEasilyReport, null) as IList;
                iFieldItems.Add(dataSourceItem);
            }
        }

        private String FResxFileName;
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
                string BaseFormName = cbTemplate.Text;
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
                FResxFileName = P2.Name;              
            }
            return true;
        }

        private string GetTemplatePath()
        {
            string TemplatePath = "\\Template";
            TemplatePath = EEPRegistry.WebClient + TemplatePath;
            return TemplatePath;
        }

        private void GetWebDesignerHost(ProjectItem projItem)
        {
#if VS90
            //FDesignWindow = projItem.Open("{7651A702-06E5-11D1-8EBD-00A0C90F26EA}");
            FDesignWindow = projItem.Open(Constants.vsViewKindDesigner);
            FDesignWindow.Activate();

            HTMLWindow W = (HTMLWindow)FDesignWindow.Object;

            W.CurrentTab = vsHTMLTabs.vsHTMLTabsDesign;
            if (W.CurrentTabObject is WebDevPage.DesignerDocument)
            {
                FDesignerDocument = W.CurrentTabObject as WebDevPage.DesignerDocument; 
            }
#else
            FDesignWindow = projItem.Open("{00000000-0000-0000-0000-000000000000}");
            FDesignWindow.Activate();
            FDesignWindow = projItem.Open("{7651A702-06E5-11D1-8EBD-00A0C90F26EA}");
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
                    //NotifyRefresh(200);
                    Application.DoEvents();
                }
                finally
                {
                    Marshal.Release(pObject);
                }
            }
#endif
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

#if VS90
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
#else
            WebDataSource Master = (WebDataSource)FPage.FindControl("Master");
            Master.DataMember = cParam.TableName;
            IComponentChangeService FComponentChangeService = (IComponentChangeService)FDesignerHost.RootComponent.Site.GetService(typeof(IComponentChangeService));
            FComponentChangeService.OnComponentChanged(Master, null, "", "M");

            if (isMasterDetails)
            {
                WebDataSource Detail = (WebDataSource)FPage.FindControl("Detail");
                string[] DetailTableName = cParam.ChildTableName.Split(',');
                Detail.DataMember = DetailTableName[0];
                FComponentChangeService.OnComponentChanged(Detail, null, "", "M");
            }
            WebClientCreator.SaveWebDataSet(WizardDataSet, WizardDetailDataSet, projItem);
#endif

        }

        private void NotifyRefresh(uint SleepTime)
        {
            return;
        }

        public void GenWebQuery(ClientParam cParam)
        {
            List<TBlockFieldItem> listfield = SetFields(cParam, cParam.TableName, cParam.RealTableName);
            foreach (TBlockFieldItem aFieldItem in listfield)
            {
                CreateWebQueryField(aFieldItem, "", true);
            }
        }

        private List<TBlockFieldItem> SetFields(ClientParam cParam, String tableName, String realTableName)
        {
            DataTable dtDD = ReportCreator.GetDDTable(cParam.ClientType, cParam.SelectAlias, realTableName);
            DataTable dtTableSchema = FInfoDataSet.RealDataSet.Tables[tableName];
            List<TBlockFieldItem> list = new List<TBlockFieldItem>();
            if (dtDD.Rows.Count > 0)
            {
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
                        aBlockFieldItem.Length = Convert.ToInt32(DR["FIELD_LENGTH"]);
                        if (aBlockFieldItem.DataType == typeof(DateTime))
                        {
                            if (aBlockFieldItem.ControlType == null || aBlockFieldItem.ControlType == "")
                                aBlockFieldItem.ControlType = "DateTimeBox";
                        }
                        aBlockFieldItem.QueryMode = DR["QUERYMODE"].ToString();
                        list.Add(aBlockFieldItem);
                    }
                }
            }
            else
            {
                for (int I = 0; I < dtTableSchema.Columns.Count; I++)
                {
                    TBlockFieldItem aBlockFieldItem = new TBlockFieldItem();
                    aBlockFieldItem.DataField = dtTableSchema.Columns[I].ColumnName;
                    aBlockFieldItem.DataType = dtTableSchema.Columns[I].DataType;
                    aBlockFieldItem.Description = dtTableSchema.Columns[I].Caption;
                    aBlockFieldItem.EditMask = String.Empty;
                    list.Add(aBlockFieldItem);
                }
            }
            return list;
        }

        private void CreateWebQueryField(TBlockFieldItem aFieldItem, string Range, bool NewLine)
        {
#if VS90
            WebQueryColumnsCollection QueryColumns = new WebQueryColumnsCollection(null, typeof(QueryColumns));
            if (aFieldItem.QueryMode != null && (string.Compare(aFieldItem.QueryMode, "normal", true) == 0
                  || string.Compare(aFieldItem.QueryMode, "range", true) == 0))
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
#else
            WebClientQuery WebClientQuery1 = (WebClientQuery)FPage.FindControl("WebClientQuery1");
            if (WebClientQuery1 != null)
            {
                if (aFieldItem.QueryMode != null && (aFieldItem.QueryMode.ToUpper() == "NORMAL" || aFieldItem.QueryMode.ToUpper() == "RANGE"))
                {
                    WebQueryColumns qColumns = new WebQueryColumns();
                    qColumns.Column = aFieldItem.DataField;
                    qColumns.Caption = aFieldItem.Description;
                    if (qColumns.Caption == "")
                        qColumns.Caption = aFieldItem.DataField;
                    qColumns.Condition = "And";
                    if (aFieldItem.QueryMode.ToUpper() == "NORMAL")
                    {
                        if (aFieldItem.DataType == typeof(DateTime))
                            qColumns.Operator = "=";
                        if (aFieldItem.DataType == typeof(int) || aFieldItem.DataType == typeof(float) ||
                            aFieldItem.DataType == typeof(double) || aFieldItem.DataType == typeof(Int16))
                            qColumns.Operator = "=";
                        if (aFieldItem.DataType == typeof(String))
                            qColumns.Operator = "%";
                    }

                    qColumns.NewLine = NewLine;

                    if (aFieldItem.QueryMode.ToUpper() == "RANGE")
                    {
                        qColumns.Condition = "And";
                        if (Range == "")
                        {
                            qColumns.Operator = "<=";
                            qColumns.NewLine = false;
                            CreateWebQueryField(aFieldItem, ">=", true);
                        }
                        else
                        {
                            qColumns.Operator = Range;
                        }
                    }
                    switch (aFieldItem.ControlType.ToUpper())
                    {
                        case "DATETIMEBOX":
                            qColumns.ColumnType = "ClientQueryCalendarColumn";
                            break;
                        case "CHECKBOX":
                            qColumns.ColumnType = "ClientQueryCheckBoxColumn";
                            break;
                        default:
                            qColumns.ColumnType = "ClientQueryTextBoxColumn";
                            break;
                    }


                    WebClientQuery1.Columns.Add(qColumns);
                }
                IComponentChangeService FComponentChangeService = (IComponentChangeService)FDesignerHost.RootComponent.Site.GetService(typeof(IComponentChangeService));
                NotifyRefresh(200);
                FComponentChangeService.OnComponentChanged(WebClientQuery1, null, "", "M");
            }
#endif
        }

#if VS90
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

        private void SetCollectionValue(WebDevPage.IHTMLElement controlElement, PropertyInfo prop, Infolight.EasilyReportTools.ReportItemCollection collection)
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
                        controlElement.innerHTML = html.Insert(index, collectionxml);
                    }
                    if (index != 0)
                    {
                        collectionxml = collectionxml.Replace("<" + prop.Name + ">", "");
                        controlElement.innerHTML = html.Replace("</" + prop.Name + ">", collectionxml);
                    }
                }
            }
        }

        private void SetCollectionValue(WebDevPage.IHTMLElement controlElement, PropertyInfo prop, Infolight.EasilyReportTools.FieldItemCollection collection)
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

        private void SetCollectionValue(WebDevPage.IHTMLElement controlElement, PropertyInfo prop, Infolight.EasilyReportTools.DataSourceItemCollection collection)
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

        private string GetCollectionXml(PropertyInfo prop, Infolight.EasilyReportTools.ReportItemCollection collection)
        {
            StringBuilder builder = new StringBuilder();
            if (prop != null && collection != null && prop.PropertyType == collection.GetType())
            {
                if (collection.Count > 0)
                {
                    builder.AppendLine(string.Format("\t<{0}>", prop.Name));
                    for (int i = 0; i < collection.Count; i++)
                    {
                        builder.Append(string.Format("\t\t<{0}:{1} ", "cc1", collection[i].GetType().Name));
                        PropertyInfo[] infos = collection[i].GetType().GetProperties();
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

        private string GetCollectionXml(PropertyInfo prop, Infolight.EasilyReportTools.FieldItemCollection collection)
        {
            StringBuilder builder = new StringBuilder();
            if (prop != null && collection != null && prop.PropertyType == collection.GetType())
            {
                if (collection.Count > 0)
                {
                    builder.AppendLine(string.Format("\t<{0}>", prop.Name));
                    for (int i = 0; i < collection.Count; i++)
                    {
                        builder.Append(string.Format("\t\t<{0}:{1} ", "cc1", collection[i].GetType().Name));
                        PropertyInfo[] infos = collection[i].GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
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

        private string GetCollectionXml(PropertyInfo prop, Infolight.EasilyReportTools.DataSourceItemCollection collection)
        {
            StringBuilder builder = new StringBuilder();
            if (prop != null && collection != null && prop.PropertyType == collection.GetType())
            {
                if (collection.Count > 0)
                {
                    builder.AppendLine(string.Format("\t<{0}>", prop.Name));
                    for (int i = 0; i < collection.Count; i++)
                    {
                        builder.Append(string.Format("\t\t<{0}:{1} ", "cc1", collection[i].GetType().Name));
                        PropertyInfo[] infos = collection[i].GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
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

                                if (infos[j].PropertyType == typeof(Infolight.EasilyReportTools.FieldItemCollection))
                                {
                                    builder.AppendLine(">");
                                    builder.AppendLine(string.Format("\t\t<{0}>", infos[j].Name));

                                    Infolight.EasilyReportTools.FieldItemCollection fieldItemCollection = (Infolight.EasilyReportTools.FieldItemCollection)infos[j].GetValue(collection[i], null);

                                    foreach (Infolight.EasilyReportTools.FieldItem item in fieldItemCollection)
                                    {
                                        builder.Append(string.Format("\t\t\t<{0}:{1} ", "cc1", item.GetType().Name));

                                        PropertyInfo[] itemProps = item.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);

                                        for (int k = 0; k < itemProps.Length; k++)
                                        {
                                            if (!IsVisibilityHidden(itemProps[k]))
                                            {
                                                if (itemProps[k].PropertyType == typeof(string) || itemProps[k].PropertyType == typeof(int) || itemProps[k].PropertyType == typeof(bool)
                                                || itemProps[k].PropertyType.BaseType == typeof(Enum))
                                                {
                                                    if (!itemProps[k].Name.Equals("Name"))
                                                    {
                                                        object itemValue = itemProps[k].GetValue(item, null);
                                                        object itemDefaultvalue = GetDefaultValue(itemProps[k]);
                                                        if (itemProps[k].CanWrite && itemValue != itemDefaultvalue)
                                                        {
                                                            builder.Append(string.Format("{0}=\"{1}\" ", itemProps[k].Name, itemValue));
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                        builder.AppendLine("/>");
                                    }

                                    builder.AppendLine(string.Format("\t\t</{0}>", infos[j].Name));
                                }
                            }
                        }
                        builder.Append(string.Format("\t\t</{0}:{1}>", "cc1", collection[i].GetType().Name));
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

        private int IndexOfEndTag(string html, string tag, out int length)
        {
            Match mc = Regex.Match(html, string.Format(@"</{0}\s*>", tag), RegexOptions.RightToLeft);
            if (mc.Success)
            {
                length = mc.Length;
                return mc.Index;
            }
            else
            {
                length = 0;
                return -1;
            }
        }
#endif

        private bool GenWinSolution(WinClientParam winParam, ClientParam cParam)
        {
            Solution sln = _dte2.Solution;
            string BaseFormName = this.cbTemplate.Text;
            FTemplatePath = Path.GetDirectoryName(_addIn.Object.GetType().Assembly.Location) + "\\Templates\\";
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
                else if (this.cbChooseLanguage.Text == "VB")
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
            if (currentStep == 3)
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
                    break;
                case 1:
                    this.tbcContainer.TabPages.Add(this.tp4);
                    this.btnPrevious.Enabled = true;
                    this.btnNext.Enabled = true;
                    this.btnDone.Enabled = false;
                    this.cbTemplate.Items.Clear();
                    if (this._isWebReport)
                    {
                        this.cbTemplate.Items.Add("WebEasilyReportSingle");
                        this.cbTemplate.Items.Add("WebEasilyReportMasterDetail1");
                        this.cbTemplate.SelectedIndex = 0;

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
                        this.cbTemplate.Items.Add("CEasilyReportSingle1");
                        this.cbTemplate.Items.Add("CEasilyReportMasterDetail1");
                        this.cbTemplate.SelectedIndex = 0;

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
                        this.btnNext.Enabled = true;
                        this.btnDone.Enabled = false;
                        FInfoDataSet = new InfoDataSet();
                        FInfoDataSet.SetWizardDesignMode(true);
                        break;
                    }
                    else
                    {
                        this.tbcContainer.TabPages.Add(this.tp4);
                        this.btnPrevious.Enabled = true;
                        this.btnNext.Enabled = true;
                        this.btnDone.Enabled = false;
                        currentStep--;
                        step--;
                        return;
                    }
                case 3:
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
            foreach (ProjectItem folderItem in proj.ProjectItems)
            {

            }

        }

        private void cmbProName_SelectedIndexChanged(object sender, EventArgs e)
        {

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