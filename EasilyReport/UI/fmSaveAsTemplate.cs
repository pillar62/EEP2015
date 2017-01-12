using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Infolight.EasilyReportTools.Tools;
using Infolight.EasilyReportTools.DataCenter;
using System.IO;

namespace Infolight.EasilyReportTools.UI
{
    public partial class fmSaveAsTemplate : Form
    {
        DBGateway dbGateway;
        public fmSaveAsTemplate(IReport report, bool designTime)
        {
            InitializeComponent();
            dbGateway = new DBGateway(report);

            if (!designTime)
            {
                btOK.Text = ERptMultiLanguage.GetLanValue("btOK");
                btCancel.Text = ERptMultiLanguage.GetLanValue("btCancel");
                this.Text = ERptMultiLanguage.GetLanValue("fmSaveAsTemplate");
                lbFileName.Text = ERptMultiLanguage.GetLanValue("lbTemplateName");
            }
        }

        private string templateName;

        public string TemplateName
        {
            get { return templateName; }
        }

        private void btOK_Click(object sender, EventArgs e)
        {
          
            string fileName = Path.GetFileNameWithoutExtension(tbFileName.Text.Trim());
            if (fileName.Length > 0)
            {
                ExecutionResult exeRes = SaveTemplate(fileName);
                if (exeRes.Status)
                {
                    templateName = fileName;
                }
                else
                {
                    MessageBox.Show(exeRes.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    tbFileName.Focus();
                    DialogResult = DialogResult.None;
                }
            }
            else
            {
                MessageBox.Show(MessageInfo.TemplateFileNameIsNull, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tbFileName.Focus();
                DialogResult = DialogResult.None;
            }
        }

        public ExecutionResult SaveTemplate(string filename)
        {
            return dbGateway.SaveTemplate(filename);
        }

        private void fmSaveAsTemplate_Load(object sender, EventArgs e)
        {
            tbFileName.Focus();
        }
    }
}