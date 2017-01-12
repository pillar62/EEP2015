using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Data;
using System.IO;
using System.Windows.Forms;
using System.Drawing.Design;
using System.Drawing;
using System.Collections;
using System.Windows.Forms.Design;
using System.Threading;
using System.Xml;
using Microsoft.Win32;
using System.Reflection;

namespace Srvtools
{
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(AutoTest), "Resources.AutoTest.png")]
    public partial class AutoTest : InfoBaseComp
    {
        private SYS_LANGUAGE language;
        public AutoTest()
        {
            //language = CliSysMegLag.GetClientLanguage();
            language = CliUtils.fClientLang;
            _Columns = new infoColumns(this, typeof(infoColumn));
            InitializeComponent();
        }

        protected override void DoAfterSetOwner(IDataModule value)
        {
            //foreach (Control c in ((InfoForm)this.OwnerComp).Controls)
            //{
            //    if (c is Button)
            //    {
            //        if (((Button)c).Text == this.ClickControl)
            //            ((Button)c).PerformClick();
            //    }
            //    else if (c is InfoNavigator)
            //    {
            //        foreach (ToolStripItem tsi in ((InfoNavigator)c).Items)
            //        {
            //            if (tsi.Text == this.ClickControl)
            //                tsi.PerformClick();
            //        }
            //    }
            //}
        }

        public void AutoClick(int Times, String pn, String un, int Interval)
        {
            for (int i = 0; i < Times; i++)
            {
                foreach (Control c in ((InfoForm)this.OwnerComp).Controls)
                {
                    if (c is Button)
                    {
                        if (((Button)c).Text == this.ClickControl)
                        {
                            BttonClick((Button)c, pn, un);
                        }
                    }
                    else if (c is InfoNavigator)
                    {
                        foreach (ToolStripItem tsi in ((InfoNavigator)c).Items)
                        {
                            if (tsi.Text == this.ClickControl)
                                tsi.PerformClick();
                        }
                    }
                    else if (c is Panel || c is SplitterPanel || c is SplitContainer)
                    {
                        AutoClick(c, pn, un);
                    }
                }

                System.Threading.Thread.Sleep(Interval);
            }
            WriteLog();
        }

        private void AutoClick(Control cc, String pn, String un)
        {
            foreach (Control c in cc.Controls)
            {
                if (c is Button)
                {
                    if (((Button)c).Text == this.ClickControl)
                    {
                        BttonClick((Button)c, pn, un);
                    }
                }
                else if (c is InfoNavigator)
                {
                    foreach (ToolStripItem tsi in ((InfoNavigator)c).Items)
                    {
                        if (tsi.Text == this.ClickControl)
                            tsi.PerformClick();
                    }
                }
                else if (c is Panel || c is SplitterPanel || c is SplitContainer)
                {
                    AutoClick(c, pn, un);
                }
            }
        }

        private void BttonClick(Button btn, String packageName, String userName)
        {
            DateTime start = DateTime.Now;
            String startTime = start.ToLongTimeString() + "." + start.Millisecond.ToString();
            btn.PerformClick();
            DateTime end = DateTime.Now;
            String endTime = end.ToLongTimeString() + "." + end.Millisecond.ToString();
            TimeSpan ts = end - start;

            alLog.Add("UserName : " + userName + ". PackageName : " + packageName + ". ButtonName: " + btn.Text + "   StartTime: " + startTime);
            alLog.Add("UserName : " + userName + ". PackageName : " + packageName + ". ButtonName: " + btn.Text + "   EndTime: " + endTime);
            alLog.Add("UserName : " + userName + ". PackageName : " + packageName + ". ButtonName: " + btn.Text + "   Between: " + ts.TotalMilliseconds.ToString());
            alLog.Add("");
        }

        private void WriteLog()
        {
            if (this.Log != null && this.Log != "")
            {
                string s = null;
                if (Log.Length > 4)
                {
                    for (int h = 0; h < 4; h++)
                        s = s + this.Log[Log.Length - 4 + h];
                    if (string.Compare(s, ".txt", true) != 0)//IgnoreCase
                        this.Log = this.Log + ".txt";
                }
                else
                    this.Log = this.Log + ".txt";

                try
                {
                    if (File.Exists(this.Log))
                    {
                        FileStream fs = File.Open(this.Log, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                        StreamReader sreader = new StreamReader(fs);
                        alLog.Add(sreader.ReadToEnd());
                        sreader.Close();

                        fs = File.Open(this.Log, FileMode.Open, FileAccess.Write, FileShare.ReadWrite);
                        StreamWriter swriter = new StreamWriter(fs);
                        for (int i = 0; i < alLog.Count; i++)
                        {
                            swriter.WriteLine(alLog[i]);
                        }
                        swriter.Close();
                        fs.Close();
                    }
                    else
                    {
                        language = CliUtils.fClientLang;
                        string message = SysMsg.GetSystemMessage(language, "Srvtools", "AutoTest", "LogError");
                        MessageBox.Show(message);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    MessageBox.Show(ex.GetType().ToString());
                }
            }
        }

        private InfoBindingSource _infoBindingSource;
        [Category("Infolight"),
        Description("The InfoBindingSource which the control is bound to")]
        public InfoBindingSource infoBindingSource
        {
            get
            {
                return _infoBindingSource;
            }
            set
            {
                if (value == null || value is InfoBindingSource)
                    _infoBindingSource = value;
            }
        }

        private string _ClickControl;
        [Category("Infolight"),
        Description("Specifies the control to perform click event when the form loads")]
        [Editor(typeof(GetControl), typeof(UITypeEditor))]
        public string ClickControl
        {
            get
            {
                return _ClickControl;
            }
            set
            {
                _ClickControl = value;
            }
        }

        private infoColumns _Columns;
        [Category("Infolight"),
        Description("Specifies the columns which AutoTest is applied to")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public infoColumns Columns
        {
            get
            {
                return _Columns;
            }
            set
            {
                _Columns = value;
            }
        }

        private ArrayList ChildAutoTest = new ArrayList();
        private AutoTest _ParentAutoTest;
        [Category("Infolight"),
        Description("The AutoTest of the master table while the control is the AutoTest of the detail table")]
        public AutoTest ParentAutoTest
        {
            get
            {
                return _ParentAutoTest;
            }
            set
            {
                _ParentAutoTest = value;
                if (_ParentAutoTest != null)
                {
                    ParentAutoTest.ChildAutoTest.Add(this);
                }
            }
        }

        private int _Rows = 1;
        [Category("Infolight"),
        Description("Specifies the amount of the records to insert each time")]
        public int Rows
        {
            get
            {
                return _Rows;
            }
            set
            {
                if (this.infoBindingSource != null)
                {
                    InfoDataSet ids = (InfoDataSet)this.infoBindingSource.GetDataSource();
                    if (this.infoBindingSource.DataSource == ids)
                        _Rows = 1;
                    else
                        _Rows = value;
                }
            }
        }

        private string _Log;
        [Category("Infolight"),
        Description("The name of file to store the log of AutoTest")]
        public string Log
        {
            get
            {
                return _Log;
            }
            set
            {
                _Log = value;

            }
        }

        private string _KeyField;
        [Category("Infolight"),
        Description("Prime key of AutoTest")]
        [Editor(typeof(GetKeyField), typeof(UITypeEditor))]
        public string KeyField
        {
            get
            {
                return _KeyField;
            }
            set
            {
                _KeyField = value;
            }
        }

        private int _RecordTime = 500;
        [Category("Infolight")]
        public int RecordTime
        {
            get
            {
                return _RecordTime;
            }
            set
            {
                _RecordTime = value;
            }
        }

        private bool _isFlowClient = false;
        [Category("Infolight")]
        [DefaultValue(false)]
        public bool IsFlowClient
        {
            get
            {
                return _isFlowClient;
            }
            set
            {
                _isFlowClient = value;
            }
        }

        private string _flowFileName = "";
        [Browsable(false)]
        public string FlowFileName
        {
            get
            {
                return _flowFileName;
            }
            set
            {
                _flowFileName = value;
            }
        }

        private string GetDefaultValue(infoColumn ifColumn)
        {
            return CliUtils.GetValue(ifColumn.Value, this.OwnerComp).ToString();
            //return GetValue(ifColumn.Value);
        }

        private string GetValue(string value)
        {
            Char[] cs = value.ToCharArray();

            object[] myret = CliUtils.GetValue(value);
            if (myret != null && (int)myret[0] == 0)
            {
                return (string)myret[1];
            }

            if (cs[0] != '"' && cs[0] != '\'')
            {
                Char[] sep1 = "()".ToCharArray();
                String[] sps1 = value.Split(sep1);

                if (sps1.Length == 3)
                {
                    return InvokeOwerMethod(sps1[0], null);
                }

                if (sps1.Length == 1)
                {
                    return sps1[0];
                }

                if (sps1.Length != 1 && sps1.Length == 3)
                {
                    //SYS_LANGUAGE language = CliSysMegLag.GetClientLanguage();
                    SYS_LANGUAGE language = CliUtils.fClientLang;
                    String message = SysMsg.GetSystemMessage(language, "Srvtools", "AutoTest", "msg_AutoTestPropertyIsError");

                    MessageBox.Show(string.Format(message), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return "";
                }
            }

            Char[] sep2 = null;
            if (cs[0] == '"')
            {
                sep2 = "\"".ToCharArray();
            }

            if (cs[0] == '\'')
            {
                sep2 = "'".ToCharArray();
            }

            String[] sps2 = value.Split(sep2);
            if (sps2.Length == 3)
            { return sps2[1]; }
            else
            {
                //SYS_LANGUAGE language = CliSysMegLag.GetClientLanguage();
                SYS_LANGUAGE language = CliUtils.fClientLang;
                String message = SysMsg.GetSystemMessage(language, "Srvtools", "AutoTest", "msg_AutoTestPropertyIsError");

                MessageBox.Show(string.Format(message), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "";
            }
        }

        private String InvokeOwerMethod(String methodName, Object[] parameters)
        {
            MethodInfo methodInfo = this.OwnerComp.GetType().GetMethod(methodName);

            Object obj = null;
            if (methodInfo != null)
            {
                obj = methodInfo.Invoke(this.OwnerComp, parameters);
            }
            else
            {
                //SYS_LANGUAGE language = CliSysMegLag.GetClientLanguage();
                SYS_LANGUAGE language = CliUtils.fClientLang;
                String message = SysMsg.GetSystemMessage(language, "Srvtools", "AutoTest", "msg_DefMethodNotFound");
                Exception ex = new Exception(string.Format(message, methodName + "()"));
                CliUtils.Application_ThreadException(null, new System.Threading.ThreadExceptionEventArgs(ex));
            }

            if (obj != null)
            {
                return obj.ToString();
            }
            else
            {
                return "";
            }
        }

        InfoDataSet ids = null;
        private Double MaxRespendTime, MinRespendTime;
        private Double AverageRespendTime;
        private ArrayList alLog = new ArrayList();
        public void ExecuteTest(int Times, int Interval)
        {
            ExecuteTest(Times, Interval, CliUtils.fLoginUser, "", "");
        }

        public void ExecuteTest(int Times, int Interval, string package)
        {
            ExecuteTest(Times, Interval, CliUtils.fLoginUser, package, "");
        }

        public void ExecuteTest(int Times, int Interval, string user, string package, string Fixed)
        {
            MaxRespendTime = 0;
            MinRespendTime = 0;
            AverageRespendTime = 0;

            //if (this.ClickControl == null || this.ClickControl.ToString() == "(none)" || this.ClickControl.ToString() == "")
            //{

            DateTime start = DateTime.Now;
            alLog.Add("AutoTest log file.");
            alLog.Add("UserID: " + user + "   PackageName: " + package + "   Start Time: " + start + " " + start.Millisecond);

            int temp = 0;
            if (user[user.Length - 1] > 47 && user[user.Length - 1] < 58)
            {
                switch (user[user.Length - 1])
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
            }
            else
                temp = m_rnd.Next(0, 9);

            for (int i = 0; i < Times; i++)
            {
                int number = i + 1;
                if (number % (temp + 1) == 0 || i == temp || number == Times)
                {
                    try
                    {
                        string strPath = Application.StartupPath + "\\";
                        XmlDocument DBXML = new XmlDocument();
                        FileStream aFileStream;
                        if (!File.Exists(strPath + Fixed + "AutoRunMessage.xml"))
                        {
                            try
                            {
                                aFileStream = new FileStream(strPath + Fixed + "AutoRunMessage.xml", FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
                                try
                                {
                                    XmlTextWriter w = new XmlTextWriter(aFileStream, new System.Text.ASCIIEncoding());
                                    w.Formatting = Formatting.Indented;
                                    w.WriteStartElement("InfolightAutoRunMessage");
                                    w.WriteEndElement();
                                    w.Close();
                                }
                                finally
                                {
                                    aFileStream.Close();
                                }
                            }
                            catch (Exception e) { string str = e.Message; }
                        }

                        try
                        {
                            aFileStream = new FileStream(strPath + Fixed + "AutoRunMessage.xml", FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                            try
                            {
                                DBXML.Load(aFileStream);
                                XmlNode aNode = null;

                                for (int j = DBXML.DocumentElement.ChildNodes.Count - 1; j >= 0; j--)
                                {
                                    if (string.Compare(DBXML.DocumentElement.ChildNodes[j].Attributes["UserId"].InnerText.Trim(), user, true) == 0//IgnoreCase
                                        && string.Compare(DBXML.DocumentElement.ChildNodes[j].Attributes["Module"].InnerText.Trim(), package, true) == 0)//IgnoreCase
                                    {
                                        aNode = DBXML.DocumentElement.ChildNodes[j];
                                        break;
                                    }
                                }

                                if (aNode == null)
                                {
                                    XmlElement elem = DBXML.CreateElement("String");

                                    XmlAttribute attr = DBXML.CreateAttribute("UserId");
                                    attr.Value = user;
                                    elem.Attributes.Append(attr);

                                    attr = DBXML.CreateAttribute("Module");
                                    attr.Value = package;
                                    elem.Attributes.Append(attr);

                                    attr = DBXML.CreateAttribute("StartTime");
                                    attr.Value = start.ToString();
                                    elem.Attributes.Append(attr);

                                    attr = DBXML.CreateAttribute("Number");
                                    attr.Value = number.ToString();
                                    elem.Attributes.Append(attr);

                                    attr = DBXML.CreateAttribute("Times");
                                    attr.Value = Times.ToString();
                                    elem.Attributes.Append(attr);

                                    attr = DBXML.CreateAttribute("Status");
                                    attr.Value = ((float)(number) / (float)(Times)) * 100 + "%";
                                    elem.Attributes.Append(attr);

                                    attr = DBXML.CreateAttribute("CompleteTime");
                                    if (number == Times)
                                        attr.Value = DateTime.Now.ToString();
                                    else
                                        attr.Value = "";
                                    elem.Attributes.Append(attr);

                                    attr = DBXML.CreateAttribute("Flag");
                                    if (number == Times)
                                        attr.Value = "1";
                                    else
                                        attr.Value = "";
                                    elem.Attributes.Append(attr);


                                    DBXML.DocumentElement.AppendChild(elem);
                                }
                                else
                                {
                                    aNode.Attributes["Number"].InnerText = number.ToString();
                                    aNode.Attributes["Status"].InnerText = ((float)(number) / (float)(Times)) * 100 + "%";
                                    if (number == Times)
                                    {
                                        aNode.Attributes["CompleteTime"].InnerText = DateTime.Now.ToString();
                                        aNode.Attributes["Flag"].InnerText = "1";
                                    }

                                }
                            }
                            finally
                            {
                                aFileStream.Close();
                            }
                            DBXML.Save(strPath + Fixed + "AutoRunMessage.xml");
                        }
                        catch (Exception e) { string str = e.Message; }
                    }
                    finally
                    {
                    }
                }
                DateTime BeforeExecute = DateTime.Now;
                Execute(user);
                DateTime AfterExecute = DateTime.Now;

                TimeSpan betweenExecute = AfterExecute - BeforeExecute;
                if (betweenExecute.TotalMilliseconds > RecordTime)
                {
                alLog.Add("Times: " + (i + 1) + "   BetweenExecute: " + betweenExecute.TotalMilliseconds);
}
                DateTime BeforeApply = DateTime.Now;

                DataTable t = ids.RealDataSet.Tables[0].GetChanges(DataRowState.Added);
                DataTable cloneTable = null;
                if (t != null) cloneTable = t.Copy();

                try
                {
                    ids.ApplyUpdates();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }

                if (cloneTable != null)
                {
                    foreach (DataRow row in cloneTable.Rows)
                        this.SubmitFlow(row);
                }

                DateTime AfterApply = DateTime.Now;
                TimeSpan betweenApply = AfterApply - BeforeApply;

                if (betweenApply.TotalMilliseconds > RecordTime)
                {
                    alLog.Add("Times: " + (i + 1) + "   BetweenApply: " + betweenApply.TotalMilliseconds);
                }
                MaxRespendTime = Max(BeforeApply, AfterApply, MaxRespendTime);
                MinRespendTime = Min(BeforeApply, AfterApply, MinRespendTime);

                System.Threading.Thread.Sleep(Interval);
            }

            if (this.Log != null && this.Log != "")
            {
                string s = null;
                if (Log.Length > 4)
                {
                    for (int h = 0; h < 4; h++)
                        s = s + this.Log[Log.Length - 4 + h];
                    if (string.Compare(s, ".txt", true) != 0)//IgnoreCase
                        this.Log = this.Log + ".txt";
                }
                else
                    this.Log = this.Log + ".txt";

                AverageRespendTime /= Times;
                DateTime complete = DateTime.Now;
                alLog.Add("UserID: " + user + "   PackageName: " + package + "   Complete Time: " + complete + " " + DateTime.Now.Millisecond);
                alLog.Add("Total " + (Times + 1) + " Records, Use" + (complete - start) + " Seconds.");
                alLog.Add("MaxRespendTime: " + MaxRespendTime + " MilliSeconds ");
                alLog.Add("MinRespendTime: " + MinRespendTime + " MilliSeconds ");
                alLog.Add("AverageRespendTime: " + AverageRespendTime + "MilliSeconds");
                alLog.Add("");

                try
                {
                    if (File.Exists(this.Log))
                    {
                        FileStream fs = File.Open(this.Log, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                        StreamReader sreader = new StreamReader(fs);
                        alLog.Add(sreader.ReadToEnd());
                        sreader.Close();

                        fs = File.Open(this.Log, FileMode.Open, FileAccess.Write, FileShare.ReadWrite);
                        StreamWriter swriter = new StreamWriter(fs);
                        for (int i = 0; i < alLog.Count; i++)
                        {
                            swriter.WriteLine(alLog[i]);
                        }
                        swriter.Close();
                        fs.Close();
                    }
                    else
                    {
                        language = CliUtils.fClientLang;
                        string message = SysMsg.GetSystemMessage(language, "Srvtools", "AutoTest", "LogError");
                        MessageBox.Show(message);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    MessageBox.Show(ex.GetType().ToString());
                }
            }
        }

        private static int deep = 0;
        private int sequence;
        public void Execute(string user)
        {
            if (this.ParentAutoTest != null && (this.KeyField == "" || this.KeyField == null))
            {
                language = CliUtils.fClientLang;
                string message = SysMsg.GetSystemMessage(language, "Srvtools", "AutoTest", "KeyFieldNull");
                MessageBox.Show(message);
                return;
            }
            if (this.infoBindingSource != null)
            {
                if (ids == null)
                {
                ids = (InfoDataSet)this.infoBindingSource.GetDataSource();
                }
                if (this.ParentAutoTest != null || this.infoBindingSource.DataSource == ids)
                {
                    for (int times = 0; times < this.Rows; times++)
                    {
                        List<String> listKey = new List<String>();
                        DataRow dr = ids.RealDataSet.Tables[deep].NewRow();
                        foreach (infoColumn ColumnsItem in Columns)
                        {
                            for (int i = 0; i < ids.RealDataSet.Tables[deep].Columns.Count; i++)
                            {
                                if (ids.RealDataSet.Tables[deep].Columns[i].Caption == ColumnsItem.ColumnName)
                                {
                                    if (deep > 0 && ids.RealDataSet.Tables[0].Rows.Count > 0)
                                    {
                                        DataRow dr_temp = ids.RealDataSet.Tables[0].Rows[ids.RealDataSet.Tables[0].Rows.Count - 1];
                                        if (ColumnsItem.ColumnName == ids.RealDataSet.Tables[0].PrimaryKey.GetValue(0).ToString())
                                        {
                                            dr[ids.RealDataSet.Tables[deep].Columns[i].Caption] = dr_temp[ids.RealDataSet.Tables[0].Columns[i].Caption];
                                            break;
                                        }
                                    }
                                    if (ColumnsItem.valueMode == ValueMode.Fixed)
                                    {
                                        dr[ids.RealDataSet.Tables[deep].Columns[i].Caption] = GetDefaultValue(ColumnsItem);
                                        break;
                                    }
                                    else if (ColumnsItem.valueMode == ValueMode.Random)
                                    {
                                        bool flag = true;
                                        for (int j = 0; j < ColumnsItem.Value.Length; j++)
                                            if (ColumnsItem.Value[j] < 48 || ColumnsItem.Value[j] > 57)
                                                flag = false;
                                        for (int j = 0; j < ColumnsItem.RandomTo.Length; j++)
                                            if (ColumnsItem.RandomTo[j] < 48 || ColumnsItem.RandomTo[j] > 57)
                                                flag = false;
                                        if (flag == false)
                                        {
                                            int length;
                                            String value = null;
                                            if (ColumnsItem.Value.Length < ColumnsItem.RandomTo.Length)
                                                length = ColumnsItem.Value.Length;
                                            else
                                                length = ColumnsItem.RandomTo.Length;

                                        Label1:
                                            StringBuilder sb = new StringBuilder(length);
                                            for (int x = 0; x < length; x++)
                                            {
                                                sb.Append(getRandomChar(ColumnsItem.Value[x], ColumnsItem.RandomTo[x]));
                                            }
                                            for (int x = 0; x < ColumnsItem.RandomTo.Length - ColumnsItem.Value.Length; x++)
                                            {
                                                sb.Append(getRandomChar(ColumnsItem.RandomTo[x]));
                                            }

                                            if (ColumnsItem.ColumnName == this.KeyField && listKey.Contains(sb.ToString()))
                                            {
                                                goto Label1;
                                            }
                                            value = sb.ToString();
                                            dr[ids.RealDataSet.Tables[deep].Columns[i].Caption] = value;
                                            listKey.Add(value);
                                        }
                                        else
                                        {
                                            int begin, end;
                                            Random m_rnd = new Random();
                                            int value;
                                            begin = this.getRandomInt(ColumnsItem.Value);
                                            end = this.getRandomInt(ColumnsItem.RandomTo);
                                        Label2:
                                            value = this.m_rnd.Next(begin, end);
                                            if (ColumnsItem.ColumnName == this.KeyField && listKey.Contains(value.ToString()))
                                            {
                                                goto Label2;
                                            }
                                            dr[ids.RealDataSet.Tables[deep].Columns[i].Caption] = value;
                                            listKey.Add(value.ToString());
                                        }
                                        break;
                                    }
                                    else if (ColumnsItem.valueMode == ValueMode.RefRandom)
                                    {
                                        if (ColumnsItem.RefVal != null)
                                        {
                                            InfoDataSet r_ds = (InfoDataSet)ColumnsItem.RefVal.GetDataSource();
                                            Random rnd = new Random();
                                        Label3:
                                            int x = rnd.Next(r_ds.RealDataSet.Tables[0].Rows.Count);
                                            DataRow r_dr = r_ds.RealDataSet.Tables[0].Rows[x];
                                            if (ColumnsItem.ColumnName == this.KeyField && listKey.Contains(r_dr[ColumnsItem.RefVal.ValueMember].ToString()))
                                            {
                                                goto Label3;
                                            }
                                            dr[ColumnsItem.ColumnName] = r_dr[ColumnsItem.RefVal.ValueMember];
                                            listKey.Add(r_dr[ColumnsItem.RefVal.ValueMember].ToString());
                                        }
                                        break;

                                    }
                                    else
                                    {
                                        dr[ColumnsItem.ColumnName] = user + sequence++;
                                        break;
                                    }
                                }
                            }
                        }
                        try
                        {
                            ids.RealDataSet.Tables[deep].Rows.Add(dr);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                            return;
                        }

                        if (this.ChildAutoTest.Count > 0)
                        {
                            int xcount = 0;
                            for (; xcount < this.ChildAutoTest.Count; xcount++)
                            {
                                deep++;
                                deep = deep + xcount;
                                AutoTest test = (AutoTest)this.ChildAutoTest[xcount];

                                //DateTime StartTime = DateTime.Now;
                                //int start = (StartTime.Hour * 60 * 60 + StartTime.Minute * 60 + StartTime.Second) * 1000 + StartTime.Millisecond;
                                //alLog.Add("UserID: " + user + "   PackageName: " + package + "   Satuts: DetailStart");

                                test.Execute(user);

                                //DateTime CompleteTime = DateTime.Now;
                                //int complete = (CompleteTime.Hour * 60 * 60 + CompleteTime.Minute * 60 + CompleteTime.Second) * 1000 + CompleteTime.Millisecond;
                                //int between = complete - start;
                                //alLog.Add("UserID: " + user + "   PackageName: " + package + "   Satuts: DetailComplete   DateTime: " + between.ToString());
                                //MaxRespendTime = Max(StartTime, CompleteTime, MaxRespendTime);
                                //MinRespendTime = Min(StartTime, CompleteTime, MinRespendTime);
                            }
                            if (xcount > 0)
                                deep -= xcount;
                            else
                                deep++;
                        }

                        //if(deep > 0)
                        //{
                        //    DateTime CompleteTime = DateTime.Now;
                        //    int complete = (CompleteTime.Hour * 60 * 60 + CompleteTime.Minute * 60 + CompleteTime.Second) * 1000 + CompleteTime.Millisecond;
                        //    int between = complete - start;
                        //    alLog.Add("UserID: " + user + "  PackageName: " + package + "   Satuts: MasterComplete   DateTime: " + between.ToString());
                        //}
                    }
                }
                deep = 0;
                //if (this.Log != null && this.Log != "")
                //{
                //    complete = DateTime.Now;
                //MaxRespendTime = Max(start, complete, MaxRespendTime);
                //MinRespendTime = Min(start, complete, MinRespendTime);

                //    alLog.Add("UserID: " + user + "  PackageName: " + package);
                //    int time = (complete.Hour * 60 * 60 + complete.Minute * 60 + complete.Second) * 1000 + complete.Millisecond;
                //    alLog.Add("       Status: COMPLETE TEST   DateTime: " + time.ToString());
                //    //alLog.Add("       Status: COMPLETE TEST   DateTime: " + complete.ToString());
                //}
            }
        }

        public void SubmitFlow(DataRow row)
        {
            if (this.IsFlowClient)
            {
                string curUser = CliUtils.fLoginUser;
                string flowDesc = GetFlowDesc(this.FlowFileName);
                string curTime = DateTimeString(DateTime.Now);
                string sql = "SELECT GROUPID,GROUPNAME FROM GROUPS WHERE GROUPID IN (SELECT GROUPID FROM USERGROUPS WHERE USERID='" + curUser + "')  AND ISROLE='Y' UNION SELECT ROLE_ID AS GROUPID,GROUPS.GROUPNAME  FROM SYS_ROLES_AGENT LEFT JOIN GROUPS ON SYS_ROLES_AGENT.ROLE_ID=GROUPS.GROUPID WHERE (SYS_ROLES_AGENT.FLOW_DESC='*' OR SYS_ROLES_AGENT.FLOW_DESC='" + flowDesc + "') AND AGENT='" + curUser + "' AND START_DATE+START_TIME<='" + curTime + "' AND END_DATE+END_TIME>='" + curTime + "'";
                DataTable tabRoles = CliUtils.ExecuteSql("GLModule", "cmdDDUse", sql, true, CliUtils.fCurrentProject).Tables[0];

                string keys = "", values = "";
                ArrayList lstKeys = new ArrayList();
                foreach (DataColumn col in row.Table.PrimaryKey)
                {
                    lstKeys.Add(col.ColumnName);
                }
                if (lstKeys.Count > 0)
                {
                    foreach (string key in lstKeys)
                    {
                        keys += key + ";";
                        if (this.IsNumeric(row[key].GetType()))
                        {
                            values += key + " = " + row[key].ToString() + ";";
                        }
                        else
                        {
                            values += key + " = ''" + row[key].ToString() + "'';";
                        }
                    }
                }
                if (keys != "" && values != "")
                {
                    keys = keys.Substring(0, keys.Length - 1);
                    values = values.Substring(0, values.Length - 1);
                }
                object[] objParams = CliUtils.CallFLMethod("Submit", new object[] { null, new object[] { this.FlowFileName, "", 0, 0, "", tabRoles.Rows[0][0].ToString(), ids.RemoteName, 0, "" }, new object[] { keys, values } });
            }
        }

        private string GetFlowDesc(string param)
        {
            string sql = "SELECT FLOW_DESC FROM SYS_TODOLIST WHERE LISTID='" + param + "'";
            DataTable table = CliUtils.ExecuteSql("GLModule", "cmdDDUse", sql, true, CliUtils.fCurrentProject).Tables[0];
            if (table.Rows.Count > 0)
                return table.Rows[0][0].ToString();
            return "";
        }

        private string DateTimeString(DateTime date)
        {
            string year = date.Year.ToString();
            string month = dformat(date.Month.ToString());
            string day = dformat(date.Day.ToString());
            string hour = dformat(date.Hour.ToString());
            string minute = dformat(date.Minute.ToString());
            string second = dformat(date.Second.ToString());

            return year + month + day + hour + minute + second;
        }

        private string dformat(string datePart)
        {
            return (datePart.Length == 2) ? datePart : "0" + datePart;
        }

        private bool IsNumeric(Type dataType)
        {
            string type = dataType.ToString().ToLower();
            if (type == "system.uint" || type == "system.uint16" || type == "system.uint32" || type == "system.uint64"
              || type == "system.int" || type == "system.int16" || type == "system.int32" || type == "system.int64"
              || type == "system.single" || type == "system.double" || type == "system.decimal")
            {
                return true;
            }
            return false;
        }


        public StringBuilder deleSpace(string str)
        {
            StringBuilder sb = new StringBuilder(str);

            for (int i = sb.Length - 1; i >= 0; i--)
                if (sb[i].ToString() == " ")
                    sb.Remove(i, 1);

            return sb;
        }

        Random m_rnd = new Random();
        public char getRandomChar(char a, char b)
        {
            int ret = m_rnd.Next(122);
            char begin, end;
            if (a > b)
            {
                end = a;
                begin = b;
            }
            else if (a < b)
            {
                begin = a;
                end = b;
            }
            else
            {
                return (char)(ret = (int)a);
            }
            while (ret < begin || (ret > 90 && ret < 97) || ret > end)
            {
                ret = m_rnd.Next(122);
            }
            return (char)ret;
        }

        public char getRandomChar(char end)
        {
            int ret = m_rnd.Next(122);
            while (ret < 65 || (ret > 90 && ret < 97) || ret > end)
            {
                ret = m_rnd.Next(122);
            }
            return (char)ret;
        }

        public int getRandomInt(string str)
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

        public Double Max(DateTime start, DateTime complete, Double MaxRespendTime)
        {
            //int temp, StartTemp, CompleteTemp;
            //StartTemp = (start.Hour * 60 * 60 + start.Minute * 60 + start.Second) * 1000 + start.Millisecond;
            //if (start.Hour - complete.Hour == 11)
            //    CompleteTemp = ((complete.Hour + 12) * 60 * 60 + complete.Minute * 60 + complete.Second) * 1000 + complete.Millisecond;
            //else if (start.Hour - complete.Hour == 23)
            //    CompleteTemp = ((complete.Hour + 24) * 60 * 60 + complete.Minute * 60 + complete.Second) * 1000 + complete.Millisecond;
            //else
            //    CompleteTemp = (complete.Hour * 60 * 60 + complete.Minute * 60 + complete.Second) * 1000 + complete.Millisecond;
            //temp = CompleteTemp - StartTemp;
            TimeSpan temp = complete - start;

            AverageRespendTime += temp.TotalMilliseconds;
            if (temp.TotalMilliseconds >= MaxRespendTime)
                return temp.TotalMilliseconds;
            else
                return MaxRespendTime;
        }

        public Double Min(DateTime start, DateTime complete, Double MinRespendTime)
        {
            //int temp, StartTemp, CompleteTemp;
            //StartTemp = (start.Hour * 60 * 60 + start.Minute * 60 + start.Second) * 1000 + start.Millisecond;
            //if (start.Hour - complete.Hour == 11)
            //    CompleteTemp = ((complete.Hour + 12) * 60 * 60 + complete.Minute * 60 + complete.Second) * 1000 + complete.Millisecond;
            //else if (start.Hour - complete.Hour == 23)
            //    CompleteTemp = ((complete.Hour + 24) * 60 * 60 + complete.Minute * 60 + complete.Second) * 1000 + complete.Millisecond;
            //else
            //    CompleteTemp = (complete.Hour * 60 * 60 + complete.Minute * 60 + complete.Second) * 1000 + complete.Millisecond;
            //temp = CompleteTemp - StartTemp;
            TimeSpan temp = complete - start;
            if (temp.TotalMilliseconds <= MinRespendTime || MinRespendTime == 0)
                return temp.TotalMilliseconds;
            else
                return MinRespendTime;
        }

        public AutoTest(IContainer container)
        {
            //language = CliSysMegLag.GetClientLanguage();
            language = CliUtils.fClientLang;
            container.Add(this);
            _Columns = new infoColumns(this, typeof(infoColumn));
            InitializeComponent();
        }
    }

    public class infoColumn : InfoOwnerCollectionItem
    {
        private string _Name = "";
        override public string Name
        {
            get
            {
                if (ColumnName != null)
                    return this.ColumnName;
                else
                    return _Name;
            }
            set
            {
                _Name = value;
            }
        }

        private string _ColumnName;
        [Editor(typeof(TestGetColumnName), typeof(UITypeEditor))]
        public string ColumnName
        {
            get
            {
                return _ColumnName;
            }
            set
            {
                _ColumnName = value;
            }
        }

        private ValueMode _valueMode;
        [Category("Value")]
        public ValueMode valueMode
        {
            get
            {
                return _valueMode;
            }
            set
            {
                _valueMode = value;
                switch (value)
                {
                    case (ValueMode.Fixed):
                        this.RandomTo = null;
                        this.RefVal = null;
                        break;
                    case (ValueMode.Random):
                        this.RefVal = null;
                        break;
                    case (ValueMode.RefRandom):
                        this.Value = null;
                        this.RandomTo = null;
                        break;
                    case (ValueMode.Sequence):
                        this.Value = null;
                        this.RandomTo = null;
                        this.RefVal = null;
                        break;
                }
            }
        }

        private string _Value;
        [Category("Value")]
        public string Value
        {
            get
            {
                return _Value;
            }
            set
            {
                _Value = value;


            }
        }

        private string _RandomTo;
        [Category("Value")]
        public string RandomTo
        {
            get
            {
                return _RandomTo;
            }
            set
            {
                _RandomTo = value;
            }
        }

        private InfoRefVal _RefVal;
        public InfoRefVal RefVal
        {
            get
            {
                return _RefVal;
            }
            set
            {
                _RefVal = value;
            }
        }
    }

    public class infoColumns : InfoOwnerCollection
    {
        public infoColumns(object aOwner, Type aItemType)
            : base(aOwner, typeof(infoColumn))
        {

        }

        new public infoColumn this[int index]
        {
            get
            {
                return (infoColumn)InnerList[index];
            }
            set
            {
                if (index > -1 && index < Count)
                    if (value is infoColumn)
                    {
                        //原来的Collection设置为0
                        ((infoColumn)InnerList[index]).Collection = null;
                        InnerList[index] = value;
                        //Collection设置为this
                        ((infoColumn)InnerList[index]).Collection = this;
                    }
            }
        }
    }

    public enum ValueMode
    {
        Fixed = 0,

        Random = 1,

        RefRandom = 2,

        Sequence = 3
    }

    public class TestGetColumnName : System.Drawing.Design.UITypeEditor
    {
        //private IWindowsFormsEditorService edSvc;
        public TestGetColumnName()
        {

        }

        // Indicates whether the UITypeEditor provides a form-based (modal) dialog,
        // drop down dialog, or no UI outside of the properties window.
        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
        public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        //    // Displays the UI for value selection.
        //    [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService EditorService = null;
            if (provider != null)
            {
                EditorService = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
            }

            if (EditorService != null)
            {
                ListBox ColumnList = new ListBox();
                ColumnList.SelectionMode = SelectionMode.One;
                ColumnList.Items.Add("( None )");
                infoColumn ifc = context.Instance as infoColumn;
                if (ifc != null)
                {
                    InfoBindingSource bindingSource = ((AutoTest)(ifc.Owner)).infoBindingSource;
                    DataView dataView = bindingSource.List as DataView;

                    if (dataView != null)
                    {
                        foreach (DataColumn column in dataView.Table.Columns)
                        {
                            ColumnList.Items.Add(column.ColumnName);
                        }
                    }
                    else
                    {
                        int iRelationPos = -1;
                        DataSet ds = ((InfoDataSet)((AutoTest)(ifc.Owner)).infoBindingSource.GetDataSource()).RealDataSet;
                        for (int i = 0; i < ds.Relations.Count; i++)
                        {
                            if (((AutoTest)(ifc.Owner)).infoBindingSource.DataMember == ds.Relations[i].RelationName)
                            {
                                iRelationPos = i;
                                break;
                            }
                        }
                        if (iRelationPos != -1)
                        {
                            foreach (DataColumn column in ds.Relations[iRelationPos].ChildTable.Columns)
                            {
                                ColumnList.Items.Add(column.ColumnName);
                            }
                        }
                    }

                }

                ColumnList.SelectedIndexChanged += delegate(object sender, EventArgs e)
                {
                    int index = ColumnList.SelectedIndex;
                    if (index != -1)
                    {
                        if (index == 0)
                        {
                            value = "";
                        }
                        else
                        {
                            value = ColumnList.Items[index].ToString();
                        }
                    }
                    EditorService.CloseDropDown();
                };

                EditorService.DropDownControl(ColumnList);
            }

            return value;
        }
    }

    public class GetControl : System.Drawing.Design.UITypeEditor
    {
        //private IWindowsFormsEditorService edSvc;
        public GetControl()
        {

        }

        // Indicates whether the UITypeEditor provides a form-based (modal) dialog,
        // drop down dialog, or no UI outside of the properties window.
        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
        public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        //    // Displays the UI for value selection.
        //    [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService EditorService = null;
            if (provider != null)
            {
                EditorService = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
            }

            if (EditorService != null)
            {
                ListBox ControlList = new ListBox();
                ControlList.SelectionMode = SelectionMode.One;
                ControlList.Items.Add("( None )");
                AutoTest ifrm = context.Instance as AutoTest;
                if (ifrm != null)
                {
                    for (int i = 0; i < ifrm.Container.Components.Count; i++)
                    {
                        IComponent c = ifrm.Container.Components[i];
                        if (c is Button)
                            ControlList.Items.Add(((Button)c).Text);
                        else if (c is ToolStripButton)
                            ControlList.Items.Add(((ToolStripButton)c).Text);
                    }
                }

                ControlList.SelectedIndexChanged += delegate(object sender, EventArgs e)
                {
                    int index = ControlList.SelectedIndex;
                    if (index != -1)
                    {
                        if (index == 0)
                        {
                            value = "(none)";
                        }
                        else
                        {
                            value = ControlList.Items[index].ToString();
                        }
                    }
                    EditorService.CloseDropDown();
                };

                EditorService.DropDownControl(ControlList);
            }

            return value;
        }
    }

    public class GetKeyField : System.Drawing.Design.UITypeEditor
    {
        //private IWindowsFormsEditorService edSvc;
        public GetKeyField()
        {

        }

        // Indicates whether the UITypeEditor provides a form-based (modal) dialog,
        // drop down dialog, or no UI outside of the properties window.
        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
        public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        //    // Displays the UI for value selection.
        //    [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService EditorService = null;
            if (provider != null)
            {
                EditorService = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
            }

            if (EditorService != null)
            {
                ListBox ColumnList = new ListBox();
                ColumnList.SelectionMode = SelectionMode.One;
                ColumnList.Items.Add("( None )");
                AutoTest ifc = context.Instance as AutoTest;
                if (ifc != null)
                {
                    InfoBindingSource bindingSource = ifc.infoBindingSource;
                    DataView dataView = bindingSource.List as DataView;

                    if (dataView != null)
                    {
                        foreach (DataColumn column in dataView.Table.Columns)
                        {
                            ColumnList.Items.Add(column.ColumnName);
                        }
                    }
                    else
                    {
                        int iRelationPos = -1;
                        DataSet ds = ((InfoDataSet)ifc.infoBindingSource.GetDataSource()).RealDataSet;
                        for (int i = 0; i < ds.Relations.Count; i++)
                        {
                            if (ifc.infoBindingSource.DataMember == ds.Relations[i].RelationName)
                            {
                                iRelationPos = i;
                                break;
                            }
                        }
                        if (iRelationPos != -1)
                        {
                            foreach (DataColumn column in ds.Relations[iRelationPos].ChildTable.Columns)
                            {
                                ColumnList.Items.Add(column.ColumnName);
                            }
                        }
                    }
                }

                ColumnList.SelectedIndexChanged += delegate(object sender, EventArgs e)
                {
                    int index = ColumnList.SelectedIndex;
                    if (index != -1)
                    {
                        if (index == 0)
                        {
                            value = "";
                        }
                        else
                        {
                            value = ColumnList.Items[index].ToString();
                        }
                    }
                    EditorService.CloseDropDown();
                };

                EditorService.DropDownControl(ColumnList);
            }
            return value;
        }
    }


}
