using DataAccessLayer;
using Services;
using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows;

namespace NetCafeManagement_WDF.AdminControl
{
    public partial class ActivityLogControl : UserControl
    {
        private readonly ActivityLogService _logService;

        public ActivityLogControl()
        {
            InitializeComponent();
            _logService = new ActivityLogService(new NetCafeContext());
            LoadAllLogs(); // Hiện log ban đầu
        }

        private void LoadAllLogs()
        {
            var logs = _logService.GetAllLogs();
            ActivityLogDataGrid.ItemsSource = logs;
            TotalActivitiesText.Text = logs.Count().ToString();
            TodayLoginsText.Text = logs.Count(l => l.ActivityType == "Login" && l.Timestamp.Date == DateTime.Today).ToString();
            ActiveUsersText.Text = logs.Count(l => l.ActivityType == "Login").ToString();
            TodayRevenueText.Text = logs.Where(l => l.ActivityType == "Payment" && l.Timestamp.Date == DateTime.Today)
                                        .Sum(l => Decimal.TryParse(l.Amount?.Replace(" VNĐ", ""), out var v) ? v : 0).ToString("N0") + " VNĐ";
        }

        private void ApplyFilter(string type)
        {
            var from = FromDatePicker.SelectedDate;
            var to = ToDatePicker.SelectedDate;
            var logs = _logService.FilterLogs(type, from, to);
            ActivityLogDataGrid.ItemsSource = logs;
            TotalActivitiesText.Text = logs.Count().ToString();
            TodayLoginsText.Text = logs.Count(l => l.ActivityType == "Login" && l.Timestamp.Date == DateTime.Today).ToString();
            ActiveUsersText.Text = logs.Count(l => l.ActivityType == "Login").ToString();
            TodayRevenueText.Text = logs.Where(l => l.ActivityType == "Payment" && l.Timestamp.Date == DateTime.Today)
                                        .Sum(l => Decimal.TryParse(l.Amount?.Replace(" VNĐ", ""), out var v) ? v : 0).ToString("N0") + " VNĐ";
        }

        private void AllFilter_Click(object sender, RoutedEventArgs e) => ApplyFilter("All");
        private void LoginFilter_Click(object sender, RoutedEventArgs e) => ApplyFilter("Login");
        private void LogoutFilter_Click(object sender, RoutedEventArgs e) => ApplyFilter("Logout");
        private void PaymentFilter_Click(object sender, RoutedEventArgs e) => ApplyFilter("Payment");
        private void SearchButton_Click(object sender, RoutedEventArgs e) => ApplyFilter("All");

        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Export to Excel logic (có thể dùng thư viện ClosedXML hoặc Excel interop)
            MessageBox.Show("Tính năng xuất Excel sẽ được triển khai sau.", "Thông báo");
        }

        private void ActivityLogDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}