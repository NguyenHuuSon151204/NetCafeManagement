using NetCafeManagement_WDF.Views;
using System.Windows;
using System.Windows.Controls;

namespace NetCafeManagement_WDF
{
    public partial class StaffDashboardView : Window
    {
        public StaffDashboardView()
        {
            InitializeComponent();
        }


        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            // Quay về màn hình đăng nhập
            var loginView = new LoginView();
            loginView.Show();
            this.Close();
        }

        private void BtnViewComputers_Click(object sender, RoutedEventArgs e)
        {
            MainContentArea.Children.Clear();
            var view = new ComputerStatusView();
            MainContentArea.Children.Add(view);
        }

        private void BtnStartSession_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnOrderFood_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnRecharge_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnEndSession_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
