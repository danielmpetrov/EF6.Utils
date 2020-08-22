using System.Data.Entity;

namespace EF6.Utils.Demo.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Comment> Comments { get; set; }
    }
}
