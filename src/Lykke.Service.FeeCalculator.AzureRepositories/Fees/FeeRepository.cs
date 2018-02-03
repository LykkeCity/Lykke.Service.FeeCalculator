using System.Collections.Generic;
using System.Threading.Tasks;
using AzureStorage;
using Lykke.Service.FeeCalculator.Core.Domain.Fees;

namespace Lykke.Service.FeeCalculator.AzureRepositories.Fees
{
    public class FeeRepository : IFeeRepository
    {
        private readonly INoSQLTableStorage<FeeEntity> _tableStorage;

        public FeeRepository(INoSQLTableStorage<FeeEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }
        
        public async Task AddFeeAsync(IFee fee)
        {
            await _tableStorage.InsertOrMergeAsync(FeeEntity.Create(fee));
        }

        public async Task<IEnumerable<IFee>> GetFeesAsync()
        {
            return await _tableStorage.GetDataAsync(FeeEntity.GeneratePartitionKey());
        }

        public async Task DeleteFeeAsync(decimal volume)
        {
            await _tableStorage.DeleteIfExistAsync(FeeEntity.GeneratePartitionKey(), FeeEntity.GenerateRowKey(volume));
        }
    }
}
