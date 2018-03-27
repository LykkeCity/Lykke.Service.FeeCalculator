namespace Lykke.Service.FeeCalculator.Core.Domain.Fees
{
    public class Fee : IFee
    {
        public string Id { get; set; }
        public decimal Volume { get; set; }
        public decimal TakerFee { get; set; }
        public decimal MakerFee { get; set; }
        public FeeType TakerFeeType { get; set; }
        public FeeType MakerFeeType { get; set; }
        public decimal MakerFeeModificator { get; set; }

        public static IFee Create(IFee src)
        {
            return new Fee
            {
                Id = src.Id,
                Volume = src.Volume,
                TakerFee = src.TakerFee,
                MakerFee = src.MakerFee,
                MakerFeeType = src.MakerFeeType,
                TakerFeeType = src.TakerFeeType,
                MakerFeeModificator = src.MakerFeeModificator
            };
        }
    }
}
