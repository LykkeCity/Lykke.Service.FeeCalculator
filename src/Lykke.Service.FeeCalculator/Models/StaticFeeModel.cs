namespace Lykke.Service.FeeCalculator.Models
{
    public class StaticFeeModel
    {
        public string AssetPair { get; set; }
        public decimal MakerFee { get; set; }
        public decimal TakerFee { get; set; }
    }
}
