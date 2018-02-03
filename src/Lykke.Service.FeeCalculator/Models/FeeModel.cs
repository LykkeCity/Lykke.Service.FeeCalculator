namespace Lykke.Service.FeeCalculator.Models
{
    public class FeeModel
    {
        public decimal Volume { get; set; }
        public decimal MakerFee { get; set; }
        public decimal TakerFee { get; set; }
    }
}
