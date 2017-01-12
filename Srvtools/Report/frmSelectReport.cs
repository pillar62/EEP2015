using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Srvtools.Report
{
    public partial class frmSelectReport : Form
    {
        public frmSelectReport()
        {
            InitializeComponent();
        }

        public string ReturnValue { get; set; }

        private void frmSelectReport_Load(object sender, EventArgs e)
        {
            if (!this.DesignMode)
            {
                this.infoDataSetSYS_REPORT.Active = true;
                foreach (DataRow row in this.infoDataSetSYS_REPORT.RealDataSet.Tables[0].Rows)
                {
                    if (!this.listBoxReportID.Items.Contains(row["REPORTID"].ToString()))
                    {
                        this.listBoxReportID.Items.Add(row["REPORTID"].ToString());
                    }
                }

               
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (this.listBoxReportID.SelectedIndex != -1)
            {
                DialogResult = System.Windows.Forms.DialogResult.OK;
                if (this.listBoxTemplateName.SelectedIndex != -1)
                {
                    this.ReturnValue = string.Format("{0};{1}", this.listBoxReportID.SelectedItem.ToString(), this.listBoxTemplateName.SelectedItem.ToString());
                }
                else
                {
                    this.ReturnValue = this.listBoxReportID.SelectedItem.ToString();
                }
            }
            else
            {
                MessageBox.Show("Please select ReportID first !");
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void listBoxReportID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxReportID.SelectedIndex != -1)
            {
                foreach (DataRow row in this.infoDataSetSYS_REPORT.RealDataSet.Tables[0].Rows)
                {
                    if (row["REPORTID"].ToString() == this.listBoxReportID.SelectedItem.ToString())
                    {
                        this.listBoxTemplateName.Items.Add(row["FILENAME"].ToString());
                    }
                }
            }
        }
    }
}
