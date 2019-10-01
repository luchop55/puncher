using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SinglePuncher.Classes.Tools
{
    [Serializable]
    public class Square : Tool
    {

        private SolidColorBrush NonSelectedColor = Brushes.Red;
        private SolidColorBrush SelectedColor = Brushes.Blue;

        public Square()
        {

        }

        public double side { get; set; }

        public Square(double side)
        {
            this.size = new Size(side, side);
            this.side = side;            
            toolName = "SQUARE";
            toolDimensions = string.Format("SIDE = {0}", side);
        }

        public override Shape GetToolShape(Point centerPoint, bool selected)
        {
            Rectangle squareShape = new Rectangle();
            squareShape.Stroke = selected ? SelectedColor : NonSelectedColor;
            squareShape.Width = side;
            squareShape.Height = side;
            squareShape.StrokeThickness = this.StrokeThickness;            
            double xPos = centerPoint.X - squareShape.Width / 2;
            double yPos = centerPoint.Y - squareShape.Height / 2;
            squareShape.Margin = new Thickness(xPos, yPos, -xPos, -yPos);

            return squareShape;
        }
    }
}
