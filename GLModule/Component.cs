//#define MySql
using System;
using System.ComponentModel;
using System.Collections;
using System.Diagnostics;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Xml;
using System.IO;
using Microsoft.Win32;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using Srvtools;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Data.Odbc;
using System.Data.OracleClient;
#if MySql
using MySql.Data.MySqlClient;
#endif
using System.Windows.Forms;
using System.Threading;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;
using System.Text.RegularExpressions;
//要更改packageupload里ToString("G")为ToString("yyyy/MM/dd HH:mm:ss")
namespace GLModule
{
    /// <summary>
    /// Summary description for Component.
    /// </summary>
    public class Component : DataModule
    {
        private InfoCommand userInfo;
        private InfoCommand groupInfo;
        private InfoCommand UGInfo;
        private InfoCommand sqlMGroups;
        private InfoCommand sqlMGroupMenus;
        private InfoCommand sqlMenus;
        private InfoDataSource dsUserGroups;
        private UpdateComponent updateCompGroups;
        private UpdateComponent updateCompUsers;
        private InfoDataSource dsGroupMenus;
        //private InfoDataSource dsMenuGroups;
        //private UpdateComponent updateCompUG;
        private InfoCommand packageInfo;
        private UpdateComponent updateCompUGInfo;
        private UpdateComponent updateCompSolution;
        private InfoCommand solutionInfo;
        private InfoCommand menuTableLogInfo;
        private UpdateComponent updateCompMenuTableLog;
        private InfoCommand menuTableLogInfoWithoutBinary;
        private UpdateComponent updateCompMenuTableLogWithoutBinary;
        private InfoCommand cmdDBTables;
        private InfoCommand cmdColDEF;
        private UpdateComponent updateColDEF;
        private InfoCommand cmdColDEF_Details;
        private UpdateComponent updateColDEF_Details;
        private InfoCommand cmdRefValUse;
        private InfoCommand cmdERRLOG;
        private UpdateComponent updateERRLOG;
        private InfoCommand cmdSYSEEPLOGforDB;
        private InfoCommand cmdDDUse;
        private UpdateComponent updateCompMGroupMenus;
        private InfoCommand userMenus;
        private UpdateComponent updateCompUserMenus;
        private InfoCommand packageversion;
        private InfoCommand cmdSysRefVal;
        private InfoCommand cmdSysRefVal_D;
        private UpdateComponent updateCompSysRefVal;
        private UpdateComponent updateCompSysRefVal_D;
        private InfoDataSource idsRefVal;
        private InfoCommand cmdToDoList;
        private InfoCommand cmdToDoHis;
        private InfoCommand cmdRoles;
        private InfoCommand cmdOrgRoles;
        private UpdateComponent ucOrgRoles;
        private InfoCommand cmdOrgLevel;
        private UpdateComponent ucOrgLevel;
        private InfoCommand cmdOrgKind;
        private UpdateComponent ucOrgKind;
        private InfoCommand cmdWorkflow;
        private InfoCommand cmdRoleAgent;
        private UpdateComponent ucRoleAgent;
        private InfoCommand cmdSlSource;
        private ServiceManager serviceManager1;
        private InfoCommand cmdGROUPMENUCONTROL;
        private InfoCommand cmdUSERMENUCONTROL;
        private InfoCommand cmdSYS_REPORT;
        private InfoCommand cmdMENUTABLECONTROL;
        private UpdateComponent ucMENUTABLECONTROL;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components;

        public ConnectionType getConnectionType(IDbConnection connection)
        {
            ConnectionType createType = ConnectionType.SqlClient;
            if (connection is SqlConnection)
                createType = ConnectionType.SqlClient;
            else if (connection is OdbcConnection)
                createType = ConnectionType.Odbc;
            else if (connection is OracleConnection)
                createType = ConnectionType.OracleClient;
            else if (connection is OleDbConnection)
                createType = ConnectionType.OleDb;
            else if (connection.GetType().Name == "MySqlConnection")
                createType = ConnectionType.MySqlClient;
            else if (connection.GetType().Name == "IfxConnection")
                createType = ConnectionType.IfxClient;
            else if (connection.GetType().Name == "AseConnection")
                createType = ConnectionType.Sybase;

            return createType;
        }

        public Component(System.ComponentModel.IContainer container)
        {
            ///
            /// Required for Windows.Forms Class Composition Designer support
            ///
            container.Add(this);
            InitializeComponent();
            string type = GetSystemDBTypeforString();
            if (type == "1" && !(cmdDBTables.CommandText.Contains("sysobjects")))
                cmdDBTables.CommandText = "select name from sysobjects where (xtype='U' or xtype='V') order by name";
            else if (type == "2")
                cmdDBTables.CommandText = "sp_help";
            else if (type == "3" && !(cmdDBTables.CommandText.Contains("user_objects")))
                cmdDBTables.CommandText = "select OBJECT_NAME from USER_OBJECTS where (OBJECT_TYPE = 'TABLE' or OBJECT_TYPE = 'VIEW' or OBJECT_TYPE = 'SYNONYM') order by OBJECT_NAME";
            else if (type == "4")
                cmdDBTables.CommandText = "select * from SYSTABLES where (TABTYPE = 'T' or TABTYPE = 'V') and TABID >= 100 order by TABNAME";
            else if (type == "5")
                cmdDBTables.CommandText = "show tables;";
            else if (type == "6")
                cmdDBTables.CommandText = "select * from SYSTABLES where (TABTYPE = 'T' or TABTYPE = 'V') and TABID >= 100 order by TABNAME";
            else if (type == "7")
                cmdDBTables.CommandText = "sp_help";

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        public Component()
        {
            ///
            /// This call is required by the Windows.Forms Designer.
            ///
            InitializeComponent();
            string type = GetSystemDBTypeforString();
            if (type == "1" && !(cmdDBTables.CommandText.Contains("sysobjects")))
                cmdDBTables.CommandText = "select name from sysobjects where (xtype='U' or xtype='V') order by name";
            else if (type == "2")
                cmdDBTables.CommandText = "sp_help";
            else if (type == "3" && !(cmdDBTables.CommandText.Contains("user_objects")))
                cmdDBTables.CommandText = "select OBJECT_NAME from USER_OBJECTS where (OBJECT_TYPE = 'TABLE' or OBJECT_TYPE = 'VIEW' or OBJECT_TYPE = 'SYNONYM') order by OBJECT_NAME";
            else if (type == "4")
                cmdDBTables.CommandText = "select * from SYSTABLES where (TABTYPE = 'T' or TABTYPE = 'V') and TABID >= 100 order by TABNAME";
            else if (type == "5")
                cmdDBTables.CommandText = "show tables;";
            else if (type == "6")
                cmdDBTables.CommandText = "select * from SYSTABLES where (TABTYPE = 'T' or TABTYPE = 'V') and TABID >= 100 order by TABNAME";
            else if (type == "7")
                cmdDBTables.CommandText = "sp_help";
            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        public object[] GetSysTableByLoginDB(object[] param)
        {
            String sDBAlias = GetClientInfo(ClientInfoType.LoginDB).ToString();
            ClientType ct = ClientType.ctMsSql;
            IDbConnection mySqlConnection = AllocateConnection(sDBAlias, ref ct, false);
            DataSet dsReturn = new DataSet();
            try
            {
                IDbCommand myCommand = mySqlConnection.CreateCommand();
                myCommand.CommandText = "select name from sysobjects where (xtype='U' or xtype='V') order by name";
                if (ct == ClientType.ctMsSql && !(cmdDBTables.CommandText.Contains("sysobjects")))
                    myCommand.CommandText = "select name from sysobjects where (xtype='U' or xtype='V') order by name";
                else if (ct == ClientType.ctOleDB)
                    myCommand.CommandText = "sp_help";
                else if (ct == ClientType.ctOracle && !(cmdDBTables.CommandText.Contains("user_objects")))
                    myCommand.CommandText = "select OBJECT_NAME from USER_OBJECTS where (OBJECT_TYPE = 'TABLE' or OBJECT_TYPE = 'VIEW' or OBJECT_TYPE = 'SYNONYM') order by OBJECT_NAME";
                else if (ct == ClientType.ctODBC)
                {
                    var subType = GetDataBaseSubType(new object[] { sDBAlias, false });
                    if (subType[1].ToString() == "2")
                        myCommand.CommandText = "select TABLE_NAME AS name, TABLE_OWNER AS owner from qsys2.TABLES where TABLE_TYPE='BASE TABLE' OR TABLE_TYPE = 'VIEW'";
                    else if (subType[1].ToString() == "0")
                        myCommand.CommandText = "select * from SYSTABLES where (TABTYPE = 'T' or TABTYPE = 'V') and TABID >= 100 order by TABNAME";
                }
                else if (ct == ClientType.ctMySql)
                    myCommand.CommandText = "show tables;";
                else if (ct == ClientType.ctInformix)
                    myCommand.CommandText = "select * from SYSTABLES where (TABTYPE = 'T' or TABTYPE = 'V') and TABID >= 100 order by TABNAME";
                else if (ct == ClientType.ctSybase)
                    myCommand.CommandText = "sp_help";

                IDataAdapter adpater = DBUtils.CreateDbDataAdapter(myCommand);
                adpater.Fill(dsReturn);

                if (ct == ClientType.ctMsSql)
                {
                    //dsReturn.Tables[0].Columns.Add("owner", typeof(string));
                    myCommand.CommandText = "exec sp_tables";
                    DataSet ownerDataSet = new DataSet();
                    adpater.Fill(ownerDataSet);

                    DataSet dataSet = dsReturn.Clone();
                    dataSet.Tables[0].Columns.Add("owner", typeof(string));
                    for (int i = 0; i < ownerDataSet.Tables[0].Rows.Count; i++)
                    {
                        var tableName = ownerDataSet.Tables[0].Rows[i]["TABLE_NAME"];
                        if (dsReturn.Tables[0].Select("name='" + tableName + "'").Length > 0)
                        {
                            dataSet.Tables[0].Rows.Add(new object[] { tableName, string.Format("{0}.[{1}]", ownerDataSet.Tables[0].Rows[i]["TABLE_OWNER"], tableName) });
                        }
                    }
                    dsReturn = dataSet;
                    //for (int i = 0; i < dsReturn.Tables[0].Rows.Count; i++)
                    //{
                    //    var tableName = dsReturn.Tables[0].Rows[i][0].ToString();
                    //    var ownerRow = ownerDataSet.Tables[0].Select("TABLE_NAME='" + tableName + "'");
                    //    if (ownerRow.Length > 0)
                    //    {
                    //        dsReturn.Tables[0].Rows[i]["owner"] = string.Format("{0}.[{1}]", ownerRow[0]["TABLE_OWNER"], tableName);
                    //    }
                    //    else
                    //    {
                    //        dsReturn.Tables[0].Rows[i]["owner"] = tableName;
                    //    }
                    //}
                }

            }
            finally
            {
                ReleaseConnection(sDBAlias, mySqlConnection);
            }
            return new object[] { 0, dsReturn, ct };
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        //private string GetPwdString(string s)
        //{
        //    string sRet = "";
        //    for (int i = 0; i < s.Length; i++)
        //    {
        //        sRet = sRet + (char)(((int)(s[s.Length - 1 - i])) ^ s.Length);
        //    }
        //    return sRet;
        //}

        //private string GetDBString(string sDB, ref ClientType ct, bool bGetSysDB)
        //{
        //    string sRet = "";
        //    XmlDocument DBXML = new XmlDocument();
        //    ct = ClientType.ctNone;

        //    String s = SystemFile.DBFile;

        //    if (File.Exists(s))
        //    {
        //        DBXML.Load(s);
        //        XmlNode aNode = DBXML.DocumentElement.FirstChild;
        //        XmlNode sysNode = null;
        //        string sysDB = "";

        //        while ((null != aNode))
        //        {
        //            if (string.Compare(aNode.Name, "SYSTEMDB", true) == 0)//IgnoreCase
        //            {
        //                sysNode = aNode;
        //                sysDB = sysNode.InnerText.Trim();
        //                break;
        //            }
        //            aNode = aNode.NextSibling;
        //        }

        //        aNode = DBXML.DocumentElement.FirstChild;
        //        while ((null != aNode) && string.Compare(aNode.Name, "DATABASE", true) != 0)//IgnoreCase
        //        {
        //            aNode = aNode.NextSibling;
        //        }
        //        if (null != aNode) aNode = aNode.FirstChild;

        //        XmlNode yNode = aNode;
        //        bool bSplit = false;
        //        while (yNode != null)
        //        {
        //            if (string.Compare(yNode.LocalName, sDB, true) == 0)//IgnoreCase
        //            {
        //                if (!yNode.Attributes["Master"].InnerText.Trim().Equals("1"))//如果不是Split system table
        //                {
        //                    sRet = yNode.Attributes["String"].InnerText;
        //                    if (null != yNode.Attributes["Password"] && yNode.Attributes["Password"].InnerText != "")
        //                    {
        //                        string stemp = GetPwdString(yNode.Attributes["Password"].InnerText);
        //                        if (sRet.Length > 0)
        //                        {
        //                            if (sRet[sRet.Length - 1] != ';')
        //                                sRet = sRet + ";Password=" + stemp;
        //                            else
        //                                sRet = sRet + "Password=" + stemp;
        //                        }
        //                    }

        //                    ct = (ClientType)(Int32.Parse(yNode.Attributes["Type"].InnerText));
        //                    break;
        //                }
        //                else
        //                {
        //                    bSplit = true;
        //                    break;//如果是Split system table,则SysDB有效
        //                }
        //            }
        //            else
        //            {
        //                yNode = yNode.NextSibling;
        //                continue;
        //            }
        //        }

        //        if (bSplit && bGetSysDB)//如果是分割系统，并且系统DataBase不是空
        //        {
        //            if (!sysDB.Equals(""))
        //            {
        //                XmlNode xNode = aNode;
        //                while (xNode != null)
        //                {
        //                    if (string.Compare(xNode.LocalName, sysDB, true) == 0)//IgnoreCase
        //                    {
        //                        sRet = xNode.Attributes["String"].InnerText;
        //                        if (null != xNode.Attributes["Password"] && xNode.Attributes["Password"].InnerText != "")
        //                        {
        //                            string stemp = GetPwdString(xNode.Attributes["Password"].InnerText);
        //                            if (sRet.Length > 0)
        //                            {
        //                                if (sRet[sRet.Length - 1] != ';')
        //                                    sRet = sRet + ";Password=" + stemp;
        //                                else
        //                                    sRet = sRet + "Password=" + stemp;
        //                            }
        //                            //sRet = sRet + ";Password=" + stemp ;
        //                        }

        //                        ct = (ClientType)(Int32.Parse(xNode.Attributes["Type"].InnerText));
        //                        return sRet;
        //                    }
        //                    else
        //                    {
        //                        xNode = xNode.NextSibling;
        //                        continue;
        //                    }
        //                }
        //            }
        //            else
        //                throw new Exception("split-system-table is TRUE, but SystemDB is EMPTY!");
        //        }
        //        else
        //        {
        //            // modify by rax
        //            if (yNode == null)
        //                return "";
        //            sRet = yNode.Attributes["String"].InnerText;
        //            if (null != yNode.Attributes["Password"] && yNode.Attributes["Password"].InnerText != "")
        //            {
        //                string stemp = GetPwdString(yNode.Attributes["Password"].InnerText);
        //                if (sRet.Length > 0)
        //                {
        //                    if (sRet[sRet.Length - 1] != ';')
        //                        sRet = sRet + ";Password=" + stemp;
        //                    else
        //                        sRet = sRet + "Password=" + stemp;
        //                }
        //            }

        //            ct = (ClientType)(Int32.Parse(yNode.Attributes["Type"].InnerText));
        //            // end
        //            return sRet;
        //        }
        //    }
        //    return sRet;
        //}

        public object[] GetSplitSysDB2(object[] p)
        {
            string db = p[0].ToString();
            string sysDb = GetSplitSysDBSD(db);
            //string sysDb = GetSplitSysDB(db);
            return new object[] { 0, sysDb };
        }

        private string GetSplitSysDB(string sDB)
        {
            String s = SystemFile.DBFile;

            if (File.Exists(s))
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(s);
                XmlNode node = xml.SelectSingleNode(string.Format("InfolightDB/DataBase/{0}", sDB));
                if (node != null)
                {
                    if (node.Attributes["Master"] != null && node.Attributes["Master"].Value.Trim() == "1")
                    {
                        XmlNode nodesys = xml.SelectSingleNode("InfolightDB/SystemDB");
                        if (nodesys != null)
                        {
                            string sysdb = nodesys.InnerText.Trim();
                            XmlNode nodecheck = xml.SelectSingleNode(string.Format("InfolightDB/DataBase/{0}", sysdb));
                            if (nodecheck != null)
                            {
                                return sysdb;
                            }
                            else
                            {
                                throw new Exception("SystemDB does not exsit in db list");
                            }
                        }
                        else
                        {
                            throw new Exception("SystemDB is Empty");
                        }
                    }
                    else
                    {
                        return sDB;
                    }
                }
                else
                {
                    throw new Exception(string.Format("EEPAlias:{0} does not exsit", sDB));
                }
            }
            else
            {
                throw new Exception(string.Format("{0} does not exsit", s));
            }

        }
        //        private IDbConnection AllocateConnection(string connectionString, int dbType)
        //        {
        //            if (dbType == 2)
        //            {
        //                return new OleDbConnection(connectionString);
        //            }
        //            else if (dbType == 3)
        //            {
        //                return new OracleConnection(connectionString);
        //            }
        //            else if (dbType == 4)
        //            {
        //                return new OdbcConnection(connectionString);
        //            }
        //            else if (dbType == 1)
        //            {
        //                return new SqlConnection(connectionString);
        //            }
        //#if MySql
        //            else if (dbType == 5)
        //            {
        //                return new MySqlConnection(connectionString);
        //            }
        //#endif
        //            else
        //            {
        //                return null;
        //            }
        //        }

        private IDbConnection AllocateConnection(string DBName, ref ClientType ct, bool bGetSysDB)
        {
            string dbname = bGetSysDB ? GetSplitSysDBSD(DBName) : DBName;
            //string dbname = bGetSysDB ? GetSplitSysDB(DBName) : DBName;
            IDbConnection conn = AllocateConnection(dbname);
            if (conn is SqlConnection)
                ct = ClientType.ctMsSql;
            else if (conn is OracleConnection)
                ct = ClientType.ctOracle;
            else if (conn is OdbcConnection)
                ct = ClientType.ctODBC;
            else if (conn is OleDbConnection)
                ct = ClientType.ctOleDB;
#if MySql
            else if (conn is MySqlConnection)
                ct = ClientType.ctMySql;
#endif
            else if (conn.GetType().FullName == "IBM.Data.Informix.IfxConnection")
                ct = ClientType.ctInformix;
            else if (conn.GetType().FullName == "Sybase.Data.AseClient.AseConnection")
                ct = ClientType.ctSybase;
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            return conn;
        }

        private void ReleaseConnection(string DBName, IDbConnection conn, bool bGetSysDB)
        {
            string dbname = bGetSysDB ? GetSplitSysDBSD(DBName) : DBName;
            //string dbname = bGetSysDB ? GetSplitSysDB(DBName) : DBName;
            ReleaseConnection(dbname, conn);
        }

        public object LogError(object[] objParam)
        {
            #region Build the command and exec the command to log error

            ClientType ct = ClientType.ctNone;
            IDbConnection conn = AllocateConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), ref ct, true);
            try
            {
                String sQL = String.Empty;


                //为了区分不同的数据库 by Rei
                InfoCommand command = new InfoCommand(ClientInfo);
                command.Connection = conn;

                DbParameter paramUserId = null;
                DbParameter paramMoudleName = null;
                DbParameter paramErrMessage = null;
                DbParameter paramErrStack = null;
                DbParameter paramErrDescrip = null;
                DbParameter paramErrDate = null;
                DbParameter paramErrScreen = null;
                DbParameter paramStatus = null;

                if (conn is OleDbConnection)
                {
                    sQL = "insert into SYSERRLOG(USERID, MODULENAME, ERRMESSAGE, ERRSTACK, ERRDESCRIP, ERRDATE, ERRSCREEN, STATUS)" +
                            "values(?, ?, ?, ?, ?, ?, ?, ?)";
                    paramUserId = new OleDbParameter("@UserId", OleDbType.VarChar, 20);
                    paramMoudleName = new OleDbParameter("@ModuleName", OleDbType.VarChar, 30);
                    paramErrMessage = new OleDbParameter("@ErrMessage", OleDbType.VarChar, 300);
                    paramErrStack = new OleDbParameter("@ErrStack", OleDbType.VarChar, 5000);
                    paramErrDescrip = new OleDbParameter("@ErrDescrip", OleDbType.VarChar, 300);
                    paramErrDate = new OleDbParameter("@ErrDate", OleDbType.Date);
                    paramErrScreen = new OleDbParameter("@ErrScreen", OleDbType.Binary);
                    paramStatus = new OleDbParameter("@Status", OleDbType.VarChar, 2);
                }
                else if (conn is OdbcConnection)
                {
                    sQL = "insert into SYSERRLOG(USERID, MODULENAME, ERRMESSAGE, ERRSTACK, ERRDESCRIP, ERRDATE, ERRSCREEN, STATUS)" +
                            "values(@UserId, @ModuleName, @ErrMessage, @ErrStack, @ErrDescrip, @ErrDate, @ErrScreen, @Status)";
                    paramUserId = new OdbcParameter("@UserId", OdbcType.VarChar, 20);
                    paramMoudleName = new OdbcParameter("@ModuleName", OdbcType.VarChar, 30);
                    paramErrMessage = new OdbcParameter("@ErrMessage", OdbcType.VarChar, 300);
                    paramErrStack = new OdbcParameter("@ErrStack", OdbcType.VarChar, 5000);
                    paramErrDescrip = new OdbcParameter("@ErrDescrip", OdbcType.VarChar, 300);
                    paramErrDate = new OdbcParameter("@ErrDate", OdbcType.Date);
                    paramErrScreen = new OdbcParameter("@ErrScreen", OdbcType.Binary);
                    paramStatus = new OdbcParameter("@Status", OdbcType.VarChar, 2);
                }
                else if (conn is OracleConnection)
                {
                    sQL = "insert into SYSERRLOG(USERID, MODULENAME, ERRMESSAGE, ERRSTACK, ERRDESCRIP, ERRDATE, ERRSCREEN, STATUS)" +
                            "values(:UserId, :ModuleName, :ErrMessage, :ErrStack, :ErrDescrip, :ErrDate, :ErrScreen, :Status)";
                    paramUserId = new OracleParameter(":UserId", OracleType.VarChar, 20);
                    paramMoudleName = new OracleParameter(":ModuleName", OracleType.VarChar, 30);
                    paramErrMessage = new OracleParameter(":ErrMessage", OracleType.VarChar, 300);
                    paramErrStack = new OracleParameter(":ErrStack", OracleType.VarChar, 5000);
                    paramErrDescrip = new OracleParameter(":ErrDescrip", OracleType.VarChar, 300);
                    paramErrDate = new OracleParameter(":ErrDate", OracleType.DateTime);
                    paramErrScreen = new OracleParameter(":ErrScreen", OracleType.Blob, (objParam[6] as byte[]).Length, ParameterDirection.Input, false,
                                                                        0, 0, null, DataRowVersion.Current, objParam[6]);
                    paramStatus = new OracleParameter(":Status", OracleType.VarChar, 2);
                }
                else if (conn is SqlConnection)
                {
                    sQL = "insert into SYSERRLOG(USERID, MODULENAME, ERRMESSAGE, ERRSTACK, ERRDESCRIP, ERRDATE, ERRSCREEN, STATUS)" +
                            "values(@UserId, @ModuleName, @ErrMessage, @ErrStack, @ErrDescrip, @ErrDate, @ErrScreen, @Status)";

                    paramUserId = new SqlParameter("@UserId", SqlDbType.VarChar, 20);
                    paramMoudleName = new SqlParameter("@ModuleName", SqlDbType.VarChar, 30);
                    paramErrMessage = new SqlParameter("@ErrMessage", SqlDbType.VarChar, 300);
                    paramErrStack = new SqlParameter("@ErrStack", SqlDbType.VarChar, 5000);
                    paramErrDescrip = new SqlParameter("@ErrDescrip", SqlDbType.VarChar, 300);
                    paramErrDate = new SqlParameter("@ErrDate", SqlDbType.DateTime);
                    paramErrScreen = new SqlParameter("@ErrScreen", SqlDbType.Binary);
                    paramStatus = new SqlParameter("@Status", SqlDbType.VarChar, 2);
                }
#if MySql
                else if (conn is MySqlConnection)
                {
                    paramUserId = new MySqlParameter("@UserId", MySqlDbType.VarChar, 20);
                    paramMoudleName = new MySqlParameter("@ModuleName", MySqlDbType.VarChar, 30);
                    paramErrMessage = new MySqlParameter("@ErrMessage", MySqlDbType.VarChar, 300);
                    paramErrStack = new MySqlParameter("@ErrStack", MySqlDbType.VarChar, 5000);
                    paramErrDescrip = new MySqlParameter("@ErrDescrip", MySqlDbType.VarChar, 300);
                    paramErrDate = new MySqlParameter("@ErrDate", MySqlDbType.Datetime);
                    paramErrScreen = new MySqlParameter("@ErrScreen", MySqlDbType.Binary);
                    paramStatus = new MySqlParameter("@Status", MySqlDbType.VarChar, 2);
                }
#endif
#if Informix
            else if (conn is IBM.Data.Informix.IfxConnection)
            {
                sQL = "insert into SYSERRLOG(USERID, MODULENAME, ERRMESSAGE, ERRSTACK, ERRDESCRIP, ERRDATE, ERRSCREEN, STATUS)" +
                            "values(?, ?, ?, ?, ?, ?, ?, ?)";

                paramUserId = new IBM.Data.Informix.IfxParameter("?", IBM.Data.Informix.IfxType.VarChar, 20);
                paramMoudleName = new IBM.Data.Informix.IfxParameter("?", IBM.Data.Informix.IfxType.VarChar, 30);
                paramErrMessage = new IBM.Data.Informix.IfxParameter("?", IBM.Data.Informix.IfxType.VarChar, 300);
                paramErrStack = new IBM.Data.Informix.IfxParameter("?", IBM.Data.Informix.IfxType.Text, 5000);
                paramErrDescrip = new IBM.Data.Informix.IfxParameter("?", IBM.Data.Informix.IfxType.VarChar, 300);
                paramErrDate = new IBM.Data.Informix.IfxParameter("?", IBM.Data.Informix.IfxType.DateTime);
                paramErrScreen = new IBM.Data.Informix.IfxParameter("?", IBM.Data.Informix.IfxType.Blob);
                paramStatus = new IBM.Data.Informix.IfxParameter("?", IBM.Data.Informix.IfxType.VarChar, 2);
            }
#endif

                command.CommandText = sQL;
                paramUserId.Value = (objParam[0] == null ? "" : objParam[0]);
                paramMoudleName.Value = (objParam[1] == null ? "" : objParam[1]);
                paramErrMessage.Value = (objParam[2] == null ? "" : objParam[2]);
                paramErrStack.Value = (objParam[3] == null ? "" : objParam[3]);
                paramErrDescrip.Value = (objParam[4] == null ? "" : objParam[4]);
                paramErrDate.Value = (objParam[5] == null ? DateTime.Now : objParam[5]); ;
                paramErrScreen.Value = (objParam[6] == null || (objParam[6] as byte[]).Length == 0 ? DBNull.Value : objParam[6]);
                paramStatus.Value = (objParam[7] == null ? "" : objParam[7]);

                command.Parameters.Add(paramUserId);
                command.Parameters.Add(paramMoudleName);
                command.Parameters.Add(paramErrMessage);
                command.Parameters.Add(paramErrStack);
                command.Parameters.Add(paramErrDescrip);
                command.Parameters.Add(paramErrDate);
                command.Parameters.Add(paramErrScreen);
                command.Parameters.Add(paramStatus);

                Int32 i = command.ExecuteNonQuery();
                if (i != 1)
                {
                    return new object[] { 1 };
                }
                else
                {
                    return new object[] { 0 };
                }
            }
            finally
            {
                ReleaseConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), conn, true);
            }

            #endregion
        }

        public object[] CheckUser(object[] objParam)
        {
            string[] ss = objParam[0].ToString().Split(":".ToCharArray());
            string username = "";
            string useridindb = "";
            try
            {
                string sUserId = ss[0];
                string sUserPwd = ss[1];
                string sDB = ss[2];
                string relogin = ss[3];

                //matida 2010/1/29 add 
                String SharePoint = String.Empty;
                if (ss.Length == 5)
                    SharePoint = ss[4].ToLower();
                if (ss.Length == 7)
                    SharePoint = ss[6].ToLower();

                object computername = GetClientInfo(ClientInfoType.ComputerName);
                string strcomputer = computername != null ? computername.ToString() : string.Empty;

                if (!SrvGL.AllowLoginInOtherPC && relogin == "0" && SrvGL.isUserLogined(sUserId.ToLower()))
                {
                    UserInfo info = SrvGL.GetUsersInfo(sUserId.ToLower());
                    if (!info.Contains(strcomputer))
                    {
                        return new object[] { 0, LoginResult.RequestReLogin, username, useridindb };
                    }
                }
                if (ServerConfig.LoginObjectEnabled)//ILogin
                {
                    if (!ServerConfig.LoginObject.CheckUser(sUserId, sUserPwd))
                    {
                        return new object[] { 0, LoginResult.PasswordError, username, useridindb };
                    }
                    username = ServerConfig.LoginObject.GetUserInfo(sUserId, sUserPwd, UserInfoType.UserName).ToString();
                    useridindb = sUserId;
                    SrvGL.LogUser(sUserId.ToLower(), username, strcomputer, 1);
                }
                else
                {

                    //rich modified, 根据andy的最高指示,systemtable和eepalias要分离；(如果systemtable未设置,则使用前端传过来的eepalias)
                    ClientType ct = ClientType.ctMsSql;
                    IDbConnection mySqlConnection = AllocateConnection(sDB, ref ct, true);
                    try
                    {


                        //为了区分不同的数据库 by Rei
                        InfoCommand myInfoCommand = new InfoCommand(ClientInfo);

                        if (ServerConfig.UserDefination)
                        {
                            myInfoCommand.CommandText = string.Format("Select {0},{1},{2},'N','S' From {3} Where {0} = '{4}'", ServerConfig.UserID
                                , ServerConfig.Password, ServerConfig.UserName, ServerConfig.UserTable, sUserId);
                        }
                        else
                        {
                            myInfoCommand.CommandText = "SELECT USERID,PWD,USERNAME,MSAD,AUTOLOGIN FROM USERS WHERE USERID = '" +
                              sUserId + "'";
                        }

                        myInfoCommand.Connection = mySqlConnection;
                        IDataReader aReader = myInfoCommand.ExecuteReader(CommandBehavior.CloseConnection);

                        if (!aReader.Read())  // userid not found 
                        {
                            myInfoCommand.Cancel();
                            aReader.Close();
                            return new object[] { 0, LoginResult.UserNotFound, username, useridindb };
                        }
                        if (string.Compare(aReader.GetValue(4).ToString(), "x", true) == 0)
                        {
                            myInfoCommand.Cancel();
                            aReader.Close();
                            return new object[] { 0, LoginResult.Disabled, username, useridindb };
                        }
                        object sPwd = aReader.GetValue(1);
                        if (ct == ClientType.ctOleDB && sPwd.ToString() == " ")
                            sPwd = "";
                        username = aReader.GetValue(2).ToString();
                        useridindb = aReader.GetValue(0).ToString();    //get userid in database to avoid case problem
                        string msad = aReader.GetValue(3).ToString();
                        string domainname = ss.Length == 6 ? ss[4] : string.Empty;
                        string domaincheck = ss.Length == 6 ? ss[5] : string.Empty;
                        myInfoCommand.Cancel();
                        aReader.Close();

                        if (domainname.Length == 0)
                        {
                            if (string.Compare(msad, "Y", true) == 0)
                            {
                                var valid = false;
                                foreach (var domain in ServerConfig.Domains)
                                {
                                    var ad = new ADClass() { ADPath = "LDAP://" + domain.Path, ADUser = domain.User, ADPassword = domain.Password };
                                    valid = ad.IsUserValid(sUserId, sUserPwd);
                                    if (valid)
                                    {
                                        break;
                                    }
                                }
                                if (!valid)
                                {
                                    return new object[] { 0, LoginResult.PasswordError, username, useridindb };
                                }
                            }
                            else
                            {
                                if (sUserPwd.Length > 10)
                                {
                                    return new object[] { 0, LoginResult.PasswordError, username, useridindb };
                                }
                                string enPwd = sUserPwd;
                                if (sUserPwd.Length > 0)
                                {
                                    char[] p = new char[] { };
                                    bool q = Encrypt.EncryptPassword(sUserId, sUserPwd, 10, ref p, false);
                                    enPwd = new string(p);
                                }

                                if (!Comparer.Equals(enPwd, sPwd.ToString().Trim()) && SharePoint != "sharepoint")  // pwd not correct //matida 2010/1/29 add 
                                {
                                    return new object[] { 0, LoginResult.PasswordError, username, useridindb };
                                }
                            }
                        }
                        else
                        {
                            var domainValid = false;
                            foreach (var domain in ServerConfig.Domains)
                            {
                                if (string.Compare(domainname, domain.Path, true) == 0)
                                {
                                    domainValid = true;
                                    break;
                                }
                            }
                            if (!domainValid || (string.Compare(msad, "Y", true) != 0) || CliUtils.DomainCheckSum(domainname) != domaincheck)
                            {
                                return new object[] { 0, LoginResult.PasswordError, username, useridindb };
                            }
                        }
                        SrvGL.LogUser(sUserId.ToLower(), username, strcomputer, 1);
                    }
                    finally
                    {
                        ReleaseConnection(sDB, mySqlConnection, true);
                    }
                }
            }
            catch (Exception e)
            {
                return new object[] { 1, e.Message };
            }
            return new object[] { 0, LoginResult.Success, username, useridindb };
        }

        public object[] CheckManagerRight(object[] objParam)
        {
            string sDB = objParam[0].ToString();
            string sUserId = objParam[1].ToString();
            string password = (string)GetClientInfo(ClientInfoType.Password);

            if (ServerConfig.LoginObjectEnabled)//ILogin
            {
                if (ServerConfig.LoginObject.GetMenuRight(sUserId, password))
                {
                    return new object[] { 0, "0" };
                }
                else
                {
                    return new object[] { 0, "1" };
                }
            }
            else
            {
                ClientType ct = ClientType.ctMsSql;
                IDbConnection mySqlConnection = AllocateConnection(sDB, ref ct, true);
                try
                {
                    string sRet = "";
                    //为了区分不同的数据库 by Rei
                    InfoCommand myInfoCommand = new InfoCommand(ClientInfo);
                    myInfoCommand.CommandText = "SELECT USERID,AUTOLOGIN FROM USERS WHERE USERID = '" +
                      sUserId + "'";

                    myInfoCommand.Connection = mySqlConnection;
                    IDataReader aReader = myInfoCommand.ExecuteReader();
                    if (!aReader.Read())  // userid not found 
                    {
                        sRet = "2";
                    }
                    else
                    {
                        string autologin = aReader.GetValue(1).ToString();
                        if (string.Compare(autologin, "s", true) == 0)//IgnoreCase
                        {
                            sRet = "0";
                        }
                        else
                        {
                            sRet = "1";
                        }
                    }
                    myInfoCommand.Cancel();
                    aReader.Close();
                    return new object[] { 0, sRet };
                }
                finally
                {
                    ReleaseConnection(sDB, mySqlConnection, true);
                }
            }
        }

        public object[] GetSysMsgXml(object[] objParam)
        {
            try
            {
                object o = objParam[0];
                if (o != null)
                {
                    DateTime t1 = (DateTime)o;
                    if (File.Exists(SystemFile.SysMsgFile))
                    {
                        FileInfo fileInfo = new FileInfo(SystemFile.SysMsgFile);
                        DateTime t2 = fileInfo.LastWriteTime;
                        if (t2 > t1)
                        {
                            XmlDocument xmlDoc = new XmlDocument();
                            xmlDoc.Load(SystemFile.SysMsgFile);

                            return new object[] { 0, xmlDoc.InnerXml };
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return new object[] { 1, e.Message };
            }

            return new object[] { 0, "0" };
        }

        public object[] UpdateFLXoml(object[] objParam)
        {
            try
            {
                string fileName = objParam[0].ToString();
                string xml = objParam[1].ToString();

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);

                string s = Application.StartupPath + fileName;
                if (!Directory.Exists(Path.GetDirectoryName(s)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(s));
                }

                doc.Save(s);
            }
            catch (Exception e)
            {
                return new object[] { 1, e.Message };
            }

            return new object[] { 0, null };
        }

        public object[] SaveFlowXoml(object[] param)
        {
            String webPath = param[0].ToString();
            String fileName = string.Format("SD_{0}_{1}", DeveloperID, param[1].ToString());
            String sXml = param[2].ToString();
            String path = Application.StartupPath + string.Format("\\Workflow\\{0}", DeveloperID);
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            String xomlName = Path.Combine(path, fileName + ".xoml");

            if (File.Exists(xomlName))
            {
                File.Delete(xomlName);
            }
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(sXml);
            doc.Save(xomlName);

            String s = string.Format("{0}\\FLDesigner.exe", EEPRegistry.Server);
            if (File.Exists(s))
            {
                Assembly assembly = Assembly.LoadFrom(s);
                object aMainForm = assembly.CreateInstance("FLDesigner.MainForm");
                MethodInfo mSaveWebFlow2 = aMainForm.GetType().GetMethod("SaveWebFlow2");
                mSaveWebFlow2.Invoke(aMainForm, new object[] { new object[] { webPath, fileName, xomlName } });
            }
            return new object[] { 0 };
        }

        public object[] GetSystemDBType(object[] objParam)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(SystemFile.DBFile);

            XmlNode node = xmlDoc.FirstChild.SelectSingleNode("SystemDB");

            object[] DbString = new object[1];
            DbString[0] = node.FirstChild.Value;
            string systemDBType = "";
            object[] temp = GetDataBaseType(DbString);
            if (temp != null && temp[0].ToString() == "0")
                systemDBType = temp[1].ToString();

            return new object[] { 0, systemDBType };
        }

        public string GetSystemDBTypeforString()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(SystemFile.DBFile);

            XmlNode node = xmlDoc.FirstChild.SelectSingleNode("SystemDB");

            object[] DbString = new object[1];
            DbString[0] = node.FirstChild.Value;
            string systemDBType = "";
            object[] temp = GetDataBaseType(DbString);
            if (temp != null && temp[0].ToString() == "0")
                systemDBType = temp[1].ToString();

            return systemDBType;
        }

        private string GetSystemDBName()
        {
            XmlDocument DBXML = new XmlDocument();
            string sysDB = "";

            if (File.Exists(SystemFile.DBFile))
            {
                DBXML.Load(SystemFile.DBFile);
                XmlNode aNode = DBXML.DocumentElement.FirstChild;
                XmlNode sysNode = null;

                while ((null != aNode))
                {
                    if (string.Compare(aNode.Name, "SYSTEMDB", true) == 0)//IgnoreCase
                    {
                        sysNode = aNode;
                        sysDB = sysNode.InnerText.Trim();
                        break;
                    }
                    aNode = aNode.NextSibling;
                }
            }
            return sysDB;
        }

        //1. MSSql   2.OleDb   3.Oracle   4.ODBC   5.MySql   6.Informix
        public object[] GetDataBaseType(object[] objParam)
        {
            string aliasName = objParam[0].ToString();
            bool split = true;
            if (objParam.Length > 1)
                split = (bool)objParam[1];
            string dbname = aliasName;
            if (split)
                dbname = GetSplitSysDBSD(aliasName);
            if (ClientInfo == null)
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(SystemFile.DBFile);

                XmlNode node = xmlDoc.FirstChild.FirstChild.SelectSingleNode(aliasName);
                if (node == null)
                {
                    throw new Exception("SystemDB is not exist!");
                }
                string DBType = node.Attributes["Type"].Value.Trim();
                string OdbcType = (node.Attributes["OdbcType"] == null ? "" : node.Attributes["OdbcType"].Value.Trim());
                return new object[] { 0, DBType, OdbcType };
            }
            else
            {
                IDbConnection conn1 = AllocateConnection(dbname);
                try
                {
                    String DBType = "0";
                    if (conn1.GetType().Name == "SqlConnection")
                        DBType = "1";
                    else if (conn1.GetType().Name == "OleDbConnection")
                        DBType = "2";
                    else if (conn1.GetType().Name == "OracleConnection")
                        DBType = "3";
                    else if (conn1.GetType().Name == "OdbcConnection")
                        DBType = "4";
                    else if (conn1.GetType().Name == "MySqlConnection")
                        DBType = "5";
                    else if (conn1.GetType().Name == "Ifxonnection")
                        DBType = "6";
                    else if (conn1.GetType().Name == "AseConnection")
                        DBType = "7";
                    return new object[] { 0, DBType, "0" };
                }
                finally
                {
                    ReleaseConnection(dbname, conn1);
                }
            }
        }

        public object[] GetDataBaseSubType(object[] objParam)
        {
            string aliasName = objParam[0].ToString();
            bool split = true;
            if (objParam.Length > 1)
                split = (bool)objParam[1];
            string dbname = aliasName;
            if (split)
                dbname = GetSplitSysDBSD(aliasName);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(SystemFile.DBFile);

            XmlNode node = xmlDoc.FirstChild.FirstChild.SelectSingleNode(aliasName);
            if (node == null)
            {
                throw new Exception("SystemDB is not exist!");
            }
            string DBType = node.Attributes["Type"].Value.Trim();
            string OdbcType = (node.Attributes["OdbcType"] == null ? "" : node.Attributes["OdbcType"].Value.Trim());
            return new object[] { 0, OdbcType };
        }

        public string GetDataBaseTypeforString(string objParam)
        {
            string aliasName = objParam;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(SystemFile.DBFile);

            XmlNode node = xmlDoc.FirstChild.FirstChild.SelectSingleNode(aliasName);
            if (node == null)
                return "";
            string DBType = node.Attributes["Type"].Value.Trim();
            return DBType;
        }

        public object[] GetWebSitePath(object[] objParam)
        {
            string webSitePath = string.Format("{0}\\", EEPRegistry.WebClient);
            return new object[] { 0, webSitePath };
        }

        public object[] GetWebSiteConfig(object[] objParam)
        {
            string xpath = objParam[0].ToString();
            string configFile = string.Format("{0}\\Web.config", EEPRegistry.WebClient);
            if (File.Exists(configFile))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(configFile);

                XmlNode nFlowFilesBySolutions = null;
                if (!string.IsNullOrEmpty(doc.DocumentElement.NamespaceURI))
                {
                    XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
                    nsmgr.AddNamespace("wf", doc.DocumentElement.NamespaceURI);
                    nFlowFilesBySolutions = doc.DocumentElement.SelectSingleNode(string.Format(xpath, "wf:"), nsmgr);
                }
                else
                {
                    nFlowFilesBySolutions = doc.DocumentElement.SelectSingleNode(string.Format(xpath, string.Empty));
                }

                if (nFlowFilesBySolutions != null)
                {
                    return new object[] { 0, string.Format("{0}\\", EEPRegistry.WebClient), string.Compare(nFlowFilesBySolutions.Attributes["value"].Value, "true", true) == 0 };
                }
            }
            return null;
        }

        public object[] DeleteWorkFlowAttachFile(object[] objParam)
        {
            string filename = objParam[0].ToString();
            string solname = objParam[1].ToString();
            string webclient = EEPRegistry.WebClient;
            string configFile = string.Format(@"{0}\Web.config", webclient);
            if (File.Exists(configFile) && !string.IsNullOrEmpty(filename))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(configFile);

                XmlNode nFlowFilesBySolutions = null;
                if (!string.IsNullOrEmpty(doc.DocumentElement.NamespaceURI))
                {
                    XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
                    nsmgr.AddNamespace("wf", doc.DocumentElement.NamespaceURI);
                    nFlowFilesBySolutions = doc.DocumentElement.SelectSingleNode("wf:appSettings/wf:add[@key='FlowFilesBySolutions']", nsmgr);
                }
                else
                {
                    nFlowFilesBySolutions = doc.DocumentElement.SelectSingleNode("appSettings/add[@key='FlowFilesBySolutions']");
                }
                if (nFlowFilesBySolutions != null)
                {
                    bool bySol = (string.Compare(nFlowFilesBySolutions.Attributes["value"].Value, "true", true) == 0);
                    string srvPath = string.Format(@"{0}\WorkflowFiles\{1}", webclient, filename);
                    if (bySol)
                    {
                        srvPath = string.Format(@"{0}\WorkflowFiles\{1}\{2}", webclient, solname, filename);
                    }
                    File.Delete(srvPath);
                    return new object[] { 0, srvPath };
                }
            }
            return null;
        }

        public object[] GetLoginFile(object[] objParam)
        {
            try
            {
                if (File.Exists(SystemFile.LoginFile))
                {
                    FileInfo fileInfo = new FileInfo(SystemFile.LoginFile);
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(SystemFile.LoginFile);
                    if (xmlDoc.SelectSingleNode("InfolightAllowUserToPerLogin") != null && xmlDoc.SelectSingleNode("InfolightAllowUserToPerLogin").SelectSingleNode("PasswordPolicy") != null)
                    {
                        return new object[] { 0, xmlDoc.SelectSingleNode("InfolightAllowUserToPerLogin").SelectSingleNode("PasswordPolicy").OuterXml };
                    }
                    else
                        return new object[] { 0, "" };
                }
            }
            catch (Exception e)
            {
                return new object[] { 1, e.Message };
            }

            return new object[] { 0, "0" };
        }

        //ILogin不用
        public object[] ChangePassword(object[] objParam)
        {
            string XMLParams = objParam[0].ToString();
            string[] ss = XMLParams.Split(":".ToCharArray());
            string sUserId = ss[0];

            //MD5 md5 = new MD5CryptoServiceProvider();

            //byte[] pwdBytes = md5.ComputeHash(Encoding.ASCII.GetBytes(ss[1]));
            //byte[] result = md5.ComputeHash(pwdBytes);
            //string sOldPwd = BitConverter.ToString(result);

            //pwdBytes = md5.ComputeHash(Encoding.ASCII.GetBytes(ss[2]));
            //result = md5.ComputeHash(pwdBytes);
            //string sNewPwd = BitConverter.ToString(result);
            if (ss[1].Length > 10 || ss[2].Length > 10)
            {
                return new object[] { 0, "E" };
            }

            char[] p = new char[] { };
            bool q;
            if (ss[1] != "") q = Encrypt.EncryptPassword(sUserId, ss[1], 10, ref p, false);
            string sOldPwd = new string(p);

            p = new char[] { };
            if (ss[2] != "") q = Encrypt.EncryptPassword(sUserId, ss[2], 10, ref p, false);
            string sNewPwd = new string(p);

            #region Build the command and exec the command to log error

            ClientType ct = ClientType.ctNone;
            IDbConnection conn = AllocateConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), ref ct, true);
            try
            {
                String sQL = "";

                //为了区分不同的数据库 by Rei
                InfoCommand command = new InfoCommand(ClientInfo);
                if (conn.GetType().ToString() == "System.Data.SqlClient.SqlConnection")
                {
                    if (ss[1] != "") sQL = "update USERS set PWD = @NewPwd where USERID = @UserId and PWD = @OldPwd";
                    else sQL = "update USERS set PWD = @NewPwd where USERID = @UserId and (PWD = '' or PWD is null)";
                }
                else if (conn.GetType().ToString() == "System.Data.OracleClient.OracleConnection")
                {
                    if (ss[1] != "") sQL = "update USERS set PWD = :NewPwd where USERID = :UserId and PWD = :OldPwd";
                    else sQL = "update USERS set PWD = :NewPwd where USERID = :UserId and (PWD = '' or PWD is null)";
                }
                else if (conn.GetType().ToString() == "System.Data.Odbc.OdbcConnection")
                {
                    if (ss[1] != "") sQL = "update USERS set PWD =? where USERID =? and PWD =?";
                    else sQL = "update USERS set PWD =? where USERID =? and (PWD = '' or PWD is null)";

                    //if (ss[1] != "") sQL = "update USERS set PWD = $NewPwd where USERID = $UserId and PWD = $OldPwd";
                    //else sQL = "update USERS set PWD = $NewPwd where USERID = $UserId and PWD = ''";
                }
                else if (conn.GetType().ToString() == "System.Data.OleDb.OleDbConnection")
                {
                    if (ss[1] != "") sQL = "update USERS set PWD = ? where USERID = ? and PWD = ?";
                    else sQL = "update USERS set PWD = ? where USERID = ? and (PWD = '' or PWD is null)";
                }
                else if (conn.GetType().Name == "MySqlConnection")
                {
                    if (ss[1] != "") sQL = "update USERS set PWD = @NewPwd where USERID = @UserId and PWD = @OldPwd";
                    else sQL = "update USERS set PWD = @NewPwd where USERID = @UserId and (PWD = '' or PWD is null)";
                }
                else if (conn.GetType().Name == "IfxConnection")
                {
                    if (ss[1] != "") sQL = "update USERS set PWD =? where USERID =? and PWD =?";
                    else sQL = "update USERS set PWD =? where USERID =? and (PWD = '' or PWD is null)";
                }

                command.CommandText = sQL;
                command.Connection = conn;

                DbParameter paramNewPwd = null;
                DbParameter paramUserId = null;
                DbParameter paramOldPwd = null;

                if (conn is OleDbConnection)
                {
                    paramNewPwd = new OleDbParameter("@NewPwd", OleDbType.VarChar, 10);
                    paramUserId = new OleDbParameter("@UserId", OleDbType.VarChar, 20);
                    paramOldPwd = new OleDbParameter("@OldPwd", OleDbType.VarChar, 10);
                }
                else if (conn is OdbcConnection)
                {
                    paramNewPwd = new OdbcParameter("?", OdbcType.NVarChar, 10);
                    paramUserId = new OdbcParameter("?", OdbcType.VarChar, 20);
                    paramOldPwd = new OdbcParameter("?", OdbcType.NVarChar, 10);
                }
                else if (conn is OracleConnection)
                {
                    paramNewPwd = new OracleParameter(":NewPwd", OracleType.NVarChar, 10);
                    paramUserId = new OracleParameter(":UserId", OracleType.VarChar, 20);
                    paramOldPwd = new OracleParameter(":OldPwd", OracleType.NVarChar, 10);
                }
                else if (conn is SqlConnection)
                {
                    paramNewPwd = new SqlParameter("@NewPwd", SqlDbType.VarChar, 10);
                    paramUserId = new SqlParameter("@UserId", SqlDbType.VarChar, 20);
                    paramOldPwd = new SqlParameter("@OldPwd", SqlDbType.VarChar, 10);
                }
#if MySql
                else if (conn is MySqlConnection)
                {
                    paramNewPwd = new MySqlParameter("@NewPwd", MySqlDbType.VarChar, 10);
                    paramUserId = new MySqlParameter("@UserId", MySqlDbType.VarChar, 20);
                    paramOldPwd = new MySqlParameter("@OldPwd", MySqlDbType.VarChar, 10);
                }
#endif
#if Informix
                else if (conn is IBM.Data.Informix.IfxConnection)
                {
                    paramNewPwd = new IBM.Data.Informix.IfxParameter("?", IBM.Data.Informix.IfxType.VarChar, 10);
                    paramUserId = new IBM.Data.Informix.IfxParameter("?", IBM.Data.Informix.IfxType.VarChar, 20);
                    paramOldPwd = new IBM.Data.Informix.IfxParameter("?", IBM.Data.Informix.IfxType.VarChar, 10);
                }
#endif
                paramNewPwd.Value = sNewPwd;
                paramUserId.Value = sUserId;
                paramOldPwd.Value = sOldPwd;

                if (!(conn is OdbcConnection))
                {
                    command.Parameters.Add(paramNewPwd);
                    command.Parameters.Add(paramUserId);
                    if (ss[1] != "")
                        command.Parameters.Add(paramOldPwd);

                }

                Int32 i = command.ExecuteNonQuery();

                if (i != 1)
                {
                    return new object[] { 0, "E" };  //By Lily 表示没有找到该User，可能是UserID错误，或者原始密码错误。
                }
                else
                {
                    //验证密码有效期 by rei
                    command.Parameters.Clear();
                    command.CommandText = "UPDATE USERS SET LASTDATE='" + DateTime.Today.Year.ToString() + DateTime.Today.Month.ToString("00") + DateTime.Today.Day.ToString("00") + "' WHERE USERID='" + sUserId + "'";
                    command.ExecuteNonQuery();

                    return new object[] { 0, "O" };  //找到该User，并更新好新密码。
                }
            }
            finally
            {
                ReleaseConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), conn, true);
            }
            #endregion
        }

        public char[] EncryptPassword(string UserID, char[] Password, int PasswordLen)
        {
            char[] RetPassword = new char[] { };

            int KeyOffset = -1;
            int StartKey = -1;
            int MultKey = -1;
            int AddKey = -1;
            char EncryptChar = ' ';

            int RealPassLen = -1;
            byte b = new byte();
            bool bHasEncryptChar = false;
            int offset = -1;
            int c = -1;
            EncryptByte(KeyOffset, StartKey, MultKey, AddKey, b);

            if (PasswordLen < 5)
            {

            }

            if (Password.Length > PasswordLen)
            {
                return RetPassword;
            }

            for (int i = 1; i <= Password.Length; i++)
            {
                if (Password[i] == ' ')
                {
                    return RetPassword;
                }
                else if (Password[i] == EncryptChar)
                {
                    return RetPassword;
                }
                else if (((byte)Password[i]) < 33 || ((byte)Password[i]) > 126)
                {
                    return RetPassword;
                }
            }

            KeyOffset = 32 + PasswordLen;
            StartKey = 1234;
            MultKey = 12674;
            AddKey = 35891;

            UserID = UserID.Trim().ToUpper();
            for (int i = 1; i <= UserID.Length; i++)
            {
                KeyOffset += (((byte)UserID[i]) + 2) * i;
            }

            for (int i = 1; i <= Password.Length; i++)
            {
                KeyOffset += (((byte)Password[i]) + 2) * i;
            }

            RealPassLen = Password.Length;
            for (int i = RealPassLen + 1; i <= PasswordLen; i++)
            {
                Password[i] = '0';
            }

            RetPassword = new char[] { };

            if (PasswordLen % 2 == 0)
            {
                offset = 2;
            }
            else
            {
                offset = 1;
            }

            bHasEncryptChar = false;
            for (int i = 1; i <= PasswordLen; i++)
            {
                b = (byte)Password[i];
                b = EncryptByte(KeyOffset, StartKey, MultKey, AddKey, b);
                if (b == (byte)EncryptChar)
                {
                    bHasEncryptChar = true;
                }

                if (i % 2 == 0)
                {
                    RetPassword[PasswordLen + offset - i] = (char)b;
                }
                else
                {
                    RetPassword[i] = (char)b;
                }
            }

            if (!bHasEncryptChar)
            {
                if (RealPassLen == 0)
                {
                    RealPassLen = 1;
                }

                offset = (byte)EncryptChar - (byte)RetPassword[RealPassLen];
                for (int i = 1; i <= PasswordLen; i++)
                {
                    c = (byte)RetPassword[i] + offset;
                    while (c > 126 || c < 33)
                    {
                        if (c > 126)
                        {
                            c = c - 127 + 33;
                        }
                        else
                        {
                            c = 126 - (32 - c);
                        }
                    }
                    RetPassword[i] = (char)c;
                }
            }

            return RetPassword;
        }

        public byte EncryptByte(int KeyOffset, int StartKey, int MultKey, int AddKey, byte b)
        {
            int Result;
            do
            {
                Result = (b ^ ((StartKey >> 8) + KeyOffset)) + 33;
                StartKey = (Result + StartKey) * MultKey + AddKey;
                Result = Result & 127;
            }
            while (Result < 33 || Result > 126);
            return (byte)Result;
        }

        public object LogOut(object[] objParam)
        {
            string XMLParams = objParam[0].ToString();
            string sUserId = XMLParams;

            SrvGL.LogUser(sUserId.ToLower(), string.Empty, string.Format("{0}", GetClientInfo(ClientInfoType.ComputerName)), -1);
            //ServerConfig.UserLoginCount--;
            //if (ServerConfig.UserLoginCount < 0)
            //{
            //    ServerConfig.UserLoginCount = 0;
            //}

            return new object[] { 0 };
        }

        public object[] FLOvertimeList(object[] objParam)
        {
            ClientType ct = ClientType.ctMsSql;
            IDbConnection nwindConn = AllocateConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), ref ct, true);
            try
            {
                //获取本人的Roles
                List<string> lstRoles = new List<string>();
                string currentUser = objParam[0].ToString();
                string sqlCurRoles = "select GROUPID from USERGROUPS where " + (string.IsNullOrEmpty(currentUser) ? "" : "USERID = '" + currentUser + "' and ") + "GROUPID in (select GROUPID from GROUPS where ISROLE = 'Y')";
                InfoCommand cmdCurRoles = new InfoCommand(ClientInfo);
                cmdCurRoles.Connection = nwindConn;
                cmdCurRoles.CommandText = sqlCurRoles;
                IDataReader drCurRoles = cmdCurRoles.ExecuteReader();
                while (drCurRoles.Read())
                {
                    lstRoles.Add(drCurRoles["GROUPID"].ToString());
                }
                cmdCurRoles.Cancel();
                drCurRoles.Close();

                List<string> lstOrgs = new List<string>();

                int level = Convert.ToInt16(objParam[1]);
                for (int i = 0; i < level; i++)
                {
                    string orgMans = "";
                    foreach (string man in lstRoles)
                    {
                        orgMans += "'" + man + "',";
                    }
                    if (orgMans.IndexOf(',') != -1)
                        orgMans = orgMans.Substring(0, orgMans.LastIndexOf(','));
                    string sqlOrg = string.IsNullOrEmpty(orgMans) ? "select ORG_NO from SYS_ORG where ORG_MAN IS NULL" : "select ORG_NO from SYS_ORG where ORG_MAN in (" + orgMans + ")";
                    InfoCommand cmdOrg = new InfoCommand(getConnectionType(nwindConn), ClientInfo);
                    cmdOrg.Connection = nwindConn;
                    cmdOrg.CommandText = sqlOrg;
                    IDataReader drOrg = cmdOrg.ExecuteReader();
                    while (drOrg.Read())
                    {
                        string org = drOrg["ORG_NO"].ToString();
                        if (!lstOrgs.Contains(org))
                            lstOrgs.Add(org);
                    }
                    cmdOrg.Cancel();
                    drOrg.Close();

                    string upperOrgs = "";
                    foreach (string org in lstOrgs)
                    {
                        upperOrgs += "'" + org + "',";
                    }
                    if (upperOrgs.IndexOf(',') != -1)
                        upperOrgs = upperOrgs.Substring(0, upperOrgs.LastIndexOf(','));
                    string sqlOrgManRole = string.IsNullOrEmpty(upperOrgs) ? "select ORG_MAN from SYS_ORG where UPPER_ORG IS NULL" : "select ORG_MAN from SYS_ORG where UPPER_ORG in (" + upperOrgs + ")";
                    InfoCommand cmdOrgManRole = new InfoCommand(getConnectionType(nwindConn), ClientInfo);
                    cmdOrgManRole.Connection = nwindConn;
                    cmdOrgManRole.CommandText = sqlOrgManRole;
                    IDataReader drOrgManRole = cmdOrgManRole.ExecuteReader();
                    while (drOrgManRole.Read())
                    {
                        string orgMan = drOrgManRole["ORG_MAN"].ToString();
                        if (!lstRoles.Contains(orgMan))
                            lstRoles.Add(orgMan);
                    }
                    cmdOrgManRole.Cancel();
                    drOrgManRole.Close();
                }

                string roles = "";
                foreach (string role in lstRoles)
                {
                    roles += "'" + role + "',";
                }
                if (roles.IndexOf(',') != -1)
                    roles = roles.Substring(0, roles.LastIndexOf(','));
                bool delay = Convert.ToBoolean(objParam[4]);
                //joy 2010/1/11 modify : 增加 ATTACHMENTS,MULTISTEPRETURN,PARAMETERS 欄位,因為逾時需要用到這些欄位
                string sqlTodolist = "SELECT " + (delay ? "LISTID, FLOW_ID, FLOW_DESC, APPLICANT, S_USER_ID, S_STEP_ID, S_STEP_DESC, D_STEP_ID, D_STEP_DESC, EXP_TIME, URGENT_TIME, TIME_UNIT, USERNAME, FORM_NAME, NAVIGATOR_MODE, FLNAVIGATOR_MODE, PARAMETERS, SENDTO_KIND, SENDTO_ID, FLOWIMPORTANT, FLOWURGENT, STATUS, FORM_TABLE, FORM_KEYS, FORM_PRESENTATION, FORM_PRESENT_CT, REMARK, PROVIDER_NAME, VERSION, EMAIL_ADD, EMAIL_STATUS, VDSNAME, SENDBACKSTEP, LEVEL_NO, WEBFORM_NAME, UPDATE_DATE, UPDATE_TIME, FLOWPATH, PLUSAPPROVE, PLUSROLES, ATTACHMENTS, MULTISTEPRETURN, PARAMETERS" : "FLOW_DESC, TIME_UNIT, FLOWURGENT, UPDATE_DATE, UPDATE_TIME, URGENT_TIME, EXP_TIME") + " from SYS_TODOLIST where " + (string.IsNullOrEmpty(roles) ? "1=0" : ("(SENDTO_ID in (" + roles + ")  and SENDTO_KIND='1') or (SENDTO_ID ='" + currentUser + "' and SENDTO_KIND='2')")) + (delay ? " ORDER BY UPDATE_DATE" : " ORDER BY FLOW_DESC");
                InfoCommand cmdTodolist = new InfoCommand(getConnectionType(nwindConn), ClientInfo);
                cmdTodolist.Connection = nwindConn;
                cmdTodolist.CommandText = sqlTodolist;
                IDataAdapter adpater = DBUtils.CreateDbDataAdapter(cmdTodolist);

                DataTable allList = new DataTable();
                (adpater as DbDataAdapter).Fill(allList);

                if (delay)
                {
                    DataColumn colSendToDetail = new DataColumn("SENDTO_DETAIL", typeof(string), "SENDTO_ID+'('+USERNAME+')'");
                    DataColumn colUpdateWholeTime = new DataColumn("UPDATE_WHOLE_TIME", typeof(string), "UPDATE_DATE + ' ' + UPDATE_TIME");
                    DataColumn colOverTime = new DataColumn("OVERTIME", typeof(string));
                    allList.Columns.AddRange(new DataColumn[] { colSendToDetail, colUpdateWholeTime, colOverTime });
                }
                List<DataRow> overTimeRows = new List<DataRow>();
                #region find over time
                foreach (DataRow row in allList.Rows)
                {
                    string TIME_UNIT = row["TIME_UNIT"].ToString();
                    string FLOWURGENT = row["FLOWURGENT"].ToString();
                    string UPDATE_DATE = row["UPDATE_DATE"].ToString();
                    string UPDATE_TIME = row["UPDATE_TIME"].ToString();
                    string URGENT_TIME = row["URGENT_TIME"].ToString();
                    string EXP_TIME = row["EXP_TIME"].ToString();

                    if (TIME_UNIT == "Day" && FLOWURGENT == "1")
                    {
                        if (Convert.ToDecimal(URGENT_TIME) == Decimal.Zero) continue;
                        TimeSpan span = this.WorkTimeSpan(DateTime.Now.Date, Convert.ToDateTime(UPDATE_DATE), (bool)objParam[2], (objParam[3] == null) ? null : (List<string>)objParam[3]);

                        int overtimes = span.Days - Convert.ToInt32(Convert.ToDecimal(URGENT_TIME));
                        if (delay) row["OVERTIME"] = overtimes.ToString() + "Days";
                        if (overtimes >= 0)
                        {
                            overTimeRows.Add(row);
                        }
                    }
                    else if (TIME_UNIT == "Day" && FLOWURGENT == "0")
                    {
                        if (Convert.ToDecimal(EXP_TIME) == Decimal.Zero) continue;
                        TimeSpan span = this.WorkTimeSpan(DateTime.Now.Date, Convert.ToDateTime(UPDATE_DATE), (bool)objParam[2], (objParam[3] == null) ? null : (List<string>)objParam[3]);
                        int overtimes = span.Days - Convert.ToInt32(Convert.ToDecimal(EXP_TIME));
                        if (delay) row["OVERTIME"] = overtimes.ToString() + "Days";
                        if (overtimes >= 0)
                        {
                            overTimeRows.Add(row);
                        }
                    }
                    else if (TIME_UNIT == "Hour" && FLOWURGENT == "1")
                    {
                        if (Convert.ToDecimal(URGENT_TIME) == Decimal.Zero) continue;
                        TimeSpan spanDay = this.WorkTimeSpan(DateTime.Now.Date, Convert.ToDateTime(UPDATE_DATE), (bool)objParam[2], (objParam[3] == null) ? null : (List<string>)objParam[3]);
                        int spanHour = DateTime.Now.Hour - Convert.ToDateTime(UPDATE_TIME).Hour;
                        int overtimes = spanDay.Days * 8 + spanHour - Convert.ToInt32(Convert.ToDecimal(URGENT_TIME));
                        if (delay) row["OVERTIME"] = overtimes.ToString() + "Hours";
                        if (overtimes >= 0)
                        {
                            overTimeRows.Add(row);
                        }
                    }
                    else if (TIME_UNIT == "Hour" && FLOWURGENT == "0")
                    {
                        if (Convert.ToDecimal(EXP_TIME) == Decimal.Zero) continue;
                        TimeSpan spanDay = this.WorkTimeSpan(DateTime.Now.Date, Convert.ToDateTime(UPDATE_DATE), (bool)objParam[2], (objParam[3] == null) ? null : (List<string>)objParam[3]);
                        int spanHour = DateTime.Now.Hour - Convert.ToDateTime(UPDATE_TIME).Hour;
                        int overtimes = spanDay.Days * 8 + spanHour - Convert.ToInt32(Convert.ToDecimal(EXP_TIME));
                        if (delay) row["OVERTIME"] = overtimes.ToString() + "Hours";
                        if (overtimes >= 0)
                        {
                            overTimeRows.Add(row);
                        }
                    }
                }
                #endregion

                DataTable overtimeList = null;
                if (delay)
                {
                    overtimeList = allList.Clone();
                    foreach (DataRow row in overTimeRows)
                    {
                        overtimeList.ImportRow(row);
                    }
                }
                else
                {
                    overtimeList = new DataTable();
                    DataColumn colFlowDesc = new DataColumn("FLOW_DESC", typeof(string));
                    DataColumn colDelayCount = new DataColumn("DELAY_COUNT", typeof(int));
                    overtimeList.Columns.AddRange(new DataColumn[] { colFlowDesc, colDelayCount });

                    string desc = "";
                    foreach (DataRow row in overTimeRows)
                    {
                        if (desc != row["FLOW_DESC"].ToString())
                        {
                            desc = row["FLOW_DESC"].ToString();
                            int count = overTimeRows.FindAll(delegate(DataRow irow) { return irow["FLOW_DESC"].ToString() == desc; }).Count;
                            DataRow newRow = overtimeList.NewRow();
                            newRow["FLOW_DESC"] = desc;
                            newRow["DELAY_COUNT"] = count;
                            overtimeList.Rows.Add(newRow);
                        }
                    }
                }
                return new object[] { 0, overtimeList };
            }
            finally
            {
                ReleaseConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), nwindConn, true);
            }
        }

        private TimeSpan WorkTimeSpan(DateTime nowTime, DateTime updateTime, bool weekendSensible, List<string> extDates)
        {
            TimeSpan span = new TimeSpan();
            if (weekendSensible)
            {
                if (nowTime.DayOfWeek == DayOfWeek.Saturday)
                {
                    nowTime = nowTime.Date.AddSeconds(-1);
                }
                else if (nowTime.DayOfWeek == DayOfWeek.Sunday)
                {
                    nowTime = nowTime.Date.AddDays(-1).AddSeconds(-1);
                }

                if (updateTime.DayOfWeek == DayOfWeek.Saturday)
                {
                    updateTime = updateTime.Date.AddDays(2);
                }
                else if (updateTime.DayOfWeek == DayOfWeek.Sunday)
                {
                    updateTime = updateTime.Date.AddDays(1);
                }
            }
            span = nowTime - updateTime;
            if (weekendSensible)
            {
                int weekends = span.Days / 7;
                int i = nowTime.DayOfWeek - updateTime.DayOfWeek;
                if (i < 0)
                    weekends++;
                span = span.Subtract(new TimeSpan(2 * weekends, 0, 0, 0));
            }
            int extDays = 0;
            if (extDates == null) return span;
            foreach (string extDate in extDates)
            {
                if (Convert.ToDateTime(extDate).CompareTo(nowTime) < 0
                    && Convert.ToDateTime(extDate).CompareTo(updateTime) > 0)
                {
                    if (weekendSensible)
                    {
                        if (Convert.ToDateTime(extDate).DayOfWeek != DayOfWeek.Saturday
                            && Convert.ToDateTime(extDate).DayOfWeek != DayOfWeek.Sunday)
                        {
                            extDays++;
                        }
                    }
                    else
                    {
                        extDays++;
                    }
                }
            }
            span = span.Subtract(new TimeSpan(extDays, 0, 0, 0));
            return span;
        }

        public object[] FetchMenus(object[] objParam)
        {
            ClientType ct = ClientType.ctMsSql;
            string fLoginUser = GetClientInfo(ClientInfoType.LoginUser).ToString(); ;
            IDbConnection nwindConn = AllocateConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), ref ct, true);
            try
            {
                string groupids = "";
                if (((object[])ClientInfo[0]).Length > 18 && ((object[])ClientInfo[0])[18] != null && ((object[])ClientInfo[0])[18].ToString() != "")
                {
                    groupids = "'" + ((object[])ClientInfo[0])[18].ToString() + "'";
                }
                else
                {
                    if (ServerConfig.LoginObjectEnabled)
                    {
                        string[] groups = ServerConfig.LoginObject.GetUserGroups(fLoginUser, (string)GetClientInfo(ClientInfoType.Password));
                        foreach (string group in groups)
                        {
                            if (groupids.Length > 0)
                            {
                                groupids += ",";
                            }
                            groupids += string.Format("'{0}'", group);
                        }
                        if (groupids.Length == 0)
                        {
                            groupids = "''";
                        }
                    }
                    else
                    {
                        groupids = "select GROUPID from USERGROUPS where USERID = '" + fLoginUser + "'";
                    }
                }
                //string strSqlGetMenu = "select * from menutable where moduletype = '" + objParam[1].ToString()
                //            + "' and menuid in (select menuid from groupmenus where"
                //            + " groupid in (select groupid from usergroups where userid = '" + fLoginUser
                //            + "') or groupid = '00') and itemtype in (select itemtype from MENUITEMTYPE where itemtype = '"
                //            + objParam[0].ToString() + "') or (isnull(parent, '') = '' and isnull(package, '') = '')";
                string strSqlGetMenu = "";
                //Oracle里用没有isnull函数
                if (nwindConn is SqlConnection)
                    //strSqlGetMenu = "select * from MENUTABLE where MODULETYPE = '" + objParam[1].ToString() + "' "
                    //           + "and ITEMTYPE in (select ITEMTYPE from MENUITEMTYPE where ITEMTYPE = '" + objParam[0].ToString() + "')"
                    //           + " and (MENUID in (select MENUID from GROUPMENUS where GROUPID in (select GROUPID from USERGROUPS where USERID = '" + fLoginUser + "') or GROUPID = '00') "
                    //           + " or MENUID in (select MENUID from USERMENUS where USERID = '" + fLoginUser + "')"
                    //           + " or (isnull(PARENT, '') = '' and isnull(PACKAGE, '') = '')) order by SEQ_NO";
                    strSqlGetMenu = "select * from MENUTABLE where MODULETYPE in ('" + objParam[1].ToString() + "', 'O') "
                               + "and ITEMTYPE in (select ITEMTYPE from MENUITEMTYPE where ITEMTYPE = '" + objParam[0].ToString() + "')"
                               + " and (MENUID in (select MENUID from GROUPMENUS where GROUPID in (" + groupids + ") or GROUPID = '00')"
                               + " or MENUID in (select MENUID from USERMENUS where USERID = '" + fLoginUser + "')"
                               + " or (isnull(PARENT, '') = '' and isnull(PACKAGE, '') = '')) order by SEQ_NO";
                else if (nwindConn is OracleConnection)
                    strSqlGetMenu = "select * from MENUTABLE where MODULETYPE in ('" + objParam[1].ToString() + "', 'O') "
                               + "and ITEMTYPE in (select ITEMTYPE from MENUITEMTYPE where ITEMTYPE = '" + objParam[0].ToString() + "')"
                               + " and (MENUID in (select MENUID from GROUPMENUS where GROUPID in (" + groupids + ") or GROUPID = '00')"
                               + " or MENUID in (select MENUID from USERMENUS where USERID = '" + fLoginUser + "')"
                               + " or (nvl(PARENT, '') = '' and nvl(PACKAGE, '') = '')) order by SEQ_NO";
                else if (nwindConn is OdbcConnection)
                    strSqlGetMenu = "select * from MENUTABLE where MODULETYPE in ('" + objParam[1].ToString() + "', 'O') "
                                              + "and ITEMTYPE in (select ITEMTYPE from MENUITEMTYPE where ITEMTYPE = '" + objParam[0].ToString() + "')"
                                              + " and (MENUID in (select MENUID from GROUPMENUS where GROUPID in (" + groupids + ") or GROUPID = '00')"
                                              + " or MENUID in (select MENUID from USERMENUS where USERID = '" + fLoginUser + "')"
                                              + " or (nvl(PARENT, ' ') = ' ' and nvl(PACKAGE, ' ') = ' ')) order by SEQ_NO";
                else if (nwindConn is OleDbConnection)
                    strSqlGetMenu = "select * from MENUTABLE where MODULETYPE in ('" + objParam[1].ToString() + "', 'O') "
                               + "and ITEMTYPE in (select ITEMTYPE from MENUITEMTYPE where ITEMTYPE = '" + objParam[0].ToString() + "')"
                               + " and (MENUID in (select MENUID from GROUPMENUS where GROUPID in (" + groupids + ") or GROUPID = '00')"
                               + " or MENUID in (select MENUID from USERMENUS where USERID = '" + fLoginUser + "')"
                               + " or (isnull(PARENT, '') = '' and isnull(PACKAGE, '') = '')) order by SEQ_NO";
                else if (nwindConn.GetType().Name == "MySqlConnection")
                    strSqlGetMenu = "select * from MENUTABLE where MODULETYPE in ('" + objParam[1].ToString() + "', 'O') "
                               + "and ITEMTYPE in (select ITEMTYPE from MENUITEMTYPE where ITEMTYPE = '" + objParam[0].ToString() + "')"
                               + " and (MENUID in (select MENUID from GROUPMENUS where GROUPID in (" + groupids + ") or GROUPID = '00')"
                               + " or MENUID in (select MENUID from USERMENUS where USERID = '" + fLoginUser + "')"
                               + " or (ifnull(PARENT, '') = '' and ifnull(PARENT, '') = '')) order by SEQ_NO";
                else if (nwindConn.GetType().Name == "IfxConnection")
                    strSqlGetMenu = "select * from MENUTABLE where MODULETYPE in ('" + objParam[1].ToString() + "', 'O') "
                                              + "and ITEMTYPE in (select ITEMTYPE from MENUITEMTYPE where ITEMTYPE = '" + objParam[0].ToString() + "')"
                                              + " and (MENUID in (select MENUID from GROUPMENUS where GROUPID in (" + groupids + ") or GROUPID = '00')"
                                              + " or MENUID in (select MENUID from USERMENUS where USERID = '" + fLoginUser + "')"
                                              + " or (nvl(PARENT, ' ') = ' ' and nvl(PACKAGE, ' ') = ' ')) order by SEQ_NO";

                //为了区分不同的数据库 by Rei
                InfoCommand myCommand = new InfoCommand(ClientInfo);

                myCommand.Connection = nwindConn;
                myCommand.CommandText = strSqlGetMenu;
                IDataAdapter adpater = DBUtils.CreateDbDataAdapter(myCommand);

                DataSet dsGetMenu = new DataSet();
                DataTable dt = new DataTable("menuInfo");
                (adpater as DbDataAdapter).Fill(dt);
                dsGetMenu.Tables.Add(dt);

                return new object[] { 0, dsGetMenu };
            }
            finally
            {
                ReleaseConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), nwindConn, true);
            }
        }

        public object[] FetchFavorMenus(object[] objParam)
        {
            String moduleType = "'" + objParam[1].ToString() + "'";
            if (objParam[1].ToString() == "W")
            {
                moduleType = "'W', 'C'";
            }
            ClientType ct = ClientType.ctMsSql;
            string fLoginUser = GetClientInfo(ClientInfoType.LoginUser).ToString();
            IDbConnection nwindConn = AllocateConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), ref ct, true);
            try
            {
                string strSqlGetMenu = "";
                if (nwindConn is SqlConnection)
                    strSqlGetMenu = "select MENUTABLE.*, MENUFAVOR.GROUPNAME from MENUTABLE LEFT JOIN MENUFAVOR ON MENUTABLE.MENUID=MENUFAVOR.MENUID AND USERID='" + fLoginUser + "' where MENUTABLE.MODULETYPE in (" + moduleType + ", 'O') "
                                + "AND MENUTABLE.ITEMTYPE in (select ITEMTYPE from MENUITEMTYPE where ITEMTYPE ='" + objParam[0].ToString() + "')"
                                + " AND (MENUTABLE.MENUID in (Select MENUID From MENUFAVOR Where USERID='" + fLoginUser + "' and MENUTABLE.MENUID in (SELECT MENUID FROM USERMENUS WHERE USERID='" + fLoginUser + "'"
                                + " UNION SELECT  DISTINCT  MENUID FROM GROUPMENUS WHERE GROUPID IN (SELECT GROUPID FROM USERGROUPS WHERE USERID='" + fLoginUser + "') OR GROUPID='00'))"
                                + ")  order by SEQ_NO";
                else if (nwindConn is OracleConnection)
                    strSqlGetMenu = "select MENUTABLE.*, MENUFAVOR.GROUPNAME from MENUTABLE LEFT JOIN MENUFAVOR ON MENUTABLE.MENUID=MENUFAVOR.MENUID AND USERID='" + fLoginUser + "' where MENUTABLE.MODULETYPE in (" + moduleType + ") "
                                + "AND MENUTABLE.ITEMTYPE in (select ITEMTYPE from MENUITEMTYPE where ITEMTYPE ='" + objParam[0].ToString() + "')"
                                + " AND (MENUTABLE.MENUID in (Select MENUID From MENUFAVOR Where USERID='" + fLoginUser + "' and MENUTABLE.MENUID in (SELECT MENUID FROM USERMENUS WHERE USERID='" + fLoginUser + "'"
                                + " UNION SELECT  DISTINCT  MENUID FROM GROUPMENUS WHERE GROUPID IN (SELECT GROUPID FROM USERGROUPS WHERE USERID='" + fLoginUser + "') OR GROUPID='00'))"
                                + ") AND MENUFAVOR.USERID = '" + fLoginUser + "' order by MENUTABLE.CAPTION";
                else if (nwindConn is OdbcConnection)
                    strSqlGetMenu = "select MENUTABLE.*, MENUFAVOR.GROUPNAME from MENUTABLE LEFT JOIN MENUFAVOR ON MENUTABLE.MENUID=MENUFAVOR.MENUID AND USERID='" + fLoginUser + "' where MENUTABLE.MODULETYPE in (" + moduleType + ", 'O') "
                                + "AND MENUTABLE.ITEMTYPE ='" + objParam[0].ToString() + "' "
                                + " AND (MENUTABLE.MENUID in (Select MENUID From MENUFAVOR Where USERID='" + fLoginUser + "' and (MENUTABLE.MENUID in (SELECT MENUID FROM USERMENUS WHERE USERID='" + fLoginUser + "')"
                                + " OR MENUTABLE.MENUID in (SELECT DISTINCT MENUID FROM GROUPMENUS WHERE GROUPID IN (SELECT GROUPID FROM USERGROUPS WHERE USERID='" + fLoginUser + "') OR GROUPID='00'))"
                                + "))  order by SEQ_NO";
                else if (nwindConn is OleDbConnection)
                    strSqlGetMenu = "select MENUTABLE.*, MENUFAVOR.GROUPNAME from MENUTABLE LEFT JOIN MENUFAVOR ON MENUTABLE.MENUID=MENUFAVOR.MENUID AND USERID='" + fLoginUser + "' where MENUTABLE.MODULETYPE in (" + moduleType + ", 'O') "
                                + "AND MENUTABLE.ITEMTYPE ='" + objParam[0].ToString() + "' "
                                + " AND (MENUTABLE.MENUID in (Select MENUID From MENUFAVOR Where USERID='" + fLoginUser + "' and (MENUTABLE.MENUID in (SELECT MENUID FROM USERMENUS WHERE USERID='" + fLoginUser + "')"
                                + " OR MENUTABLE.MENUID in (SELECT DISTINCT MENUID FROM GROUPMENUS WHERE GROUPID IN (SELECT GROUPID FROM USERGROUPS WHERE USERID='" + fLoginUser + "') OR GROUPID='00'))"
                                + "))  order by SEQ_NO";
                else if (nwindConn.GetType().Name == "MySqlConnection")
                    strSqlGetMenu = "select MENUTABLE.*, MENUFAVOR.GROUPNAME from MENUTABLE LEFT JOIN MENUFAVOR ON MENUTABLE.MENUID=MENUFAVOR.MENUID AND USERID='" + fLoginUser + "' where MENUTABLE.MODULETYPE in (" + moduleType + ", 'O') "
                                + "AND MENUTABLE.ITEMTYPE in (select ITEMTYPE from MENUITEMTYPE where ITEMTYPE ='" + objParam[0].ToString() + "')"
                                + " AND (MENUTABLE.MENUID in (Select MENUID From MENUFAVOR Where USERID='" + fLoginUser + "' and MENUTABLE.MENUID in (SELECT MENUID FROM USERMENUS WHERE USERID='" + fLoginUser + "'"
                                + " UNION SELECT  DISTINCT  MENUID FROM GROUPMENUS WHERE GROUPID IN (SELECT GROUPID FROM USERGROUPS WHERE USERID='" + fLoginUser + "') OR GROUPID='00'))"
                                + ")  order by SEQ_NO";
                else if (nwindConn.GetType().Name == "IfxConnection")
                    strSqlGetMenu = "select MENUTABLE.*, MENUFAVOR.GROUPNAME from MENUTABLE LEFT JOIN MENUFAVOR ON MENUTABLE.MENUID=MENUFAVOR.MENUID AND USERID='" + fLoginUser + "' where MENUTABLE.MODULETYPE in (" + moduleType + ", 'O') "
                                + "AND MENUTABLE.ITEMTYPE ='" + objParam[0].ToString() + "' "
                                + " AND (MENUTABLE.MENUID in (Select MENUID From MENUFAVOR Where USERID='" + fLoginUser + "' and (MENUTABLE.MENUID in (SELECT MENUID FROM USERMENUS WHERE USERID='" + fLoginUser + "')"
                                + " OR MENUTABLE.MENUID in (SELECT DISTINCT MENUID FROM GROUPMENUS WHERE GROUPID IN (SELECT GROUPID FROM USERGROUPS WHERE USERID='" + fLoginUser + "') OR GROUPID='00'))"
                                + "))  order by SEQ_NO";

                //为了区分不同的数据库 by Rei
                InfoCommand myCommand = new InfoCommand(ClientInfo);

                myCommand.Connection = nwindConn;
                myCommand.CommandText = strSqlGetMenu;

                IDataAdapter adpater = DBUtils.CreateDbDataAdapter(myCommand);

                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                (adpater as DbDataAdapter).Fill(dt);
                ds.Tables.Add(dt);

                strSqlGetMenu = "SELECT DISTINCT GROUPNAME FROM MENUFAVOR LEFT JOIN MENUTABLE ON MENUFAVOR.MENUID=MENUTABLE.MENUID WHERE MENUTABLE.MODULETYPE IN (" + moduleType + ") AND MENUFAVOR.USERID='" + fLoginUser + "'";
                myCommand.CommandText = strSqlGetMenu;
                adpater = DBUtils.CreateDbDataAdapter(myCommand);

                DataSet dsGroup = new DataSet();
                DataTable dt1 = new DataTable();
                (adpater as DbDataAdapter).Fill(dt1);
                dsGroup.Tables.Add(dt1);

                return new object[] { 0, ds, dsGroup };
            }
            finally
            {
                ReleaseConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), nwindConn, true);
            }
        }

        public object[] isTableExist(object[] objParam)//这个方法要不要取split system table?
        {
            String tableName = objParam[0].ToString();
            ClientType ct = ClientType.ctMsSql;
            IDbConnection nwindConn = AllocateConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), ref ct, true);
            try
            {
                string strSqlGetMenu = "";
                if (nwindConn is SqlConnection)
                    strSqlGetMenu = "select count(*) from sysobjects where name='" + tableName + "'";
                else if (nwindConn is OracleConnection)
                    strSqlGetMenu = "select count(*) from USER_TABLES where TABLE_NAME='" + tableName + "'";
                else if (ct == ClientType.ctODBC)
                {
                    String sDBAlias = GetClientInfo(ClientInfoType.LoginDB).ToString();
                    var subType = GetDataBaseSubType(new object[] { sDBAlias, false });
                    if (subType[1].ToString() == "2")
                        strSqlGetMenu = "select count(*) from qsys2.SYSTABLES where TABLE_TYPE='T' OR TABLE_TYPE = 'V'";
                    else if (subType[1].ToString() == "0")
                        strSqlGetMenu = "select count(*) from SYSTABLES where (TABTYPE = 'T' or TABTYPE = 'V') and TABID >= 100";
                }
                else if (nwindConn is OleDbConnection)
                    strSqlGetMenu = "select count(*) from sysobjects where name='" + tableName + "'";
                else if (nwindConn.GetType().Name == "MySqlConnection")
                    strSqlGetMenu = "DESC " + tableName;
                else if (nwindConn.GetType().Name == "IfxConnection")
                    strSqlGetMenu = "select count(*) from SYSTABLES where (TABTYPE = 'T' or TABTYPE = 'V') and TABID >= 100";

                InfoCommand myCommand = new InfoCommand(ClientInfo);
                myCommand.Connection = nwindConn;
                myCommand.CommandText = strSqlGetMenu;
                IDataAdapter adpater = DBUtils.CreateDbDataAdapter(myCommand);

                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                (adpater as DbDataAdapter).Fill(dt);
                ds.Tables.Add(dt);
                if (Convert.ToInt16(ds.Tables[0].Rows[0][0]) > 0)
                    return new object[] { 0, 0 };
                else
                    return new object[] { 0, 1 };
            }
            catch
            {
                return new object[] { 0, 1 };
            }
            finally
            {
                ReleaseConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), nwindConn, true);
            }
        }

        public object[] FetchAllMenus(object[] objParam)
        {
            ClientType ct = ClientType.ctMsSql;
            IDbConnection nwindConn = AllocateConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), ref ct, true);
            try
            {
                string strSqlGetMenu = "select * from MENUTABLE where ITEMTYPE in (select ITEMTYPE from MENUITEMTYPE where ITEMTYPE = '" + objParam[0].ToString() + "')"
                            + " and MODULETYPE='" + objParam[1].ToString() + "'"
                            + " order by SEQ_NO";

                //为了区分不同的数据库 by Rei
                InfoCommand myCommand = new InfoCommand(ClientInfo);

                myCommand.Connection = nwindConn;
                myCommand.CommandText = strSqlGetMenu;
                IDataAdapter adpater = DBUtils.CreateDbDataAdapter(myCommand);

                DataSet dsGetMenu = new DataSet();
                DataTable dt = new DataTable("menuInfo");
                (adpater as DbDataAdapter).Fill(dt);
                dsGetMenu.Tables.Add(dt);

                return new object[] { 0, dsGetMenu };
            }
            finally
            {
                ReleaseConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), nwindConn, true);
            }
        }

        public object GetParam(object[] objParam)
        {
            // byte[] buffer = new byte[24];


            ClientType ct = ClientType.ctMsSql;
            IDbConnection nwindConn = AllocateConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), ref ct, true);
            try
            {
                string strCaption = (string)objParam[0];
                string strItemType = (string)objParam[1];
                string captionlanguage = (string)objParam[2];
                string strSql = "select * from MENUTABLE where MENUID = '" + strCaption + "' and ITEMTYPE = '" + strItemType + "'";

                //为了区分不同的数据库 by Rei
                InfoCommand cmd = new InfoCommand(ClientInfo);

                cmd.CommandText = strSql;
                cmd.Connection = nwindConn;
                ArrayList lst = new ArrayList();
                IDataReader dr = cmd.ExecuteReader();

                dr.Read();
                lst.Add(dr["MENUID"].ToString());
                lst.Add(dr["CAPTION" + captionlanguage].ToString());
                lst.Add(dr["PARENT"].ToString());
                lst.Add(dr["MODULETYPE"].ToString());
                lst.Add(dr["IMAGEURL"].ToString());
                lst.Add(dr["PACKAGE"].ToString());
                lst.Add(dr["ITEMPARAM"].ToString());
                lst.Add(dr["FORM"].ToString());
                lst.Add(dr["ITEMTYPE"].ToString());
                lst.Add(dr["SEQ_NO"].ToString());
                cmd.Cancel();
                dr.Close();


                string strBlob = "";
                //为了区分不同的数据库 by Rei
                if (nwindConn is SqlConnection)
                    strBlob = "select [IMAGE] from MENUTABLE where MENUID = '" + strCaption + "' and ITEMTYPE = '" + strItemType + "'";
                else if (nwindConn is OracleConnection)
                    strBlob = "select IMAGE from MENUTABLE where MENUID = '" + strCaption + "' and ITEMTYPE = '" + strItemType + "'";
                else if (nwindConn is OdbcConnection)
                    strBlob = "select IMAGE from MENUTABLE WHERE MENUID = '" + strCaption + "' and ITEMTYPE = '" + strItemType + "'";
                else if (nwindConn is OleDbConnection)
                    strBlob = "select IMAGE from MENUTABLE WHERE MENUID = '" + strCaption + "' and ITEMTYPE = '" + strItemType + "'";
                else if (nwindConn.GetType().Name == "MySqlConnection")
                    strBlob = "select IMAGE from MENUTABLE WHERE MENUID = '" + strCaption + "' and ITEMTYPE = '" + strItemType + "'";
                else if (nwindConn.GetType().Name == "IfxConnection")
                    strBlob = "select IMAGE from MENUTABLE WHERE MENUID = '" + strCaption + "' and ITEMTYPE = '" + strItemType + "'";

                InfoCommand cmd1 = new InfoCommand(ClientInfo);
                cmd1.Connection = nwindConn;
                cmd1.CommandText = strBlob;
                IDataReader idr = cmd1.ExecuteReader();
                idr.Read();

                try
                {
                    byte[] blob = new byte[idr.GetBytes(0, 0, null, 0, int.MaxValue)];
                    idr.GetBytes(0, 0, blob, 0, blob.Length);
                    cmd1.Cancel();
                    idr.Close();
                    return new object[] { 0, lst, blob };
                }
                catch
                {
                    cmd1.Cancel();
                    idr.Close();
                    return new object[] { 0, lst, new byte[1] };
                }
            }
            finally
            {
                ReleaseConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), nwindConn, true);
            }

            //nwindConn.Close();

            //return new object[] { 0, lst, blob};
        }

        public object AutoSeqMenuID(object[] objParam)
        {
            ClientType ct = ClientType.ctMsSql;
            IDbConnection nwindConn = AllocateConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), ref ct, true);
            try
            {
                string strSql = "";

                //为了区分不同的数据库 by Rei
                InfoCommand cmd = new InfoCommand(ClientInfo);
                if (nwindConn is SqlConnection)
                {
                    strSql = "select max(convert(int,MENUID)) from MENUTABLE where isnumeric(MENUID)=1";
                }
                else if (nwindConn is OracleConnection)
                {
                    strSql = "select max(to_number(MENUID)) from MENUTABLE";
                }
                else if (nwindConn is OdbcConnection)
                {
                    strSql = "select max(MENUID) from MENUTABLE";
                }
                else if (nwindConn is OleDbConnection)
                {
                    strSql = "select max(convert(int,MENUID)) from MENUTABLE";
                }
                else if (nwindConn.GetType().Name == "MySqlConnection")
                {
                    strSql = "select max(cast(MENUID as signed)) from MENUTABLE";
                }
                else if (nwindConn.GetType().Name == "IfxConnection")
                {
                    strSql = "select max(MENUID) from MENUTABLE";
                }

                cmd.Connection = nwindConn;
                cmd.CommandText = strSql;
                IDataReader dr = cmd.ExecuteReader();
                dr.Read();
                string count = dr[0].ToString();
                cmd.Cancel();
                dr.Close();
                int i = Convert.ToInt32(count) + 1;

                return new object[] { 0, i };
            }
            finally
            {
                ReleaseConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), nwindConn, true);
            }
        }

        private String GetSplitSysDBSD(String DBName)
        {
            string dbname = "";
            if (DbConnectionSet.GetDbConn(DBName, DeveloperID) != null)
                if (DbConnectionSet.GetDbConn(DBName, DeveloperID).SplitSystemTable)
                    dbname = DbConnectionSet.GetSystemDatabase(DeveloperID);// GetSplitSysDB(DBName);
                else
                    dbname = DBName;
            else
                dbname = GetSplitSysDB(DBName);

            return dbname;
        }

        public object UpdateWorkFlow(object[] objParam)
        {
            if (IsWorkFlowTransactionEnabled && GetWorkFlowTransaction(GetClientInfo(ClientInfoType.LoginDB).ToString()) != null)
            {
                IDbConnection nwindConn = AllocateWorkFlowConnection(GetClientInfo(ClientInfoType.LoginDB).ToString());
                try
                {
                    string sql = (string)objParam[0];

                    InfoCommand cmd = new InfoCommand(ClientInfo);
                    cmd.Connection = nwindConn;
                    cmd.CommandText = sql;
                    //set work flow transaction
                    cmd.Transaction = GetWorkFlowTransaction(GetClientInfo(ClientInfoType.LoginDB).ToString());
                    //
                    try
                    {
                        cmd.ExecuteNonQuery();
                        return new object[] { 0, null };
                    }
                    catch (Exception e)
                    {
                        return new object[] { 1, e.Message };
                    }
                }
                finally
                {
                    ReleaseConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), nwindConn, true);
                }
            }
            else
            {
                string dbname = GetSplitSysDBSD(GetClientInfo(ClientInfoType.LoginDB).ToString());
                if (WorkFlowTransmitterPropagationToken != null)
                {
                    using (System.Transactions.TransactionScope ts = new System.Transactions.TransactionScope(System.Transactions.TransactionInterop.GetTransactionFromTransmitterPropagationToken(WorkFlowTransmitterPropagationToken)))
                    {
                        using (IDbConnection nwindConn = DbConnectionSet.GetDbConn(dbname, DeveloperID).CreateConnection())
                        {
                            if (nwindConn.State != ConnectionState.Open)
                            {
                                nwindConn.Open();
                            }
                            string sql = (string)objParam[0];
                            InfoCommand cmd = new InfoCommand(ClientInfo);
                            cmd.Connection = nwindConn;
                            cmd.CommandText = sql;

                            try
                            {
                                var dbHelper = DbHelperFactory.CreateDbHelper();
                                if (dbHelper != null)
                                {
                                    dbHelper.ExecuteNonQuery(cmd);
                                }
                                else
                                {
                                    cmd.ExecuteNonQuery();
                                }
                                ts.Complete();
                                return new object[] { 0, null };
                            }
                            catch (Exception e)
                            {
                                return new object[] { 1, e.Message };
                            }
                        }
                    }
                }
                else
                {
                    using (IDbConnection nwindConn = DbConnectionSet.GetDbConn(dbname, DeveloperID).CreateConnection())
                    {
                        if (nwindConn.State != ConnectionState.Open)
                        {
                            nwindConn.Open();
                        }

                        string sql = (string)objParam[0];
                        InfoCommand cmd = new InfoCommand(ClientInfo);
                        cmd.Connection = nwindConn;
                        cmd.CommandText = sql;

                        try
                        {
                            var dbHelper = DbHelperFactory.CreateDbHelper();
                            if (dbHelper != null)
                            {
                                dbHelper.ExecuteNonQuery(cmd);
                            }
                            else
                            {
                                cmd.ExecuteNonQuery();
                            }
                            return new object[] { 0, null };
                        }
                        catch (Exception e)
                        {
                            return new object[] { 1, e.Message };
                        }
                    }
                }
            }
        }

        public object ExcuteWorkFlow(object[] objParam)
        {
            try
            {
                object transactionFlag = GetClientInfo(ClientInfoType.ClientSystem);
                bool useTransaction = transactionFlag != null && transactionFlag.ToString() == "EEP_WorkFlow";
                if (useTransaction && IsWorkFlowTransactionEnabled && GetWorkFlowTransaction(GetClientInfo(ClientInfoType.LoginDB).ToString()) != null)
                {
                    IDbConnection nwindConn = AllocateWorkFlowConnection(GetClientInfo(ClientInfoType.LoginDB).ToString());

                    string sql = (string)objParam[0];

                    InfoCommand cmd = new InfoCommand(ClientInfo);
                    cmd.Connection = nwindConn;
                    cmd.CommandText = sql;
                    //set work flow transaction
                    cmd.Transaction = GetWorkFlowTransaction(GetClientInfo(ClientInfoType.LoginDB).ToString());
                    //
                    IDataAdapter adpater = DBUtils.CreateDbDataAdapter(cmd);

                    DataSet ds = new DataSet();
                    DataTable dt = new DataTable();
                    (adpater as DbDataAdapter).Fill(dt);
                    ds.Tables.Add(dt);
                    return new object[] { 0, ds };

                }
                else
                {
                    string dbname = GetSplitSysDBSD(GetClientInfo(ClientInfoType.LoginDB).ToString());
                    if (WorkFlowTransmitterPropagationToken != null)
                    {
                        using (System.Transactions.TransactionScope ts = new System.Transactions.TransactionScope(System.Transactions.TransactionInterop.GetTransactionFromTransmitterPropagationToken(WorkFlowTransmitterPropagationToken)))
                        {
                            using (IDbConnection nwindConn = DbConnectionSet.GetDbConn(dbname, DeveloperID).CreateConnection())
                            {
                                if (nwindConn.State != ConnectionState.Open)
                                {
                                    nwindConn.Open();
                                }
                                string sql = (string)objParam[0];

                                InfoCommand cmd = new InfoCommand(ClientInfo);
                                cmd.Connection = nwindConn;
                                cmd.CommandText = sql;
                                IDataAdapter adpater = DBUtils.CreateDbDataAdapter(cmd);

                                DataSet ds = new DataSet();
                                DataTable dt = new DataTable();
                                (adpater as DbDataAdapter).Fill(dt);
                                ds.Tables.Add(dt);
                                ts.Complete();
                                return new object[] { 0, ds };
                            }
                        }
                    }
                    else
                    {
                        using (IDbConnection nwindConn = DbConnectionSet.GetDbConn(dbname, DeveloperID).CreateConnection())
                        {
                            if (nwindConn.State != ConnectionState.Open)
                            {
                                nwindConn.Open();
                            }
                            string sql = (string)objParam[0];

                            InfoCommand cmd = new InfoCommand(ClientInfo);
                            cmd.Connection = nwindConn;
                            cmd.CommandText = sql;
                            IDataAdapter adpater = DBUtils.CreateDbDataAdapter(cmd);

                            DataSet ds = new DataSet();
                            DataTable dt = new DataTable();
                            (adpater as DbDataAdapter).Fill(dt);
                            ds.Tables.Add(dt);
                            return new object[] { 0, ds };
                        }
                    }
                }
            }
            catch (Exception e)
            {
                using (StreamWriter writer = new StreamWriter(Path.Combine(EEPRegistry.Server, "executeWorkFlow.log"), true))
                {
                    writer.WriteLine(string.Format("DateTime:{0:yyyy/MM/dd HH:mm:ss}", DateTime.Now));
                    writer.WriteLine(string.Format("SQL:{0}", (string)objParam[0]));
                    writer.WriteLine(string.Format("Message:{0}", e.Message));
                    writer.WriteLine(string.Format("StackTrace:{0}", e.StackTrace));
                    writer.WriteLine();
                }
                throw e;
            }
        }


        const bool WORKFLOW_TRANSACTION_ENABLE = true;
        const string WORKFLOW_COMMAND_NAME = "EEP_WorkFlow";

        private bool IsWorkFlowTransactionEnabled
        {
            get
            {
                return WorkFlowTransmitterPropagationToken == null && WORKFLOW_TRANSACTION_ENABLE;
            }
        }

        private byte[] WorkFlowTransmitterPropagationToken
        {
            get
            {
                if (GetClientInfo(ClientInfoType.UserParam2) != null && GetClientInfo(ClientInfoType.UserParam2) is byte[])
                {
                    return (byte[])GetClientInfo(ClientInfoType.UserParam2);
                }
                return null;
            }
        }

        public object[] BeginWorkFlowTransaction(object[] objParam)
        {
            if (IsWorkFlowTransactionEnabled)
            {
                if (GetWorkFlowTransaction(GetClientInfo(ClientInfoType.LoginDB).ToString()) == null) //防止重复开始transaction
                {
                    IDbConnection conn = AllocateWorkFlowConnection(GetClientInfo(ClientInfoType.LoginDB).ToString());
                    string dbname = GetSplitSysDBSD(GetClientInfo(ClientInfoType.LoginDB).ToString());
                    //string dbname = GetSplitSysDB(GetClientInfo(ClientInfoType.LoginDB).ToString());
                    string developerID = DeveloperID;
                    IDbTransaction transaction = conn.BeginTransaction();
                    DbConnectionSet.SetTransaction(dbname, developerID, GetClientInfo(ClientInfoType.LoginUser).ToString(), this.GetType().FullName, WORKFLOW_COMMAND_NAME, transaction);
                }
            }

            return new object[] { 0 };
        }

        private IDbTransaction GetWorkFlowTransaction(string DBName)
        {
            string dbname = GetSplitSysDBSD(DBName);
            //string dbname = GetSplitSysDB(DBName);
            string developerID = DeveloperID;
            IDbTransaction transaction = DbConnectionSet.GetTransaction(dbname, developerID, GetClientInfo(ClientInfoType.LoginUser).ToString(), this.GetType().FullName, WORKFLOW_COMMAND_NAME);
            return transaction;
        }

        public object[] ComitWorkFlowTransaction(object[] objParam)
        {
            if (IsWorkFlowTransactionEnabled)
            {
                IDbTransaction transaction = GetWorkFlowTransaction(GetClientInfo(ClientInfoType.LoginDB).ToString());
                transaction.Commit();
                ReleaseWorkFlowConnection(GetClientInfo(ClientInfoType.LoginDB).ToString());
            }
            return new object[] { 0 };
        }

        public object[] RollBackWorkFlowTransaction(object[] objParam)
        {
            if (IsWorkFlowTransactionEnabled)
            {
                IDbTransaction transaction = GetWorkFlowTransaction(GetClientInfo(ClientInfoType.LoginDB).ToString());
                transaction.Rollback();
                ReleaseWorkFlowConnection(GetClientInfo(ClientInfoType.LoginDB).ToString());
            }
            return new object[] { 0 };
        }

        private IDbConnection AllocateWorkFlowConnection(string DBName)
        {
            string dbname = GetSplitSysDBSD(DBName); //GetSplitSysDB(DBName);
            IDbConnection conn = AllocateCacheConnection(dbname, WORKFLOW_COMMAND_NAME);
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            return conn;
        }

        private void ReleaseWorkFlowConnection(string DBName)
        {
            string dbname = GetSplitSysDBSD(DBName); //GetSplitSysDB(DBName);
            ReleaseCacheConnection(dbname, WORKFLOW_COMMAND_NAME);
        }

        public object OPMenu(object[] objParam)
        {
            string strMenuID = (String)objParam[0];
            string strCaption = (String)objParam[1];
            string strParent = (String)objParam[2];
            string strModuleType = (String)objParam[3];
            string strPackage = (String)objParam[4];
            string strItemParam = (String)objParam[5];
            string strForm = (String)objParam[6];
            string strItemType = (String)objParam[7];
            string strSEQ_NO = (String)objParam[8];
            string strImageUrl = (String)objParam[11];
            string captionlanguage = (String)objParam[12];
            Srvtools.MGControl.OpType optype = (Srvtools.MGControl.OpType)objParam[9];

            ClientType ct = ClientType.ctMsSql;
            IDbConnection nwindConn = AllocateConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), ref ct, true);
            try
            {
                string strSql = "";

                string strBlob = "";
                if (nwindConn is SqlConnection)
                    strBlob = "update MENUTABLE set [IMAGE] = @icon where MENUID = '" + strMenuID + "'";
                else if (nwindConn is OracleConnection)
                    strBlob = "update MENUTABLE set IMAGE = :icon where MENUID = '" + strMenuID + "'";
                else if (nwindConn is OdbcConnection)
                    strBlob = "update MENUTABLE set IMAGE = ? where MENUID = '" + strMenuID + "'";
                else if (nwindConn is OleDbConnection)
                    strBlob = "update MENUTABLE set IMAGE = ? where MENUID = '" + strMenuID + "'";
                else if (nwindConn.GetType().Name == "MySqlConnection")
                    strBlob = "update MENUTABLE set IMAGE = @icon where MENUID = '" + strMenuID + "'";
                else if (nwindConn.GetType().Name == "IfxConnection")
                    strBlob = "update MENUTABLE set IMAGE = ? where MENUID = '" + strMenuID + "'";

                InfoCommand cmd = new InfoCommand(ClientInfo);
                cmd.Connection = nwindConn;
                if (optype == Srvtools.MGControl.OpType.add)
                {
                    if (nwindConn is SqlConnection)
                        strSql = "insert into MENUTABLE (MENUID, CAPTION" + captionlanguage + ", PARENT, MODULETYPE, PACKAGE, ITEMPARAM, FORM, ITEMTYPE, SEQ_NO, IMAGEURL) " +
                            "values (@MENUID, @CAPTION" + captionlanguage + ", @PARENT, @MODULETYPE, @PACKAGE, @ITEMPARAM, @FORM, @ITEMTYPE, @SEQ_NO, @IMAGEURL)";
                    else if (nwindConn is OracleConnection)
                        strSql = "insert into MENUTABLE (MENUID, CAPTION" + captionlanguage + ", PARENT, MODULETYPE, PACKAGE, ITEMPARAM, FORM, ITEMTYPE, SEQ_NO, IMAGEURL) " +
                            "values (:MENUID, :CAPTION" + captionlanguage + ", :PARENT, :MODULETYPE, :PACKAGE, :ITEMPARAM, :FORM, :ITEMTYPE, :SEQ_NO, :IMAGEURL)";
                    else if (nwindConn is OdbcConnection)
                        strSql = "insert into MENUTABLE (MENUID, CAPTION" + captionlanguage + ", PARENT, MODULETYPE, PACKAGE, ITEMPARAM, FORM, ITEMTYPE, SEQ_NO, IMAGEURL) " +
                            "values ('" + strMenuID + "', '" + strCaption + "', '" + strParent + "', '" + strModuleType + "', '" + strPackage +
                            "', '" + strItemParam + "', '" + strForm + "', '" + strItemType + "', '" + strSEQ_NO + "', '" + strImageUrl + "')";
                    else if (nwindConn is OleDbConnection)
                        //strSql = "insert into MENUTABLE (MENUID, CAPTION" + captionlanguage + ", PARENT, MODULETYPE, PACKAGE, ITEMPARAM, FORM, ITEMTYPE, SEQ_NO, IMAGEURL) " +
                        //    "values ('" + strMenuID + "', '" + strCaption + "', '" + strParent + "', '" + strModuleType + "', '" + strPackage +
                        //    "', '" + strItemParam + "', '" + strForm + "', '" + strItemType + "', '" + strSEQ_NO + "', '" + strImageUrl + "')";
                        strSql = "insert into MENUTABLE (MENUID, CAPTION" + captionlanguage + ", PARENT, MODULETYPE, PACKAGE, ITEMPARAM, FORM,IMAGE, ITEMTYPE, SEQ_NO, IMAGEURL) " +
                            "values ('" + strMenuID + "', '" + strCaption + "', '" + strParent + "', '" + strModuleType + "', '" + strPackage +
                            "', '" + strItemParam + "', '" + strForm + "', " + "0x0" + ", '" + strItemType + "', '" + strSEQ_NO + "', '" + strImageUrl + "')";
                    else if (nwindConn.GetType().Name == "MySqlConnection")
                        strSql = "insert into MENUTABLE (MENUID, CAPTION" + captionlanguage + ", PARENT, MODULETYPE, PACKAGE, ITEMPARAM, FORM,IMAGE, ITEMTYPE, SEQ_NO, IMAGEURL) " +
                            "values ('" + strMenuID + "', '" + strCaption + "', '" + strParent + "', '" + strModuleType + "', '" + strPackage +
                            "', '" + strItemParam + "', '" + strForm + "', " + "0x0" + ", '" + strItemType + "', '" + strSEQ_NO + "', '" + strImageUrl + "')";
                    else if (nwindConn.GetType().Name == "IfxConnection")
                        strSql = "insert into MENUTABLE (MENUID, CAPTION" + captionlanguage + ", PARENT, MODULETYPE, PACKAGE, ITEMPARAM, FORM, ITEMTYPE, SEQ_NO, IMAGEURL) " +
                            "values ('" + strMenuID + "', '" + strCaption + "', '" + strParent + "', '" + strModuleType + "', '" + strPackage +
                            "', '" + strItemParam + "', '" + strForm + "', '" + strItemType + "', '" + strSEQ_NO + "', '" + strImageUrl + "')";

                    if (nwindConn is SqlConnection)
                    {
                        cmd.CommandText = strSql;
                        cmd.Parameters.Add(new SqlParameter("@MENUID", strMenuID));
                        cmd.Parameters.Add(new SqlParameter("@CAPTION" + captionlanguage, strCaption == null ? String.Empty : strCaption));
                        cmd.Parameters.Add(new SqlParameter("@PARENT", strParent == null ? String.Empty : strParent));
                        cmd.Parameters.Add(new SqlParameter("@MODULETYPE", strModuleType == null ? String.Empty : strModuleType));
                        cmd.Parameters.Add(new SqlParameter("@PACKAGE", strPackage == null ? String.Empty : strPackage));
                        cmd.Parameters.Add(new SqlParameter("@ITEMPARAM", strItemParam == null ? String.Empty : strItemParam));
                        cmd.Parameters.Add(new SqlParameter("@FORM", strForm == null ? String.Empty : strForm));
                        //cmd.Parameters.Add(new SqlParameter("@IMAGE", "'0'"));
                        cmd.Parameters.Add(new SqlParameter("@ITEMTYPE", strItemType == null ? String.Empty : strItemType));
                        cmd.Parameters.Add(new SqlParameter("@SEQ_NO", strSEQ_NO == null ? String.Empty : strSEQ_NO));
                        cmd.Parameters.Add(new SqlParameter("@IMAGEURL", strImageUrl == null ? String.Empty : strImageUrl));
                        cmd.ExecuteNonQuery();
                    }
                    else if (nwindConn is OracleConnection)
                    {
                        cmd.CommandText = strSql;
                        cmd.Parameters.Add(new OracleParameter(":MENUID", strMenuID));
                        cmd.Parameters.Add(new OracleParameter(":CAPTION" + captionlanguage, strCaption == null ? String.Empty : strCaption));
                        cmd.Parameters.Add(new OracleParameter(":PARENT", strParent == null ? String.Empty : strParent));
                        cmd.Parameters.Add(new OracleParameter(":MODULETYPE", strModuleType == null ? String.Empty : strModuleType));
                        cmd.Parameters.Add(new OracleParameter(":PACKAGE", strPackage == null ? String.Empty : strPackage));
                        cmd.Parameters.Add(new OracleParameter(":ITEMPARAM", strItemParam == null ? String.Empty : strItemParam));
                        cmd.Parameters.Add(new OracleParameter(":FORM", strForm == null ? String.Empty : strForm));
                        //cmd.Parameters.Add(new OracleParameter(":IMAGE", "'0'"));
                        cmd.Parameters.Add(new OracleParameter(":ITEMTYPE", strItemType == null ? String.Empty : strItemType));
                        cmd.Parameters.Add(new OracleParameter(":SEQ_NO", strSEQ_NO == null ? String.Empty : strSEQ_NO));
                        cmd.Parameters.Add(new OracleParameter(":IMAGEURL", strImageUrl == null ? String.Empty : strImageUrl));
                        cmd.ExecuteNonQuery();
                    }
                    else if (nwindConn is OdbcConnection)
                    {
                        cmd.CommandText = strSql;
                        cmd.Parameters.Add(new OdbcParameter("?", strMenuID));
                        cmd.Parameters.Add(new OdbcParameter("?" + captionlanguage, strCaption));
                        cmd.Parameters.Add(new OdbcParameter("?", strParent));
                        cmd.Parameters.Add(new OdbcParameter("?", strModuleType));
                        cmd.Parameters.Add(new OdbcParameter("?", strPackage));
                        cmd.Parameters.Add(new OdbcParameter("?", strItemParam));
                        cmd.Parameters.Add(new OdbcParameter("?", strForm));
                        cmd.Parameters.Add(new OdbcParameter("?", "'0'"));
                        cmd.Parameters.Add(new OdbcParameter("?", strItemType));
                        cmd.Parameters.Add(new OdbcParameter("?", strSEQ_NO));
                        cmd.Parameters.Add(new OdbcParameter("?", strImageUrl));
                        cmd.ExecuteNonQuery();
                    }
                    else if (nwindConn is OleDbConnection)
                    {
                        cmd.CommandText = strSql;
                        //cmd.Parameters.Add(new OleDbParameter("@MENUID", strMenuID));
                        //cmd.Parameters.Add(new OleDbParameter("@CAPTION" + captionlanguage, strCaption));
                        //cmd.Parameters.Add(new OleDbParameter("@PARENT", strParent));
                        //cmd.Parameters.Add(new OleDbParameter("@MODULETYPE", strModuleType));
                        //cmd.Parameters.Add(new OleDbParameter("@PACKAGE", strPackage));
                        //cmd.Parameters.Add(new OleDbParameter("@ITEMPARAM", strItemParam));
                        //cmd.Parameters.Add(new OleDbParameter("@FORM", strForm));
                        //cmd.Parameters.Add(new OleDbParameter("@IMAGE", "'0'"));
                        //cmd.Parameters.Add(new OleDbParameter("@ITEMTYPE", strItemType));
                        //cmd.Parameters.Add(new OleDbParameter("@SEQ_NO", strSEQ_NO));
                        //cmd.Parameters.Add(new OleDbParameter("@IMAGEURL", strImageUrl));
                        cmd.ExecuteNonQuery();
                    }
#if MySql
                    else if (nwindConn is MySqlConnection)
                    {
                        cmd.CommandText = strSql;
                        cmd.Parameters.Add(new MySqlParameter("@MENUID", strMenuID));
                        cmd.Parameters.Add(new MySqlParameter("@CAPTION" + captionlanguage, strCaption));
                        cmd.Parameters.Add(new MySqlParameter("@PARENT", strParent));
                        cmd.Parameters.Add(new MySqlParameter("@MODULETYPE", strModuleType));
                        cmd.Parameters.Add(new MySqlParameter("@PACKAGE", strPackage));
                        cmd.Parameters.Add(new MySqlParameter("@ITEMPARAM", strItemParam));
                        cmd.Parameters.Add(new MySqlParameter("@FORM", strForm));
                        cmd.Parameters.Add(new MySqlParameter("@IMAGE", "'0'"));
                        cmd.Parameters.Add(new MySqlParameter("@ITEMTYPE", strItemType));
                        cmd.Parameters.Add(new MySqlParameter("@SEQ_NO", strSEQ_NO));
                        cmd.Parameters.Add(new MySqlParameter("@IMAGEURL", strImageUrl));
                        cmd.ExecuteNonQuery();
                    }
#endif
#if Informix
                    else if (nwindConn is IBM.Data.Informix.IfxConnection)
                    {
                        cmd.CommandText = strSql;
                        cmd.Parameters.Add(new IBM.Data.Informix.IfxParameter("?", strMenuID));
                        cmd.Parameters.Add(new IBM.Data.Informix.IfxParameter("?" + captionlanguage, strCaption));
                        cmd.Parameters.Add(new IBM.Data.Informix.IfxParameter("?", strParent));
                        cmd.Parameters.Add(new IBM.Data.Informix.IfxParameter("?", strModuleType));
                        cmd.Parameters.Add(new IBM.Data.Informix.IfxParameter("?", strPackage));
                        cmd.Parameters.Add(new IBM.Data.Informix.IfxParameter("?", strItemParam));
                        cmd.Parameters.Add(new IBM.Data.Informix.IfxParameter("?", strForm));
                        cmd.Parameters.Add(new IBM.Data.Informix.IfxParameter("?", "'0'"));
                        cmd.Parameters.Add(new IBM.Data.Informix.IfxParameter("?", strItemType));
                        cmd.Parameters.Add(new IBM.Data.Informix.IfxParameter("?", strSEQ_NO));
                        cmd.Parameters.Add(new IBM.Data.Informix.IfxParameter("?", strImageUrl));
                        cmd.ExecuteNonQuery();
                    }
#endif
                }
                else if (optype == Srvtools.MGControl.OpType.modify)
                {
                    if (nwindConn is SqlConnection)
                        strSql = "update MENUTABLE set CAPTION" + captionlanguage + " = @CAPTION" + captionlanguage + ", PARENT = @PARENT, MODULETYPE = @MODULETYPE"
                                    + ", PACKAGE = @PACKAGE, ITEMPARAM = @ITEMPARAM, FORM = @FORM, ITEMTYPE = @ITEMTYPE, SEQ_NO = @SEQ_NO, IMAGEURL = @IMAGEURL where MENUID = '" + strMenuID + "'";
                    else if (nwindConn is OracleConnection)
                        strSql = "update MENUTABLE set CAPTION" + captionlanguage + " = :CAPTION" + captionlanguage + ", PARENT = :PARENT, MODULETYPE = :MODULETYPE"
                                    + ", PACKAGE = :PACKAGE, ITEMPARAM = :ITEMPARAM, FORM = :FORM, ITEMTYPE = :ITEMTYPE, SEQ_NO = :SEQ_NO, IMAGEURL = :IMAGEURL where MENUID = '" + strMenuID + "'";
                    else if (nwindConn is OdbcConnection)
                        strSql = "update MENUTABLE set CAPTION" + captionlanguage + " = '" + strCaption + "', PARENT = '" + strParent + "', MODULETYPE = '" +
                            strModuleType + "', PACKAGE = '" + strPackage + "', ITEMPARAM = '" + strItemParam + "', FORM = '" +
                            strForm + "', ITEMTYPE = '" + strItemType + "', SEQ_NO = '" + strSEQ_NO + "',IMAGEURL = '" + strImageUrl + "' where MENUID = '" + strMenuID + "'";
                    else if (nwindConn is OleDbConnection)
                        strSql = "update MENUTABLE set CAPTION" + captionlanguage + " = N'" + strCaption + "', PARENT = '" + strParent + "', MODULETYPE = '" +
                            strModuleType + "', PACKAGE = '" + strPackage + "', ITEMPARAM = '" + strItemParam + "', FORM = '" +
                            strForm + "', ITEMTYPE = '" + strItemType + "', SEQ_NO = '" + strSEQ_NO + "',IMAGEURL = '" + strImageUrl + "' where MENUID = '" + strMenuID + "'";
                    else if (nwindConn.GetType().Name == "MySqlConnection")
                        strSql = "update MENUTABLE set CAPTION" + captionlanguage + " = N'" + strCaption + "', PARENT = '" + strParent + "', MODULETYPE = '" +
                            strModuleType + "', PACKAGE = '" + strPackage + "', ITEMPARAM = '" + strItemParam + "', FORM = '" +
                            strForm + "', ITEMTYPE = '" + strItemType + "', SEQ_NO = '" + strSEQ_NO + "',IMAGEURL = '" + strImageUrl + "' where MENUID = '" + strMenuID + "'";
                    else if (nwindConn.GetType().Name == "IfxConnection")
                        strSql = "update MENUTABLE set CAPTION" + captionlanguage + " = '" + strCaption + "', PARENT = '" + strParent + "', MODULETYPE = '" +
                            strModuleType + "', PACKAGE = '" + strPackage + "', ITEMPARAM = '" + strItemParam + "', FORM = '" +
                            strForm + "', ITEMTYPE = '" + strItemType + "', SEQ_NO = '" + strSEQ_NO + "',IMAGEURL = '" + strImageUrl + "' where MENUID = '" + strMenuID + "'";

                    if (nwindConn is SqlConnection)
                    {
                        cmd.CommandText = strSql;
                        //cmd.Parameters.Add(new SqlParameter("@MENUID", strMenuID));
                        cmd.Parameters.Add(new SqlParameter("@CAPTION" + captionlanguage, strCaption == null ? String.Empty : strCaption));
                        cmd.Parameters.Add(new SqlParameter("@PARENT", strParent == null ? String.Empty : strParent));
                        cmd.Parameters.Add(new SqlParameter("@MODULETYPE", strModuleType == null ? String.Empty : strModuleType));
                        cmd.Parameters.Add(new SqlParameter("@PACKAGE", strPackage == null ? String.Empty : strPackage));
                        cmd.Parameters.Add(new SqlParameter("@ITEMPARAM", strItemParam == null ? String.Empty : strItemParam));
                        cmd.Parameters.Add(new SqlParameter("@FORM", strForm == null ? String.Empty : strForm));
                        //cmd.Parameters.Add(new SqlParameter("@IMAGE", "'0'"));
                        cmd.Parameters.Add(new SqlParameter("@ITEMTYPE", strItemType == null ? String.Empty : strItemType));
                        cmd.Parameters.Add(new SqlParameter("@SEQ_NO", strSEQ_NO == null ? String.Empty : strSEQ_NO));
                        cmd.Parameters.Add(new SqlParameter("@IMAGEURL", strImageUrl == null ? String.Empty : strImageUrl));
                        cmd.ExecuteNonQuery();
                    }
                    else if (nwindConn is OracleConnection)
                    {
                        cmd.CommandText = strSql;
                        //cmd.Parameters.Add(new OracleParameter(":MENUID", strMenuID));
                        cmd.Parameters.Add(new OracleParameter(":CAPTION" + captionlanguage, strCaption == null ? String.Empty : strCaption));
                        cmd.Parameters.Add(new OracleParameter(":PARENT", strParent == null ? String.Empty : strParent));
                        cmd.Parameters.Add(new OracleParameter(":MODULETYPE", strModuleType == null ? String.Empty : strModuleType));
                        cmd.Parameters.Add(new OracleParameter(":PACKAGE", strPackage == null ? String.Empty : strPackage));
                        cmd.Parameters.Add(new OracleParameter(":ITEMPARAM", strItemParam == null ? String.Empty : strItemParam));
                        cmd.Parameters.Add(new OracleParameter(":FORM", strForm == null ? String.Empty : strForm));
                        //cmd.Parameters.Add(new OracleParameter(":IMAGE", "'0'"));
                        cmd.Parameters.Add(new OracleParameter(":ITEMTYPE", strItemType == null ? String.Empty : strItemType));
                        cmd.Parameters.Add(new OracleParameter(":SEQ_NO", strSEQ_NO == null ? String.Empty : strSEQ_NO));
                        cmd.Parameters.Add(new OracleParameter(":IMAGEURL", strImageUrl == null ? String.Empty : strImageUrl));
                        cmd.ExecuteNonQuery();
                    }
                    else if (nwindConn is OdbcConnection)
                    {
                        cmd.CommandText = strSql;
                        cmd.Parameters.Add(new OdbcParameter("?", strMenuID));
                        cmd.Parameters.Add(new OdbcParameter("?" + captionlanguage, strCaption));
                        cmd.Parameters.Add(new OdbcParameter("?", strParent));
                        cmd.Parameters.Add(new OdbcParameter("?", strModuleType));
                        cmd.Parameters.Add(new OdbcParameter("?", strPackage));
                        cmd.Parameters.Add(new OdbcParameter("?", strItemParam));
                        cmd.Parameters.Add(new OdbcParameter("?", strForm));
                        cmd.Parameters.Add(new OdbcParameter("?", "'0'"));
                        cmd.Parameters.Add(new OdbcParameter("?", strItemType));
                        cmd.Parameters.Add(new OdbcParameter("?", strSEQ_NO));
                        cmd.Parameters.Add(new OdbcParameter("?", strImageUrl));
                        cmd.ExecuteNonQuery();
                    }
                    else if (nwindConn is OleDbConnection)
                    {
                        cmd.CommandText = strSql;
                        //cmd.Parameters.Add(new OleDbParameter("@MENUID", strMenuID));
                        //cmd.Parameters.Add(new OleDbParameter("@CAPTION" + captionlanguage, strCaption));
                        //cmd.Parameters.Add(new OleDbParameter("@PARENT", strParent));
                        //cmd.Parameters.Add(new OleDbParameter("@MODULETYPE", strModuleType));
                        //cmd.Parameters.Add(new OleDbParameter("@PACKAGE", strPackage));
                        //cmd.Parameters.Add(new OleDbParameter("@ITEMPARAM", strItemParam));
                        //cmd.Parameters.Add(new OleDbParameter("@FORM", strForm));
                        //cmd.Parameters.Add(new OleDbParameter("@IMAGE", "'0'"));
                        //cmd.Parameters.Add(new OleDbParameter("@ITEMTYPE", strItemType));
                        //cmd.Parameters.Add(new OleDbParameter("@SEQ_NO", strSEQ_NO));
                        //cmd.Parameters.Add(new OleDbParameter("@IMAGEURL", strImageUrl));
                        cmd.ExecuteNonQuery();
                    }
#if MySql
                    else if (nwindConn is MySqlConnection)
                    {
                        cmd.CommandText = strSql;
                        cmd.Parameters.Add(new MySqlParameter("@MENUID", strMenuID));
                        cmd.Parameters.Add(new MySqlParameter("@CAPTION" + captionlanguage, strCaption));
                        cmd.Parameters.Add(new MySqlParameter("@PARENT", strParent));
                        cmd.Parameters.Add(new MySqlParameter("@MODULETYPE", strModuleType));
                        cmd.Parameters.Add(new MySqlParameter("@PACKAGE", strPackage));
                        cmd.Parameters.Add(new MySqlParameter("@ITEMPARAM", strItemParam));
                        cmd.Parameters.Add(new MySqlParameter("@FORM", strForm));
                        cmd.Parameters.Add(new MySqlParameter("@IMAGE", "'0'"));
                        cmd.Parameters.Add(new MySqlParameter("@ITEMTYPE", strItemType));
                        cmd.Parameters.Add(new MySqlParameter("@SEQ_NO", strSEQ_NO));
                        cmd.Parameters.Add(new MySqlParameter("@IMAGEURL", strImageUrl));
                        cmd.ExecuteNonQuery();
                    }
#endif
#if Informix
                    else if (nwindConn is IBM.Data.Informix.IfxConnection)
                    {
                        cmd.CommandText = strSql;
                        cmd.Parameters.Add(new IBM.Data.Informix.IfxParameter("?", strMenuID));
                        cmd.Parameters.Add(new IBM.Data.Informix.IfxParameter("?" + captionlanguage, strCaption));
                        cmd.Parameters.Add(new IBM.Data.Informix.IfxParameter("?", strParent));
                        cmd.Parameters.Add(new IBM.Data.Informix.IfxParameter("?", strModuleType));
                        cmd.Parameters.Add(new IBM.Data.Informix.IfxParameter("?", strPackage));
                        cmd.Parameters.Add(new IBM.Data.Informix.IfxParameter("?", strItemParam));
                        cmd.Parameters.Add(new IBM.Data.Informix.IfxParameter("?", strForm));
                        cmd.Parameters.Add(new IBM.Data.Informix.IfxParameter("?", "'0'"));
                        cmd.Parameters.Add(new IBM.Data.Informix.IfxParameter("?", strItemType));
                        cmd.Parameters.Add(new IBM.Data.Informix.IfxParameter("?", strSEQ_NO));
                        cmd.Parameters.Add(new IBM.Data.Informix.IfxParameter("?", strImageUrl));
                        cmd.ExecuteNonQuery();
                    }
#endif
                }
                else
                {
                    strSql = "delete from MENUTABLE where MENUID = '" + strMenuID + "'";
                    cmd.CommandText = strSql;
                    cmd.ExecuteNonQuery();

                    strSql = "delete from USERMENUS where MENUID = '" + strMenuID + "'";
                    cmd.CommandText = strSql;
                    cmd.ExecuteNonQuery();

                    strSql = "delete from GROUPMENUS where MENUID = '" + strMenuID + "'";
                    cmd.CommandText = strSql;
                    cmd.ExecuteNonQuery();

                    strSql = "delete from MENUTABLECONTROL where MENUID = '" + strMenuID + "'";
                    cmd.CommandText = strSql;
                    cmd.ExecuteNonQuery();

                    strSql = "delete from USERMENUCONTROL where MENUID = '" + strMenuID + "'";
                    cmd.CommandText = strSql;
                    cmd.ExecuteNonQuery();

                    strSql = "delete from GROUPMENUCONTROL where MENUID = '" + strMenuID + "'";
                    cmd.CommandText = strSql;
                    cmd.ExecuteNonQuery();

                }

                if (optype != Srvtools.MGControl.OpType.delete && objParam[10] != null)
                {
                    byte[] blob = (byte[])objParam[10];

                    if (nwindConn is SqlConnection)
                    {
                        SqlParameter param = new SqlParameter("@icon", SqlDbType.VarBinary, blob.Length, ParameterDirection.Input, false,
                           0, 0, null, DataRowVersion.Current, blob);
                        cmd.CommandText = strBlob;
                        cmd.Parameters.Add(param);
                        cmd.ExecuteNonQuery();
                    }
                    else if (nwindConn is OracleConnection)
                    {
                        OracleParameter param = new OracleParameter(":icon", OracleType.Blob, blob.Length, ParameterDirection.Input, false,
                           0, 0, null, DataRowVersion.Current, blob);
                        cmd.CommandText = strBlob;
                        cmd.Parameters.Add(param);
                        cmd.ExecuteNonQuery();
                    }
                    else if (nwindConn is OdbcConnection)
                    {
                        OdbcParameter param = new OdbcParameter("?", OdbcType.VarBinary, blob.Length, ParameterDirection.Input, false,
                           0, 0, null, DataRowVersion.Current, blob);
                        cmd.CommandText = strBlob;
                        cmd.Parameters.Add(param);
                        try
                        {
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception e)
                        {
                            string str = e.Message;
                        }
                    }
                    else if (nwindConn is OleDbConnection)
                    {
                        OleDbParameter param = new OleDbParameter("@icon", OleDbType.LongVarBinary, blob.Length, ParameterDirection.Input, false,
                           0, 0, null, DataRowVersion.Current, blob);
                        cmd.CommandText = strBlob;
                        cmd.Parameters.Add(param);
                        cmd.ExecuteNonQuery();
                    }
#if MySql
                    else if (nwindConn is MySqlConnection)
                    {
                        MySqlParameter param = new MySqlParameter("@icon", MySqlDbType.Blob, blob.Length, ParameterDirection.Input, false,
                           0, 0, null, DataRowVersion.Current, blob);
                        cmd.CommandText = strBlob;
                        cmd.Parameters.Add(param);
                        cmd.ExecuteNonQuery();
                    }
#endif
#if Informix
                    else if (nwindConn is IBM.Data.Informix.IfxConnection)
                    {
                        IBM.Data.Informix.IfxParameter param = new IBM.Data.Informix.IfxParameter("?", IBM.Data.Informix.IfxType.Blob, blob.Length,
                                                                                    ParameterDirection.Input, false, 0, 0, null, DataRowVersion.Current, blob);
                        cmd.CommandText = strBlob;
                        cmd.Parameters.Add(param);
                        try
                        {
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception e)
                        {
                            string str = e.Message;
                        }
                    }
#endif

                }

                return new object[] { 0, null };
            }
            finally
            {
                ReleaseConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), nwindConn, true);
            }
        }

        public object LoadGroups(object[] objParam)
        {
            string strMenuID = (String)objParam[0];
            ClientType ct = ClientType.ctMsSql;
            IDbConnection nwindConn = AllocateConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), ref ct, true);

            try
            {
                string strSql = "";
                if (ServerConfig.LoginObjectEnabled)
                {
                    strSql = "select GROUPMENUS.*, GROUPMENUS.GROUPID AS GROUPNAME from GROUPMENUS where GROUPMENUS.MENUID = '" + strMenuID + "'";
                }
                else
                {
                    if (nwindConn is SqlConnection)
                        strSql = "select GROUPMENUS.*, GROUPS.GROUPNAME from GROUPMENUS left join GROUPS on GROUPS.GROUPID = GROUPMENUS.GROUPID where GROUPMENUS.MENUID = '" + strMenuID + "'";
                    else if (nwindConn is OdbcConnection)
                        strSql = "select GROUPMENUS.*, GROUPS.GROUPNAME from GROUPMENUS, GROUPS where GROUPS.GROUPID = GROUPMENUS.GROUPID and GROUPMENUS.MENUID = '" + strMenuID + "'";
                    else if (nwindConn is OracleConnection)
                        strSql = "select GROUPMENUS.*, GROUPS.GROUPNAME from GROUPMENUS left join GROUPS on GROUPS.GROUPID = GROUPMENUS.GROUPID where GROUPMENUS.MENUID = '" + strMenuID + "'";
                    else if (nwindConn is OleDbConnection)
                        strSql = "select GROUPMENUS.*, GROUPS.GROUPNAME from GROUPMENUS left join GROUPS on GROUPS.GROUPID = GROUPMENUS.GROUPID where GROUPMENUS.MENUID = '" + strMenuID + "'";
                    else if (nwindConn.GetType().Name == "MySqlConnection")
                        strSql = "select GROUPMENUS.*, GROUPS.GROUPNAME from GROUPMENUS left join GROUPS on GROUPS.GROUPID = GROUPMENUS.GROUPID where GROUPMENUS.MENUID = '" + strMenuID + "'";
                    else if (nwindConn.GetType().Name == "IfxConnection")
                        strSql = "select GROUPMENUS.*, GROUPS.GROUPNAME from GROUPMENUS, GROUPS where GROUPS.GROUPID = GROUPMENUS.GROUPID and GROUPMENUS.MENUID = '" + strMenuID + "'";
                }

                //为了区分不同的数据库 by Rei
                InfoCommand myCommand = new InfoCommand(ClientInfo);

                myCommand.Connection = nwindConn;
                myCommand.CommandText = strSql;
                IDataAdapter adpater = DBUtils.CreateDbDataAdapter(myCommand);

                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                (adpater as DbDataAdapter).Fill(dt);
                ds.Tables.Add(dt);

                return new object[] { 0, ds };
            }
            finally
            {
                ReleaseConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), nwindConn, true);
            }
        }

        public object setGroups(object[] objParam)
        {
            string strMenuID = (String)objParam[0];
            ClientType ct = ClientType.ctMsSql;
            IDbConnection nwindConn = AllocateConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), ref ct, true);
            try
            {
                ArrayList GroupID = (ArrayList)objParam[1];
                ArrayList MenuID = (ArrayList)objParam[2];

                //delete groups which will possibly be repeated later
                string strDel = "delete from GROUPMENUS where MENUID = '" + strMenuID + "'";

                //为了区分不同的数据库 by Rei
                InfoCommand cmd = new InfoCommand(ClientInfo);

                cmd.Connection = nwindConn;
                cmd.CommandText = strDel;
                cmd.ExecuteNonQuery();

                //modify by lily 2011/5/26 刪除聯動的MenuControl的table內容
                string lst = "";
                for (int i = 0; i < GroupID.Count; i++)
                {
                    lst = lst + "','" + GroupID[i].ToString();
                }
                if (lst.Length > 3)
                {
                    strDel = "delete from GROUPMENUCONTROL where MENUID='" + strMenuID + "' and GROUPID not in ('" + lst + "')";
                }
                cmd.CommandText = strDel;
                cmd.ExecuteNonQuery();

                //add new groups
                for (int i = 0; i < MenuID.Count; i++)
                {
                    string strInsert = "insert into GROUPMENUS (GROUPID, MENUID) values ('"
                        + GroupID[i].ToString() + "', '" + MenuID[i].ToString() + "')";

                    //为了区分不同的数据库 by Rei
                    InfoCommand InsertCmd = new InfoCommand(ClientInfo);

                    InsertCmd.Connection = nwindConn;
                    InsertCmd.CommandText = strInsert;
                    InsertCmd.ExecuteNonQuery();
                }

                return new object[] { 0, null };
            }
            finally
            {
                ReleaseConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), nwindConn, true);
            }
        }

        public object SetUserMenus(object[] objParam)
        {
            String strUserID = objParam[0].ToString();
            String strItemType = objParam[2].ToString();
            ClientType ct = ClientType.ctMsSql;
            IDbConnection nwindConn = AllocateConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), ref ct, true);
            try
            {
                //delete groups which will possibly be repeated later
                string strDel = "delete from USERMENUS where USERID = '" + strUserID + "' and MENUID in (select MENUID from MENUTABLE where ITEMTYPE='" + strItemType + "')";

                //为了区分不同的数据库 by Rei
                InfoCommand cmd = new InfoCommand(ClientInfo);

                cmd.Connection = nwindConn;
                cmd.CommandText = strDel;
                cmd.ExecuteNonQuery();

                //add new groups
                if (objParam[1] != null && objParam[1].ToString() != "")
                {
                    string[] MenuID = ((string)objParam[1]).Split(';');

                    for (int i = 0; i < MenuID.Length; i++)
                        if (MenuID[i].ToString() != "" && strUserID != "")
                        {
                            string strInsert = "insert into USERMENUS (USERID, MENUID) values ('"
                               + strUserID + "', '" + MenuID[i].ToString() + "')";

                            //为了区分不同的数据库 by Rei
                            InfoCommand InsertCmd = new InfoCommand(ClientInfo);

                            InsertCmd.Connection = nwindConn;
                            InsertCmd.CommandText = strInsert;
                            InsertCmd.ExecuteNonQuery();
                        }
                }

                return new object[] { 0, null };
            }
            finally
            {
                ReleaseConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), nwindConn, true);
            }
        }

        public object SetGroupMenus(object[] objParam)
        {
            String strGroupID = objParam[0].ToString();
            String strItemType = objParam[2].ToString();
            ClientType ct = ClientType.ctMsSql;
            IDbConnection nwindConn = AllocateConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), ref ct, true);
            try
            {
                //delete groups which will possibly be repeated later
                string strDel = "delete from GROUPMENUS where GROUPID = '" + strGroupID + "' and MENUID in (select MENUID from MENUTABLE where ITEMTYPE='" + strItemType + "')";

                //为了区分不同的数据库 by Rei
                InfoCommand cmd = new InfoCommand(ClientInfo);

                cmd.Connection = nwindConn;
                cmd.CommandText = strDel;
                cmd.ExecuteNonQuery();

                //add new groups
                if (objParam[1] != null && objParam[1].ToString() != "")
                {
                    string[] MenuID = ((string)objParam[1]).Split(';');

                    for (int i = 0; i < MenuID.Length; i++)
                        if (MenuID[i].ToString() != "" && strGroupID != "")
                        {
                            string strInsert = "insert into GROUPMENUS (GROUPID, MENUID) values ('"
                               + strGroupID + "', '" + MenuID[i].ToString() + "')";

                            //为了区分不同的数据库 by Rei
                            InfoCommand InsertCmd = new InfoCommand(ClientInfo);

                            InsertCmd.Connection = nwindConn;
                            InsertCmd.CommandText = strInsert;
                            InsertCmd.ExecuteNonQuery();
                        }
                }

                return new object[] { 0, null };
            }
            finally
            {
                ReleaseConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), nwindConn, true);
            }
        }

        public object LoadUsers(object[] objParam)
        {
            string strMenuID = (String)objParam[0];
            ClientType ct = ClientType.ctMsSql;
            IDbConnection nwindConn = AllocateConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), ref ct, true);
            try
            {
                string strSql;
                if (ServerConfig.LoginObjectEnabled)
                {
                    strSql = "select USERMENUS.*, USERMENUS.USERID AS USERNAME  from USERMENUS where USERMENUS.MENUID = '" + strMenuID + "'";
                }
                else
                {
                    strSql = "select USERMENUS.*, USERS.USERNAME from USERMENUS left join USERS on USERS.USERID = USERMENUS.USERID where USERMENUS.MENUID = '" + strMenuID + "'";
                }

                //为了区分不同的数据库 by Rei
                InfoCommand myCommand = new InfoCommand(ClientInfo);

                myCommand.Connection = nwindConn;
                myCommand.CommandText = strSql;

                IDataAdapter adpater = DBUtils.CreateDbDataAdapter(myCommand);

                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                (adpater as DbDataAdapter).Fill(dt);
                ds.Tables.Add(dt);

                return new object[] { 0, ds };
            }
            finally
            {
                ReleaseConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), nwindConn, true);
            }
        }

        public object SetUsers(object[] objParam)
        {
            string strMenuID = (String)objParam[0];
            ClientType ct = ClientType.ctMsSql;
            IDbConnection nwindConn = AllocateConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), ref ct, true);
            try
            {
                ArrayList GroupID = (ArrayList)objParam[1];

                //delete groups which will possibly be repeated later
                string strDel = "delete from USERMENUS where MENUID = '" + strMenuID + "'";

                //为了区分不同的数据库 by Rei
                InfoCommand cmd = new InfoCommand(ClientInfo);

                cmd.Connection = nwindConn;
                cmd.CommandText = strDel;
                cmd.ExecuteNonQuery();

                //modify by lily 2011/5/26 刪除聯動的MenuControl的table內容
                string lst = "";
                for (int i = 0; i < GroupID.Count; i++)
                {
                    lst = lst + "','" + GroupID[i].ToString();
                }
                if (lst.Length > 3)
                {
                    strDel = "delete from USERMENUCONTROL where MENUID='" + strMenuID + "' and USERID not in ('" + lst + "')";
                }
                cmd.CommandText = strDel;
                cmd.ExecuteNonQuery();

                //add new groups
                for (int i = 0; i < GroupID.Count; i++)
                {
                    string strInsert = "insert into USERMENUS (USERID, MENUID) values ('"
                        + GroupID[i].ToString() + "', '" + strMenuID + "')";

                    //为了区分不同的数据库 by Rei
                    InfoCommand InsertCmd = new InfoCommand(ClientInfo);

                    InsertCmd.Connection = nwindConn;
                    InsertCmd.CommandText = strInsert;
                    InsertCmd.ExecuteNonQuery();
                }

                return new object[] { 0, null };
            }
            finally
            {
                ReleaseConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), nwindConn, true);
            }
        }

        public object SetSingleSignOnPath(object[] objParam)
        {
            String ComputerName = objParam[0].ToString();
            String Path = objParam[1].ToString();

            String spath = Application.StartupPath + "\\SingleSignOnPath.xml";
            XmlDocument DBXML = new XmlDocument();
            FileStream aFileStream;
            if (!File.Exists(spath))
            {
                aFileStream = new FileStream(spath, FileMode.Create);
                try
                {
                    XmlTextWriter w = new XmlTextWriter(aFileStream, new System.Text.ASCIIEncoding());
                    w.Formatting = Formatting.Indented;
                    w.WriteStartElement("SingleSignOnPath");
                    w.WriteEndElement();
                    w.Close();
                }
                finally
                {
                    aFileStream.Close();
                }
            }
            try
            {
                aFileStream = new FileStream(spath, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
                try
                {
                    DBXML.Load(aFileStream);
                    XmlNode aNode = null;

                    for (int j = DBXML.DocumentElement.ChildNodes.Count - 1; j >= 0; j--)
                        if (DBXML.DocumentElement.ChildNodes[j].Attributes["ComputerName"].Value.Trim().Equals(ComputerName))
                        {
                            aNode = DBXML.DocumentElement.ChildNodes[j];
                            break;
                        }

                    if (aNode == null)
                    {
                        XmlElement elem = DBXML.CreateElement("String");
                        XmlAttribute attr = DBXML.CreateAttribute("ComputerName");
                        attr.Value = ComputerName;
                        elem.Attributes.Append(attr);

                        attr = DBXML.CreateAttribute("Path");
                        attr.Value = Path;
                        elem.Attributes.Append(attr);

                        DBXML.DocumentElement.AppendChild(elem);
                        aFileStream.Close();
                    }
                    else
                    {
                        aNode.Attributes["Path"].Value = Path;
                    }
                }
                finally
                {
                    aFileStream.Close();
                }
                DBXML.Save(spath);
            }
            catch { }

            return new object[] { 0 };
        }

        public object GetSingleSignOnPath(object[] objParam)
        {
            String ComputerName = objParam[0].ToString();
            String Path = "";
            if (File.Exists(SystemFile.SingleSignOnPathFile))
            {
                XmlDocument DBXML = new XmlDocument();
                FileStream aFileStream = new FileStream(SystemFile.SingleSignOnPathFile, FileMode.Open, FileAccess.Read, FileShare.None);
                DBXML.Load(aFileStream);
                XmlNode aNode = null;

                for (int j = DBXML.DocumentElement.ChildNodes.Count - 1; j >= 0; j--)
                    if (DBXML.DocumentElement.ChildNodes[j].Attributes["ComputerName"].Value.Trim().Equals(ComputerName))
                    {
                        aNode = DBXML.DocumentElement.ChildNodes[j];
                        break;
                    }

                try
                {
                    if (aNode != null)
                    {
                        Path = aNode.Attributes["Path"].InnerText;
                    }
                }
                finally
                {
                    aFileStream.Close();
                }
            }
            return new object[] { 0, Path };
        }

        //ILogin 不用
        public object SetUserGroups(object[] objParam)
        {
            string strGroupID = (String)objParam[0];
            ClientType ct = ClientType.ctMsSql;
            IDbConnection nwindConn = AllocateConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), ref ct, true);
            try
            {

                //delete groups which will possibly be repeated later
                string strDel = "delete from USERGROUPS where GROUPID = '" + strGroupID + "'";

                //为了区分不同的数据库 by Rei
                InfoCommand cmd = new InfoCommand(ClientInfo);

                cmd.Connection = nwindConn;
                cmd.CommandText = strDel;
                cmd.ExecuteNonQuery();

                //add new groups
                if (objParam[1] != null && objParam[1].ToString() != "")
                {
                    string[] UserID = ((string)objParam[1]).Split(';');

                    for (int i = 0; i < UserID.Length; i++)
                        if (UserID[i].ToString() != "" && strGroupID != "")
                        {
                            string strInsert = "insert into USERGROUPS (USERID, GROUPID) values ('"
                               + UserID[i].ToString() + "', '" + strGroupID + "')";

                            //为了区分不同的数据库 by Rei
                            InfoCommand InsertCmd = new InfoCommand(ClientInfo);

                            InsertCmd.Connection = nwindConn;
                            InsertCmd.CommandText = strInsert;
                            InsertCmd.ExecuteNonQuery();
                        }
                }

                return new object[] { 0, null };
            }
            finally
            {
                ReleaseConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), nwindConn, true);
            }
        }

        public object GetGroup(object[] objParam)
        {
            string str = (String)objParam[0];
            string[] id = str.Split(';');
            ClientType ct = ClientType.ctMsSql;
            IDbConnection nwindConn = AllocateConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), ref ct, true);
            try
            {
                string strSql = "";
                if (nwindConn is SqlConnection)
                    strSql = "select [GROUPMENUCONTROL].[GROUPID],[GROUPMENUCONTROL].[MENUID],[GROUPMENUCONTROL].[CONTROLNAME],[MENUTABLECONTROL].[DESCRIPTION],[GROUPMENUCONTROL].[TYPE],[GROUPMENUCONTROL].[ENABLED]"
                                   + ",[GROUPMENUCONTROL].[VISIBLE],[GROUPMENUCONTROL].[ALLOWADD],[GROUPMENUCONTROL].[ALLOWUPDATE],[GROUPMENUCONTROL].[ALLOWDELETE],[GROUPMENUCONTROL].[ALLOWPRINT] from [GROUPMENUCONTROL]"
                                   + " left join [MENUTABLECONTROL] on [GROUPMENUCONTROL].[MENUID]=[MENUTABLECONTROL].[MENUID] and [GROUPMENUCONTROL].[CONTROLNAME]=[MENUTABLECONTROL].[CONTROLNAME]"
                                   + " where [GROUPMENUCONTROL].[GROUPID] = '" + id[0] + "' and [GROUPMENUCONTROL].[MENUID] = '" + id[1] + "'";
                else if (nwindConn is OracleConnection)
                    strSql = "select GROUPMENUCONTROL.GROUPID,GROUPMENUCONTROL.MENUID,GROUPMENUCONTROL.CONTROLNAME,MENUTABLECONTROL.DESCRIPTION,GROUPMENUCONTROL.TYPE,GROUPMENUCONTROL.ENABLED"
                                   + ",GROUPMENUCONTROL.VISIBLE,GROUPMENUCONTROL.ALLOWADD,GROUPMENUCONTROL.ALLOWUPDATE,GROUPMENUCONTROL.ALLOWDELETE,GROUPMENUCONTROL.ALLOWPRINT from GROUPMENUCONTROL"
                                   + " left join MENUTABLECONTROL on GROUPMENUCONTROL.MENUID=MENUTABLECONTROL.MENUID and GROUPMENUCONTROL.CONTROLNAME=MENUTABLECONTROL.CONTROLNAME"
                                   + " where GROUPMENUCONTROL.GROUPID = '" + id[0] + "' and GROUPMENUCONTROL.MENUID = '" + id[1] + "'";
                else if (nwindConn is OdbcConnection)
                    strSql = "select GROUPMENUCONTROL.GROUPID,GROUPMENUCONTROL.MENUID,GROUPMENUCONTROL.CONTROLNAME,MENUTABLECONTROL.DESCRIPTION,GROUPMENUCONTROL.TYPE,GROUPMENUCONTROL.ENABLED"
                                   + ",GROUPMENUCONTROL.VISIBLE,GROUPMENUCONTROL.ALLOWADD,GROUPMENUCONTROL.ALLOWUPDATE,GROUPMENUCONTROL.ALLOWDELETE,GROUPMENUCONTROL.ALLOWPRINT from GROUPMENUCONTROL"
                                   + ", MENUTABLECONTROL where GROUPMENUCONTROL.MENUID=MENUTABLECONTROL.MENUID and GROUPMENUCONTROL.CONTROLNAME=MENUTABLECONTROL.CONTROLNAME"
                                   + " and GROUPMENUCONTROL.GROUPID = '" + id[0] + "' and GROUPMENUCONTROL.MENUID = '" + id[1] + "'";
                else if (nwindConn is OleDbConnection)
                    strSql = "select GROUPMENUCONTROL.GROUPID,GROUPMENUCONTROL.MENUID,GROUPMENUCONTROL.CONTROLNAME,MENUTABLECONTROL.DESCRIPTION,GROUPMENUCONTROL.TYPE,GROUPMENUCONTROL.ENABLED"
                                   + ",GROUPMENUCONTROL.VISIBLE,GROUPMENUCONTROL.ALLOWADD,GROUPMENUCONTROL.ALLOWUPDATE,GROUPMENUCONTROL.ALLOWDELETE,GROUPMENUCONTROL.ALLOWPRINT from GROUPMENUCONTROL"
                                   + " left join MENUTABLECONTROL on GROUPMENUCONTROL.MENUID=MENUTABLECONTROL.MENUID and GROUPMENUCONTROL.CONTROLNAME=MENUTABLECONTROL.CONTROLNAME"
                                   + " where GROUPMENUCONTROL.GROUPID = '" + id[0] + "' and GROUPMENUCONTROL.MENUID = '" + id[1] + "'";
                else if (nwindConn.GetType().Name == "MySqlConnection")
                    strSql = "select GROUPMENUCONTROL.GROUPID,GROUPMENUCONTROL.MENUID,GROUPMENUCONTROL.CONTROLNAME,MENUTABLECONTROL.DESCRIPTION,GROUPMENUCONTROL.TYPE,GROUPMENUCONTROL.ENABLED"
                                   + ",GROUPMENUCONTROL.VISIBLE,GROUPMENUCONTROL.ALLOWADD,GROUPMENUCONTROL.ALLOWUPDATE,GROUPMENUCONTROL.ALLOWDELETE,GROUPMENUCONTROL.ALLOWPRINT from GROUPMENUCONTROL"
                                   + " left join MENUTABLECONTROL on GROUPMENUCONTROL.MENUID=MENUTABLECONTROL.MENUID and GROUPMENUCONTROL.CONTROLNAME=MENUTABLECONTROL.CONTROLNAME"
                                   + " where GROUPMENUCONTROL.GROUPID = '" + id[0] + "' and GROUPMENUCONTROL.MENUID = '" + id[1] + "'";
                else if (nwindConn.GetType().Name == "IfxConnection")
                    strSql = "select GROUPMENUCONTROL.GROUPID,GROUPMENUCONTROL.MENUID,GROUPMENUCONTROL.CONTROLNAME,MENUTABLECONTROL.DESCRIPTION,GROUPMENUCONTROL.TYPE,GROUPMENUCONTROL.ENABLED"
                                   + ",GROUPMENUCONTROL.VISIBLE,GROUPMENUCONTROL.ALLOWADD,GROUPMENUCONTROL.ALLOWUPDATE,GROUPMENUCONTROL.ALLOWDELETE,GROUPMENUCONTROL.ALLOWPRINT from GROUPMENUCONTROL"
                                   + ", MENUTABLECONTROL where GROUPMENUCONTROL.MENUID=MENUTABLECONTROL.MENUID and GROUPMENUCONTROL.CONTROLNAME=MENUTABLECONTROL.CONTROLNAME"
                                   + " and GROUPMENUCONTROL.GROUPID = '" + id[0] + "' and GROUPMENUCONTROL.MENUID = '" + id[1] + "'";


                //为了区分不同的数据库 by Rei
                InfoCommand cmd = new InfoCommand(ClientInfo);

                cmd.CommandText = strSql;
                cmd.Connection = nwindConn;
                IDataAdapter adpater = DBUtils.CreateDbDataAdapter(cmd);

                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                (adpater as DbDataAdapter).Fill(dt);
                ds.Tables.Add(dt);
                return new object[] { 0, ds };
            }
            finally
            {
                ReleaseConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), nwindConn, true);
            }
        }

        public object GetUserGroup(object[] objParam)
        {
            string userid = objParam[0].ToString();
            string groupid = "";
            string groupname = string.Empty;
            ClientType ct = ClientType.ctMsSql;
            IDbConnection nwindConn = AllocateConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), ref ct, true);
            try
            {
                string strSql = "select USERGROUPS.USERID,USERGROUPS.GROUPID,GROUPS.GROUPNAME FROM USERGROUPS LEFT JOIN GROUPS ON USERGROUPS.GROUPID=GROUPS.GROUPID where USERID ='" + userid + "'";

                //为了区分不同的数据库 by Rei
                InfoCommand cmd = new InfoCommand(ClientInfo);

                cmd.CommandText = strSql;
                cmd.Connection = nwindConn;
                IDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    groupid += dr["GROUPID"].ToString() + ";";
                    groupname += dr["GROUPNAME"].ToString() + ";";
                }
                if (groupid != "")
                {
                    groupid = groupid.Substring(0, groupid.LastIndexOf(';'));
                    groupname = groupname.Substring(0, groupname.LastIndexOf(';'));
                }
                cmd.Cancel();
                dr.Close();

                return new object[] { 0, groupid, groupname };
            }
            finally
            {
                ReleaseConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), nwindConn, true);
            }
        }

        //FL use Cancel
        public object GetUserRole(object[] objParam)
        {
            ClientType ct = ClientType.ctNone;
            IDbConnection nwindConn = AllocateConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), ref ct, true);
            try
            {
                string user = (string)GetClientInfo(ClientInfoType.LoginUser);
                string orgKind = (string)GetClientInfo(ClientInfoType.OrgKind);
                StringBuilder role = new StringBuilder();
                StringBuilder orgRole = new StringBuilder();
                StringBuilder orgShare = new StringBuilder();

                StringBuilder groupList = new StringBuilder();
                InfoCommand cmd = new InfoCommand(ClientInfo);
                cmd.Connection = nwindConn;
                cmd.CommandText = String.Format("SELECT GROUPID FROM GROUPS WHERE GROUPID IN (SELECT GROUPID FROM USERGROUPS WHERE USERID='{0}') AND ISROLE='Y'"
                                    , user);
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (role.Length > 0)
                        {
                            role.Append(';');
                            groupList.Append(',');
                        }
                        role.Append((string)reader["GROUPID"]);
                        groupList.Append(string.Format("'{0}'", reader["GROUPID"]));
                    }
                    cmd.Cancel();
                    reader.Close();
                }
                if (role.Length > 0)
                {
                    orgRole.Append(role);
                    cmd = new InfoCommand(ClientInfo);
                    cmd.Connection = nwindConn;
                    cmd.CommandText = string.Format("Select ORG_NO From SYS_ORG Where ORG_MAN IN({0}) and ORG_KIND='{1}'", groupList, orgKind);
                    StringBuilder orglist = new StringBuilder();
                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (orglist.Length > 0)
                            {
                                orglist.Append(',');
                            }
                            orglist.Append(string.Format("'{0}'", reader["ORG_NO"]));
                        }
                        cmd.Cancel();
                        reader.Close();
                    }
                    if (orglist.Length > 0)//找到Org_No
                    {
                        StringBuilder orgParentlist = new StringBuilder();
                        orgParentlist.Append(orglist);
                        while (true)//递归找到所有的子org
                        {
                            cmd = new InfoCommand(ClientInfo);
                            cmd.Connection = nwindConn;
                            cmd.CommandText = string.Format("Select ORG_NO,ORG_MAN From SYS_ORG Where UPPER_ORG IN ({0})", orgParentlist);
                            using (IDataReader reader = cmd.ExecuteReader())
                            {
                                orgParentlist = new StringBuilder();
                                while (reader.Read())
                                {
                                    orglist.Append(',');
                                    orglist.Append(string.Format("'{0}'", reader["ORG_NO"]));
                                    if (orgParentlist.Length > 0)
                                    {
                                        orgParentlist.Append(',');
                                    }
                                    orgParentlist.Append(string.Format("'{0}'", reader["ORG_NO"]));
                                    orgRole.Append(';');
                                    orgRole.Append((string)reader["ORG_MAN"]);
                                }
                                cmd.Cancel();
                                reader.Close();
                                if (orgParentlist.Length == 0)//找到底了
                                {
                                    break;
                                }
                            }
                        }
                        cmd = new InfoCommand(ClientInfo);
                        cmd.Connection = nwindConn;
                        cmd.CommandText = string.Format("Select ROLE_ID From SYS_ORGROLES WHERE ORG_NO IN ({0})", orglist);
                        using (IDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                orgRole.Append(';');
                                orgRole.Append((string)reader["ROLE_ID"]);
                            }
                            cmd.Cancel();
                            reader.Close();
                        }
                        orgShare.Append(orgRole);
                    }
                    else
                    {
                        orgShare.Append(role);
                        cmd = new InfoCommand(ClientInfo);
                        cmd.Connection = nwindConn;
                        cmd.CommandText = string.Format("Select ROLE_ID From SYS_ORGROLES WHERE ORG_NO IN (Select ORG_NO From SYS_ORGROLES Where ROLE_ID IN ({0}))", groupList);
                        using (IDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                orgShare.Append(';');
                                orgShare.Append((string)reader["ROLE_ID"]);
                            }
                            cmd.Cancel();
                            reader.Close();
                        }
                    }
                }
                return new object[] { 0, role.ToString(), orgRole.ToString(), orgShare.ToString() };
            }
            finally
            {
                ReleaseConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), nwindConn, true);
            }
        }

        //ILogin不用
        public object ListUsers(object[] objParam)
        {
            string groupid = objParam[0].ToString();
            ClientType ct = ClientType.ctMsSql;
            IDbConnection nwindConn = AllocateConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), ref ct, true);
            try
            {
                string strSql = string.Format("select USERS.USERID, USERS.USERNAME from USERGROUPS left join USERS on USERGROUPS.USERID = USERS.USERID where USERGROUPS.GROUPID ='{0}' order by USERS.USERID", groupid);
                //为了区分不同的数据库 by Rei
                InfoCommand cmd = new InfoCommand(ClientInfo);
                cmd.CommandText = strSql;
                cmd.Connection = nwindConn;
                IDataAdapter adpater = DBUtils.CreateDbDataAdapter(cmd);

                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                (adpater as DbDataAdapter).Fill(dt);
                ds.Tables.Add(dt);
                return new object[] { 0, ds };
            }
            finally
            {
                ReleaseConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), nwindConn, true);
            }
        }

        public object GetUser(object[] objParam)
        {
            string str = (String)objParam[0];
            string[] id = str.Split(';');
            ClientType ct = ClientType.ctMsSql;
            IDbConnection nwindConn = AllocateConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), ref ct, true);
            try
            {
                string strSql = "";
                if (nwindConn is SqlConnection)
                    strSql = "select [USERMENUCONTROL].[USERID],[USERMENUCONTROL].[MENUID],[USERMENUCONTROL].[CONTROLNAME],[MENUTABLECONTROL].[DESCRIPTION],[USERMENUCONTROL].[TYPE],[USERMENUCONTROL].[ENABLED]"
                                   + ",[USERMENUCONTROL].[VISIBLE],[USERMENUCONTROL].[ALLOWADD],[USERMENUCONTROL].[ALLOWUPDATE],[USERMENUCONTROL].[ALLOWDELETE],[USERMENUCONTROL].[ALLOWPRINT] from [USERMENUCONTROL]"
                                   + " left join [MENUTABLECONTROL] on [USERMENUCONTROL].[MENUID]=[MENUTABLECONTROL].[MENUID] and [USERMENUCONTROL].[CONTROLNAME]=[MENUTABLECONTROL].[CONTROLNAME]"
                                   + " where [USERMENUCONTROL].[USERID] = '" + id[0] + "' and [USERMENUCONTROL].[MENUID] = '" + id[1] + "'";
                else if (nwindConn is OracleConnection)
                    strSql = "select USERMENUCONTROL.USERID,USERMENUCONTROL.MENUID,USERMENUCONTROL.CONTROLNAME,MENUTABLECONTROL.DESCRIPTION,USERMENUCONTROL.TYPE,USERMENUCONTROL.ENABLED"
                                   + ",USERMENUCONTROL.VISIBLE,USERMENUCONTROL.ALLOWADD,USERMENUCONTROL.ALLOWUPDATE,USERMENUCONTROL.ALLOWDELETE,USERMENUCONTROL.ALLOWPRINT from USERMENUCONTROL"
                                   + " left join MENUTABLECONTROL on USERMENUCONTROL.MENUID=MENUTABLECONTROL.MENUID and USERMENUCONTROL.CONTROLNAME=MENUTABLECONTROL.ControlName"
                                   + " where USERMENUCONTROL.USERID = '" + id[0] + "' and USERMENUCONTROL.MENUID = '" + id[1] + "'";
                else if (nwindConn is OdbcConnection)
                    strSql = "select USERMENUCONTROL.USERID,USERMENUCONTROL.MENUID,USERMENUCONTROL.CONTROLNAME,MENUTABLECONTROL.DESCRIPTION,USERMENUCONTROL.TYPE,USERMENUCONTROL.ENABLED"
                                   + ",USERMENUCONTROL.VISIBLE,USERMENUCONTROL.ALLOWADD,USERMENUCONTROL.ALLOWUPDATE,USERMENUCONTROL.ALLOWDELETE,USERMENUCONTROL.ALLOWPRINT from USERMENUCONTROL"
                                   + ", MENUTABLECONTROL where USERMENUCONTROL.MENUID=MENUTABLECONTROL.MENUID and USERMENUCONTROL.CONTROLNAME=MENUTABLECONTROL.CONTROLNAME"
                                   + " and USERMENUCONTROL.USERID = '" + id[0] + "' and USERMENUCONTROL.MENUID = '" + id[1] + "'";
                else if (nwindConn is OleDbConnection)
                    strSql = "select USERMENUCONTROL.USERID,USERMENUCONTROL.MENUID,USERMENUCONTROL.CONTROLNAME,MENUTABLECONTROL.DESCRIPTION,USERMENUCONTROL.TYPE,USERMENUCONTROL.ENABLED"
                                + ",USERMENUCONTROL.VISIBLE,USERMENUCONTROL.ALLOWADD,USERMENUCONTROL.ALLOWUPDATE,USERMENUCONTROL.ALLOWDELETE,USERMENUCONTROL.ALLOWPRINT from USERMENUCONTROL"
                                + " left join MENUTABLECONTROL on USERMENUCONTROL.MENUID=MENUTABLECONTROL.MENUID and USERMENUCONTROL.CONTROLNAME=MENUTABLECONTROL.CONTROLNAME"
                                + " where USERMENUCONTROL.USERID = '" + id[0] + "' and USERMENUCONTROL.MENUID = '" + id[1] + "'";
                else if (nwindConn.GetType().Name == "MySqlConnection")
                    strSql = "select USERMENUCONTROL.USERID,USERMENUCONTROL.MENUID,USERMENUCONTROL.CONTROLNAME,MENUTABLECONTROL.DESCRIPTION,USERMENUCONTROL.TYPE,USERMENUCONTROL.ENABLED"
                                + ",USERMENUCONTROL.VISIBLE,USERMENUCONTROL.ALLOWADD,USERMENUCONTROL.ALLOWUPDATE,USERMENUCONTROL.ALLOWDELETE,USERMENUCONTROL.ALLOWPRINT from USERMENUCONTROL"
                                + " left join MENUTABLECONTROL on USERMENUCONTROL.MENUID=MENUTABLECONTROL.MENUID and USERMENUCONTROL.CONTROLNAME=MENUTABLECONTROL.CONTROLNAME"
                                + " where USERMENUCONTROL.USERID = '" + id[0] + "' and USERMENUCONTROL.MENUID = '" + id[1] + "'";
                else if (nwindConn.GetType().Name == "IfxConnection")
                    strSql = "select USERMENUCONTROL.USERID,USERMENUCONTROL.MENUID,USERMENUCONTROL.CONTROLNAME,MENUTABLECONTROL.DESCRIPTION,USERMENUCONTROL.TYPE,USERMENUCONTROL.ENABLED"
                                   + ",USERMENUCONTROL.VISIBLE,USERMENUCONTROL.ALLOWADD,USERMENUCONTROL.ALLOWUPDATE,USERMENUCONTROL.ALLOWDELETE,USERMENUCONTROL.ALLOWPRINT from USERMENUCONTROL"
                                   + ", MENUTABLECONTROL where USERMENUCONTROL.MENUID=MENUTABLECONTROL.MENUID and USERMENUCONTROL.CONTROLNAME=MENUTABLECONTROL.CONTROLNAME"
                                   + " and USERMENUCONTROL.USERID = '" + id[0] + "' and USERMENUCONTROL.MENUID = '" + id[1] + "'";

                //为了区分不同的数据库 by Rei
                InfoCommand cmd = new InfoCommand(ClientInfo);

                cmd.CommandText = strSql;
                cmd.Connection = nwindConn;
                IDataAdapter adpater = DBUtils.CreateDbDataAdapter(cmd);

                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                (adpater as DbDataAdapter).Fill(dt);
                ds.Tables.Add(dt);
                return new object[] { 0, ds };
            }
            finally
            {
                ReleaseConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), nwindConn, true);
            }
        }

        public object GetGroupControl(object[] objParam)
        {
            string str = (String)objParam[0];
            string[] id = str.Split(';');
            ClientType ct = ClientType.ctMsSql;
            IDbConnection nwindConn = AllocateConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), ref ct, true);
            try
            {
                string groupids = "";
                if (ServerConfig.LoginObjectEnabled)
                {
                    string[] groups = ServerConfig.LoginObject.GetUserGroups(id[0], (string)GetClientInfo(ClientInfoType.Password));
                    foreach (string group in groups)
                    {
                        if (groupids.Length > 0)
                        {
                            groupids += ",";
                        }
                        groupids += string.Format("'{0}'", group);
                    }
                    if (groupids.Length == 0)
                    {
                        groupids = "''";
                    }
                }
                else
                {
                    if (((object[])ClientInfo[0]).Length > 18 && ((object[])ClientInfo[0])[18] != null && ((object[])ClientInfo[0])[18].ToString() != "")
                    {
                        groupids = "'" + ((object[])ClientInfo[0])[18].ToString() + "'";
                    }
                    else
                    {
                        groupids = "select GROUPID from USERGROUPS where USERID = '" + id[0] + "'";
                    }
                }
                string strSql = "select * from GROUPMENUCONTROL where (GROUPID in (" + groupids + ") or GROUPID = '00') and MENUID = '" + id[1] + "'";
                //为了区分不同的数据库 by Rei
                InfoCommand cmd = new InfoCommand(ClientInfo);

                cmd.CommandText = strSql;
                cmd.Connection = nwindConn;
                IDataAdapter adpater = DBUtils.CreateDbDataAdapter(cmd);

                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                (adpater as DbDataAdapter).Fill(dt);
                ds.Tables.Add(dt);
                return new object[] { 0, ds };
            }
            finally
            {
                ReleaseConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), nwindConn, true);
            }
        }

        public object GetUserControl(object[] objParam)
        {
            string str = (String)objParam[0];
            string[] id = str.Split(';');
            ClientType ct = ClientType.ctMsSql;
            IDbConnection nwindConn = AllocateConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), ref ct, true);
            try
            {
                string strSql = "select * from USERMENUCONTROL where USERID = '" + id[0] + "' and MENUID = '" + id[1] + "'";

                //为了区分不同的数据库 by Rei
                InfoCommand cmd = new InfoCommand(ClientInfo);

                cmd.CommandText = strSql;
                cmd.Connection = nwindConn;
                IDataAdapter adpater = DBUtils.CreateDbDataAdapter(cmd);

                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                (adpater as DbDataAdapter).Fill(dt);
                ds.Tables.Add(dt);
                return new object[] { 0, ds };
            }
            finally
            {
                ReleaseConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), nwindConn, true);
            }
        }

        public object GetNewGroup(object[] objParam)
        {
            string str = (String)objParam[0];
            string[] id = str.Split(';');
            ClientType ct = ClientType.ctMsSql;
            IDbConnection nwindConn = AllocateConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), ref ct, true);
            try
            {
                string strSql = "select * from GROUPMENUCONTROL where GROUPID = '" + id[0] + "'and MENUID = '" + id[1] + "'and CONTROLNAME in (select CONTROLNAME From MENUTABLECONTROL Where MENUID='" + id[1] + "')";

                //为了区分不同的数据库 by Rei
                InfoCommand cmd = new InfoCommand(ClientInfo);

                cmd.CommandText = strSql;
                cmd.Connection = nwindConn;
                IDataAdapter adpater = DBUtils.CreateDbDataAdapter(cmd);

                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                (adpater as DbDataAdapter).Fill(dt);
                ds.Tables.Add(dt);
                return new object[] { 0, ds };
            }
            finally
            {
                ReleaseConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), nwindConn, true);
            }
        }

        public object GetServerPath(object[] objParam)
        {
            return new object[] { 0, EEPRegistry.Server };
        }

        public object GetSrvoolAssemblyName(object[] objParam)
        {
            Assembly assembly = Assembly.LoadFile(EEPRegistry.Server + "\\Srvtools.dll");
            if (assembly != null)
            {
                return new object[] { 0, assembly.FullName };
            }
            return new object[] { 1, "" };
        }

        public object UpdateNodes(object[] objParam)
        {
            string strMenuID = (String)objParam[0];
            string strParent = (String)objParam[1];

            ClientType ct = ClientType.ctMsSql;
            IDbConnection nwindConn = AllocateConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), ref ct, true);
            try
            {
                string strSql;
                if (strParent != null)
                {
                    strSql = "update MENUTABLE set PARENT = '" + strParent + "' where MENUID = '" + strMenuID + "'";
                }
                else
                {
                    strSql = "update MENUTABLE set PARENT = null where MENUID = '" + strMenuID + "'";
                }

                //为了区分不同的数据库 by Rei
                InfoCommand cmd = new InfoCommand(ClientInfo);

                cmd.CommandText = strSql;
                cmd.Connection = nwindConn;
                cmd.ExecuteNonQuery();

                return new object[] { 0, null };
            }
            finally
            {
                ReleaseConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), nwindConn, true);
            }
        }

        public object GetSolution(object[] objParam)
        {
            ClientType ct = ClientType.ctMsSql;
            IDbConnection nwindConn;
            //if (((object[])ClientInfo[0])[2] != null && GetClientInfo(ClientInfoType.LoginDB).ToString() != "")
            //{
            //    nwindConn = AllocateConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), ref ct, true);
            //}
            //else
            //{
            //    nwindConn = AllocateConnection((string)objParam[0], ref ct, true);
            //}
            String DBName = GetSystemDBName();
            nwindConn = AllocateConnection(DBName, ref ct, true);

            try
            {
                string strSql = "select * from MENUITEMTYPE";

                //为了区分不同的数据库 by Rei
                InfoCommand cmd = new InfoCommand(ClientInfo);   //solution 全部从SystemTablez中获取

                cmd.CommandText = strSql;
                cmd.Connection = nwindConn;
                IDataAdapter adpater = DBUtils.CreateDbDataAdapter(cmd);

                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                (adpater as DbDataAdapter).Fill(dt);
                ds.Tables.Add(dt);

                return new object[] { 0, ds };
            }
            finally
            {
                ReleaseConnection(DBName, nwindConn, true);
            }
        }

        public object GetSolutionSecurity(object[] objParam)
        {
            String UserID = objParam[0].ToString();
            String[] GroupID = objParam[1].ToString().Split(';');
            String Groups = "";
            for (int i = 0; i < GroupID.Length; i++)
            {
                Groups += "GROUPID='" + GroupID[i] + "' or ";
            }
            Groups = Groups.Remove(Groups.LastIndexOf("or"));
            ClientType ct = ClientType.ctMsSql;
            IDbConnection nwindConn;
            String DBName = "";
            if (GetClientInfo(ClientInfoType.LoginDB).ToString().Length > 0)
                DBName = GetClientInfo(ClientInfoType.LoginDB).ToString();
            else
                DBName = GetSystemDBName();
            nwindConn = AllocateConnection(DBName, ref ct, true);
            try
            {
                string strSql = "Select distinct MENUTABLE.ITEMTYPE, MENUITEMTYPE.ITEMNAME from MENUTABLE LEFT JOIN MENUITEMTYPE ON MENUTABLE.ITEMTYPE=MENUITEMTYPE.ITEMTYPE Where MENUID IN ((Select MENUID from USERMENUS Where USERID='" + UserID + "') UNION (Select MENUID from GROUPMENUS Where " + Groups + ")) AND (PARENT IS NULL OR PARENT='')";

                //为了区分不同的数据库 by Rei
                InfoCommand cmd = new InfoCommand(ClientInfo);   //solution 全部从SystemTablez中获取

                cmd.CommandText = strSql;
                cmd.Connection = nwindConn;
                IDataAdapter adpater = DBUtils.CreateDbDataAdapter(cmd);

                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                (adpater as DbDataAdapter).Fill(dt);
                ds.Tables.Add(dt);

                return new object[] { 0, ds };
            }
            finally
            {
                ReleaseConnection(DBName, nwindConn, true);
            }
        }

        public object[] CheckAndDownLoad(object[] objParams)
        {
            string sCurPrj = (string)objParams[0];
            string sDll = (string)objParams[1];
            DateTime d = (DateTime)objParams[2];
            string clientname = "EEPNetClient";
            if (objParams.Length > 3)
            {
                if ((bool)objParams[3])
                {
                    clientname = "EEPNetFLClient";
                }
            }

            string sParent = Path.GetDirectoryName(EEPRegistry.Server);
            string sFile = string.Format("{0}\\{1}\\{2}\\{3}", sParent, clientname, sCurPrj, sDll);

            if (File.Exists(sFile))
            {
                DateTime ds = File.GetLastWriteTime(sFile);
                if (ds > d)
                {
                    byte[] bs = File.ReadAllBytes(sFile);
                    return new object[] { 0, 0, ds, (object)bs };
                }
                else
                {
                    return new object[] { 0, 1 };
                }
            }
            else
                return new object[] { 0, 1 };//1表示不用做了...
        }

        public object[] GetMessage(object[] objParams)
        {
            string sNow = FormatDateTime(DateTime.Now);
            string sLoginUser = GetClientInfo(ClientInfoType.LoginUser).ToString();

            ClientType ct = ClientType.ctMsSql;
            IDbConnection nwindConn = AllocateConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), ref ct, true);
            try
            {

                //为了区分不同的数据库 by Rei
                InfoCommand myCommand = new InfoCommand(ClientInfo);

                myCommand.Connection = nwindConn;
                string strSql = "";

                strSql = string.Format("select * from SYS_MESSENGER where STATUS = 'S' and USERID = '{0}'", sLoginUser);
                myCommand.CommandText = strSql;
                IDataAdapter adpater = DBUtils.CreateDbDataAdapter(myCommand);

                DataSet dsMessage = new DataSet();
                DataTable dt = new DataTable("Message");
                (adpater as DbDataAdapter).Fill(dt);
                dsMessage.Tables.Add(dt);

                strSql = string.Format("Update SYS_MESSENGER SET STATUS = 'R' where STATUS = 'S' and USERID = '{0}'", sLoginUser);
                myCommand.CommandText = strSql;
                myCommand.ExecuteNonQuery();

                return new object[] { 0, dsMessage };

            }
            finally
            {
                ReleaseConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), nwindConn, true);
            }
        }

        private string FormatDateTime(DateTime dateTime)
        {
            String marker = "{0:0#}";
            return dateTime.Year + String.Format(marker, dateTime.Month) + String.Format(marker, dateTime.Day)
                + String.Format(marker, dateTime.Hour) + String.Format(marker, dateTime.Minute)
                + String.Format(marker, dateTime.Second);// + String.Format(marker, dateTime.Millisecond);
        }

        public object[] SendMessage(object[] objParams)
        {
            bool bGroup = (bool)objParams[0];
            string sUser = (string)objParams[1];
            string sMsg = (string)objParams[2];
            string sParams = (string)objParams[3];
            string sNow = FormatDateTime(DateTime.Now);
            string sLoginUser = GetClientInfo(ClientInfoType.LoginUser).ToString();
            string status = "S";

            ClientType ct = ClientType.ctMsSql;
            IDbConnection nwindConn = AllocateConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), ref ct, true);
            try
            {
                //为了区分不同的数据库 by Rei
                InfoCommand myCommand = new InfoCommand(ClientInfo);

                myCommand.Connection = nwindConn;
                string strSql = "";

                ArrayList arrUsers = new ArrayList();
                if (bGroup)
                {
                    strSql = string.Format("select distinct USERID from USERGROUPS where GROUPID = '{0}'", sUser);
                    myCommand.CommandText = strSql;
                    IDataReader aReader = myCommand.ExecuteReader();
                    try
                    {
                        bool b = aReader.Read();
                        while (b)
                        {
                            arrUsers.Add(aReader.GetString(0));
                            b = aReader.Read();
                        }
                    }
                    finally
                    {
                        aReader.Close();
                    }
                }
                else
                {
                    arrUsers.Add(sUser);
                }

                for (int i = 0; i < arrUsers.Count; i++)
                {
                    strSql = string.Format("insert into SYS_MESSENGER (USERID, MESSAGE, PARAS, SENDTIME, SENDERID, STATUS) values('{0}', '{1}', '{2}', '{3}', '{4}', '{5}')",
                        (string)arrUsers[i], sMsg, sParams, sNow, sLoginUser, status);
                    myCommand.CommandText = strSql;
                    myCommand.ExecuteNonQuery();
                }

                return new object[] { 0 };
            }
            finally
            {
                ReleaseConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), nwindConn, true);
            }
        }

        public object GetDataDic(object[] objParam)
        {
            string strTableName = (String)objParam[0];
            ClientType ct = ClientType.ctMsSql;
            //modified by lily GetDD的时候，不能取SystemDB。
            IDbConnection nwindConn = AllocateConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), ref ct, false);
            try
            {
                string strSql = "select * from COLDEF where TABLE_NAME = '" + strTableName + "'";

                //为了区分不同的数据库 by Rei
                InfoCommand cmd = new InfoCommand(ClientInfo);

                cmd.CommandText = strSql;
                cmd.Connection = nwindConn;
                IDataAdapter adpater = DBUtils.CreateDbDataAdapter(cmd);

                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                (adpater as DbDataAdapter).Fill(dt);
                ds.Tables.Add(dt);

                return new object[] { 0, ds };
            }
            finally
            {
                ReleaseConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), nwindConn, false);
            }
        }

        public object GetMenuID(object[] objParam)
        {
            string TabName = (String)objParam[0];
            ClientType ct = ClientType.ctMsSql;
            IDbConnection nwindConn = AllocateConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), ref ct, true);
            try
            {
                string strSql = "select MENUID, CAPTION from MENUTABLE Order BY MENUID ASC";

                //为了区分不同的数据库 by Rei
                InfoCommand cmd = new InfoCommand(ClientInfo);

                cmd.CommandText = strSql;
                cmd.Connection = nwindConn;
                IDataAdapter adpater = DBUtils.CreateDbDataAdapter(cmd);

                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                (adpater as DbDataAdapter).Fill(dt);
                ds.Tables.Add(dt);

                ArrayList lstMenuID = new ArrayList();
                ArrayList lstCaption = new ArrayList();
                int i = ds.Tables[0].Rows.Count;
                for (int j = 0; j < i; j++)
                {
                    lstMenuID.Add(ds.Tables[0].Rows[j][0].ToString());
                    lstCaption.Add(ds.Tables[0].Rows[j][1].ToString());
                }
                return new object[] { 0, lstMenuID, lstCaption };
            }
            finally
            {
                ReleaseConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), nwindConn, true);
            }
        }

        public object GetFavorMenuID(object[] objParam)
        {
            String userID = objParam[0].ToString();
            String itemType = objParam[1].ToString();
            ArrayList menuID = objParam[2] as ArrayList;
            ArrayList caption = objParam[3] as ArrayList;
            String groupName = objParam[4].ToString();

            ClientType ct = ClientType.ctMsSql;
            IDbConnection nwindConn = AllocateConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), ref ct, true);
            try
            {
                InfoCommand cmd = new InfoCommand(ClientInfo);
                cmd.Connection = nwindConn;

                String group = "";
                if (groupName == "")
                {
                    if (nwindConn is SqlConnection)
                    {
                        group = "GROUPNAME='" + group + "'";
                    }
                    else if (nwindConn is OracleConnection)
                    {
                        group = "GROUPNAME is null";
                    }
                    else if (nwindConn is OdbcConnection)
                    {
                        group = "GROUPNAME='" + group + "'";
                    }
                    else if (nwindConn is OleDbConnection)
                    {
                        group = "GROUPNAME='" + group + "'";
                    }
                    else if (nwindConn.GetType().Name == "MySqlConnection")
                    {
                        group = "GROUPNAME='" + group + "'";
                    }
                    else if (nwindConn.GetType().Name == "IfxConnection")
                    {
                        group = "GROUPNAME='" + group + "'";
                    }
                }
                else
                {
                    group = "GROUPNAME='" + groupName + "'";
                }
                String strSql = "delete MENUFAVOR where USERID='" + userID + "' and ITEMTYPE='" + itemType + "' AND " + group;
                cmd.CommandText = strSql;
                cmd.Connection = nwindConn;
                cmd.ExecuteNonQuery();

                for (int i = 0; i < menuID.Count; i++)
                {
                    strSql = "insert into MENUFAVOR (MENUID, CAPTION, USERID, ITEMTYPE, GROUPNAME) values ('" + menuID[i] + "', '" + caption[i] + "', '" + userID + "', '" + itemType + "', '" + groupName + "')";
                    cmd.CommandText = strSql;
                    cmd.ExecuteNonQuery();
                }
                return new object[] { 0 };
            }
            finally
            {
                ReleaseConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), nwindConn, true);
            }
        }

        public object GetMenu(object[] objParam)
        {
            string MenuID = (String)objParam[0];
            ClientType ct = ClientType.ctMsSql;
            IDbConnection nwindConn = AllocateConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), ref ct, true);
            try
            {
                string strSql = "select * from MENUTABLECONTROL where MENUID = '" + MenuID + "'";

                //为了区分不同的数据库 by Rei
                InfoCommand cmd = new InfoCommand(ClientInfo);

                cmd.CommandText = strSql;
                cmd.Connection = nwindConn;
                IDataAdapter adpater = DBUtils.CreateDbDataAdapter(cmd);

                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                (adpater as DbDataAdapter).Fill(dt);
                ds.Tables.Add(dt);

                ArrayList lstControlName = new ArrayList();
                ArrayList lstDescription = new ArrayList();
                ArrayList lstType = new ArrayList();
                int i = ds.Tables[0].Rows.Count;
                for (int j = 0; j < i; j++)
                {
                    lstControlName.Add(ds.Tables[0].Rows[j]["CONTROLNAME"].ToString());
                    lstDescription.Add(ds.Tables[0].Rows[j]["DESCRIPTION"].ToString());
                    lstType.Add(ds.Tables[0].Rows[j]["TYPE"].ToString());
                }
                return new object[] { 0, lstControlName, lstDescription, lstType };
            }
            finally
            {
                ReleaseConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), nwindConn, true);
            }
        }

        public object GetLanguage(object[] objParam)
        {
            string MenuID = (String)objParam[0];
            ClientType ct = ClientType.ctMsSql;
            IDbConnection nwindConn = AllocateConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), ref ct, true);
            try
            {
                string strSql = "select * from MENUTABLE where MENUID = '" + MenuID + "'";

                //为了区分不同的数据库 by Rei
                InfoCommand cmd = new InfoCommand(ClientInfo);

                cmd.CommandText = strSql;
                cmd.Connection = nwindConn;
                IDataAdapter adpater = DBUtils.CreateDbDataAdapter(cmd);

                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                (adpater as DbDataAdapter).Fill(dt);
                ds.Tables.Add(dt);
                return new object[] { 0, ds };
            }
            finally
            {
                ReleaseConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), nwindConn, true);
            }
        }

        public object GetPF(object[] objParam)
        {
            string MenuID = (String)objParam[0];
            ClientType ct = ClientType.ctMsSql;
            IDbConnection nwindConn = AllocateConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), ref ct, true);
            try
            {
                string strSql = "select PACKAGE, FORM from MENUTABLE where MENUID = '" + MenuID + "'";

                //为了区分不同的数据库 by Rei
                InfoCommand cmd = new InfoCommand(ClientInfo);

                cmd.CommandText = strSql;
                cmd.Connection = nwindConn;
                IDataAdapter adpater = DBUtils.CreateDbDataAdapter(cmd);

                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                (adpater as DbDataAdapter).Fill(dt);
                ds.Tables.Add(dt);
                ArrayList lstPackage = new ArrayList();
                ArrayList lstForm = new ArrayList();
                int i = ds.Tables[0].Rows.Count;
                for (int j = 0; j < i; j++)
                {
                    lstPackage.Add(ds.Tables[0].Rows[j][0].ToString());
                    lstForm.Add(ds.Tables[0].Rows[j][1].ToString());
                }
                return new object[] { 0, lstPackage, lstForm };
            }
            finally
            {
                ReleaseConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), nwindConn, true);
            }
        }

        public object InsertToMenu(object[] objParam)//这个方法效率太低
        {
            for (int i = 0; i < objParam.Length; i++)
            {
                string Menu = (String)objParam[i];
                if (Menu == ";;" || Menu == "" || Menu == null)
                    continue;
                string[] Insert = Menu.Split(';');
                ClientType ct = ClientType.ctMsSql;
                IDbConnection nwindConn = AllocateConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), ref ct, true);
                try
                {
                    string strSql = "insert into MENUTABLECONTROL (MENUID, CONTROLNAME , DESCRIPTION, TYPE) values ('" + Insert[0] + "', '" + Insert[1] + "', '" + Insert[2] + "', '" + Insert[3] + "')";

                    //为了区分不同的数据库 by Rei
                    InfoCommand cmd = new InfoCommand(ClientInfo);

                    cmd.CommandText = strSql;
                    cmd.Connection = nwindConn;
                    cmd.ExecuteNonQuery();
                }
                finally
                {
                    ReleaseConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), nwindConn, true);
                }
            }
            return new object[] { 0 };
        }

        public object DeleteGroupMenuControls(object[] objParam)
        {
            String strMenuID = objParam[0].ToString();
            String strGroupID = objParam[1].ToString();

            ClientType ct = ClientType.ctMsSql;
            IDbConnection nwindConn = AllocateConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), ref ct, true);
            try
            {
                string strSql = "DELETE FROM GROUPMENUCONTROL WHERE MENUID='" + strMenuID + "' AND GROUPID='" + strGroupID + "'";

                //为了区分不同的数据库 by Rei
                InfoCommand cmd = new InfoCommand(ClientInfo);

                cmd.CommandText = strSql;
                cmd.Connection = nwindConn;
                cmd.ExecuteNonQuery();
                return new object[] { 0 };
            }
            finally
            {
                ReleaseConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), nwindConn, true);
            }
        }

        public object DeleteUserMenuControls(object[] objParam)
        {
            String strMenuID = objParam[0].ToString();
            String strUserID = objParam[1].ToString();

            ClientType ct = ClientType.ctMsSql;
            IDbConnection nwindConn = AllocateConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), ref ct, true);
            try
            {
                string strSql = "DELETE FROM USERMENUCONTROL WHERE MENUID='" + strMenuID + "' AND USERID='" + strUserID + "'";

                //为了区分不同的数据库 by Rei
                InfoCommand cmd = new InfoCommand(ClientInfo);

                cmd.CommandText = strSql;
                cmd.Connection = nwindConn;
                cmd.ExecuteNonQuery();
                return new object[] { 0 };
            }
            finally
            {
                ReleaseConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), nwindConn, true);
            }
        }

        public object InsertToGroup(object[] objParam)
        {
            string Group = (String)objParam[0];
            string[] insert = Group.Split(';');
            if (insert[0] == null || insert[0] == "")
                return new object[] { 0 };
            else
            {
                ClientType ct = ClientType.ctMsSql;
                IDbConnection nwindConn = AllocateConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), ref ct, true);
                try
                {
                    string strSql = "insert into GROUPMENUCONTROL (GROUPID, MENUID, CONTROLNAME, TYPE, ENABLED, VISIBLE, ALLOWADD, ALLOWUPDATE, ALLOWDELETE, ALLOWPRINT)" +
                                    " values ( '" + insert[0] + "',  '" + insert[1] + "',  '" + insert[2] + "', '" + insert[3] +
                                    "', '" + insert[4] + "', '" + insert[5] + "', '" + insert[6] + "', '" + insert[7] + "', '" + insert[8] + "', '" + insert[9] + "')";

                    //为了区分不同的数据库 by Rei
                    InfoCommand cmd = new InfoCommand(ClientInfo);

                    cmd.CommandText = strSql;
                    cmd.Connection = nwindConn;
                    cmd.ExecuteNonQuery();
                    return new object[] { 0 };
                }
                finally
                {
                    ReleaseConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), nwindConn, true);
                }
            }
        }

        public object InsertToUser(object[] objParam)
        {
            string User = (String)objParam[0];
            string[] insert = User.Split(';');
            if (insert[0] == null || insert[0] == "")
                return new object[] { 0 };
            else
            {
                ClientType ct = ClientType.ctMsSql;
                IDbConnection nwindConn = AllocateConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), ref ct, true);
                try
                {
                    string strSql = "insert into USERMENUCONTROL (USERID, MENUID, CONTROLNAME, TYPE, ENABLED, VISIBLE, ALLOWADD, ALLOWUPDATE, ALLOWDELETE, ALLOWPRINT)" +
                                    " values ( '" + insert[0] + "',  '" + insert[1] + "',  '" + insert[2] + "', '" + insert[3] +
                                    "', '" + insert[4] + "', '" + insert[5] + "', '" + insert[6] + "', '" + insert[7] + "', '" + insert[8] + "', '" + insert[9] + "')";

                    //为了区分不同的数据库 by Rei
                    InfoCommand cmd = new InfoCommand(ClientInfo);

                    cmd.CommandText = strSql;
                    cmd.Connection = nwindConn;
                    cmd.ExecuteNonQuery();
                    return new object[] { 0 };
                }
                finally
                {
                    ReleaseConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), nwindConn, true);
                }
            }
        }

        public object UpdateGroup(object[] objParam)
        {
            string Group = (String)objParam[0];
            string[] insert = Group.Split(';');
            if (insert[0] == null || insert[0] == "")
                return new object[] { 0 };
            else
            {
                ClientType ct = ClientType.ctMsSql;
                IDbConnection nwindConn = AllocateConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), ref ct, true);
                try
                {
                    string strSql = "update GROUPMENUCONTROL set ENABLED = '" + insert[4] + "', VISIBLE = '" + insert[5] + "', ALLOWADD =  '" + insert[6]
                                    + "', ALLOWUPDATE = '" + insert[7] + "', ALLOWDELETE = '" + insert[8] + "', ALLOWPRINT = '" + insert[9] + "' where GROUPID = '"
                                    + insert[0] + "' and MENUID = '" + insert[1] + "' and CONTROLNAME = '" + insert[2] + "'";

                    //为了区分不同的数据库 by Rei
                    InfoCommand cmd = new InfoCommand(ClientInfo);

                    cmd.CommandText = strSql;
                    cmd.Connection = nwindConn;
                    cmd.ExecuteNonQuery();
                    return new object[] { 0 };
                }
                finally
                {
                    ReleaseConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), nwindConn, true);
                }
            }
        }

        public object UpdateMenu(object[] objParam)
        {
            for (int i = 0; i < objParam.Length; i++)
            {
                string Group = (String)objParam[i];
                if (Group == null)
                    continue;
                string[] insert = Group.Split(';');
                if (insert[0] == null || insert[0] == "")
                    return new object[] { 0 };
                else
                {
                    ClientType ct = ClientType.ctMsSql;
                    IDbConnection nwindConn = AllocateConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), ref ct, true);
                    try
                    {
                        string strSql = "update MENUTABLECONTROL set DESCRIPTION = '" + insert[2] + "' where MENUID = '"
                                        + insert[0] + "' and CONTROLNAME = '" + insert[1] + "'";

                        //为了区分不同的数据库 by Rei
                        InfoCommand cmd = new InfoCommand(ClientInfo);

                        cmd.CommandText = strSql;
                        cmd.Connection = nwindConn;
                        cmd.ExecuteNonQuery();
                    }
                    finally
                    {
                        ReleaseConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), nwindConn, true);
                    }
                }
            }
            return new object[] { 0 };
        }

        public object UpdateMenuTable(object[] objParam)
        {
            string[] itemType = ((String)objParam[0]).Split(';');
            ClientType ct = ClientType.ctMsSql;
            IDbConnection nwindConn = AllocateConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), ref ct, true);
            try
            {
                string strSql = "update MENUTABLE set ITEMTYPE = '" + itemType[1] + "' where ITEMTYPE = '" + itemType[0] + "'";

                //为了区分不同的数据库 by Rei
                InfoCommand cmd = new InfoCommand(ClientInfo);

                cmd.CommandText = strSql;
                cmd.Connection = nwindConn;
                cmd.ExecuteNonQuery();
                return new object[] { 0 };
            }
            finally
            {
                ReleaseConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), nwindConn, true);
            }
        }

        public object UpdateUser(object[] objParam)
        {
            string User = (String)objParam[0];
            string[] insert = User.Split(';');
            if (insert[0] == null || insert[0] == "")
                return new object[] { 0 };
            else
            {
                ClientType ct = ClientType.ctMsSql;
                IDbConnection nwindConn = AllocateConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), ref ct, true);
                try
                {
                    string strSql = "update USERMENUCONTROL set ENABLED = '" + insert[4] + "', VISIBLE = '" + insert[5] + "', ALLOWADD =  '" + insert[6]
                                    + "', ALLOWUPDATE = '" + insert[7] + "', ALLOWDELETE = '" + insert[8] + "', ALLOWPRINT = '" + insert[9] + "' where USERID = '"
                                    + insert[0] + "' and MENUID = '" + insert[1] + "' and CONTROLNAME = '" + insert[2] + "'";

                    //为了区分不同的数据库 by Rei
                    InfoCommand cmd = new InfoCommand(ClientInfo);

                    cmd.CommandText = strSql;
                    cmd.Connection = nwindConn;
                    cmd.ExecuteNonQuery();
                    return new object[] { 0 };
                }
                finally
                {
                    ReleaseConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), nwindConn, true);
                }
            }
        }

        public object UpdateRoleAgent(object[] objParam)
        {
            String roleID = (String)objParam[0];
            String[] roles = objParam[1].ToString().Split(new String[] { "!" }, StringSplitOptions.RemoveEmptyEntries);

            ClientType ct = ClientType.ctMsSql;
            IDbConnection nwindConn = AllocateConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), ref ct, true);
            InfoCommand cmd = new InfoCommand(ClientInfo);
            try
            {
                String strSql = "DELETE FROM SYS_ROLES_AGENT WHERE ROLE_ID='" + roleID + "'";
                cmd.Transaction = nwindConn.BeginTransaction();
                cmd.CommandText = strSql;
                cmd.Connection = nwindConn;
                cmd.ExecuteNonQuery();

                for (int i = 0; i < roles.Length; i++)
                {
                    if (roles[i].EndsWith(","))
                        roles[i] = roles[i].Remove(roles[i].LastIndexOf(","));
                    strSql = "INSERT INTO SYS_ROLES_AGENT (ROLE_ID, AGENT, FLOW_DESC, START_DATE, START_TIME, END_DATE, END_TIME, PAR_AGENT, REMARK) " +
                        "VALUES ('" + roleID + "', " + roles[i] + ")";

                    cmd.CommandText = strSql;
                    cmd.ExecuteNonQuery();
                }
                cmd.Transaction.Commit();
                return new object[] { 0 };
            }
            catch (Exception ex)
            {
                cmd.Transaction.Rollback();
                return new object[] { 0, ex.Message };
            }
            finally
            {
                ReleaseConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), nwindConn, true);
            }
        }

        public object GetDDColumns(object[] objParam)//这个方法
        {
            string TabName = (String)objParam[0];
            ClientType ct = ClientType.ctMsSql;
            IDbConnection nwindConn = AllocateConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), ref ct, false);
            try
            {
                string strSql = "";
                if (nwindConn is SqlConnection)
                    strSql = "select [name], length from syscolumns where [id] in (select [id] from sysobjects where [name] = '" + TabName + "')";
                else if (ct == ClientType.ctODBC)
                {
                    String sDBAlias = GetClientInfo(ClientInfoType.LoginDB).ToString();
                    var subType = GetDataBaseSubType(new object[] { sDBAlias, false });
                    if (subType[1].ToString() == "2")
                        strSql = "select COLUMN_NAME AS COLNAME, LENGTH AS COLLENGTH from qsys2.SYSCOLUMNS WHERE TABLE_NAME = '" + TabName + "'";
                    else if (subType[1].ToString() == "0")
                        strSql = "select COLNAME, COLLENGTH from SYSCOLUMNS where TABID in (select TABID from SYSTABLES where TABNAME = '" + TabName + "')";
                }
                else if (nwindConn is OracleConnection)
                    strSql = "select COLUMN_NAME, DATA_LENGTH from ALL_TAB_COLUMNS where TABLE_NAME = '" + TabName + "'";
                else if (nwindConn is OleDbConnection)
                    strSql = "select name, length from syscolumns where id in (select id from sysobjects where name = '" + TabName + "')";
                else if (nwindConn.GetType().Name == "MySqlConnection")
                    strSql = "desc " + TabName;
                else if (nwindConn.GetType().Name == "IfxConnection")
                    strSql = "select COLNAME, COLLENGTH from SYSCOLUMNS where TABID in (select TABID from SYSTABLES where TABNAME = '" + TabName + "')";

                //IDbTransaction it = nwindConn.BeginTransaction();
                //为了区分不同的数据库 by Rei
                InfoCommand cmd = new InfoCommand(ClientInfo);
                cmd.CommandText = strSql;
                cmd.Connection = nwindConn;
                //cmd.Transaction = it;
                IDataAdapter adpater = DBUtils.CreateDbDataAdapter(cmd);

                DataSet ds = new DataSet();
                DataTable dt1 = new DataTable();
                (adpater as DbDataAdapter).Fill(dt1);
                ds.Tables.Add(dt1);                //it.Commit();
                DataTable dt = ds.Tables.Count > 0 ? ds.Tables[0] : null;
                return new object[] { 0, dt };
            }
            finally
            {
                ReleaseConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), nwindConn, false);
            }
        }

        public object GetRoles(object[] parames)
        {
            ClientType ct = ClientType.ctMsSql;
            IDbConnection nwindConn = AllocateConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), ref ct, true);
            try
            {
                List<string> list = new List<string>();
                String sql = "";
                String connectMark = "+";
                if (nwindConn is SqlConnection)
                    connectMark = "+";
                else if (nwindConn is OdbcConnection)
                    connectMark = "||";
                else if (nwindConn is OracleConnection)
                    connectMark = "||";
                else if (nwindConn is OleDbConnection)
                    connectMark = "+";
                else if (nwindConn.GetType().Name == "MySqlConnection")
                    connectMark = "||";
                else if (nwindConn.GetType().Name == "IfxConnection")
                    connectMark = "||";

                if (nwindConn.GetType().Name == "MySqlConnection")
                    sql = "select CONCAT(GROUPID,' ; ',GROUPNAME) from GROUPS where ISROLE='Y'";
                else
                    sql = "select GROUPID " + connectMark + " ' ; ' " + connectMark + " GROUPNAME from GROUPS where ISROLE='Y'";

                InfoCommand command = new InfoCommand(ClientInfo);
                command.CommandText = sql;
                command.Connection = nwindConn;

                IDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection);

                while (reader.Read())
                {
                    list.Add(reader[0].ToString());
                }
                command.Cancel();
                reader.Close();
                return new object[] { 0, list.ToArray() };
            }
            finally
            {
                ReleaseConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), nwindConn, false);
            }
        }

        public object GetRefRoles(object[] parames)//???????
        {
            string tableName = parames[0].ToString();
            ClientType ct = ClientType.ctMsSql;
            IDbConnection nwindConn = AllocateConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), ref ct, false);
            try
            {
                List<string> list = new List<string>();

                string sql = "select * from {0} where 1 > 1";
                sql = string.Format(sql, tableName);

                InfoCommand command = new InfoCommand(ClientInfo);
                command.CommandText = sql;
                command.Connection = nwindConn;

                IDataAdapter adpater = DBUtils.CreateDbDataAdapter(command);

                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                (adpater as DbDataAdapter).Fill(dt);
                ds.Tables.Add(dt);
                DataColumnCollection columns = ds.Tables[0].Columns;
                foreach (DataColumn c in columns)
                {
                    list.Add(c.ColumnName);
                }

                return new object[] { 0, list.ToArray() };
            }
            finally
            {
                ReleaseConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), nwindConn, false);
            }
        }

        public object GetOrgLevel(object[] parames)
        {
            ClientType ct = ClientType.ctMsSql;
            IDbConnection nwindConn = AllocateConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), ref ct, true);
            try
            {
                List<string> list = new List<string>();
                String sql = "";
                String connectMark = "+";
                if (nwindConn is SqlConnection)
                    connectMark = "+";
                else if (nwindConn is OdbcConnection)
                    connectMark = "||";
                else if (nwindConn is OracleConnection)
                    connectMark = "||";
                else if (nwindConn is OleDbConnection)
                    connectMark = "+";
                else if (nwindConn.GetType().Name == "MySqlConnection")
                    connectMark = "||";
                else if (nwindConn.GetType().Name == "IfxConnection")
                    connectMark = "||";

                if (nwindConn.GetType().Name == "MySqlConnection")
                    sql = "select CONCAT(LEVEL_NO,' ; ',LEVEL_DESC) from SYS_ORGLEVEL where 1=1";
                else
                    sql = "select LEVEL_NO " + connectMark + " ' ; ' " + connectMark + " LEVEL_DESC from SYS_ORGLEVEL where 1=1";

                InfoCommand command = new InfoCommand(ClientInfo);
                command.CommandText = sql;
                command.Connection = nwindConn;

                IDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection);

                while (reader.Read())
                {
                    list.Add(reader[0].ToString());

                }
                command.Cancel();
                reader.Close();
                return new object[] { 0, list.ToArray() };
            }
            finally
            {
                ReleaseConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), nwindConn, true);
            }
        }

        public object GetOrgKinds(object[] parames)
        {
            ClientType ct = ClientType.ctMsSql;
            IDbConnection nwindConn = AllocateConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), ref ct, true);
            try
            {
                List<string> list = new List<string>();
                string sql = "select ORG_KIND from SYS_ORGKIND";

                InfoCommand command = new InfoCommand(ClientInfo);
                command.CommandText = sql;
                command.Connection = nwindConn;

                IDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection);

                while (reader.Read())
                {
                    list.Add(reader[0].ToString());
                }
                reader.Close();
                return new object[] { 0, list.ToArray() };
            }
            finally
            {
                ReleaseConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), nwindConn, true);
            }
        }

        public object GetTableNames(object[] parames)
        {
            ClientType ct = ClientType.ctMsSql;
            IDbConnection nwindConn = AllocateConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), ref ct, false);
            try
            {
                string strSql = "";
                if (nwindConn is SqlConnection)
                {
                    strSql = "select @@version as version";
                    InfoCommand cmd = new InfoCommand(ClientInfo);
                    cmd.CommandText = strSql;
                    cmd.Connection = nwindConn;
                    Object o = cmd.ExecuteScalar();
                    if (o.ToString().ToLower().IndexOf("microsoft sql server 2005") >= 0)
                    {
                        strSql = @"select (
                                case when b.name != 'dbo' then 
	                            case when (Charindex(' ',Rtrim(Ltrim(b.name)),0) > 0) then 
                                    '[' + b.[name] + ']'
	                            else
		                            b.[name]
	                            end
	                            + '.' +
	                            case when (Charindex(' ',Rtrim(Ltrim(a.name)),0) != 0) then
		                            '[' + a.[name] + ']'
	                            else
		                            a.[name]
	                            end
                            else 
	                            case when (Charindex(' ',Rtrim(Ltrim(a.name)),0) != 0) then
		                            '[' + a.[name] + ']'
	                            else
		                            a.[name]
	                            end
                            end
                        )as name from sysobjects a,sys.schemas b where a.uid=b.schema_id and a.xtype in ('u','U','v','V') order by a.[name]";
                    }
                    else
                    {
                        strSql = @"select(
                            case when (Charindex(' ',Rtrim(Ltrim(name)),0) != 0) then
		                        '[' + [name] + ']'
	                        else
		                        [name]
                            end
                            ) as name from sysobjects where xtype in ('u','U','v','V')  order by [name]";
                    }
                }
                else if (ct == ClientType.ctODBC)
                {
                    String sDBAlias = GetClientInfo(ClientInfoType.LoginDB).ToString();
                    var subType = GetDataBaseSubType(new object[] { sDBAlias, false });
                    if (subType[1].ToString() == "2")
                        strSql = "select TABLE_NAME AS tabname, TABLE_OWNER AS owner from qsys2.SYSTABLES where TABLE_TYPE='T' OR TABLE_TYPE = 'V'";
                    else if (subType[1].ToString() == "0")
                        strSql = "select * from systables where (tabtype = 'T' or tabtype = 'V') and tabid >= 100 order by tabname";
                }
                else if (nwindConn is OracleConnection)
                {
                    return new object[] { 0, GetTableNames(nwindConn.ConnectionString, nwindConn as DbConnection).ToArray() };
                    //strSql = "SELECT * FROM USER_OBJECTS WHERE OBJECT_TYPE = 'TABLE' OR OBJECT_TYPE = 'VIEW'order by OBJECT_NAME";
                }
                else if (nwindConn is OleDbConnection)
                    strSql = "sp_help";
                else if (nwindConn.GetType().Name == "MySqlConnection")
                    strSql = "show tables;";
                else if (nwindConn.GetType().Name == "IfxConnection")
                    strSql = "select * from SYSTABLES where (TABTYPE = 'T' or TABTYPE = 'V') and TABID >= 100 order by TABNAME";

                InfoCommand cmd2 = new InfoCommand(ClientInfo);
                if (nwindConn is OleDbConnection)
                    cmd2.CommandType = CommandType.StoredProcedure;
                cmd2.CommandText = strSql;
                cmd2.Connection = nwindConn;
                IDataReader reader = cmd2.ExecuteReader();
                List<String> tablesList = new List<string>();
                while (reader.Read())
                {
                    if (nwindConn is SqlConnection)
                        tablesList.Add(reader["name"].ToString());
                    else if (nwindConn is OdbcConnection)
                        tablesList.Add(reader["tabname"].ToString());
                    else if (nwindConn is OracleConnection)
                        tablesList.Add(reader["OBJECT_NAME"].ToString());
                    else if (nwindConn is OleDbConnection)
                        tablesList.Add(reader["NAME"].ToString());
                    else if (nwindConn.GetType().Name == "MySqlConnection")
                        tablesList.Add(reader[0].ToString());
                    else if (nwindConn.GetType().Name == "IfxConnection")
                        tablesList.Add(reader["tabname"].ToString());
                }
                reader.Close();
                return new object[] { 0, tablesList.ToArray() };
            }
            finally
            {
                ReleaseConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), nwindConn, false);
            }
        }

        private List<String> GetTableNames(String connectionString, DbConnection conn)
        {
            List<String> LV = new List<string>();

            int I;
            String[] Params = null;
            String ViewFieldName = "TABLE_NAME";
            String UserID = GetFieldParam(connectionString.ToLower(), "user id");
            Params = new String[] { UserID.ToUpper() };
            ViewFieldName = "VIEW_NAME";

            DataTable D = conn.GetSchema("Tables", Params);
            DataRow[] dr = D.Select("", "TABLE_NAME DESC");

            bool flag = false;
            foreach (DataColumn DC in D.Columns)
            {
                if (DC.Caption.ToLower() == "owner")
                    flag = true;
            }
            foreach (DataRow DR in dr)
            {
                String S = "";
                if (flag && !String.IsNullOrEmpty(DR["OWNER"].ToString()))
                    S = DR["OWNER"].ToString() + '.';
                LV.Add(S + DR["TABLE_NAME"].ToString());
            }

            DataTable D1 = conn.GetSchema("Views", Params);
            DataRow[] dr1 = D1.Select("", ViewFieldName + " DESC");

            flag = false;
            foreach (DataColumn DC in D1.Columns)
            {
                if (DC.Caption.ToLower() == "owner")
                    flag = true;
            }
            foreach (DataRow DR in dr1)
            {
                String S = "";
                if (flag && !String.IsNullOrEmpty(DR["OWNER"].ToString()))
                    S = DR["OWNER"].ToString() + '.';
                if (LV.IndexOf(DR[ViewFieldName].ToString()) < 0)
                    LV.Add(S + DR[ViewFieldName].ToString());
            }

            LV.Sort();

            return LV;
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

        private static String GetToken(ref String AString, char[] Fmt)
        {
            String Result = "";
            while (AString.Length != 0 && AString[0] == ' ')
            {
                AString = AString.Remove(0, 1);
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

        public object GetDDColumnsSchema(object[] objParam)
        {
            string TabName = (String)objParam[0];

            ClientType ct = ClientType.ctMsSql;
            IDbConnection nwindConn = AllocateConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), ref ct, false);
            try
            {
                string strSql = "";
                if (nwindConn is SqlConnection)
                {
                    ct = ClientType.ctMsSql;
                    strSql = "select * from [" + TabName + "]";
                }
                else if (nwindConn is OdbcConnection)
                {
                    ct = ClientType.ctODBC;
                    strSql = "select * from " + TabName;
                }
                else if (nwindConn is OracleConnection)
                {
                    ct = ClientType.ctOracle;
                    strSql = "select * from " + TabName;
                }
                else if (nwindConn is OleDbConnection)
                {
                    ct = ClientType.ctOleDB;
                    strSql = "select * from " + TabName;
                }
                else if (nwindConn.GetType().Name == "MySqlConnection")
                {
                    ct = ClientType.ctMySql;
                    strSql = "select * from " + TabName;
                }
                else if (nwindConn.GetType().Name == "IfxConnection")
                {
                    ct = ClientType.ctInformix;
                    strSql = "select * from " + TabName;
                }

                strSql += " WHERE 1=0";

                IDbCommand cmd = nwindConn.CreateCommand();
                cmd.CommandText = strSql;
                IDataReader idr = cmd.ExecuteReader(CommandBehavior.SchemaOnly | CommandBehavior.KeyInfo);
                DataTable dt = idr.GetSchemaTable();

                return new object[] { 0, dt };
            }
            finally
            {
                ReleaseConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), nwindConn, false);
            }
        }

        public object DeleteDDColumns(object[] objParam)
        {
            string tabName = (String)objParam[0];
            ClientType ct = ClientType.ctMsSql;
            IDbConnection nwindConn = AllocateConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), ref ct, false);
            try
            {
                string strSql = "delete from COLDEF where TABLE_NAME = '" + tabName + "'";
                //IDbTransaction it = nwindConn.BeginTransaction();
                //为了区分不同的数据库 by Rei
                InfoCommand cmd = new InfoCommand(ClientInfo);
                cmd.CommandText = strSql;
                cmd.Connection = nwindConn;
                //cmd.Transaction = it;
                cmd.ExecuteNonQuery();
                //it.Commit();
                return new object[] { 0, null };
            }
            finally
            {
                ReleaseConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), nwindConn, false);
            }
        }

        public object DelMenu(object[] objParam)
        {
            string str = (String)objParam[0];
            string[] id = str.Split(';');
            ClientType ct = ClientType.ctMsSql;
            IDbConnection nwindConn = AllocateConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), ref ct, true);
            try
            {
                string strSql = "delete from MENUTABLECONTROL where MENUID = '" + id[0] + "' and CONTROLNAME = '" + id[1] + "'";

                //为了区分不同的数据库 by Rei
                InfoCommand cmd = new InfoCommand(ClientInfo);

                cmd.CommandText = strSql;
                cmd.Connection = nwindConn;
                cmd.ExecuteNonQuery();
                return new object[] { 0, null };
            }
            finally
            {
                ReleaseConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), nwindConn, true);
            }
        }

        public object DelGroup(object[] objParam)
        {
            string str = (String)objParam[0];
            string[] id = str.Split(';');
            ClientType ct = ClientType.ctMsSql;
            IDbConnection nwindConn = AllocateConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), ref ct, true);
            try
            {
                string strSql = "delete from GROUPMENUCONTROL where GROUPID = '" + id[0] + "' and MENUID = '" + id[1] + "' and CONTROLNAME = '" + id[2] + "'";

                //为了区分不同的数据库 by Rei
                InfoCommand cmd = new InfoCommand(ClientInfo);

                cmd.CommandText = strSql;
                cmd.Connection = nwindConn;
                cmd.ExecuteNonQuery();
                return new object[] { 0, null };
            }
            finally
            {
                ReleaseConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), nwindConn, true);
            }
        }

        public object DelUser(object[] objParam)
        {
            string str = (String)objParam[0];
            string[] id = str.Split(';');
            ClientType ct = ClientType.ctMsSql;
            IDbConnection nwindConn = AllocateConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), ref ct, true);
            try
            {
                string strSql = "delete from USERMENUCONTROL where USERID = '" + id[0] + "' and MENUID = '" + id[1] + "' and CONTROLNAME = '" + id[2] + "'";

                //为了区分不同的数据库 by Rei
                InfoCommand cmd = new InfoCommand(ClientInfo);

                cmd.CommandText = strSql;
                cmd.Connection = nwindConn;
                cmd.ExecuteNonQuery();
                return new object[] { 0, null };
            }
            finally
            {
                ReleaseConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), nwindConn, true);
            }
        }

        public object DataDRefresh(object[] objParam)
        {
            string tableName = (String)objParam[0];
            ClientType ct = ClientType.ctMsSql;
            IDbConnection nwindConn = AllocateConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), ref ct, false);
            try
            {
                string strSql = "";
                if (nwindConn is SqlConnection)
                    strSql = "select distinct FIELD_NAME, CAPTION from COLDEF where FIELD_NAME in (select [name] from syscolumns where [id] in (select [id] from sysobjects where [name] = '" + tableName + "'))";
                else if (ct == ClientType.ctODBC)
                {
                    String sDBAlias = GetClientInfo(ClientInfoType.LoginDB).ToString();
                    var subType = GetDataBaseSubType(new object[] { sDBAlias, false });
                    if (subType[1].ToString() == "2")
                        strSql = "select distinct FIELD_NAME, CAPTION from COLDEF where FIELD_NAME in (select COLUMN_NAME from qsys2.SYSCOLUMNS where TABLE_NAME = '" + tableName + "')";
                    else if (subType[1].ToString() == "0")
                        strSql = "select distinct FIELD_NAME, CAPTION from COLDEF where FIELD_NAME in (select COLNAME from SYSCOLUMNS where TABID in (select TABID from SYSTABLES where TABNAME = '" + tableName + "'))";
                }
                else if (nwindConn is OracleConnection)
                    strSql = "select distinct FIELD_NAME, CAPTION from COLDEF where FIELD_NAME in (select COLUMN_NAME from ALL_TAB_COLUMNS where TABLE_NAME ='" + tableName + "')";
                else if (nwindConn is OleDbConnection)
                    strSql = "select distinct FIELD_NAME, CAPTION from COLDEF where FIELD_NAME in (select name from syscolumns where id in (select id from sysobjects where name = '" + tableName + "'))";
                else if (nwindConn.GetType().Name == "MySqlConnection")
                    strSql = "select distinct FIELD_NAME, CAPTION from COLDEF where TABLE_NAME='" + tableName + "'";
                else if (nwindConn.GetType().Name == "IfxConnection")
                    strSql = "select distinct FIELD_NAME, CAPTION from COLDEF where FIELD_NAME in (select COLNAME from SYSCOLUMNS where TABID in (select TABID from SYSTABLES where TABNAME = '" + tableName + "'))";

                //"select distinct Field_name, caption from coldef where exists (select name from syscolumns where id where [id] in (select [id] from sysobjects where [name] = '" + tableName + "') and [name]=' and name ='" + fieldName + "')";
                //"select DISTINCT CAPTION from COLDEF where FIELD_NAME = '" + fieldName + "' and CAPTION <> ''";
                //IDbTransaction it = nwindConn.BeginTransaction();

                //为了区分不同的数据库 by Rei
                InfoCommand cmd = new InfoCommand(ClientInfo);
                cmd.CommandText = strSql;
                cmd.Connection = nwindConn;
                //cmd.Transaction = it;
                //it.Commit();
                IDataAdapter adpater = DBUtils.CreateDbDataAdapter(cmd);

                DataSet ds = new DataSet();
                DataTable dt1 = new DataTable();
                (adpater as DbDataAdapter).Fill(dt1);
                ds.Tables.Add(dt1);
                DataTable dt = ds.Tables.Count > 0 ? ds.Tables[0] : null;
                return new object[] { 0, dt };
            }
            finally
            {
                ReleaseConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), nwindConn, false);
            }
        }

        public object GetFieldCaption(object[] objParam)
        {
            string field_name = (String)objParam[0];
            ClientType ct = ClientType.ctMsSql;
            IDbConnection nwindConn = AllocateConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), ref ct, false);
            try
            {
                string strSql = "select CAPTION from COLDEF where FIELD_NAME='" + field_name + "'";
                //IDbTransaction it = nwindConn.BeginTransaction();
                InfoCommand cmd = new InfoCommand(ClientInfo);
                //cmd.Transaction = it;
                cmd.CommandText = strSql;
                cmd.Connection = nwindConn;
                //it.Commit();
                IDataAdapter adpater = DBUtils.CreateDbDataAdapter(cmd);

                DataSet ds = new DataSet();
                DataTable dt1 = new DataTable();
                (adpater as DbDataAdapter).Fill(dt1);
                ds.Tables.Add(dt1);
                DataTable dt = ds.Tables.Count > 0 ? ds.Tables[0] : null;
                String caption = "";
                if (dt != null && dt.Rows.Count > 0)
                    caption = dt.Rows[0][0].ToString();
                return new object[] { 0, caption };
            }
            finally
            {
                ReleaseConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), nwindConn, false);
            }
        }

        public object[] GetDB(object[] objParam)
        {
            ArrayList dba = new ArrayList();
            ArrayList dbs = new ArrayList();
            XmlDocument DBXML = new XmlDocument();

            if (File.Exists(SystemFile.DBFile))
            {
                DBXML.Load(SystemFile.DBFile);
                XmlNode aNode = DBXML.DocumentElement.FirstChild;
                XmlNode sysNode = null;
                string sysDB = "";

                while ((null != aNode))
                {
                    if (string.Compare(aNode.Name, "SYSTEMDB", true) == 0)//IgnoreCase
                    {
                        sysNode = aNode;
                        sysDB = sysNode.InnerText.Trim();
                        break;
                    }
                    aNode = aNode.NextSibling;
                }

                aNode = DBXML.DocumentElement.FirstChild;
                while ((null != aNode) && string.Compare(aNode.Name, "DATABASE", true) != 0)//IgnoreCase
                {
                    aNode = aNode.NextSibling;
                }
                if (null != aNode) aNode = aNode.FirstChild;

                XmlNode yNode = aNode;

                while (yNode != null)
                {
                    dba.Add(yNode.LocalName);
                    dbs.Add(yNode.Attributes["Master"].InnerText.Trim());
                    yNode = yNode.NextSibling;
                }
                return new object[] { 0, dba, dbs, sysDB };
            }
            return new object[] { 1 };
        }

        public object[] GetProviderName(object[] param)
        {
            String SolutionName = param[0] as String;
            ArrayList ServerModule = new ArrayList();
            XmlDocument PkgXml = new XmlDocument();


            if (File.Exists(SystemFile.PackagesFile))
            {
                PkgXml.Load(SystemFile.PackagesFile);

                XmlNode aNode = PkgXml.DocumentElement.FirstChild;
                while (aNode != null)
                {
                    if (string.Compare(SolutionName, aNode.Name, true) == 0)//IgnoreCase
                    {
                        XmlNode bNode = aNode.FirstChild;
                        while (bNode != null)
                        {
                            ServerModule.Add(bNode.Attributes["Name"].Value.Remove(bNode.Attributes["Name"].Value.LastIndexOf('.')));
                            bNode = bNode.NextSibling;
                        }
                        break;
                    }
                    aNode = aNode.NextSibling;
                }
            }
            return new object[] { 0, ServerModule };
        }

        public object[] GetMethodName(object[] param)
        {
            String solution = Convert.ToString(param[0]);
            String packageName = Convert.ToString(param[1]);
            String package = "";
            if (solution.Length == 0)
            {
                package = string.Format("{0}\\{1}.dll", EEPRegistry.Server, packageName);
            }
            else
            {
                package = string.Format("{0}\\{1}\\{2}.dll", EEPRegistry.Server, solution, packageName);
            }

            Assembly a = Assembly.LoadFrom(package);
            String ModuleName = Path.GetFileNameWithoutExtension(packageName);
            if (!ModuleName.EndsWith("."))
                ModuleName += ".";
            Type myType = a.GetType(ModuleName + "Component");
            if (myType == null)
            {
                foreach (Type t in a.GetTypes())
                    if (t.FullName.Contains(ModuleName) && !t.FullName.Contains(".My."))
                        myType = a.GetType(t.FullName);
            }

            ArrayList methodList = new ArrayList();
            if (myType != null)
            {
                Object obj = Activator.CreateInstance(myType);

                ServiceManager serviceMan = (ServiceManager)((IDataModule)obj).GetIntfObject(typeof(IServiceManager));
                if (null != serviceMan)
                {
                    ServiceCollection serviceCol = serviceMan.ServiceCollection;

                    if (serviceCol.Count > 0)
                        foreach (Service service in serviceCol)
                            methodList.Add(service.ServiceName);
                }
            }

            return new object[] { 0, methodList };
        }

        public object[] GetDataModule(object[] param)
        {
            String SolutionName = param[0] as String;
            String ModuleName = param[1] as String;
            int iPos = Convert.ToInt32(param[2]);

            ArrayList DBCommand = new ArrayList();

            String s;
            if (SolutionName.Length == 0)
            {
                s = string.Format("{0}\\{1}", EEPRegistry.Server, ModuleName);
            }
            else
            {
                s = string.Format("{0}\\{1}\\{2}", EEPRegistry.Server, SolutionName, ModuleName);
            }

            String sDll = Path.GetFileNameWithoutExtension(s);

            FileStream fs = null;
            fs = new FileStream(s, FileMode.Open, FileAccess.Read, FileShare.Read);
            byte[] b = new byte[fs.Length];
            fs.Read(b, 0, (int)fs.Length);
            fs.Close();

            Assembly a = Assembly.Load(b);
            Type myType = a.GetType(sDll + ".Component");

            if (myType != null)
            {
                FieldInfo[] Fields = myType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
                for (int i = 0; i < Fields.Length; i++)
                    if (null != Fields[i].FieldType.GetInterface("IInfoCommand"))
                        DBCommand.Add(Fields[i].Name);
            }

            return new object[] { 0, DBCommand };
        }

        public object[] GetUserName(object[] objParam)
        {
            if (File.Exists(SystemFile.UsersFile))
            {
                XmlDocument DBXML = new XmlDocument();
                DBXML.Load(SystemFile.UsersFile);
                XmlNode aNode = DBXML.DocumentElement.FirstChild;

                while ((null != aNode) && !aNode.Attributes["UserId"].InnerText.Equals((string)objParam[0]))
                {
                    aNode = aNode.NextSibling;
                }

                if (aNode == null)
                {
                    return new object[] { 1 };
                }
                else
                {
                    string strName = aNode.Attributes["UserName"].InnerText;
                    return new object[] { 0, strName };
                }
            }
            else
            {
                return new object[] { 1 };
            }
        }

        public object[] GetServerTime(object[] objParam)
        {
            string strServerDate = DateTime.Now.ToShortDateString();
            string strServerTime = DateTime.Now.ToShortTimeString();
            return new object[] { 0, strServerDate, strServerTime };
        }

        public object GetADUsers(object[] objParam)
        {
            ArrayList list = new ArrayList();
            foreach (var domain in ServerConfig.Domains)
            {
                try
                {
                    var ad = new ADClass() { ADPath = "LDAP://" + domain.Path, ADUser = domain.User, ADPassword = domain.Password };
                    List<ADUser> lstUser = ad.GetADAllUser();
                    list.AddRange(lstUser.ToArray());
                }
                catch
                {

                }
            }
            return new object[] { 0, list };
        }

        public object UpdateADUsers(object[] objParam)
        {
            String[] strAdUsers = objParam[0].ToString().Split(new String[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            object[] myRet = (object[])GetADUsers(null);
            ArrayList alADUsers = myRet[1] as ArrayList;

            ClientType ct = ClientType.ctMsSql;
            IDbConnection nwindConn = AllocateConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), ref ct, false);
            try
            {
                foreach (String strADUser in strAdUsers)
                {
                    foreach (ADUser user in alADUsers)
                    {
                        if (strADUser == user.ID)
                        {
                            InfoCommand cmd = new InfoCommand(ClientInfo);
                            cmd.Connection = nwindConn;
                            if (ct == ClientType.ctMsSql || ct == ClientType.ctMySql)
                            {
                                cmd.CommandText = "DELETE FROM USERS WHERE USERID=@USERID";
                                IDbDataParameter idpUSERID = cmd.CreateParameter();
                                idpUSERID.ParameterName = "@USERID";
                                idpUSERID.Value = user.ID;
                                cmd.Parameters.Add(idpUSERID);
                            }
                            else if (ct == ClientType.ctOleDB || ct == ClientType.ctSybase || ct == ClientType.ctODBC || ct == ClientType.ctInformix)
                            {
                                cmd.CommandText = "DELETE FROM USERS WHERE USERID=?";
                                IDbDataParameter idpUSERID = cmd.CreateParameter();
                                idpUSERID.ParameterName = "?";
                                idpUSERID.Value = user.ID;
                                cmd.Parameters.Add(idpUSERID);
                            }
                            else if (ct == ClientType.ctOracle)
                            {
                                cmd.CommandText = "DELETE FROM USERS WHERE USERID=:USERID";
                                IDbDataParameter idpUSERID = cmd.CreateParameter();
                                idpUSERID.ParameterName = ":USERID";
                                idpUSERID.Value = user.ID;
                                cmd.Parameters.Add(idpUSERID);
                            }
                            cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();

                            if (ct == ClientType.ctMsSql || ct == ClientType.ctMySql)
                            {
                                cmd.CommandText = "INSERT INTO USERS (USERID, USERNAME, DESCRIPTION, AUTOLOGIN, EMAIL, MSAD, CREATEDATE) " +
                                    "VALUES (@USERID, @USERNAME, @DESCRIPTION, 'S', @EMAIL, 'Y', @CREATEDATE)";
                                IDbDataParameter idpUSERID = cmd.CreateParameter();
                                idpUSERID.ParameterName = "@USERID";
                                idpUSERID.Value = user.ID;

                                IDbDataParameter idpUSERNAME = cmd.CreateParameter();
                                idpUSERNAME.ParameterName = "@USERNAME";
                                idpUSERNAME.Value = user.Name;

                                IDbDataParameter idpDESCRIPTION = cmd.CreateParameter();
                                idpDESCRIPTION.ParameterName = "@DESCRIPTION";
                                idpDESCRIPTION.Value = user.Description;

                                IDbDataParameter idpEMAIL = cmd.CreateParameter();
                                idpEMAIL.ParameterName = "@EMAIL";
                                idpEMAIL.Value = user.Email;

                                IDbDataParameter idpCREATEDATE = cmd.CreateParameter();
                                idpCREATEDATE.ParameterName = "@CREATEDATE";
                                idpCREATEDATE.Value = DateTime.Today.ToString("yyyyMMdd");

                                cmd.Parameters.Add(idpUSERID);
                                cmd.Parameters.Add(idpUSERNAME);
                                cmd.Parameters.Add(idpDESCRIPTION);
                                cmd.Parameters.Add(idpEMAIL);
                                cmd.Parameters.Add(idpCREATEDATE);
                            }
                            else if (ct == ClientType.ctOleDB || ct == ClientType.ctSybase || ct == ClientType.ctODBC || ct == ClientType.ctInformix)
                            {
                                cmd.CommandText = "INSERT INTO USERS (USERID, USERNAME, DESCRIPTION, AUTOLOGIN, EMAIL, MSAD, CREATEDATE) " +
                                    "VALUES (?, ?, ?, 'S', ?, 'Y', ?)";
                                IDbDataParameter idpUSERID = cmd.CreateParameter();
                                idpUSERID.ParameterName = "?";
                                idpUSERID.Value = user.ID;

                                IDbDataParameter idpUSERNAME = cmd.CreateParameter();
                                idpUSERNAME.ParameterName = "?";
                                idpUSERNAME.Value = user.Name;

                                IDbDataParameter idpDESCRIPTION = cmd.CreateParameter();
                                idpDESCRIPTION.ParameterName = "?";
                                idpDESCRIPTION.Value = user.Description;

                                IDbDataParameter idpEMAIL = cmd.CreateParameter();
                                idpEMAIL.ParameterName = "?";
                                idpEMAIL.Value = user.Email;

                                IDbDataParameter idpCREATEDATE = cmd.CreateParameter();
                                idpCREATEDATE.ParameterName = "?";
                                idpCREATEDATE.Value = DateTime.Today.ToString("yyyyMMdd");

                                cmd.Parameters.Add(idpUSERID);
                                cmd.Parameters.Add(idpUSERNAME);
                                cmd.Parameters.Add(idpDESCRIPTION);
                                cmd.Parameters.Add(idpEMAIL);
                                cmd.Parameters.Add(idpCREATEDATE);
                            }
                            else if (ct == ClientType.ctOracle)
                            {
                                cmd.CommandText = "INSERT INTO USERS (USERID, USERNAME, DESCRIPTION, AUTOLOGIN, EMAIL, MSAD, CREATEDATE) " +
                                    "VALUES (:USERID, :USERNAME, :DESCRIPTION, 'S', :EMAIL, 'Y', :CREATEDATE)";
                                IDbDataParameter idpUSERID = cmd.CreateParameter();
                                idpUSERID.ParameterName = ":USERID";
                                idpUSERID.Value = user.ID;

                                IDbDataParameter idpUSERNAME = cmd.CreateParameter();
                                idpUSERNAME.ParameterName = ":USERNAME";
                                idpUSERNAME.Value = user.Name;

                                IDbDataParameter idpDESCRIPTION = cmd.CreateParameter();
                                idpDESCRIPTION.ParameterName = ":DESCRIPTION";
                                idpDESCRIPTION.Value = user.Description;

                                IDbDataParameter idpEMAIL = cmd.CreateParameter();
                                idpEMAIL.ParameterName = ":EMAIL";
                                idpEMAIL.Value = user.Email;

                                IDbDataParameter idpCREATEDATE = cmd.CreateParameter();
                                idpCREATEDATE.ParameterName = ":CREATEDATE";
                                idpCREATEDATE.Value = DateTime.Today.ToString("yyyyMMdd");

                                cmd.Parameters.Add(idpUSERID);
                                cmd.Parameters.Add(idpUSERNAME);
                                cmd.Parameters.Add(idpDESCRIPTION);
                                cmd.Parameters.Add(idpEMAIL);
                                cmd.Parameters.Add(idpCREATEDATE);
                            }
                            cmd.ExecuteNonQuery();
                            break;
                        }
                    }
                }
                return new object[] { 0 };
            }
            finally
            {
                ReleaseConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), nwindConn, false);
            }
        }

        public object UpdateADGroups(object[] objParam)
        {
            String[] strAdGroups = objParam[0].ToString().Split(new String[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            object[] myRet = (object[])GetADUserForGroup(null);
            ArrayList alADGroups = myRet[1] as ArrayList;

            ClientType ct = ClientType.ctMsSql;
            IDbConnection nwindConn = AllocateConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), ref ct, false);
            try
            {
                foreach (String strADGroup in strAdGroups)
                {
                    foreach (ADGroup group in alADGroups)
                    {
                        if (strADGroup == group.ID)
                        {
                            InfoCommand cmd = new InfoCommand(ClientInfo);
                            cmd.Connection = nwindConn;
                            if (ct == ClientType.ctMsSql || ct == ClientType.ctMySql)
                            {
                                cmd.CommandText = "DELETE FROM GROUPS WHERE GROUPNAME=@GROUPNAME";
                                IDbDataParameter idpGROUPNAME = cmd.CreateParameter();
                                idpGROUPNAME.ParameterName = "@GROUPNAME";
                                idpGROUPNAME.Value = group.ID;
                                cmd.Parameters.Add(idpGROUPNAME);
                            }
                            else if (ct == ClientType.ctOleDB || ct == ClientType.ctSybase || ct == ClientType.ctODBC || ct == ClientType.ctInformix)
                            {
                                cmd.CommandText = "DELETE FROM GROUPS WHERE GROUPNAME=?";
                                IDbDataParameter idpGROUPNAME = cmd.CreateParameter();
                                idpGROUPNAME.ParameterName = "?";
                                idpGROUPNAME.Value = group.ID;
                                cmd.Parameters.Add(idpGROUPNAME);
                            }
                            else if (ct == ClientType.ctOracle)
                            {
                                cmd.CommandText = "DELETE FROM GROUPS WHERE GROUPNAME=:GROUPNAME";
                                IDbDataParameter idpGROUPNAME = cmd.CreateParameter();
                                idpGROUPNAME.ParameterName = ":GROUPNAME";
                                idpGROUPNAME.Value = group.ID;
                                cmd.Parameters.Add(idpGROUPNAME);
                            }
                            cmd.ExecuteNonQuery();

                            cmd.Parameters.Clear();
                            if (ct == ClientType.ctMsSql || ct == ClientType.ctMySql)
                            {
                                cmd.CommandText = "INSERT INTO GROUPS (GROUPID, GROUPNAME, DESCRIPTION, MSAD) " +
                                    "VALUES (@GROUPID, @GROUPNAME, @DESCRIPTION, 'Y')";
                                IDbDataParameter idpGROUPID = cmd.CreateParameter();
                                idpGROUPID.ParameterName = "@GROUPID";
                                idpGROUPID.Value = "ad" + GetGroupID().ToString("000");

                                IDbDataParameter idpGROUPNAME = cmd.CreateParameter();
                                idpGROUPNAME.ParameterName = "@GROUPNAME";
                                idpGROUPNAME.Value = group.ID;

                                IDbDataParameter idpDESCRIPTION = cmd.CreateParameter();
                                idpDESCRIPTION.ParameterName = "@DESCRIPTION";
                                idpDESCRIPTION.Value = group.Description;

                                cmd.Parameters.Add(idpGROUPID);
                                cmd.Parameters.Add(idpGROUPNAME);
                                cmd.Parameters.Add(idpDESCRIPTION);
                            }
                            else if (ct == ClientType.ctOleDB || ct == ClientType.ctSybase || ct == ClientType.ctODBC || ct == ClientType.ctInformix)
                            {
                                cmd.CommandText = "INSERT INTO GROUPS (GROUPID, GROUPNAME, DESCRIPTION, MSAD) " +
                                    "VALUES (?, ?, ?, 'Y')";
                                IDbDataParameter idpGROUPID = cmd.CreateParameter();
                                idpGROUPID.ParameterName = "?";
                                idpGROUPID.Value = "ad" + GetGroupID().ToString("000");

                                IDbDataParameter idpGROUPNAME = cmd.CreateParameter();
                                idpGROUPNAME.ParameterName = "?";
                                idpGROUPNAME.Value = group.ID;

                                IDbDataParameter idpDESCRIPTION = cmd.CreateParameter();
                                idpDESCRIPTION.ParameterName = "?";
                                idpDESCRIPTION.Value = group.Description;

                                cmd.Parameters.Add(idpGROUPID);
                                cmd.Parameters.Add(idpGROUPNAME);
                                cmd.Parameters.Add(idpDESCRIPTION);
                            }
                            else if (ct == ClientType.ctOracle)
                            {
                                cmd.CommandText = "INSERT INTO GROUPS (GROUPID, GROUPNAME, DESCRIPTION, MSAD) " +
                                    "VALUES (:GROUPID, :GROUPNAME, :DESCRIPTION, 'Y')";
                                IDbDataParameter idpGROUPID = cmd.CreateParameter();
                                idpGROUPID.ParameterName = ":GROUPID";
                                idpGROUPID.Value = "ad" + GetGroupID().ToString("000");

                                IDbDataParameter idpGROUPNAME = cmd.CreateParameter();
                                idpGROUPNAME.ParameterName = ":GROUPNAME";
                                idpGROUPNAME.Value = group.ID;

                                IDbDataParameter idpDESCRIPTION = cmd.CreateParameter();
                                idpDESCRIPTION.ParameterName = ":DESCRIPTION";
                                idpDESCRIPTION.Value = group.Description;

                                cmd.Parameters.Add(idpGROUPID);
                                cmd.Parameters.Add(idpGROUPNAME);
                                cmd.Parameters.Add(idpDESCRIPTION);
                            }
                            cmd.ExecuteNonQuery();

                            foreach (String user in group.Users)
                            {
                                cmd.Parameters.Clear();
                                if (ct == ClientType.ctMsSql || ct == ClientType.ctMySql)
                                {
                                    cmd.CommandText = "DELETE FROM USERGROUPS WHERE GROUPID=@GROUPID AND USERID=@USERID";
                                    IDbDataParameter idpGROUPID = cmd.CreateParameter();
                                    idpGROUPID.ParameterName = "@GROUPID";
                                    idpGROUPID.Value = group.ID;
                                    cmd.Parameters.Add(idpGROUPID);

                                    IDbDataParameter idpUSERID = cmd.CreateParameter();
                                    idpUSERID.ParameterName = "@USERID";
                                    idpUSERID.Value = user;
                                    cmd.Parameters.Add(idpUSERID);
                                }
                                else if (ct == ClientType.ctOleDB || ct == ClientType.ctSybase || ct == ClientType.ctODBC || ct == ClientType.ctInformix)
                                {
                                    cmd.CommandText = "DELETE FROM USERGROUPS WHERE GROUPID=? AND USERID=?";
                                    IDbDataParameter idpGROUPID = cmd.CreateParameter();
                                    idpGROUPID.ParameterName = "?";
                                    idpGROUPID.Value = group.ID;
                                    cmd.Parameters.Add(idpGROUPID);

                                    IDbDataParameter idpUSERID = cmd.CreateParameter();
                                    idpUSERID.ParameterName = "?";
                                    idpUSERID.Value = user;
                                    cmd.Parameters.Add(idpUSERID);
                                }
                                else if (ct == ClientType.ctOracle)
                                {
                                    cmd.CommandText = "DELETE FROM USERGROUPS WHERE GROUPID=:GROUPID AND USERID=:USERID";
                                    IDbDataParameter idpGROUPID = cmd.CreateParameter();
                                    idpGROUPID.ParameterName = ":GROUPID";
                                    idpGROUPID.Value = group.ID;
                                    cmd.Parameters.Add(idpGROUPID);

                                    IDbDataParameter idpUSERID = cmd.CreateParameter();
                                    idpUSERID.ParameterName = ":USERID";
                                    idpUSERID.Value = user;
                                    cmd.Parameters.Add(idpUSERID);
                                }
                                cmd.ExecuteNonQuery();

                                if (ct == ClientType.ctMsSql || ct == ClientType.ctMySql)
                                {
                                    cmd.CommandText = "INSERT INTO USERGROUPS (GROUPID, USERID) " +
                                        "VALUES (@GROUPID, @USERID)";
                                }
                                else if (ct == ClientType.ctOleDB || ct == ClientType.ctSybase || ct == ClientType.ctODBC || ct == ClientType.ctInformix)
                                {
                                    cmd.CommandText = "INSERT INTO USERGROUPS (GROUPID, USERID) " +
                                        "VALUES (?, ?)";

                                }
                                else if (ct == ClientType.ctOracle)
                                {
                                    cmd.CommandText = "INSERT INTO USERGROUPS (GROUPID, USERID) " +
                                        "VALUES (:GROUPID, :USERID)";
                                }
                                cmd.ExecuteNonQuery();
                            }
                            break;
                        }
                    }
                }
                return new object[] { 0 };
            }
            finally
            {
                ReleaseConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), nwindConn, false);
            }
        }

        private int GetGroupID()
        {
            ClientType ct = ClientType.ctMsSql;
            IDbConnection nwindConn = AllocateConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), ref ct, false);
            try
            {
                int maxid = 0;
                String strSql = "SELECT * FROM GROUPS";
                InfoCommand cmd = new InfoCommand(ClientInfo);
                cmd.CommandText = strSql;
                cmd.Connection = nwindConn;
                IDataAdapter adpater = DBUtils.CreateDbDataAdapter(cmd);
                DataTable dt1 = new DataTable();
                (adpater as DbDataAdapter).Fill(dt1);
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    string strGroupID = dt1.Rows[i]["GROUPID"].ToString();
                    if (strGroupID.StartsWith("ad"))
                    {
                        int id = 0;
                        try
                        {
                            id = int.Parse(strGroupID.Substring(2));
                        }
                        catch { }
                        maxid = Math.Max(id, maxid);
                    }
                }
                return maxid + 1;
            }
            finally
            {
                ReleaseConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), nwindConn, false);
            }
        }

        public object[] GetADUserForGroup(object[] objParam)
        {

            ArrayList list = new ArrayList();
            foreach (var domain in ServerConfig.Domains)
            {
                try
                {
                    var ad = new ADClass() { ADPath = "LDAP://" + domain.Path, ADUser = domain.User, ADPassword = domain.Password };
                    List<ADGroup> lstGroup = ad.GetADUserForGroup();
                    list.AddRange(lstGroup.ToArray());
                }
                catch
                {
                    return new object[] { 1, "Please Setup \"EEPNetServer/Login Manger\" Frist" };
                }
            }
            return new object[] { 0, list };
        }

        public object[] DownLoadFile(object[] objParam)
        {
            string strFile = objParam[0].ToString();
            if (File.Exists(strFile))
            {
                byte[] bfile = File.ReadAllBytes(strFile);
                return new object[] { 0, 0, (object)bfile };
            }
            else
            {
                return new object[] { 0, 1 };
            }
        }

        public object[] UpLoadFile(object[] objParam)
        {
            string strFile = objParam[0].ToString();
            byte[] bfile = (byte[])objParam[1];
            string sPath = Path.GetDirectoryName(strFile);
            if (!Directory.Exists(sPath))
            {
                Directory.CreateDirectory(sPath);
            }
            if (File.Exists(strFile))
            {
                return new object[] { 0, 1 };
            }
            File.WriteAllBytes(strFile, bfile);
            return new object[] { 0, 0 };
        }

        public object[] UpLoadWorkflowFile(object[] objParam)
        {
            string strFile = objParam[0].ToString();
            byte[] bfile = (byte[])objParam[1];
            string sPath = Path.GetDirectoryName(strFile);
            if (!Directory.Exists(sPath))
            {
                Directory.CreateDirectory(sPath);
            }
            bool uploadSucceed = tryUpload(strFile, bfile);
            if (uploadSucceed)
            {
                return new object[] { 0, strFile.Substring(strFile.LastIndexOf('\\') + 1) };
            }

            int i = 1;
            string filename = "";
            while (!uploadSucceed && i < 100)
            {
                string extendedName = i.ToString();

                if (extendedName.Length == 1) extendedName = "-00" + extendedName;
                else if (extendedName.Length == 2) extendedName = "-0" + extendedName;

                if (strFile.IndexOf('.') != -1)
                {
                    filename = strFile.Insert(strFile.LastIndexOf('.'), extendedName);
                }
                else
                {
                    filename += extendedName;
                }
                uploadSucceed = tryUpload(filename, bfile);
                if (uploadSucceed)
                {
                    return new object[] { 0, filename.Substring(filename.LastIndexOf('\\') + 1) };
                }
                i++;
            }
            return new object[] { 1, strFile };
        }

        bool tryUpload(string file, byte[] bfile)
        {
            if (File.Exists(file)) return false;
            File.WriteAllBytes(file, bfile);
            return true;
        }

        public object[] DownloadModule(object[] objParam)
        {
            string sproject = objParam[0].ToString();
            string filename = objParam[1].ToString();
            bool needcheck = (bool)objParam[2];
            DateTime dts = new DateTime();

            string sfile = string.Format("{0}\\{1}\\{2}", EEPRegistry.Client, sproject, filename);
            if (!File.Exists(sfile))
            {
                return new object[] { 0, 1 };
            }
            dts = File.GetLastWriteTime(sfile);
            if (needcheck)
            {
                DateTime dtc = (DateTime)objParam[3];

                if (dtc >= dts)
                {
                    return new object[] { 0, 2 };
                }
            }
            byte[] bfile = File.ReadAllBytes(sfile);
            return new object[] { 0, 0, (object)bfile, dts };
        }

        private static bool lockflag = false;
        public object[] DoRecordLock(object[] objParam)
        {
            while (lockflag)
            {
                Thread.Sleep(100);
            }
            lockflag = true;
            string DBAlias = (string)objParam[0];
            string Table = (string)objParam[1];
            string KeyFields = (string)objParam[2];
            //string KeyValues = (string)objParam[3];
            string UserID = (string)objParam[4];
            string type = (string)objParam[5];
            if (!File.Exists(RecordLock.RecordFileName))
            {
                RecordLock.CreateRecordFile();
            }
            if (type == "Release")
            {
                ArrayList keyvalues = (ArrayList)objParam[3];
                RecordLock.RemoveRecordLock(DBAlias, Table, KeyFields, keyvalues, UserID);
            }
            else
            {
                string keyvalues = (string)objParam[3];
                RecordLock.LockType ltrtn = RecordLock.LockType.Other;
                if (type == "Updating")
                {
                    ltrtn = RecordLock.AddRecordLock(DBAlias, Table, KeyFields, keyvalues, ref UserID, RecordLock.LockType.Updating);
                }
                else if (type == "Deleting")
                {
                    ltrtn = RecordLock.AddRecordLock(DBAlias, Table, KeyFields, keyvalues, ref UserID, RecordLock.LockType.Deleting);
                }
                if (ltrtn == RecordLock.LockType.Idle)
                {
                    if ((string)objParam[7] == "ReLoad")
                    {
                        ClientType ct = ClientType.ctMsSql;
                        IDbConnection nwindConn = AllocateConnection(DBAlias, ref ct, false);
                        try
                        {
                            string strSql = (string)objParam[6];
                            InfoCommand cmd = new InfoCommand(ClientInfo);

                            string[] arrkeyfields = KeyFields.Split(';');
                            string[] arrkeyvalues = keyvalues.Split(';');
                            string whereString = "";

                            for (int i = 0; i < arrkeyfields.Length; i++)
                            {
                                if (whereString != "")
                                {
                                    whereString += "and ";
                                }
                                whereString += "[" + Table + "].[" + arrkeyfields[i] + "] = '" + arrkeyvalues[i] + "' ";
                            }
                            string newSql = CliUtils.InsertWhere(strSql, whereString);

                            cmd.CommandText = newSql;

                            cmd.Connection = nwindConn;
                            IDataAdapter adpater = DBUtils.CreateDbDataAdapter(cmd);

                            DataSet ds = new DataSet();
                            DataTable dt = new DataTable();
                            (adpater as DbDataAdapter).Fill(dt);
                            ds.Tables.Add(dt);
                            if (ds.Tables[0].Rows.Count == 0)
                            {
                                ArrayList arrlist = new ArrayList();
                                arrlist.Add(keyvalues);
                                RecordLock.RemoveRecordLock(DBAlias, Table, KeyFields, arrlist, UserID);
                            }
                            lockflag = false;
                            return new object[] { 0, 0, ds };
                        }
                        finally
                        {
                            ReleaseConnection(DBAlias, nwindConn, false);
                        }
                    }
                    else
                    {
                        lockflag = false;
                        return new object[] { 0, 0 };
                    }


                }
                else
                {
                    lockflag = false;
                    switch (ltrtn)
                    {
                        case RecordLock.LockType.Updating: return new object[] { 0, 1, "Updating", UserID };
                        case RecordLock.LockType.Deleting: return new object[] { 0, 1, "Deleting", UserID };
                        default: return new object[] { 0, 1, "Other", UserID };
                    }

                }
            }
            lockflag = false;
            return new object[] { 0 };
        }

        public object[] PackageUpload(object[] objParam)    //修改比较多,多写点注释:(   oracle要改写...
        {
            string projectname = ((string)objParam[0]);
            string filename = (string)objParam[1];                    //应该包括.dll
            string package = Path.GetFileNameWithoutExtension(filename);
            DateTime dtclient = ((DateTime)objParam[2]);//取得Client传上来的Dll时间
            PackageType ptype = ((PackageType)objParam[3]);
            byte[] data = ((byte[])objParam[4]);   //取得Client传上来的Dll内容
            string packagetype = "";
            switch (ptype)
            {
                case PackageType.Client: packagetype = "C"; break;
                case PackageType.Server: packagetype = "S"; break;
                case PackageType.WebClient: packagetype = "W"; break;
            }

            ClientType ct = ClientType.ctMsSql;
            IDbConnection nwindConn = AllocateConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), ref ct, true);  //menuchecklog用sysDB里的
            try
            {
                IDbCommand cmd = nwindConn.CreateCommand();
                cmd.CommandText = string.Format("Select * from MENUCHECKLOG where FILENAME = '{0}' and ITEMTYPE = '{1}' and FILETYPE = '{2}' order by PACKAGEDATE desc"
                                , filename, projectname, packagetype);

                IDataReader reader = cmd.ExecuteReader();
                DateTime dtdb = DateTime.MinValue;
                if (reader.Read())                                             //找不到记录
                {
                    try
                    {
                        dtdb = (DateTime)reader["PACKAGEDATE"];                   //找到记录
                    }
                    catch { }
                }
                cmd.Cancel();
                reader.Close();
                DateTime dtserver = new DateTime();
                byte[] buff;
                string strsql = string.Empty;
                PackageService ps = new PackageService();
                if (nwindConn is SqlConnection)
                {
                    strsql = "Insert into MENUCHECKLOG (ITEMTYPE,PACKAGE,PACKAGEDATE,FILETYPE,[FILENAME],FILEDATE,FILECONTENT) values ('{0}', '{1}','{2:yyyy/MM/dd HH:mm:ss}','{3}','{4}','{5:yyyy/MM/dd HH:mm:ss}',@content)";
                }
                else if (nwindConn is OdbcConnection)
                {
                    strsql = "Insert into MENUCHECKLOG (ITEMTYPE,PACKAGE,PACKAGEDATE,FILETYPE,FILENAME,FILEDATE,FILECONTENT) values ('{0}', '{1}',to_date('{2:yyyyMMddHHmmss}','%Y%m%d%H%M%S'),'{3}','{4}',to_date('{5:yyyyMMddHHmmss}','%Y%m%d%H%M%S'),?)";
                }
                else if (nwindConn is OracleConnection)
                {
                    strsql = "Insert into MENUCHECKLOG (ITEMTYPE,PACKAGE,PACKAGEDATE,FILETYPE,FILENAME,FILEDATE,FILECONTENT) values ('{0}', '{1}',to_date('{2:yyyy/MM/dd HH:mm:ss}','yyyy/mm/dd hh24:mi:ss'),'{3}','{4}',to_date('{5:yyyy/MM/dd HH:mm:ss}','yyyy/mm/dd hh24:mi:ss'),:content)";
                }
                else if (nwindConn is OleDbConnection)
                {
                    strsql = "Insert into MENUCHECKLOG (ITEMTYPE,PACKAGE,PACKAGEDATE,FILETYPE,FILENAME,FILEDATE,FILECONTENT) values ('{0}', '{1}','{2:yyyy/MM/dd HH:mm:ss}','{3}','{4}','{5:yyyy/MM/dd HH:mm:ss}',?)";
                }
                else if (nwindConn.GetType().Name == "MySqlConnection")
                {
                    strsql = "Insert into MENUCHECKLOG (ITEMTYPE,PACKAGE,PACKAGEDATE,FILETYPE,FILENAME,FILEDATE,FILECONTENT) values ('{0}', '{1}','{2:yyyy/MM/dd HH:mm:ss}','{3}','{4}','{5:yyyy/MM/dd HH:mm:ss}',?content)";
                }
                else if (nwindConn.GetType().Name == "IfxConnection")
                {
                    strsql = "Insert into MENUCHECKLOG (ITEMTYPE,PACKAGE,PACKAGEDATE,FILETYPE,FILENAME,FILEDATE,FILECONTENT) values ('{0}', '{1}',to_date('{2:yyyyMMddHHmmss}','%Y%m%d%H%M%S'),'{3}','{4}',to_date('{5:yyyyMMddHHmmss}','%Y%m%d%H%M%S'),?)";
                }

                cmd = nwindConn.CreateCommand();
                if (ps.VersionControl(filename, projectname, ptype, dtdb, out buff, out dtserver))   //Server是否已经存在有这个DLL,如果有,而且比数据库里的版本新,要备份到数据库
                {
                    cmd.CommandText = string.Format(strsql, projectname, package, dtserver, packagetype, filename, dtserver);
                    IDbDataParameter parameter = cmd.CreateParameter();
                    if (nwindConn is SqlConnection)
                        parameter.ParameterName = "@content";
                    else if (nwindConn is OdbcConnection)
                        parameter.ParameterName = "content";
                    else if (nwindConn is OracleConnection)
                        parameter.ParameterName = ":content";
                    else if (nwindConn is OleDbConnection)
                    {
                        parameter.ParameterName = "@content";
                        (parameter as OleDbParameter).OleDbType = OleDbType.LongVarBinary;
                    }
                    else if (nwindConn.GetType().Name == "MySqlConnection")
                        parameter.ParameterName = "?content";
                    else if (nwindConn.GetType().Name == "IfxConnection")
                        parameter.ParameterName = "?";
                    parameter.Value = buff;
                    cmd.Parameters.Add(parameter);
                    cmd.ExecuteNonQuery();
                }
                cmd.CommandText = string.Format(strsql, projectname, package, DateTime.Now, packagetype, filename, dtclient);
                if (cmd.Parameters.Contains("@content"))
                {
                    (cmd.Parameters["@content"] as IDbDataParameter).Value = data;
                }
                else if (cmd.Parameters.Contains("content"))
                {
                    (cmd.Parameters["content"] as IDbDataParameter).Value = data;
                }
                else if (cmd.Parameters.Contains("?"))
                {
                    (cmd.Parameters["?"] as IDbDataParameter).Value = data;
                }
                else if (cmd.Parameters.Contains(":content"))
                {
                    (cmd.Parameters[":content"] as IDbDataParameter).Value = data;
                }
                else
                {
                    IDbDataParameter parameter = cmd.CreateParameter();
                    parameter.ParameterName = "content";
                    parameter.Value = data;
                    cmd.Parameters.Add(parameter);
                }
                cmd.ExecuteNonQuery();
                ps.Upload(filename, projectname, ptype, data, dtclient);//将DLL拷贝到Server端的临时文件夹,这个DLL的版本是最新的,在ServerUpdate.exe里更新后删除

                return new object[] { 0 };
            }
            finally
            {
                ReleaseConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), nwindConn, true);
            }
        }

        //更改使用文件名来查找
        public object[] PackageDownLoad(object[] objParam)
        {
            string projectname = ((string)objParam[0]);
            string filename = ((string)objParam[1]);
            string dt = ((string)objParam[2]);
            PackageType ptype = ((PackageType)objParam[3]);
            DateTime dtpackage = new DateTime();
            try
            {
                dtpackage = DateTime.Parse(dt);
            }
            catch (Exception e)
            {
                return new object[] { 0, 1, e.Message };
            }
            string packagetype = "";
            switch (ptype)
            {
                case PackageType.Client: packagetype = "C"; break;
                case PackageType.Server: packagetype = "S"; break;
                case PackageType.WebClient: packagetype = "W"; break;
            }
            ClientType ct = ClientType.ctMsSql;
            //IDbConnection nwindConn = AllocateConnection(GetSystemDBName(), ref ct, false);  //menuchecklog用sysDB里的
            IDbConnection nwindConn = AllocateConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), ref ct, true);  //取登陆时的DBAlias ——by Rei
            try
            {
                string strBlob = "";
                if (nwindConn is SqlConnection)
                    strBlob = "SELECT FILECONTENT, FILEDATE FROM MENUCHECKLOG WHERE [FILENAME] = '" + filename + "' AND PACKAGEDATE = '" + dtpackage.ToString("yyyy/MM/dd HH:mm:ss")
                            + "' AND ITEMTYPE ='" + projectname + "' AND FILETYPE='" + packagetype + "'";
                else if (nwindConn is OdbcConnection)
                    strBlob = "SELECT FILECONTENT, FILEDATE FROM MENUCHECKLOG WHERE FILENAME = '" + filename + "' AND PACKAGEDATE = to_date('" + String.Format("{0:yyyyMMddHHmmss}", dtpackage) + "', '%Y%m%d%H%M%S')"
                            + " AND ITEMTYPE ='" + projectname + "' AND FILETYPE='" + packagetype + "'";
                else if (nwindConn is OracleConnection)
                    strBlob = "SELECT FILECONTENT, FILEDATE FROM MENUCHECKLOG WHERE FILENAME = '" + filename + "' AND PACKAGEDATE = " + "to_date('" + dtpackage.ToString("yyyy/MM/dd HH:mm:ss") + "', 'yyyy/mm/dd hh24:mi:ss')"
                            + " AND ITEMTYPE ='" + projectname + "' AND FILETYPE='" + packagetype + "'";
                else if (nwindConn is OleDbConnection)
                    strBlob = "SELECT FILECONTENT, FILEDATE FROM MENUCHECKLOG WHERE FILENAME = '" + filename + "' AND PACKAGEDATE = '" + dtpackage.ToString("yyyy/MM/dd HH:mm:ss")
                            + "' AND ITEMTYPE ='" + projectname + "' AND FILETYPE='" + packagetype + "'";
                else if (nwindConn.GetType().Name == "MySqlConnection")
                    strBlob = "SELECT FILECONTENT, FILEDATE FROM MENUCHECKLOG WHERE FILENAME = '" + filename + "' AND PACKAGEDATE = '" + dtpackage.ToString("yyyy/MM/dd HH:mm:ss")
                            + "' AND ITEMTYPE ='" + projectname + "' AND FILETYPE='" + packagetype + "'";
                else if (nwindConn.GetType().Name == "IfxConnection")
                    strBlob = "SELECT FILECONTENT, FILEDATE FROM MENUCHECKLOG WHERE FILENAME = '" + filename + "' AND PACKAGEDATE = to_date('" + String.Format("{0:yyyyMMddHHmmss}", dtpackage) + "', '%Y%m%d%H%M%S')"
                            + " AND ITEMTYPE ='" + projectname + "' AND FILETYPE='" + packagetype + "'";

                InfoCommand cmd = new InfoCommand(ClientInfo);
                cmd.Connection = nwindConn;
                cmd.CommandText = strBlob;
                IDataReader idr = cmd.ExecuteReader();
                idr.Read();

                try
                {
                    byte[] blob = new byte[idr.GetBytes(0, 0, null, 0, int.MaxValue)];
                    idr.GetBytes(0, 0, blob, 0, blob.Length);
                    DateTime dtfile = new DateTime();
                    try
                    {
                        dtfile = (DateTime)idr.GetValue(1);                   //找到记录
                    }
                    catch
                    {
                        dtfile = DateTime.MinValue;
                    }
                    cmd.Cancel();
                    idr.Close();
                    return new object[] { 0, 0, blob, dtfile };
                }
                catch (Exception e)
                {
                    return new object[] { 0, 1, e.Message };

                }
            }
            finally
            {
                ReleaseConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), nwindConn, true);
            }
        }

        public object[] PackageRollback(object[] objParam)
        {
            string projectname = ((string)objParam[0]);
            string filename = ((string)objParam[1]);
            string dt = ((string)objParam[2]);
            PackageType ptype = ((PackageType)objParam[3]);
            string packagetype = "";
            switch (ptype)
            {
                case PackageType.Client: packagetype = "C"; break;
                case PackageType.Server: packagetype = "S"; break;
                case PackageType.WebClient: packagetype = "W"; break;
            }
            ClientType ct = ClientType.ctMsSql;
            //IDbConnection nwindConn = AllocateConnection(GetSystemDBName(), ref ct, false);  //menuchecklog用sysDB里的
            IDbConnection nwindConn = AllocateConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), ref ct, true);  //取登陆后的DBAlias ——by Rei
            try
            {
                string strsql = "";
                if (nwindConn is SqlConnection)
                    strsql = "DELETE FROM MENUCHECKLOG WHERE ITEMTYPE ='" + projectname + "' AND [FILENAME] ='" + filename
                        + "' AND FILETYPE='" + packagetype + "' AND PACKAGEDATE >'" + dt + "'";
                else if (nwindConn is OdbcConnection)
                    strsql = "DELETE FROM MENUCHECKLOG WHERE ITEMTYPE ='" + projectname + "' AND FILENAME ='" + filename
                        + "' AND FILETYPE='" + packagetype + "' AND PACKAGEDATE > to_date('" + String.Format("{0:yyyyMMddHHmmss}", Convert.ToDateTime(dt)) + "', '%Y%m%d%H%M%S')";
                else if (nwindConn is OracleConnection)
                    strsql = "DELETE FROM MENUCHECKLOG WHERE ITEMTYPE ='" + projectname + "' AND FILENAME ='" + filename
                        + "' AND FILETYPE='" + packagetype + "' AND PACKAGEDATE > to_date('" + dt + "', 'yyyy-mm-dd hh24:mi:ss')";
                else if (nwindConn is OleDbConnection)
                    strsql = "DELETE FROM MENUCHECKLOG WHERE ITEMTYPE ='" + projectname + "' AND FILENAME ='" + filename
                        + "' AND FILETYPE='" + packagetype + "' AND PACKAGEDATE >'" + dt + "'";
                else if (nwindConn.GetType().Name == "MySqlConnection")
                    strsql = "DELETE FROM MENUCHECKLOG WHERE ITEMTYPE ='" + projectname + "' AND FILENAME ='" + filename
                        + "' AND FILETYPE='" + packagetype + "' AND PACKAGEDATE > '" + dt + "'";
                else if (nwindConn.GetType().Name == "IfxConnection")
                    strsql = "DELETE FROM MENUCHECKLOG WHERE ITEMTYPE ='" + projectname + "' AND FILENAME ='" + filename
                        + "' AND FILETYPE='" + packagetype + "' AND PACKAGEDATE > to_date('" + String.Format("{0:yyyyMMddHHmmss}", Convert.ToDateTime(dt)) + "', '%Y%m%d%H%M%S')";

                InfoCommand cmd = new InfoCommand(ClientInfo);
                cmd.Connection = nwindConn;
                cmd.CommandText = strsql;
                cmd.ExecuteNonQuery();

                string strBlob = "";
                if (nwindConn is SqlConnection)
                    strBlob = "SELECT FILECONTENT, FILEDATE FROM MENUCHECKLOG WHERE [FILENAME] = '" + filename + "' AND PACKAGEDATE = '" + dt
                            + "' AND ITEMTYPE ='" + projectname + "' AND FILETYPE='" + packagetype + "'";
                else if (nwindConn is OdbcConnection)
                    strBlob = "SELECT FILECONTENT, FILEDATE FROM MENUCHECKLOG WHERE FILENAME = '" + filename + "' AND PACKAGEDATE = to_date('" + String.Format("{0:yyyyMMddHHmmss}", Convert.ToDateTime(dt)) + "', '%Y%m%d%H%M%S') "
                            + " AND ITEMTYPE ='" + projectname + "' AND FILETYPE='" + packagetype + "'";
                else if (nwindConn is OracleConnection)
                    strBlob = "SELECT FILECONTENT, FILEDATE FROM MENUCHECKLOG WHERE FILENAME = '" + filename + "' AND PACKAGEDATE = to_date('" + dt + "', 'yyyy-mm-dd hh24:mi:ss')"
                            + " AND ITEMTYPE ='" + projectname + "' AND FILETYPE='" + packagetype + "'";
                else if (nwindConn is OleDbConnection)
                    strBlob = "SELECT FILECONTENT, FILEDATE FROM MENUCHECKLOG WHERE FILENAME = '" + filename + "' AND PACKAGEDATE = '" + dt
                            + "' AND ITEMTYPE ='" + projectname + "' AND FILETYPE='" + packagetype + "'";
                else if (nwindConn.GetType().Name == "MySqlConnection")
                    strBlob = "SELECT FILECONTENT, FILEDATE FROM MENUCHECKLOG WHERE FILENAME = '" + filename + "' AND PACKAGEDATE = '" + dt + "'"
                            + " AND ITEMTYPE ='" + projectname + "' AND FILETYPE='" + packagetype + "'";
                else if (nwindConn.GetType().Name == "IfxConnection")
                    strBlob = "SELECT FILECONTENT, FILEDATE FROM MENUCHECKLOG WHERE FILENAME = '" + filename + "' AND PACKAGEDATE = to_date('" + String.Format("{0:yyyyMMddHHmmss}", Convert.ToDateTime(dt)) + "', '%Y%m%d%H%M%S') "
                            + " AND ITEMTYPE ='" + projectname + "' AND FILETYPE='" + packagetype + "'";
                cmd.CommandText = strBlob;

                IDataReader idr = cmd.ExecuteReader();
                idr.Read();

                try
                {
                    byte[] blob = new byte[idr.GetBytes(0, 0, null, 0, int.MaxValue)];
                    idr.GetBytes(0, 0, blob, 0, blob.Length);
                    DateTime dtfile = new DateTime();
                    try
                    {
                        dtfile = (DateTime)idr.GetValue(1);                   //找到记录
                    }
                    catch
                    {
                        dtfile = DateTime.MinValue;
                    }
                    cmd.Cancel();
                    idr.Close();
                    PackageService ps = new PackageService();
                    ps.Upload(filename, projectname, ptype, blob, dtfile);
                    return new object[] { 0, 0 };
                }
                catch (Exception e)
                {
                    return new object[] { 0, 1, e.Message };
                }
            }
            finally
            {
                ReleaseConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), nwindConn, true);
            }
        }

        public object[] GetLogDatas(object[] objParam)
        {
            DataTable tabLog = new DataTable();
            bool isLogSqlSentence = (bool)objParam[0];
            string startDate = (string)objParam[1];
            string endDate = (string)objParam[2];
            bool exportError = false;
            if (objParam.Length == 4 && objParam[3] != null && objParam[3] is bool)
                exportError = (bool)objParam[3];
            string logDir = string.Format("{0}\\{1}", EEPRegistry.Server, "EEPLogFiles");
            DirectoryInfo dirInfo = new DirectoryInfo(logDir);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }
            FileInfo[] files = dirInfo.GetFiles();
            if (!isLogSqlSentence)
            {
                #region CreateLogData
                DataColumn colConnID = new DataColumn("CONNID", typeof(string));
                DataColumn colLogStyle = new DataColumn("LOGSTYLE", typeof(string));
                DataColumn colLogDateTime = new DataColumn("LOGDATETIME", typeof(DateTime));
                DataColumn colDomainID = new DataColumn("DOMAINID", typeof(string));
                DataColumn colUserID = new DataColumn("USERID", typeof(string));
                DataColumn colLogType = new DataColumn("LOGTYPE", typeof(string));
                DataColumn colTitle = new DataColumn("TITLE", typeof(string));
                DataColumn colDescription = new DataColumn("DESCRIPTION", typeof(string));
                DataColumn colComputerIP = new DataColumn("COMPUTERIP", typeof(string));
                DataColumn colComputerName = new DataColumn("COMPUTERNAME", typeof(string));
                DataColumn colExecutionTime = new DataColumn("EXECUTIONTIME", typeof(string));
                tabLog.Columns.AddRange(new DataColumn[] { 
                colConnID, 
                colLogStyle,
                colLogDateTime,
                colDomainID,
                colUserID,
                colLogType,
                colTitle,
                colDescription,
                colComputerIP,
                colComputerName,
                colExecutionTime
                });

                foreach (FileInfo file in files)
                {
                    if (!file.Name.StartsWith("sql"))
                    {
                        //string simpName = file.Name.Substring(0, file.Name.IndexOf(file.Extension));
                        String simpName = file.Name;
                        if (simpName.Contains("_"))
                            simpName = file.Name.Substring(0, file.Name.IndexOf("_"));  //读取一天里所有的Logs档
                        if (simpName.CompareTo(startDate) >= 0 && simpName.CompareTo(endDate) <= 0)
                        {
                            using (StreamReader r = file.OpenText())
                            {
                                while (r.Peek() != -1)
                                {
                                    string log = r.ReadLine();
                                    if (log != null && log != "" && log.Split(';').Length == 11)
                                    {
                                        string[] logs = log.Split(';');
                                        if (!exportError && logs[5] == "2") continue;
                                        DataRow row = tabLog.NewRow();
                                        row["CONNID"] = logs[0];
                                        row["LOGSTYLE"] = logs[1];
                                        row["LOGDATETIME"] = Convert.ToDateTime(logs[2]);
                                        row["DOMAINID"] = logs[3];
                                        row["USERID"] = logs[4];
                                        row["LOGTYPE"] = logs[5];
                                        row["TITLE"] = logs[6];
                                        row["DESCRIPTION"] = logs[7];
                                        row["COMPUTERIP"] = logs[8];
                                        row["COMPUTERNAME"] = logs[9];
                                        row["EXECUTIONTIME"] = logs[10];
                                        DataRow[] rows = tabLog.Select("CONNID='" + logs[0] + "'");
                                        foreach (DataRow fixRow in rows)
                                        {
                                            tabLog.Rows.Remove(fixRow);
                                        }
                                        tabLog.Rows.Add(row);
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion
            }
            else
            {
                #region CreateSqlLogData
                DataColumn colUserID = new DataColumn("USERId", typeof(string));
                DataColumn colLogDateTime = new DataColumn("LOGDATETIME", typeof(DateTime));
                DataColumn colSqlLogType = new DataColumn("SQLLOGTYPE", typeof(string));
                DataColumn colTitle = new DataColumn("TITLE", typeof(string));
                DataColumn colSqlSentence = new DataColumn("SQLSENTENCE", typeof(string));
                tabLog.Columns.AddRange(new DataColumn[] { 
                colUserID, 
                colLogDateTime,
                colSqlLogType,
                colTitle,
                colSqlSentence
                });

                foreach (FileInfo file in files)
                {
                    if (file.Name.StartsWith("sql"))
                    {
                        //string simpName = file.Name.Substring(3, file.Name.IndexOf(file.Extension) - 3);
                        String simpName = file.Name;
                        if (simpName.Contains("_"))
                            simpName = file.Name.Substring(0, file.Name.IndexOf("_")).Replace("sql", string.Empty);  //读取一天里所有的Logs档
                        if (simpName.CompareTo(startDate) >= 0 && simpName.CompareTo(endDate) <= 0)
                        {
                            using (StreamReader r = file.OpenText())
                            {
                                while (r.Peek() != -1)
                                {
                                    string log = r.ReadLine();
                                    if (log != null && log != "" && log.Split(';').Length >= 5)
                                    {
                                        DataRow row = tabLog.NewRow();
                                        row["USERId"] = log.Split(';')[0];
                                        row["LOGDATETIME"] = Convert.ToDateTime(log.Split(';')[1]);
                                        row["SQLLOGTYPE"] = log.Split(';')[2];
                                        row["TITLE"] = log.Split(';')[3];
                                        string sentence = "";
                                        for (int i = 4; i < log.Split(';').Length; i++)
                                        {
                                            if (i == log.Split(';').Length - 1)
                                                sentence += log.Split(';')[i];
                                            else
                                                sentence += log.Split(';')[i] + ";";
                                        }
                                        sentence = sentence.Replace("\\r\\n", "\r\n");
                                        row["SQLSENTENCE"] = sentence;

                                        tabLog.Rows.Add(row);
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion
            }
            return new object[] { 0, tabLog };
        }

        public object[] GetDBLog(object[] objParam)
        {
            ClientType ct = ClientType.ctMsSql;
            string sysdb = GetSystemDBName();
            IDbConnection nwindConn = AllocateConnection(sysdb, ref ct, false);
            try
            {
                InfoCommand cmd = new InfoCommand(ClientInfo);
                cmd.CommandText = (string)objParam[0];
                cmd.Connection = nwindConn;
                IDataAdapter adpater = DBUtils.CreateDbDataAdapter(cmd);

                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                (adpater as DbDataAdapter).Fill(dt);
                ds.Tables.Add(dt);
                return new object[] { 0, ds };
            }
            finally
            {
                ReleaseConnection(sysdb, nwindConn, false);
            }
        }

        //ILogin 不用
        public object[] GetPasswordLastDate(object[] param)
        {
            ClientType ct = ClientType.ctMsSql;
            String userid = param[0].ToString();
            IDbConnection nwindConn = AllocateConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), ref ct, true);
            try
            {
                String sql = "SELECT * FROM USERS WHERE USERID='" + userid + "'";
                InfoCommand myCommand = new InfoCommand(ClientInfo);

                myCommand.Connection = nwindConn;
                myCommand.CommandText = sql;

                IDataAdapter adpater = DBUtils.CreateDbDataAdapter(myCommand);

                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                (adpater as DbDataAdapter).Fill(dt);
                ds.Tables.Add(dt);
                String date = "";
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    date = ds.Tables[0].Rows[0]["LASTDATE"].ToString();
                return new object[] { 0, date };
            }
            finally
            {
                ReleaseConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), nwindConn, true);
            }
        }

        public object[] AnyQueryLoadFile(object[] param)
        {
            ClientType ct = ClientType.ctMsSql;
            String queryID = param[0].ToString();
            String userid = GetClientInfo(ClientInfoType.LoginUser).ToString();
            IDbConnection nwindConn = AllocateConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), ref ct, true);
            try
            {
                String sql = "SELECT DISTINCT TEMPLATEID from SYS_ANYQUERY where USERID = '" + userid + "' AND QUERYID='" + queryID + "'";
                InfoCommand myCommand = new InfoCommand(ClientInfo);

                myCommand.Connection = nwindConn;
                myCommand.CommandText = sql;

                IDataAdapter adpater = DBUtils.CreateDbDataAdapter(myCommand);

                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                (adpater as DbDataAdapter).Fill(dt);
                ds.Tables.Add(dt);
                return new object[] { 0, ds };
            }
            finally
            {
                ReleaseConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), nwindConn, true);
            }
        }

        public object[] AnyQueryDeleteFile(object[] param)
        {
            ClientType ct = ClientType.ctMsSql;
            String queryID = param[0].ToString();
            String fileName = param[1].ToString();
            String userid = GetClientInfo(ClientInfoType.LoginUser).ToString();
            IDbConnection nwindConn = AllocateConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), ref ct, true);
            try
            {
                String sql = "DELETE from SYS_ANYQUERY where USERID = '" + userid + "' AND QUERYID='" + queryID + "' AND TEMPLATEID='" + fileName + "'";
                InfoCommand myCommand = new InfoCommand(ClientInfo);

                myCommand.Connection = nwindConn;
                myCommand.CommandText = sql;

                myCommand.ExecuteNonQuery();
                return new object[] { 0 };
            }
            finally
            {
                ReleaseConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), nwindConn, true);
            }
        }

        public object[] UpdatePackage(object[] param)
        {
            Process.Start(string.Format("{0}\\{1}", EEPRegistry.Server, "EEPServerUpdate.exe"));
            Environment.Exit(0);

            return new object[] { 0 };
        }

        public object[] UserDefineLog(object[] param)
        {
            if (param.Length >= 2 && SysEEPLogService.Enable)
            {
                string title = (string)param[0];
                string description = (string)param[1];

                SysEEPLog eeplog = new SysEEPLog(base.GetClientInfo(), SysEEPLog.LogStyleType.UserDefine
                    , SysEEPLog.LogTypeType.Normal, DateTime.Now, title, description);

                eeplog.Log();
            }
            return new object[] { 0 };
        }

        public object[] AnyQuerySave(object[] param)
        {
            String userID = GetClientInfo(ClientInfoType.LoginUser).ToString();
            String queryID = param[0].ToString();
            String templateID = param[1].ToString();
            String xmlText = param[2].ToString();
            String tableName = param[3].ToString();

            ClientType ct = ClientType.ctMsSql;
            IDbConnection nwindConn = AllocateConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), ref ct, true);
            try
            {
                String sql = "DELETE FROM SYS_ANYQUERY WHERE USERID='" + userID + "' AND QUERYID='" + queryID + "' AND TEMPLATEID='" + templateID + "'";
                InfoCommand myCommand = new InfoCommand(ClientInfo);

                myCommand.Connection = nwindConn;
                myCommand.CommandText = sql;
                myCommand.ExecuteNonQuery();

                String dt = DateTime.Now.ToShortDateString() + " " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;
                if (ct == ClientType.ctMsSql)
                {
                    sql = "INSERT INTO SYS_ANYQUERY (QUERYID, USERID, TEMPLATEID, TABLENAME, LASTDATE, CONTENT) VALUES ('" + queryID + "', '" + userID + "', '" + templateID + "', '" + tableName + "', '" + dt + "', @CONTENT)";
                    SqlParameter paramContent = new SqlParameter();
                    paramContent.ParameterName = "@CONTENT";
                    paramContent.SqlDbType = SqlDbType.Text;
                    paramContent.Value = xmlText;
                    myCommand.CommandText = sql;
                    myCommand.Parameters.Add(paramContent);
                }
                else if (ct == ClientType.ctOracle)
                {
                    sql = "INSERT INTO SYS_ANYQUERY (QUERYID, USERID, TEMPLATEID, TABLENAME, LASTDATE, CONTENT) VALUES ('" + queryID + "', '" + userID + "', '" + templateID + "', '" + tableName + "', to_date('" + dt + "', 'yyyy/mm/dd hh24:mi:ss'), :CONTENT)";
                    OracleParameter paramContent = new OracleParameter();
                    paramContent.ParameterName = ":CONTENT";
                    paramContent.OracleType = OracleType.Blob;
                    byte[] byteArray = System.Text.Encoding.Default.GetBytes(xmlText);
                    paramContent.Value = byteArray;
                    myCommand.CommandText = sql;
                    myCommand.Parameters.Add(paramContent);
                }
                else if (ct == ClientType.ctODBC)
                {
                    sql = "INSERT INTO SYS_ANYQUERY (QUERYID, USERID, TEMPLATEID, TABLENAME, LASTDATE, CONTENT) VALUES ('" + queryID + "', '" + userID + "', '" + templateID + "', '" + tableName + "', '" + dt + "', ?)";
                    OdbcParameter paramContent = new OdbcParameter();
                    paramContent.ParameterName = "@CONTENT";
                    paramContent.OdbcType = OdbcType.NText;
                    paramContent.Value = xmlText;
                    myCommand.CommandText = sql;
                    myCommand.Parameters.Add(paramContent);
                }
                else if (ct == ClientType.ctOleDB)
                {
                    sql = "INSERT INTO SYS_ANYQUERY (QUERYID, USERID, TEMPLATEID, TABLENAME, LASTDATE, CONTENT) VALUES ('" + queryID + "', '" + userID + "', '" + templateID + "', '" + tableName + "', '" + dt + "', ?)";
                    OleDbParameter paramContent = new OleDbParameter();
                    paramContent.ParameterName = "@CONTENT";
                    paramContent.OleDbType = OleDbType.Binary;
                    paramContent.Value = xmlText;
                    myCommand.CommandText = sql;
                    myCommand.Parameters.Add(paramContent);
                }
#if MySQL
                else if (ct == ClientType.ctMySql)
                {
                    sql = "INSERT INTO SYS_ANYQUERY (QUERYID, USERID, TEMPLATEID, TABLENAME, LASTDATE, CONTENT) VALUES ('" + queryID + "', '" + userID + "', '" + templateID + "', '" + tableName + "', '" + dt + "', @CONTENT)";
                    SqlParameter paramContent = new SqlParameter();
                    paramContent.ParameterName = "@CONTENT";
                    paramContent.SqlDbType = SqlDbType.Text;
                    paramContent.Value = xmlText;
                    myCommand.CommandText = sql;
                    myCommand.Parameters.Add(paramContent);
                }
#endif
#if Informix
                else if (ct == ClientType.ctInformix)
                {
                    sql = "INSERT INTO SYS_ANYQUERY (QUERYID, USERID, TEMPLATEID, TABLENAME, LASTDATE, CONTENT) VALUES ('" + queryID + "', '" + userID + "', '" + templateID + "', '" + tableName + "', '" + dt + "', ?)";
                    IBM.Data.Informix.IfxParameter paramContent = new IBM.Data.Informix.IfxParameter();
                    paramContent.ParameterName = "?";
                    paramContent.IfxType = IBM.Data.Informix.IfxType.Text;
                    paramContent.Value = xmlText;
                    myCommand.CommandText = sql;
                    myCommand.Parameters.Add(paramContent);
                }
#endif
                myCommand.ExecuteNonQuery();

                return new object[] { 0 };
            }
            finally
            {
                ReleaseConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), nwindConn, true);
            }
        }

        public object[] AnyQueryLoad(object[] param)
        {
            String userID = GetClientInfo(ClientInfoType.LoginUser).ToString();
            String queryID = param[0].ToString();
            String templateID = param[1].ToString();
            String xmlText = String.Empty;
            String tableName = String.Empty;

            ClientType ct = ClientType.ctMsSql;
            IDbConnection nwindConn = AllocateConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), ref ct, true);
            try
            {
                String sql = "SELECT * FROM SYS_ANYQUERY WHERE USERID='" + userID + "' AND QUERYID='" + queryID + "' AND TEMPLATEID='" + templateID + "'";
                InfoCommand myCommand = new InfoCommand(ClientInfo);

                myCommand.Connection = nwindConn;
                myCommand.CommandText = sql;
                IDataAdapter adpater = DBUtils.CreateDbDataAdapter(myCommand);

                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                (adpater as DbDataAdapter).Fill(dt);
                ds.Tables.Add(dt);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["CONTENT"] is byte[])
                        xmlText = System.Text.Encoding.Default.GetString((byte[])ds.Tables[0].Rows[0]["CONTENT"]);
                    else if (ds.Tables[0].Rows[0]["CONTENT"] is String)
                        xmlText = ds.Tables[0].Rows[0]["CONTENT"].ToString();
                    tableName = ds.Tables[0].Rows[0]["TABLENAME"].ToString();
                }
                return new object[] { 0, xmlText, tableName };
            }
            finally
            {
                ReleaseConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), nwindConn, true);
            }
        }

        public object[] GetPasswordPolicy(object[] param)
        {
            return new object[] { 0, ServerConfig.PassWordMinSize, ServerConfig.PasswordMaxSize, ServerConfig.PasswordCharNum };
        }

        public object[] GetAllUsers(object[] param)
        {

            if (ServerConfig.LoginObjectEnabled)
            {
                DataSet ds = new DataSet();
                DataTable table = ds.Tables.Add();
                table.Columns.Add("USERID", typeof(string));
                table.Columns.Add("USERName", typeof(string));

                string[] users = ServerConfig.LoginObject.GetAllUsers();
                foreach (string user in users)
                {
                    table.Rows.Add(new object[] { user, user });
                }
                return new object[] { 0, ds };
            }
            else
            {
                ClientType ct = ClientType.ctMsSql;
                IDbConnection nwindConn = AllocateConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), ref ct, true);
                try
                {
                    string strSql = "select * from USERS";

                    //为了区分不同的数据库 by Rei
                    InfoCommand cmd = new InfoCommand(ClientInfo);

                    cmd.CommandText = strSql;
                    cmd.Connection = nwindConn;
                    IDataAdapter adpater = DBUtils.CreateDbDataAdapter(cmd);

                    DataSet ds = new DataSet();
                    DataTable dt = new DataTable();
                    (adpater as DbDataAdapter).Fill(dt);
                    ds.Tables.Add(dt);

                    return new object[] { 0, ds };
                }
                finally
                {
                    ReleaseConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), nwindConn, true);
                }
            }


        }

        public object[] GetAllGroups(object[] param)
        {
            if (ServerConfig.LoginObjectEnabled)
            {
                DataSet ds = new DataSet();
                DataTable table = ds.Tables.Add();
                table.Columns.Add("GROUPID", typeof(string));
                table.Columns.Add("GROUPNAME", typeof(string));

                string[] groups = ServerConfig.LoginObject.GetAllGroups();
                foreach (string group in groups)
                {
                    table.Rows.Add(new object[] { group, group });
                }
                return new object[] { 0, ds };
            }
            else
            {
                ClientType ct = ClientType.ctMsSql;
                IDbConnection nwindConn = AllocateConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), ref ct, true);
                try
                {
                    string strSql = "select * from GROUPS";

                    //为了区分不同的数据库 by Rei
                    InfoCommand cmd = new InfoCommand(ClientInfo);

                    cmd.CommandText = strSql;
                    cmd.Connection = nwindConn;
                    IDataAdapter adpater = DBUtils.CreateDbDataAdapter(cmd);

                    DataSet ds = new DataSet();
                    DataTable dt = new DataTable();
                    (adpater as DbDataAdapter).Fill(dt);
                    ds.Tables.Add(dt);

                    return new object[] { 0, ds };
                }
                finally
                {
                    ReleaseConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), nwindConn, true);
                }
            }
        }

        public object[] SavePersonalSettings(object[] objParam)
        {
            ClientType ct = ClientType.ctMsSql;
            IDbConnection conncetion = AllocateConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), ref ct, true);
            string formName = (string)objParam[0];
            string compName = (string)objParam[1];
            string userId = (string)objParam[2];
            string remark = (string)objParam[3];
            string propContent = (string)objParam[4];

            string sql = string.Format("SELECT COUNT(*) FROM SYS_PERSONAL WHERE FORMNAME='{0}' AND COMPNAME='{1}' AND USERID='{2}'",
                formName,
                compName,
                userId);
            InfoCommand cmd = new InfoCommand(ClientInfo);
            cmd.Connection = conncetion;
            cmd.CommandText = sql;
            try
            {
                int count = (int)cmd.ExecuteScalar();
                if (count > 0)
                {
                    sql = string.Format("UPDATE SYS_PERSONAL SET REMARK='{0}',PROPCONTENT='{1}',CREATEDATE='{2}' WHERE FORMNAME='{3}' AND COMPNAME='{4}' AND USERID='{5}'",
                        remark,
                        propContent,
                        DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                        formName,
                        compName,
                        userId);
                }
                else
                {
                    sql = string.Format("INSERT INTO SYS_PERSONAL (FORMNAME,COMPNAME,USERID,REMARK,PROPCONTENT,CREATEDATE) VALUES ('{0}','{1}','{2}','{3}','{4}','{5}')",
                        formName,
                        compName,
                        userId,
                        remark,
                        propContent,
                        DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                return new object[] { 0 };
            }
            catch (Exception e)
            {
                return new object[] { 1, e.Message };
            }
            finally
            {
                ReleaseConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), conncetion, true);
            }
        }

        public object[] LoadPersonalSettings(object[] objParam)
        {
            ClientType ct = ClientType.ctMsSql;
            IDbConnection conncetion = AllocateConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), ref ct, true);
            string formName = (string)objParam[0];
            string compName = (string)objParam[1];
            string userId = (string)objParam[2];
            string sql = string.Format("SELECT * FROM SYS_PERSONAL WHERE FORMNAME='{0}' AND COMPNAME='{1}' AND USERID='{2}'",
                formName,
                compName,
                userId);
            InfoCommand cmd = new InfoCommand(ClientInfo);
            cmd.Connection = conncetion;
            cmd.CommandText = sql;
            try
            {
                IDataAdapter adpater = DBUtils.CreateDbDataAdapter(cmd);
                DataTable dt = new DataTable();
                (adpater as DbDataAdapter).Fill(dt);
                return new object[] { 0, dt };
            }
            catch (Exception e)
            {
                return new object[] { 1, e.Message };
            }
            finally
            {
                ReleaseConnection(GetClientInfo(ClientInfoType.LoginDB).ToString(), conncetion, true);
            }
        }


        #region Component Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Srvtools.KeyItem keyItem1 = new Srvtools.KeyItem();
            Srvtools.KeyItem keyItem2 = new Srvtools.KeyItem();
            Srvtools.ColumnItem columnItem1 = new Srvtools.ColumnItem();
            Srvtools.ColumnItem columnItem2 = new Srvtools.ColumnItem();
            Srvtools.ColumnItem columnItem3 = new Srvtools.ColumnItem();
            Srvtools.ColumnItem columnItem4 = new Srvtools.ColumnItem();
            Srvtools.KeyItem keyItem3 = new Srvtools.KeyItem();
            Srvtools.KeyItem keyItem4 = new Srvtools.KeyItem();
            Srvtools.KeyItem keyItem5 = new Srvtools.KeyItem();
            Srvtools.FieldAttr fieldAttr1 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr2 = new Srvtools.FieldAttr();
            Srvtools.KeyItem keyItem6 = new Srvtools.KeyItem();
            Srvtools.KeyItem keyItem7 = new Srvtools.KeyItem();
            Srvtools.KeyItem keyItem8 = new Srvtools.KeyItem();
            Srvtools.KeyItem keyItem9 = new Srvtools.KeyItem();
            Srvtools.ColumnItem columnItem5 = new Srvtools.ColumnItem();
            Srvtools.ColumnItem columnItem6 = new Srvtools.ColumnItem();
            Srvtools.KeyItem keyItem10 = new Srvtools.KeyItem();
            Srvtools.KeyItem keyItem11 = new Srvtools.KeyItem();
            Srvtools.FieldAttr fieldAttr3 = new Srvtools.FieldAttr();
            Srvtools.KeyItem keyItem12 = new Srvtools.KeyItem();
            Srvtools.KeyItem keyItem13 = new Srvtools.KeyItem();
            Srvtools.KeyItem keyItem14 = new Srvtools.KeyItem();
            Srvtools.KeyItem keyItem15 = new Srvtools.KeyItem();
            Srvtools.KeyItem keyItem16 = new Srvtools.KeyItem();
            Srvtools.Service service1 = new Srvtools.Service();
            Srvtools.Service service2 = new Srvtools.Service();
            Srvtools.Service service3 = new Srvtools.Service();
            Srvtools.Service service4 = new Srvtools.Service();
            Srvtools.Service service5 = new Srvtools.Service();
            Srvtools.Service service6 = new Srvtools.Service();
            Srvtools.Service service7 = new Srvtools.Service();
            Srvtools.Service service8 = new Srvtools.Service();
            Srvtools.Service service9 = new Srvtools.Service();
            Srvtools.Service service10 = new Srvtools.Service();
            Srvtools.Service service11 = new Srvtools.Service();
            Srvtools.Service service12 = new Srvtools.Service();
            Srvtools.Service service13 = new Srvtools.Service();
            Srvtools.Service service14 = new Srvtools.Service();
            Srvtools.Service service15 = new Srvtools.Service();
            Srvtools.Service service16 = new Srvtools.Service();
            Srvtools.Service service17 = new Srvtools.Service();
            Srvtools.Service service18 = new Srvtools.Service();
            Srvtools.Service service19 = new Srvtools.Service();
            Srvtools.Service service20 = new Srvtools.Service();
            Srvtools.Service service21 = new Srvtools.Service();
            Srvtools.Service service22 = new Srvtools.Service();
            Srvtools.Service service23 = new Srvtools.Service();
            Srvtools.Service service24 = new Srvtools.Service();
            Srvtools.Service service25 = new Srvtools.Service();
            Srvtools.Service service26 = new Srvtools.Service();
            Srvtools.Service service27 = new Srvtools.Service();
            Srvtools.Service service28 = new Srvtools.Service();
            Srvtools.Service service29 = new Srvtools.Service();
            Srvtools.Service service30 = new Srvtools.Service();
            Srvtools.Service service31 = new Srvtools.Service();
            Srvtools.Service service32 = new Srvtools.Service();
            Srvtools.Service service33 = new Srvtools.Service();
            Srvtools.Service service34 = new Srvtools.Service();
            Srvtools.Service service35 = new Srvtools.Service();
            Srvtools.Service service36 = new Srvtools.Service();
            Srvtools.Service service37 = new Srvtools.Service();
            Srvtools.Service service38 = new Srvtools.Service();
            Srvtools.Service service39 = new Srvtools.Service();
            Srvtools.Service service40 = new Srvtools.Service();
            Srvtools.Service service41 = new Srvtools.Service();
            Srvtools.Service service42 = new Srvtools.Service();
            Srvtools.Service service43 = new Srvtools.Service();
            Srvtools.Service service44 = new Srvtools.Service();
            Srvtools.Service service45 = new Srvtools.Service();
            Srvtools.Service service46 = new Srvtools.Service();
            Srvtools.Service service47 = new Srvtools.Service();
            Srvtools.Service service48 = new Srvtools.Service();
            Srvtools.Service service49 = new Srvtools.Service();
            Srvtools.Service service50 = new Srvtools.Service();
            Srvtools.Service service51 = new Srvtools.Service();
            Srvtools.Service service52 = new Srvtools.Service();
            Srvtools.Service service53 = new Srvtools.Service();
            Srvtools.Service service54 = new Srvtools.Service();
            Srvtools.Service service55 = new Srvtools.Service();
            Srvtools.Service service56 = new Srvtools.Service();
            Srvtools.Service service57 = new Srvtools.Service();
            Srvtools.Service service58 = new Srvtools.Service();
            Srvtools.Service service59 = new Srvtools.Service();
            Srvtools.Service service60 = new Srvtools.Service();
            Srvtools.Service service61 = new Srvtools.Service();
            Srvtools.Service service62 = new Srvtools.Service();
            Srvtools.Service service63 = new Srvtools.Service();
            Srvtools.Service service64 = new Srvtools.Service();
            Srvtools.Service service65 = new Srvtools.Service();
            Srvtools.Service service66 = new Srvtools.Service();
            Srvtools.Service service67 = new Srvtools.Service();
            Srvtools.Service service68 = new Srvtools.Service();
            Srvtools.Service service69 = new Srvtools.Service();
            Srvtools.Service service70 = new Srvtools.Service();
            Srvtools.Service service71 = new Srvtools.Service();
            Srvtools.Service service72 = new Srvtools.Service();
            Srvtools.Service service73 = new Srvtools.Service();
            Srvtools.Service service74 = new Srvtools.Service();
            Srvtools.Service service75 = new Srvtools.Service();
            Srvtools.Service service76 = new Srvtools.Service();
            Srvtools.Service service77 = new Srvtools.Service();
            Srvtools.Service service78 = new Srvtools.Service();
            Srvtools.Service service79 = new Srvtools.Service();
            Srvtools.Service service80 = new Srvtools.Service();
            Srvtools.Service service81 = new Srvtools.Service();
            Srvtools.Service service82 = new Srvtools.Service();
            Srvtools.Service service83 = new Srvtools.Service();
            Srvtools.Service service84 = new Srvtools.Service();
            Srvtools.Service service85 = new Srvtools.Service();
            Srvtools.Service service86 = new Srvtools.Service();
            Srvtools.Service service87 = new Srvtools.Service();
            Srvtools.Service service88 = new Srvtools.Service();
            Srvtools.Service service89 = new Srvtools.Service();
            Srvtools.Service service90 = new Srvtools.Service();
            Srvtools.Service service91 = new Srvtools.Service();
            Srvtools.Service service92 = new Srvtools.Service();
            Srvtools.Service service93 = new Srvtools.Service();
            Srvtools.Service service94 = new Srvtools.Service();
            Srvtools.Service service95 = new Srvtools.Service();
            Srvtools.Service service96 = new Srvtools.Service();
            Srvtools.Service service97 = new Srvtools.Service();
            Srvtools.Service service98 = new Srvtools.Service();
            Srvtools.Service service99 = new Srvtools.Service();
            Srvtools.Service service100 = new Srvtools.Service();
            Srvtools.Service service101 = new Srvtools.Service();
            Srvtools.Service service102 = new Srvtools.Service();
            Srvtools.Service service103 = new Srvtools.Service();
            Srvtools.Service service104 = new Srvtools.Service();
            Srvtools.Service service105 = new Srvtools.Service();
            Srvtools.Service service106 = new Srvtools.Service();
            Srvtools.Service service107 = new Srvtools.Service();
            Srvtools.Service service108 = new Srvtools.Service();
            Srvtools.Service service109 = new Srvtools.Service();
            Srvtools.Service service110 = new Srvtools.Service();
            Srvtools.Service service111 = new Srvtools.Service();
            Srvtools.Service service112 = new Srvtools.Service();
            Srvtools.Service service113 = new Srvtools.Service();
            Srvtools.Service service114 = new Srvtools.Service();
            Srvtools.Service service115 = new Srvtools.Service();
            Srvtools.Service service116 = new Srvtools.Service();
            Srvtools.Service service117 = new Srvtools.Service();
            Srvtools.Service service118 = new Srvtools.Service();
            Srvtools.Service service119 = new Srvtools.Service();
            Srvtools.Service service120 = new Srvtools.Service();
            Srvtools.KeyItem keyItem17 = new Srvtools.KeyItem();
            Srvtools.KeyItem keyItem18 = new Srvtools.KeyItem();
            Srvtools.KeyItem keyItem19 = new Srvtools.KeyItem();
            Srvtools.KeyItem keyItem20 = new Srvtools.KeyItem();
            Srvtools.KeyItem keyItem21 = new Srvtools.KeyItem();
            Srvtools.KeyItem keyItem22 = new Srvtools.KeyItem();
            Srvtools.KeyItem keyItem23 = new Srvtools.KeyItem();
            Srvtools.KeyItem keyItem24 = new Srvtools.KeyItem();
            Srvtools.FieldAttr fieldAttr4 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr5 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr6 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr7 = new Srvtools.FieldAttr();
            this.userInfo = new Srvtools.InfoCommand(this.components);
            this.groupInfo = new Srvtools.InfoCommand(this.components);
            this.UGInfo = new Srvtools.InfoCommand(this.components);
            this.sqlMGroups = new Srvtools.InfoCommand(this.components);
            this.sqlMGroupMenus = new Srvtools.InfoCommand(this.components);
            this.sqlMenus = new Srvtools.InfoCommand(this.components);
            this.dsUserGroups = new Srvtools.InfoDataSource(this.components);
            this.updateCompGroups = new Srvtools.UpdateComponent(this.components);
            this.updateCompUsers = new Srvtools.UpdateComponent(this.components);
            this.dsGroupMenus = new Srvtools.InfoDataSource(this.components);
            this.packageInfo = new Srvtools.InfoCommand(this.components);
            this.updateCompUGInfo = new Srvtools.UpdateComponent(this.components);
            this.updateCompSolution = new Srvtools.UpdateComponent(this.components);
            this.solutionInfo = new Srvtools.InfoCommand(this.components);
            this.menuTableLogInfo = new Srvtools.InfoCommand(this.components);
            this.updateCompMenuTableLog = new Srvtools.UpdateComponent(this.components);
            this.menuTableLogInfoWithoutBinary = new Srvtools.InfoCommand(this.components);
            this.updateCompMenuTableLogWithoutBinary = new Srvtools.UpdateComponent(this.components);
            this.cmdDBTables = new Srvtools.InfoCommand(this.components);
            this.cmdColDEF = new Srvtools.InfoCommand(this.components);
            this.updateColDEF = new Srvtools.UpdateComponent(this.components);
            this.cmdColDEF_Details = new Srvtools.InfoCommand(this.components);
            this.updateColDEF_Details = new Srvtools.UpdateComponent(this.components);
            this.cmdRefValUse = new Srvtools.InfoCommand(this.components);
            this.cmdERRLOG = new Srvtools.InfoCommand(this.components);
            this.updateERRLOG = new Srvtools.UpdateComponent(this.components);
            this.cmdSYSEEPLOGforDB = new Srvtools.InfoCommand(this.components);
            this.cmdDDUse = new Srvtools.InfoCommand(this.components);
            this.updateCompMGroupMenus = new Srvtools.UpdateComponent(this.components);
            this.userMenus = new Srvtools.InfoCommand(this.components);
            this.updateCompUserMenus = new Srvtools.UpdateComponent(this.components);
            this.packageversion = new Srvtools.InfoCommand(this.components);
            this.cmdSysRefVal = new Srvtools.InfoCommand(this.components);
            this.cmdSysRefVal_D = new Srvtools.InfoCommand(this.components);
            this.updateCompSysRefVal = new Srvtools.UpdateComponent(this.components);
            this.updateCompSysRefVal_D = new Srvtools.UpdateComponent(this.components);
            this.idsRefVal = new Srvtools.InfoDataSource(this.components);
            this.cmdToDoList = new Srvtools.InfoCommand(this.components);
            this.cmdToDoHis = new Srvtools.InfoCommand(this.components);
            this.cmdRoles = new Srvtools.InfoCommand(this.components);
            this.cmdOrgRoles = new Srvtools.InfoCommand(this.components);
            this.ucOrgRoles = new Srvtools.UpdateComponent(this.components);
            this.cmdOrgLevel = new Srvtools.InfoCommand(this.components);
            this.ucOrgLevel = new Srvtools.UpdateComponent(this.components);
            this.cmdOrgKind = new Srvtools.InfoCommand(this.components);
            this.ucOrgKind = new Srvtools.UpdateComponent(this.components);
            this.cmdWorkflow = new Srvtools.InfoCommand(this.components);
            this.cmdRoleAgent = new Srvtools.InfoCommand(this.components);
            this.ucRoleAgent = new Srvtools.UpdateComponent(this.components);
            this.cmdSlSource = new Srvtools.InfoCommand(this.components);
            this.serviceManager1 = new Srvtools.ServiceManager(this.components);
            this.cmdGROUPMENUCONTROL = new Srvtools.InfoCommand(this.components);
            this.cmdUSERMENUCONTROL = new Srvtools.InfoCommand(this.components);
            this.cmdSYS_REPORT = new Srvtools.InfoCommand(this.components);
            this.cmdMENUTABLECONTROL = new Srvtools.InfoCommand(this.components);
            this.ucMENUTABLECONTROL = new Srvtools.UpdateComponent(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.userInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UGInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sqlMGroups)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sqlMGroupMenus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sqlMenus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.packageInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.solutionInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.menuTableLogInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.menuTableLogInfoWithoutBinary)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmdDBTables)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmdColDEF)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmdColDEF_Details)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmdRefValUse)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmdERRLOG)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmdSYSEEPLOGforDB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmdDDUse)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.userMenus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.packageversion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmdSysRefVal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmdSysRefVal_D)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmdToDoList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmdToDoHis)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmdRoles)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmdOrgRoles)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmdOrgLevel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmdOrgKind)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmdWorkflow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmdRoleAgent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmdSlSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmdGROUPMENUCONTROL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmdUSERMENUCONTROL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmdSYS_REPORT)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmdMENUTABLECONTROL)).BeginInit();
            // 
            // userInfo
            // 
            this.userInfo.CacheConnection = false;
            this.userInfo.CommandText = "select * from USERS order by USERID";
            this.userInfo.CommandTimeout = 0;
            this.userInfo.CommandType = System.Data.CommandType.Text;
            this.userInfo.DynamicTableName = false;
            this.userInfo.EEPAlias = null;
            this.userInfo.EncodingAfter = null;
            this.userInfo.EncodingBefore = "Windows-1252";
            this.userInfo.InfoConnection = null;
            keyItem1.KeyName = "USERID";
            this.userInfo.KeyFields.Add(keyItem1);
            this.userInfo.MultiSetWhere = false;
            this.userInfo.Name = "userInfo";
            this.userInfo.NotificationAutoEnlist = false;
            this.userInfo.SecExcept = "";
            this.userInfo.SecFieldName = null;
            this.userInfo.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.userInfo.SelectPaging = false;
            this.userInfo.SelectTop = 0;
            this.userInfo.SiteControl = false;
            this.userInfo.SiteFieldName = null;
            this.userInfo.UpdatedRowSource = System.Data.UpdateRowSource.Both;
            // 
            // groupInfo
            // 
            this.groupInfo.CacheConnection = false;
            this.groupInfo.CommandText = "select * from GROUPS";
            this.groupInfo.CommandTimeout = 0;
            this.groupInfo.CommandType = System.Data.CommandType.Text;
            this.groupInfo.DynamicTableName = false;
            this.groupInfo.EEPAlias = null;
            this.groupInfo.EncodingAfter = null;
            this.groupInfo.EncodingBefore = "Windows-1252";
            this.groupInfo.InfoConnection = null;
            keyItem2.KeyName = "GROUPID";
            this.groupInfo.KeyFields.Add(keyItem2);
            this.groupInfo.MultiSetWhere = false;
            this.groupInfo.Name = "groupInfo";
            this.groupInfo.NotificationAutoEnlist = false;
            this.groupInfo.SecExcept = "";
            this.groupInfo.SecFieldName = null;
            this.groupInfo.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.groupInfo.SelectPaging = false;
            this.groupInfo.SelectTop = 0;
            this.groupInfo.SiteControl = false;
            this.groupInfo.SiteFieldName = null;
            this.groupInfo.UpdatedRowSource = System.Data.UpdateRowSource.Both;
            // 
            // UGInfo
            // 
            this.UGInfo.CacheConnection = false;
            this.UGInfo.CommandText = "select USERGROUPS.*, USERS.USERNAME from USERGROUPS left join USERS on USERS.USER" +
    "ID = USERGROUPS.USERID";
            this.UGInfo.CommandTimeout = 0;
            this.UGInfo.CommandType = System.Data.CommandType.Text;
            this.UGInfo.DynamicTableName = false;
            this.UGInfo.EEPAlias = null;
            this.UGInfo.EncodingAfter = null;
            this.UGInfo.EncodingBefore = "Windows-1252";
            this.UGInfo.InfoConnection = null;
            this.UGInfo.MultiSetWhere = false;
            this.UGInfo.Name = "UGInfo";
            this.UGInfo.NotificationAutoEnlist = false;
            this.UGInfo.SecExcept = "";
            this.UGInfo.SecFieldName = null;
            this.UGInfo.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.UGInfo.SelectPaging = false;
            this.UGInfo.SelectTop = 0;
            this.UGInfo.SiteControl = false;
            this.UGInfo.SiteFieldName = null;
            this.UGInfo.UpdatedRowSource = System.Data.UpdateRowSource.Both;
            // 
            // sqlMGroups
            // 
            this.sqlMGroups.CacheConnection = false;
            this.sqlMGroups.CommandText = "select * from GROUPS";
            this.sqlMGroups.CommandTimeout = 0;
            this.sqlMGroups.CommandType = System.Data.CommandType.Text;
            this.sqlMGroups.DynamicTableName = false;
            this.sqlMGroups.EEPAlias = null;
            this.sqlMGroups.EncodingAfter = null;
            this.sqlMGroups.EncodingBefore = "Windows-1252";
            this.sqlMGroups.InfoConnection = null;
            this.sqlMGroups.MultiSetWhere = false;
            this.sqlMGroups.Name = "sqlMGroups";
            this.sqlMGroups.NotificationAutoEnlist = false;
            this.sqlMGroups.SecExcept = "";
            this.sqlMGroups.SecFieldName = null;
            this.sqlMGroups.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.sqlMGroups.SelectPaging = false;
            this.sqlMGroups.SelectTop = 0;
            this.sqlMGroups.SiteControl = false;
            this.sqlMGroups.SiteFieldName = null;
            this.sqlMGroups.UpdatedRowSource = System.Data.UpdateRowSource.Both;
            // 
            // sqlMGroupMenus
            // 
            this.sqlMGroupMenus.CacheConnection = false;
            this.sqlMGroupMenus.CommandText = "select * from GROUPMENUS";
            this.sqlMGroupMenus.CommandTimeout = 0;
            this.sqlMGroupMenus.CommandType = System.Data.CommandType.Text;
            this.sqlMGroupMenus.DynamicTableName = false;
            this.sqlMGroupMenus.EEPAlias = null;
            this.sqlMGroupMenus.EncodingAfter = null;
            this.sqlMGroupMenus.EncodingBefore = "Windows-1252";
            this.sqlMGroupMenus.InfoConnection = null;
            this.sqlMGroupMenus.MultiSetWhere = false;
            this.sqlMGroupMenus.Name = "sqlMGroupMenus";
            this.sqlMGroupMenus.NotificationAutoEnlist = false;
            this.sqlMGroupMenus.SecExcept = "";
            this.sqlMGroupMenus.SecFieldName = null;
            this.sqlMGroupMenus.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.sqlMGroupMenus.SelectPaging = false;
            this.sqlMGroupMenus.SelectTop = 0;
            this.sqlMGroupMenus.SiteControl = false;
            this.sqlMGroupMenus.SiteFieldName = null;
            this.sqlMGroupMenus.UpdatedRowSource = System.Data.UpdateRowSource.Both;
            // 
            // sqlMenus
            // 
            this.sqlMenus.CacheConnection = false;
            this.sqlMenus.CommandText = "select * from MENUTABLE order by SEQ_NO,MENUID";
            this.sqlMenus.CommandTimeout = 0;
            this.sqlMenus.CommandType = System.Data.CommandType.Text;
            this.sqlMenus.DynamicTableName = false;
            this.sqlMenus.EEPAlias = null;
            this.sqlMenus.EncodingAfter = null;
            this.sqlMenus.EncodingBefore = "Windows-1252";
            this.sqlMenus.InfoConnection = null;
            this.sqlMenus.MultiSetWhere = false;
            this.sqlMenus.Name = "sqlMenus";
            this.sqlMenus.NotificationAutoEnlist = false;
            this.sqlMenus.SecExcept = "";
            this.sqlMenus.SecFieldName = null;
            this.sqlMenus.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.sqlMenus.SelectPaging = false;
            this.sqlMenus.SelectTop = 0;
            this.sqlMenus.SiteControl = false;
            this.sqlMenus.SiteFieldName = null;
            this.sqlMenus.UpdatedRowSource = System.Data.UpdateRowSource.Both;
            // 
            // dsUserGroups
            // 
            this.dsUserGroups.Detail = this.UGInfo;
            columnItem1.FieldName = "GROUPID";
            this.dsUserGroups.DetailColumns.Add(columnItem1);
            this.dsUserGroups.DynamicTableName = false;
            this.dsUserGroups.Master = this.groupInfo;
            columnItem2.FieldName = "GROUPID";
            this.dsUserGroups.MasterColumns.Add(columnItem2);
            // 
            // updateCompGroups
            // 
            this.updateCompGroups.AutoTrans = false;
            this.updateCompGroups.ExceptJoin = false;
            this.updateCompGroups.LogInfo = null;
            this.updateCompGroups.Name = "updateCompGroups";
            this.updateCompGroups.RowAffectsCheck = true;
            this.updateCompGroups.SelectCmd = this.groupInfo;
            this.updateCompGroups.SelectCmdForUpdate = null;
            this.updateCompGroups.ServerModify = true;
            this.updateCompGroups.ServerModifyGetMax = false;
            this.updateCompGroups.TranscationScopeTimeOut = System.TimeSpan.Parse("00:02:00");
            this.updateCompGroups.TransIsolationLevel = System.Data.IsolationLevel.ReadCommitted;
            this.updateCompGroups.UseTranscationScope = false;
            this.updateCompGroups.WhereMode = Srvtools.WhereModeType.All;
            // 
            // updateCompUsers
            // 
            this.updateCompUsers.AutoTrans = false;
            this.updateCompUsers.ExceptJoin = false;
            this.updateCompUsers.LogInfo = null;
            this.updateCompUsers.Name = "updateCompUsers";
            this.updateCompUsers.RowAffectsCheck = true;
            this.updateCompUsers.SelectCmd = this.userInfo;
            this.updateCompUsers.SelectCmdForUpdate = null;
            this.updateCompUsers.ServerModify = true;
            this.updateCompUsers.ServerModifyGetMax = false;
            this.updateCompUsers.TranscationScopeTimeOut = System.TimeSpan.Parse("00:02:00");
            this.updateCompUsers.TransIsolationLevel = System.Data.IsolationLevel.ReadCommitted;
            this.updateCompUsers.UseTranscationScope = false;
            this.updateCompUsers.WhereMode = Srvtools.WhereModeType.Keyfields;
            // 
            // dsGroupMenus
            // 
            this.dsGroupMenus.Detail = this.sqlMGroupMenus;
            columnItem3.FieldName = "GROUPID";
            this.dsGroupMenus.DetailColumns.Add(columnItem3);
            this.dsGroupMenus.DynamicTableName = false;
            this.dsGroupMenus.Master = this.sqlMGroups;
            columnItem4.FieldName = "GROUPID";
            this.dsGroupMenus.MasterColumns.Add(columnItem4);
            // 
            // packageInfo
            // 
            this.packageInfo.CacheConnection = false;
            this.packageInfo.CommandText = "SELECT ITEMTYPE, FILENAME, FILEDATE, PACKAGEDATE from MENUCHECKLOG WHERE LOGID IN" +
    "(SELECT MAX(LOGID) FROM MENUCHECKLOG GROUP BY ITEMTYPE,FILENAME ) ORDER BY FILEN" +
    "AME";
            this.packageInfo.CommandTimeout = 0;
            this.packageInfo.CommandType = System.Data.CommandType.Text;
            this.packageInfo.DynamicTableName = false;
            this.packageInfo.EEPAlias = null;
            this.packageInfo.EncodingAfter = null;
            this.packageInfo.EncodingBefore = "Windows-1252";
            this.packageInfo.InfoConnection = null;
            this.packageInfo.MultiSetWhere = false;
            this.packageInfo.Name = "packageInfo";
            this.packageInfo.NotificationAutoEnlist = false;
            this.packageInfo.SecExcept = "";
            this.packageInfo.SecFieldName = null;
            this.packageInfo.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.packageInfo.SelectPaging = false;
            this.packageInfo.SelectTop = 0;
            this.packageInfo.SiteControl = false;
            this.packageInfo.SiteFieldName = null;
            this.packageInfo.UpdatedRowSource = System.Data.UpdateRowSource.Both;
            // 
            // updateCompUGInfo
            // 
            this.updateCompUGInfo.AutoTrans = false;
            this.updateCompUGInfo.ExceptJoin = false;
            this.updateCompUGInfo.LogInfo = null;
            this.updateCompUGInfo.Name = "updateCompUGInfo";
            this.updateCompUGInfo.RowAffectsCheck = true;
            this.updateCompUGInfo.SelectCmd = this.UGInfo;
            this.updateCompUGInfo.SelectCmdForUpdate = null;
            this.updateCompUGInfo.ServerModify = true;
            this.updateCompUGInfo.ServerModifyGetMax = false;
            this.updateCompUGInfo.TranscationScopeTimeOut = System.TimeSpan.Parse("00:02:00");
            this.updateCompUGInfo.TransIsolationLevel = System.Data.IsolationLevel.ReadCommitted;
            this.updateCompUGInfo.UseTranscationScope = false;
            this.updateCompUGInfo.WhereMode = Srvtools.WhereModeType.All;
            // 
            // updateCompSolution
            // 
            this.updateCompSolution.AutoTrans = false;
            this.updateCompSolution.ExceptJoin = false;
            this.updateCompSolution.LogInfo = null;
            this.updateCompSolution.Name = "updateCompSolution";
            this.updateCompSolution.RowAffectsCheck = true;
            this.updateCompSolution.SelectCmd = this.solutionInfo;
            this.updateCompSolution.SelectCmdForUpdate = null;
            this.updateCompSolution.ServerModify = true;
            this.updateCompSolution.ServerModifyGetMax = false;
            this.updateCompSolution.TranscationScopeTimeOut = System.TimeSpan.Parse("00:02:00");
            this.updateCompSolution.TransIsolationLevel = System.Data.IsolationLevel.ReadCommitted;
            this.updateCompSolution.UseTranscationScope = false;
            this.updateCompSolution.WhereMode = Srvtools.WhereModeType.All;
            // 
            // solutionInfo
            // 
            this.solutionInfo.CacheConnection = false;
            this.solutionInfo.CommandText = "select * from MENUITEMTYPE";
            this.solutionInfo.CommandTimeout = 0;
            this.solutionInfo.CommandType = System.Data.CommandType.Text;
            this.solutionInfo.DynamicTableName = false;
            this.solutionInfo.EEPAlias = null;
            this.solutionInfo.EncodingAfter = null;
            this.solutionInfo.EncodingBefore = "Windows-1252";
            this.solutionInfo.InfoConnection = null;
            this.solutionInfo.MultiSetWhere = false;
            this.solutionInfo.Name = "solutionInfo";
            this.solutionInfo.NotificationAutoEnlist = false;
            this.solutionInfo.SecExcept = "";
            this.solutionInfo.SecFieldName = null;
            this.solutionInfo.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.solutionInfo.SelectPaging = false;
            this.solutionInfo.SelectTop = 0;
            this.solutionInfo.SiteControl = false;
            this.solutionInfo.SiteFieldName = null;
            this.solutionInfo.UpdatedRowSource = System.Data.UpdateRowSource.Both;
            // 
            // menuTableLogInfo
            // 
            this.menuTableLogInfo.CacheConnection = false;
            this.menuTableLogInfo.CommandText = "select * from MENUTABLELOG";
            this.menuTableLogInfo.CommandTimeout = 0;
            this.menuTableLogInfo.CommandType = System.Data.CommandType.Text;
            this.menuTableLogInfo.DynamicTableName = false;
            this.menuTableLogInfo.EEPAlias = null;
            this.menuTableLogInfo.EncodingAfter = null;
            this.menuTableLogInfo.EncodingBefore = "Windows-1252";
            this.menuTableLogInfo.InfoConnection = null;
            this.menuTableLogInfo.MultiSetWhere = false;
            this.menuTableLogInfo.Name = "menuTableLogInfo";
            this.menuTableLogInfo.NotificationAutoEnlist = false;
            this.menuTableLogInfo.SecExcept = "";
            this.menuTableLogInfo.SecFieldName = null;
            this.menuTableLogInfo.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.menuTableLogInfo.SelectPaging = false;
            this.menuTableLogInfo.SelectTop = 0;
            this.menuTableLogInfo.SiteControl = false;
            this.menuTableLogInfo.SiteFieldName = null;
            this.menuTableLogInfo.UpdatedRowSource = System.Data.UpdateRowSource.Both;
            // 
            // updateCompMenuTableLog
            // 
            this.updateCompMenuTableLog.AutoTrans = false;
            this.updateCompMenuTableLog.ExceptJoin = false;
            this.updateCompMenuTableLog.LogInfo = null;
            this.updateCompMenuTableLog.Name = "updateCompMenuTableLog";
            this.updateCompMenuTableLog.RowAffectsCheck = true;
            this.updateCompMenuTableLog.SelectCmd = this.menuTableLogInfo;
            this.updateCompMenuTableLog.SelectCmdForUpdate = null;
            this.updateCompMenuTableLog.ServerModify = true;
            this.updateCompMenuTableLog.ServerModifyGetMax = false;
            this.updateCompMenuTableLog.TranscationScopeTimeOut = System.TimeSpan.Parse("00:02:00");
            this.updateCompMenuTableLog.TransIsolationLevel = System.Data.IsolationLevel.ReadCommitted;
            this.updateCompMenuTableLog.UseTranscationScope = false;
            this.updateCompMenuTableLog.WhereMode = Srvtools.WhereModeType.FieldAttrs;
            // 
            // menuTableLogInfoWithoutBinary
            // 
            this.menuTableLogInfoWithoutBinary.CacheConnection = false;
            this.menuTableLogInfoWithoutBinary.CommandText = "select LOGID, MENUID, PACKAGE, PACKAGEDATE, LASTDATE, OWNER, OLDDATE from MENUTAB" +
    "LELOG";
            this.menuTableLogInfoWithoutBinary.CommandTimeout = 0;
            this.menuTableLogInfoWithoutBinary.CommandType = System.Data.CommandType.Text;
            this.menuTableLogInfoWithoutBinary.DynamicTableName = false;
            this.menuTableLogInfoWithoutBinary.EEPAlias = null;
            this.menuTableLogInfoWithoutBinary.EncodingAfter = null;
            this.menuTableLogInfoWithoutBinary.EncodingBefore = "Windows-1252";
            this.menuTableLogInfoWithoutBinary.InfoConnection = null;
            this.menuTableLogInfoWithoutBinary.MultiSetWhere = false;
            this.menuTableLogInfoWithoutBinary.Name = "menuTableLogInfoWithoutBinary";
            this.menuTableLogInfoWithoutBinary.NotificationAutoEnlist = false;
            this.menuTableLogInfoWithoutBinary.SecExcept = "";
            this.menuTableLogInfoWithoutBinary.SecFieldName = null;
            this.menuTableLogInfoWithoutBinary.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.menuTableLogInfoWithoutBinary.SelectPaging = false;
            this.menuTableLogInfoWithoutBinary.SelectTop = 0;
            this.menuTableLogInfoWithoutBinary.SiteControl = false;
            this.menuTableLogInfoWithoutBinary.SiteFieldName = null;
            this.menuTableLogInfoWithoutBinary.UpdatedRowSource = System.Data.UpdateRowSource.Both;
            // 
            // updateCompMenuTableLogWithoutBinary
            // 
            this.updateCompMenuTableLogWithoutBinary.AutoTrans = false;
            this.updateCompMenuTableLogWithoutBinary.ExceptJoin = false;
            this.updateCompMenuTableLogWithoutBinary.LogInfo = null;
            this.updateCompMenuTableLogWithoutBinary.Name = "updateCompMenuTableLogWithoutBinary";
            this.updateCompMenuTableLogWithoutBinary.RowAffectsCheck = true;
            this.updateCompMenuTableLogWithoutBinary.SelectCmd = this.menuTableLogInfoWithoutBinary;
            this.updateCompMenuTableLogWithoutBinary.SelectCmdForUpdate = null;
            this.updateCompMenuTableLogWithoutBinary.ServerModify = true;
            this.updateCompMenuTableLogWithoutBinary.ServerModifyGetMax = false;
            this.updateCompMenuTableLogWithoutBinary.TranscationScopeTimeOut = System.TimeSpan.Parse("00:02:00");
            this.updateCompMenuTableLogWithoutBinary.TransIsolationLevel = System.Data.IsolationLevel.ReadCommitted;
            this.updateCompMenuTableLogWithoutBinary.UseTranscationScope = false;
            this.updateCompMenuTableLogWithoutBinary.WhereMode = Srvtools.WhereModeType.FieldAttrs;
            // 
            // cmdDBTables
            // 
            this.cmdDBTables.CacheConnection = false;
            this.cmdDBTables.CommandText = "select name from sysobjects where (xtype=\'U\' or xtype =\'V\') order by name";
            this.cmdDBTables.CommandTimeout = 0;
            this.cmdDBTables.CommandType = System.Data.CommandType.Text;
            this.cmdDBTables.DynamicTableName = false;
            this.cmdDBTables.EEPAlias = null;
            this.cmdDBTables.EncodingAfter = null;
            this.cmdDBTables.EncodingBefore = "Windows-1252";
            this.cmdDBTables.InfoConnection = null;
            this.cmdDBTables.MultiSetWhere = false;
            this.cmdDBTables.Name = "cmdDBTables";
            this.cmdDBTables.NotificationAutoEnlist = false;
            this.cmdDBTables.SecExcept = "";
            this.cmdDBTables.SecFieldName = null;
            this.cmdDBTables.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.cmdDBTables.SelectPaging = false;
            this.cmdDBTables.SelectTop = 0;
            this.cmdDBTables.SiteControl = false;
            this.cmdDBTables.SiteFieldName = null;
            this.cmdDBTables.UpdatedRowSource = System.Data.UpdateRowSource.Both;
            this.cmdDBTables.BeforeExecuteSql += new Srvtools.SqlEventHandler(this.cmdDBTables_BeforeExecuteSql);
            // 
            // cmdColDEF
            // 
            this.cmdColDEF.CacheConnection = false;
            this.cmdColDEF.CommandText = "select distinct TABLE_NAME from COLDEF order by TABLE_NAME";
            this.cmdColDEF.CommandTimeout = 0;
            this.cmdColDEF.CommandType = System.Data.CommandType.Text;
            this.cmdColDEF.DynamicTableName = false;
            this.cmdColDEF.EEPAlias = null;
            this.cmdColDEF.EncodingAfter = null;
            this.cmdColDEF.EncodingBefore = "Windows-1252";
            this.cmdColDEF.InfoConnection = null;
            this.cmdColDEF.MultiSetWhere = false;
            this.cmdColDEF.Name = "cmdColDEF";
            this.cmdColDEF.NotificationAutoEnlist = false;
            this.cmdColDEF.SecExcept = "";
            this.cmdColDEF.SecFieldName = null;
            this.cmdColDEF.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.cmdColDEF.SelectPaging = false;
            this.cmdColDEF.SelectTop = 0;
            this.cmdColDEF.SiteControl = false;
            this.cmdColDEF.SiteFieldName = null;
            this.cmdColDEF.UpdatedRowSource = System.Data.UpdateRowSource.Both;
            // 
            // updateColDEF
            // 
            this.updateColDEF.AutoTrans = true;
            this.updateColDEF.ExceptJoin = false;
            this.updateColDEF.LogInfo = null;
            this.updateColDEF.Name = "updateColDEF";
            this.updateColDEF.RowAffectsCheck = true;
            this.updateColDEF.SelectCmd = this.cmdColDEF;
            this.updateColDEF.SelectCmdForUpdate = null;
            this.updateColDEF.ServerModify = true;
            this.updateColDEF.ServerModifyGetMax = false;
            this.updateColDEF.TranscationScopeTimeOut = System.TimeSpan.Parse("00:02:00");
            this.updateColDEF.TransIsolationLevel = System.Data.IsolationLevel.ReadCommitted;
            this.updateColDEF.UseTranscationScope = false;
            this.updateColDEF.WhereMode = Srvtools.WhereModeType.All;
            // 
            // cmdColDEF_Details
            // 
            this.cmdColDEF_Details.CacheConnection = false;
            this.cmdColDEF_Details.CommandText = "select * from COLDEF  ORDER BY SEQ";
            this.cmdColDEF_Details.CommandTimeout = 0;
            this.cmdColDEF_Details.CommandType = System.Data.CommandType.Text;
            this.cmdColDEF_Details.DynamicTableName = false;
            this.cmdColDEF_Details.EEPAlias = null;
            this.cmdColDEF_Details.EncodingAfter = null;
            this.cmdColDEF_Details.EncodingBefore = "Windows-1252";
            this.cmdColDEF_Details.InfoConnection = null;
            keyItem3.KeyName = "TABLE_NAME";
            keyItem4.KeyName = "FIELD_NAME";
            this.cmdColDEF_Details.KeyFields.Add(keyItem3);
            this.cmdColDEF_Details.KeyFields.Add(keyItem4);
            this.cmdColDEF_Details.MultiSetWhere = false;
            this.cmdColDEF_Details.Name = "cmdColDEF_Details";
            this.cmdColDEF_Details.NotificationAutoEnlist = false;
            this.cmdColDEF_Details.SecExcept = "";
            this.cmdColDEF_Details.SecFieldName = null;
            this.cmdColDEF_Details.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.cmdColDEF_Details.SelectPaging = false;
            this.cmdColDEF_Details.SelectTop = 0;
            this.cmdColDEF_Details.SiteControl = false;
            this.cmdColDEF_Details.SiteFieldName = null;
            this.cmdColDEF_Details.UpdatedRowSource = System.Data.UpdateRowSource.Both;
            // 
            // updateColDEF_Details
            // 
            this.updateColDEF_Details.AutoTrans = true;
            this.updateColDEF_Details.ExceptJoin = false;
            this.updateColDEF_Details.LogInfo = null;
            this.updateColDEF_Details.Name = "updateColDEF_Details";
            this.updateColDEF_Details.RowAffectsCheck = true;
            this.updateColDEF_Details.SelectCmd = this.cmdColDEF_Details;
            this.updateColDEF_Details.SelectCmdForUpdate = null;
            this.updateColDEF_Details.ServerModify = true;
            this.updateColDEF_Details.ServerModifyGetMax = false;
            this.updateColDEF_Details.TranscationScopeTimeOut = System.TimeSpan.Parse("00:02:00");
            this.updateColDEF_Details.TransIsolationLevel = System.Data.IsolationLevel.ReadCommitted;
            this.updateColDEF_Details.UseTranscationScope = false;
            this.updateColDEF_Details.WhereMode = Srvtools.WhereModeType.All;
            // 
            // cmdRefValUse
            // 
            this.cmdRefValUse.CacheConnection = false;
            this.cmdRefValUse.CommandText = "";
            this.cmdRefValUse.CommandTimeout = 0;
            this.cmdRefValUse.CommandType = System.Data.CommandType.Text;
            this.cmdRefValUse.DynamicTableName = false;
            this.cmdRefValUse.EEPAlias = null;
            this.cmdRefValUse.EncodingAfter = null;
            this.cmdRefValUse.EncodingBefore = "Windows-1252";
            this.cmdRefValUse.InfoConnection = null;
            this.cmdRefValUse.MultiSetWhere = false;
            this.cmdRefValUse.Name = "cmdRefValUse";
            this.cmdRefValUse.NotificationAutoEnlist = false;
            this.cmdRefValUse.SecExcept = "";
            this.cmdRefValUse.SecFieldName = null;
            this.cmdRefValUse.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.cmdRefValUse.SelectPaging = false;
            this.cmdRefValUse.SelectTop = 100;
            this.cmdRefValUse.SiteControl = false;
            this.cmdRefValUse.SiteFieldName = null;
            this.cmdRefValUse.UpdatedRowSource = System.Data.UpdateRowSource.Both;
            // 
            // cmdERRLOG
            // 
            this.cmdERRLOG.CacheConnection = false;
            this.cmdERRLOG.CommandText = "select * from SYSERRLOG";
            this.cmdERRLOG.CommandTimeout = 0;
            this.cmdERRLOG.CommandType = System.Data.CommandType.Text;
            this.cmdERRLOG.DynamicTableName = false;
            this.cmdERRLOG.EEPAlias = null;
            this.cmdERRLOG.EncodingAfter = null;
            this.cmdERRLOG.EncodingBefore = "Windows-1252";
            this.cmdERRLOG.InfoConnection = null;
            keyItem5.KeyName = "ERRID";
            this.cmdERRLOG.KeyFields.Add(keyItem5);
            this.cmdERRLOG.MultiSetWhere = false;
            this.cmdERRLOG.Name = "cmdERRLOG";
            this.cmdERRLOG.NotificationAutoEnlist = false;
            this.cmdERRLOG.SecExcept = "";
            this.cmdERRLOG.SecFieldName = null;
            this.cmdERRLOG.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.cmdERRLOG.SelectPaging = false;
            this.cmdERRLOG.SelectTop = 0;
            this.cmdERRLOG.SiteControl = false;
            this.cmdERRLOG.SiteFieldName = null;
            this.cmdERRLOG.UpdatedRowSource = System.Data.UpdateRowSource.Both;
            // 
            // updateERRLOG
            // 
            this.updateERRLOG.AutoTrans = true;
            this.updateERRLOG.ExceptJoin = false;
            fieldAttr1.CheckNull = false;
            fieldAttr1.DataField = "ERRID";
            fieldAttr1.DefaultMode = Srvtools.DefaultModeType.Update;
            fieldAttr1.DefaultValue = null;
            fieldAttr1.TrimLength = 0;
            fieldAttr1.UpdateEnable = true;
            fieldAttr1.WhereMode = true;
            fieldAttr2.CheckNull = false;
            fieldAttr2.DataField = "ERRSCREEN";
            fieldAttr2.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr2.DefaultValue = null;
            fieldAttr2.TrimLength = 0;
            fieldAttr2.UpdateEnable = false;
            fieldAttr2.WhereMode = false;
            this.updateERRLOG.FieldAttrs.Add(fieldAttr1);
            this.updateERRLOG.FieldAttrs.Add(fieldAttr2);
            this.updateERRLOG.LogInfo = null;
            this.updateERRLOG.Name = "updateERRLOG";
            this.updateERRLOG.RowAffectsCheck = true;
            this.updateERRLOG.SelectCmd = this.cmdERRLOG;
            this.updateERRLOG.SelectCmdForUpdate = null;
            this.updateERRLOG.ServerModify = true;
            this.updateERRLOG.ServerModifyGetMax = false;
            this.updateERRLOG.TranscationScopeTimeOut = System.TimeSpan.Parse("00:02:00");
            this.updateERRLOG.TransIsolationLevel = System.Data.IsolationLevel.ReadCommitted;
            this.updateERRLOG.UseTranscationScope = false;
            this.updateERRLOG.WhereMode = Srvtools.WhereModeType.FieldAttrs;
            // 
            // cmdSYSEEPLOGforDB
            // 
            this.cmdSYSEEPLOGforDB.CacheConnection = false;
            this.cmdSYSEEPLOGforDB.CommandText = "select * from SYSEEPLOG";
            this.cmdSYSEEPLOGforDB.CommandTimeout = 0;
            this.cmdSYSEEPLOGforDB.CommandType = System.Data.CommandType.Text;
            this.cmdSYSEEPLOGforDB.DynamicTableName = false;
            this.cmdSYSEEPLOGforDB.EEPAlias = null;
            this.cmdSYSEEPLOGforDB.EncodingAfter = null;
            this.cmdSYSEEPLOGforDB.EncodingBefore = "Windows-1252";
            this.cmdSYSEEPLOGforDB.InfoConnection = null;
            keyItem6.KeyName = "LOGID";
            this.cmdSYSEEPLOGforDB.KeyFields.Add(keyItem6);
            this.cmdSYSEEPLOGforDB.MultiSetWhere = false;
            this.cmdSYSEEPLOGforDB.Name = "cmdSYSEEPLOGforDB";
            this.cmdSYSEEPLOGforDB.NotificationAutoEnlist = false;
            this.cmdSYSEEPLOGforDB.SecExcept = "";
            this.cmdSYSEEPLOGforDB.SecFieldName = null;
            this.cmdSYSEEPLOGforDB.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.cmdSYSEEPLOGforDB.SelectPaging = false;
            this.cmdSYSEEPLOGforDB.SelectTop = 0;
            this.cmdSYSEEPLOGforDB.SiteControl = false;
            this.cmdSYSEEPLOGforDB.SiteFieldName = null;
            this.cmdSYSEEPLOGforDB.UpdatedRowSource = System.Data.UpdateRowSource.Both;
            // 
            // cmdDDUse
            // 
            this.cmdDDUse.CacheConnection = false;
            this.cmdDDUse.CommandText = "";
            this.cmdDDUse.CommandTimeout = 0;
            this.cmdDDUse.CommandType = System.Data.CommandType.Text;
            this.cmdDDUse.DynamicTableName = false;
            this.cmdDDUse.EEPAlias = null;
            this.cmdDDUse.EncodingAfter = null;
            this.cmdDDUse.EncodingBefore = "Windows-1252";
            this.cmdDDUse.InfoConnection = null;
            this.cmdDDUse.MultiSetWhere = false;
            this.cmdDDUse.Name = "cmdDDUse";
            this.cmdDDUse.NotificationAutoEnlist = false;
            this.cmdDDUse.SecExcept = "";
            this.cmdDDUse.SecFieldName = null;
            this.cmdDDUse.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.cmdDDUse.SelectPaging = false;
            this.cmdDDUse.SelectTop = 0;
            this.cmdDDUse.SiteControl = false;
            this.cmdDDUse.SiteFieldName = null;
            this.cmdDDUse.UpdatedRowSource = System.Data.UpdateRowSource.Both;
            // 
            // updateCompMGroupMenus
            // 
            this.updateCompMGroupMenus.AutoTrans = true;
            this.updateCompMGroupMenus.ExceptJoin = false;
            this.updateCompMGroupMenus.LogInfo = null;
            this.updateCompMGroupMenus.Name = "updateCompMGroupMenus";
            this.updateCompMGroupMenus.RowAffectsCheck = true;
            this.updateCompMGroupMenus.SelectCmd = this.sqlMGroupMenus;
            this.updateCompMGroupMenus.SelectCmdForUpdate = null;
            this.updateCompMGroupMenus.ServerModify = true;
            this.updateCompMGroupMenus.ServerModifyGetMax = false;
            this.updateCompMGroupMenus.TranscationScopeTimeOut = System.TimeSpan.Parse("00:02:00");
            this.updateCompMGroupMenus.TransIsolationLevel = System.Data.IsolationLevel.ReadCommitted;
            this.updateCompMGroupMenus.UseTranscationScope = false;
            this.updateCompMGroupMenus.WhereMode = Srvtools.WhereModeType.All;
            // 
            // userMenus
            // 
            this.userMenus.CacheConnection = false;
            this.userMenus.CommandText = "select * from USERMENUS";
            this.userMenus.CommandTimeout = 0;
            this.userMenus.CommandType = System.Data.CommandType.Text;
            this.userMenus.DynamicTableName = false;
            this.userMenus.EEPAlias = null;
            this.userMenus.EncodingAfter = null;
            this.userMenus.EncodingBefore = "Windows-1252";
            this.userMenus.InfoConnection = null;
            this.userMenus.MultiSetWhere = false;
            this.userMenus.Name = "userMenus";
            this.userMenus.NotificationAutoEnlist = false;
            this.userMenus.SecExcept = "";
            this.userMenus.SecFieldName = null;
            this.userMenus.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.userMenus.SelectPaging = false;
            this.userMenus.SelectTop = 0;
            this.userMenus.SiteControl = false;
            this.userMenus.SiteFieldName = null;
            this.userMenus.UpdatedRowSource = System.Data.UpdateRowSource.Both;
            // 
            // updateCompUserMenus
            // 
            this.updateCompUserMenus.AutoTrans = true;
            this.updateCompUserMenus.ExceptJoin = false;
            this.updateCompUserMenus.LogInfo = null;
            this.updateCompUserMenus.Name = "updateCompUserMenus";
            this.updateCompUserMenus.RowAffectsCheck = true;
            this.updateCompUserMenus.SelectCmd = this.userMenus;
            this.updateCompUserMenus.SelectCmdForUpdate = null;
            this.updateCompUserMenus.ServerModify = true;
            this.updateCompUserMenus.ServerModifyGetMax = false;
            this.updateCompUserMenus.TranscationScopeTimeOut = System.TimeSpan.Parse("00:02:00");
            this.updateCompUserMenus.TransIsolationLevel = System.Data.IsolationLevel.ReadCommitted;
            this.updateCompUserMenus.UseTranscationScope = false;
            this.updateCompUserMenus.WhereMode = Srvtools.WhereModeType.All;
            // 
            // packageversion
            // 
            this.packageversion.CacheConnection = false;
            this.packageversion.CommandText = "SELECT LOGID, ITEMTYPE, PACKAGE, PACKAGEDATE, FILETYPE, FILENAME, FILEDATE FROM M" +
    "ENUCHECKLOG ORDER BY PACKAGEDATE DESC";
            this.packageversion.CommandTimeout = 0;
            this.packageversion.CommandType = System.Data.CommandType.Text;
            this.packageversion.DynamicTableName = false;
            this.packageversion.EEPAlias = null;
            this.packageversion.EncodingAfter = null;
            this.packageversion.EncodingBefore = "Windows-1252";
            this.packageversion.InfoConnection = null;
            this.packageversion.MultiSetWhere = false;
            this.packageversion.Name = "packageversion";
            this.packageversion.NotificationAutoEnlist = false;
            this.packageversion.SecExcept = "";
            this.packageversion.SecFieldName = null;
            this.packageversion.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.packageversion.SelectPaging = false;
            this.packageversion.SelectTop = 0;
            this.packageversion.SiteControl = false;
            this.packageversion.SiteFieldName = null;
            this.packageversion.UpdatedRowSource = System.Data.UpdateRowSource.Both;
            // 
            // cmdSysRefVal
            // 
            this.cmdSysRefVal.CacheConnection = false;
            this.cmdSysRefVal.CommandText = "select * from SYS_REFVAL";
            this.cmdSysRefVal.CommandTimeout = 0;
            this.cmdSysRefVal.CommandType = System.Data.CommandType.Text;
            this.cmdSysRefVal.DynamicTableName = false;
            this.cmdSysRefVal.EEPAlias = null;
            this.cmdSysRefVal.EncodingAfter = null;
            this.cmdSysRefVal.EncodingBefore = "Windows-1252";
            this.cmdSysRefVal.InfoConnection = null;
            keyItem7.KeyName = "REFVAL_NO";
            this.cmdSysRefVal.KeyFields.Add(keyItem7);
            this.cmdSysRefVal.MultiSetWhere = false;
            this.cmdSysRefVal.Name = "cmdSysRefVal";
            this.cmdSysRefVal.NotificationAutoEnlist = false;
            this.cmdSysRefVal.SecExcept = "";
            this.cmdSysRefVal.SecFieldName = null;
            this.cmdSysRefVal.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.cmdSysRefVal.SelectPaging = false;
            this.cmdSysRefVal.SelectTop = 0;
            this.cmdSysRefVal.SiteControl = false;
            this.cmdSysRefVal.SiteFieldName = null;
            this.cmdSysRefVal.UpdatedRowSource = System.Data.UpdateRowSource.Both;
            // 
            // cmdSysRefVal_D
            // 
            this.cmdSysRefVal_D.CacheConnection = false;
            this.cmdSysRefVal_D.CommandText = "Select * from SYS_REFVAL_D1";
            this.cmdSysRefVal_D.CommandTimeout = 0;
            this.cmdSysRefVal_D.CommandType = System.Data.CommandType.Text;
            this.cmdSysRefVal_D.DynamicTableName = false;
            this.cmdSysRefVal_D.EEPAlias = null;
            this.cmdSysRefVal_D.EncodingAfter = null;
            this.cmdSysRefVal_D.EncodingBefore = "Windows-1252";
            this.cmdSysRefVal_D.InfoConnection = null;
            keyItem8.KeyName = "REFVAL_NO";
            keyItem9.KeyName = "FIELD_NAME";
            this.cmdSysRefVal_D.KeyFields.Add(keyItem8);
            this.cmdSysRefVal_D.KeyFields.Add(keyItem9);
            this.cmdSysRefVal_D.MultiSetWhere = false;
            this.cmdSysRefVal_D.Name = "cmdSysRefVal_D";
            this.cmdSysRefVal_D.NotificationAutoEnlist = false;
            this.cmdSysRefVal_D.SecExcept = "";
            this.cmdSysRefVal_D.SecFieldName = null;
            this.cmdSysRefVal_D.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.cmdSysRefVal_D.SelectPaging = false;
            this.cmdSysRefVal_D.SelectTop = 0;
            this.cmdSysRefVal_D.SiteControl = false;
            this.cmdSysRefVal_D.SiteFieldName = null;
            this.cmdSysRefVal_D.UpdatedRowSource = System.Data.UpdateRowSource.Both;
            // 
            // updateCompSysRefVal
            // 
            this.updateCompSysRefVal.AutoTrans = true;
            this.updateCompSysRefVal.ExceptJoin = false;
            this.updateCompSysRefVal.LogInfo = null;
            this.updateCompSysRefVal.Name = "updateCompSysRefVal";
            this.updateCompSysRefVal.RowAffectsCheck = true;
            this.updateCompSysRefVal.SelectCmd = this.cmdSysRefVal;
            this.updateCompSysRefVal.SelectCmdForUpdate = null;
            this.updateCompSysRefVal.ServerModify = true;
            this.updateCompSysRefVal.ServerModifyGetMax = false;
            this.updateCompSysRefVal.TranscationScopeTimeOut = System.TimeSpan.Parse("00:02:00");
            this.updateCompSysRefVal.TransIsolationLevel = System.Data.IsolationLevel.ReadCommitted;
            this.updateCompSysRefVal.UseTranscationScope = false;
            this.updateCompSysRefVal.WhereMode = Srvtools.WhereModeType.Keyfields;
            // 
            // updateCompSysRefVal_D
            // 
            this.updateCompSysRefVal_D.AutoTrans = true;
            this.updateCompSysRefVal_D.ExceptJoin = false;
            this.updateCompSysRefVal_D.LogInfo = null;
            this.updateCompSysRefVal_D.Name = "updateCompSysRefVal_D";
            this.updateCompSysRefVal_D.RowAffectsCheck = true;
            this.updateCompSysRefVal_D.SelectCmd = this.cmdSysRefVal_D;
            this.updateCompSysRefVal_D.SelectCmdForUpdate = null;
            this.updateCompSysRefVal_D.ServerModify = true;
            this.updateCompSysRefVal_D.ServerModifyGetMax = false;
            this.updateCompSysRefVal_D.TranscationScopeTimeOut = System.TimeSpan.Parse("00:02:00");
            this.updateCompSysRefVal_D.TransIsolationLevel = System.Data.IsolationLevel.ReadCommitted;
            this.updateCompSysRefVal_D.UseTranscationScope = false;
            this.updateCompSysRefVal_D.WhereMode = Srvtools.WhereModeType.Keyfields;
            // 
            // idsRefVal
            // 
            this.idsRefVal.Detail = this.cmdSysRefVal_D;
            columnItem5.FieldName = "REFVAL_NO";
            this.idsRefVal.DetailColumns.Add(columnItem5);
            this.idsRefVal.DynamicTableName = false;
            this.idsRefVal.Master = this.cmdSysRefVal;
            columnItem6.FieldName = "REFVAL_NO";
            this.idsRefVal.MasterColumns.Add(columnItem6);
            // 
            // cmdToDoList
            // 
            this.cmdToDoList.CacheConnection = false;
            this.cmdToDoList.CommandText = "";
            this.cmdToDoList.CommandTimeout = 0;
            this.cmdToDoList.CommandType = System.Data.CommandType.Text;
            this.cmdToDoList.DynamicTableName = false;
            this.cmdToDoList.EEPAlias = "";
            this.cmdToDoList.EncodingAfter = null;
            this.cmdToDoList.EncodingBefore = "Windows-1252";
            this.cmdToDoList.InfoConnection = null;
            this.cmdToDoList.MultiSetWhere = false;
            this.cmdToDoList.Name = "cmdToDoList";
            this.cmdToDoList.NotificationAutoEnlist = false;
            this.cmdToDoList.SecExcept = "";
            this.cmdToDoList.SecFieldName = null;
            this.cmdToDoList.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.cmdToDoList.SelectPaging = false;
            this.cmdToDoList.SelectTop = 0;
            this.cmdToDoList.SiteControl = false;
            this.cmdToDoList.SiteFieldName = null;
            this.cmdToDoList.UpdatedRowSource = System.Data.UpdateRowSource.Both;
            // 
            // cmdToDoHis
            // 
            this.cmdToDoHis.CacheConnection = false;
            this.cmdToDoHis.CommandText = "";
            this.cmdToDoHis.CommandTimeout = 0;
            this.cmdToDoHis.CommandType = System.Data.CommandType.Text;
            this.cmdToDoHis.DynamicTableName = false;
            this.cmdToDoHis.EEPAlias = null;
            this.cmdToDoHis.EncodingAfter = null;
            this.cmdToDoHis.EncodingBefore = "Windows-1252";
            this.cmdToDoHis.InfoConnection = null;
            this.cmdToDoHis.MultiSetWhere = false;
            this.cmdToDoHis.Name = "cmdToDoHis";
            this.cmdToDoHis.NotificationAutoEnlist = false;
            this.cmdToDoHis.SecExcept = "";
            this.cmdToDoHis.SecFieldName = null;
            this.cmdToDoHis.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.cmdToDoHis.SelectPaging = false;
            this.cmdToDoHis.SelectTop = 0;
            this.cmdToDoHis.SiteControl = false;
            this.cmdToDoHis.SiteFieldName = null;
            this.cmdToDoHis.UpdatedRowSource = System.Data.UpdateRowSource.Both;
            // 
            // cmdRoles
            // 
            this.cmdRoles.CacheConnection = false;
            this.cmdRoles.CommandText = "select * from GROUPS where ISROLE=\'Y\'";
            this.cmdRoles.CommandTimeout = 0;
            this.cmdRoles.CommandType = System.Data.CommandType.Text;
            this.cmdRoles.DynamicTableName = false;
            this.cmdRoles.EEPAlias = null;
            this.cmdRoles.EncodingAfter = null;
            this.cmdRoles.EncodingBefore = "Windows-1252";
            this.cmdRoles.InfoConnection = null;
            this.cmdRoles.MultiSetWhere = false;
            this.cmdRoles.Name = "cmdRoles";
            this.cmdRoles.NotificationAutoEnlist = false;
            this.cmdRoles.SecExcept = "";
            this.cmdRoles.SecFieldName = null;
            this.cmdRoles.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.cmdRoles.SelectPaging = false;
            this.cmdRoles.SelectTop = 0;
            this.cmdRoles.SiteControl = false;
            this.cmdRoles.SiteFieldName = null;
            this.cmdRoles.UpdatedRowSource = System.Data.UpdateRowSource.Both;
            // 
            // cmdOrgRoles
            // 
            this.cmdOrgRoles.CacheConnection = false;
            this.cmdOrgRoles.CommandText = "SELECT SYS_ORGROLES.*,GROUPS.GROUPNAME FROM SYS_ORGROLES\r\n LEFT JOIN GROUPS ON SY" +
    "S_ORGROLES.ROLE_ID=GROUPS.GROUPID";
            this.cmdOrgRoles.CommandTimeout = 0;
            this.cmdOrgRoles.CommandType = System.Data.CommandType.Text;
            this.cmdOrgRoles.DynamicTableName = false;
            this.cmdOrgRoles.EEPAlias = "";
            this.cmdOrgRoles.EncodingAfter = null;
            this.cmdOrgRoles.EncodingBefore = "Windows-1252";
            this.cmdOrgRoles.InfoConnection = null;
            keyItem10.KeyName = "ORG_NO";
            keyItem11.KeyName = "ROLE_ID";
            this.cmdOrgRoles.KeyFields.Add(keyItem10);
            this.cmdOrgRoles.KeyFields.Add(keyItem11);
            this.cmdOrgRoles.MultiSetWhere = false;
            this.cmdOrgRoles.Name = "cmdOrgRoles";
            this.cmdOrgRoles.NotificationAutoEnlist = false;
            this.cmdOrgRoles.SecExcept = "";
            this.cmdOrgRoles.SecFieldName = null;
            this.cmdOrgRoles.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.cmdOrgRoles.SelectPaging = false;
            this.cmdOrgRoles.SelectTop = 0;
            this.cmdOrgRoles.SiteControl = false;
            this.cmdOrgRoles.SiteFieldName = null;
            this.cmdOrgRoles.UpdatedRowSource = System.Data.UpdateRowSource.Both;
            // 
            // ucOrgRoles
            // 
            this.ucOrgRoles.AutoTrans = false;
            this.ucOrgRoles.ExceptJoin = false;
            fieldAttr3.CheckNull = false;
            fieldAttr3.DataField = "GROUPNAME";
            fieldAttr3.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr3.DefaultValue = null;
            fieldAttr3.TrimLength = 0;
            fieldAttr3.UpdateEnable = false;
            fieldAttr3.WhereMode = true;
            this.ucOrgRoles.FieldAttrs.Add(fieldAttr3);
            this.ucOrgRoles.LogInfo = null;
            this.ucOrgRoles.Name = "ucOrgRoles";
            this.ucOrgRoles.RowAffectsCheck = true;
            this.ucOrgRoles.SelectCmd = this.cmdOrgRoles;
            this.ucOrgRoles.SelectCmdForUpdate = null;
            this.ucOrgRoles.ServerModify = false;
            this.ucOrgRoles.ServerModifyGetMax = false;
            this.ucOrgRoles.TranscationScopeTimeOut = System.TimeSpan.Parse("00:02:00");
            this.ucOrgRoles.TransIsolationLevel = System.Data.IsolationLevel.ReadCommitted;
            this.ucOrgRoles.UseTranscationScope = false;
            this.ucOrgRoles.WhereMode = Srvtools.WhereModeType.Keyfields;
            // 
            // cmdOrgLevel
            // 
            this.cmdOrgLevel.CacheConnection = false;
            this.cmdOrgLevel.CommandText = "SELECT * FROM SYS_ORGLEVEL";
            this.cmdOrgLevel.CommandTimeout = 0;
            this.cmdOrgLevel.CommandType = System.Data.CommandType.Text;
            this.cmdOrgLevel.DynamicTableName = false;
            this.cmdOrgLevel.EEPAlias = "";
            this.cmdOrgLevel.EncodingAfter = null;
            this.cmdOrgLevel.EncodingBefore = "Windows-1252";
            this.cmdOrgLevel.InfoConnection = null;
            keyItem12.KeyName = "LEVEL_NO";
            this.cmdOrgLevel.KeyFields.Add(keyItem12);
            this.cmdOrgLevel.MultiSetWhere = false;
            this.cmdOrgLevel.Name = "cmdOrgLevel";
            this.cmdOrgLevel.NotificationAutoEnlist = false;
            this.cmdOrgLevel.SecExcept = "";
            this.cmdOrgLevel.SecFieldName = null;
            this.cmdOrgLevel.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.cmdOrgLevel.SelectPaging = false;
            this.cmdOrgLevel.SelectTop = 0;
            this.cmdOrgLevel.SiteControl = false;
            this.cmdOrgLevel.SiteFieldName = null;
            this.cmdOrgLevel.UpdatedRowSource = System.Data.UpdateRowSource.Both;
            // 
            // ucOrgLevel
            // 
            this.ucOrgLevel.AutoTrans = false;
            this.ucOrgLevel.ExceptJoin = false;
            this.ucOrgLevel.LogInfo = null;
            this.ucOrgLevel.Name = "ucOrgLevel";
            this.ucOrgLevel.RowAffectsCheck = true;
            this.ucOrgLevel.SelectCmd = this.cmdOrgLevel;
            this.ucOrgLevel.SelectCmdForUpdate = null;
            this.ucOrgLevel.ServerModify = false;
            this.ucOrgLevel.ServerModifyGetMax = false;
            this.ucOrgLevel.TranscationScopeTimeOut = System.TimeSpan.Parse("00:02:00");
            this.ucOrgLevel.TransIsolationLevel = System.Data.IsolationLevel.ReadCommitted;
            this.ucOrgLevel.UseTranscationScope = false;
            this.ucOrgLevel.WhereMode = Srvtools.WhereModeType.All;
            // 
            // cmdOrgKind
            // 
            this.cmdOrgKind.CacheConnection = false;
            this.cmdOrgKind.CommandText = "SELECT * FROM SYS_ORGKIND";
            this.cmdOrgKind.CommandTimeout = 0;
            this.cmdOrgKind.CommandType = System.Data.CommandType.Text;
            this.cmdOrgKind.DynamicTableName = false;
            this.cmdOrgKind.EEPAlias = "";
            this.cmdOrgKind.EncodingAfter = null;
            this.cmdOrgKind.EncodingBefore = "Windows-1252";
            this.cmdOrgKind.InfoConnection = null;
            keyItem13.KeyName = "ORG_KIND";
            this.cmdOrgKind.KeyFields.Add(keyItem13);
            this.cmdOrgKind.MultiSetWhere = false;
            this.cmdOrgKind.Name = "cmdOrgKind";
            this.cmdOrgKind.NotificationAutoEnlist = false;
            this.cmdOrgKind.SecExcept = "";
            this.cmdOrgKind.SecFieldName = null;
            this.cmdOrgKind.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.cmdOrgKind.SelectPaging = false;
            this.cmdOrgKind.SelectTop = 0;
            this.cmdOrgKind.SiteControl = false;
            this.cmdOrgKind.SiteFieldName = null;
            this.cmdOrgKind.UpdatedRowSource = System.Data.UpdateRowSource.Both;
            // 
            // ucOrgKind
            // 
            this.ucOrgKind.AutoTrans = false;
            this.ucOrgKind.ExceptJoin = false;
            this.ucOrgKind.LogInfo = null;
            this.ucOrgKind.Name = "ucOrgKind";
            this.ucOrgKind.RowAffectsCheck = true;
            this.ucOrgKind.SelectCmd = this.cmdOrgKind;
            this.ucOrgKind.SelectCmdForUpdate = null;
            this.ucOrgKind.ServerModify = false;
            this.ucOrgKind.ServerModifyGetMax = false;
            this.ucOrgKind.TranscationScopeTimeOut = System.TimeSpan.Parse("00:02:00");
            this.ucOrgKind.TransIsolationLevel = System.Data.IsolationLevel.ReadCommitted;
            this.ucOrgKind.UseTranscationScope = false;
            this.ucOrgKind.WhereMode = Srvtools.WhereModeType.All;
            // 
            // cmdWorkflow
            // 
            this.cmdWorkflow.CacheConnection = false;
            this.cmdWorkflow.CommandText = "";
            this.cmdWorkflow.CommandTimeout = 0;
            this.cmdWorkflow.CommandType = System.Data.CommandType.Text;
            this.cmdWorkflow.DynamicTableName = false;
            this.cmdWorkflow.EEPAlias = null;
            this.cmdWorkflow.EncodingAfter = null;
            this.cmdWorkflow.EncodingBefore = "Windows-1252";
            this.cmdWorkflow.InfoConnection = null;
            this.cmdWorkflow.MultiSetWhere = false;
            this.cmdWorkflow.Name = "cmdWorkflow";
            this.cmdWorkflow.NotificationAutoEnlist = false;
            this.cmdWorkflow.SecExcept = "";
            this.cmdWorkflow.SecFieldName = null;
            this.cmdWorkflow.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.cmdWorkflow.SelectPaging = false;
            this.cmdWorkflow.SelectTop = 0;
            this.cmdWorkflow.SiteControl = false;
            this.cmdWorkflow.SiteFieldName = null;
            this.cmdWorkflow.UpdatedRowSource = System.Data.UpdateRowSource.Both;
            // 
            // cmdRoleAgent
            // 
            this.cmdRoleAgent.CacheConnection = false;
            this.cmdRoleAgent.CommandText = "select * from SYS_ROLES_AGENT";
            this.cmdRoleAgent.CommandTimeout = 0;
            this.cmdRoleAgent.CommandType = System.Data.CommandType.Text;
            this.cmdRoleAgent.DynamicTableName = false;
            this.cmdRoleAgent.EEPAlias = null;
            this.cmdRoleAgent.EncodingAfter = null;
            this.cmdRoleAgent.EncodingBefore = "Windows-1252";
            this.cmdRoleAgent.InfoConnection = null;
            keyItem14.KeyName = "ROLE_ID";
            keyItem15.KeyName = "AGENT";
            keyItem16.KeyName = "FLOW_DESC";
            this.cmdRoleAgent.KeyFields.Add(keyItem14);
            this.cmdRoleAgent.KeyFields.Add(keyItem15);
            this.cmdRoleAgent.KeyFields.Add(keyItem16);
            this.cmdRoleAgent.MultiSetWhere = false;
            this.cmdRoleAgent.Name = "cmdRoleAgent";
            this.cmdRoleAgent.NotificationAutoEnlist = false;
            this.cmdRoleAgent.SecExcept = "";
            this.cmdRoleAgent.SecFieldName = null;
            this.cmdRoleAgent.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.cmdRoleAgent.SelectPaging = false;
            this.cmdRoleAgent.SelectTop = 0;
            this.cmdRoleAgent.SiteControl = false;
            this.cmdRoleAgent.SiteFieldName = null;
            this.cmdRoleAgent.UpdatedRowSource = System.Data.UpdateRowSource.Both;
            // 
            // ucRoleAgent
            // 
            this.ucRoleAgent.AutoTrans = true;
            this.ucRoleAgent.ExceptJoin = false;
            this.ucRoleAgent.LogInfo = null;
            this.ucRoleAgent.Name = "ucRoleAgent";
            this.ucRoleAgent.RowAffectsCheck = true;
            this.ucRoleAgent.SelectCmd = this.cmdRoleAgent;
            this.ucRoleAgent.SelectCmdForUpdate = null;
            this.ucRoleAgent.ServerModify = true;
            this.ucRoleAgent.ServerModifyGetMax = false;
            this.ucRoleAgent.TranscationScopeTimeOut = System.TimeSpan.Parse("00:02:00");
            this.ucRoleAgent.TransIsolationLevel = System.Data.IsolationLevel.ReadCommitted;
            this.ucRoleAgent.UseTranscationScope = false;
            this.ucRoleAgent.WhereMode = Srvtools.WhereModeType.All;
            // 
            // cmdSlSource
            // 
            this.cmdSlSource.CacheConnection = false;
            this.cmdSlSource.CommandText = "";
            this.cmdSlSource.CommandTimeout = 0;
            this.cmdSlSource.CommandType = System.Data.CommandType.Text;
            this.cmdSlSource.DynamicTableName = false;
            this.cmdSlSource.EEPAlias = null;
            this.cmdSlSource.EncodingAfter = null;
            this.cmdSlSource.EncodingBefore = "Windows-1252";
            this.cmdSlSource.InfoConnection = null;
            this.cmdSlSource.MultiSetWhere = false;
            this.cmdSlSource.Name = "cmdSlSource";
            this.cmdSlSource.NotificationAutoEnlist = false;
            this.cmdSlSource.SecExcept = "";
            this.cmdSlSource.SecFieldName = null;
            this.cmdSlSource.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.cmdSlSource.SelectPaging = false;
            this.cmdSlSource.SelectTop = 0;
            this.cmdSlSource.SiteControl = false;
            this.cmdSlSource.SiteFieldName = null;
            this.cmdSlSource.UpdatedRowSource = System.Data.UpdateRowSource.Both;
            // 
            // serviceManager1
            // 
            service1.DelegateName = "LogOut";
            service1.NonLogin = false;
            service1.ServiceName = "LogOut";
            service2.DelegateName = "CheckUser";
            service2.NonLogin = false;
            service2.ServiceName = "CheckUser";
            service3.DelegateName = "GetUG";
            service3.NonLogin = false;
            service3.ServiceName = "GetUG";
            service4.DelegateName = "FetchMenus";
            service4.NonLogin = false;
            service4.ServiceName = "FetchMenus";
            service5.DelegateName = "GetParam";
            service5.NonLogin = false;
            service5.ServiceName = "GetParam";
            service6.DelegateName = "OPMenu";
            service6.NonLogin = false;
            service6.ServiceName = "OPMenu";
            service7.DelegateName = "LoadGroups";
            service7.NonLogin = false;
            service7.ServiceName = "LoadGroups";
            service8.DelegateName = "setGroups";
            service8.NonLogin = false;
            service8.ServiceName = "setGroups";
            service9.DelegateName = "UpdateNodes";
            service9.NonLogin = false;
            service9.ServiceName = "UpdateNodes";
            service10.DelegateName = "CheckAndDownLoad";
            service10.NonLogin = false;
            service10.ServiceName = "CheckAndDownLoad";
            service11.DelegateName = "LogError";
            service11.NonLogin = false;
            service11.ServiceName = "LogError";
            service12.DelegateName = "GetSolution";
            service12.NonLogin = false;
            service12.ServiceName = "GetSolution";
            service13.DelegateName = "AutoSeqMenuID";
            service13.NonLogin = false;
            service13.ServiceName = "AutoSeqMenuID";
            service14.DelegateName = "SendMessage";
            service14.NonLogin = false;
            service14.ServiceName = "SendMessage";
            service15.DelegateName = "GetMessage";
            service15.NonLogin = false;
            service15.ServiceName = "GetMessage";
            service16.DelegateName = "GetDataDic";
            service16.NonLogin = false;
            service16.ServiceName = "GetDataDic";
            service17.DelegateName = "GetDDColumns";
            service17.NonLogin = false;
            service17.ServiceName = "GetDDColumns";
            service18.DelegateName = "DeleteDDColumns";
            service18.NonLogin = false;
            service18.ServiceName = "DeleteDDColumns";
            service19.DelegateName = "DataDRefresh";
            service19.NonLogin = false;
            service19.ServiceName = "DataDRefresh";
            service20.DelegateName = "GetDB";
            service20.NonLogin = false;
            service20.ServiceName = "GetDB";
            service21.DelegateName = "GetUserName";
            service21.NonLogin = false;
            service21.ServiceName = "GetUserName";
            service22.DelegateName = "GetMenu";
            service22.NonLogin = false;
            service22.ServiceName = "GetMenu";
            service23.DelegateName = "GetMenuID";
            service23.NonLogin = false;
            service23.ServiceName = "GetMenuID";
            service24.DelegateName = "InsertToMenu";
            service24.NonLogin = false;
            service24.ServiceName = "InsertToMenu";
            service25.DelegateName = "GetServerTime";
            service25.NonLogin = false;
            service25.ServiceName = "GetServerTime";
            service26.DelegateName = "GetGroup";
            service26.NonLogin = false;
            service26.ServiceName = "GetGroup";
            service27.DelegateName = "InsertToGroup";
            service27.NonLogin = false;
            service27.ServiceName = "InsertToGroup";
            service28.DelegateName = "GetNewGroup";
            service28.NonLogin = false;
            service28.ServiceName = "GetNewGroup";
            service29.DelegateName = "UpdateGroup";
            service29.NonLogin = false;
            service29.ServiceName = "UpdateGroup";
            service30.DelegateName = "DelMenu";
            service30.NonLogin = false;
            service30.ServiceName = "DelMenu";
            service31.DelegateName = "DelGroup";
            service31.NonLogin = false;
            service31.ServiceName = "DelGroup";
            service32.DelegateName = "GetGroupControl";
            service32.NonLogin = false;
            service32.ServiceName = "GetGroupControl";
            service33.DelegateName = "GetPF";
            service33.NonLogin = false;
            service33.ServiceName = "GetPF";
            service34.DelegateName = "GetLanguage";
            service34.NonLogin = false;
            service34.ServiceName = "GetLanguage";
            service35.DelegateName = "ChangePassword";
            service35.NonLogin = false;
            service35.ServiceName = "ChangePassword";
            service36.DelegateName = "UpdateMenu";
            service36.NonLogin = false;
            service36.ServiceName = "UpdateMenu";
            service37.DelegateName = "GetADUsers";
            service37.NonLogin = false;
            service37.ServiceName = "GetADUsers";
            service38.DelegateName = "GetADUserForGroup";
            service38.NonLogin = false;
            service38.ServiceName = "GetADUserForGroup";
            service39.DelegateName = "GetSysMsgXml";
            service39.NonLogin = false;
            service39.ServiceName = "GetSysMsgXml";
            service40.DelegateName = "GetServerPath";
            service40.NonLogin = false;
            service40.ServiceName = "GetServerPath";
            service41.DelegateName = "LoadUsers";
            service41.NonLogin = false;
            service41.ServiceName = "LoadUsers";
            service42.DelegateName = "SetUsers";
            service42.NonLogin = false;
            service42.ServiceName = "SetUsers";
            service43.DelegateName = "GetUser";
            service43.NonLogin = false;
            service43.ServiceName = "GetUser";
            service44.DelegateName = "DelUser";
            service44.NonLogin = false;
            service44.ServiceName = "DelUser";
            service45.DelegateName = "UpdateUser";
            service45.NonLogin = false;
            service45.ServiceName = "UpdateUser";
            service46.DelegateName = "InsertToUser";
            service46.NonLogin = false;
            service46.ServiceName = "InsertToUser";
            service47.DelegateName = "GetUserControl";
            service47.NonLogin = false;
            service47.ServiceName = "GetUserControl";
            service48.DelegateName = "DownLoadFile";
            service48.NonLogin = false;
            service48.ServiceName = "DownLoadFile";
            service49.DelegateName = "UpLoadFile";
            service49.NonLogin = false;
            service49.ServiceName = "UpLoadFile";
            service50.DelegateName = "DownloadModule";
            service50.NonLogin = false;
            service50.ServiceName = "DownloadModule";
            service51.DelegateName = "GetUserGroup";
            service51.NonLogin = false;
            service51.ServiceName = "GetUserGroup";
            service52.DelegateName = "ListUsers";
            service52.NonLogin = false;
            service52.ServiceName = "ListUsers";
            service53.DelegateName = "SetUserGroups";
            service53.NonLogin = false;
            service53.ServiceName = "SetUserGroups";
            service54.DelegateName = "UpdateMenuTable";
            service54.NonLogin = false;
            service54.ServiceName = "UpdateMenuTable";
            service55.DelegateName = "FetchAllMenus";
            service55.NonLogin = false;
            service55.ServiceName = "FetchAllMenus";
            service56.DelegateName = "SetUserMenus";
            service56.NonLogin = false;
            service56.ServiceName = "SetUserMenus";
            service57.DelegateName = "SetGroupMenus";
            service57.NonLogin = false;
            service57.ServiceName = "SetGroupMenus";
            service58.DelegateName = "GetDataBaseType";
            service58.NonLogin = false;
            service58.ServiceName = "GetDataBaseType";
            service59.DelegateName = "GetSystemDBType";
            service59.NonLogin = false;
            service59.ServiceName = "GetSystemDBType";
            service60.DelegateName = "DoRecordLock";
            service60.NonLogin = false;
            service60.ServiceName = "DoRecordLock";
            service61.DelegateName = "PackageUpload";
            service61.NonLogin = false;
            service61.ServiceName = "PackageUpload";
            service62.DelegateName = "PackageDownLoad";
            service62.NonLogin = false;
            service62.ServiceName = "PackageDownLoad";
            service63.DelegateName = "PackageRollback";
            service63.NonLogin = false;
            service63.ServiceName = "PackageRollback";
            service64.DelegateName = "CheckManagerRight";
            service64.NonLogin = false;
            service64.ServiceName = "CheckManagerRight";
            service65.DelegateName = "GetRolesByUserID";
            service65.NonLogin = false;
            service65.ServiceName = "GetRolesByUserID";
            service66.DelegateName = "GetDDColumnsSchema";
            service66.NonLogin = false;
            service66.ServiceName = "GetDDColumnsSchema";
            service67.DelegateName = "GetProviderName";
            service67.NonLogin = false;
            service67.ServiceName = "GetProviderName";
            service68.DelegateName = "GetDataModule";
            service68.NonLogin = false;
            service68.ServiceName = "GetDataModule";
            service69.DelegateName = "GetFieldCaption";
            service69.NonLogin = false;
            service69.ServiceName = "GetFieldCaption";
            service70.DelegateName = "GetSolutionSecurity";
            service70.NonLogin = false;
            service70.ServiceName = "GetSolutionSecurity";
            service71.DelegateName = "SetSingleSignOnPath";
            service71.NonLogin = false;
            service71.ServiceName = "SetSingleSignOnPath";
            service72.DelegateName = "GetSingleSignOnPath";
            service72.NonLogin = false;
            service72.ServiceName = "GetSingleSignOnPath";
            service73.DelegateName = "FLOvertimeList";
            service73.NonLogin = false;
            service73.ServiceName = "FLOvertimeList";
            service74.DelegateName = "FetchFavorMenus";
            service74.NonLogin = false;
            service74.ServiceName = "FetchFavorMenus";
            service75.DelegateName = "GetFavorMenuID";
            service75.NonLogin = false;
            service75.ServiceName = "GetFavorMenuID";
            service76.DelegateName = "isTableExist";
            service76.NonLogin = false;
            service76.ServiceName = "isTableExist";
            service77.DelegateName = "GetLogDatas";
            service77.NonLogin = false;
            service77.ServiceName = "GetLogDatas";
            service78.DelegateName = "GetPasswordLastDate";
            service78.NonLogin = false;
            service78.ServiceName = "GetPasswordLastDate";
            service79.DelegateName = "UpdatePackage";
            service79.NonLogin = false;
            service79.ServiceName = "UpdatePackage";
            service80.DelegateName = "GetMethodName";
            service80.NonLogin = false;
            service80.ServiceName = "GetMethodName";
            service81.DelegateName = "CheckAndDownLoad";
            service81.NonLogin = false;
            service81.ServiceName = "CheckAndDownLoad";
            service82.DelegateName = "UserDefineLog";
            service82.NonLogin = false;
            service82.ServiceName = "UserDefineLog";
            service83.DelegateName = "GetDBLog";
            service83.NonLogin = false;
            service83.ServiceName = "GetDBLog";
            service84.DelegateName = "GetWebSitePath";
            service84.NonLogin = false;
            service84.ServiceName = "GetWebSitePath";
            service85.DelegateName = "AnyQuerySave";
            service85.NonLogin = false;
            service85.ServiceName = "AnyQuerySave";
            service86.DelegateName = "AnyQueryLoad";
            service86.NonLogin = false;
            service86.ServiceName = "AnyQueryLoad";
            service87.DelegateName = "UpdateFLXoml";
            service87.NonLogin = false;
            service87.ServiceName = "UpdateFLXoml";
            service88.DelegateName = "AnyQueryLoadFile";
            service88.NonLogin = false;
            service88.ServiceName = "AnyQueryLoadFile";
            service89.DelegateName = "AnyQueryDeleteFile";
            service89.NonLogin = false;
            service89.ServiceName = "AnyQueryDeleteFile";
            service90.DelegateName = "ExcuteWorkFlow";
            service90.NonLogin = false;
            service90.ServiceName = "ExcuteWorkFlow";
            service91.DelegateName = "GetSplitSysDB2";
            service91.NonLogin = false;
            service91.ServiceName = "GetSplitSysDB2";
            service92.DelegateName = "GetTableNames";
            service92.NonLogin = false;
            service92.ServiceName = "GetTableNames";
            service93.DelegateName = "GetOrgKinds";
            service93.NonLogin = false;
            service93.ServiceName = "GetOrgKinds";
            service94.DelegateName = "GetRoles";
            service94.NonLogin = false;
            service94.ServiceName = "GetRoles";
            service95.DelegateName = "GetRefRoles";
            service95.NonLogin = false;
            service95.ServiceName = "GetRefRoles";
            service96.DelegateName = "GetOrgLevel";
            service96.NonLogin = false;
            service96.ServiceName = "GetOrgLevel";
            service97.DelegateName = "UpdateWorkFlow";
            service97.NonLogin = false;
            service97.ServiceName = "UpdateWorkFlow";
            service98.DelegateName = "GetUserRole";
            service98.NonLogin = false;
            service98.ServiceName = "GetUserRole";
            service99.DelegateName = "GetSrvoolAssemblyName";
            service99.NonLogin = false;
            service99.ServiceName = "GetSrvoolAssemblyName";
            service100.DelegateName = "GetPasswordPolicy";
            service100.NonLogin = false;
            service100.ServiceName = "GetPasswordPolicy";
            service101.DelegateName = "GetAllUsers";
            service101.NonLogin = false;
            service101.ServiceName = "GetAllUsers";
            service102.DelegateName = "GetAllGroups";
            service102.NonLogin = false;
            service102.ServiceName = "GetAllGroups";
            service103.DelegateName = "GetWebSitePath";
            service103.NonLogin = false;
            service103.ServiceName = "GetWebSitePath";
            service104.DelegateName = "GetWebSiteConfig";
            service104.NonLogin = false;
            service104.ServiceName = "GetWebSiteConfig";
            service105.DelegateName = "UpLoadWorkflowFile";
            service105.NonLogin = false;
            service105.ServiceName = "UpLoadWorkflowFile";
            service106.DelegateName = "DeleteWorkFlowAttachFile";
            service106.NonLogin = false;
            service106.ServiceName = "DeleteWorkFlowAttachFile";
            service107.DelegateName = "SavePersonalSettings";
            service107.NonLogin = false;
            service107.ServiceName = "SavePersonalSettings";
            service108.DelegateName = "LoadPersonalSettings";
            service108.NonLogin = false;
            service108.ServiceName = "LoadPersonalSettings";
            service109.DelegateName = "UpdateADUsers";
            service109.NonLogin = false;
            service109.ServiceName = "UpdateADUsers";
            service110.DelegateName = "UpdateADGroups";
            service110.NonLogin = false;
            service110.ServiceName = "UpdateADGroups";
            service111.DelegateName = "DeleteGroupMenuControls";
            service111.NonLogin = false;
            service111.ServiceName = "DeleteGroupMenuControls";
            service112.DelegateName = "DeleteUserMenuControls";
            service112.NonLogin = false;
            service112.ServiceName = "DeleteUserMenuControls";
            service113.DelegateName = "UpdateRoleAgent";
            service113.NonLogin = false;
            service113.ServiceName = "UpdateRoleAgent";
            service114.DelegateName = "GetSysTableByLoginDB";
            service114.NonLogin = false;
            service114.ServiceName = "GetSysTableByLoginDB";
            service115.DelegateName = "BeginWorkFlowTransaction";
            service115.NonLogin = false;
            service115.ServiceName = "BeginWorkFlowTransaction";
            service116.DelegateName = "ComitWorkFlowTransaction";
            service116.NonLogin = false;
            service116.ServiceName = "ComitWorkFlowTransaction";
            service117.DelegateName = "RollBackWorkFlowTransaction";
            service117.NonLogin = false;
            service117.ServiceName = "RollBackWorkFlowTransaction";
            service118.DelegateName = "SaveFlowXoml";
            service118.NonLogin = false;
            service118.ServiceName = "SaveFlowXoml";
            service119.DelegateName = "GetDataBaseSubType";
            service119.NonLogin = false;
            service119.ServiceName = "GetDataBaseSubType";
            service120.DelegateName = "GetLoginFile";
            service120.NonLogin = false;
            service120.ServiceName = "GetLoginFile";
            this.serviceManager1.ServiceCollection.Add(service1);
            this.serviceManager1.ServiceCollection.Add(service2);
            this.serviceManager1.ServiceCollection.Add(service3);
            this.serviceManager1.ServiceCollection.Add(service4);
            this.serviceManager1.ServiceCollection.Add(service5);
            this.serviceManager1.ServiceCollection.Add(service6);
            this.serviceManager1.ServiceCollection.Add(service7);
            this.serviceManager1.ServiceCollection.Add(service8);
            this.serviceManager1.ServiceCollection.Add(service9);
            this.serviceManager1.ServiceCollection.Add(service10);
            this.serviceManager1.ServiceCollection.Add(service11);
            this.serviceManager1.ServiceCollection.Add(service12);
            this.serviceManager1.ServiceCollection.Add(service13);
            this.serviceManager1.ServiceCollection.Add(service14);
            this.serviceManager1.ServiceCollection.Add(service15);
            this.serviceManager1.ServiceCollection.Add(service16);
            this.serviceManager1.ServiceCollection.Add(service17);
            this.serviceManager1.ServiceCollection.Add(service18);
            this.serviceManager1.ServiceCollection.Add(service19);
            this.serviceManager1.ServiceCollection.Add(service20);
            this.serviceManager1.ServiceCollection.Add(service21);
            this.serviceManager1.ServiceCollection.Add(service22);
            this.serviceManager1.ServiceCollection.Add(service23);
            this.serviceManager1.ServiceCollection.Add(service24);
            this.serviceManager1.ServiceCollection.Add(service25);
            this.serviceManager1.ServiceCollection.Add(service26);
            this.serviceManager1.ServiceCollection.Add(service27);
            this.serviceManager1.ServiceCollection.Add(service28);
            this.serviceManager1.ServiceCollection.Add(service29);
            this.serviceManager1.ServiceCollection.Add(service30);
            this.serviceManager1.ServiceCollection.Add(service31);
            this.serviceManager1.ServiceCollection.Add(service32);
            this.serviceManager1.ServiceCollection.Add(service33);
            this.serviceManager1.ServiceCollection.Add(service34);
            this.serviceManager1.ServiceCollection.Add(service35);
            this.serviceManager1.ServiceCollection.Add(service36);
            this.serviceManager1.ServiceCollection.Add(service37);
            this.serviceManager1.ServiceCollection.Add(service38);
            this.serviceManager1.ServiceCollection.Add(service39);
            this.serviceManager1.ServiceCollection.Add(service40);
            this.serviceManager1.ServiceCollection.Add(service41);
            this.serviceManager1.ServiceCollection.Add(service42);
            this.serviceManager1.ServiceCollection.Add(service43);
            this.serviceManager1.ServiceCollection.Add(service44);
            this.serviceManager1.ServiceCollection.Add(service45);
            this.serviceManager1.ServiceCollection.Add(service46);
            this.serviceManager1.ServiceCollection.Add(service47);
            this.serviceManager1.ServiceCollection.Add(service48);
            this.serviceManager1.ServiceCollection.Add(service49);
            this.serviceManager1.ServiceCollection.Add(service50);
            this.serviceManager1.ServiceCollection.Add(service51);
            this.serviceManager1.ServiceCollection.Add(service52);
            this.serviceManager1.ServiceCollection.Add(service53);
            this.serviceManager1.ServiceCollection.Add(service54);
            this.serviceManager1.ServiceCollection.Add(service55);
            this.serviceManager1.ServiceCollection.Add(service56);
            this.serviceManager1.ServiceCollection.Add(service57);
            this.serviceManager1.ServiceCollection.Add(service58);
            this.serviceManager1.ServiceCollection.Add(service59);
            this.serviceManager1.ServiceCollection.Add(service60);
            this.serviceManager1.ServiceCollection.Add(service61);
            this.serviceManager1.ServiceCollection.Add(service62);
            this.serviceManager1.ServiceCollection.Add(service63);
            this.serviceManager1.ServiceCollection.Add(service64);
            this.serviceManager1.ServiceCollection.Add(service65);
            this.serviceManager1.ServiceCollection.Add(service66);
            this.serviceManager1.ServiceCollection.Add(service67);
            this.serviceManager1.ServiceCollection.Add(service68);
            this.serviceManager1.ServiceCollection.Add(service69);
            this.serviceManager1.ServiceCollection.Add(service70);
            this.serviceManager1.ServiceCollection.Add(service71);
            this.serviceManager1.ServiceCollection.Add(service72);
            this.serviceManager1.ServiceCollection.Add(service73);
            this.serviceManager1.ServiceCollection.Add(service74);
            this.serviceManager1.ServiceCollection.Add(service75);
            this.serviceManager1.ServiceCollection.Add(service76);
            this.serviceManager1.ServiceCollection.Add(service77);
            this.serviceManager1.ServiceCollection.Add(service78);
            this.serviceManager1.ServiceCollection.Add(service79);
            this.serviceManager1.ServiceCollection.Add(service80);
            this.serviceManager1.ServiceCollection.Add(service81);
            this.serviceManager1.ServiceCollection.Add(service82);
            this.serviceManager1.ServiceCollection.Add(service83);
            this.serviceManager1.ServiceCollection.Add(service84);
            this.serviceManager1.ServiceCollection.Add(service85);
            this.serviceManager1.ServiceCollection.Add(service86);
            this.serviceManager1.ServiceCollection.Add(service87);
            this.serviceManager1.ServiceCollection.Add(service88);
            this.serviceManager1.ServiceCollection.Add(service89);
            this.serviceManager1.ServiceCollection.Add(service90);
            this.serviceManager1.ServiceCollection.Add(service91);
            this.serviceManager1.ServiceCollection.Add(service92);
            this.serviceManager1.ServiceCollection.Add(service93);
            this.serviceManager1.ServiceCollection.Add(service94);
            this.serviceManager1.ServiceCollection.Add(service95);
            this.serviceManager1.ServiceCollection.Add(service96);
            this.serviceManager1.ServiceCollection.Add(service97);
            this.serviceManager1.ServiceCollection.Add(service98);
            this.serviceManager1.ServiceCollection.Add(service99);
            this.serviceManager1.ServiceCollection.Add(service100);
            this.serviceManager1.ServiceCollection.Add(service101);
            this.serviceManager1.ServiceCollection.Add(service102);
            this.serviceManager1.ServiceCollection.Add(service103);
            this.serviceManager1.ServiceCollection.Add(service104);
            this.serviceManager1.ServiceCollection.Add(service105);
            this.serviceManager1.ServiceCollection.Add(service106);
            this.serviceManager1.ServiceCollection.Add(service107);
            this.serviceManager1.ServiceCollection.Add(service108);
            this.serviceManager1.ServiceCollection.Add(service109);
            this.serviceManager1.ServiceCollection.Add(service110);
            this.serviceManager1.ServiceCollection.Add(service111);
            this.serviceManager1.ServiceCollection.Add(service112);
            this.serviceManager1.ServiceCollection.Add(service113);
            this.serviceManager1.ServiceCollection.Add(service114);
            this.serviceManager1.ServiceCollection.Add(service115);
            this.serviceManager1.ServiceCollection.Add(service116);
            this.serviceManager1.ServiceCollection.Add(service117);
            this.serviceManager1.ServiceCollection.Add(service118);
            this.serviceManager1.ServiceCollection.Add(service119);
            this.serviceManager1.ServiceCollection.Add(service120);
            // 
            // cmdGROUPMENUCONTROL
            // 
            this.cmdGROUPMENUCONTROL.CacheConnection = false;
            this.cmdGROUPMENUCONTROL.CommandText = "SELECT * FROM GROUPMENUCONTROL";
            this.cmdGROUPMENUCONTROL.CommandTimeout = 30;
            this.cmdGROUPMENUCONTROL.CommandType = System.Data.CommandType.Text;
            this.cmdGROUPMENUCONTROL.DynamicTableName = false;
            this.cmdGROUPMENUCONTROL.EEPAlias = null;
            this.cmdGROUPMENUCONTROL.EncodingAfter = null;
            this.cmdGROUPMENUCONTROL.EncodingBefore = "Windows-1252";
            this.cmdGROUPMENUCONTROL.InfoConnection = null;
            keyItem17.KeyName = "GROUPID";
            keyItem18.KeyName = "MENUID";
            keyItem19.KeyName = "CONTROLNAME";
            this.cmdGROUPMENUCONTROL.KeyFields.Add(keyItem17);
            this.cmdGROUPMENUCONTROL.KeyFields.Add(keyItem18);
            this.cmdGROUPMENUCONTROL.KeyFields.Add(keyItem19);
            this.cmdGROUPMENUCONTROL.MultiSetWhere = false;
            this.cmdGROUPMENUCONTROL.Name = "cmdGROUPMENUCONTROL";
            this.cmdGROUPMENUCONTROL.NotificationAutoEnlist = false;
            this.cmdGROUPMENUCONTROL.SecExcept = null;
            this.cmdGROUPMENUCONTROL.SecFieldName = null;
            this.cmdGROUPMENUCONTROL.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.cmdGROUPMENUCONTROL.SelectPaging = false;
            this.cmdGROUPMENUCONTROL.SelectTop = 0;
            this.cmdGROUPMENUCONTROL.SiteControl = false;
            this.cmdGROUPMENUCONTROL.SiteFieldName = null;
            this.cmdGROUPMENUCONTROL.UpdatedRowSource = System.Data.UpdateRowSource.None;
            // 
            // cmdUSERMENUCONTROL
            // 
            this.cmdUSERMENUCONTROL.CacheConnection = false;
            this.cmdUSERMENUCONTROL.CommandText = "SELECT * FROM USERMENUCONTROL";
            this.cmdUSERMENUCONTROL.CommandTimeout = 30;
            this.cmdUSERMENUCONTROL.CommandType = System.Data.CommandType.Text;
            this.cmdUSERMENUCONTROL.DynamicTableName = false;
            this.cmdUSERMENUCONTROL.EEPAlias = null;
            this.cmdUSERMENUCONTROL.EncodingAfter = null;
            this.cmdUSERMENUCONTROL.EncodingBefore = "Windows-1252";
            this.cmdUSERMENUCONTROL.InfoConnection = null;
            keyItem20.KeyName = "USERID";
            keyItem21.KeyName = "MENUID";
            keyItem22.KeyName = "CONTROLNAME";
            this.cmdUSERMENUCONTROL.KeyFields.Add(keyItem20);
            this.cmdUSERMENUCONTROL.KeyFields.Add(keyItem21);
            this.cmdUSERMENUCONTROL.KeyFields.Add(keyItem22);
            this.cmdUSERMENUCONTROL.MultiSetWhere = false;
            this.cmdUSERMENUCONTROL.Name = "cmdUSERMENUCONTROL";
            this.cmdUSERMENUCONTROL.NotificationAutoEnlist = false;
            this.cmdUSERMENUCONTROL.SecExcept = null;
            this.cmdUSERMENUCONTROL.SecFieldName = null;
            this.cmdUSERMENUCONTROL.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.cmdUSERMENUCONTROL.SelectPaging = false;
            this.cmdUSERMENUCONTROL.SelectTop = 0;
            this.cmdUSERMENUCONTROL.SiteControl = false;
            this.cmdUSERMENUCONTROL.SiteFieldName = null;
            this.cmdUSERMENUCONTROL.UpdatedRowSource = System.Data.UpdateRowSource.None;
            // 
            // cmdSYS_REPORT
            // 
            this.cmdSYS_REPORT.CacheConnection = false;
            this.cmdSYS_REPORT.CommandText = "select * from SYS_REPORT";
            this.cmdSYS_REPORT.CommandTimeout = 30;
            this.cmdSYS_REPORT.CommandType = System.Data.CommandType.Text;
            this.cmdSYS_REPORT.DynamicTableName = false;
            this.cmdSYS_REPORT.EEPAlias = null;
            this.cmdSYS_REPORT.EncodingAfter = null;
            this.cmdSYS_REPORT.EncodingBefore = "Windows-1252";
            this.cmdSYS_REPORT.InfoConnection = null;
            this.cmdSYS_REPORT.MultiSetWhere = false;
            this.cmdSYS_REPORT.Name = "cmdSYS_REPORT";
            this.cmdSYS_REPORT.NotificationAutoEnlist = false;
            this.cmdSYS_REPORT.SecExcept = null;
            this.cmdSYS_REPORT.SecFieldName = null;
            this.cmdSYS_REPORT.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.cmdSYS_REPORT.SelectPaging = false;
            this.cmdSYS_REPORT.SelectTop = 0;
            this.cmdSYS_REPORT.SiteControl = false;
            this.cmdSYS_REPORT.SiteFieldName = null;
            this.cmdSYS_REPORT.UpdatedRowSource = System.Data.UpdateRowSource.None;
            // 
            // cmdMENUTABLECONTROL
            // 
            this.cmdMENUTABLECONTROL.CacheConnection = false;
            this.cmdMENUTABLECONTROL.CommandText = "select * from MENUTABLECONTROL";
            this.cmdMENUTABLECONTROL.CommandTimeout = 0;
            this.cmdMENUTABLECONTROL.CommandType = System.Data.CommandType.Text;
            this.cmdMENUTABLECONTROL.DynamicTableName = false;
            this.cmdMENUTABLECONTROL.EEPAlias = null;
            this.cmdMENUTABLECONTROL.EncodingAfter = null;
            this.cmdMENUTABLECONTROL.EncodingBefore = "Windows-1252";
            this.cmdMENUTABLECONTROL.InfoConnection = null;
            keyItem23.KeyName = "MENUID";
            keyItem24.KeyName = "CONTROLNAME";
            this.cmdMENUTABLECONTROL.KeyFields.Add(keyItem23);
            this.cmdMENUTABLECONTROL.KeyFields.Add(keyItem24);
            this.cmdMENUTABLECONTROL.MultiSetWhere = false;
            this.cmdMENUTABLECONTROL.Name = "cmdMENUTABLECONTROL";
            this.cmdMENUTABLECONTROL.NotificationAutoEnlist = false;
            this.cmdMENUTABLECONTROL.SecExcept = "";
            this.cmdMENUTABLECONTROL.SecFieldName = null;
            this.cmdMENUTABLECONTROL.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.cmdMENUTABLECONTROL.SelectPaging = false;
            this.cmdMENUTABLECONTROL.SelectTop = 0;
            this.cmdMENUTABLECONTROL.SiteControl = false;
            this.cmdMENUTABLECONTROL.SiteFieldName = null;
            this.cmdMENUTABLECONTROL.UpdatedRowSource = System.Data.UpdateRowSource.Both;
            // 
            // ucMENUTABLECONTROL
            // 
            this.ucMENUTABLECONTROL.AutoTrans = true;
            this.ucMENUTABLECONTROL.ExceptJoin = false;
            fieldAttr4.CheckNull = false;
            fieldAttr4.DataField = "MENUID";
            fieldAttr4.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr4.DefaultValue = null;
            fieldAttr4.TrimLength = 0;
            fieldAttr4.UpdateEnable = true;
            fieldAttr4.WhereMode = true;
            fieldAttr5.CheckNull = false;
            fieldAttr5.DataField = "CONTROLNAME";
            fieldAttr5.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr5.DefaultValue = null;
            fieldAttr5.TrimLength = 0;
            fieldAttr5.UpdateEnable = true;
            fieldAttr5.WhereMode = true;
            fieldAttr6.CheckNull = false;
            fieldAttr6.DataField = "DESCRIPTION";
            fieldAttr6.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr6.DefaultValue = null;
            fieldAttr6.TrimLength = 0;
            fieldAttr6.UpdateEnable = true;
            fieldAttr6.WhereMode = true;
            fieldAttr7.CheckNull = false;
            fieldAttr7.DataField = "TYPE";
            fieldAttr7.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr7.DefaultValue = null;
            fieldAttr7.TrimLength = 0;
            fieldAttr7.UpdateEnable = true;
            fieldAttr7.WhereMode = true;
            this.ucMENUTABLECONTROL.FieldAttrs.Add(fieldAttr4);
            this.ucMENUTABLECONTROL.FieldAttrs.Add(fieldAttr5);
            this.ucMENUTABLECONTROL.FieldAttrs.Add(fieldAttr6);
            this.ucMENUTABLECONTROL.FieldAttrs.Add(fieldAttr7);
            this.ucMENUTABLECONTROL.LogInfo = null;
            this.ucMENUTABLECONTROL.Name = "ucMENUTABLECONTROL";
            this.ucMENUTABLECONTROL.RowAffectsCheck = true;
            this.ucMENUTABLECONTROL.SelectCmd = this.cmdMENUTABLECONTROL;
            this.ucMENUTABLECONTROL.SelectCmdForUpdate = null;
            this.ucMENUTABLECONTROL.ServerModify = true;
            this.ucMENUTABLECONTROL.ServerModifyGetMax = false;
            this.ucMENUTABLECONTROL.TranscationScopeTimeOut = System.TimeSpan.Parse("00:02:00");
            this.ucMENUTABLECONTROL.TransIsolationLevel = System.Data.IsolationLevel.ReadCommitted;
            this.ucMENUTABLECONTROL.UseTranscationScope = false;
            this.ucMENUTABLECONTROL.WhereMode = Srvtools.WhereModeType.FieldAttrs;
            ((System.ComponentModel.ISupportInitialize)(this.userInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UGInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sqlMGroups)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sqlMGroupMenus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sqlMenus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.packageInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.solutionInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.menuTableLogInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.menuTableLogInfoWithoutBinary)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmdDBTables)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmdColDEF)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmdColDEF_Details)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmdRefValUse)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmdERRLOG)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmdSYSEEPLOGforDB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmdDDUse)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.userMenus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.packageversion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmdSysRefVal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmdSysRefVal_D)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmdToDoList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmdToDoHis)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmdRoles)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmdOrgRoles)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmdOrgLevel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmdOrgKind)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmdWorkflow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmdRoleAgent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmdSlSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmdGROUPMENUCONTROL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmdUSERMENUCONTROL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmdSYS_REPORT)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmdMENUTABLECONTROL)).EndInit();

        }
        #endregion

        private void cmdDBTables_BeforeExecuteSql(object sender, SqlEventArgs e)
        {
            string loginDataBaseAlias = ((object[])(ClientInfo[0]))[2].ToString();
            string loginDataBaseTypeNO = GetDataBaseTypeforString(loginDataBaseAlias);
            string type = GetSystemDBTypeforString();

            if (type != loginDataBaseTypeNO)
            {
                if (loginDataBaseTypeNO == "1" && !(cmdDBTables.CommandText.Contains("sysobjects")))
                    e.Sql = "select name from sysobjects where (xtype='U' or xtype='V') order by name";
                else if (loginDataBaseTypeNO == "2")
                    e.Sql = "sp_help";
                else if (loginDataBaseTypeNO == "3" && !(cmdDBTables.CommandText.Contains("user_objects")))
                    e.Sql = "select OBJECT_NAME from USER_OBJECTS where (OBJECT_TYPE = 'TABLE' or OBJECT_TYPE = 'VIEW' or OBJECT_TYPE = 'SYNONYM') order by OBJECT_NAME";
                else if (loginDataBaseTypeNO == "4")
                {
                    var subType = GetDataBaseSubType(new object[] { loginDataBaseAlias, false });
                    if (subType[1].ToString() == "2")
                        e.Sql = "select TABLE_NAME AS tabname, TABLE_OWNER AS owner from qsys2.SYSTABLES where TABLE_TYPE='T' OR TABLE_TYPE = 'V'";
                    else if (subType[1].ToString() == "0")
                        e.Sql = "select * from SYSTABLES where (TABTYPE = 'T' or TABTYPE = 'V') and TABID >= 100 order by TABNAME";
                }
                else if (loginDataBaseTypeNO == "5")
                    e.Sql = "show tables;";
                else if (loginDataBaseTypeNO == "6")
                    e.Sql = "select * from SYSTABLES where (TABTYPE = 'T' or TABTYPE = 'V') and TABID >= 100 order by TABNAME";
                else if (loginDataBaseTypeNO == "7")
                    e.Sql = "sp_help";
            }
        }
    }
}