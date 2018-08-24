using System;
using Autofac;
using Common.Log;
using Lykke.Logs;
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

        public ClientsModule(IReloadingManager<AppSettings> settings)
        {
            _settings = settings;
        }

        protected override void Load(ContainerBuilder builder)
        {
            var feeSettings = _settings.CurrentValue.FeeCalculatorService;

            // todo: use updated registration method with ILogFactory parameter instead of ILog
            //builder.RegisterTradeVolumesClient(_settings.CurrentValue.TradeVolumesServiceClient.ServiceUrl, log);
            var log = EmptyLogFactory.Instance.CreateLog(this);
            builder.RegisterInstance(new TradeVolumesClient(_settings.CurrentValue.TradeVolumesServiceClient.ServiceUrl, log))
                .As<ITradeVolumesClient>().SingleInstance();

            builder.RegisterAssetsClient(AssetServiceSettings.Create(
                new Uri(_settings.CurrentValue.AssetsServiceClient.ServiceUrl),
                feeSettings.Cache.AssetsUpdateInterval));

            builder.RegisterLykkeServiceClient(_settings.CurrentValue.ClientAccountClient.ServiceUrl);
        }
    }
}
