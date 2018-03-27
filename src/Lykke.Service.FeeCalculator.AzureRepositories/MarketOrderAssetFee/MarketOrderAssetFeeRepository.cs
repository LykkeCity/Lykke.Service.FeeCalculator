using System.Collections.Generic;
using System.Threading.Tasks;
using AzureStorage;
using Lykke.Service.FeeCalculator.Core.Domain.MarketOrderAssetFee;

namespace Lykke.Service.FeeCalculator.AzureRepositories.MarketOrderAssetFee
{
    public class MarketOrderAssetFeeRepository : IMarketOrderAssetFeesRepository
    {
        private readonly INoSQLTableStorage<MarketOrderAssetFeeEntity> _tableStorage;

        public MarketOrderAssetFeeRepository(INoSQLTableStorage<MarketOrderAssetFeeEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public async Task<IEnumerable<IMarketOrderAssetFee>> GetAllAsync()
        {
            return await _tableStorage.GetDataAsync(MarketOrderAssetFeeEntity.GeneratePartitionKey());
        }

        public async Task<IMarketOrderAssetFee> AddAsync(IMarketOrderAssetFee assetFee)
        {
            var entity = MarketOrderAssetFeeEntity.Create(assetFee);
            await _tableStorage.InsertOrMergeAsync(entity);
            return entity;
        }

        public async Task DeleteAsync(string id)
        {
            await _tableStorage.DeleteIfExistAsync(MarketOrderAssetFeeEntity.GeneratePartitionKey(), MarketOrderAssetFeeEntity.GenerateRowKey(id));
        }
    }
}
