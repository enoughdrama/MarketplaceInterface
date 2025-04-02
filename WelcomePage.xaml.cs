using System;
using System.Windows;
using System.Windows.Controls;

namespace AppAuthorization
{
    public partial class WelcomePage : Page
    {
        private MainWindow? mainWindow;

        public WelcomePage()
        {
            InitializeComponent();
            Loaded += WelcomePage_Loaded;
        }
        
        private void WelcomePage_Loaded(object sender, RoutedEventArgs e)
        {
            mainWindow = Window.GetWindow(this) as MainWindow;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (mainWindow != null)
            {
                mainWindow.NavigateToPage(new LoginPage());
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
