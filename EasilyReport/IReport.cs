using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Infolight.EasilyReportTools.Tools;

namespace Infolight.EasilyReportTools
{
    /// <summary>
    /// The enum of output mode of report
    /// </summary>
    public enum OutputModeType
    {
        None,
        Launch,
        Email,
    }

    public interface IReport
    {
        string ReportID { get; set;}
        string ReportName { get; set;}
        string Description { get; set;}
        object HeaderDataSource { get;set;}

        bool HeaderRepeat { get;set;}

        Font HeaderFont { get; set;}
        ReportItemCollection HeaderItems { get; }
        Font FooterFont { get; set;}
        ReportItemCollection FooterItems { get; }
        Font FieldFont { get; set;}
        DataSourceItemCollection FieldItems { get;}

        ReportFormat Format { get; }
        ParameterItemCollection Parameters { get; }
        ImageItemCollection Images { get; }
        MailConfig MailSetting { get; }

        int CurrentPageIndex { get;}
        int PageCount { get;}

        int DataSourceCount { get;}

        void SetCurrentPageIndex(int pageIndex);
        void SetPageCount(int pageCount);

        IReport Copy();
        void CopyTo(IReport report);

        string EEPAlias { get; }
        string FilePath { get; set;}

        OutputModeType OutputMode { get; set;}

        void Execute();
        bool Load(string fileName);
        void SaveAs(string fileName);
    }

    public interface IReportExport
    {
        /// <summary>
        /// Report instance
        /// </summary>
        IReport Report { get;}

        /// <summary>
        /// The infomation durning the output process
        /// </summary>
        string TagInfo { get; set;}

        /// <summary>
        /// The finished process number the output process
        /// </summary>
        int ProgressInfo { get; set;}

        /// <summary>
        /// The total process count durning the output process
        /// </summary>
        int ProgressCount { get; set;}

        /// <summary>
        /// The full file path of export file
        /// </summary>
        string FileName { get; set;}

        void View();

        /// <summary>
        /// Export report
        /// </summary>
        void Export();

        ExecutionResult CheckValidate();
    }

    public interface IReportGetValues
    {
        string[] GetValues(string propertyName);
    }
}
