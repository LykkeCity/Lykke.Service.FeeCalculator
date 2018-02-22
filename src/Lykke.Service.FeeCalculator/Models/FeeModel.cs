namespace Lykke.Service.FeeCalculator.Models
{
    public class FeeModel
    {
        public string Id { get; set; }
        public decimal Volume { get; set; }
        public decimal MakerFee { get; set; }
        public decimal TakerFee { get; set; }
    }
}
