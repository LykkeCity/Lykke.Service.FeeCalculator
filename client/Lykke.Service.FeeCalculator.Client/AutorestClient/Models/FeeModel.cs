// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Lykke.Service.FeeCalculator.AutorestClient.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class FeeModel
    {
        /// <summary>
        /// Initializes a new instance of the FeeModel class.
        /// </summary>
        public FeeModel()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the FeeModel class.
        /// </summary>
        public FeeModel(decimal volume, decimal makerFee, decimal takerFee, string id = default(string))
        {
            Id = id;
            Volume = volume;
            MakerFee = makerFee;
            TakerFee = takerFee;
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
        [JsonProperty(PropertyName = "Volume")]
        public decimal Volume { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "MakerFee")]
        public decimal MakerFee { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "TakerFee")]
        public decimal TakerFee { get; set; }

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