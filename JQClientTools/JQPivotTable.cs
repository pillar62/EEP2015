using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Web.UI;
using System.Drawing.Design;
using System.Text.RegularExpressions;
using System.Web.UI.Design;
using System.ComponentModel.Design;
using System.Xml;
using Microsoft.Win32;

namespace JQClientTools
{
    public class JQPivotTable : WebControl, IJQDataSourceProvider, IColumnCaptions
    {
        public JQPivotTable()
        {
            PanelHeight = 200;
            columns = new JQCollection<JQPivotTableColumn>(this);
            rows = new JQCollection<JQPivotTableColumn>(this);
            showColumns = new JQCollection<JQPivotTableColumn>(this);
        }

        private string remoteName;
        /// <summary>
        /// 数据源
        /// </summary>
        [Category("Infolight")]
        [Editor(typeof(RemoteNameEditor), typeof(UITypeEditor))]
        public string RemoteName
        {
            get
            {
                return remoteName;
            }
            set
            {
                remoteName = value;
            }
        }

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


        [Category("Infolight")]
        public string OnBeforeLoad { get; set; }

        [Category("Infolight")]
        public int PanelHeight { get; set; }

        private JQCollection<JQPivotTableColumn> columns;
        [Category("Infolight")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [NotifyParentProperty(true)]
        public JQCollection<JQPivotTableColumn> Columns
        {
            get
            {
                return columns;
            }
        }
        private JQCollection<JQPivotTableColumn> rows;
        [Category("Infolight")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [NotifyParentProperty(true)]
        public JQCollection<JQPivotTableColumn> Rows
        {
            get
            {
                return rows;
            }
        }
        private JQCollection<JQPivotTableColumn> showColumns;
        [Category("Infolight")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [NotifyParentProperty(true)]
        public JQCollection<JQPivotTableColumn> ShowColumns
        {
            get
            {
                return showColumns;
            }
        }

        private String _PivotTableRenderers;
        [Category("Infolight")]
        [Editor(typeof(PivotTableRenderersTypeEditor), typeof(UITypeEditor))]
        public String PivotTableRenderers
        {
            get { return _PivotTableRenderers; }
            set
            {
                _PivotTableRenderers = value;
            }
        }
    
        private String _PivotTableRenderersI;
        [Category("Infolight")]
        public String PivotTableRenderersI
        {
            get { return _PivotTableRenderersI; }
            set
            {
                _PivotTableRenderersI = value;
            }
        }


        private PivotTableRenderersType _PivotTableRenderersS;
        [Browsable(false)]
        public PivotTableRenderersType PivotTableRenderersS
        {
            get 
            {
                if (PivotTableRenderers != null && PivotTableRenderers != "")
                {
                    _PivotTableRenderersS = (PivotTableRenderersType)Enum.Parse(typeof(PivotTableRenderersType), PivotTableRenderers);
                }

                return _PivotTableRenderersS; 
            }
            set
            {
                _PivotTableRenderersS = value;
            }
        }

        private String _PivotTableAggregators;
        [Category("Infolight")]
        [Editor(typeof(PivotTableAggregatorsModeEditor), typeof(UITypeEditor))]
        public String PivotTableAggregators
        {
            get { return _PivotTableAggregators; }
            set
            {
                _PivotTableAggregators = value;
            }
        }

        private String _PivotTableAggregatorsI;
        [Category("Infolight")]
        public String PivotTableAggregatorsI
        {
            get { return _PivotTableAggregatorsI; }
            set
            {
                _PivotTableAggregatorsI = value;
            }
        }

        private PivotTableAggregatorsMode _PivotTableAggregatorsS;
        [Browsable(false)]
        public PivotTableAggregatorsMode PivotTableAggregatorsS
        {
            get
            {
                if (PivotTableAggregators != null && PivotTableAggregators != "")
                {
                    _PivotTableAggregatorsS = (PivotTableAggregatorsMode)Enum.Parse(typeof(PivotTableAggregatorsMode), PivotTableAggregators);
                }

                return _PivotTableAggregatorsS;
            }
            set
            {
                _PivotTableAggregatorsS = value;
            }
        }
        [Category("Infolight")]
        public bool AlwaysClose { get; set; }
        
        private Dictionary<string, string> columnCaptions;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        Dictionary<string, string> IColumnCaptions.ColumnCaptions
        {
            get
            {
                if (this.DesignMode)
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


        internal void CheckProperties()
        {
            if (string.IsNullOrEmpty(RemoteName))
            {
                throw new JQProperyNullException(this.ID, typeof(JQDataGrid), "RemoteName");
            }
            foreach (var column in this.ShowColumns)
            {
                column.CheckProperties();
            }
            foreach (var column in this.Columns)
            {
                column.CheckProperties();
            }
            foreach (var row in this.Rows)
            {
                row.CheckProperties();
            }

        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (!this.DesignMode)
            {
                EFClientTools.ClientUtility.ClientInfo.cErrorCode = 8070;
                CheckProperties();
                writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ID);
                writer.AddAttribute(HtmlTextWriterAttribute.Class, JQControl.PivotTable);
                writer.AddAttribute(JQProperty.InfolightOptions, this.InfolightOptions);
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
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

                options.Add(string.Format("remoteName:'{0}'", RemoteName));
                options.Add(string.Format("tableName:'{0}'", DataMember));
                //var columnList = new List<string>();
                //foreach (var column in Columns)
                //{
                //    columnList.Add(string.Format("'{0}'", column.FieldName));
                //}
                //options.Add(string.Format("columns:[{0}]", string.Join(",", columnList)));
                //var rowList = new List<string>();
                //foreach (var row in Rows)
                //{
                //    rowList.Add(string.Format("'{0}'", row.FieldName));
                //}
                //options.Add(string.Format("rows:[{0}]", string.Join(",", rowList)));
                //var showColumnList = new List<string>();
                //foreach (var showColumn in ShowColumns)
                //{
                //    showColumnList.Add(string.Format("'{0}'", showColumn.FieldName));
                //}
                //options.Add(string.Format("showColumns:[{0}]", string.Join(",", showColumnList)));
                var columns = new List<string>();
                foreach (var column in Columns)
                {
                    columns.Add(string.Format("{{field:'{0}',caption:'{1}'}}"
                        , column.FieldName, column.Caption));
                }
                options.Add(string.Format("columns:[{0}]", string.Join(",", columns)));
                var rows = new List<string>();
                foreach (var row in Rows)
                {
                    rows.Add(string.Format("{{field:'{0}',caption:'{1}'}}"
                        , row.FieldName, row.Caption));
                }
                options.Add(string.Format("rows:[{0}]", string.Join(",", rows)));
                var showColumns = new List<string>();
                foreach (var showcolumn in ShowColumns)
                {
                    showColumns.Add(string.Format("{{field:'{0}',caption:'{1}'}}"
                        , showcolumn.FieldName, showcolumn.Caption));
                }
                options.Add(string.Format("showColumns:[{0}]", string.Join(",", showColumns)));
                options.Add(string.Format("renderers:'{0}'", PivotTableRenderersI));
                options.Add(string.Format("aggregators:'{0}'", PivotTableAggregatorsI));
                if (!string.IsNullOrEmpty(OnBeforeLoad))
                {
                    options.Add(string.Format("onBeforeLoad:{0}", OnBeforeLoad));
                }
                if (PanelHeight > 0)
                {
                    options.Add(string.Format("panelHeight:{0}", PanelHeight));
                }
                else if (PanelHeight == 0)
                {
                    options.Add(string.Format("panelHeight:'auto'"));
                }
                options.Add(string.Format("alwaysClose:{0}", AlwaysClose.ToString().ToLower()));
                return string.Join(",", options);
            }
        }
    }

    public class JQPivotTableColumn : JQCollectionItem, IJQDataSourceProvider
    {
        public JQPivotTableColumn()
        {
        }

        private string fieldName;
        /// <summary>
        /// 栏位名
        /// </summary>
        [Category("Infolight")]
        [Editor(typeof(FieldEditor), typeof(UITypeEditor))]
        public string FieldName
        {
            get
            {
                return fieldName;
            }
            set
            {
                fieldName = value;

                if (Component != null && Component.ColumnCaptions != null)
                {
                    if (string.IsNullOrEmpty(caption) && Component.ColumnCaptions.ContainsKey(fieldName))
                    {
                        Caption = Component.ColumnCaptions[fieldName];
                    }
                }
            }
        }

        private string caption;
        /// <summary>
        /// 标题
        /// </summary>
        [Category("Infolight")]
        public string Caption
        {
            get
            {
                if (string.IsNullOrEmpty(caption))
                {
                    return FieldName;
                }
                else
                {
                    return caption;
                }
            }
            set
            {
                caption = value;
            }
        }
        [Category("Infolight")]
        [Browsable(false)]
        public string TableName { get; set; }

        internal void CheckProperties()
        {
        }

        public void Render(HtmlTextWriter writer)
        {
            writer.AddAttribute(JQProperty.DataOptions, this.DataOptions);
            writer.AddAttribute(JQProperty.InfolightOptions, this.InfolightOptions);
            writer.RenderBeginTag(HtmlTextWriterTag.Th);
            writer.Write(this.Caption);
            writer.RenderEndTag();
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        private string DataOptions
        {
            get
            {

                var options = new List<string>();
                options.Add(string.Format("field:'{0}'", FieldName));
                if (!string.IsNullOrEmpty(TableName))
                {
                    options.Add(string.Format("tableName:'{0}'", TableName));
                }
                return string.Join(",", options);
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        private string InfolightOptions
        {
            get
            {

                var options = new List<string>();
                options.Add(string.Format("field:'{0}'", FieldName));
                return string.Join(",", options);
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Default { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool CarryOn { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Validate { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string AutoSeq { get; set; }

        public override string ToString()
        {
            if (!string.IsNullOrEmpty(FieldName))
            {
                return this.FieldName;
            }
            else
            {
                return base.ToString();
            }
        }


        public IColumnCaptions Component
        {
            get
            {
                if ((this as IJQProperty).ParentProperty != null && (this as IJQProperty).ParentProperty.Component != null)
                {
                    return (this as IJQProperty).ParentProperty.Component as IColumnCaptions;
                }
                return null;
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

    [Flags]
    public enum PivotTableRenderersType
    {
        None = 0,
        Table = 1,
        TableBarChart = 2,
        Heatmap = 4,
        RowHeatmap = 8,
        ColHeatmap = 16,
        LineChart = 32,
        BarChart = 64,
        StackedBarChart = 128,
        AreaChart = 256,
        ScatterChart = 512
    }

    [Flags]
    public enum PivotTableAggregatorsMode
    {
        None = 0,
        Count = 1,
        CountUniqueValues = 2,
        ListUniqueValues = 4,
        Sum = 8,
        IntegerSum = 16,
        Average = 32,
        Minimum = 64,
        Maximum = 128,
        SumoverSum = 256,
        UpperBound = 512,
        LowerBound = 1024,
        CountasFractionofTotal = 2048,
        CountasFractionofRows = 4096,
        CountasFractionofColumns = 8192,
        SumasFractionofTotal = 16384,
        SumasFractionofRows = 32768,
        SumasFractionofColumns = 65536
    }
}
