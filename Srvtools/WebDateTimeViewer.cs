using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.Xml;
using System.Globalization;
using System.Resources;
using System.Collections;
using System.IO;
using System.Web.UI;
using System.Runtime.InteropServices;
using System.Xml.Serialization;
using System.Data;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Web.UI.Design;
using System.Drawing;
using System.Web;

namespace Srvtools
{
    [Designer(typeof(DataSourceDesigner), typeof(IDesigner))]
    [ToolboxBitmap(typeof(WebDateTimeViewer), "Resources.WebDataTimeViewer.png")]
    public class WebDateTimeViewer: WebControl,IGetValues
    {
        public WebDateTimeViewer()
        {
            //datetype = "DayMode";
            daylightonly = false;
        }

        [Browsable(false)]
        [Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
        [NotifyParentProperty(true)]
        public string Text
        {
            get
            {
                object obj = this.ViewState["Text"];
                if (obj != null)
                {
                    return (string)obj;
                }
                return string.Empty;
            }
            set
            {
                this.ViewState["Text"] = value;
            }
        }


        [Category("Infolight"),
        Description("The ID of WebDataSource which the control is bound to")]
        [Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
        [NotifyParentProperty(true)]
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


        private string datefromfield;
        [Category("Infolight"),
        Description("The column which stores the data of the day to start")]
        [Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
        [NotifyParentProperty(true)]
        public string DateFromField
        {
            get
            {
                return datefromfield;
            }
            set 
            {
                datefromfield = value;
            }
        }

        private string datetofield;
        [Category("Infolight"),
        Description("The column which stores the data of the day to end")]
        [Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
        [NotifyParentProperty(true)]
        public string DateToField
        {
            get
            {
                return datetofield;
            }
            set
            {
                datetofield = value;
            }
        }
        private string timefromfield;
        [Category("Infolight"),
        Description("The column which stores the data of the time to start")]
        [Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
        [NotifyParentProperty(true)]
        public string TimeFromField
        {
            get
            {
                return timefromfield;
            }
            set
            {
                timefromfield = value;
            }
        }
        private string timetofield;
        [Category("Infolight"),
        Description("The column which stores the data of the time to end")]
        [Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
        [NotifyParentProperty(true)]
        public string TimeToField
        {
            get
            {
                return timetofield;
            }
            set
            {
                timetofield = value;
            }
        }
        private string weekfield;
        [Category("Infolight"),
        Description("The column which stores the data of day of week")]
        [Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
        [NotifyParentProperty(true)]
        public string WeekField
        {
            get
            {
                return weekfield;
            }
            set
            {
                weekfield = value;
            }
        }
        private string monthfield;
        [Category("Infolight"),
        Description("The column which stores the data of the day of month")]
        [Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
        [NotifyParentProperty(true)]
        public string MonthField
        {
            get
            {
                return monthfield;
            }
            set
            {
                monthfield = value;
            }
        }

        private bool daylightonly;
        [Category("Infolight"),
        Description("Indicates whether the calendar display the data during the daylight time")]
        [NotifyParentProperty(true)]
        public bool DayLightOnly
        {
            get
            {
                return daylightonly;
            }
            set
            {
                daylightonly = value;
            }
        }

        private string _caption;
        [Category("Infolight"),
        Description("Caption of the page which WebDateTimeViewer opens")]
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
        //private string datetypefield;
        //[Category("Infolight")]
        //[Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
        //[NotifyParentProperty(true)]
        //public string DateTypeField
        //{
        //    get
        //    {
        //        return datetypefield;
        //    }
        //    set
        //    {
        //        datetypefield = value;
        //    }
        //}
        //private string datetype;
        //[Category("Infolight")]
        //[Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
        //[NotifyParentProperty(true)]
        //public string DateType
        //{
        //    get
        //    {
        //        return datetype;
        //    }
        //    set
        //    {
        //        datetype = value;
        //    }
        //}
        protected override void OnLoad(EventArgs e)
        {
            if (this.Page.Request.QueryString != null && this.Page.Request.QueryString["DateTimeViewerTime"] != null)
            {
                this.Text = this.Page.Request.QueryString["DateTimeViewerTime"];
            }
        }

        public  void Show()
        {   
            WebDataSource wds = (WebDataSource)GetObjByID(this.DataSourceID);
            string strDataSetID = wds.WebDataSetID;
            string strDataMember = wds.DataMember;
            string strPsyPagePath = this.Page.Request.PhysicalPath;
            string strPath = this.Page.Request.Path;
            strPsyPagePath = strPsyPagePath.Replace('\\', '/');

            string strField = DateFromField + ";" + DateToField + ";" + TimeFromField + ";" + TimeToField
            + ";" + WeekField + ";" + MonthField;

            this.Page.Response.Write("<script type='text/javascript'>window.open('../InnerPages/frmDateTimeViewer.aspx?Field="
            + strField + "&Datasetid=" + strDataSetID + "&Datamember=" + strDataMember + "&Psypagepath=" + strPsyPagePath + "&Daylightonly=" + DayLightOnly.ToString() + "&Caption=" + HttpUtility.UrlEncode(this.Caption)
            + "&Path=" + strPath + "&Day=" + DateTime.Now.ToShortDateString().Replace('/', '-') + "', 'Calendar','width=400,height=360,scrollbars=no,resizable=no,toolbar=no,menubar=no,location=no,status=no');</script>");

        }

        public void Show(string whereString)
        {
            WebDataSource wds = (WebDataSource)GetObjByID(this.DataSourceID);
            string strDataSetID = wds.WebDataSetID;
            string strDataMember = wds.DataMember;
            string strPsyPagePath = this.Page.Request.PhysicalPath;
            string strPath = this.Page.Request.Path;
            strPsyPagePath = strPsyPagePath.Replace('\\', '/');

            string strField = DateFromField + ";" + DateToField + ";" + TimeFromField + ";" + TimeToField
            + ";" + WeekField + ";" + MonthField;

            this.Page.Response.Write("<script type='text/javascript'>window.open('../InnerPages/frmDateTimeViewer.aspx?Field="
            + strField + "&Datasetid=" + strDataSetID + "&Datamember=" + strDataMember + "&Psypagepath=" + strPsyPagePath + "&Daylightonly=" + DayLightOnly.ToString() + "&Caption=" + HttpUtility.UrlEncode(this.Caption)
            + "&Path=" + strPath + "&Day=" + DateTime.Now.ToShortDateString().Replace('/', '-') + "&Wherestring=" + whereString.Replace("'", "*_*") + "', 'Calendar','width=400,height=360,scrollbars=no,resizable=no,toolbar=no,menubar=no,location=no,status=no');</script>");      
        }

        public string GetShowURL()
        {
            return GetShowURL("");    
        }

        public string GetShowURL(string whereString)
        {
            WebDataSource wds = (WebDataSource)GetObjByID(this.DataSourceID);
            string strDataSetID = wds.WebDataSetID;
            string strDataMember = wds.DataMember;
            string strPsyPagePath = this.Page.Request.PhysicalPath;
            string strPath = this.Page.Request.Path;
            strPsyPagePath = strPsyPagePath.Replace('\\', '/');

            string strField = DateFromField + ";" + DateToField + ";" + TimeFromField + ";" + TimeToField
            + ";" + WeekField + ";" + MonthField;

            string url = "../InnerPages/frmDateTimeViewer.aspx?Field="
            + strField + "&Datasetid=" + strDataSetID + "&Datamember=" + strDataMember + "&Psypagepath=" + strPsyPagePath + "&Daylightonly=" + DayLightOnly.ToString() + "&Caption=" + HttpUtility.UrlEncode(this.Caption)
            + "&Path=" + strPath + "&Day=" + DateTime.Now.ToShortDateString().Replace('/', '-') + "&Wherestring=" + whereString.Replace("'", "*_*") + "', 'Calendar','width=400,height=360";
            return url;
        
        }

        #region IGetValues Members

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

        public string[] GetValues(string sKind)
        {
            string[] retList = null;
            List<string> values = new List<string>();
            if (this is WebDateTimeViewer)
            {
                if (string.Compare(sKind, "datasourceid", true) == 0)//IgnoreCase
                {
                    ControlCollection ctrlList = ((WebDateTimeViewer)this).Page.Controls;
                    foreach (Control ctrl in ctrlList)
                    {
                        if (ctrl is WebDataSource)
                        {
                           values.Add(ctrl.ID);
                        }
                    }
                }
                else if (string.Compare(sKind, "datefromfield", true) == 0 || string.Compare(sKind,"datetofield", true) == 0 
                    || string.Compare(sKind, "timefromfield", true) == 0 || string.Compare(sKind,"timetofield", true) == 0
                    || string.Compare(sKind, "weekfield", true) == 0 || string.Compare(sKind, "monthfield", true) == 0)//IgnoreCase
                {
                    WebDateTimeViewer wdtv = (WebDateTimeViewer)this;
                    if (wdtv.Page != null && wdtv.DataSourceID != null && wdtv.DataSourceID != "")
                    {
                        foreach (Control ctrl in wdtv.Page.Controls)
                        {
                            if (ctrl is WebDataSource && ((WebDataSource)ctrl).ID == wdtv.DataSourceID)
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
                    }     
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

            return retList;
        }
        #endregion
    }
}
