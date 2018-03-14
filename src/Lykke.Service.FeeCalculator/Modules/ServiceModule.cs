using System;
using System.Linq;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AzureStorage.Tables;
using Common;
using Common.Log;
using Lykke.Service.Assets.Client;
using Lykke.Service.Assets.Client.Models;
using Lykke.Service.ClientAccount.Client;
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

            builder.RegisterInstance(feeSettings).SingleInstance();

            builder.RegisterInstance(new DummySettingsHolder(feeSettings.CashoutFees, feeSettings.BankCard))
                .As<IDummySettingsHolder>()
                .SingleInstance();
            
            builder.RegisterType<CacheUpdaterHandler>()
                .AsSelf()
                .WithParameter(TypedParameter.From(feeSettings.Cache.TradeVolumesUpdateInterval))
                .WithParameter(TypedParameter.From(feeSettings.TradeVolumeToGetInDays))
                .SingleInstance();
            
            builder.RegisterType<TradeVolumesCacheService>()
                .As<ITradeVolumesCacheService>()
                .SingleInstance();
            
            builder.RegisterType<FeeService>()
                .As<IFeeService>()
                .WithParameter(TypedParameter.From(feeSettings.TradeVolumeToGetInDays))
                .WithParameter(TypedParameter.From(feeSettings.MarketOrderFees))
                .SingleInstance();
            
            builder.RegisterInstance<IFeeRepository>(
                new FeeRepository(AzureTableStorage<FeeEntity>.Create(
                    _settings.ConnectionString(x => x.FeeCalculatorService.Db.DataConnString), "VolumeFees", _log))
            ).SingleInstance();
            
            builder.Register(x =>
            {
                var ctx = x.Resolve<IComponentContext>();
                return new CachedDataDictionary<decimal, IFee>(
                    async () => (await ctx.Resolve<IFeeRepository>().GetFeesAsync()).ToDictionary(itm => itm.Volume), feeSettings.Cache.FeesUpdateInterval);
            }).SingleInstance();
            
            
            builder.RegisterInstance<IStaticFeeRepository>(
                new StaticFeeRepository(AzureTableStorage<StaticFeeEntity>.Create(
                    _settings.ConnectionString(x => x.FeeCalculatorService.Db.DataConnString), "StaticFees", _log))
            ).SingleInstance();
            
            builder.Register(x =>
            {
                var ctx = x.Resolve<IComponentContext>();
                return new CachedDataDictionary<string, IStaticFee>(
                    async () => (await ctx.Resolve<IStaticFeeRepository>().GetFeesAsync()).ToDictionary(itm => itm.AssetPair), feeSettings.Cache.FeesUpdateInterval);
            }).SingleInstance();
            
            builder.RegisterTradeVolumesClient(_settings.CurrentValue.TradeVolumesServiceClient.ServiceUrl, _log);
            
            _services.RegisterAssetsClient(AssetServiceSettings.Create(
                new Uri(_settings.CurrentValue.AssetsServiceClient.ServiceUrl),
                feeSettings.Cache.AssetsUpdateInterval));

            builder.Populate(_services);
            
            builder.Register(x =>
            {
                var ctx = x.Resolve<IComponentContext>();
                return new CachedDataDictionary<string, AssetPair>(
                    async () => (await ctx.Resolve<IAssetsServiceWithCache>().GetAllAssetPairsAsync()).ToDictionary(itm => itm.Id), feeSettings.Cache.AssetsUpdateInterval);
            }).SingleInstance();
            
            builder.RegisterLykkeServiceClient(_settings.CurrentValue.ClientAccountClient.ServiceUrl);
            
            builder.RegisterType<ClientIdCacheService>()
                .As<IClientIdCacheService>()
                .SingleInstance();
        }
    }
}
