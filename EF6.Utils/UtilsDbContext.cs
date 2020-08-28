using System;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EF6.Utils
{
    /// <summary>
    /// A DbContext with support for timestamping entites.
    /// </summary>
    public abstract class UtilsDbContext : DbContext
    {
        public UtilsDbContext()
        {

        }

        public UtilsDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {

        }

        public UtilsDbContext(string nameOrConnectionString, DbCompiledModel model)
            : base(nameOrConnectionString, model)
        {

        }

        public UtilsDbContext(DbConnection existingConnection, bool contextOwnsConnection)
            : base(existingConnection, contextOwnsConnection)
        {

        }

        public UtilsDbContext(ObjectContext objectContext, bool dbContextOwnsObjectContext)
            : base(objectContext, dbContextOwnsObjectContext)
        {

        }

        public UtilsDbContext(DbConnection existingConnection, DbCompiledModel model, bool contextOwnsConnection)
            : base(existingConnection, model, contextOwnsConnection)
        {

        }

        public override int SaveChanges()
        {
            SetAddedTimestamps();

            SetModifiedTimestamps();

            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            SetAddedTimestamps();

            SetModifiedTimestamps();

            return await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        private void SetModifiedTimestamps()
        {
            var modified = ChangeTracker.Entries<ITimestampedEntity>()
                .Where(x => x.State == EntityState.Modified)
                .Select(x => x.Entity);

            var now = DateTime.Now;

            foreach (var e in modified)
            {
                e.UpdatedOn = now;
            }
        }

        private void SetAddedTimestamps()
        {
            var newRecords = ChangeTracker.Entries<ITimestampedEntity>()
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
