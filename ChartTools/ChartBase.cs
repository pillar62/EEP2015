using System;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using Srvtools;
using System.Data;
using System.Drawing.Drawing2D;

namespace ChartTools
{
    public abstract class ChartBase : PictureBox
    {
        public ChartBase()
        {
            _dataFields = new ChartFieldsCollection(this, typeof(ChartField));
        }

        #region Properties
        int _sampleRegionWidth = 50;
        [Category("Infolight")]
        [DefaultValue(50)]
        public int SampleRegionWidth
        {
            get { return _sampleRegionWidth; }
            set { _sampleRegionWidth = value; }
        }

        private int _topMargin = 30;
        [Category("Infolight")]
        [DefaultValue(30)]
        public int TopMargin
        {
            get { return _topMargin; }
            set { _topMargin = value; }
        }

        private int _leftMargin = 30;
        [Category("Infolight")]
        [DefaultValue(30)]
        public int LeftMargin
        {
            get { return _leftMargin; }
            set { _leftMargin = value; }
        }

        private int _rightMargin = 30;
        [Category("Infolight")]
        [DefaultValue(30)]
        public int RightMargin
        {
            get { return _rightMargin; }
            set { _rightMargin = value; }
        }

        private int _bottomMargin = 30;
        [Category("Infolight")]
        [DefaultValue(30)]
        public int BottomMargin
        {
            get { return _bottomMargin; }
            set { _bottomMargin = value; }
        }

        private string _caption = "";
        [Category("Infolight")]
        [DefaultValue("")]
        public string Caption
        {
            get { return _caption; }
            set { _caption = value; }
        }

        private Font _captionFont = new Font("Arial", 7, FontStyle.Regular);
        [Category("Infolight")]
        [DefaultValue(typeof(Font), "Arial, 7pt")]
        public Font CaptionFont
        {
            get { return _captionFont; }
            set { _captionFont = value; }
        }

        private Color _captionColor = Color.FromArgb(254, 0, 0);
        [Category("Infolight")]
        [DefaultValue(typeof(Color), "254, 0, 0")]
        public Color CaptionColor
        {
            get { return _captionColor; }
            set { _captionColor = value; }
        }

        private Color _backGroundStartColor = Color.YellowGreen;
        [Category("Infolight")]
        [DefaultValue(typeof(Color), "YellowGreen")]
        public Color BackGroundStartColor
        {
            get { return _backGroundStartColor; }
            set { _backGroundStartColor = value; }
        }

        private LinearGradientMode _backGroundLinearGradientMode = LinearGradientMode.Horizontal;
        [Category("Infolight")]
        [DefaultValue(typeof(LinearGradientMode), "Horizontal")]
        public LinearGradientMode BackGroundLinearGradientMode
        {
            get { return _backGroundLinearGradientMode; }
            set { _backGroundLinearGradientMode = value; }
        }

        private Color _backGroundEndColor = Color.LightYellow;
        [Category("Infolight")]
        [DefaultValue(typeof(Color), "LightYellow")]
        public Color BackGroundEndColor
        {
            get { return _backGroundEndColor; }
            set { _backGroundEndColor = value; }
        }

        private Color _fontColor = Color.FromArgb(1, 1, 1);
        [Category("Infolight")]
        [DefaultValue(typeof(Color), "1, 1, 1")]
        public Color FontColor
        {
            get { return _fontColor; }
            set { _fontColor = value; }
        }

        private bool _showValue = true;
        [Category("Infolight")]
        [DefaultValue(true)]
        public bool ShowValue
        {
            get { return _showValue; }
            set { _showValue = value; }
        }

        private InfoBindingSource _bindingSource;
        [Category("Infolight")]
        public InfoBindingSource BindingSource
        {
            get { return _bindingSource; }
            set { _bindingSource = value; }
        }

        //private SourceNumericType _srcNumType;
        //[Category("Infolight")]
        //public SourceNumericType SrcNumType
        //{
        //    get { return _srcNumType; }
        //    set { _srcNumType = value; }
        //}

        private ChartFieldsCollection _dataFields;
        [Category("Infolight")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ChartFieldsCollection DataFields
        {
            get{ return _dataFields;}
            set { _dataFields = value; }
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

        public DataTable GetDt()
        {
            if (this.BindingSource != null)
            {
                InfoDataSet infoDs = this.BindingSource.GetDataSource() as InfoDataSet;
                if (infoDs != null && infoDs.RealDataSet != null && infoDs.RealDataSet.Tables.Count > 0)
                {
                    return infoDs.RealDataSet.Tables[0];
                }
            }
            return null;
        }
    }
}
