using BusinessObjects.Models;
using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using Services;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace NetCafeManagement_WDF.Views
{
    public partial class ComputerActionPopup : UserControl
    {
        public Computer SelectedComputer { get; }
        private readonly Action _onBack;
        private readonly DispatcherTimer _usageTimer = new DispatcherTimer();
        private DateTime _sessionStartTime;
        private decimal _lastDeductedAmount = 0;


        public ComputerActionPopup(Computer computer, Action onBack)
        {
            InitializeComponent();
            SelectedComputer = computer;
            _onBack = onBack;

            ComputerNameText.Text = computer.Name;
            ComputerTierText.Text = $"Tầng: {computer.Tier}";

            ComputerStatusText.Text = computer.Status switch
            {
                0 => "Trạng thái: 🟥 Tắt",
                1 => "Trạng thái: 🟢 Trống",
                2 => "Trạng thái: 🔴 Đang sử dụng",
                3 => "Trạng thái: 🛠 Bảo trì",
                _ => "Trạng thái: ❓ Không xác định"
            };
            using var context = new NetCafeContext();

            var activeSession = context.Sessions
                .Include(s => s.Customer)
                .FirstOrDefault(s => s.ComputerId == computer.ComputerId && s.EndTime == null);

            if (activeSession != null && activeSession.Customer != null)
            {
                SessionInfoPanel.Visibility = Visibility.Visible;

                CustomerNameText.Text = $"👤 Khách: {activeSession.Customer.Name}";
                CustomerBalanceText.Text = $"💰 Số dư: {activeSession.Customer.Balance:N0} VNĐ";
                SessionStartTimeText.Text = $"🕒 Bắt đầu: {activeSession.StartTime:HH:mm dd/MM/yyyy}";

                _sessionStartTime = activeSession.StartTime;

                UpdateUsageText();

                _usageTimer.Interval = TimeSpan.FromSeconds(30);
                _usageTimer.Tick += (s, e) => UpdateUsageText();
                _usageTimer.Start();

            }
            else
            {
                SessionInfoPanel.Visibility = Visibility.Collapsed;
            }

        }

        private void StartSession_Click(object sender, RoutedEventArgs e)
        {
            var selectWindow = new SelectCustomerWindow();
            if (selectWindow.ShowDialog() == true)
            {
                int customerId = selectWindow.SelectedCustomerId;

                try
                {
                    using var context = new NetCafeContext();

                    var customer = context.Customers.FirstOrDefault(c => c.CustomerId == customerId);
                    var computer = context.Computers.FirstOrDefault(c => c.ComputerId == SelectedComputer.ComputerId);

                    if (customer == null || computer == null)
                    {
                        MessageBox.Show("Không tìm thấy thông tin khách hoặc máy.");
                        return;
                    }

                    // ❌ Nếu máy đang dùng rồi (tồn tại phiên chưa kết thúc)
                    var existingSession = context.Sessions
                        .FirstOrDefault(s => s.ComputerId == computer.ComputerId && s.EndTime == null);

                    if (existingSession != null)
                    {
                        MessageBox.Show("Máy đang được sử dụng. Không thể bắt đầu phiên mới.");
                        return;
                    }

                    // ❌ Nếu máy đang tắt hoặc bảo trì
                    if (computer.Status == 0)
                    {
                        MessageBox.Show("Máy đang tắt. Không thể bắt đầu phiên.");
                        return;
                    }
                    if (computer.Status == 3)
                    {
                        MessageBox.Show("Máy đang bảo trì. Không thể bắt đầu phiên.");
                        return;
                    }

                    // Kiểm tra số dư tối thiểu
                    var minRequired = computer.HourlyRate / 12; // đủ ~5 phút
                    if (customer.Balance < minRequired)
                    {
                        MessageBox.Show($"Khách không đủ số dư (cần ít nhất {minRequired:N0} VNĐ) để bắt đầu phiên.");
                        return;
                    }

                    // ✅ Nếu đủ điều kiện, bắt đầu phiên
                    SessionService.StartSession(computer.ComputerId, customer.CustomerId);

                    MessageBox.Show($"Đã bắt đầu phiên. Đã nạp {selectWindow.RechargeAmount:#,##0} VNĐ nếu có.");
                    _onBack?.Invoke();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            }
        }



        private void OrderFood_Click(object sender, RoutedEventArgs e)
        {
            using var context = new NetCafeContext();

            var activeSession = context.Sessions
                .FirstOrDefault(s => s.ComputerId == SelectedComputer.ComputerId && s.EndTime == null);

            if (activeSession == null)
            {
                MessageBox.Show("Hiện tại máy chưa có phiên hoạt động nào.");
                return;
            }

            var orderWindow = new OrderFoodWindow(activeSession.SessionId);
            orderWindow.ShowDialog();

            _onBack?.Invoke(); // Reload nếu có thay đổi trạng thái máy
        }

        private void Recharge_Click(object sender, RoutedEventArgs e)
        {
            using var context = new NetCafeContext();

            var session = context.Sessions
                .FirstOrDefault(s => s.ComputerId == SelectedComputer.ComputerId && s.EndTime == null);

            if (session == null || session.CustomerId == null)
            {
                MessageBox.Show("Không tìm thấy khách đang sử dụng máy.");
                return;
            }

            var rechargeWindow = new RechargeWindow();
            if (rechargeWindow.ShowDialog() == true)
            {
                var customer = context.Customers.Find(session.CustomerId);
                if (customer != null)
                {
                    decimal amount = rechargeWindow.Amount;

                    customer.Balance += amount;
                    context.Transactions.Add(new Transaction
                    {
                        CustomerId = customer.CustomerId,
                        Amount = amount,
                        Type = "Deposit",
                        CreatedAt = DateTime.Now
                    });

                    context.SaveChanges();

                    MessageBox.Show($"Đã nạp {amount:N0} VNĐ cho {customer.Name}.");
                }
            }
        }


        private void EndSession_Click(object sender, RoutedEventArgs e)
        {
            using var context = new NetCafeContext();

            var session = context.Sessions
                .FirstOrDefault(s => s.ComputerId == SelectedComputer.ComputerId && s.EndTime == null);

            if (session == null)
            {
                MessageBox.Show("Không tìm thấy phiên hoạt động nào cho máy này.");
                return;
            }

            try
            {
                // Cập nhật EndTime
                session.EndTime = DateTime.Now;
                context.SaveChanges();

                // Gọi SP tính tổng tiền
                var result = context.Set<CalculateResult>()
                    .FromSqlRaw("EXEC sp_CalculateSessionTotal @p0", session.SessionId)
                    .AsEnumerable()
                    .FirstOrDefault();

                // Cập nhật máy về trạng thái Sẵn sàng
                var computer = context.Computers.FirstOrDefault(c => c.ComputerId == SelectedComputer.ComputerId);
                if (computer != null)
                {
                    computer.Status = 1;
                }

                // Trừ tiền khỏi tài khoản khách (nếu có)
                if (session.CustomerId != null && session.TotalAmount != null)
                {
                    var customer = context.Customers.FirstOrDefault(c => c.CustomerId == session.CustomerId);
                    if (customer != null)
                    {
                        customer.Balance -= session.TotalAmount.Value;

                        context.Transactions.Add(new Transaction
                        {
                            CustomerId = customer.CustomerId,
                            Amount = -session.TotalAmount.Value,
                            Type = "Withdraw",
                            CreatedAt = DateTime.Now
                        });
                    }
                }

                context.SaveChanges();

                // Hiển thị kết quả
                MessageBox.Show($"Đã kết thúc phiên.\n" +
                                $"Tiền máy: {result?.ComputerCharge:N0} VNĐ\n" +
                                $"Tiền đồ ăn: {result?.FoodCharge:N0} VNĐ\n" +
                                $"Tổng cộng: {(result?.ComputerCharge + result?.FoodCharge):N0} VNĐ");

                _onBack?.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi kết thúc phiên: " + ex.Message);
            }
        }




        private void Back_Click(object sender, RoutedEventArgs e)
        {
            _usageTimer.Stop(); // ⏹ Dừng cập nhật
            _onBack?.Invoke();  // Quay về ComputerStatusView
        }

        private void UpdateUsageText()
        {
            var now = DateTime.Now;
            var duration = now - _sessionStartTime;
            int hours = (int)duration.TotalHours;
            int minutes = duration.Minutes;

            string formatted = hours > 0
                ? $"⌛ Đã dùng: {hours} giờ {minutes} phút"
                : $"⌛ Đã dùng: {minutes} phút";

            SessionUsageTimeText.Text = formatted;

            // ⚠️ Tính tiền thực tế và trừ vào balance
            using var context = new NetCafeContext();

            var session = context.Sessions
                .Include(s => s.Customer)
                .Include(s => s.Computer)
                .FirstOrDefault(s => s.ComputerId == SelectedComputer.ComputerId && s.EndTime == null);

            if (session == null || session.Customer == null || session.Computer == null) return;

            // Tính tiền tính đến hiện tại
            var minutesUsed = (decimal)duration.TotalMinutes;
            var hourlyRate = session.Computer.HourlyRate;
            var totalCost = minutesUsed / 60 * hourlyRate;

            var costToChargeNow = Math.Round(totalCost - _lastDeductedAmount, 0); // phần mới

            if (costToChargeNow >= 1000) // chỉ trừ nếu >= 1000đ để tránh spam DB
            {
                session.Customer.Balance -= costToChargeNow;
                _lastDeductedAmount += costToChargeNow;

                context.Transactions.Add(new Transaction
                {
                    CustomerId = session.Customer.CustomerId,
                    Amount = -costToChargeNow,
                    Type = "Withdraw",
                    CreatedAt = now
                });

                context.SaveChanges();

                // Cập nhật lại Balance hiển thị
                CustomerBalanceText.Text = $"💰 Số dư: {session.Customer.Balance:N0} VNĐ";

                // Nếu hết tiền → kết thúc phiên tự động
                if (session.Customer.Balance <= 0)
                {
                    MessageBox.Show("Khách đã hết tiền. Phiên sẽ kết thúc tự động.");

                    session.EndTime = now;
                    session.TotalAmount = _lastDeductedAmount;
                    session.Computer.Status = 1;
                    context.SaveChanges();

                    _usageTimer.Stop();
                    _onBack?.Invoke();
                }
            }
        }
    }
}
