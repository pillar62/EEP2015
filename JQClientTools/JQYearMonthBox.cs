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
    public class JQYearMonthBox : WebControl
    {
        public JQYearMonthBox()
        {
            Format = "yyyy.mm";
            DurationMinus = 12;
            DurationPlus = 12;
            Type = JQYearMonthBoxType.Year;
            DataType = JQYearMonthBoxDataType.Varchar;
        }

        [Category("Infolight")]
        public string OnSelect { get; set; }

        [Category("Infolight")]
        public bool SelectOnly { get; set; }

        [Category("Infolight")]
        public int PanelHeight { get; set; }

        [Category("Infolight")]
        public int DurationMinus { get; set; }

        [Category("Infolight")]
        public int DurationPlus { get; set; }

        [Category("Infolight")]
        public string Format { get; set; }

        [Category("Infolight")]
        public JQYearMonthBoxDataType DataType { get; set; }

        [Category("Infolight")]
        public JQYearMonthBoxType Type { get; set; }

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

                        if (pname == "onSelect")
                        {
                            this.OnSelect = pvalue;
                        }
                        else if (pname == "format")
                        {
                            this.Format = pvalue;
                        }

                        else if (pname == "selectOnly")
                        {
                            bool b = false;
                            if (Boolean.TryParse(pvalue, out b))
                            {
                                this.SelectOnly = b;
                            }

                        }
                        else if (pname == "durationMinus")
                        {
                            int i = 0;
                            if (Int32.TryParse(pvalue, out i))
                                this.DurationMinus = i;
                        }
                        else if (pname == "type")
                        {
                            JQYearMonthBoxType i = 0;
                            if (Enum.TryParse(pvalue,true, out i))
                                this.Type = i;
                        }
                        else if (pname == "datatype")
                        {
                            JQYearMonthBoxDataType i = 0;
                            if (Enum.TryParse(pvalue,true, out i))
                                this.DataType = i;
                        }
                        else if (pname == "durationPlus")
                        {
                            int i = 0;
                            if (Int32.TryParse(pvalue, out i))
                                this.DurationPlus = i;
                        }
                        else if (pname == "panelHeight")
                        {
                            int i = 0;
                            if (Int32.TryParse(pvalue, out i))
                                this.PanelHeight = i;
                        }
                    }
                    op = string.Empty;
                }
            }
        }

        internal void CheckProperties()
        {
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (!this.DesignMode)
            {
                EFClientTools.ClientUtility.ClientInfo.cErrorCode = 8440;
                CheckProperties();
                writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ID);
                writer.AddAttribute(HtmlTextWriterAttribute.Class, JQControl.YearMonth);
                writer.AddAttribute(JQProperty.InfolightOptions, this.InfolightOptions);
                writer.RenderBeginTag(HtmlTextWriterTag.Select);
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
                options.Add(string.Format("durationMinus:{0}", DurationMinus.ToString().ToLower()));
                options.Add(string.Format("durationPlus:{0}", DurationPlus.ToString().ToLower()));
                options.Add(string.Format("type:'{0}'", Type.ToString().ToLower()));
                options.Add(string.Format("datatype:'{0}'", DataType.ToString().ToLower()));
                options.Add(string.Format("format:'{0}'", Format.ToString()));
                options.Add(string.Format("selectOnly:{0}", SelectOnly.ToString().ToLower()));
                if (!string.IsNullOrEmpty(OnSelect))
                {
                    options.Add(string.Format("onSelect:{0}", OnSelect));
                }
                //if (PageSize != null)
                //{
               
                //}
                if (PanelHeight > 0)
                {
                    options.Add(string.Format("panelHeight:{0}", PanelHeight));
                }
                else if (PanelHeight == 0)
                {
                    options.Add(string.Format("panelHeight:'auto'"));
                }
                return string.Join(",", options);
            }
        }
    }
    public enum JQYearMonthBoxType
    {
        Year, Month
    }
    public enum JQYearMonthBoxDataType
    {
        Varchar, Datetime
    }
}
