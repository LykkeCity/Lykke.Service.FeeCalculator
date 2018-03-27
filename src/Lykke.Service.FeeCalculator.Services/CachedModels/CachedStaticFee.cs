using JetBrains.Annotations;
using Lykke.Service.FeeCalculator.Core.Domain.Fees;
using MessagePack;

namespace Lykke.Service.FeeCalculator.Services.CachedModels
{
    [MessagePackObject]
    public class CachedStaticFee : IStaticFee
    {
        [Key(0)]
        [UsedImplicitly]
        public string Id { get; set; }
        [Key(1)]
        [UsedImplicitly]
        public string AssetPair { get; set; }
        [Key(2)]
        [UsedImplicitly]
        public decimal MakerFee { get; set; }
        [Key(3)]
        [UsedImplicitly]
        public decimal TakerFee { get; set; }
        [Key(4)]
        [UsedImplicitly]
        public FeeType MakerFeeType { get; set; }
        [Key(5)]
        [UsedImplicitly]
        public FeeType TakerFeeType { get; set; }
        [Key(6)]
        [UsedImplicitly]
        public decimal MakerFeeModificator { get; set; }

        [UsedImplicitly]
        public CachedStaticFee()
        {
        }

        public CachedStaticFee(IStaticFee src)
        {
            Id = src.Id;
            AssetPair = src.AssetPair;
            MakerFee = src.MakerFee;
            TakerFee = src.TakerFee;
            MakerFeeType = src.MakerFeeType;
            TakerFeeType = src.TakerFeeType;
            MakerFeeModificator = src.MakerFeeModificator;
        }
    }
}
