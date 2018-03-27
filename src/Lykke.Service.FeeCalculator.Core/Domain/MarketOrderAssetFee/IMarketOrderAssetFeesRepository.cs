using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.Service.FeeCalculator.Core.Domain.MarketOrderAssetFee
{
    public interface IMarketOrderAssetFeesRepository
    {
        Task<IEnumerable<IMarketOrderAssetFee>> GetAllAsync();
        Task<IMarketOrderAssetFee> AddAsync(IMarketOrderAssetFee assetFee);
        Task DeleteAsync(string id);
    }
}
