using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.Globalization;
using System.ComponentModel;
using System.Web.UI;

namespace Srvtools
{
    public class WebMonthPicker: DropDownList
    {
        private bool rocYear;
        /// <summary>
        /// Get or set display in roc mode
        /// </summary>
        [Category("InfoLight"),
        Description("Display in roc mode")]
        public bool RocYear
        {
            get { return rocYear; }
            set { rocYear = value; }
        }

        private string separator = "/";
        /// <summary>
        /// Get or set the separator between month and year
        /// </summary>
        [Category("InfoLight"),
        Description("The separator between month and year")]
        public string Separator
        {
            get { return separator; }
            set { separator = value; }
        }

        private int monthScope = 12;
        /// <summary>
        /// Get or set the scope of months to select
        /// </summary>
        [Category("InfoLight"),
        Description("The scope of months to select")]
        public int MonthScope
        {
            get { return monthScope; }
            set { monthScope = value; }
        }

        protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
        {
            if (this.DesignMode)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Selected, "selected");
                writer.AddAttribute(HtmlTextWriterAttribute.Value, DateTime.Today.ToString("yyyyMM", new CultureInfo("en-us")));
                writer.RenderBeginTag(HtmlTextWriterTag.Option);
                TaiwanCalendar twCalendar = new TaiwanCalendar();
                string format = RocYear ? "{0:000}" + Separator + "{1:00}" : "{0:0000}" + Separator + "{1:00}";
                writer.Write(RocYear ? string.Format(format, twCalendar.GetYear(DateTime.Today), twCalendar.GetMonth(DateTime.Today))
                : string.Format(format, DateTime.Today.Year, DateTime.Today.Month));
                writer.RenderEndTag();
            }
            else
            {
                base.RenderContents(writer);
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!Page.IsPostBack && string.IsNullOrEmpty(this.SelectedValue))
            {
                InitialItems(this.SelectedValue);
            }
            base.OnLoad(e);
        }

        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            base.OnSelectedIndexChanged(e);
            InitialItems(this.SelectedValue);
        }

        private void InitialItems(string selectedValue)
        {
            DateTime SelectedDate = DateTime.Today;
            if (!string.IsNullOrEmpty(selectedValue))
            {
                if (!DateTime.TryParseExact(selectedValue, "yyyyMM", new CultureInfo("en-us"), DateTimeStyles.None, out SelectedDate))
                {
                    throw new FormatException(string.Format("{0} can not recgonized as yyyyMM", selectedValue));
                }
            }
            string format = RocYear ? "{0:000}" + Separator + "{1:00}" : "{0:0000}" + Separator + "{1:00}";
            this.Items.Clear();
            TaiwanCalendar twCalendar = new TaiwanCalendar();
            this.Items.Add(new ListItem(RocYear ? string.Format(format, twCalendar.GetYear(SelectedDate), twCalendar.GetMonth(SelectedDate))
                : string.Format(format, SelectedDate.Year, SelectedDate.Month), SelectedDate.ToString("yyyyMM", new CultureInfo("en-us"))));
            for (int i = 1; i <= MonthScope; i++)
            {
                DateTime date = SelectedDate.AddMonths(-i);
                this.Items.Insert(0, new ListItem(RocYear ? string.Format(format, twCalendar.GetYear(date), twCalendar.GetMonth(date))
                    : string.Format(format, date.Year, date.Month), date.ToString("yyyyMM", new CultureInfo("en-us"))));
                date = SelectedDate.AddMonths(i);
                this.Items.Add(new ListItem(RocYear ? string.Format(format, twCalendar.GetYear(date), twCalendar.GetMonth(date))
                    : string.Format(format, date.Year, date.Month), date.ToString("yyyyMM", new CultureInfo("en-us"))));
            }
            base.SelectedValue = SelectedDate.ToString("yyyyMM", new CultureInfo("en-us"));
        }

        public override string SelectedValue
        {
            get
            {
                return base.SelectedValue;
            }
            set
            {
                InitialItems(value);
                if (!string.IsNullOrEmpty(value))
                {
                    base.SelectedValue = value;
                }
            }
        }

        [Browsable(false)]
        public override bool AutoPostBack
        {
            get
            {
                return true;
            }
            set
            {
                base.AutoPostBack = value;
            }
        }
    }
}
