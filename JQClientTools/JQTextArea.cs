using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Web.UI;

namespace JQClientTools
{
    public class JQTextArea : WebControl
    {
        [Category("Infolight")]
        public override Unit Height 
        {
            get { return base.Height; }
            set { base.Height = value; }
        }

        internal void LoadProperties(string value)
        {
            if (!string.IsNullOrEmpty((string)value))
            {
                var options = ((string)value).Split(',');
                var op = string.Empty;
                foreach (var option in options)
                {
                    if (op.Length > 0)
                    {
                        op += ',';
                    }
                    op += option;
                    if (op.Split('{').Length != op.Split('}').Length)
                    {
                        continue;
                    }
                    if (op.Split('[').Length != op.Split(']').Length)
                    {
                        continue;
                    }
                    var index = op.IndexOf(':');

                    if (index > 0)
                    {
                        var pname = op.Substring(0, index).Trim();
                        var pvalue = op.Substring(index + 1).Trim('\'');

                        if (pname == "height")
                        {
                            double height = 0;
                            if (double.TryParse(pvalue, out height))
                            {
                                this.Height = new Unit(height, UnitType.Pixel);
                            }
                        }
                    }
                    op = string.Empty;
                }
            }
        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            if (!this.DesignMode)
            {
                EFClientTools.ClientUtility.ClientInfo.cErrorCode = 8380;
                writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ID);
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
                writer.RenderBeginTag(HtmlTextWriterTag.Textarea);
                writer.RenderEndTag();
                EFClientTools.ClientUtility.ClientInfo.cErrorCode = 0;
            }
        }


        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        internal string InfolightOptions
        {
            get
            {
                var options = new List<string>();
                if (this.Height.Type == UnitType.Pixel && Height.Value > double.Epsilon)
                {
                    options.Add(string.Format("height:{0}", this.Height.Value));
                }
                return string.Join(",", options);
            }
        }

    }
}
