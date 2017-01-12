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
    public class JQImage : WebControl
    {
        public JQImage()
        {
            Width = new Unit(160, UnitType.Pixel);
            //Height = new Unit(120, UnitType.Pixel);
        }

        [Category("Infolight")]
        public string Src { get; set; }

        protected override void Render(HtmlTextWriter writer)
        {
            if (!DesignMode)
            {
                EFClientTools.ClientUtility.ClientInfo.cErrorCode = 8190;
                writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ID);
                writer.AddAttribute(HtmlTextWriterAttribute.Src, Src);
                writer.AddAttribute(HtmlTextWriterAttribute.Width, string.Format("{0}px", Width.Value));
                if (this.Height.Type == UnitType.Pixel && Height.Value > double.Epsilon)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Height, string.Format("{0}px", Height.Value));
                }
                writer.RenderBeginTag(HtmlTextWriterTag.Img);
                //writer.Write(Text);
                writer.RenderEndTag();
                EFClientTools.ClientUtility.ClientInfo.cErrorCode = 0;
            }
        }
    }
}
