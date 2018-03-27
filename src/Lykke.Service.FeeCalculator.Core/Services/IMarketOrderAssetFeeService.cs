using System.Threading.Tasks;
using Lykke.Service.FeeCalculator.Core.Domain.MarketOrderAssetFee;

namespace Lykke.Service.FeeCalculator.Core.Services
{
    public interface IMarketOrderAssetFeeService
    {
        Task<IMarketOrderAssetFee[]> GetAllAsync();
        Task<IMarketOrderAssetFee> GetAsync(string assetId);
        Task AddAsync(IMarketOrderAssetFee fee);
        Task DeleteAsync(string id);
        //TODO: remove in next release, used for init settings in db from settings
        Task InitAsync();
    }
}
