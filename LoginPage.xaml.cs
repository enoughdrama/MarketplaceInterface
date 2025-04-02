using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace AppAuthorization
{
    public partial class LoginWindow : Window
    {
        private readonly AppDbContext _context;

        public LoginWindow()
        {
            InitializeComponent();
            _context = new AppDbContext();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = pwdPassword.Password;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                txtError.Text = "Пожалуйста, введите имя пользователя и пароль.";
                txtError.Visibility = Visibility.Visible;
                return;
            }

            var user = _context.Users.FirstOrDefault(u => u.Username == username);

            if (user == null)
            {
                txtError.Text = "Пользователь не найден.";
                txtError.Visibility = Visibility.Visible;
                return;
            }

            if (!user.IsActive)
            {
                txtError.Text = "Аккаунт деактивирован. Обратитесь к администратору.";
                txtError.Visibility = Visibility.Visible;
                return;
            }

            bool isPasswordValid = PasswordHelper.VerifyPassword(password, user.PasswordHash, user.Salt);

            if (!isPasswordValid)
            {
                txtError.Text = "Неверный пароль.";
                txtError.Visibility = Visibility.Visible;
                return;
            }

            txtError.Visibility = Visibility.Collapsed;
            
            MainWindow mainWindow = new MainWindow(user);
            mainWindow.Show();
            this.Close();
        }

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindow registerWindow = new RegisterWindow();
            registerWindow.Show();
            this.Close();
        }
    }
}
