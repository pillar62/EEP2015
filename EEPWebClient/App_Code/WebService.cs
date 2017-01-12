using System;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using Srvtools;
using System.Runtime.Remoting;
using Microsoft.Win32;
using System.Web.Script.Services;


/// <summary>
/// WebService 的摘要说明
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[ScriptService]           
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class WebService : System.Web.Services.WebService
{

    public WebService()
    {
        //如果使用设计的组件，请取消注释以下行 
        //InitializeComponent(); 

        CliUtils.LoadLoginServiceConfig(string.Format("{0}\\EEPWebClient.exe.config", EEPRegistry.WebClient));
        string message = "";
        if (CliUtils.Register(ref message) == false)
        {
            throw new Exception(message);
        }
        //    bool needReg = false;
        //    if (System.Runtime.Remoting.Channels.ChannelServices.RegisteredChannels.Length == 0)
        //    {
        //        needReg = true;
        //        RemotingConfiguration.Configure(strPath + "EEPWebClient.exe.config", true);
        //    }

        //    LoginService loginService = new LoginService(); // Remoting object
        //BeginObtainService:
        //    string serverIP = "";
        //    try
        //    {
        //        string[] strrtn = loginService.GetServerIP();
        //        serverIP = strrtn[0];
        //        CliUtils.fRemotePort = int.Parse(strrtn[2]);
        //    }
        //    catch (Exception err)
        //    {
        //        throw err;
        //    }

        //    try
        //    {
        //        EEPRemoteModule module = Activator.GetObject(typeof(EEPRemoteModule),
        //            string.Format("http://{0}:{1}/InfoRemoteModule.rem", serverIP, CliUtils.fRemotePort)) as EEPRemoteModule;
        //        module.ToString();
        //    }
        //    catch
        //    {
        //        loginService.DeRegisterRemoteServer(serverIP, CliUtils.fRemotePort.ToString());
        //        goto BeginObtainService;
        //    }

        //    WellKnownClientTypeEntry clientEntry = new WellKnownClientTypeEntry(typeof(EEPRemoteModule),
        //        string.Format("http://{0}:{1}/InfoRemoteModule.rem", serverIP, CliUtils.fRemotePort));
        //    if (needReg)
        //    {
        //        RemotingConfiguration.RegisterWellKnownClientType(clientEntry);
        //    }
    }

    private object[] CreateClientInfo(string user, string password, string loginDB, string projectName, SYS_LANGUAGE language)
    {
        return new object[] { language, user, loginDB
                , "", "", ""
                , projectName, "", ""
                , "", "", ""
                , "", "", "", "0", password };
    }

    private object[] CallMethod(object[] clientInfo, string moduleName, string methodName, object[] objParams)
    {
        return CliUtils.RemoteObject.CallMethod(new object[] { clientInfo }, moduleName, methodName, objParams);
    }

    [WebMethod(EnableSession = true)]
    public string SingleSignOn(string userId, string password, string dataBase, string solution)
    {
        try
        {
            //CliUtils.fClientSystem = "WebService";
            //CliUtils.fLoginUser = userId;
            //CliUtils.fLoginPassword = password;
            //CliUtils.fLoginDB = dataBase;
            //CliUtils.fCurrentProject = solution;
            object[] clientInfo = CreateClientInfo(userId, password, dataBase, solution, SYS_LANGUAGE.ENG);
            string sParam = userId + ':' + password + ':' + dataBase + ':' + "1";
            object[] myRet = CallMethod(clientInfo, "GLModule", "CheckUser", new object[] { (object)sParam });

            LoginResult result = (LoginResult)myRet[1];
            if (result == LoginResult.Success)
            {
                string userName = myRet[2].ToString();
                object[] myRet1 = CallMethod(clientInfo, "GLModule", "GetUserGroup", new object[] { userId });
                string groupId = myRet1[1].ToString();
                string s = PublicKey.GetPublicKey2(userId, dataBase, solution, 0) + ":" + HttpUtility.UrlEncode(userName) + "-" + groupId;
                return s;
            }
            else
            {
                return string.Empty;
            }
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    [WebMethod(EnableSession = true)]
    public void SingleSignOut()
    {
        //CliUtils.fLoginUser = "";
        //CliUtils.fLoginPassword = "";
        //CliUtils.fLoginDB = "";
        //CliUtils.fCurrentProject = "";
        //CliUtils.fClientSystem = "";
        //CliUtils.fUserName = "";
    }
}

