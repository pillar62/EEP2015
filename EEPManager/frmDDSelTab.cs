using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using Srvtools;
using System.Linq;

namespace EEPManager
{
    public partial class frmDDSelTab : Form
    {
        public List<string> SelTabList = new List<string>();
        public List<DictionaryEntry> AddedTabList = new List<DictionaryEntry>();

        public frmDDSelTab(List<string> SelectedTabList)
        {
            InitializeComponent();
            SelTabList = SelectedTabList;
        }

        private void frmDDSelTab_Load(object sender, EventArgs e)
        {
            LoadItem("");
        }

        private void LoadItem(String where)
        {
            this.clstTableName.DisplayMember = "Key";
            this.clstTableName.ValueMember = "Value";
            this.clstTableName.Items.Clear();
            DataSet dsAllTable = new DataSet();
            object[] obj = CliUtils.CallMethod("GLModule", "GetSysTableByLoginDB", null);
            if (obj[0].ToString() == "0")
            {
                dsAllTable = obj[1] as DataSet;
            }
            foreach (DataRow row in dsAllTable.Tables[0].Rows)
            {
                if (row[0].ToString().Contains(where))
                {
                    if (dsAllTable.Tables[0].Columns.Contains("owner"))
                    {
                        if (dsAllTable.Tables[0].Columns.Contains("name"))
                        {
                            if (CliUtils.fLoginDBType == ClientType.ctODBC)
                            {
                                this.clstTableName.Items.Add(new DictionaryEntry(row["owner"].ToString() + "." + row["name"].ToString(), row["owner"].ToString() + "." + row["name"].ToString()));
                            }
                            this.clstTableName.Items.Add(new DictionaryEntry(row["name"].ToString(), row["owner"].ToString() + "." + row["name"].ToString()));
                        }
                        else
                            this.clstTableName.Items.Add(new DictionaryEntry(row[0].ToString(), row["owner"].ToString() + "." + row[0].ToString()));
                    }
                    else
                    {
                        if (dsAllTable.Tables[0].Columns.Count > 1)
                        {
                            this.clstTableName.Items.Add(new DictionaryEntry(row[1].ToString(), row[0].ToString()));
                        }
                        else
                        {
                            this.clstTableName.Items.Add(new DictionaryEntry(row[0].ToString(), row[0].ToString()));
                        }
                    }
                }
            }

            for (int i = 0; i < this.clstTableName.Items.Count; i++)
            {
                var tableName = ((DictionaryEntry)this.clstTableName.Items[i]).Value.ToString();
                if (SelTabList.Contains(tableName))
                {
                    this.clstTableName.SetItemChecked(i, true);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String where = tbQuery.Text == null ? String.Empty : tbQuery.Text;
            where = where.Replace("'", "''");

            String type = "";
            object[] myRet = CliUtils.CallMethod("GLModule", "GetDataBaseType", new object[] { CliUtils.fLoginDB });
            if (myRet != null && myRet[0].ToString() == "0")
                type = myRet[1].ToString();
            switch (type)
            {
                case "1": infoDsDBTables.SetWhere("name like '%" + where + "%'"); break;
                case "2": infoDsDBTables.SetWhere("name like '%" + where + "%'"); break;
                case "3": infoDsDBTables.SetWhere("OBJECT_NAME like '%" + where + "%'"); break;
                case "4": infoDsDBTables.SetWhere("TABNAME like '%" + where + "%'"); break;
                case "5": infoDsDBTables.SetWhere("name like '%" + where + "%'"); break;
                case "6": infoDsDBTables.SetWhere("name like '%" + where + "%'"); break;
            }
            LoadItem(where);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < clstTableName.CheckedItems.Count; i++)
            {
                var item = (DictionaryEntry)clstTableName.CheckedItems[i];
                var tableName = item.Value.ToString();
                if (!SelTabList.Contains(tableName))
                {
                    if (AddedTabList.Where(c => c.Value == tableName).Count() == 0) //防止重复
                    {
                        AddedTabList.Add(item);
                    }
                }
            }
        }
    }
}