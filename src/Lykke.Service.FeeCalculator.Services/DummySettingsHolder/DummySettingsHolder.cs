using Lykke.Service.FeeCalculator.Core.Settings.ServiceSettings;
using System.Collections.Generic;

namespace Lykke.Service.FeeCalculator.Services.DummySettingsHolder
{
    public class DummySettingsHolder : IDummySettingsHolder
    {
        private readonly MarketOrderSettings _marketOrderSettings;
        private readonly LimitOrderSettings _limitOrderSettings;
        private readonly List<CashoutFee> _cashoutFees;

        public DummySettingsHolder(MarketOrderSettings marketOrderSettings, LimitOrderSettings limitOrderSettings, List<CashoutFee> cashoutFees)
        {
            _marketOrderSettings = marketOrderSettings;
            _limitOrderSettings = limitOrderSettings;
            _cashoutFees = cashoutFees;
        }

        public LimitOrderSettings GetLimitOrderSettings()
        {
            return _limitOrderSettings;
        }

        public MarketOrderSettings GetMarkerOrderSettings()
        {
            return _marketOrderSettings;
        }

        public List<CashoutFee> GetCashoutFees()
        {
            return _cashoutFees;
        }
    }
}
