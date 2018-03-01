using Lykke.Service.FeeCalculator.Core.Settings.ServiceSettings;

namespace Lykke.Service.FeeCalculator.Models
{
    public class StaticFeeModel
    {
        public string AssetPair { get; set; }
        public decimal MakerFee { get; set; }
        public decimal TakerFee { get; set; }
        public FeeType MakerFeeType { get; set; }
        public FeeType TakerFeeType { get; set; }
        public decimal MakerFeeModificator { get; set; }
    }
}
