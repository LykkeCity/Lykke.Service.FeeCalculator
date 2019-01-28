using System;

namespace Lykke.Service.FeeCalculator.Settings.ServiceSettings
{
    public class CacheSettings
    {
        public TimeSpan AssetsUpdateInterval { get; set; }
        public TimeSpan TradeVolumesUpdateInterval { get; set; }
        public string RedisConfiguration { get; set; }
        public string InstanceName { get; set; }
    }
}
