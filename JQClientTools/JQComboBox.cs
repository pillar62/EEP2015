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
    public class JQComboBox : WebControl, IJQDataSourceProvider
    {
        public JQComboBox()
        {
            PageSize = -1;
            PanelHeight = 200;
            items = new JQCollection<JQComboItem>(this);
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
        public string DisplayMember { get; set; }

        [Category("Infolight")]
        public bool CheckData { get; set; }

        [Category("Infolight")]
        [Editor(typeof(FieldEditor), typeof(UITypeEditor))]
        public string ValueMember { get; set; }

        [Category("Infolight")]
        public string OnSelect { get; set; }

        [Category("Infolight")]
        public string OnBeforeLoad { get; set; }

        [Category("Infolight")]
        public int PageSize { get; set; }

        [Category("Infolight")]
        public bool SelectOnly { get; set; }

        [Category("Infolight")]
        public int PanelHeight { get; set; }

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
        public bool CacheRelationText { get; set; }

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
                        else if (pname == "onSelect")
                        {
                            this.OnSelect = pvalue;
                        }
                        else if (pname == "onBeforeLoad")
                        {
                            this.OnBeforeLoad = pvalue;
                        }
                        else if (pname == "checkData")
                        {
                            bool b = false;
                            if (Boolean.TryParse(pvalue, out b))
                            {
                                this.CheckData = b;
                            }

                        }
                        else if (pname == "selectOnly")
                        {
                            bool b = false;
                            if (Boolean.TryParse(pvalue, out b))
                            {
                                this.SelectOnly = b;
                            }

                        }
                        else if (pname == "cacheRelationText")
                        {
                            bool vcacheRelationText = true;
                            Boolean.TryParse(pvalue, out vcacheRelationText);
                            this.CacheRelationText = vcacheRelationText;
                        }
                        else if (pname == "items")
                        {
                            var columns = pvalue.Trim('[', ']');
                            var matches = Regex.Matches(columns, @"(?<=\{).*?(?=\})");
                            if (matches.Count > 0)
                            {
                                foreach (Match match in matches)
                                {
                                    var item = new JQComboItem();
                                    var itemOptions = match.Value.Split(',');
                                    foreach (var cop in itemOptions)
                                    {
                                        var cparts = cop.Split(':');
                                        if (cparts.Length >= 2)
                                        {
                                            var cpname = cparts[0].Trim();
                                            var cpvalue = cop.Substring(cpname.Length+1).Trim('\'');
                                            if (cpname == "value")
                                            {
                                                item.Value = cpvalue;
                                            }
                                            else if (cpname == "text")
                                            {
                                                item.Text = cpvalue;
                                            }
                                            else if (cpname == "selected")
                                            {
                                                try
                                                {
                                                    item.Selected = bool.Parse(cpvalue);
                                                }
                                                catch { }
                                            }
                                        }
                                    }
                                    this.Items.Add(item);
                                }
                            }
                        }
                        else if (pname == "pageSize")
                        {
                            int i = 0;
                            if (Int32.TryParse(pvalue, out i))
                                this.PageSize = i;
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
            if (Items.Count == 0)
            {
                if (string.IsNullOrEmpty(RemoteName))
                {
                    throw new JQProperyNullException(this.ID, typeof(JQComboBox), "RemoteName");
                }
                if (string.IsNullOrEmpty(DisplayMember))
                {
                    throw new JQProperyNullException(this.ID, typeof(JQComboBox), "DisplayMember");
                }
                if (string.IsNullOrEmpty(ValueMember))
                {
                    throw new JQProperyNullException(this.ID, typeof(JQComboBox), "ValueMember");
                }
            }
            else if (!string.IsNullOrEmpty(RemoteName))
            {
                if (string.IsNullOrEmpty(DisplayMember))
                {
                    throw new JQProperyNullException(this.ID, typeof(JQComboBox), "DisplayMember");
                }
                if (string.IsNullOrEmpty(ValueMember))
                {
                    throw new JQProperyNullException(this.ID, typeof(JQComboBox), "ValueMember");
                }
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (!this.DesignMode)
            {
                EFClientTools.ClientUtility.ClientInfo.cErrorCode = 8070;
                CheckProperties();
                writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ID);
                writer.AddAttribute(HtmlTextWriterAttribute.Class, JQControl.ComboBox);
                writer.AddAttribute(JQProperty.InfolightOptions, this.InfolightOptions);
                if (RemoteName != null && RemoteName != "")
                {
                    writer.RenderBeginTag(HtmlTextWriterTag.Input);
                }
                else
                {
                    writer.RenderBeginTag(HtmlTextWriterTag.Select);
                    if (Items != null)
                    {
                        foreach (JQComboItem item in Items)
                        {
                            if (item.Selected)
                            {
                                writer.Write("<option value=\"" + item.Value + "\" selected>" + item.Text + "</option>");
                            }
                            else
                                writer.Write("<option value=\"" + item.Value + "\">" + item.Text + "</option>");
                        }
                    }
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

                if (string.IsNullOrEmpty(RemoteName) && Items.Count > 0)
                {
                    var columns = new List<string>();
                    foreach (var column in Items)
                    {
                        columns.Add(string.Format("{{value:'{0}',text:'{1}',selected:'{2}'}}"
                            , column.Value, column.Text, column.Selected.ToString().ToLower()));
                    }

                    options.Add(string.Format("items:[{0}]", string.Join(",", columns)));
                }
                else
                {

                    options.Add(string.Format("valueField:'{0}'", ValueMember));
                    options.Add(string.Format("textField:'{0}'", DisplayMember));
                    options.Add(string.Format("remoteName:'{0}'", RemoteName));
                    options.Add(string.Format("tableName:'{0}'", DataMember));
                    options.Add(string.Format("pageSize:'{0}'", PageSize.ToString()));
                }

                options.Add(string.Format("checkData:{0}", CheckData.ToString().ToLower()));
                options.Add(string.Format("selectOnly:{0}", SelectOnly.ToString().ToLower()));
                options.Add(string.Format("cacheRelationText:{0}", CacheRelationText.ToString().ToLower()));
                if (!string.IsNullOrEmpty(OnSelect))
                {
                    options.Add(string.Format("onSelect:{0}", OnSelect));
                }
                if (!string.IsNullOrEmpty(OnBeforeLoad))
                {
                    options.Add(string.Format("onBeforeLoad:{0}", OnBeforeLoad));
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

    public class JQComboItem : JQCollectionItem
    {
        public JQComboItem()
        {

        }

        /// <summary>
        /// value
        /// </summary>
        [Category("Infolight")]
        public string Value { get; set; }
        /// <summary>
        /// text
        /// </summary>
        [Category("Infolight")]
        public string Text { get; set; }

        private bool _Selected = false;
        /// <summary>
        /// 是否select
        /// </summary>
        [Category("Infolight")]
        public bool Selected
        {
            get
            {
                return _Selected;
            }
            set
            {
                _Selected = value;
            }
        }

        public void Render(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Value, Value);
            if (Selected)
                writer.AddAttribute(HtmlTextWriterAttribute.Selected, "Selected");
            writer.RenderBeginTag(HtmlTextWriterTag.Option);
            writer.Write(this.Text);
            writer.RenderEndTag();

        }

        public override string ToString()
        {
            if (!string.IsNullOrEmpty(Text))
            {
                return Text;
            }
            else
            {
                return base.ToString();
            }
        }
    }

}
