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
    public class JQImageContainer : Panel
    {
        public JQImageContainer()
        {
            AutoSize = false;
            autoWidthAndHeight = true;
        }
        [Category("Infolight")]
        [Obsolete()]
        [Browsable(false)]
        public bool autoWidthAndHeight { get; set; }

        [Category("Infolight")]
        public bool AutoSize { get; set; }
        [Category("Infolight")]
        public override Unit Width
        {
            get
            {
                return base.Width;
            }
            set
            {
                base.Width = value;
            }
        }
        [Category("Infolight")]
        public override Unit Height
        {
            get
            {
                return base.Height;
            }
            set
            {
                base.Height = value;
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (!this.DesignMode)
            {
                EFClientTools.ClientUtility.ClientInfo.cErrorCode = 8200;
                writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ID);
                if (AutoSize)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Style, "display:none;");
                }
                else
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Style, "display:none;width:" + Width + ";height:" + Height + "");
                }
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "info-imagecontainer");
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                writer.RenderEndTag();
                EFClientTools.ClientUtility.ClientInfo.cErrorCode = 0;
            }
        }
    }
}
