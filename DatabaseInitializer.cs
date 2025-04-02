using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace AppAuthorization
{
    public class DatabaseInitializer : CreateDatabaseIfNotExists<AppDbContext>
    {
        // Fill database with mock data
        protected override void Seed(AppDbContext context)
        {
            // Seed Roles
            List<Role> roles = new List<Role>
            {
                new Role { RoleName = "Admin", Description = "Administrator with full access" },
                new Role { RoleName = "Seller", Description = "Can sell products on the marketplace" },
                new Role { RoleName = "Buyer", Description = "Can purchase products" }
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
                RoleId = roles.First(r => r.RoleName == "Admin").RoleId,
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
            
            base.Seed(context);
        }
    }
}
