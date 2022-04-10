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
        private readonly FoodMenuUserControl foodMenuUserControl = new FoodMenuUserControl();
        private readonly ManagerUserControl managerUserControl = new ManagerUserControl();

        public MainWindow()
        {
            InitializeComponent();
            if (string.IsNullOrEmpty(Properties.Settings.Default.ManagerPinCode))
            {
                MainFrame.Content = managerUserControl;
            }
            else
            {
                MainFrame.Content = foodMenuUserControl;
            }
        }

        public void SwitchToFoodMenu()
        {
            MessageBoxButton buttons = MessageBoxButton.YesNo;
            var result = MessageBox.Show(
                "To switch back to manager mode:\n press [ESC] and enter the manager pincode ",
                "Do you want to switch to user mode?",
                buttons);

            //Set content to Food menu and reload menu data
            if (result == MessageBoxResult.Yes)
            {
                MainFrame.Content = foodMenuUserControl;
                foodMenuUserControl.InitializeCategoryListBox();
            }
        }

        public void EscCommandEventHandler(object sender, ExecutedRoutedEventArgs e)
        {
            //Only execute if user is in FoodMenu
            if (MainFrame.Content != foodMenuUserControl) return;
            var pinCodeWindow = new PinCodeWindow();
            pinCodeWindow.Owner = this;
            var result = pinCodeWindow.ShowDialog();
            if(result == true) MainFrame.Content = managerUserControl;
        }
    }
}
