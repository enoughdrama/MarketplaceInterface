using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace AppAuthorization
{
    public class DatabaseInitializer : DropCreateDatabaseAlways<AppDbContext>
    {
        protected override void Seed(AppDbContext context)
        {
            try 
            {
                // Seed Roles with specific IDs
                List<Role> roles = new List<Role>
                {
                    new Role { RoleId = 1, RoleName = "Admin", Description = "Administrator with full access" },
                    new Role { RoleId = 2, RoleName = "Seller", Description = "Can sell products on the marketplace" },
                    new Role { RoleId = 3, RoleName = "Buyer", Description = "Can purchase products" }
                };
                
                roles.ForEach(r => context.Roles.Add(r));
                context.SaveChanges();
                
                // Seed Admin User
                string adminPassword = "Admin123!";
                PasswordHelper.CreatePasswordHash(adminPassword, out string adminHash, out string adminSalt);
                
                User adminUser = new User
                {
                    Username = "admin",
                    PasswordHash = adminHash,
                    Salt = adminSalt,
                    FirstName = "System",
                    LastName = "Administrator",
                    Email = "admin@marketplace.com",
                    RoleId = 1, // Admin role
                    RegistrationDate = DateTime.Now,
                    IsActive = true
                };
                
                context.Users.Add(adminUser);
                context.SaveChanges();
                
                // Create a sample seller
                string sellerPassword = "Seller123!";
                PasswordHelper.CreatePasswordHash(sellerPassword, out string sellerHash, out string sellerSalt);
                
                User sellerUser = new User
                {
                    Username = "seller",
                    PasswordHash = sellerHash,
                    Salt = sellerSalt,
                    FirstName = "Test",
                    LastName = "Seller",
                    Email = "seller@marketplace.com",
                    RoleId = 2, // Seller role
                    RegistrationDate = DateTime.Now,
                    IsActive = true
                };
                
                context.Users.Add(sellerUser);
                context.SaveChanges();
                
                // Create a sample buyer
                string buyerPassword = "Buyer123!";
                PasswordHelper.CreatePasswordHash(buyerPassword, out string buyerHash, out string buyerSalt);
                
                User buyerUser = new User
                {
                    Username = "buyer",
                    PasswordHash = buyerHash,
                    Salt = buyerSalt,
                    FirstName = "Test",
                    LastName = "Buyer",
                    Email = "buyer@marketplace.com",
                    RoleId = 3, // Buyer role
                    RegistrationDate = DateTime.Now,
                    IsActive = true
                };
                
                context.Users.Add(buyerUser);
                context.SaveChanges();
                
                // Seed Payment Methods
                List<PaymentMethod> paymentMethods = new List<PaymentMethod>
                {
                    new PaymentMethod { Name = "Credit Card", Description = "Pay with Visa, MasterCard or other cards" },
                    new PaymentMethod { Name = "Bank Transfer", Description = "Pay directly from your bank account" },
                    new PaymentMethod { Name = "PayPal", Description = "Pay using PayPal" },
                    new PaymentMethod { Name = "Cash on Delivery", Description = "Pay when you receive your order" }
                };
                
                paymentMethods.ForEach(pm => context.PaymentMethods.Add(pm));
                context.SaveChanges();
                
                // Seed Categories
                List<Category> categories = new List<Category>
                {
                    new Category { Name = "Electronics", Description = "Electronic devices and gadgets" },
                    new Category { Name = "Clothing", Description = "Apparel and fashion items" },
                    new Category { Name = "Home & Garden", Description = "Items for home and garden" },
                    new Category { Name = "Beauty", Description = "Beauty and personal care products" },
                    new Category { Name = "Sports", Description = "Sports equipment and accessories" },
                    new Category { Name = "Books", Description = "Books and literature" }
                };
                
                categories.ForEach(c => context.Categories.Add(c));
                context.SaveChanges();
                
                // Seed sample products
                List<Product> products = new List<Product>
                {
                    new Product 
                    { 
                        Name = "Смартфон Galaxy S23", 
                        Description = "Оперативная память: 8 ГБ, Память: 256 ГБ", 
                        Price = 69990M, 
                        Quantity = 100, 
                        CategoryId = context.Categories.First(c => c.Name == "Electronics").CategoryId,
                        SellerId = sellerUser.Id
                    },
                    new Product 
                    { 
                        Name = "Ноутбук ProBook X360", 
                        Description = "Core i7, 16 ГБ RAM, 512 ГБ SSD, 14\" FHD IPS", 
                        Price = 89990M, 
                        Quantity = 50, 
                        CategoryId = context.Categories.First(c => c.Name == "Electronics").CategoryId, 
                        SellerId = sellerUser.Id
                    },
                    new Product 
                    { 
                        Name = "Наушники AirSound Pro", 
                        Description = "Активное шумоподавление, 24 часа работы", 
                        Price = 12490M, 
                        Quantity = 200, 
                        CategoryId = context.Categories.First(c => c.Name == "Electronics").CategoryId,
                        SellerId = sellerUser.Id
                    },
                    new Product 
                    { 
                        Name = "Умные часы FitTrack 5", 
                        Description = "Пульсометр, SpO2, GPS, до 14 дней работы", 
                        Price = 15990M, 
                        Quantity = 75, 
                        CategoryId = context.Categories.First(c => c.Name == "Electronics").CategoryId,
                        SellerId = sellerUser.Id
                    },
                    new Product 
                    { 
                        Name = "Футболка Summer Style", 
                        Description = "100% хлопок, разные цвета", 
                        Price = 1990M, 
                        Quantity = 500, 
                        CategoryId = context.Categories.First(c => c.Name == "Clothing").CategoryId,
                        SellerId = sellerUser.Id
                    },
                    new Product 
                    { 
                        Name = "Джинсы Classic Fit", 
                        Description = "Классический крой, темно-синий цвет", 
                        Price = 4990M, 
                        Quantity = 300, 
                        CategoryId = context.Categories.First(c => c.Name == "Clothing").CategoryId,
                        SellerId = sellerUser.Id
                    }
                };
                
                products.ForEach(p => context.Products.Add(p));
                context.SaveChanges();
                
                // Create a few sample orders
                List<Order> orders = new List<Order>
                {
                    new Order
                    {
                        BuyerId = buyerUser.Id,
                        SellerId = sellerUser.Id,
                        OrderDate = DateTime.Now.AddDays(-5),
                        TotalAmount = 69990M,
                        Status = "Completed"
                    },
                    new Order
                    {
                        BuyerId = buyerUser.Id,
                        SellerId = sellerUser.Id,
                        OrderDate = DateTime.Now.AddDays(-2),
                        TotalAmount = 12490M,
                        Status = "Shipped"
                    },
                    new Order
                    {
                        BuyerId = buyerUser.Id,
                        SellerId = sellerUser.Id,
                        OrderDate = DateTime.Now.AddDays(-1),
                        TotalAmount = 6980M,
                        Status = "Processing"
                    }
                };
                
                orders.ForEach(o => context.Orders.Add(o));
                context.SaveChanges();
                
                // Add order items
                List<OrderItem> orderItems = new List<OrderItem>
                {
                    new OrderItem
                    {
                        OrderId = 1,
                        ProductId = 1, // Galaxy S23
                        Quantity = 1,
                        Price = 69990M
                    },
                    new OrderItem
                    {
                        OrderId = 2,
                        ProductId = 3, // AirSound Pro
                        Quantity = 1,
                        Price = 12490M
                    },
                    new OrderItem
                    {
                        OrderId = 3,
                        ProductId = 5, // T-shirt
                        Quantity = 2,
                        Price = 1990M
                    },
                    new OrderItem
                    {
                        OrderId = 3,
                        ProductId = 6, // Jeans
                        Quantity = 1,
                        Price = 4990M
                    }
                };
                
                orderItems.ForEach(oi => context.OrderItems.Add(oi));
                context.SaveChanges();
                
                // Add payments
                List<Payment> payments = new List<Payment>
                {
                    new Payment
                    {
                        OrderId = 1,
                        PaymentMethodId = 1, // Credit Card
                        Amount = 69990M,
                        PaymentDate = DateTime.Now.AddDays(-5),
                        Status = "Completed",
                        TransactionId = "TX-" + Guid.NewGuid().ToString().Substring(0, 8)
                    },
                    new Payment
                    {
                        OrderId = 2,
                        PaymentMethodId = 3, // PayPal
                        Amount = 12490M,
                        PaymentDate = DateTime.Now.AddDays(-2),
                        Status = "Completed",
                        TransactionId = "TX-" + Guid.NewGuid().ToString().Substring(0, 8)
                    },
                    new Payment
                    {
                        OrderId = 3,
                        PaymentMethodId = 4, // Cash on Delivery
                        Amount = 6980M,
                        PaymentDate = DateTime.Now.AddDays(-1),
                        Status = "Pending",
                        TransactionId = "TX-" + Guid.NewGuid().ToString().Substring(0, 8)
                    }
                };
                
                payments.ForEach(p => context.Payments.Add(p));
                context.SaveChanges();
                
                base.Seed(context);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in database initialization: {ex.Message}");
                throw;
            }
        }
    }
}
