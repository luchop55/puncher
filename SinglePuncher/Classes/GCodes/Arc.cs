using SinglePuncher.Classes.Tools;
using System;
using System.Windows;
using System.Windows.Controls;
using WpfApplication1.Classes.GCommands;

namespace SinglePuncher.Classes.GCodes
{
    [Serializable]
    public class Arc : Gcode
    {        
        public double arcRadius { get; set; }
        public double startingAngle { get; set; }
        public double angleBetweenHits { get; set; }
        public int numberOfPunches { get; set; }
        Size sheetSize = MainWindow.SheetSize;


        public Arc(double xCenter, double yCenter, double arcRadius, double startingAngle,
                   double angleBetweenHits, int numberOfPunches, Tool tool, bool selected)
        {
            this.xStart = xCenter;
            this.yStart = yCenter;
            this.arcRadius = arcRadius;
            this.startingAngle = startingAngle;
            this.angleBetweenHits = angleBetweenHits;
            this.numberOfPunches = numberOfPunches;
            this.tool = tool;
            this.selected = selected;          
           
        }

        public Arc()
        {
        }

        public override string getParameterDescription()
        {
            return String.Format("(ARC, {0}, {1}, {2}, {3}, {4}, {5}, {6})",                                                        
                                                        xStart,
                                                        yStart,
                                                        arcRadius,
                                                        startingAngle,
                                                        angleBetweenHits,
                                                        numberOfPunches,
                                                        tool.toolName,
                                                        tool.toolDimensions
                                                        );
        }

        public override void Draw(ref Canvas mainCanvas)
        {
            double currentAngle = startingAngle;            

            Canvas shapeCanvas = HelperClass.CreateCanvas(xStart, yStart);

            Point punchPoint = new Point();

            punchPoint = HelperClass.GetCircularCoordinates(arcRadius, currentAngle);

            for (int i = 0; i < numberOfPunches; i++)
            {
                shapeCanvas.Children.Add(tool.GetToolShape(punchPoint, selected));

                currentAngle += angleBetweenHits;
                punchPoint = HelperClass.GetCircularCoordinates(arcRadius, currentAngle);
            }
            mainCanvas.Children.Add(shapeCanvas);
        }

        public override void Punch(SinglePuncher singlePuncher)
        {
            SinglePuncher _singlePuncher = singlePuncher;

            double currentAngle = startingAngle;

            Point punchPoint = new Point(0, 0);

            punchPoint = HelperClass.GetCircularCoordinates(arcRadius, currentAngle);

            punchPoint.Offset(xStart, yStart);

            for (int i = 0; i < numberOfPunches; i++)
            {

                //insertar aqui el metodo para posicionar ejes                

                _singlePuncher.MoveCommand(punchPoint);

                currentAngle += angleBetweenHits;
                punchPoint = HelperClass.GetCircularCoordinates(arcRadius, currentAngle);
                punchPoint.Offset(xStart, yStart);
            }
        }      

    }
}
