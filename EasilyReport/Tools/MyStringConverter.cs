using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Infolight.EasilyReportTools.Tools
{
    internal class MyStringConverter
    {
        public static string GetFullFilePath(string filePath, string fileName, ReportFormat.ExportType exportFormat)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return string.Empty;
            }
            if (filePath.Substring(filePath.Length - 1) != "\\")
            {
                filePath += "\\";
            }

            return filePath + GetFullFileName(fileName, exportFormat);
        }

        public static string GetFullFileName(string fileName, ReportFormat.ExportType exportFormat)
        {
            string filePrefix = String.Empty;

            if (fileName != String.Empty)
            {
                filePrefix = fileName.Split('.')[0];
            }
            else
            {
                filePrefix = fileName;
            }

            switch (exportFormat)
            {
                case ReportFormat.ExportType.Excel:
                    if (filePrefix.IndexOf(FileSuffix.ExcelFileSuffix, StringComparison.CurrentCultureIgnoreCase) == -1)
                    {
                        return filePrefix + FileSuffix.ExcelFileSuffix;
                    }
                    else
                    {
                        return fileName;
                    }
                case ReportFormat.ExportType.Pdf:
                    if (filePrefix.IndexOf(FileSuffix.Pdf, StringComparison.CurrentCultureIgnoreCase) == -1)
                    {
                        return filePrefix + FileSuffix.Pdf;
                    }
                    else
                    {
                        return fileName;
                    }
            }

            return String.Empty;
        }

        public static string GetFileNameWithoutExtension(string fileName, ReportFormat.ExportType exportFormat)
        {
            switch (exportFormat)
            {
                case ReportFormat.ExportType.Excel:
                    if (fileName.IndexOf(FileSuffix.ExcelFileSuffix, StringComparison.CurrentCultureIgnoreCase) == -1)
                    {
                        return fileName;
                    }
                    else
                    {
                        return fileName.Split('.')[0];
                    }
                case ReportFormat.ExportType.Pdf:
                    if (fileName.IndexOf(FileSuffix.Pdf, StringComparison.CurrentCultureIgnoreCase) == -1)
                    {
                        return fileName;
                    }
                    else
                    {
                        return fileName.Split('.')[0];
                    }
            }

            return String.Empty;
        }
    }

    internal class FileSuffix
    {
        public const string ExcelFileSuffix = ".xls";
        public const string Excel2007FileSuffix = ".xlsx";
        public const string Pdf = ".pdf";
    }
}
