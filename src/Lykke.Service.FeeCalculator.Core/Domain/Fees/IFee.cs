namespace Lykke.Service.FeeCalculator.Core.Domain.Fees
{
    public interface IFee : IBaseFee
    {
        decimal Volume { get; }
    }
}
