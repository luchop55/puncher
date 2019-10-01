using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SinglePuncher.Classes.Tools
{
    [Serializable]
    public class Rect : Tool
    {

        private SolidColorBrush NonSelectedColor = Brushes.Red;
        private SolidColorBrush SelectedColor = Brushes.Blue;

        public Rect()
        {

        }

        public Rect(Size size)
        {
            this.size = size;            
            toolName = "RECTANGLE";
            toolDimensions = string.Format("W = {0} x H = {1}", size.Width, size.Height);
        }

        public override Shape GetToolShape(Point centerPoint, bool selected)
        {
            Rectangle RectangleShape = new Rectangle();
            RectangleShape.Stroke = selected ? SelectedColor : NonSelectedColor;
            RectangleShape.Width = size.Width;
            RectangleShape.Height = size.Height;
            RectangleShape.StrokeThickness = this.StrokeThickness;
            double xPos = centerPoint.X - RectangleShape.Width / 2;
            double yPos = centerPoint.Y - RectangleShape.Height / 2;
            RectangleShape.Margin = new Thickness(xPos, yPos, -xPos, -yPos);

            return RectangleShape;
        }
    }
}
