using System.Windows;
using System.Windows.Controls;

namespace AppAuthorization
{
    public partial class WelcomePage : Page
    {
        private MainWindow mainWindow;

        public WelcomePage()
        {
            InitializeComponent();
            mainWindow = (MainWindow)Application.Current.MainWindow;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.NavigateToPage(new LoginPage());
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.NavigateToPage(new RegistrationPage());
        }
    }
}
