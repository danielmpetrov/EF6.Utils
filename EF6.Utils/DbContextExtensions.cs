using System;
using System.Data.Entity;
using System.Linq;

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

        public static int SaveChangesTimestamped(this DbContext context)
        {
            SetAddedTimestamps(context);

            SetModifiedTimestamps(context);

            return context.SaveChanges();
        }

        private static void SetModifiedTimestamps(DbContext context)
        {
            var modified = context.ChangeTracker.Entries<ITimestampedEntity>()
                .Where(x => x.State == EntityState.Modified)
                .Select(x => x.Entity);

            var now = DateTime.Now;

            foreach (var e in modified)
            {
                e.UpdatedOn = now;
            }
        }

        private static void SetAddedTimestamps(DbContext context)
        {
            var newRecords = context.ChangeTracker.Entries<ITimestampedEntity>()
                .Where(x => x.State == EntityState.Added)
                .Select(x => x.Entity);

            var now = DateTime.Now;

            foreach (var e in newRecords)
            {
                e.CreatedOn = now;
                e.UpdatedOn = now;
            }
        }
    }
}
