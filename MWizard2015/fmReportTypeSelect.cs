using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using EnvDTE80;
using EnvDTE;

namespace MWizard2015
{
    public partial class fmReportTypeSelect : Form
    {
        public fmReportTypeSelect()
        {
            InitializeComponent();
        }

        private String Result;
        private DTE2 applicationObject;
        private AddIn addInInstance;
        public fmReportTypeSelect(String r, DTE2 ao, AddIn aii)
        {
            Result = r;
            applicationObject = ao;
            addInInstance = aii;
            InitializeComponent();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (this.rbEEPReport.Checked)
            {
                bool isWebRpt = (Result == "WebReport");
                frmEEPReport bForm = new frmEEPReport(applicationObject, addInInstance, isWebRpt);
                bForm.ShowDialog();
            }
            else if (this.rbEasilyReport.Checked)
            {
                bool isWebRpt = (Result == "WebReport");
                fmEasilyReport bForm = new fmEasilyReport(applicationObject, addInInstance, isWebRpt);
                bForm.ShowDialog();
            }
        }
    }
}