using System.Collections.Generic;
using System.Threading.Tasks;
using AzureStorage;
using Lykke.Service.FeeCalculator.Core.Domain.WithdrawalFee;

namespace Lykke.Service.FeeCalculator.AzureRepositories.WithdrawalFee
{
    public class WithdrawalFeesRepository : IWithdrawalFeesRepository
    {
        private readonly INoSQLTableStorage<WithdrawalFeeEntity> _tableStorage;

        public WithdrawalFeesRepository(INoSQLTableStorage<WithdrawalFeeEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }
        
        public async Task<IEnumerable<IWithdrawalFee>> GetAllAsync()
        {
            return await _tableStorage.GetDataAsync(WithdrawalFeeEntity.GeneratePartitionKey());
        }

        public async Task<IWithdrawalFee> SaveAsync(IWithdrawalFee fee)
        {
            var entity = WithdrawalFeeEntity.Create(fee);
            await _tableStorage.InsertOrMergeAsync(entity);
            return entity;
        }

        public Task DeleteAsync(string assetId)
        {
            return _tableStorage.DeleteIfExistAsync(WithdrawalFeeEntity.GeneratePartitionKey(), WithdrawalFeeEntity.GenerateRowKey(assetId));
        }
    }
}
