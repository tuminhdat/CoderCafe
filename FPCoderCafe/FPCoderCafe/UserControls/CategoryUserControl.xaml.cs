using FPCoderCafe.Entities;
using FPCoderCafe.Utilities;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;

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
            ToggleEventsHandle(false);
            initializeDataGrid();
            setUpCategoryGrid();
            populateCategoryDataGrid();
            //Make an event handlers
            ToggleEventsHandle(true);

        }


        private void ToggleEventsHandle(bool toggle)
        {
            if (toggle)
            {
                SaveCategoryButton.Click += saveCategoryOnClick;
                UpdateCategoryButton.Click += UpdateCategoryOnClick;
                DeleteCategoryButton.Click += DeleteCategoryOnClick;
                SelectImageButton.Click += selectFile;
                CategoryDataGrid.SelectionChanged += displayCategoryToUpdate;
            }
            else
            {
                SaveCategoryButton.Click -= saveCategoryOnClick;
                UpdateCategoryButton.Click -= UpdateCategoryOnClick;
                DeleteCategoryButton.Click -= DeleteCategoryOnClick;
                SelectImageButton.Click -= selectFile;
                CategoryDataGrid.SelectionChanged -= displayCategoryToUpdate;
            }
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
                //Set the name of chosen image file
                ImagePathTextBox.Text = System.IO.Path.GetFileName(fileName);
                //Display selected image
                CategoryImage.Source = new BitmapImage(new Uri(fileName));
                //Get the desired destination path to save image there
                string destination = Directory.GetCurrentDirectory() + @"\Images\" + System.IO.Path.GetFileName(fileName);
               
                if (!File.Exists(destination))
                {
                    //Copy the image file to that destination
                    File.Copy(fileName, destination);
                    //File.Copy(fileName, "/Images/" + System.IO.Path.GetFileName(fileName));

                }
            }
            
        }

        private void setUpCategoryGrid()
        {
            CategoryDataGrid.SelectionMode = DataGridSelectionMode.Single;
            CategoryDataGrid.IsReadOnly = true;
        }

        private void initializeDataGrid()
        {
            //Create columns for the datagrid
            DataGridTextColumn CategoryNameNoColumn = new DataGridTextColumn();
            CategoryNameNoColumn.Header = "Category Name";
            CategoryNameNoColumn.Binding = new Binding("Name");

            DataGridTextColumn DescriptionColumn = new DataGridTextColumn();
            DescriptionColumn.Header = "Description";
            DescriptionColumn.Binding = new Binding("Description");

            DataGridTextColumn ImageNamecolumn = new DataGridTextColumn();
            ImageNamecolumn.Header = "Image Name";
            ImageNamecolumn.Binding = new Binding("ImageName");

            CategoryDataGrid.Columns.Add(CategoryNameNoColumn);
            CategoryDataGrid.Columns.Add(DescriptionColumn);
            CategoryDataGrid.Columns.Add(ImageNamecolumn);

        }
        private void populateCategoryDataGrid()
        {
            CategoryDataGrid.Items.Clear();
            using(var ctx = new PointOfSaleContext())
            {
                //Add data from database to Category datagrid
                categoryList = ctx.Categories.ToList();
                Category category = new Category();
                foreach(Category c in categoryList)
                {
                    CategoryDataGrid.Items.Add(c);
                }
            }
        }
        private void saveCategoryOnClick(Object s, EventArgs e)
        {

            if (CategoryTextBox.Text == "" || ImagePathTextBox.Text == "")
            {
                MessageBox.Show("Please enter category name and select category image");
            }
            else
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

        }
        private void displayCategoryToUpdate(Object s, EventArgs e)
        {
            if(CategoryDataGrid.SelectedItem != null)
            {
                //Get the selected category
                Category selectedCategory = (Category)CategoryDataGrid.SelectedItem;

                //populate the form elements
                CategoryTextBox.Text = selectedCategory.Name;
                CategoryDescripTextBox.Text = selectedCategory.Description;
                ImagePathTextBox.Text = selectedCategory.ImageName;
                string des = Directory.GetCurrentDirectory() + @"\Images\" + selectedCategory.ImageName;
                CategoryImage.Source = new BitmapImage(new Uri(des));     
                UpdateCategoryButton.IsEnabled = true;
            }
            else
            {
                UpdateCategoryButton.IsEnabled = false;
            }
        }

        private void UpdateCategoryOnClick(Object s, EventArgs e)
        {
            using(var ctx = new PointOfSaleContext())
            {
                Category selectedCategory = (Category)CategoryDataGrid.SelectedItem;
                int idToUpdate = selectedCategory.Id;
                //Pull the repestive phone for the id we have
                Category uc = (Category)ctx.Categories.Where(x => x.Id == idToUpdate).First();
                //Reconstruct the object based on the form data
                uc.Name = CategoryTextBox.Text;
                uc.Description = CategoryDescripTextBox.Text;
                uc.ImageName = ImagePathTextBox.Text;
                //Update the object
                ctx.Categories.Update(uc);
                //Save changes
                ctx.SaveChanges();
                //Update datagrid
                populateCategoryDataGrid();
            }
            //Clear the textbox after clicking update button
            clearTextBox();
        }
        private void DeleteCategoryOnClick(Object s, EventArgs e)
        {
            using(var ctx = new PointOfSaleContext())
            {
                Category selectedCategory = (Category)CategoryDataGrid.SelectedItem;
                int idToUpdate = selectedCategory.Id;
                //Pull the repestive phone for the id we have
                Category uc = (Category)ctx.Categories.Where(x => x.Id == idToUpdate).First();
                ctx.Categories.Remove(uc);
                ctx.SaveChanges();
                populateCategoryDataGrid();
            }
            clearTextBox();
        }
        private void clearTextBox()
        {
            //clear the text box anf image view
            CategoryTextBox.Text = "";
            CategoryDescripTextBox.Text = "";
            ImagePathTextBox.Text = "";
            CategoryImage.Source = null;
        }
    }
}
