using Lykke.Sdk.Settings;
using Lykke.Service.FeeCalculator.Core.Settings.ClientSettings;
using Lykke.Service.FeeCalculator.Core.Settings.ServiceSettings;

namespace Lykke.Service.FeeCalculator.Core.Settings
{
    public class AppSettings : BaseAppSettings
    {
        public FeeCalculatorSettings FeeCalculatorService { get; set; }
        public AssetsServiceSettings AssetsServiceClient { get; set; }
        public TradeVolumesServiceSettings TradeVolumesServiceClient { get; set; }
        public ClientAccountServiceSettings ClientAccountClient { get; set; }
    }
}
