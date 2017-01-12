using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using EnvDTE;
using EnvDTE80;
using Microsoft.Win32;
using System.IO;

namespace MWizard2015
{
    public partial class fmNewEmptySolution : Form
    {
        private DTE2 FDTE;
        private AddIn FAddIn;

        public fmNewEmptySolution(DTE2 aDTE, AddIn addin)
        {
            FDTE = aDTE;
            FAddIn = addin;
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (tbSolutionPath.Text.Trim().Length == 0)
            {
                tbSolutionPath.Focus();
                throw new Exception("Please input Solution Path !");
            }
            if (tbSolutionName.Text.Trim().Length == 0)
            {
                tbSolutionName.Focus();
                throw new Exception("Please input Solution Name !");
            }

            GenerateSolution();
        }


        private void GenerateSolution()
        {
            String TargetPath, SlnName, FullSlnName;
            SlnName = tbSolutionName.Text;
            if (SlnName.IndexOf('.') > -1)
                SlnName = SlnName.Substring(0, SlnName.IndexOf('.'));
            if (tbSolutionPath.Text[tbSolutionPath.Text.Length - 1] != '\\')
                tbSolutionPath.Text = tbSolutionPath.Text + @"\";

            if (cbCreateDirectory.Checked)
            {
                FullSlnName = tbSolutionPath.Text + SlnName + @"\" + SlnName + ".sln";
                TargetPath = tbSolutionPath.Text + SlnName;
            }
            else
            {
                TargetPath = tbSolutionPath.Text;
                FullSlnName = tbSolutionPath.Text + SlnName + ".sln";
            }


            if (System.IO.File.Exists(FullSlnName))
            {
                String Message = String.Format("The Solution is already exists, would you like to delete it ?\n\"{0}\"", FullSlnName);
                if (MessageBox.Show(Message, "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    System.IO.File.Delete(FullSlnName);
                }
                else
                {
                    return;
                }
            }

            if (cbCreateDirectory.Checked)
            {
                if (System.IO.Directory.Exists(TargetPath))
                {
                    String Message = String.Format("The path is already exists, would you like to delete it ?\n\"{0}\" ", TargetPath);
                    if (MessageBox.Show(Message, "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        System.IO.Directory.Delete(TargetPath, true);
                    }
                    else
                    {
                        return;
                    }
                }
                System.IO.Directory.CreateDirectory(TargetPath);
            }

            try
            {
                FDTE.Solution.Create(TargetPath, SlnName);
                if (cbCordova.Checked)
                    ProjectLoader.AddDefaultProject(FDTE, "Cordova");
                if (!cbJQuery.Checked)
                    ProjectLoader.AddDefaultProject(FDTE);
                if (cbWorkflow.Checked)
                    ProjectLoader.AddDefaultProject(FDTE, "Workflow");
                if (cbEntityFramework.Checked)
                    ProjectLoader.AddDefaultProject(FDTE, "EntityFramework");
                if (cbJQuery.Checked)
                    ProjectLoader.AddDefaultProject(FDTE, "JQuery");
                FDTE.Solution.SaveAs(FullSlnName);
                Application.DoEvents();
                System.Threading.Thread.Sleep(3000);
                Application.DoEvents();
                FDTE.Solution.Close();
                Application.DoEvents();
                System.Threading.Thread.Sleep(3000);
                Application.DoEvents();
                if (cbJQuery.Checked)
                {
                    using (StreamReader sr = File.OpenText(FullSlnName))
                    {
                        String allText = sr.ReadToEnd();
                        if (allText.Contains("http://localhost"))
                        {
                            string old = allText.Substring(allText.IndexOf("http://localhost"), 21);
                            allText = allText.Replace(old, "JQWebClient\\");
                            allText = allText.Replace("localhost", "JQWebClient");
                            allText = allText.Insert(allText.IndexOf("EndProjectSection"), "\t\tVWDPort = \"1048\" \r\n");
                        }
                        sr.Close();
                        File.WriteAllText(FullSlnName, allText);
                    }
                }
                FDTE.Solution.Open(FullSlnName);
                String Dir = tbSolutionPath.Text + tbSolutionName.Text;
                System.IO.Directory.CreateDirectory(Dir);
                Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                tbSolutionPath.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void fmNewEmptySolution_Load(object sender, EventArgs e)
        {
            StringBuilder ServerPath = new StringBuilder(WzdUtils.GetServerPath(FAddIn, false));
            for (int i = ServerPath.Length - 1; i > 0; i--)
            {
                if (ServerPath[i] != '\\')
                    ServerPath.Remove(i, 1);
                else
                {
                    ServerPath.Remove(i, 1);
                    break;
                }
            }
            tbSolutionPath.Text = EEPRegistry.Server.Replace("\\EEPNetServer", string.Empty); //ServerPath.ToString();
        }
    }

    public static class ProjectLoader
    {
        private static void AddByName(String ProjectName, DTE2 aDTE)
        {
            String Path = EEPRegistry.Server;
            String S1 = "", S2 = "";
            S1 = WzdUtils.GetToken(ref Path, new char[] { '\\' });
            while (Path != "")
            {
                S2 = S2 + S1 + '\\';
                S1 = WzdUtils.GetToken(ref Path, new char[] { '\\' });
            }
            aDTE.Solution.AddFromFile(S2 + ProjectName, false);
        }

        private static void AddWebSite(DTE2 aDTE)
        {
            String Path = EEPRegistry.WebClient;
            if (Path.Contains("EFWebClient"))
                Path = EEPRegistry.WebClient.Replace("EFWebClient", "EEPWebClient");
            AddWebSite(aDTE, Path);
        }

        private static void AddWebSite(DTE2 aDTE, String WebFolderPath)
        {
            String Path = WebFolderPath;
            VsWebSite.VSWebPackage webPackage = aDTE.GetObject("WebPackage") as VsWebSite.VSWebPackage;
            EnvDTE.Project proj = webPackage.OpenWebSite(Path,
               VsWebSite.OpenWebsiteOptions.OpenWebsiteOption_None, true);
            String str = string.Empty;
        }

        public static void AddDefaultProject(DTE2 aDTE)
        {
            AddDefaultProject(aDTE, "");
        }

        public static void AddDefaultProject(DTE2 aDTE, String containProject)
        {
            if (containProject == "Workflow")
            {
                AddByName(@"FLCore\FLCore.csproj", aDTE);
                AddByName(@"FLDesigner\FLDesigner.csproj", aDTE);
                AddByName(@"FLDesignerCore\FLDesignerCore.csproj", aDTE);
                AddByName(@"FLRuntime\FLRuntime.csproj", aDTE);
                AddByName(@"FLTools\FLTools.csproj", aDTE);
            }
            else if (containProject == "EntityFramework")
            {
                AddByName(@"EFClientTools\EFClientTools.csproj", aDTE);
                AddByName(@"EFGlobalModule\EFGlobalModule.csproj", aDTE);
                AddByName(@"EFServerTools\EFServerTools.csproj", aDTE);
                AddByName(@"EFBase\EFBase.csproj", aDTE);
                AddByName(@"EFWCFModule\EFWCFModule.csproj", aDTE);
                AddByName(@"ExtTools\ExtTools.csproj", aDTE);
                AddByName(@"SLClient\SLClient.csproj", aDTE);
                AddByName(@"SLMain\SLMain.csproj", aDTE);
                AddByName(@"SLTools\SLTools.csproj", aDTE);
                AddByName(@"SLTools.Design\SLTools.Design.csproj", aDTE);
                AddByName(@"SLTools.Service\SLTools.Service.csproj", aDTE);
                String sPath = EEPRegistry.WebClient.Replace("EEPWebClient", "EFWebClient");
                AddWebSite(aDTE, sPath);
            }
            else if (containProject == "JQuery")
            {
                AddByName(@"EFClientTools\EFClientTools.csproj", aDTE);
                AddByName(@"EFGlobalModule\EFGlobalModule.csproj", aDTE);
                AddByName(@"EFServerTools\EFServerTools.csproj", aDTE);
                AddByName(@"EFBase\EFBase.csproj", aDTE);
                AddByName(@"EFWCFModule\EFWCFModule.csproj", aDTE);
                AddByName(@"JQClientTools\JQClientTools.csproj", aDTE);
                AddByName(@"EEPNetServer\EEPNetServer.csproj", aDTE);
                AddByName(@"Srvtools\Srvtools.csproj", aDTE);
                String sPath = EEPRegistry.WebClient.Replace("EEPWebClient", "JQWebClient");
                AddWebSite(aDTE, sPath);
            }
            else if (containProject == "Cordova")
            {
                AddByName(@"EEPAPP\EEPApp.jsproj", aDTE);
                AddByName(@"JQMobileTools\JQMobileTools.csproj", aDTE);
            }
            else
            {
                AddByName(@"Srvtools\Srvtools.csproj", aDTE);
                AddByName(@"EEPNetServer\EEPNetServer.csproj", aDTE);
                AddByName(@"EEPNetClient\EEPNetClient.csproj", aDTE);
                AddWebSite(aDTE);
            }
        }
    }
}

/*
  Sub RemoveWebSite()
       

        Dim dte2 As EnvDTE80.DTE2
        Dim sln2 As Solution2
        Dim proj As EnvDTE.Project
        Dim tmpFile As String
        Dim webPackage As VsWebSite.VSWebPackage
        Dim webSites As Projects


        Dim sitepath As String = 
             "http://localhost/StevenRoot/Whidbey/WebSites/IISSite/"


        dte2 = DTE
        sln2 = dte2.Solution
        webPackage = dte2.GetObject("WebPackage")

        webSites = dte2.GetObject("WebSites")

        proj = webSites.Item(sitepath)

        sln2.Remove(proj)

        MsgBox("done")
    End Sub

 */