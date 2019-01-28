namespace Lykke.Service.FeeCalculator.Core.Domain.Fees
{
    public class BaseFee : IBaseFee
    {
        public string Id { get; set; }
        public decimal TakerFee { get; set; }
        public decimal MakerFee { get; set; }
        public FeeType TakerFeeType { get; set; }
        public FeeType MakerFeeType { get; set; }
        public decimal MakerFeeModificator { get; set; }
    }
}
