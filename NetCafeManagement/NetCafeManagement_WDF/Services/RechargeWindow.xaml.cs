using System;
using System.Windows;

namespace NetCafeManagement_WDF.Views
{
    public partial class RechargeWindow : Window
    {
        public decimal Amount { get; private set; }

        public RechargeWindow()
        {
            InitializeComponent();
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            if (decimal.TryParse(AmountBox.Text.Trim(), out decimal value) && value > 0)
            {
                Amount = value;
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Vui lòng nhập số tiền hợp lệ.");
            }
        }
    }
}
