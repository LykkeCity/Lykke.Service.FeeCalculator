using Autofac;
using AzureStorage.Tables;
using Lykke.Common.Log;
using Lykke.Sdk;
using Lykke.Service.FeeCalculator.AzureRepositories.CashoutFee;
using Lykke.Service.FeeCalculator.AzureRepositories.Fee;
using Lykke.Service.FeeCalculator.AzureRepositories.FeeLog;
using Lykke.Service.FeeCalculator.AzureRepositories.MarketOrderAssetFee;
using Lykke.Service.FeeCalculator.AzureRepositories.StaticFee;
using Lykke.Service.FeeCalculator.AzureRepositories.WithdrawalFee;
using Lykke.Service.FeeCalculator.Core.Domain.CashoutFee;
using Lykke.Service.FeeCalculator.Core.Domain.FeeLog;
using Lykke.Service.FeeCalculator.Core.Domain.Fees;
using Lykke.Service.FeeCalculator.Core.Domain.MarketOrderAssetFee;
using Lykke.Service.FeeCalculator.Core.Domain.WithdrawalFee;
using Lykke.Service.FeeCalculator.Core.Services;
using Lykke.Service.FeeCalculator.Core.Settings;
using Lykke.Service.FeeCalculator.Services;
using Lykke.Service.FeeCalculator.Services.DummySettingsHolder;
using Lykke.Service.FeeCalculator.Services.PeriodicalHandlers;
using Lykke.SettingsReader;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;
using StackExchange.Redis;

namespace Lykke.Service.FeeCalculator.Modules
{
    public class ServiceModule : Module
    {
        private readonly IReloadingManager<AppSettings> _settings;

        public ServiceModule(IReloadingManager<AppSettings> settings)
        {
            _settings = settings;
        }

        protected override void Load(ContainerBuilder builder)
        {
            var feeSettings = _settings.CurrentValue.FeeCalculatorService;
            
            builder.RegisterType<StartupManager>()
                .As<IStartupManager>();
            
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

            builder.Register(ctx =>
            {
                var logFactory = ctx.Resolve<ILogFactory>();
                return new FeeRepository(AzureTableStorage<FeeEntity>.Create(
                    _settings.ConnectionString(x => x.FeeCalculatorService.Db.DataConnString), "VolumeFees",
                    logFactory));
            })
                .As<IFeeRepository>()
                .SingleInstance();

            builder.RegisterType<FeeService>()
                .As<IFeeService>()
                .WithParameter(TypedParameter.From(_settings.CurrentValue.FeeCalculatorService.Cache.InstanceName))
                .SingleInstance();

            builder.Register(ctx =>
                {
                    var logFactory = ctx.Resolve<ILogFactory>();
                    return new StaticFeeRepository(AzureTableStorage<StaticFeeEntity>.Create(
                        _settings.ConnectionString(x => x.FeeCalculatorService.Db.DataConnString), "StaticFees",
                        logFactory));
                })
                .As<IStaticFeeRepository>()
                .SingleInstance();

            builder.RegisterType<StaticFeeService>()
                .As<IStaticFeeService>()
                .WithParameter(TypedParameter.From(_settings.CurrentValue.FeeCalculatorService.Cache.InstanceName))
                .SingleInstance();

            builder.Register(ctx =>
                {
                    var logFactory = ctx.Resolve<ILogFactory>();
                    return new MarketOrderAssetFeeRepository(AzureTableStorage<MarketOrderAssetFeeEntity>.Create(
                        _settings.ConnectionString(x => x.FeeCalculatorService.Db.DataConnString), "MarketOrderAssetFees",
                        logFactory));
                })
                .As<IMarketOrderAssetFeesRepository>()
                .SingleInstance();

            builder.RegisterType<MarketOrderAssetFeeService>()
                .As<IMarketOrderAssetFeeService>()
                .WithParameter(TypedParameter.From(_settings.CurrentValue.FeeCalculatorService.Cache.InstanceName))
                .WithParameter(TypedParameter.From(_settings.CurrentValue.FeeCalculatorService.MarketOrderFees))
                .SingleInstance();

            builder.Register(ctx =>
                {
                    var logFactory = ctx.Resolve<ILogFactory>();
                    return new CashoutFeesRepository(AzureTableStorage<CashoutFeeEntity>.Create(
                        _settings.ConnectionString(x => x.FeeCalculatorService.Db.DataConnString), "CashoutFees",
                        logFactory));
                })
                .As<ICashoutFeesRepository>()
                .SingleInstance();

            builder.RegisterType<CashoutFeesService>()
                .As<ICashoutFeesService>()
                .WithParameter(TypedParameter.From(_settings.CurrentValue.FeeCalculatorService.Cache.InstanceName))
                .WithParameter(TypedParameter.From(_settings.CurrentValue.FeeCalculatorService.CashoutFees))
                .SingleInstance();

            builder.Register(ctx =>
                {
                    var logFactory = ctx.Resolve<ILogFactory>();
                    return new WithdrawalFeesRepository(AzureTableStorage<WithdrawalFeeEntity>.Create(
                        _settings.ConnectionString(x => x.FeeCalculatorService.Db.DataConnString), "WithdrawalFees",
                        logFactory));
                })
                .As<IWithdrawalFeesRepository>()
                .SingleInstance();

            builder.RegisterType<WithdrawalFeesService>()
                .As<IWithdrawalFeesService>()
                .WithParameter(TypedParameter.From(_settings.CurrentValue.FeeCalculatorService.Cache.InstanceName))
                .SingleInstance();

            // FeeLogsConnString
            builder.Register(ctx =>
                {
                    var logFactory = ctx.Resolve<ILogFactory>();
                    return new FeeLogRepository(AzureTableStorage<FeeLogEntryEntity>.Create(
                        _settings.ConnectionString(x => x.FeeCalculatorService.Db.DataConnString), "OperationsFeeLog",
                        logFactory));
                })
                .As<IFeeLogRepository>()
                .SingleInstance();

            builder.RegisterType<ClientIdCacheService>()
                .As<IClientIdCacheService>()
                .SingleInstance();
        }
    }
}
