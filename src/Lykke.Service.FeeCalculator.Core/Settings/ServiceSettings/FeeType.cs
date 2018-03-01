using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Lykke.Service.FeeCalculator.Core.Settings.ServiceSettings
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum FeeType
    {
        Unknown = 0,
        Absolute = 10,
        Relative = 20
    }
}
