using System;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Common.Log;
using Lykke.Service.FeeCalculator.Core.Domain;
using Lykke.Service.FeeCalculator.Core.Domain.Fees;
using Lykke.Service.FeeCalculator.Core.Services;
using Lykke.Service.TradeVolumes.Client;
using Lykke.Service.TradeVolumes.Client.Models;

namespace Lykke.Service.FeeCalculator.Services
{
    public class FeeService : IFeeService
    {
        private readonly ITradeVolumesCacheService _tradeVolumesCacheService;
        private readonly ITradeVolumesClient _tradeVolumesClient;
        private readonly CachedDataDictionary<decimal, IFee> _feesCache;
        private readonly CachedDataDictionary<string, IStaticFee> _feesStaticCache;
        private readonly ILog _log;

        public FeeService(
            ITradeVolumesCacheService tradeVolumesCacheService,
            ITradeVolumesClient tradeVolumesClient,
            CachedDataDictionary<decimal, IFee> feesCache,
            CachedDataDictionary<string, IStaticFee> feesStaticCache,
            ILog log
            )
        {
            _tradeVolumesCacheService = tradeVolumesCacheService;
            _tradeVolumesClient = tradeVolumesClient;
            _feesCache = feesCache;
            _feesStaticCache = feesStaticCache;
            _log = log;
        }
        
        public async Task<IBaseFee> GetFeeAsync(string clientId, string assetPairId, string assetId)
        {
            var staticFees = await _feesStaticCache.Values();
            var fee = staticFees.FirstOrDefault(item => item.AssetPair == assetPairId);
            
            if (fee != null)
                return fee;
            
            AssetPairTradeVolumeResponse clientVolume = null;
            
            try
            {
                clientVolume = await _tradeVolumesClient.GetClientAssetPairTradeVolumeAsync(assetPairId, clientId,
                    DateTime.UtcNow.AddDays(-30).Date, DateTime.UtcNow.Date);
            }
            catch (Exception ex)
            {
                _log.WriteWarning(nameof(GetFeeAsync), new {ClientId = clientId, AssetPairId = assetPairId}, "can't get client volume", ex);
            }
             
            AssetPairTradeVolume assetPairVolume = _tradeVolumesCacheService.GetTradeVolume(assetPairId);

            double percentage = 0;
            
            if (clientVolume != null && assetPairVolume != null)
            {
                percentage = (assetPairVolume.BaseAssetId == assetId
                    ? clientVolume.BaseVolume / assetPairVolume.BaseVolume
                    : clientVolume.QuotingVolume / assetPairVolume.QuotingVolume) * 100;
            }

            return await GetFeeByPercentageAsync(percentage);
        }

        public async Task<IBaseFee> GetFeeByPercentageAsync(double percentage)
        {
            var value = Convert.ToDecimal(percentage);
            var fees = (await _feesCache.Values()).OrderByDescending(item => item.Volume).ToList();

            foreach (var fee in fees)
            {
                if (value > fee.Volume)
                    return fee;
            }

            return fees.Last();
        }
    }
}
