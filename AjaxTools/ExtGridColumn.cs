using System.ComponentModel;
using System.Drawing.Design;
using System.Data;
using Srvtools;
using System;

namespace AjaxTools
{
    public class ExtSimpleColumn : InfoOwnerCollectionItem
    {
        string _columnName = "";
        string _headerText = "";
        string _dataField = "";
        string _fieldType = "string";
        string _textAlign = "left";
        string _formatter = "";
        int _width = 75;
        bool _allowSort = false;
        bool _resizable = true;
        bool _isKeyField = false;

        [Category("Infolight")]
        [DefaultValue("")]
        [NotifyParentProperty(true)]
        public string ColumnName
        {
            get { return _columnName; }
            set { _columnName = value; }
        }

        [Category("Infolight")]
        [DefaultValue("")]
        [NotifyParentProperty(true)]
        public string HeaderText
        {
            get { return _headerText; }
            set { _headerText = value; }
        }

        [Category("Infolight")]
        [DefaultValue("")]
        [NotifyParentProperty(true)]
        [Editor(typeof(ExtStaticStringEditor), typeof(UITypeEditor))]
        public virtual string DataField
        {
            get { return _dataField; }
            set 
            {
                _dataField = value;
                if (this.Owner != null && ((AjaxBaseControl)this.Owner).Site.DesignMode && !string.IsNullOrEmpty(_dataField))
                {
                    ColumnName = string.Format("col{0}{1}",((AjaxBaseControl)this.Owner).Parent.ID, _dataField);
                }
            }
        }

        [Category("Infolight")]
        [DefaultValue("string")]
        [NotifyParentProperty(true)]
        [Editor(typeof(ExtStaticStringEditor), typeof(UITypeEditor))]
        public string FieldType
        {
            get { return _fieldType; }
            set { _fieldType = value; }
        }

        [Category("Infolight")]
        [DefaultValue("left")]
        [NotifyParentProperty(true)]
        [Editor(typeof(ExtStaticStringEditor), typeof(UITypeEditor))]
        public string TextAlign
        {
            get { return _textAlign; }
            set { _textAlign = value; }
        }

        [Category("Infolight")]
        [DefaultValue("")]
        [Description("define a javascript function which returns a formatted display value./n/rthe function is something like:function converter(value,column,record,rowIndex,columnIndex,store){return 'display';}")]
        [NotifyParentProperty(true)]
        public string Formatter
        {
            get { return _formatter; }
            set { _formatter = value; }
        }

        [Category("Infolight")]
        [DefaultValue(75)]
        [NotifyParentProperty(true)]
        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }

        [Category("Infolight")]
        [DefaultValue(false)]
        [NotifyParentProperty(true)]
        public bool AllowSort
        {
            get { return _allowSort; }
            set { _allowSort = value; }
        }

        [Category("Infolight")]
        [DefaultValue(true)]
        [NotifyParentProperty(true)]
        public bool Resizable
        {
            get { return _resizable; }
            set { _resizable = value; }
        }

        [Category("Infolight")]
        [DefaultValue(false)]
        [NotifyParentProperty(true)]
        public bool IsKeyField
        {
            get { return _isKeyField; }
            set { _isKeyField = value; }
        }

        [NotifyParentProperty(true)]
        public override string Name
        {
            get { return _columnName; }
            set { _columnName = value; }
        }

        public override string ToString()
        {
            return _columnName;
        }
    }

    public class ExtGridColumn : ExtSimpleColumn
    {
        public ExtGridColumn()
        {
        }

        string _editControlId = "";
        string _defaultValue = "";
        string _defaultMethod = "";
        string _validMethod = "";
        string _validText = "";
        string _beforeEdit = "";
        string _afterEdit = "";
        ValidateType _validType = ValidateType.None;
        ExtGridEditor _editor = ExtGridEditor.TextBox;
        bool _allowNull = true;
        bool _visible = true;
        bool _isExpandColumn = true;
        bool _newLine = true;
        bool _readOnly = false;

        public override string DataField
        {
            get
            {
                return base.DataField;
            }
            set
            {
                base.DataField = value;
                if (this.Owner != null && ((AjaxBaseControl)this.Owner).Site.DesignMode && !string.IsNullOrEmpty(base.DataField))
                {
                    ColumnName = string.Format("col{0}", base.DataField);
                    AjaxBaseControl baseControl = this.Owner as AjaxBaseControl;
                    DataTable tab = this.GetSourceTable(baseControl);
                    if (tab != null)
                    {
                        Type type = tab.Columns[base.DataField].DataType;
                        if (type == typeof(Int16) || type == typeof(Int32) || type == typeof(Int64)
                            || type == typeof(UInt16) || type == typeof(UInt32) || type == typeof(UInt64))
                        {
                            FieldType = "int";
                            if (ValidType != ValidateType.Method)
                            {
                                ValidType = ValidateType.Int;
                            }
                        }
                        else if (type == typeof(float) || type == typeof(decimal) || type == typeof(double))
                        {
                            FieldType = "float";
                            if (ValidType != ValidateType.Method)
                            {
                                ValidType = ValidateType.Float;
                            }
                        }
                        else
                        {
                            if (type == typeof(DateTime))
                            {
                                FieldType = "date";
                            }
                            else if (type == typeof(Boolean))
                            {
                                FieldType = "boolean";
                            }
                            else
                            {
                                FieldType = "string";
                            }
                            if (ValidType != ValidateType.Method)
                            {
                                ValidType = ValidateType.None;
                            }
                        }
                    }
                }
            }
        }

        [Category("Infolight")]
        [DefaultValue("")]
        [NotifyParentProperty(true)]
        [Editor(typeof(ExtStaticStringEditor), typeof(UITypeEditor))]
        public string EditControlId
        {
            get { return _editControlId; }
            set { _editControlId = value; }
        }

        [Category("Infolight")]
        [DefaultValue("")]
        [NotifyParentProperty(true)]
        public string DefaultValue
        {
            get { return _defaultValue; }
            set { _defaultValue = value; }
        }

        [Category("Infolight")]
        [DefaultValue("")]
        [NotifyParentProperty(true)]
        public string DefaultMethod
        {
            get { return _defaultMethod; }
            set { _defaultMethod = value; }
        }

        [Category("Infolight")]
        [DefaultValue("")]
        [NotifyParentProperty(true)]
        public string ValidMethod
        {
            get { return _validMethod; }
            set { _validMethod = value; }
        }

        [Category("Infolight")]
        [DefaultValue("")]
        [NotifyParentProperty(true)]
        public string ValidText
        {
            get { return _validText; }
            set { _validText = value; }
        }

        [Category("Infolight")]
        [DefaultValue("")]
        [NotifyParentProperty(true)]
        [Description("function(eventObject);eventObject:{grid,record,field,value,row,column,cancel}")]
        public string BeforeEdit
        {
            get { return _beforeEdit; }
            set { _beforeEdit = value; }
        }

        [Category("Infolight")]
        [DefaultValue("")]
        [NotifyParentProperty(true)]
        [Description("function(eventObject);eventObject:{grid,record,field,value,originalValue,row,column,cancel}")]
        public string AfterEdit
        {
            get { return _afterEdit; }
            set { _afterEdit = value; }
        }

        [Category("Infolight")]
        [DefaultValue(typeof(ValidateType), "None")]
        [NotifyParentProperty(true)]
        public ValidateType ValidType
        {
            get { return _validType; }
            set { _validType = value; }
        }

        [Category("Infolight")]
        [DefaultValue(typeof(ExtGridEditor), "TextBox")]
        [NotifyParentProperty(true)]
        public ExtGridEditor Editor
        {
            get 
            {
                return _editor;
            }
            set 
            {
                _editor = value;
                //if (_editor == ExtGridEditor.DateTimePicker)
                //{
                //    PropertyDescriptor propEditId = TypeDescriptor.GetProperties(this)["EditControlId"];
                //}
            }
        }

        [Category("Infolight")]
        [DefaultValue(true)]
        [NotifyParentProperty(true)]
        public bool AllowNull
        {
            get { return _allowNull; }
            set { _allowNull = value; }
        }

        [Category("Infolight")]
        [DefaultValue(true)]
        [NotifyParentProperty(true)]
        public bool Visible
        {
            get { return _visible; }
            set { _visible = value; }
        }

        [Category("Infolight")]
        [DefaultValue(true)]
        [NotifyParentProperty(true)]
        public bool ExpandColumn
        {
            get { return _isExpandColumn; }
            set { _isExpandColumn = value; }
        }

        [Category("Infolight")]
        [DefaultValue(true)]
        [NotifyParentProperty(true)]
        public bool NewLine
        {
            get { return _newLine; }
            set { _newLine = value; }
        }

        [Category("Infolight")]
        [DefaultValue(false)]
        [NotifyParentProperty(true)]
        public bool ReadOnly
        {
            get { return _readOnly; }
            set { _readOnly = value; }
        }


        DataTable GetSourceTable(AjaxBaseControl ctrl)
        {
            if (ctrl is IAjaxDataSource)
            {
                object src = ctrl.GetObjByID(((IAjaxDataSource)ctrl).DataSourceID);
                if (src != null && src is WebDataSource)
                {
                    WebDataSource wds = src as WebDataSource;
                    if (string.IsNullOrEmpty(wds.SelectAlias) && string.IsNullOrEmpty(wds.SelectCommand))
                    {
                        if (wds.DesignDataSet == null)
                        {
                            WebDataSet ds = GloFix.CreateDataSet(wds.WebDataSetID);
                            wds.DesignDataSet = ds.RealDataSet;
                        }
                        return wds.DesignDataSet.Tables[wds.DataMember].Clone();
                    }
                    else
                    {
                        return wds.CommandTable.Clone();
                    }
                }
            }
            return null;
        }

        /*---------------------------过期属性---------------------------*/
        string _queryOperator = "=";
        string _queryCaption = "";
        string _queryDefaultValue = "";
        string _queryCondition = "None";

        [Category("Infolight")]
        [DefaultValue("=")]
        [Editor(typeof(ExtStaticStringEditor), typeof(UITypeEditor))]
        [NotifyParentProperty(true)]
        [Browsable(false)]
        public string QueryOperator
        {
            get { return _queryOperator; }
            set { _queryOperator = value; }
        }

        [Category("Infolight")]
        [DefaultValue("")]
        [NotifyParentProperty(true)]
        [Browsable(false)]
        public string QueryCaption
        {
            get { return _queryCaption; }
            set { _queryCaption = value; }
        }

        [Category("Infolight")]
        [DefaultValue("")]
        [NotifyParentProperty(true)]
        [Browsable(false)]
        public string QueryDefaultValue
        {
            get { return _queryDefaultValue; }
            set { _queryDefaultValue = value; }
        }

        [Category("Infolight")]
        [DefaultValue("None")]
        [Editor(typeof(ExtStaticStringEditor), typeof(UITypeEditor))]
        [NotifyParentProperty(true)]
        [Browsable(false)]
        public string QueryCondition
        {
            get { return _queryCondition; }
            set { _queryCondition = value; }
        }
        /*---------------------------过期属性---------------------------*/
    }

    public class ExtQueryField : InfoOwnerCollectionItem
    {
        //bool _newLine = true;
        string _id = "";
        string _dataField = "";
        string _operator = "=";
        string _caption = "";
        string _defaultValue = "";
        string _condition = "None";

        //[Category("Infolight")]
        //[DefaultValue(true)]
        //[NotifyParentProperty(true)]
        //public bool NewLine
        //{
        //    get { return _newLine; }
        //    set { _newLine = value; }
        //}

        [Category("Infolight")]
        [DefaultValue("")]
        [NotifyParentProperty(true)]
        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        [Category("Infolight")]
        [DefaultValue("")]
        [NotifyParentProperty(true)]
        [Editor(typeof(ExtStaticStringEditor), typeof(UITypeEditor))]
        public virtual string DataField
        {
            get { return _dataField; }
            set 
            { 
                _dataField = value;
                if (this.Owner != null && !string.IsNullOrEmpty(_dataField))
                {
                    Id = _dataField;
                    AjaxBaseControl baseControl = this.Owner as AjaxBaseControl;
                    if (baseControl.Site.DesignMode && baseControl is IAjaxDataSource)
                    {
                        WebDataSource wds = baseControl.GetObjByID((baseControl as IAjaxDataSource).DataSourceID) as WebDataSource;
                        if (wds != null)
                        {
                            Caption = setDDCaption(_dataField, DBUtils.GetDataDictionary(wds, true).Tables[0]);
                        }
                        else
                        {
                            Caption = _dataField;
                        }
                    }
                }
            }
        }

        [Category("Infolight")]
        [DefaultValue("=")]
        [Editor(typeof(ExtStaticStringEditor), typeof(UITypeEditor))]
        [NotifyParentProperty(true)]
        public string Operator
        {
            get { return _operator; }
            set { _operator = value; }
        }

        [Category("Infolight")]
        [DefaultValue("")]
        [NotifyParentProperty(true)]
        public string Caption
        {
            get { return _caption; }
            set { _caption = value; }
        }

        [Category("Infolight")]
        [DefaultValue("")]
        [NotifyParentProperty(true)]
        public string DefaultValue
        {
            get { return _defaultValue; }
            set { _defaultValue = value; }
        }

        [Category("Infolight")]
        [DefaultValue("None")]
        [Editor(typeof(ExtStaticStringEditor), typeof(UITypeEditor))]
        [NotifyParentProperty(true)]
        public string Condition
        {
            get { return _condition; }
            set { _condition = value; }
        }

        [NotifyParentProperty(true)]
        public override string Name
        {
            get { return _id; }
            set { _id = value; }
        }

        string setDDCaption(string fieldName, DataTable ddTable)
        {
            foreach (DataRow row in ddTable.Rows)
            {
                if (string.Compare(row["FIELD_NAME"].ToString(), fieldName, true) == 0)
                {
                    return row["CAPTION"].ToString();
                }
            }
            return fieldName;
        }
    }
}
