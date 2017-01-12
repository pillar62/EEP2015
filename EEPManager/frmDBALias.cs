using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Srvtools;
using System.Collections;

namespace EEPManager
{
    public partial class frmDBALias : Form
    {
        public frmDBALias()
        {
            InitializeComponent();
        }
       
        private void frmDBALias_Load(object sender, EventArgs e)
        {
            GetDataBaseName();
        }

        private Hashtable DBSplit = new Hashtable();
        private void GetDataBaseName()
        {
            object[] myRet = CliUtils.CallMethod("GLModule", "GetDB", null);
            if (myRet != null && (int)myRet[0] == 0)
            {
                 ArrayList dba = (ArrayList)myRet[1];
                int count = dba.Count;
                for (int i = 0; i < count; i++)
                {
                    string db = dba[i].ToString();
                    cmbDataBase.Items.Add(dba[i].ToString());
                    if (DBSplit.Contains(db))
                    {
                        DBSplit[db] = ((ArrayList)myRet[2])[i].ToString();
                    }
                    else
                    {
                        DBSplit.Add(db, ((ArrayList)myRet[2])[i].ToString());
                    }
                }
            }
            cmbDataBase.Text = CliUtils.fLoginDB;
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
            string strdb = this.cmbDataBase.Text;
            string sParam = CliUtils.fLoginUser + ':' + CliUtils.fLoginPassword + ':' + strdb;
            if (relogin)
            {
                sParam += ":1";
            }
            else
            {
                sParam += ":0";
            }
            object[] myRet = CliUtils.CallMethod("GLModule", "CheckManagerRight", new object[] { CliUtils.fLoginDB, CliUtils.fLoginUser });
            if (myRet[1].ToString() != "0")
            {
                if (myRet[1].ToString() == "1")
                {
                    MessageBox.Show("No right to use Manager.");
                   
                }
                this.Close();
                return;
            }

            myRet = CliUtils.CallMethod("GLModule", "CheckUser", new object[] { (object)sParam });

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
                        else
                        {
                            CliUtils.fLoginUser = string.Empty;
                            this.Close();
                        }
                        return;
                    }
                default:
                    {
                        CliUtils.fUserName = myRet[2].ToString();
                        CliUtils.fLoginUser = myRet[3].ToString();
                        CliUtils.fLoginDB = strdb;
                        if (DBSplit.Contains(strdb))
                        {
                            CliUtils.fLoginDBSplit = (DBSplit[strdb].ToString() == "1");
                        }

                        object[] DataBaseType = CliUtils.CallMethod("GLModule", "GetDataBaseType", new object[] { CliUtils.fLoginDB });
                        if (DataBaseType != null && DataBaseType[0].ToString() == "0")
                        {
                            //0:None   1:Sql Server   2:OleDb   3:Oracle   4:Odbc   5:MySql
                            switch (DataBaseType[1].ToString())
                            {
                                case "1": CliUtils.fLoginDBType = ClientType.ctMsSql; break;
                                case "2": CliUtils.fLoginDBType = ClientType.ctOleDB; break;
                                case "3": CliUtils.fLoginDBType = ClientType.ctOracle; break;
                                case "4": CliUtils.fLoginDBType = ClientType.ctODBC; break;
                                case "5": CliUtils.fLoginDBType = ClientType.ctMySql; break;
                                case "6": CliUtils.fLoginDBType = ClientType.ctInformix; break;
                            }
                        }
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