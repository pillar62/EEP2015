using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using System.Data;
using System.Xml;
using System.IO;
using EFClientTools.EFServerReference;
using System.Text;


public class DataSetHelper
{
    const string PREFIX_PATTERN = @"(?<=[\s\]\)]|^)";
    const string SUFFIX_PATTERN = @"(?=[\s\[\(]|$)";
    const string TABLE_PATTERN = @"(\w+\s*\.\s*)*(\w+|\[(\w+\$*\s*)+\])";

    public static bool IsPartInBracket(string sql, int index)
    {
        string leftString = sql.Substring(0, index);
        if (leftString.Split('[').Length != leftString.Split(']').Length)
        {
            return true;
        }
        if (leftString.Split('(').Length != leftString.Split(')').Length)
        {
            return true;
        }
        return false;
    }

    public static string InsertCount(string sql)
    {
        MatchCollection mcs = Regex.Matches(sql, PREFIX_PATTERN + @"from\s+" + TABLE_PATTERN, RegexOptions.IgnoreCase);
        foreach (Match mc in mcs)
        {
            if (!IsPartInBracket(sql, mc.Index))
            {
                MatchCollection matches = Regex.Matches(sql, PREFIX_PATTERN + @"order\b(\s+)\bby" + SUFFIX_PATTERN, RegexOptions.IgnoreCase);
                foreach (Match mco in matches)
                {
                    if (!IsPartInBracket(sql, mco.Index))
                    {
                        if (mco.Index > mc.Index)
                        {
                            sql = sql.Remove(mco.Index);
                            break;
                        }
                    }
                }

                return string.Format("SELECT COUNT(*) {0}", sql.Substring(mc.Index));
            }
        }
        return string.Empty;
        //throw new Exception("Sql is invalid.");
    }

    public static DataSet Deserialize(string xml)
    {
        XmlReaderSettings settings = new XmlReaderSettings();
        using (StringReader textReader = new StringReader(xml))
        {
            using (XmlReader xmlReader = XmlReader.Create(textReader))
            {
                DataSet dataset = new DataSet();
                dataset.ReadXml(xmlReader, XmlReadMode.DiffGram);
                return dataset;
            }
        }
    }

    public static DataSet GetDataSet(string packageName, string commandName, bool onlySchema)
    {
        return GetDataSet(packageName, commandName, new PacketInfo() { WhereParameters = new List<WhereParameter>(), OrderParameters = new List<OrderParameter>(), OnlySchema = true });
    }

    public static DataSet GetDataSet(string packageName, string commandName, PacketInfo packetInfo)
    { 
        var clientInfo = EFClientTools.ClientUtility.ClientInfo;
        var xml = (string)EFClientTools.ClientUtility.Client.GetDataSet(clientInfo, packageName, commandName, packetInfo);
        return Deserialize(xml);
    }

    public static DataSet ExecuteSql(string commandText, SDTableType tableType)
    {
        return ExecuteSql(new SQLCommandInfo() { CommandText = commandText }, tableType);
    }

    public static DataSet ExecuteSql(SQLCommandInfo sqlCommand, SDTableType tableType)
    {
        return ExecuteSql(sqlCommand, new PacketInfo() { WhereParameters = new List<WhereParameter>(), OrderParameters = new List<OrderParameter>() }, tableType);
    }

    public static DataSet ExecuteSchema(string commandText, SDTableType tableType)
    {
        return ExecuteSql(new SQLCommandInfo() { CommandText = commandText }, new PacketInfo() { WhereParameters = new List<WhereParameter>(), OrderParameters = new List<OrderParameter>(), OnlySchema = true }, tableType);
    }

    public static DataSet ExecuteSql(string commandText, PacketInfo packetInfo, SDTableType tableType)
    {
        return ExecuteSql(new SQLCommandInfo() { CommandText = commandText }, packetInfo, tableType);
    }

    public static DataSet ExecuteSql(SQLCommandInfo sqlCommand, PacketInfo packetInfo, SDTableType tableType)
    {
        var clientInfo = EFClientTools.ClientUtility.ClientInfo;
        var xml = (string)EFClientTools.ClientUtility.Client.SDExcuteSql(clientInfo, sqlCommand, packetInfo, tableType);
        return Deserialize(xml);
    }

    public static DataSet GetSolutions(string developerID)
    {
        var clientInfo = new ClientInfo() { SDDeveloperID = developerID };
        var xml = (string)EFClientTools.ClientUtility.Client.SDGetSolutions(clientInfo);
        return Deserialize(xml);
    }

    public static DataSet GetSolution(string solutionID, string developerID)
    {
        var clientInfo = new ClientInfo() { SDDeveloperID = developerID, Solution = solutionID };
        var xml = (string)EFClientTools.ClientUtility.Client.SDGetSolutions(clientInfo);
        return Deserialize(xml);
    }

    public static int ExecuteCommand(string commandText, SDTableType tableType)
    {
        return ExecuteCommand(new SQLCommandInfo() { CommandText = commandText }, tableType);
    }

    public static int ExecuteCommand(SQLCommandInfo sqlCommand, SDTableType tableType)
    {
        var clientInfo = EFClientTools.ClientUtility.ClientInfo;
        var sqlCommands = new List<SQLCommandInfo>();
        sqlCommands.Add(sqlCommand);
        return EFClientTools.ClientUtility.Client.SDExcuteCommands(clientInfo, sqlCommands, tableType);

    }

    public static int ExecuteCommands(List<SQLCommandInfo> sqlCommands, SDTableType tableType)
    {
        var clientInfo = EFClientTools.ClientUtility.ClientInfo;
        return EFClientTools.ClientUtility.Client.SDExcuteCommands(clientInfo, sqlCommands, tableType);

    }

    public static void UpdateTable(string tableName, List<UpdateRow> updateRows, SDTableType tableType)
    {
        var clientInfo = EFClientTools.ClientUtility.ClientInfo;
        EFClientTools.ClientUtility.Client.SDUpdateTable(clientInfo, tableName, updateRows, tableType);
    }
}

public class SQLHelper
{
    public const string SELECT_USER_DETAIL_SQL = "SELECT *, GROUPS.GROUPNAME FROM USERGROUPS LEFT JOIN GROUPS ON USERGROUPS.GROUPID = GROUPS.GROUPID WHERE USERGROUPS.USERID = @USERID";
    public const string SELECT_GROUP_DETAIL_SQL = "SELECT *, USERS.USERNAME FROM USERGROUPS LEFT JOIN USERS ON USERGROUPS.USERID = USERS.USERID WHERE USERGROUPS.GROUPID = @GROUPID";
    public const string SELECT_ACCOUNT_LOG_SQL = "SELECT u.UserID, u.UserName,u.Email,u.Description, u.GroupID, u.Password, u.SYSTYPE, u.Password AS OPassword,u.ExpiryDate,u.CreateDate,u.Owner, MAX(ud.LoginTime) AS LastLoginTime, COUNT(ud.UserID) AS LoginCount, sum(DATEDIFF(\"mi\",LoginTime, LogoutTime)) as Minutes FROM SYS_SDUSERS AS u LEFT JOIN SYS_SDUSERS_LOG AS ud ON ud.UserID = u.UserID WHERE {0} GROUP BY ud.UserID, u.UserID, u.UserName, u.GroupID, u.Password,u.Email,u.Description, u.SYSTYPE, u.ExpiryDate, u.CreateDate,u.Owner";
    public const string SELECT_ACCOUNT_LOG_DETAIL_SQL = "SELECT ud.UserID, LoginTime, LogoutTime, DATEDIFF(\"mi\",LoginTime, LogoutTime) AS Minutes, COUNT(logType) as Changes FROM SYS_SDUSERS_LOG AS ud left join SYS_SDUSERLOGDetail AS ld ON UpdateDateTime > LoginTime AND UpdateDateTime < LogoutTime AND ud.UserID = ld.UserID WHERE ud.UserID =  @UserID GROUP BY ud.UserID, LoginTime, LogoutTime ORDER BY LoginTime DESC";
    public const string SELECT_ACCOUNT_WORK_SQL = "SELECT * FROM SYS_SDUSERLOGDetail WHERE UserID = @UserID AND UpdateDateTime > @LoginTime AND UpdateDateTime < @LogoutTime";
    public const string SELECT_USER_LOG_DETAIL_SQL = "SELECT * FROM SYSSQLLOG WHERE USERID = @UserID AND DEVELOPERID = @DeveloperID ORDER BY LOGDATETIME DESC";
    public const string SELECT_MENU_GROUPS_SQL = "SELECT GROUPMENUS.*, GROUPS.GROUPNAME FROM GROUPMENUS LEFT JOIN GROUPS ON GROUPS.GROUPID = GROUPMENUS.GROUPID WHERE GROUPMENUS.MENUID = @MENUID";
    public const string SELECT_MENU_USERS_SQL = "SELECT USERMENUS.*, USERS.USERNAME FROM USERMENUS LEFT JOIN USERS ON USERS.USERID = USERMENUS.USERID WHERE USERMENUS.MENUID = @MENUID";
    public const string DEPLOY_SYS_WEBPAGES_SQL = "INSERT INTO SYS_WEBPAGES (PageName, PageType, Description, Content, UserID, SolutionID, SERVERDLL, UpdateDateTime) SELECT PageName, PageType, Description, Content, UserID, @DeploySolution, SERVERDLL, UpdateDateTime FROM SYS_WEBPAGES WHERE PageName = @PageName AND PageType = @PageType AND UserID= @UserID AND SolutionID = @SolutionID";
    public const string COPY_SYS_WEBPAGES_SQL = "INSERT INTO SYS_WEBPAGES (PageName, PageType, Description, Content, UserID, SolutionID, SERVERDLL, UpdateDateTime) SELECT @CopyPageName, PageType, Description, Content, UserID, SolutionID, SERVERDLL, UpdateDateTime FROM SYS_WEBPAGES WHERE PageName = @PageName AND PageType = @PageType AND UserID= @UserID AND SolutionID = @SolutionID";
    public const string SELECT_INDEXES_SQL = "SELECT NAME FROM sys.indexes WHERE object_id = object_id(@object_id) AND is_primary_key = 1";
    public const string SELECT_GROUP_SOLUTION_SQL = "SELECT * FROM SYS_SDSOLUTIONS WHERE UserID IN (SELECT UserID FROM SYS_SDUSERS WHERE GroupID = @GroupID AND UserID != @UserID) AND Exclusive != 'Y'";
    public const string SELECT_CAPACITY_SQL = @"SELECT DB_NAME(database_id) AS [DatabaseName],[Name] AS [LogicalName],[Physical_Name] AS [PhysicalName],((size * 8) / 1024) AS [Size],[differential_base_time] AS [Differential Base Time] FROM sys.master_files WHERE DB_NAME(database_id) = @DBNAME";
    public const string SELECT_CAPACITY_MYSQL = @"SELECT round(sum(data_length/1024/1024),0) as data from information_schema.tables where table_schema=@DBNAME;";
    public const string SELECT_MESSAGE_SQL = @"SELECT * FROM SYS_SDMESSAGE WHERE ((Type = 'P' AND UserID = @UserID) OR (Type = 'S')) AND ExpiryDate >= @ExpiryDate AND ID NOT IN (SELECT MESSAGE_ID FROM SYS_SDMessageUsers WHERE UserID = @UserID)";
    public const string SELECT_SYS_ORGROLES_SQL = @"SELECT * FROM SYS_ORGROLES LEFT JOIN GROUPS ON SYS_ORGROLES.ROLE_ID=GROUPS.GROUPID WHERE SYS_ORGROLES.ORG_NO=@ORG_NO";
    public const string UPDATE_SYS_ROLES_AGENT_SQL = @"UPDATE SYS_ROLES_AGENT SET ROLE_ID=@ROLE_ID,AGENT=@AGENT,FLOW_DESC=@FLOW_DESC,START_DATE=@START_DATE,START_TIME=@START_TIME,END_DATE=@END_DATE,END_TIME=@END_TIME,PAR_AGENT=@PAR_AGENT,REMARK=@REMARK WHERE ROLE_ID=@ROLE_ID_O AND AGENT=@AGENT_O AND FLOW_DESC=@FLOW_DESC_O AND START_DATE=@START_DATE_O AND START_TIME=@START_TIME_O AND END_DATE=@END_DATE_O AND END_TIME=@END_TIME_O AND PAR_AGENT=@PAR_AGENT_O AND REMARK=@REMARK_O";
    public const string SELECT_GROUPMENUCONTROL_SQL = @"SELECT GROUPMENUCONTROL.GROUPID,GROUPMENUCONTROL.MENUID,GROUPMENUCONTROL.CONTROLNAME,MENUTABLECONTROL.DESCRIPTION,GROUPMENUCONTROL.TYPE,GROUPMENUCONTROL.ENABLED,GROUPMENUCONTROL.VISIBLE,GROUPMENUCONTROL.ALLOWADD,GROUPMENUCONTROL.ALLOWUPDATE,GROUPMENUCONTROL.ALLOWDELETE,GROUPMENUCONTROL.ALLOWPRINT FROM GROUPMENUCONTROL LEFT JOIN MENUTABLECONTROL ON GROUPMENUCONTROL.MENUID=MENUTABLECONTROL.MENUID AND GROUPMENUCONTROL.CONTROLNAME=MENUTABLECONTROL.CONTROLNAME WHERE GROUPMENUCONTROL.GROUPID = @GROUPID AND GROUPMENUCONTROL.MENUID = @MENUID";
    public const string SELECT_GROUPMENUCONTROL_ORACLE = @"SELECT GROUPMENUCONTROL.GROUPID,GROUPMENUCONTROL.MENUID,GROUPMENUCONTROL.CONTROLNAME,MENUTABLECONTROL.DESCRIPTION,GROUPMENUCONTROL.TYPE,GROUPMENUCONTROL.ENABLED,GROUPMENUCONTROL.VISIBLE,GROUPMENUCONTROL.ALLOWADD,GROUPMENUCONTROL.ALLOWUPDATE,GROUPMENUCONTROL.ALLOWDELETE,GROUPMENUCONTROL.ALLOWPRINT FROM GROUPMENUCONTROL LEFT JOIN MENUTABLECONTROL ON GROUPMENUCONTROL.MENUID=MENUTABLECONTROL.MENUID AND GROUPMENUCONTROL.CONTROLNAME=MENUTABLECONTROL.CONTROLNAME WHERE GROUPMENUCONTROL.GROUPID = :GROUPID AND GROUPMENUCONTROL.MENUID = :MENUID";
    public const string SELECT_USERMENUCONTROL_SQL = @"SELECT USERMENUCONTROL.USERID,USERMENUCONTROL.MENUID,USERMENUCONTROL.CONTROLNAME,MENUTABLECONTROL.DESCRIPTION,USERMENUCONTROL.TYPE,USERMENUCONTROL.ENABLED,USERMENUCONTROL.VISIBLE,USERMENUCONTROL.ALLOWADD,USERMENUCONTROL.ALLOWUPDATE,USERMENUCONTROL.ALLOWDELETE,USERMENUCONTROL.ALLOWPRINT FROM USERMENUCONTROL LEFT JOIN MENUTABLECONTROL on USERMENUCONTROL.MENUID=MENUTABLECONTROL.MENUID AND USERMENUCONTROL.CONTROLNAME=MENUTABLECONTROL.CONTROLNAME WHERE USERMENUCONTROL.USERID = @USERID AND USERMENUCONTROL.MENUID = @MENUID";
    public const string SELECT_USERMENUCONTROL_ORACLE = @"SELECT USERMENUCONTROL.USERID,USERMENUCONTROL.MENUID,USERMENUCONTROL.CONTROLNAME,MENUTABLECONTROL.DESCRIPTION,USERMENUCONTROL.TYPE,USERMENUCONTROL.ENABLED,USERMENUCONTROL.VISIBLE,USERMENUCONTROL.ALLOWADD,USERMENUCONTROL.ALLOWUPDATE,USERMENUCONTROL.ALLOWDELETE,USERMENUCONTROL.ALLOWPRINT FROM USERMENUCONTROL LEFT JOIN MENUTABLECONTROL on USERMENUCONTROL.MENUID=MENUTABLECONTROL.MENUID AND USERMENUCONTROL.CONTROLNAME=MENUTABLECONTROL.CONTROLNAME WHERE USERMENUCONTROL.USERID = :USERID AND USERMENUCONTROL.MENUID = :MENUID";
    public const string SELECT_USERPWD_SQL = @"SELECT COUNT(*) FROM USERS WHERE USERID = @UserId AND (PWD = '' or PWD is null)";

    public const string UPDATE_SDUSERS_APPROVE_SQL = "UPDATE SYS_SDUSERS SET Active='Y' WHERE  CONVERT(nvarchar(100),SYS_SDUSERS.CreateDate,23) = @CreateDate";
    public const string DELETE_EXPIRED_MESSAGE_SQL = "DELETE FROM SYS_SDMESSAGE WHERE ExpiryDate < @ExpiryDate";

    public const string INSERT_TABLE_SQL = "CREATE TABLE {0} ({1})";
    public const string DELETE_TABLE_SQL = "IF object_id('{0}', 'U') IS NOT NULL DROP TABLE {1}";
    public const string DELETE_TABLE_MYSQL = "DROP TABLE IF EXISTS {0}";
    public const string ALTER_TABLE_DROP_COLUMN_SQL = "ALTER TABLE {0} DROP COLUMN {1}";
    public const string ALTER_TABLE_ALTER_COLUMN_SQL = "ALTER TABLE {0} ALTER COLUMN {1} {2} {3}";
    public const string ALTER_TABLE_CHANGE_COLUMN_SQL = "EXEC sp_rename '{0}.{1}', '{2}', 'COLUMN'";
    public const string ALTER_TABLE_ADD_COLUMN_SQL = "ALTER TABLE {0} ADD {1} {2} {3}";
    public const string ALTER_TABLE_DROP_CONSTRAINT_SQL = "ALTER TABLE {0} DROP CONSTRAINT {1}";
    public const string ALTER_TABLE_ADD_PRIMARY_SQL = "ALTER TABLE {0} ADD PRIMARY KEY ({1})";


    public const string CREATE_DATABASE_SQL = "IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = '{0}') CREATE DATABASE {0}";
    public const string CREATE_DATABASE_MYSQL = "CREATE DATABASE IF NOT EXISTS {0}";
    public const string DELETE_DATABASE_SQL = @"use master;ALTER DATABASE {0} SET OFFLINE WITH ROLLBACK IMMEDIATE;ALTER DATABASE {0} SET ONLINE;drop database {0}";
    public const string DELETE_DATABASE_MYSQL = @"DROP DATABASE IF EXISTS {0}";
    public const string USE_DATABASE_SQL = "USE {0};";
    public const string SET_DATABASE_OWNER = "EXEC dbo.sp_changedbowner @loginame = N'sduser', @map = false";

    public const string BACKUP_DATABASE_SQL = @"use master;BACKUP DATABASE {0} TO disk='{1}' WITH FORMAT, NAME = N'{2}-完整 数据库 备份', SKIP, NOREWIND, NOUNLOAD, STATS = 10";
    public const string RESTORE_DATABASE_SQL = @"use master;Alter Database {0} SET SINGLE_USER With ROLLBACK IMMEDIATE;RESTORE DATABASE {0} FROM disk='{1}' WITH FILE = 1, NOUNLOAD, REPLACE, STATS = 10;Alter Database {0} SET MULTI_USER";

    public const string GETVIEWTEXT = "SELECT definition FROM sysobjects a,sys.all_sql_modules b WHERE a.id = b.object_id and name =@name";

    public const string DELETE_MENUFAVOR_SQL = "DELETE FROM MENUFAVOR WHERE USERID=@USERID AND GROUPNAME=@GROUPNAME";
    public const string DELETE_MENUFAVOR_ORACLE = "DELETE FROM MENUFAVOR WHERE USERID=:USERID AND GROUPNAME=:GROUPNAME";
    public const string INSERT_MENUFAVOR_SQL = "INSERT INTO MENUFAVOR (MENUID,CAPTION,USERID,ITEMTYPE,GROUPNAME) VALUES (@MENUID,@CAPTION,@USERID,@ITEMTYPE,@GROUPNAME)";
    public const string INSERT_MENUFAVOR_ORACLE = "INSERT INTO MENUFAVOR (MENUID,CAPTION,USERID,ITEMTYPE,GROUPNAME) VALUES (:MENUID,:CAPTION,:USERID,:ITEMTYPE,:GROUPNAME)";

    public static Dictionary<string, object> CreateParameters(string key, object value)
    {
        var parameters = new Dictionary<string, object>();
        parameters.Add(key, value);
        return parameters;
    }

    public static Dictionary<string, object> CreateParameters(IEnumerable<string> keys, IEnumerable<object> values)
    {
        var keyArray = keys.ToArray();
        var valueArray = values.ToArray();
        var parameters = new Dictionary<string, object>();
        for (int i = 0; i < keyArray.Length && i < valueArray.Length; i++)
        {
            parameters.Add(keyArray[i], valueArray[i]);
        }
        return parameters;
    }
    
    public static string CreateSelectSql(string table, IEnumerable<string> selectColumns, IEnumerable<string> whereColumns)
    {
        var sql = new StringBuilder();
        var selectPart = string.Empty;
        if (selectColumns != null)
        {
            selectPart = string.Join(",", selectColumns);
        }
        if(string.IsNullOrEmpty(selectPart))
        {
            selectPart = "*";
        }
        sql.AppendFormat("SELECT {1} FROM {0}", table, selectPart);
        sql.Append(CreateWherePart(whereColumns));
        return sql.ToString();
    }

    public static string CreateInsertSql(string table, IEnumerable<string> insertColumns)
    {
        return string.Format("INSERT INTO {0} ({1}) VALUES ({2})", table, string.Join(",", insertColumns), string.Join(",", insertColumns.Select(c=> string.Format("@{0}", c))));
    }

    public static string CreateUpdateSql(string table, IEnumerable<string> updateColumns, IEnumerable<string> whereColumns)
    {
        var sql = new StringBuilder();
        sql.AppendFormat("UPDATE {0} SET {1}", table, string.Join(",", updateColumns.Select(c => string.Format("{0}=@{0}", c))));
        sql.Append(CreateWherePart(whereColumns));
        return sql.ToString();
    }

    public static string CreateDeleteSql(string table, IEnumerable<string> whereColumns)
    {
        var sql = new StringBuilder();
        sql.AppendFormat("DELETE FROM {0}", table);
        sql.Append(CreateWherePart(whereColumns));
        return sql.ToString();
    }

    private static string CreateWherePart(IEnumerable<string> whereColumns)
    {
        var sql = new StringBuilder();
        var mark = "@";
        if (EFClientTools.ClientUtility.ClientInfo.DatabaseType == "Oracle")
        {
            mark = ":";
        }
        if (whereColumns != null)
        {
            var wherePart = string.Join(" AND ", whereColumns.Select(c => string.Format("{0}={1}{0}", c, mark)));
            if (!string.IsNullOrEmpty(wherePart))
            {
                sql.AppendFormat(" WHERE {0}", wherePart);
            }
        }
        return sql.ToString();
    }
}