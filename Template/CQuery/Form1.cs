using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Srvtools;

namespace CQuery
{
    public partial class Form1 : InfoForm
    {
        public Form1()
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

        private void Form1_Load(object sender, EventArgs e)
        {
            clientQuery1.Show(splitContainer1.Panel1);
        }
     }
}

