using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.FeeCalculator.Core.Domain.CashoutFee;

namespace Lykke.Service.FeeCalculator.Core.Services
{
    public interface ICashoutFeesService
    {
        Task<IReadOnlyCollection<ICashoutFee>> GetAllAsync();
        Task<ICashoutFee> GetAsync(string assetId);
        Task AddAsync(ICashoutFee fee);
        Task DeleteAsync(string id);
        //TODO: remove in next release, used for init settings in db from settings
        Task InitAsync();
    }
}
