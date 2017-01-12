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
    public class ExtComboBox : AjaxBaseControl, IAjaxDataSource
    {
        bool _forceSelection = true;
        bool _autoRender = true;
        bool _allowPage = false;
        int _listWidth = 180;
        string _combPanel = "";
        string _dataSourceId = "";
        string _displayField = "";
        string _valueField = "";
        string _emptyText = "select a value...";
        ExtSimpleColumnCollection _columns;
        ExtColumnMatchCollection _columnMatch;
        ExtWhereItemCollection _whereItem;

        #region Properties
        [Category("Infolight")]
        [DefaultValue(true)]
        [Description("True to restrict the selected value to one of the values in the list, false to allow the user to set arbitrary text into the field")]
        public bool ForceSelection
        {
            get { return _forceSelection; }
            set { _forceSelection = value; }
        }

        [Category("Infolight")]
        [DefaultValue(true)]
        public bool AutoRender
        {
            get { return _autoRender; }
            set { _autoRender = value; }
        }

        [Category("Infolight")]
        [DefaultValue(false)]
        public bool AllowPage
        {
            get { return _allowPage; }
            set { _allowPage = value; }
        }

        [Category("Infolight")]
        [DefaultValue(180)]
        public int ListWidth
        {
            get { return _listWidth; }
            set { _listWidth = value; }
        }

        [DefaultValue("")]
        [NotifyParentProperty(true)]
        [Editor(typeof(ExtStaticStringEditor), typeof(UITypeEditor))]
        public string ComboPanel
        {
            get { return _combPanel; }
            set { _combPanel = value; }
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
        [DefaultValue("select a value...")]
        [Description("The default text to display in an empty field")]
        public string EmptyText
        {
            get { return _emptyText; }
            set { _emptyText = value; }
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
        #endregion

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

                this.RenderComboBox();
            }
        }

        public string GenStoreConfig()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("{");
            builder.Append("remoteSort:true,");
            builder.Append("model:Ext.define('");
            builder.Append(this.ID + "ComboModel',{extend:'Ext.data.Model',");
            builder.AppendFormat("fields:{0}", this.GenModelFields());
            builder.Append("}),");
            builder.Append("proxy:{url:'../ExtJs/infolight/ExtGetComboData.ashx',type:'ajax',reader: {totalProperty: 'total', type: 'json', root: 'data'},"); 
            builder.Append("extraParams:{");
            if (!string.IsNullOrEmpty(this.ValueField))
            {
                builder.Append("oper:'select',");
                builder.AppendFormat("fields:'{0}',", this.GenFields(false));
                builder.AppendFormat("keyFilterField:'{0}',", this.ValueField);
            }
            object o = this.GetObjByID(this.DataSourceID);
            if (o != null && o is WebDataSource)
            {
                WebDataSource wds = o as WebDataSource;
                builder.Append(this.GenWhereItemConfig(wds));
                if (!string.IsNullOrEmpty(wds.SelectAlias) && !string.IsNullOrEmpty(wds.SelectCommand))
                {
                    builder.AppendFormat("alias:'{0}',sql:'{1}',", wds.SelectAlias, wds.SelectCommand.Replace("'", @"\'"));
                    builder.AppendFormat("cacheDataSet:'{0}',", this.Page.AppRelativeVirtualPath + this.DataSourceID + ".CommandTable");
                }
                else
                {
                    string remoteName = wds.RemoteName;
                    if (!string.IsNullOrEmpty(remoteName) && remoteName.IndexOf('.') != -1)
                    {
                        builder.AppendFormat("module:'{0}',command:'{1}',", remoteName.Split('.')[0], wds.DataMember);
                        builder.AppendFormat("cacheDataSet:'{0}',", this.Page.AppRelativeVirtualPath + this.DataSourceID);
                    }
                }
            }

            if (builder.ToString().EndsWith(","))
            {
                builder.Remove(builder.Length - 1, 1);
            }
            builder.Append("}}");
            builder.Append("}");
            return builder.ToString();
        }

        public string GenComboBoxConfig(string hiddenName)
        {
            return GenComboBoxConfig(null, hiddenName, false);
        }

        public string GenComboBoxConfig(string valiConfig, string hiddenName)
        {
            return GenComboBoxConfig(valiConfig, hiddenName, false);
        }

        public string GenComboBoxConfig(string valiConfig, string hiddenName, bool asPartOfConfig)
        {
            string tpl = this.GenTemplate();
            StringBuilder builder = new StringBuilder();
            if (!asPartOfConfig)
            {
                builder.Append("{");
            }
            builder.AppendFormat("store:Ext.create('Ext.data.Store',{0}),", this.GenStoreConfig());
            builder.AppendFormat("displayField:'{0}',valueField:'{1}',hiddenName:'{2}',tpl:{3},forceSelection:{4},emptyText:'{5}',listWidth:{6},{7}{8}",
                string.IsNullOrEmpty(this.DisplayField) ? this.ValueField : this.DisplayField,
                this.ValueField,
                hiddenName,
                tpl,
                "false",//this.ForceSelection.ToString().ToLower(),
                this.EmptyText,
                this.ListWidth,
                this.AllowPage ? string.Format("pageSize:{0},", this.getPageSize()) : "",
                string.IsNullOrEmpty(valiConfig) ? "" : (valiConfig + ","));
            //builder.Append("listConfig: {getInnerTpl: function() {return '" + tpl + "';}},");
            if (this.Columns.Count > 0)
            {
                builder.Append("itemSelector:'tr.x-grid3-row',");
                builder.Append("selectedClass:'x-grid3-row x-grid3-row-selected',");
            }
            builder.AppendFormat("columnMatch:{0},", this.GenColumnMatchConfig());
            builder.Append("listeners:{focus:function(field){field.getEl().dom.value=field.getValue();},select:function(combo,record,index){combo.getEl().dom.value=combo.getValue();}},");
            builder.Append("typeAhead:false,");
            builder.Append("selectOnFocus:true,");
            builder.Append("mode:'remote',");
            builder.Append("triggerAction:'all',");
            builder.Append("minChars:1");
            if (!asPartOfConfig)
            {
                builder.Append("}");
            }
            return builder.ToString();
        }

        public string GenColumnMatchConfig()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("[");
            foreach (ExtColumnMatch match in this.ColumnMatch)
            {
                builder.AppendFormat("{{destField:'{0}',srcField:'{1}'}},", match.DestField, match.SrcField);
            }
            if (builder.ToString().EndsWith(","))
            {
                builder.Remove(builder.Length - 1, 1);
            }
            builder.Append("]");
            return builder.ToString();
        }

        public string GenWhereItemConfig(WebDataSource wds)
        {
            StringBuilder builder = new StringBuilder();
            if (this.WhereItem.Count > 0)
            {
                DataTable tab = null;
                if (!string.IsNullOrEmpty(wds.SelectAlias) && !string.IsNullOrEmpty(wds.SelectCommand))
                {
                    tab = wds.CommandTable;
                }
                else
                {
                    tab = wds.InnerDataSet.Tables[wds.DataMember];
                }

                StringBuilder whereBuilder = new StringBuilder();
                StringBuilder mtdBuilder = new StringBuilder();
                foreach (ExtWhereItem wi in this.WhereItem)
                {
                    if (!string.IsNullOrEmpty(wi.Value))
                    {
                        Type type = tab.Columns[wi.FieldName].DataType;
                        string quote = "", value = wi.Value;
                        string condition = wi.Condition;
                        if (GloFix.IsNumeric(type) || type == typeof(Boolean))
                        {
                            if (condition == "%" || condition == "%%")
                            {
                                condition = "=";
                            }
                        }
                        else
                        {
                            quote = "'";
                            if (condition == "%")
                            {
                                condition = "like";
                                value = string.Format("{0}%", value);
                            }
                            else if(condition == "%%")
                            {
                                condition = "like";
                                value = string.Format("%{0}%", value);
                            }
                        }
                        if (condition == "!=")
                        {
                            condition = "<>";
                        }
                        if (whereBuilder.Length == 0)
                        {
                            whereBuilder.AppendFormat("[{0}] {1} {2}{3}{2}", wi.FieldName, condition, quote, value);
                        }
                        else
                        {
                            whereBuilder.AppendFormat(" AND [{0}] {1} {2}{3}{2}", wi.FieldName, condition, quote, value);
                        }
                    }
                    else if (!string.IsNullOrEmpty(wi.ValueMethod))
                    {
                        mtdBuilder.Append("{");
                        mtdBuilder.AppendFormat("field:'{0}',condition:'{1}',method:'{2}'", wi.FieldName, wi.Condition, wi.ValueMethod);
                        mtdBuilder.Append("},");
                    }
                }
                if (mtdBuilder.ToString().EndsWith(","))
                {
                    mtdBuilder.Remove(mtdBuilder.Length - 1, 1);
                }
                if (whereBuilder.Length > 0)
                {
                    builder.AppendFormat("where:\"{0}\",", whereBuilder.ToString());
                }
                if (mtdBuilder.Length > 0)
                {
                    builder.AppendFormat("whereMethods:Ext.encode([{0}]),", mtdBuilder.ToString());
                }
            }
            return builder.ToString();
        }

        public void RenderComboBox()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("var {0}Config=", this.ID);
            builder.Append("{");
            if (this.AutoRender)
            { 
                string renderTo = (string.IsNullOrEmpty(this.ComboPanel)) ? "document.body" : string.Format("'{0}'", this.ComboPanel);
                builder.AppendFormat("comboConfig:{0},renderTo:{1},", this.GenComboBoxConfig(""), renderTo);
            }
            if (builder.ToString().EndsWith(","))
            {
                builder.Remove(builder.Length - 1, 1);
            }
            builder.Append("};");
            builder.AppendFormat("Ext.onReady(function(){{Infolight.ComboBoxHelper.createComboBox({0}Config);}});", this.ID);

            this.Page.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), builder.ToString(), true);
        }

        public string GenFields(bool returnArray)
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(this.ValueField))
            {
                if (this.Columns.Count > 0)
                {
                    List<string> fieldsList = new List<string>();
                    fieldsList.Add(this.ValueField);
                    if (!string.IsNullOrEmpty(this.DisplayField))
                    {
                        fieldsList.Add(this.DisplayField);
                    }
                    foreach (ExtSimpleColumn column in this.Columns)
                    {
                        if (!string.IsNullOrEmpty(column.DataField) && 
                            column.DataField != this.ValueField && 
                            column.DataField != this.DisplayField)
                        {
                            fieldsList.Add(column.DataField);
                        }
                    }
                    foreach (ExtColumnMatch match in this.ColumnMatch)
                    {
                        if (!fieldsList.Contains(match.SrcField))
                        {
                            fieldsList.Add(match.SrcField);
                        }
                    }
                    if (returnArray)
                    {
                        builder.AppendFormat("['{0}']", string.Join("','", fieldsList.ToArray()));
                    }
                    else
                    {
                        builder.Append(string.Join(",", fieldsList.ToArray()));
                    }
                }
                else
                {
                    if (returnArray)
                    {
                        if (string.IsNullOrEmpty(this.DisplayField))
                        {
                            builder.AppendFormat("['{0}']", this.ValueField);
                        }
                        builder.AppendFormat("['{0}','{1}']", this.ValueField, this.DisplayField);
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(this.DisplayField))
                        {
                            builder.AppendFormat("{0}", this.ValueField);
                        }
                        else
                        {
                            builder.AppendFormat("{0},{1}", this.ValueField, this.DisplayField);
                        }
                    }
                }
            }
            return builder.ToString();
        }

        public string GenModelFields()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(this.ValueField))
            {
                if (this.Columns.Count > 0)
                {
                    List<string> fieldsList = new List<string>();
                    fieldsList.Add(this.ValueField);
                    if (!string.IsNullOrEmpty(this.DisplayField))
                    {
                        fieldsList.Add(this.DisplayField);
                    }
                    foreach (ExtSimpleColumn column in this.Columns)
                    {
                        if (!string.IsNullOrEmpty(column.DataField) &&
                            column.DataField != this.ValueField &&
                            column.DataField != this.DisplayField)
                        {
                            fieldsList.Add(column.DataField);
                        }
                    }
                    foreach (ExtColumnMatch match in this.ColumnMatch)
                    {
                        if (!fieldsList.Contains(match.SrcField))
                        {
                            fieldsList.Add(match.SrcField);
                        }
                    }
                    builder.Append("[");
                    foreach (string field in fieldsList)
                    {
                        builder.Append("{name:'" + field + "'},");
                    }
                    if (builder.ToString().EndsWith(","))
                    {
                        builder.Remove(builder.Length - 1, 1);
                    }

                    builder.Append("]");
                }
                else
                {
                    builder.Append("[");
                    if (string.IsNullOrEmpty(this.DisplayField))
                    {
                        builder.Append("{name:'" + ValueField + "'}");
                    }
                    else
                    {
                        builder.Append("{name:'" + ValueField + "'},");
                        builder.Append("{name:'" + DisplayField + "'}");
                    }
                    builder.Append("]");
                }
            }
            return builder.ToString();
        }


        public string GenTemplate()
        {
            StringBuilder builder = new StringBuilder();
            if (this.Columns.Count > 0)
            {
                builder.Append("Ext.create(\"Ext.XTemplate\", '");
                builder.Append("<div style=\"width:"+this.ListWidth+";\">");
                builder.Append("<table style=\"width:100%;\">");
                builder.Append("<tr class=\"x-grid3-header\" style=\"font:normal 11px arial, tahoma, helvetica, sans-serif;\">");
                foreach (ExtSimpleColumn column in this.Columns)
                {
                    builder.AppendFormat("<td style=\"width:{0}px\">{1}</td>", column.Width, column.HeaderText);
                }
                builder.Append("</tr>");
                builder.Append("<tpl for=\".\">");
                builder.Append("<tr class=\"x-boundlist-item\">");
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
                builder.Append("</div>");
                builder.Append("')");
            }
            else
            {
                builder.AppendFormat("'<tpl for=\".\"><div ext:qtip=\"{0}:{{{0}}}\" class=\"x-combo-list-item\">{{{1}}}</div></tpl>'",
                    this.ValueField,
                    string.IsNullOrEmpty(this.DisplayField) ? this.ValueField : this.DisplayField);
            }
            return builder.ToString();
        }

        int getPageSize()
        {
            object o = this.GetObjByID(this.DataSourceID);
            if (o != null && o is WebDataSource)
            {
                WebDataSource wds = o as WebDataSource;
                if (!string.IsNullOrEmpty(wds.SelectAlias) && !string.IsNullOrEmpty(wds.SelectCommand))
                {
                    return wds.CommandPacketRecords;
                }
                else
                {
                    return wds.PacketRecords;
                }
            }
            return -1;
        }
    }

    public class ExtTriggerDesigner : ControlDesigner
    {
        public override string GetDesignTimeHtml()
        {
            if (this.Component is ExtComboBox)
            {
                ExtComboBox cmb = this.Component as ExtComboBox;
                return string.Format("<div style='width:130px;'><input type='text' value='{0}' style='width:113px;background:#fff url(../ExtJs/resources/images/default/form/text-bg.gif);border:1px solid #B5B8C8;'/><img style='width:17px;height:19px;background:transparent url(../ExtJs/resources/images/default/form/trigger.gif);border-bottom:1px solid #B5B8C8;' src='http://extjs.com/s.gif'/></div>", cmb.ID);
            }
            else if (this.Component is ExtRefButton)
            {
                ExtRefButton refButton = this.Component as ExtRefButton;
                return string.Format("<div style='width:130px;'><input type='text' value='{0}' style='width:106px;background:#fff url(../ExtJs/resources/images/default/form/text-bg.gif);border:1px solid #B5B8C8;'/><img style='width:24px;height:21px;background:transparent url(../Image/Ext/RefButton/refButtonAppearance.png);' src='http://extjs.com/s.gif'/></div>", refButton.ID);
            }
            else if (this.Component is ExtRefVal)
            {
                ExtRefVal refVal = this.Component as ExtRefVal;
                return string.Format("<div style='width:130px;'><input type='text' value='{0}' style='width:106px;background:#fff url(../ExtJs/resources/images/default/form/text-bg.gif);border:1px solid #B5B8C8;'/><img style='width:24px;height:21px;background:transparent url(../Image/refval/RefVal.gif);' src='http://extjs.com/s.gif'/></div>", refVal.ID);

            }
            else
            {
                return base.GetDesignTimeHtml();
            }
        }
    }

    public class ExtWhereItem : InfoOwnerCollectionItem
    {
        string _fieldName = "";
        string _condition = "=";
        string _value = "";
        string _valueMethod = "";

        [NotifyParentProperty(true)]
        [DefaultValue("")]
        [Editor(typeof(ExtStaticStringEditor), typeof(UITypeEditor))]
        public string FieldName
        {
            get { return _fieldName; }
            set { _fieldName = value; }
        }

        [NotifyParentProperty(true)]
        [DefaultValue("=")]
        [Editor(typeof(ExtStaticStringEditor), typeof(UITypeEditor))]
        public string Condition
        {
            get {return _condition;}
            set{ _condition = value;}
        }

        [DefaultValue("")]
        [NotifyParentProperty(true)]
        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }

        [DefaultValue("")]
        [NotifyParentProperty(true)]
        public string ValueMethod
        {
            get { return _valueMethod; }
            set { _valueMethod = value; }
        }

        [NotifyParentProperty(true)]
        public override string Name
        {
            get { return _fieldName; }
            set { _fieldName = value; }
        }

        public override string ToString()
        {
            return _fieldName;
        }
    }

    [ParseChildren(true)]
    [PersistChildren(false)]
    [Designer(typeof(ExtTriggerDesigner), typeof(IDesigner))]
    public class ExtRefButton : AjaxBaseControl
    {
        string _modalPanel = "";
        string _destinDataSourceID = "";
        string _sourceDataSourceID = "";
        ExtRefButtonColumnMatchCollection _columnMatch;

        [Category("Infolight")]
        [DefaultValue("")]
        [Editor(typeof(ExtStaticStringEditor), typeof(UITypeEditor))]
        public string ModalPanelID
        {
            get { return _modalPanel; }
            set { _modalPanel = value; }
        }

        [Category("Infolight")]
        [DefaultValue("")]
        [Editor(typeof(ExtStaticStringEditor), typeof(UITypeEditor))]
        public string DestinDataSourceID
        {
            get { return _destinDataSourceID; }
            set { _destinDataSourceID = value; }
        }

        [Category("Infolight")]
        [DefaultValue("")]
        [Editor(typeof(ExtStaticStringEditor), typeof(UITypeEditor))]
        public string SourceDataSourceID
        {
            get { return _sourceDataSourceID; }
            set { _sourceDataSourceID = value; }
        }

        [Category("Infolight")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [TypeConverter(typeof(CollectionConverter))]
        [NotifyParentProperty(true)]
        public ExtRefButtonColumnMatchCollection ColumnMatch
        {
            get
            {
                if (_columnMatch == null)
                    _columnMatch = new ExtRefButtonColumnMatchCollection(this, typeof(ExtRefButtonColumnMatch));
                return _columnMatch;
            }
        }

        public string GenClickEventScope()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("{");
            UpdatePanel upan = this.GetModalUpdatePanel() as UpdatePanel;
            if (upan != null && !string.IsNullOrEmpty(this.ModalPanelID))
            {
                builder.AppendFormat("targetPan:'{0}',updatePanId:'{1}'",
                    this.ModalPanelID,
                    upan.UniqueID);
            }
            builder.Append("}");
            return builder.ToString();
        }

        public void RefButtonShowModal()
        {
            UpdatePanel upan = this.GetModalUpdatePanel() as UpdatePanel;
            if (upan != null && this.Page.Request.Form["__EVENTTARGET"] == upan.UniqueID)
            {
                string script = string.Format("var behavior=$find('{0}behavior');if(behavior){{behavior.show();}}", this.ModalPanelID);
                ScriptManager.RegisterStartupScript(upan, this.GetType(), Guid.NewGuid().ToString(), script, true);
            }
        }

        public void RefButtonSubmit()
        {
            RefButtonSubmit(null);
        }

        public void RefButtonSubmit(Dictionary<string, object> customerRefValues)
        {
            UpdatePanel upan = this.GetModalUpdatePanel() as UpdatePanel;
            if (upan != null)
            {
                Dictionary<string, object> refValues = new Dictionary<string, object>();
                if (customerRefValues != null && customerRefValues.Count > 0)
                {
                    refValues = customerRefValues;
                }
                StringBuilder builder = new StringBuilder();
                builder.Append("var matches={");
                if (!string.IsNullOrEmpty(this.SourceDataSourceID))
                {
                    CompositeDataBoundControl dataBoundControl = null;
                    foreach (Control ctrl in upan.ContentTemplateContainer.Controls)
                    {
                        if (ctrl is CompositeDataBoundControl && (ctrl as CompositeDataBoundControl).DataSourceID == this.SourceDataSourceID)
                        {
                            dataBoundControl = ctrl as CompositeDataBoundControl;
                        }
                    }
                    if (dataBoundControl != null)
                    {
                        DataRowView rowView = null;
                        WebDataSource wds = this.GetObjByID(dataBoundControl.DataSourceID) as WebDataSource;
                        if (wds != null)
                        {
                            if (dataBoundControl is FormView)
                            {
                                rowView = wds.View[((FormView)dataBoundControl).DataItemIndex];
                            }
                            else if (dataBoundControl is DetailsView)
                            {
                                rowView = wds.View[((DetailsView)dataBoundControl).DataItemIndex];
                            }
                            else if (dataBoundControl is GridView)
                            {
                                GridView gridView = dataBoundControl as GridView;
                                if (gridView.SelectedRow != null)
                                {
                                    rowView = wds.View[gridView.SelectedRow.DataItemIndex];
                                }
                            }
                            foreach (ExtRefButtonColumnMatch match in this.ColumnMatch)
                            {
                                if (!refValues.ContainsKey(match.DestField))
                                {
                                    if (!string.IsNullOrEmpty(match.SrcField))
                                    {
                                        refValues.Add(match.DestField, rowView[match.SrcField]);
                                    }
                                    else if (!string.IsNullOrEmpty(match.SrcControlId))
                                    {
                                        object value = null;
                                        Control ctrl = upan.ContentTemplateContainer.FindControl(match.SrcControlId);
                                        if (ctrl != null)
                                        {
                                            PropertyInfo prop = ctrl.GetType().GetProperty(match.SrcControlValueProperty);
                                            if (prop != null)
                                            {
                                                value = prop.GetValue(ctrl, null);
                                            }
                                        }
                                        refValues.Add(match.DestField, value);
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    foreach (ExtRefButtonColumnMatch match in this.ColumnMatch)
                    {
                        if (!refValues.ContainsKey(match.DestField) && !string.IsNullOrEmpty(match.SrcControlId))
                        {
                            object value = null;
                            Control ctrl = upan.ContentTemplateContainer.FindControl(match.SrcControlId);
                            if (ctrl != null)
                            {
                                PropertyInfo prop = ctrl.GetType().GetProperty(match.SrcControlValueProperty);
                                if (prop != null)
                                {
                                    value = prop.GetValue(ctrl, null);
                                }
                            }
                            refValues.Add(match.DestField, value);
                        }
                    }
                }

                foreach (KeyValuePair<string, object> pair in refValues)
                {
                    Type type = pair.Value.GetType();
                    if (GloFix.IsNumeric(type))
                    {
                        builder.AppendFormat("{0}:{1},", pair.Key, pair.Value.ToString());
                    }
                    else if (type == typeof(DateTime))
                    {
                        if (pair.Value != null)
                        {
                            builder.AppendFormat("{0}:Infolight.convertDate('{1}'),", pair.Key, ((DateTime)pair.Value).ToShortDateString());
                        }
                        else
                        {
                            builder.AppendFormat("{0}:null),", pair.Key);
                        }
                    }
                    else if (type == typeof(bool))
                    {
                        builder.AppendFormat("{0}:{1},", pair.Key, pair.Value.ToString().ToLower());
                    }
                    else
                    {
                        builder.AppendFormat("{0}:'{1}',", pair.Key, pair.Value.ToString());
                    }
                }
                if (builder.ToString().EndsWith(","))
                {
                    builder.Remove(builder.Length - 1, 1);
                }
                builder.Append("};");
                string formPan = GetOwnerFormPanel();
                if (!string.IsNullOrEmpty(formPan))
                {
                    builder.AppendFormat("Ext.getCmp('{0}').getForm().setValues(matches);", formPan);
                }
                builder.AppendFormat("var behavior=$find('{0}behavior');if(behavior){{behavior.hide();}}", this.ModalPanelID);
                if (builder.Length > 0)
                {
                    ScriptManager.RegisterStartupScript(upan, this.GetType(), Guid.NewGuid().ToString(), builder.ToString(), true);
                }
            }
        }

        string GetOwnerFormPanel()
        {
            if (!string.IsNullOrEmpty(this.DestinDataSourceID))
            {
                string formViewId = "";
                foreach (Control ctrl in this.Page.Form.Controls)
                {
                    if (ctrl is AjaxFormView)
                    {
                        AjaxFormView formView = ctrl as AjaxFormView;
                        foreach (AjaxFormField field in formView.Fields)
                        {
                            if (field.Editor == ExtGridEditor.RefButton && field.EditControlId == this.ID)
                            {
                                formViewId = formView.ID;
                                break;
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(formViewId))
                    {
                        break;
                    }
                }
                if (!string.IsNullOrEmpty(formViewId))
                {
                    foreach (Control ctrl in this.Page.Form.Controls)
                    {
                        if (ctrl is AjaxLayout)
                        {
                            AjaxLayout layout = ctrl as AjaxLayout;

                            foreach (MultiViewItem item in layout.Masters)
                            {
                                if (item.ControlId == formViewId)
                                {
                                    return layout.ID;
                                }
                            }
                        }
                    }
                }
            }
            return "";
        }

        public UpdatePanel GetModalUpdatePanel()
        {
            ExtModalPanel pan = this.GetObjByID(this.ModalPanelID) as ExtModalPanel;
            if (pan != null)
            {
                foreach (Control ctrl in pan.Controls)
                {
                    if (ctrl is UpdatePanel)
                    {
                        return (UpdatePanel)ctrl;
                    }
                }
            }
            return null;
        }
    }
}