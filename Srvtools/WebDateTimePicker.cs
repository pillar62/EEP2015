using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.Design;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Drawing;
using System.Web.UI.HtmlControls;
using System.Drawing.Design;

namespace Srvtools
{
    public enum dateTimeType
    {
        DateTime = 0,
        VarChar = 1
    }
    public interface IDateTimePicker
    {
        string Text { get; set;}
        string DateString { get; set;}
        dateTimeType DateTimeType { get; set;}
    }

    [DefaultProperty("Text"), ToolboxData("<{0}:WebDateTimePicker runat=server></{0}:WebDateTimePicker>")]
    [ToolboxBitmap(typeof(WebDateTimePicker), "Resources.WebDataTimePicker.ico")]
    public class WebDateTimePicker : TextBox, INamingContainer, IPostBackEventHandler, IDateTimePicker
    {
        public WebDateTimePicker()
        {
            this.Width = 100;
            //this.Height = 18;
            this.UseButtonImage = true;
            this.TextChanged += new EventHandler(WebDateTimePicker_TextChanged);
            _Localize = false;
            _MinYear = 1950;
            _MaxYear = 2050;
        }

        void WebDateTimePicker_TextChanged(object sender, EventArgs e)
        {
            if (this.DateTimeType == dateTimeType.VarChar)
            {
                this.DateString = ConvertDateToString(this.Text);
            }
        }

#if AjaxTools
        [Category("Infolight")]
        public string UpdatePanelID
        {
            get
            {
                object obj = this.ViewState["UpdatePanelID"];
                if (obj != null)
                {
                    return (string)obj;
                }
                return "";
            }
            set
            {
                this.ViewState["UpdatePanelID"] = value;
            }
        }
#endif

        [Category("Infolight"),
        Description("Indicate whether the InnerButton should use image for it's appearance")]
        public bool UseButtonImage
        {
            get
            {
                if (ViewState["UseButtonImage"] != null)
                {
                    return (bool)ViewState["UseButtonImage"];
                }
                return false;
            }
            set
            {
                ViewState["UseButtonImage"] = value;
            }
        }

        private string _ButtonCaption;
        [Category("Infolight"),
        Description("Caption of the InnerButton")]
        [Bindable(true)]
        public string ButtonCaption
        {
            get
            {
                return _ButtonCaption;
            }
            set
            {
                _ButtonCaption = value;
            }
        }

        [Category("Infolight"),
        Description("The string bound to data when datatimetype set as varchar")]
        [Bindable(true), DefaultValue("")]
        public string DateString
        {
            get
            {
                object obj = this.ViewState["DateString"];
                if (obj != null)
                {
                    return (string)obj;
                }
                return "";
            }
            set
            {
                this.ViewState["DateString"] = value;
                //modified by ccm 2007/8/13带入初始值，针对Varchar类型无法清空的问题
                if (Text.Length == 0 && value.Length == 8)
                {
                    Text = ConvertStringToDate(value);
                }
            }
        }


        public DateFormatString DateFormatString
        {
            get
            {
                if (ViewState["DateFormatString"] != null)
                {
                    return (DateFormatString)ViewState["DateFormatString"];
                }
                return DateFormatString.yyyyMMdd;
            }
            set
            {
                ViewState["DateFormatString"] = value;
            }
        }
	

        [Category("Infolight"),
        Description("Specifies the format of the datestring")]
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


        [Category("Infolight"),
        Description("Specifies the type of data of datatime")]
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

        private string _caption;
        [Category("Infolight"),
        Description("Caption of the page which WebDateTimePicker opens")]
        public string Caption
        {
            get
            {
                return _caption;
            }
            set
            {
                _caption = value;
            }
        }

        [Category("Infolight"),
        DefaultValue(true),
        Description("Indicate whether to check date or not")]
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

        private bool _Localize;
        [Category("Infolight"),
        Description("Localize time format")]
        public bool Localize
        {
            get
            {
                return _Localize;
            }
            set
            {
                _Localize = value;
            }
        }

        private bool localizeForROC;
        [Category("Infolight"),
        Description("Localize roc time format")]
        public bool LocalizeForROC
	    {
            get { return localizeForROC; }
            set { localizeForROC = value; }
	    }

        private int _MinYear;
        [Category("Infolight"),
        Description("Specifies minimun Year")]
        public int MinYear
        {
            get { return _MinYear; }
            set 
            {
                if (value > 0 && value < MaxYear)
                {
                    _MinYear = value;
                }
            }
        }

        private int _MaxYear;
        [Category("Infolight"),
        Description("Specifies maximun Year")]
        public int MaxYear
        {
            get { return _MaxYear; }
            set
            {
                if (value > MinYear && value < 9999)
                {
                    _MaxYear = value;
                }
            }
        }


        [Category("Infolight"),
        Description("ImageUrl of the InnerButton")]
        [EditorAttribute(typeof(ImageUrlEditor), typeof(UITypeEditor))]
        public string ButtonImageUrl
        {
            get
            {
                if (ViewState["ButtonImageUrl"] != null && ViewState["ButtonImageUrl"].ToString() != "")
                {
                    return (string)ViewState["ButtonImageUrl"];
                }
                return "../Image/datetimepicker/datetimepicker.gif";
            }
            set
            {
                string strUrl = value;
                if (value.StartsWith("~"))
                {
                    strUrl = strUrl.Substring(1);
                    strUrl = ".." + strUrl;
                }
                ViewState["ButtonImageUrl"] = strUrl;
            }
        }


        [Browsable(false)]
        public bool TraDate
        {
            get
            {
                return  localizeForROC || (!DesignMode && Localize && Page.Request.UserLanguages[0].Equals("zh-tw"));
            }   
        }

        public override void DataBind()
        {
            base.DataBind();
            if (TraDate && !this.DesignMode && !string.IsNullOrEmpty(base.Text) && this.DateTimeType == dateTimeType.DateTime)
            {
                DateTime dt = DateTime.Parse(base.Text);
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

        public new string Text//显示和实际值转换
        {
            get
            {
                if (TraDate && base.Text.Trim().Length > 0 )
                {
                    string[] date = base.Text.Split("/-.".ToCharArray());
                    DateTime dt = new DateTime();
                    try
                    {

                        int year = Convert.ToInt32(date[0]);
                        int month = Convert.ToInt32(date[1]);
                        int day = Convert.ToInt32(date[2]);
                        year += 1911;
                        dt = new DateTime(year, month, day);
                    }
                    catch
                    { 
                    
                    }
                    return dt.ToShortDateString();
                }
                else
                {
                    return base.Text;
                }
            }
            set
            {
                base.Text = value;
            }
        }

        private string ConvertDateToString(string date)
        {
            char[] datePart = date.ToCharArray();
            string strYear = "";
            string strMonth = "";
            string strDay = "";
            string part1 = "";
            string part2 = "";
            string part3 = "";


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
                            part1 += j.ToString();
                        }
                        else if (flag == 1)
                        {
                            part2 += j.ToString();
                        }
                        else if (flag == 2)
                        {
                            part3 += j.ToString();
                        }
                    }
                }
                catch
                {
                    flag++;
                }
            }
            if (TraDate)
            {
                strYear = part1;
                strMonth = part2;
                strDay = part3;
            }
            else if (DateFormatString == DateFormatString.ddMMyyyy)
            {
                strYear = part3;
                strMonth = part2;
                strDay = part1;
            }
            else if (DateFormatString == DateFormatString.MMddyyyy)
            {
                strYear = part3;
                strMonth = part1;
                strDay = part2;
            }
            else
            {
                strYear = part1;
                strMonth = part2;
                strDay = part3;
            }

            if (strMonth.Length == 1)
                strMonth = "0" + strMonth;
            if (strDay.Length == 1)
                strDay = "0" + strDay;
            return strYear + strMonth + strDay;
        }


        private string ConvertStringToDate(string varChar)
        {
            string strYear = varChar.Substring(0, 4);
            string strMonth = varChar.Substring(4, 2);
            string strDay = varChar.Substring(6, 2);

            //if (strMonth.StartsWith("0"))
            //    strMonth = strMonth.Substring(1, 1);
            //if (strDay.StartsWith("0"))
            //    strDay = strDay.Substring(1, 1);

            DateTime dt = new DateTime(Convert.ToInt32(strYear), Convert.ToInt32(strMonth), Convert.ToInt32(strDay));
            //dt = dt.AddYears(Convert.ToInt32(strYear) - dt.Year);
            //dt = dt.AddMonths(Convert.ToInt32(strMonth) - dt.Month);
            //dt = dt.AddDays(Convert.ToInt32(strDay) - dt.Day);
            //string nowYear = dt.Year.ToString();
            //string nowMonth = dt.Month.ToString();
            //string nowDay = dt.Day.ToString();
            if (TraDate)
            {
                if (dt.Year < 1912)
                {
                    throw new Exception("Year should be larger than 1911 in ROC Calender");
                }
                return  string.Format("{0}.{1:00}.{2:00}", dt.Year - 1911, dt.Month, dt.Day);
            }
            string sdt = "";
            if (this.DateFormat == dateFormat.None)
            {
                sdt = dt.ToString().TrimStart(new char[] {'0'});
            }
            else if (this.DateFormat == dateFormat.LongDate)
            {
                sdt = dt.ToLongDateString().TrimStart(new char[] { '0' });
            }
            else if (this.DateFormat == dateFormat.ShortDate)
            {
                sdt = dt.ToShortDateString().TrimStart(new char[] { '0' });
            }
            //int YearLoc = sdt.IndexOf(nowYear);
            //sdt = sdt.Remove(YearLoc, nowYear.Length);
            //sdt = sdt.Insert(YearLoc, strYear);

            //int MonthLoc = sdt.IndexOf(nowMonth);
            //sdt = sdt.Remove(MonthLoc, nowMonth.Length);
            //sdt = sdt.Insert(MonthLoc, strMonth);

            //int DayLoc = sdt.IndexOf(nowDay);
            //sdt = sdt.Remove(DayLoc, nowDay.Length);
            //sdt = sdt.Insert(DayLoc, strDay);

            return sdt;
        }

        void IPostBackEventHandler.RaisePostBackEvent(string eventArgument)
        {
           
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (this.Page != null)
            {
#if AjaxTools
                object obj = this.GetObjByID(this.UpdatePanelID);
                if (obj != null && obj is UpdatePanel)
                {
                    UpdatePanel panel = (UpdatePanel)obj;
                    ScriptManager.RegisterClientScriptResource(panel, this.GetType(), "Srvtools.WebDateTimePicker.js");
                }
                else
                {
#endif
                Page.ClientScript.RegisterClientScriptResource(this.GetType(), "Srvtools.WebDateTimePicker.js");
#if AjaxTools
                }
#endif
                base.OnPreRender(e);
            }
        }

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
                if (this.Page != null)
                {
                    if (this.Page.Form != null)
                        return GetAllCtrls(ObjID, this.Page.Form);
                    else
                        return GetAllCtrls(ObjID, this.Page);
                }
                else
                {
                    return GetAllCtrls(ObjID, this.NamingContainer);
                }
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            ClientScriptManager csm = Page.ClientScript;
            writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
            writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0");
            writer.RenderBeginTag(HtmlTextWriterTag.Table); // <table>
            writer.RenderBeginTag(HtmlTextWriterTag.Tr); // <tr>
            writer.RenderBeginTag(HtmlTextWriterTag.Td); // <td>
            if (this.CssClass != "") { writer.AddAttribute(HtmlTextWriterAttribute.Class, this.CssClass); }
            writer.AddAttribute(HtmlTextWriterAttribute.Name, this.UniqueID);
            if (Page.Site == null)
            {
                if (this.DateTimeType == dateTimeType.VarChar)
                {
                    if (this.DateString != null && this.DateString.Length == 8)
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Value, ConvertStringToDate(this.DateString));
                    }
                    else
                        writer.AddAttribute(HtmlTextWriterAttribute.Value, null);
                }
                else if (base.Text == null || base.Text.Trim().Length == 0)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Value, null);
                }
                string blurScript = "";
                if (this.AutoPostBack)
                {
                    blurScript = csm.GetPostBackEventReference(this, "") + ";";
                }
                if (this.CheckDate)
                {
                    string message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "Srvtools", "WebDateTimePicker", "DateTimeValidate", true);
                    writer.AddAttribute("onblur", 
                        blurScript + string.Format("var params={{dtp:'{0}',traDate:{1},dateformat:'{2}',msg:'{3}'}};_dtpBlur(params);",
                        this.ClientID,
                        this.TraDate.ToString().ToLower(),
                        this.DateFormatString,
                        message));
                }
            }
            this.RenderBeginTag(writer);
            base.RenderContents(writer);
            this.RenderEndTag(writer);
            writer.RenderEndTag(); // <td>
            if (!this.ReadOnly)
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Td); // <td>
                string buttonName = this.UniqueID + "$InnerButton";
                writer.AddAttribute(HtmlTextWriterAttribute.Id, buttonName);
                writer.AddAttribute(HtmlTextWriterAttribute.Name, buttonName);
                if (Page.Site == null)
                {
                    string param = string.Format("&DateFormat={0}&UniqueID={1}&Caption={2}&TraDate={3}&MinYear={4}&MaxYear={5}",
                        this.DateFormat.ToString(),
                        HttpUtility.UrlEncode(this.ClientID),
                        HttpUtility.UrlEncode(this.Caption),
                        this.TraDate,
                        MinYear.ToString(),
                        MaxYear.ToString());
                    param = QueryStringEncrypt.Encrypt(param);
                    string ScriptBlock = string.Format("var a=encodeURI(document.getElementById('{0}').value);window.open('../InnerPages/frmDateTime.aspx?DateTime='+a+'{1}', '', 'width=255,height=270,top=300,left=300,scrollbars=no,resizable=no,toolbar=no,menubar=no,location=no,status=no');return false;",
                        this.ClientID,
                        param);
                    writer.AddAttribute(HtmlTextWriterAttribute.Onclick, ScriptBlock);
                }
                if (this.UseButtonImage)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Src, this.ButtonImageUrl);
                    writer.AddAttribute(HtmlTextWriterAttribute.Style, "border-right: thin outset;border-top: thin outset;border-left: thin outset; border-bottom: thin outset; background-color: buttonface");
                    writer.AddAttribute(HtmlTextWriterAttribute.Type, "image");
                }
                else
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Style, "width: 25px;");
                    writer.AddStyleAttribute(HtmlTextWriterStyle.Height, (this.Height.Value + 8).ToString() + "px;");
                    writer.AddAttribute(HtmlTextWriterAttribute.Value, this.ButtonCaption);
                    writer.AddAttribute(HtmlTextWriterAttribute.Type, "button");
                }
                writer.RenderBeginTag(HtmlTextWriterTag.Input);
                writer.RenderEndTag();
                writer.RenderEndTag(); // <td>
            }
            writer.RenderEndTag(); // <tr>
            writer.RenderEndTag(); // <table>
        }
    }

    public enum dateFormat
    {
        None = 0,
        ShortDate = 1,
        LongDate = 2
    }

    public enum DateFormatString
    { 
        yyyyMMdd,
        ddMMyyyy,
        MMddyyyy,
        varChar
    }
}
