using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Srvtools;
using Microsoft.Reporting.WinForms;

namespace TAG_NAMESPACE
{
    public partial class TAG_FORMNAME : InfoForm
    {
        public TAG_FORMNAME()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            clientQuery1.Show(this.panel1);

            this.reportViewer1.LocalReport.SubreportProcessing += new Microsoft.Reporting.WinForms.SubreportProcessingEventHandler(LocalReport_SubreportProcessing);

            this.reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("NewDataSet_", this.DataSet.RealDataSet.Tables[0]));
        }

        void LocalReport_SubreportProcessing(object sender, Microsoft.Reporting.WinForms.SubreportProcessingEventArgs e)
        {
            e.DataSources.Add(new Microsoft.Reporting.WinForms.ReportDataSource("NewDataSet_", Detail));
        }

        private void btQuery_Click(object sender, EventArgs e)
        {
            clientQuery1.Execute(this.panel1);

            this.reportViewer1.RefreshReport();
        }

        private void btClear_Click(object sender, EventArgs e)
        {
            clientQuery1.Clear(this.panel1);
        }
    }
}

