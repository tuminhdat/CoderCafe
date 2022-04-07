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

        public FoodMenuUserControl()
        {
            InitializeComponent();

            ToggleEventHandlers(false);

            SecondGrid.Visibility = Visibility.Collapsed;
            ThirdGrid.Visibility = Visibility.Collapsed;
            SignInGrid.Visibility = Visibility.Collapsed;
            SignUpGrid.Visibility = Visibility.Collapsed;
            PaymentGrid.Visibility = Visibility.Collapsed;

            InitializeCategoryListBox();
            initializeDataGrid();

            ToggleEventHandlers(true);
        }

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
            }
        }

        private void BackButtonOfSecondGrid(object o, EventArgs e)
        {
            CategoryListBox.UnselectAll();

            SecondGrid.Visibility = Visibility.Collapsed;

            CategoryListBox.Visibility = Visibility.Visible;
        }

        private void BackButtonOfThirdGrid(object o, EventArgs e)
        {
            ProductListBox.UnselectAll();

            ResetThirdGrid();

            ThirdGrid.Visibility = Visibility.Collapsed;

            SecondGrid.Visibility = Visibility.Visible;
        }

        private void BackButtonOfPayment(object o, EventArgs e)
        {
            PaymentGrid.Visibility = Visibility.Collapsed;

            CategoryListBox.Visibility = Visibility.Visible;
        }

        private void BackButtonOfSignIn(object o, EventArgs e)
        {
            PaymentGrid.Visibility = Visibility.Visible;
            SignInGrid.Visibility = Visibility.Collapsed;
        }

        private void BackButtonOfSignUp(object o, EventArgs e)
        {
            SignInGrid.Visibility = Visibility.Visible;
            SignUpGrid.Visibility = Visibility.Collapsed;
        }

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

        private void CategorySelected(object o, EventArgs e)
        {
            Category getSelectedItem = (Category)CategoryListBox.SelectedItem;

            if (getSelectedItem != null)
            {
                using (var ctx = new PointOfSaleContext())
                {
                    var getProductList = ctx.Products.Where(x => x.Category.Id == getSelectedItem.Id && x.IsEnabled).ToList();

                    ProductListBox.ItemsSource = getProductList;
                }
            }

            CategoryListBox.Visibility = Visibility.Collapsed;

            SecondGrid.Visibility = Visibility.Visible;
        }

        private void InitializeCategoryListBox()
        {
            using (var ctx = new PointOfSaleContext())
            {
                var getCategoryList = ctx.Categories.Where(x => x.IsEnable).ToList();

                CategoryListBox.ItemsSource = getCategoryList;
            }
        }

        private void ProductSelected(object o, EventArgs e)
        {
            Product getSelectedItem = (Product)ProductListBox.SelectedItem;

            if (getSelectedItem != null)
            {
                using (var ctx = new PointOfSaleContext())
                {
                    var getProductItem = ctx.Products.Where(x => x.Id == getSelectedItem.Id).First();

                    if (getProductItem.SmallPrice == null || getProductItem.SmallPrice == 0)
                    {
                        SmallButton.IsEnabled = false;
                    } else
                    {
                        SmallButton.IsEnabled = true;
                        SmallButton.Content = "Small: $" + getProductItem.SmallPrice;
                    }

                    if (getProductItem.MediumPrice == null || getProductItem.MediumPrice == 0)
                    {
                        MediumButton.IsEnabled = false;
                    }
                    else
                    {
                        MediumButton.IsEnabled = true;
                        MediumButton.Content = "Medium: $" + getProductItem.MediumPrice;
                    }

                    if (getProductItem.LargePrice == null || getProductItem.LargePrice == 0)
                    {
                        LargeButton.IsEnabled = false;
                    }
                    else
                    {
                        LargeButton.IsEnabled = true;
                        LargeButton.Content = "Large: $" + getProductItem.LargePrice;
                    }

                    PlaceProductId.Text = getProductItem.Id.ToString();
                    ProductNameText.Content = getProductItem.Name;
                    ItemImage.Source = new BitmapImage(new Uri(getProductItem.FullImagePath, UriKind.Relative));
                }
            }

            SecondGrid.Visibility = Visibility.Collapsed;

            ThirdGrid.Visibility = Visibility.Visible;
        }

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

        private void AddItemButtonClick(object o, EventArgs e)
        {
            if (ValidateUserInput())
            {
                return;
            }

            TempItem newItem = new TempItem();

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

            tempItems.Add(newItem);

            ItemDataGrid.Items.Add(newItem);

            TotalPrice.Text = "$" + tempItems.Select(x => x.TotalPrice).Sum();

            ProductListBox.UnselectAll();
            CategoryListBox.UnselectAll();

            ResetThirdGrid();

            ThirdGrid.Visibility = Visibility.Collapsed;
            SecondGrid.Visibility = Visibility.Collapsed;
            CategoryListBox.Visibility = Visibility.Visible;
        }

        private bool ValidateUserInput()
        {
            if (SmallButton.Background == Brushes.LightGray &&
                MediumButton.Background == Brushes.LightGray &&
                LargeButton.Background == Brushes.LightGray)
            {
                MessageBox.Show("Please select a Size!");
                return true;
            }

            if (QuantityTextBox.Text == "")
            {
                MessageBox.Show("Please enter Quantity of Item!");
                return true;
            }

            if (!Regex.IsMatch(QuantityTextBox.Text, @"^\d+$"))
            {
                MessageBox.Show("Please enter Cardinal Number for Quantity and more than 1");
                return true;
            }

            if (int.Parse(QuantityTextBox.Text) <= 0)
            {
                MessageBox.Show("Please input more than 1 for Quantity of Item");
                return true;
            }

            return false;
        }

        private void FinishPayButtonClick(object o, EventArgs e)
        {
            if (ItemDataGrid.Items.Count == 0)
            {
                MessageBox.Show("Sorry, You don't have any Item to checkout. Please add at least an Item");
                return;
            }

            MakePayButton.IsEnabled = false;
            DeleteButton.IsEnabled = false;
            ResetThirdGrid();
            ProductListBox.UnselectAll();
            CategoryListBox.UnselectAll();
            CategoryListBox.Visibility = Visibility.Collapsed;
            SecondGrid.Visibility = Visibility.Collapsed;
            ThirdGrid.Visibility = Visibility.Collapsed;
            PaymentGrid.Visibility = Visibility.Visible;

            double beforeTax = tempItems.Select(x => x.TotalPrice).Sum();
            double tax = beforeTax * 5 / 100;
            double afterTax = beforeTax + tax;

            BeforeTaxTextBox.Text = "$" + Math.Round(beforeTax, 2).ToString();
            TaxTextBox.Text = "$" + Math.Round(tax, 2).ToString();
            AfterTaxTextBox.Text = "$" + Math.Round(afterTax, 2).ToString();
        }

        private void DeleteItemButtonClick(object o, EventArgs e)
        {
            TempItem selectedItem = (TempItem)ItemDataGrid.SelectedItem;
            ItemDataGrid.Items.Remove(ItemDataGrid.SelectedItem);
            tempItems.Remove(selectedItem);
            TotalPrice.Text = "$" + tempItems.Select(x => x.TotalPrice).Sum();
            DeleteButton.IsEnabled = false;
        }

        private void SmallButtonClick(object o, EventArgs e)
        {
            SmallButton.Background = Brushes.LightBlue;
            MediumButton.Background = Brushes.LightGray;
            LargeButton.Background = Brushes.LightGray;

            int currentProductId = int.Parse(PlaceProductId.Text);
            using (var ctx = new PointOfSaleContext())
            {
                var getProductItem = ctx.Products.Where(x => x.Id == currentProductId).First();
                PriceTextBlock.Text = getProductItem.SmallPrice.ToString();
            }
        }

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

        private void DebitButtonClick(object o, EventArgs e)
        {
            DebitButton.Background = Brushes.LightBlue;
            CreditButton.Background = Brushes.LightGray;
        }

        private void CreditButtonClick(object o, EventArgs e)
        {
            DebitButton.Background = Brushes.LightGray;
            CreditButton.Background = Brushes.LightBlue;
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

        private void LoadLoginFormClick(object o, EventArgs e)
        {
            PaymentGrid.Visibility = Visibility.Collapsed;
            SignInGrid.Visibility = Visibility.Visible;
        }

        private void LoadSignUpFormClick(object o, EventArgs e)
        {
            SignInGrid.Visibility = Visibility.Collapsed;
            SignUpGrid.Visibility = Visibility.Visible;
        }

        private void SignUpButtonClick(object o, EventArgs e)
        {
            string phone = SignUpPhoneTextBox.Text;
            string pin = SignUpPinTextBox.Text;

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
                var checkExistPhone = ctx.Customers.Where(x => x.Phone.Equals(phone)).Count();

                if (checkExistPhone == 0)
                {
                    Customer newCustomer = new Customer();
                    newCustomer.Phone = phone;
                    newCustomer.BarCode = pin;
                    newCustomer.RedeemPoint = "0";

                    ctx.Customers.Add(newCustomer);
                    ctx.SaveChanges();

                    SignUpPhoneTextBox.Text = "";
                    SignUpPinTextBox.Text = "";

                    SignInGrid.Visibility = Visibility.Visible;
                    SignUpGrid.Visibility = Visibility.Collapsed;
                } else
                {
                    MessageBox.Show("You already created account with this phone number");
                    return;
                }
            } 
        }

        private void LoginButtonClick(object o, EventArgs e)
        {
            string phone = SignInPhoneTextBox.Text;
            string pin = SignInPinTextBox.Text;

            using (var ctx = new PointOfSaleContext())
            {
                var checkExistPhone = ctx.Customers.Where(x => x.Phone.Equals(phone) && x.BarCode.Equals(pin)).First();

                if (checkExistPhone != null)
                {
                    UserPhoneNum.Content = checkExistPhone.Phone;
                    UserPoint.Content = checkExistPhone.RedeemPoint;

                    SignInPhoneTextBox.Text = "";
                    SignInPinTextBox.Text = "";

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
