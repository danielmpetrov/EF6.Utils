using System.Data.Entity;

namespace EF6.Utils
{
    public static class DbContextExtensions
    {
        public static T LatestCreatedOrDefault<T>(this DbContext context) where T : class, ITimestampedEntity
        {
            return context.Set<T>().LatestCreatedOrDefault();
        }

        public static T LatestCreated<T>(this DbContext context) where T : class, ITimestampedEntity
        {
            return context.Set<T>().LatestCreated();
        }
    }
}
