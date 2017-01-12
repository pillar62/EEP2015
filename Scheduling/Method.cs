using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using Srvtools;

namespace Scheduling
{
    public partial class Method : Form
    {
        public Method()
        {
            InitializeComponent();
        }

        private String solution;
        private String packageName;
        public String method;
        public Method(String pName, String sName)
        {
            packageName = pName;
            if (packageName == "GLModule")
                solution = "";
            else
                solution = sName;
            InitializeComponent();
        }

        private void Metnod_Load(object sender, EventArgs e)
        {
            ArrayList methodList = LoadAllServiceName();
            lbxServices.DataSource = methodList;
        }

        private ArrayList LoadAllServiceName()
        {
            ArrayList list = new ArrayList();
            if (packageName == "FLRuntime")
            {
                list.Add("Approve2");
                list.Add("DelaySendMail");
            }
            else
            {
                object[] myRet = CliUtils.CallMethod("GLModule", "GetMethodName", new object[] { solution, packageName });
                if (myRet != null && myRet[0].ToString() == "0")
                {
                    list = myRet[1] as ArrayList;
                }
            }
            return list;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (lbxServices.SelectedItem != null)
                method = lbxServices.SelectedItem.ToString();
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}