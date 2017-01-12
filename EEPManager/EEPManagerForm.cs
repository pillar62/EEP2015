using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Srvtools;
using System.Runtime.Remoting;
using System.Text.RegularExpressions;

namespace EEPManager
{
    public partial class EEPManagerForm : Form
    {
        public EEPManagerForm()
        {
            InitializeComponent();
        }

        private bool Register(bool isShowMessage)
        {
            string message = "";
            bool rtn = CliUtils.Register(ref message, false);
            if (rtn)
            {
                CliUtils.GetSysXml(Application.StartupPath + @"\sysmsg.xml");
            }
            else
            {
                if (isShowMessage)
                {
                    MessageBox.Show(message);
                }
            }
            return rtn;
        }

        frmLogin fFrmLogin = null;
        private void EEPManagerForm_Load(object sender, EventArgs e)
        {
            try
            {
                fFrmLogin = new frmLogin();
                if (fFrmLogin.ShowDialog(this) == DialogResult.OK)
                {
                    CheckUser();
                }
                else
                {
                    this.Close();
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
                CliUtils.fLoginUser = string.Empty;
                this.Close();
            }
        }

        private void CheckUser()
        {
            CheckUser(false);
        }

        private void CheckUser(bool relogin)
        {
            CliUtils.fLoginUser = fFrmLogin.GetUserId();
            CliUtils.fLoginPassword = fFrmLogin.GetPwd();
            CliUtils.fLoginDB = fFrmLogin.GetDB();
            LoginResult result = LoginResult.Success;
            if (CliUtils.fLoginUser.Contains("'"))
            {
                result = LoginResult.UserNotFound;
            }
            else
            {
                EEPRemoteModule module = Activator.GetObject(typeof(EEPRemoteModule)
                       , string.Format("http://{0}:{1}/InfoRemoteModule.rem", CliUtils.fRemoteIP, CliUtils.fRemotePort)) as EEPRemoteModule;
                try
                {
                    module.ToString();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                    CheckUser();
                    return;
                }

                string sParam = CliUtils.fLoginUser + ':' + CliUtils.fLoginPassword + ':' + CliUtils.fLoginDB;
                if (relogin)
                {
                    sParam += ":1";
                }
                else
                {
                    sParam += ":0";
                }

                object[] myRet = module.CallMethod(new object[] { CliUtils.GetBaseClientInfo() }, "GLModule", "CheckManagerRight", new object[] { CliUtils.fLoginDB, CliUtils.fLoginUser });
                if (myRet[1].ToString() != "0")
                {
                    if (myRet[1].ToString() == "1")
                    {
                        MessageBox.Show("No right to use Manager.");
                    }
                    else
                    {
                        MessageBox.Show("User Not Found.");
                    }
                    if (fFrmLogin.ShowDialog(this) == DialogResult.OK)
                    {
                        CheckUser();
                    }
                    else
                    {
                        this.Close();
                    }
                    return;
                }

                myRet = module.CallMethod(new object[] { CliUtils.GetBaseClientInfo() }, "GLModule", "CheckUser", new object[] { (object)sParam });
                result = (LoginResult)myRet[1];
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
                            WellKnownClientTypeEntry clientEntry = new WellKnownClientTypeEntry(typeof(EEPRemoteModule),
                               string.Format("http://{0}:{1}/InfoRemoteModule.rem", CliUtils.fRemoteIP, CliUtils.fRemotePort));
                            RemotingConfiguration.RegisterWellKnownClientType(clientEntry);
                            CliUtils.fUserName = myRet[2].ToString();
                            CliUtils.fLoginUser = myRet[3].ToString();
                            CliUtils.GetPasswordPolicy();
                            myRet = CliUtils.CallMethod("GLModule", "GetUserGroup", new object[] { CliUtils.fLoginUser });
                            if (myRet != null && (int)myRet[0] == 0)
                            {
                                CliUtils.fGroupID = myRet[1].ToString();
                            }
                            SaveToManagerXML(CliUtils.fLoginUser, CliUtils.fLoginDB, CliUtils.fRemoteIP + ":" + CliUtils.fRemotePort.ToString());

                            object[] DataBaseType = CliUtils.CallMethod("GLModule", "GetDataBaseType", new object[] { CliUtils.fLoginDB, false });
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
                            DisabledSystemMenuItem(!CliUtils.fLoginDBSplit);
                            break;
                        }
                }
            }
            if (result != LoginResult.Success)
            {
                if (fFrmLogin.ShowDialog(this) == DialogResult.OK)
                {
                    CheckUser();
                }
                else
                {
                    CliUtils.fLoginUser = string.Empty;
                    this.Close();
                }
            }
        }

        private void DisabledSystemMenuItem(bool enabled)
        {
            menuItemSecurityManager.Enabled = enabled;
            menuItemSolutionDefine.Enabled = enabled;
            menuItemPackageManager.Enabled = enabled;
            menuItemRefVal.Enabled = enabled;
            menuItemEM.Enabled = enabled;
            menuItemSLVFD.Enabled = enabled;
        }

        private void SaveToManagerXML(string sLoginUser, string sLoginDB, string sLoginServer)
        {
            String sfile = Application.StartupPath + "\\EEPManager.xml";
            string sUser = sLoginUser;
            string sDB = sLoginDB;
            string sServer = sLoginServer;
            string stemp = "";
            XmlDocument xml = new XmlDocument();
            if (File.Exists(sfile))
            {
                xml.Load(sfile);
                XmlNode el = xml.DocumentElement;
                foreach (XmlNode xNode in el.ChildNodes)
                {
                    if (string.Compare(xNode.Name, "USER", true) == 0)//IgnoreCase
                    {
                        stemp = xNode.InnerText.Trim();
                        string[] ss = stemp.Split(new char[] { ',' });
                        foreach (string s in ss)
                        {
                            if (!s.Equals(sLoginUser))
                                sUser = sUser + "," + s;
                        }
                    }
                    if (string.Compare(xNode.Name, "SERVER", true) == 0)
                    {
                        stemp = xNode.InnerText.Trim();
                        string[] ss = stemp.Split(new char[] { ',' });
                        foreach (string s in ss)
                        {
                            if (!s.Equals(sLoginServer))
                                sServer = sServer + "," + s;
                        }
                    }
                }

                File.Delete(sfile);
            }
            else
            {
                sUser = sLoginUser; sDB = sLoginDB;
            }

            FileStream aFileStream = new FileStream(sfile, FileMode.Create);
            try
            {
                XmlTextWriter w = new XmlTextWriter(aFileStream, new System.Text.ASCIIEncoding());
                w.Formatting = Formatting.Indented;
                w.WriteStartElement("LoginInfo");

                w.WriteStartElement("User");
                w.WriteValue(sUser);
                w.WriteEndElement();

                w.WriteStartElement("DataBase");
                w.WriteValue(sDB);
                w.WriteEndElement();

                w.WriteStartElement("Server");
                w.WriteValue(sServer);
                w.WriteEndElement();

                w.WriteEndElement();
                w.Close();
            }
            finally
            {
                aFileStream.Close();
            }
        }

        private void menuItemExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void EEPManagerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (CliUtils.fLoginUser.Length > 0)
                {
                    if (CliUtils.fLoginUser != "")
                    {
                        CliUtils.CallMethod("GLModule", "LogOut", new object[] { (object)(CliUtils.fLoginUser) });
                    }
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void menuItemSecurityManager_Click(object sender, EventArgs e)
        {
            if (CliUtils.fLoginUser != "")
            {
                frmSecurityMain securityForm = new frmSecurityMain();
                securityForm.ShowDialog();
            }
        }

        private void menuItemPackageManager_Click(object sender, EventArgs e)
        {
            if (CliUtils.fLoginUser != "")
            {
                PackageManagerForm packageManager = new PackageManagerForm();
                packageManager.ShowDialog();
            }
        }

        private void menuItemSolutionDefine_Click(object sender, EventArgs e)
        {
            if (CliUtils.fLoginUser != "")
            {
                SolutionDefineForm solutionDefine = new SolutionDefineForm();
                solutionDefine.ShowDialog();
            }
        }

        //private void menuItemSourceControl_Click(object sender, EventArgs e)
        //{
        //if (CliUtils.fLoginUser != "")
        //{
        //    SourceControlForm sourceControl = new SourceControlForm();
        //    sourceControl.ShowDialog();
        //}
        //}

        private void menuItemDD_Click(object sender, EventArgs e)
        {
            if (CliUtils.fLoginUser != "")
            {
                frmDataDictonary frmDD = new frmDataDictonary();
                frmDD.ShowDialog();
            }
        }

        private void menuItemEM_Click(object sender, EventArgs e)
        {
            if (CliUtils.fLoginUser != "")
            {
                frmErrorLogMaintenance frmEM = new frmErrorLogMaintenance();
                frmEM.ShowDialog();
            }
        }

        private void menuItemSLVFD_Click(object sender, EventArgs e)
        {
            if (CliUtils.fLoginUser != "")
            {
                frmSysLogViewerForDB frmSLVfDB = new frmSysLogViewerForDB();
                frmSLVfDB.ShowDialog();
            }
        }

        private void menuItemDatabaseAlias_Click(object sender, EventArgs e)
        {
            if (CliUtils.fLoginUser != "")
            {
                frmDBALias frmdba = new frmDBALias();
                frmdba.ShowDialog();
                DisabledSystemMenuItem(!CliUtils.fLoginDBSplit);
            }
        }

        private void menuItemRefVal_Click(object sender, EventArgs e)
        {
            frmRefVal frmrv = new frmRefVal();
            frmrv.ShowDialog();
        }

        private void menuItemAbout_Click(object sender, EventArgs e)
        {
            FormVersion form = new FormVersion("About EEP Manager");
            form.ShowDialog(this);
        }

    }
}