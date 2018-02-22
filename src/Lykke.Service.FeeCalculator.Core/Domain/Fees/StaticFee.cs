namespace Lykke.Service.FeeCalculator.Core.Domain.Fees
{
    public class StaticFee : IStaticFee
    {
        public string AssetPair { get; set; }
        public decimal TakerFee { get; set; }
        public decimal MakerFee { get; set; }
    }
}
