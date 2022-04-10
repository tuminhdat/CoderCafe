using FPCoderCafe.Entities;
using FPCoderCafe.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace FPCoderCafe.UserControls
{
    /// <summary>
    /// Interaction logic for FoodMenuUserControl.xaml
    /// </summary>
    public partial class FoodMenuUserControl : UserControl
    {
        private List<TempItem> tempItems = new List<TempItem>();

        private bool checkPointClick = false;

        private double currentAmountPay = 0;

        public FoodMenuUserControl()
        {
            InitializeComponent();

            ToggleEventHandlers(false);

            // make default that all grid will be invisible except the first grid
            SecondGrid.Visibility = Visibility.Collapsed;
            ThirdGrid.Visibility = Visibility.Collapsed;
            SignInGrid.Visibility = Visibility.Collapsed;
            SignUpGrid.Visibility = Visibility.Collapsed;
            PaymentGrid.Visibility = Visibility.Collapsed;
            PointGrid.Visibility = Visibility.Collapsed;

            // add data to category list box
            InitializeCategoryListBox();
            // initialize the column and binding data of item grid
            initializeDataGrid();

            ToggleEventHandlers(true);
        }

        // this function is for event handler
        private void ToggleEventHandlers(bool toggle)
        {
            if (toggle)
            {
                SecondGridBack.Click += BackButtonOfSecondGrid;
                ThirdGridBack.Click += BackButtonOfThirdGrid;
                CategoryListBox.SelectionChanged += CategorySelected;
                ProductListBox.SelectionChanged += ProductSelected;
                AddItemButton.Click += AddItemButtonClick;
                MakePayButton.Click += FinishPayButtonClick;
                DeleteButton.Click += DeleteItemButtonClick;
                SmallButton.Click += SmallButtonClick;
                MediumButton.Click += MediumButtonClick;
                LargeButton.Click += LargeButtonClick;
                DebitButton.Click += DebitButtonClick;
                CreditButton.Click += CreditButtonClick;
                PaymentButton.Click += BackButtonOfPayment;
                ItemDataGrid.SelectionChanged += EnableDeleteButton;
                LoadSignUpButton.Click += LoadSignUpFormClick;
                LoadSignInButton.Click += LoadLoginFormClick;
                SignInBackButton.Click += BackButtonOfSignIn;
                SignUpBackButton.Click += BackButtonOfSignUp;
                SignUpButton.Click += SignUpButtonClick;
                LoginButton.Click += LoginButtonClick;
                CashButton.Click += CashButtonClick;
                CancelButton.Click += CancelButtonClick;
                PointButton.Click += SavingPointClick;
                PayButton.Click += PayClick;
            }
            else
            {
                SecondGridBack.Click -= BackButtonOfSecondGrid;
                ThirdGridBack.Click -= BackButtonOfThirdGrid;
                CategoryListBox.SelectionChanged -= CategorySelected;
                ProductListBox.SelectionChanged -= ProductSelected;
                AddItemButton.Click -= AddItemButtonClick;
                MakePayButton.Click -= FinishPayButtonClick;
                DeleteButton.Click -= DeleteItemButtonClick;
                SmallButton.Click -= SmallButtonClick;
                MediumButton.Click -= MediumButtonClick;
                LargeButton.Click -= LargeButtonClick;
                DebitButton.Click -= DebitButtonClick;
                CreditButton.Click -= CreditButtonClick;
                PaymentButton.Click -= BackButtonOfPayment;
                ItemDataGrid.SelectionChanged -= EnableDeleteButton;
                LoadSignUpButton.Click -= LoadSignUpFormClick;
                LoadSignInButton.Click -= LoadLoginFormClick;
                SignInBackButton.Click -= BackButtonOfSignIn;
                SignUpBackButton.Click -= BackButtonOfSignUp;
                SignUpButton.Click -= SignUpButtonClick;
                LoginButton.Click -= LoginButtonClick;
                CashButton.Click -= CashButtonClick;
                CancelButton.Click -= CancelButtonClick;
                PointButton.Click -= SavingPointClick;
                PayButton.Click -= PayClick;
            }
        }

        // this fucntion is for when user click button back inside Products form
        private void BackButtonOfSecondGrid(object o, EventArgs e)
        {
            // deselect all item inside the category list box
            CategoryListBox.UnselectAll();
            // make the Product data grid to be invisible
            SecondGrid.Visibility = Visibility.Collapsed;
            // make the Category list box to be visible
            CategoryListBox.Visibility = Visibility.Visible;
        }

        // this fucntion is for when user click button back inside Item info form
        private void BackButtonOfThirdGrid(object o, EventArgs e)
        {
            // deselect all item inside the product list box
            ProductListBox.UnselectAll();
            // reset all fields inside Item form
            ResetThirdGrid();
            // make the Item form to be invisible
            ThirdGrid.Visibility = Visibility.Collapsed;
            // make the product form to be visible
            SecondGrid.Visibility = Visibility.Visible;
        }

        // this fucntion is for when user click button back inside payment form
        private void BackButtonOfPayment(object o, EventArgs e)
        {
            // make the button Pay to be active 
            MakePayButton.IsEnabled = true;
            // make the payment form to be invisible
            PaymentGrid.Visibility = Visibility.Collapsed;
            // make the point infor grid to be invisible
            PointGrid.Visibility = Visibility.Collapsed;
            // make the category list box to be visible
            CategoryListBox.Visibility = Visibility.Visible;
        }

        // this fucntion is for when user click button back inside signin form
        private void BackButtonOfSignIn(object o, EventArgs e)
        {
            // make the payment form to be visible and invisible sign in form
            PaymentGrid.Visibility = Visibility.Visible;
            SignInGrid.Visibility = Visibility.Collapsed;
        }

        // this fucntion is for when user click button back inside signup form
        private void BackButtonOfSignUp(object o, EventArgs e)
        {
            // reset all fields inside sign in form
            SignInPhoneTextBox.Text = "";
            SignInPinTextBox.Password = "";
            // make the signin form to be visible and invisible sign up form
            SignInGrid.Visibility = Visibility.Visible;
            SignUpGrid.Visibility = Visibility.Collapsed;
        }

        // this function si for reset all fields inside Item info form
        private void ResetThirdGrid()
        {
            PlaceProductId.Text = "";
            ItemImage.Source = null;
            QuantityTextBox.Text = "";
            NoteTextBox.Text = "";
            PriceTextBlock.Text = "";
            SmallButton.Content = "";
            MediumButton.Content = "";
            LargeButton.Content = "";
            SmallButton.Background = Brushes.LightGray;
            MediumButton.Background = Brushes.LightGray;
            LargeButton.Background = Brushes.LightGray;
        }

        // this function is for when user select an item in category list box
        private void CategorySelected(object o, EventArgs e)
        {
            // get selected item
            Category getSelectedItem = (Category)CategoryListBox.SelectedItem;

            if (getSelectedItem != null)
            {
                using (var ctx = new PointOfSaleContext())
                {
                    // get all product which has the same category id
                    var getProductList = ctx.Products.Where(x => x.Category.Id == getSelectedItem.Id && x.IsEnabled).ToList();
                    // display all products into product list box
                    ProductListBox.ItemsSource = getProductList;
                }
            }
            // make category list box to be invisible and make visible of Product list
            CategoryListBox.Visibility = Visibility.Collapsed;
            SecondGrid.Visibility = Visibility.Visible;
        }

        // this function is for load the data from db into category list box
        private void InitializeCategoryListBox()
        {
            using (var ctx = new PointOfSaleContext())
            {
                // get all categories from db
                var getCategoryList = ctx.Categories.Where(x => x.IsEnable).ToList();
                // load all categories into category list box
                CategoryListBox.ItemsSource = getCategoryList;
            }
        }

        // this function is for when user select a product 
        private void ProductSelected(object o, EventArgs e)
        {
            // get the selected product
            Product getSelectedItem = (Product)ProductListBox.SelectedItem;
            // check if there is no product inside db
            if (getSelectedItem != null)
            {
                using (var ctx = new PointOfSaleContext())
                {
                    // get info of the product from db
                    var getProductItem = ctx.Products.Where(x => x.Id == getSelectedItem.Id).First();

                    // check if product has price for small size
                    if (getProductItem.SmallPrice == null || getProductItem.SmallPrice == 0)
                    {
                        SmallButton.IsEnabled = false;
                    } else
                    {
                        // if there is size, load price into button
                        SmallButton.IsEnabled = true;
                        SmallButton.Content = "Small: $" + getProductItem.SmallPrice;
                    }
                    // check if product has price for medium size
                    if (getProductItem.MediumPrice == null || getProductItem.MediumPrice == 0)
                    {
                        MediumButton.IsEnabled = false;
                    }
                    else
                    {
                        // if there is size, load price into button
                        MediumButton.IsEnabled = true;
                        MediumButton.Content = "Medium: $" + getProductItem.MediumPrice;
                    }
                    // check if product has price for large size
                    if (getProductItem.LargePrice == null || getProductItem.LargePrice == 0)
                    {
                        LargeButton.IsEnabled = false;
                    }
                    else
                    {
                        // if there is size, load price into button
                        LargeButton.IsEnabled = true;
                        LargeButton.Content = "Large: $" + getProductItem.LargePrice;
                    }
                    // load value into field
                    PlaceProductId.Text = getProductItem.Id.ToString();
                    ProductNameText.Content = getProductItem.Name;
                    ItemImage.Source = new BitmapImage(new Uri(getProductItem.FullImagePath));
                }
            }
            // make product list box invisible
            SecondGrid.Visibility = Visibility.Collapsed;
            // make the item info form to be visible
            ThirdGrid.Visibility = Visibility.Visible;
        }

        // initilize column and binding data
        private void initializeDataGrid()
        {
            DataGridTextColumn ProductIDColumn = new DataGridTextColumn();
            ProductIDColumn.Header = "ProductId";
            ProductIDColumn.Binding = new Binding("ProductId");
            ProductIDColumn.Visibility = Visibility.Collapsed;

            DataGridTextColumn ProductNameColumn = new DataGridTextColumn();
            ProductNameColumn.Header = "Item Name";
            ProductNameColumn.Binding = new Binding("ProductName");

            DataGridTextColumn QuantityColumn = new DataGridTextColumn();
            QuantityColumn.Header = "Quantity";
            QuantityColumn.Binding = new Binding("Quantity");

            DataGridTextColumn PriceColumn = new DataGridTextColumn();
            PriceColumn.Header = "Total";
            PriceColumn.Binding = new Binding("TotalPrice");

            DataGridTextColumn SizeColumn = new DataGridTextColumn();
            SizeColumn.Header = "Size";
            SizeColumn.Binding = new Binding("Size");

            DataGridTextColumn NoteColumn = new DataGridTextColumn();
            NoteColumn.Header = "Note";
            NoteColumn.Binding = new Binding("Note");

            ItemDataGrid.Columns.Add(ProductIDColumn);
            ItemDataGrid.Columns.Add(ProductNameColumn);
            ItemDataGrid.Columns.Add(QuantityColumn);
            ItemDataGrid.Columns.Add(PriceColumn);
            ItemDataGrid.Columns.Add(SizeColumn);
            ItemDataGrid.Columns.Add(NoteColumn);
        }

        // this funtion is for when user want to add item into cart
        private void AddItemButtonClick(object o, EventArgs e)
        {
            // check for user input. if no, return a message
            if (ValidateUserInput())
            {
                return;
            }
            // create new object of item
            TempItem newItem = new TempItem();
            // add all data from item form into property of item class
            newItem.ProductId = int.Parse(PlaceProductId.Text);
            newItem.ProductName = ProductNameText.Content.ToString();
            newItem.Quantity = int.Parse(QuantityTextBox.Text);
            if (SmallButton.Background == Brushes.LightBlue)
            {
                newItem.Size = Item.Size.Small.ToString();
            }
            else if (MediumButton.Background == Brushes.LightBlue)
            {
                newItem.Size = Item.Size.Medium.ToString();
            }
            else if (LargeButton.Background == Brushes.LightBlue)
            {
                newItem.Size = Item.Size.Large.ToString();
            }
            newItem.Note = NoteTextBox.Text;
            newItem.TotalPrice = Math.Round(double.Parse(PriceTextBlock.Text) * newItem.Quantity, 2);
            // add item into item list
            tempItems.Add(newItem);
            // add item into item grid
            ItemDataGrid.Items.Add(newItem);

            TotalPrice.Text = "$" + tempItems.Select(x => x.TotalPrice).Sum();
            // deselect all selected item in any form
            ProductListBox.UnselectAll();
            CategoryListBox.UnselectAll();
            // reset all fields of item form
            ResetThirdGrid();
            // reset visibility of all form
            ThirdGrid.Visibility = Visibility.Collapsed;
            SecondGrid.Visibility = Visibility.Collapsed;
            CategoryListBox.Visibility = Visibility.Visible;
        }

        // check if user input correctly
        private bool ValidateUserInput()
        {
            // check if user has selected any size, if no, return message
            if (SmallButton.Background == Brushes.LightGray &&
                MediumButton.Background == Brushes.LightGray &&
                LargeButton.Background == Brushes.LightGray)
            {
                MessageBox.Show("Please select a Size!");
                return true;
            }
            // check if user input any number of quantity
            if (QuantityTextBox.Text == "")
            {
                MessageBox.Show("Please enter Quantity of Item!");
                return true;
            }
            // check if user input for quantity is Cardinal number or not
            if (!Regex.IsMatch(QuantityTextBox.Text, @"^\d+$"))
            {
                MessageBox.Show("Please enter Cardinal Number for Quantity and more than 1");
                return true;
            }
            // check if user input 0 or lest
            if (int.Parse(QuantityTextBox.Text) <= 0)
            {
                MessageBox.Show("Please input more than 1 for Quantity of Item");
                return true;
            }
            // if there is no error, return false
            return false;
        }

        // this function is for when user want to make checkout
        private void FinishPayButtonClick(object o, EventArgs e)
        {
            // check if there is any item inside the list
            if (ItemDataGrid.Items.Count == 0)
            {
                MessageBox.Show("Sorry, You don't have any Item to checkout. Please add at least an Item");
                return;
            }

            if (LoadSignInButton.Visibility == Visibility.Collapsed)
            {
                PointButton.IsEnabled = true;
            } else
            {
                PointButton.IsEnabled = false;
            }

            if (PointButton.Background == Brushes.LightBlue && PointButton.IsEnabled == true)
            {
                DoCalculatePoint();
            }
            // reset all form
            MakePayButton.IsEnabled = false;
            DeleteButton.IsEnabled = false;
            ResetThirdGrid();
            ProductListBox.UnselectAll();
            CategoryListBox.UnselectAll();
            CategoryListBox.Visibility = Visibility.Collapsed;
            SecondGrid.Visibility = Visibility.Collapsed;
            ThirdGrid.Visibility = Visibility.Collapsed;
            PaymentGrid.Visibility = Visibility.Visible;
            // get the price after text
            double beforeTax = tempItems.Select(x => x.TotalPrice).Sum();
            double tax = beforeTax * 5 / 100;
            double afterTax = beforeTax + tax;
            // display data indo text box
            BeforeTaxTextBox.Text = "$" + Math.Round(beforeTax, 2).ToString();
            TaxTextBox.Text = "$" + Math.Round(tax, 2).ToString();
            AfterTaxTextBox.Text = "$" + Math.Round(afterTax, 2).ToString();
            currentAmountPay = Math.Round(afterTax, 2);
        }

        // this function is for when user want to delete an item inside the list
        private void DeleteItemButtonClick(object o, EventArgs e)
        {
            // get selected item 
            TempItem selectedItem = (TempItem)ItemDataGrid.SelectedItem;
            // remove fromt the grid
            ItemDataGrid.Items.Remove(ItemDataGrid.SelectedItem);
            // remove fromt the list
            tempItems.Remove(selectedItem);
            TotalPrice.Text = "$" + tempItems.Select(x => x.TotalPrice).Sum();
            DeleteButton.IsEnabled = false;
        }

        // this funciton is for when user wants to pick small size
        private void SmallButtonClick(object o, EventArgs e)
        {
            // change color of button small size
            SmallButton.Background = Brushes.LightBlue;
            MediumButton.Background = Brushes.LightGray;
            LargeButton.Background = Brushes.LightGray;

            int currentProductId = int.Parse(PlaceProductId.Text);
            using (var ctx = new PointOfSaleContext())
            {
                // get the selected item from product list box
                var getProductItem = ctx.Products.Where(x => x.Id == currentProductId).First();
                // display price
                PriceTextBlock.Text = getProductItem.SmallPrice.ToString();
            }
        }

        // this funciton is for when user wants to pick medium size - same thing with smallbuttonclick
        private void MediumButtonClick(object o, EventArgs e)
        {
            SmallButton.Background = Brushes.LightGray;
            MediumButton.Background = Brushes.LightBlue;
            LargeButton.Background = Brushes.LightGray;

            int currentProductId = int.Parse(PlaceProductId.Text);
            using (var ctx = new PointOfSaleContext())
            {
                var getProductItem = ctx.Products.Where(x => x.Id == currentProductId).First();
                PriceTextBlock.Text = getProductItem.MediumPrice.ToString();
            }
        }

        // this funciton is for when user wants to pick large size - same thing with smallbuttonclick
        private void LargeButtonClick(object o, EventArgs e)
        {
            SmallButton.Background = Brushes.LightGray;
            MediumButton.Background = Brushes.LightGray;
            LargeButton.Background = Brushes.LightBlue;
            int currentProductId = int.Parse(PlaceProductId.Text);
            using (var ctx = new PointOfSaleContext())
            {
                var getProductItem = ctx.Products.Where(x => x.Id == currentProductId).First();
                PriceTextBlock.Text = getProductItem.LargePrice.ToString();
            }
        }

        // this function is for when user want to pay by debit
        private void DebitButtonClick(object o, EventArgs e)
        {
            // change the color of the debit button
            DebitButton.Background = Brushes.LightBlue;
            CreditButton.Background = Brushes.LightGray;
            CashButton.Background = Brushes.LightGray;
        }

        // this function is for when user want to pay by credit
        private void CreditButtonClick(object o, EventArgs e)
        {
            DebitButton.Background = Brushes.LightGray;
            CreditButton.Background = Brushes.LightBlue;
            CashButton.Background = Brushes.LightGray;
        }

        // this function is for when user want to pay by cash
        private void CashButtonClick(object o, EventArgs e)
        {
            CashButton.Background = Brushes.LightBlue;
            DebitButton.Background = Brushes.LightGray;
            CreditButton.Background = Brushes.LightGray;
        }

        private void EnableDeleteButton(object o, EventArgs e)
        {
            if (PaymentGrid.Visibility == Visibility.Visible)
            {
                ItemDataGrid.UnselectAll();
            } else
            {
                DeleteButton.IsEnabled = true;
            }
        }

        // this function is for when user want to do sign in
        private void LoadLoginFormClick(object o, EventArgs e)
        {
            SignInPhoneTextBox.Text = "";
            SignInPinTextBox.Password = "";
            PaymentGrid.Visibility = Visibility.Collapsed;
            SignInGrid.Visibility = Visibility.Visible;
        }

        // this function is for when user want to do sign up
        private void LoadSignUpFormClick(object o, EventArgs e)
        {
            SignUpPhoneTextBox.Text = "";
            SignUpPinTextBox.Password = "";
            SignInGrid.Visibility = Visibility.Collapsed;
            SignUpGrid.Visibility = Visibility.Visible;
        }

        // this function is for when user finish input phone and pin and finally, do sign up
        private void SignUpButtonClick(object o, EventArgs e)
        {
            // recieve from user input
            string phone = SignUpPhoneTextBox.Text;
            string pin = SignUpPinTextBox.Password.ToString();

            // check if user input correctly
            if (phone == "" || !Regex.IsMatch(phone, @"^\d+$"))
            {
                MessageBox.Show("Please input your Phone number");
                return;
            }

            if (pin == "" || pin.Length < 4)
            {
                MessageBox.Show("Please input your Pin number and it should at least 4 characters");
                return;
            }

            using (var ctx = new PointOfSaleContext())
            {
                // check if user exist indb
                var checkExistPhone = ctx.Customers.Where(x => x.Phone.Equals(phone)).Count();

                if (checkExistPhone == 0)
                {
                    Customer newCustomer = new Customer();
                    newCustomer.Phone = phone;
                    newCustomer.BarCode = pin;
                    newCustomer.RedeemPoint = "0";
                    // create new customer in db
                    ctx.Customers.Add(newCustomer);
                    ctx.SaveChanges();

                    SignUpPhoneTextBox.Text = "";
                    SignUpPinTextBox.Password = "";

                    SignInGrid.Visibility = Visibility.Visible;
                    SignUpGrid.Visibility = Visibility.Collapsed;
                } else
                {
                    MessageBox.Show("You already created account with this phone number");
                    return;
                }
            } 
        }

        // this function is for when user finish input phone and pin and finally, do sign in
        private void LoginButtonClick(object o, EventArgs e)
        {
            // get user input
            string phone = SignInPhoneTextBox.Text;
            string pin = SignInPinTextBox.Password.ToString();

            using (var ctx = new PointOfSaleContext())
            {
                // check if user nput that matches in db
                var checkExistPhone = ctx.Customers.Where(x => x.Phone.Equals(phone) && x.BarCode.Equals(pin)).FirstOrDefault();

                if (checkExistPhone != null)
                {
                    UserPhoneNum.Content = checkExistPhone.Phone;
                    UserPoint.Content = checkExistPhone.RedeemPoint;

                    SignInPhoneTextBox.Text = "";
                    SignInPinTextBox.Password = "";

                    PointButton.IsEnabled = true;
                    SignInGrid.Visibility = Visibility.Collapsed;
                    PaymentGrid.Visibility = Visibility.Visible;
                    LoadSignInButton.Visibility = Visibility.Collapsed;
                    UserPhoneNum.Visibility = Visibility.Visible;
                    UserPoint.Visibility = Visibility.Visible;
                    PhoneLabel.Visibility = Visibility.Visible;
                    PointLabel.Visibility = Visibility.Visible;
                }
                else
                {
                    MessageBox.Show("You are either enter wrong phone number or pin");
                    return;
                }
            }
        }

        // this function is for when user wants to use point for payment
        private void SavingPointClick(object o, EventArgs e)
        {
            if (checkPointClick)
            {
                checkPointClick = !checkPointClick;

                PointButton.Background = Brushes.LightGray;

                using (var ctx = new PointOfSaleContext())
                {
                    var getUserByPhone = ctx.Customers.Where(x => x.Phone.Equals(UserPhoneNum.Content.ToString())).First();

                    UserPhoneNum.Content = getUserByPhone.Phone;
                    UserPoint.Content = getUserByPhone.RedeemPoint;
                }

                double beforeTax = tempItems.Select(x => x.TotalPrice).Sum();
                double tax = beforeTax * 5 / 100;
                double afterTax = beforeTax + tax;

                currentAmountPay = Math.Round(afterTax, 2);

                PointGrid.Visibility = Visibility.Collapsed;
            } else
            {
                checkPointClick = true;

                PointButton.Background = Brushes.LightBlue;

                DoCalculatePoint();
            }
        }

        // this function is for when user wants to cancel the payment and stop shopping
        private void CancelButtonClick(object o, EventArgs e)
        {
            ResetPayment();
        }

        private void DoCalculatePoint()
        {
            Customer getCustomer;

            using (var ctx = new PointOfSaleContext())
            {
                getCustomer = (Customer)ctx.Customers.Where(x => x.Phone == UserPhoneNum.Content.ToString()).First();
            }

            if (int.Parse(getCustomer.RedeemPoint) <= 0)
            {
                MessageBox.Show("Sorry, You don't have enough Point");
                checkPointClick = false;
                PointButton.Background = Brushes.LightGray;
                return;
            }

            double currentUserPoint = double.Parse(getCustomer.RedeemPoint);
            double currentSavingAmount = currentUserPoint / 10.0;
            double afterUsePoint = currentAmountPay - currentSavingAmount;

            if (afterUsePoint < 0)
            {
                UserPoint.Content = Math.Abs((int)afterUsePoint * 10).ToString();
                RemainAmount.Text = "$0";
                currentAmountPay = 0;
            }
            else
            {
                RemainAmount.Text = "$" + Math.Round(afterUsePoint, 2).ToString();
                currentAmountPay = Math.Round(afterUsePoint, 2);
            }

            PointGrid.Visibility = Visibility.Visible;
        }

        // reset all fields in every form
        private void ResetPayment()
        {
            currentAmountPay = 0;
            ItemDataGrid.Items.Clear();
            LoadSignInButton.Visibility = Visibility.Visible;
            PhoneLabel.Visibility = Visibility.Collapsed;
            PointLabel.Visibility = Visibility.Collapsed;
            UserPhoneNum.Content = "";
            UserPoint.Content = "";
            TotalPrice.Text = "";
            CashButton.Background = Brushes.LightGray;
            DebitButton.Background = Brushes.LightGray;
            CreditButton.Background = Brushes.LightGray;
            PointButton.Background = Brushes.LightGray;
            BeforeTaxTextBox.Text = "";
            TaxTextBox.Text = "";
            AfterTaxTextBox.Text = "";
            RemainAmount.Text = "";
            PaymentGrid.Visibility = Visibility.Collapsed;
            CategoryListBox.Visibility = Visibility.Visible;
            PointGrid.Visibility = Visibility.Collapsed;
            MakePayButton.IsEnabled = true;

        }

        // this function is for when user want to make payment
        private void PayClick(object o, EventArgs e)
        {
            // if user doesn't choose any method to pay, display message
            if (PointButton.Background == Brushes.LightGray &&
                CashButton.Background == Brushes.LightGray &&
                DebitButton.Background == Brushes.LightGray &&
                CreditButton.Background == Brushes.LightGray)
            {
                MessageBox.Show("Please select a method to pay");
                return;
            }

            List<Payment> payments = new List<Payment>();
            // create new object of Order
            Order nOrder = new Order();

            // user uses point to pay
            if (PointButton.Background == Brushes.LightBlue)
            {
                using (var ctx = new PointOfSaleContext())
                {
                    Customer getCustomer = (Customer)ctx.Customers.Where(x => x.Phone.Equals(UserPhoneNum.Content.ToString())).First();
                    getCustomer.RedeemPoint = UserPoint.Content.ToString();
                    ctx.Customers.Update(getCustomer);
                    Payment nPayment = new Payment();
                    nPayment.Type = Payment.PaymentType.Point;
                    nPayment.Amount = (Decimal.Parse(getCustomer.RedeemPoint) - Decimal.Parse(UserPoint.Content.ToString()))/10;
                    nPayment.CreateTime = DateTime.Now;
                    payments.Add(nPayment);
                    ctx.SaveChanges();
                    nOrder.CustomerId = getCustomer.Id;
                }
            }

            // after use point and there is remaining amount that need to be pay, use one of the following method
            if (currentAmountPay > 0)
            {
                // if user doesn't use any method to pay except point, return message
                if (CashButton.Background == Brushes.LightGray &&
                    DebitButton.Background == Brushes.LightGray &&
                    CreditButton.Background == Brushes.LightGray)
                {
                    MessageBox.Show("Please select a method to pay because your point is not enought.");
                    return;
                }
                // user pay by cash
                if (CashButton.Background == Brushes.LightBlue)
                {
                    //Calculate total amount
                    var totalAmount = (Decimal)currentAmountPay;

                    //Create and show cash tethering window dialog
                    var cashDialog = new CashWindow(totalAmount);
                    cashDialog.Owner = Window.GetWindow(this);
                    var result = cashDialog.ShowDialog();

                    //Handle dialog result
                    if (result == true)
                    {
                        var cashAmount = cashDialog.TetheredAmount;
                        var refundAmount = cashDialog.RefundAmount;
                        MessageBox.Show($"Inserted cash: {cashAmount}\nRefund amount: {refundAmount}");
                    }

                    Payment nPayment = new Payment();
                    nPayment.Type = Payment.PaymentType.Cash;
                    nPayment.Amount = (Decimal)currentAmountPay;
                    nPayment.CreateTime = DateTime.Now;
                    payments.Add(nPayment);
                }
                // if user pay by debit
                else if (DebitButton.Background == Brushes.LightBlue)
                {
                    Payment nPayment = new Payment();
                    nPayment.Type = Payment.PaymentType.Debit;
                    nPayment.Amount = (Decimal)currentAmountPay;
                    nPayment.CreateTime = DateTime.Now;
                    payments.Add(nPayment);
                }
                // if user pay by credit
                else if (CreditButton.Background == Brushes.LightBlue)
                {
                    Payment nPayment = new Payment();
                    nPayment.Type = Payment.PaymentType.Credit;
                    nPayment.Amount = (Decimal)currentAmountPay;
                    nPayment.CreateTime = DateTime.Now;
                    payments.Add(nPayment);
                }
            }
            // update, create db
            using (var ctx = new PointOfSaleContext())
            {
                List<Item> items = new List<Item>();
                nOrder.CreateTime = DateTime.Now;
                var currentOrderNumber = ctx.Orders.Select(x => x.OrderNumber).ToList();
                nOrder.OrderNumber = currentOrderNumber.LastOrDefault() + 1;
                nOrder.Description = "";
                foreach (TempItem tempItem in tempItems)
                {
                    Item nItem = new Item();
                    nItem.Quantity = tempItem.Quantity;
                    nItem.Amount = (Decimal)tempItem.TotalPrice;
                    nItem.ItemSize = (Item.Size)Enum.Parse(typeof(Item.Size), tempItem.Size);
                    nItem.Note = tempItem.Note;
                    nItem.ProductId = tempItem.ProductId;
                    nItem.TaxAmount = (Decimal)tempItem.TotalPrice * 5 / 100;
                    items.Add(nItem);
                }
                nOrder.Items = items;
                nOrder.Payments = payments;
                // add Order into db
                ctx.Orders.Add(nOrder);
                ctx.SaveChanges();
            }
            // reset everything
            ResetPayment();
        }

        public class TempItem
        {
            public int ProductId { get; set; }
            public string ProductName { get; set; }
            public int Quantity { get; set; }
            public string Size { get; set; }
            public string Note { get; set; }
            public double TotalPrice { get; set; }
            public TempItem()
            {

            }
        }
    }
}
