using System;
using System.Data.Entity;
using System.Linq;

namespace EF6.Utils.Internal
{
    internal static class ContextHelpers
    {
        public static void SetModifiedTimestamps(this DbContext context)
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

        public static void SetAddedTimestamps(this DbContext context)
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
