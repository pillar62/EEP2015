using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data;
using System.Xml;
using System.IO;
using System.Timers;
using System.Reflection;

using System.Data.SqlClient;
using System.Data.OleDb;
using System.Data.OracleClient;
using System.Data.Odbc;

namespace Srvtools
{
    public static class DbConnectionSet
    {
        private static Hashtable DbConnections = new Hashtable();

        //USE FOR DESIGN!!!
        public static string[] GetAvaliableAlias()
        {
            return GetAvaliableAlias(null);
        }


        public static string[] GetAvaliableAlias(string developerID)
        {
            //string dataBaseConfigFile = string.IsNullOrEmpty(developerID) ? SystemFile.DBFile : GetDataBaseConfigFilePath(developerID);
            //if (File.Exists(dataBaseConfigFile))
            if (string.IsNullOrEmpty(developerID))
            {
                if (File.Exists(SystemFile.DBFile))
                {
                    XmlDocument xml = new XmlDocument();
                    xml.Load(SystemFile.DBFile);
                    XmlNode node = xml.SelectSingleNode("InfolightDB/DataBase");
                    if (node != null)
                    {
                        List<string> listAlias = new List<string>();
                        foreach (XmlNode nodeChild in node.ChildNodes)
                        {
                            listAlias.Add(nodeChild.Name);
                        }
                        return listAlias.ToArray();
                    }
                }
            }
            else
            {
                using (var connection = DbConnectionSet.GetDbConn(SystemDatabase).CreateConnection())
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = string.Format("SELECT AliasName FROM SYS_SDALIAS WHERE USERID = '{0}'", developerID);
                    var reader = command.ExecuteReader();
                    List<string> listAlias = new List<string>();
                    while (reader.Read())
                    {
                        listAlias.Add(reader["AliasName"].ToString());
                    }
                    return listAlias.ToArray();
                }
            }
            return new string[] { };
        }

        private static string GetPwdString(string s)
        {
            string sRet = "";
            for (int i = 0; i < s.Length; i++)
            {
                sRet = sRet + (char)(((int)(s[s.Length - 1 - i])) ^ s.Length);
            }
            return sRet;
        }

        private static string systemDatabase;

        public static string SystemDatabase
        {
            get
            {
                if (string.IsNullOrEmpty(systemDatabase))
                {
                    systemDatabase = GetSystemDatabase(null);
                }
                return systemDatabase;
            }
        }

        //private static string GetDataBaseConfigFilePath(string developerID)
        //{
        //    return string.Format("{0}\\SDModule\\{1}\\DB.xml", EEPRegistry.Server, developerID);
        //}

        public static string GetSystemDatabase(string developerID)
        {
            //string dataBaseConfigFile = string.IsNullOrEmpty(developerID) ? SystemFile.DBFile : GetDataBaseConfigFilePath(developerID);
            //return GetSystemDatabaseInternal(dataBaseConfigFile);
            if (string.IsNullOrEmpty(developerID))
            {
                return GetSystemDatabaseInternal(SystemFile.DBFile);
            }
            else
            {
                using (var connection = DbConnectionSet.GetDbConn(SystemDatabase).CreateConnection())
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = string.Format("SELECT SystemAlias FROM SYS_SDALIAS WHERE USERID = '{0}'", developerID);
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        return reader["SystemAlias"].ToString();
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        public static string GetSystemDatabaseInternal(string dataBaseConfigFile)
        {
            if (File.Exists(dataBaseConfigFile))
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(dataBaseConfigFile);
                XmlNode node = xml.SelectSingleNode("InfolightDB/SystemDB");
                if (node != null)
                {
                    return node.InnerText;
                }
            }
            return null;

        }

        //1. MSSql   2.OleDb   3.Oracle   4.ODBC   5.MySql   6.Informix
        public static List<string> GetDataBaseType(string dbname)
        {
            List<string> dbType = new List<string>();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(SystemFile.DBFile);

            XmlNode node = xmlDoc.FirstChild.FirstChild.SelectSingleNode(dbname);
            if (node == null)
            {
                throw new Exception("SystemDB is not exist!");
            }
            string DBType = node.Attributes["Type"].Value.Trim();
            string OdbcType = (node.Attributes["OdbcType"] == null ? "" : node.Attributes["OdbcType"].Value.Trim());
            switch (DBType)
            {
                case "1": dbType.Add("MSSql"); break;
                case "2": dbType.Add("OleDb"); break;
                case "3": dbType.Add("Oracle"); break;
                case "4": dbType.Add("ODBC"); break;
                case "5": dbType.Add("MySql"); break;
                case "6": dbType.Add("Informix"); break;
                case "7": dbType.Add("Sybase"); break;
                default: dbType.Add("MSSql"); break;
            }
            switch (OdbcType)
            {
                case "1": dbType.Add("Informix"); break;
                case "2": dbType.Add("FoxPro"); break;
                case "3": dbType.Add("DB2"); break;
                default: dbType.Add("-1"); break;
            }
            return dbType;
        }


        //public static IDbConnection WaitForConnection(string dbName, string user, string module, DateTime time)
        //{
        //    ClientType ct = ClientType.ctMsSql;
        //    return WaitForConnection(dbName, ref ct, user, module, time);
        //}

        //public static IDbConnection WaitForConnection(string dbName, ref ClientType ct, string user, string module, DateTime time)
        //{
        //    DbConnection dbConn = GetDbConn(dbName);

        //    if (dbConn != null)
        //    {
        //        ct = dbConn.DbType;
        //        return dbConn.WaitOne(user, module, time);
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        public static IDbConnection WaitForConnection(string dbName, string developerID, string user, string module, DateTime time)
        {
            ClientType ct = ClientType.ctMsSql;
            return WaitForConnection(dbName, developerID, ref ct, user, module, time);
        }

        public static IDbConnection WaitForConnection(string dbName, string developerID, ref ClientType ct, string user, string module, DateTime time)
        {
            return WaitForConnection(dbName, developerID, ref ct, user, module, null, time);
        }

        public static IDbConnection WaitForConnection(string dbName, string developerID, ref ClientType ct, string user, string module, string cacheCommandName, DateTime time)
        {
            DbConnection dbConn = GetDbConn(dbName, developerID);

            if (dbConn != null)
            {
                ct = dbConn.DbType;
                return dbConn.WaitOne(user, module, cacheCommandName, time);
            }
            else
            {
                return null;
            }
        }

        //public static void ReleaseConnection(string dbName, IDbConnection conn)
        //{
        //    DbConnection dbConn = GetDbConn(dbName);

        //    if (dbConn != null)
        //    {
        //        dbConn.Release(conn);
        //    }
        //}

        public static IDbTransaction GetTransaction(string dbName, string developerID, string user, string module, string commandName)
        {
            DbConnection dbConn = GetDbConn(dbName, developerID);

            if (dbConn != null)
            {
                return dbConn.GetTransaction(user, module, commandName);
            }
            return null;
        }

        public static void SetTransaction(string dbName, string developerID, string user, string module, string commandName, IDbTransaction transaction)
        {
            DbConnection dbConn = GetDbConn(dbName, developerID);

            if (dbConn != null)
            {
                dbConn.SetTransaction(user, module, commandName, transaction);
            }
        }


        public static void ReleaseConnection(string dbName, string developerID, IDbConnection conn)
        {
            DbConnection dbConn = GetDbConn(dbName, developerID);

            if (dbConn != null)
            {
                dbConn.Release(conn);
            }
        }


        public static void ReleaseConnection(string dbName, string developerID, string user)
        {
            DbConnection dbConn = GetDbConn(dbName, developerID);

            if (dbConn != null)
            {
                dbConn.Release(user);
            }
        }


        public static void ReleaseConnection(string dbName, string developerID, string user, string module, string commandName)
        {
            DbConnection dbConn = GetDbConn(dbName, developerID);

            if (dbConn != null)
            {
                dbConn.Release(user, module, commandName);
            }
        }

        public static DbConnection GetDbConn(string dataBaseName)
        {
            return GetDbConn(dataBaseName, null);
        }

        public static DbConnection GetDbConn(string dataBaseName, string developerID)
        {
            if (string.IsNullOrEmpty(developerID))
            {
                return GetDbConnInternal(dataBaseName, dataBaseName, SystemFile.DBFile);
            }
            else
            {
                string databaseNameForSD = string.Format("SD_{0}_{1}", developerID, dataBaseName);

                lock (typeof(DbConnectionSet))
                {
                    if (!DbConnections.Contains(databaseNameForSD))
                    {
                        var dbName = string.Empty;
                        var aliasName = string.Empty;
                        var split = true;
                        using (var connection = DbConnectionSet.GetDbConn(SystemDatabase).CreateConnection())
                        {
                            connection.Open();
                            var command = connection.CreateCommand();
                            command.CommandText = string.Format("SELECT DBName, DBAlias, Split FROM SYS_SDALIAS WHERE USERID = '{0}' AND AliasName = '{1}'"
                                , developerID, dataBaseName);
                            var reader = command.ExecuteReader();
                            if (reader.Read())
                            {
                                dbName = reader["DBName"].ToString();
                                aliasName = reader["DBAlias"].ToString();
                                if (string.Compare(reader["Split"].ToString(), bool.FalseString, true) == 0)
                                {
                                    split = false;
                                }
                            }
                            else
                            {
                                return null;
                            }
                        }

                        if (string.IsNullOrEmpty(aliasName))
                        {
                            aliasName = SystemDatabase;
                        }

                        var systemConn = DbConnectionSet.GetDbConn(aliasName);
                        //var match = System.Text.RegularExpressions.Regex.Match(systemConn.ConnectionString, @"(server|Data\s+Source)=\w+", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                        //var server = match.Success ? match.Value: ".";
                        //var connString = string.Format("{0};database={1};Connect Timeout=5;User Id=sduser;Password=1", server, dbName);

                        var connString = System.Text.RegularExpressions.Regex.Replace(systemConn.ConnectionString, @"(database|Initial\s+Catalog)=\w+;", string.Format("database={0};", dbName)
                            , System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                        DbConnection dbconn = new DbConnection(dataBaseName, connString, systemConn.MaxCount, systemConn.TimeOut, systemConn.DbType);
                        dbconn.SplitSystemTable = split;
                        DbConnections.Add(databaseNameForSD, dbconn);
                    }
                    return (DbConnection)DbConnections[databaseNameForSD];
                }

                //string dataBaseConfigFile = GetDataBaseConfigFilePath(developerID);
                //return GetDbConnInternal(databaseNameForSD, dataBaseName, dataBaseConfigFile);
            }
        }

        //public static void UpdateDataBaseConfig(string developerID)
        //{
        //    //update file
        //    string databaseNameForSD = string.Format("SD_{0}_", developerID);
        //    string dataBaseConfigFile = GetDataBaseConfigFilePath(developerID);
        //    using (IDbConnection connection = GetDbConn(SystemDatabase).CreateConnection())
        //    {
        //        IDbCommand command = connection.CreateCommand();
        //        command.CommandText = string.Format("Select * from SYS_SDDBXML where USERID = '{0}'", developerID);
        //        IDataReader reader = command.ExecuteReader();
        //        if (reader.Read())
        //        {
        //            byte[] bytes = (byte[])reader["DBXML"];
        //            File.WriteAllBytes(dataBaseConfigFile, bytes);
        //            lock (typeof(DbConnectionSet))
        //            {
        //                List<string> keys = new List<string>();
        //                //release cache
        //                foreach (string key in DbConnections.Keys)
        //                {
        //                    if (key.StartsWith(databaseNameForSD))
        //                    {
        //                        keys.Add(key);
        //                    }
        //                }
        //                foreach (string key in keys)
        //                {
        //                    DbConnections.Remove(key);
        //                }
        //            }
        //        }
        //    }
        //}

        private static DbConnection GetDbConnInternal(string dataBaseKey, string dataBaseName, string dataBaseConfigFile)
        {
            lock (typeof(DbConnectionSet))
            {
                if (!DbConnections.Contains(dataBaseKey))
                {
                    if (File.Exists(dataBaseConfigFile))
                    {
                        XmlDocument xml = new XmlDocument();
                        xml.Load(dataBaseConfigFile);
                        XmlNode node = xml.SelectSingleNode(string.Format("InfolightDB/DataBase/{0}", dataBaseName));
                        if (node != null)
                        {
                            string connString;
                            bool encrypt = node.Attributes["Encrypt"] == null ? false : Convert.ToBoolean(node.Attributes["Encrypt"].Value);
                            if (encrypt)
                            {
                                connString = ServerConfig.LoginObject.GetDBConnection(dataBaseName);
                            }
                            else
                            {
                                connString = node.Attributes["String"].InnerText;
                                if (connString.Length > 0)
                                {
                                    connString = connString.TrimEnd(';');
                                    if (node.Attributes["Password"].InnerText != String.Empty)
                                        connString = connString + ";Password=" + GetPwdString(node.Attributes["Password"].InnerText);
                                }
                            }

                            int maxCount = int.Parse(node.Attributes["MaxCount"].InnerText);
                            int timeOut = int.Parse(node.Attributes["TimeOut"].InnerText);
                            bool splitSystemTable = node.Attributes["Master"] != null && node.Attributes["Master"].Value.Trim() == "1";
                            ClientType dbType = (ClientType)int.Parse(node.Attributes["Type"].InnerText);
                            connString = GetCurrentConnectionString(connString, dbType);
                            OdbcDBType odbcType = node.Attributes["OdbcType"] != null ? (OdbcDBType)int.Parse(node.Attributes["OdbcType"].InnerText) : OdbcDBType.None;
                            DbConnection dbconn = new DbConnection(dataBaseName, connString, maxCount, timeOut, dbType);
                            dbconn.Encoding = node.Attributes["Encoding"] != null ? node.Attributes["Encoding"].Value : "";
                            dbconn.SplitSystemTable = splitSystemTable;
                            dbconn.OdbcType = odbcType;
                            DbConnections.Add(dataBaseKey, dbconn);
                        }
                    }
                }
                return (DbConnection)DbConnections[dataBaseKey];
            }
        }

        public static DbConnection GetDbConnForEF(string dataBaseName, string developerID)
        {
            if (string.IsNullOrEmpty(developerID))
            {
                return GetDbConnForEFInternal(dataBaseName, dataBaseName, SystemFile.DBFile);
            }
            else
            {
                string databaseNameForSD = string.Format("SD_{0}_{1}", developerID, dataBaseName);

                lock (typeof(DbConnectionSet))
                {
                    if (!DbConnections.Contains(databaseNameForSD))
                    {
                        var dbName = string.Empty;
                        var aliasName = string.Empty;
                        var split = true;
                        using (var connection = DbConnectionSet.GetDbConn(SystemDatabase).CreateConnection())
                        {
                            connection.Open();
                            var command = connection.CreateCommand();
                            command.CommandText = string.Format("SELECT DBName, DBAlias, Split FROM SYS_SDALIAS WHERE USERID = '{0}' AND AliasName = '{1}'"
                                , developerID, dataBaseName);
                            var reader = command.ExecuteReader();
                            if (reader.Read())
                            {
                                dbName = reader["DBName"].ToString();
                                aliasName = reader["DBAlias"].ToString();
                                if (string.Compare(reader["Split"].ToString(), bool.FalseString, true) == 0)
                                {
                                    split = false;
                                }
                            }
                            else
                            {
                                return null;
                            }
                        }

                        var systemConn = DbConnectionSet.GetDbConn(aliasName);
                        //var match = System.Text.RegularExpressions.Regex.Match(systemConn.ConnectionString, @"(server|Data\s+Source)=\w+", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                        //var server = match.Success ? match.Value: ".";
                        //var connString = string.Format("{0};database={1};Connect Timeout=5;User Id=sduser;Password=1", server, dbName);

                        var connString = System.Text.RegularExpressions.Regex.Replace(systemConn.ConnectionString, @"(database|Initial\s+Catalog)=\w+;", string.Format("database={0};", dbName)
                            , System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                        DbConnection dbconn = new DbConnection(dataBaseName, connString, systemConn.MaxCount, systemConn.TimeOut, systemConn.DbType);
                        dbconn.SplitSystemTable = split;
                        DbConnections.Add(databaseNameForSD, dbconn);
                    }
                    return (DbConnection)DbConnections[databaseNameForSD];
                }
                //string databaseNameForSD = string.Format("SD_{0}_{1}", developerID, dataBaseName);
                //string dataBaseConfigFile = GetDataBaseConfigFilePath(developerID);
                //return GetDbConnForEFInternal(databaseNameForSD, dataBaseName, dataBaseConfigFile);
            }
        }

        private static DbConnection GetDbConnForEFInternal(string dataBaseKey, string dataBaseName, string dataBaseConfigFile)
        {
            lock (typeof(DbConnectionSet))
            {
                if (!DbConnections.Contains(dataBaseKey))
                {
                    if (File.Exists(dataBaseConfigFile))
                    {
                        XmlDocument xml = new XmlDocument();
                        xml.Load(dataBaseConfigFile);
                        XmlNode node = xml.SelectSingleNode(string.Format("InfolightDB/DataBase/{0}", dataBaseName));
                        if (node != null)
                        {
                            string connString;
                            bool encrypt = node.Attributes["Encrypt"] == null ? false : Convert.ToBoolean(node.Attributes["Encrypt"].Value);
                            if (encrypt)
                            {
                                connString = ServerConfig.LoginObject.GetDBConnection(dataBaseName);
                            }
                            else
                            {
                                connString = node.Attributes["String"].InnerText;
                                if (connString.Length > 0)
                                {
                                    connString = connString.TrimEnd(';');
                                    if (node.Attributes["Password"].InnerText != String.Empty)
                                        connString = connString + ";Password=" + GetPwdString(node.Attributes["Password"].InnerText);
                                }
                            }

                            int maxCount = int.Parse(node.Attributes["MaxCount"].InnerText);
                            int timeOut = int.Parse(node.Attributes["TimeOut"].InnerText);
                            bool splitSystemTable = node.Attributes["Master"] != null && node.Attributes["Master"].Value.Trim() == "1";
                            ClientType dbType = (ClientType)int.Parse(node.Attributes["Type"].InnerText);
                            OdbcDBType odbcType = node.Attributes["OdbcType"] != null ? (OdbcDBType)int.Parse(node.Attributes["OdbcType"].InnerText) : OdbcDBType.None;
                           
                            DbConnection dbconn = new DbConnection(dataBaseName, connString, maxCount, timeOut, dbType);
                            dbconn.Encoding = node.Attributes["Encoding"] != null ? node.Attributes["Encoding"].Value : "";
                            dbconn.SplitSystemTable = splitSystemTable;
                            dbconn.OdbcType = odbcType;
                            DbConnections.Add(dataBaseKey, dbconn);
                        }
                    }
                }
                return (DbConnection)DbConnections[dataBaseKey];
            }
        }

        public static String GetConnectionString(string dataBaseName)
        {
            lock (typeof(DbConnectionSet))
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(SystemFile.DBFile);
                XmlNode node = xml.SelectSingleNode(string.Format("InfolightDB/DataBase/{0}", dataBaseName));
                if (node != null)
                {
                    string connString;
                    bool encrypt = node.Attributes["Encrypt"] == null ? false : Convert.ToBoolean(node.Attributes["Encrypt"].Value);
                    if (encrypt)
                    {
                        connString = ServerConfig.LoginObject.GetDBConnection(dataBaseName);
                    }
                    else
                    {
                        connString = node.Attributes["String"].InnerText;
                        if (connString.Length > 0)
                        {
                            connString = connString.TrimEnd(';');
                            if (node.Attributes["Password"].InnerText != String.Empty)
                                connString = connString + ";Password=" + GetPwdString(node.Attributes["Password"].InnerText);
                        }
                    }

                    ClientType dbType = (ClientType)int.Parse(node.Attributes["Type"].InnerText);
                    connString = GetCurrentConnectionString(connString, dbType);
                    return connString;
                }
            }
            return "";
        }

        public static String GetCurrentConnectionString(string connectionString, ClientType dbType)
        {
            String value = connectionString;
            if (dbType == ClientType.ctOracle && connectionString.ToUpper().StartsWith("HOST="))
            {
                value = "SERVER=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST={0})(PORT={1})))(CONNECT_DATA=(SID={2})(SERVER=DEDICATED)));UID={3};PASSWORD={4}";
                String Host = String.Empty;
                String Port = "1521";
                String Sid = String.Empty;
                String UserID = String.Empty;
                String Password = String.Empty;
                String[] strings = connectionString.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (String item in strings)
                {
                    if (item.ToUpper().StartsWith("HOST"))
                    {
                        Host = item.Split('=')[1];
                    }
                    else if (item.ToUpper().StartsWith("PORT"))
                    {
                        Port = item.Split('=')[1];
                    }
                    else if (item.ToUpper().StartsWith("USER ID"))
                    {
                        UserID = item.Split('=')[1];
                    }
                    else if (item.ToUpper().StartsWith("SID"))
                    {
                        Sid = item.Split('=')[1];
                    }
                    else if (item.ToUpper().StartsWith("PASSWORD"))
                    {
                        Password = item.Split('=')[1];
                    }
                }
                value = String.Format(value, Host, Port, Sid, UserID, Password);
            }
            return value;
        }

        #region DbConnection
        public class DbConnection
        {
            private int m_semaphore;
            private string m_connString;
            private string m_dbName;
            private int m_maxCount;
            private int m_timeOut;
            private ArrayList m_connectionInfoList;
            private ClientType m_dbType;
            private System.Threading.Semaphore FSemaphore = null;
            private System.Threading.Mutex FMutex;

            private IDbConnection GetReleasedConnection(string user, string module, string cacheCommandName, DateTime time)
            {
                FMutex.WaitOne();
                try
                {
                    if (string.IsNullOrEmpty(cacheCommandName))
                    {
                        foreach (ConnectionInfo Info in ConnectionInfoList)
                        {
                            if (Info.IsReleased)
                            {
                                Info.IsReleased = false;
                                Info.SetInfo(user, module, cacheCommandName, time);
                                return Info.Connection;
                            }
                        }
                    }
                    else
                    {
                        //判断有command属性
                        foreach (ConnectionInfo Info in ConnectionInfoList)
                        {
                            if (!Info.IsReleased && Info.UserID == user && Info.Module == module && Info.Command == cacheCommandName)
                            {
                                return Info.Connection;
                            }
                        }
                    }
                    return null;
                }
                finally
                {
                    FMutex.ReleaseMutex();
                }
            }

            public IDbConnection WaitOne(string user, string module, DateTime time)
            {
                return WaitOne(user, module, null, time);
            }

            public IDbConnection WaitOne(string user, string module, string cacheCommandName, DateTime time)
            {
                if (!string.IsNullOrEmpty(cacheCommandName))
                {
                    lock (this)
                    {
                        IDbConnection conn = GetReleasedConnection(user, module, cacheCommandName, time);
                        if (conn != null)
                        {
                            return conn;
                        }
                    }
                }
                if (FSemaphore.WaitOne(TimeOut * 1000, true))
                {
                    IDbConnection conn = null;
                    lock (this)
                    {
                        conn = GetReleasedConnection(user, module, cacheCommandName, time);
                    }
                    if (conn == null)
                    {
                        conn = CreateConnection();
                        if (conn != null)
                        {
                            if (conn != null && conn.State != ConnectionState.Open)
                            {
                                conn.Open();
                            }
                            lock (this)
                            {
                                ConnectionInfo info = new ConnectionInfo(conn, false);
                                info.SetInfo(user, module, cacheCommandName, time);
                                ConnectionInfoList.Add(info);
                            }
                        }
                    }
                    else if (conn != null && conn.State != ConnectionState.Open)
                    {
                        conn.Open();
                    }
                    return conn;
                }
                else
                {
                    return null;
                }
            }

            public IDbConnection CreateConnection()
            {
                IDbConnection conn = null;
                if (ClientType.ctMsSql == DbType)
                {
                    conn = new SqlConnection(ConnectionString);
                }
                else if (ClientType.ctOleDB == DbType)
                {
                    conn = new OleDbConnection(ConnectionString);
                }
                else if (ClientType.ctOracle == DbType)
                {
                    conn = new OracleConnection(ConnectionString);
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    OracleTransaction tran = conn.BeginTransaction(IsolationLevel.Unspecified) as OracleTransaction;
                    tran.Commit();
                    conn.Close();
                }
                else if (ClientType.ctODBC == DbType)
                {
                    conn = new OdbcConnection(ConnectionString);
                }
#if MySql
                else if (ClientType.ctMySql == DbType)
                {
                    conn = new MySql.Data.MySqlClient.MySqlConnection(ConnectionString);
                }
#endif
#if Informix
                else if (ClientType.ctInformix == DbType)
                {
                    conn = new IBM.Data.Informix.IfxConnection(ConnectionString);
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    var command = conn.CreateCommand();
                    command.CommandText = "EXECUTE PROCEDURE ifx_allow_newline ('t')";
                    command.ExecuteNonQuery();
                }
#endif
#if Sybase
                else if (ClientType.ctSybase == DbType)
                {
                    conn = new Sybase.Data.AseClient.AseConnection(ConnectionString);
                }
#endif
                else
                {
                    throw new EEPException(EEPException.ExceptionType.MethodNotSupported, this.GetType(), null, "CreateConnection", null);
                }
                return conn;
            }

            public void Release(IDbConnection Conn)
            {
                lock (this)
                {
                    foreach (ConnectionInfo Info in ConnectionInfoList)
                    {
                        if (Info.Connection == Conn && !Info.IsReleased)
                        {
                            if (string.IsNullOrEmpty(Info.Command))
                            {
                                if (Info.Connection.State == ConnectionState.Open)
                                    Info.Connection.Close();
                                Info.SetInfo(null, null, null, DateTime.Now);
                                Info.Transaction = null;
                                Info.IsReleased = true;
                                FSemaphore.Release(1);
                            }
                            else
                            {
                                //判断有command属性就不释放
                            }
                        }
                    }
                }
            }

            public void Release(string user)
            {
                lock (this)
                {
                    foreach (ConnectionInfo Info in ConnectionInfoList)
                    {
                        if (!Info.IsReleased && string.Compare(Info.UserID, user, true) == 0 && !string.IsNullOrEmpty(Info.Command))
                        {
                            if (Info.Connection.State == ConnectionState.Open)
                                Info.Connection.Close();
                            Info.SetInfo(null, null, null, DateTime.Now);
                            Info.IsReleased = true;
                            FSemaphore.Release(1);
                        }
                    }
                }
            }

            public void Release(string user, string module, string cacheCommandName)
            {
                lock (this)
                {
                    foreach (ConnectionInfo Info in ConnectionInfoList)
                    {
                        if (!Info.IsReleased && Info.UserID == user && Info.Module == module && Info.Command == cacheCommandName)
                        {
                            if (Info.Connection.State == ConnectionState.Open)
                                Info.Connection.Close();
                            Info.SetInfo(null, null, null, DateTime.Now);
                            Info.IsReleased = true;
                            FSemaphore.Release(1);
                        }
                    }
                }
            }

            public IDbTransaction GetTransaction(string user, string module, string cacheCommandName)
            {
                if (!string.IsNullOrEmpty(cacheCommandName))
                {
                    lock (this)
                    {
                        foreach (ConnectionInfo Info in ConnectionInfoList)
                        {
                            if (!Info.IsReleased && Info.UserID == user && Info.Module == module && Info.Command == cacheCommandName)
                            {
                                return Info.Transaction;
                            }
                        }
                    }
                }
                return null;
            }

            public void SetTransaction(string user, string module, string cacheCommandName, IDbTransaction transaction)
            {
                if (!string.IsNullOrEmpty(cacheCommandName))
                {
                    lock (this)
                    {
                        foreach (ConnectionInfo Info in ConnectionInfoList)
                        {
                            if (!Info.IsReleased && Info.UserID == user && Info.Module == module && Info.Command == cacheCommandName)
                            {
                                Info.Transaction = transaction;
                            }
                        }
                    }
                }
            }



            public ConnectionInfo[] GetUnReleasedConnections()
            {
                lock (this)
                {
                    List<ConnectionInfo> infos = new List<ConnectionInfo>();
                    foreach (ConnectionInfo info in ConnectionInfoList)
                    {
                        if (!info.IsReleased)
                        {
                            infos.Add(info);
                        }
                    }

                    return infos.ToArray();
                }
            }

            public void Clear()
            {
                lock (this)
                {
                    ConnectionInfoList.Clear();
                }
            }

            public DbConnection(string dbName, string connString, int maxCount, int timeOut, ClientType dbType)
            {
                this.m_connString = connString;
                this.m_semaphore = maxCount;
                this.m_dbName = dbName;
                this.m_maxCount = maxCount;
                this.m_timeOut = timeOut;
                this.m_dbType = dbType;

                this.m_connectionInfoList = new ArrayList();
                FSemaphore = new System.Threading.Semaphore(maxCount, maxCount);
                FMutex = new System.Threading.Mutex();
            }

            public string ConnectionString
            {
                get
                {
                    return m_connString;
                }
                set
                {
                    m_connString = value;
                }
            }

            public int Semaphore
            {
                get
                {
                    return m_semaphore;
                }
                set
                {
                    m_semaphore = value;
                }
            }

            public string DbName
            {
                get
                {
                    return this.m_dbName;
                }
                set
                {
                    m_dbName = value;
                }
            }

            public int MaxCount
            {
                get
                {
                    return this.m_maxCount;
                }
                set
                {
                    this.m_maxCount = value;
                }
            }

            public int TimeOut
            {
                get
                {
                    return this.m_timeOut;
                }
                set
                {
                    this.m_timeOut = value;
                }
            }

            public ArrayList ConnectionInfoList
            {
                get
                {
                    return this.m_connectionInfoList;
                }
                set
                {
                    this.m_connectionInfoList = value;
                }
            }

            public ClientType DbType
            {
                get
                {
                    return m_dbType;
                }
                set
                {
                    m_dbType = value;
                }
            }

            private OdbcDBType odbcType = OdbcDBType.None;

            public OdbcDBType OdbcType
            {
                get { return odbcType; }
                set { odbcType = value; }
            }

            private bool splitSystemTable = false;

            public bool SplitSystemTable
            {
                get
                {
                    return splitSystemTable;
                }
                set
                {
                    splitSystemTable = value;
                }
            }

            public string Encoding { get; set; }


        }
        #endregion DbConnection

        #region ConnectionInfo
        public class ConnectionInfo
        {
            public ConnectionInfo(IDbConnection connection, bool isReleased)
            {
                m_isReleased = isReleased;
                m_connection = connection;
            }

            private IDbConnection m_connection;
            public IDbConnection Connection
            {
                get
                {
                    return m_connection;
                }
                set
                {
                    m_connection = value;
                }
            }

            /// <summary>
            /// use for workflow
            /// </summary>
            private IDbTransaction m_transaction;
            public IDbTransaction Transaction
            {
                get
                {
                    return m_transaction;
                }
                set
                {
                    m_transaction = value;
                }
            }

            private bool m_isReleased;
            public bool IsReleased
            {
                get
                {
                    return m_isReleased;
                }
                set
                {
                    m_isReleased = value;
                }
            }

            private string userID;

            public string UserID
            {
                get { return userID; }
            }

            private string module;

            public string Module
            {
                get { return module; }
            }

            private string command;
            public string Command
            {
                get { return command; }
            }

            private DateTime time;

            public DateTime Time
            {
                get { return time; }
            }

            public void SetInfo(string u, string m, string c, DateTime t)
            {
                userID = u;
                module = m;
                time = t;
                command = c;
            }

            public override string ToString()
            {
                return string.Format("{0}({1:yyyy/MM/dd HH:mm:ss})", UserID, Time);
            }
        }
        #endregion ConnectionInfo
    }
}
