using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using System.Data.SqlClient;
using System.Data.Odbc;
using System.Data.OracleClient;
using System.Data.OleDb;

using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Data.Common; 

namespace Srvtools
{
    public class Transaction : InfoOwnerCollectionItem, IGetValues
    {
        #region Constructor

        public Transaction() : this("")
        {
        }

        public Transaction(String name)
        {
            _transKeyFields = new TransKeyFieldCollection(this, typeof(TransKeyField));
            _transFields = new TransFieldCollection(this, typeof(TransField));
            _transMode = TransMode.AutoAppend;
            _name = name;
            _whenDelete = true;
            _whenInsert = true;
            _whenUpdate = true;
        }

        #endregion

        #region Properties

        [Category("Data")]
        public Int32 TransStep
        {
            set { _transStep = value; }
            get { return _transStep; }
        }

        [Category("Data")]
        [Editor(typeof(TransTableNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public String TransTableName
        {
            set { _transTableName = value; }
            get { return _transTableName; }
        }

        [Category("Design")]
        public TransMode TransMode
        {
            set { _transMode = value; }
            get { return _transMode; }
        }

        [Category("Design")]
        public bool WhenInsert
        {
            set { _whenInsert = value; }
            get { return _whenInsert; }
        }

        [Category("Design")]
        public bool WhenUpdate
        {
            set { _whenUpdate = value; }
            get { return _whenUpdate; }
        }

        [Category("Design")]
        public bool WhenDelete
        {
            set { _whenDelete = value; }
            get { return _whenDelete; }
        }

        [Category("Data")]
        public AutoNumber AutoNumber
        {
            set { _autoNumber = value; }
            get { return _autoNumber; }
        }

        [Category("Data")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public TransKeyFieldCollection TransKeyFields
        {
            set { _transKeyFields = value; }
            get { return _transKeyFields; }
        }

        [Category("Data")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public TransFieldCollection TransFields
        {
            set { _transFields = value; }
            get { return _transFields; }
        }

        [Category("Data")]
        public override String Name
        {
            set { _name = value; }
            get { return _name; }
        }

        #endregion

        #region Methods

        public override string ToString()
        {
            return _name;
        }

        public DataTable GetTransTableSchema()
        {
            if (_transTableSchema != null)
            {
                return _transTableSchema;
            }
            else
            {
                return null;
            }
        }

        public void SetTransTableSchema(DataTable transTableSchema)
        {
            _transTableSchema = transTableSchema;
        }

        #endregion

        #region IGetValues

        public string[] GetValues(string sKind)
        {
            List<string> tablesList = new List<string>();
            InfoTransaction infoTrans = (InfoTransaction)this.Owner;

           
            if (infoTrans.UpdateComp == null)
            { throw new Exception("The 'UpdateComp' property is null."); }

            if (infoTrans.UpdateComp.SelectCmd == null)
            { throw new Exception("The 'UpdateComp.SelectCmd' property is null."); }

            if (infoTrans.UpdateComp.SelectCmd.InfoConnection == null)
            { throw new Exception("The 'UpdateComp.SelectCmd.InfoConnection' property is null."); }

            IDbConnection myConn = infoTrans.UpdateComp.SelectCmd.InfoConnection.InternalConnection;

            String sQL = "";
            if (myConn is SqlConnection)
                sQL = "select * from sysobjects where xtype in('u','U') order by [name]";
            else if (myConn is OdbcConnection)
            {
                //sQL = "select * from systables where tabtype = 'T' and tabid >= 100 order by tabname";
                DbConnection dbc = myConn as DbConnection;
                String[] Params = null;
                String UserID = GetFieldParam(myConn.ConnectionString.ToLower(), "user id");
                Params = new String[] { UserID.ToUpper() };
                if (dbc.State == ConnectionState.Closed) dbc.Open();
                DataTable T = dbc.GetSchema("Tables", Params);
                T.Select("", "TABLE_NAME DESC");
                foreach (DataRow DR in T.Rows)
                {
                    tablesList.Add(DR["TABLE_NAME"].ToString());
                }
                return tablesList.ToArray();
            }
            else if (myConn is OracleConnection)
            {
                //sQL = "select * from user_tables order by table_name";
                DbConnection dbc = myConn as DbConnection;
                String[] Params = null;
                String UserID = GetFieldParam(myConn.ConnectionString.ToLower(), "user id");
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
                return tablesList.ToArray();
            }
            else if (myConn is OleDbConnection)
                sQL = "select * from sysobjects where type in('u','U') order by [name]";
            else if (myConn.GetType().Name == "MySqlConnection")
                sQL = "show tables;";
            else if (myConn.GetType().Name == "IfxConnection")
                sQL = "select * from systables where tabtype = 'T' and tabid >= 100 order by tabname";

            //MessageBox.Show(sQL);

            IDbCommand myCmd = myConn.CreateCommand();
            myCmd.CommandText = sQL;

            if (myConn.State == ConnectionState.Closed)
            {
                myConn.Open();
            }

            IDataReader reader = myCmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (reader.Read())
            {
                if (myConn is SqlConnection)
                    tablesList.Add(reader["name"].ToString());
                else if (myConn is OdbcConnection)
                    tablesList.Add(reader["tabname"].ToString());
                else if (myConn is OracleConnection)
                    tablesList.Add(reader["table_name"].ToString());
                else if (myConn is OleDbConnection)
                    tablesList.Add(reader["name"].ToString());
                else if (myConn.GetType().Name == "MySqlConnection")
                    tablesList.Add(reader[0].ToString());
                else if (myConn.GetType().Name == "IfxConnection")
                    tablesList.Add(reader["tabname"].ToString());
            }
            reader.Close();
            myConn.Close();

            return tablesList.ToArray();
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

        #endregion

        #region Vars

        private Int32 _transStep;
        private String _transTableName;
        private AutoNumber _autoNumber;
        private TransKeyFieldCollection _transKeyFields;
        private TransFieldCollection _transFields;
        private DataTable _transTableSchema;
        private String _name;
        private TransMode _transMode;
        private bool _whenInsert;
        private bool _whenUpdate;
        private bool _whenDelete;

        #endregion
    }

    public enum TransMode
    {
        /// <summary>
        /// If not exist the transtable then auto append the transtable.
        /// </summary>
        AutoAppend = 0,

        /// <summary>
        /// If not exist the transtable then throw exception and rollback.
        /// </summary>
        Exception = 1,

        /// <summary>
        /// If not exist the transtable then contiute the next transstep.
        /// </summary>
        Ignore = 2,

        /// <summary>
        /// 
        /// </summary>
        AlwaysAppend = 3
    }

    public class TransTableNameEditor : UITypeEditor
    {
        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand)]
        public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand)]
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService editorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

            IGetValues aItem = (IGetValues)context.Instance;
            if (editorService != null)
            {
                StringListSelector mySelector = new StringListSelector(editorService, aItem.GetValues(context.PropertyDescriptor.Name));
                string strValue = (string)value;
                if (mySelector.Execute(ref strValue)) value = strValue;
            }
            return value;
        }
    } 
}
