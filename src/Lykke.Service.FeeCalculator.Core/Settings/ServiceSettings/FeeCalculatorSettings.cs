using System.Collections.Generic;
using Lykke.Service.FeeCalculator.Core.Domain.Fees;

namespace Lykke.Service.FeeCalculator.Core.Settings.ServiceSettings
{
    public class FeeCalculatorSettings
    {
        public int TradeVolumeToGetInDays { get; set; }
        public CacheSettings Cache { get; set; }
        public DbSettings Db { get; set; }
        public MarketOrderFee[] MarketOrderFees { get; set; }
        public List<CashoutFee> CashoutFees { get; set; }
        public BankCardSettings BankCard { get; set; }
    }
}
