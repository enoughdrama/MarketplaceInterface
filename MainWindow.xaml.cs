using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace AppAuthorization
{
    public partial class MainWindow : Window
    {
        private readonly AppDbContext _context;
        public User? CurrentUser { get; private set; }
        private DispatcherTimer? _timer;

        public MainWindow()
        {
            InitializeComponent();
            _context = new AppDbContext();
            
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        public MainWindow(User user)
        {
            InitializeComponent();
            _context = new AppDbContext();
            CurrentUser = user;
            
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _timer.Tick += Timer_Tick;
            _timer.Start();
            
            InitializeDashboard();
            UpdateMenuPermissions();
            
            NavigateToPage(new HomePage());
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            txtStatusDateTime.Text = $"Время: {DateTime.Now:HH:mm:ss}";
            
            if (CurrentUser != null)
            {
                txtStatusUser.Text = $"Пользователь: {CurrentUser.Username}";
                txtStatusRole.Text = $"Роль: {CurrentUser.Role?.RoleName ?? "Не задана"}";
            }
        }

        private void UpdateMenuPermissions()
        {
            if (CurrentUser == null || CurrentUser.Role == null)
                return;
                
            string roleName = CurrentUser.Role.RoleName;
            
            // Enable menu items based on role
            menuOrders.IsEnabled = roleName == "Admin" || roleName == "Seller";
            menuProducts.IsEnabled = roleName == "Admin" || roleName == "Seller";
            menuPayments.IsEnabled = roleName == "Admin";
            menuReports.IsEnabled = roleName == "Admin";
            menuUsers.IsEnabled = roleName == "Admin";
            
            // Show dashboard for admin and seller
            if (roleName == "Admin" || roleName == "Seller")
            {
                MainFrame.Visibility = Visibility.Collapsed;
                mainTabControl.Visibility = Visibility.Visible;
            }
            else
            {
                MainFrame.Visibility = Visibility.Visible;
                mainTabControl.Visibility = Visibility.Collapsed;
            }
        }

        private void InitializeDashboard()
        {
            try
            {
                // Load dashboard data
                var orderCount = _context.Orders.Count();
                var totalSales = _context.Payments.Where(p => p.Status == "Completed").Sum(p => p.Amount);
                var commissions = _context.Commissions.Sum(c => c.Amount);
                var userCount = _context.Users.Count();
                
                // Update UI
                txtOrderCount.Text = orderCount.ToString();
                txtTotalSales.Text = $"{totalSales:N0} ₽";
                txtTotalCommissions.Text = $"{commissions:N0} ₽";
                txtUserCount.Text = userCount.ToString();
                
                // Load latest orders
                dgOrders.ItemsSource = _context.Orders
                    .Include(o => o.Buyer)
                    .OrderByDescending(o => o.OrderDate)
                    .Take(10)
                    .ToList();
                
                // Load latest payments
                dgPayments.ItemsSource = _context.Payments
                    .Include(p => p.PaymentMethod)
                    .OrderByDescending(p => p.PaymentDate)
                    .Take(10)
                    .ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void NavigateToPage(Page page)
        {
            MainFrame.Navigate(page);
        }
        
        private void MenuExit_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите выйти из программы?", "Выход", 
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }
        
        private void MenuOrders_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to orders page - can be implemented later
            MessageBox.Show("Функционал в разработке", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        
        private void MenuProducts_Click(object sender, RoutedEventArgs e)
        {
            NavigateToPage(new ProductManagementPage());
        }
        
        private void MenuPayments_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to payments page - can be implemented later
            MessageBox.Show("Функционал в разработке", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        
        private void MenuReports_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to reports page - can be implemented later
            MessageBox.Show("Функционал в разработке", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        
        private void MenuUsers_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to users management page - can be implemented later
            MessageBox.Show("Функционал в разработке", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        
        private void DgOrders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgOrders.SelectedItem is Order selectedOrder)
            {
                // Show order details - can be implemented later
                // MessageBox.Show($"Выбран заказ: {selectedOrder.OrderId}", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
