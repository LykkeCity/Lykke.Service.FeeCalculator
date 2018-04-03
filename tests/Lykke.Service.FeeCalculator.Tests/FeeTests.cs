using System;
using System.Threading.Tasks;
using Autofac;
using Lykke.Service.Assets.Client.Models;
using Lykke.Service.FeeCalculator.Core.Domain;
using Lykke.Service.FeeCalculator.Core.Services;
using Lykke.Service.TradeVolumes.Client;
using Lykke.Service.TradeVolumes.Client.Models;
using Moq;
using Xunit;

namespace Lykke.Service.FeeCalculator.Tests
{
    public class FeeTests : BaseTest
    {
        private readonly ITradeVolumesCacheService _cache;
        private readonly Mock<ITradeVolumesClient> _client;
        private readonly IFeeCalculatorService _feeCalculatorService;

        public FeeTests()
        {
            _cache = Container.Resolve<ITradeVolumesCacheService>();
            _client = Container.Resolve<Mock<ITradeVolumesClient>>();
            _feeCalculatorService = Container.Resolve<IFeeCalculatorService>();
        }
        
        [Fact]
        public async Task FeeService_IsFeeByPercentageCorrect()
        {
            var fee = await _feeCalculatorService.GetFeeByPercentageAsync(0.01);
            
            Assert.Equal(0.19M, fee.TakerFee);
            Assert.Equal(0.1M, fee.MakerFee);
            
            fee = await _feeCalculatorService.GetFeeByPercentageAsync(5);
            
            Assert.Equal(0.17M, fee.TakerFee);
            Assert.Equal(0.08M, fee.MakerFee);
            
            fee = await _feeCalculatorService.GetFeeByPercentageAsync(32);
            
            Assert.Equal(0.14M, fee.TakerFee);
            Assert.Equal(0.05M, fee.MakerFee);
        }
        
        [Fact]
        public async Task FeeService_IsCorrectStaticFee()
        {
            var fee = await _feeCalculatorService.GetFeeAsync("1", "BTCCHF", "BTC");
            
            Assert.Equal(0.03M, fee.TakerFee);
            Assert.Equal(0.03M, fee.MakerFee);
        }
        
        [Fact]
        public async Task FeeService_IsCorrectFee()
        {
            const string assetPair = "BTCUSD";
            
            var assetPairVolume = new AssetPairTradeVolume
            {
                BaseAssetId = "BTC",
                QuoteAssetId = "USD",
                BaseVolume = 1,
                QuotingVolume = 10000
            };
            
            var clientVolume = new AssetPairTradeVolumeResponse
            {
                AssetPairId = assetPair,
                BaseVolume = 0.1,  //10% of total
                QuotingVolume = 200 //2% of total
            };
            
            _cache.AddTadeVolume(new AssetPair{Id = assetPair, BaseAssetId = assetPairVolume.BaseAssetId, QuotingAssetId = assetPairVolume.QuoteAssetId},
                assetPairVolume.BaseVolume, assetPairVolume.QuotingVolume);
            
            _client.Setup(c => c.GetClientAssetPairTradeVolumeAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .ReturnsAsync(clientVolume);
            
            var fee = await _feeCalculatorService.GetFeeAsync("1", assetPair, assetPairVolume.BaseAssetId);
            
            Assert.Equal(0.16M, fee.TakerFee);
            Assert.Equal(0.07M, fee.MakerFee);
            
            fee = await _feeCalculatorService.GetFeeAsync("1", assetPair, assetPairVolume.QuoteAssetId);
            
            Assert.Equal(0.18M, fee.TakerFee);
            Assert.Equal(0.09M, fee.MakerFee);
        }

        
    }
}
