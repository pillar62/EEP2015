using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Collections;

namespace Srvtools
{
    public partial class InfoRefPanel : Form
    {
        public InfoRefPanel()
        {
            InitializeComponent();
        }

        private InfoRefButton refButton;
        public InfoRefPanel(InfoRefButton irb)
        {
            InitializeComponent();
            refButton = irb;
            if (refButton.SearchColumns.Count == 0)
            {
                this.pSearch.Visible = false;
            }
            else
            {
                foreach (RefButtonSearchColumn item in refButton.SearchColumns)
                {
                    label1.Text += String.Format("{0},", item.ColumnHeader);
                }
                label1.Text = label1.Text.Remove(label1.Text.Length - 1);
            }
        }

        //end add
        private void InfoRefPanel_Load(object sender, EventArgs e)
        {
            String[] texts = SysMsg.GetSystemMessage(CliUtils.fClientLang, "Srvtools", "InfoRefPanel", "Text").Split(';');
            this.btnOK.Text = texts[0];
            this.btnCancel.Text = texts[1];
            this.btnQuery.Text = texts[2];

            if (refButton.autoPanel)
            {
                if (refButton.Columns != null && refButton.Columns.Count > 0)
                {
                    this.infoDataGridView1.AutoGenerateColumns = false;
                    foreach (RefColumns column in refButton.Columns)
                    {
                        DataGridViewTextBoxColumn textBoxColumn = new DataGridViewTextBoxColumn();
                        textBoxColumn.Name = column.Column;
                        textBoxColumn.DataPropertyName = column.Column;
                        textBoxColumn.HeaderText = column.HeaderText;
                        textBoxColumn.DefaultCellStyle = column.DefaultCellStyle;
                        textBoxColumn.Width = column.Width;
                        textBoxColumn.Visible = column.Visible;
                        this.infoDataGridView1.Columns.Add(textBoxColumn);
                    }
                }
                this.infoDataGridView1.DataSource = refButton.infoTranslate.BindingSource;
                foreach (DataGridViewColumn dgvc in this.infoDataGridView1.Columns)
                {
                    if (dgvc.Name == "SelectItems")
                    {
                        dgvc.HeaderText = texts[3];
                        if (!refButton.multiSelect)
                        {
                            dgvc.Visible = false;
                            this.infoDataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                        }
                    }
                    else
                    {
                        if (refButton.Columns == null || refButton.Columns.Count == 0)
                        {
                            dgvc.HeaderText = GetHeaderText(dgvc.DataPropertyName);
                        }
                        dgvc.ReadOnly = true;
                    }
                }
            }
            else
            {
                this.btnQuery.Visible = false;

                this.infoDataGridView1.Visible = false;
                this.panel1.Controls.Add(refButton.panel);
                refButton.panel.Dock = DockStyle.Fill;
                refButton.panel.Show();
            }
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        public ArrayList value;
        public ArrayList display;
        private void btnOK_Click(object sender, EventArgs e)
        {
            value = new ArrayList();
            display = new ArrayList();
            if (refButton.infoTranslate.RefReturnFields.Count > 0)
            {
                if (refButton.autoPanel)
                {

                    for (int i = 0; i < refButton.infoTranslate.RefReturnFields.Count; i++)
                    {
                        String ColumnName = ((TranslateRefReturnFields)(refButton.infoTranslate.RefReturnFields[i])).ColumnName;
                        String DisplayColumnName = ((TranslateRefReturnFields)(refButton.infoTranslate.RefReturnFields[i])).DisplayColumnName;
                        if (refButton.multiSelect)
                        {
                            String values = String.Empty;
                            String displays = String.Empty;
                            for (int j = 0; j < this.infoDataGridView1.Rows.Count; j++)
                            {
                                if (this.infoDataGridView1.Rows[j].Cells["SelectItems"].Value != null && (bool)this.infoDataGridView1.Rows[j].Cells["SelectItems"].Value)
                                {
                                    values += this.infoDataGridView1.Rows[j].Cells[ColumnName].Value + ",";
                                    displays += this.infoDataGridView1.Rows[j].Cells[DisplayColumnName].Value + ",";
                                }
                            }
                            if (values.Length > 0)
                                values = values.Remove(values.Length - 1);
                            if (displays.Length > 0)
                                displays = displays.Remove(displays.Length - 1);

                            value.Add(values);
                            display.Add(displays);
                        }
                        else
                        {
                            value.Add(((DataRowView)refButton.infoTranslate.BindingSource.Current).Row[ColumnName].ToString());
                            display.Add(((DataRowView)refButton.infoTranslate.BindingSource.Current).Row[DisplayColumnName].ToString());
                        }
                    }


                }
                else
                {
                    for (int i = 0; i < refButton.infoTranslate.RefReturnFields.Count; i++)
                    {
                        String ColumnName = ((TranslateRefReturnFields)(refButton.infoTranslate.RefReturnFields[i])).ColumnName;
                        String DisplayColumnName = ((TranslateRefReturnFields)(refButton.infoTranslate.RefReturnFields[i])).DisplayColumnName;

                        //add multiSelect by luciferling 20080730
                        if (refButton.multiSelect)
                        {
                            String values = String.Empty;
                            String displays = String.Empty;
                            InfoDataGridView idgv = (InfoDataGridView)refButton.panel.Controls[0];

                            int m, n;
                            for (m = 0; m < idgv.ColumnCount; m++)
                            {
                                if (idgv.Columns[m].DataPropertyName.ToString() == ColumnName)
                                    break;
                            }
                            for (n = 0; n < idgv.ColumnCount; n++)
                            {
                                if (idgv.Columns[n].DataPropertyName.ToString() == DisplayColumnName)
                                    break;
                            }


                            for (int k = 0; k < idgv.SelectedRows.Count; k++)
                            {
                                values += idgv.SelectedRows[k].Cells[m].Value + ",";
                                displays += idgv.SelectedRows[k].Cells[n].Value + ",";
                            }
                            if (values.Length > 0)
                                values = values.Remove(values.Length - 1);
                            if (displays.Length > 0)
                                displays = displays.Remove(displays.Length - 1);

                            value.Add(values);
                            display.Add(displays);
                        }
                        //end add
                        else
                        {
                            value.Add(((DataRowView)refButton.infoTranslate.BindingSource.Current).Row[ColumnName].ToString());
                            display.Add(((DataRowView)refButton.infoTranslate.BindingSource.Current).Row[DisplayColumnName].ToString());
                        }
                    }
                }
            }
            this.Hide();
            AfterOKEventArgs args = new AfterOKEventArgs();
            args.Values = value;
            args.Displays = display;
            OnAfterOK(args);
        }

        static readonly object afterOKEventKey = new object();

        public event EventHandler<AfterOKEventArgs> AfterOK
        {
            add { Events.AddHandler(afterOKEventKey, value); }
            remove { Events.RemoveHandler(afterOKEventKey, value); }
        }
        protected void OnAfterOK(AfterOKEventArgs args)
        {
            EventHandler<AfterOKEventArgs> handler = (EventHandler<AfterOKEventArgs>)Events[afterOKEventKey];
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private String GetHeaderText(String ColName)
        {
            string strTableName = refButton.infoTranslate.BindingSource.GetTableName();
            DataSet ds = DBUtils.GetDataDictionary(refButton.infoTranslate.BindingSource, this.DesignMode);

            String strHeaderText = ColName;
            if (ds != null && ds.Tables.Count > 0)
            {
                int i = ds.Tables[strTableName].Rows.Count;
                for (int j = 0; j < i; j++)
                {
                    if (ds.Tables[strTableName].Rows[j]["FIELD_NAME"].ToString().ToLower() == ColName.ToLower())
                    {
                        if (strHeaderText != String.Empty)
                            strHeaderText = ds.Tables[strTableName].Rows[j]["CAPTION"].ToString();
                    }
                }
            }

            return strHeaderText;
        }

        private string strFilter = string.Empty;
        private void btnQuery_Click(object sender, EventArgs e)
        {
            InfoRefVal aInfoRefVal = new InfoRefVal();
            aInfoRefVal.DataSource = refButton.infoTranslate.BindingSource;
            frmRefButtonQuery afrmRefButtonQuery = null;
            int i = infoDataGridView1.Columns.Count;
            string[] colName = new String[i];
            String[] colCaption = new String[i];
            for (int j = 0; j < i; j++)
            {
                colName[j] = infoDataGridView1.Columns[j].Name;
                colCaption[j] = infoDataGridView1.Columns[j].HeaderText;
            }
            afrmRefButtonQuery = new frmRefButtonQuery(colName, colCaption, infoDataGridView1.DataSource as InfoBindingSource, strFilter, aInfoRefVal);
            afrmRefButtonQuery.Font = this.Font;
            afrmRefButtonQuery.ShowDialog();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            ExecuteSearch();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                ExecuteSearch();
            }
        }

        private void ExecuteSearch()
        {
            object[] myRet = CliUtils.CallMethod("GLModule", "GetDataBaseType", new object[] { CliUtils.fLoginDB });
            if (myRet != null && myRet[0].ToString() == "0")
            {
                CliUtils.fLoginDBType = (ClientType)Enum.Parse(typeof(ClientType), myRet[1].ToString(), true);
            }

            if (tbSearch.Text.Contains("#"))
            {
                String[] sConditions = tbSearch.Text.Split(new char[] { '#' }, StringSplitOptions.None);
                if (sConditions.Length != refButton.SearchColumns.Count)
                {
                    MessageBox.Show("Paraments quantity and column number don't match.");
                    return;
                }
                String sWheres = String.Empty;
                ArrayList alWheres = new ArrayList();

                for (int i = 0; i < refButton.SearchColumns.Count; i++)
                {
                    sWheres += String.Format("{0} like @{0}{1} AND ", refButton.SearchColumns[i].ColumnName, i);
                    IDbDataParameter iParam = CliUtils.CreateDataParameter(CliUtils.fLoginDBType);
                    iParam.ParameterName = String.Format("{0}{1}", refButton.SearchColumns[i].ColumnName, i);
                    if (refButton.SearchColumns[i].Type == "%")
                    {
                        iParam.Value = String.Format("{0}%", sConditions[i]);
                    }
                    else if (refButton.SearchColumns[i].Type == "%%")
                    {
                        iParam.Value = String.Format("%{0}%", sConditions[i]);
                    }
                    alWheres.Add(iParam);
                }
                sWheres = sWheres.Remove(sWheres.LastIndexOf(" AND "));
                (refButton.infoTranslate.BindingSource.DataSource as InfoDataSet).SetWhere(sWheres, alWheres);
            }
            else
            {
                if (String.IsNullOrEmpty(tbSearch.Text))
                {
                    MessageBox.Show("Search condition can't be null");
                    return;
                }

                for (int i = 0; i < refButton.SearchColumns.Count; i++)
                {
                    String sWhere = String.Format("{0} like @{0}{1} ", refButton.SearchColumns[i].ColumnName, i);
                    ArrayList alWhere = new ArrayList();
                    IDbDataParameter iParam = CliUtils.CreateDataParameter(CliUtils.fLoginDBType);
                    iParam.ParameterName = String.Format("{0}{1}", refButton.SearchColumns[i].ColumnName, i);
                    if (refButton.SearchColumns[i].Type == "%")
                    {
                        iParam.Value = String.Format("{0}%", tbSearch.Text);
                    }
                    else if (refButton.SearchColumns[i].Type == "%%")
                    {
                        iParam.Value = String.Format("%{0}%", tbSearch.Text);
                    }
                    alWhere.Add(iParam);
                    (refButton.infoTranslate.BindingSource.DataSource as InfoDataSet).SetWhere(sWhere, alWhere);

                    if (this.infoDataGridView1.Rows.Count > 1)
                    {
                        return;
                    }
                }
            }
        }
    }

    public class AfterOKEventArgs : EventArgs
    {
        ArrayList _values = null;
        ArrayList _displays = null;

        public ArrayList Values
        {
            get { return _values; }
            set { _values = value; }
        }

        public ArrayList Displays
        {
            get { return _displays; }
            set { _displays = value; }
        }
    }
}