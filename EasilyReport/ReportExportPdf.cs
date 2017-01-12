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
using Infolight.EasilyReportTools.Config;
using System.Web.UI;

namespace Infolight.EasilyReportTools
{
    internal class PdfReportExporter:IReportExport
    {
        public PdfReportExporter(IReport rpt, ExportMode eMode, bool dMode)
        {
            report = rpt;
            exportMode = eMode;
            designMode = dMode;
            log = new EasilyReportLog("Pdf Report", this.GetType().FullName, LogFileInfo.logFileName, rpt);

            if (rpt.Format.PageHeight != 0.0)
            {
                exportByHeight = true;
            }
            else
            {
                exportByHeight = false;
            }
        }

        EasilyReportLog log;
        PdfController pdf;

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

        private bool exportByHeight;
        /// <summary>
        /// 是否计算高度
        /// </summary>
        public bool ExportByHeight
        {
            get { return exportByHeight; }
            set { exportByHeight = value; }
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

        #region Page Members
        private List<List<object>> headerList;
        /// <summary>
        /// Report Header List
        /// </summary>
        public List<List<object>> HeaderList
        {
            get { return headerList; }
            set { headerList = value; }
        }

        private List<List<object>> footerList;
        /// <summary>
        /// Report Header List
        /// </summary>
        public List<List<object>> FooterList
        {
            get { return footerList; }
            set { footerList = value; }
        }

        private List<List<List<PdfDesc>>> allFieldList = new List<List<List<PdfDesc>>>();
        /// <summary>
        /// All Field List
        /// </summary>
        internal List<List<List<PdfDesc>>> AllFieldList
        {
            get { return allFieldList; }
            set { allFieldList = value; }
        }

        private List<int> processInfoCollection = new List<int>();
        /// <summary>
        /// 记录进度条的Info信息,区分每一个Field
        /// </summary>
        public List<int> ProcessInfoCollection
        {
            get { return processInfoCollection; }
            set { processInfoCollection = value; }
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
        #endregion

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
                    DataSet dsMaster = DataSourceExchange.GetDataSet(this.report.HeaderDataSource, DesignMode).Clone();
                    dsMaster = DataSourceExchange.GetPreviewDataSet(dsMaster);
                    InfoDataSet idsMaster = new InfoDataSet();
                    idsMaster.RealDataSet = dsMaster;
                    InfoBindingSource ibsMaster = new InfoBindingSource();
                    ibsMaster.DataSource = idsMaster;
                    ibsMaster.DataMember = dsMaster.Tables[0].TableName;
                    this.report.HeaderDataSource = ibsMaster;
                    #endregion
                }

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

                ExportPdf();

                System.Diagnostics.Process.Start(ExportFileInfo.PdfPreviewFilePath);
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
            ExportPdf();
        }

        #region Export Logic
        /// <summary>
        /// Export Pdf
        /// </summary>
        private void ExportPdf()
        {
            #region Variable Definition
            ExecutionResult execResult = new ExecutionResult();
            SizeInfo pdfSize = new SizeInfo(this.Report, this.Report.Format.PageSize);
            int maxGroupCount = 0;
            List<List<int>> allStartIndex = new List<List<int>>();
            #endregion
            try
            {
                this.progressInfo = 0;
                this.progressCount = 0;
                
                pdf = new PdfController(Report, exportMode, ExportByHeight, DesignMode, PageWidth);

                #region 計算總頁數
                for (int i = 0; i < Report.FieldItems.Count; i++)
                {
                    DataSourceItem fieldItem = Report.FieldItems[i];
                    List<int> startIndex = new List<int>();

                    startIndex.Clear();

                    if (fieldItem.Fields.Count > 0)
                    {
                        List<string> groupColumns = new List<string>();
                        DataTable table = GroupBy(i, groupColumns);//对Table进行排序

                        if (table.Rows.Count > 0)
                        {
                            startIndex.Add(0);
                        }

                        if (fieldItem.GroupGap == DataSourceItem.GroupGapType.Page)
                        {
                            for (int j = 0; j < table.Rows.Count; j++)
                            {
                                if (j > 0 && GroupBreak(table.Rows[j], table.Rows[j - 1], groupColumns))
                                {
                                    startIndex.Add(j);
                                }
                            }
                        }
                        else
                        {
                            int maxCount = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(table.Rows.Count) / Convert.ToDouble(Report.Format.PageRecords)));
                            for (int j = 0; j < maxCount - 1; j++)
                            {
                                startIndex.Add((j + 1) * Report.Format.PageRecords);
                            }
                        }

                        if (startIndex.Count > maxGroupCount)
                        {
                            maxGroupCount = startIndex.Count;
                        }
                    }

                    allStartIndex.Add(startIndex);

                    processInfoCollection.Add(0);
                }

                if (maxGroupCount != 0)
                {
                    pdfSize.TotalPageCount = maxGroupCount;
                }
                #endregion

                if (exportMode == ExportMode.Export)
                {
                    Report.SetPageCount(pdfSize.TotalPageCount);
                }
                else
                {
                    Report.SetPageCount(1);
                }

                this.TagInfo = ProcessTagInfo.Exporting;

                for (int i = 0; i < pdfSize.TotalPageCount; i++)
                {
                    Report.SetCurrentPageIndex(i + 1);

                    CreateItem(Report.HeaderItems, Report.HeaderFont);

                    if (ExportByHeight)
                    {
                        AllFieldList.Add(ExportField(allStartIndex));
                    }
                    else
                    {
                        ExportField(allStartIndex);
                    }

                    CreateItem(Report.FooterItems, Report.FooterFont);

                    if (i != Report.PageCount - 1 && !ExportByHeight)
                    {
                        pdf.NewPage();
                    }
                }

                if (ExportByHeight)
                {
                    FitsPage();
                }

                if (exportMode == ExportMode.Export)
                {
                    this.TagInfo = ProcessTagInfo.ProcessFinished;
                    this.ProgressInfo = this.ProgressCount;
                }
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            finally
            {
                if (pdf != null)
                {
                    if (Report.PageCount == 0)
                    {
                        pdf.SetEmpty();
                    }
                    pdf.Dispose();
                }
            }
        }

        private void FitsPage()
        {
            int allPageCount = AllFieldList.Count; //匹配前的总页数
            List<List<PdfDesc>> newList = null;


            try
            {
                for (int i = 0; i < allPageCount; i++)
                {
                    int delCount = pdf.FitsTableToPage(AllFieldList[i], Report.FieldFont, multiRow);

                    if (delCount != 0)
                    {
                        if (i != allPageCount - 1)
                        {
                            //int startIndex = AllFieldList[i].Count - delCount;
                            for (int j = 0; j < delCount; j++)
                            {
                                //判断后一页的第一行是不是Caption
                                if (AllFieldList[i + 1][0][0].IsCaption && AllFieldList[i + 1][0][0].FieldNum == AllFieldList[i][AllFieldList[i].Count - 1][0].FieldNum)
                                {
                                    if (AllFieldList[i + 1].Count == multiRow)
                                    {
                                        AllFieldList[i + 1].Add(AllFieldList[i][AllFieldList[i].Count - 1]);
                                    }
                                    else
                                    {
                                        if (AllFieldList[i][AllFieldList[i].Count - 1][0].IsCaption)
                                        {
                                            AllFieldList[i + 1].Insert(0, AllFieldList[i][AllFieldList[i].Count - 1]);
                                        }
                                        else
                                        {
                                            AllFieldList[i + 1].Insert(multiRow, AllFieldList[i][AllFieldList[i].Count - 1]);
                                        }
                                    }
                                }
                                else
                                {
                                    AllFieldList[i + 1].Insert(0, AllFieldList[i][AllFieldList[i].Count - 1]);
                                }

                                AllFieldList[i].RemoveAt(AllFieldList[i].Count - 1);
                            }

                            //判断当前页的最后一行是不是Caption
                            for (int k = 0; k < multiRow; k++)
                            {
                                if (AllFieldList[i][AllFieldList[i].Count - 1][0].IsCaption) //是Caption就移到下一页
                                {
                                    AllFieldList[i + 1].Insert(0, AllFieldList[i][AllFieldList[i].Count - 1]);
                                    AllFieldList[i].RemoveAt(AllFieldList[i].Count - 1);
                                }
                            }
                            
                        }
                        else
                        {
                            #region New Function
                            int startIndex = 0;

                            while (delCount != 0)
                            {
                                newList = new List<List<PdfDesc>>();

                                startIndex = AllFieldList[i].Count - delCount;

                                for (int j = 0; j < delCount; j++)
                                {
                                    newList.Add(AllFieldList[i][startIndex]);
                                    AllFieldList[i].RemoveAt(startIndex);
                                }

                                //判断当前页的最后一行是不是Caption
                                for (int k = 0; k < multiRow; k++)
                                {
                                    if (AllFieldList[i][AllFieldList[i].Count - 1][0].IsCaption) //是Caption就移到下一页
                                    {
                                        if (newList.Count == 0)
                                        {
                                            newList.Add(AllFieldList[i][AllFieldList[i].Count - 1]);
                                        }
                                        else
                                        {
                                            newList.Insert(0, AllFieldList[i][AllFieldList[i].Count - 1]);
                                        }
                                        AllFieldList[i].RemoveAt(AllFieldList[i].Count - 1);
                                    }
                                }

                                AllFieldList.Add(newList);
                                i++;

                                delCount = pdf.FitsTableToPage(AllFieldList[i], Report.FieldFont, multiRow);

                                //判断当前页的最后一行是不是Caption
                                if (delCount == 0)
                                {
                                    for (int k = 0; k < multiRow; k++)
                                    {
                                        if (AllFieldList[i][AllFieldList[i].Count - 1][0].IsCaption) //是Caption就移到下一页
                                        {
                                            AllFieldList[i].RemoveAt(AllFieldList[i].Count - 1);
                                        }
                                    }
                                }
                            }
                            #endregion
                        }
                    }
                }


                Report.SetPageCount(AllFieldList.Count);

                for (int i = 0; i < AllFieldList.Count; i++)
                {
                    Report.SetCurrentPageIndex(i + 1);

                    //FitsPage以后的List,如果没有FieldCaption,就补上,这样一来会稍微影响换页的计算。
                    if (!AllFieldList[i][0][0].IsCaption && Report.FieldItems[AllFieldList[i][0][0].FieldNum].CaptionStyle == DataSourceItem.CaptionStyleType.ColumnHeader)
                    {
                        List<List<PdfDesc>> captionList = CreateFieldCaption(AllFieldList[i][0][0].FieldNum);
                        AllFieldList[i].InsertRange(0, captionList);
                    }



                    pdf.ExportToPdf(CreateItem(Report.HeaderItems, Report.HeaderFont), AllFieldList[i], CreateItem(Report.FooterItems, Report.FooterFont), multiRow);

                    if (i != Report.PageCount - 1)
                    {
                        pdf.NewPage();
                    }
                }
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
        }

        private List<List<object>> CreateItem(ReportItemCollection collection, Font font)
        {
            List<List<object>> lists = new List<List<object>>();
            List<object> list = null;

            try
            {
                for (int i = 0; i < collection.Count; i++)
                {
                    if (i == 0 || collection[i].NewLine)
                    {
                        list = new List<object>();
                    }

                    list.Add(collection[i]);

                    if (!lists.Contains(list))
                    {
                        lists.Add(list);
                    }
                }

                if (collection.Count > 0)
                {
                    pdf.WriteItem(lists, font);
                }
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return lists;
        }

        private List<List<PdfDesc>> ExportField(List<List<int>> allStartIndex)
        {
            List<List<PdfDesc>> allList = new List<List<PdfDesc>>();
            int currentRowCount = 0;

            try
            {
                for (int i = 0; i < Report.FieldItems.Count; i++)
                {
                    DataSourceItem fieldItem = Report.FieldItems[i];
                    List<string> groupColumns = new List<string>();
                    DataTable table = GroupBy(i, groupColumns);//对Table进行排序

                    if (table.Rows.Count > this.ProgressCount)
                    {
                        this.ProgressCount = table.Rows.Count;
                    }

                    currentRowCount = 0;

                    int startIndex = 0;
                    int count = 0;

                    if (allStartIndex[i].Count > Report.CurrentPageIndex - 1)
                    {
                        if (table.Rows.Count > 0 && allStartIndex[i].Count > Report.CurrentPageIndex - 1)
                        {
                            startIndex = allStartIndex[i][Report.CurrentPageIndex - 1];
                        }
                    }

                    if (allStartIndex[i].Count == 1 && Report.CurrentPageIndex == 1)
                    {
                        count = table.Rows.Count;
                    }
                    else if (allStartIndex[i].Count >= Report.CurrentPageIndex)
                    {
                        if (allStartIndex[i].Count == Report.CurrentPageIndex)
                        {
                            count = table.Rows.Count - allStartIndex[i][Report.CurrentPageIndex - 1];
                        }
                        else
                        {
                            count = Report.CurrentPageIndex < Report.PageCount ? allStartIndex[i][Report.CurrentPageIndex] - allStartIndex[i][Report.CurrentPageIndex - 1] : table.Rows.Count - allStartIndex[i][Report.CurrentPageIndex - 1];
                        }
                    }

                    if (count != 0)
                    {
                        if (fieldItem.CaptionStyle == DataSourceItem.CaptionStyleType.ColumnHeader)
                        {
                            List<List<PdfDesc>> captionList = CreateFieldCaption(i);
                            allList.AddRange(captionList);
                        }
                    }

                    if (count != 0)
                    {
                        for (int j = startIndex; j < table.Rows.Count; j++)
                        {
                            bool groupEmpty = groupColumns.Count > 0 && j > startIndex && !GroupBreak(table.Rows[j - 1], table.Rows[j], groupColumns);//Group重复的资料不显示
                            object[] dataValues = new object[fieldItem.Fields.Count];
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
                            }
                            List<List<PdfDesc>> dataList = GetFieldList(dataValues, i, ListContentType.Field);
                            allList.AddRange(dataList);

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
                                        List<List<PdfDesc>> groupList = GetFieldList(groupValues, i, ListContentType.GroupTotal);
                                        allList.AddRange(groupList);
                                    }
                                }

                                switch (fieldItem.GroupGap)
                                {
                                    case DataSourceItem.GroupGapType.None:
                                        break;
                                    case DataSourceItem.GroupGapType.EmptyRow:
                                    case DataSourceItem.GroupGapType.SingleLine:
                                    case DataSourceItem.GroupGapType.DoubleLine:
                                        if (j != table.Rows.Count - 1)//最后一组不换
                                        {
                                            List<List<PdfDesc>> emptyList = new List<List<PdfDesc>>();
                                            List<PdfDesc> emptyString = new List<PdfDesc>();
                                            PdfDesc pdfDesc = new PdfDesc();
                                            pdfDesc.Value = String.Empty;
                                            pdfDesc.LeftLine = false;
                                            pdfDesc.RightLine = false;
                                            pdfDesc.TopLine = Report.Format.RowGridLine;
                                            pdfDesc.BottomLine = false;
                                            pdfDesc.GroupGap = fieldItem.GroupGap;
                                            pdfDesc.FieldNum = i;
                                            emptyString.Add(pdfDesc);
                                            emptyList.Add(emptyString);
                                            allList.AddRange(emptyList);
                                        }
                                        break;
                                    case DataSourceItem.GroupGapType.Page:
                                        break;
                                }
                            }

                            if (currentRowCount >= count)
                            {
                                if (!ExportByHeight)
                                {
                                    pdf.WriteFields(allList, Report.FieldFont, multiRow);
                                    allList.Clear();
                                }
                                break;
                            }

                            processInfoCollection[i] = j + 1;

                            this.ProgressInfo = processInfoCollection[i];
                        }

                        if (Report.CurrentPageIndex == allStartIndex[i].Count)
                        {
                            //汇总
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
                                List<List<PdfDesc>> totalList = GetFieldList(totalValues, i, ListContentType.Total);

                                allList.AddRange(totalList);

                                if (!ExportByHeight)
                                {
                                    pdf.WriteFields(totalList, Report.FieldFont, multiRow);
                                    allList.Clear();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }

            return allList;
        }

        private List<List<PdfDesc>> CreateFieldCaption(int index)
        {
            List<List<PdfDesc>> captionList = new List<List<PdfDesc>>();
            
            try
            {
                DataSourceItem fieldItem = Report.FieldItems[index];
                if (fieldItem.Fields.Count > 0)
                {
                    object[] values = new object[fieldItem.Fields.Count];

                    for (int i = 0; i < values.Length; i++)
                    {
                        FieldItem field = fieldItem.Fields[i];
                        values[i] = field.Caption;
                    }

                    captionList = GetFieldList(values, index, ListContentType.Caption);
                }
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }

            return captionList;
        }

        /// <summary>
        /// 折列后一笔资料的行数
        /// </summary>
        private int multiRow;

        /// <summary>
        /// 数据加入数组
        /// </summary>
        /// <param name="values">数据</param>
        /// <param name="index">FieldItem的索引</param>
        /// <returns>数组</returns>
        private List<List<PdfDesc>> GetFieldList(object[] values, int datasourceIndex, ListContentType contentType)
        {
            List<List<PdfDesc>> list = new List<List<PdfDesc>>();
            List<PdfDesc> row = new List<PdfDesc>();
            DataSourceItem fieldItem = Report.FieldItems[datasourceIndex];
            bool allowAdd = false;
            multiRow = 1;

            for (int i = 0; i < fieldItem.Fields.Count; i++)
            {
                FieldItem field = fieldItem.Fields[i];
                if (i > 0 && field.NewLine)
                {
                    multiRow++;
                    #region Add row to list
                    
                    if (contentType == ListContentType.GroupTotal || contentType == ListContentType.Total)
                    {
                        foreach (PdfDesc item in row)
                        {
                            allowAdd = item.Value != String.Empty;
                            if (allowAdd)
                            {
                                break;
                            }
                        }
                    }
                    else
                    {
                        allowAdd = true;
                    }

                    if (allowAdd)
                    {
                        list.Add(row);
                    }
                    #endregion
                    row = new List<PdfDesc>();
                    for (int j = 0; j < field.NewLinePostion - 1; j++)
                    {
                        PdfDesc pdfDesc = new PdfDesc();
                        pdfDesc.Value = String.Empty;
                        pdfDesc.LeftLine = false;
                        pdfDesc.RightLine = Report.Format.ColumnInsideGridLine;
                        pdfDesc.TopLine = false;
                        pdfDesc.BottomLine = false;
                        pdfDesc.Width = field.Width;
                        pdfDesc.FieldNum = datasourceIndex;
                        pdfDesc.IsCaption = contentType == ListContentType.Caption;
                        row.Add(pdfDesc);
                    }
                }

                PdfDesc pdfValue = new PdfDesc();
                pdfValue.Value = values[i].ToString();
                pdfValue.Cells = field.Cells;
                pdfValue.FieldNum = datasourceIndex;
                pdfValue.IsCaption = contentType == ListContentType.Caption;
                if (contentType == ListContentType.Caption)
                {
                    pdfValue.HAlign = field.CaptionAlignment;
                }
                else
                {
                    pdfValue.HAlign = field.ColumnAlignment;
                }
                pdfValue.LeftLine = false;

                switch (contentType)
                {
                    case ListContentType.GroupTotal:
                    case ListContentType.Total:
                        pdfValue.RightLine = false;
                        break;
                    case ListContentType.Field:
                    case ListContentType.Caption:
                        pdfValue.RightLine = Report.Format.ColumnInsideGridLine;
                        break;
                }

                if (i == 0 || multiRow == 1 || contentType == ListContentType.GroupTotal || contentType == ListContentType.Total)
                {
                    pdfValue.TopLine = Report.Format.RowGridLine;
                }
                pdfValue.BottomLine = false;
                pdfValue.Width = field.Width;
                row.Add(pdfValue);
            }

            allowAdd = false;
            if (contentType == ListContentType.GroupTotal || contentType == ListContentType.Total)
            {
                foreach (PdfDesc item in row)
                {
                    allowAdd = item.Value != String.Empty;
                    if (allowAdd)
                    {
                        break;
                    }
                }
            }
            else
            {
                allowAdd = true;
            }

            if (allowAdd)
            {
                list.Add(row);
            }

            if (contentType == ListContentType.Field || contentType == ListContentType.Caption)
            {
                if (multiRow > 1)
                {
                    int maxColumnCount = 0;
                    int tempCount = 0;
                    List<int> allLength = new List<int>();
                    foreach (List<PdfDesc> crow in list)
                    {
                        foreach (PdfDesc pdfDesc in crow)
                        {
                            tempCount += pdfDesc.Cells;
                        }

                        if (tempCount > maxColumnCount)
                        {
                            maxColumnCount = tempCount;
                        }

                        allLength.Add(tempCount);

                        tempCount = 0;
                    }

                    foreach (List<PdfDesc> crow in list)
                    {
                        int restCount = maxColumnCount - allLength[list.IndexOf(crow)];
                        for (int i = 0; i < restCount; i++)
                        {
                            if (row[0].GroupGap == DataSourceItem.GroupGapType.None)
                            {
                                PdfDesc pdfDesc = new PdfDesc();
                                pdfDesc.Value = String.Empty;
                                pdfDesc.LeftLine = false;
                                pdfDesc.RightLine = report.Format.ColumnInsideGridLine;

                                pdfDesc.TopLine = report.Format.RowGridLine && list.IndexOf(crow) == 0;
                                pdfDesc.BottomLine = false;
                                pdfDesc.FieldNum = datasourceIndex;
                                pdfDesc.IsCaption = contentType == ListContentType.Caption;
                                crow.Add(pdfDesc);
                            }
                        }
                    }
                }
            }

            return list;
        }
        #endregion

        #region Common Function
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
                }
            }
            if (sort.Length > 0)
            {
                view.Sort = sort.ToString();
            }
            return view.ToTable();
        }

        private string groupFilter;

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


        private Hashtable tableAlignment = new Hashtable();
        private HorizontalAlignment GetDetailAlignment(int index)
        {
            if (tableWidth.Contains(index))
            {
                return (HorizontalAlignment)tableWidth[index];
            }
            else
            {
                return HorizontalAlignment.Left;
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

        private void InitTable()
        {
            for (int i = 0; i < Report.FieldItems.Count; i++)
            {
                int index = 0;
                DataSourceItem fieldItem = Report.FieldItems[i];
                for (int j = 0; j < fieldItem.Fields.Count; j++)
                {
                    FieldItem field = fieldItem.Fields[j];
                    if (field.NewLine && j > 0)
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
                    index += field.Cells;
                }
            }
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

            return string.Format(formatString.ToString(), value);
        }

        private void ThrowException(Exception ex)
        {
            ThrowException(ex, true);
        }

        private void ThrowException(Exception ex, bool showMsg)
        {
            log.WriteExceptionInfo(ex);
            if (showMsg)
            {
                if (Report is EasilyReport)
                {
                    MessageBox.Show(ex.Message);
                }
                else//WebEasilyReport
                {
                    ScriptHelper.ShowMessage(Report as System.Web.UI.Control, ex.Message);
                }
            }
            else
            {
                throw ex;
            }
        }
        #endregion
    }

    enum ListContentType
    { 
        Caption,
        GroupTotal,
        Total,
        Field
    }

    #region CommonClass
    internal class ScriptHelper
    {
        public static void RegisterStartupScript(System.Web.UI.Control ctrl, string script)
        {
            RegisterStartupScript(ctrl, null, String.Empty, script);
        }

        public static void RegisterStartupScript(System.Web.UI.Control ctrl, string key, string script)
        {
            RegisterStartupScript(ctrl, null, key, script);
        }

        public static void RegisterStartupScript(System.Web.UI.Control ctrl, Page page, string key, string script)
        {
            if (string.IsNullOrEmpty(key))
            {
                key = Guid.NewGuid().ToString();
            }

            if (page == null)
            {
                page = ctrl.Page;
            }

            System.Web.UI.Control panel = ctrl.Parent;
            while (panel != null && panel.GetType() != typeof(UpdatePanel))
            {
                panel = panel.Parent;
            }
            if (panel != null)
            {
                ScriptManager.RegisterStartupScript(panel as UpdatePanel, page.GetType(), key, script, true);
            }
            else
            {
                page.ClientScript.RegisterStartupScript(page.GetType(), key, "<script>" + script + "</script>");
            }
        }

        public static void ShowMessage(System.Web.UI.Control ctrl, string key, string message)
        {
            string script = "alert('" + message + "')";
            RegisterStartupScript(ctrl, key, message);
        }

        public static void ShowMessage(System.Web.UI.Control ctrl, string message)
        {
            ShowMessage(ctrl, string.Empty, message);
        }
    }
    #endregion
}
