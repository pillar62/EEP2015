using System;
using Excel;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Drawing;
using System.IO;
using System.Collections.Generic;

namespace Infolight.EasilyReportTools.Tools
{
    /// <summary>
    /// ExcelController 的摘要说明。
    /// </summary>
    internal class ExcelController
    {
        #region Variable Definition
        /// <summary>
        /// Excel
        /// </summary>
        public Excel.Application CurExcel = null;

        /// <summary>
        /// Workbook
        /// </summary>
        public Excel._Workbook CurBook = null;

        /// <summary>
        /// Worksheet
        /// </summary>
        public Excel._Worksheet CurSheet = null;

        private object mValue = System.Reflection.Missing.Value;

        private System.DateTime dtBefore;
        private System.DateTime dtAfter;
        private int sheetIndex;

        private Excel.Range objRange = null;
        private int pictureIndex;

        private EasilyReportLog log;
        #endregion

        public ExcelController(bool excelVisible, IReport report)
        {
            dtBefore = System.DateTime.Now;

            CurExcel = new Excel.ApplicationClass();
            CurExcel.DisplayAlerts = false;
            dtAfter = System.DateTime.Now;

            CurExcel.Visible = excelVisible;

            //CurExcel.Workbooks.Add(true);
            CurExcel.Workbooks.Add(mValue);
            sheetIndex = 1;
            CurSheet = CurExcel.Worksheets[sheetIndex] as Excel.Worksheet;
            CurBook = CurExcel.Workbooks[1];

            pictureIndex = 0;
            log = new EasilyReportLog("Excel Report", this.GetType().FullName, LogFileInfo.logFileName, report);
        }

        #region Write Value to Excel
        /// <summary>
        /// Write value to worksheet
        /// </summary>
        /// <param name="point"></param>
        /// <param name="objValue">ReportItem or FieldItem or other value</param>
        public void WriteValue(PointInfo point, object objValue)
        {
            WriteValue(point, null, objValue);
        }

        /// <summary>
        /// Write value to worksheet
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        /// <param name="objValue">ReportItem or FieldItem or other value</param>
        public void WriteValue(PointInfo startPoint, PointInfo endPoint, object objValue)
        {
            #region Variable Definition
            object[] cell = null;
            object startCell = null;
            object endCell = null;
            #endregion

            cell = this.ConvertPointToCell(startPoint, endPoint);
            startCell = cell[0];
            endCell = cell[1];

            objRange = CurSheet.get_Range(startCell, endCell);

            if (startCell.ToString() != endCell.ToString())
            {
                objRange.Merge(mValue);
            }

            objRange.Value2 = objValue;
        }

        public void WriteFormatedValue(PointInfo startPoint, PointInfo endPoint, ReportItem reportItem)
        {
            #region Variable Definition
            object[] cell = null;
            object startCell = null;
            object endCell = null;
            #endregion

            cell = this.ConvertPointToCell(startPoint, endPoint);
            startCell = cell[0];
            endCell = cell[1];

            objRange = CurSheet.get_Range(startCell, endCell);

            if (startCell.ToString() != endCell.ToString())
            {
                objRange.Merge(mValue);
            }

            switch (reportItem.ContentAlignment)
            {
                case HorizontalAlignment.Center:
                    objRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    break;
                case HorizontalAlignment.Left:
                    objRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                    break;
                case HorizontalAlignment.Right:
                    objRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                    break;
                default:
                    objRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                    break;
            }

            if (reportItem.GetType().Name == "ReportConstantItem")
            {
                switch (((ReportConstantItem)reportItem).Style)
                {
                    case ReportConstantItem.StyleType.PageIndex:
                    case ReportConstantItem.StyleType.PageIndexAndTotalPageCount:
                        if (reportItem.Format == String.Empty)
                        {
                            objRange.Value2 = String.Format(reportItem.Format, "'" + reportItem.Value);
                        }
                        else
                        {
                            objRange.Value2 = String.Format(reportItem.Format, reportItem.Value);
                        }
                        break;
                    default:
                        objRange.Value2 = String.Format(reportItem.Format, reportItem.Value);
                        break;
                }
            }
            else
            {
                objRange.Value2 = String.Format(reportItem.Format, reportItem.Value);
            }

            
        }

        public void Merge(PointInfo startPoint, PointInfo endPoint)
        {
            object[] cell = this.ConvertPointToCell(startPoint, endPoint);
            object startCell = cell[0];
            object endCell = cell[1];
            if (startCell.ToString() != endCell.ToString())
            {
                Range range = this.CurSheet.get_Range(startCell, endCell);
                range.Merge(mValue);
                range = null;
            }
        }

        public double WriteFieldValue(PointInfo startPoint, PointInfo endPoint, List<List<string>> lists, System.Drawing.Font font)
        {
            #region Variable Definition
            object[] cell = null;
            object startCell = null;
            object endCell = null;
            #endregion

            cell = this.ConvertPointToCell(startPoint, endPoint);
            startCell = cell[0];
            endCell = cell[1];

            objRange = this.CurSheet.get_Range(startCell, endCell);
            if (startCell.Equals(endCell))
            {
                objRange.set_Value(mValue, lists[0][0]);
            }
            else
            {
                object[,] objValue = (object[,])objRange.get_Value(mValue);

                for (int i = 0; i < lists.Count; i++)
                {
                    List<string> row = lists[i];
                    int mergeStart = 0;
                    int mergeCount = 1;
                    for (int j = 0; j < row.Count; j++)
                    {
                        //merge cell计算
                        if (row[j] != null && row[j].Equals(ExcelReportExporter.MERGE_CELL))
                        {
                            objValue[i + 1, j + 1] = string.Empty;
                            mergeCount++;
                        }
                        else
                        {
                            if (mergeCount > 1)
                            {
                                //merge
                                Merge(new PointInfo(startPoint.RowIndex + i, mergeStart), new PointInfo(startPoint.RowIndex + i, mergeStart + mergeCount - 1));
                                mergeCount = 1;
                            }
                           
                            objValue[i + 1, j + 1] = row[j];
                            mergeStart = j;
                        }
                        if (j == row.Count - 1 && mergeCount > 1)
                        {
                            //merge
                            Merge(new PointInfo(startPoint.RowIndex + i, mergeStart), new PointInfo(startPoint.RowIndex + i, mergeStart + mergeCount - 1));
                        }
                    }
                }
                objRange.set_Value(mValue, objValue);
            }
            SetFont(startPoint, endPoint, font);

            return (double)objRange.Height;
        }

        public void Replace(object what, object replacement)
        {
            this.CurSheet.UsedRange.Replace(what, replacement, mValue, mValue, mValue, mValue, mValue, mValue);
        }

        public double CopyRange(int sheetIndex, PointInfo desStartPoint, int PageIndex)
        {
            Worksheet sheet = this.CurBook.Sheets[sheetIndex] as Worksheet;

            int column = sheet.UsedRange.Columns.Count;
            int row = sheet.UsedRange.Rows.Count;

            PointInfo desEndPoint = new PointInfo(desStartPoint.RowIndex + row - 1, desStartPoint.ColumnIndex + column - 1);

            object[] cell = this.ConvertPointToCell(desStartPoint, desEndPoint);
            object desStartCell = cell[0];
            object desEndCell = cell[1];

            Range desRange = this.CurSheet.get_Range(desStartCell, desEndCell);
            int picCount = (this.CurSheet.Pictures(mValue) as Pictures).Count;
            sheet.UsedRange.Copy(desRange);
            int deleteCount = (this.CurSheet.Pictures(mValue) as Pictures).Count - picCount;
            if (deleteCount > 0)
            {
                for (int i = 0; i < deleteCount; i++)
                {
                    (this.CurSheet.Pictures(picCount + 1) as Picture).Delete();
                }
            }
            desStartPoint.RowIndex += sheet.UsedRange.Rows.Count;
            desStartPoint.ColumnIndex = 0;
            picCount = (sheet.Pictures(mValue) as Pictures).Count;
            if (picCount > 0)
            {
                for (int i = 1; i <= row; i++)
                {
                    (desRange.Rows[i, mValue] as Excel.Range).RowHeight = (sheet.UsedRange[i, 1] as Excel.Range).RowHeight;
                }
                for (int i = 1; i <= picCount; i++)
                {
                    Picture pic = sheet.Pictures(i) as Picture;
                    pic.Copy();
                    this.CurSheet.Paste(mValue, mValue);
                    Picture picCopy = this.CurSheet.Pictures((PageIndex - 1) * picCount + i) as Picture;
                    picCopy.Top = pic.Top + (double)desRange.Top;
                    picCopy.Left = pic.Left + (double)desRange.Left;
                }              
            }
            desRange.Replace(ExcelReportExporter.PAGE_INDEX, PageIndex, mValue, mValue, mValue, mValue, mValue, mValue);
            return (double)sheet.UsedRange.Height;
        }

        public double CopyRange(int sheetIndex, PointInfo srcStartPoint, PointInfo srcEndPoint, PointInfo desStartPoint)
        {
            Worksheet sheet = this.CurBook.Sheets[sheetIndex] as Worksheet;

            object[] cell = this.ConvertPointToCell(srcStartPoint, srcEndPoint);
            object srcStartCell = cell[0];
            object srcEndCell = cell[1];
            Range objRange = sheet.get_Range(srcStartCell, srcEndCell);

            int column = objRange.Columns.Count;
            int row = objRange.Rows.Count;

            PointInfo desEndPoint = new PointInfo(desStartPoint.RowIndex + row - 1, desStartPoint.ColumnIndex + column - 1);

            cell = this.ConvertPointToCell(desStartPoint, desEndPoint);
            object desStartCell = cell[0];
            object desEndCell = cell[1];
            Range desRange = this.CurSheet.get_Range(desStartCell, desEndCell);

            objRange.Copy(desRange);

            desStartPoint.RowIndex += row;
            desStartPoint.ColumnIndex = 0;
            return (double)objRange.Height;
            
        }



        public void CopyRange(PointInfo srcStartPoint, PointInfo srcEndPoint, PointInfo desStartPoint)
        {
            #region Variable Definition
            object[] cell = null;
            object srcStartCell = null;
            object srcEndCell = null;
            object desStartCell = null;
            object desEndCell = null;
            Excel.Range desRange = null;
            #endregion

            cell = this.ConvertPointToCell(srcStartPoint, srcEndPoint);
            srcStartCell = cell[0];
            srcEndCell = cell[1];

            objRange = this.CurSheet.get_Range(srcStartCell, srcEndCell);

            cell = this.ConvertPointToCell(desStartPoint, desStartPoint);
            desStartCell = cell[0];
            desEndCell = cell[1];

            desRange = this.CurSheet.get_Range(desStartCell, desStartCell);

            objRange.Copy(desRange);
            desRange = null;
        }

        public void DeleteSheet(int sheetIndex)
        {
            (this.CurBook.Sheets[sheetIndex] as Excel.Worksheet).Delete();
        }

        public void DeleteRow(PointInfo startPoint, PointInfo endPoint)
        {
            object[] cell = this.ConvertPointToCell(startPoint, endPoint);
            object startCell = cell[0];
            object endCell = cell[1];

            objRange = CurSheet.get_Range(startCell, endCell);
            objRange.Select();
            objRange.Rows.Delete(Excel.XlDirection.xlUp);//待测试
        }

        /// <summary>
        /// Insert picture to excel sheet
        /// </summary>
        /// <param name="point"></param>
        /// <param name="imagePath"></param>
        public void InsertPicutre(PointInfo point, Image image)
        {
            InsertPicutre(point, null, image);
        }
        
        /// <summary>
        /// Insert picture to excel sheet
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        /// <param name="imagePath"></param>
        public void InsertPicutre(PointInfo startPoint, PointInfo endPoint, Image image)
        {
            #region Variable Definition
            object[] cell = null;
            object startCell = null;
            object endCell = null;
            string imagePath = "";
            #endregion

            cell = this.ConvertPointToCell(startPoint, endPoint);
            startCell = cell[0];
            endCell = cell[1];

            objRange = CurSheet.get_Range(startCell, endCell);

            if (startCell.ToString() != endCell.ToString())
            {
                objRange.Merge(mValue);
            }

            imagePath = @"C:\" + Guid.NewGuid().ToString() + ".jpg";
            ((Bitmap)image).Save(imagePath);
            
            ((Excel.Pictures)CurSheet.Pictures(mValue)).Insert(imagePath, mValue);
            pictureIndex++;
            ((Excel.Picture)CurSheet.Pictures(pictureIndex)).Left = (double)objRange.Left;
            ((Excel.Picture)CurSheet.Pictures(pictureIndex)).Top = (double)objRange.Top;
            double height = ((Excel.Picture)CurSheet.Pictures(pictureIndex)).Height;

            if ((double)objRange.RowHeight < height)
            {
                objRange.RowHeight = height;
            }
            File.Delete(imagePath);
        }
        #endregion

        #region Get Excel Info
        /// <summary>
        /// Get excel cell width
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public double GetCellWidth(PointInfo point)
        {
            return GetAreaWidth(point, null);
        }

        /// <summary>
        /// Get excel area width
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        /// <returns></returns>
        public double GetAreaWidth(PointInfo startPoint, PointInfo endPoint)
        {
            #region Variable Definition
            object[] cell = null;
            object startCell = null;
            object endCell = null;
            #endregion

            cell = this.ConvertPointToCell(startPoint, endPoint);
            startCell = cell[0];
            endCell = cell[1];
            return Convert.ToDouble(CurSheet.get_Range(startCell, endCell).Width);
        }

        /// <summary>
        /// Get excel cell height
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public double GetCellHeight(PointInfo point)
        {
            return GetAreaHeight(point, null);
        }

        /// <summary>
        /// Get excel cell height
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        /// <returns></returns>
        public double GetAreaHeight(PointInfo startPoint, PointInfo endPoint)
        {
            #region Variable Definition
            object[] cell = null;
            object startCell = null;
            object endCell = null;
            #endregion

            cell = this.ConvertPointToCell(startPoint, endPoint);
            startCell = cell[0];
            endCell = cell[1];
            return Convert.ToDouble(CurSheet.get_Range(startCell, endCell).Height);
        }
        #endregion

        #region Change Excel Format (Cell、Column、Row and so on)
        /// <summary>
        /// Set font of excel sheet cell
        /// </summary>
        /// <param name="point"></param>
        /// <param name="font"></param>
        public void SetFont(PointInfo point, System.Drawing.Font font)
        {
            SetFont(point, null, font);
        }

        /// <summary>
        /// Set font of excel sheet area
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        /// <param name="font"></param>
        public void SetFont(PointInfo startPoint, PointInfo endPoint, System.Drawing.Font font)
        {
            #region Variable Definition
            object[] cell = null;
            object startCell = null;
            object endCell = null;
            #endregion

            cell = this.ConvertPointToCell(startPoint, endPoint);
            startCell = cell[0];
            endCell = cell[1];

            if (font != null)
            {

                objRange = CurSheet.get_Range(startCell, endCell);
                if (font.Bold)
                {
                    objRange.Font.Bold = font.Bold;
                }
                if (font.Italic)
                {
                    objRange.Font.Italic = font.Italic;
                }
                objRange.Font.Name = font.Name;
                objRange.Font.Size = font.Size;
                if (font.Strikeout)
                {
                    objRange.Font.Strikethrough = font.Strikeout;
                }
                if (font.Underline)
                {
                    objRange.Font.Underline = font.Underline;
                }

            }
        }

        /// <summary>
        /// Set HorizontalAlignment of excel sheet cell
        /// </summary>
        /// <param name="point"></param>
        /// <param name="hAlignment"></param>
        public void SetHorizontalAlignment(PointInfo point, HorizontalAlignment hAlignment)
        {
            SetHorizontalAlignment(point, null, hAlignment);
        }

        /// <summary>
        /// Set HorizontalAlignment of excel sheet area
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        /// <param name="hAlignment"></param>
        public void SetHorizontalAlignment(PointInfo startPoint, PointInfo endPoint, HorizontalAlignment hAlignment)
        {
            #region Variable Definition
            object[] cell = null;
            object startCell = null;
            object endCell = null;
            #endregion

            cell = this.ConvertPointToCell(startPoint, endPoint);
            startCell = cell[0];
            endCell = cell[1];

            switch (hAlignment)
            {
                case HorizontalAlignment.Center:
                    CurSheet.get_Range(startCell, endCell).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    break;
                case HorizontalAlignment.Left:
                    CurSheet.get_Range(startCell, endCell).HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                    break;
                case HorizontalAlignment.Right:
                    CurSheet.get_Range(startCell, endCell).HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                    break;
                default:
                    CurSheet.get_Range(startCell, endCell).HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                    break;
            }
        }

        /// <summary>
        /// Set excel column width
        /// </summary>
        /// <param name="point"></param>
        /// <param name="font">letter font</param>
        /// <param name="width">unit by letter count</param>
        public void SetColumnWidth(PointInfo point, System.Drawing.Font font, int width)
        {
            SetColumnWidth(point, null, font, width);
        }
        
        /// <summary>
        /// Set excel column width
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        /// <param name="font">letter font</param>
        /// <param name="width">unit by letter count</param>
        public void SetColumnWidth(PointInfo startPoint, PointInfo endPoint, System.Drawing.Font font, int width)
        {
            #region Variable Definition
            object[] cell = null;
            object startCell = null;
            object endCell = null;
            #endregion

            cell = this.ConvertPointToCell(startPoint, endPoint);
            startCell = cell[0];
            endCell = cell[1];

            //CurSheet.get_Range(startCell, endCell).Merge(mValue);
            //endCell = mValue;

            if (width != 0)
            {
                ((Range)CurSheet.Columns[startPoint.ColumnIndex + 1, mValue]).ColumnWidth = UnitConversion.GetLetterWidth(width, font);
                //CurSheet.get_Range(startCell, endCell).ColumnWidth = UnitConversion.GetLetterWidth(width, font);
            }
        }

        public void AutoFit(PointInfo startPoint, PointInfo endPoint)
        {
            #region Variable Definition
            object[] cell = null;
            object startCell = null;
            object endCell = null;
            #endregion

            cell = this.ConvertPointToCell(startPoint, endPoint);
            startCell = cell[0];
            endCell = cell[1];

            //CurSheet.get_Range(startCell, endCell).Merge(mValue);
            //endCell = mValue;

            CurSheet.get_Range(startCell, endCell).AutoFit();
        }

        /// <summary>
        /// Set excel row height
        /// </summary>
        /// <param name="point"></param>
        /// <param name="font">letter font</param>
        /// <param name="height">unit by letter count</param>
        public void SetRowHeight(PointInfo point, System.Drawing.Font font, int height)
        {
            SetRowHeight(point, null, font, height);
        }

        /// <summary>
        /// Set excel row height
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        /// <param name="font">letter font</param>
        /// <param name="height">unit by letter count</param>
        public void SetRowHeight(PointInfo startPoint, PointInfo endPoint, System.Drawing.Font font, int height)
        {
            #region Variable Definition
            object[] cell = null;
            object startCell = null;
            object endCell = null;
            #endregion

            cell = this.ConvertPointToCell(startPoint, endPoint);
            startCell = cell[0];
            endCell = cell[1];

            objRange = CurSheet.get_Range(startCell, endCell);

            if (startCell.ToString() != endCell.ToString())
            { 
                objRange.Merge(mValue);
            }

            if (height != 0)
            {
                objRange.RowHeight = UnitConversion.GetLetterHeight(height, font);
            }
        }

        public void SetRowHeight(PointInfo startPoint, PointInfo endPoint, double height)
        {
            #region Variable Definition
            object[] cell = null;
            object startCell = null;
            object endCell = null;
            #endregion

            cell = this.ConvertPointToCell(startPoint, endPoint);
            startCell = cell[0];
            endCell = cell[1];

            objRange = CurSheet.get_Range(startCell, endCell);

            if (startCell.ToString() != endCell.ToString())
            {
                objRange.Merge(mValue);
            }

            if (height != 0)
            {
                objRange.RowHeight = height;
            }
        }

        /// <summary>
        /// Set column grid line
        /// </summary>
        /// <param name="point"></param>
        public void SetColumnGridLine(PointInfo point)
        {
            SetColumnGridLine(point, null);
        }

        public void SetColumnGridLine(PointInfo startPoint, PointInfo endPoint)
        {
            SetColumnGridLine(startPoint, endPoint);
        }

        /// <summary>
        /// Set column grid line
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        public void SetColumnGridLine(PointInfo startPoint, PointInfo endPoint, bool inside)
        {
            #region Variable Definition
            object[] cell = null;
            object startCell = null;
            object endCell = null;
            #endregion

            cell = this.ConvertPointToCell(startPoint, endPoint);
            startCell = cell[0];
            endCell = cell[1];

            CurSheet.get_Range(startCell, endCell).Borders[Excel.XlBordersIndex.xlEdgeLeft].Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);
            CurSheet.get_Range(startCell, endCell).Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlLineStyleNone;
            CurSheet.get_Range(startCell, endCell).Borders[Excel.XlBordersIndex.xlEdgeLeft].Weight = Excel.XlBorderWeight.xlThin;

            CurSheet.get_Range(startCell, endCell).Borders[Excel.XlBordersIndex.xlEdgeRight].Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);
            CurSheet.get_Range(startCell, endCell).Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlLineStyleNone;
            CurSheet.get_Range(startCell, endCell).Borders[Excel.XlBordersIndex.xlEdgeRight].Weight = Excel.XlBorderWeight.xlThin;

            //CurSheet.get_Range(startCell, endCell).Borders[Excel.XlBordersIndex.xlInsideHorizontal].Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);
            //CurSheet.get_Range(startCell, endCell).Borders[Excel.XlBordersIndex.xlInsideHorizontal].LineStyle = Excel.XlLineStyle.xlContinuous;

            if (startPoint.ColumnIndex != endPoint.ColumnIndex && inside)
            {
                CurSheet.get_Range(startCell, endCell).Borders[Excel.XlBordersIndex.xlInsideVertical].Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);
                CurSheet.get_Range(startCell, endCell).Borders[Excel.XlBordersIndex.xlInsideVertical].LineStyle = Excel.XlLineStyle.xlContinuous;
                CurSheet.get_Range(startCell, endCell).Borders[Excel.XlBordersIndex.xlInsideVertical].Weight = Excel.XlBorderWeight.xlThin;
            }
        }

        /// <summary>
        /// Set row grid line
        /// </summary>
        /// <param name="point"></param>
        public void SetRowGridLine(PointInfo point, bool innerLine)
        {
            SetRowGridLine(point, null, innerLine);
        }

        /// <summary>
        /// Set row grid line
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        public void SetRowGridLine(PointInfo startPoint, PointInfo endPoint, bool innerLine)
        {
            #region Variable Definition
            object[] cell = null;
            object startCell = null;
            object endCell = null;
            #endregion

            cell = this.ConvertPointToCell(startPoint, endPoint);
            startCell = cell[0];
            endCell = cell[1];

            CurSheet.get_Range(startCell, endCell).Borders[Excel.XlBordersIndex.xlEdgeTop].Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);
            CurSheet.get_Range(startCell, endCell).Borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.XlLineStyle.xlContinuous;
            CurSheet.get_Range(startCell, endCell).Borders[Excel.XlBordersIndex.xlEdgeTop].Weight = Excel.XlBorderWeight.xlThin;

            CurSheet.get_Range(startCell, endCell).Borders[Excel.XlBordersIndex.xlEdgeBottom].Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);
            CurSheet.get_Range(startCell, endCell).Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
            CurSheet.get_Range(startCell, endCell).Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = Excel.XlBorderWeight.xlThin;

            if (endPoint.RowIndex != startPoint.RowIndex)
            {
                CurSheet.get_Range(startCell, endCell).Borders[Excel.XlBordersIndex.xlInsideHorizontal].Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);
                CurSheet.get_Range(startCell, endCell).Borders[Excel.XlBordersIndex.xlInsideHorizontal].Weight = Excel.XlBorderWeight.xlThin;
                CurSheet.get_Range(startCell, endCell).Borders[Excel.XlBordersIndex.xlInsideHorizontal].LineStyle = innerLine ? Excel.XlLineStyle.xlContinuous : XlLineStyle.xlLineStyleNone;
            }
           

            //CurSheet.get_Range(startCell, endCell).Borders[Excel.XlBordersIndex.xlInsideVertical].Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);
            //CurSheet.get_Range(startCell, endCell).Borders[Excel.XlBordersIndex.xlInsideVertical].LineStyle = Excel.XlLineStyle.xlContinuous;
        }
        #endregion

        #region Excel sheet setting
        public void PageBreak(PointInfo point)
        {
            #region Variable Definition
            object[] cell = null;
            object startCell = null;
            object endCell = null;
            #endregion

            cell = this.ConvertPointToCell(point, null);
            startCell = cell[0];
            endCell = cell[1];

            CurSheet.HPageBreaks.Add(CurSheet.get_Range(startCell, mValue));
        }

        public void PageWidthBreak(PointInfo point)
        {
            #region Variable Definition
            object[] cell = null;
            object startCell = null;
            object endCell = null;
            #endregion

            cell = this.ConvertPointToCell(point, null);
            startCell = cell[0];
            endCell = cell[1];
            
            CurSheet.VPageBreaks.Add(CurSheet.get_Range(startCell, mValue));
        }
        #endregion

        #region Group By

        public void Group(PointInfo startPoint, PointInfo endPoint, FieldItem.SumType sumType, int groupIndex, int[] subTotalList)
        {
            #region Variable Definition
            object[] cell = null;
            object startCell = null;
            object endCell = null;
            #endregion

            cell = this.ConvertPointToCell(startPoint, endPoint);
            startCell = cell[0];
            endCell = cell[1];

            CurSheet.get_Range(startCell, endCell).Subtotal(groupIndex, GetExcelSumType(sumType), subTotalList, true, false, XlSummaryRow.xlSummaryBelow);
        }

        private XlConsolidationFunction GetExcelSumType(FieldItem.SumType sumType)
        {
            XlConsolidationFunction excelSumType = XlConsolidationFunction.xlUnknown;
            switch (sumType)
            {
                case FieldItem.SumType.None:
                    excelSumType = XlConsolidationFunction.xlUnknown;
                    break;
                case FieldItem.SumType.Sum:
                    excelSumType = XlConsolidationFunction.xlSum;
                    break;
                case FieldItem.SumType.Count:
                    excelSumType = XlConsolidationFunction.xlCount;
                    break;
                case FieldItem.SumType.Max:
                    excelSumType = XlConsolidationFunction.xlMax;
                    break;
                case FieldItem.SumType.Min:
                    excelSumType = XlConsolidationFunction.xlMin;
                    break;
                case FieldItem.SumType.Average:
                    excelSumType = XlConsolidationFunction.xlAverage;
                    break;
            }
            return excelSumType;
        }
        #endregion

        #region Common Function
        /// <summary>
        /// Convert point to excel cell
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        /// <returns></returns>
        private object[] ConvertPointToCell(PointInfo startPoint, PointInfo endPoint)
        {
            object startCell = null;
            object endCell = null;

            startCell = (((int)(startPoint.ColumnIndex / 26) > 0) ? Convert.ToChar((startPoint.ColumnIndex / 26) + 64).ToString() : "") + Convert.ToChar((startPoint.ColumnIndex % 26) + 65).ToString() + Convert.ToString(startPoint.RowIndex + 1);

            if (endPoint != null)
            {
                endCell = (((int)(endPoint.ColumnIndex / 26) > 0) ? Convert.ToChar((endPoint.ColumnIndex / 26) + 64).ToString() : "") + Convert.ToChar((endPoint.ColumnIndex % 26) + 65).ToString() + Convert.ToString(endPoint.RowIndex + 1);
            }
            else
            {
                endCell = mValue;
            }

            return new object[2] { startCell, endCell };
        }

        #endregion

        #region Excel File Operation
        /// <summary>
        /// Save excel
        /// </summary>
        /// <param name="fileName">full Path</param>
        public void SaveExcel(string fileName)
        {
            this.CurBook.SaveAs(fileName, mValue, mValue, mValue, mValue, mValue, Excel.XlSaveAsAccessMode.xlNoChange, mValue, mValue, mValue, mValue, mValue);
        }

        /// <summary>
        /// Save as excel
        /// </summary>
        public void SaveAsExcel()
        {
            SaveFileDialog saveFileDialog;
            saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel|*.xls|Excel2007|*.xlsx";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                //this.CurExcel.Save(saveFileDialog.FileName);
                this.CurBook.SaveAs(saveFileDialog.FileName, mValue, mValue, mValue, mValue, mValue, Excel.XlSaveAsAccessMode.xlNoChange, mValue, mValue, mValue, mValue, mValue);
                this.CurExcel.Quit();
            }
        }

        /// <summary>
        /// Save as excel
        /// </summary>
        /// <param name="fileName">full Path</param>
        public void SaveAsExcel(string fileName)
        {
            this.CurBook.SaveAs(fileName, mValue, mValue, mValue, mValue, mValue, Excel.XlSaveAsAccessMode.xlExclusive, XlSaveConflictResolution.xlUserResolution, mValue, mValue, mValue, mValue);
            this.CurExcel.Quit();
        }

        /// <summary>
        /// Add sheet
        /// </summary>
        public void AddSheet()
        {
            this.sheetIndex++;
            if (this.CurBook.Sheets.Count < sheetIndex)
            {
                this.CurSheet = (Excel.Worksheet)this.CurBook.Sheets.Add(mValue, this.CurBook.Sheets[sheetIndex - 1], 1, Excel.XlSheetType.xlWorksheet);
            }
            else
            {
                this.CurSheet = this.CurExcel.Worksheets[sheetIndex] as Excel.Worksheet;
            }
            pictureIndex = 0;
        }

        public void SetSheetLandscape()
        {
            this.CurSheet.PageSetup.Orientation = XlPageOrientation.xlLandscape;
        }

        #endregion

        public void SetAutoFilter(string strStartCelID, string strEndCelID)
        {
            CurSheet.get_Range(strStartCelID, strEndCelID).AutoFilter(1, mValue,
             Excel.XlAutoFilterOperator.xlAnd, mValue, true);
        }

        /// <summary>
        /// 合并单元格，并在合并后的单元格中插入指定的值
        /// </summary>
        /// <param name="strStartCell"></param>
        /// <param name="strEndCell"></param>
        /// <param name="objValue"></param>
        public void WriteAfterMerge(string strStartCell, string strEndCell, object objValue)
        {
            CurSheet.get_Range(strStartCell, strEndCell).Merge(mValue);
            CurSheet.get_Range(strStartCell, mValue).Value2 = objValue;
        }
        /// <summary>
        /// 为单元格设置公式
        /// </summary>
        /// <param name="strCell">单元格标识符</param>
        /// <param name="strFormula">公式</param>
        public void SetFormula(string strCell, string strFormula)
        {
            CurSheet.get_Range(strCell, mValue).Formula = strFormula;
        }
        /// <summary>
        /// 设置单元格或连续区域的字体为黑体
        /// </summary>
        /// <param name="strCell">单元格标识符</param>
        public void SetBold(string strCell)
        {
            CurSheet.get_Range(strCell, mValue).Font.Bold = true;
        }
        /// <summary>
        /// 设置单元格或连续区域的字体为黑体
        /// </summary>
        /// <param name="strCell">单元格标识符</param>
        public void SetBold(string strStartCell, string strEndCell)
        {
            CurSheet.get_Range(strStartCell, strEndCell).Font.Bold = true;
        }
        /// <summary>
        /// 设置单元格或连续区域的边框：上下左右都为黑色连续边框
        /// </summary>
        /// <param name="strCell">单元格标识符</param>
        public void SetBorderAll(string strCell)
        {
            CurSheet.get_Range(strCell, mValue).Borders[Excel.XlBordersIndex.xlEdgeTop].Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);
            CurSheet.get_Range(strCell, mValue).Borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.XlLineStyle.xlContinuous;
            CurSheet.get_Range(strCell, mValue).Borders[Excel.XlBordersIndex.xlEdgeTop].Weight = Excel.XlBorderWeight.xlThick;

            CurSheet.get_Range(strCell, mValue).Borders[Excel.XlBordersIndex.xlEdgeBottom].Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);
            CurSheet.get_Range(strCell, mValue).Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
            CurSheet.get_Range(strCell, mValue).Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = Excel.XlBorderWeight.xlThick;

            CurSheet.get_Range(strCell, mValue).Borders[Excel.XlBordersIndex.xlEdgeLeft].Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);
            CurSheet.get_Range(strCell, mValue).Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlContinuous;

            CurSheet.get_Range(strCell, mValue).Borders[Excel.XlBordersIndex.xlEdgeRight].Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);
            CurSheet.get_Range(strCell, mValue).Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;
            CurSheet.get_Range(strCell, mValue).Borders[Excel.XlBordersIndex.xlEdgeRight].Weight = Excel.XlBorderWeight.xlThick;

            CurSheet.get_Range(strCell, mValue).Borders[Excel.XlBordersIndex.xlInsideHorizontal].Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);
            CurSheet.get_Range(strCell, mValue).Borders[Excel.XlBordersIndex.xlInsideHorizontal].LineStyle = Excel.XlLineStyle.xlContinuous;

            CurSheet.get_Range(strCell, mValue).Borders[Excel.XlBordersIndex.xlInsideVertical].Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);
            CurSheet.get_Range(strCell, mValue).Borders[Excel.XlBordersIndex.xlInsideVertical].LineStyle = Excel.XlLineStyle.xlContinuous;
        }

        /// <summary>
        /// 设置连续区域的边框：上下左右都为黑色连续边框
        /// </summary>
        /// <param name="strStartCell">开始单元格标识符</param>
        /// <param name="strEndCell">结束单元格标识符</param>
        public void SetBorderAll(string strStartCell, string strEndCell, Excel.XlBorderWeight xlEdgeWeight, Excel.XlBorderWeight xlInsideWeight)
        {
            CurSheet.get_Range(strStartCell, strEndCell).Borders[Excel.XlBordersIndex.xlEdgeTop].Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);
            CurSheet.get_Range(strStartCell, strEndCell).Borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.XlLineStyle.xlContinuous;
            CurSheet.get_Range(strStartCell, strEndCell).Borders[Excel.XlBordersIndex.xlEdgeTop].Weight = xlEdgeWeight;

            CurSheet.get_Range(strStartCell, strEndCell).Borders[Excel.XlBordersIndex.xlEdgeBottom].Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);
            CurSheet.get_Range(strStartCell, strEndCell).Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
            CurSheet.get_Range(strStartCell, strEndCell).Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = xlEdgeWeight;

            CurSheet.get_Range(strStartCell, strEndCell).Borders[Excel.XlBordersIndex.xlEdgeLeft].Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);
            CurSheet.get_Range(strStartCell, strEndCell).Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlContinuous;
            CurSheet.get_Range(strStartCell, strEndCell).Borders[Excel.XlBordersIndex.xlEdgeLeft].Weight = xlEdgeWeight;

            CurSheet.get_Range(strStartCell, strEndCell).Borders[Excel.XlBordersIndex.xlEdgeRight].Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);
            CurSheet.get_Range(strStartCell, strEndCell).Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;
            CurSheet.get_Range(strStartCell, strEndCell).Borders[Excel.XlBordersIndex.xlEdgeRight].Weight = xlEdgeWeight;

            string iStart = "", iEnd = "";
            Regex r = new Regex(@"(\d+)", RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Match m = r.Match(strStartCell);
            if (m.Success)
            {
                iStart = m.Groups[1].ToString();
            }
            m = r.Match(strEndCell);
            if (m.Success)
            {
                iEnd = m.Groups[1].ToString();
            }
            if (iStart == iEnd)
                return;


            CurSheet.get_Range(strStartCell, strEndCell).Borders[Excel.XlBordersIndex.xlInsideHorizontal].Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);
            CurSheet.get_Range(strStartCell, strEndCell).Borders[Excel.XlBordersIndex.xlInsideHorizontal].LineStyle = Excel.XlLineStyle.xlContinuous;
            CurSheet.get_Range(strStartCell, strEndCell).Borders[Excel.XlBordersIndex.xlInsideHorizontal].Weight = xlInsideWeight;

            CurSheet.get_Range(strStartCell, strEndCell).Borders[Excel.XlBordersIndex.xlInsideVertical].Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);
            CurSheet.get_Range(strStartCell, strEndCell).Borders[Excel.XlBordersIndex.xlInsideVertical].LineStyle = Excel.XlLineStyle.xlContinuous;
            CurSheet.get_Range(strStartCell, strEndCell).Borders[Excel.XlBordersIndex.xlInsideVertical].Weight = xlInsideWeight;
        }

        /// <summary>
        /// 设置单元格或连续区域水平居左
        /// </summary>
        /// <param name="strCell">单元格标识符</param>
        public void SetHAlignLeft(string strCell)
        {
            SetHAlignLeft(strCell, strCell);
        }

        /// <summary>
        /// 设置单元格或连续区域水平居左
        /// </summary>
        /// <param name="strCell">单元格标识符</param>
        public void SetHAlignLeft(string strStartCell, string strEndCell)
        {
            CurSheet.get_Range(strStartCell, strEndCell).HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
        }

        /// <summary>
        /// 设置单元格或连续区域水平居左
        /// </summary>
        /// <param name="strCell">单元格标识符</param>
        public void SetHAlignRight(string strCell)
        {
            SetHAlignRight(strCell, strCell);
        }

        /// <summary>
        /// 设置单元格或连续区域水平居左
        /// </summary>
        /// <param name="strCell">单元格标识符</param>
        public void SetHAlignRight(string strStartCell, string strEndCell)
        {
            CurSheet.get_Range(strStartCell, strEndCell).HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
        }

        /// <summary>
        /// 设置单元格或连续区域的显示格式
        /// </summary>
        /// <param name="strCell">单元格标识符</param>
        /// <param name="strNF">如"#,##0.00"的显示格式</param>
        public void SetNumberFormat(string strCell, string strNF)
        {
            CurSheet.get_Range(strCell, mValue).NumberFormat = strNF;
        }

        /// <summary>
        /// 设置单元格或连续区域的字体大小
        /// </summary>
        /// <param name="strCell">单元格或连续区域标识符</param>
        /// <param name="intFontSize"></param>
        public void SetFontSize(string strCell, int intFontSize)
        {
            CurSheet.get_Range(strCell, mValue).Font.Size = intFontSize.ToString();
        }
        public void SetFontSize(string strStartCol, string strEndCol, int intFontSize)
        {
            CurSheet.get_Range(strStartCol, strEndCol).Font.Size = intFontSize.ToString();
        }
        public void SetFontColor(string strStartCol, string strEndCol, System.Drawing.Color color)
        {
            CurSheet.get_Range(strStartCol, strEndCol).Font.Color = System.Drawing.ColorTranslator.ToOle(color);
        }

        /// <summary>
        /// 设置列宽
        /// </summary>
        /// <param name="strColID">列标识，如A代表第一列</param>
        /// <param name="decWidth">宽度</param>
        public void SetColumnWidth(string strStartColID, string strEndColID, double dblWidth)
        {
            ((Excel.Range)CurSheet.Columns.GetType().InvokeMember("Item", System.Reflection.BindingFlags.GetProperty, null, CurSheet.Columns, new object[] { (strStartColID + ":" + strEndColID).ToString() })).ColumnWidth = dblWidth;
        }
        public void SetRowHeight(string strStartRowID, string strEndRowID, double dblWidth)
        {
            ((Excel.Range)CurSheet.Rows.GetType().InvokeMember("Item", System.Reflection.BindingFlags.GetProperty, null, CurSheet.Rows, new object[] { (strStartRowID + ":" + strEndRowID).ToString() })).RowHeight = dblWidth;
        }

        /// <summary>
        /// 设置单元格或连续区域水平居左
        /// </summary>
        /// <param name="strCell">单元格标识符</param>
        public void SetHAlignCenter(string strCell)
        {
            CurSheet.get_Range(strCell, mValue).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
        }

        /// <summary>
        /// 设置连续区域水平居中
        /// </summary>
        /// <param name="strStartCell">开始单元格标识符</param>
        /// <param name="strEndCell">结束单元格标识符</param>
        public void SetHAlignCenter(string strStartCell, string strEndCell)
        {
            CurSheet.get_Range(strStartCell, strEndCell).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
        }
        public string NtoL(int intNumber)
        {
            if (intNumber > 702)
                return String.Empty;

            if (intNumber == 702)
                return "ZZ";

            string strRtn = String.Empty;

            string strLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            if (intNumber > 26)
                strRtn = strLetters.Substring(intNumber / 26 - 1, 1);

            int mv = intNumber % 26;
            if (mv == 0)
                strRtn += "Z";
            else
                strRtn += strLetters.Substring(mv - 1, 1);

            return strRtn;
        }

        /// <summary>
        /// 在指定Range中插入指定的值
        /// </summary>
        /// <param name="strStartCell">Range的开始单元格</param>
        /// <param name="strEndCell">Range的结束单元格</param>
        /// <param name="objValue">文本、数字等值</param>
        public void WriteRange(string strStartCell, string strEndCell, object objValue)
        {
            CurSheet.get_Range(strStartCell, strEndCell).Value2 = objValue;
        }

        /// <summary>
        /// 在指定Range中插入指定的值
        /// </summary>
        /// <param name="strStartCell">Range的开始单元格</param>
        /// <param name="strEndCell">Range的结束单元格</param>
        /// <param name="objValue">文本、数字等值</param>
        public void WriteRange(string strCell, object objValue)
        {
            CurSheet.get_Range(strCell, mValue).Value2 = objValue;
        }

        public object GetRangeValue(string strStartCell, string strEndCell)
        {
            return CurSheet.get_Range(strStartCell, strEndCell).Value2;
        }
        public object GetRangeValue(string strStartCell)
        {
            return CurSheet.get_Range(strStartCell, strStartCell).Value2;
        }

        /// <summary>
        /// 释放内存空间
        /// </summary>
        public void Dispose()
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(CurSheet);
                CurSheet = null;

                CurBook.Close(false, mValue, mValue);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(CurBook);
                CurBook = null;

                CurExcel.Quit();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(CurExcel);
                CurExcel = null;

                GC.Collect();
                GC.WaitForPendingFinalizers();

            }
            catch (System.Exception)
            {
                //    MessageBox.Show("在释放Excel内存空间时发生了一个错误："+ex.Message);
            }
            finally
            {
                //foreach (System.Diagnostics.Process pro in System.Diagnostics.Process.GetProcessesByName("Excel"))
                //    if (pro.StartTime > this.dtBefore && pro.StartTime < this.dtAfter)
                //    {
                //        try
                //        {
                //            pro.Kill();
                //        }
                //        catch { }
                //    }
            }
            System.GC.SuppressFinalize(this);
        }
    }
}


