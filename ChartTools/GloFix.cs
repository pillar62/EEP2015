using System;
using System.Drawing;

namespace ChartTools
{
    public class GloFix
    {
        public static Color[] ChartColors()
        {
            return new Color[] { 
                //Color.FromArgb(51, 51, 255), 
                //Color.FromArgb(255, 255, 0), 
                //Color.FromArgb(204, 102, 0), 
                //Color.FromArgb(204, 204, 255), 
                //Color.FromArgb(153, 102, 255), 
                //Color.FromArgb(0, 153, 255), 
                //Color.FromArgb(255, 255, 153)

                
                Color.FromArgb(255, 255, 0, 0),
                
                Color.FromArgb(255, 0, 255, 255),
                Color.FromArgb(255, 255, 0, 255),
                Color.FromArgb(255, 255, 255, 0),
                Color.FromArgb(255,255,165,0),

                Color.FromArgb(255, 0, 0, 127),
                Color.FromArgb(255, 0, 127, 0),
                Color.FromArgb(255, 127, 0, 0),

                //Color.FromArgb(255, 127, 255, 255),//去掉
                Color.FromArgb(255, 255, 127, 255),
                Color.FromArgb(255, 255, 255, 127),
               

                Color.FromArgb(255, 0, 127, 127),
                Color.FromArgb(255, 127, 0, 127),
                Color.FromArgb(255, 127, 127, 0),

                Color.FromArgb(127, 0, 0, 255),
                Color.FromArgb(127, 0, 255, 0),
                Color.FromArgb(127, 255, 0, 0),
                
                //Color.FromArgb(127, 0, 255, 255),//去掉
                Color.FromArgb(127, 255, 0, 255),
                Color.FromArgb(127, 255, 255, 0),
                 Color.FromArgb(127,255,165,0),

                Color.FromArgb(127, 0, 0, 127),
                Color.FromArgb(127, 0, 127, 0),
                Color.FromArgb(127, 127, 0, 0),

                Color.FromArgb(127, 127, 255, 255),
                Color.FromArgb(127, 255, 127, 255),
                Color.FromArgb(127, 255, 255, 127),

                Color.FromArgb(127, 0, 127, 127),
                Color.FromArgb(127, 127, 0, 127),
                Color.FromArgb(127, 127, 127, 0),

                Color.FromArgb(255, 0, 0, 255), //放到最后
                Color.FromArgb(255, 0, 255, 0),

               
            };
        }

        public static int MaxLimit(int maxValue)
        {
            string sMaxValue = maxValue.ToString();
            int length = sMaxValue.Length;
            string zero = "";
            for(int i = 0; i < length - 1; i++)
            {
                zero += "0";
            }
            return Convert.ToInt32((Convert.ToInt32(sMaxValue.Substring(0, 1)) + 1).ToString() + zero);
        }

        public static bool ContainsField(string columnName, string dataFields)
        {
            string[] keyFields = dataFields.Split(';');
            foreach (string key in keyFields)
            {
                if (string.Compare(key, columnName, true) == 0)
                    return true;
            }
            return false;
        }
    }

    //public enum SourceNumericType
    //{ 
    //    ColumnNumricType = 0,
    //    RowNumricType = 1
    //}
}
