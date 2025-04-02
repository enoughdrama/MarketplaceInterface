using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Linq;

namespace AppAuthorization
{
    public partial class RegistrationPage : Page
    {
        private MainWindow mainWindow;
        private AppDbContext _context;
        private SolidColorBrush _validBrush = new SolidColorBrush(Colors.Green);
        private SolidColorBrush _invalidBrush = new SolidColorBrush(Color.FromRgb(85, 85, 85));

        public RegistrationPage()
        {
            InitializeComponent();
            mainWindow = (MainWindow)Application.Current.MainWindow;
            _context = new AppDbContext();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.NavigateToPage(new LoginPage());
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text.Trim();
            string password = PasswordBox.Password;
            string confirmPassword = ConfirmPasswordBox.Password;

            if (string.IsNullOrEmpty(username))
            {
                ErrorTextBlock.Text = "Введите имя пользователя.";
                ErrorTextBlock.Visibility = Visibility.Visible;
                return;
            }

            if (_context.Users.Any(u => u.Username == username))
            {
                ErrorTextBlock.Text = "Пользователь с таким именем уже существует.";
                ErrorTextBlock.Visibility = Visibility.Visible;
                return;
            }

            if (!IsPasswordValid(password))
            {
                ErrorTextBlock.Text = "Пароль не соответствует требованиям.";
                ErrorTextBlock.Visibility = Visibility.Visible;
                return;
            }

            if (password != confirmPassword)
            {
                ErrorTextBlock.Text = "Пароли не совпадают.";
                ErrorTextBlock.Visibility = Visibility.Visible;
                return;
            }

            try
            {
                PasswordHelper.CreatePasswordHash(password, out string passwordHash, out string salt);

                User newUser = new User
                {
                    Username = username,
                    PasswordHash = passwordHash,
                    Salt = salt,
                    FirstName = username, // Default values for demo
                    LastName = "User",
                    Email = $"{username}@example.com",
                    RoleId = 3, // Default to Buyer role
                    RegistrationDate = DateTime.Now,
                    IsActive = true
                };

                _context.Users.Add(newUser);
                _context.SaveChanges();

                // Successful registration - navigate to login page
                mainWindow.NavigateToPage(new LoginPage());
            }
            catch (Exception ex)
            {
                ErrorTextBlock.Text = $"Ошибка при регистрации: {ex.Message}";
                ErrorTextBlock.Visibility = Visibility.Visible;
            }
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            UpdatePasswordValidation();
        }

        private void ConfirmPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            UpdatePasswordValidation();
        }

        private void UpdatePasswordValidation()
        {
            string password = PasswordBox.Password;
            
            // Check min length
            MinLengthTextBlock.Foreground = password.Length >= 8 ? _validBrush : _invalidBrush;
            
            // Check for letter
            HasLetterTextBlock.Foreground = Regex.IsMatch(password, "[a-zA-Z]") ? _validBrush : _invalidBrush;
            
            // Check for digit
            HasDigitTextBlock.Foreground = Regex.IsMatch(password, "[0-9]") ? _validBrush : _invalidBrush;
            
            // Check for special character
            HasSpecialTextBlock.Foreground = Regex.IsMatch(password, "[！@#$%^&*]") ? _validBrush : _invalidBrush;
            
            // Hide error message when typing
            ErrorTextBlock.Visibility = Visibility.Collapsed;
        }

        private bool IsPasswordValid(string password)
        {
            return password.Length >= 8 &&
                   Regex.IsMatch(password, "[a-zA-Z]") &&
                   Regex.IsMatch(password, "[0-9]") &&
                   Regex.IsMatch(password, "[！@#$%^&*]");
        }
    }
}
