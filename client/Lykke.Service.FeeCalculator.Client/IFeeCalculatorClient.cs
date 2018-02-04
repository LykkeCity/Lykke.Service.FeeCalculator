using System.Threading.Tasks;
using Lykke.Service.FeeCalculator.Client.Models;
using System.Collections.Generic;
using Lykke.Service.FeeCalculator.AutorestClient.Models;

namespace Lykke.Service.FeeCalculator.Client
{
    public interface IFeeCalculatorClient
    {
        Task<MarketOrderFeeModel> GetMarketOrderFees(string clientId, string assetPair, string assetId,
            OrderAction orderAction);

        Task<LimitOrderFeeModel> GetLimitOrderFees(string clientId, string assetPair, string assetId,
            OrderAction orderAction);

        Task<List<CashoutFee>> GetCashoutFeesAsync(string assetId = null);
        
        Task<BankCardsFeeModel> GetBankCardFees();

        Task AddFeeAsync(FeeModel fee);
        Task<List<Fee>> GetFeesAsync();
        Task DeleteFeeAsync(string id);
        
        Task AddStaticFeeAsync(StaticFeeModel fee);
        Task<List<StaticFee>> GetStaticFeesAsync();
        Task DeleteStaticFeeAsync(string assetPair);
    }
}
