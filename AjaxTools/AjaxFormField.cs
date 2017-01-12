using System.ComponentModel;
using System.Drawing.Design;
using System.Data;
using Srvtools;
using System;

namespace AjaxTools
{
    public class AjaxFormField : InfoOwnerCollectionItem
    {
        int _width = 230;
        int _height = 26;
        int _colSpan = 1;
        int _rowSpan = 1;
        bool _allowNull = true;
        bool _newLine = true;
        bool _isKeyField = false;
        bool _readOnly = false;
        bool _visible = true;
        string _fieldControlId = "";
        string _caption = "";
        string _dataField = "";
        string _editControlId = "";
        string _fieldType = "string";
        string _formatter = "";
        string _defaultValue = "";
        string _defaultMethod = "";
        string _validMethod = "";
        string _validText = "";
        string _onFocus = "";
        string _onLeave = "";
        ExtGridEditor _editor = ExtGridEditor.TextBox;
        ValidateType _validType = ValidateType.None;

        [Category("Infolight")]
        [DefaultValue(230)]
        [NotifyParentProperty(true)]
        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }
        [Category("Infolight")]
        [DefaultValue(140)]
        [NotifyParentProperty(true)]
        public int Height
        {
            get { return _height; }
            set { _height = value; }
        }

        [Category("Infolight")]
        [DefaultValue(1)]
        [NotifyParentProperty(true)]
        public int ColumnSpan
        {
            get { return _colSpan; }
            set { _colSpan = value; }
        }

        [Category("Infolight")]
        [DefaultValue(1)]
        [NotifyParentProperty(true)]
        public int RowSpan
        {
            get { return _rowSpan; }
            set { _rowSpan = value; }
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
        public bool NewLine
        {
            get { return _newLine; }
            set { _newLine = value; }
        }

        [Category("Infolight")]
        [DefaultValue(false)]
        [NotifyParentProperty(true)]
        public bool IsKeyField
        {
            get { return _isKeyField; }
            set { _isKeyField = value; }
        }

        [Category("Infolight")]
        [DefaultValue("")]
        [NotifyParentProperty(true)]
        public string FieldControlId
        {
            get { return _fieldControlId; }
            set { _fieldControlId = value; }
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
        [Editor(typeof(ExtStaticStringEditor), typeof(UITypeEditor))]
        public string DataField
        {
            get { return _dataField; }
            set
            {
                _dataField = value;
                AjaxBaseControl baseControl = this.Owner as AjaxBaseControl;
                if (baseControl != null && baseControl.Site.DesignMode)
                {
                    DataTable tab = this.GetSourceTable(baseControl);
                    if (tab != null)
                    {
                        Type type = tab.Columns[_dataField].DataType;
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
        [DefaultValue("string")]
        [NotifyParentProperty(true)]
        [Editor(typeof(ExtStaticStringEditor), typeof(UITypeEditor))]
        public string FieldType
        {
            get { return _fieldType; }
            set { _fieldType = value; }
        }

        [Category("Infolight")]
        [DefaultValue("")]
        [NotifyParentProperty(true)]
        public string Formatter
        {
            get { return _formatter; }
            set { _formatter = value; }
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
        public string OnFocus
        {
            get { return _onFocus; }
            set { _onFocus = value; }
        }

        [Category("Infolight")]
        [DefaultValue("")]
        [NotifyParentProperty(true)]
        public string OnLeave
        {
            get { return _onLeave; }
            set { _onLeave = value; }
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
            }
        }

        [Category("Infolight")]
        [DefaultValue(typeof(ValidateType), "None")]
        [NotifyParentProperty(true)]
        public ValidateType ValidType
        {
            get { return _validType; }
            set { _validType = value; }
        }

        [NotifyParentProperty(true)]
        public override string Name
        {
            get { return _fieldControlId; }
            set { _fieldControlId = value; }
        }

        [Category("Infolight")]
        [DefaultValue(false)]
        [NotifyParentProperty(true)]
        public bool ReadOnly
        {
            get { return _readOnly; }
            set { _readOnly = value; }
        }

        [Category("Infolight")]
        [DefaultValue(true)]
        [NotifyParentProperty(true)]
        public bool Visible
        {
            get { return _visible; }
            set { _visible = value; }
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
    }
}
