using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Drawing.Design;
using System.Runtime.InteropServices;

namespace Srvtools
{
    [ToolboxBitmap(typeof(InfoDateTimePicker), "Resources.InfoDataTimePicker.ico")]
    public class InfoDateTimePicker : DateTimePicker, ISupportInitialize, IWriteValue
    {
        public InfoDateTimePicker()
        {
            init = true;
            this.ValueChanged += new EventHandler(InfoDateTimePicker_ValueChanged);
            this.KeyDown += new KeyEventHandler(InfoDateTimePicker_KeyDown);
            this.KeyPress += new KeyPressEventHandler(InfoDateTimePicker_KeyPress);
            this.Leave += new EventHandler(InfoDateTimePicker_Leave);
            this.Enter += new EventHandler(InfoDateTimePicker_Enter);
            this.Value = this.MaxDate;
            this.ShowCheckBox = true;
            
        }
	
        void InfoDateTimePicker_Enter(object sender, EventArgs e)
        {
            if (EditOnEnter)
            {
                Binding DateTimeStringBinding = this.DataBindings["DateTimeString"];
                if (DateTimeStringBinding != null)
                {
                    InfoBindingSource bindingSource = DateTimeStringBinding.DataSource as InfoBindingSource;
                    if (bindingSource != null)
                    {
                        bindingSource.BeginEdit();
                        return;
                    }
                }
                else
                {
                    Binding TextBinding = this.DataBindings["Text"];
                    if (TextBinding != null)
                    {
                        InfoBindingSource bindingSource = TextBinding.DataSource as InfoBindingSource;
                        if (bindingSource != null)
                        {
                            bindingSource.BeginEdit();
                            return;
                        }
                    }
                }
            }
        }

        void InfoDateTimePicker_KeyPress(object sender, KeyPressEventArgs e)
        {
            Binding DateTimeStringBinding = this.DataBindings["DateTimeString"];
            if (DateTimeStringBinding != null)
            {
                InfoBindingSource bindingSource = DateTimeStringBinding.DataSource as InfoBindingSource;
                if (bindingSource != null)
                {
                    if (!EditOnEnter && bindingSource.BeginEdit() == false)
                    {
                        e.KeyChar = (char)0;
                        return;
                    }
                }
            }
            else
            {
                Binding TextBinding = this.DataBindings["Text"];
                if (TextBinding != null)
                {
                    InfoBindingSource bindingSource = TextBinding.DataSource as InfoBindingSource;
                    if (bindingSource != null)
                    {
                        if (!EditOnEnter && bindingSource.BeginEdit() == false)
                        {
                            e.KeyChar = (char)0;
                            return;
                        }
                    }
                }
            }
        }

        void InfoDateTimePicker_Leave(object sender, EventArgs e)
        {
            if (!this.Checked)
            {
                Text = string.Empty;
                this.DateTimeString = string.Empty;
            }
            Binding DateTimeStringBinding = this.DataBindings["DateTimeString"];
            if (DateTimeStringBinding != null)
            {
                DateTimeStringBinding.WriteValue();
            }
            else
            {
                Binding TextBinding = this.DataBindings["Text"];
                if (TextBinding != null)
                {
                    InfoBindingSource bindingSource = TextBinding.DataSource as InfoBindingSource;
                    if (bindingSource.Current != null)
                    {
                        if (Text.Trim().Length == 0)
                        {
                            ((DataRowView)bindingSource.Current).Row[TextBinding.BindingMemberInfo.BindingField] = System.DBNull.Value;
                        }
                        else
                        {
                            TextBinding.WriteValue();
                        }
                    }
                }
            }
        }

        protected override void OnCloseUp(EventArgs eventargs)
        {
            base.OnCloseUp(eventargs);
            Binding DateTimeStringBinding = this.DataBindings["DateTimeString"];
            if (DateTimeStringBinding != null)
            {
                InfoBindingSource bindingSource = DateTimeStringBinding.DataSource as InfoBindingSource;
                if (bindingSource != null)
                {
                    if (!EditOnEnter && bindingSource.BeginEdit() == false)
                    {
                        DateTimeStringBinding.ReadValue();
                        return;
                    }
                }
            }
            else
            {
                Binding TextBinding = this.DataBindings["Text"];
                if (TextBinding != null)
                {
                    InfoBindingSource bindingSource = TextBinding.DataSource as InfoBindingSource;
                    if (bindingSource != null)
                    {
                        if (!EditOnEnter && bindingSource.BeginEdit() == false)
                        {
                            TextBinding.ReadValue();
                            return;
                        }
                    }
                }
            }
        }

        protected override void OnDropDown(EventArgs eventargs)
        {
            base.OnDropDown(eventargs);
        }

        private void InfoDateTimePicker_KeyDown(object sender, KeyEventArgs e)
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
                        }
                        else
                        {
                            SendKeys.Send("+{Tab}");
                        }
                        e.SuppressKeyPress = true;
                    }
                }
            }
        }

        private bool _EnterEnable;
        [Category("Infolight"),
        Description("Indicates whether user can leave the control by press key of enter")]
        public bool EnterEnable
        {
            get { return _EnterEnable; }
            set { _EnterEnable = value; }
        }

        private bool editOnEnter = true;

        public bool EditOnEnter
        {
            get { return editOnEnter; }
            set { editOnEnter = value; }
        }

        bool init = false;

        public void BeginInit()
        {
           
        }

        public void EndInit()
        {
            Binding DateTimeStringBinding = this.DataBindings["DateTimeString"];
            if (DateTimeStringBinding != null)
            {
                DateTimeStringBinding.DataSourceUpdateMode = DataSourceUpdateMode.Never;
                InfoBindingSource bindingSource = DateTimeStringBinding.DataSource as InfoBindingSource;
                if (bindingSource != null)
                {
                    bindingSource.PositionChanged += new EventHandler(bindingSource_PositionChanged);
                }
                DateTimeStringBinding.BindingComplete += new BindingCompleteEventHandler(DateTimeStringBinding_BindingComplete);
            }


            Binding TextBinding = this.DataBindings["Text"];
            if (TextBinding != null)
            {
                TextBinding.DataSourceUpdateMode = DataSourceUpdateMode.Never;
                InfoBindingSource bindingSource = TextBinding.DataSource as InfoBindingSource;
                if (bindingSource != null)
                {
                    bindingSource.ListChanged += new ListChangedEventHandler(bindingSource_ListChanged);
                }
            }
            init = false;
            this.RefreshLabel();
           
            Form form = this.FindForm();
            if (form != null)
            {
                form.Load += new EventHandler(form_Load);
            }
        }

        void form_Load(object sender, EventArgs e)
        {
            this.LocationChanged += new EventHandler(InfoDateTimePicker_Changed);
            this.SizeChanged += new EventHandler(InfoDateTimePicker_Changed);
        }

        void InfoDateTimePicker_Changed(object sender, EventArgs e)
        {
            if (checkedLabel != null)
            {
                this.Controls.Remove(checkedLabel);
            }
            checkedLabel = null;
            RefreshLabel();
        }
      

        //为了解决新增时DateTimePicker的显示值不会清空的问题
        void bindingSource_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
                DataRowView view = (sender as InfoBindingSource).List[e.NewIndex] as DataRowView;
                string column = this.DataBindings[0].BindingMemberInfo.BindingField;
                if (view.Row[column] != null && view.Row[column].ToString().Length == 0)
                {
                    this.Text = string.Empty;
                }
            }
        }

        private void DateTimeStringBinding_BindingComplete(object sender, BindingCompleteEventArgs e)
        {
            if (this.DataBindings["DateTimeString"] != null && this.Site == null/* && IsFirstBinding*/)
            {
                try
                {
                    if (!string.IsNullOrEmpty(this.DateTimeString))
                    {
                        VarCharToDateTime(this.DateTimeString);
                    }
                    else
                    {
                        this.Text = string.Empty;
                    }
                }
                catch { }
            }
        }

        private void bindingSource_PositionChanged(object sender, EventArgs e)
        {
            if (this.DataBindings["DateTimeString"] != null && this.Site == null)
            {
                try
                {
                    if (!string.IsNullOrEmpty(this.DateTimeString))
                    {
                        VarCharToDateTime(this.DateTimeString);
                    }
                    else
                    {
                        this.Text = string.Empty;
                    }
                }
                catch { }
            }
        }

        private void VarCharToDateTime(string strDateTime)
        {
            //string strValue = "";
            if (strDateTime != "")
            {
                int year = Convert.ToInt32(strDateTime.Substring(0, 4));
                int month = Convert.ToInt32(strDateTime.Substring(4, 2));
                int day = Convert.ToInt32(strDateTime.Substring(6, 2));
                this.Value = new DateTime(year, month, day);
                this.Checked = true;
            }
            //return strValue;
        }

        private string DateTimeToVarChar(DateTime date)
        {
            string strYear = date.Year.ToString();
            string strMonth = (date.Month.ToString().Length == 1) ? "0" + date.Month.ToString() : date.Month.ToString();
            string strDay = (date.Day.ToString().Length == 1) ? "0" + date.Day.ToString() : date.Day.ToString();

            return strYear + strMonth + strDay;
        }

        private void InfoDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            if (this.DataBindings["DateTimeString"] != null && this.Checked)
            {
                this.DateTimeString = DateTimeToVarChar(this.Value);
            }
            RefreshLabel();
        }

        Label checkedLabel = null;
        private void RefreshLabel()
        {
            if (!init)
            {
                if (this.ShowCheckBox)
                {
                    if (checkedLabel == null)
                    {
                        checkedLabel = new Label();
                        checkedLabel.Name = this.Name + "CheckLabel";
                        checkedLabel.AutoSize = false;
                        checkedLabel.BackColor = Color.White;
                        int checkWidth = System.Environment.OSVersion.Version.Major == 6 ? 18 : 16;
                        int comboWidth = GetComboWidth();
                        int borderWidth = System.Environment.OSVersion.Version.Major == 6 ? 1 : 0;
                        checkedLabel.Location = new Point(checkWidth, (this.Size.Height - this.ClientRectangle.Height) / 2 + borderWidth);
                        checkedLabel.Size = new Size(this.ClientRectangle.Width - checkWidth - comboWidth, this.ClientRectangle.Height - 2 * borderWidth);
                        this.Controls.Add(checkedLabel);
                        checkedLabel.BringToFront();
                    }

                    checkedLabel.Visible = !this.Checked;
                }
            }
        }

        private int GetComboWidth()
        {
            if (System.Environment.OSVersion.Version.Major == 6)
            {
                if (System.Environment.OSVersion.Version.Minor == 0)
                {
                    return 19;
                }
                else if (System.Environment.OSVersion.Version.Minor == 1)
                {
                    return 0;
                }
            }
            return ComboBoxInfoHelper.GetComboDropDownWidth();
        }
        
        public enum dtType
        {
            DateTime,
            VarChar
        }

        private dtType _DateTimeType;
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Category("Infolight"),
        Description("Specifies the type of data of datatime")]
        [Obsolete("The property is obsolete, datatype will depend on databind property")]
        public dtType DateTimeType
        {
            get
            {
                return _DateTimeType;
            }
            set
            {
                _DateTimeType = value;
            }
        }

        private string _DateTimeString;
        [Category("Infolight"),
        Description("The string bound to data when datatimetype set as varchar")]
        [Bindable(true)]
        public string DateTimeString
        {
            get
            {
                return _DateTimeString;
            }
            set
            {
                _DateTimeString = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false), EditorBrowsable(EditorBrowsableState.Advanced)]
        public override string Text
        {
            get
            {
                if (this.Checked)
                {
                    return base.Text;
                }
                else
                {
                    return "";
                }
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                { 
                    this.Checked = true;
                }
                base.Text = value;

                //refresh label
            }
        }

        [DefaultValue(true)]
        public new bool ShowCheckBox
        {
            get
            {
                return base.ShowCheckBox;
            }
            set
            {
                base.ShowCheckBox = value;
            }
        }

        #region IWriteValue Members

        public void WriteValue(object value)
        {
            if (value != null)
            {
                if (!value.ToString().Equals(string.Empty))
                {
                    this.Checked = true;
                    if (value is DateTime)
                    {
                        this.Value = (DateTime)value;
                    }
                    else
                    {
                        this.Text = value.ToString();
                    }
                }
                else
                {
                    this.Text = string.Empty;
                }
                WriteValue();
            }
        }

        public void WriteValue()
        {
            if (this.DataBindings["DateTimeString"] != null)
            {
                this.DataBindings["DateTimeString"].WriteValue();
            }
            if (this.DataBindings["Text"] != null)
            {
                this.DataBindings["Text"].WriteValue();
            }
        }

        #endregion
    }

        #region ComboInfoHelper
        internal class ComboBoxInfoHelper
        {
            [DllImport("user32")]
            private static extern bool GetComboBoxInfo(IntPtr hwndCombo, ref ComboBoxInfo info);

            #region RECT struct
            [StructLayout(LayoutKind.Sequential)]
            private struct RECT
            {
                public int Left;
                public int Top;
                public int Right;
                public int Bottom;
            }
            #endregion

            #region ComboBoxInfo Struct
            [StructLayout(LayoutKind.Sequential)]
            private struct ComboBoxInfo
            {
                public int cbSize;
                public RECT rcItem;
                public RECT rcButton;
                public IntPtr stateButton;
                public IntPtr hwndCombo;
                public IntPtr hwndEdit;
                public IntPtr hwndList;
            }
            #endregion

            public static int GetComboDropDownWidth()
            {
                ComboBox cb = new ComboBox();
                int width = GetComboDropDownWidth(cb.Handle);
                cb.Dispose();
                return width;
            }

            public static int GetComboDropDownWidth(IntPtr handle)
            {
                ComboBoxInfo cbi = new ComboBoxInfo();
                cbi.cbSize = Marshal.SizeOf(cbi);
                GetComboBoxInfo(handle, ref cbi);
                int width = cbi.rcButton.Right - cbi.rcButton.Left;
                return width;
            }
        }
        #endregion
}
