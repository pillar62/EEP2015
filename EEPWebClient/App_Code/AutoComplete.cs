using System;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Data;
using System.Collections.Generic;
using Srvtools;
using System.Runtime.Remoting;
using Microsoft.Win32;

/// <summary>
/// AutoComplete 的摘要说明
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[System.Web.Script.Services.ScriptService]
public class AutoComplete : System.Web.Services.WebService
{
    public AutoComplete()
    {
        CliUtils.LoadLoginServiceConfig(string.Format("{0}\\EEPWebClient.exe.config", EEPRegistry.WebClient));
        string message = "";
        if (CliUtils.Register(ref message) == false)
        {
            throw new Exception(message);
        }
    }

    [WebMethod]
    public string[] GetCompletionList(string prefixText, int count, 
        string commandTable, string dataKeyField, 
        string userId, string password, string dataBase, string solution)
    {
        List<string> items = new List<string>();
        if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(dataBase) && !string.IsNullOrEmpty(solution))
        {
            string securityKey = SingleSignOn(userId, password, dataBase, solution);
            object[] clientInfo = CreateClientInfo(userId, password, dataBase, solution, SYS_LANGUAGE.ENG);
            DataTable table = ExecuteSql(clientInfo,"GLModule", "cmdDDUse", "select " + dataKeyField + " from " + commandTable + " where " + dataKeyField + " like '" + prefixText + "%'", true).Tables[0];

            count = Math.Min(count, table.Rows.Count);
            for (int i = 0; i < count; i++)
            {
                items.Add(table.Rows[i][dataKeyField].ToString());
            }
        }
        return items.ToArray();
    }


    private object[] InitClientInfoForAllPlatform(string securityKey)
    {
        string[] ss = securityKey.Split("-".ToCharArray());

        string userId = ss[0];
        string dataBase = ss[1];
        string solution = ss[2];
        int language = int.Parse(ss[3]);

        return CreateClientInfo(userId, null, dataBase, solution, (SYS_LANGUAGE)language);
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

    private DataSet ExecuteSql(object[] clientInfo, string ModuleName, string DataSetName, string sSql, bool IsCursor)
    {
        object[] aryRet = CliUtils.RemoteObject.ExecuteSql(new object[] { clientInfo }, ModuleName, DataSetName, sSql, IsCursor);
        if (null != aryRet)
        {
            if ((0 == (int)(aryRet[0])))
            {
                if (IsCursor)
                {
                    byte[] buff = (byte[])(aryRet[1]);
                    return DataSetCompressor.Decompress(buff);
                }
                else
                    return null;
            }
            else
            {
                return null;
            }
        }
        else
            return null;
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
                string s = PublicKey.GetPublicKey(userId, dataBase, solution) + ":" + userName + "-" + groupId;
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
    //    CliUtils.fLoginUser = "";
    //    CliUtils.fLoginPassword = "";
    //    CliUtils.fLoginDB = "";
    //    CliUtils.fCurrentProject = "";
    //    CliUtils.fClientSystem = "";
    //    CliUtils.fUserName = "";
    }
}

