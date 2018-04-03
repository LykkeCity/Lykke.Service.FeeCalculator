using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.Service.FeeCalculator.Core.Domain.CashoutFee
{
    public interface ICashoutFeesRepository
    {
        Task<IEnumerable<ICashoutFee>> GetAllAsync();
        Task<ICashoutFee> AddAsync(ICashoutFee fee);
        Task DeleteAsync(string assetId);
    }
}
