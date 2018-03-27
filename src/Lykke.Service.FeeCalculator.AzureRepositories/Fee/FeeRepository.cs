using System.Collections.Generic;
using System.Threading.Tasks;
using AzureStorage;
using Lykke.Service.FeeCalculator.Core.Domain.Fees;

namespace Lykke.Service.FeeCalculator.AzureRepositories.Fee
{
    public class FeeRepository : IFeeRepository
    {
        private readonly INoSQLTableStorage<FeeEntity> _tableStorage;

        public FeeRepository(INoSQLTableStorage<FeeEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }
        
        public async Task<IFee> AddFeeAsync(IFee fee)
        {
            var entity = FeeEntity.Create(fee);
            await _tableStorage.InsertOrMergeAsync(entity);
            return entity;
        }

        public async Task<IEnumerable<IFee>> GetFeesAsync()
        {
            return await _tableStorage.GetDataAsync(FeeEntity.GeneratePartitionKey());
        }

        public async Task DeleteFeeAsync(string id)
        {
            await _tableStorage.DeleteIfExistAsync(FeeEntity.GeneratePartitionKey(), FeeEntity.GenerateRowKey(id));
        }
    }
}
