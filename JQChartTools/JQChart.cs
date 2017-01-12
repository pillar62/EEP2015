using EFBase.Design;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace JQChartTools
{
    public class JQChartBase : WebControl
    {
        public JQChartBase()
        {
            whereItems = new JQCollection<JQChartWhereItem>(this);
            LegendLocation = LegendLocationEnum.ne;
            LegendPlacement = LegendPlacementEnum.outsideGrid;
        }

        /// <summary>
        /// 标题
        /// </summary>
        [Category("Infolight")]
        public string Title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Category("Infolight")]
        public bool LegendShow { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Category("Infolight")]
        public LegendLocationEnum LegendLocation { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Category("Infolight")]
        public LegendPlacementEnum LegendPlacement { get; set; }

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
        private string keyField;
        /// <summary>
        /// 主轴对应栏位
        /// </summary>
        [Category("Infolight")]
        [Editor(typeof(FieldEditor), typeof(UITypeEditor))]
        public string KeyField
        {
            get
            {
                return keyField;
            }
            set
            {
                keyField = value;
            }
        }
        private string valueField;
        /// <summary>
        /// Label对应栏位
        /// </summary>
        [Category("Infolight")]
        [Editor(typeof(FieldEditor), typeof(UITypeEditor))]
        public string ValueField
        {
            get
            {
                return valueField;
            }
            set
            {
                valueField = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Category("Infolight")]
        public bool AlwaysClose { get; set; }

        /// <summary>
        /// left
        /// </summary>
        [Category("Infolight")]
        public Unit Left { get; set; }
        /// <summary>
        /// top
        /// </summary>
        [Category("Infolight")]
        public Unit Top { get; set; }

        /// <summary>
        /// it trigger before load data
        /// </summary>
        [Category("Infolight")]
        public string OnBeforeLoad { get; set; }

        private JQCollection<JQChartWhereItem> whereItems;
        [Category("Infolight")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [NotifyParentProperty(true)]
        public JQCollection<JQChartWhereItem> WhereItems
        {
            get
            {
                return whereItems;
            }
        }
        /// <summary>
        /// click event name 
        /// </summary>
        [Category("Infolight")]
        public string OnClick { get; set; }

        /// <summary>
        /// if mobile set this to true
        /// </summary>
        [Category("Infolight")]
        [Editor(typeof(DataControlEditor), typeof(UITypeEditor))]
        public string RenderObjectID { get; set; }


    }
    public class JQChartWhereItem : JQCollectionItem, IJQDataSourceProvider
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
        [Editor(typeof(ConditionEditor), typeof(UITypeEditor))]
        public string Condition { get; set; }

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
    public class JQChartDataField : JQCollectionItem, IJQDataSourceProvider
    {
        private string _captionFieldName = "";
        /// <summary>
        /// 标题
        /// </summary>
        [Category("Infolight")]
        [NotifyParentProperty(true)]
        [Editor(typeof(FieldEditor), typeof(UITypeEditor))]
        [DefaultValue("")]
        public string CaptionFieldName
        {
            get
            {
                return _captionFieldName;
            }
            set
            {
                _captionFieldName = value;
                //if (!string.IsNullOrEmpty(_captionFieldName))
                //{
                //    FeildCaption = "";
                //}
            }
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

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        private string DataOptions
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
        private string InfolightOptions
        {
            get
            {
                var options = new List<string>();
                options.Add(string.Format("field:'{0}'", FieldName));
                return string.Join(",", options);
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
    public class JQLineChartDataField : JQCollectionItem, IJQDataSourceProvider
    {
        public JQLineChartDataField()
        {
            MarkerStyle = MarkerStyleStyle.circle;
        }
        private string _captionFieldName = "";
        /// <summary>
        /// 标题
        /// </summary>
        [Category("Infolight")]
        [NotifyParentProperty(true)]
        [Editor(typeof(FieldEditor), typeof(UITypeEditor))]
        [DefaultValue("")]
        public string CaptionFieldName
        {
            get
            {
                return _captionFieldName;
            }
            set
            {
                _captionFieldName = value;
                //if (!string.IsNullOrEmpty(_captionFieldName))
                //{
                //    FeildCaption = "";
                //}
            }
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

        private double _LineWidth = 2;
        /// <summary>
        /// 
        /// </summary>
        [Category("Infolight")]
        [NotifyParentProperty(true)]
        public double LineWidth
        {
            get
            {
                return _LineWidth;
            }
            set
            {
                _LineWidth = value;
            }
        }

        private bool _ShowPointLabels = false;
        /// <summary>
        /// 
        /// </summary>
        [Category("Infolight")]
        [NotifyParentProperty(true)]
        public bool ShowPointLabels
        {
            get
            {
                return _ShowPointLabels;
            }
            set
            {
                _ShowPointLabels = value;
            }
        }

        /// <summary>
        /// One of diamond, circle, square, x, plus, dash, filledDiamond, filledCircle, filledSquare
        /// </summary>
        [Category("Infolight")]
        [NotifyParentProperty(true)]
        public MarkerStyleStyle MarkerStyle { get; set; }

        public enum MarkerStyleStyle
        {
            diamond, circle, square, x, plus, dash, filledDiamond, filledCircle, filledSquare
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        private string DataOptions
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
        private string InfolightOptions
        {
            get
            {
                var options = new List<string>();
                options.Add(string.Format("field:'{0}'", FieldName));
                return string.Join(",", options);
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

    public class JQPieChart : JQChartBase, IJQDataSourceProvider
    {
        public JQPieChart()
        {
            Width = new Unit(600, UnitType.Pixel);
            Height = new Unit(600, UnitType.Pixel);
            Title = "JQPieChart";
            SliceMargin = 1;
            ShadowDepth = 5;
            Radius = 0;
            LabelShow = true;
            LegendShow = false;
            LabelStyle = JQPieChartLabelStyle.Key;
            Fill = true;
        }

        /// <summary>
        ///  If value is between 0 and 1 (inclusive) then it will use that as a percentage of the available space (size of the container), otherwise it will use the value as a direct pixel length
        /// </summary>
        [Category("Infolight")]
        public double Radius { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Category("Infolight")]
        public int SliceMargin { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Category("Infolight")]
        public int ShadowDepth { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Category("Infolight")]
        public double LineWidth { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Category("Infolight")]
        public bool LabelShow { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Category("Infolight")]
        public bool Fill { get; set; }

        private string labelShowField;
        /// <summary>
        /// Label对应栏位
        /// </summary>
        [Category("Infolight")]
        [Editor(typeof(FieldEditor), typeof(UITypeEditor))]
        public string LabelShowField
        {
            get
            {
                return labelShowField;
            }
            set
            {
                labelShowField = value;
            }
        }

        /// <summary>
        /// Label Style,Only work in labelShow is true
        /// </summary>
        [Category("Infolight")]
        public JQPieChartLabelStyle LabelStyle { get; set; }

        public override void RenderBeginTag(HtmlTextWriter writer)
        {
            if (!this.DesignMode)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ID);
                writer.AddAttribute(HtmlTextWriterAttribute.Class, JQControl.Pie);
                writer.AddAttribute(HtmlTextWriterAttribute.Title, Title);
                writer.AddAttribute(HtmlTextWriterAttribute.Style, string.Format("width:{0}px;padding:10px", Width.Value));
                writer.AddAttribute(JQProperty.DataOptions, DataOptions);
                writer.AddAttribute(JQProperty.InfolightOptions, InfolightOptions);
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
            }
        }

        public override void RenderEndTag(HtmlTextWriter writer)
        {
            if (!this.DesignMode)
            {
                writer.RenderEndTag();
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (!this.DesignMode)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ID);

                var styles = new List<string>();
                styles.Add("padding:10px");
                if (this.Width.Type == UnitType.Pixel && Width.Value > double.Epsilon)
                {
                    styles.Add(string.Format("width:{0}px", Width.Value));
                }
                if (this.Height.Type == UnitType.Pixel && Height.Value > double.Epsilon)
                {
                    styles.Add(string.Format("height:{0}px", Height.Value));
                }
                if (this.Left.Type == UnitType.Pixel && Left.Value > double.Epsilon)
                {
                    styles.Add(string.Format("left:{0}px", Left.Value));
                }
                if (this.Top.Type == UnitType.Pixel && Top.Value > double.Epsilon)
                {
                    styles.Add(string.Format("top:{0}px", Top.Value));
                }
                if (styles.Count > 0)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Style, string.Join(";", styles));
                }

                writer.AddAttribute(HtmlTextWriterAttribute.Title, Title);

                writer.AddAttribute(JQProperty.DataOptions, DataOptions);
                writer.AddAttribute(JQProperty.InfolightOptions, InfolightOptions);
                writer.AddAttribute(HtmlTextWriterAttribute.Class, JQControl.Pie);
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                RenderChildren(writer);
                writer.RenderEndTag();
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        private string DataOptions
        {
            get
            {
                var options = new List<string>();
                if (this.Width.Type == UnitType.Pixel && Width.Value > double.Epsilon)
                {
                    options.Add(string.Format("width:{0}", Width.Value));
                }
                if (this.Height.Type == UnitType.Pixel && Height.Value > double.Epsilon)
                {
                    options.Add(string.Format("height:{0}", Height.Value));
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
                options.Add(string.Format("remoteName:'{0}'", RemoteName));
                options.Add(string.Format("tableName:'{0}'", DataMember));
                options.Add(string.Format("valueField:'{0}'", ValueField));
                options.Add(string.Format("keyField:'{0}'", KeyField));
                options.Add(string.Format("labelShowField:'{0}'", LabelShowField));
                options.Add(string.Format("alwaysClose:{0}", AlwaysClose.ToString().ToLower()));
                options.Add(string.Format("labelStyle:'{0}'", LabelStyle.ToString().ToLower()));
                options.Add(string.Format("shadowDepth:{0}", ShadowDepth.ToString().ToLower()));
                if (!string.IsNullOrEmpty(OnBeforeLoad))
                {
                    options.Add(string.Format("onBeforeLoad:'{0}'", OnBeforeLoad));
                }
                if (!string.IsNullOrEmpty(Title))
                {
                    options.Add(string.Format("title:'{0}'", Title));
                }
                if (LineWidth > 0)
                {
                    options.Add(string.Format("lineWidth:'{0}'", LineWidth.ToString()));
                }
                if (Radius > 0)
                {
                    options.Add(string.Format("radius:{0}", Radius));
                }
                options.Add(string.Format("labelShow:{0}", LabelShow.ToString().ToLower()));
                options.Add(string.Format("fill:{0}", Fill.ToString().ToLower()));
                options.Add(string.Format("legendShow:{0}", LegendShow.ToString().ToLower()));
                options.Add(string.Format("legendLocation:'{0}'", LegendLocation.ToString().ToLower()));
                options.Add(string.Format("legendPlacement:'{0}'", LegendPlacement.ToString()));
                options.Add(string.Format("sliceMargin:{0}", SliceMargin.ToString()));

                var whereIterms = new List<string>();
                foreach (var whereItem in WhereItems)
                {
                    whereIterms.Add(string.Format("{{field:'{0}',value:'{1}',condition:'{2}'}}", whereItem.FieldName, whereItem.Value, whereItem.Condition));
                }
                options.Add(string.Format("width:'{0}'", Width.Value.ToString()));
                options.Add(string.Format("whereItems:[{0}]", string.Join(",", whereIterms)));
                options.Add(string.Format("renderObjectID:'{0}'", RenderObjectID != null ? RenderObjectID.ToString() : ""));
                return string.Join(",", options);
            }
        }
    }
    public class JQBarChart : JQChartBase, IJQDataSourceProvider
    {
        public JQBarChart()
        {
            Width = new Unit(600, UnitType.Pixel);
            Height = new Unit(600, UnitType.Pixel);
            Title = "JQBarChart";
            barWidth = 20;
            labelShow = true;
            LegendShow = false;
            stack = false;
            pointLabels = true;
            dataFields = new JQCollection<JQChartDataField>(this);
        }
        private bool stack;
        /// <summary>
        /// 
        /// </summary>
        [Category("Infolight")]
        public bool Stack
        {
            get
            {
                return stack;
            }
            set
            {
                stack = value;
            }
        }

        private string valueField;
        /// <summary>
        /// Label对应栏位
        /// </summary>
        [Category("Infolight")]
        [Editor(typeof(FieldEditor), typeof(UITypeEditor))]
        [Browsable(false)]
        public string ValueField
        {
            get
            {
                return valueField;
            }
            set
            {
                valueField = value;
            }
        }

        private JQCollection<JQChartDataField> dataFields;
        /// <summary>
        /// Y轴内容
        /// </summary>
        [Category("Infolight")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [NotifyParentProperty(true)]
        public JQCollection<JQChartDataField> DataFields
        {
            get { return dataFields; }
        }

        /// <summary>
        /// Show Point Label or not 
        /// </summary>
        [Category("Infolight")]
        public bool pointLabels { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Category("Infolight")]
        public bool labelShow { get; set; }

        /// <summary>
        /// between 0 and 1
        /// </summary>
        [Category("Infolight")]
        public double barWidth { get; set; }

        public override void RenderBeginTag(HtmlTextWriter writer)
        {
            if (!this.DesignMode)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ID);
                writer.AddAttribute(HtmlTextWriterAttribute.Class, JQControl.Bar);
                writer.AddAttribute(HtmlTextWriterAttribute.Title, Title);
                writer.AddAttribute(HtmlTextWriterAttribute.Style, string.Format("width:{0}px;padding:10px", Width.Value));
                writer.AddAttribute(JQProperty.DataOptions, DataOptions);
                writer.AddAttribute(JQProperty.InfolightOptions, InfolightOptions);
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
            }
        }

        public override void RenderEndTag(HtmlTextWriter writer)
        {
            if (!this.DesignMode)
            {
                writer.RenderEndTag();
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (!this.DesignMode)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ID);

                var styles = new List<string>();
                if (this.Width.Type == UnitType.Pixel && Width.Value > double.Epsilon)
                {
                    styles.Add(string.Format("width:{0}px", Width.Value));
                }
                if (this.Height.Type == UnitType.Pixel && Height.Value > double.Epsilon)
                {
                    styles.Add(string.Format("height:{0}px", Height.Value));
                }
                if (this.Left.Type == UnitType.Pixel && Left.Value > double.Epsilon)
                {
                    styles.Add(string.Format("left:{0}px", Left.Value));
                }
                if (this.Top.Type == UnitType.Pixel && Top.Value > double.Epsilon)
                {
                    styles.Add(string.Format("top:{0}px", Top.Value));
                }

                if (styles.Count > 0)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Style, string.Join(";", styles));
                }

                writer.AddAttribute(HtmlTextWriterAttribute.Title, Title);

                writer.AddAttribute(JQProperty.DataOptions, DataOptions);
                writer.AddAttribute(JQProperty.InfolightOptions, InfolightOptions);
                writer.AddAttribute(HtmlTextWriterAttribute.Class, JQControl.Bar);
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                RenderChildren(writer);
                writer.RenderEndTag();
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        private string DataOptions
        {
            get
            {
                var options = new List<string>();
                if (this.Width.Type == UnitType.Pixel && Width.Value > double.Epsilon)
                {
                    options.Add(string.Format("width:{0}", Width.Value));
                }
                if (this.Height.Type == UnitType.Pixel && Height.Value > double.Epsilon)
                {
                    options.Add(string.Format("height:{0}", Height.Value));
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
                options.Add(string.Format("remoteName:'{0}'", RemoteName));
                options.Add(string.Format("tableName:'{0}'", DataMember));
                options.Add(string.Format("valueField:'{0}'", ValueField));
                options.Add(string.Format("keyField:'{0}'", KeyField));
                options.Add(string.Format("alwaysClose:{0}", AlwaysClose.ToString().ToLower()));
                if (!string.IsNullOrEmpty(OnBeforeLoad))
                {
                    options.Add(string.Format("onBeforeLoad:'{0}'", OnBeforeLoad));
                }
                if (!string.IsNullOrEmpty(Title))
                {
                    options.Add(string.Format("title:'{0}'", Title));
                }
                options.Add(string.Format("labelShow:{0}", labelShow.ToString().ToLower()));
                options.Add(string.Format("legendShow:{0}", LegendShow.ToString().ToLower()));
                options.Add(string.Format("legendLocation:'{0}'", LegendLocation.ToString().ToLower()));
                options.Add(string.Format("legendPlacement:'{0}'", LegendPlacement.ToString()));
                options.Add(string.Format("barWidth:{0}", barWidth));
                options.Add(string.Format("stack:{0}", Stack.ToString().ToLower()));
                options.Add(string.Format("pointLabels:{0}", pointLabels.ToString().ToLower()));
                if (!string.IsNullOrEmpty(OnClick))
                {
                    options.Add(string.Format("onClick:'{0}'", OnClick));
                }
                var whereIterms = new List<string>();
                foreach (var whereItem in WhereItems)
                {
                    whereIterms.Add(string.Format("{{field:'{0}',value:'{1}',condition:'{2}'}}", whereItem.FieldName, whereItem.Value, whereItem.Condition));
                }
                options.Add(string.Format("whereItems:[{0}]", string.Join(",", whereIterms)));

                var dataFieldList = new List<string>();
                foreach (var dataField in DataFields)
                {
                    dataFieldList.Add(string.Format("{{fieldName:'{0}',captionFieldName:'{1}',caption:'{2}'}}", dataField.FieldName, dataField.CaptionFieldName, dataField.Caption));
                }
                options.Add(string.Format("width:'{0}'", Width.Value.ToString()));
                options.Add(string.Format("dataFields:[{0}]", string.Join(",", dataFieldList)));
                options.Add(string.Format("renderObjectID:'{0}'", RenderObjectID != null ? RenderObjectID.ToString() : ""));
                return string.Join(",", options);
            }
        }
    }
    public class JQLineChart : JQChartBase, IJQDataSourceProvider
    {
        public JQLineChart()
        {
            Width = new Unit(600, UnitType.Pixel);
            Height = new Unit(600, UnitType.Pixel);
            Title = "JQLineChart";
            LegendShow = false;
            dataFields = new JQCollection<JQLineChartDataField>(this);
        }

        private string valueField;
        /// <summary>
        /// Label对应栏位
        /// </summary>
        [Category("Infolight")]
        [Editor(typeof(FieldEditor), typeof(UITypeEditor))]
        [Browsable(false)]
        public string ValueField
        {
            get
            {
                return valueField;
            }
            set
            {
                valueField = value;
            }
        }

        private string keyShowField;
        /// <summary>
        /// Label对应栏位
        /// </summary>
        [Category("Infolight")]
        [Editor(typeof(FieldEditor), typeof(UITypeEditor))]
        public string KeyShowField
        {
            get
            {
                return keyShowField;
            }
            set
            {
                keyShowField = value;
            }
        }
        private JQCollection<JQLineChartDataField> dataFields;
        /// <summary>
        /// 轴内容
        /// </summary>
        [Category("Infolight")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [NotifyParentProperty(true)]
        public JQCollection<JQLineChartDataField> DataFields
        {
            get { return dataFields; }
        }
        public override void RenderBeginTag(HtmlTextWriter writer)
        {
            if (!this.DesignMode)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ID);
                writer.AddAttribute(HtmlTextWriterAttribute.Class, JQControl.Line);
                writer.AddAttribute(HtmlTextWriterAttribute.Title, Title);
                writer.AddAttribute(HtmlTextWriterAttribute.Style, string.Format("width:{0}px;padding:10px", Width.Value));
                writer.AddAttribute(JQProperty.DataOptions, DataOptions);
                writer.AddAttribute(JQProperty.InfolightOptions, InfolightOptions);
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
            }
        }

        public override void RenderEndTag(HtmlTextWriter writer)
        {
            if (!this.DesignMode)
            {
                writer.RenderEndTag();
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (!this.DesignMode)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ID);

                var styles = new List<string>();
                if (this.Width.Type == UnitType.Pixel && Width.Value > double.Epsilon)
                {
                    styles.Add(string.Format("width:{0}px", Width.Value));
                }
                if (this.Height.Type == UnitType.Pixel && Height.Value > double.Epsilon)
                {
                    styles.Add(string.Format("height:{0}px", Height.Value));
                }
                if (this.Left.Type == UnitType.Pixel && Left.Value > double.Epsilon)
                {
                    styles.Add(string.Format("left:{0}px", Left.Value));
                }
                if (this.Top.Type == UnitType.Pixel && Top.Value > double.Epsilon)
                {
                    styles.Add(string.Format("top:{0}px", Top.Value));
                }
                if (styles.Count > 0)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Style, string.Join(";", styles));
                }
                writer.AddAttribute(HtmlTextWriterAttribute.Title, Title);

                writer.AddAttribute(JQProperty.DataOptions, DataOptions);
                writer.AddAttribute(JQProperty.InfolightOptions, InfolightOptions);
                writer.AddAttribute(HtmlTextWriterAttribute.Class, JQControl.Line);
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                RenderChildren(writer);
                writer.RenderEndTag();
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        private string DataOptions
        {
            get
            {
                var options = new List<string>();
                if (this.Width.Type == UnitType.Pixel && Width.Value > double.Epsilon)
                {
                    options.Add(string.Format("width:{0}", Width.Value));
                }
                if (this.Height.Type == UnitType.Pixel && Height.Value > double.Epsilon)
                {
                    options.Add(string.Format("height:{0}", Height.Value));
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
                options.Add(string.Format("remoteName:'{0}'", RemoteName));
                options.Add(string.Format("tableName:'{0}'", DataMember));
                options.Add(string.Format("valueField:'{0}'", ValueField));
                options.Add(string.Format("keyField:'{0}'", KeyField));
                options.Add(string.Format("keyShowField:'{0}'", KeyShowField));
                options.Add(string.Format("alwaysClose:{0}", AlwaysClose.ToString().ToLower()));
                if (!string.IsNullOrEmpty(OnBeforeLoad))
                {
                    options.Add(string.Format("onBeforeLoad:'{0}'", OnBeforeLoad));
                }
                if (!string.IsNullOrEmpty(Title))
                {
                    options.Add(string.Format("title:'{0}'", Title));
                }
                options.Add(string.Format("legendShow:{0}", LegendShow.ToString().ToLower()));
                options.Add(string.Format("legendLocation:'{0}'", LegendLocation.ToString().ToLower()));
                options.Add(string.Format("legendPlacement:'{0}'", LegendPlacement.ToString()));
                var whereIterms = new List<string>();
                foreach (var whereItem in WhereItems)
                {
                    whereIterms.Add(string.Format("{{field:'{0}',value:'{1}',condition:'{2}'}}", whereItem.FieldName, whereItem.Value, whereItem.Condition));
                }
                options.Add(string.Format("whereItems:[{0}]", string.Join(",", whereIterms)));

                var dataFieldList = new List<string>();
                foreach (var dataField in DataFields)
                {
                    dataFieldList.Add(string.Format("{{fieldName:'{0}',captionFieldName:'{1}',caption:'{2}',lineWidth:{3},markerStyle:'{4}',showPointLabels:{5}}}", dataField.FieldName, dataField.CaptionFieldName, dataField.Caption, dataField.LineWidth, dataField.MarkerStyle, dataField.ShowPointLabels.ToString().ToLower()));
                }
                options.Add(string.Format("width:'{0}'", Width.Value.ToString()));
                options.Add(string.Format("dataFields:[{0}]", string.Join(",", dataFieldList)));
                options.Add(string.Format("renderObjectID:'{0}'", RenderObjectID != null ? RenderObjectID.ToString() : ""));
                return string.Join(",", options);
            }
        }
    }
    public enum JQPieChartLabelStyle
    {
        Key, Value, Percent, LabelShowField
    }
    public enum LegendLocationEnum
    {
        e, w, n, s, ne, nw, se, sw
    }
    public enum LegendPlacementEnum
    {
        insideGrid, outsideGrid
    }

    public class JQDashBoard : WebControl
    {
        public JQDashBoard()
        {
            Ticks = "[0, 20, 40, 60, 80, 100, 120]";
            Intervals = "[40, 80, 120]";
            IntervalColors = "['#66cc66', '#E7E658', '#cc6666']";
            Background = Color.Snow;
            RingColor = Color.Silver;
            TickColor = Color.DarkBlue;
            LabelColor = Color.Black;
            RingWidth = null;
            Width = 300;
            Height = 300;
        }
        /// <summary>
        /// Value
        /// </summary>
        [Category("Infolight")]
        public int Value { get; set; }

        /// <summary>
        /// Array of tick values. eg:[0, 20, 40, 60, 80, 100, 120]
        /// </summary>
        [Category("Infolight")]
        public string Ticks { get; set; }

        /// <summary>
        /// Array of ranges to be drawn around the gauge.eg:[40, 80, 120]
        /// </summary>
        [Category("Infolight")]
        public string Intervals { get; set; }

        /// <summary>
        /// A gauge label like ‘kph’ or ‘Volts’
        /// </summary>
        [Category("Infolight")]
        public string Label { get; set; }

        /// <summary>
        /// Array of colors to use for the intervals.
        /// </summary>
        [Category("Infolight")]
        [Editor(typeof(ColorCollectionEditor), typeof(UITypeEditor))]
        public string IntervalColors { get; set; }

        /// <summary>
        /// background color of the inside of the gauge.
        /// </summary>
        [Category("Infolight")]
        [Editor(typeof(System.Drawing.Design.ColorEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public Color Background { get; set; }

        /// <summary>
        /// color of the outer ring, hub, and needle of the gauge.
        /// </summary>
        [Category("Infolight")]
        [Editor(typeof(System.Drawing.Design.ColorEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public Color RingColor { get; set; }

        /// <summary>
        /// color of the tick marks around the gauge.
        /// </summary>
        [Category("Infolight")]
        [Editor(typeof(System.Drawing.Design.ColorEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public Color TickColor { get; set; }

        /// <summary>
        /// color of the tick marks around the gauge.
        /// </summary>
        [Category("Infolight")]
        [Editor(typeof(System.Drawing.Design.ColorEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public Color LabelColor { get; set; }

        /// <summary>
        /// width of the ring around the gauge.
        /// </summary>
        [Category("Infolight")]
        public string RingWidth { get; set; }

        /// <summary>
        /// Number of Pixels to offset the label up (-) or down (+) from its default position.
        /// </summary>
        [Category("Infolight")]
        public double LabelHeightAdjust { get; set; }

        /// <summary>
        /// left
        /// </summary>
        [Category("Infolight")]
        public Unit Left { get; set; }
        /// <summary>
        /// top
        /// </summary>
        [Category("Infolight")]
        public Unit Top { get; set; }
        /// <summary>
        /// it trigger before load data
        /// </summary>
        [Category("Infolight")]
        public string OnBeforeLoad { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Category("Infolight")]
        [Editor(typeof(DataControlEditor), typeof(UITypeEditor))]
        public string RenderObjectID { get; set; }

        protected override void Render(HtmlTextWriter writer)
        {
            if (!this.DesignMode)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ID);

                var styles = new List<string>();
                if (this.Width.Type == UnitType.Pixel && Width.Value > double.Epsilon)
                {
                    styles.Add(string.Format("width:{0}px", Width.Value));
                }
                if (this.Height.Type == UnitType.Pixel && Height.Value > double.Epsilon)
                {
                    styles.Add(string.Format("height:{0}px", Height.Value));
                }
                if (this.Left.Type == UnitType.Pixel && Left.Value > double.Epsilon)
                {
                    styles.Add(string.Format("left:{0}px", Left.Value));
                }
                if (this.Top.Type == UnitType.Pixel && Top.Value > double.Epsilon)
                {
                    styles.Add(string.Format("top:{0}px", Top.Value));
                }
                if (styles.Count > 0)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Style, string.Join(";", styles));
                }
                writer.AddAttribute(JQProperty.DataOptions, DataOptions);
                writer.AddAttribute(JQProperty.InfolightOptions, InfolightOptions);
                writer.AddAttribute(HtmlTextWriterAttribute.Class, JQControl.DashBoard);
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                RenderChildren(writer);
                writer.RenderEndTag();
            }
        }

        public override void RenderBeginTag(HtmlTextWriter writer)
        {
            if (!this.DesignMode)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ID);
                writer.AddAttribute(HtmlTextWriterAttribute.Class, JQControl.DashBoard);
                writer.AddAttribute(HtmlTextWriterAttribute.Style, string.Format("width:{0}px;padding:10px", Width.Value));
                writer.AddAttribute(JQProperty.DataOptions, DataOptions);
                writer.AddAttribute(JQProperty.InfolightOptions, InfolightOptions);
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
            }
        }

        public override void RenderEndTag(HtmlTextWriter writer)
        {
            if (!this.DesignMode)
            {
                writer.RenderEndTag();
            }
        }
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        private string DataOptions
        {
            get
            {
                var options = new List<string>();
                if (this.Width.Type == UnitType.Pixel && Width.Value > double.Epsilon)
                {
                    options.Add(string.Format("width:{0}", Width.Value));
                }
                if (this.Height.Type == UnitType.Pixel && Height.Value > double.Epsilon)
                {
                    options.Add(string.Format("height:{0}", Height.Value));
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
                options.Add(string.Format("value:{0}", Value.ToString()));
                options.Add(string.Format("label:'{0}'", Label == null ? "" : Label.ToString()));
                options.Add(string.Format("ticks:{0}", Ticks));
                options.Add(string.Format("intervals:{0}", Intervals.ToString()));
                options.Add(string.Format("intervalColors:{0}", IntervalColors.Replace("'", "\"")));
                options.Add(string.Format("background:'{0}'", ColorTranslator.ToHtml(Background)));
                options.Add(string.Format("ringColor:'{0}'", ColorTranslator.ToHtml(RingColor)));
                options.Add(string.Format("tickColor:'{0}'", ColorTranslator.ToHtml(TickColor)));
                options.Add(string.Format("labelColor:'{0}'", ColorTranslator.ToHtml(LabelColor)));
                if (!string.IsNullOrEmpty(RingWidth))
                {
                    options.Add(string.Format("ringWidth:'{0}'", RingWidth.ToString()));
                }
                if (!string.IsNullOrEmpty(OnBeforeLoad))
                {
                    options.Add(string.Format("onBeforeLoad:{0}", OnBeforeLoad));
                }
                options.Add(string.Format("width:'{0}'", Width.Value.ToString()));
                options.Add(string.Format("labelHeightAdjust:{0}", LabelHeightAdjust.ToString()));
                options.Add(string.Format("renderObjectID:'{0}'", RenderObjectID != null ? RenderObjectID.ToString() : ""));
                return string.Join(",", options);
            }
        }
    }

    public class ColorCollectionEditor : PropertyModalEditor
    {
        public override ModalForm GetModalForm(ITypeDescriptorContext context)
        {
            return new ColorCollection();

        }
    }


}
