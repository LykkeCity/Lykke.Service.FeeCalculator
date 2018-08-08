using System;
using Lykke.Service.FeeCalculator.Core.Domain.FeeLog;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Service.FeeCalculator.AzureRepositories.FeeLog
{
    public class FeeLogEntryEntity : TableEntity, IFeeLogEntry
    {
        public string Id => RowKey;
        public string Fee { get; set; }
        public FeeOperationType Type { get; set; }
        public string OperationId { get; set; }

        public static FeeLogEntryEntity Create(IFeeLogEntry item)
        {
            return new FeeLogEntryEntity
            {
                PartitionKey = GeneratePartitionKey(),
                RowKey = GenerateRowKey(),
                OperationId = item.OperationId,
                Type = item.Type,
                Fee = item.Fee
            };
        }

        public static string GeneratePartitionKey()
        {
            return DateTime.UtcNow.ToString("yyyy-MM-dd");
        }

        public static string GenerateRowKey()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
