using Lykke.Service.FeeCalculator.Core.Settings;

namespace Lykke.Service.FeeCalculator.Services.DummySettingsHolder
{
    public class DummySettingsHolder : IDummySettingsHolder
    {
        private readonly BankCardSettings _bankCardSettings;

        public DummySettingsHolder(
            BankCardSettings bankCardSettings)
        {
            _bankCardSettings = bankCardSettings;
        }

        public BankCardSettings GetBankCardSettings()
        {
            return _bankCardSettings;
        }
    }
}
