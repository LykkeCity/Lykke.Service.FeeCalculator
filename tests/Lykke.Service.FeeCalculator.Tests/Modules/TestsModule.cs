using System.Linq;
using Autofac;
using AzureStorage.Tables;
using Common;
using Common.Log;
using Lykke.Service.ClientAccount.Client;
using Lykke.Service.FeeCalculator.AzureRepositories.Fees;
using Lykke.Service.FeeCalculator.Core.Domain.Fees;
using Lykke.Service.FeeCalculator.Core.Services;
using Lykke.Service.FeeCalculator.Services;
using Lykke.Service.TradeVolumes.Client;
using NSubstitute;


namespace Lykke.Service.FeeCalculator.Tests.Modules
{
    public class TestsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            const int tradeVolumeToGetInDays = 30;
            builder.RegisterInstance(Substitute.For<ILog>())
                .As<ILog>()
                .SingleInstance();

            builder.RegisterType<TradeVolumesCacheService>()
                .As<ITradeVolumesCacheService>()
                .SingleInstance();
            
            var mockTradeVolumesClient = Substitute.For<ITradeVolumesClient>();
            
            
            builder.RegisterInstance(mockTradeVolumesClient)
                .SingleInstance();
            
            builder.RegisterInstance(
                GetFeeRepository()  
            ).As<IFeeRepository>().SingleInstance();
            
            builder.RegisterInstance(
                GetStaticFeeRepository()  
            ).As<IStaticFeeRepository>().SingleInstance();
            
            builder.Register(x =>
            {
                var ctx = x.Resolve<IComponentContext>();
                return new CachedDataDictionary<decimal, IFee>(
                    async () => (await ctx.Resolve<IFeeRepository>().GetFeesAsync()).ToDictionary(itm => itm.Volume));
            }).SingleInstance();
            
            builder.Register(x =>
            {
                var ctx = x.Resolve<IComponentContext>();
                return new CachedDataDictionary<string, IStaticFee>(
                    async () => (await ctx.Resolve<IStaticFeeRepository>().GetFeesAsync()).ToDictionary(itm => itm.AssetPair));
            }).SingleInstance();
            
            builder.RegisterType<FeeService>()
                .As<IFeeService>()
                .WithParameter(TypedParameter.From(tradeVolumeToGetInDays))
                .SingleInstance();
            
            builder.RegisterInstance(Substitute.For<IClientAccountClient>())
                .As<IClientAccountClient>()
                .SingleInstance();
            
            builder.RegisterType<ClientIdCacheService>()
                .As<IClientIdCacheService>()
                .SingleInstance();
        }
        
        private IFeeRepository GetFeeRepository()
        {
            var repository = new FeeRepository(new NoSqlTableInMemory<FeeEntity>());
            
            repository.AddFeeAsync(new Fee{Volume = 0.1M, MakerFee = 0.1M, TakerFee = 0.19M}).GetAwaiter().GetResult();
            repository.AddFeeAsync(new Fee{Volume = 1M, MakerFee = 0.09M, TakerFee = 0.18M}).GetAwaiter().GetResult();
            repository.AddFeeAsync(new Fee{Volume = 2.5M, MakerFee = 0.08M, TakerFee = 0.17M}).GetAwaiter().GetResult();
            repository.AddFeeAsync(new Fee{Volume = 5M, MakerFee = 0.07M, TakerFee = 0.16M}).GetAwaiter().GetResult();
            repository.AddFeeAsync(new Fee{Volume = 10M, MakerFee = 0.06M, TakerFee = 0.15M}).GetAwaiter().GetResult();
            repository.AddFeeAsync(new Fee{Volume = 20M, MakerFee = 0.05M, TakerFee = 0.14M}).GetAwaiter().GetResult();

            return repository;
        }
        
        private IStaticFeeRepository GetStaticFeeRepository()
        {
            var repository = new StaticFeeRepository(new NoSqlTableInMemory<StaticFeeEntity>());
            
            repository.AddFeeAsync(new StaticFee{AssetPair = "BTCCHF", MakerFee = 0.03M, TakerFee = 0.03M}).GetAwaiter().GetResult();

            return repository;
        }
    }
}
