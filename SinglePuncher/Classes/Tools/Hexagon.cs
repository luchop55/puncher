using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SinglePuncher.Classes.Tools
{
    [Serializable]
    public class Hexagon : Tool
    {
        private SolidColorBrush NonSelectedColor = Brushes.Red;
        private SolidColorBrush SelectedColor = Brushes.Blue;

        public Hexagon()
        {

        }

        public double diameter { get; set; }

        public Hexagon(double diameter)
        {
            this.size = new Size(diameter, diameter);
            this.diameter = diameter;
            toolName = "HEXAGON";
            toolDimensions = string.Format("DIAM = {0}", diameter);
        }

        public override Shape GetToolShape(Point centerPoint, bool selected)
        {
            double radius = diameter / 2;
            double shortSide = radius / 2;
            double longSide = radius * (Math.Sin(1.0472));

            Point pt1 = new Point(2 * radius, radius);
            Point pt2 = new Point(1.5 * radius, radius - longSide);
            Point pt3 = new Point(radius / 2, radius - longSide);
            Point pt4 = new Point(0, radius);
            Point pt5 = new Point(radius / 2, radius + longSide);
            Point pt6 = new Point(1.5 * radius, radius + longSide);

            Polygon hexagon = new Polygon();
            hexagon.Stroke = selected ? SelectedColor : NonSelectedColor;
            hexagon.StrokeThickness = this.StrokeThickness;
            hexagon.Points = new PointCollection() { pt1, pt2, pt3, pt4, pt5, pt6 };
            double xPos = centerPoint.X - radius;
            double yPos = centerPoint.Y - radius;
            hexagon.Margin = new Thickness(xPos, yPos, -xPos, -yPos);

            return hexagon;
        }
    }
}
