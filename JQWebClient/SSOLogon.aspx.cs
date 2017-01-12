using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EFClientTools.EFServerReference;
using System.Configuration;
using System.Text;

public partial class SSOLogon : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        var userid = "";
        var domain = "";
        if (!string.IsNullOrEmpty(Request.LogonUserIdentity.Name) && Request.LogonUserIdentity.Name.IndexOf("\\") > 0)
        {
            domain = Request.LogonUserIdentity.Name.Remove(Request.LogonUserIdentity.Name.IndexOf("\\"));
            userid = Request.LogonUserIdentity.Name.Substring(Request.LogonUserIdentity.Name.IndexOf("\\") + 1);
        }


        if (string.IsNullOrEmpty(userid))
        {
            Response.Write("UserID is empty.");
            return;
        }
       
        if (string.IsNullOrEmpty(domain))
        {
            Response.Write("Domain is empty.");
            return;
        }
        var database = Request.QueryString["Database"];
        if (string.IsNullOrEmpty(database))
        {
            Response.Write("Database is empty.");
            return;
        }
        var solution = Request.QueryString["Solution"];
        if(string.IsNullOrEmpty(solution))
        {
            Response.Write("Solution is empty.");
            return;
        }

        EFServiceClient client = EFClientTools.ClientUtility.Client;
        var locale = Request.UserLanguages.Length > 0 ? Request.UserLanguages[0] : "en-us";
        var ipAddress = Request.UserHostAddress;
        EFClientTools.ClientUtility.ServerIPAddress = client.GetServerIPAddress();
        var clientInfo = new ClientInfo()
        {
            UserID = userid,
            Password = "",
            Database = database,
            Solution = solution,
            IPAddress = ipAddress,
            Locale = locale,
            UseDataSet = true,
            SecurityKey = EncryptValue(domain)
        };
        client = EFClientTools.ClientUtility.Client;
        var result = client.LogOn(clientInfo);
        if (result.LogonResult == LogonResult.Logoned)
        {
            result.Locale = Request.UserLanguages.FirstOrDefault();
            Session["ClientInfo"] = result;
            String flow = ConfigurationManager.AppSettings["IsFlow"];
            if (String.IsNullOrEmpty(flow))
                this.Response.Redirect("MainPage.aspx");
            else if (flow.ToLower() == "true")
            {
                this.Response.Redirect("MainPage_Flow.aspx");
                this.Page.Session["IsFlow"] = flow;
            }
            else
            {
                this.Response.Redirect("MainPage.aspx");
            }
        }
        else
        {
            string messageKey = string.Empty;
            switch (result.LogonResult)
            {
                case LogonResult.UserNotFound: messageKey = "EEPWebNetClient/WinSysMsg/msg_UserNotFound"; break;
                case LogonResult.UserDisabled: messageKey = "EEPWebNetClient/WinSysMsg/msg_UserDisabled"; break;
                case LogonResult.PasswordError: messageKey = "EEPWebNetClient/WinSysMsg/msg_UserOrPasswordError"; break;
                default: messageKey = "EEPWebNetClient/WinSysMsg/msg_UserOrPasswordError"; break;
            }
            EFBase.MessageProvider provider = new EFBase.MessageProvider(this.Request.PhysicalApplicationPath, locale);
            Response.Write(provider[messageKey]);
        }
    }

    private string EncryptValue(string value)
    {
        var encryptBuilder = new StringBuilder();
        foreach (var c in value)
        {
            encryptBuilder.Append(((int)c).ToString("X"));
        }
        return encryptBuilder.ToString();
    }
}