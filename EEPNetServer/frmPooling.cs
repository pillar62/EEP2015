using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Srvtools;

namespace EEPNetServer
{
    public partial class frmPooling : Form
    {
        
        private DbConnectionSet.DbConnection connection;
        public DbConnectionSet.DbConnection Connection
        {
            get { return  connection; }
        }
	

        public frmPooling(DbConnectionSet.DbConnection conn)
        {
            InitializeComponent();
            connection = conn;
            RefreshPooling();
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            RefreshPooling();
        }

        private void buttonRelease_Click(object sender, EventArgs e)
        {
            if (listBox.SelectedItem != null)
            {
                Connection.Release((listBox.SelectedItem as DbConnectionSet.ConnectionInfo).Connection);
                RefreshPooling();
            }
        }

        private void RefreshPooling()
        {
            DbConnectionSet.ConnectionInfo[] infos = Connection.GetUnReleasedConnections();
            this.listBox.Items.Clear();
            this.listBox.Items.AddRange(infos);
            labelMax.Text = Connection.MaxCount.ToString();
            labelUsed.Text = infos.Length.ToString();
            groupBoxCurrent.Visible = false;
        }

        private void listBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox.SelectedIndex != -1)
            {
                groupBoxCurrent.Visible = true;
                DbConnectionSet.ConnectionInfo info = listBox.SelectedItem as DbConnectionSet.ConnectionInfo;
                labelUser.Text = info.UserID;
                labelModule.Text = info.Module;
                labelDateTime.Text = info.Time.ToString("yyyy/MM/dd HH:mm:ss");
            }
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Clear connection pooling maybe cause Instability Problem, Continue?", "Warning"
                , MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                try
                {
                    Connection.Clear();
                    MessageBox.Show(this, "Connection pooling cleared", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch(Exception ex)
                {
                    MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}