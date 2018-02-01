using Autofac;
using Lykke.Service.FeeCalculator.Tests.Modules;

namespace Lykke.Service.FeeCalculator.Tests
{
    public class BaseTest
    {
        protected IContainer Container { get; set; }

        public BaseTest()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule(new TestsModule());
            
            Container = builder.Build();
        }
    }
}
