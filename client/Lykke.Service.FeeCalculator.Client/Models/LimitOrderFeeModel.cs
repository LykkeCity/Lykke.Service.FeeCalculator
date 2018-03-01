using Lykke.Service.FeeCalculator.AutorestClient.Models;

namespace Lykke.Service.FeeCalculator.Client.Models
{
    public class LimitOrderFeeModel
    {
        public decimal MakerFeeSize { get; set; }
        public decimal TakerFeeSize { get; set; }
        public FeeType MakerFeeType { get; set; }
        public FeeType TakerFeeType { get; set; }
        public decimal MakerFeeModificator { get; set; }
    }
}
