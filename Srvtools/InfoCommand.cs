using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Data.Odbc;
using System.Data.OracleClient;

#if MySql
using MySql.Data.MySqlClient;
#endif

using System.Xml;
using System.Text.RegularExpressions;
using Microsoft.Win32;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Reflection;

namespace Srvtools
{
    public enum SecurityStyle 
    {
        ssByNone,
        ssByUser,
        ssByGroup,
        ssByRole,
        [Browsable(false)]
        [Obsolete("The option has been abolished", true)]
        ssByRoleGroup,
        ssByOrg,
        [Browsable(false)]
        [Obsolete("The recommended alternative is ssByOrgShare", true)]
        ssByOrgSharre,
        ssByOrgShare
    };

    /// <summary>
    /// IDbCommand of EEP, no longer inherit IInfoCommand interface
    /// </summary>
    [ToolboxItem(true)]
    [Designer(typeof(infoCommandDesigner), typeof(IDesigner))]
    [ToolboxBitmap(typeof(InfoCommand), "Resources.InfoCommand.ico")]
    public class InfoCommand : InfoBaseComp, IDbCommand, ICloneable, IGetValues, ISupportInitialize, IInfoCommand
    {
        private IContainer components;
        private bool fInit = false;

        /// <summary>
        /// Initializes a new instance of the InfoCommand class.
        /// </summary>
        public InfoCommand()
        {
            InitializeComponent();
            keyFields = new KeyItems(this, typeof(KeyItem));
            EncodingBefore = "Windows-1252";
        }

        /// <summary>
        /// Initializes a new instance of the InfoCommand class.
        /// and adds the InfoCommand to the specified container.
        /// </summary>
        /// <param name="container">The System.ComponentModel.IContainer to add the current InfoCommand to.</param>
        public InfoCommand(System.ComponentModel.IContainer container)
            : this()
        {
            container.Add(this);
        }

        public InfoCommand(object[] clientInfo)
            : this()
        {
            ClientInfo = clientInfo;
        }


        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
        }

        #region IDbCommand Member
        private string commandText;
        /// <summary>
        /// Gets or sets the text command to run against the data source.
        /// </summary>
        [Category("Infolight"),
        Description("The text command to run against the data source")]
        [Editor(typeof(CommandTextEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string CommandText
        {
            get
            {
                return commandText;
            }
            set
            {
                if (DesignMode && this.CommandType == CommandType.Text && !fInit)
                {
                    if (InfoConnection != null)
                    {
                        try
                        {
                            if (string.IsNullOrEmpty(value))
                            {
                                if (this.KeyFields != null && this.KeyFields.Count != 0)
                                {
                                    if (MessageBox.Show("Would you like to clear the InfoCommand KeyFields?"
                                                , "Microsoft Visual Studio", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                    {
                                        this.KeyFields.Clear();
                                    }
                                }
                            }
                            else
                            {
                                string oldtable = string.IsNullOrEmpty(commandText) ? string.Empty : DBUtils.GetTableName(commandText, true);
                                string newtable = DBUtils.GetTableName(value, true);
                                if (DBUtils.RemoveQuote(oldtable) != DBUtils.RemoveQuote(newtable))
                                {
                                    if (this.KeyFields.Count == 0 || MessageBox.Show("Would you like to regenerate the InfoCommand KeyFields using the table primary keys?"
                               , "Microsoft Visual Studio", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                    {
                                        this.KeyFields.Clear();
                                        IDbCommand cmd = this.InfoConnection.CreateCommand();
                                        cmd.CommandText = value;
                                        IDbDataAdapter adapter = DBUtils.CreateDbDataAdapter(cmd);
                                        DataSet ds = new DataSet();
                                        if (this.InfoConnection.State != ConnectionState.Open)
                                        {
                                            this.InfoConnection.Open();
                                        }
                                        try
                                        {
                                            adapter.FillSchema(ds, SchemaType.Mapped);
                                        }
                                        finally
                                        {
                                            this.InfoConnection.Close();
                                        }
                                        foreach (DataColumn column in ds.Tables[0].PrimaryKey)
                                        {
                                            KeyItem item = new KeyItem();
                                            item.KeyName = column.ColumnName;
                                            this.KeyFields.Add(item);
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                commandText = value;
                if (internalDbCommand != null)
                {
                    internalDbCommand.CommandText = value;
                }
            }
        }

        private int commandTimeout = 30;
        /// <summary>
        /// Gets or sets the wait time before terminating the attempt to execute a command 
        /// and generating an error.
        /// </summary>
        [Category("Infolight"),
        Description("The wait time before terminating the attempt to execute a command and generating an error")]
        public int CommandTimeout
        {
            get
            {
                return commandTimeout;
            }
            set
            {
                commandTimeout = value;
                if (internalDbCommand != null)
                {
                    internalDbCommand.CommandTimeout = value;
                }
            }
        }

        private CommandType commandType = CommandType.Text;
        /// <summary>
        /// Indicates or specifies how the InfoCommand.CommandText property 
        /// is interpreted.
        /// </summary>
        [Category("Infolight"),
        Description("How the InfoCommand.CommandText property is interpreted")]
        public CommandType CommandType
        {
            get { return commandType; }
            set {
                    commandType = value;
                    if (internalDbCommand != null)
                    {
                        internalDbCommand.CommandType = value;
                    }
                }
        }

        private IDbConnection connection;
        /// <summary>
        /// Gets or sets the System.Data.IDbConnection used by this instance of the InfoCommand.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IDbConnection Connection
        {
            get 
            {
                if (!DesignMode)
                {
                    if (connection == null)
                    {
                        string alias = EEPAlias;
                        if (string.IsNullOrEmpty(alias))
                        {
                            if (this.InfoConnection != null)
                            {
                                alias = this.InfoConnection.EEPAlias;
                            }
                        }
                        if (!string.IsNullOrEmpty(alias))
                        {
                            DbConnectionSet.DbConnection db = DbConnectionSet.GetDbConn(alias);
                            if (db != null)
                            {
                                connection = db.CreateConnection();
                            }
                            else
                            {
                                throw new EEPException(EEPException.ExceptionType.PropertyInvalid, this.GetType(), null, "EEPAlias", alias);
                            }
                        }
                    }
                }
                return connection;
            }
            set
            {
                if (value != connection)
                {
                    connection = value;
                    internalDbCommand = null;
                }
            }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IDataParameterCollection Parameters
        {
            get { return InternalDbCommand.Parameters; }
        }

        private IDbTransaction transaction;
        /// <summary>
        /// Gets or sets the transaction within which the Command object of a .NET Framework 
        /// data provider executes.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IDbTransaction Transaction
        {
            get { return transaction; }
            set
            { 
                transaction = value;
                if (internalDbCommand != null)
                {
                    internalDbCommand.Transaction = value;
                }
            }
        }

        private UpdateRowSource updatedRowSource;
        /// <summary>
        /// Gets or sets how command results are applied to the System.Data.DataRow when
        /// used by the System.Data.IDataAdapter.Update(System.Data.DataSet) method of
        /// a System.Data.Common.DbDataAdapter.
        /// </summary>
        [Category("Infolight"),
        Description(" Specifies how query command results are applied to the row being updated")]
        public UpdateRowSource UpdatedRowSource
        {
            get { return updatedRowSource; }
            set
            {
                updatedRowSource = value;
                if (internalDbCommand != null)
                {
                    internalDbCommand.UpdatedRowSource = value;
                }
            }
        }

        /// <summary>
        /// Attempts to cancels the execution of an System.Data.IDbCommand
        /// </summary>
        public void Cancel()
        {
            InternalDbCommand.Cancel();
        }

        /// <summary>
        /// Creates a new instance of an System.Data.IDbDataParameter object.
        /// </summary>
        /// <returns>An IDbDataParameter object.</returns>
        public IDbDataParameter CreateParameter()
        {
            return InternalDbCommand.CreateParameter();
        }

        /// <summary>
        /// Executes an SQL statement against the Connection object of a .NET Framework
        /// data provider, and returns the number of rows affected.
        /// </summary>
        /// <returns>The number of rows affected.</returns>
        public int ExecuteNonQuery()
        {
            this.Log(InternalDbCommand.CommandText);
            int number = InternalDbCommand.ExecuteNonQuery();
            GetParameters(InternalDbCommand);
            return number;
        }

        /// <summary>
        /// Executes the System.Data.IDbCommand.CommandText against the System.Data.IDbCommand.Connection
        /// and builds an System.Data.IDataReader.
        /// </summary>
        /// <returns>An System.Data.IDataReader object.</returns>
        public IDataReader ExecuteReader()
        {
            this.Log(InternalDbCommand.CommandText);
            IDataReader reader = InternalDbCommand.ExecuteReader();
            GetParameters(InternalDbCommand);
            return reader;
        }

        /// <summary>
        /// Executes the System.Data.IDbCommand.CommandText against the System.Data.IDbCommand.Connection,
        /// and builds an System.Data.IDataReader using one of the System.Data.CommandBehavior
        /// values.
        /// </summary>
        /// <param name="behavior">One of the System.Data.CommandBehavior values.</param>
        /// <returns>An System.Data.IDataReader object.</returns>
        public IDataReader ExecuteReader(CommandBehavior behavior)
        {
            this.Log(InternalDbCommand.CommandText);
            IDataReader reader = InternalDbCommand.ExecuteReader(behavior);
            GetParameters(InternalDbCommand);
            return reader;
        }

        /// <summary>
        /// Executes the query, and returns the first column of the first row in the
        /// resultset returned by the query. Extra columns or rows are ignored.
        /// </summary>
        /// <returns>The first column of the first row in the resultset.</returns>
        public object ExecuteScalar()
        {
            this.Log(InternalDbCommand.CommandText);
            object obj = InternalDbCommand.ExecuteScalar();
            GetParameters(InternalDbCommand);
            return obj;
        }

        /// <summary>
        /// Creates a prepared (or compiled) version of the command on the data source.
        /// </summary>
        public void Prepare()
        {
            InternalDbCommand.Prepare();
        }
        #endregion

        #region ICloneable Member
        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        #endregion

        #region IGetValues Member
        /// <summary>
        /// Gets options of property.
        /// </summary>
        /// <param name="sKind">Name of property.</param>
        /// <returns>Options of property</returns>
        public string[] GetValues(string sKind)
        {
            if (sKind.Equals("SecFieldName") || sKind.Equals("SiteFieldName"))//SecFieldName的属性编辑器
            {
                return this.GetFields();
            }
            else if(sKind.Equals("EEPAlias"))
            {
                return DbConnectionSet.GetAvaliableAlias();
            }
            else
                return new string[] { };
        }
        #endregion

        #region ISupportInitialize Member
        /// <summary>
        /// Signals the object that initialization is starting.
        /// </summary>
        public void BeginInit()
        {
            fInit = true;
        }

        /// <summary>
        /// Signals the object that initialization is complete.
        /// </summary>
        public void EndInit() { fInit = false; }
        #endregion

        private bool dynamicTableName;

        public bool DynamicTableName
        {
            get { return dynamicTableName; }
            set { dynamicTableName = value; }
        }

        [Category("Infolight")]
        public string EncodingAfter { get; set; }


        [Category("Infolight")]
        public string EncodingBefore {get; set;}

        [Category("Infolight")]
        private string encodingConvert;
        public string EncodingConvert
        {
            get { return encodingConvert; }
            set
            {
                encodingConvert = value;
                if (!string.IsNullOrEmpty(value))
                {
                    if (string.IsNullOrEmpty(EncodingBefore))
                    {
                        EncodingBefore = "iso-8859-1";
                    }
                    if (string.IsNullOrEmpty(EncodingAfter))
                    {
                        EncodingAfter = "Unicode";
                    }
                }
            }
        }


        private string eEPAlias;
        /// <summary>
        /// Gets or sets the alias of System.Data.IDbConnection used by this instance of the InfoCommand.
        /// </summary>
        [Category("Infolight"),
        Description("The alias of System.Data.IDbConnection used by this instance of the InfoCommand")]
        [Editor(typeof(PropertyDropDownEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string EEPAlias
        {
            get { return eEPAlias; }
            set { eEPAlias = value; }
        }

        private InfoConnection infoConnection;
        /// <summary>
        /// Gets or sets the InfoConnection used by this instance of the InfoCommand.
        /// </summary>
        [Category("Infolight"),
        Description("The InfoConnection used by this instance of the InfoCommand")]
        public InfoConnection InfoConnection
        {
            get { return infoConnection; }
            set { infoConnection = value; }
        }

        private bool cacheConnection;
        [Category("Infolight")]
        public bool CacheConnection
        {
            get { return cacheConnection; }
            set { cacheConnection = value; }
        }

        private bool notificationAutoEnlist;
        /// <summary>
        /// Gets or sets a value indicating whether the application should automatically
        /// receive query notifications from a common System.Data.SqlClient.SqlDependency
        /// object.
        /// </summary>
        [Category("Infolight"),
        Description(@"a value indicating whether the application should automatically
receive query notifications from a common System.Data.SqlClient.SqlDependency
object")]
        public bool NotificationAutoEnlist
        {
            get
            {
                return notificationAutoEnlist;
            }
            set
            {
                notificationAutoEnlist = value;
                if (internalDbCommand != null && internalDbCommand is SqlCommand)
                {
                    (internalDbCommand as SqlCommand).NotificationAutoEnlist = value;
                }
            }
        }

        private List<InfoParameter> infoParameters = new List<InfoParameter>();
        /// <summary>
        /// Gets the collection of InfoParameter objects.
        /// </summary>
        [Category("Infolight"),
        Description("Specifies the parameters of Stored Procedure")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public List<InfoParameter> InfoParameters
        {
            get
            {
                return infoParameters;
            }
        }

        private SecurityStyle secStyle;
        /// <summary>
        /// Gets or sets the style of security of query command.
        /// </summary>
        [Category("Infolight"),
        Description("The style of security of query command")]
        public SecurityStyle SecStyle
        {
            get { return secStyle; }
            set { secStyle = value; }
        }

        private string secFieldName;
        /// <summary>
        /// Gets or sets the field of security of query command.
        /// </summary>
        [Category("Infolight"),
        Description("The field of security of query command")]
        [Editor(typeof(PropertyDropDownEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string SecFieldName
        {
            get { return secFieldName; }
            set { secFieldName = value; }
        }

        private string secExcept;
        /// <summary>
        /// Gets or sets the value of group not apply security of query command.
        /// </summary>
        [Category("Infolight"),
        Description("The value of group not apply security of query command.")]
        public string SecExcept
        {
            get { return secExcept; }
            set { secExcept = value; }
        }

        private bool siteControl;
        /// <summary>
        /// Gets or sets a value indicating whether use sitecode of query command.
        /// </summary>
        [Category("Infolight"),
        Description("A value indicating whether use sitecode of query command")]
        public bool SiteControl
        {
            get { return siteControl; }
            set { siteControl = value; }
        }

        private string siteFieldName;
        /// <summary>
        /// Gets or sets the field of sitecode.
        /// </summary>
        [Category("Infolight"),
        Description("The field of sitecode")]
        [Editor(typeof(PropertyDropDownEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string SiteFieldName
        {
            get { return siteFieldName; }
            set { siteFieldName = value; }
        }

        private bool selectPaging;
        [Category("Infolight"),
        Description("SelectPaging")]
        public bool SelectPaging
        {
            get { return selectPaging; }
            set { selectPaging = value; }
        }

        private int selectTop;
        /// <summary>
        /// Gets or sets the amount of data in 'select top' string.
        /// </summary>
        [Category("Infolight"),
        Description("The amount of data in 'select top' string")]
        public int SelectTop
        {
            get { return selectTop; }
            set { selectTop = value; }
        }

        private bool multiSetWhere;
        /// <summary>
        /// Gets or sets a value indicating style of apply where.
        /// </summary>
        [Category("Infolight"),
        Description("A value indicating style of apply where")]
        public bool MultiSetWhere
        {
            get { return multiSetWhere; }
            set { multiSetWhere = value; }
        }

        private KeyItems keyFields;
        /// <summary>
        /// Gets or sets primary key of InfoCommand.
        /// </summary>
        [Category("Infolight"),
        Description("Primary key of InfoCommand")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public KeyItems KeyFields
        {
            get
            {
                return keyFields;
            }
        }

        private IDbCommand internalDbCommand;
        private IDbCommand InternalDbCommand
        {
            get
            {
                if (internalDbCommand == null)
                {
                    if (Connection == null)
                    {
                        throw new EEPException(EEPException.ExceptionType.PropertyNull, this.GetType(), null, "Connection", null);
                    }
                    internalDbCommand = Connection.CreateCommand();
                    internalDbCommand.CommandText = CommandText;
                    internalDbCommand.CommandTimeout = CommandTimeout;
                    internalDbCommand.CommandType = CommandType;
                    internalDbCommand.Transaction = Transaction;
                    internalDbCommand.UpdatedRowSource = UpdatedRowSource;
                    if (internalDbCommand is SqlCommand)
                    {
                        (internalDbCommand as SqlCommand).NotificationAutoEnlist = NotificationAutoEnlist;
                    }
                    SetParameters(internalDbCommand);
                }
                return internalDbCommand;
            }
        }

        private DataModule OwnerModule
        {
            get { return (DataModule)this.OwnerComp; }
        }

        private object[] ClientInfo;

        private String fName = string.Empty;
        [Browsable(false)]
        public String Name
        {
            get
            {
                if (this.Site != null)
                    return Site.Name;
                else
                    return fName;
            }
            set
            {
                if (this.Site != null)
                    Site.Name = value;
                else
                    fName = value;
            }
        }

        /// <summary>
        /// builds an System.Data.IDataReader using InfoCommand.
        /// </summary>
        /// <returns>An System.Data.IDataReader using InfoCommand.</returns>
        public IDbDataAdapter CreateDbDataAdapter()
        {
            return DBUtils.CreateDbDataAdapter(InternalDbCommand);
        }

        /// <summary>
        /// Adds or refreshes rows in a specified range in the System.Data.DataSet to
        /// match those in the data source using the System.Data.DataSet and System.Data.DataTable
        /// names.
        /// </summary>
        /// <returns>The dataset.</returns>
        public DataSet ExecuteDataSet()
        {
            return ExecuteDataSet(string.Empty);
        }

        /// <summary>
        /// Adds or refreshes rows in a specified range in the System.Data.DataSet to
        /// match those in the data source using the System.Data.DataSet and System.Data.DataTable
        /// names.
        /// </summary>
        /// <param name="tableName">The name of the source table to use for table mapping.</param>
        /// <returns>The dataset.</returns>
        public DataSet ExecuteDataSet(string tableName)
        {
            IDataAdapter adapter = DBUtils.CreateDbDataAdapter(InternalDbCommand);
            DataSet ds = new DataSet();
            ds.CaseSensitive = true;
            this.Log(InternalDbCommand.CommandText);//先记录
            if (!string.IsNullOrEmpty(tableName))
            {
                ((DbDataAdapter)adapter).Fill(ds, tableName);
            }
            else
            {
                adapter.Fill(ds);
            }
            GetParameters(InternalDbCommand);

            return ds;
        }

        /// <summary>
        /// Adds or refreshes rows in a specified range in the System.Data.DataSet to
        /// match those in the data source using the System.Data.DataSet and System.Data.DataTable
        /// names.
        /// </summary>
        /// <param name="tableName">The name of the source table to use for table mapping.</param>
        /// <param name="startRecord">The zero-based record number to start with.</param>
        /// <param name="maxRecords">The maximum number of records to retrieve.</param>
        /// <returns>The dataset.</returns>
        public DataSet ExecuteDataSet(string tableName, int startRecord, int maxRecords)
        {
            IDataAdapter adapter = DBUtils.CreateDbDataAdapter(InternalDbCommand);
            DataSet ds = new DataSet();
            ds.CaseSensitive = true;
            this.Log(InternalDbCommand.CommandText);//先记录
            if (!string.IsNullOrEmpty(tableName))
            {
                ((DbDataAdapter)adapter).Fill(ds, startRecord, maxRecords, tableName);
            }
            else
            {
                adapter.Fill(ds);
            }
            GetParameters(InternalDbCommand);

            return ds;
        }

        /// <summary>
        /// Gets primary key of InfoCommand.
        /// </summary>
        /// <returns>Primary key of InfoCommand.</returns>
        public ArrayList GetKeys()
        {
            ArrayList myal = new ArrayList();
            foreach (KeyItem aItem in KeyFields)
            {
                myal.Add(aItem.KeyName);
            }
            return myal;
        }

        internal void ReunionInfoCommand(String strWhere)
        {
            if (!string.IsNullOrEmpty(strWhere))
            {
                if (!string.IsNullOrEmpty(EncodingAfter))
                {
                    //strWhere = System.Text.Encoding.GetEncoding(EncodingBefore).GetString(System.Text.Encoding.GetEncoding(EncodingAfter).GetBytes(strWhere));
                    strWhere = this.EncodeString(strWhere);
                }
                string sql = this.CommandText;
                String[] where = strWhere.Split(';');
                for (int i = 0; i < where.Length; i++)
                {
                    if (where[i].Trim().Length > 0)
                    {
                        sql = sql.Replace(string.Format("{0}={0}", i + 11), where[i]);
                    }
                }
                this.CommandText = sql;
            }
        }

        internal void DealAllSqlText(string strWhere, string strOrder)
        {
            ClientType type = DBUtils.GetDatabaseType(this.Connection);
            string sql = this.CommandText;
            if (this.CommandType == System.Data.CommandType.Text)
            {

                List<string> secExceptValues = new List<string>();
                if (!string.IsNullOrEmpty(SecExcept))
                {
                    secExceptValues.AddRange(SecExcept.Split(';'));
                }
                StringBuilder whereBuilder = new StringBuilder();
                if (SecStyle != SecurityStyle.ssByNone)
                {
                    if (string.IsNullOrEmpty(SecFieldName))
                    {
                        throw new EEPException(EEPException.ExceptionType.PropertyNull, this.GetType(), null, "SecFieldName", null);
                    }
                    else
                    {
                        string column = DBUtils.GetTableNameForColumn(sql, SecFieldName, type);
                        switch (SecStyle)
                        {
                            case SecurityStyle.ssByUser:
                                {
                                    string user = (string)OwnerModule.GetClientInfo(ClientInfoType.LoginUser);
                                    if (!string.IsNullOrEmpty(user))
                                    {
                                        if (!secExceptValues.Contains(user))
                                        {
                                            whereBuilder.Append(string.Format("{0} = '{1}'", column, user));
                                        }
                                    }
                                    break;
                                }
                            case SecurityStyle.ssByGroup:
                            case SecurityStyle.ssByRole:
                            case SecurityStyle.ssByOrg:
                            case SecurityStyle.ssByOrgShare:
                                {
                                    string list = string.Empty;
                                    if (SecStyle == SecurityStyle.ssByGroup)
                                    {
                                        list = (string)OwnerModule.GetClientInfo(ClientInfoType.GroupID);
                                    }
                                    else if (SecStyle == SecurityStyle.ssByRole)
                                    {
                                        list = (string)OwnerModule.GetClientInfo(ClientInfoType.Roles);
                                    }
                                    else if (SecStyle == SecurityStyle.ssByOrg)
                                    {
                                        list = (string)OwnerModule.GetClientInfo(ClientInfoType.OrgRoles);
                                    }
                                    else if (SecStyle == SecurityStyle.ssByOrgShare)
                                    {
                                        list = (string)OwnerModule.GetClientInfo(ClientInfoType.OrgShares);
                                    }
                                    bool isExcept = false;
                                    string[] values = list.Split(';');
                                    foreach (string value in values)
                                    {
                                        if (secExceptValues.Contains(value))
                                        {
                                            isExcept = true;
                                        }
                                    }
                                    if (string.IsNullOrEmpty(list))
                                    {
                                        whereBuilder.Append("1=0");
                                    }
                                    else if (!isExcept)
                                    {
                                        whereBuilder.Append(string.Format("{0} in ('{1}')", column, list.Replace(";", "','")));
                                    }
                                    break;
                                }
                        }
                    }
                }
                if (SiteControl)
                {
                    if (string.IsNullOrEmpty(SiteFieldName))
                    {
                        throw new EEPException(EEPException.ExceptionType.PropertyNull, this.GetType(), null, "SiteFieldName", null);
                    }
                    string column = DBUtils.GetTableNameForColumn(sql, SiteFieldName, type);
                    string sitecode = (string)OwnerModule.GetClientInfo(ClientInfoType.SiteCode);
                    if (!string.IsNullOrEmpty(sitecode))
                    {
                        if (whereBuilder.Length > 0)
                        {
                            whereBuilder.Append(" AND ");
                        }
                        whereBuilder.Append(string.Format("{0} = '{1}'", column, sitecode));
                    }
                }
                if (!string.IsNullOrEmpty(strWhere))
                {
                    if (!string.IsNullOrEmpty(EncodingAfter))
                    {
                        //strWhere = System.Text.Encoding.GetEncoding(EncodingBefore).GetString(System.Text.Encoding.GetEncoding(EncodingAfter).GetBytes(strWhere));
                        strWhere = EncodeString(strWhere);
                    }
                    if (whereBuilder.Length > 0)
                    {
                        whereBuilder.Append(" AND ");
                    }
                    whereBuilder.Append(strWhere);
                }
                sql = DBUtils.InsertWhere(sql, whereBuilder.ToString());
                sql = DBUtils.InsertOrder(sql, strOrder);
                if (this.SelectTop > 0)
                {
                    sql = DBUtils.InsertTop(sql, this.SelectTop, type);
                }
                this.CommandText = sql;
            }
            else if (this.CommandType == System.Data.CommandType.StoredProcedure)
            {
                var values = new Dictionary<string, string>();
                if (!string.IsNullOrEmpty(strWhere))
                {
                    var wheres = Regex.Split(strWhere, @"\s+and\s+", RegexOptions.IgnoreCase);
                    foreach (var w in wheres)
                    {
                        var conditions = new string[] { "<=" , ">=", "="};

                        foreach (var c in conditions)
                        {
                            var index = w.IndexOf(c);
                            if (index > 0)
                            {
                                var field = w.Substring(0, index).Trim();
                                if (c == "<=")
                                {
                                    field += "2";
                                }
                                else if (c == ">=")
                                {
                                    field += "1";
                                }
                                var value = w.Substring(index + c.Length);
                                values[field] = value;
                            }
                        }
                    }
                }
                foreach (InfoParameter p in InfoParameters)
                {
                    if (values.ContainsKey(p.ParameterName))
                    {
                        p.Value = values[p.ParameterName].Trim(" '".ToCharArray());
                    }
                    else
                    {
                        p.Value = DBNull.Value;
                    }
                }
            }
        }

        internal void DealPacketText(int startIndex, int count)
        {
            if (true)
            {
                ClientType type = DBUtils.GetDatabaseType(this.Connection);
                string sql = this.CommandText;

                //create primary key order
                StringBuilder order = new StringBuilder();
                foreach (KeyItem keyField in this.KeyFields)
                {
                    if (order.Length > 0)
                    {
                        order.Append(",");
                    }
                    order.Append(DBUtils.GetTableNameForColumn(sql, keyField.KeyName, type));
                }
                sql = DBUtils.InsertTop(sql, startIndex, count, order.ToString(), type);
                this.CommandText = sql;
            }
        }

        public string EncodeString(string str)
        {
            if (string.IsNullOrEmpty(EncodingConvert))
            {
                return Encoding.GetEncoding(EncodingBefore).GetString(Encoding.GetEncoding(EncodingAfter).GetBytes(str));
            }
            else
            {
                if (EncodingConvert == "utf-8")
                {
                    return str;
                }

                byte[] srcBytes = Encoding.GetEncoding(EncodingAfter).GetBytes(str);
                byte[] dstBytes = Encoding.Convert(Encoding.GetEncoding(EncodingAfter), Encoding.GetEncoding(EncodingConvert), srcBytes);
                char[] dstChars = new char[Encoding.GetEncoding(EncodingBefore).GetCharCount(dstBytes, 0, dstBytes.Length)];
                Encoding.GetEncoding(EncodingBefore).GetChars(dstBytes, 0, dstBytes.Length, dstChars, 0);
                return new string(dstChars);
            }
        }

        public string DecodeString(string str)
        {
            if (string.IsNullOrEmpty(EncodingConvert))
            {
                return Encoding.GetEncoding(EncodingAfter).GetString(Encoding.GetEncoding(EncodingBefore).GetBytes(str));
            }
            else
            {
                if (EncodingConvert == "utf-8")
                {
                    return str;
                }

                byte[] srcBytes = Encoding.GetEncoding(EncodingBefore).GetBytes(str);
                byte[] dstBytes = Encoding.Convert(Encoding.GetEncoding(EncodingConvert), Encoding.GetEncoding(EncodingAfter), srcBytes);
                char[] dstChars = new char[Encoding.GetEncoding(EncodingAfter).GetCharCount(dstBytes, 0, dstBytes.Length)];
                Encoding.GetEncoding(EncodingAfter).GetChars(dstBytes, 0, dstBytes.Length, dstChars, 0);
                return new string(dstChars);
            }
        }

        internal string[] GetFields()//design use
        {
            List<string> list = new List<string>();
            if (this.InfoConnection != null)
            {
                IDbCommand cmd = this.InfoConnection.CreateCommand();
                cmd.CommandText = this.CommandText;
                IDbDataAdapter adapter = DBUtils.CreateDbDataAdapter(cmd);
                if (this.InfoConnection.State != ConnectionState.Open)
                {
                    this.InfoConnection.Open();

                }
                DataSet ds = new DataSet();
                try
                {
                    adapter.FillSchema(ds, SchemaType.Mapped);
                }
                finally
                {
                    this.InfoConnection.Close();
                }

                foreach (DataColumn column in ds.Tables[0].Columns)
                {
                    list.Add(column.ColumnName);
                }
            }
            else
            {
                throw new EEPException(EEPException.ExceptionType.PropertyNull, this.GetType(), null,  "InfoConnection", null);
            }
            return list.ToArray();
        }

        private void Log(string sql)
        {
            if (SysEEPLogService.Enable && SysEEPLogService.LogSql)
            {
                SysEEPLog sysLog = new SysEEPLog(OwnerComp != null? OwnerComp.GetClientInfo(): ClientInfo, SysEEPLog.LogStyleType.Provider
                    , SysEEPLog.LogTypeType.Normal, DateTime.Now, string.Empty, string.Empty);
                sysLog.OnlyLogSqlSentence = true;
                sysLog.SqlLogType = 1; //0:ExecuteSql,1:InfoCommand,2:UpdateComp 
                sysLog.SqlSentence = sql;
                sysLog.Log();
            }
        }

        private void SetParameters(IDbCommand cmd)
        {
            foreach (InfoParameter p in InfoParameters)
            {
                IDbDataParameter param = cmd.CreateParameter();
                param.Direction = p.Direction;
                param.ParameterName = p.ParameterName;
                param.Precision = p.Precision;
                param.Scale = p.Scale;
                param.Size = p.Size;
                param.Value = p.Value;
                param.SourceColumn = p.SourceColumn;
                param.SourceVersion = p.SourceVersion;

                if (param is DbParameter)
                {
                    (param as DbParameter).SourceColumnNullMapping = p.SourceColumnNullMapping;
                }

                if (param is SqlParameter)
                {
                    (param as SqlParameter).SqlDbType = InfoDbTypeConverter.GetSqlDbType(p.InfoDbType);
                    (param as SqlParameter).XmlSchemaCollectionDatabase = p.XmlSchemaCollectionDatabase;
                    (param as SqlParameter).XmlSchemaCollectionName = p.XmlSchemaCollectionName;
                    (param as SqlParameter).XmlSchemaCollectionOwningSchema = p.XmlSchemaCollectionOwningSchema;
                }
                else if (param is OracleParameter)
                {
                    (param as OracleParameter).OracleType = InfoDbTypeConverter.GetOracleType(p.InfoDbType);
                }
                else if (param is OdbcParameter)
                {
                    (param as OdbcParameter).OdbcType = InfoDbTypeConverter.GetOdbcType(p.InfoDbType);
                }
                else if (param is OleDbParameter)
                {
                    (param as OleDbParameter).OleDbType = InfoDbTypeConverter.GetOleDbType(p.InfoDbType);
                }
#if MySql
                else if (param is MySqlParameter)
                {
                    (param as MySqlParameter).MySqlDbType = InfoDbTypeConverter.GetMySqlDbType(p.InfoDbType);
                }
#endif
#if Informix
                else if (param is IBM.Data.Informix.IfxParameter)
                {
                    (param as IBM.Data.Informix.IfxParameter).IfxType = InfoDbTypeConverter.GetIfxType(p.InfoDbType);
                }
#endif
#if Sybase
                else if (param is Sybase.Data.AseClient.AseParameter)
                {
                    (param as Sybase.Data.AseClient.AseParameter).AseDbType = InfoDbTypeConverter.GetAseType(p.InfoDbType);
                }
#endif
                cmd.Parameters.Add(param);
            }
        }

        private void GetParameters(IDbCommand cmd)
        {
            foreach (InfoParameter p in InfoParameters)
            {
                if (p.Direction != ParameterDirection.Input)
                {
                    p.Value = (cmd.Parameters[p.ParameterName] as IDbDataParameter).Value;
                }
            }
        }

      

        public event SqlEventHandler BeforeExecuteSql
        {
            add { Events.AddHandler(EventBeforeExecuteSql, value); }
            remove { Events.RemoveHandler(EventBeforeExecuteSql, value); }
        }

        private static readonly object EventBeforeExecuteSql = new object();

        internal void OnBeforeExecuteSql(SqlEventArgs e)
        {
            SqlEventHandler handler = (SqlEventHandler)Events[EventBeforeExecuteSql];
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public event SqlEventHandler AfterExecuteSql
        {
            add { Events.AddHandler(EventAfterExecuteSql, value); }
            remove { Events.RemoveHandler(EventAfterExecuteSql, value); }
        }

        private static readonly object EventAfterExecuteSql = new object();

        internal void OnAfterExecuteSql(SqlEventArgs e)
        {
            SqlEventHandler handler = (SqlEventHandler)Events[EventAfterExecuteSql];
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public static implicit operator SqlCommand(InfoCommand value)
        {
            return (SqlCommand)(value.InternalDbCommand);
        }

        public static implicit operator OracleCommand(InfoCommand value)
        {
            return (OracleCommand)(value.InternalDbCommand);
        }

        public static implicit operator OdbcCommand(InfoCommand value)
        {
            return (OdbcCommand)(value.InternalDbCommand);
        }

        public static implicit operator OleDbCommand(InfoCommand value)
        {
            return (OleDbCommand)(value.InternalDbCommand);
        }

#if MySql
        public static implicit operator MySqlCommand(InfoCommand value)
        {
            return (MySqlCommand)(value.InternalDbCommand);
        }
#endif
#if Informix
        public static implicit operator IBM.Data.Informix.IfxCommand(InfoCommand value)
        {
            return (IBM.Data.Informix.IfxCommand)(value.InternalDbCommand);
        }
#endif
#if Sybase
        public static implicit operator Sybase.Data.AseClient.AseCommand(InfoCommand value)
        {
            return (Sybase.Data.AseClient.AseCommand)(value.InternalDbCommand);
        }
#endif
        #region Obsolete Member
        [Obsolete("The recommended alternative is InfoCommand()", false)]
        public InfoCommand(ClientType type)
            : this() { }

        [Obsolete("The recommended alternative is InfoCommand()", false)]
        public InfoCommand(ConnectionType type)
            : this() { }

        [Obsolete("The recommended alternative is InfoCommand()", false)]
        public InfoCommand(string Alias) 
            : this() 
        {
            InfoConnection conn = new InfoConnection();
            conn.EEPAlias = Alias;
            this.Connection = conn;
        }

        [Obsolete("The recommended alternative is InfoCommand()", false)]
        public InfoCommand(ClientType type, string name)
            : this() { }

        [Obsolete("The recommended alternative is InfoCommand(object[])", false)]
        public InfoCommand(ClientType type, object[] clientInfo)
            :this()
        {
            ClientInfo = clientInfo;
        }

        [Obsolete("The recommended alternative is InfoCommand(object[])", false)]
        public InfoCommand(ConnectionType type, object[] clientInfo) 
            :this()
        {
            ClientInfo = clientInfo;
        }

        [Obsolete("The recommended alternative is InfoCommand(object[])", false)]
        public InfoCommand(string Alias, object[] clientInfo)
            : this()
        {
            InfoConnection conn = new InfoConnection();
            conn.EEPAlias = Alias;
            this.Connection = conn;
            ClientInfo = clientInfo;
        }

        [Obsolete("The recommended alternative is InfoCommand(object[])", false)]
        public InfoCommand(ClientType type, string name, object[] clientInfo) 
            :this()
        {
            ClientInfo = clientInfo;
        }

        [Obsolete("The recommended alternative is DealAllSqlText(string, string)", false)]
        public void DealAllSqlText(object[] ClientInfo, string strWhere)
        {
            DealAllSqlText(strWhere, string.Empty);
        }

        [Obsolete("The recommended alternative is DealAllSqlText(string, string)", false)]
        public void DealAllSqlText(object[] ClientInfo, string strWhere, string strOrder)
        {
            DealAllSqlText(strWhere, strOrder);
        }

        [Obsolete("The recommended alternative is DbConnectionSet.GetDbConn(string)", false)]
        public string FindConnectionString(string aliasName)
        {
            DbConnectionSet.DbConnection db = DbConnectionSet.GetDbConn(aliasName);
            if (db != null)
            {
                return db.ConnectionString;
            }
            else
            {
                return string.Empty;
            }
        }

        [Obsolete("The recommended alternative is DbConnectionSet.GetDbConn(string)", true)]
        public string FindDBType(string aliasName)
        {
            return string.Empty;
        }

        [Obsolete("The recommended alternative is DbConnectionSet.GetDbConn(string)", true)]
        public string FindSystemDBType(string aliasName)
        {
            return string.Empty;
        }

        [Obsolete("The recommended alternative is DBUtils.QuoteWords(string, ClientType)", true)]
        public string[] GetDataBaseQuote()
        {
            return null;
        }

        [Obsolete("The recommended alternative is DbConnectionSet.GetDbConn(string)", false)]
        public string[] GetDbConnectionStringAlias()
        {
            if (this.DesignMode)
            {
                return DbConnectionSet.GetAvaliableAlias();
            }
            else return null;
        }

        [Obsolete("The recommended alternative is EEPAlias", false)]
        public string GetEEPAlias()
        {
            return eEPAlias;
        }

        [Obsolete("The recommended alternative is this", false)]
        public IDbCommand GetInternalCommand()
        {
            return this.InternalDbCommand;
        }

        [Obsolete("The method has been abolished", true)]
        public void ReCreateInterCommand(ClientType ct) { }
        #endregion
    }

    public class KeyItems : InfoOwnerCollection
    {
        public KeyItems(Component aOwner, Type aItemType)
            : base(aOwner, typeof(KeyItem))
        {

        }

        new public KeyItem this[int index]
        {
            get
            {
                return (KeyItem)InnerList[index];
            }
            set
            {
                if (index > -1 && index < Count)
                    if (value is KeyItem)
                    {
                        //原来的Collection设置为0
                        ((KeyItem)InnerList[index]).Collection = null;
                        InnerList[index] = value;
                        //Collection设置为this
                        ((KeyItem)InnerList[index]).Collection = this;
                    }

            }
        }
    }

    public class KeyItem : InfoOwnerCollectionItem, IGetValues
    {
        private string fKeyName = "";
        
        [Editor(typeof(PropertyDropDownEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string KeyName
        {
            get
            {
                return fKeyName;
            }
            set
            {
                fKeyName = value;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override string Name
        {
            get
            {
                return KeyName;
            }

            set
            {
                KeyName = value;
            }
        }

        public string[] GetValues(string sKind)
        {
            if (sKind.Equals("KeyName"))
            {
                InfoCommand cmd = null;
                if (Owner is InfoCommand)
                {
                    cmd = (InfoCommand)Owner;
                    return cmd.GetFields();
                }
                else if (Owner is InfoIOTReceiver)
                {
                    InfoIOTReceiver iot = (InfoIOTReceiver)Owner;
                    UpdateComponent uc = iot.UpdateComp;
                    cmd = uc.SelectCmd;
                }
                return cmd.GetFields();
            }
            else
                return new string[] { };
        }

        //pending...
    }

    public delegate void SqlEventHandler(object sender, SqlEventArgs e);

    public class SqlEventArgs : EventArgs
    {
        public SqlEventArgs(string sql)
        {
            _sql = sql;
        }

        private string _sql;

        public string Sql
        {
            get { return _sql; }
            set { _sql = value; }
        }

        public DataSet DataSet { get; set; }
    }

    public class CommandTextEditor : System.Drawing.Design.UITypeEditor
    {
        public CommandTextEditor()
        {
        }

        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
        public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        // Displays the UI for value selection.
        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            // Uses the IWindowsFormsEditorService to display a
            // drop-down UI in the Properties window.
            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

            InfoCommand cmd = (InfoCommand)context.Instance;

            if (cmd.InfoConnection == null || cmd.InfoConnection == null)
            { MessageBox.Show("The InfoConnection property is empty."); return null; }

            CommandTextOptionDialog dialog = new CommandTextOptionDialog(cmd.InfoConnection.InternalConnection, cmd.CommandText);
            edSvc.ShowDialog(dialog);
            String commandText = dialog.CommandText;
            dialog.Dispose();

            return dialog.CommandText;
        }
    }
}