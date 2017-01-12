using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EFServerTools.Common
{
    internal class MessageHelper
    {
        public class EFTransactionMessage
        {
            public static string NotExistRowInTable = "Not exist row in table !";
            public static string TransDesFieldIsReadOnly = "Transaction destination field is ReadOnly !";
            public static string ValueIsNegativeNumber = "The value is a negative number !";
            public static string ColumnNotInTable = "Column {0} is not in {1} !";
        }

        public class EFAutoNumberMessage
        { 
            
        }
    }
}
