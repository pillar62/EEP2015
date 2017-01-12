using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Drawing.Design;
using System.Text;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;
using Srvtools;
using System.Collections;

namespace AjaxTools
{
    [ParseChildren(true)]
    [PersistChildren(false)]
    [Designer(typeof(ExtGridDesigner), typeof(IDesigner))]
    public class AjaxGridView : AjaxBaseControl, IAjaxDataSource
    {
        public AjaxGridView()
        {
        }

        int _queryColumns = 1;
        int _queryInnerPanelWidth = 265;
        int _queryInnerPanelHeight = 400;
        int _editFormPanelWidth = 580;
        int _editFormPanelHeight = 400;
        string _queryTitle = "";
        string _dataSourceId = "";
        string _expanderRowTemplate = "";
        string _edtPanId = "";
        string _queryPanId = "";
        bool _genRowNumberer = false;
        bool _allowExpandRow = true;
        bool _getServerText = true;
        ExtViewPagingSet _pagingSet = null;
        ExtGridGridSet _gridSet = null;
        ExtGridColumnCollection _columns;
        ExtGridToolItemCollection _toolItems;
        ExtQueryFieldCollection _queryFields;
        String _editFormViewID;

        #region Properties
        [Category("Infolight")]
        [DefaultValue(1)]
        public int QueryColumns
        {
            get { return _queryColumns; }
            set { _queryColumns = value; }
        }

        [Category("Infolight")]
        [DefaultValue(265)]
        public int QueryInnerPanelWidth
        {
            get { return _queryInnerPanelWidth; }
            set { _queryInnerPanelWidth = value; }
        }

        [Category("Infolight")]
        [DefaultValue(400)]
        public int QueryInnerPanelHeight
        {
            get { return _queryInnerPanelHeight; }
            set { _queryInnerPanelHeight = value; }
        }

        [Category("Infolight")]
        [DefaultValue(580)]
        public int EditFormPanelWidth
        {
            get { return _editFormPanelWidth; }
            set { _editFormPanelWidth = value; }
        }

        [Category("Infolight")]
        [DefaultValue(400)]
        public int EditFormPanelHeight
        {
            get { return _editFormPanelHeight; }
            set { _editFormPanelHeight = value; }
        }

        [Category("Infolight")]
        [DefaultValue("")]
        public string QueryTitle
        {
            get { return _queryTitle; }
            set { _queryTitle = value; }
        }

        [Category("Infolight")]
        [DefaultValue("")]
        [Editor(typeof(ExtStaticStringEditor), typeof(UITypeEditor))]
        public string EditFormViewID
        {
            get { return _editFormViewID; }
            set { _editFormViewID = value; }
        }

        [Category("Infolight")]
        [DefaultValue("")]
        [Editor(typeof(DataSourceEditor), typeof(UITypeEditor))]
        public string DataSourceID
        {
            get { return _dataSourceId; }
            set { _dataSourceId = value; }
        }

        [Category("Infolight")]
        [DefaultValue("")]
        public string ExpanderRowTemplateHtml
        {
            get { return _expanderRowTemplate; }
            set { _expanderRowTemplate = value; }
        }

        [Category("Infolight")]
        [DefaultValue("")]
        [Editor(typeof(ExtStaticStringEditor), typeof(UITypeEditor))]
        public string EditPanelID
        {
            get { return _edtPanId; }
            set { _edtPanId = value; }
        }

        [Category("Infolight")]
        [DefaultValue("")]
        [Editor(typeof(ExtStaticStringEditor), typeof(UITypeEditor))]
        public string QueryPanelID
        {
            get { return _queryPanId; }
            set { _queryPanId = value; }
        }

        [Category("Infolight")]
        [DefaultValue(false)]
        public bool GenRowNumberer
        {
            get { return _genRowNumberer; }
            set { _genRowNumberer = value; }
        }

        [Category("Infolight")]
        [DefaultValue(true)]
        public bool AllowExpandRow
        {
            get { return _allowExpandRow; }
            set { _allowExpandRow = value; }
        }

        [Category("Infolight")]
        [DefaultValue(true)]
        public bool GetServerText
        {
            get { return _getServerText; }
            set { _getServerText = value; }
        }

        [Category("Infolight")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [NotifyParentProperty(true)]
        public ExtViewPagingSet PagingSet
        {
            get
            {
                if (_pagingSet == null)
                {
                    _pagingSet = new ExtViewPagingSet(this);
                }
                return _pagingSet;
            }
            set { _pagingSet = value; }
        }

        [Category("Infolight")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [NotifyParentProperty(true)]
        public ExtGridGridSet GridSet
        {
            get
            {
                if (_gridSet == null)
                {
                    _gridSet = new ExtGridGridSet(this);
                }
                return _gridSet;
            }
            set { _gridSet = value; }
        }

        [Category("Infolight")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [TypeConverter(typeof(CollectionConverter))]
        [NotifyParentProperty(true)]
        public ExtGridColumnCollection Columns
        {
            get
            {
                if (_columns == null)
                    _columns = new ExtGridColumnCollection(this, typeof(ExtGridColumn));
                return _columns;
            }
        }

        [Category("Layout")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [NotifyParentProperty(true)]
        public ExtGridToolItemCollection ToolItems
        {
            get
            {
                if (_toolItems == null)
                {
                    _toolItems = new ExtGridToolItemCollection(this, typeof(ExtGridToolItem));
                }
                return _toolItems;
            }
        }

        [Category("Infolight")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [TypeConverter(typeof(CollectionConverter))]
        [NotifyParentProperty(true)]
        public ExtQueryFieldCollection QueryFields
        {
            get
            {
                if (_queryFields == null)
                    _queryFields = new ExtQueryFieldCollection(this, typeof(ExtQueryField));
                return _queryFields;
            }
        }


        #endregion

        //public void SavePersonalSettings(string remark)
        //{
        //    string userId = CliUtils.fLoginUser;
        //}

        //public void LoadPersonalSettings()
        //{
        //}

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!Page.IsPostBack && !this.RenderToExtComp())
            {
                this.RenderGrid(true);
            }
        }

        public string GenColumnsJsonArray()
        {
            ExtGridColumnCollection columns = this.Columns;
            if (columns.Count > 0)
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("[");
                if (this.GenRowNumberer)
                {
                    builder.Append("Ext.create('Ext.grid.RowNumberer'),");
                }
                for (int i = 0; i < columns.Count; i++)
                {
                    if (columns[i].Editor == ExtGridEditor.CheckBox && columns[i].FieldType == "boolean")
                    {
                        builder.Append("new Ext.ux.CheckColumn({");
                        builder.Append("disabled:true,");
                        builder.Append("editable:false,");
                        //builder.Append("xtype:'checkcolumn',");
                        builder.Append("editor:{xtype:'checkbox',checked:'{check}',cls:'x-grid-checkheader-editor'},");
                    }
                    else
                    {
                        builder.Append("{");
                    }
                    if (!string.IsNullOrEmpty(columns[i].ColumnName))
                    {
                        builder.AppendFormat("id:'{0}',", columns[i].ColumnName);
                    }
                    if (!string.IsNullOrEmpty(columns[i].HeaderText))
                    {
                        builder.AppendFormat("header:'{0}',", columns[i].HeaderText);
                    }
                    if (!string.IsNullOrEmpty(columns[i].DataField))
                    {
                        builder.AppendFormat("dataIndex:'{0}',", columns[i].DataField);
                    }
                    if (this.GridSet.AutoFillingColumn == columns[i].ColumnName)
                    {
                        builder.Append("flex:1,");
                    }
                    if (!string.IsNullOrEmpty(columns[i].TextAlign) && columns[i].TextAlign != "left")
                    {
                        builder.AppendFormat("align:'{0}',", columns[i].TextAlign);
                    }
                    if (!string.IsNullOrEmpty(columns[i].Formatter))
                    {
                        if (columns[i].Editor == ExtGridEditor.DateTimePicker)
                        {
                            // renderer控制顯示在grid-cell內日期的格式
                            builder.AppendFormat("renderer:Ext.util.Format.dateRenderer('{0}'),", columns[i].Formatter);
                        }
                        else if (columns[i].Editor == ExtGridEditor.ComboBox && !string.IsNullOrEmpty(columns[i].EditControlId) && columns[i].Formatter.StartsWith("ref:"))
                        {
                            ExtComboBox cmb = this.GetObjByID(columns[i].EditControlId) as ExtComboBox;
                            if (cmb != null)
                            {
                                builder.Append("renderer:function(value, metaData, record, rowIndex, colIndex, store){return Infolight.GridHelper.combColumnFomatter.apply({");
                                builder.AppendFormat("column:'{0}',showfield:'{1}'",
                                    columns[i].DataField,
                                    columns[i].Formatter.Remove(0, 4).Trim()); //ref:xxxx
                                builder.Append("},[value, metaData, record, rowIndex, colIndex, store]);},");
                            }
                        }
                        else
                        {
                            if (columns[i].FieldType == "int" || columns[i].FieldType == "float")
                            {
                                builder.AppendFormat("renderer:Ext.util.Format.numberRenderer('{0}'),", columns[i].Formatter);
                            }
                            else
                            {
                                builder.AppendFormat("renderer:{0},", columns[i].Formatter);
                            }
                        }
                    }
                    if (!columns[i].Visible)
                    {
                        builder.Append("hidden:true,");
                    }
                    //editable好像无效了，这边的稳妥做法应该是在readonly时不添加editor才对，见下面
                    //if (columns[i].ReadOnly)
                    //{
                    //    builder.Append("editable:false,");
                    //}
                    if (columns[i].AllowSort)
                    {
                        builder.Append("sortable:true,");
                    }
                    if (!columns[i].Resizable)
                    {
                        builder.Append("resizable:false,");
                    }
                    //if (columns[i].Editor == ExtGridEditor.CheckBox && columns[i].FieldType == "boolean")
                    //{

                    //    builder.Append("disabled:true,");
                    //    //builder.Append("xtype:'checkcolumn',");
                    //    builder.Append("xtype:'checkcolumn',editor:{xtype:'checkbox',checked:'{check}',cls:'x-grid-checkheader-editor'},");
                    //}
                    if (columns[i].Editor == ExtGridEditor.CheckBox && columns[i].FieldType == "boolean")
                    {
                        builder.AppendFormat("width:{0}}}),", columns[i].Width.ToString());
                    }
                    else if (columns[i].ReadOnly)//这里先这样处理，使得readonly不恩能够编辑，同时应该还要加上readonly的css
                    {
                        builder.AppendFormat("width:{0}}},", columns[i].Width.ToString());
                    }
                    else
                    {
                        string valiConfig = ExtValidator.GenValidateConfig(columns[i].AllowNull, columns[i].ValidType, columns[i].ValidMethod, columns[i].ValidText);
                        switch (columns[i].Editor)
                        {
                            case ExtGridEditor.TextBox:
                                builder.AppendFormat("editor:{{{0}}},",
                                    string.IsNullOrEmpty(valiConfig) ? "" : valiConfig);
                                break;
                            case ExtGridEditor.DateTimePicker:
                                // format控制datepicker內日期的格式
                                if (string.IsNullOrEmpty(valiConfig))
                                {
                                    builder.AppendFormat("editor:new Ext.form.DateField({{format:'{0}'}}),",
                                        columns[i].Formatter);
                                }
                                else
                                {
                                    builder.AppendFormat("editor:new Ext.form.DateField({{format:'{0}',{1}}}),",
                                        columns[i].Formatter,
                                        valiConfig);
                                }
                                break;
                            case ExtGridEditor.ComboBox:
                                if (!string.IsNullOrEmpty(columns[i].EditControlId))
                                {
                                    ExtComboBox cmb = this.GetObjByID(columns[i].EditControlId) as ExtComboBox;
                                    if (cmb != null)
                                    {
                                        //builder.AppendFormat("editor:new Ext.form.field.ComboBox({0}),", cmb.GenComboBoxConfig(valiConfig, columns[i].DataField));
                                        builder.AppendFormat("editor:new Infolight.ComboBox({0}),", cmb.GenComboBoxConfig(valiConfig, columns[i].DataField));
                                    }
                                }
                                break;
                            case ExtGridEditor.RefVal:
                                if (!string.IsNullOrEmpty(columns[i].EditControlId))
                                {
                                    //Test

                                    //builder.AppendFormat("editor:new Infolight.TextFieldTest(),");

                                    //endtest
                                    ExtRefVal refval = this.GetObjByID(columns[i].EditControlId) as ExtRefVal;
                                    if (refval != null)
                                    {
                                        string buildapp = refval.GenRefValConfig(true, columns[i].DataField, columns[i].HeaderText, columns[i].Width, this.ID, columns[i].AllowNull, columns[i].ValidType, columns[i].ValidMethod, columns[i].ValidText);
                                        builder.AppendFormat("editor:{{{0}}},", buildapp);
                                        //builder.AppendFormat("editor:new Ext.Panel({{{0}}}),", buildapp);
                                    }
                                }
                                break;
                        }
                        builder.AppendFormat("width:{0}}},", columns[i].Width.ToString());
                    }
                }
                if (builder.ToString().EndsWith(","))
                {
                    builder.Remove(builder.Length - 1, 1);
                }
                builder.Append("]");
                return builder.ToString();
            }
            return "";
        }

        public string GenFieldsJsonArray()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("[");
            ExtGridColumnCollection columns = this.Columns;
            if (columns.Count > 0)
            {
                for (int i = 0; i < columns.Count; i++)
                {
                    if (!string.IsNullOrEmpty(columns[i].DataField))
                    {
                        builder.Append("{");
                        builder.AppendFormat("name:'{0}'", columns[i].DataField);
                        if (!string.IsNullOrEmpty(columns[i].FieldType) && columns[i].FieldType != "string")
                        {
                            builder.AppendFormat(",type:'{0}'", columns[i].FieldType);
                        }
                        if (i == columns.Count - 1)
                        {
                            builder.Append("}");
                        }
                        else
                        {
                            builder.Append("},");
                        }
                    }
                }
            }
            builder.Append("]");
            return builder.ToString();
        }

        public string GenToolItemsJsonArray()
        {
            StringBuilder builder = new StringBuilder();
            if (this.ToolItems.Count > 0)
            {
                builder.Append("[");
                for (int i = 0; i < this.ToolItems.Count; i++)
                {
                    switch (this.ToolItems[i].ToolItemType)
                    {
                        case ExtGridToolItemType.Button:
                            builder.Append("{");
                            if (!string.IsNullOrEmpty(this.ToolItems[i].ButtonName))
                            {
                                builder.AppendFormat("id:'{0}{1}',", this.ID, this.ToolItems[i].ButtonName);
                            }
                            if (!string.IsNullOrEmpty(this.ToolItems[i].Text))
                            {
                                string text = this.ToolItems[i].Text;
                                if (this.GetServerText)
                                {
                                    string[] toolTexts = SysMsg.GetSystemMessage(CliUtils.fClientLang, "AjaxTools", "ExtGrid", "ToolItems", true).Split(',');
                                    switch (this.ToolItems[i].SysHandlerType)
                                    {
                                        case ExtGridSystemHandler.Add:          text = toolTexts[0]; break;
                                        case ExtGridSystemHandler.Edit:         text = toolTexts[1]; break;
                                        case ExtGridSystemHandler.Delete:       text = toolTexts[2]; break;
                                        case ExtGridSystemHandler.Save:         text = toolTexts[3]; break;
                                        case ExtGridSystemHandler.Abort:        text = toolTexts[4]; break;
                                        case ExtGridSystemHandler.Query:        text = toolTexts[6]; break;
                                        case ExtGridSystemHandler.Refresh:      text = toolTexts[5]; break;
                                        case ExtGridSystemHandler.SavePersonal: text = toolTexts[9]; break;
                                        case ExtGridSystemHandler.LoadPersonal: text = toolTexts[10]; break;
                                    }
                                }
                                builder.AppendFormat("text:'{0}',", text);
                            }
                            if (!string.IsNullOrEmpty(this.ToolItems[i].IconUrl))
                            {
                                builder.AppendFormat("icon:'{0}',", this.ResolveClientUrl(this.ToolItems[i].IconUrl));
                            }
                            if (!string.IsNullOrEmpty(this.ToolItems[i].CssClass))
                            {
                                builder.AppendFormat("cls:'{0}',", this.ToolItems[i].CssClass);
                            }
                            WebDataSource wds = this.GetObjByID(this.DataSourceID) as WebDataSource;
                            builder.Append("handler:function(sender, args){");
                            string custHandler = this.ToolItems[i].Handler;
                            if (!string.IsNullOrEmpty(custHandler))
                            {
                                builder.AppendFormat("var c={0}();if(c===false){{return;}}", custHandler);
                            }
                            switch (this.ToolItems[i].SysHandlerType)
                            {
                                case ExtGridSystemHandler.Add:
                                    #region add
                                    if (wds != null)
                                    {
                                        DataTable tab = wds.InnerDataSet.Tables[wds.DataMember];
                                        string defaultValues = this.GenDefaultValues(wds);

                                        if (!string.IsNullOrEmpty(this.EditFormViewID))
                                        {
                                            builder.AppendFormat("Ext.getCmp('{0}').addFormViewModal('{1}',{2})", this.ID,this.EditFormViewID, string.IsNullOrEmpty(defaultValues) ? "{}" : defaultValues);
                                        }
                                        else if (string.IsNullOrEmpty(this.EditPanelID))
                                        {
                                            builder.AppendFormat("Ext.getCmp('{0}').addRow({1}", this.ID, string.IsNullOrEmpty(defaultValues) ? "{}" : defaultValues);
                                            if (!string.IsNullOrEmpty(wds.MasterDataSource))
                                            {
                                                WebDataSource masterWds = this.GetObjByID(wds.MasterDataSource) as WebDataSource;
                                                if (masterWds != null)
                                                {
                                                    foreach (DataRelation relation in wds.InnerDataSet.Relations)
                                                    {
                                                        if (relation.ParentTable.TableName == masterWds.DataMember && relation.ChildTable.TableName == wds.DataMember)
                                                        {
                                                            builder.Append(",[");
                                                            foreach (DataColumn column in relation.ParentColumns)
                                                            {
                                                                builder.AppendFormat("'{0}',", column.ColumnName);
                                                            }
                                                            if (builder.ToString().EndsWith(","))
                                                            {
                                                                builder.Remove(builder.Length - 1, 1);
                                                            }
                                                            builder.Append("]");

                                                            builder.Append(",[");
                                                            foreach (DataColumn column in relation.ChildColumns)
                                                            {
                                                                builder.AppendFormat("'{0}',", column.ColumnName);
                                                            }
                                                            if (builder.ToString().EndsWith(","))
                                                            {
                                                                builder.Remove(builder.Length - 1, 1);
                                                            }
                                                            builder.Append("]");
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                            builder.Append(");");
                                        }
                                        else
                                        {
                                            UpdatePanel upan = this.GetModalUpdatePanel(1);
                                            if (upan != null)
                                            {
                                                builder.AppendFormat("Ext.getCmp('{0}').addModal('{1}');", this.ID, upan.UniqueID);
                                            }
                                        }
                                    }
                                    builder.Append("}");
                                    #endregion
                                    break;
                                case ExtGridSystemHandler.Edit:
                                    #region edit
                                    if (!string.IsNullOrEmpty(this.EditFormViewID))
                                    {
                                        builder.AppendFormat("Ext.getCmp('{0}').editFormViewModal('{1}');", this.ID, this.EditFormViewID);
                                    }
                                    else if (string.IsNullOrEmpty(this.EditPanelID))
                                    {
                                        builder.AppendFormat("Ext.getCmp('{0}').editRow();", 
                                            this.ID);
                                    }
                                    else
                                    {
                                        UpdatePanel upan = this.GetModalUpdatePanel(1);
                                        if (upan != null)
                                        {
                                            builder.AppendFormat("Ext.getCmp('{0}').editModal('{1}');",
                                                this.ID,
                                                upan.UniqueID);
                                        }
                                    }
                                    builder.Append("}");
                                    #endregion
                                    break;
                                case ExtGridSystemHandler.Delete:
                                    #region delete
                                    builder.AppendFormat("Ext.getCmp('{0}').deleteRow();", this.ID);
                                    builder.Append("}");
                                    #endregion
                                    break;
                                case ExtGridSystemHandler.Refresh:
                                    builder.AppendFormat("Ext.getCmp('{0}').refresh();", this.ID);
                                    builder.Append("}");
                                    break;
                                case ExtGridSystemHandler.Save:
                                    #region save
                                    if (wds != null)
                                    {
                                        string remoteName = wds.RemoteName;
                                        if (!string.IsNullOrEmpty(remoteName) && remoteName.IndexOf('.') != -1)
                                        {
                                            builder.AppendFormat("Ext.getCmp('{0}').save();", this.ID);
                                        }
                                    }
                                    builder.Append("}");
                                    #endregion
                                    break;
                                case ExtGridSystemHandler.Abort:
                                    builder.AppendFormat("Ext.getCmp('{0}').abort();", this.ID);
                                    builder.Append("}");
                                    break;
                                case ExtGridSystemHandler.Query:
                                    #region query
                                    if (!string.IsNullOrEmpty(this.QueryPanelID))
                                    {
                                        UpdatePanel upan = this.GetModalUpdatePanel(2);
                                        if (upan != null)
                                        {
                                            builder.AppendFormat("__doPostBack('{0}');",
                                                upan.UniqueID);
                                        }
                                    }
                                    else if (wds != null)
                                    {
                                        DataTable tab = wds.InnerDataSet.Tables[wds.DataMember];
                                        builder.Append("var queries=new Ext.util.MixedCollection();");
                                        foreach (ExtQueryField field in this.QueryFields)
                                        {
                                            string queryPart = this.GenQueryPart(field, tab.Columns[field.DataField].DataType);
                                            if (!string.IsNullOrEmpty(queryPart))
                                            {
                                                builder.AppendFormat("if(queries.containsKey('{0}')){{", field.DataField);
                                                builder.AppendFormat("queries.get('{0}').push({1});",
                                                    field.DataField,
                                                    queryPart);
                                                builder.Append("}else{");
                                                builder.AppendFormat("queries.add('{0}',[{1}]);",
                                                    field.DataField,
                                                    queryPart);
                                                builder.Append("}");
                                            }
                                        }
                                        string[] buttonCaptions = SysMsg.GetSystemMessage(CliUtils.fClientLang, "AjaxTools", "ExtModelPanel", "UIText", true).Split(',');
                                        builder.AppendFormat("if(queries.getCount()>0){{Ext.getCmp('{0}').gridQuery(queries,{{columnsCount:{1},panWidth:{2},panHeight:{3},uiCaptions:['{4}','{5}','{6}']}});}}",
                                            this.ID,
                                            this.QueryColumns,
                                            this.QueryInnerPanelWidth,
                                            this.QueryInnerPanelHeight,
                                            this.QueryTitle,
                                            buttonCaptions[0],
                                            buttonCaptions[1]);
                                    }
                                    builder.Append("}");
                                    #endregion
                                    break;
                                case ExtGridSystemHandler.SavePersonal:
                                case ExtGridSystemHandler.LoadPersonal:
                                    #region save presonal settings & load presonal settings:
                                    builder.AppendFormat("Ext.getCmp('{0}').grid{1}('{2}');", 
                                        this.ID,
                                        this.ToolItems[i].SysHandlerType.ToString(), 
                                        CliUtils.fLoginUser);
                                    builder.Append("}");
                                    #endregion
                                    break;
                                case ExtGridSystemHandler.CustomDefine:
                                    builder.Append("}");
                                    break;
                            }
                            builder.Append("}");
                            break;
                        case ExtGridToolItemType.Label:
                            builder.AppendFormat("'{0}'", this.ToolItems[i].Text);
                            break;
                        case ExtGridToolItemType.Separation:
                            builder.AppendFormat("'-'");
                            break;
                        case ExtGridToolItemType.Fill:
                            builder.AppendFormat("'->'");
                            break;
                    }
                    if (i < this.ToolItems.Count - 1)
                    {
                        builder.Append(",");
                    }
                }
                builder.Append("]");
            }
            return builder.ToString();
        }

        public ExtGridColumn GetExtGridColumnByDataField(string dataField)
        {
            foreach (ExtGridColumn column in this.Columns)
            {
                if (column.DataField == dataField)
                {
                    return column;
                }
            }
            return null;
        }

        public string GenQueryPart(ExtQueryField field, Type dataType)
        {
            StringBuilder builder = new StringBuilder();
            if (field.Condition != "None")
            {
                ExtGridColumn editorColumn = this.GetExtGridColumnByDataField(field.DataField);
                if (editorColumn != null)
                {
                    builder.Append("{");
                    builder.AppendFormat("condition:'{0}',operator:'{1}',caption:'{2}',editor:'{3}'",
                        field.Condition,
                        field.Operator,
                        field.Caption,
                        editorColumn.Editor.ToString());
                    if (editorColumn.Editor == ExtGridEditor.ComboBox)
                    {
                        ExtComboBox cmb = this.GetObjByID(editorColumn.EditControlId) as ExtComboBox;
                        if (cmb != null)
                        {
                            builder.AppendFormat(",editorConfig:{0}", cmb.GenComboBoxConfig(field.DataField));
                        }
                    }
                    if (!string.IsNullOrEmpty(field.DefaultValue))
                    {
                        if (GloFix.IsNumeric(dataType))
                        {
                            builder.AppendFormat(",defVal:{0}", field.DefaultValue);
                        }
                        else if (dataType == typeof(Boolean))
                        {
                            builder.AppendFormat(",defVal:{0}", field.DefaultValue.ToLower());
                        }
                        else
                        {
                            builder.AppendFormat(",defVal:'{0}'", field.DefaultValue);
                        }
                    }
                    builder.Append("}");
                }
            }
            return builder.ToString();
        }

        public string GenValidJsonArray()
        {
            StringBuilder builder = new StringBuilder();
            foreach (ExtGridColumn column in this.Columns)
            {
                string validConfig = ExtValidator.GenValidateConfig(column.AllowNull, column.ValidType, column.ValidMethod, column.ValidText);
                if (!string.IsNullOrEmpty(validConfig))
                {
                    builder.AppendFormat("{{ field: '{0}', validConfig: {{ {1} }} }},", column.DataField, validConfig);
                }
            }
            if (builder.ToString().EndsWith(","))
            {
                builder.Remove(builder.Length - 1, 1);
            }
            return builder.ToString();
        }

        public string GenStoreConfigJsonObject()
        {
            StringBuilder builder = new StringBuilder();
            WebDataSource wds = this.GetObjByID(this.DataSourceID) as WebDataSource;
            if (wds != null)
            {
                builder.Append("{");
                builder.AppendFormat("pageSize:{0},", this.getPageSize());
                #region model
                builder.AppendFormat("model: Ext.define('{0}',", this.ID + "Model");
                builder.Append("{");
                builder.Append("state:[],");
                builder.Append("extend: 'Ext.data.Model',");
                builder.AppendFormat("fields: {0}", this.GenFieldsJsonArray());
                builder.Append("}),");
                #endregion
                builder.Append("proxy:{url:'../ExtJs/infolight/ExtGetData.ashx',type:'ajax',reader: {totalProperty: 'total', type: 'json', root: 'data'},"); 
                #region extraParams
                builder.Append("extraParams:{");
                if (this.Columns.Count > 0)
                {
                    builder.Append("oper:'select',");
                    builder.Append("fields:'");
                    for (int i = 0; i < this.Columns.Count; i++)
                    {
                        if (i < this.Columns.Count - 1)
                        {
                            builder.Append(this.Columns[i].DataField + ",");
                        }
                        else
                        {
                            builder.Append(this.Columns[i].DataField + "'");
                        }
                    }
                }
                string remoteName = wds.RemoteName;
                if (!string.IsNullOrEmpty(remoteName) && remoteName.IndexOf('.') != -1)
                {
                    if (builder.ToString().EndsWith("'"))
                    {
                        builder.Append(",");
                    }
                    builder.AppendFormat("module:'{0}',command:'{1}',{2}sevmod:{3},alwaysClose:{4},where:'{5}',",
                        remoteName.Split('.')[0],
                        wds.DataMember,
                        string.IsNullOrEmpty(wds.MasterDataSource) ? "" : string.Format("masterCommand:'{0}',", (this.GetObjByID(wds.MasterDataSource) as WebDataSource).DataMember),
                        wds.ServerModify.ToString().ToLower(),
                        wds.AlwaysClose.ToString().ToLower(),
                        wds.WhereStr.Trim().Replace("'", "\\'"));
                    if (string.IsNullOrEmpty(wds.MasterDataSource))
                    {
                        builder.AppendFormat("cacheDataSet:'{0}',isDetails:'false',", this.Page.AppRelativeVirtualPath + this.DataSourceID);
                    }
                    else
                    {
                        WebDataSource wdsMaster = this.GetObjByID(wds.MasterDataSource) as WebDataSource;
                        if (wdsMaster != null)
                        {
                            builder.AppendFormat("cacheDataSet:'{0}',isDetails:'true',",
                                this.Page.AppRelativeVirtualPath + wds.MasterDataSource);
                            if (!this.RenderToExtComp())
                            {
                                foreach (DataRelation relation in wds.InnerDataSet.Relations)
                                {
                                    if (relation.ParentTable.TableName == wdsMaster.DataMember && relation.ChildTable.TableName == wds.DataMember)
                                    {
                                        builder.Append("masterKeys:Ext.encode({");
                                        for (int i = 0; i < relation.ChildColumns.Length; i++)
                                        {
                                            Type colType = relation.ChildColumns[i].DataType;
                                            string formatter = "{0}:{1},";
                                            if (!GloFix.IsNumeric(colType) && colType != typeof(bool))
                                            {
                                                formatter = "{0}:'{1}',";
                                            }
                                            builder.AppendFormat(formatter,
                                                relation.ParentColumns[i].ColumnName,
                                                wds.RelationValues[relation.ChildColumns[i].ColumnName]);
                                        }
                                        if (builder.ToString().EndsWith(","))
                                        {
                                            builder.Remove(builder.Length - 1, 1);
                                        }
                                        builder.Append("}),");
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                if (builder.ToString().EndsWith(","))
                {
                    builder.Remove(builder.Length - 1, 1);
                }
                builder.Append("}");
                #endregion
                builder.Append("}}");
            }
            return builder.ToString();
        }

        public string GenPageConfigJsonObject()
        {
            StringBuilder builder = new StringBuilder();
            if (this.PagingSet.AllowPage)
            {
                builder.Append("{");
                builder.AppendFormat("displayInfo:{0},emptyMsg:'{1}',displayMsg:'{2}',pageSize:{3}",
                    this.PagingSet.DisplayPageInfo.ToString().ToLower(),
                    SysMsg.GetSystemMessage(CliUtils.fClientLang, "AjaxTools", "ExtGrid", "EmptyMsg", true),
                    SysMsg.GetSystemMessage(CliUtils.fClientLang, "AjaxTools", "ExtGrid", "DisplayMsg", true),
                    this.getPageSize());
                builder.Append("}");
            }
            return builder.ToString();
        }

        public string GenGridConfigJsonObject(string title)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("{");
            
            if (string.IsNullOrEmpty(this.GridSet.GridPanel))
            {
                if (!RenderToExtComp())
                {
                    builder.Append("renderTo:document.body,");
                }
            }
            else
            {
                builder.AppendFormat("renderTo:'{0}',", this.GridSet.GridPanel);
            }
            string tools = this.GenToolItemsJsonArray();
            builder.AppendFormat("id:'{0}',stripeRows:{1},collapsible:{2},autoExpandColumn:'{3}',title:'{4}',loadMask:{5},clicksToEdit:{6},{7}width:{8},height:{9}}}",
                this.ID,
                this.GridSet.AlternateRow.ToString().ToLower(),
                this.GridSet.GridCollapsible.ToString().ToLower(),
                this.GridSet.AutoFillingColumn,
                string.IsNullOrEmpty(title) ? this.GridSet.Title.Replace("'", @"\'") : title,
                this.GridSet.LoadMask.ToString().ToLower(),
                this.GridSet.ClicksToEdit,
                string.IsNullOrEmpty(tools) ? "" : string.Format("tbar:{0},", tools),
                this.GridSet.Width,
                this.GridSet.Height);

            return builder.ToString();
        }

        public string GenExpandRowTemplateHtml()
        {
            if (this.AllowExpandRow)
            {
                if (!string.IsNullOrEmpty(this.ExpanderRowTemplateHtml))
                {
                    return this.ExpanderRowTemplateHtml;
                }
                if (this.Columns.Count > 0)
                {
                    List<ExtGridColumn> expandColumnList = new List<ExtGridColumn>();
                    foreach (ExtGridColumn column in this.Columns)
                    {
                        if (column.ExpandColumn)
                        {
                            expandColumnList.Add(column);
                        }
                    }
                    int maxOneLine = 1, oneLine = 1;
                    foreach (ExtGridColumn column in expandColumnList)
                    {
                        if (column.NewLine)
                        {
                            maxOneLine = Math.Max(maxOneLine, oneLine);
                            oneLine = 1;
                        }
                        else
                        {
                            oneLine++;
                        }
                    }
                    maxOneLine = Math.Max(maxOneLine, oneLine);

                    StringBuilder builder = new StringBuilder();
                    builder.Append("<table class=\\'quid_grid_expandtab\\' cellspacing=\\'1\\'>");

                    int j = 1;
                    int percentWidth = 100 / maxOneLine;
                    for (int i = 0; i < expandColumnList.Count; i++)
                    {
                        string columnHtml = "";
                        ExtGridColumn col = expandColumnList[i];
                        ExtGridColumn nextCol = (i < expandColumnList.Count - 1) ? expandColumnList[i + 1] : null;

                        if (col.NewLine)
                        {
                            if (i != 0)
                            {
                                columnHtml += "</tr>";
                            }
                            columnHtml += "<tr>";
                        }

                        string styleWidth = "";
                        if (this.GridSet.ExpandColumnWidth > 0)
                        {
                            styleWidth = string.Format(" style=\\'width:{0}px\\'", this.GridSet.ExpandColumnWidth);
                        }

                        if (nextCol != null)
                        {
                            if (nextCol.NewLine)
                            {
                                if ((2 * (maxOneLine - j) + 1) > 1)
                                {

                                    columnHtml += "<td nowrap class=\\'quid_grid_nowarptd\\'" + styleWidth + "><b>{0}:</b></td><td class=\\'quid_grid_contenttd\\' colspan=\\'" + (2 * (maxOneLine - j) + 1).ToString() + "\\'>{{{1}}}</td>";
                                }
                                else
                                {
                                    columnHtml += "<td nowrap class=\\'quid_grid_nowarptd\\'" + styleWidth + "><b>{0}:</b></td><td class=\\'quid_grid_contenttd\\'>{{{1}}}</td>";
                                }
                                j = 1;
                            }
                            else
                            {
                                columnHtml += "<td nowrap class=\\'quid_grid_nowarptd\\'" + styleWidth + "><b>{0}:</b></td><td class=\\'quid_grid_contenttd\\'>{{{1}}}</td>";
                                j++;
                            }
                        }
                        else
                        {
                            if ((2 * (maxOneLine - j) + 1) > 1)
                            {
                                columnHtml += "<td nowrap class=\\'quid_grid_nowarptd\\'" + styleWidth + "><b>{0}:</b></td><td class=\\'quid_grid_contenttd\\' colspan=\\'" + (2 * (maxOneLine - j) + 1).ToString() + "\\'>{{{1}}}</td></tr>";
                            }
                            else
                            {
                                columnHtml += "<td nowrap class=\\'quid_grid_nowarptd\\'" + styleWidth + "><b>{0}:</b></td><td class=\\'quid_grid_contenttd\\'>{{{1}}}</td></tr>";
                            }
                        }
                        builder.AppendFormat(columnHtml, col.HeaderText, col.DataField);
                    }
                    builder.Append("</table>");
                    return builder.ToString();
                }
            }
            return "";
        }

        public string GenKeyFieldsArray()
        {
            ExtGridColumnCollection columns = this.Columns;
            StringBuilder builder = new StringBuilder();
            builder.Append("[");
            if (columns.Count > 0)
            {
                for (int i = 0; i < columns.Count; i++)
                {
                    if (columns[i].IsKeyField)
                    {
                        builder.AppendFormat("'{0}',", columns[i].DataField);
                    }
                }
                if (builder.ToString().EndsWith(","))
                {
                    builder.Remove(builder.Length - 1, 1);
                }
            }
            builder.Append("]");

            return builder.ToString();
        }

        public void RenderGrid(bool isRegisterToUpdatePanel)
        {
            string renderScript = this.GenGrid(false);
            if (isRegisterToUpdatePanel)
            {
                object scriptContainer = this.GetObjByID(this.GridSet.GridPanel);
                if (scriptContainer != null && scriptContainer is UpdatePanel)
                {
                    ScriptManager.RegisterStartupScript(scriptContainer as UpdatePanel, this.GetType(), Guid.NewGuid().ToString(), renderScript, true);
                    return;
                }
            }
            this.Page.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), renderScript, true);
        }

        //public void Set

        public string GenGrid(bool renderToExtComp)
        {
            return this.GenGrid(renderToExtComp, "");
        }

        public string GenGrid(bool renderToExtComp, string title)
        {
            StringBuilder builder = new StringBuilder();
            WebDataSource dsc = this.GetObjByID(this.DataSourceID) as WebDataSource;
            foreach (DataColumn primaryKey in dsc.PrimaryKey)
            {
                foreach (ExtGridColumn column in this.Columns)
                {
                    if (column.DataField == primaryKey.ColumnName)
                    {
                        column.IsKeyField = true;
                    }
                }
            }
            if (dsc != null)
            {
                string columns = this.GenColumnsJsonArray();
                string valids = this.GenValidJsonArray();
                string keys = this.GenKeyFieldsArray();
                if (!string.IsNullOrEmpty(columns))
                {
                    string expandRowHtml = this.GenExpandRowTemplateHtml();
                    string pageConfig = this.GenPageConfigJsonObject();
                    if (!renderToExtComp)
                    {
                        builder.AppendFormat("var {0}Config=", this.ID);
                    }
                    builder.AppendFormat("{{columns:{0},allowPage:{1},{2}{3}{4}keys:{5},validArray:[{6}],storeConfig:{7},{8}gridConfig:{9},autoApply:{10},{13}focusEventHandlers:{11},leaveEventHandlers:{12}}}",
                        columns,
                        this.PagingSet.AllowPage.ToString().ToLower(),
                        string.IsNullOrEmpty(expandRowHtml) ? "" : string.Format("expandRowTemplateHtml:'{0}',", expandRowHtml),
                        string.IsNullOrEmpty(this.EditPanelID) ? "" : string.Format("editPan:'{0}',", this.EditPanelID),
                        string.IsNullOrEmpty(this.QueryPanelID) ? "" : string.Format("queryPan:'{0}',", this.QueryPanelID),
                        keys,
                        valids,
                        this.GenStoreConfigJsonObject(),
                        string.IsNullOrEmpty(pageConfig) ? "" : string.Format("pageConfig:{0},", pageConfig),
                        this.GenGridConfigJsonObject(title),
                        dsc.AutoApply.ToString().ToLower(),
                        this.GenEventHandlers("beforeEdit"),
                        this.GenEventHandlers("afterEdit"),
                        GenEditFormPanelConfig());
                    if (!renderToExtComp)
                    {
                        //builder.AppendFormat(";Ext.onReady(function(){{gridHelper2.createGrid({0}Config);}});", this.ID);
                        builder.AppendFormat(";Ext.onReady(function(){{Infolight.GridHelper.createGrid({0}Config);}});", this.ID);
                    }
                }
            }
            return builder.ToString();
        }

        private string GenEditFormPanelConfig()
        {
            if (this.EditFormViewID != null && this.EditFormViewID.Trim() != string.Empty)
            {
                return string.Format("editFormViewID:{0},editFormViewConfig:[{1}],editFormViewWidth:{2},editFormViewHeight:{3},editFormViewKeyFields:[{4}],editFormViewFields:[{5}],editFormViewValids:[{6}],editFormViewFocusEventHandlers:{7},editFormViewLeaveEventHandlers:{8},",
                    "'" + this.EditFormViewID + "'",
                    this.GenEidtFormView(),
                    this.EditFormPanelWidth,
                    this.EditFormPanelHeight,
                    this.GenEidtFormViewFields(true),
                    this.GenEidtFormViewFields(false),
                    this.GenEidtFormViewValids(),
                    //this.GenEidtFormViewDefaultValues(),
                    this.GenEidtFormViewEventHandlers("focus"),
                    this.GenEidtFormViewEventHandlers("leave"));
            }
            else
                return "";
        }

        private string GenEidtFormView()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(EditFormViewID))
                {
                    AjaxFormView EidtFormView = this.GetObjByID(EditFormViewID) as AjaxFormView;
                    if (EidtFormView != null)
                    {
                        builder.AppendFormat("{0},", EidtFormView.GenFormView(""));
                    }
                }
            
            if (builder.ToString().EndsWith(","))
            {
                builder.Remove(builder.Length - 1, 1);
            }
            return builder.ToString();
        }

        private string GenEidtFormViewFields(bool keyFields)
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(EditFormViewID))
            {
                AjaxFormView EidtFormView = this.GetObjByID(EditFormViewID) as AjaxFormView;
                if (EidtFormView != null)
                {
                    if (keyFields)
                    {
                        string itemKeyFields = EidtFormView.GenKeyFields();
                        if (!string.IsNullOrEmpty(itemKeyFields))
                        {
                            builder.AppendFormat("{0},", itemKeyFields);
                        }
                    }
                    else
                    {
                        string itemFields = EidtFormView.GenFields();
                        if (!string.IsNullOrEmpty(itemFields))
                        {
                            builder.AppendFormat("{0},", itemFields);
                        }
                    }
                }
            }

            if (builder.ToString().EndsWith(","))
            {
                builder.Remove(builder.Length - 1, 1);
            }
            return builder.ToString();
        }

        private string GenEidtFormViewValids()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(EditFormViewID))
            {
                AjaxFormView EidtFormView = this.GetObjByID(EditFormViewID) as AjaxFormView;
                if (EidtFormView != null)
                {
                    string itemValids = EidtFormView.GenValidJsonArray();
                    if (!string.IsNullOrEmpty(itemValids))
                    {
                        builder.AppendFormat("{0},", itemValids);
                    }
                }
            }

            if (builder.ToString().EndsWith(","))
            {
                builder.Remove(builder.Length - 1, 1);
            }
            return builder.ToString();
        }

        private string GenEidtFormViewEventHandlers(string eventName)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("[");
            AjaxFormView EidtFormView = this.GetObjByID(EditFormViewID) as AjaxFormView;
            if (EidtFormView != null)
            {
                builder.Append(EidtFormView.GenEventHandlers(eventName));
            }

            if (builder.ToString().EndsWith(","))
            {
                builder.Remove(builder.Length - 1, 1);
            }
            builder.Append("]");
            return builder.ToString();
        }

        public string GenEventHandlers(string eventName)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("[");
            foreach (ExtGridColumn col in this.Columns)
            {
                if (eventName == "beforeEdit" && !string.IsNullOrEmpty(col.BeforeEdit))
                {
                    builder.Append("{");
                    builder.AppendFormat("field:'{0}',handler:{1}",
                        col.DataField,
                        col.BeforeEdit);
                    builder.Append("},");
                }
                else if (eventName == "afterEdit" && !string.IsNullOrEmpty(col.AfterEdit))
                {
                    builder.Append("{");
                    builder.AppendFormat("field:'{0}',handler:{1}",
                        col.DataField,
                        col.AfterEdit);
                    builder.Append("},");
                }
            }
            if (builder.ToString().EndsWith(","))
            {
                builder.Remove(builder.Length - 1, 1);
            }
            builder.Append("]");
            return builder.ToString();
        }

        public string GenDefaultValues(WebDataSource wds)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("{");
            DataTable tab = wds.InnerDataSet.Tables[wds.DataMember];
            foreach (ExtGridColumn column in this.Columns)
            {
                if (!string.IsNullOrEmpty(column.DefaultValue))
                {
                    Type type = tab.Columns[column.DataField].DataType;
                    if (GloFix.IsNumeric(type))
                    {
                        builder.AppendFormat("{0}:{1},", column.DataField, column.DefaultValue);
                    }
                    else if (type == typeof(Boolean))
                    {
                        builder.AppendFormat("{0}:{1},", column.DataField, column.DefaultValue.ToLower());
                    }
                    else
                    {
                        builder.AppendFormat("{0}:'{1}',", column.DataField, column.DefaultValue);
                    }
                }
                else if (!string.IsNullOrEmpty(column.DefaultMethod))
                {
                    if (column.DefaultMethod.IndexOf('.') != -1)
                    {
                        builder.AppendFormat("{0}:'@srvMethod:{1}',", column.DataField, column.DefaultMethod);
                    }
                    else
                    {
                        builder.AppendFormat("{0}:{1}(),", column.DataField, column.DefaultMethod);
                    }
                }
            }
            //if (!string.IsNullOrEmpty(wds.MasterDataSource))
            //{
            //    builder.Append(GloFix.GetWhereString(tab, wds.RelationValues, ":", ","));
            //}
            if (builder.ToString().EndsWith(","))
            {
                builder.Remove(builder.Length - 1, 1);
            }
            builder.Append("}");
            return builder.ToString();
        }

        int getPageSize()
        {
            object o = this.GetObjByID(this.DataSourceID);
            if (o != null && o is WebDataSource)
            {
                WebDataSource wds = o as WebDataSource;
                return wds.PacketRecords;
            }
            return -1;
        }

        public void SetEditMode()
        {
            if (this.Page.Request.Form["__EVENTARGUMENT"] != null)
            {
                UpdatePanel upan = this.GetModalUpdatePanel(1);
                if (upan != null && this.Page.Request.Form["__EVENTTARGET"] == upan.UniqueID)
                {
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    object args = serializer.DeserializeObject(this.Page.Request.Form["__EVENTARGUMENT"]);
                    if (args != null)
                    {
                        Dictionary<string, object> dicArgs = args as Dictionary<string, object>;
                        CompositeDataBoundControl view = this.GetEditView();
                        if (view != null)
                        {
                            this.ChangeEditViewMode(view, dicArgs);
                        }
                    }
                }
            }
        }

        public void SetQueryState(WebClientQuery qry, Panel pan)
        {
            UpdatePanel upan = this.GetModalUpdatePanel(2);
            if (upan != null && this.Page.Request.Form["__EVENTTARGET"] == upan.UniqueID)
            {
                //qry.Clear(pan);
                string script = string.Format("var behavior=$find('{0}behavior');if(behavior){{behavior.show();}}", this.QueryPanelID);
                ScriptManager.RegisterStartupScript(upan, this.GetType(), Guid.NewGuid().ToString(), script, true);
            }
        }

        public void Submit(CompositeDataBoundControl view)
        {
            object o = this.GetObjByID(this.DataSourceID);
            if (o != null && o is WebDataSource)
            {
                WebDataSource wds = o as WebDataSource;
                string loadType = "";
                //int position = -1;
                if (view is DetailsView)
                {
                    WebDetailsView detView = view as WebDetailsView;
                    if (detView.CurrentMode == DetailsViewMode.Insert)
                    {
                        detView.InsertItem(false);
                        loadType = "insert";
                        //position = wds.InnerDataSet.Tables[wds.DataMember].Rows.Count - 1;
                    }
                    else if (detView.CurrentMode == DetailsViewMode.Edit)
                    {
                        detView.UpdateItem(false);
                        loadType = "edit";
                    }
                    WebValidate validate = (WebValidate)detView.ExtendedFindChildControl(this.DataSourceID, FindControlType.DataSourceID, typeof(WebValidate));
                    if (validate != null && detView.ValidateFailed)
                    {
                        return;
                    }
                }
                else if (view is FormView)
                {
                    WebFormView fmView = view as WebFormView;
                    if (fmView.CurrentMode == FormViewMode.Insert)
                    {
                        fmView.InsertItem(false);
                        loadType = "insert";
                        //position = wds.InnerDataSet.Tables[wds.DataMember].Rows.Count - 1;
                    }
                    else if (fmView.CurrentMode == FormViewMode.Edit)
                    {
                        fmView.UpdateItem(false);
                        loadType = "edit";
                    }
                    WebValidate validate = (WebValidate)fmView.ExtendedFindChildControl(this.DataSourceID, FindControlType.DataSourceID, typeof(WebValidate));
                    if (validate != null && fmView.ValidateFailed)
                    {
                        return;
                    }
                }

                StringBuilder builder = new StringBuilder();
                if (wds.AutoApply)
                {
                    builder.AppendFormat("var grid=Ext.getCmp('{0}');", this.ID);
                    builder.Append("grid.store.load();");
                }
                else
                {
                    DataTable tab = wds.InnerDataSet.Tables[wds.DataMember].GetChanges();
                    if (tab != null && tab.Rows.Count == 1)
                    {
                        builder.AppendFormat("var grid=Ext.getCmp('{0}');", this.ID);
                        DataRow changedRow = tab.Rows[0];
                        if (loadType == "insert")
                        {
                            builder.Append("var changedRecord={");
                            foreach (DataColumn column in tab.Columns)
                            {
                                if (changedRow[column] != DBNull.Value)
                                {
                                    Type type = column.DataType;
                                    if (GloFix.IsNumeric(type))
                                    {
                                        builder.AppendFormat("{0}:{1},", column.ColumnName, changedRow[column.ColumnName]);
                                    }
                                    else if (type == typeof(bool))
                                    {
                                        builder.AppendFormat("{0}:{1},", column.ColumnName, changedRow[column.ColumnName].ToString().ToLower());
                                    }
                                    else
                                    {
                                        builder.AppendFormat("{0}:'{1}',", column.ColumnName, changedRow[column.ColumnName]);
                                    }
                                }
                            }
                            if (builder.ToString().EndsWith(","))
                            {
                                builder.Remove(builder.Length - 1, 1);
                            }
                            builder.Append("};");
                            builder.AppendFormat("Infolight.GridHelper.insertStore(grid.store,changedRecord);", this.ID);
                        }
                        else if (loadType == "edit")
                        {
                            builder.Append("var recordKeys={");
                            foreach (ExtGridColumn extColumn in this.Columns)
                            {
                                if (extColumn.IsKeyField)
                                {
                                    Type type = tab.Columns[extColumn.DataField].DataType;
                                    if (GloFix.IsNumeric(type))
                                    {
                                        builder.AppendFormat("{0}:{1},", extColumn.DataField, changedRow[extColumn.DataField]);
                                    }
                                    else if (type == typeof(bool))
                                    {
                                        builder.AppendFormat("{0}:{1},", extColumn.DataField, changedRow[extColumn.DataField].ToString().ToLower());
                                    }
                                    else
                                    {
                                        builder.AppendFormat("{0}:'{1}',", extColumn.DataField, changedRow[extColumn.DataField]);
                                    }
                                }
                            }
                            if (builder.ToString().EndsWith(","))
                            {
                                builder.Remove(builder.Length - 1, 1);
                            }
                            builder.Append("};");

                            builder.Append("var changedRecord={");
                            foreach (DataColumn column in tab.Columns)
                            {
                                if (!changedRow[column, DataRowVersion.Current].Equals(changedRow[column, DataRowVersion.Original]))
                                {
                                    Type type = column.DataType;
                                    if (GloFix.IsNumeric(type))
                                    {
                                        builder.AppendFormat("{0}:{1},", column.ColumnName, changedRow[column.ColumnName]);
                                    }
                                    else if (type == typeof(bool))
                                    {
                                        builder.AppendFormat("{0}:{1},", column.ColumnName, changedRow[column.ColumnName].ToString().ToLower());
                                    }
                                    else
                                    {
                                        builder.AppendFormat("{0}:'{1}',", column.ColumnName, changedRow[column.ColumnName]);
                                    }
                                }
                            }
                            if (builder.ToString().EndsWith(","))
                            {
                                builder.Remove(builder.Length - 1, 1);
                            }
                            builder.Append("};");
                            builder.Append("Infolight.GridHelper.updateStore(grid.store,recordKeys,changedRecord);");
                        }
                        wds.InnerDataSet.Tables[wds.DataMember].AcceptChanges();
                    }
                }

                UpdatePanel upan = this.GetModalUpdatePanel(1);
                if (upan != null)
                {
                    builder.AppendFormat("var behavior=$find('{0}behavior');", this.EditPanelID);
                    builder.Append("if(behavior){behavior.hide();}");
                    if (loadType == "insert")
                    {
                        builder.AppendFormat("grid.locateLastRecord();");
                    }
                }
                if (builder.Length > 0)
                {
                    ScriptManager.RegisterStartupScript(upan, this.GetType(), Guid.NewGuid().ToString(), builder.ToString(), true);
                }
            }
        }

        public void Cancel()
        {
            CompositeDataBoundControl view = this.GetEditView();
            if (view != null)
            {
                Dictionary<string, object> dicArgs = new Dictionary<string, object>();
                dicArgs.Add("mode", "readonly");
                this.ChangeEditViewMode(view, dicArgs);
            }
        }

        public void Query(WebClientQuery qry, Panel pan)
        {
            UpdatePanel upan = this.GetModalUpdatePanel(2);
            if (upan != null)
            {
                qry.Execute(pan);
                WebDataSource wds = this.GetObjByID(qry.DataSourceID) as WebDataSource;
                if (wds != null)
                {
                    StringBuilder builder = new StringBuilder();
                    builder.AppendFormat("var behavior=$find('{0}behavior');", this.QueryPanelID);
                    builder.Append("if(behavior){behavior.hide();}");


                    builder.AppendFormat("Ext.getCmp('{0}').setWhere('{1}');",
                        this.ID,
                        wds.WhereStr.Trim().Replace("'", "\\'"));
                    ScriptManager.RegisterStartupScript(upan, this.GetType(), Guid.NewGuid().ToString(), builder.ToString(), true);
                }
            }
        }

        public void Query(WebClientQuery qry, Panel pan, UpdatePanel upan)
        {
            if (upan == null)
                upan = this.GetModalUpdatePanel(2);
            if (upan != null)
            {
                qry.Execute(pan);
                WebDataSource wds = this.GetObjByID(qry.DataSourceID) as WebDataSource;
                if (wds != null)
                {
                    StringBuilder builder = new StringBuilder();
                    builder.AppendFormat("var behavior=$find('{0}behavior');", this.QueryPanelID);
                    builder.Append("if(behavior){behavior.hide();}");


                    builder.AppendFormat("Ext.getCmp('{0}').setWhere('{1}');",
                        this.ID,
                        wds.WhereStr.Trim().Replace("'", "\\'"));
                    ScriptManager.RegisterStartupScript(upan, this.GetType(), Guid.NewGuid().ToString(), builder.ToString(), true);
                }
            }
        }

        ExtModalPanel GetModalPanel(int modalPanelType)
        {
            string panel = "";
            switch (modalPanelType)
            {
                case 1:
                    panel = this.EditPanelID;
                    break;
                case 2:
                    panel = this.QueryPanelID;
                    break;
            }
            object o = this.GetObjByID(panel);
            if (o != null && o is ExtModalPanel)
            {
                return (ExtModalPanel)o;
            }
            return null;
        }

        UpdatePanel GetModalUpdatePanel(int modaltype)
        {
            ExtModalPanel pan = this.GetModalPanel(modaltype);
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

        CompositeDataBoundControl GetEditView()
        {
            UpdatePanel upan = this.GetModalUpdatePanel(1);
            if (upan != null)
            {
                foreach (Control ctrl in upan.ContentTemplateContainer.Controls)
                {
                    if (ctrl is FormView || ctrl is DetailsView)
                    {
                        return ctrl as CompositeDataBoundControl;
                    }
                }
            }
            return null;
        }

        void ChangeEditViewMode(CompositeDataBoundControl view, Dictionary<string, object> dicArgs)
        {
            string mode = dicArgs["mode"] as string;
            if (!string.IsNullOrEmpty(mode))
            {
                object o = this.GetObjByID(this.DataSourceID);
                if (o != null && o is WebDataSource)
                {
                    WebDataSource wds = ((WebDataSource)o);
                    if (mode == "insert")
                    {
                        #region insert
                        if (view is DetailsView)
                        {
                            DetailsView detView = view as DetailsView;
                            if (detView.CurrentMode != DetailsViewMode.Insert)
                            {
                                detView.ChangeMode(DetailsViewMode.Insert);
                            }
                        }
                        else if (view is FormView)
                        {
                            FormView fmView = view as FormView;
                            if (fmView.CurrentMode != FormViewMode.Insert)
                            {
                                fmView.ChangeMode(FormViewMode.Insert);
                            }
                        }
                        #endregion
                    }
                    else if (mode == "edit")
                    {
                        #region edit
                        List<object> lstKeys = new List<object>();
                        foreach (KeyValuePair<string, object> pair in dicArgs)
                        {
                            if (pair.Key != "mode")
                            {
                                lstKeys.Add(pair.Value);
                            }
                        }
                        int index = -1;
                        if (string.IsNullOrEmpty(wds.MasterDataSource))
                        {
                            if (string.IsNullOrEmpty(wds.View.Sort))
                            {
                                wds.View.ApplyDefaultSort = true;
                            }
                            index = wds.View.Find(lstKeys.ToArray());
                        }
                        else
                        {
                            DataTable detTab = wds.View.Table;
                            DataColumn[] keyColumns = detTab.PrimaryKey;
                            DataRow editRow = detTab.Rows.Find(lstKeys.ToArray());
                            for (int i = 0; i < detTab.Rows.Count; i++)
                            {
                                bool isEditRow = true;
                                foreach (DataColumn column in keyColumns)
                                {
                                    if (!editRow[column].Equals(detTab.Rows[i][column]))
                                    {
                                        isEditRow = false;
                                        break;
                                    }
                                }
                                if (isEditRow)
                                {
                                    index = i;
                                    break;
                                }
                            }
                        }

                        if (view is DetailsView)
                        {
                            DetailsView detView = view as DetailsView;
                            if (index != -1 && detView.CurrentMode != DetailsViewMode.Edit)
                            {
                                detView.PageIndex = index;
                                detView.DataBind();
                                detView.ChangeMode(DetailsViewMode.Edit);
                            }
                        }
                        else if (view is FormView)
                        {
                            FormView fmView = view as FormView;
                            if (index != -1 && fmView.CurrentMode != FormViewMode.Edit)
                            {
                                fmView.PageIndex = index;
                                fmView.DataBind();
                                fmView.ChangeMode(FormViewMode.Edit);
                            }
                        }
                        #endregion
                    }
                    else if (mode == "readonly")
                    {
                        #region readonly
                        if (view is DetailsView)
                        {
                            DetailsView detView = view as DetailsView;
                            if (detView.CurrentMode != DetailsViewMode.ReadOnly)
                            {
                                detView.ChangeMode(DetailsViewMode.ReadOnly);
                            }
                        }
                        else if (view is FormView)
                        {
                            FormView fmView = view as FormView;
                            if (fmView.CurrentMode != FormViewMode.ReadOnly)
                            {
                                fmView.ChangeMode(FormViewMode.ReadOnly);
                            }
                        }
                        #endregion
                    }
                }
                UpdatePanel upan = this.GetModalUpdatePanel(1);
                if (upan != null)
                {
                    string oper = "";
                    if(mode == "insert" || mode=="edit")
                    {
                        oper = "show";
                    }
                    else if(mode=="readonly")
                    {
                        oper = "hide";
                    }
                    string script = string.Format("var behavior=$find('{0}behavior');if(behavior){{behavior.{1}();}}", this.EditPanelID, oper);
                    ScriptManager.RegisterStartupScript(upan, this.GetType(), Guid.NewGuid().ToString(), script, true);
                }
            }
        }

        bool RenderToExtComp()
        {
            foreach (Control ctrl in this.Page.Form.Controls)
            {
                if (ctrl is AjaxLayout)
                {
                    AjaxLayout layout = ctrl as AjaxLayout;
                    if (layout.View == this.ID)
                    {
                        return true;
                    }
                    else
                    {
                        foreach (MultiViewItem item in layout.Details)
                        {
                            if (item.ControlId == this.ID)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class ExtViewPagingSet : IChildSet
    {
        public ExtViewPagingSet(AjaxBaseControl parent)
        {
            _ownerView = parent;
        }

        AjaxBaseControl _ownerView = null;
        bool _allowPage = true;
        bool _displayPageInfo = true;
        //string _emptyMessage = "there's no record!";
        //string _displayMessage = "display from {0} to {1}, total:{2}";

        [Browsable(false)]
        public AjaxBaseControl OwnerView
        {
            get { return _ownerView; }
        }

        [DefaultValue(true)]
        [NotifyParentProperty(true)]
        public bool AllowPage
        {
            get { return _allowPage; }
            set { _allowPage = value; }
        }

        [DefaultValue(true)]
        [NotifyParentProperty(true)]
        public bool DisplayPageInfo
        {
            get { return _displayPageInfo; }
            set { _displayPageInfo = value; }
        }

        //[DefaultValue("there's no record!")]
        //[NotifyParentProperty(true)]
        //public string EmptyMessage
        //{
        //    get { return _emptyMessage; }
        //    set { _emptyMessage = value; }
        //}

        //[DefaultValue("display from {0} to {1}, total:{2}")]
        //[NotifyParentProperty(true)]
        //public string DisplayMessage
        //{
        //    get { return _displayMessage; }
        //    set { _displayMessage = value; }
        //}
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class ExtGridGridSet : IChildSet
    {
        public ExtGridGridSet(AjaxBaseControl parent)
        {
            _ownerView = parent;
        }

        AjaxBaseControl _ownerView;
        string _gridPanel = "";
        string _title = "";
        string _autoFillingColumn = "";

        int _clicksToEdit = 2;
        int _width = 600;
        int _height = 300;
        int _expandColumnWidth = 0;

        bool _alternateRow = true;
        bool _gridCollapsible = true;
        bool _loadMask = true;

        [Browsable(false)]
        public AjaxBaseControl OwnerView
        {
            get { return _ownerView; }
        }

        [DefaultValue("")]
        [NotifyParentProperty(true)]
        [Editor(typeof(ExtStaticStringEditor), typeof(UITypeEditor))]
        public string GridPanel
        {
            get { return _gridPanel; }
            set { _gridPanel = value; }
        }

        [DefaultValue("")]
        [NotifyParentProperty(true)]
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        [DefaultValue("")]
        [NotifyParentProperty(true)]
        [Editor(typeof(ExtStaticStringEditor), typeof(UITypeEditor))]
        public string AutoFillingColumn
        {
            get { return _autoFillingColumn; }
            set { _autoFillingColumn = value; }
        }

        [DefaultValue(600)]
        [NotifyParentProperty(true)]
        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }

        [DefaultValue(2)]
        [NotifyParentProperty(true)]
        public int ClicksToEdit
        {
            get { return _clicksToEdit; }
            set { _clicksToEdit = value; }
        }

        [DefaultValue(300)]
        [NotifyParentProperty(true)]
        public int Height
        {
            get { return _height; }
            set { _height = value; }
        }

        [DefaultValue(0)]
        [NotifyParentProperty(true)]
        public int ExpandColumnWidth
        {
            get { return _expandColumnWidth; }
            set { _expandColumnWidth = value; }
        }

        [DefaultValue(true)]
        [NotifyParentProperty(true)]
        public bool AlternateRow
        {
            get { return _alternateRow; }
            set { _alternateRow = value; }
        }

        [DefaultValue(true)]
        [NotifyParentProperty(true)]
        public bool GridCollapsible
        {
            get { return _gridCollapsible; }
            set { _gridCollapsible = value; }
        }

        [DefaultValue(true)]
        [NotifyParentProperty(true)]
        public bool LoadMask
        {
            get { return _loadMask; }
            set { _loadMask = value; }
        }
    }
}