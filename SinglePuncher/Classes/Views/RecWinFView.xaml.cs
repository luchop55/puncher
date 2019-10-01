using SinglePuncher.Classes.GCodes;
using SinglePuncher.Classes.Tools;
using System.Windows;
using System.Windows.Controls;
using WpfApplication1.Classes.GCommands;

namespace SinglePuncher.Classes.Views
{
    /// <summary>
    /// Interaction logic for RecWinFView.xaml
    /// </summary>
    public partial class RecWinFView : Window
    {
        private RecWinF recWinF;
        private double xStart;
        private double yStart;
        private double xLenght;
        private double angle;
        private double lineLenght;
        private Size toolSize;
        private Tool tool;
        private double overlap;
        private double yLenght;

        public RecWinFView()
        {
            InitializeComponent();
        }

        public RecWinF ShowWindow()
        {
            xStartTextBlock.Focus();
            this.ShowDialog();
            return recWinF;
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
            xLenght = HelperClass.ConvertToDouble(xLenghtTextBlock.Text);
            yLenght = HelperClass.ConvertToDouble(yLenghtTextBlock.Text);
            overlap = HelperClass.ConvertToDouble(overlapTextBlock.Text);
            toolSize = new Size(HelperClass.ConvertToDouble(toolWidthTextBox.Text), HelperClass.ConvertToDouble(toolHeightTextBox.Text));
            tool = HelperClass.GetToolFromComboBox(cmbTool, toolSize);

            recWinF = new RecWinF(xStart, yStart, xLenght, yLenght, overlap, tool, false);

            this.Close();
        }

        private void TextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            HelperClass.GoToNextControl(e);
        }
    }
}
