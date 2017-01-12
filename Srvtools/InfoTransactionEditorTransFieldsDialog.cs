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
using System.ComponentModel.Design;
#if MySql
using MySql.Data.MySqlClient;
#endif

namespace Srvtools
{
    public partial class InfoTransactionEditorTransFieldsDialog : Form
    {
        Transaction transaction = null;
        Transaction transPrivateCopy = null;
        UpdateComponent uctran = null;
        string strDesTable = "";
        string strSrcTable = "";
        IDbConnection _conn = null;

        public InfoTransactionEditorTransFieldsDialog(Transaction trans, UpdateComponent uc, string destablename, string srctablename)
        {
            transaction = trans;
            uctran = uc;
            _conn = FindConnection(uctran);
            strDesTable = destablename;
            strSrcTable = srctablename;
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void InfoTransactionEditorTransFieldsDialog_Load(object sender, EventArgs e)
        {
            // Copy the editing Transaction
            //
            SetTransactionPrivateCopy();

            // Set up UI
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
            //cbxUpdateMode.Items.Add("Dec");
            //cbxUpdateMode.Items.Add("Disable");
            //cbxUpdateMode.Items.Add("Inc");
            //cbxUpdateMode.Items.Add("Replace");
            //cbxUpdateMode.Items.Add("WriteBack");

            // Set up ListBox 
            foreach (TransField field in transPrivateCopy.TransFields)
            {
                this.lbDesField.Items.Add(field.DesField);
                this.lbScrField.Items.Add(field.SrcField);

                switch (field.UpdateMode)
                {
                    case UpdateMode.Dec:
                        this.lbRelation.Items.Add(field.DesField + " - " + field.SrcField);
                        break;
                    case UpdateMode.Disable:
                        this.lbRelation.Items.Add(field.DesField + " X " + field.SrcField);
                        break;
                    case UpdateMode.Inc:
                        this.lbRelation.Items.Add(field.DesField + " + " + field.SrcField);
                        break;
                    case UpdateMode.Replace:
                        this.lbRelation.Items.Add(field.DesField + " = " + field.SrcField);
                        break;
                    case UpdateMode.WriteBack:
                        this.lbRelation.Items.Add(field.DesField + " <- " + field.SrcField);
                        break;
                }
            }
        }

        private void SetTransactionPrivateCopy()
        {
            transPrivateCopy = new Transaction();

            foreach (TransField field in transaction.TransFields)
            {
                TransField tf = new TransField();
                tf.DesField = field.DesField;
                tf.DesValue = field.DesValue;
                tf.FieldType = field.FieldType;
                tf.SrcField = field.SrcField;
                tf.SrcGetValue = field.SrcGetValue;
                tf.SrcValue = field.SrcValue;
                tf.UpdateMode = field.UpdateMode;

                transPrivateCopy.TransFields.Add(tf);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            List<string> lstSrcColumn = new List<string>();
            List<string> lstDesColumn = new List<string>();

            InfoTransactionTransFieldsAddDialog ittfad = new InfoTransactionTransFieldsAddDialog();

            if (strSrcTable != string.Empty && _conn != null)
            {

                if (uctran != null && uctran.SelectCmd != null)
                {
                    ittfad.cmbSrcField.Items.AddRange(uctran.SelectCmd.GetFields());
                }
            }

            if (strDesTable != string.Empty && _conn != null)
            {
                try
                {
                    lstDesColumn = AddColumn(strDesTable);
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error",  MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                foreach (string strColumn in lstDesColumn)
                {
                    ittfad.cmbDesField.Items.Add(strColumn);
                }
            }


            ittfad.cbxUpdateMode.Items.Add("Dec");
            ittfad.cbxUpdateMode.Items.Add("Disable");
            ittfad.cbxUpdateMode.Items.Add("Inc");
            ittfad.cbxUpdateMode.Items.Add("Replace");
            ittfad.cbxUpdateMode.Items.Add("WriteBack");
            ittfad.cbxUpdateMode.SelectedIndex = 3;

            if (ittfad.ShowDialog() == DialogResult.OK)
            {
                TransField tf = new TransField();
                tf.DesField = ittfad.cmbDesField.Text;
                tf.SrcField = ittfad.cmbSrcField.Text;
                tf.SrcGetValue = ittfad.txtSrcGetValue.Text;

                switch (ittfad.cbxUpdateMode.SelectedIndex)
                {
                    case 0:
                        tf.UpdateMode = UpdateMode.Dec;
                        break;
                    case 1:
                        tf.UpdateMode = UpdateMode.Disable;
                        break;
                    case 2:
                        tf.UpdateMode = UpdateMode.Inc;
                        break;
                    case 3:
                        tf.UpdateMode = UpdateMode.Replace;
                        break;
                    case 4:
                        tf.UpdateMode = UpdateMode.WriteBack;
                        break;
                }
                this.transPrivateCopy.TransFields.Add(tf);

                this.lbDesField.Items.Add(tf.DesField);
                this.lbScrField.Items.Add(tf.SrcField);

                switch (tf.UpdateMode)
                {
                    case UpdateMode.Dec:
                        this.lbRelation.Items.Add(tf.DesField + " - " + tf.SrcField);
                        break;
                    case UpdateMode.Disable:
                        this.lbRelation.Items.Add(tf.DesField + " X " + tf.SrcField);
                        break;
                    case UpdateMode.Inc:
                        this.lbRelation.Items.Add(tf.DesField + " + " + tf.SrcField);
                        break;
                    case UpdateMode.Replace:
                        this.lbRelation.Items.Add(tf.DesField + " = " + tf.SrcField);
                        break;
                    case UpdateMode.WriteBack:
                        this.lbRelation.Items.Add(tf.DesField + " <- " + tf.SrcField);
                        break;
                }
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

        private void lbRelation_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.lbDesField.SelectedIndex = this.lbRelation.SelectedIndex;
            this.lbScrField.SelectedIndex = this.lbRelation.SelectedIndex;

            int index = this.lbRelation.SelectedIndex;
            if (index != -1)
            {
                //switch (this.transPrivateCopy.TransFields[index].UpdateMode)
                //{
                //    case UpdateMode.Dec:
                //        this.cbxUpdateMode.SelectedIndex = 0;
                //        break;
                //    case UpdateMode.Disable:
                //        this.cbxUpdateMode.SelectedIndex = 1;
                //        break;
                //    case UpdateMode.Inc:
                //        this.cbxUpdateMode.SelectedIndex = 2;
                //        break;
                //    case UpdateMode.Replace:
                //        this.cbxUpdateMode.SelectedIndex = 3;
                //        break;
                //    case UpdateMode.WriteBack:
                //        this.cbxUpdateMode.SelectedIndex = 4;
                //        break;
                //}

                this.txtSrcGetValue.Text = this.transPrivateCopy.TransFields[index].SrcGetValue;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int index = this.lbRelation.SelectedIndex;
            if (index != -1)
            {
                this.lbRelation.Items.RemoveAt(index);
                this.lbDesField.Items.RemoveAt(index);
                this.lbScrField.Items.RemoveAt(index);
                this.transPrivateCopy.TransFields.RemoveAt(index);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            int selindex = this.lbRelation.SelectedIndex;

            List<string> lstSrcColumn = new List<string>();
            List<string> lstDesColumn = new List<string>();

            if (selindex != -1)
            {
                InfoTransactionTransFieldsAddDialog ittfad = new InfoTransactionTransFieldsAddDialog();

                if (strSrcTable != string.Empty && _conn != null)
                {
                    if (uctran != null && uctran.SelectCmd != null)
                    {
                        ittfad.cmbSrcField.Items.AddRange(uctran.SelectCmd.GetFields());
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
                        ittfad.cmbDesField.Items.Add(strColumn);
                    }
                }

                ittfad.cbxUpdateMode.Items.Add("Dec");
                ittfad.cbxUpdateMode.Items.Add("Disable");
                ittfad.cbxUpdateMode.Items.Add("Inc");
                ittfad.cbxUpdateMode.Items.Add("Replace");
                ittfad.cbxUpdateMode.Items.Add("WriteBack");

                ittfad.cmbDesField.Text = this.lbDesField.Items[selindex].ToString();
                ittfad.cmbSrcField.Text = this.lbScrField.Items[selindex].ToString();
                ittfad.txtSrcGetValue.Text = this.txtSrcGetValue.Text;

                switch (((TransField)this.transPrivateCopy.TransFields[selindex]).UpdateMode)
                {
                    case UpdateMode.Dec:
                        ittfad.cbxUpdateMode.SelectedIndex = 0;
                        break;
                    case UpdateMode.Disable:
                        ittfad.cbxUpdateMode.SelectedIndex = 1;
                        break;
                    case UpdateMode.Inc:
                        ittfad.cbxUpdateMode.SelectedIndex = 2;
                        break;
                    case UpdateMode.Replace:
                        ittfad.cbxUpdateMode.SelectedIndex = 3;
                        break;
                    case UpdateMode.WriteBack:
                        ittfad.cbxUpdateMode.SelectedIndex = 4;
                        break;
                }

                if (ittfad.ShowDialog() == DialogResult.OK)
                {
                    TransField tf = new TransField();
                    tf.DesField = ittfad.cmbDesField.Text;
                    tf.SrcField = ittfad.cmbSrcField.Text;
                    tf.SrcGetValue = ittfad.txtSrcGetValue.Text;

                    switch (ittfad.cbxUpdateMode.SelectedIndex)
                    {
                        case 0:
                            tf.UpdateMode = UpdateMode.Dec;
                            break;
                        case 1:
                            tf.UpdateMode = UpdateMode.Disable;
                            break;
                        case 2:
                            tf.UpdateMode = UpdateMode.Inc;
                            break;
                        case 3:
                            tf.UpdateMode = UpdateMode.Replace;
                            break;
                        case 4:
                            tf.UpdateMode = UpdateMode.WriteBack;
                            break;
                    }

                    this.transPrivateCopy.TransFields[selindex] = tf;
                    this.lbDesField.Items[selindex] = tf.DesField;
                    this.lbScrField.Items[selindex] = tf.SrcField;

                    switch (tf.UpdateMode)
                    {
                        case UpdateMode.Dec:
                            this.lbRelation.Items[selindex] = tf.DesField + " - " + tf.SrcField;
                            break;
                        case UpdateMode.Disable:
                            this.lbRelation.Items[selindex] = tf.DesField + " X " + tf.SrcField;
                            break;
                        case UpdateMode.Inc:
                            this.lbRelation.Items[selindex] = tf.DesField + " + " + tf.SrcField;
                            break;
                        case UpdateMode.Replace:
                            this.lbRelation.Items[selindex] = tf.DesField + " = " + tf.SrcField;
                            break;
                        case UpdateMode.WriteBack:
                            this.lbRelation.Items[selindex] = tf.DesField + " <- " + tf.SrcField;
                            break;
                    }

                }

            }
        }


        //int index = this.lbRelation.SelectedIndex;

        //if (index != -1)
        //{
        //    switch (this.cbxUpdateMode.SelectedIndex)
        //    {
        //        case 0:
        //            this.transPrivateCopy.TransFields[index].UpdateMode = UpdateMode.Dec;
        //            break;
        //        case 1:
        //            this.transPrivateCopy.TransFields[index].UpdateMode = UpdateMode.Disable;
        //            break;
        //        case 2:
        //            this.transPrivateCopy.TransFields[index].UpdateMode = UpdateMode.Inc;
        //            break;
        //        case 3:
        //            this.transPrivateCopy.TransFields[index].UpdateMode = UpdateMode.Replace;
        //            break;
        //        case 4:
        //            this.transPrivateCopy.TransFields[index].UpdateMode = UpdateMode.WriteBack;
        //            break;
        //    }

        //    this.transPrivateCopy.TransFields[index].SrcGetValue = this.txtSrcGetValue.Text;

        //    switch (this.transPrivateCopy.TransFields[index].UpdateMode)
        //    {
        //        case UpdateMode.Dec:
        //            this.lbRelation.Items[index] = transPrivateCopy.TransField[index].DesField + " dec " + transPrivateCopy.TransFields[index].SrcField;
        //            break;
        //        case UpdateMode.Disable:
        //            this.lbRelation.Items[index] = transPrivateCopy.TransFields[index].DesField + " disable " + transPrivateCopy.TransFields[index].SrcField;
        //            break;
        //        case UpdateMode.Inc:
        //            this.lbRelation.Items[index] = transPrivateCopy.TransFields[index].DesField + " inc " + transPrivateCopy.TransFields[index].SrcField;
        //            break;
        //        case UpdateMode.Replace:
        //            this.lbRelation.Items[index] = transPrivateCopy.TransFields[index].DesField + " = " + transPrivateCopy.TransFields[index].SrcField;
        //            break;
        //        case UpdateMode.WriteBack:
        //            this.lbRelation.Items[index] = transPrivateCopy.TransFields[index].DesField + " <= " + transPrivateCopy.TransFields[index].SrcField;
        //            break;
        //    }
        //}


        private void btnOK_Click(object sender, EventArgs e)
        {
            this.transaction.TransFields = this.transPrivateCopy.TransFields;
        }
    }
}