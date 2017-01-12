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
    public class JQRefval : WebControl, IJQDataSourceProvider, IColumnCaptions
    {
        public JQRefval()
        {
            columns = new JQCollection<JQGridColumn>(this);
            columnMatches = new JQCollection<JQColumnMatch>(this);
            whereItems = new JQCollection<JQWhereItem>(this);
            DialogTitle = "JQRefval";
            CacheRelationText = true;
            CheckData = true;
            ShowValueAndText = false;
            //DialogWidth = 450;
            CapsLock = CapsLockEnum.None;
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

        private JQCollection<JQColumnMatch> columnMatches;
        [Category("Infolight")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [NotifyParentProperty(true)]
        public JQCollection<JQColumnMatch> ColumnMatches
        {
            get
            {
                return columnMatches;
            }
        }

        private JQCollection<JQWhereItem> whereItems;
        [Category("Infolight")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [NotifyParentProperty(true)]
        public JQCollection<JQWhereItem> WhereItems
        {
            get
            {
                return whereItems;
            }
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
        [Browsable(false)]
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
        [Browsable(false)]
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

        [Category("Infolight")]
        public bool CheckData { get; set; }

        [Category("Infolight")]
        public bool FixTextbox { get; set; }

        [Category("Infolight")]
        public bool CacheRelationText { get; set; }

        //[Category("Infolight")]
        //public int DialogWidth { get; set; }

        [Category("Infolight")]
        public string DialogTitle { get; set; }

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

        [Category("Infolight")]
        public string OnSelect { get; set; }
        [Category("Infolight")]
        public string OnFilter { get; set; }

        [Category("Infolight")]
        public bool SelectOnly { get; set; }

        [Category("Infolight")]
        public bool ShowValueAndText { get; set; }
        [Category("Infolight")]
        public bool DialogCenter { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object ParentObject { get; set; }

        //[Browsable(false)]
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        //public string FieldName { get; set; }

        //[Browsable(false)]
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        //public string Form { get; set; }

        //[Browsable(false)]
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        //public string Default { get; set; }

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
                        else if (pname == "onSelect")
                        {
                            this.OnSelect = pvalue;
                        }
                        else if (pname == "onFilter")
                        {
                            this.OnFilter = pvalue;
                        }
                        else if (pname == "selectOnly")
                        {
                            bool vselectonly = false;
                            Boolean.TryParse(pvalue, out vselectonly);
                            this.SelectOnly = vselectonly;
                        }
                        else if (pname == "cacheRelationText")
                        {
                            bool vcacheRelationText = true;
                            Boolean.TryParse(pvalue, out vcacheRelationText);
                            this.CacheRelationText = vcacheRelationText;
                        }
                        else if (pname == "checkData")
                        {
                            bool vcheckdata = true;
                            Boolean.TryParse(pvalue, out vcheckdata);
                            this.CheckData = vcheckdata;
                        }
                        else if (pname == "showValueAndText")
                        {
                            bool vshowValueAndText = true;
                            Boolean.TryParse(pvalue, out vshowValueAndText);
                            this.ShowValueAndText = vshowValueAndText;
                        }
                        else if (pname == "dialogCenter")
                        {
                            bool vsdialogCenter = true;
                            Boolean.TryParse(pvalue, out vsdialogCenter);
                            this.DialogCenter = vsdialogCenter;
                        }
                        if (pname == "capsLock")
                        {
                            this.CapsLock = (CapsLockEnum)Enum.Parse(typeof(CapsLockEnum), pvalue, true);
                        }
                        if (pname == "fixTextbox")
                        {
                            this.FixTextbox = bool.Parse(pvalue);
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
                                            else if (cpname == "table")
                                            {
                                                column.TableName = cpvalue;
                                            }
                                            else if (cpname == "isNvarChar")
                                            {
                                                column.IsNvarChar = cpvalue == "true";
                                            }
                                            else if (cpname == "format")
                                            {
                                                column.Format = cpvalue;
                                            }
                                            else if (cpname == "queryCondition")
                                            {
                                                column.QueryCondition = cpvalue;
                                            }
                                        }
                                    }
                                    this.Columns.Add(column);
                                }
                            }
                        }
                        else if (pname == "columnMatches")
                        {
                            var columnMatches = pvalue.Trim('[', ']');
                            var matches = Regex.Matches(columnMatches, @"(?<=\{).*?(?=\})");
                            if (matches.Count > 0)
                            {

                                foreach (Match match in matches)
                                {
                                    var columnMatch = new JQColumnMatch();
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
                                                columnMatch.TargetFieldName = cpvalue;
                                            }
                                            else if (cpname == "value")
                                            {
                                                if (cpvalue.StartsWith("remote["))
                                                {
                                                    columnMatch.RemoteMethod = true;
                                                    columnMatch.SourceMethod = cpvalue.Replace("remote[", "").Replace("]", "");
                                                }
                                                else if (cpvalue.StartsWith("client["))
                                                {
                                                    columnMatch.RemoteMethod = false;
                                                    columnMatch.SourceMethod = cpvalue.Replace("client[", "").Replace("]", "");
                                                }
                                                else
                                                {
                                                    columnMatch.SourceFieldName = cpvalue;
                                                }
                                            }
                                        }
                                    }
                                    this.ColumnMatches.Add(columnMatch);
                                }

                            }
                        }
                        else if (pname == "whereItems")
                        {
                            var whereItems = pvalue.Trim('[', ']');
                            var matches = Regex.Matches(whereItems, @"(?<=\{).*?(?=\})");
                            if (matches.Count > 0)
                            {

                                foreach (Match match in matches)
                                {
                                    var whereItem = new JQWhereItem();
                                    var columnOptions = match.Value.Split(',');
                                    var columnOp = "";
                                    foreach (var cop in columnOptions)
                                    {
                                        if (columnOp.Length > 0)
                                        {
                                            columnOp += ',';
                                        }
                                        columnOp += cop;
                                        if (columnOp.Split('\'').Length % 2 == 0)
                                        {
                                            continue;
                                        }


                                        var cparts = columnOp.Split(':');
                                        if (cparts.Length == 2)
                                        {
                                            var cpname = cparts[0].Trim();
                                            var cpvalue = cparts[1].Trim('\'');
                                            if (cpname == "field")
                                            {
                                                whereItem.FieldName = cpvalue;
                                            }
                                            else if (cpname == "value")
                                            {
                                                if (cpvalue.StartsWith("remote["))
                                                {
                                                    whereItem.RemoteMethod = true;
                                                    cpvalue = cpvalue.Replace("remote[", "").Replace("]", "");
                                                    if (cpvalue.StartsWith("_"))
                                                    {
                                                        whereItem.WhereValue = cpvalue;
                                                    }
                                                    else
                                                    {
                                                        whereItem.WhereMethod = cpvalue;
                                                    }
                                                }
                                                else if (cpvalue.StartsWith("client["))
                                                {
                                                    whereItem.RemoteMethod = false;
                                                    whereItem.WhereMethod = cpvalue.Replace("client[", "").Replace("]", "");
                                                }
                                                else if (cpvalue.StartsWith("row["))
                                                {
                                                    whereItem.RemoteMethod = false;
                                                    whereItem.WhereField = cpvalue.Replace("row[", "").Replace("]", "");
                                                }
                                                else
                                                {
                                                    whereItem.WhereValue = cpvalue;
                                                }
                                            }
                                        }
                                        columnOp = string.Empty;
                                    }
                                    this.WhereItems.Add(whereItem);
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
                throw new JQProperyNullException(this.ID, typeof(JQRefval), "RemoteName");
            }
            if (string.IsNullOrEmpty(DisplayMember))
            {
                throw new JQProperyNullException(this.ID, typeof(JQRefval), "DisplayMember");
            }
            if (string.IsNullOrEmpty(ValueMember))
            {
                throw new JQProperyNullException(this.ID, typeof(JQRefval), "ValueMember");
            }
            foreach (var column in this.Columns)
            {
                column.CheckProperties();
            }
            foreach (var column in this.ColumnMatches)
            {
                column.CheckProperties();
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (!this.DesignMode)
            {
                EFClientTools.ClientUtility.ClientInfo.cErrorCode = 8310;
                CheckProperties();
                if (this.Columns.Count == 0)
                {
                    var clientInfo = (EFClientTools.EFServerReference.ClientInfo)System.Web.HttpContext.Current.Session["ClientInfo"];
                    if (clientInfo != null && clientInfo.LogonResult == EFClientTools.EFServerReference.LogonResult.Logoned)
                    {
                        var assemblyName = RemoteName.Split('.')[0];
                        //get dd
                        var columnDefinations = EFClientTools.ClientUtility.Client.GetColumnDefination(EFClientTools.ClientUtility.ClientInfo, assemblyName, DataMember, null)
                             .OfType<EFClientTools.EFServerReference.COLDEF>();
                        var caption = this.ValueMember;
                        var column = columnDefinations.FirstOrDefault(c => c.FIELD_NAME == this.ValueMember);
                        if (column != null && !string.IsNullOrEmpty(column.CAPTION))
                        {
                            caption = column.CAPTION;
                        }

                        this.Columns.Add(new JQGridColumn() { FieldName = this.ValueMember, Caption = caption });

                        caption = this.DisplayMember;
                        column = columnDefinations.FirstOrDefault(c => c.FIELD_NAME == this.DisplayMember);
                        if (column != null && !string.IsNullOrEmpty(column.CAPTION))
                        {
                            caption = column.CAPTION;
                        }
                        this.Columns.Add(new JQGridColumn() { FieldName = this.DisplayMember, Caption = caption });
                    }
                }


                writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ID);
                writer.AddAttribute(HtmlTextWriterAttribute.Class, JQControl.RefValBox);
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
                //var optionBuilder = new StringBuilder();
                //optionBuilder.AppendFormat("remoteName:'{0}'", RemoteName);
                //optionBuilder.Append(",");
                //optionBuilder.AppendFormat("tableName:'{0}'", DataMember);
                //optionBuilder.Append(",");
                //var columnBuilder = new StringBuilder();
                //foreach (var column in Columns)
                //{
                //    columnBuilder.Append("{");
                //    columnBuilder.AppendFormat("field:'{0}'", column.FieldName);
                //    columnBuilder.AppendFormat("title:'{0}'", column.Caption);
                //    columnBuilder.AppendFormat("width:{0}", column.Width);
                //    columnBuilder.AppendFormat("align:'{0}'", column.Alignment);
                //    columnBuilder.Append("}");
                //}
                //optionBuilder.AppendFormat("columns:[{0}]", columnBuilder);
                //optionBuilder.Append(",");
                //var columnMatchBuilder = new StringBuilder();
                //foreach (var columnMatch in ColumnMatches)
                //{
                //    columnBuilder.Append("{");
                //    columnBuilder.AppendFormat("field:'{0}'", columnMatch.TargetFieldName);
                //    columnBuilder.AppendFormat("value:'{0}'", columnMatch.Value);
                //    columnBuilder.Append("}");
                //}
                //optionBuilder.AppendFormat("columnMatches:[{0}]", columnMatchBuilder);
                //optionBuilder.Append(",");
                //optionBuilder.AppendFormat("valueField:'{0}'", ValueMember);
                //return optionBuilder.ToString();
                var options = new List<string>();
                options.Add(string.Format("title:'{0}'", DialogTitle));
                options.Add(string.Format("panelWidth:{0}", DialogWidth));
                if (DialogHeight > 0)
                {
                    options.Add(string.Format("panelHeight:{0}", DialogHeight));
                }
                options.Add(string.Format("remoteName:'{0}'", RemoteName));
                options.Add(string.Format("tableName:'{0}'", DataMember));
                var columns = new List<string>();
                foreach (var column in Columns)
                {
                    columns.Add(string.Format("{{field:'{0}',title:'{1}',width:{2},align:'{3}',table:'{4}',isNvarChar:{5},queryCondition:'{6}'{7}}}"
                        , column.FieldName, column.Caption, column.Width, column.Alignment, column.TableName, column.IsNvarChar.ToString().ToLower()
                        , column.QueryCondition, (column.Format != null && column.Format != "") ? ",formatter:formatValue,format:'" + column.Format + "'" : ""));
                }
                options.Add(string.Format("columns:[{0}]", string.Join(",", columns)));
                var columnMatches = new List<string>();
                foreach (var columnMatch in ColumnMatches)
                {
                    columnMatches.Add(string.Format("{{field:'{0}',value:'{1}'}}", columnMatch.TargetFieldName, columnMatch.Value));
                }
                options.Add(string.Format("columnMatches:[{0}]", string.Join(",", columnMatches)));

                var whereIterms = new List<string>();
                foreach (var whereItem in WhereItems)
                {
                    whereIterms.Add(string.Format("{{field:'{0}',value:'{1}'}}", whereItem.FieldName, whereItem.Value));
                }
                options.Add(string.Format("whereItems:[{0}]", string.Join(",", whereIterms)));

                options.Add(string.Format("valueField:'{0}'", ValueMember));
                options.Add(string.Format("textField:'{0}'", DisplayMember));
                options.Add(string.Format("valueFieldCaption:'{0}'", ValueMemberCaption));
                options.Add(string.Format("textFieldCaption:'{0}'", DisplayMemberCaption));
                options.Add(string.Format("cacheRelationText:{0}", CacheRelationText.ToString().ToLower()));
                options.Add(string.Format("checkData:{0}", CheckData.ToString().ToLower()));
                options.Add(string.Format("showValueAndText:{0}", ShowValueAndText.ToString().ToLower()));
                options.Add(string.Format("dialogCenter:{0}", DialogCenter.ToString().ToLower()));
                if (!string.IsNullOrEmpty(OnSelect))
                {
                    options.Add(string.Format("onSelect:{0}", OnSelect));
                }
                if (!string.IsNullOrEmpty(OnFilter))
                {
                    options.Add(string.Format("onFilter:{0}", OnFilter));
                }
                options.Add(string.Format("selectOnly:{0}", SelectOnly.ToString().ToLower()));
                options.Add(string.Format("capsLock:'{0}'", CapsLock.ToString().ToLower()));
                options.Add(string.Format("fixTextbox:'{0}'", FixTextbox.ToString().ToLower()));
                return string.Join(",", options);
            }
        }


        //protected override void Render(HtmlTextWriter writer)
        //{
        //    if (!this.DesignMode)
        //    {
        //        writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ID);
        //        writer.AddAttribute(JQProperty.InfolightOptions, this.InfolightOptions);
        //        writer.RenderBeginTag(HtmlTextWriterTag.Span);

        //        writer.AddAttribute(HtmlTextWriterAttribute.Name, this.FieldName);
        //        writer.AddAttribute(JQProperty.InfolightOptions, this.InputInfolightOptions);
        //        writer.RenderBeginTag(HtmlTextWriterTag.Input);
        //        writer.RenderEndTag();

        //        writer.AddAttribute(HtmlTextWriterAttribute.Class, JQIcon.View);
        //        writer.AddAttribute(HtmlTextWriterAttribute.Style, "vertical-align:sub; display:inline-block;width:16px;height:16px;cursor:pointer;");
        //        writer.AddAttribute(HtmlTextWriterAttribute.Onclick, "refButtonClick(this);");
        //        writer.RenderBeginTag(HtmlTextWriterTag.Span);
        //        writer.RenderEndTag();

        //        JQDialog dialog = new JQDialog() { ID = EditDialogID, Width = DialogWidth, Title = DialogTitle, Icon = JQIcon.Search };
        //        JQDataGrid grid = new JQDataGrid() { RemoteName = this.RemoteName, DataMember = this.DataMember, Title = string.Empty };
        //        foreach (var column in this.Columns)
        //        {
        //            grid.Columns.Add(new JQGridColumn() { FieldName = column.FieldName, Caption = column.Caption, Width = column.Width });
        //        }

        //        //dialog.Controls.Add(grid);
        //        //dialog.RenderControl(writer);

        //        dialog.RenderBeginTag(writer);
        //        grid.RenderControl(writer);
        //        dialog.RenderEndTag(writer);

        //        writer.RenderEndTag();
        //    }
        //}

        //[Browsable(false)]
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        //private string InfolightOptions
        //{
        //    get
        //    {
        //        var optionBuilder = new StringBuilder();
        //        //optionBuilder.AppendFormat("remoteName:'{0}'", RemoteName);
        //        //optionBuilder.Append(",");
        //        //optionBuilder.AppendFormat("tableName:'{0}'", DataMember);
        //        //optionBuilder.Append(",");
        //        //var columnBuilder = new StringBuilder();
        //        //var captionBuilder = new StringBuilder();
        //        //var widthBuilder = new StringBuilder();
        //        //foreach (var column in Columns)
        //        //{
        //        //    if (columnBuilder.Length > 0)
        //        //    {
        //        //        columnBuilder.Append(";");
        //        //    }
        //        //    if (captionBuilder.Length > 0)
        //        //    {
        //        //        captionBuilder.Append(";");
        //        //    }
        //        //    if (widthBuilder.Length > 0)
        //        //    {
        //        //        widthBuilder.Append(";");
        //        //    }
        //        //    columnBuilder.Append(column.FieldName);
        //        //    captionBuilder.Append(column.Caption);
        //        //    widthBuilder.Append(column.Width);
        //        //}
        //        //optionBuilder.AppendFormat("fields:'{0}'", columnBuilder);
        //        //optionBuilder.Append(",");
        //        //optionBuilder.AppendFormat("captions:'{0}'", captionBuilder);
        //        //optionBuilder.Append(",");
        //        //optionBuilder.AppendFormat("widths:'{0}'", widthBuilder);
        //        //optionBuilder.Append(",");
        //        optionBuilder.AppendFormat("editDialog:'#{0}'", EditDialogID);
        //        optionBuilder.Append(",");
        //        var columnMatchBuilder = new StringBuilder();
        //        foreach (var columnMatch in ColumnMatches)
        //        {
        //            if (columnMatchBuilder.Length > 0)
        //            {
        //                columnMatchBuilder.Append(";");
        //            }
        //            columnMatchBuilder.AppendFormat("{0}={1}", columnMatch.TargetFieldName, columnMatch.Value);
        //        }
        //        optionBuilder.AppendFormat("columnMatches:'{0}'", columnMatchBuilder);
        //        optionBuilder.Append(",");
        //        optionBuilder.AppendFormat("valueField:'{0}'", ValueMember);
        //        return optionBuilder.ToString();
        //    }
        //}

        //[Browsable(false)]
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        //private string InputInfolightOptions
        //{
        //    get
        //    {
        //        var optionBuilder = new StringBuilder();
        //        optionBuilder.AppendFormat("field:'{0}'", FieldName);
        //        optionBuilder.Append(",");
        //        if (!string.IsNullOrEmpty(Default))
        //        {
        //            optionBuilder.AppendFormat("defaultValue:'{0}'", Default);
        //            optionBuilder.Append(",");
        //        }
        //        optionBuilder.AppendFormat("form:'{0}'", Form);
        //        return optionBuilder.ToString();
        //    }
        //}

        //[Browsable(false)]
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        //public string EditDialogID 
        //{
        //    get
        //    {
        //        return string.Format("edit{0}", this.ID);
        //    }
        //}

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


    public class JQColumnMatch : JQCollectionItem, IJQDataSourceProvider
    {
        public JQColumnMatch()
        {
            RemoteMethod = true;
        }

        /// <summary>
        /// 栏位名
        /// </summary>
        [Category("Infolight")]
        [Editor(typeof(FieldEditor), typeof(UITypeEditor))]
        public string SourceFieldName { get; set; }

        /// <summary>
        /// 栏位名
        /// </summary>
        [Category("Infolight")]
        [Editor(typeof(ParentFieldEditor), typeof(UITypeEditor))]
        public string TargetFieldName { get; set; }


        /// <summary>
        /// 方法
        /// </summary>
        [Category("Infolight")]
        public string SourceMethod { get; set; }

        /// <summary>
        /// 是否后台方法
        /// </summary>
        [Category("Infolight")]
        public bool RemoteMethod { get; set; }


        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Value
        {
            get
            {
                //var valueBuilder = new StringBuilder();
                //if (!string.IsNullOrEmpty(this.SourceFieldName))
                //{
                //    valueBuilder.Append(SourceFieldName);
                //}
                //else
                //{
                //    if (RemoteMethod)
                //    {
                //        valueBuilder.AppendFormat("remote[{0}]", SourceMethod);
                //    }
                //    else
                //    {
                //        valueBuilder.AppendFormat("client[{0}]", SourceMethod);
                //    }
                //}
                //return valueBuilder.ToString().TrimEnd(',');
                var values = new List<string>();
                if (!string.IsNullOrEmpty(this.SourceFieldName))
                {
                    values.Add(SourceFieldName);
                }
                else
                {
                    if (RemoteMethod)
                    {
                        values.Add(string.Format("remote[{0}]", SourceMethod));
                    }
                    else
                    {
                        values.Add(string.Format("client[{0}]", SourceMethod));
                    }
                }
                return string.Join(",", values);
            }
        }

        internal void CheckProperties()
        {
            if (string.IsNullOrEmpty(this.TargetFieldName))
            {
                var controlID = ((this as IJQProperty).ParentProperty.Component as Control).ID;
                throw new JQProperyNullException(controlID, typeof(JQRefval), "ColumnMatches.TargetFieldName");
            }
        }

        public override string ToString()
        {
            if (!string.IsNullOrEmpty(TargetFieldName))
            {
                return this.TargetFieldName;
            }
            else
            {
                return base.ToString();
            }
        }

        #region IJQDataSourceProvider Members

        string IJQDataSourceProvider.RemoteName
        {
            get
            {
                return ((this as IJQProperty).ParentProperty.Component as IJQDataSourceProvider).RemoteName;
            }
            set { }
        }

        string IJQDataSourceProvider.DataMember
        {
            get
            {
                return ((this as IJQProperty).ParentProperty.Component as IJQDataSourceProvider).DataMember;
            }
            set { }
        }

        #endregion
    }

    public class JQWhereItem : JQCollectionItem, IJQDataSourceProvider
    {
        [Category("Infolight")]
        [Editor(typeof(FieldEditor), typeof(UITypeEditor))]
        public string FieldName { get; set; }

        [Category("Infolight")]
        public string WhereValue { get; set; }

        [Category("Infolight")]
        public string WhereMethod { get; set; }

        [Category("Infolight")]
        public bool RemoteMethod { get; set; }

        [Category("Infolight")]
        [Editor(typeof(ParentFieldEditor), typeof(UITypeEditor))]
        public string WhereField { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Value
        {
            get
            {
                var values = new List<string>();
                if (!string.IsNullOrEmpty(this.WhereValue))
                {
                    if (this.WhereValue.StartsWith("_"))
                    {
                        values.Add(string.Format("remote[{0}]", WhereValue));
                    }
                    else
                    {
                        values.Add(WhereValue.Replace("'", "\'"));
                    }
                }
                else if (!string.IsNullOrEmpty(WhereField))
                {
                    values.Add(string.Format("row[{0}]", WhereField));
                }
                else if (!string.IsNullOrEmpty(WhereMethod))
                {
                    if (RemoteMethod)
                    {
                        values.Add(string.Format("remote[{0}]", WhereMethod));
                    }
                    else
                    {
                        values.Add(string.Format("client[{0}]", WhereMethod));
                    }
                }
                return string.Join(",", values);
            }
        }

        #region IJQDataSourceProvider Members

        string IJQDataSourceProvider.RemoteName
        {
            get
            {
                return ((this as IJQProperty).ParentProperty.Component as IJQDataSourceProvider).RemoteName;
            }
            set { }
        }

        string IJQDataSourceProvider.DataMember
        {
            get
            {
                return ((this as IJQProperty).ParentProperty.Component as IJQDataSourceProvider).DataMember;
            }
            set { }
        }

        #endregion
    }
}
