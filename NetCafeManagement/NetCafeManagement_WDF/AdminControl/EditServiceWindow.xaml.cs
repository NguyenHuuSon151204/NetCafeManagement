using BusinessObjects.Models;
using DataAccessLayer;
using Repositories;
using Services;
using System;
using System.Windows;

namespace NetCafeManagement_WDF.AdminControl
{
    public partial class EditServiceWindow : Window
    {
        private readonly ProductService _serviceRepo;
        private readonly Product _service;

        public EditServiceWindow(Product service)
        {
            InitializeComponent();
            _serviceRepo = new ProductService(new NetCafeContext());
            _service = service;
            LoadData();
        }

        private void LoadData()
        {
            NameTextBox.Text = _service.Name;
  
            PriceTextBox.Text = _service.Price.ToString();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            _service.Name = NameTextBox.Text.Trim();
           

            if (!decimal.TryParse(PriceTextBox.Text, out decimal price))
            {
                MessageBox.Show("Giá không hợp lệ.");
                return;
            }

            _service.Price = price;
            _serviceRepo.Edit(_service);
            MessageBox.Show("Đã cập nhật dịch vụ.");
            this.DialogResult = true;
            this.Close();
        }
    }
}
