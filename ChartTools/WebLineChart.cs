using System;
using System.Collections;
using System.Drawing;
using System.Data;
using System.Drawing.Drawing2D;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace ChartTools
{
    public class WebLineChart : WebChartBase
    {
        public WebLineChart()
        {
        }

        #region Properties
        [Category("Infolight")]
        [DefaultValue(1)]
        public int LineWidth
        {
            get
            {
                object obj = this.ViewState["LineWidth"];
                if (obj != null)
                {
                    return (int)obj;
                }
                return 1;
            }
            set
            {
                this.ViewState["LineWidth"] = value;
            }
        }

        [Category("Infolight")]
        [DefaultValue(typeof(DashStyle), "Solid")]
        public DashStyle DashStyle
        {
            get
            {
                object obj = this.ViewState["DashStyle"];
                if (obj != null)
                {
                    return (DashStyle)obj;
                }
                return DashStyle.Solid;
            }
            set
            {
                this.ViewState["DashStyle"] = value;
            }
        }

        [Category("Infolight")]
        [DefaultValue(10)]
        public int HorizontalLines
        {
            get
            {
                object obj = this.ViewState["HorizontalLines"];
                if (obj != null)
                {
                    return (int)obj;
                }
                return 10;
            }
            set
            {
                this.ViewState["HorizontalLines"] = value;
            }
        }
        #endregion

        Graphics G;
        SolidBrush brush = new SolidBrush(Color.Black);
        SolidBrush blackBrush = new SolidBrush(Color.Black);
        Pen pen = new Pen(Color.Black);
        Pen blackPen = new Pen(Color.Black);
        Bitmap BMP;
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

        protected override void OnLoad(EventArgs e)
        {
            Unit w = (this.Width == Unit.Empty) ? Unit.Pixel(500) : this.Width;
            Unit h = (this.Height == Unit.Empty) ? Unit.Pixel(300) : this.Height;
            //建立圖形
            WebChart.drawInit(ref G, ref BMP, Convert.ToSingle(w.Value), Convert.ToSingle(h.Value));

            //初始化
            this.x = this.LeftMargin;
            this.y = this.TopMargin;
            this.width = BMP.Width - this.LeftMargin - this.RightMargin;
            this.height = BMP.Height - this.TopMargin - this.BottomMargin;
            base.OnLoad(e);
        }

        public void ShowImage()
        {
            //繪製框線 
            RenderOutLine();

            //先檢查資料確定範圍 
            if (this.GetDt() == null)
            {
                WebChart.ShowImage(BMP, this);
                return;
            }
            //繪製格線
            RenderNumberLines();

            //繪製line圖表 
            DrawLineChart();

            //顯示圖表 
            WebChart.ShowImage(BMP, this);
        }

        /// <summary>
        /// 繪製框線 
        /// </summary>
        public void RenderOutLine()
        {
            //清空全部區域
            Rectangle rect = new Rectangle(0, 0, BMP.Width, BMP.Height);
            LinearGradientBrush lineBrush = new LinearGradientBrush(rect, this.BackGroundStartColor, this.BackGroundEndColor, this.BackGroundLinearGradientMode);
            G.FillRectangle(lineBrush, rect); 
            pen.Color = Color.Gray;
            //畫出線條 - 外框
            G.DrawLine(pen, x, y, x, y + height);
            G.DrawLine(pen, x, y + height, x + width, y + height);

            if (this.Caption != null && this.Caption != "")
            {
                brush.Color = this.CaptionColor;
                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                G.DrawString(this.Caption, this.CaptionFont, brush, new RectangleF(0, 0, BMP.Width, BMP.Height), sf);
            }
        }

        public void RenderNumberLines()
        {
            DataTable dt = this.GetDt();
            //找出最大最小值 
            foreach (DataRow row in dt.Rows)
            {
                foreach (WebChartField field in this.DataFields)
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
            MinBoundary = MinValue;
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

        public void DrawLineChart()
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
                    foreach (WebChartField field in this.DataFields)
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
                            //G.DrawString(field.FeildCaption, new Font("Verdana", 8, FontStyle.Regular), blackBrush, xPosition - 2, y + height + 1, sf);
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
                    WebChartField field = this.DataFields[0];

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

        /*public void DrawLineChart()
        {
            DataTable dt = this.GetDt();
            int unitCount = getUnitCount(dt.Rows.Count);
            int UnitxPosition = Convert.ToInt32(width / unitCount);
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
                foreach (WebChartField field in this.DataFields)
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

        private bool isNumColumn()
        {
            if (this.DataFields.Count == 1)
            {
                WebChartField field = this.DataFields[0];
                if (!string.IsNullOrEmpty(field.CaptionFieldName))
                {
                    return false;
                }
            }
            return true;
        }

        private int getUnitCount(int rowCount)
        {
            int unitCount = 0;
            if (isNumColumn())
                unitCount = this.DataFields.Count;
            else
                unitCount = rowCount;
            return unitCount;
        }
    }
}
