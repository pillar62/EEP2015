using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Infolight.EasilyReportTools.Tools
{
    internal class UnitConversion
    {
        private static Control control;
        private static Graphics graphics;

        static UnitConversion()
        {
            control = new Control();
            graphics = control.CreateGraphics();
        }
        
        /// <summary>
        /// Change pound to pixel
        /// </summary>
        /// <param name="pound"></param>
        /// <returns></returns>
        public static double PoundToPixel(double pound)
        {
            return pound * 4 / 3;
            //return pound * 13;
        }

        /// <summary>
        /// Change pixel to pound
        /// </summary>
        /// <param name="pixel"></param>
        /// <returns></returns>
        public static double PixelToPound(double pixel)
        {
            return pixel * 3 / 4;
            //return pixel / 13;
        }

        /// <summary>
        /// Change pixel to millimeter
        /// </summary>
        /// <param name="pixel"></param>
        /// <returns></returns>
        public static double PixelToMM(double pixel)
        {
            return pixel * 0.265;
        }

        /// <summary>
        /// Change millimeter to pixel
        /// </summary>
        /// <param name="mm"></param>
        /// <returns></returns>
        public static double MMToPixel(double mm)
        {
            return mm / 0.265;
        }

        /// <summary>
        /// Change pound to millimeter
        /// </summary>
        /// <param name="pound"></param>
        /// <returns></returns>
        public static double PoundToMM(double pound)
        {
            return PixelToMM(PoundToPixel(pound));
        }

        /// <summary>
        /// Change millimeter to pound
        /// </summary>
        /// <param name="mm"></param>
        /// <returns></returns>
        public static double MMToPound(double mm)
        {
            return PixelToPound(MMToPixel(mm));
        }

        /// <summary>
        /// Change inch to pound
        /// </summary>
        /// <param name="inch"></param>
        /// <returns></returns>
        public static double InchToPound(double inch)
        {
            return inch * 72;
        }

        /// <summary>
        /// Change pound to inch
        /// </summary>
        /// <param name="pound"></param>
        /// <returns></returns>
        public static double PoundToInch(double pound)
        {
            return pound / 72;
        }

        /// <summary>
        /// Get letter width (pound) for excel
        /// </summary>
        /// <param name="letterCount"></param>
        /// <param name="font"></param>
        /// <returns></returns>
        public static double GetLetterWidth(int letterCount, System.Drawing.Font font)
        {
            //return PixelToPound(GetSizeW(font) * letterCount);
            return GetSizeW(font) * letterCount / 13;
        }

        /// <summary>
        /// Get letter height (pound) 
        /// </summary>
        /// <param name="letterCount"></param>
        /// <param name="font"></param>
        /// <returns></returns>
        public static double GetLetterHeight(int letterCount, System.Drawing.Font font)
        {
            return PixelToPound(GetSizeH(font) * letterCount);
        }

        /// <summary>
        /// Get letter height (pound) 
        /// </summary>
        /// <param name="font"></param>
        /// <returns></returns>
        public static double GetLetterHeight(System.Drawing.Font font)
        {
            return GetLetterHeight(1, font);
        }

        /// <summary>
        /// Get letter width (pound) for pdf
        /// </summary>
        /// <param name="letterCount"></param>
        /// <param name="font"></param>
        /// <returns></returns>
        public static double GetPdfLetterWidth(int letterCount, System.Drawing.Font font)
        {
            return PixelToPound(GetSizeW(font) * letterCount);
        }

        private static SizeF GetSizeF(System.Drawing.Font font)
        {
            return graphics.MeasureString("M", font);
        }

        /// <summary>
        /// Get letter height by font (pixel)
        /// </summary>
        /// <param name="font"></param>
        /// <returns></returns>
        private static double GetSizeH(System.Drawing.Font font)
        {
            return GetSizeF(font).Height;
        }

        /// <summary>
        /// Get letter width by font (pixel)
        /// </summary>
        /// <param name="font"></param>
        /// <returns></returns>
        private static double GetSizeW(System.Drawing.Font font)
        {
            return GetSizeF(font).Width;
        }

    }
}
