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

namespace SinglePuncher.Classes.Views
{
    /// <summary>
    /// Interaction logic for SetSheetProperties.xaml
    /// </summary>
    public partial class SetSheetProperties : Window
    {
        public SetSheetProperties()
        {
            InitializeComponent();
            SheetWithTextBox.Focus();
        }

        public bool isCancel = true;
        public Size sheetSize;
        public double sheetTickness;

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            tb.SelectAll();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            isCancel = true;
            this.Close();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            sheetSize = new Size(HelperClass.ConvertToDouble(SheetWithTextBox.Text), HelperClass.ConvertToDouble(SheetHeightTextBox.Text));
            sheetTickness = HelperClass.ConvertToDouble(SheetTicknessTextBox.Text);

            isCancel = false;

            if (sheetSize.Width <= 0 || sheetSize.Height <= 0 || sheetTickness <= 0)
            {
                MessageBox.Show("Una o mas dimensiones de la plancha son erroneas, introduzca valores válidos",
                    "Error de datos introducidos");

                sheetSize.Height = 0;
                sheetSize.Width = 0;
                sheetTickness = 0;
            }
            else { this.Close(); }
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            HelperClass.GoToNextControl(e);
        }
    }
}
