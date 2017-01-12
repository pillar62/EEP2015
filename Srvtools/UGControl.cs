using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;
using System.Security.Cryptography;

namespace Srvtools
{
    [ToolboxBitmap(typeof(UGControl), "Resources.UGControl.png")]
    public partial class UGControl : UserControl
    {
#if UseFL
        private System.Windows.Forms.DataGridViewTextBoxColumn ISROLE;
#endif
        public UGControl()
        {
            InitializeComponent();
            //SYS_LANGUAGE language = CliSysMegLag.GetClientLanguage();
            SYS_LANGUAGE language = CliUtils.fClientLang;
            string caption = SysMsg.GetSystemMessage(language, "Srvtools", "UGControl", "Caption_User");
            string[] arrcaption = caption.Split(';');

            this.dgViewUsers.Columns["uSERIDDataGridViewTextBoxColumn"].HeaderText = arrcaption[0];
            this.dgViewUsers.Columns["uSERNAMEDataGridViewTextBoxColumn"].HeaderText = arrcaption[1];
            this.dgViewUsers.Columns["aGENTDataGridViewTextBoxColumn"].HeaderText = arrcaption[2];
            this.dgViewUsers.Columns["cREATEDATEDataGridViewTextBoxColumn"].HeaderText = arrcaption[3];
            this.dgViewUsers.Columns["dESCRIPTIONDataGridViewTextBoxColumn"].HeaderText = arrcaption[4];
            this.dgViewUsers.Columns["eMAILDataGridViewTextBoxColumn"].HeaderText = arrcaption[5];
            this.dgViewUsers.Columns["MSAD"].HeaderText = arrcaption[6];

            language = CliUtils.fClientLang;
            caption = SysMsg.GetSystemMessage(language, "Srvtools", "UGControl", "Caption_Group");
            arrcaption = caption.Split(';');

            this.dgViewGroups.Columns["gROUPIDDataGridViewTextBoxColumn"].HeaderText = arrcaption[0];
            this.dgViewGroups.Columns["gROUPNAMEDataGridViewTextBoxColumn"].HeaderText = arrcaption[1];
            this.dgViewGroups.Columns["dESCRIPTIONDataGridViewTextBoxColumn1"].HeaderText = arrcaption[2];
            this.dgViewGroups.Columns["MSADGroup"].HeaderText = arrcaption[3];

#if UseFL
            this.ISROLE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            // 
            // ISROLE
            // 
            this.ISROLE.DataPropertyName = "ISROLE";
            this.ISROLE.HeaderText = arrcaption[4];
            this.ISROLE.Name = "ISROLE";
            this.dgViewGroups.Columns.Add(this.ISROLE);
            this.btnAgent.Visible = true;
#else
            this.btnAgent.Visible = false;
#endif

            language = CliUtils.fClientLang;
            caption = SysMsg.GetSystemMessage(language, "Srvtools", "UGControl", "Caption_Relations");
            //this.dgViewRelations.Columns["USERID"].HeaderText = caption;

            string temp = SysMsg.GetSystemMessage(language, "Srvtools", "UGControl", "Caption_Users");
            string[] labelCaption = temp.Split(';');
            labelChoosed.Text = labelCaption[1];
            labelUnchecked.Text = labelCaption[0];
        }

        private void txtPassword_Leave(object sender, EventArgs e)
        {
            isChanged = true;
            SYS_LANGUAGE language = CliUtils.fClientLang;
            if (txtPassword.Text.Length >= CliUtils.fPassWordMinSize && txtPassword.Text.Length <= CliUtils.fPassWordMaxSize)
            {
                if (CliUtils.fPassWordCharNum)
                {
                    int x = 0, y = 0;
                    for (int i = 0; i < txtPassword.Text.Length; i++)
                    {
                        if (!char.IsLetterOrDigit(txtPassword.Text, i))
                        {
                            string message = SysMsg.GetSystemMessage(language, "Srvtools", "UGControl", "PasswordCharCheck");
                            MessageBox.Show(message);
                            txtPassword.Focus();
                            return;
                        }
                        else if (char.IsLetter(txtPassword.Text, i))
                        {
                            x++;
                        }
                        else if (char.IsNumber(txtPassword.Text, i))
                        {
                            y++;
                        }
                    }
                    if (x == 0 || y == 0)
                    {
                        string message = SysMsg.GetSystemMessage(language, "Srvtools", "UGControl", "PasswordCharNum");
                        MessageBox.Show(message);
                        txtPassword.Focus();
                        return;
                    }
                }

                int j = bsUsers.Position;
                if (j >= 0)
                {
                    String sUserId = dgViewUsers.CurrentRow.Cells["uSERIDDataGridViewTextBoxColumn"].Value.ToString(); //infoDsUsers.RealDataSet.Tables[0].Rows[j][0].ToString();
                    String sUserName = dgViewUsers.CurrentRow.Cells["uSERIDDataGridViewTextBoxColumn"].Value.ToString(); //infoDsUsers.RealDataSet.Tables[0].Rows[j][1].ToString();
                    char[] p = new char[] { };
                    bool s = Encrypt.EncryptPassword(sUserId, txtPassword.Text, 10, ref p, false);
                    string pwd = new string(p);

                    for (int i = 0; i < infoDsUsers.RealDataSet.Tables[0].Rows.Count; i++)
                    {
                        if (infoDsUsers.RealDataSet.Tables[0].Rows[i]["USERID"].ToString() == sUserId)
                        {
                            infoDsUsers.RealDataSet.Tables[0].Rows[i]["PWD"] = pwd;
                            break;
                        }
                    }
                }
            }
            else
            {
                String message = String.Format(SysMsg.GetSystemMessage(language, "Srvtools", "UGControl", "PasswordLength"), CliUtils.fPassWordMinSize, CliUtils.fPassWordMaxSize);
                MessageBox.Show(message);
                txtPassword.Focus();
            }
        }

        private ArrayList SelectADObject(ArrayList list)
        {
            Form form = new Form();
            CheckedListBox listbox = new CheckedListBox();
            listbox.CheckOnClick = true;
            listbox.Dock = DockStyle.Top;
            listbox.Height = 400;
            for (int i = 0; i < list.Count; i++)
            {
                object obj = list[i];
                if (obj is ADUser)
                {
                    listbox.Items.Add((obj as ADUser).ID);
                }
                else if (obj is ADGroup)
                {
                    listbox.Items.Add((obj as ADGroup).ID);
                }
            }


            Button buttonSelectAll = new Button();
            buttonSelectAll.Location = new Point(100, 420);
            buttonSelectAll.Text = "Select all";
            buttonSelectAll.Click += delegate(object sender, EventArgs e)
            {
                for (int i = 0; i < listbox.Items.Count; i++)
                {
                    listbox.SetItemChecked(i, true);
                }
            };

            Button buttonUnselectAll = new Button();
            buttonUnselectAll.Location = new Point(200, 420);
            buttonUnselectAll.Text = "Unselect all";
            buttonUnselectAll.Click += delegate(object sender, EventArgs e)
            {
                for (int i = 0; i < listbox.Items.Count; i++)
                {
                    listbox.SetItemChecked(i, false);
                }
            };

            Button buttonOK = new Button();
            buttonOK.Location = new Point(300, 420);
            buttonOK.Text = "OK";
            buttonOK.DialogResult = DialogResult.OK;

            form.Controls.Add(listbox);
            form.Controls.Add(buttonSelectAll);
            form.Controls.Add(buttonUnselectAll);
            form.Controls.Add(buttonOK);

            form.Size = new Size(400, 500);
            form.AcceptButton = buttonOK;
            form.FormBorderStyle = FormBorderStyle.FixedSingle;
            form.MaximizeBox = false;
            form.MinimizeBox = false;
            form.ShowInTaskbar = false;
            form.StartPosition = FormStartPosition.CenterParent;
            form.Text = "Select Object";

            ArrayList listSelected = new ArrayList();
            if (form.ShowDialog(this) == DialogResult.OK)
            {
                foreach (int checkindex in listbox.CheckedIndices)
                {
                    listSelected.Add(list[checkindex]);
                }
            }
            return listSelected;
        }

        private ArrayList SortADObject(ArrayList list)
        {
            List<object> sortList = new List<object>(list.ToArray());
            sortList.Sort(new Comparison<object>(CompareADObject));
            return new ArrayList(sortList);
        }

        private int CompareADObject(object obj1, object obj2)
        {
            if (obj1 is ADUser && obj2 is ADUser)
            {
                return string.Compare((obj1 as ADUser).ID, (obj2 as ADUser).ID);
            }
            else if (obj1 is ADGroup && obj2 is ADGroup)
            {
                return string.Compare((obj1 as ADGroup).ID, (obj2 as ADGroup).ID);
            }
            return 0;
        }


        private void btnGetADUser_Click(object sender, EventArgs e)
        {
            Boolean ReplaceAll = false;
            ArrayList lstUser = new ArrayList();
            object[] myRet = CliUtils.CallMethod("GLModule", "GetADUsers", new object[] { });
            if ((null != myRet) && (0 == (int)myRet[0]))
            {
                lstUser = ((ArrayList)myRet[1]);

                lstUser = SelectADObject(SortADObject(lstUser));

                foreach (ADUser user in lstUser)
                {
                    DataRow druser = null;
                    DataRow[] drintables = infoDsUsers.RealDataSet.Tables[0].Select("USERID='" + user.ID + "'");
                    if (drintables.Length > 0)
                    {
                        if (ReplaceAll)
                        {
                            druser = drintables[0];
                        }
                        else
                        {
                            fmReplaceDialog aDialog = new fmReplaceDialog(string.Format("Replace {0} infomation by AD defination?", user.ID));
                            DialogResult aResult = aDialog.ShowDialog(this);
                            switch (aResult)
                            {
                                case DialogResult.OK:
                                    druser = drintables[0];
                                    break;
                                case DialogResult.Retry:
                                    ReplaceAll = true;
                                    druser = drintables[0];
                                    break;
                                case DialogResult.Cancel:
                                    continue;
                                //break;
                                default:
                                    continue;
                                //break;
                            }
                        }
                    }
                    else
                    {
                        druser = infoDsUsers.RealDataSet.Tables[0].NewRow();
                        druser["USERID"] = user.ID;
                        druser["AUTOLOGIN"] = "";
                        infoDsUsers.RealDataSet.Tables[0].Rows.Add(druser);
                    }
                    druser["USERNAME"] = user.Name;
                    druser["DESCRIPTION"] = user.Description;
                    druser["EMAIL"] = user.Email;
                    druser["MSAD"] = "Y";
                }
                infoDsUsers.ApplyUpdates();
            }
        }

        private void btnGetADGroup_Click(object sender, EventArgs e)
        {
            Boolean ReplaceAll = false;
            ArrayList lstGroup = new ArrayList();
            object[] myRet = CliUtils.CallMethod("GLModule", "GetADUserForGroup", new object[] { });
            if ((null != myRet) && (0 == (int)myRet[0]))
            {
                lstGroup = ((ArrayList)myRet[1]);

                lstGroup = SelectADObject(SortADObject(lstGroup));

                foreach (ADGroup group in lstGroup)
                {
                    DataRow drgroup = null;
                    DataRow[] drintables = infoDsGroups.RealDataSet.Tables[0].Select("GROUPNAME='" + group.ID + "'");
                    if (drintables.Length > 0)
                    {
                        if (ReplaceAll)
                        {
                            drgroup = drintables[0];
                        }
                        else
                        {
                            fmReplaceDialog aDialog = new fmReplaceDialog(string.Format("Replace {0} infomation by AD defination?", group.ID));
                            DialogResult aResult = aDialog.ShowDialog(this);
                            switch (aResult)
                            {
                                case DialogResult.OK:
                                    drgroup = drintables[0];
                                    break;
                                case DialogResult.Retry:
                                    ReplaceAll = true;
                                    drgroup = drintables[0];
                                    break;
                                case DialogResult.Cancel:
                                    continue;
                                //break;
                                default:
                                    continue;
                                //break;
                            }
                        }
                    }
                    else
                    {
                        drgroup = infoDsGroups.RealDataSet.Tables[0].NewRow();
                        drgroup["GROUPID"] = "ad" + GetGroupID().ToString("000");
                        drgroup["GROUPNAME"] = group.ID;
                        infoDsGroups.RealDataSet.Tables[0].Rows.Add(drgroup);
                    }
                    drgroup["DESCRIPTION"] = group.Description;
                    drgroup["MSAD"] = "Y";
                    foreach (string user in group.Users)
                    {
                        DataRow[] useringroup = infoDsGroups.RealDataSet.Tables[1].Select("GROUPID='" + drgroup["GROUPID"]
                            + "' and USERID='" + user + "'");
                        if (useringroup.Length == 0)
                        {
                            DataRow druser = infoDsGroups.RealDataSet.Tables[1].NewRow();
                            druser["GROUPID"] = drgroup["GROUPID"];
                            druser["USERID"] = user;
                            infoDsGroups.RealDataSet.Tables[1].Rows.Add(druser);
                        }
                    }
                }
                infoDsGroups.ApplyUpdates();
            }
        }

        private int GetGroupID()
        {
            int maxid = 0;
            int rowcount = this.infoDsGroups.RealDataSet.Tables[0].Rows.Count;
            for (int i = 0; i < rowcount; i++)
            {
                string strGroupID = this.infoDsGroups.RealDataSet.Tables[0].Rows[i]["GROUPID"].ToString();
                if (strGroupID.StartsWith("ad"))
                {
                    int id = 0;
                    try
                    {
                        id = int.Parse(strGroupID.Substring(2));
                    }
                    catch { }
                    maxid = Math.Max(id, maxid);
                }
            }
            return maxid + 1;
        }

        private void btnAM_Click(object sender, EventArgs e)
        {
            //int count = dgViewGroups.Rows.Count;
            //string strGroupID = "";
            //string strGroupName = "";
            //for (int i = 0; i < count; i++)
            //{
            //    if (dgViewGroups.Rows[i].Selected)
            //    {
            //        strGroupID = dgViewGroups.Rows[i].Cells["gROUPIDDataGridViewTextBoxColumn"].Value.ToString();
            //        strGroupName = dgViewGroups.Rows[i].Cells["gROUPNAMEDataGridViewTextBoxColumn"].Value.ToString();
            //        break;
            //    }
            //}
            //if (strGroupID != "")
            //{
            //    frmAccessMenus fam = new frmAccessMenus(this.infoDsGroups, strGroupID,strGroupName);
            //    fam.ShowDialog();
            //}
            string strGroupID = "";
            string strGroupName = "";
            int index = -1;
            if (dgViewGroups.SelectedRows.Count == 1)
            {
                index = dgViewGroups.SelectedRows[0].Index;
                strGroupID = dgViewGroups.Rows[index].Cells["gROUPIDDataGridViewTextBoxColumn"].Value.ToString();
                strGroupName = dgViewGroups.Rows[index].Cells["gROUPNAMEDataGridViewTextBoxColumn"].Value.ToString();
            }
            else if (dgViewGroups.SelectedCells.Count == 1)
            {
                index = dgViewGroups.SelectedCells[0].RowIndex;
                strGroupID = dgViewGroups.Rows[index].Cells["gROUPIDDataGridViewTextBoxColumn"].Value.ToString();
                strGroupName = dgViewGroups.Rows[index].Cells["gROUPNAMEDataGridViewTextBoxColumn"].Value.ToString();
            }
            if (strGroupID != "")
            {
                frmAccessMenus fam = new frmAccessMenus(this.infoDsGroups, strGroupID, strGroupName);
                fam.ShowDialog();
            }
        }

        private void btnAMUser_Click(object sender, EventArgs e)
        {
            //int count = dgViewUsers.Rows.Count;
            //string strUserID = "";
            //string strUserName = "";
            //for (int i = 0; i < count; i++)
            //{
            //    if (dgViewUsers.Rows[i].Selected)
            //    {
            //        strUserID = dgViewUsers.Rows[i].Cells["uSERIDDataGridViewTextBoxColumn"].Value.ToString();
            //        strUserName = dgViewUsers.Rows[i].Cells["uSERNAMEDataGridViewTextBoxColumn"].Value.ToString();
            //        break;
            //    }
            //}
            //if (strUserID != "")
            //{
            //    frmAccessMenusForUser fam = new frmAccessMenusForUser(this.infoDsUsers, strUserID, strUserName);
            //    fam.ShowDialog();
            //}
            string strUserID = "";
            string strUserName = "";
            int index = -1;
            if (dgViewUsers.SelectedRows.Count == 1)
            {
                index = dgViewUsers.SelectedRows[0].Index;
                strUserID = dgViewUsers.Rows[index].Cells["uSERIDDataGridViewTextBoxColumn"].Value.ToString();
                strUserName = dgViewUsers.Rows[index].Cells["uSERNAMEDataGridViewTextBoxColumn"].Value.ToString();
            }
            else if (dgViewUsers.SelectedCells.Count == 1)
            {
                index = dgViewUsers.SelectedCells[0].RowIndex;
                strUserID = dgViewUsers.Rows[index].Cells["uSERIDDataGridViewTextBoxColumn"].Value.ToString();
                strUserName = dgViewUsers.Rows[index].Cells["uSERNAMEDataGridViewTextBoxColumn"].Value.ToString();
            }
            if (strUserID != "")
            {
                frmAccessMenusForUser fam = new frmAccessMenusForUser(this.infoDsUsers, strUserID, strUserName);
                fam.ShowDialog();
            }
        }

        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.NavigatorUsers.SetState("Editing");
        }

        private String sGroupID = String.Empty;
        private void dgViewGroups_SelectionChanged(object sender, EventArgs e)
        {
            if (!DesignMode && !string.IsNullOrEmpty(CliUtils.fLoginDB))//modify by ccm 2008/05/15 EEPManager的frmSecurityMain设计界面不能打开
            {
                if (dgViewGroups.SelectedCells.Count > 0 && sGroupID != dgViewGroups.Rows[dgViewGroups.SelectedCells[0].RowIndex].Cells["gROUPIDDataGridViewTextBoxColumn"].Value.ToString())
                {
                    //for (int i = 0; i < dataSetGroupUsers.Tables["dataTableSelected"].Rows.Count; i++)
                    //{
                    //    dataSetGroupUsers.Tables["dataTableUnSelected"].Rows.Add(new object[] { dataSetGroupUsers.Tables["dataTableSelected"].Rows[i]["USERID"], dataSetGroupUsers.Tables["dataTableSelected"].Rows[i]["USERNAME"] });
                    //}
                    DataTable tableUnSelected = infoDsUsers.RealDataSet.Tables[0].Copy();
                    for (var i = 0; i < tableUnSelected.Columns.Count; i++)
                    {
                        tableUnSelected.Columns[i].ColumnName = tableUnSelected.Columns[i].ColumnName.ToUpper();
                    }
                    dataSetGroupUsers.Tables["dataTableUnSelected"].Clear();
                    dataSetGroupUsers.Tables["dataTableSelected"].Clear();

                    sGroupID = dgViewGroups.Rows[dgViewGroups.SelectedCells[0].RowIndex].Cells["gROUPIDDataGridViewTextBoxColumn"].Value.ToString();
                    object[] myRet = CliUtils.CallMethod("GLModule", "ListUsers", new object[] { sGroupID });
                    if ((null != myRet) && (0 == (int)myRet[0]))
                    {
                        //DataTable tableAll = infoDsUsers.RealDataSet.Tables[0];
                        DataTable tableSelected = ((DataSet)(myRet[1])).Tables[0];
                        for (var i = 0; i < tableSelected.Columns.Count; i++)
                        {
                            tableSelected.Columns[i].ColumnName = tableSelected.Columns[i].ColumnName.ToUpper();
                        }
                        //String strWhere = String.Empty;
                        for (int i = 0; i < tableSelected.Rows.Count; i++)
                        {
                            //strWhere = "USERID = '" + tableSelected.Rows[i]["USERID"] + "'";

                            //DataRow[] dr = dataSetGroupUsers.Tables["dataTableUnSelected"].Select(strWhere);
                            //if (dr.Length > 0)
                            //{
                            //    dataSetGroupUsers.Tables["dataTableUnSelected"].Rows.Remove(dr[0]);
                            //}
                            DataRow dr = tableUnSelected.Rows.Find(tableSelected.Rows[i]["USERID"]);
                            if (dr != null)
                            {
                                tableUnSelected.Rows.Remove(dr);
                            }

                            //dataSetGroupUsers.Tables["dataTableSelected"].Rows.Add(new object[] { tableSelected.Rows[i]["USERID"],tableSelected.Rows[i]["USERNAME"] });
                        }
                        dataSetGroupUsers.Tables["dataTableSelected"].Merge(tableSelected);
                    }
                    dataSetGroupUsers.Tables["dataTableUnSelected"].Merge(tableUnSelected, false, MissingSchemaAction.Ignore);
                }
                this.listBox1.SelectedIndex = -1;
                this.listBox2.SelectedIndex = -1;
            }
        }

        private void btnRightAll_Click(object sender, EventArgs e)
        {
            //for (int i = 0; i < dataSetGroupUsers.Tables["dataTableUnSelected"].Rows.Count; i++)
            //{
            //    dataSetGroupUsers.Tables["dataTableSelected"].Rows.Add(dataSetGroupUsers.Tables["dataTableUnSelected"].Rows[i].ItemArray);
            //}
            //dataSetGroupUsers.Tables["dataTableSelected"].Rows.Clear();
            //dataSetGroupUsers.Tables["dataTableSelected"].Merge(infoDsUsers.RealDataSet.Tables[0], false, MissingSchemaAction.Ignore);
            //dataSetGroupUsers.Tables["dataTableUnSelected"].Rows.Clear();
            //modified by lily 2010/12/01 如果有過濾資料，應該只移動過濾的部分，不應該全部移動
            for (int i = 0; i < this.bindingSourceUserUnSelected.Count; i++)
            {
                dataSetGroupUsers.Tables["dataTableSelected"].Rows.Add((this.bindingSourceUserUnSelected.List as DataView)[0].Row.ItemArray);
                dataSetGroupUsers.Tables["dataTableUnSelected"].Rows.Remove((this.bindingSourceUserUnSelected.List as DataView)[0].Row);
            }
            this.listBox1.SelectedIndex = -1;
            this.listBox2.SelectedIndex = -1;
        }

        private void btnRight_Click(object sender, EventArgs e)
        {
            // List<DataRow> rows = new List<DataRow>();
            DataTable table = dataSetGroupUsers.Tables["dataTableSelected"].Clone();
            for (int i = 0; i < listBox1.SelectedItems.Count; i++)
            {
                DataRow row = (listBox1.SelectedItems[i] as DataRowView).Row;
                table.Rows.Add(row.ItemArray);
                //dataSetGroupUsers.Tables["dataTableSelected"].Rows.Add(row.ItemArray);
                //rows.Add(row);
            }
            //foreach (DataRow row in rows)
            //{
            //    dataSetGroupUsers.Tables["dataTableUnSelected"].Rows.Remove(row);
            //}
            dataSetGroupUsers.Tables["dataTableSelected"].Merge(table);
            DataTable tableUnSelected = infoDsUsers.RealDataSet.Tables[0].Copy();
            DataTable tableSelected = dataSetGroupUsers.Tables["dataTableSelected"];
            for (int i = 0; i < tableSelected.Rows.Count; i++)
            {
                DataRow dr = tableUnSelected.Rows.Find(tableSelected.Rows[i]["USERID"]);
                if (dr != null)
                {
                    tableUnSelected.Rows.Remove(dr);
                }
            }
            dataSetGroupUsers.Tables["dataTableUnSelected"].Rows.Clear();
            dataSetGroupUsers.Tables["dataTableUnSelected"].Merge(tableUnSelected, false, MissingSchemaAction.Ignore);

            this.listBox1.SelectedIndex = -1;
            this.listBox2.SelectedIndex = -1;
        }

        private void btnLeft_Click(object sender, EventArgs e)
        {
            //List<DataRow> rows = new List<DataRow>();
            DataTable table = dataSetGroupUsers.Tables["dataTableUnSelected"].Clone();
            for (int i = 0; i < listBox2.SelectedItems.Count; i++)
            {
                DataRow row = (listBox2.SelectedItems[i] as DataRowView).Row;
                table.Rows.Add(row.ItemArray);
                // dataSetGroupUsers.Tables["dataTableUNSelected"].Rows.Add(row.ItemArray);
                //rows.Add(row);
            }
            //foreach (DataRow row in rows)
            //{
            //    dataSetGroupUsers.Tables["dataTableSelected"].Rows.Remove(row);
            //}
            dataSetGroupUsers.Tables["dataTableUnSelected"].Merge(table);
            DataTable tableSelected = infoDsUsers.RealDataSet.Tables[0].Copy();
            DataTable tableUnSelected = dataSetGroupUsers.Tables["dataTableUnSelected"];
            for (int i = 0; i < tableUnSelected.Rows.Count; i++)
            {
                DataRow dr = tableSelected.Rows.Find(tableUnSelected.Rows[i]["USERID"]);
                if (dr != null)
                {
                    tableSelected.Rows.Remove(dr);
                }
            }
            dataSetGroupUsers.Tables["dataTableSelected"].Rows.Clear();
            dataSetGroupUsers.Tables["dataTableSelected"].Merge(tableSelected, false, MissingSchemaAction.Ignore);
            this.listBox1.SelectedIndex = -1;
            this.listBox2.SelectedIndex = -1;
        }

        private void btnLeftAll_Click(object sender, EventArgs e)
        {
            //for (int i = 0; i < dataSetGroupUsers.Tables["dataTableSelected"].Rows.Count; i++)
            //{
            //    dataSetGroupUsers.Tables["dataTableUnSelected"].Rows.Add(dataSetGroupUsers.Tables["dataTableSelected"].Rows[i].ItemArray);
            //}
            //dataSetGroupUsers.Tables["dataTableUnSelected"].Rows.Clear();
            //dataSetGroupUsers.Tables["dataTableUnSelected"].Merge(infoDsUsers.RealDataSet.Tables[0], false, MissingSchemaAction.Ignore);
            //dataSetGroupUsers.Tables["dataTableSelected"].Rows.Clear();
            //modified by lily 2010/12/01 如果有過濾資料，應該只移動過濾的部分，不應該全部移動
            for (int i = 0; i < this.bindingSourceUserSelected.Count; i++)
            {
                dataSetGroupUsers.Tables["dataTableUnSelected"].Rows.Add((this.bindingSourceUserSelected.List as DataView)[0].Row.ItemArray);
                dataSetGroupUsers.Tables["dataTableSelected"].Rows.Remove((this.bindingSourceUserSelected.List as DataView)[0].Row);
            }
            this.listBox1.SelectedIndex = -1;
            this.listBox2.SelectedIndex = -1;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            object[] param = new object[2];
            param[0] = dgViewGroups.Rows[dgViewGroups.SelectedCells[0].RowIndex].Cells["gROUPIDDataGridViewTextBoxColumn"].Value;
            for (int i = 0; i < ((listBox2.DataSource as BindingSource).DataSource as DataSet).Tables[0].Rows.Count; i++)
            {
                DataRow row = ((listBox2.DataSource as BindingSource).DataSource as DataSet).Tables[0].Rows[i];
                param[1] += row["USERID"].ToString() + ";";
            }
            CliUtils.CallMethod("GLModule", "SetUserGroups", param);
        }

        private void NavigatorUsers_AfterItemClick(object sender, AfterItemClickEventArgs e)
        {
            if (e.ItemName == "Apply")
            {
                isNew = false;

                this.infoDsGroups.ApplyUpdates();
                //重贴的话bindingNavigatorRefreshItem2可能会改变，现在的位置是14
                this.NavigatorGroups.Items["bindingNavigatorRefreshItem2"].PerformClick();
            }
            else if (e.ItemName == "About")
            {
                isNew = false;
            }
            else if (e.ItemName == "Cancel")
            {
                isNew = false;
            }
            else if (e.ItemName == "OK")
            {
                //isNew = false;
            }
            else if (e.ItemName == "Add")
            {
                isNew = true;
            }
        }

        private void btnAgent_Click(object sender, EventArgs e)
        {
#if UseFL
            if (this.bsGroups.Current != null)
            {
                DataRowView row = this.bsGroups.Current as DataRowView;
                string isRole = row["ISROLE"].ToString().Trim();
                if (isRole != "Y")
                {
                    SYS_LANGUAGE language = CliUtils.fClientLang;
                    string message = SysMsg.GetSystemMessage(language, "Srvtools", "UGControl", "SecRoleAgent");
                    MessageBox.Show(message);
                }
                else
                {
                    frmAgent fAgent = new frmAgent();
                    fAgent.RoleId = row["GROUPID"].ToString();
                    fAgent.RoleName = row["GROUPNAME"].ToString();
                    fAgent.ShowDialog();
                }
            }
#endif
        }

        private ArrayList deleteGroupID = new ArrayList();
        private void NavigatorGroups_BeforeItemClick(object sender, BeforeItemClickEventArgs e)
        {
            if (e.ItemName == "Apply")
            {
                dgViewGroups.EndEdit();
            }
            else if (e.ItemName == "Delete")
            {
                if (!deleteGroupID.Contains(this.dgViewGroups.CurrentRow.Cells["gROUPIDDataGridViewTextBoxColumn"].Value.ToString()))
                {
                    isChanged = true;
                    deleteGroupID.Add(this.dgViewGroups.CurrentRow.Cells["gROUPIDDataGridViewTextBoxColumn"].Value.ToString());
                }
            }
        }

        private ArrayList deleteUserID = new ArrayList();
        private void NavigatorUsers_BeforeItemClick(object sender, BeforeItemClickEventArgs e)
        {
            if (e.ItemName == "Apply")
            {
                dgViewUsers.EndEdit();
            }
            else if (e.ItemName == "Delete")
            {
                if (!deleteUserID.Contains(this.dgViewUsers.CurrentRow.Cells["uSERIDDataGridViewTextBoxColumn"].Value.ToString()))
                {
                    isChanged = true;
                    deleteUserID.Add(this.dgViewUsers.CurrentRow.Cells["uSERIDDataGridViewTextBoxColumn"].Value.ToString());
                }
            }
        }

        private bool isChanged;
        private bool isNew;
        private ArrayList oldGroupID = new ArrayList();
        private ArrayList newGroupID = new ArrayList();
        private void dgViewGroups_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1 && e.ColumnIndex == 0)
            {
                isChanged = true;
                if (!isNew)
                {
                    if (oldGroupID.Contains(bsGroups.GetOldValue("GROUPID").ToString()))
                    {
                        int i = -1;
                        foreach (String str in oldGroupID)
                        {
                            i++;
                            if (str == bsGroups.GetOldValue("GROUPID").ToString()) break;
                        }
                        newGroupID[i] = dgViewGroups.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                    }
                    else if (bsGroups.GetOldValue("GROUPID").ToString() != dgViewGroups.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString())
                    {
                        oldGroupID.Add(bsGroups.GetOldValue("GROUPID").ToString());
                        newGroupID.Add(dgViewGroups.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
                    }
                }
            }
            if (isNew)
            {
                for (int i = 0; i < newGroup.Count; i++)
                {
                    int count = Convert.ToInt16(newGroup[i]);
                    if (dgViewGroups.Rows[count].Cells[1].Value != null)
                    {
                        DataRow[] drs = infoDsGroups.RealDataSet.Tables[0].Select("GROUPNAME='" + dgViewGroups.Rows[count].Cells[1].Value.ToString() + "'");
                        if (drs.Length > 1)
                        {
                            DialogResult dr = MessageBox.Show(dgViewGroups.Rows[count].Cells[1].Value.ToString() + " is existed. Do you want continue?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                            if (dr == DialogResult.No)
                            {
                                dgViewGroups.Rows[count].Cells[1].Value = "";
                                this.NavigatorGroups.SetState("Browsed");
                                newGroup.Clear();
                                break;
                            }
                        }
                    }
                }
            }
        }

        private ArrayList oldUserID = new ArrayList();
        private ArrayList newUserID = new ArrayList();

        private void dgViewUsers_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1 && e.ColumnIndex == 0)
            {
                isChanged = true;
                if (!isNew)
                {
                    if (oldUserID.Contains(bsUsers.GetOldValue("USERID").ToString()))
                    {
                        int i = -1;
                        foreach (String str in oldUserID)
                        {
                            i++;
                            if (str == bsUsers.GetOldValue("USERID").ToString()) break;
                        }
                        newUserID[i] = dgViewUsers.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                    }
                    else if (bsUsers.GetOldValue("USERID").ToString() != dgViewUsers.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString())
                    {
                        oldUserID.Add(bsUsers.GetOldValue("USERID").ToString());
                        newUserID.Add(dgViewUsers.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
                    }
                }
            }
        }

        private void infoDsGroups_AfterApplyUpdates(object sender, EventArgs e)
        {
            if (isChanged)
            {
                for (int i = 0; i < oldGroupID.Count; i++)
                {
                    if (oldGroupID[i] != newGroupID[i])
                    {
                        infoDsGroups.Execute("Update GROUPMENUS set GROUPID='" + newGroupID[i].ToString() + "' where GROUPID='" + oldGroupID[i].ToString() + "'");
                        infoDsGroups.Execute("Update GROUPMENUCONTROL set GROUPID='" + newGroupID[i].ToString() + "' where GROUPID='" + oldGroupID[i].ToString() + "'");
                    }
                }

                for (int i = 0; i < deleteGroupID.Count; i++)
                {
                    int count = infoDsGroups.RealDataSet.Tables[0].Select("GROUPID='" + deleteGroupID[i].ToString() + "'").Length;
                    if (count == 0)          //if (!compareGroupID(this.dgViewGroups.Rows, deleteGroupID[i].ToString()))
                    {
                        infoDsGroups.Execute("delete GROUPMENUS where GROUPID='" + deleteGroupID[i].ToString() + "'");
                        infoDsGroups.Execute("delete GROUPMENUCONTROL where GROUPID='" + deleteGroupID[i].ToString() + "'");
                    }
                }
            }

            isChanged = false;
            oldGroupID.Clear();
            newGroupID.Clear();
            deleteGroupID.Clear();
            newGroup.Clear();
        }

        private void infoDsUsers_AfterApplyUpdates(object sender, EventArgs e)
        {
            if (isChanged)
            {
                for (int i = 0; i < oldUserID.Count; i++)
                {
                    if (oldUserID[i] != newUserID[i])
                    {
                        infoDsUsers.Execute("Update USERMENUS set USERID='" + newUserID[i].ToString() + "' where USERID='" + oldUserID[i].ToString() + "'");
                        infoDsUsers.Execute("Update USERMENUCONTROL set USERID='" + newUserID[i].ToString() + "' where USERID='" + oldUserID[i].ToString() + "'");
                    }
                }

                for (int i = 0; i < deleteUserID.Count; i++)
                {
                    int count = infoDsUsers.RealDataSet.Tables[0].Select("USERID='" + deleteUserID[i].ToString() + "'").Length;
                    if (count == 0)          //if (!compareUserID(this.dgViewUsers.Rows, deleteUserID[i].ToString()))
                    {
                        infoDsUsers.Execute("delete USERMENUS where USERID='" + deleteUserID[i].ToString() + "'");
                        infoDsUsers.Execute("delete USERMENUCONTROL where USERID='" + deleteUserID[i].ToString() + "'");
                    }
                }
            }

            isChanged = false;
            isNew = false;
            oldUserID.Clear();
            newUserID.Clear();
            deleteUserID.Clear();
        }

        private void dgViewGroups_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (!deleteGroupID.Contains(e.Row.Cells["gROUPIDDataGridViewTextBoxColumn"].Value.ToString()))
            {
                isChanged = true;
                deleteGroupID.Add(e.Row.Cells["gROUPIDDataGridViewTextBoxColumn"].Value.ToString());
            }
        }

        private void dgViewUsers_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (!deleteUserID.Contains(e.Row.Cells["uSERIDDataGridViewTextBoxColumn"].Value.ToString()))
            {
                isChanged = true;
                deleteUserID.Add(e.Row.Cells["uSERIDDataGridViewTextBoxColumn"].Value.ToString());
            }
        }

        private void NavigatorGroups_AfterItemClick(object sender, AfterItemClickEventArgs e)
        {
            if (e.ItemName == "Apply")
            {
                isNew = false;
            }
            else if (e.ItemName == "About")
            {
                isNew = false;
                newGroup.Clear();
            }
            else if (e.ItemName == "Cancel")
            {
                isNew = false;
                newGroup.Clear();
            }
            else if (e.ItemName == "OK")
            {
                //isNew = false;
            }
            else if (e.ItemName == "Add")
            {
                isNew = true;
            }
        }

        private void bsUsers_AddingNew(object sender, AddingNewEventArgs e)
        {
            isNew = true;
        }

        ArrayList newGroup = new ArrayList();
        private void bsGroups_AddingNew(object sender, AddingNewEventArgs e)
        {
            isNew = true;
            newGroup.Add(dgViewGroups.Rows.Count - 1);
        }

        private void btnResetPWD_Click(object sender, EventArgs e)
        {
            String PWD = "";
            try
            {
                infoDsUsers.Execute("UPDATE USERS SET PWD='" + PWD + "', LASTDATE='' where USERID='" + (bsUsers.Current as DataRowView).Row["USERID"].ToString() + "'");
                MessageBox.Show("Reset succeed!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtAll_TextChanged(object sender, EventArgs e)
        {
            this.bindingSourceUserUnSelected.Filter = string.Format("USERID like '%{0}%' or USERNAME like '%{0}%'", txtAll.Text);
            listBox1.SelectedIndex = -1;
        }

        private void txtChoosed_TextChanged(object sender, EventArgs e)
        {
            this.bindingSourceUserSelected.Filter = string.Format("USERID like '%{0}%' or USERNAME like '%{0}%'", txtChoosed.Text);
            listBox2.SelectedIndex = -1;
        }

        public void SetLoadState()
        {
            this.NavigatorUsers.SetState("Browsed");
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if ((sender as TabControl).SelectedTab.Name == "tabPageGroups")
            //{
            //    dataSetGroupUsers.Tables["dataTableUnSelected"].Rows.Clear();
            //    //DataTable tableAll = infoDsUsers.RealDataSet.Tables[0];
            //    //for (int i = 0; i < tableAll.Rows.Count; i++)
            //    //    dataSetGroupUsers.Tables["dataTableUnSelected"].Rows.Add(new object[] { tableAll.Rows[i]["USERID"], tableAll.Rows[i]["USERNAME"] });
            //    dataSetGroupUsers.Tables["dataTableUnSelected"].Merge(infoDsUsers.RealDataSet.Tables[0], false, MissingSchemaAction.Ignore);
            //}
        }
    }
}
