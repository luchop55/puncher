using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;


namespace SinglePuncher.Classes.Tools
{
    [Serializable]
    public class Round : Tool
    {
        private SolidColorBrush NonSelectedColor = Brushes.Red;
        private SolidColorBrush SelectedColor = Brushes.Blue;

        public Round()
        {

        }

        public double diameter { get; set; }        

        public Round(double diameter)
        {
            this.size = new Size(diameter, diameter);
            this.diameter = diameter;            
            toolName = "ROUND";
            toolDimensions = string.Format("DIAM = {0}",diameter);            
        }

        public override Shape GetToolShape(Point centerPoint, bool selected)
        {
            Ellipse roundShape = new Ellipse();
            roundShape.Stroke = selected ? SelectedColor : NonSelectedColor;            
            roundShape.StrokeThickness = this.StrokeThickness;
            roundShape.Width = diameter;
            roundShape.Height = diameter;
            double xPos = centerPoint.X - roundShape.Width / 2;
            double yPos = centerPoint.Y - roundShape.Height / 2;
            roundShape.Margin = new Thickness(xPos, yPos, -xPos, -yPos);

            return roundShape;
        }
        
    }
}
