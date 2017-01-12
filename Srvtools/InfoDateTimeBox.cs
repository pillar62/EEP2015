using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Srvtools
{
    /// <summary>
    /// Control of InfoDateTimeBox
    /// </summary>
    public partial class InfoDateTimeBox : UserControl, IWriteValue
    {
        private readonly CultureInfo EN_CULTURE = new CultureInfo("en-us");

        private readonly CultureInfo TW_CULTURE = new CultureInfo("zh-tw");

        /// <summary>
        /// Create an instance of InfoDateTimeBox
        /// </summary>
        public InfoDateTimeBox()
        {
            InitializeComponent();
            this.DataBindings.CollectionChanged += new CollectionChangeEventHandler(DataBindings_CollectionChanged);
            this.button.Paint += new PaintEventHandler(button_Paint);
            this.maskedTextBox.KeyPress += new KeyPressEventHandler(maskedTextBox_KeyPress);
            this.maskedTextBox.KeyDown += new KeyEventHandler(maskedTextBox_KeyDown);
            this.maskedTextBox.Leave += new EventHandler(maskedTextBox_Leave);
            Format = "yyyy/MM/dd";
            // maskedTextBox.Mask = "0000/00/00";
        }

        private bool leaveLock;

        void maskedTextBox_Leave(object sender, EventArgs e)
        {
            if (!leaveLock)
            {
                leaveLock = true;
                try
                {
                    maskedTextBox.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
                    if (maskedTextBox.Text.Length == 0)
                    {
                        IsEmpty = true;
                    }
                    else
                    {
                        maskedTextBox.TextMaskFormat = MaskFormat.IncludeLiterals;
                        CultureInfo culture = EN_CULTURE;
                        if (RocYear)
                        {
                            culture = TW_CULTURE;
                            culture.DateTimeFormat.Calendar = new System.Globalization.TaiwanCalendar();
                        }
                        var format = System.Text.RegularExpressions.Regex.Replace(Format, "[/-]", CultureInfo.CurrentCulture.DateTimeFormat.DateSeparator);

                        Value = DateTime.ParseExact(Text, format, culture);
                        IsEmpty = false;
                    }
                    WriteValue();
                }
                catch
                {
                    ParseDateTime(Text);
                    maskedTextBox.Text = string.Empty;
                    maskedTextBox.Focus();
                }
                leaveLock = false;
            }
        }

        private void ParseDateTime(string text)//有分隔符才能解析
        {
            List<char> listChar = new List<char>();
            List<char> formats = new List<char>(new char[] { 'y', 'M', 'd', 'H', 'm', 's' });

            for (int i = 0; i < format.Length; i++)
            {
                char ch = format[i];
                if (formats.Contains(ch) && (i == 0 || !ch.Equals(format[i - 1])))
                {
                    listChar.Add(ch);
                }
            }

            MatchCollection matches = Regex.Matches(text, @"\d+");
            if (matches.Count == listChar.Count)
            {
                int month = 0;
                int year = 0;
                for (int i = 0; i < listChar.Count; i++)
                {
                    int value = Convert.ToInt32(matches[i].Value);
                    switch (listChar[i])
                    {
                        case 'y':
                            {
                                year = value;
                                break;
                            }
                        case 'M':
                            {
                                if (value > 12 || value == 0)
                                {
                                    ShowErrorMessage(formats.IndexOf(listChar[i]), 1, 12);
                                    return;
                                }
                                else
                                {
                                    month = value;
                                }
                                break;
                            }
                        case 'H':
                            {
                                if (value >= 24)
                                {
                                    ShowErrorMessage(formats.IndexOf(listChar[i]), 0, 23);
                                    return;
                                }
                                break;
                            }
                        case 'm':
                        case 's':
                            {
                                if (value >= 60)
                                {
                                    ShowErrorMessage(formats.IndexOf(listChar[i]), 0, 59);
                                    return;
                                }
                                break;
                            }
                    }
                }
                if (month != 0 && year >= DateTime.MinValue.Year)
                {
                    ShowErrorMessage(formats.IndexOf('d'), 1, DateTime.DaysInMonth(year, month));
                    return;
                }
            }

            string message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "Srvtools", "WebDateTimePicker", "DateTimeValidate");
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        private void ShowErrorMessage(int part, int minValue, int maxValue)
        {
            string message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "Srvtools", "InfoDateTimeBox", "DateTimePart");
            string[] list = message.Split(',');
            string strpart = list[part];
            message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "Srvtools", "InfoDateTimeBox", "DateTimeError");
            MessageBox.Show(string.Format(message, strpart, minValue, maxValue), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        void maskedTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!e.Control && !e.Alt)
                {
                    if (this.EnterEnable)
                    {
                        if (!e.Shift)
                        {
                            SendKeys.Send("{Tab}");
                            if (this.ShowPicker)
                            {
                                SendKeys.Send("{Tab}");
                            }
                        }
                        else
                        {
                            SendKeys.Send("+{Tab}");
                        }
                        //e.SuppressKeyPress = true;
                    }
                }
            }
        }

        void maskedTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!this.ReadOnly)
            {
                if (this.DataBindings["BindingValue"] != null)
                {
                    InfoBindingSource bindingSource = this.DataBindings["BindingValue"].DataSource as InfoBindingSource;
                    if (bindingSource != null)
                    {
                        if (!bindingSource.BeginEdit())
                        {
                            e.KeyChar = (char)0;
                            return;
                        }
                        else
                        {
                            OnValueChanged(e);
                        }
                    }
                }
            }
        }

        void button_Paint(object sender, PaintEventArgs e)
        {
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;
            e.Graphics.DrawString("...", new Font("Simsun", 9.0f), new SolidBrush(ForeColor), new Point(11, 8), sf);
        }

        void DataBindings_CollectionChanged(object sender, CollectionChangeEventArgs e)
        {
            if (e.Action == CollectionChangeAction.Add)
            {
                if (this.DataBindings["BindingValue"] != null)
                {
                    this.DataBindings["BindingValue"].DataSourceUpdateMode = DataSourceUpdateMode.Never;
                    this.DataBindings["BindingValue"].BindingComplete += new BindingCompleteEventHandler(InfoDateTimeBox_BindingComplete);
                }
            }
        }

        void InfoDateTimeBox_BindingComplete(object sender, BindingCompleteEventArgs e)
        {
            if (e.BindingCompleteContext == BindingCompleteContext.ControlUpdate)
            {
                if (BindingType == null)
                {
                    object dataSource = e.Binding.DataSource;
                    if (dataSource is BindingSource)
                    {
                        DataView view = (dataSource as BindingSource).List as DataView;
                        if (view != null)
                        {
                            DataColumn column = view.Table.Columns[e.Binding.BindingMemberInfo.BindingField];
                            if (column != null)
                            {
                                BindingType = column.DataType;
                            }
                        }
                    }
                    else
                    {
                        DataTable table = null;
                        if (dataSource is DataSet)
                        {
                            table = (dataSource as DataSet).Tables[e.Binding.BindingMemberInfo.BindingPath];
                        }
                        else if (dataSource is InfoDataSet)
                        {
                            table = (dataSource as InfoDataSet).RealDataSet.Tables[e.Binding.BindingMemberInfo.BindingPath];
                        }
                        if (table != null)
                        {
                            DataColumn column = table.Columns[e.Binding.BindingMemberInfo.BindingField];
                            if (column != null)
                            {
                                BindingType = column.DataType;
                            }
                        }
                    }
                }
            }
        }

        private bool rocYear;
        /// <summary>
        /// Gets or sets datetime is displayed as a roc format
        /// </summary>
        [Category("Infolight"),
        Description("Value indicates whether datetime is displayed as a roc format")]
        public bool RocYear
        {
            get { return rocYear; }
            set { rocYear = value; }
        }

        private string format;
        /// <summary>
        /// Gets or sets the format of datetime
        /// </summary>
        [Category("Infolight"),
        Description("Format of datetime")]
        public string Format
        {
            get { return format; }
            set
            {
                format = value;
                if (format != null)
                {
                    InitMask();
                }
            }
        }

        ///// <summary>
        ///// Gets or sets the mask of textbox
        ///// </summary>
        //[Category("Infolight"),
        //Description("Mask of textbox")]
        //public string Mask
        //{
        //    get 
        //    {
        //        return maskedTextBox.Mask;
        //    }
        //    set 
        //    {
        //        maskedTextBox.Mask = value;
        //    }
        //}

        /// <summary>
        /// Gets or sets user can leave the control by pressing key of enter
        /// </summary>
        private bool enterEnable;
        [Category("Infolight"),
        Description("Indicate whether user can leave the control by pressing key of enter")]
        public bool EnterEnable
        {
            get { return enterEnable; }
            set { enterEnable = value; }
        }

        private bool showPicker = true;

        /// <summary>
        /// Gets or sets the button is visible
        /// </summary>
        [Category("Infolight"),
        Description("Value indicates whether button is vibible")]
        public bool ShowPicker
        {
            get
            {
                return showPicker;
            }
            set
            {
                showPicker = value;
                button.Visible = value;
            }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override string Text
        {
            get
            {
                return maskedTextBox.Text;
            }
            set
            {
                maskedTextBox.Text = value;
            }
        }

        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
        }

        private DateTime _value = DateTime.Now;
        /// <summary>
        /// Gets or sets the value of datetime
        /// </summary>
        [Category("Infolight"),
        Description("Value of datetime")]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DateTime Value
        {
            get { return _value; }
            set
            {
                _value = value;
                RefreshText();
            }
        }

        private bool isEmpty = true;
        /// <summary>
        /// Gets or sets the value is empty
        /// </summary>
        [Category("Infolight"),
        Description("Value indicates whether value is empty")]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsEmpty
        {
            get { return isEmpty; }
            set
            {
                isEmpty = value;
                RefreshText();
            }
        }

        public override Color ForeColor
        {
            get
            {
                return base.ForeColor;
            }
            set
            {
                base.ForeColor = value;
                maskedTextBox.ForeColor = value;
            }
        }

        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                base.BackColor = value;
                maskedTextBox.BackColor = value;
            }
        }

        [Category("Infolight")]
        public bool ReadOnly
        {
            get { return maskedTextBox.ReadOnly; }
            set
            {
                maskedTextBox.ReadOnly = value;
                button.Enabled = !value;
            }
        }



        [Bindable(BindableSupport.Yes)]
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object BindingValue
        {
            get
            {
                if (IsEmpty)
                {
                    return DBNull.Value;
                }
                else if (BindingType == null || BindingType == typeof(DateTime))
                {
                    return Value;
                }
                else if (BindingType == typeof(string))
                {
                    return Value.ToString("yyyyMMdd", EN_CULTURE);
                }
                return DBNull.Value;
            }
            set
            {
                if (value == null || value.Equals(DBNull.Value) || value.Equals(string.Empty))
                {
                    Value = DateTime.Now;
                    IsEmpty = true;
                }
                else
                {
                    if (value.GetType() == typeof(DateTime))
                    {
                        Value = (DateTime)value;
                        IsEmpty = false;
                    }
                    else if (value.GetType() == typeof(string))
                    {
                        DateTime dt = DateTime.Now;
                        bool isempty = !DateTime.TryParseExact((string)value, "yyyyMMdd", EN_CULTURE, DateTimeStyles.None, out dt);
                        Value = dt;
                        IsEmpty = isempty;
                    }
                    else
                    {
                        DateTime dt = DateTime.Now;
                        IsEmpty = true;
                    }
                }
            }
        }

        private void InitMask()
        {
            string mask = format;
            string[] formats = new string[] { "y", "MM", "M", "dd", "d", "HH", "H", "mm", "m", "ss", "s" };
            string[] masks = new string[] { "0", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00" };
            for (int i = 0; i < formats.Length; i++)
            {
                mask = mask.Replace(formats[i], masks[i]);
            }
            maskedTextBox.Mask = mask;
        }

        protected Type BindingType;

        private void RefreshText()
        {
            if (IsEmpty)
            {
                Text = string.Empty;
            }
            else
            {
                CultureInfo culture = EN_CULTURE;
                string format = Format;
                if (RocYear)
                {
                    culture = TW_CULTURE;
                    culture.DateTimeFormat.Calendar = new System.Globalization.TaiwanCalendar();
                    if (format.Contains("yyy") && culture.DateTimeFormat.Calendar.GetYear(Value) < 100)
                    {
                        format = format.Replace("yyy", "0yy");
                    }
                }

                Text = Value.ToString(format, culture);//三位年份的问题未解
            }
        }

        Form formCalendar = null;
        private void button_Click(object sender, EventArgs e)
        {
            if (this.DataBindings["BindingValue"] != null)
            {
                InfoBindingSource bindingSource = this.DataBindings["BindingValue"].DataSource as InfoBindingSource;
                if (bindingSource != null)
                {
                    if (!bindingSource.BeginEdit())
                    {
                        return;
                    }
                }
            }
            if (formCalendar == null)
            {
                formCalendar = new Form();
                MonthCalendar calendar = new MonthCalendar();
                calendar.MaxSelectionCount = 1;
                calendar.DateSelected += delegate(object senderc, DateRangeEventArgs ec)
                {
                    IsEmpty = false;
                    Value = ec.Start;
                    maskedTextBox.Focus();
                    OnValueChanged(e);
                    (senderc as MonthCalendar).FindForm().Hide();
                };
                calendar.LostFocus += delegate(object senderc, EventArgs ec)
                {
                    (senderc as MonthCalendar).FindForm().Hide();
                };
                formCalendar.Controls.Add(calendar);
                formCalendar.Size = calendar.Size;
                formCalendar.FormBorderStyle = FormBorderStyle.FixedSingle;
                formCalendar.ControlBox = false;

                formCalendar.StartPosition = FormStartPosition.Manual;
                formCalendar.ShowInTaskbar = false;
            }
            if (!IsEmpty)
            {
                (formCalendar.Controls[0] as MonthCalendar).SelectionRange = new SelectionRange(Value, Value);
            }
            formCalendar.Location = this.PointToScreen(new Point(0, this.Height));
            formCalendar.Show();
            formCalendar.Size = formCalendar.Controls[0].Size;
            formCalendar.BringToFront();
        }

        #region IWriteValue Members

        public void WriteValue()
        {
            if (this.DataBindings["BindingValue"] != null)
            {
                this.DataBindings["BindingValue"].WriteValue();
            }
        }

        public void WriteValue(DateTime date)
        {
            Value = date;
            IsEmpty = false;
            WriteValue();
        }

        void IWriteValue.WriteValue(object value)
        {

        }

        #endregion

        protected virtual void OnValueChanged(EventArgs eventargs)
        {

        }


    }
}
