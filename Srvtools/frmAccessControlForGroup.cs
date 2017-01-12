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
    public partial class frmAccessControlForGroup : Form
    {
        private SYS_LANGUAGE language;
        public frmAccessControlForGroup()
        {
            InitializeComponent();
        }

        private string menuID;
        private string menuName;
        private string groupID;
        private string groupName;
        public frmAccessControlForGroup(string mID,string mName, string gID, string gName)
        {
            InitializeComponent();
            menuID = mID;
            menuName = mName;
            groupID = gID;
            groupName = gName;
        }

        private void btnAddAll_Click(object sender, EventArgs e)
        {
            object[] param = new object[1];
            param[0] = menuID;
            //List<string> listControlName = new List<string>();
            //List<string> listDescription = new List<string>();
            //List<string> listType = new List<string>();
            ArrayList listControlName = new ArrayList();
            ArrayList listDescription = new ArrayList();
            ArrayList listType = new ArrayList();
            object[] myRet = CliUtils.CallMethod("GLModule", "GetMenu", param);
            if ((myRet != null) && (0 == (int)myRet[0]))
            {
                //listControlName = (List<string>)myRet[1];
                //listDescription = (List<string>)myRet[2];
                //listType = (List<string>)myRet[3];
                listControlName = (ArrayList)myRet[1];
                listDescription = (ArrayList)myRet[2];
                listType = (ArrayList)myRet[3];
            }
            int RowCount = infoDataGridView1.Rows.Count - 1;
            int y = 0;
            for (int i = 0; i < listControlName.Count; i++)
            {
                bool flag = false;
                for (int x = 0; x < infoDataGridView1.Rows.Count; x++)
                    if (infoDataGridView1.Rows[x].Cells["ControlName"].Value == null)
                        break;
                    else if (infoDataGridView1.Rows[x].Cells["ControlName"].Value.ToString() == (string)listControlName[i])
                    {
                        flag = true;
                        break;
                    }
                if (flag == false)
                {
                    infoDataGridView1.Rows.Add(1);
                    infoDataGridView1.Rows[RowCount].Cells["Group_ID"].Value = groupID;
                    infoDataGridView1.Rows[RowCount].Cells["Menu_ID"].Value = menuID;
                    infoDataGridView1.Rows[RowCount].Cells["ControlName"].Value = listControlName[i];
                    infoDataGridView1.Rows[RowCount].Cells["Description"].Value = listDescription[i];
                    infoDataGridView1.Rows[RowCount].Cells["Type"].Value = listType[y].ToString();
                    infoDataGridView1.Rows[RowCount].Cells["gridEnabled"].Value = "Y";
                    infoDataGridView1.Rows[RowCount].Cells["gridVisible"].Value = "Y";
                    infoDataGridView1.Rows[RowCount].Cells["AllowAdd"].Value = "Y";
                    infoDataGridView1.Rows[RowCount].Cells["AllowUpdate"].Value = "Y";
                    infoDataGridView1.Rows[RowCount].Cells["AllowDelete"].Value = "Y";
                    infoDataGridView1.Rows[RowCount].Cells["AllowPrint"].Value = "Y";
                    RowCount++;
                }
                y++;
            }
        }

        private void frmAccessControl_Load(object sender, EventArgs e)
        {
            this.labelGroupID.Text = groupID;
            this.labelGroupName.Text = groupName;
            this.lblMenuID.Text = menuID;
            this.lblMenuName.Text = menuName;
            
            object[] param = new object[1];
            param[0] = groupID + ";" + menuID;
            DataSet ds = new DataSet();
            object[] myRet = CliUtils.CallMethod("GLModule", "GetGroup", param);
            if ((myRet != null) && (0 == (int)myRet[0]))
            {
                ds = (DataSet)myRet[1];
            }
            if (ds.Tables[0].Rows.Count > 0)
            {
                infoDataGridView1.Rows.Add(ds.Tables[0].Rows.Count);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    infoDataGridView1.Rows[i].Cells["Group_ID"].Value = ds.Tables[0].Rows[i]["GroupID"].ToString();
                    infoDataGridView1.Rows[i].Cells["Menu_ID"].Value = ds.Tables[0].Rows[i]["MenuID"].ToString();
                    infoDataGridView1.Rows[i].Cells["ControlName"].Value = ds.Tables[0].Rows[i]["ControlName"].ToString();
                    infoDataGridView1.Rows[i].Cells["Description"].Value = ds.Tables[0].Rows[i]["Description"].ToString();
                    infoDataGridView1.Rows[i].Cells["Type"].Value = ds.Tables[0].Rows[i]["Type"].ToString();
                    infoDataGridView1.Rows[i].Cells["gridEnabled"].Value = ds.Tables[0].Rows[i]["Enabled"].ToString();
                    infoDataGridView1.Rows[i].Cells["gridVisible"].Value = ds.Tables[0].Rows[i]["Visible"].ToString();
                    infoDataGridView1.Rows[i].Cells["AllowAdd"].Value = ds.Tables[0].Rows[i]["AllowAdd"].ToString();
                    infoDataGridView1.Rows[i].Cells["AllowUpdate"].Value = ds.Tables[0].Rows[i]["AllowUpdate"].ToString();
                    infoDataGridView1.Rows[i].Cells["AllowDelete"].Value = ds.Tables[0].Rows[i]["AllowDelete"].ToString();
                    infoDataGridView1.Rows[i].Cells["AllowPrint"].Value = ds.Tables[0].Rows[i]["AllowPrint"].ToString();
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            object[] param = new object[1];
            param[0] = groupID + ";" + menuID;
            DataSet ds = new DataSet();
            object[] myRet = CliUtils.CallMethod("GLModule", "GetGroup", param);
            //language = CliSysMegLag.GetClientLanguage();
            language = CliUtils.fClientLang;
            if ((myRet != null) && (0 == (int)myRet[0]))
            {
                ds = (DataSet)myRet[1];
            }
            for(int i = 0; i < infoDataGridView1.Rows.Count;i++)
            {
                bool flag = false;
                for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                {
                    if (infoDataGridView1.Rows[i].Cells["Group_ID"].Value == null
                        || (ds.Tables[0].Rows[j]["GroupID"].ToString() == infoDataGridView1.Rows[i].Cells["Group_ID"].Value.ToString()
                        && ds.Tables[0].Rows[j]["MenuID"].ToString() == infoDataGridView1.Rows[i].Cells["Menu_ID"].Value.ToString()
                        && ds.Tables[0].Rows[j]["ControlName"].ToString() == infoDataGridView1.Rows[i].Cells["ControlName"].Value.ToString()))
                    {
                        flag = true;
                        object[] UpdateGroup = new object[1];
                        if (infoDataGridView1.Rows[i].Cells["Group_ID"].Value != null
                            && (infoDataGridView1.Rows[i].Cells["Type"].Value.ToString() == "InfoBindingSource"
                                || infoDataGridView1.Rows[i].Cells["Type"].Value.ToString() == "WebDataSource"
                                || infoDataGridView1.Rows[i].Cells["Type"].Value.ToString() == "JQDataGrid"))
                        {
                            if (infoDataGridView1.Rows[i].Cells["Group_ID"].Value != null
                               && (infoDataGridView1.Rows[i].Cells["AllowAdd"].Value == null
                               || infoDataGridView1.Rows[i].Cells["AllowUpdate"].Value == null
                               || infoDataGridView1.Rows[i].Cells["AllowDelete"].Value == null
                               || infoDataGridView1.Rows[i].Cells["AllowPrint"].Value == null))
                            {
                                language = CliUtils.fClientLang;
                                string message = SysMsg.GetSystemMessage(language, "Srvtools", "InfoSecurity", "IsNull");
                                MessageBox.Show(string.Format(message, infoDataGridView1.Rows[i].Cells["ControlName"].Value));
                            }
                            else
                            {
                                if (infoDataGridView1.Rows[i].Cells["Group_ID"].Value != null 
                                    && (string.Compare(infoDataGridView1.Rows[i].Cells["Type"].Value.ToString(), "infobindingsource", true) == 0//IgnoreCase
                                    || string.Compare(infoDataGridView1.Rows[i].Cells["Type"].Value.ToString(), "webdatasource", true) == 0
                                    || string.Compare(infoDataGridView1.Rows[i].Cells["Type"].Value.ToString(), "servicedatasource", true) == 0
                                    || string.Compare(infoDataGridView1.Rows[i].Cells["Type"].Value.ToString(), "jqdatagrid", true) == 0))//IgnoreCase
                                {
                                    UpdateGroup[0] = infoDataGridView1.Rows[i].Cells["Group_ID"].Value + ";"
                                                  + infoDataGridView1.Rows[i].Cells["Menu_ID"].Value + ";"
                                                  + infoDataGridView1.Rows[i].Cells["ControlName"].Value + ";"
                                                  + infoDataGridView1.Rows[i].Cells["Type"].Value + ";" + " ; ;" 
                                                  + infoDataGridView1.Rows[i].Cells["AllowAdd"].Value + ";"
                                                  + infoDataGridView1.Rows[i].Cells["AllowUpdate"].Value + ";"
                                                  + infoDataGridView1.Rows[i].Cells["AllowDelete"].Value + ";"
                                                  + infoDataGridView1.Rows[i].Cells["AllowPrint"].Value;
                                }
                                else
                                {
                                    UpdateGroup[0] = infoDataGridView1.Rows[i].Cells["Group_ID"].Value + ";"
                                                  + infoDataGridView1.Rows[i].Cells["Menu_ID"].Value + ";"
                                                  + infoDataGridView1.Rows[i].Cells["ControlName"].Value + ";"
                                                  + infoDataGridView1.Rows[i].Cells["Type"].Value + ";"
                                                  + infoDataGridView1.Rows[i].Cells["gridEnabled"].Value + ";"
                                                  + infoDataGridView1.Rows[i].Cells["gridVisible"].Value + "; ; ; ; ";
                                }
                                myRet = CliUtils.CallMethod("GLModule", "UpdateGroup", UpdateGroup);
                            }
                        }
                        else
                        {
                            if (infoDataGridView1.Rows[i].Cells["Group_ID"].Value != null
                                   && (infoDataGridView1.Rows[i].Cells["gridEnabled"].Value == null
                                   || infoDataGridView1.Rows[i].Cells["gridVisible"].Value == null))
                            {
                                language = CliUtils.fClientLang;
                                string message = SysMsg.GetSystemMessage(language, "Srvtools", "InfoSecurity", "IsNull");
                                MessageBox.Show(string.Format(message, infoDataGridView1.Rows[i].Cells["ControlName"].Value));
                            }
                            else
                            {
                                if (infoDataGridView1.Rows[i].Cells["Group_ID"].Value != null
                                    && (string.Compare(infoDataGridView1.Rows[i].Cells["Type"].Value.ToString(), "infobindingsource", true) == 0//IgnoreCase
                                    || string.Compare(infoDataGridView1.Rows[i].Cells["Type"].Value.ToString(), "webdatasource", true) == 0
                                    || string.Compare(infoDataGridView1.Rows[i].Cells["Type"].Value.ToString(), "servicedatasource", true) == 0
                                    || string.Compare(infoDataGridView1.Rows[i].Cells["Type"].Value.ToString(), "jqdatagrid", true) == 0))//IgnoreCase
                                {
                                    UpdateGroup[0] = infoDataGridView1.Rows[i].Cells["Group_ID"].Value + ";"
                                                  + infoDataGridView1.Rows[i].Cells["Menu_ID"].Value + ";"
                                                  + infoDataGridView1.Rows[i].Cells["ControlName"].Value + ";"
                                                  + infoDataGridView1.Rows[i].Cells["Type"].Value + ";" + " ; ;"
                                                  + infoDataGridView1.Rows[i].Cells["AllowAdd"].Value + ";"
                                                  + infoDataGridView1.Rows[i].Cells["AllowUpdate"].Value + ";"
                                                  + infoDataGridView1.Rows[i].Cells["AllowDelete"].Value + ";"
                                                  + infoDataGridView1.Rows[i].Cells["AllowPrint"].Value;
                                }
                                else
                                {
                                    UpdateGroup[0] = infoDataGridView1.Rows[i].Cells["Group_ID"].Value + ";"
                                                  + infoDataGridView1.Rows[i].Cells["Menu_ID"].Value + ";"
                                                  + infoDataGridView1.Rows[i].Cells["ControlName"].Value + ";"
                                                  + infoDataGridView1.Rows[i].Cells["Type"].Value + ";"
                                                  + infoDataGridView1.Rows[i].Cells["gridEnabled"].Value + ";"
                                                  + infoDataGridView1.Rows[i].Cells["gridVisible"].Value + "; ; ; ; ";
                                }
                                myRet = CliUtils.CallMethod("GLModule", "UpdateGroup", UpdateGroup);
                            }
                        }
                    }
                }
                if (flag == false)
                {
                    object[] NewGroup = new object[1];
                    if (infoDataGridView1.Rows[i].Cells["Group_ID"].Value != null
                        && (infoDataGridView1.Rows[i].Cells["Type"].Value.ToString() == "InfoBindingSource"
                            || infoDataGridView1.Rows[i].Cells["Type"].Value.ToString() == "WebDataSource"
                            || infoDataGridView1.Rows[i].Cells["Type"].Value.ToString() == "JQDataGrid"))
                    {
                        if (infoDataGridView1.Rows[i].Cells["Group_ID"].Value != null
                           && (infoDataGridView1.Rows[i].Cells["AllowAdd"].Value == null
                           || infoDataGridView1.Rows[i].Cells["AllowUpdate"].Value == null
                           || infoDataGridView1.Rows[i].Cells["AllowDelete"].Value == null
                           || infoDataGridView1.Rows[i].Cells["AllowPrint"].Value == null))
                        {
                            language = CliUtils.fClientLang;
                            string message = SysMsg.GetSystemMessage(language, "Srvtools", "InfoSecurity", "IsNull");
                            MessageBox.Show(string.Format(message, infoDataGridView1.Rows[i].Cells["ControlName"].Value));
                        }
                        else
                        {
                            if (infoDataGridView1.Rows[i].Cells["Group_ID"].Value != null
                                && (string.Compare(infoDataGridView1.Rows[i].Cells["Type"].Value.ToString(), "infobindingsource", true) == 0//IgnoreCase
                                || string.Compare(infoDataGridView1.Rows[i].Cells["Type"].Value.ToString(), "webdatasource", true) == 0
                                || string.Compare(infoDataGridView1.Rows[i].Cells["Type"].Value.ToString(), "servicedatasource", true) == 0
                                || string.Compare(infoDataGridView1.Rows[i].Cells["Type"].Value.ToString(), "jqdatagrid", true) == 0))//IgnoreCase
                            {
                                NewGroup[0] = infoDataGridView1.Rows[i].Cells["Group_ID"].Value + ";"
                                              + infoDataGridView1.Rows[i].Cells["Menu_ID"].Value + ";"
                                              + infoDataGridView1.Rows[i].Cells["ControlName"].Value + ";"
                                              + infoDataGridView1.Rows[i].Cells["Type"].Value + ";" + " ; ;"
                                              + infoDataGridView1.Rows[i].Cells["AllowAdd"].Value + ";"
                                              + infoDataGridView1.Rows[i].Cells["AllowUpdate"].Value + ";"
                                              + infoDataGridView1.Rows[i].Cells["AllowDelete"].Value + ";"
                                              + infoDataGridView1.Rows[i].Cells["AllowPrint"].Value;
                            }
                            else
                            {
                                NewGroup[0] = infoDataGridView1.Rows[i].Cells["Group_ID"].Value + ";"
                                              + infoDataGridView1.Rows[i].Cells["Menu_ID"].Value + ";"
                                              + infoDataGridView1.Rows[i].Cells["ControlName"].Value + ";"
                                              + infoDataGridView1.Rows[i].Cells["Type"].Value + ";" + " ; ;"
                                              + infoDataGridView1.Rows[i].Cells["gridEnabled"].Value + ";"
                                              + infoDataGridView1.Rows[i].Cells["gridVisible"].Value + "; ; ; ; ";
                            }
                            myRet = CliUtils.CallMethod("GLModule", "InsertToGroup", NewGroup);
                        }
                    }
                    else
                    {
                        if (infoDataGridView1.Rows[i].Cells["Group_ID"].Value != null
                               && (infoDataGridView1.Rows[i].Cells["gridEnabled"].Value == null
                               || infoDataGridView1.Rows[i].Cells["gridVisible"].Value == null))
                        {
                            language = CliUtils.fClientLang;
                            string message = SysMsg.GetSystemMessage(language, "Srvtools", "InfoSecurity", "IsNull");
                            MessageBox.Show(string.Format(message, infoDataGridView1.Rows[i].Cells["ControlName"].Value));
                        }
                        else
                        {
                            if (infoDataGridView1.Rows[i].Cells["Group_ID"].Value != null
                                && (string.Compare(infoDataGridView1.Rows[i].Cells["Type"].Value.ToString(), "infobindingsource", true) == 0//IgnoreCase
                                || string.Compare(infoDataGridView1.Rows[i].Cells["Type"].Value.ToString(), "webdatasource", true) == 0
                                || string.Compare(infoDataGridView1.Rows[i].Cells["Type"].Value.ToString(), "servicedatasource", true) == 0
                                || string.Compare(infoDataGridView1.Rows[i].Cells["Type"].Value.ToString(), "jqdatagrid", true) == 0))//IgnoreCase
                            {
                                NewGroup[0] = infoDataGridView1.Rows[i].Cells["Group_ID"].Value + ";"
                                              + infoDataGridView1.Rows[i].Cells["Menu_ID"].Value + ";"
                                              + infoDataGridView1.Rows[i].Cells["ControlName"].Value + ";"
                                              + infoDataGridView1.Rows[i].Cells["Type"].Value + ";" + " ; ;"
                                              + infoDataGridView1.Rows[i].Cells["AllowAdd"].Value + ";"
                                              + infoDataGridView1.Rows[i].Cells["AllowUpdate"].Value + ";"
                                              + infoDataGridView1.Rows[i].Cells["AllowDelete"].Value + ";"
                                              + infoDataGridView1.Rows[i].Cells["AllowPrint"].Value;
                            }
                            else
                            {
                                NewGroup[0] = infoDataGridView1.Rows[i].Cells["Group_ID"].Value + ";"
                                              + infoDataGridView1.Rows[i].Cells["Menu_ID"].Value + ";"
                                              + infoDataGridView1.Rows[i].Cells["ControlName"].Value + ";"
                                              + infoDataGridView1.Rows[i].Cells["Type"].Value + ";"
                                              + infoDataGridView1.Rows[i].Cells["gridEnabled"].Value + ";"
                                              + infoDataGridView1.Rows[i].Cells["gridVisible"].Value + "; ; ; ; ";
                            }
                            myRet = CliUtils.CallMethod("GLModule", "InsertToGroup", NewGroup);
                        }
                    }
                }
            }
            language = CliUtils.fClientLang;
            string message1 = SysMsg.GetSystemMessage(language, "Srvtools", "InfoSecurity", "IsUpdate");
            MessageBox.Show(message1);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            object[] param = new object[1];
            param[0] = menuID;
            //List<string> listControlName = new List<string>();
            //List<string> listDescription = new List<string>();
            //List<string> listType = new List<string>();
            ArrayList listControlName = new ArrayList();
            ArrayList listDescription = new ArrayList();
            ArrayList listType = new ArrayList();
            object[] myRet = CliUtils.CallMethod("GLModule", "GetMenu", param);
            if ((myRet != null) && (0 == (int)myRet[0]))
            {
                //listControlName = (List<string>)myRet[1];
                //listDescription = (List<string>)myRet[2];
                //listType = (List<string>)myRet[3];
                listControlName = (ArrayList)myRet[1];
                listDescription = (ArrayList)myRet[2];
                listType = (ArrayList)myRet[3];
            }

            ArrayList ControlList = new ArrayList();
            for (int j = 0; j < infoDataGridView1.Rows.Count; j++)
            {
                if (infoDataGridView1.Rows[j].Cells["ControlName"].Value != null)
                    ControlList.Add(infoDataGridView1.Rows[j].Cells["ControlName"].Value.ToString()
                                    + ":" + infoDataGridView1.Rows[j].Cells["Description"].Value.ToString());
            }

            frmAccessControlAdd faca = new frmAccessControlAdd(listControlName, ControlList, listDescription);
            faca.ShowDialog();
            if (faca.DialogResult == DialogResult.OK)
            {
                int RowCount = infoDataGridView1.Rows.Count - 1;
                foreach (string strControlName in faca.SelColbList)
                {
                    string[] list = strControlName.Split(':');
                    bool flag = false;
                    for (int x = 0; x < infoDataGridView1.Rows.Count; x++)
                        if (infoDataGridView1.Rows[x].Cells["ControlName"].Value == null)
                            break;
                        else if (infoDataGridView1.Rows[x].Cells["ControlName"].Value.ToString() == list[0])
                        {
                            flag = true;
                            break;
                        }
                    if (flag == false)
                    {
                        int y = 0;
                        foreach (string str in listControlName)
                        {
                            if (str == list[0]) break;
                            y++;
                        }
                        infoDataGridView1.Rows.Add(1);
                        infoDataGridView1.Rows[RowCount].Cells["Group_ID"].Value = groupID;
                        infoDataGridView1.Rows[RowCount].Cells["Menu_ID"].Value = menuID;
                        infoDataGridView1.Rows[RowCount].Cells["ControlName"].Value = list[0];
                        for (int j = 1; j < list.Length; j++)
                        {
                            if(list[j] != null)
                                infoDataGridView1.Rows[RowCount].Cells["Description"].Value += list[j];
                            if(j != list.Length - 1)
                                infoDataGridView1.Rows[RowCount].Cells["Description"].Value += ":";
                        }
                        infoDataGridView1.Rows[RowCount].Cells["Type"].Value = listType[y].ToString();
                        infoDataGridView1.Rows[RowCount].Cells["gridEnabled"].Value = "Y";
                        infoDataGridView1.Rows[RowCount].Cells["gridVisible"].Value = "Y";
                        infoDataGridView1.Rows[RowCount].Cells["AllowAdd"].Value = "Y";
                        infoDataGridView1.Rows[RowCount].Cells["AllowUpdate"].Value = "Y";
                        infoDataGridView1.Rows[RowCount].Cells["AllowDelete"].Value = "Y";
                        infoDataGridView1.Rows[RowCount].Cells["AllowPrint"].Value = "Y";
                        RowCount++;
                    }
                }
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            int x = 0;
            for (int i = 0; i < infoDataGridView1.Rows.Count; i++)
                for (int j = 0; j < infoDataGridView1.Rows[i].Cells.Count; j++)
                    if (infoDataGridView1.Rows[i].Selected == true || infoDataGridView1.Rows[i].Cells[j].Selected == true)
                    {
                        x = i;
                        break;
                    }
            if (infoDataGridView1.Rows[x].Cells[1].Value != null)
            {
                object[] param = new object[1];
                param[0] = groupID + ";" + menuID + ";" + infoDataGridView1.Rows[x].Cells["ControlName"].Value;
                object[] myRet = CliUtils.CallMethod("GLModule", "DelGroup", param);
                infoDataGridView1.Rows.Remove(infoDataGridView1.Rows[x]);
            }
        }
    }
}