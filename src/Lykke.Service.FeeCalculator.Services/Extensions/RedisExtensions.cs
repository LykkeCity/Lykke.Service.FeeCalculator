using System.Threading.Tasks;
using Lykke.Service.FeeCalculator.Core.Extensions;
using StackExchange.Redis;

namespace Lykke.Service.FeeCalculator.Services.Extensions
{
    public static class RedisExtensions
    {
        public static async Task DeleteFromCacheByIdAsync(this IDatabase db, string key, string id)
        {
            await db.SortedSetRemoveRangeByValueAsync(key, id, id.GenerateNextId(), Exclude.Stop);
        }
    }
}
