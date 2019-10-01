using SinglePuncher.Classes.Tools;
using System;
using System.Windows;
using System.Windows.Controls;
using WpfApplication1.Classes.GCommands;

namespace SinglePuncher.Classes.GCodes
{
    [Serializable]
    public class CurveContour : Gcode
    {
        public double arcRadius{ get; set; }
        public double startingAngle { get; set; }
        public double endingAngle { get; set; }
        public int P { get; set; }
        public double distanceBetweenHits { get; set; }
        Size sheetSize = MainWindow.SheetSize;

        public CurveContour()
        {

        }

        public CurveContour(double xCenter, double yCenter, double arcRadius, double startingAngle,
                                double endingAngle, int P, double distanceBetweenHits, Tool tool, bool selected)
        {
            this.xStart = xCenter;
            this.yStart = yCenter;
            this.arcRadius = arcRadius;
            this.startingAngle = startingAngle;
            this.endingAngle = endingAngle;
            this.P = P;
            this.distanceBetweenHits = distanceBetweenHits;
            this.tool = tool;
            this.selected = selected;
            
        }


        public override string getParameterDescription()
        {
            return String.Format("(CURVE CONTOUR, {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7},{8})",
                                                        xStart,
                                                        yStart,
                                                        arcRadius,
                                                        startingAngle,
                                                        endingAngle,
                                                        P,
                                                        distanceBetweenHits,
                                                        tool.toolName,
                                                        tool.toolDimensions
                                                        );
        }


        public override void Draw(ref Canvas mainCanvas)
        {
            if (!(arcRadius > 0)) { return; }

            double trayAngle = endingAngle - startingAngle;
            double hitsAngle = distanceBetweenHits / arcRadius;
            hitsAngle = HelperClass.RadianToDegree(hitsAngle);
            int numberOfPunches = Math.Abs(Convert.ToInt32(trayAngle / hitsAngle));
            double hitsAngleFixed = trayAngle / Convert.ToDouble(numberOfPunches);
            double currentAngle = startingAngle;

            double arcRadiusFixed = 0;

            //double toolDiam = Math.Sqrt(Math.Pow(tool.size.Width, 2) + Math.Pow(tool.size.Height, 2));

            double toolDiam = tool.size.Width;

            if (P == 0)
                arcRadiusFixed = arcRadius;
            else if (P == 1)
                arcRadiusFixed = arcRadius + (toolDiam / 2);
            else if (P == -1)
                arcRadiusFixed = arcRadius - (toolDiam / 2);

            Canvas shapeCanvas = HelperClass.CreateCanvas(xStart, yStart);

            Point punchPoint = new Point();

            for (int i = 0; i <= numberOfPunches; i++)
            {
                punchPoint = HelperClass.GetCircularCoordinates(arcRadiusFixed, currentAngle);
                
                shapeCanvas.Children.Add(tool.GetToolShape(punchPoint, selected));

                currentAngle += hitsAngleFixed;
            }

            mainCanvas.Children.Add(shapeCanvas);
        }

        public override void Punch(SinglePuncher singlePuncher)
        {
            SinglePuncher _singlePuncher = singlePuncher;

            if (!(arcRadius > 0)) { return; }

            double trayAngle = endingAngle - startingAngle;
            double hitsAngle = distanceBetweenHits / arcRadius;
            hitsAngle = HelperClass.RadianToDegree(hitsAngle);
            int numberOfPunches = Math.Abs(Convert.ToInt32(trayAngle / hitsAngle));
            double hitsAngleFixed = trayAngle / Convert.ToDouble(numberOfPunches);
            double currentAngle = startingAngle;

            double arcRadiusFixed = 0;

            //double toolDiam = Math.Sqrt(Math.Pow(tool.size.Width, 2) + Math.Pow(tool.size.Height, 2));

            double toolDiam = tool.size.Width;

            if (P == 0)
                arcRadiusFixed = arcRadius;
            else if (P == 1)
                arcRadiusFixed = arcRadius + (toolDiam / 2);
            else if (P == -1)
                arcRadiusFixed = arcRadius - (toolDiam / 2);

            Point punchPoint = new Point(0, 0);

            for (int i = 0; i <= numberOfPunches; i++)
            {
                punchPoint = HelperClass.GetCircularCoordinates(arcRadiusFixed, currentAngle);
                punchPoint.Offset(xStart, yStart);

                //insertar aqui el metodo para posicionar ejes                
                _singlePuncher.MoveCommand(punchPoint);

                currentAngle += hitsAngleFixed;
            }
        }
    }
}
