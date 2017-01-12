using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Drawing.Design;

namespace JQClientTools
{
    public class JQRelation: IJQDataSourceProvider
    {
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
        public string DisplayMember { get; set; }

        [Category("Infolight")]
        [Editor(typeof(FieldEditor), typeof(UITypeEditor))]
        public string ValueMember { get; set; }

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
                        else if (pname == "textField")
                        {
                            this.DisplayMember = pvalue;
                        }
                        else if (pname == "valueField")
                        {
                            this.ValueMember = pvalue;
                        }
                       
                    }
                    op = string.Empty;
                }
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        internal string InfolightOptions
        {
            get
            {
                var options = new List<string>();
                if (!string.IsNullOrEmpty(RemoteName))
                {
                    options.Add(string.Format("valueField:'{0}'", ValueMember));
                    options.Add(string.Format("textField:'{0}'", DisplayMember));
                    options.Add(string.Format("remoteName:'{0}'", RemoteName));
                    options.Add(string.Format("tableName:'{0}'", DataMember));
                }
                return string.Join(",", options);
            }
        }
    }
}
