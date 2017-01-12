using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Reflection;

namespace Srvtools
{
    public partial class frmNavQuery : Form
    {
        private SYS_LANGUAGE language;
        private QueryFieldCollection queryFields;
        private InfoBindingSource bSource;
        private InfoNavigator infoNavigator;
        string[] colName;
        int colNum;
        public frmNavQuery(QueryFieldCollection queryfields, InfoBindingSource bs, InfoNavigator nav)
        {
            //language = CliSysMegLag.GetClientLanguage();
            language = CliUtils.fClientLang;
            queryFields = queryfields;

            if (queryfields == null || queryfields.Count == 0)
            {
                DataColumnCollection dcc = ((InfoDataSet)bs.GetDataSource()).RealDataSet.Tables[bs.DataMember].Columns;
                colNum = dcc.Count;
                colName = new string[colNum];
                for (int j = 0; j < colNum; j++)
                {
                    colName[j] = dcc[j].ColumnName;
                }
            }
            else
            {
                colNum = queryfields.Count;
                colName = new string[colNum];
                for (int j = 0; j < colNum; j++)
                {
                    colName[j] = queryfields[j].FieldName;
                }
            }
            bSource = bs;
            infoNavigator = nav;
            InitializeComponent();
            InitializeQueryConditionItem();
        }

        private DataSet ds = new DataSet();
        private void DataDictionary()
        {
            ds = DBUtils.GetDataDictionary(bSource, false);
        }

        private string GetCaption(string strFieldName, int index)
        {
            string strCaption = "";
            string CaptionNum = "CAPTION";
            //if (infoNavigator.MultiLanguage)
            //{
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
            //}
            int i = ds.Tables[0].Rows.Count;
            for (int j = 0; j < i; j++)
            {
                if (string.Compare(ds.Tables[0].Rows[j]["FIELD_NAME"].ToString(), strFieldName, true) == 0)//IgnoreCase
                {
                    strCaption = ds.Tables[0].Rows[j][CaptionNum].ToString();
                    if (strCaption == "")
                    {
                        strCaption = ds.Tables[0].Rows[j]["CAPTION"].ToString();
                    }
                }
            }
            if (strCaption == "")
            {
                strCaption = strFieldName;
            }
            if (this.queryFields != null && this.queryFields.Count > 0)
            {
                int x = this.queryFields.Count;
                for (int y = 0; y < x; y++)
                {
                    if (y == index && this.queryFields[y].FieldName == strFieldName &&
                        this.queryFields[y].Caption != null && this.queryFields[y].Caption != "")
                    {
                        strCaption = this.queryFields[y].Caption;
                        break;
                    }
                }
            }
            return strCaption;
        }

        private Label[] labels;
        private ComboBox[] comboBoxes;
        private TextBox[] textBoxes;
        private void InitializeQueryConditionItem()
        {
            DataDictionary();
            labels = new Label[colNum];
            comboBoxes = new ComboBox[colNum];
            textBoxes = new TextBox[colNum];

            int LocationY = 10;
            for (int i = 0; i < colNum; i++)
            {
                // labels 
                labels[i] = new Label();
                labels[i].AutoSize = true;
                if (LocationY == 0)
                    labels[i].Location = new System.Drawing.Point(68, LocationY += 12);
                else
                    labels[i].Location = new System.Drawing.Point(68, LocationY += 26);
                labels[i].AutoSize = false;
                labels[i].Name = "lbl" + i.ToString();//colName[i];
                labels[i].Size = new System.Drawing.Size(150, 12);
                labels[i].Text = GetCaption(colName[i], i);
                labels[i].BackColor = System.Drawing.Color.Transparent;

                // combobox
                comboBoxes[i] = new ComboBox();
                comboBoxes[i].FormattingEnabled = true;
                comboBoxes[i].Items.AddRange(new object[] { "<=", "<", "=", "!=", ">", ">=", "%", "%%" });
                comboBoxes[i].Location = new System.Drawing.Point(18, LocationY += 18);
                comboBoxes[i].Name = "cmb" + i.ToString();//colName[i];
                comboBoxes[i].Size = new System.Drawing.Size(40, 20);
                comboBoxes[i].DropDownStyle = ComboBoxStyle.DropDownList;

                // TextBox
                textBoxes[i] = new TextBox();
                textBoxes[i].Location = new System.Drawing.Point(68, LocationY);
                textBoxes[i].Name = "txt" + i.ToString();//colName[i];
                textBoxes[i].Size = new System.Drawing.Size(240, 20);
                if (i < queryFields.Count)
                {
                    textBoxes[i].Text = GetDefaultValue((queryFields[i] as QueryField).DefaultValue);
                }
                Type type = ((InfoDataSet)bSource.DataSource).RealDataSet.Tables[bSource.DataMember].Columns[colName[i]].DataType;
                textBoxes[i].Tag = type;
                textBoxes[i].KeyPress += delegate(object sender, KeyPressEventArgs e)
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



                // frmNavQuery
                this.splitContainer1.Panel1.Controls.Add(labels[i]);
                this.splitContainer1.Panel1.Controls.Add(comboBoxes[i]);
                this.splitContainer1.Panel1.Controls.Add(textBoxes[i]);
            }
            language = CliUtils.fClientLang;
            String message = SysMsg.GetSystemMessage(language,
                                 "Srvtools",
                                 "InfoNavigator",
                                 "ButtonName");
            string[] buttons = message.Split(';');
            this.btnOK.Text = buttons[0];
            this.btnCancel.Text = buttons[1];


            String title = SysMsg.GetSystemMessage(language,
                               "Srvtools",
                               "InfoNavigator",
                               "QueryFormTitle");
            this.Text = title;

            // add empty label for bottom
            Label labeltmp = new Label();
            labeltmp.Location = new System.Drawing.Point(68, LocationY += 24);
            labeltmp.Text = "";
            labeltmp.BackColor = System.Drawing.Color.Transparent;
            this.splitContainer1.Panel1.Controls.Add(labeltmp);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            infoNavigator.PreQueryField.Clear();
            infoNavigator.PreQueryCondition.Clear();
            infoNavigator.PreQueryValue.Clear();

            string sqlcmd = DBUtils.GetCommandText(bSource);

            string strQueryCondition = "";
            for (int i = 0; i < colNum; i++)
            {
                if (comboBoxes[i].Text != string.Empty && textBoxes[i].Text != string.Empty)
                {
                    string columnName = CliUtils.GetTableNameForColumn(sqlcmd, colName[i]);
                    Type type = ((InfoDataSet)bSource.DataSource).RealDataSet.Tables[bSource.DataMember].Columns[colName[i]].DataType;
                    string nvarCharMark = type == typeof(string) && queryFields.Count > 0 && (queryFields[i] as QueryField).IsNvarChar ? "N" : string.Empty;
                    if (string.Compare(textBoxes[i].Text.Trim(), "null", true) == 0) //IgnoreCase                       
                    {
                        if (type == typeof(uint) || type == typeof(UInt16) || type == typeof(UInt32)
                               || type == typeof(UInt64) || type == typeof(int) || type == typeof(Int16)
                               || type == typeof(Int32) || type == typeof(Int64) || type == typeof(Single)
                               || type == typeof(Double) || type == typeof(Decimal) || type == typeof(DateTime))
                        {
                            if (comboBoxes[i].Text.Equals("!="))
                            {
                                strQueryCondition += columnName + "is not null and ";
                            }
                            else
                            {
                                strQueryCondition += columnName + "is null and ";
                            }
                        }
                        else
                        {
                            if (comboBoxes[i].Text.Equals("!="))
                            {
                                strQueryCondition += "(" + columnName + "is not null and" + columnName + "<>'') and ";
                            }
                            else
                            {
                                strQueryCondition += "(" + columnName + "is null or" + columnName + "='') and ";
                            }
                        }
                    }
                    else
                    {
                        if (type == typeof(Guid))
                        {
                            try
                            {
                                Guid id = new Guid(textBoxes[i].Text);
                            }
                            catch (FormatException)
                            {
                                MessageBox.Show(string.Format("{0}'s type should be {1}", labels[i].Text, type.Name));
                                this.DialogResult = DialogResult.None;
                                return;
                            }
                        }
                        else
                        {
                            try
                            {
                                Convert.ChangeType(textBoxes[i].Text, type);
                            }
                            catch
                            {
                                MessageBox.Show(string.Format("{0}'s type should be {1}", labels[i].Text, type.Name));
                                this.DialogResult = DialogResult.None;
                                return;
                            }
                        }
                        if (comboBoxes[i].Text != "%" && comboBoxes[i].Text != "%%")
                        {
                            if (type == typeof(uint) || type == typeof(UInt16) || type == typeof(UInt32)
                                || type == typeof(UInt64) || type == typeof(int) || type == typeof(Int16)
                                || type == typeof(Int32) || type == typeof(Int64) || type == typeof(Single)
                                || type == typeof(Double) || type == typeof(Decimal))
                            {
                                strQueryCondition += columnName + comboBoxes[i].Text + textBoxes[i].Text + " and ";
                            }
                            else if (type == typeof(DateTime))
                            {
                                string[] DBType = getDBType();
                                if (DBType[0] == "1")
                                    strQueryCondition += columnName + comboBoxes[i].Text + " '" + textBoxes[i].Text + "' and ";
                                else if (DBType[0] == "2")
                                    strQueryCondition += columnName + comboBoxes[i].Text + " '" + textBoxes[i].Text + "' and ";
                                else if (DBType[0] == "3")
                                {
                                    string Date = changeDate(textBoxes[i].Text);
                                    strQueryCondition += columnName + comboBoxes[i].Text + " to_Date('" + Date + "', 'yyyymmdd') and ";
                                }
                                else if (DBType[0] == "4")
                                {
                                    DateTime t;
                                    string Date = "";
                                    if (DBType[1] == "0")
                                    {
                                        if (textBoxes[i].Text.Contains("-") || textBoxes[i].Text.Contains("/"))
                                        {
                                            t = Convert.ToDateTime(textBoxes[i].Text);
                                            Date = t.ToString("yyyyMMddHHmmss");
                                        }
                                        else if (textBoxes[i].Text.Length < 14)
                                            Date = textBoxes[i].Text + "000000";

                                        strQueryCondition += columnName + comboBoxes[i].Text + " to_Date('" + Date + "',  '%Y%m%d%H%M%S') and ";
                                    }
                                    else if (DBType[1] == "1")
                                    {
                                        if (textBoxes[i].Text.Contains("-") || textBoxes[i].Text.Contains("/"))
                                        {
                                            t = Convert.ToDateTime(textBoxes[i].Text);
                                            Date = t.ToString("MM/dd/yyyy");
                                        }
                                        else if (textBoxes[i].Text.Length < 14)
                                            Date = textBoxes[i].Text + "000000";

                                        strQueryCondition += columnName + comboBoxes[i].Text + "{" + Date + "} and ";
                                    }
                                }
                                else if (DBType[0] == "5")
                                    strQueryCondition += columnName + comboBoxes[i].Text + " '" + textBoxes[i].Text + "' and ";
                                else if (DBType[0] == "6")
                                {
                                    DateTime t;
                                    string Date = "";
                                    if (textBoxes[i].Text.Contains("-") || textBoxes[i].Text.Contains("/"))
                                    {
                                        t = Convert.ToDateTime(textBoxes[i].Text);
                                        Date = t.ToString("yyyyMMddHHmmss");
                                    }
                                    else if (textBoxes[i].Text.Length < 14)
                                        Date = textBoxes[i].Text + "000000";

                                    strQueryCondition += columnName + comboBoxes[i].Text + " to_Date('" + Date + "',  '%Y%m%d%H%M%S') and ";
                                }
                                else if (DBType[0] == "7")
                                {
                                    strQueryCondition += columnName + comboBoxes[i].Text + " '" + textBoxes[i].Text + "' and ";
                                }
                            }
                            else
                            {
                                strQueryCondition += columnName + comboBoxes[i].Text + " " + nvarCharMark + "'" + textBoxes[i].Text.Replace("'", "''") + "' and ";
                            }
                        }
                        else
                        {
                            if (comboBoxes[i].Text == "%")
                            {
                                strQueryCondition += columnName + " like " + nvarCharMark + "'" + textBoxes[i].Text.Replace("'", "''") + "%' and ";
                            }
                            if (comboBoxes[i].Text == "%%")
                            {
                                strQueryCondition += columnName + " like " + nvarCharMark + "'%" + textBoxes[i].Text.Replace("'", "''") + "%' and ";
                            }
                        }
                    }
                }
                infoNavigator.PreQueryField.Add(colName[i]);
                infoNavigator.PreQueryCondition.Add(comboBoxes[i].Text);
                infoNavigator.PreQueryValue.Add(textBoxes[i].Text);
            }
            if (strQueryCondition != string.Empty)
            {
                strQueryCondition = strQueryCondition.Substring(0, strQueryCondition.LastIndexOf(" and "));
            }
            if (!infoNavigator.QuerySQLSend)
            {
                infoNavigator.OnQueryConfirm(new QueryConfirmEventArgs(strQueryCondition));
            }
            else
            {
                NavigatorQueryWhereEventArgs args = new NavigatorQueryWhereEventArgs(strQueryCondition);
                infoNavigator.OnQueryWhere(args);
                if (!args.Cancel)
                {
                    strQueryCondition = args.WhereString;
                    if (strQueryCondition != string.Empty)
                    {
                        if (infoNavigator.StatusStrip != null && infoNavigator.StatusStrip.ShowProgress)
                        {
                            infoNavigator.StatusStrip.ShowProgressBar();
                        }
                        ((InfoDataSet)(bSource.DataSource)).SetWhere(strQueryCondition);
                        if (infoNavigator.StatusStrip != null && infoNavigator.StatusStrip.ShowProgress)
                        {
                            infoNavigator.StatusStrip.HideProgressBar();
                        }
                        if (bSource.List.Count == 0 && infoNavigator.ViewBindingSource != null
                            && infoNavigator.ViewBindingSource != infoNavigator.BindingSource)
                        {
                            //    infoNavigator.BindingSource.Filter = "1<>1";
                            InfoDataSet ids = infoNavigator.BindingSource.DataSource as InfoDataSet;
                            if (ids != null)
                            {
                                ids.SetWhere("1=0");
                            }
                        }
                        //else
                        //{
                        //    infoNavigator.BindingSource.Filter = "";
                        //}
                    }
                    else
                    {
                        ((InfoDataSet)(bSource.DataSource)).ClearWhere();
                        if (bSource.List.Count != 0 && infoNavigator.ViewBindingSource != infoNavigator.BindingSource)
                        {
                            infoNavigator.BindingSource.Filter = "";
                        }
                    }
                }
            }
        }

        private string[] getDBType()
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

        private string changeDate(string str)
        {
            char[] mark = { '-', '/' };
            string[] temp = str.Split(mark);
            string Date = "";
            for (int i = 0; i < temp.Length; i++)
            {
                if (temp[i].Length < 2)
                    temp[i] = '0' + temp[i];
                Date += temp[i];
            }
            return Date;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            NavigatorQueryWhereEventArgs args = new NavigatorQueryWhereEventArgs(null);
            infoNavigator.OnQueryWhere(args);
        }

        private void frmNavQuery_Load(object sender, EventArgs e)
        {
            this.Font = infoNavigator.QueryFont;
            for (int i = 0; i < colNum; i++)
            {
                Type type = ((InfoDataSet)bSource.DataSource).RealDataSet.Tables[bSource.DataMember].Columns[i].DataType;
                if (type == typeof(String))
                {
                    comboBoxes[i].Text = "%";
                }
                else
                {
                    comboBoxes[i].Text = "=";
                }
                if (this.queryFields != null && this.queryFields.Count > 0)
                {
                    comboBoxes[i].Text = this.queryFields[i].Condition;
                }
            }
            if (infoNavigator.QueryKeepCondition
                && infoNavigator.PreQueryField.Count > 0
                && infoNavigator.PreQueryCondition.Count > 0
                && infoNavigator.PreQueryValue.Count > 0)
            {
                foreach (Control ctrl in this.splitContainer1.Panel1.Controls)
                {
                    int m = infoNavigator.PreQueryField.Count;
                    for (int n = 0; n < m; n++)
                    {
                        if (string.Compare(ctrl.Name, "cmb" + n.ToString(), true) == 0)//IgnoreCase
                        {
                            ((ComboBox)ctrl).Text = infoNavigator.PreQueryCondition[n];
                        }
                        if (string.Compare(ctrl.Name, "txt" + n.ToString(), true) == 0)//IgnoreCase
                        {
                            ((TextBox)ctrl).Text = infoNavigator.PreQueryValue[n];
                        }
                    }
                }
            }
        }

        private String Quote(String table_or_column)
        {
            object[] param = new object[1];
            param[0] = CliUtils.fLoginDB;
            object[] myRet = CliUtils.CallMethod("GLModule", "GetDataBaseType", param);
            string type = "";
            if (myRet != null && (int)myRet[0] == 0)
                type = myRet[1].ToString();

            if (type == "1")
            {
                if (_quotePrefix == null || _quoteSuffix == null)
                    return table_or_column;
                return _quotePrefix + table_or_column + _quoteSuffix;
            }
            else if (type == "3")
            {
                return table_or_column;
            }
            else if (type == "2")
            {
                if (_quotePrefix == null || _quoteSuffix == null)
                    return table_or_column;
                return _quotePrefix + table_or_column + _quoteSuffix;
            }
            else if (type == "4")
            {
                return table_or_column;
            }
            else if (type == "5")
            {
                return table_or_column;
            }
            else if (type == "6")
            {
                return table_or_column;
            }
            return _quotePrefix + table_or_column + _quoteSuffix;
        }

        private string GetDefaultValue(string value)
        {
            return CliUtils.GetValue(value, bSource.OwnerComp).ToString();
        }

        private String _quotePrefix = "[";
        private String _quoteSuffix = "]";
    }
}