using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Drawing.Design;
using System.Web.UI;
using System.Web.UI.Design;
using System.ComponentModel.Design;

namespace JQMobileTools
{
    public class JQMsgPush: WebControl
    {
        [Category("Infolight")]
        public string To { get; set; }
        [Category("Infolight")]
        public string Subject { get; set; }
        [Category("Infolight")]
        public string Body { get; set; }


        protected override void Render(HtmlTextWriter writer)
        {
            if (!this.DesignMode)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ID);
                writer.AddAttribute(HtmlTextWriterAttribute.Class, JQClass.MsgPush);
                writer.AddAttribute(JQProperty.DataOptions, this.InfolightOptions);
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
                options.Add(string.Format("to:'{0}'", To));
                options.Add(string.Format("subject:'{0}'", Subject));
                options.Add(string.Format("body:'{0}'", Body));
                return string.Join(",", options);
            }
        }
    }
}
