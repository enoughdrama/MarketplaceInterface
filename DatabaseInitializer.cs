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
                        SellerId = sellerUser.Id  // Use the seller we created
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
                    }
                };
                
                products.ForEach(p => context.Products.Add(p));
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
