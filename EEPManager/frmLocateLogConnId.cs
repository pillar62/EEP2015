using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace EEPManager
{
    public partial class frmLocateLogConnId : Form
    {
        public frmLocateLogConnId()
        {
            InitializeComponent();
        }

        public string ConnId = "";
        private void btnLocate_Click(object sender, EventArgs e)
        {
            ConnId = this.txtConnId.Text;
        }
    }
}