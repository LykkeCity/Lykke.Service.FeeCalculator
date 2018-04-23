using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzureStorage;
using Common;
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
        
        public async Task<IEnumerable<IWithdrawalFeeModel>> GetAllAsync()
        {
            var models = await _tableStorage.GetDataAsync(WithdrawalFeeEntity.GeneratePartitionKey());
            foreach(var model in models)
            {
                model.Countries = model.SerializedCountries?.DeserializeJson<IReadOnlyCollection<string>>();
            }
            return models;
        }

        public async Task SaveAsync(WithdrawalFeeModel fee)
        {
            var entity = WithdrawalFeeEntity.Create(fee);
            await _tableStorage.InsertOrMergeAsync(entity);
        }

    }
}
