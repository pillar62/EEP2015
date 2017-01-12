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
    public class JQQrcode: WebControl
    {
        public JQQrcode()
        {
            Size = 120;
            RenderMode = JQQrcodeMode.table;
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (!this.DesignMode)
            {
                EFClientTools.ClientUtility.ClientInfo.cErrorCode = 8300;
                writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ID);
                writer.AddAttribute(HtmlTextWriterAttribute.Class, JQControl.Qrcode);
                writer.AddAttribute(JQProperty.DataOptions, DataOptions);
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                writer.RenderEndTag();
                EFClientTools.ClientUtility.ClientInfo.cErrorCode = 0;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string DataOptions
        {
            get
            {
                var options = new List<string>();
                if (Size.Type == UnitType.Pixel && Size.Value > double.Epsilon)
                {
                    options.Add(string.Format("size:{0}", Size.Value.ToString()));
                }
                options.Add(string.Format("render:'{0}'", RenderMode.ToString()));
                
                return string.Join(",", options);
            }
        }
        [Category("Infolight")]
        public Unit Size { get; set; }
        private JQQrcodeMode _RenderMode = JQQrcodeMode.table;
        [Category("Infolight")]
        public JQQrcodeMode RenderMode
        {
            get
            {
                return _RenderMode;
            }
            set { _RenderMode = value; }
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

                        if (pname == "size")
                        {
                            this.Size = Unit.Parse(pvalue);
                        }
                        if (pname == "render")
                        {
                            try
                            {
                                this.RenderMode = (JQQrcodeMode)(Enum.Parse(typeof(JQQrcodeMode), pvalue, false));
                            }
                            catch { }
                        }
                    }
                }
            }
        }

        public enum JQQrcodeMode
        {
            table, canvas
        }
    }
}
