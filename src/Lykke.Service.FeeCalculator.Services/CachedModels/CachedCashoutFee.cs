using JetBrains.Annotations;
using Lykke.Service.FeeCalculator.Core.Domain.CashoutFee;
using Lykke.Service.FeeCalculator.Core.Domain.Fees;
using MessagePack;

namespace Lykke.Service.FeeCalculator.Services.CachedModels
{
    [MessagePackObject]
    public class CachedCashoutFee : ICashoutFee
    {
        [Key(0)]
        [UsedImplicitly]
        public string Id { get; set; }
        [Key(1)]
        [UsedImplicitly]
        public string AssetId { get; set; }
        [Key(2)]
        [UsedImplicitly]
        public double Size { get; set; }
        [Key(3)]
        [UsedImplicitly]
        public FeeType Type { get; set; }
        
        [UsedImplicitly]
        public CachedCashoutFee()
        {
        }

        public CachedCashoutFee(ICashoutFee src)
        {
            Id = src.Id;
            AssetId = src.AssetId;
            Size = src.Size;
            Type = src.Type;
        }
    }
}
