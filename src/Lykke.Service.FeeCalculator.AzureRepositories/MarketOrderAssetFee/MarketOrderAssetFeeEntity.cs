using System;
using Lykke.AzureStorage.Tables;
using Lykke.AzureStorage.Tables.Entity.Annotation;
using Lykke.AzureStorage.Tables.Entity.ValueTypesMerging;
using Lykke.Service.FeeCalculator.Core.Domain.Fees;
using Lykke.Service.FeeCalculator.Core.Domain.MarketOrderAssetFee;

namespace Lykke.Service.FeeCalculator.AzureRepositories.MarketOrderAssetFee
{
    [ValueTypeMergingStrategy(ValueTypeMergingStrategy.UpdateAlways)]
    public class MarketOrderAssetFeeEntity : AzureTableEntity, IMarketOrderAssetFee
    {
        public string Id { get; set; }
        public decimal Amount { get; set; }
        public FeeType Type { get; set; }
        public string AssetId { get; set; }
        public string TargetAssetId { get; set; }
        public string TargetWalletId { get; set; }

        internal static string GeneratePartitionKey() => "MarketOrderAssetFee";
        internal static string GenerateRowKey(string id) => id;

        public static MarketOrderAssetFeeEntity Create(IMarketOrderAssetFee assetFee)
        {
            string id = assetFee.Id ?? Guid.NewGuid().ToString();
            
            return new MarketOrderAssetFeeEntity
            {
                PartitionKey = GeneratePartitionKey(),
                RowKey = GenerateRowKey(id),
                Id = id,
                Amount = assetFee.Amount,
                Type = assetFee.Type,
                AssetId = assetFee.AssetId,
                TargetAssetId = assetFee.TargetAssetId,
                TargetWalletId = assetFee.TargetWalletId
            };
        }
    }
}
