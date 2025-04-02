using System.Data.Entity;

namespace AppAuthorization
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base("DefaultConnection")
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<AppDbContext>());
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Commission> Commissions { get; set; }
        public DbSet<Payout> Payouts { get; set; }
        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Configure entity properties
            modelBuilder.Entity<User>()
                .Property(u => u.Username)
                .IsRequired()
                .HasMaxLength(50);
                
            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .HasMaxLength(150);
                
            modelBuilder.Entity<Role>()
                .Property(r => r.RoleName)
                .IsRequired()
                .HasMaxLength(50);
                
            // Create unique indexes
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();
                
            modelBuilder.Entity<Role>()
                .HasIndex(r => r.RoleName)
                .IsUnique();
                
            // Configure relationships to avoid cycles
            modelBuilder.Entity<User>()
                .HasRequired(u => u.Role)
                .WithMany()
                .HasForeignKey(u => u.RoleId);
            
            // Configure Order-User relationship to prevent circular dependencies
            modelBuilder.Entity<Order>()
                .HasRequired(o => o.Buyer)
                .WithMany()
                .HasForeignKey(o => o.BuyerId)
                .WillCascadeOnDelete(false);
                
            modelBuilder.Entity<Order>()
                .HasRequired(o => o.Seller)
                .WithMany()
                .HasForeignKey(o => o.SellerId)
                .WillCascadeOnDelete(false);
                
            // Prevent circular dependencies for Product-User relationship
            modelBuilder.Entity<Product>()
                .HasRequired(p => p.Seller)
                .WithMany()
                .HasForeignKey(p => p.SellerId)
                .WillCascadeOnDelete(false);
                
            // Prevent circular dependencies for Payout-User relationship
            modelBuilder.Entity<Payout>()
                .HasRequired(p => p.Seller)
                .WithMany()
                .HasForeignKey(p => p.SellerId)
                .WillCascadeOnDelete(false);
                
            // Configure OrderItem relationships
            modelBuilder.Entity<OrderItem>()
                .HasRequired(oi => oi.Order)
                .WithMany()
                .HasForeignKey(oi => oi.OrderId);
                
            modelBuilder.Entity<OrderItem>()
                .HasRequired(oi => oi.Product)
                .WithMany()
                .HasForeignKey(oi => oi.ProductId)
                .WillCascadeOnDelete(false);
                
            // Configure Payment-Order relationship
            modelBuilder.Entity<Payment>()
                .HasRequired(p => p.Order)
                .WithMany()
                .HasForeignKey(p => p.OrderId)
                .WillCascadeOnDelete(false);
                
            // Configure Commission-Payment relationship
            modelBuilder.Entity<Commission>()
                .HasRequired(c => c.Payment)
                .WithMany()
                .HasForeignKey(c => c.PaymentId)
                .WillCascadeOnDelete(false);
                
            base.OnModelCreating(modelBuilder);
        }
    }
}
