﻿using Lykke.Service.FeeCalculator.AutorestClient.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Lykke.Service.FeeCalculator.Client.Models
{
    public class MarketOrderFeeModel
    {
        public decimal Amount { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public FeeType Type { get; set; }
        public string AssetId { get; set; }
        public string TargetAssetId { get; set; }
        public string TargetWalletId { get; set; }
    }
}
