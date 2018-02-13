// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Lykke.Service.FeeCalculator.AutorestClient.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class MarketOrderFee
    {
        /// <summary>
        /// Initializes a new instance of the MarketOrderFee class.
        /// </summary>
        public MarketOrderFee()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the MarketOrderFee class.
        /// </summary>
        /// <param name="type">Possible values include: 'Unknown', 'Absolute',
        /// 'Relative'</param>
        public MarketOrderFee(decimal amount, FeeType type, string assetId = default(string), string targetAssetId = default(string), string targetWalletId = default(string))
        {
            Amount = amount;
            Type = type;
            AssetId = assetId;
            TargetAssetId = targetAssetId;
            TargetWalletId = targetWalletId;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Amount")]
        public decimal Amount { get; set; }

        /// <summary>
        /// Gets or sets possible values include: 'Unknown', 'Absolute',
        /// 'Relative'
        /// </summary>
        [JsonProperty(PropertyName = "Type")]
        public FeeType Type { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "AssetId")]
        public string AssetId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "TargetAssetId")]
        public string TargetAssetId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "TargetWalletId")]
        public string TargetWalletId { get; set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="Microsoft.Rest.ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public virtual void Validate()
        {
        }
    }
}
