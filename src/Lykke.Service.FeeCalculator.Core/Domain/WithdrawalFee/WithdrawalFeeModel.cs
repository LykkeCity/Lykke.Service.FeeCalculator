using Lykke.SettingsReader.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;

namespace Lykke.Service.FeeCalculator.Core.Domain.WithdrawalFee
{
    public class WithdrawalFeeModel
    {
        public string AssetId { get; set; }

        public double SizeForSelectedCountries { get; set; }
        public double SizeForOtherCountries { get; set; }

        public IReadOnlyCollection<string> Countries { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public PaymentSystemType PaymentSystemForSelectedCountries { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public PaymentSystemType PaymentSystemForOtherCountries { get; set; }
    }
}
