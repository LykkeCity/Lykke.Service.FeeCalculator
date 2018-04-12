using System;
using System.Threading.Tasks;
using Common.Log;
using Lykke.Service.FeeCalculator.Client.Models;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<MarketOrderAssetFeeModel> GetMarketOrderAssetFee(string clientId, string assetPair, string assetId, OrderAction orderAction)
        {
            var response = await _service.GetMarketOrderAssetFeeAsync(orderAction, clientId, assetPair, assetId);

            switch (response)
            {
                case ErrorResponse error:
                    throw new Exception(error.ErrorMessage);
                case MoAssetFee result:
                    return new MarketOrderAssetFeeModel
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

        public async Task<LimitOrderFeeModel> GetLimitOrderFees(string clientId, string assetPair, string assetId, OrderAction orderAction)
        {
            var response = await _service.GetLimitOrderFeeAsync(orderAction, clientId, assetPair, assetId);

            switch (response)
            {
                case ErrorResponse error:
                    throw new Exception(error.ErrorMessage);
                case LimitOrderFeeResponseModel result:
                    return new LimitOrderFeeModel
                    {
                        TakerFeeSize = result.TakerFeeSize,
                        MakerFeeSize = result.MakerFeeSize,
                        TakerFeeType = result.TakerFeeType,
                        MakerFeeType = result.MakerFeeType,
                        MakerFeeModificator = result.MakerFeeModificator
                    };
            }

            throw new Exception(ApiError);
        }

        public async Task<IReadOnlyCollection<CashoutFee>> GetCashoutFeesAsync(string assetId = null)
        {
            var response = await _service.GetCashoutFeesAsync(assetId);

            switch (response)
            {
                case ErrorResponse error:
                    throw new Exception(error.ErrorMessage);
                case List<CashoutFee> result:
                    return result;
            }

            throw new Exception(ApiError);
        }

        public async Task<CashoutFee> GetCashoutFeeAsync(string assetId)
        {
            var response = await _service.GetCashoutFeesAsync(assetId);

            switch (response)
            {
                case ErrorResponse error:
                    throw new Exception(error.ErrorMessage);
                case List<CashoutFee> result:
                    return result.FirstOrDefault(item => item.AssetId == assetId);
            }

            throw new Exception(ApiError);
        }

        public async Task AddCashoutFeeAsync(CashoutFeeModel model)
        {
            var response = await _service.AddCashoutFeeAsync(model);

            if (response != null)
                throw new Exception(response.ErrorMessage);
        }

        public async Task DeleteCashoutFeeAsync(string id)
        {
            var response = await _service.DeleteCashoutFeeAsync(id);

            if (response != null)
                throw new Exception(response.ErrorMessage);
        }

        public async Task<MarketOrderFeeModel> GetMarketOrderFees(string clientId, string assetPair, string assetId, OrderAction orderAction)
        {
            var response = await _service.GetMarketOrderFeeAsync(orderAction, clientId, assetPair, assetId);

            switch (response)
            {
                case ErrorResponse error:
                    throw new Exception(error.ErrorMessage);
                case MarketOrderFeeResponseModel result:
                    return new MarketOrderFeeModel
                    {
                        DefaultFeeSize = result.DefaultFeeSize
                    };
            }

            throw new Exception(ApiError);
        }

        public async Task<IReadOnlyCollection<WithdrawalFeeModel>> GetWithdrawalFeesAsync()
        {
            var response = await _service.GetWithdrawalFeesAsync();

            switch (response)
            {
                case ErrorResponse error:
                    throw new Exception(error.ErrorMessage);
                case List<WithdrawalFeeModel> result:
                    return result;
            }

            throw new Exception(ApiError);
        }

        public async Task SaveWithdrawalFeeAsync(List<WithdrawalFeeModel> model)
        {
            var response = await _service.SaveWithdrawalFeeAsync(model);

            if (response != null)
                throw new Exception(response.ErrorMessage);

            await Task.CompletedTask;
        }

        public async Task<WithdrawalFee> GetWithdrawalFeeAsync(string assetId, string countryCode)
        {
            var response = await _service.GetWithdrawalFeeAsync(assetId, countryCode);

            if (response is ErrorResponse error)
            {
                await _log.WriteErrorAsync(nameof(FeeCalculatorClient), nameof(GetWithdrawalFeeAsync),
                    $"assetId = {assetId}, error = {error.ErrorMessage}", null);

                throw new Exception(error.ErrorMessage);
            }

            if (response is WithdrawalFee result)
                return result;

            throw new Exception(ApiError);
        }

        public async Task<BankCardsFeeModel> GetBankCardFees()
        {
            var response = await _service.GetPercentageAsync();

            switch (response)
            {
                case ErrorResponse error:
                    await _log.WriteErrorAsync(nameof(FeeCalculatorClient), nameof(GetBankCardFees), error.ToJson(), null);

                    throw new Exception(error.ErrorMessage);
                case BankCardsFeeResponseModel result:
                    return new BankCardsFeeModel {Percentage = result.Percentage};
            }

            throw new Exception(ApiError);
        }

        public async Task AddFeeAsync(FeeModel fee)
        {
            var response = await _service.AddFeeAsync(fee);

            if (response != null)
                throw new Exception(response.ErrorMessage);
        }

        public async Task<IReadOnlyCollection<Fee>> GetFeesAsync()
        {
            var response = await _service.GetFeesAsync();

            switch (response)
            {
                case ErrorResponse error:
                    throw new Exception(error.ErrorMessage);
                case List<Fee> fees:
                    return fees;
            }

            throw new Exception(ApiError);
        }

        public async Task DeleteFeeAsync(string id)
        {
            var response = await _service.DeleteFeeAsync(id);

            if (response != null)
                throw new Exception(response.ErrorMessage);
        }

        public async Task AddStaticFeeAsync(StaticFeeModel fee)
        {
            var response = await _service.AddStaticFeeAsync(fee);

            if (response != null)
                throw new Exception(response.ErrorMessage);
        }

        public async Task<IReadOnlyCollection<StaticFee>> GetStaticFeesAsync()
        {
            var response = await _service.GetStaticFeesAsync();

            switch (response)
            {
                case ErrorResponse error:
                    throw new Exception(error.ErrorMessage);
                case List<StaticFee> fees:
                    return fees;
            }

            throw new Exception(ApiError);
        }

        public async Task DeleteStaticFeeAsync(string assetPair)
        {
            var response = await _service.DeleteStaticFeeAsync(assetPair);

            if (response != null)
                throw new Exception(response.ErrorMessage);
        }

        public async Task<IReadOnlyCollection<MoAssetFee>> GetMarketOrderAssetFeesAsync()
        {
            var response = await _service.GetMarketOrderAssetFeesAsync();

            switch (response)
            {
                case ErrorResponse error:
                    throw new Exception(error.ErrorMessage);
                case List<MoAssetFee> result:
                    return result;
            }

            throw new Exception(ApiError);
        }

        public async Task AddMarketOrderAssetFeeAsync(MoAssetFeeModel model)
        {
            var response = await _service.AddMarketOrderAssetFeeAsync(model);

            if (response != null)
                throw new Exception(response.ErrorMessage);
        }

        public async Task DeleteMarketOrderAssetFeeAsync(string id)
        {
            var response = await _service.DeleteMarketOrderAssetFeeAsync(id);

            if (response != null)
                throw new Exception(response.ErrorMessage);
        }
    }
}
