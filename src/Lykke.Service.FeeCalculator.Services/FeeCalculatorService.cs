using System;
using System.Linq;
using System.Threading.Tasks;
using Common.Log;
using JetBrains.Annotations;
using Lykke.Service.FeeCalculator.Core.Domain;
using Lykke.Service.FeeCalculator.Core.Domain.Fees;
using Lykke.Service.FeeCalculator.Core.Domain.MarketOrderAssetFee;
using Lykke.Service.FeeCalculator.Core.Services;
using Lykke.Service.TradeVolumes.Client;
using Lykke.Service.TradeVolumes.Client.Models;

namespace Lykke.Service.FeeCalculator.Services
{
    public class FeeCalculatorService : IFeeCalculatorService
    {
        private readonly ITradeVolumesCacheService _tradeVolumesCacheService;
        private readonly ITradeVolumesClient _tradeVolumesClient;
        private readonly IFeeService _feeService;
        private readonly IStaticFeeService _staticFeeService;
        private readonly IClientIdCacheService _clientIdCacheService;
        private readonly int _tradeVolumeToGetInDays;
        private readonly IMarketOrderAssetFeeService _marketOrderAssetFeeService;
        private readonly ILog _log;

        [UsedImplicitly]
        public FeeCalculatorService(
            ITradeVolumesCacheService tradeVolumesCacheService,
            ITradeVolumesClient tradeVolumesClient,
            IFeeService feeService,
            IStaticFeeService staticFeeService,
            IClientIdCacheService clientIdCacheService,
            int tradeVolumeToGetInDays,
            IMarketOrderAssetFeeService marketOrderAssetFees,
            ILog log
            )
        {
            _tradeVolumesCacheService = tradeVolumesCacheService;
            _tradeVolumesClient = tradeVolumesClient;
            _feeService = feeService;
            _staticFeeService = staticFeeService;
            _clientIdCacheService = clientIdCacheService;
            _tradeVolumeToGetInDays = tradeVolumeToGetInDays;
            _marketOrderAssetFeeService = marketOrderAssetFees;
            _log = log;
        }

        public async Task<IMarketOrderAssetFee> GetMarketOrderFeeAsync(string clientId, string assetPairId, string assetId)
        {
            var marketOrderFee = await _marketOrderAssetFeeService.GetAsync(assetId);

            if (marketOrderFee != null)
                return marketOrderFee;

            var fee = await GetFeeAsync(clientId, assetPairId, assetId);
            
            return new MarketOrderAssetFee
            {
                Amount = fee.TakerFee,
                AssetId = assetId,
                Type = fee.TakerFeeType,
                TargetAssetId = null
            };
        }

        public async Task<IBaseFee> GetFeeAsync(string clientId, string assetPairId, string assetId)
        {
            var fee = await _staticFeeService.GetAsync(assetPairId);
            
            if (fee != null)
                return fee;

            var id = await _clientIdCacheService.GetClientId(clientId);
            
            AssetPairTradeVolumeResponse clientVolume = null;
            
            try
            {
                clientVolume = await _tradeVolumesClient.GetClientAssetPairTradeVolumeAsync(assetPairId, id,
                    DateTime.UtcNow.AddDays(-_tradeVolumeToGetInDays).Date, DateTime.UtcNow.Date);
            }
            catch (Exception ex)
            {
                _log.WriteWarning(nameof(GetFeeAsync), new {ClientId = id, AssetPairId = assetPairId}, "can't get client volume", ex);
            }
             
            AssetPairTradeVolume assetPairVolume = _tradeVolumesCacheService.GetTradeVolume(assetPairId);

            double percentage = 0;
            
            if (clientVolume != null && assetPairVolume != null && assetPairVolume.BaseVolume > 0 && assetPairVolume.QuotingVolume > 0)
            {
                percentage = (assetPairVolume.BaseAssetId == assetId
                    ? clientVolume.BaseVolume / assetPairVolume.BaseVolume
                    : clientVolume.QuotingVolume / assetPairVolume.QuotingVolume) * 100;
            }

            var result = await GetFeeByPercentageAsync(percentage);

            return result ?? new BaseFee();
        }

        public async Task<IBaseFee> GetFeeByPercentageAsync(double percentage)
        {
            var value = Convert.ToDecimal(percentage);
            var fees = await _feeService.GetAllAsync();

            foreach (var fee in fees)
            {
                if (value > fee.Volume)
                    return fee;
            }

            return fees.LastOrDefault();
        }
    }
}
