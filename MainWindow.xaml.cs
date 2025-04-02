using System;
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
            NavigateToPage(new WelcomePage());
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
            
            NavigateToPage(new HomePage());
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            // Update status bar or any time-dependent UI elements
        }

        public void NavigateToPage(Page page)
        {
            MainFrame.Navigate(page);
        }
    }
}
