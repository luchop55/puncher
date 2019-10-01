using SinglePuncher.Classes.GCodes;
using System.Windows;
using System.Windows.Controls;
using WpfApplication1.Classes.GCommands;
using SinglePuncher.Classes.Tools;

namespace SinglePuncher.Classes.Views
{
    /// <summary>
    /// Interaction logic for LinearContourView.xaml
    /// </summary>
    public partial class LinearContourView : Window
    {
        private LinearContour linearContour;
        private double xStart;
        private double yStart;
        private double hitsDistance;
        private double angle;
        private double lineLenght;
        private Size toolSize;
        private Tool tool;

        public LinearContourView()
        {
            InitializeComponent();
        }

        public LinearContour ShowWindow()
        {
            xStartTextBlock.Focus();
            this.ShowDialog();
            return linearContour;
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
            yStart= HelperClass.ConvertToDouble(yStartTextBlock.Text);
            hitsDistance = HelperClass.ConvertToDouble(hitsDistanceTextBlock.Text);
            angle = HelperClass.ConvertToDouble(angleTextBlock.Text);
            lineLenght = HelperClass.ConvertToDouble(lineLenghtTextBlock.Text);
            toolSize = new Size(HelperClass.ConvertToDouble(toolWidthTextBox.Text), HelperClass.ConvertToDouble(toolHeightTextBox.Text));
            tool = HelperClass.GetToolFromComboBox(cmbTool, toolSize);

            linearContour = new LinearContour(xStart, yStart, hitsDistance, angle, lineLenght, tool, false);

            this.Close();
        }

        private void TextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            HelperClass.GoToNextControl(e);
        }
    }
}
