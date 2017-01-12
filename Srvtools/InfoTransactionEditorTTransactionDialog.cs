using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel.Design;
using System.Data.SqlClient;
using System.Data.OracleClient;
using System.Data.OleDb;
using System.Data.Odbc;
using System.Reflection;
#if MySql
using MySql.Data.MySqlClient;
#endif
using System.Data.Common;

namespace Srvtools
{
    public partial class InfoTransactionEditorTTransactionDialog : Form
    {
        Transaction transaction = null;
        Transaction transPrivateCopy = null;
        IDesignerHost DesignerHost;
        UpdateComponent uctran = null;
        IDbConnection _conn;
        string SrcTableName = "";

        public InfoTransactionEditorTTransactionDialog(Transaction trans, IDesignerHost host, string srctablename)
        {
            transaction = trans;
            DesignerHost = host;
            uctran = (this.transaction.Owner as InfoTransaction).UpdateComp;
            _conn = FindConnection(uctran);
            InitializeComponent();
            this.SrcTableName = srctablename;

        }


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
                idbcon = uc.SelectCmd.InfoConnection.InternalConnection;
            }
            return idbcon;
        }

        private List<string> AddTable()
        {
            List<string> tablesList = new List<string>();
            String sQL = "";
            if (_conn is SqlConnection)
                sQL = "select * from sysobjects where xtype in ('u','U')  order by [name]";
            else if (_conn is OdbcConnection)
            {
                //sQL = "select * from systables where tabtype = 'T' and tabid >= 100 order by tabname";
                System.Data.Common.DbConnection dbc = _conn as System.Data.Common.DbConnection;
                String[] Params = null;
                String UserID = GetFieldParam(_conn.ConnectionString.ToLower(), "user id");
                Params = new String[] { UserID.ToUpper() };
                if (dbc.State == ConnectionState.Closed) dbc.Open();
                DataTable T = dbc.GetSchema("Tables", Params);
                T.Select("", "TABLE_NAME DESC");
                foreach (DataRow DR in T.Rows)
                {
                    tablesList.Add(DR["TABLE_NAME"].ToString());
                }
                return tablesList;
            }
            else if (_conn is OracleConnection)
            {
                //sQL = "select * from user_tables order by table_name";
                DbConnection dbc = _conn as DbConnection;
                String[] Params = null;
                String UserID = GetFieldParam(_conn.ConnectionString.ToLower(), "user id");
                Params = new String[] { UserID.ToUpper() };
                if (dbc.State == ConnectionState.Closed) dbc.Open();
                DataTable T = dbc.GetSchema("Tables", Params);
                T.Select("", "TABLE_NAME DESC");
                foreach (DataRow DR in T.Rows)
                {
                    String S = "";
                    if (!String.IsNullOrEmpty(DR["OWNER"].ToString()))
                        S = DR["OWNER"].ToString() + '.';
                    tablesList.Add(S + DR["TABLE_NAME"].ToString());
                }
                T = dbc.GetSchema("Views", Params);
                T.Select("", "VIEW_NAME DESC");
                foreach (DataRow DR in T.Rows)
                {
                    String S = "";
                    if (!String.IsNullOrEmpty(DR["OWNER"].ToString()))
                        S = DR["OWNER"].ToString() + '.';
                    tablesList.Add(S + DR["VIEW_NAME"].ToString());
                }
                T = dbc.GetSchema("Synonyms", Params);
                T.Select("", "SYNONYM_NAME DESC");
                foreach (DataRow DR in T.Rows)
                {
                    String S = "";
                    if (!String.IsNullOrEmpty(DR["OWNER"].ToString()))
                        S = DR["OWNER"].ToString() + '.';
                    tablesList.Add(S + DR["SYNONYM_NAME"].ToString());
                }
                return tablesList;
            }
            else if (_conn is OleDbConnection)
                sQL = "select * from sysobjects where type in ('u','U')  order by [name]";
            else if (_conn.GetType().Name == "MySqlConnection")
                sQL = "show tables";
            else if (_conn.GetType().Name == "IfxConnection")
                sQL = "select * from sysobjects where xtype in ('u','U')  order by [name]";

            IDbCommand cmd = _conn.CreateCommand();
            cmd.CommandText = sQL;
            if (_conn.State == ConnectionState.Closed)
            { _conn.Open(); }

            IDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (reader.Read())
            {
                if (_conn is SqlConnection)
                    tablesList.Add(reader["name"].ToString());
                else if (_conn is OdbcConnection)
                    tablesList.Add(reader["name"].ToString());
                else if (_conn is OracleConnection)
                    tablesList.Add(reader["table_name"].ToString());
                else if (_conn is OleDbConnection)
                    tablesList.Add(reader["name"].ToString());
                else if (_conn.GetType().Name == "MySqlConnection")
                    tablesList.Add(reader[0].ToString());
                else if (_conn.GetType().Name == "IfxConnection")
                    tablesList.Add(reader["name"].ToString());
            }
            reader.Close();
            _conn.Close();
            return tablesList;
        }

        public static String GetFieldParam(String Source, String PropName)
        {
            String Result = "";
            String TempParam, S1, S2;
            Char AChar;
            if (Source == "" || Source == null)
                return "";
            AChar = Source[0];
            if (AChar == ';')
            {
                TempParam = Source.Substring(1, Source.Length - 1);
            }
            else
            {
                TempParam = Source;
            }

            while (TempParam != "")
            {
                S1 = GetToken(ref TempParam, new Char[] { ';' });
                if (S1 == "")
                    break;
                S2 = GetToken(ref S1, new Char[] { '=' });
                S2 = S2.Trim();
                if (S2.CompareTo(PropName) != 0)
                    continue;
                Result = S1;
                break;
            }
            return Result;
        }

        public static String GetToken(ref String AString, char[] Fmt)
        {
            String Result = "";
            while (AString.Length != 0 && AString[0] == ' ')
            {
                AString = AString.Remove(1, 1);
            }

            if (AString.Length == 0)
                return Result;

            Boolean Found = false;
            int I = 0;
            while (I < AString.Length)
            {
                Found = false;
                if ((byte)AString[I] <= 128)
                {
                    foreach (char C in Fmt)
                    {
                        if (AString[I] == C)
                        {
                            Found = true;
                            break;
                        }
                    }
                    if (!Found)
                        I++;
                }
                else
                {
                    I = I + 2;
                }
                if (Found)
                    break;
            }

            if (Found)
            {
                Result = AString.Substring(0, I);
                AString = AString.Remove(0, I + 1);
            }
            else
            {
                Result = AString;
                AString = "";
            }

            return Result;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void InfoTransactionEditorTTransactionDialog_Load(object sender, EventArgs e)
        {
            // Copy the editing Transaction
            //
            SetTransactionPrivateCopy();

            // Set up UI
            SetUpUI();
        }


        private void SetUpUI()
        {
            // cbxTransMode
            this.cbxTransMode.Items.Add("AlwaysAppend");
            this.cbxTransMode.Items.Add("AutoAppend");
            this.cbxTransMode.Items.Add("Exception");
            this.cbxTransMode.Items.Add("Ignore");


            //cmbTransTableName
            if (_conn != null)
            {
                List<string> lstTable = AddTable();
                foreach (string table in lstTable)
                {
                    cmbTransTableName.Items.Add(table);
                }

            }

            // cbxAutoNumber
            this.cbxAutoNumber.Items.Add("");
            foreach (IComponent comp in DesignerHost.Container.Components)
            {
                if (comp is AutoNumber)
                {
                    this.cbxAutoNumber.Items.Add(comp.Site.Name);
                }
            }

            this.txtTransStep.Text = this.transPrivateCopy.TransStep.ToString();

            this.cmbTransTableName.Text = this.transPrivateCopy.TransTableName;

            if (this.transPrivateCopy.AutoNumber != null
                && this.transPrivateCopy.AutoNumber.Site != null)
            {
                this.cbxAutoNumber.Text = this.transPrivateCopy.AutoNumber.Site.Name;
            }
            else
            {
                this.cbxAutoNumber.Text = "";
            }

            switch (this.transPrivateCopy.TransMode)
            {
                case TransMode.AlwaysAppend:
                    this.cbxTransMode.SelectedIndex = 0;
                    break;
                case TransMode.AutoAppend:
                    this.cbxTransMode.SelectedIndex = 1;
                    break;
                case TransMode.Exception:
                    this.cbxTransMode.SelectedIndex = 2;
                    break;
                case TransMode.Ignore:
                    this.cbxTransMode.SelectedIndex = 3;
                    break;
            }

            //new add by ccm

            this.cbInsert.Checked = this.transPrivateCopy.WhenInsert;
            this.cbUpdate.Checked = this.transPrivateCopy.WhenUpdate;
            this.cbDelete.Checked = this.transPrivateCopy.WhenDelete;

            // BefaoreTransaction
            //try
            //{
            //    IEventBindingService ebs =
            //        this.DesignerHost.GetService(typeof(IEventBindingService)) as IEventBindingService;
            //    EventDescriptor ed = TypeDescriptor.GetEvents(transaction)["AfterTrans"];
            //    PropertyDescriptor pd = ebs.GetEventProperty(ed);
            //    this.Text = pd.GetValue(transaction).ToString();
            //    pd.SetValue(transaction, "AG");
            //}
            //catch (Exception err)
            //{
            //    MessageBox.Show(err.Message);
            //}

            // AfterTransaction

            // Enable


        }

        private void SetTransactionPrivateCopy()
        {
            transPrivateCopy = new Transaction();

            transPrivateCopy.AutoNumber = transaction.AutoNumber;
            transPrivateCopy.Name = transaction.Name;

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
            transPrivateCopy.TransMode = transaction.TransMode;
            transPrivateCopy.TransStep = transaction.TransStep;
            transPrivateCopy.TransTableName = transaction.TransTableName;
            //new add by ccm
            transPrivateCopy.WhenInsert = transaction.WhenInsert;
            transPrivateCopy.WhenUpdate = transaction.WhenUpdate;
            transPrivateCopy.WhenDelete = transaction.WhenDelete;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (this.cbxAutoNumber.Text != "")
            {
                transaction.AutoNumber = this.DesignerHost.Container.Components[this.cbxAutoNumber.Text] as AutoNumber;
            }
            else
            {
                transaction.AutoNumber = null;
            }

            transaction.TransFields = transPrivateCopy.TransFields;

            transaction.TransKeyFields = transPrivateCopy.TransKeyFields;


            switch (this.cbxTransMode.SelectedIndex)
            {
                case 0:
                    transaction.TransMode = TransMode.AlwaysAppend;
                    break;
                case 1:
                    transaction.TransMode = TransMode.AutoAppend;
                    break;
                case 2:
                    transaction.TransMode = TransMode.Exception;
                    break;
                case 3:
                    transaction.TransMode = TransMode.Ignore;
                    break;
            }

            transaction.TransTableName = this.cmbTransTableName.Text;

            //new add by ccm
            transaction.WhenInsert = cbInsert.Checked;
            transaction.WhenUpdate = cbUpdate.Checked;
            transaction.WhenDelete = cbDelete.Checked;
        }

        private void btnTransKeyFields_Click(object sender, EventArgs e)
        {
            InfoTransactionEditorKeyFieldsDialog itekfd =
                new InfoTransactionEditorKeyFieldsDialog(this.transPrivateCopy, this.uctran, this.cmbTransTableName.Text, SrcTableName);
            itekfd.ShowDialog();
        }

        private void btnTransFields_Click(object sender, EventArgs e)
        {
            InfoTransactionEditorTransFieldsDialog itetfd =
                new InfoTransactionEditorTransFieldsDialog(this.transPrivateCopy, this.uctran, this.cmbTransTableName.Text, SrcTableName);
            itetfd.ShowDialog();
        }
    }
}