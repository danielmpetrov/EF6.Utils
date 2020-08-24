using System.Data.Entity.Migrations;

namespace EF6.Utils.Demo.Data.Migrations
{

    internal sealed class Configuration : DbMigrationsConfiguration<AppDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }
    }
}
