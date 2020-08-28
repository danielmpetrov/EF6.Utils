using System.Data.Entity;
using System.Threading.Tasks;

namespace EF6.Utils
{
    public static class DbContextExtensions
    {
        public static T LatestCreatedOrDefault<T>(this DbContext context) where T : class, ITimestampedEntity
        {
            return context.Set<T>().LatestCreatedOrDefault();
        }

        public static async Task<T> LatestCreatedOrDefaultAsync<T>(this DbContext context) where T : class, ITimestampedEntity
        {
            return await context.Set<T>().LatestCreatedOrDefaultAsync().ConfigureAwait(false);
        }

        public static T LatestCreated<T>(this DbContext context) where T : class, ITimestampedEntity
        {
            return context.Set<T>().LatestCreated();
        }

        public static async Task<T> LatestCreatedAsync<T>(this DbContext context) where T : class, ITimestampedEntity
        {
            return await context.Set<T>().LatestCreatedAsync().ConfigureAwait(false);
        }

        public static T LatestUpdatedOrDefault<T>(this DbContext context) where T : class, ITimestampedEntity
        {
            return context.Set<T>().LatestUpdatedOrDefault();
        }

        public static async Task<T> LatestUpdatedOrDefaultAsync<T>(this DbContext context) where T : class, ITimestampedEntity
        {
            return await context.Set<T>().LatestUpdatedOrDefaultAsync().ConfigureAwait(false);
        }

        public static T LatestUpdated<T>(this DbContext context) where T : class, ITimestampedEntity
        {
            return context.Set<T>().LatestUpdated();
        }

        public static async Task<T> LatestUpdatedAsync<T>(this DbContext context) where T : class, ITimestampedEntity
        {
            return await context.Set<T>().LatestUpdatedAsync().ConfigureAwait(false);
        }
    }
}
