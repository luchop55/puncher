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
    /// Interaction logic for GridXView.xaml
    /// </summary>
    public partial class GridXView : Window
    {
        private GridX gridX;
        private double xStart;
        private double yStart;
        private double hitsDistanceX;
        private int numberOfPunchesX;
        private double hitsDistanceY;
        private int numberOfPunchesY;
        private Size toolSize;
        private Tool tool;

        public GridXView()
        {
            InitializeComponent();
        }

        public GridX ShowWindow()
        {
            xStartTextBlock.Focus();
            this.ShowDialog();
            return gridX;
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
            numberOfPunchesX= HelperClass.ConvertToInt(numberOfPunchesXTextBlock.Text);
            hitsDistanceY = HelperClass.ConvertToDouble(hitsDistanceYTextBlock.Text);
            numberOfPunchesY = HelperClass.ConvertToInt(numberOfPunchesYTextBlock.Text);
            toolSize = new Size(HelperClass.ConvertToDouble(toolWidthTextBox.Text), HelperClass.ConvertToDouble(toolHeightTextBox.Text));
            tool = HelperClass.GetToolFromComboBox(cmbTool, toolSize);

            gridX = new GridX(xStart, yStart, hitsDistanceX, numberOfPunchesX, hitsDistanceY, numberOfPunchesY, tool, false);

            this.Close();
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            HelperClass.GoToNextControl(e);
        }
    }
}
