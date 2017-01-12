using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data.OracleClient;
using System.Data.OleDb;
using System.Data.Odbc;
using System.Reflection;
#if MySql
using MySql.Data.MySqlClient;
#endif

namespace Srvtools
{
    public partial class InfoTransactionEditorKeyFieldsDialog : Form
    {
        Transaction transaction = null;
        Transaction transPrivateCopy = null;
        UpdateComponent uctran = null;
        string strDesTable = "";
        string strSrcTable = "";
        IDbConnection _conn = null;

        public InfoTransactionEditorKeyFieldsDialog(Transaction trans, UpdateComponent uc, string destablename, string srctablename)
        {
            transaction = trans;
            uctran = uc;
            _conn = FindConnection(uctran);
            strDesTable = destablename;
            strSrcTable = srctablename;
            InitializeComponent();
        }

        private void InfoTransactionEditorKeyFieldsDialog_Load(object sender, EventArgs e)
        {
            // Copy the editing Transaction
            //
            SetTransactionPrivateCopy();

            // SetUpUI
            SetUpUI();
        }

        #region getsourcecolumn
        private IDbConnection FindConnection(UpdateComponent uc)
        {
            IDbConnection idbcon = null;

            if (uc == null)
            {
                return null;
            }
            else if (uc.SelectCmd == null)
            {
                return null;
            }
            else if (uc.SelectCmd.InfoConnection == null)
            {
                return null;
            }
            else
            {
                idbcon = uc.SelectCmd.InfoConnection;
            }
            return idbcon;
        }

        private List<string> AddColumn(string tablename)
        {
            List<string> columnList = new List<string>();
            ClientType type = DBUtils.GetDatabaseType(_conn);
            IDbCommand cmd = _conn.CreateCommand();
            cmd.CommandText = string.Format("SELECT * FROM {0}", DBUtils.QuoteWords(tablename, type));

            IDbDataAdapter adapter = DBUtils.CreateDbDataAdapter(cmd);

            if (_conn.State == ConnectionState.Closed)
            {
                _conn.Open();
            }
            DataSet ds = new DataSet();
            try
            {
                adapter.FillSchema(ds, SchemaType.Mapped);
            }
            finally
            {
                _conn.Close();
            }
            foreach (DataColumn dc in ds.Tables[0].Columns)
            {
                columnList.Add(dc.ColumnName);
            }

            return columnList;
        }
        #endregion

        private void SetUpUI()
        {
            // Set up ComboBox - cbxWhereMode
            cbxWhereMode.Items.Add("WhereOnly");
            cbxWhereMode.Items.Add("InsertOnly");
            cbxWhereMode.Items.Add("Both");

            // Set up ListBox 
            foreach (TransKeyField keyField in transPrivateCopy.TransKeyFields)
            {
                this.lbDesField.Items.Add(keyField.DesField);
                this.lbScrField.Items.Add(keyField.SrcField);
                this.lbRelation.Items.Add(keyField.DesField + " = " + keyField.SrcField);
            }
        }

        private void SetTransactionPrivateCopy()
        {
            transPrivateCopy = new Transaction();

            foreach (TransKeyField keyField in transaction.TransKeyFields)
            {
                TransKeyField tkf = new TransKeyField();
                tkf.DesField = keyField.DesField;
                tkf.FieldType = keyField.FieldType;
                tkf.SrcField = keyField.SrcField;
                tkf.SrcGetValue = keyField.SrcGetValue;
                tkf.SrcValue = keyField.SrcValue;
                tkf.WhereMode = keyField.WhereMode;

                transPrivateCopy.TransKeyFields.Add(tkf);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lbRelation_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = this.lbRelation.SelectedIndex;
            this.lbDesField.SelectedIndex = index;
            this.lbScrField.SelectedIndex = index;
            if (index != -1)
            {
                switch (transPrivateCopy.TransKeyFields[index].WhereMode)
                {
                    case WhereMode.WhereOnly:
                        cbxWhereMode.SelectedIndex = 0;
                        break;
                    case WhereMode.InsertOnly:
                        cbxWhereMode.SelectedIndex = 1;
                        break;
                    case WhereMode.Both:
                        cbxWhereMode.SelectedIndex = 2;
                        break;
                }
                this.txtSrcGetValue.Text =
                    this.transPrivateCopy.TransKeyFields[index].SrcGetValue;
            }
            else
            {
                this.cbxWhereMode.SelectedIndex = -1;
                this.txtSrcGetValue.Clear();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            List<string> lstSrcColumn = new List<string>();
            List<string> lstDesColumn = new List<string>();

            InfoTransactionTransKeyFieldsAddDialog ittkfad = new InfoTransactionTransKeyFieldsAddDialog();

            if (strSrcTable != string.Empty && _conn != null)
            {
                if (uctran != null && uctran.SelectCmd != null)
                {
                    ittkfad.cmbSrcField.Items.AddRange(uctran.SelectCmd.GetFields());
                }
            }

            if (strDesTable != string.Empty && _conn != null)
            {
                try
                {
                    lstDesColumn = AddColumn(strDesTable);
                }
                catch
                {
                    MessageBox.Show(string.Format("Transtable :{0} \ndoesn't exist", strDesTable));
                    return;
                }
                foreach (string strColumn in lstDesColumn)
                {
                    ittkfad.cmbDesField.Items.Add(strColumn);
                }
            }

            ittkfad.cbxWhereMode.Items.Add("WhereOnly");
            ittkfad.cbxWhereMode.Items.Add("InsertOnly");
            ittkfad.cbxWhereMode.Items.Add("Both");
            ittkfad.cbxWhereMode.SelectedIndex = 2;


            if (ittkfad.ShowDialog() == DialogResult.OK)
            {
                TransKeyField tkf = new TransKeyField();
                tkf.DesField = ittkfad.cmbDesField.Text;
                tkf.SrcField = ittkfad.cmbSrcField.Text;
                tkf.SrcGetValue = ittkfad.txtSrcGetValue.Text;
                switch (ittkfad.cbxWhereMode.SelectedIndex)
                {
                    case 0:
                        tkf.WhereMode = WhereMode.WhereOnly;
                        break;

                    case 1:
                        tkf.WhereMode = WhereMode.InsertOnly;
                        break;

                    case 2:
                        tkf.WhereMode = WhereMode.Both;
                        break;
                }
                this.transPrivateCopy.TransKeyFields.Add(tkf);
                this.lbDesField.Items.Add(tkf.DesField);
                this.lbScrField.Items.Add(tkf.SrcField);
                this.lbRelation.Items.Add(tkf.DesField + " = " + tkf.SrcField);
            }
        }

        // ---------------------------------------------------------------------
        // Added by yangdong
        //private string GetTableName(DataTable shema)
        //{
        //    return shema.Rows[0]["BaseTableName"].ToString();
        //}

        //private DataTable _schema;
        //private DataTable GetSchema(IDbConnection conn, string sSql)
        //{
        //    if (_schema != null)
        //    {
        //        return _schema;
        //    }

        //    IDbDataAdapter adapter = AllocateDataAdapter(conn, sSql);
        //    if (conn.State == ConnectionState.Closed)
        //        conn.Open();
        //    IDataReader dr = adapter.SelectCommand.ExecuteReader(CommandBehavior.SchemaOnly | CommandBehavior.KeyInfo);

        //    DataTable schema = dr.GetSchemaTable();
        //    _schema = schema;
        //    if (conn.State == ConnectionState.Open)
        //        conn.Close();

        //    dr.Close();
        //    return schema;
        //}

        //private IDbDataAdapter AllocateDataAdapter(IDbConnection conn, string sSql)
        //{
        //    if (conn is SqlConnection)
        //        return new SqlDataAdapter(sSql, (SqlConnection)conn);
        //    else if (conn is OdbcConnection)
        //        return new OdbcDataAdapter(sSql, (OdbcConnection)conn);
        //    else if (conn is OracleConnection)
        //        return new OracleDataAdapter(sSql, (OracleConnection)conn);
        //    else if (conn is OleDbConnection)
        //        return new OleDbDataAdapter(sSql, (OleDbConnection)conn);
        //    else return null;

        //}
        // ---------------------------------------------------------------------

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int index = this.lbRelation.SelectedIndex;
            if (index != -1)
            {
                this.lbRelation.Items.RemoveAt(index);
                this.lbDesField.Items.RemoveAt(index);
                this.lbScrField.Items.RemoveAt(index);
                this.transPrivateCopy.TransKeyFields.RemoveAt(index);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            int selindex = this.lbRelation.SelectedIndex;
            List<string> lstSrcColumn = new List<string>();
            List<string> lstDesColumn = new List<string>();

            if (selindex != -1)
            {
                InfoTransactionTransKeyFieldsAddDialog ittkfad = new InfoTransactionTransKeyFieldsAddDialog();

                if (strSrcTable != string.Empty && _conn != null)
                {
                    if (uctran != null && uctran.SelectCmd != null)
                    {
                        ittkfad.cmbSrcField.Items.AddRange(uctran.SelectCmd.GetFields());
                    }
                }

                if (strDesTable != string.Empty && _conn != null)
                {
                    try
                    {
                        lstDesColumn = AddColumn(strDesTable);
                    }
                    catch
                    {
                        MessageBox.Show(string.Format("Transtable :{0} \ndoesn't exist", strDesTable));
                        return;
                    }
                    foreach (string strColumn in lstDesColumn)
                    {
                        ittkfad.cmbDesField.Items.Add(strColumn);
                    }
                }

                ittkfad.cbxWhereMode.Items.Add("WhereOnly");
                ittkfad.cbxWhereMode.Items.Add("InsertOnly");
                ittkfad.cbxWhereMode.Items.Add("Both");

                ittkfad.cmbDesField.Text = this.lbDesField.Items[selindex].ToString();
                ittkfad.cmbSrcField.Text = this.lbScrField.Items[selindex].ToString();
                ittkfad.cbxWhereMode.Text = this.cbxWhereMode.Text;
                ittkfad.txtSrcGetValue.Text = this.txtSrcGetValue.Text;

                if (ittkfad.ShowDialog() == DialogResult.OK)
                {
                    TransKeyField tkf = new TransKeyField();
                    tkf.DesField = ittkfad.cmbDesField.Text;
                    tkf.SrcField = ittkfad.cmbSrcField.Text;
                    tkf.SrcGetValue = ittkfad.txtSrcGetValue.Text;
                    switch (ittkfad.cbxWhereMode.SelectedIndex)
                    {
                        case 0:
                            tkf.WhereMode = WhereMode.WhereOnly;
                            break;

                        case 1:
                            tkf.WhereMode = WhereMode.InsertOnly;
                            break;

                        case 2:
                            tkf.WhereMode = WhereMode.Both;
                            break;
                    }
                    this.transPrivateCopy.TransKeyFields[selindex] = tkf;
                    this.lbDesField.Items[selindex] = tkf.DesField;
                    this.lbScrField.Items[selindex] = tkf.SrcField;
                    this.lbRelation.Items[selindex] = tkf.DesField + " = " + tkf.SrcField;
                }

            }
        }


        private void btnOK_Click(object sender, EventArgs e)
        {
            transaction.TransKeyFields = this.transPrivateCopy.TransKeyFields;
        }

        private void lbDesField_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.lbRelation.SelectedIndex = this.lbDesField.SelectedIndex;
            this.lbScrField.SelectedIndex = this.lbDesField.SelectedIndex;
        }

        private void lbScrField_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.lbRelation.SelectedIndex = this.lbScrField.SelectedIndex;
            this.lbDesField.SelectedIndex = this.lbScrField.SelectedIndex;
        }
    }
}

