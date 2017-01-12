using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.Win32;

namespace Infolight.EasilyReportTools.Tools
{
    internal class EasilyReportLog:IDisposable
    {
        private StreamWriter sw;
        private string mReportExportMode;
        private string mObjType;
        private int hasErrors = 0;
        private string mLogFileName;
        private IReport report;

        public EasilyReportLog(string reportExportMode, string objType, string logFileName, IReport rpt)
        {
            mReportExportMode = reportExportMode;
            mObjType = objType;
            mLogFileName = logFileName;
            report = rpt;
        }

        private string GetLogPath(string logFileName)
        {
            string logPath = String.Empty;

            if (report is EasilyReport)
            {
                logPath = Path.Combine(System.Windows.Forms.Application.StartupPath, logFileName);
            }
            else
            {
                logPath = String.Format("{0}\\{1}", ((WebEasilyReport)report).Page.MapPath(""), logFileName);
            }

            return logPath;
        }

        /// <summary>
        /// Start log and write system info
        /// </summary>
        public void StartLog()
        {
            hasErrors = 0;
            if (sw == null)
            {
                string fullPath = GetLogPath(mLogFileName);
                string directory = Path.GetDirectoryName(fullPath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                sw = new StreamWriter(fullPath, true);
                WriteSystemInfo();

                sw.Close();
                sw = null;
            }
            else
            {
                throw new Exception("Log has started, can not start again until end log");
            }
        }

        /// <summary>
        /// Save and end the log
        /// </summary>
        public void EndLog()
        {
            if (sw == null)
            {
                string fullPath = GetLogPath(mLogFileName);
                string directory = Path.GetDirectoryName(fullPath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                sw = new StreamWriter(fullPath, true);
            }

            if (hasErrors == 0)
            {
                sw.WriteLine(string.Format("\tExecution Time:\t{0}", DateTime.Now.ToString()));
                sw.WriteLine(mReportExportMode + " Process success");
            }
            sw.WriteLine(string.Empty);
            sw.WriteLine(string.Empty);
            sw.WriteLine(string.Empty);
            sw.Close();
            sw = null;
        }

        public void Dispose()
        {
            sw.Close();
            sw = null;
        }

        /// <summary>
        /// Write the infomation of system to the log
        /// </summary>
        public void WriteSystemInfo()
        {
            StringBuilder separator = new StringBuilder(); ;
            separator.Append('-', Environment.OSVersion.VersionString.Length + 16);

            sw.WriteLine(string.Format("\tExecution Time:\t{0}", DateTime.Now.ToString()));
            sw.WriteLine(separator.ToString());
            sw.WriteLine(string.Format("System Version: {0}", Environment.OSVersion.VersionString));
            if (mReportExportMode == "Excel Report")
            {
                sw.WriteLine(string.Format("Office Version: {0}", FindOfficeVersion()));
            }
            else
            {
                sw.WriteLine(string.Format("Pdf Version: {0}", FindPdfVersion()));
            }
            sw.WriteLine(separator.ToString());
        }

        private string FindOfficeVersion()
        {
            string[] strver = new string[] { "97", "2000", "2002(XP)", "2003", "2007" };
            for (int i = 12; i >= 8; i--)
            {
                RegistryKey rk = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Office\\" + i.ToString("f1") + "\\Common\\InstallRoot");
                if (rk != null && rk.GetValue("Path") != null)
                {
                    if (rk.GetValue("Path") != null)
                    {
                        string value = rk.GetValue("Path").ToString();
                        rk.Close();
                        if (Directory.Exists(value))
                        {
                            return "Office " + strver[i - 8];
                        }
                    }
                }
            }
            return "Unknown";
        }

        private string FindPdfVersion()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void WriteInfo(string message)
        {
            if (sw == null)
            {
                string fullPath = GetLogPath(mLogFileName);
                string directory = Path.GetDirectoryName(fullPath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                sw = new StreamWriter(fullPath, true);
            }

            sw.WriteLine(string.Format("\tClass Type:\t{0}", this.mObjType));
            sw.WriteLine(string.Format("\tExecution Time:\t{0}", DateTime.Now.ToString()));
            sw.WriteLine(string.Format("\tDescription:\t{0}", message));
            sw.WriteLine(string.Empty);

            sw.Close();
            sw = null;
        }

        /// <summary>
        /// Write the infomation of error to the log
        /// </summary>
        /// <param name="ErrorType">The type of error</param>
        /// <param name="Message">The infomation of error</param>
        public void WriteErrorInfo(string errorType, string message)
        {
            if (sw == null)
            {
                string fullPath = GetLogPath(mLogFileName);
                string directory = Path.GetDirectoryName(fullPath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                sw = new StreamWriter(fullPath, true);
            }
            
            if (hasErrors == 0)
            {
                sw.WriteLine(mReportExportMode + " Process exists errors:");
            }
            hasErrors++;
            sw.WriteLine(string.Format("\tClass Type:\t{0}", this.mObjType));
            sw.WriteLine(string.Format("\tExecution Time:\t{0}", DateTime.Now.ToString()));
            sw.WriteLine(string.Format("\tError Type:\t{0}", errorType));
            sw.WriteLine(string.Format("\tDescription:\t{0}", message));
            sw.WriteLine(string.Empty);

            sw.Close();
            sw = null;
        }

        /// <summary>
        /// Write the infomation of exception to the log
        /// </summary>
        /// <param name="e">The exception encountered</param>
        public void WriteExceptionInfo(Exception e)
        {
            hasErrors = 1;

            if (sw == null)
            {
                string fullPath = GetLogPath(mLogFileName);
                string directory = Path.GetDirectoryName(fullPath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                sw = new StreamWriter(fullPath, true);
            }

            sw.WriteLine(string.Format("\tClass Type:\t{0}", this.mObjType));
            sw.WriteLine(string.Format("\tExecution Time:\t{0}", DateTime.Now.ToString()));
            sw.WriteLine(mReportExportMode + " Process encounter exception:");
            sw.WriteLine(string.Format("Message:{0}", e.Message));
            sw.WriteLine("StackTrace:");
            sw.Write(e.StackTrace);
            sw.Close();
            sw = null;
        }
    }

   
}
