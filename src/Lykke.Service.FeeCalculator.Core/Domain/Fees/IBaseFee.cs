namespace Lykke.Service.FeeCalculator.Core.Domain.Fees
{
    public interface IBaseFee
    {
        decimal TakerFee { get; }
        decimal MakerFee { get; }
    }
}
