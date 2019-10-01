using SinglePuncher.Classes.GCodes;
using SinglePuncher.Classes.Tools;
using System.Windows;
using System.Windows.Controls;
using WpfApplication1.Classes.GCommands;

namespace SinglePuncher.Classes.Views
{
    /// <summary>
    /// Interaction logic for GridYView.xaml
    /// </summary>
    public partial class GridYView : Window
    {
        private GridY gridY;
        private double xStart;
        private double yStart;
        private double hitsDistanceX;
        private int numberOfPunchesX;
        private double hitsDistanceY;
        private int numberOfPunchesY;
        private Size toolSize;
        private Tool tool;

        public GridYView()
        {
            InitializeComponent();
        }

        public GridY ShowWindow()
        {
            xStartTextBlock.Focus();
            this.ShowDialog();
            return gridY;
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
            hitsDistanceX = HelperClass.ConvertToDouble(hitsDistanceXTextBlock.Text);
            numberOfPunchesX = HelperClass.ConvertToInt(numberOfPunchesXTextBlock.Text);
            hitsDistanceY = HelperClass.ConvertToDouble(hitsDistanceYTextBlock.Text);
            numberOfPunchesY = HelperClass.ConvertToInt(numberOfPunchesYTextBlock.Text);
            toolSize = new Size(HelperClass.ConvertToDouble(toolWidthTextBox.Text), HelperClass.ConvertToDouble(toolHeightTextBox.Text));
            tool = HelperClass.GetToolFromComboBox(cmbTool, toolSize);

            gridY = new GridY(xStart, yStart, hitsDistanceX, numberOfPunchesX, hitsDistanceY, numberOfPunchesY, tool, false);

            this.Close();
        }

        private void TextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            HelperClass.GoToNextControl(e);
        }
    }
}

