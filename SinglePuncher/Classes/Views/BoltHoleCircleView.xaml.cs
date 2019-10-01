using SinglePuncher.Classes.GCodes;
using SinglePuncher.Classes.Tools;
using System.Windows;
using System.Windows.Controls;
using WpfApplication1.Classes.GCommands;

namespace SinglePuncher.Classes.Views
{
    /// <summary>
    /// Interaction logic for BoltHoleCircleView.xaml
    /// </summary>
    public partial class BoltHoleCircleView : Window
    {

        private double xCenter;
        private double yCenter;
        private double radius;
        private double firstPunchAngle;        
        private int numberOfPunches;
        private Tool tool;
        private Size toolSize;
        private Arc arc;
        private BoltHoleCircle bhc;

        public BoltHoleCircleView()
        {
            InitializeComponent();
        }

        public BoltHoleCircle ShowWindow()
        {
            xCenterTextBox.Focus();

            this.ShowDialog();            
            return bhc;
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
            radius = HelperClass.ConvertToDouble(radiusTextBox.Text);
            firstPunchAngle = HelperClass.ConvertToDouble(firstPunchAngleTextBox.Text);            
            numberOfPunches = HelperClass.ConvertToInt(numberOfPunchesTextBox.Text);
            toolSize = new Size(HelperClass.ConvertToDouble(toolWidthTextBox.Text), HelperClass.ConvertToDouble(toolHeightTextBox.Text));
            tool = HelperClass.GetToolFromComboBox(cmbTool, toolSize);

            bhc = new BoltHoleCircle(xCenter, yCenter, radius, firstPunchAngle, numberOfPunches, tool, false);

            this.Close();
        }

        private void TextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            HelperClass.GoToNextControl(e);
        }
    }
}
