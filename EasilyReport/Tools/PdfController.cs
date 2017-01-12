#region Copyright & License
/******************************************************************************
 * This document used "iTextSharp.dll",the Copyright notice of iTextSharp as follows:
 * 
 * Copyright 2008 by Paulo Soares.
 *
 * The contents of this file are subject to the Mozilla Public License Version 1.1
 * (the "License"); you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at http://www.mozilla.org/MPL/
 *
 * Software distributed under the License is distributed on an "AS IS" basis,
 * WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License
 * for the specific language governing rights and limitations under the License.
 *
 * The Original Code is 'iText, a free JAVA-PDF library'.
 *
 * The Initial Developer of the Original Code is Bruno Lowagie. Portions created by
 * the Initial Developer are Copyright (C) 1999, 2000, 2001, 2002 by Bruno Lowagie.
 * All Rights Reserved.
 * Co-Developer of the code is Paulo Soares. Portions created by the Co-Developer
 * are Copyright (C) 2000, 2001, 2002 by Paulo Soares. All Rights Reserved.
 *
 * Contributor(s): all the names of the contributors are added in the source code
 * where applicable.
 *
 * Alternatively, the contents of this file may be used under the terms of the
 * LGPL license (the "GNU LIBRARY GENERAL PUBLIC LICENSE"), in which case the
 * provisions of LGPL are applicable instead of those above.  If you wish to
 * allow use of your version of this file only under the terms of the LGPL
 * License and not to allow others to use your version of this file under
 * the MPL, indicate your decision by deleting the provisions above and
 * replace them with the notice and other provisions required by the LGPL.
 * If you do not delete the provisions above, a recipient may use your version
 * of this file under either the MPL or the GNU LIBRARY GENERAL PUBLIC LICENSE.
 *
 * This library is free software; you can redistribute it and/or modify it
 * under the terms of the MPL as stated above or under the terms of the GNU
 * Library General Public License as published by the Free Software Foundation;
 * either version 2 of the License, or any later version.
 *
 * This library is distributed in the hope that it will be useful, but WITHOUT
 * ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS
 * FOR A PARTICULAR PURPOSE. See the GNU Library general Public License for more
 * details.
 *
 * If you didn't download this code from the following link, you should check if
 * you aren't using an obsolete version:
 * http://www.lowagie.com/iText/
 *******************************************************************************/
#endregion

using System;
using System.Collections.Generic;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Collections;
using System.Xml;
using System.Windows.Forms;
using iTextSharp.text.pdf.draw;
using System.Data;
using Srvtools;
using System.Threading;
using Infolight.EasilyReportTools.Config;

namespace Infolight.EasilyReportTools.Tools
{
    internal class PdfController
    {
        #region Variable Definition
        public Document pdfDoc;
        private IReport report;
        private BaseFont baseFont;
        private string fontPath;
        EasilyReportLog log;
        private PdfWriter writer;
        private bool mDesignTime;
        private int mPageWidth;
        #endregion

        public PdfController(IReport rpt, ExportMode exportMode, bool mExportByHeight, bool designTime, int pageWidth)
        {
            string filePath = String.Empty;
            this.report = rpt;
            mDesignTime = designTime;
            mPageWidth = pageWidth;

            ExportByHeight = mExportByHeight;

            log = new EasilyReportLog("Pdf Report", this.GetType().FullName, LogFileInfo.logFileName, rpt);

            string path = String.Empty;

            //System.Drawing.Font font = new System.Drawing.Font("Arial", 9.0f);
            //int left = Convert.ToInt32(Math.Ceiling(UnitConversion.GetPdfLetterWidth(this.report.Format.MarginLeft, font)));
            //int right = Convert.ToInt32(Math.Ceiling(UnitConversion.GetPdfLetterWidth(this.report.Format.MarginRight, font)));
            //int top = Convert.ToInt32(Math.Ceiling(UnitConversion.GetPdfLetterWidth(this.report.Format.MarginTop, font)));
            //int bottom = Convert.ToInt32(Math.Ceiling(UnitConversion.GetPdfLetterWidth(this.report.Format.MarginBottom, font)));
            double left = UnitConversion.InchToPound(this.report.Format.MarginLeft);
            double right = UnitConversion.InchToPound(this.report.Format.MarginRight);
            double top = UnitConversion.InchToPound(this.report.Format.MarginTop);
            double bottom = UnitConversion.InchToPound(this.report.Format.MarginBottom);
            pdfDoc = new Document(this.GetPageSize(rpt.Format.PageSize, rpt.Format.Orientation), 
                (float)left, (float)right, (float)top, (float)bottom);

            if (exportMode == ExportMode.Export)
            {
                filePath = this.report.FilePath;
            }
            else
            {
                filePath = ExportFileInfo.PdfPreviewFilePath;
            }

            writer = PdfWriter.GetInstance(pdfDoc, new FileStream(filePath, FileMode.Create));

            pdfDoc.Open();

            //path = Application.StartupPath + "\\" + CliUtils.fCurrentProject + "\\";
            //BaseFont.AddToResourceSearch(path + "iTextAsian.dll");
            //BaseFont.AddToResourceSearch(path + "iTextAsianCmaps.dll");

            //baseFont = BaseFont.CreateFont("STSong-Light", "UniGB-UCS2-H", BaseFont.EMBEDDED);

            fontPath = Environment.GetFolderPath(Environment.SpecialFolder.System).Substring(0, Environment.GetFolderPath(Environment.SpecialFolder.System).LastIndexOf("\\")) + @"\Fonts";

            FontFactory.RegisterDirectory(fontPath);
        }

        public void NewPage()
        {
            this.pdfDoc.NewPage();
        }

        public void Dispose()
        {
            pdfDoc.Close();
            pdfDoc = null;
        }

        public void SetEmpty()
        {
            pdfDoc.Add(new Chunk());
        }

        public void Save(string fileName)
        { 
            
        }

        public bool FitsPage(Table tb)
        {
            return FitsPage(tb, 0);
        }

        public bool FitsPage(Table tb, float margin)
        {
            return writer.FitsPage(tb, margin);
        }

        #region Properties
        private Table headerTable;
        /// <summary>
        /// Header Table
        /// </summary>
        public Table HeaderTable
        {
            get { return headerTable; }
            set { headerTable = value; }
        }

        private Table footerTable;
        /// <summary>
        /// Footer Table
        /// </summary>
        public Table FooterTable
        {
            get { return footerTable; }
            set { footerTable = value; }
        }

        private bool exportByHeight;
        /// <summary>
        /// 是否计算高度
        /// </summary>
        public bool ExportByHeight
        {
            get { return exportByHeight; }
            set { exportByHeight = value; }
        }

        private double headerHeight = 0.0;
        /// <summary>
        /// Report Header Height
        /// </summary>
        public double HeaderHeight
        {
            get { return headerHeight; }
            set { headerHeight = value; }
        }

        private double footerHeight = 0.0;
        /// <summary>
        /// Report Footer Height
        /// </summary>
        public double FooterHeight
        {
            get { return footerHeight; }
            set { footerHeight = value; }
        }
        #endregion

        public void ExportToPdf(List<List<object>> headerList, List<List<PdfDesc>> fieldList, List<List<object>> footerList, int multiRow)
        {
            try
            {
                AddTableToPdf(WriteItem(headerList, report.HeaderFont));
                WriteFields(fieldList, report.FieldFont, multiRow, true);
                AddTableToPdf(WriteItem(footerList, report.FooterFont));
            }
            catch (Exception ex)
            {
                log.WriteExceptionInfo(ex);
                throw ex;
            }
        }

        private void AddTableToPdf(Table tb)
        {
            if (tb != null)
            {
                pdfDoc.Add(tb);
            }
        }

        public Table WriteItem(List<List<object>> lists, System.Drawing.Font sysFont)
        {
            #region Variable Definition
            //int tempCount = 0;
            int cellCount = 0;
            int maxColumnCount = -1;
            Font pdfFont = null;
            Table tb = null;
            Image image = null;
            string imagePath = String.Empty;
            float imageWidth = float.Epsilon;
            Cell cell = null;
            double height = 0.0;
            #endregion

            //try
            //{
                pdfFont = this.GetPdfFont(sysFont);

                //计算Table中最大的Column数。
                #region Old Function
                //foreach (List<object> list in lists)
                //{
                //    foreach (ReportItem item in list)
                //    {
                //        tempCount += item.Cells;
                //    }
                //    if (tempCount > maxColumnCount)
                //    {
                //        maxColumnCount = tempCount;
                //    }
                //    tempCount = 0;
                //}
                #endregion

                maxColumnCount = mPageWidth;
                
                if (lists.Count == 0)
                {
                    return tb;
                }

                tb = new Table(maxColumnCount, lists.Count);
                tb.Border = Rectangle.NO_BORDER;

                tb.Cellpadding = PdfSizeConfig.Cellpadding;
                //tb.Width = ((this.pdfDoc.PageSize.Width - this.pdfDoc.LeftMargin - this.pdfDoc.RightMargin) / this.pdfDoc.PageSize.Width) * 100; //此處為百分比
                tb.Width = 100;
                tb.Alignment = Element.ALIGN_LEFT;

                foreach (List<object> list in lists)
                {
                    cellCount = 0;

                    height += GetHeight(list, sysFont);

                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i].GetType().Name != "ReportImageItem")
                        {
                            if (((ReportItem)list[i]).Font == null)
                            {
                                pdfFont = this.GetPdfFont(sysFont);
                            }
                            else
                            {
                                pdfFont = this.GetPdfFont(((ReportItem)list[i]).Font);
                            }
                            string value = "";
                            string format = "";

                            if (string.IsNullOrEmpty(((ReportItem)list[i]).Format))
                            {
                                if (list[i] is ReportDataSourceItem)
                                {
                                    DDProvider ddProvider = new DDProvider(report.HeaderDataSource, mDesignTime);
                                    string ddValue = ddProvider.GetDDValue(((ReportDataSourceItem)list[i]).ColumnName, DDInfo.FieldCaption).ToString();
                                    if (ddValue == "")
                                    {
                                        ddValue = ddProvider.GetDDValue(((ReportDataSourceItem)list[i]).ColumnName, DDInfo.FieldName).ToString();
                                    }
                                    format = ddValue + ":{0}";
                                }
                                else
                                {
                                    format = "{0}";
                                }
                            }
                            else
                            {
                                format = ((ReportItem)list[i]).Format;
                            }

                            value = string.IsNullOrEmpty(((ReportItem)list[i]).Format) ? string.Format(format, ((ReportItem)list[i]).Value) :
                                String.Format(format, ((ReportItem)list[i]).Value);

                            cell = new Cell(new Chunk(value, pdfFont));
                        }
                        else
                        {
                            #region Image Item
                            image = Image.GetInstance((System.Drawing.Image)((ReportItem)list[i]).Value, System.Drawing.Imaging.ImageFormat.Jpeg);

                            image.Alignment = this.GetPdfImageHAlign(((ReportItem)list[i]).ContentAlignment);

                            if (list.Count > 1)
                            {
                                imageWidth = (this.pdfDoc.PageSize.Width - this.pdfDoc.LeftMargin - this.pdfDoc.RightMargin) / (float)list.Count;
                                //imageWidth -= (float)16;
                                //image.ScaleAbsoluteWidth(imageWidth);
                            }
                            Chunk chuck = new Chunk(image, 0, 0, true);
                            cell = new Cell(chuck);
                            #endregion
                        }

                        cell.UseAscender = true; //此屬性設置為True的時候VerticalAlignment才會起作用
                        cell.VerticalAlignment = Cell.ALIGN_MIDDLE;

                        cell.HorizontalAlignment = this.GetPdfHAlign(((ReportItem)list[i]).ContentAlignment);

                        if (((ReportItem)list[i]).Cells == 0)
                        {
                            if (tb.Columns - cellCount != 0)
                            {
                                cell.Colspan = tb.Columns - cellCount;
                            }
                        }
                        else
                        {
                            cell.Colspan = ((ReportItem)list[i]).Cells;
                        }
                        cell.Border = Rectangle.NO_BORDER;

                        if (((ReportItem)list[i]).Position == ReportItem.PositionAlign.Right && ((ReportItem)list[i]).Cells != 0)
                        {
                            if (maxColumnCount - cellCount - ((ReportItem)list[i]).Cells != 0)
                            {
                                Cell tempCell = new Cell();
                                tempCell.Colspan = maxColumnCount - cellCount - ((ReportItem)list[i]).Cells;
                                tempCell.Border = Rectangle.NO_BORDER;
                                tb.AddCell(tempCell);

                                cellCount = maxColumnCount - ((ReportItem)list[i]).Cells;
                            }
                        }

                        if (i == 0 && ((ReportItem)list[i]).Position != ReportItem.PositionAlign.Right)
                        {
                            tb.AddCell(cell, lists.IndexOf(list), i);
                        }
                        else
                        {
                            tb.AddCell(cell, lists.IndexOf(list), cellCount);
                        }

                        cellCount += ((ReportItem)list[i]).Cells;
                    }
                }

                if (ExportByHeight)
                {
                    if (HeaderTable == null && object.ReferenceEquals(sysFont, report.HeaderFont))
                    {
                        HeaderTable = tb;

                        if (HeaderHeight == 0.0)
                        {
                            HeaderHeight = height;
                        }

                    }
                    else if (FooterTable == null && object.ReferenceEquals(sysFont, report.FooterFont))
                    {
                        FooterTable = tb;

                        if (FooterHeight == 0.0)
                        {
                            FooterHeight = height;
                        }
                    }
                }
                else
                {
                    this.pdfDoc.Add(tb);
                }
            //}
            //catch (Exception ex)
            //{
            //    log.WriteExceptionInfo(ex);
            //    throw ex;
            //}

            return tb;
        }

        private double GetHeight(List<object> list, System.Drawing.Font sysFont)
        {
            double maxHeight = 0.0;
            double height = 0.0;

            foreach (ReportItem item in list)
            {
                if (item is ReportImageItem)
                {
                    height = UnitConversion.PixelToPound(Convert.ToDouble(((System.Drawing.Image)((ReportImageItem)item).Value).Height));
                }
                else
                {
                    height = UnitConversion.GetLetterHeight(sysFont);
                }

                if (height > maxHeight)
                {
                    maxHeight = height;
                }
            }

            maxHeight += Convert.ToDouble(PdfSizeConfig.Cellpadding);

            return maxHeight;
        }

        /// <summary>
        /// 输出到PDF
        /// </summary>
        /// <param name="list"></param>
        /// <param name="sysFont"></param>
        /// <param name="multiRow"></param>
        /// <param name="export">算高度的情况最后输出时使用</param>
        /// <returns></returns>
        public Table WriteFields(List<List<PdfDesc>> list, System.Drawing.Font sysFont, int multiRow, bool export)
        {
            #region Variable Definition
            Cell cell = null;
            int maxColumnCount = -1;
            int maxRowCount = -1;
            LineSeparator lineSeparator = null;
            int tempCount = 0;
            int previousFieldCells = 0;
            Table tb = null;
            #endregion

            //try
            //{
                Font pdfFont = this.GetPdfFont(sysFont);

                //Hashtable allStartIndex = new Hashtable();

                Dictionary<int, int> allStartIndex = new Dictionary<int, int>();

                if (export)
                {
                    foreach (List<PdfDesc> row in list)
                    {
                        if (!allStartIndex.ContainsKey(row[0].FieldNum))
                        {
                            allStartIndex.Add(row[0].FieldNum, list.IndexOf(row));
                        }
                    }
                }
                else
                {
                    allStartIndex.Add(0, 0);
                }

                List<int> startIndex = new List<int>();

                foreach (int index in allStartIndex.Values)
                {
                    startIndex.Add(index);
                }

                for (int l = 0; l < startIndex.Count; l++)
                {
                    //計算最大Column和最大Row

                    maxColumnCount = 0;

                    if (startIndex.Count == 1)
                    { 
                        maxRowCount = list.Count;
                    }
                    else if (l != startIndex.Count - 1)
                    {
                        maxRowCount = startIndex[l + 1] - startIndex[l];
                    }
                    else
                    {
                        maxRowCount = list.Count - startIndex[l];
                    }
                    
                    for (int s = startIndex[l]; s < list.Count; s++)
                    //foreach (List<PdfDesc> row in list)
                    {
                        if (startIndex.Count != 1)
                        {
                            if (l != startIndex.Count - 1 && s == startIndex[l + 1])
                            {
                                break;
                            }
                        }
                        
                        List<PdfDesc> row = list[s];
                        
                        foreach (PdfDesc pdfDesc in row)
                        {
                            tempCount += pdfDesc.Cells;
                        }

                        if (tempCount > maxColumnCount)
                        {
                            maxColumnCount = tempCount;
                        }

                        tempCount = 0;
                    }

                    tb = new Table(maxColumnCount, maxRowCount);

                    #region 計算欄位寬度
                    if (multiRow == 1)
                    {
                        int[] widths = new int[maxColumnCount];

                        previousFieldCells = 0;

                        List<PdfDesc> firstRow = list[startIndex[l]];

                        for (int i = 0; i < firstRow.Count; i++)
                        {
                            int widthPercent = Convert.ToInt32(Math.Truncate((UnitConversion.GetPdfLetterWidth(firstRow[i].Width, sysFont)
                                / Convert.ToDouble((this.pdfDoc.PageSize.Width - this.pdfDoc.LeftMargin - this.pdfDoc.RightMargin))) * 100)); //算出百分比

                            if (i == 0)
                            {
                                widths[i] = widthPercent;

                                if (firstRow[i].Cells > 1)
                                {
                                    for (int j = 0; j < firstRow[i].Cells - 1; j++)
                                    {
                                        widths[i + j + 1] = widthPercent;
                                    }
                                }
                            }
                            else
                            {
                                widths[previousFieldCells] = widthPercent;

                                if (firstRow[i].Cells > 1)
                                {
                                    for (int j = 0; j < firstRow[i].Cells - 1; j++)
                                    {
                                        widths[previousFieldCells + j + 1] = widthPercent;
                                    }
                                }
                            }

                            previousFieldCells += firstRow[i].Cells;
                        }

                        tb.SetWidths(widths);

                        previousFieldCells = 0;
                    }
                    #endregion

                    if (!this.report.Format.ColumnGridLine)
                    {
                        tb.Border = Rectangle.NO_BORDER;
                    }

                    tb.Cellpadding = PdfSizeConfig.Cellpadding;
                    //tb.Width = ((this.pdfDoc.PageSize.Width - this.pdfDoc.LeftMargin - this.pdfDoc.RightMargin) / this.pdfDoc.PageSize.Width) * 100; //此處為百分比
                    tb.Width = 100;
                    tb.Alignment = Element.ALIGN_LEFT;

                    for (int j = startIndex[l]; j < list.Count; j++)
                    {
                        if (startIndex.Count != 1)
                        {
                            if (l != startIndex.Count - 1 && j == startIndex[l + 1])
                            {
                                break;
                            }
                        }
                        
                        List<PdfDesc> row = list[j];

                        previousFieldCells = 0;
                        for (int i = 0; i < row.Count; i++)
                        {
                            PdfDesc pdfDesc = row[i];

                            switch (pdfDesc.GroupGap)
                            {
                                case DataSourceItem.GroupGapType.None:
                                    cell = new Cell(new Chunk(pdfDesc.Value, pdfFont));
                                    cell.Colspan = pdfDesc.Cells;
                                    cell.HorizontalAlignment = this.GetPdfHAlignByStr(pdfDesc.HAlign);
                                    break;
                                case DataSourceItem.GroupGapType.EmptyRow:
                                    if (i == 0)
                                    {
                                        cell = new Cell(new Chunk(String.Empty, pdfFont));
                                        cell.Colspan = maxColumnCount;
                                    }
                                    break;
                                case DataSourceItem.GroupGapType.SingleLine:
                                    if (i == 0)
                                    {
                                        cell = new Cell();
                                        lineSeparator = new LineSeparator();
                                        lineSeparator.LineWidth = cell.Width;
                                        lineSeparator.Offset = PdfSizeConfig.LineSeparatorOffsetU;
                                        cell.AddElement(lineSeparator);
                                        cell.Colspan = tb.Columns;
                                    }
                                    break;
                                case DataSourceItem.GroupGapType.DoubleLine:
                                    if (i == 0)
                                    {
                                        cell = new Cell();
                                        lineSeparator = new LineSeparator();
                                        lineSeparator.LineWidth = cell.Width;
                                        lineSeparator.Offset = PdfSizeConfig.LineSeparatorOffsetU;
                                        cell.AddElement(lineSeparator);
                                        lineSeparator = new LineSeparator();
                                        lineSeparator.LineWidth = cell.Width;
                                        lineSeparator.Offset = PdfSizeConfig.LineSeparatorOffsetD;
                                        cell.AddElement(lineSeparator);
                                        cell.Colspan = tb.Columns;
                                    }
                                    break;
                            }

                            cell.BorderWidthLeft = pdfDesc.LeftLine == true ? PdfSizeConfig.BorderWidth : PdfSizeConfig.BorderWidthZero;
                            cell.BorderWidthRight = pdfDesc.RightLine == true ? PdfSizeConfig.BorderWidth : PdfSizeConfig.BorderWidthZero;
                            cell.BorderWidthTop = pdfDesc.TopLine == true ? PdfSizeConfig.BorderWidth : PdfSizeConfig.BorderWidthZero;
                            cell.BorderWidthBottom = pdfDesc.BottomLine == true ? PdfSizeConfig.BorderWidth : PdfSizeConfig.BorderWidthZero;

                            if (j == list.Count - 1)
                            {
                                cell.BorderWidthBottom = report.Format.RowGridLine == true ? PdfSizeConfig.BorderWidth : PdfSizeConfig.BorderWidthZero;
                            }
                            
                            cell.UseAscender = true; //此屬性設置為True的時候VerticalAlignment才會起作用
                            cell.VerticalAlignment = Cell.ALIGN_MIDDLE;


                            switch (pdfDesc.GroupGap)
                            {
                                case DataSourceItem.GroupGapType.None:
                                    if (i == 0)
                                    {
                                        tb.AddCell(cell, j, i);
                                    }
                                    else
                                    {
                                        tb.AddCell(cell, j, previousFieldCells);
                                    }
                                    break;
                                case DataSourceItem.GroupGapType.EmptyRow:
                                case DataSourceItem.GroupGapType.SingleLine:
                                case DataSourceItem.GroupGapType.DoubleLine:
                                    if (i == 0)
                                    {
                                        tb.AddCell(cell, j, i);
                                    }
                                    break;
                            }

                            previousFieldCells += pdfDesc.Cells;
                        }
                    }

                    if (!ExportByHeight || export)
                    {
                        this.pdfDoc.Add(tb);
                    }
                }
            //}
            //catch (Exception ex)
            //{
            //    log.WriteExceptionInfo(ex);
            //    throw ex;
            //}

            return tb;
        }

        public Table WriteFields(List<List<PdfDesc>> list, System.Drawing.Font sysFont, int multiRow)
        {
            return WriteFields(list, sysFont, multiRow, !ExportByHeight);
        }

        public int FitsTableToPage(List<List<PdfDesc>> list, System.Drawing.Font sysFont, int multiRow)
        {
            int delCount = 0;

            Table fieldTable = WriteFields(list, sysFont, multiRow);
            while(!writer.FitsPage(fieldTable, (float)GetRestHeight(multiRow)))
            {
                fieldTable.DeleteLastRow();
                delCount++;
            }

            return delCount;
        }

        private float GetRestHeight(int multiRow)
        {
            float restHeight = 0f;

            System.Drawing.Font font = new System.Drawing.Font("Arial", 9.0f);
            restHeight = (pdfDoc.PageSize.Height - (float)UnitConversion.InchToPound(report.Format.PageHeight)) + (float)HeaderHeight + (float)FooterHeight
                //上下边距在iTextSharp的PdfWriter的FitsPage方法里面已经计算掉了，这边再次计算的话会导致死循环。
                //+ (float)UnitConversion.InchToPound((double)report.Format.MarginTop) + (float)UnitConversion.InchToPound((double)report.Format.MarginBottom)
                + (float)UnitConversion.GetLetterHeight(font) * (float)multiRow + PdfSizeConfig.Cellpadding * 2f * (float)multiRow;

            if (restHeight < 0) //小于零的时候以零来算
            {
                restHeight = 0f;
            }

            return restHeight;
        }

        private Table GetAllTable(Table fieldTable)
        {
            Table allTable = new Table(1);
            int k = 0;

            if (HeaderTable != null)
            {
                allTable.InsertTable(HeaderTable, new System.Drawing.Point(k, 0));
                k++;
            }
            allTable.InsertTable(fieldTable, new System.Drawing.Point(k, 0));
            k++;
            if (FooterTable != null)
            {
                allTable.InsertTable(FooterTable, new System.Drawing.Point(k, 0));
            }

            //float[] widthf = new float[fieldTable.ProportionalWidths.Length];
            //fieldTable.ProportionalWidths.CopyTo(widthf, 0);

            //int[] widths = new int[fieldTable.ProportionalWidths.Length];
            //for (int i = 0; i < widthf.Length; i++)
            //{
            //    widths[i] = Convert.ToInt32(widthf[i]);
            //}
            //allTable.SetWidths(widths);
            return allTable;
        }

        #region Common Function
        private Rectangle GetPageSize(ReportFormat.PageType pageType, Orientation orientation)
        {
            Rectangle pageSize = PageSize.A4;

            switch (pageType)
            {
                case ReportFormat.PageType.A3:
                    pageSize = PageSize.A3;
                    break;
                case ReportFormat.PageType.A4:
                    pageSize = PageSize.A4;
                    break;
                case ReportFormat.PageType.B4:
                    pageSize = PageSize.B4;
                    break;
                case ReportFormat.PageType.B5:
                    pageSize = PageSize.B5;
                    break;
                case ReportFormat.PageType.Letter:
                    pageSize = PageSize.LETTER;
                    break;
                default:
                    pageSize = PageSize.A4;
                    break;
            }

            if (orientation == Orientation.Horizontal)
            {
                pageSize = pageSize.Rotate();
            }
            return pageSize;
        }

        private Font GetPdfFont(System.Drawing.Font sysFont)
        {
            Font pdfFont = null;
            int font = -1;

            if (sysFont.Bold && sysFont.Italic)
            {
                font = Font.BOLDITALIC;
            }
            else if (sysFont.Bold)
            {
                font = Font.BOLD;
            }
            else if (sysFont.Italic)
            {
                font = Font.ITALIC;
            }
            else
            {
                font = Font.NORMAL;
            }

            if (sysFont.Strikeout)
            {
                font = font | Font.STRIKETHRU;
            }

            if (sysFont.Underline)
            {
                font = font | Font.UNDERLINE;
            }

            switch (sysFont.FontFamily.GetName(1033).ToLower())
            {
                case "basemic symbol":
                case "basemic times":
                case "basemicnew":
                case "kingsoft phonetic":
                case "mt extra":
                    pdfFont = FontFactory.GetFont(sysFont.Name, sysFont.Size, font);
                    break;

                default:
                    baseFont = BaseFont.CreateFont(fontPath + "\\" + FontNameMapper.GetFontFileName(sysFont), BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                    pdfFont = new Font(baseFont, sysFont.Size, font);
                    break;
            }

            return pdfFont;
        }

        private int GetPdfHAlignByStr(HorizontalAlignment hAlign)
        {
            int pdfHAlign = -1;

            switch (hAlign)
            {
                case HorizontalAlignment.Center:
                    pdfHAlign = Element.ALIGN_CENTER;
                    break;
                case HorizontalAlignment.Left:
                    pdfHAlign = Element.ALIGN_LEFT;
                    break;
                case HorizontalAlignment.Right:
                    pdfHAlign = Element.ALIGN_RIGHT;
                    break;
            }

            return pdfHAlign;
        }

        private int GetPdfHAlign(HorizontalAlignment hAlign)
        {
            int pdfHAlign = -1;

            switch (hAlign)
            {
                case HorizontalAlignment.Center:
                    pdfHAlign = Element.ALIGN_CENTER;
                    break;
                case HorizontalAlignment.Left:
                    pdfHAlign = Element.ALIGN_LEFT;
                    break;
                case HorizontalAlignment.Right:
                    pdfHAlign = Element.ALIGN_RIGHT;
                    break;
            }

            return pdfHAlign;
        }

        private int GetPdfImageHAlign(HorizontalAlignment hAlign)
        {
            int pdfImageHAlign = -1;

            switch (hAlign)
            {
                case HorizontalAlignment.Center:
                    pdfImageHAlign = Image.MIDDLE_ALIGN;
                    break;
                case HorizontalAlignment.Left:
                    pdfImageHAlign = Image.LEFT_ALIGN;
                    break;
                case HorizontalAlignment.Right:
                    pdfImageHAlign = Image.RIGHT_ALIGN;
                    break;
            }

            return pdfImageHAlign;
        }
        #endregion
    }

    internal class PdfDesc
    {
        public PdfDesc()
        {
            topLine = false;
            bottomLine = false;
            leftLine = false;
            rightLine = false;
            groupGap = DataSourceItem.GroupGapType.None;
            cells = 1;
        }
        
        private string value;

        public string Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        private HorizontalAlignment hAlign;

        public HorizontalAlignment HAlign
        {
            get { return hAlign; }
            set { hAlign = value; }
        }

        private int cells;

        public int Cells
        {
            get { return cells; }
            set { cells = value; }
        }

        private int width;

        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        private bool topLine;

        public bool TopLine
        {
            get { return topLine; }
            set { topLine = value; }
        }

        private bool bottomLine;

        public bool BottomLine
        {
            get { return bottomLine; }
            set { bottomLine = value; }
        }

        private bool leftLine;

        public bool LeftLine
        {
            get { return leftLine; }
            set { leftLine = value; }
        }

        private bool rightLine;

        public bool RightLine
        {
            get { return rightLine; }
            set { rightLine = value; }
        }

        private Infolight.EasilyReportTools.DataSourceItem.GroupGapType groupGap;

        public Infolight.EasilyReportTools.DataSourceItem.GroupGapType GroupGap
        {
            get { return groupGap; }
            set { groupGap = value; }
        }

        private int fieldNum;

        public int FieldNum
        {
            get { return fieldNum; }
            set { fieldNum = value; }
        }

        private bool isCaption;

        public bool IsCaption
        {
            get { return isCaption; }
            set { isCaption = value; }
        }
    }
}
