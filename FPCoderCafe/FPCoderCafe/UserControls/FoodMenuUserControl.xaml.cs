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

        private void ResetThirdGrid()
        {
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
            if (SmallButton.Background == Brushes.LightGray)
            {
                newItem.Size = Item.Size.Small.ToString();
            }
            else if (MediumButton.Background == Brushes.LightGray)
            {
                newItem.Size = Item.Size.Medium.ToString();
            }
            else if (LargeButton.Background == Brushes.LightGray)
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
            
        }

        private void DeleteItemButtonClick(object o, EventArgs e)
        {
            TempItem selectedItem = (TempItem)ItemDataGrid.SelectedItem;
            ItemDataGrid.Items.Remove(ItemDataGrid.SelectedItem);
            tempItems.Remove(selectedItem);
            TotalPrice.Text = "$" + tempItems.Select(x => x.TotalPrice).Sum();
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
                PriceTextBlock.Text = "$" + getProductItem.SmallPrice.ToString();
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
                PriceTextBlock.Text = "$" + getProductItem.MediumPrice.ToString();
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
                PriceTextBlock.Text = "$" + getProductItem.LargePrice.ToString();
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
