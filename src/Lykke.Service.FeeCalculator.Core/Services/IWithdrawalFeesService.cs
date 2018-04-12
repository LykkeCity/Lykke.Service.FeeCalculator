using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.FeeCalculator.Core.Domain.CashoutFee;
using Lykke.Service.FeeCalculator.Core.Domain.WithdrawalFee;

namespace Lykke.Service.FeeCalculator.Core.Services
{
    public interface IWithdrawalFeesService
    {
        Task<IReadOnlyCollection<WithdrawalFeeModel>> GetAllAsync();
        Task<IWithdrawalFee> GetAsync(string assetId);
        Task SaveAsync(List<WithdrawalFeeModel> fee);
    }
}
