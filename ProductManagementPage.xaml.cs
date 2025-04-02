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
        private AppDbContext _context;
        private Product _currentProduct;
        private MainWindow _mainWindow;

        public ProductManagementPage()
        {
            InitializeComponent();
            _context = new AppDbContext();
            _mainWindow = (MainWindow)Application.Current.MainWindow;
            
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                _context.Products.Load();
                _context.Categories.Load();
                
                ProductsDataGrid.ItemsSource = _context.Products.Local;
                CategoryComboBox.ItemsSource = _context.Categories.Local;
                
                StatusTextBlock.Text = "Данные загружены";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                StatusTextBlock.Text = "Ошибка при загрузке данных";
            }
        }

        private void ClearFields()
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
                ClearFields();
                return;
            }

            NameTextBox.Text = product.Name;
            DescriptionTextBox.Text = product.Description;
            PriceTextBox.Text = product.Price.ToString();
            QuantityTextBox.Text = product.Quantity.ToString();
            
            if (product.CategoryId > 0)
            {
                CategoryComboBox.SelectedItem = _context.Categories.Local.FirstOrDefault(c => c.CategoryId == product.CategoryId);
            }
            else
            {
                CategoryComboBox.SelectedIndex = -1;
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(NameTextBox.Text))
            {
                MessageBox.Show("Введите название товара", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!decimal.TryParse(PriceTextBox.Text, out decimal price) || price < 0)
            {
                MessageBox.Show("Введите корректную цену", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!int.TryParse(QuantityTextBox.Text, out int quantity) || quantity < 0)
            {
                MessageBox.Show("Введите корректное количество", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (CategoryComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите категорию", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        private void ProductsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _currentProduct = ProductsDataGrid.SelectedItem as Product;
            DisplayProductDetails(_currentProduct);
        }

        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            ClearFields();
            
            if (_context.Categories.Local.Count > 0)
            {
                CategoryComboBox.SelectedIndex = 0;
            }
            
            _currentProduct = null;
            NameTextBox.Focus();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInput())
                return;

            try
            {
                if (_currentProduct == null)
                {
                    _currentProduct = new Product
                    {
                        SellerId = 1 // Default to first user or current logged-in user
                    };
                    _context.Products.Add(_currentProduct);
                }

                _currentProduct.Name = NameTextBox.Text;
                _currentProduct.Description = DescriptionTextBox.Text;
                _currentProduct.Price = decimal.Parse(PriceTextBox.Text);
                _currentProduct.Quantity = int.Parse(QuantityTextBox.Text);
                _currentProduct.CategoryId = ((Category)CategoryComboBox.SelectedItem).CategoryId;

                _context.SaveChanges();
                
                ProductsDataGrid.Items.Refresh();
                StatusTextBlock.Text = "Товар сохранен";
                
                MessageBox.Show("Товар успешно сохранен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении товара: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                StatusTextBlock.Text = "Ошибка при сохранении товара";
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentProduct == null)
            {
                MessageBox.Show("Выберите товар для удаления", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show($"Вы уверены, что хотите удалить товар '{_currentProduct.Name}'?", 
                "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);
                
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    _context.Products.Remove(_currentProduct);
                    _context.SaveChanges();
                    
                    ClearFields();
                    ProductsDataGrid.Items.Refresh();
                    StatusTextBlock.Text = "Товар удален";
                    
                    MessageBox.Show("Товар успешно удален", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении товара: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    StatusTextBlock.Text = "Ошибка при удалении товара";
                }
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.NavigateToPage(new HomePage());
        }
    }
}
