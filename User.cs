using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AppAuthorization
{
    public partial class RegisterWindow : Window
    {
        private readonly AppDbContext _context;
        private readonly SolidColorBrush _validColor = new SolidColorBrush(Colors.Green);
        private readonly SolidColorBrush _invalidColor = new SolidColorBrush(Colors.Gray);

        public RegisterWindow()
        {
            InitializeComponent();
            _context = new AppDbContext();
            LoadRoles();
        }

        private void LoadRoles()
        {
            var roles = _context.Roles.Where(r => r.RoleName != "Admin").ToList();
            cmbRole.ItemsSource = roles;
            
            if (roles.Count > 0)
                cmbRole.SelectedIndex = 0;
        }

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string username = txtUsername.Text.Trim();
                string firstName = txtFirstName.Text.Trim();
                string lastName = txtLastName.Text.Trim();
                string email = txtEmail.Text.Trim();
                string password = pwdPassword.Password;
                string confirmPassword = pwdConfirmPassword.Password;

                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(firstName) || 
                    string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(email) || 
                    string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
                {
                    txtError.Text = "Все поля обязательны для заполнения.";
                    txtError.Visibility = Visibility.Visible;
                    return;
                }

                if (cmbRole.SelectedItem == null)
                {
                    txtError.Text = "Выберите роль пользователя.";
                    txtError.Visibility = Visibility.Visible;
                    return;
                }

                if (!IsValidPassword(password))
                {
                    txtError.Text = "Пароль не соответствует требованиям безопасности.";
                    txtError.Visibility = Visibility.Visible;
                    return;
                }

                if (password != confirmPassword)
                {
                    txtError.Text = "Пароли не совпадают.";
                    txtError.Visibility = Visibility.Visible;
                    return;
                }

                if (!IsValidEmail(email))
                {
                    txtError.Text = "Неверный формат электронной почты.";
                    txtError.Visibility = Visibility.Visible;
                    return;
                }

                if (_context.Users.Any(u => u.Username == username))
                {
                    txtError.Text = "Пользователь с таким именем уже существует.";
                    txtError.Visibility = Visibility.Visible;
                    return;
                }

                if (_context.Users.Any(u => u.Email == email))
                {
                    txtError.Text = "Пользователь с таким email уже существует.";
                    txtError.Visibility = Visibility.Visible;
                    return;
                }

                PasswordHelper.CreatePasswordHash(password, out string passwordHash, out string salt);

                var newUser = new User
                {
                    Username = username,
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    PasswordHash = passwordHash,
                    Salt = salt,
                    RoleId = (int)cmbRole.SelectedValue,
                    RegistrationDate = DateTime.Now,
                    IsActive = true
                };

                _context.Users.Add(newUser);
                _context.SaveChanges();

                txtError.Visibility = Visibility.Collapsed;
                LoginWindow loginWindow = new LoginWindow();
                loginWindow.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                txtError.Text = $"Произошла ошибка при регистрации: {ex.Message}";
                txtError.Visibility = Visibility.Visible;
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }

        private void PwdPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            ValidatePassword();
        }

        private void PwdConfirmPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            ValidatePassword();
        }

        private void ValidatePassword()
        {
            string password = pwdPassword.Password;
            string confirmPassword = pwdConfirmPassword.Password;

            txtMinLength.Foreground = password.Length >= 8 ? _validColor : _invalidColor;
            txtUpperCase.Foreground = Regex.IsMatch(password, "[A-Z]") ? _validColor : _invalidColor;
            txtLowerCase.Foreground = Regex.IsMatch(password, "[a-z]") ? _validColor : _invalidColor;
            txtDigit.Foreground = Regex.IsMatch(password, "[0-9]") ? _validColor : _invalidColor;
            txtSpecialChar.Foreground = Regex.IsMatch(password, "[！@#$%^&*]") ? _validColor : _invalidColor;
            txtPasswordsMatch.Foreground = !string.IsNullOrEmpty(confirmPassword) && password == confirmPassword ? _validColor : _invalidColor;
        }

        private bool IsValidPassword(string password)
        {
            return password.Length >= 8 &&
                   Regex.IsMatch(password, "[A-Z]") &&
                   Regex.IsMatch(password, "[a-z]") &&
                   Regex.IsMatch(password, "[0-9]") &&
                   Regex.IsMatch(password, "[！@#$%^&*]");
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
