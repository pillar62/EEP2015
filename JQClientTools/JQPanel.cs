using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Web.UI;
using System.Drawing.Design;

namespace JQClientTools
{
    public class JQPanel : Panel
    {
        public JQPanel()
        {
            Width = new Unit(600, UnitType.Pixel);
            Title = "JQPanel";
        }


        /// <summary>
        /// 标题
        /// </summary>
        [Category("Infolight")]
        public string Title { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        [Category("Infolight")]
        [Editor(typeof(IconEditor), typeof(UITypeEditor))]
        public string Icon { get; set; }
        [Category("Infolight")]
        public bool Collapsible { get; set; }
        [Category("Infolight")]
        public bool Minimizable { get; set; }
        [Category("Infolight")]
        public bool Maximizable { get; set; }

        public override void RenderBeginTag(HtmlTextWriter writer)
        {
            if (!this.DesignMode)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ID);
                writer.AddAttribute(HtmlTextWriterAttribute.Class, JQControl.Panel);
                writer.AddAttribute(HtmlTextWriterAttribute.Title, Title);

                if (this.Width.Type == UnitType.Pixel && Width.Value > double.Epsilon)
                {
                    writer.AddStyleAttribute(HtmlTextWriterStyle.Width, string.Format("{0}px", Width.Value));
                }
                if (this.Height.Type == UnitType.Pixel && Height.Value > double.Epsilon)
                {
                    writer.AddStyleAttribute(HtmlTextWriterStyle.Height, string.Format("{0}px", Height.Value));
                }
                writer.AddStyleAttribute(HtmlTextWriterStyle.Padding, "10px");

                writer.AddAttribute(JQProperty.DataOptions, DataOptions);
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
            }
        }

        public override void RenderEndTag(HtmlTextWriter writer)
        {
            if (!this.DesignMode)
            {
                writer.RenderEndTag();
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (!DesignMode)
            {
                EFClientTools.ClientUtility.ClientInfo.cErrorCode = 8290;
                writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ID);
                writer.AddAttribute(HtmlTextWriterAttribute.Class, JQControl.Panel);
                writer.AddAttribute(HtmlTextWriterAttribute.Title, Title);

                if (this.Width.Type == UnitType.Pixel && Width.Value > double.Epsilon)
                {
                    writer.AddStyleAttribute(HtmlTextWriterStyle.Width, string.Format("{0}px", Width.Value));
                }
                if (this.Height.Type == UnitType.Pixel && Height.Value > double.Epsilon)
                {
                    writer.AddStyleAttribute(HtmlTextWriterStyle.Height, string.Format("{0}px", Height.Value));
                }
                writer.AddStyleAttribute(HtmlTextWriterStyle.Padding, "10px");
               
                writer.AddAttribute(JQProperty.DataOptions, DataOptions);
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                RenderChildren(writer);
                writer.RenderEndTag();
                EFClientTools.ClientUtility.ClientInfo.cErrorCode = 0;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        private string DataOptions
        {
            get
            {
               
                var options = new List<string>();
                if (!string.IsNullOrEmpty(Icon))
                {
                    options.Add(string.Format("iconCls:'{0}'", Icon));
                }
                options.Add(string.Format("collapsible:{0}", Collapsible.ToString().ToLower()));
                options.Add(string.Format("maximizable:{0}", Maximizable.ToString().ToLower()));
                options.Add(string.Format("minimizable:{0}", Minimizable.ToString().ToLower()));
                
                return string.Join(",", options);
            }
        }
    }


}
