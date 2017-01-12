using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using Srvtools;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Srvtools
{
    public partial class frmDGVGridRefVal : Form
    {
        private SYS_LANGUAGE language;

        public frmDGVGridRefVal()
        {
            InitializeComponent();
            language = CliUtils.fClientLang;
        }

        private InfoBindingSource _DataSource;
        public InfoBindingSource DataSource
        {
            get { return _DataSource; }
            set { _DataSource = value; }
        }

        private string _DataMember;
        public string DataMember
        {
            get { return _DataMember; }
            set { _DataMember = value; }
        }

        private string _ValueField;
        public string ValueField
        {
            get { return _ValueField; }
            set { _ValueField = value; }
        }

        private InfoRefValForGrid _GridCtrl;
        public InfoRefValForGrid GridCtrl
        {
            get { return _GridCtrl; }
            set { _GridCtrl = value; }
        }

        private InfoRefVal _RefVal;
        public InfoRefVal RefVal
        {
            get { return _RefVal; }
            set { _RefVal = value; }
        }

        private InfoRefvalBox _BoxCtrl;
        public InfoRefvalBox BoxCtrl
        {
            get { return _BoxCtrl; }
            set { _BoxCtrl = value; }
        }

        private string _InitValue;
        public string InitValue
        {
            get { return _InitValue; }
            set { _InitValue = value; }
        }

        private bool _AllowAddData;
        public bool AllowAddData
        {
            get { return _AllowAddData; }
            set { _AllowAddData = value; }
        }

        //private object tempDs;
        private string strFilter = string.Empty;
        private InfoDataSet dsAllData = new InfoDataSet();
        private void frmDGVGridRefVal_Load(object sender, EventArgs e)
        {
            language = CliUtils.fClientLang;
            String message = SysMsg.GetSystemMessage(language,
                     "Srvtools",
                     "InfoRefVal",
                     "ButtonName");
            string[] buttons = message.Split(';');
            this.btnOK.Text = buttons[0];
            this.btnCancel.Text = buttons[1];
            this.btnRefresh.Text = buttons[2];
            this.btnQuery.Text = buttons[3];
            this.btnAdd.Text = buttons[4];
            this.btnApply.Text = buttons[5];
            this.btnOK.Text += "(&O)";
            this.btnCancel.Text += "(&C)";
            this.btnRefresh.Text += "(&U)";
            this.btnQuery.Text += "(&Q)";
            this.btnAdd.Text += "(&A)";
            this.Font = this.RefVal.Font;
            this.Text = this.RefVal.Caption;
            this.dgView.DataSource = this.DataSource;
            if (this.DataSource.DataSource is InfoDataSet)
            {
                (this.DataSource.DataSource as InfoDataSet).NextPacket += new InfoDataSet.PacketEventHandler(DataSource_NextPacket);
            }

            if (this.RefVal.Columns.Count == 0)
            {
                DataSet ds = new DataSet();
                if (this.RefVal.DataSource != null && this.RefVal.DataSource is InfoBindingSource)
                {
                    ds = DBUtils.GetDataDictionary(this.RefVal.DataSource as InfoBindingSource, false);
                }
                if (ds != null && ds.Tables.Count > 0)
                {
                    int x = ds.Tables[0].Rows.Count;
                    for (int y = 0; y < x; y++)
                    {
                        int m = this.dgView.Columns.Count;
                        for (int n = 0; n < m; n++)
                        {
                            if (string.Compare(ds.Tables[0].Rows[y]["FIELD_NAME"].ToString(), this.dgView.Columns[n].DataPropertyName, true) == 0//IgnoreCase
                                && ds.Tables[0].Rows[y]["CAPTION"].ToString() != "")
                            {
                                this.dgView.Columns[n].HeaderText = ds.Tables[0].Rows[y]["CAPTION"].ToString();
                            }
                        }
                    }
                }
            }

            SetColumnStyles();
            AutoGridSize();
            if (this.AllowAddData)
            {
                this.dgView.AllowUserToAddRows = true;
                this.dgView.ReadOnly = false;
                this.btnAdd.Visible = true;
                this.btnApply.Visible = true;
            }
            // locate data
            int txtLenth = this.InitValue.Length;
            int rows = this.AllowAddData ? this.dgView.Rows.Count - 1 : this.dgView.Rows.Count;
            bool HasLocated = false;
            int ix = 0;
            while (!this.dgView.Columns[ix].Visible)
            { ix++; }
            for (int row = 0; row < rows; row++)
            {
                if (string.Compare(this.dgView[RefVal.ValueMember, row].Value.ToString(), this.InitValue, true) == 0)//IgnoreCase
                {
                    HasLocated = true;
                    //this.dgView[RefVal.ValueMember, row].Selected = true;
                    this.dgView[ix, row].Selected = true;
                    break;
                }
            }
            if (!HasLocated)
            {
                //merge a new datarow
                object[] obj = this.RefVal.CheckValid_And_ReturnDisplayValue(this.InitValue, false, false);
                if (obj.Length == 3 && obj[2] != null)
                {
                    DataTable table = (this.RefVal.GetDataSource() as InfoDataSet).RealDataSet.Tables[0];
                    DataRow dr = table.NewRow();
                    DataRow drsource = (DataRow)obj[2];
                    foreach (DataColumn column in table.Columns)
                    {
                        dr[column] = drsource[column.ColumnName];
                    }
                    if (table.PrimaryKey.Length == 0)
                    {
                        try
                        {
                            table.PrimaryKey = new DataColumn[] { table.Columns[this.RefVal.ValueMember] };
                        }
                        catch { }
                    }
                    table.Rows.Add(dr);
                    closepacketrecord = true;
                    this.dgView[ix, rows].Selected = true;
                    closepacketrecord = false;

                }
            }
            if (!HasLocated)
            {
                for (int row = 0; row < rows; row++)
                {
                    if (dgView[RefVal.ValueMember, row].Value.ToString().Length >= txtLenth)
                    {
                        if (string.Compare(dgView[RefVal.ValueMember, row].Value.ToString().Substring(0, txtLenth), this.InitValue, true) == 0)//IgnoreCase
                        {
                            //dgView[RefVal.ValueMember, row].Selected = true;
                            this.dgView[ix, row].Selected = true;
                            break;
                        }
                    }
                }
            }
            this.dgView.Select();
        }
        private bool closepacketrecord = false;
        void DataSource_NextPacket(object sender, PacketEventArgs e)
        {
            if (e.State == PacketEventArgs.PacketState.Before)
            {
                e.Cancel = closepacketrecord;
            }
        }

        void dgView_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            object obj = this.DataSource.Current;
            if (obj != null && obj is DataRowView)
            {
                DataRowView rowView = (DataRowView)obj;
                rowView.BeginEdit();
            }
        }

        void dgView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            object obj = this.DataSource.Current;
            if (obj != null && obj is DataRowView)
            {
                DataRowView rowView = (DataRowView)obj;
                rowView.EndEdit();
            }
        }

        private void AutoGridSize()
        {
            if (this.RefVal.AutoGridSize)
            {
                //width
                int i = this.dgView.Columns.Count;
                int width = 80;
                for (int j = 0; j < i; j++)
                {
                    width += this.dgView.Columns[j].Width;
                }
                this.panel2.Width = width;
                width += this.panel1.Width;
                if (((Form)this.RefVal.OwnerComp).Width > width)
                {
                    this.Width = width;
                }
                else
                {
                    this.Width = ((Form)this.RefVal.OwnerComp).Width;
                }
                //height
                int m = this.dgView.Rows.Count;
                int height = 150;
                if (m == 0)
                {
                    height += 20;
                }
                for (int n = 0; n < m; n++)
                {
                    height += this.dgView.Rows[n].Height;
                }
                if (((Form)this.RefVal.OwnerComp).Height > height)
                {
                    this.Height = height;
                }
                else
                {
                    this.Height = ((Form)this.RefVal.OwnerComp).Height;
                }
            }
            else
            {
                this.Size = this.RefVal.FormSize;
            }
            //mdi centerparent
            //if (((Form)this.RefVal.OwnerComp).Parent is MdiClient)
            //{
            //    Point main = ((Form)this.RefVal.OwnerComp).ParentForm.Location;
            //    Point mdi = (((Form)this.RefVal.OwnerComp).Parent as MdiClient).Location;
            //    Point form = ((Form)this.RefVal.OwnerComp).Location;
            //    int vshifht = (((Form)this.RefVal.OwnerComp).Width - this.Width) / 2;
            //    int hshift = (((Form)this.RefVal.OwnerComp).Height - this.Height) / 2 + 50;
            //    this.Location = new Point(main.X + mdi.X + form.X + vshifht, main.Y + mdi.Y + form.Y + hshift);
            //}
            //else
            //{
            //    //this.StartPosition = FormStartPosition.CenterScreen;
            //    int vshifht = (Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2;
            //    int hshift = (Screen.PrimaryScreen.WorkingArea.Height - this.Height) / 2;
            //    this.Location = new Point(vshifht, hshift);
            //}

        }

        private void InternalDataError(object sender, DataGridViewDataErrorEventArgs e)
        {
        }

        private void SetColumnStyles()
        {
            //根据RefVal.Columns的顺序显示栏位
            int i = this.RefVal.Columns.Count;
            if (i > 0)
            {
                ArrayList AddColumnList = new ArrayList();
                int n = this.dgView.Columns.Count;
                for (int j = 0; j < i; j++)
                {
                    for (int m = 0; m < n; m++)
                    {
                        if (string.Compare(this.RefVal.Columns[j].Column, this.dgView.Columns[m].Name, true) == 0)//IgnoreCase
                        {
                            this.dgView.Columns[this.RefVal.Columns[j].Column].HeaderText = this.RefVal.Columns[j].HeaderText;
                            this.dgView.Columns[this.RefVal.Columns[j].Column].DefaultCellStyle = this.RefVal.Columns[j].DefaultCellStyle;
                            this.dgView.Columns[this.RefVal.Columns[j].Column].Width = this.RefVal.Columns[j].Width;
                            this.dgView.Columns[this.RefVal.Columns[j].Column].Visible = this.RefVal.Columns[j].Visible;
                            AddColumnList.Add(this.dgView.Columns[m]);
                            break;
                        }
                    }
                }
                for (int y = 0; y < n; y++)
                {
                    this.dgView.Columns.RemoveAt(0);
                }
                for (int y = 0; y < AddColumnList.Count; y++)
                {
                    ((DataGridViewColumn)AddColumnList[y]).DisplayIndex = y;
                    this.dgView.Columns.Insert(y, (DataGridViewColumn)AddColumnList[y]);

                }
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            doOk();
        }

        private bool bCloseEventArises = false;
        private void doOk()
        {
            if (dgView.CurrentRow == null)
            {
                this.Close();
                return;
            }

            dsAllData = (InfoDataSet)this.DataSource.GetDataSource();
            if (this.BoxCtrl == null && this.GridCtrl != null)
            {
                #region GridCtrl
                string strValue = this.ValueField;
                string strSelectedValue = string.Empty;

                int i = this.dgView.Columns.Count;
                int k = -1;
                for (int j = 0; j < i; j++)
                {
                    if (string.Compare(this.dgView.Columns[j].Name, strValue, true) == 0)//IgnoreCase
                    {
                        k = j;
                        break;
                    }
                }
                if (k != -1)
                    strSelectedValue = dgView[k, dgView.CurrentRow.Index].Value.ToString();
                if (strSelectedValue != string.Empty)
                    this.GridCtrl.InnerTextBox.Text = strSelectedValue;

                this.Close();
                if (!bCloseEventArises)
                {
                    this.RefVal.Close(new OnCloseEventArgs());
                    bCloseEventArises = true;
                }
                this.RefVal.Return(new OnReturnEventArgs((DataRowView)dgView.CurrentRow.DataBoundItem));
                //if (this.RefVal.SelectCommand == null || this.RefVal.SelectCommand == "")
                //{
                //    dsAllData.ClearWhere();
                //    bClearWhere = true;
                //}
                //else
                //{
                //    dsAllData.Execute(RefVal.SelectCommand, true);
                //    bClearWhere = true;
                //}
                #endregion
            }
            else if (this.GridCtrl == null && this.BoxCtrl != null)
            {
                #region BoxCtrl
                string strValue = this.ValueField;
                string strSelectedValue = string.Empty;

                int i = this.dgView.Columns.Count;
                int k = -1;
                for (int j = 0; j < i; j++)
                {
                    if (string.Compare(this.dgView.Columns[j].Name, strValue, true) == 0)//IgnoreCase
                    {
                        k = j;
                        break;
                    }
                }
                if (k != -1)
                    strSelectedValue = dgView[k, dgView.CurrentRow.Index].Value.ToString();
                if (strSelectedValue != string.Empty)
                {
                    if (this.BoxCtrl.DisableValueMember)
                    {
                        this.BoxCtrl.TextBoxLeaveText = strSelectedValue;
                    }
                    else
                    {
                        this.BoxCtrl.TextBoxText = strSelectedValue;
                        this.BoxCtrl.TextBoxLeaveText = strSelectedValue;
                    }
                }

                this.Close();
                if (!bCloseEventArises)
                {
                    this.BoxCtrl.RefVal.Close(new OnCloseEventArgs());
                    bCloseEventArises = true;
                }
                this.BoxCtrl.RefVal.Return(new OnReturnEventArgs((DataRowView)dgView.CurrentRow.DataBoundItem));

                //if (this.BoxCtrl.RefVal.SelectCommand == null || this.BoxCtrl.RefVal.SelectCommand == "")
                //{
                //    dsAllData.ClearWhere();
                //    bClearWhere = true;
                //}
                //else
                //{

                //    //modified by lily 2007/3/13 RefVal.SelectAlias-->null,for runtime loginDB is the right db.
                //    dsAllData.Execute(RefVal.SelectCommand, null, true);
                //    bClearWhere = true;
                //}
                #endregion
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            if (strFilter != string.Empty)
            {
                if (this.RefVal.SelectCommand == null || this.RefVal.SelectCommand == "")
                {
                    ((InfoDataSet)this.DataSource.GetDataSource()).SetWhere(strFilter);
                }
                else
                {
                    string strCommand = RefVal.SelectCommand;
                    if (strFilter != "")
                    {
                        strCommand = CliUtils.InsertWhere(strCommand, strFilter);
                    }
                    #region where string
                    /*int ipos = -1;

                    ipos = strCommand.IndexOf(" where ");
                    if (ipos != -1)
                    {
                        goto Label1;
                    }

                    ipos = strCommand.IndexOf("\r\nwhere ");
                    if (ipos != -1)
                    {
                        goto Label1;
                    }

                    ipos = strCommand.IndexOf(" where\r\n");
                    if (ipos != -1)
                    {
                        goto Label1;
                    }

                    ipos = strCommand.IndexOf("\r\nwhere\r\n");
                    if (ipos != -1)
                    {
                        goto Label1;
                    }

                Label1:
                    if (ipos != -1)
                        strCommand += " and " + strFilter;
                    else
                        strCommand += " where " + strFilter;*/
                    #endregion

                    dsAllData.Execute(strCommand, true);
                }
            }
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            frmRefValQuery frmRefValQuery = null;
            //int i = ((InfoDataSet)this.DataSource.GetDataSource()).RealDataSet.Tables[this.DataMember].Columns.Count;
            int i = dgView.Columns.Count;
            string[] colName = new string[i];
            String[] colCaption = new String[i];
            for (int j = 0; j < i; j++)
            {
                colName[j] = dgView.Columns[j].Name;
                colCaption[j] = dgView.Columns[j].HeaderText;
                //colName[j] = ((InfoDataSet)this.DataSource.GetDataSource()).RealDataSet.Tables[this.DataMember].Columns[j].ColumnName;
                //colCaption[j] = dgView.Columns[colName[j]].HeaderText;
            }
            frmRefValQuery = new frmRefValQuery(colName, colCaption, this.DataSource, strFilter, this.RefVal);
            frmRefValQuery.Font = this.Font;
            frmRefValQuery.ShowDialog();
        }

        private void frmDGVGridRefVal_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!bCloseEventArises)
            {
                this.RefVal.Close(new OnCloseEventArgs());
                bCloseEventArises = true;
            }
            dsAllData = (InfoDataSet)this.DataSource.GetDataSource();
            //if (!bClearWhere)
            //{
            if (this.RefVal.SelectCommand == null || this.RefVal.SelectCommand == "")
            {
                dsAllData.ClearWhere();
            }
            else
            {
                //modified by lily 2007/3/13 RefVal.SelectAlias-->null,for runtime loginDB is the right db.
                dsAllData.Execute(RefVal.SelectCommand, null, true);
            }
            //}
        }

        #region HotKey
        private StringBuilder hotkey = new StringBuilder();
        private void dgView_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (RefVal.AutoLocate && !this.AllowAddData && e.KeyChar != '\b' && e.KeyChar != '\t' && e.KeyChar != '\r' && (int)e.KeyChar != 27)
            {
                this.timer.Stop();
                hotkey.Append(e.KeyChar);
                this.Text = string.Format("({0})", hotkey);
                for (int i = 0; i < dgView.Rows.Count; i++)
                {
                    string strCell = dgView[this.ValueField, i].Value.ToString();

                    if (strCell.Length >= hotkey.Length)
                    {
                        if (string.Compare(strCell, 0, hotkey.ToString(), 0, hotkey.Length, true) == 0)
                        {
                            this.dgView[this.ValueField, i].Selected = true;
                            break;
                        }
                    }
                }
                this.timer.Start();
            }
        }

        private void clearHotKeyBuffer()
        {
            if (RefVal.AutoLocate && !this.AllowAddData)
            {
                this.timer.Stop();
                hotkey = new StringBuilder();
                this.Text = string.Empty;
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            clearHotKeyBuffer();
        }
        #endregion

        private void dgView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                doOk();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                this.DialogResult = DialogResult.Cancel;
            }
        }

        private void dgView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!this.AllowAddData)
                doOk();
        }

        private void dgView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            this.DataSource.EndEdit();
            if ((this.RefVal.SelectAlias == null || this.RefVal.SelectAlias == "") &&
                (this.RefVal.SelectCommand == null || this.RefVal.SelectCommand == ""))
            {
                dsAllData.ApplyUpdates();
            }
            else
            {
                DataSet ds = ((InfoDataSet)this.DataSource.GetDataSource()).RealDataSet;
                string dm = this.DataSource.DataMember;
                if (ds.Tables.Count > 0 && dm != null && dm != "")
                {
                    DataTable table = ds.Tables[dm].GetChanges(DataRowState.Added);
                    if (table != null)
                    {

                        string tabName = CliUtils.GetTableName(this.RefVal.SelectCommand, true);
                        //s.Substring(s.LastIndexOf(' ') + 1);
                        //if (tabName.IndexOf('[') != -1 && tabName.IndexOf(']') != -1)
                        //{
                        //    tabName = tabName.Replace("[", "");
                        //    tabName = tabName.Replace("]", "");
                        //}
                        foreach (DataRow row in table.Rows)
                        {
                            string strFields = "";
                            string strValues = "";
                            foreach (DataColumn column in table.Columns)
                            {
                                if (row[column.ColumnName] != null && row[column.ColumnName].ToString() != "")
                                {
                                    strFields += column.ColumnName + ",";
                                    string type = column.DataType.ToString().ToLower();
                                    if (type == "system.uint" || type == "system.uint16" || type == "system.uint32"
                                    || type == "system.uint64" || type == "system.int" || type == "system.int16"
                                    || type == "system.int32" || type == "system.int64" || type == "system.single"
                                    || type == "system.double" || type == "system.decimal")
                                    {
                                        strValues += row[column.ColumnName].ToString() + ",";
                                    }
                                    else
                                    {
                                        strValues += "'" + row[column.ColumnName].ToString() + "',";
                                    }
                                }
                            }
                            if (strFields != "")
                            {
                                strFields = strFields.Substring(0, strFields.LastIndexOf(','));
                            }
                            if (strValues != "")
                            {
                                strValues = strValues.Substring(0, strValues.LastIndexOf(','));
                            }
                            if (tabName != "" && strFields != "" && strValues != "")
                            {
                                string sql = "insert into " + tabName + " (" + strFields + ") values (" + strValues + ")";
                                CliUtils.ExecuteSql("GLModule", "cmdRefValUse", sql, true, CliUtils.fCurrentProject);
                            }
                        }
                    }
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            this.DataSource.AddNew();
        }


    }
}