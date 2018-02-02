using System;

namespace Lykke.Service.FeeCalculator.Core.Settings.ServiceSettings
{
    public class CacheSettings
    {
        public TimeSpan AssetsUpdateInterval { get; set; }
        public TimeSpan TradeVolumesUpdateInterval { get; set; }
        public TimeSpan FeesUpdateInterval { get; set; }
    }
}
