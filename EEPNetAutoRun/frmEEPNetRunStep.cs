using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Runtime.Remoting;
using System.IO;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.Reflection;
using Srvtools;
using System.Collections;
using System.Diagnostics;
using System.Text;
using System.Xml;
using System.Threading;

namespace EEPNetRunStep
{
    public partial class frmEEPNetRunStep: Form
    {
        private SYS_LANGUAGE language;
        
        public frmEEPNetRunStep()
        {
            language = CliSysMegLag.GetClientLanguage();
            InitializeComponent();
        }

        string[] UserMessage;
        int Interval;
        string[] PackageMessage;
        string Log;
        public frmEEPNetRunStep(string userMessage, string[] packageMessage, string interval, string log)
        {
            UserMessage = userMessage.Split(';');
            Interval = getInt(interval);
            PackageMessage = packageMessage;
            Log = log;
            InitializeComponent();
        }

        public int getInt(string str)
        {
            int x = 0;
            for (int i = 0; i < str.Length; i++)
            {
                int temp = 0;
                switch (str[i])
                {
                    case '0': temp = 0; break;
                    case '1': temp = 1; break;
                    case '2': temp = 2; break;
                    case '3': temp = 3; break;
                    case '4': temp = 4; break;
                    case '5': temp = 5; break;
                    case '6': temp = 6; break;
                    case '7': temp = 7; break;
                    case '8': temp = 8; break;
                    case '9': temp = 9; break;
                }
                x = x + temp * (int)Math.Pow(10, (str.Length - i - 1));
            }
            return x;
        }

        public string getPath()
        {
            RegistryKey rk = Registry.LocalMachine.OpenSubKey("SOFTWARE\\infolight\\eep.net");
            String sPath = (String)rk.GetValue("Server Path");
            rk.Close();
            if (sPath.Length > 0 && sPath[sPath.Length - 1] != '\\') sPath = sPath + "\\";
            XmlDocument DBXML = new XmlDocument();
            string strPath = "";

            try
            {
                FileStream aFileStream = new FileStream(sPath + "Path.xml", FileMode.Open, FileAccess.ReadWrite, FileShare.None);
                DBXML.Load(aFileStream);

                for (int j = DBXML.DocumentElement.ChildNodes.Count - 1; j >= 0; j--)
                {
                    if (DBXML.DocumentElement.ChildNodes[j].Attributes["Name"].Value.Equals("EEPNetClient"))
                    {
                        strPath = DBXML.DocumentElement.ChildNodes[j].Attributes["Path"].Value;
                        if (sPath.Length > 0 && strPath[strPath.Length - 1] != '\\') strPath = strPath + "\\";
                        break;
                    }
                }
                aFileStream.Close();
            }
            catch (Exception e)
            {
                string str = e.Message;
            }
            return strPath;
        }
        
        private void frmEEPNetRunStep_Load(object sender, EventArgs e)
        {
            string message = SysMsg.GetSystemMessage(language, "Srvtools", "EEPNetRunStep", "LabelName");
            string[] user = message.Split(';');
            this.labUserID.Text = user[0];
            this.labPassword.Text = user[1];
            this.labDataBase.Text = user[2];
            this.labSolution.Text = user[3];
            this.labPackageName.Text = user[4];
            this.labFormName.Text = user[5];
            this.labTimes.Text = user[6];
            
            if (UserMessage != null)
            {
                CliUtils.fLoginUser = UserMessage[0];
                CliUtils.fLoginPassword = UserMessage[1];
                CliUtils.fLoginDB = UserMessage[2];
                CliUtils.fCurrentProject = UserMessage[3];
                string sParam = CliUtils.fLoginUser + ':' + CliUtils.fLoginPassword + ':' + CliUtils.fLoginDB;
                object[] myRet = CliUtils.CallMethod("GLModule", "CheckUser", new object[] { (object)sParam });
                if (myRet[1] == null || myRet[1] == DBNull.Value || myRet[1].ToString().Length == 0)
                    MessageBox.Show("User or Password is error.");

                string[,] packages = new string[PackageMessage.Length, 3];
                for (int i = 0; i < PackageMessage.Length; i++)
                {
                    string[] temp = PackageMessage[i].Split(';');
                    for (int j = 0; j < 3; j++)
                        packages[i, j] = temp[j];
                }

                for (int i = 0; i < PackageMessage.Length; i++)
                {
                    string PackageName = packages[i, 0];
                    string FormName = packages[i, 1];
                    string ss;

                    ss = getPath() + CliUtils.fCurrentProject + "\\";
                    string strPackage = ss + PackageName + ".dll";
                    Assembly a = null;
                    bool bLoaded = DllContainer.DllLoaded(strPackage);

                    //if (!bLoaded || !File.Exists(strPackage)) CheckAndDownLoad(strPackage, PackageName + ".dll");
                    try
                    {
                        a = Assembly.LoadFrom(strPackage);
                    }
                    finally
                    {
                        if (!bLoaded) DllContainer.AddDll(strPackage);
                    }

                    Type myType = a.GetType(PackageName + "." + FormName);
                    if (myType != null)
                    {
                        object obj = Activator.CreateInstance(myType);

                        Type type = ((InfoForm)obj).GetType();
                        FieldInfo[] fi = type.GetFields(BindingFlags.Instance
                                                        | BindingFlags.Public
                                                        | BindingFlags.NonPublic);
                        object o = fi[0].GetValue((InfoForm)obj);
                        foreach (Component ctl in ((IContainer)o).Components)
                        {
                            if (ctl is AutoTest)
                            {
                                ((AutoTest)ctl).Log = Log;
                                if (((AutoTest)ctl).ClickControl != "(none)" || ((AutoTest)ctl).ClickControl != null || ((AutoTest)ctl).ClickControl != "")
                                {
                                    ((InfoForm)obj).WindowState = FormWindowState.Minimized;
                                    ((InfoForm)obj).Show();
                                }
                            }
                        }

                        foreach (Component ctl in ((IContainer)o).Components)
                        {
                            if (ctl is AutoTest)
                            {
                                if (((AutoTest)ctl).ParentAutoTest == null)
                                {
                                    ((AutoTest)ctl).ExecuteTest(getInt(packages[i, 2]), Interval, UserMessage[0], PackageName);
                                }
                            }
                        }
                        ((IInfoForm)obj).SetPackageForm(PackageName, FormName);
                    }
                }
                CliUtils.CallMethod("GLModule", "LogOut", new object[] { (object)(UserMessage[0]) });
                Environment.Exit(0);
            }
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            CliUtils.fLoginUser = tbUserID.Text;
            CliUtils.fLoginPassword = tbPassword.Text;
            CliUtils.fLoginDB = tbDataBase.Text;
            CliUtils.fCurrentProject = tbSolution.Text;
            string sParam = CliUtils.fLoginUser + ':' + CliUtils.fLoginPassword + ':' + CliUtils.fLoginDB;
            object[] myRet = CliUtils.CallMethod("GLModule", "CheckUser", new object[] { (object)sParam });
            if (myRet[1] == null || myRet[1] == DBNull.Value || myRet[1].ToString().Length == 0)
                MessageBox.Show("User or Password is error.");
         
            string PackageName = tbPackageName.Text;
            string FormName = tbFormName.Text;
            string ss;
            //StringBuilder sb = new StringBuilder(Application.StartupPath);
            //for (int i = sb.Length - 1; i > 0; i--)
            //{
            //    if (sb[i] != '\\')
            //        sb.Remove(i, 1);
            //    else
            //        break;
            //}
            ss = getPath() + CliUtils.fCurrentProject + "\\";
            string strPackage = ss + PackageName + ".dll";
            Assembly a = null;
            bool bLoaded = DllContainer.DllLoaded(strPackage);

            //if (!bLoaded || !File.Exists(strPackage)) CheckAndDownLoad(strPackage, PackageName + ".dll");
            try
            {
                a = Assembly.LoadFrom(strPackage);
            }
            finally
            {
                if (!bLoaded) DllContainer.AddDll(strPackage);
            }

            Type myType = a.GetType(PackageName + "." + FormName);
            if (myType != null)
            {
                object obj = Activator.CreateInstance(myType);

                Type type = ((InfoForm)obj).GetType();
                FieldInfo[] fi = type.GetFields(BindingFlags.Instance
                                                | BindingFlags.Public
                                                | BindingFlags.NonPublic);
                object o = fi[0].GetValue((InfoForm)obj);
                foreach (Component ctl in ((IContainer)o).Components)
                {
                    if (ctl is AutoTest)
                    {
                        ((AutoTest)ctl).Log = Log;
                        if (((AutoTest)ctl).ClickControl != null)
                        {
                            ((InfoForm)obj).WindowState = FormWindowState.Minimized;
                            ((InfoForm)obj).Show();
                        }
                    }
                }

                foreach (Component ctl in ((IContainer)o).Components)
                {
                    if (ctl is AutoTest)
                    {
                        if (((AutoTest)ctl).ParentAutoTest == null)
                        {
                            ((AutoTest)ctl).ExecuteTest(getInt(tbTimes.Text), 100, tbUserID.Text, PackageName);
                        }
                    }
                }
                ((IInfoForm)obj).SetPackageForm(PackageName, FormName);
                CliUtils.CallMethod("GLModule", "LogOut", new object[] { (object)(tbUserID.Text) });
            }
        }
    }

    static class Program
    {
        ///<summary>
        ///The main entry point for the application.
        ///</summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            CliUtils.fClientLang = GetClientLanguage();
            String s;
            s = Application.StartupPath + "\\";
            RemotingConfiguration.Configure(s + "EEPNetRunStep.exe.config", true);

            RegistryKey rk = Registry.LocalMachine.OpenSubKey("SOFTWARE\\infolight\\eep.net");
            String sPath = (String)rk.GetValue("Server Path");
            rk.Close();
            if (sPath.Length > 0 && sPath[sPath.Length - 1] != '\\') sPath = sPath + "\\";
            string name = "EEPNetRunStep";

            try
            {
                XmlDocument DBXML = new XmlDocument();
                FileStream aFileStream;
                if (!File.Exists(sPath + "Path.xml"))
                {
                    try
                    {
                        aFileStream = new FileStream(sPath + "Path.xml", FileMode.Create, FileAccess.ReadWrite, FileShare.None);
                        try
                        {
                            XmlTextWriter w = new XmlTextWriter(aFileStream, new System.Text.ASCIIEncoding());
                            w.Formatting = Formatting.Indented;
                            w.WriteStartElement("InfolightAutoRunMessage");
                            w.WriteEndElement();
                            w.Close();
                        }
                        finally
                        {
                            aFileStream.Close();
                        }
                    }
                    catch (Exception e)
                    {
                        string str = e.Message;
                    }
                }

                try
                {
                    aFileStream = new FileStream(sPath + "Path.xml", FileMode.Open, FileAccess.ReadWrite, FileShare.None);
                    try
                    {
                        DBXML.Load(aFileStream);
                        XmlNode aNode = null;

                        for (int j = DBXML.DocumentElement.ChildNodes.Count - 1; j >= 0; j--)
                        {
                            if (DBXML.DocumentElement.ChildNodes[j].Attributes["Name"].InnerText.Trim().ToUpper().Equals(name.ToUpper()))
                            {
                                aNode = DBXML.DocumentElement.ChildNodes[j];
                                break;
                            }
                        }

                        if (aNode == null)
                        {

                            XmlElement elem = DBXML.CreateElement("String");

                            XmlAttribute attr = DBXML.CreateAttribute("Name");
                            attr.Value = name;
                            elem.Attributes.Append(attr);

                            attr = DBXML.CreateAttribute("Path");
                            attr.Value = Application.StartupPath;
                            elem.Attributes.Append(attr);

                            DBXML.DocumentElement.AppendChild(elem);
                        }
                        else
                        {
                            aNode.Attributes["Path"].InnerText = Application.StartupPath;
                        }
                    }
                    finally
                    {
                        aFileStream.Close();
                    }
                    DBXML.Save(sPath + "Path.xml");
                }
                catch (Exception e)
                {
                    string str = e.Message;
                }
            }
            finally { }

            LoginService loginService = new LoginService(); // Remoting object

BeginObtainService:
            // Obtain service from the master server
            string serverIP = "";

            try
            {
                serverIP = loginService.GetServerIP();

            }
            catch (Exception err)
            {
                if ((string.Compare(err.Message.ToLower(), "unable to connect to the remote server") == 0) &&
                    CliUtils.PassByEEPListener())
                {
                    try
                    {
                        CliUtils.EEPListenerService.StartupEEPNetServer();
                    }
                    catch (Exception E)
                    {
                        MessageBox.Show(E.Message);
                        //this.Close();
                        return;
                    }
                    goto BeginObtainService;
                }
                else
                {
                    MessageBox.Show(err.Message);
                    //this.Close();
                    return;
                }
            }

            if (serverIP == null || serverIP.Trim() == "")
            {
                MessageBox.Show("Can not login due to busy service");
                //this.Close();
                return;
            }

            // Try to connect to server, reobtain service from the master server if failed
            try
            {
                EEPRemoteModule module = Activator.GetObject(typeof(EEPRemoteModule),
                    string.Format("http://{0}:8989/InfoRemoteModule.rem", serverIP)) as EEPRemoteModule;
                module.ToString();
            }
            catch
            {
                loginService.DeRegisterRemoteServer(serverIP);
                goto BeginObtainService;
            }

            // Register EEPRemoteModule on the server
            WellKnownClientTypeEntry clientEntry = new WellKnownClientTypeEntry(typeof(EEPRemoteModule),
                string.Format("http://{0}:8989/InfoRemoteModule.rem", serverIP));
            RemotingConfiguration.RegisterWellKnownClientType(clientEntry);

            // End Add
            if (args.Length > 0)
            {
                string[] temp = args[0].Split('!');
                string userMessage = temp[0];
                string[] packageMessage = temp[1].Split(',');
                string interval = temp[2];
                string log = temp[3];
                Mutex m = new Mutex(false, userMessage);
                Application.Run(new frmEEPNetRunStep(userMessage, packageMessage, interval, log));
            }
            else
                Application.Run(new frmEEPNetRunStep());
        }

        static private SYS_LANGUAGE GetClientLanguage()
        {
            uint dwlang = GetThreadLocale();
            ushort wlang = (ushort)dwlang;
            ushort wprilangid = (ushort)(wlang & 0x3FF);
            ushort wsublangid = (ushort)(wlang >> 10);

            if (0x09 == wprilangid)
                return SYS_LANGUAGE.ENG;
            else if (0x04 == wprilangid)
            {
                if (0x01 == wsublangid)
                    return SYS_LANGUAGE.TRA;
                else if (0x02 == wsublangid)
                    return SYS_LANGUAGE.SIM;
                else if (0x03 == wsublangid)
                    return SYS_LANGUAGE.HKG;
                else
                    return SYS_LANGUAGE.TRA;
            }
            else if (0x11 == wprilangid)
                return SYS_LANGUAGE.JPN;
            else
                return SYS_LANGUAGE.ENG;
        }

        [DllImport("KERNEL32.DLL", EntryPoint = "GetThreadLocale", SetLastError = true,
            CharSet = CharSet.Unicode, ExactSpelling = true,
            CallingConvention = CallingConvention.StdCall)]
        public static extern uint GetThreadLocale();

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
}