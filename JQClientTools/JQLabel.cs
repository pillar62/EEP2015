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
    public class JQLabel : WebControl
    {

        public JQLabel()
        {
            Text = "Label";
        }

        /// <summary>
        /// 标题
        /// </summary>
        [Category("Infolight")]
        public string Text { get; set; }

        protected override void Render(HtmlTextWriter writer)
        {
            if (!DesignMode)
            {
                EFClientTools.ClientUtility.ClientInfo.cErrorCode = 8210;
                writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ID);
                writer.RenderBeginTag(HtmlTextWriterTag.Span);
                writer.Write(Text);
                writer.RenderEndTag();
                EFClientTools.ClientUtility.ClientInfo.cErrorCode = 0;
            }
        }
    
    }
}
