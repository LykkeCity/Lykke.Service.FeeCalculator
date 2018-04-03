using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.FeeCalculator.Core.Domain.Fees;

namespace Lykke.Service.FeeCalculator.Core.Services
{
    public interface IFeeService
    {
        Task<IReadOnlyCollection<IFee>> GetAllAsync();
        Task AddAsync(IFee fee);
        Task DeleteAsync(string id);
    }
}
