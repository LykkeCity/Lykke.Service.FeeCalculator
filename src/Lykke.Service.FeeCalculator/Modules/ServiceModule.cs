using Autofac;
using Autofac.Extensions.DependencyInjection;
using Common.Log;
using Lykke.Service.FeeCalculator.Core.Services;
using Lykke.Service.FeeCalculator.Core.Settings.ServiceSettings;
using Lykke.Service.FeeCalculator.Services;
using Lykke.Service.FeeCalculator.Services.DummySettingsHolder;
using Lykke.SettingsReader;
using Microsoft.Extensions.DependencyInjection;

namespace Lykke.Service.FeeCalculator.Modules
{
    public class ServiceModule : Module
    {
        private readonly IReloadingManager<FeeCalculatorSettings> _settings;
        private readonly ILog _log;
        // NOTE: you can remove it if you don't need to use IServiceCollection extensions to register service specific dependencies
        private readonly IServiceCollection _services;

        public ServiceModule(IReloadingManager<FeeCalculatorSettings> settings, ILog log)
        {
            _settings = settings;
            _log = log;

            _services = new ServiceCollection();
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(_log)
                .As<ILog>()
                .SingleInstance();

            builder.RegisterType<HealthService>()
                .As<IHealthService>()
                .SingleInstance();

            builder.RegisterType<StartupManager>()
                .As<IStartupManager>();

            builder.RegisterType<ShutdownManager>()
                .As<IShutdownManager>();

            builder.RegisterInstance(new DummySettingsHolder(_settings.CurrentValue.MarketOrder, _settings.CurrentValue.LimitOrder, _settings.CurrentValue.CashoutFees))
                .As<IDummySettingsHolder>()
                .SingleInstance();

            builder.Populate(_services);
        }
    }
}
