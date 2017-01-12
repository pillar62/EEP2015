using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Web.UI;
using System.Drawing.Design;
using System.Reflection;

namespace JQClientTools
{
    public class JQButton : WebControl
    {

        public JQButton()
        {
            Text = "Button";
        }

        /// <summary>
        /// 标题
        /// </summary>
        [Category("Infolight")]
        public string Text { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        [Category("Infolight")]
        [Editor(typeof(IconEditor), typeof(UITypeEditor))]
        public string Icon { get; set; }

        [Category("Infolight")]
        public string OnClick { get; set; }
        
        [Category("Infolight")]
        public bool Plain { get; set; }

        [Category("Infolight")]
        public bool Postback { get; set; }

        protected override void OnLoad(EventArgs e)
        {
            if (Postback && this.Page.IsPostBack && !string.IsNullOrEmpty(OnClick))
            {
                var postControl = GetPostBackControl(this.Page);
                if (postControl != null && postControl.ID == this.ID)
                {
                    var method = this.Page.GetType().GetMethod(OnClick, BindingFlags.NonPublic | BindingFlags.Instance);
                    if (method != null)
                    {
                        method.Invoke(this.Page, new object[] { this, new EventArgs() });
                    }
                }
            }
            base.OnLoad(e);
        }

        public static Control GetPostBackControl(Page page)
        {
            Control control = null;
            string ctrlname = page.Request.Params.Get("__EVENTTARGET");
            if (ctrlname != null && ctrlname != string.Empty)
            {
                control = page.FindControl(ctrlname);
            }
            else
            {
                foreach (string ctl in page.Request.Form)
                {
                    Control c = page.FindControl(ctl);
                    if (c is JQButton)
                    {
                        control = c;
                        break;
                    }
                }
            }
            return control;
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (!DesignMode)
            {
                EFClientTools.ClientUtility.ClientInfo.cErrorCode = 8040;

                if (Postback)
                {
                    Button button = new Button();
                    button.ID = this.ID;
                    button.Text = this.Text;
                    //if (!this.Page.IsPostBack)
                    //{
                    //    if (!string.IsNullOrEmpty(OnClick))
                    //    {
                    //        //var method = this.Page.GetType().GetMethod("Button1_Click", BindingFlags.NonPublic | BindingFlags.Instance);
                    //       // var clickEvent = button.GetType().GetEvent("Click");
                    //       // clickEvent.AddEventHandler(button, Delegate.CreateDelegate(clickEvent.EventHandlerType, this.Page, OnClick));
                    //        //button.Click += 
                    //        //button.Click +=new EventHandler(button_Click);
                    //    }
                    //}
                    button.RenderControl(writer);
                }
                else
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ID);
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, JQControl.LinkButton);
                    if (!string.IsNullOrEmpty(OnClick))
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Onclick, string.Format("{0}();", OnClick));
                    }
                    writer.AddAttribute(JQProperty.DataOptions, DataOptions);
                    writer.RenderBeginTag(HtmlTextWriterTag.A);

                    writer.Write(Text);
                    writer.RenderEndTag();
                }

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
                options.Add(string.Format("plain:{0}", Plain.ToString().ToLower()));
                return string.Join(",", options);
            }
        }
    }
}
