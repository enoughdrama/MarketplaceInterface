using System.Data.Entity;

namespace AppAuthorization
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base("DefaultConnection")
        {
        }

        public DbSet<User> Users { get; set; }
    }
}
