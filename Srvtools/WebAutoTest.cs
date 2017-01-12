using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Web.UI.Design;
using System.Web.UI.WebControls;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing.Design;
using System.Web.UI;
using System.Windows.Forms.Design;
using System.Windows.Forms;
using System.Data;
using System.Xml;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Resources;
using System.IO;
using System.Xml.Serialization;
using Srvtools;
using System.Drawing;
using Microsoft.Win32;

namespace Srvtools
{
    [Designer(typeof(DataSourceDesigner), typeof(IDesigner))]
    [ToolboxBitmap(typeof(WebAutoTest), "Resources.WebAutoTest.png")]
    public partial class WebAutoTest : WebControl, IGetValues, ISupportInitialize
    {
        public WebAutoTest()
        {
            _Columns = new webColumnCollection(this, typeof(webColumn));
        }

        public WebAutoTest(IContainer container)
        {
            container.Add(this);
            _Columns = new webColumnCollection(this, typeof(webColumn));
        }

        protected override void OnLoad(EventArgs e)
        {
            bool flag = true;
            foreach (System.Web.UI.Control ctrl in this.Page.Form.Controls)
                if (ctrl is WebAutoTest && (ctrl as WebAutoTest).ParentAutoTest != "" && (ctrl as WebAutoTest).ParentAutoTest != null)
                    foreach (System.Web.UI.Control c in this.Page.Form.Controls)
                        if (c is WebAutoTest && (c as WebAutoTest).ID == (ctrl as WebAutoTest).ParentAutoTest)
                        {
                            for (int i = 0; i < (c as WebAutoTest).ChildAutoTest.Count; i++)
                                if ((c as WebAutoTest).ChildAutoTest[i] == (ctrl as WebAutoTest))
                                {
                                    flag = false;
                                    break;
                                }
                            if (flag != false || (c as WebAutoTest).ChildAutoTest.Count == 0)
                                (c as WebAutoTest).ChildAutoTest.Add((ctrl as WebAutoTest));
                        }
            this.Page.LoadComplete += new EventHandler(Page_LoadComplete);
        }

        void Page_LoadComplete(object sender, EventArgs e)
        {
            if (this.Page.Session["active"] != null && this.Page.Session["active"].ToString() == "true")
            {
                this.Active = true;
                if(this.Log == "" || this.Log == null)
                    this.Log = this.Page.Session["Log"].ToString();
                if (this.Active == true && (this.ParentAutoTest == null || this.ParentAutoTest == ""))
                {
                    this.ExecuteTest(getInt(this.Page.Session["times"].ToString()), getInt(this.Page.Session["Interval"].ToString()),
                                            this.Page.Session["userid"].ToString(), this.Page.Session["packagename"].ToString());
                    try
                    {
                        this.Page.Session.Abandon();
                    }
                    finally
                    {
                        Page.Response.Write("<script>window.opener=null;window.close();</script>");
                    }
                }
            }
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

        [Category("Infolight"),
        Description("The ID of WebDataSource which the control is bound to")]
        [Editor(typeof(watDataSourceEditor), typeof(UITypeEditor))]
        public string DataSourceID
        {
            get
            {
                object obj = this.ViewState["DataSourceID"];
                if (obj != null)
                {
                    return (string)obj;
                }
                return string.Empty;
            }
            set
            {
                this.ViewState["DataSourceID"] = value;
            }
        }

        private bool Active = false;
        
        private webColumnCollection _Columns;
        [Category("Infolight"),
        Description("Specifies the columns which WebAutoTest is applied to")]
        [PersistenceMode(PersistenceMode.InnerProperty),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
         TypeConverter(typeof(CollectionConverter)),
         NotifyParentProperty(true)]
        public webColumnCollection Columns
        {
            get
            {
                return _Columns;
            }
        }

        private ArrayList ChildAutoTest = new ArrayList();
        private string _ParentAutoTest;
        [Category("Infolight"),
        Description("The WebAutoTest of the master table while the control is the WebAutoTest of the detail table")]
        [Editor(typeof(WebGetKeyField), typeof(UITypeEditor))]
        public string ParentAutoTest
        {
            get
            {
                return _ParentAutoTest;
            }
            set
            {
                _ParentAutoTest = value;
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
                _Rows = value;
            }
        }

        private string _Log;
        [Category("Infolight"),
        Description("The name of file to store the log of WebAutoTest")]
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
        Description("Prime key of WebAutoTest")]
        [Editor(typeof(WebGetKeyField), typeof(UITypeEditor))]
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
        
        public string[] GetValues(string sKind)
        {
            string[] retList = null;
            List<string> values = new List<string>();
            if (string.Compare(sKind, "keyfield", true) == 0)//IgnoreCase
            {
                    if (this.Page != null && this.DataSourceID != null && this.DataSourceID != "")
                    {
                        foreach (System.Web.UI.Control ctrl in this.Page.Controls)
                            if (ctrl is WebDataSource && ((WebDataSource)ctrl).ID == this.DataSourceID)
                            {
                                WebDataSource ds = (WebDataSource)ctrl;
                                if (ds.DesignDataSet == null)
                                {
                                    WebDataSet wds = WebDataSet.CreateWebDataSet(ds.WebDataSetID);
                                    if (wds != null)
                                    {
                                        ds.DesignDataSet = wds.RealDataSet;
                                    }
                                }
                                if (ds.DesignDataSet != null && ds.DesignDataSet.Tables.Contains(ds.DataMember))
                                {
                                    foreach (DataColumn column in ds.DesignDataSet.Tables[ds.DataMember].Columns)
                                        values.Add(column.ColumnName);
                                }
                                break;
                            }
                        if (values.Count > 0)
                        {
                            int i = values.Count;
                            retList = new string[i];
                            for (int j = 0; j < i; j++)
                                retList[j] = values[j];
                        }
                    }
                }
                else if (string.Compare(sKind, "parentautotest", true) == 0)//IgnoreCase
                {
                    if (this.Page != null)
                    {
                        foreach (System.Web.UI.Control ctrl in this.Page.Controls)
                        {
                            if (ctrl is WebAutoTest)
                            {
                                WebAutoTest wat = (WebAutoTest)ctrl;
                                if (wat.ID != null && wat.ID != "")
                                    values.Add(wat.ID);
                            }
                        }
                        if (values.Count > 0)
                        {
                            int i = values.Count;
                            retList = new string[i];
                            for (int j = 0; j < i; j++)
                                retList[j] = values[j];
                        }
                    }
                }
            return retList;
        }

        WebDataSource ws = new WebDataSource();
        private int MaxRespendTime, MinRespendTime;
        private float AverageRespendTime;
        private ArrayList alLog = new ArrayList();
        public void ExecuteTest(int Times, int Interval, string user, string package)
        {
            MaxRespendTime = 0;
            MinRespendTime = 0;
            AverageRespendTime = 0;

            DateTime start = DateTime.Now;
            alLog.Add("WebAutoTest Log File.");
            alLog.Add("UserID: " + user + "   PackageName: " + package + "   Start Time: " + start + " " + start.Millisecond);

            for (int i = 0; i < Times; i++)
            {
                DateTime BeforeApply = DateTime.Now;
                string before = BeforeApply.ToString() + " " + BeforeApply.Millisecond.ToString();

                this.Execute(user, 0, "");

                DateTime AfterApply = DateTime.Now;
                string after = AfterApply.ToString() + " " + AfterApply.Millisecond.ToString();
                int between = ((AfterApply.Hour * 60 * 60 + AfterApply.Minute * 60 + AfterApply.Second) * 1000 + AfterApply.Millisecond)
                                - ((BeforeApply.Hour * 60 * 60 + BeforeApply.Minute * 60 + BeforeApply.Second) * 1000 + BeforeApply.Millisecond);

                if (between > RecordTime)
                    alLog.Add("Times: " + i + "   Between: " + between);
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

                if (File.Exists(this.Log))
                {
                    FileStream fs = File.Open(this.Log, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                    StreamReader sreader = new StreamReader(fs);
                    alLog.Add(sreader.ReadToEnd());
                    sreader.Close();

                    fs = File.Open(this.Log, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                    StreamWriter swriter = new StreamWriter(fs);
                    for (int i = 0; i < alLog.Count; i++)
                        swriter.WriteLine(alLog[i]);
                    swriter.Close();
                }
            }
        }
        
        private int sequence = 0;
        private int deep;
        public void Execute(string user, int d, string Key)
        {
            if ((this.ParentAutoTest != "" && this.ParentAutoTest != null) && (this.KeyField == "" || this.KeyField == null)) return;
            bool temp = new bool(); ;
            deep = d;
            string KeyValue = "";
            if (this.DataSourceID != "" && this.DataSourceID != null)
            {
                List<string> listKey = new List<string>();
                for (int times = 0; times < this.Rows; times++)
                {
                    Hashtable ht = new Hashtable();
                    DataSet ds = new DataSet();
                    foreach (System.Web.UI.Control c in this.Page.Form.Controls)
                    {
                        if (c is WebDataSource && (c as WebDataSource).ID == this.DataSourceID)
                        {
                            ws = c as WebDataSource;
                            ds = (c as WebDataSource).InnerDataSet;
                            break;
                        }
                    }
                    if (ws != null && ds != null)
                    {
                        string fieldName;
                        for (int i = 0; i < ds.Tables[deep].Columns.Count; i++)
                        {
                            fieldName = ds.Tables[deep].Columns[i].ToString();
                            if (fieldName == ds.Tables[0].PrimaryKey[0].ToString() && deep == 0)
                            {
                                KeyValue = user + sequence++;
                                ht.Add(fieldName, KeyValue);
                            }
                            else if (fieldName == ds.Tables[0].PrimaryKey[0].ToString())
                            {
                                ht.Add(fieldName, Key);
                            }
                            else
                            {
                                foreach (webColumn wc in this.Columns)
                                {
                                    if (wc.ColumnName == fieldName && wc.ColumnName == this.KeyField && wc.Value != null)
                                    {
                                        bool flag = true;
                                        for (int j = 0; j < wc.Value.Length; j++)
                                            if (wc.Value[j] < 48 || wc.Value[j] > 57)
                                                flag = false;
                                        for (int j = 0; j < wc.RandomTo.Length; j++)
                                            if (wc.RandomTo[j] < 48 || wc.RandomTo[j] > 57)
                                                flag = false;
                                        if (flag == false)
                                        {
                                            int length;
                                            if (wc.Value.Length < wc.RandomTo.Length)
                                                length = wc.Value.Length;
                                            else
                                                length = wc.RandomTo.Length;
                                            StringBuilder sb = new StringBuilder(length);

                                        Label1:
                                            if (sb.Length > 0)
                                                sb.Remove(0, sb.Length);
                                            for (int x = 0; x < length; x++)
                                                sb.Append(getRandomChar(wc.Value[x], wc.RandomTo[x]));
                                            for (int x = 0; x < wc.RandomTo.Length - wc.Value.Length; x++)
                                                sb.Append(getRandomChar(wc.RandomTo[x]));
                                            bool isHave = false;
                                            foreach (string strKey in listKey)
                                                if (sb.ToString() == strKey) isHave = true;
                                            if (isHave == false)
                                            {
                                                listKey.Add(sb.ToString());
                                                ht.Add(wc.ColumnName, sb.ToString());
                                            }
                                            else
                                                goto Label1;
                                        }
                                        else
                                        {
                                            int begin, end;
                                            Random m_rnd = new Random();
                                            int value;
                                            begin = this.getRandomInt(wc.Value);
                                            end = this.getRandomInt(wc.RandomTo);
                                        Label2:
                                            value = this.m_rnd.Next(begin, end);
                                            bool isHave = false;
                                            foreach (string strKey in listKey)
                                                if (value.ToString() == strKey) isHave = true;
                                            if (isHave == false)
                                            {
                                                listKey.Add(value.ToString());
                                                ht.Add(wc.ColumnName, value);
                                            }
                                            else
                                                goto Label2;
                                        }
                                    }
                                    else if (wc.ColumnName == fieldName)
                                    {
                                        if (wc.valueMode == ValueMode.Fixed)
                                            ht.Add(wc.ColumnName, wc.Value);
                                        else if (wc.valueMode == ValueMode.Random)
                                        {
                                            bool flag = true;
                                            for (int j = 0; j < wc.Value.Length; j++)
                                                if (wc.Value[j] < 48 || wc.Value[j] > 57)
                                                    flag = false;
                                            for (int j = 0; j < wc.RandomTo.Length; j++)
                                                if (wc.RandomTo[j] < 48 || wc.RandomTo[j] > 57)
                                                    flag = false;
                                            if (flag == false)
                                            {
                                                int length;
                                                if (wc.Value.Length < wc.RandomTo.Length)
                                                    length = wc.Value.Length;
                                                else
                                                    length = wc.RandomTo.Length;
                                                StringBuilder sb = new StringBuilder(length);

                                                if (sb.Length > 0)
                                                    sb.Remove(0, sb.Length);
                                                for (int x = 0; x < length; x++)
                                                    sb.Append(getRandomChar(wc.Value[x], wc.RandomTo[x]));
                                                for (int x = 0; x < wc.RandomTo.Length - wc.Value.Length; x++)
                                                    sb.Append(getRandomChar(wc.RandomTo[x]));
                                                ht.Add(wc.ColumnName, sb.ToString());
                                            }
                                            else
                                            {
                                                int begin, end;
                                                Random m_rnd = new Random();
                                                int value;
                                                begin = this.getRandomInt(wc.Value);
                                                end = this.getRandomInt(wc.RandomTo);
                                                value = this.m_rnd.Next(begin, end);
                                                ht.Add(wc.ColumnName, value);
                                            }
                                        }
                                        else if (wc.valueMode == ValueMode.RefRandom)
                                        {
                                            if (wc.RefVal != null)
                                            {
                                                DataSet R_ds = new DataSet();
                                                WebRefVal wrv = new WebRefVal();
                                                foreach (System.Web.UI.Control ctrl in this.Page.Form.Controls)
                                                    if (ctrl is WebRefVal && (ctrl as WebRefVal).ID == wc.RefVal)
                                                        foreach (System.Web.UI.Control c in this.Page.Form.Controls)
                                                            if (c is WebDataSource && (c as WebDataSource).ID == (ctrl as WebRefVal).DataSourceID)
                                                                if ((c as WebDataSource).WebDataSetID == null || (c as WebDataSource).WebDataSetID == "")
                                                                {
                                                                    wrv = (WebRefVal)ctrl;
                                                                    Random rnd = new Random();
                                                                    int x = rnd.Next((c as WebDataSource).CommandTable.Rows.Count);
                                                                    DataRow r_dr = (c as WebDataSource).CommandTable.Rows[x];
                                                                    ht.Add(wc.ColumnName, r_dr[wrv.DataValueField]);
                                                                }
                                                                else
                                                                {
                                                                    R_ds = (c as WebDataSource).InnerDataSet;
                                                                    wrv = (WebRefVal)ctrl;
                                                                    Random rnd = new Random();
                                                                    int x = rnd.Next(R_ds.Tables[0].Rows.Count);
                                                                    DataRow r_dr = R_ds.Tables[0].Rows[x];
                                                                    ht.Add(wc.ColumnName, r_dr[wrv.DataValueField]);
                                                                }
                                            }
                                        }
                                        else
                                            ht.Add(wc.ColumnName, user + sequence++);
                                    }
                                }
                            }
                        }

                        DataRow dr = ds.Tables[deep].NewRow();
                        for (int j = 0; j < dr.Table.Columns.Count; j++)
                        {
                            if (ht[dr.Table.Columns[j].Caption] != null)
                            {
                                dr[dr.Table.Columns[j].Caption] = ht[dr.Table.Columns[j].Caption];
                            }
                        }
                        ds.Tables[deep].Rows.Add(dr);
                    }

                    if (this.ChildAutoTest.Count > 0)
                    {
                        int childCount = 0;
                        for (; childCount < this.ChildAutoTest.Count; childCount++)
                        {
                            deep = 1 + childCount;
                            WebAutoTest wat = (WebAutoTest)this.ChildAutoTest[childCount];
                            wat.Execute(user, deep, KeyValue);
                        }
                        deep = 0;
                    }

                    if (this.ParentAutoTest == null || this.ParentAutoTest == "")
                    {
                        temp = ws.AutoApply;
                        ws.AutoApply = true;
                        //ws.Insert(ht);
                        ws.ApplyUpdates();
                        SubmitFlow(ht);
                        ws.AutoApply = temp;
                    }


                }
            }
        }

        private System.Web.UI.Control GetAllCtrls(string strid, System.Web.UI.Control ct)
        {
            if (ct.ID == strid)
            {
                return ct;
            }
            else
            {
                if (ct.HasControls())
                {
                    foreach (System.Web.UI.Control ctchild in ct.Controls)
                    {
                        System.Web.UI.Control ctrtn = GetAllCtrls(strid, ctchild);
                        if (ctrtn != null)
                        {
                            return ctrtn;
                        }
                    }
                    return null;
                }
                else
                {
                    return null;
                }
            }
        }
        public object GetObjByID(string ObjID)
        {
            if (this.Site != null)
            {
                return GetAllCtrls(ObjID, this.Page);
            }
            else
            {
                if (this.Page != null)
                {
                    if (this.Page.Form != null)
                        return GetAllCtrls(ObjID, this.Page.Form);
                    else
                        return GetAllCtrls(ObjID, this.Page);
                }
                else
                {
                    return GetAllCtrls(ObjID, this.NamingContainer);
                }
            }
        }
        public WebDataSource GetDataSource()
        {
            object obj = this.GetObjByID(this.DataSourceID);
            if (obj != null && obj is WebDataSource)
            {
                return (WebDataSource)obj;
            }
            return null;
        }
        public DataTable GetSrcTable()
        {
            WebDataSource wds = this.GetDataSource();
            if (string.IsNullOrEmpty(wds.SelectAlias) && string.IsNullOrEmpty(wds.SelectCommand))
            {
                return wds.InnerDataSet.Tables[wds.DataMember];
            }
            else
            {
                return wds.CommandTable;
            }
        }

        public void SubmitFlow(Hashtable ht)
        {
            if (this.IsFlowClient)
            {
                string curUser = CliUtils.fLoginUser;
                string flowFileName = this.Page.Session["flowfilename"].ToString();
                if (!string.IsNullOrEmpty(flowFileName))
                {
                    flowFileName = string.Format("{0}\\WorkFlow\\{1}", EEPRegistry.Server, flowFileName);
                }
                string flowDesc = GetFlowDesc(flowFileName);
                string curTime = DateTimeString(DateTime.Now);
                string sql = "SELECT GROUPID,GROUPNAME FROM GROUPS WHERE GROUPID IN (SELECT GROUPID FROM USERGROUPS WHERE USERID='" + curUser + "')  AND ISROLE='Y' UNION SELECT ROLE_ID AS GROUPID,GROUPS.GROUPNAME  FROM SYS_ROLES_AGENT LEFT JOIN GROUPS ON SYS_ROLES_AGENT.ROLE_ID=GROUPS.GROUPID WHERE (SYS_ROLES_AGENT.FLOW_DESC='*' OR SYS_ROLES_AGENT.FLOW_DESC='" + flowDesc + "') AND AGENT='" + curUser + "' AND START_DATE+START_TIME<='" + curTime + "' AND END_DATE+END_TIME>='" + curTime + "'";
                DataTable tabRoles = CliUtils.ExecuteSql("GLModule", "cmdDDUse", sql, true, CliUtils.fCurrentProject).Tables[0];

                string keys = "", values = "";
                ArrayList lstKeys = new ArrayList();

                foreach (DataColumn col in this.GetSrcTable().PrimaryKey)
                {
                    lstKeys.Add(col.ColumnName);
                }
                if (lstKeys.Count > 0)
                {
                    foreach (string key in lstKeys)
                    {
                        keys += key + ";";
                        if (this.IsNumeric(ht[key].GetType()))
                        {
                            values += key + " = " + ht[key].ToString() + ";";
                        }
                        else
                        {
                            values += key + " = ''" + ht[key].ToString() + "'';";
                        }
                    }
                }
                if (keys != "" && values != "")
                {
                    keys = keys.Substring(0, keys.Length - 1);
                    values = values.Substring(0, values.Length - 1);
                }
                object[] objParams = CliUtils.CallFLMethod("Submit", new object[] { null, new object[] { flowFileName, "", 0, 0, "", tabRoles.Rows[0][0].ToString(), this.GetDataSource().RemoteName, 0, "" }, new object[] { keys, values } });
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
                return (char)(ret = (int)a);
            while (ret < begin || (ret > 90 && ret < 97) || ret > end)
                ret = m_rnd.Next(122);
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


        public int Max(DateTime start, DateTime complete, int MaxRespendTime)
        {
            int temp, StartTemp, CompleteTemp;
            StartTemp = (start.Hour * 60 * 60 + start.Minute * 60 + start.Second) * 1000 + start.Millisecond;
            if (start.Hour - complete.Hour == 11)
                CompleteTemp = ((complete.Hour + 12) * 60 * 60 + complete.Minute * 60 + complete.Second) * 1000 + complete.Millisecond;
            else if (start.Hour - complete.Hour == 23)
                CompleteTemp = ((complete.Hour + 24) * 60 * 60 + complete.Minute * 60 + complete.Second) * 1000 + complete.Millisecond;
            else
                CompleteTemp = (complete.Hour * 60 * 60 + complete.Minute * 60 + complete.Second) * 1000 + complete.Millisecond;
            temp = CompleteTemp - StartTemp;
            AverageRespendTime += temp;
            if (temp >= MaxRespendTime)
                return temp;
            else
                return MaxRespendTime;
        }

        public int Min(DateTime start, DateTime complete, int MinRespendTime)
        {
            int temp, StartTemp, CompleteTemp;
            StartTemp = (start.Hour * 60 * 60 + start.Minute * 60 + start.Second) * 1000 + start.Millisecond;
            if (start.Hour - complete.Hour == 11)
                CompleteTemp = ((complete.Hour + 12) * 60 * 60 + complete.Minute * 60 + complete.Second) * 1000 + complete.Millisecond;
            else if (start.Hour - complete.Hour == 23)
                CompleteTemp = ((complete.Hour + 24) * 60 * 60 + complete.Minute * 60 + complete.Second) * 1000 + complete.Millisecond;
            else
                CompleteTemp = (complete.Hour * 60 * 60 + complete.Minute * 60 + complete.Second) * 1000 + complete.Millisecond;
            temp = CompleteTemp - StartTemp;
            if (temp <= MinRespendTime || MinRespendTime == 0)
                return temp;
            else
                return MinRespendTime;
        }

        #region ISupportInitialize Members

        public void BeginInit()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void EndInit()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }


    public class webColumn : InfoOwnerCollectionItem, IGetValues
    {
        public webColumn()
        {

        }

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
        [Editor(typeof(WebGetColumns), typeof(UITypeEditor))]
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

        private string _RefVal;
        [Editor(typeof(GetWebRefVal), typeof(UITypeEditor))]
        public string RefVal
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

        public string[] GetValues(string sKind)
        {
            string[] retList = null;
            List<string> values = new List<string>();
            if (string.Compare(sKind, "columnname", true) == 0)//IgnoreCase
            {
                if (this.Owner is WebAutoTest)
                {
                    WebAutoTest wcq = (WebAutoTest)this.Owner;
                    if (wcq.Page != null && wcq.DataSourceID != null && wcq.DataSourceID != "")
                    {
                        foreach (System.Web.UI.Control ctrl in wcq.Page.Controls)
                        {
                            if (ctrl is WebDataSource && ((WebDataSource)ctrl).ID == wcq.DataSourceID)
                            {
                                WebDataSource ds = (WebDataSource)ctrl;
                                if (ds.DesignDataSet == null)
                                {
                                    WebDataSet wds = WebDataSet.CreateWebDataSet(ds.WebDataSetID);
                                    if (wds != null)
                                    {
                                        ds.DesignDataSet = wds.RealDataSet;
                                    }
                                }
                                if (ds.DesignDataSet != null && ds.DesignDataSet.Tables.Contains(ds.DataMember))
                                {
                                    foreach (DataColumn column in ds.DesignDataSet.Tables[ds.DataMember].Columns)
                                    {
                                        values.Add(column.ColumnName);
                                    }
                                }
                                break;
                            }
                        }
                        if (values.Count > 0)
                        {
                            int i = values.Count;
                            retList = new string[i];
                            for (int j = 0; j < i; j++)
                            {
                                retList[j] = values[j];
                            }
                        }
                    }
                }
            }
            else if (string.Compare(sKind, "refval", true) == 0)//IgnoreCase
            {
                if (this.Owner is WebAutoTest)
                {
                    WebAutoTest wat = (WebAutoTest)this.Owner;
                    if (wat.Page != null)
                    {
                        foreach (System.Web.UI.Control ctrl in wat.Page.Controls)
                        {
                            if (ctrl is WebRefVal)
                            {
                                values.Add(((WebRefVal)ctrl).ID);
                            }
                        }
                        if (values.Count > 0)
                        {
                            int i = values.Count;
                            retList = new string[i];
                            for (int j = 0; j < i; j++)
                            {
                                retList[j] = values[j];
                            }
                        }
                    }
                }
            }
            return retList;
        }
    }

    public class webColumnCollection : InfoOwnerCollection
    {
        public webColumnCollection()
        {

        }
        
        public webColumnCollection(object aOwner, Type aItemType)
            : base(aOwner, typeof(webColumn))
        {

        }

        public new webColumn this[int index]
        {
            get
            {
                return (webColumn)InnerList[index];
            }
            set
            {
                if (index > -1 && index < Count)
                    if (value is webColumn)
                    {
                        //原来的Collection设置为0
                        ((webColumn)InnerList[index]).Collection = null;
                        InnerList[index] = value;
                        //Collection设置为this
                        ((webColumn)InnerList[index]).Collection = this;
                    }
            }
        }
    }

    public class watDataSourceEditor : UITypeEditor
    {
        public watDataSourceEditor()
            : base()
        {
        }

        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand)]
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand)]
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            List<string> objName = new List<string>();
            if (context.Instance is WebAutoTest)
            {
                ControlCollection ctrlList = ((WebAutoTest)context.Instance).Page.Controls;
                foreach (System.Web.UI.Control ctrl in ctrlList)
                {
                    if (ctrl is WebDataSource)
                    {
                        objName.Add(ctrl.ID);
                    }
                }
            }

            IWindowsFormsEditorService service = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (service != null)
            {
                StringListSelector selector = new StringListSelector(service, objName.ToArray());
                string strValue = (string)value;
                if (selector.Execute(ref strValue)) value = strValue;
            }
            return value;
        }
    }

    public class WebGetColumns : System.Drawing.Design.UITypeEditor
    {
        //private IWindowsFormsEditorService edSvc;
        public WebGetColumns()
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
            // Uses the IWindowsFormsEditorService to display a drop-down UI in the Properties window.
            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            IGetValues aItem = (IGetValues)context.Instance;
            if (edSvc != null)
            {
                StringListSelector mySelector = new StringListSelector(edSvc, aItem.GetValues(context.PropertyDescriptor.Name));
                string strValue = (string)value;
                if (mySelector.Execute(ref strValue)) value = strValue;
            }
            return value;
        }
    }

    public class GetWebRefVal : System.Drawing.Design.UITypeEditor
    {
        //private IWindowsFormsEditorService edSvc;
        public GetWebRefVal()
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
            // Uses the IWindowsFormsEditorService to display a drop-down UI in the Properties window.
            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            IGetValues aItem = (IGetValues)context.Instance;
            if (edSvc != null)
            {
                StringListSelector mySelector = new StringListSelector(edSvc, aItem.GetValues(context.PropertyDescriptor.Name));
                string strValue = (string)value;
                if (mySelector.Execute(ref strValue)) value = strValue;
            }
            return value;
        }
    }

    public class WebGetKeyField : System.Drawing.Design.UITypeEditor
    {
        //private IWindowsFormsEditorService edSvc;
        public WebGetKeyField()
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
            // Uses the IWindowsFormsEditorService to display a drop-down UI in the Properties window.
            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            IGetValues aItem = (IGetValues)context.Instance;
            if (edSvc != null)
            {
                StringListSelector mySelector = new StringListSelector(edSvc, aItem.GetValues(context.PropertyDescriptor.Name));
                string strValue = (string)value;
                if (mySelector.Execute(ref strValue)) value = strValue;
            }
            return value;
        }
    }

    public class GetWebAutoTest : System.Drawing.Design.UITypeEditor
    {
        //private IWindowsFormsEditorService edSvc;
        public GetWebAutoTest()
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
            // Uses the IWindowsFormsEditorService to display a drop-down UI in the Properties window.
            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            IGetValues aItem = (IGetValues)context.Instance;
            if (edSvc != null)
            {
                StringListSelector mySelector = new StringListSelector(edSvc, aItem.GetValues(context.PropertyDescriptor.Name));
                string strValue = (string)value;
                if (mySelector.Execute(ref strValue)) value = strValue;
            }
            return value;
        }
    }


}
