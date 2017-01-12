using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Net.Mail;
using System.Net;

namespace EEPServerMonitor
{
    public partial class FormOptions : Form
    {
        public FormOptions()
        {
            InitializeComponent();
        }

        private void FormOptions_Load(object sender, EventArgs e)
        {
            textBoxRefreshInterval.Text = Config.RefreshInterval.ToString();
            textBoxCpuWarning.Text = Config.CpuWarning.ToString();
            textBoxMemoryWarning.Text = Config.MemoryWarning.ToString();
            textBoxIntervalWarning.Text = Config.IntervalWarning.ToString();
            textBoxServer.Text = Config.EmailServer;
            textBoxUser.Text = Config.EmailUser;
            textBoxPassword.Text = Config.EmailPassword;
            textBoxAddress.Text = Config.EmailAddress;
            checkBoxWarning.Checked = Config.WarningNotify;
            checkBoxError.Checked = Config.ErrorNotify;
            textBoxNotifyCycle.Text = Config.NotifyCycle.ToString();
            checkBoxEnableSSL.Checked = Config.EnableSSL;
            foreach (Server srv in Config.Servers)
            {
                treeView.Nodes[0].Nodes.Add(srv.Uri, srv.Uri, 0);
            }
            treeView.ExpandAll();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            FormServer form = new FormServer();
            if (form.ShowDialog(this) == DialogResult.OK)
            { 
                string uri = form.ServerUri;
                if (treeView.Nodes[0].Nodes[uri] == null)
                {
                    treeView.Nodes[0].Nodes.Add(uri, uri, 0);
                }
            }
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            if (treeView.SelectedNode != null && treeView.SelectedNode.Level == 1)
            {
                treeView.SelectedNode.Remove();
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            int refreshinterval = 0;
            float cpu = 0.0f;
            float memory = 0;
            double interval = 0;
            int notifycycle = 0;
            try
            {
                refreshinterval = int.Parse(textBoxRefreshInterval.Text);
                cpu = float.Parse(textBoxCpuWarning.Text);
                memory = float.Parse(textBoxMemoryWarning.Text);
                interval = double.Parse(textBoxIntervalWarning.Text);
                if (refreshinterval <= 0)
                {
                    throw new Exception("Value of refresh interval should be postive interger");
                }
                else if (cpu <= 0.0f || cpu > 100.0f)
                {
                    throw new Exception("Value of cpu warning should between 0 to 100");
                }
                else if (memory <= 20.0f)
                {
                    throw new Exception("Value of memory warning should large than 20MB");
                }
                else if (interval < 50)
                {
                    throw new Exception("Value of response interval should large than 50ms");
                }
            }
            catch(Exception ex)
            {
                tabControl1.SelectedIndex = 0;
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.None;
            }
            try
            {
                notifycycle = int.Parse(textBoxNotifyCycle.Text);
                if (notifycycle <= 0)
                {
                    throw new Exception("Value of notify cycle should be postive interger");
                }
            }
            catch (Exception ex)
            {
                tabControl1.SelectedIndex = 2;
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.None;
            }

            Config.RefreshInterval = refreshinterval;
            Config.CpuWarning = cpu;
            Config.MemoryWarning = memory;
            Config.IntervalWarning = interval;
            Config.EmailServer = textBoxServer.Text;
            Config.EmailUser = textBoxUser.Text;
            Config.EmailPassword = textBoxPassword.Text;
            Config.EmailAddress = textBoxAddress.Text;
            Config.WarningNotify = checkBoxWarning.Checked;
            Config.ErrorNotify = checkBoxError.Checked;
            Config.NotifyCycle = notifycycle;
            Config.EnableSSL = checkBoxEnableSSL.Checked;

            foreach (Server srv in Config.Servers)
            {
                if (srv.Thread != null && srv.Thread.ThreadState != ThreadState.Stopped)
                {
                    srv.Thread.Abort();
                }
            }
            Config.Servers.Clear();
            for (int i = 0; i < treeView.Nodes[0].Nodes.Count; i++)
            {
                Config.Servers.Add(new Server(treeView.Nodes[0].Nodes[i].Text));
            }
            Config.Save();
        }

        private void buttonTest_Click(object sender, EventArgs e)
        {
            if (textBoxServer.Text.Length == 0)
            {
                MessageBox.Show(this, "Mail server is empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (textBoxAddress.Text.Length == 0)
            {
                MessageBox.Show(this, "Mail address is empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (textBoxUser.Text.Length == 0)
            {
                MessageBox.Show(this, "Mail user is empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                MailMessage mailMessage = new MailMessage(textBoxUser.Text, textBoxAddress.Text);
                mailMessage.Subject = string.Format("This is test mail from EEPServer Moniter");

                SmtpClient smtpClient = new SmtpClient(textBoxServer.Text);
                smtpClient.EnableSsl = checkBoxEnableSSL.Checked;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(textBoxUser.Text, textBoxPassword.Text);
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.SendCompleted += new SendCompletedEventHandler(smtpClient_SendCompleted);
                smtpClient.SendAsync(mailMessage, null);
            }
            catch(Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public delegate void ShowMessage(Exception e);

        void smtpClient_SendCompleted(object sender, AsyncCompletedEventArgs e)
        {
            ShowMessage call = delegate(Exception ex)
            {
                if (ex == null)
                {
                    MessageBox.Show(this, "Test successfully", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(this, string.Format("Test failed\r\n{0}", e.Error.Message), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
            this.Invoke(call, new object[] { e.Error });
        }
    }
}