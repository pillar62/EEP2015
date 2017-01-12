using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;
using System.IO;

namespace EFWCFModule.EEPAdapter
{
    /// <summary>
    /// Item of eep
    /// </summary>
    public class EEPItem
    {
        /// <summary>
        /// Srvtools
        /// </summary>
        public const string SRVTOOLS_ASSEMBLY = "Srvtools";
        /// <summary>
        /// InfoRemoteModule
        /// </summary>
        public const string INFOREMOTEMODULE_ASSEMBLY = "InfoRemoteModule";
        /// <summary>
        /// FLRuntime
        /// </summary>
        public const string FLRUNTIME_ASSEMBLY = "FLRuntime";
        /// <summary>
        /// FLTools
        /// </summary>
        public const string FLTOOLS_ASSEMBLY = "FLTools";

        /// <summary>
        /// Srvtools.DbConnectionSet
        /// </summary>
        public const string DBCONNECTIONSET_TYPE = "Srvtools.DbConnectionSet";
        /// <summary>
        /// Srvtools.Encrypts
        /// </summary>
        public const string ENCRYPT_TYPE = "Srvtools.Encrypt";
        /// <summary>
        /// SrvGL
        /// </summary>
        public const string SRVGL_TYPE = "SrvGL";
        /// <summary>
        /// SysEEPLogService
        /// </summary>
        public const string SYSEEPLOGSERVICE_TYPE = "SysEEPLogService";
        /// <summary>
        /// SysEEPLog
        /// </summary>
        public const string SYSEEPLOG_TYPE = "SysEEPLog";

        /// <summary>
        /// FLRuntime.InstanceManager
        /// </summary>
        public const string INSTANCEMANAGER_TYPE = "FLRuntime.InstanceManager";

        /// <summary>
        /// FLTools.GloFix
        /// </summary>
        public const string GLOFIX_TYPE = "FLTools.GloFix";

        /// <summary>
        /// Creates a new instance of eep item
        /// </summary>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="typeName">Name of type</param>
        public EEPItem(string assemblyName, string typeName)
        {
            if (string.IsNullOrEmpty(assemblyName))
            {
                throw new ArgumentNullException("assemblyName");
            }
            if (string.IsNullOrEmpty(typeName))
            {
                throw new ArgumentNullException("typeName");
            }
            _itemType = GetType(assemblyName, typeName);
        }

        /// <summary>
        /// Creates a new instance of eep item
        /// </summary>
        /// <param name="instance">Instance of item</param>
        public EEPItem(object instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }
            _itemInstance = instance;
        }

        /// <summary>
        /// Creates a new instance of eep item
        /// </summary>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="typeName">Name of type</param>
        /// <param name="parameters">Parameters</param>
        public EEPItem(string assemblyName, string typeName, object[] parameters)
        {
            if (string.IsNullOrEmpty(assemblyName))
            {
                throw new ArgumentNullException("assemblyName");
            }
            if (string.IsNullOrEmpty(typeName))
            {
                throw new ArgumentNullException("typeName");
            }
            var type = GetType(assemblyName, typeName);
            var constructor = type.GetConstructors(BindingFlags.Public | BindingFlags.Instance).FirstOrDefault();
            if (constructor == null)
            {
                throw new MissingMemberException(typeName, typeName);
            }
            _itemInstance = constructor.Invoke(parameters);
        }

        /// <summary>
        /// Gets specialfied type
        /// </summary>
        /// <param name="assemblyName">Name of assembly</param>
        /// <param name="typeName">Name of type</param>
        /// <returns>Type</returns>
        private Type GetType(string assemblyName, string typeName)
        {
            if (string.IsNullOrEmpty(assemblyName))
            {
                throw new ArgumentNullException("assemblyName");
            }
            if (string.IsNullOrEmpty(typeName))
            {
                throw new ArgumentNullException("typeName");
            }
            var serverAssembly = Assembly.GetEntryAssembly();
            if (serverAssembly == null)
            {
                throw new NotSupportedException("Design time not support.");
            }
            var referenceName = serverAssembly.GetReferencedAssemblies().FirstOrDefault(c => c.Name.Equals(assemblyName));
            if (referenceName == null)
            {
                var dir = Path.GetDirectoryName(serverAssembly.Location);
                var assemblyFile = Path.Combine(dir, string.Format("{0}.dll", assemblyName));
                if (File.Exists(assemblyFile))
                {
                    var assembly = Assembly.LoadFrom(assemblyFile);
                    return assembly.GetType(typeName, true);
                }
                else
                {
                    throw new ObjectNotFoundException(string.Format("Assembly:{0} not found.", assemblyName));
                }
            }
            else
            {
                var assembly = Assembly.Load(referenceName);
                return assembly.GetType(typeName, true);
            }
        }

        private Type _itemType;
        /// <summary>
        /// Gets type of item
        /// </summary>
        public Type ItemType
        {
            get
            {
                if (ItemInstance != null)
                {
                    return ItemInstance.GetType();
                }
                else
                {
                    return _itemType;
                }
            }
        }

        private object _itemInstance;
        /// <summary>
        /// Gets instance of item
        /// </summary>
        public object ItemInstance
        {
            get
            {
                return _itemInstance;
            }
        }

        /// <summary>
        /// Gets property value
        /// </summary>
        /// <param name="propertyName">Name of property</param>
        /// <returns>Value of property</returns>
        public object GetValue(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentNullException("propertyName");
            }
            var property = GetProperty(propertyName);
            return property.GetValue(ItemInstance, null);
        }

        /// <summary>
        /// Sets property value
        /// </summary>
        /// <param name="propertyName">Name of property</param>
        /// <param name="value">Value of property</param>
        public void SetValue(string propertyName, object value)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentNullException("propertyName");
            }
            var property = GetProperty(propertyName);
            property.SetValue(ItemInstance, value, null);
        }

        /// <summary>
        /// Executes method
        /// </summary>
        /// <param name="methodName">Name of method</param>
        /// <param name="parameters">Parameters</param>
        /// <returns>Object returned by method</returns>
        public object Execute(string methodName, object[] parameters)
        {
            if (string.IsNullOrEmpty(methodName))
            {
                throw new ArgumentNullException("methodName");
            }
            var method = GetMethod(methodName);
            return method.Invoke(ItemInstance, parameters);
        }

        /// <summary>
        /// Executes method
        /// </summary>
        /// <param name="methodName">Name of method</param>
        /// <param name="parameters">Parameters</param>
        /// <returns>Object returned by method</returns>
        public object Execute(string methodName, object[] parameters, Type[] types)
        {
            if (string.IsNullOrEmpty(methodName))
            {
                throw new ArgumentNullException("methodName");
            }
            var method = GetMethod(methodName, types);
            return method.Invoke(ItemInstance, parameters);
        }

        /// <summary>
        /// Executes method
        /// </summary>
        /// <param name="methodName">Name of method</param>
        /// <param name="parameters">Parameters</param>
        /// <returns>Object returned by method</returns>
        public object Execute(string methodName, ref object[] parameters)
        {
            if (string.IsNullOrEmpty(methodName))
            {
                throw new ArgumentNullException("methodName");
            }
            var method = GetMethod(methodName);
            return method.Invoke(ItemInstance, parameters);
        }

        /// <summary>
        /// Gets information of property
        /// </summary>
        /// <param name="propertyName">Name of property</param>
        /// <returns>Information of property</returns>
        private PropertyInfo GetProperty(string propertyName)
        {
            var type = ItemType;
            if (type == null)
            {
                throw new ArgumentNullException("ItemType");
            }
            var property = type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            if (property == null)
            {
                throw new MissingMemberException(type.Name, propertyName);
            }
            return property;
        }

        /// <summary>
        /// Gets information of method
        /// </summary>
        /// <param name="methodName">Name of method</param>
        /// <returns>Information of method</returns>
        private MethodInfo GetMethod(string methodName)
        {
            if (string.IsNullOrEmpty(methodName))
            {
                throw new ArgumentNullException("methodName");
            }
            var type = ItemType;
            if (type == null)
            {
                throw new ArgumentNullException("ItemType");
            }
            var method = type.GetMethod(methodName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
            if (method == null)
            {
                throw new MissingMemberException(type.Name, methodName);
            }
            return method;
        }

        /// <summary>
        /// Gets information of method
        /// </summary>
        /// <param name="methodName">Name of method</param>
        /// <returns>Information of method</returns>
        private MethodInfo GetMethod(string methodName, Type[] types)
        {
            if (string.IsNullOrEmpty(methodName))
            {
                throw new ArgumentNullException("methodName");
            }
            var type = ItemType;
            if (type == null)
            {
                throw new ArgumentNullException("ItemType");
            }
            var method = type.GetMethod(methodName, types);
            if (method == null)
            {
                throw new MissingMemberException(type.Name, methodName);
            }
            return method;
        }
    }

    /// <summary>
    /// Encrypt item
    /// </summary>
    public static class Encrypt
    {
        private static EEPItem Item = new EEPItem(EEPItem.SRVTOOLS_ASSEMBLY, EEPItem.ENCRYPT_TYPE);

        /// <summary>
        /// Encrypts password
        /// </summary>
        /// <param name="UserID">UserID</param>
        /// <param name="Password">Password</param>
        /// <param name="PasswordLen">PasswordLen</param>
        /// <param name="RetPassword">RetPassword</param>
        /// <param name="bCheckValidate">bCheckValidate</param>
        /// <returns>Boolean</returns>
        public static bool EncryptPassword(string UserID, string Password, int PasswordLen, ref char[] RetPassword, bool bCheckValidate)
        {
            var parameters = new object[] { UserID, Password, PasswordLen, RetPassword, bCheckValidate };
            var returnObject = (bool)Item.Execute("EncryptPassword", ref parameters);
            RetPassword = (char[])parameters[3];
            return returnObject;
        }
    }

    /// <summary>
    /// DbConnectionSet item
    /// </summary>
    public static class DbConnectionSet
    {
        private static EEPItem Item = new EEPItem(EEPItem.SRVTOOLS_ASSEMBLY, EEPItem.DBCONNECTIONSET_TYPE);

        /// <summary>
        /// Gets system database
        /// </summary>
        public static string SystemDatabase
        {
            get 
            {
                return (string)Item.GetValue("SystemDatabase");
            }
        }

        /// <summary>
        /// Gets avaliable alias
        /// </summary>
        /// <returns>Avaliable alias</returns>
        public static string[] GetAvaliableAlias()
        {
            return (string[])Item.Execute("GetAvaliableAlias", null);
        }

        /// <summary>
        /// Gets DbConnection
        /// </summary>
        /// <param name="dataBaseName">Name of database</param>
        /// <returns>DbConnection</returns>
        public static DbConnection GetDbConn(string dataBaseName)
        {
            var obj = Item.Execute("GetDbConnForEF", new object[] { dataBaseName });
            if (obj == null)
            {
                throw new ObjectNotFoundException(string.Format("Database:{0} not found.", dataBaseName));
            }
            return new DbConnection(obj);
        }
       
    }

    /// <summary>
    /// DbConnection item
    /// </summary>
    public class DbConnection: EEPItem
    {
        /// <summary>
        /// Creates a new instance of DbConnection
        /// </summary>
        /// <param name="instance"></param>
        public DbConnection(object instance) : base(instance) { }

        /// <summary>
        /// Gets or sets connection string
        /// </summary>
        public string ConnectionString
        {
            get
            {
                return (string)GetValue("ConnectionString");
            }
            set
            {
                SetValue("ConnectionString", value);
            }
        }

        /// <summary>
        /// Gets or sets name of database
        /// </summary>
        public string DbName
        {
            get
            {
                return (string)GetValue("DbName");
            }
            set
            {
                SetValue("DbName", value);
            }
        }

        /// <summary>
        /// Gets or sets max count of database
        /// </summary>
        public int MaxCount
        {
            get
            {
                return (int)GetValue("MaxCount");
            }
            set
            {
                SetValue("MaxCount", value);
            }
        }

        /// <summary>
        /// Gets or sets timeout of database
        /// </summary>
        public int TimeOut
        {
            get
            {
                return (int)GetValue("TimeOut");
            }
            set
            {
                SetValue("TimeOut", value);
            }
        }

        /// <summary>
        /// Gets type of database
        /// </summary>
        public DataBaseType DbType
        {
            get
            {
                return (DataBaseType)GetValue("DbType");
            }
            set
            {
                SetValue("DbType", value);
            }
        }
    }

    /// <summary>
    /// SrvGL item
    /// </summary>
    public static class SrvGL
    {
        private static EEPItem Item = new EEPItem(EEPItem.INFOREMOTEMODULE_ASSEMBLY, EEPItem.SRVGL_TYPE);

        /// <summary>
        /// Logon and logoff
        /// </summary>
        /// <param name="userid">ID of user</param>
        /// <param name="username">Name of user</param>
        /// <param name="computer">Name of computer</param>
        /// <param name="logcount">Count of logon</param>
        public static void LogUser(string userid, string username, string computer, int logcount)
        {
            Item.Execute("LogUser", new object[] { userid, username, computer, logcount });
        }


        ///// <summary>
        ///// Gets whether user is logoned
        ///// </summary>
        ///// <param name="userid">ID of user</param>
        ///// <returns>Whether user is logoned</returns>
        //public static bool isUserLogined(string userid)
        //{
        //    return (bool)Item.Execute("isUserLogined", new object[] { userid });
        //}

        /// <summary>
        /// Gets whether user is logoned
        /// </summary>
        /// <param name="userid">ID of user</param>
        /// <returns>Whether user is logoned</returns>
        public static bool IsUserLogined(string userid)
        {
            return (bool)Item.Execute("IsUserLogined", new object[] { userid });
        }
    }

    /// <summary>
    /// SysEEPLogService item
    /// </summary>
    public static class SysEEPLogService
    {
        private static EEPItem Item = new EEPItem(EEPItem.INFOREMOTEMODULE_ASSEMBLY, EEPItem.SYSEEPLOGSERVICE_TYPE);

        /// <summary>
        /// Gets or sets whether log is enabled
        /// </summary>
        public static bool Enable
        {
            get
            {
                return (bool)Item.GetValue("Enable");
            }
            set
            {
                Item.SetValue("Enable", value);
            }
        }

        /// <summary>
        /// Gets or sets whether log to file
        /// </summary>
        public static bool LogToFile
        {
            get
            {
                return (bool)Item.GetValue("LogToFile");
            }
            set
            {
                Item.SetValue("LogToFile", value);
            }
        }

        /// <summary>
        /// Gets or sets whether log sql
        /// </summary>
        public static bool LogSql
        {
            get
            {
                return (bool)Item.GetValue("LogSql");
            }
            set
            {
                Item.SetValue("LogSql", value);
            }
        }

        /// <summary>
        /// Gets or sets maximun count of record in database
        /// </summary>
        public static int MaxRecord
        {
            get
            {
                return (int)Item.GetValue("MaxRecord");
            }
            set
            {
                Item.SetValue("MaxRecord", value);
            }
        }

        /// <summary>
        /// Gets or sets maximun days of file
        /// </summary>
        public static int MaxLogDays
        {
            get
            {
                return (int)Item.GetValue("MaxLogDays");
            }
            set
            {
                Item.SetValue("MaxLogDays", value);
            }
        }

        /// <summary>
        /// Gets or sets maximun size of log file
        /// </summary>
        public static int MaxSize
        {
            get
            {
                return (int)Item.GetValue("MaxSize");
            }
            set
            {
                Item.SetValue("MaxSize", value);
            }
        }

        /// <summary>
        /// Gets directory of log file
        /// </summary>
        public static string LogFileDir
        {
            get
            {
                return (string)Item.GetValue("LogFileDir");
            }
        }

        /// <summary>
        /// Gets or sets enabled logs
        /// </summary>
        public static int EnableLogs
        {
            get
            {
                return (int)Item.GetValue("EnableLogs");
            }
            set
            {
                Item.SetValue("EnableLogs", value);
            }
        }

        /// <summary>
        /// Gets or sets interval of warning
        /// </summary>
        public static int WarningInterval
        {
            get
            {
                return (int)Item.GetValue("WarningInterval");
            }
            set
            {
                Item.SetValue("WarningInterval", value);
            }
        }
    }

    /// <summary>
    /// SysEEPLog item
    /// </summary>
    public class SysEEPLog : EEPItem
    {
        /// <summary>
        /// Creates a new instance of SysEEPLog
        /// </summary>
        /// <param name="clientinfo">Information of client</param>
        /// <param name="style">Log style</param>
        /// <param name="status">Log status</param>
        /// <param name="time">Datetime</param>
        /// <param name="title">Title of log</param>
        /// <param name="description">Description of log</param>
        public SysEEPLog(object[] clientinfo, LogStyle style, LogStatus status, DateTime time, string title, string description)
            : base(INFOREMOTEMODULE_ASSEMBLY, SYSEEPLOG_TYPE, new object[] { clientinfo, style, status, time, title, description })
        { }

        /// <summary>
        /// Gets style of log
        /// </summary>
        public LogStyle LogStyle
        {
            get
            {
                return (LogStyle)GetValue("LogStyle");
            }
        }

        /// <summary>
        /// Gets status of log
        /// </summary>
        public LogStatus LogType
        {
            get
            {
                return (LogStatus)GetValue("LogType");
            }
        }

        /// <summary>
        /// Gets datetime
        /// </summary>
        public DateTime LogDateTime
        {
            get
            {
                return (DateTime)GetValue("LogDateTime");
            }
        }
        
        /// <summary>
        /// Gets ID of user
        /// </summary>
        public string UserID
        {
            get
            {
                return (string)GetValue("UserID");
            }
        }

        /// <summary>
        /// Gets name of database
        /// </summary>
        public string DomainID
        {
            get
            {
                return (string)GetValue("DomainID");
            }
        }

        /// <summary>
        /// Gets title of log
        /// </summary>
        public string Title
        {
            get
            {
                return (string)GetValue("Title");
            }
        }

        /// <summary>
        /// Gets description of log
        /// </summary>
        public string Description
        {
            get
            {
                return (string)GetValue("Description");
            }
        }

        /// <summary>
        /// Gets ip of computer
        /// </summary>
        public string ComputerIP
        {
            get
            {
                return (string)GetValue("ComputerName");
            }
        }

        /// <summary>
        /// Gets name of computer
        /// </summary>
        public string ComputerName
        {
            get
            {
                return (string)GetValue("ComputerName");
            }
        }

        /// <summary>
        /// Gets or sets time span of execution
        /// </summary>
        public double ExecutionTime
        {
            get
            {
                return (double)GetValue("ExecutionTime");
            }
            set
            {
                SetValue("ExecutionTime", value);
            }
        }

        /// <summary>
        /// Gets or sets type of log sql
        /// </summary>
        public int SqlLogType
        {
            get
            {
                return (int)GetValue("SqlLogType");
            }
            set
            {
                SetValue("SqlLogType", value);
            }
        }

        /// <summary>
        /// Gets or sets sentence of sql
        /// </summary>
        public string SqlSentence
        {
            get
            {
                return (string)GetValue("SqlSentence");
            }
            set
            {
                SetValue("SqlSentence", value);
            }
        }

        /// <summary>
        /// Gets or sets whether only log sql sentence
        /// </summary>
        public bool OnlyLogSqlSentence
        {
            get 
            {
                return (bool)GetValue("OnlyLogSqlSentence"); 
            }
            set 
            {
                SetValue("OnlyLogSqlSentence", value);
            }
        }

        /// <summary>
        /// Gets whether log is enabled
        /// </summary>
        public bool Enable
        {
            get
            {
                return (bool)GetValue("Enable");
            }
        }

        /// <summary>
        /// Log
        /// </summary>
        public void Log()
        {
            Execute("Log", null);
        }
    }

    /// <summary>
    /// Type of database(renamed from ClientType)
    /// </summary>
    public enum DataBaseType
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0,
        /// <summary>
        /// MsSql
        /// </summary>
        MsSql = 1,
        /// <summary>
        /// OleDB
        /// </summary>
        OleDB = 2,
        /// <summary>
        /// Oracle
        /// </summary>
        Oracle = 3,
        /// <summary>
        /// ODBC
        /// </summary>
        ODBC = 4,
        /// <summary>
        /// MySql
        /// </summary>
        MySql = 5,
        /// <summary>
        /// Informix
        /// </summary>
        Informix = 6,
        /// <summary>
        /// Sybase
        /// </summary>
        Sybase = 7
    }

    /// <summary>
    /// Style of log(renamed from LogStyleType)
    /// </summary>
    public enum LogStyle
    {
        /// <summary>
        /// System
        /// </summary>
        System = 0,
        /// <summary>
        /// Provider
        /// </summary>
        Provider = 1,
        /// <summary>
        /// Call Method
        /// </summary>
        CallMethod = 2,
        /// <summary>
        /// User Define
        /// </summary>
        UserDefine = 3,
        /// <summary>
        /// Email
        /// </summary>
        Email = 4
    }

    /// <summary>
    /// Status of log(renamed from LogTypeType)
    /// </summary>
    public enum LogStatus
    {
        /// <summary>
        /// Normal
        /// </summary>
        Normal = 0,
        /// <summary>
        /// Warning
        /// </summary>
        Warning = 1,
        /// <summary>
        /// Error
        /// </summary>
        Error = 2,
        /// <summary>
        /// Unkown
        /// </summary>
        Unknown = 3,
    }

    /// <summary>
    /// Type of system language
    /// </summary>
    public enum SYS_LANGUAGE
    {
        /// <summary>
        /// English
        /// </summary>
        ENG,
        /// <summary>
        /// Traditional Chinese(zh-tw)
        /// </summary>
        TRA,
        /// <summary>
        /// Simplified Chinese
        /// </summary>
        SIM,
        /// <summary>
        /// Traditional Chinese(hk)
        /// </summary>
        HKG,
        /// <summary>
        /// Japanese
        /// </summary>
        JPN,
        /// <summary>
        /// Language 1
        /// </summary>
        LAN1,
        /// <summary>
        /// Language 2
        /// </summary>
        LAN2,
        /// <summary>
        /// Language 3
        /// </summary>
        LAN3
    }

    /// <summary>
    /// Flow instance manager
    /// </summary>
    public class InstanceManager: EEPItem
    {
        /// <summary>
        /// Creates a new instance of InstanceManager
        /// </summary>
        public InstanceManager() : base(EEPItem.FLRUNTIME_ASSEMBLY, EEPItem.INSTANCEMANAGER_TYPE, null) 
        { }

        /// <summary>
        /// Submit
        /// </summary>
        /// <param name="parameters">Flow patameter</param>
        /// <param name="clientInfo">Information of client</param>
        /// <returns>Result of submit</returns>
        public object[] Submit(object[] parameters, object[] clientInfo)
        {
            return (object[])Execute("Submit", new object[] { parameters , clientInfo});
        }

        public object[] Approve(object[] parameters, object[] clientInfo)
        {
            return (object[])Execute("Approve", new object[] { parameters, clientInfo });
        }

        public object[] Return(object[] parameters, object[] clientInfo)
        {
            return (object[])Execute("Return", new object[] { parameters, clientInfo });
        }

        public object[] Return2(object[] parameters, object[] clientInfo)
        {
            return (object[])Execute("Return2", new object[] { parameters, clientInfo });
        }

        public object[] Retake(object[] parameters, object[] clientInfo)
        {
            return (object[])Execute("Retake", new object[] { parameters, clientInfo });
        }

        public object[] Reject(object[] parameters, object[] clientInfo)
        {
            return (object[])Execute("Reject", new object[] { parameters, clientInfo });
        }

        public object[] PlusApprove(object[] parameters, object[] clientInfo)
        {
            return (object[])Execute("PlusApprove", new object[] { parameters, clientInfo });
        }

        public object[] PlusReturn(object[] parameters, object[] clientInfo)
        {
            return (object[])Execute("PlusReturn", new object[] { parameters, clientInfo });
        }

        public object[] Pause(object[] parameters, object[] clientInfo)
        {
            return (object[])Execute("Pause", new object[] { parameters, clientInfo });
        }

        public object[] Notify(object[] parameters, object[] clientInfo)
        {
            return (object[])Execute("Notify", new object[] { parameters, clientInfo });
        }

        public object[] DeleteNotify(object[] parameters, object[] clientInfo)
        {
            return (object[])Execute("DeleteNotify", new object[] { parameters, clientInfo });
        }

        public object[] Preview(object[] parameters, object[] clientInfo)
        {
            return (object[])Execute("Preview", new object[] { parameters, clientInfo });
        }

        public object[] GetFLPathList(object[] parameters, object[] clientInfo)
        {
            return (object[])Execute("GetFLPathList", new object[] { parameters, clientInfo });
        }
    }

    /// <summary>
    /// FLTools GloFix
    /// </summary>
    public static class GloFix
    {
        private static EEPItem Item = new EEPItem(EEPItem.FLTOOLS_ASSEMBLY, EEPItem.GLOFIX_TYPE);

        /// <summary>
        /// ShowMessage
        /// </summary>
        /// <param name="string">sendToIds</param>
        /// <param name="bool">isWeb</param>
        /// <param name="clientInfo">Information of client</param>
        /// <returns>Result of submit</returns>
        public static string ShowMessage(string sendToIds, bool isWeb, object[] clientInfo)
        {
            var parameters = new object[] { sendToIds, isWeb, clientInfo };
            var types = new Type[] { sendToIds.GetType(), isWeb.GetType(), clientInfo.GetType() };
            var returnObject = (string)Item.Execute("ShowMessage", parameters, types);
            return returnObject;
        }

        /// <summary>
        /// ShowParallelMessage
        /// </summary>
        /// <param name="string">sendToIds</param>
        /// <param name="clientInfo">Information of client</param>
        /// <returns>Result of Parallel</returns>
        public static string ShowParallelMessage(string sendToIds, object[] clientInfo)
        {
            var parameters = new object[] { sendToIds, clientInfo };
            var types = new Type[] { sendToIds.GetType(), clientInfo.GetType() };
            var returnObject = (string)Item.Execute("ShowParallelMessage", parameters, types);
            return returnObject;
        }

        /// <summary>
        /// ShowNotifyMessage
        /// </summary>
        /// <param name="string">sendToIds</param>
        /// <param name="clientInfo">Information of client</param>
        /// <returns>Result of Notify</returns>
        public static string ShowNotifyMessage(string sendToIds, object[] clientInfo)
        {
            var parameters = new object[] { sendToIds, clientInfo };
            var types = new Type[] { sendToIds.GetType(), clientInfo.GetType() };
            var returnObject = (string)Item.Execute("ShowNotifyMessage", parameters, types);
            return returnObject;
        }

        /// <summary>
        /// ShowPlusMessage
        /// </summary>
        /// <param name="string">sendToIds</param>
        /// <param name="clientInfo">Information of client</param>
        /// <returns>Result of PlusMessage</returns>
        public static string ShowPlusMessage(string sendToIds, object[] clientInfo)
        {
            var parameters = new object[] { sendToIds, clientInfo };
            var types = new Type[] { sendToIds.GetType(), clientInfo.GetType() };
            var returnObject = (string)Item.Execute("ShowPlusMessage", parameters, types);
            return returnObject;
        }
    }
}
