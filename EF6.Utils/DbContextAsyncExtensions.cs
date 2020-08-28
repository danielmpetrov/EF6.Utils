using System.Data.Entity;
using System.Threading.Tasks;

namespace EF6.Utils
{
    public static class DbContextAsyncExtensions
    {
        public static async Task<T> LatestCreatedOrDefaultAsync<T>(this DbContext context) where T : class, ITimestampedEntity
        {
            return await context.Set<T>().LatestCreatedOrDefaultAsync().ConfigureAwait(false);
        }

        public static async Task<T> LatestCreatedAsync<T>(this DbContext context) where T : class, ITimestampedEntity
        {
            return await context.Set<T>().LatestCreatedAsync().ConfigureAwait(false);
        }
    }
}
