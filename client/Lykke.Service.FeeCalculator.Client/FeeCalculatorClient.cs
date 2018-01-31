using System;
using System.Threading.Tasks;
using Common.Log;
using Lykke.Service.FeeCalculator.Client.Models;
using Lykke.Service.FeeCalculator.AutorestClient;
using Lykke.Service.FeeCalculator.AutorestClient.Models;
using System.Collections.Generic;
using Common;

namespace Lykke.Service.FeeCalculator.Client
{
    public class FeeCalculatorClient : IFeeCalculatorClient, IDisposable
    {
        private readonly ILog _log;
        private IFeeCalculatorAPI _service;

        private const string ApiError = "ApiError";

        public FeeCalculatorClient(string serviceUrl, ILog log)
        {
            _log = log;
            _service = new FeeCalculatorAPI(new Uri(serviceUrl));
        }

        public void Dispose()
        {
            if (_service == null)
                return;
            _service.Dispose();
            _service = null;
        }

        public async Task<LimitOrderFeeModel> GetLimitOrderFees(string clientId, string assetPair, string assetId, OrderAction orderAction)
        {
            var response = await _service.GetLimitOrderFeeAsync(clientId, assetPair, assetId, orderAction);

            if (response is ErrorResponse error)
            {
                await _log.WriteErrorAsync(nameof(FeeCalculatorClient), nameof(GetLimitOrderFees),
                    $"clientId = {clientId}, assetPair = {assetPair}, assetId = {assetId}, orderAction = {orderAction}, error = {error.ErrorMessage}", null);

                throw new Exception(error.ErrorMessage);
            }

            if (response is LimitOrderFeeResponseModel result)
            {
                return new LimitOrderFeeModel
                {
                    TakerFeeSize = (decimal) result.TakerFeeSize,
                    MakerFeeSize = (decimal) result.MakerFeeSize
                };
            }

            throw new Exception(ApiError);
        }

        public async Task<MarketOrderFeeModel> GetMarketOrderFees(string clientId, string assetPair, string assetId, OrderAction orderAction)
        {
            var response = await _service.GetMarketOrderFeeAsync(clientId, assetPair, assetId, orderAction);

            if (response is ErrorResponse error)
            {
                await _log.WriteErrorAsync(nameof(FeeCalculatorClient), nameof(GetMarketOrderFees),
                    $"clientId = {clientId}, assetPair = {assetPair}, assetId = {assetId}, orderAction = {orderAction}, error = {error.ErrorMessage}", null);

                throw new Exception(error.ErrorMessage);
            }

            if (response is MarketOrderFeeResponseModel result)
            {
                return new MarketOrderFeeModel
                {
                    DefaultFeeSize = (decimal) result.DefaultFeeSize
                };
            }

            throw new Exception(ApiError);
        }

        public async Task<List<CashoutFee>> GetCashoutFeesAsync(string assetId = null)
        {
            var response = await _service.GetCashoutFeesAsync(assetId);

            if (response is ErrorResponse error)
            {
                await _log.WriteErrorAsync(nameof(FeeCalculatorClient), nameof(GetMarketOrderFees),
                    $"assetId = {assetId}, error = {error.ErrorMessage}", null);

                throw new Exception(error.ErrorMessage);
            }

            if (response is List<CashoutFee> result)
            {
                return result as List<CashoutFee>;
            }

            throw new Exception(ApiError);
        }

        public async Task<BankCardsFeeModel> GetBankCardFees()
        {
            var response = await _service.GetPercentageAsync();

            if (response is ErrorResponse error)
            {
                await _log.WriteErrorAsync(nameof(FeeCalculatorClient), nameof(GetBankCardFees), error.ToJson(), null);

                throw new Exception(error.ErrorMessage);
            }

            if (response is BankCardsFeeResponseModel result)
            {
                return new BankCardsFeeModel {Percentage = result.Percentage};
            }

            throw new Exception(ApiError);
        }
    }
}
