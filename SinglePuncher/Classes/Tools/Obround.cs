using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SinglePuncher.Classes.Tools
{
    [Serializable]
    public class Obround : Tool
    {

        private SolidColorBrush NonSelectedColor = Brushes.Red;
        private SolidColorBrush SelectedColor = Brushes.Blue;

        public Obround()
        {

        }

        public Obround(Size size)
        {
            this.size = size;            
            toolName = "OBROUND";
            toolDimensions = string.Format("W = {0} x H = {1}", size.Width, size.Height);
        }

        public override Shape GetToolShape(Point centerPoint, bool selected)
        {
            Point startingPoint;
            Point secPoint;
            Point thrdPoint;
            Point fthPoint;
            PathGeometry pathGeometry = new PathGeometry();
            PathFigure figure = new PathFigure();
            Path path = new Path();
            double xPos = 0;
            double yPos = 0;

            if(size.Width > size.Height)
            {
                startingPoint = new Point(-(size.Width - size.Height) / 2, size.Height / 2);
                secPoint = new Point(startingPoint.X + size.Width - size.Height, startingPoint.Y);
                thrdPoint = new Point(secPoint.X, secPoint.Y - size.Height);
                fthPoint = new Point(thrdPoint.X - (size.Width - size.Height), thrdPoint.Y);

                figure.StartPoint = startingPoint;
                figure.Segments.Add(new LineSegment((secPoint), true));

                figure.Segments.Add(new ArcSegment(thrdPoint, new Size(size.Height / 2, size.Height / 2),
                                    180, false, SweepDirection.Counterclockwise, true));

                figure.Segments.Add(new LineSegment(fthPoint, true));

                figure.Segments.Add(new ArcSegment(startingPoint, new Size(size.Height / 2, size.Height / 2),
                                    0, false, SweepDirection.Counterclockwise, true));

                pathGeometry.Figures.Add(figure);                
            }

            else
            {
                startingPoint = new Point(-size.Width / 2, (size.Height - size.Width) / 2);
                secPoint = new Point(startingPoint.X, -(size.Height - size.Width) / 2);
                thrdPoint = new Point(secPoint.X + size.Width, secPoint.Y);
                fthPoint = new Point(thrdPoint.X, startingPoint.Y);

                figure.StartPoint = startingPoint;

                figure.Segments.Add(new LineSegment(secPoint, true));

                figure.Segments.Add(new ArcSegment(thrdPoint, new Size(size.Width / 2, size.Width / 2), 
                    0,false, SweepDirection.Clockwise, true));

                figure.Segments.Add(new LineSegment(fthPoint, true));

                figure.Segments.Add(new ArcSegment(startingPoint, new Size(size.Width/2, size.Width/2), 
                    0, false, SweepDirection.Clockwise, true));

                pathGeometry.Figures.Add(figure);                
            }

            path.Data = pathGeometry;

            path.Stroke = selected ? SelectedColor : NonSelectedColor;

            path.StrokeThickness = this.StrokeThickness;

            xPos = centerPoint.X;
            yPos = centerPoint.Y;

            path.Margin = new Thickness(xPos, yPos, -xPos, -yPos);

            return path;
        }
    }
}
