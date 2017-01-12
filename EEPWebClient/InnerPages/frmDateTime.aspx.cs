using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Srvtools;
using System.Text;


public partial class InnerPages_frmDateTime : System.Web.UI.Page
{
    public string CarryDateTime
    {
       
        get
        { 
            object obj = this.ViewState["CarryDateTime"];
            if(obj != null)
            {
                return (string)obj;
            }
            return "";
        }
        set
        {
            this.ViewState["CarryDateTime"] = value;
        }
    }

    public string CarryDateFormat
    {
        get
        {
            object obj = this.ViewState["CarryDateFormat"];
            if (obj != null)
            {
                return (string)obj;
            }
            return "";
        }
        set
        {
            this.ViewState["CarryDateFormat"] = value;
        }
    }

    public string CarryUniqueID
    {
        get
        {
            object obj = this.ViewState["CarryUniqueID"];
            if (obj != null)
            {
                return (string)obj;
            }
            return "";
        }
        set
        {
            this.ViewState["CarryUniqueID"] = value;
        }
    }

    public bool TraDate
    {
        get
        {
            object obj = this.ViewState["TraDate"];
            if (obj != null)
            {
                return (bool)obj;
            }
            return false;
        }
        set
        {
            this.ViewState["TraDate"] = value;
        }
    
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            QueryStringEncrypt.Check(this, new string[] { "DateTime"});
            string message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "Srvtools", "WebDateTimePicker", "DateValues", true);
            string[] DateVals = message.Split(';');
            this.lblYear.Text = DateVals[0];
            this.lblMonth.Text = DateVals[1];
            this.Calendar1.VisibleDate = DateTime.Today;
            try
            {
                TraDate = Convert.ToBoolean(Request.QueryString["TraDate"]);
            }
            catch
            {

            }
            int minyear = 1950;
            int maxyear = 2050;
            try
            {
                minyear = Convert.ToInt32(Request.QueryString["MinYear"]);
                maxyear = Convert.ToInt32(Request.QueryString["MaxYear"]);
            }
            catch
            { 
                
            }

            if (TraDate)
            {
                //this.Calendar1.TitleFormat = TitleFormat.Month;
                minyear -= 1911;
                maxyear -= 1911;
            }

            for (int i = Math.Max(1,minyear); i <= maxyear; i++)
            {
                this.ddlSelYear.Items.Add(i.ToString());
            }
            for (int i = 1; i < 13; i++)
            {
                this.ddlSelMonth.Items.Add(i.ToString());
            }
            this.CarryDateTime = Request.QueryString["DateTime"];
            this.CarryDateFormat = Request.QueryString["DateFormat"];
            this.CarryUniqueID = Request.QueryString["UniqueID"];
          

            string strDate = this.CarryDateTime;
            DateTime dt = new DateTime();
            if (strDate == "")
            {
                dt = DateTime.Today;
                this.ddlSelYear.Text = dt.Year.ToString();
            }
            else
            {
                try
                {
                    if (TraDate)
                    {
                        string[] date = strDate.Split("/-.".ToCharArray());
                        try
                        {
                            int year = Convert.ToInt32(date[0]);
                            int month = Convert.ToInt32(date[1]);
                            int day = Convert.ToInt32(date[2]);
                            this.ddlSelYear.Text = year.ToString();
                            year += 1911;
                            dt = new DateTime(year, month, day);
                        }
                        catch
                        {

                        }
                    }
                    else
                    {
                        dt = Convert.ToDateTime(strDate);
                        this.ddlSelYear.Text = dt.Year.ToString();
                    }
                }
                catch
                {
                    dt = BuildLackDate(strDate);
                }
                this.Calendar1.SelectedDate = dt;//only set selectdate while datetimepicker has value
            }
           
            this.ddlSelMonth.Text = dt.Month.ToString();
            this.Calendar1.VisibleDate = dt;
            
        }
        this.Title = Request.QueryString["Caption"];
    }

    private DateTime BuildLackDate(string datePart)
    {
        DateTime dt = DateTime.Today;
        string syear = "",smonth = "1",sday = "1";
        int year, month, day;
        int i = datePart.Length;
        if (i <= 4)
        {
            for (int j = 0; j < i; j++)
            {
                syear += datePart[j].ToString();
            }
        }
        else if (i > 4 && i <= 6)
        {
            smonth = "";
            for (int j = 0; j < i; j++)
            {
                if (j < 4)
                    syear += datePart[j].ToString();
                else
                    smonth += datePart[j].ToString();
            }
        }
        else if (i > 6 && i <= 8)
        {
            smonth = "";
            sday = "";
            for (int j = 0; j < i; j++)
            {
                if (j < 4)
                    syear += datePart[j].ToString();
                else if (j < 6)
                    smonth += datePart[j].ToString();
                else
                    sday += datePart[j].ToString();
            }
        }

        if (Int32.TryParse(syear, out year) && Int32.TryParse(smonth, out month) && Int32.TryParse(sday, out day))
        {
            try
            {
                dt = new DateTime(year, month, day);
            }
            catch
            {
                dt = DateTime.Today;
            }
        }
        return dt;
    }

    protected void Calendar1_SelectionChanged(object sender, EventArgs e)
    {
        string strValue = "";
        DateTime dt = this.Calendar1.SelectedDate;
        if (TraDate)
        {
            strValue = string.Format("{0}.{1:00}.{2:00}", dt.Year - 1911, dt.Month, dt.Day);
        }
        else
        {
            if (string.Compare(CarryDateFormat, "shortdate", true) == 0)//IgnoreCase
            {
                strValue = dt.ToShortDateString().TrimStart(new char[] { '0' });
            }
            else if (string.Compare(CarryDateFormat, "longdate", true) == 0)//IgnoreCase
            {
                strValue = dt.ToLongDateString().TrimStart(new char[] { '0' });
            }
            else if (string.Compare(CarryDateFormat, "none", true) == 0)//IgnoreCase
            {
                strValue = dt.ToString().TrimStart(new char[] { '0' });
            }
        }
        Response.Write("<script language=javascript>var source = window.opener.document.getElementById('" + CarryUniqueID + "');source.value = '" + strValue + "';source.focus();window.close();</script>");
    }

    protected void ddlSelYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        string year = this.ddlSelYear.Text;
        int i = Convert.ToInt16(year);
        if (TraDate)
        {
            i += 1911;
        }
        DateTime time = this.Calendar1.VisibleDate;
        this.Calendar1.VisibleDate = time.AddYears(i - time.Year);
    }

    protected void ddlSelMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        string month = this.ddlSelMonth.Text;
        int i = Convert.ToInt16(month);
        DateTime time = this.Calendar1.VisibleDate;
        this.Calendar1.VisibleDate = time.AddMonths(i - time.Month);
    }
    protected void Calendar1_VisibleMonthChanged(object sender, MonthChangedEventArgs e)
    {
        int year = e.NewDate.Year;
        if (TraDate)
        {
            year -= 1911;
        }
        this.ddlSelYear.Text = year.ToString();
        this.ddlSelMonth.Text = e.NewDate.Month.ToString();
    }
    protected void Calendar1_PreRender(object sender, EventArgs e)
    {
        string cultureName = "zh-tw";
        if (CliUtils.fClientLang == SYS_LANGUAGE.ENG)
        {
            cultureName = "en-us";
        }
        else if (CliUtils.fClientLang == SYS_LANGUAGE.SIM)
        {
            cultureName = "zh-cn";
        }
        if (CliUtils.fClientLang == SYS_LANGUAGE.TRA)
        {
            cultureName = "zh-tw";
        }
        System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo(cultureName); ;
        System.Threading.Thread.CurrentThread.CurrentCulture = culture;
    }
}
