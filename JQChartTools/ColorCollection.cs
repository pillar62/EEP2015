using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace JQChartTools
{
    public partial class ColorCollection : ModalForm
    {
        public ColorCollection()
        {
            InitializeComponent();

        }

        private void btdelete_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                foreach (ListViewItem lvi in listView1.SelectedItems)
                    listView1.Items.Remove(lvi);
            }
        }

        private void btcolorSelect_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (colorDialog1.Color.IsNamedColor)
                    textBox1.Text = colorDialog1.Color.Name;
                else
                    textBox1.Text = ColorTranslator.ToHtml(colorDialog1.Color);
            }
        }

        private void btadd_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                if (!listView1.Items.ContainsKey(textBox1.Text.ToLower()))
                    listView1.Items.Add(textBox1.Text.ToLower());
            }
        }

        private string _result = "";
        private void btOK_Click(object sender, EventArgs e)
        {
            _result = "[]";
            List<string> colorList = new List<string>();
            foreach (ListViewItem lvi in listView1.Items)
            {
                colorList.Add("'"+lvi.Text+"'");
            }
            _result = "[" + string.Join(",", colorList) + "]";
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        public override object SelectedValue
        {
            get
            {
                return _result;
            }
            set
            {
                _result = value.ToString() ;
                if (_result != "")
                {
                    listView1.Items.Clear();
                    if (_result.StartsWith("[") && _result.EndsWith("]"))
                    {
                        string[] s = _result.Substring(1, _result.Length - 2).Split(new char[]{','},StringSplitOptions.RemoveEmptyEntries);
                        foreach (string v in s)
                        {
                            if (v.Trim().StartsWith("'") && v.Trim().EndsWith("'"))
                            {
                                listView1.Items.Add(v.Trim().Substring(1, v.Trim().Length - 2).ToLower());
                            }
                        }
                    }
                }
            }
        }
    }
}
