using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AzureStorage.Tables;
using Common.Log;
using Lykke.Service.Assets.Client;
using Lykke.Service.ClientAccount.Client;
using Lykke.Service.FeeCalculator.AzureRepositories.CashoutFee;
using Lykke.Service.FeeCalculator.AzureRepositories.Fee;
using Lykke.Service.FeeCalculator.AzureRepositories.MarketOrderAssetFee;
using Lykke.Service.FeeCalculator.AzureRepositories.StaticFee;
using Lykke.Service.FeeCalculator.AzureRepositories.WithdrawalFee;
using Lykke.Service.FeeCalculator.Core.Domain.CashoutFee;
using Lykke.Service.FeeCalculator.Core.Domain.Fees;
using Lykke.Service.FeeCalculator.Core.Domain.MarketOrderAssetFee;
using Lykke.Service.FeeCalculator.Core.Domain.WithdrawalFee;
using Lykke.Service.FeeCalculator.Core.Services;
using Lykke.Service.FeeCalculator.Core.Settings;
using Lykke.Service.FeeCalculator.Services;
using Lykke.Service.FeeCalculator.Services.DummySettingsHolder;
using Lykke.Service.FeeCalculator.Services.PeriodicalHandlers;
using Lykke.Service.TradeVolumes.Client;
using Lykke.SettingsReader;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

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

            //TODO: remove
            builder.RegisterInstance(new DummySettingsHolder(feeSettings.BankCard))
                .As<IDummySettingsHolder>()
                .SingleInstance();
            
            builder.Register(c => new RedisCache(new RedisCacheOptions
                {
                    Configuration = feeSettings.Cache.RedisConfiguration,
                    InstanceName = $"{feeSettings.Cache.InstanceName}:"
                }))
                .As<IDistributedCache>()
                .SingleInstance();
            
            builder.Register(c => 
                ConnectionMultiplexer.Connect(feeSettings.Cache.RedisConfiguration)
            ).As<IConnectionMultiplexer>().SingleInstance();
            
            builder.RegisterType<CacheUpdaterHandler>()
                .AsSelf()
                .WithParameter(TypedParameter.From(feeSettings.Cache.TradeVolumesUpdateInterval))
                .WithParameter(TypedParameter.From(feeSettings.TradeVolumeToGetInDays))
                .SingleInstance();
            
            builder.RegisterType<TradeVolumesCacheService>()
                .As<ITradeVolumesCacheService>()
                .SingleInstance();
            
            builder.RegisterType<FeeCalculatorService>()
                .As<IFeeCalculatorService>()
                .WithParameter(TypedParameter.From(feeSettings.TradeVolumeToGetInDays))
                .WithParameter(TypedParameter.From(feeSettings.MarketOrderFees))
                .SingleInstance();
            
            builder.RegisterInstance<IFeeRepository>(
                new FeeRepository(AzureTableStorage<FeeEntity>.Create(
                    _settings.ConnectionString(x => x.FeeCalculatorService.Db.DataConnString), "VolumeFees", _log))
            ).SingleInstance();
            
            builder.RegisterType<FeeService>()
                .As<IFeeService>()
                .WithParameter(TypedParameter.From(_settings.CurrentValue.FeeCalculatorService.Cache.InstanceName))
                .SingleInstance();
            
            builder.RegisterInstance<IStaticFeeRepository>(
                new StaticFeeRepository(AzureTableStorage<StaticFeeEntity>.Create(
                    _settings.ConnectionString(x => x.FeeCalculatorService.Db.DataConnString), "StaticFees", _log))
            ).SingleInstance();
            
            builder.RegisterType<StaticFeeService>()
                .As<IStaticFeeService>()
                .WithParameter(TypedParameter.From(_settings.CurrentValue.FeeCalculatorService.Cache.InstanceName))
                .SingleInstance();
            
            builder.RegisterInstance<IMarketOrderAssetFeesRepository>(
                new MarketOrderAssetFeeRepository(AzureTableStorage<MarketOrderAssetFeeEntity>.Create(
                    _settings.ConnectionString(x => x.FeeCalculatorService.Db.DataConnString), "MarketOrderAssetFees", _log))
            ).SingleInstance();
            
            builder.RegisterType<MarketOrderAssetFeeService>()
                .As<IMarketOrderAssetFeeService>()
                .WithParameter(TypedParameter.From(_settings.CurrentValue.FeeCalculatorService.Cache.InstanceName))
                .WithParameter(TypedParameter.From(_settings.CurrentValue.FeeCalculatorService.MarketOrderFees))
                .SingleInstance();
            
            builder.RegisterInstance<ICashoutFeesRepository>(
                new CashoutFeesRepository(AzureTableStorage<CashoutFeeEntity>.Create(
                    _settings.ConnectionString(x => x.FeeCalculatorService.Db.DataConnString), "CashoutFees", _log))
            ).SingleInstance();
            
            builder.RegisterType<CashoutFeesService>()
                .As<ICashoutFeesService>()
                .WithParameter(TypedParameter.From(_settings.CurrentValue.FeeCalculatorService.Cache.InstanceName))
                .WithParameter(TypedParameter.From(_settings.CurrentValue.FeeCalculatorService.CashoutFees))
                .SingleInstance();

            builder.RegisterInstance<IWithdrawalFeesRepository>(
                new WithdrawalFeesRepository(AzureTableStorage<WithdrawalFeeEntity>.Create(
                    _settings.ConnectionString(x => x.FeeCalculatorService.Db.DataConnString), "WithdrawalFees", _log))
            ).SingleInstance();

            builder.RegisterType<WithdrawalFeesService>()
                .As<IWithdrawalFeesService>()
                .WithParameter(TypedParameter.From(_settings.CurrentValue.FeeCalculatorService.Cache.InstanceName))
                .SingleInstance();


            builder.RegisterTradeVolumesClient(_settings.CurrentValue.TradeVolumesServiceClient.ServiceUrl, _log);
            
            _services.RegisterAssetsClient(AssetServiceSettings.Create(
                new Uri(_settings.CurrentValue.AssetsServiceClient.ServiceUrl),
                feeSettings.Cache.AssetsUpdateInterval), _log);

            builder.Populate(_services);
            
            builder.RegisterLykkeServiceClient(_settings.CurrentValue.ClientAccountClient.ServiceUrl);
            
            builder.RegisterType<ClientIdCacheService>()
                .As<IClientIdCacheService>()
                .SingleInstance();
        }
    }
}
