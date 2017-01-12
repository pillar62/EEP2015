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
    public class JQComboGrid : WebControl, IJQDataSourceProvider, IColumnCaptions
    {
        public JQComboGrid()
        {
            columns = new JQCollection<JQGridColumn>(this);
            MultiSelect = false;
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

                if ((this as IColumnCaptions).ColumnCaptions != null)
                {
                    if ((this as IColumnCaptions).ColumnCaptions.ContainsKey(_DisplayMember))
                    {
                        DisplayMemberCaption = (this as IColumnCaptions).ColumnCaptions[_DisplayMember];
                    }
                }
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

                if ((this as IColumnCaptions).ColumnCaptions != null)
                {
                    if ((this as IColumnCaptions).ColumnCaptions.ContainsKey(_ValueMember))
                    {
                        ValueMemberCaption = (this as IColumnCaptions).ColumnCaptions[_ValueMember];
                    }
                }
            }
        }

        private String _DisplayMemberCaption;
        [Category("Infolight")]
        public string DisplayMemberCaption
        {
            get
            {
                if (string.IsNullOrEmpty(_DisplayMemberCaption))
                {
                    return DisplayMember;
                }
                else
                {
                    return _DisplayMemberCaption;
                }
            }
            set
            {
                _DisplayMemberCaption = value;
            }
        }

        private String _ValueMemberCaption;
        [Category("Infolight")]
        public string ValueMemberCaption
        {
            get
            {
                if (string.IsNullOrEmpty(_ValueMemberCaption))
                {
                    return ValueMember;
                }
                else
                {
                    return _ValueMemberCaption;
                }
            }
            set
            {
                _ValueMemberCaption = value;
            }
        }

        private int _DialogWidth = 350;
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

        private JQCollection<JQGridColumn> columns;
        [Category("Infolight")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [NotifyParentProperty(true)]
        public JQCollection<JQGridColumn> Columns
        {
            get
            {
                return columns;
            }
        }
        [Category("Infolight")]
        public bool CheckData { get; set; }

        [Category("Infolight")]
        public string OnSelect { get; set; }

        [Category("Infolight")]
        public string OnBeforeLoad { get; set; }

        [Category("Infolight")]
        public bool SelectOnly { get; set; }

        [Category("Infolight")]
        public bool CacheRelationText { get; set; }

        [Category("Infolight")]
        public bool MultiSelect { get; set; }

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
                        if (pname == "panelWidth")
                        {
                            try
                            {
                                this.DialogWidth = int.Parse(pvalue);
                            }
                            catch
                            { }
                        }
                        if (pname == "panelHeight")
                        {
                            try
                            {
                                this.DialogHeight = int.Parse(pvalue);
                            }
                            catch
                            { }
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
                        else if (pname == "valueFieldCaption")
                        {
                            this.ValueMemberCaption = pvalue;
                        }
                        else if (pname == "textFieldCaption")
                        {
                            this.DisplayMemberCaption = pvalue;
                        }
                        else if (pname == "checkData")
                        {
                            bool b = false;
                            if (Boolean.TryParse(pvalue, out b))
                            {
                                this.CheckData = b;
                            }

                        }
                        else if (pname == "onSelect")
                        {
                            this.OnSelect = pvalue;
                        }
                        else if (pname == "onBeforeLoad")
                        {
                            this.OnBeforeLoad = pvalue;
                        }
                        else if (pname == "selectOnly")
                        {
                            bool b = false;
                            if (Boolean.TryParse(pvalue, out b))
                            {
                                this.SelectOnly = b;
                            }
                        }
                        else if (pname == "multiple")
                        {
                            bool b = false;
                            if (Boolean.TryParse(pvalue, out b))
                            {
                                this.MultiSelect = b;
                            }
                        }
                        else if (pname == "cacheRelationText")
                        {
                            bool vcacheRelationText = true;
                            Boolean.TryParse(pvalue, out vcacheRelationText);
                            this.CacheRelationText = vcacheRelationText;
                        }
                        else if (pname == "columns")
                        {
                            var columns = pvalue.Trim('[', ']');
                            var matches = Regex.Matches(columns, @"(?<=\{).*?(?=\})");
                            if (matches.Count > 0)
                            {
                                foreach (Match match in matches)
                                {
                                    var column = new JQGridColumn();
                                    var columnOptions = match.Value.Split(',');
                                    foreach (var cop in columnOptions)
                                    {
                                        var cparts = cop.Split(':');
                                        if (cparts.Length == 2)
                                        {
                                            var cpname = cparts[0].Trim();
                                            var cpvalue = cparts[1].Trim('\'');
                                            if (cpname == "field")
                                            {
                                                column.FieldName = cpvalue;
                                            }
                                            else if (cpname == "title")
                                            {
                                                column.Caption = cpvalue;
                                            }
                                            else if (cpname == "width")
                                            {
                                                try
                                                {
                                                    column.Width = int.Parse(cpvalue);
                                                }
                                                catch { }
                                            }
                                            else if (cpname == "align")
                                            {
                                                column.Alignment = cpvalue;
                                            }
                                            else if (cpname == "sortable")
                                            {
                                                bool b = false;
                                                if (Boolean.TryParse(cpvalue, out b))
                                                {
                                                    column.Sortable = b;
                                                }

                                            }
                                            else if (cpname == "format")
                                            {
                                                column.Format = cpvalue;
                                            }
                                        }
                                    }
                                    this.Columns.Add(column);
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
            if (string.IsNullOrEmpty(RemoteName))
            {
                throw new JQProperyNullException(this.ID, typeof(JQComboGrid), "RemoteName");
            }
            if (string.IsNullOrEmpty(DisplayMember))
            {
                throw new JQProperyNullException(this.ID, typeof(JQComboGrid), "DisplayMember");
            }
            if (string.IsNullOrEmpty(ValueMember))
            {
                throw new JQProperyNullException(this.ID, typeof(JQComboGrid), "ValueMember");
            }
            foreach (var column in this.Columns)
            {
                column.CheckProperties();
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (!this.DesignMode)
            {
                EFClientTools.ClientUtility.ClientInfo.cErrorCode = 8080;
                CheckProperties();
                writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ID);
                writer.AddAttribute(HtmlTextWriterAttribute.Class, JQControl.ComboGrid);
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
                options.Add(string.Format("panelWidth:{0}", DialogWidth));
                if (DialogHeight > 0)
                    options.Add(string.Format("panelHeight:{0}", DialogHeight));
                options.Add(string.Format("valueField:'{0}'", ValueMember));
                options.Add(string.Format("textField:'{0}'", DisplayMember));
                options.Add(string.Format("remoteName:'{0}'", RemoteName));
                options.Add(string.Format("tableName:'{0}'", DataMember));
                options.Add(string.Format("valueFieldCaption:'{0}'", ValueMemberCaption));
                options.Add(string.Format("textFieldCaption:'{0}'", DisplayMemberCaption));
                options.Add(string.Format("selectOnly:{0}", SelectOnly.ToString().ToLower()));
                options.Add(string.Format("checkData:{0}", CheckData.ToString().ToLower()));
                var columns = new List<string>();
                foreach (var column in Columns)
                {
                    columns.Add(string.Format("{{field:'{0}',title:'{1}',width:{2},align:'{3}',sortable:{4}{5}}}"
                        , column.FieldName, column.Caption, column.Width, column.Alignment, column.Sortable.ToString().ToLower(), (column.Format != null && column.Format != "") ? ",formatter:formatValue,format:'" + column.Format + "'" : ""));
                }
                options.Add(string.Format("columns:[{0}]", string.Join(",", columns)));
                options.Add(string.Format("cacheRelationText:{0}", CacheRelationText.ToString().ToLower()));
                if (!string.IsNullOrEmpty(OnSelect))
                {
                    options.Add(string.Format("onSelect:{0}", OnSelect));
                }
                if (!string.IsNullOrEmpty(OnBeforeLoad))
                {
                    options.Add(string.Format("onBeforeLoad:{0}", OnBeforeLoad));
                }
                options.Add(string.Format("multiple:{0}", MultiSelect.ToString().ToLower()));

                return string.Join(",", options);
            }
        }

        internal bool DesignFlag = false;
        private Dictionary<string, string> columnCaptions;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        Dictionary<string, string> IColumnCaptions.ColumnCaptions
        {
            get
            {
                if (this.DesignMode || DesignFlag)
                {
                    if (string.IsNullOrEmpty(RemoteName) || string.IsNullOrEmpty(DataMember))
                    {
                        return null;
                    }
                    if (columnCaptions == null)
                    {
                        columnCaptions = new Dictionary<string, string>();
                        var assemblyName = RemoteName.Split('.')[0];
                        var commandName = RemoteName.Split('.')[1];
                        var clientInfo = EFClientTools.DesignClientUtility.ClientInfo;
                        clientInfo.UseDataSet = true;
                        var columnDefinations = EFClientTools.DesignClientUtility.Client.GetColumnDefination(clientInfo, assemblyName, DataMember, null)
                            .OfType<EFClientTools.EFServerReference.COLDEF>();
                        foreach (var columnDefination in columnDefinations)
                        {
                            columnCaptions.Add(columnDefination.FIELD_NAME
                                , string.IsNullOrEmpty(columnDefination.CAPTION) ? columnDefination.FIELD_NAME : columnDefination.CAPTION);
                        }
                    }
                }
                return columnCaptions;
            }
        }
    }
}
