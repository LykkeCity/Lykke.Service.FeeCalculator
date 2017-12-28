using Lykke.Service.FeeCalculator.Core.Settings.ServiceSettings;
using System.Collections.Generic;

namespace Lykke.Service.FeeCalculator.Services.DummySettingsHolder
{
    public interface IDummySettingsHolder
    {
        MarketOrderSettings GetMarkerOrderSettings();
        LimitOrderSettings GetLimitOrderSettings();
        List<CashoutFee> GetCashoutFees();
    }
}
