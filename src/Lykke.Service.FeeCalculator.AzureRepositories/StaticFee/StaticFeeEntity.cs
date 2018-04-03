using System;
using Lykke.AzureStorage.Tables;
using Lykke.AzureStorage.Tables.Entity.Annotation;
using Lykke.AzureStorage.Tables.Entity.ValueTypesMerging;
using Lykke.Service.FeeCalculator.Core.Domain.Fees;

namespace Lykke.Service.FeeCalculator.AzureRepositories.StaticFee
{
    [ValueTypeMergingStrategy(ValueTypeMergingStrategy.UpdateAlways)]
    public class StaticFeeEntity : AzureTableEntity, IStaticFee
    {
        public string Id { get; set; }
        public string AssetPair { get; set; }
        public decimal TakerFee { get; set; }
        public decimal MakerFee { get; set; }
        public FeeType TakerFeeType { get; set; }
        public FeeType MakerFeeType { get; set; }
        public decimal MakerFeeModificator { get; set; }

        internal static string GeneratePartitionKey() => "StaticFee";
        internal static string GenerateRowKey(string id) => id;
        internal static string GenerateOldRowKey(string assetPair) => assetPair;

        internal static StaticFeeEntity Create(IStaticFee fee)
        {
            string id = fee.Id ?? Guid.NewGuid().ToString();

            return new StaticFeeEntity
            {
                PartitionKey = GeneratePartitionKey(),
                RowKey = GenerateRowKey(id),
                Id = id,
                AssetPair = fee.AssetPair,
                MakerFee = fee.MakerFee,
                TakerFee = fee.TakerFee,
                MakerFeeType = fee.MakerFeeType,
                TakerFeeType = fee.TakerFeeType,
                MakerFeeModificator = fee.MakerFeeModificator
            };
        }
    }
}
