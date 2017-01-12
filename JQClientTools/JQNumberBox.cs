using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.ComponentModel;
namespace JQClientTools
{
    public class JQNumberBox : WebControl
    {
        public JQNumberBox()
        {
            Precision = 0;
        }

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
                        if (pname == "precision")
                        {
                            try
                            {
                                this.Precision = int.Parse(pvalue);
                            }
                            catch { }
                        }
                    }
                }
            }
        }

        [Category("Infolight")]
        public int Precision { get; set; }


        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            if (!this.DesignMode)
            {
                EFClientTools.ClientUtility.ClientInfo.cErrorCode = 8270;
                writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ID);
                writer.AddAttribute(HtmlTextWriterAttribute.Type, "text");
                writer.AddAttribute(HtmlTextWriterAttribute.Class, JQControl.NumberBox);
                writer.AddAttribute(JQProperty.DataOptions, this.DataOptions);
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

                options.Add(string.Format("precision:{0}", Precision));
                return string.Join(",", options);
            }
        }
    }
}
