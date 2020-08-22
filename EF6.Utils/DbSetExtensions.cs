using System.Data.Entity;
using System.Linq;

namespace EF6.Utils
{
    public static class DbSetExtensions
    {
        public static T LatestCreatedOrDefault<T>(this DbSet<T> set) where T : class, ITimestampedEntity
        {
            return set.OrderByDescending(e => e.CreatedOn).FirstOrDefault();
        }
    }
}
