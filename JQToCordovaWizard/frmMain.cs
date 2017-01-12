using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EnvDTE;
using EnvDTE80;
using System.IO;

namespace JQToCordovaWizard
{
    public partial class frmMain : Form
    {
        private DTE2 _dte;
        public DTE2 DTE
        {
            get { return _dte; }
        }

        public string WebsitePath { get; set; }

        public string CordovaPath { get; set; }

        public frmMain(DTE2 dte)
        {
            InitializeComponent();
            _dte = dte;
            LoadControls();

        }

        private void LoadControls()
        {
            foreach (Project project in DTE.Solution.Projects)
            {
                var kind = project.Kind;
                if (kind == "{E24C65DC-7377-472b-9ABA-BC803B73C61A}")
                {
                    comboBoxWebsite.Items.Add(project.Name);
                }
                else if (kind == "{262852C6-CD72-467D-83FE-5EEB1973A190}")
                {
                    comboBoxCordova.Items.Add(project.Name);
                }
            }
            if (comboBoxCordova.Items.Count > 0)
            {
                comboBoxCordova.SelectedIndex = 0;
            }
            if (comboBoxWebsite.Items.Count > 0)
            {
                comboBoxWebsite.SelectedIndex = 0;
            }
        }

        private void comboBoxWebsite_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxFolder.Items.Clear();
            foreach (Project project in DTE.Solution.Projects)
            {
                if (project.Name == (string)comboBoxWebsite.SelectedItem)
                {
                    WebsitePath = project.FullName;
                    foreach (ProjectItem projectItem in project.ProjectItems)
                    {
                        if (string.Compare(projectItem.Kind, "{6BB5F8EF-4483-11D3-8BCF-00C04F8EC28C}") == 0)
                        {
                            comboBoxFolder.Items.Add(projectItem.Name);
                        }
                    }
                    if (comboBoxFolder.Items.Count > 0)
                    {
                        comboBoxFolder.SelectedIndex = 0;
                    }
                    break;
                }
            }
        }

        private void comboBoxCordova_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (Project project in DTE.Solution.Projects)
            {
                if (project.Name == (string)comboBoxCordova.SelectedItem)
                {
                    CordovaPath = Path.GetDirectoryName(project.FullName);

                    var jsPath = Path.Combine(CordovaPath, "www", "js", "jquery.infolight.mobile.js");
                    if (System.IO.File.Exists(jsPath))
                    {
                        using (var reader = new StreamReader(jsPath, true))
                        {
                            var line = reader.ReadLine();
                            if (line.StartsWith("var webSiteUrl ="))
                            {
                                textBoxVirtualPath.Text = line.Split('=')[1].Trim(" ';".ToArray()).Replace("http://", string.Empty);
                            }
                        }
                    }

                    break;
                }
            }
        }

        private void comboBoxFolder_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshWebsiteFolder((string)comboBoxFolder.SelectedItem);
            RefreshCordovaFolder((string)comboBoxFolder.SelectedItem);
        }


        private void RefreshWebsiteFolder(string folder)
        {
            checkedListBoxJqueryPage.Items.Clear();
            if (!string.IsNullOrEmpty(WebsitePath))
            {
                var directory = Path.Combine(WebsitePath, folder);
                if (Directory.Exists(directory))
                {
                    var files = Directory.GetFiles(directory, "*.aspx");
                    foreach (var file in files)
                    {
                        var fileName = Path.GetFileNameWithoutExtension(file);
                        checkedListBoxJqueryPage.Items.Add(fileName, true);
                    }
                }
            }
        }
        private void RefreshCordovaFolder(string folder)
        {
            listViewCordovaPage.Items.Clear();
            if (!string.IsNullOrEmpty(CordovaPath))
            {
                var directory = Path.Combine(CordovaPath, "www", folder);
                if (Directory.Exists(directory))
                {
                    var files = Directory.GetFiles(directory, "*.html");
                    foreach (var file in files)
                    {
                        var fileName = Path.GetFileNameWithoutExtension(file);
                        listViewCordovaPage.Items.Add(new ListViewItem(fileName));
                    }
                }
            }
        }

        private void buttonSelectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBoxJqueryPage.Items.Count; i++)
            {
                checkedListBoxJqueryPage.SetItemChecked(i, true);
            }
        }

        private void buttonUnSelectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBoxJqueryPage.Items.Count; i++)
            {
                checkedListBoxJqueryPage.SetItemChecked(i, false);
            }
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            //int port = 0;
            if (comboBoxWebsite.SelectedItem == null)
            {
                labelInfomation.ForeColor = Color.Red;
                labelInfomation.Text = "Website path is empty.";
            }
            else if (comboBoxFolder.SelectedItem == null)
            {
                labelInfomation.ForeColor = Color.Red;
                labelInfomation.Text = "Page folder is empty.";
            }
            else if (comboBoxCordova.SelectedItem == null)
            {
                labelInfomation.ForeColor = Color.Red;
                labelInfomation.Text = "Cordova path is empty.";
            }
            else if (textBoxVirtualPath.Text == string.Empty)
            {
                labelInfomation.ForeColor = Color.Red;
                labelInfomation.Text = "IIS virtual directory is empty.";
            }
            else
            {
                var jsPath = Path.Combine(CordovaPath, "www", "js", "jquery.infolight.mobile.js");
                if (System.IO.File.Exists(jsPath))
                {
                    var js = string.Empty;
                    using (var reader = new StreamReader(jsPath, true))
                    {
                        js = reader.ReadToEnd();
                    }
                    var line = js.Split('\r')[0];
                    if (line.StartsWith("var webSiteUrl ="))
                    {
                        js = js.Replace(line, string.Format("var webSiteUrl = 'http://{0}';", textBoxVirtualPath.Text));
                        using (var writer = new StreamWriter(jsPath, false, new UTF8Encoding(true)))
                        {
                            writer.Write(js);
                        }
                    }
                }

                if (checkedListBoxJqueryPage.CheckedItems.Count > 0)
                {
                    progressBar.Value = 0;
                    progressBar.Maximum = checkedListBoxJqueryPage.CheckedItems.Count;
                    labelInfomation.ForeColor = Color.Black;
                    labelInfomation.Text = string.Empty;
                    buttonStart.Visible = false;
                    buttonCancel.Visible = true;
                    splitContainer1.Panel1.Enabled = false;


                    var pages = new List<string>();
                    foreach (var item in checkedListBoxJqueryPage.CheckedItems)
                    {
                        pages.Add((string)item);
                    }

                    var parameters = new ConvertParameters()
                    {
                        Website = (string)comboBoxWebsite.SelectedItem,
                        WebsitePath = WebsitePath,
                        CordovaPath = CordovaPath,
                        Folder = (string)comboBoxFolder.SelectedItem,
                        VirtualPath = textBoxVirtualPath.Text,
                        Pages = pages

                    };
                    backgroundWorker.RunWorkerAsync(parameters);
                }
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            if (backgroundWorker.IsBusy)
            {
                backgroundWorker.CancelAsync();
            }
            buttonStart.Visible = true;
            buttonCancel.Visible = false;
        }

        private bool CheckProcess(string processName)
        {
            return System.Diagnostics.Process.GetProcessesByName(processName).Length > 0;
        }

        private bool StartEEPNetServer(string eepDirectory)
        {
            var directory = Path.Combine(eepDirectory, "EEPNetServer");
            if (Directory.Exists(directory) && File.Exists((Path.Combine(directory, "EEPNetServer.exe"))))
            {
                var process = System.Diagnostics.Process.Start(Path.Combine(directory, "EEPNetServer.exe"));
                process.WaitForInputIdle();
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool StartIIS(string website)
        {
            var directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "IIS Express");
            if (!Directory.Exists(directory))
            {
                directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "IIS Express");
            }
            if (Directory.Exists(directory))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var parameters = (ConvertParameters)e.Argument;

            backgroundWorker.ReportProgress(0, "Checking process EEPNetServer");

            if (!CheckProcess("EEPNetServer"))
            {
                backgroundWorker.ReportProgress(0, "Starting process EEPNetServer");
                var eepDirectory = new DirectoryInfo(WebsitePath).Parent.FullName;
                if (!StartEEPNetServer(eepDirectory))
                {
                    throw new Exception("EEPNetServer not found.");
                }
            }
            backgroundWorker.ReportProgress(0, "Checking process iisexpress");
            if (!CheckProcess("iisexpress"))
            {
                backgroundWorker.ReportProgress(0, "Starting process iisexpress");
                if (!StartIIS(parameters.WebsitePath))
                {
                    throw new Exception("IIS Express not found.");
                }
            }
            for (int i = 0; i < parameters.Pages.Count; i++)
            {
                var page = parameters.Pages[i];

                backgroundWorker.ReportProgress(i, string.Format("Converting page:{0}/{1}.aspx", parameters.Folder, page));

                System.Net.WebClient client = new System.Net.WebClient();
                client.Encoding = new System.Text.UTF8Encoding();
                client.Headers.Add("Accept", "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, */*");
                client.Headers.Add("Accept-Language", "zh-cn");
                client.Headers.Add("UA-CPU", "x86");
                client.Headers.Add("User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)");

                var html = client.DownloadString(new Uri(string.Format("http://{0}/{1}/{2}.aspx?Cordova=true", parameters.VirtualPath, parameters.Folder, page)));
                var targetDir = Path.Combine(parameters.CordovaPath, "www", parameters.Folder);
                if (!Directory.Exists(targetDir))
                {
                    Directory.CreateDirectory(targetDir);
                }
                using (StreamWriter writer = new StreamWriter(Path.Combine(targetDir, string.Format("{0}.html", page)), false, new System.Text.UTF8Encoding(true)))
                {
                    writer.WriteLine(html.Replace("&#39;", "'").Replace("../MobileMainPage.aspx", "../main.html"));
                    writer.Flush();
                }
            }
        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
            labelInfomation.Text = (string)e.UserState;
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressBar.Value = progressBar.Maximum;
            buttonCancel.Visible = false;
            buttonStart.Visible = true;
            splitContainer1.Panel1.Enabled = true;
            if (e.Cancelled)
            {
                labelInfomation.Text = "Convert operation is canceled.";
            }
            else if (e.Error != null)
            {
                labelInfomation.ForeColor = Color.Red;
                labelInfomation.Text = e.Error.Message;
            }
            else
            {
                labelInfomation.Text = "Convert operation is completed.";
            }
            RefreshCordovaFolder((string)comboBoxFolder.SelectedItem);
        }

      
    }

    public class ConvertParameters
    {
        public string Website { get; set; }

        public string WebsitePath { get; set; }

        public string Folder { get; set; }

        public List<string> Pages { get; set; }

        public string CordovaPath { get; set; }


        public string VirtualPath { get; set; }
        //public int Port { get; set; }
    }
}
