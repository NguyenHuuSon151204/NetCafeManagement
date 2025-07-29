using BusinessObjects.Models;
using Repositories;
using DataAccessLayer;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using NetCafeManagement_WDF.ViewModels;

namespace NetCafeManagement_WDF.AdminControl
{
    public partial class ManagePCsControl : UserControl
    {
        private readonly ComputerRepository _computerRepo;

        public ManagePCsControl()
        {
            InitializeComponent();
            var context = new NetCafeContext();
            _computerRepo = new ComputerRepository(context);
            LoadComputers();
        }

        private void LoadComputers()
        {
            var computers = _computerRepo.GetAll();
            var displayList = computers.Select(c => new ComputerViewModel
            {
                ComputerId = c.ComputerId,
                Name = c.Name,
                Tier = c.Tier,
                HourlyRate = c.HourlyRate,
                Status = c.Status
            }).ToList();

            PCsDataGrid.ItemsSource = displayList;
        }




        private string GetStatusText(byte? status)
        {
            return status switch
            {
                1 => "Rảnh",
                2 => "Đang sử dụng",
                3 => "Bảo trì",
                _ => "Không xác định"
            };
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddComputerWindow();
            if (addWindow.ShowDialog() == true)
            {
                LoadComputers();
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var selected = PCsDataGrid.SelectedItem as ComputerViewModel;
            if (selected == null)
            {
                MessageBox.Show("Vui lòng chọn một máy để sửa");
                return;
            }

            // Lấy máy trạm gốc từ DB để sửa
            var computer = _computerRepo.GetById(selected.ComputerId);
            if (computer == null)
            {
                MessageBox.Show("Không tìm thấy máy trong cơ sở dữ liệu.");
                return;
            }

            // Mở cửa sổ chỉnh sửa
            var editWindow = new EditComputerWindow(computer);
            if (editWindow.ShowDialog() == true)
            {
                LoadComputers();
                MessageBox.Show("Đã cập nhật máy thành công.");
            }
        }





        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var selected = PCsDataGrid.SelectedItem as ComputerViewModel;
            if (selected == null)
            {
                MessageBox.Show("Vui lòng chọn một máy để xoá", "Thông báo");
                return;
            }

            var result = MessageBox.Show("Bạn có chắc muốn xoá máy này?", "Xác nhận", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                var computer = _computerRepo.GetById(selected.ComputerId);
                if (computer != null)
                {
                    _computerRepo.Delete(computer);
                    LoadComputers(); // Làm mới danh sách
                    MessageBox.Show("Đã xoá máy thành công!", "Thông báo");
                }
                else
                {
                    MessageBox.Show("Không tìm thấy máy để xoá.", "Lỗi");
                }
            }
        }





        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            LoadComputers();
            MessageBox.Show("Đã làm mới dữ liệu", "Thông báo");
        }
    }
}