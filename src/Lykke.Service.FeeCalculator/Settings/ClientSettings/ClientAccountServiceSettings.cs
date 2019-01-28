using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.FeeCalculator.Settings.ClientSettings
{
    public class ClientAccountServiceSettings
    {
        [HttpCheck("/api/isalive")]
        public string ServiceUrl { get; set; }
    }
}
