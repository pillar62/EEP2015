using System;
using System.Collections;
using System.Drawing;
using System.Data;
using System.Drawing.Drawing2D;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Srvtools;
using System.ComponentModel.Design;
using System.ComponentModel;
using System.Web.UI.Design;

namespace ChartTools
{
    public class WebPieChart : WebChartBase
    {
        ArrayList PieValues = new ArrayList();

        #region Properties
        [Category("Infolight")]
        [DefaultValue(true)]
        public bool ShowRate
        {
            get
            {
                object obj = this.ViewState["ShowRate"];
                if (obj != null)
                {
                    return (bool)obj;
                }
                return true;
            }
            set
            {
                this.ViewState["ShowRate"] = value;
            }
        }
        #endregion

        Graphics G;
        Bitmap BMP;
        SolidBrush brush = new SolidBrush(Color.Black);
        SolidBrush blackBrush = new SolidBrush(Color.Black);
        Pen blackPen = new Pen(Color.Black);
        Font font = new Font("Verdana", 9, FontStyle.Regular);
        float x;
        float y;
        float width;
        float height;
        public Color[] DrawingColors = GloFix.ChartColors();

        public WebPieChart()
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            Unit w = (this.Width == Unit.Empty) ? Unit.Pixel(500) : this.Width;
            Unit h = (this.Height == Unit.Empty) ? Unit.Pixel(300) : this.Height;
            WebChart.drawInit(ref G, ref BMP, Convert.ToSingle(w.Value), Convert.ToSingle(h.Value));

            this.x = this.TopMargin;
            this.y = this.LeftMargin;
            this.width = BMP.Width - this.LeftMargin - this.RightMargin;
            this.height = BMP.Height - this.TopMargin - this.BottomMargin;
            base.OnLoad(e);
        }

        public void ShowImage(int rowIndex)
        {
            RenderOutLine();
            if (this.GetDt() == null)
            {
                WebChart.ShowImage(BMP, this);
                return;
            }
            DrawPieChart(rowIndex);
            WebChart.ShowImage(BMP, this);
        }

        public void RenderOutLine()
        {
            Rectangle rect = new Rectangle(0, 0, BMP.Width, BMP.Height);
            LinearGradientBrush lineBrush = new LinearGradientBrush(rect, this.BackGroundStartColor, this.BackGroundEndColor, this.BackGroundLinearGradientMode);
            G.FillRectangle(lineBrush, rect);
            if (this.Caption != null && this.Caption != "")
            {
                brush.Color = this.CaptionColor;
                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                G.DrawString(this.Caption, this.CaptionFont, brush, new RectangleF(0, 0, BMP.Width, BMP.Height), sf);
            }
        }

        public void DrawPieChart(int rowIndex)
        {
            float pieX = x + 30;//圓左上角X
            float pieY = x + 30;//圓左上角Y
            float diameter = (height > width) ? width - 50 : height - 50;//圓直徑

            DataTable dt = this.GetDt();
            //計算總計 
            float sum = 0;
            foreach (WebChartField field in this.DataFields)
            {
                sum += Convert.ToSingle(dt.Rows[rowIndex][field.FieldName]);
            }
            //填入值
            foreach (WebChartField field in this.DataFields)
            {
                float value = Convert.ToSingle(dt.Rows[rowIndex][field.FieldName]);
                PieValues.Add(new PieValue(field.FeildCaption, value, value / sum));
            }

            //繪製圖形
            float y = 30;
            int n = 0, startAngle = 0/*開始角度*/, endAngle = 0/*結束角度*/;
            foreach (PieValue item in PieValues)
            {
                //繪製
                endAngle = Convert.ToInt32(startAngle + item.Percent * 360);
                if (n + 1 == this.DataFields.Count)
                    endAngle = 360;
                Rectangle rect = new Rectangle(Convert.ToInt32(pieX), Convert.ToInt32(pieY), Convert.ToInt32(diameter), Convert.ToInt32(diameter));
                LinearGradientBrush lineBrush = new LinearGradientBrush(rect, Color.Honeydew, DrawingColors[n % DrawingColors.Length], startAngle + (endAngle - startAngle) / 2);
                G.FillPie(lineBrush, pieX, pieY, diameter, diameter, startAngle, endAngle - startAngle);
                //圖例
                brush.Color = DrawingColors[n % DrawingColors.Length];
                float sampleX = width - this.RightMargin - this.LeftMargin;//圖例x
                G.FillRectangle(brush, sampleX, y, 10, 10);
                G.DrawRectangle(blackPen, sampleX, y, 10, 10);
                G.DrawString(item.Title, font, blackBrush, sampleX + 10, y - 2);
                y += 20;
                //繪出比例 xx% 和值
                float circleCentreX, circleCentreY, radii;
                circleCentreX = pieX + (diameter / 2);
                circleCentreY = pieY + (diameter / 2);
                radii = diameter / 2;
                //畫出圓心
                float percentX, percentY;
                float angle;//角度
                angle = startAngle + (endAngle - startAngle) / 2;
                percentX = Convert.ToSingle(circleCentreX + (radii * 2 / 3) * Math.Cos(angle * Math.PI / 180));
                percentY = Convert.ToSingle(circleCentreY + (radii * 2 / 3) * Math.Sin(angle * Math.PI / 180));

                //畫出文字
                string Dw = "";
                if (this.ShowValue)
                    Dw += item.Value.ToString();
                if (this.ShowRate)
                {
                    string rate = string.Format("{0:N1}", item.Percent * 100);
                    if (Dw != "")
                        Dw += "(" + rate + "%)";
                    else
                        Dw += rate + "%";
                }
                percentX = percentX - 9 / 3 * Dw.Length;
                G.DrawString(Dw, font, blackBrush, percentX, percentY);
                //下一個
                startAngle = endAngle;
                n += 1;
            }
        }
    }
}
