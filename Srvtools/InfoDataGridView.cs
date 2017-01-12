using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Drawing.Design;
using System.Reflection;
using System.IO;
using System.Runtime.InteropServices;

namespace Srvtools
{
    [ToolboxBitmap(typeof(InfoDataGridView), "Resources.InfoDataGridView.png")]
    public class InfoDataGridView : DataGridView, IReadOnly
    {
        private const int WM_ShowWindow = 0x0018;

        private bool isInitialized = false;
        private SYS_LANGUAGE language;

        protected override void WndProc(ref Message m)
        {
            
            if (m.Msg == WM_ShowWindow)
            {
                isInitialized = true;
            }

            base.WndProc(ref m);
        }

        public InfoDataGridView()
            : base()
        {
            this.Paint += new PaintEventHandler(InfoDataGridView_Paint);
            this.CellEnter += new DataGridViewCellEventHandler(InfoDataGridView_CellEnter);
            this.CellLeave += new DataGridViewCellEventHandler(InfoDataGridView_CellLeave);
            this.TotalActive = true;
            this.TotalBackColor = SystemColors.Info;
            this.EnterEnable = true;
            this.TotalFont = new Font("SimSun", 9);
            this.TotalCaptionFont = new Font("SimSun", 9);
            _TotalColumns = new TotalColumnCollection(this, typeof(TotalColumn));
            this.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(InfoDataGridView_DataBindingComplete);
            this.CellValueChanged += new DataGridViewCellEventHandler(InfoDataGridView_CellValueChanged);
            this.CellBeginEdit += new DataGridViewCellCancelEventHandler(InfoDataGridView_CellBeginEdit);
            this.CellEndEdit += new DataGridViewCellEventHandler(InfoDataGridView_CellEndEdit);
            this.DataSourceChanged += new EventHandler(InfoDataGridView_DataSourceChanged);
            this.CurrentCellChanged += new EventHandler(InfoDataGridView_CurrentCellChanged);
            //this.GotFocus += new EventHandler(InfoDataGridView_GotFocus);
            //this.LostFocus += new EventHandler(InfoDataGridView_LostFocus);
            this.DataError += new DataGridViewDataErrorEventHandler(InfoDataGridView_DataError);
            this.ColumnAdded += new DataGridViewColumnEventHandler(InfoDataGridView_ColumnAdded);
            this.UserDeletingRow += new DataGridViewRowCancelEventHandler(InfoDataGridView_UserDeletingRow);
            //language = CliSysMegLag.GetClientLanguage();
            language = CliUtils.fClientLang;
        }

        void InfoDataGridView_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (this.SureDelete)
            {
                language = CliUtils.fClientLang;
                String message = SysMsg.GetSystemMessage(language,
                 "Srvtools",
                 "InfoDataGridView",
                 "SureDelete");
                if (MessageBox.Show(message, "", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    if ((this.DataSource as InfoBindingSource).AutoRecordLock)
                    {
                        if (!(this.DataSource as InfoBindingSource).AddLock("Deleting"))
                        {
                            e.Cancel = true;
                            return;
                        }
                    }
                }
                else
                {
                    e.Cancel = true;
                    return;
                }
            }
            else
            {
                if ((this.DataSource as InfoBindingSource).AutoRecordLock)
                {
                    if (!(this.DataSource as InfoBindingSource).AddLock("Deleting"))
                    {
                        e.Cancel = true;
                        return;
                    }
                }
            }
        }

        private bool _SureDelete;
        [Category("Infolight"),
        Description("Indicate whether user need to confirm to delete data")]
        public bool SureDelete
        {
            get
            {
                return _SureDelete;
            }
            set
            {
                _SureDelete = value;
            }
        }

        private bool _EnterRefValControl = false;
        [Browsable(false)]
        public bool EnterRefValControl
        {
            get
            {
                return _EnterRefValControl;
            }
            set
            {
                _EnterRefValControl = value;
            }
        }

        protected override void OnDataBindingComplete(DataGridViewBindingCompleteEventArgs e)
        {
            base.OnDataBindingComplete(e);
            //重算expression column
            foreach (DataGridViewColumn column in this.Columns)
            {
                if (column is InfoDataGridViewExpressionColumn)
                {
                    for (int i = 0; i < this.Rows.Count; i++)
                    {
                        if (!this.Rows[i].IsNewRow)
                        {
                            object obj = ((InfoDataGridViewExpressionColumn)column).GetExpression(i);
                            this.Rows[i].Cells[column.Index].Value = obj;
                        }
                    }
                }
            }
        }

        void InfoDataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >=0 && isInitialized && this.Columns[e.ColumnIndex].DataPropertyName != null && this.Columns[e.ColumnIndex].DataPropertyName != "")
            {
                this.Changed = true;
            }
            if (bHasFocused)
            {
                if (e.RowIndex != -1)
                {
                    if (this.Columns[e.ColumnIndex] is InfoDataGridViewCalendarColumn
                        && this.Columns[e.ColumnIndex].ValueType == typeof(string))
                    {
                        if (this[e.ColumnIndex, e.RowIndex].Value != null)
                        {
                            string strValue = this[e.ColumnIndex, e.RowIndex].Value.ToString();
                            if (strValue.IndexOf('-') > 0)
                            {
                                string[] DatePart = strValue.Split('-');
                                if (DatePart[1].Length == 1)
                                {
                                    DatePart[1] = "0" + DatePart[1];
                                }
                                if (DatePart[2].Length == 1)
                                {
                                    DatePart[2] = "0" + DatePart[2];
                                }
                                this[e.ColumnIndex, e.RowIndex].Value = DatePart[0] + DatePart[1] + DatePart[2];
                            }
                        }
                    }
                }
            }
        }

        protected override void OnRowsRemoved(DataGridViewRowsRemovedEventArgs e)
        {
            if (this.DataSource != null && this.DataSource is InfoBindingSource)
            {
                if (this.Rows.Count > 0)//在BindSource Reset中两个Count会不相同
                {
                    if (this.TotalActive)
                    {
                        for (int i = 0; i < this.Columns.Count; i++)
                        {
                            ChangeTotal(i);
                        }
                        this.Refresh();
                    }
                }
            }
            base.OnRowsRemoved(e);
        }

        private void InfoDataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            //refresh expression column
            foreach (DataGridViewColumn column in this.Columns)
            {
                if (column is InfoDataGridViewExpressionColumn
                    && ((InfoDataGridViewExpressionColumn)column).EffectColumnNames != null
                    && ((InfoDataGridViewExpressionColumn)column).EffectColumnNames != "")
                {
                    List<string> effectCols = ((InfoDataGridViewExpressionColumn)column).GetEffectColumnList();
                    foreach (string columnName in effectCols)
                    {
                        //if (!(this.Columns[e.ColumnIndex] is InfoDataGridViewRefValColumn))
                        //{
                            if (!string.IsNullOrEmpty(this.Columns[e.ColumnIndex].DataPropertyName)
                                && string.Compare(columnName, this.Columns[e.ColumnIndex].DataPropertyName, true) == 0)
                            {
                                // 2006/06/01 为解决GetExpression(e.RowIndex)无法取到最新数据问题
                                ((DataRowView)((InfoBindingSource)this.DataSource).Current).EndEdit();
                                // 2006/06/01
                                object obj = ((InfoDataGridViewExpressionColumn)column).GetExpression(e.RowIndex);
                                this.Rows[e.RowIndex].Cells[column.Index].Value = obj;
                                if (this.TotalActive)
                                {
                                    ChangeTotal(column.Index);
                                }
                            }
                        //}
                    }
                }
            }

            if (this.TotalActive)
            {
                ChangeTotal(e.ColumnIndex);
                this.Refresh();
            }
        }

        public void ChangeTotal(int columnindex)
        {
            if (this.TotalColumns.Count > 0)
            {
                foreach (TotalColumn col in this.TotalColumns)
                {
                    if (col.ColumnName == this.Columns[columnindex].Name)
                    {
                        string strSum = this.DoSum(columnindex, col.TotalMode);
                        OnTotalChanged(new TotalChangedEventArgs(columnindex, this.Columns[columnindex].Name, strSum));
                    }
                }
            }
        }

        void InfoDataGridView_DataSourceChanged(object sender, EventArgs e)
        {
            if (this.DataSource is InfoBindingSource)
            {
                (this.DataSource as InfoBindingSource).CanceledEdit += new EventHandler(BindingSource_CanceledEdit);
            }
        }

        void BindingSource_CanceledEdit(object sender, EventArgs e)
        {
            if (this.TotalActive)
            {
                Point cell = this.CurrentCellAddress;
                (this.DataSource as BindingSource).ResetBindings(false);
                if (cell.X != -1 && cell.Y != -1)
                {
                    this.CurrentCell = this[cell.X, cell.Y];
                }
                for (int i = 0; i < this.Columns.Count; i++)
                {
                    ChangeTotal(i);
                }
                this.Refresh();
            }
        }

        private void InfoDataGridView_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (this.CurrentCell.IsInEditMode)
            {
                this.Changed = true;
            }
        }

        private bool bDDHasGot = false;

        DataSet ds = new DataSet();
        private void InfoDataGridView_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            if (this.DataSource != null && (this.DataSource is InfoDataSet || this.DataSource is InfoBindingSource))
            {
                if (!bDDHasGot)
                {
                    if (this.DataSource is InfoDataSet)
                    {
                        ds = DBUtils.GetDataDictionary(this.DataSource as InfoDataSet, null, this.DesignMode);
                    }
                    else if(this.DataSource is InfoBindingSource && !string.IsNullOrEmpty((this.DataSource as InfoBindingSource).DataMember))
                    {
                        ds = DBUtils.GetDataDictionary(this.DataSource as InfoBindingSource, this.DesignMode);
                    }
                    bDDHasGot = true;
                }
                if (ds != null && ds.Tables.Count > 0)
                {
                    int i = ds.Tables[0].Rows.Count;
                    for (int j = 0; j < i; j++)
                    {
                        if (string.Compare(ds.Tables[0].Rows[j]["FIELD_NAME"].ToString(), e.Column.DataPropertyName, true) == 0)//IgnoreCase
                        {
                            string strCaption = ds.Tables[0].Rows[j]["CAPTION"].ToString();
                            if (e.Column.HeaderText == e.Column.DataPropertyName && strCaption != "")
                            {
                                e.Column.HeaderText = strCaption;
                                break;
                            }
                        }
                    }
                }
            }
        }

        public bool bHasFocused = false;
        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);
            if (!bHasFocused)
            {
                Type type = this.FindForm().GetType();
                FieldInfo[] fi = type.GetFields(BindingFlags.Instance
                                                    | BindingFlags.NonPublic
                                                    | BindingFlags.DeclaredOnly);
                for (int i = 0; i < fi.Length; i++)
                {
                    if (fi[i].GetValue(this.FindForm()) is InfoBindingSource)
                    {
                        InfoBindingSource bs = (InfoBindingSource)fi[i].GetValue(this.FindForm());
                        if (bs == this.DataSource && bs.DataSource is InfoBindingSource)
                        {
                            //DataRowView current = ((InfoBindingSource)bs.DataSource).Current as DataRowView;
                            //if (current != null && current.IsNew)
                            //{
                                ((InfoBindingSource)bs.DataSource).EndEdit();
                            //}
                        }
                    }
                }
                bHasFocused = true;
            }
        }

        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            bHasFocused = false;
        }

        protected override void OnSorted(EventArgs e)
        {
            base.OnSorted(e);
            InfoForm frm = this.FindForm() as InfoForm;
            if (frm != null)//EEPManager里的form不是继承自InfoForm
            {
                List<Control> listctrl = new List<Control>();
                frm.FindControl(typeof(InfoNavigator), listctrl);
                for (int i = 0; i < listctrl.Count; i++)
                {
                    if (listctrl[i] is InfoNavigator)
                    {
                        if ((listctrl[i] as InfoNavigator).ViewBindingSource == this.DataSource)
                        {
                            (this.DataSource as InfoBindingSource).DoDelay();
                        }
                    }
                }
            }
            if (this.DataSource is InfoBindingSource)
            {
                InfoBindingSource ibs = this.DataSource as InfoBindingSource;
                if (ibs.DataSource is InfoDataSet)
                {
                    InfoDataSet ids = ibs.DataSource as InfoDataSet;
                    if (ids.PacketRecords != -1)
                    {
                        string[] remotename = ids.RemoteName.Split('.');
                        string tablename = CliUtils.GetTableName(remotename[0], remotename[1], CliUtils.fCurrentProject);
                        string sql = CliUtils.GetSqlCommandText(remotename[0], remotename[1], CliUtils.fCurrentProject);
                        string[] quote = CliUtils.GetDataBaseQuote();
                        if (this.SortedColumn != null && !string.IsNullOrEmpty(this.SortedColumn.DataPropertyName))
                        {
                            string order = string.Empty;
                            if (this.SortOrder == SortOrder.Ascending)
                            {
                                order = "asc";
                            }
                            else if (this.SortOrder == SortOrder.Descending)
                            {
                                order = "desc";
                            }
                            ClientType ct = CliUtils.GetDataBaseType(CliUtils.fLoginDB);
                            String columnname = String.Empty;
                            switch (ct)
                            {
                                case ClientType.ctMsSql:
                                    columnname = CliUtils.GetTableNameForColumn(sql, this.SortedColumn.DataPropertyName);
                                    break;
                                case ClientType.ctOracle:
                                    columnname = this.SortedColumn.DataPropertyName;
                                    break;
                                case ClientType.ctOleDB:
                                    columnname = CliUtils.GetTableNameForColumn(sql, this.SortedColumn.DataPropertyName);
                                    break;
                                case ClientType.ctODBC:
                                    columnname = CliUtils.GetTableNameForColumn(sql, this.SortedColumn.DataPropertyName);
                                    break;
                                case ClientType.ctMySql:
                                    columnname = CliUtils.GetTableNameForColumn(sql, this.SortedColumn.DataPropertyName);
                                    break;
                                case ClientType.ctInformix:
                                    columnname = CliUtils.GetTableNameForColumn(sql, this.SortedColumn.DataPropertyName);
                                    break;
                            }
                            
                            ids.SetOrder(string.Format("{0} {1}", columnname, order));   
                        }
                    }
                }
            }
        }

        private void InfoDataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            if (this.DataSource is InfoBindingSource)
            {
                Type type = this.FindForm().GetType();
                FieldInfo[] fi = type.GetFields(BindingFlags.Instance
                                                    | BindingFlags.NonPublic
                                                    | BindingFlags.DeclaredOnly);
                for (int i = 0; i < fi.Length; i++)
                {
                    if (fi[i].GetValue(this.FindForm()) is InfoNavigator)
                    {
                        InfoNavigator Nav = (InfoNavigator)fi[i].GetValue(this.FindForm());
                        //modified by andy 2008/3/30  for detail+navigator+default inserting error.
                        //if (Nav.BindingSource.GetDataSource() == this.GetDataSource())
                        //remarked by lily 2007/4/27 for 不需要更改状态，如果是detail，不需更改，如果是master或单档，positionchanged等会更改
                        //if (Nav.BindingSource != null && Nav.BindingSource.GetDataSource() == this.GetDataSource()) 
                        //{
                        //    Nav.SetState("Browsed");
                        //}
                    }
                    else if (fi[i].GetValue(this.FindForm()) is DefaultValidate && (e.Exception is ConstraintException || e.Exception is NoNullAllowedException))
                    {
                        DefaultValidate defVal = (DefaultValidate)fi[i].GetValue(this.FindForm());
                        //if (this.GetDataSource() != null && this.GetDataSource() is InfoDataSet
                        //    && defVal.BindingSource.GetDataSource() == this.GetDataSource())
                        if(this.DataSource == defVal.BindingSource)
                        {
                            if (defVal.DuplicateCheck || defVal.CheckKeyFieldEmpty)
                            {
                                InfoDataSet infoDs = (InfoDataSet)this.GetDataSource();
                                string tableName = defVal.BindingSource.GetTableName();
                                DataTable table = infoDs.RealDataSet.Tables[tableName];
                                object obj = this.Rows[e.RowIndex].DataBoundItem;
                                if (obj != null && obj is DataRowView)
                                {
                                    DataRowView rowView = (DataRowView)obj;
                                    ArrayList keyValues = new ArrayList();
                                    foreach (DataColumn key in table.PrimaryKey)
                                    {
                                        if (defVal.CheckKeyFieldEmpty && e.Exception is NoNullAllowedException)
                                        {
                                            String message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "Srvtools", "DefaultValidate", "msg_DefaultValidateCheckNull");
                                            if (rowView[key.ColumnName] == null || rowView[key.ColumnName].ToString() == string.Empty)
                                            {
                                                for (int j = 0; j < this.Columns.Count; j++)
                                                {
                                                    if (string.Compare(this.Columns[j].DataPropertyName, key.ColumnName, true) == 0)
                                                    {
                                                        MessageBox.Show(string.Format(message, this.Columns[j].HeaderText));
                                                        return;
                                                    }
                                                }
                                                MessageBox.Show(string.Format(message,key.ColumnName));
                                                return;
                                            }
                                        }
                                        keyValues.Add(rowView[key.ColumnName]);
                                    }
                                    if (defVal.DuplicateCheck && e.Exception is ConstraintException)
                                    {
                                        DataRow row = table.Rows.Find(keyValues.ToArray());
                                        if (row != null)
                                        {
                                            language = CliUtils.fClientLang;
                                            String message = SysMsg.GetSystemMessage(language, "Srvtools", "DefaultValidate", "DeplicateWarning");
                                            MessageBox.Show(message);
                                        }
                                    }
                                }
                            }

                        }
                    }
                }
            }
        }

        private List<Control> AllCtrls = new List<Control>();
        private void GetAllCtrls(ControlCollection ctrls)
        {
            foreach (Control ctrl in ctrls)
            {
                AllCtrls.Add(ctrl);
                GetAllCtrls(ctrl.Controls);
            }
        }

        // Add By Chenjian 2006-01-09
        public bool Changed = false;
        public bool FirstEdit = false;
        private void InfoDataGridView_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            InfoBindingSource bindingSource = this.DataSource as InfoBindingSource;
            if (bindingSource != null)
            {
                if (!bindingSource.BeginEdit())
                {
                    e.Cancel = true;
                    return;
                }
                // 2006/07/26 解决detail在cancel后无法再次进入编辑状态
                AllCtrls.Clear();
                this.GetAllCtrls(this.FindForm().Controls);
                foreach (Control ctrl in AllCtrls)
                {
                    if (ctrl is InfoNavigator && ((InfoNavigator)ctrl).BindingSource != null && ((InfoNavigator)ctrl).BindingSource.GetDataSource() == this.GetDataSource())
                    {
                        if (((InfoNavigator)ctrl).GetCurrentState() != "Inserting")
                            ((InfoNavigator)ctrl).SetState("Editing");
                    }
                }
            }

            if (!FirstEdit)
            {
                //如果存在InfoDataGridViewExpressionColumn，那么就要求及时刷新
                int i = this.Columns.Count;
                for (int j = 0; j < i; j++)
                {
                    if (this.Columns[j].GetType() == typeof(InfoDataGridViewExpressionColumn))
                    {
                        bRefreshEnable = true;
                        break;
                    }
                }
                FirstEdit = true;
            }
        }
        private bool bRefreshEnable = false;

        private DataGridViewRow PrevRow = null;
        private void InfoDataGridView_CurrentCellChanged(object sender, EventArgs e)
        {
            if (PrevRow != this.CurrentRow)
            {
                if (Changed)
                {
                    InfoBindingSource bindingSource = this.DataSource as InfoBindingSource;
                    if (bindingSource != null /*andy为解決DataGridView一進來就進入EditMode的問題*/&& this.CurrentRow != null)
                    {
                        bindingSource.BeginChange();
                    }
                    Changed = false;
                }
                PrevRow = this.CurrentRow;
            }
        }
        // End Add

        #region EnterEnable
        private bool bEnterEdit = false;
        private void InfoDataGridView_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (this.EnterEnable)
            {
                bEnterEdit = true;
            }
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (this.EnterEnable)
            {
                Keys key = (keyData & Keys.KeyCode);
                if (key == Keys.Enter)
                {
                    this.bHasFocused = true;
                    if (this.bEnterEdit && this.CurrentCell != null && !this.CurrentCell.IsInEditMode && !this.CurrentCell.ReadOnly)
                    {
                        bEnterEdit = false;
                        return this.BeginEdit(true);
                    }
                    return this.ProcessRightKey(keyData);
                }
            }
            if ((keyData & Keys.KeyCode) == Keys.Tab)
            {
                if (this.CurrentCell != null)
                {
                    DataGridViewCell cell = this.CurrentCell;
                    do
                    {
                        DataGridViewCell celltemp = this.CurrentCell;
                        base.ProcessDialogKey(keyData);
                        if (this.CurrentCell == null)
                        {
                            return true;
                        }
                        if (this.CurrentCell == celltemp)
                        {
                            this.CurrentCell = cell;
                            return true;
                        }
                        else if (this.CurrentCell.ColumnIndex == cell.ColumnIndex)
                        {
                            if ((int)(keyData & Keys.Shift) == 0)
                            {
                                this.ProcessUpKey(Keys.Up);//reset current since all columns are readonly
                            }
                            else
                            {
                                this.ProcessDownKey(Keys.Down);//reset current since all columns are readonly
                            }
                            return true;
                        }
                    }
                    while (this.Columns[this.CurrentCell.ColumnIndex].ReadOnly);
                }
                return true;
            }
            else
            {
                return base.ProcessDialogKey(keyData);
            }
        }

        public new bool ProcessRightKey(Keys keyData)
        {
            try
            {
                Keys key = (keyData & Keys.KeyCode);
                if (keyData == (Keys.Control | Keys.Enter))
                {
                    if (base.CurrentCell != null && base.CurrentCell.OwningColumn.GetType() == typeof(InfoDataGridViewRefValColumn))
                    {
                        return false;
                    }
                }
                //modified by lily 2007/6/15 for <enter> to jump
                if (keyData == (Keys.Enter))//key == Keys.Enter)
                {
                    if (this.CurrentCell != null)
                    {
                        DataGridViewCell aCell = null;
                        InfoRefVal aRefVal = null;
                        if (Columns[CurrentCell.ColumnIndex].GetType().Equals(typeof(InfoDataGridViewRefValColumn)))
                        {
                            InfoDataGridViewRefValColumn RefColumn = (InfoDataGridViewRefValColumn)Columns[CurrentCell.ColumnIndex];
                            aCell = CurrentCell;
                            aRefVal = RefColumn.RefValue;
                        }
                        this.ProcessDialogKey(Keys.Tab);
                        if (aCell != null)
                        {
                            string sText = aRefVal.FLookupValue;
                            object[] obj = aRefVal.CheckValid_And_ReturnDisplayValue(ref sText, false, false);
                            if ((bool)obj[0] == false)
                            {
                                Focus();
                                CurrentCell = aCell;
                                return false;
                            }
                            else
                            {
                                return true;
                            }
                        }
                        else
                        {
                            return true;
                        }
                    }
                    //if ((base.CurrentCell.ColumnIndex == (base.ColumnCount - 1)) && (base.CurrentCell.RowIndex == (base.RowCount - 2)))
                    //{
                    //    base.CurrentCell = base.Rows[base.RowCount - 1].Cells[0];
                    //    return true;
                    //}
                    //if ((base.CurrentCell.ColumnIndex == (base.ColumnCount - 1)) && (base.CurrentCell.RowIndex + 1 != base.NewRowIndex))
                    //{
                    //    base.CurrentCell = base.Rows[base.CurrentCell.RowIndex + 1].Cells[0];
                    //    return true;
                    //}
                }
                return base.ProcessRightKey(keyData);
            }
            catch
            {
                return true;
            }
        }

        protected override bool ProcessDataGridViewKey(KeyEventArgs e)
        {
            if (this.EnterEnable)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (this.bEnterEdit && this.CurrentCell != null && !this.CurrentCell.IsInEditMode && !this.CurrentCell.ReadOnly)
                    {
                        bEnterEdit = false;
                        return this.BeginEdit(true);
                    }
                    return this.ProcessRightKey(e.KeyData);
                }
            }
            if (e.KeyCode == Keys.Tab)// jump over the readonly cell
            {
                return this.ProcessDialogKey(e.KeyData);
            }
            else
            {
                return base.ProcessDataGridViewKey(e);
            }
        }
        #endregion

        private void InfoDataGridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (bRefreshEnable)
            {
                foreach (DataGridViewColumn column in this.Columns)
                {
                    if (column is InfoDataGridViewExpressionColumn)
                    {
                        ((InfoDataGridViewExpressionColumn)column).Width += 1;
                        ((InfoDataGridViewExpressionColumn)column).Width -= 1;
                    }
                }
            }
        }

        public int GetCurrentRowIndex()
        {
            return this.CurrentCell.RowIndex;
        }

        public object GetDataSource()
        { 
            object obj = this.DataSource;
            while (!(obj is InfoDataSet))
            {
                if (obj is InfoBindingSource)
                {
                    obj = ((InfoBindingSource)obj).DataSource;
                }
                else
                {
                    return null;
                }
            }
            return obj;
        }

        public string GetDataMember()
        {
            object obj = this.DataSource;
            string strDataMember = this.DataMember;
            int i = 0;
            while (strDataMember == "" && i < 5)
            {
                if (obj is InfoBindingSource)
                {
                    strDataMember = ((InfoBindingSource)obj).DataMember.ToString();
                    obj = ((InfoBindingSource)obj).DataSource;
                }
                i++;
            }
            if (strDataMember.IndexOf('.') >= 0)
            {
                strDataMember = strDataMember.Substring(strDataMember.IndexOf('.') + 1);
            }
            return strDataMember;
        }

        public string GetDataTable()
        {
            string strTableName = this.GetDataMember();
            InfoDataSet infoDs = (InfoDataSet)this.GetDataSource();
            if (infoDs.RealDataSet.Relations != null)
            {
                int i = infoDs.RealDataSet.Relations.Count;
                for (int j = 0; j < i; j++)
                {
                    if (infoDs.RealDataSet.Relations[j].RelationName == strTableName)
                    {
                        strTableName = infoDs.RealDataSet.Relations[j].ChildTable.TableName;
                        break;
                    }
                }
            }
            return strTableName;
        }

        #region Properties
        private bool _TotalActive;
        [Category("Infolight"),
        Description("Indicate whether total is enabled or disabled")]
        public bool TotalActive
        {
            get
            {
                return _TotalActive;
            }
            set
            {
                _TotalActive = value;
                /*if (_TotalActive && this.Site != null && this.Site.DesignMode)
                {
                    int rowCont = this.DisplayedRowCount(true);
                    int vHeight = this.ColumnHeadersHeight + this.FirstDisplayedCell.OwningRow.Height * rowCont;
                }*/
            }
        }

        private string _TotalCaption;
        [Category("Infolight"),
        Description("Caption of total column")]
        public string TotalCaption
        {
            get
            {
                return _TotalCaption;
            }
            set
            {
                _TotalCaption = value;
            }
        }

        private Font _TotalCaptionFont;
        [Category("Infolight"),
        Description("The font used for caption in total column")]
        public Font TotalCaptionFont
        {
            get
            {
                return _TotalCaptionFont;
            }
            set
            {
                _TotalCaptionFont = value;
            }
        }

        private Font _TotalFont;
        [Category("Infolight"),
        Description("The font used for text in total column")]
        public Font TotalFont
        {
            get
            {
                return _TotalFont;
            }
            set
            {
                _TotalFont = value;
            }
        }

        private Color _TotalBackColor;
        [Category("Infolight"),
        Description("The Color of background of total column")]
        public Color TotalBackColor
        {
            get
            {
                return _TotalBackColor;
            }
            set
            {
                _TotalBackColor = value;
            }
        }

        /*private InfoPanel _TotalPanel;
        [Category("Infolight"),
        Description("total column's panel show")]
        public InfoPanel TotalPanel
        {
            get
            {
                return _TotalPanel;
            }
            set
            {
                _TotalPanel = value;
                if (this.Site != null && this.Site.DesignMode && _TotalPanel != null)
                {
                    _TotalPanel.Orientation = "asTop";
                    _TotalPanel.Width = this.Size.Width;
                    _TotalPanel.MinSize = 0;
                    _TotalPanel.MaxSize = 50;
                    _TotalPanel.AutoHide = true;
                    _TotalPanel.AutoHideButton = true;
                    _TotalPanel.CloseButton = false;
                    _TotalPanel.DesignHide = false;
                    _TotalPanel.ControlByUser = true;
                    _TotalPanel.BackColor = this.BackgroundColor;
                    _TotalPanel.Location = new Point(this.Location.X, this.Location.Y + this.Height);
                }
            }
        }*/

        private bool fEnterEnable;
        [Category("Infolight"),
        Description("Indicates whether user can  move to the next cell by press key of enter")]
        public bool EnterEnable
        {
            get
            {
                return fEnterEnable;
            }
            set
            {
                fEnterEnable = value;
            }
        }

        private TotalColumnCollection _TotalColumns;
        [Category("Infolight"),
        Description("Specifies the columns need to totalize")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public TotalColumnCollection TotalColumns
        {
            get { return _TotalColumns; }
            set { _TotalColumns = value; }
        }

        [Category("Infolight"),
        Description("The InfoBindingSource which the control is bound to")]
        public new object DataSource
        {
            get
            {
                return base.DataSource;
            }
            set
            {
                base.DataSource = value;
            }
        }
        
        [Category("Infolight"),
        Description("Indicate whether user can modify the data")]  
        public new bool ReadOnly
        {
            get
            {
                return base.ReadOnly;
            }
            set
            {
                base.ReadOnly = value;
                this.AllowUserToAddRows = !value;
                this.AllowUserToDeleteRows = !value;
            }
        }
        #endregion

        /*private void InfoDataGridView_Paint(object sender, PaintEventArgs e)
        {
            Graphics g;
            if (e == null)
            {
                g = Graphics.FromHwnd(this.Handle);
            }
            else
            {
                g = e.Graphics;
            }
            SolidBrush myBrush1 = new SolidBrush(SystemColors.Control);
            SolidBrush myBrush2 = new SolidBrush(this.TotalBackColor);
            Pen pen1 = new Pen(Brushes.White, 1);
            if (this.Rows.Count > 0 && this.TotalActive && this.Rows[this.Rows.Count - 1].Displayed)
            {
                int LocY = this.GetRowDisplayRectangle(this.Rows.Count - 1, true).Location.Y + this.Rows[this.Rows.Count - 1].Height;
                //draw caption
                g.FillRectangle(myBrush1, 2, LocY, this.RowHeadersWidth - 2, 23);
                //caption's top line
                g.DrawLine(pen1, new Point(2, LocY), new Point(this.RowHeadersWidth - 1, LocY));
                // caption's left line
                g.DrawLine(pen1, new Point(2, LocY), new Point(2, LocY + 23));

                //draw cells
                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Near;
                sf.LineAlignment = StringAlignment.Near;

                int cellLocX = this.RowHeadersWidth + 1;
                int i = this.Columns.Count;
                for (int j = 0; j < i; j++)
                {
                    if (this.Columns[j].Displayed)
                    {
                        g.FillRectangle(myBrush2, cellLocX, LocY, this.GetColumnDisplayRectangle(j, false).Width - 1, 23);
                        //Deleted DrawString place here...
                        #region DrawString
                        int x = this.TotalColumns.Count;
                        for (int y = 0; y < x; y++)
                        {
                            if (this.Columns[j].Name == this.TotalColumns[y].ColumnName
                                && this.TotalColumns[y].ShowTotal
                                && this.GetColumnDisplayRectangle(j, false).Width == this.Columns[j].Width
                                && !this.DesignMode)
                            {
                                int totalLocX = cellLocX;
                                int cellWidth = this.GetColumnDisplayRectangle(j, false).Width;
                                if (DoSum(j, this.TotalColumns[y].TotalMode) != "")
                                {
                                    try
                                    {
                                        if (this.Columns[j].DefaultCellStyle.Format != null)
                                        {
                                            if (this.TotalColumns[y].TotalMode == totalMode.average
                                               || this.TotalColumns[y].TotalMode == totalMode.sum
                                               || this.TotalColumns[y].TotalMode == totalMode.max
                                               || this.TotalColumns[y].TotalMode == totalMode.min)
                                            {
                                                double FmValue = Convert.ToDouble(DoSum(j, this.TotalColumns[y].TotalMode));
                                                string strFmValue = FmValue.ToString(this.Columns[j].DefaultCellStyle.Format, this.Columns[j].DefaultCellStyle.FormatProvider);
                                                switch (this.TotalColumns[y].TotalAlignment)
                                                {
                                                    case TotalColumn.TotalAlign.right:
                                                        totalLocX = totalLocX + cellWidth - (int)g.MeasureString(strFmValue, this.TotalFont, cellWidth).Width;
                                                        break;
                                                    case TotalColumn.TotalAlign.center:
                                                        totalLocX = totalLocX + cellWidth / 2 - (int)(g.MeasureString(strFmValue, this.TotalFont, cellWidth).Width / 2);
                                                        break;
                                                }
                                                g.DrawString(strFmValue, this.TotalFont, Brushes.Black, totalLocX, LocY + 6, sf);
                                            }
                                            else
                                            {
                                                switch (this.TotalColumns[y].TotalAlignment)
                                                {
                                                    case TotalColumn.TotalAlign.right:
                                                        totalLocX = totalLocX + cellWidth - (int)g.MeasureString(DoSum(j, this.TotalColumns[y].TotalMode), this.TotalFont, cellWidth).Width;
                                                        break;
                                                    case TotalColumn.TotalAlign.center:
                                                        totalLocX = totalLocX + cellWidth / 2 - (int)(g.MeasureString(DoSum(j, this.TotalColumns[y].TotalMode), this.TotalFont, cellWidth).Width / 2);
                                                        break;
                                                }
                                                g.DrawString(DoSum(j, this.TotalColumns[y].TotalMode), this.TotalFont, Brushes.Black, totalLocX, LocY + 6, sf);
                                            }
                                        }
                                        else
                                        {
                                            switch (this.TotalColumns[y].TotalAlignment)
                                            {
                                                case TotalColumn.TotalAlign.right:
                                                    totalLocX = totalLocX + cellWidth - (int)g.MeasureString(DoSum(j, this.TotalColumns[y].TotalMode), this.TotalFont, cellWidth).Width;
                                                    break;
                                                case TotalColumn.TotalAlign.center:
                                                    totalLocX = totalLocX + cellWidth / 2 - (int)(g.MeasureString(DoSum(j, this.TotalColumns[y].TotalMode), this.TotalFont, cellWidth).Width / 2);
                                                    break;
                                            }
                                            g.DrawString(DoSum(j, this.TotalColumns[y].TotalMode), this.TotalFont, Brushes.Black, totalLocX, LocY + 6, sf);
                                        }
                                    }
                                    catch
                                    {
                                        g.DrawString("", this.TotalFont, Brushes.Black, totalLocX, LocY + 6, sf);
                                    }
                                }
                            }
                        }
                        #endregion
                        cellLocX += this.GetColumnDisplayRectangle(j, false).Width;
                    }
                }
                // draw caption string
                g.DrawString(this.TotalCaption, this.TotalCaptionFont, Brushes.Black, 4, LocY + 6, sf);
            }
        }*/

        private void InfoDataGridView_Paint(object sender, PaintEventArgs e)
        {
            Graphics g;
            if (e == null)
            {
                g = Graphics.FromHwnd(this.Handle);
            }
            else
            {
                g = e.Graphics;
            }
            SolidBrush myBrush1 = new SolidBrush(SystemColors.Control);
            SolidBrush myBrush2 = new SolidBrush(this.TotalBackColor);
            Pen pen1 = new Pen(Brushes.White, 1);
            if (this.Rows.Count > 0 && this.TotalActive && this.Rows[this.Rows.Count - 1].Displayed)
            {
                int LocY = this.GetRowDisplayRectangle(this.Rows.Count - 1, true).Location.Y + this.Rows[this.Rows.Count - 1].Height;
                //draw caption
                g.FillRectangle(myBrush1, 2, LocY, this.RowHeadersWidth - 2, 23);
                //caption's top line
                g.DrawLine(pen1, new Point(2, LocY), new Point(this.RowHeadersWidth - 1, LocY));
                // caption's left line
                g.DrawLine(pen1, new Point(2, LocY), new Point(2, LocY + 23));

                //draw cells
                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Near;
                sf.LineAlignment = StringAlignment.Near;

                int cellLocX = this.RowHeadersWidth + 1;

                DataGridViewColumn displayColumn = this.Columns.GetFirstColumn(DataGridViewElementStates.Displayed);
                while (displayColumn != null)
                {
                    g.FillRectangle(myBrush2, cellLocX, LocY, this.GetColumnDisplayRectangle(displayColumn.Index, false).Width - 1, 23);
                    #region DrawString
                    int x = this.TotalColumns.Count;
                    for (int y = 0; y < x; y++)
                    {
                        if (displayColumn.Name == this.TotalColumns[y].ColumnName
                            && this.TotalColumns[y].ShowTotal
                            && this.GetColumnDisplayRectangle(displayColumn.Index, false).Width == displayColumn.Width
                            && !this.DesignMode)
                        {
                            int totalLocX = cellLocX;
                            int cellWidth = this.GetColumnDisplayRectangle(displayColumn.Index, false).Width;
                            if (DoSum(displayColumn.Index, this.TotalColumns[y].TotalMode) != "")
                            {
                                try
                                {
                                    if (displayColumn.DefaultCellStyle.Format != null)
                                    {
                                        if (this.TotalColumns[y].TotalMode == totalMode.average
                                           || this.TotalColumns[y].TotalMode == totalMode.sum
                                           || this.TotalColumns[y].TotalMode == totalMode.max
                                           || this.TotalColumns[y].TotalMode == totalMode.min)
                                        {
                                            double FmValue = Convert.ToDouble(DoSum(displayColumn.Index, this.TotalColumns[y].TotalMode));
                                            string strFmValue = FmValue.ToString(displayColumn.DefaultCellStyle.Format, displayColumn.DefaultCellStyle.FormatProvider);
                                            switch (this.TotalColumns[y].TotalAlignment)
                                            {
                                                case TotalColumn.TotalAlign.right:
                                                    totalLocX = totalLocX + cellWidth - (int)g.MeasureString(strFmValue, this.TotalFont, cellWidth).Width - 2;
                                                    break;
                                                case TotalColumn.TotalAlign.center:
                                                    totalLocX = totalLocX + cellWidth / 2 - (int)(g.MeasureString(strFmValue, this.TotalFont, cellWidth).Width / 2);
                                                    break;
                                            }
                                            g.DrawString(strFmValue, this.TotalFont, Brushes.Black, totalLocX, LocY + 4, sf);
                                        }
                                        else
                                        {
                                            switch (this.TotalColumns[y].TotalAlignment)
                                            {
                                                case TotalColumn.TotalAlign.right:
                                                    totalLocX = totalLocX + cellWidth - (int)g.MeasureString(DoSum(displayColumn.Index, this.TotalColumns[y].TotalMode), this.TotalFont, cellWidth).Width - 2;
                                                    break;
                                                case TotalColumn.TotalAlign.center:
                                                    totalLocX = totalLocX + cellWidth / 2 - (int)(g.MeasureString(DoSum(displayColumn.Index, this.TotalColumns[y].TotalMode), this.TotalFont, cellWidth).Width / 2);
                                                    break;
                                            }
                                            g.DrawString(DoSum(displayColumn.Index, this.TotalColumns[y].TotalMode), this.TotalFont, Brushes.Black, totalLocX, LocY + 4, sf);
                                        }
                                    }
                                    else
                                    {
                                        switch (this.TotalColumns[y].TotalAlignment)
                                        {
                                            case TotalColumn.TotalAlign.right:
                                                totalLocX = totalLocX + cellWidth - (int)g.MeasureString(DoSum(displayColumn.Index, this.TotalColumns[y].TotalMode), this.TotalFont, cellWidth).Width - 2;
                                                break;
                                            case TotalColumn.TotalAlign.center:
                                                totalLocX = totalLocX + cellWidth / 2 - (int)(g.MeasureString(DoSum(displayColumn.Index, this.TotalColumns[y].TotalMode), this.TotalFont, cellWidth).Width / 2);
                                                break;
                                        }
                                        g.DrawString(DoSum(displayColumn.Index, this.TotalColumns[y].TotalMode), this.TotalFont, Brushes.Black, totalLocX, LocY + 4, sf);
                                    }
                                }
                                catch
                                {
                                    g.DrawString("", this.TotalFont, Brushes.Black, totalLocX, LocY + 4, sf);
                                }
                            }
                        }
                    }
                    #endregion
                    cellLocX += this.GetColumnDisplayRectangle(displayColumn.Index, false).Width;
                    displayColumn = this.Columns.GetNextColumn(displayColumn, DataGridViewElementStates.Displayed, DataGridViewElementStates.None);
                }

                // draw caption string
                g.DrawString(this.TotalCaption, this.TotalCaptionFont, Brushes.Black, 4, LocY + 6, sf);
            }
        }

        /*private class User32
        {
            [StructLayout(LayoutKind.Sequential)]
            public struct RECT
            {
                public int left;
                public int top;
                public int right;
                public int bottom;
            }

            [DllImport("user32.dll")]
            public static extern IntPtr GetDesktopWindow();
            [DllImport("user32.dll")]
            public static extern IntPtr GetWindowDC(IntPtr hWnd);
            [DllImport("user32.dll")]
            public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDC);
            [DllImport("user32.dll")]
            public static extern IntPtr GetWindowRect(IntPtr hWnd, ref RECT rect);
            [DllImport("user32.dll")]
            public static extern void InvalidateRect(IntPtr hWnd, RECT rect, bool b);
        }*/

        public string DoSum(int colIndex, totalMode showMode)
        {
            decimal total = 0;
            decimal max = decimal.MinValue;
            decimal min = decimal.MaxValue;
            decimal average = new decimal();
            int i = -1;
            if (this.AllowUserToAddRows)
                i = this.Rows.Count - 1;
            else
                i = this.Rows.Count;
            string strValue = "";
            if (this.Columns[colIndex].ValueType == typeof(Int16))
            {
                switch (showMode)
                {
                    case totalMode.count:
                        strValue = i.ToString();
                        break;
                    case totalMode.sum:
                        for (int j = 0; j < i; j++)
                        {
                            if (this[colIndex, j].Value != null && this[colIndex, j].Value.ToString() != "")
                                total += (Int16)this[colIndex, j].Value;
                        }
                        strValue = total.ToString();
                        break;
                    case totalMode.max:
                        for (int j = 0; j < i; j++)
                        {
                            if (this[colIndex, j].Value != null && this[colIndex, j].Value.ToString() != "")
                                max = Math.Max((Int16)this[colIndex, j].Value, max);
                        }
                        if (max != decimal.MinValue)
                            strValue = max.ToString();
                        break;
                    case totalMode.min:
                        for (int j = 0; j < i; j++)
                        {
                            if (this[colIndex, j].Value != null && this[colIndex, j].Value.ToString() != "")
                                min = Math.Min((Int16)this[colIndex, j].Value, min);
                        }
                        if (min != decimal.MaxValue)
                            strValue = min.ToString();
                        break;
                    case totalMode.average:
                        if (i != 0)
                        {
                            for (int j = 0; j < i; j++)
                            {
                                if (this[colIndex, j].Value != null && this[colIndex, j].Value.ToString() != "")
                                    total += (Int16)this[colIndex, j].Value;
                            }
                            average = total / i;
                            strValue = average.ToString();
                        }
                        break;
                }
            }
            else if (this.Columns[colIndex].ValueType == typeof(Int32))
            {
                switch (showMode)
                {
                    case totalMode.count:
                        strValue = i.ToString();
                        break;
                    case totalMode.sum:
                        for (int j = 0; j < i; j++)
                        {
                            if (this[colIndex, j].Value != null && this[colIndex, j].Value.ToString() != "")
                                total += (Int32)this[colIndex, j].Value;
                        }
                        strValue = total.ToString();
                        break;
                    case totalMode.max:
                        for (int j = 0; j < i; j++)
                        {
                            if (this[colIndex, j].Value != null && this[colIndex, j].Value.ToString() != "")
                                max = Math.Max((Int32)this[colIndex, j].Value, max);
                        }
                        if (max != decimal.MinValue)
                            strValue = max.ToString();
                        break;
                    case totalMode.min:
                        for (int j = 0; j < i; j++)
                        {
                            if (this[colIndex, j].Value != null && this[colIndex, j].Value.ToString() != "")
                                min = Math.Min((Int32)this[colIndex, j].Value, min);
                        }
                        if (min != decimal.MaxValue)
                            strValue = min.ToString();
                        break;
                    case totalMode.average:
                        if (i != 0)
                        {
                            for (int j = 0; j < i; j++)
                            {
                                if (this[colIndex, j].Value != null && this[colIndex, j].Value.ToString() != "")
                                    total += (Int32)this[colIndex, j].Value;
                            }
                            average = total / i;
                            strValue = average.ToString();
                        }
                        break;
                }
            }
            else if (this.Columns[colIndex].ValueType == typeof(Int64))
            {
                switch (showMode)
                {
                    case totalMode.count:
                        strValue = i.ToString();
                        break;
                    case totalMode.sum:
                        for (int j = 0; j < i; j++)
                        {
                            if (this[colIndex, j].Value != null && this[colIndex, j].Value.ToString() != "")
                                total += (Int64)this[colIndex, j].Value;
                        }
                        strValue = total.ToString();
                        break;
                    case totalMode.max:
                        for (int j = 0; j < i; j++)
                        {
                            if (this[colIndex, j].Value != null && this[colIndex, j].Value.ToString() != "")
                                max = Math.Max((Int64)this[colIndex, j].Value, max);
                        }
                        if (max != decimal.MinValue)
                            strValue = max.ToString();
                        break;
                    case totalMode.min:
                        for (int j = 0; j < i; j++)
                        {
                            if (this[colIndex, j].Value != null && this[colIndex, j].Value.ToString() != "")
                                min = Math.Min((Int64)this[colIndex, j].Value, min);
                        }
                        if (min != decimal.MaxValue)
                            strValue = min.ToString();
                        break;
                    case totalMode.average:
                        if (i != 0)
                        {
                            for (int j = 0; j < i; j++)
                            {
                                if (this[colIndex, j].Value != null && this[colIndex, j].Value.ToString() != "")
                                    total += (Int64)this[colIndex, j].Value;
                            }
                            average = total / i;
                            strValue = average.ToString();
                        }
                        break;
                }
            }
            else if (this.Columns[colIndex].ValueType == typeof(decimal))
            {
                switch (showMode)
                {
                    case totalMode.count:
                        strValue = i.ToString();
                        break;
                    case totalMode.sum:
                        for (int j = 0; j < i; j++)
                        {
                            if (this[colIndex, j].Value != null && this[colIndex, j].Value.ToString() != "")
                                total += (decimal)this[colIndex, j].Value;
                        }
                        strValue = total.ToString();
                        break;
                    case totalMode.max:
                        for (int j = 0; j < i; j++)
                        {
                            if (this[colIndex, j].Value != null && this[colIndex, j].Value.ToString() != "")
                                max = Math.Max((decimal)this[colIndex, j].Value, max);
                        }
                        if (max != decimal.MinValue)
                            strValue = max.ToString();
                        break;
                    case totalMode.min:
                        for (int j = 0; j < i; j++)
                        {
                            if (this[colIndex, j].Value != null && this[colIndex, j].Value.ToString() != "")
                                min = Math.Min((decimal)this[colIndex, j].Value, min);
                        }
                        if (min != decimal.MaxValue)
                            strValue = min.ToString();
                        break;
                    case totalMode.average:
                        if (i != 0)
                        {
                            for (int j = 0; j < i; j++)
                            {
                                if (this[colIndex, j].Value != null && this[colIndex, j].Value.ToString() != "")
                                    total += (decimal)this[colIndex, j].Value;
                            }
                            average = total / i;
                            strValue = average.ToString();
                        }
                        break;
                }
            }
            else if (this.Columns[colIndex].ValueType == typeof(double))
            {
                double dtotal = 0;
                double dmax = double.MinValue;
                double dmin = double.MaxValue;
                double daverage = new double();
                switch (showMode)
                {
                    case totalMode.count:
                        strValue = i.ToString();
                        break;
                    case totalMode.sum:
                        for (int j = 0; j < i; j++)
                        {
                            if (this[colIndex, j].Value != null && this[colIndex, j].Value.ToString() != "")
                                dtotal += (double)this[colIndex, j].Value;
                        }
                        strValue = dtotal.ToString();
                        break;
                    case totalMode.max:
                        for (int j = 0; j < i; j++)
                        {
                            if (this[colIndex, j].Value != null && this[colIndex, j].Value.ToString() != "")
                                dmax = Math.Max((double)this[colIndex, j].Value, dmax);
                        }
                        if (dmax != double.MinValue)
                            strValue = dmax.ToString();
                        break;
                    case totalMode.min:
                        for (int j = 0; j < i; j++)
                        {
                            if (this[colIndex, j].Value != null && this[colIndex, j].Value.ToString() != "")
                                dmin = Math.Min((double)this[colIndex, j].Value, dmin);
                        }
                        if (dmin != double.MaxValue)
                            strValue = dmin.ToString();
                        break;
                    case totalMode.average:
                        if (i != 0)
                        {
                            for (int j = 0; j < i; j++)
                            {
                                if (this[colIndex, j].Value != null && this[colIndex, j].Value.ToString() != "")
                                    dtotal += (double)this[colIndex, j].Value;
                            }
                            daverage = dtotal / i;
                            strValue = daverage.ToString();
                        }
                        break;
                }
            }
            else 
            {
                switch (showMode)
                {
                    case totalMode.count:
                        strValue = i.ToString();
                        break;
                    case totalMode.sum:
                        for (int j = 0; j < i; j++)
                        {
                            object value = this.Columns[colIndex] is InfoDataGridViewExpressionColumn 
                                ? ((InfoDataGridViewExpressionColumn)this.Columns[colIndex]).GetExpression(j) : this[colIndex, j].Value;
                            if (value != null && value.ToString().Length > 0)
                            {
                                total += decimal.Parse(value.ToString());
                            }
                        }
                        strValue = total.ToString();
                        break;
                    case totalMode.max:
                        for (int j = 0; j < i; j++)
                        {
                          object value = this.Columns[colIndex] is InfoDataGridViewExpressionColumn 
                                ? ((InfoDataGridViewExpressionColumn)this.Columns[colIndex]).GetExpression(j) : this[colIndex, j].Value;
                             if (value != null && value.ToString().Length > 0)
                             {
                                 max = Math.Max(decimal.Parse(value.ToString()), max);
                             }
                        }
                        if (max != decimal.MinValue)
                            strValue = max.ToString();
                        break;
                    case totalMode.min:
                        for (int j = 0; j < i; j++)
                        {
                            object value = this.Columns[colIndex] is InfoDataGridViewExpressionColumn 
                                ? ((InfoDataGridViewExpressionColumn)this.Columns[colIndex]).GetExpression(j) : this[colIndex, j].Value;
                            if (value != null && value.ToString().Length > 0)
                            {
                                min = Math.Min(decimal.Parse(value.ToString()), min);
                            }
                        }
                        if (min != decimal.MaxValue)
                            strValue = min.ToString();
                        break;
                    case totalMode.average:
                        if (i != 0)
                        {
                            for (int j = 0; j < i; j++)
                            {
                                 object value = this.Columns[colIndex] is InfoDataGridViewExpressionColumn 
                                ? ((InfoDataGridViewExpressionColumn)this.Columns[colIndex]).GetExpression(j) : this[colIndex, j].Value;
                                if (value != null && value.ToString().Length > 0)
                                {
                                    total += decimal.Parse(value.ToString());
                                }
                            }
                            average = total / i;
                            strValue = average.ToString();
                        }
                        break;
                }
            }
            return strValue;
        }

        protected void OnTotalChanged(TotalChangedEventArgs value)
        {
            TotalChangedEventHandler handler = (TotalChangedEventHandler)Events[EventOnTotalChanged];
            if ((handler != null) && (value is TotalChangedEventArgs))
            {
                handler(this, (TotalChangedEventArgs)value);
            }
        }

        internal static readonly object EventOnTotalChanged = new object();
        public event TotalChangedEventHandler TotalChanged
        {
            add { Events.AddHandler(EventOnTotalChanged, value); }
            remove { Events.RemoveHandler(EventOnTotalChanged, value); }
        }

        /*public void OnAfterTotalDisplay(EventArgs value)
        {
            EventHandler handler = (EventHandler)Events[EventOnAfterTotalDisplay];
            if ((handler != null) && (value is EventArgs))
            {
                handler(this, (EventArgs)value);
            }
            //DesignTotalPanel();
        }

        private void DesignTotalPanel()
        {
            if (this.TotalPanel != null)
            {
                int captionWit = this.RowHeadersWidth;
                if (this.TotalPanel.Controls.Find("grdInfoTotal", true).Length == 0)
                {
                    DataGridView gridView = new DataGridView();
                    gridView.Name = "grdInfoTotal";
                    gridView.Width = this.Width - captionWit;
                    gridView.BackgroundColor = this.BackgroundColor;
                    gridView.AllowUserToResizeColumns = false;
                    gridView.AllowUserToResizeRows = false;
                    gridView.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
                    gridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
                    gridView.ColumnHeadersHeight = 4;
                    gridView.ColumnHeadersVisible = false;
                    gridView.RowHeadersVisible = false;
                    DataGridViewColumn totalColumn = new DataGridViewColumn();
                    foreach (DataGridViewColumn column in this.Columns)
                    {
                        totalColumn = (DataGridViewColumn)column.Clone();
                        totalColumn.HeaderText = "";
                        totalColumn.ReadOnly = true;
                        gridView.Columns.Add(totalColumn);
                    }
                    if (this.TotalActive)
                    {
                        RefreshTotalPanel(gridView, null);
                    }
                    this.TotalPanel.Controls.Add(gridView);
                    gridView.Dock = DockStyle.Left;
                }
                if (this.TotalPanel.Controls.Find("lblInfoTotalCaption", true).Length == 0)
                {
                    Label lbl = new Label();
                    lbl.Name = "lblInfoTotalCaption";
                    switch (this.RowHeadersBorderStyle)
                    {
                        case DataGridViewHeaderBorderStyle.None:
                            lbl.BorderStyle = BorderStyle.None;
                            break;
                        case DataGridViewHeaderBorderStyle.Single:
                            lbl.BorderStyle = BorderStyle.FixedSingle;
                            break;
                        case DataGridViewHeaderBorderStyle.Raised:
                        case DataGridViewHeaderBorderStyle.Sunken:
                        case DataGridViewHeaderBorderStyle.Custom:
                            lbl.BorderStyle = BorderStyle.Fixed3D;
                            break;
                    }
                    lbl.BackColor = this.RowHeadersDefaultCellStyle.BackColor;
                    lbl.Width = this.RowHeadersWidth;
                    lbl.Text = this.TotalCaption;
                    this.TotalPanel.Controls.Add(lbl);
                    lbl.Dock = DockStyle.Left;
                }
            }
        }

        private void RefreshTotalPanel(DataGridView gridView, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = gridView.Rows[0];
            if (e == null)
            {
                foreach (DataGridViewColumn column in gridView.Columns)
                {
                    foreach (TotalColumn tc in this.TotalColumns)
                    {
                        if (tc.ColumnName == column.Name && tc.ShowTotal)
                        {
                            row.Cells[column.Name].Value = this.DoSum(column.Index, tc.TotalMode);
                            switch (tc.TotalAlignment)
                            {
                                case TotalColumn.TotalAlign.left:
                                    row.Cells[column.Name].Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
                                    break;
                                case TotalColumn.TotalAlign.center:
                                    row.Cells[column.Name].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                    break;
                                case TotalColumn.TotalAlign.right:
                                    row.Cells[column.Name].Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                                    break;
                            }
                            break;
                        }
                    }
                }
            }
            else
            {
                foreach (TotalColumn col in this.TotalColumns)
                {
                    if (col.ColumnName == this.Columns[e.ColumnIndex].Name)
                    {
                        string strSum = this.DoSum(e.ColumnIndex, col.TotalMode);
                        row.Cells[col.Name].Value = strSum;
                        OnTotalChanged(new TotalChangedEventArgs(e.ColumnIndex, this.Columns[e.ColumnIndex].Name, strSum));
                    }
                }
            }
        }

        protected override void OnColumnWidthChanged(DataGridViewColumnEventArgs e)
        {
            base.OnColumnWidthChanged(e);
            if(this.TotalPanel != null)
            {
                Control[] ctrls = this.TotalPanel.Controls.Find("grdInfoTotal", true);
                if (ctrls.Length == 1)
                {
                    DataGridView dgv = (DataGridView)ctrls[0];
                    foreach (DataGridViewColumn column in dgv.Columns)
                    {
                        if (column.Name == e.Column.Name)
                        {
                            column.Width = e.Column.Width;
                        }
                    }
                }
            }
        }

        protected override void OnRowHeadersWidthChanged(EventArgs e)
        {
            base.OnRowHeadersWidthChanged(e);
            if (this.TotalPanel != null)
            {
                Control[] ctrls = this.TotalPanel.Controls.Find("lblInfoTotalCaption", true);
                if (ctrls.Length == 1)
                {
                    Label lbl = (Label)ctrls[0];
                    lbl.Width = this.RowHeadersWidth;
                }
            }
        }

        internal static readonly object EventOnAfterTotalDisplay = new object();

        public event EventHandler AfterTotalDisplay
        {
            add { Events.AddHandler(EventOnAfterTotalDisplay, value); }
            remove { Events.RemoveHandler(EventOnAfterTotalDisplay, value); }
        }*/

        protected override void OnCellEnter(DataGridViewCellEventArgs e)
        {
            base.OnCellEnter(e);
            if ((this.DataSource as InfoBindingSource) != null && (this.DataSource as InfoBindingSource).AutoApply)
            {
                if (this.SelectedCells.Count > 0)
                {
                    this.Rows[this.SelectedCells[0].RowIndex].Cells[this.SelectedCells[0].ColumnIndex].Selected = false;
                    this.Rows[e.RowIndex].Cells[e.ColumnIndex].Selected = true;
                }
            }
        }
    }

    #region TotalChangedEvent
    public delegate void TotalChangedEventHandler(object sender, TotalChangedEventArgs e);

    public sealed class TotalChangedEventArgs : EventArgs
    {
        public TotalChangedEventArgs(int columnIndex, string columnName, object totalValue)
        {
            _TotalValue = totalValue;
            _ColumnIndex = columnIndex;
            _ColumnName = columnName;
        }

        private object _TotalValue;
        public object TotalValue
        {
            get
            {
                return _TotalValue;
            }
        }

        private int _ColumnIndex;
        public int ColumnIndex
        {
            get
            {
                return _ColumnIndex;
            }
        }

        private string _ColumnName;
        public string ColumnName
        {
            get
            {
                return _ColumnName;
            }
        }
    }
    #endregion
}
