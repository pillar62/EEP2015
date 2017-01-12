using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Scheduling
{
    public partial class ServerMethod : Form
    {
        public ServerMethod()
        {
            InitializeComponent();
        }

        public ServerMethod(String sName, String mt)
        {
            solution = sName;
            methodType = mt;
            InitializeComponent();
        }

        private String solution;
        public String packageName;
        public String methodName;
        public String methodType;
        private void btnPackage_Click(object sender, EventArgs e)
        {
            PackagesSelect ps = new PackagesSelect(solution, methodType);
            ps.ShowDialog();
            txtPackage.Text = ps.selectedPackage;
            //openFileDialog1.InitialDirectory = Application.StartupPath;
            //if (openFileDialog1.ShowDialog() == DialogResult.OK)
            //{
            //    fullPackage = openFileDialog1.FileName;
            //    txtPackage.Text = openFileDialog1.FileName.Substring(openFileDialog1.FileName.LastIndexOf('\\') + 1);
            //    string [] temp = txtPackage.Text.Split('.');
            //    txtPackage.Text = temp[0];
            //}
        }

        private void bthMethod_Click(object sender, EventArgs e)
        {
            if (txtPackage.Text == "")
            {
                MessageBox.Show("Please choose the package first.");
            }
            else
            {
                Method frmm = new Method(txtPackage.Text, solution);
                frmm.ShowDialog();
                txtMethod.Text = frmm.method;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            packageName = txtPackage.Text;
            methodName = txtMethod.Text;
            Close();
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}