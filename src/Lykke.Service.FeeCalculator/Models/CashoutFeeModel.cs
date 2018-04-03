using System.ComponentModel.DataAnnotations;
using Lykke.Service.FeeCalculator.Core.Domain.Fees;

namespace Lykke.Service.FeeCalculator.Models
{
    public class CashoutFeeModel
    {
        public string Id { get; set; }
        [Required]
        public string AssetId { get; set; }
        public double Size { get; set; }
        [Required]
        public FeeType Type { get; set; }
    }
}
