using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Common.Log;
using Lykke.Service.Assets.Client.Models;
using Lykke.Service.FeeCalculator.Core.Services;
using Lykke.Service.TradeVolumes.Client;
using Lykke.Service.TradeVolumes.Client.Models;

namespace Lykke.Service.FeeCalculator.Services.PeriodicalHandlers
{
    public class CacheUpdaterHandler : TimerPeriod
    {
        private readonly CachedDataDictionary<string, AssetPair> _assetPairCache;
        private readonly ITradeVolumesClient _tradeVolumesClient;
        private readonly ITradeVolumesCacheService _tradeVolumesCacheService;
        private readonly int _tradeVolumeToGetInDays;
        private bool _firstRun;

        public CacheUpdaterHandler(
            CachedDataDictionary<string, AssetPair> assetPairCache,
            ITradeVolumesClient tradeVolumesClient,
            ITradeVolumesCacheService tradeVolumesCacheService,
            TimeSpan updateInterval,
            int tradeVolumeToGetInDays,
            ILog log
            ) :
            base(nameof(CacheUpdaterHandler), (int)updateInterval.TotalMilliseconds, log)
        {
            _assetPairCache = assetPairCache;
            _tradeVolumesClient = tradeVolumesClient;
            _tradeVolumesCacheService = tradeVolumesCacheService;
            _tradeVolumeToGetInDays = tradeVolumeToGetInDays;
            _firstRun = true;
        }

        public override async Task Execute()
        {
            if (_firstRun)
            {
                _firstRun = false;
                return;
            }

            await FillCache();
        }

        public async Task FillCache()
        {
            Dictionary<string, AssetPair> assetPairs = (await _assetPairCache.Values()).ToDictionary(item => item.Id);

            List<AssetPairTradeVolumeResponse> tradeVolumes = await _tradeVolumesClient.GetAssetPairsTradeVolumeAsync(assetPairs.Keys.ToArray(), 
                DateTime.UtcNow.AddDays(-_tradeVolumeToGetInDays).Date, DateTime.UtcNow);

            foreach (var tradeVolume in tradeVolumes)
            {
                if (assetPairs.TryGetValue(tradeVolume.AssetPairId, out var assetPair))
                    _tradeVolumesCacheService.AddTadeVolume(assetPair, tradeVolume.BaseVolume, tradeVolume.QuotingVolume);
            }
            
        }
    }
}
