using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using System.Xml;
using System.Data.SqlClient;
using Srvtools;

namespace Srvtools
{
    public partial class frmAnyQuery : Form
    {
        private AnyQuery innerAnyQuery = null;
        private InfoNavigator aInfoNavigator = null;
        public String Where = String.Empty;
        public bool isExecute = true;
        private int Count = 0;

        private void Create()
        {
            string message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "Srvtools", "AnyQuery", "ButtonText");
            string[] ButtonText = message.Split(';');
            this.buttonQuery.Text = ButtonText[0];
            this.buttonAdd.Text = ButtonText[1];
            this.buttonSubtract.Text = ButtonText[2];
            this.buttonSave.Text = ButtonText[3];
            this.buttonLoad.Text = ButtonText[4];
            this.buttonQuit.Text = ButtonText[5];

            message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "Srvtools", "AnyQuery", "ColumnValue");
            String[] ColumnValue = message.Split(';');
            this.labelColumn.Text = ColumnValue[0];
            this.labelValue.Text = ColumnValue[1];
            this.labCondition.Text = ColumnValue[2];

            this.cbCondition.SelectedIndex = 0;

            InfoDataSet ids = null;
            if (innerAnyQuery.BindingSource != null)
            {
                ids = innerAnyQuery.BindingSource.DataSource as InfoDataSet;

                foreach (AnyQueryColumns aqc in innerAnyQuery.Columns)
                {
                    if (innerAnyQuery.MaxColumnCount == -1 || Count < innerAnyQuery.MaxColumnCount)
                    {
                        Count++;
                        CreateColumns(ids, aqc, Count, aqc.AutoSelect);
                    }
                    if (Count == 5)
                    {
                        break;
                    }
                }
            }
        }

        public frmAnyQuery()
        {
            InitializeComponent();
            Create();
        }

        public frmAnyQuery(AnyQuery aq)
        {
            innerAnyQuery = aq;
            InitializeComponent();
            Create();
        }

        public frmAnyQuery(AnyQuery aq, InfoNavigator infoN, bool executeSQL)
        {
            aInfoNavigator = infoN;
            innerAnyQuery = aq;
            isExecute = executeSQL;
            InitializeComponent();
            Create();
        }

        protected void OnValueControlEdit(EventArgs value)
        {
            EventHandler handler = (EventHandler)base.Events[EventOnValueControlEdit];
            if (handler != null)
            {
                handler(this, value);
            }
        }
        internal static readonly object EventOnValueControlEdit = new object();
        public event EventHandler ValueControlEdit
        {
            add { base.Events.AddHandler(EventOnValueControlEdit, value); }
            remove { base.Events.RemoveHandler(EventOnValueControlEdit, value); }
        }

        //当按下Query按钮之后执行的事件
        protected void OnAfterQuery(AfterQueryEventArgs value)
        {
            AfterQueryEventHandler handler = (AfterQueryEventHandler)base.Events[EventOnAfterQuery];
            if (handler != null)
            {
                handler(this, value);
            }
        }
        internal static readonly object EventOnAfterQuery = new object();
        public event AfterQueryEventHandler AfterQuery
        {
            add { base.Events.AddHandler(EventOnAfterQuery, value); }
            remove { base.Events.RemoveHandler(EventOnAfterQuery, value); }
        }

        public String[] GetControlValues(int rows)
        {
            CheckBox cbActive = null;
            ComboBox cbColumn = null;
            ComboBox cbOperator = null;
            Control cValue1 = null;
            Control cValue2 = null;
            int count = rows + 1;
            if (count > 0)
            {
                foreach (Control c in this.panel2.Controls)
                {
                    if (c.Name.StartsWith(count + "AnyQueryActiveCheckBox"))
                    {
                        cbActive = c as CheckBox;
                        continue;
                    }

                    if (c.Name.StartsWith(count + "AnyQueryColumnComboBox"))
                    {
                        cbColumn = c as ComboBox;
                        continue;
                    }

                    if (c.Name.StartsWith(count + "AnyQueryOperatorComboBox"))
                    {
                        cbOperator = c as ComboBox;
                        continue;
                    }

                    if (c.Name.StartsWith(count + "AnyQueryValue1RefTextBox"))
                    {
                        cValue1 = c;
                        continue;
                    }

                    if (c.Name.StartsWith(count + "AnyQueryValue1") && !c.Name.StartsWith(count + "AnyQueryValue1to2") && !c.Name.StartsWith(count + "AnyQueryValue1InfoRefButton"))
                    {

                        cValue1 = c;
                        continue;
                    }

                    if (c.Name.StartsWith(count + "AnyQueryValue2"))
                    {
                        cValue2 = c;
                        continue;
                    }
                }
            }

            String[] values = new String[5];
            if (cbActive != null)
            {
                if (cbActive.Checked)
                    values[0] = "Y";
                else
                    values[0] = "N";
            }
            if (cbColumn != null)
            {
                values[1] = cbColumn.Text;
            }
            if (cbOperator != null)
            {
                values[2] = cbOperator.Text;
            }
            if (cValue1 != null)
            {
                if (cValue1.Name.StartsWith(count + "AnyQueryValue1RefTextBox"))
                {
                    values[3] = (cValue1 as InfoRefbuttonBox).RealValue;
                }
                else if (cValue1.Name.StartsWith(count + "AnyQueryValue1InfoRefvalBox"))
                {
                    values[3] = (cValue1 as InfoRefvalBox).TextBoxSelectedValue;
                }
                else if (cValue1.Name.StartsWith(count + "AnyQueryValue1InfoDateTimePicker"))
                {
                    if ((cValue1 as InfoDateTimePicker).Checked)
                    {
                        values[3] = (cValue1 as InfoDateTimePicker).Value.ToString();
                    }
                }
                else if (cValue1.Name.StartsWith(count + "AnyQueryValue1CheckBox"))
                {
                    if ((cValue1 as CheckBox).Checked)
                        values[3] = "1";
                    else
                        values[3] = "0";

                }
                else
                {
                    values[3] = cValue1.Text;
                }
            }
            if (cValue2 != null)
            {
                if (cValue2.Name.StartsWith(count + "AnyQueryValue2InfoRefvalBox"))
                {
                    values[4] = (cValue1 as InfoRefvalBox).TextBoxSelectedValue;
                }
                else if (cValue2.Name.StartsWith(count + "AnyQueryValue2InfoDateTimePicker"))
                {
                    if ((cValue2 as InfoDateTimePicker).Checked)
                    {
                        values[4] = (cValue2 as InfoDateTimePicker).Value.ToString();
                    }
                }
                else
                {
                    values[4] = cValue2.Text;
                }
            }
            return values;
        }

        private void AnyQuery_Load(object sender, EventArgs e)
        {

        }

        private void CreateColumns(InfoDataSet ids, AnyQueryColumns aqc, int columnsCount, bool isActive)
        {
            String value = aqc.DefaultValue;
            if (aqc.DefaultValue.StartsWith("_"))
            {
                object[] realValue = CliUtils.GetValue(aqc.DefaultValue);
                if (realValue[0].ToString() == "0")
                    value = realValue[1].ToString();
            }

            int columnWidth = aqc.ColumnWidth;
            int ColumnsCount = columnsCount;
            if (120 + ColumnsCount * 30 > this.Height) this.Height += 30;

            int standard = 40;
            CheckBox cbActive = new CheckBox();
            cbActive.Name = ColumnsCount + "AnyQueryActiveCheckBox";
            cbActive.Width = 15;
            cbActive.Location = new Point(standard, 13 + ColumnsCount * 30);
            cbActive.Checked = isActive;
            cbActive.CheckedChanged += new EventHandler(cbActive_CheckedChanged);
            this.panel2.Controls.Add(cbActive);

            //standard += 30;
            //Label labelCondition = new Label();
            //labelCondition.Name = ColumnsCount + "AnyQueryConditionLabel";
            //labelCondition.Width = 50;
            //labelCondition.Text = aqc.Condition.ToUpper();
            //labelCondition.Location = new Point(standard, 17 + ColumnsCount * 30);
            //labelCondition.Click += new EventHandler(labelCondition_Click);
            //this.panel2.Controls.Add(labelCondition);

            standard += 30;
            ComboBox cbColumn = new ComboBox();
            cbColumn.Name = ColumnsCount + "AnyQueryColumnComboBox";
            this.panel2.Controls.Add(cbColumn);
            DataView dvColumn = CreateDataViewColumn(ids);
            cbColumn.DataSource = dvColumn;
            cbColumn.DisplayMember = "CAPTION";
            cbColumn.ValueMember = "FIELDNAME";
            cbColumn.Width = columnWidth;
            //if (innerAnyQuery.QueryColumnMode == AnyQueryColumnMode.ByBindingSource)
            //    cbColumn.Text = GetHeaderText(aqc.Column);
            //else
            cbColumn.DropDownStyle = ComboBoxStyle.DropDownList;
            cbColumn.Location = new Point(standard, 15 + ColumnsCount * 30);
            cbColumn.Text = aqc.Caption;
            cbColumn.SelectedIndexChanged += new EventHandler(cbColumn_SelectedIndexChanged);

            standard += cbColumn.Width + 5;
            ComboBox cbOperator = new ComboBox();
            Type cbType = GetDataType(aqc.Column);
            List<String> op = new List<string>();
            op.AddRange(new String[] { "=", "!=", ">", "<", ">=", "<=", "%**", "**%", "%%", "!%%", "<->", "!<->", "IN", "NOT IN" });
            if (!innerAnyQuery.DisplayAllOperator)
            {
                if (cbType == typeof(Char) || cbType == typeof(String))
                {
                    op.Clear();
                    //op.AddRange(new String[] { "=", "!=", "%**", "**%", "%%", "!%%", "IN", "NOT IN" });
                    op.AddRange(new String[] { "=", "!=", "%**", "**%", "%%", "!%%" });
                }
                else if (cbType != null && (cbType == typeof(int) || cbType == typeof(float) || cbType == typeof(double) || cbType == typeof(DateTime) || cbType.FullName == "System.Decimal" || cbType == typeof(Int16) || cbType == typeof(Int64)))
                {
                    op.Clear();
                    //op.AddRange(new String[] { "=", "!=", "<", ">", "<=", ">=", "<->", "!<->", "IN", "NOT IN" });
                    op.AddRange(new String[] { "=", "!=", "<", ">", "<=", ">=", "<->", "!<->" });
                }
            }
            cbOperator.Items.AddRange(op.ToArray());
            cbOperator.DropDownStyle = ComboBoxStyle.DropDownList;
            cbOperator.Name = ColumnsCount + "AnyQueryOperatorComboBox";
            cbOperator.Width = 65;
            cbOperator.Location = new Point(standard, 15 + ColumnsCount * 30);
            SetOperatorHelp(cbOperator);
            cbOperator.SelectedIndexChanged += new EventHandler(cbOperator_SelectedIndexChanged);
            this.panel2.Controls.Add(cbOperator);

            standard += cbOperator.Width + 5;
            switch (aqc.ColumnType)
            {
                case "AnyQueryTextBoxColumn":
                    TextBox tbValue = new TextBox();
                    tbValue.Name = ColumnsCount + "AnyQueryValue1TextBox" + aqc.Enabled.ToString();
                    tbValue.Text = value;
                    tbValue.Width = aqc.Width;
                    tbValue.Location = new Point(standard, 15 + ColumnsCount * 30);
                    tbValue.Enabled = aqc.Enabled;
                    tbValue.Enter += new EventHandler(tbValue_Enter);
                    tbValue.KeyPress += new KeyPressEventHandler(tbValue_KeyPress);
                    this.panel2.Controls.Add(tbValue);
                    break;
                case "AnyQueryComboBoxColumn":
                    cbOperator.Items.Clear();
                    op.Clear();
                    op.AddRange(new String[] { "=", "!=" });
                    cbOperator.Items.AddRange(op.ToArray());

                    InfoComboBox icbValue = new InfoComboBox();
                    if (aqc.InfoRefVal != null)
                    {
                        icbValue.DisplayMemberOnly = true;
                        if (aqc.InfoRefVal.SelectAlias != null && aqc.InfoRefVal.SelectAlias != String.Empty && aqc.InfoRefVal.SelectCommand != null && aqc.InfoRefVal.SelectCommand != String.Empty)
                        {
                            icbValue.SelectAlias = aqc.InfoRefVal.SelectAlias;
                            icbValue.SelectCommand = aqc.InfoRefVal.SelectCommand;
                        }
                        else
                        {
                            icbValue.DataSource = aqc.InfoRefVal.DataSource;
                        }
                        icbValue.DisplayMember = aqc.InfoRefVal.DisplayMember;
                        icbValue.ValueMember = aqc.InfoRefVal.ValueMember;
                        icbValue.EndInit();
                    }
                    else
                    {
                        icbValue.DisplayMemberOnly = false;
                        icbValue.Items.AddRange(aqc.Items);
                    }

                    icbValue.Name = ColumnsCount + "AnyQueryValue1InfoComboBox" + aqc.Enabled.ToString();
                    icbValue.Text = value;
                    icbValue.Width = aqc.Width;
                    icbValue.Location = new Point(standard, 15 + ColumnsCount * 30);
                    icbValue.Enabled = aqc.Enabled;
                    icbValue.Enter += new EventHandler(tbValue_Enter);
                    this.panel2.Controls.Add(icbValue);
                    break;
                case "AnyQueryCheckBoxColumn":
                    cbOperator.Items.Clear();
                    op.Clear();
                    op.AddRange(new String[] { "=", "!=" });
                    cbOperator.Items.AddRange(op.ToArray());

                    CheckBox cbValue = new CheckBox();
                    cbValue.Name = ColumnsCount + "AnyQueryValue1CheckBox" + aqc.Enabled.ToString();
                    if (aqc.DefaultValue == "1")
                        cbValue.Checked = true;
                    cbValue.Width = aqc.Width;
                    cbValue.Location = new Point(standard, 15 + ColumnsCount * 30);
                    cbValue.Enabled = aqc.Enabled;
                    cbValue.Enter += new EventHandler(tbValue_Enter);
                    this.panel2.Controls.Add(cbValue);
                    break;
                case "AnyQueryRefValColumn":
                    cbOperator.Items.Clear();
                    op.Clear();
                    op.AddRange(new String[] { "=", "!=", "IN", "NOT IN" });
                    cbOperator.Items.AddRange(op.ToArray());

                    if (aqc.Operator == "IN" || aqc.Operator == "NOT IN")
                    {
                        InfoTranslate it = new InfoTranslate();
                        if (aqc.InfoRefVal.SelectAlias != null && aqc.InfoRefVal.SelectCommand != null && aqc.InfoRefVal.SelectAlias != String.Empty && aqc.InfoRefVal.SelectCommand != String.Empty)
                        {
                            InfoDataSet idsRefButton = new InfoDataSet();
                            idsRefButton.RemoteName = "GLModule.cmdRefValUse";
                            idsRefButton.Execute(aqc.InfoRefVal.SelectCommand, CliUtils.fLoginDB, true);
                            InfoBindingSource ibsRefButton = new InfoBindingSource();
                            ibsRefButton.DataSource = idsRefButton;
                            ibsRefButton.DataMember = "cmdRefValUse";
                            it.BindingSource = ibsRefButton;
                        }
                        else
                        {
                            it.BindingSource = aqc.InfoRefVal.DataSource as InfoBindingSource;
                        }
                        TranslateRefReturnFields trrf = new TranslateRefReturnFields();
                        trrf.ColumnName = aqc.InfoRefVal.ValueMember;
                        trrf.DisplayColumnName = aqc.InfoRefVal.DisplayMember;
                        it.RefReturnFields.Add(trrf);

                        InfoRefbuttonBox aInfoRefbuttonBox = new InfoRefbuttonBox();
                        aInfoRefbuttonBox.Name = ColumnsCount + "AnyQueryValue1RefTextBox" + aqc.Enabled.ToString();
                        aInfoRefbuttonBox.Text = value;
                        aInfoRefbuttonBox.Location = new Point(standard, 15 + ColumnsCount * 30);
                        aInfoRefbuttonBox.Width = aqc.Width;
                        aInfoRefbuttonBox.Enabled = aqc.Enabled;
                        aInfoRefbuttonBox.Enter += new EventHandler(tbValue_Enter);
                        this.panel2.Controls.Add(aInfoRefbuttonBox);

                        //TextBox tbRefButton = new TextBox();
                        //tbRefButton.Name = ColumnsCount + "AnyQueryValue1RefTextBox" + aqc.Enabled.ToString();
                        //tbRefButton.Text = aqc.DefaultValue;
                        //tbRefButton.Width = aqc.Width;
                        //tbRefButton.Location = new Point(standard, 15 + ColumnsCount * 30);
                        //tbRefButton.Enabled = aqc.Enabled;
                        //tbRefButton.Enter += new EventHandler(tbValue_Enter);
                        //this.panel2.Controls.Add(tbRefButton);

                        InfoRefButton ifb = new InfoRefButton();
                        ifb.autoPanel = true;
                        ifb.infoTranslate = it;
                        RefButtonMatch rbm = new RefButtonMatch();
                        rbm.matchColumnName = aInfoRefbuttonBox.Name;
                        ifb.refButtonMatchs.Add(rbm);
                        ifb.Name = ColumnsCount + "AnyQueryValue1InfoRefButton" + aqc.Enabled.ToString();
                        ifb.Text = "...";
                        ifb.Width = 20;
                        ifb.Location = new Point(aInfoRefbuttonBox.Location.X + aInfoRefbuttonBox.Width + 2, 15 + ColumnsCount * 30);
                        //ifb.Enabled = aqc.Enabled;
                        this.panel2.Controls.Add(ifb);
                    }
                    else
                    {
                        cbOperator.Items.Remove("IN");
                        cbOperator.Items.Remove("NOT IN");

                        InfoRefvalBox irbValue = new InfoRefvalBox();
                        irbValue.Name = ColumnsCount + "AnyQueryValue1InfoRefvalBox" + aqc.Enabled.ToString();
                        irbValue.RefVal = aqc.InfoRefVal;
                        irbValue.TextBoxText = value;
                        irbValue.Width = aqc.Width;
                        irbValue.Location = new Point(standard, 15 + ColumnsCount * 30);
                        irbValue.TextBoxEnabled = aqc.Enabled;
                        irbValue.Enter += new EventHandler(tbValue_Enter);
                        this.panel2.Controls.Add(irbValue);
                    }
                    break;
                case "AnyQueryCalendarColumn":
                    InfoDateTimePicker idtpValue = new InfoDateTimePicker();
                    idtpValue.BeginInit();
                    idtpValue.Name = ColumnsCount + "AnyQueryValue1InfoDateTimePicker" + aqc.Enabled.ToString();
                    idtpValue.Text = value;
                    idtpValue.Width = aqc.Width;
                    idtpValue.Location = new Point(standard, 15 + ColumnsCount * 30);
                    idtpValue.Enabled = aqc.Enabled;
                    idtpValue.Format = DateTimePickerFormat.Long;
                    idtpValue.EndInit();
                    idtpValue.Enter += new EventHandler(tbValue_Enter);
                    this.panel2.Controls.Add(idtpValue);
                    break;
                case "AnyQueryRefButtonColumn":
                    cbOperator.Items.Clear();
                    op.Clear();
                    op.AddRange(new String[] { "IN", "NOT IN" });
                    cbOperator.Items.AddRange(op.ToArray());

                    InfoTranslate itRefButton = new InfoTranslate();
                    if (aqc.InfoRefVal.SelectAlias != null && aqc.InfoRefVal.SelectCommand != null && aqc.InfoRefVal.SelectAlias != String.Empty && aqc.InfoRefVal.SelectCommand != String.Empty)
                    {
                        InfoDataSet idsRefButton = new InfoDataSet();
                        idsRefButton.RemoteName = "GLModule.cmdRefValUse";
                        idsRefButton.Execute(aqc.InfoRefVal.SelectCommand, CliUtils.fLoginDB, true);
                        InfoBindingSource ibsRefButton = new InfoBindingSource();
                        ibsRefButton.DataSource = idsRefButton;
                        ibsRefButton.DataMember = "cmdRefValUse";
                        itRefButton.BindingSource = ibsRefButton;
                    }
                    else
                    {
                        itRefButton.BindingSource = aqc.InfoRefVal.DataSource as InfoBindingSource;
                    }
                    TranslateRefReturnFields trrf2 = new TranslateRefReturnFields();
                    trrf2.ColumnName = aqc.InfoRefVal.ValueMember;
                    trrf2.DisplayColumnName = aqc.InfoRefVal.DisplayMember;
                    itRefButton.RefReturnFields.Add(trrf2);

                    InfoRefbuttonBox aInfoRefbuttonBox2 = new InfoRefbuttonBox();
                    aInfoRefbuttonBox2.Name = ColumnsCount + "AnyQueryValue1RefTextBox" + aqc.Enabled.ToString();
                    aInfoRefbuttonBox2.Text = value;
                    aInfoRefbuttonBox2.Location = new Point(standard, 15 + ColumnsCount * 30);
                    aInfoRefbuttonBox2.Width = aqc.Width;
                    aInfoRefbuttonBox2.Enabled = aqc.Enabled;
                    aInfoRefbuttonBox2.Enter += new EventHandler(tbValue_Enter);
                    this.panel2.Controls.Add(aInfoRefbuttonBox2);

                    //TextBox tbRefButton2 = new TextBox();
                    //tbRefButton2.Name = ColumnsCount + "AnyQueryValue1RefTextBox" + aqc.Enabled.ToString();
                    //tbRefButton2.Text = aqc.DefaultValue;
                    //tbRefButton2.Width = aqc.Width;
                    //tbRefButton2.Location = new Point(standard, 15 + ColumnsCount * 30);
                    //tbRefButton2.Enabled = aqc.Enabled;
                    //tbRefButton2.Enter += new EventHandler(tbValue_Enter);
                    //this.panel2.Controls.Add(tbRefButton2);

                    InfoRefButton ifb2 = new InfoRefButton();
                    if (aqc.InfoRefButtonAutoPanel)
                        ifb2.autoPanel = true;
                    else
                    {
                        ifb2.autoPanel = false;
                        ifb2.panel = aqc.InfoRefButtonPanel;
                    }
                    ifb2.infoTranslate = itRefButton;
                    RefButtonMatch rbm2 = new RefButtonMatch();
                    rbm2.matchColumnName = aInfoRefbuttonBox2.Name;//rbm2.matchColumnName = tbRefButton2.Name;
                    ifb2.refButtonMatchs.Add(rbm2);
                    ifb2.Name = ColumnsCount + "AnyQueryValue1InfoRefButton" + aqc.Enabled.ToString();
                    ifb2.Text = "...";
                    ifb2.Width = 20;
                    ifb2.Location = new Point(aInfoRefbuttonBox2.Location.X + aInfoRefbuttonBox2.Width + 2, 15 + ColumnsCount * 30);//ifb2.Location = new Point(tbRefButton2.Location.X + tbRefButton2.Width + 2, 15 + ColumnsCount * 30);
                    //ifb2.Enabled = aqc.Enabled;
                    this.panel2.Controls.Add(ifb2);
                    break;
            }
            if (aqc.Operator == "%")
                cbOperator.Text = "**%";
            else
                cbOperator.Text = aqc.Operator;

            //if (aqc.Operator == "<->" || aqc.Operator == "!<->")
            //{
            //    standard += aqc.Width + 5;

            //    Label aLabel = new Label();
            //    aLabel.Name = ColumnsCount + "AnyQueryValue1to2Label";
            //    aLabel.Text = "～";
            //    aLabel.Width = 10;
            //    aLabel.Location = new Point(standard, 15 + ColumnsCount * 30);
            //    this.panel2.Controls.Add(aLabel);

            //    standard += aLabel.Width + 5;
            //    switch (aqc.ColumnType)
            //    {
            //        case "AnyQueryTextBoxColumn":
            //            TextBox tbValue = new TextBox();
            //            tbValue.Name = ColumnsCount + "AnyQueryValue2TextBox" + aqc.Enabled.ToString();
            //            tbValue.Text = aqc.DefaultValue;
            //            tbValue.Width = aqc.Width;
            //            tbValue.Location = new Point(standard, 15 + ColumnsCount * 30);
            //            tbValue.Enabled = aqc.Enabled;
            //            tbValue.Enter += new EventHandler(tbValue_Enter);
            //            this.panel2.Controls.Add(tbValue);
            //            break;
            //        case "AnyQueryComboBoxColumn":
            //            InfoComboBox icbValue = new InfoComboBox();
            //            icbValue.DisplayMemberOnly = true;
            //            if (aqc.InfoRefVal != null)
            //            {
            //                if (icbValue.SelectAlias != null && icbValue.SelectAlias != String.Empty && icbValue.SelectCommand != null && icbValue.SelectCommand != String.Empty)
            //                {
            //                    icbValue.SelectAlias = aqc.InfoRefVal.SelectAlias;
            //                    icbValue.SelectCommand = aqc.InfoRefVal.SelectCommand;
            //                }
            //                else
            //                {
            //                    icbValue.DataSource = aqc.InfoRefVal.DataSource;
            //                }
            //                icbValue.DisplayMember = aqc.InfoRefVal.DisplayMember;
            //                icbValue.ValueMember = aqc.InfoRefVal.ValueMember;
            //                icbValue.EndInit();
            //            }
            //            else
            //            {
            //                icbValue.Items.AddRange(aqc.Items);
            //            }

            //            icbValue.Name = ColumnsCount + "AnyQueryValue2InfoComboBox2" + aqc.Enabled.ToString();
            //            icbValue.Text = aqc.DefaultValue;
            //            icbValue.Width = aqc.Width;
            //            icbValue.Location = new Point(standard, 15 + ColumnsCount * 30);
            //            icbValue.Enabled = aqc.Enabled;
            //            icbValue.Enter += new EventHandler(tbValue_Enter);
            //            this.panel2.Controls.Add(icbValue);
            //            break;
            //        case "AnyQueryCheckBoxColumn":
            //            break;
            //        case "AnyQueryRefValColumn":
            //            InfoRefvalBox irbValue = new InfoRefvalBox();
            //            irbValue.Name = ColumnsCount + "AnyQueryValue2InfoRefvalBox2" + aqc.Enabled.ToString();
            //            irbValue.RefVal = aqc.InfoRefVal;
            //            irbValue.TextBoxText = aqc.DefaultValue;
            //            irbValue.Width = aqc.Width;
            //            irbValue.Location = new Point(standard, 15 + ColumnsCount * 30);
            //            irbValue.TextBoxEnabled = aqc.Enabled;
            //            irbValue.Enter += new EventHandler(tbValue_Enter);
            //            this.panel2.Controls.Add(irbValue);
            //            break;
            //        case "AnyQueryCalendarColumn":
            //            InfoDateTimePicker idtpValue = new InfoDateTimePicker();
            //            idtpValue.BeginInit();
            //            idtpValue.Name = ColumnsCount + "AnyQueryValue2InfoDateTimePicker2" + aqc.Enabled.ToString();
            //            idtpValue.Text = aqc.DefaultValue;
            //            idtpValue.Width = aqc.Width;
            //            idtpValue.Location = new Point(standard, 15 + ColumnsCount * 30);
            //            idtpValue.Enabled = aqc.Enabled;
            //            idtpValue.Format = DateTimePickerFormat.Long;
            //            idtpValue.EndInit();
            //            idtpValue.Enter += new EventHandler(tbValue_Enter);
            //            this.panel2.Controls.Add(idtpValue);
            //            break;
            //        case "AnyQueryRefButtonColumn"://<->不会用到RefButton
            //            //InfoTranslate itRefButton = new InfoTranslate();
            //            //if (aqc.InfoRefVal.SelectAlias != null && aqc.InfoRefVal.SelectCommand != null && aqc.InfoRefVal.SelectAlias != String.Empty && aqc.InfoRefVal.SelectCommand != String.Empty)
            //            //{
            //            //    InfoDataSet idsRefButton = new InfoDataSet();
            //            //    idsRefButton.RemoteName = "GLModule.cmdRefValUse";
            //            //    idsRefButton.Execute(aqc.InfoRefVal.SelectCommand, aqc.InfoRefVal.SelectAlias, true);
            //            //    InfoBindingSource ibsRefButton = new InfoBindingSource();
            //            //    ibsRefButton.DataSource = idsRefButton;
            //            //    ibsRefButton.DataMember = "cmdRefValUse";
            //            //    itRefButton.BindingSource = ibsRefButton;
            //            //}
            //            //else
            //            //{
            //            //    itRefButton.BindingSource = aqc.InfoRefVal.DataSource as InfoBindingSource;
            //            //}
            //            //TranslateRefReturnFields trrf2 = new TranslateRefReturnFields();
            //            //trrf2.ColumnName = aqc.InfoRefVal.ValueMember;
            //            //itRefButton.RefReturnFields.Add(trrf2);

            //            //TextBox tbRefButton2 = new TextBox();
            //            //tbRefButton2.Name = ColumnsCount + "AnyQueryValue1RefTextBox" + aqc.Enabled.ToString();
            //            //tbRefButton2.Text = aqc.DefaultValue;
            //            //tbRefButton2.Width = aqc.Width;
            //            //tbRefButton2.Location = new Point(standard, 15 + ColumnsCount * 30);
            //            //tbRefButton2.Enabled = aqc.Enabled;
            //            //this.panel2.Controls.Add(tbRefButton2);

            //            //InfoRefButton ifb2 = new InfoRefButton();
            //            //if (aqc.InfoRefButtonAutoPanel)
            //            //    ifb2.autoPanel = true;
            //            //else
            //            //{
            //            //    ifb2.autoPanel = false;
            //            //    ifb2.panel = aqc.InfoRefButtonPanel;
            //            //}
            //            //ifb2.infoTranslate = itRefButton;
            //            //RefButtonMatch rbm2 = new RefButtonMatch();
            //            //rbm2.matchColumnName = tbRefButton2.Name;
            //            //ifb2.refButtonMatchs.Add(rbm2);
            //            //ifb2.Name = ColumnsCount + "AnyQueryValue1InfoRefButton" + aqc.Enabled.ToString();
            //            //ifb2.Text = "...";
            //            //ifb2.Width = 20;
            //            //ifb2.Location = new Point(tbRefButton2.Location.X + tbRefButton2.Width + 2, 15 + ColumnsCount * 30);
            //            //ifb2.Enabled = aqc.Enabled;
            //            //this.panel2.Controls.Add(ifb2);
            //            break;
            //    }
            //}

            if (innerAnyQuery.AutoDisableColumns)
            {
                cbActive_CheckedChanged(cbActive, new EventArgs());
            }
        }

        void tbValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Convert.ToInt32(e.KeyChar) >= 32 && Convert.ToInt32(e.KeyChar) != 127)
            {
                int count = 0;
                String columnName = String.Empty;
                for (int i = 0; i < (sender as TextBox).Name.Length; i++)
                {
                    if (Char.IsDigit((sender as TextBox).Name[i]) && count >= i)
                    {
                        count = count * Convert.ToInt32(Math.Pow(10, i)) + Convert.ToInt32(Char.ToString((sender as TextBox).Name[i]));
                    }
                }

                foreach (Control c in this.panel2.Controls)
                {
                    if (c is ComboBox && (c as ComboBox).Name.StartsWith(count.ToString()))
                    {
                        columnName = (c as ComboBox).SelectedValue.ToString();
                        break;
                    }
                }

                InfoDataSet ids = innerAnyQuery.BindingSource.DataSource as InfoDataSet;
                Type columnType = null;
                if (ids != null)
                {
                    foreach (DataRow dr in aDataTable.Rows)
                    {
                        if (columnName == dr["FIELDNAME"].ToString())
                        {
                            columnType = (Type)dr["DATATYPE"];
                            break;
                        }
                    }
                }

                if (columnType == typeof(int) || columnType == typeof(uint) || columnType == typeof(byte) || columnType == typeof(Int16))
                {
                    if (Convert.ToInt32(e.KeyChar) < 48 || Convert.ToInt32(e.KeyChar) > 57)
                    {
                        e.KeyChar = Convert.ToChar(Keys.Escape);
                    }
                }
                else if (columnType == typeof(float) || columnType == typeof(double) || columnType == typeof(decimal))
                {
                    if ((Convert.ToInt32(e.KeyChar) < 48 || Convert.ToInt32(e.KeyChar) > 57) && Convert.ToInt32(e.KeyChar) != 46)
                    {
                        e.KeyChar = Convert.ToChar(Keys.Escape);
                    }
                }
            }
        }

        void tbValue_Enter(object sender, EventArgs e)
        {
            OnValueControlEdit(new EventArgs());
        }

        private void SetOperatorHelp(Control c)
        {
            String[] help = SysMsg.GetSystemMessage(CliUtils.fClientLang, "Srvtools", "AnyQuery", "OperatorHelp").Split(';');
            String strHelp = "=      ----    " + help[0] + "\n"
                           + "!=     ----    " + help[1] + "\n"
                           + ">      ----    " + help[2] + "\n"
                           + "<      ----    " + help[3] + "\n"
                           + ">=     ----    " + help[4] + "\n"
                           + "<=     ----    " + help[5] + "\n"
                           + "%**    ----    " + help[6] + "\n"
                           + "**%    ----    " + help[7] + "\n"
                           + "%%     ----    " + help[8] + "\n"
                           + "!%%    ----    " + help[9] + "\n"
                           + "<->    ----    " + help[10] + "\n"
                           + "!<->   ----    " + help[11] + "\n"
                           + "IN     ----    " + help[12] + "\n"
                           + "NOT IN ----    " + help[13];
            this.helpProvider1.SetHelpString(c, strHelp);
            this.helpProvider1.SetShowHelp(c, true);
        }

        void cbActive_CheckedChanged(object sender, EventArgs e)
        {
            if (innerAnyQuery.AutoDisableColumns)
            {
                int count = 0;
                for (int i = 0; i < (sender as CheckBox).Name.Length; i++)
                {
                    if (Char.IsDigit((sender as CheckBox).Name[i]))
                    {
                        count = count * Convert.ToInt16(Math.Pow(10, i)) + Convert.ToInt16(Char.ToString((sender as CheckBox).Name[i]));
                    }
                }

                foreach (Control c in this.panel2.Controls)
                {
                    if (c is Button)
                        continue;

                    if (c.Name.StartsWith(count + "AnyQueryValue"))
                    {
                        if (c.Name.EndsWith("False"))
                        {
                            if (c is InfoRefvalBox)
                                (c as InfoRefvalBox).TextBoxEnabled = false;
                            else
                                c.Enabled = false;
                        }
                        else
                        {
                            if ((sender as CheckBox).Checked)
                            {
                                if (c is InfoRefvalBox)
                                    (c as InfoRefvalBox).TextBoxEnabled = true;
                                else
                                    c.Enabled = true;
                            }
                            else
                            {
                                if (c is InfoRefvalBox)
                                    (c as InfoRefvalBox).TextBoxEnabled = false;
                                else
                                    c.Enabled = false;
                            }
                        }
                    }
                    else if (c.Name.StartsWith(count + "AnyQuery"))
                    {
                        if ((sender as CheckBox).Checked)
                        {
                            if (c is InfoRefvalBox)
                                (c as InfoRefvalBox).TextBoxEnabled = true;
                            else
                                c.Enabled = true;
                        }
                        else
                        {
                            if (c is InfoRefvalBox)
                                (c as InfoRefvalBox).TextBoxEnabled = false;
                            else
                                c.Enabled = false;
                        }
                    }
                }
                (sender as CheckBox).Enabled = true;
            }
        }

        void cbColumn_SelectedIndexChanged(object sender, EventArgs e)
        {
            int count = 0;
            for (int i = 0; i < (sender as ComboBox).Name.Length; i++)
            {
                if (Char.IsDigit((sender as ComboBox).Name[i]))
                {
                    count = count * Convert.ToInt16(Math.Pow(10, i)) + Convert.ToInt16(Char.ToString((sender as ComboBox).Name[i]));
                }
            }

            if (count > 0)
            {
                bool flag = false;
                AnyQueryColumns aqcTemp = new AnyQueryColumns();
                InfoDataSet ids = null;
                if (innerAnyQuery.BindingSource != null)
                {
                    ids = innerAnyQuery.BindingSource.DataSource as InfoDataSet;
                    foreach (AnyQueryColumns aqc in innerAnyQuery.Columns)
                    {
                        if (aqc.Caption == (sender as ComboBox).Text)
                        {
                            aqcTemp = aqc;
                            flag = true;
                            break;
                        }
                    }
                }

                if (flag)
                {
                    ArrayList arr = new ArrayList();
                    foreach (Control c in this.panel2.Controls)
                    {
                        if (c.Name.StartsWith(count + "AnyQuery"))
                        {
                            arr.Add(c);
                        }
                    }
                    for (int i = 0; i < arr.Count; i++)
                        this.panel2.Controls.Remove(arr[i] as Control);

                    CreateColumns(ids, aqcTemp, count, true);
                }
                else
                {
                    ArrayList arr = new ArrayList();
                    foreach (Control c in this.panel2.Controls)
                    {
                        if (c.Name.StartsWith(count + "AnyQuery"))
                        {
                            arr.Add(c);
                        }
                    }
                    for (int i = 0; i < arr.Count; i++)
                        this.panel2.Controls.Remove(arr[i] as Control);

                    aqcTemp.Caption = (sender as ComboBox).Text;
                    aqcTemp.Column = CaptionToColumn(aqcTemp.Caption, innerAnyQuery.BindingSource);
                    Type aqcType = GetDataType(aqcTemp.Column);
                    if (aqcType == typeof(DateTime))
                        aqcTemp.ColumnType = "AnyQueryCalendarColumn";
                    else
                        aqcTemp.ColumnType = "AnyQueryTextBoxColumn";
                    CreateColumns(ids, aqcTemp, count, true);
                }
            }
        }

        void cbOperator_SelectedIndexChanged(object sender, EventArgs e)
        {
            int count = 0;
            bool flag = false;
            object cValue1 = null;
            object cValue2 = null;
            Label cLabel = null;
            for (int i = 0; i < (sender as ComboBox).Name.Length; i++)
            {
                if (Char.IsDigit((sender as ComboBox).Name[i]))
                {
                    count = count * Convert.ToInt16(Math.Pow(10, i)) + Convert.ToInt16(Char.ToString((sender as ComboBox).Name[i]));
                }
            }

            if (count > 0)
            {
                foreach (Control c in this.panel2.Controls)
                {
                    if (c.Name.StartsWith(count + "AnyQueryValue1to2Label"))
                    {
                        cLabel = c as Label;
                        continue;
                    }

                    if (c.Name.StartsWith(count + "AnyQueryValue1"))
                    {
                        cValue1 = c;
                        continue;
                    }

                    if (c.Name.StartsWith(count + "AnyQueryValue2"))
                    {
                        flag = true;
                        cValue2 = c;
                        continue;
                    }
                }
            }

            if ((sender as ComboBox).Text == "<->" || (sender as ComboBox).Text == "!<->")
            {
                if (!flag)
                {
                    int standard = (cValue1 as Control).Location.X + (cValue1 as Control).Width + 10;

                    Label aLabel = new Label();
                    aLabel.Name = count + "AnyQueryValue1to2Label";
                    aLabel.Text = "～";
                    aLabel.Width = 10;
                    aLabel.Location = new Point(standard, 15 + count * 30);
                    this.panel2.Controls.Add(aLabel);

                    standard += aLabel.Width + 10;
                    if (cValue1 is TextBox)
                    {
                        cValue2 = new TextBox();
                        (cValue2 as TextBox).Name = count + "AnyQueryValue2TextBox";
                        (cValue2 as TextBox).Text = (cValue1 as TextBox).Text;
                        (cValue2 as TextBox).Width = (cValue1 as TextBox).Width;
                        (cValue2 as TextBox).Location = new Point(standard, (cValue1 as TextBox).Location.Y);
                        (cValue2 as TextBox).Enter += new EventHandler(tbValue_Enter);
                        (cValue2 as TextBox).KeyPress += new KeyPressEventHandler(tbValue_KeyPress);
                        this.panel2.Controls.Add((cValue2 as TextBox));
                    }
                    else if (cValue1 is InfoComboBox)
                    {
                        cValue2 = new InfoComboBox();
                        (cValue2 as InfoComboBox).SelectAlias = (cValue1 as InfoComboBox).SelectAlias;
                        (cValue2 as InfoComboBox).SelectCommand = (cValue1 as InfoComboBox).SelectCommand;
                        (cValue2 as InfoComboBox).DisplayMember = (cValue1 as InfoComboBox).DisplayMember;
                        (cValue2 as InfoComboBox).ValueMember = (cValue1 as InfoComboBox).ValueMember;
                        (cValue2 as InfoComboBox).EndInit();

                        (cValue2 as InfoComboBox).DisplayMemberOnly = (cValue1 as InfoComboBox).DisplayMemberOnly; ;
                        (cValue2 as InfoComboBox).Name = count + "AnyQueryValue2InfoComboBox2";
                        (cValue2 as InfoComboBox).Text = (cValue1 as InfoComboBox).Text;
                        (cValue2 as InfoComboBox).Width = (cValue1 as InfoComboBox).Width;
                        (cValue2 as InfoComboBox).Location = new Point(standard, (cValue1 as InfoComboBox).Location.Y);
                        this.panel2.Controls.Add((cValue2 as InfoComboBox));
                    }
                    else if (cValue1 is CheckBox)
                    {

                    }
                    else if (cValue1 is InfoRefvalBox)
                    {
                        cValue2 = new InfoRefvalBox();
                        (cValue2 as InfoRefvalBox).Name = count + "AnyQueryValue2InfoRefvalBox2";
                        (cValue2 as InfoRefvalBox).RefVal = (cValue1 as InfoRefvalBox).RefVal;
                        (cValue2 as InfoRefvalBox).TextBoxText = (cValue1 as InfoRefvalBox).TextBoxText;
                        (cValue2 as InfoRefvalBox).Width = (cValue1 as InfoRefvalBox).Width;
                        (cValue2 as InfoRefvalBox).Location = new Point(standard, (cValue1 as InfoRefvalBox).Location.Y);
                        this.panel2.Controls.Add((cValue2 as InfoRefvalBox));
                    }
                    else if (cValue1 is InfoDateTimePicker)
                    {
                        cValue2 = new InfoDateTimePicker();
                        (cValue2 as InfoDateTimePicker).BeginInit();
                        (cValue2 as InfoDateTimePicker).Name = count + "AnyQueryValue2InfoDateTimePicker2";
                        (cValue2 as InfoDateTimePicker).Text = (cValue1 as InfoDateTimePicker).Text;
                        (cValue2 as InfoDateTimePicker).Width = (cValue1 as InfoDateTimePicker).Width;
                        (cValue2 as InfoDateTimePicker).Location = new Point(standard, (cValue1 as InfoDateTimePicker).Location.Y);
                        (cValue2 as InfoDateTimePicker).Format = DateTimePickerFormat.Long;
                        (cValue2 as InfoDateTimePicker).EndInit();
                        this.panel2.Controls.Add((cValue2 as InfoDateTimePicker));
                    }
                }
            }
            else
            {
                if (flag)
                {
                    this.panel2.Controls.Remove((cValue2 as Control));
                    this.panel2.Controls.Remove(cLabel);
                }
            }
        }

        void labelCondition_Click(object sender, EventArgs e)
        {
            if ((sender as Label).Text == "AND")
                (sender as Label).Text = "OR";
            else
                (sender as Label).Text = "AND";
        }

        private String CreateColumns(InfoDataSet ids, XmlNode xn, List<Panel> panels)
        {
            int columnWidth = Convert.ToInt16(xn.Attributes["ColumnWidth"].Value);
            int ColumnsCount = Count;
            if (120 + ColumnsCount * 30 > this.Height) this.Height += 30;

            int standard = 40;
            CheckBox cbActive = new CheckBox();
            cbActive.Name = ColumnsCount + "AnyQueryActiveCheckBox";
            if (xn.Attributes["IsActive"].Value == "1")
                cbActive.Checked = true;
            cbActive.Width = 15;
            cbActive.Location = new Point(standard, 13 + ColumnsCount * 30);
            cbActive.CheckedChanged += new EventHandler(cbActive_CheckedChanged);
            this.panel2.Controls.Add(cbActive);

            //standard += 30;
            //Label labelCondition = new Label();
            //labelCondition.Name = ColumnsCount + "AnyQueryConditionLabel";
            //labelCondition.Width = 50;
            //labelCondition.Text = xn.Attributes["Condition"].Value;
            //labelCondition.Location = new Point(standard, 17 + ColumnsCount * 30);
            //labelCondition.Click += new EventHandler(labelCondition_Click);
            //this.panel2.Controls.Add(labelCondition);

            standard += 30;
            ComboBox cbColumn = new ComboBox();
            cbColumn.Name = ColumnsCount + "AnyQueryColumnComboBox";
            this.panel2.Controls.Add(cbColumn);
            DataView dvColumn = CreateDataViewColumn(ids);
            cbColumn.DataSource = dvColumn;
            cbColumn.DisplayMember = "CAPTION";
            cbColumn.ValueMember = "FIELDNAME";
            cbColumn.DropDownStyle = ComboBoxStyle.DropDownList;
            cbColumn.Width = columnWidth;
            cbColumn.Location = new Point(standard, 15 + ColumnsCount * 30);
            cbColumn.Text = xn.Attributes["Caption"].Value;
            cbColumn.SelectedIndexChanged += new EventHandler(cbColumn_SelectedIndexChanged);

            standard += cbColumn.Width + 5;
            ComboBox cbOperator = new ComboBox();
            Type cbType = GetDataType(CaptionToColumn(xn.Attributes["Caption"].Value, innerAnyQuery.BindingSource));
            List<String> op = new List<string>();
            op.AddRange(new String[] { "=", "!=", ">", "<", ">=", "<=", "%**", "**%", "%%", "!%%", "<->", "!<->", "IN", "NOT IN" });
            if (!innerAnyQuery.DisplayAllOperator)
            {
                if (cbType == typeof(Char) || cbType == typeof(String))
                {
                    op.Clear();
                    //op.AddRange(new String[] { "=", "!=", "%**", "**%", "%%", "!%%", "IN", "NOT IN" });
                    op.AddRange(new String[] { "=", "!=", "%**", "**%", "%%", "!%%" });
                }
                else if (cbType != null && (cbType == typeof(int) || cbType == typeof(float) || cbType == typeof(double) || cbType == typeof(DateTime) || cbType.FullName == "System.Decimal" || cbType == typeof(Int16) || cbType == typeof(Int64)))
                {
                    op.Clear();
                    //op.AddRange(new String[] { "=", "!=", "<", ">", "<=", ">=", "<->", "!<->", "IN", "NOT IN" });
                    op.AddRange(new String[] { "=", "!=", "<", ">", "<=", ">=", "<->", "!<->" });
                }
            }
            cbOperator.Items.AddRange(op.ToArray());
            cbOperator.DropDownStyle = ComboBoxStyle.DropDownList;
            cbOperator.Name = ColumnsCount + "AnyQueryOperatorComboBox";
            cbOperator.Width = 65;
            cbOperator.Location = new Point(standard, 15 + ColumnsCount * 30);
            SetOperatorHelp(cbOperator);
            cbOperator.SelectedIndexChanged += new EventHandler(cbOperator_SelectedIndexChanged);
            this.panel2.Controls.Add(cbOperator);

            standard += cbOperator.Width + 5;
            switch (xn.Attributes["ValueType"].Value)
            {
                case "AnyQueryTextBoxColumn":
                    TextBox tbValue = new TextBox();
                    tbValue.Name = ColumnsCount + "AnyQueryValue1TextBox" + xn.Attributes["ValueEnabled"].Value;
                    tbValue.Text = xn.Attributes["Value1"].Value;
                    tbValue.Width = Convert.ToInt16(xn.Attributes["Width"].Value);
                    tbValue.Location = new Point(standard, 15 + ColumnsCount * 30);
                    tbValue.Enabled = Convert.ToBoolean(xn.Attributes["Enabled"].Value);
                    tbValue.Enter += new EventHandler(tbValue_Enter);
                    this.panel2.Controls.Add(tbValue);
                    break;
                case "AnyQueryComboBoxColumn":
                    cbOperator.Items.Clear();
                    op.Clear();
                    op.AddRange(new String[] { "=", "!=" });
                    cbOperator.Items.AddRange(op.ToArray());

                    InfoComboBox icbValue = new InfoComboBox();
                    if (xn.Attributes["SelectAlias"].Value == String.Empty || xn.Attributes["SelectCommand"].Value == String.Empty)
                    {
                        if (xn.Attributes["RemoteName"].Value == String.Empty)
                        {
                            String[] temp = xn.Attributes["Items"].Value.Split(';');
                            foreach (String str in temp)
                            {
                                if (str != String.Empty)
                                    icbValue.Items.Add(str);
                            }
                        }
                        else
                        {
                            InfoDataSet idsComboBox = new InfoDataSet();
                            idsComboBox.RemoteName = xn.Attributes["RemoteName"].Value;
                            idsComboBox.Active = true;
                            InfoBindingSource ibsComboBox = new InfoBindingSource();
                            ibsComboBox.DataSource = idsComboBox;
                            ibsComboBox.DataMember = idsComboBox.RealDataSet.Tables[0].TableName;

                            icbValue.DataSource = ibsComboBox;
                            icbValue.DisplayMember = xn.Attributes["DisplayMember"].Value;
                            icbValue.ValueMember = xn.Attributes["ValueMember"].Value;
                            icbValue.EndInit();
                        }
                    }
                    else
                    {
                        icbValue.SelectAlias = xn.Attributes["SelectAlias"].Value;
                        icbValue.SelectCommand = xn.Attributes["SelectCommand"].Value;
                        icbValue.DisplayMember = xn.Attributes["DisplayMember"].Value;
                        icbValue.ValueMember = xn.Attributes["ValueMember"].Value;
                        icbValue.EndInit();
                    }

                    icbValue.DisplayMemberOnly = Convert.ToBoolean(xn.Attributes["DisplayMemberOnly"].Value);
                    icbValue.Name = ColumnsCount + "AnyQueryValue1InfoComboBox" + xn.Attributes["ValueEnabled"].Value;
                    icbValue.Width = Convert.ToInt16(xn.Attributes["Width"].Value);
                    icbValue.Location = new Point(standard, 15 + ColumnsCount * 30);
                    icbValue.Enabled = Convert.ToBoolean(xn.Attributes["Enabled"].Value);
                    icbValue.Enter += new EventHandler(tbValue_Enter);
                    this.panel2.Controls.Add(icbValue);
                    icbValue.SelectedValue = xn.Attributes["Value1"].Value;
                    icbValue.Text = xn.Attributes["ValueReal"].Value;
                    break;
                case "AnyQueryCheckBoxColumn":
                    cbOperator.Items.Clear();
                    op.Clear();
                    op.AddRange(new String[] { "=", "!=" });
                    cbOperator.Items.AddRange(op.ToArray());

                    CheckBox cbValue = new CheckBox();
                    cbValue.Name = ColumnsCount + "AnyQueryValue1CheckBox" + xn.Attributes["ValueEnabled"].Value;
                    if (xn.Attributes["Value1"].Value == "1")
                        cbValue.Checked = true;
                    cbValue.Width = Convert.ToInt16(xn.Attributes["Width"].Value);
                    cbValue.Location = new Point(standard, 15 + ColumnsCount * 30);
                    cbValue.Enabled = Convert.ToBoolean(xn.Attributes["Enabled"].Value);
                    cbValue.Enter += new EventHandler(tbValue_Enter);
                    this.panel2.Controls.Add(cbValue);
                    break;
                case "AnyQueryRefValColumn":
                    cbOperator.Items.Clear();
                    op.Clear();
                    op.AddRange(new String[] { "=", "!=", "IN", "NOT IN" });
                    cbOperator.Items.AddRange(op.ToArray());

                    if (xn.Attributes["Operator"].Value == "IN" || xn.Attributes["Operator"].Value == "NOT IN")
                    {
                        InfoDataSet idsRefButton = new InfoDataSet();
                        idsRefButton.RemoteName = "GLModule.cmdRefValUse";
                        idsRefButton.Execute(xn.Attributes["SelectCommand"].Value, CliUtils.fLoginDB, true);

                        InfoBindingSource ibsRefButton = new InfoBindingSource();
                        ibsRefButton.DataSource = idsRefButton;
                        ibsRefButton.DataMember = "cmdRefValUse";
                        InfoTranslate it = new InfoTranslate();
                        it.BindingSource = ibsRefButton;
                        TranslateRefReturnFields trrf = new TranslateRefReturnFields();
                        trrf.ColumnName = xn.Attributes["ValueMember"].Value;
                        trrf.DisplayColumnName = xn.Attributes["DisplayMember"].Value;
                        it.RefReturnFields.Add(trrf);

                        InfoRefbuttonBox aInfoRefbuttonBox = new InfoRefbuttonBox();
                        aInfoRefbuttonBox.Name = ColumnsCount + "AnyQueryValue1RefTextBox" + xn.Attributes["ValueEnabled"].Value;
                        aInfoRefbuttonBox.Text = xn.Attributes["Value1"].Value;
                        aInfoRefbuttonBox.RealValue = xn.Attributes["ValueReal"].Value;
                        aInfoRefbuttonBox.Location = new Point(standard, 15 + ColumnsCount * 30);
                        aInfoRefbuttonBox.Width = Convert.ToInt16(xn.Attributes["Width"].Value); ;
                        aInfoRefbuttonBox.Enabled = Convert.ToBoolean(xn.Attributes["Enabled"].Value);
                        aInfoRefbuttonBox.Enter += new EventHandler(tbValue_Enter);
                        this.panel2.Controls.Add(aInfoRefbuttonBox);

                        //TextBox tbRefButton = new TextBox();
                        //tbRefButton.Name = ColumnsCount + "AnyQueryValue1RefTextBox" + xn.Attributes["ValueEnabled"].Value;
                        //tbRefButton.Text = xn.Attributes["Value1"].Value;
                        //tbRefButton.AccessibleName = xn.Attributes["ValueReal"].Value;
                        //tbRefButton.Width = Convert.ToInt16(xn.Attributes["Width"].Value);
                        //tbRefButton.Location = new Point(standard, 15 + ColumnsCount * 30);
                        //tbRefButton.Enabled = Convert.ToBoolean(xn.Attributes["Enabled"].Value);
                        //tbRefButton.Enter += new EventHandler(tbValue_Enter);
                        //this.panel2.Controls.Add(tbRefButton);

                        InfoRefButton ifb = new InfoRefButton();
                        ifb.autoPanel = true;
                        ifb.infoTranslate = it;
                        RefButtonMatch rbm = new RefButtonMatch();
                        rbm.matchColumnName = aInfoRefbuttonBox.Name;
                        ifb.refButtonMatchs.Add(rbm);
                        ifb.Name = ColumnsCount + "AnyQueryValue1InfoRefButton" + xn.Attributes["ValueEnabled"].Value;
                        ifb.Text = "...";
                        ifb.Width = 20;
                        ifb.Location = new Point(aInfoRefbuttonBox.Location.X + aInfoRefbuttonBox.Width + 2, 15 + ColumnsCount * 30);
                        //ifb.Enabled = Convert.ToBoolean(xn.Attributes["Enabled"].Value);
                        this.panel2.Controls.Add(ifb);
                    }
                    else
                    {
                        cbOperator.Items.Remove("IN");
                        cbOperator.Items.Remove("NOT IN");

                        InfoRefvalBox irbValue = new InfoRefvalBox();
                        InfoRefVal aInfoRefVal = new InfoRefVal();
                        if (xn.Attributes["SelectAlias"].Value != String.Empty && xn.Attributes["SelectCommand"].Value != String.Empty)
                        {
                            aInfoRefVal.SelectAlias = xn.Attributes["SelectAlias"].Value;
                            aInfoRefVal.SelectCommand = xn.Attributes["SelectCommand"].Value;
                        }
                        else
                        {
                            InfoDataSet idsRefVal = new InfoDataSet();
                            idsRefVal.RemoteName = xn.Attributes["RemoteName"].Value;
                            idsRefVal.Active = true;
                            InfoBindingSource ibsRefVal = new InfoBindingSource();
                            ibsRefVal.DataSource = idsRefVal;
                            ibsRefVal.DataMember = idsRefVal.RealDataSet.Tables[0].TableName;

                            aInfoRefVal.DataSource = ibsRefVal;
                        }
                        aInfoRefVal.DisplayMember = xn.Attributes["DisplayMember"].Value;
                        aInfoRefVal.ValueMember = xn.Attributes["ValueMember"].Value;
                        aInfoRefVal.EndInit();

                        irbValue.Name = ColumnsCount + "AnyQueryValue1InfoRefvalBox" + xn.Attributes["ValueEnabled"].Value;
                        irbValue.RefVal = aInfoRefVal;
                        irbValue.TextBoxText = xn.Attributes["Value1"].Value;
                        irbValue.Width = Convert.ToInt16(xn.Attributes["Width"].Value);
                        irbValue.Location = new Point(standard, 15 + ColumnsCount * 30);
                        irbValue.TextBoxEnabled = Convert.ToBoolean(xn.Attributes["Enabled"].Value);
                        irbValue.TextBoxSelectedValue = xn.Attributes["ValueReal"].Value;
                        irbValue.Enter += new EventHandler(tbValue_Enter);
                        this.panel2.Controls.Add(irbValue);
                    }
                    break;
                case "AnyQueryCalendarColumn":
                    InfoDateTimePicker idtpValue = new InfoDateTimePicker();
                    idtpValue.BeginInit();
                    idtpValue.Name = ColumnsCount + "AnyQueryValue1InfoDateTimePicker" + xn.Attributes["ValueEnabled"].Value;
                    idtpValue.Text = xn.Attributes["Value1"].Value;
                    idtpValue.Width = Convert.ToInt16(xn.Attributes["Width"].Value);
                    idtpValue.Location = new Point(standard, 15 + ColumnsCount * 30);
                    idtpValue.Enabled = Convert.ToBoolean(xn.Attributes["Enabled"].Value);
                    idtpValue.Format = DateTimePickerFormat.Long;
                    idtpValue.EndInit();
                    idtpValue.Enter += new EventHandler(tbValue_Enter);
                    this.panel2.Controls.Add(idtpValue);
                    break;
                case "AnyQueryRefButtonColumn":
                    cbOperator.Items.Clear();
                    op.Clear();
                    op.AddRange(new String[] { "IN", "NOT IN" });
                    cbOperator.Items.AddRange(op.ToArray());

                    InfoDataSet idsRefButton2 = new InfoDataSet();
                    if (xn.Attributes["RemoteName"].Value == "GLModule.cmdRefValUse")
                    {
                        idsRefButton2.RemoteName = "GLModule.cmdRefValUse";
                        idsRefButton2.Execute(xn.Attributes["SelectCommand"].Value, CliUtils.fLoginDB, true);
                    }
                    else
                    {
                        idsRefButton2.RemoteName = xn.Attributes["RemoteName"].Value;
                        idsRefButton2.Active = true;
                    }

                    InfoBindingSource ibsRefButton2 = new InfoBindingSource();
                    ibsRefButton2.DataSource = idsRefButton2;
                    if (xn.Attributes["RemoteName"].Value != null)
                        ibsRefButton2.DataMember = idsRefButton2.RealDataSet.Tables[0].TableName;
                    else
                        ibsRefButton2.DataMember = "cmdRefValUse";

                    InfoTranslate it2 = new InfoTranslate();
                    it2.BindingSource = ibsRefButton2;
                    TranslateRefReturnFields trrf2 = new TranslateRefReturnFields();
                    trrf2.ColumnName = xn.Attributes["ValueMember"].Value;
                    trrf2.DisplayColumnName = xn.Attributes["DisplayMember"].Value;
                    it2.RefReturnFields.Add(trrf2);

                    InfoRefbuttonBox aInfoRefbuttonBox2 = new InfoRefbuttonBox();
                    aInfoRefbuttonBox2.Name = ColumnsCount + "AnyQueryValue1RefTextBox" + xn.Attributes["ValueEnabled"].Value;
                    aInfoRefbuttonBox2.Text = xn.Attributes["Value1"].Value;
                    aInfoRefbuttonBox2.RealValue = xn.Attributes["ValueReal"].Value;
                    aInfoRefbuttonBox2.Location = new Point(standard, 15 + ColumnsCount * 30);
                    aInfoRefbuttonBox2.Width = Convert.ToInt16(xn.Attributes["Width"].Value); ;
                    aInfoRefbuttonBox2.Enabled = Convert.ToBoolean(xn.Attributes["Enabled"].Value);
                    aInfoRefbuttonBox2.Enter += new EventHandler(tbValue_Enter);
                    this.panel2.Controls.Add(aInfoRefbuttonBox2);

                    //TextBox tbRefButton2 = new TextBox();
                    //tbRefButton2.Name = ColumnsCount + "AnyQueryValue1RefTextBox" + xn.Attributes["ValueEnabled"].Value;
                    //tbRefButton2.Text = xn.Attributes["Value1"].Value;
                    //tbRefButton2.AccessibleName = xn.Attributes["ValueReal"].Value;
                    //tbRefButton2.Width = Convert.ToInt16(xn.Attributes["Width"].Value);
                    //tbRefButton2.Location = new Point(standard, 15 + ColumnsCount * 30);
                    //tbRefButton2.Enabled = Convert.ToBoolean(xn.Attributes["Enabled"].Value);
                    //tbRefButton2.Enter += new EventHandler(tbValue_Enter);
                    //this.panel2.Controls.Add(tbRefButton2);

                    InfoRefButton ifb2 = new InfoRefButton();
                    ifb2.autoPanel = Convert.ToBoolean(xn.Attributes["AutoPanel"].Value);
                    if (!ifb2.autoPanel)
                    {
                        Control[] innerPanel = (this.innerAnyQuery.OwnerComp as InfoForm).Controls.Find(xn.Attributes["Panel"].Value, true);
                        if (innerPanel.Length > 0)
                        {
                            ifb2.panel = innerPanel[0] as Panel;
                        }

                        if (ifb2.panel == null)
                        {
                            for (int i = 0; i < panels.Count; i++)
                            {
                                if (panels[i] == null) continue;
                                ifb2.panel = panels[i];
                            }

                            if (ifb2.panel == null)
                            {
                                MessageBox.Show("Can not find the Panel '" + xn.Attributes["Panel"].Value + "' in your Form.");
                                return "AND";
                            }
                        }
                    }
                    ifb2.infoTranslate = it2;
                    RefButtonMatch rbm2 = new RefButtonMatch();
                    rbm2.matchColumnName = aInfoRefbuttonBox2.Name;
                    ifb2.refButtonMatchs.Add(rbm2);
                    ifb2.Name = ColumnsCount + "AnyQueryValue1InfoRefButton" + xn.Attributes["ValueEnabled"].Value;
                    ifb2.Text = "...";
                    ifb2.Width = 20;
                    ifb2.Location = new Point(aInfoRefbuttonBox2.Location.X + aInfoRefbuttonBox2.Width + 2, 15 + ColumnsCount * 30);
                    //ifb2.Enabled = Convert.ToBoolean(xn.Attributes["Enabled"].Value);
                    this.panel2.Controls.Add(ifb2);
                    break;
            }
            cbOperator.SelectedIndexChanged -= new EventHandler(cbOperator_SelectedIndexChanged);
            cbOperator.Text = xn.Attributes["Operator"].Value;
            cbOperator.SelectedIndexChanged += new EventHandler(cbOperator_SelectedIndexChanged);

            if (xn.Attributes["Operator"].Value == "<->" || xn.Attributes["Operator"].Value == "!<->")
            {
                standard += Convert.ToInt16(xn.Attributes["Width"].Value) + 5;

                Label aLabel = new Label();
                aLabel.Name = ColumnsCount + "AnyQueryValue1to2Label";
                aLabel.Text = "～";
                aLabel.Width = 10;
                aLabel.Location = new Point(standard, 15 + ColumnsCount * 30);
                this.panel2.Controls.Add(aLabel);

                standard += aLabel.Width + 5;
                switch (xn.Attributes["ValueType"].Value)
                {
                    case "AnyQueryTextBoxColumn":
                        TextBox tbValue = new TextBox();
                        tbValue.Name = ColumnsCount + "AnyQueryValue2TextBox2" + xn.Attributes["ValueEnabled"].Value;
                        tbValue.Text = xn.Attributes["Value2"].Value;
                        tbValue.Width = Convert.ToInt16(xn.Attributes["Width"].Value);
                        tbValue.Location = new Point(standard, 15 + ColumnsCount * 30);
                        tbValue.Enabled = Convert.ToBoolean(xn.Attributes["Enabled"].Value);
                        tbValue.Enter += new EventHandler(tbValue_Enter);
                        this.panel2.Controls.Add(tbValue);
                        break;
                    case "AnyQueryComboBoxColumn":
                        InfoComboBox icbValue = new InfoComboBox();
                        icbValue.DisplayMemberOnly = true;
                        if (xn.Attributes["SelectAlias"].Value == String.Empty || xn.Attributes["SelectCommand"].Value == String.Empty)
                        {

                            if (xn.Attributes["DataSource"].Value == String.Empty)
                            {
                                String[] temp = xn.Attributes["Items"].Value.Split(';');
                                foreach (String str in temp)
                                {
                                    if (str != String.Empty)
                                        icbValue.Items.Add(str);
                                }
                            }
                            else
                            {
                                InfoDataSet idsComboBox = new InfoDataSet();
                                idsComboBox.RemoteName = xn.Attributes["RemoteName"].Value;
                                idsComboBox.Active = true;
                                InfoBindingSource ibsComboBox = new InfoBindingSource();
                                ibsComboBox.DataSource = idsComboBox;
                                ibsComboBox.DataMember = idsComboBox.RealDataSet.Tables[0].TableName;

                                icbValue.DataSource = ibsComboBox;
                                icbValue.DisplayMember = xn.Attributes["DisplayMember"].Value;
                                icbValue.ValueMember = xn.Attributes["ValueMember"].Value;
                                icbValue.EndInit();
                            }
                        }
                        else
                        {
                            icbValue.SelectAlias = xn.Attributes["SelectAlias"].Value;
                            icbValue.SelectCommand = xn.Attributes["SelectCommand"].Value;
                            icbValue.DisplayMember = xn.Attributes["DisplayMember"].Value;
                            icbValue.ValueMember = xn.Attributes["ValueMember"].Value;
                            icbValue.EndInit();
                        }

                        icbValue.Name = ColumnsCount + "AnyQueryValue1InfoComboBox" + xn.Attributes["ValueEnabled"].Value;
                        icbValue.Text = xn.Attributes["Value1"].Value;
                        icbValue.Width = Convert.ToInt16(xn.Attributes["Width"].Value);
                        icbValue.Location = new Point(standard, 15 + ColumnsCount * 30);
                        icbValue.Enabled = Convert.ToBoolean(xn.Attributes["Enabled"].Value);
                        icbValue.Enter += new EventHandler(tbValue_Enter);
                        this.panel2.Controls.Add(icbValue);
                        break;
                    case "AnyQueryCheckBoxColumn":
                        break;
                    case "AnyQueryRefValColumn":
                        InfoRefvalBox irbValue = new InfoRefvalBox();
                        InfoRefVal aInfoRefVal = new InfoRefVal();
                        aInfoRefVal.SelectAlias = xn.Attributes["SelectAlias"].Value;
                        aInfoRefVal.SelectCommand = xn.Attributes["SelectCommand"].Value;
                        aInfoRefVal.DisplayMember = xn.Attributes["DisplayMember"].Value;
                        aInfoRefVal.ValueMember = xn.Attributes["ValueMember"].Value;
                        aInfoRefVal.EndInit();

                        irbValue.Name = ColumnsCount + "AnyQueryValue2InfoRefvalBox2" + xn.Attributes["ValueEnabled"].Value;
                        irbValue.RefVal = aInfoRefVal;
                        irbValue.TextBoxText = xn.Attributes["Value2"].Value;
                        irbValue.Width = Convert.ToInt16(xn.Attributes["Width"].Value);
                        irbValue.Location = new Point(standard, 15 + ColumnsCount * 30);
                        irbValue.TextBoxEnabled = Convert.ToBoolean(xn.Attributes["Enabled"].Value);
                        irbValue.Enter += new EventHandler(tbValue_Enter);
                        this.panel2.Controls.Add(irbValue);
                        break;
                    case "AnyQueryCalendarColumn":
                        InfoDateTimePicker idtpValue = new InfoDateTimePicker();
                        idtpValue.BeginInit();
                        idtpValue.Name = ColumnsCount + "AnyQueryValue2InfoDateTimePicker2" + xn.Attributes["ValueEnabled"].Value;
                        idtpValue.Text = xn.Attributes["Value2"].Value;
                        idtpValue.Width = Convert.ToInt16(xn.Attributes["Width"].Value);
                        idtpValue.Location = new Point(standard, 15 + ColumnsCount * 30);
                        idtpValue.Enabled = Convert.ToBoolean(xn.Attributes["Enabled"].Value);
                        idtpValue.Format = DateTimePickerFormat.Long;
                        idtpValue.EndInit();
                        idtpValue.Enter += new EventHandler(tbValue_Enter);
                        this.panel2.Controls.Add(idtpValue);
                        break;
                    case "AnyQueryRefButtonColumn"://<->不会用到RefButton
                        //InfoDataSet idsRefButton2 = new InfoDataSet();
                        //if (xn.Attributes["RemoteName"].Value == "GLModule.cmdRefValUse")
                        //{
                        //    idsRefButton2.RemoteName = "GLModule.cmdRefValUse";
                        //    idsRefButton2.Execute(xn.Attributes["SelectCommand"].Value, xn.Attributes["SelectAlias"].Value, true);
                        //}
                        //else
                        //{
                        //    idsRefButton2.RemoteName = xn.Attributes["RemoteName"].Value;
                        //    idsRefButton2.Active = true;
                        //}

                        //InfoBindingSource ibsRefButton2 = new InfoBindingSource();
                        //ibsRefButton2.DataSource = idsRefButton2;
                        //if (xn.Attributes["RemoteName"].Value != null)
                        //    ibsRefButton2.DataMember = idsRefButton2.RealDataSet.Tables[0].TableName;
                        //else
                        //    ibsRefButton2.DataMember = "cmdRefValUse";

                        //InfoTranslate it2 = new InfoTranslate();
                        //it2.BindingSource = ibsRefButton2;
                        //TranslateRefReturnFields trrf2 = new TranslateRefReturnFields();
                        //trrf2.ColumnName = xn.Attributes["ValueMember"].Value;
                        //it2.RefReturnFields.Add(trrf2);

                        //TextBox tbRefButton2 = new TextBox();
                        //tbRefButton2.Name = ColumnsCount + "AnyQueryValue1RefTextBox" + xn.Attributes["ValueEnabled"].Value;
                        //tbRefButton2.Text = xn.Attributes["Value1"].Value;
                        //tbRefButton2.Width = Convert.ToInt16(xn.Attributes["Width"].Value);
                        //tbRefButton2.Location = new Point(standard, 15 + ColumnsCount * 30);
                        //tbRefButton2.Enabled = Convert.ToBoolean(xn.Attributes["Enabled"].Value);
                        //this.panel2.Controls.Add(tbRefButton2);

                        //InfoRefButton ifb2 = new InfoRefButton();
                        //ifb2.autoPanel = Convert.ToBoolean(xn.Attributes["AutoPanel"].Value);
                        //if (!ifb2.autoPanel)
                        //{
                        //    Control[] innerPanel = (this.innerAnyQuery.OwnerComp as InfoForm).Controls.Find(xn.Attributes["Panel"].Value, true);
                        //    if (innerPanel.Length > 0)
                        //    {
                        //        ifb2.panel = innerPanel[0] as Panel;
                        //    }

                        //    if (ifb2.panel == null)
                        //    {
                        //        for (int i = 0; i < panels.Count; i++)
                        //        {
                        //            if (panels[i] == null) continue;
                        //            ifb2.panel = panels[i];
                        //        }

                        //        if (ifb2.panel == null)
                        //        {
                        //            MessageBox.Show("Can not find the Panel '" + xn.Attributes["Panel"].Value + "' in your Form.");
                        //            return "AND";
                        //        }
                        //    }
                        //}
                        //ifb2.infoTranslate = it2;
                        //RefButtonMatch rbm2 = new RefButtonMatch();
                        //rbm2.matchColumnName = tbRefButton2.Name;
                        //ifb2.refButtonMatchs.Add(rbm2);
                        //ifb2.Name = ColumnsCount + "AnyQueryValue1InfoRefButton" + xn.Attributes["ValueEnabled"].Value;
                        //ifb2.Text = "...";
                        //ifb2.Width = 20;
                        //ifb2.Location = new Point(tbRefButton2.Location.X + tbRefButton2.Width + 2, 15 + ColumnsCount * 30);
                        ////ifb2.Enabled = Convert.ToBoolean(xn.Attributes["Enabled"].Value);
                        //this.panel2.Controls.Add(ifb2);
                        break;
                }
            }

            if (innerAnyQuery.AutoDisableColumns)
            {
                cbActive_CheckedChanged(cbActive, new EventArgs());
            }

            return xn.Attributes["Condition"].Value;
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if (innerAnyQuery.AllowAddQueryField)
            {
                InfoDataSet ids = null;
                if (innerAnyQuery.BindingSource != null)
                {
                    ids = innerAnyQuery.BindingSource.DataSource as InfoDataSet;
                    if (innerAnyQuery.MaxColumnCount == -1 || Count < innerAnyQuery.MaxColumnCount)
                    {
                        if (innerAnyQuery.QueryColumnMode == AnyQueryColumnMode.ByBindingSource)
                        {
                            if (Count >= ids.RealDataSet.Tables[0].Columns.Count)
                                return;
                        }
                        else if (innerAnyQuery.QueryColumnMode == AnyQueryColumnMode.ByColumns)
                        {
                            if (Count >= innerAnyQuery.Columns.Count)
                                return;
                        }

                        Count++;
                        AnyQueryColumns aqcTemp = new AnyQueryColumns();
                        foreach (AnyQueryColumns aqc in innerAnyQuery.Columns)
                        {
                            aqcTemp = aqc;
                            break;
                        }
                        CreateColumns(ids, aqcTemp, Count, false);
                    }
                }
            }
        }

        private void buttonSubtract_Click(object sender, EventArgs e)
        {
            if (Count > 5)
            {
                if (innerAnyQuery.AllowAddQueryField)
                {
                    ArrayList arr = new ArrayList();
                    foreach (Control c in this.panel2.Controls)
                    {
                        if (c.Name.StartsWith(Count + "AnyQuery"))
                        {
                            arr.Add(c);
                        }
                    }
                    for (int i = 0; i < arr.Count; i++)
                        this.panel2.Controls.Remove(arr[i] as Control);

                    if (Count > 0)
                        Count--;
                }
            }
            else
            {
                string message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "Srvtools", "AnyQuery", "SubtractWarning");
                MessageBox.Show(message);
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (innerAnyQuery.AnyQueryID == String.Empty)
                innerAnyQuery.AnyQueryID = CliUtils.GetMenuID(innerAnyQuery.PackageForm);

            frmAnyQuerySaveLoad faqsl = new frmAnyQuerySaveLoad(innerAnyQuery.BindingSource, "Save", innerAnyQuery.AnyQueryID);
            faqsl.ShowDialog();
            if (faqsl.isOK && faqsl.fileName != String.Empty)
            {
                String templateID = faqsl.fileName;
                String text = String.Empty;
                String xml = GetSaveXML();

                object[] param = new object[4];
                param[0] = innerAnyQuery.AnyQueryID;
                param[1] = templateID;
                param[2] = xml;
                param[3] = GetTableName();
                object[] myRet = CliUtils.CallMethod("GLModule", "AnyQuerySave", param);
                if (myRet != null && myRet[0].ToString() == "0")
                {
                    string message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "Srvtools", "AnyQuery", "SaveSuccess");
                    MessageBox.Show(message);
                }
            }
        }

        private String GetSaveXML()
        {
            XmlDocument DBXML = new XmlDocument();
            XmlElement xeMain = DBXML.CreateElement("AnyQuery");

            for (int i = 1; i <= Count; i++)
            {
                String isActive = "0";
                String caption = String.Empty;
                String operatorMask = String.Empty;
                String value1 = String.Empty;
                String value2 = String.Empty;
                String valueType = String.Empty;
                String Width = String.Empty;
                String selectAlias = String.Empty;
                String selectCommand = String.Empty;
                String displayMember = String.Empty;
                String valueMember = String.Empty;
                String condition = String.Empty;
                String formart = String.Empty;
                String items = String.Empty;
                String autoPanel = String.Empty;
                String panel = String.Empty;
                String remoteName = String.Empty;
                String enabled = String.Empty;
                String valueEnable = "True";
                String valueReal = String.Empty;
                String columnWidth = String.Empty;
                String displayMemberOnly = String.Empty;

                CheckBox cbActive = this.panel2.Controls[i + "AnyQueryActiveCheckBox"] as CheckBox;
                if (cbActive.Checked) isActive = "1";

                ComboBox cbCaption = this.panel2.Controls[i + "AnyQueryColumnComboBox"] as ComboBox;
                caption = cbCaption.Text;
                columnWidth = cbCaption.Width.ToString();

                //Label labelCondition = this.panel2.Controls[i + "AnyQueryConditionLabel"] as Label;
                Control[] tempCondition = this.Controls.Find(this.cbCondition.Name, true);
                condition = tempCondition[0].Text;

                ComboBox cbOperator = this.panel2.Controls[i + "AnyQueryOperatorComboBox"] as ComboBox;
                operatorMask = cbOperator.Text;

                foreach (Control c in this.panel2.Controls)
                {
                    if (c.Name.StartsWith(i + "AnyQueryValue1") && !c.Name.StartsWith(i + "AnyQueryValue1to"))
                    {
                        enabled = c.Enabled.ToString();
                        if (!(c is InfoRefButton))
                        {
                            value1 = c.Text;
                            if (c is InfoRefbuttonBox)
                                valueReal = (c as InfoRefbuttonBox).RealValue;
                            Width = c.Width.ToString();
                        }

                        if (c is TextBox)
                        {
                            valueType = "AnyQueryTextBoxColumn";
                        }
                        else if (c is InfoComboBox)
                        {
                            valueType = "AnyQueryComboBoxColumn";
                            if ((c as InfoComboBox).DataSource != null)
                            {
                                remoteName = (((c as InfoComboBox).DataSource as InfoBindingSource).DataSource as InfoDataSet).RemoteName;
                                displayMember = (c as InfoComboBox).DisplayMember;
                                valueMember = (c as InfoComboBox).ValueMember;
                            }

                            if ((c as InfoComboBox).SelectAlias != null && (c as InfoComboBox).SelectAlias != String.Empty && (c as InfoComboBox).SelectCommand != null && (c as InfoComboBox).SelectCommand != String.Empty)
                            {
                                selectAlias = (c as InfoComboBox).SelectAlias;
                                selectCommand = (c as InfoComboBox).SelectCommand;
                                displayMember = (c as InfoComboBox).DisplayMember;
                                valueMember = (c as InfoComboBox).ValueMember;
                            }
                            else
                            {
                                foreach (object obj in (c as InfoComboBox).Items)
                                {
                                    items += obj.ToString() + ";";
                                }
                            }
                            if ((c as InfoComboBox).SelectedValue != null)
                                value1 = (c as InfoComboBox).SelectedValue.ToString();
                            else
                                value1 = (c as InfoComboBox).Text;
                            valueReal = (c as InfoComboBox).Text;
                            displayMemberOnly = (c as InfoComboBox).DisplayMemberOnly.ToString();
                        }
                        else if (c is CheckBox)
                        {
                            valueType = "AnyQueryCheckBoxColumn";
                            if ((c as CheckBox).Checked)
                                value1 = "1";
                        }
                        else if (c is InfoRefvalBox)
                        {
                            valueType = "AnyQueryRefValColumn";
                            selectAlias = (c as InfoRefvalBox).RefVal.SelectAlias;
                            selectCommand = (c as InfoRefvalBox).RefVal.SelectCommand;
                            displayMember = (c as InfoRefvalBox).RefVal.DisplayMember;
                            valueMember = (c as InfoRefvalBox).RefVal.ValueMember;
                            value1 = (c as InfoRefvalBox).TextBoxText;
                            remoteName = (((c as InfoRefvalBox).RefVal.DataSource as InfoBindingSource).DataSource as InfoDataSet).RemoteName;
                            valueReal = (c as InfoRefvalBox).TextBoxSelectedValue;
                        }
                        else if (c is InfoDateTimePicker)
                        {
                            valueType = "AnyQueryCalendarColumn";
                        }
                        else if (c is InfoRefButton)
                        {
                            valueType = "AnyQueryRefButtonColumn";
                            selectAlias = ((c as InfoRefButton).infoTranslate.BindingSource.DataSource as InfoDataSet).RefDBAlias;
                            selectCommand = ((c as InfoRefButton).infoTranslate.BindingSource.DataSource as InfoDataSet).RefCommandText;
                            remoteName = ((c as InfoRefButton).infoTranslate.BindingSource.DataSource as InfoDataSet).RemoteName;
                            valueMember = ((c as InfoRefButton).infoTranslate.RefReturnFields[0] as TranslateRefReturnFields).ColumnName;
                            displayMember = ((c as InfoRefButton).infoTranslate.RefReturnFields[0] as TranslateRefReturnFields).DisplayColumnName;
                            autoPanel = (c as InfoRefButton).autoPanel.ToString();
                            if ((c as InfoRefButton).panel != null)
                                panel = (c as InfoRefButton).panel.Name;
                        }
                    }
                }

                foreach (Control c in this.panel2.Controls)
                {
                    if (c.Name.StartsWith(i + "AnyQueryValue2"))
                    {
                        value2 = c.Text;

                        break;
                    }
                }

                foreach (AnyQueryColumns aqc in innerAnyQuery.Columns)
                {
                    if (caption == aqc.Caption)
                        valueEnable = aqc.Enabled.ToString();
                }

                XmlElement elem = DBXML.CreateElement("String");
                XmlAttribute attr = DBXML.CreateAttribute("IsActive");
                attr.Value = isActive;
                elem.Attributes.Append(attr);

                attr = DBXML.CreateAttribute("Caption");
                attr.Value = caption;
                elem.Attributes.Append(attr);

                attr = DBXML.CreateAttribute("Operator");
                attr.Value = operatorMask;
                elem.Attributes.Append(attr);

                attr = DBXML.CreateAttribute("ValueType");
                attr.Value = valueType;
                elem.Attributes.Append(attr);

                attr = DBXML.CreateAttribute("Width");
                attr.Value = Width;
                elem.Attributes.Append(attr);

                attr = DBXML.CreateAttribute("Value1");
                attr.Value = value1;
                elem.Attributes.Append(attr);

                attr = DBXML.CreateAttribute("Value2");
                attr.Value = value2;
                elem.Attributes.Append(attr);

                attr = DBXML.CreateAttribute("SelectAlias");
                attr.Value = selectAlias;
                elem.Attributes.Append(attr);

                attr = DBXML.CreateAttribute("SelectCommand");
                attr.Value = selectCommand;
                elem.Attributes.Append(attr);

                attr = DBXML.CreateAttribute("DisplayMember");
                attr.Value = displayMember;
                elem.Attributes.Append(attr);

                attr = DBXML.CreateAttribute("ValueMember");
                attr.Value = valueMember;
                elem.Attributes.Append(attr);

                attr = DBXML.CreateAttribute("Condition");
                attr.Value = condition;
                elem.Attributes.Append(attr);

                attr = DBXML.CreateAttribute("Items");
                attr.Value = items;
                elem.Attributes.Append(attr);

                attr = DBXML.CreateAttribute("AutoPanel");
                attr.Value = autoPanel;
                elem.Attributes.Append(attr);

                attr = DBXML.CreateAttribute("Panel");
                attr.Value = panel;
                elem.Attributes.Append(attr);

                attr = DBXML.CreateAttribute("RemoteName");
                attr.Value = remoteName;
                elem.Attributes.Append(attr);

                attr = DBXML.CreateAttribute("Enabled");
                attr.Value = enabled;
                elem.Attributes.Append(attr);

                attr = DBXML.CreateAttribute("ValueEnabled");
                attr.Value = valueEnable;
                elem.Attributes.Append(attr);

                attr = DBXML.CreateAttribute("ValueReal");
                attr.Value = valueReal;
                elem.Attributes.Append(attr);

                attr = DBXML.CreateAttribute("ColumnWidth");
                attr.Value = columnWidth;
                elem.Attributes.Append(attr);

                attr = DBXML.CreateAttribute("DisplayMemberOnly");
                attr.Value = displayMemberOnly;
                elem.Attributes.Append(attr);
                xeMain.AppendChild(elem);
            }

            return xeMain.OuterXml;
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            if (innerAnyQuery.AnyQueryID == String.Empty)
                innerAnyQuery.AnyQueryID = CliUtils.GetMenuID(innerAnyQuery.PackageForm);

            frmAnyQuerySaveLoad faqsl = new frmAnyQuerySaveLoad(innerAnyQuery.BindingSource, "Load", innerAnyQuery.AnyQueryID);
            faqsl.ShowDialog();
            if (faqsl.isOK && faqsl.fileName != String.Empty)
            {
                String templateID = faqsl.fileName;
                String text = String.Empty;
                String tableName = String.Empty;
                object[] param = new object[2];
                param[0] = innerAnyQuery.AnyQueryID;
                param[1] = templateID;
                object[] myRet = CliUtils.CallMethod("GLModule", "AnyQueryLoad", param);
                if (myRet != null && myRet[0].ToString() == "0")
                {
                    text = myRet[1].ToString();
                    tableName = myRet[2].ToString();
                }

                String message = String.Empty;
                if (tableName == "" || tableName != GetTableName())
                {
                    message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "Srvtools", "AnyQuery", "CheckTable");
                    MessageBox.Show(message);
                    this.DialogResult = System.Windows.Forms.DialogResult.None;
                    return;
                }

                List<Panel> tempPanel = new List<Panel>();
                for (int i = 0; i < this.panel2.Controls.Count; i++)
                {
                    if (this.panel2.Controls[i] is InfoRefButton && !(this.panel2.Controls[i] as InfoRefButton).autoPanel)
                    {
                        tempPanel.Add((this.panel2.Controls[i] as InfoRefButton).panel);
                    }
                }
                this.panel2.Controls.Clear();
                Count = 0;

                message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "Srvtools", "AnyQuery", "ColumnValue");
                String[] ColumnValue = message.Split(';');
                Label labelColumn = new Label();
                labelColumn.Location = new Point(124, 16);
                labelColumn.Text = ColumnValue[0];
                this.panel2.Controls.Add(labelColumn);
                Label labelValue = new Label();
                labelValue.Location = new Point(335, 16);
                labelValue.Text = ColumnValue[1];
                this.panel2.Controls.Add(labelValue);
                Label conditionLabel = new Label();
                conditionLabel.Text = ColumnValue[2];
                conditionLabel.Location = new Point(477, 16);
                ComboBox cbCondition = new ComboBox();
                cbCondition.Name = "cbCondition";
                cbCondition.Location = new Point(557, 13);
                cbCondition.Size = new Size(54, 20);
                cbCondition.Items.AddRange(new String[] { "AND", "OR" });
                cbCondition.DropDownStyle = ComboBoxStyle.DropDownList;
                cbCondition.SelectedIndex = 0;

                InfoDataSet ids = null;
                if (innerAnyQuery.BindingSource != null)
                {
                    ids = innerAnyQuery.BindingSource.DataSource as InfoDataSet;

                    XmlDocument DBXML = new XmlDocument();
                    DBXML.LoadXml(text);
                    XmlNode aNode = DBXML.DocumentElement.FirstChild;

                    while (aNode != null)
                    {
                        Count++;
                        cbCondition.Text = CreateColumns(ids, aNode, tempPanel);
                        aNode = aNode.NextSibling;
                    }
                }

                this.panel2.Controls.Add(cbCondition);
                this.panel2.Controls.Add(conditionLabel);
            }
        }

        private void buttonQuery_Click(object sender, EventArgs e)
        {
            InfoDataSet ids = innerAnyQuery.BindingSource.DataSource as InfoDataSet;
            if (ids != null)
            {
                List<String> detailWhere = new List<string>();

                String whereString = String.Empty;
                String[] remotename = (innerAnyQuery.BindingSource.DataSource as InfoDataSet).RemoteName.Split('.');
                String strModuleName = remotename[0];
                String strTableName = remotename[1];
                String tablename = CliUtils.GetTableName(strModuleName, strTableName, CliUtils.fCurrentProject);
                String detailtablename = String.Empty;
                String strDetailTableName = String.Empty;
                if (innerAnyQuery.DetailBindingSource != null)
                {
                    DataView dataView = innerAnyQuery.DetailBindingSource.List as DataView;
                    if (dataView != null)
                    {
                        strDetailTableName = dataView.Table.TableName;
                    }
                    else
                    {
                        int iRelationPos = -1;
                        DataSet dSet = ((InfoDataSet)innerAnyQuery.DetailBindingSource.GetDataSource()).RealDataSet;
                        for (int i = 0; i < dSet.Relations.Count; i++)
                        {
                            if (innerAnyQuery.DetailBindingSource.DataMember == dSet.Relations[i].RelationName)
                            {
                                iRelationPos = i;
                                break;
                            }
                        }
                        if (iRelationPos != -1)
                        {
                            strDetailTableName = dSet.Relations[iRelationPos].ChildTable.TableName;
                        }
                    }
                    detailtablename = CliUtils.GetTableName(strModuleName, strDetailTableName, CliUtils.fCurrentProject);
                    //if (!detailtablename.Contains("[") && !detailtablename.Contains("}"))
                    //{
                    //    detailtablename = String.Format("[{0}]", detailtablename);
                    //}
                }

                String[] quote = CliUtils.GetDataBaseQuote();
                for (int i = 1; i <= Count; i++)
                {
                    String strWhere = String.Empty;
                    foreach (DataColumn dc in ids.RealDataSet.Tables[0].Columns)
                    {
                        String caption = String.Empty;
                        String columnName = String.Empty;
                        String operatorMask = String.Empty;
                        String value1 = String.Empty;
                        String value2 = String.Empty;
                        Type valueType = null;
                        String condition = String.Empty;
                        object realValue1 = null;
                        object realValue2 = null;
                        bool DateConver = false;

                        CheckBox cbActive = this.panel2.Controls[i + "AnyQueryActiveCheckBox"] as CheckBox;
                        if (!cbActive.Checked) break;

                        ComboBox cbColumn = this.panel2.Controls[i + "AnyQueryColumnComboBox"] as ComboBox;
                        caption = cbColumn.Text;
                        columnName = cbColumn.SelectedValue.ToString();
                        if (columnName != CliUtils.GetTableNameForColumn(masterSql, dc.ColumnName)) continue;

                        valueType = dc.DataType;
                        bool isNvarChar = false;
                        foreach (AnyQueryColumns aqc in innerAnyQuery.Columns)
                        {
                            if (aqc.Column == dc.ColumnName)
                            {
                                isNvarChar = aqc.IsNvarChar;
                                break;
                            }
                        }
                        String nvarCharMark = valueType == typeof(string) && isNvarChar ? "N" : string.Empty;

                        Control[] tempCondition = this.Controls.Find(this.cbCondition.Name, true);
                        condition = tempCondition[0].Text;

                        ComboBox cbOperator = this.panel2.Controls[i + "AnyQueryOperatorComboBox"] as ComboBox;
                        operatorMask = cbOperator.Text;

                        DateConver = GetDataConver(dc.ColumnName);
                        foreach (Control c in this.panel2.Controls)
                        {
                            if (c.Name.StartsWith(i + "AnyQueryValue1"))
                            {
                                if (c.Name.StartsWith(i + "AnyQueryValue1CheckBox"))
                                {
                                    if (valueType == typeof(bool))
                                    {
                                        if ((c as CheckBox).Checked)
                                            value1 = "1";
                                        else
                                            value1 = "0";
                                    }
                                    else
                                    {
                                        if ((c as CheckBox).Checked)
                                            value1 = "'Y'";
                                        else
                                            value1 = "'N'";
                                    }
                                }
                                else if (c.Name.StartsWith(i + "AnyQueryValue1RefTextBox"))
                                {
                                    if (operatorMask == "IN" || operatorMask == "NOT IN")
                                    {
                                        String[] temp = (c as InfoRefbuttonBox).RealValue.Split(',');
                                        for (int j = 0; j < temp.Length; j++)
                                        {
                                            if (temp[j] != String.Empty)
                                            {
                                                temp[j] = IsLike(operatorMask, temp[j]);
                                                value1 += Mark(valueType, dc.ColumnName, operatorMask, temp[j], DateConver, nvarCharMark) + ",";
                                            }
                                        }
                                        if (value1.EndsWith(","))
                                        {
                                            value1 = value1.Remove(value1.LastIndexOf(","));
                                        }
                                    }
                                    else
                                    {
                                        value1 = (c as InfoRefbuttonBox).RealValue;
                                        value1 = Mark(valueType, dc.ColumnName, operatorMask, value1, DateConver, nvarCharMark);
                                        realValue1 = (c as InfoRefbuttonBox).RealValue;
                                    }
                                }
                                else if (c.Name.StartsWith(i + "AnyQueryValue1InfoRefvalBox"))
                                {
                                    if (operatorMask == "IN" || operatorMask == "NOT IN")
                                    {
                                        String[] temp = (c as InfoRefvalBox).TextBoxSelectedValue.Split(',');
                                        for (int j = 0; j < temp.Length; j++)
                                        {
                                            if (temp[j] != String.Empty)
                                            {
                                                temp[j] = IsLike(operatorMask, temp[j]);
                                                value1 += Mark(valueType, dc.ColumnName, operatorMask, temp[j], DateConver, nvarCharMark) + ",";
                                            }
                                        }
                                        if (value1.EndsWith(","))
                                        {
                                            value1 = value1.Remove(value1.LastIndexOf(","));
                                        }

                                        realValue1 = (c as InfoRefvalBox).TextBoxSelectedValue;
                                    }
                                    else
                                    {
                                        value1 = (c as InfoRefvalBox).TextBoxSelectedValue;
                                        value1 = Mark(valueType, dc.ColumnName, operatorMask, value1, DateConver, nvarCharMark);
                                        realValue1 = (c as InfoRefvalBox).TextBoxSelectedValue;
                                    }
                                }
                                else if (c.Name.StartsWith(i + "AnyQueryValue1InfoDateTimePicker"))
                                {
                                    if ((c as InfoDateTimePicker).Checked)
                                    {
                                        value1 = (c as InfoDateTimePicker).Value.ToString("yyyy/MM/dd");
                                        value1 = Mark(valueType, columnName, operatorMask, value1, DateConver, nvarCharMark);
                                        realValue1 = Convert.ToDateTime((c as InfoDateTimePicker).Value);
                                    }
                                }
                                else if (c.Name.StartsWith(i + "AnyQueryValue1InfoComboBox"))
                                {
                                    if ((c as InfoComboBox).SelectedValue != null)
                                    {
                                        value1 = IsLike(operatorMask, (c as InfoComboBox).SelectedValue.ToString());
                                        realValue1 = (c as InfoComboBox).SelectedValue.ToString();
                                    }
                                    else
                                    {
                                        value1 = IsLike(operatorMask, (c as InfoComboBox).Text);
                                        realValue1 = (c as InfoComboBox).Text;
                                    }
                                    value1 = Mark(valueType, dc.ColumnName, operatorMask, value1, DateConver, nvarCharMark);
                                }
                                else
                                {
                                    if (operatorMask == "IN" || operatorMask == "NOT IN")
                                    {
                                        String[] temp = c.Text.Split(',');
                                        for (int j = 0; j < temp.Length; j++)
                                        {
                                            if (temp[j] != String.Empty)
                                            {
                                                temp[j] = IsLike(operatorMask, temp[j]);
                                                value1 += Mark(valueType, dc.ColumnName, operatorMask, temp[j], DateConver, nvarCharMark) + ",";
                                            }
                                        }
                                        if (value1.EndsWith(","))
                                        {
                                            value1 = value1.Remove(value1.LastIndexOf(","));
                                        }
                                        realValue1 = c.Text;
                                    }
                                    else
                                    {
                                        value1 = IsLike(operatorMask, c.Text);
                                        value1 = Mark(valueType, dc.ColumnName, operatorMask, value1, DateConver, nvarCharMark);
                                        realValue1 = c.Text;
                                    }
                                }
                                break;
                            }
                        }

                        if (value1 == String.Empty)
                        {
                            string message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "Srvtools", "AnyQuery", "ValueNull");
                            MessageBox.Show(String.Format(message, caption));
                            this.DialogResult = System.Windows.Forms.DialogResult.None;
                            return;
                        }

                        if (operatorMask == "IN" || operatorMask == "NOT IN")
                            value1 = String.Format("({0})", value1);

                        if (operatorMask == "<->" || operatorMask == "!<->")
                        {
                            foreach (Control c in this.panel2.Controls)
                            {
                                if (c.Name.StartsWith(i + "AnyQueryValue2InfoRefvalBox"))
                                {
                                    value2 = (c as InfoRefvalBox).TextBoxSelectedValue;
                                    value2 = Mark(valueType, dc.ColumnName, operatorMask, value2, DateConver, nvarCharMark);
                                    realValue2 = (c as InfoRefvalBox).TextBoxSelectedValue;
                                    break;
                                }
                                else if (c.Name.StartsWith(i + "AnyQueryValue2InfoDateTimePicker"))
                                {
                                    if ((c as InfoDateTimePicker).Checked)
                                    {
                                        value2 = (c as InfoDateTimePicker).Value.ToString("yyyy/MM/dd");
                                        value2 = Mark(valueType, columnName, operatorMask, value2, DateConver, nvarCharMark);
                                        realValue2 = Convert.ToDateTime((c as InfoDateTimePicker).Value);

                                        //value2 = (c as InfoDateTimePicker).Value.AddDays(1).AddSeconds(-1).ToString();
                                        //realValue2 = Convert.ToDateTime((c as InfoDateTimePicker).Value.AddDays(1).AddSeconds(-1));
                                        break;
                                    }
                                }
                                else if (c.Name.StartsWith(i + "AnyQueryValue2"))
                                {
                                    value2 = c.Text;
                                    value2 = Mark(valueType, dc.ColumnName, operatorMask, value2, DateConver, nvarCharMark);
                                    realValue2 = c.Text;
                                    break;
                                }
                            }
                        }

                        if (String.IsNullOrEmpty(value2))
                        {
                            value2 = value1;
                            realValue2 = realValue1;
                        }

                        if (caption != String.Empty)
                        {
                            if (dc.DataType != typeof(DateTime) || !DateConver)
                            {
                                if (operatorMask == "<->")
                                {
                                    if (realValue2 != null)
                                    {
                                        bool flag = false;
                                        if (realValue2 is DateTime && ((DateTime)realValue2).Date < ((DateTime)realValue1).Date)
                                            flag = true;
                                        else if (realValue2 is Int32 && Convert.ToInt32(realValue2) < Convert.ToInt32(realValue1))
                                            flag = true;

                                        if (flag)
                                        {
                                            string message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "Srvtools", "AnyQuery", "ValueOverFlow");
                                            MessageBox.Show(message);
                                            this.DialogResult = System.Windows.Forms.DialogResult.None;
                                            return;
                                        }
                                    }
                                    strWhere = columnName + ">=" + value1 + " AND " + columnName + "<=" + value2;
                                }
                                else if (operatorMask == "!<->")
                                {
                                    if (realValue2 != null)
                                    {
                                        bool flag = false;
                                        if (realValue2 is DateTime && ((DateTime)realValue2).Date < ((DateTime)realValue1).Date)
                                            flag = true;
                                        else if (realValue2 is Int32 && Convert.ToInt32(realValue2) < Convert.ToInt32(realValue1))
                                            flag = true;

                                        if (flag)
                                        {
                                            string message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "Srvtools", "AnyQuery", "ValueOverFlow");
                                            MessageBox.Show(message);
                                            this.DialogResult = System.Windows.Forms.DialogResult.None;
                                            return;
                                        }
                                    }
                                    strWhere = columnName + "<" + value1 + " OR " + columnName + ">" + value2;
                                }
                                else if (operatorMask == "%**" || operatorMask == "**%" || operatorMask == "%%")
                                    strWhere = columnName + " like " + value1;
                                else if (operatorMask == "!%%")
                                    strWhere = columnName + " not like " + value1;
                                else
                                    strWhere = columnName + " " + operatorMask + " " + value1;
                            }
                            else
                            {
                                if (operatorMask == "<->")
                                {
                                    if (realValue2 != null)
                                    {
                                        bool flag = false;
                                        if (realValue2 is DateTime)
                                        {
                                            if (((DateTime)realValue2).Date < ((DateTime)realValue1).Date)
                                                flag = true;
                                        }
                                        else if (Convert.ToInt32(realValue2) < Convert.ToInt32(realValue1))
                                            flag = true;

                                        if (flag)
                                        {
                                            string message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "Srvtools", "AnyQuery", "ValueOverFlow");
                                            MessageBox.Show(message);
                                            this.DialogResult = System.Windows.Forms.DialogResult.None;
                                            return;
                                        }
                                    }
                                    strWhere = value1.Replace("<->", ">=") + " AND " + value2.Replace("<->", "<=");
                                }
                                else if (operatorMask == "!<->")
                                {
                                    if (realValue2 != null)
                                    {
                                        bool flag = false;
                                        if (realValue2 is DateTime)
                                        {
                                            if (((DateTime)realValue2).Date < ((DateTime)realValue1).Date)
                                                flag = true;
                                        }
                                        else if (Convert.ToInt32(realValue2) < Convert.ToInt32(realValue1))
                                            flag = true;

                                        if (flag)
                                        {
                                            string message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "Srvtools", "AnyQuery", "ValueOverFlow");
                                            MessageBox.Show(message);
                                            this.DialogResult = System.Windows.Forms.DialogResult.None;
                                            return;
                                        }
                                    }
                                    strWhere = value1.Replace("<->", "<") + " OR " + value2.Replace("<->", ">");
                                }
                                else
                                    strWhere = value1;
                            }

                            strWhere = String.Format("({0})", strWhere);
                            whereString += strWhere + " " + condition + " ";
                            break;
                        }
                    }

                    if (strDetailTableName != String.Empty && ids.RealDataSet.Tables[strDetailTableName] != null)
                    {
                        foreach (DataColumn dc in ids.RealDataSet.Tables[strDetailTableName].Columns)
                        {
                            String caption = String.Empty;
                            String columnName = String.Empty;
                            String operatorMask = String.Empty;
                            String value1 = String.Empty;
                            String value2 = String.Empty;
                            Type valueType = null;
                            String condition = String.Empty;
                            object realValue1 = null;
                            object realValue2 = null;
                            bool DateConver = false;

                            CheckBox cbActive = this.panel2.Controls[i + "AnyQueryActiveCheckBox"] as CheckBox;
                            if (!cbActive.Checked) break;

                            ComboBox cbColumn = this.panel2.Controls[i + "AnyQueryColumnComboBox"] as ComboBox;
                            caption = cbColumn.Text;
                            columnName = cbColumn.SelectedValue.ToString();
                            if (detailSql == String.Empty) continue;
                            if (columnName != CliUtils.GetTableNameForColumn(detailSql, dc.ColumnName)) continue;

                            valueType = dc.DataType;
                            bool isNvarChar = false;
                            foreach (AnyQueryColumns aqc in innerAnyQuery.Columns)
                            {
                                if (aqc.Column == dc.ColumnName)
                                {
                                    isNvarChar = aqc.IsNvarChar;
                                    break;
                                }
                            }
                            String nvarCharMark = valueType == typeof(string) && isNvarChar ? "N" : string.Empty;

                            Control[] tempCondition = this.Controls.Find(this.cbCondition.Name, true);
                            condition = tempCondition[0].Text;

                            ComboBox cbOperator = this.panel2.Controls[i + "AnyQueryOperatorComboBox"] as ComboBox;
                            operatorMask = cbOperator.Text;
                            DateConver = GetDataConver(dc.ColumnName);
                            foreach (Control c in this.panel2.Controls)
                            {
                                if (c.Name.StartsWith(i + "AnyQueryValue1"))
                                {
                                    if (c.Name.StartsWith(i + "AnyQueryValue1CheckBox"))
                                    {
                                        if (valueType == typeof(bool))
                                        {
                                            if ((c as CheckBox).Checked)
                                                value1 = "1";
                                            else
                                                value1 = "0";
                                        }
                                        else
                                        {
                                            if ((c as CheckBox).Checked)
                                                value1 = "'Y'";
                                            else
                                                value1 = "'N'";
                                        }
                                    }
                                    else if (c.Name.StartsWith(i + "AnyQueryValue1RefTextBox"))
                                    {
                                        String[] temp = (c as InfoRefbuttonBox).RealValue.Split(',');
                                        for (int j = 0; j < temp.Length; j++)
                                        {
                                            if (temp[j] != String.Empty)
                                            {
                                                temp[j] = IsLike(operatorMask, temp[j]);
                                                value1 += Mark(valueType, dc.ColumnName, operatorMask, temp[j], DateConver, nvarCharMark) + ",";
                                            }
                                        }
                                        if (value1.EndsWith(","))
                                        {
                                            value1 = value1.Remove(value1.LastIndexOf(","));
                                        }
                                    }
                                    else if (c.Name.StartsWith(i + "AnyQueryValue1InfoRefvalBox"))
                                    {
                                        String[] temp = (c as InfoRefvalBox).TextBoxSelectedValue.Split(',');
                                        for (int j = 0; j < temp.Length; j++)
                                        {
                                            if (temp[j] != String.Empty)
                                            {
                                                temp[j] = IsLike(operatorMask, temp[j]);
                                                value1 += Mark(valueType, dc.ColumnName, operatorMask, temp[j], DateConver, nvarCharMark) + ",";
                                            }
                                        }
                                        if (value1.EndsWith(","))
                                        {
                                            value1 = value1.Remove(value1.LastIndexOf(","));
                                        }

                                        realValue1 = (c as InfoRefvalBox).TextBoxSelectedValue;
                                    }
                                    else if (c.Name.StartsWith(i + "AnyQueryValue1InfoDateTimePicker"))
                                    {
                                        if ((c as InfoDateTimePicker).Checked)
                                        {
                                            value1 = (c as InfoDateTimePicker).Value.ToString("yyyy/MM/dd");
                                            value1 = Mark(valueType, dc.ColumnName, operatorMask, value1, DateConver, nvarCharMark);
                                            realValue1 = Convert.ToDateTime((c as InfoDateTimePicker).Value);
                                        }
                                    }
                                    else if (c.Name.StartsWith(i + "AnyQueryValue1InfoComboBox"))
                                    {
                                        if ((c as InfoComboBox).SelectedValue != null)
                                        {
                                            value1 = IsLike(operatorMask, (c as InfoComboBox).SelectedValue.ToString());
                                            realValue1 = (c as InfoComboBox).SelectedValue.ToString();
                                        }
                                        else
                                        {
                                            value1 = IsLike(operatorMask, (c as InfoComboBox).Text);
                                            realValue1 = (c as InfoComboBox).Text;
                                        }
                                        value1 = Mark(valueType, dc.ColumnName, operatorMask, value1, DateConver, nvarCharMark);
                                    }
                                    else
                                    {
                                        String[] temp = c.Text.Split(',');
                                        for (int j = 0; j < temp.Length; j++)
                                        {
                                            if (temp[j] != String.Empty)
                                            {
                                                temp[j] = IsLike(operatorMask, temp[j]);
                                                value1 += Mark(valueType, dc.ColumnName, operatorMask, temp[j], DateConver, nvarCharMark) + ",";
                                            }
                                        }
                                        if (value1.EndsWith(","))
                                        {
                                            value1 = value1.Remove(value1.LastIndexOf(","));
                                        }
                                        realValue1 = c.Text;
                                    }

                                    break;
                                }
                            }

                            if (value1 == String.Empty)
                            {
                                string message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "Srvtools", "AnyQuery", "ValueNull");
                                MessageBox.Show(String.Format(message, caption));
                                this.DialogResult = System.Windows.Forms.DialogResult.None;
                                return;
                            }

                            if (operatorMask == "IN" || operatorMask == "NOT IN")
                                value1 = String.Format("({0})", value1);

                            if (operatorMask == "<->" || operatorMask == "!<->")
                            {
                                foreach (Control c in this.panel2.Controls)
                                {
                                    if (c.Name.StartsWith(i + "AnyQueryValue2InfoRefvalBox"))
                                    {
                                        value2 = (c as InfoRefvalBox).TextBoxSelectedValue;
                                        realValue2 = (c as InfoRefvalBox).TextBoxSelectedValue;
                                        break;
                                    }
                                    else if (c.Name.StartsWith(i + "AnyQueryValue2InfoDateTimePicker"))
                                    {
                                        if ((c as InfoDateTimePicker).Checked)
                                        {
                                            value2 = (c as InfoDateTimePicker).Value.ToString("yyyy/MM/dd");
                                            realValue2 = Convert.ToDateTime((c as InfoDateTimePicker).Value);
                                            break;
                                        }
                                    }
                                    else if (c.Name.StartsWith(i + "AnyQueryValue2"))
                                    {
                                        value2 = c.Text;
                                        realValue2 = c.Text;
                                        break;
                                    }
                                }
                                value2 = Mark(valueType, dc.ColumnName, operatorMask, value2, DateConver, nvarCharMark);
                            }

                            if (caption != String.Empty)
                            {
                                if (dc.DataType != typeof(DateTime) || !DateConver)
                                {
                                    if (operatorMask == "<->")
                                    {
                                        if (realValue2 != null)
                                        {
                                            bool flag = false;
                                            int iRealValue1 = 0;
                                            int iRealValue2 = 0;
                                            if (realValue2 is DateTime)
                                            {
                                                if (((DateTime)realValue2).Date < ((DateTime)realValue1).Date)
                                                    flag = true;
                                            }
                                            else if (int.TryParse(realValue1.ToString(), out iRealValue1) && int.TryParse(realValue2.ToString(), out iRealValue2))
                                            {
                                                if (iRealValue2 < iRealValue1)
                                                    flag = true;
                                            }

                                            if (flag)
                                            {
                                                string message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "Srvtools", "AnyQuery", "ValueOverFlow");
                                                MessageBox.Show(message);
                                                this.DialogResult = System.Windows.Forms.DialogResult.None;
                                                return;
                                            }
                                        }
                                        strWhere = columnName + ">=" + value1 + " AND " + columnName + "<=" + value2;
                                    }
                                    else if (operatorMask == "!<->")
                                    {
                                        if (realValue2 != null)
                                        {
                                            bool flag = false;
                                            int iRealValue1 = 0;
                                            int iRealValue2 = 0;
                                            if (realValue2 is DateTime)
                                            {
                                                if (((DateTime)realValue2).Date < ((DateTime)realValue1).Date)
                                                    flag = true;
                                            }
                                            else if (int.TryParse(realValue1.ToString(), out iRealValue1) && int.TryParse(realValue2.ToString(), out iRealValue2))
                                            {
                                                if (iRealValue2 < iRealValue1)
                                                    flag = true;
                                            }
                                            if (flag)
                                            {
                                                string message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "Srvtools", "AnyQuery", "ValueOverFlow");
                                                MessageBox.Show(message);
                                                this.DialogResult = System.Windows.Forms.DialogResult.None;
                                                return;
                                            }
                                        }
                                        strWhere = columnName + "<" + value1 + " OR " + columnName + ">" + value2;
                                    }
                                    else if (operatorMask == "%**" || operatorMask == "**%" || operatorMask == "%%")
                                        strWhere = columnName + " like " + value1;
                                    else if (operatorMask == "!%%")
                                        strWhere = columnName + " not like " + value1;
                                    else
                                        strWhere = columnName + " " + operatorMask + " " + value1;
                                }

                                strWhere = String.Format("({0})", strWhere);
                                detailWhere.Add(strWhere);
                                break;
                            }
                        }
                    }
                }

                if (detailWhere.Count > 0)
                {
                    String strDetail = String.Empty;
                    foreach (String str in detailWhere)
                    {
                        Control[] tempCondition = this.Controls.Find(this.cbCondition.Name, true);
                        strDetail += str + tempCondition[0].Text;
                    }
                    if (strDetail.EndsWith("OR"))
                        strDetail = strDetail.Remove(strDetail.LastIndexOf("OR"));
                    else if (strDetail.EndsWith("AND"))
                        strDetail = strDetail.Remove(strDetail.LastIndexOf("AND"));

                    //if (detailtablename.Contains(" ") && !detailtablename.Contains("[")) detailtablename = "[" + detailtablename + "]";

                    if (String.IsNullOrEmpty(innerAnyQuery.MasterDetailField))
                        innerAnyQuery.MasterDetailField = innerAnyQuery.DetailKeyField;
                    whereString += String.Format("{3} in (SELECT {1} FROM {0} WHERE {2})", detailtablename, innerAnyQuery.DetailKeyField, strDetail, innerAnyQuery.MasterDetailField);

                    //String[] detailTemp = detailSql.Split(new string[] { "SELECT", "FROM", "WHERE" }, StringSplitOptions.RemoveEmptyEntries);
                    //if (String.IsNullOrEmpty(innerAnyQuery.MasterDetailField))
                    //    innerAnyQuery.MasterDetailField = innerAnyQuery.DetailKeyField;
                    //whereString += String.Format("{3} in (SELECT {1} FROM {0} WHERE {2})", detailTemp[1], innerAnyQuery.DetailKeyField, strDetail, innerAnyQuery.MasterDetailField);
                    //if (detailTemp.Length > 2)
                    //    whereString += " and " + detailTemp[2];
                }

                if (whereString.EndsWith("OR "))
                    whereString = whereString.Remove(whereString.LastIndexOf("OR "));
                else if (whereString.EndsWith("AND "))
                    whereString = whereString.Remove(whereString.LastIndexOf("AND "));
                Where = whereString;

                if (aInfoNavigator != null && !aInfoNavigator.QuerySQLSend)
                {
                    aInfoNavigator.OnQueryConfirm(new QueryConfirmEventArgs(Where));
                }
                else if (isExecute)
                {
                    AfterQueryEventArgs aqe = new AfterQueryEventArgs();
                    aqe.whereString = whereString;
                    OnAfterQuery(aqe);
                    if (!aqe.Cancel)
                        ids.SetWhere(aqe.whereString);
                }
            }
            if (Where == String.Empty)
                Where = "1=1";
            this.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.Close();
        }

        private Type GetDataType(String columnName)
        {
            if (columnName.Contains("."))
                columnName = columnName.Split('.')[1];
            InfoDataSet ids = innerAnyQuery.BindingSource.DataSource as InfoDataSet;
            if (ids != null)
            {
                for (int i = 0; i < ids.RealDataSet.Tables.Count; i++)
                {
                    foreach (DataColumn dc in ids.RealDataSet.Tables[i].Columns)
                    {
                        if (dc.ColumnName == columnName)
                        {
                            return dc.DataType;
                        }
                    }
                }
            }
            return null;
        }

        private bool GetDataConver(String columnName)
        {
            foreach (AnyQueryColumns aqc in innerAnyQuery.Columns)
            {
                if (aqc.Column == columnName)
                {
                    return aqc.DateConver;
                }
            }
            return false;
        }

        private String IsLike(String oprator, String value)
        {
            if (oprator == "%**")
                value = "%" + value;
            else if (oprator == "**%")
                value = value + "%";
            else if (oprator == "%%" || oprator == "!%%")
                value = "%" + value + "%";

            return value;
        }

        private String Mark(Type valueType, String column, String operatorMask, String value, bool dateConvert, String nvarCharMark)
        {
            String strwhere = String.Empty;
            String[] DBType = GetDBType();
            try
            {
                if (value.StartsWith("_"))
                {
                    object[] realValue = CliUtils.GetValue(value);
                    if (realValue[0].ToString() == "0")
                        value = realValue[1].ToString();
                }

                if (value.ToString().Length == 0 || string.Compare(value.ToString(), "null", true) == 0)
                {
                    strwhere = value.ToString();
                    if (valueType == typeof(String))
                    {
                        strwhere = nvarCharMark + "\'" + strwhere.ToString() + "\'";
                    }

                }
                else if (valueType == typeof(DateTime))
                {
                    if (dateConvert)
                    {
                        switch (DBType[0])
                        {
                            case "1": strwhere = "Convert(varchar," + column + ",111)" + operatorMask + "'" + value + "'"; break;
                            case "2": strwhere = "Convert(varchar," + column + ",111)" + operatorMask + "'" + value + "'"; break;
                            case "3": strwhere = column + "{0}to_Date('" + value + "', 'yyyy/mm/dd')"; break;
                            case "4":
                                {
                                    if (DBType[1] == "0")
                                    {
                                        strwhere = string.Format("to_Date('{0}', '%Y%m%d%H%M%S')", value);
                                    }
                                    else
                                    {
                                        strwhere = value;
                                    }
                                    break;
                                }
                            case "5": strwhere = "Convert(varchar," + column + ",111)" + operatorMask + "'" + value + "'"; break;
                            case "6": strwhere = value; break;
                            case "7": strwhere = strwhere = "Convert(varchar," + column + ",111)" + operatorMask + value; break;
                        }
                    }
                    else
                    {
                        DateTime dt = (DateTime)Convert.ChangeType(value, typeof(DateTime));//所有时间类型分数据库
                        switch (DBType[0])
                        {
                            case "1":
                                strwhere = string.Format("'{0}-{1}-{2}'", dt.Year, dt.Month, dt.Day);
                                break;
                            case "2":
                                strwhere = string.Format("'{0}-{1}-{2}'", dt.Year, dt.Month, dt.Day);
                                break;
                            case "3":
                                strwhere = string.Format("to_Date('{0:0000}{1:00}{2:00}', 'yyyymmdd')", dt.Year, dt.Month, dt.Day);
                                break;
                            case "4":
                                {
                                    if (DBType[1] == "0")
                                    {
                                        strwhere = string.Format("to_Date('{0:0000}{1:00}{2:00}', '%Y%m%d')", dt.Year, dt.Month, dt.Day);
                                    }
                                    else
                                    {
                                        strwhere = string.Format("{{1:00}/{2:00}/{0:0000}}", dt.Year, dt.Month, dt.Day);
                                    }
                                    break;
                                }
                            case "5":
                                strwhere = string.Format("'{0}-{1}-{2}'", dt.Year, dt.Month, dt.Day);
                                break;
                            case "6":
                                strwhere = string.Format("to_date('{0:0000}{1:00}{2:00}{3:00}{4:00}{5:00}', '%Y%m%d%H%M%S')", dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, dt.Millisecond);
                                break;
                            case "7":
                                strwhere = string.Format("'{0}-{1}-{2} {3}:{4}:{5}.{6}'", dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, dt.Millisecond);
                                break;
                        }
                        //switch (DBType[0])
                        //{
                        //    case "1":
                        //        strwhere = string.Format("'{0}-{1}-{2} {3}:{4}:{5}.{6}'", dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, dt.Millisecond);
                        //        break;
                        //    case "2":
                        //        strwhere = string.Format("'{0}-{1}-{2} {3}:{4}:{5}.{6}'", dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, dt.Millisecond);
                        //        break;
                        //    case "3":
                        //        strwhere = string.Format("to_Date('{0:0000}{1:00}{2:00}, {3:00}:{4:00}:{5:00} ', 'yyyymmdd hh24:mi:ss')", dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second);
                        //        break;
                        //    case "4":
                        //        {
                        //            if (DBType[1] == "0")
                        //            {
                        //                strwhere = string.Format("to_Date('{0:0000}{1:00}{2:00}{3:00}{4:00}{5:00}', '%Y%m%d%H%M%S')", dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second);
                        //            }
                        //            else
                        //            {
                        //                strwhere = string.Format("{{1:00}/{2:00}/{0:0000}}", dt.Year, dt.Month, dt.Day);
                        //            }
                        //            break;
                        //        }
                        //    case "5":
                        //        strwhere = string.Format("'{0}-{1}-{2} {3}:{4}:{5}.{6}'", dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, dt.Millisecond);
                        //        break;
                        //}
                    }
                }
                else if (valueType == typeof(bool))//checkbox redefination
                {
                    strwhere = bool.Equals(value, "Y") ? "1" : "0";
                }
                else if (valueType == typeof(Guid))
                {
                    try
                    {
                        Guid id = new Guid(value.ToString());
                        strwhere = value.ToString();
                    }
                    catch (FormatException)
                    {
                        throw new InvalidCastException(string.Format("Can not convert '{0}' to {1} type", value, valueType.Name));
                    }
                }
                else if (valueType == typeof(String))
                {
                    strwhere = value.ToString().Replace("'", "''");
                    strwhere = nvarCharMark + "\'" + strwhere.ToString() + "\'";
                }
                else if (valueType == typeof(int))
                {
                    int res = 0;
                    if (Int32.TryParse(value, out res))
                    {
                        Convert.ChangeType(value, valueType);
                        strwhere = value.ToString().Replace("'", "''");
                    }
                    else
                    {
                        throw new Exception(column + " must be entered a int.");
                    }
                }
                else
                {
                    Convert.ChangeType(value, valueType);
                    strwhere = value.ToString().Replace("'", "''");
                }
            }
            catch (InvalidCastException)
            {
                throw new InvalidCastException(string.Format("Can not convert '{0}' to {1} type", value, valueType.Name));
            }
            return strwhere;
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

        private String GetTableName()
        {
            String tabName = String.Empty;
            String strModuleName = ((InfoDataSet)innerAnyQuery.BindingSource.DataSource).RemoteName;
            strModuleName = strModuleName.Substring(0, strModuleName.IndexOf('.'));
            tabName = CliUtils.GetTableName(strModuleName, innerAnyQuery.BindingSource.DataMember, CliUtils.fCurrentProject, "", true);

            return tabName;
        }

        DataSet dsDDMaster;
        DataSet dsDDDetail;
        private String GetHeaderText(String ColName, InfoBindingSource ibs)
        {
            if (dsDDMaster == null)
            {
                dsDDMaster = DBUtils.GetDataDictionary(ibs, false);
            }
            String strHeaderText = String.Empty;
            if (dsDDMaster != null && dsDDMaster.Tables.Count > 0)
            {
                int i = dsDDMaster.Tables[0].Rows.Count;
                for (int j = 0; j < i; j++)
                {
                    if (dsDDMaster.Tables[0].Rows[j]["FIELD_NAME"].ToString().ToLower() == ColName.ToLower())
                    {
                        strHeaderText = dsDDMaster.Tables[0].Rows[j]["CAPTION"].ToString();
                        break;
                    }
                }
            }
            if (strHeaderText == String.Empty)
                strHeaderText = ColName;
            return strHeaderText;
        }

        private String GetDetailHeaderText(String ColName, InfoBindingSource ibs)
        {
            if (dsDDDetail == null)
            {
                dsDDDetail = DBUtils.GetDataDictionary(ibs, false);
            }
            String strHeaderText = String.Empty;
            if (dsDDDetail != null && dsDDDetail.Tables.Count > 0)
            {
                int i = dsDDDetail.Tables[0].Rows.Count;
                for (int j = 0; j < i; j++)
                {
                    if (dsDDDetail.Tables[0].Rows[j]["FIELD_NAME"].ToString().ToLower() == ColName.ToLower())
                    {
                        strHeaderText = dsDDDetail.Tables[0].Rows[j]["CAPTION"].ToString();
                        break;
                    }
                }
            }
            if (strHeaderText == String.Empty)
                strHeaderText = ColName;
            return strHeaderText;
        }

        private String CaptionToColumn(String caption, InfoBindingSource ibs)
        {
            String column = String.Empty;

            InfoDataSet ids = ibs.DataSource as InfoDataSet;
            if (ids != null)
            {
                foreach (DataColumn dc in ids.RealDataSet.Tables[0].Columns)
                {
                    if (GetHeaderText(dc.ColumnName, ibs) == caption)
                    {
                        column = dc.ColumnName;
                        break;
                    }
                }
            }
            return column;
        }

        DataTable aDataTable = null;
        String masterSql = String.Empty;
        String detailSql = String.Empty;
        private DataView CreateDataViewColumn(InfoDataSet ids)
        {
            if (aDataTable == null)
            {
                aDataTable = new DataTable("Columns");
                aDataTable.Columns.Add("CAPTION");
                aDataTable.Columns.Add("FIELDNAME");
                aDataTable.Columns.Add(new DataColumn("DATATYPE", typeof(Type)));

                String[] remotename = (innerAnyQuery.BindingSource.DataSource as InfoDataSet).RemoteName.Split('.');
                String strModuleName = remotename[0];
                String strTableName = remotename[1];
                String tablename = CliUtils.GetTableName(strModuleName, strTableName, CliUtils.fCurrentProject);
                //if (!tablename.Contains("[") && !tablename.Contains("}"))
                //{
                //    tablename = String.Format("[{0}]", tablename);
                //}
                String detailtablename = String.Empty;
                String strDetailTableName = String.Empty;
                String[] quote = CliUtils.GetDataBaseQuote();
                int iRelationPos = -1;

                if (innerAnyQuery.DetailBindingSource != null)
                {
                    DataSet dSet = ((InfoDataSet)innerAnyQuery.DetailBindingSource.GetDataSource()).RealDataSet;
                    for (int i = 0; i < dSet.Relations.Count; i++)
                    {
                        if (innerAnyQuery.DetailBindingSource.DataMember == dSet.Relations[i].RelationName)
                        {
                            iRelationPos = i;
                            break;
                        }
                    }

                    DataView dataView = innerAnyQuery.DetailBindingSource.List as DataView;
                    if (dataView != null)
                    {
                        strDetailTableName = dataView.Table.TableName;
                    }
                    else
                    {
                        if (iRelationPos != -1)
                        {
                            strDetailTableName = dSet.Relations[iRelationPos].ChildTable.TableName;
                        }
                    }
                    detailtablename = CliUtils.GetTableName(strModuleName, strDetailTableName, CliUtils.fCurrentProject);
                    //if (!detailtablename.Contains("[") && !detailtablename.Contains("}"))
                    //{
                    //    detailtablename = String.Format("[{0}]", detailtablename);
                    //}
                }
                if (masterSql == String.Empty)
                    masterSql = CliUtils.GetSqlCommandText(strModuleName, strTableName, CliUtils.fCurrentProject);
                if (detailSql == String.Empty)
                    detailSql = CliUtils.GetSqlCommandText(strModuleName, strDetailTableName, CliUtils.fCurrentProject);

                if (innerAnyQuery.QueryColumnMode == AnyQueryColumnMode.ByBindingSource)
                {
                    foreach (DataColumn dc in ids.RealDataSet.Tables[0].Columns)
                    {
                        bool flag = false;
                        String sqlStr = masterSql;
                        String tName = tablename;

                        foreach (AnyQueryColumns aqc in innerAnyQuery.Columns)
                        {
                            if (!aqc.Column.StartsWith("Detail."))
                            {
                                if (dc.ColumnName == aqc.Column)
                                {
                                    if (aqc.Column != aqc.Caption)
                                    {
                                        DataRow drNew = aDataTable.NewRow();
                                        drNew["CAPTION"] = aqc.Caption;
                                        drNew["FIELDNAME"] = CliUtils.GetTableNameForColumn(sqlStr, dc.ColumnName); //drNew["FIELDNAME"] = aqc.Column;
                                        drNew["DATATYPE"] = dc.DataType;
                                        aDataTable.Rows.Add(drNew);
                                    }
                                    else
                                    {
                                        DataRow drNew = aDataTable.NewRow();
                                        drNew["CAPTION"] = GetHeaderText(dc.ColumnName, innerAnyQuery.BindingSource);
                                        drNew["FIELDNAME"] = CliUtils.GetTableNameForColumn(sqlStr, dc.ColumnName);
                                        drNew["DATATYPE"] = dc.DataType;
                                        aDataTable.Rows.Add(drNew);
                                    }
                                    flag = true;
                                    break;
                                }
                            }
                        }
                        if (!flag)
                        {
                            DataRow drNew = aDataTable.NewRow();
                            drNew["CAPTION"] = GetHeaderText(dc.ColumnName, innerAnyQuery.BindingSource);
                            drNew["FIELDNAME"] = CliUtils.GetTableNameForColumn(sqlStr, dc.ColumnName);
                            drNew["DATATYPE"] = dc.DataType;
                            aDataTable.Rows.Add(drNew);
                        }
                    }

                    if (iRelationPos != -1)
                    {
                        foreach (DataColumn dc in ids.RealDataSet.Relations[iRelationPos].ChildTable.Columns)
                        {
                            bool flag = false;
                            String sqlStr = detailSql;
                            String tName = detailtablename;

                            foreach (AnyQueryColumns aqc in innerAnyQuery.Columns)
                            {
                                if ("Detail." + dc.ColumnName == aqc.Column)
                                {
                                    if (aqc.Column != aqc.Caption)
                                    {
                                        DataRow drNew = aDataTable.NewRow();
                                        drNew["CAPTION"] = aqc.Caption;
                                        drNew["FIELDNAME"] = CliUtils.GetTableNameForColumn(sqlStr, dc.ColumnName);
                                        drNew["DATATYPE"] = dc.DataType;
                                        aDataTable.Rows.Add(drNew);
                                    }
                                    else
                                    {
                                        DataRow drNew = aDataTable.NewRow();
                                        drNew["CAPTION"] = GetDetailHeaderText(dc.ColumnName, innerAnyQuery.DetailBindingSource);
                                        drNew["FIELDNAME"] = CliUtils.GetTableNameForColumn(sqlStr, dc.ColumnName);
                                        drNew["DATATYPE"] = dc.DataType;
                                        aDataTable.Rows.Add(drNew);
                                    }
                                    flag = true;
                                    break;
                                }
                            }
                            if (!flag)
                            {
                                DataRow drNew = aDataTable.NewRow();
                                drNew["CAPTION"] = GetDetailHeaderText(dc.ColumnName, innerAnyQuery.DetailBindingSource);
                                drNew["FIELDNAME"] = CliUtils.GetTableNameForColumn(sqlStr, dc.ColumnName);
                                drNew["DATATYPE"] = dc.DataType;
                                aDataTable.Rows.Add(drNew);
                            }
                        }
                    }
                }
                else if (innerAnyQuery.QueryColumnMode == AnyQueryColumnMode.ByColumns)
                {
                    foreach (AnyQueryColumns aqc in innerAnyQuery.Columns)
                    {
                        if (!aqc.Column.StartsWith("Detail."))
                        {
                            foreach (DataColumn dc in ids.RealDataSet.Tables[0].Columns)
                            {
                                String sqlStr = masterSql;
                                String tName = tablename;
                                String cName = dc.ColumnName;

                                if (aqc.Column == cName)
                                {
                                    DataRow drNew = aDataTable.NewRow();
                                    drNew["CAPTION"] = aqc.Caption;
                                    drNew["FIELDNAME"] = CliUtils.GetTableNameForColumn(sqlStr, dc.ColumnName);
                                    drNew["DATATYPE"] = dc.DataType;
                                    aDataTable.Rows.Add(drNew);

                                    break;
                                }
                            }
                        }
                        else if (iRelationPos != -1)
                        {
                            foreach (DataColumn dc in ids.RealDataSet.Relations[iRelationPos].ChildTable.Columns)
                            {
                                String sqlStr = detailSql;
                                String tName = detailtablename;
                                String cName = "Detail." + dc.ColumnName;

                                if (aqc.Column == cName)
                                {
                                    DataRow drNew = aDataTable.NewRow();
                                    drNew["CAPTION"] = aqc.Caption;
                                    drNew["FIELDNAME"] = CliUtils.GetTableNameForColumn(sqlStr, dc.ColumnName);
                                    drNew["DATATYPE"] = dc.DataType;
                                    aDataTable.Rows.Add(drNew);

                                    break;
                                }

                            }

                        }

                    }
                }
            }
            return new DataView(aDataTable);
        }
    }
}