using SinglePuncher.Classes.GCodes;
using SinglePuncher.Classes.Tools;
using System.Windows;
using System.Windows.Controls;
using WpfApplication1.Classes.GCommands;

namespace SinglePuncher.Classes.Views
{
    /// <summary>
    /// Interaction logic for SinglePunchView.xaml
    /// </summary>
    public partial class SinglePunchView : Window
    {
        private SinglePunch singlePunch;
        private double xCenter;
        private double yCenter;        
        private Size toolSize;
        private Tool tool;
        private double overlap;
        private double yLenght;

        public SinglePunchView()
        {
            InitializeComponent();
        }

        public SinglePunch ShowWindow()
        {
            xCenterTextBlock.Focus();
            this.ShowDialog();
            return singlePunch;
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
            xCenter = HelperClass.ConvertToDouble(xCenterTextBlock.Text);
            yCenter = HelperClass.ConvertToDouble(yCenterTextBlock.Text);           
            toolSize = new Size(HelperClass.ConvertToDouble(toolWidthTextBox.Text), HelperClass.ConvertToDouble(toolHeightTextBox.Text));
            tool = HelperClass.GetToolFromComboBox(cmbTool, toolSize);

            singlePunch = new SinglePunch(xCenter, yCenter, tool, false);

            this.Close();
        }

        private void TextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            HelperClass.GoToNextControl(e);
        }
    }
}
