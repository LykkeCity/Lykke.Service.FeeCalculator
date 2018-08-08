using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Common.Log;
using JetBrains.Annotations;
using Lykke.Common.Log;
using Lykke.Service.Assets.Client;
using Lykke.Service.Assets.Client.Models;
using Lykke.Service.FeeCalculator.Core.Services;
using Lykke.Service.TradeVolumes.Client;
using Lykke.Service.TradeVolumes.Client.Models;

namespace Lykke.Service.FeeCalculator.Services.PeriodicalHandlers
{
    public class CacheUpdaterHandler : TimerPeriod
    {
        private readonly IAssetsServiceWithCache _assetsServiceWithCache;
        private readonly ITradeVolumesClient _tradeVolumesClient;
        private readonly ITradeVolumesCacheService _tradeVolumesCacheService;
        private readonly int _tradeVolumeToGetInDays;
        private bool _firstRun;

        [UsedImplicitly]
        public CacheUpdaterHandler(
            IAssetsServiceWithCache assetsServiceWithCache,
            ITradeVolumesClient tradeVolumesClient,
            ITradeVolumesCacheService tradeVolumesCacheService,
            TimeSpan updateInterval,
            int tradeVolumeToGetInDays,
            ILogFactory logFactory
        ) :
            base(updateInterval, logFactory, nameof(CacheUpdaterHandler))
        {
            _assetsServiceWithCache = assetsServiceWithCache;
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
            try
            {
                Dictionary<string, AssetPair> assetPairs = (await _assetsServiceWithCache.GetAllAssetPairsAsync()).ToDictionary(item => item.Id);

                List<AssetPairTradeVolumeResponse> tradeVolumes = await _tradeVolumesClient.GetAssetPairsTradeVolumeAsync(assetPairs.Keys.ToArray(),
                    DateTime.UtcNow.AddDays(-_tradeVolumeToGetInDays).Date, DateTime.UtcNow);

                foreach (var tradeVolume in tradeVolumes)
                {
                    if (assetPairs.TryGetValue(tradeVolume.AssetPairId, out var assetPair))
                        _tradeVolumesCacheService.AddTadeVolume(assetPair, tradeVolume.BaseVolume, tradeVolume.QuotingVolume);
                }
            }
            catch (Exception ex)
            {
                Log.WriteWarning(nameof(FillCache), null, ex.Message);
            }
        }
    }
}
