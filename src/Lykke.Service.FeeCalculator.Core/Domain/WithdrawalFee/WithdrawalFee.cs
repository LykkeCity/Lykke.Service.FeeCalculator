using Lykke.SettingsReader.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;

namespace Lykke.Service.FeeCalculator.Core.Domain.WithdrawalFee
{
    public class WithdrawalFee : IWithdrawalFee
    {
        public string AssetId { get; set; }
        public double Size { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public PaymentSystemType PaymentSystem { get; set; }
        [Optional]
        public IReadOnlyCollection<string> Countries { get; set; }
    }
}
