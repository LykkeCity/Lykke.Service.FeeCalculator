using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.Service.FeeCalculator.Core.Domain.Fees
{
    public interface IStaticFeeRepository
    {
        Task<IStaticFee> AddFeeAsync(IStaticFee fee);
        Task<IEnumerable<IStaticFee>> GetFeesAsync();
        Task DeleteFeeAsync(string id);
    }
}
