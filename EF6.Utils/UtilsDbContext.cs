using EF6.Utils.Common;
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
        private readonly IClock _clock;

        public UtilsDbContext() : this(new Clock())
        {

        }

        public UtilsDbContext(IClock clock)
        {
            _clock = clock;
        }

        public UtilsDbContext(string nameOrConnectionString)
            : this(nameOrConnectionString, new Clock())
        {

        }

        public UtilsDbContext(string nameOrConnectionString, IClock clock)
            : base(nameOrConnectionString)
        {
            _clock = clock;
        }

        public UtilsDbContext(string nameOrConnectionString, DbCompiledModel model)
            : this(nameOrConnectionString, model, new Clock())
        {

        }

        public UtilsDbContext(string nameOrConnectionString, DbCompiledModel model, IClock clock)
            : base(nameOrConnectionString, model)
        {
            _clock = clock;
        }

        public UtilsDbContext(DbConnection existingConnection, bool contextOwnsConnection)
            : this(existingConnection, contextOwnsConnection, new Clock())
        {

        }

        public UtilsDbContext(DbConnection existingConnection, bool contextOwnsConnection, IClock clock)
            : base(existingConnection, contextOwnsConnection)
        {
            _clock = clock;
        }

        public UtilsDbContext(ObjectContext objectContext, bool dbContextOwnsObjectContext)
            : this(objectContext, dbContextOwnsObjectContext, new Clock())
        {

        }

        public UtilsDbContext(ObjectContext objectContext, bool dbContextOwnsObjectContext, IClock clock)
            : base(objectContext, dbContextOwnsObjectContext)
        {
            _clock = clock;
        }

        public UtilsDbContext(DbConnection existingConnection, DbCompiledModel model, bool contextOwnsConnection)
            : this(existingConnection, model, contextOwnsConnection, new Clock())
        {

        }

        public UtilsDbContext(DbConnection existingConnection, DbCompiledModel model, bool contextOwnsConnection, IClock clock)
            : base(existingConnection, model, contextOwnsConnection)
        {
            _clock = clock;
        }

        public override int SaveChanges()
        {
            BeforeSaveChanges();

            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            BeforeSaveChanges();

            return await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        private void BeforeSaveChanges()
        {
            var now = _clock.Now();

            SetAddedTimestamps(now);

            SetModifiedTimestamps(now);

            SoftDeleteEntities(now);
        }

        private void SoftDeleteEntities(DateTime now)
        {
            var removedEntries = ChangeTracker.Entries<ISoftDeletableEntity>()
                .Where(x => x.State == EntityState.Deleted);

            foreach (var entry in removedEntries)
            {
                entry.State = EntityState.Modified;
                entry.Entity.DeletedOn = now;
            }
        }

        private void SetModifiedTimestamps(DateTime now)
        {
            var modifiedEntities = ChangeTracker.Entries<ITimestampedEntity>()
                .Where(x => x.State == EntityState.Modified)
                .Select(x => x.Entity);

            foreach (var entity in modifiedEntities)
            {
                entity.UpdatedOn = now;
            }
        }

        private void SetAddedTimestamps(DateTime now)
        {
            var addedEntities = ChangeTracker.Entries<ITimestampedEntity>()
                .Where(x => x.State == EntityState.Added)
                .Select(x => x.Entity);

            foreach (var entity in addedEntities)
            {
                entity.CreatedOn = now;
                entity.UpdatedOn = now;
            }
        }
    }
}
