namespace Lykke.Service.FeeCalculator.Core.Domain.Fees
{
    public interface IFee : IBaseFee
    {
        string Id { get; }
        decimal Volume { get; }
    }
}
