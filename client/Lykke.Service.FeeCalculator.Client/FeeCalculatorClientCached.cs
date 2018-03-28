using System;
using System.Threading.Tasks;
using Lykke.Service.FeeCalculator.Client.Models;
using Lykke.Service.FeeCalculator.AutorestClient.Models;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;
using MarketOrderAssetFeeModel = Lykke.Service.FeeCalculator.Client.Models.MarketOrderAssetFeeModel;

namespace Lykke.Service.FeeCalculator.Client
{
    public sealed class FeeCalculatorClientCached : IFeeCalculatorClient
    {
        private readonly IFeeCalculatorClient _client;
        private readonly IMemoryCache _cache;
        private readonly TimeSpan _expirationPeriod;

        public FeeCalculatorClientCached(IFeeCalculatorClient client, TimeSpan expirationPeriod, IMemoryCache cache)
        {
            _client = client;
            _cache = cache;
            _expirationPeriod = expirationPeriod;
        }

        public void Dispose()
        {
            _client.Dispose();
            _cache.Dispose();
        }

        public async Task<MarketOrderAssetFeeModel> GetMarketOrderAssetFee(string clientId, string assetPair, string assetId, OrderAction orderAction)
        {
            var key = KeyGenerator.GetKeyForMarketAssetOrder(clientId, assetPair, assetId, orderAction);
            
            if (_cache.TryGetValue<MarketOrderAssetFeeModel>(key, out var fee))
            {
                return fee;
            }

            var newFee = await _client.GetMarketOrderAssetFee(clientId, assetPair, assetId, orderAction);

            _cache.Set(key, newFee, _expirationPeriod);
            return newFee;
        }

        async Task<MarketOrderAssetFeeModel> IFeeCalculatorClient.GetMarketOrderAssetFee(string clientId, string assetPair, string assetId, OrderAction orderAction)
        {
            return await GetMarketOrderAssetFee(clientId, assetPair, assetId, orderAction);
        }

        public async Task<LimitOrderFeeModel> GetLimitOrderFees(string clientId, string assetPair, string assetId, OrderAction orderAction)
        {
            var key = KeyGenerator.GetKeyForLimitOrder(clientId, assetPair, assetId, orderAction);
            if (_cache.TryGetValue<LimitOrderFeeModel>(key, out var fee))
            {
                return fee;
            }

            var newFee = await _client.GetLimitOrderFees(clientId, assetPair, assetId, orderAction);

            _cache.Set(key, newFee, _expirationPeriod);
            return newFee;
        }

        public async Task<IReadOnlyCollection<CashoutFee>> GetCashoutFeesAsync(string assetId = null)
        {
            var key = KeyGenerator.GetKeyForCashOut(null);
            
            if (_cache.TryGetValue<IReadOnlyList<CashoutFee>>(key, out var cashOut))
            {
                return cashOut;
            }

            var fees = await _client.GetCashoutFeesAsync(assetId);
            
            _cache.Set(key, fees, _expirationPeriod);
            
            return fees;
        }

        public async Task<CashoutFee> GetCashoutFeeAsync(string assetId)
        {
            var key = KeyGenerator.GetKeyForCashOut(assetId);
            
            if (_cache.TryGetValue<CashoutFee>(key, out var cashOut))
            {
                return cashOut;
            }

            var fee = await _client.GetCashoutFeeAsync(assetId);
            
            _cache.Set(key, fee, _expirationPeriod);
            
            return fee;
        }

        public Task AddCashoutFeeAsync(CashoutFeeModel model)
        {
            return _client.AddCashoutFeeAsync(model);
        }

        public Task DeleteCashoutFeeAsync(string id)
        {
            return _client.DeleteCashoutFeeAsync(id);
        }

        public async Task<MarketOrderFeeModel> GetMarketOrderFees(string clientId, string assetPair, string assetId, OrderAction orderAction)
        {
            var key = KeyGenerator.GetKeyForMarketOrder(clientId, assetPair, assetId, orderAction);
            if (_cache.TryGetValue<MarketOrderFeeModel>(key, out var fee))
            {
                return fee;
            }

            var newFee = await _client.GetMarketOrderFees(clientId, assetPair, assetId, orderAction);

            _cache.Set(key, newFee, _expirationPeriod);
            return newFee;
        }

        public Task<BankCardsFeeModel> GetBankCardFees()
        {
            return _client.GetBankCardFees();
        }

        public Task AddFeeAsync(FeeModel fee)
        {
            return _client.AddFeeAsync(fee);
        }

        public Task<IReadOnlyCollection<Fee>> GetFeesAsync()
        {
            return _client.GetFeesAsync();
        }

        public Task DeleteFeeAsync(string id)
        {
            return _client.DeleteFeeAsync(id);
        }

        public Task AddStaticFeeAsync(StaticFeeModel fee)
        {
            return _client.AddStaticFeeAsync(fee);
        }

        public Task<IReadOnlyCollection<StaticFee>> GetStaticFeesAsync()
        {
            return _client.GetStaticFeesAsync();
        }

        public Task DeleteStaticFeeAsync(string assetPair)
        {
            return _client.DeleteStaticFeeAsync(assetPair);
        }

        public Task<IReadOnlyCollection<MarketOrderAssetFee>> GetMarketOrderAssetFeesAsync()
        {
            return _client.GetMarketOrderAssetFeesAsync();
        }

        public Task AddMarketOrderAssetFeeAsync(MarketOrderAssetFeeModel model)
        {
            return _client.AddMarketOrderAssetFeeAsync(model);
        }

        public Task DeleteMarketOrderAssetFeeAsync(string id)
        {
            return _client.DeleteMarketOrderAssetFeeAsync(id);
        }

        public Task<WithdrawalFeeModel> GetWithdrawalFeeAsync(string assetId, string countryCode)
        {
            return _client.GetWithdrawalFeeAsync(assetId, countryCode);
        }

        private static class KeyGenerator
        {
            private const string GetAllCashOuts = "GET_ALL_CASHOUTS";
  
            public static object GetKeyForMarketOrder(string clientId, string assetPair, string assetId, OrderAction orderAction)
            {
                return "market" + clientId + assetPair + assetId + orderAction;
            }

            public static object GetKeyForMarketAssetOrder(string clientId, string assetPair, string assetId, OrderAction orderAction)
            {
                return "marketAsset" + clientId + assetPair + assetId + orderAction;
            }

            public static object GetKeyForLimitOrder(string clientId, string assetPair, string assetId, OrderAction orderAction)
            {
                return "limit" + clientId + assetPair + assetId + orderAction;
            }

            public static object GetKeyForCashOut(string assetId)
            {
                if (assetId == null)
                {
                    return GetAllCashOuts;
                }
                return assetId;
            }
        }
    }
}
