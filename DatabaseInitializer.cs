using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace AppAuthorization
{
    public class DatabaseInitializer : CreateDatabaseIfNotExists<AppDbContext>
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
                
                context.Roles.AddRange(roles);
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
                
                // Seed Payment Methods
                List<PaymentMethod> paymentMethods = new List<PaymentMethod>
                {
                    new PaymentMethod { Name = "Credit Card", Description = "Pay with Visa, MasterCard or other cards" },
                    new PaymentMethod { Name = "Bank Transfer", Description = "Pay directly from your bank account" },
                    new PaymentMethod { Name = "PayPal", Description = "Pay using PayPal" },
                    new PaymentMethod { Name = "Cash on Delivery", Description = "Pay when you receive your order" }
                };
                
                context.PaymentMethods.AddRange(paymentMethods);
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
                
                context.Categories.AddRange(categories);
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
                        CategoryId = 1, // Electronics
                        SellerId = 1  // Admin user (for demonstration)
                    },
                    new Product 
                    { 
                        Name = "Ноутбук ProBook X360", 
                        Description = "Core i7, 16 ГБ RAM, 512 ГБ SSD, 14\" FHD IPS", 
                        Price = 89990M, 
                        Quantity = 50, 
                        CategoryId = 1, // Electronics
                        SellerId = 1  // Admin user
                    },
                    new Product 
                    { 
                        Name = "Наушники AirSound Pro", 
                        Description = "Активное шумоподавление, 24 часа работы", 
                        Price = 12490M, 
                        Quantity = 200, 
                        CategoryId = 1, // Electronics
                        SellerId = 1  // Admin user
                    }
                };
                
                context.Products.AddRange(products);
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
