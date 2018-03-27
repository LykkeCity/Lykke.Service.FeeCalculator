using Lykke.Service.FeeCalculator.Core.Domain.Fees;

namespace Lykke.Service.FeeCalculator.Models
{
    public class CashoutFee
    {
        public string Id { get; set; }
        public string AssetId { get; set; }
        public double Size { get; set; }
        public FeeType Type { get; set; }
    }
}
