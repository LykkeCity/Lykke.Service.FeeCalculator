namespace Lykke.Service.FeeCalculator.Core.Domain.Fees
{
    public class StaticFee : IStaticFee
    {
        public string Id { get; set; }
        public string AssetPair { get; set; }
        public decimal TakerFee { get; set; }
        public decimal MakerFee { get; set; }
        public FeeType TakerFeeType { get; set; }
        public FeeType MakerFeeType { get; set; }
        public decimal MakerFeeModificator { get; set; }

        public static IStaticFee Create(IStaticFee src)
        {
            return new StaticFee
            {
                Id = src.Id,
                AssetPair = src.AssetPair,
                MakerFee = src.MakerFee,
                TakerFee = src.TakerFee,
                MakerFeeType = src.MakerFeeType,
                TakerFeeType = src.TakerFeeType,
                MakerFeeModificator = src.MakerFeeModificator
            };
        }
    }
}
