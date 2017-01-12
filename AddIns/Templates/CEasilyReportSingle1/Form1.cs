using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Srvtools;

namespace TAG_NAMESPACE
{
    public partial class TAG_FORMNAME : InfoForm
    {
        public TAG_FORMNAME()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            clientQuery1.Execute(splitContainer1.Panel1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            clientQuery1.Clear(splitContainer1.Panel1);
        }

        private void TAG_FORMNAME_Load(object sender, EventArgs e)
        {
            clientQuery1.Show(splitContainer1.Panel1);
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            easilyReport1.Execute(false);
        }
     }
}

