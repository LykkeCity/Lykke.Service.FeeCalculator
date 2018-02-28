using Lykke.AzureStorage.Tables;
using Lykke.AzureStorage.Tables.Entity.Annotation;
using Lykke.AzureStorage.Tables.Entity.ValueTypesMerging;
using Lykke.Service.FeeCalculator.Core.Domain.Fees;

namespace Lykke.Service.FeeCalculator.AzureRepositories.Fees
{
    [ValueTypeMergingStrategy(ValueTypeMergingStrategy.UpdateAlways)]
    public class StaticFeeEntity : AzureTableEntity, IStaticFee
    {
        public string AssetPair { get; set; }
        public decimal TakerFee { get; set; }
        public decimal MakerFee { get; set; }
        public decimal MakerFeeModificator { get; set; }

        internal static string GeneratePartitionKey() => "StaticFee";
        internal static string GenerateRowKey(string assetPair) => assetPair;

        internal static StaticFeeEntity Create(IStaticFee fee) => new StaticFeeEntity
        {
            PartitionKey = GeneratePartitionKey(),
            RowKey = GenerateRowKey(fee.AssetPair),
            AssetPair = fee.AssetPair,
            MakerFee = fee.MakerFee,
            TakerFee = fee.TakerFee,
            MakerFeeModificator = fee.MakerFeeModificator
        };
    }
}
