using System.Windows;
using System.Windows.Controls;
using NetCafeManagement_WDF.Views;
using NetCafeManagement_WDF.AdminControl;

namespace NetCafeManagement_WDF
{
    public partial class AdminDashboardView : Window
    {
        public AdminDashboardView()
        {
            InitializeComponent();

            // Load mặc định báo cáo doanh thu
            LoadRevenueReport();
        }

        private void LoadRevenueReport()
        {
            MainContentArea.Children.Clear();
            MainContentArea.Children.Add(new RevenueReportControl());
        }

        private void BtnReport_Click(object sender, RoutedEventArgs e)
        {
            LoadRevenueReport();
        }

        private void BtnManagePCs_Click(object sender, RoutedEventArgs e)
        {
            MainContentArea.Children.Clear();
            MainContentArea.Children.Add(new ManagePCsControl()); // cần tạo
        }

        private void BtnManageUsers_Click(object sender, RoutedEventArgs e)
        {
            MainContentArea.Children.Clear();
            MainContentArea.Children.Add(new ManageUsersControl()); // cần tạo
        }

        private void BtnManageServices_Click(object sender, RoutedEventArgs e)
        {
            MainContentArea.Children.Clear();
            MainContentArea.Children.Add(new ManageServicesControl()); // cần tạo
        }

        private void BtnRevenue_Click(object sender, RoutedEventArgs e)
        {
            MainContentArea.Children.Clear();
            MainContentArea.Children.Add(new RevenueControl()); // cần tạo
        }

        private void BtnManageStaff_Click(object sender, RoutedEventArgs e)
        {
            MainContentArea.Children.Clear();
            MainContentArea.Children.Add(new ManageStaffControl()); // cần tạo
        }

        private void BtnActivityLog_Click(object sender, RoutedEventArgs e)
        {
            MainContentArea.Children.Clear();
            MainContentArea.Children.Add(new ActivityLogControl()); // cần tạo
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            var loginView = new LoginView();
            loginView.Show();
            this.Close();
        }
    }
    }
