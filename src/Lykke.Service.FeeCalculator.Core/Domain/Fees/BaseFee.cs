using Lykke.Service.FeeCalculator.Core.Settings.ServiceSettings;

namespace Lykke.Service.FeeCalculator.Core.Domain.Fees
{
    public class BaseFee : IBaseFee
    {
        public decimal TakerFee { get; set; }
        public decimal MakerFee { get; set; }
        public FeeType TakerFeeType { get; set; }
        public FeeType MakerFeeType { get; set; }
        public decimal MakerFeeModificator { get; set; }
    }
}
