using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.FeeCalculator.Core.Domain.Fees;

namespace Lykke.Service.FeeCalculator.Core.Services
{
    public interface IStaticFeeService
    {
        Task<IReadOnlyCollection<IStaticFee>> GetAllAsync();
        Task<IStaticFee> GetAsync(string assetPair);
        Task AddAsync(IStaticFee fee);
        Task DeleteAsync(string id);
        //TODO: remove in next release, used for init settings in db from old db settings
        Task InitAsync();
    }
}
