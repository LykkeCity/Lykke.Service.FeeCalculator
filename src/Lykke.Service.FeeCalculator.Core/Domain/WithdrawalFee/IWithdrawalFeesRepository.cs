using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.Service.FeeCalculator.Core.Domain.WithdrawalFee
{
    public interface IWithdrawalFeesRepository
    {
        Task<IEnumerable<IWithdrawalFee>> GetAllAsync();
        Task<IWithdrawalFee> SaveAsync(IWithdrawalFee fee);
        Task DeleteAsync(string assetId);
    }
}
