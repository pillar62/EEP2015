using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Windows.Forms;

namespace Infolight.EasilyReportTools.Tools
{
    internal class SizeInfo
    {
        private IReport report;
        public SizeInfo(IReport rpt, ReportFormat.PageType pageType)
        {
            this.report = rpt;
            double tempValue = 0;
            //PageSize
            switch (pageType)
            {
                case ReportFormat.PageType.A3:
                    this.pageWidth = 420;
                    this.pageHeight = 297;
                    break;
                case ReportFormat.PageType.A4:
                    this.pageWidth = 210;
                    this.pageHeight = 297;
                    break;
                case ReportFormat.PageType.B4:
                    this.pageWidth = 257;
                    this.pageHeight = 364;
                    break;
                case ReportFormat.PageType.B5:
                    this.pageWidth = 182;
                    this.pageHeight = 257;
                    break;
                case ReportFormat.PageType.Letter:
                    this.pageWidth = 176;
                    this.pageHeight = 125;
                    break;
            }

            if (rpt.Format.Orientation == Orientation.Horizontal)
            {
                tempValue = this.pageWidth;
                this.pageWidth = this.pageHeight;
                this.pageHeight = tempValue;
            }

            this.pageWidth = UnitConversion.MMToPound(this.pageWidth);
            this.pageHeight = UnitConversion.MMToPound(this.pageHeight);

            this.currentDetailHeight = 0;
            this.detailCaptionHeight = 0;
            this.detailRowHeight = 0;
            this.foldedRowCount = 1;
            this.sheetWidth = GetSheetWidth();
            this.fieldColumnCount = this.sheetWidth;
            //this.totalPageCount = GetTotalPageCount();
        }

        private double pageWidth;
        /// <summary>
        /// Excel page width (millimeter)
        /// </summary>
        public double PageWidth
        {
            get { return pageWidth; }
        }

        private double pageHeight;
        /// <summary>
        /// Excel page height (millimeter)
        /// </summary>
        public double PageHeight
        {
            get { return pageHeight; }
        }

        private double pageDetailHeight = 0.0;
        /// <summary>
        /// Page detail block height;
        /// </summary>
        public double PageDetailHeight
        {
            get { return pageDetailHeight; }
        }

        private double currentDetailHeight;
        /// <summary>
        /// Current detail height
        /// </summary>
        public double CurrentDetailHeight
        {
            get { return currentDetailHeight; }
            set { currentDetailHeight = value; }
        }

        private double detailCaptionHeight;
        /// <summary>
        /// Detail caption height
        /// </summary>
        public double DetailCaptionHeight
        {
            get { return detailCaptionHeight; }
        }

        private double detailRowHeight;
        /// <summary>
        /// Detail row height
        /// </summary>
        public double DetailRowHeight
        {
            get { return detailRowHeight; }
        }

        private int sheetWidth;
        /// <summary>
        /// excel sheet width (cell count, n times as great as three)
        /// </summary>
        public int SheetWidth
        {
            get { return sheetWidth; }
            set { sheetWidth = value; }
        }

        private int totalPageCount;
        /// <summary>
        /// Total print page count
        /// </summary>
        public int TotalPageCount
        {
            get { return totalPageCount; }
            set { totalPageCount = value; }
        }

        private double pageRecordsHeight  =0.0;

        public double PageRecordsHeight
        {
            get { return pageRecordsHeight; }
        }

        private int foldedRowCount; //折列后每筆資料打印幾行 

        public int FoldedRowCount
        {
            get { return foldedRowCount; }
        }

        private int fieldRowCount;

        public int FieldRowCount
        {
            get { return fieldRowCount; }
            set { fieldRowCount = value; }
        }

        private int fieldUnitNum;

        public int FieldUnitNum
        {
            get { return fieldUnitNum; }
            set { fieldUnitNum = value; }
        }

        private int fieldColumnCount;

        public int FieldColumnCount
        {
            get { return fieldColumnCount; }
            set { fieldColumnCount = value; }
        }

        private int GetSheetWidth()
        {
            int width = 0;
            for (int i = 0; i < report.FieldItems.Count; i++)
            {
                width = Math.Max(width, GetPageWidth(i));
            }
            if (pageWidth == 0)
            {
                width = 2;
            }
            return width;
        }

        private int GetPageWidth(int index)
        {
            if (index >= 0 && index < report.FieldItems.Count)
            {
                int maxWidth = 0;
                int width = 0;
                DataSourceItem fieldItem = report.FieldItems[index];
                for (int i = 0; i < fieldItem.Fields.Count; i++)
                {
                    FieldItem field = fieldItem.Fields[i];
                    if (field.NewLine && i > 0)
                    {
                        maxWidth = Math.Max(maxWidth, width);
                        width = field.NewLinePostion + field.Cells - 1;
                    }
                    else
                    {
                        width += field.Cells;
                    }
                }
                maxWidth = Math.Max(maxWidth, width);
                return maxWidth;
            }
            return 0;
        }

        //public void GetTotalPageCount(DataView view, List<int> startIndex)
        //{
        //    try
        //    {
        //        if (view.Count > 0)
        //        {
        //            startIndex.Add(0);
        //        }

        //        //按行算或者按尺寸算
        //        if (report.Format.PageHeight > double.Epsilon)
        //        {
        //            double pageSize = UnitConversion.InchToPound(report.Format.PageHeight);//设定的Page高度
        //            double itemHeight = GetItemHeight(report.HeaderItems, report.HeaderFont) + GetItemHeight(report.FooterItems, report.FooterFont)
        //                + GetFieldHeaderHeight(report.FieldItems, report.FieldFont);//固定的头高度
        //            double currentHeight = itemHeight;//当前的高度
        //            double fontheight = GetFontHeight(report.FieldFont);
        //            for (int i = 0; i < view.Count; i++)
        //            {
        //                DataRowView rowView = view[i];

        //                double rowheight = 0;
        //                double maxHeight = 0;
        //                foreach (FieldItem field in report.FieldItems)
        //                {
        //                    if (field.NewLine)
        //                    {
        //                        rowheight += maxHeight;
        //                        maxHeight = 0;
        //                    }
        //                    maxHeight = Math.Max(maxHeight, GetStringHeight(rowView[field.ColumnName].ToString(), fontheight));
        //                }
        //                rowheight += maxHeight;

        //                if (currentHeight + rowheight > pageSize)//超出高度,需要分页
        //                {
        //                    currentHeight = itemHeight + rowheight;
        //                    if (currentHeight > pageSize)
        //                    {
        //                        throw new Exception("Page size too small");
        //                    }
        //                    startIndex.Add(i);
        //                }
        //                else
        //                {
        //                    currentHeight += rowheight;
        //                }
        //            }
        //        }
        //        else if (report.Format.PageRecords != 0)
        //        {
        //            for (int i = 1; i < view.Count; i++)
        //            {
        //                if (i % report.Format.PageRecords == 0)
        //                {
        //                    startIndex.Add(i);
        //                }
        //            }
        //        }

        //        totalPageCount = startIndex.Count;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        /// <summary>
        /// Return height of reportitems in pounds
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        private double GetItemHeight(ReportItemCollection collection, System.Drawing.Font font)
        {
            double height = 0;
            double fontHeight = GetFontHeight(font);
            double maxHeight = 0;

            foreach (ReportItem item in collection)
            {
                if (item.NewLine)
                {
                    height += maxHeight;
                    maxHeight = 0;
                }
                if (item is ReportImageItem && (item as ReportImageItem).Value != null)
                {
                    maxHeight = Math.Max(maxHeight, UnitConversion.PixelToPound(((item as ReportImageItem).Value as System.Drawing.Image).Height));
                }
                else
                {
                    object value = item.Value;
                    maxHeight = Math.Max(maxHeight, GetStringHeight(value == null ? string.Empty : value.ToString(), fontHeight));
                }
            }

            return height;
        }

        private double GetFieldHeaderHeight(FieldItemCollection collection, System.Drawing.Font font)
        {
            double height = 0;
            double fontHeight = GetFontHeight(font);
            double maxHeight = 0;

            foreach (FieldItem field in collection)
            {
                if (field.NewLine)
                {
                    height += maxHeight;
                    maxHeight = 0;
                }
                maxHeight = Math.Max(maxHeight, GetStringHeight(field.Caption, fontHeight));
            }

            height += maxHeight;

            return height;
        }

        private double GetStringHeight(string str, double fontHeight)
        {
            int line = -1;
            double resValue = 0;

            if (str == null)
            {
                str = string.Empty;
            }
            line = str.Split('\n').Length;
            resValue = ((double)line) * fontHeight;

            return resValue;
        }

        private double GetFontHeight(System.Drawing.Font font)
        {
            double resValue = 0;
            double height = 0;

            height = font.GetHeight() / 1.2;
            resValue = Math.Max(height, 14.25);

            return resValue;
        }

    }
}