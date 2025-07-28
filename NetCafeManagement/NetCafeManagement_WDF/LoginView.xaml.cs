using BusinessObjects.Models;
using DataAccessLayer;
using Services;
using System.Windows;
using System.Diagnostics;

namespace NetCafeManagement_WDF
{
    public partial class LoginView : Window
    {
        private readonly AccountService _accountService;

        public LoginView()
        {
            InitializeComponent();
            var context = new NetCafeContext();
            _accountService = new AccountService(context);
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text.Trim();
            string password = PasswordBox.Password.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both username and password.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var account = _accountService.ValidateLogin(username, password);

            if (account != null)
            {
                if (account.Employee != null && account.Employee.Role != null)
                {
                    string role = account.Employee.Role.Trim().ToLower();

                    if (role == "admin")
                    {
                        new AdminDashboardView().Show();
                        this.Close();
                    }
                    else if (role == "staff")
                    {
                        new StaffDashboardView().Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Unknown role detected. Please contact administrator.", "Role Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("This account is not linked to any employee or missing role info.", "Login Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                Debug.WriteLine("== LOGIN FAILED ==");
                Debug.WriteLine("Username nhập: " + username);
                Debug.WriteLine("Password nhập: " + password);
                Debug.WriteLine("Không tìm thấy tài khoản hợp lệ hoặc mật khẩu sai.");

                MessageBox.Show("Invalid username or password.", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
