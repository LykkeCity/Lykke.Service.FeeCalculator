namespace Lykke.Service.FeeCalculator.Core.Domain.Fees
{
    public interface IFee
    {
        decimal Volume { get; }
        decimal TakerFee { get; }
        decimal MakerFee { get; }
    }
}
