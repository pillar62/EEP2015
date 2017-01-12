using System;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using System.ComponentModel;
using System.Web.UI;
using Srvtools;
using System.Web;
using System.Security.Permissions;
using System.Web.UI.HtmlControls;

[assembly: System.Web.UI.WebResource("AjaxTools.AjaxDateTimePicker.js", "text/javascript")]
namespace AjaxTools
{
    public class AjaxDateTimePicker : WebControl, INamingContainer, IDateTimePicker
    {
        private Button _hidTarget = new Button();
        private ImageButton _imgButton = new ImageButton();
        private TextBox _dateTimeEdit = new TextBox();
        private ModalPopupExtender _modalPopupExtender = new ModalPopupExtender();
        //µ¯³ö¿òÄÚÈÝ
        private Panel _popupContainer = new Panel();
        private ImageButton _btnClose = new ImageButton();
        private UpdatePanel _updatePanel = new UpdatePanel();
        private Calendar _calendar = new Calendar();
        private DropDownList _ddlYear = new DropDownList();
        private DropDownList _ddlMonth = new DropDownList();

        #region properties
        [Bindable(true)]
        [Category("Infolight")]
        [DefaultValue("")]
        public string Text
        {
            get
            {
                if (this.DateTimeType == dateTimeType.VarChar && TraDate)
                {
                    if (this.ViewState["Text"] != null && this.ViewState["Text"].ToString().Trim().Length > 0)
                    {
                        string[] date = this.ViewState["Text"].ToString().Split("/-.".ToCharArray());
                        DateTime dt = new DateTime();
                        try
                        {
                            int year = Convert.ToInt32(date[0]);
                            int month = Convert.ToInt32(date[1]);
                            int day = Convert.ToInt32(date[2].Substring(0, 2));
                            year += 1911;
                            dt = new DateTime(year, month, day);
                        }
                        catch
                        {

                        }
                        return dt.ToString();
                    }
                    else
                    {
                        return "";
                    }
                }
                else
                {
                    if (this.ViewState["Text"] != null && this.ViewState["Text"].ToString().Trim().Length > 0)
                    {
                        return this.ViewState["Text"].ToString();
                    }
                    else
                    {
                        return "";
                    }
                }
            }
            set
            {
                this.ViewState["Text"] = value;
            }
        }

        [Bindable(true)]
        [Category("Infolight")]
        [DefaultValue("")]
        public string DateString
        {
            get
            {
                if (this.ViewState["DateString"] != null)
                {
                    return (string)this.ViewState["DateString"];
                }
                return "";
            }
            set
            {
                this.ViewState["DateString"] = value;
            }
        }

        [Category("Infolight")]
        [DefaultValue(dateFormat.None)]
        public dateFormat DateFormat
        {
            get
            {
                if (ViewState["DateFormat"] != null)
                {
                    return (dateFormat)ViewState["DateFormat"];
                }
                return dateFormat.None;
            }
            set
            {
                ViewState["DateFormat"] = value;
            }
        }

        [Category("Infolight")]
        [DefaultValue(dateTimeType.DateTime)]
        public dateTimeType DateTimeType
        {
            get
            {
                if (ViewState["DateTimeType"] != null)
                {
                    return (dateTimeType)ViewState["DateTimeType"];
                }
                return dateTimeType.DateTime;
            }
            set
            {
                ViewState["DateTimeType"] = value;
                if (value == dateTimeType.VarChar)
                {
                    ViewState["DateFormat"] = dateFormat.ShortDate;
                }
            }
        }

        [Category("Infolight")]
        [DefaultValue(true)]
        public bool CheckDate
        {
            get
            {
                object obj = this.ViewState["CheckDate"];
                if (obj != null)
                {
                    return (bool)obj;
                }
                return true;
            }
            set
            {
                this.ViewState["CheckDate"] = value;
            }
        }

        [DefaultValue(typeof(Unit), "120px")]
        public override Unit Width
        {
            get
            {
                object obj = this.ViewState["Width"];
                if (obj != null)
                {
                    return (Unit)obj;
                }
                return new Unit(120, UnitType.Pixel);
            }
            set
            {
                this.ViewState["Width"] = value;
            }
        }

        [DefaultValue(typeof(Unit), "16px")]
        public override Unit Height
        {
            get
            {
                object obj = this.ViewState["Height"];
                if (obj != null)
                {
                    return (Unit)obj;
                }
                return new Unit(16, UnitType.Pixel);
            }
            set
            {
                this.ViewState["Height"] = value;
            }
        }

        [Category("Infolight")]
        [DefaultValue(1950)]
        public int MinYear
        {
            get
            {
                object minYear = this.ViewState["MinYear"];
                if (minYear != null && minYear is int && (int)minYear > 0)
                {
                    return (int)this.ViewState["MinYear"];
                }
                return 1950;
            }
            set
            {
                if (value <= 0)
                    value = 1950;

                if (this.ViewState["MaxYear"] != null)
                {
                    if ((int)this.ViewState["MaxYear"] <= value)
                        this.ViewState["MinYear"] = (int)this.ViewState["MaxYear"] - 1;
                    else
                        this.ViewState["MinYear"] = value;
                }
                else
                {
                    this.ViewState["MinYear"] = value;
                }
            }
        }

        [Category("Infolight")]
        [DefaultValue(2050)]
        public int MaxYear
        {
            get
            {
                object maxYear = this.ViewState["MaxYear"];
                if (maxYear != null && maxYear is int && (int)maxYear > 0)
                {
                    return (int)this.ViewState["MaxYear"];
                }
                return 2050;
            }
            set
            {
                if (value <= 0)
                    value = 2050;

                if (this.ViewState["MinYear"] != null)
                {
                    if ((int)this.ViewState["MinYear"] >= value)
                        this.ViewState["MaxYear"] = (int)this.ViewState["MinYear"] + 1;
                    else
                        this.ViewState["MaxYear"] = value;
                }
                else
                {
                    this.ViewState["MaxYear"] = value;
                }
            }
        }

        //[Category("Infolight")]
        //public override ControlCollection Controls
        //{
        //    get
        //    {
        //        EnsureChildControls();
        //        return base.Controls;
        //    }
        //}

        private bool _Localize;
        [Category("Infolight"),
        Description("Localize time format")]
        public bool Localize
        {
            get { return _Localize; }
            set { _Localize = value; }
        }

        private bool localizeForROC;
        [Category("Infolight"),
        Description("Localize roc time format")]
        public bool LocalizeForROC
        {
            get { return localizeForROC; }
            set { localizeForROC = value; }
        }

        [Browsable(false)]
        public bool TraDate
        {
            get
            {
                return localizeForROC || (!DesignMode && Localize && Page.Request.UserLanguages[0].Equals("zh-tw"));
            }
        }
        #endregion

        protected override void CreateChildControls()
        {
            Controls.Clear();
            //_dateTimeEdit
            _dateTimeEdit = new TextBox();
            _dateTimeEdit.ID = "dateTimeEdit";
            _dateTimeEdit.TextChanged += new EventHandler(dateTimeEdit_TextChanged);
            this.Controls.Add(_dateTimeEdit);

            //_imgButton
            _imgButton = new ImageButton();
            _imgButton.ID = "imgButton";
            _imgButton.Click += new ImageClickEventHandler(imgButton_Click);
            this.Controls.Add(_imgButton);
            //_hidTarget
            _hidTarget = new Button();
            _hidTarget.ID = "hidTarget";
            this.Controls.Add(_hidTarget);

            //_popupContainer
            _popupContainer = new Panel();
            _popupContainer.ID = "popupContainer";
            this.Controls.Add(_popupContainer);

            //_btnClose
            _btnClose = new ImageButton();
            _btnClose.ID = "btnClose";
            _btnClose.ImageUrl = "~/Image/Ajax/close.gif";
            this.Controls.Add(_btnClose);

            //_ddlYear
            _ddlYear = new DropDownList();
            _ddlYear.ID = "ddlYear";
            _ddlYear.Width = new Unit(60, UnitType.Pixel);
            if (TraDate)
            {
                for (int i = this.MinYear - 1911; i <= this.MaxYear - 1911; i++)
                {
                    _ddlYear.Items.Add(i.ToString());
                }
            }
            else
            {
                for (int i = this.MinYear; i <= this.MaxYear; i++)
                {
                    _ddlYear.Items.Add(i.ToString());
                }
            }
            _ddlYear.AutoPostBack = true;
            _ddlYear.SelectedIndexChanged += new EventHandler(_ddlYear_SelectedIndexChanged);

            //_ddlMonth
            _ddlMonth = new DropDownList();
            _ddlMonth.ID = "ddlMonth";
            _ddlMonth.Width = new Unit(60, UnitType.Pixel);
            for (int i = 1; i <= 12; i++)
            {
                _ddlMonth.Items.Add(i.ToString());
            }
            _ddlMonth.AutoPostBack = true;
            _ddlMonth.SelectedIndexChanged += new EventHandler(_ddlMonth_SelectedIndexChanged);

            //_calendar
            _calendar = new Calendar();
            //_calendar.ShowTitle = false;
            _calendar.ID = "calendar";
            _calendar.SkinID = "InnerDateTimePickerSkin1";
            _calendar.VisibleMonthChanged += new MonthChangedEventHandler(_calendar_VisibleMonthChanged);
            _calendar.SelectionChanged += new EventHandler(calendar_SelectionChanged);

            string dateMessage = SysMsg.GetSystemMessage(CliUtils.fClientLang, "Srvtools", "WebDateTimePicker", "DateValues", true);
            string[] DateVals = dateMessage.Split(';');

            //Table
            Table _innerTable = new Table();
            //Row1
            TableRow _innerRow1 = new TableRow();
            TableCell _innerCell11 = new TableCell();
            _innerCell11.Style.Add(HtmlTextWriterStyle.TextAlign, "center");
            Label lblYear = new Label();
            lblYear.Text = DateVals[0];
            _innerCell11.Controls.Add(lblYear);
            _innerCell11.Controls.Add(_ddlYear);
            TableCell _innerCell12 = new TableCell();
            _innerCell12.Style.Add(HtmlTextWriterStyle.TextAlign, "center");
            Label lblMonth = new Label();
            lblMonth.Text = DateVals[1];
            _innerCell12.Controls.Add(lblMonth);
            _innerCell12.Controls.Add(_ddlMonth);
            _innerRow1.Cells.AddRange(new TableCell[] { _innerCell11, _innerCell12 });
            //Row2
            TableRow _innerRow2 = new TableRow();
            TableCell _innerCell21 = new TableCell();
            _innerCell21.ColumnSpan = 2;
            _innerCell21.Controls.Add(_calendar);
            _innerRow2.Cells.Add(_innerCell21);
            _innerTable.Rows.AddRange(new TableRow[] { _innerRow1, _innerRow2 });

            //_updatePanel
            _updatePanel = new UpdatePanel();
            _updatePanel.ID = "upDateContent";
            _updatePanel.UpdateMode = UpdatePanelUpdateMode.Conditional;
            _updatePanel.ContentTemplateContainer.Controls.Add(_innerTable);
            AsyncPostBackTrigger trigger = new AsyncPostBackTrigger();
            trigger.ControlID = this._imgButton.UniqueID;
            trigger.EventName = "Click";
            _updatePanel.Triggers.Add(trigger);
            this.Controls.Add(_updatePanel);

            if (!this.DesignMode)
            {
                _modalPopupExtender = new ModalPopupExtender();
                _modalPopupExtender.ID = "modalPopupExtender";
                _modalPopupExtender.TargetControlID = this._hidTarget.UniqueID;
                _modalPopupExtender.PopupControlID = this._popupContainer.UniqueID;
                _modalPopupExtender.PopupDragHandleControlID = this._popupContainer.UniqueID;
                _modalPopupExtender.BackgroundCssClass = "ajaxdtp_modalBackground";
                _modalPopupExtender.CancelControlID = this._btnClose.UniqueID;
                _modalPopupExtender.BehaviorID = this.ClientID + "_dateTimeShowModalBehavior";
                //_modalPopupExtender.DropShadow = true;
                this.Controls.Add(_modalPopupExtender);
            }
        }

        void _calendar_VisibleMonthChanged(object sender, MonthChangedEventArgs e)
        {
            if (TraDate)
            {
                ScriptManager.RegisterStartupScript(this._updatePanel, this.GetType(), "datescript", string.Format("setTraDate('{0}')", this._calendar.ClientID), true);
            }
        }

        void _ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            string month = this._ddlMonth.Text;
            int i = Convert.ToInt16(month);
            DateTime time = this._calendar.VisibleDate;
            this._calendar.VisibleDate = time.AddMonths(i - time.Month);

            if (TraDate)
            {
                ScriptManager.RegisterStartupScript(this._updatePanel, this.GetType(), "datescript", string.Format("setTraDate('{0}')", this._calendar.ClientID), true);
            }
        }

        void _ddlYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            string year = this._ddlYear.Text;
            int i = Convert.ToInt16(year);
            if (TraDate)
            {
                i += 1911;
            }
            DateTime time = this._calendar.VisibleDate;
            this._calendar.VisibleDate = time.AddYears(i - time.Year);

            if (TraDate)
            {
                ScriptManager.RegisterStartupScript(this._updatePanel, this.GetType(), "datescript", string.Format("setTraDate('{0}')", this._calendar.ClientID), true);
            }
        }

        void imgButton_Click(object sender, ImageClickEventArgs e)
        {
            DateTime dt = DateTime.Now.Date;
            if (!string.IsNullOrEmpty(_dateTimeEdit.Text.Trim()))
            {
                try
                {
                    if (TraDate)
                    {
                        string[] dates = _dateTimeEdit.Text.Split("/-.".ToCharArray());
                        dt = new DateTime(Convert.ToInt16(dates[0]) + 1911, Convert.ToInt16(dates[1]), Convert.ToInt16(dates[2]));
                    }
                    else
                    {
                        dt = DateTime.Parse(_dateTimeEdit.Text.Trim());
                    }
                }
                catch 
                {
                }
            }
            if (TraDate)
            {
                this._ddlYear.Text = (dt.Year - 1911).ToString();
            }
            else
            {
                this._ddlYear.Text = dt.Year.ToString();
            }
            this._ddlMonth.Text = dt.Month.ToString();
            this._calendar.VisibleDate = dt;
            this._calendar.SelectedDate = DateTime.MaxValue;

            if (TraDate)
            {
                ScriptManager.RegisterStartupScript(this._updatePanel, this.GetType(), "datescript", string.Format("setTraDate('{0}')", this._calendar.ClientID), true);
            }
        }

        void dateTimeEdit_TextChanged(object sender, EventArgs e)
        {
            if (this.DateTimeType == dateTimeType.VarChar)
            {
                this.DateString = ConvertDateToString(this._dateTimeEdit.Text.Trim());
            }
            else
            {
                if (this.TraDate)
                {
                    string[] rocDates = this._dateTimeEdit.Text.Split("/-.".ToCharArray());
                    int year = Int32.Parse(rocDates[0]) + 1911;
                    int month = Int32.Parse(rocDates[1]);
                    int day = Int32.Parse(rocDates[2]);
                    this.Text = new DateTime(year, month, day).ToString();
                }
                else
                {
                    this.Text = this._dateTimeEdit.Text.Trim();
                }
            }
        }

        void calendar_SelectionChanged(object sender, EventArgs e)
        {
            string format = "$find('" + _modalPopupExtender.BehaviorID + "').hide();$get('" + this._dateTimeEdit.ClientID + "').value='{0}';";
            string script = "";
            string value = "";
            DateTime dt = this._calendar.SelectedDate;
            switch (this.DateFormat)
            {
                case dateFormat.LongDate:
                    value = dt.ToLongDateString();
                    script = string.Format(format, value);
                    break;
                case dateFormat.ShortDate:
                    if(TraDate)
                        value = string.Format("{0}.{1:00}.{2:00}", dt.Year - 1911, dt.Month, dt.Day);
                    else
                        value = dt.ToShortDateString();
                    script = string.Format(format, value);
                    break;
                case dateFormat.None:
                    value = dt.ToString();
                    script = string.Format(format, value);
                    break;
            }
            if (this.DateTimeType == dateTimeType.DateTime && !this.TraDate)
            {
                this.Text = value;
            }
            ScriptManager.RegisterStartupScript(this._updatePanel, this.GetType(), "datescript", script, true);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            //ClientScriptManager csm = Page.ClientScript;
            writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
            writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0");
            writer.RenderBeginTag(HtmlTextWriterTag.Table); // <table>

            writer.RenderBeginTag(HtmlTextWriterTag.Tr); // <tr>

            writer.RenderBeginTag(HtmlTextWriterTag.Td); // <td>
            //DateTimeTextBox
            if (!this.DesignMode)
            {
                if (this.DateTimeType == dateTimeType.VarChar)
                {
                    if (this.DateString != null && this.DateString.Length == 8)
                        writer.AddAttribute("value", ConvertStringToDate(this.DateString));
                    else
                        writer.AddAttribute("value", null);
                }
                else
                {
                    if(this.Text == null || this.Text.Trim().Length == 0)
                        writer.AddAttribute("value", null);
                    else
                        writer.AddAttribute("value", this.Text);
                }
                string blurScript = "";
                writer.AddAttribute("onchange", string.Format("_dtpChange1('{0}');", this._dateTimeEdit.ClientID));
                if (this.CheckDate)
                {
                    string message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "Srvtools", "WebDateTimePicker", "DateTimeValidate", true);
                    writer.AddAttribute("onblur",
                        blurScript + string.Format("var params={{dtp:'{0}',traDate:{1},msg:'{2}'}};_dtpBlur1(params);",
                        this._dateTimeEdit.ClientID,
                        this.TraDate.ToString().ToLower(),
                        message));
                    //blurScript += "if(!checkdate(" + this._dateTimeEdit.ClientID + ".value)) {" + this._dateTimeEdit.ClientID + ".focus();" + this._dateTimeEdit.ClientID + ".value='';}";
                    //writer.AddAttribute("onblur", blurScript);
                }
            }
            writer.AddStyleAttribute("width", this.Width.Value.ToString() + "px");
            writer.AddStyleAttribute("height", this.Height.Value.ToString() + "px");
            _dateTimeEdit.RenderControl(writer);
            writer.RenderEndTag(); // </td>

            writer.RenderBeginTag(HtmlTextWriterTag.Td); // <td>
            //DateTimeButton
            writer.AddAttribute("src", "../Image/datetimepicker/datetimepicker.gif");
            writer.AddStyleAttribute("position", "relative");
            writer.AddStyleAttribute("left", "-2px");
            writer.AddAttribute("onclick", "$find('" + this.ClientID + "_dateTimeShowModalBehavior').show();");
            _imgButton.RenderControl(writer);
            writer.RenderEndTag(); // </td>
            writer.RenderEndTag(); // </tr>
            writer.RenderEndTag(); // </table>
            if (!this.DesignMode)
            {
                // HidenTarget
                writer.AddStyleAttribute("display", "none");
                _hidTarget.RenderControl(writer);

                //RenderPopup
                writer.AddStyleAttribute("background-color", "#buttonface");
                writer.AddStyleAttribute("border-width", "3px");
                writer.AddStyleAttribute("border-style", "solid");
                writer.AddStyleAttribute("border-color", "Gray");
                writer.AddStyleAttribute("padding", "3px");
                writer.AddStyleAttribute("width", "225px");
                writer.AddStyleAttribute("display", "none");
                _popupContainer.RenderBeginTag(writer);

                //title
                writer.AddAttribute("id", "divTitle");
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "ajaxdtp_div_title");
                writer.RenderBeginTag(HtmlTextWriterTag.Div); //<div>
                writer.AddStyleAttribute("cursor", "pointer");
                writer.AddStyleAttribute("position", "relative");
                writer.AddStyleAttribute("top", "3px");
                _btnClose.RenderControl(writer);
                writer.RenderEndTag();//</div>
                //content
                writer.AddAttribute("id", "divContent");
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "ajaxdtp_div_content");
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                _updatePanel.RenderControl(writer);
                writer.RenderEndTag();
                _popupContainer.RenderEndTag(writer);
                if (!this.DesignMode)
                    _modalPopupExtender.RenderControl(writer);
            }
        }

        public override void DataBind()
        {
            base.DataBind();
            if (TraDate && !string.IsNullOrEmpty(this.Text))
            {
                DateTime dt = DateTime.Parse(this.Text);
                if (dt.Year < 1912)
                {
                    throw new Exception("Year should be larger than 1911 in ROC Calender");
                }
                else
                {
                    this.Text = string.Format("{0}.{1:00}.{2:00}", dt.Year - 1911, dt.Month, dt.Day);
                }
            }
        }

        #region methods
        private string ConvertDateToString(string date)
        {
            string strYear = "";
            string strMonth = "";
            string strDay = "";

            if (TraDate)
            {
                string[] dates = date.Split("/-.".ToCharArray());
                strYear = (Convert.ToInt16(dates[0]) + 1911).ToString();
                strMonth = dates[1];
                strDay = dates[2];
            }
            else
            {
                char[] datePart = date.ToCharArray();

                int flag = 0;
                for (int i = 0; i < datePart.Length; i++)
                {
                    char j = datePart[i];
                    try
                    {
                        int temp = Convert.ToInt32(j.ToString());
                        if (temp <= 9 && temp >= 0)
                        {
                            if (flag == 0)
                            {
                                strYear += j.ToString();
                            }
                            else if (flag == 1)
                            {
                                strMonth += j.ToString();
                            }
                            else if (flag == 2)
                            {
                                strDay += j.ToString();
                            }
                        }
                    }
                    catch
                    {
                        flag++;
                    }
                }
                if (strMonth.Length == 1)
                    strMonth = "0" + strMonth;
                if (strDay.Length == 1)
                    strDay = "0" + strDay;
            }
            return strYear + strMonth + strDay;
        }

        private string ConvertStringToDate(string varChar)
        {
            string strYear = varChar.Substring(0, 4);
            string strMonth = varChar.Substring(4, 2);
            string strDay = varChar.Substring(6, 2);

            DateTime dt = new DateTime(Convert.ToInt32(strYear), Convert.ToInt32(strMonth), Convert.ToInt32(strDay));
            if (TraDate)
            {
                if (dt.Year < 1912)
                {
                    throw new Exception("Year should be larger than 1911 in ROC Calender");
                }
                return string.Format("{0}.{1:00}.{2:00}", dt.Year - 1911, dt.Month, dt.Day);
            }
            string sdt = "";
            if (this.DateFormat == dateFormat.None)
            {
                sdt = dt.ToString().TrimStart(new char[] { '0' });
            }
            else if (this.DateFormat == dateFormat.LongDate)
            {
                sdt = dt.ToLongDateString().TrimStart(new char[] { '0' });
            }
            else if (this.DateFormat == dateFormat.ShortDate)
            {
                sdt = dt.ToShortDateString().TrimStart(new char[] { '0' });
            }
            return sdt;
        }

        #endregion

    }
}
