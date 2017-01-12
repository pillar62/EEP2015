using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Data;
using System.Globalization;

namespace Srvtools
{
    public class InfoDataGridViewDateTimeBoxColumn : DataGridViewTextBoxColumn
    {
        public InfoDataGridViewDateTimeBoxColumn()
            : base()
        {
            base.CellTemplate = new InfoDataGridViewDateTimeBoxCell();
        }

        [Category("Appearance")]
        public DataGridViewCellStyle HeaderCellStyle
        {
            get { return this.HeaderCell.Style; }
            set { this.HeaderCell.Style = value; }
        }

        public override DataGridViewCell CellTemplate
        {
            get
            {
                return base.CellTemplate;
            }
            set
            {
                if (value == null || value is InfoDataGridViewDateTimeBoxCell)
                {
                    base.CellTemplate = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets datetime is displayed as a roc format
        /// </summary>
        public bool RocYear
        {
            get 
            {
                return (this.CellTemplate as InfoDataGridViewDateTimeBoxCell).RocYear;
            }
            set
            {
                InfoDataGridViewDateTimeBoxCell template = this.CellTemplate as InfoDataGridViewDateTimeBoxCell;
                if (template != null)
                {
                    template.RocYear = value;
                }
                if (DataGridView != null)
                {
                    for (int i = 0; i < DataGridView.Rows.Count; i++)
                    {
                        DataGridViewRow row = DataGridView.Rows.SharedRow(i);
                        InfoDataGridViewDateTimeBoxCell cell = row.Cells[base.Index] as InfoDataGridViewDateTimeBoxCell;
                        if (cell != null)
                        {
                            cell.RocYear = value;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the format of datetime
        /// </summary>
        public string Format
        {
            get
            {
                return (this.CellTemplate as InfoDataGridViewDateTimeBoxCell).Format;
            }
            set
            {
                InfoDataGridViewDateTimeBoxCell template = this.CellTemplate as InfoDataGridViewDateTimeBoxCell;
                if (template != null)
                {
                    template.Format = value;
                }
                if (DataGridView != null)
                {
                    for (int i = 0; i < DataGridView.Rows.Count; i++)
                    {
                        DataGridViewRow row = DataGridView.Rows.SharedRow(i);
                        InfoDataGridViewDateTimeBoxCell cell = row.Cells[base.Index] as InfoDataGridViewDateTimeBoxCell;
                        if (cell != null)
                        {
                            cell.Format = value;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the button is visible
        /// </summary>
        public bool ShowPicker
        {
            get
            {
                return (this.CellTemplate as InfoDataGridViewDateTimeBoxCell).ShowPicker;
            }
            set
            {
                InfoDataGridViewDateTimeBoxCell template = this.CellTemplate as InfoDataGridViewDateTimeBoxCell;
                if (template != null)
                {
                    template.ShowPicker = value;
                }
                if (DataGridView != null)
                {
                    for (int i = 0; i < DataGridView.Rows.Count; i++)
                    {
                        DataGridViewRow row = DataGridView.Rows.SharedRow(i);
                        InfoDataGridViewDateTimeBoxCell cell = row.Cells[base.Index] as InfoDataGridViewDateTimeBoxCell;
                        if (cell != null)
                        {
                            cell.ShowPicker = value;
                        }
                    }
                }
            }
        }

    }

    public class InfoDataGridViewDateTimeBoxCell : DataGridViewTextBoxCell
    {

        private bool rocYear;
        /// <summary>
        /// Gets or sets datetime is displayed as a roc format
        /// </summary>
        public bool RocYear
        {
            get { return rocYear; }
            set { rocYear = value; }
        }

        private string format = "yyyy/MM/dd";
        /// <summary>
        /// Gets or sets the format of datetime
        /// </summary>
        public string Format
        {
            get { return format; }
            set { format = value; }
        }

        private bool showPicker = true;
        /// <summary>
        /// Gets or sets the button is visible
        /// </summary>
        public bool ShowPicker
        {
            get { return showPicker; }
            set { showPicker = value; }
        }

        public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);
            if (this.DataGridView.EditingControl != null)
            {
                InfoDataGridViewDateTimeBoxCellEditingControl control = this.DataGridView.EditingControl as InfoDataGridViewDateTimeBoxCellEditingControl;
                control.Format = Format;
                control.RocYear = RocYear;
                control.ShowPicker = ShowPicker;

                if (DataGridView.DataSource != null && DataGridView.DataSource is BindingSource)
                {
                    DataView view = (DataGridView.DataSource as BindingSource).List as DataView;
                    if (view != null)
                    {
                        DataGridViewColumn column = this.DataGridView.Columns[this.ColumnIndex];
                        DataTable table = view.Table;
                        if (table.Columns.Contains(column.DataPropertyName))
                        {
                            control.SetBindingType(table.Columns[column.DataPropertyName].DataType);
                        }
                        else
                        {
                            control.SetBindingType(typeof(DateTime));
                        }
                    }
                    else
                    {
                        control.SetBindingType(typeof(DateTime));
                    }
                }
                else
                {
                    control.SetBindingType(typeof(DateTime));
                }
                control.BindingValue = this.DataGridView[ColumnIndex, RowIndex].Value;
            }
        }

        protected override object GetFormattedValue(object value, int rowIndex, ref DataGridViewCellStyle cellStyle, TypeConverter valueTypeConverter, TypeConverter formattedValueTypeConverter, DataGridViewDataErrorContexts context)
        {
            if (value != null)
            {
                if (value.GetType() == typeof(DateTime))
                {
                    return FormatValue((DateTime)value);
                }
                else if (value.GetType() == typeof(string))
                {
                    DateTime date = DateTime.Now;
                    if (DateTime.TryParseExact((string)value, "yyyyMMdd", new CultureInfo("en-us"), DateTimeStyles.None, out date))
                    {
                        return FormatValue(date);
                    }
                }
            }
            return string.Empty;
        }

        private string FormatValue(DateTime date)
        {
            CultureInfo culture = new CultureInfo("en-us");
            string format = Format;
            if (RocYear)
            {
                culture = new CultureInfo("zh-tw");
                culture.DateTimeFormat.Calendar = new System.Globalization.TaiwanCalendar();
                if (format.Contains("yyy") && culture.DateTimeFormat.Calendar.GetYear(date) < 100)
                {
                    format = format.Replace("yyy", "0yy");
                }
            }
            return date.ToString(format, culture);//三位年份的问题未解
        }

        public override Type EditType
        {
            get
            {
                return typeof(InfoDataGridViewDateTimeBoxCellEditingControl);
            }
        }

        public override object Clone()
        {
            InfoDataGridViewDateTimeBoxCell cell = base.Clone() as InfoDataGridViewDateTimeBoxCell;
            cell.RocYear = this.RocYear;
            cell.Format = this.Format;
            cell.ShowPicker = this.ShowPicker;
            return cell;
        }
    }

    public class InfoDataGridViewDateTimeBoxCellEditingControl : InfoDateTimeBox, IDataGridViewEditingControl
    {
        internal void SetBindingType(Type type)
        {
            base.BindingType = type;
        }

        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            EditingControlDataGridView.CurrentCell.Value = this.BindingValue;
        }

        protected override void OnValidated(EventArgs e)
        {
            base.OnValidated(e);
            EditingControlDataGridView.CurrentCell.Value = this.BindingValue;
        }

        #region IDataGridViewEditingControl Members

        public void ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle)
        {
            this.Font = dataGridViewCellStyle.Font;
            this.BackColor = dataGridViewCellStyle.BackColor;
            this.ForeColor = dataGridViewCellStyle.ForeColor;
            this.Padding = dataGridViewCellStyle.Padding;
        }

        private DataGridView editingControlDataGridView;

        public DataGridView EditingControlDataGridView
        {
            get
            {
                return editingControlDataGridView;
            }
            set
            {
                editingControlDataGridView = value;
            }
        }

        public object EditingControlFormattedValue
        {
            get
            {
                return this.BindingValue;
            }
            set
            {
                this.BindingValue = value;
            }
        }

        private int editingControlRowIndex;

        public int EditingControlRowIndex
        {
            get
            {
                return editingControlRowIndex;
            }
            set
            {
                editingControlRowIndex = value;
            }
        }

        private bool editingControlValueChanged;

        public bool EditingControlValueChanged
        {
            get
            {
                return editingControlValueChanged;
            }
            set
            {
                editingControlValueChanged = value;
            }
        }

        public bool EditingControlWantsInputKey(Keys keyData, bool dataGridViewWantsInputKey)
        {
            return true;
        }

        public Cursor EditingPanelCursor
        {
            get
            {
                return this.Cursor;
            }
        }

        public object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context)
        {
            return EditingControlFormattedValue;
        }

        public void PrepareEditingControlForEdit(bool selectAll) { }

        public bool RepositionEditingControlOnValueChange
        {
            get { return false; }
        }

        #endregion

        protected override void OnValueChanged(EventArgs eventargs)
        {
            editingControlValueChanged = true;
            base.OnValueChanged(eventargs);
        }
    }
}
