using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Reflection;

namespace EEPWinLoader
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                Assembly assembly = Assembly.LoadFrom("EEPSetUpLibrary.dll");
                Form form = (Form)assembly.CreateInstance("EEPSetUpLibrary.OldClient.frmSetUp");
                if(args.Length > 0)
                {
                    form.GetType().GetField("SelectedServer", BindingFlags.NonPublic| BindingFlags.Instance).SetValue(form, args[0]);
                }
                Application.Run(form);
            }
            catch
            {
                MessageBox.Show("Can not find Assembly Files: EEPSetUpLibrary.dll, Load failed."
                    ,"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }

            
        }
    }
}