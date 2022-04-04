using FPCoderCafe.Entities;
using FPCoderCafe.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace FPCoderCafe.UserControls
{
    /// <summary>
    /// Interaction logic for FoodMenuUserControl.xaml
    /// </summary>
    public partial class FoodMenuUserControl : UserControl
    {
        public FoodMenuUserControl()
        {


            InitializeComponent();

            Toggle(false);

            SecondGrid.Visibility = Visibility.Collapsed;
            ThirdGrid.Visibility = Visibility.Collapsed;

            InitializeCategoryListBox();

            Toggle(true);
        }

        private void Toggle(Boolean value)
        {
            if (value)
            {
                SecondGridBack.Click += BackButtonOfSecondGrid;
                ThirdGridBack.Click += BackButtonOfThirdGrid;
                CategoryListBox.SelectionChanged += CategorySelected;
                ProductListBox.SelectionChanged += ProductSelected;
                SmallSize.Checked += SizeChange;
                MediumSize.Checked += SizeChange;
                LargeSize.Checked += SizeChange;
            } else
            {
                SecondGridBack.Click -= BackButtonOfSecondGrid;
                ThirdGridBack.Click -= BackButtonOfThirdGrid; 
                CategoryListBox.SelectionChanged -= CategorySelected;
                ProductListBox.SelectionChanged -= ProductSelected;
                SmallSize.Checked -= SizeChange;
                MediumSize.Checked -= SizeChange;
                LargeSize.Checked -= SizeChange;
            }
        }

        private void BackButtonOfSecondGrid(object o, EventArgs e)
        {
            SecondGrid.Visibility = Visibility.Collapsed;

            CategoryListBox.Visibility = Visibility.Visible;

            CategoryListBox.UnselectAll();
        }

        private void BackButtonOfThirdGrid(object o, EventArgs e)
        {
            MessageBox.Show("Back was clicked!");

            ProductListBox.UnselectAll();

            ThirdGrid.Visibility = Visibility.Collapsed;

            SecondGrid.Visibility = Visibility.Visible;

            
        }

        private void CategorySelected(object o, EventArgs e)
        {
            Category getSelectedItem = (Category) CategoryListBox.SelectedItem;

            if (getSelectedItem != null)
            {
                using (var ctx = new PointOfSaleContext())
                {
                    var getProductList = ctx.Products.Where(x => x.Id == getSelectedItem.Id).ToList();

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
                var getCategoryList = ctx.Categories.ToList();

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

                    PlaceProductId.Text = getProductItem.Id.ToString();
                    ItemImage.Source = new BitmapImage(new Uri(getProductItem.FullImagePath, UriKind.Relative));
                    PriceTextBlock.Text = getProductItem.SmallPrice.ToString();
                }
            }

            SecondGrid.Visibility = Visibility.Collapsed;

            ThirdGrid.Visibility = Visibility.Visible;
        }

        private void SizeChange(object o, EventArgs e)
        {
            int currentProductId = int.Parse(PlaceProductId.Text);

            using (var ctx = new PointOfSaleContext())
            {
                var getProductItem = ctx.Products.Where(x => x.Id == currentProductId).First();

                if (SmallSize.IsChecked == true)
                {
                    PriceTextBlock.Text = getProductItem.SmallPrice.ToString();
                } else if (MediumSize.IsChecked == true)
                {
                    PriceTextBlock.Text = getProductItem.MediumPrice.ToString();
                } else if (LargeSize.IsChecked == true)
                {
                    PriceTextBlock.Text = getProductItem.LargePrice.ToString();
                }
            }
        }
    }
}
