using FPCoderCafe.UserControls;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FPCoderCafe
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static MainWindow mainWindow;
        public static MainWindow GetMainWindow()
        {
            return mainWindow;
        }
        public MainWindow()
        {
            InitializeComponent();
            mainWindow = this;
            if (string.IsNullOrEmpty(Properties.Settings.Default.ManagerPinCode))
            {
                MainFrame.Content = new ManagerUserControl();

            }
            else
            {
                MainFrame.Content = new ManagerUserControl();
            }
        }
    }
}
