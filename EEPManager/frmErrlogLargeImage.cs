using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace EEPManager
{
    public partial class frmErrlogLargeImage : Form
    {
        public System.Drawing.Image picture;

        public frmErrlogLargeImage()
        {
            InitializeComponent();
        }

        public frmErrlogLargeImage(System.Drawing.Image x)
        {
            InitializeComponent();
            picture = x;
        }

        private void frmErrlogLargeImage_Load(object sender, EventArgs e)
        {
            this.pictureBox1.Image = this.picture;
            pictureBox1.Show();
        }

        private void ok_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}