using SinglePuncher.Classes.Tools;
using System;
using System.Windows;
using System.Windows.Controls;
using WpfApplication1.Classes.GCommands;

namespace SinglePuncher.Classes.GCodes
{
    [Serializable]
    public class BoltHoleCircle : Gcode
    {        
        public double radius { get; set; }
        public double firstPunchAngle { get; set; }
        public int numberOfPunches { get; set; }
        Size sheetSize = MainWindow.SheetSize;

        public BoltHoleCircle()
        {

        }

        public BoltHoleCircle(double xCenter, double yCenter, double radius, double firstPunchAngle, int numberOfPunches,
                              Tool tool, bool selected)
        {
            this.xStart = xCenter;
            this.yStart = yCenter;
            this.radius = radius;
            this.firstPunchAngle = firstPunchAngle;
            this.numberOfPunches = numberOfPunches;
            this.tool = tool;            
            this.selected = selected;            
        }

        public override string getParameterDescription()
        {
            return String.Format("(BOLT HOLE CIRCLE, {0}, {1}, {2}, {3}, {4}, {5}, {6})",
                                                        xStart,
                                                        yStart,
                                                        radius,
                                                        firstPunchAngle,
                                                        numberOfPunches,
                                                        tool.toolName,
                                                        tool.toolDimensions
                                                        );
        }



        public override void Draw(ref Canvas mainCanvas)
        {
            Canvas shapeCanvas = HelperClass.CreateCanvas(xStart, yStart);

            Point punchPoint = new Point();

            double angleCirc = firstPunchAngle;

            for (int i = 0; i < numberOfPunches; i++)
            {
                punchPoint = HelperClass.GetCircularCoordinates(radius, angleCirc);
                
                shapeCanvas.Children.Add(tool.GetToolShape(punchPoint, selected));
                                
                angleCirc += 360.0 / Convert.ToDouble(numberOfPunches);
            }

            mainCanvas.Children.Add(shapeCanvas);
        }

        public override void Punch(SinglePuncher singlePuncher)
        {
            SinglePuncher _singlePuncher = singlePuncher;

            Point punchPoint = new Point(0, 0);

            double angleCirc = firstPunchAngle;

            for (int i = 0; i < numberOfPunches; i++)
            {
                punchPoint = HelperClass.GetCircularCoordinates(radius, angleCirc);
                punchPoint.Offset(xStart, yStart);

                //insertar aqui el metodo para posicionar ejes

                _singlePuncher.MoveCommand(punchPoint);

                angleCirc += 360.0 / Convert.ToDouble(numberOfPunches);
            }
        }
    }
}
