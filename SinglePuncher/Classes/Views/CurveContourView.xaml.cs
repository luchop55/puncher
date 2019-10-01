using SinglePuncher.Classes.GCodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfApplication1.Classes.GCommands;
using SinglePuncher.Classes.Tools;

namespace SinglePuncher.Classes.Views
{
    /// <summary>
    /// Interaction logic for CurveContourView.xaml
    /// </summary>
    public partial class CurveContourView : Window
    {
        private CurveContour curveContour;
        private double xCenter;
        private double yCenter;
        private double arcRadius;
        private double startingAngle;
        private double endingAngle;
        private double distanceBetweenHits;
        private Size toolSize;
        private Tool tool;

        public int P { get; private set; }

        public CurveContourView()
        {
            InitializeComponent();
        }       

        public CurveContour ShowWindow()
        {
            xCenterTextBox.Focus();
            this.ShowDialog();
            return curveContour;
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            tb.SelectAll();
        }

        private void cmbTool_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string toolStr = cmbTool.SelectedItem.ToString();
            toolStr = toolStr.Remove(0, 38);
            if (toolHeightTextBox != null)
            {
                if (toolStr == "ROUND" || toolStr == "SQUARE" || toolStr == "HEXAGON")
                    toolHeightTextBox.IsEnabled = false;
                else
                    toolHeightTextBox.IsEnabled = true;
            }
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            xCenter = HelperClass.ConvertToDouble(xCenterTextBox.Text);
            yCenter = HelperClass.ConvertToDouble(yCenterTextBox.Text);
            arcRadius = HelperClass.ConvertToDouble(arcRadiusTextBox.Text);
            startingAngle = HelperClass.ConvertToDouble(startingAngleTextBox.Text);
            endingAngle = HelperClass.ConvertToDouble(endingAngleTextBox.Text);

            if(pCombobox.Text == "Cen")
            {
                P = 0;
            }
            else if(pCombobox.Text == "Int")
            {
                P = -1;
            }
            else
            {
                P = 1;
            }
            distanceBetweenHits = HelperClass.ConvertToDouble(distanceBetweenHitsTexBox.Text);
            toolSize = new Size(HelperClass.ConvertToDouble(toolWidthTextBox.Text), HelperClass.ConvertToDouble(toolHeightTextBox.Text));
            tool = HelperClass.GetToolFromComboBox(cmbTool, toolSize);

            curveContour = new CurveContour(xCenter, yCenter, arcRadius, startingAngle, endingAngle, P, distanceBetweenHits, tool, false);

            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            HelperClass.GoToNextControl(e);
        }
    }
}
