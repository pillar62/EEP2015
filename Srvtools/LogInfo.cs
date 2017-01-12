using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Data.SqlClient;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.OracleClient;
using System.Drawing.Design;
#if MySql
using MySql.Data.MySqlClient;
#endif

namespace Srvtools 
{
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(UpdateComponent), "Resources.LogInfo.ico")]
    public class LogInfo : InfoBaseComp,IGetValues
    {
        public LogInfo(System.ComponentModel.IContainer container)
        {
            container.Add(this);
            InitializeComponent();

            _needLog = true;
            _onlyDistinct = true;
            _logIDField = "Log_ID";
            _markField = "Log_State";
            _modifierField = "Log_User";
            _modifyDateField = "Log_DateTime";
            //_srcFieldNames = new String[] { };
            _srcFieldNames = new SrcFieldNameCollection(this, typeof(SrcFieldNameColumn));
        }

        public LogInfo()
        {
            InitializeComponent();

            _needLog = true;
            _onlyDistinct = true;
            _logIDField = "Log_ID";
            _markField = "Log_State";
            _modifierField = "Log_User";
            _modifyDateField = "Log_Date";
            _srcFieldNames = new SrcFieldNameCollection(this, typeof(SrcFieldNameColumn));
        }

        private void InitializeComponent()
        {

        }

        [Category("Infolight"),
        Description("The name of table to store log data")]
        [Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
        public String LogTableName
        {
            get { return _logTableName; }
            set { 
                    _logTableName = value;
                }
        }

        [Category("Infolight"),
        Description("Indicates whether all the columns or only modified columns need to log")]
        public Boolean OnlyDistinct
        {
            get { return _onlyDistinct; }
            set { _onlyDistinct = value; }
        }

        //[Category("Data")]
        //public String[] SrcFieldNames
        //{
        //    get { return _srcFieldNames; }
        //    set { _srcFieldNames = value; }
        //}

        [Category("Infolight"),
        Description("Specifies which columns needed to log when they are modified")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public SrcFieldNameCollection SrcFieldNames
        {
            get { return _srcFieldNames; }
            set { _srcFieldNames = value; }
        }

        [Category("Infolight"),
        Description("Indicates whether LogInfo is enabled or disabled")]
        public Boolean NeedLog
        {
            get { return _needLog; }
            set { _needLog = value; }
        }

        [Category("Infolight"),
        Description("Specifies the column to store the infomation of type of modification")]
        [Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
        public String MarkField
        {
            get { return _markField; }
            set { _markField = value; }
        }

        [Category("Infolight"),
        Description("Specifies the column to store the name of the modified column")]
        [Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
        public String ModifierField
        {
            get { return _modifierField; }
            set { _modifierField = value; }
        }

        [Category("Infolight"),
        Description("Specifies the column to store the idenfication of log")]
        [Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
        public String LogIDField
        {
            get { return _logIDField; }
            set { _logIDField = value; }
        }

        [Category("Infolight"),
        Description("Specifies the column to store the data of the modified column")]
        [Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
        public String ModifyDateField
        {
            get { return _modifyDateField; }
            set { _modifyDateField = value; }
        }

        [Category("Infolight"),
        Description("The datetype of the time of log")]
        public String LogDateType
        {
            get { return _logDateType; }
            set { _logDateType = value; }
        }

        //[Browsable(false)]
        //public Int32 Modifier
        //{
        //    get { return _modifier; }
        //    set { _modifier = value; }
        //}

        [Browsable(false)]
        public String Name
        {
            set { _name = this.Site.Name; }
            get { return _name; }
        }

        public Boolean Log(DataRow srcRow, DataTable srcSchema, IDbConnection connection, KeyItems srcKeysList)
        {
            return Log(srcRow, srcSchema, connection, null, srcKeysList);
        }

        public Boolean Log(DataRow srcRow, DataTable srcSchema, IDbConnection connection, IDbTransaction dbTrans, KeyItems keyList)
        {
            CheckLogField();

            LogSQLBuilder logSQLBuilder = new LogSQLBuilder(this, srcRow, srcSchema, keyList);
            String logSQL = logSQLBuilder.GetLogSQL(connection, dbTrans); 

            if (logSQL.Length != 0)
            {
                IDbCommand command = connection.CreateCommand();
                command.CommandText = logSQL;
                if (dbTrans != null)
                {
                    command.Transaction = dbTrans;
                    if (command.ExecuteNonQuery() == 0)
                    { throw new Exception(); /*return false; */}
                }
                else
                {
                    if (command.ExecuteNonQuery() == 0)
                    { return false; }
                }
            }

            return true;
        }

        private void CheckLogField()
        {
            if (_logIDField == null || _logIDField.Length == 0)
            {
                String message = SysMsg.GetSystemMessage(((DataModule)this.OwnerComp).Language, "Srvtools", "LogInfo", "msg_LogIDFieldIsNull");
                throw new ArgumentException(String.Format(message, this.Name));
            }

            if (_logTableName == null || _logTableName.Length == 0)
            {
                String message = SysMsg.GetSystemMessage(((DataModule)this.OwnerComp).Language, "Srvtools", "LogInfo", "msg_LogTableNameIsNull");
                throw new ArgumentException(String.Format(message, this.Name));
            }

            if (_markField == null || _markField.Length == 0)
            {
                String message = SysMsg.GetSystemMessage(((DataModule)this.OwnerComp).Language, "Srvtools", "LogInfo", "msg_MarkFieldIsNull");
                throw new ArgumentException(String.Format(message, this.Name));
            }

            if ( _modifierField == null || _modifierField.Length == 0)
            {
                String message = SysMsg.GetSystemMessage(((DataModule)this.OwnerComp).Language, "Srvtools", "LogInfo", "msg_ModifierFieldIsNull");
                throw new ArgumentException(String.Format(message, this.Name));
            }

            if (_modifyDateField == null || _modifyDateField.Length == 0)
            {
                String message = SysMsg.GetSystemMessage(((DataModule)this.OwnerComp).Language, "Srvtools", "LogInfo", "msg_ModifyDateFieldIsNull");
                throw new ArgumentException(String.Format(message, this.Name));
            }
        }

        private String _logTableName;

        private Boolean _onlyDistinct;

        private SrcFieldNameCollection _srcFieldNames;

        private Boolean _needLog;

        private String _markField;

        private String _modifierField;

        private String _logIDField;

        private String _modifyDateField;

        private String _logDateType;

        //private Int32 _modifier;

        private String _name;

        //private IDbConnection connection;

        #region IGetValues Members

        public string[] GetValues(string sKind)
        {
#warning 这段不支持oracle
            List<string> values = new List<string>();
            if (this is LogInfo)
            {
                IDbConnection connection = null;
                foreach (Component cp in this.Container.Components)
                {
                    if (cp is UpdateComponent)
                    {
                        if ((cp as UpdateComponent).LogInfo == this as LogInfo)
                        {
                            connection = (cp as UpdateComponent).SelectCmd.InfoConnection;
                        }
                    }
                }
                if (string.Compare(sKind, "logtablename", true) == 0)//IgnoreCase
                {
                    if (connection != null)
                    {
                        string sQL = "select * from sysobjects where xtype in ('u','U','v','V')  order by [name]";
                        IDbCommand cmd = connection.CreateCommand();
                        cmd.CommandText = sQL;
                        if (connection.State == ConnectionState.Closed)
                        {
                            connection.Open(); 
                        }
                        IDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                        while (reader.Read())
                        {
                            values.Add(reader["name"].ToString());
                        }
                        reader.Close();
                    }
                    connection.Close();

                }
                if (string.Compare(sKind, "logidfield", true) == 0 || string.Compare(sKind, "markfield", true) == 0
                    || string.Compare(sKind, "modifierfield", true) == 0 || string.Compare(sKind, "modifydatefield", true) == 0)//IgnoreCase
                {
                    if (connection != null && this.LogTableName != null && this.LogTableName != "")
                    {
                        
                        ClientType type = DBUtils.GetDatabaseType(connection);
                        IDbCommand cmd = connection.CreateCommand();
                        cmd.CommandText = string.Format("SELECT * FROM {0}", DBUtils.QuoteWords(LogTableName, type));

                        IDbDataAdapter adapter = DBUtils.CreateDbDataAdapter(cmd);

                        if (connection.State == ConnectionState.Closed)
                        {
                            connection.Open();
                        }
                        DataSet ds = new DataSet();
                        try
                        {
                            adapter.FillSchema(ds, SchemaType.Mapped);
                        }
                        finally
                        {
                            connection.Close();
                        }
                        foreach (DataColumn column in ds.Tables[0].Columns)
                        {
                            values.Add(column.ColumnName);
                        }       
                    }
                }
            }
            return values.ToArray();
        }

        #endregion
    }

    public enum LogFieldMark
    {
        I = 0,

        M = 1,

        D = 2
    }


    public class SrcFieldNameCollection: InfoOwnerCollection
    {
        public SrcFieldNameCollection(Object aOwner, Type aItemType)
            : base(aOwner, typeof(SrcFieldNameColumn))
        {

        }
        public new SrcFieldNameColumn this[int index]
        {
            get
            {
                return (SrcFieldNameColumn)InnerList[index];
            }
            set
            {
                if (index > -1 && index < Count)
                {
                    if (value is SrcFieldNameColumn)
                    {
                        //原来的Collection设置为0
                        ((SrcFieldNameColumn)InnerList[index]).Collection = null;
                        InnerList[index] = value;
                        //Collection设置为this
                        ((SrcFieldNameColumn)InnerList[index]).Collection = this;
                    }
                }
            }
        }
    }


    public class SrcFieldNameColumn : InfoOwnerCollectionItem,IGetValues    
    {
        public SrcFieldNameColumn()
        { 
        
        }

        private string _name;
        [Category("Design")]
        public override string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }



        private string fieldname;
        [Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
        public string FieldName
        {
            get
            {
                return fieldname;
            }
            set
            {
                fieldname = value;
                if (fieldname != null && fieldname != "")
                {
                    this.Name = fieldname;
                }         
            }
         
        }

        #region IGetValues Members

        public string[] GetValues(string sKind)
        {
            string[] retList = null;
            List<string> values = new List<string>();
            if (this.Owner is LogInfo)
            {
                if (string.Compare(sKind, "fieldname", true) == 0)//IgnoreCase
                {
                    foreach (Component cp in (this.Owner as LogInfo).Container.Components)
                    {
                        if (cp is UpdateComponent)
                        {
                            if ((cp as UpdateComponent).LogInfo == this.Owner as LogInfo)
                            {
                                return (cp as UpdateComponent).SelectCmd.GetFields();
                            }
                        }
                    }
                }
            }
            if (values.Count > 0)
            {
                int i = values.Count;
                retList = new string[i];
                for (int j = 0; j < i; j++)
                {
                    retList[j] = values[j];
                }
            }

            return retList;
        }
        #endregion
    }
}
