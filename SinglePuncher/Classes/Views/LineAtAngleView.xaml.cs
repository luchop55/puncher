using SinglePuncher.Classes.GCodes;
using SinglePuncher.Classes.Tools;
using System.Windows;
using System.Windows.Controls;
using WpfApplication1.Classes.GCommands;

namespace SinglePuncher.Classes.Views
{
    /// <summary>
    /// Interaction logic for LineAtAngle.xaml
    /// </summary>
    public partial class LineAtAngleView : Window
    {
        private LineAtAngle lineAtAngle;
        private double xStart;
        private double yStart;
        private double hitsDistance;
        private double angle;        
        private Size toolSize;
        private Tool tool;
        private int numberOfPunches;

        public LineAtAngleView()
        {
            InitializeComponent();
        }
        
        public LineAtAngle ShowWindow()
        {
            xStartTextBlock.Focus();
            this.ShowDialog();
            return lineAtAngle;
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
            xStart = HelperClass.ConvertToDouble(xStartTextBlock.Text);
            yStart = HelperClass.ConvertToDouble(yStartTextBlock.Text);
            hitsDistance = HelperClass.ConvertToDouble(hitsDistanceTextBlock.Text);
            angle = HelperClass.ConvertToDouble(angleTextBlock.Text);
            numberOfPunches = HelperClass.ConvertToInt(numberOfPunchesTextBlock.Text);
            toolSize = new Size(HelperClass.ConvertToDouble(toolWidthTextBox.Text), HelperClass.ConvertToDouble(toolHeightTextBox.Text));
            tool = HelperClass.GetToolFromComboBox(cmbTool, toolSize);

            lineAtAngle = new LineAtAngle(xStart, yStart, hitsDistance, angle, numberOfPunches, tool, false);

            this.Close();
        }

        private void TextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            HelperClass.GoToNextControl(e);
        }
    }
}

