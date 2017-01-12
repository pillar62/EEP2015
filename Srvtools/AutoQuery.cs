using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.OracleClient;
using System.Data.OleDb;
using System.Data.Odbc;
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
using System.Globalization;
using System.Resources;

namespace Srvtools
{
    [ToolboxItem(true)]
    public partial class AutoQuery : InfoBaseComp
    {
        public AutoQuery()
        {
            _Columns = new AutoQueryColumns(this, typeof(AutoQueryColumn));
            //InitializeComponent();
        }

        public AutoQuery(IContainer container)
        {
            container.Add(this);
            _Columns = new AutoQueryColumns(this, typeof(AutoQueryColumn));
            //InitializeComponent();
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

        private AutoQueryColumns _Columns;
        [Category("Infolight"),
        Description("Specifies the columns which AutoTest is applied to")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public AutoQueryColumns Columns
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

        public void ExecuteTest(int times, int interval, String user, String package, String Log, String Fixed)
        {
            ArrayList alLog = new ArrayList();
            DateTime start = DateTime.Now;
            alLog.Add("AutoQuery log file.");
            alLog.Add("UserID: " + user + "   PackageName: " + package + "   Start Time: " + start + " " + start.Millisecond);

            Execute(user, times, alLog, interval, package, start,Fixed);
            
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
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    MessageBox.Show(ex.GetType().ToString());
                }
            }
        }

        Random m_rnd = new Random();
        public void Execute(String userID, int times, ArrayList aLog, int interval, String package, DateTime start, String Fixed)
        {
            InfoBindingSource ibs = this.infoBindingSource;
            if (ibs == null || ibs.DataSource == null) return;
            InfoDataSet ids = ibs.DataSource as InfoDataSet;
            int number = 0;
            for (int i = 0; i < times; i++)
            {
                number = i + 1;

                int temp = 0;
                if (userID[userID.Length - 1] > 47 && userID[userID.Length - 1] < 58)
                {
                    switch (userID[userID.Length - 1])
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

                if (number % (temp + 1) == 0 || i == temp || number == times)
                {
                    try
                    {
                        String strPath = Application.StartupPath + "\\";
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
                                    if (string.Compare(DBXML.DocumentElement.ChildNodes[j].Attributes["UserId"].InnerText.Trim(), userID, true) == 0//IgnoreCase
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
                                    attr.Value = userID;
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
                                    attr.Value = times.ToString();
                                    elem.Attributes.Append(attr);

                                    attr = DBXML.CreateAttribute("Status");
                                    attr.Value = ((float)(number) / (float)(times)) * 100 + "%";
                                    elem.Attributes.Append(attr);

                                    attr = DBXML.CreateAttribute("CompleteTime");
                                    if (number == times)
                                        attr.Value = DateTime.Now.ToString();
                                    else
                                        attr.Value = "";
                                    elem.Attributes.Append(attr);

                                    attr = DBXML.CreateAttribute("Flag");
                                    if (number == times)
                                        attr.Value = "1";
                                    else
                                        attr.Value = "";
                                    elem.Attributes.Append(attr);


                                    DBXML.DocumentElement.AppendChild(elem);
                                }
                                else
                                {
                                    aNode.Attributes["Number"].InnerText = number.ToString();
                                    aNode.Attributes["Status"].InnerText = ((float)(number) / (float)(times)) * 100 + "%";
                                    if (number == times)
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
                ids.SetWhere(whereStr, whereArr);

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

        public char getRandomChar(char a, char b)
        {
            Random m_rnd = new Random();
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
            Random m_rnd = new Random();
            int ret = m_rnd.Next(122);
            while (ret < 65 || (ret > 90 && ret < 97) || ret > end)
            {
                ret = m_rnd.Next(122);
            }
            return (char)ret;
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
    }


    public class AutoQueryColumn : InfoOwnerCollectionItem, IGetValues
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
        [Category("Infolight")]
        [Editor(typeof(PropertyDropDownEditor), typeof(System.Drawing.Design.UITypeEditor))]
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

        private string _operator = "=";
        [Category("Infolight")]
        [Editor(typeof(FieldNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        //[DefaultValue("=")]
        public string Operator
        {
            get
            {
                return _operator;
            }
            set
            {
                _operator = value;
            }
        }

        private string _condition;
        [Category("Infolight")]
        [Editor(typeof(FieldNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [DefaultValue("And")]
        public string Condition
        {
            get
            {
                return _condition;
            }
            set
            {
                _condition = value;
            }
        }

        private string _databaseType;
        [Category("Infolight")]
        [Editor(typeof(FieldNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [DefaultValue("None")]
        public string DatabaseType
        {
            get
            {
                return _databaseType;
            }
            set
            {
                _databaseType = value;
            }
        }

        private InfoRefVal _RefVal;
        [Category("Infolight")]
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

        public string[] GetValues(string sKind)
        {
            string[] retList = null;
            List<string> values = new List<string>();
            AutoQuery aq = this.Owner as AutoQuery;
            WebAutoQuery waq = this.Owner as WebAutoQuery;
            if (string.Compare(sKind, "columnname", true) == 0)//IgnoreCase
            {
                if (aq != null)
                {
                    InfoDataSet innerDS = aq.infoBindingSource.DataSource as InfoDataSet;
                    if (innerDS != null && innerDS.RealDataSet != null && innerDS.RealDataSet.Tables.Count > 0)
                    {
                        foreach (DataColumn dc in innerDS.RealDataSet.Tables[0].Columns)
                        {
                            values.Add(dc.ColumnName);
                        }
                    }
                }
                else if (waq != null)
                {
                    WebDataSource innerDS = new WebDataSource();
                    foreach (System.Web.UI.Control c in waq.Page.Controls)
                    {
                        if (c is WebDataSource && (c as WebDataSource).ID == waq.DataSourceID)
                        {
                            innerDS = c as WebDataSource;
                            if (innerDS.DesignDataSet == null)
                            {
                                WebDataSet wds = WebDataSet.CreateWebDataSet(innerDS.WebDataSetID);
                                if (wds != null)
                                {
                                    innerDS.DesignDataSet = wds.RealDataSet;
                                }
                            }
                            break;
                        }
                    }

                    if (innerDS != null && innerDS.DesignDataSet != null)
                    {
                        foreach (DataColumn dc in innerDS.DesignDataSet.Tables[0].Columns)
                        {
                            values.Add(dc.ColumnName);
                        }
                    }
                }
            }
            else if (string.Compare(sKind, "operator", true) == 0)//IgnoreCase
            {
                values.Add("=");
                values.Add("!=");
                values.Add(">");
                values.Add("<");
                values.Add(">=");
                values.Add("<=");
                values.Add("%");
                values.Add("%%");
            }
            else if (string.Compare(sKind, "condition", true) == 0)//IgnoreCase
            {
                values.Add("AND");
                values.Add("OR");
            }
            else if (string.Compare(sKind, "databasetype", true) == 0)//IgnoreCase
            {
                if (aq != null)
                {
                    switch (aq.ConnType)
                    {
                        case ConnectionType.SqlClient:
                            values.Add("BigInt");
                            values.Add("Binary");
                            values.Add("Bit");
                            values.Add("Char");
                            values.Add("DateTime");
                            values.Add("Decimal");
                            values.Add("Float");
                            values.Add("Image");
                            values.Add("Int");
                            values.Add("Money");
                            values.Add("NChar");
                            values.Add("NText");
                            values.Add("NVarChar");
                            values.Add("Real");
                            values.Add("SmallDateTime");
                            values.Add("SmallInt");
                            values.Add("SmallMoney");
                            values.Add("Text");
                            values.Add("Timestamp");
                            values.Add("TinyInt");
                            values.Add("Udt");
                            values.Add("UniqueIdentifier");
                            values.Add("VarBinary");
                            values.Add("VarChar");
                            values.Add("Variant");
                            values.Add("Xml");
                            break;
                        case ConnectionType.OracleClient:
                            values.Add("BFile");
                            values.Add("Blob");
                            values.Add("Byte");
                            values.Add("Char");
                            values.Add("Clob");
                            values.Add("Cursor");
                            values.Add("DateTime");
                            values.Add("Double");
                            values.Add("Float");
                            values.Add("Int16");
                            values.Add("Int32");
                            values.Add("IntervalDayToSecond");
                            values.Add("IntervalYearToMonth");
                            values.Add("LongRaw");
                            values.Add("LongVarChar");
                            values.Add("NChar");
                            values.Add("NClob");
                            values.Add("Number");
                            values.Add("NVarChar");
                            values.Add("Raw");
                            values.Add("RowId");
                            values.Add("SByte");
                            values.Add("Timestamp");
                            values.Add("TimestampLocal");
                            values.Add("TimestampWithTZ");
                            values.Add("UInt16");
                            values.Add("UInt32");
                            values.Add("VarChar");
                            break;
                    }
                }
                else if (waq != null)
                {
                    switch (waq.ConnType)
                    {
                        case ConnectionType.SqlClient:
                            values.Add("BigInt");
                            values.Add("Binary");
                            values.Add("Bit");
                            values.Add("Char");
                            values.Add("DateTime");
                            values.Add("Decimal");
                            values.Add("Float");
                            values.Add("Image");
                            values.Add("Int");
                            values.Add("Money");
                            values.Add("NChar");
                            values.Add("NText");
                            values.Add("NVarChar");
                            values.Add("Real");
                            values.Add("SmallDateTime");
                            values.Add("SmallInt");
                            values.Add("SmallMoney");
                            values.Add("Text");
                            values.Add("Timestamp");
                            values.Add("TinyInt");
                            values.Add("Udt");
                            values.Add("UniqueIdentifier");
                            values.Add("VarBinary");
                            values.Add("VarChar");
                            values.Add("Variant");
                            values.Add("Xml");
                            break;
                        case ConnectionType.OracleClient:
                            values.Add("BFile");
                            values.Add("Blob");
                            values.Add("Byte");
                            values.Add("Char");
                            values.Add("Clob");
                            values.Add("Cursor");
                            values.Add("DateTime");
                            values.Add("Double");
                            values.Add("Float");
                            values.Add("Int16");
                            values.Add("Int32");
                            values.Add("IntervalDayToSecond");
                            values.Add("IntervalYearToMonth");
                            values.Add("LongRaw");
                            values.Add("LongVarChar");
                            values.Add("NChar");
                            values.Add("NClob");
                            values.Add("Number");
                            values.Add("NVarChar");
                            values.Add("Raw");
                            values.Add("RowId");
                            values.Add("SByte");
                            values.Add("Timestamp");
                            values.Add("TimestampLocal");
                            values.Add("TimestampWithTZ");
                            values.Add("UInt16");
                            values.Add("UInt32");
                            values.Add("VarChar");
                            break;
                    }
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
            return retList;
        }

        
    }

    public class AutoQueryColumns : InfoOwnerCollection
    {
        public AutoQueryColumns(object aOwner, Type aItemType)
            : base(aOwner, typeof(AutoQueryColumn))
        {

        }

        new public AutoQueryColumn this[int index]
        {
            get
            {
                return (AutoQueryColumn)InnerList[index];
            }
            set
            {
                if (index > -1 && index < Count)
                    if (value is AutoQueryColumn)
                    {
                        //原来的Collection设置为0
                        ((AutoQueryColumn)InnerList[index]).Collection = null;
                        InnerList[index] = value;
                        //Collection设置为this
                        ((AutoQueryColumn)InnerList[index]).Collection = this;
                    }
            }
        }
    }
}