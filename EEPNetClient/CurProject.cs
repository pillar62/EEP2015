using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Srvtools;

namespace EEPNetClient
{
    public partial class CurProject : Form
    {
        private frmClientMain MainForm;
        public CurProject(frmClientMain MainForm)
        {
            InitializeComponent();
            this.MainForm = MainForm;
        }

        private void CurProject_Load(object sender, EventArgs e)
        {
            DataSet dsSolution = new DataSet();
            if (CliUtils.fSolutionSecurity)
            {
                object[] myRet1 = CliUtils.CallMethod("GLModule", "GetSolutionSecurity", new object[] { CliUtils.fLoginUser, CliUtils.fGroupID });
                if ((null != myRet1) && (0 == (int)myRet1[0]))
                    dsSolution = ((DataSet)myRet1[1]);
            }
            else
            {
                object[] myRet1 = CliUtils.CallMethod("GLModule", "GetSolution", null);
                if ((null != myRet1) && (0 == (int)myRet1[0]))
                    dsSolution = ((DataSet)myRet1[1]);
            }
            this.infoCmbSolution.DataSource = dsSolution;
            string strTableName = dsSolution.Tables[0].TableName;
            this.infoCmbSolution.DisplayMember = strTableName + ".itemname";
            this.infoCmbSolution.ValueMember = strTableName + ".itemtype";
            int i = dsSolution.Tables[0].Rows.Count;
            for (int j = 0; j < i; j++)
            {
                if (string.Compare(dsSolution.Tables[0].Rows[j]["itemtype"].ToString(), CliUtils.fCurrentProject, true) == 0)//IgnoreCase
                {
                    this.infoCmbSolution.SelectedValue = dsSolution.Tables[0].Rows[j]["itemtype"].ToString();
                }
            }

            //this.infoCmbSolution.SelectedValue = CliUtils.fCurrentProject;
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            int length = MainForm.MdiChildren.Length;
            for (int i = 0; i < length; i++)
            {
                MainForm.MdiChildren[0].Close();
                if (i + MainForm.MdiChildren.Length >= length)
                {
                    return;
                }
            }

            //CliUtils.fCurrentProject = this.infoCmbSolution.SelectedValue.ToString();
            if (this.MainForm.cmbSolution.SelectedValue.ToString() == this.infoCmbSolution.SelectedValue.ToString())
                this.MainForm.cmbSolution.SelectedValue = "";
            this.MainForm.cmbSolution.SelectedValue = this.infoCmbSolution.SelectedValue;
            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}