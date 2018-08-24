using System.Threading.Tasks;

namespace Lykke.Service.FeeCalculator.Core.Domain.FeeLog
{
    public interface IFeeLogRepository
    {
        Task CreateAsync(IFeeLogEntry item);
    }
}
