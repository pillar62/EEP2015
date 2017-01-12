using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Data;
using System.Windows.Forms;
using System.Drawing.Design;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Reflection;

namespace Srvtools
{
    #region InfoDataGridViewRefValColumn
    public class InfoDataGridViewRefValColumn : DataGridViewTextBoxColumn
    {
        public InfoDataGridViewRefValColumn()
            : base()
        {
            base.CellTemplate = new InfoDataGridViewRefValCell();
            
        }

        [Category("Appearance")]
        public DataGridViewCellStyle HeaderCellStyle
        {
            get { return this.HeaderCell.Style; }
            set { this.HeaderCell.Style = value; }
        }
	

        [Category("Data")]
        public InfoRefVal RefValue
        {
            get
            {
                return (this.CellTemplate as InfoDataGridViewRefValCell).RefValue;
            }
            set
            {
                InfoDataGridViewRefValCell template = this.CellTemplate as InfoDataGridViewRefValCell;
                if (template != null)
                {
                    template.RefValue = value;
                    if (null != template.RefValue)
                    {
                        template.RefValue.SetToDgv(this.DataGridView);
                    }
                }
                if (base.DataGridView != null)
                {
                    int rowCount = base.DataGridView.Rows.Count;
                    DataGridViewRowCollection rowCollection = base.DataGridView.Rows;
                    for (int index = 0; index < rowCount; ++index)
                    {
                        DataGridViewRow row = rowCollection.SharedRow(index);
                        InfoDataGridViewRefValCell cell =
                            row.Cells[base.Index] as InfoDataGridViewRefValCell;
                        if (cell != null)
                        {
                            cell.RefValue = value;
                        }
                    }
                }
            }
        }

        [Category("Data")]
        [Editor(typeof(ExternalRefvalEditor), typeof(UITypeEditor))] 
        public string ExternalRefVal
        {
            get { return (this.CellTemplate as InfoDataGridViewRefValCell).ExternalRefVal; }
            set
            {
                InfoDataGridViewRefValCell template = this.CellTemplate as InfoDataGridViewRefValCell;
                if (template != null)
                {
                    template.ExternalRefVal = value;
                }
                if (base.DataGridView != null)
                {
                    int rowCount = base.DataGridView.Rows.Count;
                    DataGridViewRowCollection rowCollection = base.DataGridView.Rows;
                    for (int index = 0; index < rowCount; ++index)
                    {
                        DataGridViewRow row = rowCollection.SharedRow(index);
                        InfoDataGridViewRefValCell cell =
                            row.Cells[base.Index] as InfoDataGridViewRefValCell;
                        if (cell != null)
                        {
                            cell.ExternalRefVal = value;
                        }
                    }
                }
            }
        }

        [Category("Data")]
        [Browsable(false)]
        [AttributeProvider(typeof(IListSource))]
        [RefreshProperties(RefreshProperties.All)]
        public object DataSource
        {
            get
            {
                return (this.CellTemplate as InfoDataGridViewRefValCell).DataSource;
            }
            set
            {
                InfoDataGridViewRefValCell template = this.CellTemplate as InfoDataGridViewRefValCell;
                if (template != null)
                {
                    template.DataSource = value;
                }

                if (base.DataGridView != null)
                {
                    int rowCount = base.DataGridView.Rows.Count;
                    DataGridViewRowCollection rowCollection = base.DataGridView.Rows;
                    for (int index = 0; index < rowCount; ++index)
                    {
                        DataGridViewRow row = rowCollection.SharedRow(index);
                        InfoDataGridViewRefValCell cell =
                            row.Cells[base.Index] as InfoDataGridViewRefValCell;
                        if (cell != null)
                        {
                            cell.DataSource = value;
                        }
                    }
                }
            }
        }
	
        [Category("Data")]
        [Browsable(false)]
        [Editor("System.Windows.Forms.Design.DataMemberFieldEditor", typeof(UITypeEditor))]
        public string DisplayMember
        {
            get
            {
                return (this.CellTemplate as InfoDataGridViewRefValCell).DisplayMember;
            }
            set
            {
                InfoDataGridViewRefValCell template = this.CellTemplate as InfoDataGridViewRefValCell;
                if (template != null)
                {
                    template.DisplayMember = value;
                }

                if (base.DataGridView != null)
                {
                    int rowCount = base.DataGridView.Rows.Count;
                    DataGridViewRowCollection rowCollection = base.DataGridView.Rows;
                    for (int index = 0; index < rowCount; ++index)
                    {
                        DataGridViewRow row = rowCollection.SharedRow(index);
                        InfoDataGridViewRefValCell cell =
                            row.Cells[base.Index] as InfoDataGridViewRefValCell;
                        if (cell != null)
                        {
                            cell.DisplayMember = value;
                        }
                    }
                }
            }
        }

        [Category("Data")]
        [Browsable(false)]
        [Editor("System.Windows.Forms.Design.DataMemberFieldEditor", typeof(UITypeEditor))]
        public string ValueMember
        {
            get
            {
                return (this.CellTemplate as InfoDataGridViewRefValCell).ValueMember;
            }
            set
            {
                InfoDataGridViewRefValCell template = this.CellTemplate as InfoDataGridViewRefValCell;
                if (template != null)
                {
                    template.ValueMember = value;
                }

                if (base.DataGridView != null)
                {
                    int rowCount = base.DataGridView.Rows.Count;
                    DataGridViewRowCollection rowCollection = base.DataGridView.Rows;
                    for (int index = 0; index < rowCount; ++index)
                    {
                        DataGridViewRow row = rowCollection.SharedRow(index);
                        InfoDataGridViewRefValCell cell =
                            row.Cells[base.Index] as InfoDataGridViewRefValCell;
                        if (cell != null)
                        {
                            cell.ValueMember = value;
                        }
                    }
                }
            }
        }

        public override DataGridViewCell CellTemplate
        {
            get
            {
                return base.CellTemplate;
            }
            set
            {
                if (value == null || value is InfoDataGridViewRefValCell)
                {
                    base.CellTemplate = value;
                }
                else
                {
                    throw new Exception("Must be a DataGridViewInfoRefValCell");
                }
            }
        }

        #region InfoDataGridViewRefValCell
        public class InfoDataGridViewRefValCell : DataGridViewTextBoxCell
        {
            public InfoDataGridViewRefValCell()
                : base()
            {
            }

            private Hashtable cachetable = new Hashtable();// add for promote efficiency of paint
            protected override void Paint(System.Drawing.Graphics graphics,
                                          System.Drawing.Rectangle clipBounds,
                                          System.Drawing.Rectangle cellBounds,
                                          int rowIndex,
                                          DataGridViewElementStates cellState,
                                          object value,
                                          object formattedValue,
                                          string errorText,
                                          DataGridViewCellStyle cellStyle,
                                          DataGridViewAdvancedBorderStyle advancedBorderStyle,
                                          DataGridViewPaintParts paintParts)
            {
                int rowCnt = -1;
                if (this.DataGridView.AllowUserToAddRows)
                    rowCnt = this.DataGridView.Rows.Count - 1;
                else
                    rowCnt = this.DataGridView.Rows.Count;

                if (rowIndex < rowCnt)
                {
                    if (this.DataGridView.Site == null && value != null)
                    {
                        if (!this.RefValue.WhereItemCache || !cachetable.Contains(value))
                        {
                            object[] obj = this.RefValue.CheckValid_And_ReturnDisplayValue(value, false, false);
                            if (obj[1] != null)
                            {
                                formattedValue = (string)obj[1];
                                if (this.RefValue.WhereItemCache)
                                {
                                    cachetable.Add(value, formattedValue);
                                }
                            }
                        }
                        else
                        {
                            formattedValue = (string)cachetable[value];
                        }
                    }
                }
                base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);
                //Rectangle rect = cellBounds;
                //Point buttonLocation = new Point();
                //buttonLocation.Y = cellBounds.Y;
                //buttonLocation.X = cellBounds.X;
                //Pen pen1 = new Pen(Brushes.White, 1);
                //Pen pen2 = new Pen(Brushes.Black, 1);
                //Pen pen3 = new Pen(Brushes.DimGray, 1);
                //int i = this.DataGridView[this.ColumnIndex, rowIndex].Size.Width;

                //SolidBrush myBrush = new SolidBrush(SystemColors.Control);
                //graphics.FillRectangle(myBrush, buttonLocation.X + i - 21, buttonLocation.Y + 3, 18, 18);
                //graphics.DrawLine(pen1, buttonLocation.X + i - 21, buttonLocation.Y + 3, buttonLocation.X + i - 3, buttonLocation.Y + 3); //top
                //graphics.DrawLine(pen1, buttonLocation.X + i - 21, buttonLocation.Y + 3, buttonLocation.X + i - 21, buttonLocation.Y + 21); // left
                //graphics.DrawLine(pen2, buttonLocation.X + i - 3, buttonLocation.Y + 21, buttonLocation.X + i - 21, buttonLocation.Y + 21); // Bottom
                //graphics.DrawLine(pen3, buttonLocation.X + i - 4, buttonLocation.Y + 20, buttonLocation.X + i - 20, buttonLocation.Y + 20);
                //graphics.DrawLine(pen2, buttonLocation.X + i - 3, buttonLocation.Y + 20, buttonLocation.X + i - 3, buttonLocation.Y + 3); // right
                //graphics.DrawLine(pen3, buttonLocation.X + i - 4, buttonLocation.Y + 21, buttonLocation.X + i - 4, buttonLocation.Y + 4);
                //StringFormat sf = new StringFormat();
                //sf.Alignment = StringAlignment.Center;
                //sf.LineAlignment = StringAlignment.Center;
                //graphics.DrawString("...", new Font("SimSun", 9), Brushes.Black, buttonLocation.X + i - 10, buttonLocation.Y + 10, sf);

            }

            protected override Object GetFormattedValue(Object value, int rowIndex,
               ref DataGridViewCellStyle cellStyle, TypeConverter valueTypeConverter,
               TypeConverter formattedValueTypeConverter,
               DataGridViewDataErrorContexts context)
            {
                //modify by ccm 解决新增时没有显示displaymember的问题
                //int rowCnt = -1;
                //if (this.DataGridView.AllowUserToAddRows)
                //    rowCnt = this.DataGridView.Rows.Count - 1;
                //else
                //    rowCnt = this.DataGridView.Rows.Count;

                //if (rowIndex < rowCnt)
                //{
                if(value != null)
                {
                    if (this.DataGridView.Site == null && value != null)
                    {
                        if (!this.RefValue.WhereItemCache || !cachetable.Contains(value))
                        {
                            object[] obj = this.RefValue.CheckValid_And_ReturnDisplayValue(value, false, false);
                            if ((bool)obj[0] == false)
                            {
                                RaiseDataError(new DataGridViewDataErrorEventArgs(new Exception("GET"), ColumnIndex,
                                    rowIndex, context));
                                return null;
                            }
                            if (obj[1] != null)
                            {
                                if (this.RefValue.WhereItemCache)
                                {
                                    cachetable.Add(value, obj[1]);
                                }
                                return (string)obj[1];
                            }
                        }
                        else
                        {
                            return (string)cachetable[value];
                        }
                    }
                }
                return null;
            }

            //bool bShowRefance = false;
            protected override void OnMouseDown(DataGridViewCellMouseEventArgs e)
            {
                base.OnMouseDown(e);
                try
                {
                    int x = this.Size.Width - 20;
                    if (this.Selected)
                    {
                        if (e.X <= this.Size.Width && e.X >= x)
                        {
                            ((InfoDataGridView)this.DataGridView).EnterRefValControl = true;
                            //bShowRefance = true;
                        }
                    }
                }
                catch
                { }
            }

            public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
            {
                base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);
                InfoDataGridViewRefValCellEditingControl ctrl = this.DataGridView.EditingControl as InfoDataGridViewRefValCellEditingControl;
                ctrl.MaxLength = this.MaxInputLength;
            }

            private InfoRefVal m_refValue = null;
            public InfoRefVal RefValue
            {
                get
                {
                    if (m_refValue == null)
                    {
                        if (this.DataGridView != null)
                        {
                            if (!string.IsNullOrEmpty(ExternalRefVal))
                            {
                                int index = ExternalRefVal.LastIndexOf('.');
                                if (index > 0 && index < ExternalRefVal.Length - 1)
                                {
                                    string formname = ExternalRefVal.Substring(0, index);
                                    string refvalname = ExternalRefVal.Substring(index + 1);
                                    Form form = this.DataGridView.FindForm();
                                    Form mdi = null;
                                    if (form.MdiParent != null)
                                    {
                                        mdi = form.MdiParent;
                                    }
                                    else
                                    {
                                        foreach (Form frm in Application.OpenForms)
                                        {
                                            if (frm.IsMdiContainer)
                                            {
                                                mdi = frm;
                                                break;
                                            }
                                        }
                                    }
                                    if (mdi != null)
                                    {
                                        foreach (Form frm in mdi.MdiChildren)
                                        {
                                            if (string.Compare(frm.GetType().FullName, formname) == 0)
                                            {
                                                FieldInfo field = frm.GetType().GetField(refvalname, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                                                if (field != null)
                                                {
                                                    object obj = field.GetValue(frm);
                                                    if (obj is InfoRefVal)
                                                    {
                                                        m_refValue = obj as InfoRefVal;
                                                    }
                                                }
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    return m_refValue;
                }
                set
                {
                    m_refValue = value;
                }
            }

            private string externalRefVal;
            public string ExternalRefVal
            {
                get { return externalRefVal; }
                set { externalRefVal = value; }
            }

            private string _DisplayMember;
            public string DisplayMember
            {
                get
                {
                    if (this.RefValue == null)
                    {
                        return _DisplayMember;
                    }
                    else
                    {
                        return this.RefValue.DisplayMember;
                    }
                }
                set
                {
                    _DisplayMember = value;
                }
            }

            private string _ValueMember;
            public string ValueMember
            {
                get
                {
                    if (this.RefValue == null)
                    {
                        return _ValueMember;
                    }
                    else
                    {
                        return this.RefValue.ValueMember;
                    }
                }
                set
                {
                    _ValueMember = value;
                }
            }

            private object _DataSource;
            public object DataSource
            {
                get
                {
                    if (this.RefValue == null)
                    {
                        return _DataSource;
                    }
                    else
                    {
                        return this.RefValue.GetDataSource();
                    }
                }
                set
                {
                    _DataSource = value;
                }
            }

            public override Type EditType
            {
                get
                {
                    return typeof(InfoDataGridViewRefValCellEditingControl);
                }
            }

            public override object Clone()
            {
                InfoDataGridViewRefValCell cell = base.Clone() as InfoDataGridViewRefValCell;
                cell.m_refValue = this.m_refValue;
                cell.externalRefVal = this.externalRefVal;
                return cell;
            }
        }

        #endregion

        #region InfoDataGridViewRefValCellEditingControl
        [ToolboxItem(false)]
        public class InfoDataGridViewRefValCellEditingControl : InfoRefValForGrid, IDataGridViewEditingControl
        {
            private DataGridView dataGridView;
            private bool valueChanged = false;
            private int rowIndex;

            public InfoDataGridViewRefValCellEditingControl()
                : base()
            {
                InnerTextBox.Validating += new System.ComponentModel.CancelEventHandler(InnerTextBox_Validating);
            }
             
            public void InnerTextBox_Validating(Object sender, CancelEventArgs e)
            {
                String sText = InnerTextBox.Text;
                RefVal.FLookupValue = sText;
                object[] obj = this.RefVal.CheckValid_And_ReturnDisplayValue(ref sText, false, false);
                if ((bool)obj[0] == false && this.RefVal.CheckData)
                {
                    InnerTextBox.Focus();
                    e.Cancel = true;
                }
            }
            private InfoDataGridViewRefValCell cell = null;

            protected override void OnEnter(EventArgs e)
            {
                cell = (InfoDataGridViewRefValCell)this.EditingControlDataGridView.CurrentCell;
                this.RefVal = this.cell.RefValue;
    
                this.RefVal.ActiveColumn = this.dataGridView.Columns[cell.ColumnIndex] as InfoDataGridViewRefValColumn;
                this.RefVal.ActiveBox = null;
                if (cell.Value != null)
                {
                    this.InnerTextBox.Text = cell.Value.ToString();
                }

                if (((InfoDataGridView)this.EditingControlDataGridView).EnterRefValControl)
                {
                    this.InnerButton.PerformClick();
                    ((InfoDataGridView)this.EditingControlDataGridView).EnterRefValControl = false;
                }
            }

            protected override void OnLeave(EventArgs e)
            {
                string sText = this.InnerTextBox.Text;
                object[] obj = this.RefVal.CheckValid_And_ReturnDisplayValue(ref sText, this.RefVal.CheckData, true);
                //modified by lily 2007/6/6 原來的程式只能是找到displaymember才行，否則無法改成其他值，目前改成
                //如果CheckData=true則必須找到才改，如果CheckData=false，則全部都改
                if ((this.RefVal.CheckData && (bool)obj[0]) || (!this.RefVal.CheckData))
                {
                    if (sText.Length > 0)
                    {
                        cell.Value = sText;
                        this.Text = sText;
                    }
                    else
                    {
                        //点进去再离开时不应该写值,避免有autoseq时这行会存入datasource
                        if (this.dataGridView.NewRowIndex != this.rowIndex)
                        {
                            cell.Value = null;
                            this.Text = string.Empty;
                        }
                    }
                }
                //由obj[1].ToString改爲判斷sText，只要輸入的值不是""就應該作ColumnMatch，否則如果display爲空白時不會ColumnMatch
                //if (sText != "")
                //{

                //}
                base.OnLeave(e);
                //for 用tab離開時第一次會報錯
                //this.dataGridView.BeginEdit(false);
                if (this.dataGridView.NewRowIndex == this.rowIndex && this.InnerTextBox.Text != null && this.InnerTextBox.Text != "")
                {
                    this.dataGridView.NotifyCurrentCellDirty(true);
                }

                if ((bool)obj[0])
                {
                    //cell.Value = sText;
                    if (this.RefVal.columnMatch.Count > 0 && cell.Value != null)
                    {
                        //if ((this.RefVal.AlwaysClose) && (((InfoBindingSource)this.RefVal.InnerBs).Count == 0))//??????
                        //{
                        //    this.RefVal.InnerDs.RemoteName = "GLModule.cmdRefValUse";
                        //    this.RefVal.InnerDs.Execute(this.RefVal.SelectCommand, true);
                        //}
                        this.RefVal.DoColumnMatch(cell.Value.ToString(), (DataRow)obj[2]);

                        foreach (DataGridViewColumn column in this.dataGridView.Columns)
                        {
                            if (column is InfoDataGridViewExpressionColumn
                                && ((InfoDataGridViewExpressionColumn)column).EffectColumnNames != null
                                && ((InfoDataGridViewExpressionColumn)column).EffectColumnNames != "")
                            {
                                List<string> effectCols = ((InfoDataGridViewExpressionColumn)column).GetEffectColumnList();
                                foreach (string columnName in effectCols)
                                {
                                    if (this.dataGridView.CurrentCell.OwningColumn.DataPropertyName != null
                                        && this.dataGridView.CurrentCell.OwningColumn.DataPropertyName != ""
                                        && string.Compare(columnName, this.dataGridView.CurrentCell.OwningColumn.DataPropertyName, true) == 0)//IgnoreCase
                                    {
                                        ((DataRowView)((InfoBindingSource)this.dataGridView.DataSource).Current).EndEdit();
                                        object objv = ((InfoDataGridViewExpressionColumn)column).GetExpression(this.cell.RowIndex);
                                        this.dataGridView.CurrentRow.Cells[column.Index].Value = objv;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            // Implements the IDataGridViewEditingControl.EditingControlFormattedValue property.
            public object EditingControlFormattedValue
            {
                get
                {
                    return this.Text;
                }
                set
                {
                    String newValue = value as String;
                    if (newValue != null)
                    {
                       this.Text = newValue;
                    }
                }
            }

            // Implements the IDataGridViewEditingControl.GetEditingControlFormattedValue method.
            public object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context)
            {
                return EditingControlFormattedValue;
            }

            // Implements the IDataGridViewEditingControl.ApplyCellStyleToEditingControl method.
            public void ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle)
            {
                this.Font = dataGridViewCellStyle.Font;
                this.InnerButton.BackColor = SystemColors.Control;
            }

            // Implements the IDataGridViewEditingControl.EditingControlRowIndex property.
            public int EditingControlRowIndex
            {
                get
                {
                    return cell.RowIndex;
                }
                set
                {
                    rowIndex = value;
                }
            }

            // Implements the IDataGridViewEditingControl.EditingControlWantsInputKey method.
            public bool EditingControlWantsInputKey(Keys key, bool dataGridViewWantsInputKey)
            {
                if (key != Keys.Escape && key != Keys.Up && key != Keys.Down)
                {
                    //modified by lily 2007/9/21 有些符号无法输入
                    return true;
                }
                else
                {
                    return false;
                }
                //switch (key & Keys.KeyCode)
                //{
                //    case Keys.Left:
                //    case Keys.Up:
                //    case Keys.Down:
                //    case Keys.Right:
                //    case Keys.Home:
                //    case Keys.End:
                //    case Keys.PageDown:
                //    case Keys.PageUp:
                //        return true;
                //    default:
                //        return false;
                //}
            }

            // Implements the IDataGridViewEditingControl.PrepareEditingControlForEdit method.
            public void PrepareEditingControlForEdit(bool selectAll)
            {
                // No preparation needs to be done.
            }

            // Implements the IDataGridViewEditingControl RepositionEditingControlOnValueChange property.
            public bool RepositionEditingControlOnValueChange
            {
                get
                {
                    return true;
                }
            }

            // Implements the IDataGridViewEditingControl.EditingControlDataGridView property.
            public DataGridView EditingControlDataGridView
            {
                get
                {
                    return dataGridView;
                }
                set
                {
                    dataGridView = value;
                }
            }

            // Implements the IDataGridViewEditingControl.EditingControlValueChanged property.
            public bool EditingControlValueChanged
            {
                get
                {
                    return valueChanged;
                }
                set
                {
                    valueChanged = value;
                }
            }

            // Implements the IDataGridViewEditingControl.EditingPanelCursor property.
            public Cursor EditingPanelCursor
            {
                get
                {
                    return base.Cursor;
                }
            }
        }
        #endregion
    }
    #endregion
}