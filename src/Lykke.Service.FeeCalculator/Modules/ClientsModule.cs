using System;
using Autofac;
using Common.Log;
using Lykke.Service.Assets.Client;
using Lykke.Service.ClientAccount.Client;
using Lykke.Service.FeeCalculator.Core.Settings;
using Lykke.Service.TradeVolumes.Client;
using Lykke.SettingsReader;

namespace Lykke.Service.FeeCalculator.Modules
{
    public class ClientsModule : Module
    {
        private readonly IReloadingManager<AppSettings> _settings;
        private readonly ILog _log;

        public ClientsModule(IReloadingManager<AppSettings> settings, ILog log)
        {
            _settings = settings;
            _log = log;
        }

        protected override void Load(ContainerBuilder builder)
        {
            var feeSettings = _settings.CurrentValue.FeeCalculatorService;
            
            builder.RegisterTradeVolumesClient(_settings.CurrentValue.TradeVolumesServiceClient.ServiceUrl, _log);

            builder.RegisterAssetsClient(AssetServiceSettings.Create(
                new Uri(_settings.CurrentValue.AssetsServiceClient.ServiceUrl),
                feeSettings.Cache.AssetsUpdateInterval));
            
            builder.RegisterLykkeServiceClient(_settings.CurrentValue.ClientAccountClient.ServiceUrl);
        }
    }
}
