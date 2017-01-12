using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;

namespace EEPServerMonitor
{
    /// <summary>
    /// 增加Server的窗体
    /// </summary>
    public partial class FormServer : Form
    {
        public FormServer()
        {
            InitializeComponent();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (textBoxIP.Text.Trim().Length == 0)
            {
                MessageBox.Show(this, "Server ip can not be empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.None;
                return;
            }
            try
            {
                int port = int.Parse(textBoxPort.Text);
                if (port < IPEndPoint.MinPort || port > IPEndPoint.MaxPort)
                {
                    MessageBox.Show(this, "The value of port is out of range", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.DialogResult = DialogResult.None;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.None;
            }
        }

        /// <summary>
        /// 获取Server的地址
        /// </summary>
        public string ServerUri
        {
            get
            {
                return string.Format("{0}:{1}", textBoxIP.Text.Trim(), textBoxPort.Text.Trim());
            }
        }
    }
}