using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Srvtools
{
    public partial class ErrorDialog : Form
    {
        private String _errMessage;
        private String _stack;
        private string _serverstack;
        private Boolean _isFeedback;
        private String _errDescrip;

        public ErrorDialog()
        {
            InitializeComponent();
        }

        public ErrorDialog(String errMessage, String stackTrace, string serverTrace)
        {
            InitializeComponent();
            txtMessage.Text = errMessage;
            txtStack.Text = stackTrace;
            _errMessage = errMessage;
            _stack = stackTrace;
            _serverstack = serverTrace;
        }

        public Boolean IsFeedback
        {
            get { return _isFeedback; }
            set { _isFeedback = value; }
        }

        public String ErrDescrip
        {
            get { return _errDescrip; }
            set { _errDescrip = value; }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnFeedback_Click(object sender, EventArgs e)
        {
            FeedbackBugDialog frmFeedback = new FeedbackBugDialog(this);
            frmFeedback.ShowDialog();
            this.Close();

            // 读图片
            //sqlConnection1.Open();
            //Object o = sqlCommand2.ExecuteScalar();
            //byte[] bs = (byte[])o;
            //sqlConnection1.Close();
            //MemoryStream ms = new MemoryStream(bs);
            //Image image = Image.FromStream(ms);
        }

        private void ErrorDialog_Load(object sender, EventArgs e)
        {
            //this.Size = new Size(448, 250);
            lblStackTrace.Hide();
            txtStack.Hide();
        }

        private void txtMessage_TextChanged(object sender, EventArgs e)
        {
            if (txtMessage.Text == null || txtMessage.Text.Length == 0)
            {
                btnFeedback.Enabled = false;
            }
            else
            {
                btnFeedback.Enabled = true;
            }
        }

        private void btnSOrHStack_Click(object sender, EventArgs e)
        {
            if (string.Compare(btnSOrHStack.Text, "show stack", true) == 0)//IgnoreCase
            {
                this.Height = this.txtStack.Location.Y + this.txtStack.Height + 4 * btnFeedback.Height;
                lblStackTrace.Show();
                txtStack.Show();
                btnSOrHStack.Text = "Hide Stack";
                buttonServerInfo.Enabled = true;
            }
            else
            {
                this.Height = this.txtMessage.Location.Y + this.txtMessage.Height + 4 * btnFeedback.Height;
                txtMessage.Show();
                lblStackTrace.Hide();
                txtStack.Hide();
                btnSOrHStack.Text = "Show Stack";
                buttonServerInfo.Enabled = false;
            }
        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            try
            {
                CliUtils.CallMethod("GLModule", "LogOut", new object[] { (object)(CliUtils.fLoginUser) });
            }
            catch (Exception E)
            {
                if (E is System.Net.WebException && (E as System.Net.WebException).Status == System.Net.WebExceptionStatus.ConnectFailure)
                { 
                    Application.Exit();
                }
            }
            CliUtils.applicationQuit = true;
            Application.Exit();
        }

        private void buttonServerInfo_Click(object sender, EventArgs e)
        {
            if (string.Compare(btnSOrHStack.Text, "hide stack", true) == 0)
            {
                txtStack.Text = string.Compare(buttonServerInfo.Text, "server stack", true) == 0 ? _serverstack : _stack;
                buttonServerInfo.Text = string.Compare(buttonServerInfo.Text, "server stack", true) == 0 ? "Client Stack" : "Server Stack";
            }
        }
    }
}