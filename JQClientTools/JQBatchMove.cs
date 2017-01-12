using EFClientTools.EFServerReference;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;
using System.Windows.Forms.Design;
using System.Xml;

namespace JQClientTools
{
    public class JQBatchMove : WebControl
    {
        public JQBatchMove()
        {
            _MatchColumns = new JQBatchMoveColumnsCollection<JQBatchMoveColumns>(this);
            SrcSelectAll = false;
        }
        [Category("Infolight")]
        [Editor(typeof(DataControlEditor), typeof(UITypeEditor))]
        public string DesDataGrid { get; set; }

        [Category("Infolight")]
        [Editor(typeof(DataControlEditor), typeof(UITypeEditor))]
        public string SrcDataGrid { get; set; }

        [Category("Infolight")]
        public bool AlwaysInsert { get; set; }        
        
        [Category("Infolight")]
        public bool SrcSelectAll { get; set; }

        [Category("Infolight")]
        public string OnEachMove { get; set; }

        private JQBatchMoveColumnsCollection<JQBatchMoveColumns> _MatchColumns;
        [Category("Infolight")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [NotifyParentProperty(true)]
        public JQBatchMoveColumnsCollection<JQBatchMoveColumns> MatchColumns
        {
            get
            {
                return _MatchColumns;
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (!this.DesignMode)
            {
                EFClientTools.ClientUtility.ClientInfo.cErrorCode = 8030;
                writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ID);
                writer.AddAttribute(HtmlTextWriterAttribute.Class, JQControl.BatchMove);
                writer.AddAttribute(JQProperty.InfolightOptions, this.InfolightOptions);
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                RenderChildren(writer);
                writer.RenderEndTag();
                EFClientTools.ClientUtility.ClientInfo.cErrorCode = 0;
            }
        }
        public override void RenderBeginTag(HtmlTextWriter writer)
        {
            if (!this.DesignMode)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ID);
                writer.AddAttribute(HtmlTextWriterAttribute.Class, JQControl.Dialog);
                writer.AddAttribute(HtmlTextWriterAttribute.Style, string.Format("width:{0}px;padding:10px", Width.Value));
                writer.AddAttribute(JQProperty.DataOptions, DataOptions);
                writer.AddAttribute(JQProperty.InfolightOptions, InfolightOptions);
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                writer.RenderEndTag();
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        internal string InfolightOptions
        {
            get
            {
                var options = new List<string>();
                options.Add(string.Format("desDataGrid:'{0}'", DesDataGrid));
                options.Add(string.Format("srcDataGrid:'{0}'", SrcDataGrid));
                options.Add(string.Format("alwaysInsert:{0}", AlwaysInsert.ToString().ToLower()));
                options.Add(string.Format("srcSelectAll:{0}", SrcSelectAll.ToString().ToLower()));
                var columns = new List<string>();
                foreach (var column in MatchColumns)
                {
                    columns.Add(string.Format("{{desColumn:'{0}',srcColumn:'{1}'}}"
                        , column.DesColumn, column.SrcColumn));
                }
                options.Add(string.Format("matchColumns:[{0}]", string.Join(",", columns)));
                options.Add(string.Format("onEachMove:{0}", OnEachMove));
                
                return string.Join(",", options);
            }
        }
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        private string DataOptions
        {
            get
            {
                //var optionBuilder = new StringBuilder();
                //if (!string.IsNullOrEmpty(Icon))
                //{
                //    optionBuilder.AppendFormat("iconCls:'{0}'", Icon);
                //    optionBuilder.Append(",");
                //}
                //optionBuilder.AppendFormat("closed:'{0}'", Closed);
                //return optionBuilder.ToString();
                var options = new List<string>();
                options.Add(string.Format("closed:'{0}'", "true"));
                if (this.Width.Type == UnitType.Pixel && Width.Value > double.Epsilon)
                {
                    options.Add(string.Format("width:{0}", Width.Value));
                }
                if (this.Height.Type == UnitType.Pixel && Height.Value > double.Epsilon)
                {
                    options.Add(string.Format("height:{0}", Height.Value));
                }
                return string.Join(",", options);
            }
        }
    }

    public class JQBatchMoveColumns : JQBatchMoveColumnsCollectionItem,IJQBatchMoveProvider
    {
        private string _DesColumn = "";
        /// <summary>
        /// 
        /// </summary>
        [Category("Infolight")]
        [NotifyParentProperty(true)]
        [Editor(typeof(JQBatchMoveDesColumnFieldEditor), typeof(UITypeEditor))]
        [DefaultValue("")]
        public string DesColumn
        {
            get
            {
                return _DesColumn;
            }
            set
            {
                _DesColumn = value;
            }
        }
        private string _SrcColumn = "";
        /// <summary>
        /// 
        /// </summary>
        [Category("Infolight")]
        [NotifyParentProperty(true)]
        [Editor(typeof(JQBatchMoveSrcColumnFieldEditor), typeof(UITypeEditor))]
        [DefaultValue("")]
        public string SrcColumn
        {
            get
            {
                return _SrcColumn;
            }
            set
            {
                _SrcColumn = value;
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
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        private string InfolightOptions
        {
            get
            {
                var options = new List<string>();
                options.Add(string.Format("desColumn:'{0}'", DesColumn));
                options.Add(string.Format("srcColumn:'{0}'", SrcColumn));
                return string.Join(",", options);
            }
        }
        string IJQBatchMoveProvider.sRemoteName
        {
            get
            {
                var src = (this as IJQProperty).ParentProperty.Component as JQBatchMove;
                if (src.SrcDataGrid != "")
                {
                    WebControl wc = src.Page.FindControl(src.SrcDataGrid) as WebControl;
                    if (wc.GetType() == typeof(JQDataGrid))
                    {
                        return ((JQDataGrid)wc).RemoteName;
                    }
                }
                return "";
            }
            set { }
        }

        string IJQBatchMoveProvider.sDataMember
        {
            get
            {
                var src = (this as IJQProperty).ParentProperty.Component as JQBatchMove;
                if (src.SrcDataGrid != "")
                {
                    WebControl wc = src.Page.FindControl(src.SrcDataGrid) as WebControl;
                    if (wc.GetType() == typeof(JQDataGrid))
                    {
                        return ((JQDataGrid)wc).DataMember;
                    }
                }
                return "";
            }
            set { }
        }
        string IJQBatchMoveProvider.dRemoteName
        {
            get
            {
                var src = (this as IJQProperty).ParentProperty.Component as JQBatchMove;
                if (src.DesDataGrid != "")
                {
                    WebControl wc = src.Page.FindControl(src.DesDataGrid) as WebControl;
                    if (wc.GetType() == typeof(JQDataGrid))
                    {
                        return ((JQDataGrid)wc).RemoteName;
                    }
                }
                return "";
            }
            set { }
        }

        string IJQBatchMoveProvider.dDataMember
        {
            get
            {
                var src = (this as IJQProperty).ParentProperty.Component as JQBatchMove;
                if (src.DesDataGrid != "")
                {
                    WebControl wc = src.Page.FindControl(src.DesDataGrid) as WebControl;
                    if (wc.GetType() == typeof(JQDataGrid))
                    {
                        return ((JQDataGrid)wc).DataMember;
                    }
                }
                return "";
            }
            set { }
        }
    }
    /// <summary>
    /// Item of collection
    /// </summary>
    public abstract class JQBatchMoveColumnsCollectionItem :IJQProperty
    {
        private IJQProperty _parentProperty;
        IJQProperty IJQProperty.ParentProperty
        {
            get
            {
                return _parentProperty;
            }
            set
            {
                _parentProperty = value;
            }
        }

        object IJQProperty.Component
        {
            get
            {
                return null;
            }
        }
    }

    public interface IJQBatchMoveProvider
    {
        string sRemoteName { get; set; }
        string sDataMember { get; set; }
        string dRemoteName { get; set; }
        string dDataMember { get; set; }
    }
    public class JQBatchMoveColumnsCollection<T> : List<T>, IList, IJQProperty
    {        
        /// <summary>
        /// Creates a new instance of collection
        /// </summary>
        /// <param name="parentProperty">Parent property</param>
        public JQBatchMoveColumnsCollection(IJQProperty parentProperty)
        {
            _parentProperty = parentProperty;
        }

        /// <summary>
        /// Creatse a new instance of collection
        /// </summary>
        /// <param name="component">Parent component</param>
        public JQBatchMoveColumnsCollection(WebControl component)
        {
            _component = component;
        }

        /// <summary>
        /// Adds item
        /// </summary>
        /// <param name="item">Item</param>
        public new void Add(T item)
        {
            base.Add(item);
            if (item is IJQProperty)
            {
                (item as IJQProperty).ParentProperty = this;
            }
        }

        /// <summary>
        /// Adds list of items
        /// </summary>
        /// <param name="collection">List of items</param>
        public new void AddRange(IEnumerable<T> collection)
        {
            base.AddRange(collection);
            foreach (var item in collection)
            {
                if (item is IJQProperty)
                {
                    (item as IJQProperty).ParentProperty = this;
                }
            }
        }

        /// <summary>
        /// Inserts item at the specified index
        /// </summary>
        /// <param name="index">Index</param>
        /// <param name="item">Item</param>
        public new void Insert(int index, T item)
        {
            base.Insert(index, item);
            if (item is IJQProperty)
            {
                (item as IJQProperty).ParentProperty = this;
            }
        }

        /// <summary>
        /// Inserts list of items at the specified index
        /// </summary>
        /// <param name="index">Index</param>
        /// <param name="collection">Item</param>
        public new void InsertRange(int index, IEnumerable<T> collection)
        {
            base.InsertRange(index, collection);
            foreach (var item in collection)
            {
                if (item is IJQProperty)
                {
                    (item as IJQProperty).ParentProperty = this;
                }
            }
        }

        #region IList Members

        int IList.Add(object value) //编辑器里增加时只会触发这个
        {
            if (value is T)
            {
                base.Add((T)value);
                if (value is IJQProperty)
                {
                    (value as IJQProperty).ParentProperty = this;
                }
                return base.Count - 1;
            }
            else
            {
                throw new ArgumentException(string.Format("value is not of type:{0}.", typeof(T).Name));
            }
        }

        #endregion
        private IJQProperty _parentProperty;
        IJQProperty IJQProperty.ParentProperty
        {
            get
            {
                return _parentProperty;
            }
            set
            {
                _parentProperty = value;
            }
        }

        private object _component;
        object IJQProperty.Component
        {
            get
            {
                return _component;
            }
        }
    }

    public class JQBatchMoveSrcColumnFieldEditor : PropertyDropDownEditor
    {
        protected override List<string> GetListOfValues(ITypeDescriptorContext context)
        {
            var list = new List<string>();
            IJQBatchMoveProvider item = (IJQBatchMoveProvider)context.Instance;
            if (!string.IsNullOrEmpty(item.sRemoteName) && !string.IsNullOrEmpty(item.sDataMember) )
            {
                var clientInfo = EFClientTools.DesignClientUtility.ClientInfo;
                clientInfo.UseDataSet = true;
                var assemblyName = item.sRemoteName.Split('.')[0];
                var commandName = item.sDataMember;

                var fields = EFClientTools.DesignClientUtility.Client.GetEntityFields(clientInfo, assemblyName, commandName, null);
                foreach (var field in fields)
                {
                    list.Add(field);
                }
            }

            return list;
        }
    }
    public class JQBatchMoveDesColumnFieldEditor : PropertyDropDownEditor
    {
        protected override List<string> GetListOfValues(ITypeDescriptorContext context)
        {
            var list = new List<string>();
            IJQBatchMoveProvider item = (IJQBatchMoveProvider)context.Instance;
            if (!string.IsNullOrEmpty(item.dRemoteName) && !string.IsNullOrEmpty(item.dDataMember))
            {
                var clientInfo = EFClientTools.DesignClientUtility.ClientInfo;
                clientInfo.UseDataSet = true;
                var assemblyName = item.dRemoteName.Split('.')[0];
                var commandName = item.dDataMember;

                var fields = EFClientTools.DesignClientUtility.Client.GetEntityFields(clientInfo, assemblyName, commandName, null);
                foreach (var field in fields)
                {
                    list.Add(field);
                }
            }

            return list;
        }
    }
}