using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Srvtools;
using System.Data;
using System.Collections;

/// <summary>
/// Summary description for EEPMobile
/// </summary>
[WebService(Namespace = "http://infolight.com/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class EEPMobile : System.Web.Services.WebService
{

    public EEPMobile()
    {
        CliUtils.LoadLoginServiceConfig(string.Format("{0}\\EEPWebClient.exe.config", EEPRegistry.WebClient));
        string message = "";
        if (CliUtils.Register(ref message) == false)
        {
            throw new Exception(message);
        }
    }

    [WebMethod]
    public ArrayList GetDatabases()
    {
        object[] clientInfo = CreateClientInfo("", "", "", "", 0);
        object[] ret = CallMethod(clientInfo, "GLModule", "GetDB", null);
        if (ret != null && (int)ret[0] == 0)
        {
            return (ArrayList)ret[1];
        }
        return null;
    }

    [WebMethod]
    public DataSet GetSolutions()
    {
        object[] clientInfo = CreateClientInfo("", "", "", "", 0);
        object[] ret = CallMethod(clientInfo, "GLModule", "GetSolution", null);
        if (ret != null && (int)ret[0] == 0)
        {
            return (DataSet)ret[1];
        }
        return null;
    }

    [WebMethod]
    public string LoginEEP(string userID, string password, string database, string solution, int language)
    {
        object[] clientInfo = CreateClientInfo(userID, password, database, solution, (SYS_LANGUAGE)language);
        object[] ret = CallMethod(clientInfo, "GLModule", "CheckUser", new object[] { userID + ':' + password + ':' + database + ":0" });
        if (ret != null && (int)ret[0] == 0)
        {
            LoginResult result = (LoginResult)ret[1];
            if (result == LoginResult.Success)
            {
                return PublicKey.GetPublicKey2(userID, database, solution, language);
            }
            else
            {
                return string.Format("Error:{0}", result.ToString());
            }
        }
        else
        {
            return string.Format("Error:{0}", ret[1]);
        }
        return string.Empty;
    }

    [WebMethod]
    public void LogoutEEP(string userID)
    {
        object[] clientInfo = CreateClientInfo(userID, "", "", "", 0);
        CallMethod(clientInfo, "GLModule", "LogOut", new object[] { userID });
    }

    [WebMethod]
    public DataSet GetWFData(string publicKey, int WFDataMode)
    {
        if (PublicKey.CheckPublicKey2(publicKey))
        {
             object[] clientInfo = InitClientInfo(publicKey);
             lock (typeof(FLTools.GloFix))
             {
                 CliUtils.fClientSystem = "WebService";
                 CliUtils.fLoginDB = (string)clientInfo[2];
                 string userID = (string)clientInfo[1];
                 string sql = FLTools.GloFix.GetFlowSql(userID, (FLTools.ESqlMode)WFDataMode);
                 object[] ret = CallMethod(clientInfo, "GLModule", "ExcuteWorkFlow", new object[] { sql });
                 if (ret != null && (int)ret[0] == 0)
                 {
                     DataSet dataSet = (DataSet)ret[1];
                     DataTable dataTable = dataSet.Tables[0];
                     dataTable.Columns.Add(new DataColumn("HyperLink", typeof(string)));
                     for (int i = 0; i < dataTable.Rows.Count; i++)
                     {
                         DataRow row = dataTable.Rows[i];
                         string url = CreateFlowUrl(row, (FLTools.ESqlMode)WFDataMode);
                         row["HyperLink"] = string.Format("SingleSignOn.aspx?PublicKey={0}&RedirectUrl={1}", publicKey, url);
                     }

                     return dataSet;
                 }
             }
        }
        return null;
    }

    [WebMethod]
    public DataSet GetMenuData(string publicKey)
    {
        if (PublicKey.CheckPublicKey2(publicKey))
        {
            object[] clientInfo = InitClientInfo(publicKey);
            object[] ret = CallMethod(clientInfo, "GLModule", "FetchMenus", new object[] { clientInfo[6], "W" });
            if (ret != null && (int)ret[0] == 0)
            {
                DataSet dataSet = (DataSet)ret[1];
                DataTable dataTable = dataSet.Tables[0];
                dataTable.Columns.Add(new DataColumn("HyperLink", typeof(string)));
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    DataRow row = dataTable.Rows[i];
                    string url = CreateMenuUrl(row);
                    row["HyperLink"] = string.Format("SingleSignOn.aspx?PublicKey={0}&RedirectUrl={1}", publicKey, url);
                }
                return dataSet;
            }
        }
        return null;
    }

    private string CreateMenuUrl(DataRow row)
    {
        var PACKAGE = row["PACKAGE"].ToString();
        var FORM = row["FORM"].ToString();
        var ITEMPARAM = row["ITEMPARAM"].ToString();
        //flow 的参数
        var MODULETYPE = row["MODULETYPE"].ToString();

        if (MODULETYPE == "O")
        {
            return string.Format("InnerPages/FlowDesigner.aspx?FlowFileName={0}", FORM);
        }
        else
        {
            if (string.IsNullOrEmpty(ITEMPARAM))
            {
                return string.Format("{0}/{1}.aspx", PACKAGE, FORM);
            }
            else
            {
                return string.Format("{0}/{1}.aspx?ItemParam={2}", PACKAGE, FORM, HttpUtility.UrlEncode(ITEMPARAM));
            }
        }
    }

    private string CreateFlowUrl(DataRow row, FLTools.ESqlMode WFDataMode)
    {
        var LISTID = row["LISTID"].ToString();
        var FLOWPATH = row["FLOWPATH"].ToString();
        var FORM_NAME = row["WEBFORM_NAME"].ToString();
        if (string.IsNullOrEmpty(FORM_NAME))
        {
            return string.Empty;
        }
        var packageName = FORM_NAME.Split('.')[0];
        var formName = FORM_NAME.Split('.')[1];
        var FORM_PRESENTATION = row["FORM_PRESENTATION"].ToString();
  
        var ATTACHMENTS = row["ATTACHMENTS"].ToString();
       
        var VDSNAME = row["VDSNAME"].ToString();
        switch (WFDataMode)
        {
            case FLTools.ESqlMode.ToDoList:
                {
                    var NAVIGATOR_MODE = row["NAVIGATOR_MODE"].ToString();
                    var FLNAVIGATOR_MODE = row["FLNAVIGATOR_MODE"].ToString();
                    var FLOWIMPORTANT = row["FLOWIMPORTANT"].ToString();
                    var FLOWURGENT = row["FLOWURGENT"].ToString();
                    var STATUS = row["STATUS"].ToString();
                    var PLUSAPPROVE = row["PLUSAPPROVE"].ToString();
                    var MULTISTEPRETURN = row["MULTISTEPRETURN"].ToString();
                    var SENDTOID = row["SENDTO_ID"].ToString();
                    return string.Format("{0}/{1}.aspx?LISTID={2}&FLOWPATH={3}&WHERESTRING={4}&NAVMODE={5}&FLNAVMODE={6}&ISIMPORTANT={7}"
                        + "&ISURGENT={8}&STATUS={9}&PLUSAPPROVE={10}&MULTISTEPRETURN={11}&ATTACHMENTS={12}&&SENDTOID={13}&&VDSNAME={14}"
                       , packageName, formName, LISTID, HttpUtility.UrlEncode(FLOWPATH), HttpUtility.UrlEncode(FORM_PRESENTATION)
                       , NAVIGATOR_MODE, FLNAVIGATOR_MODE, FLOWIMPORTANT, FLOWURGENT, STATUS
                       , PLUSAPPROVE, MULTISTEPRETURN, HttpUtility.UrlEncode(ATTACHMENTS), SENDTOID, VDSNAME);
                }
            case FLTools.ESqlMode.ToDoHis:
                {
                    return string.Format("{0}/{1}.aspx?LISTID={2}&FLOWPATH={3}&WHERESTRING={4}&NAVMODE=0&FLNAVMODE=6&ATTACHMENTS={5}&VDSNAME={6}"
                        , packageName, formName, LISTID, HttpUtility.UrlEncode(FLOWPATH), HttpUtility.UrlEncode(FORM_PRESENTATION)
                        , HttpUtility.UrlEncode(ATTACHMENTS), VDSNAME);
                }
            case FLTools.ESqlMode.Notify:
                {
                    var NAVIGATOR_MODE = row["NAVIGATOR_MODE"].ToString();
                    var FLNAVIGATOR_MODE = row["FLNAVIGATOR_MODE"].ToString();
                    var FLOWIMPORTANT = row["FLOWIMPORTANT"].ToString();
                    var FLOWURGENT = row["FLOWURGENT"].ToString();
                    var STATUS = row["STATUS"].ToString();
                    var PLUSAPPROVE = row["PLUSAPPROVE"].ToString();
                    var MULTISTEPRETURN = row["MULTISTEPRETURN"].ToString();
                    var SENDTOID = row["SENDTO_ID"].ToString();

                    return string.Format("{0}/{1}.aspx?LISTID={2}&FLOWPATH={3}&WHERESTRING={4}&NAVMODE={5}&FLNAVMODE={6}&ISIMPORTANT={7}"
                        + "&ISURGENT={8}&STATUS={9}&PLUSAPPROVE={10}&MULTISTEPRETURN={11}&ATTACHMENTS={12}&&SENDTOID={13}&&VDSNAME={14}"
                       , packageName, formName, LISTID, HttpUtility.UrlEncode(FLOWPATH), HttpUtility.UrlEncode(FORM_PRESENTATION)
                       , NAVIGATOR_MODE, FLNAVIGATOR_MODE, FLOWIMPORTANT, FLOWURGENT, STATUS
                       , PLUSAPPROVE, MULTISTEPRETURN, HttpUtility.UrlEncode(ATTACHMENTS), SENDTOID, VDSNAME);
                }
            default:
                return string.Empty;
        }
    }

    private object[] CreateClientInfo(string user, string password, string database, string solution, SYS_LANGUAGE language)
    {
        return new object[] { language, user, database
                , "", "", ""
                , solution, "", ""
                , "", "", ""
                , "", "", "", "0", password };
    }

    private object[] InitClientInfo(string publicKey)
    {
        string[] ss = System.Text.RegularExpressions.Regex.Split(publicKey, PublicKey.SPLIT_STRING);

        string userId = ss[0];
        string dataBase = ss[1];
        string solution = ss[2];
        int language = int.Parse(ss[3]);

        return CreateClientInfo(userId, null, dataBase, solution, (SYS_LANGUAGE)language);
    }

    private object[] CallMethod(object[] clientInfo, string moduleName, string methodName, object[] objParams)
    {
        return CliUtils.RemoteObject.CallMethod(new object[] { clientInfo }, moduleName, methodName, objParams);
    }

}
