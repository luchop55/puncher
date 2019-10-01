using SinglePuncher.Classes.Tools;
using System;
using System.Windows;
using System.Windows.Controls;
using WpfApplication1.Classes.GCommands;

namespace SinglePuncher.Classes.GCodes
{
    [Serializable]
    public class GridY : Gcode
    {
        public double hitsDistanceX { get; set; }
        public int numberOfPunchesX { get; set; }
        public double hitsDistanceY { get; set; }
        public int numberOfPunchesY { get; set; }
        Size sheetSize = MainWindow.SheetSize;

        public GridY()
        {

        }

        public GridY(double xStart, double yStart, double hitsDistanceX, int numberOfPunchesX,
            double hitsDistanceY, int numberOfPunchesY, Tool tool, bool selected)
        {
            this.xStart = xStart;
            this.yStart = yStart;
            this.hitsDistanceX = hitsDistanceX;
            this.hitsDistanceY = hitsDistanceY;
            this.numberOfPunchesX = numberOfPunchesX;
            this.numberOfPunchesY = numberOfPunchesY;
            this.tool = tool;
            this.selected = selected;            
        }

        public override string getParameterDescription()
        {
            return String.Format("(GRIDY, {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7})",
                                                        xStart,
                                                        yStart,
                                                        hitsDistanceX,
                                                        hitsDistanceY,
                                                        numberOfPunchesX,
                                                        numberOfPunchesY,
                                                        tool.toolName,
                                                        tool.toolDimensions
                                                        );
        }

        public override void Draw(ref Canvas mainCanvas)
        {
            Canvas shapeCanvas = HelperClass.CreateCanvas(xStart, yStart);
            double hitsDistanceYDir = hitsDistanceY;

            Point punchPoint = new Point();
            punchPoint.X = 0;
            punchPoint.Y = 0;

            for (int i = 0; i < numberOfPunchesX; i++)
            {
                for (int j = 0; j < numberOfPunchesY; j++)
                {
                    shapeCanvas.Children.Add(tool.GetToolShape(punchPoint, selected));

                    if (j < numberOfPunchesY - 1)
                        punchPoint.Y += hitsDistanceYDir;
                }
                hitsDistanceYDir *= -1.0;
                punchPoint.X += hitsDistanceX;
            }

            mainCanvas.Children.Add(shapeCanvas);
        }

        public override void Punch(SinglePuncher singlePuncher)
        {
            SinglePuncher _singlePuncher = singlePuncher;

            double hitsDistanceYDir = hitsDistanceY;

            Point punchPoint = new Point(0, 0);

            punchPoint.Offset(xStart, yStart);

            for (int i = 0; i < numberOfPunchesX; i++)
            {
                for (int j = 0; j < numberOfPunchesY; j++)
                {

                    //insertar aqui el metodo para posicionar ejes
                    
                    _singlePuncher.MoveCommand(punchPoint);

                    if (j < numberOfPunchesY - 1)
                        punchPoint.Y += hitsDistanceYDir;
                }
                hitsDistanceYDir *= -1.0;
                punchPoint.X += hitsDistanceX;
            }
        }
    }
}
