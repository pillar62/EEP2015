using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Collections;
using System.IO;
using System.Xml;
using System.Net.Mail;

namespace Srvtools
{
    public partial class frmMenuTableControl : InfoForm
    {
        public frmMenuTableControl()
        {
            InitializeComponent();
        }
        public frmMenuTableControl(string menuid)
        {
            InitializeComponent();
            MENUID = menuid;
            dscmdMENUTABLECONTROL.SetWhere("MENUID='" + menuid + "'");
        }

        private void frmAgent_Load(object sender, EventArgs e)
        {
        }

        public object GetMENUID()
        {
            return MENUID;
        }
        public string MENUID { get; set; }

        void navRoleAgent_BeforeItemClick(object sender, BeforeItemClickEventArgs e)
        {
        }

        private static string GetPwdString(string s)
        {
            string sRet = "";
            for (int i = 0; i < s.Length; i++)
            {
                sRet = sRet + (char)(((int)(s[s.Length - 1 - i])) ^ s.Length);
            }
            return sRet;
        }
    }
}