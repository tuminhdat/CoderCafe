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
        StreamWriter sw;

        public CategoryUserControl()
        {
            InitializeComponent();
            ToggleEventsHandle(false);
            InitializeDataGrid();
            SetUpCategoryGrid();
            PopulateCategoryDataGrid();
            //Make an event handlers
            ToggleEventsHandle(true);
        }


        private void ToggleEventsHandle(bool toggle)
        {
            if (toggle)
            {
                SaveCategoryButton.Click += SaveCategoryOnClick;
                UpdateCategoryButton.Click += UpdateCategoryOnClick;
                DeleteCategoryButton.Click += DeleteCategoryOnClick;
                SelectImageButton.Click += SelectFile;
                CategoryDataGrid.SelectionChanged += DisplayCategoryToUpdate;
                CancelButton.Click += ResetAll;
            }
            else
            {
                SaveCategoryButton.Click -= SaveCategoryOnClick;
                UpdateCategoryButton.Click -= UpdateCategoryOnClick;
                DeleteCategoryButton.Click -= DeleteCategoryOnClick;
                SelectImageButton.Click -= SelectFile;
                CategoryDataGrid.SelectionChanged -= DisplayCategoryToUpdate;
                CancelButton.Click -= ResetAll;
            }
        }

        private void ResetAll(object o, EventArgs e)
        {
            //Unselect data grid and clear the textbox
            CategoryDataGrid.UnselectAll();
            ClearTextBox();
        }
        private void SelectFile(object o, EventArgs e)
        {
            OpenFileDialog openFileDialogue = new OpenFileDialog();
            openFileDialogue.InitialDirectory = "c:\\temp";
            openFileDialogue.Filter = "Images (*.png,*.jpg)|*.png;*.jpg";
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
                }
            }
        }

        private void SetUpCategoryGrid()
        {
            CategoryDataGrid.SelectionMode = DataGridSelectionMode.Single;
            CategoryDataGrid.IsReadOnly = true;
        }

        private void InitializeDataGrid()
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
        private void PopulateCategoryDataGrid()
        {
            CategoryDataGrid.Items.Clear();
            using (var ctx = new PointOfSaleContext())
            {
                //Add data from database to Category datagrid where isEnable is equals to true.
                categoryList = ctx.Categories.Where(x => x.IsEnable).ToList();
                foreach (Category c in categoryList)
                {
                    CategoryDataGrid.Items.Add(c);
                }
            }
        }
        private void SaveCategoryOnClick(object o, EventArgs e)
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
                    //Check if category name enter exist inside the database
                    categoryList = ctx.Categories.Where(x => x.Name == newCategory.Name).ToList();
                    foreach(Category c in categoryList)
                    {
                        if (newCategory.Name == c.Name)
                        {
                            MessageBox.Show("This category already add. Please enter another one");
                            return;
                        }   
                    }
                    //add new category enter from the text box to database
                    ctx.Categories.Add(newCategory);
                    ctx.SaveChanges();
                }

                /*var filepath = "Category.csv";
                if (!File.Exists(filepath))
                {
                    using (sw = new StreamWriter(new FileStream(filepath, FileMode.Create, FileAccess.Write)))
                    {
                        sw.Write(newCategory.Name + ", " + newCategory.Description + ", " + newCategory.ImageName);
                    }
                }
                else
                {
                    using (sw = new StreamWriter(new FileStream(filepath, FileMode.Open, FileAccess.Write)))
                    {
                        sw.Write(newCategory.Name + ", " + newCategory.Description + ", " + newCategory.ImageName);
                    }
                }*/
                PopulateCategoryDataGrid();
                ClearTextBox();
            }

        }
        private void DisplayCategoryToUpdate(object o, EventArgs e)
        {
            if (CategoryDataGrid.SelectedItem != null)
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
                SaveCategoryButton.IsEnabled = false;
            }
            else
            {
                UpdateCategoryButton.IsEnabled = false; 
                SaveCategoryButton.IsEnabled = true;
            }
        }

        private void UpdateCategoryOnClick(object o, EventArgs e)
        {
            using(var ctx = new PointOfSaleContext())
            {
                Category selectedCategory = (Category)CategoryDataGrid.SelectedItem;
                int idToUpdate = selectedCategory.Id;
                //Pull the repestive category for the id we have
                Category uc = ctx.Categories.Where(x => x.Id == idToUpdate).First();
                if (CategoryTextBox.Text.Equals("") || CategoryDescripTextBox.Text.Equals(""))
                {
                    CategoryTextBox.Text = selectedCategory.Name;
                    CategoryDescripTextBox.Text = selectedCategory.Description;
                    ImagePathTextBox.Text = selectedCategory.ImageName;
                    MessageBox.Show("Please enter category name and description to update.");
                    return; 
                }
                 
                //Reconstruct the object based on the form data
                uc.Name = CategoryTextBox.Text;
                uc.Description = CategoryDescripTextBox.Text;
                uc.ImageName = ImagePathTextBox.Text;
              
                //Update the object
                ctx.Categories.Update(uc);
                //Save changes
                ctx.SaveChanges();
                //Update datagrid
                PopulateCategoryDataGrid();
            }
            //Clear the textbox after clicking update button
            ClearTextBox();
        }
        private void DeleteCategoryOnClick(object o, EventArgs e)
        {
            using(var ctx = new PointOfSaleContext())
            {
                Category selectedCategory = (Category)CategoryDataGrid.SelectedItem;
                int idToUpdate = selectedCategory.Id;
                //Pull the repestive phone for the id we have
                Category uc = ctx.Categories.Where(x => x.Id == idToUpdate).First();
                uc.IsEnable = false;
                ctx.Categories.Update(uc);
                ctx.SaveChanges();
                PopulateCategoryDataGrid();
            }
            ClearTextBox();
        }
        private void ClearTextBox()
        {
            //clear the text box anf image view
            CategoryTextBox.Text = "";
            CategoryDescripTextBox.Text = "";
            ImagePathTextBox.Text = "";
            CategoryImage.Source = null;
        }
    }
}
