using System.Windows;
using System.Windows.Controls;

namespace AppAuthorization
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            NavigateToPage(new WelcomePage());
        }

        public void NavigateToPage(Page page)
        {
            MainFrame.Navigate(page);
        }
    }
}
