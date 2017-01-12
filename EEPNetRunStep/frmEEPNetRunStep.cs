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
using System.Configuration;
using System.Collections.Specialized;
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

        private string GetStartPath()
        {
            string path = Application.StartupPath.ToLower();
            string isFlowClient = "false";
            if (ConfigurationManager.AppSettings["isFlowClient"] != null)
                isFlowClient = ConfigurationManager.AppSettings["isFlowClient"].Trim().ToLower();
            if (isFlowClient == "true")
            {
                path = path.Substring(0, path.LastIndexOf("\\") + 1) + "eepnetflclient";
            }
            return path;
        }

        string[] UserMessage;
        int Interval;
        string[] PackageMessage;
        string Log;
        string Fixed;
        string TestMode;
        public frmEEPNetRunStep(string userMessage, string[] packageMessage, string interval, string log, string F, String testMode)
        {
            UserMessage = userMessage.Split(';');
            Interval = getInt(interval);
            PackageMessage = packageMessage;
            Log = log;
            Fixed = F;
            TestMode = testMode;
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
            String sPath = EEPRegistry.Server;
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

        private StringDictionary GetFLItemParamters(string xomlName)
        {
//#if UseFL
//            string flowFileName = this.GetServerPath() + "WorkFlow\\" + xomlName;
//            Activity activity = FLInstance.GetActivityByXoml(flowFileName, "");
//            if (activity != null && activity is CompositeActivity)
//            {
//                Activity cmpAct = ((CompositeActivity)activity).Activities[0];
//                if (cmpAct != null && cmpAct is FLStand)
//                {
//                    FLStand stand = (FLStand)cmpAct;
//                    string openPath = stand.FormName;
//                    if (openPath != null && openPath != "" && openPath.IndexOf('.') != -1)
//                    {
//                        StringDictionary stringDic = new StringDictionary();
//                        string packageName = openPath.Substring(0, openPath.IndexOf('.'));
//                        string formName = openPath.Substring(openPath.IndexOf('.') + 1);
//                        string flMode = ((int)stand.FLNavigatorMode).ToString();
//                        string naMode = ((int)stand.NavigatorMode).ToString();
//                        if (flowFileName.ToLower().EndsWith(".xoml"))
//                            flowFileName = flowFileName.Substring(0, flowFileName.Length - 5);
//                        stringDic.Add("FLOWFILENAME", flowFileName);
//                        stringDic.Add("FLNAVMODE", flMode);
//                        stringDic.Add("NAVMODE", naMode);
//                        return stringDic;
//                    }
//                }
//            }
//#endif
            return null;
        }
        
        private void frmEEPNetRunStep_Load(object sender, EventArgs e)
        {
            language = CliUtils.fClientLang;
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
                string sParam = CliUtils.fLoginUser + ':' + CliUtils.fLoginPassword + ':' + CliUtils.fLoginDB + ":0";

                string[,] packages = new string[PackageMessage.Length, 4];
                //for (int i = 0; i < PackageMessage.Length; i++)
                //{
                //    string[] temp = PackageMessage[i].Split(';');
                //    for (int j = 0; j < temp.Length; j++)
                //        packages[i, j] = temp[j];
                //}

                for (int i = 0; i < PackageMessage.Length; i++)
                {
                    object[] myRet = CliUtils.CallMethod("GLModule", "CheckUser", new object[] { (object)sParam });
                    if (myRet[1] == null || myRet[1] == DBNull.Value || myRet[1].ToString().Length == 0)
                        MessageBox.Show("User or Password is error.");
                    
                    ParameterizedThreadStart pts = new ParameterizedThreadStart(AutoRun);
                    Thread t = new Thread(pts);
                    t.Start(PackageMessage[i]);
                    this.Close();
                }
                //CliUtils.CallMethod("GLModule", "LogOut", new object[] { (object)(UserMessage[0]) });
                //Environment.Exit(0);
            }
        }

        private void AutoRun(object objParam)
        {
            String directory = Application.StartupPath + "\\AutoTestLogs\\" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString("00") + DateTime.Now.Day.ToString("00");
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            MessageBox.Show("Debug");
            String[] packages = objParam.ToString().Split(';');

            string PackageName = packages[0];
            string FormName = packages[1];
            //string XomlName = packages[3];
            string strPackage = this.GetStartPath() + "\\" + CliUtils.fCurrentProject + "\\" + PackageName + ".dll";
            int times = getInt(packages[2]);
            Assembly a = null;
            bool bLoaded = DllContainer.DllLoaded(strPackage);
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

                if (TestMode == "Auto Exec")
                {
                    MethodInfo method = myType.GetMethod("AutoExec");
                    if (method != null)
                        for (int x = 0; x < times; x++)
                        {
                            try
                            {
                                obj = Activator.CreateInstance(myType);
                                type = ((InfoForm)obj).GetType();
                                fi = type.GetFields(BindingFlags.Instance
                                                                | BindingFlags.Public
                                                                | BindingFlags.NonPublic);
                                
                                ((InfoForm)obj).WindowState = FormWindowState.Minimized;
                                ((InfoForm)obj).Show();
                                method.Invoke((InfoForm)obj, null);
                                ((InfoForm)obj).Close();
                                ((IInfoForm)obj).SetPackageForm(PackageName, FormName);
                            }
                            catch(Exception ex)
                            {
                                String path = directory + "\\" + PackageName + "-" + FormName + "-" + CliUtils.fLoginUser + ".txt";
                                WriteLog(path, "[" + DateTime.Now + PackageName + "-" + FormName + "]" + ex.Message);
                                ((InfoForm)obj).Close();
                                ((IInfoForm)obj).SetPackageForm(PackageName, FormName);
                            }
                            finally
                            {
                                System.Threading.Thread.Sleep(Interval);
                                obj = null;
                            }

                        }
                }
                else if (TestMode == "Auto Run")
                {
                    for (int x = 0; x < times; x++)
                    {
                        try
                        {
                            obj = Activator.CreateInstance(myType);
                            type = ((InfoForm)obj).GetType();
                            fi = type.GetFields(BindingFlags.Instance
                                                            | BindingFlags.Public
                                                            | BindingFlags.NonPublic);

                            ((InfoForm)obj).WindowState = FormWindowState.Minimized;
                            ((InfoForm)obj).Show();
                            ((InfoForm)obj).Close();
                            ((IInfoForm)obj).SetPackageForm(PackageName, FormName);
                        }
                        catch (Exception ex)
                        {
                            String path = directory + "\\" + PackageName + "-" + FormName + "-" + CliUtils.fLoginUser + ".txt";
                            WriteLog(path, "[" + DateTime.Now + PackageName + "-" + FormName + "]" + ex.Message);
                            ((InfoForm)obj).Close();
                            ((IInfoForm)obj).SetPackageForm(PackageName, FormName);
                        }
                        finally
                        {
                            System.Threading.Thread.Sleep(Interval);
                            obj = null;
                        }
                    }
                }
                else if (TestMode == "Auto Select")
                {
                    object o = new object();
                    for (int count = 0; count < fi.Length; count++)
                    {
                        o = fi[count].GetValue((InfoForm)obj);
                        if (o != null && o is Container)
                            break;
                    }

                    foreach (Component ctl in ((IContainer)o).Components)
                    {
                        if (ctl is AutoQuery)
                        {
                            //((AutoQuery)ctl).Log = Log;
                            ((AutoQuery)ctl).ExecuteTest(times, Interval, UserMessage[0], PackageName,Log, Fixed);
                        }
                    }
                    ((IInfoForm)obj).SetPackageForm(PackageName, FormName);
                }
                else
                {
                    object o = new object();
                    for (int count = 0; count < fi.Length; count++)
                    {
                        o = fi[count].GetValue((InfoForm)obj);
                        if (o != null && o is Container)
                            break;
                    }

                    foreach (Component ctl in ((IContainer)o).Components)
                    {
                        if (ctl is AutoTest)
                        {
                            ((AutoTest)ctl).Log = Log;
                            if (((AutoTest)ctl).ClickControl != "(none)" && ((AutoTest)ctl).ClickControl != null && ((AutoTest)ctl).ClickControl != "")
                            {
                                ((InfoForm)obj).WindowState = FormWindowState.Minimized;
                                ((InfoForm)obj).Show();
                                ((AutoTest)ctl).AutoClick(times, PackageName, UserMessage[0], Interval);
                                ((InfoForm)obj).Close();
                            }
                            else if (((AutoTest)ctl).ParentAutoTest == null)
                            {
                                //((AutoTest)ctl).FlowFileName = this.GetServerPath() + "WorkFlow\\" + XomlName;
                                ((AutoTest)ctl).ExecuteTest(times, Interval, UserMessage[0], PackageName, Fixed);
                            }
                        }
                    }
                    ((IInfoForm)obj).SetPackageForm(PackageName, FormName);
                }
            }
            CliUtils.CallMethod("GLModule", "LogOut", new object[] { (object)(UserMessage[0]) });
        }

        private void WriteLog(String fileName, String message)
        {
            try
            {
                ArrayList alLog = new ArrayList();
                FileStream fs = File.Open(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                StreamReader sreader = new StreamReader(fs);
                alLog.Add(sreader.ReadToEnd());
                sreader.Close();

                fs = File.Open(fileName, FileMode.Open, FileAccess.Write, FileShare.ReadWrite);
                StreamWriter swriter = new StreamWriter(fs);
                for (int i = 0; i < alLog.Count; i++)
                {
                    swriter.WriteLine(alLog[i]);
                }
                swriter.WriteLine(message);
                swriter.Close();
                fs.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                MessageBox.Show(ex.GetType().ToString());
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
            string ss = this.GetStartPath() + "\\" + CliUtils.fCurrentProject + "\\";
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
                            ((AutoTest)ctl).ExecuteTest(getInt(tbTimes.Text), 100, tbUserID.Text, PackageName, Fixed);
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
            //RemotingConfiguration.Configure(s + "EEPNetClient.exe.config", true); //replace by ccm
            string path = Application.StartupPath.ToLower();
            string isFlowClient = "false";
            if (ConfigurationManager.AppSettings["isFlowClient"] != null)
                isFlowClient = ConfigurationManager.AppSettings["isFlowClient"].Trim().ToLower();
            if (isFlowClient == "true")
            {
                path = path.Substring(0, path.LastIndexOf("\\") + 1) + "eepnetflclient\\";
                CliUtils.LoadLoginServiceConfig(path + "EEPNetFLClient.exe.config");
            }
            else
            {
                CliUtils.LoadLoginServiceConfig(path + "\\EEPNetClient.exe.config");
            }
           
            string message = "";
            bool rtn = CliUtils.Register(ref message);
            if (rtn)
            {
                if (args.Length > 0)
                {
                    String arg = args[0];
                    for (int i = 1; i < args.Length; i++)
                        arg += " " + args[i];
                    string[] temp = arg.Split('!');
                    string userMessage = temp[0];
                    string[] packageMessage = temp[1].Split(',');
                    string interval = temp[2];
                    string log = temp[3];
                    string Fixed = temp[4];
                    string testMode = temp[5];
                    Mutex m = new Mutex(false, userMessage);
                    Application.Run(new frmEEPNetRunStep(userMessage, packageMessage, interval, log, Fixed, testMode));
                }
                else
                    Application.Run(new frmEEPNetRunStep());
            }
            else
            {
                 MessageBox.Show(message);
            }
//            LoginService loginService = new LoginService(); // Remoting object

//BeginObtainService:
//            // Obtain service from the master server
//            string serverIP = "";

//            try
//            {
//                string[] strrtn = loginService.GetServerIP();
//                serverIP = strrtn[0];
//                CliUtils.fRemotePort = int.Parse(strrtn[1]);
//            }
//            catch (Exception err)
//            {
//                if ((string.Compare(err.Message.ToLower(), "unable to connect to the remote server") == 0) &&
//                    CliUtils.PassByEEPListener())
//                {
//                    try
//                    {
//                        CliUtils.EEPListenerService.StartupEEPNetServer();
//                    }
//                    catch (Exception E)
//                    {
//                        MessageBox.Show(E.Message);
//                        //this.Close();
//                        return;
//                    }
//                    goto BeginObtainService;
//                }
//                else
//                {
//                    MessageBox.Show(err.Message);
//                    //this.Close();
//                    return;
//                }
//            }

//            if (serverIP == null || serverIP.Trim() == "")
//            {
//                MessageBox.Show("Can not login due to busy service");
//                //this.Close();
//                return;
//            }

//            // Try to connect to server, reobtain service from the master server if failed
//            try
//            {
//                EEPRemoteModule module = Activator.GetObject(typeof(EEPRemoteModule),
//                    string.Format("http://{0}:{1}/InfoRemoteModule.rem", serverIP,CliUtils.fRemotePort)) as EEPRemoteModule;
//                module.ToString();
//            }
//            catch
//            {
//                loginService.DeRegisterRemoteServer(serverIP, CliUtils.fRemotePort.ToString());
//                goto BeginObtainService;
//            }

//            // Register EEPRemoteModule on the server
//            WellKnownClientTypeEntry clientEntry = new WellKnownClientTypeEntry(typeof(EEPRemoteModule),
//                string.Format("http://{0}:{1}/InfoRemoteModule.rem", serverIP, CliUtils.fRemotePort));
//            RemotingConfiguration.RegisterWellKnownClientType(clientEntry);

            // End Add
           
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