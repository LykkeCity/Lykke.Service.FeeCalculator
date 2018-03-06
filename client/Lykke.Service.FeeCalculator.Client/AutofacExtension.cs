using System;
using Autofac;
using Common.Log;
using Microsoft.Extensions.Caching.Memory;

namespace Lykke.Service.FeeCalculator.Client
{
    public static class AutofacExtension
    {
        public static void RegisterFeeCalculatorClient(this ContainerBuilder builder, string serviceUrl, ILog log)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (serviceUrl == null) throw new ArgumentNullException(nameof(serviceUrl));
            if (log == null) throw new ArgumentNullException(nameof(log));
            if (string.IsNullOrWhiteSpace(serviceUrl))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(serviceUrl));

            builder.Register(c => new FeeCalculatorClient(serviceUrl, log))
                .As<IFeeCalculatorClient>()
                .SingleInstance();
        }

        public static void RegisterFeeCalculatorClientWithCache(this ContainerBuilder builder, string serviceUrl, TimeSpan exirationPeriod, ILog log)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (serviceUrl == null) throw new ArgumentNullException(nameof(serviceUrl));
            if (log == null) throw new ArgumentNullException(nameof(log));
            if (string.IsNullOrWhiteSpace(serviceUrl))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(serviceUrl));
            
            var cache = new MemoryCache(new MemoryCacheOptions { ExpirationScanFrequency = TimeSpan.FromSeconds(30) });
            builder.Register(c => new FeeCalculatorClientCached(new FeeCalculatorClient(serviceUrl, log), exirationPeriod, cache))
                .As<IFeeCalculatorClient>()
                .SingleInstance();
        }
    }
}
