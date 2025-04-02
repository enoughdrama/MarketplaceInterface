using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace AppAuthorization
{
    public partial class ProductManagementPage : Page
    {
        private MainWindow? mainWindow;
        private readonly AppDbContext _context;
        private Product? _currentProduct;
        private User? _currentUser;
        
        public ProductManagementPage()
        {
            InitializeComponent();
            _context = new AppDbContext();
            
            Loaded += ProductManagementPage_Loaded;
        }
        
        private void ProductManagementPage_Loaded(object sender, RoutedEventArgs e)
        {
            mainWindow = Window.GetWindow(this) as MainWindow;
            
            if (mainWindow != null && mainWindow.CurrentUser != null)
            {
                _currentUser = mainWindow.CurrentUser;
                LoadData();
            }
            else
            {
                // No user logged in or no access
                StatusText.Text = "Ошибка: нет доступа к управлению товарами";
            }
        }
        
        private void LoadData()
        {
            try
            {
                // Load categories for dropdown
                var categories = _context.Categories.ToList();
                CategoryComboBox.ItemsSource = categories;
                
                // Load products based on user role
                if (_currentUser?.Role?.RoleName == "Admin")
                {
                    // Admin can see all products
                    ProductsGrid.ItemsSource = _context.Products
                        .Include(p => p.Category)
                        .ToList();
                }
                else if (_currentUser?.Role?.RoleName == "Seller")
                {
                    // Seller can only see their own products
                    ProductsGrid.ItemsSource = _context.Products
                        .Include(p => p.Category)
                        .Where(p => p.SellerId == _currentUser.Id)
                        .ToList();
                }
                else
                {
                    // Other roles have limited or no access
                    StatusText.Text = "У вас нет прав для управления товарами";
                }
                
                StatusText.Text = "Данные загружены";
            }
            catch (Exception ex)
            {
                StatusText.Text = $"Ошибка при загрузке данных: {ex.Message}";
            }
        }
        
        private void ClearForm()
        {
            NameTextBox.Text = string.Empty;
            DescriptionTextBox.Text = string.Empty;
            PriceTextBox.Text = string.Empty;
            QuantityTextBox.Text = string.Empty;
            CategoryComboBox.SelectedIndex = -1;
            _currentProduct = null;
        }
        
        private void DisplayProductDetails(Product product)
        {
            if (product == null)
            {
                ClearForm();
                return;
            }
            
            NameTextBox.Text = product.Name;
            DescriptionTextBox.Text = product.Description;
            PriceTextBox.Text = product.Price.ToString("0.00");
            QuantityTextBox.Text = product.Quantity.ToString();
            
            // Find and select the category
            var categories = CategoryComboBox.Items.Cast<Category>();
            var matchingCategory = categories.FirstOrDefault(c => c.CategoryId == product.CategoryId);
            
            if (matchingCategory != null)
            {
                CategoryComboBox.SelectedItem = matchingCategory;
            }
        }
        
        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(NameTextBox.Text))
            {
                StatusText.Text = "Ошибка: введите название товара";
                return false;
            }
            
            if (!decimal.TryParse(PriceTextBox.Text, out decimal price) || price < 0)
            {
                StatusText.Text = "Ошибка: введите корректную цену";
                return false;
            }
            
            if (!int.TryParse(QuantityTextBox.Text, out int quantity) || quantity < 0)
            {
                StatusText.Text = "Ошибка: введите корректное количество";
                return false;
            }
            
            if (CategoryComboBox.SelectedItem == null)
            {
                StatusText.Text = "Ошибка: выберите категорию";
                return false;
            }
            
            return true;
        }
        
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (mainWindow != null)
            {
                mainWindow.NavigateToPage(new HomePage());
            }
        }
        
        private void ProductsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _currentProduct = ProductsGrid.SelectedItem as Product;
            DisplayProductDetails(_currentProduct);
        }
        
        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
            
            // Set some defaults
            if (CategoryComboBox.Items.Count > 0)
            {
                CategoryComboBox.SelectedIndex = 0;
            }
        }
        
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInput() || _currentUser == null)
                return;
                
            try
            {
                if (_currentProduct == null)
                {
                    // Creating new product
                    _currentProduct = new Product
                    {
                        SellerId = _currentUser.Id
                    };
                    _context.Products.Add(_currentProduct);
                }
                
                // Update product properties
                _currentProduct.Name = NameTextBox.Text;
                _currentProduct.Description = DescriptionTextBox.Text;
                _currentProduct.Price = decimal.Parse(PriceTextBox.Text);
                _currentProduct.Quantity = int.Parse(QuantityTextBox.Text);
                _currentProduct.CategoryId = (int)CategoryComboBox.SelectedValue;
                
                // Save changes to database
                _context.SaveChanges();
                
                // Refresh the grid
                LoadData();
                
                StatusText.Text = "Товар успешно сохранен";
            }
            catch (Exception ex)
            {
                StatusText.Text = $"Ошибка при сохранении: {ex.Message}";
            }
        }
        
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentProduct == null)
            {
                StatusText.Text = "Выберите товар для удаления";
                return;
            }
            
            if (MessageBox.Show($"Вы уверены, что хотите удалить товар '{_currentProduct.Name}'?", 
                "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                try
                {
                    _context.Products.Remove(_currentProduct);
                    _context.SaveChanges();
                    
                    ClearForm();
                    LoadData();
                    
                    StatusText.Text = "Товар успешно удален";
                }
                catch (Exception ex)
                {
                    StatusText.Text = $"Ошибка при удалении: {ex.Message}";
                }
            }
        }
    }
}
