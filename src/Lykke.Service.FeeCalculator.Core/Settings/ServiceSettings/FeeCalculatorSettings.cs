using System.Collections.Generic;

namespace Lykke.Service.FeeCalculator.Core.Settings.ServiceSettings
{
    public class FeeCalculatorSettings
    {
        public DbSettings Db { get; set; }
        public MarketOrderSettings MarketOrder { get; set; }
        public LimitOrderSettings LimitOrder { get; set; }
        public List<CashoutFee> CashoutFees { get; set; }
    }
}
