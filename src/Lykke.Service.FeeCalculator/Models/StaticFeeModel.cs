using System.ComponentModel.DataAnnotations;
using Lykke.Service.FeeCalculator.Core.Domain.Fees;

namespace Lykke.Service.FeeCalculator.Models
{
    public class StaticFeeModel
    {
        public string Id { get; set; }
        [Required]
        public string AssetPair { get; set; }
        public decimal MakerFee { get; set; }
        public decimal TakerFee { get; set; }
        public FeeType MakerFeeType { get; set; }
        public FeeType TakerFeeType { get; set; }
        public decimal MakerFeeModificator { get; set; }
    }
}
