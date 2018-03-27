using System.Threading.Tasks;
using Lykke.Service.FeeCalculator.Core.Domain.Fees;

namespace Lykke.Service.FeeCalculator.Core.Services
{
    public interface IStaticFeeService
    {
        Task<IStaticFee[]> GetAllAsync();
        Task<IStaticFee> GetAsync(string assetPair);
        Task AddAsync(IStaticFee fee);
        Task DeleteAsync(string id);
    }
}
