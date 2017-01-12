using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Srvtools
{
    public partial class fmReplaceDialog : Form
    {
        public fmReplaceDialog(string message)
        {
            InitializeComponent();
            label1.Text = message;
        }
    }
}