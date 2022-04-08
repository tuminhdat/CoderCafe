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

namespace FPCoderCafe.UserControls
{
    /// <summary>
    /// Interaction logic for SaleReportUserControl.xaml
    /// </summary>
    public partial class SaleReportUserControl : UserControl
    {

        List<Category> categoryList = new List<Category>();
        List<Product> productList = new List<Product>();
        List<Item> itemList = new List<Item>();
        public SaleReportUserControl()
        {
            ToggleEventHandler(false);
            InitializeComponent();
            initializeDataGrid();
            ToggleEventHandler(true);

        }

        private void ToggleEventHandler( bool toggle)
        {
            if (toggle)
            {

            }
            else
            {

            }
        }

        private void initializeDataGrid()
        {
            DataGridTextColumn CategoryColumn = new DataGridTextColumn();
            CategoryColumn.Header = "Category";
            CategoryColumn.Binding = new Binding("Category.CategoryName");

            DataGridTextColumn NamegoryColumn = new DataGridTextColumn();
            NamegoryColumn.Header = "Name";
            NamegoryColumn.Binding = new Binding("Product.ProductName");

            DataGridTextColumn QuantityColumn = new DataGridTextColumn();
            QuantityColumn.Header = "Quantity";
            QuantityColumn.Binding = new Binding("Item.ItemQuantity");

            DataGridTextColumn UnitPriceColumn = new DataGridTextColumn();
            UnitPriceColumn.Header = "Category";
            UnitPriceColumn.Binding = new Binding("Item.ItemPrice");

            DataGridTextColumn SalesColumn = new DataGridTextColumn();
            SalesColumn.Header = "Sales";
            SalesColumn.Binding = new Binding("Item.Sales");

            SaleReportDataGrid.Columns.Add(CategoryColumn);
            SaleReportDataGrid.Columns.Add(NamegoryColumn);
            SaleReportDataGrid.Columns.Add(QuantityColumn);
            SaleReportDataGrid.Columns.Add(UnitPriceColumn);
            SaleReportDataGrid.Columns.Add(SalesColumn);
        }
        private void populatedatagrid()
        {
            using (var ctx = new PointOfSaleContext())
            {
                categoryList = ctx.Categories.ToList();
                
                foreach(Category c in categoryList)
                {
                }
            }
        }
    }
}
