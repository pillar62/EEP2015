using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using EFServerTools;
using EFWCFModule;
using System.Data.EntityClient;
using Database = EFWCFModule.EEPAdapter.DatabaseProvider;
using System.Data.Objects.DataClasses;
using System.Data.Objects;
using System.Data;
using System.IO;
using EFWCFModule.EEPAdapter;
using System.Xml;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;

namespace EFGlobalModule
{
    public partial class Component : EFModule, EFWCFModule.IGlobalModule
    {
        public Component()
        {
            InitializeComponent();
        }

        public Component(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        #region IGlobalModule Members
        private string EncryptValue(string value)
        {
            var encryptBuilder = new StringBuilder();
            foreach (var c in value)
            {
                encryptBuilder.Append(((int)c).ToString("X"));
            }
            return encryptBuilder.ToString();
        }

        ClientInfo EFWCFModule.IGlobalModule.CheckUser()
        {
            #region DataSet
            if (ClientInfo.UseDataSet)
            {
                var clientInfo = this.ClientInfo;
                if (clientInfo == null)
                {
                    throw new ArgumentNullException("clientInfo");
                }

                if (!string.IsNullOrEmpty(clientInfo.SecurityKey) && string.IsNullOrEmpty(clientInfo.UserID))//singlesignon 通过key免密码登入
                {
                    var infos = PublicKey.CheckEncryptKey(clientInfo.SecurityKey);
                    clientInfo.UserID = infos[0];
                    clientInfo.UserName = infos[1];
                    clientInfo.Database = infos[2];
                    clientInfo.Solution = infos[3];
                    clientInfo.IPAddress = infos[4];
                    clientInfo.DatabaseType = infos[5];

                    var dbAlias = string.Empty;
                    if (Database.GetSplitSystemTable(ClientInfo.Database, ClientInfo.SDDeveloperID))
                    {
                        dbAlias = Database.GetSystemDatabase(ClientInfo.SDDeveloperID);
                    }
                    else
                    {
                        dbAlias = ClientInfo.Database;
                    }
                    SetGroups(clientInfo, dbAlias);

                    if (!SrvGL.IsUserLogined(clientInfo.UserID, string.Empty, string.Empty))
                    {
                        SrvGL.LogUser(clientInfo.UserID.ToLower(), clientInfo.UserName, clientInfo.IPAddress.ToString(), 1);
                    }
                    clientInfo.SecurityKey = string.Empty;
                    //clientInfo.LogonResult = LogonResult.Logoned; 防止重复登入
                    return clientInfo;
                }
                else
                {

                    if (string.IsNullOrEmpty(clientInfo.UserID))
                    {
                        throw new ArgumentNullException("clientInfo.UserID");
                    }

                    var dbAlias = string.Empty;

                    if (Database.GetSplitSystemTable(ClientInfo.Database, ClientInfo.SDDeveloperID))
                    {
                        dbAlias = Database.GetSystemDatabase(ClientInfo.SDDeveloperID);
                    }
                    else
                    {
                        dbAlias = ClientInfo.Database;
                    }

                    string sql = "SELECT * FROM USERS WHERE USERID='" + clientInfo.UserID + "'";
                    IEFService service = new EFService();
                    var dsUsers = Database.ExecuteDataSet(dbAlias, ClientInfo.SDDeveloperID, sql, null) as DataSet;
                    if (dsUsers != null && dsUsers.Tables[0].Rows.Count > 0 && dsUsers.Tables[0].Rows[0]["USERID"].ToString() != "")
                    {
                        DataRow drUsers = dsUsers.Tables[0].Rows[0];
                        string autoLogin = drUsers["AUTOLOGIN"].ToString();
                        string pwd = drUsers["PWD"].ToString();
                        string msad = drUsers["MSAD"].ToString();
                        if (string.Compare(autoLogin, "X", true) == 0)//用户被禁用
                        {
                            clientInfo.LogonResult = LogonResult.UserDisabled;
                            return clientInfo;
                        }
                        if (string.Compare(msad, "Y", true) == 0) //AD
                        {
                            if (string.IsNullOrEmpty(clientInfo.SecurityKey))
                            {
                                var valid = false;
                                foreach (var domain in ServerConfig.Domains)
                                {
                                    var ad = new ADClass() { ADPath = "LDAP://" + domain.Path, ADUser = domain.User, ADPassword = domain.Password };
                                    valid = ad.IsUserValid(clientInfo.UserID, clientInfo.Password);
                                    if (valid)
                                    {
                                        break;
                                    }
                                }
                                if (!valid)
                                {
                                    clientInfo.LogonResult = LogonResult.PasswordError;//密码错误
                                    return clientInfo;
                                }
                            }
                            else
                            {
                                var domainValid = false;
                                foreach (var domain in ServerConfig.Domains)
                                {
                                    if (string.Compare(clientInfo.SecurityKey, EncryptValue(domain.Path)) == 0)
                                    {
                                        domainValid = true;
                                        break;
                                    }
                                }
                                if (!domainValid)
                                {
                                    clientInfo.LogonResult = LogonResult.PasswordError;//密码错误
                                    return clientInfo;
                                }
                            }
                        }
                        else if (!CheckPassword(clientInfo.UserID, clientInfo.Password, pwd))
                        {
                            clientInfo.LogonResult = LogonResult.PasswordError;//密码错误
                            return clientInfo;
                        }

                        clientInfo.LogonResult = LogonResult.Logoned;
                        clientInfo.DatabaseType = Database.GetProviderType(ClientInfo.Database, ClientInfo.SDDeveloperID).ToString();
                        clientInfo.UserName = drUsers["USERNAME"].ToString();//设置用户名
                        SetGroups(clientInfo, dbAlias);
                        clientInfo.AUTOLOGIN = autoLogin;
                        clientInfo.SecurityKey = PublicKey.GetEncryptKey(clientInfo.UserID, clientInfo.UserName, clientInfo.Database, clientInfo.Solution, clientInfo.DatabaseType, clientInfo.IPAddress);

                    }
                    else
                    {
                        clientInfo.LogonResult = LogonResult.UserNotFound;//找不到用户
                    }
                }
                return clientInfo;
            }
            #endregion
            #region Entity
            else
            {
                var clientInfo = this.ClientInfo;
                if (clientInfo == null)
                {
                    throw new ArgumentNullException("clientInfo");
                }
                if (string.IsNullOrEmpty(clientInfo.UserID))
                {
                    throw new ArgumentNullException("clientInfo.UserID");
                }
                using (var context = new Entities(CreateEntityConnection(clientInfo.Database, true)))
                {
                    context.Connection.Open();
                    var user = (from u in context.USERS where u.USERID.Equals(clientInfo.UserID, StringComparison.OrdinalIgnoreCase) select u).FirstOrDefault();
                    if (user == null)
                    {
                        clientInfo.LogonResult = LogonResult.UserNotFound;//找不到用户
                    }
                    else
                    {
                        if (string.Compare(user.AUTOLOGIN, "X", true) == 0)//用户被禁用
                        {
                            clientInfo.LogonResult = LogonResult.UserDisabled;
                        }
                        if (string.Compare(user.MSAD, "Y", true) == 0) //AD
                        {


                            //ADClass.ADPath = "LDAP://" + ServerConfig.DomainPath;
                            //ADClass.ADUser = ServerConfig.DomainUser;
                            //ADClass.ADPassword = ServerConfig.DomainPassword;

                            //if (!ADClass.IsUserValid(clientInfo.UserID, clientInfo.Password))
                            //{
                            clientInfo.LogonResult = LogonResult.PasswordError;//密码错误
                            //}
                        }
                        else if (!CheckPassword(clientInfo.UserID, clientInfo.Password, user.PWD))
                        {
                            clientInfo.LogonResult = LogonResult.PasswordError;//密码错误
                        }
                        else
                        {
                            clientInfo.LogonResult = LogonResult.Logoned;
                            //clientInfo.Password = string.Empty;//清空密码
                            clientInfo.UserName = user.USERNAME;//设置用户名
                            clientInfo.Groups = new List<GroupInfo>();
                            var groups = from g in context.GROUPS
                                         join ug in context.USERGROUPS on g.GROUPID equals ug.GROUPID
                                         where ug.USERID.Equals(clientInfo.UserID, StringComparison.OrdinalIgnoreCase)
                                         select g;
                            foreach (var group in groups)//设置群组
                            {
                                var groupInfo = new GroupInfo() { ID = group.GROUPID, Name = group.GROUPNAME };
                                groupInfo.Type = string.Compare(group.ISROLE, "Y", true) == 0 ? GroupType.Role : GroupType.Normal;
                                clientInfo.Groups.Add(groupInfo);
                            }
                        }
                    }
                    return clientInfo;
                }
            }
            #endregion
        }

        public String ResetUserPassword(string email)
        {
            if (ClientInfo.UseDataSet)
            {
                var clientInfo = this.ClientInfo;
                if (clientInfo == null)
                {
                    throw new ArgumentNullException("clientInfo");
                }


                if (string.IsNullOrEmpty(clientInfo.UserID))
                {
                    throw new ArgumentNullException("clientInfo.UserID");
                }

                var dbAlias = string.Empty;

                if (Database.GetSplitSystemTable(ClientInfo.Database, ClientInfo.SDDeveloperID))
                {
                    dbAlias = Database.GetSystemDatabase(ClientInfo.SDDeveloperID);
                }
                else
                {
                    dbAlias = ClientInfo.Database;
                }

                string sql = "SELECT * FROM USERS WHERE USERID='" + clientInfo.UserID + "'";
                IEFService service = new EFService();
                var dsUsers = Database.ExecuteDataSet(dbAlias, ClientInfo.SDDeveloperID, sql, null) as DataSet;
                if (dsUsers.Tables[0].Rows.Count > 0 && dsUsers.Tables[0].Rows[0]["USERID"].ToString() != "")
                {
                    string autoLogin = dsUsers.Tables[0].Rows[0]["AUTOLOGIN"].ToString();
                    if (string.Compare(autoLogin, "X", true) == 0)//用户被禁用
                    {
                        throw new Exception("UserNotFound");
                    }
                    if (dsUsers.Tables[0].Rows[0]["EMAIL"].ToString().ToLower() == email.ToLower())
                    {
                        var random = new Random(DateTime.Now.Millisecond);
                        var newPassword = random.Next(100000, 1000000).ToString();
                        var sqlCommands = new List<SQLCommandInfo>();
                        char[] p = new char[] { };
                        bool q = EFWCFModule.EEPAdapter.Encrypt.EncryptPassword(clientInfo.UserID, newPassword, 10, ref p, false);
                        String pwd = new String(p);
                        sqlCommands.Add(new SQLCommandInfo()
                        {
                            CommandText = "UPDATE USERS SET PWD='" + pwd + "' WHERE USERID='" + clientInfo.UserID + "'",
                            Parameters = null
                        });
                        Database.ExecuteCommands(dbAlias, ClientInfo.SDDeveloperID, sqlCommands);
                        return newPassword;
                    }
                    else
                    {
                        throw new Exception("EmailError");
                    }
                }
                throw new Exception("UserNotFound");
            }
            throw new Exception("UserNotFound");
        }

        ClientInfo EFWCFModule.IGlobalModule.LogOnDevice(string userID, string deviceID, bool check, string developerID)
        {
            var database = Database.GetSystemDatabase(developerID);
            var device = deviceID.Split(';')[0];
            var sql = string.Format("SELECT * FROM UserDevices WHERE UserID='{0}' AND UUID = '{1}'", userID, device);
            var dataset = Database.ExecuteDataSet(database, developerID, sql, null);
            if (check)
            {
                if (dataset.Tables[0].Rows.Count > 0)
                {
                    DateTime expiryDate = DateTime.Today.AddHours(24);
                    if (dataset.Tables[0].Rows[0]["ExpiryDate"] != DBNull.Value)
                    {
                        expiryDate = (DateTime)dataset.Tables[0].Rows[0]["ExpiryDate"];
                    }
                    expiryDate = expiryDate.AddDays(1);
                    if ((DateTime.Now - expiryDate).TotalSeconds > 0)
                    {
                        return new ClientInfo() { LogonResult = LogonResult.UserDisabled };
                    }
                    else
                    {
                        database = deviceID.Split(';')[1];
                        if (Database.GetSplitSystemTable(database, null))
                        {
                            database = Database.GetSystemDatabase(null);
                        }
                        var clientInfo = new ClientInfo() { UserID = userID, LogonResult = LogonResult.Logoned, Database = database };
                        this.ClientInfo = clientInfo;
                        if (!SrvGL.IsUserLogined(userID, null, null))
                        {
                            SrvGL.LogUser(userID, userID, "app", 1);
                        }
                        SetGroups(clientInfo, database);
                        clientInfo.Database = deviceID.Split(';')[1];
                        return clientInfo;
                    }
                }
                else
                {
                    return new ClientInfo() { LogonResult = LogonResult.NotLogoned };
                }
            }
            else
            {
                if (dataset.Tables[0].Rows.Count > 0)
                {
                    sql = string.Format("UPDATE UserDevices SET LoginDate='{0:yyyy-MM-dd HH:mm:ss}' WHERE UserID='{1}' AND UUID = '{2}'", DateTime.Now, userID, deviceID);
                }
                else
                {
                    sql = string.Format("INSERT INTO UserDevices (UserID, UUID, LoginDate) VALUES ('{0}', '{1}', '{2:yyyy-MM-dd HH:mm:ss}')", userID, deviceID, DateTime.Now);
                }

                Database.ExecuteCommand(database, developerID, sql);
                return new ClientInfo();
            }
        }

        void EFWCFModule.IGlobalModule.RegisterDevice(string userID, string deviceID, string regID, string tokenID, string developerID)
        {
            var database = Database.GetSystemDatabase(developerID);
            var sqls = new List<SQLCommandInfo>();

            if (!string.IsNullOrEmpty(regID))
            {
                sqls.Add(new SQLCommandInfo() { CommandText = string.Format("UPDATE UserDevices SET RegID = '' WHERE RegID = '{0}'", regID) });
            }
            if (!string.IsNullOrEmpty(tokenID))
            {
                sqls.Add(new SQLCommandInfo() { CommandText = string.Format("UPDATE UserDevices SET TokenID = '' WHERE TokenID = '{0}'", tokenID) });
            }
            sqls.Add(new SQLCommandInfo() { CommandText = string.Format("UPDATE UserDevices SET RegID= '{0}', TokenID = '{1}'  WHERE UserID='{2}' AND UUID = '{3}'", regID, tokenID, userID, deviceID) });
            Database.ExecuteCommands(database, developerID, sqls);
        }

        List<string> IGlobalModule.GetRegIDs(List<string> userIDs, string developerID)
        {
            var database = Database.GetSystemDatabase(developerID);
            var regIDs = new List<string>();
            var dataSet = Database.ExecuteDataSet(database, developerID, string.Format("SELECT * FROM UserDevices WHERE UserID IN ({0})", string.Join(",", userIDs.Select(c => string.Format("'{0}'", c)))), "");
            for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
            {
                if (!string.IsNullOrEmpty(dataSet.Tables[0].Rows[i]["RegID"].ToString()))
                {
                    regIDs.Add(dataSet.Tables[0].Rows[i]["RegID"].ToString());
                }
            }
            return regIDs;
        }

        List<string> IGlobalModule.GetTokenIDs(List<string> userIDs, string developerID)
        {
            var database = Database.GetSystemDatabase(developerID);
            var regIDs = new List<string>();
            var dataSet = Database.ExecuteDataSet(database, developerID, string.Format("SELECT * FROM UserDevices WHERE UserID IN ({0})", string.Join(",", userIDs.Select(c => string.Format("'{0}'", c)))), "");
            for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
            {
                if (!string.IsNullOrEmpty(dataSet.Tables[0].Rows[i]["TokenID"].ToString()))
                {
                    regIDs.Add(dataSet.Tables[0].Rows[i]["TokenID"].ToString());
                }
            }
            return regIDs;
        }

        void IGlobalModule.SendMessage(List<string> users, string subject, string body)
        {
            var dbAlias = string.Empty;
            if (Database.GetSplitSystemTable(ClientInfo.Database, ClientInfo.SDDeveloperID))
            {
                dbAlias = Database.GetSystemDatabase(ClientInfo.SDDeveloperID);
            }
            else
            {
                dbAlias = ClientInfo.Database;
            }
            var commands = new List<SQLCommandInfo>();
            foreach (var user in users)
            {
                commands.Add(new SQLCommandInfo()
                {
                    CommandText = string.Format("INSERT INTO SYS_MESSENGER (USERID, MESSAGE, PARAS,SENDTIME, STATUS) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}')"
                        , user, subject, body, DateTime.Now.ToString("yyyyMMddHHmmss"), "N")
                });
            }
            Database.ExecuteCommands(dbAlias, ClientInfo.SDDeveloperID, commands);
        }

        void IGlobalModule.DeleteMessage(List<string> dateTimes)
        {
            var dbAlias = string.Empty;
            if (Database.GetSplitSystemTable(ClientInfo.Database, ClientInfo.SDDeveloperID))
            {
                dbAlias = Database.GetSystemDatabase(ClientInfo.SDDeveloperID);
            }
            else
            {
                dbAlias = ClientInfo.Database;
            }
            var commands = new List<SQLCommandInfo>();
            foreach (var dateTime in dateTimes)
            {
                commands.Add(new SQLCommandInfo() { CommandText = string.Format("DELETE FROM SYS_MESSENGER WHERE USERID = '{0}' AND SENDTIME = '{1}'", ClientInfo.UserID, dateTime) });
            }
            Database.ExecuteCommands(dbAlias, ClientInfo.SDDeveloperID, commands);
        }

        void IGlobalModule.ReadMessage(string dateTime)
        {
            var dbAlias = string.Empty;
            if (Database.GetSplitSystemTable(ClientInfo.Database, ClientInfo.SDDeveloperID))
            {
                dbAlias = Database.GetSystemDatabase(ClientInfo.SDDeveloperID);
            }
            else
            {
                dbAlias = ClientInfo.Database;
            }
            Database.ExecuteCommand(dbAlias, ClientInfo.SDDeveloperID, string.Format("UPDATE SYS_MESSENGER SET STATUS = '' WHERE USERID = '{0}' AND SENDTIME = '{1}'", ClientInfo.UserID, dateTime));
        }

        string IGlobalModule.GetMessages()
        {
            var dbAlias = string.Empty;
            if (Database.GetSplitSystemTable(ClientInfo.Database, ClientInfo.SDDeveloperID))
            {
                dbAlias = Database.GetSystemDatabase(ClientInfo.SDDeveloperID);
            }
            else
            {
                dbAlias = ClientInfo.Database;
            }
            var dataSet = Database.ExecuteDataSet(dbAlias, ClientInfo.SDDeveloperID, string.Format("SELECT * FROM SYS_MESSENGER WHERE USERID = '{0}'", ClientInfo.UserID), string.Empty);
            return Serialize<DataSet>(dataSet);
        }


        private void SetGroups(ClientInfo clientInfo, string dbAlias)
        {
            clientInfo.Groups = new List<GroupInfo>();
            var userid = clientInfo.UserID;
            //string orgKind = "0";//2013/12/11 mark
            string orgKind = (clientInfo.OrgKind == null || clientInfo.OrgKind == "") ? "0" : clientInfo.OrgKind;//add by lu 2013/12/11
            StringBuilder role = new StringBuilder();
            StringBuilder orgRole = new StringBuilder();
            StringBuilder orgShare = new StringBuilder();
            StringBuilder groupList = new StringBuilder();

            if (clientInfo.CurrentGroup != null)
            {
                role.Append(clientInfo.CurrentGroup);
                var groupInfo = new GroupInfo() { ID = clientInfo.CurrentGroup, Name = "" };
                groupInfo.Type = GroupType.Role;
                clientInfo.Groups.Add(groupInfo);
                groupList.Append("'" + clientInfo.CurrentGroup + "'");
            }
            else
            {
                string getGroupSql = "SELECT * FROM GROUPS LEFT JOIN USERGROUPS ON GROUPS.GROUPID = USERGROUPS.GROUPID WHERE USERGROUPS.USERID ='" + clientInfo.UserID + "'";
                DataSet dsGroups = Database.ExecuteDataSet(dbAlias, ClientInfo.SDDeveloperID, getGroupSql, null) as DataSet;
                if (dsGroups != null && dsGroups.Tables[0].Rows.Count > 0 && dsGroups.Tables[0].Rows[0][0].ToString() != "")
                {
                    foreach (DataRow group in dsGroups.Tables[0].Rows)//设置群组
                    {
                        var groupInfo = new GroupInfo() { ID = group["GROUPID"].ToString(), Name = group["GROUPNAME"].ToString() };
                        groupInfo.Type = string.Compare(group["ISROLE"].ToString(), "Y", true) == 0 ? GroupType.Role : GroupType.Normal;
                        clientInfo.Groups.Add(groupInfo);
                        if (groupInfo.Type != GroupType.Normal)
                        {
                            if (role.Length > 0)
                            {
                                role.Append(';');
                                groupList.Append(',');
                            }
                            role.Append((string)group["GROUPID"]);
                            groupList.Append(string.Format("'{0}'", group["GROUPID"]));
                        }
                    }
                }
            }

            if (role.Length > 0)
            {
                orgRole.Append(role);
                string getOrgNoSql = string.Format("Select ORG_NO,ORG_MAN,GROUPS.GROUPNAME From SYS_ORG left join GROUPS on SYS_ORG.ORG_MAN = GROUPS.GROUPID Where ORG_MAN IN({0}) and ORG_KIND='{1}'", groupList, orgKind);
                DataSet dsOrgNos = Database.ExecuteDataSet(dbAlias, ClientInfo.SDDeveloperID, getOrgNoSql, null) as DataSet;
                StringBuilder orglist = new StringBuilder();
                if (dsOrgNos != null && dsOrgNos.Tables[0].Rows.Count > 0 && dsOrgNos.Tables[0].Rows[0][0].ToString() != "")
                {
                    foreach (DataRow org in dsOrgNos.Tables[0].Rows)
                    {
                        if (orglist.Length > 0)
                        {
                            orglist.Append(',');
                        }
                        orglist.Append(string.Format("'{0}'", org["ORG_NO"]));
                    }
                }
                if (orglist.Length > 0)//找到Org_No
                {
                    StringBuilder orgParentlist = new StringBuilder();
                    orgParentlist.Append(orglist);
                    while (true)//递归找到所有的子org
                    {
                        string getOrgParentSql = string.Format("Select ORG_NO,ORG_MAN,UPPER_ORG,GROUPS.GROUPNAME From SYS_ORG left join GROUPS on SYS_ORG.ORG_MAN = GROUPS.GROUPID Where UPPER_ORG IN ({0})", orgParentlist);
                        DataSet dsOrgParents = Database.ExecuteDataSet(dbAlias, ClientInfo.SDDeveloperID, getOrgParentSql, null) as DataSet;
                        orgParentlist = new StringBuilder();

                        if (dsOrgParents != null && dsOrgParents.Tables[0].Rows.Count > 0 && dsOrgParents.Tables[0].Rows[0][0].ToString() != "")
                        {
                            foreach (DataRow org in dsOrgParents.Tables[0].Rows)
                            {
                                orglist.Append(',');
                                orglist.Append(string.Format("'{0}'", org["ORG_NO"]));
                                if (orgParentlist.Length > 0)
                                {
                                    orgParentlist.Append(',');
                                }
                                orgParentlist.Append(string.Format("'{0}'", org["ORG_NO"]));
                                //orgRole.Append(';');
                                //orgRole.Append((string)org["ORG_MAN"]);
                                var groupInfo = new GroupInfo() { ID = org["ORG_MAN"].ToString(), Name = org["GROUPNAME"].ToString() };
                                groupInfo.Type = GroupType.Org;
                                if (clientInfo.Groups.Where(c => c.ID == org["ORG_MAN"].ToString()).Count() == 0)
                                {
                                    clientInfo.Groups.Add(groupInfo);
                                }

                            }
                        }
                        if (orgParentlist.Length == 0)//找到底了
                        {
                            break;
                        }

                    }
                    string getRoleIDSql = string.Format("Select ROLE_ID,GROUPS.GROUPNAME From SYS_ORGROLES left join GROUPS on SYS_ORGROLES.ROLE_ID = GROUPS.GROUPID WHERE ORG_NO IN ({0})", orglist);
                    DataSet dsRoleIDs = Database.ExecuteDataSet(dbAlias, ClientInfo.SDDeveloperID, getRoleIDSql, null) as DataSet;

                    if (dsRoleIDs != null && dsRoleIDs.Tables[0].Rows.Count > 0 && dsRoleIDs.Tables[0].Rows[0][0].ToString() != "")
                    {
                        foreach (DataRow org in dsRoleIDs.Tables[0].Rows)
                        {
                            //orgRole.Append(';');
                            //orgRole.Append((string)org["ROLE_ID"]);
                            var groupInfo = new GroupInfo() { ID = org["ROLE_ID"].ToString(), Name = org["GROUPNAME"].ToString() };
                            groupInfo.Type = GroupType.Org;
                            if (clientInfo.Groups.Where(c => c.ID == org["ROLE_ID"].ToString()).Count() == 0)
                            {
                                clientInfo.Groups.Add(groupInfo);
                            }
                        }
                    }
                    //orgShare.Append(orgRole);
                }
                else
                {
                    orgShare.Append(role);
                    string getRoleIDSql = string.Format("Select ROLE_ID,GROUPS.GROUPNAME From SYS_ORGROLES left join GROUPS on SYS_ORGROLES.ROLE_ID = GROUPS.GROUPID WHERE ORG_NO IN (Select ORG_NO From SYS_ORGROLES Where ROLE_ID IN ({0}))", groupList);
                    DataSet dsRoleIDs = Database.ExecuteDataSet(dbAlias, ClientInfo.SDDeveloperID, getRoleIDSql, null) as DataSet;

                    if (dsRoleIDs != null && dsRoleIDs.Tables[0].Rows.Count > 0 && dsRoleIDs.Tables[0].Rows[0][0].ToString() != "")
                    {
                        foreach (DataRow org in dsRoleIDs.Tables[0].Rows)
                        {
                            //orgShare.Append(';');
                            //orgShare.Append((string)org["ROLE_ID"]);
                            var groupInfo = new GroupInfo() { ID = org["ROLE_ID"].ToString(), Name = org["GROUPNAME"].ToString() };
                            groupInfo.Type = GroupType.OrgShare;
                            if (clientInfo.Groups.Where(c => c.ID == org["ROLE_ID"].ToString()).Count() == 0)
                            {
                                clientInfo.Groups.Add(groupInfo);
                            }
                        }
                    }
                }
            }
        }


        List<EntityObject> EFWCFModule.IGlobalModule.GetColumnDefination(string assemblyName, string commandName, string entityTypeName)
        {
            var processer = new EEPRemoteModule();
            var tableName = entityTypeName;
            var tableName2 = entityTypeName;
            if (tableName.IndexOf('.') >= 0)
            {
                tableName2 = tableName.Substring(tableName.IndexOf('.') + 1);
                tableName2 = tableName2.Trim(' ', '[', ']');
            }
            tableName = tableName.Trim(' ', '[', ']');
            var coldefSql = string.Format("SELECT * FROM {0} WHERE TABLE_NAME ='{1}' OR TABLE_NAME ='{2}'", "COLDEF", tableName, tableName2);
            var obj = processer.ExecuteSql(ClientInfo.ToArray(new PacketInfo() { Count = -1, WhereParameters = new List<WhereParameter>(), OrderParameters = new List<OrderParameter>() }), assemblyName, commandName, coldefSql, true);
            if (obj[0].Equals(0))
            {
                var dataset = (DataSet)Decompress((byte[])obj[1]);
                dataset.Tables[0].TableName = "COLDEF";
                var entityConverter = new EntityConverter();
                return entityConverter.FromDataSet(dataset).ToList();
                //return dataset;
            }
            else
            {
                throw new Exception((string)obj[1]);
            }
        }

        public static DataSet Decompress(byte[] buff)
        {
            MemoryStream source = new MemoryStream(buff);
            MemoryStream destination = new MemoryStream();
            Decompress(source, destination);//解压缩

            BinaryFormatter formater = new BinaryFormatter();
            destination.Seek(0, SeekOrigin.Begin);
            var surrogate = formater.Deserialize(destination);//反序列化

            DataSet dataset = new DataSet();
            surrogate.GetType().GetMethod("ReadSchemaIntoDataSet").Invoke(surrogate, new object[] { dataset });
            surrogate.GetType().GetMethod("ReadDataIntoDataSet").Invoke(surrogate, new object[] { dataset });
            //surrogate.ReadSchemaIntoDataSet(dataset);
            //surrogate.ReadDataIntoDataSet(dataset);//DataSetSurrogate
            return dataset;
        }

        private static void Decompress(Stream source, Stream destination)
        {
            using (GZipStream input = new GZipStream(source, CompressionMode.Decompress))
            {

                Pump(input, destination);
            }
        }

        private static void Pump(Stream input, Stream output)
        {

            byte[] bytes = new byte[4096];

            int n;

            while ((n = input.Read(bytes, 0, bytes.Length)) != 0)
            {
                output.Write(bytes, 0, n);
            }

        }

        List<EntityObject> EFWCFModule.IGlobalModule.GetColumnDefination(string entityTypeName)
        {
            #region DataSet
            if (ClientInfo.UseDataSet)
            {
                var clientInfo = this.ClientInfo;
                if (clientInfo == null)
                {
                    throw new ArgumentNullException("clientInfo");
                }
                var tableName = entityTypeName;
                var tableName2 = entityTypeName;
                if (tableName.IndexOf('.') >= 0)
                {
                    tableName2 = tableName.Substring(tableName.IndexOf('.') + 1);
                }

                var database = string.IsNullOrEmpty(clientInfo.Database) ? Database.GetSystemDatabase(clientInfo.SDDeveloperID) : clientInfo.Database;
                string coldefSql = string.Format("SELECT * FROM {0} WHERE TABLE_NAME ='{1}' OR TABLE_NAME ='{2}'", "COLDEF", tableName, tableName2);
                try
                {
                    var dataset = Database.ExecuteDataSet(database, clientInfo.SDDeveloperID, coldefSql, "COLDEF");
                    if (dataset == null || dataset.Tables.Count == 0 || dataset.Tables[0].Rows.Count == 0)
                    {
                        tableName = tableName.Trim(' ', '[', ']');
                        coldefSql = string.Format("SELECT * FROM {0} WHERE TABLE_NAME ='{1}' OR TABLE_NAME ='{2}'", "COLDEF", tableName, tableName2);
                        dataset = Database.ExecuteDataSet(database, clientInfo.SDDeveloperID, coldefSql, "COLDEF");
                    }
                    var entityConverter = new EntityConverter();
                    return entityConverter.FromDataSet(dataset).ToList();
                }
                catch
                {
                    return null;
                }
            }
            #endregion
            #region Entity
            else
            {
                var clientInfo = this.ClientInfo;
                if (clientInfo == null)
                {
                    throw new ArgumentNullException("clientInfo");
                }
                var database = string.IsNullOrEmpty(clientInfo.Database) ? Database.GetSystemDatabase(clientInfo.SDDeveloperID) : clientInfo.Database;

                using (var context = new Entities(CreateEntityConnection(database, false)))
                {
                    try
                    {
                        context.Connection.Open();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(string.Format("Database: {0} is not available", database));
                    }

                    try
                    {
                        string sql = string.Format("Select value c from {0}.{1} as c where c.TABLE_NAME = '{2}'", context.DefaultContainerName, "COLDEF", entityTypeName);
                        ObjectQuery<EntityObject> query = context.CreateQuery<EntityObject>(sql);


                        //var coldefs = from c in context.COLDEF where c.TABLE_NAME == entityTypeName select c;
                        return query.ToList();
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            #endregion
        }

        List<EntityObject> EFWCFModule.IGlobalModule.GetDataByTableNameWhere(object[] param)
        {
            String tableName = String.Empty;
            if (param.Length > 0 && param[0] != null)
                tableName = param[0].ToString();
            String whereString = String.Empty;
            if (param.Length > 1 && param[1] != null && param[1].ToString().Trim() != String.Empty)
                whereString = " WHERE " + param[1].ToString();

            #region DataSet
            if (ClientInfo.UseDataSet)
            {
                var clientInfo = this.ClientInfo;
                if (clientInfo == null)
                {
                    throw new ArgumentNullException("clientInfo");
                }

                var database = string.IsNullOrEmpty(clientInfo.Database) ? Database.GetSystemDatabase(clientInfo.SDDeveloperID) : clientInfo.Database;
                string coldefSql = string.Format("SELECT * FROM {0} {1}", tableName, whereString);
                try
                {
                    var dataset = Database.ExecuteDataSet(database, clientInfo.SDDeveloperID, coldefSql, tableName);
                    var entityConverter = new EntityConverter();
                    return entityConverter.FromDataSet(dataset).ToList();
                }
                catch
                {
                    return null;
                }
            }
            #endregion
            #region Entity
            else
            {
                var clientInfo = this.ClientInfo;
                if (clientInfo == null)
                {
                    throw new ArgumentNullException("clientInfo");
                }
                var database = string.IsNullOrEmpty(clientInfo.Database) ? Database.GetSystemDatabase(clientInfo.SDDeveloperID) : clientInfo.Database;

                using (var context = new Entities(CreateEntityConnection(database, false)))
                {
                    try
                    {
                        context.Connection.Open();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(string.Format("Database: {0} is not available", database));
                    }

                    try
                    {
                        string sql = string.Format("Select value c from {0}.{1} as c {2}", context.DefaultContainerName, tableName, whereString);
                        ObjectQuery<EntityObject> query = context.CreateQuery<EntityObject>(sql);


                        //var coldefs = from c in context.COLDEF where c.TABLE_NAME == entityTypeName select c;
                        return query.ToList();
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            #endregion
        }

        List<SolutionInfo> EFWCFModule.IGlobalModule.GetSolutions()
        {
            #region DataSet
            if (ClientInfo.UseDataSet)
            {
                var solutions = new List<SolutionInfo>();
                DataSet ds = new DataSet();
                string strSql = "select * from MENUITEMTYPE";
                var dataset = Database.ExecuteDataSet(Database.GetSystemDatabase(ClientInfo.SDDeveloperID), ClientInfo.SDDeveloperID, strSql, "MENUITEMTYPE");
                var entityConverter = new EntityConverter();
                var menuItemTypes = entityConverter.FromDataSet(dataset).OfType<MENUITEMTYPE>().ToList();
                foreach (var menuItemType in menuItemTypes)
                {
                    solutions.Add(new SolutionInfo() { ID = menuItemType.ITEMTYPE, Name = menuItemType.ITEMNAME, DefaultDatabase = menuItemType.DBALIAS });
                }
                return solutions;
            }
            #endregion
            #region Entity
            else if (ClientInfo.SDDeveloperID != null)
            {
                var solutions = new List<SolutionInfo>();
                string SDDeveloperID = ClientInfo.SDDeveloperID;
                ClientInfo.SDDeveloperID = null;
                using (var context = new Entities(CreateEntityConnection(Database.SystemDatabase, true)))
                {
                    context.Connection.Open();
                    var groups = from gr in context.SYS_SDUSERS
                                 where gr.USERID == SDDeveloperID
                                 select gr.GROUPID;
                    if (groups.Count<string>() > 0 && groups.ToList()[0] != null)
                    {

                        var users = from us in context.SYS_SDUSERS
                                    where groups.Contains(us.GROUPID)
                                    select us.USERID;
                        var solutionControl = from sr in context.SYS_SDSOLUTIONS
                                              where users.Contains(sr.USERID)
                                              select sr;
                        foreach (var obj in solutionControl)
                        {
                            solutions.Add(new SolutionInfo()
                            {
                                ID = obj.SOLUTIONID,
                                Name = obj.SOLUTIONNAME,
                                DefaultDatabase = obj.ALIASOPTIONS,
                                LogOnImage = obj.LOGONIMAGE,
                                BGStartColor = obj.BGSTARTCOLOR,
                                BGEndColor = obj.BGENDCOLOR
                            });
                        }
                    }
                    else
                    {
                        var solutionControl = from sr in context.SYS_SDSOLUTIONS
                                              where sr.USERID == SDDeveloperID
                                              select sr;
                        foreach (var obj in solutionControl)
                        {
                            solutions.Add(new SolutionInfo()
                            {
                                ID = obj.SOLUTIONID,
                                Name = obj.SOLUTIONNAME,
                                DefaultDatabase = obj.ALIASOPTIONS,
                                LogOnImage = obj.LOGONIMAGE,
                                BGStartColor = obj.BGSTARTCOLOR,
                                BGEndColor = obj.BGENDCOLOR
                            });
                        }
                    }
                }
                ClientInfo.SDDeveloperID = SDDeveloperID;
                return solutions;
            }
            else
            {
                var solutions = new List<SolutionInfo>();
                using (var context = new Entities(CreateEntityConnection(Database.GetSystemDatabase(ClientInfo.SDDeveloperID), false)))
                {
                    context.Connection.Open();
                    var menuItemTypes = from c in context.MENUITEMTYPE select c;
                    foreach (var menuItemType in menuItemTypes)
                    {
                        solutions.Add(new SolutionInfo() { ID = menuItemType.ITEMTYPE, Name = menuItemType.ITEMNAME, DefaultDatabase = menuItemType.DBALIAS });
                    }
                }
                return solutions;
            }
            #endregion
        }

        public int AutoSeqMenuID(object[] param)
        {
            int returnValue = 0;
            var clientInfo = this.ClientInfo;
            if (clientInfo == null)
            {
                throw new ArgumentNullException("clientInfo");
            }
            #region DataSet
            if (ClientInfo.UseDataSet)
            {
                string database = ClientInfo.Database;
                if (Database.GetSplitSystemTable(ClientInfo.Database, ClientInfo.SDDeveloperID))
                {
                    database = Database.GetSystemDatabase(ClientInfo.SDDeveloperID);
                }
                String strSql = String.Empty;
                DataBaseType dbtype = EFWCFModule.EEPAdapter.DatabaseProvider.GetProviderType(database, ClientInfo.SDDeveloperID);
                switch (dbtype)
                {
                    case DataBaseType.MsSql:
                        strSql = "select max(convert(int,MENUID)) from MENUTABLE where isnumeric(MENUID)=1";
                        break;
                    case DataBaseType.Oracle:
                        strSql = "select max(to_number(MENUID)) from MENUTABLE";
                        break;
                    case DataBaseType.ODBC:
                    case DataBaseType.Informix:
                        strSql = "select max(MENUID) from MENUTABLE";
                        break;
                    case DataBaseType.MySql:
                        strSql = "select max(cast(MENUID as signed)) from MENUTABLE";
                        break;
                    case DataBaseType.OleDB:
                        strSql = "select max(convert(int,MENUID)) from MENUTABLE";
                        break;
                }
                if (param.Length > 0 && param[0].ToString().ToLower() == "true")
                {
                    var dataset = Database.ExecuteDataSet(ClientInfo.Database, ClientInfo.SDDeveloperID, strSql, "MENUTABLE");
                    if (dataset.Tables[0].Rows[0][0] != null)
                        int.TryParse(dataset.Tables[0].Rows[0][0].ToString(), out returnValue);
                }
                else
                {
                    var dataset = Database.ExecuteDataSet(Database.GetSystemDatabase(ClientInfo.SDDeveloperID), ClientInfo.SDDeveloperID, strSql, "MENUTABLE");
                    if (dataset.Tables[0].Rows[0][0] != null)
                        int.TryParse(dataset.Tables[0].Rows[0][0].ToString(), out returnValue);
                }

            #endregion
            }
            return returnValue + 1;
        }

        public void SaveDataToTable(object[] param, String tableName)
        {
            var clientInfo = this.ClientInfo;
            if (clientInfo == null)
            {
                throw new ArgumentNullException("clientInfo");
            }
            #region DataSet
            if (ClientInfo.UseDataSet)
            {
                string database = ClientInfo.Database;
                if (Database.GetSplitSystemTable(ClientInfo.Database, ClientInfo.SDDeveloperID))
                {
                    database = Database.GetSystemDatabase(ClientInfo.SDDeveloperID);
                }
                String strSql = String.Empty;
                switch (tableName)
                {
                    case "MENUTABLE":
                        MENUTABLE menutable = (MENUTABLE)param[0];
                        strSql = String.Format("DELETE FROM MENUTABLE WHERE MENUID='{0}'", menutable.MENUID);
                        Database.ExecuteCommand(database, ClientInfo.SDDeveloperID, strSql);
                        strSql = String.Format("INSERT INTO MENUTABLE (MENUID, CAPTION, PARENT, PACKAGE, MODULETYPE, ITEMPARAM, FORM, ITEMTYPE, SEQ_NO, IMAGEURL,ISSHOWMODAL,VERSIONNO,CAPTION0,CAPTION1,CAPTION2,CAPTION3,CAPTION4,CAPTION5,CAPTION6,CAPTION7,ISSERVER,OWNER) " +
                                                "VALUES('{0}','{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}')",
                                                menutable.MENUID, menutable.CAPTION, menutable.PARENT, menutable.PACKAGE, menutable.MODULETYPE, menutable.ITEMPARAM, menutable.FORM, menutable.ITEMTYPE, menutable.SEQ_NO, menutable.IMAGEURL, menutable.ISSHOWMODAL, menutable.VERSIONNO,
                                                menutable.CAPTION0, menutable.CAPTION1, menutable.CAPTION2, menutable.CAPTION3, menutable.CAPTION4, menutable.CAPTION5, menutable.CAPTION6, menutable.CAPTION7, menutable.ISSERVER, menutable.OWNER);
                        Database.ExecuteCommand(database, ClientInfo.SDDeveloperID, strSql);
                        break;
                    case "MENUTABLECONTROL":
                        MENUTABLECONTROL menucontrol = (MENUTABLECONTROL)param[0];
                        strSql = String.Format("DELETE FROM MENUTABLECONTROL WHERE MENUID='{0}' AND CONTROLNAME='{1}'", menucontrol.MENUID, menucontrol.CONTROLNAME);
                        Database.ExecuteCommand(database, ClientInfo.SDDeveloperID, strSql);
                        strSql = String.Format("INSERT INTO MENUTABLECONTROL (MENUID,CONTROLNAME,DESCRIPTION,TYPE) VALUES('{0}','{1}','{2}','{3}')",
                                                menucontrol.MENUID, menucontrol.CONTROLNAME, menucontrol.DESCRIPTION, menucontrol.TYPE);
                        Database.ExecuteCommand(database, ClientInfo.SDDeveloperID, strSql);
                        break;
                    case "SYS_LANGUAGE":
                        SYS_LANGUAGE sys_language = (SYS_LANGUAGE)param[0];
                        strSql = String.Format("DELETE FROM SYS_LANGUAGE WHERE IDENTIFICATION='{0}' AND KEYS='{1}'", sys_language.IDENTIFICATION, sys_language.KEYS);
                        Database.ExecuteCommand(database, ClientInfo.SDDeveloperID, strSql);
                        strSql = String.Format("INSERT INTO SYS_LANGUAGE (IDENTIFICATION,KEYS,EN,CHT,CHS,HK,JA,KO,LAN1,LAN2) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')",
                                                sys_language.IDENTIFICATION, sys_language.KEYS,
                                                sys_language.EN, sys_language.CHT, sys_language.CHS, sys_language.HK, sys_language.JA, sys_language.KO, sys_language.LAN1, sys_language.LAN2);
                        if (Database.GetProviderType(database, ClientInfo.SDDeveloperID) == DataBaseType.MsSql)
                        {
                            strSql = String.Format("INSERT INTO SYS_LANGUAGE (IDENTIFICATION,KEYS,EN,CHT,CHS,HK,JA,KO,LAN1,LAN2) VALUES('{0}','{1}',N'{2}',N'{3}',N'{4}',N'{5}',N'{6}',N'{7}',N'{8}',N'{9}')",
                                           sys_language.IDENTIFICATION, sys_language.KEYS,
                                           sys_language.EN, sys_language.CHT, sys_language.CHS, sys_language.HK, sys_language.JA, sys_language.KO, sys_language.LAN1, sys_language.LAN2);
                        }
                        Database.ExecuteCommand(database, ClientInfo.SDDeveloperID, strSql);
                        break;
                    case "GROUPMENUS":
                        string sMenuId = param[0].ToString();
                        strSql = String.Format("DELETE FROM GROUPMENUS WHERE MENUID='{0}'", sMenuId);
                        Database.ExecuteCommand(database, ClientInfo.SDDeveloperID, strSql);
                        foreach (var item in param[1].ToString().Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            strSql = String.Format("INSERT INTO GROUPMENUS (MENUID, GROUPID) VALUES('{0}','{1}')",
                                                    sMenuId, item);
                            Database.ExecuteCommand(database, ClientInfo.SDDeveloperID, strSql);
                        }
                        break;
                    case "USERMENUS":
                        string sMenuIdU = param[0].ToString();
                        strSql = String.Format("DELETE FROM USERMENUS WHERE MENUID='{0}'", sMenuIdU);
                        Database.ExecuteCommand(database, ClientInfo.SDDeveloperID, strSql);
                        foreach (var item in param[1].ToString().Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            strSql = String.Format("INSERT INTO USERMENUS (MENUID, USERID) VALUES('{0}','{1}')",
                                                    sMenuIdU, item);
                            Database.ExecuteCommand(database, ClientInfo.SDDeveloperID, strSql);
                        }
                        break;
                }
            }
            #endregion
            #region Entity
            else if (ClientInfo.SDDeveloperID != null)
            {
                //using (var context = new Entities(CreateEntityConnection(Database.GetSystemDatabase(clientInfo.SDDeveloperID), true)))
                using (var context = new Entities(CreateEntityConnection(clientInfo.Database, true)))
                {
                    context.Connection.Open();

                    switch (tableName)
                    {
                        case "SYS_REFVAL":
                            SYS_REFVAL refval = (SYS_REFVAL)param[0];
                            refval.EntityKey = context.CreateEntityKey(tableName, refval);
                            try
                            {
                                //var lrefvals = (from r in context.SYS_REFVAL
                                //                where r.REFVAL_NO.Equals(refval.REFVAL_NO)
                                //                select r).ToList();
                                SYS_REFVAL refval1 = context.GetObjectByKey(refval.EntityKey) as SYS_REFVAL;
                                context.DeleteObject(refval1);
                                context.SaveChanges();
                            }
                            catch (Exception ex)
                            {

                            }
                            context.AddObject(tableName, refval);
                            break;
                        case "MENUTABLECONTROL":
                            if (param != null)
                            {
                                MENUTABLECONTROL menucontrol = (MENUTABLECONTROL)param[0];
                                menucontrol.EntityKey = context.CreateEntityKey(tableName, menucontrol);
                                try
                                {
                                    MENUTABLECONTROL item1 = context.GetObjectByKey(menucontrol.EntityKey) as MENUTABLECONTROL;
                                    context.DeleteObject(item1);
                                    context.SaveChanges();
                                }
                                catch (Exception ex)
                                {

                                }
                                context.AddObject(tableName, menucontrol);

                                //List<MENUTABLECONTROL> menucontrol = (List<MENUTABLECONTROL>)param;
                                //foreach (MENUTABLECONTROL item in menucontrol)
                                //{
                                //    item.EntityKey = context.CreateEntityKey("MENUTABLECONTROL", item);
                                //    try
                                //    {
                                //        MENUTABLECONTROL item1 = context.GetObjectByKey(item.EntityKey) as MENUTABLECONTROL;
                                //        context.DeleteObject(item1);
                                //        context.SaveChanges();
                                //    }
                                //    catch (Exception ex)
                                //    {

                                //    }
                                //    context.AddObject("MENUTABLECONTROL", item);
                                //}
                            }
                            break;
                        case "SYSEEPLOG":
                            SYSEEPLOG syseeplog = (SYSEEPLOG)param[0];
                            syseeplog.EntityKey = context.CreateEntityKey(tableName, syseeplog);
                            try
                            {
                                //var lrefvals = (from r in context.SYS_REFVAL
                                //                where r.REFVAL_NO.Equals(refval.REFVAL_NO)
                                //                select r).ToList();
                                SYSEEPLOG syseeplog1 = context.GetObjectByKey(syseeplog.EntityKey) as SYSEEPLOG;
                                context.DeleteObject(syseeplog1);
                                context.SaveChanges();
                            }
                            catch (Exception ex)
                            {

                            }
                            context.AddObject(tableName, syseeplog);
                            break;
                        case "MENUTABLE":
                            MENUTABLE menutable = (MENUTABLE)param[0];
                            String sStatus = param[1].ToString();
                            menutable.EntityKey = context.CreateEntityKey(tableName, menutable);
                            if (sStatus == "Modify")
                            {
                                try
                                {
                                    MENUTABLE menutable1 = context.GetObjectByKey(menutable.EntityKey) as MENUTABLE;
                                    context.DeleteObject(menutable1);
                                    context.SaveChanges();
                                }
                                catch (Exception ex)
                                {

                                }
                            }

                            try
                            {
                                context.AddObject(tableName, menutable);
                            }
                            catch (Exception ex)
                            {

                            }
                            break;
                        case "USERGROUPS":
                            String sGroupID = param[0].ToString();
                            String sSql = String.Format("DELETE FROM USERGROUPS WHERE GROUPID='{0}'", sGroupID);
                            context.ExecuteNonQuery(sSql);
                            context.SaveChanges();

                            String[] sUsers = param[1].ToString().Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (var item in sUsers)
                            {
                                USERGROUPS usergroups = new USERGROUPS();
                                usergroups.USERID = item;
                                usergroups.GROUPID = sGroupID;
                                usergroups.EntityKey = context.CreateEntityKey(tableName, usergroups);

                                try
                                {
                                    context.AddObject(tableName, usergroups);
                                }
                                catch (Exception ex)
                                {

                                }
                            }
                            break;
                        case "GROUPMENUS":
                            String sMenuID = param[0].ToString();
                            String sGROUPMENUSSql = String.Format("DELETE FROM GROUPMENUS WHERE MENUID='{0}'", sMenuID);
                            context.ExecuteNonQuery(sGROUPMENUSSql);
                            context.SaveChanges();

                            String[] sGroupIDs = param[1].ToString().Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (var item in sGroupIDs)
                            {
                                GROUPMENUS groupmenus = new GROUPMENUS();
                                groupmenus.MENUID = sMenuID;
                                groupmenus.GROUPID = item;
                                groupmenus.EntityKey = context.CreateEntityKey(tableName, groupmenus);

                                try
                                {
                                    context.AddObject(tableName, groupmenus);
                                }
                                catch (Exception ex)
                                {

                                }
                            }
                            break;
                        case "USERMENUS":
                            String smenuID = param[0].ToString();
                            String sUSERMENUSSql = String.Format("DELETE FROM USERMENUS WHERE MENUID='{0}'", smenuID);
                            context.ExecuteNonQuery(sUSERMENUSSql);
                            context.SaveChanges();

                            String[] sUserIDs = param[1].ToString().Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (var item in sUserIDs)
                            {
                                USERMENUS usermenus = new USERMENUS();
                                usermenus.MENUID = smenuID;
                                usermenus.USERID = item;
                                usermenus.EntityKey = context.CreateEntityKey(tableName, usermenus);

                                try
                                {
                                    context.AddObject(tableName, usermenus);
                                }
                                catch (Exception ex)
                                {

                                }
                            }
                            break;
                    }
                    context.SaveChanges();
                }
            }
            #endregion
        }

        public void DeleteDataFromTable(object param, String tableName)
        {
            var clientInfo = this.ClientInfo;
            if (clientInfo == null)
            {
                throw new ArgumentNullException("clientInfo");
            }
            #region Dataset
            if (ClientInfo.UseDataSet)
            {
                string database = ClientInfo.Database;
                if (Database.GetSplitSystemTable(ClientInfo.Database, ClientInfo.SDDeveloperID))
                {
                    database = Database.GetSystemDatabase(ClientInfo.SDDeveloperID);
                }
                String strSql = String.Empty;
                switch (tableName)
                {
                    case "MENUTABLECONTROL":
                        MENUTABLECONTROL menucontrol = (MENUTABLECONTROL)param;
                        strSql = String.Format("DELETE FROM MENUTABLECONTROL WHERE MENUID='{0}' AND CONTROLNAME='{1}'", menucontrol.MENUID, menucontrol.CONTROLNAME);
                        Database.ExecuteCommand(database, ClientInfo.SDDeveloperID, strSql);
                        break;
                        ;
                    case "MENUTABLE":
                        MENUTABLE menutable = (MENUTABLE)param;
                        strSql = String.Format("DELETE FROM MENUTABLE WHERE MENUID='{0}'", menutable.MENUID);
                        Database.ExecuteCommand(database, ClientInfo.SDDeveloperID, strSql);
                        break;
                }
            }
            #endregion
            #region Entity
            else if (ClientInfo.SDDeveloperID != null)
            {
                //using (var context = new Entities(CreateEntityConnection(Database.GetSystemDatabase(clientInfo.SDDeveloperID), true)))
                using (var context = new Entities(CreateEntityConnection(clientInfo.Database, true)))
                {
                    context.Connection.Open();

                    switch (tableName)
                    {
                        case "MENUTABLECONTROL":
                            if (param != null)
                            {
                                MENUTABLECONTROL menucontrol = (MENUTABLECONTROL)param;
                                menucontrol.EntityKey = context.CreateEntityKey("MENUTABLECONTROL", menucontrol);
                                try
                                {
                                    MENUTABLECONTROL item1 = context.GetObjectByKey(menucontrol.EntityKey) as MENUTABLECONTROL;
                                    context.DeleteObject(item1);
                                    context.SaveChanges();
                                }
                                catch (Exception ex)
                                {

                                }
                            }
                            break;
                        case "MENUTABLE":
                            if (param != null)
                            {
                                MENUTABLE menutable = (MENUTABLE)param;
                                menutable.EntityKey = context.CreateEntityKey("MENUTABLE", menutable);
                                try
                                {
                                    MENUTABLE item1 = context.GetObjectByKey(menutable.EntityKey) as MENUTABLE;
                                    context.DeleteObject(item1);
                                    context.SaveChanges();
                                }
                                catch (Exception ex)
                                {

                                }
                            }
                            break;
                    }

                    context.SaveChanges();
                }
            }
            #endregion
        }

        List<EntityObject> EFWCFModule.IGlobalModule.GetAllDataByTableName(String strTableName)
        {
            String dbAlias = String.Empty;
            if (Database.GetSplitSystemTable(ClientInfo.Database, ClientInfo.SDDeveloperID))
            {
                dbAlias = Database.GetSystemDatabase(ClientInfo.SDDeveloperID);
            }
            else
            {
                dbAlias = ClientInfo.Database;
            }

            return GetAllDataByTableName(dbAlias, strTableName);
        }

        List<EntityObject> EFWCFModule.IGlobalModule.GetAllDataByTableNameDBAlias(String dbAlias, String strTableName)
        {
            return GetAllDataByTableName(dbAlias, strTableName);
        }

        private List<EntityObject> GetAllDataByTableName(String dbAlias, String strTableName)
        {
            #region DataSet
            if (ClientInfo.UseDataSet)
            {
                DataSet ds = new DataSet();
                String strSql = "select * from " + strTableName;
                if (strTableName == "GROUPMENUCONTROL")
                {
                    String groupids = String.Empty;
                    if (!String.IsNullOrEmpty(ClientInfo.CurrentGroup))
                    {
                        groupids = "'" + ClientInfo.CurrentGroup + "'";
                    }
                    else
                    {
                        groupids = "select GROUPID from USERGROUPS where USERID = '" + this.ClientInfo.UserID + "'";
                    }
                    strSql = "select * from " + strTableName + " where (GROUPID in (" + groupids + ") or GROUPID = '00')";
                }
                else if (strTableName == "USERMENUCONTROL")
                {
                    strSql = "select * from " + strTableName + " where USERID='" + this.ClientInfo.UserID + "'";
                }
                EFService service = new EFService();
                var dataset = Database.ExecuteDataSet(dbAlias, ClientInfo.SDDeveloperID, strSql, strTableName);
                var entityConverter = new EntityConverter();
                return entityConverter.FromDataSet(dataset).ToList();
            }
            #endregion

            #region Entity
            else
            {
                var allData = new List<EntityObject>();
                using (var context = new Entities(CreateEntityConnection(dbAlias, true)))
                {
                    context.Connection.Open();

                    switch (strTableName)
                    {
                        case "MENUTABLE":
                            var menutable = from sr in context.MENUTABLE
                                            select sr;
                            foreach (var obj in menutable)
                            {
                                allData.Add(obj);
                            }
                            break;
                        case "SYS_REFVAL":
                            var sys_refval = from sr in context.SYS_REFVAL
                                             select sr;
                            foreach (var obj in sys_refval)
                            {
                                allData.Add(obj);
                            }
                            break;
                        case "MENUTABLECONTROL":
                            var menucontrol = from sr in context.MENUTABLECONTROL
                                              select sr;
                            foreach (var obj in menucontrol)
                            {
                                allData.Add(obj);
                            }
                            break;
                        case "USERMENUCONTROL":
                            var usermenucontrol = from sr in context.USERMENUCONTROL
                                                  where sr.USERID == this.ClientInfo.UserID
                                                  select sr;
                            foreach (var obj in usermenucontrol)
                            {
                                allData.Add(obj);
                            }
                            break;
                        case "GROUPMENUCONTROL":
                            List<String> lGroups = new List<string>();
                            lGroups.Add("00");
                            if (!String.IsNullOrEmpty(ClientInfo.CurrentGroup))
                            {
                                lGroups.Add(ClientInfo.CurrentGroup);
                            }
                            else
                            {
                                foreach (GroupInfo item in this.ClientInfo.Groups)
                                {
                                    lGroups.Add(item.ID);
                                }
                            }
                            var groupmenucontrol = from sr in context.GROUPMENUCONTROL
                                                   where lGroups.Contains(sr.GROUPID)
                                                   select sr;
                            foreach (var obj in groupmenucontrol)
                            {
                                allData.Add(obj);
                            }
                            break;
                        case "SYS_WEBPAGES":
                            var sys_webpages = from sr in context.SYS_WEBPAGES
                                               select sr;
                            foreach (var obj in sys_webpages)
                            {
                                allData.Add(obj);
                            }
                            break;
                        case "USERS":
                            var users = from u in context.USERS
                                        select u;
                            foreach (var obj in users)
                            {
                                allData.Add(obj);
                            }
                            break;
                        case "USERGROUPS":
                            var usergroups = from ug in context.USERGROUPS
                                             select ug;

                            foreach (var obj in usergroups)
                            {
                                allData.Add(obj);
                            }
                            break;
                        case "GROUPS":
                            var groups = from ug in context.GROUPS
                                         select ug;

                            foreach (var obj in groups)
                            {
                                allData.Add(obj);
                            }
                            break;
                        case "GROUPMENUS":
                            var groupmenus = from ug in context.GROUPMENUS
                                             select ug;

                            foreach (var obj in groupmenus)
                            {
                                allData.Add(obj);
                            }
                            break;
                        case "USERMENUS":
                            var usermenus = from ug in context.USERMENUS
                                            select ug;

                            foreach (var obj in usermenus)
                            {
                                allData.Add(obj);
                            }
                            break;
                    }
                }
                return allData;
            }
            #endregion
        }

        List<EntityObject> EFWCFModule.IGlobalModule.FetchMenus()
        {
            #region DataSet
            if (ClientInfo.UseDataSet)
            {
                string loginUser = this.ClientInfo.UserID;
                string groupids = "";
                var dbAlias = string.Empty;
                if (Database.GetSplitSystemTable(ClientInfo.Database, ClientInfo.SDDeveloperID))
                {
                    dbAlias = Database.GetSystemDatabase(ClientInfo.SDDeveloperID);
                }
                else
                {
                    dbAlias = ClientInfo.Database;
                }
                DataBaseType dbtype = EFWCFModule.EEPAdapter.DatabaseProvider.GetProviderType(dbAlias, ClientInfo.SDDeveloperID);
                string strSqlGetMenu = "";
                if (ClientInfo.CurrentGroup == "forCaption")
                {
                    switch (dbtype)
                    {
                        //因为Solution不同会导致Caption取不到，把solution的条件拿掉了
                        case DataBaseType.MsSql:
                            strSqlGetMenu = "select * from MENUTABLE where MODULETYPE in ('W','L','B','O','J','M','C','E','R') "
                                       + "order by SEQ_NO,MENUID";
                            break;
                        case DataBaseType.ODBC:
                        case DataBaseType.Oracle:
                        case DataBaseType.Informix:
                            strSqlGetMenu = "select * from MENUTABLE where MODULETYPE in ('W','L','B','O','J','M','C','E','R') "
                                       + "order by SEQ_NO,MENUID";
                            break;
                        case DataBaseType.MySql:
                            strSqlGetMenu = "select * from MENUTABLE where MODULETYPE in ('W','L','B','O','J','M','C','E','R') "
                                       + "order by SEQ_NO";
                            break;
                        case DataBaseType.OleDB:
                            strSqlGetMenu = "select * from MENUTABLE where MODULETYPE in ('W','L','B','O','J','M','C','E','R') "
                                       + "order by SEQ_NO";
                            break;
                    }
                }
                else if (ClientInfo.CurrentGroup == "forManager")
                {
                    switch (dbtype)
                    {
                        case DataBaseType.MsSql:
                            strSqlGetMenu = "select * from MENUTABLE where MODULETYPE in ('W','L','B','O','J','M','C','E','R') "
                                       + "and ITEMTYPE in (select ITEMTYPE from MENUITEMTYPE where ITEMTYPE = '" + ClientInfo.Solution + "') "
                                       + "order by SEQ_NO,MENUID";
                            break;
                        case DataBaseType.Oracle:
                        case DataBaseType.Informix:
                            strSqlGetMenu = "select * from MENUTABLE where MODULETYPE in ('W','L','B','O','J','M','C','E','R') "
                                       + "and ITEMTYPE in (select ITEMTYPE from MENUITEMTYPE where ITEMTYPE = '" + ClientInfo.Solution + "') "
                                       + "order by SEQ_NO,MENUID";
                            break;
                        case DataBaseType.MySql:
                            strSqlGetMenu = "select * from MENUTABLE where MODULETYPE in ('W','L','B','O','J','M','C','E','R') "
                                       + "and ITEMTYPE in (select ITEMTYPE from MENUITEMTYPE where ITEMTYPE = '" + ClientInfo.Solution + "') "
                                       + "order by SEQ_NO";
                            break;
                        case DataBaseType.OleDB:
                            strSqlGetMenu = "select * from MENUTABLE where MODULETYPE in ('W','L','B','O','J','M','C','E','R') "
                                       + "and ITEMTYPE in (select ITEMTYPE from MENUITEMTYPE where ITEMTYPE = '" + ClientInfo.Solution + "') "
                                       + "order by SEQ_NO";
                            break;
                    }
                }
                else if (ClientInfo.UserPara2 == "forClick")
                {
                    string parentId = ClientInfo.UserPara1;
                    if (String.IsNullOrEmpty(ClientInfo.CurrentGroup))
                    {
                        groupids = "select GROUPID from USERGROUPS where USERID = '" + ClientInfo.UserID + "'";
                    }
                    else
                    {
                        groupids = "'" + ClientInfo.CurrentGroup + "'";
                    }
                    switch (dbtype)
                    {
                        case DataBaseType.MsSql:
                            strSqlGetMenu = "select * from MENUTABLE where MODULETYPE in ('W','L','B','O','J','M','C','E','R') "
                                       + "and ITEMTYPE in (select ITEMTYPE from MENUITEMTYPE where ITEMTYPE = '" + ClientInfo.Solution + "')"
                                       + " and (MENUID in (select MENUID from GROUPMENUS where GROUPID in (" + groupids + ") or GROUPID = '00')"
                                       + " or MENUID in (select MENUID from USERMENUS where USERID = '" + loginUser + "')"
                                       + ") and PARENT = '" + parentId + "' order by SEQ_NO,MENUID";
                            break;
                        case DataBaseType.ODBC:
                            strSqlGetMenu = "select * from MENUTABLE where MODULETYPE in ('W','L','B','O','J','M','C','E','R') "
                                       + "and ITEMTYPE in (select ITEMTYPE from MENUITEMTYPE where ITEMTYPE = '" + ClientInfo.Solution + "')"
                                       + " and (MENUID in (select MENUID from GROUPMENUS where GROUPID in (" + groupids + ") or GROUPID = '00')"
                                       + " or MENUID in (select MENUID from USERMENUS where USERID = '" + loginUser + "')"
                                       + ") and PARENT = '" + parentId + "' order by SEQ_NO,MENUID";
                            break;
                        case DataBaseType.Oracle:
                        case DataBaseType.Informix:
                            strSqlGetMenu = "select * from MENUTABLE where MODULETYPE in ('W','L','B','O','J','M','C','E','R') "
                                       + "and ITEMTYPE in (select ITEMTYPE from MENUITEMTYPE where ITEMTYPE = '" + ClientInfo.Solution + "')"
                                       + " and (MENUID in (select MENUID from GROUPMENUS where GROUPID in (" + groupids + ") or GROUPID = '00')"
                                       + " or MENUID in (select MENUID from USERMENUS where USERID = '" + loginUser + "')"
                                       + ") and PARENT = '" + parentId + "' order by SEQ_NO,MENUID";
                            break;
                        case DataBaseType.MySql:
                            strSqlGetMenu = "select * from MENUTABLE where MODULETYPE in ('W','L','B','O','J','M','C','E','R') "
                                       + "and ITEMTYPE in (select ITEMTYPE from MENUITEMTYPE where ITEMTYPE = '" + ClientInfo.Solution + "')"
                                       + " and (MENUID in (select MENUID from GROUPMENUS where GROUPID in (" + groupids + ") or GROUPID = '00')"
                                       + " or MENUID in (select MENUID from USERMENUS where USERID = '" + loginUser + "')"
                                       + ") and PARENT = '" + parentId + "' order by SEQ_NO";
                            break;
                        case DataBaseType.OleDB:
                            strSqlGetMenu = "select * from MENUTABLE where MODULETYPE in ('W','L','B','O','J','M','C','E','R') "
                                       + "and ITEMTYPE in (select ITEMTYPE from MENUITEMTYPE where ITEMTYPE = '" + ClientInfo.Solution + "')"
                                       + " and (MENUID in (select MENUID from GROUPMENUS where GROUPID in (" + groupids + ") or GROUPID = '00')"
                                       + " or MENUID in (select MENUID from USERMENUS where USERID = '" + loginUser + "')"
                                       + ") and PARENT = '" + parentId + "' order by SEQ_NO";
                            break;
                    }
                }
                else if (ClientInfo.UserPara2 == "forCheckMenuRights")
                {
                    if (String.IsNullOrEmpty(ClientInfo.CurrentGroup))
                    {
                        groupids = "select GROUPID from USERGROUPS where USERID = '" + ClientInfo.UserID + "'";
                    }
                    else
                    {
                        groupids = "'" + ClientInfo.CurrentGroup + "'";
                    }
                    switch (dbtype)
                    {
                        case DataBaseType.MsSql:
                            strSqlGetMenu = "select * from MENUTABLE where MODULETYPE in ('W','L','B','O','J','M','C','E','R') "
                                       + "and ITEMTYPE in (select ITEMTYPE from MENUITEMTYPE where ITEMTYPE = '" + ClientInfo.Solution + "')"
                                       + " and (MENUID in (select MENUID from GROUPMENUS where GROUPID in (" + groupids + ") or GROUPID = '00')"
                                       + " or MENUID in (select MENUID from USERMENUS where USERID = '" + loginUser + "')"
                                       + " ) order by SEQ_NO,MENUID";
                            break;
                        case DataBaseType.ODBC:
                            strSqlGetMenu = "select * from MENUTABLE where MODULETYPE in ('W','L','B','O','J','M','C','E','R') "
                                       + "and ITEMTYPE in (select ITEMTYPE from MENUITEMTYPE where ITEMTYPE = '" + ClientInfo.Solution + "')"
                                       + " and (MENUID in (select MENUID from GROUPMENUS where GROUPID in (" + groupids + ") or GROUPID = '00')"
                                       + " or MENUID in (select MENUID from USERMENUS where USERID = '" + loginUser + "')"
                                       + " ) order by SEQ_NO,MENUID";
                            break;
                        case DataBaseType.Oracle:
                        case DataBaseType.Informix:
                            strSqlGetMenu = "select * from MENUTABLE where MODULETYPE in ('W','L','B','O','J','M','C','E','R') "
                                       + "and ITEMTYPE in (select ITEMTYPE from MENUITEMTYPE where ITEMTYPE = '" + ClientInfo.Solution + "')"
                                       + " and (MENUID in (select MENUID from GROUPMENUS where GROUPID in (" + groupids + ") or GROUPID = '00')"
                                       + " or MENUID in (select MENUID from USERMENUS where USERID = '" + loginUser + "')"
                                       + " ) order by SEQ_NO,MENUID";
                            break;
                        case DataBaseType.MySql:
                            strSqlGetMenu = "select * from MENUTABLE where MODULETYPE in ('W','L','B','O','J','M','C','E','R') "
                                       + "and ITEMTYPE in (select ITEMTYPE from MENUITEMTYPE where ITEMTYPE = '" + ClientInfo.Solution + "')"
                                       + " and (MENUID in (select MENUID from GROUPMENUS where GROUPID in (" + groupids + ") or GROUPID = '00')"
                                       + " or MENUID in (select MENUID from USERMENUS where USERID = '" + loginUser + "')"
                                       + " ) order by SEQ_NO";
                            break;
                        case DataBaseType.OleDB:
                            strSqlGetMenu = "select * from MENUTABLE where MODULETYPE in ('W','L','B','O','J','M','C','E','R') "
                                       + "and ITEMTYPE in (select ITEMTYPE from MENUITEMTYPE where ITEMTYPE = '" + ClientInfo.Solution + "')"
                                       + " and (MENUID in (select MENUID from GROUPMENUS where GROUPID in (" + groupids + ") or GROUPID = '00')"
                                       + " or MENUID in (select MENUID from USERMENUS where USERID = '" + loginUser + "')"
                                       + " ) order by SEQ_NO";
                            break;
                    }
                }
                else if (ClientInfo.UserPara2 == "forMyFavor")
                {
                    String moduleType = "'J', 'M'";
                    String fLoginUser = ClientInfo.UserID;
                    switch (dbtype)
                    {
                        case DataBaseType.MsSql:
                            strSqlGetMenu = "select MENUTABLE.*, MENUFAVOR.GROUPNAME from MENUTABLE LEFT JOIN MENUFAVOR ON MENUTABLE.MENUID=MENUFAVOR.MENUID AND USERID='" + fLoginUser + "' where MENUTABLE.MODULETYPE in (" + moduleType + ", 'O') "
                                        + "AND MENUTABLE.ITEMTYPE in (select ITEMTYPE from MENUITEMTYPE where ITEMTYPE ='" + ClientInfo.Solution + "')"
                                        + " AND (MENUTABLE.MENUID in (Select MENUID From MENUFAVOR Where USERID='" + fLoginUser + "' and MENUTABLE.MENUID in (SELECT MENUID FROM USERMENUS WHERE USERID='" + fLoginUser + "'"
                                        + " UNION SELECT  DISTINCT  MENUID FROM GROUPMENUS WHERE GROUPID IN (SELECT GROUPID FROM USERGROUPS WHERE USERID='" + fLoginUser + "') OR GROUPID='00'))"
                                        + ")  order by SEQ_NO";
                            break;
                        case DataBaseType.ODBC:
                            strSqlGetMenu = "select MENUTABLE.*, MENUFAVOR.GROUPNAME from MENUTABLE LEFT JOIN MENUFAVOR ON MENUTABLE.MENUID=MENUFAVOR.MENUID AND USERID='" + fLoginUser + "' where MENUTABLE.MODULETYPE in (" + moduleType + ", 'O') "
                                        + "AND MENUTABLE.ITEMTYPE ='" + ClientInfo.Solution + "' "
                                        + " AND (MENUTABLE.MENUID in (Select MENUID From MENUFAVOR Where USERID='" + fLoginUser + "' and (MENUTABLE.MENUID in (SELECT MENUID FROM USERMENUS WHERE USERID='" + fLoginUser + "')"
                                        + " OR MENUTABLE.MENUID in (SELECT DISTINCT MENUID FROM GROUPMENUS WHERE GROUPID IN (SELECT GROUPID FROM USERGROUPS WHERE USERID='" + fLoginUser + "') OR GROUPID='00'))"
                                        + "))  order by SEQ_NO";
                            break;
                        case DataBaseType.Oracle:
                            strSqlGetMenu = "select MENUTABLE.*, MENUFAVOR.GROUPNAME from MENUTABLE LEFT JOIN MENUFAVOR ON MENUTABLE.MENUID=MENUFAVOR.MENUID AND USERID='" + fLoginUser + "' where MENUTABLE.MODULETYPE in (" + moduleType + ") "
                                        + "AND MENUTABLE.ITEMTYPE in (select ITEMTYPE from MENUITEMTYPE where ITEMTYPE ='" + ClientInfo.Solution + "')"
                                        + " AND (MENUTABLE.MENUID in (Select MENUID From MENUFAVOR Where USERID='" + fLoginUser + "' and MENUTABLE.MENUID in (SELECT MENUID FROM USERMENUS WHERE USERID='" + fLoginUser + "'"
                                        + " UNION SELECT  DISTINCT  MENUID FROM GROUPMENUS WHERE GROUPID IN (SELECT GROUPID FROM USERGROUPS WHERE USERID='" + fLoginUser + "') OR GROUPID='00'))"
                                        + ") AND MENUFAVOR.USERID = '" + fLoginUser + "' order by MENUTABLE.CAPTION";
                            break;
                        case DataBaseType.Informix:
                            strSqlGetMenu = "select MENUTABLE.*, MENUFAVOR.GROUPNAME from MENUTABLE LEFT JOIN MENUFAVOR ON MENUTABLE.MENUID=MENUFAVOR.MENUID AND USERID='" + fLoginUser + "' where MENUTABLE.MODULETYPE in (" + moduleType + ", 'O') "
                                        + "AND MENUTABLE.ITEMTYPE ='" + ClientInfo.Solution + "' "
                                        + " AND (MENUTABLE.MENUID in (Select MENUID From MENUFAVOR Where USERID='" + fLoginUser + "' and (MENUTABLE.MENUID in (SELECT MENUID FROM USERMENUS WHERE USERID='" + fLoginUser + "')"
                                        + " OR MENUTABLE.MENUID in (SELECT DISTINCT MENUID FROM GROUPMENUS WHERE GROUPID IN (SELECT GROUPID FROM USERGROUPS WHERE USERID='" + fLoginUser + "') OR GROUPID='00'))"
                                        + "))  order by SEQ_NO";
                            break;
                        case DataBaseType.MySql:
                            strSqlGetMenu = "select MENUTABLE.*, MENUFAVOR.GROUPNAME from MENUTABLE LEFT JOIN MENUFAVOR ON MENUTABLE.MENUID=MENUFAVOR.MENUID AND USERID='" + fLoginUser + "' where MENUTABLE.MODULETYPE in (" + moduleType + ", 'O') "
                                        + "AND MENUTABLE.ITEMTYPE in (select ITEMTYPE from MENUITEMTYPE where ITEMTYPE ='" + ClientInfo.Solution + "')"
                                        + " AND (MENUTABLE.MENUID in (Select MENUID From MENUFAVOR Where USERID='" + fLoginUser + "' and MENUTABLE.MENUID in (SELECT MENUID FROM USERMENUS WHERE USERID='" + fLoginUser + "'"
                                        + " UNION SELECT  DISTINCT  MENUID FROM GROUPMENUS WHERE GROUPID IN (SELECT GROUPID FROM USERGROUPS WHERE USERID='" + fLoginUser + "') OR GROUPID='00'))"
                                        + ")  order by SEQ_NO";
                            break;
                        case DataBaseType.OleDB:
                            strSqlGetMenu = "select MENUTABLE.*, MENUFAVOR.GROUPNAME from MENUTABLE LEFT JOIN MENUFAVOR ON MENUTABLE.MENUID=MENUFAVOR.MENUID AND USERID='" + fLoginUser + "' where MENUTABLE.MODULETYPE in (" + moduleType + ", 'O') "
                                        + "AND MENUTABLE.ITEMTYPE ='" + ClientInfo.Solution + "' "
                                        + " AND (MENUTABLE.MENUID in (Select MENUID From MENUFAVOR Where USERID='" + fLoginUser + "' and (MENUTABLE.MENUID in (SELECT MENUID FROM USERMENUS WHERE USERID='" + fLoginUser + "')"
                                        + " OR MENUTABLE.MENUID in (SELECT DISTINCT MENUID FROM GROUPMENUS WHERE GROUPID IN (SELECT GROUPID FROM USERGROUPS WHERE USERID='" + fLoginUser + "') OR GROUPID='00'))"
                                        + "))  order by SEQ_NO";
                            break;
                    }
                }
                else
                {
                    if (String.IsNullOrEmpty(ClientInfo.CurrentGroup))
                    {
                        //List<String> lGROUPID = new List<string>();
                        //foreach (GroupInfo gi in this.ClientInfo.Groups)
                        //{
                        //    lGROUPID.Add(gi.ID);
                        //}
                        //foreach (string group in lGROUPID)
                        //{
                        //    if (groupids.Length > 0)
                        //    {
                        //        groupids += ",";
                        //    }
                        //    groupids += string.Format("'{0}'", group);
                        //}
                        //if (groupids.Length == 0)
                        //{
                        //    groupids = "''";
                        //}
                        groupids = "select GROUPID from USERGROUPS where USERID = '" + ClientInfo.UserID + "'";
                    }
                    else
                    {
                        groupids = "'" + ClientInfo.CurrentGroup + "'";
                    }
                    switch (dbtype)
                    {
                        case DataBaseType.MsSql:
                            strSqlGetMenu = "select * from MENUTABLE where MODULETYPE in ('W','L','B','O','J','M','C','E','R') "
                                       + "and ITEMTYPE in (select ITEMTYPE from MENUITEMTYPE where ITEMTYPE = '" + ClientInfo.Solution + "')"
                                       + " and (MENUID in (select MENUID from GROUPMENUS where GROUPID in (" + groupids + ") or GROUPID = '00')"
                                       + " or MENUID in (select MENUID from USERMENUS where USERID = '" + loginUser + "')"
                                       + " or (isnull(PARENT, '') = '' and isnull(PACKAGE, '') = '')) order by SEQ_NO,MENUID";
                            break;
                        case DataBaseType.ODBC:
                            strSqlGetMenu = "select * from MENUTABLE where MODULETYPE in ('W','L','B','O','J','M','C','E','R') "
                                       + "and ITEMTYPE in (select ITEMTYPE from MENUITEMTYPE where ITEMTYPE = '" + ClientInfo.Solution + "')"
                                       + " and (MENUID in (select MENUID from GROUPMENUS where GROUPID in (" + groupids + ") or GROUPID = '00')"
                                       + " or MENUID in (select MENUID from USERMENUS where USERID = '" + loginUser + "')"
                                       + " or (coalesce(PARENT, '') = '' and coalesce(PACKAGE, '') = '')) order by SEQ_NO,MENUID";
                            break;
                        case DataBaseType.Oracle:
                        case DataBaseType.Informix:
                            strSqlGetMenu = "select * from MENUTABLE where MODULETYPE in ('W','L','B','O','J','M','C','E','R') "
                                       + "and ITEMTYPE in (select ITEMTYPE from MENUITEMTYPE where ITEMTYPE = '" + ClientInfo.Solution + "')"
                                       + " and (MENUID in (select MENUID from GROUPMENUS where GROUPID in (" + groupids + ") or GROUPID = '00')"
                                       + " or MENUID in (select MENUID from USERMENUS where USERID = '" + loginUser + "')"
                                       + " or (nvl(PARENT, ' ') = ' ' and nvl(PACKAGE, ' ') = ' ')) order by SEQ_NO,MENUID";
                            break;
                        case DataBaseType.MySql:
                            strSqlGetMenu = "select * from MENUTABLE where MODULETYPE in ('W','L','B','O','J','M','C','E','R') "
                                       + "and ITEMTYPE in (select ITEMTYPE from MENUITEMTYPE where ITEMTYPE = '" + ClientInfo.Solution + "')"
                                       + " and (MENUID in (select MENUID from GROUPMENUS where GROUPID in (" + groupids + ") or GROUPID = '00')"
                                       + " or MENUID in (select MENUID from USERMENUS where USERID = '" + loginUser + "')"
                                       + " or (ifnull(PARENT, '') = '' and ifnull(PARENT, '') = '')) order by SEQ_NO";
                            break;
                        case DataBaseType.OleDB:
                            strSqlGetMenu = "select * from MENUTABLE where MODULETYPE in ('W','L','B','O','J','M','C','E','R') "
                                       + "and ITEMTYPE in (select ITEMTYPE from MENUITEMTYPE where ITEMTYPE = '" + ClientInfo.Solution + "')"
                                       + " and (MENUID in (select MENUID from GROUPMENUS where GROUPID in (" + groupids + ") or GROUPID = '00')"
                                       + " or MENUID in (select MENUID from USERMENUS where USERID = '" + loginUser + "')"
                                       + " or (isnull(PARENT, '') = '' or PARENT=' ' and isnull(PACKAGE, '') = '')) order by SEQ_NO";
                            break;
                    }
                }

                if (ClientInfo.UserPara2 == "forRoot" || ClientInfo.UserPara2 == "forRootMenuButton")
                {
                    //foreach (var menu in menus)
                    //{
                    //    if (!string.IsNullOrEmpty(menu.PARENT))
                    //    {
                    //        var parentMenu = menus.FirstOrDefault(c => c.MENUID.Equals(menu.PARENT, StringComparison.OrdinalIgnoreCase));
                    //        if (parentMenu != null)
                    //        {
                    //            parentMenu.MENUTABLE1.Add(menu);
                    //            menu.MENUTABLE2 = parentMenu;
                    //        }
                    //    }
                    //}
                    //List<MENUTABLE> rootMENUs = new List<MENUTABLE>();
                    switch (dbtype)
                    {
                        case DataBaseType.MsSql:
                            strSqlGetMenu = "select * from MENUTABLE where MODULETYPE in ('W','L','B','O','J','M','C','E','R') "
                                       + "and ITEMTYPE in (select ITEMTYPE from MENUITEMTYPE where ITEMTYPE = '" + ClientInfo.Solution + "')"
                                       + " and (MENUID in (select MENUID from GROUPMENUS where GROUPID in (" + groupids + ") or GROUPID = '00')"
                                       + " or MENUID in (select MENUID from USERMENUS where USERID = '" + loginUser + "')"
                                       + " ) order by SEQ_NO,MENUID";
                            break;
                        case DataBaseType.ODBC:
                            strSqlGetMenu = "select * from MENUTABLE where MODULETYPE in ('W','L','B','O','J','M','C','E','R') "
                                       + "and ITEMTYPE in (select ITEMTYPE from MENUITEMTYPE where ITEMTYPE = '" + ClientInfo.Solution + "')"
                                       + " and (MENUID in (select MENUID from GROUPMENUS where GROUPID in (" + groupids + ") or GROUPID = '00')"
                                       + " or MENUID in (select MENUID from USERMENUS where USERID = '" + loginUser + "')"
                                       + " ) order by SEQ_NO,MENUID";
                            break;
                        case DataBaseType.Oracle:
                        case DataBaseType.Informix:
                            strSqlGetMenu = "select * from MENUTABLE where MODULETYPE in ('W','L','B','O','J','M','C','E','R') "
                                       + "and ITEMTYPE in (select ITEMTYPE from MENUITEMTYPE where ITEMTYPE = '" + ClientInfo.Solution + "')"
                                       + " and (MENUID in (select MENUID from GROUPMENUS where GROUPID in (" + groupids + ") or GROUPID = '00')"
                                       + " or MENUID in (select MENUID from USERMENUS where USERID = '" + loginUser + "')"
                                       + " ) order by SEQ_NO,MENUID";
                            break;
                        case DataBaseType.MySql:
                            strSqlGetMenu = "select * from MENUTABLE where MODULETYPE in ('W','L','B','O','J','M','C','E','R') "
                                       + "and ITEMTYPE in (select ITEMTYPE from MENUITEMTYPE where ITEMTYPE = '" + ClientInfo.Solution + "')"
                                       + " and (MENUID in (select MENUID from GROUPMENUS where GROUPID in (" + groupids + ") or GROUPID = '00')"
                                       + " or MENUID in (select MENUID from USERMENUS where USERID = '" + loginUser + "')"
                                       + " ) order by SEQ_NO";
                            break;
                        case DataBaseType.OleDB:
                            strSqlGetMenu = "select * from MENUTABLE where MODULETYPE in ('W','L','B','O','J','M','C','E','R') "
                                       + "and ITEMTYPE in (select ITEMTYPE from MENUITEMTYPE where ITEMTYPE = '" + ClientInfo.Solution + "')"
                                       + " and (MENUID in (select MENUID from GROUPMENUS where GROUPID in (" + groupids + ") or GROUPID = '00')"
                                       + " or MENUID in (select MENUID from USERMENUS where USERID = '" + loginUser + "')"
                                       + " ) order by SEQ_NO";
                            break;
                    }
                    var dataset = Database.ExecuteDataSet(dbAlias, ClientInfo.SDDeveloperID, strSqlGetMenu, "MENUTABLE");
                    var entityConverter = new EntityConverter();
                    var menus = entityConverter.FromDataSet(dataset).OfType<MENUTABLE>().ToList();
                    if (ClientInfo.UserPara2 == "forRootMenuButton")
                    {
                        foreach (var menu in menus)
                        {
                            if (!string.IsNullOrEmpty(menu.PARENT))
                            {
                                var parentMenu = menus.FirstOrDefault(c => c.MENUID.Equals(menu.PARENT, StringComparison.OrdinalIgnoreCase));
                                if (parentMenu != null)
                                {
                                    parentMenu.MENUTABLE1.Add(menu);
                                    menu.MENUTABLE2 = parentMenu;
                                }
                            }
                        }
                    }
                    else
                    {
                        menus = menus.OfType<MENUTABLE>().Where(m => String.IsNullOrEmpty(m.PARENT)).ToList();
                    }
                    //foreach (var menu in menus)
                    //{
                    //    if (menu.MENUTABLE1.Count > 0)
                    //    {
                    //        menu.MENUTABLE1.Clear();
                    //        rootMENUs.Add(menu);
                    //    }
                    //}
                    return menus.ToList<EntityObject>();
                }
                else if (ClientInfo.UserPara2 == "forMyFavor")
                {
                    var dataset = Database.ExecuteDataSet(dbAlias, ClientInfo.SDDeveloperID, strSqlGetMenu, "MENUFAVOR");
                    var entityConverter = new EntityConverter();
                    var menus = entityConverter.FromDataSet(dataset).OfType<EntityObject>().ToList();
                    return menus.OfType<EntityObject>().ToList();
                }
                else
                {
                    var dataset = Database.ExecuteDataSet(dbAlias, ClientInfo.SDDeveloperID, strSqlGetMenu, "MENUTABLE");
                    var entityConverter = new EntityConverter();
                    var menus = entityConverter.FromDataSet(dataset).OfType<MENUTABLE>().ToList();
                    foreach (var menu in menus)
                    {
                        if (!string.IsNullOrEmpty(menu.PARENT))
                        {
                            var parentMenu = menus.FirstOrDefault(c => c.MENUID.Equals(menu.PARENT, StringComparison.OrdinalIgnoreCase));
                            if (parentMenu != null)
                            {
                                parentMenu.MENUTABLE1.Add(menu);
                                menu.MENUTABLE2 = parentMenu;
                            }
                        }
                    }
                    return menus.OfType<EntityObject>().ToList();
                }

            }
            #endregion
            #region Entity
            else
            {
                var menus = new List<EntityObject>();
                using (var context = new Entities(CreateEntityConnection(ClientInfo.Database, true)))
                {
                    context.Connection.Open();

                    List<string> lMENUTABLE = new List<string>();
                    lMENUTABLE.Add("W");
                    lMENUTABLE.Add("O");
                    lMENUTABLE.Add("L");
                    lMENUTABLE.Add("B");
                    lMENUTABLE.Add("J");
                    lMENUTABLE.Add("M");
                    lMENUTABLE.Add("C");

                    List<String> lGROUPID = new List<string>();
                    if (String.IsNullOrEmpty(ClientInfo.CurrentGroup))
                    {
                        var userGroups = (from ug in context.USERGROUPS
                                          where ug.USERID == ClientInfo.UserID
                                          select ug).ToList();
                        foreach (USERGROUPS ug in userGroups)
                        {
                            lGROUPID.Add(ug.GROUPID);
                        }
                    }
                    else
                    {
                        lGROUPID.Add(ClientInfo.CurrentGroup);
                    }

                    List<String> listg = new List<String>();
                    List<String> listu = new List<String>();
                    var menuid_g = (from gm in context.GROUPMENUS
                                    where lGROUPID.Contains(gm.GROUPID)
                                    || gm.GROUPID.Equals("00")
                                    select gm);
                    foreach (var obj in menuid_g)
                    {
                        listg.Add(obj.MENUID);
                    }
                    var menuid_u = from um in context.USERMENUS
                                   where um.USERID.Equals(this.ClientInfo.UserID)
                                   select um;
                    foreach (var obj in menuid_u)
                    {
                        listu.Add(obj.MENUID);
                    }
                    var menuTables = from m in context.MENUTABLE
                                     where
                                     lMENUTABLE.Contains(m.MODULETYPE)
                                     && m.ITEMTYPE.Equals(this.ClientInfo.Solution)
                                     && (listg.Contains(m.MENUID) || listu.Contains(m.MENUID) || String.IsNullOrEmpty(m.PARENT))// || m.PARENT == String.Empty || m.PACKAGE == String.Empty
                                     orderby m.SEQ_NO
                                     select m;
                    foreach (var obj in menuTables)
                    {
                        menus.Add(obj);
                    }
                }
                return menus;
            }
            #endregion
        }

        public string GetServerPath()
        {
            String path = Environment.CommandLine.Replace("\"", String.Empty);
            path = path.Remove(path.LastIndexOf("\\"));
            return path;
        }

        string EFWCFModule.IGlobalModule.GetFlowDataDS(FlowDataType dataType, FlowDataParameter parameter)
        {
            #region DataSet
            if (ClientInfo.UseDataSet)
            {
                var clientInfo = this.ClientInfo;

                if (clientInfo == null)
                {
                    throw new ArgumentNullException("clientInfo");
                }
                var DateTimeString = DateTime.Now.ToString("yyyyMMddHHmmss");



                string strsql = string.Empty;
                string orderby = String.Empty;
                string tableName = string.Empty;
                var roles = clientInfo.Groups.Where(g => g.Type == GroupType.Role).Select(g => g.ID).ToList();
                string rolesString = string.Empty;
                foreach (var obj in roles)
                {
                    if (rolesString != string.Empty)
                        rolesString += ",";
                    rolesString += "'" + obj.ToString() + "'";
                }
                if (string.IsNullOrEmpty(rolesString))
                {
                    rolesString = "''";
                }
                string connectMark = "+";

                var dbAlias = string.Empty;
                if (Database.GetSplitSystemTable(ClientInfo.Database, ClientInfo.SDDeveloperID))
                {
                    dbAlias = Database.GetSystemDatabase(ClientInfo.SDDeveloperID);
                }
                else
                {
                    dbAlias = ClientInfo.Database;
                }
                DataBaseType dbtype = EFWCFModule.EEPAdapter.DatabaseProvider.GetProviderType(dbAlias, ClientInfo.SDDeveloperID);


                var nolock = "";
                switch (dbtype)
                {
                    case DataBaseType.MsSql:
                    case DataBaseType.OleDB:
                        nolock = "with (nolock)";
                        connectMark = "+";
                        break;
                    case DataBaseType.Oracle:
                    case DataBaseType.Informix:
                    case DataBaseType.MySql:
                    case DataBaseType.ODBC:
                        connectMark = "||";
                        break;
                }

                switch (dataType)
                {
                    case FlowDataType.Do:
                        {
                            tableName = "SYS_TODOLIST";
                            if (parameter == null || parameter.ListID == null || parameter.ListID == Guid.Empty)
                            {
                                strsql = "SELECT * FROM SYS_TODOLIST " + nolock + " WHERE STATUS<>'F' AND ((SENDTO_KIND='1' "
                                     + "AND (SENDTO_ID IN (" + rolesString + ") OR SENDTO_ID IN "
                                     + "(SELECT ROLE_ID FROM SYS_ROLES_AGENT WHERE SYS_TODOLIST.SENDTO_ID=SYS_ROLES_AGENT.ROLE_ID AND (SYS_ROLES_AGENT.FLOW_DESC='*' OR SYS_ROLES_AGENT.FLOW_DESC IS NULL OR SYS_TODOLIST.FLOW_DESC=SYS_ROLES_AGENT.FLOW_DESC) AND (SYS_ROLES_AGENT.FLOW_DESC='*' OR SYS_ROLES_AGENT.FLOW_DESC IS NULL OR SYS_TODOLIST.FLOW_DESC=SYS_ROLES_AGENT.FLOW_DESC) AND AGENT ='" + clientInfo.UserID + "' AND '" + DateTimeString + "' > (START_DATE " + connectMark + " START_TIME) AND '" + DateTimeString + "' < (END_DATE " + connectMark + " END_TIME)))) "
                                     + "OR (SENDTO_KIND='2' AND SENDTO_ID = '" + clientInfo.UserID + "'))";
                            }
                            else
                            {
                                string listID = parameter.ListID.ToString();
                                strsql = "SELECT * FROM SYS_TODOLIST WHERE LISTID='" + listID + "' and STATUS <> 'F'";

                            }
                            if (parameter != null && !String.IsNullOrEmpty(parameter.Description))
                                strsql += " AND " + parameter.Description;

                            if (parameter != null)
                            {
                                if (String.IsNullOrEmpty(parameter.OrderBy))
                                    strsql += orderby = " ORDER BY UPDATE_DATE DESC, UPDATE_TIME DESC ";
                                else
                                    strsql += orderby = " ORDER BY " + parameter.OrderBy;
                            }
                            break;
                        }
                    case FlowDataType.History:
                        {
                            tableName = "SYS_TODOLIST";
                            if (parameter == null || parameter.ListID == null || parameter.ListID == Guid.Empty)
                            {
                                strsql = "SELECT * FROM SYS_TODOLIST " + nolock + " WHERE STATUS<>'F' "
                                     + "AND EXISTS "
                                     + "(SELECT SYS_TODOHIS.LISTID FROM SYS_TODOHIS WHERE SYS_TODOHIS.LISTID = SYS_TODOLIST.LISTID AND SYS_TODOHIS.USER_ID ='" + clientInfo.UserID + "') "
                                     + "AND ( "
                                     + "(SYS_TODOLIST.SENDTO_KIND='1' AND SYS_TODOLIST.SENDTO_ID NOT IN (" + rolesString + ")) "
                                     + "OR (SYS_TODOLIST.SENDTO_KIND='2' AND SYS_TODOLIST.SENDTO_ID <> '" + clientInfo.UserID + "'))";
                            }
                            else
                            {
                                string listID = parameter.ListID.ToString();
                                strsql = "SELECT * FROM SYS_TODOHIS WHERE LISTID='" + listID + "'";
                            }
                            if (parameter != null && !String.IsNullOrEmpty(parameter.Description))
                                strsql += " AND " + parameter.Description;

                            if (parameter != null)
                            {
                                if (String.IsNullOrEmpty(parameter.OrderBy))
                                    strsql += orderby = " ORDER BY UPDATE_DATE DESC, UPDATE_TIME DESC ";
                                else
                                    strsql += orderby = " ORDER BY " + parameter.OrderBy;
                            }
                            break;
                        }
                    case FlowDataType.Notify:
                        {
                            tableName = "SYS_TODOLIST";
                            strsql = "SELECT * FROM SYS_TODOLIST WHERE STATUS='F' AND ((SENDTO_KIND='1' "
                                 + "AND (SENDTO_ID IN (" + rolesString + ") OR SENDTO_ID IN "
                                 + "(SELECT ROLE_ID FROM SYS_ROLES_AGENT WHERE SYS_TODOLIST.SENDTO_ID=SYS_ROLES_AGENT.ROLE_ID AND AGENT ='" + clientInfo.UserID + "' AND '" + DateTimeString + "' > (START_DATE " + connectMark + " START_TIME) AND '" + DateTimeString + "' < (END_DATE " + connectMark + " END_TIME)))) "
                                 + "OR (SENDTO_KIND='2' AND SENDTO_ID = '" + clientInfo.UserID + "'))";
                            if (parameter != null && !String.IsNullOrEmpty(parameter.Description))
                                strsql += " AND " + parameter.Description;

                            if (parameter != null)
                            {
                                if (String.IsNullOrEmpty(parameter.OrderBy))
                                    strsql += orderby = " ORDER BY UPDATE_DATE DESC, UPDATE_TIME DESC ";
                                else
                                    strsql += orderby = " ORDER BY " + parameter.OrderBy;
                            }
                            break;
                        }
                    case FlowDataType.End:
                        {
                            tableName = "SYS_TODOHIS";
                            strsql = @"SELECT * FROM SYS_TODOHIS
                                       WHERE (STATUS = 'Z' OR STATUS = 'X') AND LISTID IN 
                                       (SELECT DISTINCT LISTID FROM SYS_TODOHIS WHERE USER_ID ='" + clientInfo.UserID + "')";
                            if (parameter != null && !String.IsNullOrEmpty(parameter.Description))
                                strsql += " AND " + parameter.Description;

                            if (parameter != null)
                            {
                                if (String.IsNullOrEmpty(parameter.OrderBy))
                                    strsql += orderby = " ORDER BY UPDATE_DATE DESC, UPDATE_TIME DESC ";
                                else
                                    strsql += orderby = " ORDER BY " + parameter.OrderBy;
                            }
                            break;
                        }
                    case FlowDataType.Organization:
                        {
                            tableName = "SYS_ORGKIND";
                            strsql = @"SELECT * FROM SYS_ORGKIND";
                            break;
                        }
                    case FlowDataType.AllUsers:
                        {
                            string whereStr = "";
                            if (!String.IsNullOrEmpty(parameter.Description.Trim()))
                            {
                                whereStr = parameter.Description;
                            }
                            tableName = "USERS";
                            strsql = @"SELECT DISTINCT USERS.USERID, USERS.USERNAME FROM USERS LEFT JOIN USERGROUPS ON USERGROUPS.USERID=USERS.USERID LEFT JOIN GROUPS ON GROUPS.GROUPID = USERGROUPS.GROUPID WHERE (GROUPS.ISROLE='Y' OR GROUPS.ISROLE='y') AND (AUTOLOGIN <> 'X' or AUTOLOGIN <> 'x' or AUTOLOGIN is null) " + whereStr;
                            break;
                        }
                    case FlowDataType.AllGroups:
                        {
                            string whereStr = "";
                            if (!String.IsNullOrEmpty(parameter.Description.Trim()))
                            {
                                whereStr = parameter.Description;
                            }
                            tableName = "GROUPS";
                            strsql = "SELECT * FROM GROUPS WHERE (ISROLE ='Y' OR ISROLE='y')" + whereStr;
                            break;
                        }
                    case FlowDataType.Group:
                        {
                            tableName = "GROUPS";
                            strsql = @"SELECT * FROM GROUPS WHERE (GROUPS.ISROLE ='Y' OR GROUPS.ISROLE='y') AND GROUPID IN (SELECT GROUPID FROM USERGROUPS WHERE USERID ='" + clientInfo.UserID + "') OR GROUPID IN "
                                   + "(SELECT ROLE_ID FROM SYS_ROLES_AGENT WHERE AGENT ='" + clientInfo.UserID + "' AND '" + DateTimeString + "' > (START_DATE " + connectMark + " START_TIME) AND '" + DateTimeString + "' < (END_DATE " + connectMark + " END_TIME))";
                            break;
                        }
                    case FlowDataType.Users:
                        {
                            tableName = "USERS";
                            strsql = @"SELECT USERID, USERNAME FROM USERS WHERE USERID='" + parameter.UserID + "'";
                            break;
                        }
                    case FlowDataType.secGroup:
                        {
                            tableName = "GROUPS";
                            strsql = @"SELECT * FROM GROUPS LEFT JOIN GROUPMENUS ON GROUPS.GROUPID = GROUPMENUS.GROUPID WHERE (ISROLE ='Y' OR ISROLE='y') AND GROUPS.GROUPID IN 
                                    (SELECT GROUPID FROM USERGROUPS WHERE USERGROUPS.USERID IN 
                                    (SELECT USERMENUS.USERID FROM USERMENUS WHERE USERMENUS.MENUID ='" + parameter.Description + "'))"
                                    + " AND GROUPMENUS.GROUPID IN (SELECT GROUPID FROM GROUPS WHERE (GROUPS.ISROLE ='Y' OR GROUPS.ISROLE = 'y')) AND GROUPMENUS.MENUID ='" + parameter.Description + "'";
                            break;
                        }
                    case FlowDataType.Overtime:
                        {
                            if (parameter != null && !string.IsNullOrEmpty(parameter.Description) && parameter.Description == "2147483646")
                            {
                                strsql = "SELECT * FROM SYS_TODOLIST WHERE STATUS <> 'F'";
                                tableName = "SYS_TODOLIST";
                                if (parameter != null && !String.IsNullOrEmpty(parameter.SpecialUse))
                                    strsql += " AND " + parameter.SpecialUse;

                                if (parameter != null)
                                {
                                    if (String.IsNullOrEmpty(parameter.OrderBy))
                                        strsql += orderby = " ORDER BY UPDATE_DATE DESC, UPDATE_TIME DESC ";
                                    else
                                        strsql += orderby = " ORDER BY " + parameter.OrderBy;
                                }
                            }
                            else
                            {
                                //获取本人的Roles
                                int level = int.Parse(parameter.Description);
                                List<string> lstRoles = new List<string>();
                                List<string> lstSubRoles = new List<string>();
                                string currentUser = clientInfo.UserID;
                                string sqlCurRoles = "select GROUPID from USERGROUPS where " + (string.IsNullOrEmpty(currentUser) ? "" : "USERID = '" + currentUser + "' and ") + "GROUPID in (select GROUPID from GROUPS where ISROLE = 'Y')";
                                DataSet ds = Database.ExecuteDataSet(dbAlias, ClientInfo.SDDeveloperID, sqlCurRoles, "USERGROUPS");
                                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                {
                                    lstRoles.Add(ds.Tables[0].Rows[i]["GROUPID"].ToString());
                                }

                                List<string> lstOrgs = new List<string>();
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
                                    ds = Database.ExecuteDataSet(dbAlias, ClientInfo.SDDeveloperID, sqlOrg, "SYS_ORG");
                                    for (int x = 0; x < ds.Tables[0].Rows.Count; x++)
                                    {
                                        string org = ds.Tables[0].Rows[x]["ORG_NO"].ToString();
                                        if (!lstOrgs.Contains(org))
                                            lstOrgs.Add(org);
                                        if (!string.IsNullOrEmpty(orgMans))
                                        {
                                            var orgRoleds = Database.ExecuteDataSet(dbAlias, ClientInfo.SDDeveloperID, "select *  from SYS_ORGROLES where ORG_NO ='" + org + "'", "SYS_ORG");
                                            for (int j = 0; j < orgRoleds.Tables[0].Rows.Count; j++)
                                            {
                                                var role = orgRoleds.Tables[0].Rows[j]["ROLE_ID"].ToString();
                                                if (!lstRoles.Contains(role))
                                                    lstRoles.Add(role);
                                                if (!lstSubRoles.Contains(role))
                                                    lstSubRoles.Add(role);
                                            }
                                        }
                                    }

                                    string upperOrgs = "";
                                    foreach (string org in lstOrgs)
                                    {
                                        upperOrgs += "'" + org + "',";
                                    }
                                    if (upperOrgs.IndexOf(',') != -1)
                                        upperOrgs = upperOrgs.Substring(0, upperOrgs.LastIndexOf(','));
                                    string sqlOrgManRole = string.IsNullOrEmpty(upperOrgs) ? "select ORG_MAN from SYS_ORG where UPPER_ORG IS NULL" : "select ORG_MAN from SYS_ORG where UPPER_ORG in (" + upperOrgs + ")";
                                    ds = Database.ExecuteDataSet(dbAlias, ClientInfo.SDDeveloperID, sqlOrgManRole, "SYS_ORG");
                                    for (int x = 0; x < ds.Tables[0].Rows.Count; x++)
                                    {
                                        string orgMan = ds.Tables[0].Rows[x]["ORG_MAN"].ToString();
                                        if (!lstRoles.Contains(orgMan))
                                            lstRoles.Add(orgMan);
                                        if (!lstSubRoles.Contains(orgMan))
                                            lstSubRoles.Add(orgMan);
                                    }

                                    foreach (string org in lstOrgs)
                                    {
                                        string sqlOrgRoles = "select ROLE_ID from SYS_ORGROLES where ORG_NO='" + org + "'";
                                        ds = Database.ExecuteDataSet(dbAlias, ClientInfo.SDDeveloperID, sqlOrgRoles, "SYS_ORGROLES");
                                        for (int x = 0; x < ds.Tables[0].Rows.Count; x++)
                                        {
                                            string orgRole = ds.Tables[0].Rows[x]["ROLE_ID"].ToString();
                                            if (!lstRoles.Contains(orgRole))
                                                lstRoles.Add(orgRole);
                                            if (!lstSubRoles.Contains(orgRole))
                                                lstSubRoles.Add(orgRole);
                                        }
                                    }
                                }

                                string sRoles = "";
                                foreach (string role in lstRoles)
                                {
                                    sRoles += "'" + role + "',";
                                }
                                if (sRoles.IndexOf(',') != -1)
                                    sRoles = sRoles.Substring(0, sRoles.LastIndexOf(','));

                                string sSubRoles = "";
                                foreach (string role in lstSubRoles)
                                {
                                    sSubRoles += "'" + role + "',";
                                }
                                if (sSubRoles.IndexOf(',') != -1)
                                    sSubRoles = sSubRoles.Substring(0, sSubRoles.LastIndexOf(','));

                                bool delay = true;
                                //joy 2010/1/11 modify : 增加 ATTACHMENTS,MULTISTEPRETURN,PARAMETERS 欄位,因為逾時需要用到這些欄位
                                if (sSubRoles == "")
                                    strsql = "SELECT * FROM SYS_TODOLIST where " + (string.IsNullOrEmpty(sRoles) ? "1=0" : ("((SENDTO_ID in (" + sRoles + ")  and SENDTO_KIND='1') or (SENDTO_ID ='" + currentUser + "' and SENDTO_KIND='2')) and STATUS !='F' "));
                                else
                                    strsql = "SELECT * FROM SYS_TODOLIST where " + (string.IsNullOrEmpty(sRoles) ? "1=0" : ("((SENDTO_ID in (" + sRoles + ")  and SENDTO_KIND='1') or ((SENDTO_ID ='" + currentUser + "' or SENDTO_ID IN (SELECT USERID FROM USERGROUPS WHERE GROUPID IN (" + sSubRoles + "))) and SENDTO_KIND='2')) and STATUS !='F' "));

                                tableName = "SYS_TODOLIST";
                                if (!string.IsNullOrEmpty(parameter.SpecialUse))
                                {
                                    strsql = "SELECT * FROM SYS_TODOLIST where " + (string.IsNullOrEmpty(sRoles) ? "1=0" : ("((SENDTO_ID in (" + sRoles + ")  and SENDTO_KIND='1') or ((SENDTO_ID ='" + currentUser + "' or SENDTO_ID IN (SELECT USERID FROM USERGROUPS WHERE GROUPID IN (" + sRoles + "))) and SENDTO_KIND='2') and STATUS !='F') AND " + parameter.SpecialUse));
                                }

                                if (parameter != null)
                                {
                                    if (String.IsNullOrEmpty(parameter.OrderBy))
                                    {
                                        strsql += orderby = delay ? " ORDER BY UPDATE_DATE" : " ORDER BY FLOW_DESC";
                                    }
                                    else
                                    {
                                        strsql += orderby = " ORDER BY " + parameter.OrderBy;
                                    }
                                }
                            }
                            break;
                        }
                }
                var packetInfo = new PacketInfo();
                if (parameter != null)
                {
                    packetInfo.StartIndex = parameter.StartIndex;
                    packetInfo.Count = parameter.Count;
                }

                var dataset = Database.ExecuteDataSet(dbAlias, clientInfo.SDDeveloperID, strsql, tableName, packetInfo);
                if (dataType == FlowDataType.Do || dataType == FlowDataType.History || dataType == FlowDataType.Notify || dataType == FlowDataType.Overtime || dataType == FlowDataType.End)
                {
                    strsql = strsql.Replace("SELECT * FROM", "SELECT COUNT(*) FROM");
                    if (!String.IsNullOrEmpty(orderby))
                        strsql = strsql.Replace(orderby, String.Empty);
                    //strsql = strsql.Replace(" ORDER BY UPDATE_DATE DESC, UPDATE_TIME DESC ", string.Empty);
                    strsql = strsql.Replace("ORDER BY UPDATE_DATE", string.Empty);

                    var countDataSet = Database.ExecuteDataSet(dbAlias, clientInfo.SDDeveloperID, strsql, string.Empty);

                    dataset.Tables[0].ExtendedProperties.Add("Count", countDataSet.Tables[0].Rows[0][0].ToString());
                }



                if (dataType == FlowDataType.History)
                {
                    for (int i = 0; i < dataset.Tables[0].Rows.Count; i++)
                    {
                        string formNameSql = "SELECT FORM_NAME, WEBFORM_NAME, PARAMETERS FROM SYS_TODOHIS WHERE SYS_TODOHIS.LISTID = '{0}' AND SYS_TODOHIS.USER_ID ='{1}' ORDER BY UPDATE_DATE DESC, UPDATE_TIME DESC";
                        var formNameDataSet = Database.ExecuteDataSet(dbAlias, ClientInfo.SDDeveloperID, string.Format(formNameSql, dataset.Tables[0].Rows[i]["LISTID"], clientInfo.UserID), tableName);
                        if (formNameDataSet.Tables[0].Rows.Count > 0)
                        {
                            dataset.Tables[0].Rows[i]["FORM_NAME"] = formNameDataSet.Tables[0].Rows[0]["FORM_NAME"];
                            dataset.Tables[0].Rows[i]["WEBFORM_NAME"] = formNameDataSet.Tables[0].Rows[0]["WEBFORM_NAME"];
                            dataset.Tables[0].Rows[i]["PARAMETERS"] = formNameDataSet.Tables[0].Rows[0]["PARAMETERS"];
                        }
                    }
                }
                else if (dataType == FlowDataType.Group)
                {
                    if (parameter.ListID != null)
                    {
                        string roleSQL = "SELECT ROLE_ID  FROM SYS_TODOHIS WHERE SYS_TODOHIS.LISTID = '{0}' AND SYS_TODOHIS.USER_ID ='{1}' ORDER BY UPDATE_DATE, UPDATE_TIME";
                        var roleDataSet = Database.ExecuteDataSet(dbAlias, ClientInfo.SDDeveloperID, string.Format(roleSQL, parameter.ListID, clientInfo.UserID), tableName);
                        if (roleDataSet.Tables[0].Rows.Count > 0)
                        {
                            var role = roleDataSet.Tables[0].Rows[0]["ROLE_ID"].ToString();
                            for (int i = 0; i < dataset.Tables[0].Rows.Count; i++)
                            {
                                var groupID = dataset.Tables[0].Rows[i]["GROUPID"].ToString();
                                if (groupID == role)
                                {
                                    var itemArray = dataset.Tables[0].Rows[i].ItemArray;
                                    dataset.Tables[0].Rows[i].Delete();
                                    var row = dataset.Tables[0].NewRow();
                                    row.ItemArray = itemArray;
                                    dataset.Tables[0].Rows.InsertAt(row, 0);
                                    dataset.AcceptChanges();
                                }
                            }
                        }
                    }
                }


                return Serialize<DataSet>(dataset);
            }
            #endregion
            return null;
        }

        private static string Serialize<T>(T value)
        {
            if (value == null)
            {
                return null;
            }
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = new UnicodeEncoding(false, false);
            settings.Indent = false;
            settings.OmitXmlDeclaration = false;
            settings.CheckCharacters = false;
            using (StringWriter textWriter = new StringWriter())
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(textWriter, settings))
                {
                    serializer.Serialize(xmlWriter, value);
                }
                return textWriter.ToString();
            }
        }

        List<EntityObject> EFWCFModule.IGlobalModule.GetFlowData(FlowDataType dataType, FlowDataParameter parameter)
        {
            #region DataSet
            if (ClientInfo.UseDataSet)
            {
                var clientInfo = this.ClientInfo;

                if (clientInfo == null)
                {
                    throw new ArgumentNullException("clientInfo");
                }
                var DateTimeString = DateTime.Now.ToString("yyyyMMddHHmmss");

                string strsql = string.Empty;
                string tableName = string.Empty;
                var roles = clientInfo.Groups.Where(g => g.Type == GroupType.Role).Select(g => g.ID).ToList();
                string rolesString = string.Empty;
                foreach (var obj in roles)
                {
                    if (rolesString != string.Empty)
                        rolesString += ",";
                    rolesString += "'" + obj.ToString() + "'";
                }
                if (string.IsNullOrEmpty(rolesString))
                {
                    rolesString = "''";
                }
                string connectMark = "+";

                var dbAlias = string.Empty;
                if (Database.GetSplitSystemTable(ClientInfo.Database, ClientInfo.SDDeveloperID))
                {
                    dbAlias = Database.GetSystemDatabase(ClientInfo.SDDeveloperID);
                }
                else
                {
                    dbAlias = ClientInfo.Database;
                }
                DataBaseType dbtype = EFWCFModule.EEPAdapter.DatabaseProvider.GetProviderType(dbAlias, ClientInfo.SDDeveloperID);
                switch (dbtype)
                {
                    case DataBaseType.MsSql:
                    case DataBaseType.OleDB:
                        connectMark = "+";
                        break;
                    case DataBaseType.Oracle:
                    case DataBaseType.Informix:
                    case DataBaseType.MySql:
                    case DataBaseType.ODBC:
                        connectMark = "||";
                        break;
                }

                switch (dataType)
                {
                    case FlowDataType.Do:
                        {
                            tableName = "SYS_TODOLIST";
                            if (parameter == null || parameter.ListID == null)
                            {
                                strsql = "SELECT * FROM SYS_TODOLIST WHERE STATUS<>'F' AND ((SENDTO_KIND='1' "
                                     + "AND (SENDTO_ID IN (" + rolesString + ") OR SENDTO_ID IN "
                                     + "(SELECT ROLE_ID FROM SYS_ROLES_AGENT WHERE SYS_TODOLIST.SENDTO_ID=SYS_ROLES_AGENT.ROLE_ID AND AGENT ='" + clientInfo.UserID + "' AND '" + DateTimeString + "' > (START_DATE " + connectMark + " START_TIME) AND '" + DateTimeString + "' < (END_DATE " + connectMark + " END_TIME)))) "
                                     + "OR (SENDTO_KIND='2' AND SENDTO_ID = '" + clientInfo.UserID + "'))";
                            }
                            else
                            {
                                string listID = parameter.ListID.ToString();
                                strsql = "SELECT * FROM SYS_TODOLIST WHERE LISTID='" + listID + "'";

                            }
                            break;
                        }
                    case FlowDataType.History:
                        {
                            tableName = "SYS_TODOLIST";
                            if (parameter == null || parameter.ListID == null)
                            {
                                strsql = "SELECT * FROM SYS_TODOLIST WHERE STATUS<>'F' "
                                     + "AND EXISTS "
                                     + "(SELECT SYS_TODOHIS.LISTID FROM SYS_TODOHIS WHERE SYS_TODOHIS.LISTID = SYS_TODOLIST.LISTID AND SYS_TODOHIS.USER_ID ='" + clientInfo.UserID + "') "
                                     + "AND ( "
                                     + "(SYS_TODOLIST.SENDTO_KIND='1' AND SYS_TODOLIST.SENDTO_ID NOT IN (" + rolesString + ")) "
                                     + "OR (SYS_TODOLIST.SENDTO_KIND='2' AND SYS_TODOLIST.SENDTO_ID <> '" + clientInfo.UserID + "'))";
                            }
                            else
                            {
                                string listID = parameter.ListID.ToString();
                                strsql = "SELECT * FROM SYS_TODOHIS WHERE LISTID='" + listID + "'";
                            }
                            break;
                        }
                    case FlowDataType.Notify:
                        {
                            tableName = "SYS_TODOLIST";
                            strsql = "SELECT * FROM SYS_TODOLIST WHERE STATUS='F' AND ((SENDTO_KIND='1' "
                                 + "AND (SENDTO_ID IN (" + rolesString + ") OR SENDTO_ID IN "
                                 + "(SELECT ROLE_ID FROM SYS_ROLES_AGENT WHERE SYS_TODOLIST.SENDTO_ID=SYS_ROLES_AGENT.ROLE_ID AND AGENT ='" + clientInfo.UserID + "' AND '" + DateTimeString + "' > (START_DATE " + connectMark + " START_TIME) AND '" + DateTimeString + "' < (END_DATE " + connectMark + " END_TIME)))) "
                                 + "OR (SENDTO_KIND='2' AND SENDTO_ID = '" + clientInfo.UserID + "'))";
                            break;
                        }
                    case FlowDataType.End:
                        {
                            tableName = "SYS_TODOHIS";
                            strsql = @"SELECT * FROM SYS_TODOHIS
                                       WHERE STATUS = 'Z' AND LISTID IN 
                                       (SELECT DISTINCT LISTID FROM SYS_TODOHIS WHERE S_USER_ID ='" + clientInfo.UserID + ")";
                            break;
                        }
                    case FlowDataType.Organization:
                        {
                            tableName = "SYS_ORGKIND";
                            strsql = @"SELECT * FROM SYS_ORGKIND";
                            break;
                        }
                    case FlowDataType.AllUsers:
                        {
                            tableName = "USERS";
                            strsql = @"SELECT * FROM USERS LEFT JOIN USERGROUPS ON USERGROUPS.USERID=USERS.USERID LEFT JOIN GROUPS ON GROUPS.GROUPID = USERGROUPS.GROUPID WHERE GROUPS.ISROLE='Y' OR GROUPS.ISROLE='y'";
                            break;
                        }
                    case FlowDataType.AllGroups:
                        {
                            tableName = "GROUPS";
                            strsql = "SELECT * FROM GROUPS WHERE ISROLE ='Y' OR ISROLE='y'";
                            break;
                        }
                    case FlowDataType.Group:
                        {
                            tableName = "GROUPS";
                            strsql = @"SELECT * FROM GROUPS WHERE (GROUPS.ISROLE ='Y' OR GROUPS.ISROLE='y') AND GROUPID IN (SELECT GROUPID FROM USERGROUPS WHERE USERID ='" + clientInfo.UserID + "') OR GROUPID IN "
                                   + "(SELECT ROLE_ID FROM SYS_ROLES_AGENT WHERE AGENT ='" + clientInfo.UserID + "' AND '" + DateTimeString + "' > (START_DATE " + connectMark + " START_TIME) AND '" + DateTimeString + "' < (END_DATE " + connectMark + " END_TIME))";
                            break;
                        }
                    case FlowDataType.secGroup:
                        {
                            tableName = "GROUPS";
                            strsql = @"SELECT * FROM GROUPS LEFT JOIN GROUPMENUS ON GROUPS.GROUPID = GROUPMENUS.GROUPID WHERE (ISROLE ='Y' OR ISROLE='y') AND GROUPS.GROUPID IN 
                                    (SELECT GROUPID FROM USERGROUPS WHERE USERGROUPS.USERID IN 
                                    (SELECT USERMENUS.USERID FROM USERMENUS WHERE USERMENUS.MENUID ='" + parameter.Description + "'))"
                                    + " AND GROUPMENUS.GROUPID IN (SELECT GROUPID FROM GROUPS WHERE (GROUPS.ISROLE ='Y' OR GROUPS.ISROLE = 'y')) AND GROUPMENUS.MENUID ='" + parameter.Description + "'";
                            break;
                        }
                }
                var dataset = Database.ExecuteDataSet(dbAlias, ClientInfo.SDDeveloperID, strsql, tableName);
                var entityConverter = new EntityConverter();
                return entityConverter.FromDataSet(dataset).ToList();
            }
            #endregion
            #region Entitity
            else
            {
                var clientInfo = this.ClientInfo;
                if (clientInfo == null)
                {
                    throw new ArgumentNullException("clientInfo");
                }
                var database = clientInfo.Database;
                var DateTimeString = DateTime.Now.ToString("yyyyMMddHHmmss");
                using (var context = new Entities(CreateEntityConnection(database, true)))
                {
                    var list = new List<EntityObject>();
                    var roles = clientInfo.Groups.Where(g => g.Type == GroupType.Role).Select(g => g.ID).ToList();
                    switch (dataType)
                    {
                        case FlowDataType.Do:
                            {
                                if (parameter == null || parameter.ListID == null)
                                {
                                    var listd = from d in context.SYS_TODOLIST
                                                where
                                                !d.STATUS.Equals("F")
                                                &&
                                                ((d.SENDTO_KIND.Equals("1") &&
                                                (roles.Contains(d.SENDTO_ID) || //role
                                                (from a in context.SYS_ROLES_AGENT
                                                 where
                                                a.ROLE_ID.Equals(d.SENDTO_ID) && a.AGENT.Equals(clientInfo.UserID)
                                                && DateTimeString.CompareTo(a.START_DATE + a.START_TIME) > 0 && DateTimeString.CompareTo(a.END_DATE + a.END_TIME) < 0
                                                 select a).Count() > 0)) //agent
                                                ||
                                                (d.SENDTO_KIND.Equals("2") && d.SENDTO_ID.Equals(clientInfo.UserID))//user
                                                )
                                                select d;
                                    foreach (var obj in listd)
                                    {
                                        list.Add(obj);
                                    }
                                }
                                else
                                {
                                    string listID = parameter.ListID.ToString();
                                    var listd = from d in context.SYS_TODOLIST
                                                where
                                                d.LISTID.Equals(listID)
                                                select d;
                                    foreach (var obj in listd)
                                    {
                                        list.Add(obj);
                                    }
                                }
                                break;
                            }
                        case FlowDataType.History:
                            {
                                if (parameter == null || parameter.ListID == null)
                                {
                                    var listd = from d in context.SYS_TODOLIST
                                                where
                                                !d.STATUS.Equals("F")
                                                &&
                                                (from h in context.SYS_TODOHIS
                                                 where h.LISTID == d.LISTID && h.USER_ID.Equals(clientInfo.UserID)
                                                 select h).Count() > 0
                                                &&
                                                !((d.SENDTO_KIND.Equals("1") &&
                                                (roles.Contains(d.SENDTO_ID)) || //role
                                                (d.SENDTO_KIND.Equals("2") && d.SENDTO_ID.Equals(clientInfo.UserID)))//user
                                                )
                                                select d;
                                    foreach (var obj in listd)
                                    {
                                        list.Add(obj);
                                    }
                                }
                                else
                                {
                                    string listID = parameter.ListID.ToString();
                                    var listh = from h in context.SYS_TODOHIS
                                                where
                                                h.LISTID.Equals(listID)
                                                select h;
                                    foreach (var obj in listh)
                                    {
                                        list.Add(obj);
                                    }
                                }
                                break;
                            }
                        case FlowDataType.Notify:
                            {
                                var listd = from d in context.SYS_TODOLIST
                                            where
                                            d.STATUS.Equals("F")
                                            &&
                                            ((d.SENDTO_KIND.Equals("1") &&
                                            (roles.Contains(d.SENDTO_ID) || //role
                                            (from a in context.SYS_ROLES_AGENT
                                             where
                                            a.ROLE_ID.Equals(d.SENDTO_ID) && a.AGENT.Equals(clientInfo.UserID)
                                            && DateTimeString.CompareTo(a.START_DATE + a.START_TIME) > 0 && DateTimeString.CompareTo(a.END_DATE + a.END_TIME) < 0
                                             select a).Count() > 0)) //agent
                                            ||
                                            (d.SENDTO_KIND.Equals("2") && d.SENDTO_ID.Equals(clientInfo.UserID))//user
                                            )
                                            select d;
                                foreach (var obj in listd)
                                {
                                    list.Add(obj);
                                }
                                break;
                            }
                        case FlowDataType.End:
                            {
                                var listid = (from d in context.SYS_TODOHIS
                                              where
                                              d.S_USER_ID.Equals(clientInfo.UserID)
                                              select d.LISTID).Distinct().ToList();


                                var listd = from d in context.SYS_TODOHIS
                                            where
                                            d.STATUS.Equals("Z")
                                            && listid.Contains(d.LISTID)
                                            select d;
                                foreach (var obj in listd)
                                {
                                    list.Add(obj);
                                }
                                break;
                            }
                        case FlowDataType.Organization:
                            {
                                var listo = from o in context.SYS_ORGKIND select o;
                                foreach (var obj in listo)
                                {
                                    list.Add(obj);
                                }
                                break;
                            }
                        case FlowDataType.AllUsers:
                            {
                                var listu = (from u in context.USERS
                                             join ug in context.USERGROUPS on u.USERID equals ug.USERID
                                             join g in context.GROUPS on ug.GROUPID equals g.GROUPID
                                             where string.Compare(g.ISROLE, "Y", true) == 0
                                             select u).Distinct();
                                foreach (var obj in listu)
                                {
                                    list.Add(obj);
                                }

                                break;
                            }
                        case FlowDataType.AllGroups:
                            {
                                var listg = from g in context.GROUPS
                                            where
                                            string.Compare(g.ISROLE, "Y", true) == 0
                                            select g;
                                foreach (var obj in listg)
                                {
                                    list.Add(obj);
                                }
                                break;
                            }
                        case FlowDataType.Group:
                            {
                                var gid = (from ug in context.USERGROUPS
                                           where
                                           ug.USERID.Equals(clientInfo.UserID)
                                           select ug.GROUPID).ToList();
                                var aid = (from a in context.SYS_ROLES_AGENT
                                           where
                                           a.AGENT.Equals(clientInfo.UserID)
                                           && DateTimeString.CompareTo(a.START_DATE + a.START_TIME) > 0 && DateTimeString.CompareTo(a.END_DATE + a.END_TIME) < 0
                                           select a.ROLE_ID).ToList();

                                //var listg = from g in context.GROUPS
                                //            where
                                //            (from ug in context.USERGROUPS
                                //             where
                                //             ug.USERID.Equals(clientInfo.UserID)
                                //             select ug.GROUPID).ToList().Contains(g.GROUPID)
                                //            ||
                                //            (from a in context.SYS_ROLES_AGENT
                                //             where
                                //             a.AGENT.Equals(clientInfo.UserID)
                                //             && DateTimeString.CompareTo(a.START_DATE + a.START_TIME) > 0 && DateTimeString.CompareTo(a.END_DATE + a.END_TIME) < 0
                                //             select a.ROLE_ID).ToList().Contains(g.GROUPID)
                                //            select g;



                                var listg = from g in context.GROUPS
                                            where
                                            (gid.Contains(g.GROUPID)
                                            && string.Compare(g.ISROLE, "Y", true) == 0)
                                            ||
                                            (aid.Contains(g.GROUPID)
                                            && string.Compare(g.ISROLE, "Y", true) == 0)
                                            select g;
                                foreach (var obj in listg)
                                {
                                    list.Add(obj);
                                }
                                break;
                            }
                        case FlowDataType.secGroup:
                            var listUM = (from um in context.USERMENUS
                                          where um.MENUID.Equals(parameter.Description)
                                          select um.USERID).ToList();
                            var listUG = (from ug in context.USERGROUPS
                                          where listUM.Contains(ug.USERID)
                                          select ug.GROUPID).ToList();
                            var listG = (from g in context.GROUPS
                                         where string.Compare(g.ISROLE, "Y", true) == 0
                                         select g.GROUPID).ToList();

                            var listggm = (from g in context.GROUPS
                                           join gm in context.GROUPMENUS on g.GROUPID equals gm.GROUPID
                                           where string.Compare(g.ISROLE, "Y", true) == 0
                                           && listUG.Contains(g.GROUPID)
                                           && gm.MENUID.Equals(parameter.Description)
                                           && listG.Contains(gm.GROUPID)
                                           select g).Distinct();

                            foreach (var obj in listggm)
                            {
                                list.Add(obj);
                            }
                            break;
                    }
                    return list;
                }
            }
            #endregion
        }

        #region SDServer
        ClientInfo EFWCFModule.IGlobalModule.CheckUserForSDModule()
        {
            var clientInfo = this.ClientInfo;
            if (clientInfo == null)
            {
                throw new ArgumentNullException("clientInfo");
            }
            if (string.IsNullOrEmpty(clientInfo.UserID))
            {
                throw new ArgumentNullException("clientInfo.UserID");
            }
            string sql = "SELECT * FROM Sys_SDUsers WHERE USERID='" + clientInfo.UserID + "'";
            IEFService service = new EFService();
            var dsUsers = Database.ExecuteDataSet(Database.SystemDatabase, sql, null) as DataSet;
            if (dsUsers != null && dsUsers.Tables[0].Rows.Count > 0 && dsUsers.Tables[0].Rows[0]["USERID"].ToString() != "")
            {
                DataRow drUsers = dsUsers.Tables[0].Rows[0];
                string pwd = drUsers["Password"].ToString();
                string active = drUsers["Active"].ToString();
                DateTime expiryDate = DateTime.Today.AddHours(24);
                if (drUsers["ExpiryDate"] != DBNull.Value)
                {
                    expiryDate = (DateTime)drUsers["ExpiryDate"];
                }
                expiryDate = expiryDate.AddDays(1);
                if ((DateTime.Now - expiryDate).TotalSeconds > 0)
                {
                    clientInfo.LogonResult = LogonResult.UserDisabled;
                }
                else if (string.Compare(active, "Y", true) != 0)
                {
                    clientInfo.LogonResult = LogonResult.UserDisabled;
                }
                else if (!CheckPassword(clientInfo.UserID, clientInfo.Password, pwd))
                {
                    clientInfo.LogonResult = LogonResult.PasswordError;//密码错误
                }
                else
                {
                    string groupID = drUsers["GroupID"].ToString();
                    clientInfo.LogonResult = LogonResult.Logoned;
                    clientInfo.DatabaseType = Database.GetProviderType(Database.GetSystemDatabase(null), null).ToString();
                    clientInfo.UserName = drUsers["USERNAME"].ToString();//设置用户名
                    clientInfo.Groups = new List<GroupInfo>();
                    if (groupID != "")
                    {
                        string getGroupSql = "SELECT * FROM Sys_SDGroups WHERE Sys_SDGroups.GroupID ='" + groupID + "'";
                        DataSet dsGroups = Database.ExecuteDataSet(Database.SystemDatabase, getGroupSql, null) as DataSet;
                        if (dsGroups != null && dsGroups.Tables[0].Rows.Count > 0 && dsGroups.Tables[0].Rows[0][0].ToString() != "")
                        {
                            foreach (DataRow group in dsGroups.Tables[0].Rows)//设置群组
                            {
                                var groupInfo = new GroupInfo() { ID = group["GROUPID"].ToString(), Name = group["GROUPNAME"].ToString() };
                                groupInfo.Type = GroupType.Normal;
                                clientInfo.Groups.Add(groupInfo);
                            }

                        }
                    }
                    string getAliasSql = "SELECT * FROM Sys_SDAlias WHERE Sys_SDAlias.USERID ='" + clientInfo.UserID + "'";
                    DataSet dsAlias = Database.ExecuteDataSet(Database.SystemDatabase, getAliasSql, null) as DataSet;
                    if (dsAlias != null && dsAlias.Tables[0].Rows.Count > 0 && dsAlias.Tables[0].Rows[0][0].ToString() != "")
                    {
                        var aliasInfo = dsAlias.Tables[0].Rows[0]["AliasName"].ToString();
                        clientInfo.Database = aliasInfo;
                    }
                    InsertUserLogForSD();
                    clientInfo.SecurityKey = PublicKey.GetEncryptKey(clientInfo.UserID, clientInfo.UserName, Database.SystemDatabase, string.Empty, clientInfo.DatabaseType, clientInfo.IPAddress);
                }
            }
            else
            {
                clientInfo.LogonResult = LogonResult.UserNotFound;//找不到用户
            }
            return clientInfo;
        }

        public List<EntityObject> GetAllDataByTableNameForSDSystemTable(String strTableName, String strUserID)
        {
            var allData = new List<EntityObject>();
            ClientInfo.SDDeveloperID = null;//清除设计者ID
            using (var context = new Entities(CreateEntityConnection(Database.SystemDatabase, true)))
            {
                context.Connection.Open();

                switch (strTableName)
                {
                    case "SYS_SDSOLUTIONS":
                        var groups = from gr in context.SYS_SDUSERS
                                     where gr.USERID == strUserID
                                     select gr.GROUPID;
                        if (groups.Count<string>() > 0 && groups.ToList()[0] != null)
                        {

                            var users = from us in context.SYS_SDUSERS
                                        where groups.Contains(us.GROUPID)
                                        select us.USERID;
                            var solutionControl = from sr in context.SYS_SDSOLUTIONS
                                                  where users.Contains(sr.USERID)
                                                  select sr;
                            foreach (var obj in solutionControl)
                            {
                                allData.Add(obj);
                            }
                        }
                        else
                        {
                            var solutionControl = from sr in context.SYS_SDSOLUTIONS
                                                  where sr.USERID == strUserID
                                                  select sr;
                            foreach (var obj in solutionControl)
                            {
                                allData.Add(obj);
                            }
                        }
                        break;
                    case "SYS_SDALIAS":
                        var groups3 = from gr in context.SYS_SDUSERS
                                      where gr.USERID == strUserID
                                      select gr.GROUPID;
                        if (groups3.Count<string>() > 0 && groups3.ToList()[0] != null)
                        {
                            var users3 = from us in context.SYS_SDUSERS
                                         where groups3.Contains(us.GROUPID)
                                         select us.USERID;
                            var aliasControl = from sr in context.SYS_SDALIAS
                                               where users3.Contains(sr.USERID)
                                               select sr;

                            foreach (var obj in aliasControl)
                            {
                                allData.Add(obj);
                            }
                        }
                        else
                        {
                            var aliasControl = from sr in context.SYS_SDALIAS
                                               where sr.USERID == strUserID
                                               select sr;

                            foreach (var obj in aliasControl)
                            {
                                allData.Add(obj);
                            }
                        }
                        break;
                    case "SYS_WEBPAGES":
                        String[] sParams2 = strUserID.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                        if (sParams2.Length == 4)//只取一个项目或者几个项目
                        {
                            String userId2 = sParams2[0];
                            String solutionId2 = sParams2[1];
                            String pageType2 = sParams2[2];
                            String pageName2 = sParams2[3];
                            if (pageName2.IndexOf(',') > 0)
                            {
                                List<string> pageList = pageName2.Split(new char[] { ',' }).ToList<string>();
                                var groups2 = from gr in context.SYS_SDUSERS
                                              where gr.USERID == userId2
                                              select gr.GROUPID;
                                if (groups2.Count<string>() > 0 && groups2.ToList()[0] != null)
                                {
                                    var users2 = from us in context.SYS_SDUSERS
                                                 where groups2.Contains(us.GROUPID)
                                                 select us.USERID;
                                    var sys_webpages = from sr in context.SYS_WEBPAGES
                                                       where users2.Contains(sr.USERID) && sr.SOLUTIONID == solutionId2
                                                       && sr.PAGETYPE == pageType2 && pageList.Contains(sr.PAGENAME)
                                                       select sr;
                                    foreach (var obj in sys_webpages)
                                    {
                                        allData.Add(obj);
                                    }
                                }
                                else
                                {
                                    var sys_webpages = from sr in context.SYS_WEBPAGES
                                                       where sr.USERID == userId2 && sr.SOLUTIONID == solutionId2
                                                       && sr.PAGETYPE == pageType2 && pageList.Contains(sr.PAGENAME)
                                                       select sr;
                                    foreach (var obj in sys_webpages)
                                    {
                                        allData.Add(obj);
                                    }
                                }
                            }
                            else
                            {
                                var groups2 = from gr in context.SYS_SDUSERS
                                              where gr.USERID == userId2
                                              select gr.GROUPID;
                                if (groups2.Count<string>() > 0 && groups2.ToList()[0] != null)
                                {
                                    var users2 = from us in context.SYS_SDUSERS
                                                 where groups2.Contains(us.GROUPID)
                                                 select us.USERID;
                                    var sys_webpages = from sr in context.SYS_WEBPAGES
                                                       where users2.Contains(sr.USERID) && sr.SOLUTIONID == solutionId2
                                                       && sr.PAGETYPE == pageType2 && sr.PAGENAME == pageName2
                                                       select sr;
                                    foreach (var obj in sys_webpages)
                                    {
                                        allData.Add(obj);
                                    }
                                }
                                else
                                {
                                    var sys_webpages = from sr in context.SYS_WEBPAGES
                                                       where sr.USERID == userId2 && sr.SOLUTIONID == solutionId2
                                                       && sr.PAGETYPE == pageType2 && sr.PAGENAME == pageName2
                                                       select sr;
                                    foreach (var obj in sys_webpages)
                                    {
                                        allData.Add(obj);
                                    }
                                }
                            }
                        }
                        else if (sParams2.Length == 2)//开始时取所有的DDTableRef
                        {
                            String userId2 = sParams2[0];
                            String solutionId2 = sParams2[1];
                            var groups2 = from gr in context.SYS_SDUSERS
                                          where gr.USERID == userId2
                                          select gr.GROUPID;
                            if (groups2.Count<string>() > 0 && groups2.ToList()[0] != null)
                            {
                                var users2 = from us in context.SYS_SDUSERS
                                             where groups2.Contains(us.GROUPID)
                                             select us.USERID;
                                var sys_webpages = from sr in context.SYS_WEBPAGES
                                                   where users2.Contains(sr.USERID) && sr.SOLUTIONID == solutionId2
                                                   && (sr.PAGETYPE == "T" || sr.PAGETYPE == "D" || sr.PAGETYPE == "R")
                                                   select sr;
                                foreach (var obj in sys_webpages)
                                {
                                    allData.Add(obj);
                                }
                            }
                            else
                            {
                                var sys_webpages = from sr in context.SYS_WEBPAGES
                                                   where sr.USERID == userId2 && sr.SOLUTIONID == solutionId2
                                                   && (sr.PAGETYPE == "T" || sr.PAGETYPE == "D" || sr.PAGETYPE == "R")
                                                   select sr;
                                foreach (var obj in sys_webpages)
                                {
                                    allData.Add(obj);
                                }
                            }
                        }
                        else
                        {
                            var groups2 = from gr in context.SYS_SDUSERS
                                          where gr.USERID == strUserID
                                          select gr.GROUPID;
                            if (groups2.Count<string>() > 0 && groups2.ToList()[0] != null)
                            {
                                var users2 = from us in context.SYS_SDUSERS
                                             where groups2.Contains(us.GROUPID)
                                             select us.USERID;
                                var sys_webpages = from sr in context.SYS_WEBPAGES
                                                   where users2.Contains(sr.USERID)
                                                   select sr;
                                foreach (var obj in sys_webpages)
                                {
                                    allData.Add(obj);
                                }
                            }
                            else
                            {
                                var sys_webpages = from sr in context.SYS_WEBPAGES
                                                   where sr.USERID == strUserID
                                                   select sr;
                                foreach (var obj in sys_webpages)
                                {
                                    allData.Add(obj);
                                }
                            }
                        }
                        break;
                    case "SYS_WEBRUNTIME":
                        {
                            String[] sParams = strUserID.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                            String userId = sParams[0];
                            String solutionId = sParams[1];
                            String pageType = sParams[2];
                            String pageName = sParams[3];
                            var groupsOpenMenu = from gr in context.SYS_SDUSERS
                                                 where gr.USERID == userId
                                                 select gr.GROUPID;
                            if (groupsOpenMenu.Count<string>() > 0 && groupsOpenMenu.ToList()[0] != null)
                            {
                                var users2 = from us in context.SYS_SDUSERS
                                             where groupsOpenMenu.Contains(us.GROUPID)
                                             select us.USERID;
                                var sys_webpages = from sr in context.SYS_WEBRUNTIME
                                                   where users2.Contains(sr.USERID) && sr.SOLUTIONID == solutionId
                                                   && sr.PAGETYPE == pageType && sr.PAGENAME == pageName
                                                   select sr;
                                foreach (var obj in sys_webpages)
                                {
                                    allData.Add(obj);
                                }
                            }
                            else
                            {
                                var sys_webpages = from sr in context.SYS_WEBRUNTIME
                                                   where sr.USERID == userId && sr.SOLUTIONID == solutionId
                                                   && sr.PAGETYPE == pageType && sr.PAGENAME == pageName
                                                   select sr;
                                foreach (var obj in sys_webpages)
                                {
                                    allData.Add(obj);
                                }
                            }

                            break;
                        }
                    case "SYS_WEBPAGES_OpenMenu":
                        {
                            String[] sParams = strUserID.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                            String userId = sParams[0];
                            String solutionId = sParams[1];
                            String pageType = sParams[2];
                            String pageName = sParams[3];
                            var groupsOpenMenu = from gr in context.SYS_SDUSERS
                                                 where gr.USERID == userId
                                                 select gr.GROUPID;
                            if (groupsOpenMenu.Count<string>() > 0 && groupsOpenMenu.ToList()[0] != null)
                            {
                                var users2 = from us in context.SYS_SDUSERS
                                             where groupsOpenMenu.Contains(us.GROUPID)
                                             select us.USERID;
                                var sys_webpages = from sr in context.SYS_WEBPAGES
                                                   where users2.Contains(sr.USERID) && sr.SOLUTIONID == solutionId
                                                   && sr.PAGETYPE == pageType && sr.PAGENAME == pageName
                                                   select sr;
                                foreach (var obj in sys_webpages)
                                {
                                    allData.Add(obj);
                                }
                            }
                            else
                            {
                                var sys_webpages = from sr in context.SYS_WEBPAGES
                                                   where sr.USERID == userId && sr.SOLUTIONID == solutionId
                                                   && sr.PAGETYPE == pageType && sr.PAGENAME == pageName
                                                   select sr;
                                foreach (var obj in sys_webpages)
                                {
                                    allData.Add(obj);
                                }
                            }
                            break;
                        }
                    case "SYS_WEBRUNTIME_Slim":
                        var groups6 = from gr in context.SYS_SDUSERS
                                      where gr.USERID == strUserID
                                      select gr.GROUPID;
                        if (groups6.Count<string>() > 0 && groups6.ToList()[0] != null)
                        {
                            var users2 = from us in context.SYS_SDUSERS
                                         where groups6.Contains(us.GROUPID)
                                         select us.USERID;
                            var sys_webpages = from sr in context.SYS_WEBRUNTIME
                                               where users2.Contains(sr.USERID)
                                               select new { sr.PAGENAME, sr.PAGETYPE, sr.USERID, sr.SOLUTIONID };
                            foreach (var obj in sys_webpages)
                            {
                                SYS_WEBRUNTIME newSYS_WEBPAGES = new SYS_WEBRUNTIME();
                                newSYS_WEBPAGES.PAGENAME = obj.PAGENAME;
                                newSYS_WEBPAGES.PAGETYPE = obj.PAGETYPE;
                                newSYS_WEBPAGES.USERID = obj.USERID;
                                newSYS_WEBPAGES.SOLUTIONID = obj.SOLUTIONID;
                                allData.Add(newSYS_WEBPAGES);
                            }
                        }
                        else
                        {
                            var sys_webpages = from sr in context.SYS_WEBRUNTIME
                                               where sr.USERID == strUserID
                                               select new { sr.PAGENAME, sr.PAGETYPE, sr.USERID, sr.SOLUTIONID };
                            foreach (var obj in sys_webpages)
                            {
                                SYS_WEBRUNTIME newSYS_WEBPAGES = new SYS_WEBRUNTIME();
                                newSYS_WEBPAGES.PAGENAME = obj.PAGENAME;
                                newSYS_WEBPAGES.PAGETYPE = obj.PAGETYPE;
                                newSYS_WEBPAGES.USERID = obj.USERID;
                                newSYS_WEBPAGES.SOLUTIONID = obj.SOLUTIONID;
                                allData.Add(newSYS_WEBPAGES);
                            }
                        }
                        break;
                    //case "SYS_WEBPAGES_Slim":
                    //    var groups6 = from gr in context.SYS_SDUSERS
                    //                  where gr.UserID == strUserID
                    //                  select gr.GroupID;
                    //    if (groups6.Count<string>() > 0 && groups6.ToList()[0] != null)
                    //    {
                    //        var users2 = from us in context.SYS_SDUSERS
                    //                     where groups6.Contains(us.GroupID)
                    //                     select us.UserID;
                    //        var sys_webpages = from sr in context.SYS_WEBPAGES
                    //                           where users2.Contains(sr.UserID)
                    //                           select new { sr.PageName, sr.PageType, sr.UserID, sr.SolutionID };
                    //        foreach (var obj in sys_webpages)
                    //        {
                    //            SYS_WEBPAGES newSYS_WEBPAGES = new SYS_WEBPAGES();
                    //            newSYS_WEBPAGES.PageName = obj.PageName;
                    //            newSYS_WEBPAGES.PageType = obj.PageType;
                    //            newSYS_WEBPAGES.UserID = obj.UserID;
                    //            newSYS_WEBPAGES.SolutionID = obj.SolutionID;
                    //            allData.Add(newSYS_WEBPAGES);
                    //        }
                    //    }
                    //    else
                    //    {
                    //        var sys_webpages = from sr in context.SYS_WEBPAGES
                    //                           where sr.UserID == strUserID
                    //                           select new { sr.PageName, sr.PageType, sr.UserID, sr.SolutionID };
                    //        foreach (var obj in sys_webpages)
                    //        {
                    //            SYS_WEBPAGES newSYS_WEBPAGES = new SYS_WEBPAGES();
                    //            newSYS_WEBPAGES.PageName = obj.PageName;
                    //            newSYS_WEBPAGES.PageType = obj.PageType;
                    //            newSYS_WEBPAGES.UserID = obj.UserID;
                    //            newSYS_WEBPAGES.SolutionID = obj.SolutionID;
                    //            allData.Add(newSYS_WEBPAGES);
                    //        }
                    //    }
                    //    break;
                    case "SYS_WEBPAGES_LOG":
                        String[] sParams4 = strUserID.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                        if (sParams4.Length == 4)//只取一个项目
                        {
                            String userId2 = sParams4[0];
                            String solutionId2 = sParams4[1];
                            String pageType2 = sParams4[2];
                            String pageName2 = sParams4[3];
                            var groups4 = from gr in context.SYS_SDUSERS
                                          where gr.USERID == strUserID
                                          select gr.GROUPID;
                            if (groups4.Count<string>() > 0 && groups4.ToList()[0] != null)
                            {
                                var users2 = from us in context.SYS_SDUSERS
                                             where groups4.Contains(us.GROUPID)
                                             select us.USERID;
                                var sys_webpages_log = from sr in context.SYS_WEBPAGES_LOG
                                                       where users2.Contains(sr.USERID) && sr.SOLUTIONID == solutionId2
                                                       && sr.PAGETYPE == pageType2 && sr.PAGENAME == pageName2
                                                       select sr;
                                foreach (var obj in sys_webpages_log)
                                {
                                    allData.Add(obj);
                                }
                            }
                            else
                            {
                                var sys_webpages_log = from sr in context.SYS_WEBPAGES_LOG
                                                       where sr.USERID == userId2 && sr.SOLUTIONID == solutionId2
                                                       && sr.PAGETYPE == pageType2 && sr.PAGENAME == pageName2
                                                       select sr;
                                foreach (var obj in sys_webpages_log)
                                {
                                    allData.Add(obj);
                                }
                            }
                        }
                        else
                        {
                            var groups4 = from gr in context.SYS_SDUSERS
                                          where gr.USERID == strUserID
                                          select gr.GROUPID;
                            if (groups4.Count<string>() > 0 && groups4.ToList()[0] != null)
                            {
                                var users2 = from us in context.SYS_SDUSERS
                                             where groups4.Contains(us.GROUPID)
                                             select us.USERID;
                                var sys_webpages_log = from sr in context.SYS_WEBPAGES_LOG
                                                       where users2.Contains(sr.USERID)
                                                       select sr;
                                foreach (var obj in sys_webpages_log)
                                {
                                    allData.Add(obj);
                                }
                            }
                            else
                            {
                                var sys_webpages_log = from sr in context.SYS_WEBPAGES_LOG
                                                       where sr.USERID == strUserID
                                                       select sr;
                                foreach (var obj in sys_webpages_log)
                                {
                                    allData.Add(obj);
                                }
                            }
                        }
                        break;
                    case "SYS_SDQUEUE":
                        var sys_queue = from sr in context.SYS_SDQUEUE
                                        where sr.USERID == strUserID
                                        select sr;
                        foreach (var obj in sys_queue)
                        {
                            allData.Add(obj);
                        }

                        break;
                    case "SYS_SDUSERS":
                        if (string.IsNullOrEmpty(strUserID))
                        {
                            var sys_sdusers = from sr in context.SYS_SDUSERS select sr;
                            foreach (var obj in sys_sdusers)
                            {
                                allData.Add(obj);
                            }
                        }
                        else
                        {
                            var sys_sdusers = from sr in context.SYS_SDUSERS where string.Compare(sr.USERID, strUserID, true) == 0 select sr;
                            foreach (var obj in sys_sdusers)
                            {
                                allData.Add(obj);
                            }
                        }
                        break;
                    case "SYS_SDGROUPS":
                        var sys_sdgroups = from sr in context.SYS_SDGROUPS select sr;
                        foreach (var obj in sys_sdgroups)
                        {
                            allData.Add(obj);
                        }
                        break;

                    case "SYS_SDUSERS_LOG":
                        {
                            string[] logParams = strUserID.Split(new char[] { ';' });
                            var userID = logParams[0];
                            var strStart = logParams[1];
                            var strEnd = logParams[2];
                            DateTime timeStart = string.IsNullOrEmpty(strStart) ? DateTime.MinValue : DateTime.Parse(strStart);
                            DateTime timeEnd = string.IsNullOrEmpty(strEnd) ? DateTime.MaxValue : DateTime.Parse(strEnd);
                            var sys_sdusers_log = from sr in context.SYS_SDUSERS_LOG
                                                  where string.Compare(sr.USERID, userID, true) == 0 && (sr.LOGINTIME > timeStart && sr.LOGINTIME < timeEnd)
                                                  orderby sr.LOGINTIME descending
                                                  select sr;
                            foreach (var obj in sys_sdusers_log.Take(100))
                            {
                                allData.Add(obj);
                            }
                            break;
                        }

                    case "SYS_SDUSERS_LOG_STAT":
                        {
                            string[] logParams = strUserID.Split(new char[] { ';' });
                            var strStart = logParams[0];
                            var strEnd = logParams[1];
                            DateTime timeStart = string.IsNullOrEmpty(strStart) ? DateTime.MinValue : DateTime.Parse(strStart);
                            DateTime timeEnd = string.IsNullOrEmpty(strEnd) ? DateTime.MaxValue : DateTime.Parse(strEnd);
                            var log_stat = from sr in context.SYS_SDUSERS_LOG
                                           where (sr.LOGINTIME > timeStart && sr.LOGINTIME < timeEnd)
                                           group sr by sr.USERID
                                               into g
                                               where g.Count() > 0
                                               select new
                                               {
                                                   UserID = g.Key,
                                                   LastLoinTime = g.Max(c => c.LOGINTIME),
                                                   LoginCount = g.Count(),
                                                   //TotalMinutes = 0/*g.Sum(c => (c.LogoutTime - c.LoginTime).TotalMinutes)   System.Data.Objects.SqlClient.SqlFunctions.DateDiff(*/
                                                   TotalMinutes = g.Sum(c => System.Data.Objects.SqlClient.SqlFunctions.DateDiff("mi", c.LOGINTIME, c.LOGOUTTIME))
                                               };
                            foreach (var log in log_stat)
                            {
                                var sys_sduser_log_stat = new SYS_SDUSERS_LOG_STAT();
                                sys_sduser_log_stat.UserID = log.UserID;
                                sys_sduser_log_stat.LastDateTime = log.LastLoinTime;
                                sys_sduser_log_stat.LoginCount = log.LoginCount;
                                sys_sduser_log_stat.TotalMinutes = (int)log.TotalMinutes;
                                allData.Add(sys_sduser_log_stat);
                            }
                            break;
                        }
                }
            }
            return allData;
        }

        public void SaveDataToTableForSDSystemTable(object[] param, String tableName)
        {
            var clientInfo = this.ClientInfo;
            if (clientInfo == null)
            {
                throw new ArgumentNullException("clientInfo");
            }
            ClientInfo.SDDeveloperID = null;//清除设计者ID
            using (var context = new Entities(CreateEntityConnection(Database.SystemDatabase, true)))
            {
                context.Connection.Open();

                switch (tableName)
                {
                    case "SYS_WEBPAGES":
                        SYS_WEBPAGES sys_webpages = (SYS_WEBPAGES)param[0];
                        if (param.Count() > 2 && param[1].ToString().StartsWith("ServerComponentCS:"))
                        {
                            String sServerComponentCS = param[1].ToString().Replace("ServerComponentCS:", String.Empty);
                            String sAssemblyName = param[2].ToString();
                            sys_webpages.SERVERDLL = SDModuleProvider.CreateServerDllAssembly(sServerComponentCS, sAssemblyName, string.Empty, this.ClientInfo);
                        }
                        else if (sys_webpages.PAGETYPE == "P")
                        {
                            var content = System.Text.Encoding.UTF8.GetString(sys_webpages.CONTENT, 0, sys_webpages.CONTENT.Length);
                            //先取出Code
                            var CODE_STRING = "ClientCS:";
                            var xaml = string.Empty;
                            var code = string.Empty;
                            if (!string.IsNullOrEmpty(content))
                            {
                                var codeIndex = content.LastIndexOf(CODE_STRING);
                                if (codeIndex != -1)
                                {
                                    xaml = content.Substring(0, codeIndex);
                                    if (content.Length > codeIndex + CODE_STRING.Length)
                                    {
                                        code = content.Substring(codeIndex + CODE_STRING.Length);
                                    }
                                }
                                else
                                {
                                    xaml = content;
                                }
                            }
                            if (param.Length == 3)
                            {
                                var saveOption = (int)param[1];
                                //改存sys_webruntime表
                                DeleteSysWebRuntime(context, sys_webpages.PAGENAME, sys_webpages.USERID, sys_webpages.SOLUTIONID);
                                if ((saveOption & 0x01) > 0) // silverlight
                                {
                                    SYS_WEBRUNTIME page = new SYS_WEBRUNTIME()
                                    {
                                        PAGENAME = sys_webpages.PAGENAME,
                                        USERID = sys_webpages.USERID,
                                        SOLUTIONID = sys_webpages.SOLUTIONID,
                                        PAGETYPE = "L"
                                    };
                                    page.CONTENT = SDModuleProvider.CreateSilverlightClientAssembly(xaml, code);
                                    context.AddObject("SYS_WEBRUNTIME", page);

                                }
                                if ((saveOption & 0x02) > 0) // webpage
                                {
                                    SYS_WEBRUNTIME page = new SYS_WEBRUNTIME()
                                    {
                                        PAGENAME = sys_webpages.PAGENAME,
                                        USERID = sys_webpages.USERID,
                                        SOLUTIONID = sys_webpages.SOLUTIONID,
                                        PAGETYPE = "W"
                                    };
                                    page.CONTENT = System.Text.Encoding.UTF8.GetBytes((string)param[2]);
                                    context.AddObject("SYS_WEBRUNTIME", page);
                                }
                            }
                            else
                            {
                                sys_webpages.SERVERDLL = SDModuleProvider.CreateSilverlightClientAssembly(xaml, code);
                            }
                        }
                        else if (sys_webpages.PAGETYPE == "W")
                        {
                            var content = System.Text.Encoding.UTF8.GetString(sys_webpages.CONTENT, 0, sys_webpages.CONTENT.Length);
                            //先取出Code
                            var CODE_STRING = "ClientCS:";
                            var xaml = string.Empty;
                            var code = string.Empty;
                            if (!string.IsNullOrEmpty(content))
                            {
                                var codeIndex = content.LastIndexOf(CODE_STRING);
                                if (codeIndex != -1)
                                {
                                    xaml = content.Substring(0, codeIndex);
                                    if (content.Length > codeIndex + CODE_STRING.Length)
                                    {
                                        code = content.Substring(codeIndex + CODE_STRING.Length);
                                    }
                                }
                                else
                                {
                                    xaml = content;
                                }
                            }
                            //sys_webpages.SERVERDLL = SDModuleProvider.CreateSilverlightClientAssembly(xaml, code);
                        }


                        sys_webpages.EntityKey = context.CreateEntityKey(tableName, sys_webpages);
                        try
                        {
                            SYS_WEBPAGES sys_webpages1 = context.GetObjectByKey(sys_webpages.EntityKey) as SYS_WEBPAGES;
                            context.DeleteObject(sys_webpages1);
                            context.SaveChanges();
                        }
                        catch (Exception ex)
                        {

                        }
                        context.AddObject(tableName, sys_webpages);
                        break;
                    case "SYS_WEBPAGES_LOG":
                        SYS_WEBPAGES_LOG sys_webpages_log = (SYS_WEBPAGES_LOG)param[0];
                        if (param.Count() > 2 && param[1].ToString().StartsWith("ServerComponentCS:"))
                        {
                            String sServerComponentCS = param[1].ToString().Replace("ServerComponentCS:", String.Empty);
                            String sAssemblyName = param[2].ToString();
                            sys_webpages_log.SERVERDLL = SDModuleProvider.CreateServerDllAssembly(sServerComponentCS, sAssemblyName, string.Empty, this.ClientInfo);
                        }
                        sys_webpages_log.EntityKey = context.CreateEntityKey(tableName, sys_webpages_log);
                        try
                        {
                            SYS_WEBPAGES_LOG sys_webpages_log1 = context.GetObjectByKey(sys_webpages_log.EntityKey) as SYS_WEBPAGES_LOG;
                            context.DeleteObject(sys_webpages_log1);
                            context.SaveChanges();
                        }
                        catch (Exception ex)
                        {

                        }
                        context.AddObject(tableName, sys_webpages_log);
                        break;
                    case "SYS_SDSOLUTIONS":
                        SYS_SDSOLUTIONS sys_sdsolutions = (SYS_SDSOLUTIONS)param[0];
                        sys_sdsolutions.EntityKey = context.CreateEntityKey(tableName, sys_sdsolutions);
                        try
                        {
                            SYS_SDSOLUTIONS sys_sdsolutions1 = context.GetObjectByKey(sys_sdsolutions.EntityKey) as SYS_SDSOLUTIONS;
                            context.DeleteObject(sys_sdsolutions1);
                            context.SaveChanges();
                        }
                        catch (Exception ex)
                        {

                        }
                        context.AddObject(tableName, sys_sdsolutions);
                        break;
                    case "SYS_SDALIAS":
                        SYS_SDALIAS sys_sdalias = (SYS_SDALIAS)param[0];
                        sys_sdalias.EntityKey = context.CreateEntityKey(tableName, sys_sdalias);
                        try
                        {
                            SYS_SDALIAS sys_sdalias1 = context.GetObjectByKey(sys_sdalias.EntityKey) as SYS_SDALIAS;
                            context.DeleteObject(sys_sdalias1);
                            context.SaveChanges();
                        }
                        catch (Exception ex)
                        {

                        }
                        context.AddObject(tableName, sys_sdalias);
                        break;
                    case "SYS_SDUSERS":
                        SYS_SDUSERS sys_sdusers = (SYS_SDUSERS)param[0];
                        sys_sdusers.EntityKey = context.CreateEntityKey(tableName, sys_sdusers);
                        try
                        {
                            SYS_SDUSERS sys_sdusers1 = context.GetObjectByKey(sys_sdusers.EntityKey) as SYS_SDUSERS;
                            context.DeleteObject(sys_sdusers1);
                            context.SaveChanges();
                        }
                        catch { }
                        context.AddObject(tableName, sys_sdusers);
                        break;
                    case "SYS_SDGROUPS":
                        SYS_SDGROUPS sys_sdgroups = (SYS_SDGROUPS)param[0];
                        sys_sdgroups.EntityKey = context.CreateEntityKey(tableName, sys_sdgroups);
                        try
                        {
                            SYS_SDGROUPS sys_sdusers1 = context.GetObjectByKey(sys_sdgroups.EntityKey) as SYS_SDGROUPS;
                            context.DeleteObject(sys_sdgroups);
                            context.SaveChanges();
                        }
                        catch { }
                        context.AddObject(tableName, sys_sdgroups);
                        break;
                }

                context.SaveChanges();
            }
        }

        public void DeleteDataFromTableForSDSystemTable(object param, String tableName)
        {
            var clientInfo = this.ClientInfo;
            if (clientInfo == null)
            {
                throw new ArgumentNullException("clientInfo");
            }
            ClientInfo.SDDeveloperID = null;//清除设计者ID
            using (var context = new Entities(CreateEntityConnection(Database.SystemDatabase, true)))
            {
                context.Connection.Open();

                switch (tableName)
                {
                    case "SYS_SDSOLUTIONS":

                        SYS_SDSOLUTIONS sys_sdsolutions = (SYS_SDSOLUTIONS)param;
                        sys_sdsolutions.EntityKey = context.CreateEntityKey(tableName, sys_sdsolutions);
                        try
                        {
                            SYS_SDSOLUTIONS sys_sdsolutions1 = context.GetObjectByKey(sys_sdsolutions.EntityKey) as SYS_SDSOLUTIONS;
                            context.DeleteObject(sys_sdsolutions1);
                            context.SaveChanges();
                        }
                        catch (Exception ex)
                        {

                        }
                        break;
                    case "SYS_SDALIAS":
                        SYS_SDALIAS sys_sdalias = (SYS_SDALIAS)param;
                        sys_sdalias.EntityKey = context.CreateEntityKey(tableName, sys_sdalias);
                        try
                        {
                            SYS_SDALIAS sys_sdqueue1 = context.GetObjectByKey(sys_sdalias.EntityKey) as SYS_SDALIAS;
                            context.DeleteObject(sys_sdqueue1);
                            context.SaveChanges();
                        }
                        catch (Exception ex)
                        {

                        }
                        break;
                    case "SYS_SDQUEUE":
                        SYS_SDQUEUE sys_sdqueue = (SYS_SDQUEUE)param;
                        sys_sdqueue.EntityKey = context.CreateEntityKey(tableName, sys_sdqueue);
                        try
                        {
                            SYS_SDQUEUE sys_sdqueue1 = context.GetObjectByKey(sys_sdqueue.EntityKey) as SYS_SDQUEUE;
                            sys_sdqueue1.SYS_SDQUEUEPAGE.Load();
                            context.DeleteObject(sys_sdqueue1);
                            context.SaveChanges();
                        }
                        catch (Exception ex)
                        {

                        }
                        break;
                    case "SYS_WEBPAGES":
                        SYS_WEBPAGES sys_webpages = (SYS_WEBPAGES)param;
                        sys_webpages.EntityKey = context.CreateEntityKey(tableName, sys_webpages);
                        try
                        {
                            SYS_WEBPAGES sys_webpages1 = context.GetObjectByKey(sys_webpages.EntityKey) as SYS_WEBPAGES;
                            context.DeleteObject(sys_webpages1);
                            DeleteSysWebRuntime(context, sys_webpages1.PAGENAME, sys_webpages1.USERID, sys_webpages1.SOLUTIONID);
                            context.SaveChanges();
                        }
                        catch (Exception ex)
                        {

                        }
                        break;
                    case "SYS_WEBPAGES_LOG":
                        SYS_WEBPAGES_LOG sys_webpages_log = (SYS_WEBPAGES_LOG)param;
                        sys_webpages_log.EntityKey = context.CreateEntityKey(tableName, sys_webpages_log);
                        try
                        {
                            SYS_WEBPAGES_LOG sys_webpages_log1 = context.GetObjectByKey(sys_webpages_log.EntityKey) as SYS_WEBPAGES_LOG;
                            context.DeleteObject(sys_webpages_log1);
                            context.SaveChanges();
                        }
                        catch (Exception ex)
                        {

                        }
                        break;
                    case "SYS_SDUSERS":
                        SYS_SDUSERS sys_sdusers = (SYS_SDUSERS)param;
                        sys_sdusers.EntityKey = context.CreateEntityKey(tableName, sys_sdusers);
                        try
                        {
                            SYS_SDUSERS sys_sdusers1 = context.GetObjectByKey(sys_sdusers.EntityKey) as SYS_SDUSERS;
                            context.DeleteObject(sys_sdusers1);
                            context.SaveChanges();
                        }
                        catch { }
                        break;
                    case "SYS_SDGROUPS":
                        SYS_SDGROUPS sys_sdgroups = (SYS_SDGROUPS)param;
                        sys_sdgroups.EntityKey = context.CreateEntityKey(tableName, sys_sdgroups);
                        try
                        {
                            SYS_SDGROUPS sys_sdusers1 = context.GetObjectByKey(sys_sdgroups.EntityKey) as SYS_SDGROUPS;
                            context.DeleteObject(sys_sdgroups);
                            context.SaveChanges();
                        }
                        catch { }
                        break;
                }

                context.SaveChanges();
            }
        }

        private void DeleteSysWebRuntime(Entities context, string pageName, string userID, string solutionID)
        {
            var pages = from c in context.SYS_WEBRUNTIME where c.PAGENAME == pageName && c.USERID == userID && c.SOLUTIONID == solutionID select c;
            foreach (var page in pages)
            {
                context.DeleteObject(page);
            }
        }

        public List<EntityObject> GetSecurityTableForSDDesign(String strTableName)
        {
            String dbAlias = String.Empty;
            if (Database.GetSplitSystemTable(ClientInfo.Database, ClientInfo.SDDeveloperID))
            {
                dbAlias = Database.GetSystemDatabase(ClientInfo.SDDeveloperID);
            }
            else
            {
                dbAlias = ClientInfo.Database;
            }

            return GetAllDataByTableName(dbAlias, strTableName);
        }

        public void SaveDataToTableForSDDesignSecurityTable(object[] param, String tableName)
        {
            var clientInfo = this.ClientInfo;
            if (clientInfo == null)
            {
                throw new ArgumentNullException("clientInfo");
            }
            String dbAlias = String.Empty;
            if (Database.GetSplitSystemTable(ClientInfo.Database, ClientInfo.SDDeveloperID))
            {
                dbAlias = Database.GetSystemDatabase(ClientInfo.SDDeveloperID);
            }
            else
            {
                dbAlias = ClientInfo.Database;
            }

            using (var context = new Entities(CreateEntityConnection(dbAlias, true)))
            {
                context.Connection.Open();

                switch (tableName)
                {
                    case "USERGROUPS":
                        String sGroupID = param[0].ToString();
                        String sSql = String.Format("DELETE FROM USERGROUPS WHERE GROUPID='{0}'", sGroupID);
                        context.ExecuteNonQuery(sSql);
                        context.SaveChanges();

                        String[] sUsers = param[1].ToString().Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (var item in sUsers)
                        {
                            USERGROUPS usergroups = new USERGROUPS();
                            usergroups.USERID = item;
                            usergroups.GROUPID = sGroupID;
                            usergroups.EntityKey = context.CreateEntityKey(tableName, usergroups);

                            try
                            {
                                context.AddObject(tableName, usergroups);
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                        break;
                    case "MENUTABLECONTROL":
                        if (param != null)
                        {
                            MENUTABLECONTROL menucontrol = (MENUTABLECONTROL)param[0];
                            menucontrol.EntityKey = context.CreateEntityKey(tableName, menucontrol);
                            try
                            {
                                MENUTABLECONTROL item1 = context.GetObjectByKey(menucontrol.EntityKey) as MENUTABLECONTROL;
                                context.DeleteObject(item1);
                                context.SaveChanges();
                            }
                            catch (Exception ex)
                            {

                            }
                            context.AddObject(tableName, menucontrol);

                            //List<MENUTABLECONTROL> menucontrol = (List<MENUTABLECONTROL>)param;
                            //foreach (MENUTABLECONTROL item in menucontrol)
                            //{
                            //    item.EntityKey = context.CreateEntityKey("MENUTABLECONTROL", item);
                            //    try
                            //    {
                            //        MENUTABLECONTROL item1 = context.GetObjectByKey(item.EntityKey) as MENUTABLECONTROL;
                            //        context.DeleteObject(item1);
                            //        context.SaveChanges();
                            //    }
                            //    catch (Exception ex)
                            //    {

                            //    }
                            //    context.AddObject("MENUTABLECONTROL", item);
                            //}
                        }
                        break;
                    case "SYSEEPLOG":
                        SYSEEPLOG syseeplog = (SYSEEPLOG)param[0];
                        syseeplog.EntityKey = context.CreateEntityKey(tableName, syseeplog);
                        try
                        {
                            //var lrefvals = (from r in context.SYS_REFVAL
                            //                where r.REFVAL_NO.Equals(refval.REFVAL_NO)
                            //                select r).ToList();
                            SYSEEPLOG syseeplog1 = context.GetObjectByKey(syseeplog.EntityKey) as SYSEEPLOG;
                            context.DeleteObject(syseeplog1);
                            context.SaveChanges();
                        }
                        catch (Exception ex)
                        {

                        }
                        context.AddObject(tableName, syseeplog);
                        break;
                    case "MENUTABLE":
                        MENUTABLE menutable = (MENUTABLE)param[0];
                        String sStatus = param[1].ToString();
                        menutable.EntityKey = context.CreateEntityKey(tableName, menutable);
                        if (sStatus == "Modify")
                        {
                            try
                            {
                                MENUTABLE menutable1 = context.GetObjectByKey(menutable.EntityKey) as MENUTABLE;
                                context.DeleteObject(menutable1);
                                context.SaveChanges();
                            }
                            catch (Exception ex)
                            {

                            }
                        }

                        try
                        {
                            context.AddObject(tableName, menutable);
                        }
                        catch (Exception ex)
                        {

                        }
                        break;
                    case "GROUPMENUS":
                        String sMenuID = param[0].ToString();
                        String sGROUPMENUSSql = String.Format("DELETE FROM GROUPMENUS WHERE MENUID='{0}'", sMenuID);
                        context.ExecuteNonQuery(sGROUPMENUSSql);
                        context.SaveChanges();

                        String[] sGroupIDs = param[1].ToString().Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (var item in sGroupIDs)
                        {
                            GROUPMENUS groupmenus = new GROUPMENUS();
                            groupmenus.MENUID = sMenuID;
                            groupmenus.GROUPID = item;
                            groupmenus.EntityKey = context.CreateEntityKey(tableName, groupmenus);

                            try
                            {
                                context.AddObject(tableName, groupmenus);
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                        break;
                    case "USERMENUS":
                        String smenuID = param[0].ToString();
                        String sUSERMENUSSql = String.Format("DELETE FROM USERMENUS WHERE MENUID='{0}'", smenuID);
                        context.ExecuteNonQuery(sUSERMENUSSql);
                        context.SaveChanges();

                        String[] sUserIDs = param[1].ToString().Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (var item in sUserIDs)
                        {
                            USERMENUS usermenus = new USERMENUS();
                            usermenus.MENUID = smenuID;
                            usermenus.USERID = item;
                            usermenus.EntityKey = context.CreateEntityKey(tableName, usermenus);

                            try
                            {
                                context.AddObject(tableName, usermenus);
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                        break;

                }

                context.SaveChanges();
            }
        }

        public void DeleteDataFromTableForSDDesignSecurityTable(object param, String tableName)
        {
            var clientInfo = this.ClientInfo;
            if (clientInfo == null)
            {
                throw new ArgumentNullException("clientInfo");
            }
            String dbAlias = String.Empty;
            if (Database.GetSplitSystemTable(ClientInfo.Database, ClientInfo.SDDeveloperID))
            {
                dbAlias = Database.GetSystemDatabase(ClientInfo.SDDeveloperID);
            }
            else
            {
                dbAlias = ClientInfo.Database;
            }

            using (var context = new Entities(CreateEntityConnection(dbAlias, true)))
            {
                context.Connection.Open();

                switch (tableName)
                {
                    case "MENUTABLECONTROL":
                        if (param != null)
                        {
                            MENUTABLECONTROL menucontrol = (MENUTABLECONTROL)param;
                            menucontrol.EntityKey = context.CreateEntityKey("MENUTABLECONTROL", menucontrol);
                            try
                            {
                                MENUTABLECONTROL item1 = context.GetObjectByKey(menucontrol.EntityKey) as MENUTABLECONTROL;
                                context.DeleteObject(item1);
                                context.SaveChanges();
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                        break;
                    case "MENUTABLE":
                        if (param != null)
                        {
                            MENUTABLE menutable = (MENUTABLE)param;
                            menutable.EntityKey = context.CreateEntityKey("MENUTABLE", menutable);
                            try
                            {
                                MENUTABLE item1 = context.GetObjectByKey(menutable.EntityKey) as MENUTABLE;
                                context.DeleteObject(item1);
                                context.SaveChanges();
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                        break;
                    case "USERMENUS":
                        if (param != null)
                        {
                            USERMENUS usermenus = (USERMENUS)param;
                            usermenus.EntityKey = context.CreateEntityKey("USERMENUS", usermenus);
                            try
                            {
                                USERMENUS item1 = context.GetObjectByKey(usermenus.EntityKey) as USERMENUS;
                                context.DeleteObject(item1);
                                context.SaveChanges();
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                        break;
                    case "GROUPMENUS":
                        if (param != null)
                        {
                            GROUPMENUS groupmenus = (GROUPMENUS)param;
                            groupmenus.EntityKey = context.CreateEntityKey("GROUPMENUS", groupmenus);
                            try
                            {
                                GROUPMENUS item1 = context.GetObjectByKey(groupmenus.EntityKey) as GROUPMENUS;
                                context.DeleteObject(item1);
                                context.SaveChanges();
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                        break;
                    case "USERGROUPS":
                        if (param != null)
                        {
                            USERGROUPS usergroups = (USERGROUPS)param;
                            usergroups.EntityKey = context.CreateEntityKey("USERGROUPS", usergroups);
                            try
                            {
                                USERGROUPS item1 = context.GetObjectByKey(usergroups.EntityKey) as USERGROUPS;
                                context.DeleteObject(item1);
                                context.SaveChanges();
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                        break;
                    case "GROUPS":
                        if (param != null)
                        {
                            GROUPS groups = (GROUPS)param;
                            groups.EntityKey = context.CreateEntityKey("GROUPS", groups);
                            try
                            {
                                GROUPS item1 = context.GetObjectByKey(groups.EntityKey) as GROUPS;
                                context.DeleteObject(item1);
                                context.SaveChanges();
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                        break;
                    case "USER":
                        if (param != null)
                        {
                            USER user = (USER)param;
                            user.EntityKey = context.CreateEntityKey("USER", user);
                            try
                            {
                                USER item1 = context.GetObjectByKey(user.EntityKey) as USER;
                                context.DeleteObject(item1);
                                context.SaveChanges();
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                        break;
                }

                context.SaveChanges();
            }
        }

        public void InsertUserLogForSD()
        {
            var clientInfo = this.ClientInfo;
            if (clientInfo == null)
            {
                throw new ArgumentNullException("clientInfo");
            }
            var dateTime = DateTime.Now;
            var sql = string.Format("INSERT INTO SYS_SDUSERS_LOG (USERID, IPADDRESS, LOGINTIME, LOGOUTTIME) VALUES ('{0}', '{1}', '{2:yyyy-MM-dd HH:mm:ss}', '{3:yyyy-MM-dd HH:mm:ss}')"
                , clientInfo.UserID, ClientInfo.IPAddress, dateTime, dateTime);
            Database.ExecuteCommand(Database.SystemDatabase, sql);
        }

        public void RefreshUserLogForSD()
        {
            var clientInfo = this.ClientInfo;
            if (clientInfo == null)
            {
                throw new ArgumentNullException("clientInfo");
            }
            var dateTime = DateTime.Now;
            var sql = string.Format("UPDATE SYS_SDUSERS_LOG SET LOGOUTTIME = '{0:yyyy-MM-dd HH:mm:ss}' WHERE ID= (SELECT MAX(ID) FROM SYS_SDUSERS_LOG where USERID = '{1}')"
              , dateTime, clientInfo.UserID);
            Database.ExecuteCommand(Database.SystemDatabase, sql);
        }


        #endregion

        #endregion

        private bool CheckPassword(string userid, string password, string passwordInDatabase)
        {
            if (string.IsNullOrEmpty(userid))
            {
                throw new ArgumentNullException("userid");
            }
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            if (password.Length > 0)
            {
                char[] p = new char[] { };
                bool q = EFWCFModule.EEPAdapter.Encrypt.EncryptPassword(userid, password, 10, ref p, false);
                String pwd = new String(p);
                return pwd.Equals(passwordInDatabase);
            }
            else
            {
                return string.IsNullOrEmpty(passwordInDatabase);
            }
        }

        private EntityConnection CreateEntityConnection(string database, bool useSystemDatabase)
        {
            if (string.IsNullOrEmpty(database))
            {
                throw new ArgumentNullException("database");
            }

            System.Reflection.Assembly assembly = null;
            if (useSystemDatabase)
            {
                assembly = this.GetType().Assembly;
            }
            else
            {
                DataBaseType dbType = EFWCFModule.EEPAdapter.DatabaseProvider.GetProviderType(database, ClientInfo.SDDeveloperID);
                switch (dbType)
                {
                    case DataBaseType.MsSql:
                        assembly = System.Reflection.Assembly.LoadFile(GetServerPath() + "\\EFGlobalModule.dll");
                        break;
                    case DataBaseType.Oracle:
                        //assembly = this.GetType().Assembly;
                        assembly = System.Reflection.Assembly.LoadFile(GetServerPath() + "\\EFGlobalModule_Oracle.dll");
                        break;
                }
            }

            var metadataFile = "EFModel";
            return EntityProvider.CreateEntityConnection(database, assembly, metadataFile, useSystemDatabase, ClientInfo.SDDeveloperID);
        }

        #region Global Method

        public List<EntityObject> GetUsersWithGroupID(object[] param)
        {
            var clientInfo = this.ClientInfo;
            var database = clientInfo.Database;
            var list = new List<EntityObject>();
            using (var context = new Entities(CreateEntityConnection(database, true)))
            {
                String groupid = param[0].ToString();
                var listu = (from u in context.USERS
                             join ug in context.USERGROUPS on u.USERID equals ug.USERID
                             join g in context.GROUPS on ug.GROUPID equals g.GROUPID
                             where string.Compare(g.ISROLE, "Y", true) == 0
                             && string.Compare(g.GROUPID, groupid, true) == 0
                             select u).Distinct();
                foreach (var obj in listu)
                {
                    list.Add(obj);
                }
            }
            return list;
        }

        public void SaveReport(object[] param)
        {
            var clientInfo = this.ClientInfo;
            if (clientInfo == null)
            {
                throw new ArgumentNullException("clientInfo");
            }

            SYS_REPORT sysReport = (SYS_REPORT)param[0];
            EntityState state = (EntityState)param[1];

            using (var context = new Entities(CreateEntityConnection(clientInfo.Database, true)))
            {
                context.Connection.Open();

                switch (state)
                {
                    case EntityState.Added:
                        context.AddObject("SYS_REPORT", sysReport);
                        break;
                    case EntityState.Deleted:
                        context.Attach(sysReport);
                        context.DeleteObject(sysReport);
                        break;
                    case EntityState.Modified:
                        context.AttachUpdated(sysReport);
                        break;
                }

                context.SaveChanges();
            }
        }

        public void Upload(object[] param)
        {
            var fileName = (string)param[0];
            var buffer = (byte[])param[1];
            var filePath = Path.Combine(Environment.CurrentDirectory, "Files", fileName);
            var dir = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            if (!File.Exists(filePath))
            {
                File.WriteAllBytes(filePath, buffer);
            }
        }

        public byte[] Download(object[] param)
        {
            var fileName = (string)param[0];
            var filePath = Path.Combine(Environment.CurrentDirectory, "Files", fileName);
            if (File.Exists(filePath))
            {
                var buffer = File.ReadAllBytes(filePath);
                return buffer;
            }
            else
            {
                return null;
            }
        }

        private List<String> GetPasswordPolicy()
        {
            List<String> passwordInfo = null;
            String file = String.Format("{0}\\Login.xml", GetServerPath());
            if (File.Exists(file))
            {
                passwordInfo = new List<String>();
                XmlDocument xml = new XmlDocument();
                try
                {
                    xml.Load(file);

                    XmlNode node = xml.SelectSingleNode("InfolightAllowUserToPerLogin/PasswordPolicy");
                    if (node != null)
                    {
                        passwordInfo.Add(node.Attributes["CharNum"].Value);//PasswordCharNum
                        passwordInfo.Add(node.Attributes["MinSize"].Value);//PassWordMinSize
                        passwordInfo.Add(node.Attributes["MaxSize"].Value);//passwordMaxSize
                    }
                }
                catch { }
            }
            return passwordInfo;
        }

        public object ChangePassword(object[] objParam)
        {
            var clientInfo = this.ClientInfo;
            if (clientInfo == null)
            {
                throw new ArgumentNullException("clientInfo");
            }
            List<String> passwordInfo = GetPasswordPolicy();

            String sUserId = objParam[0].ToString();
            String sOldPWD = objParam[1].ToString();
            String sNewPWD = objParam[2].ToString();

            if (passwordInfo != null)
            {
                if (sNewPWD.Length > Convert.ToInt32(passwordInfo[2]) || sNewPWD.Length < Convert.ToInt32(passwordInfo[1]))
                {
                    return passwordInfo[2] + ";" + passwordInfo[1];
                }

                if (Convert.ToBoolean(passwordInfo[0]) == true)
                {
                    int x = 0, y = 0;
                    for (int i = 0; i < sNewPWD.Length; i++)
                    {
                        if (!char.IsLetterOrDigit(sNewPWD, i))
                        {
                            return "C";
                        }
                        else if (char.IsLetter(sNewPWD, i))
                        {
                            x++;
                        }
                        else if (char.IsNumber(sNewPWD, i))
                        {
                            y++;
                        }
                    }
                    if (x == 0 || y == 0)
                    {
                        return "N";
                    }
                }
            }

            char[] p = new char[] { };
            bool q;
            if (sOldPWD != "") q = Encrypt.EncryptPassword(sUserId, sOldPWD, 10, ref p, false);
            string sOldPwd = new string(p);

            p = new char[] { };
            if (sNewPWD != "") q = Encrypt.EncryptPassword(sUserId, sNewPWD, 10, ref p, false);
            string sNewPwd = new string(p);

            using (var context = new Entities(CreateEntityConnection(clientInfo.Database, true)))
            {
                context.Connection.Open();

                var listUser = (from u in context.USERS
                                where u.USERID == sUserId
                                && u.PWD == sOldPwd
                                select u).ToList();

                if (listUser.Count == 0)
                {
                    return "E";
                }
                else
                {
                    USER u = listUser[0];
                    u.PWD = sNewPwd;
                    u.LASTDATE = DateTime.Today.Year.ToString() + DateTime.Today.Month.ToString("00") + DateTime.Today.Day.ToString("00");

                    context.AttachUpdated(u);
                    context.SaveChanges();
                    return "O";
                }
            }
        }

        public object ResetUsersPassword(object[] objParam)
        {
            var clientInfo = this.ClientInfo;
            if (clientInfo == null)
            {
                throw new ArgumentNullException("clientInfo");
            }
            List<String> passwordInfo = GetPasswordPolicy();

            String sUserId = objParam[0].ToString();

            using (var context = new Entities(CreateEntityConnection(clientInfo.Database, true)))
            {
                context.Connection.Open();

                var listUser = (from u in context.USERS
                                where u.USERID == sUserId
                                select u).ToList();

                if (listUser.Count == 0)
                {
                    return "E";
                }
                else
                {
                    USER u = listUser[0];
                    u.PWD = String.Empty;
                    u.LASTDATE = String.Format("{0}{1}{2}", DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
                    u.LASTTIME = String.Format("{0}{1}{2}", DateTime.Today.Hour, DateTime.Today.Minute, DateTime.Today.Second);

                    context.AttachUpdated(u);
                    context.SaveChanges();
                    return "O";
                }
            }
        }


        public string GloFix_ShowMessage(object[] param)
        {
            String SendToIds = param[0].ToString();
            bool isWeb = (bool)param[1];
            object[] clientInfo = ClientInfoExtension.ToArray(this.ClientInfo);
            String retString = EFWCFModule.EEPAdapter.GloFix.ShowMessage(SendToIds, isWeb, clientInfo);
            return retString;
        }

        public string GloFix_ShowParallelMessage(object[] param)
        {
            String SendToIds = param[0].ToString();
            object[] clientInfo = ClientInfoExtension.ToArray(this.ClientInfo);
            String retString = EFWCFModule.EEPAdapter.GloFix.ShowParallelMessage(SendToIds, clientInfo);
            return retString;
        }

        public string GloFix_ShowNotifyMessage(object[] param)
        {
            String SendToIds = param[0].ToString();
            object[] clientInfo = ClientInfoExtension.ToArray(this.ClientInfo);
            String retString = EFWCFModule.EEPAdapter.GloFix.ShowNotifyMessage(SendToIds, clientInfo);
            return retString;
        }

        public string GloFix_ShowPlusMessage(object[] param)
        {
            String SendToIds = param[0].ToString();
            object[] clientInfo = ClientInfoExtension.ToArray(this.ClientInfo);
            String retString = EFWCFModule.EEPAdapter.GloFix.ShowPlusMessage(SendToIds, clientInfo);
            return retString;
        }

        #endregion

        #region Personal Settings

        public bool SavePersonalSettings(object[] objParam)
        {
            var clientInfo = this.ClientInfo;
            if (clientInfo == null)
            {
                throw new ArgumentNullException("clientInfo");
            }
            if (string.IsNullOrEmpty(clientInfo.UserID))
            {
                throw new ArgumentNullException("clientInfo.UserID");
            }
            string formName = (string)objParam[0];
            string compName = (string)objParam[1];
            string userId = (string)objParam[2];
            string remark = (string)objParam[3];
            string propContent = (string)objParam[4];

            using (var context = new Entities(CreateEntityConnection(clientInfo.Database, true)))
            {
                context.Connection.Open();
                SYS_PERSONAL personal = new SYS_PERSONAL();
                personal.USERID = userId;
                personal.REMARK = remark;
                personal.PROPCONTENT = propContent;
                personal.FORMNAME = formName;
                personal.COMPNAME = compName;
                personal.CREATEDATE = DateTime.Now;
                personal.EntityKey = context.CreateEntityKey("SYS_PERSONAL", personal);

                try
                {
                    SYS_PERSONAL personal1 = context.GetObjectByKey(personal.EntityKey) as SYS_PERSONAL;
                    context.DeleteObject(personal1);
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                }

                context.AddObject("SYS_PERSONAL", personal);
                context.SaveChanges();
                return true;
            }
        }

        public String LoadPersonalSettings(object[] objParam)
        {
            var clientInfo = this.ClientInfo;
            if (clientInfo == null)
            {
                throw new ArgumentNullException("clientInfo");
            }
            if (string.IsNullOrEmpty(clientInfo.UserID))
            {
                throw new ArgumentNullException("clientInfo.UserID");
            }
            string formName = (string)objParam[0];
            string compName = (string)objParam[1];
            string userId = (string)objParam[2];
            using (var context = new Entities(CreateEntityConnection(clientInfo.Database, true)))
            {
                context.Connection.Open();

                var listPR = (from pr in context.SYS_PERSONAL
                              where pr.USERID.Equals(userId) && pr.FORMNAME.Equals(formName) && pr.COMPNAME.Equals(compName)
                              select pr.PROPCONTENT).ToList();
                List<String> propcontent = new List<String>();
                foreach (var pr in listPR)
                {
                    propcontent.Add(pr);
                }
                if (propcontent.Count == 0 || propcontent.Count > 1)
                    return "";
                else
                    return propcontent[0];
            }
        }

        #endregion

        private void ucUsers_BeforeInsert(object sender, EFUpdateComponentUpdateEventArgs e)
        {
            if (e.Object != null)
            {
                System.Reflection.PropertyInfo piUSERID = e.Object.GetType().GetProperty("USERID");
                String sUSERID = piUSERID.GetValue(e.Object, null).ToString();
                System.Reflection.PropertyInfo piPWD = e.Object.GetType().GetProperty("PWD");
                if (piPWD != null)
                {
                    object oPassword = piPWD.GetValue(e.Object, null);
                    if (oPassword != null)
                    {
                        String sPassword = oPassword.ToString();
                        if (sPassword.Length > 0)
                        {
                            char[] p = new char[] { };
                            EFWCFModule.EEPAdapter.Encrypt.EncryptPassword(sUSERID, sPassword, 10, ref p, false);
                            sPassword = new String(p);
                            piPWD.SetValue(e.Object, sPassword, null);
                        }
                    }
                }
            }
        }
    }

    public class SYS_SDUSERS_LOG_STAT : EntityObject
    {
        public string UserID { get; set; }

        //public string UserName { get; set; }

        public DateTime LastDateTime { get; set; }

        public int TotalMinutes { get; set; }

        public int LoginCount { get; set; }
    }
}
