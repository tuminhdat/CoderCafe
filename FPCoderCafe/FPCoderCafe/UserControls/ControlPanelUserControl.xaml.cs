using FPCoderCafe.Entities;
using FPCoderCafe.Utilities;
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
        List<Customer> customerList = new List<Customer>();
        public ControlPanelUserControl()
        {
            InitializeComponent();
            LoadSettings();
            InititializeUserInfoDataGrid();
            PopulateUserInfoDataGrid();
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
                CustomerInfoDataGrid.SelectionChanged += DisplayUserInfo;
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

        private void InititializeUserInfoDataGrid()
        {
            //Create columns for the datagrid
            DataGridTextColumn PhoneColumn = new DataGridTextColumn();
            PhoneColumn.Header = "Customer Phone Number";
            PhoneColumn.Binding = new Binding("Phone");

            DataGridTextColumn RedeemPointColumn = new DataGridTextColumn();
            RedeemPointColumn.Header = "Redeem Point";
            RedeemPointColumn.Binding = new Binding("RedeemPoint");

            CustomerInfoDataGrid.Columns.Add(PhoneColumn);
            CustomerInfoDataGrid.Columns.Add(RedeemPointColumn);
        }

        private void PopulateUserInfoDataGrid()
        {
            CustomerInfoDataGrid.Items.Clear();
            using(var ctx = new PointOfSaleContext())
            {
                customerList = ctx.Customers.Where(x => x.IsEnable).ToList();
                foreach(Customer c in customerList)
                {
                    CustomerInfoDataGrid.Items.Add(c);
                }
            }
        }
        private void DisplayUserInfo(object o, EventArgs e)
        {
            Customer customer = (Customer)CustomerInfoDataGrid.SelectedItem;
            CustomerPhoneText.Text = customer.Phone;
            RedeemPointText.Text = customer.RedeemPoint;
        }
    }
}
