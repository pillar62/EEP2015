using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Xml;
using System.IO;
using Srvtools;

namespace EEPNetServer
{
    public partial class frmLoginMan : Form
    {
        private bool bHasSaved = true;

        public frmLoginMan()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
           
            try
            {
                SaveDomain();
                if (checkBoxEnableDatabase.Checked)
                {
                    CheckTable();
                }
                SrvGL.SaveConfig(chkAllowPerLogin.Checked, checkBoxLoginInSameIP.Checked, checkBoxEnableDatabase.Checked);
                SrvGL.ResetAllowLoginInOtherPC();
                SrvGL.ResetAllowLoginInSameIP();
                ServerConfig.SaveUserTableConfig(checkBoxUserTable.Checked
                    , textBoxTable.Text, textBoxUserID.Text, textBoxUserName.Text, textBoxPassword.Text);
                SavePasswordPolicy();
            }
            catch(Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            bHasSaved = true;

            this.Close();
        }

        private void SavePasswordPolicy()
        { 
            string s = SystemFile.LoginFile;
            XmlDocument xml = new XmlDocument();
            XmlNode node = null;
            if (File.Exists(s))
            {
                xml.Load(s);
                node = xml.SelectSingleNode("InfolightAllowUserToPerLogin");
            }
            if (node == null)
            {
                node = xml.AppendChild(xml.CreateElement("InfolightAllowUserToPerLogin"));
            }
            XmlNode nodePolicy = node.SelectSingleNode("PasswordPolicy");
            if (nodePolicy == null)
            {
                nodePolicy = node.AppendChild(xml.CreateElement("PasswordPolicy"));
            }
            nodePolicy.RemoveAll();
            XmlAttribute att = xml.CreateAttribute("MinSize");
            ServerConfig.PassWordMinSize = Convert.ToInt32(textBoxPasswordMinSize.Text);
            att.Value = ServerConfig.PassWordMinSize.ToString();
            nodePolicy.Attributes.Append(att);
            att = xml.CreateAttribute("MaxSize");
            ServerConfig.PasswordMaxSize = Convert.ToInt32(textBoxPasswordMaxSize.Text);
            att.Value = ServerConfig.PasswordMaxSize.ToString();
            nodePolicy.Attributes.Append(att);
            att = xml.CreateAttribute("CharNum");
            ServerConfig.PasswordCharNum = checkBoxPasswordCharNum.Checked;
            att.Value = checkBoxPasswordCharNum.Checked.ToString();
            nodePolicy.Attributes.Append(att);
            att = xml.CreateAttribute("PassWrodExpiry");
            ServerConfig.PassWrodExpiry = Convert.ToInt32(textBoxPassWrodExpiry.Text);
            att.Value = ServerConfig.PassWrodExpiry.ToString();
            nodePolicy.Attributes.Append(att);
            xml.Save(s);
        }

        private void SaveDomain()
        {


            XmlDocument DomainXml = new XmlDocument();

            XmlNode root = DomainXml.CreateElement("Infolight");

            //xn.Attributes["Path"].Value = tbDomain.Text;
            //xn.Attributes["User"].Value = tbAdministrator.Text;
            //xn.Attributes["Password"].Value = ServerConfig.Encrypt(tbAdministrator.Text, tbPassword.Text);


            for (int i = 0; i < dataGridViewAD.Rows.Count; i++)
            {

                var row = dataGridViewAD.Rows[i];
                if (i != dataGridViewAD.NewRowIndex)
                {
                    var path = (string)row.Cells[0].Value;
                    var user = (string)row.Cells[1].Value;
                    var password = (string)row.Cells[2].Value;

                    XmlNode node = DomainXml.CreateElement("Domain");
                    var attPath = DomainXml.CreateAttribute("Path");
                    attPath.Value = path;
                    node.Attributes.Append(attPath);
                    var attUser = DomainXml.CreateAttribute("User");
                    attUser.Value = user;
                    node.Attributes.Append(attUser);
                    var attPasword = DomainXml.CreateAttribute("Password");
                    attPasword.Value = ServerConfig.Encrypt(user, password);
                    node.Attributes.Append(attPasword);
                    root.AppendChild(node);
                }
            }

            DomainXml.AppendChild(root);

            DomainXml.Save(SystemFile.DomainFile);
            ServerConfig.LoadDomainConfig();


        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmLoginMan_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!bHasSaved)
            {
                if (MessageBox.Show("Setting has been changed. Are you sure to save them?", "Confirmation", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    btnOK_Click(null, null);
                }
            }
        }

        private void chkAllowPerLogin_CheckedChanged(object sender, EventArgs e)
        {
            bHasSaved = false;
        }

        private void frmLoginMan_Load(object sender, EventArgs e)
        {
            chkAllowPerLogin.Checked = SrvGL.AllowLoginInOtherPC;
            checkBoxLoginInSameIP.Checked = SrvGL.AllowLoginInSameIP;
            checkBoxEnableDatabase.Checked = SrvGL.EnableDataBase;
            //tbDomain.Text = ServerConfig.DomainPath;
            //tbAdministrator.Text = ServerConfig.DomainUser;
            //tbPassword.Text = ServerConfig.DomainPassword;
           
            foreach (var domain in ServerConfig.Domains)
            {
                var index = dataGridViewAD.Rows.Add();
                dataGridViewAD.Rows[index].Cells[0].Value = domain.Path;
                dataGridViewAD.Rows[index].Cells[1].Value = domain.User;
                dataGridViewAD.Rows[index].Cells[2].Value = domain.Password;
                //var index =  dataGridViewAD.Rows.Add();
            }
            
            

            checkBoxUserTable.Checked = ServerConfig.UserDefination;
            groupBoxUserTable.Enabled = checkBoxUserTable.Checked;
            textBoxTable.Text = ServerConfig.UserTable;
            textBoxUserID.Text = ServerConfig.UserID;
            textBoxUserName.Text = ServerConfig.UserName;
            textBoxPassword.Text = ServerConfig.Password;
            textBoxPasswordMinSize.Text = ServerConfig.PassWordMinSize.ToString();
            textBoxPasswordMaxSize.Text = ServerConfig.PasswordMaxSize.ToString();
            checkBoxPasswordCharNum.Checked = ServerConfig.PasswordCharNum;
            textBoxPassWrodExpiry.Text = ServerConfig.PassWrodExpiry.ToString();
            bHasSaved = true;
        }

        private void btnTest_Click(object sender, EventArgs e)
        {


            if (dataGridViewAD.SelectedRows.Count > 0)
            {

                var row = dataGridViewAD.SelectedRows[0];
                var path = (string)row.Cells[0].Value;
                var user = (string)row.Cells[1].Value;
                var password = (string)row.Cells[2].Value;
                var ad = new ADClass() { ADPath = "LDAP://" + path, ADUser = user, ADPassword = password };
                if (ad.TestDirecoryOjbect())
                {
                    MessageBox.Show("Success to Connect to Domain:" + path, "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Fail to Connect to Domain:" + path, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
    
        }

        private void checkBoxUserTable_CheckedChanged(object sender, EventArgs e)
        {
            groupBoxUserTable.Enabled = checkBoxUserTable.Checked;
        }

        private IDbConnection AllocateConnection()
        {
            var systemDatabase = DbConnectionSet.GetSystemDatabase(null);
            return DbConnectionSet.GetDbConn(systemDatabase).CreateConnection();
        }

        private void CheckTable()
        {

            using (var connection = AllocateConnection())
            {
                connection.Open();
                string checksql = string.Empty;
                if (connection is System.Data.OracleClient.OracleConnection)
                {
                    checksql = "select count(*) from USER_OBJECTS where OBJECT_TYPE = 'TABLE' and OBJECT_NAME='SYS_EEP_USERS'";
                }
                else
                {
                    checksql = "select count(*) from sysobjects where xtype in('u','U') and name='SYS_EEP_USERS'";
                }

                IDbCommand cmd = connection.CreateCommand();
                cmd.CommandText = checksql;
                object obj = cmd.ExecuteScalar();
                if (obj != null && Convert.ToInt32(obj) > 0)
                {

                }
                else
                {
                    var createsql = string.Empty;
                    if (connection is System.Data.OracleClient.OracleConnection)
                    {
                        createsql = "CREATE TABLE SYS_EEP_USERS ("
                       + "USERID varchar2 (20) NULL,"
                       + "USERNAME nvarchar2 (50) NULL,"
                       + "COMPUTER nvarchar2 (50) NULL,"
                       + "LOGINTIME nvarchar2 (50) NULL, "
                       + "LASTACTIVETIME nvarchar2 (50) NULL,"
                       + "LOGINCOUNT int NULL"
                       + ")";
                    }
                    else
                    {
                        createsql = "CREATE TABLE SYS_EEP_USERS ("
                        + "USERID varchar (20) NULL,"
                        + "USERNAME nvarchar (50) NULL,"
                        + "COMPUTER nvarchar (50) NULL,"
                        + "LOGINTIME nvarchar (50) NULL, "
                        + "LASTACTIVETIME nvarchar (50) NULL,"
                        + "LOGINCOUNT int NULL"
                        + ")";
                    }


                    cmd.CommandText = createsql;
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void dataGridViewAD_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {

            if (dataGridViewAD.CurrentCell.ColumnIndex == 2)
            {
                TextBox txt = e.Control as TextBox;
                if (txt != null)
                {
                    txt.UseSystemPasswordChar = true;
                }
            }
            else
            {
                TextBox txt = e.Control as TextBox;
                if (txt != null)
                {
                    txt.UseSystemPasswordChar = false;
                }
            }
        }

        //添加事件2
        private void dataGridViewAD_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.Value != null)
            {
                string s = e.Value as string;
                if (e.ColumnIndex == 2)
                {
                    e.Value = "".PadLeft(s.Length, '*');
                }
            }
        }
    }
}