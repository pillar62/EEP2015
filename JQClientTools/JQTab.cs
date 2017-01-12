using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.ComponentModel;
using System.ComponentModel.Design;

namespace JQClientTools
{
    public class JQTab : MultiView
    {
        [Category("Layout")]
        public Unit Width { get; set; }
        [Category("Layout")]
        public Unit Height { get; set; }
        [Category("Layout")]
        public string OnSelect { get; set; }

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            if (!this.DesignMode)
            {
                EFClientTools.ClientUtility.ClientInfo.cErrorCode = 8370;
                writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ID);
                writer.AddAttribute(HtmlTextWriterAttribute.Class, JQControl.Tab);
                writer.AddAttribute(JQProperty.DataOptions, DataOptions);
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
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                foreach (View view in this.Views)
                {
                    if (view is JQTabItem)
                    {
                        

                        var title = string.IsNullOrEmpty((view as JQTabItem).Title) ? ID : (view as JQTabItem).Title;

                        if (!(view as JQTabItem).PreLoad)
                        {
                            writer.AddAttribute(HtmlTextWriterAttribute.Class, "no-preload");
                        }
                        writer.AddAttribute(HtmlTextWriterAttribute.Title, title);
                        writer.AddAttribute(JQProperty.DataOptions, (view as JQTabItem).DataOptions);
                       // writer.AddAttribute(HtmlTextWriterAttribute.Style, "margin:5px; padding: 5px");
                        writer.RenderBeginTag(HtmlTextWriterTag.Div);
                        foreach (Control control in view.Controls)
                        {
                            control.RenderControl(writer);
                        }
                        writer.RenderEndTag();
                    }
                    else
                    {
                        writer.RenderBeginTag(HtmlTextWriterTag.Div);
                        foreach (Control control in view.Controls)
                        {
                            control.RenderControl(writer);
                        }
                        writer.RenderEndTag();
                    }

                }
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
                if (!string.IsNullOrEmpty(OnSelect))
                {
                    options.Add(string.Format("onSelect:{0}", OnSelect));
                }
                return string.Join(",", options);
            }
        }
    }

    public class JQTabItem : View
    {
        public JQTabItem()
        {
            PreLoad = true;
        }

        [Category("Infolight")]
        public string Title { get; set; }
        [Category("Infolight")]
        public string Icon { get; set; }
        [Category("Infolight")]
        public bool PreLoad { get; set; }


        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            if (!this.DesignMode)
            {
                var title = string.IsNullOrEmpty(Title) ? ID : Title;
                if (!PreLoad)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "no-preload");
                }
                writer.AddAttribute(HtmlTextWriterAttribute.Title, title);
                writer.AddAttribute(JQProperty.DataOptions, DataOptions);
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                foreach (Control control in this.Controls)
                {
                    control.RenderControl(writer);
                }
                writer.RenderEndTag();
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        internal string DataOptions
        {
            get
            {
                var options = new List<string>();
                if (!string.IsNullOrEmpty(Icon))
                {
                    options.Add(string.Format("iconCls:'{0}'", Icon));
                }
                return string.Join(",", options);
            }
        }
    }
}
