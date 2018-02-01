using System;
using System.Linq;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AzureStorage.Tables;
using Common;
using Common.Log;
using Lykke.Service.Assets.Client;
using Lykke.Service.Assets.Client.Models;
using Lykke.Service.FeeCalculator.AzureRepositories.Fees;
using Lykke.Service.FeeCalculator.Core.Domain.Fees;
using Lykke.Service.FeeCalculator.Core.Services;
using Lykke.Service.FeeCalculator.Core.Settings;
using Lykke.Service.FeeCalculator.Services;
using Lykke.Service.FeeCalculator.Services.DummySettingsHolder;
using Lykke.Service.FeeCalculator.Services.PeriodicalHandlers;
using Lykke.Service.TradeVolumes.Client;
using Lykke.SettingsReader;
using Microsoft.Extensions.DependencyInjection;

namespace Lykke.Service.FeeCalculator.Modules
{
    public class ServiceModule : Module
    {
        private readonly IReloadingManager<AppSettings> _settings;
        private readonly ILog _log;
        private readonly ServiceCollection _services;

        public ServiceModule(IReloadingManager<AppSettings> settings, ILog log)
        {
            _settings = settings;
            _log = log;
            _services = new ServiceCollection();
        }

        protected override void Load(ContainerBuilder builder)
        {
            var feeSettings = _settings.CurrentValue.FeeCalculatorService;
            
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

            builder.RegisterType<DummySettingsHolder>()
                .WithParameter(TypedParameter.From(feeSettings.MarketOrder))
                .WithParameter(TypedParameter.From(feeSettings.LimitOrder))
                .WithParameter(TypedParameter.From(feeSettings.CashoutFees))
                .WithParameter(TypedParameter.From(feeSettings.BankCard))
                .As<IDummySettingsHolder>()
                .SingleInstance();
            
            builder.RegisterType<CacheUpdaterHandler>()
                .As<IStartable>()
                .AsSelf()
                .AutoActivate()
                .WithParameter(TypedParameter.From(feeSettings.CacheUpdateInterval))
                .SingleInstance();
            
            builder.RegisterType<TradeVolumesCacheService>()
                .As<ITradeVolumesCacheService>()
                .SingleInstance();
            
            builder.RegisterType<FeeService>()
                .As<IFeeService>()
                .SingleInstance();
            
            builder.RegisterInstance<IFeeRepository>(
                new FeeRepository(AzureTableStorage<FeeEntity>.Create(
                    _settings.ConnectionString(x => x.FeeCalculatorService.Db.DataConnString), "VolumeFees", _log))
            ).SingleInstance();
            
            builder.Register(x =>
            {
                var ctx = x.Resolve<IComponentContext>();
                return new CachedDataDictionary<decimal, IFee>(
                    async () => (await ctx.Resolve<IFeeRepository>().GetFeesAsync()).ToDictionary(itm => itm.Volume), feeSettings.CacheUpdateInterval);
            }).SingleInstance();
            
            builder.RegisterTradeVolumesClient(_settings.CurrentValue.TradeVolumesServiceClient.ServiceUrl, _log);
            
            _services.RegisterAssetsClient(AssetServiceSettings.Create(
                new Uri(_settings.CurrentValue.AssetsServiceClient.ServiceUrl),
                _settings.CurrentValue.FeeCalculatorService.CacheUpdateInterval));

            builder.Populate(_services);
            
            builder.Register(x =>
            {
                var ctx = x.Resolve<IComponentContext>();
                return new CachedDataDictionary<string, AssetPair>(
                    async () => (await ctx.Resolve<IAssetsServiceWithCache>().GetAllAssetPairsAsync()).ToDictionary(itm => itm.Id), feeSettings.CacheUpdateInterval);
            }).SingleInstance();
        }
    }
}
