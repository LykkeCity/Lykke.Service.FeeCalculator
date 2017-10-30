using Lykke.Service.FeeCalculator.Core.Settings.ServiceSettings;
using Lykke.Service.FeeCalculator.Core.Settings.SlackNotifications;

namespace Lykke.Service.FeeCalculator.Core.Settings
{
    public class AppSettings
    {
        public FeeCalculatorSettings FeeCalculatorService { get; set; }
        public SlackNotificationsSettings SlackNotifications { get; set; }
    }
}
