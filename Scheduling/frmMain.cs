using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Collections;
using Srvtools;
using System.Runtime.Remoting;
using System.Diagnostics;
using System.Net.Mail;
using System.Web;
using System.Text.RegularExpressions;
using System.Threading;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Net;

namespace Scheduling
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private string state = "";
        string Mode = "";
        int ErrorCount = 5;
        private void TSsetting_Click(object sender, EventArgs e)
        {
            this.Show();
            this.ShowInTaskbar = true;
            this.WindowState = System.Windows.Forms.FormWindowState.Normal;
        }

        private void TShide_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void TSclose_Click(object sender, EventArgs e)
        {
            CloseOut();

            components.Dispose();
            base.Dispose(true);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CliUtils.fClientLang = GetClientLanguage();
            RemoveFormText();
            this.Text += "(Stop)";
            TSactive.Checked = false;
            TSstop.Checked = true;
            String ss;
            ss = Application.StartupPath + "\\";
            RemotingConfiguration.Configure(Application.StartupPath + "\\Scheduling.exe.config", false);

            this.Hide();
            this.ShowInTaskbar = false;
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;

            findScheduling();
            checkStartActive();
        }

        [DllImport("KERNEL32.DLL", EntryPoint = "GetThreadLocale", SetLastError = true,
            CharSet = CharSet.Unicode, ExactSpelling = true,
            CallingConvention = CallingConvention.StdCall)]
        public static extern uint GetThreadLocale();
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

        private void checkStartActive()
        {
            string s = Application.StartupPath + "\\SchedulingOptions.xml";
            string isStart = "";
            if (File.Exists(s))
            {
                XmlDocument DBXML = new XmlDocument();
                using (FileStream aFileStream = new FileStream(s, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    DBXML.Load(aFileStream);
                    XmlNode aNode = DBXML.DocumentElement.FirstChild;
                    isStart = aNode.Attributes["StartActive"].Value;
                    aFileStream.Close();
                }
            }

            if (string.Compare(isStart, "true", true) == 0)//IgnoreCase
                Start();
        }

        private void findScheduling()
        {
            string s = Application.StartupPath + "\\Scheduling.xml";
            if (File.Exists(s))
            {
                if (this.Text.Contains("Active"))
                {
                    XmlNode bNode = bDBXML.DocumentElement.FirstChild;
                    lbScheduling.BeginUpdate();
                    lbScheduling.Items.Clear();
                    while (bNode != null)
                    {
                        lbScheduling.Items.Add(bNode.Attributes["SchedulName"].Value);
                        bNode = bNode.NextSibling;
                    }
                    lbScheduling.EndUpdate();
                }
                else
                {
                    XmlDocument DBXML = new XmlDocument();
                    using (FileStream aFileStream = new FileStream(s, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
                    {
                        DBXML.Load(aFileStream);
                        XmlNode aNode = DBXML.DocumentElement.FirstChild;
                        lbScheduling.BeginUpdate();
                        try
                        {
                            lbScheduling.Items.Clear();
                            while (aNode != null)
                            {
                                lbScheduling.Items.Add(aNode.Attributes["SchedulName"].Value);
                                aNode = aNode.NextSibling;
                            }
                        }
                        finally
                        {
                            lbScheduling.EndUpdate();
                            aFileStream.Close();
                        }
                    }
                }
            }
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
                this.Hide();
        }

        private void TSMainNew_Click(object sender, EventArgs e)
        {
            if (this.Text.Contains("(Active)")) state = "Active";
            Stop();
            openAll();
            clearAll();
        }

        private void openAll()
        {
            Mode = "Changing";
            //this.tbCycle.Enabled = true;
            //this.tbCycleHour.Enabled = true;
            this.LDescription.Enabled = true;
            this.tbErr.Enabled = true;
            this.tbLastDateTime.Enabled = true;
            this.tbName.Enabled = true;
            //this.tbParameters.Enabled = true;
            //this.tbServerMethod.Enabled = true;
            this.tbDescriotion.Enabled = true;
            this.cbActive.Enabled = true;
            this.cbCycleUnit.Enabled = true;
            this.cbLog.Enabled = true;
            this.cbSchedulingMode.Enabled = true;
            this.cbWhen.Enabled = true;
            this.cbDataBase.Enabled = true;
            this.cbSolution.Enabled = true;
            this.btnServerMethod.Enabled = true;
            this.cbSendMailMode.Enabled = true;
            //CMC
            this.tbErrN.Enabled = true;
            this.tbUserMsg.Enabled = true;
        }

        private void clearAll()
        {
            this.tbCycle.Text = "";
            this.tbCycleHour.Text = "";
            this.tbErr.Text = "";
            this.tbLastDateTime.Text = "";
            this.tbName.Text = "";
            this.tbParameters.Text = "";
            this.tbServerMethod.Text = "";
            this.tbDescriotion.Text = "";
            this.cbActive.Checked = false;
            this.cbCycleUnit.Text = "";
            this.cbLog.SelectedIndex = 0;
            this.cbSchedulingMode.Text = "";
            this.cbWhen.Text = "";
            this.cbSolution.Text = "";
            this.cbDataBase.Text = "";
            this.cbSendMailMode.SelectedIndex = 0;
            //CMC
            this.tbErrN.Text = "";
            this.tbUserMsg.Text = "";
        }

        private void closeAll()
        {
            this.tbCycle.Enabled = false;
            this.tbCycleHour.Enabled = false;
            this.LDescription.Enabled = false;
            this.tbErr.Enabled = false;
            this.tbLastDateTime.Enabled = false;
            this.tbName.Enabled = false;
            this.tbParameters.Enabled = false;
            this.tbServerMethod.Enabled = false;
            this.tbDescriotion.Enabled = false;
            this.cbActive.Enabled = false;
            this.cbCycleUnit.Enabled = false;
            this.cbLog.Enabled = false;
            this.cbSchedulingMode.Enabled = false;
            this.cbWhen.Enabled = false;
            this.cbDataBase.Enabled = false;
            this.cbSolution.Enabled = false;
            this.btnServerMethod.Enabled = false;
            this.cbSendMailMode.Enabled = false;
            //CMC
            this.tbErrN.Enabled = false;
            this.tbUserMsg.Enabled = false;
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            this.Show();
            this.ShowInTaskbar = true;
            this.WindowState = System.Windows.Forms.FormWindowState.Normal;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (Mode == "Changing" && tbName.Text.Trim() != "")
                Save();
        }

        private void lbScheduling_SelectedIndexChanged(object sender, EventArgs e)
        {
            Mode = "Browsing";
            closeAll();
            setScheduling();
        }

        private void setScheduling()
        {
            string s = Application.StartupPath + "\\Scheduling.xml";
            if (File.Exists(s))
            {
                XmlDocument DBXML = new XmlDocument();
                using (FileStream aFileStream = new FileStream(s, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    if (this.Text.Contains("Active"))
                    {
                        DBXML = bDBXML;
                    }
                    else
                    {
                        DBXML.Load(aFileStream);
                    }
                    XmlNode aNode = DBXML.DocumentElement.FirstChild;
                    try
                    {
                        while (aNode != null)
                        {
                            if (this.lbScheduling.SelectedItem != null && aNode.Attributes["SchedulName"].Value == this.lbScheduling.SelectedItem.ToString())
                            {
                                if (aNode.Attributes["Cycle"] != null)
                                    this.tbCycle.Text = aNode.Attributes["Cycle"].Value;
                                if (aNode.Attributes["CycleHour"] != null)
                                    this.tbCycleHour.Text = aNode.Attributes["CycleHour"].Value;
                                if (aNode.Attributes["Error"] != null)
                                    this.tbErr.Text = aNode.Attributes["Error"].Value;
                                if (aNode.Attributes["LastDateTime"] != null)
                                    this.tbLastDateTime.Text = aNode.Attributes["LastDateTime"].Value;
                                if (aNode.Attributes["SchedulName"] != null)
                                    this.tbName.Text = aNode.Attributes["SchedulName"].Value;
                                if (aNode.Attributes["Parameters"] != null)
                                    this.tbParameters.Text = aNode.Attributes["Parameters"].Value;
                                if (aNode.Attributes["ServerMethod"] != null)
                                    this.tbServerMethod.Text = aNode.Attributes["ServerMethod"].Value;
                                if (aNode.Attributes["Description"] != null)
                                    this.tbDescriotion.Text = aNode.Attributes["Description"].Value;
                                if (aNode.Attributes["Active"] != null)
                                {
                                    if (aNode.Attributes["Active"].Value == "1")
                                        cbActive.Checked = true;
                                    else
                                        cbActive.Checked = false;
                                }
                                if (aNode.Attributes["SchedulingMode"] != null)
                                    this.cbSchedulingMode.Text = aNode.Attributes["SchedulingMode"].Value;
                                if (aNode.Attributes["CycleUnit"] != null)
                                    this.cbCycleUnit.Text = aNode.Attributes["CycleUnit"].Value;
                                if (aNode.Attributes["LogScheduling"] != null)
                                    this.cbLog.Text = aNode.Attributes["LogScheduling"].Value;
                                if (aNode.Attributes["When"] != null)
                                    this.cbWhen.Text = aNode.Attributes["When"].Value;
                                if (aNode.Attributes["Database"] != null)
                                    this.cbDataBase.Text = aNode.Attributes["Database"].Value;
                                if (aNode.Attributes["Solution"] != null)
                                    this.cbSolution.Text = aNode.Attributes["Solution"].Value;
                                if (aNode.Attributes["SendMailMode"] != null)
                                    this.cbSendMailMode.Text = aNode.Attributes["SendMailMode"].Value;
                                //CMC
                                if (aNode.Attributes["SendMailNumber"] != null)
                                {
                                    this.tbErrN.Text = aNode.Attributes["SendMailNumber"].Value;
                                }
                                else
                                {
                                    this.tbErrN.Text = "";
                                }
                                if (aNode.Attributes["Subject"] != null)
                                {
                                    this.tbUserMsg.Text = aNode.Attributes["Subject"].Value;
                                }
                                else
                                {
                                    this.tbUserMsg.Text = "";
                                }
                            }
                            aNode = aNode.NextSibling;
                        }
                    }
                    finally
                    {
                        aFileStream.Close();
                    }
                }
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            Delete();
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            Modify();
        }

        private void cbSchedulingMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            checkSchedulingMode();
        }

        private void checkSchedulingMode()
        {
            if (Mode == "Changing")
            {
                this.tbParameters.Enabled = true;
                this.tbServerMethod.Enabled = true;
                if (cbSchedulingMode.Text == "EXE")
                {
                    this.cbDataBase.Enabled = false;
                    this.cbSolution.Enabled = false;
                }
                else
                {
                    this.cbDataBase.Enabled = true;
                    this.cbSolution.Enabled = true;
                }
            }
        }

        private void checkCycleUnit()
        {
            if (Mode == "Changing")
            {
                this.tbCycle.Enabled = true;
                if (cbCycleUnit.Text == "Monthly" || cbCycleUnit.Text == "Weekly")
                    this.tbCycleHour.Enabled = true;
                else
                    this.tbCycleHour.Enabled = false;
            }
        }

        private void cbCycleUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            checkCycleUnit();
        }

        private void TSactive_Click(object sender, EventArgs e)
        {
            Start();
        }

        FileStream bFileStream;
        XmlDocument bDBXML;
        private void Start()
        {
            string[] OptionMessage = getOption();
            timer.Interval = Convert.ToInt16(OptionMessage[6]) * 1000;

            if (!isRegister)
                isRegister = Register(false, OptionMessage[2], OptionMessage[12]);

            String sPath = Application.StartupPath + "\\Scheduling.xml";
            if (File.Exists(sPath))
            {
                if (bDBXML == null)
                {
                    bDBXML = new XmlDocument();
                    bFileStream = new FileStream(sPath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                    bDBXML.Load(bFileStream);
                }
            }
            else
            {
                MessageBox.Show("Schedulings are not exists, please set it first.");
                return;
            }

            RemoveFormText();
            this.Text += "(Active)";
            TSstop.Checked = false;
            TSactive.Checked = true;

            //Assembly a = this.GetType().Assembly;
            //Stream st = a.GetManifestResourceStream("Scheduling.Resources.Scheduling16.ico");
            //Image image = this.imageList1.Images["Scheduling16.ico"];
            //MemoryStream mstream = new MemoryStream();
            //image.Save(mstream, ImageFormat.Png);
            //Icon icon = Icon.FromHandle(new Bitmap(mstream).GetHicon());
            //mstream.Close();
            //notifyIcon1.Icon = icon;
            ChangeIcon("Scheduling16.ico");

            String[] option = getOption();
            timer.Interval = Convert.ToInt16(option[6]) * 1000;
            timer.Start();
            timerConnect.Start();
        }

        private void TSstop_Click(object sender, EventArgs e)
        {
            Stop();
        }

        private void Stop()
        {
            CloseOut();

            RemoveFormText();
            this.Text += "(Stop)";
            TSactive.Checked = false;
            TSstop.Checked = true;
            timer.Stop();
            timerConnect.Stop();

            //Assembly a = this.GetType().Assembly;
            //Stream st = a.GetManifestResourceStream("Scheduling.Resources.SchedulingStop16.ico");
            //Icon ic2 = new Icon(st);
            //notifyIcon1.Icon = ic2;
            //Image image = this.imageList1.Images["SchedulingStop16.ico"];
            //MemoryStream mstream = new MemoryStream();
            //image.Save(mstream, ImageFormat.Png);
            //Icon icon = Icon.FromHandle(new Bitmap(mstream).GetHicon());
            //mstream.Close();
            //notifyIcon1.Icon = icon;
            ChangeIcon("SchedulingStop16.ico");
        }


        private void CloseOut()
        {
            if (this.Text.Contains("(Active)"))
            {
                XmlNode bNode = bDBXML.DocumentElement.FirstChild;
                String sPath = Application.StartupPath + "\\Scheduling.xml";
                while (bNode != null)
                {
                    if (bNode.Attributes["IsRunning"].Value == "1")
                        LogOut();

                    bNode.Attributes["IsRunning"].Value = "0";
                    bNode.Attributes["ErrorCount"].Value = "0";
                    bNode = bNode.NextSibling;
                }
                bFileStream.Close();
                bDBXML.Save(sPath);
                bDBXML = null;
            }
        }


        private void RemoveFormText()
        {
            if (this.Text.Contains("("))
            {
                this.Text = this.Text.Remove(this.Text.IndexOf('('));
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < lbScheduling.Items.Count; i++)
            {
                checkScheduling(this.lbScheduling.Items[i].ToString(), false);
            }
        }

        private void checkScheduling(string Name, bool isTest)
        {
            XmlNode bNode = bDBXML.DocumentElement.FirstChild;
            try
            {
                while (bNode != null)
                {
                    DateTime dtNow = DateTime.Now;
                    if (bNode.Attributes["SchedulName"].Value == Name)
                    {
                        if (isTest)
                        {
                            Execute(dtNow, bNode, isTest);
                            bFileStream.Close();
                            return;
                        }

                        if (bNode.Attributes["Active"].Value == "1")
                        {
                            if (bNode.Attributes["IsRunning"] != null && bNode.Attributes["IsRunning"].Value == "1")
                            {
                                //switch (bNode.Attributes["LogScheduling"].Value)
                                //{
                                //    case ("All"):
                                //        Log(bNode, "Running", "This Scheduling is running.");
                                //        break;
                                //}
                                return;
                            }
                            bNode.Attributes["IsRunning"].Value = "1";
                            bool isExcute = false;

                            DateTime dtLast = new DateTime();
                            if (bNode.Attributes["LastDateTime"].Value == "")
                                bNode.Attributes["LastDateTime"].Value = dtLast.ToString("yyyy/MM/dd HH:mm:ss.fff");
                            dtLast = Convert.ToDateTime(bNode.Attributes["LastDateTime"].Value);
                            DateTime CycleHour = new DateTime();
                            if (bNode.Attributes["CycleHour"].Value != "")
                                CycleHour = Convert.ToDateTime(bNode.Attributes["CycleHour"].Value[0] + "" + bNode.Attributes["CycleHour"].Value[1] + ":" + bNode.Attributes["CycleHour"].Value[2] + "" + bNode.Attributes["CycleHour"].Value[3]);

                            switch (bNode.Attributes["CycleUnit"].Value)
                            {
                                case ("InterVal"):
                                    if (((TimeSpan)(dtNow - dtLast)).TotalSeconds * 1000 > Convert.ToInt32(bNode.Attributes["Cycle"].Value))
                                    {
                                        Execute(dtNow, bNode);
                                        isExcute = true;
                                    }
                                    break;
                                case ("Daily"):
                                    if (((TimeSpan)(dtNow - dtLast)).TotalDays >= 1)
                                    {
                                        Execute(dtNow, bNode);
                                        isExcute = true;
                                        break;
                                    }

                                    string[] hour = bNode.Attributes["Cycle"].Value.Split(',');
                                    double LastHour = dtLast.Hour + Convert.ToDouble(dtLast.Minute) / 60.0;
                                    double NowHour = dtNow.Hour + Convert.ToDouble(dtNow.Minute) / 60.0;
                                    if (dtNow.Date > dtLast.Date)
                                    {
                                        for (int j = 0; j < hour.Length; j++)
                                            if (LastHour < Convert.ToDouble(hour[j]))
                                            {
                                                Execute(dtNow, bNode);
                                                isExcute = true;
                                                break;
                                            }

                                        LastHour = -1;
                                    }

                                    for (int j = 0; j < hour.Length; j++)
                                        if (dtNow > dtLast && LastHour < Convert.ToDouble(hour[j]) && NowHour >= Convert.ToDouble(hour[j]))
                                        {
                                            Execute(dtNow, bNode);
                                            isExcute = true;
                                            break;
                                        }
                                    break;
                                case ("Monthly")://2015/9/10 上午 04:02:58 ExecuteTime: 2015/10/11 上午 04:06:13
                                    if (dtNow.Hour * 60 + dtNow.Minute >= CycleHour.Hour * 60 + CycleHour.Minute)
                                    {
                                        int theDay = 31;
                                        switch (dtLast.Month.ToString())
                                        {
                                            case "1":
                                            case "3":
                                            case "5":
                                            case "7":
                                            case "8":
                                            case "10":
                                            case "12":
                                                theDay = 31;
                                                break;
                                            case "2":
                                                if (dtLast.Year % 400 == 0 || (dtLast.Year % 4 == 0 && dtLast.Year % 100 != 0))
                                                {
                                                    theDay = 29;
                                                }
                                                else
                                                {
                                                    theDay = 28;
                                                }
                                                break;
                                            case "4":
                                            case "6":
                                            case "9":
                                            case "11":
                                                theDay = 30;
                                                break;
                                        }
                                        if (((TimeSpan)(dtNow - dtLast)).TotalDays >= theDay)
                                        {
                                            Execute(dtNow, bNode);
                                            isExcute = true;
                                        }

                                        string[] day = bNode.Attributes["Cycle"].Value.Split(',');
                                        int LastDay = dtLast.Day;
                                        if (!isExcute)
                                        {
                                            if (dtNow.Year > dtLast.Year || dtNow.Month > dtLast.Month)
                                            {
                                                for (int j = 0; j < day.Length; j++)
                                                    if (LastDay < Convert.ToInt32(day[j]))
                                                    {
                                                        Execute(dtNow, bNode);
                                                        isExcute = true;
                                                        break;
                                                    }

                                                LastDay = -1;
                                            }
                                        }

                                        if (!isExcute)
                                        {
                                            for (int j = 0; j < day.Length; j++)
                                                if (dtNow > dtLast && LastDay < Convert.ToInt32(day[j]) && dtNow.Day >= Convert.ToInt32(day[j]))
                                                {
                                                    Execute(dtNow, bNode);
                                                    isExcute = true;
                                                    break;
                                                }
                                        }
                                    }
                                    break;
                                case ("Weekly"):
                                    if (dtNow.Hour * 60 + dtNow.Minute >= CycleHour.Hour * 60 + CycleHour.Minute)
                                    {
                                        if (((TimeSpan)(dtNow - dtLast)).TotalDays >= 7)
                                        {
                                            Execute(dtNow, bNode);
                                            isExcute = true;
                                        }

                                        string[] day = bNode.Attributes["Cycle"].Value.Split(',');
                                        int LastDay = changeDays(dtLast.DayOfWeek.ToString());
                                        int NowDay = changeDays(dtNow.DayOfWeek.ToString());
                                        if (!isExcute)
                                        {
                                            if (LastDay > NowDay)
                                            {
                                                for (int j = 0; j < day.Length; j++)
                                                    if (LastDay < Convert.ToInt32(day[j]))
                                                    {
                                                        Execute(dtNow, bNode);
                                                        isExcute = true;
                                                        break;
                                                    }
                                                LastDay = -1;
                                            }
                                        }

                                        if (!isExcute)
                                        {
                                            for (int j = 0; j < day.Length; j++)
                                                if (dtNow > dtLast && LastDay < Convert.ToInt32(day[j]) && NowDay >= Convert.ToInt32(day[j]))
                                                {
                                                    Execute(dtNow, bNode);
                                                    isExcute = true;
                                                    break;
                                                }
                                        }
                                    }
                                    break;
                            }
                            if (!isExcute)
                            {
                                bNode.Attributes["IsRunning"].Value = "0";
                            }
                        }
                    }
                    bNode = bNode.NextSibling;
                }
            }
            finally
            {
                //aFileStream.Close();
                //DBXML.Save(s);
            }
        }

        private int changeDays(string str)
        {
            switch (str)
            {
                case ("Sunday"): return 0;
                case ("Monday"): return 1;
                case ("Tuesday"): return 2;
                case ("Wednesday"): return 3;
                case ("Thursday"): return 4;
                case ("Friday"): return 5;
                case ("Saturday"): return 6;
            }
            return -1;
        }

        private void Execute(DateTime dtNow, XmlNode aNode)
        {
            Execute(dtNow, aNode, false);
        }

        private void Execute(DateTime dtNow, XmlNode aNode, bool isTest)
        {
            String[] option = getOption();
            String DayTime = option[7];
            String NightTime = option[8];
            String[] holidays = option[9].Split(',');
            ArrayList Holiday = new ArrayList();
            foreach (string str in holidays)
                Holiday.Add(str);

            DateTime dtDay = Convert.ToDateTime(DayTime[0] + "" + DayTime[1] + ":" + DayTime[2] + "" + DayTime[3]);
            DateTime dtNight = Convert.ToDateTime(NightTime[0] + "" + NightTime[1] + ":" + NightTime[2] + "" + NightTime[3]);

            ParameterizedThreadStart pts = new ParameterizedThreadStart(Do);
            Thread t = new Thread(pts);
            t.IsBackground = true;
            switch (aNode.Attributes["When"].Value.ToLower())
            {
                case ("all"):
                    t.Start(aNode);

                    //Do(aNode);
                    break;
                case ("night"):
                    if (dtNow.TimeOfDay >= dtNight.TimeOfDay || dtNow.TimeOfDay < dtDay.TimeOfDay)
                    {
                        t.Start(aNode);
                    }
                    else
                    {
                        aNode.Attributes["IsRunning"].Value = "0";
                    }

                    //Do(aNode, isTest);
                    break;
                case ("day"):
                    if (dtNow.TimeOfDay < dtNight.TimeOfDay || dtNow.TimeOfDay >= dtDay.TimeOfDay)
                    {
                        t.Start(aNode);
                    }
                    else
                    {
                        aNode.Attributes["IsRunning"].Value = "0";
                    }
                    //Do(aNode, isTest);
                    break;
                case ("holiday"):
                    bool holiday = false;
                    int Now = changeDays(dtNow.DayOfWeek.ToString());
                    for (int j = 0; j < Holiday.Count; j++)
                    {
                        if (Now == Convert.ToInt32(Holiday[j]))
                        {
                            holiday = true;
                            t.Start(aNode);
                        }
                    }
                    if (!holiday)
                    {
                        aNode.Attributes["IsRunning"].Value = "0";
                    }

                    //Do(aNode, isTest);
                    break;
                case ("workday"):
                    bool work = true;
                    int Day = changeDays(dtNow.DayOfWeek.ToString());
                    for (int j = 0; j < Holiday.Count; j++)
                    {
                        if (Day == Convert.ToInt32(Holiday[j]))
                        { work = false; break; }
                    }
                    if (work == true)
                    {
                        t.Start(aNode);
                    }
                    else
                    {
                        aNode.Attributes["IsRunning"].Value = "0";
                    }

                    //Do(aNode, isTest);
                    break;
            }
        }

        private void Do(object Node)
        {
            XmlNode aNode = Node as XmlNode;
            if (aNode.Attributes["SchedulingMode"].Value.Contains("Server Method") || aNode.Attributes["SchedulingMode"].Value.Contains("Server FLMethod"))
            {
                string DB = aNode.Attributes["Database"].Value;
                string Solution = aNode.Attributes["Solution"].Value;
                string[] logMessage = new string[2];
                try
                {
                    aNode.Attributes["StartDateTime"].Value = DateTime.Now.ToString();
                    logMessage = Login(DB, Solution);
                }
                catch
                {
                    logMessage[0] = "false";
                }

                if (logMessage[0] == "false")
                {
                    int errCount = Convert.ToInt16(aNode.Attributes["ErrorCount"].Value) + 1;
                    aNode.Attributes["ErrorCount"].Value = errCount.ToString();

                    String strMailMessage = "";
                    String strSMSMessage = "";
                    if (aNode.Attributes["SendMailMode"] != null)
                    {
                        switch (aNode.Attributes["SendMailMode"].Value)
                        {
                            case "None": break;
                            case "All":
                            case "ErrorOnly":
                                if (aNode.Attributes["Error"].Value != "" && Convert.ToInt16(aNode.Attributes["ErrorCount"].Value) <= ErrorCount)
                                {
                                    strMailMessage = sendErrorEmail(aNode, logMessage[1]);
                                }
                                if (aNode.Attributes["SendMailNumber"] != null && aNode.Attributes["SendMailNumber"].Value != "" && Convert.ToInt16(aNode.Attributes["ErrorCount"].Value) <= ErrorCount)
                                {
                                    //CMC
                                    strSMSMessage = SendSMS(aNode, logMessage[1]);
                                }
                                break;
                        }
                    }

                    String LogMessage = logMessage[1] + " " + strMailMessage + " " + strSMSMessage;
                    switch (aNode.Attributes["LogScheduling"].Value)
                    {
                        case ("None"):
                            break;
                        case ("All"):
                        case ("ErrorOnly"):
                            if (Convert.ToInt16(aNode.Attributes["ErrorCount"].Value) <= ErrorCount)
                                Log(aNode, "Error", LogMessage);
                            break;
                    }
                    aNode.Attributes["IsRunning"].Value = "0";
                    return;
                }

                DateTime dtStart = DateTime.Now;
                String Status = "Succeed";
                String catchMessage = "";

                String[] Methods = aNode.Attributes["ServerMethod"].Value.Split(';');
                for (int x = 0; x < Methods.Length; x++)
                {
                    String[] Method = Methods[0].Split('.');

                    object[] param = aNode.Attributes["Parameters"].Value.Split(',');
                    object[] myRet = new object[2];
                    if (aNode.Attributes["SchedulingMode"].Value == "Server Method(sync)")
                    {
                        try
                        {
                            object[] clientinfo = new object[1];
                            clientinfo[0] = CliUtils.GetBaseClientInfo();
                            (clientinfo[0] as Object[])[2] = DB;
                            (clientinfo[0] as Object[])[6] = Solution;

                            myRet = CliUtils.RemoteObject.CallMethod(clientinfo, Method[0], Method[1], param);
                        }
                        catch (Exception ex)
                        {
                            catchMessage = ex.Message;
                        }
                        finally
                        {
                            if (myRet[0] == null || myRet[0].ToString() != "0" || catchMessage != "")
                            {
                                Status = "Error";
                                int errCount = Convert.ToInt16(aNode.Attributes["ErrorCount"].Value) + 1;
                                aNode.Attributes["ErrorCount"].Value = errCount.ToString();
                            }
                            else
                            {
                                aNode.Attributes["ErrorCount"].Value = "0";
                            }

                            String strMailMessage = "";
                            //CMC
                            String strSMSMessage = "";
                            if (aNode.Attributes["SendMailMode"] != null)
                            {
                                switch (aNode.Attributes["SendMailMode"].Value)
                                {
                                    case "None": break;
                                    case "All":
                                        if ((aNode.Attributes["Error"].Value != "") && Convert.ToInt16(aNode.Attributes["ErrorCount"].Value) <= ErrorCount)
                                        {
                                            if (myRet[0].ToString() != "0")
                                            {
                                                strMailMessage = sendErrorEmail(aNode, myRet[1].ToString() + " " + catchMessage);
                                            }
                                            else
                                            {
                                                strMailMessage = sendErrorEmail(aNode, catchMessage);
                                            }
                                        }
                                        if (aNode.Attributes["SendMailNumber"] != null && aNode.Attributes["SendMailNumber"].Value != "" && Convert.ToInt16(aNode.Attributes["ErrorCount"].Value) <= ErrorCount)
                                        {
                                            //CMC
                                            if (myRet[0].ToString() != "0")
                                            {
                                                strSMSMessage = SendSMS(aNode, myRet[1].ToString() + " " + catchMessage);
                                            }
                                            else
                                            {
                                                strSMSMessage = SendSMS(aNode, catchMessage);
                                            }
                                        }

                                        break;
                                    case "ErrorOnly":
                                        if ((myRet[0].ToString() != "0" || catchMessage != "") && Convert.ToInt16(aNode.Attributes["ErrorCount"].Value) <= ErrorCount)
                                        {
                                            if (aNode.Attributes["Error"].Value != "")
                                            {
                                                strMailMessage = sendErrorEmail(aNode, myRet[1].ToString() + " " + catchMessage);
                                            }
                                            //CMC
                                            else if (aNode.Attributes["SendMailNumber"] != null && aNode.Attributes["SendMailNumber"].Value != "")
                                            {
                                                strSMSMessage = SendSMS(aNode, myRet[1].ToString() + " " + catchMessage);
                                            }
                                        }
                                        break;
                                }
                            }

                            DateTime dtEnd = DateTime.Now;
                            switch (aNode.Attributes["LogScheduling"].Value)
                            {
                                case ("None"):
                                    break;
                                case ("All"):
                                    if (myRet[0] == null && Convert.ToInt16(aNode.Attributes["ErrorCount"].Value) <= ErrorCount)
                                    {
                                        Log(aNode, "Error", strMailMessage + strSMSMessage);
                                    }
                                    else
                                    {
                                        if (myRet[0].ToString() != "0" || catchMessage != "")
                                        {
                                            if (Convert.ToInt16(aNode.Attributes["ErrorCount"].Value) <= ErrorCount)
                                            {
                                                if (myRet.Length > 1)
                                                    Log(aNode, "Error", myRet[1].ToString() + " " + strMailMessage + " " + strSMSMessage);
                                                else
                                                    Log(aNode, "Error", myRet[0].ToString() + " " + strMailMessage + " " + strSMSMessage);
                                            }
                                        }
                                        else
                                            Log(aNode, "Succeed", "");
                                    }
                                    break;
                                case ("ErrorOnly"):
                                    if ((myRet[0].ToString() != "0" || catchMessage != "") && Convert.ToInt16(aNode.Attributes["ErrorCount"].Value) <= ErrorCount)
                                        Log(aNode, "Error", myRet[1].ToString() + " " + strMailMessage + " " + strSMSMessage);
                                    break;
                            }
                            //if (isTest)
                            //{
                            //    if (catchMessage != "")
                            //        MessageBox.Show("Test is Error.The Error Message: " + catchMessage);
                            //    else if (myRet[0] == null || myRet[0].ToString() != "0")
                            //        MessageBox.Show("Test is Error.The Error Message: " + myRet[1].ToString());
                            //    else
                            //        MessageBox.Show("Test is succeed.");
                            //}
                            aNode.Attributes["LastDateTime"].Value = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff");
                            aNode.Attributes["IsRunning"].Value = "0";
                            runSave();
                            LogOut();
                        }
                    }
                    else if (aNode.Attributes["SchedulingMode"].Value == "Server FLMethod(sync)")
                    {
                        try
                        {
                            object[] clientinfo = new object[4] { null, -1, -1, string.Empty };
                            clientinfo[0] = CliUtils.GetBaseClientInfo();
                            (clientinfo[0] as Object[])[2] = DB;
                            (clientinfo[0] as Object[])[6] = Solution;
                            myRet = CliUtils.RemoteObject.CallFLMethod(clientinfo, Method[1], param);
                        }
                        catch (Exception ex)
                        {
                            catchMessage = ex.Message;
                        }
                        finally
                        {
                            if (myRet[0] == null || myRet[0].ToString() != "0" || catchMessage != "")
                            {
                                Status = "Error";
                                int errCount = Convert.ToInt16(aNode.Attributes["ErrorCount"].Value) + 1;
                                aNode.Attributes["ErrorCount"].Value = errCount.ToString();
                            }
                            else
                            {
                                aNode.Attributes["ErrorCount"].Value = "0";
                            }

                            try
                            {
                                String strMailMessage = "";
                                String strSMSMessage = "";

                                if (aNode.Attributes["SendMailMode"] != null)
                                {
                                    switch (aNode.Attributes["SendMailMode"].Value)
                                    {
                                        case "None": break;
                                        case "All":
                                            if ((aNode.Attributes["Error"].Value != "") && Convert.ToInt16(aNode.Attributes["ErrorCount"].Value) <= ErrorCount)
                                            {
                                                if (myRet[0].ToString() != "0")
                                                {
                                                    strMailMessage = sendErrorEmail(aNode, myRet[1].ToString() + " " + catchMessage);
                                                }
                                                else
                                                {
                                                    strMailMessage = sendErrorEmail(aNode, catchMessage);
                                                }
                                            }
                                            if (aNode.Attributes["SendMailNumber"] != null && aNode.Attributes["SendMailNumber"].Value != "" && Convert.ToInt16(aNode.Attributes["ErrorCount"].Value) <= ErrorCount)
                                            {
                                                //CMC
                                                if (myRet[0].ToString() != "0")
                                                {
                                                    strSMSMessage = SendSMS(aNode, myRet[1].ToString() + " " + catchMessage);
                                                }
                                                else
                                                {
                                                    strSMSMessage = SendSMS(aNode, catchMessage);
                                                }
                                            }

                                            break;
                                        case "ErrorOnly":
                                            if ((myRet[0].ToString() != "0" || catchMessage != "") && Convert.ToInt16(aNode.Attributes["ErrorCount"].Value) <= ErrorCount)
                                            {
                                                if (aNode.Attributes["Error"].Value != "")
                                                {
                                                    strMailMessage = sendErrorEmail(aNode, myRet[1].ToString() + " " + catchMessage);
                                                }
                                                //CMC
                                                else if (aNode.Attributes["SendMailNumber"] != null && aNode.Attributes["SendMailNumber"].Value != "")
                                                {
                                                    strSMSMessage = SendSMS(aNode, myRet[1].ToString() + " " + catchMessage);
                                                }
                                            }

                                            break;
                                    }
                                }

                                DateTime dtEnd = DateTime.Now;
                                switch (aNode.Attributes["LogScheduling"].Value)
                                {
                                    case ("None"):
                                        break;
                                    case ("All"):
                                        if (myRet[0] == null && Convert.ToInt16(aNode.Attributes["ErrorCount"].Value) <= ErrorCount)
                                        {
                                            Log(aNode, "Error", strMailMessage + strSMSMessage);
                                        }
                                        else
                                        {
                                            if (myRet[0].ToString() != "0" || catchMessage != "")
                                            {
                                                if (Convert.ToInt16(aNode.Attributes["ErrorCount"].Value) <= ErrorCount)
                                                {
                                                    if (myRet.Length > 1)
                                                        Log(aNode, "Error", myRet[1].ToString() + " " + strMailMessage + " " + strSMSMessage);
                                                    else
                                                        Log(aNode, "Error", myRet[0].ToString() + " " + strMailMessage + " " + strSMSMessage);
                                                }
                                            }
                                            else
                                                Log(aNode, "Succeed", "");
                                        }
                                        break;
                                    case ("ErrorOnly"):
                                        if ((myRet[0].ToString() != "0" || catchMessage != "") && Convert.ToInt16(aNode.Attributes["ErrorCount"].Value) <= ErrorCount)
                                            Log(aNode, "Error", myRet[1].ToString() + " " + strMailMessage + " " + strSMSMessage);
                                        break;
                                }
                            }
                            finally
                            {
                                aNode.Attributes["LastDateTime"].Value = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff");
                                aNode.Attributes["IsRunning"].Value = "0";
                                runSave();
                                LogOut();
                            }

                            //if (isTest)
                            //{
                            //    if (catchMessage != "")
                            //        MessageBox.Show("Test is Error.The Error Message: " + catchMessage);
                            //    else if (myRet[0] == null || myRet[0].ToString() != "0")
                            //        MessageBox.Show("Test is Error.The Error Message: " + myRet[1].ToString());
                            //    else
                            //        MessageBox.Show("Test is succeed.");
                            //}

                        }
                    }
                }
            }
            else if (aNode.Attributes["SchedulingMode"].Value == "EXE")
            {
                DateTime exeStart = DateTime.Now;
                string exeStatus = "Succeed";

                ProcessStartInfo ps = new ProcessStartInfo(aNode.Attributes["ServerMethod"].Value, aNode.Attributes["Parameters"].Value);
                Process.Start(ps);

                DateTime exeEnd = DateTime.Now;
                String strMailMessage = "";
                //CMC
                String strSMSMessage = "";

                if (aNode.Attributes["SendMailMode"] != null)
                {
                    switch (aNode.Attributes["SendMailMode"].Value)
                    {
                        case "None": break;
                        case "All":
                            if (aNode.Attributes["Error"].Value != "")
                            {
                                strMailMessage = sendErrorEmail(aNode, exeStatus);
                            }
                            if (aNode.Attributes["SendMailNumber"] != null && aNode.Attributes["SendMailNumber"].Value != "")
                            {
                                //CMC
                                strSMSMessage = SendSMS(aNode, exeStatus);
                            }
                            break;
                        case "ErrorOnly":
                            break;
                    }
                }

                switch (aNode.Attributes["LogScheduling"].Value)
                {
                    case ("None"):
                        break;
                    case ("All"):
                        Log(aNode, exeStatus, "");
                        break;
                    case ("ErrorOnly"):
                        break;
                }

                //if (isTest)
                //    MessageBox.Show("Test is succeed.");
                aNode.Attributes["LastDateTime"].Value = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff");
                aNode.Attributes["IsRunning"].Value = "0";
                runSave();
            }
        }

        private void runSave()
        {
            lock (bDBXML)
            {
                String sPath = Application.StartupPath + "\\Scheduling.xml";
                bFileStream.Close();
                bDBXML.Save(sPath);
                //bDBXML = null;

                //bDBXML = new XmlDocument();
                //bFileStream = new FileStream(sPath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                //bDBXML.Load(bFileStream);
            }
        }

        private void LogOut()
        {
            try
            {
                CliUtils.CallMethod("GLModule", "LogOut", new object[] { (object)(CliUtils.fLoginUser) });
            }
            catch { }
        }

        private void Log(XmlNode aNode, string str, string Message)
        {
            String s = Application.StartupPath + "\\SchedulingLogs\\" + DateTime.Now.Year + DateTime.Now.Month.ToString("00") + DateTime.Now.Day.ToString("00") + "\\" + aNode.Attributes["SchedulName"].Value + ".txt";

            if (!Directory.Exists(Application.StartupPath + "\\SchedulingLogs\\" + DateTime.Now.Year + DateTime.Now.Month.ToString("00") + DateTime.Now.Day.ToString("00")))
            {
                Directory.CreateDirectory(Application.StartupPath + "\\SchedulingLogs\\" + DateTime.Now.Year + DateTime.Now.Month.ToString("00") + DateTime.Now.Day.ToString("00"));
            }

            ArrayList alLog = new ArrayList();
            using (FileStream file = File.Open(s, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                StreamReader sreader = new StreamReader(file);
                DateTime dtLast = DateTime.MinValue;
                if (aNode.Attributes["LastDateTime"].Value != null && aNode.Attributes["LastDateTime"].Value != String.Empty)
                    dtLast = Convert.ToDateTime(aNode.Attributes["LastDateTime"].Value);
                DateTime dtNow = DateTime.Now;

                alLog.Add(sreader.ReadToEnd());
                sreader.Close();
                alLog.Add("SchedulName: " + aNode.Attributes["SchedulName"].Value + " Status: " + str + " Message:" + Message);
                alLog.Add("LastDateTime: " + aNode.Attributes["LastDateTime"].Value + " ExecuteTime: " + DateTime.Now);
                alLog.Add("CycleUnit:" + aNode.Attributes["CycleUnit"].Value);
                alLog.Add("Cycle:" + aNode.Attributes["Cycle"].Value);
                alLog.Add("CycleHour:" + aNode.Attributes["CycleHour"].Value);
                alLog.Add("When:" + aNode.Attributes["When"].Value);
                alLog.Add("Error:" + aNode.Attributes["Error"].Value);
                alLog.Add("StartDateTime:" + aNode.Attributes["StartDateTime"].Value);
                if (dtLast != DateTime.MinValue)
                    alLog.Add("TimeSpan:" + ((TimeSpan)(dtNow - dtLast)).TotalDays + " days");
                alLog.Add("");
                file.Close();
            }
            using (FileStream file = File.Open(s, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                StreamWriter swriter = new StreamWriter(file);
                for (int i = 0; i < alLog.Count; i++)
                    swriter.WriteLine(alLog[i]);
                swriter.Close();
                file.Close();
            }
        }

        private String sendErrorEmail(XmlNode aNode, string errMessage)
        {
            String[] option = getOption();
            //String Email = option[3] == "" ? "reikami@gmail.cn" : option[3];
            //String EmailPwd = Email == "reikami@gmail.cn" ? "56734452" : option[4];
            //String Host = Email == "reikami@gmail.cn" ? "gmail.cn" : option[5];
            //String Port = Email == "reikami@gmail.cn" ? "" : option[10];
            String Email = option[3];
            String EmailPwd = option[4];
            String Host = option[5];
            String Port = option[10];
            String SSL = option[13];
            String message = "";
            try
            {
                //Builed The MSG
                MailMessage msg = new MailMessage();
                msg.To.Add(aNode.Attributes["Error"].Value);
                msg.From = new MailAddress(Email, "Infolight", System.Text.Encoding.UTF8);
                msg.Subject = "SchedulName: " + aNode.Attributes["SchedulName"].Value;
                msg.SubjectEncoding = System.Text.Encoding.UTF8;
                msg.Body = (aNode.Attributes["Subject"] != null ? aNode.Attributes["Subject"].Value : "") + " SchedulName: " + aNode.Attributes["SchedulName"].Value + ". Messages :" + errMessage + ". LastDateTime: " + aNode.Attributes["LastDateTime"].Value + " ExecuteTime: " + DateTime.Now;
                msg.BodyEncoding = System.Text.Encoding.UTF8;
                msg.IsBodyHtml = false;
                msg.Priority = MailPriority.High;

                //Add the Creddentials
                SmtpClient client = new SmtpClient();
                client.Credentials = new System.Net.NetworkCredential(Email, EmailPwd);
                client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                client.EnableSsl = SSL.ToLower() == "true" ? true : false;

                if (Host != "")
                    client.Host = Host;
                if (Port != "")
                    client.Port = Convert.ToInt16(Port);
                client.SendCompleted += new SendCompletedEventHandler(client_SendCompleted);
                object userState = msg;
                if (client != null)
                {
                    try
                    {
                        //you can also call client.Send(msg)
                        //client.SendAsync(msg, userState);
                        client.Send(msg);
                    }
                    catch (Exception ex)
                    {
                        message = ex.Message;
                    }
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            return message;
        }

        //CMC
        private string SendSMS(XmlNode aNode, string errMessage)
        {
            String[] option = getOption();
            String sms = option[14];

            string message = string.Empty; ;
            string toSMS = aNode.Attributes["SendMailNumber"].Value;
            if (string.IsNullOrEmpty(sms))
            {
                message = "SMS is empty";
                return message;
            }
            if (string.IsNullOrEmpty(toSMS))
            {
                message = "ErrorSendNumber is empty";
                return message;
            }
            string body = "";
            if (aNode.Attributes["Subject"] != null && aNode.Attributes["Subject"].Value != "")
                body = aNode.Attributes["Subject"].Value + " Messages :" + errMessage;
            else
                body = "SchedulName: " + aNode.Attributes["SchedulName"].Value + ". Messages :" + errMessage;// +". LastDateTime: " + aNode.Attributes["LastDateTime"].Value + " ExecuteTime: " + DateTime.Now;
            System.Text.Encoding en = System.Text.Encoding.GetEncoding("big5");
            string realbody = HttpUtility.UrlEncode(body, en);

            if (realbody.Length > 140)
                realbody = realbody.Substring(0, 140);
            string username = "eservice";
            string password = "A874560307#";
            string CharacterSetID = "17";
            System.Net.WebClient WebClientObj = new System.Net.WebClient();
            System.Collections.Specialized.NameValueCollection PostVars = new System.Collections.Specialized.NameValueCollection();

            //添加值域
            PostVars.Add("username", username);
            PostVars.Add("password", password);
            PostVars.Add("CharacterSetID", CharacterSetID);
            PostVars.Add("smslist", toSMS);
            //PostVars.Add("referurl", referurl);
            PostVars.Add("content", realbody);
            try
            {
                byte[] byRemoteInfo = WebClientObj.UploadValues(sms, "POST", PostVars);
                string sRemoteInfo = System.Text.Encoding.GetEncoding("utf-8").GetString(byRemoteInfo);
                message = sRemoteInfo;
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return message;
        }

        private void sendEmail(string Message, bool isConnect)
        {
            String[] option = getOption();
            String Email = option[3];
            String EmailPwd = option[4];
            String Host = option[5];
            String Port = option[10];
            String SSL = option[13];

            String SendToMail = "";
            String Subject = "";
            if (option[11] != null)
            {
                SendToMail = option[11];
                if (isConnect)
                {
                    Subject = "Can't connect Server";
                }
                else
                {
                    Subject = "Connect Server succeed again.";
                }
                //Builed The MSG
                MailMessage msg = new MailMessage();
                msg.To.Add(SendToMail);
                msg.From = new MailAddress(Email, "Infolight", System.Text.Encoding.UTF8);
                msg.Subject = Subject;
                msg.SubjectEncoding = System.Text.Encoding.UTF8;
                msg.Body = "Scheduling: " + Message + "\n" + DateTime.Now;
                msg.BodyEncoding = System.Text.Encoding.UTF8;
                msg.IsBodyHtml = false;
                msg.Priority = MailPriority.High;

                //Add the Creddentials
                SmtpClient client = new SmtpClient();
                client.Credentials = new System.Net.NetworkCredential(Email, EmailPwd);
                client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                client.EnableSsl = SSL.ToLower() == "true" ? true : false;

                if (Host != "")
                    client.Host = Host;
                if (Port != "")
                    client.Port = Convert.ToInt16(Port);
                client.SendCompleted += new SendCompletedEventHandler(client_SendCompleted);
                object userState = msg;
                if (client != null)
                {
                    try
                    {
                        //you can also call client.Send(msg)
                        //client.SendAsync(msg, userState);
                        client.Send(msg);
                    }
                    catch
                    {

                    }
                }
            }
        }

        void client_SendCompleted(object sender, AsyncCompletedEventArgs e)
        {
            MailMessage mail = (MailMessage)e.UserState;
            string subject = mail.Subject;

            if (e.Cancelled)
            {
                string cancelled = string.Format("[{0}] Send canceled.", subject);
                MessageBox.Show(cancelled);
            }
            if (e.Error != null)
            {
                string error = String.Format("[{0}] {1}", subject, e.Error.ToString());
                MessageBox.Show(error);
            }
            else
            {
                MessageBox.Show("Message sent.");
            }
        }

        private void TSMainOptions_Click(object sender, EventArgs e)
        {
            frmOptions fo = new frmOptions();
            fo.ShowDialog();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            setScheduling();
            closeAll();
        }

        private void Save()
        {
            Mode = "Save";
            string s = Application.StartupPath + "\\Scheduling.xml";
            XmlDocument DBXML = new XmlDocument();
            FileStream aFileStream;
            String SchedulName = this.tbName.Text;
            String Description = this.tbDescriotion.Text;
            String Active = "0";
            if (this.cbActive.Checked == true)
                Active = "1";
            String SchedulingMode = this.cbSchedulingMode.Text;
            String ServerMethod = this.tbServerMethod.Text;
            String Parameters = this.tbParameters.Text;
            String DataBase = this.cbDataBase.Text;
            String Solution = this.cbSolution.SelectedValue == null ? this.cbSolution.Text : this.cbSolution.SelectedValue.ToString();
            String CycleUnit = this.cbCycleUnit.Text;
            String Cycle = this.tbCycle.Text;
            String CycleHour = this.tbCycleHour.Text;
            String When = this.cbWhen.Text;
            String Error = this.tbErr.Text;
            String LogScheduling = this.cbLog.Text;
            String LastDateTime = this.tbLastDateTime.Text;
            String SendMailMode = this.cbSendMailMode.Text;
            //CMC
            String SendMailNumber = this.tbErrN.Text;
            String Subject = this.tbUserMsg.Text;

            if (!File.Exists(s))
            {
                try
                {
                    if (bDBXML == null)
                    {
                        aFileStream = new FileStream(s, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                    }
                    else
                    {
                        aFileStream = bFileStream;
                    }

                    try
                    {
                        XmlTextWriter w = new XmlTextWriter(aFileStream, new System.Text.ASCIIEncoding());
                        w.Formatting = Formatting.Indented;
                        w.WriteStartElement("InfolightScheduling");
                        w.WriteEndElement();
                        w.Close();
                    }
                    finally
                    {
                        aFileStream.Close();
                    }
                }
                catch (Exception er) { string str = er.Message; }
            }

            try
            {
                if (bDBXML == null)
                {
                    aFileStream = new FileStream(s, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                }
                else
                {
                    aFileStream = bFileStream;
                }

                try
                {
                    //改成编辑时自动暂停，每次都存取本地文件
                    //if (this.Text.Contains("(Active)"))
                    //    DBXML = bDBXML;
                    //else
                    DBXML.Load(aFileStream);
                    XmlNode aNode = null;

                    for (int j = DBXML.DocumentElement.ChildNodes.Count - 1; j >= 0; j--)
                        if (string.Compare(DBXML.DocumentElement.ChildNodes[j].Attributes["SchedulName"].Value.Trim(), SchedulName, true) == 0)//IgnoreCase
                        {
                            aNode = DBXML.DocumentElement.ChildNodes[j];
                            break;
                        }

                    if (aNode == null)
                    {
                        XmlElement elem = DBXML.CreateElement("String");
                        XmlAttribute attr = DBXML.CreateAttribute("SchedulName");
                        attr.Value = SchedulName;
                        elem.Attributes.Append(attr);

                        attr = DBXML.CreateAttribute("Description");
                        attr.Value = Description;
                        elem.Attributes.Append(attr);

                        attr = DBXML.CreateAttribute("Active");
                        attr.Value = Active;
                        elem.Attributes.Append(attr);

                        attr = DBXML.CreateAttribute("SchedulingMode");
                        attr.Value = SchedulingMode;
                        elem.Attributes.Append(attr);

                        attr = DBXML.CreateAttribute("ServerMethod");
                        attr.Value = ServerMethod;
                        elem.Attributes.Append(attr);

                        attr = DBXML.CreateAttribute("Parameters");
                        attr.Value = Parameters;
                        elem.Attributes.Append(attr);

                        attr = DBXML.CreateAttribute("Database");
                        attr.Value = DataBase;
                        elem.Attributes.Append(attr);

                        attr = DBXML.CreateAttribute("Solution");
                        attr.Value = Solution;
                        elem.Attributes.Append(attr);

                        attr = DBXML.CreateAttribute("CycleUnit");
                        attr.Value = CycleUnit;
                        elem.Attributes.Append(attr);

                        attr = DBXML.CreateAttribute("Cycle");
                        attr.Value = Cycle;
                        elem.Attributes.Append(attr);

                        attr = DBXML.CreateAttribute("CycleHour");
                        attr.Value = CycleHour;
                        elem.Attributes.Append(attr);

                        attr = DBXML.CreateAttribute("When");
                        attr.Value = When;
                        elem.Attributes.Append(attr);

                        attr = DBXML.CreateAttribute("Error");
                        attr.Value = Error;
                        elem.Attributes.Append(attr);

                        attr = DBXML.CreateAttribute("LogScheduling");
                        attr.Value = LogScheduling;
                        elem.Attributes.Append(attr);

                        attr = DBXML.CreateAttribute("LastDateTime");
                        attr.Value = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff");
                        elem.Attributes.Append(attr);

                        attr = DBXML.CreateAttribute("SendMailMode");
                        attr.Value = SendMailMode;
                        elem.Attributes.Append(attr);

                        attr = DBXML.CreateAttribute("IsRunning");
                        attr.Value = "0";
                        elem.Attributes.Append(attr);

                        attr = DBXML.CreateAttribute("StartDateTime");
                        attr.Value = "";
                        elem.Attributes.Append(attr);

                        attr = DBXML.CreateAttribute("ErrorCount");
                        attr.Value = "0";
                        elem.Attributes.Append(attr);

                        //CMC
                        attr = DBXML.CreateAttribute("SendMailNumber");
                        attr.Value = SendMailNumber;
                        elem.Attributes.Append(attr);
                        attr = DBXML.CreateAttribute("Subject");
                        attr.Value = Subject;
                        elem.Attributes.Append(attr);

                        DBXML.DocumentElement.AppendChild(elem);
                    }
                    else
                    {
                        aNode.Attributes["SchedulName"].InnerText = SchedulName;
                        aNode.Attributes["Description"].InnerText = Description;
                        aNode.Attributes["Active"].InnerText = Active;
                        aNode.Attributes["SchedulingMode"].InnerText = SchedulingMode;
                        aNode.Attributes["ServerMethod"].InnerText = ServerMethod;
                        aNode.Attributes["Parameters"].InnerText = Parameters;
                        aNode.Attributes["Database"].InnerText = DataBase;
                        aNode.Attributes["Solution"].InnerText = Solution;
                        aNode.Attributes["CycleUnit"].InnerText = CycleUnit;
                        aNode.Attributes["Cycle"].InnerText = Cycle;
                        aNode.Attributes["CycleHour"].InnerText = CycleHour;
                        aNode.Attributes["When"].InnerText = When;
                        aNode.Attributes["Error"].InnerText = Error;
                        aNode.Attributes["LogScheduling"].InnerText = LogScheduling;
                        aNode.Attributes["LastDateTime"].InnerText = LastDateTime;
                        if (aNode.Attributes["SendMailMode"] == null)
                        {
                            XmlAttribute attr = DBXML.CreateAttribute("SendMailMode");
                            attr.Value = SendMailMode;
                            aNode.Attributes.Append(attr);
                        }
                        else
                            aNode.Attributes["SendMailMode"].InnerText = SendMailMode;
                        if (aNode.Attributes["IsRunning"] == null)
                        {
                            XmlAttribute attr = DBXML.CreateAttribute("IsRunning");
                            attr.Value = "0";
                            aNode.Attributes.Append(attr);
                        }
                        else
                            aNode.Attributes["IsRunning"].InnerText = "0";
                        if (aNode.Attributes["StartDateTime"] == null)
                        {
                            XmlAttribute attr = DBXML.CreateAttribute("StartDateTime");
                            attr.Value = "";
                            aNode.Attributes.Append(attr);
                        }
                        else
                            aNode.Attributes["StartDateTime"].InnerText = "";
                        if (aNode.Attributes["ErrorCount"] == null)
                        {
                            XmlAttribute attr = DBXML.CreateAttribute("ErrorCount");
                            attr.Value = "0";
                            aNode.Attributes.Append(attr);
                        }
                        else
                            aNode.Attributes["ErrorCount"].InnerText = "0";
                        //CMC
                        if (aNode.Attributes["SendMailNumber"] == null)
                        {
                            XmlAttribute attr = DBXML.CreateAttribute("SendMailNumber");
                            attr.Value = SendMailNumber;
                            aNode.Attributes.Append(attr);
                        }
                        else
                            aNode.Attributes["SendMailNumber"].InnerText = SendMailNumber;
                        if (aNode.Attributes["Subject"] == null)
                        {
                            XmlAttribute attr = DBXML.CreateAttribute("Subject");
                            attr.Value = Subject;
                            aNode.Attributes.Append(attr);
                        }
                        else
                            aNode.Attributes["Subject"].InnerText = Subject;
                    }
                }
                finally
                {
                    aFileStream.Close();
                }
                DBXML.Save(s);
            }
            catch (Exception er) { string str = er.Message; }

            //MessageBox.Show("Save success!");
            findScheduling();
            closeAll();

            if (state == "Active")
            {
                Start();
                state = "";
            }
        }

        private void Delete()
        {
            DialogResult dr = MessageBox.Show("Do you want to DELETE it?", "warning", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                string SchedulName = "";
                if (lbScheduling.SelectedItems.Count == 0)
                    return;
                else
                    SchedulName = lbScheduling.SelectedItems[0].ToString();

                if (this.Text.Contains("Active"))
                {
                    XmlNode bNode = bDBXML.DocumentElement.FirstChild;
                    while (bNode != null)
                    {
                        if (bNode.Attributes["SchedulName"].InnerText == SchedulName)
                        {
                            bDBXML.DocumentElement.RemoveChild(bNode);
                            break;
                        }
                        bNode = bNode.NextSibling;
                    }
                }
                else
                {
                    XmlDocument DBXML = new XmlDocument();
                    String s = Application.StartupPath + "\\Scheduling.xml";

                    if (File.Exists(s))
                    {
                        DBXML.Load(s);
                        XmlNode bNode = DBXML.DocumentElement.FirstChild;
                        while (bNode != null)
                        {
                            if (bNode.Attributes["SchedulName"].InnerText == SchedulName)
                                DBXML.DocumentElement.RemoveChild(bNode);
                            bNode = bNode.NextSibling;
                        }
                        DBXML.Save(s);
                    }
                }
                findScheduling();
            }
        }

        private void Modify()
        {
            if (this.Text.Contains("(Active)")) state = "Active";
            Stop();
            Mode = "Changing";
            Name = this.tbName.Text;
            closeAll();
            openAll();
            checkSchedulingMode();
            checkCycleUnit();
        }

        private void TSMainSave_Click(object sender, EventArgs e)
        {
            if (this.Text.Contains("Active"))
            {
                String sPath = Application.StartupPath + "\\Scheduling.xml";

                try
                {
                    bFileStream.Close();
                    bDBXML.Save(sPath);
                }
                catch
                {

                }

                if (bDBXML == null)
                {
                    bDBXML = new XmlDocument();
                    bFileStream = new FileStream(sPath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                    bDBXML.Load(bFileStream);
                }
            }

            if (Mode == "Changing")
                Save();
        }

        private void TSMainClose_Click(object sender, EventArgs e)
        {
            //DialogResult dr = MessageBox.Show("Scheduling is still Active.Do you want to DELETE it?", "warning", MessageBoxButtons.YesNo);
            //if (dr == DialogResult.Yes)
            //{
            CloseOut();

            components.Dispose();
            base.Dispose(true);
            //}
        }

        private void TSMainModify_Click(object sender, EventArgs e)
        {
            Modify();
        }

        private void TSMainDelete_Click(object sender, EventArgs e)
        {
            Delete();
        }

        private String[] option = null;
        private DateTime optionDT = new DateTime();
        private string[] getOption()
        {
            String s = Application.StartupPath + "\\SchedulingOptions.xml";
            if (File.Exists(s))
            {
                FileInfo fi = new FileInfo(s);
                if (option == null || optionDT != fi.LastWriteTime)
                {
                    option = new String[15];
                    optionDT = fi.LastWriteTime;
                    XmlDocument DBXML = new XmlDocument();
                    using (FileStream aFileStream = new FileStream(s, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        try
                        {
                            DBXML.Load(aFileStream);
                            XmlNode aNode = DBXML.DocumentElement.FirstChild;
                            option[0] = aNode.Attributes["UserID"].Value;
                            option[1] = aNode.Attributes["Password"].Value;
                            if (aNode.Attributes["ServerIP"] == null || aNode.Attributes["ServerIP"].Value == "")
                            {
                                String cpath;
                                cpath = Application.StartupPath + "\\";

                                XmlDocument xmlDocC = new XmlDocument();
                                xmlDocC.Load(cpath + "Scheduling.exe.config");

                                if (xmlDocC != null && xmlDocC.FirstChild != null)
                                {
                                    XmlNode n = xmlDocC.FirstChild.FirstChild.FirstChild.FirstChild.
                                        SelectSingleNode("wellknown[@type='Srvtools.LoginService, Srvtools']");
                                    if (n != null)
                                    {
                                        String url = n.Attributes["url"].InnerText;

                                        MatchCollection mc = Regex.Matches(url, @"(\S+):");
                                        String[] ip = mc[0].ToString().Split("/,:".ToCharArray());
                                        option[2] = ip[3];
                                    }
                                }

                            }
                            else
                                option[2] = aNode.Attributes["ServerIP"].Value;
                            option[3] = aNode.Attributes["Email"].Value;
                            option[4] = aNode.Attributes["EmailPwd"].Value;
                            option[5] = aNode.Attributes["Host"].Value;
                            option[6] = aNode.Attributes["Interval"].Value;
                            option[7] = aNode.Attributes["DayTime"].Value;
                            option[8] = aNode.Attributes["NightTime"].Value;
                            option[9] = aNode.Attributes["Holiday"].Value;
                            option[10] = aNode.Attributes["Port"].Value;
                            if (aNode.Attributes["SystemEmail"] != null)
                                option[11] = aNode.Attributes["SystemEmail"].Value;
                            if (aNode.Attributes["ServerPort"] != null)
                                option[12] = aNode.Attributes["ServerPort"].Value;
                            if (aNode.Attributes["SSL"] != null)
                                option[13] = aNode.Attributes["SSL"].Value;
                            if (aNode.Attributes["SMS"] != null)
                                option[14] = aNode.Attributes["SMS"].Value;

                            aFileStream.Close();
                        }
                        finally
                        {
                            aFileStream.Close();
                        }
                    }
                }
            }
            return option;
        }

        private string[] Login(string DB, string sn)
        {
            string[] OptionMessage = getOption();

            if (!isRegister)
            {
                isRegister = Register(false, OptionMessage[2], OptionMessage[12]);
                if (!isRegister)
                    return new string[] { "false", "Register is field." };
            }

            //CliUtils.fLoginUser = OptionMessage[0];
            //CliUtils.fLoginPassword = OptionMessage[1];
            //CliUtils.fLoginDB = DB;
            //CliUtils.fCurrentProject = sn;
            LoginResult result = LoginResult.Success;
            if (CliUtils.fLoginUser.Contains("'"))
            {
                result = LoginResult.UserNotFound;
            }
            else
            {
                object[] clientinfo = new object[1];
                clientinfo[0] = CliUtils.GetBaseClientInfo();
                (clientinfo[0] as Object[])[1] = OptionMessage[0];
                (clientinfo[0] as Object[])[2] = DB;
                (clientinfo[0] as Object[])[6] = sn;

                string sParam = (clientinfo[0] as Object[])[1].ToString() + ':' + OptionMessage[1] + ':' + (clientinfo[0] as Object[])[2] + ':' + "0";
                object[] myRet = CliUtils.RemoteObject.CallMethod(clientinfo, "GLModule", "CheckUser", new object[] { sParam });

                //object[] myRet = CliUtils.CallMethod("GLModule", "CheckUser", new object[] { (object)sParam });
                result = (LoginResult)myRet[1];
                switch (result)
                {
                    case LoginResult.UserNotFound:
                        {
                            string message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "EEPWebNetClient", "WinSysMsg", "msg_UserNotFound");
                            return new string[] { "false", message };
                        }
                    case LoginResult.PasswordError:
                        {
                            string message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "EEPWebNetClient", "WinSysMsg", "msg_UserOrPasswordError");
                            return new string[] { "false", message };
                        }
                    case LoginResult.Disabled:
                        {
                            string message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "EEPWebNetClient", "WinSysMsg", "msg_UserDisabled");
                            MessageBox.Show(this, message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                        }
                    case LoginResult.UserLogined:
                        {
                            String message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "EEPWebNetClient", "WinSysMsg", "msg_UserIsLogined");
                            return new string[] { "false", message };
                        }
                    default:
                        {
                            CliUtils.fUserName = myRet[2].ToString();
                            CliUtils.fLoginUser = myRet[3].ToString();
                            myRet = CliUtils.RemoteObject.CallMethod(clientinfo, "GLModule", "GetUserGroup", new object[] { CliUtils.fLoginUser });
                            //myRet = CliUtils.CallMethod("GLModule", "GetUserGroup", new object[] { CliUtils.fLoginUser });
                            if (myRet != null && (int)myRet[0] == 0)
                                CliUtils.fGroupID = myRet[1].ToString();
                            else
                                LogOut();
                            DataSet dsSolution = new DataSet();
                            if (CliUtils.fSolutionSecurity)
                            {
                                object[] myRet1 = CliUtils.RemoteObject.CallMethod(clientinfo, "GLModule", "GetSolutionSecurity", new object[] { CliUtils.fLoginUser, CliUtils.fGroupID });
                                //object[] myRet1 = CliUtils.CallMethod("GLModule", "GetSolutionSecurity", new object[] { CliUtils.fLoginUser, CliUtils.fGroupID });
                                if ((null != myRet1) && (0 == (int)myRet1[0]))
                                    dsSolution = ((DataSet)myRet1[1]);
                                bool flag = false;
                                for (int i = 0; i < dsSolution.Tables[0].Rows.Count; i++)
                                {
                                    if (dsSolution.Tables[0].Rows[i]["ITEMTYPE"].ToString() == CliUtils.fCurrentProject)
                                    {
                                        flag = true;
                                        break;
                                    }
                                }
                                if (!flag)
                                {
                                    SYS_LANGUAGE language = CliUtils.fClientLang;
                                    string message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "EEPWebNetClient", "WinSysMsg", "msg_SolutionSecurity");
                                    LogOut();
                                    return new string[] { "false", message };
                                }
                            }
                            else
                            {
                                object[] myRet1 = CliUtils.RemoteObject.CallMethod(clientinfo, "GLModule", "GetSolution", null);
                                //object[] myRet1 = CliUtils.CallMethod("GLModule", "GetSolution", null);
                                if ((null != myRet1) && (0 == (int)myRet1[0]))
                                    dsSolution = ((DataSet)myRet1[1]);
                            }
                            break;
                        }
                }
            }
            return new string[] { "true", "" };
        }

        private bool isRegister = false;
        private bool Register(bool showmessage, String IP, String Port)
        {
            if (IP != "")
            {
                CliUtils.fRemoteIP = IP;
                if (Port == null || Port == "")
                    CliUtils.fRemotePort = 8989;
                else
                    CliUtils.fRemotePort = Convert.ToInt16(Port);
                LoginService loginService = new LoginService(); // Remoting object
            GetLoginService:
                // Try to connect to server, reobtain service from the master server if failed
                try
                {
                    EEPRemoteModule module = Activator.GetObject(typeof(EEPRemoteModule),
                        string.Format("http://{0}:" + CliUtils.fRemotePort + "/InfoRemoteModule.rem", CliUtils.fRemoteIP)) as EEPRemoteModule;
                    // module.ToString(); andy kao modified 
                }
                catch
                {
                    loginService.DeRegisterRemoteServer(CliUtils.fRemoteIP, CliUtils.fRemotePort);
                    goto GetLoginService;
                }

                // Register EEPRemoteModule on the server
                WellKnownClientTypeEntry clientEntry = new WellKnownClientTypeEntry(typeof(EEPRemoteModule),
                    string.Format("http://{0}:" + CliUtils.fRemotePort + "/InfoRemoteModule.rem", CliUtils.fRemoteIP));
                RemotingConfiguration.RegisterWellKnownClientType(clientEntry);

                // End Add
                isRegister = true;
            }
            return isRegister;
        }

        private void SaveToClientXML(string sLoginUser, string sLoginDB, string sCurrentProject)
        {
            String sfile = Application.StartupPath + "\\EEPNetClient.xml";
            string sUser = sLoginUser;
            string sDB = sLoginDB;
            string sSol = sCurrentProject;
            string stemp = "";
            XmlDocument xml = new XmlDocument();
            if (File.Exists(sfile))
            {
                xml.Load(sfile);
                XmlNode el = xml.DocumentElement;
                foreach (XmlNode xNode in el.ChildNodes)
                {

                    if (string.Compare(xNode.Name, "USER", true) == 0)//IgnoreCase
                    {
                        stemp = xNode.InnerText.Trim();
                        string[] ss = stemp.Split(new char[] { ',' });
                        foreach (string s in ss)
                        {
                            if (!s.Equals(sLoginUser))
                                sUser = sUser + "," + s;
                        }
                    }
                    else if (string.Compare(xNode.Name, "DATABASE", true) == 0)//IgnoreCase
                    {
                        stemp = xNode.InnerText.Trim();
                        string[] ss = stemp.Split(new char[] { ',' });
                        foreach (string s in ss)
                        {
                            if (!s.Equals(sLoginDB))
                                sDB = sDB + "," + s;
                        }
                    }
                    else if (string.Compare(xNode.Name, "SOLUTION", true) == 0)//IgnoreCase
                    {
                        stemp = xNode.InnerText.Trim();
                        string[] ss = stemp.Split(new char[] { ',' });
                        foreach (string s in ss)
                        {
                            if (!s.Equals(sCurrentProject))
                                sSol = sSol + "," + s;
                        }
                    }
                }

                File.Delete(sfile);
            }
            else
            {
                sUser = sLoginUser; sDB = sLoginDB; sSol = sCurrentProject;
            }

            using (FileStream aFileStream = new FileStream(sfile, FileMode.Create))
            {
                try
                {
                    XmlTextWriter w = new XmlTextWriter(aFileStream, new System.Text.ASCIIEncoding());
                    w.Formatting = Formatting.Indented;
                    w.WriteStartElement("LoginInfo");

                    w.WriteStartElement("User");
                    w.WriteValue(sUser);
                    w.WriteEndElement();

                    w.WriteStartElement("DataBase");
                    w.WriteValue(sDB);
                    w.WriteEndElement();

                    w.WriteStartElement("Solution");
                    w.WriteValue(sSol);
                    w.WriteEndElement();

                    w.WriteEndElement();
                    w.Close();
                }
                finally
                {
                    aFileStream.Close();
                }
            }
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            if (this.tbName.Text != "")
            {
                String sPath = Application.StartupPath + "\\Scheduling.xml";
                if (File.Exists(sPath))
                {
                    if (bDBXML == null)
                    {
                        bDBXML = new XmlDocument();
                        bFileStream = new FileStream(sPath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                        bDBXML.Load(bFileStream);
                    }
                }
                else
                {
                    MessageBox.Show("Schedulings are not exists, please set it first.");
                    return;
                }

                checkScheduling(this.tbName.Text, true);
            }
        }

        private void TSMainMonitor_Click(object sender, EventArgs e)
        {
            Monitor m = new Monitor(bDBXML);
            m.ShowDialog();
        }

        private void TSMainWatchLog_Click(object sender, EventArgs e)
        {
            if (lbScheduling.SelectedItem != null)
            {
                String path = Application.StartupPath + "\\SchedulingLogs\\" + DateTime.Now.Year + DateTime.Now.Month.ToString("00") + DateTime.Now.Day.ToString("00") + "\\" + lbScheduling.SelectedItem.ToString() + ".txt";
                if (!File.Exists(path))
                    MessageBox.Show("Your log file isn't exists!");
                else
                    Process.Start("notepad.exe", path);
            }
            else
            {
                MessageBox.Show("Please select a Scheduling first.");
            }
        }

        private void btnServerMethod_Click(object sender, EventArgs e)
        {
            if (this.cbSchedulingMode.Text == "Server Method(sync)" || this.cbSchedulingMode.Text == "Server Method(async)" || this.cbSchedulingMode.Text == "Server FLMethod(sync)")
            {
                String sName = this.cbSolution.SelectedValue == null ? "" : this.cbSolution.SelectedValue.ToString();
                ServerMethod frmsm = new ServerMethod(sName, this.cbSchedulingMode.Text);
                frmsm.ShowDialog();
                if (frmsm.packageName != "" && frmsm.methodName != "")
                    this.tbServerMethod.Text = frmsm.packageName + "." + frmsm.methodName;
            }
            else if (this.cbSchedulingMode.Text == "EXE")
            {
                openFileDialog1.InitialDirectory = Application.StartupPath;
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    this.tbServerMethod.Text = openFileDialog1.FileNames[0];
                }
            }
        }

        private void timerConnect_Tick(object sender, EventArgs e)
        {
            ThreadStart ts = new ThreadStart(ConnectServer);
            Thread t = new Thread(ts);
            t.Start();
        }

        private int isConnect = -1;
        private void ConnectServer()
        {
            try
            {
                EEPRemoteModule module = Activator.GetObject(typeof(EEPRemoteModule),
                    string.Format("http://{0}:{1}/InfoRemoteModule.rem", getOption()[2], getOption()[12])) as EEPRemoteModule;
                module.ToString();
                if (isConnect == 1)
                {
                    sendEmail("Connect server succeed again.", false);
                }
                isConnect = 0;

                //Assembly a = this.GetType().Assembly;
                //Stream st = a.GetManifestResourceStream("Scheduling.Resources.Scheduling16.ico");
                //Icon ic1 = new Icon(st);
                //notifyIcon1.Icon = ic1;
                //Image image = this.imageList1.Images["Scheduling16.ico"];
                //MemoryStream mstream = new MemoryStream();
                //image.Save(mstream, ImageFormat.Png);
                //Icon icon = Icon.FromHandle(new Bitmap(mstream).GetHicon());
                //mstream.Close();
                //notifyIcon1.Icon = icon;
                ChangeIcon("Scheduling16.ico");
            }
            catch (Exception ex)
            {
                if (isConnect == 0 || isConnect == -1)
                {
                    sendEmail(ex.Message, true);
                }
                //Assembly a = this.GetType().Assembly;
                //Stream st = a.GetManifestResourceStream("Scheduling.Resources.SchedulingCanConnect.ico");
                //Icon ic1 = new Icon(st);
                //notifyIcon1.Icon = ic1;
                //isConnect = 1;

                //Image image = this.imageList1.Images["SchedulingCanConnect.ico"];
                //MemoryStream mstream = new MemoryStream();
                //image.Save(mstream, ImageFormat.Png);
                //Icon icon = Icon.FromHandle(new Bitmap(mstream).GetHicon());
                //mstream.Close();
                //notifyIcon1.Icon = icon;
                ChangeIcon("SchedulingCanConnect.ico");

                if (this.Text.Contains("(Active)"))
                {
                    XmlNode bNode = bDBXML.DocumentElement.FirstChild;
                    while (bNode != null)
                    {
                        //if (bNode.Attributes["IsRunning"].Value == "1")
                        //    LogOut();
                        bNode.Attributes["IsRunning"].Value = "0";
                        bNode = bNode.NextSibling;
                    }
                }
            }
        }


        private string currentIconName = string.Empty;
        private void ChangeIcon(string iconName)
        {
            if (!currentIconName.Equals(iconName))
            {
                Image image = this.imageList1.Images[iconName];
                Bitmap bmp = new Bitmap(image);
                IntPtr hicon = bmp.GetHicon();
                notifyIcon1.Icon = Icon.FromHandle(hicon);
                DestroyIcon(hicon);
                currentIconName = iconName;
            }
        }


        [DllImport("user32.dll", SetLastError = true)]
        static extern bool DestroyIcon(IntPtr hIcon);


        private void btnRefresh_Click(object sender, EventArgs e)
        {
            string[] OptionMessage = getOption();
            if (!isRegister)
                isRegister = Register(false, OptionMessage[2], OptionMessage[12]);

            if (isRegister)
            {
                this.cbDataBase.Items.Clear();
                object[] myRet1 = CliUtils.CallMethod("GLModule", "GetDB", null);
                if (myRet1[1] != null && myRet1[1] is ArrayList)
                {
                    ArrayList dbList = (ArrayList)myRet1[1];
                    foreach (string db in dbList)
                    {
                        this.cbDataBase.Items.Add(db);
                    }
                }

                DataSet dsSolution = new DataSet();
                object[] myRet2 = CliUtils.CallMethod("GLModule", "GetSolution", null);
                if ((null != myRet2) && (0 == (int)myRet2[0]))
                    dsSolution = ((DataSet)myRet2[1]);
                this.cbSolution.DataSource = dsSolution.Tables[0];
                this.cbSolution.DisplayMember = "itemname";
                this.cbSolution.ValueMember = "itemtype";
            }
        }
    }
}