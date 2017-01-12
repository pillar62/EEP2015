using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Data.SqlClient;

namespace Srvtools
{
    public partial class frmSelGroups : Form
    {
        private SYS_LANGUAGE language; 
        private string menuID;
        private string menuName;
        public frmSelGroups(string strID, string strName)
        {
            InitializeComponent();
            menuID = strID;
            menuName = strName;

            this.Text += " ( " + menuName + " )";
            infodsMenuGroups.RealDataSet = (DataSet)CliUtils.CallMethod("GLModule", "GetAllGroups", null)[1];
        }

        private void btnRemoveAll_Click(object sender, EventArgs e)
        {
            lstSelected.Items.Clear();
            lstLibrary.Items.Clear();
            for (int i = 0; i < infodsMenuGroups.RealDataSet.Tables[0].Rows.Count; i++)
            {
                lstLibrary.Items.Add("(" + infodsMenuGroups.RealDataSet.Tables[0].Rows[i]["GROUPID"].ToString() + ")" + infodsMenuGroups.RealDataSet.Tables[0].Rows[i]["GROUPNAME"].ToString());
            }
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

        private void btnSelAll_Click(object sender, EventArgs e)
        {
            //lstSelected.Items.Clear();
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
            for(int j = 0; j < libRemoveList.Count; j++)
            {
                lstLibrary.Items.Remove(libRemoveList[j]);
            }
        }

        private DataSet dsGroups;

        private void frmSelGroups_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < infodsMenuGroups.RealDataSet.Tables[0].Rows.Count; i++)
            {
                lstLibrary.Items.Add("(" + infodsMenuGroups.RealDataSet.Tables[0].Rows[i]["GROUPID"].ToString() +")" + infodsMenuGroups.RealDataSet.Tables[0].Rows[i]["GROUPNAME"].ToString());
            }

            object[] LoadParam = new object[1];
            LoadParam[0] = menuID;

            object[] myRet = CliUtils.CallMethod("GLModule", "LoadGroups", LoadParam);
            if ((null != myRet) && (0 == (int)myRet[0]))
                dsGroups = (DataSet)(myRet[1]);

            for (int j = 0; j < dsGroups.Tables[0].Rows.Count; j++)
            {
                lstSelected.Items.Add("(" + dsGroups.Tables[0].Rows[j]["GROUPID"].ToString() + ")" + dsGroups.Tables[0].Rows[j]["GROUPNAME"].ToString());
            }
            ArrayList libRemoveList = new ArrayList();

            for (int k = 0; k < dsGroups.Tables[0].Rows.Count; k++)
            {
                libRemoveList.Add("(" + dsGroups.Tables[0].Rows[k]["GROUPID"].ToString() + ")" + dsGroups.Tables[0].Rows[k]["GROUPNAME"].ToString());
            }
            for (int l = 0; l < libRemoveList.Count; l++)
            {
                lstLibrary.Items.Remove(libRemoveList[l]);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            dsGroups.Tables[0].Clear();
            object[] lst = new object[3];
            lst[0] = menuID;
            ArrayList strGroupID = new ArrayList();
            ArrayList strMenuID = new ArrayList();
            for (int i = 0; i < lstSelected.Items.Count; i++)
            {
                String[] str = lstSelected.Items[i].ToString().Split(new char[] { '(', ')' });

                strGroupID.Add(str[1]);
                strMenuID.Add(menuID);
            }
            lst[1] = strGroupID;
            lst[2] = strMenuID;

            CliUtils.CallMethod("GLModule", "setGroups", lst);

            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String Mid = menuID;
            String IdName = String.Empty;
            String Gid = String.Empty;
            //language = CliSysMegLag.GetClientLanguage();
            language = CliUtils.fClientLang;
            if (lstSelected.SelectedItem != null)
                IdName = lstSelected.SelectedItem.ToString();
            else if (lstLibrary.SelectedItem != null)
                IdName = lstLibrary.SelectedItem.ToString();
            if (String.IsNullOrEmpty(IdName))
            {
                language = CliUtils.fClientLang;
                string message = SysMsg.GetSystemMessage(language, "Srvtools", "InfoSecurity", "IsChooseGroup");
                MessageBox.Show(message);
            }
            else
            {
                String[] str = IdName.Split(new char[] { '(', ')' });
                Gid = str[1];
                if (Mid == "" || Mid == null)
                {
                    language = CliUtils.fClientLang;
                    string message = SysMsg.GetSystemMessage(language, "Srvtools", "InfoSecurity", "IsChooseMenu");
                    MessageBox.Show(message);
                }
                else
                {
                    frmAccessControlForGroup fac = new frmAccessControlForGroup(Mid, menuName, Gid, IdName);
                    fac.ShowDialog();
                }
            }
        }
    }
}