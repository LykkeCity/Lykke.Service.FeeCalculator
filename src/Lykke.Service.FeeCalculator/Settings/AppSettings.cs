using Lykke.Sdk.Settings;
using Lykke.Service.FeeCalculator.Settings.ClientSettings;
using Lykke.Service.FeeCalculator.Settings.ServiceSettings;

namespace Lykke.Service.FeeCalculator.Settings
{
    public class AppSettings : BaseAppSettings
    {
        public FeeCalculatorSettings FeeCalculatorService { get; set; }
        public AssetsServiceSettings AssetsServiceClient { get; set; }
        public TradeVolumesServiceSettings TradeVolumesServiceClient { get; set; }
        public ClientAccountServiceSettings ClientAccountClient { get; set; }
    }
}
