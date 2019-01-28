using System.Collections.Generic;
using Lykke.Service.FeeCalculator.Core.Domain.CashoutFee;
using Lykke.Service.FeeCalculator.Core.Domain.MarketOrderAssetFee;
using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.FeeCalculator.Settings.ServiceSettings
{
    public class FeeCalculatorSettings
    {
        public int TradeVolumeToGetInDays { get; set; }
        public CacheSettings Cache { get; set; }
        public DbSettings Db { get; set; }
        public IReadOnlyCollection<CashoutFee> CashoutFees { get; set; }
        [Optional]
        public IReadOnlyCollection<MarketOrderAssetFee> MarketOrderFees { get; set; }
        public BankCardSettings BankCard { get; set; }

        public CqrsSettings Cqrs { get; set; }
    }
}
