<%@ WebHandler Language="C#" Class="JQMobileHandler" %>

using System;
using System.Web;
using System.Linq;
using EFClientTools;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

public class JQMobileHandler : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    public void ProcessRequest(HttpContext context)
    {
        context.Response.Cache.SetCacheability(HttpCacheability.NoCache);

        var mode = context.Request.Form["mode"];

        if (mode == "logon")
        {
            Logon(context);
        }
        else if (mode == "checkDevice")
        {
            CheckDevice(context);
        }
        else if (mode == "registerDevice")
        {
            RegisterDevice(context);
        }
        else if (mode == "getMessages")
        {
            GetMessages(context);
        }
        else if (mode == "getMessageCount")
        {
            GetMessageCount(context);
        }
        else if (mode == "readMessage")
        {
            ReadMessage(context);
        }
        else if (mode == "deleteMessages")
        {
            DeleteMessages(context);
        }
        else if (mode == "logout")
        {
            Logout(context);
        }
        else if (mode == "getDatabases")
        {
            GetDatabases(context);
        }
        else if (mode == "getSolutions")
        {
            GetSolutions(context);
        }
        else if (mode == "getMenus")
        {
            GetMenus(context);
        }
        else if (mode == "getMenusForIonic")
        {
            GetMenusForIonic(context);
        }
    }

    private object GetJObjectValue(Newtonsoft.Json.Linq.JObject obj, string propertyName)
    {
        if (obj[propertyName] != null && obj[propertyName] is Newtonsoft.Json.Linq.JValue)
        {
            return (obj[propertyName] as Newtonsoft.Json.Linq.JValue).Value;
        }
        return null;
    }

    private void Logon(HttpContext context)
    {
        var data = (Newtonsoft.Json.Linq.JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(context.Request.Form["data"]);
        var ipAddress = context.Request.UserHostAddress;
        var locale = context.Request.UserLanguages.Length > 0 ? context.Request.UserLanguages[0] : "en-us";

        var userID = (string)GetJObjectValue(data, "userID");
        var deviceID = (string)GetJObjectValue(data, "deviceID");

        var clientInfo = new EFClientTools.EFServerReference.ClientInfo()
        {
            UserID = userID,
            Password = (string)GetJObjectValue(data, "password"),
            Database = (string)GetJObjectValue(data, "database"),
            Solution = (string)GetJObjectValue(data, "solution"),
            IPAddress = ipAddress,
            Locale = locale,
            UseDataSet = true
        };
        EFClientTools.EFServerReference.EFServiceClient client = new EFClientTools.EFServerReference.EFServiceClient();
        var result = new EFClientTools.EFServerReference.ClientInfo();
        if (clientInfo.UserID.Contains("'"))
        {
            result = new EFClientTools.EFServerReference.ClientInfo() { LogonResult = EFClientTools.EFServerReference.LogonResult.UserNotFound };
        }
        else
        {

            result = client.LogOn(clientInfo);
        }
        if (result.LogonResult == EFClientTools.EFServerReference.LogonResult.Logoned)
        {
            context.Session["ClientInfo"] = result;
            if (!string.IsNullOrEmpty(deviceID))
            {
                client.LogOnDevice(userID, deviceID, false, null);
            }
        }
        else
        {
            string messageKey = string.Empty;
            switch (result.LogonResult)
            {
                case EFClientTools.EFServerReference.LogonResult.UserNotFound: messageKey = "EEPWebNetClient/WinSysMsg/msg_UserNotFound"; break;
                case EFClientTools.EFServerReference.LogonResult.UserDisabled: messageKey = "EEPWebNetClient/WinSysMsg/msg_UserDisabled"; break;
                case EFClientTools.EFServerReference.LogonResult.PasswordError: messageKey = "EEPWebNetClient/WinSysMsg/msg_UserOrPasswordError"; break;
                default: messageKey = "EEPWebNetClient/WinSysMsg/msg_UserOrPasswordError"; break;
            }
            EFBase.MessageProvider provider = new EFBase.MessageProvider(context.Request.PhysicalApplicationPath, locale);
            throw new Exception(provider[messageKey]);
        }
    }

    private void CheckDevice(HttpContext context)
    {
        var userID = context.Request.Form["userID"];
        var deviceID = context.Request.Form["deviceID"];
        var database = context.Request.Form["database"];
        var solution = context.Request.Form["solution"];
        var developerID = context.Request.Form["developerID"];
        var locale = context.Request.UserLanguages.Length > 0 ? context.Request.UserLanguages[0] : "en-us";
        EFClientTools.EFServerReference.EFServiceClient client = new EFClientTools.EFServerReference.EFServiceClient();
        var r = client.LogOnDevice(userID, deviceID + ';' + database, true, developerID);
        if (r.LogonResult == EFClientTools.EFServerReference.LogonResult.Logoned)
        {
            //var result = new EFClientTools.EFServerReference.ClientInfo()
            //{
            //    UserID = userID,
            //    Database = database,
            //    Solution = solution,
            //    LogonResult = EFClientTools.EFServerReference.LogonResult.Logoned
            //    , Groups = new System.Collections.Generic.List<EFClientTools.EFServerReference.GroupInfo>(), IPAddress= context.Request.UserHostAddress, UseDataSet = true, Locale = locale};
            r.IPAddress = context.Request.UserHostAddress;
            r.UseDataSet = true;
            r.Locale = locale;
            r.Solution = solution;
            context.Session["ClientInfo"] = r;
            context.Response.Write("");
        }
        else
        {
            context.Response.Write(r.LogonResult);
        }
    }


    private void RegisterDevice(HttpContext context)
    {
        var userID = context.Request.Form["userID"];
        var deviceID = context.Request.Form["deviceID"];
        var regID = context.Request.Form["regID"];
        var tokenID = context.Request.Form["tokenID"];
        var developerID = context.Request.Form["developerID"];
        EFClientTools.EFServerReference.EFServiceClient client = new EFClientTools.EFServerReference.EFServiceClient();
        client.RegisterDevice(userID, deviceID, regID, tokenID, null);
    }

    private static T Deserialize<T>(string xml)
    {
        if (string.IsNullOrEmpty(xml))
        {
            return default(T);
        }

        XmlSerializer serializer = new XmlSerializer(typeof(T));
        XmlReaderSettings settings = new XmlReaderSettings();
        // No settings need modifying here      
        using (StringReader textReader = new StringReader(xml))
        {
            using (XmlReader xmlReader = XmlReader.Create(textReader, settings))
            {
                return (T)serializer.Deserialize(xmlReader);
            }
        }
    }

    private void GetMessages(HttpContext context)
    {
        var messages = (string)EFClientTools.ClientUtility.Client.GetMessages(EFClientTools.ClientUtility.ClientInfo);
        var dataSet = Deserialize<System.Data.DataSet>(messages);
        var json = Newtonsoft.Json.JsonConvert.SerializeObject(dataSet.Tables[0]);
        context.Response.ContentType = "text/plain";
        context.Response.Write(json);
    }

    private void GetMessageCount(HttpContext context)
    {
        var messages = (string)EFClientTools.ClientUtility.Client.GetMessages(EFClientTools.ClientUtility.ClientInfo);
        var dataSet = Deserialize<System.Data.DataSet>(messages);
        var count = 0;
        for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
        {
            if (dataSet.Tables[0].Rows[i]["STATUS"].ToString() == "N")
            {
                count++;
            }
        }
        context.Response.ContentType = "text/plain";
        context.Response.Write(count.ToString());
    }

    private void ReadMessage(HttpContext context)
    {
        var dateTime = context.Request.Form["datetime"];
        EFClientTools.ClientUtility.Client.ReadMessage(EFClientTools.ClientUtility.ClientInfo, dateTime);
        context.Response.ContentType = "text/plain";
        context.Response.Write(string.Empty);
    }

    private void DeleteMessages(HttpContext context)
    {
        var dateTimes = context.Request.Form["datetimes"];
        var list = new System.Collections.Generic.List<string>(dateTimes.Split(';'));
        EFClientTools.ClientUtility.Client.DeleteMessage(EFClientTools.ClientUtility.ClientInfo, list);
        context.Response.ContentType = "text/plain";
        context.Response.Write(string.Empty);
    }


    private void Logout(HttpContext context)
    {
        EFClientTools.ClientUtility.Client.LogOff(EFClientTools.ClientUtility.ClientInfo);
        context.Session.Clear();
    }

    private void GetDatabases(HttpContext context)
    {
        EFClientTools.EFServerReference.EFServiceClient client = new EFClientTools.EFServerReference.EFServiceClient();
        var databases = client.GetDatabases(string.Empty);
        var json = Newtonsoft.Json.JsonConvert.SerializeObject(databases);
        context.Response.ContentType = "text/plain";
        context.Response.Write(json);
    }

    private void GetSolutions(HttpContext context)
    {
        EFClientTools.EFServerReference.EFServiceClient client = new EFClientTools.EFServerReference.EFServiceClient();
        var solutions = client.GetSolutions(new EFClientTools.EFServerReference.ClientInfo() { UseDataSet = true });
        var json = solutions.ToEntitiesJson(string.Empty);
        context.Response.ContentType = "text/plain";
        context.Response.Write(json);

    }

    private void GetMenus(HttpContext context)
    {
        var menus = EFClientTools.ClientUtility.Client.FetchMenus(EFClientTools.ClientUtility.ClientInfo);
        var mm = menus.Cast<EFClientTools.EFServerReference.MENUTABLE>().Where(m => m.MODULETYPE == "M").ToList();
        var json = mm.ToEntitiesJson(string.Empty);
        context.Response.ContentType = "text/plain";
        context.Response.Write(json);
    }

    private void GetMenusForIonic(HttpContext context)
    {
        var menus = EFClientTools.ClientUtility.Client.FetchMenus(EFClientTools.ClientUtility.ClientInfo);
        var mm = menus.Cast<EFClientTools.EFServerReference.MENUTABLE>().Where(m => m.MODULETYPE == "M" && String.IsNullOrEmpty(m.PARENT)).ToList();
        var json = EFClientTools.EntityObjectExtensionMethods.ToEntitiesJson(mm, "MENUTABLE1");
        context.Response.ContentType = "text/plain";
        context.Response.Write(json);
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}