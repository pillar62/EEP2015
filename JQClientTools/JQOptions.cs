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
    public class JQOptions : WebControl, IJQDataSourceProvider
    {
        public JQOptions()
        {
            ColumnCount = 2;
            DialogTitle = "JQOptions";
            items = new JQCollection<JQComboItem>(this);
            OpenDialog = true;
        }

        /// <summary>
        /// 数据源
        /// </summary>
        [Category("Infolight")]
        [Editor(typeof(RemoteNameEditor), typeof(UITypeEditor))]
        public string RemoteName { get; set; }

        [Category("Infolight")]
        public bool OpenDialog { get; set; }
        [Category("Infolight")]
        public bool SelectAll { get; set; }

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

        private String _DisplayMember;
        [Category("Infolight")]
        [Editor(typeof(FieldEditor), typeof(UITypeEditor))]
        public string DisplayMember
        {
            get
            {
                return _DisplayMember;
            }
            set
            {
                _DisplayMember = value;
            }
        }

        private String _ValueMember;
        [Category("Infolight")]
        [Editor(typeof(FieldEditor), typeof(UITypeEditor))]
        public string ValueMember
        {
            get
            {
                return _ValueMember;
            }
            set
            {
                _ValueMember = value;


            }
        }

        private JQCollection<JQComboItem> items;
        [Category("Infolight")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [NotifyParentProperty(true)]
        public JQCollection<JQComboItem> Items
        {
            get
            {
                return items;
            }
        }

        [Category("Infolight")]
        public string DialogTitle { get; set; }

        private int _DialogWidth = 300;
        [Category("Infolight")]
        public int DialogWidth
        {
            get
            {
                return _DialogWidth;

            }
            set
            {
                _DialogWidth = value;
            }
        }

        private int _DialogHeight = 0;
        [Category("Infolight")]
        public int DialogHeight
        {
            get
            {
                return _DialogHeight;
            }
            set
            {
                _DialogHeight = value;
            }
        }

        [Category("Infolight")]
        public string OnSelect { get; set; }
        [Category("Infolight")]
        public string OnWhere { get; set; }
        [Category("Infolight")]
        public int ColumnCount { get; set; }
        [Category("Infolight")]
        public bool MultiSelect { get; set; }
        [Category("Infolight")]
        public bool SelectOnly { get; set; }

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
                        if (pname == "title")
                        {
                            this.DialogTitle = pvalue;
                        }
                        else if (pname == "panelWidth")
                        {
                            try
                            {
                                this.DialogWidth = int.Parse(pvalue);
                            }
                            catch
                            { }
                        }
                        else if (pname == "panelHeight")
                        {
                            try
                            {
                                this.DialogHeight = int.Parse(pvalue);
                            }
                            catch
                            { }
                        }
                        else if (pname == "columnCount")
                        {
                            try
                            {
                                this.ColumnCount = int.Parse(pvalue);
                            }
                            catch
                            { }
                        }
                        else if (pname == "multiSelect")
                        {
                            bool b = false;
                            if (Boolean.TryParse(pvalue, out b))
                            {
                                this.MultiSelect = b;
                            }
                        }
                        else if (pname == "openDialog")
                        {
                            bool b = false;
                            if (Boolean.TryParse(pvalue, out b))
                            {
                                this.OpenDialog = b;
                            }
                        }
                        else if (pname == "selectAll")
                        {
                            bool b = false;
                            if (Boolean.TryParse(pvalue, out b))
                            {
                                this.SelectAll = b;
                            }
                        }
                        else if (pname == "remoteName")
                        {
                            this.RemoteName = pvalue;
                        }
                        else if (pname == "valueField")
                        {
                            this.ValueMember = pvalue;
                        }
                        else if (pname == "textField")
                        {
                            this.DisplayMember = pvalue;
                        }
                        else if (pname == "onSelect")
                        {
                            this.OnSelect = pvalue;
                        }
                        else if (pname == "selectOnly")
                        {
                            bool vselectonly = false;
                            Boolean.TryParse(pvalue, out vselectonly);
                            this.SelectOnly = vselectonly;
                        }
                        else if (pname == "onWhere")
                        {
                            this.OnWhere = pvalue;
                        }
                        else if (pname == "items")
                        {
                            var items = pvalue.Trim('[', ']');
                            var matches = Regex.Matches(items, @"(?<=\{).*?(?=\})");
                            if (matches.Count > 0)
                            {
                                foreach (Match match in matches)
                                {
                                    var item = new JQComboItem();
                                    var columnOptions = match.Value.Split(',');
                                    foreach (var cop in columnOptions)
                                    {
                                        var cparts = cop.Split(':');
                                        if (cparts.Length >= 2)
                                        {
                                            var cpname = cparts[0].Trim();
                                            var cpvalue = cop.Substring(cpname.Length + 1).Trim('\'');
                                            if (cpname == "text")
                                            {
                                                item.Text = cpvalue;
                                            }
                                            else if (cpname == "value")
                                            {
                                                item.Value = cpvalue;
                                            }
                                        }
                                    }
                                    this.Items.Add(item);
                                }
                            }
                        }
                      
                       
                    }
                    op = string.Empty;
                }
            }
        }


        internal void CheckProperties()
        {
            if (Items.Count == 0)
            {
                if (string.IsNullOrEmpty(RemoteName))
                {
                    throw new JQProperyNullException(this.ID, typeof(JQOptions), "RemoteName");
                }
                if (string.IsNullOrEmpty(DisplayMember))
                {
                    throw new JQProperyNullException(this.ID, typeof(JQOptions), "DisplayMember");
                }
                if (string.IsNullOrEmpty(ValueMember))
                {
                    throw new JQProperyNullException(this.ID, typeof(JQOptions), "ValueMember");
                }
            }
            else if (!string.IsNullOrEmpty(RemoteName))
            {
                if (string.IsNullOrEmpty(DisplayMember))
                {
                    throw new JQProperyNullException(this.ID, typeof(JQOptions), "DisplayMember");
                }
                if (string.IsNullOrEmpty(ValueMember))
                {
                    throw new JQProperyNullException(this.ID, typeof(JQOptions), "ValueMember");
                }
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (!this.DesignMode)
            {
                EFClientTools.ClientUtility.ClientInfo.cErrorCode = 8280;
                CheckProperties();
                writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ID);
                writer.AddAttribute(HtmlTextWriterAttribute.Class, JQControl.Options);
                writer.AddAttribute(JQProperty.InfolightOptions, this.InfolightOptions);
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
                options.Add(string.Format("title:'{0}'", DialogTitle));
                options.Add(string.Format("panelWidth:{0}", DialogWidth));
                if (DialogHeight > 0)
                {
                    options.Add(string.Format("panelHeight:{0}", DialogHeight));
                }
                options.Add(string.Format("remoteName:'{0}'", RemoteName));
                options.Add(string.Format("tableName:'{0}'", DataMember));

                options.Add(string.Format("valueField:'{0}'", ValueMember));
                options.Add(string.Format("textField:'{0}'", DisplayMember));
                options.Add(string.Format("columnCount:{0}", ColumnCount));
                options.Add(string.Format("multiSelect:{0}", MultiSelect.ToString().ToLower()));
                options.Add(string.Format("openDialog:{0}", OpenDialog.ToString().ToLower()));
                options.Add(string.Format("selectAll:{0}", SelectAll.ToString().ToLower()));
                if (!string.IsNullOrEmpty(OnSelect))
                {
                    options.Add(string.Format("onSelect:{0}", OnSelect));
                }
                if (!string.IsNullOrEmpty(OnWhere))
                {
                    options.Add(string.Format("onWhere:{0}", OnWhere));

                }
                var items = new List<string>();
                foreach (var item in this.Items)
                {
                    items.Add(string.Format("{{text:'{0}',value:'{1}'}}", item.Text, item.Value));
                }
                options.Add(string.Format("selectOnly:{0}", SelectOnly.ToString().ToLower()));
                options.Add(string.Format("items:[{0}]", string.Join(",", items)));
                return string.Join(",", options);
            }
        }
    }
}
