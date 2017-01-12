<%@ WebHandler Language="C#" Class="SystemHandler_Security" %>

using System;
using System.Web;
using System.Data;
using System.Xml;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Reflection;
using System.ComponentModel;
using System.Collections.Generic;
using EFClientTools.EFServerReference;
using System.Text.RegularExpressions;
using System.Collections;

/// <summary>
/// 与系统菜单有关的方法
/// </summary>
public class SystemHandler_Security : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    public void ProcessRequest(HttpContext context)
    {
        try
        {
            Context = context;
            if (Type == "MenuTables")
            {
                if (Mode == "Add" || Mode == "Modify" || Mode == "Delete")
                    InsertMenuTable(context);
                else if (Mode == "GroupMenus")
                    GetGroupMenus(context);
                else if (Mode == "InsertGroupMenus")
                    InsertGroupMenus(context);
                else if (Mode == "UserMenus")
                    GetUserMenus(context);
                else if (Mode == "InsertUserMenus")
                    InsertUserMenus(context);
                else if (Mode == "Pages")
                    GetPages(context);
                else if (Mode == "AutoSeqMenuID")
                    GetAutoSeqMenuID(context);
                else if (Mode == "Runtime")
                    GetRuntimeMenuTables(context);
                else
                    GetMenuTables(context);
            }
            else if (Type == "Solution")
            {
                GetSolutions(context);
            }
            else if (Type == "ClientInfo")
            {
                GetClientInfo(context);
            }
            else if (Type == "Security")
            {
                if (Mode == "Update")
                {
                    UpdateSecurityData(context);
                }
                else if (Mode == "UpdateDetail")
                {
                    UpdateDetailSecurityData(context);
                }
                else
                {
                    GetSecurityData(context);
                }
            }
            else if (Type == "GetGroupMenuControl")
            {
                GetGroupMenuControl(context);
            }
            else if (Type == "GetUserMenuControl")
            {
                GetUserMenuControl(context);
            }
            else if (Type == "SaveGroupMenuControl")
            {
                SaveGroupMenuControl(context);
            }
            else if (Type == "SaveUserMenuControl")
            {
                SaveUserMenuControl(context);
            }
            else if (Type == "GetSecurity")
            {
                GetSecurity(context);
            }
            else
            {
                throw new NotImplementedException();
            }
        }
        catch (Exception e)
        {
            var ex = e;
            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
            }
            throw ex;
            //context.Response.StatusCode = 500;
            //var exception = new JObject();
            //exception["message"] = ex.Message;
            //context.Response.Write(JsonConvert.SerializeObject(exception));
        }
    }

    private string DataTableToJson(DataTable dt)
    {
        return Newtonsoft.Json.JsonConvert.SerializeObject(dt, Newtonsoft.Json.Formatting.Indented);//Indented縮排
    }

    public HttpContext Context { get; set; }

    public string Type
    {
        get
        {
            return Context.Request.QueryString["type"];
        }
    }

    public string Mode
    {
        get
        {
            return Context.Request.Form["mode"];
        }
    }

    public string PageType
    {
        get
        {
            var type = Context.Request.Form["type"];
            var pageType = string.Empty;
            switch (type)
            {
                case "server": pageType = "S"; break;
                case "client": pageType = "W"; break;
                case "mobile": pageType = "M"; break;
            }
            return pageType;
        }
    }

    public string UserID   //除了sys_sdsolution和sys_sdalias表用userid 其他都用developerid来实现同群组的操作
    {
        get
        {
            return EFClientTools.ClientUtility.ClientInfo.UserID;
        }
    }

    public string DeveloperID
    {
        get
        {
            return EFClientTools.ClientUtility.ClientInfo.SDDeveloperID;
        }
    }

    public string RuntimeDeveloperID  //runtime登入前的三个方法使用，和设计时区分
    {
        get
        {
            return Context.Request.Form["developer"];
        }
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

    private void GetClientInfo(HttpContext context)
    {
        var clientInfo = new JObject();
        clientInfo["UserID"] = UserID;
        clientInfo["UserName"] = EFClientTools.ClientUtility.ClientInfo.UserName;
        clientInfo["Database"] = EFClientTools.ClientUtility.ClientInfo.Database;
        if (!string.IsNullOrEmpty(EFClientTools.ClientUtility.ClientInfo.Solution))
        {
            clientInfo["Solution"] = EFClientTools.ClientUtility.ClientInfo.Solution;
            //clientInfo["Solution"] = string.Format("{0}@{1}", EFClientTools.ClientUtility.ClientInfo.Solution, DeveloperID);
        }
        var locale = EFClientTools.ClientUtility.ClientInfo.Locale.ToLower();
        clientInfo["Locale"] = locale; ;

        var json = JsonConvert.SerializeObject(clientInfo);
        context.Response.Write(json);
    }

    const string SELECT_SOLUTION_SQL = "SELECT * FROM MENUITEMTYPE";
    const string SELECT_USER_SQL = "SELECT USERS.*, PWD as OPassword FROM USERS";
    const string SELECT_USER_DETAIL_SQL = "SELECT *, GROUPS.GROUPNAME FROM USERGROUPS LEFT JOIN GROUPS ON USERGROUPS.GROUPID = GROUPS.GROUPID WHERE USERGROUPS.USERID = '{0}'";
    const string SELECT_GROUP_SQL = "SELECT * FROM GROUPS";
    const string SELECT_GROUP_DETAIL_SQL = "SELECT *, USERS.USERNAME FROM USERGROUPS LEFT JOIN USERS ON USERGROUPS.USERID = USERS.USERID WHERE USERGROUPS.GROUPID = '{0}'";
    const string SELECT_MENU_SQL = "SELECT * FROM MENUTABLE WHERE ITEMTYPE = '{0}'";
    const string ORIGINAL_PASSWORD_FIELD = "OPassword";
    private void GetSecurityData(HttpContext context)
    {
        var type = context.Request.Form["type"];
        var commandText = string.Empty;
        var pageSize = -1;
        if (context.Request.Form["rows"] != null)
        {
            int.TryParse(context.Request.Form["rows"].ToString(), out pageSize);
        }
        var pageIndex = 1;
        if (context.Request.Form["page"] != null)
        {
            int.TryParse(context.Request.Form["page"].ToString(), out pageIndex);
        }
        var packetInfo = new PacketInfo() { WhereParameters = new List<WhereParameter>(), OrderParameters = new List<OrderParameter>(), Count = pageSize, StartIndex = (pageIndex - 1) * pageSize };
        var tableType = SDTableType.SystemTable;

        var parameters = new Dictionary<string, object>();
        var whereItems = new List<string>();
        if (context.Request.Form["queryItems"] != null)
        {
            var queryItems = (JArray)JsonConvert.DeserializeObject(context.Request.Form["queryItems"]);
            int i = 0;
            string mark = "@";
            if (EFClientTools.ClientUtility.ClientInfo.DatabaseType == "Oracle")
            {
                mark = ":";
            }
            foreach (JObject queryItem in queryItems)
            {
                whereItems.Add(string.Format("{0} {1} {3}p{2}", queryItem["field"], queryItem["condition"], i, mark));
                parameters.Add(string.Format("p{0}", i), ((JValue)queryItem["value"]).Value);
                i++;
            }
        }
        if (type == "solution")
        {
            commandText = string.Format(SELECT_SOLUTION_SQL, UserID); //使用userid
            tableType = SDTableType.SDSystemTable;
            parameters = SQLHelper.CreateParameters("UserID", UserID);
        }
        if (type == "user")
        {
            commandText = SELECT_USER_SQL;
            if (whereItems.Count > 0)
            {
                commandText += " WHERE " + string.Join(" AND ", whereItems);
            }
        }
        else if (type == "userdetail")
        {
            commandText = string.Format(SELECT_USER_DETAIL_SQL, context.Request.Form["id"]);
            parameters = SQLHelper.CreateParameters("USERID", context.Request.Form["id"]);
        }
        else if (type == "group")
        {
            commandText = SELECT_GROUP_SQL;
            if (whereItems.Count > 0)
            {
                commandText += " WHERE " + string.Join(" AND ", whereItems);
            }
        }
        else if (type == "groupdetail")
        {
            commandText = string.Format(SELECT_GROUP_DETAIL_SQL, context.Request.Form["id"]);
            parameters = SQLHelper.CreateParameters("GROUPID", context.Request.Form["id"]);
        }
        else if (type == "menu")
        {
            commandText = string.Format(SELECT_MENU_SQL, EFClientTools.ClientUtility.ClientInfo.Solution);
        }

        var dataSet = DataSetHelper.ExecuteSql(new SQLCommandInfo() { CommandText = commandText, Parameters = parameters }, packetInfo, tableType);
        var dataTable = dataSet.Tables[0];
        var countDataSet = DataSetHelper.ExecuteSql(new SQLCommandInfo() { CommandText = DataSetHelper.InsertCount(commandText), Parameters = parameters }, tableType);
        var count = int.Parse(countDataSet.Tables[0].Rows[0][0].ToString());
        context.Response.Write(JsonHelper.CreateGridItems(dataTable, count));
        //context.Response.Write(JsonHelper.CreateGridItems(dataTable, dataTable.Rows.Count));
    }


    private void UpdateSecurityData(HttpContext context)
    {
        var type = context.Request.Form["type"];
        var tableName = string.Empty;
        var userField = string.Empty;
        var passwordField = string.Empty;
        var tableType = SDTableType.SystemTable;
        if (type == "user")
        {
            tableName = "USERS";
            userField = "USERID";
            passwordField = "PWD";
        }
        else if (type == "group")
        {
            tableName = "GROUPS";
        }
        else if (type == "menu")
        {
            tableName = "MENUTABLE";
        }

        var updateRows = new List<UpdateRow>();
        var rows = (JObject)JsonConvert.DeserializeObject(context.Request.Form["rows"]);
        var deleted = (JArray)rows["deleted"];
        foreach (JObject row in deleted)
        {
            updateRows.Add(JsonHelper.CreateUpdateRow(row, EFClientTools.EFServerReference.DataRowState.Deleted));
        }
        var updated = (JArray)rows["updated"];
        foreach (JObject row in updated)
        {
            foreach (var property in row.Properties())
            {
                if (property.Name == passwordField)
                {
                    //A6-Sensitive Data Exposure
                    //var password = (string)row[property.Name];
                    var user = (string)row[userField];
                    //A6-Sensitive Data Exposure
                    //var originalPassword = (string)row[ORIGINAL_PASSWORD_FIELD];

                    if ((string)row[ORIGINAL_PASSWORD_FIELD] != (string)row[property.Name])
                    {
                        row[property.Name] = EncryptPassword(user, (string)row[property.Name]);
                    }
                    else
                    {
                        row[property.Name] = HttpUtility.HtmlDecode((string)row[property.Name]);
                    }
                }
            }
            updateRows.Add(JsonHelper.CreateUpdateRow(row, EFClientTools.EFServerReference.DataRowState.Modified));
        }
        var inserted = (JArray)rows["inserted"];
        foreach (JObject row in inserted)
        {
            if (type == "solution")
            {
                row[userField] = UserID;
            }
            foreach (var property in row.Properties())
            {
                if (property.Name == passwordField)
                {
                    //A6-Sensitive Data Exposure
                    //var password = (string)row[property.Name];
                    var user = (string)row[userField];
                    row[property.Name] = EncryptPassword(user, (string)row[property.Name]);
                }
            }
            updateRows.Add(JsonHelper.CreateUpdateRow(row, EFClientTools.EFServerReference.DataRowState.Added));
        }
        DataSetHelper.UpdateTable(tableName, updateRows, tableType);
    }

    private void GetSolutions(HttpContext context)
    {
        var solutions = RuntimeDeveloperID == null ? GetSolutions(true) : GetSolutions(RuntimeDeveloperID);
        var items = JsonHelper.CreateComboItems(solutions);
        var json = JsonConvert.SerializeObject(items);
        context.Response.Write(json);
    }

    /// <summary>
    /// design
    /// </summary>
    private List<JObject> GetSolutions(bool includeGroupSolutions)
    {
        var solutions = new List<JObject>();
        var dataSet = DataSetHelper.ExecuteSql(string.Format(SELECT_SOLUTION_SQL), SDTableType.SDSystemTable);
        var dataTable = dataSet.Tables[0];
        for (int i = 0; i < dataTable.Rows.Count; i++)
        {
            var solution = new JObject();
            solution["value"] = dataTable.Rows[i]["ITEMTYPE"].ToString();
            solution["text"] = dataTable.Rows[i]["ITEMNAME"].ToString();
            solution["solutionID"] = dataTable.Rows[i]["ITEMTYPE"].ToString();
            solutions.Add(solution);
            //solutions.Add(new DictionaryEntry(dataTable.Rows[i]["SolutionName"], dataTable.Rows[i]["SolutionID"]));
        }
        return solutions;
    }

    /// <summary>
    /// runtime 不需要登入
    /// </summary>
    private List<string> GetSolutionDatabases(string solutionID, string developerID)
    {
        var databases = new List<string>();
        if (!string.IsNullOrEmpty(solutionID) && !string.IsNullOrEmpty(developerID))
        {
            var dataSet = DataSetHelper.GetSolution(solutionID, developerID);
            var dataTable = dataSet.Tables[0];
            if (dataTable.Rows.Count > 0)
            {
                var aliasOptions = dataTable.Rows[0]["AliasOptions"].ToString();
                if (!string.IsNullOrEmpty(aliasOptions))
                {
                    databases.AddRange(aliasOptions.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries));
                }
            }
        }
        return databases;
    }

    /// <summary>
    /// runtime 不需要logon
    /// </summary>
    private List<JObject> GetSolutions(string developerID)
    {
        var solutions = new List<JObject>();
        var dataSet = DataSetHelper.GetSolutions(developerID);
        var dataTable = dataSet.Tables[0];
        for (int i = 0; i < dataTable.Rows.Count; i++)
        {
            var solution = new JObject();
            solution["value"] = dataTable.Rows[i]["SolutionID"].ToString();
            solution["text"] = dataTable.Rows[i]["SolutionName"].ToString();
            solutions.Add(solution);
        }
        return solutions;
    }

    const string DELETE_SOLUTION_SQL = "DELETE FROM MENUITEMTYPE";
    const string INSERT_SOLUTION_SQL = "INSERT INTO MENUITEMTYPE (ITEMTYPE, ITEMNAME) VALUES ('{0}', '{1}')";

    const string DELETE_USER_DETAIL_SQL = "DELETE FROM USERGROUPS WHERE USERID = '{0}'";
    const string DELETE_GROUP_DETAIL_SQL = "DELETE FROM USERGROUPS WHERE GROUPID = '{0}'";
    const string INSERT_USER_GROUP_SQL = "INSERT INTO USERGROUPS (USERID, GROUPID) VALUES ('{0}', '{1}')";
    private void UpdateDetailSecurityData(HttpContext context)
    {
        var type = context.Request.Form["type"];
        var id = context.Request.Form["id"];
        var rows = (JObject)JsonConvert.DeserializeObject(context.Request.Form["rows"]);
        var inserted = (JArray)rows["inserted"];

        var sqls = new List<string>();
        if (type == "userdetail")
        {
            sqls.Add(string.Format(DELETE_USER_DETAIL_SQL, id));
            foreach (JObject row in inserted)
            {
                sqls.Add(string.Format(INSERT_USER_GROUP_SQL, id, (string)row["GROUPID"]));
            }
        }
        else if (type == "groupdetail")
        {
            sqls.Add(string.Format(DELETE_GROUP_DETAIL_SQL, id));
            foreach (JObject row in inserted)
            {
                sqls.Add(string.Format(INSERT_USER_GROUP_SQL, (string)row["USERID"], id));
            }
        }
        DataSetHelper.ExecuteCommand(string.Join(";", sqls), SDTableType.SystemTable);
    }

    private string EncryptPassword(string user, string code)
    {
        if (!string.IsNullOrEmpty(code))
        {
            char[] p = new char[] { };
            Srvtools.Encrypt.EncryptPassword(user, code, 10, ref p, false);
            return new string(p);
        }
        else
        {
            return string.Empty;
        }
    }

    private void GetRuntimeMenuTables(HttpContext context)
    {
        var client = EFClientTools.ClientUtility.Client;
        String solutionId = context.Request.Form["solutionid"];
        if (!String.IsNullOrEmpty(solutionId))
        {
            EFClientTools.ClientUtility.ClientInfo.Solution = solutionId;
        }
        var menus = client.FetchMenus(EFClientTools.ClientUtility.ClientInfo).OfType<EFClientTools.EFServerReference.MENUTABLE>().Where(m => String.IsNullOrEmpty(m.PARENT)).ToList();
        string JsonString = EFClientTools.EntityObjectExtensionMethods.ToEntitiesJson(menus, "MENUTABLE1");
        context.Response.Write(JsonString);
    }

    private void GetMenuTables(HttpContext context)
    {
        var client = EFClientTools.ClientUtility.Client;
        String solutionId = context.Request.Form["solutionid"];
        if (!String.IsNullOrEmpty(solutionId))
        {
            EFClientTools.ClientUtility.ClientInfo.Solution = solutionId;
        }
        var currentGroup = EFClientTools.ClientUtility.ClientInfo.CurrentGroup;
        EFClientTools.ClientUtility.ClientInfo.CurrentGroup = "forManager";
        var menus = client.FetchMenus(EFClientTools.ClientUtility.ClientInfo).OfType<EFClientTools.EFServerReference.MENUTABLE>().Where(m => String.IsNullOrEmpty(m.PARENT)).ToList();
        EFClientTools.ClientUtility.ClientInfo.CurrentGroup = currentGroup;
        string JsonString = EFClientTools.EntityObjectExtensionMethods.ToEntitiesJson(menus, "MENUTABLE1");
        //var dataSet = DataSetHelper.ExecuteSql(String.Format(SELECT_MENUTABLE_SQL, EFClientTools.ClientUtility.ClientInfo.Solution), SDTableType.SystemTable);
        //var dataTable = dataSet.Tables[0];
        //var menutables = new JArray();
        //for (int i = 0; i < dataTable.Rows.Count; i++)
        //{
        //    var menu = new JObject();
        //    menu["MENUID"] = dataTable.Rows[i]["MENUID"].ToString();
        //    menu["CAPTION"] = dataTable.Rows[i]["CAPTION"].ToString();
        //    menu["PARENT"] = dataTable.Rows[i]["PARENT"].ToString();
        //    menu["PACKAGE"] = dataTable.Rows[i]["PACKAGE"].ToString();
        //    menu["MODULETYPE"] = dataTable.Rows[i]["MODULETYPE"].ToString();
        //    menu["ITEMPARAM"] = dataTable.Rows[i]["ITEMPARAM"].ToString();
        //    menu["FORM"] = dataTable.Rows[i]["FORM"].ToString();
        //    menu["ITEMTYPE"] = dataTable.Rows[i]["ITEMTYPE"].ToString();
        //    menu["SEQ_NO"] = dataTable.Rows[i]["SEQ_NO"].ToString();
        //    menu["IMAGEURL"] = dataTable.Rows[i]["IMAGEURL"].ToString();
        //    menutables.Add(menu);
        //}
        //var JsonString = JsonConvert.SerializeObject(menutables);
        context.Response.Write(JsonString);
    }

    const string GET_MAX_MENUTABLEID_SQL = "SELECT MAX(CONVERT(INT,MENUID)) FROM MENUTABLE WHERE ISNUMERIC(MENUID)=1";
    private void GetAutoSeqMenuID(HttpContext context)
    {
        var client = EFClientTools.ClientUtility.Client;
        List<object> lParams = new List<object>();
        lParams.Add(true);
        var i = client.AutoSeqMenuID(EFClientTools.ClientUtility.ClientInfo, lParams);
        context.Response.Write(i);
        //var dataSet = DataSetHelper.ExecuteSql(GET_MAX_MENUTABLEID_SQL, SDTableType.SDSystemTable);
        //var dataTable = dataSet.Tables[0];
        //if (dataTable.Rows[0][0] != null && !string.IsNullOrEmpty(dataTable.Rows[0][0].ToString()))
        //{
        //    int i = Convert.ToInt32(dataTable.Rows[0][0]) + 1;
        //    context.Response.Write(i);
        //}
        //else
        //{
        //    context.Response.Write("0");
        //}
    }

    const string INSERT_MENUTABLE_SQL = "INSERT INTO MENUTABLE (MENUID, CAPTION, PARENT, PACKAGE, MODULETYPE, ITEMPARAM, FORM, ITEMTYPE, SEQ_NO, IMAGEURL, CAPTION0, CAPTION1, CAPTION2, CAPTION3, CAPTION4, CAPTION5, CAPTION6, CAPTION7) VALUES ('{0}','{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}', '{14}', '{15}', '{16}', '{17}')";
    const string UPDATE_MENUTABLE_SQL = "UPDATE MENUTABLE SET CAPTION='{1}', PARENT='{2}', PACKAGE='{3}', MODULETYPE='{4}', ITEMPARAM='{5}', FORM='{6}', ITEMTYPE='{7}', SEQ_NO='{8}', IMAGEURL='{9}', CAPTION0='{10}', CAPTION1='{11}', CAPTION2='{12}', CAPTION3='{13}', CAPTION4='{14}', CAPTION5='{15}', CAPTION6='{16}', CAPTION7='{17}' WHERE MENUID='{0}'";
    const string DELETE_MENUTABLE_SQL = "DELETE FROM MENUTABLE WHERE MENUID='{0}'";
    private void InsertMenuTable(HttpContext context)
    {
        //UpdateMenu(context);
        if (Mode == "Add")
        {
            String sSql = string.Format(INSERT_MENUTABLE_SQL, context.Request.Form["menuid"], context.Request.Form["caption"],
                context.Request.Form["parent"], context.Request.Form["package"], context.Request.Form["moduletype"], context.Request.Form["itemparam"],
                context.Request.Form["form"], context.Request.Form["itemtype"], context.Request.Form["seq_no"], context.Request.Form["imageurl"],
                context.Request.Form["caption0"], context.Request.Form["caption1"], context.Request.Form["caption2"], context.Request.Form["caption3"], context.Request.Form["caption4"], context.Request.Form["caption5"], context.Request.Form["caption6"], context.Request.Form["caption7"]);
            DataSetHelper.ExecuteCommand(sSql, SDTableType.SystemTable);
        }
        else if (Mode == "Modify")
        {
            String sSql = string.Format(UPDATE_MENUTABLE_SQL, context.Request.Form["menuid"], context.Request.Form["caption"],
                context.Request.Form["parent"], context.Request.Form["package"], context.Request.Form["moduletype"], context.Request.Form["itemparam"],
                context.Request.Form["form"], context.Request.Form["itemtype"], context.Request.Form["seq_no"], context.Request.Form["imageurl"],
                context.Request.Form["caption0"], context.Request.Form["caption1"], context.Request.Form["caption2"], context.Request.Form["caption3"], context.Request.Form["caption4"], context.Request.Form["caption5"], context.Request.Form["caption6"], context.Request.Form["caption7"]);
            DataSetHelper.ExecuteCommand(sSql, SDTableType.SystemTable);

        }
        else if (Mode == "Delete")
        {
            String sSql = string.Format(DELETE_MENUTABLE_SQL, context.Request.Form["menuid"]);
            DataSetHelper.ExecuteCommand(sSql, SDTableType.SystemTable);
        }
    }

    const string SELECT_LOADGROUPS_SQL = "SELECT GROUPMENUS.*, GROUPS.GROUPNAME FROM GROUPMENUS LEFT JOIN GROUPS ON GROUPS.GROUPID = GROUPMENUS.GROUPID WHERE GROUPMENUS.MENUID = '{0}'";
    private void GetGroupMenus(HttpContext context)
    {
        var clientInfo = EFClientTools.ClientUtility.ClientInfo;
        String solutionId = context.Request.Form["solutionid"];
        if (!String.IsNullOrEmpty(solutionId))
        {
            EFClientTools.ClientUtility.ClientInfo.Solution = solutionId;
        }
        var dataSet = DataSetHelper.ExecuteSql(string.Format(SELECT_LOADGROUPS_SQL, context.Request.Form["menuid"]), SDTableType.SystemTable);
        var dataTable = dataSet.Tables[0];
        context.Response.Write(JsonConvert.SerializeObject(dataTable, Newtonsoft.Json.Formatting.Indented));
    }

    private void InsertGroupMenus(HttpContext context)
    {
        var client = EFClientTools.ClientUtility.Client;
        List<object> param = new List<object>();
        param.Add(context.Request.Form["menuid"]);
        param.Add(context.Request.Form["groupids"]);
        client.SaveDataToTable(EFClientTools.ClientUtility.ClientInfo, param, "GROUPMENUS");
    }

    const string SELECT_LOADUSERS_SQL = "select USERMENUS.*, USERS.USERNAME from USERMENUS left join USERS on USERS.USERID = USERMENUS.USERID where USERMENUS.MENUID = '{0}'";
    private void GetUserMenus(HttpContext context)
    {
        var clientInfo = EFClientTools.ClientUtility.ClientInfo;
        String solutionId = context.Request.Form["solutionid"];
        if (!String.IsNullOrEmpty(solutionId))
        {
            EFClientTools.ClientUtility.ClientInfo.Solution = solutionId;
        }
        var dataSet = DataSetHelper.ExecuteSql(string.Format(SELECT_LOADUSERS_SQL, context.Request.Form["menuid"]), SDTableType.SystemTable);
        var dataTable = dataSet.Tables[0];
        context.Response.Write(JsonConvert.SerializeObject(dataTable, Newtonsoft.Json.Formatting.Indented));
    }

    private void InsertUserMenus(HttpContext context)
    {
        var client = EFClientTools.ClientUtility.Client;
        List<object> param = new List<object>();
        param.Add(context.Request.Form["menuid"]);
        param.Add(context.Request.Form["userids"]);
        client.SaveDataToTable(EFClientTools.ClientUtility.ClientInfo, param, "USERMENUS");
    }

    const string SELECT_SYS_PAGES_SQL = "SELECT * FROM SYS_WEBPAGES WHERE PageType in ('M','W') AND SolutionID = '{0}'";
    private void GetPages(HttpContext context)
    {
        var clientInfo = EFClientTools.ClientUtility.ClientInfo;
        String solutionId = context.Request.Form["solutionid"];
        if (!String.IsNullOrEmpty(solutionId))
        {
            EFClientTools.ClientUtility.ClientInfo.Solution = solutionId;
        }
        var dataSet = DataSetHelper.ExecuteSql(string.Format(SELECT_SYS_PAGES_SQL, EFClientTools.ClientUtility.ClientInfo.Solution), SDTableType.SDSystemTable);
        var dataTable = dataSet.Tables[0];
        var pages = new JArray();
        if (dataTable.Rows.Count > 0)
        {
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                var page = new JObject();
                page["PageName"] = dataTable.Rows[i]["PageName"].ToString();
                pages.Add(page);
            }

            var json = JsonConvert.SerializeObject(pages);
            context.Response.Write(json);
        }
    }

    private static T Deserialize<T>(string xml)
    {
        if (string.IsNullOrEmpty(xml))
        {
            return default(T);
        }

        System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
        System.Xml.XmlReaderSettings settings = new System.Xml.XmlReaderSettings();
        // No settings need modifying here      
        using (System.IO.StringReader textReader = new System.IO.StringReader(xml))
        {
            using (System.Xml.XmlReader xmlReader = System.Xml.XmlReader.Create(textReader, settings))
            {
                return (T)serializer.Deserialize(xmlReader);
            }
        }
    }

    private void GetGroupMenuControl(HttpContext context)
    {
        var clientInfo = EFClientTools.ClientUtility.ClientInfo;
        String groupId = context.Request.Form["groupId"];
        String menuId = context.Request.Form["menuId"];
        String sql = SQLHelper.SELECT_GROUPMENUCONTROL_SQL;
        if (EFClientTools.ClientUtility.ClientInfo.DatabaseType == "Oracle")
        {
            sql = SQLHelper.SELECT_GROUPMENUCONTROL_ORACLE;
        }
        var dataSet = DataSetHelper.ExecuteSql(new SQLCommandInfo()
        {
            CommandText = string.Format(sql, "", ""),
            Parameters = SQLHelper.CreateParameters(new string[] { "GROUPID", "MENUID" }, new object[] { groupId, menuId })
        }, SDTableType.SystemTable);
        var dataTable = dataSet.Tables[0];
        context.Response.Write(DataTableToJson(dataTable));
    }

    private void GetUserMenuControl(HttpContext context)
    {
        var clientInfo = EFClientTools.ClientUtility.ClientInfo;
        String userId = context.Request.Form["userId"];
        String menuId = context.Request.Form["menuId"];
        String sql = SQLHelper.SELECT_USERMENUCONTROL_SQL;
        if (EFClientTools.ClientUtility.ClientInfo.DatabaseType == "Oracle")
        {
            sql = SQLHelper.SELECT_USERMENUCONTROL_ORACLE;
        }
        var dataSet = DataSetHelper.ExecuteSql(new SQLCommandInfo()
        {
            CommandText = string.Format(sql, "", ""),
            Parameters = SQLHelper.CreateParameters(new string[] { "USERID", "MENUID" }, new object[] { userId, menuId })
        }, SDTableType.SystemTable);
        var dataTable = dataSet.Tables[0];
        context.Response.Write(DataTableToJson(dataTable));
    }

    private void SaveGroupMenuControl(HttpContext context)
    {
        var clientInfo = EFClientTools.ClientUtility.ClientInfo;
        String tableName = "GROUPMENUCONTROL";
        var groupId = context.Request.Form["groupId"];
        var menuId = context.Request.Form["menuId"];
        var updateRows = new List<UpdateRow>();
        var content = context.Request.Form["rows"];
        content = content.Replace("&lt;", "<").Replace("&gt;", ">");
        var rows = (JObject)JsonConvert.DeserializeObject(content);
        DataSetHelper.ExecuteCommand(new SQLCommandInfo()
        {
            CommandText = SQLHelper.CreateDeleteSql(tableName, new string[] { "GROUPID", "MENUID" }),
            Parameters = SQLHelper.CreateParameters(new string[] { "GROUPID", "MENUID" }, new object[] { groupId, menuId })
        }, SDTableType.SystemTable);
        var inserted = (JArray)rows["inserted"];
        foreach (JObject row in inserted)
        {
            updateRows.Add(JsonHelper.CreateUpdateRow(row, EFClientTools.EFServerReference.DataRowState.Added));
        }
        DataSetHelper.UpdateTable(tableName, updateRows, SDTableType.SystemTable);
    }

    private void SaveUserMenuControl(HttpContext context)
    {
        var clientInfo = EFClientTools.ClientUtility.ClientInfo;
        String tableName = "USERMENUCONTROL";
        var userId = context.Request.Form["userId"];
        var menuId = context.Request.Form["menuId"];
        var updateRows = new List<UpdateRow>();
        var content = context.Request.Form["rows"];
        content = content.Replace("&lt;", "<").Replace("&gt;", ">");
        var rows = (JObject)JsonConvert.DeserializeObject(content);
        DataSetHelper.ExecuteCommand(new SQLCommandInfo()
        {
            CommandText = SQLHelper.CreateDeleteSql(tableName, new string[] { "USERID", "MENUID" }),
            Parameters = SQLHelper.CreateParameters(new string[] { "USERID", "MENUID" }, new object[] { userId, menuId })
        }, SDTableType.SystemTable);
        var inserted = (JArray)rows["inserted"];
        foreach (JObject row in inserted)
        {
            updateRows.Add(JsonHelper.CreateUpdateRow(row, EFClientTools.EFServerReference.DataRowState.Added));
        }
        DataSetHelper.UpdateTable(tableName, updateRows, SDTableType.SystemTable);
    }

    private void GetSecurity(HttpContext context)
    {
        var clientInfo = EFClientTools.ClientUtility.ClientInfo;
        String menuId = context.Request.Form["menuId"];
        var dataSet = DataSetHelper.ExecuteSql(new SQLCommandInfo()
        {
            CommandText = SQLHelper.CreateSelectSql("MENUTABLECONTROL", null, new string[] { "MENUID" }),
            Parameters = SQLHelper.CreateParameters(new string[] { "MENUID" }, new object[] { menuId })
        }, SDTableType.SystemTable);
        var dataTable = dataSet.Tables[0];
        context.Response.Write(DataTableToJson(dataTable));
    }
}

