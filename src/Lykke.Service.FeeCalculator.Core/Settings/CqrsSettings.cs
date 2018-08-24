using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.FeeCalculator.Core.Settings
{
    public class CqrsSettings
    {
        [AmqpCheck]
        public string RabbitConnectionString { get; set; }
    }
}
