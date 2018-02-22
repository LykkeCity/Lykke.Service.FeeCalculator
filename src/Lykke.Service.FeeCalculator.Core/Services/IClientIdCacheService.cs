using System.Threading.Tasks;

namespace Lykke.Service.FeeCalculator.Core.Services
{
    public interface IClientIdCacheService
    {
        Task<string> GetClientId(string id);
    }
}
