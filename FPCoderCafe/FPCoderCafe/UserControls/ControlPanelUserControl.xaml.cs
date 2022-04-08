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
                ReloadButton.Click += LoadSettingsEventHandler;
                SwitchModeButton.Click += SwitchModeEventHandler;
                CustomerInfoDataGrid.SelectionChanged += DisplayUserInfo;
                UpdateUserButton.Click += UpdateCustomerInfo;
                DeleteUserButton.Click += DeleteUser;
                ResetPointButton.Click += ResetPoint;
            }
            else
            {
                SaveButton.Click -= SaveSettingEventHandler;
                ReloadButton.Click -= LoadSettingsEventHandler;
                SwitchModeButton.Click -= SwitchModeEventHandler;
                CustomerInfoDataGrid.SelectionChanged -= DisplayUserInfo;
                UpdateUserButton.Click -= UpdateCustomerInfo;
                DeleteUserButton.Click -= DeleteUser;
                ResetPointButton.Click += ResetPoint;
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
                MessageBox.Show(ErrorMessage, "Could not save settings");
                return;
            }
            //Save settings to app config
            Default.ManagerPinCode = pinCode;
            Default.Save();
            MessageBox.Show("Config saved to setting!");
        }
        public void LoadSettingsEventHandler(object o, EventArgs args)
        {
            LoadSettings();
            MessageBox.Show("Config reloaded from setting!");
        }

        public void SwitchModeEventHandler(object o, EventArgs args)
        {
            MainWindow.GetMainWindow().SwitchToFoodMenu();
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
            using (var ctx = new PointOfSaleContext())
            {
                customerList = ctx.Customers.Where(x => x.IsEnable).ToList();
                foreach (Customer c in customerList)
                {
                    CustomerInfoDataGrid.Items.Add(c);
                }
            }
        }
        private void DisplayUserInfo(object o, EventArgs e)
        {
            if (CustomerInfoDataGrid.SelectedItem != null)
            {
                //Get the selected customer
                Customer customer = (Customer)CustomerInfoDataGrid.SelectedItem;
                //Display customer's phone and redeem points
                CustomerPhoneText.Text = customer.Phone;
                RedeemPointText.Text = customer.RedeemPoint;
            }
        }

        private void ResetPoint(object o, EventArgs e)
        {
            using (var ctx = new PointOfSaleContext())
            {
                //Check if customer is selected
                if (CustomerInfoDataGrid.SelectedItem != null)
                {
                    //Get id from selected customer
                    Customer selectedCustomer = (Customer)CustomerInfoDataGrid.SelectedItem;
                    int idToUpdated = selectedCustomer.Id;

                    //Pull the repestive customer information for the id we have
                    Customer uc = ctx.Customers.Where(x => x.Id == idToUpdated).First();
                    //Reset the redeem point to 0
                    uc.RedeemPoint = "0";
                    uc.Phone = CustomerPhoneText.Text;
                    //Update customer information to database
                    ctx.Customers.Update(uc);
                    ctx.SaveChanges();
                    //Update datagrid
                    PopulateUserInfoDataGrid();
                }
                else
                    MessageBox.Show("No customer available to reset point!");
                //Clear all the value dislay on text box and set value of slider to 0 
                resetAll();
            }
        }

        private void UpdateCustomerInfo(object o, EventArgs e)
        {
            using (var ctx = new PointOfSaleContext())
            {
                if (CustomerInfoDataGrid.SelectedItem != null)
                {
                    //Get the original point and added point from the text box 
                    int addedPoint = Convert.ToInt32(PointAdded.Text);
                    int originalPoint = Convert.ToInt32(RedeemPointText.Text);
                    int newPoint = addedPoint + originalPoint;

                    Customer selectedCustomer = (Customer)CustomerInfoDataGrid.SelectedItem;
                    int idToUpdated = selectedCustomer.Id;
                    Customer uc = ctx.Customers.Where(x => x.Id == idToUpdated).First();
                    uc.RedeemPoint = newPoint.ToString();
                    uc.Phone = CustomerPhoneText.Text;

                    ctx.Customers.Update(uc);
                    ctx.SaveChanges();
                    PopulateUserInfoDataGrid();
                    MessageBox.Show("You added " + addedPoint + " points.");
                }else
                    MessageBox.Show("No customer to update!");
                resetAll();
            }
        }

        private void DeleteUser(Object o, EventArgs e)
        {
            using (var ctx = new PointOfSaleContext())
            {
                if (CustomerInfoDataGrid.SelectedItem != null)
                {
                    Customer selectedCustomer = (Customer)CustomerInfoDataGrid.SelectedItem;
                    int idToUpdated = selectedCustomer.Id;

                    Customer uc = ctx.Customers.Where(x => x.Id == idToUpdated).First();
                    //Remove customer from the data grid by set the Isnable to false
                    uc.IsEnable = false;
                    ctx.Customers.Update(uc);
                    ctx.SaveChanges();
                    PopulateUserInfoDataGrid();
                }else
                    MessageBox.Show("No customer to delete!");
                resetAll();
            }
        }

        private void resetAll()
        {
            CustomerPhoneText.Text = "";
            RedeemPointText.Text = "";
            PointSlider.Value = 0;
        }
    }
}
