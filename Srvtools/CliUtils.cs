using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;
using System.Windows.Forms;
using System.Net;
using System.Threading;
using System.Xml;
using Microsoft.Win32;
using System.Web;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Runtime.Remoting;
using System.Reflection;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Data.Odbc;
using System.Data.OracleClient;
using System.Runtime.Remoting.Channels.Http;
using System.Web.UI;
#if MySql
using MySql.Data.MySqlClient;
#endif
#if Informix
using IBM.Data.Informix;
#endif
#if Sybase
using Sybase.Data.AseClient;
#endif
#if UseCrystalReportDD
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Web;
#endif


namespace Srvtools
{
    /// <summary>
    /// check user login result
    /// </summary>
    public enum LoginResult
    {
        /// <summary>
        /// success
        /// </summary>
        Success,
        /// <summary>
        /// user not found
        /// </summary>
        UserNotFound,
        /// <summary>
        /// password is error
        /// </summary>
        PasswordError,
        /// <summary>
        /// user has logined and not allow to login again
        /// </summary>
        UserLogined,
        /// <summary>
        /// user has logined and allow to login again
        /// </summary>
        RequestReLogin,
        /// <summary>
        /// user has been disabled
        /// </summary>
        Disabled
    }

    public class CliUtils
    {
        static private EEPRemoteModule remoteobject = null;
        static private ListenerService FListenerService = null;
        public static bool applicationQuit = false;
        public static bool closeProtected = false;

        static public EEPRemoteModule RemoteObject
        {
            get
            {
                if (null == remoteobject)
                    remoteobject = new EEPRemoteModule();
            Label_RemoteObject:
                try
                {
                    if (fClientSystem == "Web")
                    {
                        try
                        {
                            if (!remoteobject.Check())
                            {
                                //redirect 
                                remoteobject = Activator.GetObject(typeof(EEPRemoteModule),
                                 string.Format("http://{0}:{1}/InfoRemoteModule.rem", fRemoteIP, fRemotePort)) as EEPRemoteModule;
                            }
                            remoteobject.ToString();
                        }
                        catch
                        {
                            //redirect 
                            if (HttpContext.Current != null)
                            {
                                CliUtils.LoadLoginServiceConfig(Path.Combine(HttpContext.Current.Request.MapPath(HttpContext.Current.Request.ApplicationPath), "EEPWebClient.exe.config"));
                            }
                            remoteobject = Activator.GetObject(typeof(EEPRemoteModule),
                             string.Format("http://{0}:{1}/InfoRemoteModule.rem", fRemoteIP, fRemotePort)) as EEPRemoteModule;
                        }
                        //remoteobject = new EEPRemoteModule();
                    }
                    else
                    {
                        try
                        {
                            remoteobject.ToString();
                        }
                        catch
                        {
                            //redirect 
                            remoteobject = Activator.GetObject(typeof(EEPRemoteModule),
                             string.Format("http://{0}:{1}/InfoRemoteModule.rem", fRemoteIP, fRemotePort)) as EEPRemoteModule;
                        }
                    }
                }
                catch (WebException e)
                {
                    if (e.Status == WebExceptionStatus.ConnectFailure)
                    {
                        if (PassByEEPListener())
                        {
                            //EEPListenerService.StartupEEPNetServer();

                            if (LoadServer())
                            {
                                goto Label_RemoteObject;
                            }
                            else
                            {
                                throw;
                            }
                        }
                    }
                    else
                    {
                        throw;
                    }
                }
                //catch (Exception E)
                //{
                //    if ((string.Compare(E.Message.ToLower(), "unable to connect to the remote server") == 0) &&
                //        PassByEEPListener())
                //    {
                //        EEPListenerService.StartupEEPNetServer();
                //        goto Label_RemoteObject;
                //    }
                //    else
                //    {
                //        throw new Exception(E.Message);
                //    }
                //}
                return remoteobject;
            }
        }

        static private string _fSiteCode = "";//这个sitecode要由用户来设...

        public static string fSiteCode
        {
            get
            {
                if (fClientSystem == "Web")
                {
                    object o = HttpContext.Current.Session["fSiteCode"];
                    return o == null ? "" : o.ToString();
                }
                else
                {
                    return _fSiteCode;
                }
            }
            set
            {
                if (fClientSystem == "Web")
                {
                    HttpContext.Current.Session["fSiteCode"] = value;
                }
                else
                {
                    _fSiteCode = value;
                }
            }
        }
        static public string fClientSystem = "";
        static public IntPtr fCliMainHandle;
        static public Int32 fRemotePort = 8989;
        static public string fRemoteIP = "";
        static public string reserveServerIP = string.Empty;
        static public int reserveServerPort = 8989;
        static public String fServerLoaderURL = "";
        static public bool fSolutionSecurity = false;

        static private string _fLoginUser = "";
        static private string _fUserName = "";
        static private string _fGroupID = "";
        static private string _fGroupName = "";
        static private string _fLoginPassword = "";
        static private string _fLoginDB = "";
        static private ClientType _fLoginDBType = ClientType.ctNone;
        static private OdbcDBType _fLoginOdbcType = OdbcDBType.None;
        static private SYS_LANGUAGE _fClientLang = SYS_LANGUAGE.ENG;
        static private string _fComputerName = SetComputerIP();
        static private string _fComputerIp = SetComputerIP();
        //static private string _fComputerIp = Dns.GetHostEntry(fComputerName).AddressList[0].ToString();
        static private String SetComputerIP()
        {

            IPAddress[] ips = Dns.GetHostEntry(Dns.GetHostName()).AddressList;

            Regex reg = new Regex(@"^(?:(?:25[0-5]|2[0-4]\d|[01]\d\d|\d?\d)(?(?=\.?\d)\.)){4}$");

            //Regex reg = new Regex(@"\d{0,3}\.\d{0,3}\.\d{0,3}\.\d{0,3}");
            String ip = "";
            for (int i = 0; i < ips.Length; i++)
            {
                if (reg.IsMatch(ips[i].ToString()))
                {
                    ip = ips[i].ToString();
                    break;
                }
            }
            return ip;
        }
        static private string _fCurrentProject = "";
        static private string _fCurrentProjectName = "";
        static private string _UserPara1 = "";
        static private string _UserPara2 = "";
        static private int _fPassWordMaxSize = 10;
        static private int _fPassWordMinSize = 0;
        static private int _fPassWordExpiry = 99999;
        static private int _fPassWordNotify = 0;
        static private bool _fPassWordCharNum = false;
        static private bool _fLogMenuOpenForm = false;


        private static string fModuleName;

        static private bool _fClientMainFlow = true;
        static private string _fFlowSelectedRole = "";

        public static bool fClientMainFlow
        {
            get
            {
                if (fClientSystem == "Web")
                {
                    object o = HttpContext.Current.Session["fClientMainFlow"];
                    return ((o == null) ? true : (bool)o);
                }
                else
                    return _fClientMainFlow;
            }
            set
            {
                if (fClientSystem == "Web")
                    HttpContext.Current.Session["fClientMainFlow"] = value;
                else
                    _fClientMainFlow = value;

            }
        }

        public static string fFlowSelectedRole
        {
            get
            {
                if (fClientSystem == "Web")
                {
                    object o = HttpContext.Current.Session["fFlowSelectedRole"];
                    return o == null ? "" : o.ToString();
                }
                else
                    return _fFlowSelectedRole;
            }
            set
            {
                if (fClientSystem == "Web")
                    HttpContext.Current.Session["fFlowSelectedRole"] = value;
                else
                    _fFlowSelectedRole = value;

            }
        }

        public static string fLoginUser
        {
            get
            {
                if (fClientSystem == "Web")
                {
                    object o = HttpContext.Current.Session["fLoginUser"];
                    return o == null ? "" : o.ToString();
                }
                else
                    return _fLoginUser;
            }
            set
            {
                if (fClientSystem == "Web")
                    HttpContext.Current.Session["fLoginUser"] = value;
                else
                    _fLoginUser = value;

            }
        }

        public static string fUserName
        {
            get
            {
                if (fClientSystem == "Web")
                {
                    object o = HttpContext.Current.Session["fUserName"];
                    return o == null ? "" : o.ToString();
                }
                else
                    return _fUserName;
            }
            set
            {
                if (fClientSystem == "Web")
                    HttpContext.Current.Session["fUserName"] = value;
                else
                    _fUserName = value;

            }
        }

        public static string fGroupID
        {
            get
            {
                if (fClientSystem == "Web")
                {
                    object o = HttpContext.Current.Session["fGroupID"];
                    return o == null ? "" : o.ToString();
                }
                else
                    return _fGroupID;
            }
            set
            {
                if (fClientSystem == "Web")
                    HttpContext.Current.Session["fGroupID"] = value;
                else
                    _fGroupID = value;
            }
        }

        public static string fGroupName
        {
            get
            {
                if (fClientSystem == "Web")
                {
                    object o = HttpContext.Current.Session["fGroupName"];
                    return o == null ? "" : o.ToString();
                }
                else
                    return _fGroupName;
            }
            set
            {
                if (fClientSystem == "Web")
                    HttpContext.Current.Session["fGroupName"] = value;
                else
                    _fGroupName = value;
            }
        }

        private static String _CurrentGroup;
        public static String CurrentGroup
        {
            get
            {
                if (fClientSystem == "Web")
                {
                    object o = HttpContext.Current.Session["CurrentGroup"];
                    return o == null ? "" : o.ToString();
                }
                else
                    return _CurrentGroup;
            }
            set
            {
                if (fClientSystem == "Web")
                    HttpContext.Current.Session["fGroupName"] = value;
                else
                    _CurrentGroup = value;
            }
        }

        public static string fLoginPassword
        {
            get
            {
                if (fClientSystem == "Web")
                {
                    object o = HttpContext.Current.Session["fLoginPassword"];
                    return o == null ? "" : o.ToString();
                }
                else
                    return _fLoginPassword;
            }
            set
            {
                if (fClientSystem == "Web")
                    HttpContext.Current.Session["fLoginPassword"] = value;
                else
                    _fLoginPassword = value;
            }
        }

        public static string fLoginDB
        {
            get
            {
                if (fClientSystem == "Web")
                {
                    object o = HttpContext.Current.Session["fLoginDB"];
                    return o == null ? "" : o.ToString();
                }
                else
                    return _fLoginDB;
            }
            set
            {
                if (fClientSystem == "Web")
                    HttpContext.Current.Session["fLoginDB"] = value;
                else
                    _fLoginDB = value;

            }
        }

        private static bool _fLoginDBSplit;
        public static bool fLoginDBSplit
        {
            get
            {
                if (fClientSystem == "Web")
                {
                    object o = HttpContext.Current.Session["fLoginDBSplit"];
                    return o == null ? false : (bool)o;
                }
                else
                    return _fLoginDBSplit;
            }
            set
            {
                if (fClientSystem == "Web")
                    HttpContext.Current.Session["fLoginDBSplit"] = value;
                else
                    _fLoginDBSplit = value;
            }
        }

        public static ClientType fLoginDBType
        {
            get
            {
                if (fClientSystem == "Web")
                {
                    object o = HttpContext.Current.Session["fLoginDBType"];
                    return o == null ? ClientType.ctNone : (ClientType)o;
                }
                else
                    return _fLoginDBType;
            }
            set
            {
                if (fClientSystem == "Web")
                    HttpContext.Current.Session["fLoginDBType"] = value;
                else
                    _fLoginDBType = value;
            }
        }

        public static OdbcDBType fLoginOdbcType
        {
            get
            {
                if (fClientSystem == "Web")
                {
                    object o = HttpContext.Current.Session["fLoginOdbcType"];
                    return o == null ? OdbcDBType.None : (OdbcDBType)o;
                }
                else
                    return _fLoginOdbcType;
            }
            set
            {
                if (fClientSystem == "Web")
                    HttpContext.Current.Session["fLoginOdbcType"] = value;
                else
                    _fLoginOdbcType = value;
            }

        }

        public static SYS_LANGUAGE fClientLang
        {
            get
            {
                if (fClientSystem == "Web")
                {
                    object o = HttpContext.Current.Session["fClientLang"];
                    return o == null ? SYS_LANGUAGE.ENG : (SYS_LANGUAGE)o;
                }
                else
                    return _fClientLang;
            }
            set
            {
                if (fClientSystem == "Web")
                    HttpContext.Current.Session["fClientLang"] = value;
                else
                    _fClientLang = value;
            }
        }

        public static string fComputerName
        {
            get
            {
                if (fClientSystem == "Web")
                {
                    object o = HttpContext.Current.Session["fComputerName"];
                    return o == null ? "" : o.ToString();
                }
                else
                    return _fComputerName;
            }
            set
            {
                if (fClientSystem == "Web")
                    HttpContext.Current.Session["fComputerName"] = value;
                else
                    _fComputerName = value;
            }
        }

        public static string fComputerIp
        {
            get
            {
                if (fClientSystem == "Web")
                {
                    object o = HttpContext.Current.Session["fComputerIp"];
                    return o == null ? "" : o.ToString();
                }
                else
                    return _fComputerIp;
            }
            set
            {
                if (fClientSystem == "Web")
                    HttpContext.Current.Session["fComputerIp"] = value;
                else
                    _fComputerIp = value;
            }
        }

        public static string fCurrentProject
        {
            get
            {
                if (fClientSystem == "Web")
                {
                    object o = HttpContext.Current.Session["fCurrentProject"];
                    return o == null ? "" : o.ToString();
                }
                else
                    return _fCurrentProject;
            }
            set
            {
                if (fClientSystem == "Web")
                    HttpContext.Current.Session["fCurrentProject"] = value;
                else
                    _fCurrentProject = value;
            }
        }

        public static string fCurrentProjectName
        {
            get
            {
                if (fClientSystem == "Web")
                {
                    object o = HttpContext.Current.Session["fCurrentProjectName"];
                    return o == null ? "" : o.ToString();
                }
                else
                    return _fCurrentProjectName;
            }
            set
            {
                if (fClientSystem == "Web")
                    HttpContext.Current.Session["fCurrentProjectName"] = value;
                else
                    _fCurrentProjectName = value;
            }
        }

        public static string UserPara1
        {
            get
            {
                if (fClientSystem == "Web")
                {
                    object o = HttpContext.Current.Session["UserPara1"];
                    return o == null ? "" : o.ToString();
                }
                else
                    return _UserPara1;
            }
            set
            {
                if (fClientSystem == "Web")
                    HttpContext.Current.Session["UserPara1"] = value;
                else
                    _UserPara1 = value;
            }
        }

        public static string UserPara2
        {
            get
            {
                if (fClientSystem == "Web")
                {
                    object o = HttpContext.Current.Session["UserPara2"];
                    return o == null ? "" : o.ToString();
                }
                else
                    return _UserPara2;
            }
            set
            {
                if (fClientSystem == "Web")
                    HttpContext.Current.Session["UserPara2"] = value;
                else
                    _UserPara2 = value;
            }
        }

        private static string _ReplaceTableName = string.Empty;

        public static string ReplaceTableName
        {
            get
            {
                if (fClientSystem == "Web")
                {
                    object o = HttpContext.Current.Session["ReplaceTableName"];
                    return o == null ? string.Empty : o.ToString();
                }
                else
                {
                    return _ReplaceTableName;
                }
            }
            set
            {
                if (fClientSystem == "Web")
                {
                    HttpContext.Current.Session["ReplaceTableName"] = value;
                }
                else
                {
                    _ReplaceTableName = value;
                }
            }
        }

        private static string _Roles = string.Empty;

        public static string Roles
        {
            get
            {
                if (fClientSystem == "Web")
                {
                    object o = HttpContext.Current.Session["Roles"];
                    return o == null ? string.Empty : o.ToString();
                }
                else
                {
                    return _Roles;
                }
            }
            set
            {
                if (fClientSystem == "Web")
                {
                    HttpContext.Current.Session["Roles"] = value;
                }
                else
                {
                    _Roles = value;
                }
            }
        }

        private static string _OrgRoles = string.Empty;

        public static string OrgRoles
        {
            get
            {
                if (fClientSystem == "Web")
                {
                    object o = HttpContext.Current.Session["OrgRoles"];
                    return o == null ? string.Empty : o.ToString();
                }
                else
                {
                    return _OrgRoles;
                }
            }
            set
            {
                if (fClientSystem == "Web")
                {
                    HttpContext.Current.Session["OrgRoles"] = value;
                }
                else
                {
                    _OrgRoles = value;
                }
            }
        }

        private static string _OrgShares = string.Empty;

        public static string OrgShares
        {
            get
            {
                if (fClientSystem == "Web")
                {
                    object o = HttpContext.Current.Session["OrgShares"];
                    return o == null ? string.Empty : o.ToString();
                }
                else
                {
                    return _OrgShares;
                }
            }
            set
            {
                if (fClientSystem == "Web")
                {
                    HttpContext.Current.Session["OrgShares"] = value;
                }
                else
                {
                    _OrgShares = value;
                }
            }
        }

        private static string _OrgKind = "0";

        public static string OrgKind
        {
            get
            {
                if (fClientSystem == "Web")
                {
                    object o = HttpContext.Current.Session["OrgKind"];
                    return o == null ? "0" : o.ToString();
                }
                else
                {
                    return _OrgKind;
                }
            }
            set
            {
                if (fClientSystem == "Web")
                {
                    HttpContext.Current.Session["OrgKind"] = value;
                }
                else
                {
                    _OrgKind = value;
                }
            }
        }


        public static int fPassWordMaxSize
        {
            get
            {
                if (fClientSystem == "Web")
                {
                    object o = HttpContext.Current.Session["fPassWordMaxSize"];
                    return o == null ? 10 : Convert.ToInt32(o);
                }
                else
                    return _fPassWordMaxSize;
            }
            set
            {
                if (fClientSystem == "Web")
                    HttpContext.Current.Session["fPassWordMaxSize"] = value;
                else
                    _fPassWordMaxSize = value;
            }
        }

        public static int fPassWordMinSize
        {
            get
            {
                if (fClientSystem == "Web")
                {
                    object o = HttpContext.Current.Session["fPassWordMinSize"];
                    return o == null ? 0 : Convert.ToInt32(o);
                }
                else
                    return _fPassWordMinSize;
            }
            set
            {
                if (fClientSystem == "Web")
                    HttpContext.Current.Session["fPassWordMinSize"] = value;
                else
                    _fPassWordMinSize = value;
            }
        }

        public static int fPassWordExpiry
        {
            get
            {
                if (fClientSystem == "Web")
                {
                    object o = HttpContext.Current.Session["fPassWordExpiry"];
                    return o == null ? 999999 : Convert.ToInt32(o);
                }
                else
                    return _fPassWordExpiry;
            }
            set
            {
                if (fClientSystem == "Web")
                    HttpContext.Current.Session["fPassWordExpiry"] = value;
                else
                    _fPassWordExpiry = value;
            }
        }

        public static int fPassWordNotify
        {
            get
            {
                if (fClientSystem == "Web")
                {
                    object o = HttpContext.Current.Session["fPassWordNotify"];
                    return o == null ? 0 : Convert.ToInt32(o);
                }
                else
                    return _fPassWordNotify;
            }
            set
            {
                if (fClientSystem == "Web")
                    HttpContext.Current.Session["fPassWordNotify"] = value;
                else
                    _fPassWordNotify = value;
            }
        }

        public static bool fPassWordCharNum
        {
            get
            {
                if (fClientSystem == "Web")
                {
                    object o = HttpContext.Current.Session["fPassWordCharNum"];
                    return o == null ? false : Convert.ToBoolean(o);
                }
                else
                    return _fPassWordCharNum;
            }
            set
            {
                if (fClientSystem == "Web")
                    HttpContext.Current.Session["fPassWordCharNum"] = value;
                else
                    _fPassWordCharNum = value;
            }
        }

        public static bool fLogMenuOpenForm
        {
            get
            {
                if (fClientSystem == "Web")
                {
                    object o = HttpContext.Current.Session["fLogMenuOpenForm"];
                    return o == null ? false : Convert.ToBoolean(o);
                }
                else
                    return _fLogMenuOpenForm;
            }
            set
            {
                if (fClientSystem == "Web")
                    HttpContext.Current.Session["fLogMenuOpenForm"] = value;
                else
                    _fLogMenuOpenForm = value;
            }
        }

        static CliUtils()
        {
            // Get the client appliation exception.
            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
        }

        public static void Application_ThreadException(object sender, ThreadExceptionEventArgs args)
        {
            Exception e = (Exception)args.Exception;
            if (fClientSystem.ToLower() == "web")
            {
                throw e;
            }
            else
            {
                ScreenCapture sc = new ScreenCapture();
                Image image = sc.CaptureWindow(fCliMainHandle);
                MemoryStream ms = new MemoryStream();
                image.Save(ms, ImageFormat.Jpeg);
                Byte[] bytes = ms.ToArray();

                // ExecuteSql("GLModule", "cmdLogError", sql, false, fCurrentProject);
                if (e is TargetInvocationException && e.InnerException != null)
                {
                    e = e.InnerException;
                }
                string serverstatck = e.InnerException == null ? string.Empty : e.InnerException.StackTrace;
                ErrorDialog fError = new ErrorDialog(e.Message, e.StackTrace, serverstatck);
                fError.ShowDialog();
                Boolean isFeedback = fError.IsFeedback;
                String errDescrip = fError.ErrDescrip;
                fError.Dispose();

                if (isFeedback)
                {
                    FeedbackBug(e.Message, e.StackTrace, errDescrip, bytes, false);
                }
            }
        }

        public static void FeedbackBug(String errMessage, String stack, String errDescrip, Byte[] bytes, bool isWebClient)
        {
            List<Object> listParam = new List<object>();
            listParam.Add(fLoginUser);
            listParam.Add(fModuleName);
            listParam.Add(errMessage);
            listParam.Add(stack);
            listParam.Add(errDescrip);
            listParam.Add(DateTime.Now);
            listParam.Add(bytes);
            listParam.Add("E");

            // listParam.Add(new byte[] { 0, 1, 0 });

            try
            {
                object[] objs = RemoteObject.CallMethod(new object[] { GetBaseClientInfo() }, "GLModule", "LogError", listParam.ToArray());
                if (objs != null && objs[0] != null && objs[0].ToString().ToUpper() == "0")
                {
                    LogErrToXml(fLoginUser, fModuleName, errMessage, stack, errDescrip, DateTime.Now, "E", isWebClient);
                }
            }
            catch
            {
                LogErrToXml(fLoginUser, fModuleName, errMessage, stack, errDescrip, DateTime.Now, "E", isWebClient);
            }
        }

        public static void LogErrToXml(string logUser, string moduleName, string message, string stack, string descrip, DateTime datetime, string tag,
            bool isWebClient)
        {
            String s;
            if (!isWebClient)
            {
                s = Application.StartupPath + @"\";
            }
            else
            {
                RegistryKey rk = Registry.LocalMachine.OpenSubKey("SOFTWARE\\infolight\\eep.net");
                s = (string)rk.GetValue("WebClient Path");
                if (s[s.Length - 1].ToString() != @"\")
                    s = s + @"\";
            }

            if (!Directory.Exists(s + @"Error"))
            {
                Directory.CreateDirectory(s + @"Error");
            }

            XmlDocument xmlDoc = new XmlDocument();
            XmlElement nErrors;
            if (!File.Exists(s + @"Error\SysErrLog.xml"))
            {
                nErrors = xmlDoc.CreateElement("Errors");
                xmlDoc.AppendChild(nErrors);
            }
            else
            {
                xmlDoc.Load(s + @"Error\SysErrLog.xml");
                nErrors = (XmlElement)xmlDoc.SelectSingleNode("Errors");
                if (nErrors == null)
                {
                    nErrors = xmlDoc.CreateElement("Errors");
                    xmlDoc.AppendChild(nErrors);
                }
            }

            XmlElement nError = xmlDoc.CreateElement("Error");

            XmlAttribute aLogUser = xmlDoc.CreateAttribute("LogUser");
            aLogUser.InnerText = logUser;
            nError.Attributes.Append(aLogUser);

            XmlAttribute aModuleName = xmlDoc.CreateAttribute("ModuleName");
            aModuleName.InnerText = moduleName;
            nError.Attributes.Append(aModuleName);

            XmlAttribute aMessage = xmlDoc.CreateAttribute("ErrMessage");
            aMessage.InnerText = message;
            nError.Attributes.Append(aMessage);

            XmlAttribute aCallStack = xmlDoc.CreateAttribute("CallStack");
            aCallStack.InnerText = stack;
            nError.Attributes.Append(aCallStack);

            XmlAttribute aDescrip = xmlDoc.CreateAttribute("ErrDescrip");
            aDescrip.InnerText = descrip;
            nError.Attributes.Append(aDescrip);

            XmlAttribute aDateTime = xmlDoc.CreateAttribute("DateTime");
            aDateTime.InnerText = datetime.ToString();
            nError.Attributes.Append(aDateTime);

            XmlAttribute aStatus = xmlDoc.CreateAttribute("Status");
            aStatus.InnerText = tag;
            nError.Attributes.Append(aStatus);

            nErrors.AppendChild(nError);
            xmlDoc.Save(s + @"Error\SysErrLog.xml");
        }

        static public object GetBaseClientInfo()
        {
            return new object[] { fClientLang, fLoginUser, fLoginDB
                , fSiteCode, fComputerName, fComputerIp
                , fCurrentProject, fClientSystem, fGroupID
                , UserPara1, UserPara2, ReplaceTableName
                , Roles, OrgRoles, OrgShares, OrgKind, fLoginPassword, null, CurrentGroup };
        }

        static public object[] WebLogOut(string ModuleName, string MethodName, object[] objParams, object[] clientInfo)
        {
            object[] myRet = RemoteObject.CallMethod(clientInfo, ModuleName, MethodName, objParams);
            if (null != myRet)
            {
                if (0 == (int)(myRet[0]))
                    return myRet;
                else
                {
                    // MessageBox.Show(ModuleName + ":" + MethodName + ":" + (string)(myRet[1]));
                    throw new Exception(ModuleName + ":" + MethodName + ":" + (string)(myRet[1]));
                }
            }
            else
                return null;
        }

        static public object[] CallFLMethod(string MethodName, object[] objParams)
        {
            object[] clientInfo = new object[] { GetBaseClientInfo(), -1, -1, string.Empty };
            object[] myRet = RemoteObject.CallFLMethod(clientInfo, MethodName, objParams);
            if (null != myRet)
            {
                if (0 == (int)(myRet[0]) || 2 == (int)(myRet[0]))
                    return myRet;
                else
                {
                    // MessageBox.Show(ModuleName + ":" + MethodName + ":" + (string)(myRet[1]));
                    throw new Exception(myRet[1].ToString());
                }
            }
            else
                return null;
        }

        // Scheduling
        //public static object[] CallFLMethod2(string MethodName, object[] objParams)
        //{
        //    object[] clientInfo = new object[] { GetBaseClientInfo(), -1, -1, string.Empty };

        //    object[] myRet = RemoteObject.CallFLMethod2(clientInfo, ModuleName, MethodName, objParams);

        //    return null;
        //}

        static public object[] CallMethod(string ModuleName, string MethodName, object[] objParams)
        {
            if (!MethodName.ToLower().Equals("checkuser") && !MethodName.ToLower().Equals("getsysmsgxml")
                 && !MethodName.ToLower().Equals("getdb") && !MethodName.ToLower().Equals("getsolution"))
            {
                CheckSession();
            }

            fModuleName = ModuleName;

            object[] myRet = RemoteObject.CallMethod(new object[] { GetBaseClientInfo() }, ModuleName, MethodName, objParams);
            if (null != myRet)
            {
                if (0 == (int)(myRet[0]))
                    return myRet;
                else
                {
                    Exception innerexception = myRet.Length == 3 ? (Exception)myRet[2] : null;
                    // MessageBox.Show(ModuleName + ":" + MethodName + ":" + (string)(myRet[1]));
                    throw new Exception(ModuleName + ":" + MethodName + ":" + (string)(myRet[1]), innerexception);
                }
            }
            else
                return null;
        }

        static public object[] CallMethod(string ModuleName, string MethodName, object[] objParams, bool isScheduling)
        {
            if (!MethodName.ToLower().Equals("checkuser") && !MethodName.ToLower().Equals("getsysmsgxml")
                 && !MethodName.ToLower().Equals("getdb") && !MethodName.ToLower().Equals("getsolution"))
            {
                CheckSession();
            }

            fModuleName = ModuleName;

            object[] myRet = RemoteObject.CallMethod(new object[] { GetBaseClientInfo() }, ModuleName, MethodName, objParams);
            if (null != myRet)
                return myRet;
            else
                return null;
        }

        public static DataSet GetMessage()
        {
            DataSet dsRet = null;
            object[] oRet = CallMethod("GLModule", "GetMessage", new object[] { fLoginUser });
            if (null != oRet)
            {
                if (0 == (int)(oRet[0])) dsRet = (DataSet)(oRet[1]);
            }
            return dsRet;
        }

        public static void SendMessage(bool bGroup, string sUsers, string sMessage, string sParams)
        {
            CallMethod("GLModule", "SendMessage", new object[] { bGroup, sUsers, sMessage, sParams });
        }

        public delegate void CallBack(object[] oRet);

        static public object[] AsyncCallMethod(string ModuleName, string MethodName, object[] objParams, CallBack justdo)
        {
            fModuleName = ModuleName;

            RemotingDelegates rd = new RemotingDelegates();
            rd.Run(new object[] { GetBaseClientInfo() }, ModuleName, MethodName, objParams, justdo);
            return null;
        }

        static public DataSet GetSqlCommand(string ModuleName, string DataSetName, InfoDataSet myDataSet, string strWhere, string sCurProject, string CommandText)
        {
            return GetSqlCommand(ModuleName, DataSetName, myDataSet, strWhere, sCurProject, CommandText, null);
        }

        static public DataSet GetSqlCommand(string ModuleName, string DataSetName, InfoDataSet myDataSet, string strWhere, string sCurProject, string CommandText, ArrayList paramWhere)
        {
            return GetSqlCommand(ModuleName, DataSetName, myDataSet, strWhere, sCurProject, CommandText, paramWhere, string.Empty);
        }

        public static DataSet GetSqlCommand(string ModuleName, string DataSetName, InfoDataSet myDataSet, string strWhere, string sCurProject, string CommandText, ArrayList paramWhere, string strOrder)
        {
            CheckSession();

            fModuleName = ModuleName;

            object[] baseClientInfo = (object[])GetBaseClientInfo();
            if ((sCurProject != null) && (!sCurProject.Equals("")))
            {
                baseClientInfo[6] = sCurProject;
            }

            ArrayList[] WhereParam = SeparateWhere(paramWhere);

            // object[] aryRet = RemoteObject.GetSqlCommand(new object[] { baseClientInfo, (object)(myDataSet.PacketRecords), (object)(myDataSet.LastKeyValues) }, ModuleName, DataSetName, strWhere);
            object[] aryRet = RemoteObject.GetSqlCommand(new object[] { baseClientInfo, (object)(myDataSet.PacketRecords), (object)(myDataSet.LastIndex), (object)CommandText, myDataSet.DataCompressed }, ModuleName, DataSetName, strWhere, false, WhereParam, strOrder);
            if (null != aryRet)
            {
                if (0 == (int)(aryRet[0]))
                {
                    if (myDataSet.DataCompressed)
                    {
                        byte[] buff = (byte[])(aryRet[1]);
                        return DataSetCompressor.Decompress(buff);
                    }
                    else
                    {
                        return (DataSet)(aryRet[1]);
                    }

                }
                else
                {
                    // MessageBox.Show("GetSqlCommand return error: " + (string)(aryRet[1]));
                    // throw new Exception("GetSqlCommand return error: " + (string)(aryRet[1]));
                    Application_ThreadException(null, new ThreadExceptionEventArgs(new Exception("GetSqlCommand return error: " + (string)(aryRet[1]))));
                    return null;
                }
            }
            else
                return null;
        }

        private static ArrayList[] SeparateWhere(ArrayList param)
        {
            if (param == null || param.Count == 0) return null;
            ArrayList[] Where = new ArrayList[param.Count];
            if (param.Count > 0)
            {
                if (fLoginDB != null && fLoginDB != "")
                {
                    object[] myRet = CallMethod("GLModule", "GetDataBaseType", new object[] { fLoginDB });
                    String type = "";
                    if (myRet != null && myRet[0].ToString() == "0")
                    {
                        type = myRet[1].ToString();
                    }

                    switch (type)
                    {
                        case "1":
                            for (int i = 0; i < param.Count; i++)
                            {
                                if (param[i] != null)
                                {
                                    Where[i] = new ArrayList();
                                    SqlParameter dbp = param[i] as SqlParameter;
                                    Where[i].Add(dbp.ParameterName);
                                    Where[i].Add(dbp.SqlDbType);
                                    Where[i].Add(dbp.Size);
                                    Where[i].Add(dbp.Value);
                                }
                            }
                            break;
                        case "2":
                            for (int i = 0; i < param.Count; i++)
                            {
                                if (param[i] != null)
                                {
                                    Where[i] = new ArrayList();
                                    OleDbParameter dbp = param[i] as OleDbParameter;
                                    Where[i].Add(dbp.ParameterName);
                                    Where[i].Add(dbp.OleDbType);
                                    Where[i].Add(dbp.Size);
                                    Where[i].Add(dbp.Value);
                                }
                            }
                            break;
                        case "3":
                            for (int i = 0; i < param.Count; i++)
                            {
                                Where[i] = new ArrayList();
                                if (param[i] != null)
                                {
                                    OracleParameter dbp = param[i] as OracleParameter;
                                    Where[i].Add(dbp.ParameterName);
                                    Where[i].Add(dbp.OracleType);
                                    Where[i].Add(dbp.Size);
                                    Where[i].Add(dbp.Value);
                                }
                            }
                            break;
                        case "4":
                            for (int i = 0; i < param.Count; i++)
                            {
                                if (param[i] != null)
                                {
                                    Where[i] = new ArrayList();
                                    OdbcParameter dbp = param[i] as OdbcParameter;
                                    Where[i].Add(dbp.ParameterName);
                                    Where[i].Add(dbp.OdbcType);
                                    Where[i].Add(dbp.Size);
                                    Where[i].Add(dbp.Value);
                                }
                            }
                            break;
#if MySql
                        case "5":
                            for (int i = 0; i < param.Count; i++)
                            {
                                if (param[i] != null)
                                {
                                    Where[i] = new ArrayList();
                                    MySqlParameter dbp = param[i] as MySqlParameter;
                                    Where[i].Add(dbp.ParameterName);
                                    Where[i].Add(dbp.MySqlDbType);
                                    Where[i].Add(dbp.Size);
                                    Where[i].Add(dbp.Value);
                                }
                            }
                            break;
#endif
#if Informix
                        case "6":
                            for (int i = 0; i < param.Count; i++)
                            {
                                if (param[i] != null)
                                {
                                    Where[i] = new ArrayList();
                                    IfxParameter dbp = param[i] as IfxParameter;
                                    Where[i].Add(dbp.ParameterName);
                                    Where[i].Add(dbp.IfxType);
                                    Where[i].Add(dbp.Size);
                                    Where[i].Add(dbp.Value);
                                }
                            }
                            break;
#endif
#if Sybase
                        case "7":
                            for (int i = 0; i < param.Count; i++)
                            {
                                if (param[i] != null)
                                {
                                    Where[i] = new ArrayList();
                                    AseParameter dbp = param[i] as AseParameter;
                                    Where[i].Add(dbp.ParameterName);
                                    Where[i].Add(dbp.AseDbType);
                                    Where[i].Add(dbp.Size);
                                    Where[i].Add(dbp.Value);
                                }
                            }
                            break;
#endif
                    }
                }
            }
            return Where;
        }

        public static IDbDataParameter CreateDataParameter(ClientType ct)
        {
            IDbDataParameter iParam = null;
            switch (ct)
            {
                case ClientType.ctMsSql:
                    iParam = new SqlParameter();
                    break;
                case ClientType.ctOleDB:
                    iParam = new OleDbParameter();
                    break;
                case ClientType.ctOracle:
                    iParam = new OracleParameter();
                    break;
                case ClientType.ctODBC:
                    iParam = new OdbcParameter();
                    break;
#if MySql
                case ClientType.ctMySql:
                    iParam = new MySqlParameter();
                    break;
#endif
#if Informix
                case ClientType.ctInformix:
                    iParam = new IfxParameter();
                    break;
#endif
#if Sybase
                case ClientType.ctSybase:
                    iParam = new AseParameter();
                    break;
#endif

            }
            return iParam;
        }

        public static int GetRecordsCount(string ModuleName, string DataSetName, string strWhere, string sCurProject)
        {
            CheckSession();

            fModuleName = ModuleName;

            object[] baseClientInfo = (object[])GetBaseClientInfo();
            if ((sCurProject != null) && (!sCurProject.Equals("")))
            {
                baseClientInfo[6] = sCurProject;
            }
            object[] aryRet = RemoteObject.GetRecordsCount(new object[] { baseClientInfo }, ModuleName, DataSetName, strWhere);
            if (null != aryRet)
            {
                if (0 == (int)(aryRet[0]))
                    return (int)(aryRet[1]);
                else
                {
                    // MessageBox.Show("GetSqlCommand return error: " + (string)(aryRet[1]));
                    // throw new Exception("GetSqlCommand return error: " + (string)(aryRet[1]));
                    Application_ThreadException(null, new ThreadExceptionEventArgs(new Exception("GetRecordsCount return error: " + (string)(aryRet[1]))));
                    return 0;
                }
            }
            else
                return 0;
        }

        static public DataSet ExecuteSql(string ModuleName, string DataSetName, string sSql, string DBAlias, bool IsCursor, string sCurProject, object[] packetRecords, ArrayList paramWhere)
        {
            return ExecuteSql(ModuleName, DataSetName, sSql, DBAlias, IsCursor, sCurProject, packetRecords, paramWhere, false);
        }

        //Get dictionary使用
        static internal DataSet ExecuteSql(string ModuleName, string DataSetName, string sSql, string DBAlias, bool IsCursor, string sCurProject, object[] packetRecords, ArrayList paramWhere, bool getSplitSystemDB)
        {
            CheckSession();

            fModuleName = ModuleName;

            object[] baseClientInfo = (object[])GetBaseClientInfo();
            if ((sCurProject != null) && (!sCurProject.Equals("")))
            {
                baseClientInfo[6] = sCurProject;
            }
            object recordsCount = null, lastIndex = null;
            if (packetRecords != null && packetRecords.Length == 2)
            {
                recordsCount = packetRecords[0];
                lastIndex = packetRecords[1];
            }

            ArrayList[] WhereParam = SeparateWhere(paramWhere);

            object[] aryRet = RemoteObject.ExecuteSql(new object[] { baseClientInfo, recordsCount, lastIndex, null/*為datacompress預留*/, getSplitSystemDB }, ModuleName, DataSetName, sSql, DBAlias, IsCursor, WhereParam);
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
                    // MessageBox.Show("GetSqlCommand return error: " + (string)(aryRet[1]));
                    // throw new Exception("GetSqlCommand return error: " + (string)(aryRet[1]));
                    Application_ThreadException(null, new ThreadExceptionEventArgs(new Exception("ExecuteSql return error: " + (string)(aryRet[1]))));
                    return null;
                }
            }
            else
                return null;
        }

        static public DataSet ExecuteSql(string ModuleName, string DataSetName, string sSql, string DBAlias, bool IsCursor, string sCurProject, object[] packetRecords)
        {
            return ExecuteSql(ModuleName, DataSetName, sSql, DBAlias, IsCursor, sCurProject, packetRecords, null);

        }

        static public DataSet ExecuteSql(string ModuleName, string DataSetName, string sSql, string DBAlias, bool IsCursor, string sCurProject)
        {
            return ExecuteSql(ModuleName, DataSetName, sSql, DBAlias, IsCursor, sCurProject, null);
        }

        static public DataSet ExecuteSql(string ModuleName, string DataSetName, string sSql, bool IsCursor, string sCurProject)
        {
            return ExecuteSql(ModuleName, DataSetName, sSql, null, IsCursor, sCurProject);
        }

        [Obsolete("The recommended alternative is DataTable.PrimaryKey", false)]
        static public ArrayList GetKeyFields(string ModuleName, string DataSetName, string sCurProject)
        {
            CheckSession();

            fModuleName = ModuleName;

            object[] baseClientInfo = (object[])GetBaseClientInfo();
            if ((sCurProject != null) && (!sCurProject.Equals("")))
            {
                baseClientInfo[6] = sCurProject;
            }
            object[] aryRet = RemoteObject.GetKeyFields(new object[] { baseClientInfo }, ModuleName, DataSetName);
            if (null != aryRet)
            {
                if (0 == (int)(aryRet[0]))
                    return (ArrayList)(aryRet[1]);
                else
                {
                    // MessageBox.Show("GetSqlCommand return error: " + (string)(aryRet[1]));
                    // throw new Exception("GetSqlCommand return error: " + (string)(aryRet[1]));
                    Application_ThreadException(null, new ThreadExceptionEventArgs(new Exception("GetKeyFields return error: " + (string)(aryRet[1]))));
                    return null;
                }
            }
            else
                return null;
        }

        static public string GetSqlCommandText(string ModuleName, string DataSetName, string sCurProject)
        {
            CheckSession();

            fModuleName = ModuleName;

            object[] baseClientInfo = (object[])GetBaseClientInfo();
            if ((sCurProject != null) && (!sCurProject.Equals("")))
            {
                baseClientInfo[6] = sCurProject;
            }
            object[] aryRet = RemoteObject.GetSqlCommandText(new object[] { baseClientInfo }, ModuleName, DataSetName);
            if (null != aryRet)
            {
                if (0 == (int)(aryRet[0]))
                    return (string)(aryRet[1]);
                else
                {
                    // MessageBox.Show("GetSqlCommand return error: " + (string)(aryRet[1]));
                    // throw new Exception("GetSqlCommand return error: " + (string)(aryRet[1]));
                    Application_ThreadException(null, new ThreadExceptionEventArgs(new Exception("GetSqlCommandText return error: " + (string)(aryRet[1]))));
                    return null;
                }
            }
            else
                return null;
        }

        public static string GetTableName(string sql)
        {
            return GetTableName(sql, false);
        }

        public static string GetTableName(string sql, bool origian)
        {
            //modify by ccm 2009/04/27 不保留括号
            return DBUtils.GetTableName(sql, origian).Replace("[", string.Empty).Replace("]", string.Empty);
        }

        public static string GetTableName(string ModuleName, string DataSetName, string sCurProject)
        {
            return GetTableName(ModuleName, DataSetName, sCurProject, "");
        }

        public static string GetTableName(string ModuleName, string DataSetName, string sCurProject, string CommandText)
        {
            return GetTableName(ModuleName, DataSetName, sCurProject, CommandText, false);
        }

        public static string GetTableName(string ModuleName, string DataSetName, string sCurProject, string CommandText, bool origian)
        {
            if (string.IsNullOrEmpty(CommandText))
            {
                CheckSession();

                fModuleName = ModuleName;

                object[] baseClientInfo = (object[])GetBaseClientInfo();
                if ((sCurProject != null) && (!sCurProject.Equals("")))
                {
                    baseClientInfo[6] = sCurProject;
                }
                object[] aryRet = RemoteObject.GetSqlCommandText(new object[] { baseClientInfo }, ModuleName, DataSetName);
                if (null != aryRet)
                {
                    if (0 == (int)(aryRet[0]))
                    {
                        CommandText = (string)(aryRet[1]);
                    }
                    else
                    {
                        Application_ThreadException(null, new ThreadExceptionEventArgs(new Exception("GetTableName return error: " + (string)(aryRet[1]))));
                        return string.Empty;
                    }
                }
            }
            return CliUtils.GetTableName(CommandText, origian);
        }

        static public string GetTableNameForColumn(string sqlCommandText, string columnName)
        {
            return " " + DBUtils.GetTableNameForColumn(sqlCommandText, columnName, CliUtils.GetDataBaseType()) + " ";
        }

        //改写
        [Obsolete("The recommended alternative is GetTableNameForColumn(string, string)", false)]
        static public string GetTableNameForColumn(string sqlCommandText, string columnName, string defaultTable, string[] quote)
        {
            return CliUtils.GetTableNameForColumn(sqlCommandText, columnName);
        }

        static public DataSet UpdateDataSet(string sModule, string sSqlCommand, DataSet custDS, string replaceCmd)
        {
            CheckSession();

            fModuleName = sModule;

            object[] aryRet = null;
            try
            {
                aryRet = RemoteObject.UpdateDataSet(new object[] { GetBaseClientInfo(), replaceCmd }, sModule, sSqlCommand, custDS);
            }
            catch (Exception e)
            {
                Application_ThreadException(null, new ThreadExceptionEventArgs(e));
            }

            if (null != aryRet)
            {
                if (0 == (int)(aryRet[0]))
                    return (DataSet)(aryRet[1]);
                else
                {
                    Exception exin = null;
                    if (aryRet.Length == 3 && aryRet[2] != null)
                    {
                        exin = (Exception)aryRet[2];
                    }
                    Exception ex = new Exception((string)(aryRet[1]), exin);
                    throw ex;
                    //Application_ThreadException(null, new ThreadExceptionEventArgs(new Exception((string)(aryRet[1]))));
                    //return null;
                }
            }
            else
                return null;
        }

        //public static int Recover(string sModule, string sSqlCommand, DataSet custDS)
        //{
        //    CheckSession();

        //    fModuleName = sModule;

        //    object[] aryRet = null;
        //    try
        //    {
        //        aryRet = RemoteObject.Recover(new object[] { GetBaseClientInfo() }, sModule, sSqlCommand, custDS.Tables[0]);
        //    }
        //    catch (Exception e)
        //    {
        //        Application_ThreadException(null, new ThreadExceptionEventArgs(e));
        //    }

        //    if (null != aryRet)
        //    {
        //        if (0 == (int)(aryRet[0]))
        //            return (int)(aryRet[1]);
        //        else
        //        {
        //            Exception ex = new Exception((string)(aryRet[1]));
        //            throw ex;
        //        }
        //    }
        //    else
        //        return 0;
        //}

        public static string EncodePassword(string userId, string password)
        {
            char[] p = new char[] { };
            Encrypt.EncryptPassword(userId, password, 10, ref p, false);

            return new string(p);
        }

        public static string DomainCheckSum(string domain)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider provider = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] code = provider.ComputeHash(Encoding.Unicode.GetBytes(domain));
            return BitConverter.ToString(code);
        }

        static public void RefreshServerModule(string sModule)
        {
            CheckSession();

            fModuleName = sModule;

            object[] aryRet = RemoteObject.RefreshServerModule(new object[] { GetBaseClientInfo() }, sModule);
            if (null != aryRet)
            {
                if (0 != (int)(aryRet[0]))
                {
                    // MessageBox.Show((string)(aryRet[1]));
                    // throw new Exception((string)(aryRet[1]));
                    Application_ThreadException(null, new ThreadExceptionEventArgs(new Exception((string)(aryRet[1]))));
                }
            }
        }

        static private string GetListenerObjectUriFromConfig()
        {
            return CliUtils.fServerLoaderURL;
            //string Result = "";
            //WellKnownClientTypeEntry[] ClientEntrys = RemotingConfiguration.GetRegisteredWellKnownClientTypes();
            //foreach (WellKnownClientTypeEntry ClientEntry in ClientEntrys)
            //{
            //    if (string.Compare(ClientEntry.TypeName, "Srvtools.ListenerService") == 0)
            //    {
            //        Result = ClientEntry.ObjectUrl;
            //        break;
            //    }
            //}
            //return Result;
        }

        static public ListenerService EEPListenerService
        {
            get
            {
                try
                {
                    if (FListenerService == null)
                    {
                        string ObjectUri = GetListenerObjectUriFromConfig();
                        FListenerService = Activator.GetObject(typeof(ListenerService), ObjectUri) as ListenerService;
                    }
                }

                //try
                //{
                //    FListenerService.ToString();
                //}
                catch (Exception EE)
                {
                    if (string.Compare(EE.Message.ToLower(), "unable to connect to the remote server") == 0)
                    {
                        throw new Exception("Unable to connect to EEPNetServer Listerner !!");
                    }
                    else
                    {
                        throw new Exception(EE.Message);
                    }
                }
                return FListenerService;
            }
        }

        static public bool PassByEEPListener()
        {
            bool Result = false;
            Result = GetListenerObjectUriFromConfig() != "";
            return Result;
        }

        //所有取值以后都从这里
        public static object GetValue(string expression, object comp)
        {
            object retValue = DBNull.Value;
            if (!string.IsNullOrEmpty(expression))
            {
                object[] obj = CliUtils.GetValue(expression);
                if ((int)obj[0] == 0)
                {
                    return obj[1];
                }

                String[] sps = expression.Split("()".ToCharArray());
                if (sps.Length == 3 && comp != null)
                {
                    Type type = comp.GetType();
                    MethodInfo function = type.GetMethod(sps[0],
                              BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    retValue = function.Invoke(comp, null);
                }
                else if (sps.Length == 1)
                {
                    if (expression.StartsWith("@"))
                    {
                        string[] fields = expression.Trim('@').Split('.');
                        if (fields.Length == 2)
                        {
                            FieldInfo info = comp.GetType().GetField(fields[0], BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                            if (info != null)
                            {
                                InfoBindingSource ibs = info.GetValue(comp) as InfoBindingSource;
                                if (ibs != null && ibs.Current != null)
                                {
                                    retValue = (ibs.Current as DataRowView).Row[fields[1]];
                                }
                            }
                        }
                    }
                    else
                    {
                        retValue = expression;
                    }
                }
                if (retValue == null)//convert null to dbnull
                {
                    retValue = DBNull.Value;
                }
            }
            return retValue;
        }

        static public object[] GetValue(string str)
        {
            string strval = "";

            Char[] cs = str.ToCharArray();
            if (cs.Length == 0)
            {
                strval = "";
                return new object[] { 0, strval };
            }
            if (cs[0].Equals('\\'))
            {
                if (cs.Length > 1)
                {
                    strval = str.Substring(1);
                }
                else
                {
                    strval = "";
                }

                return new object[] { 0, strval };
            }
            else if (cs[0].Equals('_'))
            {
                switch (str.ToLower())
                {
                    case "_usercode": strval = fLoginUser; break;
                    case "_username":
                        {
                            //object[] myRet = CallMethod("GLModule", "GetUserName", new object[] { fLoginUser });
                            //if(myRet != null && (int)myRet[0] == 0)
                            //{
                            //    strval = (string)myRet[1];
                            //}
                            strval = fUserName;
                            break;
                        }
                    case "_solution": strval = fCurrentProject; break;
                    case "_database": strval = fLoginDB; break;
                    case "_sitecode": strval = fSiteCode; break;
                    case "_ipaddress": strval = fComputerIp; break;
                    case "_language": strval = fClientLang.ToString(); break;
                    case "_today": strval = DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day; break;
                    case "_sysdate": strval = DateTime.Now.ToString(); break;
                    case "_servertoday":
                        {
                            object[] myRet = CallMethod("GLModule", "GetServerTime", new object[] { });
                            if (myRet != null && (int)myRet[0] == 0)
                            {
                                strval = (string)myRet[1];
                            }
                            break;
                        }
                    case "_firstday":
                        {
                            int day = DateTime.Now.Day;
                            DateTime retday = DateTime.Now.AddDays(1 - day);
                            strval = retday.ToShortDateString();
                            break;
                        }
                    case "_lastday":
                        {
                            int day = DateTime.Now.Day;
                            DateTime retday = DateTime.Now.AddDays(1 - day);
                            retday = retday.AddMonths(1);
                            retday = retday.AddDays(-1);
                            strval = retday.ToShortDateString();
                            break;
                        }
                    case "_firstdaylm":
                        {
                            int day = DateTime.Now.Day;
                            DateTime retday = DateTime.Now.AddDays(1 - day);
                            retday = retday.AddMonths(-1);
                            strval = retday.ToShortDateString();
                            break;
                        }
                    case "_lastdaylm":
                        {
                            int day = DateTime.Now.Day;
                            DateTime retday = DateTime.Now.AddDays(-day);
                            strval = retday.ToShortDateString();
                            break;
                        }
                    case "_firstdayty":
                        {
                            int year = DateTime.Now.Year;
                            DateTime retday = new DateTime(year, 1, 1);
                            strval = retday.ToShortDateString();
                            break;
                        }
                    case "_lastdayty":
                        {
                            int year = DateTime.Now.Year;
                            DateTime retday = new DateTime(year, 12, 31);
                            strval = retday.ToShortDateString();
                            break;
                        }
                    case "_firstdayly":
                        {
                            int year = DateTime.Now.Year - 1;
                            DateTime retday = new DateTime(year, 1, 1);
                            strval = retday.ToShortDateString();
                            break;
                        }
                    case "_lastdayly":
                        {
                            int year = DateTime.Now.Year - 1;
                            DateTime retday = new DateTime(year, 12, 31);
                            strval = retday.ToShortDateString();
                            break;
                        }
                    default: strval = ""; break;
                }
                return new object[] { 0, strval };
            }
            else
            {
                return new object[] { 1 };
            }
        }

        static public String FormatDateString(String dateString)
        {
            ClientType dbType = GetDataBaseType(CliUtils.fLoginDB);
            OdbcDBType odbcType = GetOdbcDataBaseType(CliUtils.fLoginDB);
            DateTime t = Convert.ToDateTime(dateString);
            switch (dbType)
            {
                case ClientType.ctMsSql:
                    dateString = "'" + dateString + "'";
                    break;
                case ClientType.ctOleDB:
                    dateString = "'" + dateString + "'";
                    break;
                case ClientType.ctOracle:
                    dateString = t.Year.ToString("0000") + "-" + t.Month.ToString("00") + "-" + t.Day.ToString("00") + " "
                        + t.Hour.ToString("00") + ":" + t.Minute.ToString("00") + ":" + t.Second.ToString("00");
                    dateString = "to_date('" + dateString + "', 'yyyy-mm-dd hh24:mi:ss')";
                    break;
                case ClientType.ctODBC:
                    if (odbcType == OdbcDBType.InfoMix)
                    {
                        dateString = t.Year.ToString("0000") + t.Month.ToString("00") + t.Day.ToString("00") + t.Hour.ToString("00") + t.Minute.ToString("00") + t.Second.ToString("00");
                        dateString = "to_date('" + dateString + "', '%Y%m%d%H%M%S')";
                    }
                    break;
                case ClientType.ctMySql:
                    dateString = "'" + t.Year.ToString() + "-" + t.Month.ToString() + "-" + t.Day.ToString() + " "
                        + t.Hour.ToString() + ":" + t.Minute.ToString() + ":" + t.Second.ToString() + "'";
                    break;
                case ClientType.ctInformix:
                    dateString = t.Year.ToString("0000") + t.Month.ToString("00") + t.Day.ToString("00") + t.Hour.ToString("00") + t.Minute.ToString("00") + t.Second.ToString("00");
                    dateString = "to_date('" + dateString + "', '%Y%m%d%H%M%S')";
                    break;
                case ClientType.ctNone:
                    dateString = "'" + dateString + "'";
                    break;
            }
            return dateString;
        }

        static public string[] GetDataBaseQuote()
        {
            return GetDataBaseQuote(CliUtils.fLoginDB);
        }

        static public string[] GetDataBaseQuote(String DBAlias)
        {
            ClientType type = GetDataBaseType(DBAlias);
            switch (type)
            {
                case ClientType.ctMsSql:
                case ClientType.ctOleDB: return new string[] { "[", "]" };
                default: return new string[] { "", "" };
            }
        }

        static public ClientType GetDataBaseType()
        {
            return GetDataBaseType(CliUtils.fLoginDB);
        }

        static public ClientType GetDataBaseType(string DBAlias)
        {
            object[] myRet = CliUtils.CallMethod("GLModule", "GetDataBaseType", new object[] { DBAlias });
            if (myRet != null && (int)myRet[0] == 0)
            {
                int intdb = int.Parse(myRet[1].ToString());
                return (ClientType)intdb;
            }
            else
            {
                return ClientType.ctMsSql;
            }
        }

        static public OdbcDBType GetOdbcDataBaseType(string DBAlias)
        {
            object[] myRet = CliUtils.CallMethod("GLModule", "GetDataBaseType", new object[] { DBAlias });
            if (myRet != null && (int)myRet[0] == 0)
            {
                int intdb = int.Parse(myRet[2].ToString());
                return (OdbcDBType)intdb;
            }
            else
            {
                return OdbcDBType.None;
            }
        }

        static public bool DownLoad(string serverFileName, string clientFileName)
        {
            object[] myRet = CallMethod("GLModule", "DownLoadFile", new object[] { serverFileName });
            if (myRet != null && (int)myRet[0] == 0)
            {
                if ((int)myRet[1] == 0)
                {
                    byte[] bfile = (byte[])myRet[2];
                    string sPath = Path.GetDirectoryName(clientFileName);
                    if (!Directory.Exists(sPath))
                    {
                        Directory.CreateDirectory(sPath);
                    }
                    File.WriteAllBytes(clientFileName, bfile);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        static public bool UpLoad(string clientFileName, string serverFileName)
        {
            if (File.Exists(clientFileName))
            {
                byte[] bfile = File.ReadAllBytes(clientFileName);
                object[] myRet = CallMethod("GLModule", "UpLoadFile", new object[] { serverFileName, bfile });
                if (myRet != null && (int)myRet[0] == 0)
                {
                    if ((int)myRet[1] == 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        static public bool DownLoadModule(string FileName, bool needCheck)
        {
            bool needck = needCheck;
            DateTime dtc = new DateTime();
            string sfile = Application.StartupPath + "\\" + CliUtils.fCurrentProject + "\\" + FileName;
            if (!File.Exists(sfile))
            {
                needck = false;
            }
            else
            {
                dtc = File.GetLastWriteTime(sfile);
            }
            object[] myRet = CallMethod("GLModule", "DownloadModule", new object[] { CliUtils.fCurrentProject, FileName, needck, dtc });
            if (myRet != null && (int)myRet[0] == 0)
            {
                if ((int)myRet[1] == 0)
                {
                    byte[] bfile = (byte[])myRet[2];
                    DateTime dtnew = (DateTime)myRet[3];
                    File.WriteAllBytes(sfile, bfile);
                    File.SetLastWriteTime(sfile, dtnew);
                    return true;
                }
            }
            return false;
        }

        public static void CheckSession()
        {
            if (fClientSystem.ToLower() == "web" && (fLoginUser.Length == 0 || fLoginDB.Length == 0
                || fCurrentProject.Length == 0))
            {
                //if (fClientMainFlow)
                //{
                //    throw new Exception("6A8F8FE2-60B3-4cc3-8C43-FC9FDA58360E");
                //}
                //else
                //{
                throw new Exception("75FF57F7-7AC0-43c8-9454-C92B4A2723BB");
                //}
            }
        }

        public static InfoForm OpenPackageForm(string Package, string Form, string Parameters)
        {
            return OpenPackageForm(Package, Form, Parameters, false);
        }

        public static InfoForm OpenPackageForm(string Package, string Form, string Parameters, bool DialogBox)
        {
            return OpenPackageForm(Package, Form, Parameters, DialogBox, null);
        }

        public static InfoForm OpenPackageForm(string Package, string Form, string Parameters, bool DialogBox, Form MdiParent)
        {
            try
            {
                DownLoadModule(Package + ".dll", true);
                string strPackage = Application.StartupPath + "\\" + CliUtils.fCurrentProject + "\\" + Package + ".dll";
                Assembly ass = Assembly.LoadFrom(strPackage);
                Type assType = ass.GetType(Package + "." + Form);
                if (assType != null)
                {
                    object obj = Activator.CreateInstance(assType);
                    if (obj is InfoForm)
                    {
                        InfoForm ShowForm = (InfoForm)obj;
                        if (MdiParent != null)
                        {
                            ShowForm.MdiParent = MdiParent;
                        }
                        ShowForm.ItemParamters = Parameters;
                        if (DialogBox && MdiParent == null)
                        {
                            ShowForm.ShowDialog();
                        }
                        else
                        {
                            ShowForm.Show();
                        }
                        return ShowForm;
                    }
                    else
                    {
                        throw new Exception("Type should be InfoForm");
                    }
                }
                else
                {
                    throw new Exception(string.Format("Form:{0} does not exsit", Form));
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static string InsertWhere(string sql, string where)
        {
            return DBUtils.InsertWhere(sql, where);
        }

        public static bool Sure(string message)
        {
            return MessageBox.Show(message, "Sure", MessageBoxButtons.YesNo) == DialogResult.Yes;
        }

        public static string GetMenuID(string filename)
        {
            return GetMenuID(filename, null);
        }


        public static string GetMenuID(string filename, string itemparam)
        {
            string package = string.Empty;
            string form = string.Empty;
            string moduletype = "F";
            if (fClientSystem.ToLower().Equals("web"))
            {
                moduletype = "W";
                string[] arr = filename.Split("\\/".ToCharArray());
                if (arr.Length == 1)
                {
                    return string.Empty;
                }
                package = arr[arr.Length - 2];
                //form = arr[arr.Length - 1].Replace(".aspx", string.Empty);
                form = CliUtils.ReplaceFileName(arr[arr.Length - 1], ".aspx", string.Empty);
            }
            else
            {
                string[] arr = filename.Split('.');
                if (arr.Length == 1)
                {
                    return string.Empty;
                }
                package = arr[0];
                form = arr[1];
            }
            object[] myRet = CallMethod("GLModule", "FetchMenus", new object[] { fCurrentProject, moduletype });

            if ((null != myRet) && (0 == (int)myRet[0]))
            {
                DataSet menuDataSet = (DataSet)(myRet[1]);

                string where = string.IsNullOrEmpty(itemparam) ? string.Format("PACKAGE ='{0}' and FORM ='{1}'", package, form)
                    : string.Format("PACKAGE ='{0}' and FORM ='{1}' and ITEMPARAM = '{2}'", package, form, itemparam);

                DataRow[] dr = menuDataSet.Tables[0].Select(where);
                if (dr.Length > 0)
                {
                    return dr[0]["MENUID"].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
            else
            {
                return string.Empty;
            }
        }

        public static bool CheckMenuRightsByName(string filename)
        {
            return !GetMenuID(filename).Equals(string.Empty);
        }

        public static bool CheckMenuRights(string MenuID)
        {
            string moduletype = "F";
            if (fClientSystem.ToLower().Equals("web"))
            {
                moduletype = "W";
            }
            object[] myRet = CallMethod("GLModule", "FetchMenus", new object[] { fCurrentProject, moduletype });

            if ((null != myRet) && (0 == (int)myRet[0]))
            {
                DataSet menuDataSet = (DataSet)(myRet[1]);

                DataRow[] dr = menuDataSet.Tables[0].Select("MENUID = '" + MenuID + "'");
                if (dr.Length > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        #region Register Method
        public static void LoadLoginServiceConfig(string configfile)
        {
            XmlDocument xmlDocC = new XmlDocument();
            xmlDocC.Load(configfile);
            if (xmlDocC != null && xmlDocC.FirstChild != null)
            {
                XmlNode n = xmlDocC.SelectSingleNode("configuration/system.runtime.remoting/application/client/wellknown[@type='Srvtools.LoginService, Srvtools']");
                if (n != null)
                {
                    string url = n.Attributes["url"].InnerText;
                    System.Text.RegularExpressions.MatchCollection mc = System.Text.RegularExpressions.Regex.Matches(url, @"(\S+):");
                    string[] ip = mc[0].ToString().Split("/,:".ToCharArray());
                    CliUtils.fRemoteIP = ip[3];
                    string c = url.Replace(mc[0].ToString(), "");
                    string[] ss = c.Split(@"/".ToCharArray());
                    CliUtils.fRemotePort = Convert.ToInt32(ss[0]);
                }
                XmlNode n1 = xmlDocC.SelectSingleNode("configuration/system.runtime.remoting/application/client/wellknown[@type='Srvtools.ListenerService, Srvtools']");
                if (n1 != null)
                {
                    CliUtils.fServerLoaderURL = n1.Attributes["url"].InnerText;
                }
                XmlNode nodeReserve = xmlDocC.SelectSingleNode("configuration/system.runtime.remoting/application/reserveServer");
                if (nodeReserve != null)
                {
                    string[] reserveServer = nodeReserve.InnerText.Split(':');
                    reserveServerIP = reserveServer[0];
                    if (reserveServer.Length > 1)
                    {
                        try
                        {
                            reserveServerPort = int.Parse(reserveServer[1]);
                        }
                        catch { }
                    }
                }

            }
        }

        public static void RegisterProxy(bool enable, string uri, string user, string password)
        {
            WebRequest.DefaultWebProxy = null;

            if (enable)
            {
                WebProxy proxy = new WebProxy(uri);
                proxy.Credentials = new NetworkCredential(user, password);
                WebRequest.DefaultWebProxy = proxy;
            }
        }

        public static bool Register(ref string showmessage)
        {
            return Register(ref showmessage, true);
        }

        public static bool Register(ref string showmessage, bool loadbalance)
        {
            bool isWeb = string.Compare(fClientSystem, "Web", true) == 0;
            if (RemotingConfiguration.IsWellKnownClientType(typeof(EEPRemoteModule)) != null)
            {
                EEPRemoteModule module = new EEPRemoteModule();
                try
                {
                    module.ToString();
                    return true;
                }
                catch
                {
                    try
                    {
                        //EEPListenerService.StartupEEPNetServer();
                        LoadServer();
                        module.ToString();
                        return true;
                    }
                    catch
                    {
                        try
                        {
                            showmessage = SysMsg.GetSystemMessage(fClientLang, "EEPWebNetClient", "WinSysMsg", "msg_CanNotFindServer", isWeb);
                        }
                        catch
                        {
                            showmessage = "Unable to connect to the remote server";
                        }
                        return false;
                    }
                }
            }

            if (string.Compare(fRemoteIP, "127.0.0.1") != 0 && string.Compare(fRemoteIP, "localhost", true) != 0)
            {
                LoginService loginService = Activator.GetObject(typeof(LoginService),
                        string.Format("http://{0}:{1}/Srvtools.rem", fRemoteIP, fRemotePort)) as LoginService;
            GetLoginService:
                try
                {
                    object[] rtn = loginService.GetServerIP(loadbalance);
                    string newip = rtn[0].ToString();
                    if (newip == null || newip.Trim() == "")
                    {
                        try
                        {
                            showmessage = SysMsg.GetSystemMessage(fClientLang, "EEPWebNetClient", "WinSysMsg", "msg_BusyService", isWeb);
                        }
                        catch
                        {
                            showmessage = "System Busy, try again later";
                        }
                        return false;
                    }
                    else if (newip == "000")
                    {
                        try
                        {
                            if (fClientSystem == "Web")
                            {
                                showmessage = SysMsg.GetSystemMessage(fClientLang, "Web", "InfoLogin", "ExceedsMax", isWeb);
                            }
                            else
                            {
                                showmessage = SysMsg.GetSystemMessage(fClientLang, "EEPNetClient", "FrmClientMain", "ExceedsMax", isWeb);
                            }
                        }
                        catch
                        {
                            showmessage = "The current number of users exceeds the max number set by the AP server!try again?";
                        }
                        return false;
                    }
                    else if (newip != "001")  //has redirect to another server
                    {
                        fRemoteIP = newip;
                        fRemotePort = (int)rtn[1];
                    }
                }
                catch (WebException E)
                {
                    if (E.Status == WebExceptionStatus.ConnectFailure)
                    {
                        if (fRemoteIP != reserveServerIP)
                        {
                            if (LoadServer())
                            {
                                goto GetLoginService;
                            }
                            else if (reserveServerIP.Length > 0)
                            {
                                fRemoteIP = reserveServerIP;
                                fRemotePort = reserveServerPort;
                                return Register(ref showmessage, loadbalance);
                            }
                        }

                        string message = E.Message;
                        if (E.Message == "Unable to connect to the remote server")
                        {
                            try
                            {
                                message = SysMsg.GetSystemMessage(fClientLang, "EEPWebNetClient", "WinSysMsg", "msg_CanNotFindServer", isWeb);
                            }
                            catch
                            {
                                message = "Unable to connect to the remote server";
                            }
                        }
                        showmessage = message;
                        return false;
                    }
                    else
                    {
                        showmessage = E.Message;
                        return false;
                    }
                }

                try
                {
                    EEPRemoteModule module = Activator.GetObject(typeof(EEPRemoteModule),
                        string.Format("http://{0}:{1}/InfoRemoteModule.rem", fRemoteIP, fRemotePort)) as EEPRemoteModule;
                    module.ToString();
                }
                catch
                {
                    loginService.DeRegisterRemoteServer(fRemoteIP, fRemotePort);
                    goto GetLoginService;
                }
            }
            else// local host do not load banlance
            {
                try
                {
                    EEPRemoteModule module = Activator.GetObject(typeof(EEPRemoteModule),
                        string.Format("http://{0}:{1}/InfoRemoteModule.rem", fRemoteIP, fRemotePort)) as EEPRemoteModule;
                    module.ToString();
                }
                catch (Exception e)
                {
                    try
                    {
                        //EEPListenerService.StartupEEPNetServer();
                        LoadServer();
                        EEPRemoteModule module = Activator.GetObject(typeof(EEPRemoteModule),
                         string.Format("http://{0}:{1}/InfoRemoteModule.rem", fRemoteIP, fRemotePort)) as EEPRemoteModule;
                        module.ToString();
                    }
                    catch
                    {
                        string message = e.Message;
                        try
                        {
                            message = SysMsg.GetSystemMessage(fClientLang, "EEPWebNetClient", "WinSysMsg", "msg_CanNotFindServer", isWeb);
                        }
                        catch
                        {
                            message = "Unable to connect to the remote server";
                        }
                        showmessage = message;
                        return false;
                    }
                }
            }

            // Register EEPRemoteModule on the server
            WellKnownClientTypeEntry clientEntry = new WellKnownClientTypeEntry(typeof(EEPRemoteModule),
                string.Format("http://{0}:{1}/InfoRemoteModule.rem", fRemoteIP, fRemotePort));
            RemotingConfiguration.RegisterWellKnownClientType(clientEntry);
            return true;
        }

        public static void GetSysXml(string path)
        {
            DateTime t = new DateTime(1912, 1, 1);
            if (File.Exists(path))
            {
                FileInfo fileInfo = new FileInfo(path);
                t = fileInfo.LastWriteTime;
            }
            object[] ret = CallMethod("GLModule", "GetSysMsgXml", new object[] { t });
            if (ret != null)
            {
                if (ret[0].ToString() == "0" && ret[1].ToString() != "0")
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(ret[1].ToString());
                    try
                    {
                        xmlDoc.Save(path);
                    }
                    catch
                    {

                    }
                }
                else if (ret[0].ToString() == "1")
                {
                    MessageBox.Show(ret[1].ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public static void GetPasswordPolicy()
        {
            object[] ret = CallMethod("GLModule", "GetPasswordPolicy", null);
            if (ret != null && ret[0].ToString() == "0")
            {
                CliUtils.fPassWordMinSize = (int)ret[1];
                CliUtils.fPassWordMaxSize = (int)ret[2];
                CliUtils.fPassWordCharNum = (bool)ret[3];
            }
        }

        public static bool LoadServer()
        {
            ListenerService obj = (ListenerService)Activator.GetObject(typeof(ListenerService), fServerLoaderURL);
            try
            {
                return obj.StartServer();
            }
            catch
            {
                return false;
            }

        }
        #endregion


        public static string ReplaceFileName(string origian, string oldname, string newname)
        {
            string strrtn = Regex.Replace(origian, oldname.Replace(".", @"\."), newname, RegexOptions.IgnoreCase);

            return strrtn;
        }

        private const double MAXTOTAL = 999999999.99;
        public static string SayChineseTotal(double number)
        {
            bool isWeb = string.Compare(fClientSystem, "Web", true) == 0;
            if (number < 0 || number > MAXTOTAL)
            {
                throw new ArgumentOutOfRangeException("number");
            }
            SYS_LANGUAGE language = CliUtils.fClientLang == SYS_LANGUAGE.SIM ? SYS_LANGUAGE.SIM : SYS_LANGUAGE.TRA;

            string[] words = SysMsg.GetSystemMessage(language, "Srvtools", "CliUtils", "TotalString", isWeb).Split(';');
            StringBuilder builder = new StringBuilder();
            StringBuilder partbuilder = new StringBuilder();
            bool zerorequired = false;
            for (int i = 8; i >= -2; i--)
            {
                int bit = GetNumberBit(number, i);

                if (i >= 0)
                {
                    if (bit != 0)
                    {
                        if (zerorequired)
                        {
                            partbuilder.Append(words[0]);
                            zerorequired = false;
                        }
                        partbuilder.Append(words[bit]);
                        if (i % 4 != 0)
                        {
                            partbuilder.Append(words[i % 4 + 9]); //append 拾佰仟   
                        }
                    }
                    else if (builder.Length > 0 || partbuilder.Length > 0)
                    {
                        zerorequired = true;
                    }
                    if (i % 4 == 0)
                    {
                        if (partbuilder.Length > 0)
                        {
                            builder.Append(partbuilder.ToString());
                            partbuilder = new StringBuilder();
                            zerorequired = false;
                            if (i != 0)
                            {
                                builder.Append(words[i / 4 + 12]); //append 億                   
                            }
                        }
                        else if (i == 0)
                        {
                            if (builder.Length == 0)
                            {
                                builder.Append(words[0]);
                            }
                        }
                    }
                }
                else
                {
                    if (bit != 0)
                    {
                        if (zerorequired)
                        {
                            partbuilder.Append(words[0]);
                            zerorequired = false;
                        }
                        partbuilder.Append(words[bit]);
                    }
                    else
                    {
                        zerorequired = true;
                    }
                    if (i == -2)
                    {
                        if (partbuilder.Length > 0)
                        {
                            builder.Append(words[15]);
                            builder.Append(partbuilder.ToString());
                        }
                    }
                }
            }
            return builder.ToString();
        }

        public static string SayTotal(double number)
        {
            if (number < 0 || number > MAXTOTAL)
            {
                throw new ArgumentOutOfRangeException("number");
            }
            string[] personwords = new string[] { "ZERO", "ONE", "TWO", "THREE", "FOUR", "FIVE", "SIX", "SEVEN", "EIGHT", "NINE", " HUNDRED ", " THOUSAND ", " MILLION " };
            string[] teenwords = new string[] { "TEN", "ELEVEN", "TWELVE", "THIRTEEN", "FOURTEEN", "FIFTEEN", "SIXTEEN", "SEVENTEEN", "EIGHTEEN", "NINETEEN " };
            string[] tenwords = new string[] { "ZERO", "TEN", "TWENTY", "THIRTY", "FORTY", "FIFTY", "SIXTY", "SEVENTY", "EIGHTY", "NINETY" };
            StringBuilder builder = new StringBuilder();
            StringBuilder partbuilder = new StringBuilder();
            for (int i = 8; i >= -2; i--)
            {
                int bit = GetNumberBit(number, i);

                if (i >= 0)
                {
                    if (i % 3 == 2)
                    {
                        if (bit != 0)
                        {
                            partbuilder.Append(personwords[bit]);
                            partbuilder.Append(personwords[10]);
                        }
                    }

                    else if (i % 3 == 1)// 十位和一位一起
                    {
                        int bitper = GetNumberBit(number, i - 1);
                        if (bit == 1)
                        {
                            partbuilder.Append(teenwords[bitper]);
                        }
                        else
                        {
                            if (bit != 0)
                            {
                                partbuilder.Append(tenwords[bit]);
                            }
                            if (bit != 0 && bitper != 0)
                            {
                                partbuilder.Append(" ");
                            }
                            if (bitper != 0)
                            {
                                partbuilder.Append(personwords[bitper]);
                            }
                        }
                    }
                    if (i % 3 == 0)
                    {
                        if (partbuilder.Length > 0)
                        {
                            builder.Append(partbuilder.ToString());
                            partbuilder = new StringBuilder();
                            if (i != 0)
                            {
                                builder.Append(personwords[i / 3 + 10]);
                            }
                        }
                    }
                }
                else
                {
                    if (i == -1)
                    {
                        int bitper = GetNumberBit(number, i - 1);
                        if (bit == 1)
                        {
                            partbuilder.Append(teenwords[bitper]);
                        }
                        else
                        {
                            if (bit != 0)
                            {
                                partbuilder.Append(tenwords[bit]);
                            }
                            if (bit != 0 && bitper != 0)
                            {
                                partbuilder.Append(" ");
                            }
                            if (bitper != 0)
                            {
                                partbuilder.Append(personwords[bitper]);
                            }
                        }
                        if (partbuilder.Length > 0)
                        {

                            if (builder.Length > 0)
                            {
                                if (builder[builder.Length - 1] == ' ')
                                {
                                    builder.Remove(builder.Length - 1, 1);
                                }
                                builder.Append(string.Format(" AND CENTS {0}", partbuilder));
                            }
                            else
                            {
                                builder.Append(string.Format("{0} CENTS", partbuilder));
                            }
                        }
                    }
                }
            }
            return builder.ToString();
        }

        private static int GetNumberBit(double number, int bitposition)
        {
            string[] arrnumber = number.ToString().Split('.');
            if (bitposition < 0)
            {
                if (arrnumber.Length == 2)
                {
                    if (-bitposition <= arrnumber[1].Length)
                    {
                        return int.Parse(arrnumber[1][-bitposition - 1].ToString());
                    }
                }
            }
            else
            {
                if (bitposition < arrnumber[0].Length)
                {
                    return int.Parse(arrnumber[0][arrnumber[0].Length - bitposition - 1].ToString());
                }
            }
            return 0;
        }

        public static void RegisterStartupScript(System.Web.UI.Control control, string script)
        {
            if (control == null)
            {
                throw new EEPException(EEPException.ExceptionType.ArgumentNull, typeof(CliUtils), null, "control", null);
            }
            else
            {
#if AjaxTools
                System.Web.UI.Control panel = control.Parent;
                while (panel != null && panel.GetType() != typeof(UpdatePanel))
                {
                    panel = panel.Parent;
                }
                if (panel != null)
                {
                    ScriptManager.RegisterStartupScript(panel as UpdatePanel, control.Page.GetType(), Guid.NewGuid().ToString(), script, true);
                }
                else
                {
#endif
                    control.Page.ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), script, true);
#if AjaxTools
                }
#endif
            }

        }

        public static void LogToSystem(string title, string description)
        {
            LogToSystem(title, description, true, 7);
        }
        public static void LogToSystem(string title, string description, bool isLogToFile, int maxLogDays)
        {
            LogToSystem(title, description, isLogToFile, maxLogDays, 3);
        }
        public static void LogToSystem(string title, string description, bool isLogToFile, int maxLogDays, int logStyle)
        {
            object[] objParam = new object[] { title, description };
            CallMethod("GLModule", "UserDefineLog", objParam);
        }

#if UseCrystalReportDD
        public static ReportDocument GetReportDD(ref ReportDocument CrystalReport, ref InfoDataSet infoDataSet)
        {
            DataSet dsDD = new DataSet();
            string strModuleName = infoDataSet.RemoteName.Substring(0, infoDataSet.RemoteName.IndexOf('.'));
            int flag = 0;
            int k, l, i, j;
            if (infoDataSet.RealDataSet.Relations.Count != 0)
            {
                string strRelationTableName = infoDataSet.RealDataSet.Relations[0].ChildTable.TableName;
                string relationTabName = CliUtils.GetTableName(strModuleName, strRelationTableName, fCurrentProject);
                string strSqlRelation = "select * from COLDEF where TABLE_NAME = '" + relationTabName + "'";
                dsDD = CliUtils.ExecuteSql(strModuleName, strRelationTableName, strSqlRelation, true, fCurrentProject);
                k = infoDataSet.RealDataSet.Relations[0].ChildTable.Columns.Count;
                j = CrystalReport.ReportDefinition.Sections["Section2"].ReportObjects.Count;
                if (dsDD.Tables[0].Rows.Count != 0)
                {
                    for (i = 0; i < j; i++)
                    {
                        for (l = 0; l < k; l++)
                        {
                            string DD;
                            DD = dsDD.Tables[0].Rows[l]["FIELD_NAME"].ToString().ToLower();
                            TextObject textObject = CrystalReport.ReportDefinition.Sections["Section2"].ReportObjects[i] as TextObject;
                            if (textObject != null)
                            {
                                if (DD == textObject.Text.ToString().ToLower())
                                {
                                    textObject.Text = dsDD.Tables[0].Rows[l]["CAPTION"].ToString();
                                }
                            }
                        }
                    }
                    flag = flag + 1;
                }
            }
            string strTableName = infoDataSet.RemoteName.Substring(infoDataSet.RemoteName.IndexOf('.') + 1);
            string tabName = CliUtils.GetTableName(strModuleName, strTableName, fCurrentProject);
            string strSql = "select * from COLDEF where TABLE_NAME = '" + tabName + "'";
            dsDD = CliUtils.ExecuteSql(strModuleName, strTableName, strSql, true, fCurrentProject);
            k = infoDataSet.RealDataSet.Tables[0].Columns.Count;
            j = CrystalReport.ReportDefinition.Sections["Section2"].ReportObjects.Count;
            if (dsDD.Tables[0].Rows.Count != 0)
            {
                for (i = 0; i < j; i++)
                {
                    for (l = 0; l < k; l++)
                    {
                        string DD;
                        DD = dsDD.Tables[0].Rows[l]["FIELD_NAME"].ToString().ToLower();
                        TextObject textObject = CrystalReport.ReportDefinition.Sections["Section2"].ReportObjects[i] as TextObject;
                        if (textObject != null)
                        {
                            if (DD == textObject.Text.ToString().ToLower())
                            {
                                textObject.Text = dsDD.Tables[0].Rows[l]["CAPTION"].ToString();
                            }
                        }

                    }
                }
                flag = flag + 1;
            }
            if (flag == 0)
            {
                MessageBox.Show("DD has not been set up!");
            }
            return CrystalReport;
        }

        public static CrystalReportSource GetReportDD(ref CrystalReportSource crystalReportSource, ref WebDataSource webDataSource)
        {
            DataSet dsDD = new DataSet();
            string strModuleName = webDataSource.RemoteName.Substring(0, webDataSource.RemoteName.IndexOf('.'));
            int flag = 0;
            int k, l, i, j;
            if (webDataSource.InnerDataSet.Relations.Count != 0)
            {
                string strRelationTableName = webDataSource.InnerDataSet.Relations[0].ChildTable.TableName;
                string relationTabName = CliUtils.GetTableName(strModuleName, strRelationTableName, fCurrentProject);
                string strSqlRelation = "select * from COLDEF where TABLE_NAME = '" + relationTabName + "'";
                dsDD = CliUtils.ExecuteSql(strModuleName, strRelationTableName, strSqlRelation, true, fCurrentProject);
                k = webDataSource.InnerDataSet.Relations[0].ChildTable.Columns.Count;
                j = crystalReportSource.ReportDocument.ReportDefinition.Sections["Section2"].ReportObjects.Count;
                if (dsDD.Tables[0].Rows.Count != 0)
                {
                    for (i = 0; i < j; i++)
                    {
                        for (l = 0; l < k; l++)
                        {
                            string DD;
                            DD = dsDD.Tables[0].Rows[l]["FIELD_NAME"].ToString().ToLower();
                            TextObject textObject = crystalReportSource.ReportDocument.ReportDefinition.Sections["Section2"].ReportObjects[i] as TextObject;
                            if (textObject != null)
                            {
                                if (DD == textObject.Text.ToString().ToLower())
                                {
                                    textObject.Text = dsDD.Tables[0].Rows[l]["CAPTION"].ToString();
                                }
                            }
                        }
                    }
                    flag = flag + 1;
                }
            }
            string strTableName = webDataSource.RemoteName.Substring(webDataSource.RemoteName.IndexOf('.') + 1);
            string tabName = CliUtils.GetTableName(strModuleName, strTableName, fCurrentProject);
            string strSql = "select * from COLDEF where TABLE_NAME = '" + tabName + "'";
            dsDD = CliUtils.ExecuteSql(strModuleName, strTableName, strSql, true, fCurrentProject);
            k = webDataSource.InnerDataSet.Tables[0].Columns.Count;
            j = crystalReportSource.ReportDocument.ReportDefinition.Sections["Section2"].ReportObjects.Count;
            if (dsDD.Tables[0].Rows.Count != 0)
            {
                for (i = 0; i < j; i++)
                {
                    for (l = 0; l < k; l++)
                    {
                        string DD;
                        DD = dsDD.Tables[0].Rows[l]["FIELD_NAME"].ToString().ToLower();
                        TextObject textObject = crystalReportSource.ReportDocument.ReportDefinition.Sections["Section2"].ReportObjects[i] as TextObject;
                        if (textObject != null)
                        {
                            if (DD == textObject.Text.ToString().ToLower())
                            {
                                textObject.Text = dsDD.Tables[0].Rows[l]["CAPTION"].ToString();
                            }
                        }
                    }
                }
                flag = flag + 1;
            }
            if (flag == 0)
            {
                MessageBox.Show("DD has not been set up!");
            }
            return crystalReportSource;
         }
#endif

#if VS90

        //public static object[] LinqInsert(string moduleName, string linqCommandName, object[] instances)
        //{
        //    try
        //    {
        //        object[] clientInfo = new object[] { (object[])GetBaseClientInfo(), string.Empty, string.Empty };

        //        // ----------------------------------------------------------------------
        //        // 序列化Entity
        //        List<object[]> serializedEntities = new List<object[]>();
        //        foreach (object entity in instances)
        //        {
        //            Type entityType = entity.GetType();
        //            serializedEntities.Add(new object[] { entityType, EntitySerializer.Serialize(entityType, entity) });
        //        }
        //        // ----------------------------------------------------------------------

        //        object[] objs = (object[])RemoteObject.LinqInsert(clientInfo, moduleName, linqCommandName, serializedEntities.ToArray());

        //        if (objs[0].ToString() == "0")
        //        {
        //            object[] returnSerializedEntities = (object[])objs[1];

        //            // ----------------------------------------------------------------------
        //            // 反序列化Entity
        //            List<object> entities = new List<object>();
        //            foreach (object returnSerializedEntity in returnSerializedEntities)
        //            {
        //                object[] objs1 = (object[])returnSerializedEntity;
        //                Type entityType = (Type)objs1[0];
        //                byte[] bytes = (byte[])objs1[1];

        //                entities.Add(EntitySerializer.Deserialize(entityType, bytes));
        //            }
        //            // ----------------------------------------------------------------------

        //            return entities.ToArray();
        //        }
        //        else
        //        {

        //        }
        //    }
        //    catch (Exception e)
        //    {
        //    }

        //    return null;
        //}

        //// 要在实体类的主键属性上加上IsVersion=true，不然Update会出现Error
        //public static object[] LinqUpdate(string moduleName, string linqCommandName, object[] instances)
        //{
        //    try
        //    {
        //        object[] clientInfo = new object[] { (object[])GetBaseClientInfo(), string.Empty, string.Empty };

        //        // ----------------------------------------------------------------------
        //        // 序列化Entity
        //        List<object[]> serializedEntities = new List<object[]>();
        //        foreach (object temp in instances)
        //        {
        //            object[] os = (object[])temp;

        //            object entity = os[0];
        //            object oldEntity = os[1];

        //            Type entityType = entity.GetType();
        //            serializedEntities.Add(new object[] { entityType, new object[] { EntitySerializer.Serialize(entityType, entity), EntitySerializer.Serialize(entityType, oldEntity) } });
        //        }
        //        // ----------------------------------------------------------------------

        //        object[] objs = (object[])RemoteObject.LinqUpdate(clientInfo, moduleName, linqCommandName, serializedEntities.ToArray());

        //        if (objs[0].ToString() == "0")
        //        {
        //            object[] returnSerializedEntities = (object[])objs[1];

        //            // ----------------------------------------------------------------------
        //            // 反序列化Entity
        //            List<object> entities = new List<object>();
        //            foreach (object returnSerializedEntity in returnSerializedEntities)
        //            {
        //                object[] os = (object[])returnSerializedEntity;

        //                Type entityType = (Type)os[0];
        //                byte[] bytes = (byte[])os[1];

        //                entities.Add(EntitySerializer.Deserialize(entityType, bytes));
        //            }
        //            // ----------------------------------------------------------------------

        //            return entities.ToArray();
        //        }
        //        else
        //        {

        //        }
        //    }
        //    catch (Exception e)
        //    {
        //    }

        //    return null;
        //}

        //public static void LinqDelete(string moduleName, string linqCommandName, object[] instances)
        //{
        //    try
        //    {
        //        object[] clientInfo = new object[] { (object[])GetBaseClientInfo(), string.Empty, string.Empty };

        //        // ----------------------------------------------------------------------
        //        // 序列化Entity
        //        List<object[]> serializedEntities = new List<object[]>();
        //        foreach (object entity in instances)
        //        {
        //            Type entityType = entity.GetType();
        //            serializedEntities.Add(new object[] { entityType, EntitySerializer.Serialize(entityType, entity) });
        //        }
        //        // ----------------------------------------------------------------------

        //        object[] objs = RemoteObject.LinqDelete(clientInfo, moduleName, linqCommandName, serializedEntities.ToArray());

        //        if (objs[0].ToString() == "0")
        //        {
        //        }
        //        else
        //        {

        //        }
        //    }
        //    catch (Exception e)
        //    {
        //    }
        //}

        public static DataSet LinqUpdate(string moduleName, string linqCommandName, DataSet dataSet)
        {
            CheckSession();
            object[] objs = null;
            try
            {
                object[] clientInfo = new object[] { (object[])GetBaseClientInfo(), string.Empty, string.Empty };
                objs = remoteobject.LinqUpdate(clientInfo, moduleName, linqCommandName, dataSet);
            }
            catch (Exception e)
            {
                Application_ThreadException(null, new ThreadExceptionEventArgs(e));
            }

            if (null != objs)
            {
                if ((int)(objs[0]) == 0)
                {
                    return (DataSet)(objs[1]);
                }
                else
                {
                    Exception ex = null;
                    if (objs.Length == 3 && objs[2] != null)
                    {
                        ex = (Exception)objs[2];
                    }

                    Exception ex2 = new Exception((string)(objs[1]), ex);
                    throw ex2;
                }
            }

            return null;
        }

        //// 1、支持select count(*) from blog，resultType为int，而且也是返回IList。
        //// 2、blog表对应Blog实体类
        //// public class Blog
        //// {
        ////     public string Id { get; set; }
        ////     public string Name { get; set; }
        ////     public string Author { get; set; }
        //// }

        //// public class BlogInfo
        //// {
        ////     public string Id { get; set; }
        ////     public string Name { get; set; }
        ////     public string A { get; set; }
        //// }
        //// 支持select id,name,author a from blog，resultType为BlogInfo，Blog->BlogInfo的转换规则为BlogInfo.Id=Blog.Id、BlogInfo.Name=Blog.Name,BlogInfo.A=Blog.Author
        //// 3、BlogInfo可以定义在Client，但是BlogInfo必须有不带参数的构造函数。
        //public static IList LinqExecuteSQL(string moduleName, string linqCommandName, Type resultType, string sql)
        //{
        //    try
        //    {
        //        object[] clientInfo = new object[] { (object[])GetBaseClientInfo(), string.Empty, string.Empty };

        //        object[] objs = (object[])RemoteObject.LinqExecuteSQL(clientInfo, moduleName, linqCommandName, resultType, sql);

        //        if (objs[0].ToString() == "0")
        //        {
        //            object[] serializedEntities = (object[])objs[1];

        //            // ----------------------------------------------------------------------
        //            // 反序列化Entity
        //            List<object> entities = new List<object>();
        //            foreach (object serializedEntity in serializedEntities)
        //            {
        //                object[] objs1 = (object[])serializedEntity;
        //                Type entityType = (Type)objs1[0];
        //                byte[] bytes = (byte[])objs1[1];

        //                entities.Add(EntitySerializer.Deserialize(entityType, bytes));
        //            }
        //            // ----------------------------------------------------------------------

        //            return entities;
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //    }

        //    return null;
        //}

        //public static IList<R> LinqExecuteSQL<R>(string moduleName, string linqCommandName, string sql)
        //{
        //    IList list = LinqExecuteSQL(moduleName, linqCommandName, typeof(R), sql);

        //    List<R> entities = new List<R>();
        //    foreach (object entity in list)
        //    {
        //        entities.Add((R)entity);
        //    }

        //    return entities;
        //}


        //public static IList<E> LinqExecuteQuery<E>(string moduleName, string linqCommandName, string where)
        //{
        //    return LinqExecuteQuery<E, E>(moduleName, linqCommandName, -1, -1, where, string.Empty, string.Empty, string.Empty, string.Empty);
        //}

        //public static IList<R> LinqExecuteQuery<E, R>(string moduleName, string linqCommandName, string where)
        //{
        //    return LinqExecuteQuery<E, R>(moduleName, linqCommandName, -1, -1, where, string.Empty, string.Empty, string.Empty, string.Empty);
        //}

        //// 有select必须要有result
        //public static IList<R> LinqExecuteQuery<E, R>(string moduleName, string linqCommandName, string where, string groupByKey, string groupByElement, string select)
        //{
        //    return LinqExecuteQuery<E, R>( moduleName, linqCommandName, -1, -1, where, string.Empty, groupByKey, groupByElement, select);
        //}

        //public static IList<E> LinqExecuteQuery<E>(string moduleName, string linqCommandName, string where, string orderBy)
        //{
        //    return LinqExecuteQuery<E, E>(moduleName, linqCommandName, -1, -1, where, orderBy, string.Empty, string.Empty, string.Empty);
        //}

        //public static IList<E> LinqExecuteQuery<E>(string moduleName, string linqCommandName, int skip, int take, string where, string orderBy)
        //{
        //    return LinqExecuteQuery<E, E>(moduleName, linqCommandName, skip, take, where, orderBy, string.Empty, string.Empty, string.Empty);
        //}

        //public static IList<R> LinqExecuteQuery<E, R>(string moduleName, string linqCommandName, string where, string orderBy)
        //{
        //    return LinqExecuteQuery<E, R>(moduleName, linqCommandName, -1, -1, where, orderBy, string.Empty, string.Empty, string.Empty);
        //}

        //public static IList<R> LinqExecuteQuery<E, R>(string moduleName, string linqCommandName, int skip, int take, string where, string orderBy)
        //{
        //    return LinqExecuteQuery<E, R>(moduleName, linqCommandName, skip, take, where, orderBy, string.Empty, string.Empty, string.Empty);
        //}

        //public static IList LinqExecuteQuery(string moduleName, string linqCommandName, Type entityType, string where)
        //{
        //    return LinqExecuteQuery(moduleName, linqCommandName, entityType, entityType, -1, -1, where, string.Empty, string.Empty, string.Empty, string.Empty);
        //}

        //public static IList LinqExecuteQuery(string moduleName, string linqCommandName, Type entityType, Type resultType, string where)
        //{
        //    return LinqExecuteQuery(moduleName, linqCommandName, entityType, resultType, -1, -1, where, string.Empty, string.Empty, string.Empty, string.Empty);
        //}

        //// 有select必须要有result
        //public static IList LinqExecuteQuery(string moduleName, string linqCommandName, Type entityType, Type resultType, string where, string groupByKey, string groupByElement, string select)
        //{
        //    return LinqExecuteQuery(moduleName, linqCommandName, entityType, resultType, -1, -1, where, string.Empty, groupByKey, groupByElement, select);
        //}

        //public static IList LinqExecuteQuery(string moduleName, string linqCommandName, Type entityType, string where, string orderBy)
        //{
        //    return LinqExecuteQuery(moduleName, linqCommandName, entityType, entityType, -1, -1, where, orderBy, string.Empty, string.Empty, string.Empty);
        //}

        //public static IList LinqExecuteQuery(string moduleName, string linqCommandName, Type entityType, Type resultType, string where, string orderBy)
        //{
        //    return LinqExecuteQuery(moduleName, linqCommandName, entityType, resultType, -1, -1, where, orderBy, string.Empty, string.Empty, string.Empty);
        //}

        //public static IList<R> LinqExecuteQuery<E, R>(string moduleName, string linqCommandName, int skip, int take, string where, string orderBy, string groupByKey, string groupByElement, string select)
        //{
        //    IList list = LinqExecuteQuery(moduleName, linqCommandName, typeof(E), typeof(R), skip, take, where, orderBy, groupByKey, groupByElement, select);

        //    List<R> entities = new List<R>();
        //    foreach (object entity in list)
        //    {
        //        entities.Add((R)entity);
        //    }

        //    return entities;
        //}

        //// 1、blog表对应Blog实体类
        //// public class Blog
        //// {
        ////     public string Id { get; set; }
        ////     public string Name { get; set; }
        ////     public string Author { get; set; }
        //// }

        //// public class BlogInfo
        //// {
        ////     public string Id { get; set; }
        ////     public string Name { get; set; }
        ////     public string A { get; set; }
        //// }
        //// 支持select id,name,author a from blog，resultType为BlogInfo，Blog->BlogInfo的转换规则为BlogInfo.Id=Blog.Id、BlogInfo.Name=Blog.Name,BlogInfo.A=Blog.Author
        //// 2、BlogInfo可以定义在Client，但是BlogInfo必须有不带参数的构造函数。
        //// 3、GroupBy单Column
        //// ExecuteQuery("ServerComp", "linqCommand1", Type entityType, Type resultType, "", string.Empty, "Name", "Author", "new (Key as Name)");
        //// 4、GroupBy多Column
        //// ExecuteQuery("ServerComp", "linqCommand1", Type entityType, Type resultType, "", string.Empty, "new (Name, Author)", "new (Name, Author)", "new (Key.Name as Name, Key.Author as Author)");

        //public static IList LinqExecuteQuery(string moduleName, string linqCommandName, Type entityType, Type resultType, int skip, int take, string where, string orderBy, string groupByKey, string groupByElement, string select)
        //{
        //    try
        //    {
        //        object[] clientInfo = new object[] { (object[])GetBaseClientInfo(), string.Empty, string.Empty };

        //        object[] objs = (object[])RemoteObject.LinqExecuteQuery(clientInfo, moduleName, linqCommandName, entityType, resultType, skip, take, where, orderBy, groupByKey, groupByElement, select);

        //        if (objs[0].ToString() == "0")
        //        {
        //            object[] serializedEntities = (object[])objs[1];

        //            // ----------------------------------------------------------------------
        //            // 反序列化Entity
        //            List<object> entities = new List<object>();
        //            foreach (object serializedEntity in serializedEntities)
        //            {
        //                byte[] bytes = (byte[])serializedEntity;

        //                entities.Add(EntitySerializer.Deserialize(resultType, bytes));
        //            }
        //            // ----------------------------------------------------------------------

        //            return entities;
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //    }

        //    return null;
        //}

        public static DataSet LinqExecuteSQL(string moduleName, string linqCommandName, string entityType, string sql)
        {
            try
            {
                object[] clientInfo = new object[] { (object[])GetBaseClientInfo(), string.Empty, string.Empty };

                object[] objs = (object[])RemoteObject.LinqExecuteSQL(clientInfo, moduleName, linqCommandName, sql);

                if (objs[0].ToString() == "0")
                {
                    DataSet dataSet = (DataSet)objs[1];

                    return dataSet;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
            }

            return null;
        }

        public static DataSet LinqExecuteQuery(string moduleName, string linqCommandName, string projectName, int skip, int take, string where, string orderBy, string groupByKey, string groupByElement, string select)
        {
            try
            {
                object[] baseClientInfo = (object[])GetBaseClientInfo();
                if ((projectName != null) && (!projectName.Equals("")))
                {
                    baseClientInfo[6] = projectName;
                }

                object[] clientInfo = new object[] { baseClientInfo, string.Empty, string.Empty };

                object[] objs = (object[])RemoteObject.LinqExecuteQuery(clientInfo, moduleName, linqCommandName, skip, take, where, orderBy, groupByKey, groupByElement, select);

                if (objs[0].ToString() == "0")
                {
                    DataSet dataSet = (DataSet)objs[1];

                    return dataSet;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {

            }

            return null;
        }

        public static DataSet LinqExecuteQuery(string moduleName, string linqCommandName, string projectName)
        {
            return LinqExecuteQuery(moduleName, linqCommandName, projectName, -1, -1, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
        }

        public static DataSet LinqExecuteQuery(string moduleName, string linqCommandName)
        {
            return LinqExecuteQuery(moduleName, linqCommandName, string.Empty, -1, -1, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
        }

        public static DataSet LinqExecuteQuery(string moduleName, string linqCommandName, int skip, int take)
        {
            return LinqExecuteQuery(moduleName, linqCommandName, string.Empty, skip, take, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
        }

#endif

        public static void SetASPxGridViewCommandText(object aspxGridView)
        {
            string[] ControlTexts = SysMsg.GetSystemMessage(fClientLang, "Srvtools", "WebNavigator", "ControlText").Split(new char[] { ';' });
            string sureDeleteText = SysMsg.GetSystemMessage(fClientLang, "Srvtools", "WebNavigator", "sureDeleteText");
            string[] ButtonNames = SysMsg.GetSystemMessage(fClientLang, "Srvtools", "WebNavigator", "ButtonName").Split(new char[] { ';' });

            var settingsText = aspxGridView.GetType().GetProperty("SettingsText").GetValue(aspxGridView, null);
            settingsText.GetType().GetProperty("CommandNew").SetValue(settingsText, ControlTexts[4], null);
            settingsText.GetType().GetProperty("CommandEdit").SetValue(settingsText, ControlTexts[5], null);
            settingsText.GetType().GetProperty("CommandDelete").SetValue(settingsText, ControlTexts[6], null);
            settingsText.GetType().GetProperty("ConfirmDelete").SetValue(settingsText, sureDeleteText, null);
            settingsText.GetType().GetProperty("CommandUpdate").SetValue(settingsText, ButtonNames[0], null);
            settingsText.GetType().GetProperty("CommandCancel").SetValue(settingsText, ButtonNames[1], null);

        }
    }

    public class ScreenCapture
    {
        public Image CaptureScreen()
        {
            return CaptureWindow(User32.GetDesktopWindow());
        }

        public Image CaptureWindow(IntPtr handle)
        {
            // get te hDC of the target window 
            IntPtr hdcSrc = User32.GetWindowDC(handle);
            // get the size 
            User32.RECT windowRect = new User32.RECT();
            User32.GetWindowRect(handle, ref windowRect);
            int width = windowRect.right - windowRect.left;
            int height = windowRect.bottom - windowRect.top;
            // create a device context we can copy to 
            IntPtr hdcDest = GDI32.CreateCompatibleDC(hdcSrc);
            // create a bitmap we can copy it to, 
            // using GetDeviceCaps to get the width/height 
            IntPtr hBitmap = GDI32.CreateCompatibleBitmap(hdcSrc, width, height);
            // select the bitmap object 
            IntPtr hOld = GDI32.SelectObject(hdcDest, hBitmap);
            // bitblt over 
            GDI32.BitBlt(hdcDest, 0, 0, width, height, hdcSrc, 0, 0, GDI32.SRCCOPY);
            // restore selection 
            GDI32.SelectObject(hdcDest, hOld);
            // clean up 
            GDI32.DeleteDC(hdcDest);
            User32.ReleaseDC(handle, hdcSrc);

            // get a .NET image object for it 
            Image img = Image.FromHbitmap(hBitmap);
            // free up the Bitmap object 
            GDI32.DeleteObject(hBitmap);

            return img;
        }
        public MemoryStream CaptureScreenToStream(ImageFormat format)
        {
            Image img = CaptureScreen();
            MemoryStream ms = new MemoryStream();
            img.Save(ms, format);
            ms.Flush();
            return ms;
        }
        public void CaptureWindowToFile(IntPtr handle, string filename, ImageFormat format)
        {
            Image img = CaptureWindow(handle);
            img.Save(filename, format);
        }

        public void CaptureScreenToFile(string filename, ImageFormat format)
        {
            Image img = CaptureScreen();
            img.Save(filename, format);
        }

        private class GDI32
        {
            public const int SRCCOPY = 0x00CC0020;

            [DllImport("gdi32.dll")]
            public static extern bool BitBlt(IntPtr hObject, int nXDest, int nYDest,
            int nWidth, int nHeight, IntPtr hObjectSource,
            int nXSrc, int nYSrc, int dwRop);
            [DllImport("gdi32.dll")]
            public static extern IntPtr CreateCompatibleBitmap(IntPtr hDC, int nWidth,
            int nHeight);
            [DllImport("gdi32.dll")]
            public static extern IntPtr CreateCompatibleDC(IntPtr hDC);
            [DllImport("gdi32.dll")]
            public static extern bool DeleteDC(IntPtr hDC);
            [DllImport("gdi32.dll")]
            public static extern bool DeleteObject(IntPtr hObject);
            [DllImport("gdi32.dll")]
            public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);
        }

        private class User32
        {
            [StructLayout(LayoutKind.Sequential)]
            public struct RECT
            {
                public int left;
                public int top;
                public int right;
                public int bottom;
            }

            [DllImport("user32.dll")]
            public static extern IntPtr GetDesktopWindow();
            [DllImport("user32.dll")]
            public static extern IntPtr GetWindowDC(IntPtr hWnd);
            [DllImport("user32.dll")]
            public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDC);
            [DllImport("user32.dll")]
            public static extern IntPtr GetWindowRect(IntPtr hWnd, ref RECT rect);
            [DllImport("user32.dll")]
            public static extern void InvalidateRect(IntPtr hWnd, RECT rect, bool b);

        }


    }

    public static class DllContainer
    {
        public static ArrayList NameList = new ArrayList();

        public static bool DllLoaded(string sName)
        {
            bool bRet = false;
            for (int i = 0; i < NameList.Count; i++)
            {
                if (((string)NameList[i]).Trim().ToUpper().Equals(sName.Trim().ToUpper()))
                {
                    bRet = true;
                    break;
                }
            }

            return bRet;
        }

        public static void AddDll(string sName)
        {
            NameList.Add(sName);
        }
    }

    public class FormItem : InfoOwnerCollectionItem
    {
        private string fPackageName = "";
        public string PackageName
        {
            get
            {
                return fPackageName;
            }
            set
            {
                fPackageName = value;
            }
        }

        private string fFormName = "";
        public string FormName
        {
            get
            {
                return fFormName;
            }
            set
            {
                fFormName = value;
            }
        }

        public override string Name
        {
            get
            {
                return fPackageName + "." + fFormName;
            }
            set
            {
                int iPos = value.IndexOf('.');
                if (-1 != iPos)
                {
                    fPackageName = value.Substring(0, iPos);
                    fFormName = value.Substring(iPos + 1);
                }
            }
        }

        private bool fMultiInstance = false;
        public bool MultiInstance
        {
            get
            {
                return fMultiInstance;
            }
            set
            {
                fMultiInstance = value;
            }
        }

        private int fCount = 0;
        public int Count
        {
            get
            {
                return fCount;
            }
            set
            {
                fCount = value;
            }
        }
    }

    public class FormCollection : InfoOwnerCollection
    {
        public FormCollection(Component aOwner, Type aItemType)
            : base(aOwner, typeof(FormItem))
        {

        }

        new public FormItem this[int index]
        {
            get
            {
                return (FormItem)InnerList[index];
            }
            set
            {
                if (index > -1 && index < Count)
                    if (value is FormItem)
                    {
                        //原来的Collection设置为0
                        ((FormItem)InnerList[index]).Collection = null;
                        InnerList[index] = value;
                        //Collection设置为this
                        ((FormItem)InnerList[index]).Collection = this;
                    }
            }
        }

        public FormItem FindFormItem(string pkg, string frm)
        {
            FormItem fRet = null;
            foreach (FormItem f in this)
            {
                if (f.PackageName.ToUpper().Equals(pkg.ToUpper()) && f.FormName.ToUpper().Equals(frm.ToUpper()))
                {
                    fRet = f;
                    break;
                }
            }
            return fRet;
        }

        public FormItem AddPackageForm(string pkg, string frm)
        {
            FormItem fRet = FindFormItem(pkg, frm);

            if (null == fRet)
            {
                fRet = new FormItem();
                fRet.PackageName = pkg; fRet.FormName = frm;
                this.Add(fRet);
            }

            fRet.Count++;
            return fRet;
        }

        public void RemovePackageForm(string pkg, string frm)
        {
            FormItem fRet = FindFormItem(pkg, frm);

            if (null != fRet)
            {
                fRet.Count--;
                if (fRet.Count == 0)
                    this.Remove(fRet);
            }
        }
    }

}
