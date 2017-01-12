using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Web.UI.Design;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Resources;
using System.Windows.Forms.Design;
using Microsoft.Win32;
using System.Xml;
using System.Xml.Serialization;
using System.Data;
using System.Web.UI.WebControls;
using System.Reflection;
using System.IO;
using System.Runtime.InteropServices;
using System.Data.SqlClient;
using System.Data.OracleClient;
using System.Data.Odbc;
using System.Data.OleDb;
#if MySql
using MySql.Data.MySqlClient;
#endif

namespace Srvtools
{
    public sealed class WebDataSourceAddingEventArgs : EventArgs
    {
        private IDictionary _addingValues;
        public IDictionary AddingValues
        {
            get
            {
                return _addingValues;
            }
            set
            {
                _addingValues = value;
            }
        }

        public WebDataSourceAddingEventArgs()
            : base()
        {
        }

        public WebDataSourceAddingEventArgs(IDictionary addingValues)
            : base()
        {
            AddingValues = addingValues;
        }
    }
    public delegate void WebDataSourceAddingEventHandler(object sender, WebDataSourceAddingEventArgs e);

    public sealed class WebDataSourceUpdatingEventArgs : EventArgs
    {
        private IDictionary _updatingValues;
        public IDictionary UpdatingValues
        {
            get
            {
                return _updatingValues;
            }
            set
            {
                _updatingValues = value;
            }
        }

        private IDictionary _updatingOldValues;
        public IDictionary UpdatingOldValues
        {
            get
            {
                return _updatingOldValues;
            }
            set
            {
                _updatingOldValues = value;
            }
        }

        private IDictionary _updatingKeys;
        public IDictionary UpdatingKeys
        {
            get
            {
                return _updatingKeys;
            }
            set
            {
                _updatingKeys = value;
            }
        }

        public WebDataSourceUpdatingEventArgs()
            : base()
        {
        }

        public WebDataSourceUpdatingEventArgs(IDictionary updatingKeys, IDictionary updatingValues, IDictionary updatingOldValues)
            : base()
        {
            UpdatingKeys = updatingKeys;
            UpdatingValues = updatingValues;
            UpdatingOldValues = updatingOldValues;
        }
    }
    public delegate void WebDataSourceUpdatingEventHandler(object sender, WebDataSourceUpdatingEventArgs e);

    public sealed class WebDataSourceDeletingEventArgs : EventArgs
    {
        private IDictionary _deletingValues;
        public IDictionary DeletingValues
        {
            get
            {
                return _deletingValues;
            }
            set
            {
                _deletingValues = value;
            }
        }

        private IDictionary _deletingKeys;
        public IDictionary DeletingKeys
        {
            get
            {
                return _deletingKeys;
            }
            set
            {
                _deletingKeys = value;
            }
        }

        public WebDataSourceDeletingEventArgs()
            : base()
        {
        }

        public WebDataSourceDeletingEventArgs(IDictionary deletingKeys, IDictionary deletingValues)
            : base()
        {
            DeletingKeys = deletingKeys;
            DeletingValues = deletingValues;
        }
    }
    public delegate void WebDataSourceDeletingEventHandler(object sender, WebDataSourceDeletingEventArgs e);

    public sealed class SetWhereArgs : CancelEventArgs
    {
        public SetWhereArgs(string str, ArrayList param)
        {
            whereStr = str;
            whereParam = param;
        }

        private string  whereStr;
        public string  WhereStr
        {
            get { return whereStr; }
            set { whereStr = value; }
        }

        private ArrayList whereParam;

        public ArrayList WhereParam
        {
            get { return whereParam; }
            set { whereParam = value; }
        }
    }
    public delegate void SetWhereEventHandler(object sender, SetWhereArgs e);

    [ToolboxBitmap(typeof(WebDataSource), "Resources.WebDataSource.png")]
    [Designer(typeof(WebDataSourceDesigner), typeof(IDesigner))]
    public class WebDataSource : DataSourceControl, IListSource, IGetSelectAlias, IUseSelectCommand
    {
        private string _webDataSetID;
        //private string _pageName;
        //private static string s;
        private string _dataMember;
        private object _dataSource;

        private string _masterDataSource = "";
        private WebDataSourceView _dataSourceView = null;

        private string _selectCommand;
        private string _selectAlias;
        private Char _marker = '\'';
        private String _quotePrefix = "[";
        private String _quoteSuffix = "]";
        private DataSouceDDInfomationsCollection _DDInfomations;

        public WebDataSource()
        {
            LoadViewState(null);
            this.Init += new EventHandler(Detail_Init);
            _DDInfomations = new DataSouceDDInfomationsCollection(this, typeof(DataSouceDDInfomationsItem));
        }

        protected void Detail_Init(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                CliUtils.fClientSystem = "Web";
            }
        }

        [Category("Infolight")]
        [PersistenceMode(PersistenceMode.InnerProperty),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        TypeConverter(typeof(CollectionConverter)),
        NotifyParentProperty(true)]
        [Browsable(false)]
        public DataSouceDDInfomationsCollection DDInfomations
        {
            get
            {
                if (this._DDInfomations.Count == 0)
                {
                    String keyName = "WebDataSources";

                    String aspxName = EditionDifference.ActiveDocumentFullName();
                    String resourceName = aspxName + @".vi-VN.resx";
                    if (!File.Exists(resourceName)) return _DDInfomations;
                    ResXResourceReader resourceReader = new ResXResourceReader(resourceName);
                    IDictionaryEnumerator enumerator = resourceReader.GetEnumerator();

                    XmlDocument xmlDoc = new XmlDocument();
                    while (enumerator.MoveNext())
                    {
                        if (enumerator.Key.ToString() == keyName)
                        {
                            String sXml = (String)enumerator.Value;
                            xmlDoc.LoadXml(sXml);
                            break;
                        }
                    }
                    resourceReader.Close();

                    if (xmlDoc == null) return _DDInfomations;

                    XmlNode nWDSs = xmlDoc.SelectSingleNode("WebDataSources");
                    if (nWDSs == null) return _DDInfomations;

                    XmlNode nWDS = nWDSs.SelectSingleNode("WebDataSources[@Name='" + this.Site.Name + "']");
                    if (nWDS == null) return _DDInfomations;

                    for (int i = 0; i < nWDS.ChildNodes.Count; i++)
                    {
                        String[] str = nWDS.ChildNodes[i].InnerText.Split(';');
                        DataSouceDDInfomationsItem dd = new DataSouceDDInfomationsItem();
                        dd.ColumnName = str[0];
                        dd.RealColumnName = str[1];
                        dd.RealTableName = str[2];

                        this._DDInfomations.Add(dd);
                    }
                }
                else
                {
                    String keyName = "WebDataSources";

                    String aspxName = EditionDifference.ActiveDocumentFullName();
                    String resourceName = aspxName + @".vi-VN.resx";
                    ResXResourceReader resourceReader = new ResXResourceReader(resourceName);
                    ResXResourceWriter resourceWriter = new ResXResourceWriter(resourceName);
                    IDictionaryEnumerator enumerator = resourceReader.GetEnumerator();

                    XmlDocument xmlDoc = new XmlDocument();
                    while (enumerator.MoveNext())
                    {
                        if (enumerator.Key.ToString() == keyName)
                        {
                            String sXml = (String)enumerator.Value;
                            xmlDoc.LoadXml(sXml);
                        }
                        else
                        {
                            String sXml = (String)enumerator.Value;
                            XmlDocument xmlTemp = new XmlDocument();
                            xmlTemp.LoadXml(sXml);
                            resourceWriter.AddResource(enumerator.Key.ToString(), xmlTemp.InnerXml);
                        }
                    }
                    resourceReader.Close();

                    if (xmlDoc == null) return _DDInfomations;

                    XmlNode nWDSs = xmlDoc.SelectSingleNode("WebDataSources");
                    if (nWDSs == null) return _DDInfomations;

                    XmlNode nWDS = nWDSs.SelectSingleNode("WebDataSources[@Name='" + this.Site.Name + "']");
                    if (nWDS == null) return _DDInfomations;

                    //bool isChange = false;
                    for (int i = 0; i < nWDS.ChildNodes.Count; i++)
                    {
                        String[] str = nWDS.ChildNodes[i].InnerText.Split(';');
                        DataSouceDDInfomationsItem dd = this._DDInfomations[i] as DataSouceDDInfomationsItem;
                        if (dd.ColumnName != str[0] || dd.RealColumnName != str[1] || dd.RealTableName != str[2])
                        {
                            //isChange = true;
                            nWDS.ChildNodes[i].InnerText = dd.ColumnName + ";" + dd.RealColumnName + ";" + dd.RealTableName;
                        }
                    }
                    resourceWriter.AddResource(keyName, xmlDoc.InnerXml);
                    resourceWriter.Close();

                    // 保存当前的文档
                    //EnvDTE.Document doc = EditionDifference.ActiveDocument();
                    //doc.Save(doc.FullName);
                    //doc.Saved = true;
                }
                return _DDInfomations;
            }
            set
            {
                _DDInfomations = value;
            }
        }

        [Category("Infolight")]
        [DefaultValue(-1)]
        public int CommandPacketRecords
        {
            get
            {
                if (this.ViewState["CommandPacketRecords"] != null)
                {
                    return (int)this.ViewState["CommandPacketRecords"];
                }
                return -1;
            }
            set
            {
                this.ViewState["CommandPacketRecords"] = value;
            }
        }

        [Browsable(false)]
        public Char Marker
        {
            get { return _marker; }
            set { _marker = value; }
        }

        [Browsable(false)]
        public string EntityTypeName
        {
            get { return ViewState["EntityTypeName"] == null ? null : (string)ViewState["EntityTypeName"]; }
            set { ViewState["EntityTypeName"] = value; }
        }

        [Browsable(false)]
        public String QuotePrefix
        {
            get { return _quotePrefix; }
            set { _quotePrefix = value; }
        }

        [Browsable(false)]
        public String QuoteSuffix
        {
            get { return _quoteSuffix; }
            set { _quoteSuffix = value; }
        }

        [Category("Infolight"),
        Description("The ID of WebDataSource of master table when the control is the webdatasource of detail table")]
        [Editor(typeof(MasterDataSourceEditor), typeof(UITypeEditor))]
        public string MasterDataSource
        {
            get { return _masterDataSource; }
            set { _masterDataSource = value; }
        }

        [Editor(typeof(WebDataSetEditor), typeof(UITypeEditor))]
        [Category("Infolight"),
        Description("The ID of WebDataSet which the control is bound to")]
        public string WebDataSetID
        {
            get
            {
                return _webDataSetID;
            }
            set
            {
                _webDataSetID = value;
            }
        }

#if VS90
        private string _linqDataSetID;

        [Editor(typeof(LinqDataSetEditor), typeof(UITypeEditor))]
        [Category("Infolight"),
        Description("The ID of LinqDataSet which the control is bound to")]
        public string LinqDataSetID
        {
            get
            {
                return _linqDataSetID;
            }
            set
            {
                _linqDataSetID = value;
            }
        }
#endif

        private ArrayList keyFields;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Obsolete("The recommended alternative is PrimaryKey", false)]
        public ArrayList KeyFields
        {
            get 
            {
                if (keyFields == null)
                {
                    keyFields = new ArrayList();
                    DataColumn[] keys = PrimaryKey;
                    foreach (DataColumn column in keys)
                    {
                        keyFields.Add(column.ColumnName);
                    }
                }
                return keyFields;
            }
            set { }
        }

        [Browsable(false)]
        public bool AlwaysClose
        {
            get { return ViewState["AlwaysClose"] == null ? false : (bool)ViewState["AlwaysClose"]; }
            set { ViewState["AlwaysClose"] = value; }
        }

        [Browsable(false)]
        public object DataSource
        {
            get { return _dataSource; }
            set
            {
                _dataSource = value;
                if (string.IsNullOrEmpty(this.MasterDataSource))
                {
                    if (value is WebDataSet)
                    {
                        RemoteName = ((WebDataSet)value).RemoteName;
                        DataCompressed = ((WebDataSet)value).DataCompressed;
                        ServerModify = ((WebDataSet)value).ServerModify;
                        LastIndex = ((WebDataSet)_dataSource).LastIndex;
                        Eof = ((WebDataSet)_dataSource).Eof;
                        AlwaysClose = ((WebDataSet)_dataSource).AlwaysClose;
                        PacketRecords = ((WebDataSet)_dataSource).PacketRecords;
                        WhereStr = ((WebDataSet)_dataSource).WhereStr;
                        InnerDataSet = ((WebDataSet)_dataSource).RealDataSet;
                        RaiseDataSourceChangedEvent(EventArgs.Empty);
                    }
#if VS90
                    else if (value is LinqDataSet)
                    {
                        RemoteName = ((LinqDataSet)_dataSource).RemoteName;
                        ServerModify = ((LinqDataSet)_dataSource).ServerModify;
                        // LastKeyValues = ((LinqDataSet)_dataSource).LastKeyValues;
                        LastIndex = ((LinqDataSet)_dataSource).LastIndex;
                        CommandText = ((LinqDataSet)_dataSource).CommandText;
                        Eof = ((LinqDataSet)_dataSource).Eof;
                        AlwaysClose = ((LinqDataSet)_dataSource).AlwaysClose;
                        PacketRecords = ((LinqDataSet)_dataSource).PacketRecords;
                        //KeyFields = ((LinqDataSet)_dataSource).GetKeyFields(DataMember);

                        InnerDataSet = (DataSet)((LinqDataSet)_dataSource).RealDataSet;
                        RaiseDataSourceChangedEvent(EventArgs.Empty);
                    }
#endif
                    if (InnerDataSet != null && InnerDataSet.Tables.Count > 0 && InnerDataSet.Tables[0].Rows.Count > 0)
                    {
                        SetPostion(0);
                    }
                }
            }
        }

        private bool _CacheDataSet;
        [Category("Infolight"),
        Description("Indicate whether the dataset is cache in session or viewstate, select true to cache in session,otherwise cache in viewstate")]
        public bool CacheDataSet
        {
            get { return _CacheDataSet; }
            set { _CacheDataSet = value; }
        }


        private bool _autoApply;
        [Category("Infolight"),
        Description("Indicate whether it will apply data to the database automatically after the data is modified")]
        public bool AutoApply
        {
            get { return _autoApply; }
            set { _autoApply = value; }
        }

        private bool _autoApplyForInsert;
        [Category("Infolight"),
        Description("Indicate whether it will apply data to the database automatically after the data is inserted")]
        public bool AutoApplyForInsert
        {
            get
            {
                return _autoApplyForInsert;
            }
            set
            {
                _autoApplyForInsert = value;
            }
        }

        // [Editor("System.Windows.Forms.Design.DataMemberListEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor)), DefaultValue("")]
        [Category("Infolight"),
        Description("The table of view used for binding against")]
        [Editor(typeof(DataMemberEditor), typeof(UITypeEditor))]
        public string DataMember
        {
            get { return _dataMember; }
            set
            {
                _dataMember = value;
                RaiseDataSourceChangedEvent(EventArgs.Empty);
            }
        }

        [Browsable(false)]
        public string RemoteName
        {
            get
            {
                if (DesignMode)
                {
                    if (_webDataSetID != null && _webDataSetID != "")
                    {
                        ViewState["RemoteName"] = WebDataSet.GetRemoteName(_webDataSetID);
                    }
                    return (string)ViewState["RemoteName"];
                }
                else if (string.IsNullOrEmpty(this.MasterDataSource))
                {
                    return (string)ViewState["RemoteName"];
                }
                else
                {
                    return MasterWebDataSource.RemoteName;
                }
            }
            set { ViewState["RemoteName"] = value; }

        }

        [Browsable(false)]
        public bool ServerModify
        {
            get
            {
                if (string.IsNullOrEmpty(this.MasterDataSource))
                {
                    return (bool)ViewState["ServerModify"];
                }
                else
                {
                    return MasterWebDataSource.ServerModify;
                }
            }
            set
            {
                ViewState["ServerModify"] = value;
            }
        }


        [Browsable(false)]
        public bool DataCompressed
        {
            get
            {

                return (bool)ViewState["DataCompressed"];
            }
            set
            {
                ViewState["DataCompressed"] = value;
            }
        }


        [Browsable(false)]
        public DataSet InnerDataSet
        {
            get
            {
                if(!string.IsNullOrEmpty(this.SelectAlias) &&  !string.IsNullOrEmpty(this.SelectCommand))
                {
                    throw new EEPException(EEPException.ExceptionType.PropertyNotSupported, this.GetType(), this.ID, "InnerDataSet", null);
                }
                else if (string.IsNullOrEmpty(this.MasterDataSource))
                {
                    object o = null;
                    if (CacheDataSet && !DesignMode)
                    {
                        o = (DataSet)HttpContext.Current.Session[this.Page.AppRelativeVirtualPath + this.ID];
                    }
                    else
                    {
                        o = (DataSet)ViewState["InnerDataSet"];
                    }
                    return (DataSet)o;
                }
                else
                {
                    return this.MasterWebDataSource.InnerDataSet;
                }
            }
            set
            {
                if (string.IsNullOrEmpty(this.MasterDataSource))
                {
                    if (CacheDataSet && !DesignMode)
                    {
                        HttpContext.Current.Session[this.Page.AppRelativeVirtualPath + this.ID] = value;
                    }
                    else
                    {
                        ViewState["InnerDataSet"] = value;
                    }
                }
            }
        }

        [Browsable(false)]
        public DataTable CommandTable
        {
            get
            {
                object o = null;
                if (CacheDataSet && !DesignMode)
                {
                    o = (DataTable)HttpContext.Current.Session[this.Page.AppRelativeVirtualPath + this.ID + ".CommandTable"];
                }
                else
                {

                    o = (DataTable)ViewState["CommandTable"];
                }
                if (o == null)
                {
                    o = GetCommandTable();
                }

                CommandTable = (DataTable)o;
                return (DataTable)o;
            }
            set
            {
                if (CacheDataSet && !DesignMode)
                {
                    HttpContext.Current.Session[this.Page.AppRelativeVirtualPath + this.ID + ".CommandTable"] = value;
                }
                else
                {
                    ViewState["CommandTable"] = value;
                }
            }
        }


        [Category("Infolight"),
        Description("")]
        public bool AllowAdd
        {
            get
            {
                object obj = this.ViewState["AllowAdd"];
                if (obj != null)
                {
                    return (bool)obj;
                }
                return true;
            }
            set
            {
                this.ViewState["AllowAdd"] = value;
            }
        }

        [Category("Infolight"),
        Description("")]
        public bool AllowDelete
        {
            get
            {
                object obj = this.ViewState["AllowDelete"];
                if (obj != null)
                {
                    return (bool)obj;
                }
                return true;
            }
            set
            {
                this.ViewState["AllowDelete"] = value;
            }
        }

        [Category("Infolight"),
        Description("")]
        public bool AllowUpdate
        {
            get
            {
                object obj = this.ViewState["AllowUpdate"];
                if (obj != null)
                {
                    return (bool)obj;
                }
                return true;
            }
            set
            {
                this.ViewState["AllowUpdate"] = value;
            }
        }

        [Category("Infolight"),
        Description("")]
        public bool AllowPrint
        {
            get
            {
                object obj = this.ViewState["AllowPrint"];
                if (obj != null)
                {
                    return (bool)obj;
                }
                return true;
            }
            set
            {
                this.ViewState["AllowPrint"] = value;
            }
        }

        [Browsable(false)]
        [Obsolete("this property is never use", true)]
        public Hashtable KeysAndValues
        {
            get
            {
                return ViewState["KeysAndValues"] == null ? null : (Hashtable)ViewState["KeysAndValues"];
            }
            set
            {
                ViewState["KeysAndValues"] = value;
            }
        }

        [Browsable(false)]
        [Obsolete("this property is never use", true)]
        public object LastKeyValues
        {
            get
            {
                return ViewState["LastKeyValues"] == null ? null : ViewState["LastKeyValues"];
            }
            set
            {
                ViewState["LastKeyValues"] = value;
            }
        }

        [Browsable(false)]
        public int LastIndex
        {
            get
            {
                if (string.IsNullOrEmpty(this.MasterDataSource))
                {
                    return ViewState["LastIndex"] == null ? -1 : (int)ViewState["LastIndex"];
                }
                else
                {
                    return MasterWebDataSource.LastIndex;
                }
            }
            set
            {
                ViewState["LastIndex"] = value;
            }
        }

        [Browsable(false)]
        public string WhereStr
        {
            get
            {
                if (string.IsNullOrEmpty(this.MasterDataSource))
                {
                    return ViewState["WhereStr"] == null ? null : (string)ViewState["WhereStr"];
                }
                else
                {
                    return MasterWebDataSource.WhereStr;
                }
            }
            set
            {
                ViewState["WhereStr"] = value;
            }
        }

        [Browsable(false)]
        public ArrayList WhereParam
        {
            get
            {
                return (ArrayList)HttpContext.Current.Session[this.Page.AppRelativeVirtualPath + this.ID + "WhereParam"];
            }
            set
            {
                HttpContext.Current.Session[this.Page.AppRelativeVirtualPath + this.ID + "WhereParam"] = value;
            }
        }

        [Browsable(false)]
        public bool Eof
        {
            get
            {
                if (string.IsNullOrEmpty(this.MasterDataSource))
                {
                    bool initEof = true;
                    if (!string.IsNullOrEmpty(_selectAlias) && !string.IsNullOrEmpty(_selectCommand))
                    {
                        initEof = false;
                    }
                    if (CacheDataSet && !DesignMode)
                    {
                        if (this.Page != null && HttpContext.Current.Session[this.Page.AppRelativeVirtualPath + this.ID + "Eof"] != null)
                            return (bool)HttpContext.Current.Session[this.Page.AppRelativeVirtualPath + this.ID + "Eof"];
                        else return false;
                    }
                    else
                    {
                        return ViewState["Eof"] == null ? initEof : (bool)ViewState["Eof"];
                    }
                }
                else
                {
                    return MasterWebDataSource.Eof;
                }
            }
            set
            {
                if (CacheDataSet && !DesignMode && this.Page != null)
                {
                    HttpContext.Current.Session[this.Page.AppRelativeVirtualPath + this.ID + "Eof"] = value;
                }
                else
                {
                    ViewState["Eof"] = value;
                }
            }
        }

        [Browsable(false)]
        public int PacketRecords
        {
            get
            {
                if (string.IsNullOrEmpty(this.MasterDataSource))
                {
                    return ViewState["PacketRecords"] == null ? 100 : (int)ViewState["PacketRecords"];
                }
                return MasterWebDataSource.PacketRecords;
            }
            set
            {
                ViewState["PacketRecords"] = value;
            }
        }

        private DataSet _designDataSet;
        [Browsable(false)]
        public DataSet DesignDataSet
        {
            get
            {
                return _designDataSet;
            }
            set
            {
                _designDataSet = value;
            }
        }

        [Category("Infolight"),
        Description("Specifies the SQL command to get data")]
        [Editor(typeof(SelectCommandEditor), typeof(UITypeEditor))]
        public string SelectCommand
        {
            get
            {
                if (_selectCommand == null)
                    return "";
                else
                    return _selectCommand;
            }
            set
            {
                _selectCommand = value;
            }
        }

        [Category("Infolight"),
        Description("Specifies database")]
        [Editor(typeof(SelectAliasEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string SelectAlias
        {
            get
            {
                return _selectAlias;
            }
            set
            {
                _selectAlias = value;
            }
        }

        private WebDataSource GetMasterDataSource(WebDataSource owner)
        {
            if (string.IsNullOrEmpty(owner.MasterDataSource))//当前就是Master
            {
                return owner;
            }
            else
            {
                if (owner.MasterDataSource == owner.ID)
                {
                    //MasterDataSource不能是自己本身,会造成死循环
                    throw new EEPException(EEPException.ExceptionType.PropertyInvalid, owner.GetType(), owner.ID,  "MasterDataSource", owner.MasterDataSource);
                }
                Control parent = owner.Parent.FindControl(owner.MasterDataSource);

                if (parent == null)
                {
                    //在Page上找不到MasterDataSrouce
                    throw new EEPException(EEPException.ExceptionType.ControlNotFound, owner.GetType(), owner.ID, "WebDataSource", owner.MasterDataSource);
                }
                else if (parent is WebDataSource)
                {
                    return GetMasterDataSource(parent as WebDataSource);//递归直到最顶层
                }
                else
                {
                    throw new EEPException(EEPException.ExceptionType.ControlTypeNotMatch,owner.GetType(), owner.ID, "WebDataSource", owner.MasterDataSource);
                }
            }
        }

        /// <summary>
        /// 取得当前WebDataSource的主Master的WebDataSource(最顶级)
        /// </summary>
        [Browsable(false)]
        public WebDataSource MasterWebDataSource
        {
            get
            {
                WebDataSource master = GetMasterDataSource(this);
                return master;
            }
        }

        /// <summary>
        /// 取得当前WebDataSource的Parent的WebDataSource(父级)
        /// </summary>
        [Browsable(false)]
        public WebDataSource ParentWebDataSource
        {
            get
            {
                if (this.MasterDataSource == this.ID)
                {
                    //MasterDataSource不能是自己本身,会造成死循环
                    throw new EEPException(EEPException.ExceptionType.PropertyInvalid, this.GetType(), this.ID, "MasterDataSource", this.MasterDataSource);
                }
                Control parent = this.Parent.FindControl(this.MasterDataSource);
                if (parent == null)
                {
                    //在Page上找不到MasterDataSrouce
                    throw new EEPException(EEPException.ExceptionType.ControlNotFound, this.GetType(), this.ID,  "WebDataSource", this.MasterDataSource);
                }
                else if (parent is WebDataSource)
                {
                    return parent as WebDataSource;
                }
                else
                {
                    throw new EEPException(EEPException.ExceptionType.ControlTypeNotMatch, this.GetType(), this.ID, "WebDataSource", this.MasterDataSource);
                }
            }
        }


        //private DataView view;
        /// <summary>
        /// 取得当前WebDataSource的视图
        /// </summary>
        [Browsable(false)]
        public DataView View
        {
            get 
            {
                //if (view == null)
                //{
                    DataView view = (this.GetView(this.DataMember) as WebDataSourceView).View;
                    string sort = Sort;
                    if (view != null && !string.IsNullOrEmpty(sort))
                    {
                        view.Sort = Sort;
                    }
                //}
                return view;
            }
        }

        /// <summary>
        ///  取得当前WebDataSource的视图是否为空
        /// </summary>
        [Browsable(false)]
        public bool IsEmpty
        {
            get
            {
                DataView view = View;
                if (view != null)
                {
                    if (view.Count == 1)
                    {
                        if (view.Table.ExtendedProperties.Contains("Empty") && view.Table.ExtendedProperties["Empty"].Equals("True"))
                        {
                            return true;
                        }
                    }
                    else if (view.Count == 0)
                    {
                        return true;
                    }
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        private void SetSort(string sort)
        {
            ViewState["Sort"] = sort;
        }

        /// <summary>
        /// 取得当前的排序表达式(只在ExecuteSelect后生效)
        /// </summary>
        [Browsable(false)]
        public string Sort
        {
            get
            {
                object o = ViewState["Sort"];
                return o == null ? string.Empty : (string)o;
            }
        }

        private void SetPostion(int position)
        {
            ViewState["Position"] = position;
        }

        /// <summary>
        /// 取得选中行在View里的位置(只在ExecuteSelect后生效)
        /// </summary>
        [Browsable(false)]
        public int Position                       
        {
            get 
            {
                object o = ViewState["Position"];
                return o == null ? -1 : (int)o;
            }
        }

        /// <summary>
        /// 取得当前选中的行(只在ExecuteSelect后生效)
        /// </summary>
        [Browsable(false)]
        public DataRow CurrentRow
        {
            get 
            {
                if (IsEmpty)
                {
                    return null;
                }
                DataView view = View;
                int position = Position;
                if (position >= 0 && position < view.Count)
                {
                    return view[position].Row;
                }
                return null;
            }
        }

        private Hashtable relationValues;
        /// <summary>
        /// 取得Relation关联的栏位值
        /// </summary>
        [Browsable(false)]
        public Hashtable RelationValues
        {
            get
            {
                if (!string.IsNullOrEmpty(SelectAlias) && !string.IsNullOrEmpty(SelectCommand))//此属性不支持SelectCmd模式
                {
                    throw new EEPException(EEPException.ExceptionType.PropertyNotSupported, this.GetType(), this.ID, "RelationValues", null);
                }
                else
                {
                    if (relationValues == null)
                    {
                        relationValues = new Hashtable();
                        if (!string.IsNullOrEmpty(MasterDataSource))//不是master
                        {
                            WebDataSource master = MasterWebDataSource;
                            if (master.InnerDataSet == null)
                            {
                                throw new EEPException(EEPException.ExceptionType.ControlNotInitial, this.GetType(), this.ID, "WebDataSource", this.ID);
                            }
                            else if (string.IsNullOrEmpty(DataMember))
                            {
                                throw new EEPException(EEPException.ExceptionType.PropertyNull, this.GetType(), this.ID, "DataMember", null);
                            }
                            DataTable table = master.InnerDataSet.Tables[DataMember];
                            WebDataSource parent = ParentWebDataSource;

                            //直接用CurrentRow属性
                            if (table == null)
                            {
                                throw new EEPException(EEPException.ExceptionType.PropertyInvalid, this.GetType(), this.ID, "DataMember", this.DataMember);
                            }
                            else if (table.ParentRelations.Count == 0 || table.ParentRelations[0].ParentTable.TableName != parent.DataMember)
                            {
                                throw new EEPException(EEPException.ExceptionType.PropertyInvalid,this.GetType(), this.ID, "MasterDataSource", this.MasterDataSource);
                            }
                            else
                            {
                                DataRow parentRow = parent.CurrentRow;
                                if (parentRow != null)
                                {
                                    DataRelation parentRelation = table.ParentRelations[0];
                                    for (int i = 0; i < parentRelation.ParentColumns.Length; i++)
                                    {
                                        relationValues.Add(parentRelation.ChildColumns[i].ColumnName, parentRow[parentRelation.ParentColumns[i].ColumnName]);
                                    }
                                }
                            }
                        }
                    }
                }
                return relationValues;
            }
        }

        private DataColumn[] primaryKey;
        /// <summary>
        /// 取得主键
        /// </summary>
        [Browsable(false)]
        public DataColumn[] PrimaryKey
        {
            get
            {
                if (primaryKey == null)
                {
                    DataView view = View;
                    if (view == null)
                    {
                        primaryKey = new DataColumn[] { };
                    }
                    else
                    {
                        primaryKey = view.Table.PrimaryKey;
                    }
                }
                return primaryKey;
            }
        }


        private string commandText;
        /// <summary>
        /// 取得Sql语句
        /// </summary>
        [Browsable(false)]
        public string CommandText
        {
            get
            {
                if (commandText == null && !this.DesignMode)
                {
                    commandText = DBUtils.GetCommandText(this);
                }
                return commandText;
            }
            set { }
        }

        /// <summary>
        /// 取得TableName的名字
        /// </summary>
        [Browsable(false)]
        public string TableName
        {
            get
            {
                if (!this.DesignMode && !string.IsNullOrEmpty(CommandText))
                {
                    return CliUtils.GetTableName(this.CommandText);
                }
                return string.Empty;
            }
            set { }
        }

        [Browsable(false)]
        private ArrayList LockedRecords
        {
            get 
            {
                return ViewState["LockedRecords"] == null ? new ArrayList() : (ArrayList)ViewState["LockedRecords"];
            }
        }

        /// <summary>
        /// 更新资料
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="values"></param>
        /// <param name="oldValues"></param>
        public void Update(IDictionary keys, IDictionary values, IDictionary oldValues)
        {
            this.GetView(this.DataMember).Update(keys, values, oldValues, new DataSourceViewOperationCallback(UpdateCallback));
        }

        private bool UpdateCallback(int a, Exception b)
        {
            return true;
        }

        /// <summary>
        /// 同步WebDataSource
        /// </summary>
        /// <param name="selectedObject">绑定到Master的控件</param>
        public void ExecuteSelect(Control selectedObject)//selectobject是parent的view
        {
            int position = -1;
            string sort = string.Empty;
            string datasourceID = string.Empty;
            if (selectedObject is GridView)
            {
                GridView gridView = (GridView)selectedObject;
                if (gridView.SortExpression.Length > 0)
                {
                    sort = string.Format("{0}{1}", gridView.SortExpression, gridView.SortDirection == SortDirection.Descending ? " desc" : string.Empty);
                }
                position = (gridView.PageIndex * gridView.PageSize) + gridView.SelectedIndex;
                datasourceID = gridView.DataSourceID;
            }
            else if (selectedObject is DetailsView)
            {
                DetailsView detailsView = (DetailsView)selectedObject;
                position = detailsView.PageIndex;
                datasourceID = detailsView.DataSourceID;
            }
            else if (selectedObject is FormView)
            {
                FormView formView = (FormView)selectedObject;
                position = formView.PageIndex;
                datasourceID = formView.DataSourceID;
            }
            else
            {
                throw new EEPException(EEPException.ExceptionType.MethodNotSupported, this.GetType(), this.ID, "ExecuteSelect", null);
            }
            if (string.IsNullOrEmpty(datasourceID))
            {
                throw new EEPException(EEPException.ExceptionType.PropertyNull, selectedObject.GetType(), selectedObject.ID, "DataSourceID", null);
            }
            else if (datasourceID == this.ID)
            {
                this.SetPostion(position);
                this.SetSort(sort);
            }
            else if (!string.IsNullOrEmpty(this.MasterDataSource) && datasourceID == this.MasterDataSource)
            {
                WebDataSource parent = ParentWebDataSource;
                parent.SetPostion(position);
                parent.SetSort(sort);
            }
            else
            {
                throw new EEPException(EEPException.ExceptionType.MethodNotSupported,this.GetType(), this.ID, "ExecuteSelect", null);
            }
        }

        /// <summary>
        /// 新增时同步WebDataSource
        /// </summary>
        /// <param name="addedObject">绑定到Master的控件</param>
        public void ExecuteAdd(Control addedObject)
        {
            string datasourceID = string.Empty;
            if (addedObject is WebGridView)
            {
                WebGridView gridView = (WebGridView)addedObject;
                datasourceID = gridView.DataSourceID;
                if (!gridView.NeedExecuteAdd)
                    return;
            }
            else if (addedObject is WebDetailsView)
            {
                WebDetailsView detailsView = (WebDetailsView)addedObject;
                datasourceID = detailsView.DataSourceID;
                if (!detailsView.NeedExecuteAdd)
                    return;
            }
            else if (addedObject is WebFormView)
            {
                WebFormView formView = (WebFormView)addedObject;
                datasourceID = formView.DataSourceID;
                if (!formView.NeedExecuteAdd)
                    return;
            }
            else
            {
                throw new EEPException(EEPException.ExceptionType.MethodNotSupported, this.GetType(), this.ID, "ExecuteAdd", null);
            }
            if (string.IsNullOrEmpty(datasourceID))
            {
                throw new EEPException(EEPException.ExceptionType.PropertyNull, addedObject.GetType(), addedObject.ID, "DataSourceID", null);
            }
            else if (!string.IsNullOrEmpty(this.MasterDataSource) && datasourceID == this.MasterDataSource)
            {
                WebDataSource parent = ParentWebDataSource;
                DataView view = parent.View;
                if (view != null)
                {
                    parent.SetPostion(view.Count);
                }
                else
                {
                    parent.SetPostion(-1);
                }
            }
            else
            {
                throw new EEPException(EEPException.ExceptionType.MethodNotSupported, this.GetType(), this.ID, "ExecuteAdd", null);
            }
        }

        /// <summary>
        /// 和View同步资料
        /// </summary>
        /// <param name="syncObject">绑定到View的控件</param>
        public void ExecuteSync(Control syncObject)
        {
            int position = -1;
            string sort = string.Empty;
            string datasourceID = string.Empty;

            if (syncObject is GridView)
            {
                GridView gridView = (GridView)syncObject;
                if (gridView.SortExpression.Length > 0)
                {
                    sort = string.Format("{0}{1}", gridView.SortExpression, gridView.SortDirection == SortDirection.Descending ? " desc" : string.Empty);
                }
                position = (gridView.PageIndex * gridView.PageSize) + gridView.SelectedIndex;
                datasourceID = gridView.DataSourceID;
            }
            else if (syncObject is DetailsView)
            {
                DetailsView detailsView = (DetailsView)syncObject;
                position = detailsView.PageIndex;
                datasourceID = detailsView.DataSourceID;
            }
            else if (syncObject is FormView)
            {
                FormView formView = (FormView)syncObject;
                position = formView.PageIndex;
                datasourceID = formView.DataSourceID;
            }
            else
            {
                throw new EEPException(EEPException.ExceptionType.MethodNotSupported, this.GetType(), this.ID, "ExecuteSync", null);
            }
            if (string.IsNullOrEmpty(datasourceID))
            {
                throw new EEPException(EEPException.ExceptionType.PropertyNull, syncObject.GetType(), syncObject.ID, "DataSourceID", null);
            }
            else if (this.ID == datasourceID )
            {
                throw new EEPException(EEPException.ExceptionType.PropertyInvalid, syncObject.GetType(), syncObject.ID, "DataSourceID", datasourceID); //
            }
            else if(string.IsNullOrEmpty(this.MasterDataSource))
            {
                Control control = this.Parent.FindControl(datasourceID);
                if (control == null)
                {
                    throw new EEPException(EEPException.ExceptionType.ControlNotFound, syncObject.GetType(), syncObject.ID, "WebDataSource", datasourceID);
                }
                else if (control is WebDataSource)
                {
                    WebDataSource view = control as WebDataSource;
                    view.SetSort(sort);
                    view.SetPostion(position);

                    DataRow row = view.CurrentRow;
                    if (row != null)
                    {
                        DataColumn[] viewPrimaryKey = view.PrimaryKey;
                        DataColumn[] masterPrimaryKey = this.PrimaryKey;
                        if (viewPrimaryKey.Length == masterPrimaryKey.Length)
                        {
                            string commandText = this.CommandText;
                            string[] quote = CliUtils.GetDataBaseQuote();
                            StringBuilder where = new StringBuilder();
                            for (int i = 0; i < viewPrimaryKey.Length; i++)
                            {
                                if (viewPrimaryKey[i].DataType == masterPrimaryKey[i].DataType)
                                {
                                    Type type = viewPrimaryKey[i].DataType;
                                    if (where.Length > 0)
                                    {
                                        where.Append(" AND ");
                                    }
                                    where.Append(string.Format("{0}={1}"
                                        , CliUtils.GetTableNameForColumn(commandText, masterPrimaryKey[i].ColumnName)
                                        , Mark(type, TransformMarkerInColumnValue(type, row[viewPrimaryKey[i].ColumnName]))));
                                }
                                else
                                {
                                    throw new EEPException(EEPException.ExceptionType.ControlTypeNotMatch, this.GetType(), this.ID, "WebDataSource", null);
                                }
                            }
                            this.SetWhere(where.ToString());
                        }
                        else
                        {
                            throw new EEPException(EEPException.ExceptionType.ColumnTypeInvalid, this.GetType(), this.ID, "WebDataSource", null);
                        }
                    }
                    else
                    {
                        this.SetWhere("1=0");
                    }
                }
                else
                {
                    throw new EEPException(EEPException.ExceptionType.ControlTypeNotMatch, this.GetType(), this.ID, "WebDataSource", datasourceID);
                }
            }
            else
            {
                throw new EEPException(EEPException.ExceptionType.MethodNotSupported,this.GetType(), this.ID,  "ExecuteSync", null);
            }
        }

        // ------------------------------------------------------------------------------------------
        // Event
        internal static readonly object EventAdding = new object();
        public event WebDataSourceAddingEventHandler Adding
        {
            add { Events.AddHandler(EventAdding, value); }
            remove { Events.RemoveHandler(EventAdding, value); }
        }
        public void OnAdding(WebDataSourceAddingEventArgs value)
        {
            WebDataSourceAddingEventHandler handler = (WebDataSourceAddingEventHandler)Events[EventAdding];
            if ((handler != null) && (value is WebDataSourceAddingEventArgs))
            {
                handler(this, (WebDataSourceAddingEventArgs)value);
            }
        }

        internal static readonly object EventUpdating = new object();
        public event WebDataSourceUpdatingEventHandler Updating
        {
            add { Events.AddHandler(EventUpdating, value); }
            remove { Events.RemoveHandler(EventUpdating, value); }
        }
        public void OnUpdating(WebDataSourceUpdatingEventArgs value)
        {
            WebDataSourceUpdatingEventHandler handler = (WebDataSourceUpdatingEventHandler)Events[EventUpdating];
            if ((handler != null) && (value is WebDataSourceUpdatingEventArgs))
            {
                handler(this, (WebDataSourceUpdatingEventArgs)value);
            }
        }

        internal static readonly object EventDeleting = new object();
        public event WebDataSourceDeletingEventHandler Deleting
        {
            add { Events.AddHandler(EventDeleting, value); }
            remove { Events.RemoveHandler(EventDeleting, value); }
        }
        public void OnDeleting(WebDataSourceDeletingEventArgs value)
        {
            WebDataSourceDeletingEventHandler handler = (WebDataSourceDeletingEventHandler)Events[EventDeleting];
            if ((handler != null) && (value is WebDataSourceDeletingEventArgs))
            {
                handler(this, (WebDataSourceDeletingEventArgs)value);
            }
        }

        internal static readonly object EventOnApplyError = new object();
        /// <summary>
        /// The event ocured when apply data encounter errors
        /// </summary>
        [Category("Infolight"),
        Description("The event ocured when apply data encounter errors")]
        public event Srvtools.InfoDataSet.ApplyErrorEventHandler ApplyError
        {
            add { base.Events.AddHandler(EventOnApplyError, value); }
            remove { base.Events.RemoveHandler(EventOnApplyError, value); }
        }

        protected void OnApplyError(ApplyErrorEventArgs value)
        {
            Srvtools.InfoDataSet.ApplyErrorEventHandler handler = (Srvtools.InfoDataSet.ApplyErrorEventHandler)base.Events[EventOnApplyError];
            if (handler != null)
            {
                handler(this, value);
            }
        }

        internal static readonly object EventOnSetWhereing = new object();
        /// <summary>
        /// The event ocured when set where
        /// </summary>
        [Category("Infolight"),
        Description("The event ocured when set where")]
        public event SetWhereEventHandler SetWhereing
        {
            add { base.Events.AddHandler(EventOnSetWhereing, value); }
            remove { base.Events.RemoveHandler(EventOnSetWhereing, value); }
        }

        protected void OnSetWhereing(SetWhereArgs value)
        {
            SetWhereEventHandler handler = (SetWhereEventHandler)base.Events[EventOnSetWhereing];
            if (handler != null)
            {
                handler(this, value);
            }
        }

        // ------------------------------------------------------------------------------------------

        public void Reload()
        {
            WebDataSet dataSet = new WebDataSet();

            dataSet.RemoteName = RemoteName;
            dataSet.ServerModify = ServerModify;
            dataSet.WhereStr = WhereStr;
            dataSet.PacketRecords = PacketRecords;
            dataSet.WhereParam = WhereParam;
            dataSet.Active = true;
            int i = -1;
            while ((!dataSet.Eof) && i >= LastIndex)
            {
                dataSet.GetNextPacket();
                i = dataSet.RealDataSet.Tables[0].Rows.Count - 1;
            }

            InnerDataSet.Clear();
            InnerDataSet.Tables[0].ExtendedProperties.Clear();
            InnerDataSet.Merge(dataSet.RealDataSet);
        }

        public bool GetNextPacket()
        {
            if (!this.Eof)
            {
                if(!string.IsNullOrEmpty(WebDataSetID))
                {
                    InfoDataSet infoDataSet = new InfoDataSet();

                    // infoDataSet.LastKeyValues = this.LastKeyValues;
                    infoDataSet.DataCompressed = DataCompressed;
                    infoDataSet.LastIndex = this.LastIndex;
                    infoDataSet.Eof = this.Eof;
                    infoDataSet.WhereStr = this.WhereStr;
                    infoDataSet.WhereParam = this.WhereParam;
                    if (!string.IsNullOrEmpty(this.SelectAlias) && !string.IsNullOrEmpty(this.SelectCommand))
                    {
                       // infoDataSet.refDBAlias = this.SelectAlias;
                        infoDataSet.RefCommandText = this.SelectCommand;
                        infoDataSet.RemoteName = "GLModule.cmdRefValUse";
                        infoDataSet.PacketRecords = this.CommandPacketRecords;
                        infoDataSet.RealDataSet.Tables.Add(this.CommandTable);
                    }
                    else
                    {
                        infoDataSet.RemoteName = this.RemoteName;
                        infoDataSet.PacketRecords = this.PacketRecords;
                        infoDataSet.RealDataSet = this.InnerDataSet;
                    }
                    bool b = infoDataSet.GetNextPacket();

                    if (b)
                    {
                        if (!string.IsNullOrEmpty(this.SelectAlias) && !string.IsNullOrEmpty(this.SelectCommand))
                        {
                            this.CommandTable = infoDataSet.RealDataSet.Tables[0];
                        }
                        else
                        {
                            this.InnerDataSet = infoDataSet.RealDataSet;
                        }
                        this.LastIndex = infoDataSet.LastIndex;
                        this.Eof = infoDataSet.Eof;

                        if (View != null && View.Count > 0)
                        {
                            SetPostion(0);
                        }
                        else
                        {
                            SetPostion(-1);
                        }

                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
#if VS90
                else if (!string.IsNullOrEmpty(LinqDataSetID))
                {
                    LinqDataSet linqDataSet = new LinqDataSet();

                    linqDataSet.LastIndex = this.LastIndex;
                    linqDataSet.CommandText = this.CommandText;
                    linqDataSet.Eof = this.Eof;
                    //if (!string.IsNullOrEmpty(this.SelectAlias) && !string.IsNullOrEmpty(this.SelectCommand))
                    //{
                    //    linqDataSet.refDBAlias = this.SelectAlias;
                    //    linqDataSet.refCommandText = this.SelectCommand;
                    //    linqDataSet.RemoteName = "GLModule.cmdRefValUse";
                    //    linqDataSet.PacketRecords = this.CommandPacketRecords;
                    //    linqDataSet.RealDataSet.Tables.Add(this.CommandTable);
                    //}
                    //else
                    //{
                        linqDataSet.RemoteName = this.RemoteName;
                        linqDataSet.PacketRecords = this.PacketRecords;
                        linqDataSet.RealDataSet = this.InnerDataSet;
                    //}
                    bool b = linqDataSet.GetNextPacket();

                    if (b)
                    {
                        //if (!string.IsNullOrEmpty(this.SelectAlias) && !string.IsNullOrEmpty(this.SelectCommand))
                        //{
                        //    this.CommandTable = linqDataSet.RealDataSet.Tables[0];
                        //}
                        //else
                        //{
                            this.InnerDataSet = linqDataSet.RealDataSet;
                        //}
                        this.LastIndex = linqDataSet.LastIndex;
                        linqDataSet.CommandText = this.CommandText;
                        this.Eof = linqDataSet.Eof;

                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
#endif
            }
                
            return false;
        }

        public bool SetWhere(string strWhere)
        {
            return SetWhere(strWhere, null);
        }

        public bool SetWhere(string strWhere, ArrayList param)
        {
            SetWhereArgs args = new SetWhereArgs(strWhere, param);
            OnSetWhereing(args);
            if (args.Cancel)
            {
                return false;
            }
            this.WhereStr = args.WhereStr;
            this.WhereParam = args.WhereParam;
            if (this.RemoteName != null && this.RemoteName.Trim() != "")
            {
                InfoDataSet infoDataSet = new InfoDataSet();
                infoDataSet.DataCompressed = DataCompressed;
                infoDataSet.RemoteName = this.RemoteName;
                infoDataSet.PacketRecords = this.PacketRecords;
                // infoDataSet.LastKeyValues = null;
                infoDataSet.LastIndex = -1;
                infoDataSet.WhereStr = this.WhereStr;
                if (infoDataSet.WhereParam != null)
                    infoDataSet.WhereParam.Clear();
                infoDataSet.WhereParam = this.WhereParam;
                infoDataSet.Active = true;
                this.InnerDataSet = infoDataSet.RealDataSet;
                // this.LastKeyValues = infoDataSet.LastKeyValues;
                this.LastIndex = infoDataSet.LastIndex;
                this.Eof = infoDataSet.Eof;
            }
            else
            {
                this.LastIndex = -1;
                string strSql = CliUtils.InsertWhere(this.SelectCommand, this.WhereStr);
                DataSet ds = new DataSet();
                ds = CliUtils.ExecuteSql("GLModule", "cmdRefValUse", strSql, null, true, CliUtils.fCurrentProject, new object[] { this.CommandPacketRecords, this.LastIndex }, this.WhereParam);
                if (ds != null && ds.Tables.Count != 0 && ds.Tables[0] != null)
                {
                    this.CommandTable = ds.Tables[0];
                }
            }
            if (View != null && View.Count > 0)
            {
                SetPostion(0);
            }
            else
            {
                SetPostion(-1);
            }

            return true;
        }

        public int GetRecordsCount(string strWhere)
        {
            InfoDataSet infoDataSet = new InfoDataSet();
            infoDataSet.RemoteName = this.RemoteName;

            return (infoDataSet.GetRecordsCount(strWhere));
        }

        public int GetRecordsCount()
        {
            return GetRecordsCount("");
        }

        public DataTable GetSchema()
        {
            if (string.IsNullOrEmpty(DataMember))
            {
                throw new EEPException(EEPException.ExceptionType.PropertyNull, this.GetType(), this.ID, "DataMember", null);
            }


            string keyName = "WebDataSets";
            string aspxName = EditionDifference.ActiveDocumentFullName();

            string resourceName = aspxName + @".vi-VN.resx";
            ResXResourceReader reader = new ResXResourceReader(resourceName);
            IDictionaryEnumerator enumerator = reader.GetEnumerator();

            XmlDocument xmlDoc = new XmlDocument();
            while (enumerator.MoveNext())
            {
                if (enumerator.Key.ToString() == keyName)
                {
                    string sXml = (string)enumerator.Value;
                    xmlDoc.LoadXml(sXml);
                    break;
                }
            }
            reader.Close();

            if (xmlDoc == null) return null;

            XmlNode nWDSs = xmlDoc.SelectSingleNode("WebDataSets");
            if (nWDSs == null) return null;

            XmlNode nWDS = nWDSs.SelectSingleNode("WebDataSet[@Name='" + _webDataSetID + "']");
            if (nWDS == null) return null;

            string remoteName = "";
            int packetRecords = 100;
            bool active = false;
            bool serverModify = false;

            XmlNode nRemoteName = nWDS.SelectSingleNode("RemoteName");
            if (nRemoteName != null)
                remoteName = nRemoteName.InnerText;

            XmlNode nPacketRecords = nWDS.SelectSingleNode("PacketRecords");
            if (nPacketRecords != null)
                packetRecords = nPacketRecords.InnerText.Length == 0 ? 100 : Convert.ToInt32(nPacketRecords.InnerText);

            XmlNode nActive = nWDS.SelectSingleNode("Active");
            if (nActive != null)
                active = nActive.InnerText.Length == 0 ? false : Convert.ToBoolean(nActive.InnerText);

            XmlNode nServerModify = nWDS.SelectSingleNode("ServerModify");
            if (nServerModify != null)
                serverModify = nServerModify.InnerText.Length == 0 ? false : Convert.ToBoolean(nServerModify.InnerText);

            WebDataSet wds = new WebDataSet(true);
            wds.RemoteName = remoteName;
            wds.PacketRecords = packetRecords;
            wds.ServerModify = serverModify;
            wds.Active = true;

            DataTable table2 = wds.RealDataSet.Tables[DataMember].Clone();
            table2.Rows.Clear();

            return table2;
        }

        public DataTable GetCommandTable()
        {
            DataTable table = null;
            if (SelectCommand != null && SelectCommand.Length != 0 && SelectAlias != null && SelectAlias.Length != 0)
            {
                string cmd = SelectCommand.Clone().ToString();
                string pro = CliUtils.fCurrentProject;
                if (this.DesignMode)
                {
                    cmd = CliUtils.InsertWhere(cmd, "1=0");
                }
                //modified by lily 运行时不应使用Alias
                DataSet ds;
                if (!DesignMode)
                {
                    ds = CliUtils.ExecuteSql("GLModule", "cmdRefValUse", cmd, null, true, pro, new object[] { this.CommandPacketRecords, this.LastIndex });
                }
                else
                {
                    ds = CliUtils.ExecuteSql("GLModule", "cmdRefValUse", cmd, SelectAlias, true, pro);
                }

                if (ds != null && ds.Tables.Count != 0 && ds.Tables[0] != null)
                {
                    table = ds.Tables[0];
                }
                else
                {
                    table = new DataTable("Table1");
                }
            }

            return table;
        }

        private String Mark(Type type, Object columnValue)
        {
            if (type.Equals(typeof(Char)) || type.Equals(typeof(String)) || type.Equals(typeof(Guid)))
            {
                return _marker.ToString() + columnValue.ToString() + _marker.ToString();
            }
            else if (type.Equals(typeof(DateTime)))
            {
                DateTime t = Convert.ToDateTime(columnValue);
                string s = t.Year.ToString() + "-" + t.Month.ToString() + "-" + t.Day.ToString() + " "
                    + t.Hour.ToString() + ":" + t.Minute.ToString() + ":" + t.Second.ToString();
                return _marker.ToString() + s + _marker.ToString();
            }
            else if (type.Equals(typeof(Boolean)))
            {
                Boolean b = (Boolean)columnValue;
                if (b)
                    return "1";
                else
                    return "0";
            }
            else if (type.Equals(typeof(Byte[])))
            {
                StringBuilder builder = new StringBuilder("0x");   // 16进制、Oracle里的Binary没有测试。
                foreach (Byte b in (Byte[])columnValue)
                {
                    string tmp = Convert.ToString(b, 16);
                    if (tmp.Length < 2)
                        tmp = "0" + tmp;
                    builder.Append(tmp);
                }
                return builder.ToString();
            }
            else
            {
                return columnValue.ToString();
            }
        }

        private Object TransformMarkerInColumnValue(Type type, Object columnValue)
        {
            if (type.Equals(typeof(Char)) || type.Equals(typeof(String)))
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

        public int Insert(IDictionary values)
        {
            if (_dataSourceView == null)
                return 0;
            // DataSourceViewOperationCallback
            _dataSourceView.Insert(values, new DataSourceViewOperationCallback(_dataSourceView.Insert));
            return 1;
        }

        public bool ApplyUpdates()
        {
            WebDataSet dataSet = new WebDataSet();
            dataSet.RemoteName = RemoteName;
            dataSet.ServerModify = ServerModify;

            dataSet.RealDataSet = InnerDataSet;

            //add by ccm do AutoSeq
            List<WebAutoSeq> wAutoSeq = new List<WebAutoSeq>();
            foreach (Control ctrl in this.Page.Form.Controls)
            {

                if (ctrl is WebAutoSeq && ((WebAutoSeq)ctrl).MasterDataSourceID == this.ID && ((WebAutoSeq)ctrl).ReNumber && ((WebAutoSeq)ctrl).Active)
                {
                    wAutoSeq.Add((WebAutoSeq)ctrl);
                }
            }
            if (wAutoSeq.Count > 0)
            {
                foreach (WebAutoSeq was in wAutoSeq)
                {
                    was.ResetDetail();
                }

            }
            //End AutoSeq

            // End Add 2006/08/22
            dataSet.ApplyError += delegate(object sender, ApplyErrorEventArgs e)
            {
                OnApplyError(e);
            };
            bool b = dataSet.ApplyUpdates();
            if (!b)
                return false;
            else
            {
                this.RemoveLock();
                InnerDataSet = dataSet.RealDataSet;
                if (wAutoSeq.Count > 0)
                {
                    foreach (WebAutoSeq was in wAutoSeq)
                    {
                        foreach (Control ctrl in this.Page.Form.Controls)
                        {
                            if (ctrl is WebGridView && ((WebGridView)ctrl).DataSourceID == was.DataSourceID)
                            {
                                ((WebGridView)ctrl).DataBind();
                            }
                        }
                    }
                }
                return true;
            }
        }

        private bool _autorecordlock;
        [Category("Infolight"),
        Description("Specifies data lock to prevent from modifying by two different users")]
        public bool AutoRecordLock
        {
            get
            {
                return _autorecordlock;
            }
            set
            {
                _autorecordlock = value;
            }
        }

        public enum LockMode
        {
            NoneReload,
            ReLoad
        }

        private LockMode _autorecordlockmode;
        [Category("Infolight"),
        Description("Specifies the behaviour before modify data")]
        public LockMode AutoRecordLockMode
        {
            get
            {
                return _autorecordlockmode;
            }
            set
            {
                _autorecordlockmode = value;
            }
        }

        [Browsable(false)]
        [Obsolete("The recommended alternative is LockedRecords", false)]
        public string KeyValues
        {
            get
            {
                return ViewState["KeyValues"] == null ? "" : (string)ViewState["KeyValues"];
            }
            set
            {
                ViewState["KeyValues"] = value;
            }
        }

        [Browsable(false)]
        [Obsolete("The recommended alternative is CommandText", false)]
        private string SQLText
        {
            get
            {
                return ViewState["SQLText"] == null ? "" : (string)ViewState["SQLText"];
            }
            set
            {
                ViewState["SQLText"] = value;
            }
        }

        [Browsable(false)]
        [Obsolete("The recommended alternative is RelationValues", true)]
        public object[] SelectedValues
        {
            get
            {
                object o = ViewState["SelectedValues"];
                return o == null ? null : (object[])o;
            }
            set
            {
                ViewState["SelectedValues"] = value;
            }
        }

        [Browsable(false)]
        [Obsolete("The recommended alternative is DataMember", false)]
        public string CommandName
        {
            get
            {
                return ViewState["CommandName"] == null ? "" : (string)ViewState["CommandName"];
            }
            set
            {
                ViewState["CommandName"] = value;
            }
        }

        internal bool AddLock(string modifytype, object[] value)
        {
            StringBuilder key = new StringBuilder();
            StringBuilder keyvalue = new StringBuilder();
            foreach (DataColumn dc in this.PrimaryKey)
            {
                if (key.Length > 0)
                {
                    key.Append(";");
                }
                key.Append(dc.ColumnName);
            }
            foreach (object obj in value)
            {
                if (keyvalue.Length > 0)
                {
                    keyvalue.Append(';');
                }
                keyvalue.Append(obj.ToString());
            }
            if (LockedRecords.Contains(value))
            {
                return true;
            }
            object[] retval = CliUtils.CallMethod("GLModule", "DoRecordLock"
                , new object[] { CliUtils.fLoginDB, TableName, key.ToString(), keyvalue.ToString(), CliUtils.fLoginUser, modifytype, CommandText, this.AutoRecordLockMode.ToString() });
            if (retval != null && (int)retval[0] == 0)
            {
                if ((int)retval[1] == 0)
                {
                    if (this.AutoRecordLockMode == LockMode.ReLoad)
                    {
                        DataSet dsnew = ((DataSet)retval[2]);
                        if (dsnew.Tables[0].Rows.Count == 0)
                        {
                            string message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "EEPNetServer", "RecordLock", "DataDeleted", true);
                            this.Page.Response.Write("<script>alert('" + message + "')</script>");
                            return false;
                        }
                        DataRow row = this.View.Table.Rows.Find(value);
                        Hashtable keys = new Hashtable();
                        Hashtable values = new Hashtable();
                        Hashtable oldvalues = new Hashtable();
                        foreach (DataColumn dc in this.View.Table.Columns)
                        {
                            keys.Add(dc.ColumnName, dc.ColumnName);
                            oldvalues.Add(dc.ColumnName, row[dc.ColumnName]);
                            if (dsnew.Tables[0].Columns.Contains(dc.ColumnName))
                            {
                                row[dc.ColumnName] = dsnew.Tables[0].Rows[0][dc.ColumnName];
                            }
                            values.Add(dc.ColumnName, row[dc.ColumnName]);
                        }
                        this.Update(keys, values, oldvalues);
                        this.MasterWebDataSource.InnerDataSet.AcceptChanges();
                    }
                    LockedRecords.Add(value);

                    return true;
                }
                else
                {
                    string message = string.Empty;
                    if (retval[2].ToString() == "Updating")
                    {
                        message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "EEPNetServer", "RecordLock", "DataUpdating", true);
                    }
                    else if (retval[2].ToString() == "Deleting")
                    {
                        message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "EEPNetServer", "RecordLock", "DataDeleting", true);
                    }
                    else
                    {
                        message = "Unknown Error";
                    }
                    this.Page.ClientScript.RegisterStartupScript(typeof(string), string.Empty, "<script>alert('" + message + "')</script>");
                    return false;
                }
            }
            return false;
        }

        internal void RemoveLock()
        {
            if (this.LockedRecords.Count > 0)
            {
                StringBuilder key = new StringBuilder();
                foreach (DataColumn dc in this.PrimaryKey)
                {
                    if (key.Length > 0)
                    {
                        key.Append(";");
                    }
                    key.Append(dc.ColumnName);
                }
                CliUtils.CallMethod("GLModule", "DoRecordLock", new object[] { CliUtils.fLoginDB, TableName, key.ToString(), this.LockedRecords, CliUtils.fLoginUser, "Release" });
                this.LockedRecords.Clear();
            }
        }

        public string ToExcel(string TableName)
        {
            return this.ToExcel(TableName, "");
        }

        public string ToExcel(string TableName, string Filter)
        {
            return this.ToExcel(TableName, Filter, true);
        }

        public string ToExcel(string TableName, string Filter, bool Open)
        {
            return this.ToExcel(TableName, Filter, Open, string.Empty);
        }

        public string ToExcel(string TableName, string Filter, bool Open, string Title)
        {
            return this.ToExcel(TableName, Filter, string.Empty, Open, Title);
        }

        public string ToExcel(string TableName, string Filter, string Sort, bool Open, string Title)
        {
            return this.ToExcel(TableName, Filter, Sort, Open, Title, null);
        }

        public string ToExcel(string TableName, string Filter, string Sort, bool Open, string Title, List<string> IgnoreColumns)
        {
            string path = this.Page.MapPath(this.Page.AppRelativeVirtualPath);
            string directory = Path.GetDirectoryName(path);
            string filename = Path.GetFileNameWithoutExtension(path);
            path = string.Format("{0}\\ExcelDoc\\{1}", directory, string.Format("{0}-{1:yyMMddHHmmss}", filename, DateTime.Now));
            path = Path.ChangeExtension(path, "xls");
            string newfilename = Path.GetFileName(path);
            InfoDataSet ids = new InfoDataSet();
            if (this.InnerDataSet != null)
            {
                ids.RealDataSet = this.InnerDataSet;
                ids.RemoteName = this.RemoteName;//DD Use
                ids.PacketRecords = this.PacketRecords;
                ids.LastIndex = this.LastIndex;
            }
            else
            {
                DataTable tb = this.CommandTable.Clone();
                tb.TableName = "CommandTable";
                tb.Merge(this.CommandTable);
                ids.RealDataSet.Tables.Add(tb);
            }
            ids.WhereStr = this.WhereStr;
            ids.WhereParam = this.WhereParam;
            DataSetToExcel dste = new DataSetToExcel();
            dste.Excelpath = path;
            dste.Filter = Filter;
            dste.Sort = Sort;
            dste.DataSet = ids;
            dste.Title = Title;
            dste.IgnoreColumns = IgnoreColumns;
            try
            {
                dste.Export(TableName);
            }
            finally
            {
                this.LastIndex = ids.LastIndex;
            }
            if (Open)
            {
                this.Export(CliUtils.ReplaceFileName(this.Page.AppRelativeVirtualPath, filename + ".aspx", string.Format("ExcelDoc/{0}", newfilename)));
            }
            return path;
        }

        public string ToExcel(int TableIndex)
        {
            return ToExcel(TableIndex, "");
        }

        public string ToExcel(int TableIndex, string Filter)
        {
            return ToExcel(TableIndex, Filter, true);
        }

        public string ToExcel(int TableIndex, string Filter, bool Open)
        {
            return ToExcel(TableIndex, Filter, Open, string.Empty);
        }

        public string ToExcel(int TableIndex, string Filter, bool Open, string Title)
        {
            return ToExcel(TableIndex, Filter, string.Empty, Open, Title);
        }

        public string ToExcel(int TableIndex, string Filter, string Sort, bool Open, string Title)
        {
            return ToExcel(TableIndex, Filter, Sort, Open, Title, null);
        }

        public string ToExcel(int TableIndex, string Filter, string Sort, bool Open, string Title, List<string> IgnoreColumns)
        {
            string path = this.Page.MapPath(this.Page.AppRelativeVirtualPath);
            string directory = Path.GetDirectoryName(path);
            string filename = Path.GetFileNameWithoutExtension(path);
            path = string.Format("{0}\\ExcelDoc\\{1}", directory, string.Format("{0}-{1:yyMMddHHmmss}", filename, DateTime.Now));
            path = Path.ChangeExtension(path, "xls");
            string newfilename = Path.GetFileName(path);

            InfoDataSet ids = new InfoDataSet();
            if (this.InnerDataSet != null)
            {
                ids.RealDataSet = this.InnerDataSet;
                ids.RemoteName = this.RemoteName;//DD Use
                ids.PacketRecords = this.PacketRecords;
                ids.LastIndex = this.LastIndex;
            }
            else
            {
                DataTable tb = this.CommandTable.Clone();
                tb.TableName = "CommandTable";
                tb.Merge(this.CommandTable);
                ids.RealDataSet.Tables.Add(tb);
            }

            ids.WhereStr = this.WhereStr;
            ids.WhereParam = this.WhereParam;
            DataSetToExcel dste = new DataSetToExcel();
            dste.Excelpath = path;
            dste.Filter = Filter;
            dste.Sort = Sort;
            dste.DataSet = ids;
            dste.Title = Title;
            dste.IgnoreColumns = IgnoreColumns;

            try
            {
                dste.Export(TableIndex);
            }
            finally
            {
                this.LastIndex = ids.LastIndex;
            }
            if (Open)
            {
                this.Export(CliUtils.ReplaceFileName(this.Page.AppRelativeVirtualPath, filename + ".aspx", string.Format("ExcelDoc/{0}", newfilename)));
            }
            return path;
        }

        private void Export(string filename)
        {
            string eurl = "../InnerPages/frmExport.aspx?File=" + filename;
            string script = "window.open('" + eurl + "','download','height=150,width=250, scrollbars=yes,resizable=yes,toolbar=no,menubar=no,location=no,status=no')";

#if AjaxTools
            Control panel = this.Parent;
            while (panel != null && panel.GetType() != typeof(UpdatePanel))
            {
                panel = panel.Parent;
            }
            if (panel != null)
            {
                ScriptManager.RegisterStartupScript(panel as UpdatePanel, this.Page.GetType(), "ScriptBlock", script, true);
            }
            else
            {
#endif
            this.Page.ClientScript.RegisterStartupScript(Page.GetType(), "", "<script>" + script + "</script>");
#if AjaxTools
            }
#endif
        }

        public void ToCSV(string TableName)
        {
            string path = this.Page.MapPath(this.Page.Request.Path);
            path = path.Substring(0, path.LastIndexOf('.') + 1);
            path = path + "xls";
            InfoDataSet ids = new InfoDataSet();
            if (this.InnerDataSet != null)
            {
                ids.RealDataSet = this.InnerDataSet;
            }
            else
            {
                DataTable tb = this.CommandTable.Clone();
                tb.TableName = "CommandTable";
                tb.Merge(this.CommandTable);
                ids.RealDataSet.Tables.Add(tb);
            }
            ids.RemoteName = this.RemoteName;//DD Use
            DataSetToExcel dste = new DataSetToExcel();
            dste.Excelpath = path;
            dste.DataSet = ids;

            dste.ExportCSV(TableName);

            this.Export(CliUtils.ReplaceFileName(this.Page.AppRelativeVirtualPath, ".aspx", ".xls"));

        }

        public void ToCSV(int TableIndex)
        {
            string path = this.Page.MapPath(this.Page.Request.Path);
            path = path.Substring(0, path.LastIndexOf('.') + 1);
            path = path + "xls";
            InfoDataSet ids = new InfoDataSet();
            if (this.InnerDataSet != null)
            {
                ids.RealDataSet = this.InnerDataSet;
            }
            else
            {
                DataTable tb = this.CommandTable.Clone();
                tb.TableName = "CommandTable";
                tb.Merge(this.CommandTable);
                ids.RealDataSet.Tables.Add(tb);
            }
            ids.RemoteName = this.RemoteName;//DD Use
            DataSetToExcel dste = new DataSetToExcel();
            dste.Excelpath = path;
            dste.DataSet = ids;
            dste.ExportCSV(TableIndex);

            this.Export(CliUtils.ReplaceFileName(this.Page.AppRelativeVirtualPath, ".aspx", ".xls"));

        }

        public void ReadFromTxt(int tableIndex, string txtFileName)
        {
            DataSet ds = this.InnerDataSet;
            DataTable table = ds.Tables[tableIndex];
            DataFileReader reader = new DataFileReader(table, DataFileReaderType.Txt);
            reader.Read(txtFileName, 0, 0);
            this.InnerDataSet = ds;
        }

        public void ReadFromTxt(string tableName, string txtFileName)
        {
            DataSet ds = this.InnerDataSet;
            DataTable table = ds.Tables[tableName];
            DataFileReader reader = new DataFileReader(table, DataFileReaderType.Txt);
            reader.Read(txtFileName, 0, 0);
            this.InnerDataSet = ds;
        }

        public void ReadFromXls(int tableIndex, string xlsFileName)
        {
            ReadFromXls(tableIndex, xlsFileName, 0, 0);
        }

        public void ReadFromXls(int tableIndex, string xlsFileName, int beginrow, int begincell)
        {
            DataSet ds = this.InnerDataSet;
            DataTable table = ds.Tables[tableIndex];
            DataFileReader reader = new DataFileReader(table, DataFileReaderType.Xls);
            reader.Read(xlsFileName, beginrow, begincell);
            this.InnerDataSet = ds;
        }

        public void ReadFromXls(string tableName, string xlsFileName)
        {
            ReadFromXls(tableName, xlsFileName, 0, 0);
        }

        public void ReadFromXls(string tableName, string xlsFileName, int beginrow, int begincell)
        {
            DataSet ds = this.InnerDataSet;
            DataTable table = ds.Tables[tableName];
            DataFileReader reader = new DataFileReader(table, DataFileReaderType.Xls);
            reader.Read(xlsFileName, beginrow, begincell);
            this.InnerDataSet = ds;
        }


        #region IListDataSource




        public bool ContainsListCollection
        {
            get
            {
                return true;
            }
        }

        public IList GetList()
        {
            //WebDataSet webDataSet = GetWebDataSet();
            //if (webDataSet != null)
            //    return webDataSet.GetList();
            //else
            //    return null;

            return ((IListSource)_dataSource).GetList();
        }

        public string[] GetSelectAlias()
        {
            List<String> aliasList = new List<String>();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(SystemFile.DBFile);

            XmlNodeList nodeList = xmlDoc.FirstChild.FirstChild.ChildNodes;
            foreach (XmlNode n in nodeList)
            {
                aliasList.Add(n.Name);
            }

            return aliasList.ToArray();
        }
        #endregion

        #region DataSourceControl

        protected override ICollection GetViewNames()
        {
            ArrayList a = new ArrayList();

            foreach (DataTable t in InnerDataSet.Tables)
            {
                a.Add(t.TableName);
            }

            return a;
        }

        protected override DataSourceView GetView(string viewName)
        {
            if (CommandTable != null)
            {
                _dataSourceView = new WebDataSourceView(this, CommandTable.TableName);
                return _dataSourceView;
            }
            else
            {
                if (string.IsNullOrEmpty(viewName))
                {
                    viewName = DataMember;
                }
                if (string.IsNullOrEmpty(viewName))
                {
                    throw new EEPException(EEPException.ExceptionType.PropertyNull, this.GetType(), this.ID, "DataMember", null);
                }

                _dataSourceView = new WebDataSourceView(this, viewName);
                return _dataSourceView;
            }
        }

        #endregion

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (Page.Request.QueryString != null)
            {
                bool isQueryBack = false;
                if (Page.Request.QueryString["IsQueryBack"] != null && Page.Request.QueryString["IsQueryBack"] != "")
                {
                    isQueryBack = true;
                }

                if (isQueryBack)
                {
                    string Filter = "";
                    if (Page.Request.QueryString["Filter"] != null && Page.Request.QueryString["DataSourceID"] == this.ID && !Page.IsPostBack)
                    {
                        Filter = Page.Request.QueryString["Filter"];
                        this.WhereStr = Filter;

                        // webDs.LastKeyValues = null;
                        this.LastIndex = -1;
                        this.InnerDataSet.Clear();
                        this.InnerDataSet.ExtendedProperties.Clear();
                        this.Eof = false;
                    }

                    if (!Page.IsPostBack && Page.Request.QueryString["DataSourceID"] == this.ID)
                    {
                        if (this.MasterDataSource != null && this.MasterDataSource != "")
                            return;

                        this.SetWhere(Filter);
                        DataBind();
                    }
                }
            }
        }
    }

    #region DataSouceDDInfomationsCollection class
    public class DataSouceDDInfomationsCollection : InfoOwnerCollection
    {
        public DataSouceDDInfomationsCollection(Object aOwner, Type aItemType)
            : base(aOwner, typeof(DataSouceDDInfomationsItem))
        {
        }

        public new DataSouceDDInfomationsItem this[int index]
        {
            get
            {
                return (DataSouceDDInfomationsItem)InnerList[index];
            }
            set
            {
                if (index > -1 && index < Count)
                {
                    if (value is DataSouceDDInfomationsItem)
                    {
                        //原来的Collection设置为0
                        ((DataSouceDDInfomationsItem)InnerList[index]).Collection = null;
                        InnerList[index] = value;
                        //Collection设置为this
                        ((DataSouceDDInfomationsItem)InnerList[index]).Collection = this;
                    }
                }
            }
        }
    }
    #endregion

    #region DataSouceDDInfomationsItem class
    public class DataSouceDDInfomationsItem : InfoOwnerCollectionItem, IGetValues
    {
        public DataSouceDDInfomationsItem()
        { }

        public override string Name
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

        #region Properties
        private String _ColumnName;
        [NotifyParentProperty(true),
        Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
        public String ColumnName
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

        private String _RealTableName;
        [NotifyParentProperty(true)]
        public String RealTableName
        {
            get
            {
                return _RealTableName;
            }
            set
            {
                _RealTableName = value;
            }
        }

        private String _RealColumnName;
        [NotifyParentProperty(true),
        Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
        public String RealColumnName
        {
            get
            {
                return _RealColumnName;
            }
            set
            {
                _RealColumnName = value;
            }
        }
        #endregion

        #region IGetValues
        public string[] GetValues(string sKind)
        {
            string[] retList = null;
            List<string> values = new List<string>();
            if (sKind.ToLower().Equals("columnname"))
            {
                if (this.Owner is WebDataSource)
                {
                    WebDataSource wds = (WebDataSource)this.Owner;
                    if (wds.DesignDataSet == null)
                    {
                        WebDataSet ds = WebDataSet.CreateWebDataSet(wds.WebDataSetID);
                        if (ds != null)
                        {
                            wds.DesignDataSet = ds.RealDataSet;
                        }
                    }
                    if (wds.DesignDataSet != null)
                    {
                        foreach (DataColumn column in wds.DesignDataSet.Tables[wds.DataMember].Columns)
                        {
                            values.Add(column.ColumnName);
                        }
                    }
                }
            }
            if (values.Count > 0)
            {
                retList = values.ToArray();
            }
            return retList;
        }
        #endregion
    }
    #endregion

    public class SelectAliasEditor : System.Drawing.Design.UITypeEditor
    {
        public SelectAliasEditor()
        {
        }

        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
        public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        // Displays the UI for value selection.
        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            // Uses the IWindowsFormsEditorService to display a
            // drop-down UI in the Properties window.
            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

            if (context != null)
            {
                object obj = context.Instance;
                WebDataSource source;
                if (obj is WebDataSource)
                {
                    source = (WebDataSource)obj;
                }
                else
                {
                    source = (WebDataSource)((WebDataSourceActionList)context.Instance).Component;
                }

                if (source == null)
                {
                    return null;
                }

                IGetSelectAlias aItem = (IGetSelectAlias)source;
                if (edSvc != null)
                {
                    // Get Alias in db.xml.
                    StringListSelector mySelector = new StringListSelector(edSvc, aItem.GetSelectAlias());
                    string strValue = (string)value;
                    if (mySelector.Execute(ref strValue)) value = strValue;

                    return strValue;
                }
                else
                { return null; }
            }
            else
            { return null; }
        }
    }

    public class SelectCommandEditor : System.Drawing.Design.UITypeEditor
    {
        public SelectCommandEditor()
        {
        }

        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
        public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        // Displays the UI for value selection.
        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            // Uses the IWindowsFormsEditorService to display a
            // drop-down UI in the Properties window.
            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            object obj = context.Instance;
            IUseSelectCommand useSelCmdObj = null;

            if (obj is WebDataSource)
            {
                useSelCmdObj = (WebDataSource)obj;
            }
            else if (obj is InfoRefVal)
            {
                useSelCmdObj = (InfoRefVal)obj;
            }
            else if (obj is InfoComboBox)
            {
                useSelCmdObj = (InfoComboBox)obj;
            }
            else
            {
                useSelCmdObj = (WebDataSource)((WebDataSourceActionList)context.Instance).Component;
            }

            if (useSelCmdObj == null)
            {
                return null;
            }

            string selectAlias = useSelCmdObj.SelectAlias;
            if (selectAlias == null || selectAlias.Length == 0)
            {
                return null;
            }

            DbConnectionSet.DbConnection db = DbConnectionSet.GetDbConn(selectAlias);
            if (db != null)
            {
                IDbConnection conn = db.CreateConnection();
                if (conn == null)
                    return null;

                CommandTextOptionDialog dialog = new CommandTextOptionDialog(conn, useSelCmdObj.SelectCommand);
                edSvc.ShowDialog(dialog);
                String commandText = dialog.CommandText;
                dialog.Dispose();

                return dialog.CommandText;
            }
            return null;

            //IGetValues aItem = (IGetValues)context.Instance;
            //if (edSvc != null)
            //{

            //    StringListSelector mySelector = new StringListSelector(edSvc, new String[] { value.ToString(), "<New CommandText...>" } /*aItem.GetValues(context.PropertyDescriptor.Name)*/);
            //    string strValue = (string)value;
            //    if (mySelector.Execute(ref strValue)) value = strValue;
            //}
            //return value;
        }
    }

    //----------------------------------------------------------------------------------

    public class WebDataSetEditor : UITypeEditor
    {
        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand)]
        public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand)]
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService service = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (service != null)
            {
                StringListSelector selector = new StringListSelector(service, WebDataSet.GetAvailableDataSetID());
                string strValue = (string)value;
                if (selector.Execute(ref strValue)) value = strValue;
            }
            return value;
        }
    }

    public class LinqDataSetEditor : UITypeEditor
    {
        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand)]
        public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand)]
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            List<string> names = new List<string>();

            string aspxName = EditionDifference.ActiveDocumentFullName();
            string keyName = "LinqDataSets";
            string resourceName = aspxName + @".vi-VN.resx";
            // ResXResourceReader reader = new ResXResourceReader(s + aspxPath + @"\" + resourceName);
            ResXResourceReader reader = new ResXResourceReader(resourceName);
            IDictionaryEnumerator enumerator = reader.GetEnumerator();

            XmlDocument xmlDoc = new XmlDocument();
            while (enumerator.MoveNext())
            {
                if (enumerator.Key.ToString() == keyName)
                {
                    string sXml = (string)enumerator.Value;
                    xmlDoc.LoadXml(sXml);
                    break;
                }
            }
            reader.Close();

            if (xmlDoc == null) return null;

            XmlNode nLinqDSs = xmlDoc.SelectSingleNode("LinqDataSets");
            if (nLinqDSs == null) return null;

            XmlNodeList nLinqDSList = nLinqDSs.SelectNodes("LinqDataSet");
            foreach (XmlNode n in nLinqDSList)
            {
                names.Add(n.Attributes["Name"].Value);
            }

            IWindowsFormsEditorService service = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (service != null)
            {
                StringListSelector selector = new StringListSelector(service, names.ToArray());
                string strValue = (string)value;
                if (selector.Execute(ref strValue)) value = strValue;
            }
            return value;
        }
    }

    //----------------------------------------------------------------------------------

    public class MasterDataSourceEditor : UITypeEditor
    {
        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand)]
        public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand)]
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            List<string> names = new List<string>();

            ControlCollection controls = (((WebDataSource)context.Instance).Parent).Controls;
            foreach (System.Web.UI.Control control in controls)
            {
                if (control.GetType() != typeof(WebDataSource))
                    continue;

                string name = control.Site.Name;
                if (name.ToLower() != ((System.Web.UI.Control)context.Instance).Site.Name.ToLower())
                    names.Add(control.Site.Name);
            }

            IWindowsFormsEditorService service = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (service != null)
            {
                StringListSelector selector = new StringListSelector(service, names.ToArray());
                string strValue = (string)value;
                if (selector.Execute(ref strValue)) value = strValue;
            }
            return value;
        }
    }

    //----------------------------------------------------------------------------------

    public class DataMemberEditor : UITypeEditor
    {
        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand)]
        public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand)]
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            WebDataSource source = null;
            object o = context.Instance;
            if (o is WebDataSource)
            {
                source = (WebDataSource)context.Instance;
            }
            else if (o is WebValidate)
            {
                WebValidate validate = (WebValidate)context.Instance;
                source = (WebDataSource)(validate.GetObjByID(validate.DataSourceID));
            }
            else if (o is WebDefault)
            {
                WebDefault def = (WebDefault)context.Instance;
                source = (WebDataSource)(def.GetObjByID(def.DataSourceID));
            }
            else
            {
                source = (WebDataSource)((WebDataSourceActionList)context.Instance).Component;
            }

            // --------------------------------------------------------------------------------
            // read xml.
            if (source.DesignDataSet == null)
            {
                WebDataSet wds = WebDataSet.CreateWebDataSet(source.WebDataSetID);
                if (wds != null)
                {
                    source.DesignDataSet = wds.RealDataSet;
                }
            }

            List<string> names = new List<string>();
            if (source.DesignDataSet != null)
            {
                DataTableCollection tables = source.DesignDataSet.Tables;
                foreach (DataTable t in tables)
                    names.Add(t.TableName);
            }

            IWindowsFormsEditorService service = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (service != null)
            {
                StringListSelector selector = new StringListSelector(service, names.ToArray());
                string strValue = (string)value;
                if (selector.Execute(ref strValue)) value = strValue;
            }
            return value;
        }
    }

    //----------------------------------------------------------------------------------

    public class WebDataSourceDesigner : DataSourceDesigner
    {
        private bool _isRefresh = false;
        private DesignerActionListCollection _actionLists;

        public WebDataSourceDesigner()
        {
            //DesignerVerb RereshDDInfomationVerb = new DesignerVerb("Reresh DDInfomation", new EventHandler(OnRereshDDInfomation));
            //this.Verbs.Add(RereshDDInfomationVerb);
        }

        public void OnRereshDDInfomation(object sender, EventArgs e)
        {
            WebDataSource wds = (WebDataSource)this.Component;
            wds.DDInfomations.Clear();
            DataTable dt = this.GetSchemaTable();
            String strTableName = GetTableName();

            String keyName = "WebDataSources";
            String aspxName = EditionDifference.ActiveDocumentFullName();
            String resourceName = aspxName + @".vi-VN.resx";
            ResXResourceReader resourceReader = new ResXResourceReader(resourceName);
            ResXResourceWriter resourceWriter = new ResXResourceWriter(resourceName);

            IDictionaryEnumerator enumerator = resourceReader.GetEnumerator();

            XmlDocument xmlDoc = new XmlDocument();
            while (enumerator.MoveNext())
            {
                if (enumerator.Key.ToString() == keyName)
                {
                    String sXml = (String)enumerator.Value;
                    xmlDoc.LoadXml(sXml);
                }
                else
                {
                    String sXml = (String)enumerator.Value;
                    XmlDocument xmlTemp = new XmlDocument();
                    xmlTemp.LoadXml(sXml);
                    resourceWriter.AddResource(enumerator.Key.ToString(), xmlTemp.InnerXml);
                }
            }
            resourceReader.Close();

            // ---------------------------------------------------------------------
            XmlNode nWDSs = xmlDoc.SelectSingleNode("WebDataSources");
            if (nWDSs == null)
            {
                nWDSs = xmlDoc.CreateElement("WebDataSources");
                xmlDoc.AppendChild(nWDSs);
            }

            // ---------------------------------------------------------------------
            // 删除已经不存在的WebDataSet。
            RemoveNotExisted(nWDSs);

            // ---------------------------------------------------------------------
            string name = this.Component.Site.Name;
            XmlNode nWDS = nWDSs.SelectSingleNode("WebDataSources[@Name='" + name + "']");
            if (nWDS != null)
                nWDSs.RemoveChild(nWDS);

            nWDS = CreateWDSNode(xmlDoc, dt, strTableName);
            nWDSs.AppendChild(nWDS);

            // ---------------------------------------------------------------------
            resourceWriter.AddResource(keyName, xmlDoc.InnerXml);
            resourceWriter.Close();


            // 保存当前的文档
            //EnvDTE.Document doc = EditionDifference.ActiveDocument();
            //doc.Save(doc.FullName);
            //doc.Saved = true;
        }

        private XmlNode CreateWDSNode(XmlDocument xmlDoc, DataTable dt, String strTableName)
        {
            XmlElement nWDSNode = xmlDoc.CreateElement("WebDataSources");

            XmlAttribute aWBSName = xmlDoc.CreateAttribute("Name");
            aWBSName.Value = this.Component.Site.Name;
            nWDSNode.Attributes.Append(aWBSName);

            // ---------------------------------------------------------------
            WebDataSource wds = (WebDataSource)this.Component;

            for (int i = 0; i < dt.Columns.Count; i++)
            {
                DataSouceDDInfomationsItem item = new DataSouceDDInfomationsItem();
                item.ColumnName = dt.Columns[i].Caption;
                item.RealColumnName = dt.Columns[i].Caption;
                item.RealTableName = strTableName;

                XmlNode node = xmlDoc.CreateElement(item.ColumnName);
                node.InnerText = item.ColumnName + ";" + item.RealColumnName + ";" + item.RealTableName;
                nWDSNode.AppendChild(node);
            }
            return nWDSNode;
        }

        private void RemoveNotExisted(XmlNode node)
        {
            List<string> lists = new List<string>();
            WebDataSource webDs = (WebDataSource)this.Component;
            ComponentCollection comps = webDs.Site.Container.Components;
            foreach (object comp in comps)
            {
                if (comp is WebDataSource)
                {
                    lists.Add((comp as WebDataSource).Site.Name);
                }
            }

            foreach (XmlNode nod in node.ChildNodes)
            {
                string name = nod.Attributes["Name"].Value;
                if (lists.IndexOf(name) < 0)
                {
                    node.RemoveChild(nod);
                }
            }
        }

        public override DesignerActionListCollection ActionLists
        {
            get
            {
                _actionLists = base.ActionLists;

                if (_actionLists != null)
                    _actionLists.Add(new WebDataSourceActionList(this.Component));

                return _actionLists;
            }
        }

        public override bool CanRefreshSchema
        {
            get
            {
                return true;
            }
        }

        private DataTable GetSchemaTable()
        {
            WebDataSource comp = (WebDataSource)this.Component;
            if (comp == null)
                return null;

            if (comp.SelectCommand != null && comp.SelectCommand.Length != 0 && comp.SelectAlias != null && comp.SelectAlias.Length != 0)
            {
                string pro = CliUtils.fCurrentProject;
                if (pro == null || pro.Length == 0)
                {
                    pro = EditionDifference.ActiveSolutionName();
                }

                string CommandText = CliUtils.InsertWhere(comp.SelectCommand, "1=0");

                DataSet ds = CliUtils.ExecuteSql("GLModule", "cmdRefValUse", CommandText, comp.SelectAlias, true, pro);

                if (ds != null && ds.Tables.Count != 0)
                    return ds.Tables[0];
                else
                    return (new DataTable("Table1"));
            }
            else
            {
                string keyName = string.Empty;
                if (!string.IsNullOrEmpty(comp.WebDataSetID))
                {
                    keyName = "WebDataSets";
                }
#if VS90
                else if (!string.IsNullOrEmpty(comp.LinqDataSetID))
                {
                    keyName = "LinqDataSets"; 
                }
#endif

                CultureInfo culture = new CultureInfo("vi-VN");
                string aspxName = EditionDifference.ActiveDocumentFullName();

                XmlDocument xmlDoc = new XmlDocument();
                string resourceName = aspxName + @".vi-VN.resx";
                // ResXResourceReader reader = new ResXResourceReader(s + resourceName);
                ResXResourceReader reader = new ResXResourceReader(resourceName);
                IDictionaryEnumerator enumerator = reader.GetEnumerator();

                while (enumerator.MoveNext())
                {
                    if (enumerator.Key.ToString() == keyName)
                    {
                        string sXml = (string)enumerator.Value;
                        xmlDoc.LoadXml(sXml);
                        break;
                    }
                }
                reader.Close();

                if (xmlDoc == null) return null;

                XmlNode nWDSs = null;
                XmlNode nWDS = null;

                if (!string.IsNullOrEmpty(comp.WebDataSetID))
                {
                    nWDSs =  xmlDoc.SelectSingleNode("WebDataSets");
                    if (nWDSs == null) return null;
                    nWDS = nWDSs.SelectSingleNode("WebDataSet[@Name='" + comp.WebDataSetID + "']");
                    if (nWDS == null) return null;
                } 
#if VS90
                else if (!string.IsNullOrEmpty(comp.LinqDataSetID))
                {
                    nWDSs = xmlDoc.SelectSingleNode("LinqDataSets");
                    if (nWDSs == null) return null;
                    nWDS = nWDSs.SelectSingleNode("LinqDataSet[@Name='" + comp.LinqDataSetID + "']");
                    if (nWDS == null) return null;
                } 
#endif
                string remoteName = "";
                int packetRecords = 100;
                bool active = false;
                bool serverModify = false;
                bool alwaysClose = false;

                XmlNode nRemoteName = nWDS.SelectSingleNode("RemoteName");
                if (nRemoteName != null)
                    remoteName = nRemoteName.InnerText;

                XmlNode nPacketRecords = nWDS.SelectSingleNode("PacketRecords");
                if (nPacketRecords != null)
                    packetRecords = nPacketRecords.InnerText.Length == 0 ? 100 : Convert.ToInt32(nPacketRecords.InnerText);

                XmlNode nActive = nWDS.SelectSingleNode("Active");
                if (nActive != null)
                    active = nActive.InnerText.Length == 0 ? false : Convert.ToBoolean(nActive.InnerText);

                XmlNode nServerModify = nWDS.SelectSingleNode("ServerModify");
                if (nServerModify != null)
                    serverModify = nServerModify.InnerText.Length == 0 ? false : Convert.ToBoolean(nServerModify.InnerText);

                XmlNode nAlwaysClose = nWDS.SelectSingleNode("AlwaysClose");
                if (nAlwaysClose != null)
                    alwaysClose = nAlwaysClose.InnerText.Length == 0 ? false : Convert.ToBoolean(nAlwaysClose.InnerText);


                DataTable tableView = null;
                if(!string.IsNullOrEmpty(comp.WebDataSetID))
                {
                    WebDataSet dataSet = new WebDataSet(true);
                    dataSet.AlwaysClose = alwaysClose;
                    dataSet.RemoteName = remoteName;
                    dataSet.PacketRecords = packetRecords;
                    dataSet.ServerModify = serverModify;
                    dataSet.WhereStr = "1=0";
                    dataSet.Active = true;

                    if (dataSet == null)
                        return null;

                    string viewName = comp.DataMember;
                    if (viewName == null || viewName.Length == 0)
                        return null;

                    tableView = dataSet.RealDataSet.Tables[viewName];
                }
#if VS90
                else if(!string.IsNullOrEmpty(comp.LinqDataSetID))
                {
                    LinqDataSet dataSet = new LinqDataSet(true);
                    dataSet.AlwaysClose = alwaysClose;
                    dataSet.RemoteName = remoteName;
                    dataSet.PacketRecords = packetRecords;
                    dataSet.ServerModify = serverModify;
                    dataSet.Active = true;

                    if (dataSet == null)
                        return null;

                    string viewName = comp.DataMember;
                    if (viewName == null || viewName.Length == 0)
                        return null;

                    tableView = dataSet.RealDataSet.Tables[viewName];
                }
#endif

                return tableView;
            }
        }

        public override DesignerDataSourceView GetView(string viewName)
        {
            if (_isRefresh)
                return new DesignerWebDataSourceView(this, viewName);
            else
                return base.GetView(viewName);
        }

        public override void RefreshSchema(bool preferSilent)
        {
            this.SchemaTable = GetSchemaTable();
            _isRefresh = true;
            this.OnSchemaRefreshed(EventArgs.Empty);
        }

        private DataTable _schemaTable;

        public DataTable SchemaTable
        {
            get { return _schemaTable; }
            set { _schemaTable = value; }
        }

        private String GetTableName()
        {
            string strModuleName = "", strTableName = "", tabName = "";
            object obj = this.Component;
            if (obj == null) return null;
            string sCurProject = EditionDifference.ActiveSolutionName();
            if (obj is WebDataSource)
            {
                WebDataSource wds = (WebDataSource)obj;
                if (wds.SelectAlias != null && wds.SelectAlias != "" && wds.SelectCommand != null && wds.SelectCommand != "")
                {
                    strModuleName = "GLModule";
                    strTableName = "cmdDDUse";
                    tabName = CliUtils.GetTableName(wds.SelectCommand, true);
                    return tabName;
                }
                else
                {
                    strModuleName = WebDataSet.GetRemoteName(wds.WebDataSetID);
                    if (strModuleName.Length > 0)
                    {
                        strModuleName = strModuleName.Substring(0, strModuleName.IndexOf('.'));
                        strTableName = wds.DataMember;

                        if (strTableName == null || strTableName.Length == 0)
                        {
                            System.Windows.Forms.MessageBox.Show((this.Component as WebDataSource).ID + "'s DataMember property is null.", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                            return null;
                        }
                        tabName = CliUtils.GetTableName(strModuleName, strTableName, sCurProject);
                    }
                    return tabName;
                }
            }
            return "";
        }
    }

    //----------------------------------------------------------------------------------

    public class WebDataSourceActionList : DesignerActionList
    {
        private WebDataSource webDataSource;

        public WebDataSourceActionList(IComponent component)
            : base(component)
        {
            this.webDataSource = component as WebDataSource;
        }

        [Editor(typeof(WebDataSetEditor), typeof(UITypeEditor))]
        public string WebDataSetID
        {
            get
            { return webDataSource.WebDataSetID; }
            set
            {
                TypeDescriptor.GetProperties(typeof(WebDataSource))["WebDataSetID"]
                  .SetValue(webDataSource, value);
            }
        }

        [Editor(typeof(DataMemberEditor), typeof(UITypeEditor))]
        public string DataMember
        {
            get
            { return webDataSource.DataMember; }
            set
            {
                TypeDescriptor.GetProperties(typeof(WebDataSource))["DataMember"]
                  .SetValue(webDataSource, value);
            }
        }

#if VS90
        [Editor(typeof(LinqDataSetEditor), typeof(UITypeEditor))]
        public string LinqDataSetID
        {
            get
            { return webDataSource.LinqDataSetID; }
            set
            {
                TypeDescriptor.GetProperties(typeof(WebDataSource))["LinqDataSetID"]
                  .SetValue(webDataSource, value);
            }
        }
#endif

        [Editor(typeof(SelectCommandEditor), typeof(UITypeEditor))]
        public string SelectCommand
        {
            get
            { return webDataSource.SelectCommand; }
            set
            {
                TypeDescriptor.GetProperties(typeof(WebDataSource))["SelectCommand"]
                  .SetValue(webDataSource, value);
            }
        }

        [Editor(typeof(SelectAliasEditor), typeof(UITypeEditor))]
        public string SelectAlias
        {
            get
            { return webDataSource.SelectAlias; }
            set
            {
                TypeDescriptor.GetProperties(typeof(WebDataSource))["SelectAlias"]
                  .SetValue(webDataSource, value);
            }
        }

        public override DesignerActionItemCollection GetSortedActionItems()
        {
            DesignerActionItemCollection items = new DesignerActionItemCollection();

            items.Add(new DesignerActionHeaderItem("Use WebDataSet", "UseWebDataSet"));
            items.Add(new DesignerActionPropertyItem("WebDataSetID", "WebDataSetID", "UseWebDataSet", "Choose WebDataSetID."));
            items.Add(new DesignerActionPropertyItem("DataMember", "DataMember", "UseWebDataSet", "Choose DataMember."));
#if VS90
            items.Add(new DesignerActionHeaderItem("Use LinqDataSet", "UseLinqDataSet"));
            items.Add(new DesignerActionPropertyItem("LinqDataSetID", "LinqDataSetID", "UseLinqDataSet", "Choose LinqDataSetID."));
            items.Add(new DesignerActionPropertyItem("DataMember", "DataMember", "UseLinqDataSet", "Choose DataMember."));
#endif

            items.Add(new DesignerActionHeaderItem("Use SelectCommand", "UseSelectCommand"));
            items.Add(new DesignerActionPropertyItem("SelectCommand", "SelectCommand", "UseSelectCommand", "SelectCommand"));
            items.Add(new DesignerActionPropertyItem("SelectAlias", "SelectAlias", "UseSelectCommand", "SelectAlias"));

            return items;
        }
    }

    //----------------------------------------------------------------------------------

    public class WebDataSourceView : DataSourceView
    {
        private string _viewName;
        private IDataSource _owner;
        //private DataTable _table;
        private Char _marker = '\'';
        private String _quotePrefix = "[";
        private String _quoteSuffix = "]";

        public WebDataSourceView(IDataSource owner, string viewName)
            : base(owner, viewName)
        {
            _owner = owner;
            _viewName = viewName;
        }

        public Char Marker
        {
            get { return _marker; }
            set { _marker = value; }
        }

        public String QuotePrefix
        {
            get { return _quotePrefix; }
            set { _quotePrefix = value; }
        }

        public String QuoteSuffix
        {
            get { return _quoteSuffix; }
            set { _quoteSuffix = value; }
        }

        private String Quote(String table_or_column)
        {
            if (_quotePrefix == null || _quoteSuffix == null)
                return table_or_column;
            return _quotePrefix + table_or_column + _quoteSuffix;
        }

        public override bool CanSort
        {
            get
            {
                return true;
            }
        }

        protected override IEnumerable ExecuteSelect(DataSourceSelectArguments selectArgs)
        {
            if (CanSort)
            {
                selectArgs.AddSupportedCapabilities(DataSourceCapabilities.Sort);
            }
            DataView view = View;
            if (view != null && CanSort && !string.IsNullOrEmpty(selectArgs.SortExpression))
            {
                view.Sort = selectArgs.SortExpression;
            }
            return view;
        }

        internal DataView View
        {
            get 
            {
                WebDataSource dataSource = (WebDataSource)_owner;
                if (dataSource == null)
                {
                    return null;
                }
                else if (!string.IsNullOrEmpty(dataSource.SelectAlias) && !string.IsNullOrEmpty(dataSource.SelectCommand))
                {
                    DataTable table = dataSource.CommandTable;
                    AddEmptyRow(table, true);
                    return dataSource.CommandTable.DefaultView;
                }
                else if (string.IsNullOrEmpty(_viewName))
                {
                    throw new EEPException(EEPException.ExceptionType.PropertyNull, dataSource.GetType(), dataSource.ID, "DataMember", null);
                }
                else if (string.IsNullOrEmpty(dataSource.MasterDataSource))
                {
                    if (dataSource.InnerDataSet == null)
                    {
                        throw new EEPException(EEPException.ExceptionType.ControlNotInitial, dataSource.GetType(), dataSource.ID, "WebDataSource", dataSource.ID);
                    }
                    DataTable table = dataSource.InnerDataSet.Tables[_viewName];
                    if (table == null)
                    {
                        throw new EEPException(EEPException.ExceptionType.PropertyInvalid, dataSource.GetType(), dataSource.ID, "DataMember", _viewName);
                    }
                    AddEmptyRow(table, true);
                    return table.DefaultView;
                }
                else
                {
                    WebDataSource master = dataSource.MasterWebDataSource;
                    if (master.InnerDataSet == null)
                    {
                        throw new EEPException(EEPException.ExceptionType.ControlNotInitial, dataSource.GetType(), dataSource.ID, "WebDataSource", null);
                    }
                    WebDataSource parent = dataSource.ParentWebDataSource;
                    DataTable table = master.InnerDataSet.Tables[_viewName];
                    if (table == null)
                    {
                        throw new EEPException(EEPException.ExceptionType.PropertyInvalid, dataSource.GetType(), dataSource.ID, "DataMember", _viewName);
                    }
                    if (table.ParentRelations.Count == 0 || table.ParentRelations[0].ParentTable.TableName != parent.DataMember)
                    {
                        throw new EEPException(EEPException.ExceptionType.PropertyInvalid, dataSource.GetType(), dataSource.ID, "MasterDataSource", dataSource.MasterDataSource);
                    }
                    else
                    {
                        DataRow parentRow = parent.CurrentRow;
                        DataRelation parentRelation = table.ParentRelations[0];
                        if (parentRow != null && parentRow.Table != table.ParentRelations[0].ParentTable)//找到DataTable中实际的那行
                        {
                            if (parentRow.Table.PrimaryKey.Length > 0)
                            {
                                object[] value = new object[parentRow.Table.PrimaryKey.Length];
                                for (int i = 0; i < parentRow.Table.PrimaryKey.Length; i++)
                                {
                                    value[i] = parentRow[parentRow.Table.PrimaryKey[i].ColumnName];
                                }
                                parentRow = table.ParentRelations[0].ParentTable.Rows.Find(value);
                            }
                            else
                            {
                                throw new EEPException(EEPException.ExceptionType.MethodNotSupported, dataSource.GetType(), dataSource.ID, "get_View", null);
                            }
                        }
                        table = table.Clone();
                        if (parentRow != null)//如果parent有选中某一行
                        {
                            DataRow[] childRows = parentRow.GetChildRows(parentRelation);
                            for (int i = 0; i < childRows.Length; i++)
                            {
                                table.Rows.Add(childRows[i].ItemArray);
                            }
                            AddEmptyRow(table, false);
                        }
                        return table.DefaultView;
                    }
                }
            }
        }

        private void AddEmptyRow(DataTable table, bool master)
        {
            DataTable tableDelete = table.GetChanges(DataRowState.Deleted);
            int deleteCount = tableDelete == null ? 0 : tableDelete.Rows.Count;
            if (table.ExtendedProperties.Contains("Empty") && table.ExtendedProperties["Empty"].Equals("True"))
            {
                if (table.Rows.Count > deleteCount + 1)
                {
                    table.Rows.RemoveAt(0);
                    table.ExtendedProperties["Empty"] = "False";
                }
            }
            else
            {
                if (table.Rows.Count == deleteCount)
                {
                    DataRow rowEmpty = table.NewRow();
                    if (master)
                    {
                        foreach (DataColumn col in table.PrimaryKey)
                        {
                            Type type = col.DataType;
                            if (type == typeof(string))
                            {
                                rowEmpty[col] = string.Empty;
                            }

                            else if (type == typeof(DateTime))
                            {
                                rowEmpty[col] = DateTime.MaxValue;
                            }
                            else if (type == typeof(Guid))
                            {
                                rowEmpty[col] = Guid.Empty;
                            }
                            else if (type == typeof(uint) || type == typeof(UInt16) || type == typeof(UInt32) || type == typeof(UInt64)
                            || type == typeof(int) || type == typeof(Int16) || type == typeof(Int32) || type == typeof(Int64)
                            || type == typeof(Single) || type == typeof(float) || type == typeof(double) || type == typeof(decimal)
                            || type == typeof(sbyte) || type == typeof(byte) || type == typeof(bool))
                            {
                                rowEmpty[col] = 0;
                            }
                            else
                            {
                                rowEmpty[col] = string.Empty;
                            }
                        }
                    }
                    else
                    {
                        table.PrimaryKey = new DataColumn[] { };
                        foreach (DataColumn col in table.Columns)
                        {
                            col.AllowDBNull = true;
                        }
                    }
                    
                    foreach (DataColumn col in table.Columns)
                    {
                        if (col.DataType == typeof(bool))
                        {
                            rowEmpty[col] = false;
                        }
                    }
                    table.Rows.InsertAt(rowEmpty, 0);
                    rowEmpty.AcceptChanges();
                    table.ExtendedProperties["Empty"] = "True";
                }
            }
        }

        public override bool CanDelete
        {
            get
            {
                if (((WebDataSource)_owner).CommandTable != null)
                    return false;
                else
                    return true;
            }
        }

        protected override int ExecuteDelete(IDictionary keys, IDictionary values)
        {
            WebDataSource dataSource = (WebDataSource)_owner;
            if (dataSource == null)
            {
                return 0;
            }
            WebDataSource master = dataSource.MasterWebDataSource;

            if (!string.IsNullOrEmpty(dataSource.SelectAlias) && !string.IsNullOrEmpty(dataSource.SelectCommand))
            {
                throw new EEPException(EEPException.ExceptionType.MethodNotSupported, dataSource.GetType(), dataSource.ID, "ExecuteDelete", null);
            }
            else if (string.IsNullOrEmpty(_viewName))
            {
                throw new EEPException(EEPException.ExceptionType.PropertyNull, dataSource.GetType(), dataSource.ID, "DataMember", null);
            }
            else if (master.InnerDataSet == null)
            {
                throw new EEPException(EEPException.ExceptionType.ControlNotInitial, dataSource.GetType(), dataSource.ID, "WebDataSource", null);
            }
            else
            {
                DataTable table = master.InnerDataSet.Tables[_viewName];
                if (table == null)
                {
                    throw new EEPException(EEPException.ExceptionType.PropertyInvalid, dataSource.GetType(), dataSource.ID, "DataMember", _viewName);
                }

                DataRow rowDelete = null;

                WebDataSourceDeletingEventArgs eventArgs = new WebDataSourceDeletingEventArgs(keys, values);
                master.OnDeleting(eventArgs);

                if (table.PrimaryKey.Length > 0)
                {
                    object[] keysDelete = new object[table.PrimaryKey.Length];
                    for (int i = 0; i < table.PrimaryKey.Length; i++)
                    {
                        string columnName = table.PrimaryKey[i].ColumnName;
                        if (values.Contains(columnName))
                        {
                            keysDelete[i] = values[columnName];
                        }
                        else if (dataSource.RelationValues != null && dataSource.RelationValues.Contains(columnName))
                        {
                            keysDelete[i] = dataSource.RelationValues[columnName];
                        }
                        else
                        {
                            throw new EEPException(EEPException.ExceptionType.ColumnValueNotFound, dataSource.GetType(), dataSource.ID, columnName, null);
                        }
                    }
                    rowDelete = table.Rows.Find(keysDelete);
                }
                else
                {
                    #region All cells on row
                    string where = string.Empty;
                    IDictionaryEnumerator vs = values.GetEnumerator();
                    while (vs.MoveNext())
                    {
                        string name = vs.Key.ToString();
                        object value = vs.Value;
                        Type type = table.Columns[name].DataType;

                        if (where.Length > 0)
                            where += " and ";

                        if (value == null || value == DBNull.Value || value.ToString().Length == 0)
                            where += Quote(name) + " is null";
                        else
                            where += Quote(name) + "=" + Mark(type, TransformMarkerInColumnValue(type, value));
                    }
                    DataRow[] rowsDelete = table.Select(where);
                    if (rowsDelete.Length != 1)
                    {
                        return 0;
                    }
                    rowDelete = rowsDelete[0];
                    #endregion
                }
                if (rowDelete != null)
                {
                    rowDelete.Delete();
                }
                if (dataSource.AutoApply)
                {
                    if (string.IsNullOrEmpty(dataSource.MasterDataSource))
                    {
                        dataSource.ApplyUpdates();
                    }
                }
                return 1;
            }
        }

        public override bool CanInsert
        {
            get
            {
                if (((WebDataSource)_owner).CommandTable != null)
                    return false;
                else
                    return true;
            }
        }

        public bool Insert(int i, Exception e)
        {
            if (e == null)
            {
                return true;
            }
            else
            {
                throw e;
            }
        }

        protected override int ExecuteInsert(IDictionary values)
        {
            WebDataSource dataSource = (WebDataSource)_owner;
            if (dataSource == null)
            {
                return 0;
            }
            WebDataSource master = dataSource.MasterWebDataSource;

            if (!string.IsNullOrEmpty(dataSource.SelectAlias) && !string.IsNullOrEmpty(dataSource.SelectCommand))
            {
                throw new EEPException(EEPException.ExceptionType.MethodNotSupported, dataSource.GetType(), dataSource.ID, "ExecuteInsert", null);
            }
            else if (string.IsNullOrEmpty(_viewName))
            {
                throw new EEPException(EEPException.ExceptionType.PropertyNull, dataSource.GetType(), dataSource.ID, "DataMember", null);
            }
            else if (master.InnerDataSet == null)
            {
                throw new EEPException(EEPException.ExceptionType.ControlNotInitial, dataSource.GetType(), dataSource.ID, "WebDataSource", null);
            }
            else
            {
                DataTable table = master.InnerDataSet.Tables[_viewName];
                if (table == null)
                {
                    throw new EEPException(EEPException.ExceptionType.PropertyInvalid, dataSource.GetType(), dataSource.ID, "DataMember", _viewName);
                }

                // 添加WebDataSource的Adding事件
                WebDataSourceAddingEventArgs eventArgs = new WebDataSourceAddingEventArgs(values);
                master.OnAdding(eventArgs);

                DataRow rowInsert = table.NewRow();
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    if (values.Contains(table.Columns[i].ColumnName))
                    {
                        if (values[table.Columns[i].ColumnName] != null && values[table.Columns[i].ColumnName].ToString().Length > 0)
                        {
                            rowInsert[i] = values[table.Columns[i].ColumnName];
                        }
                    }
                    else if (dataSource.RelationValues != null && dataSource.RelationValues.Contains(table.Columns[i].ColumnName))
                    {
                        rowInsert[i] = dataSource.RelationValues[table.Columns[i].ColumnName];
                    }
                }
                /*去除空行,不判断剩余行数, 因为肯定会加一笔,防止没有资料时,新增一笔主键相同的值报错*/
                if (table.ExtendedProperties.Contains("Empty") && table.ExtendedProperties["Empty"].Equals("True"))
                {
                    table.Rows.RemoveAt(0);
                    table.ExtendedProperties["Empty"] = "False";
                }
                /***********************************************************************************/
                table.Rows.Add(rowInsert);
                if (dataSource.AutoApply)
                {
                    if (string.IsNullOrEmpty(dataSource.MasterDataSource))
                    {
                        if (dataSource.AutoApplyForInsert)
                        {
                            dataSource.ApplyUpdates();
                        }
                        else
                        {
                            foreach (Control control in dataSource.Parent.Controls)
                            {
                                if (control is WebDataSource && (control as WebDataSource).MasterDataSource == dataSource.ID)
                                {
                                    return 1;
                                }
                            }
                            dataSource.ApplyUpdates();
                        }
                    }
                }
                return 1;
            }
        }

        public override bool CanUpdate
        {
            get
            {
                if (((WebDataSource)_owner).CommandTable != null)
                    return false;
                else
                    return true;
            }
        }

        protected override int ExecuteUpdate(IDictionary keys, IDictionary values, IDictionary oldValues)
        {
            WebDataSource dataSource = (WebDataSource)_owner;
            if (dataSource == null)
            {
                return 0;
            }
            WebDataSource master = dataSource.MasterWebDataSource;

            if (!string.IsNullOrEmpty(dataSource.SelectAlias) && !string.IsNullOrEmpty(dataSource.SelectCommand))
            {
                throw new EEPException(EEPException.ExceptionType.PropertyNotSupported, dataSource.GetType(), dataSource.ID, "ExecuteUpdate", null);
            }
            else if (string.IsNullOrEmpty(_viewName))
            {
                throw new EEPException(EEPException.ExceptionType.PropertyNull, dataSource.GetType(), dataSource.ID, "DataMember", null);
            }
            else if (master.InnerDataSet == null)
            {
                throw new EEPException(EEPException.ExceptionType.ControlNotInitial, dataSource.GetType(), dataSource.ID, "WebDataSource", null);
            }
            else
            {
                DataTable table = master.InnerDataSet.Tables[_viewName];
                if (table == null)
                {
                    throw new EEPException(EEPException.ExceptionType.PropertyInvalid, dataSource.GetType(), dataSource.ID, "DataMember", _viewName);
                }

                DataRow rowUpdate = null;

                WebDataSourceUpdatingEventArgs eventArgs = new WebDataSourceUpdatingEventArgs(keys, values, oldValues);
                master.OnUpdating(eventArgs);

                if (table.PrimaryKey.Length > 0)
                {
                    object[] keysUpdate = new object[table.PrimaryKey.Length];
                    for (int i = 0; i < table.PrimaryKey.Length; i++)
                    {
                        string columnName = table.PrimaryKey[i].ColumnName;
                        if (oldValues.Contains(columnName))
                        {
                            keysUpdate[i] = oldValues[columnName];
                        }
                        else if (dataSource.RelationValues != null && dataSource.RelationValues.Contains(columnName))
                        {
                            keysUpdate[i] = dataSource.RelationValues[columnName];
                        }
                        else
                        {
                            throw new EEPException(EEPException.ExceptionType.ColumnValueNotFound, dataSource.GetType(), dataSource.ID, columnName, null);
                        }
                    }
                    rowUpdate = table.Rows.Find(keysUpdate);
                }
                else
                {
                    #region All cells on row
                    string where = string.Empty;
                    IDictionaryEnumerator ovs = oldValues.GetEnumerator();
                    while (ovs.MoveNext())
                    {
                        string name = ovs.Key.ToString();
                        object value = ovs.Value;
                        Type type = table.Columns[name].DataType;

                        if (where.Length > 0)
                            where += " and ";

                        if (value == null || value == DBNull.Value || value.ToString() == "")
                            where += Quote(name) + " is null";
                        else
                            where += Quote(name) + "=" + Mark(type, TransformMarkerInColumnValue(type, value));
                    }
                    #endregion
                    DataRow[] rowUpdates = table.Select(where);
                    if (rowUpdates.Length != 1)
                    {
                        return 0;
                    }
                    rowUpdate = rowUpdates[0];

                }
                if (rowUpdate == null)
                {
                    return 0;
                }
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    if (values.Contains(table.Columns[i].ColumnName))
                    {
                        if (values[table.Columns[i].ColumnName] != null && values[table.Columns[i].ColumnName].ToString().Length > 0)
                        {
                            rowUpdate[i] = values[table.Columns[i].ColumnName];
                        }
                        else
                        {
                            rowUpdate[i] = DBNull.Value;
                        }
                    }
                }

                if (dataSource.AutoApply)
                {
                    if (string.IsNullOrEmpty(dataSource.MasterDataSource))
                    {
                        dataSource.ApplyUpdates();
                    }
                }
                return 1;
            }
        }

        public override void Select(DataSourceSelectArguments arguments, DataSourceViewSelectCallback callback)
        {
            base.Select(arguments, callback);
        }

        public override void Update(IDictionary keys, IDictionary values, IDictionary oldValues, DataSourceViewOperationCallback callback)
        {
            base.Update(keys, values, oldValues, callback);
        }

        private String Mark(Type type, Object columnValue)
        {
            if (type.Equals(typeof(Char)) || type.Equals(typeof(String)) || type.Equals(typeof(Guid)))
            {
                return _marker.ToString() + columnValue.ToString() + _marker.ToString();
            }
            else if (type.Equals(typeof(DateTime)))
            {
                DateTime t = Convert.ToDateTime(columnValue);
                string s = t.Year.ToString() + "-" + t.Month.ToString() + "-" + t.Day.ToString() + " "
                    + t.Hour.ToString() + ":" + t.Minute.ToString() + ":" + t.Second.ToString();
                return _marker.ToString() + s + _marker.ToString();
            }
            else if (type.Equals(typeof(Boolean)))
            {
                Boolean b = (Boolean)columnValue;
                if (b)
                    return "1";
                else
                    return "0";
            }
            else if (type.Equals(typeof(Byte[])))
            {
                StringBuilder builder = new StringBuilder("0x");   // 16进制、Oracle里的Binary没有测试。
                foreach (Byte b in (Byte[])columnValue)
                {
                    string tmp = Convert.ToString(b, 16);
                    if (tmp.Length < 2)
                        tmp = "0" + tmp;
                    builder.Append(tmp);
                }
                return builder.ToString();
            }
            else
            {
                return columnValue.ToString();
            }
        }

        private Object TransformMarkerInColumnValue(Type type, Object columnValue)
        {
            if (type.Equals(typeof(Char)) || type.Equals(typeof(String)))
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
    }

    //----------------------------------------------------------------------------------

    public class DesignerWebDataSourceView : DesignerDataSourceView
    {
        private object _owner;
        private string _viewName;

        public DesignerWebDataSourceView(WebDataSourceDesigner owner, string viewName)
            : base(owner, viewName)
        {
            _owner = owner;
            _viewName = viewName;
        }

        // Get data for design-time display
        //public override IEnumerable GetDesignTimeData(int minimumRows, out bool isSampleData)
        //{
        //    if (_data == null)
        //    {
        //        // Create a set of design-time fake data
        //        _data = new ArrayList();
        //        for (int i = 1; i <= minimumRows; i++)
        //        {
        //            _data.Add(new BookItem("ID_" + i.ToString(),
        //                "Design-Time Title 0" + i.ToString()));
        //        }
        //    }
        //    isSampleData = true;
        //    return _data as IEnumerable;
        //}

        public override IDataSourceViewSchema Schema
        {
            get { return new ColumnsSchema(_owner, _viewName); }
        }
    }

    //----------------------------------------------------------------------------------

    public class ColumnsSchema : IDataSourceViewSchema
    {
        private object _owner;
        private string _viewName;

        public ColumnsSchema(object owner, string viewName)
        {
            _owner = owner;
            _viewName = viewName;
        }

        public string Name
        {
            get { return "ColumnsSchema"; }
        }

        public IDataSourceFieldSchema[] GetFields()
        {
            if (_owner == null)
                return null;

            DataTable dt = ((WebDataSourceDesigner)_owner).SchemaTable;
            if (dt == null)
                return null;

            int i = dt.Columns.Count;
            IDataSourceFieldSchema[] fields = new IDataSourceFieldSchema[i];

            int j = 0;
            foreach (DataColumn col in dt.Columns)
            {
                string colName = col.ColumnName;
                Type dataType = col.DataType;

                fields[j] = new ColumnFieldSchema(colName, dataType);
                j++;
            }

            return fields;
        }

        public IDataSourceViewSchema[] GetChildren()
        {
            return null;
        }
    }

    //----------------------------------------------------------------------------------

    public class ColumnFieldSchema : IDataSourceFieldSchema
    {
        public ColumnFieldSchema(string name, Type dataType)
        {
            _name = name;
            _dataType = dataType;
        }

        private string _name;
        public string Name
        {
            set { _name = value; }
            get { return _name; }
        }

        private Type _dataType = typeof(string);
        public Type DataType
        {
            set { _dataType = value; }
            get { return _dataType; }
        }

        private bool _identity = false;
        public bool Identity
        {
            set { _identity = value; }
            get { return _identity; }
        }

        private bool _isReadOnly = false;
        public bool IsReadOnly
        {
            set { _isReadOnly = value; }
            get { return _isReadOnly; }
        }

        private bool _isUnique = true;
        public bool IsUnique
        {
            set { _isUnique = value; }
            get { return _isUnique; }
        }

        private int _length = 20;
        public int Length
        {
            set { _length = value; }
            get { return _length; }
        }

        private bool _nullable = true;
        public bool Nullable
        {
            set { _nullable = value; }
            get { return _nullable; }
        }

        private bool _primaryKey = false;
        public bool PrimaryKey
        {
            set { _primaryKey = value; }
            get { return _primaryKey; }
        }

        private int _precision = -1;
        public int Precision
        {
            set { _precision = value; }
            get { return _precision; }
        }

        private int _scale = -1;
        public int Scale
        {
            set { _scale = value; }
            get { return _scale; }
        }
    }
}
