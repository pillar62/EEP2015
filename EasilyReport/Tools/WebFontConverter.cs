using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.Drawing;

namespace Infolight.EasilyReportTools.Tools
{
    internal class WebFontConverter
    {
        public static FontInfo ConvertFrom(Font font)
        {
            FontInfo fontInfo = new Label().Font;
            if (font != null)
            {
                fontInfo.Bold = font.Bold;
                fontInfo.Italic = font.Italic;
                fontInfo.Name = font.Name;
                fontInfo.Strikeout = font.Strikeout;
                fontInfo.Size = new FontUnit(font.Size, UnitType.Pixel);
                fontInfo.Underline = font.Underline;
            }
            return fontInfo;
        }

    }
}
