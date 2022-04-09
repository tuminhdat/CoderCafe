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
using System.Windows.Shapes;

namespace FPCoderCafe
{
    /// <summary>
    /// Interaction logic for CashUserControl.xaml
    /// </summary>
    public partial class CashWindow : Window
    {
        private readonly decimal TotalAmount;
        private readonly decimal RoundedAmount;
        public decimal TetheredAmount;
        public decimal RefundAmount;

        public CashWindow(decimal totalAmount)
        {
            InitializeComponent();
            TotalAmount = totalAmount;
            RoundedAmount = Math.Round(totalAmount / 0.05m, 0) * 0.05m;
            ToggleEventHandler(true);
            UpdateUI();
        }

        void ToggleEventHandler(bool toggle)
        {
            if (toggle)
            {
                FiveCentButton.Click += AddCashEventHandler;
                TenCentButton.Click += AddCashEventHandler;
                TwentyFiveCentButton.Click += AddCashEventHandler;
                OneDollarButton.Click += AddCashEventHandler;
                TwoDollarButton.Click += AddCashEventHandler;
                FiveDollarButton.Click += AddCashEventHandler;
                TenDollarButton.Click += AddCashEventHandler;
                TwentyDollarButton.Click += AddCashEventHandler;
                FiftyDollarButton.Click += AddCashEventHandler;
                OneHundredDollarButton.Click += AddCashEventHandler;
            }
            else
            {
                FiveCentButton.Click -= AddCashEventHandler;
                TenCentButton.Click -= AddCashEventHandler;
                TwentyFiveCentButton.Click -= AddCashEventHandler;
                OneDollarButton.Click -= AddCashEventHandler;
                TwoDollarButton.Click -= AddCashEventHandler;
                FiveDollarButton.Click -= AddCashEventHandler;
                TenDollarButton.Click -= AddCashEventHandler;
                TwentyDollarButton.Click -= AddCashEventHandler;
                FiftyDollarButton.Click -= AddCashEventHandler;
                OneHundredDollarButton.Click -= AddCashEventHandler;
            }
        }

        void AddCashEventHandler(object o, EventArgs args)
        {
            //Add the tethered amount
            if (o == FiveCentButton)
                TetheredAmount += 0.05m;
            else if (o == TenCentButton)
                TetheredAmount += 0.1m;
            else if (o == TwentyFiveCentButton)
                TetheredAmount += 0.25m;
            else if (o == OneDollarButton)
                TetheredAmount += 1m;
            else if (o == TwoDollarButton)
                TetheredAmount += 2m;
            else if (o == FiveDollarButton)
                TetheredAmount += 5m;
            else if (o == TenDollarButton)
                TetheredAmount += 10m;
            else if (o == TwentyDollarButton)
                TetheredAmount += 20m;
            else if (o == FiftyDollarButton)
                TetheredAmount += 50m;
            else if (o == OneHundredDollarButton)
                TetheredAmount += 100m;

            UpdateUI();
            //If user has tethered enough money. Calculate refund amount and close this window dialog
            if (TetheredAmount >= RoundedAmount)
            {
                RefundAmount = TetheredAmount - RoundedAmount;
                DialogResult = true;
            }
        }

        void UpdateUI()
        {
            CurrentAmountTextBlock.Text = "Tethered: " + TetheredAmount.ToString("$0.00");
            RemainingAmountTextBlock.Text = "Remaining:\n" + (RoundedAmount - TetheredAmount).ToString("$0.00");
            TotalAmounTextBlock.Text = "Total: " + TotalAmount.ToString("$0.00");
        }
    }
}
