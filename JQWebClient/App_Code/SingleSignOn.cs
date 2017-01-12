using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using EFClientTools.EFServerReference;

/// <summary>
/// Summary description for SingleSignOn
/// </summary>
[WebService(Namespace = "http://infolight.com/")]
[ScriptService]    
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class SingleSignOn : System.Web.Services.WebService{
    public SingleSignOn () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod(true)]
    public string LogOn(string userId, string password, string dataBase, string solution)
    {
        EFServiceClient client = EFClientTools.ClientUtility.Client;
        EFClientTools.ClientUtility.ServerIPAddress = client.GetServerIPAddress();
        var ipAddress = HttpContext.Current.Request.UserHostAddress;
        var clientInfo = new ClientInfo()
        {
            UserID = userId,
            Password = password,
            Database = dataBase,
            Solution = solution,
            IPAddress = ipAddress,
            Locale = "en-us",
            UseDataSet = true
        };
        if(clientInfo.UserID.Contains("'"))
        {
            return string.Empty;
        }
        var result = client.LogOn(clientInfo);
        
        client = EFClientTools.ClientUtility.Client;
        if (result.LogonResult == LogonResult.Logoned)
        {
            return result.SecurityKey;
        }
        else
        {
            return string.Empty;
        }
    }

    [WebMethod(true)]
    public void LogOut(string userId)
    {
        EFServiceClient client = EFClientTools.ClientUtility.Client;
        var ipAddress = HttpContext.Current.Request.UserHostAddress;
        var clientInfo = new ClientInfo()
        {
            UserID = userId,
           
            IPAddress = ipAddress,
            Locale = "en-us",
            UseDataSet = true
        };
        client.LogOff(clientInfo);
    }

    [WebMethod(true)]
    public List<string> GetDatabases()
    {
        EFServiceClient client = EFClientTools.ClientUtility.Client;
        return client.GetDatabases(null);
    }

    [WebMethod(true)]
    public List<string> GetSolutions()
    {
        EFServiceClient client = EFClientTools.ClientUtility.Client;
        return client.GetSolutions(new ClientInfo()).Select(c => c.ID).ToList();
    }
}
