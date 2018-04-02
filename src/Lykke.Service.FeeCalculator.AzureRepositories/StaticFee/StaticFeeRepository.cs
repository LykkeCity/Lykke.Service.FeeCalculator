using System.Collections.Generic;
using System.Threading.Tasks;
using AzureStorage;
using Lykke.Service.FeeCalculator.Core.Domain.Fees;

namespace Lykke.Service.FeeCalculator.AzureRepositories.StaticFee
{
    public class StaticFeeRepository : IStaticFeeRepository
    {
        private readonly INoSQLTableStorage<StaticFeeEntity> _tableStorage;

        public StaticFeeRepository(INoSQLTableStorage<StaticFeeEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }
        
        public async Task<IStaticFee> AddFeeAsync(IStaticFee fee)
        {
            var entity = StaticFeeEntity.Create(fee);
            await _tableStorage.InsertOrMergeAsync(entity);
            return entity;
        }

        public async Task<IEnumerable<IStaticFee>> GetFeesAsync()
        {
            return await _tableStorage.GetDataAsync(StaticFeeEntity.GeneratePartitionKey());
        }

        public async Task DeleteFeeAsync(string id)
        {
            await _tableStorage.DeleteIfExistAsync(StaticFeeEntity.GeneratePartitionKey(), StaticFeeEntity.GenerateRowKey(id));
        }

        public async Task DeleteOldFeeAsync(string assetPair)
        {
            await _tableStorage.DeleteIfExistAsync(StaticFeeEntity.GeneratePartitionKey(), StaticFeeEntity.GenerateOldRowKey(assetPair));
        }
    }
}
