using Microsoft.Win32;
using SinglePuncher.Classes.GCodes;
using SinglePuncher.Classes.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace SinglePuncher.Classes
{
    public static class FileManager
    {
        private static void LoadGCodesInGCodeView(GCodeObject gCodeObject, ref ListBox GCodeView)
        {
            foreach (var items in gCodeObject.gCodeList)
            {
                GCodeView.Items.Add(items.parameterDescription);
            }
        }

        public static void SerializeFile(GCodeObject gCodeObject, String fileName)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(GCodeObject));

            StreamWriter stream = new StreamWriter(fileName);
            try { serializer.Serialize(stream, gCodeObject); }
            catch (Exception ex)
            {
                String excepcion = ex.InnerException.Message;
                String tipo = ex.GetType().FullName;
                String fuente = ex.Source.ToString();
            }

            stream.Close();
        }
        

        public static void DeserializeFile(ref GCodeObject gCodeObject, String fileName)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(GCodeObject));

            FileStream stream = File.OpenRead(fileName);
            try
            {
                gCodeObject = (GCodeObject)deserializer.Deserialize(stream);
            }
            catch (Exception ex)
            {
                string error = ex.InnerException.ToString();
            }

            stream.Close();
        }

        public static GCodeObject OpenFile(ref Canvas MainCanvas, ref ScaleTransform canvasScaleTrans, 
            ref ListBox GCodeView, ref System.Windows.Shapes.Path Arrow, Line xMeasure, Line yMeasure)
        {
            GCodeObject gCodeObject = new GCodeObject();
            OpenFileDialog cuadroDialogo = new OpenFileDialog();
            String rutaArchivo = "";


            cuadroDialogo.Filter = "xml Files (*.xml)|*.xml";
            cuadroDialogo.FilterIndex = 1;
            cuadroDialogo.Multiselect = true;

            cuadroDialogo.ShowDialog();

            if (cuadroDialogo.FileName != "" && File.Exists(cuadroDialogo.FileName))
            {
                rutaArchivo = cuadroDialogo.FileName;

                DeserializeFile(ref gCodeObject, rutaArchivo);

                gCodeObject.DrawObjects(ref MainCanvas, ref canvasScaleTrans, ref GCodeView, ref Arrow, xMeasure, yMeasure);                

                return gCodeObject;
            }
            else
            {
                MessageBox.Show("No se ha cargado ningún Archivo");
                return null;
            }
        }      


        public static void SaveFile(GCodeObject gCodeObject)
        {
            SaveFileDialog cuadroDialogo = new SaveFileDialog();
            string rutaArchivo = "";

            cuadroDialogo.Filter = "xml Files (*.xml)|*.xml";
            cuadroDialogo.FilterIndex = 1;

            cuadroDialogo.ShowDialog();

            if (cuadroDialogo.FileName != "")
            {
                rutaArchivo = cuadroDialogo.FileName;

                SerializeFile(gCodeObject, rutaArchivo);
            }
        }

        public static void DrawGcodeShapes(GCodeObject gCodeObject, ref ListBox GCodeView, ref Canvas MainCanvas)
        {
            MainCanvas.Width = gCodeObject.sheetSize.Width;
            MainCanvas.Height = gCodeObject.sheetSize.Height;

            MainCanvas.Children.Clear();

            foreach(Gcode gCode in gCodeObject.gCodeList)
            {
                gCode.Draw(ref MainCanvas);
            }
        }
    }
}
