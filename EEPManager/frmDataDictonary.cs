using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Resources;
using System.IO;
using System.Collections;
using Srvtools;

namespace EEPManager
{
    public partial class frmDataDictonary : InfoForm
    {
        //private List<string> TabList = new List<string>();
        // private ArrayList TabList = new ArrayList();

        public frmDataDictonary()
        {
            InitializeComponent();
        }

        private void frmDataDictonary_Load(object sender, EventArgs e)
        {
            foreach (DataRow dr in infoDsTables.RealDataSet.Tables[0].Rows)
            {
                string TableName = dr["table_name"].ToString();
                FillListBox(TableName, lstTabName);
            }
            bHasLoaded = true;
            if (this.lstTabName.Items.Count > 0)
            {
                this.lstTabName.SetSelected(0, true);
            }
            if (this.lstTabName.SelectedValue != null)
            {
                string TableName = Remove(this.lstTabName.SelectedItem.ToString());
                string strFilter = "TABLE_NAME = '" + TableName + "'";
                this.infoDsDetails.SetWhere(strFilter);
            }
        }

        private string Remove(string Tab)
        {
            if (Tab.Contains("("))
                Tab = Tab.ToString().Substring(0, Tab.IndexOf('('));
            return Tab;
        }


        private void FillListBox(string tn, ListBox lv)
        {
            string TableCaption = "";
            for (int x = 0; x < this.infoDsDetails.RealDataSet.Tables[0].Rows.Count; x++)
                if (infoDsDetails.RealDataSet.Tables[0].Rows[x]["table_name"].ToString() == tn)
                {
                    if (infoDsDetails.RealDataSet.Tables[0].Rows[x]["field_name"].ToString() == "*")
                    {
                        TableCaption = infoDsDetails.RealDataSet.Tables[0].Rows[x]["caption"].ToString();
                        break;
                    }
                }
                else
                {
                    continue;
                }
            if (TableCaption != "")
                lv.Items.Add(tn + "(" + TableCaption + ")");
            else
                lv.Items.Add(tn);

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            List<string> TabList = new List<string>();
            foreach (object item in this.lstTabName.Items)
            {
                string tn = "";
                if (item.ToString().Contains("("))
                    tn = item.ToString().Substring(0, item.ToString().IndexOf('('));
                else
                    tn = item.ToString();

                TabList.Add(tn);
            }
            frmDDSelTab frmSelTab = new frmDDSelTab(TabList);
            if (frmSelTab.ShowDialog(this) == DialogResult.OK)
            {
                foreach (var item in frmSelTab.AddedTabList)
                {
                    var tabName = item.Value.ToString();
                    if (CliUtils.fLoginDBType != ClientType.ctODBC)
                    {
                        if (tabName.Contains("."))
                            tabName = tabName.Split('.')[1];
                        tabName = tabName.Replace("[", "").Replace("]", "");
                    }

                    FillListBox(tabName, lstTabName);
                    object[] param = new object[2];
                    param[0] = tabName;// item.Key.ToString(); ;
                    ArrayList colList = new ArrayList();
                    ArrayList dataTypeList = new ArrayList();
                    ArrayList lengthList = new ArrayList();
                    ArrayList allowDBNullList = new ArrayList();
                    ArrayList isKeyList = new ArrayList();
                    object[] myRet = CliUtils.CallMethod("GLModule", "GetDDColumnsSchema", param);
                    if ((null != myRet) && (0 == (int)myRet[0]))
                    {
                        DataTable dt = myRet[1] as DataTable;
                        myRet = CliUtils.CallMethod("GLModule", "GetDDColumns", param);
                        DataTable dtColumn = new DataTable();
                        if ((null != myRet) && (0 == (int)myRet[0]))
                            dtColumn = myRet[1] as DataTable;
                        foreach (DataRow drTemp in dt.Rows)
                        {
                            colList.Add(drTemp["ColumnName"]);
                            allowDBNullList.Add(drTemp["AllowDBNull"]);
                            isKeyList.Add(drTemp["IsKey"]);
                            if (CliUtils.fLoginDBType == ClientType.ctMsSql)
                            {
                                dataTypeList.Add(drTemp["DataTypeName"]);
                                if (dt.Columns["DataTypeName"] != null && (drTemp["DataTypeName"].ToString().ToLower() == "image"
                                                                        || drTemp["DataTypeName"].ToString().ToLower() == "text"
                                                                        || drTemp["DataTypeName"].ToString().ToLower() == "ntext"))
                                {
                                    foreach (DataRow drColumn in dtColumn.Rows)
                                        if (drColumn[0].ToString() == drTemp["ColumnName"].ToString())
                                            lengthList.Add(drColumn[1].ToString());
                                }
                                else
                                    lengthList.Add(drTemp["ColumnSize"]);
                            }
                            else if (CliUtils.fLoginDBType == ClientType.ctOracle)
                            {
                                dataTypeList.Add(drTemp["DataType"].ToString().Substring(drTemp["DataType"].ToString().IndexOf('.') + 1));
                                if (dt.Columns["DataType"] != null && (drTemp["DataType"].ToString().ToLower() == "system.decimal"
                                                                        || drTemp["DataType"].ToString().ToLower() == "text"
                                                                        || drTemp["DataType"].ToString().ToLower() == "ntext"))
                                    lengthList.Add(drTemp["NumericPrecision"]);
                                else
                                    lengthList.Add(drTemp["ColumnSize"]);
                            }
                            else if (CliUtils.fLoginDBType == ClientType.ctMySql)
                            {
                                foreach (DataRow drColumn in dtColumn.Rows)
                                    if (drColumn[0].ToString() == drTemp["ColumnName"].ToString())
                                    {
                                        String[] temp = drColumn[1].ToString().Split(new char[] { '(', ')', ',' });
                                        if (temp.Length > 1)
                                            lengthList.Add(temp[1]);
                                        else
                                            lengthList.Add(0);
                                    }
                            }
                            else if (CliUtils.fLoginDBType == ClientType.ctODBC)
                            {
                                dataTypeList.Add(drTemp["DataType"].ToString().Substring(drTemp["DataType"].ToString().IndexOf('.') + 1));
                                if (dt.Columns["DataType"] != null && (drTemp["DataType"].ToString().ToLower() == "system.decimal"
                                                                        || drTemp["DataType"].ToString().ToLower() == "text"
                                                                        || drTemp["DataType"].ToString().ToLower() == "ntext"))
                                    lengthList.Add(drTemp["NumericPrecision"]);
                                else
                                    lengthList.Add(drTemp["ColumnSize"]);
                            }
                            else
                            {
                                foreach (DataRow drColumn in dtColumn.Rows)
                                    if (drColumn[0].ToString() == drTemp["ColumnName"].ToString())
                                        lengthList.Add(drColumn[1].ToString());
                            }
                        }
                    }
                    DataRow dr = null;
                    dr = this.infoDsDetails.RealDataSet.Tables[0].NewRow();
                    dr["TABLE_NAME"] = tabName;
                    dr["FIELD_NAME"] = "*";
                    dr["SEQ"] = "0";
                    dr["IS_KEY"] = "N";
                    this.infoDsDetails.RealDataSet.Tables[0].Rows.Add(dr);

                    object[] param1 = new object[1];
                    param1[0] = tabName;
                    DataTable dtCaption = new DataTable();
                    object[] myRet1 = CliUtils.CallMethod("GLModule", "DataDRefresh", param1);
                    if ((null != myRet1) && (0 == (int)myRet1[0]))
                        dtCaption = myRet1[1] as DataTable;

                    for (int m = 0; m < colList.Count; m++)
                    {
                        dr = this.infoDsDetails.RealDataSet.Tables[0].NewRow();
                        dr["TABLE_NAME"] = tabName;
                        dr["FIELD_NAME"] = colList[m].ToString();
                        dr["SEQ"] = m + 1;
                        if (dataTypeList.Count > 0)
                            dr["FIELD_TYPE"] = dataTypeList[m].ToString();
                        if (Convert.ToBoolean(isKeyList[m]) == true)
                            dr["IS_KEY"] = "Y";
                        else
                            dr["IS_KEY"] = "N";
                        if (lengthList.Count > 0 && lengthList.Count >= m)
                            dr["FIELD_LENGTH"] = lengthList[m];
                        {
                            String caption = "";
                            object[] type = CliUtils.CallMethod("GLModule", "GetDataBaseType", new object[] { CliUtils.fLoginDB });
                            if (type[1].ToString() == "5")
                            {
                                object[] FieldMesseges = new object[1];
                                FieldMesseges[0] = dr["FIELD_NAME"].ToString();
                                object[] temp = CliUtils.CallMethod("GLModule", "GetFieldCaption", FieldMesseges);
                                if ((null != temp) && (0 == (int)temp[0]))
                                    dr["CAPTION"] = temp[1].ToString();
                            }
                            if (dr["CAPTION"] == null || dr["CAPTION"].ToString() == "")
                            {
                                foreach (DataRow drCaption in dtCaption.Rows)
                                    if (colList[m].ToString() == drCaption[0].ToString() && drCaption[0].ToString() != drCaption[1].ToString())
                                        caption = drCaption[1].ToString();

                                if (caption == "")
                                    dr["CAPTION"] = colList[m];
                                else
                                    dr["CAPTION"] = caption;
                            }
                        }
                        dr["EDITMASK"] = DBNull.Value;
                        dr["NEEDBOX"] = DBNull.Value;
                        dr["CANREPORT"] = DBNull.Value;
                        dr["EXT_MENUID"] = DBNull.Value;
                        dr["FIELD_SCALE"] = DBNull.Value;
                        dr["DD_NAME"] = DBNull.Value;
                        if (Convert.ToBoolean(allowDBNullList[m]) == true)
                            dr["CHECK_NULL"] = "N";
                        else
                            dr["CHECK_NULL"] = "Y";
                        dr["QUERYMODE"] = DBNull.Value;
                        dr["NEEDBOX"] = DBNull.Value;
                        this.infoDsDetails.RealDataSet.Tables[0].Rows.Add(dr);
                    }
                }
            }
            this.infoDsDetails.ApplyUpdates();
            if (this.lstTabName.Items.Count > 0)
            {
                this.lstTabName.SetSelected(this.lstTabName.Items.Count - 1, true);
            }
        }

        private bool bHasLoaded = false;
        private void lstTabName_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strCurState = this.Navigator.GetCurrentState();
            if (strCurState == "Editing" || strCurState == "Inserting" || strCurState == "Changing")
            {
                this.infoDsDetails.ApplyUpdates();
            }
            if (bHasLoaded)
            {
                int index = this.lstTabName.SelectedIndex;
                if (index == -1)
                {
                    if (this.lstTabName.Items.Count > 0)
                    {
                        this.lstTabName.SetSelected(0, true);
                    }
                    else
                    {
                        return;
                    }
                }
                string TableName = Remove(this.lstTabName.SelectedItem.ToString());
                string strFilter = "TABLE_NAME = '" + TableName + "'";
                this.infoDsDetails.SetWhere(strFilter);
            }
        }

        public string GetTableName()
        {
            return this.lstTabName.SelectedItem.ToString();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (this.lstTabName.Items.Count > 0)
            {
                this.infoDsDetails.ApplyUpdates();
                if (MessageBox.Show("Atempt to delete DataDictonary of table: " + this.lstTabName.SelectedItem.ToString(),
                    "Comfirm to Delete", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    object[] param = new object[1];
                    if (lstTabName.SelectedItem.ToString().Contains("("))
                        param[0] = lstTabName.SelectedItem.ToString().Substring(0, lstTabName.SelectedItem.ToString().IndexOf('('));
                    else
                        param[0] = lstTabName.SelectedItem.ToString();
                    CliUtils.CallMethod("GLModule", "DeleteDDColumns", param);

                    int index = this.lstTabName.SelectedIndex;
                    this.lstTabName.Items.RemoveAt(index);
                }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            if (this.lstTabName.Items.Count > 0)
            {
                this.infoDsDetails.ApplyUpdates();
                string tabName = "";
                if (lstTabName.SelectedItem.ToString().Contains("("))
                    tabName = lstTabName.SelectedItem.ToString().Substring(0, lstTabName.SelectedItem.ToString().IndexOf('('));
                else
                    tabName = lstTabName.SelectedItem.ToString();
                object[] param = new object[1];
                param[0] = tabName;
                ArrayList colList = new ArrayList();
                ArrayList dataTypeList = new ArrayList();
                ArrayList lengthList = new ArrayList();

                object[] myRet = CliUtils.CallMethod("GLModule", "GetDDColumnsSchema", param);
                if ((null != myRet) && (0 == (int)myRet[0]))
                {
                    DataTable dt = myRet[1] as DataTable;
                    myRet = CliUtils.CallMethod("GLModule", "GetDDColumns", param);
                    DataTable dtColumn = new DataTable();
                    if ((null != myRet) && (0 == (int)myRet[0]))
                        dtColumn = myRet[1] as DataTable;
                    foreach (DataRow drTemp in dt.Rows)
                    {
                        colList.Add(drTemp["ColumnName"]);
                        if (CliUtils.fLoginDBType == ClientType.ctMsSql)
                        {
                            dataTypeList.Add(drTemp["DataTypeName"]);
                            if (dt.Columns["DataTypeName"] != null && (drTemp["DataTypeName"].ToString().ToLower() == "image"
                                                                    || drTemp["DataTypeName"].ToString().ToLower() == "text"
                                                                    || drTemp["DataTypeName"].ToString().ToLower() == "ntext"))
                            {
                                foreach (DataRow drColumn in dtColumn.Rows)
                                    if (drColumn[0].ToString() == drTemp["ColumnName"].ToString())
                                        lengthList.Add(drColumn[1].ToString());
                            }
                            else
                                lengthList.Add(drTemp["ColumnSize"]);
                        }
                        else if (CliUtils.fLoginDBType == ClientType.ctOracle)
                        {
                            dataTypeList.Add(drTemp["DataType"].ToString().Substring(drTemp["DataType"].ToString().IndexOf('.') + 1));
                            if (dt.Columns["DataType"] != null && (drTemp["DataType"].ToString().ToLower() == "system.decimal"
                                                                    || drTemp["DataType"].ToString().ToLower() == "text"
                                                                    || drTemp["DataType"].ToString().ToLower() == "ntext"))
                                lengthList.Add(drTemp["NumericPrecision"]);
                            else
                                lengthList.Add(drTemp["ColumnSize"]);
                        }
                        else if (CliUtils.fLoginDBType == ClientType.ctMySql)
                        {
                            foreach (DataRow drColumn in dtColumn.Rows)
                                if (drColumn[0].ToString() == drTemp["ColumnName"].ToString())
                                {
                                    String[] temp = drColumn[1].ToString().Split(new char[] { '(', ')', ',' });
                                    if (temp.Length > 1)
                                        lengthList.Add(temp[1]);
                                    else
                                        lengthList.Add(0);
                                }
                        }
                        else
                        {
                            foreach (DataRow drColumn in dtColumn.Rows)
                                if (drColumn[0].ToString() == drTemp["ColumnName"].ToString())
                                    lengthList.Add(drColumn[1].ToString());
                        }
                    }
                }
                DataRow dr = this.infoDsDetails.RealDataSet.Tables[0].Rows[0];
                int count = this.infoDsDetails.RealDataSet.Tables[0].Rows.Count;

                object[] param1 = new object[1];
                param1[0] = tabName;
                DataTable dtCaption = new DataTable();
                object[] myRet1 = CliUtils.CallMethod("GLModule", "DataDRefresh", param1);
                if ((null != myRet1) && (0 == (int)myRet1[0]))
                    dtCaption = myRet1[1] as DataTable;
                for (int m = 0; m < colList.Count; m++)
                {
                    bool flag = false;
                    for (int j = 0; j < count; j++)
                    {
                        dr = this.infoDsDetails.RealDataSet.Tables[0].Rows[j];
                        if ((string)dr["FIELD_NAME"] == colList[m].ToString())
                            flag = true;
                    }
                    if (flag == true)
                        continue;
                    else
                    {
                        dr = this.infoDsDetails.RealDataSet.Tables[0].NewRow();
                        dr["TABLE_NAME"] = tabName;
                        dr["FIELD_NAME"] = colList[m];
                        dr["SEQ"] = m + 1;
                        dr["FIELD_TYPE"] = DBNull.Value;
                        dr["IS_KEY"] = "0";
                        if (lengthList.Count > 0 && lengthList.Count >= m)
                            dr["FIELD_LENGTH"] = lengthList[m];
                        {
                            String caption = "";
                            object[] type = CliUtils.CallMethod("GLModule", "GetDataBaseType", new object[] { CliUtils.fLoginDB });
                            if (type[1].ToString() == "5")
                            {
                                object[] FieldMesseges = new object[1];
                                FieldMesseges[0] = dr["FIELD_NAME"].ToString();
                                object[] temp = CliUtils.CallMethod("GLModule", "GetFieldCaption", FieldMesseges);
                                if ((null != temp) && (0 == (int)temp[0]))
                                    dr["CAPTION"] = temp[1].ToString();
                            }
                            if (dr["CAPTION"] == null || dr["CAPTION"].ToString() == "")
                            {
                                foreach (DataRow drCaption in dtCaption.Rows)
                                    if (colList[m].ToString() == drCaption[0].ToString() && drCaption[0].ToString() != drCaption[1].ToString())
                                        caption = drCaption[1].ToString();

                                if (caption == "")
                                    dr["CAPTION"] = colList[m];
                                else
                                    dr["CAPTION"] = caption;
                            }
                        }
                        dr["EDITMASK"] = DBNull.Value;
                        dr["NEEDBOX"] = DBNull.Value;
                        dr["CANREPORT"] = DBNull.Value;
                        dr["EXT_MENUID"] = DBNull.Value;
                        dr["FIELD_SCALE"] = DBNull.Value;
                        dr["DD_NAME"] = DBNull.Value;
                        this.infoDsDetails.RealDataSet.Tables[0].Rows.Add(dr);
                    }
                    this.infoDsDetails.ApplyUpdates();
                }

                dr = this.infoDsDetails.RealDataSet.Tables[0].Rows[0];
                for (int j = 0; j < count; j++)
                {
                    dr = this.infoDsDetails.RealDataSet.Tables[0].Rows[j];
                    if (dr["CAPTION"] == DBNull.Value && dr["FIELD_NAME"].ToString() != "*")
                    {
                        String caption = "";
                        object[] type = CliUtils.CallMethod("GLModule", "GetDataBaseType", new object[] { CliUtils.fLoginDB });
                        if (type[1].ToString() == "5")
                        {
                            object[] FieldMesseges = new object[1];
                            FieldMesseges[0] = dr["FIELD_NAME"].ToString();
                            object[] temp = CliUtils.CallMethod("GLModule", "GetFieldCaption", FieldMesseges);
                            if ((null != temp) && (0 == (int)temp[0]))
                                dr["CAPTION"] = temp[1].ToString();
                        }
                        if (dr["CAPTION"] == null || dr["CAPTION"].ToString() == "")
                        {
                            foreach (DataRow drCaption in dtCaption.Rows)
                                if (dr["FIELD_NAME"].ToString() == drCaption[0].ToString() && drCaption[0].ToString() != drCaption[1].ToString())
                                    caption = drCaption[1].ToString() == "" ? caption : drCaption[1].ToString();
                            if (caption != "")
                                dr["CAPTION"] = caption;
                        }
                        this.infoDsDetails.ApplyUpdates();
                    }
                }
                string strFilter = "";
                if (lstTabName.SelectedItem.ToString().Contains("("))
                    strFilter = lstTabName.SelectedItem.ToString().Substring(0, lstTabName.SelectedItem.ToString().IndexOf('('));
                else
                    strFilter = lstTabName.SelectedItem.ToString();
                this.infoDsDetails.SetWhere("TABLE_NAME = '" + strFilter + "'");
            }
        }

        private void Navigator_AfterItemClick(object sender, AfterItemClickEventArgs e)
        {
            infoDsDetails.SetWhere("1=1");
            ArrayList temp = new ArrayList();
            foreach (string str in lstTabName.Items)
            {
                string tableName = Remove(str);
                temp.Add(tableName);
            }
            for (int i = 0; i < dgvColDEF.CurrentRow.Cells.Count; i++)
                if (dgvColDEF.CurrentRow.Cells[i].Value.ToString() == "*")
                {
                    int x = lstTabName.SelectedIndex;
                    lstTabName.Items.Clear();
                    foreach (string str in temp)
                        FillListBox(str, lstTabName);
                    lstTabName.SelectedIndex = x;
                    break;
                }
        }
    }
}