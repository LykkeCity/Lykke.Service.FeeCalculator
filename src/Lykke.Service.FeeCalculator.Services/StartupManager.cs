using System.Threading.Tasks;
using Common.Log;
using Lykke.Service.FeeCalculator.Core.Services;
using Lykke.Service.FeeCalculator.Services.PeriodicalHandlers;

namespace Lykke.Service.FeeCalculator.Services
{
    // NOTE: Sometimes, startup process which is expressed explicitly is not just better, 
    // but the only way. If this is your case, use this class to manage startup.
    // For example, sometimes some state should be restored before any periodical handler will be started, 
    // or any incoming message will be processed and so on.
    // Do not forget to remove As<IStartable>() and AutoActivate() from DI registartions of services, 
    // which you want to startup explicitly.

    public class StartupManager : IStartupManager
    {
        private readonly CacheUpdaterHandler _cacheUpdater;
        private readonly ILog _log;

        public StartupManager(
            CacheUpdaterHandler cacheUpdater,
            ILog log)
        {
            _cacheUpdater = cacheUpdater;
            _log = log;
        }

        public async Task StartAsync()
        {
            _log.WriteInfo(nameof(StartAsync), null, "Filling trade volumes cache for all asset pairs...");

            await _cacheUpdater.FillCache();

            _log.WriteInfo(nameof(StartAsync), null, "Trade volumes cache is initialized");
        }
    }
}
