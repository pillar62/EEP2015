using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

namespace MWizard2015
{
    public partial class fmWizardMain : Form
    {
        public fmWizardMain()
        {
            InitializeComponent();
        }

        public string ShowEEPWizard()
        {
            string Result = "";
            if (ShowDialog() == DialogResult.OK)
            {
                if (rbServerPackage.Checked)
                    Result = "Server";
                if (rbWinFormPackage.Checked)
                    Result = "Client";
                if (rbWebForm.Checked)
                    Result = "Web";
                if (rbEmptySolution.Checked)
                    Result = "NewEmptySolution";
                if (rbWebReport.Checked)
                    Result = "WebReport";
                if (rbWinReport.Checked)
                    Result = "WinReport";
                if (rbJQueryWebForm.Checked)
                    Result = "JQuery";
                if (rbJQMobileForm.Checked)
                    Result = "JQMobile";
                if (rbJQueryToJQMobile.Checked)
                    Result = "JQueryToJQMobile"; 
                if (rbSetServerPath.Checked)
                    Result = "ServerPath";
                if (rbRDLC.Checked)
                    Result = "RDLC";
            }
            else
            {
                Result = "Cancel";
            }
            return Result;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (rbServerPackage.Checked)
            {
            }
        }

        private void RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
            {
                DisableAllRadioButton(((sender as RadioButton).Parent as Panel));
            }
        }

        private void DisableAllRadioButton(Panel p)
        {
            if (p.Name == "panelAdo")
            {
                DisableRadioButton(panelSolution);
            }
            else if (p.Name == "panelEF")
            {
                DisableRadioButton(panelAdo);
                DisableRadioButton(panelSolution);
            }
            else if (p.Name == "panelSolution")
            {
                DisableRadioButton(panelAdo);
            }
        }

        private void DisableRadioButton(Panel p)
        {
            for (int i = 0; i < p.Controls.Count; i++)
            {
                if (p.Controls[i] is RadioButton)
                    (p.Controls[i] as RadioButton).Checked = false;
            }
        }
    }
}