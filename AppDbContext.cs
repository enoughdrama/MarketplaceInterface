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
                
            // Configure relationships
            modelBuilder.Entity<User>()
                .HasRequired(u => u.Role)
                .WithMany()
                .HasForeignKey(u => u.RoleId);
                
            base.OnModelCreating(modelBuilder);
        }
    }
}
