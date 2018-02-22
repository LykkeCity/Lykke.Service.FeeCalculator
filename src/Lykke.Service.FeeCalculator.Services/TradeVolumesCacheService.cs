using System.Collections.Concurrent;
using Lykke.Service.Assets.Client.Models;
using Lykke.Service.FeeCalculator.Core.Domain;
using Lykke.Service.FeeCalculator.Core.Services;

namespace Lykke.Service.FeeCalculator.Services
{
    public class TradeVolumesCacheService : ITradeVolumesCacheService
    {
        private readonly ConcurrentDictionary<string, AssetPairTradeVolume> _cache = new ConcurrentDictionary<string, AssetPairTradeVolume>();

        public void AddTadeVolume(AssetPair assetPair, double baseVolume, double quotingVolume)
        {
            _cache[assetPair.Id] = new AssetPairTradeVolume
            {
                BaseAssetId = assetPair.BaseAssetId,
                QuoteAssetId = assetPair.QuotingAssetId,
                BaseVolume = baseVolume,
                QuotingVolume = quotingVolume
            };
        }

        public AssetPairTradeVolume GetTradeVolume(string assetPair)
        {
                return !string.IsNullOrEmpty(assetPair) && _cache.ContainsKey(assetPair)
                ? _cache[assetPair]
                : null;
        }
    }
}
