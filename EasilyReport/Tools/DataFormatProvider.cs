using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Infolight.EasilyReportTools.Tools
{
    internal class DataFormatProvider
    {
        public static string GetFormatValue(DDProvider ddProvider, DataView dvData, string columnName, object value)
        {
            string formatValue = String.Empty;
            object ddValue = null;

            ddValue = ddProvider.GetDDValue(columnName, DDInfo.EditMask);
            if (ddValue != null && ddValue.ToString() != String.Empty)
            {
                switch (dvData.Table.Columns[columnName].DataType.Name)
                {
                    case "Int16":
                        formatValue = Convert.ToInt16(value).ToString(ddValue.ToString());
                        break;
                    case "Int32":
                        formatValue = Convert.ToInt32(value).ToString(ddValue.ToString());
                        break;
                    case "Int64":
                        formatValue = Convert.ToInt64(value).ToString(ddValue.ToString());
                        break;
                    case "Double":
                        formatValue = Convert.ToDouble(value).ToString(ddValue.ToString());
                        break;
                    case "Decimal":
                        formatValue = Convert.ToDecimal(value).ToString(ddValue.ToString());
                        break;
                    case "UInt16":
                        formatValue = Convert.ToUInt16(value).ToString(ddValue.ToString());
                        break;
                    case "UInt32":
                        formatValue = Convert.ToUInt32(value).ToString(ddValue.ToString());
                        break;
                    case "UInt64":
                        formatValue = Convert.ToUInt64(value).ToString(ddValue.ToString());
                        break;

                    case "DateTime":
                        formatValue = Convert.ToDateTime(value).ToString(ddValue.ToString());
                        break;

                    case "String":
                    case "Boolean":
                        formatValue = value.ToString();
                        break;

                    default:
                        formatValue = value.ToString();
                        break;
                }
            }
            else
            {
                formatValue = value.ToString();
            }

            return formatValue;
        }

        public static string ReviseExcelValue(string dataType, string value)
        {
            string reviseValue = String.Empty;
            switch (value)
            {
                case "Int16":
                case "Int32":
                case "Int64":
                case "Double":
                case "Decimal":
                case "UInt16":
                case "UInt32":
                case "UInt64":
                    if (!string.IsNullOrEmpty(value))
                    {
                        if (char.IsDigit(Convert.ToChar(value.Substring(0, 1))))
                        {
                            reviseValue = "'" + value;
                        }
                        else
                        {
                            reviseValue = value;
                        }
                    }
                    else
                    {
                        reviseValue = value;
                    }
                    break;
                case "String":
                case "DateTime":
                case "Boolean":
                default:
                    reviseValue = value;
                    break;
            }
            return reviseValue;
        }
    }
}
