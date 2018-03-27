// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Lykke.Service.FeeCalculator.AutorestClient.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class StaticFee
    {
        /// <summary>
        /// Initializes a new instance of the StaticFee class.
        /// </summary>
        public StaticFee()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the StaticFee class.
        /// </summary>
        /// <param name="takerFeeType">Possible values include: 'Unknown',
        /// 'Absolute', 'Relative'</param>
        /// <param name="makerFeeType">Possible values include: 'Unknown',
        /// 'Absolute', 'Relative'</param>
        public StaticFee(decimal takerFee, decimal makerFee, FeeType takerFeeType, FeeType makerFeeType, decimal makerFeeModificator, string id = default(string), string assetPair = default(string))
        {
            Id = id;
            AssetPair = assetPair;
            TakerFee = takerFee;
            MakerFee = makerFee;
            TakerFeeType = takerFeeType;
            MakerFeeType = makerFeeType;
            MakerFeeModificator = makerFeeModificator;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Id")]
        public string Id { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "AssetPair")]
        public string AssetPair { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "TakerFee")]
        public decimal TakerFee { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "MakerFee")]
        public decimal MakerFee { get; set; }

        /// <summary>
        /// Gets or sets possible values include: 'Unknown', 'Absolute',
        /// 'Relative'
        /// </summary>
        [JsonProperty(PropertyName = "TakerFeeType")]
        public FeeType TakerFeeType { get; set; }

        /// <summary>
        /// Gets or sets possible values include: 'Unknown', 'Absolute',
        /// 'Relative'
        /// </summary>
        [JsonProperty(PropertyName = "MakerFeeType")]
        public FeeType MakerFeeType { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "MakerFeeModificator")]
        public decimal MakerFeeModificator { get; set; }

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
