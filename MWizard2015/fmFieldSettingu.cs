using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Srvtools;
using System.Data.Common;

namespace MWizard2015
{
    public enum TWizardType { wtWinForm, wtWebPage, wtServer }

    public partial class fmFieldSetting : Form
    {
        private DbConnection FConnection;
        private DataSet dsSYS_REFVAL = new DataSet();
        private DataSet dsSYS_REFVAL_D1 = new DataSet();
        private DataSet dsSYS_REFVAL_D2 = new DataSet();
        private DataSet dsSYS_REFVAL_D3 = new DataSet();
        private InfoCommand FInfoCommand;
        private ClientType FDatabaseType;
        private ListView FListView;
        private TBlockFieldItem FSelectedBlockFieldItem;
        private ListViewItem FSelectedListViewItem;
        private Boolean FDisplayValue = false;
        private TWizardType FWizardType;
        private String DBAlias;
        private ListViewColumnSorter lvwColumnSorter;

        public fmFieldSetting(DbConnection aConnection, ClientType DatabaseType, ListView aListView, TWizardType aWizardType, String EEPAlias)
        {
            InitializeComponent();
            FConnection = aConnection;
            FInfoCommand = new InfoCommand(DatabaseType);
            FDatabaseType = DatabaseType;
            FListView = aListView;
            FSelectedBlockFieldItem = null;
            FSelectedListViewItem = null;
            FDisplayValue = false;
            FWizardType = aWizardType;
            DBAlias = EEPAlias;
            InitData();

            lvwColumnSorter = new ListViewColumnSorter();
            this.lvFields.ListViewItemSorter = lvwColumnSorter;
        }

        public Boolean ShowRefValForm(String[] Params)
        {
            FDisplayValue = true;
            //tbCaption.Text = Params[0];
            //cbCheckNull.Text = Params[1];
            //if (cbCheckNull.Text == "")
            //    cbCheckNull.Text = "N";
            //tbDefaultValue.Text = Params[2];
            //cbControlType.Text = Params[3];
            //if (cbControlType.Text == "")
            //    cbControlType.Text = "TextBox";
            //cbRefValNo.Text = Params[4];
            //cbTableName.Text = Params[5];
            //cbDataTextField.Text = Params[6];
            //cbDataValueField.Text = Params[7];
            FDisplayValue = false;

            DialogResult DR = this.ShowDialog();
            if (DR == DialogResult.OK)
            {
                //Params[0] = tbCaption.Text;
                //Params[1] = cbCheckNull.Text;
                //Params[2] = tbDefaultValue.Text;
                //Params[3] = cbControlType.Text;
                //Params[4] = cbRefValNo.Text;
                //Params[5] = cbTableName.Text;
                //Params[6] = cbDataTextField.Text;
                //Params[7] = cbDataValueField.Text;
                return true;
            }
            else
                return false;
        }

        private void InitData()
        {
            FInfoCommand.Connection = FConnection;

            if (FWizardType == TWizardType.wtWinForm)
            {
                cbControlType.Items.Clear();
                cbControlType.Items.Add("TextBox");
                cbControlType.Items.Add("ComboBox");
                cbControlType.Items.Add("RefValBox");
                cbControlType.Items.Add("DateTimeBox");
                cbControlType.Items.Add("CheckBox");
            }

            lvFields.Items.Clear();
            foreach (ListViewItem ViewItem in FListView.Items)
            {
                TBlockFieldItem aFieldItem = (TBlockFieldItem)ViewItem.Tag;
                ListViewItem NewItem = new ListViewItem();
                NewItem.Text = aFieldItem.DataField;
                NewItem.SubItems.Add(aFieldItem.Description);
                NewItem.SubItems.Add(aFieldItem.CheckNull);
                NewItem.SubItems.Add(aFieldItem.DefaultValue);
                NewItem.SubItems.Add(aFieldItem.RefValNo);
                NewItem.SubItems.Add(aFieldItem.QueryMode);
                NewItem.SubItems.Add(aFieldItem.EditMask);
                NewItem.Tag = aFieldItem;
                lvFields.Items.Add(NewItem);
            }

            if (FDatabaseType != ClientType.ctInformix)
            {
                String[] Params = null;
                String ViewFieldName = "TABLE_NAME";
                if (FDatabaseType == ClientType.ctOracle)
                {
                    String UserID = WzdUtils.GetFieldParam(FConnection.ConnectionString.ToLower(), "user id");
                    Params = new String[] { UserID.ToUpper() };
                    ViewFieldName = "VIEW_NAME";
                }
                DataTable T = FConnection.GetSchema("Tables", Params);
                SortedList<String, String> sTable = new SortedList<String, String>();
                foreach (DataRow DR in T.Rows)
                {
                    sTable.Add(DR["TABLE_NAME"].ToString(),DR["TABLE_NAME"].ToString());
                }

                DataTable D1 = FConnection.GetSchema("Views", Params);
                foreach (DataRow DR in D1.Rows)
                {
                    if (!sTable.ContainsKey(DR[ViewFieldName].ToString()))
                        sTable.Add(DR[ViewFieldName].ToString(), DR[ViewFieldName].ToString());
                }

                foreach (var item in sTable)
                    cbTableName.Items.Add(item.Key);
            }
            else
            {
                List<String> allTables = WzdUtils.GetAllTablesList(FConnection, ClientType.ctInformix);
                allTables.Sort();
                foreach (String str in allTables)
                    cbTableName.Items.Add(str);
            }
            cbTableName.Items.Add("");
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            /* 糤Common砞﹚┮ぃ浪琩
            if (lbTableName.Text == "")
                throw new Exception("Please select a table name !!");
            if (lvFieldName.SelectedItems == null)
                throw new Exception("Please select a field name !!");
             */ 
            DialogResult = DialogResult.OK;
        }

        private void cbControlType_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cbControlType.Text == "ComboBox")
            {
                cbTableName.Enabled = true;
                cbDataTextField.Enabled = true;
                cbDataValueField.Enabled = true;
                cbRefValNo.Text = "";
                cbRefValNo.Enabled = false;
                btnRefVal.Enabled = false;
            }
            else if (cbControlType.Text == "RefValBox")
            {
                if (cbRefValNo.Items.Count == 0)
                {
                    FInfoCommand.Connection = WzdUtils.AllocateConnection(DBAlias, FDatabaseType, true);
                    FInfoCommand.CommandText = "Select REFVAL_NO from SYS_REFVAL";
                    IDbDataAdapter DA = DBUtils.CreateDbDataAdapter(FInfoCommand);
                    DataSet aDataSet = new DataSet();
                    WzdUtils.FillDataAdapter(FDatabaseType, DA, aDataSet, "SYS_REFVAL");
                    DataTable aDataTable = aDataSet.Tables[0];
                    foreach (DataRow DR in aDataTable.Rows)
                    {
                        cbRefValNo.Items.Add(DR["REFVAL_NO"].ToString());
                    }

                }
                cbTableName.Text = "";
                cbDataTextField.Text = "";
                cbDataValueField.Text = "";
                cbTableName.Enabled = false;
                cbDataTextField.Enabled = false;
                cbDataValueField.Enabled = false;
                cbRefValNo.Enabled = true;
                btnRefVal.Enabled = true;
            }
            else
            {
                cbTableName.Text = "";
                cbDataTextField.Text = "";
                cbDataValueField.Text = "";
                cbTableName.Enabled = false;
                cbDataTextField.Enabled = false;
                cbDataValueField.Enabled = false;
                cbRefValNo.Text = "";
                cbRefValNo.Enabled = false;
                btnRefVal.Enabled = false;
            }
        }

        private void cbTableName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbTableName.Text == "")
                return;
            cbDataTextField.Items.Clear();
            cbDataValueField.Items.Clear();
            InfoCommand aInfoCommand = new InfoCommand(FDatabaseType);
            aInfoCommand.Connection = FConnection;
            aInfoCommand.CommandText = String.Format("Select * from {0} where 1=0", cbTableName.Text);
            IDbDataAdapter DA = DBUtils.CreateDbDataAdapter(aInfoCommand);
            DataSet aDataSet = new DataSet();
            try
            {
                WzdUtils.FillDataAdapter(FDatabaseType, DA, aDataSet, cbTableName.Text);

                foreach (DataColumn DC in aDataSet.Tables[0].Columns)
                {
                    cbDataTextField.Items.Add(DC.ColumnName);
                    cbDataValueField.Items.Add(DC.ColumnName);
                }
                cbDataTextField.Items.Add("");
                cbDataValueField.Items.Add("");
            }
            catch
            {
                MessageBox.Show(cbTableName.Text + " is a illegal table.");
                cbTableName.Text = String.Empty;
            }
        }

        private void SetValue()
        {
            if (FSelectedBlockFieldItem == null)
                return;
            FSelectedBlockFieldItem.Description = tbCaption.Text;
            FSelectedBlockFieldItem.CheckNull = cbCheckNull.Text;
            FSelectedBlockFieldItem.DefaultValue = tbDefaultValue.Text;
            FSelectedBlockFieldItem.ControlType = cbControlType.Text;
            FSelectedBlockFieldItem.RefValNo = cbRefValNo.Text;
            FSelectedBlockFieldItem.ComboEntityName = cbTableName.Text;
            FSelectedBlockFieldItem.ComboTextField = cbDataTextField.Text;
            FSelectedBlockFieldItem.ComboValueField = cbDataValueField.Text;
            FSelectedBlockFieldItem.QueryMode = cbQueryMode.Text;
            FSelectedBlockFieldItem.EditMask = tbEditMask.Text;

            FSelectedListViewItem.SubItems[1].Text = FSelectedBlockFieldItem.Description;
            FSelectedListViewItem.SubItems[2].Text = FSelectedBlockFieldItem.CheckNull;
            FSelectedListViewItem.SubItems[3].Text = FSelectedBlockFieldItem.DefaultValue;
            FSelectedListViewItem.SubItems[4].Text = FSelectedBlockFieldItem.RefValNo;
            FSelectedListViewItem.SubItems[5].Text = FSelectedBlockFieldItem.QueryMode;
            FSelectedListViewItem.SubItems[6].Text = FSelectedBlockFieldItem.EditMask;

        }

        private void DisplayValue()
        {
            if (FSelectedBlockFieldItem == null)
                return;
            cbControlType.Text = "TextBox";
            tbCaption.Text = FSelectedBlockFieldItem.Description;
            cbCheckNull.Text = FSelectedBlockFieldItem.CheckNull;
            tbDefaultValue.Text = FSelectedBlockFieldItem.DefaultValue;
            cbControlType.Text = FSelectedBlockFieldItem.ControlType;
            cbTableName.Text = FSelectedBlockFieldItem.ComboEntityName;
            cbDataTextField.Text = FSelectedBlockFieldItem.ComboTextField;
            cbDataValueField.Text = FSelectedBlockFieldItem.ComboValueField;
            cbRefValNo.Text = FSelectedBlockFieldItem.RefValNo;
            cbQueryMode.Text = FSelectedBlockFieldItem.QueryMode;
            tbEditMask.Text = FSelectedBlockFieldItem.EditMask;
            //if (cbCheckNull.Text == "" || cbCheckNull.Text == null)
            //    cbCheckNull.Text = "N";
            if (cbControlType.Text == "" || cbControlType.Text == null)
                cbControlType.Text = "TextBox";
            if (cbQueryMode.Text == null || cbQueryMode.Text == "")
                cbQueryMode.Text = "None";
        }

        private void lvFields_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvFields.SelectedItems.Count == 1)
            {
                ListViewItem aViewItem = lvFields.SelectedItems[0];
                FSelectedListViewItem = aViewItem;
                FSelectedBlockFieldItem = (TBlockFieldItem)aViewItem.Tag;
                FDisplayValue = true;
                DisplayValue();
                FDisplayValue = false;
            }
        }

        private void tbCaption_TextChanged(object sender, EventArgs e)
        {
            if (!FDisplayValue)
                SetValue();
        }

        private void btnRefVal_Click(object sender, EventArgs e)
        {
            fmRefVal aForm = new fmRefVal(FConnection, FDatabaseType, DBAlias);
            String RefValNo = aForm.ShowRefValForm();
            cbRefValNo.Text = RefValNo;
        }

        private void lvFields_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // 检查点击的列是不是现在的排序列.
            if (e.Column == lvwColumnSorter.SortColumn)
            {
                // 重新设置此列的排序方法.
                if (lvwColumnSorter.OrderOfSort == System.Windows.Forms.SortOrder.Ascending)
                {
                    lvwColumnSorter.OrderOfSort = System.Windows.Forms.SortOrder.Descending;
                }
                else
                {
                    lvwColumnSorter.OrderOfSort = System.Windows.Forms.SortOrder.Ascending;
                }
            }
            else
            {
                // 设置排序列，默认为正向排序
                lvwColumnSorter.SortColumn = e.Column;
                lvwColumnSorter.OrderOfSort = System.Windows.Forms.SortOrder.Ascending;
            }

            // 用新的排序方法对ListView排序
            (sender as ListView).Sort();
        }
    }
}