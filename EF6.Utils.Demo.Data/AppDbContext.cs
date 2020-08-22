using System.Data.Common;
using System.Data.Entity;

namespace EF6.Utils.Demo.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext()
        {
        }

        public AppDbContext(DbConnection connection) : base(connection, true)
        {
        }

        public DbSet<Comment> Comments { get; set; }
    }
}
