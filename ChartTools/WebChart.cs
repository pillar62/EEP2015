using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Web.UI.WebControls;

namespace ChartTools
{
    public class WebChart
    {
        /// <summary>
        /// 建立一個圖形物件(Bitmap) 
        /// </summary>
        /// <param name="G">繪圖物件</param>
        /// <param name="BMP">BMP物件</param>
        /// <param name="width">影像寬度</param>
        /// <param name="height">影像高度</param>
        public static void drawInit(ref Graphics G, ref Bitmap BMP, float width, float height)
        {
            //畫在BMP上面 
            BMP = new Bitmap(Convert.ToInt32(width), Convert.ToInt32(height), PixelFormat.Format24bppRgb);
            G = Graphics.FromImage(BMP);
            //筆刷 
            SolidBrush myBrush = new SolidBrush(Color.FromArgb(0, 224, 224, 224));

            //清空全部區域 
            myBrush.Color = Color.FromArgb(255, 224, 224, 224);
            G.FillRectangle(myBrush, 0, 0, width, height);
        }

        public static void ShowImage(Bitmap BMP, System.Web.UI.WebControls.Image image)
        {
            //取亂數建立 key
            string ImageKey = "IMG_" + Guid.NewGuid().ToString();
            image.Page.Application[ImageKey] = BMP;
            image.ImageUrl = "~/ImageSerivces.ashx?ImageKey=" + ImageKey;
        }
    }
}
