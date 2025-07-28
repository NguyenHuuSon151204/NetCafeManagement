using BusinessObjects.Models;
using Services;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using NetCafeManagement_WDF.Views; // <-- Thêm dòng này để dùng ComputerActionPopup

namespace NetCafeManagement_WDF.Views
{
    public partial class ComputerStatusView : UserControl
    {
        private readonly ComputerService _computerService;

        public ComputerStatusView()
        {
            InitializeComponent();
            _computerService = new ComputerService(new DataAccessLayer.NetCafeContext());

            LoadComputerCards();
        }

        private void LoadComputerCards()
        {
            List<Computer> computers = _computerService.GetAllComputers();

            ComputerPanel.Children.Clear();

            foreach (var comp in computers)
            {
                // Tùy chỉnh màu nền và trạng thái text
                Brush background;
                string statusText;

                switch (comp.Status)
                {
                    case 0:
                        background = Brushes.Gray;
                        statusText = "⚫ Đang tắt";
                        break;
                    case 1:
                        background = Brushes.LightGreen;
                        statusText = "🟢 Sẵn sàng";
                        break;
                    case 2:
                        background = Brushes.IndianRed;
                        statusText = "🔴 Đang sử dụng";
                        break;
                    case 3:
                        background = Brushes.Goldenrod;
                        statusText = "🛠️ Bảo trì";
                        break;
                    default:
                        background = Brushes.DarkGray;
                        statusText = "❓ Không rõ";
                        break;
                }

                var stack = new StackPanel
                {
                    Margin = new Thickness(10),
                    Children =
                    {
                        new TextBlock
                        {
                            Text = comp.Name,
                            FontSize = 18,
                            FontWeight = FontWeights.Bold,
                            Foreground = Brushes.White
                        },
                        new TextBlock
                        {
                            Text = $"Tầng: {comp.Tier}",
                            Foreground = Brushes.White,
                            Margin = new Thickness(0, 5, 0, 0)
                        },
                        new TextBlock
                        {
                            Text = statusText,
                            Foreground = Brushes.White,
                            Margin = new Thickness(0, 5, 0, 0)
                        }
                    }
                };

                var border = new Border
                {
                    Width = 180,
                    Height = 140,
                    Margin = new Thickness(10),
                    Background = background,
                    CornerRadius = new CornerRadius(10),
                    Effect = new DropShadowEffect
                    {
                        Color = Colors.Black,
                        BlurRadius = 10,
                        Direction = 270,
                        ShadowDepth = 4,
                        Opacity = 0.2
                    },
                    Child = stack
                };

                // 👇 Gắn sự kiện click để hiển thị popup cho từng máy
                border.MouseLeftButtonDown += (s, e) =>
                {
                    ShowComputerOptions(comp);
                };

                ComputerPanel.Children.Add(border);
            }
        }

        // 👉 Hàm mở popup
        private void ShowComputerOptions(Computer comp)
{
    // Tìm cha là Grid (MainContentArea)
    var parent = this.Parent as Grid;
    if (parent == null) return;

    parent.Children.Clear();

    // Tạo popup kèm nút quay lại
    var popup = new ComputerActionPopup(comp, () =>
    {
        // Khi gọi quay lại thì load lại ComputerStatusView
        parent.Children.Clear();
        parent.Children.Add(new ComputerStatusView());
    });

    parent.Children.Add(popup);
}

    }
}
