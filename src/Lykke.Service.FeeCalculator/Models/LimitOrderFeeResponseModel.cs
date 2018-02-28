namespace Lykke.Service.FeeCalculator.Models
{
    public class LimitOrderFeeResponseModel
    {
        public decimal MakerFeeSize { get; set; }
        public decimal TakerFeeSize { get; set; }
        public decimal MakerFeeModificator { get; set; }
    }
}
