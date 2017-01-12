using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace Srvtools
{
    public partial class frmRefValQuery : Form
    {
        private string[] strColName;
        private String[] strColCaption;
        private InfoBindingSource infoBs;
        int colNum;
        string strOrignFilter;
        InfoRefVal infoRefVal;
        public frmRefValQuery(string[] colName, String[] colCaption, InfoBindingSource bs, string OrignFilter, InfoRefVal refval)
        {
            InitializeComponent();

            int i = colName.Length;
            colNum = i;
            this.strColName = colName;
            this.strColCaption = colCaption;
            this.infoBs = bs;
            this.strOrignFilter = OrignFilter;
            this.infoRefVal = refval;
            InitializeQueryConditionItem();
        }

        private DataSet ds = new DataSet();
        private void DataDictionary()
        {
            object[] tabName = new object[1];
            if (infoRefVal.SelectCommand == null || infoRefVal.SelectCommand == "")
            {
                tabName[0] = ((DataView)infoBs.List).Table.TableName;
                string RemoteName = ((InfoDataSet)infoBs.DataSource).RemoteName;
                string moduleName = RemoteName.Substring(0, RemoteName.IndexOf('.'));
                //modified by lily 2007/7/16 取DD的時候，tablename不應考慮別名，而應該直接抓from的名字
                tabName[0] = CliUtils.GetTableName(moduleName, tabName[0].ToString(), CliUtils.fCurrentProject, "", true);
            }
            else
            {
                //modified by lily 2007/7/16 取DD的時候，tablename不應考慮別名，而應該直接抓from的名字
                tabName[0] = CliUtils.GetTableName(this.infoRefVal.SelectCommand, true);
            }
            object[] myRet = CliUtils.CallMethod("GLModule", "GetDataDic", tabName);
            if ((null != myRet) && (0 == (int)myRet[0]))
                ds = (DataSet)(myRet[1]);
        }

        private string GetCaption(string strFieldName)
        {
            for (int I = 0; I < strColName.Length; I++)
            {
                if (strColName[I].CompareTo(strFieldName) == 0)
                {
                    return strColCaption[I];
                }
            }

            string strCaption = "";
            if (ds != null && ds.Tables.Count > 0)
            {
                int i = ds.Tables[0].Rows.Count;
                for (int j = 0; j < i; j++)
                {
                    if (string.Compare(ds.Tables[0].Rows[j]["FIELD_NAME"].ToString(), strFieldName, true) == 0)//IgnoreCase
                    {
                        strCaption = ds.Tables[0].Rows[j]["CAPTION"].ToString();
                    }
                }
            }
            if (strCaption == "")
            {
                strCaption = strFieldName;
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
                    labels[i].Location = new System.Drawing.Point(68, LocationY += 11);
                else
                    labels[i].Location = new System.Drawing.Point(68, LocationY += 26);
                labels[i].Name = "lbl" + strColName[i].ToString();
                labels[i].AutoSize = false;
                labels[i].Size = new System.Drawing.Size(150, 12);
                labels[i].Text = GetCaption(strColName[i].ToString());
                labels[i].BackColor = System.Drawing.Color.Transparent;

                // combobox
                comboBoxes[i] = new ComboBox();
                comboBoxes[i].FormattingEnabled = true;
                comboBoxes[i].Items.AddRange(new object[] { "<=", "<", "=", "!=", ">", ">=", "%", "%%" });
                comboBoxes[i].Location = new System.Drawing.Point(18, LocationY += 18);
                comboBoxes[i].Name = "cmb" + strColName[i].ToString();
                //comboBoxes[i].SelectedIndex = 2;
                comboBoxes[i].Size = new System.Drawing.Size(40, 20);
                comboBoxes[i].DropDownStyle = ComboBoxStyle.DropDownList;
                comboBoxes[i].TabStop = false;

                // TextBox
                textBoxes[i] = new TextBox();
                textBoxes[i].Location = new System.Drawing.Point(68, LocationY);
                textBoxes[i].Name = "txt" + strColName[i].ToString();
                textBoxes[i].Size = new System.Drawing.Size(240, 20);
                textBoxes[i].Tag = i;
                textBoxes[i].KeyDown += new KeyEventHandler((sender, args) =>
                {
                    if (args.KeyCode == Keys.Enter)
                    {
                        if ((sender as TextBox).Tag != null && ((int)(sender as TextBox).Tag) == colNum - 1)
                        {
                            btnOK.PerformClick();
                        }
                        else
                            SendKeys.Send("{Tab}");
                    }
                });

                // frmRefValQuery
                this.splitContainer1.Panel1.Controls.Add(labels[i]);
                this.splitContainer1.Panel1.Controls.Add(comboBoxes[i]);
                this.splitContainer1.Panel1.Controls.Add(textBoxes[i]);
            }
            // add empty label for bottom
            Label labeltmp = new Label();
            labeltmp.Location = new System.Drawing.Point(68, LocationY += 24);
            labeltmp.Text = "";
            labeltmp.BackColor = System.Drawing.Color.Transparent;
            this.splitContainer1.Panel1.Controls.Add(labeltmp);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string sqlcmd = "";
            if (this.infoRefVal.SelectAlias != null && this.infoRefVal.SelectAlias != "" && this.infoRefVal.SelectCommand != null && this.infoRefVal.SelectCommand != "")
            {
                sqlcmd = this.infoRefVal.SelectCommand;
            }
            else
            {
                sqlcmd = DBUtils.GetCommandText(infoBs);
            }

            string strQueryCondition = "";
            for (int i = 0; i < colNum; i++)
            {
                if (comboBoxes[i].Text != string.Empty && textBoxes[i].Text != string.Empty)
                {
                    string columnName = CliUtils.GetTableNameForColumn(sqlcmd, strColName[i]);
                    Type type = ((DataView)infoBs.List).Table.Columns[strColName[i]].DataType;
                    string nvarCharMark = type == typeof(string) && this.infoRefVal.Columns.Count > 0 && this.infoRefVal.Columns[i].IsNvarChar ? "N" : string.Empty;

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

                        if (comboBoxes[i].Text != "%" && comboBoxes[i].Text != "%%")
                        {
                            if (type == typeof(uint) || type == typeof(UInt16) || type == typeof(UInt32)
                                || type == typeof(UInt64) || type == typeof(int) || type == typeof(Int16)
                                || type == typeof(Int32) || type == typeof(Int64) || type == typeof(Single)
                                || type == typeof(float) || type == typeof(Double) || type == typeof(Decimal))
                            {
                                strQueryCondition += columnName
                                    + comboBoxes[i].Text + textBoxes[i].Text + " and ";
                            }
                            else
                            {
                                strQueryCondition += columnName
                                    + comboBoxes[i].Text + " " + nvarCharMark + "'" + textBoxes[i].Text.Replace("'", "''") + "' and ";
                            }
                        }
                        else
                        {
                            if (comboBoxes[i].Text == "%")
                            {
                                strQueryCondition += columnName
                                    + " like " + nvarCharMark + "'" + textBoxes[i].Text.Replace("'", "''") + "%' and ";
                            }
                            if (comboBoxes[i].Text == "%%")
                            {
                                strQueryCondition += columnName
                                    + " like " + nvarCharMark + "'%" + textBoxes[i].Text.Replace("'", "''") + "%' and ";
                            }
                        }
                    }
                }
            }
            if (strOrignFilter != "")
            {
                strQueryCondition += strOrignFilter;
            }
            else if (strQueryCondition != "")
            {
                strQueryCondition = strQueryCondition.Substring(0, strQueryCondition.LastIndexOf(" and "));
            }
            //this.infoBs.Filter = strQueryCondition;
            string strwhere = this.infoRefVal.WhereString(sqlcmd);
            InfoDataSet infoDs = (InfoDataSet)this.infoBs.GetDataSource();
            if (this.infoRefVal.SelectAlias != null && this.infoRefVal.SelectAlias != ""
                && this.infoRefVal.SelectCommand != null && this.infoRefVal.SelectCommand != "")
            {
                string cmd = this.infoRefVal.SelectCommand;
                if (strQueryCondition != "")
                {
                    cmd = CliUtils.InsertWhere(cmd, strQueryCondition);
                }
                cmd = CliUtils.InsertWhere(cmd, strwhere);// insert whereitem 
                #region where string
                /*if (strQueryCondition != "")
                {
                    if (cmd.IndexOf(" where ") != -1)
                    {
                        cmd += " and " + strQueryCondition;
                    }
                    else
                    {
                        cmd += " where " + strQueryCondition;
                    }
                }*/
                #endregion
                infoDs.Execute(cmd, "", true);
            }
            else
            {
                if (strwhere.Length > 0)
                {
                    if (strQueryCondition.Length > 0)
                    {
                        strQueryCondition += " And " + strwhere;
                    }
                    else
                    {
                        strQueryCondition = strwhere;
                    }
                }
                infoDs.SetWhere(strQueryCondition);
            }

            this.Close();
        }

        private void frmRefValQuery_Load(object sender, EventArgs e)
        {
            SYS_LANGUAGE language = CliUtils.fClientLang;
            this.Text = SysMsg.GetSystemMessage(language, "Srvtools", "frmRefValQuery", "FormName");

            string TableName = ((DataView)this.infoBs.List).Table.TableName;
            for (int i = 0; i < colNum; i++)
            {
                Type type = ((DataView)this.infoBs.List).Table.Columns[i].DataType;
                if (type == typeof(String))
                {
                    comboBoxes[i].Text = "%";
                }
                else
                {
                    comboBoxes[i].Text = "=";
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}