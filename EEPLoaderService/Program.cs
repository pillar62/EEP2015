using System.Collections.Generic;
using System.ServiceProcess;
using System.Text;
using System.Windows.Forms;
using System;
using System.Runtime.InteropServices;

namespace EEPLoaderService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(String[] Params)
        {
            try
            {
                System.Diagnostics.Process[] P = System.Diagnostics.Process.GetProcessesByName("EEPServerLoader");
                if (P.Length > 1)
                {
                    MessageBox.Show("The EEPNetServer Listener is already running !!");
                    return;
                }

                if (StartService())
                {
                    ServiceBase[] ServicesToRun;

                    // More than one user Service may run within the same process. To add
                    // another service to this process, change the following line to
                    // create a second service object. For example,
                    //
                    //   ServicesToRun = new ServiceBase[] {new Service1(), new MySecondUserService()};
                    //
                    ServicesToRun = new ServiceBase[] { new Service1() };

                    ServiceBase.Run(ServicesToRun);
                }
                else
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new FormMain());
                }
            }
            catch (Exception e)
            {
                using (var writer = new System.IO.StreamWriter(@"c:\\eeploaderService.log", true, new UTF8Encoding(true)))
                {
                    writer.WriteLine("DateTime:" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                    writer.WriteLine("Message:" + e.Message);
                    writer.WriteLine("Stack:" + e.StackTrace);
                    if (e.InnerException != null)
                    {
                        writer.WriteLine("Message:" + e.InnerException.Message);
                        writer.WriteLine("Stack:" + e.InnerException.StackTrace);
                        writer.WriteLine();
                    }
                }
            }
        }

        static private Boolean Installing(String[] Params)
        {
            if (Params.Length == 1)
            {
                String Param = Params[0];
                if (Param[0] == '-' || Param[0] == '/' || Param[0] == '\\')
                {
                    Param = Param.Substring(1, Param.Length - 1);
                    if (Param.ToUpper().CompareTo("INSTALL") == 0 || Param.ToUpper().CompareTo("UNINSTALL") == 0)
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
            else
                return false;
        }

        private const int SERVICE_WIN32_OWN_PROCESS = 0x00000010;
        private const int SERVICE_DEMAND_START = 0x00000003;
        private const int SERVICE_ERROR_NORMAL = 0x00000001;
        private const int STANDARD_RIGHTS_REQUIRED = 0xF0000;
        private const int SERVICE_QUERY_CONFIG = 0x0001;
        private const int SERVICE_CHANGE_CONFIG = 0x0002;
        private const int SERVICE_QUERY_STATUS = 0x0004;
        private const int SERVICE_ENUMERATE_DEPENDENTS = 0x0008;
        private const int SERVICE_START = 0x0010;
        private const int SERVICE_STOP = 0x0020;
        private const int SERVICE_PAUSE_CONTINUE = 0x0040;
        private const int SERVICE_INTERROGATE = 0x0080;
        private const int SERVICE_USER_DEFINED_CONTROL = 0x0100;
        private const int SERVICE_DELETE = 0x10000;
        private const int SC_MANAGER_CONNECT = 0x0001;
        private const int SC_MANAGER_CREATE_SERVICE = 0x0002;
        private const int SC_MANAGER_ENUMERATE_SERVICE = 0x0004;
        private const int SC_MANAGER_LOCK = 0x0008;
        private const int SC_MANAGER_QUERY_LOCK_STATUS = 0x0010;
        private const int SC_MANAGER_MODIFY_BOOT_CONFIG = 0x0020;
        private const int SERVICE_CONFIG_DESCRIPTION = 0x0001;

        private const int SC_MANAGER_ALL_ACCESS = (STANDARD_RIGHTS_REQUIRED |
            SC_MANAGER_CONNECT | SC_MANAGER_CREATE_SERVICE | SC_MANAGER_ENUMERATE_SERVICE |
            SC_MANAGER_LOCK | SC_MANAGER_QUERY_LOCK_STATUS | SC_MANAGER_MODIFY_BOOT_CONFIG);

        private const int SERVICE_ALL_ACCESS = (STANDARD_RIGHTS_REQUIRED | SERVICE_QUERY_CONFIG |
            SERVICE_CHANGE_CONFIG | SERVICE_QUERY_STATUS | SERVICE_ENUMERATE_DEPENDENTS |
            SERVICE_START | SERVICE_STOP | SERVICE_PAUSE_CONTINUE | SERVICE_INTERROGATE |
            SERVICE_USER_DEFINED_CONTROL);

        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern IntPtr OpenSCManager(string lpMachineName, string lpSCDB, int scParameter);

        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern IntPtr OpenService(IntPtr SCHANDLE, string lpSvcName, int dwNumServiceArgs);

        [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern Boolean QueryServiceConfig(IntPtr hService, IntPtr intPtrQueryConfig, UInt32 cbBufSize, out UInt32 pcbBytesNeeded);

        [DllImport("advapi32.dll", SetLastError = true)]
        static extern bool GetUserName(System.Text.StringBuilder sb, ref Int32 length);

        static private Boolean StartService()
        {
            Boolean Result = false;
            String ServiceStartName = "";
            IntPtr Mgr = OpenSCManager(null, null, SC_MANAGER_ALL_ACCESS);
            if (Mgr != IntPtr.Zero)
            {
                IntPtr Svc = OpenService(Mgr, "EEPLoaderService", SERVICE_ALL_ACCESS);
                Result = Svc != IntPtr.Zero;
                if (Result)
                {
                    UInt32 Size = 0;
                    IntPtr Config = Marshal.AllocHGlobal(4096);
                    try
                    {
                        QueryServiceConfig(Svc, Config, 4096, out Size);
                        QUERY_SERVICE_CONFIG qUERY_SERVICE_CONFIG = new QUERY_SERVICE_CONFIG();
                        Marshal.PtrToStructure(Config, qUERY_SERVICE_CONFIG);
                        ServiceStartName = qUERY_SERVICE_CONFIG.lpServiceStartName;
                        if (String.Compare(ServiceStartName, "LocalSystem") == 0)
                            ServiceStartName = "SYSTEM";
                    }
                    finally
                    {
                        Marshal.FreeHGlobal(Config);
                    }
                }
              
            }
            if (Result)
            { 
                StringBuilder UserName = new StringBuilder();
                int NameLength = 256;
                GetUserName(UserName, ref NameLength);
                Result = ServiceStartName.CompareTo(UserName.ToString()) == 0;
            }
            return Result;
        }

        [StructLayout(LayoutKind.Sequential)]
        public class QUERY_SERVICE_CONFIG
        {
            [MarshalAs(System.Runtime.InteropServices.UnmanagedType.U4)]
            public UInt32 dwServiceType;
            [MarshalAs(System.Runtime.InteropServices.UnmanagedType.U4)]
            public UInt32 dwStartType;
            [MarshalAs(System.Runtime.InteropServices.UnmanagedType.U4)]
            public UInt32 dwErrorControl;
            [MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPWStr)]
            public String lpBinaryPathName;
            [MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPWStr)]
            public String lpLoadOrderGroup;
            [MarshalAs(System.Runtime.InteropServices.UnmanagedType.U4)]
            public UInt32 dwTagID;
            [MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPWStr)]
            public String lpDependencies;
            [MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPWStr)]
            public String lpServiceStartName;
            [MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPWStr)]
            public String lpDisplayName;
        };

    }
}