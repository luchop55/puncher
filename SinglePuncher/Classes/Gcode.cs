using SinglePuncher.Classes.GCodes;
using SinglePuncher.Classes.Tools;
using System;
using System.Windows.Controls;
using System.Xml.Serialization;

namespace SinglePuncher.Classes
{
    [Serializable]
    [XmlInclude (typeof(Arc))]
    [XmlInclude(typeof(BoltHoleCircle))]
    [XmlInclude(typeof(CurveContour))]
    [XmlInclude(typeof(GridX))]
    [XmlInclude(typeof(GridY))]
    [XmlInclude(typeof(LinearContour))]
    [XmlInclude(typeof(LineAtAngle))]
    [XmlInclude(typeof(RecWinF))]
    [XmlInclude(typeof(RecWinP))]
    [XmlInclude(typeof(SinglePunch))]

    public abstract class Gcode
    {       
        public abstract void Draw(ref Canvas mainCanvas);

        public abstract void Punch(SinglePuncher singlePuncher);

        public abstract string getParameterDescription();

        public String parameterDescription { get { return getParameterDescription();} set { }}

        public bool selected;

        public Tool tool;

        public double xStart { get; set; }
        public double yStart { get; set; }
    }
}
