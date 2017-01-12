using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Srvtools;

namespace Scheduling
{
    public partial class PackagesSelect : Form
    {
        public PackagesSelect()
        {
            InitializeComponent();
        }

        private String solution;
        public String selectedPackage;
        public String methodType;
        public PackagesSelect(String sName, String mt)
        {
            solution = sName;
            methodType = mt;
            InitializeComponent();
        }

        private void PackagesSelect_Load(object sender, EventArgs e)
        {
            object[] myRet = CliUtils.CallMethod("GLModule", "GetProviderName", new object[] { solution });
            if (myRet != null && myRet[0].ToString() == "0")
            {
                foreach (String str in (myRet[1] as ArrayList))
                {
                    this.lbPackages.Items.Add(str);
                }
                this.lbPackages.Items.Add("GLModule");
                if (methodType.Contains("FLMethod"))
                    this.lbPackages.Items.Add("FLRuntime");
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            selectedPackage = this.lbPackages.SelectedItem.ToString();
            this.Close();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}