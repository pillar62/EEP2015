using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Data.SqlClient;
using System.Data.Common;
using Srvtools;

namespace MWizard2015
{
    public partial class fmSelTableField : Form
    {
        private string FDatabaseName, FTableName, FDataSetName;
        private TDatasetItem FDatasetItem;
        private TDetailItem FDetailItem = null;
        private ListView FDestListView;
        private TServerData FServerData;
        private MWizard2015.fmServerWzd.GetFieldNamesExFunc FGetFieldNamesExFunc = null;
        private MWizard2015.fmClientWzard.GetFieldNamesFunc FGetFieldNamesFunc = null;
        private MWizard2015.fmClientWzard.RearrangeRefValButtonFunc FRearrangeRefValButtonFunc = null;
        private EventHandler FRefValClickEvent = null;
        private DbConnection FConnection;
        private bool FClientField = false;
        private ClientType FDatabaseType;
        private TBlockItem FBlockItem = null;
        private ListViewColumnSorter lvwFieldSorter;
        private ListViewColumnSorter lvwTableSorter;

        public fmSelTableField()
        {
            InitializeComponent();
            lvwFieldSorter = new ListViewColumnSorter();
            lvwTableSorter = new ListViewColumnSorter();
            this.lvFields.ListViewItemSorter = lvwFieldSorter;
            this.lvTable.ListViewItemSorter = lvwTableSorter;
        }

        public bool ShowSelTableFieldForm(TServerData ServerData, string DatabaseName, ref string TableName, DbConnection Conn)
        {
            FDatabaseName = DatabaseName;
            FTableName = TableName;
            FServerData = ServerData;
            FConnection = Conn;
            FDatabaseType = FServerData.DatabaseType;
            Init();
            DialogResult R = ShowDialog();
            if (R == DialogResult.OK)
                TableName = FTableName;
            return R == DialogResult.OK;
        }

        public bool ShowSelTableFieldForm(TDatasetItem DatasetItem, MWizard2015.fmServerWzd.GetFieldNamesExFunc GetFieldNameEx, ListView DestListView, DbConnection Conn, ClientType DatabaseType)
        {
            FDatasetItem = DatasetItem;
            FDatasetItem.AddAll = false;
            FDatabaseName = FDatasetItem.DatabaseName;
            FTableName = FDatasetItem.TableName;
            FDestListView = DestListView;
            FGetFieldNamesExFunc = GetFieldNameEx;
            FConnection = Conn;
            FDatabaseType = DatabaseType;
            Init();
            return ShowDialog() == DialogResult.OK;
        }

        public bool ShowSelTableFieldForm(TDetailItem DetailItem, MWizard2015.fmClientWzard.GetFieldNamesFunc GetFieldName, ListView DestListView, DbConnection Conn, MWizard2015.fmClientWzard.RearrangeRefValButtonFunc RearrangeRefValButton, EventHandler RefValButtonEvent, ClientType DatabaseType)
        {
            FDetailItem = DetailItem;
            FTableName = FDetailItem.RealTableName;
            FDataSetName = FDetailItem.TableName;
            FDestListView = DestListView;
            FGetFieldNamesFunc = GetFieldName;
            FRearrangeRefValButtonFunc = RearrangeRefValButton;
            FRefValClickEvent = RefValButtonEvent;
            FConnection = Conn;
            FDatabaseType = DatabaseType;
            Init();
            FClientField = true;
            return ShowDialog() == DialogResult.OK;
        }

        public Boolean ShowSelTableFieldForm(TBlockItem BlockItem, 
            MWizard2015.fmClientWzard.GetFieldNamesFunc GetFieldName, ListView DestListView, DbConnection Conn, 
            MWizard2015.fmClientWzard.RearrangeRefValButtonFunc RearrangeRefValButton, EventHandler RefValButtonEvent, 
            ClientType DatabaseType)
        {
            FBlockItem = BlockItem;
            FTableName = BlockItem.TableName;
            if (BlockItem.ProviderName == null)
            {
                MessageBox.Show("Please select a binding source first.");
                return false;
            }
            else if (BlockItem.ProviderName == String.Empty)
            {
                MessageBox.Show("Please select a DataMember first.");
                return false;
            }
            FDataSetName = BlockItem.ProviderName;
            if (FDataSetName.IndexOf('.') > -1)
                WzdUtils.GetToken(ref FDataSetName, new char[] { '.' });
            FDestListView = DestListView;
            FGetFieldNamesFunc = GetFieldName;
            FRearrangeRefValButtonFunc = RearrangeRefValButton;
            FRefValClickEvent = RefValButtonEvent;
            FConnection = Conn;
            FDatabaseType = DatabaseType;
            FClientField = true;
            if (Init())
                return ShowDialog() == DialogResult.OK;
            else
                return false;
        }

        private bool Init()
        {
            int I;

            /*
            for (I = 0; I < tabControl.TabCount; I++)
            {
                tabControl.TabPages[I].Hide();
            }
            */

            tabControl.TabPages.Clear();

            if (FDestListView == null)
            {
                tabControl.TabPages.Add(tpSelectTable);
                tabControl.SelectedTab = tpSelectTable;
                I = (panel1.ClientSize.Width - btnOK.Width - btnCancel.Width) / 3;
                btnOK.Left = I;
                btnCancel.Left = btnOK.Bounds.Width + I + 50;
                GetTableNames(lvTable);
                if (lvTable.Items.Count > 0 && lvTable.SelectedItems.Count == 0)
                    lvTable.Items[0].Selected = true;
            }
            else
            {
                tabControl.TabPages.Add(tpSelectFields);
                tabControl.SelectedTab = tpSelectFields;
                btnSelectAll.Visible = true;
                if (FTableName == String.Empty)
                {
                    MessageBox.Show("DataMember is null. Please select it first.");
                    return false;
                }
                GetFieldNames(FDatabaseName, FTableName, FDataSetName, lvFields, FDestListView);
            }
            return true;
        }

        private void AddDestFieldItem(ListViewItem lvi)
        {
            TFieldAttrItem Item = new TFieldAttrItem();
            FDatasetItem.FieldAttrItems.Add(Item);
            Item.DataField = lvi.Text;
            if (lvi.SubItems.Count > 1)
                Item.Description = lvi.SubItems[1].Text;
            lvi.Tag = Item;
        }

        private void AddDestBlockFieldItem(ListViewItem ViewItem, TBlockFieldItem SourceItem)
        {
            TBlockFieldItem BlockFieldItem = new TBlockFieldItem();
            BlockFieldItem.DataField = SourceItem.DataField;
            BlockFieldItem.Description = SourceItem.Description;
            BlockFieldItem.DataType = SourceItem.DataType;
            BlockFieldItem.IsKey = SourceItem.IsKey;
            BlockFieldItem.CheckNull = SourceItem.CheckNull;
            BlockFieldItem.DefaultValue = SourceItem.DefaultValue;
            BlockFieldItem.Length = SourceItem.Length;
            BlockFieldItem.ControlType = SourceItem.ControlType;
            BlockFieldItem.QueryMode = SourceItem.QueryMode;
            BlockFieldItem.EditMask = SourceItem.EditMask;
            /*
            InfoCommand command1 = new InfoCommand(FDatabaseType);
            command1.Connection = FConnection;
            string[] textArray1 = new string[] { "Select TABLE_NAME, FIELD_NAME, IS_KEY, FIELD_LENGTH, CHECK_NULL, DEFAULT_VALUE from COLDEF where TABLE_NAME = '", FTableName, "' and FIELD_NAME = '", BlockFieldItem.DataField, "'" };
            command1.CommandText = string.Concat(textArray1);
            IDbDataAdapter adapter1 = WzdUtils.AllocateDataAdapter(FDatabaseType);
            adapter1.SelectCommand = command1.GetInternalCommand();
            DataSet set1 = new DataSet();
            WzdUtils.FillDataAdapter(FDatabaseType, adapter1, set1, "COLDEF");
            DataRow[] rowArray1 = set1.Tables[0].Select("FIELD_NAME = '" + BlockFieldItem.DataField + "'");
            if (rowArray1.Length == 1)
            {
                BlockFieldItem.IsKey = rowArray1[0].ItemArray[2].ToString().ToUpper() == "Y";
                if (rowArray1[0].ItemArray[3] != null)
                    BlockFieldItem.Length = int.Parse(rowArray1[0].ItemArray[3].ToString());
                BlockFieldItem.CheckNull = rowArray1[0].ItemArray[4].ToString().ToUpper();
                BlockFieldItem.DefaultValue = rowArray1[0].ItemArray[5].ToString();
            }
             */

            if (FDestListView.Columns.Count == 3)
            {
                ListViewItem.ListViewSubItem LVSI = ViewItem.SubItems.Add("");
                Button B = new Button();
                B.Parent = FDestListView;
                if (FRearrangeRefValButtonFunc != null)
                    FRearrangeRefValButtonFunc(B, LVSI.Bounds);
                B.BackColor = Color.Silver;
                B.BringToFront();
                LVSI.Tag = B;
                B.Tag = ViewItem;
                ViewItem.Tag = BlockFieldItem;
                B.Click += new EventHandler(FRefValClickEvent);
                B.Text = "...";
            }

            if (FDetailItem != null)
            FDetailItem.BlockFieldItems.Add(BlockFieldItem);
            if (FBlockItem != null)
                FBlockItem.BlockFieldItems.Add(BlockFieldItem);
        }

        private bool DoOK()
        {
            bool Result = false;
            ListViewItem lvi;
            if (FDestListView == null)
            {
                lvi = lvTable.SelectedItems[0];
                FTableName = lvi.Text;
                Result = true;
            }
            else
            {
                int I;
                for (I = 0; I < lvFields.Items.Count; I++)
                {
                    if (lvFields.Items[I].Selected)
                    {
                        Result = true;
                        lvi = new ListViewItem();
                        FDestListView.Items.Add(lvi);
                        lvi.Text = lvFields.Items[I].Text;
                        lvi.SubItems.Add(lvFields.Items[I].SubItems[1]);
                        lvi.Tag = lvFields.Items[I].Tag;
                        if (FClientField)
                            AddDestBlockFieldItem(lvi, (TBlockFieldItem)lvFields.Items[I].Tag);
                        else
                            AddDestFieldItem(lvi);
                    }
                }
                if (FDestListView.Items.Count > 0)
                    FDestListView.Items[0].Selected = true;
            }
            return Result;
        }

        private void GetFieldNames(string DatabaseName, string TableName, String DataSetName, ListView SrcListView, ListView DestListView)
        {
            if (FGetFieldNamesExFunc != null)
                FGetFieldNamesExFunc(FDatasetItem, DatabaseName, TableName, DataSetName, SrcListView, DestListView);

            if (FGetFieldNamesFunc != null)
                FGetFieldNamesFunc(DatabaseName, TableName, DataSetName, SrcListView, DestListView);
        }

        private void GetTableCaptionFromCOLDEF(TStringList aTableNameCaptionList)
        {
            int I;
            DataRow DR;
            InfoCommand aInfoCommand = new InfoCommand(FDatabaseType);
            aInfoCommand.Connection = FConnection;
            aInfoCommand.CommandText = "Select TABLE_NAME, CAPTION from COLDEF where FIELD_NAME = '*' or FIELD_NAME is null order by TABLE_NAME";
            IDbDataAdapter DA = DBUtils.CreateDbDataAdapter(aInfoCommand);
            DataSet D = new DataSet();
            WzdUtils.FillDataAdapter(FDatabaseType, DA, D, "COLDEF");
            aTableNameCaptionList.Clear();
            for (I = 0; I < D.Tables[0].Rows.Count; I++)
            {
                DR = D.Tables[0].Rows[I];
                if (DR["TABLE_NAME"].ToString().Trim() != "" && DR["CAPTION"].ToString().Trim() != "")
                    aTableNameCaptionList.Add(DR["TABLE_NAME"].ToString().Trim() + "=" + DR["CAPTION"].ToString().Trim());
            }
        }

        private void GetTableNames(ListView LV)
        {
            int I;
            ListViewItem lvi;
            TStringList aTableCaptionList = new TStringList();
            TStringList aTableList = new TStringList();
            String[] Params = null;
            String ViewFieldName = "TABLE_NAME";
            if (FServerData.DatabaseType == ClientType.ctOracle)
            {
                String UserID = WzdUtils.GetFieldParam(FServerData.ConnectionString.ToLower(), "user id");
                Params = new String[] { UserID };
                ViewFieldName = "VIEW_NAME";
            }

            if (FServerData != null)
            {
                aTableCaptionList = FServerData.TableNameCaptionList;
                aTableList = FServerData.TableNameList;
            }
            else
            {
                if (FConnection.GetType().FullName != "IBM.Data.Informix.IfxConnection")
                {
                    GetTableCaptionFromCOLDEF(aTableCaptionList);
                    DataTable D = FConnection.GetSchema("Tables", Params);
                    D.Select("", "TABLE_NAME DESC");
                    for (I = 0; I < D.Rows.Count; I++)
                        aTableList.Add(D.Rows[I]["TABLE_NAME"].ToString());

                    DataTable T = FConnection.GetSchema("Views", Params);
                    DataRow[] dr = T.Select("", "VIEW_NAME ASC");
                    foreach (DataRow DR in dr)
                    {
                        aTableList.Add(DR["VIEW_NAME"].ToString());
                    }
                }
                else
                {
                    List<String> allTables = WzdUtils.GetAllTablesList(FConnection, ClientType.ctInformix);
                    allTables.Sort();
                    foreach (String str in allTables)
                        aTableList.Add(str);
                }
            }

            //if (FConnection.GetType().FullName != "IBM.Data.Informix.IfxConnection")
            //{
            //    DataTable D1 = FConnection.GetSchema("Views", Params);
            //    D1.Select("", ViewFieldName + " DESC");
            //    foreach (DataRow DR in D1.Rows)
            //    {
            //        if (aTableList.IndexOf(DR[ViewFieldName].ToString()) < 0)
            //            aTableList.Add(DR[ViewFieldName].ToString());
            //    }
            //}

            LV.Items.Clear();

            if (aTableList.Count > 0)
            {
                LV.BeginUpdate();
                for (I = 0; I < aTableList.Count; I++)
                {
                    lvi = new ListViewItem();
                    lvi.Text = aTableList[I].ToString();
                    //lvi.SubItems[""] = "";
                    LV.Items.Add(lvi);
                    String tableName = lvi.Text;
                    if (tableName.Contains("."))
                        tableName = tableName.Split('.')[1];
                    lvi.SubItems.Add(aTableCaptionList.Values(tableName));
                    lvi.Selected = lvi.Text.CompareTo(FTableName) == 0;
                }
                LV.EndUpdate();
            }

            LV.Sort();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Test1(Test2);
        }

        private string Test1(FUNC f)
        {
            MessageBox.Show(f());
            return "";
        }

        public delegate string FUNC();

        public string Test2()
        {
            return "";
        }

        private void btnOK_Click_1(object sender, EventArgs e)
        {
            if (DoOK())
            {
                DialogResult = DialogResult.OK;
            }
            else
            {
                DialogResult = DialogResult.Cancel;
            }
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            if (FDatasetItem != null)
                FDatasetItem.AddAll = true;
            if (FDestListView != null)
            {
                int I;
                for (I = 0; I < lvFields.Items.Count; I++)
                    lvFields.Items[I].Selected = true;
            }
            if (DoOK())
            {
                DialogResult = DialogResult.OK;
            }
            else
            {
                DialogResult = DialogResult.Cancel;
            }
        }

        private void cbShowView_CheckedChanged(object sender, EventArgs e)
        {
            GetTableNames(lvTable);
        }

        private void lvFields_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // 检查点击的列是不是现在的排序列.
            if (e.Column == lvwFieldSorter.SortColumn)
            {
                // 重新设置此列的排序方法.
                if (lvwFieldSorter.OrderOfSort == System.Windows.Forms.SortOrder.Ascending)
                {
                    lvwFieldSorter.OrderOfSort = System.Windows.Forms.SortOrder.Descending;
                }
                else
                {
                    lvwFieldSorter.OrderOfSort = System.Windows.Forms.SortOrder.Ascending;
                }
            }
            else
            {
                // 设置排序列，默认为正向排序
                lvwFieldSorter.SortColumn = e.Column;
                lvwFieldSorter.OrderOfSort = System.Windows.Forms.SortOrder.Ascending;
            }

            // 用新的排序方法对ListView排序
            (sender as ListView).Sort();
        }

        private void lvTable_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // 检查点击的列是不是现在的排序列.
            if (e.Column == lvwTableSorter.SortColumn)
            {
                // 重新设置此列的排序方法.
                if (lvwTableSorter.OrderOfSort == System.Windows.Forms.SortOrder.Ascending)
                {
                    lvwTableSorter.OrderOfSort = System.Windows.Forms.SortOrder.Descending;
                }
                else
                {
                    lvwTableSorter.OrderOfSort = System.Windows.Forms.SortOrder.Ascending;
                }
            }
            else
            {
                // 设置排序列，默认为正向排序
                lvwTableSorter.SortColumn = e.Column;
                lvwTableSorter.OrderOfSort = System.Windows.Forms.SortOrder.Ascending;
            }

            // 用新的排序方法对ListView排序
            (sender as ListView).Sort();
        }

        private void lvTable_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}