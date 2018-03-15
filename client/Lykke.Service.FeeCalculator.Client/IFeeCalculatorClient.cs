using System;
using System.Threading.Tasks;
using Lykke.Service.FeeCalculator.Client.Models;
using System.Collections.Generic;
using Lykke.Service.FeeCalculator.AutorestClient.Models;

namespace Lykke.Service.FeeCalculator.Client
{
    public interface IFeeCalculatorClient
    {
        /// <summary>
        /// Gets market order fee
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="assetPair"></param>
        /// <param name="assetId"></param>
        /// <param name="orderAction"></param>
        /// <returns></returns>
        [Obsolete("Use MarketOrderAssetFee method")]
        Task<MarketOrderFeeModel> GetMarketOrderFees(string clientId, string assetPair, string assetId,
            OrderAction orderAction);
        
        Task<MarketOrderAssetFeeModel> GetMarketOrderAssetFee(string clientId, string assetPair, string assetId, 
            OrderAction orderAction);

        /// <summary>
        /// Gets limit order fee
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="assetPair"></param>
        /// <param name="assetId"></param>
        /// <param name="orderAction"></param>
        /// <returns></returns>
        Task<LimitOrderFeeModel> GetLimitOrderFees(string clientId, string assetPair, string assetId,
            OrderAction orderAction);

        /// <summary>
        /// Gets cashout fee
        /// </summary>
        /// <param name="assetId"></param>
        /// <returns></returns>
        Task<IReadOnlyCollection<CashoutFee>> GetCashoutFeesAsync(string assetId = null);

        /// <summary>
        /// Gets withdrawal fee
        /// </summary>
        /// <param name="assetId"></param>
        /// <param name="countryCode"></param>
        /// <returns></returns>
        Task<WithdrawalFeeModel> GetWithdrawalFeeAsync(string assetId, string countryCode);

        /// <summary>
        /// Gets bank card fee
        /// </summary>
        /// <returns></returns>
        Task<BankCardsFeeModel> GetBankCardFees();

        /// <summary>
        /// Adds a dynamic fee
        /// </summary>
        /// <param name="fee"></param>
        /// <returns></returns>
        Task AddFeeAsync(FeeModel fee);
        
        /// <summary>
        /// Gets all the dynamic fees
        /// </summary>
        /// <returns></returns>
        Task<IReadOnlyCollection<Fee>> GetFeesAsync();
        
        /// <summary>
        /// Deletes the dynamic fee by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteFeeAsync(string id);
        
        /// <summary>
        /// Adds a static fee
        /// </summary>
        /// <param name="fee"></param>
        /// <returns></returns>
        Task AddStaticFeeAsync(StaticFeeModel fee);
        
        /// <summary>
        /// Gets all the static fees
        /// </summary>
        /// <returns></returns>
        Task<IReadOnlyCollection<StaticFee>> GetStaticFeesAsync();
        
        /// <summary>
        /// Deletes the static fee by the asset pair
        /// </summary>
        /// <param name="assetPair"></param>
        /// <returns></returns>
        Task DeleteStaticFeeAsync(string assetPair);
    }
}
