using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using Srvtools;

namespace EEPNetServer
{
    public partial class frmSystemLogMan : Form
    {

        public frmSystemLogMan()
        {
            InitializeComponent();
            checkBoxSystem.Tag = SysEEPLogService.LogTypes.System;
            checkBoxProvider.Tag = SysEEPLogService.LogTypes.Provider;
            checkBoxCallMethod.Tag = SysEEPLogService.LogTypes.CallMethod;
            CheckBoxUserDefine.Tag = SysEEPLogService.LogTypes.UserDefine;
            checkBoxEmail.Tag = SysEEPLogService.LogTypes.Email;
            checkBoxNormal.Tag = SysEEPLogService.LogTypes.Normal;
            checkBoxWarning.Tag = SysEEPLogService.LogTypes.Warning;
            checkBoxError.Tag = SysEEPLogService.LogTypes.Error;
            checkBoxUnkown.Tag = SysEEPLogService.LogTypes.Unknown;
        }

        private void frmSystemLogMan_Load(object sender, EventArgs e)
        {
            LoadXmlAndInitControls();
        }

        private void LoadXmlAndInitControls()
        {
            txtMaxLogDays.Text = SysEEPLogService.MaxLogDays.ToString();
            txtMaxSize.Text = SysEEPLogService.MaxSize.ToString();
            txtMaxRecord.Text = SysEEPLogService.MaxRecord.ToString();
            if (SysEEPLogService.Enable)
            {
                chkDisable.Checked = false;
                groupBox1.Enabled = true;
            }
            else
            {
                chkDisable.Checked = true;
                groupBox1.Enabled = false;
            }
            if (SysEEPLogService.LogToFile)
            {
                radioButtonFile.Checked = true;
                if (SysEEPLogService.Enable)
                {
                    txtMaxLogDays.Enabled = true;
                    //chkLogSql.Enabled = true;
                }
            }
            else
            {
                radioButtonDB.Checked = true;
                txtMaxLogDays.Enabled = false;
                //chkLogSql.Enabled = false;
            }
            this.chkLogSql.Checked = SysEEPLogService.LogSql;

            SysEEPLogService.LogTypes types = SysEEPLogService.EnableLogs;
            foreach (Control ct in groupBoxEnableLogs.Controls)
            {
                if (ct is CheckBox)
                {
                    SysEEPLogService.LogTypes typect = (SysEEPLogService.LogTypes)ct.Tag;
                    (ct as CheckBox).Checked = (((int)typect) & ((int)types)) > 0;
                }
            }
            this.textBoxInterval.Text = SysEEPLogService.WarningInterval.ToString();

            tbWarningEMail.Text = SysEEPLogService.WarningEmail;
            tbWarningNo.Text = SysEEPLogService.WarningNumber;
            tbErrorEMail.Text = SysEEPLogService.ErrorEmail;
            tbErrorNo.Text = SysEEPLogService.ErrorNumber;
            tbUnkownEMail.Text = SysEEPLogService.UnkownEmail;
            tbUnkownNo.Text = SysEEPLogService.UnkownNumber;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            bool enable = !chkDisable.Checked;
            bool logtofile = radioButtonFile.Checked;

            int MaxLogDays = 7;
            try
            {
                MaxLogDays = Convert.ToInt32(txtMaxLogDays.Text);
            }
            catch { }
            int MaxSize = -1;
            try
            {
                MaxSize = Convert.ToInt32(txtMaxSize.Text);
            }
            catch { }
            int MaxRecord = 1000;
            try
            {
                MaxRecord = Convert.ToInt32(txtMaxRecord.Text);
            }
            catch { }
            int Interval = 1000;
            try
            {
                Interval = Convert.ToInt32(textBoxInterval.Text);
            }
            catch { }
            SysEEPLogService.LogTypes types = SysEEPLogService.LogTypes.Normal;
            foreach (Control ct in groupBoxEnableLogs.Controls)
            {
                if (ct is CheckBox)
                {
                    if ((ct as CheckBox).Checked)
                    {
                        types |= (SysEEPLogService.LogTypes)ct.Tag;
                    }
                }
            }
            if (enable && !logtofile)
            {
                try
                {
                    SysEEPLogService.CheckConnection();
                    SysEEPLogService.StartConnection();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.DialogResult = DialogResult.None;
                    return;
                }
            }
            SysEEPLogService.EnableLogs = types;
            SysEEPLogService.LogSql = chkLogSql.Checked;
            SysEEPLogService.LogToFile = logtofile;
            SysEEPLogService.MaxLogDays = MaxLogDays;
            SysEEPLogService.MaxSize = MaxSize;
            SysEEPLogService.MaxRecord = MaxRecord;
            SysEEPLogService.WarningInterval = Interval;
            SysEEPLogService.WarningEmail = tbWarningEMail.Text;
            SysEEPLogService.WarningNumber = tbWarningNo.Text;
            SysEEPLogService.ErrorEmail = tbErrorEMail.Text;
            SysEEPLogService.ErrorNumber = tbErrorNo.Text;
            SysEEPLogService.UnkownEmail = tbUnkownEMail.Text;
            SysEEPLogService.UnkownNumber = tbUnkownNo.Text;
            string message = "modified";
            if (!SysEEPLogService.Enable && enable)
            {
                message = "started";
            }
            else if (SysEEPLogService.Enable && !enable)
            {
                message = "terminated";
            }
            SysEEPLogService.Enable = enable;
            SysEEPLogService.Save();

            MessageBox.Show(this, string.Format("EEPLog service has been {0}", message), "Info"
                , MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void chkDisable_CheckedChanged(object sender, EventArgs e)
        {
            groupBox1.Enabled = !chkDisable.Checked;
            groupBoxEnableLogs.Enabled = !chkDisable.Checked;
            chkLogToFile_CheckedChanged(sender, e);
        }

        private void chkLogToFile_CheckedChanged(object sender, EventArgs e)
        {
            if (!chkDisable.Checked)
            {
                if (radioButtonFile.Checked)
                {
                    txtMaxLogDays.Enabled = true;
                    txtMaxSize.Enabled = true;
                    txtMaxRecord.Enabled = false;
                    //chkLogSql.Enabled = true;
                }
                else
                {
                    txtMaxLogDays.Enabled = false;
                    txtMaxSize.Enabled = false;
                    txtMaxRecord.Enabled = true;
                    //chkLogSql.Enabled = false;
                }
            }
        }

        private void linkLabelDetail_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (groupBoxEnableLogs.Visible)
            {
                this.Height -= (groupBoxEnableLogs.Height + 10);
                if (groupBoxNotifySetting.Visible)
                {
                    this.Height -= (groupBoxNotifySetting.Height + 10);
                    groupBoxNotifySetting.Visible = false;
                }
                //this.Height = 220;
            }
            else
            {
                this.Height += (groupBoxEnableLogs.Height +10);
                //this.Height = 340;
            }
            groupBoxEnableLogs.Visible = !groupBoxEnableLogs.Visible;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (groupBoxNotifySetting.Visible)
            {
                this.Height -= (groupBoxNotifySetting.Height + 10);
                //this.Height = 340;
            }
            else
            {
                this.Height += (groupBoxNotifySetting.Height + 10);
                //this.Height = 500;
            }
            groupBoxNotifySetting.Visible = !groupBoxNotifySetting.Visible;
        }
    }
}