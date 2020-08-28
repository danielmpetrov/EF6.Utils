using EF6.Utils.Demo.Data.Migrations;
using System.Data.Common;
using System.Data.Entity;

namespace EF6.Utils.Demo.Data
{
    public class AppDbContext : UtilsDbContext
    {
        public AppDbContext()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<AppDbContext, Configuration>());
        }

        public AppDbContext(DbConnection connection) : base(connection, true)
        {
        }

        public DbSet<Comment> Comments { get; set; }
    }
}
