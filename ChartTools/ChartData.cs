using System.Drawing;

namespace ChartTools
{
    //Pie Data
    internal class PieValue
    {
        public string Title;
        public float Value;
        public float Percent;

        public PieValue(string title, float value, float percent)
        {
            this.Title = title;
            this.Value = value;
            this.Percent = percent;
        }
    }

    // Line Data
    internal class PointAndValue
    {
        public Point point;
        public int value;

        public PointAndValue(Point point, int value)
        {
            this.point = point;
            this.value = value;
        }
    }
}
