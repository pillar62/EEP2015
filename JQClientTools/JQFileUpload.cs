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
    public class JQFileUpload : WebControl
    {
        public JQFileUpload()
        {
            IsAutoNum = false;
            ShowButton = true;
            ShowLocalFile = false;
            FileSizeLimited = 500;
        }

        [Category("Infolight")]
        [Description("split of '|' .e.x.: jpg|png")]
        public string Filter { get; set; }

        [Category("Infolight")]
        public bool IsAutoNum { get; set; }

        [Category("Infolight")]
        public string UpLoadFolder { get; set; }

        [Category("Infolight")]
        public bool ShowButton { get; set; }

        [Category("Infolight")]
        public bool ShowLocalFile { get; set; }

        [Category("Infolight")]
        public string onSuccess { get; set; }

        [Category("Infolight")]
        public string onError { get; set; }

        [Category("Infolight")]
        public string onBeforeUpload { get; set; }

        [Category("Infolight")]
        public string SizeFieldName { get; set; }

        [Category("Infolight")]
        [Description("K")]
        public double FileSizeLimited { get; set; }

        [Category("Infolight")]
        public string Accept { get; set; }

        internal void LoadProperties(string value)
        {
            if (!string.IsNullOrEmpty((string)value))
            {
                var options = ((string)value).Split(',');
                var filter = string.Empty;
                var isAutoNum = string.Empty;
                var upLoadFolder = string.Empty;
                var showButton = string.Empty;
                var Success = string.Empty;
                var Error = string.Empty;
                var beforeUpload = string.Empty;
                var accept = string.Empty;
                var fileSizeLimited = string.Empty;
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
                        if (pname == "isAutoNum")
                        {
                            try
                            {
                                this.IsAutoNum = bool.Parse(pvalue);
                            }
                            catch
                            { }
                        }
                        else if (pname == "filter")
                        {
                            this.Filter = pvalue;
                        }
                        else if (pname == "showButton")
                        {
                            try
                            {
                                this.ShowButton = bool.Parse(pvalue);
                            }
                            catch
                            { }
                        }
                        else if (pname == "showLocalFile")
                        {
                            try
                            {
                                this.ShowLocalFile = bool.Parse(pvalue);
                            }
                            catch
                            { }
                        }
                        else if (pname == "upLoadFolder")
                        {
                            this.UpLoadFolder = pvalue;
                        }
                        else if (pname == "onSuccess")
                        {
                            this.onSuccess = pvalue;
                        }
                        else if (pname == "onError")
                        {
                            this.onError = pvalue;
                        }
                        else if (pname == "onBeforeUpload")
                        {
                            this.onBeforeUpload = pvalue;
                        }
                        else if (pname == "sizeFieldName")
                        {
                            this.SizeFieldName = pvalue;
                        }
                        else if (pname == "accept")
                        {
                            this.Accept = pvalue;
                        }
                        else if (pname == "fileSizeLimited")
                        {
                            try
                            {
                                this.FileSizeLimited = double.Parse(pvalue);
                            }
                            catch
                            { }
                        }
                    }
                    op = string.Empty;
                }
            }
        }


        protected override void Render(HtmlTextWriter writer)
        {
            if (!this.DesignMode)
            {
                EFClientTools.ClientUtility.ClientInfo.cErrorCode = 8170;
                writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ID);
                writer.AddAttribute(HtmlTextWriterAttribute.Class, JQControl.FileUpload);
                writer.AddAttribute(JQProperty.InfolightOptions, this.InfolightOptions);
                writer.AddAttribute(HtmlTextWriterAttribute.Style, "width:" + this.Width);
                writer.RenderBeginTag(HtmlTextWriterTag.Input);
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
                options.Add(string.Format("filter:'{0}'", Filter));
                options.Add(string.Format("isAutoNum:{0}", IsAutoNum.ToString().ToLower()));
                options.Add(string.Format("upLoadFolder:'{0}'", UpLoadFolder));
                options.Add(string.Format("showButton:{0}", ShowButton.ToString().ToLower())); 
                options.Add(string.Format("showLocalFile:{0}", ShowLocalFile.ToString().ToLower()));
                if(!string.IsNullOrEmpty(onSuccess))
                {
                    options.Add(string.Format("onSuccess:{0}", onSuccess));
                }
                if (!string.IsNullOrEmpty(onError))
                {
                    options.Add(string.Format("onError:{0}", onError));
                }
                if (!string.IsNullOrEmpty(onBeforeUpload))
                {
                    options.Add(string.Format("onBeforeUpload:{0}", onBeforeUpload));
                }
                if (!string.IsNullOrEmpty(SizeFieldName))
                {
                    options.Add(string.Format("sizeFieldName:'{0}'", SizeFieldName));
                }
                if (!Double.IsNaN(FileSizeLimited))
                {
                    options.Add(string.Format("fileSizeLimited:'{0}'", FileSizeLimited.ToString()));
                }
                if (!string.IsNullOrEmpty(Accept))
                {
                    options.Add(string.Format("accept:'{0}'", Accept));
                }
                return string.Join(",", options);
            }
        }

    }
}
