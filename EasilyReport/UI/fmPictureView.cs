using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Infolight.EasilyReportTools.Tools;

namespace Infolight.EasilyReportTools.UI
{
    public partial class fmPictureView : Form
    {
        private IReport report;
        
        public fmPictureView(IReport rpt, int index)
        {
            InitializeComponent();
            this.report = rpt;

            this.Text = ERptMultiLanguage.GetLanValue("fmPictureView");
            this.pictureBox.Image = this.report.Images[index].Image;
        }

        private void fmPictureView_Load(object sender, EventArgs e)
        {

        }
    }
}