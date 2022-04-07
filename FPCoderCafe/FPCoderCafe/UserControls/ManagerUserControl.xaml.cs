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
    /// Interaction logic for ManagerUserControl.xaml
    /// </summary>
    public partial class ManagerUserControl : UserControl
    {
        public ManagerUserControl()
        {
            InitializeComponent();
            ToggleEventHandler(true);
        }

        void ToggleEventHandler(bool toggle)
        {
            if (toggle)
            {
                MainTabControl.SelectionChanged += TabControlSelectionChangeEventHandler;
            }
            else
            {
                MainTabControl.SelectionChanged -= TabControlSelectionChangeEventHandler;
            }
        }

        void TabControlSelectionChangeEventHandler(object o, SelectionChangedEventArgs args)
        {
            if (o != MainTabControl) return;
            if (!(args.OriginalSource is TabControl)) return;
            TabItem tab = MainTabControl.SelectedItem as TabItem;
            if (tab.Content == FoodControl)
            {
                FoodControl.SetupCategory();
                FoodControl.ClearInput();
                FoodControl.UpdateDataGrid();
            }
        }
    }
}
