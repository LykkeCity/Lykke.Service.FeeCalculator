using System.Threading.Tasks;
using Lykke.Service.FeeCalculator.Core.Domain.Fees;

namespace Lykke.Service.FeeCalculator.Core.Services
{
    public interface IFeeService
    {
        Task<IFee[]> GetAllAsync();
        Task AddAsync(IFee fee);
        Task DeleteAsync(string id);
    }
}
