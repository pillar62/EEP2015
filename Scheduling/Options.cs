using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using Srvtools;

namespace Scheduling
{
    public partial class frmOptions : Form
    {
        public frmOptions()
        {
            InitializeComponent();
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            String s = Application.StartupPath + "\\SchedulingOptions.xml";
            XmlDocument DBXML = new XmlDocument();
            FileStream aFileStream;
            String Interval = this.tbInterval.Text;
            String DayTime = this.tbDayTime.Text;
            String NightTime = this.tbNightTime.Text;
            String Holiday = this.tbHoliday.Text;
            String ServerIP = this.tbServerIP.Text;
            String ServerPort = this.tbServerPort.Text;
            String UserID = this.tbUserID.Text;
            //A6-Sensitive Data Exposure
            //String Password = this.tbPassword.Text;
            Boolean StartActive = this.cbStartActive.Checked;
            //2007.12.28新加属性，在读取原来的xml的时候可能因为读不到新属性而报错
            String Email = this.tbEmail.Text;
            String EmailPwd = this.tbEmailPwd.Text;
            String Host = this.tbHost.Text;
            String Port = this.tbPort.Text;
            Boolean ssl = this.cbSSL.Checked;
            String SMS = this.tbSMS.Text;

            if (!File.Exists(s))
            {
                try
                {
                    using (aFileStream = new FileStream(s, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                    {
                        try
                        {
                            XmlTextWriter w = new XmlTextWriter(aFileStream, new System.Text.ASCIIEncoding());
                            w.Formatting = Formatting.Indented;
                            w.WriteStartElement("InfolightSchedulingOptions");
                            w.WriteEndElement();
                            w.Close();
                        }
                        finally
                        {
                            aFileStream.Close();
                        }
                    }
                }
                catch (Exception er) { string str = er.Message; }
            }

            try
            {
                using (aFileStream = new FileStream(s, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    try
                    {
                        DBXML.Load(aFileStream);
                        XmlNode aNode = null;

                        aNode = DBXML.DocumentElement.ChildNodes[0];

                        if (aNode == null)
                        {
                            XmlElement elem = DBXML.CreateElement("String");
                            XmlAttribute attr = DBXML.CreateAttribute("Interval");
                            attr.Value = Interval;
                            elem.Attributes.Append(attr);

                            attr = DBXML.CreateAttribute("DayTime");
                            attr.Value = DayTime;
                            elem.Attributes.Append(attr);

                            attr = DBXML.CreateAttribute("NightTime");
                            attr.Value = NightTime;
                            elem.Attributes.Append(attr);

                            attr = DBXML.CreateAttribute("Holiday");
                            attr.Value = Holiday;
                            elem.Attributes.Append(attr);

                            attr = DBXML.CreateAttribute("ServerIP");
                            attr.Value = ServerIP;
                            elem.Attributes.Append(attr);

                            attr = DBXML.CreateAttribute("ServerPort");
                            attr.Value = ServerPort;
                            elem.Attributes.Append(attr);

                            attr = DBXML.CreateAttribute("UserID");
                            attr.Value = UserID;
                            elem.Attributes.Append(attr);

                            attr = DBXML.CreateAttribute("Password");
                            attr.Value = this.tbPassword.Text;
                            elem.Attributes.Append(attr);

                            attr = DBXML.CreateAttribute("StartActive");
                            attr.Value = StartActive.ToString();
                            elem.Attributes.Append(attr);

                            attr = DBXML.CreateAttribute("Email");
                            attr.Value = Email.ToString();
                            elem.Attributes.Append(attr);

                            attr = DBXML.CreateAttribute("EmailPwd");
                            attr.Value = EmailPwd.ToString();
                            elem.Attributes.Append(attr);

                            attr = DBXML.CreateAttribute("Host");
                            attr.Value = Host.ToString();
                            elem.Attributes.Append(attr);

                            attr = DBXML.CreateAttribute("Port");
                            attr.Value = Port.ToString();
                            elem.Attributes.Append(attr);

                            attr = DBXML.CreateAttribute("SSL");
                            attr.Value = ssl.ToString();
                            elem.Attributes.Append(attr);

                            attr = DBXML.CreateAttribute("SMS");
                            attr.Value = SMS.ToString();
                            elem.Attributes.Append(attr);

                            DBXML.DocumentElement.AppendChild(elem);
                        }
                        else
                        {
                            aNode.Attributes["Interval"].InnerText = Interval;
                            aNode.Attributes["DayTime"].InnerText = DayTime;
                            aNode.Attributes["NightTime"].InnerText = NightTime;
                            aNode.Attributes["Holiday"].InnerText = Holiday;
                            aNode.Attributes["ServerIP"].InnerText = ServerIP;
                            if (aNode.Attributes["ServerPort"] == null)
                            {
                                XmlAttribute attr = DBXML.CreateAttribute("ServerPort");
                                attr.Value = ServerPort;
                                aNode.Attributes.Append(attr);
                            }
                            else
                                aNode.Attributes["ServerPort"].InnerText = ServerPort;
                            aNode.Attributes["UserID"].InnerText = UserID;
                            aNode.Attributes["Password"].InnerText = this.tbPassword.Text;
                            aNode.Attributes["StartActive"].InnerText = StartActive.ToString();
                            aNode.Attributes["Email"].InnerText = Email;
                            aNode.Attributes["EmailPwd"].InnerText = EmailPwd;
                            aNode.Attributes["Host"].InnerText = Host;
                            aNode.Attributes["Port"].InnerText = Port;
                            //public version
                            if (aNode.Attributes["SSL"] == null)
                            {
                                XmlAttribute attr = DBXML.CreateAttribute("SSL");
                                attr.Value = ssl.ToString();
                                aNode.Attributes.Append(attr);
                            }
                            else
                                aNode.Attributes["SSL"].InnerText = ssl.ToString();
                            //CMC
                            if (aNode.Attributes["SMS"] == null)
                            {
                                XmlAttribute attr = DBXML.CreateAttribute("SMS");
                                attr.Value = SMS.ToString();
                                aNode.Attributes.Append(attr);
                            }
                            else
                                aNode.Attributes["SMS"].InnerText = SMS;
                        }
                    }
                    finally
                    {
                        aFileStream.Close();
                    }
                }
                DBXML.Save(s);
            }
            catch (Exception er) { string str = er.Message; }
            this.Close();
        }

        private void frmOptions_Load(object sender, EventArgs e)
        {
            string s = Application.StartupPath + "\\SchedulingOptions.xml";
            if (File.Exists(s))
            {
                XmlDocument DBXML = new XmlDocument();
                FileStream aFileStream = new FileStream(s, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                DBXML.Load(aFileStream);
                XmlNode aNode = DBXML.DocumentElement.FirstChild;
                try
                {
                    this.tbInterval.Text = aNode.Attributes["Interval"].Value;
                    this.tbDayTime.Text = aNode.Attributes["DayTime"].Value;
                    this.tbNightTime.Text = aNode.Attributes["NightTime"].Value;
                    this.tbHoliday.Text = aNode.Attributes["Holiday"].Value;
                    this.tbServerIP.Text = aNode.Attributes["ServerIP"].Value;
                    if (aNode.Attributes["ServerPort"] != null)
                        this.tbServerPort.Text = aNode.Attributes["ServerPort"].Value;
                    this.tbUserID.Text = aNode.Attributes["UserID"].Value;
                    this.tbPassword.Text = aNode.Attributes["Password"].Value;
                    if (string.Compare(aNode.Attributes["StartActive"].Value, "false", true) == 0)//IgnoreCase
                        this.cbStartActive.Checked = false;
                    else
                        this.cbStartActive.Checked = true;
                    this.tbEmail.Text = aNode.Attributes["Email"].Value;
                    this.tbEmailPwd.Text = aNode.Attributes["EmailPwd"].Value;
                    this.tbHost.Text = aNode.Attributes["Host"].Value;
                    this.tbPort.Text = aNode.Attributes["Port"].Value;
                    if (aNode.Attributes["SSL"] != null && string.Compare(aNode.Attributes["SSL"].Value, "false", true) == 0)//IgnoreCase
                        this.cbSSL.Checked = false;
                    else
                        this.cbSSL.Checked = true;
                    if (aNode.Attributes["SMS"] != null)
                        this.tbSMS.Text = aNode.Attributes["SMS"].Value;
                }
                finally
                {
                    aFileStream.Close();
                }
            }
        }

        private void btnTestIP_Click(object sender, EventArgs e)
        {
            try
            {
                EEPRemoteModule module = Activator.GetObject(typeof(EEPRemoteModule),
                    string.Format("http://{0}:{1}/InfoRemoteModule.rem", tbServerIP.Text, tbServerPort.Text)) as EEPRemoteModule;
                module.ToString();
                MessageBox.Show("Connect succeed.");
            }
            catch
            {
                MessageBox.Show("Can't connect to " + tbServerIP.Text);
            }
        }
    }
}