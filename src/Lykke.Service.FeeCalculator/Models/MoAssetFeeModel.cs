using System.ComponentModel.DataAnnotations;
using Lykke.Service.FeeCalculator.Core.Domain.Fees;

namespace Lykke.Service.FeeCalculator.Models
{
    public class MoAssetFeeModel
    {
        public string Id { get; set; }
        public decimal Amount { get; set; }
        public FeeType Type { get; set; }
        [Required]
        public string AssetId { get; set; }
        [Required]
        public string TargetAssetId { get; set; }
        [Required]
        public string TargetWalletId { get; set; }
    }
}
