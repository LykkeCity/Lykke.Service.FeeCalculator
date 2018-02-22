using System;
using System.Globalization;
using Lykke.AzureStorage.Tables;
using Lykke.AzureStorage.Tables.Entity.Annotation;
using Lykke.AzureStorage.Tables.Entity.ValueTypesMerging;
using Lykke.Service.FeeCalculator.Core.Domain.Fees;

namespace Lykke.Service.FeeCalculator.AzureRepositories.Fees
{
    [ValueTypeMergingStrategy(ValueTypeMergingStrategy.UpdateAlways)]
    public class FeeEntity : AzureTableEntity, IFee
    {
        public string Id { get; set; }
        public decimal Volume { get; set; }
        public decimal TakerFee { get; set; }
        public decimal MakerFee { get; set; }

        internal static string GeneratePartitionKey() => "Fee";
        internal static string GenerateRowKey(string id) => id;

        public static FeeEntity Create(IFee fee)
        {
            string id = fee.Id ?? Guid.NewGuid().ToString();
            
            return new FeeEntity
            {
                PartitionKey = GeneratePartitionKey(),
                RowKey = GenerateRowKey(id),
                Id = id,
                Volume = fee.Volume,
                MakerFee = fee.MakerFee,
                TakerFee = fee.TakerFee
            };
        }
    }
}
