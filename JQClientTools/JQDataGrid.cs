using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Web.UI.Design;
using System.Web.UI.WebControls;
using System.Drawing.Design;
using System.Collections.Specialized;

namespace JQClientTools
{
    [Designer(typeof(JQDataGridDesigner), typeof(IDesigner))]
    public class JQDataGrid : WebControl, IJQDataSourceProvider, IDetailObject, IColumnCaptions
    {
        public JQDataGrid()
        {
            columns = new JQCollection<JQGridColumn>(this);
            relationColumns = new JQCollection<JQRelationColumn>(this);
            toolitems = new JQCollection<JQToolItem>(this);
            queryColumns = new JQCollection<JQQueryColumn>(this);
            AutoApply = true;
            EditOnEnter = true;
            Pagination = true;
            PageSize = 10;
            PageList = "10,20,30,40,50";
            MultiSelect = false;
            ViewCommandVisible = true;
            UpdateCommandVisible = true;
            DeleteCommandVisible = true;
            Title = "JQDataGrid";
            QueryTitle = "Query";
            TotalCaption = "Total:";
            CheckOnSelect = true;
            ColumnsHibeable = false;
            RowNumbers = true;
        }

        private string remoteName;
        /// <summary>
        /// 数据源
        /// </summary>
        [Category("Infolight")]
        [Editor(typeof(RemoteNameEditor), typeof(UITypeEditor))]
        public string RemoteName
        {
            get
            {
                return remoteName;
            }
            set
            {
                if (this.DesignMode && value != remoteName)
                {
                    columnCaptions = null;
                }
                remoteName = value;
            }
        }

        private string dataMember;
        /// <summary>
        /// 表名
        /// </summary>
        [Category("Infolight")]
        [Editor(typeof(DataMemberEditor), typeof(UITypeEditor))]
        public string DataMember
        {
            get
            {
                return dataMember;
            }
            set
            {
                if (this.DesignMode && value != dataMember)
                {
                    columnCaptions = null;
                }
                dataMember = value;
            }
        }

        /// <summary>
        /// 是否自动保存
        /// </summary>
        [Category("Infolight")]
        public bool AutoApply { get; set; }

        [Category("Infolight")]
        public bool AlwaysClose { get; set; }

        [Category("Infolight")]
        public bool EditOnEnter { get; set; }

        [Category("Infolight")]
        public string IDField { get; set; }

        [Category("Infolight")]
        public bool MultiSelect { get; set; }

        [Category("Infolight")]
        [Editor(typeof(GridControlEditor), typeof(UITypeEditor))]
        public string MultiSelectGridID { get; set; }

        private bool _AllowAdd = true;
        /// <summary>
        /// 是否自动保存
        /// </summary>
        [Category("Infolight")]
        public bool AllowAdd
        {
            get
            {
                return _AllowAdd;
            }
            set
            {
                _AllowAdd = value;
            }
        }
        private bool _AllowUpdate = true;
        /// <summary>
        /// 是否自动保存
        /// </summary>
        [Category("Infolight")]
        public bool AllowUpdate
        {
            get
            {
                return _AllowUpdate;
            }
            set
            {
                _AllowUpdate = value;
            }
        }
        private bool _AllowDelete = true;
        /// <summary>
        /// 是否自动保存
        /// </summary>
        [Category("Infolight")]
        public bool AllowDelete
        {
            get
            {
                return _AllowDelete;
            }
            set
            {
                _AllowDelete = value;
            }
        }

        [Category("Infolight")]
        [EditorAttribute(typeof(RDCLUrlEditor), typeof(UITypeEditor))]
        public string ReportFileName { set; get; }

        [Category("Infolight")]
        public bool ViewCommandVisible { get; set; }

        private bool _InsertCommandVisible = true;
        [Category("Infolight"), Browsable(false)]
        public bool InsertCommandVisible
        {
            get
            {
                if (_AllowAdd == false)
                    _InsertCommandVisible = _AllowAdd;
                return _InsertCommandVisible;
            }
            set
            {
                _InsertCommandVisible = value;
            }
        }

        private bool _UpdateCommandVisible = true;
        [Category("Infolight")]
        public bool UpdateCommandVisible
        {
            get
            {
                if (_AllowUpdate == false)
                    _UpdateCommandVisible = _AllowUpdate;
                return _UpdateCommandVisible;
            }
            set
            {
                _UpdateCommandVisible = value;
            }
        }

        private bool _DeleteCommandVisible = true;
        [Category("Infolight")]
        public bool DeleteCommandVisible
        {
            get
            {
                if (_AllowDelete == false)
                    _DeleteCommandVisible = _AllowDelete;
                return _DeleteCommandVisible;
            }
            set
            {
                _DeleteCommandVisible = value;
            }
        }

        /// <summary>
        /// 父控件
        /// </summary>
        [Category("Infolight")]
        [Editor(typeof(DataControlEditor), typeof(UITypeEditor))]
        public string ParentObjectID { get; set; }

        /// <summary>
        /// 编辑窗口
        /// </summary>
        [Category("Infolight")]
        [Editor(typeof(WindowControlEditor), typeof(UITypeEditor))]
        public string EditDialogID { get; set; }

        [Category("Infolight")]
        [Obsolete("共用editDialog,多一個 editMode")]
        [Browsable(false)]
        [Editor(typeof(FormControlEditor), typeof(UITypeEditor))]
        public string ExpandFormID { get; set; }

        /// <summary>
        /// 编辑模式
        /// </summary>
        [Category("Infolight")]
        [Browsable(false)]
        public EidtModeType EditMode { get; set; }
        
        [Category("Infolight")]
        public bool BufferView { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [Category("Infolight")]
        public string Title { get; set; }

        /// <summary>
        /// 查询标题
        /// </summary>
        [Category("Infolight")]
        public string QueryTitle { get; set; }

        /// <summary>
        /// Windows時開窗的位置
        /// </summary>
        [Category("Infolight")]
        public Unit QueryTop { get; set; }
        /// <summary>
        /// Windows時開窗的位置
        /// </summary>
        [Category("Infolight")]
        public Unit QueryLeft { get; set; }

        /// <summary>
        /// 是否分页
        /// </summary>
        [Category("Infolight")]
        public bool Pagination { get; set; }



        [Category("Infolight")]
        public bool RecordLock { get; set; }

        [Category("Infolight")]
        public RecordLockType RecordLockMode { get; set; }

        [Category("Infolight")]
        public bool CheckOnSelect { get; set; }

        /// <summary>
        /// 分页大小
        /// </summary>
        [Category("Infolight")]
        public int PageSize { get; set; }

        [Category("Infolight")]
        public string PageList { get; set; }

        [Category("Infolight")]
        public bool RowNumbers { get; set; }

        /// <summary>
        /// 条件模式
        /// </summary>
        [Category("Infolight")]
        public QueryModeType QueryMode { get; set; }

        /// <summary>
        /// 自动Render查询条件
        /// </summary>
        [Category("Infolight")]
        public bool QueryAutoColumn { get; set; }

        private JQCollection<JQGridColumn> columns;
        [Category("Infolight")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [NotifyParentProperty(true)]
        public JQCollection<JQGridColumn> Columns
        {
            get
            {
                return columns;
            }
        }

        private JQCollection<JQRelationColumn> relationColumns;
        [Category("Infolight")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [NotifyParentProperty(true)]
        public JQCollection<JQRelationColumn> RelationColumns
        {
            get
            {
                return relationColumns;
            }
        }

        private JQCollection<JQToolItem> toolitems;
        [Category("Infolight")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [NotifyParentProperty(true)]
        public JQCollection<JQToolItem> TooItems
        {
            get
            {
                return toolitems;
            }
        }

        private JQCollection<JQQueryColumn> queryColumns;
        [Category("Infolight")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [NotifyParentProperty(true)]
        public JQCollection<JQQueryColumn> QueryColumns
        {
            get
            {
                return queryColumns;
            }
        }

        [Category("Infolight")]
        public string TotalCaption { get; set; }

        [Category("Infolight")]
        public string OnView { get; set; }

        [Category("Infolight")]
        public string OnInsert { get; set; }

        [Category("Infolight")]
        public string OnUpdate { get; set; }

        [Category("Infolight")]
        public string OnDelete { get; set; }

        [Category("Infolight")]
        public string OnLoadSuccess { get; set; }

        [Category("Infolight")]
        public string OnSelect { get; set; }

        [Category("Infolight")]
        public string OnInserted { get; set; }

        [Category("Infolight")]
        public string OnDeleting { get; set; }

        [Category("Infolight")]
        public string OnDeleted { get; set; }


        [Category("Infolight")]
        public string OnUpdated { get; set; }

        [Category("Infolight")]
        public bool ColumnsHibeable { get; set; }
        [Category("Infolight")]
        public bool NotInitGrid { get; set; }

        [Category("Infolight")]
        public string HelpLink { get; set; }

        protected override void OnLoad(EventArgs e)
        {
            if (!this.DesignMode)
            {
                if (this.QueryColumns.Count > 0 && this.QueryMode == QueryModeType.Panel)
                {
                    //this.Page.ClientScript.RegisterStartupScript(typeof(string), "", string.Format("setQueryDefault('#{0}')", QueryDialogID), true);
                }
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public NameValueCollection QueryString
        {
            get
            {
                if (this.Page != null)
                {
                    var scriptManager = this.Page.Controls.OfType<JQScriptManager>().FirstOrDefault();
                    if (scriptManager == null)
                    {
                        scriptManager = this.Page.Form.Controls.OfType<JQScriptManager>().FirstOrDefault();
                    }
                    if (scriptManager != null)
                    {
                        return scriptManager.QueryString;
                    }
                }
                return new NameValueCollection();
            }
        }

        internal void CheckProperties()
        {
            if (string.IsNullOrEmpty(RemoteName))
            {
                throw new JQProperyNullException(this.ID, typeof(JQDataGrid), "RemoteName");
            }
            foreach (var column in this.Columns)
            {
                column.CheckProperties();
            }
            foreach (var column in this.QueryColumns)
            {
                column.CheckProperties();
            }
            foreach (var column in this.RelationColumns)
            {
                column.CheckProperties();
            }
            foreach (var item in this.TooItems)
            {
                item.CheckProperties();
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (!this.DesignMode)
            {
                EFClientTools.ClientUtility.ClientInfo.cErrorCode = 8110;
                CheckProperties();
                //create query panel
                if (this.QueryColumns.Count > 0)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Id, QueryDialogID);
                    if (this.QueryMode == QueryModeType.Window)
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Class, JQControl.Window);
                        writer.AddAttribute(JQProperty.DataOptions
                            , "iconCls:'icon-search',closed:true,collapsible:false,maximizable:false,minimizable:false");

                    }
                    else // if (this.QueryMode == QueryModeType.Panel)
                    {                   
                        writer.AddAttribute(HtmlTextWriterAttribute.Class, JQControl.Panel);
                        writer.AddAttribute(JQProperty.DataOptions
                            , "iconCls:'icon-search',closed:false,collapsible:true,maximizable:false,minimizable:false");
                    }

                    writer.AddAttribute(HtmlTextWriterAttribute.Title, QueryTitle);
                    writer.AddAttribute(HtmlTextWriterAttribute.Style, "padding:10px");
                    
                    if (this.QueryMode != QueryModeType.Window)
                    {
                        var queryStyles = new List<string>();
                        queryStyles.Add("padding:10px");
                        if (this.Width.Type == UnitType.Pixel && Width.Value > double.Epsilon)
                        {
                            queryStyles.Add(string.Format("width:{0}px", Width.Value));
                        }
                        writer.AddAttribute(HtmlTextWriterAttribute.Style, string.Join(";", queryStyles));
                    }

                    writer.RenderBeginTag(HtmlTextWriterTag.Div);
                    writer.RenderBeginTag(HtmlTextWriterTag.Table);

                    //if (QueryColumns.Count == 1 && QueryColumns[0].NewLine == false)
                    //{
                    //    writer.RenderBeginTag(HtmlTextWriterTag.Tr);
                    //    QueryColumns[0].Render(writer);
                    //    RenderQueryButton(writer, 1);
                    //    writer.RenderEndTag();//tr
                    //}
                    //else
                    //{

                        writer.RenderBeginTag(HtmlTextWriterTag.Tr);
                        var maxspan = 0;
                        var colspan = 0;
                  
                        foreach (var column in QueryColumns)
                        {
                            if (this.QueryMode == QueryModeType.Fuzzy && QueryColumns.IndexOf(column) > 0)
                            {
                                column.Caption = " ";
                                column.AndOr = JQAndOr.Or;
                                column.NewLine = true;
                            }
                            if (column.NewLine && QueryColumns.IndexOf(column) > 0)
                            {
                                if (this.QueryMode == QueryModeType.Fuzzy && QueryColumns[0].NewLine == false)
                                {
                                    RenderQueryButton(writer, 1);
                                }

                                writer.RenderEndTag();
                                if (this.QueryMode == QueryModeType.Fuzzy)
                                {
                                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "fuzzy");
                                    writer.AddStyleAttribute(HtmlTextWriterStyle.Display, "none");
                                }
                                writer.RenderBeginTag(HtmlTextWriterTag.Tr);
                                maxspan = Math.Max(maxspan, colspan);
                                colspan = 0;
                            }

                            //writer.RenderBeginTag(HtmlTextWriterTag.Td);
                            column.Render(writer);
                            //writer.RenderEndTag();//td
                            if (column.Span > 1)
                                colspan += column.Span;
                            else
                                colspan += 2;
                        }
                        maxspan = Math.Max(maxspan, colspan);


                        if (QueryColumns[0].NewLine == true)
                        {
                            writer.RenderEndTag();//tr

                            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
                        }
                        else {
                            maxspan = 1;
                        }
                        RenderQueryButton(writer, maxspan);
                        writer.RenderEndTag();//tr
                    //}

                    writer.RenderEndTag();//table
                    writer.RenderEndTag();//div
                }

                //create grid
                writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ID);
                writer.AddAttribute(HtmlTextWriterAttribute.Class, JQControl.DataGrid);
                writer.AddAttribute(HtmlTextWriterAttribute.Title, Title);
                var styles = new List<string>();
                if (this.Width.Type == UnitType.Pixel && Width.Value > double.Epsilon)
                {
                    styles.Add(string.Format("width:{0}px", Width.Value));

                }
                if (this.Height.Type == UnitType.Pixel && Height.Value > double.Epsilon)
                {
                    styles.Add(string.Format("height:{0}px", Height.Value));
                }
                if (styles.Count > 0)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Style, string.Join(";", styles));
                }
                writer.AddAttribute(JQProperty.DataOptions, this.DataOptions);
                writer.AddAttribute(JQProperty.InfolightOptions, this.InfolightOptions);
                var jqDefault = this.Parent.Controls.OfType<JQDefault>().FirstOrDefault(c => c.BindingObjectID == this.ID);
                if (jqDefault == null && this.Parent.Parent != null)
                {
                    jqDefault = this.Parent.Parent.Controls.OfType<JQDefault>().FirstOrDefault(c => c.BindingObjectID == this.ID);
                }
                var jqValidate = this.Parent.Controls.OfType<JQValidate>().FirstOrDefault(c => c.BindingObjectID == this.ID);
                if (jqValidate == null && this.Parent.Parent != null)
                {
                    jqValidate = this.Parent.Parent.Controls.OfType<JQValidate>().FirstOrDefault(c => c.BindingObjectID == this.ID);
                }

                writer.RenderBeginTag(HtmlTextWriterTag.Table);
                //frozen columns
                List<JQGridColumn> frozenColumns = new List<JQGridColumn>();
                foreach (var column2 in columns)
                {
                    if (column2.Frozen)
                    {
                        frozenColumns.Add(column2);
                    }
                }
                if (frozenColumns.Count > 0)
                {
                    writer.AddAttribute(JQProperty.DataOptions, "frozen:true");
                    writer.RenderBeginTag(HtmlTextWriterTag.Thead);
                    writer.RenderBeginTag(HtmlTextWriterTag.Tr);
                    if (MultiSelect)
                    {
                        writer.AddAttribute(JQProperty.DataOptions, "checkbox:true");
                        writer.RenderBeginTag(HtmlTextWriterTag.Th);
                        writer.RenderEndTag();
                    }
                    foreach (var column in frozenColumns)
                    {
                        //if (!column.Visible)
                        //    continue;
                        if (jqDefault != null)
                        {
                            var defaultColumn = jqDefault.Columns.FirstOrDefault(c => c.FieldName == column.FieldName);
                            if (defaultColumn != null)
                            {
                                //column.Default = EFClientTools.ClientUtility.GetSysValue(defaultColumn.Value);
                                column.Default = defaultColumn.Value;
                                column.CarryOn = defaultColumn.CarryOn;
                            }
                        }
                        if (jqValidate != null)
                        {
                            var validateColumn = jqValidate.Columns.FirstOrDefault(c => c.FieldName == column.FieldName);
                            if (validateColumn != null)
                            {
                                column.Validate = validateColumn.Value;
                            }
                        }
                        var jqAutoSeq = this.Parent.Controls.OfType<JQAutoSeq>().FirstOrDefault(c => c.BindingObjectID == this.ID && c.FieldName == column.FieldName);
                        if (jqAutoSeq == null && this.Parent.Parent != null)
                        {
                            jqAutoSeq = this.Parent.Parent.Controls.OfType<JQAutoSeq>().FirstOrDefault(c => c.BindingObjectID == this.ID && c.FieldName == column.FieldName);
                        }

                        if (jqAutoSeq != null)
                        {
                            column.AutoSeq = jqAutoSeq.Value;
                        }
                        column.Render(writer);
                    }
                    writer.RenderEndTag();
                    writer.RenderEndTag();
                }
                //other columns
                writer.RenderBeginTag(HtmlTextWriterTag.Thead);
                writer.RenderBeginTag(HtmlTextWriterTag.Tr);

                if (MultiSelect && frozenColumns.Count == 0)
                {
                    writer.AddAttribute(JQProperty.DataOptions, "checkbox:true");
                    writer.RenderBeginTag(HtmlTextWriterTag.Th);
                    writer.RenderEndTag();
                }

                foreach (var column in Columns)
                {
                    //if (!column.Visible)
                    //    continue;
                    if (frozenColumns.Contains(column))
                        continue;

                    if (jqDefault != null)
                    {
                        var defaultColumn = jqDefault.Columns.FirstOrDefault(c => c.FieldName == column.FieldName);
                        if (defaultColumn != null)
                        {
                            //column.Default = EFClientTools.ClientUtility.GetSysValue(defaultColumn.Value);
                            column.Default = defaultColumn.Value;
                            column.CarryOn = defaultColumn.CarryOn;
                        }
                    }
                    if (jqValidate != null)
                    {
                        var validateColumn = jqValidate.Columns.FirstOrDefault(c => c.FieldName == column.FieldName);
                        if (validateColumn != null)
                        {
                            column.Validate = validateColumn.Value;
                        }
                    }
                    var jqAutoSeq = this.Parent.Controls.OfType<JQAutoSeq>().FirstOrDefault(c => c.BindingObjectID == this.ID && c.FieldName == column.FieldName);
                    if (jqAutoSeq == null && this.Parent.Parent != null)
                    {
                        jqAutoSeq = this.Parent.Parent.Controls.OfType<JQAutoSeq>().FirstOrDefault(c => c.BindingObjectID == this.ID && c.FieldName == column.FieldName);
                    }

                    if (jqAutoSeq != null)
                    {
                        column.AutoSeq = jqAutoSeq.Value;
                    }
                    column.Render(writer);
                }
                writer.RenderEndTag();
                writer.RenderEndTag();
                writer.RenderEndTag();
                if (QueryString["NAVIGATOR_MODE"] == "0" || QueryString["NAVMODE"] == "0")
                {
                    this.AllowAdd = false;
                    this.AllowDelete = false;
                    this.AllowUpdate = false;
                }

                //create toolbar
                if (this.TooItems.Count > 0)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Id, ToolBarID);
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, JQControl.ToolBar);
                    writer.AddAttribute(HtmlTextWriterAttribute.Height, "auto");
                    writer.RenderBeginTag(HtmlTextWriterTag.Div);
                    foreach (var item in TooItems)
                    {
                        if (!this.AllowAdd && item.OnClick == "insertItem")
                            continue;
                        if (!this.AllowUpdate && item.OnClick == "updateItem")
                            continue;
                        if (!this.AllowDelete && item.OnClick == "deleteItem")
                            continue;
                        if (!this.AllowAdd && !this.AllowUpdate && !this.AllowDelete
                            && (item.OnClick == "apply" || item.OnClick == "cancel"))
                            continue;

                        item.Render(writer);
                    }
                    writer.RenderEndTag();
                }
                EFClientTools.ClientUtility.ClientInfo.cErrorCode = 0;
            }
        }

        private void RenderQueryButton(HtmlTextWriter writer, int maxspan)
        {
            //writer.RenderBeginTag(HtmlTextWriterTag.Td);
            //writer.RenderEndTag();//td
            writer.AddAttribute(HtmlTextWriterAttribute.Align, "right");
            writer.AddAttribute(HtmlTextWriterAttribute.Colspan, maxspan.ToString());
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            writer.AddAttribute(HtmlTextWriterAttribute.Href, "javascript:void(0)");
            writer.AddAttribute(HtmlTextWriterAttribute.Class, JQControl.LinkButton + " infosysbutton-q");
            //writer.AddAttribute(HtmlTextWriterAttribute.Style, "text-align: right");
            writer.AddAttribute(JQProperty.DataOptions, "iconCls:'icon-search'");
            writer.AddAttribute(HtmlTextWriterAttribute.Onclick, string.Format("query('#{0}')", this.ID));
            //writer.AddAttribute(HtmlTextWriterAttribute.Style, "width:66px");
            writer.RenderBeginTag(HtmlTextWriterTag.A);
            writer.Write("Query");
            writer.RenderEndTag();//a
            //writer.RenderEndTag();//td

            //writer.RenderBeginTag(HtmlTextWriterTag.Td);
            writer.AddAttribute(HtmlTextWriterAttribute.Href, "javascript:void(0)");
            writer.AddAttribute(HtmlTextWriterAttribute.Class, JQControl.LinkButton + " infosysbutton-cl");
            //writer.AddAttribute(HtmlTextWriterAttribute.Style, "text-align: right");
            writer.AddAttribute(JQProperty.DataOptions, "iconCls:'icon-undo'");
            writer.AddAttribute(HtmlTextWriterAttribute.Onclick, string.Format("clearQuery('#{0}')", this.ID));
            //writer.AddAttribute(HtmlTextWriterAttribute.Style, "width:66px");
            writer.RenderBeginTag(HtmlTextWriterTag.A);
            writer.Write("Clear");
            writer.RenderEndTag();//a

            writer.RenderEndTag();//td
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string DataOptions
        {
            get
            {
                //var optionBuilder = new StringBuilder();
                //if (this.TooItems.Count > 0)
                //{
                //    optionBuilder.AppendFormat("toolbar:'#{0}'", ToolBarID);
                //    optionBuilder.Append(",");
                //}
                //optionBuilder.AppendFormat("pagination:{0}", Pagination.ToString().ToLower());
                //optionBuilder.Append(",");
                //if (!string.IsNullOrEmpty(EditDialogID))
                //{
                //    optionBuilder.AppendFormat("view:{0}", "commandview");
                //}
                //else if (!string.IsNullOrEmpty(ExpandFormID))
                //{
                //    optionBuilder.AppendFormat("view:{0}", "detailview");
                //}
                //else
                //{
                //    optionBuilder.AppendFormat("view:{0}", "defaultview");
                //}
                //return optionBuilder.ToString().TrimEnd(',');

                var options = new List<string>();
                if (this.TooItems.Count > 0)
                {
                    options.Add(string.Format("toolbar:'#{0}'", ToolBarID));
                }
                options.Add(string.Format("pagination:{0}", Pagination.ToString().ToLower()));
                if (!string.IsNullOrEmpty(PageList))
                {
                    options.Add(string.Format("pageList:[{0}]", PageList));
                }
                options.Add(string.Format("pageSize:{0}", PageSize));
         
                if (!string.IsNullOrEmpty(EditDialogID))
                {
                    if (EditMode == EidtModeType.Dialog)
                        options.Add(string.Format("view:{0}", "commandview"));
                    else if (EditMode == EidtModeType.Expand)
                        options.Add(string.Format("view:{0}", "detailview"));
                    else
                        options.Add(string.Format("view:{0}", "commandview"));
                }
                //else if (!string.IsNullOrEmpty(ExpandFormID))
                //{
                //    options.Add(string.Format("view:{0}", "detailview"));
                //}
                else
                {
                    if (DeleteCommandVisible || UpdateCommandVisible || ViewCommandVisible)
                    {
                        options.Add(string.Format("view:{0}", "commandview"));
                    }
                }
                if (!string.IsNullOrEmpty(OnSelect))
                {
                    options.Add(string.Format("onSelect:{0}", OnSelect));
                }
                options.Add(string.Format("checkOnSelect:{0}", CheckOnSelect.ToString().ToLower()));
                if (!string.IsNullOrEmpty(IDField))
                {
                    options.Add(string.Format("idField:'{0}'", IDField));
                }
                if (!string.IsNullOrEmpty(HelpLink))
                {
                    options.Add(string.Format("tools:[{{iconCls:'icon-help',handler:function(){{window.open('{0}');}}}}]", HelpLink));
                }
                return string.Join(",", options);
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string InfolightOptions
        {
            get
            {
                //var optionBuilder = new StringBuilder();
                //optionBuilder.AppendFormat("autoApply:{0}", AutoApply.ToString().ToLower());
                //optionBuilder.Append(",");
                //optionBuilder.AppendFormat("remoteName:'{0}'", RemoteName);
                //optionBuilder.Append(",");
                //optionBuilder.AppendFormat("tableName:'{0}'", DataMember);
                //optionBuilder.Append(",");
                //if (this.QueryColumns.Count > 0)
                //{
                //    optionBuilder.AppendFormat("queryDialog:'#{0}'", QueryDialogID);
                //    optionBuilder.Append(",");
                //}
                //if (!string.IsNullOrEmpty(EditDialogID))
                //{
                //    optionBuilder.AppendFormat("editDialog:'#{0}'", EditDialogID);
                //    optionBuilder.Append(",");
                //}
                //if (!string.IsNullOrEmpty(ExpandFormID))
                //{
                //    optionBuilder.AppendFormat("expandForm:'#{0}'", ExpandFormID);
                //    optionBuilder.Append(",");
                //}
                //if (!string.IsNullOrEmpty(ParentObjectID))
                //{
                //    optionBuilder.AppendFormat("parent:'{0}'", ParentObjectID);
                //    optionBuilder.Append(",");

                //    var relationBuilder = new StringBuilder();
                //    foreach (var column in this.RelationColumns)
                //    {
                //        if (relationBuilder.Length > 0)
                //        {
                //            relationBuilder.Append(";");
                //        }
                //        relationBuilder.AppendFormat("{0}={1}", column.FieldName, column.ParentFieldName);
                //    }
                //    optionBuilder.AppendFormat("parentRelations:'{0}'", relationBuilder);
                //    optionBuilder.Append(",");
                //}
                //optionBuilder.AppendFormat("queryAutoColumn:{0}", this.QueryAutoColumn.ToString().ToLower());
                //optionBuilder.Append(",");
                //optionBuilder.AppendFormat("commandButtons:'{0}{1}{2}'", ViewCommandVisible ? "v" : string.Empty, UpdateCommandVisible ? "u" : string.Empty
                //    , DeleteCommandVisible ? "d" : string.Empty);
                //return optionBuilder.ToString();


                var options = new List<string>();
                options.Add(string.Format("autoApply:{0}", AutoApply.ToString().ToLower()));
                options.Add(string.Format("editOnEnter:{0}", EditOnEnter.ToString().ToLower()));
                options.Add(string.Format("alwaysClose:{0}", AlwaysClose.ToString().ToLower()));
                options.Add(string.Format("remoteName:'{0}'", RemoteName));
                options.Add(string.Format("tableName:'{0}'", DataMember));
                options.Add(string.Format("duplicateCheck:{0}", DuplicateCheck.ToString().ToLower()));
                options.Add(string.Format("recordLock:{0}", RecordLock.ToString().ToLower()));
                options.Add(string.Format("recordLockMode:'{0}'", RecordLockMode.ToString().ToLower()));
                options.Add(string.Format("rowNumbers:{0}", RowNumbers.ToString().ToLower()));
                if (this.QueryColumns.Count > 0)
                {
                    options.Add(string.Format("queryDialog:'#{0}'", QueryDialogID));
                }
                options.Add(string.Format("bufferView:{0}", BufferView.ToString().ToLower()));
                if (!string.IsNullOrEmpty(EditDialogID))
                {
                    var jqdialog = this.Parent.Controls.OfType<JQDialog>().FirstOrDefault(c => c.ID == EditDialogID);
                    if (jqdialog == null)
                    {
                        jqdialog = this.Page.Controls.OfType<JQDialog>().FirstOrDefault(c => c.ID == EditDialogID);
                    }
                    if (jqdialog == null)
                    {
                        jqdialog = this.Page.Form.Controls.OfType<JQDialog>().FirstOrDefault(c => c.ID == EditDialogID);
                    }
                    if (jqdialog != null)
                    {

                        options.Add(string.Format("editDialog:'#{0}'", EditDialogID));
                        options.Add(string.Format("editMode:'{0}'", jqdialog.EditMode));
                    }
                }
                //if (!string.IsNullOrEmpty(ExpandFormID))
                //{
                //    options.Add(string.Format("expandForm:'#{0}'", ExpandFormID));
                //}
                if (!string.IsNullOrEmpty(ParentObjectID))
                {
                    options.Add(string.Format("parent:'{0}'", ParentObjectID));

                    var relations = new List<string>();
                    foreach (var column in this.RelationColumns)
                    {
                        relations.Add(string.Format("{{field:'{0}',parentField:'{1}'}}", column.FieldName, column.ParentFieldName));
                    }
                    options.Add(string.Format("parentRelations:[{0}]", string.Join(",", relations)));
                }
                options.Add(string.Format("queryAutoColumn:{0}", this.QueryAutoColumn.ToString().ToLower()));
                options.Add(string.Format("commandButtons:'{0}{1}{2}'", ViewCommandVisible ? "v" : string.Empty, UpdateCommandVisible ? "u" : string.Empty
                    , DeleteCommandVisible ? "d" : string.Empty));
                options.Add(string.Format("totalCaption:'{0}'", TotalCaption));
                options.Add(string.Format("allowInsert:{0}", this.AllowAdd.ToString().ToLower()));
                options.Add(string.Format("allowUpdate:{0}", this.AllowUpdate.ToString().ToLower()));
                options.Add(string.Format("allowDelete:{0}", this.AllowDelete.ToString().ToLower()));
                if (!string.IsNullOrEmpty(this.OnView))
                {
                    options.Add(string.Format("onView:{0}", this.OnView));
                }
                if (!string.IsNullOrEmpty(this.OnInsert))
                {
                    options.Add(string.Format("onInsert:{0}", this.OnInsert));
                }
                if (!string.IsNullOrEmpty(this.OnUpdate))
                {
                    options.Add(string.Format("onUpdate:{0}", this.OnUpdate));
                }
                if (!string.IsNullOrEmpty(this.OnDelete))
                {
                    options.Add(string.Format("onDelete:{0}", this.OnDelete));
                }
                if (!string.IsNullOrEmpty(this.ReportFileName))
                {
                    options.Add(string.Format("reportFileName:'{0}'", this.ReportFileName));
                }
                if (!string.IsNullOrEmpty(OnLoadSuccess))
                {
                    options.Add(string.Format("onLoadSuccess:{0}", this.OnLoadSuccess));
                }
                if (!string.IsNullOrEmpty(OnInserted))
                {
                    options.Add(string.Format("onInserted:{0}", OnInserted));
                }
                if (!string.IsNullOrEmpty(OnDeleting))
                {
                    options.Add(string.Format("onDeleting:{0}", OnDeleting));
                }
                if (!string.IsNullOrEmpty(OnDeleted))
                {
                    options.Add(string.Format("onDeleted:{0}", OnDeleted));
                }
                if (!string.IsNullOrEmpty(OnUpdated))
                {
                    options.Add(string.Format("onUpdated:{0}", OnUpdated));
                }
                if (this.QueryLeft.Type == UnitType.Pixel && !this.QueryLeft.IsEmpty)
                {
                    options.Add(string.Format("queryLeft:{0}", this.QueryLeft.Value));
                }
                if (this.QueryTop.Type == UnitType.Pixel && !this.QueryTop.IsEmpty)
                {
                    options.Add(string.Format("queryTop:{0}", this.QueryTop.Value));
                }
                options.Add(string.Format("multiSelect:{0}", this.MultiSelect.ToString().ToLower()));

                if (!string.IsNullOrEmpty(MultiSelectGridID))
                {
                    options.Add(string.Format("multiSelectGrid:'#{0}'", MultiSelectGridID));
                }

                options.Add(string.Format("columnsHibeable:{0}", this.ColumnsHibeable.ToString().ToLower()));
                if (NotInitGrid)
                {
                    options.Add(string.Format("notInitGrid:{0}", this.NotInitGrid.ToString().ToLower()));
                }

                return string.Join(",", options);
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string ToolBarID
        {
            get
            {
                return string.Format("toolbar{0}", this.ID);
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string QueryDialogID
        {
            get
            {
                return string.Format("query{0}", this.ID);
            }
        }
        /// <summary>
        /// 是否DuplicateCheck
        /// </summary>
        [Category("Infolight")]
        public bool DuplicateCheck { get; set; }

        private Dictionary<string, string> columnCaptions;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        Dictionary<string, string> IColumnCaptions.ColumnCaptions
        {
            get
            {
                if (this.DesignMode)
                {
                    if (string.IsNullOrEmpty(RemoteName) || string.IsNullOrEmpty(DataMember))
                    {
                        return null;
                    }
                    if (columnCaptions == null)
                    {
                        columnCaptions = new Dictionary<string, string>();
                        var assemblyName = RemoteName.Split('.')[0];
                        var commandName = RemoteName.Split('.')[1];
                        var clientInfo = EFClientTools.DesignClientUtility.ClientInfo;
                        clientInfo.UseDataSet = true;
                        var columnDefinations = EFClientTools.DesignClientUtility.Client.GetColumnDefination(clientInfo, assemblyName, DataMember, null)
                            .OfType<EFClientTools.EFServerReference.COLDEF>();
                        foreach (var columnDefination in columnDefinations)
                        {
                            columnCaptions.Add(columnDefination.FIELD_NAME
                                , string.IsNullOrEmpty(columnDefination.CAPTION) ? columnDefination.FIELD_NAME : columnDefination.CAPTION);
                        }
                    }
                }
                return columnCaptions;
            }
        }
    }


    public class JQGridColumn : JQCollectionItem, IJQDataSourceProvider, ICloneable
    {
        public JQGridColumn()
        {
            Alignment = "left";
            Width = 80;
            Editor = JQEditorControl.TextBox;
            QueryCondition = string.Empty;
            _DrillFields = new JQCollection<JQDrillDownFields>(this);
        }

        private string fieldName;
        /// <summary>
        /// 栏位名
        /// </summary>
        [Category("Infolight")]
        [Editor(typeof(FieldEditor), typeof(UITypeEditor))]
        public string FieldName
        {
            get
            {
                return fieldName;
            }
            set
            {
                fieldName = value;

                if (Component != null && Component.ColumnCaptions != null)
                {
                    if (string.IsNullOrEmpty(caption) && Component.ColumnCaptions.ContainsKey(fieldName))
                    {
                        Caption = Component.ColumnCaptions[fieldName];
                    }
                }
            }
        }

        private string caption;
        /// <summary>
        /// 标题
        /// </summary>
        [Category("Infolight")]
        public string Caption
        {
            get
            {
                if (string.IsNullOrEmpty(caption))
                {
                    return FieldName;
                }
                else
                {
                    return caption;
                }
            }
            set
            {
                caption = value;
            }
        }
        [Category("Infolight")]
        public string TableName { get; set; }

        [Category("Infolight")]
        public bool IsNvarChar { get; set; }

        /// <summary>
        /// 对齐
        /// </summary>
        [Category("Infolight")]
        [Editor(typeof(JQAlignmentEditor), typeof(UITypeEditor))]
        public string Alignment { get; set; }
        /// <summary>
        /// 宽度
        /// </summary>
        [Category("Infolight")]
        public int Width { get; set; }
        /// <summary>
        /// 编辑器
        /// </summary>
        [Category("Infolight")]
        [Editor(typeof(EditControlEditor), typeof(UITypeEditor))]
        public string Editor { get; set; }
        /// <summary>
        /// 编辑选项
        /// </summary>
        [Category("Infolight")]
        [Editor(typeof(EditorOptionsEditor), typeof(UITypeEditor))]
        public string EditorOptions { get; set; }

        [Category("Infolight")]
        [Editor(typeof(EditorOptionsEditor), typeof(UITypeEditor))]
        public string RelationOptions { get; set; }

        /// <summary>
        /// 最大输入字符数
        /// </summary>
        [Category("Infolight")]
        public int MaxLength { get; set; }

        /// <summary>
        /// 格式
        /// </summary>
        [Category("Infolight")]
        public string FormatScript { get; set; }

        /// <summary>
        /// 格式
        /// </summary>
        [Category("Infolight")]
        public string Format { get; set; }

        /// <summary>
        /// 汇总
        /// </summary>
        [Category("Infolight")]
        [Editor(typeof(TotalTypeEditor), typeof(UITypeEditor))]
        public string Total { get; set; }

        /// <summary>
        /// 汇总后触发的事件，自定义想要的结果
        /// </summary>
        [Category("Infolight")]
        public string OnTotal { get; set; }

        /// <summary>
        /// Sortable
        /// </summary>
        [Category("Infolight")]
        public bool Sortable { get; set; }
        /// <summary>
        /// Frozen Column
        /// </summary>
        [Category("Infolight")]
        public bool Frozen { get; set; }

        private bool _ReadOnly = false;
        /// <summary>
        /// ReadOnly Column
        /// </summary>
        [Category("Infolight")]
        public bool ReadOnly
        {
            get
            {
                return _ReadOnly;
            }
            set
            {
                _ReadOnly = value;
            }
        }

        private bool _Visible = true;
        /// <summary>
        /// Visible Column
        /// </summary>
        [Category("Infolight")]
        public bool Visible
        {
            get
            {
                return _Visible;
            }
            set
            {
                _Visible = value;
            }
        }

        [Category("Infolight")]
        [Editor(typeof(ConditionEditor), typeof(UITypeEditor))]
        public string QueryCondition { get; set; }

        [Category("Infolight")]
        public string PlaceHolder { get; set; }

        [Category("Infolight")]
        [Editor(typeof(DrillDownControlEditor), typeof(UITypeEditor))]
        public string DrillObjectID { get; set; }

        private JQCollection<JQDrillDownFields> _DrillFields;
        [Category("Infolight")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [NotifyParentProperty(true)]
        public JQCollection<JQDrillDownFields> DrillFields
        {
            get
            {
                return _DrillFields;
            }
        }
        internal void CheckProperties()
        {
            var controlID = ((this as IJQProperty).ParentProperty.Component as Control).ID;
            if (string.IsNullOrEmpty(FieldName) && this.Editor != JQEditorControl.FileUpload)
            {
                throw new JQProperyNullException(controlID, typeof(JQDataGrid), "Columns.FieldName");
            }
            if (this.Editor == JQEditorControl.RefValBox)
            {
                var editor = new JQRefval() { ID = string.Format("{0}_{1}", controlID, FieldName) };
                editor.LoadProperties(EditorOptions);
                editor.CheckProperties();
            }
            else if (this.Editor == JQEditorControl.ComboGrid)
            {
                var editor = new JQComboGrid() { ID = string.Format("{0}_{1}", controlID, FieldName) };
                editor.LoadProperties(EditorOptions);
                editor.CheckProperties();
            }
            else if (this.Editor == JQEditorControl.ComboBox)
            {
                var editor = new JQComboBox() { ID = string.Format("{0}_{1}", controlID, FieldName) };
                editor.LoadProperties(EditorOptions);
                editor.CheckProperties();
            }
        }

        public void Render(HtmlTextWriter writer)
        {
            writer.AddAttribute(JQProperty.DataOptions, this.DataOptions);
            writer.AddAttribute(JQProperty.InfolightOptions, this.InfolightOptions);
            writer.RenderBeginTag(HtmlTextWriterTag.Th);
            writer.Write(this.Caption);
            writer.RenderEndTag();
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        private string DataOptions
        {
            get
            {
                //var optionBuilder = new StringBuilder();
                //optionBuilder.AppendFormat("field:'{0}'", FieldName);
                //optionBuilder.Append(",");
                //optionBuilder.AppendFormat("width:{0}", Width);
                //optionBuilder.Append(",");
                //optionBuilder.AppendFormat("align:'{0}'", Alignment);
                //optionBuilder.Append(",");
                //if (Editor == JQEditorControl.ComboBox)
                //{
                //    optionBuilder.AppendFormat("formatter:{0}", "formatValue");
                //    optionBuilder.Append(",");
                //}

                //var editor = Editor;
                //if (Editor == JQEditorControl.TextBox && !string.IsNullOrEmpty(Validate))
                //{
                //    editor = JQEditorControl.ValidateBox;
                //}
                //var options = new StringBuilder();
                //if (!string.IsNullOrEmpty(Validate))
                //{
                //    options.Append(Validate);
                //    options.Append(",");
                //}
                //options.Append(EditorOptions);
                //optionBuilder.AppendFormat("editor:{{type:'{0}', options:{{{1}}}}}", editor, options);
                //return optionBuilder.ToString();

                var options = new List<string>();
                options.Add(string.Format("field:'{0}'", FieldName));
                options.Add(string.Format("width:{0}", Width));
                options.Add(string.Format("align:'{0}'", Alignment));
                if (!string.IsNullOrEmpty(TableName))
                {
                    options.Add(string.Format("tableName:'{0}'", TableName));
                }
                if (!string.IsNullOrEmpty(QueryCondition))
                {
                    options.Add(string.Format("queryCondition:'{0}'", QueryCondition));
                }
                if (!Visible)
                {
                    options.Add("hidden:true");
                }
                if (Sortable)
                {
                    options.Add(string.Format("sortable:'{0}'", Sortable.ToString().ToLower()));
                }
                if (!string.IsNullOrEmpty(FormatScript))
                {
                    options.Add(string.Format("formatter:{0}", FormatScript));
                }
                else if (Editor == JQEditorControl.ComboBox || Editor == JQEditorControl.RefValBox || Editor == JQEditorControl.ComboGrid || !string.IsNullOrEmpty(RelationOptions))
                {
                    options.Add(string.Format("formatter:{0}", "getTextField"));
                }
                else if (!string.IsNullOrEmpty(Format))
                {
                    options.Add(string.Format("formatter:{0}", "formatValue"));
                    options.Add(string.Format("format:'{0}'", Format));
                }
                else if (DrillObjectID != null && DrillObjectID != "")
                {
                    var drillFieldsString = "";
                    if (DrillFields != null && DrillFields.Count > 0)
                    {
                        foreach (var fields in DrillFields)
                        {
                            if (drillFieldsString != "") drillFieldsString += ";";
                            drillFieldsString += fields.FieldName;
                        }
                    }
                    else
                    {
                        drillFieldsString = FieldName;
                    }
                    options.Add(string.Format("formatter:{0}", "formatValue"));
                    options.Add(string.Format("format:'drilldown,drillObjectID:{0},drillFields:{1}'", DrillObjectID, drillFieldsString));
                }
                var editor = Editor;
                if (Editor == JQEditorControl.TextBox && !string.IsNullOrEmpty(Validate))
                {
                    editor = JQEditorControl.ValidateBox;
                }
                var editorOptions = new List<string>();

                if (!string.IsNullOrEmpty(Validate))
                {
                    editorOptions.Add(Validate);
                    if (Editor == JQEditorControl.DateBox && !Validate.Contains("validType"))
                    {
                        editorOptions.Add("validType:'datetime'");
                    }
                }
                else if (Editor == JQEditorControl.DateBox)
                {
                    editorOptions.Add("validType:'datetime'");
                }
                if (this.MaxLength > 0)
                {
                    editorOptions.Add("maxLength:" + this.MaxLength);
                }
                editorOptions.Add("disabled:" + this.ReadOnly.ToString().ToLower());
                if (!string.IsNullOrEmpty(RelationOptions))
                {
                    editorOptions.Add(RelationOptions);
                }
                else if (!string.IsNullOrEmpty(EditorOptions))
                {
                    editorOptions.Add(EditorOptions);
                }
                if (!string.IsNullOrEmpty(PlaceHolder))
                {
                    editorOptions.Add(string.Format("placeholder:'{0}'", PlaceHolder));
                }
                options.Add(string.Format("editor:{{type:'{0}', options:{{{1}}}}}", editor, string.Join(",", editorOptions)));
                return string.Join(",", options);
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        private string InfolightOptions
        {
            get
            {
                //var optionBuilder = new StringBuilder();
                //if (!string.IsNullOrEmpty(Default))
                //{
                //    optionBuilder.AppendFormat("field:'{0}'", FieldName);
                //    optionBuilder.Append(",");
                //    optionBuilder.AppendFormat("defaultValue:'{0}'", Default);
                //}
                //return optionBuilder.ToString();

                var options = new List<string>();
                options.Add(string.Format("field:'{0}'", FieldName));
                options.Add("disabled:" + this.ReadOnly.ToString().ToLower());
                if (!string.IsNullOrEmpty(Default))
                {
                    options.Add(string.Format("defaultValue:'{0}'", Default));
                }
                if (CarryOn)
                {
                    options.Add(string.Format("carryOn:{0}", CarryOn.ToString().ToLower()));
                }
                if (!string.IsNullOrEmpty(AutoSeq))
                {
                    options.Add(string.Format("autoSeq:[{{{0}}}]", AutoSeq));
                }
                if (!string.IsNullOrEmpty(Total))
                {
                    options.Add(string.Format("total:'{0}'", Total));
                    if (!string.IsNullOrEmpty(OnTotal))
                    {
                        options.Add(string.Format("onTotal:'{0}'", OnTotal));
                    }
                }
                return string.Join(",", options);
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Default { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool CarryOn { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Validate { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string AutoSeq { get; set; }

        public override string ToString()
        {
            if (!string.IsNullOrEmpty(FieldName))
            {
                return this.FieldName;
            }
            else
            {
                return base.ToString();
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IColumnCaptions Component
        {
            get
            {
                if ((this as IJQProperty).ParentProperty != null && (this as IJQProperty).ParentProperty.Component != null)
                {
                    return (this as IJQProperty).ParentProperty.Component as IColumnCaptions;
                }
                return null;
            }
        }

        #region IJQDataSourceProvider Members

        string IJQDataSourceProvider.RemoteName
        {
            get
            {
                return ((this as IJQProperty).ParentProperty.Component as IJQDataSourceProvider).RemoteName;
            }
            set { }
        }

        string IJQDataSourceProvider.DataMember
        {
            get
            {
                return ((this as IJQProperty).ParentProperty.Component as IJQDataSourceProvider).DataMember;
            }
            set { }
        }

        #endregion



        #region ICloneable Members

        public object Clone()
        {
            var column = this.MemberwiseClone() as JQGridColumn;
            column._DrillFields = new JQCollection<JQDrillDownFields>(column);
            foreach (var drillField in this.DrillFields)
            {
                column.DrillFields.Add(drillField.Clone() as JQDrillDownFields);
            }
            return column;
        }

        #endregion
    }

    public class JQRelationColumn : JQCollectionItem, IJQDataSourceProvider
    {
        /// <summary>
        /// 栏位名
        /// </summary>
        [Category("Infolight")]
        [Editor(typeof(FieldEditor), typeof(UITypeEditor))]
        public string FieldName { get; set; }

        /// <summary>
        /// 栏位名
        /// </summary>
        [Category("Infolight")]
        [Editor(typeof(ParentFieldEditor), typeof(UITypeEditor))]
        public string ParentFieldName { get; set; }

        internal void CheckProperties()
        {
            if (string.IsNullOrEmpty(this.FieldName))
            {
                var controlID = ((this as IJQProperty).ParentProperty.Component as Control).ID;
                throw new JQProperyNullException(controlID, typeof(JQDataGrid), "RelationColumns.FieldName");
            }
            if (string.IsNullOrEmpty(this.ParentFieldName))
            {
                var controlID = ((this as IJQProperty).ParentProperty.Component as Control).ID;
                throw new JQProperyNullException(controlID, typeof(JQDataGrid), "RelationColumns.ParentFieldName");
            }
        }

        #region IJQDataSourceProvider Members

        string IJQDataSourceProvider.RemoteName
        {
            get
            {
                return ((this as IJQProperty).ParentProperty.Component as IJQDataSourceProvider).RemoteName;
            }
            set { }
        }

        string IJQDataSourceProvider.DataMember
        {
            get
            {
                return ((this as IJQProperty).ParentProperty.Component as IJQDataSourceProvider).DataMember;
            }
            set { }
        }

        #endregion
    }

    public class JQToolItem : JQCollectionItem
    {
        public JQToolItem()
        {
            ItemType = JQControl.LinkButton;
            ID = string.Empty;
        }

        [Category("Infolight")]
        [DefaultValue("")]
        public string ID { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        [Category("Infolight")]
        [Editor(typeof(IconEditor), typeof(UITypeEditor))]
        public string Icon { get; set; }
        /// <summary>
        /// 按钮类型
        /// </summary>
        [Category("Infolight")]
        public string ItemType { get; set; }
        /// <summary>
        /// 文字
        /// </summary>
        [Category("Infolight")]
        public string Text { get; set; }
        /// <summary>
        /// 按钮脚本
        /// </summary>
        [Category("Infolight")]
        public string OnClick { get; set; }

        private bool _Visible = true;
        /// <summary>
        /// 是否显示
        /// </summary>
        [Category("Infolight")]
        public bool Visible
        {
            get
            {
                return _Visible;
            }
            set
            {
                _Visible = value;
            }
        }

        private bool _Enabled = true;
        /// <summary>
        /// 是否显示
        /// </summary>
        [Category("Infolight")]
        public bool Enabled
        {
            get
            {
                return _Enabled;
            }
            set
            {
                _Enabled = value;
            }
        }

        private static string GetToolItemText(string text)
        {
            if (System.Web.HttpContext.Current != null)
            {
                //EFBase.MessageProvider provider = new EFBase.MessageProvider(System.Web.HttpContext.Current.Request.PhysicalApplicationPath, EFClientTools.ClientUtility.ClientInfo.Locale);
                var provider = new JQMessageProvider(System.Web.HttpContext.Current.Request.PhysicalApplicationPath, EFClientTools.ClientUtility.ClientInfo, EFClientTools.ClientUtility.ClientInfo.Locale);
                var itemtexts = provider["JQWebClient/toolitemtext"];
                //provider = new EFBase.MessageProvider(System.Web.HttpContext.Current.Request.PhysicalApplicationPath, string.Empty);
                provider = new JQMessageProvider(System.Web.HttpContext.Current.Request.PhysicalApplicationPath, EFClientTools.ClientUtility.ClientInfo, string.Empty);
                var itemnames = provider["JQWebClient/toolitemtext"];
                if (!string.IsNullOrEmpty(itemtexts) && !string.IsNullOrEmpty(itemnames))
                {
                    var names = itemnames.Split(',').ToList();
                    var texts = itemtexts.Split(',');
                    var index = names.IndexOf(text);
                    if (index >= 0 && index < texts.Length)
                    {
                        return texts[index];
                    }
                }
            }

            return text;
        }

        /// <summary>
        /// 新增
        /// </summary>
        public static JQToolItem InsertItem
        {
            get
            {

                return new JQToolItem() { Text = GetToolItemText("Insert"), Icon = JQIcon.Add, OnClick = "insertItem" };
            }
        }

        /// <summary>
        /// 编辑
        /// </summary>
        public static JQToolItem UpdateItem
        {
            get
            {
                return new JQToolItem() { Text = GetToolItemText("Update"), Icon = JQIcon.Edit, OnClick = "updateItem" };
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        public static JQToolItem DeleteItem
        {
            get
            {
                return new JQToolItem() { Text = GetToolItemText("Delete"), Icon = JQIcon.Remove, OnClick = "deleteItem" };
            }
        }

        /// <summary>
        /// 保存单笔
        /// </summary>
        public static JQToolItem OKItem
        {
            get
            {
                return new JQToolItem() { Text = GetToolItemText("OK"), Icon = JQIcon.OK, OnClick = "ok" };
            }
        }
        /// <summary>
        /// 保存
        /// </summary>
        public static JQToolItem ApplyItem
        {
            get
            {
                return new JQToolItem() { Text = GetToolItemText("Apply"), Icon = JQIcon.Save, OnClick = "apply" };
            }
        }
        /// <summary>
        /// 取消
        /// </summary>
        public static JQToolItem CancelItem
        {
            get
            {
                return new JQToolItem() { Text = GetToolItemText("Cancel"), Icon = JQIcon.Cancel, OnClick = "cancel" };
            }
        }
        /// <summary>
        /// 查询
        /// </summary>
        public static JQToolItem QueryItem
        {
            get
            {
                return new JQToolItem() { Text = GetToolItemText("Query"), Icon = JQIcon.Search, OnClick = "openQuery" };
            }
        }

        /// <summary>
        /// 输出
        /// </summary>
        public static JQToolItem ExportItem
        {
            get
            {
                return new JQToolItem() { Text = GetToolItemText("Export"), Icon = JQIcon.Excel, OnClick = "exportGrid" };
            }
        }

        /// <summary>
        /// 导入
        /// </summary>
        public static JQToolItem ImportItem
        {
            get
            {
                return new JQToolItem() { Text = GetToolItemText("Import"), Icon = JQIcon.View, OnClick = "importGrid" };
            }
        }

        /// <summary>
        /// Report
        /// </summary>
        public static JQToolItem ReportItem
        {
            get
            {
                return new JQToolItem() { Text = GetToolItemText("Report"), Icon = JQIcon.Excel, OnClick = "reportviewerGrid" };
            }
        }

        internal void CheckProperties()
        {

        }

        public void Render(HtmlTextWriter writer)
        {
            if (this.Visible)
            {
                var contorlID = ((this as IJQProperty).ParentProperty.Component as Control).ID;
                writer.AddAttribute(HtmlTextWriterAttribute.Id, !string.IsNullOrEmpty(ID) ? ID : string.Format("toolItem{0}{1}", contorlID, Text));
                writer.AddAttribute(HtmlTextWriterAttribute.Name, this.OnClick);
                writer.AddAttribute(HtmlTextWriterAttribute.Href, "javascript:void(0)");
                writer.AddAttribute(HtmlTextWriterAttribute.Class, JQControl.LinkButton);
                if (!Enabled)
                    writer.AddAttribute(HtmlTextWriterAttribute.Disabled, "Disabled");
                writer.AddAttribute(JQProperty.DataOptions, this.DataOptions);

                writer.AddAttribute(HtmlTextWriterAttribute.Onclick, string.Format("{0}('#{1}')", this.OnClick, contorlID));
                writer.RenderBeginTag(HtmlTextWriterTag.A);
                writer.Write(this.Text);
                writer.RenderEndTag();
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string DataOptions
        {
            get
            {
                //var optionBuilder = new StringBuilder();
                //optionBuilder.AppendFormat("iconCls:'{0}'", Icon);
                //optionBuilder.Append(",");
                //optionBuilder.AppendFormat("plain:{0}", bool.TrueString.ToLower());
                //return optionBuilder.ToString();

                var options = new List<string>();
                options.Add(string.Format("iconCls:'{0}'", Icon));
                options.Add(string.Format("plain:{0}", bool.TrueString.ToLower()));
                return string.Join(",", options);
            }
        }

        public override string ToString()
        {
            if (!string.IsNullOrEmpty(Text))
            {
                return Text;
            }
            else
            {
                return base.ToString();
            }
        }
    }

    public class JQQueryColumn : JQCollectionItem, IJQDataSourceProvider
    {
        public JQQueryColumn()
        {
            Condition = "%";
            Editor = JQEditorControl.TextBox;
            DataType = "string";
            NewLine = true;
            Width = 125;
            AndOr = "and";
        }

        private string fieldName;
        /// <summary>
        /// 栏位名
        /// </summary>
        [Category("Infolight")]
        [Editor(typeof(FieldEditor), typeof(UITypeEditor))]
        public string FieldName
        {
            get
            {
                return fieldName;
            }
            set
            {
                fieldName = value;

                if (Component != null && Component.ColumnCaptions != null)
                {
                    if (Component.ColumnCaptions.ContainsKey(fieldName))
                    {
                        Caption = Component.ColumnCaptions[fieldName];
                    }
                }
            }
        }

        [Category("Infolight")]
        public string TableName { get; set; }

        [Category("Infolight")]
        public bool IsNvarChar { get; set; }

        private string caption;
        /// <summary>
        /// 标题
        /// </summary>
        [Category("Infolight")]
        public string Caption
        {
            get
            {
                if (string.IsNullOrEmpty(caption))
                {
                    return FieldName;
                }
                else
                {
                    return caption;
                }
            }
            set
            {
                caption = value;
            }
        }
        /// <summary>
        /// 宽度
        /// </summary>
        [Category("Infolight")]
        public int Width { get; set; }
        /// <summary>
        /// 条件
        /// </summary>
        [Category("Infolight")]
        [Editor(typeof(ConditionEditor), typeof(UITypeEditor))]
        public string Condition { get; set; }
        /// <summary>
        /// 编辑器
        /// </summary>
        [Category("Infolight")]
        [Editor(typeof(EditControlEditor), typeof(UITypeEditor))]
        public string Editor { get; set; }
        /// <summary>
        /// 编辑选项
        /// </summary>
        [Category("Infolight")]
        [Editor(typeof(EditorOptionsEditor), typeof(UITypeEditor))]
        public string EditorOptions { get; set; }

        /// <summary>
        /// 默认值
        /// </summary>
        [Category("Infolight")]
        public string DefaultValue { get; set; }

        /// <summary>
        /// 默认方法
        /// </summary>
        [Category("Infolight")]
        public string DefaultMethod { get; set; }

        /// <summary>
        /// 是否后台方法
        /// </summary>
        [Category("Infolight")]
        public bool RemoteMethod { get; set; }

        /// <summary>
        /// 条件
        /// </summary>
        [Category("Infolight")]
        [Editor(typeof(AndOrEditor), typeof(UITypeEditor))]
        public string AndOr { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Value
        {
            get
            {
                var values = new List<string>();
                if (!string.IsNullOrEmpty(this.DefaultValue))
                {
                    if (this.DefaultValue.StartsWith("_"))
                    {
                        values.Add(string.Format("remote[{0}]", DefaultValue));
                    }
                    else
                    {
                        values.Add(DefaultValue.Replace("'", "\'"));
                    }
                }
                else if (!string.IsNullOrEmpty(DefaultMethod))
                {
                    if (RemoteMethod)
                    {
                        values.Add(string.Format("remote[{0}]", DefaultMethod));
                    }
                    else
                    {
                        values.Add(string.Format("client[{0}]", DefaultMethod));
                    }
                }
                return string.Join(",", values);
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Type
        {
            get
            {
                if (!string.IsNullOrEmpty(Editor))
                {
                    switch (Editor)
                    {
                        case JQEditorControl.CheckBox:
                            return "checkbox";
                    }
                }
                return "text";
            }
        }


        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Class
        {
            get
            {
                if (!string.IsNullOrEmpty(Editor))
                {
                    switch (Editor)
                    {
                        case JQEditorControl.ComboBox:
                            return JQControl.ComboBox;
                        case JQEditorControl.ComboGrid:
                            return JQControl.ComboGrid;
                        case JQEditorControl.DateBox:
                            return JQControl.DateBox;
                        case JQEditorControl.NumberBox:
                            return JQControl.NumberBox;
                        case JQEditorControl.TextBox:
                            return string.Empty;
                        case JQEditorControl.RefValBox:
                            return JQControl.RefValBox;
                        case JQEditorControl.TimeSpinner:
                            return JQControl.TimeSpinner;
                        case JQEditorControl.Options:
                            return JQControl.Options;
                        case JQEditorControl.YearMonth:
                            return JQControl.YearMonth;
                        default: break;
                    }
                }
                return string.Empty;
            }
        }

        private string dataType;
        /// <summary>
        /// 数据类型
        /// </summary>
        [Category("Infolight")]
        [Editor(typeof(DataTypeEditor), typeof(UITypeEditor))]
        public string DataType
        {
            get
            {
                return dataType;
            }
            set
            {
                if (Component != null && Component.ColumnCaptions != null)
                {
                    if (value != dataType)
                    {
                        if (value == JQDataType.String)
                        {
                            Condition = JQCondtion.BeginWith;
                        }
                        else
                        {
                            Condition = JQCondtion.Equal;
                        }
                    }
                }
                dataType = value;
            }
        }

        /// <summary>
        /// 是否另起一行
        /// </summary>
        [Category("Infolight")]
        public bool NewLine { get; set; }

        /// <summary>
        /// 格式
        /// </summary>
        [Category("Infolight")]
        public string Format { get; set; }

        [Category("Infolight")]
        public int Span { get; set; }

        [Category("Infolight")]
        public int RowSpan { get; set; }

        internal void CheckProperties()
        {
            if (string.IsNullOrEmpty(this.FieldName))
            {
                var controlID = ((this as IJQProperty).ParentProperty.Component as Control).ID;
                throw new JQProperyNullException(controlID, typeof(JQDataGrid), "QueryColumns.FieldName");
            }
        }

        public void Render(HtmlTextWriter writer)
        {
            if (this.RowSpan > 1)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Rowspan, this.RowSpan.ToString());
            }
            writer.AddAttribute(HtmlTextWriterAttribute.Nowrap, "nowrap");
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            writer.Write(this.Caption);
            writer.RenderEndTag();

            if (this.Span > 1)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Colspan, ((this.Span - 1) * 2 + 1).ToString());
            }
            if (this.RowSpan > 1)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Rowspan, this.RowSpan.ToString());
            }
            writer.AddAttribute(HtmlTextWriterAttribute.Nowrap, "nowrap");
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            writer.AddAttribute(HtmlTextWriterAttribute.Type, this.Type);
            if (!string.IsNullOrEmpty(this.Class))
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, this.Class);
            }
            writer.AddStyleAttribute(HtmlTextWriterStyle.Width, this.Width.ToString() + "px");
            writer.AddAttribute(JQProperty.DataOptions, this.DataOptions);
            writer.AddAttribute(JQProperty.InfolightOptions, this.InfolightOption);
            writer.AddAttribute(HtmlTextWriterAttribute.Id, FieldName + "_Query");
            if (string.Equals(this.Editor, JQEditorControl.ComboBox.ToString(), StringComparison.CurrentCultureIgnoreCase))
            {
                var editoptions = this.EditorOptions;
                var combobox = new JQComboBox();
                combobox.LoadProperties(editoptions);

                if (combobox.RemoteName != null && combobox.RemoteName != "")
                {
                    writer.RenderBeginTag(HtmlTextWriterTag.Input);
                }
                else
                {
                    writer.RenderBeginTag(HtmlTextWriterTag.Select);
                    if (combobox.Items != null)
                    {
                        foreach (JQComboItem item in combobox.Items)
                        {
                            if (item.Selected)
                            {
                                writer.Write("<option value=\"" + item.Value + "\" selected >" + item.Text + "</option>");
                            }
                            else
                                writer.Write("<option value=\"" + item.Value + "\">" + item.Text + "</option>");
                        }
                    }
                }
            }
            //else if (Editor == JQEditorControl.TextArea)
            //{
            //    writer.RenderBeginTag(HtmlTextWriterTag.Textarea);
            //}
            else
            {
                var editoptions = this.EditorOptions;
                var textbox = new JQTextBox();
                textbox.LoadProperties(editoptions);
                if (textbox.CapsLock != CapsLockEnum.None)
                {
                    writer.AddAttribute("onKeyUp", string.Format("$.changeCapsLock2($(this), '{0}');", textbox.CapsLock.ToString().ToLower()));
                }
                writer.RenderBeginTag(HtmlTextWriterTag.Input);
            }
            writer.RenderEndTag();
            writer.RenderEndTag();
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        private string DataOptions
        {
            get
            {
                var options = new List<string>();
                if (this.Editor == JQEditorControl.RefValBox || this.Editor == JQEditorControl.ComboGrid
                  || this.Editor == JQEditorControl.ComboBox || this.Editor == JQEditorControl.AutoComplete
                  || this.Editor == JQEditorControl.Options || this.Editor == JQEditorControl.CheckBox)
                {

                }
                else
                {
                    if (!string.IsNullOrEmpty(EditorOptions))
                    {
                        options.Add(EditorOptions);
                    }
                }
                return string.Join(",", options);
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string InfolightOption
        {
            get
            {
                //var optionBuilder = new StringBuilder();
                //optionBuilder.AppendFormat("field:'{0}'", FieldName);
                //optionBuilder.Append(",");
                //optionBuilder.AppendFormat("condition:'{0}'", Condition);
                //optionBuilder.Append(",");
                //optionBuilder.AppendFormat("dataType:'{0}'", DataType);
                //return optionBuilder.ToString();


                var options = new List<string>();
                options.Add(string.Format("field:'{0}'", FieldName));
                if (!string.IsNullOrEmpty(TableName))
                {
                    options.Add(string.Format("table:'{0}'", TableName));
                }
                options.Add(string.Format("caption:'{0}'", Caption));
                options.Add(string.Format("condition:'{0}'", Condition));
                options.Add(string.Format("andOr:'{0}'", AndOr == "" ? "and" : AndOr));
                options.Add(string.Format("dataType:'{0}'", DataType));
                options.Add(string.Format("isNvarChar:{0}", IsNvarChar.ToString().ToLower()));
                options.Add(string.Format("format:'{0}'", Format));
                if (!string.IsNullOrEmpty(Value))
                {
                    options.Add(string.Format("defaultValue:'{0}'", Value));
                }
                if (this.Editor == JQEditorControl.RefValBox || this.Editor == JQEditorControl.ComboGrid 
                    || this.Editor == JQEditorControl.ComboBox || this.Editor == JQEditorControl.AutoComplete
                    || this.Editor == JQEditorControl.Options || this.Editor == JQEditorControl.CheckBox || this.Editor == JQEditorControl.YearMonth)
                {
                    if (!string.IsNullOrEmpty(EditorOptions))
                    {
                        options.Add(EditorOptions);
                    }
                }
                return string.Join(",", options);
            }
        }

        public override string ToString()
        {
            if (!string.IsNullOrEmpty(FieldName))
            {
                return FieldName;
            }
            else
            {
                return base.ToString();
            }
        }

        public IColumnCaptions Component
        {
            get
            {
                if ((this as IJQProperty).ParentProperty != null && (this as IJQProperty).ParentProperty.Component != null)
                {
                    return (this as IJQProperty).ParentProperty.Component as IColumnCaptions;
                }
                return null;
            }
        }

        #region IJQDataSourceProvider Members

        string IJQDataSourceProvider.RemoteName
        {
            get
            {
                return ((this as IJQProperty).ParentProperty.Component as IJQDataSourceProvider).RemoteName;
            }
            set { }
        }

        string IJQDataSourceProvider.DataMember
        {
            get
            {
                return ((this as IJQProperty).ParentProperty.Component as IJQDataSourceProvider).DataMember;
            }
            set { }
        }

        #endregion
    }

    public enum QueryModeType
    {
        Window,
        Panel,
        Fuzzy
    }
    public enum EidtModeType
    {
        Dialog,
        Expand,
        Switch,
        Continue
    }

    public enum RecordLockType
    {
        None,
        Reload
    }
}
public class RDCLUrlEditor : UrlEditor
{
    protected override string Filter
    {
        get
        {
            return "RDLC Files (*.rdlc)|*.rdlc|All Files (*.*)|*.*";
        }
    }
}