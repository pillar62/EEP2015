using System.Web.UI;
using System.ComponentModel;
using System.Drawing.Design;
using System.Data;
using Srvtools;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ChartTools
{
    public abstract class WebChartBase : System.Web.UI.WebControls.Image, IChartDataSource
    {
        public WebChartBase()
        {
            _dataFields = new WebChartFieldsCollection(this, typeof(WebChartField));
        }

        #region Properties
        [Category("Infolight")]
        [DefaultValue(50)]
        public int SampleRegionWidth
        {
            get
            {
                object obj = this.ViewState["SampleRegionWidth"];
                if (obj != null)
                {
                    return (int)obj;
                }
                return 50;
            }
            set
            {
                this.ViewState["SampleRegionWidth"] = value;
            }
        }

        [Category("Infolight")]
        [DefaultValue(30)]
        public int TopMargin
        {
            get
            {
                object obj = this.ViewState["TopMargin"];
                if (obj != null)
                {
                    return (int)obj;
                }
                return 30;
            }
            set
            {
                this.ViewState["TopMargin"] = value;
            }
        }

        [Category("Infolight")]
        [DefaultValue(30)]
        public int LeftMargin
        {
            get
            {
                object obj = this.ViewState["LeftMargin"];
                if (obj != null)
                {
                    return (int)obj;
                }
                return 30;
            }
            set
            {
                this.ViewState["LeftMargin"] = value;
            }
        }

        [Category("Infolight")]
        [DefaultValue(30)]
        public int RightMargin
        {
            get
            {
                object obj = this.ViewState["RightMargin"];
                if (obj != null)
                {
                    return (int)obj;
                }
                return 30;
            }
            set
            {
                this.ViewState["RightMargin"] = value;
            }
        }

        [Category("Infolight")]
        [DefaultValue(30)]
        public int BottomMargin
        {
            get
            {
                object obj = this.ViewState["BottomMargin"];
                if (obj != null)
                {
                    return (int)obj;
                }
                return 30;
            }
            set
            {
                this.ViewState["BottomMargin"] = value;
            }
        }

        [Category("Infolight")]
        [DefaultValue("")]
        public string Caption
        {
            get
            {
                object obj = this.ViewState["Caption"];
                if (obj != null)
                {
                    return (string)obj;
                }
                return "";
            }
            set
            {
                this.ViewState["Caption"] = value;
            }
        }

        [Category("Infolight")]
        [DefaultValue(typeof(Font), "Arial, 7pt")]
        public Font CaptionFont
        {
            get
            {
                object obj = this.ViewState["CaptionFont"];
                if (obj != null)
                {
                    return (Font)obj;
                }
                return new Font("Arial", 7, FontStyle.Regular);
            }
            set
            {
                this.ViewState["CaptionFont"] = value;
            }
        }

        [Category("Infolight")]
        [DefaultValue(typeof(Color), "254, 0, 0")]
        public Color CaptionColor
        {
            get
            {
                object obj = this.ViewState["CaptionColor"];
                if (obj != null)
                {
                    return (Color)obj;
                }
                return Color.FromArgb(254, 0, 0);
            }
            set
            {
                this.ViewState["CaptionColor"] = value;
            }
        }

        [Category("Infolight")]
        [DefaultValue(typeof(Color), "YellowGreen")]
        public Color BackGroundStartColor
        {
            get
            {
                object obj = this.ViewState["BackGroundStartColor"];
                if (obj != null)
                {
                    return (Color)obj;
                }
                return Color.YellowGreen;
            }
            set
            {
                this.ViewState["BackGroundStartColor"] = value;
            }
        }

        [Category("Infolight")]
        [DefaultValue(typeof(Color), "LightYellow")]
        public Color BackGroundEndColor
        {
            get
            {
                object obj = this.ViewState["BackGroundEndColor"];
                if (obj != null)
                {
                    return (Color)obj;
                }
                return Color.LightYellow;
            }
            set
            {
                this.ViewState["BackGroundEndColor"] = value;
            }
        }

        [Category("Infolight")]
        [DefaultValue(typeof(LinearGradientMode), "Horizontal")]
        public LinearGradientMode BackGroundLinearGradientMode
        {
            get
            {
                object obj = this.ViewState["BackGroundLinearGradientMode"];
                if (obj != null)
                {
                    return (LinearGradientMode)obj;
                }
                return LinearGradientMode.Horizontal;
            }
            set
            {
                this.ViewState["BackGroundLinearGradientMode"] = value;
            }
        }

        [Category("Infolight")]
        [DefaultValue(typeof(Color), "1, 1, 1")]
        public Color FontColor
        {
            get
            {
                object obj = this.ViewState["FontColor"];
                if (obj != null)
                {
                    return (Color)obj;
                }
                return Color.FromArgb(1, 1, 1);
            }
            set
            {
                this.ViewState["FontColor"] = value;
            }
        }

        [Category("Infolight")]
        [DefaultValue(true)]
        public bool ShowValue
        {
            get
            {
                object obj = this.ViewState["ShowValue"];
                if (obj != null)
                {
                    return (bool)obj;
                }
                return true;
            }
            set
            {
                this.ViewState["ShowValue"] = value;
            }
        }
        #endregion

        protected int genSampleRegionWidth()
        {
            if (isNumColumn() && !string.IsNullOrEmpty(this.DataFields[0].CaptionFieldName))
            {
                return this.SampleRegionWidth;
            }
            return 0;
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
                if (this.Page.Form != null)
                    return GetAllCtrls(ObjID, this.Page.Form);
                else
                    return GetAllCtrls(ObjID, this.Page);
            }
        }

        protected bool isNumColumn()
        {
            if (this.DataFields.Count == 1)
            {
                IChartField field = this.DataFields[0];
                if (!string.IsNullOrEmpty(field.CaptionFieldName))
                {
                    return false;
                }
            }
            return true;
        }

        protected int getUnitCount(int rowCount)
        {
            int unitCount = 0;
            if (isNumColumn())
                unitCount = this.DataFields.Count;
            else
                unitCount = rowCount;
            return unitCount;
        }

        #region IChartDataSource Members

        [Category("Infolight")]
        [Editor(typeof(DataSourceEditor), typeof(UITypeEditor))]
        public string DataSourceID
        {
            get
            {
                object obj = this.ViewState["DataSourceID"];
                if (obj != null)
                {
                    return (string)obj;
                }
                return "";
            }
            set
            {
                this.ViewState["DataSourceID"] = value;
            }
        }

        private WebChartFieldsCollection _dataFields;
        [Category("Infolight")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [NotifyParentProperty(true)]
        [TypeConverter(typeof(CollectionConverter))]
        public WebChartFieldsCollection DataFields
        {
            get
            {
                return _dataFields;
            }
        }

        [Browsable(false)]
        public string ResxDataSet
        {
            get
            {
                object obj = this.ViewState["ResxDataSet"];
                if (obj != null)
                {
                    return (string)obj;
                }
                return "";
            }
            set
            {
                this.ViewState["ResxDataSet"] = value;
            }
        }

        //[Category("Infolight")]
        //public SourceNumericType SrcNumType
        //{
        //    get
        //    {
        //        object obj = this.ViewState["SrcNumType"];
        //        if (obj != null)
        //        {
        //            return (SourceNumericType)obj;
        //        }
        //        return SourceNumericType.ColumnNumricType;
        //    }
        //    set
        //    {
        //        this.ViewState["SrcNumType"] = value;
        //    }
        //}

        public string GetDataSetID()
        {
            string value = "";
            if (this.ResxDataSet != null && this.ResxDataSet != "")
                value = this.ResxDataSet;
            else
            {
                object wds = this.GetObjByID(this.DataSourceID);
                if (wds is WebDataSource && ((WebDataSource)wds).WebDataSetID != null && ((WebDataSource)wds).WebDataSetID != "")
                {
                    value = ((WebDataSource)wds).WebDataSetID;
                }
            }
            return value;
        }

        public DataTable GetDt()
        {
            Control ctrl = this.Page.FindControl(this.DataSourceID);
            if (ctrl != null && ctrl is WebDataSource)
            {
                WebDataSource wds = (WebDataSource)ctrl;
                if (string.IsNullOrEmpty(wds.SelectAlias) && string.IsNullOrEmpty(wds.SelectCommand))
                    return wds.InnerDataSet.Tables[0];
                else
                    return wds.CommandTable;
            }
            return null;
        }

        #endregion

    }
}
