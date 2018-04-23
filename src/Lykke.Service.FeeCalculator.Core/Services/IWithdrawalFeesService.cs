using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.FeeCalculator.Core.Domain.CashoutFee;
using Lykke.Service.FeeCalculator.Core.Domain.WithdrawalFee;

namespace Lykke.Service.FeeCalculator.Core.Services
{
    public interface IWithdrawalFeesService
    {
        Task<IEnumerable<IWithdrawalFeeModel>> GetAllAsync();
        Task<WithdrawalFeeModel> GetAsync(string assetId);
        Task SaveAsync(WithdrawalFeeModel fee);
    }
}
