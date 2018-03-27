using Lykke.Service.FeeCalculator.Core.Domain.Fees;
using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.FeeCalculator.Core.Domain.CashoutFee
{
    public class CashoutFee : ICashoutFee
    {
        [Optional]
        public string Id { get; set; }
        public string AssetId { get; set; }
        public double Size { get; set; }
        public FeeType Type { get; set; }

        public override string ToString() => $"{AssetId}: {Size} ({Type})";

        public static ICashoutFee Create(ICashoutFee src)
        {
            return new CashoutFee
            {
                Id = src.Id,
                AssetId = src.AssetId,
                Size = src.Size,
                Type = src.Type
            };
        }
    }
}
