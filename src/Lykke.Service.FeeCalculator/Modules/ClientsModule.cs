using System;
using Autofac;
using Lykke.Service.Assets.Client;
using Lykke.Service.ClientAccount.Client;
using Lykke.Service.FeeCalculator.Settings;
using Lykke.Service.TradeVolumes.Client;

namespace Lykke.Service.FeeCalculator.Modules
{
    public class ClientsModule : Module
    {
        private readonly AppSettings _settings;

        public ClientsModule(AppSettings settings)
        {
            _settings = settings;
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
