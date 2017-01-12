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
    public class JQTextBox : WebControl
    {

        public JQTextBox()
        {
            CapsLock = CapsLockEnum.None;
        }

        /// <summary>
        /// 大小写
        /// </summary>
        [Category("Infolight")]
        public CapsLockEnum CapsLock { get; set; }
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
                        if (pname == "capsLock")
                        {
                            this.CapsLock = (CapsLockEnum)Enum.Parse(typeof(CapsLockEnum),pvalue,true);
                        }
                    }
                    op = string.Empty;
                }
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (!DesignMode)
            {
                EFClientTools.ClientUtility.ClientInfo.cErrorCode = 8450;
                writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ID);
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
                options.Add(string.Format("capsLock:'{0}'", CapsLock.ToString().ToLower()));
                return string.Join(",", options);
            }
        }

    }
    public enum CapsLockEnum
    {
        None, Upper, Lower
    }

}
