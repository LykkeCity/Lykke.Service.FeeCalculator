// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Lykke.Service.FeeCalculator.AutorestClient.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class LimitOrderFeeResponseModel
    {
        /// <summary>
        /// Initializes a new instance of the LimitOrderFeeResponseModel class.
        /// </summary>
        public LimitOrderFeeResponseModel()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the LimitOrderFeeResponseModel class.
        /// </summary>
        public LimitOrderFeeResponseModel(decimal makerFeeSize, decimal takerFeeSize)
        {
            MakerFeeSize = makerFeeSize;
            TakerFeeSize = takerFeeSize;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "MakerFeeSize")]
        public decimal MakerFeeSize { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "TakerFeeSize")]
        public decimal TakerFeeSize { get; set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="Microsoft.Rest.ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public virtual void Validate()
        {
            //Nothing to validate
        }
    }
}
