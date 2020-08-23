using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace EF6.Utils
{
    public static class DbSetAsyncExtensions
    {
        public static async Task<T> LatestCreatedOrDefaultAsync<T>(this DbSet<T> set) where T : class, ITimestampedEntity
        {
            return await set.OrderByDescending(e => e.CreatedOn).FirstOrDefaultAsync();
        }
        
        public static async Task<T> LatestCreatedAsync<T>(this DbSet<T> set) where T : class, ITimestampedEntity
        {
            return await set.OrderByDescending(e => e.CreatedOn).FirstAsync();
        }
    }
}
