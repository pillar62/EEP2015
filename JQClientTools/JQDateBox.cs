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
    public class JQDateBox: WebControl
    {
        public JQDateBox()
        {
            Format = DateFormat.DateTime;
            ShowSeconds = true;
            ShowTimeSpinner = false;
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (!this.DesignMode)
            {
                EFClientTools.ClientUtility.ClientInfo.cErrorCode = 8120;
                writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ID);
                if (ShowTimeSpinner)
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, JQControl.DateTimeBox);
                else
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, JQControl.DateBox);
                writer.AddAttribute(JQProperty.DataOptions, DataOptions);
                writer.RenderBeginTag(HtmlTextWriterTag.Input);
                writer.RenderEndTag();
                EFClientTools.ClientUtility.ClientInfo.cErrorCode = 0;
            }
        }

        [Category("Infolight")]
        public bool SelectOnly { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string DataOptions
        {
            get
            {
                var options = new List<string>();
                options.Add(string.Format("dateFormat:'{0}'", Format.ToString().ToLower()));
                options.Add(string.Format("showTimeSpinner:{0}", ShowTimeSpinner.ToString().ToLower()));
                options.Add(string.Format("showSeconds:{0}", ShowSeconds.ToString().ToLower()));
                options.Add(string.Format("editable:{0}", (!SelectOnly).ToString().ToLower()));
                return string.Join(",", options);
            }
        }

        [Category("Infolight")]
        public bool ShowTimeSpinner { get; set; }
        [Category("Infolight")]
        public bool ShowSeconds { get; set; }
        [Category("Infolight")]
        public DateFormat Format { get; set; }
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
                        if (pname == "showTimeSpinner")
                        {
                            this.ShowTimeSpinner = bool.Parse(pvalue);
                        }
                        if (pname == "editable")
                        {
                            this.SelectOnly = !bool.Parse(pvalue);
                        }
                        if (pname == "showSeconds")
                        {
                            this.ShowSeconds = bool.Parse(pvalue);
                        }
                        if (pname == "dateFormat")
                        {
                            if (pvalue == "nvarchar")
                            {
                                Format = DateFormat.NVarchar;
                            }
                        }
                    }
                }
            }
        }

        public enum DateFormat
        { 
            DateTime,
            NVarchar
        }
    }
}
