using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EnvDTE80;
using EnvDTE;
using System.IO;
using Microsoft.Win32;

namespace UpdateReference2015
{
    internal partial class FormMain : Form
    {
        internal FormMain(DTE2 _applicationObject, AddIn _addInInstance)
        {
            InitializeComponent();
            ApplicationObject = _applicationObject;
            AddInInstance = _addInInstance;
        }

        const string Reference_File = "Reference.cs";

        const string Service_URI = "http://localhost:8990/EFWCFModule/EFService";

        private DTE2 ApplicationObject { get; set; }

        private AddIn AddInInstance { get; set; }

        private Solution Solution
        {
            get
            {
                return ApplicationObject.Solution;
            }
        }

        private string AddInPath
        {
            get 
            {
                const string REGISTRYNAME = "infolight\\eep.net";
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
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            backgroundWorker.RunWorkerAsync();
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            //update reference of EFClientTools
            backgroundWorker.ReportProgress(0, "Updating service reference of EFClientTools");
            var efClientToolsFile = GetFilePath("EFClientTools");
            if (!string.IsNullOrEmpty(efClientToolsFile))
            {
                var dir = string.Format("\"{0}\"", Path.GetDirectoryName(efClientToolsFile));
                var output = Path.GetFileName(efClientToolsFile);
                SvcUtil util = new SvcUtil(AddInPath)
                {
                    Directory = dir,
                    Out = output,
                    Language = SvcUtil.Language_CS,
                    NameSpace = "*,EFClientTools.EFServerReference",
                    CollectionType = SvcUtil.CollectionType_List,
                    NoConfig = true,
                    NoLogo = true,
                    EnableDataBinding = true,
                    ServiceURI = Service_URI
                };
                var result = util.Execute();
                if (!result.Contains(Reference_File))
                {
                    throw new Exception("Update service reference failed, restart your service and try again");
                }
            }
            
            //update reference of SLTools.Service
            backgroundWorker.ReportProgress(25, "Updating service reference of SLTools.Service");
            var slToolsFile = GetFilePath("SLTools.Service");
            if (!string.IsNullOrEmpty(slToolsFile))
            {
                var dir = string.Format("\"{0}\"", Path.GetDirectoryName(slToolsFile));
                var output = Path.GetFileName(slToolsFile);
                SlSvcUtil util = new SlSvcUtil(AddInPath)
                {
                    Directory = dir,
                    Out = output,
                    Language = SvcUtil.Language_CS,
                    NameSpace = "*,SLTools.EFServerReference",
                    CollectionType = SvcUtil.CollectionType_List,
                    //NoConfig = true,  5.0 SlSvcUtil NoConfig 属性有bug
                    Config = "default",
                    NoLogo = true,
                    EnableDataBinding = true,
                    ServiceURI = Service_URI
                };
                var result = util.Execute();
                if (!result.Contains(Reference_File))
                {
                    throw new Exception("Update service reference failed, restart your service and try again");
                }
            }

            //compile EFClientTools
            backgroundWorker.ReportProgress(50, "Building EFClientTools");
            CompileProject("EFClientTools");

            //compile SLTools.Service
            backgroundWorker.ReportProgress(75, "Building SLTools.Service");
            CompileProject("SLTools.Service");

            backgroundWorker.ReportProgress(100, "");
        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
            label.Text = (string)e.UserState;
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                MessageBox.Show(this, "Update service reference canceled", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (e.Error != null)
            {
                MessageBox.Show(this, e.Error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show(this, "Update service reference succeed", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            this.Close();
        }

        private void CompileProject(string projectName)
        {
            var project = GetProject(projectName);
            if (project != null)
            {
                Solution.SolutionBuild.BuildProject(Solution.SolutionBuild.ActiveConfiguration.Name, project.FullName, true);
            }
        }

        private Project GetProject(string projectName)
        {
            foreach (Project proj in Solution.Projects)
            {
                if (string.Compare(proj.Name, projectName, true) == 0)
                {
                    return proj;
                }
            }
            return null;
        }

        private ProjectItem GetProjectItem(Project project, string itemName)
        {
            foreach (ProjectItem item in project.ProjectItems)
            {
                if (string.Compare(item.Name, itemName, true) == 0)
                {
                    return item;
                }
            }
            return null;
        }

        private string GetFilePath(string projectName)
        {
            var project = GetProject(projectName);
            if (project != null)
            {
                var projectItem = GetProjectItem(project, Reference_File);
                if (projectItem != null)
                {
                    return projectItem.FileNames[0];
                }
            }
            return null;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Cancel updating service reference?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
                == System.Windows.Forms.DialogResult.Yes)
            {
                backgroundWorker.CancelAsync();
            }
        }
    }
}
