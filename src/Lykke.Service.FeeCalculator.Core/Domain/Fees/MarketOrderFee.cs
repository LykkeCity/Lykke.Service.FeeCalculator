using Lykke.Service.FeeCalculator.Core.Settings.ServiceSettings;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Lykke.Service.FeeCalculator.Core.Domain.Fees
{
    public class MarketOrderFee
    {
        public decimal Amount { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public FeeType Type { get; set; }
        public string AssetId { get; set; }
        public string TargetAssetId { get; set; }
        public string TargetWalletId { get; set; }
    }
}
