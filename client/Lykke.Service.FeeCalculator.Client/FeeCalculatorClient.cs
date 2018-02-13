using System;
using System.Threading.Tasks;
using Common.Log;
using Lykke.Service.FeeCalculator.Client.Models;
using System.Collections.Generic;
using Common;
using Lykke.Service.FeeCalculator.AutorestClient;
using Lykke.Service.FeeCalculator.AutorestClient.Models;

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
            var response = await _service.GetLimitOrderFeeAsync(orderAction, clientId, assetPair, assetId);

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
                    TakerFeeSize = result.TakerFeeSize,
                    MakerFeeSize = result.MakerFeeSize
                };
            }

            throw new Exception(ApiError);
        }

        public async Task<MarketOrderFeeModel> GetMarketOrderFees(string clientId, string assetPair, string assetId, OrderAction orderAction)
        {
            var response = await _service.GetMarketOrderFeeAsync(orderAction, clientId, assetPair, assetId);

            if (response is ErrorResponse error)
            {
                await _log.WriteErrorAsync(nameof(FeeCalculatorClient), nameof(GetMarketOrderFees),
                    $"clientId = {clientId}, assetPair = {assetPair}, assetId = {assetId}, orderAction = {orderAction}, error = {error.ErrorMessage}", null);

                throw new Exception(error.ErrorMessage);
            }

            if (response is MarketOrderFee result)
            {
                return new MarketOrderFeeModel
                {
                    Amount = result.Amount,
                    AssetId = result.AssetId,
                    TargetAssetId = result.TargetAssetId,
                    TargetWalletId = result.TargetWalletId,
                    Type = result.Type
                };
            }

            throw new Exception(ApiError);
        }

        public async Task<IReadOnlyCollection<CashoutFee>> GetCashoutFeesAsync(string assetId = null)
        {
            var response = await _service.GetCashoutFeesAsync(assetId);

            if (response is ErrorResponse error)
            {
                await _log.WriteErrorAsync(nameof(FeeCalculatorClient), nameof(GetMarketOrderFees),
                    $"assetId = {assetId}, error = {error.ErrorMessage}", null);

                throw new Exception(error.ErrorMessage);
            }

            if (response is List<CashoutFee> result)
                return result;

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

        public async Task AddFeeAsync(FeeModel fee)
        {
            var response = await _service.AddFeeAsync(fee);

            if (response is ErrorResponse error)
                throw new Exception(error.ErrorMessage);

            if (response is bool)
                return;

            throw new Exception(ApiError);
        }

        public async Task<IReadOnlyCollection<Fee>> GetFeesAsync()
        {
            var response = await _service.GetFeesAsync();

            if (response is ErrorResponse error)
                throw new Exception(error.ErrorMessage);

            if (response is List<Fee> fees)
                return fees;

            throw new Exception(ApiError);
        }

        public async Task DeleteFeeAsync(string id)
        {
            var response = await _service.DeleteFeeAsync(id);

            if (response is ErrorResponse error)
                throw new Exception(error.ErrorMessage);

            if (response is bool)
                return;

            throw new Exception(ApiError);
        }

        public async Task AddStaticFeeAsync(StaticFeeModel fee)
        {
            var response = await _service.AddStaticFeeAsync(fee);

            if (response is ErrorResponse error)
                throw new Exception(error.ErrorMessage);

            if (response is bool)
                return;

            throw new Exception(ApiError);
        }

        public async Task<IReadOnlyCollection<StaticFee>> GetStaticFeesAsync()
        {
            var response = await _service.GetStaticFeesAsync();

            if (response is ErrorResponse error)
                throw new Exception(error.ErrorMessage);

            if (response is List<StaticFee> fees)
                return fees;

            throw new Exception(ApiError);
        }

        public async Task DeleteStaticFeeAsync(string assetPair)
        {
            var response = await _service.DeleteStaticFeeAsync(assetPair);

            if (response is ErrorResponse error)
                throw new Exception(error.ErrorMessage);

            if (response is bool)
                return;

            throw new Exception(ApiError);
        }
    }
}
