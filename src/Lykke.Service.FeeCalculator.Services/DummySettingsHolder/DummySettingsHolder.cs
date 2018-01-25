using Lykke.Service.FeeCalculator.Core.Settings.ServiceSettings;
using System.Collections.Generic;
using Lykke.Service.FeeCalculator.Core.Settings;

namespace Lykke.Service.FeeCalculator.Services.DummySettingsHolder
{
    public class DummySettingsHolder : IDummySettingsHolder
    {
        private readonly MarketOrderSettings _marketOrderSettings;
        private readonly LimitOrderSettings _limitOrderSettings;
        private readonly List<CashoutFee> _cashoutFees;
        private readonly BankCardSettings _bankCardSettings;

        public DummySettingsHolder(
            MarketOrderSettings marketOrderSettings, 
            LimitOrderSettings limitOrderSettings, 
            List<CashoutFee> cashoutFees, 
            BankCardSettings bankCardSettings)
        {
            _marketOrderSettings = marketOrderSettings;
            _limitOrderSettings = limitOrderSettings;
            _cashoutFees = cashoutFees;
            _bankCardSettings = bankCardSettings;
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

        public BankCardSettings GetBankCardSettings()
        {
            return _bankCardSettings;
        }
    }
}
