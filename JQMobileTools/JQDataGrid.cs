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
using System.IO;

namespace JQMobileTools
{
    [Designer(typeof(JQDataGridDesigner), typeof(IDesigner))]
    public class JQDataGrid : WebControl, IJQDataSourceProvider, IQueryObject, IThemeObject, IColumnCaptions
    {
        public JQDataGrid()
            : base()
        {
            RenderHeader = true;
            RenderFooter = false;
            PageSize = 10;
            columns = new JQCollection<JQGridColumn>(this);
            queryColumns = new JQCollection<JQQueryColumn>(this);
            toolItems = new JQCollection<JQToolItem>(this);
            GridViewType = JQMobileTools.GridViewType.Grid;
            ListColumnCount = 2;
            ListCaptionWidth = 120;
            ToolItemsPosition = ToolItemsPostionType.Both;
            AllowAdd = true;
            AllowUpdate = true;
            AllowDelete = true;
            AllowView = true;
            Title = "JQDataGrid";
            CacheMode = CacheModeType.None;
            CacheGlobal = false;
        }

        private string theme;
        public string Theme
        {
            get
            {
                if (string.IsNullOrEmpty(theme))
                {
                    var scriptManager = this.Parent.Controls.OfType<JQScriptManager>().FirstOrDefault();
                    return scriptManager != null ? scriptManager.Theme : string.Empty;
                }
                return theme;
            }
            set
            {
                theme = value;
            }
        }

        public bool RenderHeader { get; set; }

        public bool RenderFooter { get; set; }

        private string hearderTemplate;
        public string HeaderTemplate
        {
            get
            {
                if (string.IsNullOrEmpty(hearderTemplate))
                {
                    var scriptManager = this.Parent.Controls.OfType<JQScriptManager>().FirstOrDefault();
                    return scriptManager != null ? scriptManager.HeaderTemplate : string.Empty;
                }
                return hearderTemplate;
            }
            set
            {
                hearderTemplate = value;
            }
        }

        private string footerTemplate;
        public string FooterTemplate
        {
            get
            {
                if (string.IsNullOrEmpty(footerTemplate))
                {
                    var scriptManager = this.Parent.Controls.OfType<JQScriptManager>().FirstOrDefault();
                    return scriptManager != null ? scriptManager.FooterTemplate : string.Empty;
                }
                return footerTemplate;
            }
            set
            {
                footerTemplate = value;
            }
        }

        [Category("Infolight")]
        public GridViewType GridViewType
        {
            get;
            set;
        }

        [Category("Infolight")]
        public int ListColumnCount { get; set; }
        [Category("Infolight")]
        public int ListCaptionWidth { get; set; }

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

        [Category("Infolight")]
        public int PageSize { get; set; }

        [Category("Infolight")]
        public bool AlwaysClose { get; set; }

        [Category("Infolight")]
        public string Title { get; set; }

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
        public QueryModeType QueryMode { get; set; }

        private JQCollection<JQToolItem> toolItems;
        [Category("Infolight")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [NotifyParentProperty(true)]
        public JQCollection<JQToolItem> ToolItems
        {
            get
            {
                return toolItems;
            }
        }

        [Category("Infolight")]
        [Editor(typeof(FormControlEditor), typeof(UITypeEditor))]
        public string EditFormID { get; set; }

        /// <summary>
        /// 编辑模式
        /// </summary>
        [Category("Infolight")]
        public EidtModeType EditMode { get; set; }

        [Category("Infolight")]
        public CacheModeType CacheMode { get; set; }

        [Category("Infolight")]
        public bool CacheGlobal { get; set; }

        private string cacheDateTimeField;
        /// <summary>
        /// 栏位名
        /// </summary>
        [Category("Infolight")]
        [Editor(typeof(FieldEditor), typeof(UITypeEditor))]
        public string CacheDateTimeField
        {
            get
            {
                return cacheDateTimeField;
            }
            set
            {
                cacheDateTimeField = value;
            }
        }

        private string detailObjectID;
        [Category("Infolight")]
        [Editor(typeof(GridControlEditor), typeof(UITypeEditor))]
        public string DetailObjectID
        {
            get
            {
                return detailObjectID;
            }
            set
            {
                detailObjectID = value;
            }
        }

        [Category("Infolight")]
        public bool AllowAdd { get; set; }
        [Category("Infolight")]
        public bool AllowUpdate { get; set; }
        [Category("Infolight")]
        public bool AllowDelete { get; set; }
        [Category("Infolight")]
        public bool AllowView { get; set; }

        [Category("Infolight")]
        public ToolItemsPostionType ToolItemsPosition { get; set; }

        [Category("Infolight")]
        public bool AllowPopMenu { get; set; }

        [Category("Infolight")]
        [EditorAttribute(typeof(RDLCUrlEditor), typeof(UITypeEditor))]
        public string ReportFileName { set; get; }

        [Category("Infolight")]
        public string OnBeforeLoad { get; set; }
        [Category("Infolight")]
        public string OnLoadSuccess { get; set; }
        [Category("Infolight")]
        public string OnSelect { get; set; }
        [Category("Infolight")]
        public string OnEdit { get; set; }
        [Category("Infolight")]
        public string OnInsert { get; set; }
        [Category("Infolight")]
        public string OnDelete { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string QueryObjectID
        {
            get
            {
                return string.Format("{0}_query", this.ID);
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string ToolItemObjectID
        {
            get
            {
                return string.Format("{0}_toolitem", this.ID);
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool RenderPage
        {
            get
            {
                return this.Parent == null || !(this.Parent is JQTabItem);
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (!this.DesignMode)
            {
                var gridTheme = Theme;
                if (RenderPage)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ID);
                    writer.AddAttribute(JQProperty.DataRole, JQDataRole.Page);
                    writer.RenderBeginTag(HtmlTextWriterTag.Div);

                    
                    if (RenderHeader)
                    {
                        if (GridViewType == JQMobileTools.GridViewType.ListItem)
                        {
                            HeaderTemplate = "../MobileHeaderTemplate1.htm";
                        }
                        RenderTemplate(HeaderTemplate, writer);
                    }

                    if (!string.IsNullOrEmpty(gridTheme))
                    {
                        writer.AddAttribute(JQProperty.DataTheme, gridTheme);
                    }
                    writer.AddAttribute(JQProperty.DataRole, JQDataRole.Content);
                    writer.RenderBeginTag(HtmlTextWriterTag.Div);
                }


                if (this.QueryColumns.Count > 0 && (this.QueryMode == QueryModeType.Panel || this.QueryMode == QueryModeType.Fuzzy))
                {
                    var scriptManager = this.Parent.Controls.OfType<JQScriptManager>().FirstOrDefault();
                    var jQueryMobileVersion = scriptManager != null ? scriptManager.JQueryMobileVersion : string.Empty;
                    if (GridViewType == JQMobileTools.GridViewType.ListItem)
                    {
                        writer.AddStyleAttribute(HtmlTextWriterStyle.Display, "none");
                    }
                    writer.AddAttribute(HtmlTextWriterAttribute.Id, this.QueryObjectID);
                    if (string.Compare(jQueryMobileVersion, "1.4.2") >= 0)
                    {
                        
                        writer.AddAttribute("data-collapsed", "false");
                        writer.AddAttribute(JQProperty.DataRole, "collapsible");
                    }
                    writer.RenderBeginTag(HtmlTextWriterTag.Div);
                    if (string.Compare(jQueryMobileVersion, "1.4.2") >= 0)
                    {
                        writer.RenderBeginTag(HtmlTextWriterTag.H4);
                        writer.Write("Query");
                        writer.RenderEndTag();
                    }

                    if (!string.IsNullOrEmpty(gridTheme))
                    {
                        writer.AddAttribute(JQProperty.DataTheme, gridTheme);
                    }
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, JQClass.Query);
                    writer.AddAttribute(HtmlTextWriterAttribute.Style, "min-width: 100px; max-width: 500px");
                    writer.RenderBeginTag(HtmlTextWriterTag.Div);

                    foreach (var column in this.QueryColumns)
                    {
                        column.Render(writer);
                    }
                    writer.RenderEndTag();
                    writer.RenderEndTag();
                }

                var gridClass = JQClass.DataGrid;
                if (this.GridViewType == JQMobileTools.GridViewType.List || this.GridViewType == JQMobileTools.GridViewType.ListItem)
                {
                    gridClass += " " + JQClass.DataList;
                }
                writer.AddAttribute(HtmlTextWriterAttribute.Class, gridClass + " table-stripe infolight-breakpoint");
                writer.AddAttribute(JQProperty.DataRole, JQDataRole.Table);
                writer.AddAttribute(JQProperty.DataMode, JQDataMode.Reflow);
                if (!string.IsNullOrEmpty(gridTheme))
                {
                    writer.AddAttribute(JQProperty.DataTheme, gridTheme);
                }
                writer.AddAttribute(JQProperty.DataOptions, DataOptions);

                writer.RenderBeginTag(HtmlTextWriterTag.Table);
                writer.RenderBeginTag(HtmlTextWriterTag.Thead);
                writer.RenderBeginTag(HtmlTextWriterTag.Tr);

                foreach (var column in Columns)
                {
                    if (column.Visible)
                        column.Render(writer);
                }

                writer.RenderEndTag();
                writer.RenderEndTag();
                writer.RenderEndTag();

                JQScriptManager.RenderPopup(writer, string.Format("{0}_popup", ID));
               
                if (this.AllowPopMenu && this.GridViewType == JQMobileTools.GridViewType.ListItem)
                {
                    //render toolitemPopup
                    //writer.AddAttribute(HtmlTextWriterAttribute.Class, "ui-content");
                    writer.AddAttribute(HtmlTextWriterAttribute.Id, string.Format("{0}_listpopup", ID));
                    writer.AddAttribute(JQProperty.DataRole, JQDataRole.Popup);
                    writer.AddAttribute(JQProperty.DataTheme, JQDataTheme.D);
                    writer.RenderBeginTag(HtmlTextWriterTag.Div);

                    writer.AddAttribute(JQProperty.DataRole, JQDataRole.ListView);
                    writer.AddAttribute(JQProperty.DataTheme, JQDataTheme.B);
                    writer.AddAttribute(JQProperty.DataInSet, bool.TrueString.ToLower());
                    writer.RenderBeginTag(HtmlTextWriterTag.Ul);

                    writer.RenderBeginTag(HtmlTextWriterTag.Li);
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "view");
                    writer.RenderBeginTag(HtmlTextWriterTag.A);
                    writer.Write("View");
                    writer.RenderEndTag();
                    writer.RenderEndTag();

                    writer.RenderBeginTag(HtmlTextWriterTag.Li);
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "edit");
                    writer.RenderBeginTag(HtmlTextWriterTag.A);
                    writer.Write("Edit");
                    writer.RenderEndTag();
                    writer.RenderEndTag();

                    writer.RenderBeginTag(HtmlTextWriterTag.Li);
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "delete");
                    writer.RenderBeginTag(HtmlTextWriterTag.A);
                    writer.Write("Delete");
                    writer.RenderEndTag();
                    writer.RenderEndTag();

                    writer.RenderEndTag();
                    writer.RenderEndTag();
                }

                if (this.ToolItems.Count > 0)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ToolItemObjectID);
                    writer.AddAttribute(HtmlTextWriterAttribute.Style, "display:none");
                    writer.AddAttribute(JQProperty.DataRole, JQDataRole.None);
                    writer.RenderBeginTag(HtmlTextWriterTag.Div);
                    foreach (var item in ToolItems)
                    {
                        item.Render(writer);
                    }
                    writer.RenderEndTag();
                }

                if (RenderPage)
                {
                    writer.AddAttribute(JQProperty.DataRole, JQDataRole.Popup);
                    writer.AddAttribute(JQProperty.DataTheme, JQDataTheme.D);
                    writer.AddAttribute(HtmlTextWriterAttribute.Id, "popupSMS");
                    writer.AddAttribute(HtmlTextWriterAttribute.Style, "height:80px;width:200px");
                    writer.RenderBeginTag(HtmlTextWriterTag.Div);
                    //<textarea cols="35" rows="20" name="textarea" id="textarea"></textarea>  
                    writer.AddAttribute(HtmlTextWriterAttribute.Cols, "35");
                    writer.AddAttribute(HtmlTextWriterAttribute.Rows, "20");
                    writer.AddAttribute(HtmlTextWriterAttribute.Name, "textarea");
                    writer.AddAttribute(HtmlTextWriterAttribute.Id, "textarea");
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "taSMS");
                    writer.RenderBeginTag(HtmlTextWriterTag.Textarea);
                    writer.RenderEndTag();
                    //<a data-mini="true" data-role="button" class="sendMessage"  style="display: block">傳送</a>
                    writer.AddAttribute(JQProperty.DataMini, "true");
                    writer.AddAttribute(JQProperty.DataRole, JQDataRole.Button);
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "sendMessage");
                    writer.AddAttribute(HtmlTextWriterAttribute.Style, "display: block");
                    writer.RenderBeginTag(HtmlTextWriterTag.A);
                    writer.Write("傳送");
                    writer.RenderEndTag();

                    writer.RenderEndTag();

                    
                    writer.RenderEndTag();

                    if (RenderFooter)
                    {
                        RenderTemplate(FooterTemplate, writer);
                    }
                    //var chartlist = this.Parent.Controls.OfType<WebControl>().Where(c=>c.GetType().Namespace == "JQChartTools").ToList();
                    //foreach (var chart in chartlist)
                    //{
                    //    if (chart.GetType().GetProperty("RenderObjectID").GetValue(chart,null).ToString() == this.ID)
                    //    {
                    //        writer.AddAttribute(JQProperty.DataRole, JQDataRole.Content);
                    //        writer.RenderBeginTag(HtmlTextWriterTag.Div);
                    //        chart.RenderControl(writer);
                    //        writer.RenderEndTag();
                    //    }
                    //}
                    writer.RenderEndTag();


                    if (this.QueryColumns.Count > 0 && this.QueryMode == QueryModeType.Window)
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Id, this.QueryObjectID);
                        writer.AddAttribute(JQProperty.DataOverlayTheme, Theme);
                        writer.AddAttribute(JQProperty.DataRole, JQDataRole.Page);
                        writer.RenderBeginTag(HtmlTextWriterTag.Div);

                        writer.AddAttribute(JQProperty.DataRole, JQDataRole.Header);
                        if (!string.IsNullOrEmpty(gridTheme))
                        {
                            writer.AddAttribute(JQProperty.DataTheme, gridTheme);
                        }
                        writer.RenderBeginTag(HtmlTextWriterTag.Div);
                        writer.RenderEndTag();

                        if (!string.IsNullOrEmpty(gridTheme))
                        {
                            writer.AddAttribute(JQProperty.DataTheme, gridTheme);
                        }
                        writer.AddAttribute(JQProperty.DataRole, JQDataRole.Content);
                        writer.RenderBeginTag(HtmlTextWriterTag.Div);

                        if (!string.IsNullOrEmpty(gridTheme))
                        {
                            writer.AddAttribute(JQProperty.DataTheme, gridTheme);
                        }
                        writer.AddAttribute(HtmlTextWriterAttribute.Class, JQClass.Query);
                        writer.AddAttribute(HtmlTextWriterAttribute.Style, "min-width: 100px; max-width: 500px");
                        writer.RenderBeginTag(HtmlTextWriterTag.Div);

                        foreach (var column in this.QueryColumns)
                        {
                            column.Render(writer);
                        }
                        writer.RenderEndTag();
                        writer.RenderEndTag();
                        writer.RenderEndTag();
                    }


                }
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string DataOptions
        {
            get
            {
                var options = new List<string>();
                options.Add(string.Format("remoteName:'{0}'", RemoteName));
                options.Add(string.Format("tableName:'{0}'", DataMember));
                options.Add(string.Format("pageSize:{0}", PageSize));
                if (!string.IsNullOrEmpty(EditFormID))
                {
                    options.Add(string.Format("editPage:'#{0}'", EditFormID));
                    options.Add(string.Format("editMode:'{0}'", EditMode.ToString().ToLower()));
                }
                if (!string.IsNullOrEmpty(this.DetailObjectID))
                {
                    options.Add(string.Format("detailObjectID:'#{0}'", DetailObjectID));
                }
                if (this.QueryColumns.Count > 0)
                {
                    options.Add(string.Format("queryObjectID:'#{0}'", QueryObjectID));
                }
                if (this.ToolItems.Count > 0)
                {
                    options.Add(string.Format("toolItemObjectID:'#{0}'", ToolItemObjectID));
                }
                var jqParentObject = this.Parent.Controls.OfType<JQDataGrid>().FirstOrDefault(c => c.DetailObjectID == this.ID);
                if (jqParentObject == null)
                {
                    jqParentObject = this.Page.Form.Controls.OfType<JQDataGrid>().FirstOrDefault(c => c.DetailObjectID == this.ID);
                }
                if (jqParentObject != null)
                {
                    options.Add(string.Format("parentObjectID:'#{0}'", jqParentObject.ID));
                }
                if (RenderPage)
                {
                    options.Add(string.Format("title:'{0}'", Title));
                }
                options.Add(string.Format("alwaysClose:{0}", AlwaysClose.ToString().ToLower()));
                options.Add(string.Format("allowAdd:{0}", AllowAdd.ToString().ToLower()));
                options.Add(string.Format("allowUpdate:{0}", AllowUpdate.ToString().ToLower()));
                options.Add(string.Format("allowDelete:{0}", AllowDelete.ToString().ToLower()));
                options.Add(string.Format("allowView:{0}", AllowView.ToString().ToLower()));
                options.Add(string.Format("toolItemsPosition:'{0}'", ToolItemsPosition.ToString().ToLower()));
                options.Add(string.Format("allowPopMenu:{0}", (AllowPopMenu & this.GridViewType == JQMobileTools.GridViewType.ListItem).ToString().ToLower()));
                options.Add(string.Format("listColumnCount:{0}", ListColumnCount));
                options.Add(string.Format("listCaptionWidth:{0}", ListCaptionWidth));
                options.Add(string.Format("queryMode:'{0}'", QueryMode.ToString().ToLower()));
                options.Add(string.Format("gridViewType:'{0}'", GridViewType.ToString().ToLower()));
                options.Add(string.Format("reportFileName:'{0}'", ReportFileName));
                options.Add(string.Format("cacheMode:'{0}'", CacheMode.ToString().ToLower()));
                options.Add(string.Format("cacheGlobal:{0}", CacheGlobal.ToString().ToLower()));
                if (!string.IsNullOrEmpty(CacheDateTimeField))
                {
                    options.Add(string.Format("cacheDateTimeField:'{0}'", CacheDateTimeField));
                }

                if (!string.IsNullOrEmpty(OnBeforeLoad))
                {
                    options.Add(string.Format("onBeforeLoad:{0}", OnBeforeLoad));
                }
                if (!string.IsNullOrEmpty(OnLoadSuccess))
                {
                    options.Add(string.Format("onLoadSuccess:{0}", OnLoadSuccess));
                }
                if (!string.IsNullOrEmpty(OnSelect))
                {
                    options.Add(string.Format("onSelect:{0}", OnSelect));
                }
                if (!string.IsNullOrEmpty(OnInsert))
                {
                    options.Add(string.Format("onInsert:{0}", OnInsert));
                }
                if (!string.IsNullOrEmpty(OnEdit))
                {
                    options.Add(string.Format("onEdit:{0}", OnEdit));
                }
                if (!string.IsNullOrEmpty(OnDelete))
                {
                    options.Add(string.Format("onDelete:{0}", OnDelete));
                }
                return string.Join(",", options);
            }
        }

        private void RenderTemplate(string template, HtmlTextWriter writer)
        {
            if (!string.IsNullOrEmpty(template))
            {
                var filepath = this.Page.Server.MapPath(template);
                if (File.Exists(filepath))
                {
                    using (StreamReader reader = new StreamReader(filepath))
                    {
                        var html = reader.ReadToEnd();
                        writer.Write(html);
                    }
                }
            }
        }

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

    public enum GridViewType
    {
        Grid,
        List,
        ListItem
    }

    public enum ToolItemsPostionType
    {
        Bottom,
        Top,
        Both
    }

    public class JQGridColumn : JQCollectionItem, IJQDataSourceProvider
    {
        public JQGridColumn()
        {
            RelationOptions = string.Empty;
            Width = 90;
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

        [Category("Infolight")]
        public bool NoWrap { get; set; }

        [Category("Infolight")]
        public string Format { get; set; }

        [Category("Infolight")]
        public string FormatScript { get; set; }

        /// <summary>
        /// 汇总
        /// </summary>
        [Category("Infolight")]
        [Editor(typeof(TotalTypeEditor), typeof(UITypeEditor))]
        public string Total { get; set; }

        [Category("Infolight")]
        [Editor(typeof(EditorOptionsEditor), typeof(UITypeEditor))]
        public string RelationOptions { get; set; }

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

        public void Render(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Nowrap, "nowrap");
            writer.AddAttribute(JQProperty.DataOptions, DataOptions);
            writer.RenderBeginTag(HtmlTextWriterTag.Th);
            writer.Write(Caption);
            writer.RenderEndTag();
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string DataOptions
        {
            get
            {
                var options = new List<string>();
                options.Add(string.Format("field:'{0}'", FieldName));
                options.Add(string.Format("width:{0}", Width));
                options.Add(string.Format("align:'{0}'", Alignment));
                options.Add(string.Format("nowrap:{0}", NoWrap.ToString().ToLower()));
                var relationOption = JQControl.CreateControl(JQDisplayControl.Relation, RelationOptions) as JQRelation;
                if (!string.IsNullOrEmpty(relationOption.RemoteName))
                {
                    options.Add(string.Format("format:'R-{0}-{1}-{2}-{3}'", relationOption.RemoteName, relationOption.DataMember
                        , relationOption.DisplayMember, relationOption.ValueMember));

                    var whereItemsOptions = new List<string> { };
                    foreach (var item in relationOption.WhereItems)
                    {
                        whereItemsOptions.Add(string.Format("{{field:'{0}',whereValue:{{{1}}}}}", item.FieldName, item.Value));
                    }
                    options.Add(string.Format("whereItems:[{0}]", string.Join(",", whereItemsOptions)));
                }
                else if (relationOption.Items.Count > 0)
                {
                    options.Add("format:'R-Items'");

                    var formatParameters = new List<string> { };
                    foreach (var item in relationOption.Items)
                    {
                        formatParameters.Add(string.Format("{{value:'{0}', text:'{1}'}}", item.Value, item.Text));
                    }
                    options.Add(string.Format("formatParameters:[{0}]", string.Join(",", formatParameters)));
                }
                else if (!string.IsNullOrEmpty(Format))
                {
                    options.Add(string.Format("format:'{0}'", Format));
                }
                if (!string.IsNullOrEmpty(FormatScript))
                {
                    options.Add(string.Format("formatter:{0}", FormatScript));
                }
                if (!string.IsNullOrEmpty(Total))
                {
                    options.Add(string.Format("total:'{0}'", Total));
                }
                if (DrillObjectID != null && DrillObjectID != "")
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
                    options.Add(string.Format("formatParameters:'{0}'", "fullRow"));
                    options.Add(string.Format("format:'drilldown,drillObjectID:{0},drillFields:{1}'", DrillObjectID, drillFieldsString));
                }
                if (!Visible)
                {
                    options.Add("hidden:true");
                }
                return string.Join(",", options);
            }
        }

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
    }

    public class JQQueryColumn : JQDefaultColumn, IJQDataSourceProvider
    {
        public JQQueryColumn()
        {
            Condition = "%";
            Editor = JQEditorControl.Text;
            DataType = "string";
            EditorOptions = string.Empty;
        }

        private string fieldName;
        /// <summary>
        /// 栏位名
        /// </summary>
        [Category("Infolight")]
        [Editor(typeof(FieldEditor), typeof(UITypeEditor))]
        public new string FieldName
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

        [Category("Infolight")]
        public string TableName { get; set; }

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
        /// 条件
        /// </summary>
        [Category("Infolight")]
        [Editor(typeof(ConditionEditor), typeof(UITypeEditor))]
        public string Condition { get; set; }

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

        [Category("Infolight")]
        public bool IsNvarChar { get; set; }

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

        public void Render(HtmlTextWriter writer)
        {
            writer.AddAttribute(JQProperty.DataRole, JQDataRole.FieldContain);
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            var contorlID = ((this as IJQProperty).ParentProperty.Component as IQueryObject).QueryObjectID;

            var queryMode = (this as IJQProperty).ParentProperty.Component is JQDataGrid ? ((this as IJQProperty).ParentProperty.Component as JQDataGrid).QueryMode : QueryModeType.Panel;
            var index = ((this as IJQProperty).ParentProperty as System.Collections.IList).IndexOf(this);


            var fieldID = string.Format("{0}_{1}", contorlID, this.FieldName);

            if (this.Editor != JQEditorControl.RadioButtons && this.Editor != JQEditorControl.CheckBoxes)
            {

                //writer.AddAttribute(JQProperty.For, fieldID);
                writer.AddStyleAttribute("word-break", "keep-all");


                if (queryMode == QueryModeType.Fuzzy)
                {
                    writer.AddStyleAttribute(HtmlTextWriterStyle.Display, "none");
                }
                //writer.AddAttribute(HtmlTextWriterAttribute.Style, "word-break:keep-all");
                writer.RenderBeginTag(HtmlTextWriterTag.Label);
                writer.Write(Caption);
                writer.RenderEndTag();
            }

            writer.AddAttribute(HtmlTextWriterAttribute.Id, fieldID);
            writer.AddAttribute(HtmlTextWriterAttribute.Name, FieldName);
            writer.AddAttribute(JQProperty.DataMini, bool.TrueString.ToLower());
            if (queryMode == QueryModeType.Fuzzy && index > 0)
            {
                writer.AddAttribute(JQProperty.DataRole, "none");
                writer.AddStyleAttribute(HtmlTextWriterStyle.Display, "none");
            }
            
            var control = JQControl.CreateControl(this.Editor, this.EditorOptions);
            control.ID = fieldID;
            control.Caption = Caption;
            control.Theme = ((this as IJQProperty).ParentProperty.Component as IThemeObject).Theme;
            var dataOptions = DataOptions;
            if (!string.IsNullOrEmpty(control.EditorOptions))
            {
                dataOptions += string.Format(",{0}", control.EditorOptions);
            }
            writer.AddAttribute(JQProperty.DataOptions, dataOptions);
            control.Render(writer);
            writer.RenderEndTag();
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string DataOptions
        {
            get
            {
                var options = new List<string>();
                options.Add(string.Format("queryField:'{0}'", FieldName));

                options.Add(string.Format("queryTable:'{0}'", TableName));
                options.Add(string.Format("condition:'{0}'", Condition));
                options.Add(string.Format("dataType:'{0}'", DataType));
                options.Add(string.Format("isNvarChar:{0}", IsNvarChar.ToString().ToLower()));
                if (!string.IsNullOrEmpty(Value))
                {
                    var index = ((this as IJQProperty).ParentProperty as System.Collections.IList).IndexOf(this);
                    options.Add(string.Format("field:'{0}{1}'", FieldName, index));
                    options.Add(string.Format("defaultValue:{{{0}}}", Value));
                }
                return string.Join(",", options);
            }
        }

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
    }

    public class JQToolItem : JQCollectionItem
    {
        public JQToolItem()
        {
            Visible = true;
            DataRole = JQDataRole.None;
        }

        [Category("Infolight")]
        public string Name { get; set; }

        [Category("Infolight")]
        [Editor(typeof(IconEditor), typeof(UITypeEditor))]
        public string Icon { get; set; }

        [Category("Infolight")]
        public string Text { get; set; }

        [Category("Infolight")]
        public string OnClick { get; set; }

        [Category("Infolight")]
        public bool Visible { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string DataRole { get; set; }

        public static JQToolItem InsertItem = new JQToolItem() { Name = "grid-insert", Text = "Insert", Icon = JQDataIcon.Plus };
        public static JQToolItem PreviousPageItem = new JQToolItem() { Name = "grid-previous", Text = "Previous page", Icon = JQDataIcon.ArrowL };
        public static JQToolItem NextPageItem = new JQToolItem() { Name = "grid-next", Text = "Next page", Icon = JQDataIcon.ArrowR };
        public static JQToolItem QueryItem = new JQToolItem() { Name = "grid-query", Text = "Query", Icon = JQDataIcon.Search };
        public static JQToolItem RefreshItem = new JQToolItem() { Name = "grid-refresh", Text = "Refresh", Icon = JQDataIcon.Refresh };
        public static JQToolItem BackItem = new JQToolItem() { Name = "grid-return", Text = "Back", Icon = JQDataIcon.Back };

        public void Render(HtmlTextWriter writer)
        {
            if (this.Visible)
            {
                if (!string.IsNullOrEmpty(Name))
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, Name);
                }
                writer.AddAttribute(JQProperty.DataMini, bool.TrueString.ToLower());
                writer.AddAttribute(JQProperty.DataInline, bool.TrueString.ToLower());
                writer.AddAttribute(JQProperty.DataRole, DataRole);
                writer.AddAttribute(JQProperty.DataIcon, Icon);
                writer.AddAttribute(JQProperty.DataIconPos, JQDataIconPos.NoText);
                if (!string.IsNullOrEmpty(OnClick))
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Onclick, string.Format("{0}()", OnClick));
                }
                writer.RenderBeginTag(HtmlTextWriterTag.A);
                writer.Write(Text);
                writer.RenderEndTag();
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

    public enum EidtModeType
    {
        Dialog/*,
        Switch*/
    }

    public enum QueryModeType
    {
        Window,
        Panel,
        Fuzzy
    }

    public enum CacheModeType
    {
        None,
        All,
        Daily,
        Smart
    }
}
