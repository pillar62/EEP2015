using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Infolight.EasilyReportTools.DataCenter
{
    internal class SysRptDB
    {
        public const string SysReportID = "TEMPLATE";
        public const string TableName = "SYS_REPORT";
        public const string ReportID = "REPORTID";
        public const string ReportName = "REPORTNAME";
        public const string FilePath = "FILEPATH";
        public const string FileName = "FILENAME";
        public const string Description = "DESCRIPTION";
        public const string OutputMode = "OUTPUTMODE";
        public const string HeaderRepeat = "HEADERREPEAT";
        public const string HeaderFont = "HEADERFONT";
        public const string HeaderItems = "HEADERITEMS";
        public const string FooterFont = "FOOTERFONT";
        public const string FooterItems = "FOOTERITEMS";
        public const string FieldFont = "FIELDFONT";
        public const string FieldItems = "FIELDITEMS";
        public const string Setting = "SETTING";
        public const string Format = "FORMAT";
        public const string Parameters = "PARAMETERS";
        public const string Images = "IMAGES";
        public const string MailSetting = "MAILSETTING";

        public static ArrayList sysTableColumns;
        static SysRptDB()
        {
            sysTableColumns = new ArrayList();
            sysTableColumns.Add(ReportID);
            sysTableColumns.Add(FileName);
            sysTableColumns.Add(ReportName);
            sysTableColumns.Add(Description);
            sysTableColumns.Add(FilePath);
            sysTableColumns.Add(OutputMode);
            sysTableColumns.Add(HeaderRepeat);
            sysTableColumns.Add(HeaderFont);
            sysTableColumns.Add(HeaderItems);
            sysTableColumns.Add(FooterFont);
            sysTableColumns.Add(FooterItems);
            sysTableColumns.Add(FieldFont);
            sysTableColumns.Add(FieldItems);
            sysTableColumns.Add(Setting);
            sysTableColumns.Add(Format);
            sysTableColumns.Add(Parameters);
            sysTableColumns.Add(Images);
            sysTableColumns.Add(MailSetting);
        }
    }
}
