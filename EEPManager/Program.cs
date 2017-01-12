using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Runtime.Remoting;
using ProxySetting = EEPManager.Properties.Settings;

namespace EEPManager
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            String s = Application.StartupPath + "\\";
            //#if winxp
            //            RemotingConfiguration.Configure(s + "EEPManager.exe.config", true);
            //#endif
          

            Srvtools.CliUtils.fClientLang = Srvtools.CliSysMegLag.GetClientLanguage();
            Srvtools.CliUtils.fClientSystem = "win";
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Srvtools.CliUtils.LoadLoginServiceConfig(s + "EEPManager.exe.config");
            //A6-Sensitive Data Exposure
            //Srvtools.CliUtils.RegisterProxy(ProxySetting.Default.ProxyEnable, string.Format("{0}:{1}", ProxySetting.Default.ProxyAddress
            //          , ProxySetting.Default.ProxyPort), ProxySetting.Default.ProxyUser, ProxySetting.Default.ProxyPassword);
            Application.Run(new EEPManagerForm());
        }
    }
}