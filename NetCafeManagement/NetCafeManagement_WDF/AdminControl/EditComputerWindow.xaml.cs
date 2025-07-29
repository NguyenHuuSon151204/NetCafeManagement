using BusinessObjects.Models;
using DataAccessLayer;
using Repositories;
using System;
using System.Windows;

namespace NetCafeManagement_WDF.AdminControl
{
    public partial class EditComputerWindow : Window
    {
        private readonly ComputerRepository _computerRepo;
        private readonly Computer _computer;

        public EditComputerWindow(Computer computer)
        {
            InitializeComponent();
            _computerRepo = new ComputerRepository(new NetCafeContext());
            _computer = computer;
            LoadData();
        }

        private void LoadData()
        {
            NameTextBox.Text = _computer.Name;
            TierComboBox.Text = _computer.Tier;
            HourlyRateTextBox.Text = _computer.HourlyRate.ToString();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            _computer.Name = NameTextBox.Text.Trim();
            _computer.Tier = TierComboBox.Text;

            if (!decimal.TryParse(HourlyRateTextBox.Text, out decimal hourlyRate))
            {
                MessageBox.Show("Giá mỗi giờ không hợp lệ.");
                return;
            }
            _computer.HourlyRate = hourlyRate;

            _computerRepo.Update(_computer);
            MessageBox.Show("Đã cập nhật máy thành công.");
            this.DialogResult = true;
            this.Close();
        }
    }
}