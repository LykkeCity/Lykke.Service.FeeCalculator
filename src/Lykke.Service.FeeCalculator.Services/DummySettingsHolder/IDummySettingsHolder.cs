﻿using Lykke.Service.FeeCalculator.Core.Settings.ServiceSettings;

namespace Lykke.Service.FeeCalculator.Services.DummySettingsHolder
{
    public interface IDummySettingsHolder
    {
        MarketOrderSettings GetMarkerOrderSettings();
        LimitOrderSettings GetLimitOrderSettings();
    }
}
