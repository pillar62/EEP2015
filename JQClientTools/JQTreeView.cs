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

namespace JQClientTools
{
    [Designer(typeof(JQTreeViewDesigner), typeof(IDesigner))]
    public class JQTreeView : WebControl, IJQDataSourceProvider, IColumnCaptions
    {
        public JQTreeView()
        {
            columns = new JQCollection<JQTreeViewColumn>(this);
            menuitems = new JQCollection<JQTreeViewContextItem>(this);
            DialogTitle = "Dialog";
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

        public bool Checkbox { get; set; }

        /// <summary>
        /// 查询标题
        /// </summary>
        [Category("Infolight")]
        public string DialogTitle { get; set; }

        /// <summary>
        /// Node id
        /// </summary>
        [Category("Infolight")]
        [Editor(typeof(FieldEditor), typeof(UITypeEditor))]
        public string idField { get; set; }
        /// <summary>
        /// Node Text
        /// </summary>
        [Category("Infolight")]
        [Editor(typeof(FieldEditor), typeof(UITypeEditor))]
        public string textField { get; set; }
        /// <summary>
        /// Parent Node
        /// </summary>
        [Category("Infolight")]
        [Editor(typeof(FieldEditor), typeof(UITypeEditor))]
        public string parentField { get; set; }

        /// <summary>
        /// onClick function name
        /// </summary>
        [Category("Infolight")]
        public string onClick { get; set; }
        /// <summary>
        /// 啟動 TreeView時,ParentField=RootValue, 沒有設定時, 抓全部(null)
        /// </summary>
        [Category("Infolight")]
        public string RootValue { get; set; }

        private JQCollection<JQTreeViewColumn> columns;
        [Category("Infolight")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [NotifyParentProperty(true)]
        public JQCollection<JQTreeViewColumn> Columns
        {
            get
            {
                return columns;
            }
        }

        private JQCollection<JQTreeViewContextItem> menuitems;
        [Category("Infolight")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [NotifyParentProperty(true)]
        public JQCollection<JQTreeViewContextItem> Menutems
        {
            get
            {
                return menuitems;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            if (this.DesignMode)
            {

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
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (!this.DesignMode)
            {
                EFClientTools.ClientUtility.ClientInfo.cErrorCode = 8410;
                CheckProperties();

                //create container div
                if (this.Width.Type == UnitType.Pixel && Width.Value > double.Epsilon && this.Height.Type == UnitType.Pixel && Height.Value > double.Epsilon)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Style, string.Format("width:{0}px;height:{1}px;overflow:auto", Width.Value, Height.Value));
                }
                //writer.AddAttribute(HtmlTextWriterAttribute.Class, "panel-body");
                writer.RenderBeginTag(HtmlTextWriterTag.Div);//first div for width and height

                //create treeview
                writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ID);
                writer.AddAttribute(HtmlTextWriterAttribute.Class, JQControl.TreeView);
                writer.AddAttribute(JQProperty.DataOptions, this.DataOptions);
                writer.AddAttribute(JQProperty.InfolightOptions, this.InfolightOptions);

                writer.RenderBeginTag(HtmlTextWriterTag.Ul);

                //create  editdialog
                writer.AddAttribute(HtmlTextWriterAttribute.Title, DialogTitle);
                writer.AddAttribute(HtmlTextWriterAttribute.Id, treeViewDialogID);
                writer.AddAttribute(HtmlTextWriterAttribute.Style, "width:250px");
                writer.AddAttribute(HtmlTextWriterAttribute.Class, JQControl.Window);
                writer.AddAttribute(JQProperty.DataOptions
                    , "closed:true,collapsible:false,maximizable:false,minimizable:false");

                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                writer.RenderBeginTag(HtmlTextWriterTag.Table);

                writer.RenderBeginTag(HtmlTextWriterTag.Tr);
                foreach (var column in columns)
                {
                    if (column.NewLine)
                    {
                        writer.RenderEndTag();
                        writer.RenderBeginTag(HtmlTextWriterTag.Tr);
                    }

                    //writer.RenderBeginTag(HtmlTextWriterTag.Td);
                    column.Render(writer);
                    //writer.RenderEndTag();//td
                }
                writer.RenderEndTag();//tr

                writer.RenderBeginTag(HtmlTextWriterTag.Tr);
                writer.AddAttribute(HtmlTextWriterAttribute.Colspan, "3");
                writer.RenderBeginTag(HtmlTextWriterTag.Td);
                //writer.RenderEndTag();//td
                //writer.AddAttribute(HtmlTextWriterAttribute.Align, "center");
                //writer.RenderBeginTag(HtmlTextWriterTag.Td);
                writer.AddAttribute(HtmlTextWriterAttribute.Style, "text-align: center; padding: 5px");
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                writer.AddAttribute(HtmlTextWriterAttribute.Href, "javascript:void(0)");
                writer.AddAttribute(JQProperty.DataOptions, "iconCls:'icon-ok'");
                //writer.AddAttribute(HtmlTextWriterAttribute.Style, "width:82px");
                writer.AddAttribute(HtmlTextWriterAttribute.Class, JQControl.LinkButton + " infosysbutton-o");
                writer.AddAttribute(HtmlTextWriterAttribute.Onclick, string.Format("insertTreeNodeOK('#{0}')", this.ID));
                writer.RenderBeginTag(HtmlTextWriterTag.A);
                writer.Write("   OK   ");
                writer.RenderEndTag();//a
                writer.AddAttribute(HtmlTextWriterAttribute.Href, "javascript:void(0)");
                writer.AddAttribute(HtmlTextWriterAttribute.Class, JQControl.LinkButton + " infosysbutton-c");
                //writer.AddAttribute(HtmlTextWriterAttribute.Style, "text-align: right");
                writer.AddAttribute(JQProperty.DataOptions, "iconCls:'icon-cancel'");
                //writer.AddAttribute(HtmlTextWriterAttribute.Style, "width:82px");
                writer.AddAttribute(HtmlTextWriterAttribute.Onclick, string.Format("closeTreeNodeEditor('#{0}')", this.ID));
                writer.RenderBeginTag(HtmlTextWriterTag.A);
                writer.Write("Cancel");
                writer.RenderEndTag();//a

                writer.RenderEndTag();//td
                writer.RenderEndTag();//tr

                writer.RenderEndTag();//table
                writer.RenderEndTag();//div

                //create menu
                if (this.Menutems.Count > 0)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Id, contextMenuID);
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "easyui-menu");
                    writer.AddAttribute(HtmlTextWriterAttribute.Width, "120px");
                    writer.AddAttribute(HtmlTextWriterAttribute.Height, "auto");
                    writer.RenderBeginTag(HtmlTextWriterTag.Div);
                    foreach (var item in Menutems)
                    {
                        item.Render(writer);
                    }
                    writer.RenderEndTag();
                }
                writer.RenderEndTag();//div
                writer.RenderEndTag();//ul
                writer.RenderEndTag();//first div
                EFClientTools.ClientUtility.ClientInfo.cErrorCode = 0;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string DataOptions
        {
            get
            {
                return "";
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string InfolightOptions
        {
            get
            {
                var options = new List<string>();

                options.Add(string.Format("remoteName:'{0}'", RemoteName));
                options.Add(string.Format("tableName:'{0}'", DataMember));

                options.Add(string.Format("idField:'{0}'", idField));
                options.Add(string.Format("textField:'{0}'", textField));
                options.Add(string.Format("parentField:'{0}'", parentField));
                options.Add(string.Format("menuID:'#{0}'",contextMenuID));
                options.Add(string.Format("editDialog:'#{0}'",treeViewDialogID));
                options.Add(string.Format("rootValue:'{0}'",RootValue));
                options.Add(string.Format("checkbox:{0}", Checkbox.ToString().ToLower()));
                if (!string.IsNullOrEmpty(this.onClick))
                {
                    options.Add(string.Format("onClick:{0}", this.onClick));
                }

                return string.Join(",", options);
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string contextMenuID
        {
            get
            {
                return string.Format("contextMenu{0}", this.ID);
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string treeViewDialogID
        {
            get
            {
                return string.Format("treeViewDialog{0}", this.ID);
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

    public class JQTreeViewContextItem : JQCollectionItem
    {
        public JQTreeViewContextItem()
        {
        }

        /// <summary>
        /// 图标
        /// </summary>
        [Category("Infolight")]
        public string Icon { get; set; }
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
        /// <summary>
        /// Class
        /// </summary>
        [Category("Infolight")]
        public string Class { get; set; }

        /// <summary>
        /// 新增
        /// </summary>
        public static JQTreeViewContextItem InsertContextItem = new JQTreeViewContextItem() { Text = "Insert", Icon = JQIcon.Add, OnClick = "insertTreeNode",Class="infosysbutton-i" };
        /// <summary>
        /// 编辑
        /// </summary>
        public static JQTreeViewContextItem UpdateContextItem = new JQTreeViewContextItem() { Text = "Update", Icon = JQIcon.Edit, OnClick = "updateTreeNode",Class="infosysbutton-u" };
        /// <summary>
        /// 删除
        /// </summary>
        public static JQTreeViewContextItem DeleteContextItem = new JQTreeViewContextItem() { Text = "Delete", Icon = JQIcon.Remove, OnClick = "removeTreeNode",Class="infosysbutton-d" };

        public void Render(HtmlTextWriter writer)
        {
            var contorlID = ((this as IJQProperty).ParentProperty.Component as Control).ID;
            //writer.AddAttribute(HtmlTextWriterAttribute.Id, string.Format("contextItem{0}{1}", contorlID, Text));
            //writer.AddAttribute(HtmlTextWriterAttribute.Href, "javascript:void(0)");
            if (this.Class != null && this.Class != "")
                writer.AddAttribute(HtmlTextWriterAttribute.Class, this.Class);
            writer.AddAttribute(JQProperty.DataOptions, this.DataOptions);
            writer.AddAttribute(HtmlTextWriterAttribute.Onclick, string.Format("{0}('#{1}')", this.OnClick, contorlID));
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            writer.Write(this.Text);
            writer.RenderEndTag();
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string DataOptions
        {
            get
            {
                var options = new List<string>();
                options.Add(string.Format("iconCls:'{0}'", Icon));
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

    public class JQTreeViewColumn : JQCollectionItem, IJQDataSourceProvider
    {
        public JQTreeViewColumn()
        {
            NewLine = true;
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
        /// 是否另起一行
        /// </summary>
        [Category("Infolight")]
        public bool NewLine { get; set; }

        internal void CheckProperties()
        {
            if (string.IsNullOrEmpty(this.FieldName))
            {
                var controlID = ((this as IJQProperty).ParentProperty.Component as Control).ID;
                throw new JQProperyNullException(controlID, typeof(JQTreeView), "TreeViewColumns.FieldName");
            }
        }

        public void Render(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Nowrap, "nowrap");
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            writer.Write(this.Caption);
            writer.RenderEndTag();
            writer.AddAttribute(HtmlTextWriterAttribute.Nowrap, "nowrap");
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            writer.AddAttribute(HtmlTextWriterAttribute.Type, JQEditorControl.TextBox);
            writer.AddAttribute(HtmlTextWriterAttribute.Name, this.FieldName);
            writer.AddAttribute(JQProperty.InfolightOptions, this.InfolightOption);
            writer.AddAttribute(HtmlTextWriterAttribute.Id, FieldName + "_TreeViewColumn");
            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag();
            writer.RenderEndTag();
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string InfolightOption
        {
            get
            {
                var options = new List<string>();
                var treeview = (this as IJQProperty).ParentProperty.Component as JQTreeView;
                options.Add(string.Format("form:'{0}'", treeview.treeViewDialogID));
                options.Add(string.Format("field:'{0}'", FieldName));
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

}
