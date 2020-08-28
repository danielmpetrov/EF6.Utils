using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace EF6.Utils
{
    public static class DbSetExtensions
    {
        public static T LatestCreatedOrDefault<T>(this DbSet<T> set) where T : class, ITimestampedEntity
        {
            return set.OrderByDescending(e => e.CreatedOn).FirstOrDefault();
        }

        public static async Task<T> LatestCreatedOrDefaultAsync<T>(this DbSet<T> set) where T : class, ITimestampedEntity
        {
            return await set.OrderByDescending(e => e.CreatedOn).FirstOrDefaultAsync().ConfigureAwait(false);
        }

        public static T LatestCreated<T>(this DbSet<T> set) where T : class, ITimestampedEntity
        {
            return set.OrderByDescending(e => e.CreatedOn).First();
        }

        public static async Task<T> LatestCreatedAsync<T>(this DbSet<T> set) where T : class, ITimestampedEntity
        {
            return await set.OrderByDescending(e => e.CreatedOn).FirstAsync().ConfigureAwait(false);
        }
    }
}
