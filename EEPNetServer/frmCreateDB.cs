using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Srvtools;

namespace EEPNetServer
{
    public partial class frmCreateDB : Form
    {
        public frmCreateDB()
        {
            InitializeComponent();
        }

        public radioType x;
        private void OKbutton_Click(object sender, EventArgs e)
        {
            if (radioTypical.Checked == true)
                x = radioType.typical;
            else if (radioSimplified.Checked == true)
                x = radioType.simplified;
            else if (radioEEP7M.Checked == true)
                x = radioType.EEP7m;
            else if (radioEEP2006.Checked)
                x = radioType.EEP2006m;
            else if (radioWorkFlow.Checked)
                x = radioType.WorkFlow;
            else if (radioEEPCloudSystemTable.Checked)
                x = radioType.EEPCloud;
        }

        private void frmCreateDB_Load(object sender, EventArgs e)
        {
            SYS_LANGUAGE language = CliUtils.fClientLang;
            string text = SysMsg.GetSystemMessage(language, "EEPNetServer", "CreateDB", "radioText");
            string[] radioText = text.Split(';');
            radioTypical.Text = radioText[0];
            radioSimplified.Text = radioText[1];
        }

        private void radioTypical_CheckedChanged(object sender, EventArgs e)
        {


        }
    }

    public enum radioType
    {
        typical = 0,
        simplified = 1,
        EEP7m = 2,
        EEP2006m = 3,
        WorkFlow = 4,
        EEPCloud = 5
    }
}