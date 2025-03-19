using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace AppAuthorization
{
    public partial class HomePage : Window
    {
        public HomePage()
        {
            InitializeComponent();
            AnimateElementsOnLoad();
        }

        private void AnimateElementsOnLoad()
        {
            var storyboard = new Storyboard();
            
            // Animate the search box
            var searchBoxAnim = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromMilliseconds(300),
                BeginTime = TimeSpan.FromMilliseconds(100)
            };
            Storyboard.SetTarget(searchBoxAnim, SearchBox);
            Storyboard.SetTargetProperty(searchBoxAnim, new PropertyPath("Opacity"));
            storyboard.Children.Add(searchBoxAnim);
            
            // Start the animation
            storyboard.Begin();
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
            MessageBox.Show("У вас нет новых уведомлений", "Уведомления", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void CartButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Корзина пуста", "Корзина", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ProfileButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Функция профиля будет доступна в следующем обновлении", "Профиль", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void PromoBanner_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Летняя распродажа: скидки до 50% на товары для отдыха и спорта!\nСрок акции ограничен.", "Акция", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Category_Click(object sender, MouseButtonEventArgs e)
        {
            var border = sender as Border;
            if (border != null)
            {
                var stackPanel = border.Child as StackPanel;
                if (stackPanel != null)
                {
                    var textBlock = stackPanel.Children[1] as TextBlock;
                    if (textBlock != null)
                    {
                        string categoryName = textBlock.Text;
                        MessageBox.Show($"Выбрана категория: {categoryName}", "Категория", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
        }

        private void Product_Click(object sender, MouseButtonEventArgs e)
        {
            var border = sender as Border;
            if (border != null)
            {
                var stackPanel = border.Child as StackPanel;
                if (stackPanel != null)
                {
                    var textBlock = stackPanel.Children[1] as TextBlock;
                    if (textBlock != null)
                    {
                        string productName = textBlock.Text;
                        MessageBox.Show($"Выбран товар: {productName}", "Товар", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
        }

        private void AddToCart_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                FrameworkElement parent = button.Parent as FrameworkElement;
                while (parent != null && !(parent is Border))
                {
                    parent = parent.Parent as FrameworkElement;
                }

                if (parent != null && parent is Border)
                {
                    Border border = parent as Border;
                    var stackPanel = border.Child as StackPanel;
                    if (stackPanel != null)
                    {
                        var textBlock = stackPanel.Children[1] as TextBlock;
                        if (textBlock != null)
                        {
                            string productName = textBlock.Text;
                            
                            // Create and configure the animation
                            ScaleTransform scaleTransform = new ScaleTransform(1, 1);
                            button.RenderTransform = scaleTransform;
                            
                            DoubleAnimation animX = new DoubleAnimation
                            {
                                From = 1,
                                To = 0.8,
                                Duration = TimeSpan.FromMilliseconds(100),
                                AutoReverse = true
                            };
                            
                            DoubleAnimation animY = new DoubleAnimation
                            {
                                From = 1,
                                To = 0.8,
                                Duration = TimeSpan.FromMilliseconds(100),
                                AutoReverse = true
                            };
                            
                            Storyboard storyboard = new Storyboard();
                            storyboard.Children.Add(animX);
                            storyboard.Children.Add(animY);
                            
                            Storyboard.SetTarget(animX, button);
                            Storyboard.SetTarget(animY, button);
                            
                            Storyboard.SetTargetProperty(animX, new PropertyPath("RenderTransform.ScaleX"));
                            Storyboard.SetTargetProperty(animY, new PropertyPath("RenderTransform.ScaleY"));
                            
                            storyboard.Completed += (s, args) => 
                            {
                                MessageBox.Show($"Товар '{productName}' добавлен в корзину", "Корзина", MessageBoxButton.OK, MessageBoxImage.Information);
                            };
                            
                            storyboard.Begin();
                        }
                    }
                }
            }
        }

        private void Offer_Click(object sender, MouseButtonEventArgs e)
        {
            var border = sender as Border;
            if (border != null)
            {
                Grid grid = border.Child as Grid;
                if (grid != null)
                {
                    StackPanel stackPanel = grid.Children[0] as StackPanel;
                    if (stackPanel != null)
                    {
                        TextBlock textBlock = stackPanel.Children[0] as TextBlock;
                        if (textBlock != null)
                        {
                            string offerName = textBlock.Text;
                            MessageBox.Show($"Выбрано предложение: {offerName}", "Специальное предложение", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }
            }
        }

        private void ViewOffer_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                FrameworkElement parent = button.Parent as FrameworkElement;
                while (parent != null && !(parent is Border))
                {
                    parent = parent.Parent as FrameworkElement;
                }

                if (parent != null && parent is Border)
                {
                    Border border = parent as Border;
                    Grid grid = border.Child as Grid;
                    if (grid != null)
                    {
                        StackPanel stackPanel = grid.Children[0] as StackPanel;
                        if (stackPanel != null)
                        {
                            TextBlock textBlock = stackPanel.Children[0] as TextBlock;
                            if (textBlock != null)
                            {
                                string offerName = textBlock.Text;
                                MessageBox.Show($"Просмотр предложения: {offerName}", "Специальное предложение", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                        }
                    }
                }
            }
        }
    }
}
