using System;
using System.Data.Entity;
using System.Windows;

namespace AppAuthorization
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            
            // Initialize database
            try
            {
                ResetAndInitializeDatabase();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при инициализации базы данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                Shutdown(-1);
            }
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // Show login window
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
        }
        
        private void ResetAndInitializeDatabase()
        {
            using (var context = new AppDbContext())
            {
                // We need to reset the database for a clean start
                // This is appropriate for development but would not be used in production
                Database.SetInitializer(new DropCreateDatabaseAlways<AppDbContext>());
                
                // Custom initializer that will seed data
                Database.SetInitializer(new DatabaseInitializer());
                
                // Force initialization
                context.Database.Initialize(force: true);
            }
        }
        
        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show($"Произошла непредвиденная ошибка: {e.Exception.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = true;
        }
    }
}
