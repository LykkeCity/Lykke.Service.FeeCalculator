using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Service.FeeCalculator.Core.Domain.CashoutFee;
using Lykke.Service.FeeCalculator.Core.Extensions;
using Lykke.Service.FeeCalculator.Core.Services;
using Lykke.Service.FeeCalculator.Services.CachedModels;
using Lykke.Service.FeeCalculator.Services.Extensions;
using StackExchange.Redis;

namespace Lykke.Service.FeeCalculator.Services
{
    public class CashoutFeesService : ICashoutFeesService
    {
        private readonly ICashoutFeesRepository _repository;
        private readonly IReadOnlyCollection<CashoutFee> _feesSettings;
        private readonly IDatabase _db;
        private readonly string _feesKey;

        [UsedImplicitly]
        public CashoutFeesService(
            IConnectionMultiplexer connectionMultiplexer,
            ICashoutFeesRepository repository,
            IReadOnlyCollection<CashoutFee> feesSettings,
            string cacheInstanceName
        )
        {
            _db = connectionMultiplexer.GetDatabase();
            _repository = repository;
            _feesSettings = feesSettings;
            _feesKey = $"{cacheInstanceName}:cashoutFees";
        }

        public async Task<ICashoutFee[]> GetAllAsync()
        {
            var serializedValues = await _db.SortedSetRangeByValueAsync(_feesKey);
            var fees = serializedValues.Select(item => ((byte[]) item).DeserializeFee<CachedCashoutFee>()).Select(CashoutFee.Create).ToList();

            if (fees.Count != 0)
                return fees.ToArray();

            fees = (await _repository.GetAllAsync()).Select(CashoutFee.Create).ToList();
            var entites = fees
                .Select(fee => new SortedSetEntry(fee.SerializeFee(baseFee => new CachedCashoutFee(fee)), 0))
                .ToArray();

            await _db.SortedSetAddAsync(_feesKey, entites);

            return fees.ToArray();
        }

        public async Task<ICashoutFee> GetAsync(string assetId)
        {
            return (await GetAllAsync()).FirstOrDefault(item => item.AssetId == assetId);
        }

        public async Task AddAsync(ICashoutFee fee)
        {
            bool isNew = string.IsNullOrEmpty(fee.Id);

            var item = await _repository.AddAsync(fee);
            var value = item.SerializeFee(baseFee => new CachedCashoutFee(item));

            if (!isNew)
                await _db.DeleteFromCacheByIdAsync(_feesKey, fee.Id);

            await _db.SortedSetAddAsync(_feesKey, new[] {new SortedSetEntry(value, 0)});
        }

        public async Task DeleteAsync(string id)
        {
            await _repository.DeleteAsync(id);
            await _db.DeleteFromCacheByIdAsync(_feesKey, id);
        }

        public async Task InitAsync()
        {
            var fees = await GetAllAsync();

            if (!fees.Any() && _feesSettings.Any())
            {
                foreach (var fee in _feesSettings)
                {
                    await AddAsync(fee);
                }
            }
        }
    }
}
