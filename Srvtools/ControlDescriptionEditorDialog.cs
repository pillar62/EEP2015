using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data.OracleClient;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Xml;
#if MySql
using MySql.Data.MySqlClient;
#endif

namespace Srvtools
{
    public partial class ControlDescriptionEditorDialog : Form
    {
        public ControlDescriptionEditorDialog(object editingcontrol)
        {
            InitializeComponent();
            _EditingControl = editingcontrol;
            InitialParameter();
        }

        private DataSet dataSet;
        private IDataAdapter adapter;
        private IDbConnection connection;

        private int _ID;

        public int ID
        {
            get { return _ID; }
        }

        private string _Alias;

        public string Alias
        {
            get { return _Alias; }
        }

        private object _EditingControl;

        public object EditingControl
        {
            get { return _EditingControl; }
        }

        private void InitialParameter()
        {
            if (EditingControl is ControlDescription)
            {
                ControlDescription control = EditingControl as ControlDescription;
                _ID = control.Identification;
                _Alias = control.DataBase;
            }
            else if (EditingControl is WebControlDescription)
            {
                WebControlDescription control = EditingControl as WebControlDescription;
                _ID = control.Identification;
                _Alias = control.DataBase;
            }
            else
            {
                this.Close();
            }
            this.Text = string.Format("ControlDescription Editor({0})", ID);
            connection = CreateConnection(Alias);
            if (connection == null)
            {
                this.Close();
            }
        }

        private IDbConnection CreateConnection(string alias)
        {
            var dbconnection = DbConnectionSet.GetDbConn(Alias);
            if (dbconnection != null )
            {
                return dbconnection.CreateConnection();
            }
            return null;
        }
        //A6-Sensitive Data Exposure
        //private static string GetPwdString(string s)
        //{
        //    string sRet = "";
        //    for (int i = 0; i < s.Length; i++)
        //    {
        //        sRet = sRet + (char)(((int)(s[s.Length - 1 - i])) ^ s.Length);
        //    }
        //    return sRet;
        //}

        private void buttonReadDB_Click(object sender, EventArgs e)
        {
            if (connection != null)
            {
                try
                {
                    dataSet = new DataSet();
                    IDbCommand command = connection.CreateCommand();
                    command.CommandText = string.Format("SELECT ID, IDENTIFICATION, KEYS, EN as Description FROM SYS_LANGUAGE WHERE IDENTIFICATION = '{0}'", ID);
                    adapter = CreateDbDataAdapter(command);
                    adapter.FillSchema(dataSet, SchemaType.Mapped);
                    adapter.Fill(dataSet);
                    dataGridView.DataSource = dataSet;
                    dataGridView.DataMember = dataSet.Tables[0].TableName;
                    dataGridView.Columns[0].Visible = false;
                    dataGridView.Columns[1].Visible = false;
                    dataSet.Tables[0].TableNewRow += delegate(object table, DataTableNewRowEventArgs dre)
                    {
                        dre.Row["IDENTIFICATION"] = ID;
                    };
                    buttonWriteDB.Enabled = true;
                    buttonRefresh.Enabled = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private IDbDataAdapter CreateDbDataAdapter(IDbCommand cmd)
        {
            IDbDataAdapter adapter = null;
            if (cmd is SqlCommand)
            {
                adapter = new SqlDataAdapter();
            }
            else if (cmd is OracleCommand)
            {
                adapter = new OracleDataAdapter();
            }
            else if (cmd is OdbcCommand)
            {
                adapter = new OdbcDataAdapter();
            }
            else if (cmd is OleDbCommand)
            {
                adapter = new OleDbDataAdapter();
            }
#if MySql
            else if (cmd.GetType().FullName == "MySql.Data.MySqlClient.MySqlCommand")
            {
                adapter = new MySqlDataAdapter();
            }
#endif
            adapter.SelectCommand = cmd;
            return adapter;
        }

        private void buttonWriteDB_Click(object sender, EventArgs e)
        {
            if (adapter is SqlDataAdapter)
            {
                SqlCommandBuilder builder = new SqlCommandBuilder((SqlDataAdapter)adapter);
            }
            else if (adapter is OracleDataAdapter)
            {
                OracleCommandBuilder builder = new OracleCommandBuilder((OracleDataAdapter)adapter);
            }
            else if (adapter is OleDbDataAdapter)
            {
                OleDbCommandBuilder builder = new OleDbCommandBuilder((OleDbDataAdapter)adapter);
            }
            else if (adapter is OdbcDataAdapter)
            {
                OdbcCommandBuilder builder = new OdbcCommandBuilder((OdbcDataAdapter)adapter);
            }
#if MySql
            else if (adapter.GetType().Name == "MySqlDataAdapter")
            {
                MySqlCommandBuilder builder = new MySqlCommandBuilder((MySqlDataAdapter)adapter);
            }
#endif
            try
            {
                adapter.Update(dataSet);
                MessageBox.Show(this, "Write to Database successfully", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            List<string> list = new List<string>();
            try
            {
                if (EditingControl is ControlDescription)
                {
                    ControlDescription control = EditingControl as ControlDescription;
                    list = control.GetControlValues();
                }
                else
                {
                    WebControlDescription control = EditingControl as WebControlDescription;
                    list = control.GetControlValues();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            foreach (string str in list)
            {
                DataRow[] drs = dataSet.Tables[0].Select(string.Format("KEYS = '{0}'", str));
                if (drs.Length == 0)
                {
                    DataRow dr = dataSet.Tables[0].NewRow();
                    dr["KEYS"] = str;
                    dataSet.Tables[0].Rows.Add(dr);
                }
            }
        }
    }
}