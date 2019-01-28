using System;
using Autofac;
using Lykke.Service.Assets.Client;
using Lykke.Service.ClientAccount.Client;
using Lykke.Service.FeeCalculator.Settings;
using Lykke.Service.TradeVolumes.Client;
using Lykke.SettingsReader;

namespace Lykke.Service.FeeCalculator.Modules
{
    public class ClientsModule : Module
    {
        private readonly AppSettings _settings;

        public ClientsModule(IReloadingManager<AppSettings> settings)
        {
            _settings = settings.CurrentValue;
        }

        protected override void Load(ContainerBuilder builder)
        {
            var feeSettings = _settings.FeeCalculatorService;

            builder.RegisterTradeVolumesClient(_settings.TradeVolumesServiceClient.ServiceUrl);

            builder.RegisterAssetsClient(AssetServiceSettings.Create(
                new Uri(_settings.AssetsServiceClient.ServiceUrl),
                feeSettings.Cache.AssetsUpdateInterval));

            builder.RegisterLykkeServiceClient(_settings.ClientAccountClient.ServiceUrl);
        }
    }
}
