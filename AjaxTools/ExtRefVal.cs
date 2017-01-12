using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Text;
using System.Web.UI;
using System.Web.UI.Design;
using Srvtools;
using System.Data;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;
using System.Reflection;
using System.ComponentModel.Design;

namespace AjaxTools
{
    [ParseChildren(true)]
    [PersistChildren(false)]
    [Designer(typeof(ExtTriggerDesigner), typeof(IDesigner))]
    public class ExtRefVal : AjaxBaseControl, IAjaxDataSource
    {
        bool _forceSelection = true;
        bool _autoRender = true;
        bool _allowPage = false;
        int _width = 240;
        int _OpenRefWidth = 510;
        int _OpenRefHeight = 450;
        string _refValPanel = "";
        string _dataSourceId = "";
        string _displayField = "";
        string _valueField = "";
        string _refValTitle = "RefVal Window";
        ExtSimpleColumnCollection _columns;
        ExtColumnMatchCollection _columnMatch;
        ExtWhereItemCollection _whereItem;

        [Category("Infolight")]
        [DefaultValue(true)]
        public bool AutoRender
        {
            get { return _autoRender; }
            set { _autoRender = value; }
        }

        [Category("DataSource")]
        [DefaultValue("")]
        [Editor(typeof(DataSourceEditor), typeof(UITypeEditor))]
        public string DataSourceID
        {
            get { return _dataSourceId; }
            set { _dataSourceId = value; }
        }

        [Category("DataSource")]
        [DefaultValue("")]
        [Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
        public string DisplayField
        {
            get { return _displayField; }
            set { _displayField = value; }
        }

        [Category("DataSource")]
        [DefaultValue("")]
        [Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
        public string ValueField
        {
            get { return _valueField; }
            set { _valueField = value; }
        }

        [Category("Infolight")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [TypeConverter(typeof(CollectionConverter))]
        [NotifyParentProperty(true)]
        public ExtSimpleColumnCollection Columns
        {
            get
            {
                if (_columns == null)
                    _columns = new ExtSimpleColumnCollection(this, typeof(ExtSimpleColumn));
                return _columns;
            }
        }

        [Category("Infolight")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [TypeConverter(typeof(CollectionConverter))]
        [NotifyParentProperty(true)]
        public ExtColumnMatchCollection ColumnMatch
        {
            get
            {
                if (_columnMatch == null)
                    _columnMatch = new ExtColumnMatchCollection(this, typeof(ExtColumnMatch));
                return _columnMatch;
            }
        }

        [Category("Infolight")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [TypeConverter(typeof(CollectionConverter))]
        [NotifyParentProperty(true)]
        public ExtWhereItemCollection WhereItem
        {
            get
            {
                if (_whereItem == null)
                    _whereItem = new ExtWhereItemCollection(this, typeof(ExtWhereItem));
                return _whereItem;
            }
        }

        [Category("Infolight")]
        [DefaultValue("")]
        [NotifyParentProperty(true)]
        [Editor(typeof(ExtStaticStringEditor), typeof(UITypeEditor))]
        public string RefValPanel
        {
            get { return _refValPanel; }
            set { _refValPanel = value; }
        }

        [Category("Infolight"),DefaultValue(450)]
        public int OpenRefHeight
        {
            get { return _OpenRefHeight; }
            set { _OpenRefHeight = value; }
        }

        [Category("Infolight"),
        DefaultValue(510)]
        public int OpenRefWidth
        {
            get { return _OpenRefWidth; }
            set { _OpenRefWidth = value; }
        }


        [Category("Infolight")]
        [DefaultValue(240)]
        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }

        [Category("Infolight")]
        [DefaultValue("RefVal Window")]
        public String RefValTitle
        {
            get { return _refValTitle; }
            set { _refValTitle = value; }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!Page.IsPostBack)
            {
                object o = this.GetObjByID(this.DataSourceID);
                if (o != null && o is WebDataSource)
                {
                    WebDataSource wds = o as WebDataSource;
                    if (!string.IsNullOrEmpty(wds.SelectAlias) && !string.IsNullOrEmpty(wds.SelectCommand))
                    {
                        wds.CommandTable = wds.GetCommandTable();
                    }
                }

                this.RenderRefVal();
            }
        }

        public void RenderRefVal()
        {
            StringBuilder builder = new StringBuilder();
            StringBuilder refVal = new StringBuilder();
            if (this.AutoRender)
            {
                string renderTo = (string.IsNullOrEmpty(this.RefValPanel)) ? "document.body" : string.Format("'{0}'", this.RefValPanel);

                refVal.Append("new Ext.Panel({");
                refVal.AppendFormat("renderTo: {0},", renderTo);

                refVal.Append(GenRefValConfig(String.Empty));

                refVal.Append("});");
            }

            builder.Append("Ext.onReady(function(){" + refVal + "});");

            this.Page.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), builder.ToString(), true);
        }

        public string GenRefValConfig(string fieldName)
        {
            String textBoxID = String.IsNullOrEmpty(fieldName) ? this.ClientID + "TextBox" : fieldName;
            string tpl = this.GenTemplate();
            StringBuilder refVal = new StringBuilder();

            refVal.Append("layout: 'column',");
            refVal.AppendFormat("width: {0},", this.Width);
            //refVal.Append("baseCls: \"x-plain\",");
            refVal.Append("items: [{");
            refVal.AppendFormat("id: '{0}',", textBoxID);
            if (!String.IsNullOrEmpty(fieldName))
                refVal.Append("disabled:true,disabledCls:'info-x-item-disabled',");
            refVal.Append("xtype: 'textfield',");
            refVal.Append("columnWidth: 0.8,");
            refVal.Append("listeners:");
            refVal.Append("{enable:function(){");
            refVal.AppendFormat("Ext.getCmp('{0}').enable();", textBoxID + "Button");
            refVal.Append("},disable:function(){");
            refVal.AppendFormat("Ext.getCmp('{0}').disable();", textBoxID + "Button");
            refVal.Append("}}");
            refVal.Append("},{");
            //refVal.AppendFormat("id: '{0}',", textBoxID+"_Name");
            //if (!String.IsNullOrEmpty(fieldName))
            //    refVal.Append("disabled:true,disabledClass:'info-x-item-disabled',");
            //refVal.Append("xtype: 'textfield',");
            //refVal.Append("columnWidth: 0.8");
            //refVal.Append("},{");
            refVal.AppendFormat("id: '{0}',", textBoxID + "Button");
            if (!String.IsNullOrEmpty(fieldName))
                refVal.Append("disabled:true,disabledCls:'info-x-item-disabled',");
            refVal.Append("xtype: 'button',");
            refVal.Append("iconCls:'ext_refbtn_icon',");
            refVal.Append("columnWidth: 0.2,");
            refVal.Append("listeners: {");
            refVal.Append("'click': function(){");
            refVal.Append("var param = new setRefvalparam();");
            refVal.AppendFormat("param.refvalID = \"{0}\";", this.ClientID);
            refVal.AppendFormat("param.refTitle = \"{0}\";", this.RefValTitle);
            refVal.AppendFormat("param.OpenRefWidth = {0};", this.OpenRefWidth);
            refVal.AppendFormat("param.OpenRefHeight = {0};", this.OpenRefHeight);
            refVal.Append("param.refModuleName = \"GLModule\";");
            refVal.Append("param.refCommandName = \"cmdRefValUse\";");
            refVal.Append("param.refDynamic = \"Y\";");
            object o = this.GetObjByID(this.DataSourceID);
            if (o != null && o is WebDataSource)
            {
                WebDataSource wds = o as WebDataSource;
                if (!string.IsNullOrEmpty(wds.SelectAlias) && !string.IsNullOrEmpty(wds.SelectCommand))
                {
                    refVal.AppendFormat("param.refCmdsql = \"{0}\";", wds.SelectCommand);
                }
            }
            String ShowFields = String.Empty;
            String ShowFieldsWidth = String.Empty;
            String ShowFieldsCaption = String.Empty;
            foreach (ExtSimpleColumn column in this.Columns)
            {
                if (ShowFields != String.Empty)
                    ShowFields += ",";
                if (ShowFieldsWidth != String.Empty)
                    ShowFieldsWidth += ",";
                if (ShowFieldsCaption != string.Empty)
                    ShowFieldsCaption += ",";
                ShowFields += column.DataField;
                ShowFieldsWidth += column.Width;
                ShowFieldsCaption += (column.HeaderText == null || column.HeaderText == string.Empty) ? column.DataField : column.HeaderText;
            }
            refVal.AppendFormat("param.refShowFields = \"{0}\";", ShowFields);
            refVal.AppendFormat("param.refShowFieldsWidth = \"{0}\";", ShowFieldsWidth);
            refVal.AppendFormat("paarm.refShowFieldsCaption = \"{0}\";", ShowFieldsCaption);
            refVal.AppendFormat("param.refBindControlID = \"{0}\";", textBoxID);
            refVal.AppendFormat("param.refBindValueColumn = \"{0}\";", this.ValueField);
            refVal.AppendFormat("param.refBindTextColumn = \"{0}\";", this.DisplayField);
            String strMatchSrcColumns = String.Empty;
            String strMatchDestcontrolIDs = String.Empty;
            foreach (ExtColumnMatch column in this.ColumnMatch)
            {
                if (strMatchSrcColumns != String.Empty)
                    strMatchSrcColumns += "|";
                if (strMatchDestcontrolIDs != String.Empty)
                    strMatchDestcontrolIDs += "|";
                strMatchSrcColumns += "\'" + column.SrcField + "\'";
                strMatchDestcontrolIDs += "\'" + column.DestField + "\'";
            }
            if (strMatchSrcColumns != String.Empty && strMatchDestcontrolIDs != String.Empty)
            {
                refVal.AppendFormat("param.refMatchSrcColumns = [{0}];", strMatchSrcColumns);
                refVal.AppendFormat("param.refMatchDestcontrolIDs = [{0}];", strMatchDestcontrolIDs);
            }
            String strFilterColumns = String.Empty;
            String strFilterControls = String.Empty;
            foreach (ExtColumnMatch column in this.ColumnMatch)
            {
                if (strFilterColumns != String.Empty)
                    strFilterColumns += "|";
                if (strFilterControls != String.Empty)
                    strFilterControls += "|";
                strFilterColumns += "\'" + column.SrcField + "\'";
                strFilterControls += "\'" + column.DestField + "\'";
            }
            if (strFilterColumns != String.Empty && strFilterControls != String.Empty)
            {
                refVal.AppendFormat("param.refFilterColumns = [{0}];", strFilterColumns);
                refVal.AppendFormat("param.refFilterControls = [{0}];", strFilterControls);
            }
            refVal.Append("param.refAutoShow = true;");
            refVal.Append("var GexRef_ID = createGexRef(param);");
            refVal.Append("return GexRef_ID;");
            refVal.Append("}");
            refVal.Append("}");
            refVal.Append("}]");

            return refVal.ToString();
        }

        public string GenTemplate()
        {
            StringBuilder builder = new StringBuilder();
            if (this.Columns.Count > 0)
            {
                builder.Append("<table style=\"width:100%;\">");
                builder.Append("<tr class=\"x-grid3-header\" style=\"font:normal 11px arial, tahoma, helvetica, sans-serif;\">");
                foreach (ExtSimpleColumn column in this.Columns)
                {
                    builder.AppendFormat("<td style=\"width:{0}px\">{1}</td>", column.Width, column.HeaderText);
                }
                builder.Append("</tr>");
                builder.Append("<tpl for=\".\">");
                builder.Append("<tr class=\"x-grid3-row\">");
                foreach (ExtSimpleColumn column in this.Columns)
                {
                    builder.Append("<td>{");
                    if (column.FieldType == "date")
                    {
                        builder.AppendFormat("{0}:date(\"Y/m/d\")", column.DataField);
                    }
                    else
                    {
                        builder.Append(column.DataField);
                    }
                    builder.Append("}</td>");
                }
                builder.Append("</tr>");
                builder.Append("</tpl>");
                builder.Append("</table>");
            }
            else
            {
                builder.AppendFormat("<tpl for=\".\"><div ext:qtip=\"{0}:{{{0}}}\" class=\"x-combo-list-item\">{{{1}}}</div></tpl>",
                    this.ValueField,
                    string.IsNullOrEmpty(this.DisplayField) ? this.ValueField : this.DisplayField);
            }
            return builder.ToString();
        }

        internal string GenRefValConfig(string fieldName, string labelName, int LabelWidth, string parentID)
        {
            String textBoxID = String.IsNullOrEmpty(fieldName) ? this.ClientID + "TextBox" : fieldName;
            String labelID = String.IsNullOrEmpty(labelName) ? this.ClientID + "Label" : parentID + labelName;
            string tpl = this.GenTemplate();
            StringBuilder refVal = new StringBuilder();

            refVal.Append("layout: 'column',");
            refVal.AppendFormat("width: {0},", this.Width);
            //refVal.Append("baseCls: \"x-plain\",");
            refVal.Append("items: [{");
            refVal.AppendFormat("id: '{0}',", labelID);
            if (!String.IsNullOrEmpty(labelName))
                refVal.Append("disabled:true,disabledCls:'info-x-item-disabled',");
            refVal.Append("xtype: 'label',");
            refVal.AppendFormat("width: {0},", LabelWidth + 5);
            refVal.AppendFormat("text: '{0}',", labelName);
            refVal.Append("},{");
            refVal.AppendFormat("id: '{0}',", parentID + textBoxID);
            refVal.AppendFormat("name:'{0}',", textBoxID);
            if (!String.IsNullOrEmpty(fieldName))
                refVal.Append("disabled:true,disabledCls:'info-x-item-disabled',");
            //refVal.Append("disabled:true,hidden:true,disabledCls:'info-x-item-disabled',");
            refVal.Append("xtype: 'textfield',");
            refVal.Append("columnWidth: 0.8,");
            refVal.Append("listeners:");
            refVal.Append("{enable:function(){");
            refVal.AppendFormat("Ext.getCmp('{0}').enable();", textBoxID + "Button");
            refVal.Append("},disable:function(){");
            refVal.AppendFormat("Ext.getCmp('{0}').disable();", textBoxID + "Button");
            refVal.Append("}}");
            refVal.Append("},{");
            //refVal.AppendFormat("id: '{0}',", textBoxID+"_Name");
            //if (!String.IsNullOrEmpty(fieldName))
            //    refVal.Append("disabled:true,disabledClass:'info-x-item-disabled',");
            //refVal.Append("xtype: 'textfield',");
            //refVal.Append("columnWidth: 0.8");
            //refVal.Append("},{");
            refVal.AppendFormat("id: '{0}',", textBoxID + "Button");
            if (!String.IsNullOrEmpty(fieldName))
                refVal.Append("disabled:true,disabledCls:'info-x-item-disabled',");
            refVal.Append("xtype: 'button',");
            refVal.Append("iconCls:'ext_refbtn_icon',");
            refVal.Append("columnWidth: 0.2,");
            refVal.Append("listeners: {");
            refVal.Append("'click': function(){");
            refVal.Append("var param = new setRefvalparam();");
            refVal.AppendFormat("param.refvalID = \"{0}\";", this.ClientID);
            refVal.AppendFormat("param.refTitle = \"{0}\";", this.RefValTitle);
            refVal.Append("param.refModuleName = \"GLModule\";");
            refVal.Append("param.refCommandName = \"cmdRefValUse\";");
            refVal.Append("param.refDynamic = \"Y\";");
            object o = this.GetObjByID(this.DataSourceID);
            if (o != null && o is WebDataSource)
            {
                WebDataSource wds = o as WebDataSource;
                if (!string.IsNullOrEmpty(wds.SelectAlias) && !string.IsNullOrEmpty(wds.SelectCommand))
                {
                    refVal.AppendFormat("param.refCmdsql = \"{0}\";", wds.SelectCommand);
                }
            }
            String ShowFields = String.Empty;
            String ShowFieldsWidth = String.Empty;
            String ShowFieldsCaption = String.Empty;
            foreach (ExtSimpleColumn column in this.Columns)
            {
                if (ShowFields != String.Empty)
                    ShowFields += ",";
                if (ShowFieldsWidth != String.Empty)
                    ShowFieldsWidth += ",";
                if (ShowFieldsCaption != string.Empty)
                    ShowFieldsCaption += ",";
                ShowFields += column.DataField;
                ShowFieldsWidth += column.Width;
                ShowFieldsCaption += (column.HeaderText == null || column.HeaderText == string.Empty) ? column.DataField : column.HeaderText;
            }
            refVal.AppendFormat("param.refShowFields = \"{0}\";", ShowFields);
            refVal.AppendFormat("param.refShowFieldsWidth = \"{0}\";", ShowFieldsWidth);
            refVal.AppendFormat("param.refShowFieldsCaption = \"{0}\";", ShowFieldsCaption);
            refVal.AppendFormat("param.refBindControlID = \"{0}\";", parentID + textBoxID);
            refVal.AppendFormat("param.refBindValueColumn = \"{0}\";", this.ValueField);
            refVal.AppendFormat("param.refBindTextColumn = \"{0}\";", this.DisplayField);
            String strMatchSrcColumns = String.Empty;
            String strMatchDestcontrolIDs = String.Empty;
            foreach (ExtColumnMatch column in this.ColumnMatch)
            {
                if (strMatchSrcColumns != String.Empty)
                    strMatchSrcColumns += "|";
                if (strMatchDestcontrolIDs != String.Empty)
                    strMatchDestcontrolIDs += "|";
                strMatchSrcColumns += "\'" + column.SrcField + "\'";
                strMatchDestcontrolIDs += "\'" + column.DestField + "\'";
            }
            if (strMatchSrcColumns != String.Empty && strMatchDestcontrolIDs != String.Empty)
            {
                refVal.AppendFormat("param.refMatchSrcColumns = [{0}];", strMatchSrcColumns);
                refVal.AppendFormat("param.refMatchDestcontrolIDs = [{0}];", strMatchDestcontrolIDs);
            }
            String strFilterColumns = String.Empty;
            String strFilterControls = String.Empty;
            foreach (ExtColumnMatch column in this.ColumnMatch)
            {
                if (strFilterColumns != String.Empty)
                    strFilterColumns += "|";
                if (strFilterControls != String.Empty)
                    strFilterControls += "|";
                strFilterColumns += "\'" + column.SrcField + "\'";
                strFilterControls += "\'" + column.DestField + "\'";
            }
            if (strFilterColumns != String.Empty && strFilterControls != String.Empty)
            {
                refVal.AppendFormat("param.refFilterColumns = [{0}];", strFilterColumns);
                refVal.AppendFormat("param.refFilterControls = [{0}];", strFilterControls);
            }
            refVal.Append("param.refAutoShow = true;");
            refVal.Append("var GexRef_ID = createGexRef(param);");
            refVal.Append("return GexRef_ID;");
            refVal.Append("}");
            refVal.Append("}");
            refVal.Append("}]");

            return refVal.ToString();
        }

        internal string GenRefValConfig(bool griduse,string fieldName, string labelName, int LabelWidth, string parentID, bool AllowNull, ValidateType validType, string validMethod, string validMsg)
        {
            String textBoxID = String.IsNullOrEmpty(fieldName) ? this.ClientID + "TextBox" : fieldName;
            String labelID = String.IsNullOrEmpty(labelName) ? this.ClientID + "Label" : parentID + labelName;
            string tpl = this.GenTemplate();
            StringBuilder refVal = new StringBuilder();

            refVal.Append("layout: 'hbox',");
            refVal.AppendFormat("width: {0},", this.Width);
            //refVal.Append("baseCls: \"x-plain\",");
            refVal.Append("items: [{");
            refVal.AppendFormat("id: '{0}',", parentID + textBoxID);
            refVal.AppendFormat("name:'{0}',", textBoxID);
            if (!AllowNull)
                refVal.Append("allowBlank:false,");
            string message = "";
            #region validate
            switch (validType)
            {
                case ValidateType.None:
                    break;
                case ValidateType.Method:
                    if (!string.IsNullOrEmpty(validMethod))
                    {
                        if (validMethod.IndexOf('.') != -1)
                        {
                            refVal.AppendFormat("srvValid:'{0}',msg:'{1}',", validMethod, validMsg);
                        }
                        else
                        {
                            refVal.AppendFormat("validator:{0},", validMethod);
                        }
                    }
                    break;
                case ValidateType.Alpha:
                    if (string.IsNullOrEmpty(validMsg))
                    {
                        message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "AjaxTools", "ExtGrid", "ValidAlphaMsg", true);
                    }
                    else
                    {
                        message = validMsg;
                    }
                    refVal.AppendFormat("vtype:'alpha',alphaText:'{0}',", message);
                    break;
                case ValidateType.AlphaNumber:
                    if (string.IsNullOrEmpty(validMsg))
                    {
                        message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "AjaxTools", "ExtGrid", "ValidAlphaNumMsg", true);
                    }
                    else
                    {
                        message = validMsg;
                    }
                    refVal.AppendFormat("vtype:'alphanum',alphanumText:'{0}',", message);
                    break;
                case ValidateType.Email:
                    if (string.IsNullOrEmpty(validMsg))
                    {
                        message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "AjaxTools", "ExtGrid", "ValidEmailMsg", true);
                    }
                    else
                    {
                        message = validMsg;
                    }
                    refVal.AppendFormat("vtype:'email',emailText:'{0}',", message);
                    break;
                case ValidateType.Url:
                    if (string.IsNullOrEmpty(validMsg))
                    {
                        message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "AjaxTools", "ExtGrid", "ValidUrlMsg", true);
                    }
                    else
                    {
                        message = validMsg;
                    }
                    refVal.AppendFormat("vtype:'url',urlText:'{0}',", message);
                    break;
                case ValidateType.Int:
                    if (string.IsNullOrEmpty(validMsg))
                    {
                        message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "AjaxTools", "ExtGrid", "ValidIntMsg", true);
                    }
                    else
                    {
                        message = validMsg;
                    }
                    refVal.AppendFormat("vtype:'isint',vtypeText:'{0}',", message);
                    break;
                case ValidateType.Float:
                    if (string.IsNullOrEmpty(validMsg))
                    {
                        message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "AjaxTools", "ExtGrid", "ValidFloatMsg", true);
                    }
                    else
                    {
                        message = validMsg;
                    }
                    refVal.AppendFormat("vtype:'isfloat',vtypeText:'{0}',", message);
                    break;
                case ValidateType.IPAddress:
                    if (string.IsNullOrEmpty(validMsg))
                    {
                        message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "AjaxTools", "ExtGrid", "ValidIPMsg", true);
                    }
                    else
                    {
                        message = validMsg;
                    }
                    refVal.AppendFormat("vtype:'ip',vtypeText:'{0}',", message);
                    break;
            }
            #endregion
            refVal.Append("xtype: 'textfield',");
            refVal.Append("hidden: true,");
            refVal.Append("flex: 1,");
            refVal.Append("listeners:");
            refVal.Append("{enable:function(){");
            refVal.AppendFormat("Ext.getCmp('{0}').enable();", parentID + textBoxID + "Button");
            refVal.AppendFormat("Ext.getCmp('{0}').enable();", parentID + textBoxID + "_Name");
            refVal.AppendFormat("Ext.getCmp('{0}').hide();", parentID + textBoxID + "_Name");
            refVal.AppendFormat("Ext.getCmp('{0}').show();", parentID + textBoxID);
            refVal.Append("},disable:function(){");
            refVal.AppendFormat("Ext.getCmp('{0}').disable();", parentID + textBoxID + "Button");
            refVal.AppendFormat("Ext.getCmp('{0}').disable();", parentID + textBoxID + "_Name");
            refVal.AppendFormat("Ext.getCmp('{0}').show();", parentID + textBoxID + "_Name");
            refVal.AppendFormat("Ext.getCmp('{0}').hide();", parentID + textBoxID);
            refVal.Append("},change:function(value,a1,a2){");
            //text
            #region columnMatchTest
            refVal.Append("var param = new setRefvalparam();");
            refVal.AppendFormat("param.refvalID = \"{0}\";", this.ClientID);
            refVal.AppendFormat("param.refTitle = \"{0}\";", this.RefValTitle);
            refVal.AppendFormat("param.refParentID = \"{0}\";", parentID);
            refVal.Append("param.refModuleName = \"GLModule\";");
            refVal.Append("param.refCommandName = \"cmdRefValUse\";");
            refVal.Append("param.refDynamic = \"Y\";");
            object o = this.GetObjByID(this.DataSourceID);
            if (o != null && o is WebDataSource)
            {
                WebDataSource wds = o as WebDataSource;
                if (!string.IsNullOrEmpty(wds.SelectAlias) && !string.IsNullOrEmpty(wds.SelectCommand))
                {
                    refVal.AppendFormat("param.refCmdsql = \"{0}\";", wds.SelectCommand);
                }
            }
            refVal.AppendFormat("param.refBindControlID = \"{0}\";", parentID + textBoxID);
            refVal.AppendFormat("param.refBindValueColumn = \"{0}\";", this.ValueField);
            refVal.AppendFormat("param.refBindTextColumn = \"{0}\";", this.DisplayField);
            String strMatchSrcColumnsT = String.Empty;
            String strMatchDestcontrolIDsT = String.Empty;
            foreach (ExtColumnMatch column in this.ColumnMatch)
            {
                if (strMatchSrcColumnsT != String.Empty)
                    strMatchSrcColumnsT += "|";
                if (strMatchDestcontrolIDsT != String.Empty)
                    strMatchDestcontrolIDsT += "|";
                strMatchSrcColumnsT += "\'" + column.SrcField + "\'";
                strMatchDestcontrolIDsT += "\'" + column.DestField + "\'";
            }
            if (strMatchSrcColumnsT != String.Empty && strMatchDestcontrolIDsT != String.Empty)
            {
                refVal.AppendFormat("param.refMatchSrcColumns = [{0}];", strMatchSrcColumnsT);
                refVal.AppendFormat("param.refMatchDestcontrolIDs = [{0}];", strMatchDestcontrolIDsT);
            }
            String strFilterColumns = String.Empty;
            String strFilterControls = String.Empty;
            foreach (ExtColumnMatch column in this.ColumnMatch)
            {
                if (strFilterColumns != String.Empty)
                    strFilterColumns += "|";
                if (strFilterControls != String.Empty)
                    strFilterControls += "|";
                strFilterColumns += "\'" + column.SrcField + "\'";
                strFilterControls += "\'" + column.DestField + "\'";
            }
            if (strFilterColumns != String.Empty && strFilterControls != String.Empty)
            {
                refVal.AppendFormat("param.refFilterColumns = [{0}];", strFilterColumns);
                refVal.AppendFormat("param.refFilterControls = [{0}];", strFilterControls);
            }
            refVal.Append("param.refAutoShow = 'N';");
            refVal.Append("var GexRef_ID = createGexRef(param);");
            refVal.AppendFormat("GexRef_ID.getRefName(a1);");
            refVal.Append("return GexRef_ID;");
            #endregion
            refVal.Append("}}");
            refVal.Append("},{");

            refVal.AppendFormat("id: '{0}',", parentID + textBoxID + "_Name");
            refVal.AppendFormat("name:'{0}',", textBoxID + "_Name");
            refVal.Append("xtype: 'textfield',");
            refVal.Append("flex: 1");
            refVal.Append("},{");

            refVal.AppendFormat("id: '{0}',", parentID + textBoxID + "Button");
            refVal.Append("xtype: 'button',");
            refVal.Append("iconCls:'ext_refbtn_icon',");
            refVal.Append("width: 22,");
            refVal.Append("listeners: {");
            refVal.Append("'click': function(){");
            refVal.Append("var param = new setRefvalparam();");
            refVal.AppendFormat("param.refvalID = \"{0}\";", this.ClientID);
            refVal.AppendFormat("param.refTitle = \"{0}\";", this.RefValTitle);
            refVal.AppendFormat("param.refParentID = \"{0}\";", parentID);
            refVal.Append("param.refModuleName = \"GLModule\";");
            refVal.Append("param.refCommandName = \"cmdRefValUse\";");
            refVal.Append("param.refDynamic = \"Y\";");
            //object o = this.GetObjByID(this.DataSourceID);
            if (o != null && o is WebDataSource)
            {
                WebDataSource wds = o as WebDataSource;
                if (!string.IsNullOrEmpty(wds.SelectAlias) && !string.IsNullOrEmpty(wds.SelectCommand))
                {
                    refVal.AppendFormat("param.refCmdsql = \"{0}\";", wds.SelectCommand);
                }
            }
            String ShowFields = String.Empty;
            String ShowFieldsWidth = String.Empty;
            String ShowFieldsCaption = String.Empty;
            foreach (ExtSimpleColumn column in this.Columns)
            {
                if (ShowFields != String.Empty)
                    ShowFields += ",";
                if (ShowFieldsWidth != String.Empty)
                    ShowFieldsWidth += ",";
                if (ShowFieldsCaption != string.Empty)
                    ShowFieldsCaption += ",";
                ShowFields += column.DataField;
                ShowFieldsWidth += column.Width;
                ShowFieldsCaption += (column.HeaderText == null || column.HeaderText == string.Empty) ? column.DataField : column.HeaderText;
            }
            refVal.AppendFormat("param.refShowFields = \"{0}\";", ShowFields);
            refVal.AppendFormat("param.refShowFieldsWidth = \"{0}\";", ShowFieldsWidth);
            refVal.AppendFormat("param.refShowFieldsCaption = \"{0}\";", ShowFieldsCaption);
            refVal.AppendFormat("param.refBindControlID = \"{0}\";", parentID + textBoxID);
            refVal.AppendFormat("param.refBindValueColumn = \"{0}\";", this.ValueField);
            refVal.AppendFormat("param.refBindTextColumn = \"{0}\";", this.DisplayField);
            String strMatchSrcColumns = String.Empty;
            String strMatchDestcontrolIDs = String.Empty;
            foreach (ExtColumnMatch column in this.ColumnMatch)
            {
                if (strMatchSrcColumns != String.Empty)
                    strMatchSrcColumns += "|";
                if (strMatchDestcontrolIDs != String.Empty)
                    strMatchDestcontrolIDs += "|";
                strMatchSrcColumns += "\'" + column.SrcField + "\'";
                strMatchDestcontrolIDs += "\'" + column.DestField + "\'";
            }
            if (strMatchSrcColumns != String.Empty && strMatchDestcontrolIDs != String.Empty)
            {
                refVal.AppendFormat("param.refMatchSrcColumns = [{0}];", strMatchSrcColumns);
                refVal.AppendFormat("param.refMatchDestcontrolIDs = [{0}];", strMatchDestcontrolIDs);
            }
            //String strFilterColumns = String.Empty;
            //String strFilterControls = String.Empty;
            //foreach (ExtColumnMatch column in this.ColumnMatch)
            //{
            //    if (strFilterColumns != String.Empty)
            //        strFilterColumns += "|";
            //    if (strFilterControls != String.Empty)
            //        strFilterControls += "|";
            //    strFilterColumns += "\'" + column.SrcField + "\'";
            //    strFilterControls += "\'" + column.DestField + "\'";
            //}
            if (strFilterColumns != String.Empty && strFilterControls != String.Empty)
            {
                refVal.AppendFormat("param.refFilterColumns = [{0}];", strFilterColumns);
                refVal.AppendFormat("param.refFilterControls = [{0}];", strFilterControls);
            }
            refVal.Append("param.refAutoShow = true;");
            refVal.Append("var GexRef_ID = createGexRef(param);");
            refVal.Append("return GexRef_ID;");
            refVal.Append("}");
            refVal.Append("}");
            refVal.Append("}]");

            return refVal.ToString();
        }

        internal string GenRefValConfig(string fieldName, string labelName, int LabelWidth, string parentID, bool AllowNull, ValidateType validType, string validMethod, string validMsg)
        {
            String textBoxID = String.IsNullOrEmpty(fieldName) ? this.ClientID + "TextBox" : fieldName;
            String labelID = String.IsNullOrEmpty(labelName) ? this.ClientID + "Label" : parentID + labelName;
            string tpl = this.GenTemplate();
            StringBuilder refVal = new StringBuilder();

            refVal.Append("layout: 'hbox',");
            refVal.AppendFormat("width: {0},", this.Width);
            //refVal.Append("baseCls: \"x-plain\",");
            refVal.Append("items: [{");
            refVal.AppendFormat("id: '{0}',", labelID);
            if (!String.IsNullOrEmpty(labelName))
                refVal.Append("disabled:true,disabledCls:'info-x-item-disabled',");
            refVal.Append("xtype: 'label',");
            refVal.AppendFormat("width: {0},", LabelWidth + 5);
            refVal.AppendFormat("text: '{0}',", labelName);
            refVal.Append("},{");
            refVal.AppendFormat("id: '{0}',", parentID + textBoxID);
            refVal.AppendFormat("name:'{0}',", textBoxID);
            if (!AllowNull)
                refVal.Append("allowBlank:false,");
            string message = "";
            #region validate
            switch (validType)
            {
                case ValidateType.None:
                    break;
                case ValidateType.Method:
                    if (!string.IsNullOrEmpty(validMethod))
                    {
                        if (validMethod.IndexOf('.') != -1)
                        {
                            refVal.AppendFormat("srvValid:'{0}',msg:'{1}',", validMethod, validMsg);
                        }
                        else
                        {
                            refVal.AppendFormat("validator:{0},", validMethod);
                        }
                    }
                    break;
                case ValidateType.Alpha:
                    if (string.IsNullOrEmpty(validMsg))
                    {
                        message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "AjaxTools", "ExtGrid", "ValidAlphaMsg", true);
                    }
                    else
                    {
                        message = validMsg;
                    }
                    refVal.AppendFormat("vtype:'alpha',alphaText:'{0}',", message);
                    break;
                case ValidateType.AlphaNumber:
                    if (string.IsNullOrEmpty(validMsg))
                    {
                        message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "AjaxTools", "ExtGrid", "ValidAlphaNumMsg", true);
                    }
                    else
                    {
                        message = validMsg;
                    }
                    refVal.AppendFormat("vtype:'alphanum',alphanumText:'{0}',", message);
                    break;
                case ValidateType.Email:
                    if (string.IsNullOrEmpty(validMsg))
                    {
                        message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "AjaxTools", "ExtGrid", "ValidEmailMsg", true);
                    }
                    else
                    {
                        message = validMsg;
                    }
                    refVal.AppendFormat("vtype:'email',emailText:'{0}',", message);
                    break;
                case ValidateType.Url:
                    if (string.IsNullOrEmpty(validMsg))
                    {
                        message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "AjaxTools", "ExtGrid", "ValidUrlMsg", true);
                    }
                    else
                    {
                        message = validMsg;
                    }
                    refVal.AppendFormat("vtype:'url',urlText:'{0}',", message);
                    break;
                case ValidateType.Int:
                    if (string.IsNullOrEmpty(validMsg))
                    {
                        message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "AjaxTools", "ExtGrid", "ValidIntMsg", true);
                    }
                    else
                    {
                        message = validMsg;
                    }
                    refVal.AppendFormat("vtype:'isint',vtypeText:'{0}',", message);
                    break;
                case ValidateType.Float:
                    if (string.IsNullOrEmpty(validMsg))
                    {
                        message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "AjaxTools", "ExtGrid", "ValidFloatMsg", true);
                    }
                    else
                    {
                        message = validMsg;
                    }
                    refVal.AppendFormat("vtype:'isfloat',vtypeText:'{0}',", message);
                    break;
                case ValidateType.IPAddress:
                    if (string.IsNullOrEmpty(validMsg))
                    {
                        message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "AjaxTools", "ExtGrid", "ValidIPMsg", true);
                    }
                    else
                    {
                        message = validMsg;
                    }
                    refVal.AppendFormat("vtype:'ip',vtypeText:'{0}',", message);
                    break;
            }
            #endregion
            if (!String.IsNullOrEmpty(fieldName))
                refVal.Append("disabled:true,disabledCls:'info-x-item-disabled',");
            //refVal.Append("disabled:true,hidden:true,disabledCls:'info-x-item-disabled',");
            refVal.Append("xtype: 'textfield',");
            refVal.Append("hidden: true,");
            refVal.Append("flex: 1,");
            refVal.Append("listeners:");
            refVal.Append("{enable:function(){");
            refVal.AppendFormat("Ext.getCmp('{0}').enable();", parentID+ textBoxID + "Button");
            refVal.AppendFormat("Ext.getCmp('{0}').enable();", parentID + textBoxID + "_Name");
            refVal.AppendFormat("Ext.getCmp('{0}').hide();", parentID + textBoxID + "_Name");
            refVal.AppendFormat("Ext.getCmp('{0}').show();", parentID + textBoxID);
            refVal.Append("},disable:function(){");
            refVal.AppendFormat("Ext.getCmp('{0}').disable();",parentID+ textBoxID + "Button");
            refVal.AppendFormat("Ext.getCmp('{0}').disable();", parentID + textBoxID + "_Name");
            refVal.AppendFormat("Ext.getCmp('{0}').show();", parentID + textBoxID + "_Name");
            refVal.AppendFormat("Ext.getCmp('{0}').hide();", parentID + textBoxID );
            refVal.Append("},blur:function(obj){");
            //text
            #region showDisplayMember
            refVal.Append("var param = new setRefvalparam();");
            refVal.AppendFormat("param.refvalID = \"{0}\";", this.ClientID);
            refVal.AppendFormat("param.refTitle = \"{0}\";", this.RefValTitle);
            refVal.AppendFormat("param.refParentID = \"{0}\";", parentID);
            refVal.Append("param.refModuleName = \"GLModule\";");
            refVal.Append("param.refCommandName = \"cmdRefValUse\";");
            refVal.Append("param.refDynamic = \"Y\";");
            object o = this.GetObjByID(this.DataSourceID);
            if (o != null && o is WebDataSource)
            {
                WebDataSource wds = o as WebDataSource;
                if (!string.IsNullOrEmpty(wds.SelectAlias) && !string.IsNullOrEmpty(wds.SelectCommand))
                {
                    refVal.AppendFormat("param.refCmdsql = \"{0}\";", wds.SelectCommand);
                }
            }
            refVal.AppendFormat("param.refBindControlID = \"{0}\";", parentID + textBoxID);
            refVal.AppendFormat("param.refBindValueColumn = \"{0}\";", this.ValueField);
            refVal.AppendFormat("param.refBindTextColumn = \"{0}\";", this.DisplayField);
            String strMatchSrcColumnsT = String.Empty;
            String strMatchDestcontrolIDsT = String.Empty;
            foreach (ExtColumnMatch column in this.ColumnMatch)
            {
                if (strMatchSrcColumnsT != String.Empty)
                    strMatchSrcColumnsT += "|";
                if (strMatchDestcontrolIDsT != String.Empty)
                    strMatchDestcontrolIDsT += "|";
                strMatchSrcColumnsT += "\'" + column.SrcField + "\'";
                strMatchDestcontrolIDsT += "\'" + column.DestField + "\'";
            }
            if (strMatchSrcColumnsT != String.Empty && strMatchDestcontrolIDsT != String.Empty)
            {
                refVal.AppendFormat("param.refMatchSrcColumns = [{0}];", strMatchSrcColumnsT);
                refVal.AppendFormat("param.refMatchDestcontrolIDs = [{0}];", strMatchDestcontrolIDsT);
            }
            String strFilterColumns = String.Empty;
            String strFilterControls = String.Empty;
            foreach (ExtColumnMatch column in this.ColumnMatch)
            {
                if (strFilterColumns != String.Empty)
                    strFilterColumns += "|";
                if (strFilterControls != String.Empty)
                    strFilterControls += "|";
                strFilterColumns += "\'" + column.SrcField + "\'";
                strFilterControls += "\'" + column.DestField + "\'";
            }
            if (strFilterColumns != String.Empty && strFilterControls != String.Empty)
            {
                refVal.AppendFormat("param.refFilterColumns = [{0}];", strFilterColumns);
                refVal.AppendFormat("param.refFilterControls = [{0}];", strFilterControls);
            }
            refVal.Append("param.refAutoShow = 'N';");
            refVal.Append("var GexRef_ID = createGexRef(param);");
            refVal.AppendFormat("GexRef_ID.onblurColumnMatch(obj.value);");
            refVal.Append("return GexRef_ID;");
            #endregion
            refVal.Append("},change:function(obj,newValue,oldValue){");
            //text
            #region columnMatchTest
            refVal.Append("var param = new setRefvalparam();");
            refVal.AppendFormat("param.refvalID = \"{0}\";", this.ClientID);
            refVal.AppendFormat("param.refTitle = \"{0}\";", this.RefValTitle);
            refVal.AppendFormat("param.refParentID = \"{0}\";", parentID);
            refVal.Append("param.refModuleName = \"GLModule\";");
            refVal.Append("param.refCommandName = \"cmdRefValUse\";");
            refVal.Append("param.refDynamic = \"Y\";");
            if (o != null && o is WebDataSource)
            {
                WebDataSource wds = o as WebDataSource;
                if (!string.IsNullOrEmpty(wds.SelectAlias) && !string.IsNullOrEmpty(wds.SelectCommand))
                {
                    refVal.AppendFormat("param.refCmdsql = \"{0}\";", wds.SelectCommand);
                }
            }
            refVal.AppendFormat("param.refBindControlID = \"{0}\";", parentID + textBoxID);
            refVal.AppendFormat("param.refBindValueColumn = \"{0}\";", this.ValueField);
            refVal.AppendFormat("param.refBindTextColumn = \"{0}\";", this.DisplayField);
            if (strMatchSrcColumnsT != String.Empty && strMatchDestcontrolIDsT != String.Empty)
            {
                refVal.AppendFormat("param.refMatchSrcColumns = [{0}];", strMatchSrcColumnsT);
                refVal.AppendFormat("param.refMatchDestcontrolIDs = [{0}];", strMatchDestcontrolIDsT);
            }
            if (strFilterColumns != String.Empty && strFilterControls != String.Empty)
            {
                refVal.AppendFormat("param.refFilterColumns = [{0}];", strFilterColumns);
                refVal.AppendFormat("param.refFilterControls = [{0}];", strFilterControls);
            }
            refVal.Append("param.refAutoShow = 'N';");
            refVal.Append("var GexRef_ID = createGexRef(param);");
            refVal.AppendFormat("GexRef_ID.getRefName(newValue);");
            refVal.Append("return GexRef_ID;");
            #endregion
            refVal.Append("}}");
            refVal.Append("},{");

            refVal.AppendFormat("id: '{0}',", parentID + textBoxID + "_Name");
            refVal.AppendFormat("name:'{0}',", textBoxID + "_Name");
            if (!String.IsNullOrEmpty(fieldName))
                refVal.Append("disabled:true,disabledCls:'info-x-item-disabled',");
            refVal.Append("xtype: 'textfield',");
            refVal.Append("flex: 1");
            refVal.Append("},{");

            refVal.AppendFormat("id: '{0}',",parentID + textBoxID + "Button");
            if (!String.IsNullOrEmpty(fieldName))
                refVal.Append("disabled:true,disabledCls:'info-x-item-disabled',");
            refVal.Append("xtype: 'button',");
            refVal.Append("iconCls:'ext_refbtn_icon',");
            refVal.Append("width: 22,");
            refVal.Append("listeners: {");
            refVal.Append("'click': function(){");
            refVal.Append("var param = new setRefvalparam();");
            refVal.AppendFormat("param.refvalID = \"{0}\";", this.ClientID);
            refVal.AppendFormat("param.refTitle = \"{0}\";", this.RefValTitle);
            refVal.AppendFormat("param.refParentID = \"{0}\";", parentID);
            refVal.Append("param.refModuleName = \"GLModule\";");
            refVal.Append("param.refCommandName = \"cmdRefValUse\";");
            refVal.Append("param.refDynamic = \"Y\";");
            //object o = this.GetObjByID(this.DataSourceID);
            if (o != null && o is WebDataSource)
            {
                WebDataSource wds = o as WebDataSource;
                if (!string.IsNullOrEmpty(wds.SelectAlias) && !string.IsNullOrEmpty(wds.SelectCommand))
                {
                    refVal.AppendFormat("param.refCmdsql = \"{0}\";", wds.SelectCommand);
                }
            }
            String ShowFields = String.Empty;
            String ShowFieldsWidth = String.Empty;
            String ShowFieldsCaption = String.Empty;
            foreach (ExtSimpleColumn column in this.Columns)
            {
                if (ShowFields != String.Empty)
                    ShowFields += ",";
                if (ShowFieldsWidth != String.Empty)
                    ShowFieldsWidth += ",";
                if (ShowFieldsCaption != string.Empty)
                    ShowFieldsCaption += ",";
                ShowFields += column.DataField;
                ShowFieldsWidth += column.Width;
                ShowFieldsCaption += (column.HeaderText == null || column.HeaderText == string.Empty) ? column.DataField : column.HeaderText;
            }
            refVal.AppendFormat("param.refShowFields = \"{0}\";", ShowFields);
            refVal.AppendFormat("param.refShowFieldsWidth = \"{0}\";", ShowFieldsWidth);
            refVal.AppendFormat("param.refShowFieldsCaption = \"{0}\";", ShowFieldsCaption);
            refVal.AppendFormat("param.refBindControlID = \"{0}\";", parentID + textBoxID);
            refVal.AppendFormat("param.refBindValueColumn = \"{0}\";", this.ValueField);
            refVal.AppendFormat("param.refBindTextColumn = \"{0}\";", this.DisplayField);
            String strMatchSrcColumns = String.Empty;
            String strMatchDestcontrolIDs = String.Empty;
            foreach (ExtColumnMatch column in this.ColumnMatch)
            {
                if (strMatchSrcColumns != String.Empty)
                    strMatchSrcColumns += "|";
                if (strMatchDestcontrolIDs != String.Empty)
                    strMatchDestcontrolIDs += "|";
                strMatchSrcColumns += "\'" + column.SrcField + "\'";
                strMatchDestcontrolIDs += "\'" + column.DestField + "\'";
            }
            if (strMatchSrcColumns != String.Empty && strMatchDestcontrolIDs != String.Empty)
            {
                refVal.AppendFormat("param.refMatchSrcColumns = [{0}];", strMatchSrcColumns);
                refVal.AppendFormat("param.refMatchDestcontrolIDs = [{0}];", strMatchDestcontrolIDs);
            }
            //String strFilterColumns = String.Empty;
            //String strFilterControls = String.Empty;
            //foreach (ExtColumnMatch column in this.ColumnMatch)
            //{
            //    if (strFilterColumns != String.Empty)
            //        strFilterColumns += "|";
            //    if (strFilterControls != String.Empty)
            //        strFilterControls += "|";
            //    strFilterColumns += "\'" + column.SrcField + "\'";
            //    strFilterControls += "\'" + column.DestField + "\'";
            //}
            if (strFilterColumns != String.Empty && strFilterControls != String.Empty)
            {
                refVal.AppendFormat("param.refFilterColumns = [{0}];", strFilterColumns);
                refVal.AppendFormat("param.refFilterControls = [{0}];", strFilterControls);
            }
            refVal.Append("param.refAutoShow = true;");
            refVal.Append("var GexRef_ID = createGexRef(param);");
            refVal.Append("return GexRef_ID;");
            refVal.Append("}");
            refVal.Append("}");
            refVal.Append("}]");

            return refVal.ToString();
        }
    }
}
