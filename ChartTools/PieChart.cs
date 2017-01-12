using System;
using System.ComponentModel;
using System.Drawing;
using System.Collections;
using System.Data;
using System.Drawing.Drawing2D;

namespace ChartTools
{
    public class PieChart : ChartBase
    {        
        public PieChart()
        {
        }

        #region Properties
        private bool _showRate = true;
        [Category("Infolight")]
        [DefaultValue(true)]
        public bool ShowRate
        {
            get
            {
                return _showRate;
            }
            set
            {
                _showRate = value;
            }
        }
        #endregion

        ArrayList PieValues = new ArrayList();
        SolidBrush brush = new SolidBrush(Color.Black);
        SolidBrush blackBrush = new SolidBrush(Color.Black);
        Pen blackPen = new Pen(Color.Black);
        Font font = new Font("Verdana", 9, FontStyle.Regular);
        float x;
        float y;
        float width;
        float height;
        public Color[] DrawingColors = GloFix.ChartColors();

        private void drawInit()
        {
            this.x = this.LeftMargin;
            this.y = this.TopMargin;
            this.width = this.Width - this.LeftMargin - this.RightMargin;
            this.height = this.Height - this.TopMargin - this.BottomMargin;
        }

        public void ShowImage(int rowIndex)
        {
            PieValues.Clear();
            Bitmap BMP = new Bitmap(this.Width, this.Height);
            Graphics G = Graphics.FromImage(BMP);
            drawInit();
            LinearGradientBrush lineBrush = new LinearGradientBrush(this.ClientRectangle, this.BackGroundStartColor, this.BackGroundEndColor, this.BackGroundLinearGradientMode);
            G.FillRectangle(lineBrush, this.ClientRectangle);

            RenderOutLine(G);

            DrawPieChart(G, rowIndex);
            this.Image = BMP;
        }

        public void RenderOutLine(Graphics G)
        {
            if (this.Caption != null && this.Caption != "")
            {
                brush.Color = this.CaptionColor;
                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                G.DrawString(this.Caption, this.CaptionFont, brush, new RectangleF(0, 0, this.Width, this.Height), sf);
            }
        }

        public void DrawPieChart(Graphics G, int rowIndex)
        {
            float pieX = x + 30;//圓左上角X
            float pieY = x + 30;//圓左上角Y
            float diameter = (height > width) ? width - 50 : height - 50;//圓直徑

            DataTable dt = this.GetDt();
            //計算總計 
            float sum = 0;
            foreach (ChartField field in this.DataFields)
            {
                sum += Convert.ToSingle(dt.Rows[rowIndex][field.FieldName]);
            }
            //填入值
            foreach (ChartField field in this.DataFields)
            {
                float value = Convert.ToSingle(dt.Rows[rowIndex][field.FieldName]);
                PieValues.Add(new PieValue(field.FeildCaption, value, value / sum));
            }

            //绘制图形
            //brush.Color = Color.FromArgb(80, 80, 80);
            //G.FillEllipse(brush, pieX - 8, pieY, diameter, diameter); //先绘制阴影
            //G.DrawEllipse(new Pen(Color.Black, 3f), pieX, pieY, diameter, diameter); //绘制边框
            float y = 30;
            int n = 0, startAngle = 0/*開始角度*/, endAngle = 0/*結束角度*/;
            foreach (PieValue item in PieValues)
            {
                endAngle = Convert.ToInt32(startAngle + item.Percent * 360);
                if (n + 1 == this.DataFields.Count)
                    endAngle = 360;
                Rectangle rect = new Rectangle(Convert.ToInt32(pieX), Convert.ToInt32(pieY), Convert.ToInt32(diameter), Convert.ToInt32(diameter));
                LinearGradientBrush lineBrush = new LinearGradientBrush(rect, Color.Honeydew, DrawingColors[n % DrawingColors.Length], startAngle + (endAngle - startAngle) / 2);
                G.FillPie(lineBrush, pieX, pieY, diameter, diameter, startAngle, endAngle - startAngle);

                //图例
                brush.Color = DrawingColors[n % DrawingColors.Length];
                float sampleX = width - this.RightMargin - this.LeftMargin;
                G.FillRectangle(brush, sampleX, y, 10, 10);
                G.DrawRectangle(blackPen, sampleX, y, 10, 10);
                G.DrawString(item.Title, font, blackBrush, sampleX + 10, y - 2);
                y += 20;
                //绘出值和百分比
                float circleCentreX, circleCentreY, radii;
                circleCentreX = pieX + (diameter / 2);
                circleCentreY = pieY + (diameter / 2);
                radii = diameter / 2;
                //绘出圆心
                float percentX, percentY;
                float angle;
                angle = startAngle + (endAngle - startAngle) / 2;
                percentX = Convert.ToSingle(circleCentreX + (radii * 2 / 3) * Math.Cos(angle * Math.PI / 180));
                percentY = Convert.ToSingle(circleCentreY + (radii * 2 / 3) * Math.Sin(angle * Math.PI / 180));

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
                //下一个
                startAngle = endAngle;
                n += 1;
            }
        }
    }
}
