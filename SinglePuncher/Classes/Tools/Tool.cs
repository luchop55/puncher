using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace SinglePuncher.Classes.Tools
{
    [Serializable]
    [XmlInclude(typeof(Celosy))]
    [XmlInclude(typeof(Hexagon))]
    [XmlInclude(typeof(Obround))]
    [XmlInclude(typeof(Rect))]
    [XmlInclude(typeof(Round))]
    [XmlInclude(typeof(Square))]

    public abstract class Tool
    {
        public Size size;    
        public String toolName;
        public String toolDimensions;        
        public int StrokeThickness = 1;       

        public abstract Shape GetToolShape(Point centerPoint, bool selected);        
    }
}
