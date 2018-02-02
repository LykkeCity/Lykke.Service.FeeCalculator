namespace Lykke.Service.FeeCalculator.Core.Domain.Fees
{
    public interface IStaticFee : IBaseFee
    {
        string AssetPair { get; }
    }
}
