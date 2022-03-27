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
using static FPCoderCafe.Properties.Settings;

namespace FPCoderCafe.UserControls
{
    /// <summary>
    /// Interaction logic for ConfigUserControl.xaml
    /// </summary>
    public partial class ControlPanelUserControl : UserControl
    {
        public ControlPanelUserControl()
        {
            InitializeComponent();
            LoadSettings();
            ToggleEventHandlers(true);
        }

        private void LoadSettings()
        {
            ManagerPinCode.Text = Default.ManagerPinCode;
        }

        public void ToggleEventHandlers(bool toggle)
        {
            if (toggle)
            {
                SaveButton.Click += SaveSettingEventHandler;
                SwitchModeButton.Click += SwitchModeEventHandler;
            }
            else
            {
                SaveButton.Click -= SaveSettingEventHandler;
                SwitchModeButton.Click -= SwitchModeEventHandler;
            }
        }

        public void SaveSettingEventHandler(object o, EventArgs args) 
        {
            //Validate input
            string ErrorMessage = string.Empty;
            var pinCode = ManagerPinCode.Text;
            if (string.IsNullOrWhiteSpace(pinCode))
            {
                ErrorMessage += "Pincode is invalid\n";
            }
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                MessageBox.Show(ErrorMessage,"Could not save settings");
                return;
            }
            //Save settings to app config
            Default.ManagerPinCode = pinCode;
            Default.Save();
        }

        public void SwitchModeEventHandler(object o, EventArgs args)
        {
            MainWindow.GetMainWindow().MainFrame.Content = new FoodMenuUserControl();
        }
    }
}
