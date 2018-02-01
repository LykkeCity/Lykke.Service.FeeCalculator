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
        private readonly ILog _log;

        public FeeService(
            ITradeVolumesCacheService tradeVolumesCacheService,
            ITradeVolumesClient tradeVolumesClient,
            CachedDataDictionary<decimal, IFee> feesCache,
            ILog log
            )
        {
            _tradeVolumesCacheService = tradeVolumesCacheService;
            _tradeVolumesClient = tradeVolumesClient;
            _feesCache = feesCache;
            _log = log;
        }
        
        public async Task<IFee> GetFeeAsync(string clientId, string assetPairId, string assetId)
        {
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

        public async Task<IFee> GetFeeByPercentageAsync(double percentage)
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
