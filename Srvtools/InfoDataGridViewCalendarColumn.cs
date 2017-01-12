using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Reflection;
using System.ComponentModel;

namespace Srvtools
{
    [System.Drawing.ToolboxBitmap(typeof(System.Windows.Forms.DateTimePicker), "DateTimePicker.bmp")]
    public class InfoDataGridViewCalendarColumn : DataGridViewColumn
    {
        public InfoDataGridViewCalendarColumn()
            : base(new InfoDataGridViewCalendarCell())
        {
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
                // Ensure that the cell used for the template is a InfoDataGridViewCalendarCell.
                if (value != null &&
                    !value.GetType().IsAssignableFrom(typeof(InfoDataGridViewCalendarCell)))
                {
                    throw new InvalidCastException("Must be a InfoDataGridViewCalendarCell");
                }
                base.CellTemplate = value;
            }
        }

        public bool ShowCheckBox
        {
            get
            {
                return (this.CellTemplate as InfoDataGridViewCalendarCell).ShowCheckBox;
            }
            set
            {
                InfoDataGridViewCalendarCell template = this.CellTemplate as InfoDataGridViewCalendarCell;
                if (template != null)
                {
                    template.ShowCheckBox = value;
                }
                if (base.DataGridView != null)
                {
                    int rowCount = base.DataGridView.Rows.Count;
                    DataGridViewRowCollection rowCollection = base.DataGridView.Rows;
                    for (int index = 0; index < rowCount; ++index)
                    {
                        DataGridViewRow row = rowCollection.SharedRow(index);
                        InfoDataGridViewCalendarCell cell =
                            row.Cells[base.Index] as InfoDataGridViewCalendarCell;
                        if (cell != null)
                        {
                            cell.ShowCheckBox = value;
                        }
                    }
                }
            }
        }
    }

    #region InfoDataGridViewCalendarCell
    public class InfoDataGridViewCalendarCell : DataGridViewTextBoxCell
    {
        public InfoDataGridViewCalendarCell()
            : base()
        {
            // Use the short date format.
            this.Style.Format = "d";
        }

        public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            // Set the value of the editing control to the current cell value.
            base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);
            InfoDataGridViewCalendarEditingControl ctl = DataGridView.EditingControl as InfoDataGridViewCalendarEditingControl;
            ctl.ShowCheckBox = this.ShowCheckBox;
            if (this.Value != null && this.Value.ToString() != "")
            {
                ctl.Text = VarCharToDateTime(this.Value.ToString()).ToShortDateString();
            }
            else 
            {
                ctl.Text = "";
            }
            if (bShowCalendar)
            {
                SendKeys.Send("{F4}");
                bShowCalendar = false;
            }
        }

        private bool _ShowCheckBox;
        public bool ShowCheckBox
        {
            get
            {
                return _ShowCheckBox;
            }
            set
            {
                _ShowCheckBox = value;
            }
        }

        public override Type EditType
        {
            get
            {
                // Return the type of the editing contol that InfoDataGridViewCalendarCell uses.
                return typeof(InfoDataGridViewCalendarEditingControl);
            }
        }

        public override Type ValueType
        {
            get
            {
                // Return the type of the value that InfoDataGridViewCalendarCell contains.
                return typeof(DateTime);
                //return this.DataGridView.Columns[this.ColumnIndex].ValueType;
            }
        }

        bool bShowCalendar = false;
        protected override void OnMouseDown(DataGridViewCellMouseEventArgs e)
        {
            base.OnMouseDown(e);
           
            //int y = this.Size.Height;
            if (this.Selected)
            {
                int x = this.Size.Width - 20;
                if (e.X <= this.Size.Width && e.X >= x)
                {
                    bShowCalendar = true;
                }
            }
        }

        private DateTime VarCharToDateTime(string strDateTime)
        {
            DateTime dt = new DateTime();
            if (strDateTime != "")
            {
                try
                {
                    dt = Convert.ToDateTime(strDateTime);
                }
                catch
                {
                    string strValue = strDateTime.Substring(0, 4) + "-" + strDateTime.Substring(4, 2) + "-" + strDateTime.Substring(6, 2);
                    dt = Convert.ToDateTime(strValue);
                }
            }
            return dt;
        }

        public override object Clone()
        {
            InfoDataGridViewCalendarCell cell = base.Clone() as InfoDataGridViewCalendarCell;
            cell.ShowCheckBox = this.ShowCheckBox;
            return cell;
        }

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
                if (value != null && value.ToString() != "")
                {
                    formattedValue = VarCharToDateTime(formattedValue.ToString()).ToShortDateString();
                }
                else
                {
                    formattedValue = "";//"9998-12-31";
                }
            }
            base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);
            //if (rowIndex < rowCnt)
            //{
            //    Rectangle rect = cellBounds;
            //    Point buttonLocation = new Point();
            //    buttonLocation.Y = cellBounds.Y;
            //    buttonLocation.X = cellBounds.X;
            //    Pen pen1 = new Pen(Brushes.White, 1);
            //    Pen pen2 = new Pen(Brushes.Black, 1);
            //    Pen pen3 = new Pen(Brushes.DimGray, 1);
            //    int i = this.DataGridView[this.ColumnIndex, rowIndex].Size.Width;

            //    SolidBrush myBrush = new SolidBrush(SystemColors.Control);
            //    graphics.FillRectangle(myBrush, buttonLocation.X + i - 21, buttonLocation.Y + 3, 18, 18);
            //    graphics.DrawLine(pen1, buttonLocation.X + i - 21, buttonLocation.Y + 3, buttonLocation.X + i - 3, buttonLocation.Y + 3); //top
            //    graphics.DrawLine(pen1, buttonLocation.X + i - 21, buttonLocation.Y + 3, buttonLocation.X + i - 21, buttonLocation.Y + 21); // left
            //    graphics.DrawLine(pen2, buttonLocation.X + i - 3, buttonLocation.Y + 21, buttonLocation.X + i - 21, buttonLocation.Y + 21); // Bottom
            //    graphics.DrawLine(pen3, buttonLocation.X + i - 4, buttonLocation.Y + 20, buttonLocation.X + i - 20, buttonLocation.Y + 20);
            //    graphics.DrawLine(pen2, buttonLocation.X + i - 3, buttonLocation.Y + 20, buttonLocation.X + i - 3, buttonLocation.Y + 3); // right
            //    graphics.DrawLine(pen3, buttonLocation.X + i - 4, buttonLocation.Y + 21, buttonLocation.X + i - 4, buttonLocation.Y + 4);
            //    StringFormat sf = new StringFormat();
            //    sf.Alignment = StringAlignment.Center;
            //    sf.LineAlignment = StringAlignment.Center;
            //    Brush brush = Brushes.Black;
            //    Point[] points = new Point[3] {new Point(buttonLocation.X + i - 15, buttonLocation.Y + 10), 
            //                               new Point(buttonLocation.X + i - 8, buttonLocation.Y + 10),
            //                               new Point(buttonLocation.X + i - 12, buttonLocation.Y + 14)};
            //    graphics.FillPolygon(brush, points);
            //}
        }
    }
    #endregion

    #region InfoDataGridViewCalendarEditingControl
    class InfoDataGridViewCalendarEditingControl : DateTimePicker, IDataGridViewEditingControl
    {
        DataGridView dataGridView;
        private bool valueChanged = false;
        int rowIndex;

        public InfoDataGridViewCalendarEditingControl()
        {
            this.ShowCheckBox = true;
            this.Format = DateTimePickerFormat.Short;
            this.Leave += new EventHandler(InfoDataGridViewCalendarEditingControl_Leave);
        }

        private void InfoDataGridViewCalendarEditingControl_Leave(object sender, EventArgs e)
        {
            int i = this.dataGridView.CurrentCell.ColumnIndex;
            string strYear = this.Value.Year.ToString();
            string strMonth = (this.Value.Month.ToString().Length == 1) ? "0" + this.Value.Month.ToString() : this.Value.Month.ToString();
            string strDay = (this.Value.Day.ToString().Length == 1) ? "0" + this.Value.Day.ToString() : this.Value.Day.ToString();
            if (!this.Checked)
            {
                this.Text = "";
            }
            if (this.dataGridView.Columns[i].ValueType == typeof(String))
            {
                if (this.Checked)
                {
                    this.dataGridView.CurrentCell.Value = strYear + strMonth + strDay;
                }
                else
                {
                    this.dataGridView.CurrentCell.Value = "";
                }
            }
            else if (this.dataGridView.Columns[i].ValueType == typeof(DateTime))
            {
                if (this.Checked)
                {
                    this.dataGridView.CurrentCell.Value = this.Value;
                }
            }
            Form form = this.EditingControlDataGridView.FindForm();
            Type type = form.GetType();//修正,原来this.FindForm可能找不到
            FieldInfo[] fi = type.GetFields(BindingFlags.Instance
                                | BindingFlags.NonPublic
                                | BindingFlags.DeclaredOnly);
            for (int m = 0; m < fi.Length; m++)
            {
                if (fi[m].GetValue(form) is InfoDateTimePicker)
                {
                    Binding binding1 = ((InfoDateTimePicker)fi[m].GetValue(form)).DataBindings["DateTimeString"];
                    if (binding1 != null
                        && binding1.BindingMemberInfo.BindingField == this.dataGridView.Columns[this.dataGridView.CurrentCell.ColumnIndex].DataPropertyName)
                    {
                        if (this.Checked)
                        {
                            ((InfoDateTimePicker)fi[m].GetValue(form)).DateTimeString = strYear + strMonth + strDay;
                            ((InfoDateTimePicker)fi[m].GetValue(form)).Text = this.Text;
                        }
                        else
                        {
                            ((InfoDateTimePicker)fi[m].GetValue(form)).DateTimeString = "";
                            ((InfoDateTimePicker)fi[m].GetValue(form)).Text = "";
                        }

                    }
                    Binding binding2 = ((InfoDateTimePicker)fi[m].GetValue(form)).DataBindings["Text"];
                    if (binding2 != null && binding2.DataSource == this.dataGridView.DataSource
                        && binding2.BindingMemberInfo.BindingField == this.dataGridView.Columns[this.dataGridView.CurrentCell.ColumnIndex].DataPropertyName)
                    {
                        if (this.Checked)
                        {
                            ((InfoDateTimePicker)fi[m].GetValue(form)).Text = this.Text;
                        }
                        else
                        {
                            ((InfoDateTimePicker)fi[m].GetValue(form)).Text = "";
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
                if (this.Checked)
                {
                    if (this.dataGridView.Columns[this.dataGridView.CurrentCell.ColumnIndex].ValueType == typeof(String))
                    {
                        return this.Value.ToString("yyyyMMdd");
                    }
                    else
                    {
                        return this.Value.ToShortDateString();
                    }
                }
                else
                {
                    return "";
                }
            }
            set
            {
                if (value is String)
                {
                    //this.Value = DateTime.Parse((string)value);
                    this.Text = (string)value;
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
            this.CalendarForeColor = dataGridViewCellStyle.ForeColor;
            this.CalendarMonthBackground = dataGridViewCellStyle.BackColor;
        }

        // Implements the IDataGridViewEditingControl.EditingControlRowIndex property.
        public int EditingControlRowIndex
        {
            get
            {
                return rowIndex;
            }
            set
            {
                rowIndex = value;
            }
        }

        // Implements the IDataGridViewEditingControl.EditingControlWantsInputKey method.
        public bool EditingControlWantsInputKey(Keys key, bool dataGridViewWantsInputKey)
        {
            // Let the DateTimePicker handle the keys listed.
            switch (key & Keys.KeyCode)
            {
                case Keys.Left:
                case Keys.Up:
                case Keys.Down:
                case Keys.Right:
                case Keys.Home:
                case Keys.End:
                case Keys.PageDown:
                case Keys.PageUp:
                    return true;
                default:
                    return false;
            }
        }

        // Implements the IDataGridViewEditingControl.PrepareEditingControlForEdit method.
        public void PrepareEditingControlForEdit(bool selectAll)
        {
            // No preparation needs to be done.
        }

        // Implements the IDataGridViewEditingControl.RepositionEditingControlOnValueChange property.
        public bool RepositionEditingControlOnValueChange
        {
            get
            {
                return false;
            }
        }

        /*protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
        }*/

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

        protected override void OnValueChanged(EventArgs eventargs)
        {
            // Notify the DataGridView that the contents of the cell have changed.
            valueChanged = true;
            this.EditingControlDataGridView.NotifyCurrentCellDirty(true);
            base.OnValueChanged(eventargs);
        }

        public Cursor EditingControlCursor
        {
            get { return base.Cursor; }
        }
    }
    #endregion
}

