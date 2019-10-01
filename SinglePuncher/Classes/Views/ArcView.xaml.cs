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
using SinglePuncher.Classes.Tools;
using WpfApplication1.Classes.GCommands;
using SinglePuncher.Classes.GCodes;

namespace SinglePuncher.Classes.Views
{
    /// <summary>
    /// Interaction logic for ArcView.xaml
    /// </summary>
    public partial class ArcView : Window
    {
        private double xCenter;
        private double yCenter;
        private double arcRadius;
        private double startingAngle;
        private double angleBetweenHits;
        private int numberOfPunches;
        private Tool tool;
        private Size toolSize;
        private Arc arc;

        public ArcView()
        {
            InitializeComponent();
        }

        public Arc ShowWindow()
        {
            xCenterTextBox.Focus();

            this.ShowDialog();            
            return arc;           
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
            if(toolHeightTextBox != null)
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
            angleBetweenHits = HelperClass.ConvertToDouble(hitsAngleTextBox.Text);
            numberOfPunches = HelperClass.ConvertToInt(numberOfHolesTexBox.Text);
            toolSize = new Size(HelperClass.ConvertToDouble(toolWidthTextBox.Text), HelperClass.ConvertToDouble(toolHeightTextBox.Text));
            tool = HelperClass.GetToolFromComboBox(cmbTool, toolSize);

            arc = new Arc(xCenter, yCenter, arcRadius, startingAngle, angleBetweenHits, numberOfPunches, tool, false);

            this.Close();
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            HelperClass.GoToNextControl(e);            
        }
    }
}
