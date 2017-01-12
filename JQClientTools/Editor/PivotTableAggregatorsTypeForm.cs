using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace JQClientTools.Editor
{
    public partial class PivotTableAggregatorsTypeForm : ModalForm
    {
        private List<string> items = new List<string>();
        private List<int> ov = new List<int>();
        public PivotTableAggregatorsTypeForm()
        {
            InitializeComponent();
            String serverPath = EFClientTools.DesignClientUtility.GetServerPath();
            EFBase.MessageProvider provider = new EFBase.MessageProvider(serverPath, EFClientTools.DesignClientUtility.ClientInfo.Locale);
            String message = provider["JQWebClient/PivotTable/Aggregators"];
            var messages = message.Split(';');
            foreach (var s in messages) {
                items.Add(s);
            }
        }

        public override object SelectedValue
        {
            get
            {
                string s = "";
                foreach (var item in lbright.Items)
                {
                    if (!string.IsNullOrEmpty(s))
                        s += ";";
                    s += item.ToString();
                }
                return s;
            }
            set
            {
                if (value != null)
                {
                    var istring = value.ToString().Split(';');
                    foreach(string s in istring)
                    {
                        int i = -1;
                        if (Int32.TryParse(s, out i))
                        {
                            lbright.Items.Add(items[i]);
                            ov.Add(i);
                        }
                    }
                    for (var j = 0; j < items.Count; j++)
                    {
                        if (!ov.Contains(j))
                        {
                            lbleft.Items.Add(items[j]);
                        }
                    }
                }
            }
        }

        public object TrueValue 
        {
            get {
                string s = "";
                foreach (var item in lbright.Items)
                {
                    if (!string.IsNullOrEmpty(s))
                        s += ";";
                    s += items.IndexOf(item.ToString());
                }
                return s;

            }
        }

        private void btrightall_Click(object sender, EventArgs e)
        {
            foreach (var item in lbleft.Items)
            {
                lbright.Items.Add(item.ToString());
            }
            lbleft.Items.Clear();
        }

        private void btright_Click(object sender, EventArgs e)
        {
            if (lbleft.SelectedItem != null)
            {
                lbright.Items.Add(lbleft.SelectedItem.ToString());
                lbleft.Items.Remove(lbleft.SelectedItem);
            }
        }

        private void btleft_Click(object sender, EventArgs e)
        {
            if (lbright.SelectedItem != null)
            {
                lbleft.Items.Add(lbright.SelectedItem.ToString());
                lbright.Items.Remove(lbright.SelectedItem);
            }
        }

        private void btleftall_Click(object sender, EventArgs e)
        {
            lbright.Items.Clear();
            lbleft.Items.Clear();
            foreach (var s in items)
            {
                lbleft.Items.Add(s.ToString());
            }
        }	
    }
}