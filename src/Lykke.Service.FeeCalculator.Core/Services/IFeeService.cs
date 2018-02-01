using System.Threading.Tasks;
using Lykke.Service.FeeCalculator.Core.Domain.Fees;

namespace Lykke.Service.FeeCalculator.Core.Services
{
    public interface IFeeService
    {
        Task<IFee> GetFeeAsync(string clientId, string assetPairId, string assetId);
        Task<IFee> GetFeeByPercentageAsync(double percentage);
    }
}
