using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net.Mail;
using System.Net;
using System.IO;
using System.Threading;
using Srvtools;

namespace OfficeTools.RunTime
{
    /// <summary>
    /// The form of e-mail
    /// </summary>
    public partial class frmEmail : Form
    {
        string mailto;
        string filename;
        string title;
        Thread td = null;

        /// <summary>
        /// Create a new instance of frmEmail
        /// </summary>
        /// <param name="Mailto">The address to send e-mail</param>
        /// <param name="Filename">The name of attachment</param>
        /// <param name="Title">The title of e-mail</param>
        public frmEmail(string Mailto, string Filename, string Title)
        {
            InitializeComponent();
            mailto = Mailto;
            filename = Filename;
            title = Title;
            lbAttachment.Text = Path.GetFileName(filename);
            tbMailTo.Text = mailto;
            switch (Path.GetExtension(filename).ToLower())
            {
                case ".xls": pbAttachment.Image = OfficeTools.Properties.Resources.excel; break;
                case ".doc": pbAttachment.Image = OfficeTools.Properties.Resources.word; break;
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            tbMailTo.Text = mailto;
            tbMailFrom.Text = string.Empty;
            tbPassword.Text = string.Empty;
            tbSmtpServer.Text = string.Empty;
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            lbMessage.Items.Clear();
            if (tbMailFrom.Text.Trim().Length == 0)
            {
                MessageBox.Show("Mail From can not be empty","Warning",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                return;
            }
            if (tbMailTo.Text.Trim().Length == 0)
            {
                MessageBox.Show("Mail To can not be empty", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            pnBtn.Visible = false;
            pnMessage.Visible = true;
            btnOK.Visible = false;
            cbDetail.Checked = false;
            progressBar.Value = 0;
            td = new Thread(new ThreadStart(Mail));
            td.Start();
        }

        /// <summary>
        /// The mail function
        /// </summary>
        public void Mail()
        {           
            string[] strto = tbMailTo.Text.Split(';');
            MailMessage m_message = new MailMessage();
            m_message.From = new MailAddress(tbMailFrom.Text.Trim());
           
            m_message.Subject = title;
            if (File.Exists(filename))
            { 
                MemoryStream ms = new MemoryStream(File.ReadAllBytes(filename));
                m_message.Attachments.Add(new Attachment(ms,Path.GetFileName(filename)));
            }

            SmtpClient m_smtpClient = new SmtpClient(tbSmtpServer.Text);
            m_smtpClient.UseDefaultCredentials = false;
            m_smtpClient.Credentials = new NetworkCredential(tbMailFrom.Text.Trim(), tbPassword.Text);
            m_smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            
            for (int i = 0; i < strto.Length; i++)
			{
                if (strto[i].Trim().Length > 0)
                {
                    try
                    {
                        m_message.To.Clear();
                        m_message.To.Add(new MailAddress(strto[i].Trim()));
                        m_smtpClient.Send(m_message);
                        AddItem(lbMessage, string.Format("Mail send to"));
                        AddItem(lbMessage, string.Format("{0} success", strto[i].Trim()));
                    }
                    catch(Exception e)
                    {
                        AddItem(lbMessage, string.Format("Mail send to"));
                        AddItem(lbMessage, string.Format("{0} fail", strto[i].Trim()));
                        AddItem(lbMessage, string.Format("Error:{0}", e.Message));
                        if(e.InnerException != null)
                        {
                            AddItem(lbMessage, string.Format("More Error:{0}", e.InnerException.Message));
                        }
                    }
                }
                SetProgress(progressBar, (i + 1) * 100 / strto.Length);
            }
            SetVisible(btnOK, true);
        }

        #region Delegate
        private delegate void AddTextMehtod(ListBox lb, string strmess);
        private delegate void SetVisibleMethod(Control ctrl, bool visible);
        private delegate void SetHeightMethod(Control ctrl, int height);
        private delegate void SetProgressMethod(ProgressBar bar, int value);
        public void AddItem(ListBox lb, string strmess)
        {
            if (lb.InvokeRequired)
            {
                AddTextMehtod call = delegate(ListBox lbd, string strmessd)
                {
                    lbd.Items.Add(strmessd);
                };
                this.Invoke(call, new object[] { lb, strmess });
            }
            else
                lb.Items.Add(strmess);
        }
        public void SetVisible(Control ctrl, bool visible)
        {
            if (ctrl.InvokeRequired)
            {
                SetVisibleMethod call = delegate(Control ctrld, bool visibled)
                {
                    ctrld.Visible = visibled;
                };
                this.Invoke(call, new object[] { ctrl, visible });
            }
            else
                ctrl.Visible = visible;
        }
        public void SetHeight(Control ctrl, int height)
        {
            if (ctrl.InvokeRequired)
            {
                SetHeightMethod call = delegate(Control ctrld, int heightd)
                {
                    ctrld.Height = heightd;
                };
                this.Invoke(call, new object[] { ctrl, height });
            }
            else
                ctrl.Height = height;
        }
        public void SetProgress(ProgressBar bar, int value)
        {
            if (bar.InvokeRequired)
            {
                SetProgressMethod call = delegate(ProgressBar bard, int valued)
                {
                   bard.Value = valued;
                };
                this.Invoke(call, new object[] { bar, value });
            }
            else
                bar.Value = value;
        }
        #endregion

        private void tbMailFrom_Leave(object sender, EventArgs e)
        {
            if (tbMailFrom.Text.Trim().Length == 0)
            {
                if (cbDefault.Checked)
                {
                    tbSmtpServer.Text = string.Empty;
                }
                return;
            }
            string[] mailaddress = tbMailFrom.Text.Trim().Split('@');
            if(mailaddress.Length != 2 || mailaddress[0].Length == 0 || mailaddress[1].Length == 0)
            {
                MessageBox.Show("Invalid email address");
                tbMailFrom.Focus();
                return;
            }
            else if (cbDefault.Checked)
            {
                tbSmtpServer.Text = "smtp." + mailaddress[1];
            }
        }

        private void frmEmail_Load(object sender, EventArgs e)
        {
            #region setup language
            int lang = (int)CliSysMegLag.GetClientLanguage();
            this.Text = OfficeTools.Properties.Resources.email.Split(',')[lang];
            this.btnReset.Text = OfficeTools.Properties.Resources.btnReset.Split(',')[lang];
            this.btnSend.Text = OfficeTools.Properties.Resources.btnSend.Split(',')[lang];
            this.btnOK.Text = OfficeTools.Properties.Resources.btnOK.Split(',')[lang];
            this.labelMailFrom.Text = OfficeTools.Properties.Resources.mailfrom.Split(',')[lang] + ":";
            this.labelMailTo.Text = OfficeTools.Properties.Resources.mailto.Split(',')[lang] + ":";
            this.labelPassword.Text = OfficeTools.Properties.Resources.password.Split(',')[lang] + ":";
            this.labelServer.Text = OfficeTools.Properties.Resources.server.Split(',')[lang] + ":";
            this.labelAttachment.Text = OfficeTools.Properties.Resources.attachment.Split(',')[lang] + ":";
            this.cbDetail.Text = OfficeTools.Properties.Resources.detail.Split(',')[lang];
            this.cbDefault.Text = OfficeTools.Properties.Resources.defaulvalue.Split(',')[lang]; 
            #endregion

            tbMailFrom.Focus();
        }

        private void cbDefault_CheckedChanged(object sender, EventArgs e)
        {
            tbSmtpServer.ReadOnly = cbDefault.Checked;
            if (cbDefault.Checked)
            {
                if (tbMailFrom.Text.Trim().Length == 0)
                {
                    tbSmtpServer.Text = string.Empty;
                }
                else
                {
                    string[] mailaddress = tbMailFrom.Text.Trim().Split('@');
                    tbSmtpServer.Text = "smtp." + mailaddress[1];
                }
            }
        }

        private void cbDetail_CheckedChanged(object sender, EventArgs e)
        {
            lbMessage.Visible = cbDetail.Checked;
            if (cbDetail.Checked)
            {
                this.Height = 330;
            }
            else
            {
                this.Height = 215;
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            pnBtn.Visible = true;
            pnMessage.Visible = false;
            this.Height = 215;
        }

        private void frmEmail_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (td != null && td.ThreadState != ThreadState.Stopped)
            {
                DialogResult dr = MessageBox.Show(string.Format("Mail has not completed, \ndo you really want to abort?")
                    , "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dr == DialogResult.No)
                {
                    e.Cancel = true;
                    return;
                }
                else
                {
                    td.Abort();
                    td = null;
                }
            }
        }
    }
}