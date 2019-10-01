using SinglePuncher.Classes.Tools;
using SinglePuncher.Classes.Views;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace WpfApplication1.Classes.GCommands
{
    public class HelperClass
    {
        public static double ShowInputBox(string Message)
        {
            double value = 0;

            InputBox myInputBox = new InputBox(Message);
            myInputBox.ShowDialog();

            value = myInputBox.value;

            return value;
        }

        public static void GoToNextControl(KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                KeyEventArgs eInsertBack = new KeyEventArgs(Keyboard.PrimaryDevice, Keyboard.PrimaryDevice.ActiveSource, 0, Key.Tab);
                eInsertBack.RoutedEvent = UIElement.KeyDownEvent;
                InputManager.Current.ProcessInput(eInsertBack);
            }
        }


        public static double DegreeToRadian(double angle)
        {
            return Math.PI * angle / 180.0;
        }

        public static double RadianToDegree(double angle)
        {
            return angle * 180 / Math.PI;
        }

        public static Point GetCircularCoordinates(double radius, double angle)
        {
            Point circularPoint = new Point();
            circularPoint.X = radius * Math.Cos(DegreeToRadian(angle));
            circularPoint.Y = radius * Math.Sin(DegreeToRadian(angle));

            return circularPoint;
        }

        public static Canvas CreateCanvas(double xPos, double yPos)
        {
            Canvas shapeCanvas = new Canvas();
            shapeCanvas.Width = 2;
            shapeCanvas.Height = 2;
            shapeCanvas.Background = Brushes.Red;
            shapeCanvas.Margin = new Thickness(xPos, yPos, -xPos, -yPos);
            return shapeCanvas;
        }       
               
        public static double ConvertToDouble(String strNum)
        {
            double dblNum = 0;

            try
            {
                dblNum = Convert.ToDouble(strNum);
            }
            catch
            {
                dblNum = 0;
            }
            return dblNum;
        }

        public static int ConvertToInt(String strNum)
        {
            int intNum = 0;

            try
            {
                intNum = Convert.ToInt32(strNum);
            }
            catch
            {
                intNum = 0;
            }
            return intNum;
        }


        public static Tool GetToolFromComboBox(ComboBox cmbTool, Size toolSize)
        {
            Tool tool;

            switch (cmbTool.Text)
            {
                case "ROUND":
                    tool = new Round(toolSize.Width);
                    break;
                case "SQUARE":
                    tool = new Square(toolSize.Width);
                    break;
                case "RECTANGLE":
                    tool = new SinglePuncher.Classes.Tools.Rect(toolSize);
                    break;
                case "OBROUND":
                    tool = new Obround(toolSize);
                    break;
                case "HEXAGON":
                    tool = new Hexagon(toolSize.Width);
                    break;
                case "CELOSY":
                    tool = new Celosy(toolSize);
                    break;
                default:
                    tool = null;
                    break;
            }
            return tool;                
        }

    }
}
