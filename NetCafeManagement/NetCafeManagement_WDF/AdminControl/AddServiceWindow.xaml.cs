using BusinessObjects.Models;
using DataAccessLayer;
using Repositories;
using Services;
using System;
using System.Windows;

namespace NetCafeManagement_WDF.AdminControl
{
    public partial class AddServiceWindow : Window
    {
        private readonly ProductService _serviceRepo;

        public AddServiceWindow()
        {
            InitializeComponent();
            _serviceRepo = new ProductService(new NetCafeContext());
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string name = NameTextBox.Text.Trim();
            string unit = UnitTextBox.Text.Trim();

            if (!decimal.TryParse(PriceTextBox.Text, out decimal price))
            {
                MessageBox.Show("Giá không hợp lệ.");
                return;
            }

            var service = new Product
            {
                Name = name,             
                Price = price
            };

            _serviceRepo.Add(service);
            MessageBox.Show("Đã thêm dịch vụ thành công.");
            this.DialogResult = true;
            this.Close();
        }
    }
}
