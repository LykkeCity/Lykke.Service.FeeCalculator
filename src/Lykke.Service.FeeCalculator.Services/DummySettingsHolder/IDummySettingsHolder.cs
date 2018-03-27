using Lykke.Service.FeeCalculator.Core.Settings;

namespace Lykke.Service.FeeCalculator.Services.DummySettingsHolder
{
    public interface IDummySettingsHolder
    {
        BankCardSettings GetBankCardSettings();
    }
}
