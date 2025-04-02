using System;
using System.Data.Entity;
using System.Windows;
using System.Windows.Controls;

namespace AppAuthorization
{
    public partial class LoginPage : Page
    {
        private MainWindow mainWindow;

        public LoginPage()
        {
            InitializeComponent();
            mainWindow = (MainWindow)Application.Current.MainWindow;
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text.Trim();
            string password = PasswordBox.Password;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Введите имя пользователя и пароль.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                using (var context = new AppDbContext())
                {
                    var user = await context.Users
                        .Include(u => u.Role)
                        .FirstOrDefaultAsync(u => u.Username == username);
                    
                    if (user == null)
                    {
                        MessageBox.Show("Пользователь не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    bool isPasswordValid = PasswordHelper.VerifyPassword(password, user.PasswordHash, user.Salt);
                    if (!isPasswordValid)
                    {
                        MessageBox.Show("Неверный пароль.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    MessageBox.Show($"Вход выполнен успешно. Вы вошли как {user.Role.RoleName}.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    
                    // Navigate to HomePage
                    mainWindow.NavigateToPage(new HomePage());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при авторизации: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.NavigateToPage(new RegistrationPage());
        }
    }
}
