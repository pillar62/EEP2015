using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace Srvtools
{
    public partial class frmSelUsers : Form
    {
        private SYS_LANGUAGE language;
        private string menuID;
        private string menuName;
        public frmSelUsers(string strID, string strName)
        {
            InitializeComponent();
            menuID = strID;
            menuName = strName;

            this.Text += " ( " + menuName + " )";
            infodsMenuUsers.RealDataSet = (DataSet)CliUtils.CallMethod("GLModule", "GetAllUsers", null)[1];
        }

        private void btnSelAll_Click(object sender, EventArgs e)
        {
            foreach (object selItem in lstLibrary.Items)
            {
                lstSelected.Items.Add(selItem);
            }
            lstLibrary.Items.Clear();
        }

        private void btnSel_Click(object sender, EventArgs e)
        {
            foreach (object selItem in lstLibrary.SelectedItems)
            {
                lstSelected.Items.Add(selItem);
            }

            ArrayList libRemoveList = new ArrayList();
            for (int i = 0; i < lstLibrary.SelectedItems.Count; i++)
            {
                libRemoveList.Add(lstLibrary.SelectedItems[i]);
            }
            for (int j = 0; j < libRemoveList.Count; j++)
            {
                lstLibrary.Items.Remove(libRemoveList[j]);
            }
        }

        private void btnRemoveAll_Click(object sender, EventArgs e)
        {
            foreach (object selItem in lstSelected.Items)
            {
                lstLibrary.Items.Add(selItem);
            }
            lstSelected.Items.Clear();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            foreach (object selItem in lstSelected.SelectedItems)
            {
                lstLibrary.Items.Add(selItem);
            }

            ArrayList selRemoveList = new ArrayList();
            for (int i = 0; i < lstSelected.SelectedItems.Count; i++)
            {
                selRemoveList.Add(lstSelected.SelectedItems[i]);
            }
            for (int j = 0; j < selRemoveList.Count; j++)
            {
                lstSelected.Items.Remove(selRemoveList[j]);
            }
        }

        private DataSet dsUsers;

        private void frmSelUsers_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < infodsMenuUsers.RealDataSet.Tables[0].Rows.Count; i++)
            {
                lstLibrary.Items.Add((object)("(" + infodsMenuUsers.RealDataSet.Tables[0].Rows[i]["UserID"] + ")" + infodsMenuUsers.RealDataSet.Tables[0].Rows[i]["UserName"]));
            }

            object[] myRet = CliUtils.CallMethod("GLModule", "LoadUsers", new object[] { menuID});
            if ((null != myRet) && (0 == (int)myRet[0]))
                dsUsers = (DataSet)(myRet[1]);

            for (int j = 0; j < dsUsers.Tables[0].Rows.Count; j++)
            {
                lstSelected.Items.Add((object)("(" + dsUsers.Tables[0].Rows[j]["UserID"] + ")" + dsUsers.Tables[0].Rows[j]["UserName"]));
            }
            ArrayList libRemoveList = new ArrayList();

            for (int k = 0; k < dsUsers.Tables[0].Rows.Count; k++)
            {
                libRemoveList.Add("(" + dsUsers.Tables[0].Rows[k]["UserID"] + ")" + dsUsers.Tables[0].Rows[k]["UserName"]);
            }
            for (int l = 0; l < libRemoveList.Count; l++)
            {
                lstLibrary.Items.Remove(libRemoveList[l]);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            ArrayList lstUserID = new ArrayList();
            for (int i = 0; i < lstSelected.Items.Count; i++)
            {
                string userID = GetUserID(lstSelected.Items[i].ToString());
                lstUserID.Add(userID);
            }

            CliUtils.CallMethod("GLModule", "SetUsers", new object[] { menuID, lstUserID});
            this.Close();
        }

        private string GetUserID(String strUserInfo)
        {
            String[] strTemp = strUserInfo.Split(new Char[] { '(', ')' });
            String strUserID = strTemp[1];
            return strUserID;
        }

        private void btnAC_Click(object sender, EventArgs e)
        {
            string Mid = menuID;
            String IdName = String.Empty;
            string Uid = String.Empty;
            language = CliUtils.fClientLang;
            if (lstSelected.SelectedItem != null)
                IdName = lstSelected.SelectedItem.ToString();
            else if (lstLibrary.SelectedItem != null)
                IdName = lstLibrary.SelectedItem.ToString();
            if (IdName == String.Empty)
            {
                language = CliUtils.fClientLang;
                string message = SysMsg.GetSystemMessage(language, "Srvtools", "InfoSecurity", "IsChooseUser");
                MessageBox.Show(message);
            }
            else
            {
                String[] str = IdName.Split(new char[] { '(', ')' });
                Uid = str[1];
                if (Mid == "" || Mid == null)
                {
                    language = CliUtils.fClientLang;
                    string message = SysMsg.GetSystemMessage(language, "Srvtools", "InfoSecurity", "IsChooseMenu");
                    MessageBox.Show(message);
                }
                else
                {
                    frmAccessControlForUser fac = new frmAccessControlForUser(Mid, menuName, Uid, IdName);
                    fac.ShowDialog();
                }
            }
        }
    }
}