using System;
using System.Collections.Generic;
using System.Text;
using Infolight.EasilyReportTools.Tools;
using System.Windows.Forms;
using System.Data;
using Srvtools;
using System.Collections;
using System.Drawing;
using System.IO;
using Infolight.EasilyReportTools.UI;
using System.Text.RegularExpressions;

namespace Infolight.EasilyReportTools
{
    public class ExcelReportExporter : IReportExport
    {
        public ExcelReportExporter(IReport rpt, ExportMode eMode, bool dMode)
        {
            report = rpt;
            exportMode = eMode;
            designMode = dMode;
        }

        private bool designMode;
        /// <summary>
        /// 是否设计模式
        /// </summary>
        public bool DesignMode
        {
            get { return designMode; }
        }

        private ExportMode exportMode;
        /// <summary>
        /// 输出模式
        /// </summary>
        public ExportMode ExportMode
        {
            get { return exportMode; }
        }

        #region IReportExport Members

        private IReport report;

        public IReport Report
        {
            get { return report; }
        }

        private string tagInfo;
        /// <summary>
        /// The infomation durning the output process
        /// </summary>
        public string TagInfo
        {
            get { return tagInfo; }
            set { tagInfo = value; }
        }

        private int progressInfo;
        /// <summary>
        /// The finished process number the output process
        /// </summary>
        public int ProgressInfo
        {
            get { return progressInfo; }
            set { progressInfo = value; }
        }

        private int progressCount;
        /// <summary>
        /// The total process count durning the output process
        /// </summary>
        public int ProgressCount
        {
            get { return progressCount; }
            set { progressCount = value; }
        }

        private string fileName;
        /// <summary>
        /// The full file path of export file
        /// </summary>
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        public const string PAGE_INDEX = "#PageIndex#";
        const string PAGE_COUNT = "#PageCount#";
        public const string MERGE_CELL = "#MergeCells#";

        /// <summary>
        /// 设计时预览
        /// </summary>
        public void View()
        {
            try
            {
                if (this.report.HeaderDataSource != null)
                {
                    #region Master Data Source
                    if (DesignMode)
                    {
                        DataSet dsMaster = DataSourceExchange.GetDataSet(this.report.HeaderDataSource, DesignMode).Clone();
                        dsMaster = DataSourceExchange.GetPreviewDataSet(dsMaster);
                        InfoDataSet idsMaster = new InfoDataSet();
                        idsMaster.RealDataSet = dsMaster;
                        InfoBindingSource ibsMaster = new InfoBindingSource();
                        ibsMaster.DataSource = idsMaster;
                        ibsMaster.DataMember = dsMaster.Tables[0].TableName;
                        this.report.HeaderDataSource = ibsMaster;
                    }
                    #endregion
                }

                if (DesignMode)
                {
                    for (int i = 0; i < Report.FieldItems.Count; i++)
                    {
                        DataSourceItem fieldItem = Report.FieldItems[i];
                        DataSet ds = DataSourceExchange.GetDataSet(fieldItem.DataSource, DesignMode).Clone();
                        ds = DataSourceExchange.GetPreviewDataSet(ds);
                        InfoDataSet ids = new InfoDataSet();
                        ids.RealDataSet = ds;
                        InfoBindingSource ibs = new InfoBindingSource();
                        ibs.DataSource = ids;
                        ibs.DataMember = ds.Tables[0].TableName;
                        fieldItem.DataSource = ibs;
                    }
                }


                ExportExcel(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 运行时输出
        /// </summary>
        public void Export()
        {
            ExportExcel(true);
        }

        ExcelController excel;

        private void ExportExcel(bool save)
        {
            //excel = new ExcelController(true);//调试使用
            excel = new ExcelController(this.ExportMode == ExportMode.Preview, report);
            EasilyReportLog log = new EasilyReportLog("Excel Report", this.GetType().FullName, LogFileInfo.logFileName, report);
            try
            {
                if (save)
                {
                    log.StartLog();
                }
                CreateItem(Report.HeaderItems, Report.HeaderFont);
                excel.AddSheet();
                double footerHeight = CreateItem(Report.FooterItems, Report.FooterFont);
                excel.AddSheet();
                CreateCaption();
                excel.AddSheet();
                if (Report.Format.Orientation == Orientation.Horizontal)
                {
                    excel.SetSheetLandscape();
                }
                ExportField(footerHeight);
                DisposeItem();
                if (save)
                {
                    excel.SaveExcel(FileName);
                }
            }
            catch (Exception e)
            {
                if (save)
                {
                    log.WriteExceptionInfo(e);
                }
                throw;
            }
            finally
            {
                if (save)
                {
                    log.EndLog();
                    excel.Dispose();
                }
            }
        }

        /// <summary>
        /// 创建Item的Sheet
        /// </summary>
        /// <param name="collection">Item的Collection</param>
        /// <param name="font">字体</param>
        private double CreateItem(ReportItemCollection collection, Font font)
        {
            if (collection.Count > 0)
            {
                PointInfo point = new PointInfo();
                int width = PageWidth;//取得页的总栏位数
                for (int i = 0; i < collection.Count; i++)
                {
                    ReportItem item = collection[i];
                    if (item.NewLine && i > 0)//换行
                    {
                        point.Enter();
                    }
                    object value = item.Value;
                    if (item.Position == ReportItem.PositionAlign.Right && item.Cells != 0)//向右对齐
                    {
                        point.ColumnIndex = width - item.Cells;
                    }
                    if (item is ReportConstantItem)
                    {
                        if ((item as ReportConstantItem).Style == ReportConstantItem.StyleType.PageIndex)
                        {
                            //页码特别处理
                            value = PAGE_INDEX;
                        }
                        else if ((item as ReportConstantItem).Style == ReportConstantItem.StyleType.PageIndexAndTotalPageCount)
                        {
                            //页码特别处理
                            value = PAGE_INDEX + "/" + PAGE_COUNT;
                        }
                    }
                    int columnIndex = 0;

                    if (item.Cells != 0)
                    {
                        columnIndex = point.ColumnIndex + item.Cells - 1;
                    }
                    else
                    {
                        columnIndex = PageWidth - 1;
                    }

                    PointInfo pointEnd = new PointInfo(point.RowIndex, columnIndex);
                    if (value is Image)//插入图片
                    {
                        excel.InsertPicutre(point, pointEnd, value as Image);
                    }
                    else//插入数据
                    {
                        if (!string.IsNullOrEmpty(item.Format))
                        {
                            value = string.Format(item.Format, value);
                        }
                        else
                        {
                            if (item is ReportDataSourceItem)
                            {
                                DDProvider ddProvider = new DDProvider(Report.HeaderDataSource, DesignMode);
                                string ddValue = ddProvider.GetDDValue(((ReportDataSourceItem)item).ColumnName, DDInfo.FieldCaption).ToString();
                                if (ddValue == "")
                                {
                                    ddValue = ((ReportDataSourceItem)item).ColumnName;
                                }

                                value = string.Format("{0}:{1}", ddValue, value);
                            }
                        }
                        excel.WriteValue(point, pointEnd, value);
                        excel.SetHorizontalAlignment(point, pointEnd, item.ContentAlignment);
                        Font ft = item.Font == null ? font : item.Font;
                        if (font != null)
                        {
                            excel.SetFont(point, pointEnd, ft);
                        }
                    }

                    point.ColumnIndex += item.Cells;
                }
                return (double)excel.CurSheet.UsedRange.Height;
            }
            else
            {
                return 0;
            }
        }

        private Hashtable tableCaption = new Hashtable();
        /// <summary>
        /// 创建标题的Sheet
        /// </summary>
        private void CreateCaption()
        {
            PointInfo point = new PointInfo();
            for (int i = 0; i < report.FieldItems.Count; i++)
            {
                DataSourceItem fieldItem = report.FieldItems[i];
                if (fieldItem.CaptionStyle == DataSourceItem.CaptionStyleType.ColumnHeader && fieldItem.Fields.Count > 0)
                {
                    PointInfo pointStart = new PointInfo(point.RowIndex, point.ColumnIndex);
                    for (int j = 0; j < fieldItem.Fields.Count; j++)
                    {
                        FieldItem field = fieldItem.Fields[j];
                        if (field.NewLine && i > 0)//换行
                        {
                            point.Enter();
                            point.ColumnIndex += field.NewLinePostion - 1;
                        }

                        PointInfo pointend = new PointInfo(point.RowIndex, point.ColumnIndex);
                        if (field.Cells > 1)
                        {
                            pointend.ColumnIndex += field.Cells - 1;
                        }
                        excel.SetHorizontalAlignment(point, pointend, field.CaptionAlignment);
                        excel.SetFont(point, pointend, Report.FieldFont);
                        excel.WriteValue(point, pointend, field.Caption);
                        point.ColumnIndex += field.Cells;
                    }
                    int width = GetPageWidth(i);

                    PointInfo pointEnd = new PointInfo(point.RowIndex, pointStart.ColumnIndex + width - 1);
                    if (Report.Format.RowGridLine)
                    {
                        excel.SetRowGridLine(pointStart, pointEnd, false);
                    }
                    if (Report.Format.ColumnGridLine)
                    {
                        excel.SetColumnGridLine(pointStart, pointEnd, Report.Format.ColumnInsideGridLine);
                    }
                    tableCaption.Add(i, new PointInfo[] { pointStart, pointEnd });//记录Caption的位置
                    point.Enter();//印完换行印下一个
                }
            }
        }

        /// <summary>
        /// 复制Header
        /// </summary>
        /// <param name="point">当前的坐标</param>
        /// <param name="currentHeight">当前的高度</param>
        /// <param name="currentIndex">当前的页码</param>
        private void CopyHeader(PointInfo point, ref double currentHeight, int currentIndex)
        {
            if (Report.HeaderItems.Count > 0)
            {
                CopyItem(1, point, ref currentHeight, currentIndex);
            }
        }

        /// <summary>
        /// 复制Footer
        /// </summary>
        /// <param name="point">当前的坐标</param>
        /// <param name="currentHeight">当前的高度</param>
        /// <param name="currentIndex">当前的页码</param>
        private void CopyFooter(PointInfo point, ref double currentHeight, int currentIndex)
        {
            if (Report.FooterItems.Count > 0)
            {
                CopyItem(2, point, ref currentHeight, currentIndex);
            }
        }

        /// <summary>
        /// 复制标题
        /// </summary>
        /// <param name="point">当前的坐标</param>
        /// <param name="currentHeight">当前的高度</param>
        /// <param name="index">标题的索引</param>
        private void CopyCaption(PointInfo point, ref double currentHeight, int index)
        {
            DataSourceItem fieldItem = Report.FieldItems[index];
            if (fieldItem.Fields.Count > 0 && fieldItem.CaptionStyle == DataSourceItem.CaptionStyleType.ColumnHeader)
            {
                //CopyItem(3, point, ref currentHeight, 0);
                if (tableCaption.Contains(index))
                {
                    PointInfo[] points = (PointInfo[])tableCaption[index];
                    double height = excel.CopyRange(3, points[0], points[1], point);
                    currentHeight += height;
                }
            }
        }

        /// <summary>
        /// 复制Sheet
        /// </summary>
        /// <param name="sheetindex">Sheet的索引</param>
        /// <param name="point">当前的坐标</param>
        /// <param name="currentHeight">当前的高度</param>
        /// <param name="currentIndex">当前的页码</param>
        private void CopyItem(int sheetindex, PointInfo point, ref double currentHeight, int currentIndex)
        {
            double height = excel.CopyRange(sheetindex, point, currentIndex);
            currentHeight += height;
        }

        /// <summary>
        /// 删除三个临时Sheet
        /// </summary>
        private void DisposeItem()
        {
            excel.DeleteSheet(1);//删除Header的Sheet
            excel.DeleteSheet(1);//删除Footer的Sheet
            excel.DeleteSheet(1);//刪除Caption的Sheet
        }

        /// <summary>
        /// 输出Field
        /// </summary>
        /// <param name="detailTable">DataTable</param>
        private void ExportField(double footerHeight)
        {
            double pageHeight = Report.Format.PageHeight > double.Epsilon ? UnitConversion.InchToPound(Report.Format.PageHeight) : 0;//设定的高度
            double currentPageHeight = 0;//当前高度
            int currentPageIndex = 1;//当前页码
            int currentRowCount = 0;//当前笔数
            int excelGroupIndex = -1;//ExcelGroup的列索引
            FieldItem.SumType excelGroupType = FieldItem.SumType.None;
            List<int> subTotalList = new List<int>();

            List<List<string>> allList = new List<List<string>>();//记录还没写入的数据
            PointInfo point = new PointInfo();
            CopyHeader(point, ref currentPageHeight, currentPageIndex);//输出page header

            for (int i = 0; i < Report.FieldItems.Count; i++)
            {
                InitTable(i);
                DataSourceItem fieldItem = Report.FieldItems[i];
                //如果有设高度或者格线,则一笔一笔记录和汇总输出 RowGrid
                if (fieldItem.Fields.Count > 0)
                {
                    List<string> groupColumns = new List<string>();
                    DataTable table = GroupBy(i, groupColumns);//对Table进行排序       
                    if (fieldItem.CaptionStyle == DataSourceItem.CaptionStyleType.ColumnHeader)
                    {
                        CopyCaption(point, ref currentPageHeight, i);
                    }
                    int detailStart = point.RowIndex;//设置Excel Group的起始位置

                    this.ProgressCount = table.Rows.Count;
                    for (int j = 0; j < table.Rows.Count; j++)
                    {
                        bool groupEmpty = groupColumns.Count > 0 && j > 0 && !GroupBreak(table.Rows[j - 1], table.Rows[j], groupColumns);//Group重复的资料不显示
                        //资料
                        object[] dataValues = new object[fieldItem.Fields.Count];
                        int columnIndex = 1;//excel group用
                        for (int k = 0; k < dataValues.Length; k++)
                        {
                            FieldItem field = fieldItem.Fields[k];
                            string caption = fieldItem.CaptionStyle == DataSourceItem.CaptionStyleType.RowHeader ? field.Caption : string.Empty;
                            if (field.SuppressIfDuplicated && groupEmpty && groupColumns.Contains(field.ColumnName))
                            {
                                dataValues[k] = string.Empty;
                            }
                            else
                            {
                                dataValues[k] = GetFormatedValue(field.Format, caption, " ", table.Rows[j][field.ColumnName]);
                            }
                            if (j == 0)//第一次才算GroupTotal
                            {
                                if (!string.IsNullOrEmpty(excelGroup))
                                {
                                    if (string.Compare(excelGroup, field.ColumnName) == 0)
                                    {
                                        excelGroupIndex = columnIndex;
                                    }
                                    if (field.Sum != FieldItem.SumType.None)
                                    {
                                        subTotalList.Add(columnIndex);
                                        excelGroupType = field.Sum;
                                    }
                                }
                                columnIndex += field.Cells;
                            }
                           
                        }
                        List<List<string>> dataList = GetFieldList(dataValues, i);

                        if (NeedRowExport)//如果有设定高度或者行格线就立刻写入
                        {
                            SetValue(point, dataList, pageHeight - footerHeight, ref currentRowCount, ref currentPageHeight, ref currentPageIndex, i);
                        }
                        else//如果没有设定高度或者行格线就放到List
                        {
                            allList.AddRange(dataList);
                        }

                        currentRowCount++;

                        if (groupColumns.Count > 0 && (j == table.Rows.Count - 1 || GroupBreak(table.Rows[j], table.Rows[j + 1], groupColumns)))
                        {
                            //组汇总
                            if (fieldItem.GroupTotal)
                            {
                                object[] groupValues = new object[fieldItem.Fields.Count];
                                bool grouptotal = false;
                                for (int k = 0; k < groupValues.Length; k++)
                                {
                                    FieldItem field = fieldItem.Fields[k];
                                    if (field.Sum != FieldItem.SumType.None)
                                    {
                                        groupValues[k] = GetFormatedValue(field.Format, field.GroupTotalCaption
                                    , GroupTotal(field.ColumnName, field.Sum, table, table.Rows[j], groupColumns));
                                        grouptotal = true;
                                    }
                                    else
                                    {
                                        groupValues[k] = string.Empty;
                                    }
                                }
                                groupFilter = null;//重置GroupFilter
                                if (grouptotal)
                                {
                                    List<List<string>> groupList = GetFieldList(groupValues, i);

                                    if (NeedRowExport)//如果有设定高度或者行格线就立刻写入
                                    {
                                        SetValue(point, groupList, pageHeight - footerHeight, ref currentRowCount, ref currentPageHeight, ref currentPageIndex, i);
                                    }
                                    else//如果没有设定高度或者行格线就放到List
                                    {
                                        allList.AddRange(groupList);
                                    }

                                }
                            }
                            if (fieldItem.GroupGap == DataSourceItem.GroupGapType.EmptyRow)//分组插入空行
                            {
                                if (j != table.Rows.Count - 1)//最后一组不换
                                {
                                    List<List<string>> emptyList = new List<List<string>>();
                                    List<string> emptyString = new List<string>();
                                    emptyString.Add(string.Empty);
                                    emptyList.Add(emptyString);
                                    if (NeedRowExport)//如果有设定高度或者行格线就立刻写入
                                    {
                                        SetValue(point, emptyList, pageHeight - footerHeight, ref currentRowCount, ref currentPageHeight, ref currentPageIndex, i);
                                    }
                                    else//如果没有设定高度或者行格线就放到List
                                    {
                                        allList.AddRange(emptyList);
                                    }
                                }
                            }
                            else if (fieldItem.GroupGap == DataSourceItem.GroupGapType.Page)//分组换页
                            {
                                SetValue(point, allList, pageHeight - footerHeight, ref currentRowCount, ref currentPageHeight, ref currentPageIndex, i);//写入所有数据
                                allList.Clear();
                                if (j != table.Rows.Count - 1)//最后一组不换
                                {
                                    ChangePage(point, ref currentRowCount, ref currentPageHeight, ref currentPageIndex, i);
                                }
                            }
                            else if (fieldItem.GroupGap == DataSourceItem.GroupGapType.Sheet)//分组换Sheet
                            {
                                SetValue(point, allList, pageHeight - footerHeight, ref currentRowCount, ref currentPageHeight, ref currentPageIndex, i);//写入所有数据
                                allList.Clear();
                                //修改总页数
                                excel.Replace(PAGE_COUNT, currentPageIndex);

                                //调整列宽
                                for (int k = 0; k < PageWidth; k++)
                                {
                                    int width = GetDetailWidth(j);
                                    if (width > 0)
                                    {
                                        excel.SetColumnWidth(new PointInfo(0, k), Report.FieldFont, width);
                                    }
                                }

                                if (j != table.Rows.Count - 1)//最后一组不换
                                {
                                    ChangeSheet(point, ref currentRowCount, ref currentPageHeight, ref currentPageIndex, i);
                                }
                            }
                        }

                        if (currentRowCount >= Report.Format.PageRecords)//检查笔数
                        {
                            SetValue(point, allList, pageHeight - footerHeight, ref currentRowCount, ref currentPageHeight, ref currentPageIndex, i);
                            allList.Clear();
                            ChangePage(point, ref currentRowCount, ref currentPageHeight, ref currentPageIndex, i);
                        }
                        this.ProgressInfo = j + 1;
                    }
                    //汇总
                    if (string.IsNullOrEmpty(excelGroup))
                    {
                        bool total = false;
                        object[] totalValues = new object[fieldItem.Fields.Count];
                        for (int j = 0; j < totalValues.Length; j++)
                        {
                            FieldItem field = fieldItem.Fields[j];
                            if (field.Sum != FieldItem.SumType.None)
                            {
                                totalValues[j] = GetFormatedValue(field.Format, field.TotalCaption, GroupTotal(field.ColumnName, field.Sum, table, null, groupColumns));
                                total = true;
                            }
                            else
                            {
                                totalValues[j] = string.Empty;
                            }
                        }
                        if (total)
                        {
                            List<List<string>> totalList = GetFieldList(totalValues, i);

                            if (NeedRowExport)//如果有设定高度或者行格线就立刻写入
                            {
                                SetValue(point, totalList, pageHeight - footerHeight, ref currentRowCount, ref currentPageHeight, ref currentPageIndex, i);
                            }
                            else//如果没有设定高度或者行格线就放到List
                            {
                                allList.AddRange(totalList);
                            }
                        }
                    }
                    SetValue(point, allList, pageHeight - footerHeight, ref currentRowCount, ref currentPageHeight, ref currentPageIndex, i);//写入所有数据
                    allList.Clear();
                    if (!string.IsNullOrEmpty(excelGroup))//Excel Group
                    {
                        if (point.RowIndex - 1 > detailStart)
                        {
                            excel.Group(new PointInfo(detailStart, 0), new PointInfo(point.RowIndex - 1, PageWidth - 1), excelGroupType, excelGroupIndex, subTotalList.ToArray());
                        }
                    }
                    //修改总页数
                    excel.Replace(PAGE_COUNT, currentPageIndex);
                    //调整列宽
                    for (int j = 0; j < PageWidth; j++)
                    {
                        int width = GetDetailWidth(j);
                        if (width > 0)
                        {
                            excel.SetColumnWidth(new PointInfo(0, j), Report.FieldFont, width);
                        }
                    }
                }
            }
            CopyFooter(point, ref currentPageHeight, currentPageIndex);
        }

        /// <summary>
        /// 格式化数据
        /// </summary>
        /// <param name="format">格式</param>
        /// <param name="caption">前缀</param>
        /// <param name="value">数据</param>
        /// <returns>格式化的数据</returns>
        private string GetFormatedValue(string format, string caption, object value)
        {
            return GetFormatedValue(format, caption, ":", value);
        }

        /// <summary>
        /// 格式化数据
        /// </summary>
        /// <param name="format">格式</param>
        /// <param name="caption">前缀</param>
        /// <param name="value">数据</param>
        /// <returns>格式化的数据</returns>
        private string GetFormatedValue(string format, string caption, string spliter, object value)
        {
            StringBuilder formatString = new StringBuilder();
            if (!string.IsNullOrEmpty(caption))
            {
                formatString.Append(caption);
                formatString.Append(spliter);
            }
            formatString.Append("{0");
            if (!string.IsNullOrEmpty(format))
            {
                formatString.Append(":");
                formatString.Append(format);
            }
            formatString.Append("}");
            if (value != null && value is string)
            {
                string str = (string)value;
                if (str.Length > 0 && Char.IsDigit(str, 0))
                {
                    value = string.Format("'{0}", str);
                }
            }

            return string.Format(formatString.ToString(), value);
        }

        /// <summary>
        /// 数据加入数组
        /// </summary>
        /// <param name="values">数据</param>
        /// <param name="index">FieldItem的索引</param>
        /// <returns>数组</returns>
        private List<List<string>> GetFieldList(object[] values, int datasourceIndex)
        {
            List<List<string>> list = new List<List<string>>();
            List<string> row = new List<string>();
            DataSourceItem fieldItem = Report.FieldItems[datasourceIndex];
            for (int i = 0; i < fieldItem.Fields.Count; i++)
            {
                FieldItem field = fieldItem.Fields[i];
                if (i > 0 && field.NewLine)
                {
                    multiRow = true;
                    list.Add(row);
                    row = new List<string>();
                    for (int j = 0; j < field.NewLinePostion - 1; j++)
                    {
                        row.Add(string.Empty);
                    }
                }
                row.Add(values[i].ToString());
                if (field.Cells > 1)
                {
                    for (int j = 0; j < field.Cells - 1; j++)
                    {
                        row.Add(MERGE_CELL);
                    }
                }
            }
            list.Add(row);
            return list;
        }

        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="point">坐标</param>
        /// <param name="values">数据</param>
        /// <param name="currentCount">当前的笔数</param>
        /// <param name="pageHeight">设定的高度</param>
        /// <param name="currentHeight">当前的高度</param>
        /// <param name="currentIndex">当前的页码</param>
        private void SetValue(PointInfo point, List<List<string>> values, double pageHeight, ref int currentCount, ref double currentHeight, ref int currentPageIndex, int dataSourceIndex)
        {
            int rowCount = values.Count;
            if (rowCount > 0)
            {
                int columnCount = 0;
                for (int i = 0; i < values.Count; i++)
                {
                    columnCount = Math.Max(columnCount, values[i].Count);
                }

                PointInfo pointEnd = new PointInfo(point.RowIndex + rowCount - 1, point.ColumnIndex + columnCount - 1);//计算EndPoint
                double rangeHeight = excel.WriteFieldValue(point, pointEnd, values, Report.FieldFont);
                if (pageHeight > double.Epsilon && rangeHeight + currentHeight > pageHeight)//超出高度
                {
                    excel.DeleteRow(point, pointEnd);
                    ChangePage(point, ref currentCount, ref currentHeight, ref currentPageIndex, dataSourceIndex);
                    pointEnd = new PointInfo(point.RowIndex + rowCount - 1, point.ColumnIndex + columnCount - 1);
                    rangeHeight = excel.WriteFieldValue(point, pointEnd, values, Report.FieldFont);
                    currentHeight += rangeHeight;
                }
                else
                {
                    currentHeight += rangeHeight;
                }

                //空行不设置格式
                if (rowCount == 1 && columnCount == 1 && values[0][0].Length == 0) { }
                else
                {
                    if (Report.Format.RowGridLine)
                    {
                        excel.SetRowGridLine(point, pointEnd, !multiRow);
                    }

                    if (Report.Format.ColumnGridLine)
                    {
                        excel.SetColumnGridLine(point, pointEnd, Report.Format.ColumnInsideGridLine);
                    }

                    excel.SetFont(point, pointEnd, Report.FieldFont);
                    excel.SetHorizontalAlignment(point, pointEnd, HorizontalAlignment.Left);
                    for (int i = point.ColumnIndex; i <= pointEnd.ColumnIndex; i++)
                    {
                        HorizontalAlignment align = GetDetailAlignment(i);
                        int cells = GetDetailCells(i);
                        if (align != HorizontalAlignment.Left)
                        {
                            excel.SetHorizontalAlignment(new PointInfo(point.RowIndex, i), new PointInfo(pointEnd.RowIndex, i + cells - 1), align);
                        }
                    }
                }

                point.RowIndex = pointEnd.RowIndex;
                point.Enter();
            }
        }

        private Hashtable tableAlignment = new Hashtable();
        private HorizontalAlignment GetDetailAlignment(int index)
        {
            if (tableAlignment.Contains(index))
            {
                return (HorizontalAlignment)tableAlignment[index];
            }
            else
            {
                return HorizontalAlignment.Left;
            }
        }

        private Hashtable tableCells = new Hashtable();
        private int GetDetailCells(int index)
        {
            if (tableCells.Contains(index))
            {
                return (int)tableCells[index];
            }
            else
            {
                return 1;
            }
        }

        private Hashtable tableWidth = new Hashtable();
        private int GetDetailWidth(int index)
        {
            if (tableWidth.Contains(index))
            {
                return (int)tableWidth[index];
            }
            else
            {
                return 0;
            }
        }

        private void InitTable(int dataSourceIndex)
        {
            tableAlignment = new Hashtable();
            tableWidth = new Hashtable();
            tableCells = new Hashtable();
            if(dataSourceIndex >= 0 && dataSourceIndex < Report.FieldItems.Count)
            {
                int index = 0;
                DataSourceItem fieldItem = Report.FieldItems[dataSourceIndex];
                for (int i = 0; i < fieldItem.Fields.Count; i++)
                {
                    FieldItem field = fieldItem.Fields[i];
                    if (field.NewLine && i > 0)
                    {
                        index = field.NewLinePostion - 1;
                    }
                    if (field.Cells == 1)
                    {
                        tableAlignment[index] = field.ColumnAlignment;
                        if (tableWidth.Contains(index))
                        {
                            tableWidth[index] = Math.Max((int)tableWidth[index], field.Width);
                        }
                        else
                        {
                            tableWidth[index] = field.Width;
                        }
                    }
                    else
                    {
                        tableAlignment[index] = field.ColumnAlignment;
                        tableCells[index] = field.Cells;
                    }


                    index += field.Cells;
                }
            }
        }

        /// <summary>
        /// 换页
        /// </summary>
        /// <param name="currentCount">当前的笔数</param>
        /// <param name="point">当前的坐标</param>
        /// <param name="currentHeight">当前的高度</param>
        /// <param name="currentIndex">当前的页码</param>
        private void ChangePage(PointInfo point, ref int currentCount, ref double currentHeight, ref int currentPageIndex, int dataSourceIndex)
        {
            if (string.IsNullOrEmpty(excelGroup) && Report.HeaderRepeat)
            {
                CopyFooter(point, ref currentHeight, currentPageIndex);//复制Footer
            }
            excel.PageBreak(point);//设置换页
            currentHeight = 0;//重置高度
            currentCount = 0;//重置笔数
            currentPageIndex++;//增加页码
            if (string.IsNullOrEmpty(excelGroup))
            {
                if (Report.HeaderRepeat)
                {
                    CopyHeader(point, ref currentHeight, currentPageIndex);//复制Header
                }
                DataSourceItem fieldItem = Report.FieldItems[dataSourceIndex];
                if (fieldItem.CaptionStyle == DataSourceItem.CaptionStyleType.ColumnHeader)
                {
                    CopyCaption(point, ref currentHeight, dataSourceIndex);//复制标题
                }
            }
        }

        /// <summary>
        /// 换Sheet
        /// </summary>
        /// <param name="currentCount">当前的笔数</param>
        /// <param name="point">当前的坐标</param>
        /// <param name="currentHeight">当前的高度</param>
        /// <param name="currentIndex">当前的页码</param>
        private void ChangeSheet(PointInfo point, ref int currentCount, ref double currentHeight, ref int currentPageIndex, int dataSourceIndex)
        {
            CopyFooter(point, ref currentHeight, currentPageIndex);//复制Footer
            excel.AddSheet();
            if (Report.Format.Orientation == Orientation.Horizontal)
            {
                excel.SetSheetLandscape();
            }
            point.Reset();
            currentHeight = 0;//重置高度
            currentCount = 0;//重置笔数
            currentPageIndex = 1;//重置页码
            CopyHeader(point, ref currentHeight, currentPageIndex);//复制Header
            DataSourceItem fieldItem = Report.FieldItems[dataSourceIndex];
            if (fieldItem.CaptionStyle == DataSourceItem.CaptionStyleType.ColumnHeader)
            {
                CopyCaption(point, ref currentHeight, dataSourceIndex);//复制标题
            }
        }

        /// <summary>
        /// 排序分组
        /// </summary>
        /// <param name="table">DataTable</param>
        /// <returns>排序后的DataTable</returns>
        private DataTable GroupBy(int index, List<string> columns)
        {
            DataSourceItem fieldItem = Report.FieldItems[index];
            DataView view = DataSourceExchange.GetDataView(fieldItem.DataSource);
            StringBuilder sort = new StringBuilder();
            foreach (FieldItem field in fieldItem.Fields)
            {
                if (field.Order != FieldItem.OrderType.None)
                {
                    if (sort.Length > 0)
                    {
                        sort.Append(",");
                    }
                    sort.Append(field.ColumnName);
                    sort.Append(" ");
                    sort.Append(field.Order == FieldItem.OrderType.Ascend ? "asc" : "desc");
                    if (field.Group == FieldItem.GroupType.Normal)
                    {
                        columns.Add(field.ColumnName);
                    }
                    else if (field.Group == FieldItem.GroupType.Excel)
                    {
                        excelGroup = field.ColumnName;
                    }
                }
            }
            if (sort.Length > 0)
            {
                view.Sort = sort.ToString();

            }
            return view.ToTable();
        }

        /// <summary>
        /// 是否分组
        /// </summary>
        /// <param name="row1">DataRow1</param>
        /// <param name="row2">DataRow2</param>
        /// <param name="columns">分组的栏位</param>
        /// <returns>是否分组</returns>
        private bool GroupBreak(DataRow row1, DataRow row2, List<string> columns)
        {
            foreach (string column in columns)
            {
                if (!row1[column].Equals(row2[column]))
                {
                    return true;
                }
            }
            return false;
        }

        private string groupFilter;
        private string excelGroup;

        /// <summary>
        /// 分组汇总
        /// </summary>
        /// <param name="groupColumn">汇总的栏位</param>
        /// <param name="sumType">汇总的类型</param>
        /// <param name="table">DataTable</param>
        /// <param name="row">DataRow</param>
        /// <param name="columns">分组的栏位</param>
        /// <returns>汇总的结果</returns>
        private object GroupTotal(string groupColumn, Infolight.EasilyReportTools.FieldItem.SumType sumType, DataTable table, DataRow row, List<string> columns)
        {
            if (string.IsNullOrEmpty(groupFilter))
            {
                StringBuilder filter = new StringBuilder();
                if (row != null)
                {
                    foreach (string col in columns)
                    {
                        if (table.Columns.Contains(col))
                        {
                            DataColumn column = table.Columns[col];
                            if (filter.Length > 0)
                            {
                                filter.Append(" And ");
                            }
                            filter.Append(col);
                            filter.Append("=");
                            if (column.DataType == typeof(string) || column.DataType == typeof(Guid))
                            {
                                filter.Append(string.Format("'{0}'", row[col]));
                            }
                            else if (column.DataType == typeof(DateTime))
                            {
                                filter.Append(string.Format("'{0:yyyy/MM/dd HH:mm:ss.fff}'", row[col]));
                            }
                            else
                            {
                                filter.Append(string.Format("{0}", row[col]));
                            }
                        }
                        else
                        {
                            throw new Exception(string.Format("column:{0} not in table", col));
                        }
                    }
                }
                groupFilter = filter.ToString();
            }
            return table.Compute(string.Format("{0}({1})", sumType, groupColumn), groupFilter);
        }

        private int GetPageWidth(int index)
        {
            if (index >= 0 && index < Report.FieldItems.Count)
            {
                int maxWidth = 0;
                int width = 0;
                DataSourceItem fieldItem = Report.FieldItems[index];
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

        private int pageWidth;
        /// <summary>
        /// 取得页的栏位数
        /// </summary>
        /// <returns></returns>
        private int PageWidth
        {
            get
            {
                if (pageWidth == 0)
                {
                    for (int i = 0; i < Report.FieldItems.Count; i++)
                    {
                        pageWidth = Math.Max(pageWidth, GetPageWidth(i));
                    }
                    if (pageWidth == 0)
                    {
                        pageWidth = 2;
                    }
                }
                return pageWidth;
            }
        }

        private bool multiRow;

        private bool NeedRowExport
        {
            get { return Report.Format.PageHeight > double.Epsilon || (Report.Format.RowGridLine && multiRow); }
        }

        public ExecutionResult CheckValidate()
        {
            #region Variable Definition
            ExecutionResult execResult = new ExecutionResult();
            bool newLine = false;
            int excelGroupCount = 0;
            int normalGroupCount = 0;
            #endregion

            execResult.Status = true;

            foreach (DataSourceItem fieldItem in Report.FieldItems)
            {
                if (fieldItem.DataSource == null)
                {
                    execResult.Status = false;
                    execResult.Message = MessageInfo.DataSourceNull;
                    return execResult;
                }
                else if (fieldItem.Fields.Count == 0)
                {
                    execResult.Status = false;
                    execResult.Message = MessageInfo.FieldItemsNull;
                    return execResult;
                }
                foreach (FieldItem field in fieldItem.Fields)
                {
                    switch (field.Group)
                    {
                        case FieldItem.GroupType.Normal: normalGroupCount++; break;
                        case FieldItem.GroupType.Excel: excelGroupCount++; break;
                    }
                    if (field.NewLine)
                    {
                        newLine = true;
                    }
                }
                if (excelGroupCount > 0 && normalGroupCount > 0)
                {
                    execResult.Status = false;
                    execResult.Message = MessageInfo.MultiGroupModeError;
                    return execResult;
                }

                else if (excelGroupCount > 1)
                {
                    execResult.Status = false;
                    execResult.Message = MessageInfo.MultiExcelGroupColumnError;
                    return execResult;
                }
                else if (excelGroupCount == 1 && (newLine || Report.FieldItems.Count > 1))
                {
                    execResult.Status = false;
                    execResult.Message = MessageInfo.ExcelGroupNewLineError;
                    return execResult;
                }
                else if (fieldItem.GroupTotal && excelGroupCount == 0 && normalGroupCount == 0)
                {
                    execResult.Status = false;
                    execResult.Message = MessageInfo.GroupColumnsNotSetError;
                    return execResult;
                }
            }

            return execResult;
        }

        #endregion
    }

    public enum ReportArea
    {
        ReportHeader,
        Detail,
        ReportFooter
    }

    public enum ExportMode
    { 
        Export,
        Preview
    }
}
