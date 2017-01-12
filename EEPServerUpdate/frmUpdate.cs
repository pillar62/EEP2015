using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Collections;
using Microsoft.Win32;
using System.IO;
using System.Diagnostics;

namespace EEPServerUpdate
{
    public partial class frmUpdate : Form
    {
        Thread threadupdate = null;
        bool serverstart = false;
        bool finished = false;
        public frmUpdate()
        {
            InitializeComponent();
        }

        private void frmUpdate_Load(object sender, EventArgs e)
        {
            threadupdate = new Thread(new ThreadStart(UpdatePackage));
            threadupdate.Start();
        }

        private void UpdatePackage()
        { 
            //取得要更新的文件列表
            string log = "";
            DateTime dtstart = DateTime.Now;
            log += "Update Start at " + dtstart.ToString("G");       //log date
            string tempdirectory = string.Format("{0}\\EEPServerUpdateTemp", Application.StartupPath);
            if (!Directory.Exists(tempdirectory))
            {
                Directory.CreateDirectory(tempdirectory);
            }
            string[] fileupdate = Directory.GetFiles(tempdirectory, "*.dll", SearchOption.AllDirectories);
            int filecount = fileupdate.Length;
            log += "\r\ntotal " + filecount.ToString() + " files to Update"; // log file amount
            int filesuccess = 0;
            for(int i = 0; i < filecount; i ++)
            {
                string strfile = fileupdate[i];
                if (!Directory.Exists(Path.GetDirectoryName(strfile.Replace("\\EEPServerUpdateTemp", ""))))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(strfile.Replace("\\EEPServerUpdateTemp", "")));
                }
                try
                {
                    File.Copy(strfile, strfile.Replace("\\EEPServerUpdateTemp", ""), true);
                    filesuccess++;
                    try
                    {
                        File.Delete(strfile);
                    }
                    catch
                    {
                        
                    }
                }
                catch
                {
                    Thread.Sleep(500);
                    try
                    {
                        File.Copy(strfile, strfile.Replace("\\EEPServerUpdateTemp", ""), true);//等500毫秒再试一次
                    }
                    catch(Exception e)
                    {
                        log += "\r\n" + strfile + "\r\nupdate error: " + e.Message;//log error
                    }
                }
               
                lbUpdate.Text = strfile.Substring(strfile.LastIndexOf("\\EEPServerUpdateTemp") + 21);
                int per = 100 * i / filecount;
                lbprocess.Text = per.ToString() + "%";
                if (per % 5 == 0)
                {
                    DateTime dtfinish = DateTime.Now;
                    TimeSpan ts = dtfinish - dtstart;
                    int timetofinish = (int)ts.TotalMilliseconds * (filecount - i - 1) / (i + 1);
                    ts = new TimeSpan(0, 0, 0, 0, timetofinish);
                    lbtime.Text = ts.Minutes.ToString("00") + ":" + ts.Seconds.ToString("00");
                }
                progressBarUpdate.Value = per;
            }
            //打开Server
            Process.Start("EEPNetServer.exe");
            serverstart = true;
            log += "\r\n" + filesuccess.ToString() + " files Update Success.";
            if (filecount > 0)
            {
                lbprocess.Text = "100%";
                progressBarUpdate.Value = 100;
                lbtime.Text = "00:00";

                UpdateLog(log);
                //if (MessageBox.Show("Update finish, View Log?","message",MessageBoxButtons.OKCancel,MessageBoxIcon.Information) == DialogResult.OK)
                //{
                //    Process.Start("notepad.exe", "PackageUpdate.log");
                //}
            }
            else
            { 
            
            }
            finished = true;
            this.Close();
        }

        private void UpdateLog(string log)
        {
            FileStream fs = new FileStream("PackageUpdate.log", FileMode.OpenOrCreate);
            fs.Close();
            StreamWriter sw = new StreamWriter("PackageUpdate.log",true);
            sw.Write(log);
            sw.Write("\r\n\r\n");
            sw.Close();
            
        }

        private void frmUpdate_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (threadupdate != null && finished != true)
            {
                if (MessageBox.Show("Update unfinished, package file may be DESTROYED\n\r"
                    + "are you sure to quit?", "WARNING", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning)
                    == DialogResult.OK)
                {
                    threadupdate.Abort();
                    if (!serverstart)
                    {
                        Process.Start("EEPNetServer.exe");
                    }
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }
    }
}