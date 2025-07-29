using BusinessObjects.Models;
using DataAccessLayer;
using Repositories;
using System;
using System.Windows;

namespace NetCafeManagement_WDF.AdminControl
{
    public partial class AddComputerWindow : Window
    {
        private readonly ComputerRepository _computerRepo;

        public AddComputerWindow()
        {
            InitializeComponent();
            _computerRepo = new ComputerRepository(new NetCafeContext());
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string name = NameTextBox.Text.Trim();
            string tier = TierComboBox.Text;
            if (!decimal.TryParse(HourlyRateTextBox.Text, out decimal hourlyRate))
            {
                MessageBox.Show("Giá mỗi giờ không hợp lệ.");
                return;
            }

            var computer = new Computer
            {
                Name = name,
                Tier = tier,
                HourlyRate = hourlyRate,
                Status = 1
            };

            _computerRepo.Add(computer);
            MessageBox.Show("Đã thêm máy thành công.");
            this.DialogResult = true;
            this.Close();
        }
    }
}