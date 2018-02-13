using Lykke.Service.FeeCalculator.Core.Settings.ServiceSettings;
using System.Collections.Generic;
using Lykke.Service.FeeCalculator.Core.Settings;

namespace Lykke.Service.FeeCalculator.Services.DummySettingsHolder
{
    public interface IDummySettingsHolder
    {
        List<CashoutFee> GetCashoutFees();
        BankCardSettings GetBankCardSettings();
    }
}
