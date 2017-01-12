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
using System.Data.SqlClient;
using System.Data.OracleClient;
using System.Data.OleDb;
using System.Data.Odbc;
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
    public partial class WebAutoQuery : WebControl, ISupportInitialize
    {
        public WebAutoQuery()
        {
            _Columns = new AutoQueryColumns(this, typeof(AutoQueryColumn));
        }

        public WebAutoQuery(IContainer container)
        {
            container.Add(this);
            _Columns = new AutoQueryColumns(this, typeof(AutoQueryColumn));
        }

        protected override void OnLoad(EventArgs e)
        {
            this.Page.LoadComplete += new EventHandler(Page_LoadComplete);
        }

        void Page_LoadComplete(object sender, EventArgs e)
        {
            if (this.Page.Session["active"] != null && this.Page.Session["active"].ToString() == "true")
            {
                this.Active = true;
                if (this.Active == true)
                {
                    this.ExecuteTest(Convert.ToInt32(this.Page.Session["times"].ToString()), Convert.ToInt32(this.Page.Session["Interval"].ToString()),
                                            this.Page.Session["userid"].ToString(), this.Page.Session["packagename"].ToString(), this.Page.Session["Log"].ToString());
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

        [Category("Infolight"),
        Description("The ID of WebDataSource which the control is bound to")]
        [Editor(typeof(waqDataSourceEditor), typeof(UITypeEditor))]
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

        private AutoQueryColumns _Columns;
        [Category("Infolight"),
        Description("Specifies the columns which WebAutoTest is applied to")]
        [PersistenceMode(PersistenceMode.InnerProperty),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
         TypeConverter(typeof(CollectionConverter)),
         NotifyParentProperty(true)]
        public AutoQueryColumns Columns
        {
            get
            {
                return _Columns;
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

        private ConnectionType _connType;
        [Category("Infolight")]
        public ConnectionType ConnType
        {
            get
            {

                return _connType;
            }
            set
            {
                _connType = value;
            }
        }

        public void ExecuteTest(int times, int interval, String user, String package, String Log)
        {
            ArrayList alLog = new ArrayList();
            DateTime start = DateTime.Now;
            alLog.Add("AutoQuery log file.");
            alLog.Add("UserID: " + user + "   PackageName: " + package + "   Start Time: " + start + " " + start.Millisecond);

            Execute(user, times, alLog, interval);

            if (Log != null && Log != "")
            {
                string s = null;
                if (Log.Length > 4)
                {
                    for (int h = 0; h < 4; h++)
                        s = s + Log[Log.Length - 4 + h];
                    if (string.Compare(s, ".txt", true) != 0)//IgnoreCase
                        Log = Log + ".txt";
                }
                else
                    Log = Log + ".txt";

                DateTime complete = DateTime.Now;
                alLog.Add("UserID: " + user + "   PackageName: " + package + "   Complete Time: " + complete + " " + DateTime.Now.Millisecond);
                alLog.Add("Total " + times + " Records, Use " + (complete - start) + " Seconds.");
                alLog.Add("");

                try
                {
                    FileStream fs = File.Open(Log, FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite);
                    StreamReader sreader = new StreamReader(fs);
                    alLog.Add(sreader.ReadToEnd());
                    sreader.Close();

                    fs = File.Open(Log, FileMode.Open, FileAccess.Write, FileShare.ReadWrite);
                    StreamWriter swriter = new StreamWriter(fs);
                    for (int i = 0; i < alLog.Count; i++)
                    {
                        swriter.WriteLine(alLog[i]);
                    }
                    swriter.Close();
                    fs.Close();
                }
                catch
                {
                }
            }
        }

        public void Execute(String userID, int times, ArrayList aLog, int interval)
        {
            WebDataSource ws = new WebDataSource();
            foreach (System.Web.UI.Control c in this.Page.Form.Controls)
            {
                if (c is WebDataSource && (c as WebDataSource).ID == this.DataSourceID)
                {
                    ws = c as WebDataSource;
                    break;
                }
            }

            if (ws == null || ws.DataSource == null) return;
            for (int i = 0; i < times; i++)
            {
                String whereStr = "";
                ArrayList whereArr = new ArrayList();

                foreach (AutoQueryColumn aqc in this.Columns)
                {
                    switch (this.ConnType)
                    {
                        case ConnectionType.SqlClient:
                            if (aqc.Operator == "%")
                            {
                                whereStr += aqc.ColumnName + "=@" + aqc.ColumnName + " " + aqc.Condition + " ";
                                SqlParameter sqlParam = new SqlParameter();
                                sqlParam.ParameterName = "@" + aqc.ColumnName;
                                sqlParam.SqlDbType = GetSqlDBType(aqc.DatabaseType);
                                sqlParam.Value = aqc.Operator + GetValue(aqc, userID);
                                whereArr.Add(sqlParam);
                            }
                            else if (aqc.Operator == "%%")
                            {
                                whereStr += aqc.ColumnName + "=@" + aqc.ColumnName + " " + aqc.Condition + " ";
                                SqlParameter sqlParam = new SqlParameter();
                                sqlParam.ParameterName = "@" + aqc.ColumnName;
                                sqlParam.SqlDbType = GetSqlDBType(aqc.DatabaseType);
                                sqlParam.Value = "%" + GetValue(aqc, userID) + "%";
                                whereArr.Add(sqlParam);
                            }
                            else
                            {
                                whereStr += aqc.ColumnName + aqc.Operator + "@" + aqc.ColumnName + " " + aqc.Condition + " ";
                                SqlParameter sqlParam = new SqlParameter();
                                sqlParam.ParameterName = "@" + aqc.ColumnName;
                                sqlParam.SqlDbType = GetSqlDBType(aqc.DatabaseType);
                                sqlParam.Value = GetValue(aqc, userID);
                                whereArr.Add(sqlParam);
                            }
                            break;
                        case ConnectionType.OracleClient:
                            if (aqc.Operator == "%")
                            {
                                whereStr += aqc.ColumnName + "=@" + aqc.ColumnName + " " + aqc.Condition + " ";
                                OracleParameter oracleParam = new OracleParameter();
                                oracleParam.ParameterName = "@" + aqc.ColumnName;
                                oracleParam.OracleType = GetOracleDBType(aqc.DatabaseType);
                                oracleParam.Value = aqc.Operator + GetValue(aqc, userID);
                                whereArr.Add(oracleParam);
                            }
                            else if (aqc.Operator == "%%")
                            {
                                whereStr += aqc.ColumnName + "=@" + aqc.ColumnName + " " + aqc.Condition + " ";
                                OracleParameter oracleParam = new OracleParameter();
                                oracleParam.ParameterName = "@" + aqc.ColumnName;
                                oracleParam.OracleType = GetOracleDBType(aqc.DatabaseType);
                                oracleParam.Value = "%" + GetValue(aqc, userID) + "%";
                                whereArr.Add(oracleParam);
                            }
                            else
                            {
                                whereStr += aqc.ColumnName + aqc.Operator + "@" + aqc.ColumnName + " " + aqc.Condition + " ";
                                OracleParameter oracleParam = new OracleParameter();
                                oracleParam.ParameterName = "@" + aqc.ColumnName;
                                oracleParam.OracleType = GetOracleDBType(aqc.DatabaseType);
                                oracleParam.Value = GetValue(aqc, userID);
                                whereArr.Add(oracleParam);
                            }
                            break;
                    }
                }

                if (whereStr.EndsWith(" AND "))
                    whereStr = whereStr.Remove(whereStr.Length - 5);
                else if (whereStr.EndsWith(" OR "))
                    whereStr = whereStr.Remove(whereStr.Length - 4);
                ws.SetWhere(whereStr, whereArr);

                System.Threading.Thread.Sleep(interval);
            }
        }

        private int sequence;
        public object GetValue(AutoQueryColumn column, String userID)
        {
            object obj = new object();
            switch (column.valueMode)
            {
                case ValueMode.Fixed:
                    obj = column.Value;
                    break;
                case ValueMode.Random:
                    bool flag = true;
                    for (int j = 0; j < column.Value.Length; j++)
                        if (column.Value[j] < 48 || column.Value[j] > 57)
                            flag = false;
                    for (int j = 0; j < column.RandomTo.Length; j++)
                        if (column.RandomTo[j] < 48 || column.RandomTo[j] > 57)
                            flag = false;
                    if (flag == false)
                    {
                        int length;
                        if (column.Value.Length < column.RandomTo.Length)
                            length = column.Value.Length;
                        else
                            length = column.RandomTo.Length;
                        StringBuilder sb = new StringBuilder(length);

                        if (sb.Length > 0)
                            sb.Remove(0, sb.Length);
                        for (int x = 0; x < length; x++)
                            sb.Append(getRandomChar(column.Value[x], column.RandomTo[x]));

                        Random m_rnd = new Random();
                        length = m_rnd.Next(column.RandomTo.Length - column.Value.Length);
                        for (int x = 0; x < length; x++)
                            sb.Append(getRandomChar(column.RandomTo[x]));

                        obj = sb.ToString();
                    }
                    else
                    {
                        Random m_rnd = new Random();
                        obj = m_rnd.Next(Convert.ToInt32(column.Value), Convert.ToInt32(column.RandomTo));
                    }
                    break;
                case ValueMode.Sequence:
                    obj = userID + sequence++;
                    break;
                case ValueMode.RefRandom:
                    if (column.RefVal != null)
                    {
                        InfoDataSet r_ds = (InfoDataSet)column.RefVal.GetDataSource();
                        Random rnd = new Random();
                        int count = rnd.Next(r_ds.RealDataSet.Tables[0].Rows.Count);
                        DataRow r_dr = r_ds.RealDataSet.Tables[0].Rows[count];
                        obj = r_dr[column.RefVal.ValueMember];
                    }
                    break;
            }
            return obj;
        }

        public SqlDbType GetSqlDBType(String DatabaseType)
        {
            SqlDbType sqlDB = SqlDbType.BigInt;
            switch (DatabaseType)
            {
                case "BigInt": sqlDB = SqlDbType.BigInt; break;
                case "Binary": sqlDB = SqlDbType.Binary; break;
                case "Bit": sqlDB = SqlDbType.Bit; break;
                case "Char": sqlDB = SqlDbType.Char; break;
                case "DateTime": sqlDB = SqlDbType.DateTime; break;
                case "Decimal": sqlDB = SqlDbType.Decimal; break;
                case "Float": sqlDB = SqlDbType.Float; break;
                case "Image": sqlDB = SqlDbType.Image; break;
                case "Int": sqlDB = SqlDbType.Int; break;
                case "Money": sqlDB = SqlDbType.Money; break;
                case "NChar": sqlDB = SqlDbType.NChar; break;
                case "NText": sqlDB = SqlDbType.NText; break;
                case "NVarChar": sqlDB = SqlDbType.NVarChar; break;
                case "Real": sqlDB = SqlDbType.Real; break;
                case "SmallDateTime": sqlDB = SqlDbType.SmallDateTime; break;
                case "SmallInt": sqlDB = SqlDbType.SmallInt; break;
                case "SmallMoney": sqlDB = SqlDbType.SmallMoney; break;
                case "Text": sqlDB = SqlDbType.Text; break;
                case "Timestamp": sqlDB = SqlDbType.Timestamp; break;
                case "TinyInt": sqlDB = SqlDbType.TinyInt; break;
                case "Udt": sqlDB = SqlDbType.Udt; break;
                case "UniqueIdentifier": sqlDB = SqlDbType.UniqueIdentifier; break;
                case "VarBinary": sqlDB = SqlDbType.VarBinary; break;
                case "VarChar": sqlDB = SqlDbType.VarChar; break;
                case "Variant": sqlDB = SqlDbType.Variant; break;
                case "Xml": sqlDB = SqlDbType.Xml; break;
            }
            return sqlDB;
        }

        public OracleType GetOracleDBType(String DatabaseType)
        {
            OracleType oracleDB = OracleType.BFile;
            switch (DatabaseType)
            {
                case "BFile": oracleDB = OracleType.BFile; break;
                case "Blob": oracleDB = OracleType.Blob; break;
                case "Byte": oracleDB = OracleType.Byte; break;
                case "Char": oracleDB = OracleType.Char; break;
                case "Clob": oracleDB = OracleType.Clob; break;
                case "Cursor": oracleDB = OracleType.Cursor; break;
                case "DateTime": oracleDB = OracleType.DateTime; break;
                case "Double": oracleDB = OracleType.Double; break;
                case "Float": oracleDB = OracleType.Float; break;
                case "Int16": oracleDB = OracleType.Int16; break;
                case "Int32": oracleDB = OracleType.Int32; break;
                case "IntervalDayToSecond": oracleDB = OracleType.IntervalDayToSecond; break;
                case "IntervalYearToMonth": oracleDB = OracleType.IntervalYearToMonth; break;
                case "LongRaw": oracleDB = OracleType.LongRaw; break;
                case "LongVarChar": oracleDB = OracleType.LongVarChar; break;
                case "NChar": oracleDB = OracleType.NChar; break;
                case "NClob": oracleDB = OracleType.NClob; break;
                case "Number": oracleDB = OracleType.Number; break;
                case "NVarChar": oracleDB = OracleType.NVarChar; break;
                case "Raw": oracleDB = OracleType.Raw; break;
                case "RowId": oracleDB = OracleType.RowId; break;
                case "SByte": oracleDB = OracleType.SByte; break;
                case "Timestamp": oracleDB = OracleType.Timestamp; break;
                case "TimestampLocal": oracleDB = OracleType.TimestampLocal; break;
                case "TimestampWithTZ": oracleDB = OracleType.TimestampWithTZ; break;
                case "UInt16": oracleDB = OracleType.UInt16; break;
                case "UInt32": oracleDB = OracleType.UInt32; break;
                case "VarChar": oracleDB = OracleType.VarChar; break;
            }
            return oracleDB;
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
                    flowFileName = flowFileName = string.Format("{0}\\WorkFlow\\{1}", EEPRegistry.Server, flowFileName);
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

    public class waqDataSourceEditor : UITypeEditor
    {
        public waqDataSourceEditor()
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
            if (context.Instance is WebAutoQuery)
            {
                ControlCollection ctrlList = ((WebAutoQuery)context.Instance).Page.Controls;
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
}
