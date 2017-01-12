using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Design;
using System.Collections;
using System.Reflection;

namespace Srvtools
{
    [ToolboxItem(true)]
    public partial class AnyQuery : InfoBaseComp, IGetValues
    {
        public AnyQuery()
        {
            _Columns = new AnyQueryColumnsCollection(this, typeof(AnyQueryColumns));
            InitializeComponent();
        }

        private InfoBindingSource _BindingSource;
        [Category("Infolight"),
        Description("The InfoBindingSource which the control is bound to")]
        public InfoBindingSource BindingSource
        {
            get
            {
                if (DesignMode && this._BindingSource != null && this.Columns.Count == 0)
                {
                    InfoDataSet ids = this._BindingSource.DataSource as InfoDataSet;
                    if (ids != null)
                    {
                        foreach (DataColumn dc in ids.RealDataSet.Tables[0].Columns)
                        {
                            AnyQueryColumns ac = new AnyQueryColumns();
                            this.Columns.Add(ac);
                            ac.Column = dc.ColumnName;
                            if (dc.DataType == typeof(DateTime))
                                ac.ColumnType = "AnyQueryCalendarColumn";
                            else
                                ac.ColumnType = "AnyQueryTextBoxColumn";
                        }
                    }
                }
                return _BindingSource;
            }
            set
            {
                _BindingSource = value;
            }
        }

        private AnyQueryColumnsCollection _Columns;
        [Category("Infolight"),
        Description("The columns which ClientQuery is applied to")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public AnyQueryColumnsCollection Columns
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

        private int _MaxColumnCount = 10;
        [Category("Infolight"), DefaultValue(10)]
        public int MaxColumnCount
        {
            get
            {
                return _MaxColumnCount;
            }
            set
            {
                _MaxColumnCount = value;
            }
        }

        private bool _AutoDisableColumns = true;
        [Category("Infolight"), DefaultValue(true)]
        public bool AutoDisableColumns
        {
            get
            {
                return _AutoDisableColumns;
            }
            set
            {
                _AutoDisableColumns = value;
            }
        }

        private String _AnyQueryID = String.Empty;
        [Category("Infolight")]
        public String AnyQueryID
        {
            get
            {
                return _AnyQueryID;
            }
            set
            {
                _AnyQueryID = value;
            }
        }

        private bool _AllowAddQueryField = true;
        [Category("Infolight"), DefaultValue(true)]
        public bool AllowAddQueryField
        {
            get
            {
                return _AllowAddQueryField;
            }
            set
            {
                _AllowAddQueryField = value;
            }
        }

        private AnyQueryColumnMode _QueryColumnMode = AnyQueryColumnMode.ByBindingSource;
        [Category("Infolight"), DefaultValue(AnyQueryColumnMode.ByBindingSource)]
        public AnyQueryColumnMode QueryColumnMode
        {
            get
            {
                return _QueryColumnMode;
            }
            set
            {
                _QueryColumnMode = value;
            }
        }

        private String _PackageForm;
        [Browsable(false)]
        public String PackageForm
        {
            get { return _PackageForm; }
            set { _PackageForm = value; }
        }

        private InfoBindingSource _DetailBindingSource;
        [Category("Infolight")]
        public InfoBindingSource DetailBindingSource
        {
            get
            {
                return _DetailBindingSource;
            }
            set
            {
                _DetailBindingSource = value;
            }
        }

        private String _MasterDetailField;
        [Category("Infolight")]
        [Editor(typeof(FieldNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public String MasterDetailField
        {
            get
            {
                return _MasterDetailField;
            }
            set
            {
                _MasterDetailField = value;
            }
        }

        private String _DetailKeyField;
        [Category("Infolight")]
        [Editor(typeof(FieldNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public String DetailKeyField
        {
            get
            {
                return _DetailKeyField;
            }
            set
            {
                _DetailKeyField = value;
            }
        }

        private bool _keepcondition;
        [Category("Infolight"),
        Description("Indicates whether the text will be cleared after excute query")]
        public bool KeepCondition
        {
            get
            {
                return _keepcondition;
            }
            set
            {
                _keepcondition = value;
            }
        }

        private bool _DisplayAllOperator;
        [Category("Infolight"),DefaultValue(false)]
        public bool DisplayAllOperator
        {
            get
            {
                return _DisplayAllOperator;
            }
            set
            {
                _DisplayAllOperator = value;
            }
        }

        protected void OnValueControlEnter(EventArgs value)
        {
            EventHandler handler = (EventHandler)base.Events[EventOnValueControlEnter];
            if (handler != null)
            {
                handler(this, value);
            }
        }
        internal static readonly object EventOnValueControlEnter = new object();
        public event EventHandler ValueControlEnter
        {
            add { base.Events.AddHandler(EventOnValueControlEnter, value); }
            remove { base.Events.RemoveHandler(EventOnValueControlEnter, value); }
        }

        //当按下Query按钮之后执行的事件
        protected void OnAfterQuery(AfterQueryEventArgs value)
        {
            AfterQueryEventHandler handler = (AfterQueryEventHandler)base.Events[EventOnAfterQuery];
            if (handler != null)
            {
                handler(this, value);
            }
        }
        internal static readonly object EventOnAfterQuery = new object();
        public event AfterQueryEventHandler AfterQuery
        {
            add { base.Events.AddHandler(EventOnAfterQuery, value); }
            remove { base.Events.RemoveHandler(EventOnAfterQuery, value); }
        }

        private frmAnyQuery faq = null;
        public String Execute()
        {
            if (faq == null || !this.KeepCondition)
                faq = new frmAnyQuery(this);
            faq.ValueControlEdit += new EventHandler(faq_ValueControlEdit);
            faq.AfterQuery += new AfterQueryEventHandler(faq_AfterQuery);
            DialogResult dr = faq.ShowDialog();
            if (dr == DialogResult.Yes && faq.Where != String.Empty)
            {
                return faq.Where;
            }
            else
                return null;
        }

        void faq_AfterQuery(object sender, AfterQueryEventArgs e)
        {
            OnAfterQuery(e);
        }

        public String Execute(InfoNavigator aInfoNavigator, bool executeSQL)
        {
            if (faq == null || !this.KeepCondition)
                faq = new frmAnyQuery(this, aInfoNavigator, executeSQL);
            faq.ValueControlEdit += new EventHandler(faq_ValueControlEdit);
            DialogResult dr = faq.ShowDialog();
            if (dr == DialogResult.Yes && faq.Where != String.Empty)
                return faq.Where;
            else
                return null;
        }

        void faq_ValueControlEdit(object sender, EventArgs e)
        {
            OnValueControlEnter(new EventArgs());
        }

        public String[] GetControlValues(int count)
        {
            String[] strs = faq.GetControlValues(count);
            String[] values = new String[2];
            values[0] = strs[3];
            values[1] = strs[4];

            return strs;
        }

        #region IGetValues 成员

        public string[] GetValues(string sKind)
        {
            List<String> values = new List<string>();
            if (sKind == "DetailKeyField")
            {
                if (this.DetailBindingSource != null)
                {
                    DataView dataView = this.DetailBindingSource.List as DataView;

                    if (dataView != null)
                    {
                        foreach (DataColumn column in dataView.Table.Columns)
                        {
                            values.Add(column.ColumnName);
                        }
                    }
                    else
                    {
                        int iRelationPos = -1;
                        DataSet ds = ((InfoDataSet)this.DetailBindingSource.GetDataSource()).RealDataSet;
                        for (int i = 0; i < ds.Relations.Count; i++)
                        {
                            if (this.DetailBindingSource.DataMember == ds.Relations[i].RelationName)
                            {
                                iRelationPos = i;
                                break;
                            }
                        }
                        if (iRelationPos != -1)
                        {
                            foreach (DataColumn column in ds.Relations[iRelationPos].ChildTable.Columns)
                            {
                                values.Add(column.ColumnName);
                            }
                        }
                    }
                }
            }
            else if (sKind == "MasterDetailField")
            {
                if (this.BindingSource != null)
                {
                    DataView dataView = this.BindingSource.List as DataView;

                    if (dataView != null)
                    {
                        foreach (DataColumn column in dataView.Table.Columns)
                        {
                            values.Add(column.ColumnName);
                        }
                    }
                    else
                    {
                        int iRelationPos = -1;
                        DataSet ds = ((InfoDataSet)this.BindingSource.GetDataSource()).RealDataSet;
                        for (int i = 0; i < ds.Relations.Count; i++)
                        {
                            if (this.BindingSource.DataMember == ds.Relations[i].RelationName)
                            {
                                iRelationPos = i;
                                break;
                            }
                        }
                        if (iRelationPos != -1)
                        {
                            foreach (DataColumn column in ds.Relations[iRelationPos].ChildTable.Columns)
                            {
                                values.Add(column.ColumnName);
                            }
                        }
                    }
                }
            }
            return values.ToArray();
        }

        #endregion
    }

    public delegate void AfterQueryEventHandler(object sender, AfterQueryEventArgs e);
    public class AfterQueryEventArgs : EventArgs
    {
        public AfterQueryEventArgs()
        {

        }

        public bool Cancel = false;

        public String whereString;
    }

    public enum AnyQueryColumnMode
    {
        ByColumns = 0,
        ByBindingSource = 1
    }

    public class AnyQueryColumnsCollection : InfoOwnerCollection
    {
        public AnyQueryColumnsCollection(Object aOwner, Type aItemType)
            : base(aOwner, typeof(AnyQueryColumns))
        {

        }
        public DataSet DsForDD = new DataSet();
        public new AnyQueryColumns this[int index]
        {
            get
            {
                return (AnyQueryColumns)InnerList[index];
            }
            set
            {
                if (index > -1 && index < Count)
                {
                    if (value is AnyQueryColumns)
                    {
                        //原来的Collection设置为0
                        ((AnyQueryColumns)InnerList[index]).Collection = null;
                        InnerList[index] = value;
                        //Collection设置为this
                        ((AnyQueryColumns)InnerList[index]).Collection = this;
                    }
                }
            }
        }
    }

    public class AnyQueryColumns : InfoOwnerCollectionItem, IGetValues
    {
        #region Constructor

        public AnyQueryColumns()
            : this("anyquery",  "", "", 120)
        {

        }

        public AnyQueryColumns(string name, string column, string caption, int width)
        {
            _name = name;
            _Column = column;
            _caption = caption;
            _width = width;
            _columntype = "AnyQueryTextBoxColumn";
            _condition = "And";
            _operator = "=";
            _defaultvalue = "";
            _text = "";
        }

        #endregion

        #region Properties

        private string _name;
        public override string Name
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

        private string _Column;
        [Editor(typeof(FieldNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string Column
        {
            get
            {
                return _Column;
            }
            set
            {
                _Column = value;

                if (this.Owner != null)
                {
                    String columnName = _Column;
                    if (_Column.StartsWith("Detail."))
                    {
                        columnName = columnName.Substring(columnName.IndexOf('.') + 1);

                        if (((AnyQuery)this.Owner).Site == null)
                        {
                            this.Caption = GetDetailHeaderText(columnName);
                        }
                        else if (((AnyQuery)this.Owner).Site.DesignMode)
                        {
                            this.Caption = GetDetailHeaderText(columnName);
                        }

                        //if (!this.Caption.StartsWith("Detail."))
                        //    this.Caption = "Detail." + this.Caption;
                    }
                    else
                    {
                        if (((AnyQuery)this.Owner).Site == null)
                        {
                            this.Caption = GetHeaderText(columnName);
                        }
                        else if (((AnyQuery)this.Owner).Site.DesignMode)
                        {
                            this.Caption = GetHeaderText(columnName);
                        }
                    }
                }
            }
        }

        private string _columntype;
        [Editor(typeof(FieldNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [DefaultValue("AnyQueryTextBoxColumn")]
        public string ColumnType
        {
            get
            {
                return _columntype;
            }
            set
            {
                _columntype = value;

                if (_columntype == "AnyQueryRefButtonColumn")
                {
                    _refButtonAutoPanel = true;
                }
                else
                {
                    _refButtonAutoPanel = false;
                }

                if (_columntype != "AnyQueryComboBoxColumn" && _columntype != "AnyQueryRefValColumn")
                {
                    _refval = null;
                }
            }
        }

        private string _caption;
        public string Caption
        {
            get
            {
                return _caption;
            }
            set
            {
                if (value != null && value != "")
                {
                    _caption = value;
                    _name = _caption;
                }
                else
                {
                    if (_Column != null && _Column != "")
                    {
                        _caption = _Column;
                        _name = _Column;
                    }
                    else
                    {
                        _name = "clientquery";
                    }
                }
            }
        }

        private string _operator;
        [Editor(typeof(FieldNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [DefaultValue("=")]
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

        private InfoRefVal _refval;
        public InfoRefVal InfoRefVal
        {
            get
            {
                return _refval;
            }
            set
            {
                if (_columntype != "AnyQueryComboBoxColumn" && _columntype != "AnyQueryRefValColumn" && _columntype != "AnyQueryRefButtonColumn" && value != null)
                {
                    MessageBox.Show("InfoRefval can be set only when\ncolumntype is combobox & refval.", "notice", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    _refval = value;
                }
            }
        }

        private Panel _refButtonPanel;
        public Panel InfoRefButtonPanel
        {
            get
            {
                return _refButtonPanel;
            }
            set
            {
                if (_columntype != "AnyQueryRefButtonColumn" && value != null)
                {
                    MessageBox.Show("InfoRefButtonPanel can be set only when\ncolumntype is rebutton.", "notice", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    _refButtonPanel = value;
                }
            }
        }

        private bool _refButtonAutoPanel = false;
        [DefaultValue(false)]
        public bool InfoRefButtonAutoPanel
        {
            get
            {
                return _refButtonAutoPanel;
            }
            set
            {
                if (_columntype != "AnyQueryRefButtonColumn" && value != false)
                {
                    MessageBox.Show("InfoRefButtonAutoPanel can be set only when\ncolumntype is rebutton.", "notice", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    _refButtonAutoPanel = false;
                }
                else
                {
                    _refButtonAutoPanel = value;
                }
            }
        }

        private string _defaultvalue;
        public string DefaultValue
        {
            get
            {
                return _defaultvalue;
            }
            set
            {
                _defaultvalue = value;
            }
        }

        private string _text;
        [Browsable(false)]
        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value;
            }
        }

        private int _width = 120;
        [DefaultValue(120)]
        public int Width
        {
            get
            {
                return _width;
            }
            set
            {
                _width = value;
            }
        }

        private int _columnWidth = 120;
        [DefaultValue(120)]
        public int ColumnWidth
        {
            get
            {
                return _columnWidth;
            }
            set
            {
                _columnWidth = value;
            }
        }

        private bool _dateConver;
        [DefaultValue(false)]
        public bool DateConver
        {
            get
            {
                return _dateConver;
            }
            set
            {
                _dateConver = value;
            }
        }

        private String[] _items;
        public String[] Items
        {
            get
            {
                return _items;
            }
            set
            {
                _items = value;
            }
        }

        private bool _enabled = true;
        [DefaultValue(true)]
        public bool Enabled
        {
            get
            {
                return _enabled;
            }
            set
            {
                _enabled = value;
            }
        }

        private bool _autoSelect = false;
        [DefaultValue(false)]
        public bool AutoSelect
        {
            get
            {
                return _autoSelect;
            }
            set
            {
                _autoSelect = value;
            }
        }

        private DbType _DataType;
        public DbType DataType
        {
            get
            {
                return _DataType;
            }
            set
            {
                _DataType = value;
            }
        }

        private bool isNvarChar;

        public bool IsNvarChar
        {
            get { return isNvarChar; }
            set { isNvarChar = value; }
        }
        #endregion

        #region method
        private string GetHeaderText(string ColName)
        {
            DataSet ds = ((AnyQueryColumnsCollection)this.Collection).DsForDD;
            string strTableName = "";

            strTableName = ((AnyQuery)this.Owner).BindingSource.DataMember;
            if (ds.Tables.Count == 0)
            {
                ((AnyQueryColumnsCollection)this.Collection).DsForDD = DBUtils.GetDataDictionary(((AnyQuery)this.Owner).BindingSource, true);
                ds = ((AnyQueryColumnsCollection)this.Collection).DsForDD;
            }

            string strHeaderText = "";
            if (ds != null && ds.Tables.Count > 0)
            {
                int i = ds.Tables[strTableName].Rows.Count;
                for (int j = 0; j < i; j++)
                {
                    if (ds.Tables[strTableName].Rows[j]["FIELD_NAME"].ToString().ToLower() == ColName.ToLower())
                    {
                        strHeaderText = ds.Tables[strTableName].Rows[j]["CAPTION"].ToString();
                    }
                }
            }
            return strHeaderText;
        }

        private string GetDetailHeaderText(string ColName)
        {
            DataSet ds = DBUtils.GetDataDictionary(((AnyQuery)this.Owner).DetailBindingSource, true);

            string strHeaderText = ColName;
            if (ds != null && ds.Tables.Count > 0)
            {
                int i = ds.Tables[0].Rows.Count;
                for (int j = 0; j < i; j++)
                {
                    if (ds.Tables[0].Rows[j]["FIELD_NAME"].ToString().ToLower() == ColName.ToLower())
                    {
                        if (ds.Tables[0].Rows[j]["CAPTION"].ToString() != String.Empty)
                            strHeaderText = ds.Tables[0].Rows[j]["CAPTION"].ToString();
                    }
                }
            }
            return strHeaderText;
        }

        public void setcolumn(string colname)
        {
            this._Column = colname;

        }

        #endregion


        #region IGetValues Members

        public string[] GetValues(string sKind)
        {
            string[] retList = null;
            List<string> values = new List<string>();
            if (this.Owner is AnyQuery)
            {
                if (sKind.ToLower().Equals("operator"))
                {
                    values.Add("=");
                    values.Add("!=");
                    values.Add(">");
                    values.Add("<");
                    values.Add(">=");
                    values.Add("<=");
                    values.Add("%**");
                    values.Add("**%");
                    values.Add("%%");
                    values.Add("!%%");
                    values.Add("<->");
                    values.Add("!<->");
                    values.Add("IN");
                    values.Add("NOT IN");
                }
                else if (sKind.ToLower().Equals("columntype"))
                {
                    values.Add("AnyQueryTextBoxColumn");
                    values.Add("AnyQueryComboBoxColumn");
                    values.Add("AnyQueryCheckBoxColumn");
                    values.Add("AnyQueryRefValColumn");
                    values.Add("AnyQueryCalendarColumn");
                    values.Add("AnyQueryRefButtonColumn");
                }
                else if (sKind.ToLower().Equals("column"))
                {
                    AnyQuery aq = this.Owner as AnyQuery;
                    if (aq.BindingSource != null && aq.BindingSource.DataSource != null)
                    {

                        int colNum = ((InfoDataSet)aq.BindingSource.DataSource).RealDataSet.Tables[aq.BindingSource.DataMember].Columns.Count;

                        for (int i = 0; i < colNum; i++)
                        {
                            values.Add(((InfoDataSet)aq.BindingSource.DataSource).RealDataSet.Tables[aq.BindingSource.DataMember].Columns[i].ColumnName);
                        }

                        if ((this.Owner as AnyQuery).DetailBindingSource != null)
                        {
                            DataView dataView = (this.Owner as AnyQuery).DetailBindingSource.List as DataView;

                            if (dataView != null)
                            {
                                foreach (DataColumn column in dataView.Table.Columns)
                                {
                                    values.Add("Detail." + column.ColumnName);
                                }
                            }
                            else
                            {
                                int iRelationPos = -1;
                                DataSet ds = ((InfoDataSet)(this.Owner as AnyQuery).DetailBindingSource.GetDataSource()).RealDataSet;
                                for (int i = 0; i < ds.Relations.Count; i++)
                                {
                                    if ((this.Owner as AnyQuery).DetailBindingSource.DataMember == ds.Relations[i].RelationName)
                                    {
                                        iRelationPos = i;
                                        break;
                                    }
                                }
                                if (iRelationPos != -1)
                                {
                                    foreach (DataColumn column in ds.Relations[iRelationPos].ChildTable.Columns)
                                    {
                                        values.Add("Detail." + column.ColumnName);
                                    }
                                }
                            }
                        }
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

        #endregion
    }
}
