using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Data;
using System.Reflection;
using System.Windows.Forms.Design;
using System.Windows.Forms;
using System.Drawing.Design;
using System.Drawing;

using System.Data.SqlClient;
using System.Data.Odbc;
using System.Data.OracleClient;
using System.Data.OleDb;
#if MySql
using MySql.Data.MySqlClient;
#endif

namespace Srvtools
{
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(AutoNumber), "Resources.AutoNum.ico")]
    public class AutoNumber : InfoBaseComp, IAutoNumber, IGetValues
    {
        public AutoNumber(System.ComponentModel.IContainer container)
            :this()
        {
            container.Add(this);
        }

        public AutoNumber()
        {
            InitializeComponent();

            _autoNoID = "";
            _updateComp = null;
            _targetColumn = "";
            _getFixed = "";
            _numDig = 3;
            _startValue = 1;
            _step = 1;
            _overFlow = true;
            _number = null;
            _active = true;
            _OldVersion = false;
            _isNumFill = false;
        }

        private void InitializeComponent()
        {

        }

        #region Properties

        [Category("Infolight"),
        Description("Identification of the control")]
        public String AutoNoID
        {
            get { return _autoNoID; }
            set { _autoNoID = value; }
        }

        [Category("Infolight"),
        Description("The UpdateComponent which the control is bound to")]
        public IUpdateComponent UpdateComp
        {
            get { return _updateComp; }
            set { _updateComp = value; }
        }

        [Category("Infolight"),
        Description("The column which AutoNumber is applied to")]
        [Editor(typeof(PropertyDropDownEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public String TargetColumn
        {
            get { return _targetColumn; }
            set { _targetColumn = value; }
        }

        [Category("Infolight"),
        Description("Pre-code of AutoNumber")]
        public String GetFixed
        {
            get { return _getFixed; }
            set { _getFixed = value; }
        }

        [Category("Infolight"),
        Description("The digit of coding by AutoNumber")]
        public Int32 NumDig
        {
            get { return _numDig; }
            set { _numDig = value; }
        }

        [Category("Infolight"),
        Description("The first number of each regular coding by AutoNumber")]
        public Int32 StartValue
        {
            get { return _startValue; }
            set { _startValue = value; }
        }

        [Category("Infolight"),
        Description("The increment between the coding by AutoNumber")]
        public Int32 Step
        {
            get { return _step; }
            set { _step = value; }
        }

        [Category("Infolight"),
        Description("Indicates whether overflow is allowed")]
        public Boolean OverFlow
        {
            get { return _overFlow; }
            set { _overFlow = value; }
        }

        [Category("Infolight"),
        Description("Indicates whether AutoNumber is enabled or disabled")]
        public Boolean Active
        {
            get { return _active; }
            set { _active = value; }
        }

        [Browsable(false)]
        public Object Number
        {
            set { _number = value; }
            get { return _number; }
        }

        [Browsable(false)]
        public String Name
        {
            set
            {
                if (Site != null)
                {
                    _name = Site.Name;
                }
            }
            get { return _name; }
        }

        [Category("Infolight"),
        Description("Indicates whether use old vision or not")]
        public bool OldVersion
        {
            get { return _OldVersion; }
            set { _OldVersion = value; }
        }

        [Category("Infolight"),
        Description("Indicates whether use isNumFill or not")]
        public bool isNumFill
        {
            get { return _isNumFill; }
            set { _isNumFill = value; }
        }

        [Category("Infolight"),
        Description("Description of the coding")]
        public String Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
            }
        }

        #endregion

        #region Methods

        public object Execute(IDbConnection conn)
        {
            return Execute(conn, null, null);
        }

        public object Execute(IDbConnection conn, string fixedString)
        {
            return Execute(conn, null, fixedString);
        }

        public object Execute(IDbConnection conn, IDbTransaction trans)
        {
            return Execute(conn, trans, null);
        }

        public object Execute(IDbConnection conn, IDbTransaction trans, string fixedString)
        {
            if (fixedString != null && fixedString.Length > 0)
            {
                _fixedString = fixedString;
                _getFixed = fixedString;
            }

            GetNumber(conn, trans, true);
            return _number;
        }

        public void GetNumber(IDbConnection connection, Boolean isAddStep)
        {
            GetNumber(connection, null, isAddStep);
        }

        public void GetNumber(IDbConnection connection, IDbTransaction dbTrans, Boolean isAddStep)
        {
            CheckAutoNumber();
            Object obj = GetCurrNum(connection, dbTrans);

            if (isAddStep == false)
            {
                if (obj == null || obj == DBNull.Value)
                { _number = null; }
                else
                { _number = GetFixedString() + FormatNum(Int32.Parse(obj.ToString())); }
            }
            else
            {
                String sQL = string.Empty;
                Int32 currnum = 0;
                string fixString = GetFixedString();
                if (obj == null || obj == DBNull.Value)
                {
                    if (isNumFill)
                    {
                        sQL = string.Format("insert into SYSAUTONUM(AUTOID, CURRNUM, DESCRIPTION) values('{0}',{1},'{2}')", _autoNoID, _startValue,_description);
                    }
                    else
                    {
                        if (OldVersion)
                        {
                            sQL = string.Format("insert into autonum(NUMBERNAME, CURRENTPREFIX, CURRENTDIGITS) values('{0}','{1}',{2})", _autoNoID, fixString, _startValue);
                        }
                        else
                        {
                            sQL = string.Format("insert into SYSAUTONUM(AUTOID, FIXED, CURRNUM, DESCRIPTION) values('{0}','{1}',{2},'{3}')", _autoNoID, fixString, _startValue, _description);
                        }
                    }
                    currnum = _startValue;
                }
                else
                {
                    if (isNumFill)
                    {
                        sQL = string.Format("update  SYSAUTONUM set CURRNUM = CURRNUM + {0} where  AUTOID = '{1}' and CURRNUM = {2}", _step, _autoNoID, obj);
                    }
                    else
                    {
                        ClientType type = DBUtils.GetDatabaseType(connection);
                        if (OldVersion)
                        {
                            if (type == ClientType.ctOracle)
                            {
                                if (string.IsNullOrEmpty(fixString))
                                {
                                    sQL = string.Format("update autonum set CURRENTDIGITS = CURRENTDIGITS + {0} where NUMBERNAME ='{1}' and CURRENTPREFIX is null and CURRENTDIGITS = {2}"
                                        , _step, _autoNoID, obj);
                                }
                                else
                                {
                                    sQL = string.Format("update autonum set CURRENTDIGITS = CURRENTDIGITS + {0} where NUMBERNAME ='{1}' and CURRENTPREFIX ='{2}' and CURRENTDIGITS = {3}"
                                        , _step, _autoNoID, fixString, obj);
                                }
                            }
                            else
                            {
                                sQL = string.Format("update autonum set CURRENTDIGITS = CURRENTDIGITS + {0} where NUMBERNAME ='{1}' and CURRENTPREFIX ='{2}' and CURRENTDIGITS = {3}"
                                        , _step, _autoNoID, fixString, obj);

                            }
                        }
                        else
                        {
                            if (type == ClientType.ctOracle)
                            {
                                if (string.IsNullOrEmpty(fixString))
                                {
                                    sQL = string.Format("update SYSAUTONUM set CURRNUM = CURRNUM + {0} where AUTOID = '{1}' and FIXED is null and CURRNUM = {2}"
                                        , _step, _autoNoID, obj);
                                }
                                else
                                {
                                    sQL = string.Format("update SYSAUTONUM set CURRNUM = CURRNUM + {0} where AUTOID = '{1}' and FIXED ='{2}' and CURRNUM = {3}"
                                     , _step, _autoNoID, fixString, obj);
                                }
                            }
                            else
                            {
                                sQL = string.Format("update SYSAUTONUM set CURRNUM = CURRNUM + {0} where AUTOID = '{1}' and FIXED ='{2}' and CURRNUM = {3}"
                                     , _step, _autoNoID, fixString, obj);
                            }
                        }
                    }
                    currnum = Int32.Parse(obj.ToString()) + _step;
                }

                IDbCommand command = connection.CreateCommand();
                command.CommandText = sQL;
                if (dbTrans != null)
                    command.Transaction = dbTrans;

                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                Int32 affect = command.ExecuteNonQuery();
                if (affect == 1)
                    if(isNumFill)
                        _number = currnum;
                    else
                        _number = GetFixedString() + FormatNum(currnum); 
            }
        }

        private String GetFixedString()
        {
            if (_fixedString != _getFixed)
            {
                goto GetFixed;
            }
            if (_fixedString != null && _fixedString.Length > 0)
            { return _fixedString; }

            if (_getFixed == null || _getFixed.Length == 0)
            { _fixedString = ""; return _fixedString; }

            GetFixed:
            if (_getFixed == null)
            {
                _fixedString = ""; return _fixedString;
            }
            Char[] cs = _getFixed.ToCharArray();
            object[] myret = SrvUtils.GetValue(_getFixed,this.OwnerComp as DataModule);
            if (myret != null && (int)myret[0] == 0)
            {
                return (string)myret[1];
            }
            //if (cs.Length == 0)
            //{ _fixedString = ""; return _fixedString; }

            if (cs[0] != '"' && cs[0] != '\'')
            {
                Char[] sep1 = "()".ToCharArray();
                String[] sps1 = _getFixed.Split(sep1);

                if (sps1.Length == 3)
                { _fixedString = InvokeOwerMethod(sps1[0], null); return _fixedString; }

                if (sps1.Length == 1)
                { _fixedString = sps1[0]; return _fixedString; }

                if (sps1.Length != 1 && sps1.Length == 3)
                {
                    String message = SysMsg.GetSystemMessage(((DataModule)this.OwnerComp).Language, "Srvtools", "UpdateComponent", "msg_GetFixedIsBad");
                    throw new ArgumentException(String.Format(message, (this.Name), _getFixed));
                }
            }

            Char[] sep2 = null;
            if (cs[0] == '"')
            { sep2 = "\"".ToCharArray(); }
            if (cs[0] == '\'')
            { sep2 = "'".ToCharArray(); }

            String[] sps2 = _getFixed.Split(sep2);
            if (sps2.Length == 3)
            { _fixedString = sps2[1]; return _fixedString; }
            else
            {
                String message = SysMsg.GetSystemMessage(((DataModule)this.OwnerComp).Language, "Srvtools", "UpdateComponent", "msg_GetFixedIsBad");
                throw new ArgumentException(String.Format(message, (this.Name), _getFixed));
            }
        }

        private String InvokeOwerMethod(String methodName, Object[] parameters)
        {
            MethodInfo methodInfo = this.OwnerComp.GetType().GetMethod(methodName);

            Object obj = null;
            if (methodInfo != null)
            { obj = methodInfo.Invoke(this.OwnerComp, parameters); }

            if (obj != null)
            { return obj.ToString(); }
            else
            { return ""; }
        }

        private Object GetCurrNum(IDbConnection connection, IDbTransaction dbTrans)
        {
            String sQL = string.Empty;

            if (isNumFill)
            {
                sQL = string.Format("select CURRNUM from SYSAUTONUM where AUTOID = '{0}'", _autoNoID);
            }
            else
            {
                ClientType type = DBUtils.GetDatabaseType(connection);
                string fixedString = GetFixedString();
                if (OldVersion)
                {
                    if (type == ClientType.ctOracle)
                    {
                        if (string.IsNullOrEmpty(fixedString))
                        {
                            sQL = string.Format("select CURRENTDIGITS from autonum where CURRENTPREFIX is null and NUMBERNAME = '{0}'", _autoNoID);
                        }
                        else
                        {
                            sQL = string.Format("select CURRENTDIGITS from autonum where CURRENTPREFIX = '{0}' and NUMBERNAME = '{1}'", fixedString, _autoNoID);
                        }
                    }
                    else
                    {
                        sQL = string.Format("select CURRENTDIGITS from autonum where CURRENTPREFIX = '{0}' and NUMBERNAME = '{1}'", fixedString, _autoNoID);
                    }
                }
                else
                {
                    if (type == ClientType.ctOracle)
                    {
                        if (string.IsNullOrEmpty(fixedString))
                        {
                            sQL = string.Format("select CURRNUM from SYSAUTONUM where FIXED is null and AUTOID = '{0}'", _autoNoID);
                        }
                        else
                        {
                            sQL = string.Format("select CURRNUM from SYSAUTONUM where FIXED = '{0}' and AUTOID = '{1}'", fixedString, _autoNoID);
                        }
                    }
                    else
                    {
                        sQL = string.Format("select CURRNUM from SYSAUTONUM where FIXED = '{0}' and AUTOID = '{1}'", fixedString, _autoNoID);
                    }
                }
            }
        

            IDbCommand command = connection.CreateCommand();
            command.CommandText = sQL;
            if (dbTrans != null) command.Transaction = dbTrans;

            if (connection.State == ConnectionState.Closed)
                connection.Open();
            IDataReader ida = command.ExecuteReader();
            Object scalar = null;
            if (ida.Read())
            {
                scalar = ida[0];
            }
            command.Cancel();
            ida.Close();
            //Object scalar = command.ExecuteScalar();
            return scalar;
        }

        private String FormatNum(Int32 value)
        {
            if (value >= 10 * (Int32)(Math.Pow(10, (_numDig - 1))) && _overFlow == false)
            {
                String message = SysMsg.GetSystemMessage(((DataModule)this.OwnerComp).Language, "Srvtools", "AutoNumber", "msg_AutoNoOverFlow");
                throw new ArgumentException(String.Format(message, this.Name));
            }
            if (value >= 62 * (Int32)(Math.Pow(10, (_numDig - 1))))
            {
                String message = SysMsg.GetSystemMessage(((DataModule)this.OwnerComp).Language, "Srvtools", "AutoNumber", "msg_AutoNoOverFlow");
                throw new ArgumentException(String.Format(message, this.Name));
            }

            String p1 = ""; String p2 = "";
            Int32 t1 = 0; Int32 t2 = 0; Int32 t3 = 0;

            t1 = value / (Int32)(Math.Pow(10, (_numDig - 1)));
            if (t1 >= 10 && t1 < 36)
            { t3 = t1 + 65 - 10; }

            if (t1 >= 36)
            { t3 = t1 + 97 - 36; }

            if (t1 < 10)
            { t3 = t1 + 48; }

            p1 = Char.ConvertFromUtf32(t3).ToString();


            if (_numDig > 1)
            {
                StringBuilder temp = new StringBuilder();
                for (Int32 i = 0; i < _numDig - 2; i++)
                { temp.Append("0"); }

                String formater = "{0:" + temp.ToString() + "#}";

                t2 = value % (Int32)(Math.Pow(10, (_numDig - 1)));
                //modified by lily 2007/4/2 for 2位編碼時，逢10會將後面的0省掉
                if (t2 == 0 && temp.ToString() == "")
                    p2 = "0";
                else
                    p2 = String.Format(formater, t2);
            }

            return p1 + p2;
        }

        private void CheckAutoNumber()
        {
            if (_autoNoID == "")
            {
                String message = SysMsg.GetSystemMessage(((DataModule)this.OwnerComp).Language, "Srvtools", "AutoNumber", "msg_AutoNoIDIsNull");
                throw new ArgumentException(String.Format(message, this.Name));
            }
            if (_targetColumn == "")
            {
                String message = SysMsg.GetSystemMessage(((DataModule)this.OwnerComp).Language, "Srvtools", "AutoNumber", "msg_TargerColumnIsNull");
                throw new ArgumentException(String.Format(message, this.Name));
            }
        }

        private Object TransformMarkerInColumnValue(String typeName, Object columnValue)
        {
            if (Type.GetType(typeName).Equals(typeof(Char)) || Type.GetType(typeName).Equals(typeof(String)))
            {
                StringBuilder sb = new StringBuilder();
                if (columnValue.ToString().Length > 0)
                {
                    Char[] cVChars = columnValue.ToString().ToCharArray();

                    foreach (Char cVChar in cVChars)
                    {
                        if (cVChar == _marker)
                        { sb.Append(cVChar.ToString()); }

                        sb.Append(cVChar.ToString());
                    }
                }
                return sb.ToString();
            }
            else
            { return columnValue; }
        }

        #endregion

        #region Event
        /*protected void OnGetFixedValue(GetFixedValueEventArgs value)
        {
            GetFixedValueEventHandler handler = (GetFixedValueEventHandler)Events[EventOnGetFixedValue];
            if ((handler != null) && (value is GetFixedValueEventArgs))
            {
                handler(this, (GetFixedValueEventArgs)value);
            }
        }

        internal static readonly object EventOnGetFixedValue = new object();
        public event GetFixedValueEventHandler GetFixedValue
        {
            add { Events.AddHandler(EventOnGetFixedValue, value); }
            remove { Events.RemoveHandler(EventOnGetFixedValue, value); }
        }*/
        #endregion

        #region Vars

        private String _autoNoID;
        private IUpdateComponent _updateComp;
        private String _targetColumn;
        private String _getFixed;
        private Int32 _numDig;
        private Int32 _startValue;
        private Int32 _step;
        private Boolean _overFlow;
        private Object _number;
        private String _name;
        private Boolean _active;
        private String _description;

        private String _fixedString = null;

        private Char _marker = '\'';
        private bool _OldVersion;
        private bool _isNumFill;

        #endregion

        #region IGetValues

        public string[] GetValues(string sKind)
        {
            if (string.Compare(sKind, "targetcolumn", true) == 0)//IgnoreCase
            {
                UpdateComponent updateComp = (UpdateComponent)_updateComp;
                if (updateComp == null)
                { return null; }

                if(updateComp.SelectCmd == null)
                { throw new Exception("The 'UpdateComp.SelectCmd' property is null."); }

                return updateComp.SelectCmd.GetFields();

            }
            else
            {
                return null;
            }
        }

        #endregion
    }

    #region GetFixedValue Event
    /*public delegate void GetFixedValueEventHandler(object sender, GetFixedValueEventArgs e);

    public sealed class GetFixedValueEventArgs : EventArgs
    {
        public GetFixedValueEventArgs(string fixedValue)
        {
            _FixedValue = fixedValue;
        }

        private string _FixedValue;
        public string FixedValue
        {
            get
            {
                return _FixedValue;
            }
        }
    }*/
    #endregion
}
