using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Microsoft.Win32;
using System.Xml;

namespace EEPNetServer
{
    public partial class frmRegister : Form
    {
        public frmRegister()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            x64f2717168e0a936.xeaa3f94758c74ce3(textBoxSN.Text, textBoxNS.Text, textBoxNS1.Text, textBoxNS2.Text, textBoxNS3.Text, textBoxCompany.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}