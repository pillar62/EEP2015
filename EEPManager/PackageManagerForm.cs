using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

using Microsoft.Win32;
using Srvtools;
using System.Reflection;
using System.Collections;
using System.Globalization;

namespace EEPManager
{
    public partial class PackageManagerForm : Form
    {
        private string LocalPackagePath = string.Empty;
        private string SolutionName = string.Empty;
        private PackageType PackageType = PackageType.Server;

        public PackageManagerForm()
        {
            InitializeComponent();
        }

        private void PackageManagerForm_Load(object sender, EventArgs e)
        {
            this.trvSolution.Nodes.Add("Solution");
            foreach (DataRow row in this.solution.RealDataSet.Tables[0].Rows)
            {
                this.trvSolution.Nodes[0].Nodes.Add(row["ITEMTYPE"].ToString());
            }
            this.trvSolution.ExpandAll();

            this.cbxPackageType.SelectedIndex = 0;

            RefreshPackageDisplay(true, true);
        }

        private void cbxPackageType_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.trvSolution.Nodes.Clear();
            if (cbxPackageType.SelectedIndex != 2)
            {
                this.trvSolution.Nodes.Add("Solution");
                foreach (DataRow row in this.solution.RealDataSet.Tables[0].Rows)
                {
                    this.trvSolution.Nodes[0].Nodes.Add(row["ITEMTYPE"].ToString());
                }
                this.trvSolution.ExpandAll();
                if (cbxPackageType.SelectedIndex == 0)
                {
                    PackageType = PackageType.Server;
                    this.trvSolution.Nodes.Add("WorkFlow\\FL");//加上workflow文件夹
                }
                else if (cbxPackageType.SelectedIndex == 1)
                {
                    PackageType = PackageType.Client;
                }
            }
            else if (cbxPackageType.SelectedIndex == 2)
            {
                PackageType = PackageType.WebClient;
                this.trvSolution.Nodes.Add("Folder");
                string path = EEPRegistry.WebClient;
                if (Directory.Exists(path))
                {
                    string[] directories = Directory.GetDirectories(path, "*", SearchOption.TopDirectoryOnly);
                    foreach (string str in directories)
                    {
                         this.trvSolution.Nodes[0].Nodes.Add(Path.GetFileName(str));
                    }
                }
            }
            this.trvSolution.ExpandAll();
            RefreshPackageDisplay(true, true);
        }
        
        private void RefreshPackageDisplay(bool refreshRemote, bool refreshLocal)
        {
            if (this.trvSolution.SelectedNode != null && !this.trvSolution.Nodes[0].IsSelected)
            {
                if (refreshRemote)
                { 
                    string filetype = string.Empty;
                    switch (cbxPackageType.SelectedIndex)
                    {
                        case 0: filetype = "S"; break;
                        case 1: filetype = "C"; break;
                        case 2: filetype = "W"; break;
                    }
                    this.remotePackage.SetWhere(string.Format("FILETYPE='{0}' and ITEMTYPE='{1}'", filetype, trvSolution.SelectedNode.Text));
                }
                if (refreshLocal)
                {
                    string path = string.Empty;
                    switch (cbxPackageType.SelectedIndex)
                    {
                        case 0: path = EEPRegistry.Server; break;
                        case 1: path = EEPRegistry.Client; break;
                        case 2: path = EEPRegistry.WebClient; break;
                    }
                    LocalPackagePath = string.Format("{0}\\{1}", path, this.trvSolution.SelectedNode.Text);
                    string pattern = "*.dll";
                    if(cbxPackageType.SelectedIndex == 0)
                    {
                        if (string.Compare(this.trvSolution.SelectedNode.Text, "workflow\\fl", true) == 0)
                        pattern = "*.xoml";
                    }
                    else if (cbxPackageType.SelectedIndex == 2)
                    { 
                        if(string.Compare(this.trvSolution.SelectedNode.Text, "image", true) == 0)
                        {
                             pattern = "*.jpg";//work flow image
                        }
                        else if(string.Compare(this.trvSolution.SelectedNode.Text, "bin", true) != 0)
                        {
                            pattern = "*.aspx";//aspx file
                        }
                    }
                    
                    //string pattern = (cbxPackageType.SelectedIndex == 2 && string.Compare(this.trvSolution.SelectedNode.Text, "bin", true) != 0) ?
                    //    "*.aspx" : "*.dll";
                    DataTable table = localPackage.Tables[0];
                    table.Rows.Clear();
                    if (Directory.Exists(LocalPackagePath))
                    {
                        foreach (string file in Directory.GetFiles(LocalPackagePath, pattern, SearchOption.TopDirectoryOnly))
                        {
                            DataRow row = table.NewRow();
                            row["Name"] = Path.GetFileName(file);

                            //Modified by lily 2006/4/24
                            //row["DateTime"] = File.GetLastAccessTime(file);
                            DateTime dt = File.GetLastWriteTime(file);
                            row["DateTime"] = dt.ToString("yyyy/MM/dd HH:mm:ss");
                            //Modified by lily 2006/4/24
                            table.Rows.Add(row);
                        }
                    }
                }
            }
            else
            {
                this.remotePackage.SetWhere("1 <> 1");
                this.localPackage.Tables[0].Rows.Clear();
            }
        }

        private void trvSolution_AfterSelect(object sender, TreeViewEventArgs e)
        {
            this.trvSolution.SelectedNode.BackColor = Color.DarkBlue;
            this.trvSolution.SelectedNode.ForeColor = Color.White;

            if (this.trvSolution.SelectedNode != null)
            {
                SolutionName = this.trvSolution.SelectedNode.Text;
            }
            RefreshPackageDisplay(true, true);
        }

        private void trvSolution_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (this.trvSolution.SelectedNode != null)
            {
                this.trvSolution.SelectedNode.BackColor = Color.White;
                this.trvSolution.SelectedNode.ForeColor = Color.Black;
            }
        }

        private void dgServerPackage_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            e.Cancel = true;
        }

        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            e.Cancel = true;
        }

        private void dgLocalPackage_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.trvSolution.SelectedNode != null
                && !this.trvSolution.Nodes[0].IsSelected
                && e.Y > this.dgLocalPackage.ColumnHeadersHeight 
                && e.Button == MouseButtons.Left
                && this.dgLocalPackage.SelectedRows.Count > 0)
            {
                PackageCollection dlls = new PackageCollection();
                dlls.Action = PackageAction.Upload;
                foreach (DataGridViewRow row in this.dgLocalPackage.SelectedRows)
                {
                    dlls.Packages.Add(row.Cells["dgvlocalpackage"].Value.ToString()); 
                    dlls.DateTimes.Add(row.Cells["dgvlocalfiledate"].Value.ToString());
                }
                dlls.Solution = SolutionName;
                dlls.Type = PackageType;

                this.DoDragDrop(dlls, DragDropEffects.Copy | DragDropEffects.Move);
            }
        }

        private void dgRemotePackage_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.trvSolution.SelectedNode != null 
                && !this.trvSolution.Nodes[0].IsSelected
                && e.Y > this.dgRemotePackage.ColumnHeadersHeight 
                && e.Button == MouseButtons.Left
                && this.dgRemotePackage.SelectedRows.Count > 0)
            {
                PackageCollection dlls = new PackageCollection();
                dlls.Action = PackageAction.Download;
                foreach (DataGridViewRow row in this.dgRemotePackage.SelectedRows)
                {
                    dlls.Packages.Add(row.Cells["dgvremotepackage"].Value.ToString());
                    dlls.DateTimes.Add(row.Cells["dgvremotepackagedate"].Value.ToString());
                }
                dlls.Solution = SolutionName;
                dlls.Type = PackageType;

                this.DoDragDrop(dlls, DragDropEffects.Copy | DragDropEffects.Move);
            }
        }



        private void dgLocalPackage_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void dgRemotePackage_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void dgLocalPackage_DragDrop(object sender, DragEventArgs e)                                      //下载
        {
            PackageCollection dlls = e.Data.GetData(typeof(PackageCollection)) as PackageCollection;
            if (dlls != null && dlls.Action == PackageAction.Download && dlls.Packages.Count > 0)
            {
                PackageTransferForm transForm = new PackageTransferForm(dlls, LocalPackagePath);
                transForm.Text = "Package Download State";
                transForm.ShowDialog();
                // Reload Local Packages
                RefreshPackageDisplay(false, true);
            }
        }

        private void dgRemotePackage_DragDrop(object sender, DragEventArgs e)                                     //上传
        {
            PackageCollection dlls = e.Data.GetData(typeof(PackageCollection)) as PackageCollection;
            if (dlls != null && dlls.Action == PackageAction.Upload && dlls.Packages.Count > 0)
            {
                PackageTransferForm transForm = new PackageTransferForm(dlls, LocalPackagePath);
                transForm.Text = "Package Upload State";
                transForm.ShowDialog();
                if (cbxPackageType.SelectedIndex == 1)
                {
                    //try
                    //{
                    //    EEPSetUpLibrary.LoaderObject obj = (EEPSetUpLibrary.LoaderObject)Activator.GetObject(typeof(EEPSetUpLibrary.LoaderObject)
                    //        , string.Format("http://{0}:{1}/LoaderObject.rem", CliUtils.fRemoteIP, CliUtils.fRemotePort));
                    //    obj.RefreshFileList();
                    //}
                    //catch (Exception ex)
                    //{
                    //    MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //}
                }
                // Reload Remote Packages
                RefreshPackageDisplay(true, false);
            }
        }


        private void dgRemotePackage_SelectionChanged(object sender, EventArgs e)
        {
            if (this.dgRemotePackage.SelectedRows.Count != 1)
            {
                this.versionHistoryToolStripMenuItem.Enabled = false;
            }
            else
            {
                this.versionHistoryToolStripMenuItem.Enabled = true;
            }
        }

        private void versionHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //modified by lily 2006/4/27 
            //string packageName = this.dgRemotePackage.SelectedRows[0].Cells[0].Value.ToString();
            string fileName = this.dgRemotePackage.SelectedRows[0].Cells["dgvremotepackage"].Value.ToString();
            //modified by lily 2006/4/27

            PackageVersionHistoryForm versionHistory = new PackageVersionHistoryForm(fileName, this.PackageType, this.SolutionName, LocalPackagePath);
            versionHistory.ShowDialog();
            this.RefreshPackageDisplay(true, false);
        }
    }

    public class PackageCollection
    {
        public PackageAction Action;
        public PackageType Type;
        public string Solution;
        public ArrayList Packages = new ArrayList();
        public ArrayList DateTimes = new ArrayList();
        //public List<string> Packages = new List<string>();
        //public List<string> DateTimes = new List<string>(); // Only used for Upload
    }

    public enum PackageAction
    {
        Download,
        Upload
    }
}