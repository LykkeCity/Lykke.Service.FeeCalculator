using Lykke.SettingsReader.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lykke.Service.FeeCalculator.Core.Settings.ServiceSettings
{
    public class WithdrawalFee
    {
        public string AssetId { get; set; }
        public double Size { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public PaymentSystemType PaymentSystem { get; set; }
        [Optional]
        public List<string> Countries { get; set; }
    }
}
