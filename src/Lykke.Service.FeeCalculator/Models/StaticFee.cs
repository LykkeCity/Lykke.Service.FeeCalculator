using Lykke.Service.FeeCalculator.Core.Domain.Fees;

namespace Lykke.Service.FeeCalculator.Models
{
    public class StaticFee
    {
        public string Id { get; set; }
        public string AssetPair { get; set; }
        public decimal TakerFee { get; set; }
        public decimal MakerFee { get; set; }
        public FeeType TakerFeeType { get; set; }
        public FeeType MakerFeeType { get; set; }
        public decimal MakerFeeModificator { get; set; }
    }
}
