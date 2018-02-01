using Lykke.Service.Assets.Client.Models;
using Lykke.Service.FeeCalculator.Core.Domain;

namespace Lykke.Service.FeeCalculator.Core.Services
{
    public interface ITradeVolumesCacheService
    {
        void AddTadeVolume(AssetPair assetPair, double baseVolume, double quotingVolume);
        AssetPairTradeVolume GetTradeVolume(string assetPair);
    }
}
