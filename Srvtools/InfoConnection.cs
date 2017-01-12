using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Windows.Forms.Design;
using System.Drawing.Design;
using Microsoft.Win32;
using System.Xml;
using System.Drawing;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Data.Odbc;
using System.Data.OracleClient;
using System.Data.Common;
using System.Reflection;
#if MySql
using MySql.Data.MySqlClient;
#endif

namespace Srvtools
{
    /// <summary>
    /// IDbConnection of EEP
    /// </summary>
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(UpdateComponent), "Resources.InfoConnection.ico")]
    public class InfoConnection : InfoBaseComp, IDbConnection, ISupportInitialize, IGetValues
    {
        private IContainer components;
        /// <summary>
        /// Initializes a new instance of the InfoConnection class.
        /// </summary>
        public InfoConnection()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the InfoConnection class.
        /// and adds the InfoConnection to the specified container.
        /// </summary>
        /// <param name="container">The System.ComponentModel.IContainer to add the current InfoConnection to.</param>
        public InfoConnection(System.ComponentModel.IContainer container)
            : this()
        {
            container.Add(this);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
        }

        #region IDbConnection Member
        /// <summary>
        ///  Gets or sets the string used to open a database.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(EEPAlias))
                {
                    return string.Empty;
                }
                return InternalConnection.ConnectionString;
            }
            set
            {
                throw new EEPException(EEPException.ExceptionType.MethodNotSupported, this.GetType(), null, "set_ConnectionString", null);
            }
        }

        /// <summary>
        /// Gets the time to wait while trying to establish a connection before terminating
        /// the attempt and generating an error.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int ConnectionTimeout
        {
            get
            {
                if (string.IsNullOrEmpty(EEPAlias))
                {
                    return 0;
                }
                return InternalConnection.ConnectionTimeout;
            }
        }

        /// <summary>
        /// Gets the name of the current database or the database to be used after a
        /// connection is opened.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Database
        {
            get
            {
                if (string.IsNullOrEmpty(EEPAlias))
                {
                    return string.Empty;
                }
                return InternalConnection.Database;
            }
        }

        /// <summary>
        /// Gets the current state of the connection.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ConnectionState State
        {
            get
            {
                if (string.IsNullOrEmpty(EEPAlias))
                {
                    return ConnectionState.Closed;
                }
                return InternalConnection.State;
            }
        }

        /// <summary>
        /// Begins a database transaction.
        /// </summary>
        /// <returns>An object representing the new transaction.</returns>
        public IDbTransaction BeginTransaction()
        {
            return InternalConnection.BeginTransaction();
        }

        /// <summary>
        /// Begins a database transaction with the specified System.Data.IsolationLevel
        /// value.
        /// </summary>
        /// <param name="il">One of the System.Data.IsolationLevel values.</param>
        /// <returns>An object representing the new transaction.</returns>
        public IDbTransaction BeginTransaction(IsolationLevel il)
        {
            return InternalConnection.BeginTransaction(il);
        }

        /// <summary>
        /// Changes the current database for an open Connection object.
        /// </summary>
        /// <param name="databaseName">The name of the database to use in place of the current database.</param>
        public void ChangeDatabase(string databaseName)
        {
            InternalConnection.ChangeDatabase(databaseName);
        }

        /// <summary>
        /// Closes the connection to the database.
        /// </summary>
        public void Close()
        {
            InternalConnection.Close();
        }

        /// <summary>
        /// Creates and returns a Command object associated with the connection.
        /// </summary>
        /// <returns>A Command object associated with the connection.</returns>
        public IDbCommand CreateCommand()
        {
            return InternalConnection.CreateCommand();
        }

        /// <summary>
        /// Opens a database connection with the settings specified by the ConnectionString
        /// property of the provider-specific Connection object.
        /// </summary>
        public void Open()
        {
            InternalConnection.Open();
        }

        #endregion

        #region ISupportInitialize Member
        /// <summary>
        /// Signals the object that initialization is starting.
        /// </summary>
        public void BeginInit() { }

        /// <summary>
        /// Signals the object that initialization is complete.
        /// </summary>
        public void EndInit() { }
        #endregion

        #region IGetValues Members
        /// <summary>
        /// Gets options of property.
        /// </summary>
        /// <param name="sKind">Name of property.</param>
        /// <returns>Options of property</returns>
        public string[] GetValues(string sKind)
        {
            return DbConnectionSet.GetAvaliableAlias();
        }

        #endregion

        private IDbConnection internalDbConnection;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        internal IDbConnection InternalConnection
        {
            get
            {
                if (internalDbConnection == null)
                {
                    if (string.IsNullOrEmpty(EEPAlias))
                    {
                        throw new EEPException(EEPException.ExceptionType.PropertyNull, this.GetType(), null, "EEPAlias", null);
                    }
                    else
                    {
                        DbConnectionSet.DbConnection db = DbConnectionSet.GetDbConn(EEPAlias);
                        if (db == null)
                        {
                            throw new EEPException(EEPException.ExceptionType.PropertyInvalid, this.GetType(), null, "EEPAlias", EEPAlias);
                        }
                        else
                        {
                            internalDbConnection = db.CreateConnection();
                        }
                    }
                }
                return internalDbConnection;
            }
        }

        /// <summary>
        /// Gets the database type of InfoConnection.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ClientType Type
        {
            get
            {
                if (string.IsNullOrEmpty(EEPAlias))
                {
                    return ClientType.ctNone;
                }
                return DBUtils.GetDatabaseType(InternalConnection);
            }
        }

        /// <summary>
        /// Gets the odbc database type of InfoConnection.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public OdbcDBType OdbcType
        {
            get
            {
                if (string.IsNullOrEmpty(EEPAlias))
                {
                    return OdbcDBType.None;
                }
                return DBUtils.GetOdbcDatabaseType(InternalConnection);
            }
        }

        /// <summary>
        /// Gets a string that represents the version of the server to which the object
        /// is connected.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public String ServerVersion
        {
            get { return string.Empty; }
        }

        /// <summary>
        /// Gets the name of the database server to which to connect.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string DataSource
        {
            get { return string.Empty; }
        }

        private string eEPAlias;
        /// <summary>
        /// Gets or sets the alias of System.Data.IDbConnection used by this instance of the InfoConnection.
        /// </summary>
        [Category("Infolight"),
        Description("The alias of System.Data.IDbConnection used by this instance of the InfoConnection")]
        [Editor(typeof(PropertyDropDownEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string EEPAlias
        {
            get
            {
                return eEPAlias;
            }
            set
            {
                eEPAlias = value;
                internalDbConnection = null;
            }
        }

        #region Obsolete Member
        [Obsolete("The recommended alternative is InfoConnection()", true)]
        public InfoConnection(ConnectionType connectionType) { }

        [Obsolete("The recommended alternative is InfoConnection()", true)]
        public InfoConnection(string connectionString, ConnectionType connectionType) { }

        [Browsable(false)]
        [Obsolete("The recommended alternative is Type", false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ConnectionType ConnectionType
        {
            get
            {
                if (string.IsNullOrEmpty(EEPAlias))
                {
                    return ConnectionType.SqlClient;
                }
                ClientType type = Type;
                switch (type)
                {
                    case ClientType.ctNone:
                    case ClientType.ctMsSql:
                        return Srvtools.ConnectionType.SqlClient;
                    case ClientType.ctOleDB:
                        return Srvtools.ConnectionType.OleDb;
                    case ClientType.ctOracle:
                        return Srvtools.ConnectionType.OracleClient;
                    case ClientType.ctODBC:
                        return Srvtools.ConnectionType.Odbc;
                    case ClientType.ctMySql:
                        return Srvtools.ConnectionType.MySqlClient;
                    case ClientType.ctInformix:
                        return Srvtools.ConnectionType.IfxClient;
                    case ClientType.ctSybase:
                        return Srvtools.ConnectionType.Sybase;
                }
                return ConnectionType.SqlClient;
            }
            set { }
        }

        [Browsable(false)]
        [Obsolete("The recommended alternative is this", false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DbConnection InternalDbConnection
        {
            get
            {
                if (string.IsNullOrEmpty(EEPAlias))
                {
                    return null;
                }
                return (DbConnection)this.InternalConnection;
            }
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

        [Obsolete("The recommended alternative is DbConnectionSet.GetDbConn(string)", false)]
        public string[] GetDbConnectionStringAlias()
        {
            return DbConnectionSet.GetAvaliableAlias();
        }

        [Obsolete("The recommended alternative is EEPAlias", true)]
        public void SetConnectionString() { }
        #endregion

    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public enum ConnectionType
    {
        SqlClient = 0,

        OleDb = 1,

        Odbc = 2,

        OracleClient = 3,

        MySqlClient = 4,

        IfxClient = 5,

        Sybase = 6
    }
}