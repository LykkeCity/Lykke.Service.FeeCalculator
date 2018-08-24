using System.Threading.Tasks;
using AzureStorage;
using Lykke.Service.FeeCalculator.Core.Domain.FeeLog;

namespace Lykke.Service.FeeCalculator.AzureRepositories.FeeLog
{
    public class FeeLogRepository : IFeeLogRepository
    {
        private readonly INoSQLTableStorage<FeeLogEntryEntity> _tableStorage;

        public FeeLogRepository(INoSQLTableStorage<FeeLogEntryEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public Task CreateAsync(IFeeLogEntry item)
        {
            return _tableStorage.InsertOrReplaceAsync(FeeLogEntryEntity.Create(item));
        }
    }
}
