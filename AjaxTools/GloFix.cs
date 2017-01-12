using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.OracleClient;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using Srvtools;

namespace AjaxTools
{
    public class GloFix
    {
        const string SQL_GET_COLUMNTYPE = "SELECT SYSOBJECTS.NAME AS TABLENAME,SYSCOLUMNS.NAME AS COLUMNNAME,SYSTYPES.NAME AS DATATYPE FROM SYSOBJECTS,SYSCOLUMNS,SYSTYPES WHERE SYSOBJECTS.TYPE='U' AND SYSCOLUMNS.XTYPE=SYSTYPES.XTYPE AND SYSCOLUMNS.ID=SYSOBJECTS.ID AND SYSTYPES.NAME!='sysname' AND SYSOBJECTS.NAME='{0}' AND SYSCOLUMNS.NAME='{1}'";

        public static string ToHtmlColor(Color color)
        {
            if (color.Name == "0")
                return "0";
            string r = color.R.ToString("X");
            if (r.Length < 2)
                r = "0" + r;
            string g = color.G.ToString("X");
            if (g.Length < 2)
                g = "0" + g;
            string b = color.B.ToString("X");
            if (b.Length < 2)
                b = "0" + b;

            return "#" + r + g + b;
        }

        public static bool IsNumeric(Type dataType)
        {
            return IsNumeric(dataType.ToString());
        }

        public static bool IsNumeric(string dataType)
        {
            string type = dataType.ToLower();
            if (type == "system.uint" || type == "system.uint16" || type == "system.uint32" || type == "system.uint64"
              || type == "system.int" || type == "system.int16" || type == "system.int32" || type == "system.int64"
              || type == "system.single" || type == "system.double" || type == "system.decimal")
            {
                return true;
            }
            return false;
        }

        public static bool isNumericString(string value)
        {
            Regex r = new Regex(@"^\d+(\.)?\d*$");
            return r.IsMatch(value);
        }

        public static string ConvertToSqlCondition(string tableName, string columnName, string value)
        {
            string sql = string.Format(SQL_GET_COLUMNTYPE, tableName, columnName);
            DataTable table = CliUtils.ExecuteSql("GLModule", "cmdRefValUse", sql, true, CliUtils.fCurrentProject).Tables[0];
            if (table.Rows.Count == 1)
            {
                string type = table.Rows[0]["DATATYPE"].ToString();
                if (type == "smalldatetime" || type == "datetime" || type == "text" || type == "varchar" || type == "char" || type == "nvarchar" || type == "nchar")
                {
                    return string.Format("'{0}'", value.Replace("'", "''"));
                }
                else
                {
                    return value;
                }
            }
            return value;
        }

        public static string FormatESC(string value)
        {
            return value.Replace("\\", "\\\\").Replace("'", "\\'").Replace("\r", "\\r").Replace("\n", "\\n");
        }

        public static WebDataSet CreateDataSet(string WebDataSetID)
        {
            WebDataSet wds = new WebDataSet(true);
            XmlDocument xmlDoc = new XmlDocument();
            CultureInfo culture = new CultureInfo("vi-VN");
            string aspxName = EditionDifference.ActiveDocumentFullName();
            string resourceName = aspxName + @".vi-VN.resx";
            ResXResourceReader reader = new ResXResourceReader(resourceName);
            IDictionaryEnumerator enumerator = reader.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (enumerator.Key.ToString() == "WebDataSets")
                {
                    string sXml = (string)enumerator.Value;
                    xmlDoc.LoadXml(sXml);
                    break;
                }
            }
            if (xmlDoc != null)
            {
                XmlNode nWDSs = xmlDoc.SelectSingleNode("WebDataSets");
                if (nWDSs != null)
                {
                    string webDataSetID = WebDataSetID;
                    XmlNode nWDS = nWDSs.SelectSingleNode("WebDataSet[@Name='" + webDataSetID + "']");
                    if (nWDS != null)
                    {
                        string remoteName = "";
                        int packetRecords = 100;
                        bool active = false;
                        bool serverModify = false;

                        XmlNode nRemoteName = nWDS.SelectSingleNode("RemoteName");
                        if (nRemoteName != null)
                            remoteName = nRemoteName.InnerText;

                        XmlNode nPacketRecords = nWDS.SelectSingleNode("PacketRecords");
                        if (nPacketRecords != null)
                            packetRecords = nPacketRecords.InnerText.Length == 0 ? 100 : Convert.ToInt32(nPacketRecords.InnerText);

                        XmlNode nActive = nWDS.SelectSingleNode("Active");
                        if (nActive != null)
                            active = nActive.InnerText.Length == 0 ? false : Convert.ToBoolean(nActive.InnerText);

                        XmlNode nServerModify = nWDS.SelectSingleNode("ServerModify");
                        if (nServerModify != null)
                            serverModify = nServerModify.InnerText.Length == 0 ? false : Convert.ToBoolean(nServerModify.InnerText);
                        wds.RemoteName = remoteName;
                        wds.PacketRecords = packetRecords;
                        wds.ServerModify = serverModify;
                        wds.Active = true;
                    }
                }
            }
            return wds;
        }

        public static string GetConnString(String aliasName)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(SystemFile.DBFile);

            XmlNode node = xmlDoc.FirstChild.FirstChild.SelectSingleNode(aliasName);

            string DbString = node.Attributes["String"].Value.Trim();
            string Pwd = GloFix.GetPwdString(node.Attributes["Password"].Value.Trim());
            if (DbString.Length > 0 && Pwd.Length > 0 && Pwd != String.Empty)
            {
                if (DbString[DbString.Length - 1] != ';')
                    DbString = DbString + ";Password=" + Pwd;
                else
                    DbString = DbString + "Password=" + Pwd;
            }
            return DbString;
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

        public static int GetConnType(String aliasName)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(SystemFile.DBFile);

            XmlNode node = xmlDoc.FirstChild.FirstChild.SelectSingleNode(aliasName);

            string DbType = node.Attributes["Type"].Value.Trim();
            if (DbType != null && DbType.Length > 0)
            {
                return Convert.ToInt32(DbType);
            }
            return 0;
        }

        public static List<String> GetAllTablesList(IDbConnection conn)
        {
            List<String> tablesList = new List<string>();
            String sQL = "";
            if (conn is SqlConnection)
            {
                sQL = "select @@version as version";
                IDbCommand cmd0 = AllocateCommand(conn, sQL);
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                Object o = cmd0.ExecuteScalar();
                conn.Close();
                if (o.ToString().ToLower().IndexOf("microsoft sql server 2005") >= 0)
                {
                    sQL = @"select (
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
                    sQL = @"select(
                            case when (Charindex(' ',Rtrim(Ltrim(name)),0) != 0) then
		                        '[' + [name] + ']'
	                        else
		                        [name]
                            end
                            ) as name from sysobjects where xtype in ('u','U','v','V')  order by [name]";
                }
            }
            else if (conn is OdbcConnection)
                sQL = "select * from systables where (tabtype = 'T' or tabtype = 'V') and tabid >= 100 order by tabname";
            else if (conn is OracleConnection)
                sQL = "SELECT * FROM USER_OBJECTS WHERE OBJECT_TYPE = 'TABLE' OR OBJECT_TYPE = 'VIEW'order by OBJECT_NAME";
            else if (conn is OleDbConnection)
                sQL = "select * from sysobjects where type in ('u','U','v','V')  order by [name]";
            else if (conn.GetType().Name == "MySqlConnection")
                sQL = "show tables;";
            else if (conn.GetType().Name == "IfxConnection")
                sQL = "select * from systables where (tabtype = 'T' or tabtype = 'V') and tabid >= 100 order by tabname";

            IDbCommand cmd = AllocateCommand(conn, sQL);

            if (conn.State == ConnectionState.Closed)
            { conn.Open(); }

            IDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (reader.Read())
            {
                if (conn is SqlConnection)
                    tablesList.Add(reader["name"].ToString());
                else if (conn is OdbcConnection)
                    tablesList.Add(reader["tabname"].ToString());
                else if (conn is OracleConnection)
                    tablesList.Add(reader["OBJECT_NAME"].ToString());
                else if (conn is OleDbConnection)
                    tablesList.Add(reader["name"].ToString());
                else if (conn.GetType().Name == "MySqlConnection")
                    tablesList.Add(reader["name"].ToString());
                else if (conn.GetType().Name == "IfxConnection")
                    tablesList.Add(reader["tabname"].ToString());

            }
            conn.Close();
            return tablesList;
        }

        public static IDbConnection AllocateConn(string s, ClientType ct)
        {
            if (ct == ClientType.ctMsSql)
            {
                return new SqlConnection(s);
            }
            else if (ct == ClientType.ctODBC)
            {
                return new OdbcConnection(s);
            }
            else if (ct == ClientType.ctOleDB)
            {
                return new OleDbConnection(s);
            }
            else if (ct == ClientType.ctOracle)
            {
                return new OracleConnection(s);
            }
            else if (ct == ClientType.ctMySql)
            {
                String sPath = EEPRegistry.Server + "\\MySql.Data.dll";
                Assembly assembly = Assembly.LoadFrom(sPath);
                DbConnection Result = assembly.CreateInstance("MySql.Data.MySqlClient.MySqlConnection") as DbConnection;
                Result.ConnectionString = s;
                return Result;
            }
            else if (ct == ClientType.ctInformix)
            {
                String sPath = EEPRegistry.Server + "\\IBM.Data.Informix.dll";
                Assembly assembly = Assembly.LoadFrom(sPath);
                DbConnection Result = assembly.CreateInstance("IBM.Data.Informix.IfxConnection") as DbConnection;
                Result.ConnectionString = s;
                return Result;
            }
            else
            {
                return new SqlConnection(s);
            }
        }

        public static IDbCommand AllocateCommand(IDbConnection conn, string sQL)
        {
            if (conn is SqlConnection)
            {
                return new SqlCommand(sQL, (SqlConnection)conn);
            }
            else if (conn is OdbcConnection)
            {
                return new OdbcCommand(sQL, (OdbcConnection)conn);
            }
            else if (conn is OracleConnection)
            {
                return new OracleCommand(sQL, (OracleConnection)conn);
            }
            else if (conn is OleDbConnection)
            {
                return new OleDbCommand(sQL, (OleDbConnection)conn);
            }
            else if (conn.GetType().Name == "MySql.Data.MySqlClient.MySqlConnection")
            {
                String sPath = EEPRegistry.Server + "\\MySql.Data.dll";
                Assembly assembly = Assembly.LoadFrom(sPath);
                IDbCommand Result = assembly.CreateInstance("MySql.Data.MySqlClient.MySqlCommand") as IDbCommand;
                Result.Connection = conn;
                Result.CommandText = sQL;
                return Result;
            }
            else if (conn.GetType().Name == "IBM.Data.Informix.IfxConnection")
            {
                String sPath = EEPRegistry.Server + "\\IBM.Data.Informix.dll";
                Assembly assembly = Assembly.LoadFrom(sPath);
                IDbCommand Result = assembly.CreateInstance("IBM.Data.Informix.IfxCommand") as IDbCommand;
                Result.Connection = conn;
                Result.CommandText = sQL;
                return Result;
            }
            else return null;
        }

        public static DataRow FindRow(DataRowCollection rows, string type, string keyvalue)
        {
            DataRow row = null;
            if (type == "system.uint" || type == "system.uint32")
            {
                row = rows.Find(Convert.ToUInt32(keyvalue));
            }
            else if (type == "system.uint16")
            {
                row = rows.Find(Convert.ToUInt16(keyvalue));
            }
            else if (type == "system.uint64")
            {
                row = rows.Find(Convert.ToUInt64(keyvalue));
            }
            else if (type == "system.int" || type == "system.int32")
            {
                row = rows.Find(Convert.ToInt32(keyvalue));
            }
            else if (type == "system.int16")
            {
                row = rows.Find(Convert.ToInt16(keyvalue));
            }
            else if (type == "system.int64")
            {
                row = rows.Find(Convert.ToInt64(keyvalue));
            }
            else if (type == "system.single")
            {
                row = rows.Find(Convert.ToSingle(keyvalue));
            }
            else if (type == "system.double")
            {
                row = rows.Find(Convert.ToDouble(keyvalue));
            }
            else if (type == "system.decimal")
            {
                row = rows.Find(Convert.ToDecimal(keyvalue));
            }
            else if (type == "system.datetime")
            {
                row = rows.Find(Convert.ToDateTime(keyvalue));
            }
            else
            {
                row = rows.Find(keyvalue);
            }
            return row;
        }

        public static string InsertWhereCondition(string originalSql, string condition)
        {
            int inxGroupBy = originalSql.ToLower().IndexOf("group by");
            int inxOrderBy = originalSql.ToLower().IndexOf("order by");
            int insertPos = Math.Min(inxGroupBy, inxOrderBy);
            if (originalSql.ToLower().IndexOf("where") != -1)
            {
                if (inxGroupBy == -1 && inxOrderBy == -1 && insertPos == -1)
                    originalSql += " and " + condition;
                else
                {
                    if (insertPos == -1)
                    {
                        originalSql = originalSql.Insert(Math.Max(inxGroupBy, inxOrderBy), " and " + condition + " ");
                    }
                    else
                    {
                        originalSql = originalSql.Insert(insertPos, " and " + condition + " ");
                    }
                }
            }
            else
            {
                if (inxGroupBy == -1 && inxOrderBy == -1 && insertPos == -1)
                    originalSql += " where " + condition;
                else
                {
                    if (insertPos == -1)
                    {
                        originalSql = originalSql.Insert(Math.Max(inxGroupBy, inxOrderBy), " where " + condition + " ");
                    }
                    else
                    {
                        originalSql = originalSql.Insert(insertPos, " where " + condition + " ");
                    }
                }
            }
            return originalSql;
        }

        public static string GetWhereString(DataTable tab, IDictionary dickeys, string oper, string split)
        {
            StringBuilder builder = new StringBuilder();
            foreach (DictionaryEntry dic in dickeys)
            {
                Type type = tab.Columns[dic.Key.ToString()].DataType;
                if (GloFix.IsNumeric(type))
                {
                    builder.Append(dic.Key.ToString() + oper + dic.Value.ToString() + split);
                }
                else if (type == typeof(Boolean))
                {
                    builder.AppendFormat(dic.Key.ToString() + oper + dic.Value.ToString().ToLower() + split);
                }
                else
                {
                    builder.AppendFormat("{0}{1}'{2}'{3}", dic.Key, oper, dic.Value.ToString(), split);
                }
            }
            if (builder.ToString().EndsWith(split))
            {
                builder.Remove(builder.Length - split.Length, split.Length);
            }
            return builder.ToString();
        }

        public static DataTable GetPageTable(DataTable srcTable, int startIndex, int pagesize)
        {
            int rowCount = srcTable.Rows.Count;
            DataTable tab = srcTable.Clone();
            int maxIndex = Math.Min(startIndex + pagesize, rowCount);
            for (int i = startIndex; i < maxIndex; i++)
            {
                tab.ImportRow(srcTable.Rows[i]);
            }
            return tab;
        }

        public static DataRow[] GetPageRows(DataRow[] srcRows, int startIndex, int pagesize)
        {
            int maxIndex = Math.Min(startIndex + pagesize, srcRows.Length);
            DataRow[] rows = new DataRow[maxIndex - startIndex];
            Array.Copy(srcRows, startIndex, rows, 0, maxIndex - startIndex);
            return rows;
        }
    }

    interface IAjaxDataSource
    {
        string DataSourceID { get;set;}
    }

    interface IRefDataSource
    {
        string DataSourceID { get; set; }
        string DataTextField { get; set; }
        string DataValueField { get; set; }
    }

    interface IChildSet
    {
        AjaxBaseControl OwnerView { get; }
    }

    public abstract class AjaxBaseWebControl : WebControl
    {
        private Control GetAllCtrls(string strid, Control ct)
        {
            if (ct.ID == strid)
            {
                return ct;
            }
            else
            {
                if (ct.HasControls())
                {
                    foreach (Control ctchild in ct.Controls)
                    {
                        Control ctrtn = GetAllCtrls(strid, ctchild);
                        if (ctrtn != null)
                        {
                            return ctrtn;
                        }
                    }
                    return null;
                }
                else
                {
                    return null;
                }
            }
        }

        public object GetObjByID(string ObjID)
        {
            if (this.Site != null)
            {
                return GetAllCtrls(ObjID, this.Page);
            }
            else
            {
                if (this.Page.Form != null)
                    return GetAllCtrls(ObjID, this.Page.Form);
                else
                    return GetAllCtrls(ObjID, this.Page);
            }
        }
    }

    public abstract class AjaxBaseControl : Control
    {
        private Control GetAllCtrls(string strid, Control ct)
        {
            if (ct.ID == strid)
            {
                return ct;
            }
            else
            {
                if (ct.HasControls())
                {
                    foreach (Control ctchild in ct.Controls)
                    {
                        Control ctrtn = GetAllCtrls(strid, ctchild);
                        if (ctrtn != null)
                        {
                            return ctrtn;
                        }
                    }
                    return null;
                }
                else
                {
                    return null;
                }
            }
        }

        public object GetObjByID(string ObjID)
        {
            if (this.Site != null)
            {
                return GetAllCtrls(ObjID, this.Page);
            }
            else
            {
                if (this.Page.Form != null)
                    return GetAllCtrls(ObjID, this.Page.Form);
                else
                    return GetAllCtrls(ObjID, this.Page);
            }
        }
    }
}
