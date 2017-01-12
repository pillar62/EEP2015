using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Srvtools
{
    public partial class frmProxy : Form
    {
        public frmProxy(bool enable, string address, int port, string user, string password)
        {
            InitializeComponent();
            proxyEnable = enable;
            proxyAddress = address;
            proxyPort = port;
            proxyUser = user;
            textBoxPassword.Text = password;
        }

        private bool proxyEnable;

        public bool ProxyEnable
        {
            get { return proxyEnable; }
        }

        private string proxyAddress;

        public string ProxyAddress
        {
            get { return proxyAddress; }
        }

        private int proxyPort;

        public int ProxyPort
        {
            get { return proxyPort; }
        }

        private string proxyUser;

        public string ProxyUser
        {
            get { return proxyUser; }
        }

        //A6-Sensitive Data Exposure
        //private string proxyPassword;

        //public string ProxyPassword
        //{
        //    get { return proxyPassword; }
        //}
	
        private void frmProxy_Load(object sender, EventArgs e)
        {
            checkBoxProxy.Checked = ProxyEnable;
            textBoxAddress.Text = ProxyAddress;
            textBoxPort.Text = ProxyPort.ToString();
            textBoxUser.Text = ProxyUser;
            //A6-Sensitive Data Exposure
            //textBoxPassword.Text = ProxyPassword;
            panelProxy.Enabled = checkBoxProxy.Checked;
        }

        private void checkBoxProxy_CheckedChanged(object sender, EventArgs e)
        {
            panelProxy.Enabled = checkBoxProxy.Checked;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            bool proxyenable = checkBoxProxy.Checked;
            if (proxyenable)
            {
                if (textBoxAddress.Text.Trim().Length == 0)
                {
                    MessageBox.Show(this, labelAddressError.Text, "Error"
                        , MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.DialogResult = DialogResult.None;
                    return;
                }
                try
                {
                    int proxyport = Convert.ToInt32(textBoxPort.Text);
                    if (proxyport < 1 || proxyport > 65535)
                    {
                        MessageBox.Show(this, labelPortError.Text, "Error"
                        , MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.DialogResult = DialogResult.None;
                        return;
                    }
                    else
                    {
                        proxyPort = proxyport;
                    }
                }
                catch
                {
                    MessageBox.Show(this, labelPortError.Text, "Error"
                        , MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.DialogResult = DialogResult.None;
                    return;
                }
                proxyAddress = textBoxAddress.Text.Trim(); ;
                proxyUser = textBoxUser.Text;
                //A6-Sensitive Data Exposure
                //proxyPassword = textBoxPassword.Text;

            }
            proxyEnable = proxyenable;
        }
    }
}