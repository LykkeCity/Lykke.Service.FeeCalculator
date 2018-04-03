using System;
using Lykke.AzureStorage.Tables;
using Lykke.AzureStorage.Tables.Entity.Annotation;
using Lykke.AzureStorage.Tables.Entity.ValueTypesMerging;
using Lykke.Service.FeeCalculator.Core.Domain.CashoutFee;
using Lykke.Service.FeeCalculator.Core.Domain.Fees;

namespace Lykke.Service.FeeCalculator.AzureRepositories.CashoutFee
{
    [ValueTypeMergingStrategy(ValueTypeMergingStrategy.UpdateAlways)]
    public class CashoutFeeEntity : AzureTableEntity, ICashoutFee
    {
        public string Id { get; set; }
        public string AssetId { get; set; }
        public double Size { get; set; }
        public FeeType Type { get; set; }

        internal static string GeneratePartitionKey() => "CashoutFee";
        internal static string GenerateRowKey(string id) => id;


        internal static CashoutFeeEntity Create(ICashoutFee fee)
        {
            string id = fee.Id ?? Guid.NewGuid().ToString();
            
            return new CashoutFeeEntity
            {
                PartitionKey = GeneratePartitionKey(),
                RowKey = GenerateRowKey(id),
                Id = id,
                AssetId = fee.AssetId,
                Size = fee.Size,
                Type = fee.Type
            };
        }
    }
}
