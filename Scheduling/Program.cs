using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;


namespace Scheduling
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            System.Diagnostics.Process[] sProcesses = System.Diagnostics.Process.GetProcesses();
            int count = 0;
            for (int i = 0; i < sProcesses.Length; i++)
            {
                if (sProcesses[i].ProcessName == "Scheduling")
                {
                    if (count == 1)
                    {
                        MessageBox.Show("Scheduling can run once!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); 
                        return;
                    }
                    //未关闭 
                    count++;
                }
            }   

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain());
        }
    }
}