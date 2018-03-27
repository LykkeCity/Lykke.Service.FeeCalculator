using System.Collections.Generic;
using System.Threading.Tasks;
using AzureStorage;
using Lykke.Service.FeeCalculator.Core.Domain.CashoutFee;

namespace Lykke.Service.FeeCalculator.AzureRepositories.CashoutFee
{
    public class CashoutFeesRepository : ICashoutFeesRepository
    {
        private readonly INoSQLTableStorage<CashoutFeeEntity> _tableStorage;

        public CashoutFeesRepository(INoSQLTableStorage<CashoutFeeEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }
        
        public async Task<IEnumerable<ICashoutFee>> GetAllAsync()
        {
            return await _tableStorage.GetDataAsync(CashoutFeeEntity.GeneratePartitionKey());
        }

        public async Task<ICashoutFee> AddAsync(ICashoutFee fee)
        {
            var entity = CashoutFeeEntity.Create(fee);
            await _tableStorage.InsertOrMergeAsync(entity);
            return entity;
        }

        public async Task DeleteAsync(string assetId)
        {
            await _tableStorage.DeleteIfExistAsync(CashoutFeeEntity.GeneratePartitionKey(), CashoutFeeEntity.GenerateRowKey(assetId));
        }
    }
}
