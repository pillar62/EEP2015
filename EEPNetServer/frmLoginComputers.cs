using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace EEPNetServer
{
    public partial class frmLoginComputers : Form
    {
        public frmLoginComputers(UserInfo info)
        {
            InitializeComponent();
            this.Text = string.Format("{0}({1})", info.UserID, info.UserName);
            if (!SrvGL.AllowLoginInSameIP)
            {
                this.listView.Columns.RemoveAt(3);
            }
            ComputerInfo[] infos = info.Computers;
            for (int i = 0; i < infos.Length; i++)
            {
                ListViewItem aItem = listView.Items.Add(infos[i].ComputerName);
                aItem.SubItems.Add(new ListViewItem.ListViewSubItem(aItem, infos[i].LoginTime.ToString("yyyy-MM-dd HH:mm:ss")));
                aItem.SubItems.Add(new ListViewItem.ListViewSubItem(aItem, infos[i].LastActiveTime.ToString("yyyy-MM-dd HH:mm:ss")));
                if (SrvGL.AllowLoginInSameIP)
                {
                    aItem.SubItems.Add(new ListViewItem.ListViewSubItem(aItem, infos[i].Count.ToString()));
                }
            }
        }
    }
}