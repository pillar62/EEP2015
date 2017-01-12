using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;

namespace Scheduling
{
    public partial class Monitor : Form
    {
        public Monitor()
        {
            InitializeComponent();
        }

        private XmlDocument xDoc;
        public Monitor(XmlDocument bDoc)
        {
            xDoc = bDoc;
            InitializeComponent();
        }

        private void Monitor_Load(object sender, EventArgs e)
        {
            LoadXml();
            this.timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            LoadXml();
        }

        private void LoadXml()
        {
            XmlDocument DBXML = new XmlDocument();
            String s = Application.StartupPath + "\\SchedulingMonitor.xml";
            ListViewItem aItem;
            ListViewItem.ListViewSubItem aSubItem;

            if (xDoc != null)
            {
                XmlNode aNode = xDoc.DocumentElement.FirstChild;
                lvMonitor.BeginUpdate();
                try
                {
                    lvMonitor.Items.Clear();
                    while (aNode != null)
                    {
                        if (aNode.Attributes["Active"].Value == "1")
                        {
                            aItem = lvMonitor.Items.Add(aNode.Attributes["SchedulName"].Value);
                            aSubItem = aItem.SubItems.Add(new ListViewItem.ListViewSubItem(aItem, ""));
                            aSubItem.Text = aNode.Attributes["StartDateTime"].Value;
                            aSubItem = aItem.SubItems.Add(new ListViewItem.ListViewSubItem(aItem, ""));
                            if (aNode.Attributes["IsRunning"].Value == "1")
                                aSubItem.Text = "";
                            else
                                aSubItem.Text = aNode.Attributes["LastDateTime"].Value;
                            aSubItem = aItem.SubItems.Add(new ListViewItem.ListViewSubItem(aItem, ""));
                            if (aNode.Attributes["IsRunning"].Value == "1")
                                aSubItem.Text = "running";
                            else
                                aSubItem.Text = "complete";
                            aSubItem = aItem.SubItems.Add(new ListViewItem.ListViewSubItem(aItem, ""));
                        }
                        aNode = aNode.NextSibling;
                    }
                }
                finally
                {
                    lvMonitor.EndUpdate();
                }
            }




            //if (File.Exists(s))
            //{
            //    try
            //    {
            //        FileStream aFileStream = new FileStream(s, FileMode.Open, FileAccess.Read, FileShare.None);
            //        DBXML.Load(aFileStream);
            //        XmlNode aNode = DBXML.DocumentElement.FirstChild;
            //        lvMonitor.BeginUpdate();
            //        try
            //        {
            //            lvMonitor.Items.Clear();
            //            while (aNode != null)
            //            {
            //                aItem = lvMonitor.Items.Add(aNode.Attributes["SchedulName"].InnerText);
            //                aSubItem = aItem.SubItems.Add(new ListViewItem.ListViewSubItem(aItem, ""));
            //                aSubItem.Text = aNode.Attributes["StartTime"].InnerText;
            //                aSubItem = aItem.SubItems.Add(new ListViewItem.ListViewSubItem(aItem, ""));
            //                aSubItem.Text = aNode.Attributes["EndTime"].InnerText;
            //                aSubItem = aItem.SubItems.Add(new ListViewItem.ListViewSubItem(aItem, ""));
            //                aSubItem.Text = aNode.Attributes["Status"].InnerText;
            //                aSubItem = aItem.SubItems.Add(new ListViewItem.ListViewSubItem(aItem, ""));
            //                aNode = aNode.NextSibling;
            //            }
            //        }
            //        finally
            //        {
            //            lvMonitor.EndUpdate();
            //        }
            //        aFileStream.Close();
            //    }
            //    catch
            //    { }
            //    finally
            //    {
            //    }
            //}
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String SchedulName = "";
            if (lvMonitor.SelectedItems.Count == 0 || lvMonitor.SelectedItems[0].SubItems.Count == 0)
            { return; }
            else
            {
                SchedulName = lvMonitor.SelectedItems[0].SubItems[0].Text;
            }

            XmlDocument DBXML = new XmlDocument();
            String s = Application.StartupPath + "\\SchedulingMonitor.xml";

            if (File.Exists(s))
            {
                FileStream fs = File.Open(s, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                DBXML.Load(fs);
                XmlNode aNode = DBXML.DocumentElement.FirstChild;
                while (aNode != null)
                {
                    if (aNode.Attributes["SchedulName"].InnerText == SchedulName)
                        DBXML.DocumentElement.RemoveChild(aNode);
                    aNode = aNode.NextSibling;
                }
                fs.Close();
                DBXML.Save(s);
            }
            lvMonitor.Items.Clear();
            timer1.Enabled = true;
            timer1_Tick(timer1, null);
        }

        private void clearAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            XmlDocument DBXML = new XmlDocument();
            String s = Application.StartupPath + "\\SchedulingMonitor.xml";
            if (File.Exists(s))
            {
                File.Delete(s);
            }
            this.lvMonitor.Items.Clear();
            timer1.Enabled = true;
            timer1_Tick(timer1, null);
        }
    }
}