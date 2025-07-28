using BusinessObjects.Models;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace NetCafeManagement_WDF.Views
{
    public partial class OrderFoodWindow : Window
    {
        private readonly int _sessionId;
        private List<OrderItem> _orderItems;

        public OrderFoodWindow(int sessionId)
        {
            InitializeComponent();
            _sessionId = sessionId;
            LoadProducts();
        }

        private void LoadProducts()
        {
            using var context = new NetCafeContext();

            _orderItems = context.Products
                .Where(p => p.IsActive == true)
                .Select(p => new OrderItem
                {
                    ProductId = p.ProductId,
                    Name = p.Name,
                    Category = p.Category,
                    Price = p.Price,
                    Quantity = 0
                }).ToList();

            ProductGrid.ItemsSource = _orderItems;

            foreach (var item in _orderItems)
            {
                item.PropertyChanged += (_, _) => UpdateTotal();
            }

            UpdateTotal();
        }

        private void UpdateTotal()
        {
            decimal total = _orderItems.Sum(p => p.Price * p.Quantity);
            TotalAmountText.Text = total.ToString("N0") + " VNĐ";
        }

        private void ConfirmOrder_Click(object sender, RoutedEventArgs e)
        {
            var selectedItems = _orderItems.Where(p => p.Quantity > 0).ToList();
            if (!selectedItems.Any())
            {
                MessageBox.Show("Vui lòng chọn ít nhất 1 món.");
                return;
            }

            using var context = new NetCafeContext();

            var order = new Order
            {
                SessionId = _sessionId,
                OrderTime = DateTime.Now,
                TotalAmount = selectedItems.Sum(p => p.Price * p.Quantity)
            };
            context.Orders.Add(order);
            context.SaveChanges();

            foreach (var item in selectedItems)
            {
                var detail = new OrderDetail
                {
                    OrderId = order.OrderId,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.Price
                };
                context.OrderDetails.Add(detail);
            }
            context.SaveChanges();

            MessageBox.Show("Đã ghi nhận đặt món.");
            this.Close();
        }
    }

    public class OrderItem : INotifyPropertyChanged
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public decimal Price { get; set; }

        private int _quantity;
        public int Quantity
        {
            get => _quantity;
            set
            {
                if (_quantity != value)
                {
                    _quantity = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Quantity)));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}