using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Srvtools
{
    public partial class FeedbackBugDialog : Form
    {
        private Object _parForm;

        public FeedbackBugDialog()
        {
            InitializeComponent();
        }

        public FeedbackBugDialog(Object parForm)
        {
            InitializeComponent();
            _parForm = parForm;
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            ((ErrorDialog)_parForm).IsFeedback = true;
            ((ErrorDialog)_parForm).ErrDescrip = txtDescrip.Text;
            Close();
            Dispose();
            // CliUtils.FeedbackBug(_errMessage, txtDescrip.Text);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ((ErrorDialog)_parForm).IsFeedback = false;
            Close();
            Dispose();
        }

        private void txtDescrip_TextChanged(object sender, EventArgs e)
        {
            if (txtDescrip.Text == null || txtDescrip.Text.Length == 0)
            {
                btnSend.Enabled = false;
            }
            else
            {
                btnSend.Enabled = true;
            }
        }
    }
}