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
    public class JQTimeSpinner: WebControl
    {
        public JQTimeSpinner()
        {
            ShowSeconds = false;
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (!this.DesignMode)
            {
                EFClientTools.ClientUtility.ClientInfo.cErrorCode = 8390;
                writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ID);
                writer.AddAttribute(HtmlTextWriterAttribute.Class, JQControl.TimeSpinner);
                writer.AddAttribute(JQProperty.DataOptions, DataOptions);
                writer.RenderBeginTag(HtmlTextWriterTag.Input);
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
                options.Add(string.Format("showSeconds:{0}", ShowSeconds.ToString().ToLower()));
                return string.Join(",", options);
            }
        }
        [Category("Infolight")]
        public bool ShowSeconds { get; set; }
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

                        if (pname == "showSeconds")
                        {
                            this.ShowSeconds = bool.Parse(pvalue);
                        }

                    }
                }
            }
        }


    }
}
