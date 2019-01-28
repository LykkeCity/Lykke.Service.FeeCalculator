using System.Threading.Tasks;
using Common.Log;
using JetBrains.Annotations;
using Lykke.Common.Log;
using Lykke.Cqrs;
using Lykke.Sdk;
using Lykke.Service.FeeCalculator.Core.Services;
using Lykke.Service.FeeCalculator.Services.PeriodicalHandlers;

namespace Lykke.Service.FeeCalculator.Services
{
    public class StartupManager : IStartupManager
    {
        private readonly CacheUpdaterHandler _cacheUpdater;
        private readonly ICashoutFeesService _cashoutFeesService;
        private readonly IMarketOrderAssetFeeService _marketOrderAssetFeeService;
        private readonly IStaticFeeService _staticFeeService;
        private readonly ICqrsEngine _cqrsEngine;
        private readonly ILog _log;

        [UsedImplicitly]
        public StartupManager(
            CacheUpdaterHandler cacheUpdater,
            ICashoutFeesService cashoutFeesService,
            IMarketOrderAssetFeeService marketOrderAssetFeeService,
            IStaticFeeService staticFeeService,
            ICqrsEngine cqrsEngine,
            ILogFactory logFactory)
        {
            _cacheUpdater = cacheUpdater;
            _cashoutFeesService = cashoutFeesService;
            _marketOrderAssetFeeService = marketOrderAssetFeeService;
            _staticFeeService = staticFeeService;
            _cqrsEngine = cqrsEngine;
            _log = logFactory.CreateLog(this);
        }

        public async Task StartAsync()
        {
            _log.WriteInfo(nameof(StartAsync), null, "Filling trade volumes cache for all asset pairs...");

            await _cacheUpdater.FillCache();
            _cacheUpdater.Start();

            _log.WriteInfo(nameof(StartAsync), null, "Trade volumes cache is initialized");

            //TODO: remove in next release
            _log.WriteInfo(nameof(StartAsync), null, "Init fees (cashout and market order asset fees) from settings and static fees from db...");

            await _cashoutFeesService.InitAsync();
            await _marketOrderAssetFeeService.InitAsync();
            await _staticFeeService.InitAsync();

            _log.WriteInfo(nameof(StartAsync), null, "Init fees done");

            _cqrsEngine.StartSubscribers();
        }
    }
}
