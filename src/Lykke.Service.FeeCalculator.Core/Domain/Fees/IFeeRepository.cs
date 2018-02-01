using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.Service.FeeCalculator.Core.Domain.Fees
{
    public interface IFeeRepository
    {
        Task AddFeeAsync(IFee fee);
        Task<IEnumerable<IFee>> GetFeesAsync();
    }
}
