using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Web.UI;
using System.Drawing.Design;
using System.Text.RegularExpressions;

namespace JQClientTools
{
    public class JQFLComment : WebControl
    {
        protected override void Render(HtmlTextWriter writer)
        {
            if (!DesignMode)
            {
                EFClientTools.ClientUtility.ClientInfo.cErrorCode = 8180;
                writer.AddAttribute(HtmlTextWriterAttribute.Class, JQControl.FLComment);
                writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ID);
                writer.RenderBeginTag(HtmlTextWriterTag.Table);
                writer.RenderEndTag();
                EFClientTools.ClientUtility.ClientInfo.cErrorCode = 0;
            }
        }
    }
}
