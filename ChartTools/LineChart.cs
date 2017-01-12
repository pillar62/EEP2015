using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Data;
using System;
using System.Collections;

namespace ChartTools
{
    public class LineChart : ChartBase
    {
        public LineChart()
        { 
        }

        #region Properties
        private int _lineWidth = 1;
        [Category("Infolight")]
        [DefaultValue(1)]
        public int LineWidth
        {
            get { return _lineWidth; }
            set { _lineWidth = value; }
        }

        private DashStyle _dashStyle = DashStyle.Solid;
        [Category("Infolight")]
        [DefaultValue(typeof(DashStyle), "Solid")]
        public DashStyle DashStyle
        {
            get { return _dashStyle; }
            set { _dashStyle = value; }
        }

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
            DrawLineChart(G);

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
            pen.Width = 1;
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

        private void RenderNumberLines(Graphics G)
        {
            DataTable dt = this.GetDt();
            //找出最大最小值 
            foreach (DataRow row in dt.Rows)
            {
                foreach (ChartField field in this.DataFields)
                {
                    int n = Convert.ToInt32(row[field.FieldName]);
                    if (MaxValue < n)
                        MaxValue = n;
                    if (MinValue > n)
                        MinValue = n;
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
                pen.Width = 1;
                G.DrawLine(pen, x, scaleY, x + width, scaleY);
                //累加
                scaleY = scaleY - Convert.ToInt32(height / this.HorizontalLines);
            }
        }

        public void DrawLineChart(Graphics G)
        {
            DataTable dt = this.GetDt();
            int unitCount = getUnitCount(dt.Rows.Count);
            int srw = genSampleRegionWidth();
            int UnitxPosition = Convert.ToInt32((width - srw) / unitCount); 
            ArrayList points = new ArrayList();
            //記錄要畫的點 
            ArrayList pointAndValues = new ArrayList();
            int n = 0;
            Point LastPoint2 = new Point();
            float xPosition = x + UnitxPosition / 2;
            foreach (DataRow row in dt.Rows)
            {
                //決定每一組數列的顏色 
                Color C = DrawingColors[n % DrawingColors.Length];
                if (isNumColumn())
                {
                    xPosition = x + UnitxPosition / 2;
                    Point LastPoint = new Point();
                    foreach (ChartField field in this.DataFields)
                    {
                        brush.Color = C;
                        float v = Convert.ToSingle(row[field.FieldName]);
                        float Hight = (height / scaleHeight) * v;
                        float BarY = y + (height - Hight);
                        if (LastPoint.X == 0 & LastPoint.Y == 0)
                        {
                            LastPoint.X = Convert.ToInt32(xPosition);
                            LastPoint.Y = Convert.ToInt32(BarY);
                        }
                        else
                        {
                            points.Add(LastPoint);
                            points.Add(new Point(Convert.ToInt32(xPosition), Convert.ToInt32(BarY)));

                            pen.Color = C;
                            pen.Width = this.LineWidth;
                            pen.DashStyle = this.DashStyle;
                            G.DrawLine(pen, LastPoint, new Point(Convert.ToInt32(xPosition), Convert.ToInt32(BarY)));

                            LastPoint = new Point(Convert.ToInt32(xPosition), Convert.ToInt32(BarY));
                        }

                        //繪出數值在點上面(記錄起來，因為要在最後面一次畫) 
                        pointAndValues.Add(new PointAndValue(new Point(Convert.ToInt32(xPosition), Convert.ToInt32(BarY)), Convert.ToInt32(v)));
                        if (n == 0)
                        {
                            StringFormat sf = new StringFormat();
                            sf.Alignment = StringAlignment.Center;
                            G.DrawString(field.FeildCaption, new Font("Verdana", 8, FontStyle.Regular), blackBrush, new RectangleF(xPosition - UnitxPosition / 2, y + height + 1, UnitxPosition, y + height), sf);
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
                    n += 1;
                }
                else
                {
                    ChartField field = this.DataFields[0];

                    float v = Convert.ToSingle(row[field.FieldName]);
                    float Hight = (height / scaleHeight) * v;
                    float BarY = y + (height - Hight);
                    if (LastPoint2.X == 0 & LastPoint2.Y == 0)
                    {
                        LastPoint2.X = Convert.ToInt32(xPosition);
                        LastPoint2.Y = Convert.ToInt32(BarY);
                    }
                    else
                    {
                        points.Add(LastPoint2);
                        points.Add(new Point(Convert.ToInt32(xPosition), Convert.ToInt32(BarY)));

                        pen.Color = C;
                        pen.Width = this.LineWidth;
                        pen.DashStyle = this.DashStyle;
                        G.DrawLine(pen, LastPoint2, new Point(Convert.ToInt32(xPosition), Convert.ToInt32(BarY)));

                        LastPoint2 = new Point(Convert.ToInt32(xPosition), Convert.ToInt32(BarY));
                    }

                    //繪出數值在點上面(記錄起來，因為要在最後面一次畫) 
                    pointAndValues.Add(new PointAndValue(new Point(Convert.ToInt32(xPosition), Convert.ToInt32(BarY)), Convert.ToInt32(v)));
                    StringFormat sf = new StringFormat();
                    sf.Alignment = StringAlignment.Center;
                    G.DrawString(row[field.CaptionFieldName].ToString(), new Font("Verdana", 8, FontStyle.Regular), blackBrush, new RectangleF(xPosition - UnitxPosition / 2, y + height + 1, UnitxPosition, y + height), sf);
                    //累加位移 
                    xPosition += UnitxPosition;
                }

                if (this.ShowValue)
                {
                    //繪出數值 
                    foreach (PointAndValue item in pointAndValues)
                    {
                        string value = item.value.ToString();
                        G.DrawString(value, font, blackBrush, item.point.X - 10, item.point.Y - 10);
                    }
                }
            }
        }

        /*public void DrawLineChart(Graphics G)
        {
            DataTable dt = this.GetDt();
            int UnitxPosition = Convert.ToInt32(width / this.DataFields.Count);
            ArrayList points = new ArrayList();
            //記錄要畫的點 
            ArrayList pointAndValues = new ArrayList();
            int n = 0;
            foreach (DataRow row in dt.Rows)
            {
                float xPosition = x + UnitxPosition / 2;
                //決定每一組數列的顏色 
                Color C = DrawingColors[n % DrawingColors.Length];

                Point LastPoint = new Point();
                foreach (ChartField field in this.DataFields)
                {
                    float v = Convert.ToSingle(row[field.FieldName]);
                    float Hight = (height / scaleHeight) * v;
                    float BarY = y + (height - Hight);
                    if (LastPoint.X == 0 & LastPoint.Y == 0)
                    {
                        LastPoint.X = Convert.ToInt32(xPosition);
                        LastPoint.Y = Convert.ToInt32(BarY);
                    }
                    else
                    {
                        points.Add(LastPoint);
                        points.Add(new Point(Convert.ToInt32(xPosition), Convert.ToInt32(BarY)));

                        pen.Color = C;
                        pen.Width = this.LineWidth;
                        pen.DashStyle = this.DashStyle;
                        G.DrawLine(pen, LastPoint, new Point(Convert.ToInt32(xPosition), Convert.ToInt32(BarY)));

                        LastPoint = new Point(Convert.ToInt32(xPosition), Convert.ToInt32(BarY));
                    }

                    //繪出數值在點上面(記錄起來，因為要在最後面一次畫) 
                    pointAndValues.Add(new PointAndValue(new Point(Convert.ToInt32(xPosition), Convert.ToInt32(BarY)), Convert.ToInt32(v)));
                    if (n == 0)
                    {
                        StringFormat sf = new StringFormat();
                        sf.Alignment = StringAlignment.Center;
                        //G.DrawString(field.FeildCaption, new Font("Verdana", 8, FontStyle.Regular), blackBrush, xPosition - 2, y + height + 1, sf);
                        G.DrawString(field.FeildCaption, new Font("Verdana", 8, FontStyle.Regular), blackBrush, new RectangleF(xPosition - UnitxPosition / 2, y + height + 1, UnitxPosition, y + height), sf);
                    }

                    //累加位移 
                    xPosition += UnitxPosition;
                }

                //畫出點(為了在最上層，所以最後畫)
                //SolidBrush whiteBrush = new SolidBrush(Color.White);
                //foreach (Point point in points)
                //{
                //    G.FillEllipse(whiteBrush, point.X - 2, point.Y - 2, 7, 7);
                //}

                if (this.ShowValue)
                {
                    //繪出數值 
                    foreach (PointAndValue item in pointAndValues)
                    {
                        string value = item.value.ToString();
                        G.DrawString(value, font, blackBrush, item.point.X - 10, item.point.Y - 10);
                    }
                }
                n += 1;
            }
        }*/
    }
}