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

namespace Scheduling
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

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

        private void checkStartActive()
        {
            string s = Application.StartupPath + "\\SchedulingOptions.xml";
            string isStart = "";
            if (File.Exists(s))
            {
                XmlDocument DBXML = new XmlDocument();
                FileStream aFileStream = new FileStream(s, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                DBXML.Load(aFileStream);
                XmlNode aNode = DBXML.DocumentElement.FirstChild;
                isStart = aNode.Attributes["StartActive"].Value;
                aFileStream.Close();
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
                    FileStream aFileStream = new FileStream(s, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
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

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
                this.Hide();
        }

        private void TSMainNew_Click(object sender, EventArgs e)
        {
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
                FileStream aFileStream = new FileStream(s, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
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
            String sPath = Application.StartupPath + "\\Scheduling.xml";
            if (File.Exists(sPath))
            {
                bDBXML = new XmlDocument();
                bFileStream = new FileStream(sPath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                bDBXML.Load(bFileStream);
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

            Assembly a = this.GetType().Assembly;
            Stream st = a.GetManifestResourceStream("Scheduling.Resources.Scheduling16.ico");
            Icon ic1 = new Icon(st);
            notifyIcon1.Icon = ic1;

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

            Assembly a = this.GetType().Assembly;
            Stream st = a.GetManifestResourceStream("Scheduling.Resources.SchedulingStop16.ico");
            Icon ic2 = new Icon(st);
            notifyIcon1.Icon = ic2;
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
            String[] option = getOption();
            timer.Interval = Convert.ToInt16(option[6]) * 1000;

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

                            DateTime dtLast = new DateTime();
                            if (bNode.Attributes["LastDateTime"].Value == "")
                                bNode.Attributes["LastDateTime"].Value = dtLast.ToString();
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
                                    }
                                    break;
                                case ("Daily"):
                                    if (((TimeSpan)(dtNow.Date - dtLast.Date)).TotalDays >= 1)
                                    { Execute(dtNow, bNode); break; }

                                    string[] hour = bNode.Attributes["Cycle"].Value.Split(',');
                                    int LastHour = dtLast.Hour;
                                    if (dtNow.Date > dtLast.Date)
                                    {
                                        for (int j = 0; j < hour.Length; j++)
                                            if (LastHour < Convert.ToInt32(hour[j]))
                                            { Execute(dtNow, bNode); break; }
                                        LastHour = -1;
                                    }

                                    for (int j = 0; j < hour.Length; j++)
                                        if (dtNow > dtLast && LastHour < Convert.ToInt32(hour[j]) && dtNow.Hour >= Convert.ToInt32(hour[j]))
                                        {
                                            Execute(dtNow, bNode);
                                            break;
                                        }
                                    break;
                                case ("Monthly"):
                                    if (dtNow.Hour * 60 + dtNow.Minute >= CycleHour.Hour * 60 + CycleHour.Minute)
                                    {
                                        if (((TimeSpan)(dtNow.Date - dtLast.Date)).TotalDays >= 31)
                                        { Execute(dtNow, bNode); break; }

                                        string[] day = bNode.Attributes["Cycle"].Value.Split(',');
                                        int LastDay = dtLast.Day;
                                        if (dtNow.Year > dtLast.Year || dtNow.Month > dtLast.Month)
                                        {
                                            for (int j = 0; j < day.Length; j++)
                                                if (LastDay < Convert.ToInt32(day[j]))
                                                { Execute(dtNow, bNode); break; }
                                            LastDay = -1;
                                        }

                                        for (int j = 0; j < day.Length; j++)
                                            if (dtNow > dtLast && LastDay < Convert.ToInt32(day[j]) && dtNow.Day >= Convert.ToInt32(day[j]))
                                            {
                                                Execute(dtNow, bNode);
                                                break;
                                            }
                                    }
                                    break;
                                case ("Weekly"):
                                    if (dtNow.Hour * 60 + dtNow.Minute >= CycleHour.Hour * 60 + CycleHour.Minute)
                                    {
                                        if (((TimeSpan)(dtNow.Date - dtLast.Date)).TotalDays >= 7)
                                        { Execute(dtNow, bNode); break; }

                                        string[] day = bNode.Attributes["Cycle"].Value.Split(',');
                                        int LastDay = changeDays(dtLast.DayOfWeek.ToString());
                                        int NowDay = changeDays(dtNow.DayOfWeek.ToString());
                                        if (LastDay > NowDay)
                                        {
                                            for (int j = 0; j < day.Length; j++)
                                                if (LastDay < Convert.ToInt32(day[j]))
                                                { Execute(dtNow, bNode); break; }
                                            LastDay = -1;
                                        }

                                        for (int j = 0; j < day.Length; j++)
                                            if (dtNow > dtLast && LastDay < Convert.ToInt32(day[j]) && NowDay >= Convert.ToInt32(day[j]))
                                            {
                                                Execute(dtNow, bNode);
                                                break;
                                            }
                                    }
                                    break;
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

            switch (aNode.Attributes["When"].Value)
            {
                case ("All"):
                    t.Start(aNode);

                    //Do(aNode);
                    break;
                case ("Night"):
                    if (dtNow.TimeOfDay >= dtNight.TimeOfDay || dtNow.TimeOfDay < dtDay.TimeOfDay)
                        t.Start(aNode);

                        //Do(aNode, isTest);
                    break;
                case ("Day"):
                    if (dtNow.TimeOfDay < dtNight.TimeOfDay || dtNow.TimeOfDay >= dtDay.TimeOfDay)
                        t.Start(aNode);

                        //Do(aNode, isTest);
                    break;
                case ("Holiday"):
                    int Now = changeDays(dtNow.DayOfWeek.ToString());
                    for (int j = 0; j < Holiday.Count; j++)
                        if (Now == Convert.ToInt32(Holiday[j]))
                            t.Start(aNode);

                            //Do(aNode, isTest);
                    break;
                case ("Workday"):
                    bool work = true;
                    int Day = changeDays(dtNow.DayOfWeek.ToString());
                    for (int j = 0; j < Holiday.Count; j++)
                        if (Day == Convert.ToInt32(Holiday[j]))
                        { work = false; break; }
                    if (work == true)
                        t.Start(aNode);

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
                    aNode.Attributes["IsRunning"].Value = "1";
                    logMessage = Login(DB, Solution);
                }
                catch
                {
                    aNode.Attributes["IsRunning"].Value = "0";
                    logMessage[0] = "false";
                }

                if (logMessage[0] == "false")
                {
                    int errCount = Convert.ToInt16(aNode.Attributes["ErrorCount"].Value) + 1;
                    aNode.Attributes["ErrorCount"].Value = errCount.ToString();

                    String strMailMessage = "";
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
                                break;
                        }
                    }

                    String LogMessage = logMessage[1] + " " + strMailMessage;
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
                    if (aNode.Attributes["SchedulingMode"].Value == "Server Method(async)")
                    {
                        object[] myParam = new object[param.Length + 1];
                        for (int i = 0; i < param.Length; i++)
                        {
                            myParam[i] = param[i];
                        }
                        myParam[param.Length] = aNode.Attributes["SchedulName"].Value;
                        try
                        {
                            CliUtils.AsyncCallMethod(Method[0], Method[1], myParam, CallBack);
                        }
                        catch
                        {
                        }
                    }
                    else if (aNode.Attributes["SchedulingMode"].Value == "Server Method(sync)")
                    {
                        try
                        {
                            myRet = CliUtils.CallMethod(Method[0], Method[1], param, true);
                        }
                        catch (Exception ex)
                        {
                            aNode.Attributes["IsRunning"].Value = "0";
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
                            if (aNode.Attributes["SendMailMode"] != null)
                            {
                                switch (aNode.Attributes["SendMailMode"].Value)
                                {
                                    case "None": break;
                                    case "All":
                                        if ((aNode.Attributes["Error"].Value != "") && Convert.ToInt16(aNode.Attributes["ErrorCount"].Value) <= ErrorCount)
                                        {
                                            if (myRet[0].ToString() != "0")
                                                strMailMessage = sendErrorEmail(aNode, myRet[1].ToString() + " " + catchMessage);
                                            else
                                                strMailMessage = sendErrorEmail(aNode, catchMessage);
                                        }
                                        break;
                                    case "ErrorOnly":
                                        if ((myRet[0].ToString() != "0" || catchMessage != "") && Convert.ToInt16(aNode.Attributes["ErrorCount"].Value) <= ErrorCount)
                                        {
                                            if (aNode.Attributes["Error"].Value != "")
                                                strMailMessage = sendErrorEmail(aNode, myRet[1].ToString() + " " + catchMessage);
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
                                        Log(aNode, "Error", strMailMessage);
                                    }
                                    else
                                    {
                                        if (myRet[0].ToString() != "0" || catchMessage != "")
                                        {
                                            if (Convert.ToInt16(aNode.Attributes["ErrorCount"].Value) <= ErrorCount)
                                            {
                                                if (myRet.Length > 1)
                                                    Log(aNode, "Error", myRet[1].ToString() + " " + strMailMessage);
                                                else
                                                    Log(aNode, "Error", myRet[0].ToString() + " " + strMailMessage);
                                            }
                                        }
                                        else
                                            Log(aNode, "Succeed", "");
                                    }
                                    break;
                                case ("ErrorOnly"):
                                    if ((myRet[0].ToString() != "0" || catchMessage != "") && Convert.ToInt16(aNode.Attributes["ErrorCount"].Value) <= ErrorCount)
                                        Log(aNode, "Error", myRet[1].ToString() + " " + strMailMessage);
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
                            aNode.Attributes["LastDateTime"].Value = DateTime.Now.ToString();
                            aNode.Attributes["IsRunning"].Value = "0";
                            LogOut();
                        }
                    }
                    else if (aNode.Attributes["SchedulingMode"].Value == "Server FLMethod(sync)")
                    {
                        try
                        {
                            myRet = CliUtils.CallFLMethod(Method[1], param);
                        }
                        catch (Exception ex)
                        {
                            aNode.Attributes["IsRunning"].Value = "0";
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
                            if (aNode.Attributes["SendMailMode"] != null)
                            {
                                switch (aNode.Attributes["SendMailMode"].Value)
                                {
                                    case "None": break;
                                    case "All":
                                        if ((aNode.Attributes["Error"].Value != "") && Convert.ToInt16(aNode.Attributes["ErrorCount"].Value) <= ErrorCount)
                                        {
                                            if (myRet[0].ToString() != "0")
                                                strMailMessage = sendErrorEmail(aNode, myRet[1].ToString() + " " + catchMessage);
                                            else
                                                strMailMessage = sendErrorEmail(aNode, catchMessage);
                                        }
                                        break;
                                    case "ErrorOnly":
                                        if ((myRet[0].ToString() != "0" || catchMessage != "") && Convert.ToInt16(aNode.Attributes["ErrorCount"].Value) <= ErrorCount)
                                        {
                                            if (aNode.Attributes["Error"].Value != "")
                                                strMailMessage = sendErrorEmail(aNode, myRet[1].ToString() + " " + catchMessage);
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
                                        Log(aNode, "Error", strMailMessage);
                                    }
                                    else
                                    {
                                        if (myRet[0].ToString() != "0" || catchMessage != "")
                                        {
                                            if (Convert.ToInt16(aNode.Attributes["ErrorCount"].Value) <= ErrorCount)
                                            {
                                                if (myRet.Length > 1)
                                                    Log(aNode, "Error", myRet[1].ToString() + " " + strMailMessage);
                                                else
                                                    Log(aNode, "Error", myRet[0].ToString() + " " + strMailMessage);
                                            }
                                        }
                                        else
                                            Log(aNode, "Succeed", "");
                                    }
                                    break;
                                case ("ErrorOnly"):
                                    if ((myRet[0].ToString() != "0" || catchMessage != "") && Convert.ToInt16(aNode.Attributes["ErrorCount"].Value) <= ErrorCount)
                                        Log(aNode, "Error", myRet[1].ToString() + " " + strMailMessage);
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

                            aNode.Attributes["LastDateTime"].Value = DateTime.Now.ToString();
                            aNode.Attributes["IsRunning"].Value = "0";
                            LogOut();
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
                if (aNode.Attributes["SendMailMode"] != null)
                {
                    switch (aNode.Attributes["SendMailMode"].Value)
                    {
                        case "None": break;
                        case "All":
                            if (aNode.Attributes["Error"].Value != "")
                                sendErrorEmail(aNode, "Succeed");
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
                        Log(aNode, "Succeed", "");
                        break;
                    case ("ErrorOnly"):
                        break;
                }

                //if (isTest)
                //    MessageBox.Show("Test is succeed.");
                aNode.Attributes["LastDateTime"].Value = DateTime.Now.ToString();
            }
        }

        public void CallBack(object[] myRet)
        {
            try
            {
                if (bDBXML != null)
                {
                    XmlNode bNode = bDBXML.DocumentElement.FirstChild;

                    while (bNode != null)
                    {
                        if (bNode.Attributes["SchedulName"].Value == myRet[myRet.Length - 1].ToString())
                        {
                            break;
                        }
                        bNode = bNode.NextSibling;
                    }

                    String Status = "Succeed";
                    String catchMessage = "";
                    if (myRet[0] == null || myRet[0].ToString() != "0" || catchMessage != "")
                        Status = "Error";

                    String strMailMessage = "";
                    if (bNode.Attributes["SendMailMode"] != null)
                    {
                        switch (bNode.Attributes["SendMailMode"].Value)
                        {
                            case "None": break;
                            case "All":
                                if (bNode.Attributes["Error"].Value != "" && Convert.ToInt16(bNode.Attributes["ErrorCount"].Value) <= ErrorCount)
                                {
                                    if (myRet[0].ToString() != "0")
                                        strMailMessage = sendErrorEmail(bNode, myRet[1].ToString() + " " + catchMessage);
                                    else
                                        strMailMessage = sendErrorEmail(bNode, catchMessage);
                                }
                                break;
                            case "ErrorOnly":
                                if ((myRet[0].ToString() != "0" || catchMessage != "") && Convert.ToInt16(bNode.Attributes["ErrorCount"].Value) <= ErrorCount)
                                {
                                    if (bNode.Attributes["Error"].Value != "")
                                        strMailMessage = sendErrorEmail(bNode, myRet[1].ToString() + " " + catchMessage);
                                }
                                break;
                        }
                    }

                    DateTime dtEnd = DateTime.Now;
                    switch (bNode.Attributes["LogScheduling"].Value)
                    {
                        case ("None"):
                            break;
                        case ("All"):
                            if (myRet[0].ToString() != "0" || catchMessage != "")
                                if (Convert.ToInt16(bNode.Attributes["ErrorCount"].Value) <= ErrorCount)
                                {
                                    Log(bNode, "Error", myRet[1].ToString() + " " + strMailMessage);
                                }
                                else
                                    Log(bNode, "Succeed", "");
                            break;
                        case ("ErrorOnly"):
                            if ((myRet[0].ToString() != "0" || catchMessage != "") && Convert.ToInt16(bNode.Attributes["ErrorCount"].Value) <= ErrorCount)
                                Log(bNode, "Error", myRet[1].ToString() + " " + strMailMessage);
                            break;
                    }
                    bNode.Attributes["IsRunning"].Value = "0";
                    bNode.Attributes["LastDateTime"].Value = DateTime.Now.ToString();
                    LogOut();
                }
            }
            catch { }
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

            FileStream file = File.Open(s, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);

            ArrayList alLog = new ArrayList();
            StreamReader sreader = new StreamReader(file);
            alLog.Add(sreader.ReadToEnd());
            sreader.Close();
            alLog.Add("SchedulName: " + aNode.Attributes["SchedulName"].Value + " Status: " + str + " Message:" + Message);
            alLog.Add("LastDateTime: " + aNode.Attributes["LastDateTime"].Value + " ExecuteTime: " + DateTime.Now);
            alLog.Add("");
            file = File.Open(s, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            StreamWriter swriter = new StreamWriter(file);
            for (int i = 0; i < alLog.Count; i++)
                swriter.WriteLine(alLog[i]);
            swriter.Close();
            file.Close();
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

            String message = "";
            try
            {
                //Builed The MSG
                MailMessage msg = new MailMessage();
                msg.To.Add(aNode.Attributes["Error"].Value);
                msg.From = new MailAddress(Email, "Infolight", System.Text.Encoding.UTF8);
                msg.Subject = "SchedulName: " + aNode.Attributes["SchedulName"].Value;
                msg.SubjectEncoding = System.Text.Encoding.UTF8;
                msg.Body = "SchedulName: " + aNode.Attributes["SchedulName"].Value + ". Messages :" + errMessage + ". LastDateTime: " + aNode.Attributes["LastDateTime"].Value + " ExecuteTime: " + DateTime.Now;
                msg.BodyEncoding = System.Text.Encoding.UTF8;
                msg.IsBodyHtml = false;
                msg.Priority = MailPriority.High;

                //Add the Creddentials
                SmtpClient client = new SmtpClient();
                client.Credentials = new System.Net.NetworkCredential(Email, EmailPwd);
                client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;

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

        private void sendEmail(string Message, bool isConnect)
        {
            String[] option = getOption();
            String Email = option[3];
            String EmailPwd = option[4];
            String Host = option[5];
            String Port = option[10];
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

            if (!File.Exists(s))
            {
                try
                {
                    aFileStream = new FileStream(s, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
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
                aFileStream = new FileStream(s, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                try
                {
                    if (this.Text.Contains("(Active)"))
                        DBXML = bDBXML;
                    else
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
                        attr.Value = "";
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

                bDBXML = new XmlDocument();
                bFileStream = new FileStream(sPath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                bDBXML.Load(bFileStream);
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

        private string[] getOption()
        {
            String s = Application.StartupPath + "\\SchedulingOptions.xml";
            String[] option = new String[13];

            if (File.Exists(s))
            {
                XmlDocument DBXML = new XmlDocument();
                FileStream aFileStream = new FileStream(s, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
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

                aFileStream.Close();
            }

            return option;
        }

        private string[] Login(string DB, string sn)
        {
            string[] OptionMessage = getOption();

            if (!isRegister)
            {
                isRegister = Register(false, OptionMessage[2]);
                if (!isRegister)
                    return new string[] { "false", "Register is field." };
            }

            CliUtils.fLoginUser = OptionMessage[0];
            CliUtils.fLoginPassword = OptionMessage[1];
            CliUtils.fLoginDB = DB;
            CliUtils.fCurrentProject = sn;
            if (CliUtils.fLoginUser.ToLower().Contains("'"))
            {
                string message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "EEPWebNetClient", "WinSysMsg", "msg_UserNotFound");
                return new string[] { "false", message };
            }

            string sParam = CliUtils.fLoginUser + ':' + CliUtils.fLoginPassword + ':' + CliUtils.fLoginDB + ':' + "0";

            object[] myRet = CliUtils.CallMethod("GLModule", "CheckUser", new object[] { (object)sParam });
            if (myRet[1].ToString() == "1")
            {
                string message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "EEPWebNetClient", "WinSysMsg", "msg_UserOrPasswordError");
                return new string[] { "false", message };
            }
            else if (myRet[1].ToString() == "3")
            {
                string message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "EEPWebNetClient", "WinSysMsg", "msg_UserNotFound");
                return new string[] { "false", message };
            }
            //else if (myRet[1].ToString() == "2")
            //{
            //    //SYS_LANGUAGE language = CliSysMegLag.GetClientLanguage();
            //    SYS_LANGUAGE language = CliUtils.fClientLang;
            //    String message = SysMsg.GetSystemMessage(language, "EEPWebNetClient", "WinSysMsg", "msg_UserIsLogined");
            //    //if (MessageBox.Show(String.Format(message, CliUtils.fLoginUser), "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            //    //{
            //    //    sParam = CliUtils.fLoginUser + ':' + CliUtils.fLoginPassword + ':' + CliUtils.fLoginDB + ':' + "1";
            //    //    goto Label2;
            //    //}
            //}
            else//suceed to login...
            {
                CliUtils.fUserName = myRet[2].ToString();
                CliUtils.fLoginUser = myRet[3].ToString();
                myRet = CliUtils.CallMethod("GLModule", "GetUserGroup", new object[] { CliUtils.fLoginUser });
                if (myRet != null && (int)myRet[0] == 0)
                    CliUtils.fGroupID = myRet[1].ToString();
                SaveToClientXML(CliUtils.fLoginUser, CliUtils.fLoginDB, CliUtils.fCurrentProject);
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

            FileStream aFileStream = new FileStream(sfile, FileMode.Create);
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

        private void btnTest_Click(object sender, EventArgs e)
        {
            if (this.tbName.Text != "")
            {
                String sPath = Application.StartupPath + "\\Scheduling.xml";
                if (File.Exists(sPath))
                {
                    bDBXML = new XmlDocument();
                    bFileStream = new FileStream(sPath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                    bDBXML.Load(bFileStream);
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
                ServerMethod frmsm = new ServerMethod(sName);
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
                    string.Format("http://{0}:{1}/InfoRemoteModule.rem", CliUtils.fRemoteIP, CliUtils.fRemotePort)) as EEPRemoteModule;
                module.ToString();
                if (isConnect == 1)
                {
                    sendEmail("Connect server succeed again.", false);
                }
                isConnect = 0;

                Assembly a = this.GetType().Assembly;
                Stream st = a.GetManifestResourceStream("Scheduling.Resources.Scheduling16.ico");
                Icon ic1 = new Icon(st);
                notifyIcon1.Icon = ic1;
            }
            catch (Exception ex)
            {
                if (isConnect == 0 || isConnect == -1)
                {
                    sendEmail(ex.Message, true);
                }
                Assembly a = this.GetType().Assembly;
                Stream st = a.GetManifestResourceStream("Scheduling.Resources.SchedulingCanConnect.ico");
                Icon ic1 = new Icon(st);
                notifyIcon1.Icon = ic1;
                isConnect = 1;


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