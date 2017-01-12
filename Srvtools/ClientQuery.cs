/*  file: ClientQuery.cs
 *  version: 3.3
 *  lastedittime 28/4/2006 14:47
 *  remark:
 *  1.solve the problem of refvalcolumn by edit the file("frmGridRefVal") at 14:47 28/4/2006
 *    detail: in method doOK(): add new judgement whether the property "SelectedValue" has be bonded
 *  2.add new property "gapvertical" & "gaphorizontal" at 15:14 12/5/2006
 *  3 add new previewer                                at 16:01 12/5/2006
 * 
 * 
 */
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.Drawing;
using System.Reflection;
using System.Data;

namespace Srvtools
{
    //大修改
    [ToolboxItem(true)]
    [Designer(typeof(clientQueryEditor), typeof(IDesigner))]
    [ToolboxBitmap(typeof(ClientQuery), "Resources.ClientQuery.ICO")]
    public class ClientQuery : InfoBaseComp
    {

        #region Constructor
        public ClientQuery(System.ComponentModel.IContainer container)
            : this()
        {
            container.Add(this);
        }

        public ClientQuery()
        {
            _column = new QueryColumnsCollection(this, typeof(QueryColumns));
            _caption = "";
            _margin = new Margins(100, 30, 30, 30);
            _gaphorizontal = 80;
            _gapvertical = 20;
            _keepcondition = false;
            _font = new Font("SimSun", 9.0f);
            _forecolor = SystemColors.ControlText;
            _textcolor = SystemColors.ControlText;
        }

        #endregion

        #region Properties

        private string _caption;
        [Category("Infolight"),
        Description("The caption of the form which ClientQuery creates")]
        public string Caption
        {
            get
            {
                return _caption;
            }
            set
            {
                _caption = value;
            }
        }

        private InfoBindingSource m_bindingsource = null;
        [Category("Infolight"),
        Description("The InfoBindingSource which the control is bound to")]
        [AttributeProvider(typeof(IListSource))]
        [RefreshProperties(RefreshProperties.All)]
        public InfoBindingSource BindingSource
        {
            get
            {
                return m_bindingsource;
            }
            set
            {
                if (value != m_bindingsource)
                {
                    m_bindingsource = value;
                    this.Columns.Clear();
                }
            }
        }

        private Margins _margin;
        [Category("Infolight"),
        Description("Specifies space between the controls ClientQuery created and the border of the form or panel")]
        public Margins Margin
        {
            get
            {
                return _margin;
            }
            set
            {
                _margin = value;
            }
        }

        private int _gaphorizontal;
        [Category("Infolight"),
        Description("Specifies horizontal distance between the controls ClientQuery created")]
        public int GapHorizontal
        {
            get
            {
                return _gaphorizontal;
            }
            set
            {
                _gaphorizontal = value;
            }
        }

        private int _gapvertical;
        [Category("Infolight"),
        Description("Specifies vertical distance between the controls ClientQuery created")]
        public int GapVertical
        {
            get
            {
                return _gapvertical;
            }
            set
            {
                _gapvertical = value;
            }

        }

        private QueryColumnsCollection _column;
        [Category("Infolight"),
        Description("The columns which ClientQuery is applied to")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public QueryColumnsCollection Columns
        {
            get
            {
                return _column;
            }
            set
            {
                _column = value;
            }
        }

        private bool _keepcondition;
        [Category("Infolight"),
        Description("Indicates whether the text will be cleared after excute query")]
        public bool KeepCondition
        {
            get
            {
                return _keepcondition;
            }
            set
            {
                _keepcondition = value;
            }
        }


        private Font _font;
        [Category("Infolight"),
        Description("The font used for text in the controls which ClientQuery creates")]
        public Font Font
        {
            get
            {
                return _font;
            }
            set
            {
                _font = value;
            }
        }

        private Color _forecolor;
        [Category("Infolight"),
        Description("The color of the label's text which ClientQuery creates")]
        public Color ForeColor
        {
            get
            {
                return _forecolor;
            }
            set
            {
                _forecolor = value;
            }
        }

        private Color _textcolor;
        [Category("Infolight"),
        Description("The color of the text which users input")]
        public Color TextColor
        {
            get
            {
                return _textcolor;
            }
            set
            {
                _textcolor = value;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        public List<string> isShow
        {
            get
            {
                return null;
            }
            set { }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        public bool isShowInsp
        {
            get
            {
                return false;
            }
            set { }
        }

        private Panel _ActivePanel;
        [Browsable(false)]
        public Panel ActivePanel
        {
            get { return _ActivePanel; }
        }


        #endregion

        private object EventQueryWhere = new object();
        [Category("Infolight"),
        Description("The event ocured before query")]
        public event NavigatorQueryWhereEventHandler QueryWhere
        {
            add
            {
                Events.AddHandler(EventQueryWhere, value);
            }
            remove
            {
                Events.RemoveHandler(EventQueryWhere, value);
            }
        }

        public void OnQueryWhere(NavigatorQueryWhereEventArgs value)
        {
            NavigatorQueryWhereEventHandler handler = (NavigatorQueryWhereEventHandler)Events[EventQueryWhere];
            if (handler != null)
            {
                handler(this, value);
            }
        }

        #region Method

        public void Execute()
        {
            Execute(true);
        }

        private frmClientQuery fcq = null;

        public string Execute(bool ExecuteSQL)
        {
            string strwhere = null;
            if (fcq == null || !KeepCondition)
            {
                fcq = new frmClientQuery(this, false);
            }
            if (fcq.ShowDialog() == DialogResult.OK)
            {
                if (ExecuteSQL)
                {
                    Execute(fcq.splitContainer1.Panel1);
                }
                else
                {
                    strwhere = GetWhere(fcq.splitContainer1.Panel1);
                }
            }
            return strwhere;
        }

        public string Execute(Panel pn)
        {
            if (this.BindingSource != null)
            {
                InfoDataSet ids = this.BindingSource.DataSource as InfoDataSet;
                string wherestring = this.GetWhere(pn);
                NavigatorQueryWhereEventArgs args = new NavigatorQueryWhereEventArgs(wherestring);
                OnQueryWhere(args);
                if (!args.Cancel)
                {
                    wherestring = args.WhereString;
                    ids.SetWhere(wherestring);
                }
                return wherestring;
            }
            else
            {
                throw new Exception(string.Format("BindingSource doesn't exist"));
            }
        }

        public string GetWhereText()
        {
            return QueryTranslate.Translate(this);
        }

        public string GetWhereText(Panel pn)
        {
            this.GetWhere(pn);
            return QueryTranslate.Translate(this);
        }

        private string[] GetDBType()
        {
            object[] myRet = CliUtils.CallMethod("GLModule", "GetDataBaseType", new object[] { CliUtils.fLoginDB });
            string type = "";
            string odbcType = "";
            if (myRet != null && (int)myRet[0] == 0)
            {
                type = myRet[1].ToString();
                odbcType = myRet[2].ToString();
            }
            return new string[] { type, odbcType };
        }

        public void Show(Panel panel)
        {
            if (this.ActivePanel != panel)
            {
                this._ActivePanel = panel;
                panel.AutoScroll = true;
                Point location = new Point(this.Margin.Left, this.Margin.Top);
                Control lastControl = null;
                for (int i = 0; i < this.Columns.Count; i++)
                {
                    QueryColumns column = (QueryColumns)this.Columns[i];

                    if (lastControl != null && column.Visible)
                    {
                        if (column.NewLine)
                        {
                            location = new Point(this.Margin.Left, lastControl.Bottom + GapVertical);
                        }
                        else
                        {
                            location = new Point(lastControl.Right + GapHorizontal, location.Y);
                        }
                    }

                    Control ctrl = CreateControl(column, location);
                    ctrl.Visible = column.Visible;
                    if (column.Visible)
                    {
                        lastControl = ctrl;
                        CreateLabel(column, location);
                    }
                }
            }
        }

        public void Clear(Panel panel)
        {
            if (this.ActivePanel == panel)
            {
                for (int i = 0; i < this.Columns.Count; i++)
                {
                    QueryColumns column = (QueryColumns)this.Columns[i];
                    Control control = GetControl(i);
                    if (control is TextBox)
                    {
                        (control as TextBox).Text = string.Empty;
                    }
                    else if (control is InfoRefvalBox)
                    {
                        (control as InfoRefvalBox).TextBoxSelectedValue = string.Empty;
                    }
                    if (control is DateTimePicker)
                    {
                        (control as DateTimePicker).Checked = false;
                    }
                    else if (control is ComboBox)
                    {
                        (control as ComboBox).SelectedIndex = -1;
                        //if (column.DefaultMode == QueryColumns.DefaultModeType.Focused)
                        //{
                        (control as ComboBox).SelectedIndex = -1;//why...can not find reason
                        //}
                    }
                    else if (control is CheckBox)
                    {
                        (control as CheckBox).Checked = false;
                    }
                    if (control != null)
                    {
                        SetControlDefaultValue(column, control, GetPropertyName(column));
                    }
                }
            }
        }

        public Control GetControl(int index)
        {
            Control control = null;
            if (this.ActivePanel != null)
            {
                control = this.ActivePanel.Controls[string.Format("txt{0}", index)];
            }
            return control;
        }

        public string GetWhere(Panel panel)
        {
            if (this.BindingSource == null)
            {
                throw new Exception("BindingSource propery is empty");
            }
            if (this.BindingSource.DataSource.GetType() != typeof(InfoDataSet))
            {
                throw new Exception("ClientQuery can only used in master table");
            }
            DataTable table = (this.BindingSource.DataSource as InfoDataSet).RealDataSet.Tables[this.BindingSource.DataMember];
            if (table == null)
            {
                throw new Exception(string.Format("Table: {0} does not exsit in dataset", this.BindingSource.DataMember));
            }
            string sqlcmd = DBUtils.GetCommandText(this.BindingSource);
            string[] DBType = GetDBType();
            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < this.Columns.Count; i++)
            {
                QueryColumns column = (QueryColumns)this.Columns[i];
                Control control = GetControl(i);
                if (control != null)
                {
                    if (!table.Columns.Contains(column.Column))
                    {
                        throw new Exception(string.Format("Column: {0} does not exist in dataset", column.Column));
                    }
                    Type datatype = table.Columns[column.Column].DataType;
                    string valuequote = (datatype == typeof(string) || datatype == typeof(char) || datatype == typeof(Guid))
                        ? "'" : string.Empty;
                    string nvarCharMark = datatype == typeof(string) && column.IsNvarChar ? "N" : string.Empty;
                    object value = control.GetType().GetProperty(GetPropertyName(column)).GetValue(control, null);
                    if (value == null)
                    {
                        value = string.Empty;
                    }
                    if (control is DateTimePicker && !(control as DateTimePicker).Checked)
                    {
                        value = string.Empty;
                    }
                    if (control is InfoDateTimeBox && ((control) as InfoDateTimeBox).IsEmpty)
                    {
                        value = string.Empty;
                    }

                    string strwhere = string.Empty;
                    try
                    {
                        if (value.ToString().Length == 0 || string.Compare(value.ToString(), "null", true) == 0)
                        {
                            strwhere = value.ToString();
                            column.Text = value.ToString();
                        }
                        else if (datatype == typeof(DateTime))
                        {
                            DateTime dt = (DateTime)Convert.ChangeType(value, typeof(DateTime));//所有时间类型分数据库
                            switch (DBType[0])
                            {
                                case "1": strwhere = string.Format("'{0}-{1}-{2}'", dt.Year, dt.Month, dt.Day); break;
                                case "2": strwhere = string.Format("'{0}-{1}-{2}'", dt.Year, dt.Month, dt.Day); break;
                                case "3": strwhere = string.Format("to_Date('{0:0000}{1:00}{2:00}', 'yyyymmdd')", dt.Year, dt.Month, dt.Day); break;
                                case "4":
                                    {
                                        if (DBType[1] == "0")
                                        {
                                            strwhere = string.Format("to_Date('{0:0000}{1:00}{2:00}000000', '%Y%m%d%H%M%S')", dt.Year, dt.Month, dt.Day);
                                        }
                                        else
                                        {
                                            strwhere = string.Format("{{{1:00}/{2:00}/{0:0000}}}", dt.Year, dt.Month, dt.Day);
                                        }
                                        break;
                                    }
                                case "5": strwhere = string.Format("'{0}-{1}-{2}'", dt.Year, dt.Month, dt.Day); break;
                                case "6": strwhere = string.Format("to_Date('{0:0000}{1:00}{2:00}000000', '%Y%m%d%H%M%S')", dt.Year, dt.Month, dt.Day); break;
                                case "7": strwhere = string.Format("'{0}-{1}-{2}'", dt.Year, dt.Month, dt.Day); break;
                            }
                            column.Text = dt.ToShortDateString();
                        }
                        else
                        {
                            if (value.GetType() == typeof(bool))//checkbox redefination
                            {
                                if (datatype == typeof(string))
                                {
                                    strwhere = bool.Equals(value, true) ? "Y" : "N";
                                }
                                else
                                {
                                    strwhere = bool.Equals(value, true) ? "1" : "0";
                                }
                            }
                            else if (value.GetType() == typeof(DateTime))
                            {
                                DateTime dt = (DateTime)value;
                                if (datatype == typeof(string))
                                {
                                    strwhere = string.Format("{0:0000}{1:00}{2:00}", dt.Year, dt.Month, dt.Day);
                                }
                            }
                            else if (datatype == typeof(Guid))
                            {
                                try
                                {
                                    Guid id = new Guid(value.ToString());
                                    strwhere = value.ToString();
                                }
                                catch (FormatException)
                                {
                                    throw new InvalidCastException(string.Format("Can not convert '{0}' to {1} type", value, datatype.Name));
                                }
                            }
                            else
                            {
                                if (!column.Operator.Equals("in"))
                                {
                                    Convert.ChangeType(value, datatype);
                                    strwhere = value.ToString().Replace("'", "''");
                                }
                                else
                                {
                                    string[] liststring = value.ToString().Split(',');
                                    StringBuilder inBuilder = new StringBuilder("(");
                                    foreach (string str in liststring)
                                    {
                                        Convert.ChangeType(str, datatype);
                                        if (inBuilder.Length > 1)
                                        {
                                            inBuilder.Append(",");
                                        }
                                        inBuilder.Append(string.Format("{2}{0}{1}{0}", valuequote, str.Replace("'", "''"), nvarCharMark));
                                    }
                                    inBuilder.Append(")");
                                    strwhere = inBuilder.ToString();
                                    valuequote = string.Empty;//下面不加了
                                }
                            }
                            column.Text = value.ToString();
                        }
                    }
                    catch (InvalidCastException)
                    {
                        throw new InvalidCastException(string.Format("Can not convert '{0}' to {1} type", value, datatype.Name));
                    }
                    catch (FormatException)
                    {
                        throw new InvalidCastException(string.Format("Can not convert '{0}' to {1} type", value, datatype.Name));
                    }

                    if (strwhere.Length > 0)
                    {
                        if (sBuilder.Length > 0)
                        {
                            sBuilder.Append(string.Format(" {0}", column.Condition));
                        }
                        string columnname = CliUtils.GetTableNameForColumn(sqlcmd, column.Column);


                        if (string.Compare(strwhere.Trim(), "null", true) == 0) //null
                        {
                            if (valuequote.Length > 0)
                            {
                                sBuilder.Append("(");
                            }
                            sBuilder.Append(string.Format("{0}", columnname));
                            if (column.Operator.Equals("!="))
                            {
                                sBuilder.Append(" is not null");
                            }
                            else
                            {
                                sBuilder.Append(" is null");
                            }
                            if (valuequote.Length > 0)
                            {
                                if (column.Operator.Equals("!="))
                                {
                                    sBuilder.Append(string.Format(" and {0} <> '')", columnname));
                                }
                                else
                                {
                                    sBuilder.Append(string.Format(" or {0} = '')", columnname));
                                }
                            }
                        }
                        else
                        {
                            sBuilder.Append(string.Format("{0}", columnname));
                            if (!column.Operator.StartsWith("%"))
                            {
                                sBuilder.Append(string.Format("{0} {3}{1}{2}{1}", column.Operator, valuequote, strwhere, nvarCharMark));
                            }
                            else
                            {
                                if (value.GetType() == typeof(DateTime))
                                {
                                    DateTime dt = (DateTime)Convert.ChangeType(value, typeof(DateTime));
                                    strwhere = string.Format("{0}-{1}-{2}", dt.Year, dt.Month, dt.Day);
                                }
                                sBuilder.Append(string.Format(" like {2}'{0}{1}%'", (column.Operator == "%%") ? "%" : string.Empty, strwhere, nvarCharMark));
                            }
                        }

                    }
                }
            }
            return sBuilder.ToString();
        }

        private Label CreateLabel(QueryColumns column, Point location)
        {
            Label label = new Label();
            ActivePanel.Controls.Add(label);
            label.AutoSize = true;
            label.TextAlign = ContentAlignment.MiddleRight;
            label.Name = "caplbl" + column.Column;
            label.Text = column.Caption;
            label.BackColor = Color.Transparent;
            label.ForeColor = this.ForeColor;
            label.Font = this.Font;
            label.Location = new Point(location.X - label.PreferredWidth, location.Y + 4);
            return label;
        }

        private Control CreateControl(QueryColumns column, Point location)
        {
            Control control = null;
            int height = 20;
            Point ctlocation = location;

            DataTable table = (this.BindingSource.DataSource as InfoDataSet).RealDataSet.Tables[this.BindingSource.DataMember];
            if (table == null)
            {
                throw new Exception(string.Format("Table: {0} does not exsit in dataset", this.BindingSource.DataMember));
            }


            switch (column.ColumnType)
            {
                case "ClientQueryTextBoxColumn":
                    {
                        control = new InfoTextBox();

                        (control as TextBox).TextAlign = (HorizontalAlignment)Enum.Parse(typeof(HorizontalAlignment), column.TextAlign);
                        (control as InfoTextBox).EnterEnable = true;
                        if (!table.Columns.Contains(column.Column))
                        {
                            throw new Exception(string.Format("Column: {0} does not exist in dataset", column.Column));
                        }
                        (control as TextBox).Tag = table.Columns[column.Column].DataType;
                        (control as TextBox).KeyPress += delegate(object sender, KeyPressEventArgs e)
                        {
                            Type columnType = (Type)(sender as Control).Tag;
                            if (columnType == typeof(int) || columnType == typeof(uint) || columnType == typeof(byte) || columnType == typeof(Int16))
                            {
                                if (!char.IsDigit(e.KeyChar) && !e.KeyChar.Equals('\b'))
                                {
                                    e.KeyChar = (char)0;
                                }
                            }
                            else if (columnType == typeof(float) || columnType == typeof(double) || columnType == typeof(decimal))
                            {
                                if (!char.IsDigit(e.KeyChar) && !e.KeyChar.Equals('\b') && !e.KeyChar.Equals('.'))
                                {
                                    e.KeyChar = (char)0;
                                }
                            }
                        };
                        ActivePanel.Controls.Add(control);
                        break;
                    }
                case "ClientQueryComboBoxColumn":
                    {
                        control = new ComboBox();
                        (control as ComboBox).DropDownStyle = ComboBoxStyle.DropDownList;
                        (control as ComboBox).DataSource = column.InfoRefVal.DataSource;
                        (control as ComboBox).DisplayMember = column.InfoRefVal.DisplayMember;
                        (control as ComboBox).ValueMember = column.InfoRefVal.ValueMember;
                        (control as ComboBox).KeyDown += delegate(object sender, KeyEventArgs e)
                        {
                            if (e.KeyData == Keys.Delete)
                            {
                                (sender as ComboBox).SelectedIndex = -1;
                            }
                        };
                        ActivePanel.Controls.Add(control);
                        break;
                    }
                case "ClientQueryRefValColumn":
                    {
                        control = new InfoRefvalBox();
                        (control as InfoRefvalBox).TextBoxTextAlign = (HorizontalAlignment)Enum.Parse(typeof(HorizontalAlignment), column.TextAlign);
                        (control as InfoRefvalBox).TextBoxForeColor = this.TextColor;
                        (control as InfoRefvalBox).TextBoxFont = this.Font;
                        (control as InfoRefvalBox).RefVal = column.InfoRefVal;
                        (control as InfoRefvalBox).ExternalRefVal = column.ExternalRefVal;
                        (control as InfoRefvalBox).EnterEnable = true;
                        height = 22;
                        ActivePanel.Controls.Add(control);
                        break;
                    }
                case "ClientQueryCalendarColumn":
                    {
                        control = new DateTimePicker();
                        (control as DateTimePicker).ShowCheckBox = true;
                        (control as DateTimePicker).Checked = false;
                        ActivePanel.Controls.Add(control);
                        break;
                    }
                case "ClientQueryCheckBoxColumn":
                    {
                        control = new CheckBox();
                        (control as CheckBox).Checked = false;
                        control.BackColor = Color.Transparent;
                        height = 13;
                        ctlocation.Offset(0, 3);
                        ActivePanel.Controls.Add(control);
                        break;
                    }
                case "ClientQueryRefButtonColumn":
                    {
                        InfoTranslate itRefButton = new InfoTranslate();
                        if (column.InfoRefVal.SelectAlias != String.Empty && column.InfoRefVal.SelectCommand != String.Empty)
                        {
                            InfoDataSet idsRefButton = new InfoDataSet();
                            idsRefButton.RemoteName = "GLModule.cmdRefValUse";
                            idsRefButton.Execute(column.InfoRefVal.SelectCommand, column.InfoRefVal.SelectAlias, true);
                            InfoBindingSource ibsRefButton = new InfoBindingSource();
                            ibsRefButton.DataSource = idsRefButton;
                            ibsRefButton.DataMember = "cmdRefValUse";
                            itRefButton.BindingSource = ibsRefButton;
                        }
                        else
                        {
                            itRefButton.BindingSource = column.InfoRefVal.DataSource as InfoBindingSource;
                        }
                        TranslateRefReturnFields trrf2 = new TranslateRefReturnFields();
                        trrf2.ColumnName = column.InfoRefVal.ValueMember;
                        trrf2.DisplayColumnName = column.InfoRefVal.DisplayMember;
                        itRefButton.RefReturnFields.Add(trrf2);

                        control = new InfoTextBox();
                        control.Name = "txt" + this.Columns.GetItemIndex(column);
                        (control as TextBox).TextAlign = (HorizontalAlignment)Enum.Parse(typeof(HorizontalAlignment), column.TextAlign);
                        (control as InfoTextBox).EnterEnable = true;

                        InfoRefButton ifb2 = new InfoRefButton();
                        if (column.InfoRefButtonAutoPanel)
                            ifb2.autoPanel = true;
                        else
                        {
                            ifb2.autoPanel = false;
                            ifb2.panel = column.InfoRefButtonPanel;
                        }
                        ifb2.infoTranslate = itRefButton;
                        RefButtonMatch rbm2 = new RefButtonMatch();
                        rbm2.matchColumnName = control.Name;
                        ifb2.refButtonMatchs.Add(rbm2);
                        ifb2.Name = column.Column + "ClientQueryValue1InfoRefButton";
                        ifb2.Text = "...";
                        ifb2.Width = 20;
                        ifb2.Location = new Point(ctlocation.X + control.Width, ctlocation.Y);
                        ActivePanel.Controls.Add(ifb2);
                        ActivePanel.Controls.Add(control);
                        break;
                    }
                case "ClientQueryDateTimeBoxColumn":
                    {
                        control = new InfoDateTimeBox();
                        if (column.InfoDateTimeBox != null)
                        {
                            (control as InfoDateTimeBox).Format = column.InfoDateTimeBox.Format;
                            (control as InfoDateTimeBox).BackColor = column.InfoDateTimeBox.BackColor;
                            (control as InfoDateTimeBox).ForeColor = column.InfoDateTimeBox.ForeColor;
                            (control as InfoDateTimeBox).RocYear = column.InfoDateTimeBox.RocYear;
                            (control as InfoDateTimeBox).ShowPicker = column.InfoDateTimeBox.ShowPicker;

                        }
                        (control as InfoDateTimeBox).EnterEnable = true;
                        ActivePanel.Controls.Add(control);
                        break;
                    }
                default:
                    {
                        throw new ArgumentException(string.Format("{0} is not valid clientquery columntype", column.ColumnType));
                    }
            }

            if (control is ComboBox)//should set index after add control
            {
                (control as ComboBox).SelectedIndex = -1;
                //if (column.DefaultMode == QueryColumns.DefaultModeType.Focused)
                //{
                (control as ComboBox).SelectedIndex = -1;//why...can not find reason
                //}
            }
            SetControlDefaultValue(column, control, GetPropertyName(column));
            //common property
            control.Name = "txt" + this.Columns.GetItemIndex(column);
            control.TabIndex = this.Columns.GetItemIndex(column) + 2;
            control.Size = new Size(column.Width, height);
            control.ForeColor = this.TextColor;
            control.Font = this.Font;
            control.Location = ctlocation;
            return control;
        }

        private string GetPropertyName(QueryColumns column)
        {
            string propertyname = string.Empty;
            switch (column.ColumnType)
            {
                case "ClientQueryTextBoxColumn":
                case "ClientQueryRefButtonColumn":
                    propertyname = "Text"; break;
                case "ClientQueryComboBoxColumn": propertyname = "SelectedValue"; break;
                case "ClientQueryRefValColumn": propertyname = "TextBoxSelectedValue"; break;
                case "ClientQueryCalendarColumn": propertyname = "Value"; break;
                case "ClientQueryCheckBoxColumn": propertyname = "Checked"; break;
                case "ClientQueryDateTimeBoxColumn": propertyname = "Value"; break;
                default: break;
            }
            return propertyname;
        }

        private void SetControlDefaultValue(QueryColumns column, Control control, string propertyname)
        {
            PropertyInfo pi = control.GetType().GetProperty(propertyname);
            if (pi != null)
            {
                if (column.DefaultMode == QueryColumns.DefaultModeType.Initial)
                {
                    object value = CliUtils.GetValue(column.DefaultValue, this.OwnerComp as InfoForm);
                    if (value != null && value.ToString().Length > 0)
                    {
                        if (control is DateTimePicker)
                        {
                            (control as DateTimePicker).Checked = true;
                        }
                        else if (control is InfoDateTimeBox)
                        {
                            (control as InfoDateTimeBox).IsEmpty = false;
                        }
                        try
                        {
                            pi.SetValue(control, Convert.ChangeType(value, pi.PropertyType), null);
                        }
                        catch { }
                    }
                }
                else
                {
                    control.Enter += delegate(object sender, EventArgs e)
                    {
                        object value = CliUtils.GetValue(column.DefaultValue, this.OwnerComp as InfoForm);
                        if (value != null && value.ToString().Length > 0)
                        {
                            object controlvalue = pi.GetValue(control, null);
                            if (control is DateTimePicker && !(control as DateTimePicker).Checked)
                            {
                                (control as DateTimePicker).Checked = true;
                                controlvalue = string.Empty;
                            }
                            else if (control is InfoDateTimeBox)
                            {
                                (control as InfoDateTimeBox).IsEmpty = false;
                            }
                            if (controlvalue == null || controlvalue.ToString().Length == 0)
                            {
                                try
                                {
                                    pi.SetValue(control, Convert.ChangeType(value, pi.PropertyType), null);
                                }
                                catch { }
                            }
                        }
                    };
                }
            }
        }

        public string GetText()
        {
            string strQueryText = "";
            int intColNum = this.Columns.Count;
            for (int i = 0; i < intColNum; i++)
            {
                QueryColumns qc = this.Columns[i];
                if (qc.Text != string.Empty)
                {
                    strQueryText += qc.Caption + " " + qc.Operator + " " + qc.Text + "\n";
                }
            }
            return strQueryText;
        }

        #endregion
    }


}
