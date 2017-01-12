using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Srvtools
{
    public partial class PERemoteName : Form
    {
        private string[] sInList = null;
        private string sSel = "";
        public PERemoteName(string[] sList, string sSelected)
        {
            sInList = sList;
            sSel = sSelected;
            InitializeComponent();
            DealWithInList();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox2.Items.Clear();
            string sSel = listBox1.Items[listBox1.SelectedIndex].ToString();
            foreach (string s in sInList)
            {
                bool b = s.StartsWith(sSel + ".");
                if (b)
                {
                    int iPos = s.IndexOf('.');
                    string sCommand = s.Substring(iPos + 1);
                    if (listBox2.Items.IndexOf(sCommand) == -1)
                        listBox2.Items.Add(sCommand);
                }
            }

            if (listBox2.Items.Count > 0)
                listBox2.SelectedIndex = 0;
        }

        private void DealWithInList()
        {
            listBox1.SelectedIndexChanged += new EventHandler(listBox1_SelectedIndexChanged);
            if (null == sInList ) return;
            
            foreach(string s in sInList)
            {
                int iPos = s.IndexOf('.');
                string sModule = s.Substring(0, iPos);
                if (listBox1.Items.IndexOf(sModule) == -1)
                    listBox1.Items.Add(sModule);
            }
            if (listBox1.Items.Count > 0)
                listBox1.SelectedIndex = 0;
        }

        [Browsable(false)]
        public string RemoteName
        {
            get
            {
                string sRet = "";
                if ((listBox1.Items.Count == 0) || (listBox2.Items.Count == 0)) return sRet;
                sRet = listBox1.Items[listBox1.SelectedIndex] + "." +
                    listBox2.Items[listBox2.SelectedIndex];
                return sRet;
            }
        }

        private void PERemoteName_Load(object sender, EventArgs e)
        {

        }
    }
}