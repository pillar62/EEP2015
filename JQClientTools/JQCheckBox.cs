using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.ComponentModel;

namespace JQClientTools
{
    public class JQCheckBox : WebControl
    {
        [Category("Infolight")]
        public string On { get; set; }
        [Category("Infolight")]
        public string Off { get; set; }

        internal void LoadProperties(string value)
        {
            if (!string.IsNullOrEmpty((string)value))
            {
                var options = ((string)value).Split(',');
                foreach (var op in options)
                {
                    var parts = op.Split(':');
                    if (parts.Length == 2)
                    {
                        var pname = parts[0].Trim();
                        var pvalue = parts[1].Trim();
                        if (pname == "on")
                        {
                            this.On = pvalue;
                        }
                        else if (pname == "off")
                        {
                            this.Off = pvalue;
                        }
                    }
                }
            }
        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            if (!this.DesignMode)
            {
                EFClientTools.ClientUtility.ClientInfo.cErrorCode = 8050;
                writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ID);
                writer.AddAttribute(HtmlTextWriterAttribute.Type, "checkbox");
                writer.RenderBeginTag(HtmlTextWriterTag.Input);
                writer.RenderEndTag();
                EFClientTools.ClientUtility.ClientInfo.cErrorCode = 0;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        internal string DataOptions
        {
            get
            {
                var options = new List<string>();
                if (!string.IsNullOrEmpty(On))
                {
                    options.Add(string.Format("on:{0}", On));
                }
                if (!string.IsNullOrEmpty(Off))
                {
                    options.Add(string.Format("off:{0}", Off));
                }
                return string.Join(",", options);
            }
        }



    }
}
