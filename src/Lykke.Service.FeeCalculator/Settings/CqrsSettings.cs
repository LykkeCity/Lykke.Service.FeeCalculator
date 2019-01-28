using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.FeeCalculator.Settings
{
    public class CqrsSettings
    {
        [AmqpCheck]
        public string RabbitConnectionString { get; set; }
    }
}
