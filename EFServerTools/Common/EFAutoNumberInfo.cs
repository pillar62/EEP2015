using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EFServerTools.Common
{
    internal class EFAutoNumberInfo
    {
        public const string UpdateComponent = "The EFUpdateComponent applied to update.";
        public const string AutoNoID = "Identification of the control.";
        public const string TargetColumn = "The column which AutoNumber is applied to.";
        public const string AutoNumberTableName = "The table of AutoNumber.";
        public const string GetFixed = "Pre-code of AutoNumber.";
        public const string NumDig = "The digit of coding by AutoNumber.";
        public const string StartValue = "The first number of each regular coding by AutoNumber.";
        public const string Step = "The increment between the coding by AutoNumber.";
        public const string OverFlow = "Indicates whether overflow is allowed.";
        public const string Active = "Indicates whether AutoNumber is enabled or disabled.";
        public const string IsNumFill = "Indicates whether use isNumFill or not.";
        public const string Description = "Description of the coding.";

    }
}
