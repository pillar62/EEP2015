using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EFClientTools.EFServerReference;

/// <summary>
/// Summary description for JqHttpHandler
/// </summary>
public class JqHttpHandler
{
    public JqHttpHandler()
    {

    }

    public static bool ProcessRequest(HttpContext context)
    {
        if (context.Request.Form["mode"] != null)
        {
            var page = context.CurrentHandler;
            var mode = context.Request.Form["mode"];
            if (mode == "default")
            {
                EFClientTools.ClientUtility.ClientInfo.cErrorCode = 1230;

                var methodNames = context.Request.Form["method"];
                Newtonsoft.Json.Linq.JObject result = new Newtonsoft.Json.Linq.JObject();
                Newtonsoft.Json.Linq.JObject obj = (Newtonsoft.Json.Linq.JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(methodNames);
                foreach (var item in obj)
                {
                    var key = item.Key;
                    var methodName = (string)(item.Value as Newtonsoft.Json.Linq.JValue).Value;
                    if (methodName.StartsWith("_"))
                    {
                        var defaultValue = GetSystemVariable(context, methodName);
                        result.Add(key, new Newtonsoft.Json.Linq.JValue(defaultValue));
                    }
                    else
                    {
                        var defaultValue = (string)page.GetType().GetMethod(methodName).Invoke(page, null);
                        result.Add(key, new Newtonsoft.Json.Linq.JValue(defaultValue));
                    }
                }
                context.Response.ContentType = "text/plain";
                context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(result));
                EFClientTools.ClientUtility.ClientInfo.cErrorCode = 0;
                return true;
            }
            else if (mode == "validate")
            {
                EFClientTools.ClientUtility.ClientInfo.cErrorCode = 1240;

                var methodName = context.Request.Form["method"];
                var value = context.Request.Form["value"];
                var validateValue = page.GetType().GetMethod(methodName).Invoke(page, new object[] { value });
                context.Response.ContentType = "text/plain";
                context.Response.Write(validateValue.ToString());
                EFClientTools.ClientUtility.ClientInfo.cErrorCode = 0;
                return true;
            }
            else if (mode == "language")
            {
                context.Response.ContentType = "text/plain";
                context.Response.Write(EFClientTools.ClientUtility.ClientInfo.Locale.ToString().ToLower());
                return true;
            }
            else if (mode == "message")
            {
                var locale = string.Empty;
                if (context.Session["ClientInfo"] == null)
                {
                    locale = context.Request.UserLanguages[0];
                }
                else
                {
                    locale = EFClientTools.ClientUtility.ClientInfo.Locale;
                    EFClientTools.ClientUtility.ClientInfo.cErrorCode = 1250;
                }

                EFBase.MessageProvider provider = new EFBase.MessageProvider(context.Request.PhysicalApplicationPath, locale);
                var messageKeys = context.Request.Form["keys"];
                Newtonsoft.Json.Linq.JObject result = new Newtonsoft.Json.Linq.JObject();
                Newtonsoft.Json.Linq.JArray obj = (Newtonsoft.Json.Linq.JArray)Newtonsoft.Json.JsonConvert.DeserializeObject(messageKeys);
                foreach (string item in obj)
                {
                    var messageKey = item;
                    result.Add(messageKey, provider[messageKey]);
                }
                context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(result));
                if (context.Session["ClientInfo"] != null)
                    EFClientTools.ClientUtility.ClientInfo.cErrorCode = 0;
                return true;
            }
            else if (mode == "clientinfo")
            {
                var name = context.Request.Form["key"];
                context.Response.ContentType = "text/plain";
                context.Response.Write(GetClientInfo(context, name));
                return true;
            }
            else if (mode == "databasetype")
            {
                context.Response.ContentType = "text/plain";
                context.Response.Write(EFClientTools.ClientUtility.ClientInfo.DatabaseType.ToString().ToLower());
                return true;
            }
            else if (mode == "sendMail")
            {
                EFClientTools.ClientUtility.ClientInfo.cErrorCode = 1260;
                Dictionary<string, string> mailOptions = new Dictionary<string, string>();
                var options = context.Request.Form["options"];
                Newtonsoft.Json.Linq.JObject obj = (Newtonsoft.Json.Linq.JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(options);
                var BCC = obj["bCC"].ToString();
                var CC = obj["cC"].ToString();
                var To = obj["to"].ToString();
                var Subject = obj["subject"].ToString();
                var Body = obj["body"].ToString();
                var IsBodyHtml = obj["isBodyHtml"].ToString();
                var Password = "";
                var Host = "";
                var Port = "";
                var EnableSsl = obj["enableSsl"].ToString();
                var Encoding = obj["encoding"].ToString();
                var From = "";
                var mailOption = System.Configuration.ConfigurationManager.AppSettings["MailOption"];

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
                if (mailOptions.ContainsKey("Smtp"))
                {
                    Host = mailOptions["Smtp"];
                }
                if (mailOptions.ContainsKey("Port"))
                {
                    Port = mailOptions["Port"];
                }
                if (mailOptions.ContainsKey("From"))
                {
                    From = mailOptions["From"];
                }
                if (mailOptions.ContainsKey("Password"))
                {
                    Password = mailOptions["Password"];
                }
                if (mailOptions.ContainsKey("EnableSsl"))
                {
                    EnableSsl = mailOptions["EnableSsl"];
                }
                try
                {
                    var smtpclient = new System.Net.Mail.SmtpClient(mailOptions["Smtp"]);
                    if (Port != "") smtpclient.Port = int.Parse(Port);
                    if (EnableSsl != "") smtpclient.EnableSsl = bool.Parse(EnableSsl);
                    System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
                    message.From = new System.Net.Mail.MailAddress(From);
                    AddMailAddress(message.To, To);
                    AddMailAddress(message.Bcc, BCC);
                    AddMailAddress(message.CC, CC);
                    message.SubjectEncoding = System.Text.Encoding.GetEncoding(Encoding);
                    message.BodyEncoding = System.Text.Encoding.GetEncoding(Encoding);
                    message.Subject = Subject;
                    message.Body = context.Server.HtmlDecode(Body);
                    message.IsBodyHtml = bool.Parse(IsBodyHtml);
                    smtpclient.EnableSsl = bool.Parse(EnableSsl);
                    smtpclient.UseDefaultCredentials = false;
                    smtpclient.Credentials = new System.Net.NetworkCredential(From, Password);
                    smtpclient.Send(message);
                    context.Response.ContentType = "text/plain";
                    context.Response.Write("o");
                }
                catch (Exception Exception)
                {
                    context.Response.ContentType = "text/plain";
                    context.Response.Write("e" + ":" + Exception.InnerException != null ? Exception.InnerException.Message : Exception.Message);
                }
                EFClientTools.ClientUtility.ClientInfo.cErrorCode = 0;
                return true;
            }
        }

        return false;
    }

    private static string GetClientInfo(HttpContext context, string name)
    {
        var value = string.Empty;
        var property = typeof(ClientInfo).GetProperties().FirstOrDefault(c => string.Compare(c.Name, name, true) == 0);
        if (property != null)
        {
            var propertyValue = property.GetValue(EFClientTools.ClientUtility.ClientInfo, null);
            if (propertyValue != null)
            {
                value = propertyValue.ToString();
            }
        }
        else
        {
            value = GetSystemVariable(context, name);
        }
        return value;
    }

    private static string GetSystemVariable(HttpContext context, string name)
    {
        var ClientInfo = (ClientInfo)context.Session["ClientInfo"];
        if (ClientInfo == null)
        {
            throw new Exception("Timeout, relogon please");
        }
        var strval = string.Empty;
        switch (name.ToLower())
        {
            case "_usercode": strval = ClientInfo.UserID; break;
            case "_username":
                {
                    strval = ClientInfo.UserName;
                    break;
                }
            case "_groupid": strval = string.Join(";", ClientInfo.Groups.Where(c => c.Type == GroupType.Normal).Select(c => c.ID).ToArray()); break;
            case "_groupname": strval = string.Join(";", ClientInfo.Groups.Select(c => c.Name).ToArray()); break;
            case "_solution": strval = ClientInfo.Solution; break;
            case "_database": strval = ClientInfo.Database; break;
            case "_sitecode": strval = ClientInfo.Site; break;
            case "_ipaddress": strval = ClientInfo.IPAddress; break;
            case "_language": strval = ClientInfo.Locale; break;
            case "_today": strval = DateTimeToString(DateTime.Now); break;
            case "_sysdate": strval = DateTimeToString(DateTime.Now); break;
            case "_servertoday":
                break;
            case "_firstday":
                {
                    int day = DateTime.Now.Day;
                    DateTime retday = DateTime.Now.AddDays(1 - day);
                    strval = DateTimeToString(retday);
                    break;
                }
            case "_lastday":
                {
                    int day = DateTime.Now.Day;
                    DateTime retday = DateTime.Now.AddDays(1 - day);
                    retday = retday.AddMonths(1);
                    retday = retday.AddDays(-1);
                    strval = DateTimeToString(retday);
                    break;
                }
            case "_firstdaylm":
                {
                    int day = DateTime.Now.Day;
                    DateTime retday = DateTime.Now.AddDays(1 - day);
                    retday = retday.AddMonths(-1);
                    strval = DateTimeToString(retday);
                    break;
                }
            case "_lastdaylm":
                {
                    int day = DateTime.Now.Day;
                    DateTime retday = DateTime.Now.AddDays(-day);
                    strval = DateTimeToString(retday);
                    break;
                }
            case "_firstdayty":
                {
                    int year = DateTime.Now.Year;
                    DateTime retday = new DateTime(year, 1, 1);
                    strval = DateTimeToString(retday);
                    break;
                }
            case "_lastdayty":
                {
                    int year = DateTime.Now.Year;
                    DateTime retday = new DateTime(year, 12, 31);
                    strval = DateTimeToString(retday);
                    break;
                }
            case "_firstdayly":
                {
                    int year = DateTime.Now.Year - 1;
                    DateTime retday = new DateTime(year, 1, 1);
                    strval = DateTimeToString(retday);
                    break;
                }
            case "_lastdayly":
                {
                    int year = DateTime.Now.Year - 1;
                    DateTime retday = new DateTime(year, 12, 31);
                    strval = DateTimeToString(retday);
                    break;
                }
        }
        return strval;
    }

    private static string DateTimeToString(DateTime dateTime)
    {
        return dateTime.ToString(JQUtility.DateTimeFormat);
    }
    private static void AddMailAddress(System.Net.Mail.MailAddressCollection collection, string address)
    {
        if (!string.IsNullOrEmpty(address))
        {
            string[] straddresses = address.Split(';');
            foreach (string str in straddresses)
            {
                collection.Add(str);
            }
        }
    }

}