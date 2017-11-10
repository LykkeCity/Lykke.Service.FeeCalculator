using System.Threading.Tasks;
using Lykke.Service.FeeCalculator.Client.Models;
using Lykke.Service.FeeCalculator.AutorestClient.Models;

namespace Lykke.Service.FeeCalculator.Client
{
    public interface IFeeCalculatorClient
    {
        Task<MarketOrderFeeModel> GetMarketOrderFees(string clientId, string assetPair, string assetId,
            OrderAction orderAction);

        Task<LimitOrderFeeModel> GetLimitOrderFees(string clientId, string assetPair, string assetId,
            OrderAction orderAction);
    }
}
