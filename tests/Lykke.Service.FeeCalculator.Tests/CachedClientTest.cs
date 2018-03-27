using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.FeeCalculator.AutorestClient.Models;
using Lykke.Service.FeeCalculator.Client;
using Lykke.Service.FeeCalculator.Client.Models;
using Microsoft.Extensions.Caching.Memory;
using NSubstitute;
using Xunit;
using MarketOrderAssetFeeModel = Lykke.Service.FeeCalculator.Client.Models.MarketOrderAssetFeeModel;

namespace Lykke.Service.FeeCalculator.Tests
{
    public class CachedClientTest : IDisposable
    {
        private readonly IFeeCalculatorClient _client;
        private readonly FeeCalculatorClientCached _cached;

        public CachedClientTest()
        {
            _client = Substitute.For<IFeeCalculatorClient>();
            _cached = new FeeCalculatorClientCached(_client, TimeSpan.FromSeconds(1), new MemoryCache(new MemoryCacheOptions { ExpirationScanFrequency = TimeSpan.FromSeconds(0.1) }));
        }


        [Fact]
        public async Task ShouldCacheMarketOrderFee()
        {
            await SetupMarketOrdersFee();

            await _client.ReceivedWithAnyArgs(1).GetMarketOrderFees("", "", "", OrderAction.Buy);
        }


        [Fact]
        public async Task ShouldDistinguishOrderType()
        {
            await SetupMarketOrdersFee();

            await SetupMarketOrderAssetFee();

            await SetupLimitOrdersFee();
        }

        [Fact]
        public async Task ShouldRefreshCacheMarketOrderFee()
        {
            await SetupMarketOrdersFee();

            await Task.Delay(TimeSpan.FromSeconds(1.5));

            await SetupMarketOrdersFee();

            await _client.ReceivedWithAnyArgs(2).GetMarketOrderFees("", "", "", OrderAction.Buy);
        }


        [Fact]
        public async Task ShouldCacheMarketOrderAssetFee()
        {
            await SetupMarketOrderAssetFee();

            await _client.ReceivedWithAnyArgs(1).GetMarketOrderAssetFee("", "", "", OrderAction.Buy);
        }

        [Fact]
        public async Task ShouldRefreshCacheMarketOrderAssetFee()
        {
            await SetupMarketOrderAssetFee();

            await Task.Delay(TimeSpan.FromSeconds(1.5));

            await SetupMarketOrderAssetFee();

            await _client.ReceivedWithAnyArgs(2).GetMarketOrderAssetFee("", "", "", OrderAction.Buy);
        }

        [Fact]
        public async Task ShouldCacheLimitOrderFee()
        {
            await SetupLimitOrdersFee();

            await _client.ReceivedWithAnyArgs(1).GetLimitOrderFees("", "", "", OrderAction.Buy);
        }

        [Fact]
        public async Task ShouldRefreshCacheLimitOrderFee()
        {
            await SetupLimitOrdersFee();

            await Task.Delay(TimeSpan.FromSeconds(1.5));

            await SetupLimitOrdersFee();

            await _client.ReceivedWithAnyArgs(2).GetLimitOrderFees("", "", "", OrderAction.Buy);
        }


        [Fact]
        public async Task ShouldCacheCashOut()
        {
            await SetupCachOut();

            await _client.ReceivedWithAnyArgs(1).GetCashoutFeesAsync();
        }

        [Fact]
        public async Task ShouldRefreshCacheCashOut()
        {
            await SetupCachOut();

            await Task.Delay(TimeSpan.FromSeconds(1.5));

            await SetupCachOut();

            await _client.ReceivedWithAnyArgs(2).GetCashoutFeesAsync();
        }


        private async Task SetupMarketOrdersFee()
        {
            var fee = new MarketOrderFeeModel();
            _client.GetMarketOrderFees("", "", "", OrderAction.Buy).ReturnsForAnyArgs(info => Task.FromResult(fee));
            for (var i = 0; i < 10; i++)
            {
                var cached = await _cached.GetMarketOrderFees("", "", "", OrderAction.Buy);
                Assert.Equal(fee, cached);
            }
        }

        private async Task SetupMarketOrderAssetFee()
        {
            var fee = new MarketOrderAssetFeeModel();
            _client.GetMarketOrderAssetFee("", "", "", OrderAction.Buy).ReturnsForAnyArgs(info => Task.FromResult(fee));
            for (var i = 0; i < 10; i++)
            {
                var cached = await _cached.GetMarketOrderAssetFee("", "", "", OrderAction.Buy);
                Assert.Equal(fee, cached);
            }
        }



        private async Task SetupLimitOrdersFee()
        {
            var fee = new LimitOrderFeeModel();
            _client.GetLimitOrderFees("", "", "", OrderAction.Buy).ReturnsForAnyArgs(info => Task.FromResult(fee));
            for (var i = 0; i < 10; i++)
            {
                var cached = await _cached.GetLimitOrderFees("", "", "", OrderAction.Buy);
                Assert.Equal(fee, cached);
            }
        }


        private async Task SetupCachOut()
        {
            IReadOnlyCollection<CashoutFee> fee = new List<CashoutFee> { new CashoutFee() };
            _client.GetCashoutFeesAsync().ReturnsForAnyArgs(info => Task.FromResult(fee));
            for (var i = 0; i < 10; i++)
            {
                var cached = await _cached.GetCashoutFeesAsync();
                Assert.Equal(fee, cached);
            }
        }

        public void Dispose()
        {
            _client?.Dispose();
            _cached?.Dispose();
        }
    }

}
