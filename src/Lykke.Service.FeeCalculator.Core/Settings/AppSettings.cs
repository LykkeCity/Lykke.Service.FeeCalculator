using Lykke.Service.FeeCalculator.Core.Settings.ClientSettings;
using Lykke.Service.FeeCalculator.Core.Settings.ServiceSettings;
using Lykke.Service.FeeCalculator.Core.Settings.SlackNotifications;

namespace Lykke.Service.FeeCalculator.Core.Settings
{
    public class AppSettings
    {
        public FeeCalculatorSettings FeeCalculatorService { get; set; }
        public SlackNotificationsSettings SlackNotifications { get; set; }
        public AssetsServiceSettings AssetsServiceClient { get; set; }
        public TradeVolumesServiceSettings TradeVolumesServiceClient { get; set; }
    }
}
