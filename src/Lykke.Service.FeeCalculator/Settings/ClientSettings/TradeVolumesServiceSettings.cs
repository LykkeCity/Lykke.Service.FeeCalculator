using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.FeeCalculator.Settings.ClientSettings
{
    public class TradeVolumesServiceSettings
    {
        [HttpCheck("/api/isalive")]
        public string ServiceUrl { get; set; }
    }
}
