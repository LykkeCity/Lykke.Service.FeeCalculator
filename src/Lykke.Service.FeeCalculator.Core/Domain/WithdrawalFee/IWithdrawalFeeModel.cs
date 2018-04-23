using Lykke.SettingsReader.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;

namespace Lykke.Service.FeeCalculator.Core.Domain.WithdrawalFee
{
    public interface IWithdrawalFeeModel
    {
        string AssetId { get; set; }

        double SizeForSelectedCountries { get; set; }
        double SizeForOtherCountries { get; set; }

        IReadOnlyCollection<string> Countries { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        PaymentSystemType PaymentSystemForSelectedCountries { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        PaymentSystemType PaymentSystemForOtherCountries { get; set; }
    }
}
