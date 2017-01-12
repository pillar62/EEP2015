<%@ WebHandler Language="C#" Class="SystemHandle" %>

using System;
using System.Web;
using Newtonsoft;
using Newtonsoft.Json;
using System.Data;
using EFClientTools.EFServerReference;
using System.Collections.Generic;
using System.Web.SessionState;
using System.Linq;
using Newtonsoft.Json.Linq;

public class SystemHandle : IHttpHandler, IRequiresSessionState
{
    Dictionary<string, object> returnDic = new Dictionary<string, object>();
    Dictionary<string, object> paraDic = new Dictionary<string, object>();
    Dictionary<string, object> masterKeysDic = new Dictionary<string, object>();
    Dictionary<string, object> keysDic = new Dictionary<string, object>();
    HttpContext hContext;
    public void ProcessRequest(HttpContext context)
    {
        context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
        hContext = context;
        var client = EFClientTools.ClientUtility.Client;
        if (hContext.Request.QueryString["Type"] == "Encrypt")
        {
            var param = hContext.Request.Form["param"];
            var key = Guid.NewGuid().ToString("N");

            var encryptParam = JQClientTools.JQScriptManager.EncryptParameters(param, key);
            hContext.Response.Write(string.Format("param={0}&key={1}", HttpUtility.UrlEncode(encryptParam), key));
            return;
        }

        else if (hContext.Request.QueryString["Type"] == "MENUTABLE")
        {
            String MenuName = hContext.Request.QueryString["MenuName"];
            var menus = client.FetchMenus(EFClientTools.ClientUtility.ClientInfo).OfType<EFClientTools.EFServerReference.MENUTABLE>().Where(m => m.PACKAGE + "." + m.FORM == MenuName).ToList();
            if (menus.Count > 0)
            {
                string JsonString = "";
                switch (EFClientTools.ClientUtility.ClientInfo.Locale.ToLower())
                {
                    case "zh-hk": JsonString = menus[0].CAPTION3; break;
                    case "zh-cn": JsonString = menus[0].CAPTION2; break;
                    case "zh-tw": JsonString = menus[0].CAPTION1; break;
                    default: JsonString = menus[0].CAPTION; break;
                }

                if (EFClientTools.ClientUtility.ClientInfo.Locale.ToLower().StartsWith("en"))
                {
                    JsonString = menus[0].CAPTION0;
                }

                if (String.IsNullOrEmpty(JsonString))
                {
                    JsonString = menus[0].CAPTION;
                }
                context.Response.Write(JsonString);
            }
        }
        else if (hContext.Request.QueryString["Type"] == "MENUTABLECAPTION")
        {
            String MenuName = hContext.Request.QueryString["MenuName"];
            var clientInfo = EFClientTools.ClientUtility.ClientInfo;
            var cg = clientInfo.CurrentGroup;
            clientInfo.CurrentGroup = "forCaption";
            var fetchMenus = client.FetchMenus(clientInfo).OfType<EFClientTools.EFServerReference.MENUTABLE>();
            var menus = fetchMenus.Where(m => m.PACKAGE + "." + m.FORM == MenuName).ToList();
            clientInfo.CurrentGroup = cg;
            if (menus.Count > 0)
            {
                string JsonString = "";
                switch (clientInfo.Locale.ToLower())
                {
                    case "zh-hk": JsonString = menus[0].CAPTION3; break;
                    case "zh-cn": JsonString = menus[0].CAPTION2; break;
                    case "zh-tw": JsonString = menus[0].CAPTION1; break;
                    default: JsonString = menus[0].CAPTION; break;
                }

                if (clientInfo.Locale.ToLower().StartsWith("en"))
                {
                    JsonString = menus[0].CAPTION0;
                }

                if (String.IsNullOrEmpty(JsonString))
                {
                    JsonString = menus[0].CAPTION;
                }
                context.Response.Write(JsonString);
            }
        }
        else if (hContext.Request.QueryString["Type"] == "GetMenu")
        {
            String MenuId = hContext.Request.QueryString["MENUID"];
            var menus = client.FetchMenus(EFClientTools.ClientUtility.ClientInfo).OfType<EFClientTools.EFServerReference.MENUTABLE>().Where(m => m.MENUID == MenuId).ToList();
            string JsonString = EFClientTools.EntityObjectExtensionMethods.ToEntitiesJson(menus, "MENUTABLE1");
            //string JsonString = JsonConvert.SerializeObject(menus, Formatting.None);
            context.Response.Write(JsonString);

        }
        else if (hContext.Request.QueryString["Type"] == "UserDefineLog")
        {
            var title = hContext.Request.QueryString["Title"];
            var description = hContext.Request.QueryString["Description"];
            var status = hContext.Request.QueryString["Status"];
            var logStatus = EFClientTools.EFServerReference.LogStatus.Normal;
            if (!string.IsNullOrEmpty(status))
            {
                Enum.TryParse<EFClientTools.EFServerReference.LogStatus>(status, true, out logStatus);
            }
            client.UserDefineLog(EFClientTools.ClientUtility.ClientInfo, logStatus, title, description);
        }
        else if (hContext.Request.QueryString["Type"] == "USER")
        {
            EFBase.MessageProvider provider = new EFBase.MessageProvider(hContext.Request.PhysicalApplicationPath, EFClientTools.ClientUtility.ClientInfo.Locale);
            var userString = string.Format(provider["JQWebClient/userinfo"], EFClientTools.ClientUtility.ClientInfo.UserID, EFClientTools.ClientUtility.ClientInfo.UserName);
            context.Response.Write(userString);
        }
        else if (hContext.Request.QueryString["Type"] == "ChangePassword")
        {
            var userID = hContext.Request.QueryString["UserID"];
            var oPassword = hContext.Request.QueryString["OPassword"];
            var nPassword = hContext.Request.QueryString["NPassword"];
            var parameters = new List<object>();
            parameters.Add(userID + ":" + oPassword + ":" + nPassword);
            //parameters.Add(userID);
            //parameters.Add(oPassword);
            //parameters.Add(nPassword);
            var obj2 = client.CallMethod(EFClientTools.ClientUtility.ClientInfo, "GLModule", "GetLoginFile", new List<object>() { new DateTime() });

            var PasswordPolicy = obj2.ToString();
            var max = 10;
            var min = 0;
            var CharNum = false;
            var Expiry = 0;
            try
            {
                if (!string.IsNullOrEmpty(PasswordPolicy))
                {
                    System.Text.RegularExpressions.Regex rmin = new System.Text.RegularExpressions.Regex("MinSize=\"\\d*\"");
                    if (rmin.IsMatch(PasswordPolicy))
                    {
                        var smin = rmin.Match(PasswordPolicy).Value;
                        Int32.TryParse(smin.Substring(9, smin.Length - 10), out min);
                    }
                    System.Text.RegularExpressions.Regex rmax = new System.Text.RegularExpressions.Regex("MaxSize=\"\\d*\"");
                    if (rmax.IsMatch(PasswordPolicy))
                    {
                        var smax = rmax.Match(PasswordPolicy).Value;
                        Int32.TryParse(smax.Substring(9, smax.Length - 10), out max);
                    }
                    System.Text.RegularExpressions.Regex rcn = new System.Text.RegularExpressions.Regex("CharNum=\"\\w*\"");
                    if (rcn.IsMatch(PasswordPolicy))
                    {
                        var scn = rcn.Match(PasswordPolicy).Value;
                        bool.TryParse(scn.Substring(9, scn.Length - 10), out CharNum);
                    }
                    System.Text.RegularExpressions.Regex rpwe = new System.Text.RegularExpressions.Regex("PassWrodExpiry=\"\\d*\"");
                    if (rpwe.IsMatch(PasswordPolicy))
                    {
                        var spwe = rpwe.Match(PasswordPolicy).Value;
                        Int32.TryParse(spwe.Substring(16, spwe.Length - 17), out Expiry);
                    }
                }
            }
            finally { }
            if (nPassword.Length > max || nPassword.Length < min)
            {
                EFBase.MessageProvider provider = new EFBase.MessageProvider(hContext.Request.PhysicalApplicationPath, EFClientTools.ClientUtility.ClientInfo.Locale);
                var userString = string.Format(provider["Srvtools/UGControl/PasswordLength"], min.ToString(), max.ToString());
                context.Response.Write(userString);
                return;
            }
            else if (CharNum)
            {
                int x = 0, y = 0;
                for (int i = 0; i < nPassword.Length; i++)
                {
                    if (!char.IsLetterOrDigit(nPassword, i))
                    {
                        EFBase.MessageProvider provider = new EFBase.MessageProvider(hContext.Request.PhysicalApplicationPath, EFClientTools.ClientUtility.ClientInfo.Locale);
                        var userString = string.Format(provider["Srvtools/UGControl/PasswordCharCheck"], min.ToString(), max.ToString());
                        context.Response.Write(userString);
                        return;
                    }
                    else if (char.IsLetter(nPassword, i))
                    {
                        x++;
                    }
                    else if (char.IsNumber(nPassword, i))
                    {
                        y++;
                    }
                }
                if (x == 0 || y == 0)
                {
                    EFBase.MessageProvider provider = new EFBase.MessageProvider(hContext.Request.PhysicalApplicationPath, EFClientTools.ClientUtility.ClientInfo.Locale);
                    var userString = string.Format(provider["Srvtools/UGControl/PasswordCharNum"], min.ToString(), max.ToString());
                    context.Response.Write(userString);
                    return;
                }
            }
            var obj = client.CallMethod(EFClientTools.ClientUtility.ClientInfo, "GLModule", "ChangePassword", parameters);
            if (obj.ToString() != "E")
            {
                EFClientTools.ClientUtility.ClientInfo.Password = nPassword;
            }
            context.Response.Write(obj.ToString().ToLower());
        }
        else if (hContext.Request.QueryString["Type"] == "IsLoginUSER")
        {
            String userid = hContext.Request.QueryString["UserID"];
            bool isExisted = false;
            foreach (var item in userid.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (item == EFClientTools.ClientUtility.ClientInfo.UserID) isExisted = true;
            }
            if (isExisted)
                context.Response.Write("true");
            else
                context.Response.Write("false");
        }
        else if (hContext.Request.QueryString["Type"] == "GetSolution")
        {
            var menus = client.GetSolutions(EFClientTools.ClientUtility.ClientInfo).ToList();
            string JsonString = EFClientTools.EntityObjectExtensionMethods.ToEntitiesJson(menus, "MENUITEMTYPE");
            context.Response.Write(JsonString);
        }
        else if (hContext.Request.QueryString["Type"] == "SetCurrentSolution")
        {
            context.Response.Write(EFClientTools.ClientUtility.ClientInfo.Solution);
        }
        else if (hContext.Request.QueryString["Type"] == "RefreshMenu")
        {
            String SolutionId = hContext.Request.QueryString["SolutionId"];
            if (!String.IsNullOrEmpty(SolutionId))
            {
                EFClientTools.ClientUtility.ClientInfo.Solution = SolutionId;
            }
            var menus = client.FetchMenus(EFClientTools.ClientUtility.ClientInfo).OfType<EFClientTools.EFServerReference.MENUTABLE>().Where(m => String.IsNullOrEmpty(m.PARENT)).ToList();
            string JsonString = EFClientTools.EntityObjectExtensionMethods.ToEntitiesJson(menus, "MENUTABLE1");
            //List<EFClientTools.EFServerReference.MENUTABLE> chileMenus = new List<EFClientTools.EFServerReference.MENUTABLE>();
            //String menuString = "";// "<div class=\"menu-line\"></div><div class=\"menu-item\" style=\"height: 20px;\" name=\"\" href=\"\">";
            //menuString += InitMenuItems(chileMenus, menus);
            //menuString += "</div>";
            context.Response.Write(JsonString);
        }
        else if (hContext.Request.QueryString["Type"] == "IsWorkflow")
        {
            String isFlow = "false";
            String flow = System.Configuration.ConfigurationManager.AppSettings["IsFlow"];
            if (String.IsNullOrEmpty(flow))
                isFlow = "false";
            else if (flow.ToLower() == "true")
                isFlow = "true";
            else
                isFlow = "false";
            context.Response.Write(isFlow);
        }
        else if (hContext.Request.QueryString["Type"] == "SetTheme")
        {
            String theme = System.Configuration.ConfigurationManager.AppSettings["Theme"];
            if (String.IsNullOrEmpty(theme))
                theme = "default";
            context.Response.Write(theme);
        }
        else if (hContext.Request.QueryString["Type"] == "ClientInfo")
        {
            var clientInfo = new Newtonsoft.Json.Linq.JObject();
            clientInfo["UserID"] = EFClientTools.ClientUtility.ClientInfo.UserID;
            clientInfo["UserName"] = EFClientTools.ClientUtility.ClientInfo.UserName;
            clientInfo["Database"] = EFClientTools.ClientUtility.ClientInfo.Database;
            clientInfo["Solution"] = EFClientTools.ClientUtility.ClientInfo.Solution;
            var locale = EFClientTools.ClientUtility.ClientInfo.Locale.ToLower();
            clientInfo["Locale"] = locale;

            var json = JsonConvert.SerializeObject(clientInfo);
            context.Response.Write(json);
        }
        else if (hContext.Request.QueryString["Type"] == "forClick")
        {
            String SolutionId = hContext.Request.QueryString["SolutionId"];
            String parentId = context.Request.Form["parentId"];
            if (!String.IsNullOrEmpty(SolutionId))
            {
                EFClientTools.ClientUtility.ClientInfo.Solution = SolutionId;
            }
            var clientInfo = EFClientTools.ClientUtility.ClientInfo;
            var userPara1 = clientInfo.UserPara1;
            var userPara2 = clientInfo.UserPara2;
            clientInfo.UserPara1 = parentId;
            clientInfo.UserPara2 = "forClick";
            var menus = client.FetchMenus(clientInfo).OfType<EFClientTools.EFServerReference.MENUTABLE>().ToList();
            string JsonString = EFClientTools.EntityObjectExtensionMethods.ToEntitiesJson(menus, "MENUTABLE1");
            //string JsonString = JsonConvert.SerializeObject(menus, Formatting.None);
            clientInfo.UserPara1 = userPara1;
            clientInfo.UserPara2 = userPara2;
            context.Response.Write(JsonString);
        }
        else if (hContext.Request.QueryString["Type"] == "forRoot")
        {
            String SolutionId = hContext.Request.QueryString["SolutionId"];
            if (!String.IsNullOrEmpty(SolutionId))
            {
                EFClientTools.ClientUtility.ClientInfo.Solution = SolutionId;
            }
            var clientInfo = EFClientTools.ClientUtility.ClientInfo;
            var userPara2 = clientInfo.UserPara2;
            clientInfo.UserPara2 = "forRoot";
            var menus = client.FetchMenus(clientInfo).OfType<EFClientTools.EFServerReference.MENUTABLE>().ToList();
            clientInfo.UserPara2 = userPara2;
            string JsonString = EFClientTools.EntityObjectExtensionMethods.ToEntitiesJson(menus, "MENUTABLE1");
            //string JsonString = JsonConvert.SerializeObject(menus, Formatting.None);
            context.Response.Write(JsonString);
        }
        else if (hContext.Request.QueryString["Type"] == "resetUserP")
        {
            var user = context.Request.Form["user"];
            var db = context.Request.Form["database"];
            var clientInfo = new ClientInfo()
            {
                UserID = user,
                Database = db,
                UseDataSet = true
            };
            var email = context.Request.Form["email"];
            var newPassword = EFClientTools.ClientUtility.Client.ResetUser(clientInfo, email);
            var title = "您的密碼已經重置";
            var body = string.Format("Dear {0}：<br/>您的密碼已經重置，登入帳號為：{1}，<br/>新的密碼為：{2}。"
                , user, user, newPassword);
            var mailOption = System.Configuration.ConfigurationManager.AppSettings["MailOption"];
            Dictionary<string, string> mailOptions = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(mailOption))
            {
                var o = mailOption.Split(',');
                foreach (var option in o)
                {
                    var keyValue = option.Split('=');
                    if (keyValue.Length == 2)
                    {
                        mailOptions.Add(keyValue[0], keyValue[1]);
                    }
                }
            }
            var smtpclient = new System.Net.Mail.SmtpClient(mailOptions["Smtp"]);
            smtpclient.UseDefaultCredentials = true;
            smtpclient.Credentials = new System.Net.NetworkCredential(mailOptions["From"], mailOptions["Password"]);
            if (mailOptions.ContainsKey("EnableSsl"))
            {
                smtpclient.EnableSsl = bool.Parse(mailOptions["EnableSsl"]);
            }
            if (mailOptions.ContainsKey("Port"))
            {
                smtpclient.Port = int.Parse(mailOptions["Port"]);
            }
            var message = new System.Net.Mail.MailMessage();
            message.SubjectEncoding = System.Text.Encoding.UTF8;
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.From = new System.Net.Mail.MailAddress(mailOptions["From"], "", System.Text.Encoding.UTF8);
            message.To.Add(new System.Net.Mail.MailAddress(email, email, System.Text.Encoding.UTF8));
            message.IsBodyHtml = true;
            message.Subject = title;
            message.Body = body;
            //System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(Send), new object[] { client, message });
            try
            {
                smtpclient.Send(message);
            }
            catch (Exception ex) { }
        }
        else if (hContext.Request.QueryString["Type"] == "forMyFavor")
        {
            String SolutionId = hContext.Request.QueryString["SolutionId"];
            String groupName = hContext.Request.Form["GROUPNAME"];
            if (!String.IsNullOrEmpty(SolutionId))
            {
                EFClientTools.ClientUtility.ClientInfo.Solution = SolutionId;
            }
            var clientInfo = EFClientTools.ClientUtility.ClientInfo;
            var userPara2 = clientInfo.UserPara2;
            clientInfo.UserPara2 = "forMyFavor";
            var menusFavor = client.FetchMenus(clientInfo).OfType<EFClientTools.EFServerReference.MENUFAVOR>().ToList();
            if (!String.IsNullOrEmpty(groupName))
                menusFavor = menusFavor.Where(m => m.GROUPNAME == groupName).ToList();
            string JsonString = EFClientTools.EntityObjectExtensionMethods.ToEntitiesJson(menusFavor, "");
            //string JsonString = JsonConvert.SerializeObject(menus, Formatting.None);
            clientInfo.UserPara2 = userPara2;
            context.Response.Write(JsonString);
        }
        else if (hContext.Request.QueryString["Type"] == "UpdateMyFavor")
        {
            UpdateMyFavor(hContext);   
        }
        else
        {
            String SolutionId = hContext.Request.QueryString["SolutionId"];
            if (!String.IsNullOrEmpty(SolutionId))
            {
                EFClientTools.ClientUtility.ClientInfo.Solution = SolutionId;
            }
            var menus = client.FetchMenus(EFClientTools.ClientUtility.ClientInfo).OfType<EFClientTools.EFServerReference.MENUTABLE>().Where(m => String.IsNullOrEmpty(m.PARENT)).ToList();
            string JsonString = EFClientTools.EntityObjectExtensionMethods.ToEntitiesJson(menus, "MENUTABLE1");
            //string JsonString = JsonConvert.SerializeObject(menus, Formatting.None);
            context.Response.Write(JsonString);
        }
    }

    private String InitMenuItems(List<EFClientTools.EFServerReference.MENUTABLE> menus, List<EFClientTools.EFServerReference.MENUTABLE> menuTables)
    {
        System.Text.StringBuilder returnValue = new System.Text.StringBuilder();// new StringBuilder(menus);
        foreach (var menuTable in menuTables)
        {
            if (menuTable.MENUTABLE1 == null || menuTable.MENUTABLE1.Count == 0)
            {
                menus.Add(menuTable);
                var icon = string.IsNullOrEmpty(menuTable.IMAGEURL) ? string.Empty : string.Format("data-options=\\\"iconCls:'menuicon-{0}'\\\"", menuTable.IMAGEURL.Replace(".", ""));
                // var icon = string.IsNullOrEmpty(menuTable.IMAGEURL) ? string.Empty : string.Format("data-options=\\\"iconCls:'menuicon-368304png'\\\"", menuTable.IMAGEURL.Replace(".", ""));

                returnValue.Append("<div id='" + menuTable.MENUID + "' " + icon + ">" + menuTable.CAPTION + "</div>");
                //returnValue.Append("<div id='" + menuTable.MENUID + "' " + icon + " class=\"menu-text\">" + menuTable.CAPTION + "</div>");
            }
            else
            {
                returnValue.Append("<div>");
                //returnValue.Append("<div class=\"menu-text\">");
                returnValue.AppendFormat("<span>{0}</span>", menuTable.CAPTION);
                returnValue.Append("<div>");
                returnValue.Append(InitMenuItems(menus, menuTable.MENUTABLE1));
                returnValue.Append("</div>");
                returnValue.Append("</div>");
            }
        }

        return returnValue.ToString();
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

    private void UpdateMyFavor(HttpContext context)
    {
        var type = context.Request.Form["type"];
        String[] menuids = context.Request.Form["MENUID[]"].Split(new char[]{','},  StringSplitOptions.RemoveEmptyEntries);
        String[] captions = context.Request.Form["CAPTION[]"].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        String groupname = context.Request.Form["GROUPNAME"];

        String sql = SQLHelper.DELETE_MENUFAVOR_SQL;
        if (EFClientTools.ClientUtility.ClientInfo.DatabaseType == "Oracle")
        {
            sql = SQLHelper.DELETE_MENUFAVOR_ORACLE;
        }
        var dataSet = DataSetHelper.ExecuteSql(new SQLCommandInfo()
        {
            CommandText = string.Format(sql, "", ""),
            Parameters = SQLHelper.CreateParameters(new string[] { "USERID", "GROUPNAME" }, new object[] { EFClientTools.ClientUtility.ClientInfo.UserID, groupname })
        }, SDTableType.SystemTable);

        for (var i = 0; i < menuids.Length; i++ )
        {
            sql = SQLHelper.INSERT_MENUFAVOR_SQL;
            if (EFClientTools.ClientUtility.ClientInfo.DatabaseType == "Oracle")
            {
                sql = SQLHelper.INSERT_MENUFAVOR_ORACLE;
            }
            dataSet = DataSetHelper.ExecuteSql(new SQLCommandInfo()
            {
                CommandText = string.Format(sql, "", ""),
                Parameters = SQLHelper.CreateParameters(new string[] { "MENUID", "CAPTION", "USERID", "ITEMTYPE", "GROUPNAME" }, new object[] { menuids[i], captions[i], EFClientTools.ClientUtility.ClientInfo.UserID, EFClientTools.ClientUtility.ClientInfo.Solution, groupname })
            }, SDTableType.SystemTable);
        }
    }
}