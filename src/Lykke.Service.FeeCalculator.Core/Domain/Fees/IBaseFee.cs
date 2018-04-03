using Lykke.Service.FeeCalculator.Core.Settings.ServiceSettings;

namespace Lykke.Service.FeeCalculator.Core.Domain.Fees
{
    public interface IBaseFee
    {
        decimal TakerFee { get; }
        decimal MakerFee { get; }
        FeeType TakerFeeType { get; }
        FeeType MakerFeeType { get; }
        decimal MakerFeeModificator { get; set; }
    }
}
