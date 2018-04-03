using Lykke.Service.FeeCalculator.Core.Domain.Fees;

namespace Lykke.Service.FeeCalculator.Core.Domain.MarketOrderAssetFee
{
    public interface IMarketOrderAssetFee : IFeeId
    {
        decimal Amount { get; }
        FeeType Type { get; }
        string AssetId { get; }
        string TargetAssetId { get; }
        string TargetWalletId { get; }
    }
}
