using BusinessObjects.Models;
using DataAccessLayer;
using Services;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace NetCafeManagement_WDF.AdminControl
{
    public partial class ManageServicesControl : UserControl
    {
        private readonly ProductService _productService;

        public ManageServicesControl()
        {
            InitializeComponent();
            _productService = new ProductService(new NetCafeContext());
            LoadServices();
        }

        private void LoadServices()
        {
            var products = _productService.GetAll();

            var displayList = products.Select(p => new
            {
                p.ProductId,
                p.Name,
                p.Category,
                PriceFormatted = $"{p.Price:N0} VNĐ",
                StatusText = p.IsActive == true ? "Đang bán" : "Ngừng bán"
            }).ToList();

            ServiceDataGrid.ItemsSource = displayList;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddServiceWindow();
            if (addWindow.ShowDialog() == true)
            {
                LoadServices();
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (ServiceDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn dịch vụ để sửa", "Thông báo");
                return;
            }

            var selected = ServiceDataGrid.SelectedItem;
            var productId = (int)selected.GetType().GetProperty("ProductId").GetValue(selected, null);

            var product = _productService.GetById(productId);
            if (product == null)
            {
                MessageBox.Show("Không tìm thấy dịch vụ.", "Lỗi");
                return;
            }

            var editWindow = new EditServiceWindow(product);
            if (editWindow.ShowDialog() == true)
            {
                LoadServices();
                MessageBox.Show("Đã cập nhật dịch vụ.", "Thông báo");
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (ServiceDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn dịch vụ để xoá", "Thông báo");
                return;
            }

            var selected = ServiceDataGrid.SelectedItem;
            var productId = (int)selected.GetType().GetProperty("ProductId").GetValue(selected, null);

            var confirm = MessageBox.Show("Bạn có chắc muốn xoá dịch vụ này?", "Xác nhận", MessageBoxButton.YesNo);
            if (confirm == MessageBoxResult.Yes)
            {
                _productService.Delete(productId);
                LoadServices();
                MessageBox.Show("Đã xoá dịch vụ!", "Thông báo");
            }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            LoadServices();
        }
    }
}
