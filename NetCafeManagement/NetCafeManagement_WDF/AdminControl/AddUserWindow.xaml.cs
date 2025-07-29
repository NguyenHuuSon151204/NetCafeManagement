using BusinessObjects.Models;
using DataAccessLayer;
using Services;
using System;
using System.Windows;

namespace NetCafeManagement_WDF.AdminControl
{
    public partial class AddUserWindow : Window
    {
        private readonly CustomerService _customerService;

        public AddUserWindow()
        {
            InitializeComponent();
            _customerService = new CustomerService(new NetCafeContext());
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string name = NameTextBox.Text.Trim();
            string phone = PhoneTextBox.Text.Trim();
            if (!decimal.TryParse(BalanceTextBox.Text.Trim(), out decimal balance))
            {
                MessageBox.Show("Số dư không hợp lệ.");
                return;
            }

            var newCustomer = new Customer
            {
                Name = name,
                Phone = phone,
                Balance = balance,
                JoinDate = DateTime.Now
            };

            _customerService.Create(newCustomer);
            MessageBox.Show("Thêm người dùng thành công!");
            DialogResult = true;
            Close();
        }
    }
}
