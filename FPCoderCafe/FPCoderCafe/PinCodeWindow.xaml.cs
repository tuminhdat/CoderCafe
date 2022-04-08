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

namespace FPCoderCafe
{
    /// <summary>
    /// Interaction logic for PinCodeWindow.xaml
    /// </summary>
    public partial class PinCodeWindow : Window
    {
        public PinCodeWindow()
        {
            InitializeComponent();
            OKButton.Click += OKButtonClickEventHandler;
        }

        private void OKButtonClickEventHandler(object sender, RoutedEventArgs e)
        {
            DialogResult = PinCodePasswordBox.Password == Properties.Settings.Default.ManagerPinCode;
        }
    }
}
