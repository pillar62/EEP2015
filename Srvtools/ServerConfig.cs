using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Net;
using System.ComponentModel;
using Microsoft.Win32;
using System.Reflection;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;

namespace Srvtools
{
    #region ServerConfig
    public class ServerConfig
    {
        // Used for Create Component in another thread 2005-12-22
        //public class ComponnetToCreate
        //{
        //    public Type ComponentType;
        //    public string ComponentName;
        //}

        //public static System.Collections.Generic.List<ComponnetToCreate> ComponnetToCreateList =
        //    new System.Collections.Generic.List<ComponnetToCreate>();
        // End Used

        static ServerConfig()
        {
            m_remoteServers = new ArrayList();

            LoadServerConfig();

            // Add By Chenjian 2005-12-19
            // For Package Transfer
            LoadPackageTransferList();
            // End Add
            LoadDomainConfig();

            LoadUserTableConfig();
            LoadPasswordPolicy();
        }

        public static void LoadDomainConfig()
        {
            string s = SystemFile.DomainFile;
            if (File.Exists(s))
            {
                XmlDocument DomainXml = new XmlDocument();
                try
                {
                    DomainXml.Load(s);
                    var nodes = DomainXml.SelectNodes("Infolight/Domain");
                    Domains.Clear();
                    foreach (XmlNode node in nodes)
                    {
                        Domains.Add(new Domain() { Path = node.Attributes["Path"].Value, User = node.Attributes["User"].Value, Password = Decrypt(node.Attributes["User"].Value, node.Attributes["Password"].Value )});
                    }
                }
                catch
                {
                    File.Delete(s);
                    LoadDomainConfig();
                }
            }
            else
            {
                XmlTextWriter xw = new XmlTextWriter(s, Encoding.Unicode);
                xw.WriteStartElement("Infolight");
                
                xw.WriteEndElement();
                xw.Close();
            }
        }
        public static string Encrypt(string user, string password)
        {
            string rtnpassword = "";
            if (password != "")
            {
                char[] charpwd = password.ToCharArray();
                char[] charuser = user.ToCharArray();
                char[] charrtnpwd = new char[charpwd.Length];
                for (int i = 0; i < charpwd.Length; i++)
                {
                    if (i % 2 == 0)
                    {
                        charrtnpwd[charrtnpwd.Length - 1 - i] = (char)(((uint)charpwd[i] + (uint)charuser[i % charuser.Length]) % 128);
                    }
                    else
                    {
                        charrtnpwd[charrtnpwd.Length - 1 - i] = (char)(((uint)charpwd[i] - (uint)charuser[i % charuser.Length]) % 128);
                    }
                }
                rtnpassword = new string(charrtnpwd);
            }
            return rtnpassword;
        }

        public static string Decrypt(string user, string password)
        {
            string rtnpassword = "";
            if (password != "")
            {
                char[] charpwd = password.ToCharArray();
                char[] charuser = user.ToCharArray();
                char[] charrtnpwd = new char[charpwd.Length];
                for (int i = 0; i < charpwd.Length; i++)
                {
                    if (i % 2 == 0)
                    {
                        charrtnpwd[i] = (char)(((uint)charpwd[charrtnpwd.Length - 1 - i] - (uint)charuser[i % charuser.Length]) % 128);
                    }
                    else
                    {
                        charrtnpwd[i] = (char)(((uint)charpwd[charrtnpwd.Length - 1 - i] + (uint)charuser[i % charuser.Length]) % 128);
                    }
                }
                rtnpassword = new string(charrtnpwd);
            }
            return rtnpassword;
        }

        private static void LoadUserTableConfig()
        {
            string file = SystemFile.UserTableFile;
            XmlDocument xml = new XmlDocument();
            try
            {
                xml.Load(file);
                UserTable = xml.SelectSingleNode("UserDefination/UserTable").InnerText;
                UserID = xml.SelectSingleNode("UserDefination/UserID").InnerText;
                UserName = xml.SelectSingleNode("UserDefination/UserName").InnerText;
                Password = xml.SelectSingleNode("UserDefination/Password").InnerText;
                UserDefination = true;
            }
            catch
            {
                UserDefination = false;
            }
        }

        private static void LoadPasswordPolicy()
        {
            string file = SystemFile.LoginFile;
            XmlDocument xml = new XmlDocument();
            try
            {
                xml.Load(file);

                XmlNode node = xml.SelectSingleNode("InfolightAllowUserToPerLogin/PasswordPolicy");
                if(node != null)
                {
                    ServerConfig.PasswordCharNum = Convert.ToBoolean(node.Attributes["CharNum"].Value);
                    ServerConfig.PassWordMinSize = Convert.ToInt32(node.Attributes["MinSize"].Value);
                    ServerConfig.PasswordMaxSize = Convert.ToInt32(node.Attributes["MaxSize"].Value);
                    ServerConfig.PassWrodExpiry = node.Attributes["PassWrodExpiry"].Value != null ? Convert.ToInt32(node.Attributes["PassWrodExpiry"].Value) : 0;
                }
            }
            catch { }

        }

        public static void SaveUserTableConfig(bool userdefination, string usertable, string userid, string username, string password)
        {
            string file = SystemFile.UserTableFile;
            if (userdefination)
            {
                XmlDocument xml = new XmlDocument();
                xml.AppendChild(xml.CreateElement("UserDefination"));
                XmlNode nodeusertable = xml.CreateElement("UserTable");
                nodeusertable.InnerText = usertable;
                xml.DocumentElement.AppendChild(nodeusertable);

                XmlNode nodeuserid = xml.CreateElement("UserID");
                nodeuserid.InnerText = userid;
                xml.DocumentElement.AppendChild(nodeuserid);

                XmlNode nodeusername = xml.CreateElement("UserName");
                nodeusername.InnerText = username;
                xml.DocumentElement.AppendChild(nodeusername);

                XmlNode nodepassword = xml.CreateElement("Password");
                nodepassword.InnerText = password;
                xml.DocumentElement.AppendChild(nodepassword);

                xml.Save(file);
            }
            else if (System.IO.File.Exists(file))
            {
                File.Delete(file);
            }
            UserDefination = userdefination;
            UserTable = usertable;
            UserID = userid;
            UserName = username;
            Password = password;
        }

        // Add By Chenjian 2005-12-19
        // For Package Transfer
        private static void LoadPackageTransferList()
        {
            m_packageTransferList = new DataSet();

            DataTable table = new DataTable();
            ServerConfig.PackageTransferList.Tables.Add(table);

            DataColumn colIp = new DataColumn();
            colIp.ColumnName = "IP";
            table.Columns.Add(colIp);

            DataColumn colFileName = new DataColumn();
            colFileName.ColumnName = "FileName";
            table.Columns.Add(colFileName);

            DataColumn colSolutionName = new DataColumn();
            colSolutionName.ColumnName = "SolutionName";
            table.Columns.Add(colSolutionName);

            DataColumn colPackageType = new DataColumn();
            colPackageType.ColumnName = "PackageType";
            table.Columns.Add(colPackageType);

            DataColumn colDateTime = new DataColumn();
            colDateTime.ColumnName = "DateTime";
            table.Columns.Add(colDateTime);

            DataColumn colTransferInfo = new DataColumn();
            colTransferInfo.ColumnName = "TransferInfo";
            table.Columns.Add(colTransferInfo);

            string s = SystemFile.PackageTransferListFile;
            XmlDocument PackageTransferXml = new XmlDocument();
            if (File.Exists(s))
            {
                try
                {
                    PackageTransferXml.Load(s);
                    if (string.Compare(PackageTransferXml.DocumentElement.Attributes["AutoTransfer"].InnerText, "true", true) == 0)//IgnoreCase
                    {
                        ServerConfig.AutoTransferPackage = true;
                    }
                    else
                    {
                        ServerConfig.AutoTransferPackage = false;
                    }

                    XmlNode aNode = PackageTransferXml.DocumentElement.FirstChild;
                    while (aNode != null)
                    {
                        DataRow row = table.NewRow();

                        row["IP"] = aNode.Attributes["IP"].InnerText;
                        //兼容旧版本文件
                        if (aNode.Attributes["FileName"] == null)
                        {
                            row["FileName"] = string.Format("{0}.dll", aNode.Attributes["PackageName"].InnerText);
                        }
                        else
                        {
                            row["FileName"] = aNode.Attributes["FileName"].InnerText;
                        }
                       
                        row["SolutionName"] = aNode.Attributes["SolutionName"].InnerText;
                        row["PackageType"] = aNode.Attributes["PackageType"].InnerText;
                        row["DateTime"] = aNode.Attributes["DateTime"].InnerText;
                        row["TransferInfo"] = aNode.Attributes["TransferInfo"].InnerText;

                        table.Rows.Add(row);

                        aNode = aNode.NextSibling;
                    }
                }
                catch
                {
                }
            }
        }

        internal static void AddPackageTransferListItem(string ipAddress, string fileName, string solutionName, PackageType packageType, string transferInfo)
        {
            try
            {
                DataRow[] rows = ServerConfig.PackageTransferList.Tables[0].Select(
                    string.Format("IP='{0}' and FileName='{1}' and SolutionName='{2}' and PackageType='{3}'", ipAddress, fileName, solutionName, packageType.ToString()));
                if (rows.GetLength(0) == 0)
                {
                    DataRow row = ServerConfig.PackageTransferList.Tables[0].NewRow();

                    row["IP"] = ipAddress;
                    row["FileName"] = fileName;
                    row["SolutionName"] = solutionName;
                    row["PackageType"] = packageType.ToString();
                    row["DateTime"] = DateTime.Now.ToString();
                    row["TransferInfo"] = transferInfo;

                    ServerConfig.PackageTransferList.Tables[0].Rows.Add(row);
                }
                else
                {
                    rows[0]["DateTime"] = DateTime.Now.ToString();
                    rows[0]["TransferInfo"] = transferInfo;
                }
            }
            catch //(Exception expt)
            {
                //System.Windows.Forms.MessageBox.Show(expt.Message);
            }
        }

        internal static void RemovePackageTransferListItem(string ipAddress, string fileName, string solutionName, PackageType packageType)
        {
            try
            {
                DataRow[] rows = ServerConfig.PackageTransferList.Tables[0].Select(
                    string.Format("IP='{0}' and FileName='{1}' and SolutionName='{2}' and PackageType='{3}'", ipAddress, fileName, solutionName, packageType.ToString()));
                if (rows.GetLength(0) != 0)
                {
                    ServerConfig.PackageTransferList.Tables[0].Rows.Remove(rows[0]);
                }
            }
            catch //(Exception expt)
            {
                //System.Windows.Forms.MessageBox.Show(expt.Message);
            }
        }
        // End Add

        private static void Refresh()
        {
            RemoteServers.Clear();
            LoadServerConfig();
        }

        private static void LoadServerConfig()
        {
            string s = SystemFile.ServerConfigFile;
            XmlDocument CfgXml = new XmlDocument();
            if (File.Exists(s))
            {
                CfgXml.Load(s);
                XmlNode aNode = CfgXml.DocumentElement.FirstChild;
                while (aNode != null)
                {
                    string serverType = aNode.LocalName;
                    if (string.Compare(serverType, "ssotimeout", true) == 0)//IgnoreCase
                    {
                        SSOTimeOut = int.Parse(aNode.Attributes["Value"].InnerText);
                    }

                    if (string.Compare(serverType, "ssokey", true) == 0)//IgnoreCase
                    {
                        SSOKey = aNode.Attributes["Value"].InnerText;
                    }

                    if (string.Compare(serverType, "recordlockindatabase", true) == 0)//IgnoreCase
                    {
                        RecordLockInDatabase = string.Compare(aNode.Attributes["Value"].InnerText, bool.TrueString, true) == 0;
                    }

                    if (string.Compare(serverType, "localserver", true) == 0)//IgnoreCase
                    {
                        MaxUser = int.Parse(aNode.Attributes["MaxUser"].InnerText);
                        if (string.Compare(aNode.Attributes["IsMaster"].InnerText, "true", true) == 0)//IgnoreCase
                        {
                            IsMaster = true;
                        }
                        else
                        {
                            IsMaster = false;
                        }
                        MasterServerIP = aNode.Attributes["MasterServerIP"].InnerText;
                        if (aNode.Attributes["MasterServerKey"] != null)
                        {
                            MasterServerKey = aNode.Attributes["MasterServerKey"].InnerText;
                        }

                        SrvGL.MasterServerAddress = MasterServerIP;
                        SrvGL.ServerKey = MasterServerKey;
                    }
                    else if (string.Compare(serverType, "remoteserver", true) == 0)//IgnoreCase
                    {
                        string ipAddress = aNode.Attributes["IpAddress"].InnerText;
                        int port = 8989;
                        if (aNode.Attributes["Port"] != null && aNode.Attributes["Port"].InnerText != "")
                        {
                            port = int.Parse(aNode.Attributes["Port"].InnerText);
                        }
                        RemoteServer rs = new RemoteServer(0, ipAddress, port, false);

                        RemoteServers.Add(rs);
                    }

                    aNode = aNode.NextSibling;
                }
            }
        }

        public static bool RegisterRemoteServer(string ipAddress, int port)
        {
            foreach (RemoteServer rs in ServerConfig.RemoteServers)
            {
                if (rs.IpAddress == ipAddress && rs.Port == port)
                {
                    rs.Activated = true;

                    //传送那些未成功的package
                    if (ServerConfig.AutoTransferPackage)
                    {
                        DataRow[] rows = ServerConfig.PackageTransferList.Tables[0].Select("IP='" + ipAddress + "'");

                        PackageService packageLocalService = new PackageService();
                        PackageService packageService = Activator.GetObject(typeof(PackageService),
                            string.Format("http://{0}:{1}/PackageService.rem", ipAddress, port)) as PackageService;

                        foreach (DataRow row in rows)
                        {
                            string fileName = row["FileName"].ToString();
                            string solutionName = row["SolutionName"].ToString();
                            PackageType packageType = (PackageType)Enum.Parse(typeof(PackageType), row["PackageType"].ToString(), true);


                            byte[] buffer = null;
                            DateTime dt = new DateTime();
                            if (packageLocalService.GetTransferFile(fileName, solutionName, packageType, out buffer, out dt))
                            {
                                try
                                {
                                    packageService.Upload(fileName, solutionName, packageType, buffer, dt);
                                    ServerConfig.RemovePackageTransferListItem(ipAddress, fileName, solutionName, packageType);
                                }
                                catch { }
                            }
                            else
                            {
                                //dispatch error
                            }
                        }
                    }
                    return true;
                }
            }

            return false;
        }

        public static void DeRegisterRemoteServer(string ipAddress, int port)
        {
            foreach (RemoteServer rs in ServerConfig.RemoteServers)
            {
                if (rs.IpAddress == ipAddress && rs.Port == port)
                {
                    rs.Activated = false;

                    return;
                }
            }
        }

        //private static string _DomainPath;
        //public static string DomainPath
        //{
        //    get { return _DomainPath; }
        //    set { _DomainPath = value; }
        //}

        //private static string _DomianUser;
        //public static string DomainUser
        //{
        //    get { return _DomianUser; }
        //    set { _DomianUser = value; }
        //}

        //private static string _DomainPassword;
        //public static string DomainPassword
        //{
        //    get { return _DomainPassword; }
        //    set { _DomainPassword = value; }
        //}
        private static List<Domain> domains = new List<Domain>();
        public static List<Domain> Domains
        {
            get {
                return domains;
            }
        }

	
        private static bool _UserDefination;

        public static bool UserDefination
        {
            get { return _UserDefination; }
            set { _UserDefination = value; }
        }

        private static string _UserTable;

        public static string UserTable
        {
            get { return _UserTable; }
            set { _UserTable = value; }
        }

        private static string _UserID;

        public static string UserID
        {
            get { return _UserID; }
            set { _UserID = value; }
        }

        private static string _UserName;

        public static string UserName
        {
            get { return _UserName; }
            set { _UserName = value; }
        }

        private static string _Password;

        public static string Password
        {
            get { return _Password; }
            set { _Password = value; }
        }

        public static bool LoginObjectEnabled
        {
            get { return LoginObject == null ? false : LoginObject.Enabled; }
        }

        private static ILogin loginObject;

        public static ILogin LoginObject
        {
            get
            {
                if (loginObject == null)
                {
                    //create
                    try
                    {
                        Assembly assembly = Assembly.GetEntryAssembly();
                        if (assembly == null)
                        {
                            string serverpath = Path.Combine(EEPRegistry.Server, "EEPNetServer.exe");
                            if (File.Exists(serverpath))
                            {
                                byte[] buffer = File.ReadAllBytes(serverpath);
                                assembly = Assembly.Load(buffer);
                            }
                            else
                            {
                                throw new FileNotFoundException("File not found", serverpath);
                            }
                        }

                        foreach (Type type in assembly.GetTypes())
                        {
                            if (type.GetInterface(typeof(ILogin).Name) != null)
                            {
                                loginObject = (ILogin)Activator.CreateInstance(type);
                                break;
                            }
                        }
                    }
                    catch 
                    { 
                    }
                }
                return loginObject;
            }
            set
            {
                loginObject = value;
            }
        }


        private static int passwordMaxSize = 10;

        public static int PasswordMaxSize
        {
            get { return passwordMaxSize; }
            set { passwordMaxSize = value; }
        }

        private static int passwordMinSize = 0;

        public static int PassWordMinSize
        {
            get { return passwordMinSize; }
            set { passwordMinSize = value; }
        }

        private static bool passwordCharNum = false;

        public static bool PasswordCharNum
        {
            get { return passwordCharNum; }
            set { passwordCharNum = value; }
        }
        private static int passWrodExpiry = 0;

        public static int PassWrodExpiry
        {
            get { return passWrodExpiry; }
            set { passWrodExpiry = value; }
        }
        //private static int m_userLoginCount;
        public static int UserLoginCount
        {
            get
            {
                //return m_userLoginCount;
                return SrvGL.UserLoginCount;
            }
            set
            {
                //m_userLoginCount = value;
            }
        }

        private static int m_SSOTimeOut = 24;
        public static int SSOTimeOut
        {
            get
            {
                return m_SSOTimeOut;
            }
            set
            {
                m_SSOTimeOut = value;
            }
        }


        public static string SSOKey { get; set; }

        private static int m_maxUser;
        public static int MaxUser
        {
            get
            {
                return m_maxUser;
            }
            set
            {
                m_maxUser = value;
            }
        }

        public static bool RecordLockInDatabase { get; set; }

        private static List<DateTime> AllocateTime = new List<DateTime>();
        public static int AllocatedCount
        {
            get
            {
                ReleaseExpiredAllocate();
                return AllocateTime.Count;
            }
        }

        public static void ReleaseExpiredAllocate()
        {
            lock (typeof(ServerConfig))
            {
                AllocateTime.RemoveAll(new Predicate<DateTime>(CompareAllocate));
            }
        }

        private static bool CompareAllocate(DateTime dt)
        {
            return ((TimeSpan)(DateTime.Now - dt)).TotalMinutes > 3.0;
        }

        public static void Allocate()
        {
            lock (typeof(ServerConfig))
            {
                AllocateTime.Add(DateTime.Now);
            }
        }

        private static bool m_isMaster;
        public static bool IsMaster
        {
            get
            {
                return true;
            }
            set
            {
                m_isMaster = value;
            }
        }

        private static ArrayList m_remoteServers;
        public static ArrayList RemoteServers
        {
            get
            {
                return m_remoteServers;
            }
            set
            {
                m_remoteServers = value;
            }
        }

        private static string m_masterServerIP;
        public static string MasterServerIP
        {
            get
            {
                return m_masterServerIP;
            }
            set
            {
                m_masterServerIP = value;
            }
        }

        private static string m_masterServerKey;
        public static string MasterServerKey
        {
            get { return m_masterServerKey; }
            set { m_masterServerKey = value; }
        }

        private static int m_masterServerPort;
        [Obsolete("port merge to ip")]
        public static int MasterServerPort
        {
            get
            {
                return m_masterServerPort;
            }
            set { m_masterServerPort = value; }
        }


        // Add By Chenjian 2005-12-19
        // For Package Transfer
        private static DataSet m_packageTransferList;
        public static DataSet PackageTransferList
        {
            get
            {
                return m_packageTransferList;
            }
            //set
            //{
            //    m_packageTransferList = value;
            //}
        }

        private static bool m_autoTransferPackage;
        public static bool AutoTransferPackage
        {
            get
            {
                return m_autoTransferPackage;
            }
            set
            {
                m_autoTransferPackage = value;
            }
        }
        // End Add
    }
    #endregion ServerConfig

    #region RemoteServer
    public class RemoteServer
    {
        public RemoteServer(int maxUser, string ipAddress, int port, bool activated)
        {
            m_maxUser = maxUser;
            m_ipAddress = ipAddress;
            _Port = port;
            m_activated = activated;
        }

        private int m_maxUser;
        public int MaxUser
        {
            get
            {
                return m_maxUser;
            }
            set
            {
                m_maxUser = value;
            }
        }

        private string m_ipAddress;
        public string IpAddress
        {
            get
            {
                return m_ipAddress;
            }
            set
            {
                m_ipAddress = value;
            }
        }

        private int _Port;

        public int Port
        {
            get
            {
                return _Port;
            }
            set { _Port = value; }
        }


        private bool m_activated;
        public bool Activated
        {
            get
            {
                return m_activated;
            }
            set
            {
                m_activated = value;
            }
        }

        public override string ToString()
        {
            return string.Format("{0}:{1}", IpAddress, Port);
        }
    }
    #endregion RemoteServer

    #region LoginService
    public class LoginService : MarshalByRefObject
    {
        public LoginService()
        {
            try
            {
                counter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            }
            catch { }
        }

        public object[] GetServerIP()
        {
            return GetServerIP(true);
        }

        public object[] GetServerIP(bool ServerBalance)
        {
            // Get local Ip Address
            //IPHostEntry ipHostEntry = Dns.GetHostEntry(Dns.GetHostName());
            //bool isNoRemoteServerUsable = true;

            //string ipAddress = ipHostEntry.AddressList[0].ToString();
            //int port = SrvUtils.RemotePort;
            //int currentLoginCount = ServerConfig.UserLoginCount;
            //int maxUser = ServerConfig.MaxUser;
            //if (maxUser == 0) maxUser = 50;
            //double loginRatio = 1.0 * currentLoginCount / maxUser;
            //if (currentLoginCount >= maxUser)
            //{
            //    ipAddress = "000";
            //}
            // Get local Ip Address

            //modified by matida 2007/5/19 for login in vista
            //IPHostEntry ipHostEntry = Dns.GetHostEntry(Dns.GetHostName());
            //bool isNoRemoteServerUsable = true;

            String ipAddress = "001";//use local ip
            ////foreach (IPAddress A in ipHostEntry.AddressList) //Vista include IPv4 and IPv6 address
            ////{
            ////    if (!A.IsIPv6LinkLocal)
            ////    {
            ////        ipAddress = A.ToString();
            ////        break;
            ////    }
            ////}
            ////if (ipAddress == "")
            ////    ipAddress = ipHostEntry.AddressList[0].ToString();

            int port = SrvUtils.RemotePort;
            int currentLoginCount = ServerConfig.UserLoginCount;
            int allocatecount = ServerConfig.AllocatedCount;
            int maxUser = ServerConfig.MaxUser;
            if (maxUser == 0) maxUser = 50;
            double loginRatio = 1.0 * (currentLoginCount + allocatecount) / maxUser;
            if (currentLoginCount >= maxUser)
            {
                ipAddress = "000";
            }
            //modified by matida 2007/5/19 for login in vista

            LoginService activeremoteserver = null;

            if (ServerBalance)
            {
                if (ServerConfig.IsMaster)
                {
                    foreach (RemoteServer rs in ServerConfig.RemoteServers)
                    {
                        if (rs.Activated)
                        {
                            try
                            {
                                LoginService loginService = Activator.GetObject(typeof(LoginService),
                                    string.Format("http://{0}:{1}/Srvtools.rem", rs.IpAddress, rs.Port)) as LoginService;

                                loginService.GetLoginInfo(ref currentLoginCount, ref maxUser, ref allocatecount);
                                if (currentLoginCount >= maxUser)
                                {
                                    continue;
                                }
                                if (1.0 * (currentLoginCount + allocatecount) / maxUser <= loginRatio)
                                {
                                    loginRatio = 1.0 * (currentLoginCount + allocatecount) / maxUser;
                                    ipAddress = rs.IpAddress;
                                    port = rs.Port;
                                    //isNoRemoteServerUsable = false;
                                    activeremoteserver = loginService;
                                }
                            }
                            catch //(Exception err)
                            {
                                //System.Windows.Forms.MessageBox.Show(err.Message);
                            }
                        }
                    }
                }
            }
            if (activeremoteserver != null)
            {
                try
                {
                    activeremoteserver.AcceptAllocate();
                }
                catch
                {

                }
            }
            else if (ipAddress != "000")
            {
                ServerConfig.Allocate();
            }
            return new object[] { ipAddress, port };
        }

        public void GetLoginInfo(ref int currentLoginCount, ref int maxUser, ref int allocatedcount)
        {
            currentLoginCount = ServerConfig.UserLoginCount;
            maxUser = ServerConfig.MaxUser;
            allocatedcount = ServerConfig.AllocatedCount;
        }

        public void GetLoginInfo(ref int currentLoginCount, ref int maxUser)
        {
            currentLoginCount = ServerConfig.UserLoginCount;
            maxUser = ServerConfig.MaxUser;
        }

        public void AcceptAllocate()
        {
            ServerConfig.Allocate();
        }

        public bool RegisterRemoteServer(string ipAddress, int port)
        {
            return ServerConfig.RegisterRemoteServer(ipAddress, port);
        }

        public void DeRegisterRemoteServer(string ipAddress, int port)
        {
            ServerConfig.DeRegisterRemoteServer(ipAddress, port);
        }

        private PerformanceCounter counter;

        public float GetCpuInfo()
        {
            return counter != null ? counter.NextValue() : 0.0f;
        }

        public long GetMemoryInfo()
        {
            Process process = Process.GetCurrentProcess();
            return process.WorkingSet64;
        }
    }
    #endregion LoginService

    #region PackageService
    public class PackageService : MarshalByRefObject
    {
        public bool VersionControl(string fileName, string solutionName, PackageType packageType, DateTime timedb, out byte[] buffer, out DateTime timeserver)
        {
            string packagePath = this.GetPackagePath(solutionName, packageType);
            if (!Directory.Exists(packagePath))
            {
                buffer = null;
                timeserver = new DateTime();
                return false;
            }
            string file = string.Format("{0}\\{1}", packagePath, fileName);
            if (File.Exists(file))
            {
                DateTime timefile = File.GetLastWriteTime(file);
                if (timefile.AddMilliseconds(-timefile.Millisecond - 1) > timedb)        //如果DataBase里的记录没Server里的新，就新增一条存放Server里的记录
                {
                    FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
                    buffer = new byte[fileStream.Length];
                    fileStream.Read(buffer, 0, (int)fileStream.Length);
                    fileStream.Close();
                    timeserver = timefile;
                    return true;
                }
            }
            buffer = null;
            timeserver = new DateTime();
            return false;
        }

        public bool Upload(string fileName, string solutionName, PackageType packageType, byte[] buffer, DateTime time)//存到临时文件夹,用来更新
        {
            string packagePath = "";
            if (packageType == PackageType.Server && !RegisterDll(fileName, solutionName) && !fileName.EndsWith(".xoml", StringComparison.OrdinalIgnoreCase))
            {
                packagePath = this.GetPackagePath("EEPServerUpdateTemp\\" + solutionName, packageType);
            }
            else
            {
                packagePath = this.GetPackagePath(solutionName, packageType);
            }
            if (!Directory.Exists(packagePath))
            {
                Directory.CreateDirectory(packagePath);
            }
            string filename = string.Format("{0}\\{1}", packagePath, fileName);
            if (File.Exists(filename))
            {
                try
                {
                    File.Delete(filename);
                }
                catch
                {
                    return false;
                }
            }
            File.WriteAllBytes(filename, buffer);
            File.SetLastWriteTime(filename, time);
            //TODO:  dispatch
            if (ServerConfig.IsMaster)
            {
                foreach (RemoteServer rs in ServerConfig.RemoteServers)
                {
                    if (rs.Activated)
                    {
                        try
                        {
                            PackageService packageService = Activator.GetObject(typeof(PackageService),
                                string.Format("http://{0}:{1}/PackageService.rem", rs.IpAddress, rs.Port)) as PackageService;

                            var a = packageService.Upload(fileName, solutionName, packageType, buffer, time);

                        }
                        catch
                        {
                            ServerConfig.AddPackageTransferListItem(rs.IpAddress, fileName, solutionName, packageType, time.ToString("G"));//无法COPY就记录
                        }
                        // transfer

                    }
                    else
                    {
                        ServerConfig.AddPackageTransferListItem(rs.IpAddress, fileName, solutionName, packageType, time.ToString("G"));
                    }
                }
            }
            return true;
        }

        private bool RegisterDll(string fileName, string solutionName)
        {
            if (File.Exists(SystemFile.PackagesFile) && fileName.EndsWith(".dll", StringComparison.OrdinalIgnoreCase))
            {
                XmlDocument xml = new XmlDocument();
                try
                {
                    xml.Load(SystemFile.PackagesFile);
                    XmlNode nodesolution = xml.SelectSingleNode(string.Format("InfolightPackages/{0}", solutionName));
                    if (nodesolution == null)
                    {
                        nodesolution = xml.CreateElement(solutionName);
                        xml.DocumentElement.AppendChild(nodesolution);
                    }
                    XmlNode nodedll = nodesolution.SelectSingleNode(string.Format("PackageFile[@Name='{0}']", fileName));
                    if (nodedll == null)
                    {
                        nodedll = xml.CreateElement("PackageFile");
                        XmlAttribute attributename = xml.CreateAttribute("Name");
                        attributename.Value = fileName;
                        nodedll.Attributes.Append(attributename);
                        XmlAttribute attributeactive = xml.CreateAttribute("Active");
                        attributeactive.Value = "1";
                        nodedll.Attributes.Append(attributeactive);
                        nodesolution.AppendChild(nodedll);
                        xml.Save(SystemFile.PackagesFile);
                        return true;
                    }
                    else
                    {
                        if (nodedll.Attributes["Active"].Value != "1")
                        {
                            nodedll.Attributes["Active"].Value = "1";
                            xml.Save(SystemFile.PackagesFile);
                            return true;
                        }
                        else
                        {
                            if (AssemblyCache.Enable)
                            {
                                AssemblyCache.Remove(string.Format("{0}\\{1}\\{2}", EEPRegistry.Server, solutionName, fileName));
                                return true;
                            }
                        }
                    }
                }
                catch
                {
                    return false;
                }
            }
            return false;

        }

        public bool GetTransferFile(string fileName, string solutionName, PackageType packageType, out byte[] buffer, out DateTime dt)
        {
            //UpdateTemp里有的话就用里面的
            string packagePath = "";
            bool failtofind = true;
            if (packageType == PackageType.Server)
            {
                packagePath = this.GetPackagePath("EEPServerUpdateTemp\\" + solutionName, packageType);

                string filename = string.Format("{0}\\{1}", packagePath, fileName);
                if (File.Exists(filename))
                {
                    buffer = File.ReadAllBytes(filename); 
                    dt = File.GetLastWriteTime(filename);
                    failtofind = false;
                    return true;
                }
            }
            if (packageType != PackageType.Server || failtofind) //没有就用PackagePath里的
            {
                packagePath = this.GetPackagePath(solutionName, packageType);
                string filename = string.Format("{0}\\{1}", packagePath, fileName);
                if (File.Exists(filename))
                {
                    FileStream fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read);
                    buffer = new byte[fileStream.Length];
                    fileStream.Read(buffer, 0, (int)fileStream.Length);
                    fileStream.Close();
                    dt = File.GetLastWriteTime(filename);
                    return true;
                }
            }
            buffer = null;
            dt = DateTime.Now;
            return false;
        }

        private string GetPackagePath(string solutionName, PackageType packageType)
        {
            string packagePath = string.Empty;

            if (packageType == PackageType.Server)
            {
                packagePath = EEPRegistry.Server;
            }
            else if (packageType == PackageType.Client)
            {
                packagePath = EEPRegistry.Client;
            }
            else if (packageType == PackageType.WebClient)
            {
                packagePath = EEPRegistry.WebClient;
            }
            if (solutionName.Length > 0)
            {
                return string.Format("{0}\\{1}", packagePath, solutionName);
            }
            else
            {
                return packagePath;
            }
        }
    }

    public enum PackageTransferResult
    {
        OK,
        PackageNotFound, // for download
        PackageNotValid, // for upload
        PackageNotOverwritable, // for upload
        PackagePathNotFound,
        ExceptionRaised
    }

    public enum PackageType
    {
        Server,
        Client,
        WebClient
    }
    #endregion PackageService

    #region ListenerService
    public class ListenerService : MarshalByRefObject
    {
        public bool StartServer()
        {
            lock (this)
            {
                if (!CheckServerProcess())
                {
                    if (!StartServer(Application.StartupPath))
                    {
                        return StartServer(EEPRegistry.Server);
                    }
                }
                return true;
            }
        }

        private bool CheckServerProcess()
        {
            Process[] pros = Process.GetProcessesByName("EEPNetServer");
            return pros.Length > 0;
        }

        private bool StartServer(string directory)
        {
            if (File.Exists(Application.StartupPath + "\\EEPNetServer.exe"))
            {
                StartServerProcess(directory + "\\EEPNetServer.exe");
                return true;
            }
            else
            {
                foreach (string file in Directory.GetFiles(directory, "*.exe", SearchOption.TopDirectoryOnly))
                {
                    FileVersionInfo info = FileVersionInfo.GetVersionInfo(file);
                    if (string.Compare(info.OriginalFilename, "EEPNetServer.exe") == 0)
                    {
                        StartServerProcess(file);
                        return true;
                    }
                }
                return false;
            }
        }

        private void StartServerProcess(string path)
        {
            Process pro = Process.Start(path);
            pro.WaitForInputIdle(10000);
        }
    }
    #endregion ListenerService

    public class Domain
    {
        public string Path { get; set; }

        public string User { get; set; }

        public string Password { get; set; }
    }

}
