using Lykke.SettingsReader.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;

namespace Lykke.Service.FeeCalculator.Core.Domain.WithdrawalFee
{
    public interface IWithdrawalFee
    {
        string AssetId { get; set; }
        double Size { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        PaymentSystemType PaymentSystem { get; set; }
        [Optional]
        IReadOnlyCollection<string> Countries { get; set; }
    }
}
