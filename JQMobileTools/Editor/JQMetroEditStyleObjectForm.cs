using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace JQMobileTools
{
    public partial class JQMetroEditStyleObjectForm : Form
    {
        public JQMetroEditStyleObjectForm()
        {
            InitializeComponent();
        }
        public JQMetroEditStyleObjectForm(List<string> list)
        {
            InitializeComponent();
            listBox1.DataSource = list;
        }
        public JQMetroEditStyleObjectForm(List<string> list,string value)
        {
            InitializeComponent();
            foreach (string s in list)
            {
                listBox1.Items.Add(s);
            }
            SelectedValue = value;
            listBox1.SelectedItem = value;
        }
        public string SelectedValue { get; set; }

        private void btnOK_Click(object sender, EventArgs e)
        {
            SelectedValue = listBox1.SelectedItem.ToString();
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
