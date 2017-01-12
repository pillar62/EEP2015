using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Srvtools;

namespace EEPNetServer
{
    public partial class PackageTransferInfoForm : Form
    {
        public PackageTransferInfoForm()
        {
            InitializeComponent();
        }

        private void PackageTransferInfoForm_Load(object sender, EventArgs e)
        {
            this.dgTransferInfo.DataSource = ServerConfig.PackageTransferList.Tables[0];
            if (ServerConfig.AutoTransferPackage)
            {
                this.cbxAutoTransfer.Checked = ServerConfig.AutoTransferPackage;
            }
        }

        private void dgTransferInfo_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            e.Cancel = true;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            ServerConfig.AutoTransferPackage = this.cbxAutoTransfer.Checked;
            this.Close();
        }
    }
}