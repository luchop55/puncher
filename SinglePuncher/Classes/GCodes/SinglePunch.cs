using SinglePuncher.Classes.Tools;
using System;
using System.Windows;
using System.Windows.Controls;
using WpfApplication1.Classes.GCommands;

namespace SinglePuncher.Classes.GCodes
{
    [Serializable]
    public class SinglePunch : Gcode
    {        
        Size sheetSize = MainWindow.SheetSize;

        public SinglePunch()
        {

        }

        public SinglePunch(double xCenter, double yCenter, Tool tool, bool selected)
        {
            this.xStart = xCenter;
            this.yStart = yCenter;
            this.tool = tool;
            this.selected = selected;            
        }


        public override string getParameterDescription()
        {
            return this.parameterDescription = String.Format("(SINGLE PUNCH, {0}, {1}, {2}, {3})",
                                                        xStart,
                                                        yStart,
                                                        tool.toolName,
                                                        tool.toolDimensions
                                                        );
        }

        public override void Draw(ref Canvas mainCanvas)
        {
            Canvas shapeCanvas = HelperClass.CreateCanvas(xStart, yStart);
            Point punchPoint = new Point(0, 0);
            shapeCanvas.Children.Add(tool.GetToolShape(punchPoint, selected));
            mainCanvas.Children.Add(shapeCanvas);
        }

        public override void Punch(SinglePuncher singlePuncher)
        {
            SinglePuncher _singlePuncher = singlePuncher;

            Point punchPoint = new Point(xStart, yStart);

            //insertar aqui el metodo para posicionar ejes

            _singlePuncher.MoveCommand(punchPoint);
        }
    }
}
