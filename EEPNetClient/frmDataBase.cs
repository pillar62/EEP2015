using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Microsoft.Win32;
using System.IO;
using Srvtools;
using System.Collections;

namespace EEPNetClient
{
    public partial class frmDataBase : Form
    {
        //new modify by ccm
        //private frmClientMain MainForm = new frmClientMain();
        private frmClientMain MainForm;
        public frmDataBase(frmClientMain MainForm)
        {
            InitializeComponent();
            this.MainForm = MainForm;
        }

        private void frmDataBase_Load(object sender, EventArgs e)
        {
            GetDataBaseName();
        }

        private void GetDataBaseName()
        {
            ArrayList dba = new ArrayList();
            object[] myRet = CliUtils.CallMethod("GLModule", "GetDB", null);
            if (myRet != null && (int)myRet[0] == 0)
            {
                dba = (ArrayList)myRet[1];
                int count = dba.Count;
                for (int i = 0; i < count; i++)
                {
                    infoCmbDataBase.Items.Add(dba[i].ToString());

                }
            }
            this.infoCmbDataBase.Text = CliUtils.fLoginDB;
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            CheckUser();
        }

        private void CheckUser()
        {
            CheckUser(false);
        }

        private void CheckUser(bool relogin)
        {
            string strdb = this.infoCmbDataBase.Text;
            string sParam = CliUtils.fLoginUser + ':' + CliUtils.fLoginPassword + ':' + strdb;
            if (relogin)
            {
                sParam += ":1";
            }
            else
            {
                sParam += ":0";
            }
            object[] myRet = CliUtils.CallMethod("GLModule", "CheckUser", new object[] { (object)sParam });
            LoginResult result = (LoginResult)myRet[1];
            switch (result)
            {
                case LoginResult.UserNotFound:
                    {
                        string message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "EEPWebNetClient", "WinSysMsg", "msg_UserNotFound");
                        MessageBox.Show(this, message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    }
                case LoginResult.PasswordError:
                    {
                        string message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "EEPWebNetClient", "WinSysMsg", "msg_UserOrPasswordError");
                        MessageBox.Show(this, message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    }
                case LoginResult.Disabled:
                    {
                        string message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "EEPWebNetClient", "WinSysMsg", "msg_UserDisabled");
                        MessageBox.Show(this, message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    }
                case LoginResult.UserLogined:
                    {
                        string message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "EEPWebNetClient", "WinSysMsg", "msg_UserIsLogined");
                        MessageBox.Show(this, string.Format(message, CliUtils.fLoginUser), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    }
                case LoginResult.RequestReLogin:
                    {
                        string message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "EEPWebNetClient", "WinSysMsg", "msg_UserReLogined");
                        if (MessageBox.Show(string.Format(message, CliUtils.fLoginUser)
                            , "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                        {
                            CheckUser(true);
                        }
                        return;
                    }
                default:
                    {
                        CliUtils.fUserName = myRet[2].ToString();
                        CliUtils.fLoginUser = myRet[3].ToString();
                        CliUtils.fLoginDB = strdb;
                        this.MainForm.DoLoad();
                        break;
                    }
            }
            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}