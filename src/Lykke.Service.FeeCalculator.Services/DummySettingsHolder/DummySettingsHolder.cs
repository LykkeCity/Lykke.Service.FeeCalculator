using Lykke.Service.FeeCalculator.Core.Settings.ServiceSettings;
using System.Collections.Generic;
using Lykke.Service.FeeCalculator.Core.Settings;

namespace Lykke.Service.FeeCalculator.Services.DummySettingsHolder
{
    public class DummySettingsHolder : IDummySettingsHolder
    {
        private readonly List<CashoutFee> _cashoutFees;
        private readonly BankCardSettings _bankCardSettings;

        public DummySettingsHolder(
            List<CashoutFee> cashoutFees, 
            BankCardSettings bankCardSettings)
        {
            _cashoutFees = cashoutFees;
            _bankCardSettings = bankCardSettings;
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
