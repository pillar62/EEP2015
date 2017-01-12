using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Globalization;
using System.Net;

namespace EFWCFModule
{
    /// <summary>
    /// Information of client
    /// </summary>
    [DataContract]
    public class ClientInfo
    {
        private string _userID;
        /// <summary>
        /// Gets or sets id of user
        /// </summary>
        [DataMember]
        public string UserID
        {
            get
            {
                return _userID;
            }
            set
            {
                _userID = value;
            }
        }

        private string _userName;
        /// <summary>
        /// Gets or sets name of user
        /// </summary>
        [DataMember]
        public string UserName
        {
            get
            {
                return _userName;
            }
            set
            {
                _userName = value;
            }
        }

        private string _password;
        /// <summary>
        /// Gets or sets password of user
        /// </summary>
        [DataMember]
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
            }
        }

        private List<GroupInfo> _groups;
        /// <summary>
        /// Gets or sets groups of user
        /// </summary>
        [DataMember]
        public List<GroupInfo> Groups
        {
            get
            {
                return _groups;
            }
            set
            {
                _groups = value;
            }
        }

        private String _CurrentGroup;
        /// <summary>
        /// Gets or sets groups of user
        /// </summary>
        [DataMember]
        public String CurrentGroup
        {
            get
            {
                return _CurrentGroup;
            }
            set
            {
                _CurrentGroup = value;
            }
        }

        private string _site;
        /// <summary>
        /// Gets or sets site of user
        /// </summary>
        [DataMember]
        public string Site
        {
            get
            {
                return _site;
            }
            set
            {
                _site = value;
            }
        }

        private string _database;
        /// <summary>
        /// Gets or sets database logoned
        /// </summary>
        [DataMember]
        public string Database
        {
            get
            {
                return _database;
            }
            set
            {
                _database = value;
            }
        }

        private string _databasetype;
        /// <summary>
        /// Gets or sets databasetype logoned
        /// </summary>
        [DataMember]
        public string DatabaseType
        {
            get
            {
                return _databasetype;
            }
            set
            {
                _databasetype = value;
            }
        }

        private string _databasesubtype;
        /// <summary>
        /// Gets or sets databasetype logoned
        /// </summary>
        [DataMember]
        public string DatabaseSubType
        {
            get
            {
                if (_databasetype == "ODBC" && String.IsNullOrEmpty(_databasesubtype))
                {
                    EFService efs = new EFService();
                    var res = efs.CallServerMethod(this, "GLModule", "GetDataBaseSubType", new object[] { this.Database });
                    if (res != null)
                    {
                        _databasesubtype = res.ToString();
                    }
                }
                return _databasesubtype;
            }
            set
            {
                _databasesubtype = value;
            }
        }

        private string _solution;
        /// <summary>
        /// Gets or sets solution logoned
        /// </summary>
        [DataMember]
        public string Solution
        {
            get
            {
                return _solution;
            }
            set
            {
                _solution = value;
            }
        }

        private LogonResult _logonResult;
        /// <summary>
        /// Gets or sets result of logon
        /// </summary>
        [DataMember]
        public LogonResult LogonResult
        {
            get
            {
                return _logonResult;
            }
            set
            {
                _logonResult = value;
            }
        }

        private string _securityKey;
        /// <summary>
        /// Gets or sets security key
        /// </summary>
        [DataMember]
        public string SecurityKey
        {
            get
            {
                return _securityKey;
            }
            set
            {
                _securityKey = value;
            }
        }

        private string _locale;
        /// <summary>
        /// Gets or sets locale of user
        /// </summary>
        [DataMember]
        public string Locale
        {
            get
            {
                return _locale;
            }
            set
            {
                _locale = value;
            }
        }

        private string _ipAddress;
        /// <summary>
        /// Gets or sets IP address of user
        /// </summary>
        [DataMember]
        public string IPAddress
        {
            get
            {
                return _ipAddress;
            }
            set
            {
                _ipAddress = value;
            }
        }

        private string _serverIPAddress;
        /// <summary>
        /// Gets or sets server IP address
        /// </summary>
        [DataMember]
        public string ServerIPAddress
        {
            get
            {
                return _serverIPAddress;
            }
            set
            {
                _serverIPAddress = value;
            }
        }

        private bool _useDataSet;
        /// <summary>
        /// Gets or sets use dataset
        /// </summary>
        [DataMember]
        public bool UseDataSet
        {
            get
            {
                return _useDataSet;
            }
            set
            {
                _useDataSet = value;
            }
        }

        private bool _isSDModule;
        /// <summary>
        /// Gets or sets use SDModule
        /// </summary>
        [DataMember]
        public bool IsSDModule
        {
            get
            {
                return _isSDModule;
            }
            set
            {
                _isSDModule = value;
            }
        }

        private string _sdDeveloperID;
        /// <summary>
        /// Gets or sets develperid
        /// </summary>
        [DataMember]
        public string SDDeveloperID
        {
            get
            {
                return _sdDeveloperID;
            }
            set
            {
                _sdDeveloperID = value;
            }
        }

        private string _orgKind = "0";
        /// <summary>
        /// OrgKind add new by lu 2012/12/11
        /// </summary>
        [DataMember]
        public string OrgKind
        {
            get
            {
                return _orgKind;
            }
            set
            {
                _orgKind = value;
            }
        }

        private string _UserPara1;
        /// <summary>
        /// UserPara1
        /// </summary>
        [DataMember]
        public string UserPara1
        {
            get
            {
                return _UserPara1;
            }
            set
            {
                _UserPara1 = value;
            }
        }
        private string _UserPara2;
        /// <summary>
        /// UserPara1
        /// </summary>
        [DataMember]
        public string UserPara2
        {
            get
            {
                return _UserPara2;
            }
            set
            {
                _UserPara2 = value;
            }
        }

        private string _AUTOLOGIN;
        /// <summary>
        /// UserPara1
        /// </summary>
        [DataMember]
        public string AUTOLOGIN
        {
            get
            {
                return _AUTOLOGIN;
            }
            set
            {
                _AUTOLOGIN = value;
            }

        }
        private int _cErrorCode;
        /// <summary>
        /// ClientErrorCode
        /// </summary>
        [DataMember]
        public int cErrorCode
        {
            get
            {
                return _cErrorCode;
            }
            set
            {
                _cErrorCode = value;
            }
        }

        private int _sErrorCode;
        /// <summary>
        /// ServerErrorCode
        /// </summary>
        [DataMember]
        public int sErrorCode
        {
            get
            {
                return _sErrorCode;
            }
            set
            {
                _sErrorCode = value;
            }
        }

    }

    /// <summary>
    /// Result of logon
    /// </summary>
    public enum LogonResult
    {
        /// <summary>
        /// Not logoned
        /// </summary>
        NotLogoned = 0,
        /// <summary>
        /// Logoned successfully
        /// </summary>
        Logoned = 1,
        /// <summary>
        /// Password is error
        /// </summary>
        PasswordError = 2,
        /// <summary>
        /// User not found
        /// </summary>
        UserNotFound = 3,
        /// <summary>
        /// User is disabled to logon
        /// </summary>
        UserDisabled = 4,
        /// <summary>
        /// Server exceed max user count
        /// </summary>
        UserCountExceedMax = 5
    }

    /// <summary>
    /// Information of group
    /// </summary>
    [DataContract]
    public class GroupInfo
    {
        private string _id;
        /// <summary>
        /// Gets or sets id of group
        /// </summary>
        [DataMember]
        public string ID
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }

        private string _name;
        /// <summary>
        /// Gets or sets name of group
        /// </summary>
        [DataMember]
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        private GroupType _type;
        /// <summary>
        /// Gets or sets type of group
        /// </summary>
        [DataMember]
        public GroupType Type
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
            }
        }
    }

    /// <summary>
    /// Type of group
    /// </summary>
    public enum GroupType
    {
        /// <summary>
        /// Normal group
        /// </summary>
        Normal = 0,
        /// <summary>
        /// Role group
        /// </summary>
        Role = 1,
        /// <summary>
        /// Org group
        /// </summary>
        Org = 2,
        /// <summary>
        /// Org share group
        /// </summary>
        OrgShare = 3
    }

    /// <summary>
    /// Information of packet
    /// </summary>
    [DataContract]
    public class PacketInfo
    {
        private int _startIndex;
        /// <summary>
        /// Gets or sets start index of packet records
        /// </summary>
        [DataMember]
        public int StartIndex
        {
            get
            {
                return _startIndex;
            }
            set
            {
                _startIndex = value;
            }
        }

        private int _count;
        /// <summary>
        /// Gets or sets count of packet records
        /// </summary>
        [DataMember]
        public int Count
        {
            get
            {
                return _count;
            }
            set
            {
                _count = value;
            }
        }

        private List<WhereParameter> _whereParamters;
        /// <summary>
        /// Gets or sets parameters of where
        /// </summary>
        [DataMember]
        public List<WhereParameter> WhereParameters
        {
            get
            {
                return _whereParamters;
            }
            set
            {
                _whereParamters = value;
            }
        }

        private List<OrderParameter> _orderParameters;
        /// <summary>
        /// Gets or sets parameters of order by
        /// </summary>
        [DataMember]
        public List<OrderParameter> OrderParameters
        {
            get
            {
                return _orderParameters;
            }
            set
            {
                _orderParameters = value;
            }
        }

        private bool _onlyschema;
        /// <summary>
        /// Gets or sets only schema
        /// </summary>
        [DataMember]
        public bool OnlySchema
        {
            get
            {
                return _onlyschema;
            }
            set
            {
                _onlyschema = value;
            }
        }

        private string _whereString;
        /// <summary>
        /// Gets or sets query string
        /// </summary>
        [DataMember]
        public string WhereString
        {
            get
            {
                return _whereString;
            }
            set
            {
                _whereString = value;
            }

        }
    }

    /// <summary>
    /// Parameter of where
    /// </summary>
    [DataContract]
    public class WhereParameter
    {
        private string _field;
        /// <summary>
        /// Gets or sets field of where
        /// </summary>
        [DataMember]
        public string Field
        {
            get
            {
                return _field;
            }
            set
            {
                _field = value;
            }
        }

        private bool _and;
        /// <summary>
        /// Gets or sets connect operator of where, true use 'and', otherwise use 'or'
        /// </summary>
        [DataMember]
        public bool And
        {
            get
            {
                return _and;
            }
            set
            {
                _and = value;
            }
        }

        private WhereCondition _condition;
        /// <summary>
        /// Gets or sets condtion of where
        /// </summary>
        [DataMember]
        public WhereCondition Condition
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

        private object _value;
        /// <summary>
        /// Gets or sets value of where
        /// </summary>
        [DataMember]
        public object Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }

        private List<object> _values;
        /// <summary>
        /// Gets or sets values of where
        /// </summary>
        [DataMember]
        public List<object> Values
        {
            get
            {
                return _values;
            }
            set
            {
                _values = value;
            }
        }
    }

    /// <summary>
    /// Condition of where
    /// </summary>
    public enum WhereCondition
    {
        /// <summary>
        /// Condition(=value)
        /// </summary>
        Equal = 0,
        /// <summary>
        /// Condition(&lt;&gt;value)
        /// </summary>
        NotEqual = 1,
        /// <summary>
        /// Condition(&gt;value)
        /// </summary>
        GreaterThan = 2,
        /// <summary>
        /// Condidtion(&gt;=value)
        /// </summary>
        EqualOrGreaterThan = 3,
        /// <summary>
        /// Condition(&lt;value)
        /// </summary>
        LessThan = 4,
        /// <summary>
        /// Condidtion(&lt;=value)
        /// </summary>
        EqualOrLessThan = 5,
        /// <summary>
        /// Condition(like 'value%')
        /// </summary>
        BeginWith = 6,
        /// <summary>
        /// Condition(like '%value%')
        /// </summary>
        Contain = 7,
        /// <summary>
        /// Condition(in (values))
        /// </summary>
        In = 8
    }

    /// <summary>
    /// Parameter of order by
    /// </summary>
    [DataContract]
    public class OrderParameter
    {
        private string _field;
        /// <summary>
        /// Gets or sets field of order
        /// </summary>
        [DataMember]
        public string Field
        {
            get
            {
                return _field;
            }
            set
            {
                _field = value;
            }
        }

        private OrderDirection _direction;
        /// <summary>
        /// Gets or sets direction of order
        /// </summary>
        [DataMember]
        public OrderDirection Direction
        {
            get
            {
                return _direction;
            }
            set
            {
                _direction = value;
            }

        }
    }

    /// <summary>
    /// Direction of order by
    /// </summary>
    public enum OrderDirection
    {
        /// <summary>
        /// order by asc
        /// </summary>
        Ascending = 0,
        /// <summary>
        /// order by desc
        /// </summary>
        Descending = 1
    }

    /// <summary>
    /// Infomation of solution
    /// </summary>
    [DataContract]
    public class SolutionInfo
    {
        private string _id;
        /// <summary>
        /// Gets or sets id of solution
        /// </summary>
        [DataMember]
        public string ID
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }

        private string _name;
        /// <summary>
        /// Gets or sets name of solution
        /// </summary>
        [DataMember]
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        private string _defaultDatabase;
        /// <summary>
        /// Gets or sets default database of solution
        /// </summary>
        [DataMember]
        public string DefaultDatabase
        {
            get
            {
                return _defaultDatabase;
            }
            set
            {
                _defaultDatabase = value;
            }
        }

        private byte[] _logOnImage;
        /// <summary>
        /// Gets or sets default LogOnImage of solution
        /// </summary>
        [DataMember]
        public byte[] LogOnImage
        {
            get
            {
                return _logOnImage;
            }
            set
            {
                _logOnImage = value;
            }
        }

        private string _bgStartColor;
        /// <summary>
        /// Gets or sets default BGStartColor of solution
        /// </summary>
        [DataMember]
        public string BGStartColor
        {
            get
            {
                return _bgStartColor;
            }
            set
            {
                _bgStartColor = value;
            }
        }

        private string _bgEndColor;
        /// <summary>
        /// Gets or sets default BGEndColor of solution
        /// </summary>
        [DataMember]
        public string BGEndColor
        {
            get
            {
                return _bgEndColor;
            }
            set
            {
                _bgEndColor = value;
            }
        }
    }

    /// <summary>
    /// Infomation of menu
    /// </summary>
    [DataContract]
    public class MenuInfo
    {
        private string _menuid;
        /// <summary>
        /// Gets or sets id of solution
        /// </summary>
        [DataMember]
        public string MENUID
        {
            get
            {
                return _menuid;
            }
            set
            {
                _menuid = value;
            }
        }

        private string _caption;
        /// <summary>
        /// Gets or sets name of solution
        /// </summary>
        [DataMember]
        public string CAPTION
        {
            get
            {
                return _caption;
            }
            set
            {
                _caption = value;
            }
        }

        private string _parent;
        /// <summary>
        /// Gets or sets default database of solution
        /// </summary>
        [DataMember]
        public string PARENT
        {
            get
            {
                return _parent;
            }
            set
            {
                _parent = value;
            }
        }
    }

    /// <summary>
    /// Parameter of workflow
    /// </summary>
    [DataContract]
    public class FlowParameter
    {
        private Guid _instanceID;
        /// <summary>
        /// Gets or sets id of instance
        /// </summary>
        [DataMember]
        public Guid InstanceID
        {
            get
            {
                return _instanceID;
            }
            set
            {
                _instanceID = value;
            }
        }

        private string _xomlName;
        /// <summary>
        /// Gets or sets name of xoml file
        /// </summary>
        [DataMember]
        public string XomlName
        {
            get
            {
                return _xomlName;
            }
            set
            {
                _xomlName = value;
            }
        }

        private FlowOperation _operation;
        /// <summary>
        /// Gets or sets operation of work flow
        /// </summary>
        [DataMember]
        public FlowOperation Operation
        {
            get
            {
                return _operation;
            }
            set
            {
                _operation = value;
            }
        }

        private string _roleID;
        /// <summary>
        /// Gets or  ID of role
        /// </summary>
        [DataMember]
        public string RoleID
        {
            get
            {
                return _roleID;
            }
            set
            {
                _roleID = value;
            }
        }

        private string _orgKind;
        /// <summary>
        /// Gets or sets kind of organization
        /// </summary>
        [DataMember]
        public string OrgKind
        {
            get
            {
                return _orgKind;
            }
            set
            {
                _orgKind = value;
            }
        }

        private string _provider;
        /// <summary>
        /// Gets or sets name of server provider
        /// </summary>
        [DataMember]
        public string Provider
        {
            get
            {
                return _provider;
            }
            set
            {
                _provider = value;
            }
        }

        private string _currentActivity;
        /// <summary>
        /// Gets or sets current activity
        /// </summary>
        [DataMember]
        public string CurrentActivity
        {
            get
            {
                return _currentActivity;
            }
            set
            {
                _currentActivity = value;
            }
        }

        private string _nextActivity;
        /// <summary>
        /// Gets or sets next activity
        /// </summary>
        [DataMember]
        public string NextActivity
        {
            get
            {
                return _nextActivity;
            }
            set
            {
                _nextActivity = value;
            }
        }

        private bool _important;
        /// <summary>
        /// Gets or sets whether is important
        /// </summary>
        [DataMember]
        public bool Important
        {
            get
            {
                return _important;
            }
            set
            {
                _important = value;
            }
        }

        private bool _urgent;
        /// <summary>
        /// Gets or sets whether is urgent
        /// </summary>
        [DataMember]
        public bool Urgent
        {
            get
            {
                return _urgent;
            }
            set
            {
                _urgent = value;
            }
        }

        private string _remark;
        /// <summary>
        /// Gets or sets remark
        /// </summary>
        [DataMember]
        public string Remark
        {
            get
            {
                return _remark;
            }
            set
            {
                _remark = value;
            }
        }

        private string _attachments;
        /// <summary>
        /// Gets or sets attachments
        /// </summary>
        [DataMember]
        public string Attachments
        {
            get
            {
                return _attachments;
            }
            set
            {
                _attachments = value;
            }
        }

        private string _mailAddress;
        /// <summary>
        /// Gets or sets email
        /// </summary>
        [DataMember]
        public string MailAddress
        {
            get { return _mailAddress; }
            set { _mailAddress = value; }
        }

        private bool _notifyAllRoles;
        /// <summary>
        /// Gets or sets whether notify to all roles
        /// </summary>
        [DataMember]
        public bool NotifyAllRoles
        {
            get
            {
                return _notifyAllRoles;
            }
            set
            {
                _notifyAllRoles = value;
            }
        }

        private string _sendToIDs;
        /// <summary>
        /// Gets or sets ids to send
        /// </summary>
        [DataMember]
        public string SendToIDs
        {
            get
            {
                return _sendToIDs;
            }
            set
            {
                _sendToIDs = value;
            }
        }


        private string _keys;
        /// <summary>
        /// Gets or sets keys of data
        /// </summary>
        [DataMember]
        public string Keys
        {
            get
            {
                return _keys;
            }
            set
            {
                _keys = value;
            }
        }

        private string _keyValues;
        /// <summary>
        /// Gets or sets key values of data
        /// </summary>
        [DataMember]
        public string KeyValues
        {
            get
            {
                return _keyValues;
            }
            set
            {
                _keyValues = value;
            }
        }

    }

    /// <summary>
    /// Operation of work flow
    /// </summary>
    public enum FlowOperation
    {
        /// <summary>
        /// Submit
        /// </summary>
        Submit = 0,
        /// <summary>
        /// Approve
        /// </summary>
        Approve = 1,
        /// <summary>
        /// Return
        /// </summary>
        Return = 2,
        /// <summary>
        /// Return to step
        /// </summary>
        ReturnToStep = 3,
        /// <summary>
        /// Retake
        /// </summary>
        Retake = 4,
        /// <summary>
        /// Reject
        /// </summary>
        Reject = 5,
        /// <summary>
        /// PlusApprove
        /// </summary>
        PlusApprove = 6,
        /// <summary>
        /// PlusReturn
        /// </summary>
        PlusReturn = 7,
        /// <summary>
        /// Pause
        /// </summary>
        Pause = 8,
        /// <summary>
        /// Notify
        /// </summary>
        Notify = 9,
        /// <summary>
        /// Preview
        /// </summary>
        Preview = 10,
        /// <summary>
        /// Preview
        /// </summary>
        GetFLPathList = 11,
        /// <summary>
        /// Preview
        /// </summary>
        DeleteNotify = 12,
        /// <summary>
        /// PlusReturnToSender
        /// </summary>
        PlusReturnToSender = 13,
        /// <summary>
        /// ChangeSendTo
        /// </summary>
        ChangeSendTo
    }

    /// <summary>
    /// Result of workflow
    /// </summary>
    [DataContract]
    public class FlowResult
    {
        private FlowStatus _status;
        /// <summary>
        /// Gets or sets status of flow
        /// </summary>
        [DataMember]
        public FlowStatus Status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
            }
        }

        private Guid _instanceID;
        /// <summary>
        /// Gets or sets id of instance
        /// </summary>
        [DataMember]
        public Guid InstanceID
        {
            get
            {
                return _instanceID;
            }
            set
            {
                _instanceID = value;
            }
        }

        private string _message;
        /// <summary>
        /// Gets or sets message of result
        /// </summary>
        [DataMember]
        public string Message
        {
            get
            {
                return _message;
            }
            set
            {
                _message = value;
            }
        }

        private string _nextActivity;
        /// <summary>
        /// Gets or sets next activity
        /// </summary>
        [DataMember]
        public string NextActivity
        {
            get
            {
                return _nextActivity;
            }
            set
            {
                _nextActivity = value;
            }
        }

        private string _sendToIDs;
        /// <summary>
        /// Gets or sets ids to send
        /// </summary>
        [DataMember]
        public string SendToIDs
        {
            get
            {
                return _sendToIDs;
            }
            set
            {
                _sendToIDs = value;
            }
        }

        private String[] _flPathList;
        /// <summary>
        /// Gets or sets ids to send
        /// </summary>
        [DataMember]
        public String[] FLPathList
        {
            get
            {
                return _flPathList;
            }
            set
            {
                _flPathList = value;
            }
        }

        private object _flOther;
        /// <summary>
        /// Gets or sets ids to send
        /// </summary>
        [DataMember]
        public object FLOther
        {
            get
            {
                return _flOther;
            }
            set
            {
                _flOther = value;
            }
        }
    }

    /// <summary>
    /// Status of result
    /// </summary>
    public enum FlowStatus
    {
        Normal = 0,

        Waiting = 1,

        Rejected = 2,

        End = 3,

        Exception = 4
    }

    /// <summary>
    /// Type of flow data
    /// </summary>
    public enum FlowDataType
    {
        /// <summary>
        /// Do
        /// </summary>
        Do = 0,
        /// <summary>
        /// History
        /// </summary>
        History = 1,
        /// <summary>
        /// Notify
        /// </summary>
        Notify = 2,
        /// <summary>
        /// End
        /// </summary>
        End = 3,
        /// <summary>
        /// Organization
        /// </summary>
        Organization = 4,
        /// <summary>
        /// AllUsers
        /// </summary>
        AllUsers = 5,
        /// <summary>
        /// AllGroups
        /// </summary>
        AllGroups = 6,
        /// <summary>
        /// Group
        /// </summary>
        Group = 7,
        /// <summary>
        /// Users
        /// </summary>
        Users = 8,
        /// <summary>
        /// secGroup
        /// </summary>
        secGroup = 9,
        /// <summary>
        /// Overtime
        /// </summary>
        Overtime = 10
    }

    /// <summary>
    /// Parameter
    /// </summary>
    [DataContract]
    public class FlowDataParameter
    {
        private Guid _listID;
        /// <summary>
        /// ID of list
        /// </summary>
        [DataMember]
        public Guid ListID
        {
            get
            {
                return _listID;
            }
            set
            {
                _listID = value;
            }
        }

        private String _userID;
        /// <summary>
        /// ID of list
        /// </summary>
        [DataMember]
        public String UserID
        {
            get
            {
                return _userID;
            }
            set
            {
                _userID = value;
            }

        }

        [DataMember]
        public string GroupID { get; set; }
        [DataMember]
        public string GroupName { get; set; }

        private string _description;
        /// <summary>
        /// Description of flow
        /// </summary>
        [DataMember]
        public string Description
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

        private int _startIndex;
        /// <summary>
        /// Gets or sets start index of packet records
        /// </summary>
        [DataMember]
        public int StartIndex
        {
            get
            {
                return _startIndex;
            }
            set
            {
                _startIndex = value;
            }
        }

        private int _count;
        /// <summary>
        /// Gets or sets count of packet records
        /// </summary>
        [DataMember]
        public int Count
        {
            get
            {
                return _count;
            }
            set
            {
                _count = value;
            }
        }
        /// <summary>
        /// Use for Special ,e.q. save level,save admin
        /// </summary>
        [DataMember]
        public string SpecialUse { get; set; }

        [DataMember]
        public string OrderBy { get; set; }
    }

    [DataContract]
    public class LockStatus
    {
        [DataMember]
        public EFWCFModule.EEPAdapter.LockType LockType { get; set; }
        [DataMember]
        public string UserID { get; set; }
    }

    public enum SDTableType
    {
        UserTable,
        SystemTable,
        SDSystemTable
    }

    [DataContract]
    public class UpdateRow
    {
        [DataMember]
        public System.Data.DataRowState RowState { get; set; }
        [DataMember]
        public Dictionary<string, object> OldValues { get; set; }
        [DataMember]
        public Dictionary<string, object> NewValues { get; set; }
    }

    [DataContract]
    public class SQLCommandInfo
    {
        [DataMember]
        public string CommandText { get; set; }

        [DataMember]
        public Dictionary<string, object> Parameters { get; set; }
    }
}
