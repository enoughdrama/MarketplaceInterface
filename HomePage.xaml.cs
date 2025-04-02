using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AppAuthorization
{
    public partial class HomePage : Page
    {
        private MainWindow? mainWindow;
        private readonly AppDbContext _context;
        private User? _currentUser;

        public HomePage()
        {
            InitializeComponent();
            _context = new AppDbContext();
            
            Loaded += HomePage_Loaded;
        }

        private void HomePage_Loaded(object sender, RoutedEventArgs e)
        {
            mainWindow = Window.GetWindow(this) as MainWindow;
            
            if (mainWindow != null && mainWindow.CurrentUser != null)
            {
                _currentUser = mainWindow.CurrentUser;
            }
            
            LoadCategories();
            LoadProducts();
        }

        private void LoadCategories()
        {
            try
            {
                CategoriesPanel.Children.Clear();
                
                var categories = _context.Categories.ToList();
                foreach (var category in categories)
                {
                    var productCount = _context.Products.Count(p => p.CategoryId == category.CategoryId);
                    
                    Border categoryCard = new Border
                    {
                        Style = this.FindResource("CategoryCardStyle") as Style,
                        Cursor = Cursors.Hand
                    };
                    
                    categoryCard.MouseDown += Category_Click;
                    categoryCard.Tag = category;
                    
                    StackPanel panel = new StackPanel { VerticalAlignment = VerticalAlignment.Center };
                    
                    Ellipse icon = new Ellipse
                    {
                        Width = 60,
                        Height = 60,
                        Margin = new Thickness(0, 0, 0, 10)
                    };
                    
                    // Generate random pastel color
                    Random random = new Random(category.CategoryId);
                    byte r = (byte)(155 + random.Next(100));
                    byte g = (byte)(155 + random.Next(100));
                    byte b = (byte)(155 + random.Next(100));
                    icon.Fill = new SolidColorBrush(Color.FromRgb(r, g, b));
                    
                    TextBlock nameText = new TextBlock
                    {
                        Text = category.Name,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        FontWeight = FontWeights.SemiBold
                    };
                    
                    TextBlock countText = new TextBlock
                    {
                        Text = $"{productCount} товаров",
                        HorizontalAlignment = HorizontalAlignment.Center,
                        FontSize = 12,
                        Foreground = new SolidColorBrush(Color.FromRgb(119, 119, 119))
                    };
                    
                    panel.Children.Add(icon);
                    panel.Children.Add(nameText);
                    panel.Children.Add(countText);
                    
                    categoryCard.Child = panel;
                    CategoriesPanel.Children.Add(categoryCard);
                }
            }
            catch (Exception ex)
            {
                // Log the error instead of showing a MessageBox
                Console.WriteLine($"Error loading categories: {ex.Message}");
            }
        }

        private void LoadProducts()
        {
            try
            {
                ProductsPanel.Children.Clear();
                
                var products = _context.Products
                    .OrderBy(p => Guid.NewGuid()) // Random order for "Popular" section
                    .Take(10)
                    .ToList();
                
                foreach (var product in products)
                {
                    Border productCard = new Border
                    {
                        Style = this.FindResource("ProductCardStyle") as Style,
                        Cursor = Cursors.Hand
                    };
                    
                    productCard.MouseDown += Product_Click;
                    productCard.Tag = product;
                    
                    StackPanel panel = new StackPanel { Margin = new Thickness(12) };
                    
                    Grid imageContainer = new Grid { Height = 180 };
                    Rectangle imagePlaceholder = new Rectangle
                    {
                        Fill = new SolidColorBrush(Color.FromRgb(245, 245, 245)),
                        RadiusX = 8,
                        RadiusY = 8
                    };
                    imageContainer.Children.Add(imagePlaceholder);
                    
                    TextBlock nameText = new TextBlock
                    {
                        Text = product.Name ?? string.Empty,
                        FontWeight = FontWeights.SemiBold,
                        Margin = new Thickness(0, 10, 0, 5)
                    };
                    
                    TextBlock descText = new TextBlock
                    {
                        Text = product.Description ?? "Нет описания",
                        FontSize = 12,
                        TextWrapping = TextWrapping.Wrap,
                        Foreground = new SolidColorBrush(Color.FromRgb(119, 119, 119))
                    };
                    
                    Grid priceContainer = new Grid { Margin = new Thickness(0, 15, 0, 0) };
                    
                    TextBlock priceText = new TextBlock
                    {
                        Text = $"{product.Price:N0} ₽",
                        FontWeight = FontWeights.Bold,
                        FontSize = 18,
                        HorizontalAlignment = HorizontalAlignment.Left
                    };
                    
                    Button addToCartButton = new Button
                    {
                        Content = "В корзину",
                        Style = this.FindResource("ActionButtonStyle") as Style,
                        HorizontalAlignment = HorizontalAlignment.Right
                    };
                    
                    addToCartButton.Click += AddToCart_Click;
                    addToCartButton.Tag = product;
                    
                    priceContainer.Children.Add(priceText);
                    priceContainer.Children.Add(addToCartButton);
                    
                    panel.Children.Add(imageContainer);
                    panel.Children.Add(nameText);
                    panel.Children.Add(descText);
                    panel.Children.Add(priceContainer);
                    
                    productCard.Child = panel;
                    ProductsPanel.Children.Add(productCard);
                }
            }
            catch (Exception ex)
            {
                // Log the error instead of showing a MessageBox
                Console.WriteLine($"Error loading products: {ex.Message}");
            }
        }

        private void SearchBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text == "Поиск товаров...")
            {
                textBox.Text = string.Empty;
            }
        }

        private void SearchBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = "Поиск товаров...";
            }
        }

        private void NotificationsButton_Click(object sender, RoutedEventArgs e)
        {
            // No messages for now
        }

        private void CartButton_Click(object sender, RoutedEventArgs e)
        {
            // Cart is empty for now
        }

        private void ProfileButton_Click(object sender, RoutedEventArgs e)
        {
            var menuItems = new string[] { "Управление товарами", "Мои заказы", "Настройки", "Выход" };
            var contextMenu = new ContextMenu();
            
            foreach (var item in menuItems)
            {
                var menuItem = new MenuItem { Header = item };
                menuItem.Click += ProfileMenuItem_Click;
                contextMenu.Items.Add(menuItem);
            }
            
            contextMenu.IsOpen = true;
        }
        
        private void ProfileMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem && mainWindow != null)
            {
                switch (menuItem.Header.ToString())
                {
                    case "Управление товарами":
                        mainWindow.NavigateToPage(new ProductManagementPage());
                        break;
                    case "Мои заказы":
                        // Not implemented yet
                        break;
                    case "Настройки":
                        // Not implemented yet
                        break;
                    case "Выход":
                        // Go back to login
                        mainWindow.NavigateToPage(new LoginPage());
                        break;
                }
            }
        }

        private void PromoBanner_Click(object sender, RoutedEventArgs e)
        {
            // Promo details would go here
        }

        private void Category_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border border && border.Tag is Category category)
            {
                // Load category products
            }
        }

        private void Product_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border border && border.Tag is Product product)
            {
                // Show product details
            }
        }

        private void AddToCart_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is Product product)
            {
                // Add to cart functionality
            }
        }

        private void Offer_Click(object sender, MouseButtonEventArgs e)
        {
            // Offer details would go here
        }

        private void ViewOffer_Click(object sender, RoutedEventArgs e)
        {
            // Show offer details
        }
    }
}
