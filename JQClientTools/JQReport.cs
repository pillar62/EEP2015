using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Drawing.Design;
using System.Web.UI;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.Design;
using System.ComponentModel.Design;

namespace JQClientTools
{
    [Designer(typeof(ControlDesigner), typeof(IDesigner))]
    public class JQReport : WebControl, IJQDataSourceProvider
    {
        public JQReport()
        {
            parameters = new JQCollection<JQReportParameter>(this);
            headerItems = new JQCollection<JQReportItem>(this);
            fieldItems = new JQCollection<JQReportFieldItem>(this);
            footerItems = new JQCollection<JQReportItem>(this);
            queryColumns = new JQCollection<JQQueryColumn>(this);
            //FieldFont = new System.Drawing.Font("Simsun", 9.0f);
            DetailFont = new System.Drawing.Font("Simsun", 10.0f);
            DetailHeaderFont = new System.Drawing.Font("Simsun", 12.0f);
            HeaderFont = new System.Drawing.Font("Simsun", 12.0f);
            HeaderBorderWidth = 1;         
            DetailBorderWidth = 1;
            FooterBorderWidth = 1;
            LabelCaptionWidth = 100;
            LabelHorizontalGap = 50;
            DataSetName = "NewDataSet";
            HeaderDataSetName = "NewDataSetH";
        }


        //[Category("Infolight")]
        //[Editor(typeof(RemoteNameEditor), typeof(UITypeEditor))]
        //public string HeaderRemoteName { get; set; }


        [Category("Infolight")]
        public ReportStyle ReportStyle { get; set; }

        [Category("Infolight")]
        [Editor(typeof(RemoteNameEditor), typeof(UITypeEditor))]
        public string RemoteName { get; set; }

        private string dataMember;
        /// <summary>
        /// 表名
        /// </summary>
        [Category("Infolight")]
        [Editor(typeof(DataMemberEditor), typeof(UITypeEditor))]
        public string DataMember
        {
            get
            {
                return dataMember;
            }
            set
            {
                dataMember = value;
            }
        }

        private string headerDataMember;
        /// <summary>
        /// 表名
        /// </summary>
        [Category("Infolight")]
        [Editor(typeof(DataMemberEditor), typeof(UITypeEditor))]
        public string HeaderDataMember
        {
            get
            {
                return headerDataMember;
            }
            set
            {
                headerDataMember = value;
            }
        }

        [Category("Infolight")]
        public string RDLCFileNames
        {
            get;
            set;
        }

        [Category("Infolight")]
        public string HeaderDataSetName
        {
            get;
            set;
        }


        [Category("Infolight")]
        public string DataSetName
        {
            get;
            set;
        }


        [Category("Infolight")]
        public string ReportID { get; set; }
        [Category("Infolight")]
        public string ReportName { get; set; }
        [Category("Infolight")]
        public string Description { get; set; }

       
        //[Category("Infolight")]
        //public System.Drawing.Font FieldFont { get; set; }
        [Category("Infolight")]
        public System.Drawing.Font HeaderFont { get; set; }
        [Category("Infolight")]
        public int HeaderBorderWidth { get; set; }

        [Category("Infolight")]
        public System.Drawing.Font DetailHeaderFont { get; set; }
        [Category("Infolight")]
        public System.Drawing.Font DetailFont { get; set; }
        [Category("Infolight")]
        public int DetailBorderWidth { get; set; }
        [Category("Infolight")]
        public int FooterBorderWidth { get; set; }

        [Category("Infolight")]
        public int DetailCount { get; set; }
        [Category("Infolight")]
        public int LabelCaptionWidth { get; set; }
        [Category("Infolight")]
        public int LabelHorizontalGap { get; set; }

        [Category("Infolight")]
        public GroupStyle GroupStyle { get; set; }

        [Category("Infolight")]
        public LabelStyle LabelStyle { get; set; }

        [Category("Infolight")]
        public string TotalCaption { get; set; }




        private JQCollection<JQReportParameter> parameters;
        [Category("Infolight")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [NotifyParentProperty(true)]
        public JQCollection<JQReportParameter> Parameters
        {
            get
            {
                return parameters;
            }
        }

        private JQCollection<JQReportItem> headerItems;
        [Category("Infolight")]
        [Browsable(false)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [NotifyParentProperty(true)]
        public JQCollection<JQReportItem> HeaderItems
        {
            get
            {
                return headerItems;
            }
        }

        private JQCollection<JQReportFieldItem> fieldItems;
        [Category("Infolight")]
        [Browsable(false)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [NotifyParentProperty(true)]
        public JQCollection<JQReportFieldItem> FieldItems
        {
            get
            {
                return fieldItems;
            }
        }

        private JQCollection<JQReportItem> footerItems;
        [Category("Infolight")]
        [Browsable(false)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [NotifyParentProperty(true)]
        public JQCollection<JQReportItem> FooterItems
        {
            get
            {
                return footerItems;
            }
        }

        private JQCollection<JQQueryColumn> queryColumns;
        [Category("Infolight")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [NotifyParentProperty(true)]
        public JQCollection<JQQueryColumn> QueryColumns
        {
            get
            {
                return queryColumns;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string QueryDialogID
        {
            get
            {
                var id = string.IsNullOrEmpty(this.ID) ? "JQReport" : this.ID;
                return string.Format("query{0}", id);
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!this.DesignMode)
            {
                var id = string.IsNullOrEmpty(this.ID) ? "JQReport" : this.ID;
              
                var script = new StringBuilder();
                script.AppendFormat("$(function(){{ var height = $(window).height();$('#{0}').height(height - 20);$('#{0}').layout('resize');", id);
                if (this.QueryColumns.Count == 0 && string.IsNullOrEmpty(RDLCFileNames))
                {
                    var name = System.IO.Path.GetFileNameWithoutExtension(this.Page.Request.Path);
                    var paths = this.Page.Request.Path.Split('/');
                    var directory = paths[paths.Length - 2];
                    script.AppendFormat("var panel = $('#{0}').layout('panel', 'center'); panel.children().remove();"
                    + "$('<iframe style=\"border: 0px;\" src=\"../ReportViewerTemplate.aspx?{1}&WhereString=&WhereTextString=&ReportPath={2}/{3}.rdlc\" width=\"100%\" height=\"100%\"></iframe>').appendTo(panel);"
                     , id, CreateParameters() , directory, name); 
                }
                script.Append("})");
                //var script = string.Format("$(function(){{ var height = $(window).height();$('#{0}').height(height - 20);$('#{0}').layout('resize');}})", id);
                
                var literal = new LiteralControl();
                literal.Text = string.Format("<script>{0}</script>", script);
                this.Page.Header.Controls.Add(literal);
            }
        }

        private string CreateParameters()
        {
            var parameters = new List<string>();
            parameters.Add(string.Format("RemoteName={0}", this.RemoteName));
            parameters.Add(string.Format("TableName={0}", this.DataMember));
            parameters.Add(string.Format("DataSetName={0}", this.DataSetName));
            if (!string.IsNullOrEmpty(this.HeaderDataMember))
            {
                parameters.Add(string.Format("HeaderTableName={0}", this.HeaderDataMember));
                parameters.Add(string.Format("HeaderDataSetName={0}", this.HeaderDataSetName));
            }
            foreach (var p in this.Parameters)
            {
                if (!string.IsNullOrEmpty(p.Name))
                {
                    parameters.Add(string.Format("RP{0}={1}", p.Name, p.Value));
                }
            }
            return string.Join("&", parameters);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (!this.DesignMode)
            {
                EFClientTools.ClientUtility.ClientInfo.cErrorCode = 8320;
                var id = string.IsNullOrEmpty(this.ID) ? "JQReport" : this.ID;


                writer.AddAttribute(HtmlTextWriterAttribute.Id, id);
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "easyui-layout");
                writer.AddAttribute(HtmlTextWriterAttribute.Style, "width:100%;");
                writer.AddAttribute(JQProperty.InfolightOptions, InfolightOptions);
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                if (this.QueryColumns.Count > 0 || (!string.IsNullOrEmpty(RDLCFileNames)))
                {
                    int rowCount = 1;
                    foreach (var column in QueryColumns)
                    {
                        if (column.NewLine && QueryColumns.IndexOf(column) > 0)
                        {
                            rowCount += 1;
                        }
                    }
                    if(!string.IsNullOrEmpty(RDLCFileNames))
                    {
                        rowCount += 1;
                    }

                    writer.AddAttribute(HtmlTextWriterAttribute.Id, this.QueryDialogID);
                    writer.AddAttribute(JQProperty.DataOptions, "region:'north',title:'Query',split:true");
                    var height = (QueryColumns.Count > 0 && QueryColumns[0].NewLine)? 70 + rowCount * 23 : 45 + rowCount * 23;
                    writer.AddAttribute(HtmlTextWriterAttribute.Style, string.Format("height:{0}px;", height));


                    writer.RenderBeginTag(HtmlTextWriterTag.Div);
                    writer.RenderBeginTag(HtmlTextWriterTag.Table);

                    if (!string.IsNullOrEmpty(RDLCFileNames))
                    {
                        writer.RenderBeginTag(HtmlTextWriterTag.Tr);
                        writer.RenderBeginTag(HtmlTextWriterTag.Td);
                        writer.Write("Select Report:");
                        writer.RenderEndTag();//td
                        writer.RenderBeginTag(HtmlTextWriterTag.Td);

                        writer.AddAttribute(HtmlTextWriterAttribute.Id, "comboReport");
                        writer.AddAttribute(HtmlTextWriterAttribute.Name, "comboReport");
                        writer.AddAttribute(HtmlTextWriterAttribute.Class, "easyui-combobox");
                        writer.AddAttribute(JQProperty.DataOptions, "panelHeight:'auto',editable:false");
                        writer.AddStyleAttribute(HtmlTextWriterStyle.Width, "150px");
                        writer.RenderBeginTag(HtmlTextWriterTag.Select);
                        foreach (var item in RDLCFileNames.Split(';'))
                        {
                            writer.Write("<option value=\"" + item + "\">" + item + "</option>");
                        }
                        writer.RenderEndTag();//select


                        writer.RenderEndTag();//td
                        writer.RenderEndTag();//tr
                    }


                    writer.RenderBeginTag(HtmlTextWriterTag.Tr);
                    var maxspan = 0;
                    var colspan = 0;
                    foreach (var column in QueryColumns)
                    {
                        if (column.NewLine && QueryColumns.IndexOf(column) > 0)
                        {
                            writer.RenderEndTag();
                            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
                            maxspan = Math.Max(maxspan, colspan);
                            colspan = 0;
                        }

                        column.Render(writer);
                        colspan += 2;
                    }
                    maxspan = Math.Max(maxspan, colspan);

                    if (QueryColumns.Count > 0 )
                    {
                        if (QueryColumns[0].NewLine)
                        {
                            writer.RenderEndTag();//tr

                            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
                        }
                        else
                        {
                            maxspan = 1;
                        }
                    }
                    else
                    {
                        maxspan = 2;
                    }
                  
                    writer.AddAttribute(HtmlTextWriterAttribute.Align, "right");
                    writer.AddAttribute(HtmlTextWriterAttribute.Colspan, maxspan.ToString());
                    writer.RenderBeginTag(HtmlTextWriterTag.Td);
                    writer.AddAttribute(HtmlTextWriterAttribute.Href, "javascript:void(0)");
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, JQControl.LinkButton + " infosysbutton-q");
                    writer.AddAttribute(JQProperty.DataOptions, "iconCls:'icon-search'");

                    var name = System.IO.Path.GetFileNameWithoutExtension(this.Page.Request.Path);
                    var paths = this.Page.Request.Path.Split('/');
                    var directory = paths[paths.Length - 2];
                    var script = new StringBuilder();
                    script.AppendFormat("var where = $.fn.datagrid.methods.getWhere($('#{0}')); var whereText = $.fn.datagrid.methods.getWhereText($('#{0}'));var panel = $('#{0}').layout('panel', 'center'); panel.children().remove();", id);
                    if (string.IsNullOrEmpty(RDLCFileNames))
                    {
                        script.AppendFormat("$('<iframe style=\"border: 0px;\" src=\"../ReportViewerTemplate.aspx?{0}&WhereString=' + encodeURIComponent(where) + '&WhereTextString=' + encodeURIComponent(whereText) + '&ReportPath={1}/{2}.rdlc\" width=\"100%\" height=\"100%\"></iframe>').appendTo(panel);"
                            , CreateParameters(), directory, name);
                    }
                    else
                    {
                        script.Append("var reportName = $('#comboReport').combobox('getValue');");
                        script.AppendFormat("$('<iframe style=\"border: 0px;\" src=\"../ReportViewerTemplate.aspx?{0}&WhereString=' + encodeURIComponent(where) + '&WhereTextString=' + encodeURIComponent(whereText) + '&ReportPath={1}/RDLC/' + reportName + '.rdlc\" width=\"100%\" height=\"100%\"></iframe>').appendTo(panel);"
                            , CreateParameters(), directory); 
                    }

                    ////var script = string.Format("var where = $.fn.datagrid.methods.getWhere($('#{0}')); var whereText = $.fn.datagrid.methods.getWhereText($('#{0}'));var panel = $('#{0}').layout('panel', 'center'); panel.children().remove();"
                    //+ "$('<iframe style=\"border: 0px;\" src=\"../preview/ReportViewerTemplate.aspx?RemoteName={1}&WhereString=' + encodeURIComponent(where) + '&WhereTextString=' + encodeURIComponent(whereText) + '&ReportPath={2}/{3}.rdlc\" width=\"100%\" height=\"100%\"></iframe>').appendTo(panel);"
                    //, id, this.RemoteName, directory, name); 

                    writer.AddAttribute(HtmlTextWriterAttribute.Onclick, script.ToString());
                    writer.RenderBeginTag(HtmlTextWriterTag.A);
                    writer.Write("Query");
                    writer.RenderEndTag();//a
                    writer.RenderEndTag();//td
                    writer.RenderEndTag();//tr

                    writer.RenderEndTag();//table
                    writer.RenderEndTag();
                }
                writer.AddAttribute(JQProperty.DataOptions, "region:'center'");
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                
                writer.RenderEndTag();

                writer.RenderEndTag();
                EFClientTools.ClientUtility.ClientInfo.cErrorCode = 0;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string InfolightOptions
        {
            get
            {
                var options = new List<string>();
                if (this.QueryColumns.Count > 0)
                {
                    options.Add(string.Format("queryDialog:'#{0}'", QueryDialogID));
                }
                return string.Join(",", options);
            }
        }

    }


    public class JQReportParameter
    {
        public string Name { get; set; }

        public string Value { get; set; }
    }

    public class JQReportItem
    {
        [Browsable(false)]
        public int RowIndex { get; set; }
        [Browsable(false)]
        public int ColumnIndex { get; set; }
        [Browsable(false)]
        public virtual string BackColor { get; set; }
        [Browsable(false)]
        public virtual string Color { get; set; }
        [Browsable(false)]
        public virtual System.Drawing.Font Font { get; set; }
        [Browsable(false)]
        public virtual int Cells { get; set; }
        [Browsable(false)]
        public virtual string Caption { get; set; }
        [Browsable(false)]
        public virtual string Format { get; set; }
        [Browsable(false)]
        public virtual string Alignment { get; set; }
        
    }

    public class JQReportFieldItem: JQReportItem
    {
        public JQReportFieldItem()
        {
            Font = new System.Drawing.Font("Simsun", 10.0f);
            HeaderFont = new System.Drawing.Font("Simsun", 12.0f);
            Width = 80;
            Color = "#000000";
            HeaderColor = "#000000";
            Cells = 1;
            KeepTogether = true;
            //Total = JQReportTotal.None;
        }

        [Category("Infolight")]
        public string Field { get; set; }
        [Category("Infolight")]
        public override string Caption { get; set; }

        [Category("Infolight")]
        public System.Drawing.Font HeaderFont { get; set; }

        [Category("Infolight")]
        [Editor(typeof(ColorEditor), typeof(UITypeEditor))]
        public string HeaderColor { get; set; }

        [Category("Infolight")]
        [Editor(typeof(JQReportAlignmentEditor), typeof(UITypeEditor))]
        public string HeaderAlignment { get; set; }

        [Category("Infolight")]
        public override System.Drawing.Font Font { get; set; }

        [Category("Infolight")]
        [Editor(typeof(ColorEditor), typeof(UITypeEditor))]
        public override string BackColor { get; set; }

        [Category("Infolight")]
        [Editor(typeof(ColorEditor), typeof(UITypeEditor))]
        public override string Color { get; set; }

        [Category("Infolight")]
        [Editor(typeof(JQReportAlignmentEditor), typeof(UITypeEditor))]
        public override string Alignment { get; set; }

        
        [Category("Infolight")]
        public override string Format { get; set; }

        [Category("Infolight")]
        [Editor(typeof(JQReportTotalEditor), typeof(UITypeEditor))]
        public string Total { get; set; }

        [Category("Infolight")]
        public string TotalFormat { get; set; }

        [Category("Infolight")]
        public int Width { get; set; }
        [Category("Infolight")]
        public bool Group { get; set; }

        [Category("Infolight")]
        public bool GroupTotal { get; set; }

        [Category("Infolight")]
        public string GroupTotalFormat { get; set; }

        [Category("Infolight")]
        public string Description { get; set; }

        [Category("Infolight")]
        public override int Cells { get; set; }

        [Category("Infolight")]
        public int LabelCaptionWidth { get; set; }
        [Category("Infolight")]
        public bool KeepTogether { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string GroupFor { get; set; }
    }

    public class JQReportConstantItem: JQReportItem
    {
        public JQReportConstantItem()
        {
            Font = new System.Drawing.Font("Simsun", 12.0f);
            Cells = 1;
            Color = "#000000";
        }

        public enum StyleType
        {
            /// <summary>
            /// description
            /// </summary>
            Description = 0,
            /// <summary>
            /// company name
            /// </summary>
            CompanyName = 1,
            /// <summary>
            /// page index
            /// </summary>
            PageIndex = 2,
            /// <summary>
            /// page index and total
            /// </summary>
            PageIndexAndTotal = 3,
            /// <summary>
            /// query condition
            /// </summary>
            QueryCondition = 4,
            /// <summary>
            /// report date
            /// </summary>
            ReportDate = 5,
            /// <summary>
            /// report date and time
            /// </summary>
            ReportDateTime = 6,
            /// <summary>
            /// report id
            /// </summary>
            ReportID = 7,
            /// <summary>
            /// report name
            /// </summary>
            ReportName = 8,
            /// <summary>
            /// user id
            /// </summary>
            UserID = 9,
            /// <summary>
            /// user name
            /// </summary>
            UserName = 10,
            /// <summary>
            /// Logo
            /// </summary>
            Logo = 11
        }

        [Category("Infolight")]
        public StyleType Style { get; set; }

        private string caption;
        [Category("Infolight")]
        public override string Caption 
        {
            get
            {
                if (caption == null)
                {
                    if (Style == StyleType.ReportName || Style == StyleType.CompanyName || Style == StyleType.Logo)
                    {
                        caption = "";
                    }
                    else
                    {
                        var provider = new JQMessageProvider(HttpContext.Current.Request.PhysicalApplicationPath, EFClientTools.ClientUtility.ClientInfo, EFClientTools.ClientUtility.ClientInfo.Locale);
                        //EFBase.MessageProvider provider = new EFBase.MessageProvider(HttpContext.Current.Request.PhysicalApplicationPath, EFClientTools.ClientUtility.ClientInfo.Locale);
                        var formats = provider["JQWebClient/ReportItems"];
                        if (!string.IsNullOrEmpty(formats))
                        {
                            caption = formats.Split(';')[(int)Style] + ":";
                        }
                    }
                }
                return caption;
            }
            set
            {
                caption = value;
            }
        }

        [Category("Infolight")]
        [Editor(typeof(ColorEditor), typeof(UITypeEditor))]
        public override string BackColor { get; set; }

        [Category("Infolight")]
        public override System.Drawing.Font Font { get; set; }

        [Category("Infolight")]
        [Editor(typeof(ColorEditor), typeof(UITypeEditor))]
        public override string Color { get; set; }

        [Category("Infolight")]
        [Editor(typeof(JQReportAlignmentEditor), typeof(UITypeEditor))]
        public override string Alignment { get; set; }

        private string format;
        [Category("Infolight")]
        public override string Format
        {
            get
            {
                //if (string.IsNullOrEmpty(format))
                //{
                //    if (Style == StyleType.ReportName || Style == StyleType.CompanyName || Style == StyleType.Logo)
                //    {
                //        format = "{0}";
                //    }
                //    else
                //    {
                //        var provider = new JQMessageProvider(HttpContext.Current.Request.PhysicalApplicationPath, EFClientTools.ClientUtility.ClientInfo, EFClientTools.ClientUtility.ClientInfo.Locale);
                //        //EFBase.MessageProvider provider = new EFBase.MessageProvider(HttpContext.Current.Request.PhysicalApplicationPath, EFClientTools.ClientUtility.ClientInfo.Locale);
                //        var formats = provider["JQWebClient/ReportItems"];
                //        if (!string.IsNullOrEmpty(formats))
                //        {
                //            format = formats.Split(';')[(int)Style] + ":{0}";
                //        }
                //    }
                //}
                return format;
            }
            set
            {
                format = value;
            }
        }

        [Category("Infolight")]
        public override int Cells { get; set; }
    }

    public enum ReportStyle
    {
        Report,
        Label
    }

    public enum GroupStyle
    {
        None,
        ChangePage,
        LineFeed
    }

    public enum LabelStyle
    { 
        None,
        ChangePage,
        LineFeed
    }
}
