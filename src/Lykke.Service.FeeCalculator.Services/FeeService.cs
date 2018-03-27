using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Service.FeeCalculator.Core.Domain.Fees;
using Lykke.Service.FeeCalculator.Core.Extensions;
using Lykke.Service.FeeCalculator.Core.Services;
using Lykke.Service.FeeCalculator.Services.CachedModels;
using Lykke.Service.FeeCalculator.Services.Extensions;
using StackExchange.Redis;

namespace Lykke.Service.FeeCalculator.Services
{
    public class FeeService : IFeeService
    {
        private readonly IFeeRepository _repository;
        private readonly IDatabase _db;
        private readonly string _feesKey;

        [UsedImplicitly]
        public FeeService(
            IConnectionMultiplexer connectionMultiplexer,
            IFeeRepository repository,
            string cacheInstanceName
            )
        {
            _db = connectionMultiplexer.GetDatabase();
            _repository = repository;
            _feesKey = $"{cacheInstanceName}:fees";
        }
        
        public async Task<IFee[]> GetAllAsync()
        {
            var serializedValues = await _db.SortedSetRangeByValueAsync(_feesKey);
            var fees =  serializedValues.Select(item => ((byte[])item).DeserializeFee<CachedFee>()).Select(Fee.Create).ToList();

            if (fees.Count != 0) 
                return fees.OrderByDescending(item => item.Volume).ToArray();
            
            fees = (await _repository.GetFeesAsync()).Select(Fee.Create).ToList();
            var entites = fees
                .Select(fee => new SortedSetEntry(fee.SerializeFee(baseFee => new CachedFee(fee)), 0))
                .ToArray();
            
            await _db.SortedSetAddAsync(_feesKey, entites);

            return fees.OrderByDescending(item => item.Volume).ToArray();
        }

        public async Task AddAsync(IFee fee)
        {
            bool isNew = string.IsNullOrEmpty(fee.Id);
            
            var item = await _repository.AddFeeAsync(fee);
            var value = item.SerializeFee(baseFee => new CachedFee(item));
            
            if (!isNew)
                await _db.DeleteFromCacheByIdAsync(_feesKey, fee.Id);
            
            await _db.SortedSetAddAsync(_feesKey, new[] {new SortedSetEntry(value, 0)});
        }

        public async Task DeleteAsync(string id)
        {
            await _repository.DeleteFeeAsync(id);
            await _db.DeleteFromCacheByIdAsync(_feesKey, id);
        }
    }
}
