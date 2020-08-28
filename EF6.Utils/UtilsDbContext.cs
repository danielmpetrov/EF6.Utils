using EF6.Utils.Common;
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

            var now = _clock.Now();

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

            var now = _clock.Now();

            foreach (var e in newRecords)
            {
                e.CreatedOn = now;
                e.UpdatedOn = now;
            }
        }
    }
}
