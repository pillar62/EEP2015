using System;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Drawing.Drawing2D;
using System.Collections;

namespace ChartTools
{
    public class BarChart : ChartBase
    {
        public BarChart()
        { 
        }

        #region Properties
        private int _horizontalLines = 10;
        [Category("Infolight")]
        [DefaultValue(10)]
        public int HorizontalLines
        {
            get { return _horizontalLines; }
            set { _horizontalLines = value; }
        }
        #endregion

        SolidBrush brush = new SolidBrush(Color.Black);
        SolidBrush blackBrush = new SolidBrush(Color.Black);
        Pen pen = new Pen(Color.Black);
        Pen blackPen = new Pen(Color.Black);
        Font font = new Font("Verdana", 7, FontStyle.Regular);
        float x;
        float y;
        float width;
        float height;
        int MaxValue;//數列實際最大值 
        int MinValue;//數列實際最小值 
        int MaxBoundary;
        int MinBoundary;
        float scaleHeight;
        public Color[] DrawingColors = GloFix.ChartColors();

        public void ShowImage()
        {
            Bitmap BMP = new Bitmap(this.Width, this.Height);
            Graphics G = Graphics.FromImage(BMP);
            drawInit();
            LinearGradientBrush lineBrush = new LinearGradientBrush(this.ClientRectangle, this.BackGroundStartColor, this.BackGroundEndColor, this.BackGroundLinearGradientMode);
            G.FillRectangle(lineBrush, this.ClientRectangle);
            RenderOutLine(G);
            RenderNumberLines(G);
            DrawBarChart(G);

            this.Image = BMP;
        }

        private void drawInit()
        {
            this.x = this.LeftMargin;
            this.y = this.TopMargin;
            this.width = this.Width - this.LeftMargin - this.RightMargin;
            this.height = this.Height - this.TopMargin - this.BottomMargin;
        }

        private void RenderOutLine(Graphics G)
        {
            pen.Color = Color.Gray;
            G.DrawLine(pen, x, y, x, y + height);
            G.DrawLine(pen, x, y + height, x + width, y + height);

            if (this.Caption != null && this.Caption != "")
            {
                brush.Color = this.CaptionColor;
                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                G.DrawString(this.Caption, this.CaptionFont, brush, new RectangleF(0, 0, this.Width, this.Height), sf);
            }
        }

        public void RenderNumberLines(Graphics G)
        {
            DataTable dt = this.GetDt();
            //找出最大最小值 
            foreach (DataRow row in dt.Rows)
            {
                foreach (ChartField field in this.DataFields)
                {
                    float n = Convert.ToSingle(row[field.FieldName]);
                    if (MaxValue < n)
                        MaxValue = Convert.ToInt32(n);
                    if (MinValue > n)
                        MinValue = Convert.ToInt32(n);
                }
            }

            int unitCount = getUnitCount(dt.Rows.Count);
            int UnitxPosition = Convert.ToInt32((width - genSampleRegionWidth()) / unitCount);
            for (int i = 0; i < unitCount; i++)
            {
                if (i % 2 == 1)
                {
                    Rectangle rect = new Rectangle(Convert.ToInt32(x + UnitxPosition * i), Convert.ToInt32(y), UnitxPosition, Convert.ToInt32(height));
                    LinearGradientBrush lineBrush = new LinearGradientBrush(rect, this.BackGroundEndColor, this.BackGroundStartColor, LinearGradientMode.Vertical);
                    G.FillRectangle(lineBrush, rect);
                }
            }

            //最大邊界 
            MaxBoundary = GloFix.MaxLimit(MaxValue);
            MinBoundary = 0;
            //決定繪製高度 
            scaleHeight = MaxBoundary - MinBoundary;
            //開始繪製座標 
            int scaleY = Convert.ToInt32(y + height);
            for (int i = MinBoundary; i <= MaxBoundary; i += Math.Max((MaxBoundary / this.HorizontalLines), 1))
            {
                //寫刻度文字
                brush.Color = this.FontColor;
                G.DrawString(i.ToString(), font, brush, x - 25, scaleY);
                //畫虛線
                pen.DashStyle = DashStyle.Dash;
                G.DrawLine(pen, x, scaleY, x + width, scaleY);
                //累加 
                scaleY = scaleY - Convert.ToInt32((height / this.HorizontalLines));
            }
        }

        public void DrawBarChart(Graphics G)
        {
            DataTable dt = this.GetDt();
            int unitCount = getUnitCount(dt.Rows.Count);
            int srw = genSampleRegionWidth();
            int UnitxPosition = Convert.ToInt32((width - srw) / unitCount);
            int n = 0, offsetX = 20;
            ArrayList pointAndValues = new ArrayList();
            pen.DashStyle = DashStyle.Solid;
            float xPosition = x + (n * offsetX);
            foreach (DataRow row in dt.Rows)
            {
                //決定每一組數列的顏色 
                Color C = DrawingColors[n % DrawingColors.Length];
                if (isNumColumn())
                {
                    xPosition = x + (n * offsetX);
                    foreach (ChartField field in this.DataFields)
                    {
                        float v = Convert.ToSingle(row[field.FieldName]);
                        float Hight = (height / scaleHeight) * v;
                        float BarY = y + (height - Hight);

                        // front
                        brush.Color = C;
                        G.FillRectangle(brush, xPosition, BarY, 20, y + height - BarY);
                        G.DrawRectangle(pen, xPosition, BarY, 20, y + height - BarY);
                        // upper
                        brush.Color = Color.FromArgb(((C.R + 30) < 255 ? (C.R + 30) : 255), ((C.G + 30) < 255 ? (C.G + 30) : 255), ((C.B + 30) < 255 ? (C.B + 30) : 255));
                        Point[] upperPoints = new Point[] {
                        new Point(Convert.ToInt32(xPosition), Convert.ToInt32(BarY)),
                        new Point(Convert.ToInt32(xPosition + 20), Convert.ToInt32(BarY)), 
                        new Point(Convert.ToInt32(xPosition + 25), Convert.ToInt32(BarY - 5)),
                        new Point(Convert.ToInt32(xPosition + 5), Convert.ToInt32(BarY - 5))};
                        G.FillPolygon(brush, upperPoints);
                        G.DrawPolygon(pen, upperPoints);
                        // right
                        brush.Color = Color.FromArgb(((C.R - 30) > 0 ? (C.R - 30) : 0), ((C.G - 30) > 0 ? (C.G - 30) : 0), ((C.B - 30) > 0 ? (C.B - 30) : 0));
                        Point[] rightPoints = new Point[] {
                        new Point(Convert.ToInt32(xPosition + 20), Convert.ToInt32(y + height)),
                        new Point(Convert.ToInt32(xPosition + 25), Convert.ToInt32(y + height - 5)), 
                        new Point(Convert.ToInt32(xPosition + 25), Convert.ToInt32(BarY - 5)),
                        new Point(Convert.ToInt32(xPosition + 20), Convert.ToInt32(BarY))};
                        G.FillPolygon(brush, rightPoints);
                        G.DrawPolygon(pen, rightPoints);
                        pointAndValues.Add(new PointAndValue(new Point(Convert.ToInt32(xPosition), Convert.ToInt32(BarY)), Convert.ToInt32(v)));
                        if (n == 0)
                        {
                            StringFormat sf = new StringFormat();
                            sf.Alignment = StringAlignment.Center;
                            G.DrawString(field.FeildCaption, new Font("Verdana", 8, FontStyle.Regular), blackBrush, new RectangleF(xPosition - n * offsetX, y + height + 1, UnitxPosition, y + height), sf);
                        }
                        //累加位移 
                        xPosition += UnitxPosition;
                    }
                    if (srw != 0)
                    {
                        //图例
                        int sampleX = Convert.ToInt32(width + this.LeftMargin - srw);
                        G.FillRectangle(brush, sampleX, y + n * 20, 10, 10);
                        G.DrawRectangle(blackPen, sampleX, y + n * 20, 10, 10);
                        G.DrawString(row[this.DataFields[0].CaptionFieldName].ToString(), font, blackBrush, sampleX + 10, y + n * 20 - 2);
                    }
                }
                else
                {
                    ChartField field = this.DataFields[0];

                    float v = Convert.ToSingle(row[field.FieldName]);
                    float Hight = (height / scaleHeight) * v;
                    float BarY = y + (height - Hight);

                    // front
                    brush.Color = C;
                    G.FillRectangle(brush, xPosition, BarY, 20, y + height - BarY);
                    G.DrawRectangle(pen, xPosition, BarY, 20, y + height - BarY);
                    // upper
                    brush.Color = Color.FromArgb(((C.R + 30) < 255 ? (C.R + 30) : 255), ((C.G + 30) < 255 ? (C.G + 30) : 255), ((C.B + 30) < 255 ? (C.B + 30) : 255));
                    Point[] upperPoints = new Point[] {
                        new Point(Convert.ToInt32(xPosition), Convert.ToInt32(BarY)),
                        new Point(Convert.ToInt32(xPosition + 20), Convert.ToInt32(BarY)), 
                        new Point(Convert.ToInt32(xPosition + 25), Convert.ToInt32(BarY - 5)),
                        new Point(Convert.ToInt32(xPosition + 5), Convert.ToInt32(BarY - 5))};
                    G.FillPolygon(brush, upperPoints);
                    G.DrawPolygon(pen, upperPoints);
                    // right
                    brush.Color = Color.FromArgb(((C.R - 30) > 0 ? (C.R - 30) : 0), ((C.G - 30) > 0 ? (C.G - 30) : 0), ((C.B - 30) > 0 ? (C.B - 30) : 0));
                    Point[] rightPoints = new Point[] {
                        new Point(Convert.ToInt32(xPosition + 20), Convert.ToInt32(y + height)),
                        new Point(Convert.ToInt32(xPosition + 25), Convert.ToInt32(y + height - 5)), 
                        new Point(Convert.ToInt32(xPosition + 25), Convert.ToInt32(BarY - 5)),
                        new Point(Convert.ToInt32(xPosition + 20), Convert.ToInt32(BarY))};
                    G.FillPolygon(brush, rightPoints);
                    G.DrawPolygon(pen, rightPoints);

                    pointAndValues.Add(new PointAndValue(new Point(Convert.ToInt32(xPosition), Convert.ToInt32(BarY)), Convert.ToInt32(v)));
                    StringFormat sf = new StringFormat();
                    sf.Alignment = StringAlignment.Center;
                    G.DrawString(row[field.CaptionFieldName].ToString(), new Font("Verdana", 8, FontStyle.Regular), blackBrush, new RectangleF(xPosition, y + height + 1, UnitxPosition, y + height), sf);
                    //累加位移 
                    xPosition += UnitxPosition;
                }
                n += 1;
            }
            if (this.ShowValue)
            {
                //繪出數值 
                foreach (PointAndValue item in pointAndValues)
                {
                    string value = item.value.ToString();
                    G.DrawString(value, font, blackBrush, item.point.X, item.point.Y - 10);
                }
            }
        }

        /*public void DrawBarChart(Graphics G)
        {
            DataTable dt = this.GetDt();
            int unitCount = getUnitCount(dt.Rows.Count);
            int UnitxPosition = Convert.ToInt32(width / unitCount);
            int n = 0, offsetX = 20;
            ArrayList pointAndValues = new ArrayList();
            pen.DashStyle = DashStyle.Solid;
            foreach (DataRow row in dt.Rows)
            {
                float xPosition = x + (n * offsetX);
                //決定每一組數列的顏色 
                Color C = DrawingColors[n % DrawingColors.Length];
                foreach (ChartField field in this.DataFields)
                {
                    float v = Convert.ToSingle(row[field.FieldName]);
                    float Hight = (height / scaleHeight) * v;
                    float BarY = y + (height - Hight);

                    // front
                    brush.Color = C;
                    G.FillRectangle(brush, xPosition, BarY, 20, y + height - BarY);
                    G.DrawRectangle(pen, xPosition, BarY, 20, y + height - BarY);
                    // upper
                    brush.Color = Color.FromArgb(((C.R + 30) < 255 ? (C.R + 30) : 255), ((C.G + 30) < 255 ? (C.G + 30) : 255), ((C.B + 30) < 255 ? (C.B + 30) : 255));
                    Point[] upperPoints = new Point[] {
                        new Point(Convert.ToInt32(xPosition), Convert.ToInt32(BarY)),
                        new Point(Convert.ToInt32(xPosition + 20), Convert.ToInt32(BarY)), 
                        new Point(Convert.ToInt32(xPosition + 25), Convert.ToInt32(BarY - 5)),
                        new Point(Convert.ToInt32(xPosition + 5), Convert.ToInt32(BarY - 5))
                    };
                    G.FillPolygon(brush, upperPoints);
                    G.DrawPolygon(pen, upperPoints);
                    // right
                    brush.Color = Color.FromArgb(((C.R - 30) > 0 ? (C.R - 30) : 0), ((C.G - 30) > 0 ? (C.G - 30) : 0), ((C.B - 30) > 0 ? (C.B - 30) : 0));
                    Point[] rightPoints = new Point[] {
                        new Point(Convert.ToInt32(xPosition + 20), Convert.ToInt32(y + height)),
                        new Point(Convert.ToInt32(xPosition + 25), Convert.ToInt32(y + height - 5)), 
                        new Point(Convert.ToInt32(xPosition + 25), Convert.ToInt32(BarY - 5)),
                        new Point(Convert.ToInt32(xPosition + 20), Convert.ToInt32(BarY))
                    };
                    G.FillPolygon(brush, rightPoints);
                    G.DrawPolygon(pen, rightPoints);
                    pointAndValues.Add(new PointAndValue(new Point(Convert.ToInt32(xPosition), Convert.ToInt32(BarY)), Convert.ToInt32(v)));
                    if (n == 0)
                    {
                        StringFormat sf = new StringFormat();
                        sf.Alignment = StringAlignment.Center;
                        G.DrawString(field.FeildCaption, new Font("Verdana", 8, FontStyle.Regular), blackBrush, new RectangleF(xPosition - n * offsetX, y + height + 1, UnitxPosition, y + height), sf);
                    }
                    //累加位移 
                    xPosition += UnitxPosition;
                }
                n += 1;
            }
            if (this.ShowValue)
            {
                //繪出數值 
                foreach (PointAndValue item in pointAndValues)
                {
                    string value = item.value.ToString();
                    G.DrawString(value, font, blackBrush, item.point.X, item.point.Y - 10);
                }
            }
        }*/
    }
}
