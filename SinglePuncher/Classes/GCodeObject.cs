using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace SinglePuncher.Classes
{
    [Serializable]
    [XmlInclude (typeof(MatrixTransform))]

    public class GCodeObject
    {
        public List<Gcode> gCodeList = new List<Gcode>();
        public Size sheetSize = new Size();
        public double canvasScale;

        public GCodeObject()
        {           
        }

        public void DrawObjects(ref Canvas MainCanvas, ref ScaleTransform canvasScaleTrans, ref ListBox GCodeView, 
            ref Path Arrow, Line xMeasure, Line yMeasure)
        {

            MainCanvas.Children.Clear();
            GCodeView.Items.Clear();
            
            Arrow.Width = 50 / canvasScale;
            Arrow.Height = 50 / canvasScale;

            MainCanvas.Children.Add(yMeasure);
            MainCanvas.Children.Add(xMeasure);
            MainCanvas.Children.Add(Arrow);

            MainCanvas.Width = sheetSize.Width;
            MainCanvas.Height = sheetSize.Height;

            canvasScaleTrans.ScaleX = canvasScale;
            canvasScaleTrans.ScaleY = canvasScale;
            
            foreach (Gcode gcode in gCodeList)
            {
                ListBoxItem gcodeItem = new ListBoxItem();
                TextBlock gcodeTextBlock = new TextBlock();
                gcodeTextBlock.Text = gcode.parameterDescription;
                gcodeItem.Content = gcodeTextBlock;
                gcode.Draw(ref MainCanvas);
                GCodeView.Items.Add(gcodeItem);
            }
        }

    }
}
