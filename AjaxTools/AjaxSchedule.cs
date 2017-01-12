using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.ComponentModel;
using System.Drawing.Design;
using System.ComponentModel.Design;
using Srvtools;
using System.Web.UI.HtmlControls;

namespace AjaxTools
{
    [ParseChildren(true)]
    [PersistChildren(false)]
    [Designer(typeof(AjaxScheduleDesigner), typeof(IDesigner))]
    public class AjaxSchedule : AjaxBaseControl, IAjaxDataSource
    {
        int _firstHour = 6;
        int _minScale = 30;
        bool _editable = true;
        bool _dragable = true;
        //bool _resizeable = true;
        bool _weekends = true;
        string _renderContainer = "";
        string _dataSourceId = "";
        string _idField = "";
        string _captionField = "";
        string _startDateTimeField = "";
        string _endDateTimeField = "";
        string _descriptionField = "";
        string _allDayField = "";
        string _allDayText = "";
        AjaxTheme _theme = AjaxTheme.Sunny;
        AjaxScheduleWeekMode _weekMode = AjaxScheduleWeekMode.Fixed;
        AjaxScheduleDefaultView _defView = AjaxScheduleDefaultView.Month;
        AjaxScheduleTitle _scheduleTitle = null;
        AjaxScheduleColumn _scheduleColumn = null;
        AjaxScheduleTime _scheduleTime = null;
        AjaxScheduleToolItemCollection _scheduleButtons;

        #region Properties
        [Category("Infolight")]
        [DefaultValue(6)]
        public int FirstHour
        {
            get { return _firstHour; }
            set { _firstHour = value; }
        }

        [Category("Infolight")]
        [DefaultValue(30)]
        public int MinuteScale
        {
            get { return _minScale; }
            set { _minScale = value; }
        }

        [Category("Infolight")]
        [DefaultValue(true)]
        public bool Editable
        {
            get { return _editable; }
            set { _editable = value; }
        }

        [Category("Infolight")]
        [DefaultValue(true)]
        public bool Dragable
        {
            get { return _dragable; }
            set { _dragable = value; }
        }

        //[Category("Infolight")]
        //[DefaultValue(true)]
        //public bool Resizeable
        //{
        //    get { return _resizeable; }
        //    set { _resizeable = value; }
        //}

        [Category("Infolight")]
        [DefaultValue(true)]
        public bool Weekends
        {
            get { return _weekends; }
            set { _weekends = value; }
        }

        [Category("Infolight")]
        [DefaultValue("")]
        public string RenderContainer
        {
            get { return _renderContainer; }
            set { _renderContainer = value; }
        }

        [Category("Data")]
        [DefaultValue("")]
        [Editor(typeof(DataSourceEditor), typeof(UITypeEditor))]
        public string DataSourceID
        {
            get { return _dataSourceId; }
            set { _dataSourceId = value; }
        }

        [Category("Data")]
        [DefaultValue("")]
        [Editor(typeof(StringEditor), typeof(UITypeEditor))]
        public string IdField
        {
            get { return _idField; }
            set { _idField = value; }
        }

        [Category("Data")]
        [DefaultValue("")]
        [Editor(typeof(StringEditor), typeof(UITypeEditor))]
        public string CaptionField
        {
            get { return _captionField; }
            set { _captionField = value; }
        }

        [Category("Data")]
        [DefaultValue("")]
        [Editor(typeof(StringEditor), typeof(UITypeEditor))]
        public string StartDateTimeField
        {
            get { return _startDateTimeField; }
            set { _startDateTimeField = value; }
        }

        [Category("Data")]
        [DefaultValue("")]
        [Editor(typeof(StringEditor), typeof(UITypeEditor))]
        public string EndDateTimeField
        {
            get { return _endDateTimeField; }
            set { _endDateTimeField = value; }
        }
        [Category("Data")]
        [DefaultValue("")]
        [Editor(typeof(StringEditor), typeof(UITypeEditor))]
        public string DescriptionField
        {
            get { return _descriptionField; }
            set { _descriptionField = value; }
        }

        [Category("Data")]
        [DefaultValue("")]
        [Editor(typeof(StringEditor), typeof(UITypeEditor))]
        public string AllDayField
        {
            get { return _allDayField; }
            set { _allDayField = value; }
        }

        [Category("Infolight")]
        [DefaultValue("")]
        public string AllDayText
        {
            get { return _allDayText; }
            set { _allDayText = value; }
        }

        [Category("Infolight")]
        [DefaultValue(typeof(AjaxTheme), "Sunny")]
        public AjaxTheme Theme
        {
            get { return _theme; }
            set { _theme = value; }
        }

        [Category("Infolight")]
        [DefaultValue(typeof(AjaxScheduleWeekMode), "Fixed")]
        public AjaxScheduleWeekMode WeekMode
        {
            get { return _weekMode; }
            set { _weekMode = value; }
        }

        [Category("Infolight")]
        [DefaultValue(typeof(AjaxScheduleDefaultView), "Month")]
        public AjaxScheduleDefaultView DefaultView
        {
            get { return _defView; }
            set { _defView = value; }
        }

        [Category("Infolight")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [NotifyParentProperty(true)]
        public AjaxScheduleTitle ScheduleTitleSet
        {
            get
            {
                if (_scheduleTitle == null)
                {
                    _scheduleTitle = new AjaxScheduleTitle(this);
                }
                return _scheduleTitle;
            }
            set { _scheduleTitle = value; }
        }

        [Category("Infolight")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [NotifyParentProperty(true)]
        public AjaxScheduleColumn ScheduleColumnSet
        {
            get
            {
                if (_scheduleColumn == null)
                {
                    _scheduleColumn = new AjaxScheduleColumn(this);
                }
                return _scheduleColumn;
            }
            set { _scheduleColumn = value; }
        }

        [Category("Infolight")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [NotifyParentProperty(true)]
        public AjaxScheduleTime ScheduleTimeSet
        {
            get
            {
                if (_scheduleTime == null)
                {
                    _scheduleTime = new AjaxScheduleTime(this);
                }
                return _scheduleTime;
            }
            set { _scheduleTime = value; }
        }

        [Category("Infolight")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [TypeConverter(typeof(CollectionConverter))]
        [NotifyParentProperty(true)]
        public AjaxScheduleToolItemCollection ScheduleButtons
        {
            get
            {
                if (_scheduleButtons == null)
                    _scheduleButtons = new AjaxScheduleToolItemCollection(this, typeof(AjaxScheduleToolItem));
                return _scheduleButtons;
            }
        }
        #endregion

        public void RenderSchedule()
        {
            if (!string.IsNullOrEmpty(this.RenderContainer))
            {
                string script = string.Format("$.infoReady(renderSchedule, {0});", this.GenScheduleParamters());
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), script, true);
            }
        }

        public string GenScheduleParamters()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("{");
            if (!string.IsNullOrEmpty(this.RenderContainer))
            {
                builder.AppendFormat("container:'{0}',", this.RenderContainer);
            }
            if (!string.IsNullOrEmpty(this.DataSourceID))
            {
                WebDataSource wds = this.GetObjByID(this.DataSourceID) as WebDataSource;
                if (wds != null)
                {
                    string remodeName = wds.RemoteName;
                    string[] uiTexts = SysMsg.GetSystemMessage(CliUtils.fClientLang, "AjaxTools", "AjaxSchedule", "UIText", true).Split(',');
                    builder.AppendFormat("module:'{0}',cacheDataSet:'{1}',command:'{2}',idField:'{3}',titleField:'{4}',descriptionField:'{5}',startField:'{6}',endField:'{7}',allDayField:'{8}',winTitle:'{9}',titleCaption:'{10}',descriptionCaption:'{11}',allDayCaption:'{12}',startCaption:'{13}',endCaption:'{14}',submitCaption:'{15}',cancelCaption:'{16}',deleteCaption:'{17}',", 
                        remodeName.Split('.')[0],
                        this.Page.AppRelativeVirtualPath + this.DataSourceID,
                        wds.DataMember,
                        this.IdField,
                        this.CaptionField,
                        this.DescriptionField,
                        this.StartDateTimeField,
                        this.EndDateTimeField,
                        this.AllDayField,
                        uiTexts[0],
                        uiTexts[1],
                        uiTexts[2],
                        uiTexts[3],
                        uiTexts[4],
                        uiTexts[5],
                        uiTexts[6],
                        uiTexts[7],
                        uiTexts[8]);
                }
            }
            builder.AppendFormat("scheduleConfig:{0}", this.GenScheduleConfig());
            builder.Append("}");
            return builder.ToString();
        }

        public string GenScheduleConfig()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("{");
            if (this.ScheduleButtons.Count > 0 || this.ScheduleTitleSet.ShowTitle)
            {
                //header
                builder.Append("header:{");
                builder.AppendFormat("{0},", this.GenLeftButtons(this.ScheduleButtons));
                if (this.ScheduleTitleSet.ShowTitle)
                {
                    builder.AppendFormat("center:'title',");
                }
                builder.Append(this.GenRightButtons(this.ScheduleButtons));
                if (builder.ToString().EndsWith(","))
                {
                    builder.Remove(builder.Length - 1, 1);
                }
                builder.Append("},");
                //buttonText
                builder.AppendFormat("buttonText:{0},", this.GenButtonText(this.ScheduleButtons));
                //timeFormat
                builder.AppendFormat("timeFormat:{{month:'{0}',week:'{1}',day:'{2}'}},",
                    this.ScheduleTimeSet.MonthFormat,
                    this.ScheduleTimeSet.WeekFormat,
                    this.ScheduleTimeSet.DayFormat);
                //columnFormat
                builder.AppendFormat("columnFormat:{{month:'{0}',week:'{1}',day:'{2}'}},",
                    this.ScheduleColumnSet.MonthFormat,
                    this.ScheduleColumnSet.WeekFormat,
                    this.ScheduleColumnSet.DayFormat);
                //titleFormat
                builder.AppendFormat("titleFormat:{{month:'{0}',week:'{1}',day:'{2}'}},",
                    this.ScheduleTitleSet.MonthFormat,
                    this.ScheduleTitleSet.WeekFormat,
                    this.ScheduleTitleSet.DayFormat);
                //editable
                if (this.Editable)
                {
                    builder.Append("editable:true,");
                }
                //disableDragging
                if (!this.Dragable)
                {
                    builder.Append("disableDragging:true,");
                }
                //disableResizing
                //if (!this.Resizeable)
                //{
                //    builder.Append("disableResizing:true,");
                //}
                //weekends
                if (!this.Weekends)
                {
                    builder.Append("weekends:false,");
                }
                //weekMode
                switch (this.WeekMode)
                {
                    case AjaxScheduleWeekMode.Fixed:
                        break;
                    case AjaxScheduleWeekMode.Liquid:
                        builder.Append("weekMode:'liquid',");
                        break;
                    case AjaxScheduleWeekMode.Variable:
                        builder.Append("weekMode:'variable',");
                        break;
                }
                //defaultView
                switch (this.DefaultView)
                {
                    case AjaxScheduleDefaultView.Month:
                        break;
                    case AjaxScheduleDefaultView.Week:
                        builder.Append("defaultView:'agendaWeek',");
                        break;
                    case AjaxScheduleDefaultView.Day:
                        builder.Append("defaultView:'agendaDay',");
                        break;
                }
                //allDayText
                if (!string.IsNullOrEmpty(this.AllDayText))
                {
                    builder.AppendFormat("allDayText:'{0}',", this.AllDayText);
                }
                //firstHour,slotMinutes
                builder.AppendFormat("firstHour:{0},slotMinutes:{1}", 
                    this.FirstHour,
                    this.MinuteScale);
            }
            builder.Append("}");
            return builder.ToString();
        }

        string GenLeftButtons(AjaxScheduleToolItemCollection tools)
        {
            StringBuilder builder = new StringBuilder();
            bool hasPos = false;
            foreach (AjaxScheduleToolItem item in tools)
            {
                if ((item.ButtonType == AjaxScheduleButtonType.PreviousYear
                    || item.ButtonType == AjaxScheduleButtonType.Previous
                    || item.ButtonType == AjaxScheduleButtonType.NextYear
                    || item.ButtonType == AjaxScheduleButtonType.Next) && item.Visible)
                {
                    if (!hasPos)
                    {
                        builder.Append("left:'");
                    }
                    switch (item.ButtonType)
                    {
                        case AjaxScheduleButtonType.PreviousYear:
                            builder.Append("prevYear ");
                            break;
                        case AjaxScheduleButtonType.Previous:
                            bool nextExist = false;
                            foreach (AjaxScheduleToolItem xItem in tools)
                            {
                                if (xItem.ButtonType == AjaxScheduleButtonType.Next)
                                {
                                    nextExist = true;
                                }
                            }
                            if (nextExist)
                            {
                                builder.Append("prev,");
                            }
                            else
                            {
                                builder.Append("prev ");
                            }
                            break;
                        case AjaxScheduleButtonType.NextYear:
                            builder.Append("nextYear ");
                            break;
                        case AjaxScheduleButtonType.Next:
                            builder.Append("next ");
                            break;
                        case AjaxScheduleButtonType.Today:
                            builder.Append("today ");
                            break;
                    }
                    hasPos = true;
                }
            }
            if (builder.ToString().EndsWith(" "))
            {
                builder.Remove(builder.Length - 1, 1);
            }
            if (hasPos)
            {
                builder.Append("'");
            }
            return builder.ToString();
        }

        string GenRightButtons(AjaxScheduleToolItemCollection tools)
        {
            StringBuilder builder = new StringBuilder();
            bool hasPos = false;
            foreach (AjaxScheduleToolItem item in tools)
            {
                if ((item.ButtonType == AjaxScheduleButtonType.Month
                    || item.ButtonType == AjaxScheduleButtonType.Week
                    || item.ButtonType == AjaxScheduleButtonType.Day) && item.Visible)
                {
                    if (!hasPos)
                    {
                        builder.Append("right:'");
                    }
                    switch (item.ButtonType)
                    {
                        case AjaxScheduleButtonType.Month:
                            builder.Append("month,");
                            break;
                        case AjaxScheduleButtonType.Week:
                            builder.Append("agendaWeek,");
                            break;
                        case AjaxScheduleButtonType.Day:
                            builder.Append("agendaDay,");
                            break;
                    }
                    hasPos = true;
                }
            }
            if (builder.ToString().EndsWith(","))
            {
                builder.Remove(builder.Length - 1, 1);
            }
            if (hasPos)
            {
                builder.Append("',");
            }
            return builder.ToString();
        }

        string GenButtonText(AjaxScheduleToolItemCollection tools)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("{");
            foreach (AjaxScheduleToolItem item in tools)
            {
                if (item.Visible)
                {
                    switch (item.ButtonType)
                    {
                        case AjaxScheduleButtonType.PreviousYear:
                            builder.AppendFormat("prevYear:'{0}',", item.ButtonText);
                            break;
                        case AjaxScheduleButtonType.Previous:
                            builder.AppendFormat("prev:'{0}',", item.ButtonText);
                            break;
                        case AjaxScheduleButtonType.NextYear:
                            builder.AppendFormat("nextYear:'{0}',", item.ButtonText);
                            break;
                        case AjaxScheduleButtonType.Next:
                            builder.AppendFormat("next:'{0}',", item.ButtonText);
                            break;
                        case AjaxScheduleButtonType.Today:
                            builder.AppendFormat("today:'{0}',", item.ButtonText);
                            break;
                        case AjaxScheduleButtonType.Month:
                            builder.AppendFormat("month:'{0}',", item.ButtonText);
                            break;
                        case AjaxScheduleButtonType.Week:
                            builder.AppendFormat("week:'{0}',", item.ButtonText);
                            break;
                        case AjaxScheduleButtonType.Day:
                            builder.AppendFormat("day:'{0}',", item.ButtonText);
                            break;
                    }
                }
            }
            if (builder.ToString().EndsWith(","))
            {
                builder.Remove(builder.Length - 1, 1);
            }
            builder.Append("}");
            return builder.ToString();
        }

        string VirtualPath()
        {
            string subPath = "";
            for (int i = 0; i < this.Page.Request.FilePath.Split('/').Length - 3; i++)
            {
                subPath += "../";
            }
            return subPath;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            RenderSchedule();
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            HtmlLink cssThemeLink = new HtmlLink();
            cssThemeLink.Href = this.VirtualPath() + string.Format("JQuery/themes/{0}/ui.theme.css", this.Theme);
            cssThemeLink.Attributes.Add("rel", "stylesheet");
            cssThemeLink.Attributes.Add("type", "text/css");
            this.Page.Header.Controls.Add(cssThemeLink);

            HtmlLink cssCalendarLink = new HtmlLink();
            cssCalendarLink.Href = this.VirtualPath() + "JQuery/FullCalendar/fullcalendar.css";
            cssCalendarLink.Attributes.Add("rel", "stylesheet");
            cssCalendarLink.Attributes.Add("type", "text/css");
            this.Page.Header.Controls.Add(cssCalendarLink);
        }
    }

    public class AjaxScheduleToolItemCollection : InfoOwnerCollection
    {
        public AjaxScheduleToolItemCollection(object owner, Type itemType)
            : base(owner, typeof(AjaxScheduleToolItem))
        {
        }

        public new AjaxScheduleToolItem this[int index]
        {
            get
            {
                return (AjaxScheduleToolItem)InnerList[index];
            }
            set
            {
                if (index > -1 && index < Count)
                {
                    if (value is AjaxScheduleToolItem)
                    {
                        ((AjaxScheduleToolItem)InnerList[index]).Collection = null;
                        InnerList[index] = value;
                        ((AjaxScheduleToolItem)InnerList[index]).Collection = this;
                    }
                }
            }
        }
    }

    public class AjaxScheduleToolItem : InfoOwnerCollectionItem
    {
        bool _visible = true;
        string _buttonText = "";
        //AjaxScheduleButtonPosition _buttonPos = AjaxScheduleButtonPosition.Left;
        AjaxScheduleButtonType _buttonType = AjaxScheduleButtonType.Today;

        [Category("Infolight")]
        [DefaultValue(true)]
        [NotifyParentProperty(true)]
        public bool Visible
        {
            get { return _visible; }
            set { _visible = value; }
        }

        [Category("Infolight")]
        [DefaultValue("")]
        [NotifyParentProperty(true)]
        public string ButtonText
        {
            get { return _buttonText; }
            set { _buttonText = value; }
        }

        //[Category("Infolight")]
        //[DefaultValue(typeof(AjaxScheduleButtonPosition), "Left")]
        //[NotifyParentProperty(true)]
        //public AjaxScheduleButtonPosition ButtonPosition
        //{
        //    get { return _buttonPos; }
        //    set { _buttonPos = value; }
        //}

        [Category("Infolight")]
        [DefaultValue(typeof(AjaxScheduleButtonType), "Today")]
        [NotifyParentProperty(true)]
        public AjaxScheduleButtonType ButtonType
        {
            get { return _buttonType; }
            set { _buttonType = value; }
        }

        [NotifyParentProperty(true)]
        public override string Name
        {
            get 
            { 
                return _buttonType.ToString();
            }
            set 
            {
                if (value == "PreviousYear")
                {
                    _buttonType = AjaxScheduleButtonType.PreviousYear;
                }
                else if (value == "Previous")
                {
                    _buttonType = AjaxScheduleButtonType.Previous;
                }
                else if (value == "NextYear")
                {
                    _buttonType = AjaxScheduleButtonType.NextYear;
                }
                else if (value == "Next")
                {
                    _buttonType = AjaxScheduleButtonType.Next;
                }
                else if (value == "Today")
                {
                    _buttonType = AjaxScheduleButtonType.Today;
                }
                else if (value == "Month")
                {
                    _buttonType = AjaxScheduleButtonType.Month;
                }
                else if (value == "Week")
                {
                    _buttonType = AjaxScheduleButtonType.Week;
                }
                else if (value == "Day")
                {
                    _buttonType = AjaxScheduleButtonType.Day;
                }
            }
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class AjaxScheduleTitle : IChildSet
    {
        public AjaxScheduleTitle(AjaxBaseControl parent)
        {
            _ownerView = parent;
        }

        bool _showTitle = true;
        string _monthTitleFormat = "MMMM yyyy";
        string _weekTitleFormat = "MMM d[ yyyy]{ - [ MMM] d yyyy}";
        string _dayTitleFormat = "dddd, MMM d, yyyy";
        AjaxBaseControl _ownerView;

        [DefaultValue(true)]
        [NotifyParentProperty(true)]
        public bool ShowTitle
        {
            get { return _showTitle; }
            set { _showTitle = value; }
        }

        [DefaultValue("MMMM yyyy")]
        [NotifyParentProperty(true)]
        public string MonthFormat
        {
            get { return _monthTitleFormat; }
            set { _monthTitleFormat = value; }
        }

        [DefaultValue("MMM d[ yyyy]{ - [ MMM] d yyyy}")]
        [NotifyParentProperty(true)]
        public string WeekFormat
        {
            get { return _weekTitleFormat; }
            set { _weekTitleFormat = value; }
        }

        [DefaultValue("dddd, MMM d, yyyy")]
        [NotifyParentProperty(true)]
        public string DayFormat
        {
            get { return _dayTitleFormat; }
            set { _dayTitleFormat = value; }
        }

        [Browsable(false)]
        public AjaxBaseControl OwnerView
        {
            get { return _ownerView; }
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class AjaxScheduleColumn : IChildSet
    {
        public AjaxScheduleColumn(AjaxBaseControl parent)
        {
            _ownerView = parent;
        }

        string _monthColumnFormat = "ddd";
        string _weekColumnFormat = "ddd M/d";
        string _dayColumnFormat = "dddd M/d";
        AjaxBaseControl _ownerView;

        [DefaultValue("ddd")]
        [NotifyParentProperty(true)]
        public string MonthFormat
        {
            get { return _monthColumnFormat; }
            set { _monthColumnFormat = value; }
        }

        [DefaultValue("ddd M/d")]
        [NotifyParentProperty(true)]
        public string WeekFormat
        {
            get { return _weekColumnFormat; }
            set { _weekColumnFormat = value; }
        }

        [DefaultValue("dddd M/d")]
        [NotifyParentProperty(true)]
        public string DayFormat
        {
            get { return _dayColumnFormat; }
            set { _dayColumnFormat = value; }
        }

        [Browsable(false)]
        public AjaxBaseControl OwnerView
        {
            get { return _ownerView; }
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class AjaxScheduleTime : IChildSet
    {
        public AjaxScheduleTime(AjaxBaseControl parent)
        {
            _ownerView = parent;
        }

        string _monthTimeFormat = "H(:mm)tt";
        string _weekTimeFormat = "H:mm{ - H:mm}";
        string _dayTimeFormat = "H:mm{ - H:mm}";
        AjaxBaseControl _ownerView;

        [DefaultValue("H(:mm)tt")]
        [NotifyParentProperty(true)]
        public string MonthFormat
        {
            get { return _monthTimeFormat; }
            set { _monthTimeFormat = value; }
        }

        [DefaultValue("H:mm{ - H:mm}")]
        [NotifyParentProperty(true)]
        public string WeekFormat
        {
            get { return _weekTimeFormat; }
            set { _weekTimeFormat = value; }
        }

        [DefaultValue("H:mm{ - H:mm}")]
        [NotifyParentProperty(true)]
        public string DayFormat
        {
            get { return _dayTimeFormat; }
            set { _dayTimeFormat = value; }
        }

        [Browsable(false)]
        public AjaxBaseControl OwnerView
        {
            get { return _ownerView; }
        }
    }
}