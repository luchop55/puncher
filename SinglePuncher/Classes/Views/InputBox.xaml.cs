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

namespace SinglePuncher.Classes.Views
{
    /// <summary>
    /// Lógica de interacción para InputBox.xaml
    /// </summary>
    public partial class InputBox : Window
    {

        public double value { get; private set; }

        public InputBox(string message)
        {
            InitializeComponent();
            ValueTextBox.Focus();
            messageTextBlock.Text = message;
            value = 0;
        }

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                value = Convert.ToDouble(ValueTextBox.Text);
            }
            catch
            {
                value = 0;
            }

            this.Close();
        }

        private void ValueTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            tb.SelectAll();
        }

        private void ValueTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                AcceptButton.Focus();
        }
    }
}
