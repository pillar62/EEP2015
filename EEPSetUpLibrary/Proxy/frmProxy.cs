using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ProxySetting = EEPSetUpLibrary.Properties.Settings;

namespace EEPSetUpLibrary.Proxy
{
    public partial class frmProxy : Form
    {
        public frmProxy()
        {
            InitializeComponent();
        }

        private void frmProxy_Load(object sender, EventArgs e)
        {
            checkBoxProxy.Checked = ProxySetting.Default.ProxyEnable;
            textBoxAddress.Text = ProxySetting.Default.ProxyAddress;
            textBoxPort.Text = ProxySetting.Default.ProxyPort.ToString();
            textBoxUser.Text = ProxySetting.Default.ProxyUser;
            textBoxPassword.Text = ProxySetting.Default.ProxyPassword;
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
                        ProxySetting.Default.ProxyPort = proxyport;
                    }
                }
                catch
                {
                    MessageBox.Show(this, labelPortError.Text, "Error"
                        , MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.DialogResult = DialogResult.None;
                    return;
                }
                ProxySetting.Default.ProxyAddress = textBoxAddress.Text.Trim(); ;
                ProxySetting.Default.ProxyUser = textBoxUser.Text;
                ProxySetting.Default.ProxyPassword = textBoxPassword.Text;

            }
            ProxySetting.Default.ProxyEnable = proxyenable;
            ProxySetting.Default.Save();
        }
    }
}