using System;
using System.Collections.Generic;
using System.Text;

namespace Infolight.EasilyReportTools.Tools
{
    internal class MessageInfo
    {
        public static string MultiGroupModeError = "Can not use multi group mode at one time.";
        public static string MultiExcelGroupColumnError = "Excel Group is only allow one group column.";
        public static string EEPAliasNull = "Please select EEPAlias.";
        
        public static string ExcelGroupNewLineError = ERptMultiLanguage.GetLanValue("MsgExcelGroupNewLineError");
        public static string GroupColumnsNotSetError = ERptMultiLanguage.GetLanValue("MsgGroupColumnsNotSetError");
        public static string GroupColumnNotOrderBy = ERptMultiLanguage.GetLanValue("MsgGroupColumnNotOrderBy");

        public static string DataSourceNull = ERptMultiLanguage.GetLanValue("MsgDataSourceNull");
        public static string HeaderDataSourceNull = ERptMultiLanguage.GetLanValue("MsgHeaderDataSourceNull");
        public static string ImageItemsNull = ERptMultiLanguage.GetLanValue("MsgImageItemsNull");
        public static string ParameterItemsNull = ERptMultiLanguage.GetLanValue("MsgParameterItemsNull");
        public static string FieldItemsNull = ERptMultiLanguage.GetLanValue("MsgFieldItemsNull");

        public static string SaveSuccess = ERptMultiLanguage.GetLanValue("MsgSaveSuccess");
        public static string DeleteSuccess = ERptMultiLanguage.GetLanValue("MsgDeleteSuccess");

        public static string TemplateExist = ERptMultiLanguage.GetLanValue("MsgTemplateExist");
        public static string TemplateFileNameIsNull = ERptMultiLanguage.GetLanValue("MsgTemplateFileNameIsNull");
        public static string TemplateDeleteConfirm = ERptMultiLanguage.GetLanValue("MsgTemplateDeleteConfirm");
        public static string FileTypeNotSupport = ERptMultiLanguage.GetLanValue("MsgFileTypeNotSupport");
        
    }

    internal class StatusMsgInfo
    {
        public static string HeaderItemAdd = ERptMultiLanguage.GetLanValue("MsgHeaderItemAdd");
        public static string FooterItemAdd = ERptMultiLanguage.GetLanValue("MsgFooterItemAdd");
        public static string ReportDetails = ERptMultiLanguage.GetLanValue("MsgReportDetails");
        public static string ReportSetting = ERptMultiLanguage.GetLanValue("MsgReportSetting");
    }

    internal class ProcessTagInfo
    {
        public static string ProcessStart = ERptMultiLanguage.GetLanValue("MsgProcessStart");
        public static string Exporting = ERptMultiLanguage.GetLanValue("MsgExporting");
        public static string ProcessFinished = ERptMultiLanguage.GetLanValue("MsgProcessFinished");
        public static string ProcessAbortRequest = ERptMultiLanguage.GetLanValue("MsgProcessAbortRequest");
    }

    internal class ReportDesignerTabPageInfo
    {
        public static string ReportHeader = "Report Header";
        public static string ReportDetails = "Report Details";
        public static string ReportFooter = "Report Footer";
        public static string ReportSetting = "Report Setting";
        public static string Tag = "*";
    }

    internal class LogFileInfo
    {
        public static string logFileName = String.Format("{0}{1}{2}", "EasilyReport", DateTime.Now.ToString("yyyyMMdd"), ".log");
    }

    internal class ImageFilePath
    { 
        public const string PdfImage = @"c:\PdfTempImage";
        public const string ExcelImage = @"c:\ExcelTempImage";
        public const string ProgressBarUrl = "~/Image/Ajax/updateProgress.gif";
    }

    internal class ExportFileInfo
    {
        public const string PdfPreviewFilePath = @"c:\pdfPreview.pdf";
        public static string ExcelTempFileForWeb = @"excelTemp" + CurrentDateTime + ".xls";
        public static string PdfTempFileForWeb = @"pdfTemp" + CurrentDateTime + ".pdf";
        public static string CurrentDateTime = DateTime.Now.ToString("yyyyMMddHHmmss");
    }

    internal class MailMessageInfo
    {
        public static string InvalidMailAddress = ERptMultiLanguage.GetLanValue("MsgInvalidMailAddress");
        public static string MailFromEmpty = ERptMultiLanguage.GetLanValue("MsgMailFromEmpty");
        public static string MailToEmpty = ERptMultiLanguage.GetLanValue("MsgMailToEmpty");
    }

    internal class TitleMsgInfo
    {
        public static string Error = ERptMultiLanguage.GetLanValue("MsgError");
        public static string Success = ERptMultiLanguage.GetLanValue("MsgSuccess");
        public static string Warning = ERptMultiLanguage.GetLanValue("MsgWarning");
    }

    internal class ExportMsgInfo
    {
        public static string ExportSuccess = ERptMultiLanguage.GetLanValue("MsgExportSuccess");
    }

    internal class TotalInfo
    { 
        public const string DefaultTotalCaption= "EasilyReport.TotalCaption";
        public const string DefaultGroupTotalCaption = "EasilyReport.GroupTotalCaption";
    }

}
