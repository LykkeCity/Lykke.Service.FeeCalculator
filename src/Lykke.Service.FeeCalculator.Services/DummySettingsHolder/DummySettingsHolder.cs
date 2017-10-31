using Lykke.Service.FeeCalculator.Core.Settings.ServiceSettings;

namespace Lykke.Service.FeeCalculator.Services.DummySettingsHolder
{
    public class DummySettingsHolder : IDummySettingsHolder
    {
        private readonly TradeSettings _tradeSettings;

        public DummySettingsHolder(TradeSettings tradeSettings)
        {
            _tradeSettings = tradeSettings;
        }

        public TradeSettings GetTradeSettings()
        {
            return _tradeSettings;
        }
    }
}
