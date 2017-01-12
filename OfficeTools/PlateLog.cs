using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.Win32;


namespace OfficeTools
{
    /// <summary>
    /// The class of log
    /// </summary>
    public class PlateLog
    {

        StreamWriter sw = null;
        int HasErrors = 0;

        /// <summary>
        /// Start log and write system info
        /// </summary>
        /// <param name="Logname">The name of log file</param>
        /// <param name="Title">The name of plate</param>
        public void StartLog(string Logname, string Title)
        {
            HasErrors = 0;
            if (sw == null)
            {
                string directory = Path.GetDirectoryName(Logname);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
 
                sw = new StreamWriter(Logname,true);
                WriteSystemInfo(Title);
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
            if (HasErrors == 0)
            {
                sw.WriteLine("Plate Process success");
            }
            sw.WriteLine(string.Empty);
            sw.WriteLine(string.Empty);
            sw.WriteLine(string.Empty);
            sw.Close();
            sw = null;
        }

        /// <summary>
        /// Write the infomation of system to the log
        /// </summary>
        /// <param name="Title">The type of plate</param>
        public void WriteSystemInfo(string Title)
        {
            StringBuilder separator = new StringBuilder(); ;
            separator.Append('-', Environment.OSVersion.VersionString.Length + 16);

            sw.WriteLine(separator.ToString());
            sw.WriteLine(string.Format("System Version: {0}",Environment.OSVersion.VersionString));
            sw.WriteLine(string.Format("Office Version: {0}",FindOfficeVersion()));
            sw.WriteLine(string.Format("Plate Time:\t{0}", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")));
            sw.WriteLine(string.Format("Plate Type:\t{0}", Title));
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

        /// <summary>
        /// Write the infomation of error to the log
        /// </summary>
        /// <param name="ErrorType">The type of error</param>
        /// <param name="sTag">The text of tag encounter error</param>
        /// <param name="Message">The infomation of error</param>
        public void WriteErrorInfo(string ErrorType, string sTag, string Message)
        {
            if (HasErrors == 0)
            {
                sw.WriteLine("Plate Process exists errors:");
                
            }
            HasErrors ++;
            sw.WriteLine(string.Format("{0}.\tTag Name:\t{1}",HasErrors.ToString(),sTag));
            sw.WriteLine(string.Format("\tError Type:\t{0}", ErrorType));
            sw.WriteLine(string.Format("\tDescription:\t{0}",Message));
            sw.WriteLine(string.Empty);
        }

        /// <summary>
        /// Write the infomation of exception to the log
        /// </summary>
        /// <param name="e">The exception encountered</param>
        public void WriteExceptionInfo(Exception e)
        {
            HasErrors = 1;
            sw.WriteLine("Plate Process encounter exception:");
            sw.WriteLine(string.Format("Message:{0}", e.Message));
            sw.WriteLine("StackTrace:");
            sw.Write(e.StackTrace);
        }
    }

    /// <summary>
    /// The enum of exception type of plate
    /// </summary>
    public enum PlateException
    { 
        None = 0,
        InvalidTag = 1,
        InvalidParameter = 2,
        InvalidExpression = 3,
        TagDefineNotFound = 4,
        InvokeMethodNotFound = 5,
        InvokeMethodError = 6,
        SystemParameterNotFound = 7,
        ColumnNotFound = 8,
        InvalidDataSourceItem = 9,
        InvalidFormat
    }
}
