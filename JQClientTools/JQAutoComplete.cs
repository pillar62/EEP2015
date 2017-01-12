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
    public class JQAutoComplete : WebControl, IJQDataSourceProvider
    {
        public JQAutoComplete()
        {
        }
        /// <summary>
        /// 数据源
        /// </summary>
        [Category("Infolight")]
        [Editor(typeof(RemoteNameEditor), typeof(UITypeEditor))]
        public string RemoteName { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string DataMember
        {
            get
            {
                if (!string.IsNullOrEmpty(RemoteName))
                {
                    var remoteNames = RemoteName.Split('.');
                    if (remoteNames.Length == 2)
                    {
                        return remoteNames[1];
                    }
                }
                return string.Empty;
            }
            set { }
        }

        [Category("Infolight")]
        [Editor(typeof(FieldEditor), typeof(UITypeEditor))]
        public string ValueField { get; set; }

        [Category("Infolight")]

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

                        if (pname == "remoteName")
                        {
                            this.RemoteName = pvalue;
                        }
                        else if (pname == "valueField")
                        {
                            this.ValueField = pvalue;
                        }
                    }
                    op = string.Empty;
                }
            }
        }

        internal void CheckProperties()
        {
            if (string.IsNullOrEmpty(RemoteName))
            {
                throw new JQProperyNullException(this.ID, typeof(JQComboBox), "RemoteName");
            }
            if (string.IsNullOrEmpty(ValueField))
            {
                throw new JQProperyNullException(this.ID, typeof(JQComboBox), "ValueField");
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (!this.DesignMode)
            {
                EFClientTools.ClientUtility.ClientInfo.cErrorCode = 8010;

                CheckProperties();
                writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ID);
                writer.AddAttribute(HtmlTextWriterAttribute.Class, JQControl.AutoComplete);
                if (RemoteName != null && RemoteName != "")
                {
                    writer.AddAttribute(JQProperty.InfolightOptions, this.InfolightOptions);
                    writer.RenderBeginTag(HtmlTextWriterTag.Input);
                }
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
                options.Add(string.Format("valueField:'{0}'", ValueField));
                options.Add(string.Format("remoteName:'{0}'", RemoteName));
                options.Add(string.Format("tableName:'{0}'", DataMember));
                return string.Join(",", options);
            }
        }
    }
}
