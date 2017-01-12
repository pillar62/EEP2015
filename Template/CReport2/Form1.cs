using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Srvtools;

namespace CReport2
{
    public partial class Form1 : InfoForm
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            clientQuery1.Show(panel1);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            clientQuery1.Execute(panel1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            clientQuery1.Clear(panel1);
            crystalReportViewer1.ReportSource = null;
        }
    }
}