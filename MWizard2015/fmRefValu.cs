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
    public partial class fmRefVal : Form
    {
        private DbConnection FConnection;
        private DataSet dsSYS_REFVAL = new DataSet();
        private DataSet dsSYS_REFVAL_D1 = new DataSet();
        private DataSet dsSYS_REFVAL_D2 = new DataSet();
        private DataSet dsSYS_REFVAL_D3 = new DataSet();
        private InfoCommand FInfoCommand;
        private ClientType FDatabaseType;
        private String DBAlias;
        private ListViewColumnSorter lvwColumnSorter;

        public fmRefVal(DbConnection aConnection, ClientType DatabaseType, String EEPAlias)
        {
            InitializeComponent();
            FConnection = aConnection;
            FInfoCommand = new InfoCommand(DatabaseType);
            FDatabaseType = DatabaseType;
            DBAlias = EEPAlias;
            InitData();

            lvwColumnSorter = new ListViewColumnSorter();
            this.lvFieldName.ListViewItemSorter = lvwColumnSorter;
        }

        public String ShowRefValForm()
        {
            DialogResult DR = this.ShowDialog();
            if (DR == DialogResult.OK)
                return lvFieldName.SelectedItems[0].Text;
            else
                return "";
        }

        private void InitData()
        {
            //FInfoCommand.Connection = FConnection;
            FInfoCommand.Connection = WzdUtils.AllocateConnection(DBAlias, FDatabaseType, true);
            //SYS_REFVAL
            lbTableName.Items.Clear();
            FInfoCommand.CommandText = "Select distinct(TABLE_NAME) from SYS_REFVAL";
            IDbDataAdapter DA = DBUtils.CreateDbDataAdapter(FInfoCommand);
            DataSet aDataSet = new DataSet();
            WzdUtils.FillDataAdapter(FDatabaseType, DA, aDataSet, "SYS_REFVAL");
            DataTable aDataTable = aDataSet.Tables[0];
            foreach (DataRow DR in aDataTable.Rows)
            {
                lbTableName.Items.Add(DR[0].ToString());
            }
        }

        private void lbTableName_SelectedIndexChanged(object sender, EventArgs e)
        {
            String TableName = lbTableName.Text;
            if (TableName == "")
                return;
            //SYS_REFVAL
            dsSYS_REFVAL.Clear();
            lvFieldName.Items.Clear();
            FInfoCommand.CommandText = string.Format("Select * from SYS_REFVAL where TABLE_NAME = '{0}'", TableName);
            IDbDataAdapter DA = DBUtils.CreateDbDataAdapter(FInfoCommand);
            WzdUtils.FillDataAdapter(FDatabaseType, DA, dsSYS_REFVAL, "SYS_REFVAL");
            DataTable aDataTable = dsSYS_REFVAL.Tables[0];
            foreach (DataRow DR in aDataTable.Rows)
            {
                ListViewItem LVI = new ListViewItem();
                LVI.Text = DR["REFVAL_NO"].ToString();
                LVI.SubItems.Add(DR["DESCRIPTION"].ToString());
                LVI.SubItems.Add(DR["VALUE_MEMBER"].ToString());
                LVI.SubItems.Add(DR["DISPLAY_MEMBER"].ToString());
                LVI.SubItems.Add(DR["SELECT_ALIAS"].ToString());
                LVI.SubItems.Add(DR["SELECT_COMMAND"].ToString());
                lvFieldName.Items.Add(LVI);
            }
        }

        private void lvFieldName_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (ListViewItem LVI in lvFieldName.SelectedItems)
            {
                tbSelectCommand.Text = LVI.SubItems[5].Text;
                //SYS_REFVAL_D1
                dsSYS_REFVAL_D1.Clear();
                String REFVAL_NO = LVI.Text;
                FInfoCommand.CommandText = string.Format("Select * from SYS_REFVAL_D1 where REFVAL_NO = '{0}'", REFVAL_NO);
                IDbDataAdapter DA1 = WzdUtils.AllocateDataAdapter(FDatabaseType);
                DA1.SelectCommand = FInfoCommand.GetInternalCommand();
                WzdUtils.FillDataAdapter(FDatabaseType, DA1, dsSYS_REFVAL_D1, "SYS_REFVAL_D1");
                bsSYS_REFVAL_D1.DataSource = dsSYS_REFVAL_D1;
                bsSYS_REFVAL_D1.DataMember = dsSYS_REFVAL_D1.Tables[0].TableName;
                dgvSYS_REFVAL_D1.DataSource = dsSYS_REFVAL_D1;
                dgvSYS_REFVAL_D1.DataMember = dsSYS_REFVAL_D1.Tables[0].TableName;
                dgvSYS_REFVAL_D1.Columns[0].Visible = false;
                /*
                //SYS_REFVAL_D2
                dsSYS_REFVAL_D2.Clear();
                FInfoCommand.CommandText = string.Format("Select * from SYS_REFVAL_D2 where REFVAL_NO = '{0}'", REFVAL_NO);
                IDbDataAdapter DA2 = WzdUtils.AllocateDataAdapter(ClientType.ctMsSql);
                DA2.SelectCommand = FInfoCommand.GetInternalCommand();
                WzdUtils.FillDataAdapter(ClientType.ctMsSql, DA2, dsSYS_REFVAL_D2, "SYS_REFVAL_D2");
                bsSYS_REFVAL_D2.DataSource = dsSYS_REFVAL_D2;
                bsSYS_REFVAL_D2.DataMember = dsSYS_REFVAL_D2.Tables[0].TableName;
                dgvSYS_REFVAL_D2.DataSource = dsSYS_REFVAL_D2;
                dgvSYS_REFVAL_D2.DataMember = dsSYS_REFVAL_D2.Tables[0].TableName;
                dgvSYS_REFVAL_D2.Columns[0].Visible = false;
                //SYS_REFVAL_D3
                dsSYS_REFVAL_D3.Clear();
                FInfoCommand.CommandText = string.Format("Select * from SYS_REFVAL_D3 where REFVAL_NO = '{0}'", REFVAL_NO);
                IDbDataAdapter DA3 = WzdUtils.AllocateDataAdapter(ClientType.ctMsSql);
                DA3.SelectCommand = FInfoCommand.GetInternalCommand();
                WzdUtils.FillDataAdapter(ClientType.ctMsSql, DA3, dsSYS_REFVAL_D3, "SYS_REFVAL_D3");
                bsSYS_REFVAL_D3.DataSource = dsSYS_REFVAL_D3;
                bsSYS_REFVAL_D3.DataMember = dsSYS_REFVAL_D3.Tables[0].TableName;
                dgvSYS_REFVAL_D3.DataSource = dsSYS_REFVAL_D3;
                dgvSYS_REFVAL_D3.DataMember = dsSYS_REFVAL_D3.Tables[0].TableName;
                dgvSYS_REFVAL_D3.Columns[0].Visible = false;
                 */ 
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (lvFieldName.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a refval name first!");
                return;
            }
            DialogResult = DialogResult.OK;
        }

        private void lvFieldName_ColumnClick(object sender, ColumnClickEventArgs e)
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