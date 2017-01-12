using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EFServerTools.Common
{
    internal class EFLogInfo
    {
        public const string UpdateComponent = "The EFUpdateComponent applied to update.";
        public const string LogTableName = "The name of table to store log data.";
        public const string OnlyDistinct = "Indicates whether all the columns or only modified columns need to log.";
        public const string SrcFieldNames = "Specifies which columns needed to log when they are modified.";
        public const string NeedLog = "Indicates whether LogInfo is enabled or disabled.";
        public const string MarkField = "Specifies the column to store the infomation of type of modification.";
        public const string ModifierField = "Specifies the column to store the name of the modified column.";
        public const string LogIDField = "Specifies the column to store the idenfication of log.";
        public const string ModifyDateField = "Specifies the column to store the data of the modified column.";
        public const string LogDateFormat = "The date format of the time of log.";
    }
}
