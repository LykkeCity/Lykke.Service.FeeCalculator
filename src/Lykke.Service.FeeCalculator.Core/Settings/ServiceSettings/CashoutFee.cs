using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lykke.Service.FeeCalculator.Core.Settings.ServiceSettings
{
    public class CashoutFee
    {
        public string AssetId { get; set; }
        public double Size { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public FeeType Type { get; set; }

        public override string ToString() => $"{AssetId}: {Size} ({Type})";
    }
}
