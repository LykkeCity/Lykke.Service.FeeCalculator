using Lykke.Service.FeeCalculator.Core.Domain.Fees;
using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.FeeCalculator.Core.Domain.MarketOrderAssetFee
{
    public class MarketOrderAssetFee : IMarketOrderAssetFee
    {
        [Optional]
        public string Id { get; set; }
        public decimal Amount { get; set; }
        public FeeType Type { get; set; }
        public string AssetId { get; set; }
        public string TargetAssetId { get; set; }
        public string TargetWalletId { get; set; }

        public static IMarketOrderAssetFee Create(IMarketOrderAssetFee src)
        {
            return new MarketOrderAssetFee
            {
                Id = src.Id,
                Amount = src.Amount,
                Type = src.Type,
                AssetId = src.AssetId,
                TargetAssetId = src.TargetAssetId,
                TargetWalletId = src.TargetWalletId
            };
        }
    }
}
