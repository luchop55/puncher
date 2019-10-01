using SinglePuncher.Classes.Tools;
using System;
using System.Windows;
using System.Windows.Controls;
using WpfApplication1.Classes.GCommands;

namespace SinglePuncher.Classes.GCodes
{
    [Serializable]
    public class RecWinF : Gcode
    {
        public double overlap { get; set; }
        public double xLenght { get; set; }        
        public double yLenght { get; set; }        
        Size sheetSize = MainWindow.SheetSize;

        public RecWinF()
        {

        }


        public RecWinF(double xStart, double yStart, double xLenght,
            double yLenght, double overlap, Tool tool, bool selected)
        {
            this.xStart = xStart;
            this.yStart = yStart;
            this.xLenght = xLenght;
            this.yLenght = yLenght;
            this.overlap = overlap;
            this.tool = tool;
            this.selected = selected;            
        }

        public override string getParameterDescription()
        {
            return String.Format("(REC WIN F {0}, {1}, {2}, {3}, {4}, {5}, {6})",
                                                        xStart,
                                                        yStart,
                                                        xLenght,
                                                        yLenght,
                                                        overlap,
                                                        tool.toolName,
                                                        tool.toolDimensions
                                                        );
        }

        public override void Draw(ref Canvas mainCanvas)
        {       
            if (!(tool.size.Width > 0)) { return; }

            if (!(tool.size.Height > 0)) { return; }

            double distanceX = tool.size.Width * (1 - (overlap / 100));
            double distanceY = tool.size.Height * (1 - (overlap / 100));

            int numberOfPunchesX = Convert.ToInt32(Math.Ceiling(xLenght / distanceX));

            double xDistanceFixed = (xLenght - tool.size.Width) / Convert.ToDouble(numberOfPunchesX -1);

            int numberOfPunchesY = Convert.ToInt32(Math.Ceiling(yLenght / distanceY));
            double yDistanceFixed = (yLenght - tool.size.Height) / Convert.ToDouble(numberOfPunchesY - 1);

            Canvas shapeCanvas = HelperClass.CreateCanvas(xStart, yStart);

            Point punchPoint = new Point(0,0);

            punchPoint.X += tool.size.Width / 2;
            punchPoint.Y += tool.size.Height / 2;


            for (int i = 0; i < numberOfPunchesY; i++)
            {
                for (int j = 0; j < numberOfPunchesX; j++)
                {
                    shapeCanvas.Children.Add(tool.GetToolShape(punchPoint, selected));

                    if (j == (numberOfPunchesX -1))
                        break;

                    punchPoint.X += xDistanceFixed;
                }

                xDistanceFixed *= -1;                

                punchPoint.Y += yDistanceFixed;
            }

            mainCanvas.Children.Add(shapeCanvas);
        }

        public override void Punch(SinglePuncher singlePuncher)
        {
            SinglePuncher _singlePuncher = singlePuncher;            

            if (!(tool.size.Width > 0)) { return; }

            if (!(tool.size.Height > 0)) { return; }

            double distanceX = tool.size.Width * (1 - (overlap / 100));
            double distanceY = tool.size.Height * (1 - (overlap / 100));

            int numberOfPunchesX = Convert.ToInt32(Math.Ceiling(xLenght / distanceX));

            double xDistanceFixed = (xLenght - tool.size.Width) / Convert.ToDouble(numberOfPunchesX - 1);

            int numberOfPunchesY = Convert.ToInt32(Math.Ceiling(yLenght / distanceY));
            double yDistanceFixed = (yLenght - tool.size.Height) / Convert.ToDouble(numberOfPunchesY - 1);            

            Point punchPoint = new Point(xStart, yStart);

            punchPoint.X += tool.size.Width / 2;
            punchPoint.Y += tool.size.Height / 2;


            for (int i = 0; i < numberOfPunchesY; i++)
            {
                for (int j = 0; j < numberOfPunchesX; j++)
                {                    
                    _singlePuncher.MoveCommand(punchPoint);

                    if (j == (numberOfPunchesX - 1))
                        break;

                    punchPoint.X += xDistanceFixed;
                }

                xDistanceFixed *= -1;

                punchPoint.Y += yDistanceFixed;
            }         
        }
    }
}
