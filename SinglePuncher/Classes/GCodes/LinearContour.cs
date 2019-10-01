using SinglePuncher.Classes.Tools;
using System;
using System.Windows;
using System.Windows.Controls;
using WpfApplication1.Classes.GCommands;

namespace SinglePuncher.Classes.GCodes
{
    [Serializable]
    public class LinearContour : Gcode
    {

        public double angle { get; set; }
        public double hitsDistance { get; set; }
        public double lineLenght { get; set; }        
        Size sheetSize = MainWindow.SheetSize;

        public LinearContour()
        {

        }

        public LinearContour(double xStart, double yStart, double hitsDistance, double angle,
                                double lineLenght, Tool tool, bool selected)
        {
            this.xStart = xStart;
            this.yStart = yStart;
            this.hitsDistance = hitsDistance;
            this.angle = angle;
            this.lineLenght = lineLenght;
            this.tool = tool;
            this.selected = selected;           
        }


        public override string getParameterDescription()
        {
            return String.Format("(LINEAR CONTOUR, {0}, {1}, {2}, {3}, {4}, {5}, {6})",
                                                        xStart,
                                                        yStart,
                                                        hitsDistance,
                                                        angle,
                                                        lineLenght,
                                                        tool.toolName,
                                                        tool.toolDimensions
                                                        );
        }

        public override void Draw(ref Canvas mainCanvas)
        {
            if (!(hitsDistance > 0)) { return; }

            Canvas shapeCanvas = HelperClass.CreateCanvas(xStart, yStart);

            double radAngle = HelperClass.DegreeToRadian(angle);

            Point punchPoint = new Point();

            punchPoint.X = 0;
            punchPoint.Y = 0;

            int numberOfPunches = Convert.ToInt32(lineLenght / hitsDistance);
            double holeDistanceCorr = lineLenght / numberOfPunches;

            for (int i = 0; i <= numberOfPunches; i++)
            {
                shapeCanvas.Children.Add(tool.GetToolShape(punchPoint, selected));

                punchPoint.X += holeDistanceCorr * Math.Cos(radAngle);
                punchPoint.Y += holeDistanceCorr * Math.Sin(radAngle);
            }

            mainCanvas.Children.Add(shapeCanvas);
        }

        public override void Punch(SinglePuncher singlePuncher)
        {
            SinglePuncher _singlePuncher = singlePuncher;

            if (!(hitsDistance > 0)) { return; }

            double radAngle = HelperClass.DegreeToRadian(angle);

            Point punchPoint = new Point(0, 0);
            punchPoint.Offset(xStart, yStart);

            int numberOfPunches = Convert.ToInt32(lineLenght / hitsDistance);
            double holeDistanceCorr = lineLenght / numberOfPunches;

            for (int i = 0; i <= numberOfPunches; i++)
            {

                //insertar aqui el metodo para posicionar ejes
                
                _singlePuncher.MoveCommand(punchPoint);

                punchPoint.X += holeDistanceCorr * Math.Cos(radAngle);
                punchPoint.Y += holeDistanceCorr * Math.Sin(radAngle);
            }
        }
    }
}
