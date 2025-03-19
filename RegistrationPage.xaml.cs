using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Data.Entity;
using System.Text.RegularExpressions;

namespace AppAuthorization
{
    public partial class RegistrationPage : Page
    {
        private MainWindow mainWindow;

        public RegistrationPage()
        {
            InitializeComponent();
            mainWindow = (MainWindow)Application.Current.MainWindow;
        }

        private bool IsValidPassword(string password, out string error)
        {
            error = string.Empty;

            if (password.Length < 8)
            {
                error = "Пароль должен содержать не менее 8 символов.";
                return false;
            }

            string allowedPattern = @"^[A-Za-z!@#$%^&*()_+\-=\[\]{};':""\\|,.<>\/?]+$";
            if (!Regex.IsMatch(password, allowedPattern))
            {
                error = "Пароль может содержать только английские буквы и спецсимволы.";
                return false;
            }

            if (!Regex.IsMatch(password, @"[A-Za-z]"))
            {
                error = "Пароль должен содержать хотя бы одну английскую букву.";
                return false;
            }

            if (!Regex.IsMatch(password, @"[!@#$%^&*()_+\-=\[\]{};':""\\|,.<>\/?]"))
            {
                error = "Пароль должен содержать хотя бы один спецсимвол.";
                return false;
            }

            return true;
        }

        private async void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text.Trim();
            string password = PasswordBox.Password;
            string confirmPassword = ConfirmPasswordBox.Password;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
            {
                MessageBox.Show("Заполните все поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Пароли не совпадают.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!IsValidPassword(password, out string passwordError))
            {
                MessageBox.Show(passwordError, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                using (var context = new AppDbContext())
                {
                    var existingUser = await context.Users.FirstOrDefaultAsync(u => u.Username == username);
                    if (existingUser != null)
                    {
                        MessageBox.Show("Пользователь с таким именем уже существует.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    PasswordHelper.CreatePasswordHash(password, out string passwordHash, out string salt);

                    var newUser = new User
                    {
                        Username = username,
                        PasswordHash = passwordHash,
                        Salt = salt
                    };

                    context.Users.Add(newUser);
                    await context.SaveChangesAsync();

                    MessageBox.Show("Регистрация успешна.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    
                    // Navigate to LoginPage
                    mainWindow.NavigateToPage(new LoginPage());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при регистрации: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.NavigateToPage(new LoginPage());
        }
    }
}
