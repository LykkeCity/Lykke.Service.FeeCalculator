using System.Threading.Tasks;
using Lykke.Service.FeeCalculator.Core.Domain.Fees;
using Lykke.Service.FeeCalculator.Core.Domain.MarketOrderAssetFee;

namespace Lykke.Service.FeeCalculator.Core.Services
{
    public interface IFeeCalculatorService
    {
        Task<IMarketOrderAssetFee> GetMarketOrderFeeAsync(string clientId, string assetPairId, string assetId);
        Task<IBaseFee> GetFeeAsync(string clientId, string assetPairId, string assetId);
        Task<IBaseFee> GetFeeByPercentageAsync(double percentage);
    }
}
