using BusinessObjects.Models;
using DataAccessLayer;

using Services;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace NetCafeManagement_WDF.AdminControl
{
    public partial class ManageUsersControl : UserControl
    {
        private readonly CustomerService _customerService;

        public ManageUsersControl()
        {
            InitializeComponent();
            var context = new NetCafeContext();
            _customerService = new CustomerService(context);
            LoadCustomers();
        }

        private void LoadCustomers()
        {
            var customers = _customerService.GetAll();
            var displayList = customers.Select(c => new
            {
                c.CustomerId,
                c.Name,
                c.Phone,
                BalanceFormatted = $"{c.Balance:N0} VNĐ",
                JoinDateFormatted = c.JoinDate?.ToString("dd/MM/yyyy") ?? ""
            }).ToList();

            UsersDataGrid.ItemsSource = displayList;

            //var users = _customerService.GetAll().ToList();
            //UsersDataGrid.ItemsSource = users;
        }
       


        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddUserWindow();
            if (addWindow.ShowDialog() == true)
            {
                LoadCustomers(); // làm mới danh sách
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (UsersDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn người dùng để sửa", "Thông báo");
                return;
            }

            // Lấy selected item
            var selected = UsersDataGrid.SelectedItem;

            // Lấy CustomerId từ object ẩn danh (anonymous object)
            var customerId = (int)selected.GetType().GetProperty("CustomerId")?.GetValue(selected, null);

            // Lấy dữ liệu từ DB
            var customer = _customerService.GetById(customerId);
            if (customer == null)
            {
                MessageBox.Show("Không tìm thấy người dùng.", "Lỗi");
                return;
            }

            // Mở cửa sổ sửa
            var editWindow = new EditUserWindow(customer);
            if (editWindow.ShowDialog() == true)
            {
                LoadCustomers();
                MessageBox.Show("Đã cập nhật người dùng.", "Thông báo");
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (UsersDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn người dùng để xoá", "Thông báo");
                return;
            }

            var selected = UsersDataGrid.SelectedItem;
            var customerId = (int)selected.GetType().GetProperty("CustomerId").GetValue(selected, null);

            var confirm = MessageBox.Show("Bạn có chắc muốn xoá người dùng này?", "Xác nhận", MessageBoxButton.YesNo);
            if (confirm == MessageBoxResult.Yes)
            {
                _customerService.Delete(customerId);
                LoadCustomers();
                MessageBox.Show("Đã xoá người dùng!", "Thông báo");
            }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            LoadCustomers();
            MessageBox.Show("Đã làm mới danh sách", "Thông báo");
        }
    }
}
