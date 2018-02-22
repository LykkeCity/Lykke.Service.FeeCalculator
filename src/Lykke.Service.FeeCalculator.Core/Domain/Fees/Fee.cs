namespace Lykke.Service.FeeCalculator.Core.Domain.Fees
{
    public class Fee : IFee
    {
        public string Id { get; set; }
        public decimal Volume { get; set; }
        public decimal TakerFee { get; set; }
        public decimal MakerFee { get; set; }
    }
}
