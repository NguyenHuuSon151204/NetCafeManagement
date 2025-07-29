using BusinessObjects.Models;
using DataAccessLayer;
using Services;
using System;
using System.Windows;

namespace NetCafeManagement_WDF.AdminControl
{
    public partial class EditUserWindow : Window
    {
        private readonly CustomerService _customerService;
        private readonly Customer _editingCustomer;

        public EditUserWindow(Customer customer)
        {
            InitializeComponent();
            _customerService = new CustomerService(new NetCafeContext());
            _editingCustomer = customer;

            NameTextBox.Text = _editingCustomer.Name;
            PhoneTextBox.Text = _editingCustomer.Phone;
            BalanceTextBox.Text = _editingCustomer.Balance?.ToString() ?? "0";
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            _editingCustomer.Name = NameTextBox.Text.Trim();
            _editingCustomer.Phone = PhoneTextBox.Text.Trim();

            if (!decimal.TryParse(BalanceTextBox.Text.Trim(), out decimal balance))
            {
                MessageBox.Show("Số dư không hợp lệ.");
                return;
            }

            _editingCustomer.Balance = balance;
            _customerService.Update(_editingCustomer);
            MessageBox.Show("Cập nhật người dùng thành công!");
            DialogResult = true;
            Close();
        }
    }
}
