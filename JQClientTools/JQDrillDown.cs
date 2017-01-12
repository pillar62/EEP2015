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
    public class JQDrillDown : WebControl, IJQDataSourceProvider
    {
        public JQDrillDown()
        {
            _DisplayFields = new JQCollection<JQDrillDownDisplayFields>(this);
            _KeyyFields = new JQCollection<JQDrillDownKeyFields>(this);
        }

        [Category("Infolight")]

        [EditorAttribute(typeof(AspxUrlEditor), typeof(UITypeEditor))]
        public string FormName { get; set; }

        [Category("Infolight")]
        [EditorAttribute(typeof(RDCLUrlEditor), typeof(UITypeEditor))]
        public string ReportName { get; set; }

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
        private JQCollection<JQDrillDownKeyFields> _KeyyFields;
        [Category("Infolight")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [NotifyParentProperty(true)]
        public JQCollection<JQDrillDownKeyFields> KeyFields
        {
            get
            {
                return _KeyyFields;
            }
        }

        [Category("Infolight")]
        public string FormCaption { get; set; }

        [Category("Infolight")]
        public string BeforeDrillDown { get; set; }

        [Category("Infolight")]
        public JQDrillDownDrillStyle DrillStyle { get; set; }

        [Category("Infolight")]
        public JQDrillDownOpenMode OpenMode { get; set; }
        
        private JQCollection<JQDrillDownDisplayFields> _DisplayFields;
        [Category("Infolight")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [NotifyParentProperty(true)]
        public JQCollection<JQDrillDownDisplayFields> DisplayFields             
        {
            get
            {
                return _DisplayFields;
            }
        }
        protected override void Render(HtmlTextWriter writer)
        {
            if (!this.DesignMode)
            {
                EFClientTools.ClientUtility.ClientInfo.cErrorCode = 8150;
                writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ID);
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
                options.Add(string.Format("formName:'{0}'", FormName));
                options.Add(string.Format("formCaption:'{0}'", FormCaption));
                options.Add(string.Format("remoteName:'{0}'", remoteName));
                options.Add(string.Format("tableName:'{0}'", dataMember));
                options.Add(string.Format("reportName:'{0}'", ReportName));
                if (BeforeDrillDown != null && BeforeDrillDown != "")
                    options.Add(string.Format("beforeDrillDown:{0}", BeforeDrillDown));
                options.Add(string.Format("drillStyle:'{0}'", DrillStyle.ToString().ToLower()));
                options.Add(string.Format("openMode:'{0}'",OpenMode.ToString().ToLower()));
                var keys = new List<string>();
                foreach (var key in KeyFields)
                {
                    keys.Add(string.Format("{{field:'{0}'}}", key.FieldName));
                }
                options.Add(string.Format("keyFields:[{0}]", string.Join(",", keys)));
                var displays = new List<string>();
                foreach (var display in DisplayFields)
                {
                    var drillFieldsString = "";
                    if (display.DrillFields.Count > 0)
                    {
                        var drillfields = new List<string>();
                        foreach (var fields in display.DrillFields)
                        {
                            drillfields.Add(string.Format("{{field:'{0}'}}", fields.FieldName));
                        }
                        drillFieldsString = string.Format("drillFields:[{0}]", string.Join(",",drillfields));
                    }
                    else
                    {
                        drillFieldsString = string.Format("drillFields:[{{field:'{0}'}}]", display.FieldName);
                    }
                    displays.Add(string.Format("{{field:'{0}',caption:'{1}',width:{2},drillObjectID:'{3}',{4}}}", display.FieldName, display.Caption, display.Width.ToString(), display.DrillObjectID, drillFieldsString));
                }
                options.Add(string.Format("displayFields:[{0}]", string.Join(",", displays)));

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

    public enum JQDrillDownDrillStyle
    {
        WebForm, MobileForm, RDLC, Command
    }
    public enum JQDrillDownOpenMode
    { 
        Dialog ,NewTab ,NewWindow
    }
    public class JQDrillDownDisplayFields : JQCollectionItem, IJQDataSourceProvider
    {
        public JQDrillDownDisplayFields()
        {
            _DrillFields = new JQCollection<JQDrillDownFields>((this) as IJQProperty);
            Width = 80;
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

        /// <summary>
        /// 宽度
        /// </summary>
        [Category("Infolight")]
        public int Width { get; set; }

        internal void CheckProperties()
        {
            var controlID = ((this as IJQProperty).ParentProperty.Component as Control).ID;
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
                var options = new List<string>();
                options.Add(string.Format("field:'{0}'", FieldName));
                options.Add(string.Format("width:{0}", Width));
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
                options.Add(string.Format("width:{0}", Width.ToString()));
                options.Add(string.Format("caption:'{0}'", Caption));
                options.Add(string.Format("drillFields:'{0}'", DrillFields));
                options.Add(string.Format("drillObjectID:'{0}'", DrillObjectID));
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
    public class JQDrillDownFields : JQCollectionItem, IJQDataSourceProvider, ICloneable
    {
        public JQDrillDownFields()
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
            }
        }

        public void Render(HtmlTextWriter writer)
        {
            writer.AddAttribute(JQProperty.DataOptions, this.DataOptions);
            writer.AddAttribute(JQProperty.InfolightOptions, this.InfolightOptions);
            writer.RenderBeginTag(HtmlTextWriterTag.Th);
            writer.Write(this.FieldName);
            writer.RenderEndTag();
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        private string DataOptions
        {
            get
            {
                var options = new List<string>();
                options.Add(string.Format("field:'{0}'", FieldName));
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
                return ((this as IJQProperty).ParentProperty.ParentProperty as IJQDataSourceProvider).RemoteName;
            }
            set { }
        }

        string IJQDataSourceProvider.DataMember
        {
            get
            {
                return ((this as IJQProperty).ParentProperty.ParentProperty as IJQDataSourceProvider).DataMember;
            }
            set { }
        }

        #endregion

        #region ICloneable Members

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion
    }
    public class JQDrillDownKeyFields : JQCollectionItem, IJQDataSourceProvider
    {
        public JQDrillDownKeyFields()
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
            }
        }

        public void Render(HtmlTextWriter writer)
        {
            writer.AddAttribute(JQProperty.DataOptions, this.DataOptions);
            writer.AddAttribute(JQProperty.InfolightOptions, this.InfolightOptions);
            writer.RenderBeginTag(HtmlTextWriterTag.Th);
            writer.Write(this.FieldName);
            writer.RenderEndTag();
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        private string DataOptions
        {
            get
            {
                var options = new List<string>();
                options.Add(string.Format("field:'{0}'", FieldName));
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

    public class AspxUrlEditor : UrlEditor
    {
        protected override string Filter
        {
            get
            {
                return "Aspx Files (*.aspx)|*.aspx|All Files (*.*)|*.*";
            }
        }
    }
}