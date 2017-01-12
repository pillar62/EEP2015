using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.IO;

namespace JQMobileTools
{
    public class JQTab : MultiView
    {

        public JQTab()
            : base()
        {

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

        private bool isShowCloseButton = true;
        [Category("Infolight"), DefaultValue(true)]
        public bool IsShowCloseButton
        {
            get
            {
                return isShowCloseButton;
            }
            set
            {
                isShowCloseButton = value;
            }
        }

        public string Title { get; set; }

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            if (!this.DesignMode)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ID);
                writer.AddAttribute(JQProperty.DataOverlayTheme, Theme);
                if (!IsShowCloseButton)
                    writer.AddAttribute("data-close-btn", "none");
                writer.AddAttribute(JQProperty.DataRole, JQDataRole.Page);
                writer.RenderBeginTag(HtmlTextWriterTag.Div);//page

                var tabTheme = Theme;

                writer.AddAttribute(JQProperty.DataRole, JQDataRole.Header);
                if (!string.IsNullOrEmpty(tabTheme))
                {
                    writer.AddAttribute(JQProperty.DataTheme, tabTheme);
                }
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                if (!string.IsNullOrEmpty(Title))
                {
                    writer.AddStyleAttribute(HtmlTextWriterStyle.Padding, "0px");
                    writer.RenderBeginTag(HtmlTextWriterTag.H1);
                    writer.Write(Title);
                    writer.RenderEndTag();
                }
                if (IsShowCloseButton)
                {
                    //render back button
                    writer.AddAttribute(JQProperty.DataMini, bool.TrueString.ToLower().ToLower());
                    writer.AddAttribute(JQProperty.DataInline, bool.TrueString.ToLower().ToLower());
                    writer.AddAttribute(JQProperty.DataIcon, JQDataIcon.CaratL);
                    writer.AddAttribute(JQProperty.DataRole, JQDataRole.Button);
                    writer.AddAttribute(JQProperty.DataIconPos, JQDataIconPos.NoText);
                    writer.AddAttribute(JQProperty.DataTheme, JQDataTheme.B);
                    writer.AddAttribute(HtmlTextWriterAttribute.Onclick, string.Format("javascript:$('#{0}').find('.info-form').form('close');", this.ID));
                    //writer.AddAttribute(JQProperty.DataRel, "back");
                    writer.RenderBeginTag(HtmlTextWriterTag.A);
                    writer.Write("Back");
                    writer.RenderEndTag();
                }

                writer.RenderEndTag();

                if (!string.IsNullOrEmpty(tabTheme))
                {
                    writer.AddAttribute(JQProperty.DataTheme, tabTheme);
                }
                writer.AddAttribute(JQProperty.DataRole, JQDataRole.Content);
                writer.RenderBeginTag(HtmlTextWriterTag.Div);//content

                writer.AddAttribute(JQProperty.DataRole, JQDataRole.Tabs);
                writer.RenderBeginTag(HtmlTextWriterTag.Div);//tabs
                writer.AddAttribute(JQProperty.DataRole, JQDataRole.NavBar);
                writer.RenderBeginTag(HtmlTextWriterTag.Div);//navbar
                writer.RenderBeginTag(HtmlTextWriterTag.Ul);
                foreach (View view in this.Views)
                {
                    if (view is JQTabItem)
                    {
                        var tabItem = view as JQTabItem;
                        writer.RenderBeginTag(HtmlTextWriterTag.Li);

                        writer.AddAttribute(HtmlTextWriterAttribute.Href, string.Format("#{0}", tabItem.ID));
                        writer.RenderBeginTag(HtmlTextWriterTag.A);
                        writer.Write(tabItem.Title);
                        writer.RenderEndTag();
                        writer.RenderEndTag();
                    }
                }
                writer.RenderEndTag();
                writer.RenderEndTag();//navbar

                foreach (View view in this.Views)
                {
                    if (view is JQTabItem)
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Id, view.ID);
                        writer.RenderBeginTag(HtmlTextWriterTag.Div);
                        foreach (Control control in view.Controls)
                        {
                            control.RenderControl(writer);
                        }
                        writer.RenderEndTag();
                    }
                }

                writer.RenderEndTag();//tabs
                writer.RenderEndTag();//content

                writer.RenderEndTag();//page
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
    }

    public class JQTabItem : View
    {
        [Category("Infolight")]
        public string Title { get; set; }


        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            if (!this.DesignMode)
            {
            }
        }
    }
}
