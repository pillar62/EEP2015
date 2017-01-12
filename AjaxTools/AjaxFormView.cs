using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Text;
using System.Web.UI;
using Srvtools;
using System.Data;

namespace AjaxTools
{
    [ParseChildren(true)]
    [PersistChildren(false)]
    [Designer(typeof(AjaxFormViewDesigner), typeof(IDesigner))]
    public class AjaxFormView : AjaxBaseControl, IAjaxDataSource
    {
        int _labelWidth = 90;
        int _fieldWidth = 240;
        int _width = 580;
        int _height = 300;
        string _dataSourceId = "";
        string _title = "";
        AjaxFormFieldCollection _fields;

        [Category("InfoLight")]
        [DefaultValue(90)]
        public int LabelWidth
        {
            get { return _labelWidth; }
            set { _labelWidth = value; }
        }

        [Category("InfoLight")]
        [DefaultValue(240)]
        public int FieldWidth
        {
            get { return _fieldWidth; }
            set { _fieldWidth = value; }
        }

        [Category("Infolight")]
        [DefaultValue("")]
        [Editor(typeof(DataSourceEditor), typeof(UITypeEditor))]
        public string DataSourceID
        {
            get { return _dataSourceId; }
            set { _dataSourceId = value; }
        }

        [Category("InfoLight")]
        [DefaultValue("")]
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        [Category("Infolight")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [TypeConverter(typeof(CollectionConverter))]
        [NotifyParentProperty(true)]
        public AjaxFormFieldCollection Fields
        {
            get
            {
                if (_fields == null)
                    _fields = new AjaxFormFieldCollection(this, typeof(AjaxFormField));
                return _fields;
            }
        }

        public string GenEventHandlers(string eventName)
        {
            StringBuilder builder = new StringBuilder();
            foreach (AjaxFormField field in this.Fields)
            {
                if (eventName == "focus" && !string.IsNullOrEmpty(field.OnFocus))
                {
                    builder.Append("{");
                    builder.AppendFormat("field:'{0}',handler:{1}",
                        field.DataField,
                        field.OnFocus);
                    builder.Append("},");
                }
                else if (eventName == "leave" && !string.IsNullOrEmpty(field.OnLeave))
                {
                    builder.Append("{");
                    builder.AppendFormat("field:'{0}',handler:{1}",
                        field.DataField,
                        field.OnLeave);
                    builder.Append("},");
                }
            }
            return builder.ToString();
        }

        public string GenFormView(string title)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("{");
            int columnsCount = this.ColumnsCount();
            string formTitle = string.IsNullOrEmpty(title) ? this.Title : title;
            builder.AppendFormat("id:'{0}',items:{1},title:'{2}',",
                this.ID,
                //columnsCount,
                this.GenItems(columnsCount),
                formTitle);
            builder.Append("layout: {type: 'table',align: 'stretch',columns:" + columnsCount + "}");
            builder.Append("}");
            return builder.ToString();
        }

        public string GenKeyFields()
        {
            StringBuilder builder = new StringBuilder();
            if (this.Fields.Count > 0)
            {
                for (int i = 0; i < this.Fields.Count; i++)
                {
                    if (this.Fields[i].IsKeyField)
                    {
                        builder.AppendFormat("'{0}',", this.Fields[i].DataField);
                    }
                }
                if (builder.ToString().EndsWith(","))
                {
                    builder.Remove(builder.Length - 1, 1);
                }
            }
            return builder.ToString();
        }

        public string GenFields()
        {
            StringBuilder builder = new StringBuilder();
            if (this.Fields.Count > 0)
            {
                for (int i = 0; i < this.Fields.Count; i++)
                {
                    builder.AppendFormat("'{0}',", this.Fields[i].DataField);
                }
                if (builder.ToString().EndsWith(","))
                {
                    builder.Remove(builder.Length - 1, 1);
                }
            }
            return builder.ToString();
        }

        public string GenDefaultValues(WebDataSource wds)
        {
            StringBuilder builder = new StringBuilder();
            DataTable tab = wds.InnerDataSet.Tables[wds.DataMember];
            foreach (AjaxFormField field in this.Fields)
            {
                if (!string.IsNullOrEmpty(field.DefaultValue))
                {
                    Type type = tab.Columns[field.DataField].DataType;
                    if (GloFix.IsNumeric(type))
                    {
                        builder.AppendFormat("{0}:{1},", field.DataField, field.DefaultValue);
                    }
                    else if (type == typeof(Boolean))
                    {
                        builder.AppendFormat("{0}:{1},", field.DataField, field.DefaultValue.ToLower());
                    }
                    else
                    {
                        builder.AppendFormat("{0}:'{1}',", field.DataField, field.DefaultValue);
                    }
                }
                else if (!string.IsNullOrEmpty(field.DefaultMethod))
                {
                    if (field.DefaultMethod.IndexOf('.') != -1)
                    {
                        builder.AppendFormat("{0}:'@srvMethod:{1}',", field.DataField, field.DefaultMethod);
                    }
                    else
                    {
                        builder.AppendFormat("{0}:{1}(),", field.DataField, field.DefaultMethod);
                    }
                }
            }
            if (builder.ToString().EndsWith(","))
            {
                builder.Remove(builder.Length - 1, 1);
            }
            return builder.ToString();
        }

        int ColumnsCount()
        {
            int maxColumnsCount = 1, columsCount = 1;
            foreach (AjaxFormField field in this.Fields)
            {
                if (!field.NewLine)
                {
                    columsCount++;
                }
                else
                {
                    columsCount = 1;
                }
                maxColumnsCount = Math.Max(maxColumnsCount, columsCount);
            }
            return maxColumnsCount;
        }

        string GenItems(int columnsCount)
        {
            StringBuilder builder = new StringBuilder();
            int index = 0;
            builder.Append("[");
            for (int i = 0; i < this.Fields.Count; i++)
            {
                int colSpan = 1;
                if (i < this.Fields.Count - 1)
                {
                    if (this.Fields[i + 1].NewLine)
                    {
                        colSpan = columnsCount - index;
                        index = 0;
                    }
                    else
                    {
                        index++;
                    }
                }
                else
                {
                    colSpan = columnsCount - index;
                    //index++;
                }
                if (this.Fields[i].Editor != ExtGridEditor.RefButton)
                {
                    //builder.AppendFormat("width:{0},labelWidth:{1},items:[{2}],{3}",
                    builder.AppendFormat("{0},",
                        this.GenFieldControl(this.Fields[i], this.LabelWidth,colSpan)
                        );
                }
                else
                {
                    builder.Append("{");//1{
                    ExtRefButton refButton = this.GetObjByID(this.Fields[i].EditControlId) as ExtRefButton;
                    builder.AppendFormat("{0}width:{1},",
                        colSpan > 1 ? string.Format("colspan:{0},", colSpan) : "",
                        this.FieldWidth * colSpan);
                    if (this.Fields[i].ReadOnly)
                    {
                        builder.Append("readOnly:'true',");
                    }

                    builder.Append("layout:{type:'hbox',align: 'top'},items:[{");//1[ 2{
                    //builder.AppendFormat("labelWidth:{0},layout:'form',",
                    //    this.LabelWidth);
                    //builder.Append("items:[{");
    //                builder.AppendFormat("fieldLabel:'{0}',name:'{1}',width:{2},{3}disabled:true,disabledCls:'info-x-item-disabled',xtype:'textfield',",
    //this.Fields[i].Caption,
    //this.Fields[i].DataField,
    //this.Fields[i].Width - 24,
    //string.IsNullOrEmpty(this.Fields[i].Formatter) ? "" : string.Format("format:'{0}',", this.Fields[i].Formatter));

                    builder.AppendFormat("labelWidth:{4},fieldLabel:'{0}',name:'{1}',width:{2},{3}disabled:true,disabledCls:'info-x-item-disabled',xtype:'textfield'",
                        this.Fields[i].Caption,
                        this.Fields[i].DataField,
                        this.Fields[i].Width - 24,
                        string.IsNullOrEmpty(this.Fields[i].Formatter) ? "" : string.Format("format:'{0}',", this.Fields[i].Formatter),
                        this.LabelWidth);
                    if ((!this.Fields[i].AllowNull || this.Fields[i].ValidType != ValidateType.None) && this.Fields[i].Editor != ExtGridEditor.RefVal)
                    {
                        builder.AppendFormat(",{0}", ExtValidator.GenValidateConfig(this.Fields[i].AllowNull, this.Fields[i].ValidType, this.Fields[i].ValidMethod, this.Fields[i].ValidText));
                    }
                    //builder.Append("listeners:{enable:function(cmp){Infolight.FormHelper.setRefButtonEnable(cmp,true);},disable:function(cmp){Infolight.FormHelper.setRefButtonEnable(cmp, false);}}}]");
                    //builder.Append("},{items:[{xtype:'button',disabled:true,iconCls:'ext_refbtn_icon',listeners:{click:function(button, eventObject){");
                    //if (refButton != null)
                    //{
                    //    builder.AppendFormat("Infolight.FormHelper.refButtonClick.apply({0});", refButton.GenClickEventScope());
                    //}
                    //builder.Append("}},");
                    //builder.AppendFormat("id:'refBtn{0}'", this.Fields[i].DataField);
                    //builder.Append("}],width:24}]");//3} 2] 2} 1]

                    builder.Append(",listeners:{enable:function(cmp){Infolight.FormHelper.setRefButtonEnable(cmp,true);},disable:function(cmp){Infolight.FormHelper.setRefButtonEnable(cmp, false);}}");
                    builder.Append("},{");
                    builder.AppendFormat("id:'refBtn{0}',", this.Fields[i].DataField);
                    builder.Append("xtype:'button',width:24,disabled:true,iconCls:'ext_refbtn_icon',listeners:{click:function(button, eventObject){");
                    if (refButton != null)
                    {
                        builder.AppendFormat("Infolight.FormHelper.refButtonClick.apply({0});", refButton.GenClickEventScope());
                    }
                    builder.Append("}}");
                    builder.Append("}]");//2} 1]
                    builder.Append("}");//1}
                    builder.Append(",");
                }
            }
            if (builder.ToString().EndsWith(","))
            {
                builder.Remove(builder.Length - 1, 1);
            }
            builder.Append("]");

            return builder.ToString();
        }

        string GenFieldControl(AjaxFormField field, int LabelWidth, int colSpan)
        {
            StringBuilder builder = new StringBuilder();
            if (field.Visible)
            {
                builder.Append("{");
                if (field.Editor == ExtGridEditor.RefVal)
                {
                    builder.AppendFormat("fieldLabel:'{0}',name:'{1}',",
                        field.Caption,
                        field.DataField,
                        string.IsNullOrEmpty(field.Formatter) ? "" : string.Format("format:'{0}',", field.Formatter),
                        field.Width,
                        LabelWidth,
                        colSpan > 1 ? string.Format("colspan:{0},", colSpan) : "");
                }
                else
                {
                    builder.AppendFormat("fieldLabel:'{0}',id:'{5}',name:'{1}',{2}width:{3},labelWidth:{4},disabled:true,disabledCls:'info-x-item-disabled',",
                        field.Caption,
                        field.DataField,
                        string.IsNullOrEmpty(field.Formatter) ? "" : string.Format("format:'{0}',", field.Formatter),
                        field.Width,
                        LabelWidth,
                        this.ID + field.DataField);
                }
                if (field.ReadOnly)
                {
                    builder.Append("readOnly:'true',");
                }
                switch (field.Editor)
                {
                    case ExtGridEditor.TextBox:
                        builder.Append("xtype:'textfield'");
                        break;
                    case ExtGridEditor.TextArea:
                        builder.Append("xtype:'textarea'");
                        builder.AppendFormat(",height:{0}", field.Height);
                        break;
                    case ExtGridEditor.NumberField:
                        builder.Append("xtype:'numberfield'");
                        if (field.Formatter != null)
                        {
                            string decimalPrecision = "0";
                            try
                            {
                                string[] formatters = field.Formatter.Split(new char[] { ',', '.' });
                                if (formatters.Length > 1)
                                {
                                    decimalPrecision = formatters[1].Length.ToString();
                                }
                            }
                            finally { }
                            builder.AppendFormat(",decimalPrecision:{0}", decimalPrecision);
                        }
                        break;
                    case ExtGridEditor.CheckBox:
                        builder.Append("xtype:'checkbox'");
                        break;
                    case ExtGridEditor.ComboBox:
                        builder.Append("xtype:'infoCombo',embededIn:'form',");
                        if (!string.IsNullOrEmpty(field.EditControlId))
                        {
                            ExtComboBox cmb = this.GetObjByID(field.EditControlId) as ExtComboBox;
                            if (cmb != null)
                            {
                                builder.Append(cmb.GenComboBoxConfig(null, field.DataField, true));
                            }
                        }
                        break;
                    case ExtGridEditor.DateTimePicker:
                        builder.Append("xtype:'datefield'");
                        break;
                    case ExtGridEditor.RefVal:
                        ExtRefVal refVal = this.GetObjByID(field.EditControlId) as ExtRefVal;

                        if (refVal != null)
                        {
                            builder.Append(refVal.GenRefValConfig(field.DataField, field.Caption, LabelWidth, this.ID, field.AllowNull, field.ValidType, field.ValidMethod, field.ValidText));
                        }
                        //builder.Append("xtype:'infoRefVal'");

                        //builder.AppendFormat("id: '{0}',", field.DataField);//refVal.ClientID + "TextBox"
                        //builder.Append("xtype:'panel',");
                        //builder.Append("layout: 'column',");
                        ////builder.AppendFormat("width: 80,", refVal.Width);
                        //builder.Append("baseCls: \"x-plain\",");
                        //builder.Append("items: [{");
                        //builder.Append("disabled:true,disabledClass:'info-x-item-disabled',");
                        //builder.Append("xtype: 'textfield',");
                        ////builder.Append("baseCls: \"x-plain\",");
                        //builder.Append("columnWidth: 0.8");
                        //builder.Append("},{");
                        ////builder.AppendFormat("id: '{0}',", field.DataField + "Button");//refVal.ClientID + "TextBox"
                        //builder.Append("xtype: 'button',");
                        //builder.Append("disabled:true,disabledClass:'info-x-item-disabled',");
                        ////builder.Append("baseCls: \"x-plain\",");
                        //builder.Append("columnWidth: 0.2,");
                        //builder.Append("listeners: {");
                        //builder.Append("'click': function(){");
                        //builder.Append("Ext.MessageBox.alert('Message', 'Test ! ');");
                        //builder.Append("}");
                        //builder.Append("}");
                        //builder.Append("}]");
                        break;
                }
                if ((!field.AllowNull || field.ValidType != ValidateType.None) && field.Editor != ExtGridEditor.RefVal)
                {
                    builder.AppendFormat(",{0}", ExtValidator.GenValidateConfig(field.AllowNull, field.ValidType, field.ValidMethod, field.ValidText));
                }
                builder.Append("}");
            }
            return builder.ToString();
        }

        public string GenValidJsonArray()
        {
            StringBuilder builder = new StringBuilder();
            foreach (AjaxFormField field in this.Fields)
            {
                string validConfig = ExtValidator.GenValidateConfig(field.AllowNull, field.ValidType, field.ValidMethod, field.ValidText);
                if (!string.IsNullOrEmpty(validConfig))
                {
                    builder.AppendFormat("{{ field: '{0}', validConfig: {{ {1} }} }},", field.DataField, validConfig);
                }
            }
            if (builder.ToString().EndsWith(","))
            {
                builder.Remove(builder.Length - 1, 1);
            }
            return builder.ToString();
        }
    }
}
