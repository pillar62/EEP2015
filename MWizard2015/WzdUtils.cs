using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Data.Odbc;
using System.Data.OracleClient;
using System.Data.Common;
using System.Windows.Forms;
using System.Xml;
using System.Reflection;
using Microsoft.Win32;
using System.IO;
using Srvtools;
using System.Collections;
using EnvDTE;
using System.Threading;
using EFClientTools.Beans;
using EFClientTools;
using EFClientTools.EFServerReference;
using MWizard2015.WCF;

namespace MWizard2015
{
    public class WzdUtils
    {
#if VS90
        const string REGISTRYNAME = "infolight\\eep.net"; //2010中没有infolight\\eep.net2008，统一使用infolight\\eep.net
#else
        const string REGISTRYNAME = "infolight\\eep.net";
#endif

        public static void SetListViewSelect(ListView lv, bool isSelected, int selected)
        {
            for (int i = 0; i < lv.Items.Count; i++)
                if (i == selected)
                    lv.Items[i].Selected = isSelected;
                else
                    lv.Items[i].Selected = !isSelected;
        }

        static WzdUtils()
        {
            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
        }


        internal static string GetRegistryValueByKey(String keyName)
        {
            String registryValue = String.Empty;
            RegistryKey key;
            using (key = Registry.LocalMachine.OpenSubKey("SOFTWARE\\" + REGISTRYNAME, RegistryKeyPermissionCheck.ReadWriteSubTree))
            {
                if (key != null)
                {
                    registryValue = key.GetValue(keyName).ToString();
                }
            }
            return registryValue;
        }

        internal static void SetRegistryValueByKey(String keyName, String registryValue)
        {
            RegistryKey key;
            using (key = Registry.LocalMachine.OpenSubKey("SOFTWARE\\" + REGISTRYNAME, RegistryKeyPermissionCheck.ReadWriteSubTree))
            {
                if (key == null)
                    key = Registry.LocalMachine.CreateSubKey("SOFTWARE\\" + REGISTRYNAME);

                key.SetValue(keyName, registryValue);
            }
        }

        static private AddIn fAddIn;

        public static AddIn FAddIn
        {
            get { return WzdUtils.fAddIn; }
            set { WzdUtils.fAddIn = value; }
        }

        public static void Application_ThreadException(object sender, ThreadExceptionEventArgs args)
        {
            Exception e = (Exception)args.Exception;

            if (e is TargetInvocationException)
            {
                while (e.InnerException != null)
                {
                    e = e.InnerException;
                }
            }
            string serverstatck = e.InnerException == null ? string.Empty : e.InnerException.StackTrace;
            ErrorDialog fError = new ErrorDialog(e.Message, e.StackTrace, serverstatck);
            fError.ShowDialog();
            fError.Dispose();
        }

        static public string GetAddinsPath()
        {
            RegistryKey rk = Registry.LocalMachine.OpenSubKey("SOFTWARE\\" + REGISTRYNAME);
            if (rk != null)
            {
                string value = (string)rk.GetValue("Addins Path");
                rk.Close();
                if (value != null)
                {
                    value = value.TrimEnd('\\');
                    return value;
                }
                else {
                    MessageBox.Show("Please Set Addin path in InitEEP.exe");
                }
            }
            return "";
        }
        static private XmlDocument GetServerPathXML(bool isOpenForm)
        {
            String strAddinPath = GetAddinsPath() + "\\ServerPath.xml";
            if (!File.Exists(strAddinPath))
            {
                if (isOpenForm)
                {
                    fmSetServerPath fmssp = new fmSetServerPath();
                    fmssp.ShowDialog();
                }
                else
                {
                    return null;
                }
            }
            XmlDocument x = new XmlDocument();
            x.Load(strAddinPath);

            return x;
        }

        static public String GetServerPath(AddIn addIn, bool isOpenForm)
        {
            String strReturn = String.Empty;
            XmlDocument x = GetServerPathXML(isOpenForm);
            if (x != null && x.FirstChild.ChildNodes.Count > 0)
                strReturn = x.FirstChild.ChildNodes[0].Attributes["Value"].Value;

            return strReturn;
        }

        static public String GetWebClientPath(AddIn addIn, bool isOpenForm)
        {
            String strReturn = String.Empty;
            XmlDocument x = GetServerPathXML(isOpenForm);
            if (x != null && x.FirstChild.ChildNodes.Count > 1)
                strReturn = x.FirstChild.ChildNodes[1].Attributes["Value"].Value;

            return strReturn;
        }

        static public String GetEEPAlias(AddIn addIn, bool isOpenForm)
        {
            String strReturn = String.Empty;
            XmlDocument x = GetServerPathXML(isOpenForm);
            if (x != null && x.FirstChild.ChildNodes.Count > 2)
                strReturn = x.FirstChild.ChildNodes[2].Attributes["Value"].Value;

            return strReturn;
        }

        static public List<String> GetCommandNamesByDataModuleName(String dataModuleName)
        {
            return DesignClientUtility.GetCommandNames(dataModuleName);
        }

        static public Dictionary<string, object> GetFieldsByEntityName(string assembly, string commandName, string entityTypeName)
        {
            return EFAssembly.EFClientToolsAssemblyAdapt.DesignClientUtility.GetEntityPropertiesTypes(assembly, commandName, entityTypeName);
        }

        //static public List<EFClientTools.EFServerReference.EntityObject> GetColumnDefination(string assembly, string commandName, string entityTypeName)
        //{
        //    string serverEntityClassName = EFAssembly.EFClientToolsAssemblyAdapt.EntityProvider.GetServerEntityClassName(assembly, entityTypeName);

        //    return EFAssembly.EFClientToolsAssemblyAdapt.DesignClientUtility.GetColumnDefination(assembly, commandName, serverEntityClassName);
        //}

        [Obsolete]
        static public Hashtable GetFieldsByEntityName(string entityTypeName)
        {
            return EFAssembly.EFClientToolsAssemblyAdapt.EntityProvider.GetEntityPropertiesTypes(entityTypeName);
        }

        static public List<string> GetDetailEntityNames(string strMasterEntityName)
        {
            return EFAssembly.EFClientToolsAssemblyAdapt.EntityProvider.GetDetailEntityClassNames(strMasterEntityName);
        }

        static public Dictionary<String, String> GetDetailEntityClassNameAndEntitySetName(string strMasterEntityName)
        {
            return EFAssembly.EFClientToolsAssemblyAdapt.EntityProvider.GetDetailEntityClassNameAndEntitySetName(strMasterEntityName);
        }

        static public List<object> GetAllDataByTableName(String tableName)
        {
            List<object> lists = new List<object>();
            foreach (var item in DesignClientUtility.GetAllDataByTableName(tableName))
            {
                lists.Add(item);
            }
            return lists;
        }

        static public void SaveDataToTable(List<object> lRefvals, String tableName)
        {
            DesignClientUtility.SaveDataToTable(lRefvals, tableName);
        }

        static public List<COLDEFInfo> GetColumnDefination(string assemblyName, string commandName, string entityTypeName, String loginDataBase)
        {
            //return EFAssembly.EFClientToolsAssemblyAdapt.DesignClientUtility.GetColumnDefination(assemblyName, commandName, entityTypeName);
            //string serverEntityClassName = EFAssembly.EFClientToolsAssemblyAdapt.EntityProvider.GetServerEntityClassName(assemblyName, entityTypeName);
            return DesignClientUtility.GetColumnDefination(assemblyName, commandName, entityTypeName, false, loginDataBase);
        }

        static public List<string> GetEntityPrimaryKeys(string assemblyName, string commandName, string entityTypeName)
        {
            return EFAssembly.EFClientToolsAssemblyAdapt.DesignClientUtility.GetEntityPrimaryKeys(assemblyName, commandName, entityTypeName);
            //return DesignClientUtility.GetEntityPrimaryKeys(assemblyName, commandName, entityTypeName);
        }

        static public String GetServerEntityClassName(String assembly, String entityTypeName)
        {
            string serverEntityClassName = EFAssembly.EFClientToolsAssemblyAdapt.EntityProvider.GetServerEntityClassName(assembly, entityTypeName);
            return serverEntityClassName;
        }

        static public List<String> GetEntityNavigationFields(String masterClassName)
        {
            List<String> entityNavigationFields = EFAssembly.EFClientToolsAssemblyAdapt.EntityProvider.GetEntityNavigationFields(masterClassName);
            return entityNavigationFields;
        }

        //[Obsolete]
        //static public List<string> GetDetailEntityNames(string assemblyName, string commandName, string masterClassName)
        //{
        //    return EFClientTools.DesignClientUtility.GetDetailEntityClassNames(assemblyName,commandName, masterClassName);
        //}

        static public IDbDataAdapter AllocateDataAdapter(ClientType ct)
        {
            IDbDataAdapter ob = null;
            try
            {
                if (ct == ClientType.ctMsSql)
                    ob = new SqlDataAdapter();
                else if (ct == ClientType.ctOleDB)
                    ob = new OleDbDataAdapter();
                else if (ct == ClientType.ctOracle)
                    ob = new OracleDataAdapter();
                else if (ct == ClientType.ctODBC)
                    ob = new OdbcDataAdapter();
                else if (ct == ClientType.ctMySql)
                {
                    String s = EEPRegistry.Server + "\\MySql.Data.dll";
                    Assembly assembly = Assembly.LoadFrom(s);
                    ob = assembly.CreateInstance("MySql.Data.MySqlClient.MySqlDataAdapter") as IDbDataAdapter;
                }
                else if (ct == ClientType.ctInformix)
                {
                    String s = EEPRegistry.Server + "\\IBM.Data.Informix.dll";
                    Assembly assembly = Assembly.LoadFrom(s);
                    ob = assembly.CreateInstance("IBM.Data.Informix.IfxDataAdapter") as IDbDataAdapter;
                }
                else if (ct == ClientType.ctSybase)
                {
                    String s = EEPRegistry.Server + "\\Sybase.Data.AseClient.dll";
                    Assembly assembly = Assembly.LoadFrom(s);
                    ob = assembly.CreateInstance("Sybase.Data.AseClient.AseDataAdapter") as IDbDataAdapter;
                }
            }
            catch (Exception e)
            {
                throw new Exception("Allocate DataAdapter Error: " + e.Message);
            }
            return ob;
        }

        public static void FillDataAdapter(ClientType ct, IDbDataAdapter da, DataTable table)
        {
            try
            {
                if (ct == ClientType.ctMsSql)
                    ((SqlDataAdapter)da).Fill(table);
                else if (ct == ClientType.ctOleDB)
                    ((OleDbDataAdapter)da).Fill(table);
                else if (ct == ClientType.ctOracle)
                    ((OracleDataAdapter)da).Fill(table);
                else if (ct == ClientType.ctODBC)
                    ((OdbcDataAdapter)da).Fill(table);
                else if (ct == ClientType.ctMySql)
                {
                    String s = EEPRegistry.Server + "\\MySql.Data.dll";
                    Assembly assembly = Assembly.LoadFrom(s);
                    Type t = assembly.GetType("MySql.Data.MySqlClient.MySqlDataAdapter");
                    MethodInfo temp = t.GetMethod("Fill", new Type[] { typeof(DataTable) });
                    temp.Invoke(da, new object[] { table });
                }
                else if (ct == ClientType.ctInformix)
                {
                    String s = EEPRegistry.Server + "\\IBM.Data.Informix.dll";
                    Assembly assembly = Assembly.LoadFrom(s);
                    Type t = assembly.GetType("IBM.Data.Informix.IfxDataAdapter");
                    MethodInfo temp = t.GetMethod("Fill", new Type[] { typeof(DataTable) });
                    temp.Invoke(da, new object[] { table });
                }
                else if (ct == ClientType.ctSybase)
                {
                    String s = EEPRegistry.Server + "\\Sybase.Data.AseClient.dll";
                    Assembly assembly = Assembly.LoadFrom(s);
                    Type t = assembly.GetType("Sybase.Data.AseClient.AseDataAdapter");
                    MethodInfo[] temp = t.GetMethods();
                    temp[26].Invoke(da, new object[] { table });
                }
            }
            catch (Exception e)
            {
                throw new Exception("Fill Dataset Error: " + e.Message);
            }
        }

        static public void FillDataAdapter(ClientType ct, IDbDataAdapter da, DataSet custDS, string sTable)
        {
            try
            {
                if (ct == ClientType.ctMsSql)
                    ((SqlDataAdapter)da).Fill(custDS, sTable);
                else if (ct == ClientType.ctOleDB)
                {
                    DataTable dt = new DataTable();
                    ((OleDbDataAdapter)da).Fill(dt);
                    dt.TableName = sTable;
                    custDS.Tables.Add(dt);
                    //((OleDbDataAdapter)da).Fill(custDS, sTable);
                }
                else if (ct == ClientType.ctOracle)
                    ((OracleDataAdapter)da).Fill(custDS, sTable);
                else if (ct == ClientType.ctODBC)
                    ((OdbcDataAdapter)da).Fill(custDS, sTable);
                else if (ct == ClientType.ctMySql)
                {
                    String s = EEPRegistry.Server + "\\MySql.Data.dll";
                    Assembly assembly = Assembly.LoadFrom(s);
                    Type t = assembly.GetType("MySql.Data.MySqlClient.MySqlDataAdapter");
                    MethodInfo temp = t.GetMethod("Fill", new Type[] { typeof(DataSet), typeof(String) });
                    temp.Invoke(da, new object[] { custDS, sTable });
                    //((MySqlDataAdapter)da).Fill(custDS, sTable);
                }
                else if (ct == ClientType.ctInformix)
                {
                    String s = EEPRegistry.Server + "\\IBM.Data.Informix.dll";
                    Assembly assembly = Assembly.LoadFrom(s);
                    Type t = assembly.GetType("IBM.Data.Informix.IfxDataAdapter");
                    MethodInfo temp = t.GetMethod("Fill", new Type[] { typeof(DataSet), typeof(String) });
                    temp.Invoke(da, new object[] { custDS, sTable });
                }
                else if (ct == ClientType.ctSybase)
                {
                    String s = EEPRegistry.Server + "\\Sybase.Data.AseClient.dll";
                    Assembly assembly = Assembly.LoadFrom(s);
                    Type t = assembly.GetType("Sybase.Data.AseClient.AseDataAdapter");
                    MethodInfo[] temp = t.GetMethods();
                    temp[26].Invoke(da, new object[] { custDS, sTable });
                }
            }
            catch (Exception e)
            {
                throw new Exception("Fill Dataset Error: " + e.Message);
            }
        }

        static public DbConnection AllocateConnection(String DBAlias, ClientType aType, bool bGetSysDB)
        {
            DBAlias = bGetSysDB ? GetSplitSysDB(DBAlias) : DBAlias;
            Srvtools.DbConnectionSet.DbConnection dbc = DbConnectionSet.GetDbConn(DBAlias);

            //String ConnectionString = GetConntionString(DBAlias);
            //DbConnection Result = null;
            //try
            //{
            //    if (aType == ClientType.ctMsSql)
            //        Result = new SqlConnection(ConnectionString);
            //    else if (aType == ClientType.ctOleDB)
            //        Result = new OleDbConnection(ConnectionString);
            //    else if (aType == ClientType.ctOracle)
            //        Result = new OracleConnection(ConnectionString);
            //    else if (aType == ClientType.ctODBC)
            //        Result = new OdbcConnection(ConnectionString);
            //    else if (aType == ClientType.ctMySql)
            //    {
            //        String s = EEPRegistry.Server + "\\MySql.Data.dll";
            //        Assembly assembly = Assembly.LoadFrom(s);
            //        Result = assembly.CreateInstance("MySql.Data.MySqlClient.MySqlConnection") as DbConnection;
            //        Result.ConnectionString = ConnectionString;
            //    }
            //    else if (aType == ClientType.ctInformix)
            //    {
            //        String s = EEPRegistry.Server + "\\IBM.Data.Informix.dll";
            //        Assembly assembly = Assembly.LoadFrom(s);
            //        Result = assembly.CreateInstance("IBM.Data.Informix.IfxConnection") as DbConnection;
            //        Result.ConnectionString = ConnectionString;
            //    }

            //}
            //catch (Exception e)
            //{
            //    MessageBox.Show(e.Message);
            //    throw new Exception("Allocate Connection Error: " + e.Message);
            //}
            return dbc.CreateConnection() as DbConnection;
        }

        static private string GetSplitSysDB(String sDB)
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

        public static String GetConntionString(String dataBaseName)
        {
            String connString = String.Empty;
            if (File.Exists(SystemFile.DBFile))
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(SystemFile.DBFile);
                XmlNode node = xml.SelectSingleNode(string.Format("InfolightDB/DataBase/{0}", dataBaseName));
                if (node != null)
                {
                    connString = node.Attributes["String"].InnerText;
                    if (connString.Length > 0)
                    {
                        connString = connString.TrimEnd(';');
                        if (GetPwdString(node.Attributes["Password"].InnerText) != String.Empty)
                            connString = connString + ";Password=" + GetPwdString(node.Attributes["Password"].InnerText);
                    }
                }
            }
            return connString;
        }

        private static String _quotePrefix = "[";
        private static String _quoteSuffix = "]";
        public static String Quote(String table_or_column, IDbConnection conn)
        {
            if (conn is SqlConnection)
            {
                if (table_or_column.Contains("."))
                {
                    string owner = table_or_column.Substring(0, table_or_column.IndexOf("."));
                    table_or_column = table_or_column.Substring(table_or_column.IndexOf(".") + 1);
                    return owner + "." + _quotePrefix + table_or_column + _quoteSuffix;
                }
                if (_quotePrefix == null || _quoteSuffix == null)
                    return table_or_column;
                return _quotePrefix + table_or_column + _quoteSuffix;
            }
            else if (conn is OracleConnection)
            {
                return table_or_column;
            }
            else if (conn is OdbcConnection)
            {
                return table_or_column;
            }
            else if (conn is OleDbConnection)
            {
                return table_or_column;
            }
            else if (conn.GetType().Name == "MySqlConnection")
            {
                return table_or_column;
            }
            else if (conn.GetType().Name == "IfxConnection")
            {
                return table_or_column;
            }
            else if (conn.GetType().Name == "AseConnection")
            {
                return table_or_column;
            }
            return _quotePrefix + table_or_column + _quoteSuffix;
        }

        public static string GetPwdString(string s)
        {
            string sRet = "";

            for (int i = 0; i < s.Length; i++)
            {
                sRet = sRet + (char)(((int)(s[s.Length - 1 - i])) ^ s.Length);
            }
            return sRet;
        }

        /*
        public static string GetPwdString(string s)
        {
            string text1 = "";
            for (int num1 = 0; num1 < s.Length; num1++)
            {
                text1 = text1 + (s[(s.Length - 1) - num1] ^ s.Length);
            }
            return text1;
        }
         */

        public static XmlNode FindNode(XmlDocument Doc, XmlNode ParentNode, string NodeName)
        {
            XmlNode Result = null;
            int I;
            if (ParentNode == null)
            {
                for (I = 0; I < Doc.ChildNodes.Count; I++)
                {
                    if (string.Compare(Doc.ChildNodes[I].Name, NodeName) == 0)
                    {
                        Result = Doc.ChildNodes[I];
                        break;
                    }
                }
            }
            else
            {
                for (I = 0; I < ParentNode.ChildNodes.Count; I++)
                {
                    if (string.Compare(ParentNode.ChildNodes[I].Name, NodeName) == 0)
                    {
                        Result = ParentNode.ChildNodes[I];
                        break;
                    }
                }
            }
            return Result;
        }

        public static string RemoveSpace(string SrcString)
        {
            string Result = "";
            for (int I = 0; I < SrcString.Length; I++)
            {
                if (SrcString[I].ToString() != " ")
                    Result = Result + SrcString[I];
            }
            return Result;
        }

        public static string RemoveSpecialCharacters(string SrcString)
        {
            string Result = SrcString;
            String[] sSpecialCharacters = new String[] { " ", "$", "-", "(", ")", "," };
            foreach (var item in sSpecialCharacters)
            {
                Result = Result.Replace(item, String.Empty);
            }

            return Result;
        }

        public static String RemoveQuote(String value, ClientType ct)
        {
            String rtn = value;

            return rtn;
        }

        public static String GetToken(ref String AString, char[] Fmt)
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
                if (string.Compare(S2, PropName, true) != 0)
                    continue;
                Result = S1;
                break;
            }
            return Result;
        }

        public static String FixupToVSWebSiteName(String PackageName)
        {
            int I = 0;
            String S, S1, LastFolder, Result = "";
            S1 = PackageName;
            S = WzdUtils.GetToken(ref PackageName, new char[] { '\\' });
            LastFolder = PackageName;
            while (S != "")
            {
                I++;
                LastFolder = S;
                S = WzdUtils.GetToken(ref PackageName, new char[] { '\\' });
            }
            if (I > 3)
            {
                S = System.IO.Directory.GetDirectoryRoot(S1);
                Result = S + @"...\" + LastFolder + @"\";
            }
            return Result;
        }

        public static List<String> GetAllTablesList(DbConnection dbConn, ClientType dbType)
        {
            List<String> tablesList = new List<string>();
            String sQL = "";
            if (dbType == ClientType.ctInformix)
                sQL = "select * from SYSTABLES where (TABTYPE = 'T' or TABTYPE = 'V' or TABTYPE = 'S') and TABID >= 100 order by TABNAME";

            IDbCommand cmd = dbConn.CreateCommand();
            cmd.CommandText = sQL;
            if (dbConn.State == ConnectionState.Closed)
            { dbConn.Open(); }

            IDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (reader.Read())
            {
                tablesList.Add(reader[0].ToString());
            }
            reader.Close();
            return tablesList;
        }

        public static void SelectedListViewItemUp(ListView aListView)
        {
            if (aListView.SelectedItems.Count > 0)
            {
                aListView.BeginUpdate();
                if (aListView.SelectedItems[0].Index > 0)
                {
                    foreach (ListViewItem lvi in aListView.SelectedItems)
                    {
                        ListViewItem lviSelectedItem = lvi;
                        int indexSelectedItem = lvi.Index;
                        aListView.Items.RemoveAt(indexSelectedItem);
                        aListView.Items.Insert(indexSelectedItem - 1, lviSelectedItem);
                    }
                }
                aListView.EndUpdate();

                aListView.Focus();
                aListView.SelectedItems[0].Focused = true;
                aListView.SelectedItems[0].EnsureVisible();
            }
        }

        public static void SelectedListViewItemDown(ListView aListView)
        {
            if (aListView.SelectedItems.Count > 0)
            {
                aListView.BeginUpdate();
                int indexMaxSelectedItem = aListView.SelectedItems[aListView.SelectedItems.Count - 1].Index;

                if (indexMaxSelectedItem < aListView.Items.Count - 1)
                {
                    for (int i = aListView.SelectedItems.Count - 1; i >= 0; i--)
                    {
                        ListViewItem lviSelectedItem = aListView.SelectedItems[i];
                        int indexSelectedItem = lviSelectedItem.Index;
                        aListView.Items.RemoveAt(indexSelectedItem);
                        aListView.Items.Insert(indexSelectedItem + 1, lviSelectedItem);
                    }
                }
                aListView.EndUpdate();

                aListView.Focus();
                aListView.SelectedItems[aListView.SelectedItems.Count - 1].Focused = true;
                aListView.SelectedItems[aListView.SelectedItems.Count - 1].EnsureVisible();
            }
        }

        public static string GetSystemDBName()
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

        public static ClientType GetSystemDBType()
        {
            ClientType returnValue = ClientType.ctNone;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(SystemFile.DBFile);

            XmlNode node = xmlDoc.FirstChild.SelectSingleNode("SystemDB");

            object[] DbString = new object[1];
            DbString[0] = node.FirstChild.Value;
            string systemDBType = "";
            object[] temp = GetDataBaseType(DbString);
            if (temp != null && temp[0].ToString() == "0")
                systemDBType = temp[1].ToString();
            switch (systemDBType)
            {
                case "1":
                    returnValue = ClientType.ctMsSql;
                    break;
                case "2":
                    returnValue = ClientType.ctOleDB;
                    break;
                case "3":
                    returnValue = ClientType.ctOracle;
                    break;
                case "4":
                    returnValue = ClientType.ctODBC;
                    break;
                case "5":
                    returnValue = ClientType.ctMySql;
                    break;
                case "6":
                    returnValue = ClientType.ctInformix;
                    break;
                case "7":
                    returnValue = ClientType.ctSybase;
                    break;
            }

            return returnValue;
        }

        //1. MSSql   2.OleDb   3.Oracle   4.ODBC   5.MySql   6.Informix   7.Sybase
        public static object[] GetDataBaseType(object[] objParam)
        {
            string aliasName = objParam[0].ToString();
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
    }
}