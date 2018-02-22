﻿using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.FeeCalculator.Core.Settings.ClientSettings
{
    public class ClientAccountServiceSettings
    {
        [HttpCheck("/api/isalive")]
        public string ServiceUrl { get; set; }
    }
}
