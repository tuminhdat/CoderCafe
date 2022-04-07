using FPCoderCafe.Entities;
using FPCoderCafe.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
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
    /// Interaction logic for FoodUserControl.xaml
    /// </summary>
    public partial class FoodUserControl : UserControl
    {
        private List<Product> Products;
        public FoodUserControl()
        {
            InitializeComponent();
            SetupCategory();
            SetupDataGridHeaders();
            UpdateDataGrid();
            ToggleEventhandlers(true);
        }

        private void SetupDataGridHeaders()
        {
            var productNameColumn = new DataGridTextColumn();
            productNameColumn.Header = "Product Name";
            productNameColumn.Binding = new Binding("Name");

            var smallPriceColumn = new DataGridTextColumn();
            smallPriceColumn.Header = "Small Price";
            smallPriceColumn.Binding = new Binding("SmallPrice");
            smallPriceColumn.Binding.StringFormat = "{0:C}";

            var mediumPriceColumn = new DataGridTextColumn();
            mediumPriceColumn.Header = "Medium Price";
            mediumPriceColumn.Binding = new Binding("MediumPrice");
            mediumPriceColumn.Binding.StringFormat = "{0:C}";

            var largePriceColumn = new DataGridTextColumn();
            largePriceColumn.Header = "Large Price";
            largePriceColumn.Binding = new Binding("LargePrice");
            largePriceColumn.Binding.StringFormat = "{0:C}";

            var categoryColumn = new DataGridTextColumn();
            categoryColumn.Header = "Category";
            categoryColumn.Binding = new Binding("Category.Name");

            var imageNameColumn = new DataGridTextColumn();
            imageNameColumn.Header = "Image Name";
            imageNameColumn.Binding = new Binding("ImageName");

            var descriptionColumn = new DataGridTextColumn();
            descriptionColumn.Header = "Description";
            descriptionColumn.Binding = new Binding("Description");

            FoodDataGrid.Columns.Clear();
            FoodDataGrid.Columns.Add(productNameColumn);
            FoodDataGrid.Columns.Add(smallPriceColumn);
            FoodDataGrid.Columns.Add(mediumPriceColumn);
            FoodDataGrid.Columns.Add(largePriceColumn);
            FoodDataGrid.Columns.Add(categoryColumn);
            FoodDataGrid.Columns.Add(imageNameColumn);
            FoodDataGrid.Columns.Add(descriptionColumn);
        }

        public void SetupCategory()
        {
            CategoryComboBox.Items.Clear();
            using (var context = new PointOfSaleContext())
            {
                var categories = context.Categories.Where(x => x.IsEnable).ToList();
                foreach (var category in categories)
                {
                    CategoryComboBox.Items.Add(category);
                }
            }
        }

        void ToggleEventhandlers(bool toggle)
        {
            if (toggle)
            {
                SaveButton.Click += SaveEventHandler;
                UpdateButton.Click += UpdateEventHandler;
                DeleteButton.Click += DeleteEventHandler;
                SelectImageButton.Click += SelectFileEventHandler;
                FoodDataGrid.SelectionChanged += ProductSelectedEventHandler;
            }
            else
            {
                SaveButton.Click -= SaveEventHandler;
                UpdateButton.Click -= UpdateEventHandler;
                DeleteButton.Click -= DeleteEventHandler;
                SelectImageButton.Click -= SelectFileEventHandler;
                FoodDataGrid.SelectionChanged -= ProductSelectedEventHandler;
            }
        }

        void SaveEventHandler(object o, EventArgs args)
        {
            if (o != SaveButton) return;
            var product = new Product();
            product.Name = NameTextBox.Text;
            product.Description = DescriptionTextBox.Text;

            //Try to parse the price textboxes into product prices, set to null if they're invalid.
            //A null price means the product doesn't have that size. Product needs at least 1 price to be valid.
            product.SmallPrice = (decimal.TryParse(SmallPriceTextBox.Text, out var smallPrice) && smallPrice > 0) ? smallPrice : (decimal?)null;
            product.MediumPrice = (decimal.TryParse(MediumPriceTextBox.Text, out var mediumPrice) && mediumPrice > 0) ? mediumPrice : (decimal?)null;
            product.LargePrice = (decimal.TryParse(LargePriceTextBox.Text, out var largePrice) && largePrice > 0) ? largePrice : (decimal?)null;
            product.ImageName = ImagePathTextBox.Text;
            //Cast the selected item in category combobox into a category object
            Category category = (Category)CategoryComboBox.SelectedItem;
            if (category != null) product.CategoryId = category.Id;
            //Validate input, require name, category and at least one valid price
            if (!string.IsNullOrEmpty(product.Name) && category != null &&
                (product.SmallPrice.HasValue || product.MediumPrice.HasValue || product.LargePrice.HasValue))
            {
                using (var context = new PointOfSaleContext())
                {
                    context.Products.Add(product);
                    try
                    {
                        var success = context.SaveChanges() > 0;
                        //Show error if no row is affected.
                        if (!success) MessageBox.Show($"Error! No data inserted");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Could not insert new data {ex.Message}");
                    }
                }
            }
            else
            {
                MessageBox.Show("Invalid input, please enter at least product name, one price, and select a category");
            }
            //Reload datagrid
            UpdateDataGrid();
            ClearInput();
        }

        private void UpdateEventHandler(object o, EventArgs args)
        {
            if (o != UpdateButton) return;
            {
                using (var context = new PointOfSaleContext())
                {
                    var product = context.Products.FirstOrDefault(x => x.Id == ((Product)FoodDataGrid.SelectedItem).Id);
                    product.Name = NameTextBox.Text;
                    product.Description = DescriptionTextBox.Text;

                    //Try to parse the price textboxes into product prices, set to null if they're invalid.
                    //A null price means the product doesn't have that size. Product needs at least 1 price to be valid.
                    product.SmallPrice = (decimal.TryParse(SmallPriceTextBox.Text, out var smallPrice) && smallPrice > 0) ? smallPrice : (decimal?)null;
                    product.MediumPrice = (decimal.TryParse(MediumPriceTextBox.Text, out var mediumPrice) && mediumPrice > 0) ? mediumPrice : (decimal?)null;
                    product.LargePrice = (decimal.TryParse(LargePriceTextBox.Text, out var largePrice) && largePrice > 0) ? largePrice : (decimal?)null;
                    product.ImageName = ImagePathTextBox.Text;
                    //Cast the selected item in category combobox into a category object
                    Category category = ((Category)CategoryComboBox.SelectedItem);
                    if (category != null) product.CategoryId = category.Id;
                    //Validate input, require name, category and at least one valid price
                    if (!string.IsNullOrEmpty(product.Name) && category != null &&
                        (product.SmallPrice.HasValue || product.MediumPrice.HasValue || product.LargePrice.HasValue))
                    {
                        try
                        {
                            var success = context.SaveChanges() > 0;
                            //Show error if no row is affected.
                            if (!success) MessageBox.Show($"Error! No data inserted");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Could not insert new data {ex.Message}");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid input, please enter at least product name, one price, and select a category");
                    }
                }
            }
            //Reload datagrid
            UpdateDataGrid();
            ClearInput();
        }

        private void DeleteEventHandler(object o, EventArgs args)
        {
            if (o != DeleteButton) return;
            using (var context = new PointOfSaleContext())
            {
                try
                {
                    var product = context.Products.FirstOrDefault(x => x.Id == ((Product)FoodDataGrid.SelectedItem).Id);
                    product.IsEnabled = false;
                    var success = context.SaveChanges() > 0;
                    if (!success) MessageBox.Show("Error deleting product");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error deleting product " + ex.Message);
                }
            }
            //Reload datagrid
            UpdateDataGrid();
            ClearInput();
        }

        private void SelectFileEventHandler(object o, EventArgs e)
        {
            if (o != SelectImageButton) return;
            //Show image file selection dialog
            OpenFileDialog openFileDialogue = new OpenFileDialog();
            openFileDialogue.InitialDirectory = "c:\\temp";
            openFileDialogue.Filter = "PNG Files (*.png)|*.png";
            openFileDialogue.RestoreDirectory = true;

            var result = openFileDialogue.ShowDialog();

            if (result == true)
            {
                //get file directory from dialog result
                var fileName = openFileDialogue.FileName;
                //Set image path to file name
                ImagePathTextBox.Text = System.IO.Path.GetFileName(fileName);
                //Show the image from file
                FoodImage.Source = new BitmapImage(new Uri(fileName));
                //Copy image into the designated folder for images
                var destination = Directory.GetCurrentDirectory() + @"\Images\" + System.IO.Path.GetFileName(fileName);
                if (!File.Exists(destination))
                    File.Copy(fileName, destination);
            }
        }

        private void ProductSelectedEventHandler(object o, SelectionChangedEventArgs args)
        {
            if (o != FoodDataGrid)
            {
                args.Handled = true;
                return;
            }
            Product product = (Product)FoodDataGrid.SelectedItem;
            if (product != null)
            {
                //Toggle Button enable status. Can update/delete if an item was selected
                SaveButton.IsEnabled = false;
                UpdateButton.IsEnabled = true;
                DeleteButton.IsEnabled = true;

                NameTextBox.Text = product.Name;
                SmallPriceTextBox.Text = product.SmallPrice?.ToString();
                MediumPriceTextBox.Text = product.MediumPrice?.ToString();
                LargePriceTextBox.Text = product.LargePrice?.ToString();
                DescriptionTextBox.Text = product.Description;
                for (int i = 0; i < CategoryComboBox.Items.Count; i++)
                {
                    Category category = (Category)CategoryComboBox.Items.GetItemAt(i);
                    if (category.Name == product.Category.Name)
                        CategoryComboBox.SelectedIndex = i;
                }
                ImagePathTextBox.Text = product.ImageName;
                try
                {
                    FoodImage.Source = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + product.FullImagePath));
                }
                catch (Exception)
                {
                    //Skip showing image if path is incorrect
                    FoodImage.Source = null;
                }
            }
            else 
            {

                //Toggle Button enable status
                SaveButton.IsEnabled = true;
                UpdateButton.IsEnabled = false;
                DeleteButton.IsEnabled = false;
            }
            args.Handled = true;
        }

        public void ClearInput()
        {
            NameTextBox.Text = "";
            DescriptionTextBox.Text = "";
            SmallPriceTextBox.Text = "";
            MediumPriceTextBox.Text = "";
            LargePriceTextBox.Text = "";
            ImagePathTextBox.Text = "";
            FoodImage.Source = null;
            CategoryComboBox.SelectedItem = null;
        }

        public void UpdateDataGrid()
        {
            //Only load enabled product.
            //Products that are disabled are still in database for generating sales report purpose
            using (var context = new PointOfSaleContext())
            {
                //Using Include method to avoid Category being null from lazyloading.
                Products = context.Products.Include(x => x.Category).Where(x => x.IsEnabled && x.Category.IsEnable).ToList();
            }
            FoodDataGrid.Items.Clear();
            Products.ForEach(x => FoodDataGrid.Items.Add(x));
        }
    }
}
