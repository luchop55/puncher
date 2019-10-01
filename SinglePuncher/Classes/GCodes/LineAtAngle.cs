using SinglePuncher.Classes.Tools;
using System;
using System.Windows;
using System.Windows.Controls;
using WpfApplication1.Classes.GCommands;

namespace SinglePuncher.Classes.GCodes
{
    [Serializable]
    public class LineAtAngle : Gcode
    {
        public double angle { get; set; }
        public double hitsDistance { get; set; }
        public int numberOfPunches { get; set; }        
        Size sheetSize = MainWindow.SheetSize;

        public LineAtAngle()
        {

        }

        public LineAtAngle(double xStart, double yStart, double hitsDistance, double angle,
                                int numberOfPunches, Tool tool, bool selected)
        {
            this.xStart = xStart;
            this.yStart = yStart;
            this.hitsDistance = hitsDistance;
            this.angle = angle;
            this.numberOfPunches = numberOfPunches;
            this.tool = tool;
            this.selected = selected;            
        }

        public override string getParameterDescription()
        {
            return String.Format("(LINE AT ANGLE, {0}, {1}, {2}, {3}, {4}, {5}, {6})",
                                                        xStart,
                                                        yStart,
                                                        hitsDistance,
                                                        angle,
                                                        numberOfPunches,
                                                        tool.toolName,
                                                        tool.toolDimensions
                                                        );
        }

        public override void Draw(ref Canvas mainCanvas)
        {
            double angleRads = HelperClass.DegreeToRadian(angle);

            Canvas shapeCanvas = HelperClass.CreateCanvas(xStart, yStart);

            Point punchPoint = new Point(0, 0);

            for (int i = 0; i < numberOfPunches; i++)
            {
                shapeCanvas.Children.Add(tool.GetToolShape(punchPoint, selected));

                punchPoint.X += hitsDistance * Math.Cos(angleRads);
                punchPoint.Y += hitsDistance * Math.Sin(angleRads);
            }

            mainCanvas.Children.Add(shapeCanvas);
        }

        public override void Punch(SinglePuncher singlePuncher)
        {
            SinglePuncher _singlePuncher = singlePuncher;

            double angleRads = HelperClass.DegreeToRadian(angle);

            Point punchPoint = new Point(0, 0);
            punchPoint.Offset(xStart, yStart);

            for (int i = 0; i < numberOfPunches; i++)
            {

                //insertar aqui el metodo para posicionar ejes
                
                _singlePuncher.MoveCommand(punchPoint);

                punchPoint.X += hitsDistance * Math.Cos(angleRads);
                punchPoint.Y += hitsDistance * Math.Sin(angleRads);
            }
        }
    }
}
