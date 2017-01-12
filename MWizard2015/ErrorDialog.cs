using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MWizard2015
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

        private void ErrorDialog_Load(object sender, EventArgs e)
        {
            lblStackTrace.Hide();
            txtStack.Hide();
        }

        private void txtMessage_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void btnSOrHStack_Click(object sender, EventArgs e)
        {
            if (string.Compare(btnSOrHStack.Text, "show stack", true) == 0)//IgnoreCase
            {
                this.Height = this.txtStack.Location.Y + this.txtStack.Height + 4 * btnSOrHStack.Height;
                lblStackTrace.Show();
                txtStack.Show();
                btnSOrHStack.Text = "Hide Stack";
            }
            else
            {
                this.Height = this.txtMessage.Location.Y + this.txtMessage.Height + 4 * btnSOrHStack.Height;
                txtMessage.Show();
                lblStackTrace.Hide();
                txtStack.Hide();
                btnSOrHStack.Text = "Show Stack";
            }
        }

      

      
    }
}