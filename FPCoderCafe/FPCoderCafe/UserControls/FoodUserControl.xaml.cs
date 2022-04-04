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
    /// Interaction logic for FoodUserControl.xaml
    /// </summary>
    public partial class FoodUserControl : UserControl
    {
        public FoodUserControl()
        {
            InitializeComponent();
            SetupCategory();
        }

        private void SetupCategory()
        {
            using (var context = new PointOfSaleContext())
            {
                var categories = context.Categories.ToList();
                foreach (var category in categories)
                {
                    CategoryComboBox.Items.Add(category);
                }
            }
        }

        void ToggleEventhandlers(bool toggle)
        {
        }

        void AddEventHandler(object o, EventArgs args)
        {
        }


    }
}
