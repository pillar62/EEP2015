using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Xml;
using System.ComponentModel.Design;
using System.Web.UI.Design;
using System.Drawing;
using System.Globalization;
using System.Drawing.Design;
using System.Collections.Generic;
using System.Windows.Forms.Design;
using System.Data;
using Srvtools;
using System.Collections;
using System.Text.RegularExpressions;

namespace ChartTools
{
    /// <summary>
    /// A event calendar control that displays a list of events by quarter.
    /// </summary>
    /// <remarks>
    /// </remarks>
    [DefaultProperty("ChartTools"), ToolboxData("<{0}:WebCalendarChart runat=server></{0}:WebCalendarChart>")]
    [Designer(typeof(DataSourceDesigner), typeof(IDesigner))]
    [ParseChildren(true, "Fields")]
    public class WebCalendarChart : System.Web.UI.WebControls.WebControl, IGetValues
    {
        #region Variable Definition
        private string m_cellWidth = "15";
        private string m_cellHeight = "20";
        private string m_factBlockColor = "red";
        private string m_planBlockColor = "red";
        private string m_toggleColor = "#dcdcdc";
        private GanttDate m_ganttType = GanttDate.Week;
        private int m_year, m_quarter;
        string[] m_names;
        WebColorConverter webColorConverter = new WebColorConverter();
        private int m_headerGap = 10;
        private StatType m_statType = StatType.預計和實際;
        private int totalRowCount = -1;
        private int currentRowCount = -1;
        private XmlDocument xmlDocument;
        #endregion

        public WebCalendarChart()
        {
            m_names = new string[3];

            fields = new ChartFieldItemCollection(this, typeof(ChartFieldItem));
        }

        #region Public Properties
        /// <summary>
        /// Gets or sets a value that indicates the quarter (1,2,3,4)
        /// </summary>
        [Browsable(false)]
        [Bindable(true), Category("Appearance"), DefaultValue(1)]
        public int Quarter
        {
            get
            {
                return m_quarter;
            }

            set
            {
                m_quarter = value;
            }
        }

        /// <summary>
        /// Gets or sets a value that indicates the Year e.g. 2005
        /// </summary>
        [Browsable(false)]
        [Bindable(true), Category("Appearance"), DefaultValue("")]
        public int Year
        {
            get
            {
                return m_year;
            }

            set
            {
                m_year = value;
            }
        }

        /// <summary>
        /// Gets or sets a value that indicates the default BlockColor
        /// </summary>
        [Bindable(true), Category("Infolight"), DefaultValue("red")]
        public Color FactBlockColor
        {
            get
            {
                return (Color)webColorConverter.ConvertFromString(m_factBlockColor);
            }

            set
            {
                m_factBlockColor = webColorConverter.ConvertToString(value); ;
            }
        }

        /// <summary>
        /// Gets or sets a value that indicates the default BlockColor
        /// </summary>
        [Bindable(true), Category("Infolight"), DefaultValue("red")]
        public Color PlanBlockColor
        {
            get
            {
                return (Color)webColorConverter.ConvertFromString(m_planBlockColor);
            }

            set
            {
                m_planBlockColor = webColorConverter.ConvertToString(value); ;
            }
        }

        /// <summary>
        /// Gets or sets a value that indicates the cell width of the event calendar 'grid'
        /// </summary>
        [Bindable(true), Category("Infolight")]
        public int CellWidth
        {
            get
            {
                return int.Parse(m_cellWidth);
            }

            set
            {
                m_cellWidth = value.ToString();
            }
        }

        /// <summary>
        /// Gets or sets a value that indicates the cell height of the event calendar 'grid'
        /// </summary>
        [Bindable(true), Category("Infolight")]
        public int CellHeight
        {
            get
            {
                return int.Parse(m_cellHeight);
            }

            set
            {
                m_cellHeight = value.ToString();
            }
        }

        /// <summary>
        /// Gets or sets a value that indicates a color the event calendar will use to delimit the day names and event names.
        /// </summary>		
        [Bindable(true), Category("Infolight")]
        public Color ToggleColor
        {
            get
            {
                return (Color)webColorConverter.ConvertFromString(m_toggleColor);
            }

            set
            {
                m_toggleColor = webColorConverter.ConvertToString(value);
            }
        }

        #region Infolight
        [Bindable(true)]
        [Category("Infolight"),
       Description("The ID of WebDataSource which the control is bound to")]
        [Editor(typeof(ChartDataSourceEditor), typeof(UITypeEditor))]
        public string DataSourceID
        {
            get
            {
                object obj = this.ViewState["DataSourceID"];
                if (obj != null)
                {
                    return (string)obj;
                }
                return string.Empty;
            }
            set
            {
                this.ViewState["DataSourceID"] = value;
            }
        }

        private ChartFieldItemCollection fields;
        [Category("Infolight"),
       Description("")]
        [PersistenceMode(PersistenceMode.InnerProperty),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        TypeConverter(typeof(CollectionConverter)),
        NotifyParentProperty(true)]
        public ChartFieldItemCollection Fields
        {
            get { return fields; }
        }

        private string m_planStartDateColumn;
        [Category("Infolight")]
        [Editor(typeof(FieldNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [Bindable(true)]
        public string PlanStartDateColumn
        {
            get { return m_planStartDateColumn; }
            set { m_planStartDateColumn = value; }
        }

        private string m_planEndDateColumn;
        [Category("Infolight")]
        [Editor(typeof(FieldNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [Bindable(true)]
        public string PlanEndDateColumn
        {
            get { return m_planEndDateColumn; }
            set { m_planEndDateColumn = value; }
        }

        private string m_factStartDateColumn;
        [Category("Infolight")]
        [Editor(typeof(FieldNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [Bindable(true)]
        public string FactStartDateColumn
        {
            get { return m_factStartDateColumn; }
            set { m_factStartDateColumn = value; }
        }

        private string m_factEndDateColumn;
        [Category("Infolight")]
        [Editor(typeof(FieldNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [Bindable(true)]
        public string FactEndDateColumn
        {
            get { return m_factEndDateColumn; }
            set { m_factEndDateColumn = value; }
        }

        private string m_groupColumn;
        [Category("Infolight")]
        [Editor(typeof(FieldNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [Bindable(true)]
        public string GroupColumn
        {
            get { return m_groupColumn; }
            set { m_groupColumn = value; }
        }


        [Category("Infolight")]
        [Bindable(true)]
        public int HeaderGap
        {
            get { return m_headerGap; }
            set { m_headerGap = value; }
        }

        [Category("Infolight")]
        [Bindable(true)]
        public StatType StatMode
        {
            get { return m_statType; }
            set { m_statType = value; }
        }

        [Browsable(false)]
        [Bindable(true), Category("Infolight")]
        public GanttDate GanttType
        {
            get
            {
                return m_ganttType;
            }

            set
            {
                m_ganttType = value;
            }
        }

        
        private bool autoNum;
        [Category("Infolight")]
        [Bindable(true)]
        public bool AutoNum
        {
            get { return autoNum; }
            set { autoNum = value; }
        }


        private string autoNumCaption;
        [Category("Infolight")]
        [Bindable(true)]
        public string AutoNumCaption
        {
            get { return autoNumCaption; }
            set { autoNumCaption = value; }
        }

        
        private int autoNumLength;
        [Category("Infolight")]
        [Bindable(true)]
        public int AutoNumLength
        {
            get { return autoNumLength; }
            set { autoNumLength = value; }
        }

        private bool displayWeekDetail;
        [Category("Infolight")]
        [Bindable(true)]
        public bool DisplayWeekDetail
        {
            get { return displayWeekDetail; }
            set { displayWeekDetail = value; }
        }

        /// <summary>
        /// Format:yyyyMMdd
        /// </summary>
        [Browsable(false)]
        public string QueryCondition
        {
            get { return ViewState["QueryCondition"] == null ? "" : (string)ViewState["QueryCondition"]; }
            set { ViewState["QueryCondition"] = value; }
        }

        private bool displayScrollBar = false;
        [Category("Infolight")]
        [Bindable(true)]
        public bool DisplayScrollBar
        {
            get { return displayScrollBar; }
            set { displayScrollBar = value; }
        }
        #endregion

        #region IGetValues Members

        public string[] GetValues(string sKind)
        {
            string[] retList = null;
            List<string> values = new List<string>();
            WebDataSource wds = null;
            WebDataSet wdt = null;

            switch (sKind.ToLower())
            {
                case "planstartdatecolumn":
                case "planenddatecolumn":
                case "factstartdatecolumn":
                case "factenddatecolumn":
                case "groupcolumn":
                    if (this.DataSourceID != null && this.DataSourceID != String.Empty)
                    {
                        wds = (WebDataSource)this.GetObjByID(this.DataSourceID);
                        if (wds.DesignDataSet == null)
                        {
                            wdt = WebDataSet.CreateWebDataSet(wds.WebDataSetID);
                            if (wdt != null)
                            {
                                wds.DesignDataSet = wdt.RealDataSet;
                            }
                        }
                        if (wds.DesignDataSet.Tables.Contains(wds.DataMember))
                        {
                            foreach (DataColumn column in wds.DesignDataSet.Tables[wds.DataMember].Columns)
                            {
                                values.Add(column.ColumnName);
                            }
                        }
                    }
                    break;
            }

           
            if (values.Count > 0)
            {
                int i = values.Count;
                retList = new string[i];
                for (int j = 0; j < i; j++)
                {
                    retList[j] = values[j];
                }
            }
            return retList;
        }

        #endregion

        #region Common Function
        private Control GetAllCtrls(string strid, Control ct)
        {
            if (ct.ID == strid)
            {
                return ct;
            }
            else
            {
                if (ct.HasControls())
                {
                    foreach (Control ctchild in ct.Controls)
                    {
                        Control ctrtn = GetAllCtrls(strid, ctchild);
                        if (ctrtn != null)
                        {
                            return ctrtn;
                        }
                    }
                    return null;
                }
                else
                {
                    return null;
                }
            }
        }

        public object GetObjByID(string ObjID)
        {
            if (this.Site != null)
            {
                return GetAllCtrls(ObjID, this.Page);
            }
            else
            {
                if (this.Page.Form != null)
                    return GetAllCtrls(ObjID, this.Page.Form);
                else
                    return GetAllCtrls(ObjID, this.Page);
            }
        }
        #endregion
        #endregion

        #region Event Handling

        /// <summary>
        /// writes out some javascript so the calendar grid can resize itself
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (!base.Page.ClientScript.IsStartupScriptRegistered("EventCalendar"))
            {
                base.Page.ClientScript.RegisterStartupScript(this.GetType(), "WebCalendar",
                    "<script>" +
                    "function ResizeTables()" +
                    "{" +
                    "document.getElementById('divcal').style.width = '1px';" +
                    "document.getElementById('divcal').style.width = document.getElementById('tblcal').clientWidth + 'px';" +
                    "};" +
                    "</script>");

                base.Page.ClientScript.RegisterStartupScript(this.GetType(), "WebCalendarOnLoad",
                    "<SCRIPT FOR=window EVENT=onload>" +
                    "ResizeTables();" +
                    "</script>");

                base.Page.ClientScript.RegisterStartupScript(this.GetType(), "WebCalendarOnResize",
                    "<SCRIPT FOR=window EVENT=onresize>" +
                    "ResizeTables();" +
                    "</script>");
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (this.DataSourceID == null || this.DataSourceID == String.Empty)
            {
                return;
            }

            switch (m_statType)
            {
                case StatType.預計和實際:
                    if (this.FactEndDateColumn == null || this.FactEndDateColumn == String.Empty)
                    {
                        return;
                    }

                    if (this.FactStartDateColumn == null || this.FactStartDateColumn == String.Empty)
                    {
                        return;
                    }

                    if (this.PlanEndDateColumn == null || this.PlanEndDateColumn == String.Empty)
                    {
                        return;
                    }

                    if (this.PlanStartDateColumn == null || this.PlanStartDateColumn == String.Empty)
                    {
                        return;
                    }
                    break;
                case StatType.預計:
                    if (this.PlanEndDateColumn == null || this.PlanEndDateColumn == String.Empty)
                    {
                        return;
                    }

                    if (this.PlanStartDateColumn == null || this.PlanStartDateColumn == String.Empty)
                    {
                        return;
                    }
                    break;
                case StatType.實際:
                    if (this.FactEndDateColumn == null || this.FactEndDateColumn == String.Empty)
                    {
                        return;
                    }

                    if (this.FactStartDateColumn == null || this.FactStartDateColumn == String.Empty)
                    {
                        return;
                    }
                    break;
            }
            
            RunderPicture(writer);

            //RunderPicture2(writer);
        }

        #endregion

        #region Helper Functions

        private DataTable GetDataTable()
        {
            WebDataSource wds = null;
            DataTable dt = new DataTable();
            DataView dv = new DataView();

            if(this.DataSourceID != null && this.DataSourceID != String.Empty)
            {
                wds = (WebDataSource)this.GetObjByID(this.DataSourceID);
                dt = wds.InnerDataSet.Tables[0];
            }

            if(!string.IsNullOrEmpty(m_groupColumn))
            {
                dv = dt.DefaultView;
                dv.Sort = m_groupColumn + " " + "asc";
                dt = dv.ToTable(dt.TableName);
            }
            return dt;
        }

        private XmlDocument GetXmlByDataSource()
        {
            #region Variable Definition
            XmlDocument xmlDoc = new XmlDocument();
            DataTable dt = null;
            XmlElement rootNode = null;
            XmlElement groupNode = null;
            XmlElement blockNode = null;
            XmlElement blockStartDateNode = null;
            XmlElement blockEndDateNode = null;
            XmlElement blockNameNode = null;
            XmlElement groupColumnNode = null;
            #endregion

            dt = this.GetDataTable();
            totalRowCount = dt.Rows.Count;

            rootNode = xmlDoc.CreateElement("NewDataSet");

            foreach (DataRow dr in dt.Rows)
            {
                groupNode = xmlDoc.CreateElement("group");

                foreach (ChartFieldItem fieldItem in this.fields)
                {
                    groupColumnNode = xmlDoc.CreateElement(fieldItem.FieldName);
                    groupColumnNode.InnerText = dr[fieldItem.FieldName].ToString();
                    groupNode.AppendChild(groupColumnNode);
                }

                switch (m_statType)
                {
                    case StatType.預計和實際:
                        #region 預計和實際
                        blockNode = xmlDoc.CreateElement("block");
                        blockStartDateNode = xmlDoc.CreateElement("StartDate");
                        blockStartDateNode.InnerText = dr[m_planStartDateColumn].ToString();
                        blockEndDateNode = xmlDoc.CreateElement("EndDate");
                        blockEndDateNode.InnerText = dr[m_planEndDateColumn].ToString();
                        blockNameNode = xmlDoc.CreateElement("name");
                        blockNameNode.InnerText = Config.ColPlan;
                        blockNode.AppendChild(blockStartDateNode);
                        blockNode.AppendChild(blockEndDateNode);
                        blockNode.AppendChild(blockNameNode);
                        groupNode.AppendChild(blockNode);

                        blockNode = xmlDoc.CreateElement("block");
                        blockStartDateNode = xmlDoc.CreateElement("StartDate");
                        blockStartDateNode.InnerText = dr[m_factStartDateColumn].ToString();
                        blockEndDateNode = xmlDoc.CreateElement("EndDate");
                        blockEndDateNode.InnerText = dr[m_factEndDateColumn].ToString();
                        blockNameNode = xmlDoc.CreateElement("name");
                        blockNameNode.InnerText = Config.ColFact;
                        blockNode.AppendChild(blockStartDateNode);
                        blockNode.AppendChild(blockEndDateNode);
                        blockNode.AppendChild(blockNameNode);
                        groupNode.AppendChild(blockNode);
                        #endregion
                        break;
                    case StatType.預計:
                        #region 預計
                        blockNode = xmlDoc.CreateElement("block");
                        blockStartDateNode = xmlDoc.CreateElement("StartDate");
                        blockStartDateNode.InnerText = dr[m_planStartDateColumn].ToString();
                        blockEndDateNode = xmlDoc.CreateElement("EndDate");
                        blockEndDateNode.InnerText = dr[m_planEndDateColumn].ToString();
                        blockNameNode = xmlDoc.CreateElement("name");
                        blockNameNode.InnerText = Config.ColPlan;
                        blockNode.AppendChild(blockStartDateNode);
                        blockNode.AppendChild(blockEndDateNode);
                        blockNode.AppendChild(blockNameNode);
                        groupNode.AppendChild(blockNode);
                        #endregion
                        break;
                    case StatType.實際:
                        #region 實際
                        blockNode = xmlDoc.CreateElement("block");
                        blockStartDateNode = xmlDoc.CreateElement("StartDate");
                        blockStartDateNode.InnerText = dr[m_factStartDateColumn].ToString();
                        blockEndDateNode = xmlDoc.CreateElement("EndDate");
                        blockEndDateNode.InnerText = dr[m_factEndDateColumn].ToString();
                        blockNameNode = xmlDoc.CreateElement("name");
                        blockNameNode.InnerText = Config.ColFact;
                        blockNode.AppendChild(blockStartDateNode);
                        blockNode.AppendChild(blockEndDateNode);
                        blockNode.AppendChild(blockNameNode);
                        groupNode.AppendChild(blockNode);
                        #endregion
                        break;
                }

                rootNode.AppendChild(groupNode);
            }

            xmlDoc.AppendChild(rootNode);

            return xmlDoc;
        }

        private void RunderPicture(HtmlTextWriter writer)
        {
            int pageCount = -1;

            xmlDocument = this.GetXmlByDataSource();

            if (m_headerGap > 0)
            {
                pageCount = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(totalRowCount) / Convert.ToDouble(m_headerGap)));
            }
            else
            {
                pageCount = 1;
            }

            // create a 2 column, 1 row table, the first column will contain the text for the tasks that
            // is non scrollable.

            writer.AddAttribute("border", "0");
            writer.AddAttribute("cellSpacing", "0");
            writer.AddAttribute("cellPadding", "0");
            writer.AddAttribute("width", "100%");
            writer.RenderBeginTag("table");

            writer.RenderBeginTag("tr");


            writer.AddAttribute("valign", "top");
            writer.AddAttribute("align", "right");
            writer.RenderBeginTag("td nowrap");
            RenderLeftHandTableStart(writer);
            currentRowCount = 0;
            for (int i = 0; i < pageCount; i++)
            {
                RenderLeftHandPaneCaption(writer);
                RenderLeftHandPane(writer);
            }
            RenderLeftHandTableEnd(writer);
            writer.RenderEndTag(); //end td

            writer.AddAttribute("id", "tblcal");
            writer.AddAttribute("valign", "top");
            writer.AddAttribute("align", "left");
            writer.AddAttribute("width", "100%");
            writer.RenderBeginTag("td nowrap");
            RenderRightHandDivStart(writer);
            currentRowCount = 0;
            for (int i = 0; i < pageCount; i++)
            {
                RenderRightHandPane(writer, m_ganttType);
            }
            RenderLeftHandTableEnd(writer);
            writer.RenderEndTag(); //end td

            writer.RenderEndTag(); //end tr
           
            writer.RenderEndTag(); //end table
        }

        private void RenderLeftHandTableStart(HtmlTextWriter writer)
        {
            writer.AddAttribute("border", "1");
            writer.AddAttribute("style", "FONT-SIZE: " + Font.Size + ";FONT-FAMILY: " + Font.Name + ";BORDER-COLLAPSE: collapse");
            writer.AddAttribute("borderColor", "#000000");
            writer.AddAttribute("cellSpacing", "0");
            writer.AddAttribute("cellPadding", "0");
            writer.RenderBeginTag("table");
        }

        private void RenderLeftHandTableEnd(HtmlTextWriter writer)
        {
            writer.RenderEndTag();
        }

        private void RenderLeftHandPaneCaption(HtmlTextWriter writer)
        {
            int height = 0;
            string statCaption = String.Empty;
            int colCount = 0;

            height = Convert.ToInt32(m_cellHeight) * Config.ColTrCount;

            //empty row to match month headers on right hand pane
            writer.RenderBeginTag("tr");

            if (this.AutoNum)
            {
                writer.AddAttribute("rowspan", Config.ColTrCount.ToString());
                writer.AddAttribute("height", height.ToString());
                writer.AddAttribute("align", HorizontalAlign.Center.ToString());
                writer.RenderBeginTag("td nowrap");
                writer.Write("&nbsp;<b>" + this.AutoNumCaption + "</b>&nbsp;");
                writer.RenderEndTag();

                colCount++;
            }

            if(!String.IsNullOrEmpty(m_groupColumn))
            {
                DataTable dt = GetDataTable();
                int groupCont = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (i != 0)
                    {
                        if (String.Compare(dt.Rows[i][m_groupColumn].ToString(), dt.Rows[i - 1][m_groupColumn].ToString()) != 0)
                        {
                            writer.AddAttribute("rowspan", Convert.ToString(groupCont * Config.ColTrCount));
                            writer.AddAttribute("height", Convert.ToString((height * groupCont)));
                            writer.AddAttribute("align", HorizontalAlign.Center.ToString());
                            writer.RenderBeginTag("td nowrap");
                            writer.Write("&nbsp;<b>" + dt.Rows[i - 1][m_groupColumn].ToString() + "</b>&nbsp;");
                            writer.RenderEndTag();

                            groupCont = 1;
                        }
                        else
                        {
                            groupCont++;
                        }
                    }
                    else
                    {
                        if (dt.Rows.Count > 1)
                        {
                            if (String.Compare(dt.Rows[i][m_groupColumn].ToString(), dt.Rows[i + 1][m_groupColumn].ToString()) != 0)
                            {
                                groupCont = 1;

                                writer.AddAttribute("rowspan", Convert.ToString(groupCont * Config.ColTrCount));
                                writer.AddAttribute("height", Convert.ToString((height * groupCont)));
                                writer.AddAttribute("align", HorizontalAlign.Center.ToString());
                                writer.RenderBeginTag("td nowrap");
                                writer.Write("&nbsp;<b>" + dt.Rows[i][m_groupColumn].ToString() + "</b>&nbsp;");
                                writer.RenderEndTag();
                            }
                        }
                        else
                        {
                            groupCont = 1;

                            writer.AddAttribute("rowspan", Convert.ToString(groupCont * Config.ColTrCount));
                            writer.AddAttribute("height", Convert.ToString((height * groupCont)));
                            writer.AddAttribute("align", HorizontalAlign.Center.ToString());
                            writer.RenderBeginTag("td nowrap");
                            writer.Write("&nbsp;<b>" + dt.Rows[i][m_groupColumn].ToString() + "</b>&nbsp;");
                            writer.RenderEndTag();
                        }
                    }
                }

                colCount++;
            }

            foreach (ChartFieldItem fieldItem in this.fields)
            {
                //ColN
                writer.AddAttribute("rowspan", Config.ColTrCount.ToString());
                writer.AddAttribute("height", height.ToString());
                writer.AddAttribute("align", fieldItem.CaptionAlignment.ToString());
                writer.RenderBeginTag("td nowrap");
                writer.Write("&nbsp;<b>" + fieldItem.FieldCaption + "</b>&nbsp;");
                writer.RenderEndTag();
                colCount++;
            }

            switch (m_statType)
            {
                case StatType.預計和實際:
                    statCaption = Config.ColPlanFact;
                    break;
                case StatType.預計:
                    statCaption = Config.ColPlan;
                    break;
                case StatType.實際:
                    statCaption = Config.ColFact;
                    break;
            }

            writer.AddAttribute("rowspan", Config.ColTrCount.ToString());
            writer.AddAttribute("height", height.ToString());
            writer.RenderBeginTag("td nowrap");
            writer.Write("&nbsp;<b>" + statCaption + "</b>&nbsp;");
            writer.RenderEndTag();
            colCount++;

            writer.RenderEndTag();

            if (displayWeekDetail)
            {
                for (int i = 0; i < Config.ColTrCount; i++)
                {
                    writer.RenderBeginTag("tr");
                    writer.RenderEndTag();
                }
                
                writer.RenderBeginTag("tr");

                for (int i = 0; i < colCount; i++)
                {
                    writer.AddAttribute("rowspan", "2");
                    writer.AddAttribute("height", height.ToString());
                    writer.RenderBeginTag("td nowrap");
                    writer.Write("&nbsp;");
                    writer.RenderEndTag();
                }
                writer.RenderEndTag();
            }

            //empty row to match day headers on right hand pane
            writer.RenderBeginTag("tr");
        }

        private void RenderLeftHandPane(HtmlTextWriter writer)
        {
            #region Variable Definition
            int height = 0;
            string groupIndex = String.Empty;
            string blocktext = String.Empty;
            string grouptext = String.Empty;
            XmlDocument xmlDoc = null;
            XmlNodeList xmlRows = null;
            XmlNodeList xmlBlocks = null;
            int rowCount = 0;
            #endregion

            // now need to write the rows
            // Load the XML
            xmlDoc = xmlDocument;

            xmlRows = xmlDoc.SelectNodes("//group");
            for (int i = currentRowCount; i < xmlRows.Count; i++)
            {
                xmlBlocks = xmlRows[i].SelectNodes("block");

                height = Convert.ToInt32(m_cellHeight) * xmlBlocks.Count;

                writer.RenderBeginTag("tr");

                if (this.AutoNum)
                {
                    writer.AddAttribute("bgcolor", m_toggleColor);
                    writer.AddAttribute("align", HorizontalAlign.Center.ToString());
                    writer.AddAttribute("height", height.ToString());
                    writer.AddAttribute("rowspan", xmlBlocks.Count.ToString());
                    writer.RenderBeginTag("td nowrap");
                    writer.Write("&nbsp;<b>" + (i + 1).ToString().PadLeft(this.AutoNumLength, '0') + "</b>&nbsp;");
                    writer.RenderEndTag();
                }

                foreach (ChartFieldItem fieldItem in this.fields)
                {
                    grouptext = xmlRows[i].SelectSingleNode(fieldItem.FieldName).InnerText;
                    
                    writer.AddAttribute("bgcolor", m_toggleColor);
                    writer.AddAttribute("align", fieldItem.FieldAlignment.ToString());
                    writer.AddAttribute("height", height.ToString());
                    writer.AddAttribute("rowspan", xmlBlocks.Count.ToString());
                    writer.AddStyleAttribute("word-break", "break-all"); //自动换行
                    writer.AddAttribute(HtmlTextWriterAttribute.Width, fieldItem.Width.ToString());
                    writer.RenderBeginTag("td nowrap");
                    writer.Write("&nbsp;<b>" + grouptext + "</b>&nbsp;");
                    writer.RenderEndTag();
                }

                int j = 0;
                foreach (XmlNode xmlBlock in xmlBlocks)
                {
                    if (j != 0)
                    {
                        writer.RenderBeginTag("tr");
                    }

                    blocktext = xmlBlock.SelectSingleNode("name").InnerText;

                    //write the activity
                    //writer.AddAttribute("width", "80");
                    writer.AddAttribute("align", "center");
                    writer.AddAttribute("height", m_cellHeight);
                    writer.RenderBeginTag("td nowrap");
                    #region 折行

                    #endregion
                    writer.Write("&nbsp;<b>" + blocktext + "</b>&nbsp;");
                    writer.RenderEndTag();

                    writer.RenderEndTag(); //end tr

                    j++;
                }

                rowCount++;

                if (rowCount == m_headerGap)
                {
                    currentRowCount = rowCount;
                    writer.RenderEndTag();//end tr
                    return;
                }
            }
            writer.RenderEndTag();
        }

        private void RenderRightHandDivStart(HtmlTextWriter writer)
        {
            if (DisplayScrollBar)
            {
                writer.AddAttribute("style", "width:1px; overflow-x:scroll;");
            }
            writer.AddAttribute("id", "divcal");
            writer.RenderBeginTag("div");

            writer.AddAttribute("border", "1");
            writer.AddAttribute("style", "FONT-SIZE: " + Font.Size + ";FONT-FAMILY: " + Font.Name + ";BORDER-COLLAPSE: collapse");
            writer.AddAttribute("borderColor", "#000000");
            writer.AddAttribute("cellSpacing", "0");
            writer.AddAttribute("cellPadding", "0");
            writer.RenderBeginTag("table");

        }

        private void RenderRightHandDivEnd(HtmlTextWriter writer)
        {
            writer.RenderEndTag(); // close table tag
            writer.RenderEndTag(); // close div tag
        }

        private void RenderRightHandPane(HtmlTextWriter writer, GanttDate ganttType)
        {
            switch (ganttType)
	        {
		        case GanttDate.Year:
                    break;
                case GanttDate.Month:
                    break;
                case GanttDate.Week:
                    RenderRightHandPaneByWeek(writer);
                    break;
                case GanttDate.Day:
                    break;
	        }
        }

        private void RenderRightHandPaneByWeek(HtmlTextWriter writer)
        {
            #region Variable Definition
            string blockcolor = String.Empty;
            string blocktext = String.Empty;
            string startdate = String.Empty;
            string enddate = String.Empty;
            int startindex = -1;
            int endindex = -1;
            XmlDocument xmlDoc = null;
            XmlNodeList xmlRows = null;
            XmlNodeList xmlBlocks = null;
            XmlNode node = null;
            int rowCount = 0;
            DateHelper dateHelper = new DateHelper();
            DateTime sDate = new DateTime();
            DateTime eDate = new DateTime();
            int startWeekCount = 1;
            string startYear = "";
            string nextYear = "";
            #endregion

            //write weekly headers
            writer.RenderBeginTag("tr");
            if (string.IsNullOrEmpty(QueryCondition) || dateHelper.GetWeekNumByDate(dateHelper.TryParseExact(QueryCondition)) == 1)
            {
                writer.AddAttribute("height", Convert.ToString(Convert.ToInt32(m_cellHeight) - 1));
                writer.AddAttribute("align", "center");
                writer.AddAttribute("colspan", Config.WeekCount.ToString());
                writer.RenderBeginTag("td nowrap");
                writer.Write(Config.Weekly);
                writer.RenderEndTag();
            }
            else
            {
                startWeekCount = dateHelper.GetWeekNumByDate(dateHelper.TryParseExact(QueryCondition));
                startYear = QueryCondition.Substring(0, 4);
                nextYear = Convert.ToString(Convert.ToInt32(startYear) + 1);
                
                writer.AddAttribute("height", Convert.ToString(Convert.ToInt32(m_cellHeight) - 1));
                writer.AddAttribute("align", "center");
                writer.AddAttribute("colspan", Convert.ToString(Config.WeekCount - startWeekCount + 1));
                writer.RenderBeginTag("td nowrap");
                writer.Write(startYear);
                writer.RenderEndTag();

                writer.AddAttribute("height", Convert.ToString(Convert.ToInt32(m_cellHeight) - 1));
                writer.AddAttribute("align", "center");
                writer.AddAttribute("colspan", Convert.ToString(startWeekCount - 1));
                writer.RenderBeginTag("td nowrap");
                writer.Write(nextYear);
                writer.RenderEndTag();
            }
            writer.RenderEndTag();

            //write week headers
            writer.RenderBeginTag("tr");

            if (string.IsNullOrEmpty(QueryCondition) || dateHelper.GetWeekNumByDate(dateHelper.TryParseExact(QueryCondition)) == 1)
            {
                for (int i = 1; i <= Config.WeekCount; i++)
                {
                    writer.AddAttribute("align", "center");
                    writer.AddAttribute("width", m_cellWidth);
                    writer.AddAttribute("height", m_cellHeight);
                    writer.RenderBeginTag("td nowrap");
                    writer.Write(i.ToString().PadLeft(2, '0'));
                    writer.RenderEndTag();
                }
            }
            else
            {
                for (int i = startWeekCount; i <= Config.WeekCount; i++)
                {
                    writer.AddAttribute("align", "center");
                    writer.AddAttribute("width", m_cellWidth);
                    writer.AddAttribute("height", m_cellHeight);
                    writer.RenderBeginTag("td nowrap");
                    writer.Write(i.ToString().PadLeft(2, '0'));
                    writer.RenderEndTag();
                }

                for (int i = 1; i < startWeekCount; i++)
                {
                    writer.AddAttribute("align", "center");
                    writer.AddAttribute("width", m_cellWidth);
                    writer.AddAttribute("height", m_cellHeight);
                    writer.RenderBeginTag("td nowrap");
                    writer.Write(i.ToString().PadLeft(2, '0'));
                    writer.RenderEndTag();
                }
            }
            writer.RenderEndTag();

            if (displayWeekDetail)
            {
                //DateHelper dHelper = new DateHelper();
                //write week details
                writer.RenderBeginTag("tr");

                if (string.IsNullOrEmpty(QueryCondition) || dateHelper.GetWeekNumByDate(dateHelper.TryParseExact(QueryCondition)) == 1)
                {
                    for (int i = 1; i <= Config.WeekCount; i++)
                    {
                        string weekFirstDay = dateHelper.GetWeekFirstDay(i);
                        string weekLastDay = dateHelper.GetWeekLastDay(i);

                        writer.AddAttribute("align", "center");
                        writer.AddAttribute("width", m_cellWidth);
                        writer.AddAttribute("height", Convert.ToString(Convert.ToInt32(m_cellHeight) * 2 + 1));
                        writer.RenderBeginTag("td nowrap");
                        writer.Write(weekFirstDay + "<br />" + weekLastDay);
                        writer.RenderEndTag();
                    }
                }
                else
                {
                    for (int i = startWeekCount; i <= Config.WeekCount; i++)
                    {
                        string weekFirstDay = dateHelper.GetWeekFirstDay(startYear, i);
                        string weekLastDay = dateHelper.GetWeekLastDay(startYear, i);

                        writer.AddAttribute("align", "center");
                        writer.AddAttribute("width", m_cellWidth);
                        writer.AddAttribute("height", Convert.ToString(Convert.ToInt32(m_cellHeight) * 2 + 1));
                        writer.RenderBeginTag("td nowrap");
                        writer.Write(weekFirstDay + "<br />" + weekLastDay);
                        writer.RenderEndTag();
                    }

                    for (int i = 1; i < startWeekCount; i++)
                    {
                        string weekFirstDay = dateHelper.GetWeekFirstDay(nextYear, i);
                        string weekLastDay = dateHelper.GetWeekLastDay(nextYear, i);

                        writer.AddAttribute("align", "center");
                        writer.AddAttribute("width", m_cellWidth);
                        writer.AddAttribute("height", Convert.ToString(Convert.ToInt32(m_cellHeight) * 2 + 1));
                        writer.RenderBeginTag("td nowrap");
                        writer.Write(weekFirstDay + "<br />" + weekLastDay);
                        writer.RenderEndTag();
                    }
                }
                writer.RenderEndTag();
            }

            // now need to write the rows
            // Load the XML
            xmlDoc = xmlDocument;


            xmlRows = xmlDoc.SelectNodes("//group");
            for (int i = currentRowCount; i < xmlRows.Count; i++)
            //foreach (XmlNode xmlRow in xmlRows)
            {
                node = xmlRows[i].SelectSingleNode("blockcolor");
                if (node != null)
                {
                    blockcolor = node.InnerText;
                }
                else
                {
                    blockcolor = m_factBlockColor;
                };

                // write out the events

                xmlBlocks = xmlRows[i].SelectNodes("block");
                foreach (XmlNode xmlBlock in xmlBlocks)
                {
                    writer.RenderBeginTag("tr");
                    startdate = xmlBlock.SelectSingleNode("StartDate").InnerText;
                    enddate = xmlBlock.SelectSingleNode("EndDate").InnerText;
                    blocktext = xmlBlock.SelectSingleNode("name").InnerText;

                    if (blocktext == StatType.實際.ToString())
                    {
                        blockcolor = m_factBlockColor;
                    }
                    else if (blocktext == StatType.預計.ToString())
                    {
                        blockcolor = m_planBlockColor;
                    }

                    if (!string.IsNullOrEmpty(startdate))
                    {
                        try
                        {
                            sDate = Convert.ToDateTime(startdate);
                        }
                        catch
                        {
                            sDate = dateHelper.TryParseExact(startdate);
                        }
                        startindex = dateHelper.GetWeekNumByDate(sDate);
                    }
                    else
                    {
                        startindex = -1;
                    }

                    if (!string.IsNullOrEmpty(enddate))
                    {
                        try
                        {
                            eDate = Convert.ToDateTime(enddate);
                        }
                        catch
                        {
                            eDate = dateHelper.TryParseExact(enddate);
                        }
                        endindex = dateHelper.GetWeekNumByDate(eDate);
                    }
                    else
                    {
                        endindex = startindex;
                    }

                    if (!string.IsNullOrEmpty(QueryCondition) && dateHelper.GetWeekNumByDate(dateHelper.TryParseExact(QueryCondition)) != 1)
                    {
                        int lastYearCount = 0;
                        if (sDate.Year.ToString() == nextYear)
                        {
                            lastYearCount = Config.WeekCount - startWeekCount + 1;
                        }
                        else
                        {
                            lastYearCount = startWeekCount - 1;
                        }

                        if (!string.IsNullOrEmpty(startdate))
                        {
                            startindex -= lastYearCount;
                        }
                        lastYearCount = 0;

                        if (eDate.Year.ToString() == nextYear)
                        {
                            lastYearCount = Config.WeekCount - startWeekCount + 1;
                        }
                        else
                        {
                            lastYearCount = startWeekCount - 1;
                        }

                        if (!string.IsNullOrEmpty(enddate))
                        {
                            endindex -= lastYearCount;
                        }
                    }

                    if (string.IsNullOrEmpty(startdate) && !string.IsNullOrEmpty(enddate))
                    {
                        startindex = endindex;
                    }

                    if (!string.IsNullOrEmpty(startdate) && string.IsNullOrEmpty(enddate))
                    {
                        endindex = startindex;
                    }

                    if (!string.IsNullOrEmpty(startdate) && string.Compare(startdate, QueryCondition) < 0)
                    {
                        startindex = -1;
                    }

                    if (!string.IsNullOrEmpty(enddate) && string.Compare(enddate, QueryCondition) < 0)
                    {
                        endindex = -1;
                    }

                    //write out the padding cells
                    for (int k = 0; k < startindex - 1; k++)
                    {
                        writer.AddAttribute("width", m_cellWidth);
                        writer.AddAttribute("height", m_cellHeight);
                        writer.RenderBeginTag("td nowrap");
                        writer.Write("&nbsp;");
                        writer.RenderEndTag();
                    };

                    //create the filled in block
                    //if(!string.IsNullOrEmpty(startdate) && !string.IsNullOrEmpty(enddate))
                    if (endindex != -1 && startindex != -1)
                    {
                        writer.AddAttribute("colspan", (endindex - startindex + 1).ToString());
                        writer.AddAttribute("bgColor", blockcolor);
                        writer.AddAttribute("width", m_cellWidth);
                        writer.AddAttribute("height", m_cellHeight);
                        writer.RenderBeginTag("td nowrap");
                        writer.Write("&nbsp;");
                        writer.RenderEndTag();
                    }

                    int endCount = 0;
                    endCount = Config.WeekCount - 1;

                    if (endindex == -1 && startindex == -1)
                    {
                        endindex = 0;
                    }

                    //write out the padding cells
                    for (int k = endindex; k < endCount + 1; k++)
                    {
                        writer.AddAttribute("width", m_cellWidth);
                        writer.AddAttribute("height", m_cellHeight);
                        writer.RenderBeginTag("td nowrap");
                        writer.Write("&nbsp;");
                        writer.RenderEndTag();
                    };
                    writer.RenderEndTag();
                }

                rowCount++;

                if (rowCount == m_headerGap)
                {
                    currentRowCount = rowCount;
                    return;
                }
            }
        }
        #endregion

        #region Helper Classes
        private class QuarterHelper
        {
            private int m_year;
            private int m_quarter;
            private string[] m_names = new string[3];
            private int m_NoOfDays;

            public QuarterHelper()
            { }

            public QuarterHelper(int year, int quarter)
            {
                m_quarter = quarter;
                m_year = year;
                m_names = getQuarterNames();
                m_NoOfDays = getDaysInQuarter(m_year, m_quarter);
            }

            public int Year
            {
                get
                {
                    return m_year;
                }
            }

            public int QuarterIndex
            {
                get
                {
                    return m_quarter;
                }
            }

            public string[] Names
            {
                get
                {
                    return m_names;
                }
            }

            public int Days
            {
                get
                {
                    return m_NoOfDays;
                }
            }

            public string GetMonthName(int i)
            {
                return m_names[i - 1];
            }

            public int TotalDays()
            {
                int retval = 0;
                switch (m_quarter)
                {
                    case 1:
                        retval = DateTime.DaysInMonth(m_year, 1);
                        retval += DateTime.DaysInMonth(m_year, 2);
                        retval += DateTime.DaysInMonth(m_year, 3);
                        break;
                    case 2:
                        retval = DateTime.DaysInMonth(m_year, 4);
                        retval += DateTime.DaysInMonth(m_year, 5);
                        retval += DateTime.DaysInMonth(m_year, 6);
                        break;
                    case 3:
                        retval = DateTime.DaysInMonth(m_year, 7);
                        retval += DateTime.DaysInMonth(m_year, 8);
                        retval += DateTime.DaysInMonth(m_year, 9);
                        break;
                    case 4:
                        retval = DateTime.DaysInMonth(m_year, 10);
                        retval += DateTime.DaysInMonth(m_year, 11);
                        retval += DateTime.DaysInMonth(m_year, 12);
                        break;
                }
                return retval;
            }

            public int TotalDaysInMonth(int i)
            {
                return DateTime.DaysInMonth(m_year, i);
            }

            public string GetDayName(int month, int day)
            {
                string retval = string.Empty;
                DateTime d = new DateTime(m_year, month, day);
                switch (d.DayOfWeek)
                {
                    case DayOfWeek.Monday: retval = "M"; break;
                    case DayOfWeek.Tuesday: retval = "T"; break;
                    case DayOfWeek.Wednesday: retval = "W"; break;
                    case DayOfWeek.Thursday: retval = "T"; break;
                    case DayOfWeek.Friday: retval = "F"; break;
                    case DayOfWeek.Saturday: retval = "S"; break;
                    case DayOfWeek.Sunday: retval = "S"; break;
                }
                return retval;
            }

            private string[] getQuarterNames()
            {
                string[] retval = new string[3];

                switch (m_quarter)
                {
                    case 1:
                        retval[0] = "January";
                        retval[1] = "Febuary";
                        retval[2] = "March";
                        break;
                    case 2:
                        retval[0] = "April";
                        retval[1] = "May";
                        retval[2] = "June";
                        break;
                    case 3:
                        retval[0] = "July";
                        retval[1] = "August";
                        retval[2] = "September";
                        break;
                    case 4:
                        retval[0] = "October";
                        retval[1] = "November";
                        retval[2] = "December";
                        break;
                }

                return retval;

            }

            private int getDaysInQuarter(int year, int quarter)
            {
                DateTime dtS, dtE;
                if (quarter < 4)
                {
                    dtS = new DateTime(year, (3 * quarter - 2), 1);
                    dtE = new DateTime(year, (3 * quarter - 2) + 3, 1);
                }
                else
                {
                    dtS = new DateTime(year, (3 * quarter - 2), 1);
                    dtE = new DateTime(year + 1, 1, 1);
                }

                TimeSpan ts = new TimeSpan(dtE.Subtract(dtS).Ticks);
                return ts.Days;
            }

            public int getColumnIndex(string day)
            {
                DateTime dt = DateTime.Parse(day);
                int offset = 0;
                int retval = 0;
                for (int i = 1; i < m_quarter; i++)
                {
                    offset += getDaysInQuarter(m_year, i);
                }
                retval = (dt.DayOfYear - 1) - offset;
                if (retval < 0) retval = 0;
                if (retval > getDaysInQuarter(m_year, m_quarter)) retval = getDaysInQuarter(m_year, m_quarter);
                return retval;
            }
        }
        #endregion

       
    }

    internal class CharHelper
    {
        public bool IsChinese(string value)
        {
            if (Regex.IsMatch(value, "^[\u4e00-\u9fa5]"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string GetFormatValue(string value)
        {
            string formatedValue = String.Empty;



            return formatedValue;
        }
    }

    internal class DateHelper
    {
        private CultureInfo cultureInfo;

        public DateHelper()
        {
            cultureInfo = CultureInfo.CurrentCulture;
        }

        public int GetWeekNumByDate(DateTime dateTime)
        {
            return cultureInfo.Calendar.GetWeekOfYear(dateTime, Config.calendarWeekRule, Config.dayOfWeek);
        }

        public int GetWeekCount()
        {
            //int strYear = DateTime.Now.Year;

            //System.DateTime fDt = DateTime.Parse(strYear.ToString() + "-01-01"); 
            //int k = Convert.ToInt32(fDt.DayOfWeek); 
            //if (k == 1) 
            //{ 
            //    int countDay = fDt.AddYears(1).AddDays(-1).DayOfYear; 
            //    int countWeek = countDay / 7 + 1; 
            //    return countWeek; 

            //} 
            //else 
            //{ 
            //    int countDay = fDt.AddYears(1).AddDays(-1).DayOfYear; 
            //    int countWeek = countDay / 7 + 2; 
            //    return countWeek; 
            //} 

            int year = DateTime.Now.Year;

            //The first day of this year
            DateTime firstDate = new DateTime(year, 1, 1);
            //The last day of this year
            DateTime lastDate = new DateTime(year, 12, 31);

            //The week of the firstDate
            int firstWeek = Convert.ToInt32(firstDate.DayOfWeek);
            //The week of the lastDate
            int lastWeek = Convert.ToInt32(lastDate.DayOfWeek);

            //Calculate days of the firstWeek and the lastWeek
            int extraDays = 7 - firstWeek + lastWeek;
            int mainDays = 365 - extraDays;

            //If leap year,mainDays + 1
            int judge = year % 4;
            if (judge == 0) { mainDays++; }

            int mainWeeks = mainDays / 7;
            int weeks = 2 + mainWeeks;

            return weeks;
        }

        public DateTime TryParseExact(string dateValue)
        {
            DateTime date = new DateTime();
            DateTime.TryParseExact(dateValue, "yyyyMMdd", cultureInfo, DateTimeStyles.None, out date);
            return date;
        }

        public string GetWeekFirstDay(int weekNum)
        {
            return GetWeekFirstDay(DateTime.Now.Year.ToString(), weekNum);
        }

        public string GetWeekFirstDay(string yearNum, int weekNum)
        {
            int year = Convert.ToInt32(yearNum);
            DateTime newYearDay = new DateTime(year, 1, 1);
            int firstweekfirstday = Convert.ToInt32(newYearDay.DayOfWeek);
            int days = (int)(7 - firstweekfirstday);
            DateTime secondweekfisrtday = newYearDay.AddDays(days);
            DateTime mFirstdate = secondweekfisrtday.AddDays((weekNum - 2) * 7);
            string firstdate = mFirstdate.Month + "/" + mFirstdate.Day;
            return firstdate;
        }

        public string GetWeekLastDay(int weekNum)
        {
            return GetWeekLastDay(DateTime.Now.Year.ToString(), weekNum);
        }

        public string GetWeekLastDay(string yearNum, int weekNum)
        {
            int year = Convert.ToInt32(yearNum);
            DateTime newYearDay = new DateTime(year, 1, 1);
            int firstweekfirstday = Convert.ToInt32(newYearDay.DayOfWeek);
            int days = (int)(7 - firstweekfirstday);
            DateTime secondweekfisrtday = newYearDay.AddDays(days);
            DateTime mLastdate = secondweekfisrtday.AddDays((weekNum - 2) * 7 + 6);
            string lastdate = mLastdate.Month + "/" + mLastdate.Day;
            return lastdate;
        }
    }

    internal class Config
    {
        private static DateHelper dateHelper = new DateHelper();
        private static int weekCount = dateHelper.GetWeekCount();

        static Config()
        {
            
        }
        
        public const string ColPlanFact = "進度";
        public const string ColPlan = "預計";
        public const string ColFact = "實際";
        public const int ColTrCount = 2;
        public const string Weekly = "周別";
        public static int WeekCount = weekCount;
        public const CalendarWeekRule calendarWeekRule = CalendarWeekRule.FirstDay;
        public const DayOfWeek dayOfWeek = DayOfWeek.Sunday;
        public const string CompnentName = "WebCalendarChart";
        public const string EnterTag = "<br />";
    }

    public enum GanttDate
    { 
        Year,
        Month,
        Week,
        Day
    }

    public class ChartFieldItem : InfoOwnerCollectionItem, IGetValues
    {
        public override string ToString()
        {
            return name;
        }
        
        //private void SetPropertyReadOnly(object obj, String propertyName, String value) 
        //{ 
        //    PropertyDescriptorCollection props = TypeDescriptor.GetProperties(obj); 
        //    props[propertyName].SetValue(this, value); 
        //}

        public ChartFieldItem()
        {
            captionAlignment = HorizontalAlign.Center;
            fieldAlignment = HorizontalAlign.Center;
        }
        
        private string name;
        [NotifyParentProperty(true)]
        public override string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }
        
        private string fieldName;
        [Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
        [NotifyParentProperty(true)]
        public string FieldName
        {
            get
            {
                return fieldName;
            }
            set
            {
                fieldName = value;
                if (string.IsNullOrEmpty(FieldCaption))
                {
                    FieldCaption = fieldName;
                }
            }
        }

        private string fieldCaption;
        [NotifyParentProperty(true)]
        public string FieldCaption
        {
            get
            {
                return fieldCaption; 
            }
            set 
            {
                if (value != null && value != String.Empty)
                {
                    fieldCaption = value;
                    name = fieldCaption;
                }
                else
                {
                    if (name != null && name != String.Empty)
                    {
                        fieldCaption = fieldName;
                        name = fieldCaption;
                    }
                    else
                    {
                        name = Config.CompnentName;
                    }
                }
            }
        }

        private HorizontalAlign captionAlignment;
        [NotifyParentProperty(true)]
        public HorizontalAlign CaptionAlignment
        {
            get { return captionAlignment; }
            set { captionAlignment = value; }
        }

        private HorizontalAlign fieldAlignment;
        [NotifyParentProperty(true)]
        public HorizontalAlign FieldAlignment
        {
            get { return fieldAlignment; }
            set { fieldAlignment = value; }
        }

        private int width;
        [NotifyParentProperty(true)]
        public int Width
        {
            get
            {
                return width;
            }
            set
            {
                width = value;
            }
        }

        #region IGetValues
        public string[] GetValues(string sKind)
        {
            string[] retList = null;
            List<string> values = new List<string>();
            if (string.Compare(sKind, "fieldname", true) == 0)//IgnoreCase
            {
                if (this.Owner is WebCalendarChart)
                {
                    WebCalendarChart wv = (WebCalendarChart)this.Owner;
                    if (wv.Page != null && wv.DataSourceID != null && wv.DataSourceID != "")
                    {
                        foreach (Control ctrl in wv.Page.Controls)
                        {
                            if (ctrl is WebDataSource && ((WebDataSource)ctrl).ID == wv.DataSourceID)
                            {
                                WebDataSource ds = (WebDataSource)ctrl;
                                if (ds.DesignDataSet == null)
                                {
                                    WebDataSet wds = WebDataSet.CreateWebDataSet(ds.WebDataSetID);
                                    if (wds != null)
                                    {
                                        ds.DesignDataSet = wds.RealDataSet;
                                    }
                                }
                                if (ds.DesignDataSet != null && ds.DesignDataSet.Tables.Contains(ds.DataMember))
                                {
                                    foreach (DataColumn column in ds.DesignDataSet.Tables[ds.DataMember].Columns)
                                    {
                                        values.Add(column.ColumnName);
                                    }
                                }
                                break;
                            }
                        }
                        if (values.Count > 0)
                        {
                            int i = values.Count;
                            retList = new string[i];
                            for (int j = 0; j < i; j++)
                            {
                                retList[j] = values[j];
                            }
                        }
                    }
                }
            }
            return retList;
        }

        #endregion
    }

    public class ChartFieldItemCollection : InfoOwnerCollection
    {
        public ChartFieldItemCollection(Object aOwner, Type aItemType)
            : base(aOwner, typeof(ChartFieldItem))
        {
        }

        public new ChartFieldItem this[int index]
        {
            get
            {
                return (ChartFieldItem)InnerList[index];
            }
            set
            {
                if (index > -1 && index < Count)
                {
                    if (value is ChartFieldItem)
                    {
                        //原来的Collection设置为0
                        ((ChartFieldItem)InnerList[index]).Collection = null;
                        InnerList[index] = value;
                        //Collection设置为this
                        ((ChartFieldItem)InnerList[index]).Collection = this;
                    }
                }
            }
        }
    }

    public class ChartDataSourceEditor : UITypeEditor
    {
        public ChartDataSourceEditor()
            : base()
        {
        }

        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand)]
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand)]
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            List<string> objName = new List<string>();
            if (context.Instance is WebCalendarChart)
            {
                ControlCollection ctrlList = ((WebCalendarChart)context.Instance).Page.Controls;
                foreach (Control ctrl in ctrlList)
                {
                    if (ctrl is WebDataSource)
                    {
                        objName.Add(ctrl.ID);
                    }
                }
            }
            IWindowsFormsEditorService service = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (service != null)
            {
                StringListSelector selector = new StringListSelector(service, objName.ToArray());
                string strValue = (string)value;
                if (selector.Execute(ref strValue)) value = strValue;
            }
            return value;
        }
    }

    public enum StatType
    { 
        預計和實際,
        預計,
        實際
    }

   
}
