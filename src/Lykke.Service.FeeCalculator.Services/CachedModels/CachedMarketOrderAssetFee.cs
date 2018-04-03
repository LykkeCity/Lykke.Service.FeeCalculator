using JetBrains.Annotations;
using Lykke.Service.FeeCalculator.Core.Domain.Fees;
using Lykke.Service.FeeCalculator.Core.Domain.MarketOrderAssetFee;
using MessagePack;

namespace Lykke.Service.FeeCalculator.Services.CachedModels
{
    [MessagePackObject]
    public class CachedMarketOrderAssetFee : IMarketOrderAssetFee
    {
        [Key(0)]
        [UsedImplicitly]
        public string Id { get; set; }
        [Key(1)]
        [UsedImplicitly]
        public decimal Amount { get; set; }
        [Key(2)]
        [UsedImplicitly]
        public FeeType Type { get; set; }
        [Key(3)]
        [UsedImplicitly]
        public string AssetId { get; set; }
        [Key(4)]
        [UsedImplicitly]
        public string TargetAssetId { get; set; }
        [Key(5)]
        [UsedImplicitly]
        public string TargetWalletId { get; set; }
        
        [UsedImplicitly]
        public CachedMarketOrderAssetFee()
        {
        }

        public CachedMarketOrderAssetFee(IMarketOrderAssetFee src)
        {
            Id = src.Id;
            Amount = src.Amount;
            Type = src.Type;
            AssetId = src.AssetId;
            TargetAssetId = src.TargetAssetId;
            TargetWalletId = src.TargetWalletId;
        }
    }
}
