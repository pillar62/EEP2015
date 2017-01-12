using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;
using System.Windows.Forms.Design;

namespace JQClientTools
{
    public class JQRotator : WebControl, IJQDataSourceProvider
    {
        public JQRotator()
        { 
        
        }
        /// <summary>
        /// 数据源
        /// </summary>
        [Category("Infolight")]
        [Editor(typeof(RemoteNameEditor), typeof(UITypeEditor))]
        public string RemoteName { get; set; }

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

        /// <summary>
        /// 栏位名
        /// </summary>
        [Category("Infolight")]
        [Editor(typeof(FieldEditor), typeof(UITypeEditor))]
        public string FieldName { get; set; }

        /// <summary>
        /// MenuID
        /// </summary>
        [Category("Infolight")]
        public string MenuID { get; set; }

        /// <summary>
        /// Type
        /// </summary>
        [Category("Infolight")]
        public RotatorEnum RotatorType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Category("Infolight")]
        public string OnBeforeLoad { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Category("Infolight")]
        [Description("if type is image,set this attribute for folder")]
        public string ImageFolder { get; set; }


        public override void RenderBeginTag(HtmlTextWriter writer)
        {
            if (!this.DesignMode)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ID);
                writer.AddAttribute(HtmlTextWriterAttribute.Class, JQControl.Rotator);
                writer.AddAttribute(JQProperty.InfolightOptions, InfolightOptions);
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                writer.RenderEndTag();
            }
        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            if (!this.DesignMode)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ID);
                writer.AddAttribute(HtmlTextWriterAttribute.Class, JQControl.Rotator);
                writer.AddAttribute(JQProperty.InfolightOptions, this.InfolightOptions);
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                RenderChildren(writer);
                writer.RenderEndTag();
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
                options.Add(string.Format("fieldName:'{0}'", FieldName));
                options.Add(string.Format("rotatorType:'{0}'", RotatorType.ToString().ToLower()));
                if (RotatorType == RotatorEnum.image)
                {
                    options.Add(string.Format("imageFolder:'{0}'", ImageFolder.ToString()));
                }
                if (this.MenuID != null && this.MenuID != "")
                {
                    options.Add(string.Format("menuID:'{0}'", MenuID));
                }
                if (this.OnBeforeLoad != null && this.OnBeforeLoad != "")
                {
                    options.Add(string.Format("onBeforeLoad:{0}", OnBeforeLoad));
                }

                return string.Join(",", options);
            }
        }

        public enum RotatorEnum { text,image}
    }
}
