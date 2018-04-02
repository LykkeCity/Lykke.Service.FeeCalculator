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
    public class StaticFeeService : IStaticFeeService
    {
        private readonly IStaticFeeRepository _repository;
        private readonly IDatabase _db;
        private readonly string _feesKey;

        [UsedImplicitly]
        public StaticFeeService(
            IConnectionMultiplexer connectionMultiplexer,
            IStaticFeeRepository repository,
            string cacheInstanceName
            )
        {
            _db = connectionMultiplexer.GetDatabase();
            _repository = repository;
            _feesKey = $"{cacheInstanceName}:staticFees";
        }
        
        public async Task<IStaticFee[]> GetAllAsync()
        {
            var serializedValues = await _db.SortedSetRangeByValueAsync(_feesKey);
            var fees =  serializedValues.Select(item => ((byte[])item).DeserializeFee<CachedStaticFee>()).Select(StaticFee.Create).ToList();

            if (fees.Count != 0) 
                return fees.ToArray();
            
            fees = (await _repository.GetFeesAsync()).Select(StaticFee.Create).ToList();
            var entites = fees
                .Select(fee => new SortedSetEntry(fee.SerializeFee(baseFee => new CachedStaticFee(fee)), 0))
                .ToArray();
            
            await _db.SortedSetAddAsync(_feesKey, entites);

            return fees.ToArray();
        }

        public async Task<IStaticFee> GetAsync(string assetPair)
        {
             return (await GetAllAsync()).FirstOrDefault(item => item.AssetPair == assetPair);
        }

        public async Task AddAsync(IStaticFee fee)
        {
            bool isNew = string.IsNullOrEmpty(fee.Id);
            
            var item = await _repository.AddFeeAsync(fee);
            var value = item.SerializeFee(baseFee => new CachedStaticFee(item));
            
            if (!isNew)
                await _db.DeleteFromCacheByIdAsync(_feesKey, fee.Id);
            
            await _db.SortedSetAddAsync(_feesKey, new[] {new SortedSetEntry(value, 0)});
        }

        public async Task DeleteAsync(string id)
        {
            await _repository.DeleteFeeAsync(id);
            await _db.DeleteFromCacheByIdAsync(_feesKey, id);
        }

        public async Task InitAsync()
        {
            //get fees with old format, create a new and delete the old one
            var fees = (await _repository.GetFeesAsync())
                .Where(item => string.IsNullOrEmpty(item.Id)).ToArray();

            if (fees.Length == 0)
                return;

            foreach (var fee in fees)
            {
                await _repository.AddFeeAsync(fee);
                await _repository.DeleteOldFeeAsync(fee.AssetPair);
            }
        }
    }
}
