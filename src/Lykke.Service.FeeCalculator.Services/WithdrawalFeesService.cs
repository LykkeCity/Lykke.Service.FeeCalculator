using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;
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
        private readonly IDatabase _db;
        private readonly string _feesKey;

        [UsedImplicitly]
        public WithdrawalFeesService(
            IConnectionMultiplexer connectionMultiplexer,
            IWithdrawalFeesRepository repository,
            string cacheInstanceName
        )
        {
            _db = connectionMultiplexer.GetDatabase();
            _repository = repository;
            _feesKey = $"{cacheInstanceName}:withdrawalFees";
        }

        public async Task<IEnumerable<IWithdrawalFeeModel>> GetAllAsync()
        {
            var serializedValues = await _db.HashGetAllAsync(_feesKey);
            var feesFromCache = serializedValues.Select(_ => _.Value.ToString().DeserializeJson<WithdrawalFeeModel>()).ToList();
            if (feesFromCache.Count != 0)
            {
                return feesFromCache.ToArray();
            }

            var fees = await _repository.GetAllAsync();
            await _db.HashSetAsync(_feesKey, fees.Select(_ => new HashEntry(_.AssetId, _.ToJson())).ToArray());
            return fees;
        }

        public async Task<WithdrawalFeeModel> GetAsync(string assetId)
        {
            var serializedFee = await _db.HashGetAsync(_feesKey, assetId);
            WithdrawalFeeModel model = serializedFee.ToString()?.DeserializeJson<WithdrawalFeeModel>();
            if (model == null)
            {
                return new WithdrawalFeeModel()
                {
                    AssetId = assetId,
                    PaymentSystemForOtherCountries = PaymentSystemType.Swift,
                    SizeForOtherCountries = 0
                };
            }
            return model;
        }

        public async Task SaveAsync(WithdrawalFeeModel fee)
        {
            await _repository.SaveAsync(fee);

            var he = new HashEntry(fee.AssetId, fee.ToJson());
            await _db.HashSetAsync(_feesKey, new HashEntry[] { he });

            var vals = _db.HashValuesAsync(_feesKey);
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
