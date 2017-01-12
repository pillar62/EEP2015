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
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Windows.Forms.Design;
using System.Xml;

namespace JQClientTools
{
    [Designer(typeof(JQSecurityEdit), typeof(IDesigner))]
    public class JQHtml : WebControl, IJQDataSourceProvider
    {
        public enum ViewMode
        {
            Edit, Show
        }

        public JQHtml()
        {
            keyColumns = new JQCollection<JQHtmlKeyColumn>(this);

            //_webExportControls = new WebExportControls(this, typeof(WebExportControl));
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
                dataMember = value;
            }
        }

        private JQCollection<JQHtmlKeyColumn> keyColumns;
        [Category("Infolight")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [NotifyParentProperty(true)]
        public JQCollection<JQHtmlKeyColumn> KeyColumns
        {
            get
            {
                return keyColumns;
            }
        }

        private string columnName = "Content";
        /// <summary>
        /// 栏位名
        /// </summary>
        [Category("Infolight"), DefaultValue("Content")]
        //[Editor(typeof(FieldEditor), typeof(UITypeEditor))]
        public string ColumnName
        {
            get
            {
                if (String.IsNullOrEmpty(columnName))
                    return "Content";
                return columnName;
            }
            set
            {
                columnName = value;
            }
        }

        private ViewMode mode;
        /// <summary>
        /// 栏位名
        /// </summary>
        [Category("Infolight")]
        //[Editor(typeof(FieldEditor), typeof(UITypeEditor))]
        public ViewMode Mode
        {
            get
            {
                return mode;
            }
            set
            {
                mode = value;
            }
        }

        public override Unit Height
        {
            get
            {
                if (base.Height == Unit.Empty)
                    return 500;
                return base.Height;
            }
            set
            {
                base.Height = value;
            }
        }

        public override Unit Width
        {
            get
            {
                if (base.Width == Unit.Empty)
                    return 1024;
                return base.Width;
            }
            set
            {
                base.Width = value;
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
                options.Add(string.Format("columnName:'{0}'", ColumnName));
                options.Add(string.Format("mode:'{0}'", mode.ToString()));
                return string.Join(",", options);
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
                options.Add(string.Format("columnName:'{0}'", ColumnName));
                options.Add(string.Format("mode:'{0}'", mode.ToString()));
                return string.Join(",", options);
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (!this.DesignMode)
            {
                //<script id="editor" type="text/plain" style="width:1024px;height:500px;"></script>
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ClientID);
                writer.AddAttribute(HtmlTextWriterAttribute.Style, "width:" + this.Width + ";height:" + this.Height + ";");
                writer.AddAttribute(JQProperty.DataOptions, this.DataOptions);
                writer.AddAttribute(JQProperty.InfolightOptions, this.InfolightOptions);
                var keyValues = "[";
                foreach (var keyColumn in KeyColumns)
                {
                    var keyValue = String.Format("{{\"key\":\"{0}\", \"value\":\"{1}\"}}", keyColumn.FieldName, keyColumn.KeyValue);
                    keyValues += keyValue + ",";
                }
                if (keyValues.LastIndexOf(',') != -1) keyValues = keyValues.Remove(keyValues.LastIndexOf(','));
                keyValues += "]";
                writer.AddAttribute("keyValues", keyValues);
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                writer.RenderEndTag();

                writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ClientID + "_view");
                writer.AddAttribute(HtmlTextWriterAttribute.Style, "width:" + this.Width + ";height:" + this.Height + ";");
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                writer.RenderEndTag();

                writer.RenderEndTag();


                //writer.RenderBeginTag(HtmlTextWriterTag.Div);
                //writer.AddAttribute(HtmlTextWriterAttribute.Onclick, "saveHtmlToDB('" + this.ClientID + "', eval($('#" + this.ClientID + "').attr('keyValues')));");
                //writer.AddAttribute(HtmlTextWriterAttribute.Type, "button");
                //writer.AddAttribute(HtmlTextWriterAttribute.Value, "Save");
                //writer.RenderBeginTag(HtmlTextWriterTag.Input);
                //writer.EndRender();
                //writer.EndRender();
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!Page.IsPostBack)
            {
                RenderJQHtml();
            }
        }

        public void RenderJQHtml()
        {
            if (!String.IsNullOrEmpty(this.ClientID))
            {
                //render html editor
                //baidu.editor.commands['hougelou'] = { execCommand: function() { this.execCommand('insertHtml', "<img src='http://www.hougelou.com/images/logo.png' />"); return true; }, queryCommandState: function() { } };
                StringBuilder renderScript = new StringBuilder();
                string strRenderScript = @"$(function(){
                    var ue = UE.getEditor('" + this.ClientID + @"');
                    ue.addListener( 'ready', function() {
                        loadHtmlFromDB('" + this.ClientID + @"', eval($('#" + this.ClientID + @"').attr('keyValues')));
                        baidu.editor.commands['save'] = { execCommand: function() { saveHtmlToDB('" + this.ClientID + "', eval($('#" + this.ClientID + @"').attr('keyValues'))); return true; }, queryCommandState: function() { } };
                    });
                });";
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), strRenderScript, true);
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (!this.DesignMode)
            {
                var qrcodeScripts = new string[] {
                    //qrcode
                    "../js/htmleditor/ueditor.config.js",
                    "../js/htmleditor/ueditor.all.js",
                    "../js/htmleditor/lang/zh-cn/zh-cn.js"
                    };
                AddClientScripts(qrcodeScripts);
                var styleSheets = new string[] {
                    "../js/htmleditor/themes/default/css/ueditor.css"
                };
                foreach (var sheet in styleSheets)
                {
                    if (!IsCssRegistered(sheet))
                    {
                        AddCss(sheet);
                    }
                }
            }
        }

        public void AddClientScripts(string[] urls)
        {
            var literal = new LiteralControl();
            var scripts = new StringBuilder();
            foreach (var url in urls)
            {
                scripts.Append(string.Format("<script type=\"text/javascript\" src=\"{0}\"></script>", url));
            }
            literal.Text = scripts.ToString();
            this.Page.Header.Controls.AddAt(0, literal);
        }

        public void AddClientScript(string url)
        {
            var literal = new LiteralControl();
            literal.Text = string.Format("<script type=\"text/javascript\" src=\"{0}\"></script>", url);
            this.Page.Header.Controls.Add(literal);
        }

        public bool IsClientScriptRegistered(string url)
        {
            return this.Page.Header.Controls.OfType<LiteralControl>().FirstOrDefault(c => c.Text.Contains(url)) != null;
        }

        public void AddCss(string url)
        {
            var link = new HtmlLink() { Href = url };
            link.Attributes["rel"] = "stylesheet";
            this.Page.Header.Controls.Add(link);
        }

        public void AddCss(string id, string url)
        {
            var link = new HtmlLink() { ID = id, Href = url };
            link.Attributes["rel"] = "stylesheet";
            this.Page.Header.Controls.Add(link);
        }

        public bool IsCssRegistered(string url)
        {
            return this.Page.Header.Controls.OfType<HtmlLink>().FirstOrDefault(c => c.Href.Equals(url)) != null;
        }
    }

    public class JQHtmlKeyColumn : JQCollectionItem, IJQDataSourceProvider
    {
        public JQHtmlKeyColumn()
        {

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
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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
        public string KeyValue { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        private string DataOptions
        {
            get
            {
                var options = new List<string>();
                options.Add(string.Format("field:'{0}'", FieldName));
                options.Add(string.Format("keyValue:{0}", KeyValue));
                return string.Join(",", options);
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        private string InfolightOptions
        {
            get
            {
                var options = new List<string>();
                options.Add(string.Format("field:'{0}'", FieldName));
                options.Add(string.Format("keyValue:{0}", KeyValue));
                return string.Join(",", options);
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
}