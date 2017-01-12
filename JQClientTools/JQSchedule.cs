using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Drawing.Design;
using System.Web.UI;

namespace JQClientTools
{
    public class JQSchedule : WebControl, IJQDataSourceProvider
    {
        public JQSchedule()
        {
            DateFormat = JQSchedule.DateFormatType.DateTime;
            TimeFormat = "hh:mm";
            DayHourFrom = 8;
            DayHourTo = 24;
            Interval = 1;
            WeekSummary = true;
            AllowUpdate = true;
            DefaultStyle = ScheduleStyle.Monthly;
            ShowStyle = ScheduleShowStyle.Both;
        }
        [Category("Infolight")]
        public ScheduleStyle DefaultStyle { get; set; }
        [Category("Infolight")]
        public ScheduleShowStyle ShowStyle { get; set; }

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

        [Category("Infolight")]
        public JQSchedule.DateFormatType DateFormat { get; set; }

        [Category("Infolight")]
        [Editor(typeof(JQTimeFormatEditor), typeof(UITypeEditor))]
        public string TimeFormat { get; set; }

        [Category("Infolight")]
        [Editor(typeof(FieldEditor), typeof(UITypeEditor))]
        public string DateField { get; set; }

        [Category("Infolight")]
        [Editor(typeof(FieldEditor), typeof(UITypeEditor))]
        public string DateToField { get; set; }

        [Category("Infolight")]
        [Editor(typeof(FieldEditor), typeof(UITypeEditor))]
        public string TimeFromField { get; set; }

        [Category("Infolight")]
        [Editor(typeof(FieldEditor), typeof(UITypeEditor))]
        public string TimeToField { get; set; }

        [Category("Infolight")]
        [Editor(typeof(FieldEditor), typeof(UITypeEditor))]
        public string TitleField { get; set; }

        [Category("Infolight")]
        [Editor(typeof(FieldEditor), typeof(UITypeEditor))]
        public string TipField { get; set; }

        [Category("Infolight")]
        public bool AllowUpdate { get; set; }

        [Category("Infolight")]
        public int DayHourFrom { get; set; }

        [Category("Infolight")]
        public int DayHourTo { get; set; }

        [Category("Infolight")]
        public int Interval { get; set; }
        
        [Category("Infolight")]
        public bool WeekSummary { get; set; }

        [Category("Infolight")]
        public Unit WeekHeight { get; set; }

        [Category("Infolight")]
        public Unit MonthHeight { get; set; }

        [Category("Infolight")]
        public string OnBeforeLoad { get; set; }

        [Category("Infolight")]
        public string OnItemFormating { get; set; }

        [Category("Infolight")]
        public string OnItemChanged { get; set; }

        [Category("Infolight")]
        public string OnItemRemoved { get; set; }

        [Category("Infolight")]
        public string OnPrevious { get; set; }

        [Category("Infolight")]
        public string OnNext { get; set; }

        [Category("Infolight")]
        public string OnSelected { get; set; }

        [Category("Infolight")]
        public string TimeFormatScript { get; set; }


        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            if (!this.DesignMode)
            {
                EFClientTools.ClientUtility.ClientInfo.cErrorCode = 8330;
                writer.AddAttribute(HtmlTextWriterAttribute.Class, JQControl.Schedule);
                var styles = new List<string>();
                if (this.Width.Type == UnitType.Pixel && Width.Value > double.Epsilon)
                {
                    styles.Add(string.Format("width:{0}px", Width.Value));

                }
                if (this.Height.Type == UnitType.Pixel && Height.Value > double.Epsilon)
                {
                    styles.Add(string.Format("height:{0}px", Height.Value));
                }
                if (styles.Count > 0)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Style, string.Join(";", styles));
                }
                writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ID);
                writer.AddAttribute(JQProperty.InfolightOptions, this.InfolightOptions);
                writer.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Div);
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
                options.Add(string.Format("defaultStyle:'{0}'", DefaultStyle.ToString().ToLower()));
                options.Add(string.Format("showStyle:'{0}'", ShowStyle.ToString().ToLower()));
                options.Add(string.Format("remoteName:'{0}'", RemoteName));
                options.Add(string.Format("tableName:'{0}'", DataMember));
                options.Add(string.Format("dateField:'{0}'", DateField));
                options.Add(string.Format("dateToField:'{0}'", DateToField));
                options.Add(string.Format("timeFromField:'{0}'", TimeFromField));
                options.Add(string.Format("timeToField:'{0}'", TimeToField));
                options.Add(string.Format("titleField:'{0}'", TitleField));
                options.Add(string.Format("tipField:'{0}'", TipField));
                options.Add(string.Format("dateFormat:'{0}'", DateFormat.ToString().ToLower()));
                options.Add(string.Format("timeFormat:'{0}'", TimeFormat.ToLower()));
                options.Add(string.Format("allowUpdate:{0}", AllowUpdate.ToString().ToLower()));
                options.Add(string.Format("dayHourFrom:{0}", DayHourFrom));
                options.Add(string.Format("dayHourTo:{0}", DayHourTo));
                options.Add(string.Format("interval:{0}", Interval));
                options.Add(string.Format("weekSummary:{0}", WeekSummary.ToString().ToLower()));
                if (!WeekHeight.IsEmpty)
                {
                    options.Add(string.Format("weekHeight:{0}", WeekHeight.Value));
                }
                if (!MonthHeight.IsEmpty)
                {
                    options.Add(string.Format("monthHeight:{0}", MonthHeight.Value));
                }
                if (!string.IsNullOrEmpty(OnBeforeLoad))
                {
                    options.Add(string.Format("onBeforeLoad:{0}", OnBeforeLoad));
                }
                if (!string.IsNullOrEmpty(OnItemFormating))
                {
                    options.Add(string.Format("onItemFormating:{0}", OnItemFormating));
                }
                if (!string.IsNullOrEmpty(OnItemChanged))
                {
                    options.Add(string.Format("onItemChanged:{0}", OnItemChanged));
                }
                if(!string.IsNullOrEmpty(OnItemRemoved))
                {
                    options.Add(string.Format("onItemRemoved:{0}", OnItemRemoved));
                }
                if (!string.IsNullOrEmpty(OnPrevious))
                {
                    options.Add(string.Format("onPrevious:{0}", OnPrevious));
                }
                if (!string.IsNullOrEmpty(OnNext))
                {
                    options.Add(string.Format("onNext:{0}", OnNext));
                }
                if (!string.IsNullOrEmpty(OnSelected))
                {
                    options.Add(string.Format("onSelected:{0}", OnSelected));
                }
                if (!string.IsNullOrEmpty(TimeFormatScript))
                {
                    options.Add(string.Format("onTimeFormat:{0}", TimeFormatScript));
                }
                return string.Join(",", options);
            }
        }

        public enum DateFormatType
        {
            DateTime,
            NVarchar
        }

        public enum ScheduleStyle
        { 
            Monthly,
            Weekly
        }

        public enum ScheduleShowStyle
        {
            Both,
            Monthly,
            Weekly
        }
    }

}
