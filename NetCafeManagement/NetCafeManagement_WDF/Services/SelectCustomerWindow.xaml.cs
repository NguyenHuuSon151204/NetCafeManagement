using BusinessObjects.Models;
using DataAccessLayer;
using System;
using System.Linq;
using System.Windows;

namespace NetCafeManagement_WDF.Views
{
    public partial class SelectCustomerWindow : Window
    {
        public int SelectedCustomerId { get; private set; }
        public decimal RechargeAmount { get; private set; }

        public SelectCustomerWindow()
        {
            InitializeComponent();
            LoadCustomers();
        }

        private void LoadCustomers()
        {
            using var context = new NetCafeContext();
            CustomerListBox.ItemsSource = context.Customers.ToList();
        }

        private void AddCustomer_Click(object sender, RoutedEventArgs e)
        {
            string name = NewCustomerName.Text.Trim();
            string phone = NewCustomerPhone.Text.Trim();

            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Vui lòng nhập tên khách.");
                return;
            }

            using var context = new NetCafeContext();
            var newCustomer = new Customer
            {
                Name = name,
                Phone = string.IsNullOrWhiteSpace(phone) ? null : phone,
                JoinDate = DateTime.Now,
                Balance = 0
            };
            context.Customers.Add(newCustomer);
            context.SaveChanges();

            MessageBox.Show("Đã thêm khách hàng mới.");
            LoadCustomers();
            CustomerListBox.SelectedItem = newCustomer;
        }

        private void SelectButton_Click(object sender, RoutedEventArgs e)
        {
            if (CustomerListBox.SelectedItem is Customer selected)
            {
                SelectedCustomerId = selected.CustomerId;

                // Xử lý tiền nạp
                if (decimal.TryParse(RechargeAmountBox.Text.Trim(), out decimal amount) && amount > 0)
                {
                    RechargeAmount = amount;

                    // Lưu vào TRANSACTIONS + cộng vào balance
                    using var context = new NetCafeContext();
                    var customer = context.Customers.FirstOrDefault(c => c.CustomerId == SelectedCustomerId);
                    if (customer != null)
                    {
                        customer.Balance += amount;
                        context.Transactions.Add(new Transaction
                        {
                            CustomerId = customer.CustomerId,
                            Amount = amount,
                            Type = "Deposit",
                            CreatedAt = DateTime.Now
                        });
                        context.SaveChanges();
                    }
                }

                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một khách hàng.");
            }
        }
    }
}
