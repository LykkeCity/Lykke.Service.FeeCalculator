namespace Lykke.Service.FeeCalculator.Core.Domain.Fees
{
    public class BaseFee : IBaseFee
    {
        public decimal TakerFee { get; set; }
        public decimal MakerFee { get; set; }
        public decimal MakerFeeModificator { get; set; }
    }
}
