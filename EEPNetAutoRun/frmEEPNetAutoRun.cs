using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Xml;
using Srvtools;
using System.Runtime.Remoting;
using System.IO;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Threading;
using System.Collections;
using System.Diagnostics;
using System.Text;
using System.Configuration;

namespace EEPNetAutoRun
{
    public partial class frmEEPNetAutoRun : Form
    {
        private SYS_LANGUAGE language;
        
        public frmEEPNetAutoRun()
        {
            language = CliSysMegLag.GetClientLanguage();
            InitializeComponent();
        }

        private string GetStartPath()
        {
            string path = Application.StartupPath.ToLower();
            string isFlowClient = ConfigurationManager.AppSettings["isFlowClient"].Trim().ToLower();
            if (isFlowClient == "true")
            {
                path = path.Substring(0, path.LastIndexOf("\\") + 1) + "eepnetflclient";
            }
            return path;
        }

        string Fixed = null;
        private void buttonGo_Click(object sender, EventArgs e)
        {
            Fixed = DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString();

            String testMode = this.cbTestMode.Text;
            if (testMode == null || testMode == "")
            {
                MessageBox.Show("Please choose one Test Mode first.");
                return;
            }

            int count = getInt(itbTU.Text);
            int Modules = getInt(itbTM.Text);
            if (Modules == 0) return;
            int Interval = getInt(itbInterval.Text);
            string LogFile = itbLTF.Text;
            String s = null;
            if (txtChooseXML.Text == null || txtChooseXML.Text.ToString() == "")
                s = GetStartPath() + "\\EEPNetAutoRun.xml";
            else
                s = txtChooseXML.Text.ToString();
            int maxl = 0, maxp = 0;
            int countl = 0, coutp = 0;
            if (File.Exists(s))
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(s);
                XmlNode el = xml.DocumentElement;
                foreach (XmlNode xNode in el.ChildNodes)
                {
                    if (xNode.Name.ToUpper().Equals("LOGININFORMATION"))
                    {
                        countl++;
                    }
                }
            }
            maxl = (count < countl) ? count : countl;
            string[] userID = new string[maxl];
            string[] password = new string[maxl];
            string[] db = new string[maxl];
            string[] solution = new string[maxl];
            string[] serverIPaddress = new string[maxl];
            string[] userMessage = new string[maxl];
            if (File.Exists(s))
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(s);
                XmlNode el = xml.DocumentElement;
                foreach (XmlNode xNode in el.ChildNodes)
                {
                    if (xNode.Name.ToUpper().Equals("PACKAGEINFORMATION"))
                    {
                        coutp++;
                    }
                }
            }
            maxp = (Modules < coutp) ? Modules : coutp;
            string[] packageName = new string[maxp];
            string[] formName = new string[maxp];
            string[] times = new string[maxp];
            string[] xomlName = new string[maxp];
            string[] packageMessage = new string[maxp];
            if (File.Exists(s))
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(s);
                XmlNode el = xml.DocumentElement;
                int indexl = 0, indexp = 0;

                foreach (XmlNode xNode in el.ChildNodes)
                {
                    String packageInfo = "";
                    if (xNode.Name.ToUpper().Equals("LOGININFORMATION"))
                    {
                        foreach (XmlNode xNodel in xNode)
                        {
                            if (indexl >= maxl)
                                break;
                            if (xNodel.Name.ToUpper().Equals("USERID"))
                            {
                                userID[indexl] = xNodel.InnerText.Trim();
                            }
                            else if (xNodel.Name.ToUpper().Equals("PASSWORD"))
                            {
                                password[indexl] = xNodel.InnerText.Trim();
                            }
                            else if (xNodel.Name.ToUpper().Equals("DATABASE"))
                            {
                                db[indexl] = xNodel.InnerText.Trim();
                            }
                            else if (xNodel.Name.ToUpper().Equals("SOLUTION"))
                            {
                                solution[indexl] = xNodel.InnerText.Trim();
                            }
                            else if (xNodel.Name.ToUpper().Equals("SERVERIPADDRESS"))
                            {
                                serverIPaddress[indexl] = xNodel.InnerText.Trim();
                            }
                            else if (xNodel.Name.ToUpper().Equals("PACKAGEINFO"))
                            {
                                packageInfo = xNodel.InnerText.Trim();
                            }
                            userMessage[indexl] = userID[indexl] + ";" + password[indexl] + ";" + db[indexl] + ";" + solution[indexl];
                        }
                        indexl++;
                    }
                    if (xNode.Name.ToUpper().Equals("PACKAGEINFORMATION"))
                    {
                        foreach (XmlNode xNodel in xNode)
                        {
                            if (indexp >= maxp)
                                break;
                            if (xNodel.Name.ToUpper().Equals("PACKAGENAME"))
                            {
                                packageName[indexp] = xNodel.InnerText.Trim();
                            }
                            else if (xNodel.Name.ToUpper().Equals("FORMNAME"))
                            {
                                formName[indexp] = xNodel.InnerText.Trim();
                            }
                            else if (xNodel.Name.ToUpper().Equals("TIMES"))
                            {
                                times[indexp] = xNodel.InnerText.Trim();
                            }
                            else if (xNodel.Name.ToUpper().Equals("XOMLNAME"))
                            {
                                xomlName[indexp] = xNodel.InnerText.Trim();
                            }
                            if (ConfigurationManager.AppSettings["isFlowClient"].ToLower().Trim() == "true")
                                packageMessage[indexp] = packageName[indexp] + ";" + formName[indexp] + ";" + times[indexp] + ";" + xomlName[indexp];
                            else
                                packageMessage[indexp] = packageName[indexp] + ";" + formName[indexp] + ";" + times[indexp];
                        }
                        indexp++;
                    }
                }
            }
            string package = null;
            for (int i = 0; i < packageMessage.Length; i++)
            {
                if(i < packageMessage.Length - 1)
                    package += packageMessage[i] + ",";
                else
                    package += packageMessage[i];
            }
            string strPath = Application.StartupPath + "\\";

            for (int i = 0; i < maxl; i++)
            {
                string arg = userMessage[i] + "!" + package + "!" + Interval + "!" + LogFile + "!" + Fixed + "!" + testMode;
                ProcessStartInfo ps = new ProcessStartInfo(strPath + "EEPNetRunStep.exe", arg);
                Process.Start(ps);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            string s = GetStartPath();
            if (s.Length > 0 && s[s.Length - 1] != '\\') s = s + "\\";

            if (File.Exists(s + Fixed + "AutoRunMessage.xml"))
            {
                File.Delete(s + Fixed + "AutoRunMessage.xml");
            }
            base.Dispose(disposing);
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

        static class Program
        {
            /// <summary>
            /// The main entry point for the application.
            /// </summary>
            [STAThread]
            static void Main()
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                CliUtils.fClientLang = GetClientLanguage();
                Application.Run(new frmEEPNetAutoRun());

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

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                itbLTF.Text = openFileDialog1.FileName;
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            XmlDocument DBXML = new XmlDocument();
            String s;
            ListViewItem aItem;
            ListViewItem.ListViewSubItem aSubItem;
            s = GetStartPath();
            if (s.Length > 0 && s[s.Length - 1] != '\\') s = s + "\\";

            if (File.Exists(s + Fixed + "AutoRunMessage.xml"))
            {
                try
                {
                    FileStream aFileStream = new FileStream(s + Fixed + "AutoRunMessage.xml", FileMode.Open, FileAccess.ReadWrite, FileShare.None);
                    DBXML.Load(aFileStream);
                    XmlNode aNode = DBXML.DocumentElement.FirstChild;
                    lvAutoRun.BeginUpdate();
                    try
                    {
                        lvAutoRun.Items.Clear();
                        while (aNode != null)
                        {
                            aItem = lvAutoRun.Items.Add(aNode.Attributes["UserId"].InnerText);
                            aSubItem = aItem.SubItems.Add(new ListViewItem.ListViewSubItem(aItem, ""));
                            aSubItem.Text = aNode.Attributes["Module"].InnerText;
                            aSubItem = aItem.SubItems.Add(new ListViewItem.ListViewSubItem(aItem, ""));
                            aSubItem.Text = aNode.Attributes["StartTime"].InnerText;
                            aSubItem = aItem.SubItems.Add(new ListViewItem.ListViewSubItem(aItem, ""));
                            aSubItem.Text = aNode.Attributes["Number"].InnerText;
                            aSubItem = aItem.SubItems.Add(new ListViewItem.ListViewSubItem(aItem, ""));
                            aSubItem.Text = aNode.Attributes["Times"].InnerText;
                            aSubItem = aItem.SubItems.Add(new ListViewItem.ListViewSubItem(aItem, ""));
                            aSubItem.Text = aNode.Attributes["Status"].InnerText;
                            //if (aNode.Attributes["Status"].InnerText == "100%" && aNode.Attributes["Flag"].InnerText == "")
                            //{
                            //    aNode.Attributes["CompleteTime"].InnerText = DateTime.Now.ToString();
                            //    aNode.Attributes["Flag"].InnerText = "1";
                            //    DBXML.Save(aFileStream);
                            //}
                            aSubItem = aItem.SubItems.Add(new ListViewItem.ListViewSubItem(aItem, ""));
                            aSubItem.Text = aNode.Attributes["CompleteTime"].InnerText;
                            aNode = aNode.NextSibling;
                        }
                    }
                    finally
                    {
                        lvAutoRun.EndUpdate();
                    }
                    aFileStream.Close();
                }
                catch
                { }
                finally
                {
                }
            }
        }

        private void frmEEPNetAutoRun_Load(object sender, EventArgs e)
        {
            language = CliUtils.fClientLang;
            string message = SysMsg.GetSystemMessage(language, "Srvtools", "EEPNetAutoRun", "LabelName");
            string[] user = message.Split(';');
            this.lblUsers.Text = user[0];
            this.lblModules.Text = user[1];
            this.lblInterval.Text = user[2];
            this.lblLog.Text = user[3];
            this.lblChoose.Text = user[4];
            
            XmlDocument DBXML = new XmlDocument();
            string s = GetStartPath();
            if (s.Length > 0 && s[s.Length - 1] != '\\') s = s + "\\";

            if (File.Exists(s + Fixed + "AutoRunMessage.xml"))
            {
                DBXML.Load(s + Fixed + "AutoRunMessage.xml");
                XmlNode aNode = DBXML.DocumentElement.FirstChild;
                DBXML.DocumentElement.RemoveAll();
                DBXML.Save(s + Fixed + "AutoRunMessage.xml");
            }
            lvAutoRun.Items.Clear();
            timer.Enabled = true;
            timer_Tick(timer, null);
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string userid = "";
            string module = "";
            if (lvAutoRun.SelectedItems.Count == 0 || lvAutoRun.SelectedItems[0].SubItems.Count == 0)
            { return; }
            else
            {
                userid = lvAutoRun.SelectedItems[0].SubItems[0].Text;
                module = lvAutoRun.SelectedItems[0].SubItems[1].Text;
            }
            
            XmlDocument DBXML = new XmlDocument();
            String s;
            s = GetStartPath();
            if (s.Length > 0 && s[s.Length - 1] != '\\') s = s + "\\";

            if (File.Exists(s + Fixed + "AutoRunMessage.xml"))
            {
                if (File.Exists(s + Fixed + "AutoRunMessage.lock")) return;
                StreamWriter sw = File.CreateText(s + "AutoRunMessage.lock");
                sw.Close();
                DBXML.Load(s + Fixed + "AutoRunMessage.xml");
                XmlNode aNode = DBXML.DocumentElement.FirstChild;
                while(aNode != null)
                {
                    if (aNode.Attributes["UserId"].InnerText == userid && aNode.Attributes["Module"].InnerText == module)
                        DBXML.DocumentElement.RemoveChild(aNode);
                    aNode = aNode.NextSibling;
                }
                DBXML.Save(s + Fixed + "AutoRunMessage.xml");
            }
            File.Delete(s + Fixed + "AutoRunMessage.lock");
            lvAutoRun.Items.Clear();
            timer.Enabled = true;
            timer_Tick(timer, null);
        }

        private void contextMenuStrip1_Opened(object sender, EventArgs e)
        {
            if (lvAutoRun.SelectedItems.Count == 0)
            {
                this.clearToolStripMenuItem.Enabled = false;
            }
            else
            {
                this.clearToolStripMenuItem.Enabled = true;
            }

        }

        private void clearAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            XmlDocument DBXML = new XmlDocument();
            String s;
            s = GetStartPath();
            if (s .Length > 0 && s[s.Length - 1] != '\\') s = s + "\\";

            if (File.Exists(s + Fixed + "AutoRunMessage.xml"))
            {
                File.Delete(s + Fixed + "AutoRunMessage.xml");
            }
            lvAutoRun.Items.Clear();
            timer.Enabled = true;
            timer_Tick(timer, null);
        }

        private void btnChooseXML_Click(object sender, EventArgs e)
        {
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                txtChooseXML.Text = openFileDialog2.FileName;
            }
        }
    }
}