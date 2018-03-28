using Lykke.Service.FeeCalculator.Core.Domain.Fees;

namespace Lykke.Service.FeeCalculator.Models
{
    public class MoAssetFee
    {
        public string Id { get; set; }
        public decimal Amount { get; set; }
        public FeeType Type { get; set; }
        public string AssetId { get; set; }
        public string TargetAssetId { get; set; }
        public string TargetWalletId { get; set; }
    }
}
