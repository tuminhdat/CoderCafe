using FPCoderCafe.Entities;
using FPCoderCafe.Utilities;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for CategoryUserControl.xaml
    /// </summary>
    public partial class CategoryUserControl : UserControl
    {
        List<Category> categoryList = new List<Category>();
        string fileName = "";
        public CategoryUserControl()
        {
            InitializeComponent();
            populateCategoryDataGrid();
            SaveCategoryButton.Click += saveCategoryOnClick;
            SelectImageButton.Click += selectFile;
           // ImageListBox.SelectionChanged += populateCategoryImage;
        }

        private void selectFile(object o, EventArgs e)
        {
            OpenFileDialog openFileDialogue = new OpenFileDialog();
            openFileDialogue.InitialDirectory = "c:\\temp";
            openFileDialogue.Filter = "PNG Files (*.png)|*.png";
            openFileDialogue.RestoreDirectory = true;

            Nullable<bool> result = openFileDialogue.ShowDialog();

            if (result == true)
            {
                //As soon as there is a result from showdialogue assign it to the the fileName 
                fileName = openFileDialogue.FileName;
                ImagePathTextBox.Text = fileName;
                CategoryImage.Source = new BitmapImage(new Uri(fileName));
                if (File.Exists(fileName))
                {
                    File.Copy(fileName, Directory.GetCurrentDirectory() + "/Images");
                }
            }
            
        }

        public void setUpCategoryGrid()
        {
            CategoryDataGrid.SelectionMode = DataGridSelectionMode.Single;
            CategoryDataGrid.IsReadOnly = true;
        }
        public void populateCategoryDataGrid()
        {
            using(var ctx = new PointOfSaleContext())
            {
                categoryList = ctx.Categories.ToList();
                CategoryDataGrid.ItemsSource = categoryList;
            }
        }
        private void saveCategoryOnClick(Object s, EventArgs e)
        {
            Category newCategory = new Category();
            newCategory.Name = CategoryTextBox.Text;
            newCategory.Description = CategoryDescripTextBox.Text;
            newCategory.ImageName = ImagePathTextBox.Text;
            using (var ctx = new PointOfSaleContext())
            {
                //add new category enter from the text box to database
                ctx.Categories.Add(newCategory);
                ctx.SaveChanges();
            }
            populateCategoryDataGrid();
            clearTextBox();

        }
        public void populateCategoryImage(Object s, EventArgs e)
        {
           // ImageListBox.SelectionMode = SelectionMode.Single;
           // ListBoxItem selected = ImageListBox.SelectedItem as ListBoxItem;
            //add image source to category image when appropriate item is selected from list box
         /*   switch (selected.Content.ToString())
            {
                case "Cafe":
                    CategoryImage.Source = new BitmapImage(new Uri("/FPCoderCafe;component/Images/cafe_category.png", UriKind.Relative)); ;
                    break;
                case "Pop":
                    CategoryImage.Source = new BitmapImage(new Uri("/FPCoderCafe;component/Images/pop_category.png", UriKind.Relative)); ;
                    break;
                case "Smoothie":
                    CategoryImage.Source = new BitmapImage(new Uri("/FPCoderCafe;component/Images/smoothie_category.png", UriKind.Relative)); ;
                    break;
                case "Tea":
                    CategoryImage.Source = new BitmapImage(new Uri("/FPCoderCafe;component/Images/tea_category.png", UriKind.Relative)); ;
                    break;
            }*/
        }
        public void clearTextBox()
        {
            //clear the text box 
            CategoryTextBox.Text = "";
            CategoryDescripTextBox.Text = "";
        }
    }
}
