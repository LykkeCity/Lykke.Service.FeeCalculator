using System;
using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.FeeCalculator.Client
{
    public class FeeCalculatorServiceClientSettings
    {
        [HttpCheck("/api/isalive")]
        public string ServiceUrl { get; set; }

        [Optional]
        public TimeSpan CacheExpirationPeriod { get; set; } = TimeSpan.FromMinutes(1);
    }
}
