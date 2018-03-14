using Lykke.Service.FeeCalculator.Core.Settings.ServiceSettings;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Lykke.Service.FeeCalculator.Models
{
    public class WithdrawalFeeModel
    {
        public string AssetId { get; set; }
        public double Size { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public PaymentSystemType PaymentSystem { get; set; }
    }
}
