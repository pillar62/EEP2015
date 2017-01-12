using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

namespace Infolight.EasilyReportTools
{
    public class ReportFormat
    {
        public ReportFormat()
        {
            this.PageRecords = 30;
            this.PageSize = PageType.A4;
            this.Orientation = Orientation.Vertical;
        }
        
        private int columnGap;

        public int ColumnGap
        {
            get { return columnGap; }
            set { columnGap = value; }
        }

        private bool columnGridLine;

        public bool ColumnGridLine
        {
            get { return columnGridLine; }
            set { columnGridLine = value; }
        }

        private bool columnInsideGridLine;

        public bool ColumnInsideGridLine
        {
            get { return columnInsideGridLine; }
            set { columnInsideGridLine = value; }
        }

        private int rowGap;

        public int RowGap
        {
            get { return rowGap; }
            set { rowGap = value; }
        }

        private bool rowGridLine;

        public bool RowGridLine
        {
            get { return rowGridLine; }
            set { rowGridLine = value; }
        }

        private int pageRecords;

        public int PageRecords
        {
            get { return pageRecords; }
            set { pageRecords = value; }
        }

        private double pageHeight;

        public double PageHeight
        {
            get { return pageHeight; }
            set { pageHeight = value; }
        }

        private PageType pageSize;

        public PageType PageSize
        {
            get { return pageSize; }
            set { pageSize = value; }
        }

        private Orientation orientation;

        public Orientation Orientation
        {
            get { return orientation; }
            set { orientation = value; }
        }

        private DateFormatType dateFormat;
        [Browsable(false)]
        public DateFormatType DateFormat
        {
            get { return dateFormat; }
            set { dateFormat = value; }
        }

        private PageIndexFormatType pageIndexFormat;
        [Browsable(false)]
        public PageIndexFormatType PageIndexFormat
        {
            get { return pageIndexFormat; }
            set { pageIndexFormat = value; }
        }

        private UserFormatType userFormat;
        [Browsable(false)]
        public UserFormatType UserFormat
        {
            get { return userFormat; }
            set { userFormat = value; }
        }

        private ExportType exportFormat;

        public ExportType ExportFormat
        {
            get { return exportFormat; }
            set { exportFormat = value; }
        }

        #region For Pdf
        private double marginLeft;

        public double MarginLeft
        {
            get { return marginLeft; }
            set { marginLeft = value; }
        }

        private double marginRight;

        public double MarginRight
        {
            get { return marginRight; }
            set { marginRight = value; }
        }

        private double marginTop;

        public double MarginTop
        {
            get { return marginTop; }
            set { marginTop = value; }
        }

        private double marginBottom;

        public double MarginBottom
        {
            get { return marginBottom; }
            set { marginBottom = value; }
        }

        #endregion

        public ReportFormat Copy()
        {
            ReportFormat reportFormat = new ReportFormat();
            CopyTo(reportFormat);
            return reportFormat;
        }

        public void CopyTo(ReportFormat reportFormat)
        {
            reportFormat.ColumnGap = this.ColumnGap;
            reportFormat.ColumnGridLine = this.ColumnGridLine;
            reportFormat.ColumnInsideGridLine = this.ColumnInsideGridLine;
            reportFormat.DateFormat = this.DateFormat;
            reportFormat.ExportFormat = this.ExportFormat;
            reportFormat.MarginLeft = this.MarginLeft;
            reportFormat.MarginRight = this.MarginRight;
            reportFormat.MarginTop = this.MarginTop;
            reportFormat.MarginBottom = this.MarginBottom;
            reportFormat.Orientation = this.Orientation;
            reportFormat.PageHeight = this.PageHeight;
            reportFormat.PageIndexFormat = this.PageIndexFormat;
            reportFormat.PageRecords = this.PageRecords;
            reportFormat.PageSize = this.PageSize;
            reportFormat.RowGap = this.RowGap;
            reportFormat.RowGridLine = this.RowGridLine;
            reportFormat.UserFormat = this.UserFormat;
        }
	
        public enum PageType
        {
            A3,
            A4,
            B4,
            B5,
            Letter
        }

        public enum DateFormatType
        {
            Date,
            DateTime
        }

        public enum PageIndexFormatType
        {
            Current,
            CurrentAndAll
        }

        public enum UserFormatType
        {
            ID,
            Name,
            IDAndName
        }

        public enum ExportType
        { 
            Excel,
            Pdf
        }
    }

   
}
