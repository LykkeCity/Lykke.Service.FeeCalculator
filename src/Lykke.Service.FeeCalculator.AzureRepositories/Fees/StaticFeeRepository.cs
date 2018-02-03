using System.Collections.Generic;
using System.Threading.Tasks;
using AzureStorage;
using Lykke.Service.FeeCalculator.Core.Domain.Fees;

namespace Lykke.Service.FeeCalculator.AzureRepositories.Fees
{
    public class StaticFeeRepository : IStaticFeeRepository
    {
        private readonly INoSQLTableStorage<StaticFeeEntity> _tableStorage;

        public StaticFeeRepository(INoSQLTableStorage<StaticFeeEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }
        
        public async Task AddFeeAsync(IStaticFee fee)
        {
            await _tableStorage.InsertOrMergeAsync(StaticFeeEntity.Create(fee));
        }

        public async Task<IEnumerable<IStaticFee>> GetFeesAsync()
        {
            return await _tableStorage.GetDataAsync(StaticFeeEntity.GeneratePartitionKey());
        }

        public async Task DeleteFeeAsync(string assetPair)
        {
            await _tableStorage.DeleteIfExistAsync(StaticFeeEntity.GeneratePartitionKey(), StaticFeeEntity.GenerateRowKey(assetPair));
        }
    }
}
