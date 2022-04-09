using FPCoderCafe.Entities;
using FPCoderCafe.Utilities;
using System;
using System.Collections;
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
        List<Order> orderList = new List<Order>();
        List<Customer> customerList = new List<Customer>();
        List<Category> categoryList = new List<Category>();
        List<Item> itemList = new List<Item>();
        List<Product> productList = new List<Product>();

        public SaleReportUserControl()
        {
            ToggleEventHandler(false);
            InitializeComponent();
            InitializeDataGrid();
            InitializeItemDataGrid();
            PopulateOrderDatagrid();
            ToggleEventHandler(true);

        }

        private void ToggleEventHandler( bool toggle)
        {
            if (toggle)
            {
                SearchByCategoryText.TextChanged += CheckSearchTextBox;
                DisplayItemButton.Click += DisplayItem;
            }
            else
            {
                //DisplayItemButton.Click -= DisplayItem;

            }
        }
        private void CheckSearchTextBox(object o, EventArgs e)
        {
            if (o == SearchByCategoryText)
            {
                UpdateSearchTableByCategory();
            }      
        }

        private void UpdateSearchTableByDate()
        {
            var startDate = StartDatePicker.SelectedDate;
            var endDate = EndDatePicker.SelectedDate;
            SearchTextBlock.Text = "";
            StringBuilder sb = new StringBuilder();

            using (var ctx = new PointOfSaleContext())
            {
                customerList = ctx.Customers.ToList();
                orderList = ctx.Orders.ToList();
                var orderToDisplay = from order in orderList
                                     from customer in customerList
                                     where order.CustomerId == customer.Id &&
                                     (order.CreateTime >= startDate && order.CreateTime <= endDate)
                                     select order;
                var customerName = from order in orderToDisplay select order.CusPhone;
                var orderDate = from order in orderToDisplay select order.CreateTime;
                itemList = (List<Item>)(from order in orderToDisplay select order.Items.ToList());
                sb.Append("Customer phone: " + customerName);
                sb.Append("Order:");

                for(int i = 0; i < orderToDisplay.Count(); i++)
                {
                    foreach (Item item in itemList)
                    {
                        sb.Append("\t" + i + ". Order Date: " + orderDate + " || Item: " + item);

                    }
                }

            }
        }
        private void UpdateSearchTableByCategory()
        {
            SearchTextBlock.Text = "";
            StringBuilder sb = new StringBuilder();

            using (var ctx = new PointOfSaleContext())
            {
                categoryList = ctx.Categories.ToList();

                // search by category use ToLower() to reduce case sensitive
                var searchEntered = from c in categoryList
                                where (c.Name.ToLower().Contains(SearchByCategoryText.Text.ToLower())) select c;
                // Display the message if no input
                if (searchEntered.Count() == 0)
                {
                    sb.Append("No match found!");
                    SearchTextBlock.Text += sb.ToString();
                    return;
                }
                var cateName = from c in searchEntered select c.Name;
                productList = ctx.Products.ToList();
                List<Product> newProductList = new List<Product>();
                foreach (Category c in searchEntered)
                {
                    newProductList = productList.Where(x => x.CategoryId == c.Id).ToList();
                }
                var productName = from p in newProductList select p.Name;

                sb.Append("Category Name: " + cateName.First() + "\n");
                sb.Append("Product details: \n");
                foreach (Product p in newProductList)
                {
                    sb.Append("\tName:" + p.Name +"|| Small Price:" + p.SmallPrice + " || Medium Price: " + p.MediumPrice + " || Large Price: " + p.LargePrice + " || Description: " + p.Description);
                }
                // add string to text box
                SearchTextBlock.Text += sb.ToString();
            }
        }
        private void InitializeDataGrid()
        {
            DataGridTextColumn IdColumn = new DataGridTextColumn();
            IdColumn.Header = "Id";
            IdColumn.Binding = new Binding("Id");

            DataGridTextColumn OrderNumberColumn = new DataGridTextColumn();
            OrderNumberColumn.Header = "Order Number";
            OrderNumberColumn.Binding = new Binding("OrderNumber");

            DataGridTextColumn TotalAmountColumn = new DataGridTextColumn();
            TotalAmountColumn.Header = "Total Amount";
            TotalAmountColumn.Binding = new Binding("TotalAmt");

            DataGridTextColumn CustomerPhoneColumn = new DataGridTextColumn();
            CustomerPhoneColumn.Header = "Customer Phone";
            CustomerPhoneColumn.Binding = new Binding("Order.CusPhone");

            OrderDatagrid.Columns.Add(IdColumn);
            OrderDatagrid.Columns.Add(OrderNumberColumn);
            OrderDatagrid.Columns.Add(TotalAmountColumn);
            OrderDatagrid.Columns.Add(CustomerPhoneColumn);
        }
        private void PopulateOrderDatagrid()
        {
            using (var ctx = new PointOfSaleContext())
            {
                orderList = ctx.Orders.ToList();
                foreach(Order order in orderList)
                {
                    OrderDatagrid.Items.Add(order);
                }
            }
            // get total of orders
            var totalOrders = orderList.Count();
            TotalOrderText.Text = totalOrders.ToString();

            //get total profit
            var profit = orderList.Select(x => x.TotalAmt).Sum();
            TotalProfitsText.Text = profit.ToString();
        }

        private void InitializeItemDataGrid()
        {
            DataGridTextColumn IdColumn = new DataGridTextColumn();
            IdColumn.Header = "Id";
            IdColumn.Binding = new Binding("Id");

            DataGridTextColumn QuantityColumn = new DataGridTextColumn();
            QuantityColumn.Header = "Quantity";
            QuantityColumn.Binding = new Binding("Quantity");

            DataGridTextColumn AmountColumn = new DataGridTextColumn();
            AmountColumn.Header = "Amount";
            AmountColumn.Binding = new Binding("Amount");

            DataGridTextColumn ItemSizeColumn = new DataGridTextColumn();
            ItemSizeColumn.Header = "Size";
            ItemSizeColumn.Binding = new Binding("ItemSize");

            DataGridTextColumn NoteColumn = new DataGridTextColumn();
            NoteColumn.Header = "Note";
            NoteColumn.Binding = new Binding("Note");

            DataGridTextColumn TaxColumn = new DataGridTextColumn();
            TaxColumn.Header = "Tax";
            TaxColumn.Binding = new Binding("TaxAmount");

            DataGridTextColumn SaleColumn = new DataGridTextColumn();
            SaleColumn.Header = "Sales";
            SaleColumn.Binding = new Binding("Sales");

            ItemDataGrid.Columns.Add(IdColumn);
            ItemDataGrid.Columns.Add(QuantityColumn);
            ItemDataGrid.Columns.Add(AmountColumn);
            ItemDataGrid.Columns.Add(ItemSizeColumn);
            ItemDataGrid.Columns.Add(NoteColumn);
            ItemDataGrid.Columns.Add(TaxColumn);
            ItemDataGrid.Columns.Add(SaleColumn);
        }
        private void DisplayItem(object o, EventArgs e)
        {
            Order selectedOrder = (Order)OrderDatagrid.SelectedItem;
            using (var ctx = new PointOfSaleContext())
            {
                itemList = ctx.Items.Where(x => x.OrderId == selectedOrder.Id).ToList();
                ItemDataGrid.ItemsSource = itemList.ToList();
            }
        }

      
    }
}
