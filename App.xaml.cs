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
            InitializeDatabase();
        }
        
        private void InitializeDatabase()
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    // Force the database to be created if it doesn't exist
                    if (!context.Database.Exists())
                    {
                        context.Database.Create();
                    }
                    
                    // This will apply any pending migrations and seed the database
                    Database.SetInitializer(new DatabaseInitializer());
                    context.Database.Initialize(force: false);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при инициализации базы данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show($"Произошла непредвиденная ошибка: {e.Exception.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = true;
        }
    }
}
