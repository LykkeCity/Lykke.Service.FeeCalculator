using System.Threading.Tasks;

namespace Lykke.Service.FeeCalculator.Core.Services
{
    public interface IShutdownManager
    {
        Task StopAsync();
    }
}