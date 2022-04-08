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

        List<Order> orderList = new List<Order>();
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
            DataGridTextColumn IdColumn = new DataGridTextColumn();
            IdColumn.Header = "Id";
            IdColumn.Binding = new Binding("Order.Id");

            DataGridTextColumn OrderNumberColumn = new DataGridTextColumn();
            OrderNumberColumn.Header = "Order Number";
            OrderNumberColumn.Binding = new Binding("Order.OrderNumber");

            DataGridTextColumn TotalAmountColumn = new DataGridTextColumn();
            TotalAmountColumn.Header = "Total Amount";
            TotalAmountColumn.Binding = new Binding("Order.TotalAmt");

            OrderDatagrid.Columns.Add(IdColumn);
            OrderDatagrid.Columns.Add(OrderNumberColumn);
            OrderDatagrid.Columns.Add(TotalAmountColumn);
        }
        private void populatedatagrid()
        {
            using (var ctx = new PointOfSaleContext())
            {
                orderList = ctx.Orders.ToList();
                
                foreach(Order order in orderList)
                {
                    OrderDatagrid.Items.Add(order);
                }
            }
        }
    }
}
