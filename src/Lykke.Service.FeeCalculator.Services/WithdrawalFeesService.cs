using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Service.FeeCalculator.Core.Domain.CashoutFee;
using Lykke.Service.FeeCalculator.Core.Domain.WithdrawalFee;
using Lykke.Service.FeeCalculator.Core.Extensions;
using Lykke.Service.FeeCalculator.Core.Services;
using Lykke.Service.FeeCalculator.Services.CachedModels;
using Lykke.Service.FeeCalculator.Services.Extensions;
using StackExchange.Redis;

namespace Lykke.Service.FeeCalculator.Services
{
    public class WithdrawalFeesService : IWithdrawalFeesService
    {
        private readonly IWithdrawalFeesRepository _repository;
        private readonly IReadOnlyCollection<WithdrawalFee> _feesSettings;
        private readonly IDatabase _db;
        private readonly string _feesKey;

        [UsedImplicitly]
        public WithdrawalFeesService(
            IConnectionMultiplexer connectionMultiplexer,
            IWithdrawalFeesRepository repository,
            IReadOnlyCollection<WithdrawalFee> feesSettings,
            string cacheInstanceName
        )
        {
            _db = connectionMultiplexer.GetDatabase();
            _repository = repository;
            _feesSettings = feesSettings;
            _feesKey = $"{cacheInstanceName}:withdrawalFees";
        }

        public async Task<IReadOnlyCollection<WithdrawalFeeModel>> GetAllAsync()
        {

            /*
            var serializedValues = await _db.SortedSetRangeByValueAsync(_feesKey);
            var fees = serializedValues.Select(item => ((byte[]) item).DeserializeFee<CachedCashoutFee>()).Select(CashoutFee.Create).ToList();

            if (fees.Count != 0)
                return fees.ToArray();

            fees = (await _repository.GetAllAsync()).Select(CashoutFee.Create).ToList();
            var entites = fees
                .Select(fee => new SortedSetEntry(fee.SerializeFee(baseFee => new CachedCashoutFee(fee)), 0))
                .ToArray();

            await _db.SortedSetAddAsync(_feesKey, entites);
            return fees;

            */
            return await Task.FromResult(new List<WithdrawalFeeModel>());
        }

        public async Task<IWithdrawalFee> GetAsync(string assetId)
        {
            //return (await GetAllAsync()).FirstOrDefault(item => item.AssetId == assetId);
            return await Task.FromResult((IWithdrawalFee)null);
        }

        public async Task SaveAsync(WithdrawalFeeModel fee)
        {
            await Task.FromResult(0);

            /*
            var item = await _repository.SaveAsync(fee);
            var value = item.SerializeFee(baseFee => new CachedCashoutFee(item));

            await _db.SortedSetAddAsync(_feesKey, new[] {new SortedSetEntry(value, 0)});
            */
            return;
        }

        public async Task DeleteAsync(string id)
        {
            /*
            await _repository.DeleteAsync(id);
            await _db.DeleteFromCacheByIdAsync(_feesKey, id);
            */

            await Task.FromResult(0);
        }

        public async Task InitAsync()
        {
            /*
            var fees = await GetAllAsync();

            if (!fees.Any() && _feesSettings.Any())
            {
                foreach (var fee in _feesSettings)
                {
                    await SaveAsync(fee);
                }
            }
            */

            await Task.FromResult(0);
        }

    }
}
