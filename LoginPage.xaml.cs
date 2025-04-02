using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace AppAuthorization
{
    public partial class LoginPage : Page
    {
        private MainWindow? mainWindow;
        private AppDbContext _context;

        public LoginPage()
        {
            InitializeComponent();
            _context = new AppDbContext();
            
            Loaded += LoginPage_Loaded;
        }
        
        private void LoginPage_Loaded(object sender, RoutedEventArgs e)
        {
            mainWindow = Window.GetWindow(this) as MainWindow;
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text.Trim();
            string password = PasswordBox.Password;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ErrorTextBlock.Text = "Введите имя пользователя и пароль.";
                ErrorTextBlock.Visibility = Visibility.Visible;
                return;
            }

            try
            {
                var user = await _context.Users
                    .Include(u => u.Role)
                    .FirstOrDefaultAsync(u => u.Username == username);
                
                if (user == null)
                {
                    ErrorTextBlock.Text = "Пользователь не найден.";
                    ErrorTextBlock.Visibility = Visibility.Visible;
                    return;
                }

                bool isPasswordValid = PasswordHelper.VerifyPassword(password, user.PasswordHash, user.Salt);
                if (!isPasswordValid)
                {
                    ErrorTextBlock.Text = "Неверный пароль.";
                    ErrorTextBlock.Visibility = Visibility.Visible;
                    return;
                }

                // Successful login - create new MainWindow with user
                MainWindow newMainWindow = new MainWindow(user);
                newMainWindow.Show();
                
                // Close the current MainWindow with login page
                if (Window.GetWindow(this) is Window currentWindow)
                {
                    currentWindow.Close();
                }
            }
            catch (Exception ex)
            {
                ErrorTextBlock.Text = $"Ошибка при авторизации: {ex.Message}";
                ErrorTextBlock.Visibility = Visibility.Visible;
            }
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            if (mainWindow != null)
            {
                mainWindow.NavigateToPage(new RegistrationPage());
            }
        }
    }
}
