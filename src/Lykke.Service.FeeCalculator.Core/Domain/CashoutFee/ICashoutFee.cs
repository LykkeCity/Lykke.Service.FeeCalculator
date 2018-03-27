using Lykke.Service.FeeCalculator.Core.Domain.Fees;

namespace Lykke.Service.FeeCalculator.Core.Domain.CashoutFee
{
    public interface ICashoutFee : IFeeId
    {
        string AssetId { get; set; }
        double Size { get; set; }
        FeeType Type { get; set; }
    }
}
