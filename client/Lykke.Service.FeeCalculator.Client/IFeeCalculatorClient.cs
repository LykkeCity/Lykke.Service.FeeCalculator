using System.Threading.Tasks;
using Lykke.Service.FeeCalculator.Client.AutorestClient.Models;
using Lykke.Service.FeeCalculator.Client.Models;

namespace Lykke.Service.FeeCalculator.Client
{
    public interface IFeeCalculatorClient
    {
        Task<TradeFeeModel> GetTradeFees(string clientId, string assetPair, string assetId, OrderAction orderAction);
    }
}
