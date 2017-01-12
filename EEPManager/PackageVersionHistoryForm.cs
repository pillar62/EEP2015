using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Srvtools;
using System.Reflection;

namespace EEPManager
{
    public partial class PackageVersionHistoryForm : Form
    {
        //这个form基本上要重写了...
        private string FileName = string.Empty;
        private PackageType PackageType = PackageType.Server;
        string packagetype = "";
        private string SolutionName = string.Empty;
        private string LocalPackagePath = "";

        public PackageVersionHistoryForm(string filename, PackageType pactype, string solutionname, string locPackagePath)
        { 
            InitializeComponent();
            FileName = filename;
            PackageType = pactype;
            SolutionName = solutionname;
            LocalPackagePath = locPackagePath;
            switch (pactype)
            {
                case PackageType.Client: packagetype = "C"; break;
                case PackageType.Server: packagetype = "S"; break;
                case PackageType.WebClient: packagetype = "W"; break;
            }
        }

        private void PackageVersionHistoryForm_Load(object sender, EventArgs e)
        {
            this.idsPackageVersion.SetWhere("ITEMTYPE ='" + SolutionName + "' AND FILENAME='" + FileName + "' AND FILETYPE='" + packagetype + "'");
            this.Text = "Package History of " + SolutionName + "/" + FileName;
            for (int i = 0; i < this.dgVersionHistory.Rows.Count; i++)
            { 
                int verno = this.dgVersionHistory.Rows.Count - i;
                this.dgVersionHistory.Rows[i].Cells["ColumnVerNo"].Value = verno.ToString();
            }
            this.lbHistory.Text = this.dgVersionHistory.Rows.Count.ToString() + " items";
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgVersionHistory_SelectionChanged(object sender, EventArgs e)
        {
            if (this.dgVersionHistory.SelectedRows.Count == 1)
            {
                this.btnDownload.Enabled = true;
                if (this.dgVersionHistory.SelectedRows[0].Index == 0)
                {
                    this.btnRollback.Enabled = false;
                }
                else
                {
                    this.btnRollback.Enabled = true;
                }
            }
            else
            {
                this.btnRollback.Enabled = false;
                this.btnDownload.Enabled = false;
            }
        }

        private void btnRollback_Click(object sender, EventArgs e)
        {
            string packagetime = Convert.ToDateTime(((DataGridViewRow)this.dgVersionHistory.SelectedRows[0]).Cells["pACKAGEDATEDataGridViewTextBoxColumn"].Value).ToString("yyyy/MM/dd HH:mm:ss");
            if (MessageBox.Show("ATTENTION:Rollback will cause some version of packages LOST!\n\rWould you like to continue?", "Warning"
                , MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                //callmethod rollback
                CliUtils.CallMethod("GLModule", "PackageRollback", new object[] { SolutionName, FileName, packagetime, PackageType });
                this.idsPackageVersion.SetWhere("ITEMTYPE ='" + SolutionName + "' AND FILENAME='" + FileName + "' AND FILETYPE='" + packagetype + "'");
                for (int i = 0; i < this.dgVersionHistory.Rows.Count; i++)
                {
                    int verno = this.dgVersionHistory.Rows.Count - i;
                    this.dgVersionHistory.Rows[i].Cells["ColumnVerNo"].Value = verno.ToString();
                }
                this.lbHistory.Text = this.dgVersionHistory.Rows.Count.ToString() + " items";
            }
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            string packagetime = ((DataGridViewRow)this.dgVersionHistory.SelectedRows[0]).Cells["pACKAGEDATEDataGridViewTextBoxColumn"].Value.ToString();
            PackageCollection dlls = new PackageCollection();
            dlls.Action = PackageAction.Download;
            dlls.Packages.Add(FileName);
            dlls.DateTimes.Add(packagetime);
            dlls.Solution = SolutionName;
            dlls.Type = PackageType;
            PackageTransferForm transForm = new PackageTransferForm(dlls, LocalPackagePath);
            transForm.Text = "Package Download State";
            transForm.ShowDialog();
        }
    }
}