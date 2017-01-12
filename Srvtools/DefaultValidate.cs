using System;
using System.Collections;
using System.Text;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using System.Reflection;
using System.Runtime.Remoting;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using System.Drawing;
using System.Collections.Generic;

namespace Srvtools
{
    #region Delegate
    public delegate void ValidateEventHandler(object sender, ValidateEventArgs e);
    public delegate void ShowMessageEventHandler(object sender, ShowMessageEventArgs e);
    public delegate void ValidateMethod(string value);
    #endregion Delegate

    #region DefaultValidate EventArgs
    public class ValidateEventArgs : EventArgs
    {
        public ValidateEventArgs()
            : base()
        {
            m_success = true;
        }

        public ValidateEventArgs(bool success)
            : base()
        {
            m_success = success;
        }

        private bool m_success;
        public bool Success
        {
            get
            {
                return m_success;
            }
            set
            {
                m_success = value;
            }
        }
    }


    public class ShowMessageEventArgs : EventArgs
    {
        public ShowMessageEventArgs()
            : base()
        {
            m_continue = true;
        }

        public ShowMessageEventArgs(bool willContinue)
            : base()
        {
            m_continue = willContinue;
        }

        private bool m_continue;
        public bool Continue
        {
            get
            {
                return m_continue;
            }
            set
            {
                m_continue = value;
            }
        }

        private IList m_collection;
        public IList Collection
        {
            get
            {
                return m_collection;
            }
            set
            {
                m_collection = value;
            }
        }

        private int m_index;
        public int Index
        {
            get
            {
                return m_index;
            }
            set
            {
                m_index = value;
            }
        }

        private string m_fieldName;
        public string FieldName
        {
            get
            {
                return m_fieldName;
            }
            set
            {
                m_fieldName = value;
            }
        }
    }

    #endregion DefaultValidate EventArgs

    #region DefaultValidate
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(DefaultValidate), "Resources.DefaultValidate.png")]
    public class DefaultValidate : InfoBaseComp, ISupportInitialize
    {
        #region Private Fields
        private InfoBindingSource m_bindingSource;
        private bool m_defaultActive;
        private bool m_validActive;
        private FieldItemCollection m_fieldItems;
        private bool m_carryOn;
      
        private static object EventValidate = new object();
        private static object EventShowMessage = new object();
        //private SYS_LANGUAGE language;
        //private DataRow PrevEditedRow = null;
        private Hashtable PrevEditedRowValues = new Hashtable();
        private bool bCheckNullFailed = false;
        #endregion  Private Fields

        #region ISupportInitialize
        public void BeginInit()
        { }

        public void EndInit()
        { }
        #endregion

        protected override void DoAfterSetOwner(IDataModule value)
        {
            base.DoAfterSetOwner(value);
            if (this.ValidActive)
            {
                AllCtrls.Clear();
                this.GetAllCtrls(((Form)this.OwnerComp).Controls);
                foreach (Control ctrl in AllCtrls)
                {
                    if (ctrl is Label)
                    {
                        foreach (FieldItem vfi in this.FieldItems)
                        {
                            if (vfi.ValidateLabelLink == ((Label)ctrl).Name && this.ValidateColor != null)
                            {
                                if (vfi.CheckNull || vfi.CheckRangeFrom.Trim().Length > 0
                                                || vfi.CheckRangeTo.Trim().Length > 0 || vfi.Validate.Trim().Length > 0)
                                {
                                    ((Label)ctrl).ForeColor = this.ValidateColor;
                                    ((Label)ctrl).Text = ((Label)ctrl).Text.Insert(0, this.ValidateChar);
                                }
                            }
                        }
                    }
                    if (ctrl is InfoDataGridView)
                    {
                        //modified by lily 2007/4/25 for multi-details may have many *s.
                        object obj = ((InfoDataGridView)ctrl).DataSource;
                        if (obj != null && obj is InfoBindingSource && (InfoBindingSource)obj == this.BindingSource)
                        {
                            InfoDataGridView grid = (InfoDataGridView)ctrl;
                            if (!grid.ReadOnly || (obj as InfoBindingSource).AutoDisableControl)
                            {
                                foreach (DataGridViewColumn field in grid.Columns)
                                {
                                    foreach (FieldItem vfi in this.FieldItems)
                                    {
                                        if (vfi.FieldName == field.DataPropertyName && this.ValidateColor != null)
                                        {
                                            if (vfi.CheckNull || vfi.CheckRangeFrom.Trim().Length > 0
                                                || vfi.CheckRangeTo.Trim().Length > 0 || vfi.Validate.Trim().Length > 0)
                                            {
                                                field.HeaderCell.Style.ForeColor = this.ValidateColor;
                                                field.HeaderText = field.HeaderText.Insert(0, this.ValidateChar);
                                            }
                                        }
                                    }
                                }

                            }
                        }
                    }
                }
            }

            if (this.LeaveValidation)
            {
                Form form = this.OwnerComp as Form;
                Type type = form.GetType();
                FieldInfo[] fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                foreach (FieldInfo field in fields)
                {
                    for (int i = 0; i < this.FieldItems.Count; i++)
                    {
                        FieldItem fi = FieldItems[i];
                        object obj = field.GetValue(form);
                        if (obj != null)
                        {
                            if (obj is InfoRefvalBox && (obj as InfoRefvalBox).TextBoxBindingMember == fi.FieldName)
                            {
                                (obj as Control).Leave += new EventHandler(DefaultValidate_Leave);
                                break;
                            }
                            else if (obj is Control && (obj as Control).DataBindings.Count > 0 && (obj as Control).DataBindings[0].BindingMemberInfo.BindingField == fi.FieldName)
                            {
                                (obj as Control).Leave += new EventHandler(DefaultValidate_Leave);
                                break;
                            }
                        }
                    }
                }
            }
        }

        void DefaultValidate_Leave(object sender, EventArgs e)
        {
            DataRowView rowView = ((DataRowView)this.BindingSource.Current);
            String columnName = String.Empty;
            if (sender is InfoRefvalBox)
            {
                columnName = (sender as InfoRefvalBox).TextBoxBindingMember;
            }
            else if (sender is Control && (sender as Control).DataBindings.Count > 0)
            {
                columnName = (sender as Control).DataBindings[0].BindingMemberInfo.BindingField;
            }

            if (columnName != String.Empty)
                CheckNullAndRange(rowView, columnName);
        }

        #region Constructor
        public DefaultValidate(System.ComponentModel.IContainer container)
            : this()
        {
            container.Add(this);
            _ValidateChar = "*";
            m_defaultActive = true;
            m_validActive = true;
            _ValidateMode = ValidMode.All;
            _CheckKeyFieldEmpty = true;
        }

        internal DefaultValidate()
            : base()
        {
            FieldItems = new FieldItemCollection(this);
        }
        #endregion Constructor

        #region Properties
        private DataSet dsDD = new DataSet();
        [Category("Infolight"),
        Description("The InfoBindingSource which the control is bound to")]
        public InfoBindingSource BindingSource
        {
            get
            {
                return m_bindingSource;
            }
            set
            {
                if (value == null)
                {
                    m_bindingSource = value;
                    return;
                }
                if (value is InfoBindingSource)
                {
                    m_bindingSource = value;

                    // ListChanged Event : Used to binding default value
                    m_bindingSource.ListChanged += delegate(object sender, ListChangedEventArgs e)
                    {
                        if (this.DefaultActive || this.CarryOn)
                        {
                            if (e.ListChangedType == ListChangedType.ItemAdded)
                            {
                                DataRowView rowView = m_bindingSource.List[e.NewIndex] as DataRowView;

                                if (rowView != null && rowView.IsNew)
                                {
                                    DataTable table = rowView.Row.Table;
                                    #region Do carry on
                                    if (this.CarryOn)
                                    {
                                        if(this.PrevEditedRowValues.Count > 0)
                                        //if (this.PrevEditedRow != null)
                                        {
                                            foreach (DataColumn column in table.Columns)
                                            {
                                                // add by Rax
                                                foreach (FieldItem fi in this.FieldItems)
                                                {
                                                    if (column.ColumnName == fi.FieldName)
                                                    {
                                                        if (fi.CarryOn)
                                                        {
                                                            try
                                                            {
                                                                //rowView.Row[column] = this.PrevEditedRow[column].ToString();
                                                                rowView.Row[column] = this.PrevEditedRowValues[column.ColumnName];
                                                            }
                                                            catch
                                                            {
                                                            }
                                                        }
                                                        else
                                                        {
                                                            string s = GetDefaultValue(fi);
                                                            if (s != "")
                                                                rowView.Row[column] = s;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else if (this.DefaultActive)
                                        {
                                            foreach (FieldItem fieldItem in this.FieldItems)
                                            {
                                                if (fieldItem.FieldName != null && fieldItem.FieldName != "")
                                                {
                                                    DataColumn column = table.Columns[fieldItem.FieldName];
                                                    if (column != null)
                                                    {
                                                        string s = GetDefaultValue(fieldItem);
                                                        if (s != "")
                                                            rowView.Row[column] = s;
                                                    }
                                                }
                                            }
                                        }
                                        // end
                                    }
                                    else
                                    {
                                        foreach (FieldItem fieldItem in this.FieldItems)
                                        {
                                            if (fieldItem.FieldName != null && fieldItem.FieldName != "")
                                            {
                                                DataColumn column = table.Columns[fieldItem.FieldName];
                                                if (column != null)
                                                {
                                                    string s = GetDefaultValue(fieldItem);
                                                    if (s != "")
                                                        rowView.Row[column] = s;
                                                }
                                            }
                                        }
                                        AllCtrls.Clear();
                                    }
                                    #endregion

                                    if (this.OwnerComp != null)
                                    {
                                         this.GetAllCtrls(((Form)this.OwnerComp).Controls);
                                    }

                                    foreach (Control ctrl in AllCtrls)
                                    {
                                        if (ctrl.DataBindings != null)
                                        {
                                            foreach (Binding binding in ctrl.DataBindings)
                                            {
                                                if (binding.DataSource is InfoBindingSource
                                                    && (InfoBindingSource)binding.DataSource == m_bindingSource)
                                                {
                                                    binding.ReadValue();
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    };

                    // ListChanged Event : Used to DeplicateCheck
                    m_bindingSource.ListChanged += delegate(object sender, ListChangedEventArgs e)
                    {
                        if (!this.DesignMode)
                        {
                            if (e.ListChangedType == ListChangedType.ItemChanged || e.ListChangedType == ListChangedType.ItemAdded)
                            {
                                DataRowView rowView = (DataRowView)m_bindingSource.List[e.NewIndex];
                                //modified by ccm 2009/05/14 在取消删除后会进入,此时的状态是UnChanged
                                if (rowView != null && rowView.Row.RowState != DataRowState.Unchanged && rowView.RowVersion == DataRowVersion.Current)
                                {
                                    this.BindingSource.bDeplChk = true;
                                    bool isInsert = e.ListChangedType == ListChangedType.ItemAdded;
                                    this.BindingSource.CheckDeplicateSucess = this.CheckDuplicate(rowView, isInsert);
                                }
                            }
                        }
                    };

                    // ListChanged Event : Used to Check Null & Validate
                    m_bindingSource.ListChanged += delegate(object sender, ListChangedEventArgs e)
                    {
                        //暂时这样解决,不然在InfoBindingSource的InEndEdit中会重复
                        if (!(this.BindingSource.DataSource is InfoBindingSource) && !this.BindingSource.AutoApply)
                        {
                            return;
                        }
                        if (!this.DesignMode)
                        {
                            string CaptionNum = "CAPTION";
                            if (this.MultiLanguage)
                            {
                                switch (CliUtils.fClientLang)
                                {
                                    case SYS_LANGUAGE.ENG:
                                        CaptionNum = "CAPTION1"; break;
                                    case SYS_LANGUAGE.TRA:
                                        CaptionNum = "CAPTION2"; break;
                                    case SYS_LANGUAGE.SIM:
                                        CaptionNum = "CAPTION3"; break;
                                    case SYS_LANGUAGE.HKG:
                                        CaptionNum = "CAPTION4"; break;
                                    case SYS_LANGUAGE.JPN:
                                        CaptionNum = "CAPTION5"; break;
                                    case SYS_LANGUAGE.LAN1:
                                        CaptionNum = "CAPTION6"; break;
                                    case SYS_LANGUAGE.LAN2:
                                        CaptionNum = "CAPTION7"; break;
                                    case SYS_LANGUAGE.LAN3:
                                        CaptionNum = "CAPTION8"; break;
                                }
                            }
                            if (dsDD.Tables.Count == 0)
                            {
                                dsDD = DBUtils.GetDataDictionary(this.BindingSource, false);
                            }
                            if (e.ListChangedType == ListChangedType.ItemChanged
                                || e.ListChangedType == ListChangedType.ItemAdded)
                            {
                                if (this.ValidateListBox != null)
                                {
                                    this.ValidateListBox.Items.Clear();
                                }
                                int index = e.NewIndex;

                                DataRowView rowView = m_bindingSource.List[index] as DataRowView;
                                InfoNavigator nav = new InfoNavigator();
                                if (rowView != null && rowView.RowVersion == DataRowVersion.Current)
                                {
                                    DataTable table = rowView.Row.Table;
                                    // andy add CheckSuess 2007.04.03
                                    this.BindingSource.CheckSucess = true;

                                    if (rowView.Row != null && rowView.Row.RowState != DataRowState.Detached)
                                    {
                                        bool b = false;
                                        int p = ((Form)this.OwnerComp).Controls.Count;
                                        for (int q = 0; q < p; q++)
                                        {
                                            if (((Form)this.OwnerComp).Controls[q] is InfoNavigator
                                                && ((InfoNavigator)((Form)this.OwnerComp).Controls[q]).BindingSource == this.BindingSource)
                                            {
                                                nav = (InfoNavigator)((Form)this.OwnerComp).Controls[q];
                                                b = nav.EndValidRowByRow;
                                                break;
                                            }
                                        }
                                        foreach (FieldItem fieldItem in this.FieldItems)
                                        {
                                            string caption = fieldItem.FieldName;
                                            if (this.dsDD.Tables.Count > 0)
                                            {
                                                foreach (DataRow dr in this.dsDD.Tables[0].Rows)
                                                {
                                                    if (string.Compare(dr["FIELD_NAME"].ToString(), fieldItem.FieldName, true) == 0//IgnoreCase
                                                        && dr[CaptionNum].ToString() != null && dr[CaptionNum].ToString() != "")
                                                    {
                                                        caption = dr[CaptionNum].ToString();
                                                    }
                                                }
                                            }
                                            bCheckNullFailed = false;
                                            if (fieldItem.FieldName != null && fieldItem.FieldName != "")
                                            {
                                                DataColumn column = table.Columns[fieldItem.FieldName];
                                                if (column != null)
                                                {
                                                    // Check Null
                                                    if (fieldItem.CheckNull == true && m_validActive == true)
                                                    {
                                                        if (rowView.Row[column] == null || rowView.Row[column].ToString().Trim() == "")
                                                        {
                                                            if (!this.BindingSource.AutoApply && !b)
                                                            {
                                                                String message = SysMsg.GetSystemMessage(CliUtils.fClientLang,
                                                                                                         "Srvtools",
                                                                                                         "DefaultValidate",
                                                                                                         "msg_DefaultValidateCheckNull");
                                                                this.Warn(string.Format(message, caption));
                                                                this.BindingSource.bChk = true;
                                                                //2006/08/01
                                                                this.BindingSource.CheckSucess = false;
                                                                //modified by lily 2007/4/25 for state
                                                                if (nav.GetCurrentState() != "Inserting")
                                                                {
                                                                    nav.SetState("Editing");
                                                                }
                                                                //2006/08/01
                                                            }
                                                            bCheckNullFailed = true;
                                                            lstCheckFailedRow.Add(rowView);
                                                        }
                                                    }
                                                    // Check Range
                                                    if (m_validActive == true && !bCheckNullFailed)
                                                    {
                                                        int belowRangeFlag = 1;
                                                        int aboveRangeFlag = -1;
                                                        int rangeExsitFlag = 0;
                                                        if (fieldItem.CheckRangeFrom != null && fieldItem.CheckRangeFrom != "")
                                                        {
                                                            belowRangeFlag = CompareRange(rowView.Row[column].GetType(), rowView.Row[column], fieldItem.CheckRangeFrom);
                                                            rangeExsitFlag |= 1;
                                                        }
                                                        if (fieldItem.CheckRangeTo != null && fieldItem.CheckRangeTo != "")
                                                        {
                                                            aboveRangeFlag = CompareRange(rowView.Row[column].GetType(), rowView.Row[column], fieldItem.CheckRangeTo);
                                                            rangeExsitFlag |= 2;
                                                        }
                                                       
                                                        if ((int)belowRangeFlag < 0 || (int)aboveRangeFlag > 0)
                                                        {
                                                            if (!this.BindingSource.AutoApply && !b)
                                                            {
                                                                string message = "";
                                                                if (rangeExsitFlag == 1)
                                                                {
                                                                    string mess = SysMsg.GetSystemMessage(CliUtils.fClientLang,
                                                                                                         "Srvtools",
                                                                                                         "DefaultValidate",
                                                                                                         "msg_DefaultValidateCheckRangeFrom");
                                                                    message = string.Format(mess, caption, fieldItem.CheckRangeFrom);

                                                                }
                                                                else if (rangeExsitFlag == 2)
                                                                {
                                                                    string mess = SysMsg.GetSystemMessage(CliUtils.fClientLang,
                                                                                                         "Srvtools",
                                                                                                         "DefaultValidate",
                                                                                                         "msg_DefaultValidateCheckRangeTo");
                                                                    message = string.Format(mess, caption, fieldItem.CheckRangeTo);
                                                                }
                                                                else
                                                                {
                                                                    string mess = SysMsg.GetSystemMessage(CliUtils.fClientLang,
                                                                                                         "Srvtools",
                                                                                                         "DefaultValidate",
                                                                                                         "msg_DefaultValidateCheckRange");
                                                                    message = string.Format(mess, caption, fieldItem.CheckRangeFrom, fieldItem.CheckRangeTo);
                                                                }
                                                                this.Warn(string.Format(message, caption));
                                                                this.BindingSource.bChk = true;
                                                                //2006/08/01
                                                                this.BindingSource.CheckSucess = false;
                                                                //modified by lily 2007/4/25 for state
                                                                if (nav.GetCurrentState() != "Inserting")
                                                                {
                                                                    nav.SetState("Editing");
                                                                }
                                                                //2006/08/01

                                                            }
                                                            lstCheckFailedRow.Add(rowView);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        if (!this.BindingSource.AutoApply && !b)
                                        {
                                            ValidateRow(index, rowView, nav);
                                            this.BindingSource.bChk = true;
                                            // andy add by 2007.04.03 for detail validae when master apply issue duplicate message
                                            if (this.BindingSource.DataSource != null)
                                            {
                                                //((InfoBindingSource)this.BindingSource.DataSource).CheckSucess = false;  error of type cast
                                                if (BindingSource.DataSource.GetType() == typeof(InfoBindingSource))
                                                    ((InfoBindingSource)this.BindingSource.DataSource).CheckSucess = this.BindingSource.CheckSucess;

                                            }
                                        }
                                        this.ResetWarnging();
                                    }
                                }
                            }
                        }
                    };

                    // Copy the edited row to default
                    m_bindingSource.ListChanged += delegate(object sender, ListChangedEventArgs e)
                    {
                        if (e.ListChangedType == ListChangedType.ItemChanged
                            || e.ListChangedType == ListChangedType.ItemAdded)
                        {
                            DataRowView rowView = m_bindingSource.List[e.NewIndex] as DataRowView;
                            if (rowView != null)
                            {
                                //this.PrevEditedRow = rowView.Row;
                                DataRow row = rowView.Row;
                                PrevEditedRowValues.Clear();
                                foreach (DataColumn column in row.Table.Columns)
                                {
                                    PrevEditedRowValues.Add(column.ColumnName, row[column.ColumnName]);
                                }
                            }
                        }
                    };

                    // Clear PrevEditedRow when DataMember or DataSource Changes
                    m_bindingSource.DataSourceChanged += delegate(object sender, EventArgs e)
                    {
                        //this.PrevEditedRow = null;
                        PrevEditedRowValues.Clear();
                    };

                    m_bindingSource.DataMemberChanged += delegate(object sender, EventArgs e)
                    {
                        //this.PrevEditedRow = null;
                        PrevEditedRowValues.Clear();
                    };
                }
            }
        }

        public bool CheckDuplicate(DataRowView rowView, bool IsInsert)
        {
            bool Ret = true;
            if (this.DuplicateCheck)
            {
                object obj = this.BindingSource.GetDataSource();
                if (obj != null && obj is InfoDataSet)
                {
                    InfoDataSet ds = (InfoDataSet)obj;

                    DataTable table = ds.RealDataSet.Tables[this.BindingSource.GetTableName()];
                    if (!IsInsert)
                    {
                        bool isOriginal = true;
                        foreach (DataColumn key in table.PrimaryKey)
                        {
                            DataRow row = rowView.Row;
                            if (row.HasVersion(DataRowVersion.Original)
                                && !row[key.ColumnName, DataRowVersion.Original].Equals(row[key.ColumnName, DataRowVersion.Current]))
                            {
                                isOriginal = false;
                                break;
                            }
                        }
                        if (isOriginal)
                        {
                            return true;
                        }
                    }
                    if (this.DuplicateCheckMode == DupCheckMode.ByWhere)
                    {
                        string tabName = "", strModuleName = "", strTableName = "", sCurProject = "", sql = "";
                        sCurProject = CliUtils.fCurrentProject;
                        strModuleName = ds.RemoteName.Substring(0, ds.RemoteName.LastIndexOf('.'));
                        strTableName = this.BindingSource.GetTableName();
                        //modified by lily 2007/6/7 tablename應該抓from後面的名字，不考慮別名
                        if (strModuleName != "" && strTableName != "" && sCurProject != "")
                            tabName = CliUtils.GetTableName(strModuleName, strTableName, sCurProject, "", true);
                        if (tabName != "" && table.PrimaryKey.Length > 0)
                        {
                            sql = "select * from " + tabName + " where ";
                            foreach (DataColumn key in table.PrimaryKey)
                            {
                                Type type = ds.RealDataSet.Tables[strTableName].Columns[key.ColumnName].DataType;
                                if (type == typeof(uint) || type == typeof(UInt16) || type == typeof(UInt32)
                                    || type == typeof(UInt64) || type == typeof(int) || type == typeof(Int16)
                                    || type == typeof(Int32) || type == typeof(Int64) || type == typeof(Single)
                                    || type == typeof(double) || type == typeof(decimal))
                                {
                                    if (rowView[key.ColumnName].ToString() == "")
                                        return true;
                                    sql += key.ColumnName + "=" + rowView[key.ColumnName].ToString() + " and ";
                                }
                                else if (type == typeof(DateTime))
                                {
                                    if (rowView[key.ColumnName].ToString() == "")
                                        return true;
                                    sql += key.ColumnName + "='" + ((DateTime)rowView[key.ColumnName]).ToString("yyyy/MM/dd") + "' and ";
                                }
                                else
                                {
                                    if (rowView[key.ColumnName].ToString() == "")
                                        return true;
                                    sql += key.ColumnName + "='" + rowView[key.ColumnName].ToString() + "' and ";
                                }
                            }
                        }
                        if (sql != "")
                        {
                            sql = sql.Substring(0, sql.LastIndexOf(" and "));
                        }
                        DataSet dsTemp = new DataSet();
                        if (strModuleName != "" && strTableName != "" && sCurProject != "")
                            dsTemp = CliUtils.ExecuteSql(strModuleName, strTableName, sql, true, sCurProject);
                        if (dsTemp.Tables.Count != 0 && dsTemp.Tables[0].Rows.Count != 0)
                        {
                            String message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "Srvtools", "DefaultValidate", "DeplicateWarning");
                            MessageBox.Show(message);
                            Ret = false;
                            SetEditing();
                        }
                    }
                }
            }
            return Ret;
        }

        [Obsolete("The recommended alternative is CheckDuplicate", false)]
        public bool DeplCheck(DataRowView rowView, bool IsInsert)
        {
            return CheckDuplicate(rowView, IsInsert);
        }

        private void SetEditing()
        {
            int p = ((Form)this.OwnerComp).Controls.Count;
            for (int q = 0; q < p; q++)
            {
                if (((Form)this.OwnerComp).Controls[q] is InfoNavigator
                    && ((InfoNavigator)((Form)this.OwnerComp).Controls[q]).BindingSource == this.BindingSource)
                {
                    if (((InfoNavigator)((Form)this.OwnerComp).Controls[q]).GetCurrentState() != "Inserting")
                    {
                        ((InfoNavigator)((Form)this.OwnerComp).Controls[q]).SetState("Editing");
                        break;
                    }
                }
            }
        }

        public List<DataRowView> lstCheckFailedRow = new List<DataRowView>();

        private int CompareRange(Type valType, object value, string strRange)
        {
            int range = 0;
            if (valType == typeof(string))
            {
                //range = ((string)value).CompareTo(strRange);
                int count = 0;
                string s = value.ToString();
                int i = s.Length;
                int j = strRange.Length;
                if (i >= j)
                    count = j;
                else
                    count = i;
                for (int m = 0; m < count; m++)
                {
                    if (s[m] > strRange[m])
                    {
                        range = 2;
                        break;
                    }
                    else if (s[m] < strRange[m])
                    {
                        range = -2;
                        break;
                    }
                }
                if (range == -1)
                {
                    if (i > j)
                        range = 1;
                    else if (i == j)
                        range = 0;
                }
            }
            else if (valType == typeof(Int16))
            {
                range = ((Int16)value).CompareTo(Convert.ToInt16(strRange));
            }
            else if (valType == typeof(Int32))
            {
                range = ((Int32)value).CompareTo(Convert.ToInt32(strRange));
            }
            else if (valType == typeof(Int64))
            {
                range = ((Int64)value).CompareTo(Convert.ToInt64(strRange));
            }
            else if (valType == typeof(UInt16))
            {
                range = ((UInt16)value).CompareTo(Convert.ToUInt16(strRange));
            }
            else if (valType == typeof(UInt32))
            {
                range = ((UInt32)value).CompareTo(Convert.ToUInt32(strRange));
            }
            else if (valType == typeof(UInt64))
            {
                range = ((UInt64)value).CompareTo(Convert.ToUInt64(strRange));
            }
            else if (valType == typeof(float))
            {
                range = ((float)value).CompareTo(Convert.ToSingle(strRange));
            }
            else if (valType == typeof(double))
            {
                range = ((double)value).CompareTo(Convert.ToDouble(strRange));
            }
            else if (valType == typeof(decimal))
            {
                range = ((decimal)value).CompareTo(Convert.ToDecimal(strRange));
            }
            else if (valType == typeof(DateTime))
            {
                range = ((DateTime)value).CompareTo(Convert.ToDateTime(strRange));
            }
            return range;
        }

        internal bool ValidateRow(int index, DataRowView rowView)
        {
            return ValidateRow(index, rowView, null);
        }

        internal bool ValidateRow(int index, DataRowView rowView, InfoNavigator nav)
        {
            bool isValidateSuccessful = true;
            bool continueToValidate = true;
            DataTable table = rowView.Row.Table;

            if (this.ValidActive)
            {
                foreach (FieldItem fieldItem in this.FieldItems)
                {
                    if (fieldItem.FieldName == null || fieldItem.FieldName == "")
                    {
                        continue;
                    }
                    DataColumn column = table.Columns[fieldItem.FieldName];
                    if (column != null)
                    {
                        // Validate funtion
                        Object obj = fieldItem.Validate;
                        if (obj != null)
                        {
                            string validateValue = GetValidateValue(fieldItem.Validate.Trim());
                            if (continueToValidate && validateValue != null && validateValue != "")
                            {
                                if (OwnerComp is Form)
                                {
                                    Type t = OwnerComp.GetType();
                                    MethodInfo method = null;
                                    try
                                    {
                                        method = t.GetMethod(validateValue, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                                    }
                                    catch (AmbiguousMatchException)
                                    {
                                        MessageBox.Show("More than one " + fieldItem.Validate.Trim() + " Method found, Please Rename the Validate Method of Field " + fieldItem.FieldName);
                                        continue;
                                    }
                                    if (method == null || method.ReturnType.FullName != "System.Boolean" || method.GetParameters().GetLength(0) != 1/* || method.GetParameters()[0].ParameterType.FullName != "System.Object"*/)
                                    {
                                        //language = CliSysMegLag.GetClientLanguage();
                                        String message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "Srvtools", "DefaultValidate", "msg_ValidMethodNotFound");
                                        Exception ex = new Exception(string.Format(message, validateValue + "()"));
                                        CliUtils.Application_ThreadException(null, new System.Threading.ThreadExceptionEventArgs(ex));
                                        goto EndValidateField; // add by andy 2006.06.07
                                    }
                                    bool isValid = (bool)method.Invoke(OwnerComp, new object[] { rowView.Row[column].ToString() });
                                    if (isValid == false)
                                    {
                                        isValidateSuccessful = false;
                                        ShowMessageEventHandler showMessageHandler = Events[EventShowMessage] as ShowMessageEventHandler;

                                        if (showMessageHandler == null)
                                        {
                                            string message = "";
                                            if (fieldItem.WarningMsg != null && fieldItem.WarningMsg != "")
                                            {
                                                message = fieldItem.WarningMsg;
                                            }
                                            else
                                            {
                                                message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "Srvtools", "DefaultValidate", "msg_DefaultValidateCheckMethod");
                                            }
                                            this.Warn(message);
                                        }
                                        else
                                        {
                                            ShowMessageEventArgs showMessageArgs = new ShowMessageEventArgs(true);
                                            showMessageArgs.Collection = this.m_bindingSource.List;
                                            showMessageArgs.Index = index;
                                            showMessageArgs.FieldName = fieldItem.FieldName;
                                            OnShowMessage(showMessageArgs);
                                            continueToValidate = showMessageArgs.Continue;
                                        }
                                        this.BindingSource.CheckSucess = false;
                                        //2006/08/01
                                        if (nav != null)
                                        {
                                            this.BindingSource.bChk = true;
                                            this.BindingSource.CheckSucess = false;
                                            //modified by lily 2007/4/25 for state
                                            if (nav.GetCurrentState() != "Inserting")
                                            {
                                                nav.SetState("Editing");
                                            }
                                        }
                                        //2006/08/01
                                    }

                                }
                            EndValidateField: ;
                            }
                        }
                    }
                }
                // Invoke OnValidate
                OnValidate(new ValidateEventArgs(isValidateSuccessful));
            }

            return isValidateSuccessful;
        }

        internal bool CheckNullAndRange(DataRowView rowView)
        {
            bool isValidateSuccessful = true;
            if (!this.DesignMode)
            {
                if (dsDD.Tables.Count == 0)
                {
                    dsDD = DBUtils.GetDataDictionary(this.BindingSource, false);
                }
                if (this.ValidActive)
                {
                    string CaptionNum = "CAPTION";
                    if (this.MultiLanguage)
                    {
                        switch (CliUtils.fClientLang)
                        {
                            case SYS_LANGUAGE.ENG:
                                CaptionNum = "CAPTION1"; break;
                            case SYS_LANGUAGE.TRA:
                                CaptionNum = "CAPTION2"; break;
                            case SYS_LANGUAGE.SIM:
                                CaptionNum = "CAPTION3"; break;
                            case SYS_LANGUAGE.HKG:
                                CaptionNum = "CAPTION4"; break;
                            case SYS_LANGUAGE.JPN:
                                CaptionNum = "CAPTION5"; break;
                            case SYS_LANGUAGE.LAN1:
                                CaptionNum = "CAPTION6"; break;
                            case SYS_LANGUAGE.LAN2:
                                CaptionNum = "CAPTION7"; break;
                            case SYS_LANGUAGE.LAN3:
                                CaptionNum = "CAPTION8"; break;
                        }
                    }
                    if (this.ValidateListBox != null)
                    {
                        this.ValidateListBox.Items.Clear();
                    }
                    foreach (FieldItem fieldItem in this.FieldItems)
                    {
                        string caption = fieldItem.FieldName;
                        if (this.dsDD.Tables.Count > 0)
                        {
                            foreach (DataRow dr in this.dsDD.Tables[0].Rows)
                            {
                                if (string.Compare(dr["FIELD_NAME"].ToString(), fieldItem.FieldName, true) == 0//IgnoreCase
                                    && dr[CaptionNum].ToString() != null && dr[CaptionNum].ToString() != "")
                                {
                                    caption = dr[CaptionNum].ToString();
                                }
                            }
                        }
                        if (fieldItem.FieldName != null && fieldItem.FieldName != "")
                        {
                            if (fieldItem.CheckNull)
                            {
                                if (rowView.Row[fieldItem.FieldName] == null || rowView.Row[fieldItem.FieldName].ToString() == "")
                                {
                                    //language = CliSysMegLag.GetClientLanguage();
                                    String message = SysMsg.GetSystemMessage(CliUtils.fClientLang,
                                                                             "Srvtools",
                                                                             "DefaultValidate",
                                                                             "msg_DefaultValidateCheckNull");
                                    this.Warn(string.Format(message,caption));
                                    isValidateSuccessful = false;
                                    this.BindingSource.bChk = true;
                                    //2006/08/01
                                    this.BindingSource.CheckSucess = false;
                                    //2006/08/01
                                }
                            }

                            //if ((fieldItem.CheckRangeFrom != null && fieldItem.CheckRangeFrom != "")
                            //    || (fieldItem.CheckRangeTo != null && fieldItem.CheckRangeTo != ""))
                            //{
                                int belowRangeFlag = 1;
                                int aboveRangeFlag = -1;
                                int rangeExsitFlag = 0;
                                if (fieldItem.CheckRangeFrom != null && fieldItem.CheckRangeFrom != "")
                                {
                                    belowRangeFlag = CompareRange(rowView.Row[fieldItem.FieldName].GetType(), rowView.Row[fieldItem.FieldName], fieldItem.CheckRangeFrom);
                                    rangeExsitFlag |= 1;
                                }
                                if(fieldItem.CheckRangeTo != null && fieldItem.CheckRangeTo != "")
                                {
                                    aboveRangeFlag = CompareRange(rowView.Row[fieldItem.FieldName].GetType(), rowView.Row[fieldItem.FieldName], fieldItem.CheckRangeTo);
                                    rangeExsitFlag |= 2;
                                }
                                if ((int)belowRangeFlag < 0 || (int)aboveRangeFlag > 0)
                                {
                                    string message = "";
                                    if(rangeExsitFlag == 1)
                                    {
                                        string mess = SysMsg.GetSystemMessage(CliUtils.fClientLang,
                                                                             "Srvtools",
                                                                             "DefaultValidate",
                                                                             "msg_DefaultValidateCheckRangeFrom");
                                        message = string.Format(mess, caption, fieldItem.CheckRangeFrom);
                                    
                                    }
                                    else if(rangeExsitFlag == 2)
                                    {
                                        string mess = SysMsg.GetSystemMessage(CliUtils.fClientLang,
                                                                             "Srvtools",
                                                                             "DefaultValidate",
                                                                             "msg_DefaultValidateCheckRangeTo");
                                        message = string.Format(mess, caption, fieldItem.CheckRangeTo);
                                    
                                    }
                                    else
                                    {
                                        string mess = SysMsg.GetSystemMessage(CliUtils.fClientLang,
                                                                             "Srvtools",
                                                                             "DefaultValidate",
                                                                             "msg_DefaultValidateCheckRange");
                                        message = string.Format(mess, caption, fieldItem.CheckRangeFrom, fieldItem.CheckRangeTo);
                                    }
                                    this.Warn(string.Format(message, caption));
                                    isValidateSuccessful = false;
                                    this.BindingSource.bChk = true;
                                    //2006/08/01
                                    this.BindingSource.CheckSucess = false;
                                    //2006/08/01
                                }
                            }
                        //}
                    }
                }
            }
            return isValidateSuccessful;
        }

        internal bool CheckNullAndRange(DataRowView rowView, String columnName)
        {
            bool isValidateSuccessful = true;
            if (!this.DesignMode)
            {
                if (dsDD.Tables.Count == 0)
                {
                    dsDD = DBUtils.GetDataDictionary(this.BindingSource, false);
                }
                if (this.ValidActive)
                {
                    string CaptionNum = "CAPTION";
                    if (this.MultiLanguage)
                    {
                        switch (CliUtils.fClientLang)
                        {
                            case SYS_LANGUAGE.ENG:
                                CaptionNum = "CAPTION1"; break;
                            case SYS_LANGUAGE.TRA:
                                CaptionNum = "CAPTION2"; break;
                            case SYS_LANGUAGE.SIM:
                                CaptionNum = "CAPTION3"; break;
                            case SYS_LANGUAGE.HKG:
                                CaptionNum = "CAPTION4"; break;
                            case SYS_LANGUAGE.JPN:
                                CaptionNum = "CAPTION5"; break;
                            case SYS_LANGUAGE.LAN1:
                                CaptionNum = "CAPTION6"; break;
                            case SYS_LANGUAGE.LAN2:
                                CaptionNum = "CAPTION7"; break;
                            case SYS_LANGUAGE.LAN3:
                                CaptionNum = "CAPTION8"; break;
                        }
                    }
                    if (this.ValidateListBox != null)
                    {
                        this.ValidateListBox.Items.Clear();
                    }
                    foreach (FieldItem fieldItem in this.FieldItems)
                    {
                        string caption = fieldItem.FieldName;
                        if (caption != columnName) continue;

                        if (this.dsDD.Tables.Count > 0)
                        {
                            foreach (DataRow dr in this.dsDD.Tables[0].Rows)
                            {
                                if (string.Compare(dr["FIELD_NAME"].ToString(), fieldItem.FieldName, true) == 0//IgnoreCase
                                    && dr[CaptionNum].ToString() != null && dr[CaptionNum].ToString() != "")
                                {
                                    caption = dr[CaptionNum].ToString();
                                }
                            }
                        }
                        if (fieldItem.FieldName != null && fieldItem.FieldName != "")
                        {
                            if (fieldItem.CheckNull)
                            {
                                if (rowView.Row[fieldItem.FieldName] == null || rowView.Row[fieldItem.FieldName].ToString() == "")
                                {
                                    //language = CliSysMegLag.GetClientLanguage();
                                    String message = SysMsg.GetSystemMessage(CliUtils.fClientLang,
                                                                             "Srvtools",
                                                                             "DefaultValidate",
                                                                             "msg_DefaultValidateCheckNull");
                                    this.Warn(string.Format(message, caption));
                                    isValidateSuccessful = false;
                                    this.BindingSource.bChk = true;
                                    //2006/08/01
                                    this.BindingSource.CheckSucess = false;
                                    //2006/08/01
                                }
                            }

                            //if ((fieldItem.CheckRangeFrom != null && fieldItem.CheckRangeFrom != "")
                            //    || (fieldItem.CheckRangeTo != null && fieldItem.CheckRangeTo != ""))
                            //{
                            int belowRangeFlag = 1;
                            int aboveRangeFlag = -1;
                            int rangeExsitFlag = 0;
                            if (fieldItem.CheckRangeFrom != null && fieldItem.CheckRangeFrom != "")
                            {
                                belowRangeFlag = CompareRange(rowView.Row[fieldItem.FieldName].GetType(), rowView.Row[fieldItem.FieldName], fieldItem.CheckRangeFrom);
                                rangeExsitFlag |= 1;
                            }
                            if (fieldItem.CheckRangeTo != null && fieldItem.CheckRangeTo != "")
                            {
                                aboveRangeFlag = CompareRange(rowView.Row[fieldItem.FieldName].GetType(), rowView.Row[fieldItem.FieldName], fieldItem.CheckRangeTo);
                                rangeExsitFlag |= 2;
                            }
                            if ((int)belowRangeFlag < 0 || (int)aboveRangeFlag > 0)
                            {
                                string message = "";
                                if (rangeExsitFlag == 1)
                                {
                                    string mess = SysMsg.GetSystemMessage(CliUtils.fClientLang,
                                                                         "Srvtools",
                                                                         "DefaultValidate",
                                                                         "msg_DefaultValidateCheckRangeFrom");
                                    message = string.Format(mess, caption, fieldItem.CheckRangeFrom);

                                }
                                else if (rangeExsitFlag == 2)
                                {
                                    string mess = SysMsg.GetSystemMessage(CliUtils.fClientLang,
                                                                         "Srvtools",
                                                                         "DefaultValidate",
                                                                         "msg_DefaultValidateCheckRangeTo");
                                    message = string.Format(mess, caption, fieldItem.CheckRangeTo);

                                }
                                else
                                {
                                    string mess = SysMsg.GetSystemMessage(CliUtils.fClientLang,
                                                                         "Srvtools",
                                                                         "DefaultValidate",
                                                                         "msg_DefaultValidateCheckRange");
                                    message = string.Format(mess, caption, fieldItem.CheckRangeFrom, fieldItem.CheckRangeTo);
                                }
                                this.Warn(string.Format(message, caption));
                                isValidateSuccessful = false;
                                this.BindingSource.bChk = true;
                                //2006/08/01
                                this.BindingSource.CheckSucess = false;
                                //2006/08/01
                            }
                        }
                        //}
                    }
                }
            }
            return isValidateSuccessful;
        }

        private bool ShowWarning = true;
        /// <summary>
        /// Reset flag ShowWaring to true
        /// </summary>
        public void ResetWarnging()
        {
            ShowWarning = true;
        }

        /// <summary>
        /// Warn the message
        /// </summary>
        /// <param name="message">Message of warning</param>
        private void Warn(string message)
        {
            if (ValidateMode == ValidMode.All || ShowWarning)   //When ValidateMode is One, the message is warned only once until ResetWarning
            {
                if (this.ValidateListBox != null)
                {
                    this.ValidateListBox.Items.Add(message);
                }
                else
                {
                    MessageBox.Show(message);
                }
                ShowWarning = false;
            }
        }

        private List<Control> AllCtrls = new List<Control>();
        private void GetAllCtrls(Control.ControlCollection ctrls)
        {
            foreach (Control ctrl in ctrls)
            {
                AllCtrls.Add(ctrl);
                if (ctrl.Controls.Count > 0)
                {
                    GetAllCtrls(ctrl.Controls);
                }
            }
        }

        private string GetValidateValue(string value)
        {
            Char[] cs = value.ToCharArray();
            if (cs.Length == 0)
            {
                return null;
            }

            if (cs[0] != '"' && cs[0] != '\'')
            {
                Char[] sep1 = "()".ToCharArray();
                String[] sps1 = value.Split(sep1);

                if (sps1.Length == 3)
                {
                    return sps1[0];
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        private object GetDefaultType(Type t, string s)
        {
            if (t == typeof(Int16))
                return Convert.ToInt16(s);
            else if (t == typeof(Int32))
                return Convert.ToInt32(s);
            else if (t == typeof(Int64))
                return Convert.ToInt64(s);
            else if (t == typeof(UInt16))
                return Convert.ToUInt16(s);
            else if (t == typeof(UInt32))
                return Convert.ToUInt32(s);
            else if (t == typeof(UInt64))
                return Convert.ToUInt64(s);
            else if (t == typeof(Single))
                return Convert.ToSingle(s);
            else if (t == typeof(decimal))
                return Convert.ToDecimal(s);
            else if (t == typeof(double))
                return Convert.ToDouble(s);
            else if (t == typeof(DateTime))
                return Convert.ToDateTime(s);
            else if (t == typeof(bool))
                return Convert.ToBoolean(s);
            else
                return s;

        }

        private string GetDefaultValue(FieldItem fielditem)
        {
            string defVal = CliUtils.GetValue(fielditem.DefaultValue, this.OwnerComp).ToString();
            string[] defvaldates = new string[]{"_today", "_servertoday", "_firstday", "_lastday", "_firstdaylm"
                , "_lastdaylm", "_firstdayty", "_lastdayty", "_firstdayly", "_lastdayly"};
            foreach (string str in defvaldates)
	        {
                if(string.Compare(str, fielditem.DefaultValue, true) == 0)
                {
                    InfoDataSet ids = this.BindingSource.GetDataSource() as InfoDataSet;
                    Type type = null;
                    if(this.BindingSource.DataSource is InfoDataSet)
                    {
                        type = ids.RealDataSet.Tables[this.BindingSource.DataMember].Columns[fielditem.FieldName].DataType;
                    }
                    else if(this.BindingSource.DataSource is InfoBindingSource)
                    {
                        type = ids.RealDataSet.Relations[this.BindingSource.DataMember].ChildTable.Columns[fielditem.FieldName].DataType;
                    }
                    if (type == typeof(string))
                    {
                        try
                        {
                            defVal = Convert.ToDateTime(defVal).ToString("yyyyMMdd");
                        }
                        catch
                        { 
                        
                        }
                    }
                    break;
                }
	        }
            return defVal;
        }

        [Category("Infolight"),
        Description("Indicates whether Default is enabled or disabled")]
        public bool DefaultActive
        {
            get
            {
                return m_defaultActive;
            }
            set
            {
                m_defaultActive = value;
            }
        }

        [Category("Infolight"),
        Description("Indicates whether Valid is enabled or disabled")]
        public bool ValidActive
        {
            get
            {
                return m_validActive;
            }
            set
            {
                m_validActive = value;
            }
        }

        [Category("Infolight"),
        Description("The columns which DefaultValidate is applied to")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public FieldItemCollection FieldItems
        {
            get
            {
                return m_fieldItems;
            }
            set
            {
                m_fieldItems = value;
            }
        }

        [Category("Infolight"),
        Description("Indicates whether the last update data will be automatically to the specified column")]
        public bool CarryOn
        {
            get
            {
                return m_carryOn;
            }
            set
            {
                m_carryOn = value;
            }
        }

       
        private ListBox _ValidateListBox;
        [Category("Infolight"),
        Description("ListBox to display the validate message")]
        public ListBox ValidateListBox
        {
            get
            {
                return _ValidateListBox;
            }
            set
            {
                _ValidateListBox = value;
            }
        }

       
        private Color _ValidateColor = Color.Red;
        [Category("Infolight"),
        Description("Color of the validate message")]
        public Color ValidateColor
        {
            get
            {
                return _ValidateColor;
            }
            set
            {
                _ValidateColor = value;
            }
        }

        
        private string _ValidateChar;
        [Category("Infolight"),
        Description("Character of validate")]
        public string ValidateChar
        {
            get
            {
                return _ValidateChar;
            }
            set
            {
                _ValidateChar = value;
            }
        }


        private bool _DuplicateCheck;
        [Category("Infolight"),
        Description("Indicate whether data need to check if it has exsit in dataset or database")]
        public bool DuplicateCheck
        {
            get
            {
                return _DuplicateCheck;
            }
            set
            {
                _DuplicateCheck = value;
            }
        }
        private bool _CheckKeyFieldEmpty;
        [Category("Infolight"),
        Description("Indicate whether data need to check if it has exsit in dataset or database")]
        public bool CheckKeyFieldEmpty
        {
            get
            {
                return _CheckKeyFieldEmpty;
            }
            set
            {
                _CheckKeyFieldEmpty = value;
            }
        }

        private ValidMode _ValidateMode;
        [Category("Infolight"),
        Description("Mode of Warn message")]
        public ValidMode ValidateMode
        {
            get { return _ValidateMode; }
            set { _ValidateMode = value; }
        }

        private bool _LeaveValidation;
        [Category("Infolight")]
        public bool LeaveValidation
        {
            get { return _LeaveValidation; }
            set { _LeaveValidation = value; }
        }

        private bool _MultiLanguage;
        [Category("Infolight")]
        public bool MultiLanguage
        {
            get
            {
                return _MultiLanguage;
            }
            set
            {
                _MultiLanguage = value;
            }
        }

        public enum DupCheckMode
        {
            ByLocal = 0,
            ByWhere = 1
        }

        public enum ValidMode
        { 
            All,
            One
        }

        private DupCheckMode _DuplicateCheckMode;
        [Category("Infolight"),
        Description("Mode of DuplicateCheck")]
        public DupCheckMode DuplicateCheckMode
        {
            get
            {
                return _DuplicateCheckMode;
            }
            set
            {
                _DuplicateCheckMode = value;
            }
        }
        #endregion Properties

        #region Events
        public event ValidateEventHandler Validate
        {
            add
            {
                Events.AddHandler(EventValidate, value);
            }
            remove
            {
                Events.RemoveHandler(EventValidate, value);
            }
        }

        protected void OnValidate(ValidateEventArgs e)
        {
            ValidateEventHandler handler = Events[EventValidate] as ValidateEventHandler;

            if (handler != null && e is ValidateEventArgs)
            {
                handler(this, e);
            }
        }


        public event ShowMessageEventHandler ShowMessage
        {
            add
            {
                Events.AddHandler(EventShowMessage, value);
            }
            remove
            {
                Events.RemoveHandler(EventShowMessage, value);
            }
        }

        protected void OnShowMessage(ShowMessageEventArgs e)
        {
            ShowMessageEventHandler handler = Events[EventShowMessage] as ShowMessageEventHandler;

            if (handler != null && e is ShowMessageEventArgs)
            {
                handler(this, e);
            }
        }
        #endregion Events
    }
    #endregion DefaultValidate

    #region FieldItemCollection
    public class FieldItemCollection : InfoOwnerCollection
    {
        public FieldItemCollection(Component owner)
            : base(owner, typeof(FieldItem))
        {
        }

        new public FieldItem this[int index]
        {
            get
            {
                return InnerList[index] as FieldItem;
            }
            set
            {
                if (index >= 0 && index < Count)
                {
                    if (value is FieldItem)
                    {
                        (InnerList[index] as FieldItem).Collection = null;
                        InnerList[index] = value;
                        (InnerList[index] as FieldItem).Collection = this;
                    }
                }
            }
        }
    }

    public class FieldItem : InfoOwnerCollectionItem, IGetValues
    {
        #region Private Fields
        private string m_fieldName;
        private string m_defaultValue;
        private bool m_checkNull;
        private string _CheckRangeFrom = "";
        private string _CheckRangeTo = "";

        private string m_validate;
        private string m_warningMsg;
        private bool m_carryOn = false;
        private string _ValidateLabelLink;
        #endregion Private Fields

        public FieldItem()
        {
            m_fieldName = "";
            m_defaultValue = "";
            m_checkNull = false;
            m_validate = "";
            m_warningMsg = "";
            m_carryOn = false;

            _CheckRangeFrom = "";
            _CheckRangeTo = "";
            _ValidateLabelLink = "";
        }

        #region Properties
        [Category("Check Validate")]
        public string CheckRangeFrom
        {
            get
            {
                return _CheckRangeFrom;
            }
            set
            {
                _CheckRangeFrom = value;
            }
        }

        [Category("Check Validate")]
        public string CheckRangeTo
        {
            get
            {
                return _CheckRangeTo;
            }
            set
            {
                _CheckRangeTo = value;
            }
        }

        [Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
        public string FieldName
        {
            get
            {
                return m_fieldName;
            }
            set
            {
                m_fieldName = value;
            }
        }

        [Category("Value")]
        public string DefaultValue
        {
            get
            {
                return m_defaultValue;
            }
            set
            {
                m_defaultValue = value;
            }
        }

        [Category("Value")]
        public bool CarryOn
        {
            get
            {
                return m_carryOn;
            }
            set
            {
                m_carryOn = value;
            }
        }

        [Category("Check Validate")]
        public bool CheckNull
        {
            get
            {
                return m_checkNull;
            }
            set
            {
                m_checkNull = value;
            }
        }

        [Category("Check Validate")]
        public string Validate
        {
            get
            {
                return m_validate;
            }
            set
            {
                m_validate = value;
            }
        }

        [Category("Message")]
        public string WarningMsg
        {
            get
            {
                return m_warningMsg;
            }
            set
            {
                m_warningMsg = value;
            }
        }

        [Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
        public string ValidateLabelLink
        {
            get
            {
                return _ValidateLabelLink;
            }
            set
            {
                _ValidateLabelLink = value;
                if (_ValidateLabelLink != null && _ValidateLabelLink != "" && _ValidateLabelLink.IndexOf(" (") != -1)
                {
                    _ValidateLabelLink = _ValidateLabelLink.Substring(0, _ValidateLabelLink.IndexOf(" ("));
                }
            }
        }

        public override string Name
        {
            get
            {
                return this.FieldName;
            }
            set
            {
                this.FieldName = value;
            }
        }

        public string[] GetValues(string sKind)
        {
            string[] retList = null;
            List<string> values = new List<string>();
            if (string.Compare(sKind, "fieldname", true) == 0)//IgnoreCase
            {
                if (this.Owner is DefaultValidate)
                {
                    DefaultValidate defaultValidate = this.Owner as DefaultValidate;
                    if (defaultValidate != null && defaultValidate.BindingSource != null)
                    {
                        InfoBindingSource bindingSource = defaultValidate.BindingSource;
                        DataView dataView = bindingSource.List as DataView;
                        if (dataView != null)
                        {
                            foreach (DataColumn column in dataView.Table.Columns)
                            {
                                values.Add(column.ColumnName);
                            }
                        }
                        //add by Rax
                        else
                        {
                            int iRelationPos = -1;
                            DataSet ds = ((InfoDataSet)defaultValidate.BindingSource.GetDataSource()).RealDataSet;
                            for (int i = 0; i < ds.Relations.Count; i++)
                            {
                                if (defaultValidate.BindingSource.DataMember == ds.Relations[i].RelationName)
                                {
                                    iRelationPos = i;
                                    break;
                                }
                            }
                            if (iRelationPos != -1)
                            {
                                foreach (DataColumn column in ds.Relations[iRelationPos].ChildTable.Columns)
                                {
                                    values.Add(column.ColumnName);
                                }
                            }
                        }
                        // end add
                    }
                }
            }
            else if (string.Compare(sKind, "validatelabellink", true) == 0)//IgnoreCase
            {
                if (this.Owner is DefaultValidate)
                {
                    DefaultValidate defaultValidate = (DefaultValidate)this.Owner;
                    if (defaultValidate != null)
                    {
                        foreach (IComponent comp in defaultValidate.Container.Components)
                        {
                            if (comp is Label)
                            {
                                values.Add(((Label)comp).Name + " (" + ((Label)comp).Text + ")");
                            }
                        }
                    }
                }
            }
            else if (string.Compare(sKind, "validatecontrollink", true) == 0)//IgnoreCase
            {
                if (this.Owner is DefaultValidate)
                {
                    DefaultValidate defaultValidate = (DefaultValidate)this.Owner;
                    if (defaultValidate != null)
                    {
                        foreach (IComponent comp in defaultValidate.Container.Components)
                        {
                            if (comp is InfoRefvalBox)
                            {
                                values.Add(((InfoRefvalBox)comp).Name);
                            }
                            else if (comp is TextBox)
                            {
                                values.Add(((TextBox)comp).Name);
                            }
                            else if (comp is DateTimePicker)
                            {
                                values.Add(((DateTimePicker)comp).Name);
                            }
                            else if (comp is ComboBox)
                            {
                                values.Add(((ComboBox)comp).Name);
                            }
                        }
                    }
                }
            }
            values.Sort();

            if (values.Count > 0)
            {
                int i = values.Count;
                retList = new string[i];
                for (int j = 0; j < i; j++)
                {
                    retList[j] = values[j];
                }
            }
            return retList;
        }

        #endregion

        /*private class WinFieldNameEditor : UITypeEditor
        {
            public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
            {
                return UITypeEditorEditStyle.DropDown;
            }

            public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
            {
                IWindowsFormsEditorService EditorService = null;
                if (provider != null)
                {
                    EditorService =
                        provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
                }

                if (EditorService != null)
                {
                    ListBox ColumnList = new ListBox();
                    ColumnList.SelectionMode = SelectionMode.One;
                    ColumnList.Items.Add("( None )");
                    FieldItem fieldItem = context.Instance as FieldItem;
                    if (fieldItem != null && fieldItem.Owner != null)
                    {
                        DefaultValidate defaultValidate = fieldItem.Owner as DefaultValidate;
                        if (defaultValidate != null && defaultValidate.BindingSource != null)
                        {
                            InfoBindingSource bindingSource = defaultValidate.BindingSource;
                            DataView dataView = bindingSource.List as DataView;
                            if (dataView != null)
                            {
                                foreach (DataColumn column in dataView.Table.Columns)
                                {
                                    ColumnList.Items.Add(column.ColumnName);
                                }
                            }
                            //add by Rax
                            else
                            {
                                int iRelationPos = -1;
                                DataSet ds = ((InfoDataSet)defaultValidate.BindingSource.GetDataSource()).RealDataSet;
                                for (int i = 0; i < ds.Relations.Count; i++)
                                {
                                    if (defaultValidate.BindingSource.DataMember == ds.Relations[i].RelationName)
                                    {
                                        iRelationPos = i;
                                        break;
                                    }
                                }
                                if (iRelationPos != -1)
                                {
                                    foreach (DataColumn column in ds.Relations[iRelationPos].ChildTable.Columns)
                                    {
                                        ColumnList.Items.Add(column.ColumnName);
                                    }
                                }
                            }
                            // end add
                        }

                        ColumnList.SelectedIndexChanged += delegate(object sender, EventArgs e)
                        {
                            int index = ColumnList.SelectedIndex;
                            if (index != -1)
                            {
                                if (index == 0)
                                {
                                    value = "";
                                }
                                else
                                {
                                    value = ColumnList.Items[index].ToString();
                                }
                            }
                            EditorService.CloseDropDown();
                        };

                        EditorService.DropDownControl(ColumnList);
                    }
                }

                return value;
            }
        }*/
    }
    #endregion FieldItemCollection
}
