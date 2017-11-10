using Lykke.Service.FeeCalculator.Core.Settings.ServiceSettings;

namespace Lykke.Service.FeeCalculator.Services.DummySettingsHolder
{
    public class DummySettingsHolder : IDummySettingsHolder
    {
        private readonly MarketOrderSettings _marketOrderSettings;
        private readonly LimitOrderSettings _limitOrderSettings;

        public DummySettingsHolder(MarketOrderSettings marketOrderSettings, LimitOrderSettings limitOrderSettings)
        {
            _marketOrderSettings = marketOrderSettings;
            _limitOrderSettings = limitOrderSettings;
        }

        public LimitOrderSettings GetLimitOrderSettings()
        {
            return _limitOrderSettings;
        }

        public MarketOrderSettings GetMarkerOrderSettings()
        {
            return _marketOrderSettings;
        }
    }
}
