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


namespace EEPNetAutoRunForWeb
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void buttonGo_Click(object sender, EventArgs e)
        {
            if (this.cbTestMode.Text == "")
            {
                MessageBox.Show("Please choose one Test Mode first.");
                return;
            }

            String testMode = this.cbTestMode.Text;
            int count = Convert.ToInt16(itbTU.Text);
            int Modules = Convert.ToInt16(itbTM.Text);
            if (Modules == 0) return;
            int interval = Convert.ToInt16(itbInterval.Text);
            string LogFile = itbLTF.Text;
            string Path = itbUrl.Text;
            if (Path.EndsWith("/"))
                Path = Path.Remove(Path.LastIndexOf('/'), 1);
            String strPath = "";
            if (txtChooseXML.Text == null || txtChooseXML.Text == "")
                strPath = Application.StartupPath + "\\EEPNetAutoRunForWeb.xml";
            else
                strPath = txtChooseXML.Text.ToString();
            int maxl = 0, maxp = 0;
            int countl = 0, coutp = 0;
            if (File.Exists(strPath))
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(strPath);
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
            if (File.Exists(strPath))
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(strPath);
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
            if (File.Exists(strPath))
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(strPath);
                XmlNode el = xml.DocumentElement;
                int indexl = 0, indexp = 0;

                foreach (XmlNode xNode in el.ChildNodes)
                {
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
                if (i < packageMessage.Length - 1)
                    package += packageMessage[i] + ",";
                else
                    package += packageMessage[i];
            }

            for (int i = 0; i < maxl; i++)
            {
                //Process.Start("IExplore.exe", Path + "/WebAutoRunStep.aspx?usermessage=" + userMessage[i] +
                //                               "&packagemessage=" + package + "&Interval=" + interval + "&Log=" + LogFile + "&Path=" + Path);
                Process.Start("IExplore.exe", Path + "/WebAutoRunStep.aspx?usermessage=" + userMessage[i] + "&packagemessage=" + package + "&Interval=" + interval + "&Log=" + LogFile + "&Path=" + Path + "&TestMode=" + testMode);
                Thread.Sleep(interval);
            }
        }

        private void btnChooseXML_Click(object sender, EventArgs e)
        {
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                txtChooseXML.Text = openFileDialog2.FileName;
            }
        }

        private void btnLogToFile_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                itbLTF.Text = openFileDialog1.FileName;
            }

        }
    }
}