using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace AppAuthorization
{
    public partial class MainWindow : Window
    {
        private readonly AppDbContext _context;
        private readonly User _currentUser;
        private readonly DispatcherTimer _timer;

        public MainWindow(User user)
        {
            InitializeComponent();
            _context = new AppDbContext();
            _currentUser = user;
            
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _timer.Tick += Timer_Tick;
            _timer.Start();
            
            InitializeUI();
            LoadDashboardData();
        }

        private void InitializeUI()
        {
            txtStatusUser.Text = $"Пользователь: {_currentUser.FullName}";
            txtStatusRole.Text = $"Роль: {_currentUser.Role.RoleName}";
            
            switch (_currentUser.Role.RoleName)
            {
                case "Admin":
                    EnableAllMenuItems();
                    break;
                case "Seller":
                    menuProducts.IsEnabled = true;
                    menuOrders.IsEnabled = true;
                    menuPayments.IsEnabled = true;
                    menuReports.IsEnabled = true;
                    break;
                case "Buyer":
                    menuOrders.IsEnabled = true;
                    break;
            }
        }

        private void EnableAllMenuItems()
        {
            menuOrders.IsEnabled = true;
            menuProducts.IsEnabled = true;
            menuPayments.IsEnabled = true;
            menuReports.IsEnabled = true;
            menuUsers.IsEnabled = true;
        }

        private void LoadDashboardData()
        {
            try
            {
                // Summary statistics
                txtOrderCount.Text = _context.Orders.Count().ToString();
                txtTotalSales.Text = $"{_context.Payments.Sum(p => p.Amount):N2} ₽";
                txtTotalCommissions.Text = $"{_context.Commissions.Sum(c => c.Amount):N2} ₽";
                txtUserCount.Text = _context.Users.Count().ToString();

                // Load orders based on user role
                if (_currentUser.Role.RoleName == "Admin")
                {
                    dgOrders.ItemsSource = _context.Orders
                        .OrderByDescending(o => o.OrderDate)
                        .Take(10)
                        .ToList();
                    
                    dgPayments.ItemsSource = _context.Payments
                        .OrderByDescending(p => p.PaymentDate)
                        .Take(10)
                        .ToList();
                }
                else if (_currentUser.Role.RoleName == "Seller")
                {
                    dgOrders.ItemsSource = _context.Orders
                        .Where(o => o.SellerId == _currentUser.Id)
                        .OrderByDescending(o => o.OrderDate)
                        .Take(10)
                        .ToList();
                    
                    dgPayments.ItemsSource = _context.Payments
                        .Where(p => p.Order.SellerId == _currentUser.Id)
                        .OrderByDescending(p => p.PaymentDate)
                        .Take(10)
                        .ToList();
                }
                else if (_currentUser.Role.RoleName == "Buyer")
                {
                    dgOrders.ItemsSource = _context.Orders
                        .Where(o => o.BuyerId == _currentUser.Id)
                        .OrderByDescending(o => o.OrderDate)
                        .Take(10)
                        .ToList();
                    
                    dgPayments.ItemsSource = _context.Payments
                        .Where(p => p.Order.BuyerId == _currentUser.Id)
                        .OrderByDescending(p => p.PaymentDate)
                        .Take(10)
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                // Log error instead of showing MessageBox to avoid interrupting user workflow
                Console.WriteLine($"Error loading dashboard data: {ex.Message}");
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            txtStatusDateTime.Text = $"Время: {DateTime.Now:dd.MM.yyyy HH:mm:ss}";
        }

        private void MenuExit_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите выйти из системы?", "Подтверждение", 
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                LoginWindow loginWindow = new LoginWindow();
                loginWindow.Show();
                this.Close();
            }
        }

        private void MenuOrders_Click(object sender, RoutedEventArgs e)
        {
            // Open Orders management in a new tab or window
            MessageBox.Show("Функционал управления заказами в разработке", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void MenuProducts_Click(object sender, RoutedEventArgs e)
        {
            // Open Products management in a new tab or window
            MessageBox.Show("Функционал управления товарами в разработке", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void MenuPayments_Click(object sender, RoutedEventArgs e)
        {
            // Open Payments management in a new tab or window
            MessageBox.Show("Функционал управления платежами в разработке", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void MenuReports_Click(object sender, RoutedEventArgs e)
        {
            // Open Reports in a new tab or window
            MessageBox.Show("Функционал отчетов в разработке", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void MenuUsers_Click(object sender, RoutedEventArgs e)
        {
            // Open Users management in a new tab or window
            MessageBox.Show("Функционал управления пользователями в разработке", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void DgOrders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgOrders.SelectedItem is Order selectedOrder)
            {
                // Detail view or action could be implemented here
                // For now, just show basic info
                MessageBox.Show($"Детали заказа #{selectedOrder.OrderId}\n" +
                                $"Покупатель: {selectedOrder.Buyer.FullName}\n" +
                                $"Продавец: {selectedOrder.Seller.FullName}\n" +
                                $"Сумма: {selectedOrder.TotalAmount:N2} ₽\n" +
                                $"Статус: {selectedOrder.Status}",
                    "Информация о заказе", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
