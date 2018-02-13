using System.Threading.Tasks;
using Lykke.Service.FeeCalculator.Core.Domain.Fees;

namespace Lykke.Service.FeeCalculator.Core.Services
{
    public interface IFeeService
    {
        Task<MarketOrderFee> GetMarketOrderFeeAsync(string clientId, string assetPairId, string assetId);
        Task<IBaseFee> GetFeeAsync(string clientId, string assetPairId, string assetId);
        Task<IBaseFee> GetFeeByPercentageAsync(double percentage);
    }
}
