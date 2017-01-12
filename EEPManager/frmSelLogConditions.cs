using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace EEPManager
{
    public partial class frmSelLogConditions : Form
    {
        private string _startDate = DateTime.MinValue.ToString("yyyyMMdd");
        private string _endDate = DateTime.MaxValue.ToString("yyyyMMdd");
        private DateTime _startDateValue = DateTime.MinValue;
        private DateTime _endDateValue = DateTime.MaxValue;
        private bool _showErrorItem = false;

        public frmSelLogConditions(bool showErrorItem)
        {
            InitializeComponent();
            _showErrorItem = showErrorItem;
        }

        private void frmSelLogConditions_Load(object sender, EventArgs e)
        {
            this.chkErrorAnalysis.Checked = false;
            this.chkErrorAnalysis.Enabled = _showErrorItem;
        }

        public string StartDate
        {
            get { return _startDate; }
        }

        public DateTime StartDateValue
        {
            get { return _startDateValue; }
        }

        public string EndDate
        {
            get { return _endDate; }
        }

        public DateTime EndDateValue
        {
            get { return _endDateValue; }
        }

        public bool ExportError
        {
            get { return this.chkErrorAnalysis.Checked; }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (this.dtpFrom.Checked)
            { 
                _startDate = this.dtpFrom.Value.ToString("yyyyMMdd");
                _startDateValue = this.dtpFrom.Value.Date;
            }
            if(this.dtpTo.Checked)
            {
                _endDate = this.dtpTo.Value.ToString("yyyyMMdd");
                _endDateValue = this.dtpTo.Value.Date;
            }
        }
    }
}