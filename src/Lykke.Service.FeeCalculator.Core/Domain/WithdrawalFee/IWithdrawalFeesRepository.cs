using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.Service.FeeCalculator.Core.Domain.WithdrawalFee
{
    public interface IWithdrawalFeesRepository
    {
        Task<IEnumerable<IWithdrawalFeeModel>> GetAllAsync();
        Task SaveAsync(WithdrawalFeeModel fee);
    }
}
