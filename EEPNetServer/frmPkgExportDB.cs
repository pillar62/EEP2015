using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using Srvtools;
using System.Data.SqlClient;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.OracleClient;

namespace EEPNetServer
{
    public partial class frmPkgExportDB : Form
    {
        string itemName = "";
        List<string> menuList = new List<string>();
        public frmPkgExportDB(string CategoryName,  List<string> MenuList)
        {
            InitializeComponent();
            itemName = CategoryName;
            menuList = MenuList;
        }

        DataTable table = new DataTable("DB");
        private void frmPkgExportDB_Load(object sender, EventArgs e)
        {
            DataColumn dcDBName = new DataColumn("DBName", typeof(string));
            DataColumn dcDBString = new DataColumn("DBString", typeof(string));
            DataColumn dcDBType = new DataColumn("DBType", typeof(string));
            table.Columns.Add(dcDBName);
            table.Columns.Add(dcDBString);
            table.Columns.Add(dcDBType);
            string strDBName = "";
            string strDBstring = "";
            string strPassword = "";
            string strDBType = "";

            string s = SystemFile.DBFile;
            XmlDocument DBXML = new XmlDocument();
            if (File.Exists(s))
            {
                DBXML.Load(s);
                XmlNode aNode = DBXML.DocumentElement.FirstChild;
                while (aNode != null)
                {
                    if (string.Compare(aNode.Name, "DATABASE", true) == 0)//IgnoreCase
                    {
                        XmlNode bNode = aNode.FirstChild;
                        while (bNode != null)
                        {
                            strDBName = bNode.Name;
                            strDBstring = bNode.Attributes["String"].InnerText;
                            if (bNode.Attributes["Password"] != null)
                            {
                                strPassword = bNode.Attributes["Password"].InnerText;
                                strPassword = GetPwdString(strPassword);
                            }
                            if (strPassword != String.Empty)
                                strDBstring += ";password=" + strPassword + ";";
                            strDBType = bNode.Attributes["Type"].InnerText;
                            DataRow dr = table.NewRow();
                            dr.ItemArray = new object[] { strDBName, strDBstring, strDBType };
                            table.Rows.Add(dr);
                            bNode = bNode.NextSibling;
                        }
                    }
                    aNode = aNode.NextSibling;
                }
            }

            this.cmbDB.DataSource = table;
            this.cmbDB.DisplayMember = "DBName";
            this.cmbDB.ValueMember = "DBString";
        }

        private string GetPwdString(string s)
        {
            string sRet = "";
            for (int i = 0; i < s.Length; i++)
            {
                sRet = sRet + (char)(((int)(s[s.Length - 1 - i])) ^ s.Length);
            }
            return sRet;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string DBName = this.cmbDB.Text;
            string DBString = this.cmbDB.SelectedValue.ToString();
            string DBType = "";
            string itemType = "";
            foreach (DataRow dr in table.Rows)
            {
                if (dr["DBName"].ToString() == DBName)
                {
                    DBType = dr["DBType"].ToString();
                    break;
                }
            }
            IDbConnection connection = AllocateConnection(DBType, DBString);
            InfoCommand cmd = new InfoCommand();
            string strSql = "select * from MENUITEMTYPE";
            cmd.CommandText = strSql;
            cmd.Connection = connection;
            connection.Open();
            IDataReader dreader = cmd.ExecuteReader();
            bool bItemTypeExisted = false;
            while (dreader.Read())
            {
                if (dreader["ITEMNAME"].ToString() == this.itemName)
                {
                    bItemTypeExisted = true;
                    itemType = dreader["ITEMTYPE"].ToString();
                }
            }
            dreader.Close();
            if (!bItemTypeExisted)
            {
                //Solution原本不存在，新建一个Solution
                strSql = "insert into MENUITEMTYPE (ITEMTYPE, ITEMNAME) values ('" + this.itemName + "', '" + this.itemName + "')";
                cmd.CommandText = strSql;
                cmd.Connection = connection;
                cmd.ExecuteNonQuery();
                itemType = this.itemName;
            }
            foreach (string menu in this.menuList)
            {
                strSql = "select count(*) from MENUTABLE where PACKAGE = '" + menu + "' and MODULETYPE = 'S' and ITEMTYPE = '" + itemType + "'";
                cmd.CommandText = strSql;
                cmd.Connection = connection;
                int count = (int)cmd.ExecuteScalar();
                if (count == 0)
                {
                    strSql = "insert into MENUTABLE (MENUID, CAPTION, PACKAGE, MODULETYPE, ITEMTYPE) " +
                        "values ('" + menu + "id', '" + menu + "', '" + menu + "', 'S', '" + itemType + "')";
                    cmd.CommandText = strSql;
                    cmd.Connection = connection;
                    cmd.ExecuteNonQuery();
                }
            }
            this.Close();
        }

        private IDbConnection AllocateConnection(string ConnectionType, string ConnectionString)
        {
            IDbConnection con = null;
            switch (ConnectionType)
            {
                case "1":
                    con = new SqlConnection(ConnectionString);
                    break;
                case "2":
                    con = new OleDbConnection(ConnectionString);
                    break;
                case "3":
                    con = new OracleConnection(ConnectionString);
                    break;
                case "4":
                    con = new OdbcConnection(ConnectionString);
                    break;
            }
            return con;
        }
    }
}