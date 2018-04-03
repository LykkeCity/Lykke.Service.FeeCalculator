using Lykke.Service.FeeCalculator.Core.Domain.Fees;

namespace Lykke.Service.FeeCalculator.Models
{
    public class LimitOrderFeeResponseModel
    {
        public decimal MakerFeeSize { get; set; }
        public decimal TakerFeeSize { get; set; }
        public FeeType MakerFeeType { get; set; }
        public FeeType TakerFeeType { get; set; }
        public decimal MakerFeeModificator { get; set; }
    }
}
